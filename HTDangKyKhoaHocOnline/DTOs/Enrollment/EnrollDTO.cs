using System.ComponentModel.DataAnnotations;

namespace HTDangKyKhoaHocOnline.DTOs.Enrollment
{
    public class EnrollDTO
    {
        [Required]
        public int CourseID { get; set; }
    }
}
