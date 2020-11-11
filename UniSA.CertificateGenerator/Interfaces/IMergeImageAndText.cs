using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using static UniSA.CertificateGenerator.CertificateGeneratorEngine;

namespace UniSA.CertificateGenerator.Interfaces
{
    public interface IMergeImageAndText
    {
        void ImageTextMerge(byte[] inputBytes, string fileName, string[] str, Color textColor, int x, int y, int w, int h, int width = 200, int height = 200);
    }
}
