using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeBoost.Data
{
    public class KafeVeri
    {
        public int MasaAdet { get; set; } = 20;  //default masa adeti.
        public List<Urun> Urunler { get; set; }
        public List<Siparis> AktifSiparisler { get; set; }
        public List<Siparis> GecmisSiparisler { get; set; }

        //kafe verilerini tutmak için ihtiyacımız olan prop listeleri oluşturmak için açtığımız class.

        public KafeVeri()
        {
            Urunler = new List<Urun>();
            AktifSiparisler = new List<Siparis>();
            GecmisSiparisler = new List<Siparis>();           
        }
    }
}
