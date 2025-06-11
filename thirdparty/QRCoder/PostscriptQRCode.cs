// Decompiled with JetBrains decompiler
// Type: QRCoder.PostscriptQRCode
// Assembly: QRCoder, Version=1.4.3.0, Culture=neutral, PublicKeyToken=c4ed5b9ae8358a28
// MVID: 5D0AD632-49D4-4E68-92A0-261964209740
// Assembly location: D:\PDFGear\bin\QRCoder.dll

using System;
using System.Drawing;
using System.Globalization;

#nullable disable
namespace QRCoder;

public class PostscriptQRCode : AbstractQRCode, IDisposable
{
  private const string psHeader = "%!PS-Adobe-3.0 {3}\r\n%%Creator: QRCoder.NET\r\n%%Title: QRCode\r\n%%CreationDate: {0}\r\n%%DocumentData: Clean7Bit\r\n%%Origin: 0\r\n%%DocumentMedia: Default {1} {1} 0 () ()\r\n%%BoundingBox: 0 0 {1} {1}\r\n%%LanguageLevel: 2 \r\n%%Pages: 1\r\n%%Page: 1 1\r\n%%EndComments\r\n%%BeginConstants\r\n/sz {1} def\r\n/sc {2} def\r\n%%EndConstants\r\n%%BeginFeature: *PageSize Default\r\n<< /PageSize [ sz sz ] /ImagingBBox null >> setpagedevice\r\n%%EndFeature\r\n";
  private const string psFunctions = "%%BeginFunctions \r\n/csquare {{\r\n    newpath\r\n    0 0 moveto\r\n    0 1 rlineto\r\n    1 0 rlineto\r\n    0 -1 rlineto\r\n    closepath\r\n    setrgbcolor\r\n    fill\r\n}} def\r\n/f {{ \r\n    {0} {1} {2} csquare\r\n    1 0 translate\r\n}} def\r\n/b {{ \r\n    1 0 translate\r\n}} def \r\n/background {{ \r\n    {3} {4} {5} csquare \r\n}} def\r\n/nl {{\r\n    -{6} -1 translate\r\n}} def\r\n%%EndFunctions\r\n%%BeginBody\r\n0 0 moveto\r\ngsave\r\nsz sz scale\r\nbackground\r\ngrestore\r\ngsave\r\nsc sc scale\r\n0 {6} 1 sub translate\r\n";
  private const string psFooter = "%%EndBody\r\ngrestore\r\nshowpage   \r\n%%EOF\r\n";

  public PostscriptQRCode()
  {
  }

  public PostscriptQRCode(QRCodeData data)
    : base(data)
  {
  }

  public string GetGraphic(int pointsPerModule, bool epsFormat = false)
  {
    return this.GetGraphic(new Size(pointsPerModule * this.QrCodeData.ModuleMatrix.Count, pointsPerModule * this.QrCodeData.ModuleMatrix.Count), Color.Black, Color.White, epsFormat: epsFormat);
  }

  public string GetGraphic(
    int pointsPerModule,
    Color darkColor,
    Color lightColor,
    bool drawQuietZones = true,
    bool epsFormat = false)
  {
    return this.GetGraphic(new Size(pointsPerModule * this.QrCodeData.ModuleMatrix.Count, pointsPerModule * this.QrCodeData.ModuleMatrix.Count), darkColor, lightColor, drawQuietZones, epsFormat);
  }

  public string GetGraphic(
    int pointsPerModule,
    string darkColorHex,
    string lightColorHex,
    bool drawQuietZones = true,
    bool epsFormat = false)
  {
    return this.GetGraphic(new Size(pointsPerModule * this.QrCodeData.ModuleMatrix.Count, pointsPerModule * this.QrCodeData.ModuleMatrix.Count), darkColorHex, lightColorHex, drawQuietZones, epsFormat);
  }

  public string GetGraphic(Size viewBox, bool drawQuietZones = true, bool epsFormat = false)
  {
    return this.GetGraphic(viewBox, Color.Black, Color.White, drawQuietZones, epsFormat);
  }

