// Decompiled with JetBrains decompiler
// Type: QRCoder.AsciiQRCodeHelper
// Assembly: QRCoder, Version=1.4.3.0, Culture=neutral, PublicKeyToken=c4ed5b9ae8358a28
// MVID: 5D0AD632-49D4-4E68-92A0-261964209740
// Assembly location: D:\PDFGear\bin\QRCoder.dll

#nullable disable
namespace QRCoder;

public static class AsciiQRCodeHelper
{
  public static string GetQRCode(
    string plainText,
    int pixelsPerModule,
    string darkColorString,
    string whiteSpaceString,
    QRCodeGenerator.ECCLevel eccLevel,
    bool forceUtf8 = false,
    bool utf8BOM = false,
    QRCodeGenerator.EciMode eciMode = QRCodeGenerator.EciMode.Default,
    int requestedVersion = -1,
    string endOfLine = "\n",
    bool drawQuietZones = true)
  {
    using (QRCodeGenerator qrCodeGenerator = new QRCodeGenerator())
    {
      using (QRCodeData qrCode = qrCodeGenerator.CreateQrCode(plainText, eccLevel, forceUtf8, utf8BOM, eciMode, requestedVersion))
      {
        using (AsciiQRCode asciiQrCode = new AsciiQRCode(qrCode))
          return asciiQrCode.GetGraphic(pixelsPerModule, darkColorString, whiteSpaceString, drawQuietZones, endOfLine);
      }
    }
  }
}
