using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Linq;


namespace PicTransGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            string folderName = System.Windows.Forms.Application.StartupPath;

            // 取得資料夾內所有檔案
            bool has = false;
            foreach (string fname in System.IO.Directory.GetFiles(folderName))
            {
                // 一次讀取一行
                if (System.IO.Path.GetFileName(fname) == "PicTrans.exe") has = true;
                string image = "image";
                string mime = MimeType(fname);
                int cont = 0;
                for (int i = 0; i < 5; i++)
                {
                    if (mime[i] != image[i])
                    {
                        cont = 1;
                        break;
                    }
                }
                if (cont == 1) continue;
                comboBox1.Items.Add(System.IO.Path.GetFileName(fname));

            }
            if (!has)
            {
                MessageBox.Show("No PicTrans.exe found!");
                Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "PicTrans.exe";
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.UseShellExecute = false;        //關閉Shell的使用
            p.StartInfo.RedirectStandardInput = true;   //重定向標準輸入
            p.StartInfo.RedirectStandardOutput = true;  //重定向標準輸出
            p.Start();//启动程序
            p.StandardInput.WriteLine(textBox1.Text);
            p.StandardInput.WriteLine(textBox2.Text);
            p.StandardInput.WriteLine(comboBox1.Text);
            p.WaitForExit();
            MessageBox.Show("Convertion completed!");
            //string sourceName = System.Windows.Forms.Application.StartupPath + comboBox1.Text;
            string targetName = System.IO.Path.GetFileNameWithoutExtension(comboBox1.Text)+".coe";
            System.IO.File.Move("out.coe", targetName);
        }
        private string MimeType(string Filename)
        {
            string mime = "application/octetstream";
            string ext = System.IO.Path.GetExtension(Filename).ToLower();
            Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (rk != null && rk.GetValue("Content Type") != null)
                mime = rk.GetValue("Content Type").ToString();
            return mime;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var decoder = BitmapDecoder.Create(new Uri(System.Windows.Forms.Application.StartupPath+"/"+comboBox1.Text), BitmapCreateOptions.DelayCreation, BitmapCacheOption.None);
            var frame = decoder.Frames.FirstOrDefault();
            textBox1.Text = frame.PixelWidth.ToString();
            textBox2.Text = frame.PixelHeight.ToString();
            label4.Text = "Memory Size: "+(frame.PixelHeight * frame.PixelWidth).ToString();
        }
        
    }
}
