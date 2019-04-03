/*
Ahmet Manga - 160202008
Zeki Esenalp - 160202033        
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Threading;

namespace Puzzle
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        bool change = false;
        PictureBox[] p;
        List<PictureBox> degistirilecek = new List<PictureBox>();
        string file_path;
        int skor = 100;
        int width1=0, height1=0;
        string dosya_adi; public List<Image> resimler = new List<Image>();
       
        
        private void button1_Click(object sender, EventArgs e)
        {
            baslangic:
            skor = 100;
           
            Directory.CreateDirectory(@"C:\Resim");
            file_path = "C:\\Resim\\";
            openFileDialog1.Title = "Resim Seçin";
            openFileDialog1.Filter = "Jpeg |*.jpg|Png |*.png|Jpeg|*.jpeg";
            openFileDialog1.Multiselect = false;
            openFileDialog1.ShowDialog();
            dosya_adi = openFileDialog1.FileName;

            int rows = 4, columns = 4;
            var imgarray = new Image[rows, columns];
            var img = new Bitmap(dosya_adi);
            height1 = img.Height;
            width1 = img.Width;

            if(img.Height%16 != 0 || img.Width%16 != 0)
            {
                MessageBox.Show("Çözünürlüğün 4'e bölünmesi gereklidir. Lütfen tekrar seçim yapınız.");
                goto baslangic;
            }

            int one_img_h = height1 / rows;
            int one_img_w = width1 / columns;

            for (int i = 0; i < rows; i++)
            {

                for (int j = 0; j < columns; j++)
                {

                    imgarray[i, j] = new Bitmap(one_img_w, one_img_h);
                    var graphics = Graphics.FromImage(imgarray[i, j]);
                    graphics.DrawImage(img, new Rectangle(0, 0, one_img_w, one_img_h), new Rectangle(j * one_img_w, i * one_img_h, one_img_w, one_img_h), GraphicsUnit.Pixel);
                    graphics.Dispose();
                }
            }
           
            var count = 1;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {

                    imgarray[i, j].Save(@"" + file_path + count + ".jpg");
                    resimler.Add(imgarray[i, j]);
                    count++;
                }
            }

            karistir();
            button2.Enabled = true; checkBox1.Enabled = true; textBox1.Text = "";              
        }
       


        bool[] images;

        private void button2_Click(object sender, EventArgs e)
        {
            karistir();
            crossCheck(p);
           
        }
        public void karistir()
        {
            images = new bool[17] { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true };
           
            int imageIndex;
            Random r = new Random();
          
            for(int i = 0; i < p.Length; i++)
            {
                if (i != 15)
                {
                    tekrar: imageIndex = r.Next(1, 17);  
                    if (!images[imageIndex]) goto tekrar;
                }else
                {
                    imageIndex = lastIndex();
                }
                
                p[i].Load(file_path + "\\" + imageIndex + ".jpg");
                images[imageIndex] = false; //MessageBox.Show(imageIndex.ToString());
            }
           // crossCheck(p);
        }
        public int lastIndex()
        {
            for(int i = 1; i < images.Length; i++)
            {
                if (images[i] == true) return i;
                
            }
            return 0;
        }
        bool text_ilkAcilis;
        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(Application.StartupPath + "\\enyuksekskor.txt") == false) text_ilkAcilis = true; else text_ilkAcilis = false;
            FileStream fs = new FileStream(Application.StartupPath + "\\enyuksekskor.txt", FileMode.OpenOrCreate);
            fs.Close();
            p = new PictureBox[16] {
                pictureBox1, pictureBox2, pictureBox3,pictureBox4,
                pictureBox5,pictureBox6,pictureBox7,
                pictureBox8,pictureBox9,pictureBox10,pictureBox11,
                pictureBox12,pictureBox13,pictureBox14,pictureBox15,pictureBox16};
         
         

            button2.Enabled = false;
            pictureBox1.Tag = 1; pictureBox2.Tag = 2; pictureBox3.Tag = 3; pictureBox4.Tag = 4;
            pictureBox5.Tag = 5; pictureBox6.Tag = 6; pictureBox7.Tag = 7; pictureBox8.Tag = 8;
            pictureBox9.Tag = 9; pictureBox10.Tag = 10; pictureBox11.Tag = 11; pictureBox12.Tag = 12;
            pictureBox13.Tag = 13; pictureBox14.Tag = 14; pictureBox15.Tag = 15; pictureBox16.Tag = 16;
            enYuksek();
            timer1.Enabled = true; timer1.Start();
        }
        
        public void enYuksek()
        {
            int eb;
            if (!text_ilkAcilis)
            {
                StreamReader oku = new StreamReader(Application.StartupPath + "\\enyuksekskor.txt");
                string yazi = oku.ReadLine();
                List<int> skorlar = new List<int>();
                while (yazi != null)
                {
                    skorlar.Add(int.Parse(yazi));
                    yazi = oku.ReadLine();
                }
                oku.Close();
                try { eb = skorlar[0]; } catch { eb = 0;  }
                for (int i = 1; i < skorlar.Count; i++)
                {
                    if (eb < skorlar[i]) eb = skorlar[i];
                }
            }else
            {
                eb = 0;
            }
            label1.Text = "En Büyük Skor: " + eb;
        }
        
        public void resimDegistir()
        {
                skor -= 3;
                label3.Text = skor.ToString();
                string url = degistirilecek[0].ImageLocation;
                degistirilecek[0].Load(degistirilecek[1].ImageLocation);
                degistirilecek[1].Load(url);
                textBox1.Text = degistirilecek[0].Tag + ". Resim ile " + degistirilecek[1].Tag + ". Resim değiştirildi.\r\n" + textBox1.Text;
                crossCheck(p);

        }
        public void a()
        {
            if (karistirmaKontrol())
            {
                button2.Enabled = false; checkBox1.Enabled = false;
                try
                {
                    label4.Text = degistirilecek[degistirilecek.Count - 1].Tag + " Seçildi";
                }
                catch { }
                resimDegistir();
                degistirilecek.Clear(); change = false;
            }else
            {
                MessageBox.Show("Eşleştirmeye başlayabilmeniz için en az bir resmin doğru yerde olması gerek.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
           degistirilecek.Add(pictureBox1); if (!change) change = true; else a();
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            degistirilecek.Add(pictureBox2); if (!change) change = true; else a();
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
          degistirilecek.Add(pictureBox3); if (!change) change = true; else a();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            degistirilecek.Add(pictureBox4); if (!change) change = true; else a();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            degistirilecek.Add(pictureBox5); if (!change) change = true; else a();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            degistirilecek.Add(pictureBox6); if (!change) change = true; else a();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            degistirilecek.Add(pictureBox7); if (!change) change = true; else a();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            degistirilecek.Add(pictureBox8); if (!change) change = true; else a();
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            degistirilecek.Add(pictureBox9); if (!change) change = true; else a();
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
           degistirilecek.Add(pictureBox10); if (!change) change = true; else a();
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
           degistirilecek.Add(pictureBox11); if (!change) change = true; else a();
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
           degistirilecek.Add(pictureBox12); if (!change) change = true; else a();
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
           degistirilecek.Add(pictureBox13); if (!change) change = true; else a();
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
         degistirilecek.Add(pictureBox14); if (!change) change = true; else a();
        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {
           degistirilecek.Add(pictureBox15); if (!change) change = true; else a();
        }

        private void pictureBox16_Click(object sender, EventArgs e)
        {
             degistirilecek.Add(pictureBox16); if (!change) change = true; else a();
        }
    
        public void crossCheck(PictureBox[] picList)
        {
              Bitmap yeni = new Bitmap(width1,height1);
              Bitmap kontrol = new Bitmap(dosya_adi);
            for (int i = 0; i < 16; i++)
            {
                Bitmap resim = new Bitmap(p[i].Image);
                for (int x = 0; x < height1 / 4; x++)
                {
                    for (int y = 0; y < width1 / 4; y++)
                    {
                        Color renk = resim.GetPixel(y, x);
                        yeni.SetPixel(i % 4 * width1 / 4 + y, i / 4 * height1 / 4 + x, renk);
                    }
                }
            }
            yeni.Save("resim.jpg");
            int sayac = 0;

            for (int m = 0; m < height1; m++)
            {
                for (int n = 0; n < width1; n++)
                {
                    if (kontrol.GetPixel(n, m).ToArgb().ToString() == yeni.GetPixel(n, m).ToArgb().ToString())
                    {
                        sayac++;
                    }

                }
            }


            if (sayac == width1*height1)
            {
                MessageBox.Show("Tebrikler! Oyun bitti. Skorunuz: " + skor, "Oyun Bitti!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                StreamWriter SW = File.AppendText(Application.StartupPath + "\\enyuksekskor.txt");
                SW.WriteLine(skor.ToString());
                SW.Close();
                enYuksek();
                skor = 100;
            }
        }

        public bool kucukResimKarsilastir(int h1, int w1, Bitmap kontrol, Bitmap resim)
        {
          //  MessageBox.Show("m max " + (h1 + (height1 / 4)) + " n max : " + (w1 + (width1 / 4)));
            int h2 = 0, w2 = 0;
            for (int m = h1; m < h1 + (height1/4); m++)
            {
                for (int n = w1; n < w1 + (width1/4); n++)
                {
                  //  MessageBox.Show("n " + n + " m " + m + " w2 " + w2 + " h2 " + h2);
                    try
                    {
                        if (resim.GetPixel(n, m).ToArgb().ToString() != kontrol.GetPixel(w2, h2).ToArgb().ToString())
                        {
                            return false;
                        }
                    }
                    catch {
                        if (h2 == h1 + (height1 / 4)) h2 = 0;
                        if (w2 == w1 + (width1 / 4)) w2 = 0;
                        continue;
                    }
                    w2++;
                }
                h2++;
            }
            return true;
        }

        public bool karistirmaKontrol()
        {
            ArrayList arr = new ArrayList();
            ArrayList arr2 = new ArrayList();
            for (int i = 0; i < p.Length; i++)
            {
                arr.Clear(); arr2.Clear();
                PictureBox picture = new PictureBox(); picture.Load(file_path + "\\" + (i + 1) + ".jpg");
                arr = getPixel(arr, p[i]); arr2 = getPixel(arr2, picture);
                int sonuc = difference(arr, arr2);
                if (sonuc == 100) return true;
            }
            return false;
        }

        public int difference(ArrayList arr, ArrayList arr2)
        {
            double esitlik = 0;
            double esit_olmayan = 0;
            for (int i = 0; i < arr.Count; i++)
            {
                if (arr[i].ToString() == arr2[i].ToString())
                {
                    esitlik++;
                }
                else
                {
                    esit_olmayan++;
                }
            }
            if (esit_olmayan == 0)
            {
                return 100;
            }
            else
            {          
                    return 0; 
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                for(int i = 0; i < p.Length; i++)
                {
                    p[i].Load(file_path + "\\"+(i+1)+".jpg");
                }
            }else
            {
                karistir();
            }
        }

     
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                label4.Text = degistirilecek[degistirilecek.Count - 1].Tag + " Seçildi";
            }
            catch { }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        public ArrayList getPixel(ArrayList arr, PictureBox pic)
        {
            Bitmap bmp = (Bitmap)Bitmap.FromFile(pic.ImageLocation);
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int k = 0; k < bmp.Height; k++)
                {
                    arr.Add(bmp.GetPixel(i, k).Name);
                }
            }
            return arr;
        }
    }
}
