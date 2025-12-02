using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using Azure;
using DoAnCuoiKy.Data;
using DoAnCuoiKy.Model.Entities.InformationLibrary;
using DoAnCuoiKy.Model.Entities.Usermanage;
using DoAnCuoiKy.Model.Enum.InformationLibrary;
using DoAnCuoiKy.Model.Request;
using DoAnCuoiKy.Model.Response;
using DoAnCuoiKy.Service.IService.InformationLibrary;
using Microsoft.EntityFrameworkCore;

namespace DoAnCuoiKy.Service.InformationLibrary
{
    public class BookCartService : IBookCartService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private const int Max_CartItems_Per_Cart = 5;
        public BookCartService(ApplicationDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }
        private async Task<BookCart> GetOrCreateActiveCart(Guid userId)
        {
            var cart = await _context.bookCarts.Include(x => x.BookCartItems).ThenInclude(x => x.Book).ThenInclude(x => x.BookAuthor).Include(x => x.BookCartItems).ThenInclude(x => x.Book).ThenInclude(x => x.Publisher).Include(x => x.BookCartItems).ThenInclude(x => x.Book).ThenInclude(x => x.bookFiles).FirstOrDefaultAsync(x=>x.UserId == userId && x.CartStatus == CartStatus.Active && x.IsDeleted == false);
            if (cart == null)
            {
                cart = new BookCart
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    CreateDate = DateTime.Now,
                    ExpiredDate = DateTime.Now.AddDays(8),
                    CartStatus = CartStatus.Active,
                    CreateUser = getCurrentName()
                };

                await _context.bookCarts.AddAsync(cart);
                await _context.SaveChangesAsync();
            }
            return cart;
        }
        private int TotalQuantity(Guid bookCartId)
        {
            int CurrentBookQuantity = _context.bookCartItems.Where(x=>x.CartId == bookCartId).Sum(x=>x.Quantity);
            return CurrentBookQuantity;
        }
        private int RemainingSlots(Guid cartId)
        {
            int remainingSlots = 5 - TotalQuantity(cartId);
            return remainingSlots;
        }
        private bool CanAddMore(Guid bookCartId, CartStatus cartStatus)
        {
            return TotalQuantity(bookCartId) < 5 && cartStatus == CartStatus.Active;
        }
        private bool CanAddQuantity(int quantity, Guid bookCartId)
        {
            return TotalQuantity(bookCartId) + quantity <= 5;
        }
        private int GetTotalBooks(BookCart bookCart)
        {
            return bookCart.BookCartItems?.Count ?? 0;
        }
        private async Task<BookCartResponse> MapToBookCartResponse(BookCart bookCart)
        {
            List<BookCartItemResponse> items = new List<BookCartItemResponse>();
            var currentUserId = getCurrentUserId();
            int totalInCart = TotalQuantity(bookCart.Id);
            var cart = await _context.bookCarts.Include(x => x.BookCartItems).ThenInclude(x => x.Book).ThenInclude(x => x.BookAuthor).Include(x => x.BookCartItems).ThenInclude(x => x.Book).ThenInclude(x => x.Publisher).Include(x => x.BookCartItems).ThenInclude(x => x.Book).ThenInclude(x => x.bookFiles).FirstOrDefaultAsync(x => x.UserId == currentUserId && x.CartStatus == CartStatus.Active && x.IsDeleted == false);
            if (bookCart == null) return null;
            
            foreach (var item in cart.BookCartItems)
            {
                int countAvailableBookItem = await _context.bookItems.CountAsync(x => x.IsDeleted == false && x.BookId == item.BookId && x.BookStatus == BookStatus.Available);
                BookCartItemResponse bookCartItemResponse = new BookCartItemResponse();
                bookCartItemResponse.BookCartItemId = item.Id;
                bookCartItemResponse.BookId = item.BookId;
                bookCartItemResponse.BookTitle = item.Book.Title;
                bookCartItemResponse.Author = item.Book.BookAuthor.Name;
                bookCartItemResponse.Publisher = item.Book.Publisher.PublisherName;
                bookCartItemResponse.ImageUrl = item.Book.bookFiles.FirstOrDefault().ImageUrl;
                bookCartItemResponse.RequestedQuantity = item.Quantity;
                bookCartItemResponse.AvailableQuantity = countAvailableBookItem;
                bookCartItemResponse.CanIncrease = countAvailableBookItem > item.Quantity && item.Quantity < 5 && totalInCart < 5;
                bookCartItemResponse.CanDecrease = item.Quantity > 1;
                bookCartItemResponse.StatusText = $"Còn {countAvailableBookItem} quyển sẵn";
                bookCartItemResponse.CreateAt = item.CreateDate;
                items.Add(bookCartItemResponse);
            }
            BookCartResponse bookCartResponse = new BookCartResponse();
            bookCartResponse.BookCartId = bookCart.Id;
            bookCartResponse.CreateAt = bookCart.CreateDate;
            bookCartResponse.UpdateAt = DateTime.Now;
            bookCartResponse.ExpiredDate = bookCart.ExpiredDate;
            bookCartResponse.CartStatus = bookCart.CartStatus;
            bookCartResponse.TotalQuantity = totalInCart;
            bookCartResponse.RemainingSlots = RemainingSlots(bookCart.Id);
            bookCartResponse.CanAddMore = CanAddMore(bookCart.Id, bookCart.CartStatus);
            bookCartResponse.bookCartItemResponses = items;
            return bookCartResponse;
        }
        public async Task<BaseResponse<BookCartResponse>> CreateBookCart(BookCartRequest bookCartRequest)
        {
                BaseResponse<BookCartResponse> response = new BaseResponse<BookCartResponse>();
            try
            {
                var currentUser = getCurrentUserId();
                if(bookCartRequest.Quantity < 1 || bookCartRequest.Quantity > 5)
                {
                    response.IsSuccess = false;
                    response.message = "Số lượng không được < 1 hoặc > 5";
                    return response;
                }
                BookCart bookCart = await GetOrCreateActiveCart(currentUser);
                bool hasBook = _context.bookCartItems.Where(x=>x.BookId == bookCartRequest.BookId && x.CartId == bookCart.Id).Any();
                if (hasBook)
                {
                    response.IsSuccess = false;
                    response.message = "Sách đã có trong giỏ, dùng -/+ để thay đổi số lượng";
                    return response;
                }
                if (TotalQuantity(bookCart.Id) + bookCartRequest.Quantity > 5)
                {
                    response.IsSuccess=false;
                    response.message = $"Giỏ đã có {TotalQuantity(bookCart.Id)} quyển. " +
                                 $"Chỉ có thể thêm tối đa {RemainingSlots(bookCart.Id)} quyển nữa.";
                    return response;
                }
                Book book = await _context.books.FirstOrDefaultAsync(x=>x.Id == bookCartRequest.BookId && x.IsDeleted == false);
                if (book == null)
                {
                    response.IsSuccess=false;
                    response.message = "Không tìm thấy sách";
                    return response;
                }
                int countAvailableBookItem = await _context.bookItems.CountAsync(x => x.IsDeleted == false && x.BookId == bookCartRequest.BookId && x.BookStatus == BookStatus.Available);
                if (countAvailableBookItem < bookCartRequest.Quantity)
                {
                    response.IsSuccess = false;
                    response.message = $"Sách '{book.Title}' chỉ còn {countAvailableBookItem} quyển sẵn, " +
                                 $"không đủ {bookCartRequest.Quantity} quyển";
                    return response;
                }
                BookCartItem bookCartItem = new BookCartItem();
                bookCartItem.Id = Guid.NewGuid();
                bookCartItem.CartId = bookCart.Id;
                bookCartItem.BookId = bookCartRequest.BookId;
                bookCartItem.Quantity = bookCartRequest.Quantity;
                bookCartItem.CreateDate = DateTime.Now;
                bookCartItem.CreateUser = getCurrentName();
                await _context.bookCartItems.AddAsync(bookCartItem);
                bookCart.UpdateDate = DateTime.Now;
                _context.bookCarts.Update(bookCart);
                await _context.SaveChangesAsync();
                response.data = await MapToBookCartResponse(bookCart);
                response.IsSuccess = true;
                response.message = $"Đã thêm {bookCartRequest.Quantity} quyển '{book.Title}' vào giỏ";
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.message = ex.Message;
                return response;
            }
        }

        private string getCurrentName()
        {
            return _contextAccessor.HttpContext.User.Identity.Name;
        }
        private Guid getCurrentUserId()
        {
            var user = _contextAccessor.HttpContext.User;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("User is not Authentiated");
            }
            var userId = user.FindFirst(JwtRegisteredClaimNames.Sub) ?? user.FindFirst(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                throw new Exception("userId không tồn tại");
            }
            return Guid.Parse(userId.Value);
        }

        public async Task<BaseResponse<BookCartResponse>> GetBookCartActive()
        {
            BaseResponse<BookCartResponse> response = new BaseResponse<BookCartResponse>();
            var currentUserId = getCurrentUserId(); 
            try
            {
                BookCart bookCart = await GetOrCreateActiveCart(currentUserId);
                BookCartResponse bookCartResponse = new BookCartResponse();
                if (!bookCart.BookCartItems.Any())
                {
                    bookCartResponse.BookCartId = bookCart.Id;
                    bookCartResponse.UserId = currentUserId;
                    bookCartResponse.CreateAt = bookCart.CreateDate;
                    bookCartResponse.ExpiredDate = bookCart.ExpiredDate;
                    bookCartResponse.CartStatus = bookCart.CartStatus;
                    bookCartResponse.bookCartItemResponses = new List<BookCartItemResponse>();
                    bookCartResponse.TotalBooks = 0;
                    bookCartResponse.TotalQuantity = 0;
                    bookCartResponse.RemainingSlots = Max_CartItems_Per_Cart;
                    bookCartResponse.CanAddMore = true;
                    response.IsSuccess = true;
                    response.message = "Giỏ đang trống, có thể thêm sách vào";
                    response.data = bookCartResponse;
                    return response;
                }
                response.data = await MapToBookCartResponse(bookCart);
                response.IsSuccess = true;
                response.message = response.data.TotalQuantity >= Max_CartItems_Per_Cart
                        ? $"Giỏ đã đầy ({Max_CartItems_Per_Cart}/{Max_CartItems_Per_Cart} quyển)"
                        : $"Giỏ sách: {response.data.TotalQuantity}/{Max_CartItems_Per_Cart} quyển";
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess= false;
                response.message = $"Có lỗi khi tải giỏ sách: {ex}";
                return response;
            }
        }
        public string GenerateBorrowingCode(Guid borrowingId)
        {
            var today = DateTime.Now;

            string day = today.Day.ToString("D2");         // 17
            string month = today.Month.ToString("D2");     // 07
            string year = (today.Year % 10).ToString();    // 2025 -> 5

            string datePart = $"{day}{month}{year}";       // 17075
            string guidPart = borrowingId.ToString("N").Substring(0, 6).ToUpper(); // ABC123

            return $"#{datePart}-{guidPart}";
        }
        public async Task<BaseResponse<CheckoutBookCartResponse>> CheckoutBookCart(CheckoutBookCartResquest checkoutBookCartResquest)
        {
                BaseResponse<CheckoutBookCartResponse> response = new BaseResponse<CheckoutBookCartResponse>();
                var currentUserId = getCurrentUserId();

                BookCart cart = await _context.bookCarts.Include(x => x.BookCartItems).ThenInclude(x => x.Book).FirstOrDefaultAsync(x => x.UserId == currentUserId && x.CartStatus == CartStatus.Active);

                if (cart == null || !cart.BookCartItems.Any())
                {
                    response.IsSuccess = false;
                    response.message = "Giỏ sách trống";
                    return response;
                }

                Borrowing existingBorrowing = await _context.borrowings.FirstOrDefaultAsync(x => x.UserId == currentUserId && x.BorrowingStatus == BorrowingStatus.Borrowing && x.IsDeleted == false);

                if (existingBorrowing != null)
                {
                    response.IsSuccess = false;
                    response.message = "Bạn đã có phiếu mượn chưa trả";
                    return response;
                }

                Librarian librarian = await _context.librarians.FirstOrDefaultAsync(x => x.Id == currentUserId && x.IsDeleted == false);

                if (librarian == null)
                {
                    response.IsSuccess = false;
                    response.message = "Không tìm thấy thông tin người dùng";
                    return response;
                }

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                       Borrowing newBorrowing = new Borrowing();
                       newBorrowing.Id = Guid.NewGuid();
                       newBorrowing.CreateDate = DateTime.Now;
                    if (checkoutBookCartResquest.Duration > 10 && checkoutBookCartResquest.Duration < 1)
                    {
                        response.IsSuccess = false;
                        response.message = "Chỉ được mượn trong vòng 1-10 ngày";
                        return response;
                    }
                        newBorrowing.Duration = checkoutBookCartResquest.Duration;
                        newBorrowing.BorrowingStatus = BorrowingStatus.Wait;
                        newBorrowing.UserId = currentUserId;
                        newBorrowing.CreateUser = librarian.Name;
                        newBorrowing.IsDeleted = false;
                        newBorrowing.Code = GenerateBorrowingCode(newBorrowing.Id.Value);
                        newBorrowing.DueDate = newBorrowing.CreateDate.AddDays(newBorrowing.Duration);

                        await _context.borrowings.AddAsync(newBorrowing);

                        int totalBorrowedItems = 0;

                        foreach (var cartItem in cart.BookCartItems)
                        {
                            var availableBookItems = await _context.bookItems
                                .FromSqlRaw(@"
                                    SELECT TOP ({0}) *
                                    FROM bookItems WITH (UPDLOCK, ROWLOCK)
                                    WHERE BookId = {1}
                                    AND BookStatus = {2}
                                    AND IsDeleted = 0
                                    ORDER BY BarCode",
                                    cartItem.Quantity,           // TOP N
                                    cartItem.BookId,             // BookId
                                    (int)BookStatus.Available)   // Status
                                .ToListAsync();
                            
                            int foundCount = availableBookItems.Count;

                            if (foundCount == 0)
                            {
                                continue;
                            }

                            foreach (var bookItem in availableBookItems)
                            {
                                var detail = new BorrowingDetail
                                {
                                    Id = Guid.NewGuid(),
                                    BookItemId = bookItem.Id,
                                    BorrowingId = newBorrowing.Id,
                                    ReturnedDate = newBorrowing.DueDate,
                                    CreateDate = DateTime.Now,
                                    bookStatus = BookStatus.Borrowed,
                                    IsDeleted = false
                                };

                                await _context.borrowingDetails.AddAsync(detail);

                                bookItem.BookStatus = BookStatus.Borrowed;
                                bookItem.UpdateDate = DateTime.Now;
                                bookItem.UpdateUser = librarian.Name;
                                _context.bookItems.Update(bookItem);

                                totalBorrowedItems++;
                            }

                            var book = await _context.books.FirstOrDefaultAsync(x => x.Id == cartItem.BookId);

                            if (book != null)
                            {
                                book.Quantity -= foundCount;
                                book.UpdateDate = DateTime.Now;
                                book.UpdateUser = librarian.Name;
                                _context.books.Update(book);
                            }
                                _context.bookCartItems.Remove(cartItem);
                        }

                        if (totalBorrowedItems == 0)
                        {
                            await transaction.RollbackAsync();

                            response.IsSuccess = false;
                            response.message = "Không thể mượn quyển nào. Tất cả sách đều hết";
                            return response;
                        }

                        cart.CartStatus = CartStatus.CheckedOut;
                        cart.UpdateDate = DateTime.Now;
                        _context.bookCarts.Update(cart);

                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        var borrowingResponse = new BorrowingResponse
                        {
                            Id = newBorrowing.Id,
                            Code = newBorrowing.Code,
                            DueDate = newBorrowing.DueDate,
                            UserName = librarian.Name
                        };

                        var checkoutResponse = new CheckoutBookCartResponse
                        {
                            BorrowingResponse = borrowingResponse,
                            TotalBorrowedItems = totalBorrowedItems
                        };

                        response.IsSuccess = true;
                        response.message = $"Tạo phiếu mượn thành công! Đã mượn {totalBorrowedItems} quyển sách";
                        response.data = checkoutResponse;

                        return response;
                    }
                    catch 
                    {
                        await transaction.RollbackAsync();
                    }
                }
                response.IsSuccess = false;
                response.message = $"Có lỗi khi tạo phiếu mượn";
                return response;
        }
    }
}
