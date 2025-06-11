// Decompiled with JetBrains decompiler
// Type: QRCoder.SvgQRCode
// Assembly: QRCoder, Version=1.4.3.0, Culture=neutral, PublicKeyToken=c4ed5b9ae8358a28
// MVID: 5D0AD632-49D4-4E68-92A0-261964209740
// Assembly location: D:\PDFGear\bin\QRCoder.dll

using QRCoder.Extensions;
using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Linq;

#nullable disable
namespace QRCoder;

public class SvgQRCode : AbstractQRCode, IDisposable
{
  public SvgQRCode()
  {
  }

  public SvgQRCode(QRCodeData data)
    : base(data)
  {
  }

  public string GetGraphic(int pixelsPerModule)
  {
    return this.GetGraphic(new Size(pixelsPerModule * this.QrCodeData.ModuleMatrix.Count, pixelsPerModule * this.QrCodeData.ModuleMatrix.Count), Color.Black, Color.White);
  }

  public string GetGraphic(
    int pixelsPerModule,
    Color darkColor,
    Color lightColor,
    bool drawQuietZones = true,
    SvgQRCode.SizingMode sizingMode = SvgQRCode.SizingMode.WidthHeightAttribute,
    SvgQRCode.SvgLogo logo = null)
  {
    int num1 = drawQuietZones ? 0 : 4;
    int num2 = this.QrCodeData.ModuleMatrix.Count * pixelsPerModule - num1 * 2 * pixelsPerModule;
    return this.GetGraphic(new Size(num2, num2), darkColor, lightColor, drawQuietZones, sizingMode, logo);
  }

  public string GetGraphic(
    int pixelsPerModule,
    string darkColorHex,
    string lightColorHex,
    bool drawQuietZones = true,
    SvgQRCode.SizingMode sizingMode = SvgQRCode.SizingMode.WidthHeightAttribute,
    SvgQRCode.SvgLogo logo = null)
  {
    int num1 = drawQuietZones ? 0 : 4;
    int num2 = this.QrCodeData.ModuleMatrix.Count * pixelsPerModule - num1 * 2 * pixelsPerModule;
    return this.GetGraphic(new Size(num2, num2), darkColorHex, lightColorHex, drawQuietZones, sizingMode, logo);
  }

  public string GetGraphic(
    Size viewBox,
    bool drawQuietZones = true,
    SvgQRCode.SizingMode sizingMode = SvgQRCode.SizingMode.WidthHeightAttribute,
    SvgQRCode.SvgLogo logo = null)
  {
    return this.GetGraphic(viewBox, Color.Black, Color.White, drawQuietZones, sizingMode, logo);
  }

  public string GetGraphic(
    Size viewBox,
    Color darkColor,
    Color lightColor,
    bool drawQuietZones = true,
    SvgQRCode.SizingMode sizingMode = SvgQRCode.SizingMode.WidthHeightAttribute,
    SvgQRCode.SvgLogo logo = null)
  {
    return this.GetGraphic(viewBox, ColorTranslator.ToHtml(Color.FromArgb(darkColor.ToArgb())), ColorTranslator.ToHtml(Color.FromArgb(lightColor.ToArgb())), drawQuietZones, sizingMode, logo);
  }

