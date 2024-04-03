using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;


namespace StoryBoardApp
{
    public partial class Form1 : Form
    {
        public int WidthOfStoryBoard = 1000;

        public Bitmap[] InitializationPhotoArray()
        {
            Bitmap[] bitmapArray = new Bitmap[7];
            bitmapArray[0] = new Bitmap(@"C:\\Users\\egork\\Desktop\\StoryBoardApp\\StoryBoardApp\\images\\2021-09-03 135038.jpg");
            bitmapArray[1] = new Bitmap(@"C:\\Users\\egork\\Desktop\\StoryBoardApp\\StoryBoardApp\\images\\zyro-image-11.jpg");
            bitmapArray[2] = new Bitmap(@"C:\\Users\\egork\\Desktop\\StoryBoardApp\\StoryBoardApp\\images\\William_Albert_Allard_01.jpg");
            bitmapArray[3] = new Bitmap(@"C:\\Users\\egork\\Desktop\\StoryBoardApp\\StoryBoardApp\\images\\3468433a26750044.jpg");
            bitmapArray[4] = new Bitmap(@"C:\\Users\\egork\\Desktop\\StoryBoardApp\\StoryBoardApp\\images\\238923_1_trinixy_ru.jpg");
            bitmapArray[5] = new Bitmap(@"C:\\Users\\egork\\Desktop\\StoryBoardApp\\StoryBoardApp\\images\\0ebd262c4b7f69f7ec915dbd8509328f.jpg");
            bitmapArray[6] = new Bitmap(@"C:\\Users\\egork\\Desktop\\StoryBoardApp\\StoryBoardApp\\images\\7026.jpg");

            return bitmapArray;
        }

        //нахождение максимальной ширины
        public int FindMaxWidth(Bitmap[] bitmapArray)
        {
            int indMax = 0, max = bitmapArray[0].Width;
            for (int i = 1; i < bitmapArray.Length; i++)
            {
                if (max < bitmapArray[i].Width)
                {
                    max = bitmapArray[i].Width;
                    indMax = i;
                }
            }
            return indMax;
        }

        //подведение под 1 высоту все фото
        public Bitmap[] MakeTotalHeight(Bitmap[] bitmapArray, int indMax)
        {
            double delta;
            for (int i = 0; i < bitmapArray.Length; i++)
            {
                delta= (double)bitmapArray[indMax].Height / bitmapArray[i].Height;
                System.Drawing.Image thumbnail = bitmapArray[i].GetThumbnailImage((int)(bitmapArray[i].Width * delta), 
                    (int)(bitmapArray[i].Height * delta), () => false, IntPtr.Zero);
                bitmapArray[i] = new Bitmap(thumbnail);
            }

            return bitmapArray;
        }

        //вычисление общей ширины
        public int CountingTotalWidth(Bitmap[] bitmapArray)
        {
            int totalWidth = 0;
            for (int i = 0; i < bitmapArray.Length; i++)
            {
                totalWidth += bitmapArray[i].Width;
            }
            
            return totalWidth;
        }

        //изменение размера строки
        public Bitmap[] ChangingLineSize(Bitmap[] bitmapArray, int totalWidth)
        {
            double delta;
            delta= (double)WidthOfStoryBoard / totalWidth;
            for (int i = 0; i < bitmapArray.Length ; i++)
            {
                System.Drawing.Image thumbnail = bitmapArray[i].GetThumbnailImage((int)(bitmapArray[i].Width * delta), (int)(bitmapArray[i].Height * delta), () => false, IntPtr.Zero);
                bitmapArray[i] = new Bitmap(thumbnail);
            }

            return bitmapArray;
        }

