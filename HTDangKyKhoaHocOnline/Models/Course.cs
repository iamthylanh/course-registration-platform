using System.ComponentModel.DataAnnotations;
namespace HTDangKyKhoaHocOnline.Models
{
    public class Course
    {
        //Mã khóa học
        public int CourseID { get; set; }
        [Required]
        //Tên khóa học
        public string CourseName { get; set; }
        //Giá khóa học
        public decimal Price {  get; set; }
        //Mô tả khóa học
        public string CourseDescription { get; set; }

        //Ngày bắt đầu khóa học
        [Required]
        public DateTime StartCourseDate { get; set; }

        //Ngày kết thúc khóa học
        [Required]
        public DateTime EndCourseDate { get; set; }

        //Navigation
        public ICollection<Enrollment> Enrollments { get; set; }

    }
}
