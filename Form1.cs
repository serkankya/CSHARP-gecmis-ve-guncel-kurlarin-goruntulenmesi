using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static System.Net.WebRequestMethods;

namespace dovizkurlari
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private bool internetCheck()
        {
            try
            {
                string adres = "https://www.google.com";
                WebRequest req = WebRequest.Create(adres);
                WebResponse res = req.GetResponse();
            }
            catch (Exception ex)
            {
                MessageBox.Show("İnternet bağlantısı hatası." + ex);
                return false;
            }
            return true;
        }

        private void updateData()
        {
            if (internetCheck())
            {
                try
                {
                    string today = "https://www.tcmb.gov.tr/kurlar/today.xml";
                    var xml = new XmlDocument();
                    xml.Load(today);
                    DateTime date = Convert.ToDateTime(xml.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);

                    string USD = xml.SelectSingleNode("Tarih_Date/Currency [@Kod = 'USD']/BanknoteSelling").InnerXml;
                    lblUSD.Text = string.Format("Tarih : {0}\nUSD : {1}", date.ToLongDateString(), USD);

                    string EURO = xml.SelectSingleNode("Tarih_Date/Currency [@Kod = 'EUR']/BanknoteSelling").InnerXml;
                    lblEURO.Text = string.Format("Tarih : {0}\nEURO : {1}", date.ToLongDateString(), EURO);

                    string STR = xml.SelectSingleNode("Tarih_Date/Currency [@Kod = 'GBP']/BanknoteSelling").InnerXml;
                    lblSTR.Text = string.Format("Tarih : {0}\nSTERLIn : {1}", date.ToLongDateString(), STR);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("update kısmında hata oluştu. Hata : " + ex);
                }
            }
            else
            {
                MessageBox.Show("INTERNET BAĞLANTINIZI AKTİF EDİNİZ.");
                this.Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtTarih.Text = "https://www.tcmb.gov.tr/kurlar/GİRİLECEKTARİH.xml";
            timer1.Enabled = true;
            updateData();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                int saniye;

                label2.Text = DateTime.Now.Hour.ToString();
                label3.Text = DateTime.Now.Minute.ToString();

                saniye = Convert.ToInt32(label4.Text);
                saniye++;
                label4.Text = saniye.ToString();
                if (label4.Text == "60")
                {
                    label4.Text = "1";
                    updateData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("timer'da hata oluştu. Hata : " + ex);
            }
        }

        private void btnEskiKur_Click(object sender, EventArgs e)
        {
            try
            {
                string deneme = txtTarih.Text;

                var xml = new XmlDocument();
                xml.Load(deneme);
                DateTime date = Convert.ToDateTime(xml.SelectSingleNode("//Tarih_Date").Attributes["Tarih"].Value);

                string USD = xml.SelectSingleNode("Tarih_Date/Currency [@Kod = 'USD']/BanknoteSelling").InnerXml;
                lblOldUSD.Text = string.Format("Tarih : {0}\nUSD : {1}", date.ToLongDateString(), USD);

                string EURO = xml.SelectSingleNode("Tarih_Date/Currency [@Kod = 'EUR']/BanknoteSelling").InnerXml;
                lblOldEURO.Text = string.Format("Tarih : {0}\nEURO : {1}", date.ToLongDateString(), EURO);

                string STR = xml.SelectSingleNode("Tarih_Date/Currency [@Kod = 'GBP']/BanknoteSelling").InnerXml;
                lblOldSTR.Text = string.Format("Tarih : {0}\nSTERLIn : {1}", date.ToLongDateString(), STR);
            }
            catch (Exception ex)
            {
                MessageBox.Show("btn kısmında hata oluştu. Hata : " + ex);
            }
        }
    }
}
