using StockManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace StockManagement.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Demande> Demandes { get; set; }
        public DbSet<Direction> Directions { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<HistoriqueMouvement> HistoriqueMouvements { get; set; }
        public DbSet<Produit> Produits { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Sortie> Sorties { get; set; }
        public DbSet<StatusDemande> StatusDemandes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<DemandeProduit> DemandeProduits { get; set; }
        public DbSet<HistoriqueStatusDemande> HistoriqueStatusDemandes { get; set; }
        public DbSet<Emplacement> Emplacements { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<SortieProduit> SortieProduits { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Salary)
                .HasColumnType("decimal(18,2)"); 

           
        }
    }
}
