
using Microsoft.EntityFrameworkCore;
using Product_api.model;
namespace Product_api.Data
{
    public class AppDbContext:DbContext
    {
      
        public AppDbContext(DbContextOptions options):base(options)
        {
            
        }
        public DbSet<User> Users=>Set<User>();
       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=tcp:patientserver.database.windows.net,1433;Initial Catalog=PatientDb;Persist Security Info=False;User ID=saadmin;Password=Deepamanju12#$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }
    }
   
}