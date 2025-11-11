// Models/User.cs
using System.ComponentModel.DataAnnotations;

namespace STTechUserManagement.Models
{
    public class User
    {
        [Key]
        [StringLength(8)]
        public string Ma { get; set; } = string.Empty; // Sửa ở đây

        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        [StringLength(100)]
        public string HoTen { get; set; } = string.Empty; // Sửa ở đây

        [DataType(DataType.Date)]
        [Display(Name = "Ngày Sinh")]
        public DateTime NgayThangNamSinh { get; set; }

        [Display(Name = "Giới Tính")]
        [StringLength(10)]
        public string GioiTinh { get; set; } = string.Empty; // Sửa ở đây

        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty; // Sửa ở đây

        [Display(Name = "Điện Thoại")]
        [StringLength(15)]
        public string DienThoai { get; set; } = string.Empty; // Sửa ở đây

        [Display(Name = "Địa Chỉ")]
        [StringLength(255)]
        public string DiaChi { get; set; } = string.Empty; // Sửa ở đây
    }
}