        //вывод простой
        public void OutputWithoutChanging(Bitmap[] bitmapArray)
        {
            int width = 0;
            Bitmap combinedImage = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (Graphics g = Graphics.FromImage(combinedImage))
            {
                g.DrawImage(bitmapArray[0], new Point(0, 100));
                for (int i = 1; i < bitmapArray.Length; i++)
                {
                    width += bitmapArray[i - 1].Width;
                    g.DrawImage(bitmapArray[i], new Point(width, 100));
                }
            }
            pictureBox1.Image = combinedImage;

        }

        //вывод для строки
        public void OutputLine(Bitmap[] bitmapArray)
        {
            int width = 0;
            Bitmap combinedImage = new Bitmap(WidthOfStoryBoard, pictureBox1.Height);
            using (Graphics g = Graphics.FromImage(combinedImage))
            {
                g.DrawImage(bitmapArray[0], new Point(0, 100));
                for (int i = 1; i < bitmapArray.Length; i++)
                {
                    width += bitmapArray[i - 1].Width;
                    g.DrawImage(bitmapArray[i], new Point(width, 100));
                }   
                pictureBox1.Image = combinedImage;
            }
        }

        public int CountingWidth_odd(Bitmap[] bitmapArray)
        {
            int width = 0;
            for (int i = 1; i < bitmapArray.Length / 2 + 1; i++)
            {

                width += bitmapArray[i].Width;
            }

            return width;
        }

        public int CountingWidth(Bitmap[] bitmapArray)
        {
            int width = 0;
            for (int i = 0; i < bitmapArray.Length / 2; i++)
            {
                width += bitmapArray[i].Width;
            }
            Console.WriteLine(width);
            return width;
        }

        public Bitmap[] ChangingSize(Bitmap[] bitmapArray, int totalWidth)
        {
            double delta, totalDelta;
            int width = 0, widthHelp=0;
            for (int i = bitmapArray.Length / 2; i < bitmapArray.Length; i++)
            {
                width += bitmapArray[i].Width;
            }

            delta = (double)totalWidth/width;

            System.Drawing.Image thumbnail;
            for (int i = 0; i < bitmapArray.Length; i++)
            {
                if (i < bitmapArray.Length / 2)
                {
                    thumbnail = bitmapArray[i].GetThumbnailImage(bitmapArray[i].Width, bitmapArray[i].Height, () => false, IntPtr.Zero);
                }
                else 
                {
                    thumbnail = bitmapArray[i].GetThumbnailImage((int)(bitmapArray[i].Width * delta), (int)(bitmapArray[i].Height * delta), () => false, IntPtr.Zero);
                    widthHelp += thumbnail.Width;
                }
                bitmapArray[i] = new Bitmap(thumbnail);
            }
            
            if(width != widthHelp)
            {
                thumbnail = bitmapArray[bitmapArray.Length].GetThumbnailImage(bitmapArray[bitmapArray.Length].Width + 1, bitmapArray[bitmapArray.Length].Height, () => false, IntPtr.Zero);
                bitmapArray[bitmapArray.Length] = new Bitmap(thumbnail);
            }

            totalDelta = (double)WidthOfStoryBoard /totalWidth;
            
            for (int i = 0; i < bitmapArray.Length; i++)
            {
                System.Drawing.Image thumbnail2 = bitmapArray[i].GetThumbnailImage((int)(bitmapArray[i].Width * totalDelta), (int)(bitmapArray[i].Height * totalDelta), () => false, IntPtr.Zero);
                bitmapArray[i] = new Bitmap(thumbnail2);
            }
            return bitmapArray;
        }

