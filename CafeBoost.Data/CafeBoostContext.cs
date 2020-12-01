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

        }
        public int MasaAdet { get; set; } = 20;  //default masa adeti.
        public DbSet<Urun> Urunler { get; set; }
        public DbSet<Siparis> Siparisler { get; set; }
        public DbSet<SiparisDetay> SiparisDetaylar { get; set; }

        //kafe verilerini tutmak için ihtiyacımız olan prop listeleri oluşturmak için açtığımız class.

    }
}
