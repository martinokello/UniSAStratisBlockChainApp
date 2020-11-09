using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;

namespace UniSA.CertificateGenerator.Interfaces
{
    public interface IMergeImageAndText
    {
        void ImageTextMerge(byte[] inputBytes, string fileName, string[] str, int x, int y, int w, int h, int width = 200, int height = 200);
    }
}
