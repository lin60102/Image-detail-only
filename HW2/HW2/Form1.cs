using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HW2
{
    public partial class Form1 : Form
    {
        Image orgimage,sharpimage,sobelimage,blurimage,resultimage;
        Bitmap orgbimage;

        
        public Form1()
        {
            
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            orgimage = Image.FromFile(".../images/lake.JPG");
           //
            orgbimage = (Bitmap)orgimage;
            int Height = orgbimage.Height;
            int Width = orgbimage.Width;
            int[, ,] rgbData =getRGBData(orgimage);
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int gray = (rgbData[x, y, 0] + rgbData[x, y, 1] + rgbData[x, y, 2]) / 3;
                   orgbimage.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                }
            } 
           //
         
            pictureBox1.Image =orgbimage;

            button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sharpimage=sharp(orgimage);
            pictureBox2.Image = sharpimage;
            button3.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
           sobelimage = sobel(orgimage);
           blurimage = blur(sobelimage);
           pictureBox3.Image = sobelimage;
           button4.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            resultimage = result(sharpimage, blurimage);
            pictureBox4.Image = resultimage;
            
        }
        public Image result(Image sharp,Image blur)
        {
            Bitmap sharpresult = (Bitmap)sharp;
            Bitmap blurresult = (Bitmap)blur;
            int Height = sharp.Height;
            int Width = sharp.Width;
            int[, ,] rgbDatasharp = getRGBData(sharpresult);
            int[, ,] rgbDatablur = getRGBData(blurresult);
            for (int y = 0; y < Height ; y++)
            {
                for (int x = 0; x < Width ; x++)
                {
                    int pixel = (rgbDatasharp[x, y, 0] * rgbDatablur[x, y, 0]);
                    if (pixel <= 0) { pixel = 0; }
                    else if (pixel >= 255) { pixel = 255; }
                    sharpresult.SetPixel(x, y, Color.FromArgb(pixel, pixel, pixel));
                }
            }
            return sharp;
        }
        public Image blur(Image sobel)
        {

            Bitmap blur = (Bitmap)sobel;
            int Height = blur.Height;
            int Width = blur.Width;
            int[, ,] rgbData = getRGBData(blur);
            for (int y = 1; y < Height - 1; y++)
            {
                for (int x = 1; x < Width - 1; x++)
                {
                    int pixel = ((rgbData[(x - 1), (y - 1), 0] + rgbData[x, (y - 1), 0] + rgbData[(x + 1), (y - 1), 0] + rgbData[(x - 1), y, 0] + rgbData[(x + 1), y, 0] + rgbData[x, y, 0] + rgbData[(x - 1), (y + 1), 0] + rgbData[x, (y + 1), 0] + rgbData[(x + 1), (y + 1), 0]) / 9);
                    if (pixel <= 0) { pixel = 0; }
                    else if (pixel >= 255) { pixel = 255; }
                    blur.SetPixel(x, y, Color.FromArgb(pixel, pixel, pixel));
                }
            }
            return blur;
        }
        public Image sobel(Image sharp)
        {
            Bitmap sobel = (Bitmap)sharp;
            int Height = sobel.Height;
            int Width = sobel.Width;
            int[, ,] rgbData = getRGBData(sobel);
            for (int y = 1; y < Height - 1; y++)
            {
                for (int x = 1; x < Width - 1; x++)
                {
                    int pixel = ((rgbData[(x - 1), (y + 1), 0] + (rgbData[x, (y + 1), 0] * 2) + rgbData[(x + 1), (y + 1), 0]) - (rgbData[(x - 1), (y - 1), 0] + (rgbData[x, (y - 1), 0] * 2) + rgbData[(x + 1), (y - 1), 0]));
                    if (pixel <= 0) { pixel = 0; }
                    else if (pixel >= 255) { pixel = 255; }
                    sobel.SetPixel(x, y, Color.FromArgb(pixel, pixel, pixel));
                }
            }
            for (int y = 1; y < Height - 1; y++)
            {
                for (int x = 1; x < Width - 1; x++)
                {
                    int pixel = ((rgbData[(x + 1), (y - 1), 0] + (rgbData[(x + 1), y, 0] * 2) + rgbData[(x + 1), (y - 1), 0]) - (rgbData[(x - 1), (y - 1), 0] + (rgbData[(x - 1), y, 0] * 2) + rgbData[(x - 1), (y + 1), 0]));
                    if (pixel <= 0) { pixel = 0; }
                    else if (pixel >= 255) { pixel = 255; }
                    sobel.SetPixel(x, y, Color.FromArgb(pixel, pixel, pixel));
                }
            }
            return sobel;
        }
        public  Image sharp(Image org)
        {
            Bitmap sharp = (Bitmap)org;
            int Height = sharp.Height;
            int Width = sharp.Width;
            int[, ,] rgbData = getRGBData(org);
            for (int y = 1; y < Height-1; y++)
            {
                for (int x = 1; x < Width-1; x++)
                {
                    int pixel = ((rgbData[x, y, 0] * 8) - (rgbData[(x - 1), (y - 1), 0] + rgbData[x, (y - 1), 0] + rgbData[(x + 1), (y - 1), 0] + rgbData[(x - 1), y, 0] + rgbData[(x + 1), y, 0]  + rgbData[(x - 1), (y + 1), 0] + rgbData[x, (y + 1), 0] + rgbData[(x + 1), (y + 1), 0]));
                    if (pixel <= 0) { pixel = 0; }
                    else if (pixel >= 255) { pixel = 255; }
                    sharp.SetPixel(x, y, Color.FromArgb(pixel,pixel,pixel));
                }
            } 
            return sharp;
        }
        public int[, ,] getRGBData(Image image)
        {
            // Step 1: 利用 Bitmap 將 image 包起來 
            Bitmap bimage = new Bitmap(image);
            int Height = bimage.Height;
            int Width = bimage.Width;
            int[, ,] rgbData = new int[Width, Height, 3];

            // Step 2: 取得像點顏色資訊 
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Color color = bimage.GetPixel(x, y);
                    rgbData[x, y, 0] = color.R;
                    rgbData[x, y, 1] = color.G;
                    rgbData[x, y, 2] = color.B;
                }
            }
            return rgbData;
        } 
    }
}
