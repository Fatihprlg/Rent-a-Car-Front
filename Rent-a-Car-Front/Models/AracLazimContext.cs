using Rent_a_Car.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace Rent_a_Car_Front.Models
{
    public class AracLazimContext : DbContext
    {

        public AracLazimContext()
            : base("name=AracLazimContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<Araba> Araba { get; set; }
        public virtual DbSet<Calisan> Calisan { get; set; }
        public virtual DbSet<Musteri> Musteri { get; set; }
        public virtual DbSet<Yonetici> Yonetici { get; set; }
        public virtual DbSet<KiralamaIslemi> KiralamaIslemi { get; set; }
    }
}