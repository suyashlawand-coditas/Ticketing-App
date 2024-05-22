using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Core.DTOs;
using TicketingSystem.Core.ServiceContracts;
using TicketingSystem.UI.Models;


namespace TicketingSystem.UI.Controllers
{
    [Route("[controller]/[action]")]
    public class TicketController : Controller
    {

        private readonly IDepartmentService _departmentService;
        private readonly ITicketService _ticketService;

        public TicketController(IDepartmentService departmentService, ITicketService ticketService)
        {
            _departmentService = departmentService;
            _ticketService = ticketService;
        }

        public async Task<IActionResult> CreateTicket()
        {
            AddTicketViewModel addTicketViewModel = new AddTicketViewModel();
            addTicketViewModel.Departments = await _departmentService.GetAllDepartments();

            ViewBag.AddTicketViewModel = addTicketViewModel;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromForm] CreateTicketDto createTicketDto)
        {
            TicketInfoDto ticketInfoDto = await _ticketService.CreateAndAutoAssignTicket(createTicketDto, ViewBag.User.UserId);

            AddTicketViewModel addTicketViewModel = new AddTicketViewModel();
            addTicketViewModel.Departments = await _departmentService.GetAllDepartments();
            
            ViewBag.AddTicketViewModel = addTicketViewModel;
            ViewBag.TicketInfo = ticketInfoDto;

            return View();
        }

    }
}
