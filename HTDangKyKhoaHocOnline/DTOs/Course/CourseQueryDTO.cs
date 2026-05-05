namespace HTDangKyKhoaHocOnline.DTOs.Course
{
    public class CourseQueryDTO
    {
        //Tìm kiếm
        public string? Keyword { get; set; }
        //Lọc
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        //Phân trang
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        //Sắp xếp
        public string? SortBy { get; set; }
        public bool Desc { get; set; } = false;
    }
}
