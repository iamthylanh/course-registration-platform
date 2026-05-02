using Microsoft.AspNetCore.Mvc;
using HTDangKyKhoaHocOnline.Models;
using HTDangKyKhoaHocOnline.Data;
using BCrypt.Net;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using HTDangKyKhoaHocOnline.DTOs.Auth;

namespace HTDangKyKhoaHocOnline.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SystemDBContext _systemDBContext;
        private readonly IConfiguration _config;
        public AuthController(SystemDBContext systemDBContext, IConfiguration config)
        {
            _systemDBContext = systemDBContext;
            _config = config;
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
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password)
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
                .FirstOrDefault(u => u.Phone == loginDTO.Phone);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.PasswordHash))
            {
                return Unauthorized("Sai số điện thoại hoặc mật khẩu.");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("UserID", user.UserID.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                token = jwt,
                message = "Đăng nhập thành công.",
                user.FullName,
                user.Phone,
                user.Role,
            });
        }
    }
}
