using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Tls;

namespace EmployeeManagement_WepApi.Models
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<OTPModel> OTPs { get; set; }
        public DbSet<DepartmentModel> Departments { get; set; }
        public DbSet<EmployeeModel> Employees { get; set; }
        public DbSet<CertificateModel> Certificates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define relationships and constraints
            modelBuilder.Entity<EmployeeModel>()
                .HasOne(e => e.User)
                .WithMany(u => u.Employees)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<EmployeeModel>()
                .HasOne(e => e.Certificate)
                .WithOne(c => c.Employee)
                .HasForeignKey<CertificateModel>(c => c.EmployeeId);
            modelBuilder.Entity<OTPModel>()
           .HasOne(otp => otp.User)
          .WithOne(user => user.OTP)
          .HasForeignKey<OTPModel>(otp => otp.UserId);

            modelBuilder.Entity<CertificateModel>()
               .HasOne(c => c.User)
               .WithMany(u => u.Certificates)
               .HasForeignKey(c => c.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