  public string GetGraphic(
    Size viewBox,
    string darkColorHex,
    string lightColorHex,
    bool drawQuietZones = true,
    SvgQRCode.SizingMode sizingMode = SvgQRCode.SizingMode.WidthHeightAttribute,
    SvgQRCode.SvgLogo logo = null)
  {
    int num1 = drawQuietZones ? 0 : 4;
    int length = this.QrCodeData.ModuleMatrix.Count - (drawQuietZones ? 0 : num1 * 2);
    double pixelPerModule = (double) Math.Min(viewBox.Width, viewBox.Height) / (double) length;
    double input = (double) length * pixelPerModule;
    string str = sizingMode == SvgQRCode.SizingMode.WidthHeightAttribute ? $"width=\"{viewBox.Width}\" height=\"{viewBox.Height}\"" : $"viewBox=\"0 0 {viewBox.Width} {viewBox.Height}\"";
    SvgQRCode.ImageAttributes? attr = new SvgQRCode.ImageAttributes?();
    if (logo != null)
      attr = new SvgQRCode.ImageAttributes?(this.GetLogoAttributes(logo, viewBox));
    int[,] numArray = new int[length, length];
    for (int index1 = 0; index1 < length; ++index1)
    {
      BitArray bitArray = this.QrCodeData.ModuleMatrix[index1 + num1];
      int index2 = -1;
      int num2 = 0;
      for (int index3 = 0; index3 < length; ++index3)
      {
        numArray[index1, index3] = 0;
        if (bitArray[index3 + num1] && (logo == null || !logo.FillLogoBackground() || !this.IsBlockedByLogo((double) (index3 + num1) * pixelPerModule, (double) (index1 + num1) * pixelPerModule, attr, pixelPerModule)))
        {
          if (index2 == -1)
            index2 = index3;
          ++num2;
        }
        else if (num2 > 0)
        {
          numArray[index1, index2] = num2;
          index2 = -1;
          num2 = 0;
        }
      }
      if (num2 > 0)
        numArray[index1, index2] = num2;
    }
    StringBuilder stringBuilder = new StringBuilder($"<svg version=\"1.1\" baseProfile=\"full\" shape-rendering=\"crispEdges\" {str} xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\">");
    stringBuilder.AppendLine($"<rect x=\"0\" y=\"0\" width=\"{this.CleanSvgVal(input)}\" height=\"{this.CleanSvgVal(input)}\" fill=\"{lightColorHex}\" />");
    for (int index4 = 0; index4 < length; ++index4)
    {
      double num3 = (double) index4 * pixelPerModule;
      for (int index5 = 0; index5 < length; ++index5)
      {
        int num4 = numArray[index4, index5];
        if (num4 > 0)
        {
          int num5 = 1;
          for (int index6 = index4 + 1; index6 < length && numArray[index6, index5] == num4; ++index6)
          {
            numArray[index6, index5] = 0;
            ++num5;
          }
          double num6 = (double) index5 * pixelPerModule;
          if (logo == null || !logo.FillLogoBackground() || !this.IsBlockedByLogo(num6, num3, attr, pixelPerModule))
            stringBuilder.AppendLine($"<rect x=\"{this.CleanSvgVal(num6)}\" y=\"{this.CleanSvgVal(num3)}\" width=\"{this.CleanSvgVal((double) num4 * pixelPerModule)}\" height=\"{this.CleanSvgVal((double) num5 * pixelPerModule)}\" fill=\"{darkColorHex}\" />");
        }
      }
    }
    if (logo != null)
    {
      if (!logo.IsEmbedded())
      {
        stringBuilder.AppendLine("<svg width=\"100%\" height=\"100%\" version=\"1.1\" xmlns = \"http://www.w3.org/2000/svg\">");
        stringBuilder.AppendLine($"<image x=\"{this.CleanSvgVal(attr.Value.X)}\" y=\"{this.CleanSvgVal(attr.Value.Y)}\" width=\"{this.CleanSvgVal(attr.Value.Width)}\" height=\"{this.CleanSvgVal(attr.Value.Height)}\" xlink:href=\"{logo.GetDataUri()}\" />");
        stringBuilder.AppendLine("</svg>");
      }
      else
      {
        XDocument xdocument = XDocument.Parse((string) logo.GetRawLogo());
        xdocument.Root.SetAttributeValue((XName) "x", (object) this.CleanSvgVal(attr.Value.X));
        xdocument.Root.SetAttributeValue((XName) "y", (object) this.CleanSvgVal(attr.Value.Y));
        xdocument.Root.SetAttributeValue((XName) "width", (object) this.CleanSvgVal(attr.Value.Width));
        xdocument.Root.SetAttributeValue((XName) "height", (object) this.CleanSvgVal(attr.Value.Height));
        xdocument.Root.SetAttributeValue((XName) "shape-rendering", (object) "geometricPrecision");
        stringBuilder.AppendLine(xdocument.ToString(SaveOptions.DisableFormatting).Replace("svg:", ""));
      }
    }
    stringBuilder.Append("</svg>");
    return stringBuilder.ToString();
  }

