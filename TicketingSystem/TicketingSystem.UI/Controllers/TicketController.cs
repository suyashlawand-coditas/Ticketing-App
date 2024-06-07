using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Core.DTOs;
using TicketingSystem.Core.ServiceContracts;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.UI.Models;
using Microsoft.AspNetCore.SignalR;
using TicketingSystem.UI.Hubs;
using System.Text.Json;


namespace TicketingSystem.UI.Controllers
{
    [Route("[controller]/[action]")]
    public class TicketController : Controller
    {

        private readonly ITicketResponseService _ticketResponseService;
        private readonly IDepartmentService _departmentService;
        private readonly ITicketService _ticketService;
        private readonly IHubContext<TicketResponseHub> _hubContext;

        public TicketController(IDepartmentService departmentService, ITicketService ticketService, ITicketResponseService ticketResponseService, IHubContext<TicketResponseHub> hubContext)
        {
            _departmentService = departmentService;
            _ticketService = ticketService;
            _ticketResponseService = ticketResponseService;
            _hubContext = hubContext;
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> AddResponse([FromRoute] Guid id, [FromForm] string ticketResponse)
        {
            ViewUserDto userDto = (ViewUserDto)ViewBag.User;
            Guid userId = userDto.UserId;
            var ticketResponseObj = await _ticketResponseService.CreateTicketResponse(userId, id, ticketResponse);

            await _hubContext.Clients.Groups(id.ToString()).SendAsync("TicketResponseMessage", JsonSerializer.Serialize(
                new
                {
                    TicketResponseId = ticketResponseObj.TicketResponseId,
                    ResponseMessage = ticketResponse,
                    UserName = userDto.FullName
                }
                ));

            if (userDto.Role == Core.Enums.Role.Admin)
            {
                Ticket ticket = await _ticketService.GetTicketById(id);
                if (ticket.RaisedById == userId)
                {
                    return LocalRedirect($"/Admin/TicketManagement/YourTickets/{id}");
                }
                else
                {
                    return LocalRedirect($"/Admin/TicketManagement/AssignedTickets/{id}");
                }
            }
            else
            {
                return LocalRedirect($"/Person/TicketManagement/YourTickets/{id}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> DeleteResponse([FromRoute] Guid id)
        {
            ViewUserDto userDto = (ViewUserDto)ViewBag.User;
            Guid userId = userDto.UserId;

            TicketResponse ticketResponse = await _ticketResponseService.GetTicketResponseById(id);
            if (ticketResponse.ResponseUserId == userId)
            {
                await _ticketResponseService.DeleteTicketResponse(ticketResponse.TicketResponseId);
            }

            await _hubContext.Clients.Groups(ticketResponse.TicketId.ToString()).SendAsync("DeleteTicketResponse", ticketResponse.TicketResponseId);

            Ticket ticket = await _ticketService.GetTicketById(ticketResponse.TicketId);
            if (userDto.Role == Core.Enums.Role.Admin)
            {

                if (ticket.RaisedById == userId)
                {
                    return LocalRedirect($"/Admin/TicketManagement/YourTickets/{ticket.TicketId}");
                }
                else
                {
                    return LocalRedirect($"/Admin/TicketManagement/AssignedTickets/{ticket.TicketId}");
                }
            }
            else
            {
                return LocalRedirect($"/Person/TicketManagement/YourTickets/{ticket.TicketId}");
            }
        }

        public async Task<IActionResult> CreateTicket()
        {
            AddTicketViewModel addTicketViewModel = new AddTicketViewModel();
            addTicketViewModel.Departments = await _departmentService.GetDepartmentsWithAtleastOneAdmin();

            ViewBag.AddTicketViewModel = addTicketViewModel;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromForm] CreateTicketDto createTicketDto)
        {
            string? fileNameToPass = null;

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
                    string fileNameWithPath = Path.Combine(path, fileName);
                    fileNameToPass = fileName;

                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        createTicketDto.Screenshot.CopyTo(stream);
                    }
                }

                TicketInfoDto ticketInfoDto = await _ticketService.CreateAndAutoAssignTicket(createTicketDto, ViewBag.User.UserId, fileNameToPass);

                AddTicketViewModel addTicketViewModel = new AddTicketViewModel();
                addTicketViewModel.Departments = await _departmentService.GetDepartmentsWithAtleastOneAdmin();

                ViewBag.AddTicketViewModel = addTicketViewModel;
                ViewBag.TicketInfo = ticketInfoDto;

                return View();
            }
        }

    }
}
