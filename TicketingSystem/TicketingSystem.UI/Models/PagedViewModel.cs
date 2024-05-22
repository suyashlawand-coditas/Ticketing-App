namespace TicketingSystem.UI.Models
{
    public class PagedViewModel<T>
    {
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public T? ViewModel { get; set; }
    }
}
