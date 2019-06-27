using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Diagnostics;
using EFMigrations.DataAccess.Configurations;

namespace EFMigrations.DataAccess
{
    public class EFMigrationContext : DbContext
    {

        public EFMigrationContext() : base("name=EfMigrationContext")
        {
        }

        public EFMigrationContext(string connectionString) : base(connectionString)
        {


        }

        public DbSet<Dealer> Dealers { get; set; }
        public DbSet<Make> Makes { get; set; }
        public DbSet<Model> Models { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new DealerEntityMappingConfiguration());
            modelBuilder.Configurations.Add(new MakeConfiguration());
            modelBuilder.Configurations.Add(new ModelConfiguration());
            modelBuilder.Configurations.Add(new DealerLocationConfiguration());

            base.OnModelCreating(modelBuilder);
        }



        public void Seed()
        {
            var context = this;
            if (!context.Dealers.Any(a => a.Name == "John Automobile"))
            {
                context.Dealers.Add(new Dealer()
                {
                    Name = "John Automobile",
                    IsActive = true,
                    OwnerName = "John"
                });
            }
            context.SaveChanges();
            if (!context.Dealers.Any(a => a.Name == "Michael Automobile"))
            {
                context.Dealers.Add(new Dealer()
                {
                    Name = "Michael Automobile",
                    IsActive = true,
                    OwnerName = "Michael"
                });
            }
            context.SaveChanges();
            if (!context.Makes.Any(a => a.Name == "BMW"))
            {
                context.Makes.Add(new Make()
                {
                    Name = "BMW",
                    Models = new List<Model>()
                     {
                         new Model(){ Name = "X5"},
                         new Model(){ Name = "X6"}
                     }

                });
            }
            context.SaveChanges();
            if (!context.Makes.Any(a => a.Name == "Toyota"))
            {
                context.Makes.Add(new Make()
                {
                    Name = "Toyota",
                    Models = new List<Model>()
                    {
                        new Model(){ Name = "Camry"},
                        new Model(){ Name = "Corola"}
                    }

                });
            }
            context.SaveChanges();

            var dealers = context.Dealers.ToList();
            foreach (var dealer in dealers)
            {
                if (dealer.DealerLocation == null)
                {
                    dealer.DealerLocation = new DealerLocation()
                    {
                        AddressLine1 = "Address1",
                        AddressLine2 = "Address 2",
                        Dealer = dealer,
                        City = "Fresno",
                        Country = "USA"

                    };
                }
                var makes = context.Makes.ToList();
                foreach (var make in makes)
                {
                    if (!make.Dealers.Contains(dealer))
                        make.Dealers.Add(dealer);
                }

                context.SaveChanges();
            }
        }

    }

    public class Dealer
    {
        public Dealer()
        {
            this.Makes = new HashSet<Make>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string OwnerName { get; set; }
        public bool IsActive { get; set; }
        public virtual DealerLocation DealerLocation { get; set; }
        public virtual ICollection<Make> Makes { get; set; }
    }

    public class Make
    {
        public Make()
        {
            this.Models = new HashSet<Model>();
            this.Dealers = new HashSet<Dealer>();
        }


        public int Id { get; set; }
        public string Name { get; set; }
        public string Other { get; set; }
        public ICollection<Model> Models { get; set; }
        public virtual ICollection<Dealer> Dealers { get; set; }
    }

    public class Model
    {
        public int MakeId { get; set; }
        public virtual Make Make { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class DealerLocation
    {
        public int DealerId { get; set; }
        public virtual Dealer Dealer { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }

    public class DealerEntityMappingConfiguration : EntityTypeConfiguration<Dealer>
    {

        public DealerEntityMappingConfiguration()
        {
            this.ToTable("Dealers");
            this.HasKey(k => k.Id);
            this.Property(p => p.OwnerName).HasMaxLength(50).IsRequired();
            this.Property(p => p.IsActive).IsRequired();
            this.HasMany(p => p.Makes).WithMany(p => p.Dealers).Map(mp =>
                {
                    mp.ToTable("DealerMakeMapping").MapLeftKey("DealerId").MapRightKey("MakeId");
                });
        }
    }

    public class DealerLocationConfiguration : EntityTypeConfiguration<DealerLocation>
    {

        public DealerLocationConfiguration()
        {
            this.ToTable("DealerLocations");
            this.HasKey(k => k.DealerId);
            this.HasRequired(p => p.Dealer).WithOptional(p => p.DealerLocation);
        }
    }

    public class MakeConfiguration : EntityTypeConfiguration<Make>
    {
        public MakeConfiguration()
        {
            this.ToTable("Makes")
                .HasKey(k => k.Id)
                .Property(p => p.Name);
            this.HasMany(p => p.Models).WithRequired(p => p.Make).HasForeignKey(k => k.MakeId);
        }
    }

    public class ModelConfiguration : EntityTypeConfiguration<Model>
    {
        public ModelConfiguration()
        {
            this.ToTable("Models");
            this.HasKey(p => p.Id);
        }
    }
}
