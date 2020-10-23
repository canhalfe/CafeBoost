using CafeBoost.Data;
using CafeBoost.UI.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CafeBoost.UI
{
    public partial class AnaForm : Form
    {
        KafeVeri db; //kafeveri'deki listeleri çağırdık.
        public AnaForm()  //ctor
        {
            VeriOku();
            InitializeComponent();
            //OrnekUrunleriYukle();
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
            for (int i = 1; i <= db.MasaAdet; i++)
            {
                lvi = new ListViewItem("Masa " + i);
                
                lvi.ImageKey = db.AktifSiparisler.Any(x => x.MasaNo == i) ? "dolu" : "bos";

                lvi.Tag = i; //masaları etiketliyoruz ki hangisi hangisi belli olsun ve ilerde masa ismi vs değiştirildiği zaman kod patlamasın

                lvwMasalar.Items.Add(lvi);
            }
            #endregion
        }

        private void tsmiUrunler_Click(object sender, EventArgs e)
        {
            new UrunlerForm(db).ShowDialog();
        }

        private void tsmiGecmisSiparisler_Click(object sender, EventArgs e)
        {
            new GecmisSiparislerForm(db).ShowDialog();
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

            SiparisForm frmSiparis = new SiparisForm(db, siparis);

            frmSiparis.MasaTasindi += FrmSiparis_MasaTasindi;
            
            DialogResult dr = frmSiparis.ShowDialog();

            //sipariş iptal edildiyse ya da ödeme alındıysa;
            if (dr == DialogResult.OK)
            {
                lvwMasalar.SelectedItems[0].ImageKey = "bos";
            }
        }

        private void FrmSiparis_MasaTasindi(object sender, MasaTasimaEventArgs e)
        {
            MasaTasi(e.EskiMasaNo, e.YeniMasaNo);
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

        private void MasaTasi(int kaynak, int hedef)
        {
            foreach (ListViewItem lvi in lvwMasalar.Items)
            {
                if ((int)lvi.Tag == kaynak)
                {
                    lvi.ImageKey = "bos";
                }
                if ((int)lvi.Tag == hedef)
                {
                    lvi.ImageKey = "dolu";
                }
            }
        }

        private void AnaForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            VeriKaydet();
        }

        private void VeriOku()
        {
            try
            {
                string json = File.ReadAllText("veri.json");
                db = JsonConvert.DeserializeObject<KafeVeri>(json);
            }
            catch (Exception)
            {
                db = new KafeVeri();
            }
        }

        private void VeriKaydet()
        {
            string json = JsonConvert.SerializeObject(db, Formatting.Indented); 
            //formatting.intented -> düzenli yazdırmak için.
            File.WriteAllText("veri.json", json);
        }

        private void AnaForm_DoubleClick(object sender, EventArgs e)
        {

        }
    }
}
