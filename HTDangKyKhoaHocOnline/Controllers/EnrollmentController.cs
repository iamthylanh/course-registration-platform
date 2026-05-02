using Microsoft.AspNetCore.Mvc;
using HTDangKyKhoaHocOnline.Data;
using HTDangKyKhoaHocOnline.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using HTDangKyKhoaHocOnline.DTOs.Course;
using Microsoft.EntityFrameworkCore;

namespace HTDangKyKhoaHocOnline.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class EnrollmentController : ControllerBase
    {
        private readonly SystemDBContext _systemDBContext;

        public EnrollmentController(SystemDBContext systemDBContext)
        {
            _systemDBContext = systemDBContext;
        }
        //Đăng ký khóa học
        [Authorize]
        [HttpPost("{courseID}")]
        public IActionResult Enroll(int courseID)
        {
            var userIdClaim = User.FindFirst("UserID");
            if (userIdClaim == null) return Unauthorized();

            var userID = int.Parse(userIdClaim.Value);

            //Check course tồn tại
            var course = _systemDBContext.Course.Find(courseID);
            if (course == null)
            {
                return NotFound("Khóa học không tồn tại");
            }
           
            //Check trùng đăng ký khóa học
            var exists = _systemDBContext.Enrollment
                .Any(e => e.UserID == userID && e.CourseID == courseID);
            if (exists)
            {
                return BadRequest("Bạn đã đăng ký khóa học này rồi");
            }

            //Tạo Enrollment
            var enrollment = new Enrollment
            {
                UserID = userID,
                CourseID = courseID,
                Progress = 0
            };
            _systemDBContext.Enrollment.Add(enrollment);
            _systemDBContext.SaveChanges();

            return Ok("Đăng ký thành công");
        }
        //Xem khóa học đã đăng ký
        [Authorize]
        [HttpGet]
        public IActionResult MyCourses()
        {
            var userIdClaim = User.FindFirst("UserID");
            if (userIdClaim == null) return Unauthorized();

            var userID = int.Parse(userIdClaim.Value);

            var courses = _systemDBContext.Enrollment
                .Where(e => e.UserID == userID && e.Course != null)
                .Select(e => new CourseDTO
                {
                    CourseID = e.Course!.CourseID,
                    CourseName = e.Course.CourseName,
                    Price = e.Course.Price,
                    CourseDescription = e.Course.CourseDescription,
                    StartCourseDate = e.Course.StartCourseDate,
                    EndCourseDate = e.Course.EndCourseDate,
                })
                .ToList();
            return Ok(courses);
        }
    }
}
