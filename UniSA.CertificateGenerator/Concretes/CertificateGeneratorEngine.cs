using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniSA.CertificateGenerator.Interfaces;

namespace UniSA.CertificateGenerator
{
    public class CertificateGeneratorEngine : IMergeImageAndText
    {
        public void ImageTextMerge(byte[] inputBytes, string fileName, string[] str, int x, int y, int w, int h, int width = 200, int height = 200)
        {
            try
            {
                TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));

                using (Image imgBack = (Bitmap)tc.ConvertFrom(inputBytes))
                {
                    using (var canvas = Graphics.FromImage(imgBack))
                    {
                        canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        canvas.DrawImage(imgBack, new Rectangle(0, 0, width, height), new Rectangle(0, 0, width, height), GraphicsUnit.Pixel);

                        // Create font and brush
                        Font drawFont = new Font("Arial", 7);
                        SolidBrush drawBrush = new SolidBrush(Color.Black);

                        // Create rectangle for drawing. 
                        RectangleF drawRect = new RectangleF(x, y, w, h);

                        // Draw rectangle to screen.
                        Pen blackPen = new Pen(Color.Transparent);
                        canvas.DrawRectangle(blackPen, x, y, w, h);

                        // Set format of string.
                        StringFormat drawFormat = new StringFormat();
                        drawFormat.Alignment = StringAlignment.Near;

                        // Draw string to screen.
                        if(str.Length > 0)
                        canvas.DrawString(str[0], drawFont, drawBrush, drawRect, drawFormat);

                        if (str.Length > 1)
                        {
                            drawRect.Location = new PointF(drawRect.Location.X,drawRect.Location.Y + 20);
                            canvas.DrawString(str[1], drawFont, drawBrush, drawRect, drawFormat);
                        }
                        if (str.Length > 2)
                        {
                            drawRect.Location = new PointF(drawRect.Location.X, drawRect.Location.Y + 20);
                            canvas.DrawString(str[2], drawFont, drawBrush, drawRect, drawFormat);
                        }
                        if (str.Length > 3)
                        {
                            drawRect.Location = new PointF(drawRect.Location.X, drawRect.Location.Y + 20);
                            canvas.DrawString(str[3], drawFont, drawBrush, drawRect, drawFormat);
                        }
                        canvas.Save();
                    }
                    imgBack.Save(fileName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