        public Bitmap[] ChangingSize_odd(Bitmap[] bitmapArray, int totalWidth)
        {
            double delta, totalDelta;
            int width = 0, widthHelp = 0;
            for (int i = bitmapArray.Length / 2 + 1; i < bitmapArray.Length; i++)
            {
                width += bitmapArray[i].Width;
            }

            delta = (double)totalWidth / width;

            System.Drawing.Image thumbnail;
            for (int i = 1; i < bitmapArray.Length; i++)
            {
                if (i < bitmapArray.Length / 2 + 1)
                {
                    thumbnail = bitmapArray[i].GetThumbnailImage(bitmapArray[i].Width, bitmapArray[i].Height, () => false, IntPtr.Zero);
                }
                else
                {
                    thumbnail = bitmapArray[i].GetThumbnailImage((int)(bitmapArray[i].Width * delta), (int)(bitmapArray[i].Height * delta), () => false, IntPtr.Zero);
                    widthHelp += thumbnail.Width;
                }
                bitmapArray[i] = new Bitmap(thumbnail);
            }

            int height = bitmapArray[1].Height + bitmapArray[bitmapArray.Length / 2 + 2].Height;
            double deltaHeight = (double)height / bitmapArray[0].Height;

            thumbnail = bitmapArray[0].GetThumbnailImage((int)(bitmapArray[0].Width * deltaHeight), (int)(bitmapArray[0].Height*deltaHeight), () => false, IntPtr.Zero);
            bitmapArray[0] = new Bitmap(thumbnail);

            if (width != widthHelp)
            {
                thumbnail = bitmapArray[bitmapArray.Length - 1].GetThumbnailImage(bitmapArray[bitmapArray.Length - 1].Width + 1, bitmapArray[bitmapArray.Length - 1].Height, () => false, IntPtr.Zero);
                bitmapArray[bitmapArray.Length - 1] = new Bitmap(thumbnail);
            }

            totalDelta = (double)WidthOfStoryBoard / (bitmapArray[0].Width + totalWidth);

            for (int i = 0; i < bitmapArray.Length; i++)
            {
                System.Drawing.Image thumbnail2 = bitmapArray[i].GetThumbnailImage((int)(bitmapArray[i].Width * totalDelta), (int)(bitmapArray[i].Height * totalDelta), () => false, IntPtr.Zero);
                bitmapArray[i] = new Bitmap(thumbnail2);
            }
            return bitmapArray;
        }

        public Bitmap[] ChangingBitmapArray(Bitmap[] bitmapArray, int indOfNarrowest)
        {
            Bitmap[] bitmapArrayHelp = InitializationPhotoArray();

            System.Drawing.Image thumbnail = bitmapArray[indOfNarrowest].GetThumbnailImage(bitmapArray[indOfNarrowest].Width, bitmapArray[indOfNarrowest].Height, () => false, IntPtr.Zero);
            bitmapArrayHelp[0] = new Bitmap(thumbnail);
            for (int i = 1; i<bitmapArray.Length; i++)
            {
                if (i != indOfNarrowest && i != bitmapArray.Length)
                {
                    thumbnail = bitmapArray[i - 1].GetThumbnailImage(bitmapArray[i - 1].Width, bitmapArray[i - 1].Height, () => false, IntPtr.Zero);
                    bitmapArrayHelp[i] = new Bitmap(thumbnail);
                }
                else 
                {
                    thumbnail = bitmapArray[i - 1].GetThumbnailImage(bitmapArray[i - 1].Width, bitmapArray[i - 1].Height, () => false, IntPtr.Zero);
                    bitmapArrayHelp[i] = new Bitmap(thumbnail);
                }

            }
            for (int i = 0; i < bitmapArray.Length; i++)
            {
                bitmapArray[i] = bitmapArrayHelp[i];
            }
            return bitmapArray;
        }

        public void OutputStoryBoard(Bitmap[] bitmapArray)
        {
            int width = 0;
            Bitmap combinedImage = new Bitmap(WidthOfStoryBoard, pictureBox1.Height);
            using (Graphics g = Graphics.FromImage(combinedImage))
            {
                g.DrawImage(bitmapArray[0], new Point(0, 50));
                for (int i = 1; i < bitmapArray.Length/2; i++)
                {
                    width += bitmapArray[i - 1].Width;
                    g.DrawImage(bitmapArray[i], new Point(width, 50));
                }
                
                width = 0;
                
                g.DrawImage(bitmapArray[(int)(bitmapArray.Length/2)], new Point(0, 50 + bitmapArray[1].Height));
                for (int i = bitmapArray.Length / 2 + 1; i < bitmapArray.Length; i++)
                {
                    width += bitmapArray[i - 1].Width;
                    g.DrawImage(bitmapArray[i], new Point(width, 50 + bitmapArray[1].Height));
                }
                pictureBox1.Image = combinedImage;
            }
        }

