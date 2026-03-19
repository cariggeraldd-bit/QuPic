using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QuPic.Models;
using QuPic.Data;


namespace QuPic.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Home()
    {
        return View();
    }


}