  private bool IsBlockedByLogo(
    double x,
    double y,
    SvgQRCode.ImageAttributes? attr,
    double pixelPerModule)
  {
    return x + pixelPerModule >= attr.Value.X && x <= attr.Value.X + attr.Value.Width && y + pixelPerModule >= attr.Value.Y && y <= attr.Value.Y + attr.Value.Height;
  }

  private SvgQRCode.ImageAttributes GetLogoAttributes(SvgQRCode.SvgLogo logo, Size viewBox)
  {
    double num1 = (double) logo.GetIconSizePercent() / 100.0 * (double) viewBox.Width;
    double num2 = (double) logo.GetIconSizePercent() / 100.0 * (double) viewBox.Height;
    double num3 = (double) viewBox.Width / 2.0 - num1 / 2.0;
    double num4 = (double) viewBox.Height / 2.0 - num2 / 2.0;
    return new SvgQRCode.ImageAttributes()
    {
      Width = num1,
      Height = num2,
      X = num3,
      Y = num4
    };
  }

  private string CleanSvgVal(double input)
  {
    return input.ToString("G15", (IFormatProvider) CultureInfo.InvariantCulture);
  }

  private struct ImageAttributes
  {
    public double Width;
    public double Height;
    public double X;
    public double Y;
  }

  public enum SizingMode
  {
    WidthHeightAttribute,
    ViewBoxAttribute,
  }

  public class SvgLogo
  {
    private string _logoData;
    private SvgQRCode.SvgLogo.MediaType _mediaType;
    private int _iconSizePercent;
    private bool _fillLogoBackground;
    private object _logoRaw;
    private bool _isEmbedded;

    public SvgLogo(Bitmap iconRasterized, int iconSizePercent = 15, bool fillLogoBackground = true)
    {
      this._iconSizePercent = iconSizePercent;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (Bitmap bitmap = new Bitmap((Image) iconRasterized))
        {
          bitmap.Save((Stream) memoryStream, ImageFormat.Png);
          this._logoData = Convert.ToBase64String(memoryStream.GetBuffer(), Base64FormattingOptions.None);
        }
      }
      this._mediaType = SvgQRCode.SvgLogo.MediaType.PNG;
      this._fillLogoBackground = fillLogoBackground;
      this._logoRaw = (object) iconRasterized;
      this._isEmbedded = false;
    }

    public SvgLogo(
      string iconVectorized,
      int iconSizePercent = 15,
      bool fillLogoBackground = true,
      bool iconEmbedded = true)
    {
      this._iconSizePercent = iconSizePercent;
      this._logoData = Convert.ToBase64String(Encoding.UTF8.GetBytes(iconVectorized), Base64FormattingOptions.None);
      this._mediaType = SvgQRCode.SvgLogo.MediaType.SVG;
      this._fillLogoBackground = fillLogoBackground;
      this._logoRaw = (object) iconVectorized;
      this._isEmbedded = iconEmbedded;
    }

    public object GetRawLogo() => this._logoRaw;

    public bool IsEmbedded() => this._isEmbedded;

    public SvgQRCode.SvgLogo.MediaType GetMediaType() => this._mediaType;

    public string GetDataUri()
    {
      return $"data:{this._mediaType.GetStringValue()};base64,{this._logoData}";
    }

    public int GetIconSizePercent() => this._iconSizePercent;

    public bool FillLogoBackground() => this._fillLogoBackground;

    public enum MediaType
    {
      [StringValue("image/png")] PNG,
      [StringValue("image/svg+xml")] SVG,
    }
  }
}
