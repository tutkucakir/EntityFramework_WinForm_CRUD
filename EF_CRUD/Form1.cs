using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EF_CRUD
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        RehberContext db = new RehberContext();
        private void Form1_Load(object sender, EventArgs e)
        {
            KisiListesiDoldur();
        }

        private void KisiListesiDoldur()
        {
            // DB'deki Kisiler tablosundaki verileri ListView'a dolduracak

            lstKisiler.Items.Clear(); // Kişi listesini doldurmadan önce temizle.

            foreach (Kisiler kisi in db.Kisilers.ToList())
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = kisi.ID.ToString();
                lvi.SubItems.Add(kisi.Adi);
                lvi.SubItems.Add(kisi.Soyadi);
                lvi.SubItems.Add(kisi.Telefonu);

                lvi.Tag = kisi;

                // İlgili ListView'a lvi nesnesini ekle.
                lstKisiler.Items.Add(lvi);
            }
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            string telefonKontrolEdilmis = "";
            if (txtTelefonu.Text.Length == 14) // (XXX) XXX-XXXX
            {
                telefonKontrolEdilmis = txtTelefonu.Text;

                //Kisiler yeniKisi = new Kisiler();
                //yeniKisi.Adi = txtAdi.Text;
                //yeniKisi.Soyadi = txtSoyadi.Text;
                //yeniKisi.Telefonu = telefonKontrolEdilmis;
                //db.Kisilers.Add(yeniKisi);


                db.Kisilers.Add(new Kisiler
                {
                    Adi = txtAdi.Text,
                    Soyadi = txtSoyadi.Text,
                    Telefonu = telefonKontrolEdilmis
                });
            }
            else
            {
                MessageBox.Show("Lütfen Telefon Numarasını Düzeltin!");
            }

            bool sonuc = db.SaveChanges() > 0; // Kaydedilen değişiklikler 0'dan büyük mü?

            MessageBox.Show(sonuc ? "Kişi Başarıyla Kaydedildi!" : "İşlem Hatalı!"); // ternary if
            KisiListesiDoldur();
            Temizle();
        }

        private void Temizle()
        {
            txtAdi.Text = "";
            txtSoyadi.Clear();
            txtTelefonu.Text = default(string);
            txtKisiAra.Text = default(string);
        }

        Kisiler guncellenecek;

        private void tsmGuncelle_Click(object sender, EventArgs e)
        {
            // TODO: İlgili satıra sağ tıklayıp GÜNCELLE seçeneği seçildiğinde, o satırdaki bilgileri TextBox'lara doldurunuz.

            if (lstKisiler.SelectedItems.Count <= 0)
                return;
            else
            {
                //int id = ((Kisiler)lstKisiler.SelectedItems[0].Tag).ID;
                //guncellenecek = db.Kisilers.Find(id);

                //guncellenecek = (Kisiler)lstKisiler.SelectedItems[0].Tag;
                guncellenecek = lstKisiler.SelectedItems[0].Tag as Kisiler;
                txtAdi.Text = guncellenecek.Adi;
                txtSoyadi.Text = guncellenecek.Soyadi;
                txtTelefonu.Text = guncellenecek.Telefonu;
            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            // TODO: Sağ tıklayarak, TextBox'lara aldığınız ilgili veriyi, güncelledikten sonra hem DB'de ilgili ID'ye sahip veri satırını güncelleyiniz, hem de ListView'da güncelleme işlemi yapınız.
            guncellenecek.Adi = txtAdi.Text;
            guncellenecek.Soyadi = txtSoyadi.Text;
            guncellenecek.Telefonu = txtTelefonu.Text;

            db.SaveChanges();
            KisiListesiDoldur();
            Temizle();
        }
        private void tsmSil_Click(object sender, EventArgs e)
        {
            // TODO: İlgili satıra sağ tıklayıp SİL seçeneği seçildiğinde, o satırdaki veriyi hem DB'den hem de ListView'dan siliniz.
            if (lstKisiler.SelectedItems.Count <= 0)
                return;
            else
            {
                //db.Kisilers.Remove(db.Kisilers.Find((lstKisiler.SelectedItems[0].Tag as Kisiler).ID));
                db.Kisilers.Remove((Kisiler)lstKisiler.SelectedItems[0].Tag);
                db.SaveChanges();
            }

            KisiListesiDoldur();
        }

        private void txtKisiAra_TextChanged(object sender, EventArgs e)
        {
            // txtKisiAra TextBox'a yazılan metni, DB'deki kayıtlar arasında AD veya SOYAD kolonunda arayıp, ilgili metin ile BAŞLAYAN AD veya SOYADA sahip kişileri ListView'a dolduran bir metot yazınız. Ve metodu bu Event'te çağırınız.
            // NOT: Metoda txtKisiAra.Text içerisindeki anlık metin bilgisini göndermelisiniz!
            // İPUCU: KisiListesiDoldur() metodunu overload ediniz.
            KisiListesiDoldur(txtKisiAra.Text);
        }
        private void KisiListesiDoldur(string text)
        {
            lstKisiler.Items.Clear();

            foreach (Kisiler kisi in db.Kisilers.Where(k => k.Adi.StartsWith(text) || k.Soyadi.StartsWith(text)).ToList())
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = kisi.ID.ToString();
                lvi.SubItems.Add(kisi.Adi);
                lvi.SubItems.Add(kisi.Soyadi);
                lvi.SubItems.Add(kisi.Telefonu);

                lvi.Tag = kisi;
                lstKisiler.Items.Add(lvi);
            }
        }
    }
}
