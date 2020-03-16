using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;   

namespace Html_IDE
{
    public partial class Form1 : Form
    {
        public String path = "";
        public String picPath;
        public static int pos;
        String dirPath = @"C:\HTML_IDE";
        String backupFile = @"C:\HTML_IDE\Savefile.html";
        

        int sx0 = 980; //Alapértelmezett felbontás sx0, sy0
        int sy0 = 580;
        int sx4 = 800;
        int sy4 = 600;

        public Form1()
        {
            InitializeComponent();

            if (Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);

            }
        }

        public void Masolas(String path) //a weboldalon megjelenő képeket másolja a mentés helyére
        {
                File.Copy(this.path, path, false);
        }

        private void mentésToolStripMenuItem1_Click(object sender, EventArgs e)
        {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Html fájlok (*.html)|*.html|Minden fájl (*.*)|*.*";
                sfd.ShowDialog();
                if (sfd.FileName == path)
                {
                    MessageBox.Show("Ez a fájl jelenleg használatban van!");
                }
                else
                {
                    path = sfd.FileName;

                    StreamWriter sw = new StreamWriter(path);

                    sw.Write(richTextBox1.Text);
                    sw.Flush();
                    sw.Close();
                }
            
        }

        private void ujProjektToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "<html>\n<head>\n<META HTTP-EQUIV=\"Content-Type\" CONTENT=\"text/html; charset=UTF-8\">\n</head>\n<body>\n</body>\n</html>";
        }
        
        private void betöltésToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Html fájlok (*.html)|*.html|Minden fájl (*.*)|*.*";
            ofd.ShowDialog();
            path = ofd.FileName;
            
            try
            {
                
                richTextBox1.Text = File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.StackTrace);
            }
        }

        private void kilépésToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            

            //Megjeleníti a fájlt de csak azt ami épp be van töltve
                try
                {
                    StreamWriter sw = new StreamWriter(backupFile);
                    sw.Write(richTextBox1.Text);
                    sw.Flush();
                    sw.Close();
                    Uri targetUri = new Uri(backupFile);
                    webBrowser1.Navigate(targetUri);
                }
                catch (Exception ex)
                {
                    Console.Out.WriteLine(ex.StackTrace);
                }
           
            
            //https://codingvision.net/interface/c-simple-syntax-highlighting

            //kulcsszavak
            string keywords = @"\b(html|head|body|title)\b";
            MatchCollection keywordMatches = Regex.Matches(richTextBox1.Text, keywords);

            
            string types = @"\b(href|font|size|META|img|src|face|color|align)\b";
            MatchCollection typeMatches = Regex.Matches(richTextBox1.Text, types);

            string other = @"\b(script)\b";
            MatchCollection otherMatches = Regex.Matches(richTextBox1.Text, other);

            // Menti a kurzor pozíciót és a színt
            int originalIndex = richTextBox1.SelectionStart;
            int originalLength = richTextBox1.SelectionLength;
            Color originalColor = Color.Black;

            richTextBox1.SelectionStart = 0;
            richTextBox1.SelectionLength = richTextBox1.Text.Length;
            richTextBox1.SelectionColor = originalColor;

            // keresés...
            foreach (Match m in keywordMatches)
            {
                richTextBox1.SelectionStart = m.Index;
                richTextBox1.SelectionLength = m.Length;
                richTextBox1.SelectionColor = Color.Blue;
            }

            foreach (Match m in typeMatches)
            {
                richTextBox1.SelectionStart = m.Index;
                richTextBox1.SelectionLength = m.Length;
                richTextBox1.SelectionColor = Color.Red;
            }

            foreach (Match m in otherMatches)
            {
                richTextBox1.SelectionStart = m.Index;
                richTextBox1.SelectionLength = m.Length;
                richTextBox1.SelectionColor = Color.Green;
            }

            // visszaállítja a színt
            richTextBox1.SelectionStart = originalIndex;
            richTextBox1.SelectionLength = originalLength;
            richTextBox1.SelectionColor = originalColor;

            
            
            
        }

        private void linkButton_Click(object sender, EventArgs e)
        {
            String link = "<a href=\""+urlTextbox.Text+"\" >"+linkText.Text+"</a>";
            String szoveg = richTextBox1.Text;
            
            if (pos < szoveg.Length)
            {
                String start = szoveg.Substring(0, pos);
                String end = szoveg.Substring(pos + 1);
                richTextBox1.Text = start + "\n" + link + "\n" + end;
            }
            else
            {
                richTextBox1.Text = "\n" + link + "\n";
            }
            linkText.Text = "";
            urlTextbox.Text = "";
        }

        private void szovegBeszurasButton_Click(object sender, EventArgs e)
        {
            //if (pos < szoveg.Length) nem jó helyen van
            
            String str = "\n<p>" + beszurandoSzoveg.Text + "</p>\n";
            String szoveg = richTextBox1.Text;
            
            String beszurando = "";
            if (kozepchek.Checked && !jobbszelchek.Checked && !balszelchek.Checked)
            {
                str = "\n<p align=\"center\" >" + beszurandoSzoveg.Text + "</p>\n";
                if (pos < szoveg.Length)
                {
                    if (felkover.Checked && !dolt.Checked && !alahuzott.Checked)
                    {
                        str = "\n<p align=\"center\" ><b><font size=\"" + betumeret.SelectedItem + "\" face=\"" + betutipus.SelectedItem + "\" color=\"" + betuszin.SelectedItem + "\">" + beszurandoSzoveg.Text + "</font></b></p>\n";
                            String start = szoveg.Substring(0, pos);
                            String end = szoveg.Substring(pos + 1);
                            beszurando = start + str + end;
                            richTextBox1.Text = beszurando;
                        
                    }
                    if (dolt.Checked)
                    {
                        str = "\n<p align=\"center\" ><i>" + beszurandoSzoveg.Text + "</font></i></p>\n";
                        String start = szoveg.Substring(0, pos);
                        String end = szoveg.Substring(pos + 1);
                        beszurando = start + str + end;
                        richTextBox1.Text = beszurando;
                    }
                    if (alahuzott.Checked)
                    {
                        str = "\n<p align=\"center\" ><u>" + beszurandoSzoveg.Text + "</font></u></p>\n";
                        String start = szoveg.Substring(0, pos);
                        String end = szoveg.Substring(pos + 1);
                        beszurando = start + str + end;
                        richTextBox1.Text = beszurando;
                    }
                    if (felkover.Checked && dolt.Checked)
                    {
                        str = "\n<p align=\"center\" ><b><i>" + beszurandoSzoveg.Text + "</font></b></i></p>\n";
                        String start = szoveg.Substring(0, pos);
                        String end = szoveg.Substring(pos + 1);
                        beszurando = start + str + end;
                        richTextBox1.Text = beszurando;
                    }
                    if (felkover.Checked && alahuzott.Checked)
                    {
                        str = "\n<p align=\"center\" ><b><u>" + beszurandoSzoveg.Text + "</font></b></u></p>\n";
                        String start = szoveg.Substring(0, pos);
                        String end = szoveg.Substring(pos + 1);
                        beszurando = start + str + end;
                        richTextBox1.Text = beszurando;
                    }
                    if (alahuzott.Checked && dolt.Checked)
                    {
                        str = "\n<p align=\"center\" ><u><i>" + beszurandoSzoveg.Text + "</font></u></i></p>\n";
                        String start = szoveg.Substring(0, pos);
                        String end = szoveg.Substring(pos + 1);
                        beszurando = start + str + end;
                        richTextBox1.Text = beszurando;
                    }
                    if (felkover.Checked && alahuzott.Checked && dolt.Checked)
                    {
                        str = "\n<p align=\"center\" ><b><u><i>" + beszurandoSzoveg.Text + "</font></b></u></i></p>\n";
                        String start = szoveg.Substring(0, pos);
                        String end = szoveg.Substring(pos + 1);
                        beszurando = start + str + end;
                        richTextBox1.Text = beszurando;
                    }
                    else
                    {
                    String start = szoveg.Substring(0, pos);
                    String end = szoveg.Substring(pos + 1);
                    beszurando = start + str + end;
                    richTextBox1.Text = beszurando;
                    }
                }
                else
                {
                    richTextBox1.Text = str;
                }
                
            }
            if (balszelchek.Checked && !kozepchek.Checked && !jobbszelchek.Checked)
            {
                str = "\n<p align=\"left\" >" + beszurandoSzoveg.Text + "</p>\n";
                if (pos < szoveg.Length)
                {
                    if (felkover.Checked)
                    {
                        str = "\n<p align=\"left\" ><b><font size=\""+betumeret.SelectedItem+"\" face=\""+betutipus.SelectedItem+"\" color=\""+betuszin.SelectedItem+"\">" + beszurandoSzoveg.Text + "</font></b></p>\n";
                        String start = szoveg.Substring(0, pos);
                        String end = szoveg.Substring(pos + 1);
                        beszurando = start + str + end;
                        richTextBox1.Text = beszurando;
                    }
                    if (dolt.Checked)
                    {
                        str = "\n<p align=\"left\" ><i>" + beszurandoSzoveg.Text + "</font></i></p>\n";
                        String start = szoveg.Substring(0, pos);
                        String end = szoveg.Substring(pos + 1);
                        beszurando = start + str + end;
                        richTextBox1.Text = beszurando;
                    }
                    if (alahuzott.Checked)
                    {
                        str = "\n<p align=\"left\" ><u>" + beszurandoSzoveg.Text + "</font></u></p>\n";
                        String start = szoveg.Substring(0, pos);
                        String end = szoveg.Substring(pos + 1);
                        beszurando = start + str + end;
                        richTextBox1.Text = beszurando;
                    }
                    if (felkover.Checked && dolt.Checked)
                    {
                        str = "\n<p align=\"left\" ><b><i>" + beszurandoSzoveg.Text + "</font></b></i></p>\n";
                        String start = szoveg.Substring(0, pos);
                        String end = szoveg.Substring(pos + 1);
                        beszurando = start + str + end;
                        richTextBox1.Text = beszurando;
                    }
                    if (felkover.Checked && alahuzott.Checked)
                    {
                        str = "\n<p align=\"left\" ><b><u>" + beszurandoSzoveg.Text + "</font></b></u></p>\n";
                        String start = szoveg.Substring(0, pos);
                        String end = szoveg.Substring(pos + 1);
                        beszurando = start + str + end;
                        richTextBox1.Text = beszurando;
                    }
                    if (alahuzott.Checked && dolt.Checked)
                    {
                        str = "\n<p align=\"left\" ><u><i>" + beszurandoSzoveg.Text + "</font></u></i></p>\n";
                        String start = szoveg.Substring(0, pos);
                        String end = szoveg.Substring(pos + 1);
                        beszurando = start + str + end;
                        richTextBox1.Text = beszurando;
                    }
                    if (felkover.Checked && alahuzott.Checked && dolt.Checked)
                    {
                        str = "\n<p align=\"left\" ><b><u><i>" + beszurandoSzoveg.Text + "</font></b></u></i></p>\n";
                        String start = szoveg.Substring(0, pos);
                        String end = szoveg.Substring(pos + 1);
                        beszurando = start + str + end;
                        richTextBox1.Text = beszurando;
                    }
                    else
                    {
                        String start = szoveg.Substring(0, pos);
                        String end = szoveg.Substring(pos + 1);
                        beszurando = start + str + end;
                        richTextBox1.Text = beszurando;
                    }
                }
                else
                {
                    richTextBox1.Text = str;
                }
            }
            if (jobbszelchek.Checked && !balszelchek.Checked && !kozepchek.Checked)
            {
                str = "\n<p align=\"right\" >" + beszurandoSzoveg.Text + "</p>\n";
                if (pos < szoveg.Length)
                {
                    if (felkover.Checked)
                    {
                        str = "\n<p align=\"right\" ><b>" + beszurandoSzoveg.Text + "</font></b></p>\n";
                        String start = szoveg.Substring(0, pos);
                        String end = szoveg.Substring(pos + 1);
                        beszurando = start + str + end;
                        richTextBox1.Text = beszurando;
                    }
                    if (dolt.Checked)
                    {
                        str = "\n<p align=\"right\" ><i>" + beszurandoSzoveg.Text + "</font></i></p>\n";
                        String start = szoveg.Substring(0, pos);
                        String end = szoveg.Substring(pos + 1);
                        beszurando = start + str + end;
                        richTextBox1.Text = beszurando;
                    }
                    if (alahuzott.Checked)
                    {
                        str = "\n<p align=\"right\" ><u>" + beszurandoSzoveg.Text + "</font></u></p>\n";
                        String start = szoveg.Substring(0, pos);
                        String end = szoveg.Substring(pos + 1);
                        beszurando = start + str + end;
                        richTextBox1.Text = beszurando;
                    }
                    if (felkover.Checked && dolt.Checked)
                    {
                        str = "\n<p align=\"right\" ><b><i>" + beszurandoSzoveg.Text + "</font></b></i></p>\n";
                        String start = szoveg.Substring(0, pos);
                        String end = szoveg.Substring(pos + 1);
                        beszurando = start + str + end;
                        richTextBox1.Text = beszurando;
                    }
                    if (felkover.Checked && alahuzott.Checked)
                    {
                        str = "\n<p align=\"right\" ><b><u>" + beszurandoSzoveg.Text + "</font></b></u></p>\n";
                        String start = szoveg.Substring(0, pos);
                        String end = szoveg.Substring(pos + 1);
                        beszurando = start + str + end;
                        richTextBox1.Text = beszurando;
                    }
                    if (alahuzott.Checked && dolt.Checked)
                    {
                        str = "\n<p align=\"right\" ><u><i>" + beszurandoSzoveg.Text + "</font></u></i></p>\n";
                        String start = szoveg.Substring(0, pos);
                        String end = szoveg.Substring(pos + 1);
                        beszurando = start + str + end;
                        richTextBox1.Text = beszurando;
                    }
                    if (felkover.Checked && alahuzott.Checked && dolt.Checked)
                    {
                        str = "\n<p align=\"right\" ><b><u><i>" + beszurandoSzoveg.Text + "</font></b></u></i></p>\n";
                        String start = szoveg.Substring(0, pos);
                        String end = szoveg.Substring(pos + 1);
                        beszurando = start + str + end;
                        richTextBox1.Text = beszurando;
                    }
                    else
                    {
                        String start = szoveg.Substring(0, pos);
                        String end = szoveg.Substring(pos + 1);
                        beszurando = start + str + end;
                        richTextBox1.Text = beszurando;
                    }
                }
                else
                {
                    richTextBox1.Text = str;
                }
            }
            else if (!jobbszelchek.Checked && !balszelchek.Checked && !kozepchek.Checked){
                if (pos < szoveg.Length)
                {
                    if (felkover.Checked)
                    {
                        str = "\n<p><b>" + beszurandoSzoveg.Text + "</font></b></p>\n";
                        String start = szoveg.Substring(0, pos);
                        String end = szoveg.Substring(pos + 1);
                        beszurando = start + str + end;
                        richTextBox1.Text = beszurando;
                    }
                    if (dolt.Checked)
                    {
                        str = "\n<p><i>" + beszurandoSzoveg.Text + "</font></i></p>\n";
                        String start = szoveg.Substring(0, pos);
                        String end = szoveg.Substring(pos + 1);
                        beszurando = start + str + end;
                        richTextBox1.Text = beszurando;
                    }
                    if (alahuzott.Checked)
                    {
                        str = "\n<p><u>" + beszurandoSzoveg.Text + "</font></u></p>\n";
                        String start = szoveg.Substring(0, pos);
                        String end = szoveg.Substring(pos + 1);
                        beszurando = start + str + end;
                        richTextBox1.Text = beszurando;
                    }
                    if (felkover.Checked && dolt.Checked)
                    {
                        str = "\n<p><b><i>" + beszurandoSzoveg.Text + "</font></b></i></p>\n";
                        String start = szoveg.Substring(0, pos);
                        String end = szoveg.Substring(pos + 1);
                        beszurando = start + str + end;
                        richTextBox1.Text = beszurando;
                    }
                    if (felkover.Checked && alahuzott.Checked)
                    {
                        str = "\n<p><b><u>" + beszurandoSzoveg.Text + "</font></b></u></p>\n";
                        String start = szoveg.Substring(0, pos);
                        String end = szoveg.Substring(pos + 1);
                        beszurando = start + str + end;
                        richTextBox1.Text = beszurando;
                    }
                    if (alahuzott.Checked && dolt.Checked)
                    {
                        str = "\n<p><u><i>" + beszurandoSzoveg.Text + "</font></u></i></p>\n";
                        String start = szoveg.Substring(0, pos);
                        String end = szoveg.Substring(pos + 1);
                        beszurando = start + str + end;
                        richTextBox1.Text = beszurando;
                    }
                    if (felkover.Checked && alahuzott.Checked && dolt.Checked)
                    {
                        str = "\n<p><b><i><u>" + beszurandoSzoveg.Text + "</font></i></u></b></p>\n";
                        String start = szoveg.Substring(0, pos);
                        String end = szoveg.Substring(pos + 1);
                        beszurando = start + str + end;
                        richTextBox1.Text = beszurando;
                    }
                }
                else
                {
                    richTextBox1.Text = str;
                }
            }
            pos = pos + (str.Length-1);
            beszurandoSzoveg.Text = "";

        }

        private void balszelchek_CheckedChanged(object sender, EventArgs e)
        {
            if (balszelchek.Checked)
            {
                jobbszelchek.Checked = false;
                kozepchek.Checked = false;
                
            }
        }

        private void kozepchek_CheckedChanged(object sender, EventArgs e)
        {
            if (kozepchek.Checked)
            {
                balszelchek.Checked = false;
                jobbszelchek.Checked = false;
                
            }
        }

        private void jobbszelchek_CheckedChanged(object sender, EventArgs e)
        {
            if (jobbszelchek.Checked)
            {
                balszelchek.Checked = false;
                kozepchek.Checked = false;
                
            }
        }

        private void tallozasButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Kép fájlok (*.jpg, .jpeg, .bmp)|*.jpg|*.bmp|*.jpeg|Minden fájl (*.*)|*.*";
            ofd.ShowDialog();
            picPath = ofd.FileName;
            picturePath.Text = ofd.SafeFileName;
            try
            {
                pictureBox1.Image = Image.FromFile(picPath);
                File.Copy(picPath, @"C:\HTML_IDE\" + ofd.SafeFileName, true);
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.StackTrace);
            }
        }

        private void kepbeszurasButton_Click(object sender, EventArgs e)
        {
            String kep = "<img src=\"" + picturePath.Text + "\" width=\"" + textBox1.Text + "\" height=\"" + textBox2.Text + "\" />";
            String szoveg = richTextBox1.Text;

            if (pos < szoveg.Length)
            {
                String start = szoveg.Substring(0, pos);
                String end = szoveg.Substring(pos + 1);
                richTextBox1.Text = start + "\n" + kep + "\n" + end;
            }
            else
            {
                richTextBox1.Text = "\n" + kep + "\n";
            }
            pictureBox1.Image = null;
        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            Form1.ActiveForm.Size = new Size(sx4, sy4);
            richTextBox1.Size = new Size((sx4/2)-50, sy4-110);
            tabControl.Location = new Point((sx4 / 2)-40, 30);
            tabControl.Size = new Size(sx4/2, 490);
            beszurandoSzoveg.Size = new Size(350, 250);
        }

        private void alapértelmezettToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1.ActiveForm.Size = new Size(sx0, sy0);
            richTextBox1.Size = new Size(400, 500);
            tabControl.Location = new Point(420, 30);
            tabControl.Size = new Size(530, 500);
            beszurandoSzoveg.Size = new Size(490, 270);
        }

        private void richTextBox1_MouseClick(object sender, MouseEventArgs e)
        {
            pos = richTextBox1.SelectionStart;
        }

        private void sortoresButton_Click(object sender, EventArgs e)
        {
            String szoveg = richTextBox1.Text;
            String str = "<br>";

            if (pos < szoveg.Length)
            {
                String start = szoveg.Substring(0, pos);
                String end = szoveg.Substring(pos + 1);
                richTextBox1.Text = start + "\n" + str + "\n" + end;
            }
            else
            {
                richTextBox1.Text = "\n" + str + "\n";
            }
            pos = pos + (str.Length - 1);
        }

        

        
    }
}
