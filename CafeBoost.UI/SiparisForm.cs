using CafeBoost.Data;
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
    public partial class SiparisForm : Form
    {
        readonly KafeVeri db;
        readonly Siparis siparis;

        private readonly AnaForm anaForm;

        readonly BindingList<SiparisDetay> blSiparisDetaylar;
        public SiparisForm(KafeVeri kafeVeri, Siparis siparis, AnaForm anaForm)
        {
            db = kafeVeri;

            this.siparis = siparis;
            this.anaForm = anaForm;
            InitializeComponent();
            dgvSiparisDetaylar.AutoGenerateColumns = false;

            MasalariListele();
            UrunleriListele();
            MasaNoGuncelle();
            OdemeTutariGuncelle();

            blSiparisDetaylar = new BindingList<SiparisDetay>(siparis.SiparisDetaylar);
            blSiparisDetaylar.ListChanged += BlSiparisDetaylar_ListChanged;
            dgvSiparisDetaylar.DataSource = blSiparisDetaylar;
        }

        private void MasalariListele()
        {
            cboMasalar.Items.Clear();
            
            for (int i = 1; i <= db.MasaAdet; i++)
            {
                if (!db.AktifSiparisler.Any(x => x.MasaNo == i))
                {
                    cboMasalar.Items.Add(i);
                }
                
            }
        }

        private void BlSiparisDetaylar_ListChanged(object sender, ListChangedEventArgs e)
        {
            OdemeTutariGuncelle();
        }

        private void OdemeTutariGuncelle()
        {
            lblOdemeTutari.Text = siparis.ToplamTutarTL;
        }

        private void UrunleriListele()
        {
            cboUrun.DataSource = db.Urunler;
        }

        private void MasaNoGuncelle()
        {
            //formun text'i
            Text = $"Masa {siparis.MasaNo:00} - Sipariş Detayları   Adisyon Açılış:{siparis.AcilisZamani.Value.ToShortTimeString()}";

            lblMasaNo.Text = siparis.MasaNo.ToString("00");
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            Urun secilenUrun = (Urun)cboUrun.SelectedItem;
            int adet = (int)nudAdet.Value;

            SiparisDetay detay = new SiparisDetay()
            {
                UrunAd = secilenUrun.UrunAd,
                BirimFiyat = secilenUrun.BirimFiyat,
                Adet = adet
            };
            blSiparisDetaylar.Add(detay);

        }

        private void dgvSiparisDetaylar_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            DialogResult dr =
                MessageBox.Show("Seçili detayları silmek istediğinize emin misiniz?",
                "Silme Onayı",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (dr != DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }

        private void btnAnasayfa_Click(object sender, EventArgs e)
        {
            Close();

            //yada

            //DialogResult = DialogResult.Cancel;
        }

        private void btnSiparisIptal_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(
                "Sipariş İptal Edilerek Kapatılacaktır. Emin misiniz?",
                "Ödeme Onayı",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (dr == DialogResult.Yes)
            {
                SiparisKapat(SiparisDurum.Iptal);
            }
        }

        private void btnOdemeAl_Click(object sender, EventArgs e)
        {
            SiparisKapat(SiparisDurum.Odendi, siparis.ToplamTutar());
            DialogResult dr = MessageBox.Show(
                "Ödeme alındıysa sipariş kapatılacaktır. Emin misiniz?",
                "Ödeme Onayı",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

        }
        private void SiparisKapat(SiparisDurum siparisDurum, decimal odenenTutar = 0)
        {
            siparis.OdenenTutar = odenenTutar;
            siparis.Kapaniszamani = DateTime.Now;
            siparis.Durum = siparisDurum;
            db.AktifSiparisler.Remove(siparis);
            db.GecmisSiparisler.Add(siparis);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnMasaTasi_Click(object sender, EventArgs e)
        {
            if (cboMasalar.SelectedIndex < 0) return;

            int hedef = (int)cboMasalar.SelectedItem;
            int kaynak = siparis.MasaNo;
            siparis.MasaNo = hedef;
            anaForm.MasaTasi(kaynak, hedef);

            MasaNoGuncelle();
            MasalariListele();


        }
    }
}
