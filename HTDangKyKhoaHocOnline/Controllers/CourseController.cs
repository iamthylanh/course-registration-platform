using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HTDangKyKhoaHocOnline.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public IActionResult GetCourse()
        {
            return Ok("Bạn đã đăng nhập nên mới thấy được.");
        }
    }
}
