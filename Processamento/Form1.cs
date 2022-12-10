using log4net.Core;
using log4net.Filter;
using Svg.FilterEffects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Processamento
{
    public partial class BtDivisao : Form
    {
        private Bitmap imgA;
        private int[] histogramaR, histogramaG, histogramaB;
        private byte[,] vImgAGray;
        private byte[,] vImgAR;
        private byte[,] vImgAG;
        private byte[,] vImgAB;
        private byte[,] vImgAAlfa;

        private Bitmap imgB;
        private byte[,] vImgBGray;
        private byte[,] vImgBR;
        private byte[,] vImgBG;
        private byte[,] vImgBB;
        private byte[,] vImgBAlfa;

        private Bitmap imgR;
        private byte[,] vImgRGray;
        private byte[,] vImgRR;
        private byte[,] vImgRG;
        private byte[,] vImgRB;
        private byte[,] vImgRAlfa;
        private int tomR, tomG, tomB, novoTomR, novoTomG, novoTomB;
        float[] histAcumuladoR, histAcumuladoG, histAcumuladoB;
        private int[] mapaCoresR, mapaCoresG, mapaCoresB;

        

        public BtDivisao()
        {
            InitializeComponent();
        }

        public class Ordenacao
        {
            //retorna a array ordenada
            public int[] BubbleSort(int[] vetor)
            {
                for (int i = 0; i < vetor.Length; i++)
                {
                    for (int j = 0; j < vetor.Length - 1; j++)
                    {
                        if (vetor[j] > vetor[j + 1])
                        {
                            int swap = vetor[j];
                            vetor[j] = vetor[j + 1];
                            vetor[j + 1] = swap;
                        }
                    }

                }
                return vetor;
            }
        }

        private void btLoadA_Click(object sender, EventArgs e)
        {
            //Abre a caixa de dialogo 
            var filePath = string.Empty;
            openFileDialog1.InitialDirectory = "C:\\Matlab";
            openFileDialog1.Filter = "TIFF image (*.tif)|*.tif" +
            "| JPG image (*.jpg)|.jpg" +
            "| BMP image (*.bmp)|*.bmp" +
            "| PNG image (*.png)|*.png" +
            "| All image (*.*)|*.*";

            //openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            //Se o arquivo tiver sido localizado com sucesso:
            if (openFileDialog1.ShowDialog() == DialogResult.OK)

            {
                filePath = openFileDialog1.FileName;

                bool bLoadImgOk = false;

                try
                {
                    imgA = new Bitmap(filePath);
                    bLoadImgOk = true;
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message,
                       "Erro ao abrir uma imagem...",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    bLoadImgOk = false;
                }

                if (bLoadImgOk)
                {
                    pbA.Image = imgA;
                }

                //Fim do click event

                //Se a imagem tiver sido carregado perfeitamente
                if (bLoadImgOk == true)
                {
                    // É adicionado a imagem da PictureBox
                    pbA.Image = imgA;
                    vImgAGray = new byte[imgA.Width, imgA.Height];
                    vImgAR = new byte[imgA.Width, imgA.Height];
                    vImgAG = new byte[imgA.Width, imgA.Height];
                    vImgAB = new byte[imgA.Width, imgA.Height];
                    vImgAAlfa = new byte[imgA.Width, imgA.Height];

                    // Percorre todos os pixels da imagem...
                    for (int i = 0; i < imgA.Width; i++)
                    {
                        for (int j = 0; j < imgA.Height; j++)
                        {
                            Color pixel = imgA.GetPixel(i, j);

                            // Para imagens em escala de cinza, será extraido o valor do pixel com:
                            // byte pixelIntensity = Convert.ToByte((pixel.R + pixel.G + pixel.B) / 3);
                            byte pixelIntensity = Convert.ToByte((pixel.R + pixel.G + pixel.B) / 3);
                            vImgAGray[i, j] = pixelIntensity;

                            // Para imagens RGB, será extraido o valor do pixel com:
                            byte R = pixel.R;
                            byte G = pixel.G;
                            byte B = pixel.B;
                        }
                    }
                }
            }
        }

        private void btLoadB_Click(object sender, EventArgs e)
        {
            // Abre a dialog box
            var filePath = string.Empty;
            openFileDialog1.InitialDirectory = "C:\\Matlab";
            openFileDialog1.Filter = "TIFF image (*.tif)|*.tif" +
            "| JPG image (*.jpg)|.jpg" +
            "| BMP image (*.bmp)|*.bmp" +
            "| PNG image (*.png)|*.png" +
            "| All image (*.*)|*.*";

            // openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            // Se o arquivo foi localizado com sucesso:
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog1.FileName;

                bool bLoadImgOk = false;

                try
                {
                    imgB = new Bitmap(filePath);
                    bLoadImgOk = true;
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message,
                       "Erro ao abrir uma imagem...",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    bLoadImgOk = false;
                }

                if (bLoadImgOk)
                {
                    pbB.Image = imgB;
                }

                //Fim do click event
                //Se a imagem for carregada perfeitamente:
                if (bLoadImgOk == true)
                {
                    // Adiciona a imagem da PictureBox
                    pbB.Image = imgB;
                    vImgBGray = new byte[imgB.Width, imgB.Height];
                    vImgBR = new byte[imgB.Width, imgB.Height];
                    vImgBG = new byte[imgB.Width, imgB.Height];
                    vImgBB = new byte[imgB.Width, imgB.Height];
                    vImgBAlfa = new byte[imgB.Width, imgB.Height];

                    // Percorre todos os pixels da imagem:
                    for (int i = 0; i < imgB.Width; i++)
                    {
                        for (int j = 0; j < imgB.Height; j++)
                        {
                            Color pixel = imgB.GetPixel(i, j);

                            // Para imagens em escala de cinza, será extraido o valor do pixel com:
                            // byte pixelIntensity = Convert.ToByte((pixel.R + pixel.G + pixel.B) / 3);
                            byte pixelIntensity = Convert.ToByte((pixel.R + pixel.G + pixel.B) / 3);
                            vImgBGray[i, j] = pixelIntensity;

                            // Para imagens RGB, será extraido o valor do pixel com:
                            byte R = pixel.R;
                            byte G = pixel.G;
                            byte B = pixel.B;
                        }
                    }
                }
            }
        }

        private void BtEscalaCinza_Click(object sender, EventArgs e)
        {
            Bitmap original = new Bitmap(pbA.Image);

            imgR = new Bitmap(imgA.Width, imgA.Height);

            vImgRGray = new byte[imgA.Width, imgA.Height];


            for (int i = 0; i < original.Width; i++)
            {
                for (int j = 0; j < original.Height; j++)
                {
                    // pega a cor original
                    Color pixel = original.GetPixel(i, j);

                    vImgRGray[i, j] = pixel.R;

                    Color cor = Color.FromArgb(
                 255,
                 vImgRGray[i, j],
                 vImgRGray[i, j],
                 vImgRGray[i, j]);

                    imgR.SetPixel(i, j, cor);
                }
            }
            pbR.Image = imgR;
        }

        private void btSoma_Click(object sender, EventArgs e)
        {
            Bitmap imagemA = new Bitmap(pbA.Image);
            Bitmap imagemB = new Bitmap(pbB.Image);

            imgA = new Bitmap(imgA.Width, imgA.Height);
            imgB = new Bitmap(imgB.Width, imgB.Height);
            imgR = new Bitmap(imgA.Width, imgA.Height);

            // Byte Resultante
            vImgRR = new byte[imgA.Width, imgA.Height];
            vImgRG = new byte[imgA.Width, imgA.Height];
            vImgRB = new byte[imgA.Width, imgA.Height];

            // Percorre A e B e extrai os pixels
            for (int i = 0; i < imagemA.Width; i++)
            {
                for (int j = 0; j < imagemA.Height; j++)
                {
                    // pega um por um 
                    Color pixelA = imagemA.GetPixel(i, j);
                    Color pixelB = imagemB.GetPixel(i, j);

                    int R = (pixelA.R + pixelB.R);
                    int G = (pixelA.G + pixelB.G);
                    int B = (pixelA.B + pixelB.B);

                    if (R > 255)
                    {
                        R = 255;
                    }

                    if (G > 255)
                    {
                        G = 255;
                    }

                    if (B > 255)
                    {
                        B = 255;
                    }

                    Color cor = Color.FromArgb(
                                     255,
                                     R,
                                     G,
                                     B);

                    imgR.SetPixel(i, j, cor);
                }
            }
            pbR.Image = imgR;
        }

        private void btSubtracao_Click(object sender, EventArgs e)
        {
            Bitmap imagemA = new Bitmap(pbA.Image);
            Bitmap imagemB = new Bitmap(pbB.Image);

            imgA = new Bitmap(imgA.Width, imgA.Height);
            imgB = new Bitmap(imgB.Width, imgB.Height);
            imgR = new Bitmap(imgA.Width, imgA.Height);

            // Byte Resultante
            vImgRR = new byte[imgA.Width, imgA.Height];
            vImgRG = new byte[imgA.Width, imgA.Height];
            vImgRB = new byte[imgA.Width, imgA.Height];

            // Percorre A e B e extrai os pixels
            for (int i = 0; i < imagemA.Width; i++)
            {
                for (int j = 0; j < imagemA.Height; j++)
                {
                    // pega um por um 
                    Color pixelA = imagemA.GetPixel(i, j);
                    Color pixelB = imagemB.GetPixel(i, j);


                    // Soma
                    int R = (pixelA.R - pixelB.R);
                    int G = (pixelA.G - pixelB.G);
                    int B = (pixelA.B - pixelB.B);

                    if (R < 0)
                    {
                        R = 0;
                    }

                    if (G < 0)
                    {
                        G = 0;
                    }

                    if (B < 0)
                    {
                        B = 0;
                    }

                    Color cor = Color.FromArgb(
                                     255,
                                     R,
                                     G,
                                     B);

                    imgR.SetPixel(i, j, cor);
                }
            }
            pbR.Image = imgR;

        }

        private void btMultiplicacao_Click(object sender, EventArgs e)
        {
            Bitmap imagemA = new Bitmap(pbA.Image);


            imgA = new Bitmap(imgA.Width, imgA.Height);
            imgR = new Bitmap(imgA.Width, imgA.Height);

            // Byte Resultante
            vImgRR = new byte[imgA.Width, imgA.Height];
            vImgRG = new byte[imgA.Width, imgA.Height];
            vImgRB = new byte[imgA.Width, imgA.Height];

            //Percorre A e B e extrai os pixels
            for (int i = 0; i < imagemA.Width; i++)
            {
                for (int j = 0; j < imagemA.Height; j++)
                {
                    // pega um por um
                    Color pixelA = imagemA.GetPixel(i, j);

                    // Multiplicação

                    double R = (pixelA.R * Convert.ToDouble(txtmult.Text));
                    double G = (pixelA.G * Convert.ToDouble(txtmult.Text));
                    double B = (pixelA.B * Convert.ToDouble(txtmult.Text));

                    if (R < 0)
                    {
                        R = 0;
                    }

                    if (G < 0)
                    {
                        G = 0;
                    }

                    if (B < 0)
                    {
                        B = 0;
                    }
                    if (R > 255)
                    {
                        R = 255;
                    }

                    if (G > 255)
                    {
                        G = 255;
                    }

                    if (B > 255)
                    {
                        B = 255;
                    }
                    Color cor = Color.FromArgb(
                                     255,
                                     Convert.ToInt32(R),
                                     Convert.ToInt32(G),
                                     Convert.ToInt32(B));

                    imgR.SetPixel(i, j, cor);
                }
            }
            pbR.Image = imgR;
        }

        private void btnDivisao_Click(object sender, EventArgs e)
        {
            Bitmap imagemA = new Bitmap(pbA.Image);

            imgA = new Bitmap(imgA.Width, imgA.Height);
            imgR = new Bitmap(imgA.Width, imgA.Height);

            // Resultante
            vImgRR = new byte[imgA.Width, imgA.Height];
            vImgRG = new byte[imgA.Width, imgA.Height];
            vImgRB = new byte[imgA.Width, imgA.Height];

            // Percorre A e B e extrai os pixels
            for (int i = 0; i < imagemA.Width; i++)
            {
                for (int j = 0; j < imagemA.Height; j++)
                {
                    Color pixelA = imagemA.GetPixel(i, j);

                    // Divisão
                    double R = (pixelA.R / Convert.ToDouble(TxtDiv.Text));
                    double G = (pixelA.G / Convert.ToDouble(TxtDiv.Text));
                    double B = (pixelA.B / Convert.ToDouble(TxtDiv.Text));

                    if (R < 0)
                    {
                        R = 0;
                    }

                    if (G < 0)
                    {
                        G = 0;
                    }

                    if (B < 0)
                    {
                        B = 0;
                    }
                    if (R > 255)
                    {
                        R = 255;
                    }

                    if (G > 255)
                    {
                        G = 255;
                    }

                    if (B > 255)
                    {
                        B = 255;
                    }
                    Color cor = Color.FromArgb(
                                     255,
                                     Convert.ToInt32(R),
                                     Convert.ToInt32(G),
                                     Convert.ToInt32(B));

                    imgR.SetPixel(i, j, cor);
                }
            }
            pbR.Image = imgR;
        }

        private void btMedia_Click(object sender, EventArgs e)
        {
            Bitmap imagemA = new Bitmap(pbA.Image);
            Bitmap imagemB = new Bitmap(pbB.Image);

            imgA = new Bitmap(imgA.Width, imgA.Height);
            imgB = new Bitmap(imgB.Width, imgB.Height);
            imgR = new Bitmap(imgA.Width, imgA.Height);

            // Byte Resultante
            vImgRR = new byte[imgA.Width, imgA.Height];
            vImgRG = new byte[imgA.Width, imgA.Height];
            vImgRB = new byte[imgA.Width, imgA.Height];

            // Percorre A e B e extrai os pixels
            for (int i = 0; i < imagemA.Width; i++)
            {
                for (int j = 0; j < imagemA.Height; j++)
                {
                    //pega um por um
                    Color pixelA = imagemA.GetPixel(i, j);
                    Color pixelB = imagemB.GetPixel(i, j);


                    //Soma
                    double R = (pixelA.R + pixelB.R) * 0.5;
                    double G = (pixelA.G + pixelB.G) * 0.5;
                    double B = (pixelA.B + pixelB.B) * 0.5;

                    if (R > 255)
                    {
                        R = 255;
                    }

                    if (G > 255)
                    {
                        G = 255;
                    }

                    if (B > 255)
                    {
                        B = 255;
                    }


                    Color cor = Color.FromArgb(
                                     255,
                                     Convert.ToInt32(R),
                                     Convert.ToInt32(G),
                                     Convert.ToInt32(B));

                    imgR.SetPixel(i, j, cor);
                }
            }
            pbR.Image = imgR;
        }

        private void btBlending_Click(object sender, EventArgs e)
        {
            Bitmap imagemA = new Bitmap(pbA.Image);
            Bitmap imagemB = new Bitmap(pbB.Image);

            imgA = new Bitmap(imgA.Width, imgA.Height);
            imgB = new Bitmap(imgB.Width, imgB.Height);
            imgR = new Bitmap(imgA.Width, imgA.Height);

            // Byte Resultante
            vImgRR = new byte[imgA.Width, imgA.Height];
            vImgRG = new byte[imgA.Width, imgA.Height];
            vImgRB = new byte[imgA.Width, imgA.Height];

            // Percorre A e B e extrai os pixels
            for (int i = 0; i < imagemA.Width; i++)
            {
                for (int j = 0; j < imagemA.Height; j++)
                {
                    //pega um por um
                    Color pixelA = imagemA.GetPixel(i, j);
                    Color pixelB = imagemB.GetPixel(i, j);

                    double R = (pixelA.R * Convert.ToDouble(txtBlendig.Text)) + (1 - Convert.ToDouble(txtBlendig.Text)) * pixelB.R;
                    double G = (pixelA.G * Convert.ToDouble(txtBlendig.Text)) + (1 - Convert.ToDouble(txtBlendig.Text)) * pixelB.R;
                    double B = (pixelA.B * Convert.ToDouble(txtBlendig.Text)) + (1 - Convert.ToDouble(txtBlendig.Text)) * pixelB.R;

                    if (R > 255)
                    {
                        R = 255;
                    }

                    if (G > 255)
                    {
                        G = 255;
                    }

                    if (B > 255)
                    {
                        B = 255;
                    }
                    if (R < 0)
                    {
                        R = 0;
                    }

                    if (G < 0)
                    {
                        G = 0;
                    }

                    if (B < 0)
                    {
                        B = 0;
                    }

                    Color cor = Color.FromArgb(
                                     255,
                                     Convert.ToInt32(R),
                                     Convert.ToInt32(G),
                                     Convert.ToInt32(B));

                    imgR.SetPixel(i, j, cor);
                }
            }
            pbR.Image = imgR;
        }

        private void btRGBpara1bit_Click(object sender, EventArgs e)
        {
            Bitmap original = new Bitmap(pbA.Image);

            imgR = new Bitmap(imgA.Width, imgA.Height);

            vImgRGray = new byte[imgA.Width, imgA.Height];

            for (int i = 0; i < original.Width; i++)
            {
                for (int j = 0; j < original.Height; j++)
                {
                    Color pixel = original.GetPixel(i, j);

                    vImgRGray[i, j] = pixel.R;

                    if (pixel.R < 128)
                    {
                        vImgRGray[i, j] = 0;
                    }
                    else
                    {
                        vImgRGray[i, j] = 255;
                    }


                    Color cor = Color.FromArgb(
                 255,
                 vImgRGray[i, j],
                 vImgRGray[i, j],
                 vImgRGray[i, j]);

                    imgR.SetPixel(i, j, cor);

                }
            }

            pbR.Image = imgR;
        }

        private void btOR_Click(object sender, EventArgs e)
        {
            Bitmap imagemA = new Bitmap(pbA.Image);
            Bitmap imagemB = new Bitmap(pbB.Image);

            imgA = new Bitmap(imgA.Width, imgA.Height);
            imgB = new Bitmap(imgB.Width, imgB.Height);
            imgR = new Bitmap(imgA.Width, imgA.Height);

            // Byte Resultante
            vImgRR = new byte[imgA.Width, imgA.Height];
            vImgRG = new byte[imgA.Width, imgA.Height];
            vImgRB = new byte[imgA.Width, imgA.Height];

            // Percorre A e B e extrai os pixels
            for (int i = 0; i < imagemA.Width; i++)
            {
                for (int j = 0; j < imagemA.Height; j++)
                {
                    //pega um por um
                    Color pixelA = imagemA.GetPixel(i, j);
                    Color pixelB = imagemB.GetPixel(i, j);


                    //OR
                    int R = (pixelA.R + pixelB.R);
                    int G = (pixelA.G + pixelB.G);
                    int B = (pixelA.B + pixelB.B);

                    if (R > 255)
                    {
                        R = 255;
                    }

                    if (G > 255)
                    {
                        G = 255;
                    }

                    if (B > 255)
                    {
                        B = 255;
                    }

                    Color cor = Color.FromArgb(
                                     255,
                                     R,
                                     G,
                                     B);

                    imgR.SetPixel(i, j, cor);
                }
            }
            pbR.Image = imgR;
        }

        private void btXOR_Click(object sender, EventArgs e)
        {
            Bitmap imagemA = new Bitmap(pbA.Image);
            Bitmap imagemB = new Bitmap(pbB.Image);

            imgA = new Bitmap(imgA.Width, imgA.Height);
            imgB = new Bitmap(imgB.Width, imgB.Height);
            imgR = new Bitmap(imgA.Width, imgA.Height);

            // Byte Resultante
            vImgRR = new byte[imgA.Width, imgA.Height];
            vImgRG = new byte[imgA.Width, imgA.Height];
            vImgRB = new byte[imgA.Width, imgA.Height];

            // Percorre A e B e extrai os pixels
            for (int i = 0; i < imagemA.Width; i++)
            {
                for (int j = 0; j < imagemA.Height; j++)
                {
                    //pega um por um
                    Color pixelA = imagemA.GetPixel(i, j);
                    Color pixelB = imagemB.GetPixel(i, j);

                    if (pixelA != pixelB)
                    {
                        int R = (pixelA.R + pixelB.R);
                        int G = (pixelA.G + pixelB.G);
                        int B = (pixelA.B + pixelB.B);

                        if (R > 255)
                        {
                            R = 255;
                        }

                        if (G > 255)
                        {
                            G = 255;
                        }

                        if (B > 255)
                        {
                            B = 255;
                        }

                        if (R == 1)
                        {
                            R = 255;
                        }

                        if (G == 1)
                        {
                            G = 255;
                        }

                        if (B == 1)
                        {
                            B = 255;
                        }

                        Color cor = Color.FromArgb(
                                         255,
                                         R,
                                         G,
                                         B);

                        imgR.SetPixel(i, j, cor);
                    }
                }

            }
            pbR.Image = imgR;
        }

        private void btAND_Click(object sender, EventArgs e)
        {
            {
                vImgAR = new byte[imgA.Width, imgA.Height];
                vImgAG = new byte[imgA.Width, imgA.Height];
                vImgAB = new byte[imgA.Width, imgA.Height];

                vImgBR = new byte[imgB.Width, imgB.Height];
                vImgBG = new byte[imgB.Width, imgB.Height];
                vImgBB = new byte[imgB.Width, imgB.Height];

                vImgRR = new byte[imgB.Width, imgB.Height];
                vImgRG = new byte[imgB.Width, imgB.Height];
                vImgRB = new byte[imgB.Width, imgB.Height];

                imgR = new Bitmap(imgB);

                // Faz o isolamento dos canais A e B
                for (int i = 0; i < imgB.Width; i++)
                {
                    for (int j = 0; j < imgB.Height; j++)
                    {
                        // Faz o isolamento dos canais da imagem A
                        Color pixel = imgA.GetPixel(i, j);
                        vImgAR[i, j] = pixel.R;
                        vImgAG[i, j] = pixel.G;
                        vImgAB[i, j] = pixel.B;

                        // Faz o isolamento dos canais da imagem B
                        Color pixel2 = imgB.GetPixel(i, j);
                        vImgBR[i, j] = pixel2.R;
                        vImgBG[i, j] = pixel2.G;
                        vImgBB[i, j] = pixel2.B;
                    }
                }

                for (int i = 0; i < imgB.Width; i++)
                {
                    for (int j = 0; j < imgB.Height; j++)
                    {

                        if (Convert.ToInt32(vImgAR[i, j]) < 128)
                        {
                            vImgAR[i, j] = Convert.ToByte(0);
                        }
                        else
                        {
                            vImgAR[i, j] = Convert.ToByte(255);
                        }

                        if (Convert.ToInt32(vImgBR[i, j]) < 128)
                        {
                            vImgBR[i, j] = Convert.ToByte(0);
                        }
                        else
                        {
                            vImgBR[i, j] = Convert.ToByte(255);
                        }

                        if (Convert.ToInt32(vImgAG[i, j]) < 128)
                        {
                            vImgAG[i, j] = Convert.ToByte(0);
                        }
                        else
                        {
                            vImgAG[i, j] = Convert.ToByte(255);
                        }

                        if (Convert.ToInt32(vImgBG[i, j]) < 128)
                        {
                            vImgBG[i, j] = Convert.ToByte(0);
                        }
                        else
                        {
                            vImgBG[i, j] = Convert.ToByte(255);
                        }


                        if (Convert.ToInt32(vImgAB[i, j]) < 128)
                        {
                            vImgAB[i, j] = Convert.ToByte(0);
                        }
                        else
                        {
                            vImgAB[i, j] = Convert.ToByte(255);
                        }

                        if (Convert.ToInt32(vImgBB[i, j]) < 128)
                        {
                            vImgBB[i, j] = Convert.ToByte(0);
                        }
                        else
                        {
                            vImgBB[i, j] = Convert.ToByte(255);
                        }
                    }
                }

                for (int i = 0; i < imgB.Width; i++)
                {
                    for (int j = 0; j < imgB.Height; j++)
                    {
                        if ((Convert.ToInt32(vImgAR[i, j]) + Convert.ToInt32(vImgBR[i, j])) > 255)
                        {

                            vImgRR[i, j] = Convert.ToByte(255);

                        }
                        else
                        {
                            vImgRR[i, j] = Convert.ToByte(0);
                        }


                        if ((Convert.ToInt32(vImgAG[i, j]) + Convert.ToInt32(vImgBG[i, j])) > 255)
                        {

                            vImgRG[i, j] = Convert.ToByte(255);

                        }
                        else
                        {
                            vImgRG[i, j] = Convert.ToByte(0);
                        }

                        if ((Convert.ToInt32(vImgAB[i, j]) + Convert.ToInt32(vImgBB[i, j])) > 255)
                        {
                            vImgRB[i, j] = Convert.ToByte(255);
                        }
                        else
                        {
                            vImgRB[i, j] = Convert.ToByte(0);
                        }
                    }
                }

                // Geração da nova imagem
                for (int i = 0; i < imgB.Width; i++)
                {
                    for (int j = 0; j < imgB.Height; j++)
                    {
                        Color cor = Color.FromArgb(
                            255,
                            vImgRR[i, j],
                            vImgRG[i, j],
                            vImgRB[i, j]);

                        imgR.SetPixel(i, j, cor);
                    }
                }

                pbR.Image = imgR;
            }
        }

        private void btRGBpara8bit_Click(object sender, EventArgs e)
        {
            Bitmap original = new Bitmap(pbA.Image);

            imgR = new Bitmap(imgA.Width, imgA.Height);

            vImgRGray = new byte[imgA.Width, imgA.Height];

            for (int i = 0; i < original.Width; i++)
            {
                for (int j = 0; j < original.Height; j++)
                {
                    //pega a cor original
                    Color pixel = original.GetPixel(i, j);

                    vImgRGray[i, j] = pixel.R;

                    Color cor = Color.FromArgb(
                 255,
                 vImgRGray[i, j],
                 vImgRGray[i, j],
                 vImgRGray[i, j]);

                    imgR.SetPixel(i, j, cor);
                }
            }

            pbR.Image = imgR;
        }

        private void btNOT_Click(object sender, EventArgs e)
        {
            Bitmap original = new Bitmap(pbA.Image);

            imgR = new Bitmap(imgA.Width, imgA.Height);

            vImgRGray = new byte[imgA.Width, imgA.Height];

            for (int i = 0; i < original.Width; i++)
            {
                for (int j = 0; j < original.Height; j++)
                {
                    //pega a cor original
                    Color pixel = original.GetPixel(i, j);

                    vImgRGray[i, j] = pixel.R;

                    if (pixel.R > 128)
                    {
                        vImgRGray[i, j] = 0;
                    }
                    else
                    {
                        vImgRGray[i, j] = 255;
                    }

                    Color cor = Color.FromArgb(
                 255,
                 vImgRGray[i, j],
                 vImgRGray[i, j],
                 vImgRGray[i, j]);

                    imgR.SetPixel(i, j, cor);

                }
            }

            pbR.Image = imgR;
        }

        //aplica pesos iguais a todos os pixels e faz a média da sua vizinhança
        //não é robusta em relação a grandes distribuições de ruído
        //causa desfocalização
        private void Flt_Media_Click(object sender, EventArgs e)
        {
            Bitmap Imagem = new Bitmap(pbA.Image);
            int Largura = Imagem.Width;
            int Altura = Imagem.Height;
            int pixelR = 0;
            int pixelG = 0;
            int pixelB = 0;
            int pixelA = 0;
            int i, j;
            int[] todosPixeisR = new int[9];
            int[] todosPixeisG = new int[9];
            int[] todosPixeisB = new int[9];
            int[] todosPixeisA = new int[9];
            Bitmap imgR = new Bitmap(Largura, Altura);
            //int aux;
            for (i = 1; i < Largura - 1; i++)
            {
                for (j = 1; j < Altura - 1; j++)
                {
                    pixelR = (
                             Imagem.GetPixel(i - 1, j - 1).R +
                             Imagem.GetPixel(i - 1, j).R +
                             Imagem.GetPixel(i - 1, j + 1).R +

                             Imagem.GetPixel(i, j - 1).R +
                             Imagem.GetPixel(i, j).R +
                             Imagem.GetPixel(i, j + 1).R +

                             Imagem.GetPixel(i + 1, j - 1).R + // Imagem.GetPixel(i + 1, j + 1).R + 
                             Imagem.GetPixel(i + 1, j).R +
                             Imagem.GetPixel(i + 1, j + 1).R
                             ) / 9;

                    pixelG = (
                             Imagem.GetPixel(i - 1, j - 1).G +
                             Imagem.GetPixel(i - 1, j).G +
                             Imagem.GetPixel(i - 1, j + 1).G +

                             Imagem.GetPixel(i, j - 1).G +
                             Imagem.GetPixel(i, j).G +
                             Imagem.GetPixel(i, j + 1).G +

                             Imagem.GetPixel(i + 1, j - 1).G + //Imagem.GetPixel(i + 1, j + 1).G + 
                             Imagem.GetPixel(i + 1, j).G +
                             Imagem.GetPixel(i + 1, j + 1).G
                             ) / 9;

                    pixelB = (
                              Imagem.GetPixel(i - 1, j - 1).B +
                              Imagem.GetPixel(i - 1, j).B +
                              Imagem.GetPixel(i - 1, j + 1).B +

                              Imagem.GetPixel(i, j - 1).B +
                              Imagem.GetPixel(i, j).B +
                              Imagem.GetPixel(i, j + 1).B +

                              Imagem.GetPixel(i + 1, j - 1).B + // Imagem.GetPixel(i + 1, j + 1).B +
                              Imagem.GetPixel(i + 1, j).B +
                              Imagem.GetPixel(i + 1, j + 1).B
                              ) / 9;

                    pixelA = (
                             Imagem.GetPixel(i - 1, j - 1).A +
                             Imagem.GetPixel(i - 1, j).A +
                             Imagem.GetPixel(i - 1, j + 1).A +

                             Imagem.GetPixel(i, j - 1).A +
                             Imagem.GetPixel(i, j).A +
                             Imagem.GetPixel(i, j + 1).A +

                             Imagem.GetPixel(i + 1, j - 1).A + //Imagem.GetPixel(i + 1, j + 1).A +
                             Imagem.GetPixel(i + 1, j).A +
                             Imagem.GetPixel(i + 1, j + 1).A
                            ) / 9;


                    imgR.SetPixel(i, j, Color.FromArgb(pixelA, pixelR, pixelG, pixelB));

                }
            }

            pbR.Image = imgR;
        }

        //o filtro gaussiana suaviva a imagem, removendo detalhes e ruídos
        private void Flt_Gaussiana_Click(object sender, EventArgs e)
        {

            try
            {
                int i = 0;
                int u = 0;
                double colorR = 0;
                double colorG = 0;
                double colorB = 0;
                int h = imgA.Width;
                int v = imgA.Height;
                imgR = new Bitmap(h, v);


                while (i < v)
                {
                    while (u < h)
                    {
                        if (u == 0 || i == 0 || u == h - 1 || i == v - 1)
                        {
                            imgR.SetPixel(u, i, imgA.GetPixel(u, i));
                        }
                        else
                        {
                            colorR = imgA.GetPixel(u - 1, i - 1).R * 0.0625 +
                                     imgA.GetPixel(u, i - 1).R * 0.125 +
                                     imgA.GetPixel(u + 1, i - 1).R * 0.0625 +
                                     imgA.GetPixel(u - 1, i).R * 0.125 +
                                     imgA.GetPixel(u, i).R * 0.25 +
                                     imgA.GetPixel(u + 1, i).R * 0.125 +
                                     imgA.GetPixel(u - 1, i + 1).R * 0.0625 +
                                     imgA.GetPixel(u, i + 1).R * 0.125 +
                                     imgA.GetPixel(u + 1, i + 1).R * 0.0625;

                            colorG = imgA.GetPixel(u - 1, i - 1).G * 0.0625 +
                                    imgA.GetPixel(u, i - 1).G * 0.125 +
                                    imgA.GetPixel(u + 1, i - 1).G * 0.0625 +
                                    imgA.GetPixel(u - 1, i).G * 0.125 +
                                    imgA.GetPixel(u, i).G * 0.25 +
                                    imgA.GetPixel(u + 1, i).G * 0.125 +
                                    imgA.GetPixel(u - 1, i + 1).G * 0.0625 +
                                    imgA.GetPixel(u, i + 1).G * 0.125 +
                                    imgA.GetPixel(u + 1, i + 1).G * 0.0625;

                            colorB = imgA.GetPixel(u - 1, i - 1).B * 0.0625 +
                                   imgA.GetPixel(u, i - 1).B * 0.125 +
                                   imgA.GetPixel(u + 1, i - 1).B * 0.0625 +
                                   imgA.GetPixel(u - 1, i).B * 0.125 +
                                   imgA.GetPixel(u, i).B * 0.25 +
                                   imgA.GetPixel(u + 1, i).B * 0.125 +
                                   imgA.GetPixel(u - 1, i + 1).B * 0.0625 +
                                   imgA.GetPixel(u, i + 1).B * 0.125 +
                                   imgA.GetPixel(u + 1, i + 1).B * 0.0625;

                            if (colorB > 255)
                                colorB = 255;
                            else if (colorB < 0)
                                colorB = 0;


                            if (colorR > 255)
                                colorR = 255;
                            else if (colorR < 0)
                                colorR = 0;

                            if (colorG > 255)
                                colorG = 255;
                            else if (colorG < 0)
                                colorG = 0;


                            imgR.SetPixel(u, i, Color.FromArgb(imgA.GetPixel(u, i).A, Convert.ToInt32(colorR), Convert.ToInt32(colorG), Convert.ToInt32(colorB)));
                        }

                        u = u + 1;

                    }
                    u = 0;
                    i = i + 1;
                }

                pbR.Image = imgR;

                //}
            }

            catch { }
        }

        private void Flt_Mediana_Click(object sender, EventArgs e)
        {
            Bitmap Imagem = new Bitmap(pbA.Image);
            int Largura = Imagem.Width;
            int Altura = Imagem.Height;
            int pixelR = 0;
            int pixelG = 0;
            int pixelB = 0;
            int pixelA = 0;
            int i, j;
            int[] todosPixeisR = new int[9];
            int[] todosPixeisG = new int[9];
            int[] todosPixeisB = new int[9];
            int[] todosPixeisA = new int[9];
            Ordenacao ordenar = new Ordenacao();
            int[] vetor = new int[9];

            Bitmap imgR = new Bitmap(Largura, Altura);


            for (i = 1; i < Largura - 1; i++)
            {
                for (j = 1; j < Altura - 1; j++)
                {
                    todosPixeisR[0] = Imagem.GetPixel(i - 1, j - 1).R;
                    todosPixeisR[1] = Imagem.GetPixel(i - 1, j).R;
                    todosPixeisR[2] = Imagem.GetPixel(i - 1, j + 1).R;

                    todosPixeisR[3] = Imagem.GetPixel(i, j - 1).R;
                    todosPixeisR[4] = Imagem.GetPixel(i, j).R;
                    todosPixeisR[5] = Imagem.GetPixel(i, j + 1).R;

                    todosPixeisR[6] = Imagem.GetPixel(i + 1, j - 1).R; // Imagem.GetPixel(i + 1, j + 1).R + 
                    todosPixeisR[7] = Imagem.GetPixel(i + 1, j).R;
                    todosPixeisR[8] = Imagem.GetPixel(i + 1, j + 1).R;

                    vetor = ordenar.BubbleSort(todosPixeisR);
                    pixelR = todosPixeisR[4];


                    todosPixeisG[0] = Imagem.GetPixel(i - 1, j - 1).G;
                    todosPixeisG[1] = Imagem.GetPixel(i - 1, j).G;
                    todosPixeisG[2] = Imagem.GetPixel(i - 1, j + 1).G;

                    todosPixeisG[3] = Imagem.GetPixel(i, j - 1).G;
                    todosPixeisG[4] = Imagem.GetPixel(i, j).G;
                    todosPixeisG[5] = Imagem.GetPixel(i, j + 1).G;

                    todosPixeisG[6] = Imagem.GetPixel(i + 1, j - 1).G; //Imagem.GetPixel(i + 1, j + 1).G + 
                    todosPixeisG[7] = Imagem.GetPixel(i + 1, j).G;
                    todosPixeisG[8] = Imagem.GetPixel(i + 1, j + 1).G;

                    vetor = ordenar.BubbleSort(todosPixeisG);
                    pixelG = todosPixeisG[4];


                    todosPixeisB[0] = Imagem.GetPixel(i - 1, j - 1).B;
                    todosPixeisB[1] = Imagem.GetPixel(i - 1, j).B;
                    todosPixeisB[2] = Imagem.GetPixel(i - 1, j + 1).B;

                    todosPixeisB[3] = Imagem.GetPixel(i, j - 1).B;
                    todosPixeisB[4] = Imagem.GetPixel(i, j).B;
                    todosPixeisB[5] = Imagem.GetPixel(i, j + 1).B;

                    todosPixeisB[6] = Imagem.GetPixel(i + 1, j - 1).B; // Imagem.GetPixel(i + 1, j + 1).B +
                    todosPixeisB[7] = Imagem.GetPixel(i + 1, j).B;
                    todosPixeisB[8] = Imagem.GetPixel(i + 1, j + 1).B;

                    vetor = ordenar.BubbleSort(todosPixeisB);
                    pixelB = todosPixeisB[4];


                    todosPixeisA[0] = Imagem.GetPixel(i - 1, j - 1).A;
                    todosPixeisA[1] = Imagem.GetPixel(i - 1, j).A;
                    todosPixeisA[2] = Imagem.GetPixel(i - 1, j + 1).A;

                    todosPixeisA[3] = Imagem.GetPixel(i, j - 1).A;
                    todosPixeisA[4] = Imagem.GetPixel(i, j).A;
                    todosPixeisA[5] = Imagem.GetPixel(i, j + 1).A;

                    todosPixeisA[6] = Imagem.GetPixel(i + 1, j - 1).A; //Imagem.GetPixel(i + 1, j + 1).A +
                    todosPixeisA[7] = Imagem.GetPixel(i + 1, j).A;
                    todosPixeisA[8] = Imagem.GetPixel(i + 1, j + 1).A;

                    vetor = ordenar.BubbleSort(todosPixeisA);
                    pixelA = todosPixeisA[4];


                    imgR.SetPixel(i, j, Color.FromArgb(pixelA, pixelR, pixelG, pixelB));
                }
            }
            pbR.Image = imgR;
        }

        private void Flt_Min_Click(object sender, EventArgs e)
        {
            Bitmap Imagem = new Bitmap(pbA.Image);
            int Largura = Imagem.Width;
            int Altura = Imagem.Height;
            int pixelR = 0;
            int pixelG = 0;
            int pixelB = 0;
            int pixelA = 0;
            int i, j;
            int[] todosPixeisR = new int[9];
            int[] todosPixeisG = new int[9];
            int[] todosPixeisB = new int[9];
            int[] todosPixeisA = new int[9];
            Ordenacao ordenar = new Ordenacao();
            int[] vetor = new int[9];

            Bitmap imgR = new Bitmap(Largura, Altura);

            for (i = 1; i < Largura - 1; i++)
            {
                for (j = 1; j < Altura - 1; j++)
                {
                    todosPixeisR[0] = Imagem.GetPixel(i - 1, j - 1).R;
                    todosPixeisR[1] = Imagem.GetPixel(i - 1, j).R;
                    todosPixeisR[2] = Imagem.GetPixel(i - 1, j + 1).R;

                    todosPixeisR[3] = Imagem.GetPixel(i, j - 1).R;
                    todosPixeisR[4] = Imagem.GetPixel(i, j).R;
                    todosPixeisR[5] = Imagem.GetPixel(i, j + 1).R;

                    todosPixeisR[6] = Imagem.GetPixel(i + 1, j - 1).R; // Imagem.GetPixel(i + 1, j + 1).R + 
                    todosPixeisR[7] = Imagem.GetPixel(i + 1, j).R;
                    todosPixeisR[8] = Imagem.GetPixel(i + 1, j + 1).R;

                    vetor = ordenar.BubbleSort(todosPixeisR);
                    pixelR = todosPixeisR[0];


                    todosPixeisG[0] = Imagem.GetPixel(i - 1, j - 1).G;
                    todosPixeisG[1] = Imagem.GetPixel(i - 1, j).G;
                    todosPixeisG[2] = Imagem.GetPixel(i - 1, j + 1).G;

                    todosPixeisG[3] = Imagem.GetPixel(i, j - 1).G;
                    todosPixeisG[4] = Imagem.GetPixel(i, j).G;
                    todosPixeisG[5] = Imagem.GetPixel(i, j + 1).G;

                    todosPixeisG[6] = Imagem.GetPixel(i + 1, j - 1).G; //Imagem.GetPixel(i + 1, j + 1).G + 
                    todosPixeisG[7] = Imagem.GetPixel(i + 1, j).G;
                    todosPixeisG[8] = Imagem.GetPixel(i + 1, j + 1).G;

                    vetor = ordenar.BubbleSort(todosPixeisG);
                    pixelG = todosPixeisG[0];


                    todosPixeisB[0] = Imagem.GetPixel(i - 1, j - 1).B;
                    todosPixeisB[1] = Imagem.GetPixel(i - 1, j).B;
                    todosPixeisB[2] = Imagem.GetPixel(i - 1, j + 1).B;

                    todosPixeisB[3] = Imagem.GetPixel(i, j - 1).B;
                    todosPixeisB[4] = Imagem.GetPixel(i, j).B;
                    todosPixeisB[5] = Imagem.GetPixel(i, j + 1).B;

                    todosPixeisB[6] = Imagem.GetPixel(i + 1, j - 1).B; // Imagem.GetPixel(i + 1, j + 1).B +
                    todosPixeisB[7] = Imagem.GetPixel(i + 1, j).B;
                    todosPixeisB[8] = Imagem.GetPixel(i + 1, j + 1).B;

                    vetor = ordenar.BubbleSort(todosPixeisB);
                    pixelB = todosPixeisB[0];


                    todosPixeisA[0] = Imagem.GetPixel(i - 1, j - 1).A;
                    todosPixeisA[1] = Imagem.GetPixel(i - 1, j).A;
                    todosPixeisA[2] = Imagem.GetPixel(i - 1, j + 1).A;

                    todosPixeisA[3] = Imagem.GetPixel(i, j - 1).A;
                    todosPixeisA[4] = Imagem.GetPixel(i, j).A;
                    todosPixeisA[5] = Imagem.GetPixel(i, j + 1).A;

                    todosPixeisA[6] = Imagem.GetPixel(i + 1, j - 1).A; //Imagem.GetPixel(i + 1, j + 1).A +
                    todosPixeisA[7] = Imagem.GetPixel(i + 1, j).A;
                    todosPixeisA[8] = Imagem.GetPixel(i + 1, j + 1).A;

                    vetor = ordenar.BubbleSort(todosPixeisA);
                    pixelA = todosPixeisA[0];


                    imgR.SetPixel(i, j, Color.FromArgb(pixelA, pixelR, pixelG, pixelB));
                }
            }
            pbR.Image = imgR;
        }

        // Filtro de Ordem: 
        private void Flt_Ordem_Click(object sender, EventArgs e)
        {
            //Declaração dos arrays 
            vImgAR = new byte[imgA.Width, imgA.Height];
            vImgAG = new byte[imgA.Width, imgA.Height];
            vImgAB = new byte[imgA.Width, imgA.Height];

            vImgRR = new byte[imgA.Width, imgA.Height];
            vImgRG = new byte[imgA.Width, imgA.Height];
            vImgRB = new byte[imgA.Width, imgA.Height];

            imgR = new Bitmap(imgA);

            // Faz o isolamento dos canais da imagem A
            for (int i = 0; i < imgA.Width; i++)
            {
                for (int j = 0; j < imgA.Height; j++)
                {
                    
                    Color pixel = imgA.GetPixel(i, j);
                    vImgAR[i, j] = pixel.R;
                    vImgAG[i, j] = pixel.G;
                    vImgAB[i, j] = pixel.B;

                }
            }

            // calcula a ordem do filtro
            for (int i = 0; i < imgA.Width; i++)
            {
                for (int j = 0; j < imgA.Height; j++)
                {
                    byte[,] temp = new byte[3, 3];

                    // canais de cores
                    if ((i > 0 && j > 0) && (i < imgA.Width - 1 && j < imgA.Height - 1))
                    {
                        temp[0, 0] = vImgAR[i - 1, j - 1];
                        temp[0, 1] = vImgAR[i - 1, j];
                        temp[0, 2] = vImgAR[i - 1, j + 1];
                        temp[1, 0] = vImgAR[i, j - 1];
                        temp[1, 1] = vImgAR[i, j];
                        temp[1, 2] = vImgAR[i, j + 1];
                        temp[2, 0] = vImgAR[i + 1, j - 1];
                        temp[2, 1] = vImgAR[i + 1, j];
                        temp[2, 2] = vImgAR[i + 1, j + 1];

                        temp[0, 0] = vImgAG[i - 1, j - 1];
                        temp[0, 1] = vImgAG[i - 1, j];
                        temp[0, 2] = vImgAG[i - 1, j + 1];
                        temp[1, 0] = vImgAG[i, j - 1];
                        temp[1, 1] = vImgAG[i, j];
                        temp[1, 2] = vImgAG[i, j + 1];
                        temp[2, 0] = vImgAG[i + 1, j - 1];
                        temp[2, 1] = vImgAG[i + 1, j];
                        temp[2, 2] = vImgAG[i + 1, j + 1];

                        temp[0, 0] = vImgAB[i - 1, j - 1];
                        temp[0, 1] = vImgAB[i - 1, j];
                        temp[0, 2] = vImgAB[i - 1, j + 1];
                        temp[1, 0] = vImgAB[i, j - 1];
                        temp[1, 1] = vImgAB[i, j];
                        temp[1, 2] = vImgAB[i, j + 1];
                        temp[2, 0] = vImgAB[i + 1, j - 1];
                        temp[2, 1] = vImgAB[i + 1, j];
                        temp[2, 2] = vImgAB[i + 1, j + 1];

                        byte[] tempArray = { temp[0, 0], temp[0, 1], temp[0, 2], temp[1, 0], temp[1, 1], temp[1, 2], temp[2, 0], temp[2, 1], temp[2, 2] };

                        // Ordena o array em ordem crescente
                        Array.Sort(tempArray);

                        // Seleciona o pixel conforme digitado
                        vImgRR[i, j] = tempArray[Convert.ToInt32(in_Ordem.Text)];
                        vImgRG[i, j] = tempArray[Convert.ToInt32(in_Ordem.Text)];
                        vImgRB[i, j] = tempArray[Convert.ToInt32(in_Ordem.Text)];

                    }

                }
            }

            // Apresenta imagem
            for (int i = 0; i < imgA.Width; i++)
            {
                for (int j = 0; j < imgA.Height; j++)
                {
                    Color cor = Color.FromArgb(
                        255,
                        vImgRR[i, j],
                        vImgRG[i, j],
                        vImgRB[i, j]);

                    imgR.SetPixel(i, j, cor);

                }
            }

            pbR.Image = imgR;

        }

        
        private void SuavizacaoCon_Click(object sender, EventArgs e)
        {
            vImgAR = new byte[imgA.Width, imgA.Height];
            vImgAG = new byte[imgA.Width, imgA.Height];
            vImgAB = new byte[imgA.Width, imgA.Height];

            vImgRR = new byte[imgA.Width, imgA.Height];
            vImgRG = new byte[imgA.Width, imgA.Height];
            vImgRB = new byte[imgA.Width, imgA.Height];

            imgR = new Bitmap(imgA);

            // Isola os canais da imagem A
            for (int i = 0; i < imgA.Width; i++)
            {
                for (int j = 0; j < imgA.Height; j++)
                {
                    Color pixel = imgA.GetPixel(i, j);
                    vImgAR[i, j] = pixel.R;
                    vImgAG[i, j] = pixel.G;
                    vImgAB[i, j] = pixel.B;
                }
            }

            // SUAVIZAÇÃO CONSERVATIVA POR CANAL (Os valores mínimo e máximo desta vizinhança são encontrados e o valor do pixel central é comparado contra esses valores limitantes.)
            for (int i = 0; i < imgA.Width; i++)
            {
                for (int j = 0; j < imgA.Height; j++)
                {
                    byte[,] temp = new byte[3, 3];

                    // Vermelho
                    if ((i > 0 && j > 0) && (i < imgA.Width - 1 && j < imgA.Height - 1))
                    {
                        temp[0, 0] = vImgAR[i - 1, j - 1];
                        temp[0, 1] = vImgAR[i - 1, j];
                        temp[0, 2] = vImgAR[i - 1, j + 1];
                        temp[1, 0] = vImgAR[i, j - 1];
                        temp[1, 1] = vImgAR[i, j];
                        temp[1, 2] = vImgAR[i, j + 1];
                        temp[2, 0] = vImgAR[i + 1, j - 1];
                        temp[2, 1] = vImgAR[i + 1, j];
                        temp[2, 2] = vImgAR[i + 1, j + 1];

                        byte[] tempArray = { temp[0, 0], temp[0, 1], temp[0, 2], temp[1, 0], temp[1, 1], temp[1, 2], temp[2, 0], temp[2, 1], temp[2, 2] };

                        byte minimo = tempArray.Min();
                        byte maximo = tempArray.Max();
                        byte final = Convert.ToByte(0);

                        // Se o valor do pixel central estiver acima do máximo, é definido como o valor máximo
                        if (temp[1, 1] > maximo)
                        {
                            final = maximo;
                        }

                        // Se for menor que o mínimo, é definido como o valor mínimo
                        if (temp[1, 1] < minimo)
                        {
                            final = minimo;
                        }

                        // Caso contrário, é deixado como está
                        if (final != maximo && final != minimo)
                        {
                            final = temp[1, 1];
                        }

                        vImgRR[i, j] = final;

                    }

                    // Verde
                    if ((i > 0 && j > 0) && (i < imgA.Width - 1 && j < imgA.Height - 1))
                    {
                        temp[0, 0] = vImgAG[i - 1, j - 1];
                        temp[0, 1] = vImgAG[i - 1, j];
                        temp[0, 2] = vImgAG[i - 1, j + 1];
                        temp[1, 0] = vImgAG[i, j - 1];
                        temp[1, 1] = vImgAG[i, j];
                        temp[1, 2] = vImgAG[i, j + 1];
                        temp[2, 0] = vImgAG[i + 1, j - 1];
                        temp[2, 1] = vImgAG[i + 1, j];
                        temp[2, 2] = vImgAG[i + 1, j + 1];

                        byte[] tempArray = { temp[0, 0], temp[0, 1], temp[0, 2], temp[1, 0], temp[1, 1], temp[1, 2], temp[2, 0], temp[2, 1], temp[2, 2] };

                        byte minimo = tempArray.Min();
                        byte maximo = tempArray.Max();
                        byte final = Convert.ToByte(0);

                        // Se o valor do pixel central estiver acima do máximo, é definido como o valor máximo
                        if (temp[1, 1] > maximo)
                        {
                            final = maximo;
                        }

                        // Se for menor que o mínimo, é definido como o valor mínimo
                        if (temp[1, 1] < minimo)
                        {
                            final = minimo;
                        }

                        // Caso contrário, é deixado como está
                        if (final != maximo && final != minimo)
                        {
                            final = temp[1, 1];
                        }

                        vImgRG[i, j] = final;

                    }

                    // Azul 
                    if ((i > 0 && j > 0) && (i < imgA.Width - 1 && j < imgA.Height - 1))
                    {
                        temp[0, 0] = vImgAB[i - 1, j - 1];
                        temp[0, 1] = vImgAB[i - 1, j];
                        temp[0, 2] = vImgAB[i - 1, j + 1];
                        temp[1, 0] = vImgAB[i, j - 1];
                        temp[1, 1] = vImgAB[i, j];
                        temp[1, 2] = vImgAB[i, j + 1];
                        temp[2, 0] = vImgAB[i + 1, j - 1];
                        temp[2, 1] = vImgAB[i + 1, j];
                        temp[2, 2] = vImgAB[i + 1, j + 1];

                        byte[] tempArray = { temp[0, 0], temp[0, 1], temp[0, 2], temp[1, 0], temp[1, 1], temp[1, 2], temp[2, 0], temp[2, 1], temp[2, 2] };

                        byte minimo = tempArray.Min();
                        byte maximo = tempArray.Max();
                        byte final = Convert.ToByte(0);

                        // Se o valor do pixel central estiver acima do máximo, é definido como o valor máximo
                        if (temp[1, 1] > maximo)
                        {
                            final = maximo;
                        }

                        // Se for menor que o mínimo, é definido como o valor mínimo
                        if (temp[1, 1] < minimo)
                        {
                            final = minimo;
                        }

                        // Caso contrário, é deixado como está
                        if (final != maximo && final != minimo)
                        {
                            final = temp[1, 1];
                        }

                        vImgRB[i, j] = final;

                    }


                }
            }


            // Gera nova imagem
            for (int i = 0; i < imgA.Width; i++)
            {
                for (int j = 0; j < imgA.Height; j++)
                {
                    Color cor = Color.FromArgb(
                        255,
                        vImgRR[i, j],
                        vImgRG[i, j],
                        vImgRB[i, j]);

                    imgR.SetPixel(i, j, cor);

                }
            }

            pbR.Image = imgR;
        }

        private void chart_P_Equalizado_Click(object sender, EventArgs e)
        {

        }

        private void Flt_Max_Click(object sender, EventArgs e)
        {
            Bitmap Imagem = new Bitmap(pbA.Image);
            int Largura = Imagem.Width;
            int Altura = Imagem.Height;
            int pixelR = 0;
            int pixelG = 0;
            int pixelB = 0;
            int pixelA = 0;
            int i, j;
            int[] todosPixeisR = new int[9];
            int[] todosPixeisG = new int[9];
            int[] todosPixeisB = new int[9];
            int[] todosPixeisA = new int[9];
            Ordenacao ordenar = new Ordenacao();
            int[] vetor = new int[9];

            Bitmap imgR = new Bitmap(Largura, Altura);


            for (i = 1; i < Largura - 1; i++)
            {
                for (j = 1; j < Altura - 1; j++)
                {
                    todosPixeisR[0] = Imagem.GetPixel(i - 1, j - 1).R;
                    todosPixeisR[1] = Imagem.GetPixel(i - 1, j).R;
                    todosPixeisR[2] = Imagem.GetPixel(i - 1, j + 1).R;

                    todosPixeisR[3] = Imagem.GetPixel(i, j - 1).R;
                    todosPixeisR[4] = Imagem.GetPixel(i, j).R;
                    todosPixeisR[5] = Imagem.GetPixel(i, j + 1).R;

                    todosPixeisR[6] = Imagem.GetPixel(i + 1, j - 1).R; // Imagem.GetPixel(i + 1, j + 1).R + 
                    todosPixeisR[7] = Imagem.GetPixel(i + 1, j).R;
                    todosPixeisR[8] = Imagem.GetPixel(i + 1, j + 1).R;

                    vetor = ordenar.BubbleSort(todosPixeisR);
                    pixelR = todosPixeisR[8];


                    todosPixeisG[0] = Imagem.GetPixel(i - 1, j - 1).G;
                    todosPixeisG[1] = Imagem.GetPixel(i - 1, j).G;
                    todosPixeisG[2] = Imagem.GetPixel(i - 1, j + 1).G;

                    todosPixeisG[3] = Imagem.GetPixel(i, j - 1).G;
                    todosPixeisG[4] = Imagem.GetPixel(i, j).G;
                    todosPixeisG[5] = Imagem.GetPixel(i, j + 1).G;

                    todosPixeisG[6] = Imagem.GetPixel(i + 1, j - 1).G; //Imagem.GetPixel(i + 1, j + 1).G + 
                    todosPixeisG[7] = Imagem.GetPixel(i + 1, j).G;
                    todosPixeisG[8] = Imagem.GetPixel(i + 1, j + 1).G;

                    vetor = ordenar.BubbleSort(todosPixeisG);
                    pixelG = todosPixeisG[8];


                    todosPixeisB[0] = Imagem.GetPixel(i - 1, j - 1).B;
                    todosPixeisB[1] = Imagem.GetPixel(i - 1, j).B;
                    todosPixeisB[2] = Imagem.GetPixel(i - 1, j + 1).B;

                    todosPixeisB[3] = Imagem.GetPixel(i, j - 1).B;
                    todosPixeisB[4] = Imagem.GetPixel(i, j).B;
                    todosPixeisB[5] = Imagem.GetPixel(i, j + 1).B;

                    todosPixeisB[6] = Imagem.GetPixel(i + 1, j - 1).B; // Imagem.GetPixel(i + 1, j + 1).B +
                    todosPixeisB[7] = Imagem.GetPixel(i + 1, j).B;
                    todosPixeisB[8] = Imagem.GetPixel(i + 1, j + 1).B;

                    vetor = ordenar.BubbleSort(todosPixeisB);
                    pixelB = todosPixeisB[8];


                    todosPixeisA[0] = Imagem.GetPixel(i - 1, j - 1).A;
                    todosPixeisA[1] = Imagem.GetPixel(i - 1, j).A;
                    todosPixeisA[2] = Imagem.GetPixel(i - 1, j + 1).A;

                    todosPixeisA[3] = Imagem.GetPixel(i, j - 1).A;
                    todosPixeisA[4] = Imagem.GetPixel(i, j).A;
                    todosPixeisA[5] = Imagem.GetPixel(i, j + 1).A;

                    todosPixeisA[6] = Imagem.GetPixel(i + 1, j - 1).A; //Imagem.GetPixel(i + 1, j + 1).A +
                    todosPixeisA[7] = Imagem.GetPixel(i + 1, j).A;
                    todosPixeisA[8] = Imagem.GetPixel(i + 1, j + 1).A;

                    vetor = ordenar.BubbleSort(todosPixeisA);
                    pixelA = todosPixeisA[8];


                    imgR.SetPixel(i, j, Color.FromArgb(pixelA, pixelR, pixelG, pixelB));
                }
            }
            pbR.Image = imgR;
        }

        private Color pixel;
        private int red, green, blue;

        private void Eq_PretoBranco_Click(object sender, EventArgs e)
        {
            imgR = equalizarImagem(imgA);
            pbR.Image = imgR;
            histogramaR = new int[256];

            calcularHistograma(imgR, "P");
            gerarGraficoHistogramaEq("P");
        }
        private void Eq_Cores_Click(object sender, EventArgs e)
        {
            imgR = equalizarImagemCores(imgA);
            pbR.Image = imgR;
            histogramaR = new int[256];
            histogramaG = new int[256];
            histogramaB = new int[256];
            histAcumuladoR = new float[256];
            histAcumuladoG = new float[256];
            histAcumuladoB = new float[256];
            calcularHistograma(imgR, "RGB");
            gerarGraficoHistogramaEq("RGB");

        }

        private void calcularHistograma(Bitmap imgA, string canal)
        {
            if (imgA != null)
            {
                canal.ToUpper();
                switch (canal)
                {
                    case "P":
                        for (int x = 0; x < imgA.Width; x++)
                        {
                            for (int y = 0; y < imgA.Height; y++)
                            {

                                pixel = imgA.GetPixel(x, y);
                                red = pixel.R;
                                histogramaR[red]++;
                            }
                        }
                        break;
                    case "RGB":
                        for (int x = 0; x < imgA.Width; x++)
                        {
                            for (int y = 0; y < imgA.Height; y++)
                            {
                                pixel = imgA.GetPixel(x, y);
                                red = pixel.R;
                                green = pixel.G;
                                blue = pixel.B;
                                histogramaR[red]++;
                                histogramaG[green]++;
                                histogramaB[blue]++;
                            }
                        }
                        break;
                }
            }
        }

        private void gerarGraficoHistograma(string canal)
        {
            canal.ToUpper();
            switch (canal)
            {
                case "P":
                    for (int i = 0; i < 256; i++)
                    {
                        chart_P_Original.Series[0].Points.AddXY(i + 1, histogramaR[i]);
                    }
                    break;
                case "RGB":
                    for (int i = 0; i < 256; i++)
                    {
                        chart_R_Original.Series[0].Points.AddXY(i + 1, histogramaR[i]);
                        chart_G_Original.Series[0].Points.AddXY(i + 1, histogramaG[i]);
                        chart_B_Original.Series[0].Points.AddXY(i + 1, histogramaB[i]);
                    }
                    break;
            }
        }
        private void gerarGraficoHistogramaEq(string canal)
        {
            canal.ToUpper();
            switch (canal)
            {
                case "P":
                    for (int i = 0; i < 256; i++)
                    {
                        chart_P_Equalizado.Series[0].Points.AddXY(i + 1, histogramaR[i]);
                    }
                    break;
                case "RGB":
                    for (int i = 0; i < 256; i++)
                    {
                        chart_R_Equalizado.Series[0].Points.AddXY(i + 1, histogramaR[i]);
                        chart_G_Equalizado.Series[0].Points.AddXY(i + 1, histogramaG[i]);
                        chart_B_Equalizado.Series[0].Points.AddXY(i + 1, histogramaB[i]);
                    }
                    break;
            }
        }
        private int maiorValor(int[] hist)
        {
            for (int i = (hist.Length - 1); i >= 0; i--)
            {
                if (hist[i] != 0)
                    return i;
            }
            return 0;
        }

        private Bitmap equalizarImagem(Bitmap imagem)
        {
            histogramaR = new int[256];
            histAcumuladoR = new float[256];

            //Calcula histograma
            calcularHistograma(imagem, "P");
            //Cria o gráfico do histograma
            gerarGraficoHistograma("P");

            //Calcula histograma acumulado
            float aux = histAcumuladoR[0];
            for (int i = 1; i < histogramaR.Length; i++)
            {
                if (histogramaR[i] != 0)
                {
                    histAcumuladoR[i] = aux + ((float)histogramaR[i] / (imagem.Width * imagem.Height));
                    aux = histAcumuladoR[i];
                }
            }

            //Calcula mapa de cores
            int[] mapaCores = new int[256];
            int maior = maiorValor(histogramaR);
            for (int i = 0; i < histogramaR.Length; i++)
                mapaCores[i] = (int)(Math.Round(histAcumuladoR[i] * maior));



            //Equalizando imagem (Constituído por um número N de vezes em que a intensidade de cor referente a um canal(R,G ou B) se repete em uma imagem)
            imgR = new Bitmap(imagem.Width, imagem.Height);
            for (int m = 0; m < imagem.Width; m++)
            {
                for (int n = 0; n < imagem.Height; n++)
                {
                    pixel = imagem.GetPixel(m, n);
                    tomR = pixel.R;
                    novoTomR = mapaCores[tomR];
                    imgR.SetPixel(m, n, Color.FromArgb(novoTomR, novoTomR, novoTomR));
                }
            }
            return imgR;
        }
        private Bitmap equalizarImagemCores(Bitmap imgA)
        {

            histogramaR = new int[256];
            histogramaG = new int[256];
            histogramaB = new int[256];
            histAcumuladoR = new float[256];
            histAcumuladoG = new float[256];
            histAcumuladoB = new float[256];

            //Calcula histograma
            calcularHistograma(imgA, "RGB");
            //Define histogramas
            gerarGraficoHistograma("RGB");

            //Calcula histograma acumulado
            histAcumuladoR[0] = (float)histogramaR[0] / (imgA.Width * imgA.Height);
            histAcumuladoG[0] = (float)histogramaG[0] / (imgA.Width * imgA.Height);
            histAcumuladoB[0] = (float)histogramaB[0] / (imgA.Width * imgA.Height);
            float auxR = histAcumuladoR[0];
            float auxG = histAcumuladoG[0];
            float auxB = histAcumuladoB[0];
            for (int i = 1; i < 256; i++)
            {
                if (histogramaR[i] != 0)
                {
                    histAcumuladoR[i] = auxR + ((float)histogramaR[i] / (imgA.Width * imgA.Height));
                    auxR = histAcumuladoR[i];
                }
                if (histogramaG[i] != 0)
                {
                    histAcumuladoG[i] = auxG + ((float)histogramaG[i] / (imgA.Width * imgA.Height));
                    auxG = histAcumuladoG[i];
                }
                if (histogramaB[i] != 0)
                {
                    histAcumuladoB[i] = auxB + ((float)histogramaB[i] / (imgA.Width * imgA.Height));
                    auxB = histAcumuladoB[i];
                }
            }

            //Calcula mapa de cores
            mapaCoresR = new int[256];
            mapaCoresG = new int[256];
            mapaCoresB = new int[256];
            int maiorR = maiorValor(histogramaR);
            int maiorG = maiorValor(histogramaG);
            int maiorB = maiorValor(histogramaB);
            for (int i = 0; i < 256; i++)
            {
                mapaCoresR[i] = (int)(Math.Round(histAcumuladoR[i] * maiorR));
                mapaCoresG[i] = (int)(Math.Round(histAcumuladoG[i] * maiorG));
                mapaCoresB[i] = (int)(Math.Round(histAcumuladoB[i] * maiorB));
            }


            //Equalizando imagem (Constituído por um número N de vezes em que a intensidade de cor referente a um canal(R,G ou B) se repete em uma imagem)
            imgR = new Bitmap(imgA.Width, imgA.Height);
            for (int m = 0; m < imgA.Width; m++)
            {
                for (int n = 0; n < imgA.Height; n++)
                {
                    pixel = imgA.GetPixel(m, n);
                    tomR = pixel.R;
                    tomG = pixel.G;
                    tomB = pixel.B;
                    novoTomR = mapaCoresR[tomR];
                    novoTomG = mapaCoresG[tomG];
                    novoTomB = mapaCoresB[tomB];
                    imgR.SetPixel(m, n, Color.FromArgb(novoTomR, novoTomG, novoTomB));
                }
            }
            return imgR;
        }

       }
 }
