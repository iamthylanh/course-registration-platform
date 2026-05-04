using Microsoft.AspNetCore.Mvc;
using HTDangKyKhoaHocOnline.Data;
using Microsoft.AspNetCore.Authorization;
using HTDangKyKhoaHocOnline.DTOs.Course;
using HTDangKyKhoaHocOnline.Services;
using HTDangKyKhoaHocOnline.DTOs.Common;
namespace HTDangKyKhoaHocOnline.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class EnrollmentController : ControllerBase
    {
        private readonly SystemDBContext _systemDBContext;
        private readonly EnrollmentService _enrollmentService;

        public EnrollmentController(SystemDBContext systemDBContext, EnrollmentService enrollmentService)
        {
            _systemDBContext = systemDBContext;
            _enrollmentService = enrollmentService;
        }
        //Đăng ký khóa học
        [Authorize]
        [HttpPost("{courseID}")]
        public IActionResult Enroll(int courseID)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>(false, "Dữ liệu không hợp lệ", null));

            var userIdClaim = User.FindFirst("UserID");
            if (userIdClaim == null) 
                return Unauthorized(new ApiResponse<string>(false, "Unauthorized", null));

            var userID = int.Parse(userIdClaim.Value);

            var result = _enrollmentService.Enroll(userID, courseID);

            return Ok(new ApiResponse<string>(true, result, null));
        }
        //Xem khóa học đã đăng ký
        [Authorize]
        [HttpGet]
        public IActionResult MyCourses()
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>(false, "Dữ liệu không hợp lệ", null));

            var userIdClaim = User.FindFirst("UserID");
            if (userIdClaim == null)
                return Unauthorized(new ApiResponse<string>(false, "Unauthorized", null));

            var userID = int.Parse(userIdClaim.Value);

            var courses = _enrollmentService.GetMyCourses(userID);

            return Ok(new ApiResponse<List<CourseDTO>>(
                true,
                "Lấy danh sách khóa học đã đăng ký",
                courses
                ));
        }
    }
}
