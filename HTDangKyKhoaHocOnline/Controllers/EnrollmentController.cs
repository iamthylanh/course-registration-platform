using Microsoft.AspNetCore.Mvc;
using HTDangKyKhoaHocOnline.Data;
using HTDangKyKhoaHocOnline.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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

            if (userIdClaim == null)
            {
                return Unauthorized("Không tìm thấy UserID trong token");
            }

            var userID = int.Parse(userIdClaim.Value);

            var exists = _systemDBContext.Enrollment
                .Any(e => e.UserID == userID && e.CourseID == courseID);
            if (exists)
            {
                return BadRequest("Bạn đã đăng ký khóa học này rồi");
            }

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

            if (userIdClaim == null)
            {
                return Unauthorized("Không tìm thấy UserID trong token");
            }

            var userID = int.Parse(userIdClaim.Value);

            var courses = _systemDBContext.Enrollment
                .Where(e => e.UserID == userID)
                .Select(e => e.Course)
                .ToList();

            return Ok(courses);
        }
    }
}
