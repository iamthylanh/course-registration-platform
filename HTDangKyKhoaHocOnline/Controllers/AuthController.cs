using Microsoft.AspNetCore.Mvc;
using HTDangKyKhoaHocOnline.DTOs;
using HTDangKyKhoaHocOnline.Models;
using HTDangKyKhoaHocOnline.Data;
namespace HTDangKyKhoaHocOnline.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SystemDBContext _systemDBContext;
        public AuthController(SystemDBContext systemDBContext)
        {
            _systemDBContext = systemDBContext;
        }
        //=========== REGISTER =============
        [HttpPost("register")]
        public IActionResult Register(RegisterDTO registerDTO)
        {
            //Kiểm tra số điện thoại đã tồn tại chưa?
            if (_systemDBContext.User.Any(u => u.Phone == registerDTO.Phone))
            {
                return BadRequest("Số điện thoại đã tồn tại!!!");
            }
            //Thêm người dùng mới
            var user = new User
            {
                FullName = registerDTO.FullName,
                Phone = registerDTO.Phone,
                PasswordHash = registerDTO.Password, //Chưa Hash
            };
            _systemDBContext.Add(user);
            _systemDBContext.SaveChanges();
            return Ok("Đăng ký thành công.");
        }
        //========== LOGIN ==========
        [HttpPost("login")]
        public IActionResult Login(LoginDTO loginDTO)
        {
            var user = _systemDBContext.User
                .FirstOrDefault(u => u.Phone == loginDTO.Phone
                && u.PasswordHash == loginDTO.Password);
            if (user == null)
            {
                return Unauthorized("Sai số điện thoại hoặc mật khẩu.");
            }
            return Ok(new
            {
                message = "Đăng nhập thành công.",
                user.FullName,
                user.Phone,
                user.Role,
            });
        }
    }
}
