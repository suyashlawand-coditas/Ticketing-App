using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Core.DTOs;
using TicketingSystem.Core.ServiceContracts;
using TicketingSystem.Core.Services;
using TicketingSystem.UI.Models;


namespace TicketingSystem.UI.Controllers
{
    [Route("[controller]/[action]")]
    public class TicketController : Controller
    {

        private readonly ITicketResponseService _ticketResponseService;
        private readonly IDepartmentService _departmentService;
        private readonly ITicketService _ticketService;

        public TicketController(IDepartmentService departmentService, ITicketService ticketService, ITicketResponseService ticketResponseService)
        {
            _departmentService = departmentService;
            _ticketService = ticketService;
            _ticketResponseService = ticketResponseService;
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> AddResponse([FromRoute] Guid id, [FromForm] string ticketResponse)
        {
            ViewUserDto userDto = (ViewUserDto)ViewBag.User;
            Guid userId = userDto.UserId;
            await _ticketResponseService.CreateTicketResponse(userId, id, ticketResponse);

            if (userDto.Role == Core.Enums.Role.Admin)
            {
                return LocalRedirect($"/Admin/TicketManagement/AssignedTickets/{id}");
            } else
            {
                return LocalRedirect($"/Person/TicketManagement/YourTickets/{id}");
            }
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
