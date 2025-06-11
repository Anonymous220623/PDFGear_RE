// Decompiled with JetBrains decompiler
// Type: QRCoder.ArtQRCodeHelper
// Assembly: QRCoder, Version=1.4.3.0, Culture=neutral, PublicKeyToken=c4ed5b9ae8358a28
// MVID: 5D0AD632-49D4-4E68-92A0-261964209740
// Assembly location: D:\PDFGear\bin\QRCoder.dll

using System.Drawing;

#nullable disable
namespace QRCoder;

public static class ArtQRCodeHelper
{
  public static Bitmap GetQRCode(
    string plainText,
    int pixelsPerModule,
    Color darkColor,
    Color lightColor,
    Color backgroundColor,
    QRCodeGenerator.ECCLevel eccLevel,
    bool forceUtf8 = false,
    bool utf8BOM = false,
    QRCodeGenerator.EciMode eciMode = QRCodeGenerator.EciMode.Default,
    int requestedVersion = -1,
    Bitmap backgroundImage = null,
    double pixelSizeFactor = 0.8,
    bool drawQuietZones = true,
    ArtQRCode.QuietZoneStyle quietZoneRenderingStyle = ArtQRCode.QuietZoneStyle.Flat,
    ArtQRCode.BackgroundImageStyle backgroundImageStyle = ArtQRCode.BackgroundImageStyle.DataAreaOnly,
    Bitmap finderPatternImage = null)
  {
    using (QRCodeGenerator qrCodeGenerator = new QRCodeGenerator())
    {
      using (QRCodeData qrCode = qrCodeGenerator.CreateQrCode(plainText, eccLevel, forceUtf8, utf8BOM, eciMode, requestedVersion))
      {
        using (ArtQRCode artQrCode = new ArtQRCode(qrCode))
          return artQrCode.GetGraphic(pixelsPerModule, darkColor, lightColor, backgroundColor, backgroundImage, pixelSizeFactor, drawQuietZones, quietZoneRenderingStyle, backgroundImageStyle, finderPatternImage);
      }
    }
  }
}
