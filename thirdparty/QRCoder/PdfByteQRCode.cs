// Decompiled with JetBrains decompiler
// Type: QRCoder.PdfByteQRCode
// Assembly: QRCoder, Version=1.4.3.0, Culture=neutral, PublicKeyToken=c4ed5b9ae8358a28
// MVID: 5D0AD632-49D4-4E68-92A0-261964209740
// Assembly location: D:\PDFGear\bin\QRCoder.dll

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

#nullable disable
namespace QRCoder;

public class PdfByteQRCode : AbstractQRCode, IDisposable
{
  private readonly byte[] pdfBinaryComment = new byte[5]
  {
    (byte) 37,
    (byte) 226,
    (byte) 227,
    (byte) 207,
    (byte) 211
  };

  public PdfByteQRCode()
  {
  }

  public PdfByteQRCode(QRCodeData data)
    : base(data)
  {
  }

  public byte[] GetGraphic(int pixelsPerModule)
  {
    return this.GetGraphic(pixelsPerModule, "#000000", "#ffffff");
  }

  private byte[] HexColorToByteArray(string colorString)
  {
    if (colorString.StartsWith("#"))
      colorString = colorString.Substring(1);
    byte[] byteArray = new byte[colorString.Length / 2];
    for (int index = 0; index < byteArray.Length; ++index)
      byteArray[index] = byte.Parse(colorString.Substring(index * 2, 2), NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture);
    return byteArray;
  }

  public byte[] GetGraphic(
    int pixelsPerModule,
    string darkColorHtmlHex,
    string lightColorHtmlHex,
    int dpi = 150,
    long jpgQuality = 85)
  {
    byte[] buffer1 = (byte[]) null;
    byte[] buffer2 = (byte[]) null;
    int num1 = this.QrCodeData.ModuleMatrix.Count * pixelsPerModule;
    string str1 = (num1 * 72 / dpi).ToString((IFormatProvider) CultureInfo.InvariantCulture);
    using (PngByteQRCode pngByteQrCode = new PngByteQRCode(this.QrCodeData))
      buffer2 = pngByteQrCode.GetGraphic(pixelsPerModule, this.HexColorToByteArray(darkColorHtmlHex), this.HexColorToByteArray(lightColorHtmlHex));
    using (MemoryStream memoryStream1 = new MemoryStream())
    {
      memoryStream1.Write(buffer2, 0, buffer2.Length);
      Image image = Image.FromStream((Stream) memoryStream1);
      using (MemoryStream memoryStream2 = new MemoryStream())
      {
        ImageCodecInfo encoder = ((IEnumerable<ImageCodecInfo>) ImageCodecInfo.GetImageEncoders()).First<ImageCodecInfo>((Func<ImageCodecInfo, bool>) (x => x.MimeType == "image/jpeg"));
        EncoderParameters encoderParams = new EncoderParameters(1)
        {
          Param = new EncoderParameter[1]
          {
            new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, jpgQuality)
          }
        };
        image.Save((Stream) memoryStream2, encoder, encoderParams);
        buffer1 = memoryStream2.ToArray();
      }
    }
    using (MemoryStream memoryStream = new MemoryStream())
    {
      StreamWriter streamWriter1 = new StreamWriter((Stream) memoryStream, Encoding.GetEncoding("ASCII"));
      List<long> longList = new List<long>();
      streamWriter1.Write("%PDF-1.5\r\n");
      streamWriter1.Flush();
      memoryStream.Write(this.pdfBinaryComment, 0, this.pdfBinaryComment.Length);
      streamWriter1.WriteLine();
      streamWriter1.Flush();
      longList.Add(memoryStream.Position);
      streamWriter1.Write(longList.Count.ToString() + " 0 obj\r\n<<\r\n/Type /Catalog\r\n/Pages 2 0 R\r\n>>\r\nendobj\r\n");
      streamWriter1.Flush();
      longList.Add(memoryStream.Position);
      streamWriter1.Write($"{longList.Count.ToString()} 0 obj\r\n<<\r\n/Count 1\r\n/Kids [ <<\r\n/Type /Page\r\n/Parent 2 0 R\r\n/MediaBox [0 0 {str1} {str1}]\r\n/Resources << /ProcSet [ /PDF /ImageC ]\r\n/XObject << /Im1 4 0 R >> >>\r\n/Contents 3 0 R\r\n>> ]\r\n>>\r\nendobj\r\n");
      string str2 = $"q\r\n{str1} 0 0 {str1} 0 0 cm\r\n/Im1 Do\r\nQ";
      streamWriter1.Flush();
      longList.Add(memoryStream.Position);
      StreamWriter streamWriter2 = streamWriter1;
      string[] strArray1 = new string[6];
      int num2 = longList.Count;
      strArray1[0] = num2.ToString();
      strArray1[1] = " 0 obj\r\n<< /Length ";
      num2 = str2.Length;
      strArray1[2] = num2.ToString();
      strArray1[3] = " >>\r\nstream\r\n";
      strArray1[4] = str2;
      strArray1[5] = "endstream\r\nendobj\r\n";
      string str3 = string.Concat(strArray1);
      streamWriter2.Write(str3);
      streamWriter1.Flush();
      longList.Add(memoryStream.Position);
      StreamWriter streamWriter3 = streamWriter1;
      string[] strArray2 = new string[6];
      num2 = longList.Count;
      strArray2[0] = num2.ToString();
      strArray2[1] = " 0 obj\r\n<<\r\n/Name /Im1\r\n/Type /XObject\r\n/Subtype /Image\r\n/Width ";
      strArray2[2] = num1.ToString();
      strArray2[3] = "/Height ";
      strArray2[4] = num1.ToString();
      strArray2[5] = "/Length 5 0 R\r\n/Filter /DCTDecode\r\n/ColorSpace /DeviceRGB\r\n/BitsPerComponent 8\r\n>>\r\nstream\r\n";
      string str4 = string.Concat(strArray2);
      streamWriter3.Write(str4);
      streamWriter1.Flush();
      memoryStream.Write(buffer1, 0, buffer1.Length);
      streamWriter1.Write("\r\nendstream\r\nendobj\r\n");
      streamWriter1.Flush();
      longList.Add(memoryStream.Position);
      StreamWriter streamWriter4 = streamWriter1;
      num2 = longList.Count;
      string str5 = num2.ToString();
      num2 = buffer1.Length;
      string str6 = num2.ToString();
      string str7 = $"{str5} 0 obj\r\n{str6} endobj\r\n";
      streamWriter4.Write(str7);
      streamWriter1.Flush();
      long position = memoryStream.Position;
      StreamWriter streamWriter5 = streamWriter1;
      num2 = longList.Count + 1;
      string str8 = $"xref\r\n0 {num2.ToString()}\r\n0000000000 65535 f\r\n";
      streamWriter5.Write(str8);
      foreach (long num3 in longList)
        streamWriter1.Write(num3.ToString("0000000000") + " 00000 n\r\n");
      streamWriter1.Write($"trailer\r\n<<\r\n/Size {(longList.Count + 1).ToString()}\r\n/Root 1 0 R\r\n>>\r\nstartxref\r\n{position.ToString()}\r\n%%EOF");
      streamWriter1.Flush();
      memoryStream.Position = 0L;
      return memoryStream.ToArray();
    }
  }
}
