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
    public partial class GecmisSiparislerForm : Form
    {
        private readonly CafeBoostContext db;
        public GecmisSiparislerForm(CafeBoostContext CafeBoostContext)
        {
            db = CafeBoostContext;
            InitializeComponent();
            dgvSiparisler.DataSource = db.Siparisler.Where(x => x.Durum != SiparisDurum.Aktif).ToList();
        }

        private void dgvSiparisler_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSiparisler.SelectedRows.Count > 0)  //en az 1 seçili satır varsa
            {
                //seçili satırlarının ilkinin üzerindeki Sipariş nesnesi(yani sipariş bilgilerinin tamamı) çağırılır;

                Siparis seciliSiparis = (Siparis)dgvSiparisler.SelectedRows[0].DataBoundItem;
                //databounditem --> o satıra bağlı olan nesne (bind'ın geçmiş zamanı)

                dgvSiparisDetaylar.DataSource = seciliSiparis.SiparisDetaylar;

            }
        }
    }
}
