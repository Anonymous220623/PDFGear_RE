// Decompiled with JetBrains decompiler
// Type: QRCoder.Base64QRCode
// Assembly: QRCoder, Version=1.4.3.0, Culture=neutral, PublicKeyToken=c4ed5b9ae8358a28
// MVID: 5D0AD632-49D4-4E68-92A0-261964209740
// Assembly location: D:\PDFGear\bin\QRCoder.dll

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

#nullable disable
namespace QRCoder;

public class Base64QRCode : AbstractQRCode, IDisposable
{
  private QRCode qr;

  public Base64QRCode() => this.qr = new QRCode();

  public Base64QRCode(QRCodeData data)
    : base(data)
  {
    this.qr = new QRCode(data);
  }

  public override void SetQRCodeData(QRCodeData data) => this.qr.SetQRCodeData(data);

  public string GetGraphic(int pixelsPerModule)
  {
    return this.GetGraphic(pixelsPerModule, Color.Black, Color.White);
  }

  public string GetGraphic(
    int pixelsPerModule,
    string darkColorHtmlHex,
    string lightColorHtmlHex,
    bool drawQuietZones = true,
    Base64QRCode.ImageType imgType = Base64QRCode.ImageType.Png)
  {
    return this.GetGraphic(pixelsPerModule, ColorTranslator.FromHtml(darkColorHtmlHex), ColorTranslator.FromHtml(lightColorHtmlHex), drawQuietZones, imgType);
  }

  public string GetGraphic(
    int pixelsPerModule,
    Color darkColor,
    Color lightColor,
    bool drawQuietZones = true,
    Base64QRCode.ImageType imgType = Base64QRCode.ImageType.Png)
  {
    string empty = string.Empty;
    using (Bitmap graphic = this.qr.GetGraphic(pixelsPerModule, darkColor, lightColor, drawQuietZones))
      return this.BitmapToBase64(graphic, imgType);
  }

  public string GetGraphic(
    int pixelsPerModule,
    Color darkColor,
    Color lightColor,
    Bitmap icon,
    int iconSizePercent = 15,
    int iconBorderWidth = 6,
    bool drawQuietZones = true,
    Base64QRCode.ImageType imgType = Base64QRCode.ImageType.Png)
  {
    string empty = string.Empty;
    using (Bitmap graphic = this.qr.GetGraphic(pixelsPerModule, darkColor, lightColor, icon, iconSizePercent, iconBorderWidth, drawQuietZones))
      return this.BitmapToBase64(graphic, imgType);
  }

  private string BitmapToBase64(Bitmap bmp, Base64QRCode.ImageType imgType)
  {
    string empty = string.Empty;
    ImageFormat format;
    switch (imgType)
    {
      case Base64QRCode.ImageType.Gif:
        format = ImageFormat.Gif;
        break;
      case Base64QRCode.ImageType.Jpeg:
        format = ImageFormat.Jpeg;
        break;
      case Base64QRCode.ImageType.Png:
        format = ImageFormat.Png;
        break;
      default:
        format = ImageFormat.Png;
        break;
    }
    using (MemoryStream memoryStream = new MemoryStream())
    {
      bmp.Save((Stream) memoryStream, format);
      return Convert.ToBase64String(memoryStream.ToArray(), Base64FormattingOptions.None);
    }
  }

  public enum ImageType
  {
    Gif,
    Jpeg,
    Png,
  }
}
