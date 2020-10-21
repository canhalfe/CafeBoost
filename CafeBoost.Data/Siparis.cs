using CafeBoost.Data;
using CafeBoost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeBoost.Data
{
    public class Siparis
    {
        List<SiparisDetay> SiparisDetaylar = new List<SiparisDetay>();
        public int MasaNo { get; set; }
        public DateTime? AcilisZamani { get; set; }
        public DateTime? Kapaniszamani { get; set; }
        public SiparisDurum Durum { get; set; }
        public string ToplamTutarTL { get { return ToplamTutar() + "TL"; } }

        public decimal ToplamTutar()
        {
            return ToplamTutar();
        }
    }
}
