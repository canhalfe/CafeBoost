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
    public partial class UrunlerForm : Form
    {
        private readonly KafeVeri db;

        BindingList<Urun> blUrunler;

        public UrunlerForm(KafeVeri kafeVeri)
        {
            db = kafeVeri;
            blUrunler = new BindingList<Urun>(db.Urunler);
            InitializeComponent();
            dgvUrunler.DataSource = blUrunler;
        }

        private void btnUrunEkle_Click(object sender, EventArgs e)
        {
            string urunAd = txtUrunAd.Text.Trim();

            if (urunAd == string.Empty)
            {
                errorProvider1.SetError(txtUrunAd, "Lütfen ürün adı giriniz.");
                return; //void metotta da return kullanılabilir tek başına.
            }
            if (UrunVarMi(urunAd))
            {
                errorProvider1.SetError(txtUrunAd, "Ürün zaten tanımlı.");
                return;
            }


            errorProvider1.SetError(txtUrunAd, "");

            blUrunler.Add(new Urun
            {
                UrunAd = urunAd,
                BirimFiyat = nudBirimFiyat.Value
            });

            txtUrunAd.Clear();
            nudBirimFiyat.Value = 0;
        }

        private void txtUrunAd_Validating(object sender, CancelEventArgs e)
        {
            if (txtUrunAd.Text.Trim() != "")
            {
                errorProvider1.SetError(txtUrunAd, "");
            }
        }

        private void dgvUrunler_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //https://stackoverflow.com/questions/14172883/validations-for-datagridview-cell-values-in-c-sharp

            Urun urun = (Urun)dgvUrunler.Rows[e.RowIndex].DataBoundItem;

            //mevcut hücrede değişiklik yapılmadıysa ya da yapıldysa ancak değer(isim) aynı kaldıysa;
            string mevcutDeger = dgvUrunler.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            
            if (!dgvUrunler.IsCurrentCellDirty || e.FormattedValue.ToString() == mevcutDeger)
            {
                return;
            }

            if (e.ColumnIndex == 0)
            {
                if (e.FormattedValue.ToString() == "")
                {
                    MessageBox.Show("ürün adı boş girilemez");
                    e.Cancel = true;
                }
                if (BaskaUrunVarMi(e.FormattedValue.ToString(), urun))
                {
                    MessageBox.Show("ürün zaten mevcut");
                    e.Cancel = true;
                }

            }
            else if (e.ColumnIndex == 1)
            {
                decimal birimFiyat;
                bool gecerliMi = decimal.TryParse(e.FormattedValue.ToString(), out birimFiyat);

                if (!gecerliMi || birimFiyat < 0)
                {
                    MessageBox.Show("Geçersiz Fiyat");
                    e.Cancel = true;
                }
            }
        }
        private bool UrunVarMi(string urunAd)
        {
            return db.Urunler.Any(
                x => x.UrunAd.Equals(urunAd, StringComparison.CurrentCultureIgnoreCase));
        }
        private bool BaskaUrunVarMi(string urunAd, Urun urun)
        {
            return db.Urunler.Any(
                x => x.UrunAd.Equals(urunAd, StringComparison.CurrentCultureIgnoreCase) && x != urun);
        }
    }
}
