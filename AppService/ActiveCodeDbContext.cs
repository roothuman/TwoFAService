using ArivalBank2FATask.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ArivalBank2FATask.AppService
{
    public class ActiveCodeDbContext : DbContext
    {
        public ActiveCodeDbContext(DbContextOptions<ActiveCodeDbContext> options) : base(options) { }
        public DbSet<ActiveCode> ActiveCodes { get; set; }
    }
}