  public string GetGraphic(
    Size viewBox,
    string darkColorHex,
    string lightColorHex,
    bool drawQuietZones = true,
    bool epsFormat = false)
  {
    return this.GetGraphic(viewBox, ColorTranslator.FromHtml(darkColorHex), ColorTranslator.FromHtml(lightColorHex), drawQuietZones, epsFormat);
  }

  public string GetGraphic(
    Size viewBox,
    Color darkColor,
    Color lightColor,
    bool drawQuietZones = true,
    bool epsFormat = false)
  {
    int num1 = drawQuietZones ? 0 : 4;
    int num2 = this.QrCodeData.ModuleMatrix.Count - (drawQuietZones ? 0 : num1 * 2);
    double input = (double) Math.Min(viewBox.Width, viewBox.Height) / (double) num2;
    string str = string.Format("%!PS-Adobe-3.0 {3}\r\n%%Creator: QRCoder.NET\r\n%%Title: QRCode\r\n%%CreationDate: {0}\r\n%%DocumentData: Clean7Bit\r\n%%Origin: 0\r\n%%DocumentMedia: Default {1} {1} 0 () ()\r\n%%BoundingBox: 0 0 {1} {1}\r\n%%LanguageLevel: 2 \r\n%%Pages: 1\r\n%%Page: 1 1\r\n%%EndComments\r\n%%BeginConstants\r\n/sz {1} def\r\n/sc {2} def\r\n%%EndConstants\r\n%%BeginFeature: *PageSize Default\r\n<< /PageSize [ sz sz ] /ImagingBBox null >> setpagedevice\r\n%%EndFeature\r\n", (object) DateTime.Now.ToString("s"), (object) this.CleanSvgVal((double) viewBox.Width), (object) this.CleanSvgVal(input), epsFormat ? (object) "EPSF-3.0" : (object) string.Empty) + string.Format("%%BeginFunctions \r\n/csquare {{\r\n    newpath\r\n    0 0 moveto\r\n    0 1 rlineto\r\n    1 0 rlineto\r\n    0 -1 rlineto\r\n    closepath\r\n    setrgbcolor\r\n    fill\r\n}} def\r\n/f {{ \r\n    {0} {1} {2} csquare\r\n    1 0 translate\r\n}} def\r\n/b {{ \r\n    1 0 translate\r\n}} def \r\n/background {{ \r\n    {3} {4} {5} csquare \r\n}} def\r\n/nl {{\r\n    -{6} -1 translate\r\n}} def\r\n%%EndFunctions\r\n%%BeginBody\r\n0 0 moveto\r\ngsave\r\nsz sz scale\r\nbackground\r\ngrestore\r\ngsave\r\nsc sc scale\r\n0 {6} 1 sub translate\r\n", (object) this.CleanSvgVal((double) darkColor.R / (double) byte.MaxValue), (object) this.CleanSvgVal((double) darkColor.G / (double) byte.MaxValue), (object) this.CleanSvgVal((double) darkColor.B / (double) byte.MaxValue), (object) this.CleanSvgVal((double) lightColor.R / (double) byte.MaxValue), (object) this.CleanSvgVal((double) lightColor.G / (double) byte.MaxValue), (object) this.CleanSvgVal((double) lightColor.B / (double) byte.MaxValue), (object) num2);
    for (int index1 = num1; index1 < num1 + num2; ++index1)
    {
      if (index1 > num1)
        str += "nl\n";
      for (int index2 = num1; index2 < num1 + num2; ++index2)
        str += this.QrCodeData.ModuleMatrix[index1][index2] ? "f " : "b ";
      str += "\n";
    }
    return str + "%%EndBody\r\ngrestore\r\nshowpage   \r\n%%EOF\r\n";
  }

  private string CleanSvgVal(double input)
  {
    return input.ToString((IFormatProvider) CultureInfo.InvariantCulture);
  }
}
