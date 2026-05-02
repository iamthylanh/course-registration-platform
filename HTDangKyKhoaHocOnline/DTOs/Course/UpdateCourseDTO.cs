namespace HTDangKyKhoaHocOnline.DTOs.Course
{
    public class UpdateCourseDTO
    {
        public string CourseName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string CourseDescription { get; set; } = string.Empty;
        public DateTime StartCourseDate { get; set; }
        public DateTime EndCourseDate { get; set; }
    }
}
