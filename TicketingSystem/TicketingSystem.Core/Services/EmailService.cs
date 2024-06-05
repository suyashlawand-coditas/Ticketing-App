using System.Net.Mail;
using System.Net;
using TicketingSystem.Core.Domain.Entities;
using TicketingSystem.Core.ServiceContracts;

namespace TicketingSystem.Core.Services;
public class EmailService : IEmailService
{    
    private readonly string _smtpAddress = "smtp.gmail.com";
    private readonly int _portNumber = 587;
    private readonly bool _enableSSL = true;
    private readonly bool _activateService;
    
    private readonly string _emailFromAddress;
    private readonly string _password;

    public EmailService(string emailFromAddress, string password, bool activateService) 
    {
        _emailFromAddress = emailFromAddress;
        _password = password;
        _activateService = activateService;
    }

    public async Task SendTicketCreationEmail(Ticket ticket)
    {
        if (!_activateService) return;

        using MailMessage mail = new MailMessage();
        mail.From = new MailAddress(_emailFromAddress);
        mail.To.Add(ticket.RaisedBy!.Email);
        mail.Subject = $"You just raised a new ticket.";
        mail.Body = $"Hi {ticket.RaisedBy!.FullName}," + "<br />" +
            $"You raised new ticket successfully ({ticket.Title}). It was assigned to {ticket.TicketAssignment!.AssignedUser.FullName}";
        mail.IsBodyHtml = true;

        using SmtpClient smtp = new SmtpClient(_smtpAddress, _portNumber);
        smtp.Credentials = new NetworkCredential(_emailFromAddress, _password);
        smtp.EnableSsl = _enableSSL;
        await smtp.SendMailAsync(mail);
    }

    public async Task SendTicketResponseEmail(TicketResponse ticketResponse)
    {
        if (!_activateService) return;

        using MailMessage mail = new MailMessage();
        mail.From = new MailAddress(_emailFromAddress);
        mail.To.Add(ticketResponse.Ticket.RaisedBy!.Email);
        mail.Subject = $"Response on your ticket.";
        mail.Body = $"Hi {ticketResponse.Ticket.RaisedBy!.FullName}," + "<br />" +
            $"Someone responded on your ticket: ({ticketResponse.Ticket.Title}).";
        mail.IsBodyHtml = true;

        using SmtpClient smtp = new SmtpClient(_smtpAddress, _portNumber);
        smtp.Credentials = new NetworkCredential(_emailFromAddress, _password);
        smtp.EnableSsl = _enableSSL;
        await smtp.SendMailAsync(mail);
    }

    public async Task SendTicketStatusUpdateEmail(Ticket ticket)
    {
        if (!_activateService) return;

        using MailMessage mail = new MailMessage();
        mail.From = new MailAddress(_emailFromAddress);
        mail.To.Add(ticket.RaisedBy!.Email);
        mail.Subject = "Update on your ticket.";
        mail.Body = $"Hi {ticket.RaisedBy!.FullName}," + "<br />" +
            $"The status of ticket ({ticket.Title}) was just updated to {ticket.TicketStatus}.";
        mail.IsBodyHtml = true;

        using SmtpClient smtp = new SmtpClient(_smtpAddress, _portNumber);
        smtp.Credentials = new NetworkCredential(_emailFromAddress, _password);
        smtp.EnableSsl = _enableSSL;
        await smtp.SendMailAsync(mail);
    }
}