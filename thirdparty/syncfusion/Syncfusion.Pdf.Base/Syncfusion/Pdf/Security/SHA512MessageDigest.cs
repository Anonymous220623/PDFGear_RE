// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.SHA512MessageDigest
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class SHA512MessageDigest : BigDigest
{
  private const int m_digestLength = 64 /*0x40*/;

  public override string AlgorithmName => "SHA-512";

  public override int MessageDigestSize => 64 /*0x40*/;

  public override int DoFinal(byte[] bytes, int offset)
  {
    this.Finish();
    Asn1Constants.UInt64ToBe(this.Header1, bytes, offset);
    Asn1Constants.UInt64ToBe(this.Header2, bytes, offset + 8);
    Asn1Constants.UInt64ToBe(this.Header3, bytes, offset + 16 /*0x10*/);
    Asn1Constants.UInt64ToBe(this.Header4, bytes, offset + 24);
    Asn1Constants.UInt64ToBe(this.Header5, bytes, offset + 32 /*0x20*/);
    Asn1Constants.UInt64ToBe(this.Header6, bytes, offset + 40);
    Asn1Constants.UInt64ToBe(this.Header7, bytes, offset + 48 /*0x30*/);
    Asn1Constants.UInt64ToBe(this.Header8, bytes, offset + 56);
    this.Reset();
    return 64 /*0x40*/;
  }

  public override void Reset()
  {
    base.Reset();
    this.Header1 = 7640891576956012808UL;
    this.Header2 = 13503953896175478587UL;
    this.Header3 = 4354685564936845355UL;
    this.Header4 = 11912009170470909681UL;
    this.Header5 = 5840696475078001361UL;
    this.Header6 = 11170449401992604703UL;
    this.Header7 = 2270897969802886507UL;
    this.Header8 = 6620516959819538809UL;
  }
}
