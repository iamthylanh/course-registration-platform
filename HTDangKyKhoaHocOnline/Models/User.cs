using System.ComponentModel.DataAnnotations;
namespace HTDangKyKhoaHocOnline.Models
{
    public class User
    {
        //Mã người dùng
        public int UserID { get; set; }

        //Tên người dùng
        [Required]
        public string FullName { get; set; }

        //Số điện thoại người dùng
        [Required]
        public string Phone { get; set; }

        //Email người dùng
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        //Mật khẩu người dùng
        [Required]
        public string PasswordHash { get; set; }

        //Quyền người dùng
        public string Role { get; set; } = "User";

        //Navigation
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
