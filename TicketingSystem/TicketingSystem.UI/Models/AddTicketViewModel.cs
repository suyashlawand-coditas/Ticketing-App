using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.Enums;


namespace TicketingSystem.UI.Models
{
    public class AddTicketViewModel
    {
        public List<Department> Departments { get; set; } = new List<Department>();
        public List<TicketStatus> TicketStatusList { get; set; } = new List<TicketStatus>();
        public List<TPriority> TPriorities { get; set; } = [
                TPriority.Low,
                TPriority.High,
                TPriority.Severe,
            ];
    }
}
