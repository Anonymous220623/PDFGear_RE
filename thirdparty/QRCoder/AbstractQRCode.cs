// Decompiled with JetBrains decompiler
// Type: QRCoder.AbstractQRCode
// Assembly: QRCoder, Version=1.4.3.0, Culture=neutral, PublicKeyToken=c4ed5b9ae8358a28
// MVID: 5D0AD632-49D4-4E68-92A0-261964209740
// Assembly location: D:\PDFGear\bin\QRCoder.dll

#nullable disable
namespace QRCoder;

public abstract class AbstractQRCode
{
  protected QRCodeData QrCodeData { get; set; }

  protected AbstractQRCode()
  {
  }

  protected AbstractQRCode(QRCodeData data) => this.QrCodeData = data;

  public virtual void SetQRCodeData(QRCodeData data) => this.QrCodeData = data;

  public void Dispose()
  {
    this.QrCodeData?.Dispose();
    this.QrCodeData = (QRCodeData) null;
  }
}
