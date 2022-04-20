using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace EntityOrnek
{
    public partial class Form1 : Form
    {

        DbSinavOgrenciEntities db = new DbSinavOgrenciEntities();
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnDersListesi_Click(object sender, EventArgs e)
        {
            //Ado.Net ile bağlantı
            SqlConnection baglanti = new SqlConnection(@"Data Source=.;Initial Catalog=DbSinavOgrenci;Integrated Security=True");
            SqlCommand komut = new SqlCommand("Select * From tbldersler", baglanti);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void BtnOgrenciListele_Click(object sender, EventArgs e)
        {
            //Entity framework ile baglanti
            dataGridView1.DataSource = db.TBLOGRENCI.ToList();
            dataGridView1.Columns[3].Visible = false;
            dataGridView1.Columns[4].Visible = false;
        }

        private void BtnNotListesi_Click(object sender, EventArgs e)
        {
            //Linq sorgusu
            var query = from item in db.TBLNOTLAR
                        select new
                        {
                            item.NOTID,
                            item.OGR,
                            item.TBLOGRENCI.AD,
                            item.TBLOGRENCI.SOYAD,
                            item.DERS,
                            item.SINAV1,
                            item.SINAV2,
                            item.SINAV3,
                            item.ORTALAMA
                        };
            dataGridView1.DataSource = query.ToList();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            if (TxtAd.Text != "" && TxtSoyad.Text != "")
            {
                TBLOGRENCI t = new TBLOGRENCI();
                t.AD = TxtAd.Text;
                t.SOYAD = TxtSoyad.Text;
                db.TBLOGRENCI.Add(t);
                db.SaveChanges();
                MessageBox.Show("Öğrenci listeye eklenmiştir.");
            }
            if (TxtDersAd.Text != "")
            {
                TBLDERSLER t = new TBLDERSLER();
                t.DERSAD = TxtDersAd.Text;
                db.TBLDERSLER.Add(t);
                db.SaveChanges();
                MessageBox.Show("Ders listeye eklenmiştir.");
            }

        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(TxtOgrenciID.Text);
            var x = db.TBLOGRENCI.Find(id);
            db.TBLOGRENCI.Remove(x);
            db.SaveChanges();
            MessageBox.Show("Öğrenci sistemden silindi.");
        }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(TxtOgrenciID.Text);
            var x = db.TBLOGRENCI.Find(id);
            x.AD = TxtAd.Text;
            x.SOYAD = TxtSoyad.Text;
            x.FOTOGRAF = TxtFoto.Text;
            db.SaveChanges();
            MessageBox.Show("Öğrenci bilgileri başarıyla güncellendi.");
        }

        private void BtnProsedur_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.NOTLISTESI();
        }

        private void BtnBul_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.TBLOGRENCI.Where(x => x.AD == TxtAd.Text | x.SOYAD == TxtSoyad.Text).ToList();
        }

        private void TxtAd_TextChanged(object sender, EventArgs e)
        {
            //Linq sorgusu
            string aranan = TxtAd.Text;
            var degerler = from item in db.TBLOGRENCI
                           where item.AD.Contains(aranan)
                           select item;
            dataGridView1.DataSource = degerler.ToList();

        }

        private void BtnLinqEntity_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                //asc -- Ascending
                List<TBLOGRENCI> liste1 = db.TBLOGRENCI.OrderBy(p => p.AD).ToList();
                dataGridView1.DataSource = liste1;
            }
            if (radioButton2.Checked == true)
            {
                //desc -- Descending
                List<TBLOGRENCI> liste2 = db.TBLOGRENCI.OrderByDescending(p => p.AD).ToList();
                dataGridView1.DataSource = liste2;
            }
            if (radioButton3.Checked == true)
            {
                //first 3
                List<TBLOGRENCI> liste3 = db.TBLOGRENCI.OrderBy(p => p.AD).Take(3).ToList();
                dataGridView1.DataSource = liste3;
            }
            if (radioButton4.Checked == true)
            {
                //get by ID
                List<TBLOGRENCI> liste4 = db.TBLOGRENCI.Where(p => p.ID == 5).ToList();
                dataGridView1.DataSource = liste4;
            }
            if (radioButton5.Checked == true)
            {
                //start with ".."
                List<TBLOGRENCI> liste5 = db.TBLOGRENCI.Where(p => p.AD.StartsWith("a")).ToList();
                dataGridView1.DataSource = liste5;
            }
            if (radioButton6.Checked == true)
            {
                //end with ".."
                List<TBLOGRENCI> liste6 = db.TBLOGRENCI.Where(p => p.AD.EndsWith("a")).ToList();
                dataGridView1.DataSource = liste6;
            }
            if (radioButton7.Checked == true)
            {
                //any values in the table, the method will return true or false 
                bool deger = db.TBLKULUPLER.Any();
                MessageBox.Show(deger.ToString());
            }
            if (radioButton8.Checked == true)
            {
                //count the table
                int toplam = db.TBLOGRENCI.Count();
                MessageBox.Show(toplam.ToString(), "Toplam öğrenci sayısı"); 
            }
            if (radioButton9.Checked == true)
            {
                //sum the table
                var toplam = db.TBLNOTLAR.Sum(p => p.SINAV1);
                MessageBox.Show("Toplam sınav1 puanı: " + toplam.ToString());
            }
            if (radioButton10.Checked == true)
            {
                //average the table
                var ortalama = db.TBLNOTLAR.Average(p => p.SINAV1);
                MessageBox.Show("Sınav1 puan ortalaması: " + ortalama.ToString());
            }
            if (radioButtonHomework.Checked == true)
            {
                #region with names
                //var sinavOrtalamasi = db.TBLNOTLAR.Average(x => x.SINAV1);
                //var resultSet = (from x in db.TBLNOTLAR
                //                 where (x.SINAV1 > sinavOrtalamasi)
                //                 select new
                //                 {
                //                     x.NOTID,
                //                     OGRENCI = x.TBLOGRENCI.AD + " " + x.TBLOGRENCI.SOYAD,
                //                     x.TBLDERSLER.DERSAD,
                //                     x.SINAV1,
                //                     x.SINAV2,
                //                     x.SINAV3,
                //                     x.ORTALAMA,
                //                     x.DURUM
                //                 }
                // );
                //dataGridView1.DataSource = resultSet.ToList();
                #endregion

                //homework.. exam score more than average
                var ortalama = db.TBLNOTLAR.Average(p => p.SINAV1);
                List<TBLNOTLAR> liste = db.TBLNOTLAR.Where(p => p.SINAV1 >= ortalama).ToList();
                dataGridView1.DataSource = liste;
            }
            if (radioButton11.Checked == true)
            {
                //the highest score
                var enyuksek = db.TBLNOTLAR.Max(p => p.SINAV1);
                MessageBox.Show("En yüksek puan: " + enyuksek.ToString());
            }
            if (radioButton12.Checked == true)
            {
                //the highest score
                var endusuk = db.TBLNOTLAR.Min(p => p.SINAV1);
                MessageBox.Show("En yüksek puan: " + endusuk.ToString());
            }
            if (radioButtonHomework2.Checked == true)
            {
                #region farklı yontem
                var maxPoint = db.TBLNOTLAR.Max(x => x.SINAV1);
                var resultSet = (from x in db.TBLNOTLAR
                                 where (x.SINAV1 == maxPoint)
                                 select new
                                 {
                                     x.NOTID,
                                     OGRENCI = x.TBLOGRENCI.AD + " " + x.TBLOGRENCI.SOYAD,
                                     x.TBLDERSLER.DERSAD,
                                     x.SINAV1,
                                     x.SINAV2,
                                     x.SINAV3,
                                     x.ORTALAMA,
                                     x.DURUM
                                 }
                 );
                dataGridView1.DataSource = resultSet.ToList();
                #endregion

                var maxScore = db.TBLNOTLAR.Max(p => p.SINAV1);

                var ogrAd = (String)(from item in db.TBLNOTLAR
                               where item.SINAV1== maxScore
                               select item.TBLOGRENCI.AD).Take(1).First();

                
                MessageBox.Show(ogrAd);
            }
        }

        private void BtnJoin_Click(object sender, EventArgs e)
        {
            //join example
            var sorgu = from d1 in db.TBLNOTLAR
                        join d2 in db.TBLOGRENCI
                        on d1.OGR equals d2.ID
                        join d3 in db.TBLDERSLER
                        on d1.DERS equals d3.DERSID

                        select new
                        {
                            AD_SOYAD = d2.AD + " " + d2.SOYAD,
                            DERS = d3.DERSAD,
                            //ÖĞRENCİ = d2.AD,
                            //SOYAD = d2.SOYAD,
                            SINAV1 = d1.SINAV1,
                            SINAV2 = d1.SINAV2
                        };
            dataGridView1.DataSource = sorgu.ToList();
        }
    }
}