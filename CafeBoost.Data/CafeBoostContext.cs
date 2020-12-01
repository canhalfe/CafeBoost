using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeBoost.Data
{
    public class CafeBoostContext : DbContext
    {
        public CafeBoostContext() : base("name=CafeBoostContext")
        {
            //output penceresinde çalışan sorguları göster
            Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //fluent api mapping;

            //bir sipariş detay ki; 
            //zorunlu bir Urun'u vardır
            //ki o Urun'ün birden çok SiparisDetay'ı vardır 
            //ki o SiparisDetay'lar UrunId alanı üzerinde foreign key ile Urun'le ilişki kurar
            //ki o Urun'u siler ise ona bağlı sipariş detaylar da silinmesin.
            modelBuilder
                .Entity<SiparisDetay>()
                .HasRequired(x => x.Urun)
                .WithMany(x => x.SiparisDetaylar)
                .HasForeignKey(x => x.UrunId) //gerek yok aslında çünkü yazmasan da zaten foreign key'dir.
                .WillCascadeOnDelete(false);
        }
        public int MasaAdet { get; set; } = 20;  //default masa adeti.
        public DbSet<Urun> Urunler { get; set; }
        public DbSet<Siparis> Siparisler { get; set; }
        public DbSet<SiparisDetay> SiparisDetaylar { get; set; }

        //kafe verilerini tutmak için ihtiyacımız olan prop listeleri oluşturmak için açtığımız class.

    }
}
