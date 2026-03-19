using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QuPic.Models;
using QuPic.Data;
using QuPic.DTO;


namespace QuPic.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IWebHostEnvironment env)
    {
        _logger = logger;
        _context = context;
        _env = env;
    }

    public IActionResult Home()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UploadImg([FromForm] IFormFile img)
    {
        if (img == null || img.Length == 0) return BadRequest();

        var uploadsPath = Path.Combine(_env.ContentRootPath, "Uploads");

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

        return Ok(new { fileName, url = $"~/Uploads/{fileName}" });
    }

    [HttpPost]
    public async Task<IActionResult> UploadMemories([FromBody] ClientsMemoriesDto memoriesDto)
    {
        var guid = Guid.NewGuid().ToString();
        var url = "https://polygamistic-compunctiously-olga.ngrok-free.dev/Scanned/";

        var qrUrl = $"{url}{guid}";

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

        return Ok( new { qrUrl });
    }
}
