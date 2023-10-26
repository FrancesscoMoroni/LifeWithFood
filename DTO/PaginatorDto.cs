namespace LifeWithFood.DTO
{
    public class PaginatorDto
    {
        public int Length { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PreviousPageIndex { get; set; }
    }
}
