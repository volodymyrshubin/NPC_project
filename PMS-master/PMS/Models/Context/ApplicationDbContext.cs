using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMS.Models.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<StudentRoom> StudentRooms { get; set; }
        public DbSet<RoomFile> RoomFiles { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=PMS;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
}
