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
            string? fileNameWithPath = null;

            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
                return View();
            }
            else
            {
                if (createTicketDto.Screenshot != null)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/TicketScreenshots");

                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);


                    FileInfo fileInfo = new FileInfo(createTicketDto.Screenshot.FileName);
                    string fileName = $"{Guid.NewGuid()}{fileInfo.Extension}";
                    fileNameWithPath = Path.Combine(path, fileName);

                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        createTicketDto.Screenshot.CopyTo(stream);
                    }
                }

                TicketInfoDto ticketInfoDto = await _ticketService.CreateAndAutoAssignTicket(createTicketDto, ViewBag.User.UserId, fileNameWithPath);

                AddTicketViewModel addTicketViewModel = new AddTicketViewModel();
                addTicketViewModel.Departments = await _departmentService.GetAllDepartments();

                ViewBag.AddTicketViewModel = addTicketViewModel;
                ViewBag.TicketInfo = ticketInfoDto;

                return View();
            }
        }

    }
}
