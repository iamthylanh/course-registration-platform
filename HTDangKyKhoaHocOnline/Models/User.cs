using System.ComponentModel.DataAnnotations;
namespace HTDangKyKhoaHocOnline.Models
{
    public class User
    {
        //Mã người dùng
        public int UserID { get; set; }

        //Tên người dùng
        [Required]
        public string FullName { get; set; } = string.Empty;

        //Số điện thoại người dùng
        [Required]
        public string Phone { get; set; } = string.Empty;

        //Email người dùng
        [EmailAddress]
        public string? Email { get; set; }

        //Mật khẩu người dùng
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        //Quyền người dùng
        public string Role { get; set; } = "Admin";

        //Navigation
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
