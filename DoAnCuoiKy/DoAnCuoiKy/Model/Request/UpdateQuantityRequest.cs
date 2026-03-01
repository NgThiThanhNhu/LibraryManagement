using System.ComponentModel.DataAnnotations;

namespace DoAnCuoiKy.Model.Request
{
    public class UpdateQuantityRequest
    {
        [Required(ErrorMessage = "Action là bắt buộc")]
        [RegularExpression("^(increase|decrease)$",
            ErrorMessage = "Action chỉ chấp nhận 'increase' hoặc 'decrease'")]
        public string Action { get; set; }
    }
}
