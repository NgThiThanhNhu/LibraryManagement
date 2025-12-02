using System.Net;
using System.Security.Cryptography;
using System.Text;
using DoAnCuoiKy.Data;
using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.InformationLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;

namespace DoAnCuoiKy.Service.InformationLibrary
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentService(IConfiguration configuration, ApplicationDbContext context, IHttpContextAccessor httpContext)
        {
            _configuration = configuration;
            _context = context;
            _httpContextAccessor = httpContext;
        }
        public async Task<BaseResponse<PaymentResponse>> CreatePayment(PaymentRequest paymentRequest)
        {
            BaseResponse<PaymentResponse> response = new BaseResponse<PaymentResponse>();
            var vnp_TmnCode = _configuration["VNPay:vnp_TmnCode"];
            var vnp_HashSecret = _configuration["VNPay:vnp_HashSecret"];
            var vnp_Url = _configuration["VNPay:vnp_Url"];
            var vnp_ReturnUrl = _configuration["VNPay:vnp_ReturnUrl"];

            var orderId = paymentRequest.BorrowingId.ToString();
            var amount = (int)(paymentRequest.BorrowAmount * 100);

            DateTime createDate = DateTime.Now;
            DateTime expireDate = createDate.AddMinutes(10);

            var vnpParams = new SortedDictionary<string, string>
            {
                {"vnp_Version", "2.1.0" },
                {"vnp_Command", "pay" },
                {"vnp_TmnCode", vnp_TmnCode},
                {"vnp_Amount", amount.ToString() },
                {"vnp_CurrCode", "VND" },
                {"vnp_TxnRef", orderId },
                {"vnp_OrderInfo", paymentRequest.VnpText},
                {"vnp_OrderType", "other"},
                {"vnp_Locale", "vn"},
                {"vnp_CreateDate", createDate.ToString("yyyyMMddHHmmss")},
                {"vnp_ExpireDate", expireDate.ToString("yyyyMMddHHmmss")},
                {"vnp_BankCode", "VNBANK"},
                {"vnp_IpAddr", _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1"},
                {"vnp_ReturnUrl", vnp_ReturnUrl}
            };

            string hashData = string.Join("&", vnpParams.Select(kvp =>
                $"{kvp.Key}={WebUtility.UrlEncode(kvp.Value)}"
            ));

            string secureHash = HmacSHA512(vnp_HashSecret, hashData);

            string query = string.Join("&", vnpParams.Select(kvp =>
                $"{kvp.Key}={WebUtility.UrlEncode(kvp.Value)}"
            ));

            string paymentUrl = $"{vnp_Url}?{query}&vnp_SecureHash={secureHash}";


            Payment payment = new Payment();
            payment.Id = Guid.NewGuid();
            payment.BorrowAmount = paymentRequest.BorrowAmount;
            payment.PaymentType = paymentRequest.PaymentType;
            payment.CreateDate = DateTime.Now;
            payment.VnpText = paymentRequest.VnpText;
            payment.BorrowingId = paymentRequest.BorrowingId;
            payment.OrderId = orderId;
            _context.payments.Add(payment);
            await _context.SaveChangesAsync();

            PaymentResponse paymentResponse = new PaymentResponse();
            paymentResponse.BorrowAmount = payment.BorrowAmount.Value;
            paymentResponse.PaymentType = payment.PaymentType.Value;
            paymentResponse.VnpText = payment.VnpText;
            paymentResponse.CreateDate = payment.CreateDate;
            paymentResponse.PaymentUrl = paymentUrl;
            response.IsSuccess = true;
            response.message = "Tạo mã thanh toán thành công";
            response.data = paymentResponse;
            return response;
        }
        public static String HmacSHA512(string key, String inputData)
        {
            var hash = new StringBuilder();
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }

            return hash.ToString();
        }

        public async Task<BaseResponse<PaymentResponse>> VnPayReturn(IQueryCollection query)
        {
            BaseResponse<PaymentResponse> response = new BaseResponse<PaymentResponse>();
            PaymentResponse paymentResponse = new PaymentResponse();

            var dictionary = query.ToDictionary(k => k.Key, v => v.Value.ToString());
            var returnquery = query;
            var responseCode = dictionary.GetValueOrDefault("vnp_ResponseCode");
            var transactionNo = dictionary.GetValueOrDefault("vnp_TransactionNo");
            var txnRef = dictionary.GetValueOrDefault("vnp_TxnRef");
            var vnpBankCode = dictionary.GetValueOrDefault("vnp_BankCode");
            var vnpSecureHash = dictionary.GetValueOrDefault("vnp_SecureHash");


            var payment = await _context.payments.FirstOrDefaultAsync(x => x.OrderId == txnRef);

            if (payment == null)
            {
                response.IsSuccess = false;
                response.message = "Payment không tồn tại";
                return response;
            }
            payment.VnpResponseCode = responseCode;
            payment.TransactionNo = transactionNo;
            payment.vnpBankCode = vnpBankCode;
            _context.payments.Update(payment);

            var borrowing = await _context.borrowings
                .FirstOrDefaultAsync(x => x.Id == payment.BorrowingId && x.IsDeleted == false);

            if (borrowing != null)
            {
                borrowing.BorrowingStatus = Model.Enum.InformationLibrary.BorrowingStatus.Returned;
                borrowing.UpdateDate = DateTime.Now;
                borrowing.UpdateUser = getCurrentName();
                _context.borrowings.UpdateRange(borrowing);

            }

            await _context.fines
                .Where(x => x.IsDeleted == false
                    && x.borrowingDetail.BorrowingId == payment.BorrowingId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(f => f.IsPaid, true)
                    .SetProperty(f => f.UpdateDate, DateTime.Now)
                    .SetProperty(f => f.UpdateUser, getCurrentName())
                );

            var fines = await _context.fines
                .Include(f => f.borrowingDetail)
                    .ThenInclude(bd => bd.bookItem)
                .Where(x => x.IsDeleted == false
                    && x.borrowingDetail.BorrowingId == payment.BorrowingId)
                .ToListAsync();

            if (fines.Any())
            {
                foreach (var fine in fines)
                {
                    if (fine.borrowingDetail.bookItem != null)
                    {
                        fine.borrowingDetail.bookItem.BookStatus = Model.Enum.InformationLibrary.BookStatus.Available;
                        fine.borrowingDetail.bookItem.UpdateDate = DateTime.Now;
                        fine.borrowingDetail.bookItem.UpdateUser = getCurrentName();
                        _context.bookItems.UpdateRange(fine.borrowingDetail.bookItem);
                        await _context.SaveChangesAsync();
                    }
                }

                var bookIds = fines
                    .Where(f => f.borrowingDetail?.bookItem?.BookId != null)
                    .Select(f => f.borrowingDetail.bookItem.BookId)
                    .Distinct()
                    .ToList();

                foreach (var bookId in bookIds)
                {
                    var availableCount = await _context.bookItems
                        .CountAsync(x => x.BookId == bookId
                            && x.BookStatus == Model.Enum.InformationLibrary.BookStatus.Available
                            && x.IsDeleted == false);

                    var book = await _context.books.FirstOrDefaultAsync(x => x.Id == bookId);

                    if (book != null)
                    {
                        book.Quantity = availableCount;
                        book.UpdateDate = DateTime.Now;
                        _context.books.UpdateRange(book);
                    }
                }
            }
            await _context.SaveChangesAsync();
            paymentResponse.BorrowAmount = payment.BorrowAmount.Value;
            paymentResponse.vnpBankCode = payment.vnpBankCode;
            paymentResponse.VnpText = payment.VnpText;
            paymentResponse.CreateDate = payment.CreateDate;
            paymentResponse.VnpResponseCode = payment.VnpResponseCode;
            paymentResponse.TransactionNo = transactionNo;
            paymentResponse.PaymentType = payment.PaymentType.Value; 

            response.IsSuccess = true;
            response.message = responseCode == "00"
                ? "Thanh toán thành công"
                : "Thanh toán không thành công";
            response.data = paymentResponse;

            return response;
        }

        private bool ValidateSignature(Dictionary<string, string> queryParams, string vnpSecureHash)
        {
            var vnpHashSecret = _configuration["VnPay:HashSecret"];

            var sortedParams = queryParams
                .Where(x => x.Key != "vnp_SecureHash" && x.Key != "vnp_SecureHashType")
                .OrderBy(x => x.Key)
                .ToList();

            var hashData = string.Join("&", sortedParams.Select(x => $"{x.Key}={x.Value}"));

            var computedHash = HmacSHA512(vnpHashSecret, hashData);

            return computedHash.Equals(vnpSecureHash, StringComparison.InvariantCultureIgnoreCase);
        }

        
        private string getCurrentName()
        {
            return _httpContextAccessor.HttpContext.User.Identity.Name;
        }

        public async Task<BaseResponse<PaymentResponse>> CashPayment(PaymentRequest paymentRequest)
        {
            BaseResponse<PaymentResponse> response = new BaseResponse<PaymentResponse>();
            Payment payment = new Payment();
            payment.Id = Guid.NewGuid();
            payment.PaymentType = paymentRequest.PaymentType;
            payment.BorrowingId = paymentRequest.BorrowingId;
            payment.BorrowAmount = paymentRequest.BorrowAmount;
            payment.VnpText = paymentRequest.VnpText;
            payment.CreateUser = getCurrentName();
            payment.CreateDate = DateTime.Now;
            _context.payments.Add(payment);
            var borrowing = await _context.borrowings
                .FirstOrDefaultAsync(x => x.Id == payment.BorrowingId && x.IsDeleted == false);

            if (borrowing != null)
            {
                borrowing.BorrowingStatus = Model.Enum.InformationLibrary.BorrowingStatus.Returned;
                borrowing.UpdateDate = DateTime.Now;
                borrowing.UpdateUser = getCurrentName();
                _context.borrowings.UpdateRange(borrowing);
                
            }

            await _context.fines
                .Where(x => x.IsDeleted == false
                    && x.borrowingDetail.BorrowingId == payment.BorrowingId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(f => f.IsPaid, true)
                    .SetProperty(f => f.UpdateDate, DateTime.Now)
                    .SetProperty(f => f.UpdateUser, getCurrentName())
                );

            var fines = await _context.fines
                .Include(f => f.borrowingDetail)
                    .ThenInclude(bd => bd.bookItem)
                .Where(x => x.IsDeleted == false
                    && x.borrowingDetail.BorrowingId == payment.BorrowingId)
                .ToListAsync();

            if (fines.Any())
            {
                foreach (var fine in fines)
                {
                    if (fine.borrowingDetail.bookItem != null)
                    {
                        fine.borrowingDetail.bookItem.BookStatus = Model.Enum.InformationLibrary.BookStatus.Available;
                        fine.borrowingDetail.bookItem.UpdateDate = DateTime.Now;
                        fine.borrowingDetail.bookItem.UpdateUser = getCurrentName();
                        _context.bookItems.UpdateRange(fine.borrowingDetail.bookItem);
                        await _context.SaveChangesAsync();
                    }
                }

                var bookIds = fines
                    .Where(f => f.borrowingDetail?.bookItem?.BookId != null)
                    .Select(f => f.borrowingDetail.bookItem.BookId)
                    .Distinct()
                    .ToList();

                foreach (var bookId in bookIds)
                {
                    var availableCount = await _context.bookItems
                        .CountAsync(x => x.BookId == bookId
                            && x.BookStatus == Model.Enum.InformationLibrary.BookStatus.Available
                            && x.IsDeleted == false);

                    var book = await _context.books.FirstOrDefaultAsync(x => x.Id == bookId);

                    if (book != null)
                    {
                        book.Quantity = availableCount;
                        book.UpdateDate = DateTime.Now;
                        _context.books.UpdateRange(book);
                    }
                }
            }
            await _context.SaveChangesAsync();
            PaymentResponse paymentResponse = new PaymentResponse();
            paymentResponse.BorrowAmount = payment.BorrowAmount.Value;
            paymentResponse.PaymentType = payment.PaymentType.Value;
            paymentResponse.CreateDate = payment.CreateDate;
            response.IsSuccess = true;
            response.message = "Thanh toán thành công";
            response.data = paymentResponse;
            return response;
        }
    }
}