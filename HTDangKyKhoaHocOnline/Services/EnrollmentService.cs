using HTDangKyKhoaHocOnline.Data;
using HTDangKyKhoaHocOnline.DTOs.Course;
using HTDangKyKhoaHocOnline.Models;

namespace HTDangKyKhoaHocOnline.Services
{
    public class EnrollmentService
    {
        private readonly SystemDBContext _systemDBContext;
        public EnrollmentService(SystemDBContext systemDBContext)
        {
            _systemDBContext = systemDBContext;
        }
        public string Enroll(int userID, int courseID)
        {
            var course = _systemDBContext.Course.Find(courseID);
            if (course == null)
            {
                throw new Exception("Khóa học không tồn tại");
            }
            var existed = _systemDBContext.Enrollment
                .Any(e => e.UserID == userID && e.CourseID == courseID);
            if (existed)
            {
                throw new Exception("Bạn đã đăng ký khóa học này rồi");
            }
            var enrollment = new Enrollment
            {
                UserID = userID,
                CourseID = courseID,
                Progress = 0
            };
            _systemDBContext.Enrollment.Add(enrollment);
            _systemDBContext.SaveChanges();

            return "Đăng ký khóa học thành công";
        }

        public List<CourseDTO> GetMyCourses(int userID)
        {
            return _systemDBContext.Enrollment
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
        }
    }
}
