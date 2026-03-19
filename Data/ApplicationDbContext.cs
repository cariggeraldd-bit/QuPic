
using Microsoft.EntityFrameworkCore;
using QuPic.Models;

namespace QuPic.Data
{
   public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Memories> Memories { get; set; }
    } 
}