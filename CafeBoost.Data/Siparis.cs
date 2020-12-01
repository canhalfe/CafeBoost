using CafeBoost.Data;
using CafeBoost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations.Schema;

namespace CafeBoost.Data
{
    [Table("Siparisler")]
    public class Siparis
    {
        public Siparis()
        {
            SiparisDetaylar = new HashSet<SiparisDetay>();
            AcilisZamani = DateTime.Now;
        }
        public int Id { get; set; }
        public int MasaNo { get; set; }
        public DateTime? AcilisZamani { get; set; }
        public DateTime? Kapaniszamani { get; set; }
        public SiparisDurum Durum { get; set; }
        public decimal OdenenTutar { get; set; }
        public string ToplamTutarTL => $"{ToplamTutar():0.00}₺"; //bu bir prop, seti yok get'i de gizli biz böyle tanımlayınca get'e gerek kalmıyor.

        public virtual ICollection<SiparisDetay> SiparisDetaylar { get; set; } 

        public decimal ToplamTutar()
        {
            /*
            decimal toplam = 0;
            foreach (var item in SiparisDetaylar)
            {
                toplam += item.Adet * item.BirimFiyat;
            }
            return toplam;
            */

            //yada;

            /*
            return SiparisDetaylar.Sum(x => x.Adet * x.BirimFiyat); 

            //sipariş detaylardaki her bir x'in (itemin) adetleriyle birim fiyatlarını çarp.
            */

            //yada;

            return SiparisDetaylar.Sum(x => x.Tutar());

        }
    }
}
