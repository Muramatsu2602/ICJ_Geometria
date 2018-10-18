﻿using System.Drawing;
using System.Windows.Forms;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;
using System.Drawing.Drawing2D;

namespace Geometricamente_V1
{
    public partial class frmGravaDescricao : Form
    {
        DateTime tempoInicial;
        TimeSpan diferencaTempo;
        String[] dados = new string[100];

        //gravação de audio
        [DllImport("winmm.dll")]
        private static extern long mciSendString(string command, StringBuilder retstring, int ReturnLength, IntPtr callback);

        private void frmFala_Load(object sender, EventArgs e)
        {
            pictureBox2.Enabled = false;
        }

        public frmGravaDescricao(Image imgForm, String[] dados)
        {
            InitializeComponent();
            /*Image img;
            img = Image.FromFile(caminhoImagem);*/
            // gravar audio
            mciSendString("open new Type waveaudio alias recsound", null, 0, IntPtr.Zero);
            picImagem.Image = imgForm;
            this.dados = dados;
        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            diferencaTempo = (DateTime.Now).Subtract(tempoInicial);
            lblTempo.Text = diferencaTempo.ToString("c");
        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
            mciSendString("record recsound", null, 0, IntPtr.Zero);
            pictureBox2.Enabled = false;
            pictureBox1.Enabled = true;
            //cronometro
            tempoInicial = DateTime.Now;
            timer1.Start();
        }

        private Cursor crossCursor(Pen pen, Brush brush, string name, int x, int y)
        {
            var pic = new Bitmap(x, y);
            Graphics gr = Graphics.FromImage(pic);

            var pathX = new GraphicsPath();
            var pathY = new GraphicsPath();
            pathX.AddLine(0, y / 2, x, y / 2);
            pathY.AddLine(x / 2, 0, x / 2, y);
            gr.DrawPath(pen, pathX);
            gr.DrawPath(pen, pathY);
            gr.DrawString(name, Font, brush, x / 2 + 5, y - 35);

            IntPtr ptr = pic.GetHicon();
            var c = new Cursor(ptr);
            return c;
        }

        private void btnParaGravar_Click(object sender, EventArgs e)
        {

            try
            {
                timer1.Stop();

                DialogResult dr = new DialogResult();
                dr = MessageBox.Show("Pronto! deseja salvar?", "GEOMETRIA", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (dr == DialogResult.Yes)
                {
                    DateTime agora = DateTime.Now;
                    mciSendString("Save recsound C:\\DADOS_SISTEMA\\audio\\" + agora.ToString("yyyy-MM-dd_HH-mm-ss") + "_img-" + dados[2] + "_" + dados[0] + "_" + dados[1] + "anos" + ".wav", null, 0, IntPtr.Zero);
                    mciSendString("close recsound", null, 0, IntPtr.Zero);
                    pictureBox2.Enabled = false;
                    pictureBox1.Enabled = false;
              
                }
                else
                {
                    pictureBox1.Enabled = true;
                    pictureBox2.Enabled = false;
                }
            }
            catch (Exception a)
            {
                MessageBox.Show("DEU ERRO" + a);
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void picImagem_MouseMove(object sender, MouseEventArgs e)
        {
            lblCoordenadas.Text = string.Format("X = {0}, Y = {1}", e.X, e.Y);
        }
    }
}






//


