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

namespace PhotoSelect
{
    public partial class Form1 : Form
    {
        string[] files;
        string current;
        string path;
        int currentIndex;
        string toPlace = "";
        public Form1()
        {
            InitializeComponent();
            ChoseFolder(null, null);
            currentIndex = 0;
            SetPicture();
            this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            // this.TopMost = true;
            this.WindowState = FormWindowState.Normal;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            this.TopMost = true;
        }

        void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                toPlace = "accepted";
                movePhoto(); 
            }
            if(e.KeyCode == Keys.Back)
            {
                toPlace = "declined";
                movePhoto();
            }
            if(e.KeyCode == Keys.D)
            {
                toPlace = "komische_gesichter";
                movePhoto();

            }
        }

        private void ChoseFolder(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    path = fbd.SelectedPath;
                    files = Directory.GetFiles(fbd.SelectedPath, "*.JPG");
                    System.Diagnostics.Debug.Write(current);
                    MessageBox.Show("Fotos gefunden: " + files.Length.ToString(), "Fotos gefunden", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                }
            }
        }
        private void movePhoto()
        {
            if (currentIndex >= files.Length)
            {
                MessageBox.Show("Keine weiteren Fotos", "Fertig", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
            else
            {
                String[] splitted = files[currentIndex].Split('\\');
                current = splitted[splitted.Length - 1];

                String from = files[currentIndex];
                String to = Path.Combine(path + "\\" + toPlace + "\\" + current);

                Directory.CreateDirectory(path + "\\" + toPlace);
                File.Move(from, to);

                current = current.Split(new[] { '.' })[0] + ".CR2";

                try
                {
                    from = Path.Combine(path + "\\" + current);
                    to = Path.Combine(path + "\\" + toPlace + "\\raw\\" + current);
                    Directory.CreateDirectory(path + "\\" + toPlace + "\\raw");
                    File.Move(from, to);
                }
                catch (Exception)
                {

                }
                currentIndex++;
                if (currentIndex >= files.Length)
                {
                    MessageBox.Show("Keine weiteren Fotos", "Fertig", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }
                else
                {
                    SetPicture();
                }
            }
        }

        private void SetPicture()
        {
            if (currentIndex < files.Length)
            {
                using (FileStream fileStream = new FileStream(files[currentIndex], FileMode.Open))
                {
                    pictureBox1.Image = new Bitmap(fileStream);
                    pictureBox1.Update();

                }
            }

        }
        private void eww(object sender, EventArgs e)
        {
            toPlace = "komische_gesichter";
            movePhoto();
        }
        private void accepted(object sender, EventArgs e)
        {
            toPlace = "accepted";
            movePhoto();
        }
        private void nope(object sender, EventArgs e)
        {
            toPlace = "declined";
            movePhoto();
        }

    }
}
