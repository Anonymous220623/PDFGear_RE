// Decompiled with JetBrains decompiler
// Type: QRCoder.QRCodeHelper
// Assembly: QRCoder, Version=1.4.3.0, Culture=neutral, PublicKeyToken=c4ed5b9ae8358a28
// MVID: 5D0AD632-49D4-4E68-92A0-261964209740
// Assembly location: D:\PDFGear\bin\QRCoder.dll

using System.Drawing;

#nullable disable
namespace QRCoder;

public static class QRCodeHelper
{
  public static Bitmap GetQRCode(
    string plainText,
    int pixelsPerModule,
    Color darkColor,
    Color lightColor,
    QRCodeGenerator.ECCLevel eccLevel,
    bool forceUtf8 = false,
    bool utf8BOM = false,
    QRCodeGenerator.EciMode eciMode = QRCodeGenerator.EciMode.Default,
    int requestedVersion = -1,
    Bitmap icon = null,
    int iconSizePercent = 15,
    int iconBorderWidth = 0,
    bool drawQuietZones = true)
  {
    using (QRCodeGenerator qrCodeGenerator = new QRCodeGenerator())
    {
      using (QRCodeData qrCode1 = qrCodeGenerator.CreateQrCode(plainText, eccLevel, forceUtf8, utf8BOM, eciMode, requestedVersion))
      {
        using (QRCode qrCode2 = new QRCode(qrCode1))
          return qrCode2.GetGraphic(pixelsPerModule, darkColor, lightColor, icon, iconSizePercent, iconBorderWidth, drawQuietZones);
      }
    }
  }
}
