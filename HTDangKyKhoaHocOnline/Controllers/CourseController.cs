using HTDangKyKhoaHocOnline.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HTDangKyKhoaHocOnline.Models;
using HTDangKyKhoaHocOnline.DTOs.Course;

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
            return Ok(_systemDBContext.Course.ToList());
        }

        //Tạo khóa học (Admin)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(CreateCourseDTO createCourseDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var course = new Course
            {
                CourseName = createCourseDTO.CourseName,
                Price = createCourseDTO.Price,
                CourseDescription = createCourseDTO.CourseDescription,
                StartCourseDate = createCourseDTO.StartCourseDate,
                EndCourseDate = createCourseDTO.EndCourseDate
            };
            if (createCourseDTO.StartCourseDate >= createCourseDTO.EndCourseDate)
            {
                return BadRequest("Ngày không hợp lệ");
            }
            _systemDBContext.Course.Add(course);
            _systemDBContext.SaveChanges();

            return Ok(course);
        }

        //Cập nhật khóa học (Admin)
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateCourseDTO updateCourseDTO)
        {
            var course = _systemDBContext.Course.Find(id);
            if (course == null)
            {
                return NotFound();
            }
            course.CourseName = updateCourseDTO.CourseName;
            course.Price = updateCourseDTO.Price;
            course.CourseDescription = updateCourseDTO.CourseDescription;
            course.StartCourseDate = updateCourseDTO.StartCourseDate;
            course.EndCourseDate = updateCourseDTO.EndCourseDate;

            if (updateCourseDTO.StartCourseDate >= updateCourseDTO.EndCourseDate)
            {
                return BadRequest("Ngày không hợp lệ");
            }
            _systemDBContext.SaveChanges();
            return Ok(course);
        }

        //Xóa khóa học (Admin)
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var course = _systemDBContext.Course.Find(id);
            if (course == null)
            {
                return NotFound();
            }

            _systemDBContext.Course.Remove(course);
            _systemDBContext.SaveChanges();

            return Ok("Đã xóa");
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

            return Ok(students);
        }
    }
}
