using System.ComponentModel.DataAnnotations;

namespace HTDangKyKhoaHocOnline.DTOs.Course
{
    public class CreateCourseDTO
    {
        public string CourseName { get; set; } = string.Empty;
        [Range(0, 100000000)]
        public decimal Price { get; set; }
        public string CourseDescription { get; set; } = string.Empty;
        public DateTime StartCourseDate { get; set; }
        public DateTime EndCourseDate { get; set; }
    }
}
