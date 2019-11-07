using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BlogFall.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser//application user kullanıcı classına eşdeğer, identity senin için kulanıcı classı oluşturmak için miras aldık. bunlara ek propertyler ekleyebiliriz ad soyad gibi
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        [StringLength(100)]
        public string Photo { get; set; }

        public virtual ICollection<Post> Posts { get; set; }//bir yazarın yazıları olabilir

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>//application user identityden miras alır ekstra kendi tablolarımızı ekleyebiliriz.
    {
        public ApplicationDbContext()
            : base("name=ApplicationDbContext", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();//cascade delete kaldırıyoruz bbu default olarak açık bağlı tablolar için bire çok ilişkilerde otomatik silme özelliğini koyma
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Post> Posts { get; set; }
    }
}