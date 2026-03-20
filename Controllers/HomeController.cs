using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QuPic.Models;
using QuPic.Data;
using QuPic.DTO;
using QuPic.Services.Interfaces;
using System.Net.Mail;


namespace QuPic.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;
    private readonly IEmailService _email;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IWebHostEnvironment env, IEmailService email)
    {
        _logger = logger;
        _context = context;
        _env = env;
        _email = email;
    }

    public IActionResult Home()
    {
        return View();
    }

    [HttpGet("Scanned/{guid}")]
    public IActionResult Scanned(string guid) 
    {
        var memories = _context.Memories.FirstOrDefault(c => c.Guid == guid);

        if (memories == null) return NotFound();

        return View(memories);
    }

    [HttpPost]
    public async Task<IActionResult> UploadImg([FromForm] IFormFile img)
    {
        if (img == null || img.Length == 0) return BadRequest();

        var uploadsPath = Path.Combine(_env.WebRootPath, "Uploads");

        if (!Directory.Exists(uploadsPath))
        {
            Directory.CreateDirectory(uploadsPath);
        }

        var fileName = Guid.NewGuid() + Path.GetExtension(img.FileName);
        var filePath = Path.Combine(uploadsPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await img.CopyToAsync(stream);
        }

        return Ok(new { fileName, url = $"/Uploads/{fileName}" });
    }

    [HttpPost]
    public async Task<IActionResult> UploadMemories([FromBody] ClientsMemoriesDto memoriesDto)
    {
        var guid = Guid.NewGuid().ToString();
        var url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Scanned/{guid}";

        var memories = new Memories
        {
          From = memoriesDto.From,
          To = memoriesDto.To,
          Message = memoriesDto.Message,
          Img = memoriesDto.Img,
          Guid = guid  
        };

        _context.Memories.Add(memories);
        await _context.SaveChangesAsync();

        return Ok( new { url });
    }

    [HttpPost]
    public async Task<IActionResult> SendEmailWithAttach([FromBody] EmailDto email)
    {
        if (string.IsNullOrEmpty(email.Img)) return BadRequest();

        var base64 = email.Img.Split(',')[1];
        var imageBytes = Convert.FromBase64String(base64);

        var subject = $"{email.From} QR Code";
        var body = "Here is your QR Code";

         await _email.SendEmailWithAttachAsync(
            email.To,             
            subject,
            body,
            imageBytes
        );

        return Ok();
    }
}
