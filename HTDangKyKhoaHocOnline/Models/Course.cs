using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HTDangKyKhoaHocOnline.Models
{
    public class Course
    {
        //Mã khóa học
        public int CourseID { get; set; }

        //Tên khóa học
        [Required]
        public string CourseName { get; set; }

        //Giá khóa học
        [Column(TypeName = "decimal(11,3)")]
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