        public void OutputStoryBoard_odd(Bitmap[] bitmapArray)
        {
            int width = 0;
            Bitmap combinedImage = new Bitmap(WidthOfStoryBoard, pictureBox1.Height);
            using (Graphics g = Graphics.FromImage(combinedImage))
            {
                g.DrawImage(bitmapArray[0], new Point(0, 50));
                for (int i = 1; i < bitmapArray.Length / 2 + 1 ; i++)
                {
                    width += bitmapArray[i - 1].Width;
                    g.DrawImage(bitmapArray[i], new Point(width, 50));
                }

                width = bitmapArray[0].Width;

                g.DrawImage(bitmapArray[(int)(bitmapArray.Length / 2 + 1)], new Point(width, 50 + bitmapArray[1].Height));
                for (int i = bitmapArray.Length / 2 + 2; i < bitmapArray.Length; i++)
                {
                    width += bitmapArray[i - 1].Width;
                    g.DrawImage(bitmapArray[i], new Point(width, 50 + bitmapArray[1].Height));
                }
                pictureBox1.Image = combinedImage;
            }
        }
        //____________________________________________________________________________________________________________________________________________________________

        public int FindTheNarrowest (Bitmap[] bitmapArray)
        {
            int max = 0, indMax=-1;
            for (int i = 0; i < bitmapArray.Length; i++)
            {
                if(max< (double)bitmapArray[i].Height / bitmapArray[i].Width) indMax = i;
            }
                return indMax;
        }
//____________________________________________________________________________________________________________________________________________________________
        public Form1()
        {
            InitializeComponent();

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Bitmap[] bitmapArray = InitializationPhotoArray();

            int totalWidth = CountingTotalWidth(bitmapArray);

            if (totalWidth == WidthOfStoryBoard)
            {
                OutputWithoutChanging(bitmapArray);
            }
            else
            {
                bitmapArray = ChangingLineSize(bitmapArray, totalWidth);
                OutputWithoutChanging(bitmapArray);
            }

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Bitmap[] bitmapArray = InitializationPhotoArray();
            int indMax = FindMaxWidth(bitmapArray);
            
            MakeTotalHeight(bitmapArray,indMax);

            int totalWidth = CountingTotalWidth(bitmapArray);

            if (totalWidth == WidthOfStoryBoard)
            {
                OutputLine(bitmapArray);
            }
            else
            {
                bitmapArray = ChangingLineSize(bitmapArray, totalWidth);
                OutputLine(bitmapArray);
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            int width = 0;
            Bitmap[] bitmapArray = InitializationPhotoArray();
            int indMax = FindMaxWidth(bitmapArray);

            MakeTotalHeight(bitmapArray, indMax);

            int totalWidth = CountingTotalWidth(bitmapArray);
            bitmapArray = ChangingLineSize(bitmapArray, totalWidth);
            
            if (bitmapArray.Length % 2 != 0)
            {
                int indOfNarrowest = FindTheNarrowest(bitmapArray);
                ChangingBitmapArray(bitmapArray, indOfNarrowest);
                width = CountingWidth_odd(bitmapArray);
                bitmapArray = ChangingSize_odd(bitmapArray, width);
                OutputStoryBoard_odd(bitmapArray);
            }
            else
            {
                width = CountingWidth(bitmapArray);
                bitmapArray = ChangingSize(bitmapArray, width);
                OutputStoryBoard(bitmapArray);
            }
            

        }
    }
}