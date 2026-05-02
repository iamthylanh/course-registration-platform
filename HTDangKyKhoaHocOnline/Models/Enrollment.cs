using System.ComponentModel.DataAnnotations;
namespace HTDangKyKhoaHocOnline.Models
{
    //Tuyển sinh
    public class Enrollment
    {
        public int EnrollmentID { get; set; }

        //Khóa ngoại (id người dùng, id khóa học)
        public int UserID { get; set; }

        public int CourseID { get; set; }

        //Tiến độ 
        public int Progress { get; set; } = 0;

        //Navigation
        public User? User { get; set; }
        public Course? Course { get; set; }
    }
}
