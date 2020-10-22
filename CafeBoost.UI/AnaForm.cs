using CafeBoost.Data;
using CafeBoost.UI.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CafeBoost.UI
{
    public partial class AnaForm : Form
    {
        int masaAdet = 20;
        KafeVeri db = new KafeVeri(); //kafeveri'deki listeleri çağırdık.
        public AnaForm()
        {
            InitializeComponent();
            OrnekUrunleriYukle();
            MasalariOlustur();
        }

        private void OrnekUrunleriYukle()
        {
            db.Urunler.Add(new Urun
            {
                UrunAd = "Kola",
                BirimFiyat = 6m
            });

            db.Urunler.Add(new Urun
            {
                UrunAd = "Ayran",
                BirimFiyat = 4m
            });           
        }

        private void MasalariOlustur()
        {
            #region İmage Listelerinin Oluşturulması

            ImageList il = new ImageList();
            il.Images.Add("bos", Resources.bos);
            il.Images.Add("dolu", Resources.dolu);
            il.ImageSize = new Size(80, 80);
            lvwMasalar.LargeImageList = il;

            #endregion

            #region Masaların Oluşturulması

            ListViewItem lvi;
            for (int i = 1; i <= masaAdet; i++)
            {
                lvi = new ListViewItem("Masa " + i);
                lvi.ImageKey = "bos";
                lvi.Tag = i; //masaları etiketliyoruz ki hangisi hangisi belli olsun ve ilerde masa ismi vs değiştirildiği zaman kod patlamasın

                lvwMasalar.Items.Add(lvi);
            }
            #endregion
        }

        private void tsmiUrunler_Click(object sender, EventArgs e)
        {
            new UrunlerForm().ShowDialog();
        }

        private void tsmiGecmisSiparisler_Click(object sender, EventArgs e)
        {
            new GecmisSiparislerForm().ShowDialog();
        }

        private void lvwMasalar_DoubleClick(object sender, EventArgs e)
        {
            int masaNo = (int)lvwMasalar.SelectedItems[0].Tag;

            //bu masa numaraları ile ;

            Siparis siparis = AktifSiparisBul(masaNo);

            if (siparis == null)  //sipariş boşsa yeni sipariş oluştur.
            {
                siparis = new Siparis();
                siparis.MasaNo = masaNo;
                db.AktifSiparisler.Add(siparis);
                lvwMasalar.SelectedItems[0].ImageKey = "dolu";
            }

            new SiparisForm(db,siparis).ShowDialog();
        }

        private Siparis AktifSiparisBul(int masaNo)
        {
            return db.AktifSiparisler.FirstOrDefault(x => x.MasaNo == masaNo);
          
            
            #region foreach yöntemi
            /*
            foreach (Siparis item in db.AktifSiparisler)
            {
                if (item.MasaNo == masaNo)
                {
                    return item;
                }
            }
            return null; 
            */
            #endregion
        }
    }
}
