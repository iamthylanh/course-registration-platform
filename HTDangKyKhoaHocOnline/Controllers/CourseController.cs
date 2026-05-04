using HTDangKyKhoaHocOnline.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HTDangKyKhoaHocOnline.Models;
using HTDangKyKhoaHocOnline.DTOs.Course;
using HTDangKyKhoaHocOnline.DTOs.Common;

namespace HTDangKyKhoaHocOnline.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly SystemDBContext _systemDBContext;
        public CourseController(SystemDBContext systemDBContext)
        {
            _systemDBContext = systemDBContext;
        }

        //Lấy tất cả khóa học
        [HttpGet]
        public IActionResult GetAll()
        {
            var courses = _systemDBContext.Course
                .Select(c => new CourseDTO
                {
                    CourseID = c.CourseID,
                    CourseName = c.CourseName,
                    Price = c.Price,
                    CourseDescription = c.CourseDescription,
                    StartCourseDate = c.StartCourseDate,
                    EndCourseDate = c.EndCourseDate,
                })
                .ToList();
            return Ok(new ApiResponse<List<CourseDTO>>(
                true,
                "Lấy danh sách khóa học thành công",
                courses
                )
            );
        }

        //Tạo khóa học (Admin)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(CreateCourseDTO createCourseDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>(false, "Dữ liệu không hợp lệ", null));

            if (createCourseDTO.StartCourseDate >= createCourseDTO.EndCourseDate)
            {
                return BadRequest(new ApiResponse<string>(false, "Thời gian kết thúc phải lớn hơn thời gian bắt đầu", null));
            }

            var course = new Course
            {
                CourseName = createCourseDTO.CourseName,
                Price = createCourseDTO.Price,
                CourseDescription = createCourseDTO.CourseDescription,
                StartCourseDate = createCourseDTO.StartCourseDate,
                EndCourseDate = createCourseDTO.EndCourseDate
            };
            
            _systemDBContext.Course.Add(course);
            _systemDBContext.SaveChanges();

            return Ok(new ApiResponse<Course>(true, "Tạo khóa học thành công", course));
        }

        //Cập nhật khóa học (Admin)
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateCourseDTO updateCourseDTO)
        {
            var course = _systemDBContext.Course.Find(id);
            if (course == null)
            {
                return NotFound(new ApiResponse<string>(false, "Không tìm thấy khóa học", null));
            }

            if (updateCourseDTO.StartCourseDate >= updateCourseDTO.EndCourseDate)
            {
                return BadRequest(new ApiResponse<string>(false, "Thời gian kết thúc phải lớn hơn thời gian bắt đầu", null));
            }

            course.CourseName = updateCourseDTO.CourseName;
            course.Price = updateCourseDTO.Price;
            course.CourseDescription = updateCourseDTO.CourseDescription;
            course.StartCourseDate = updateCourseDTO.StartCourseDate;
            course.EndCourseDate = updateCourseDTO.EndCourseDate;

            _systemDBContext.SaveChanges();
            return Ok(new ApiResponse<Course>(true, "Cập nhật khóa học thành công", course));
        }

        //Xóa khóa học (Admin)
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var course = _systemDBContext.Course.Find(id);
            if (course == null)
            {
                return NotFound(new ApiResponse<string>(false, "Không tìm thấy khóa học muốn xóa", null));
            }

            _systemDBContext.Course.Remove(course);
            _systemDBContext.SaveChanges();

            return Ok(new ApiResponse<string>(true,"Đã xóa", null));
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("{courseID}/students")]
        public IActionResult GetStudents(int courseID)
        {
            var students = _systemDBContext.Enrollment
                .Where(e => e.CourseID == courseID)
                .Select(e => new
                {
                    e.User!.UserID,
                    e.User.FullName,
                    e.User.Phone
                })
                .ToList();

            return Ok(new ApiResponse<object>(true, "Danh sách học viên", students));
        }
    }
}
