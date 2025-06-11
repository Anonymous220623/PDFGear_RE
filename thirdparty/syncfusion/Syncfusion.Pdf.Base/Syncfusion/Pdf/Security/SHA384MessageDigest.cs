// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.SHA384MessageDigest
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class SHA384MessageDigest : BigDigest
{
  private const int DigestLength = 48 /*0x30*/;

  public override string AlgorithmName => "SHA-384";

  public override int MessageDigestSize => 48 /*0x30*/;

  public override int DoFinal(byte[] bytes, int offset)
  {
    this.Finish();
    Asn1Constants.UInt64ToBe(this.Header1, bytes, offset);
    Asn1Constants.UInt64ToBe(this.Header2, bytes, offset + 8);
    Asn1Constants.UInt64ToBe(this.Header3, bytes, offset + 16 /*0x10*/);
    Asn1Constants.UInt64ToBe(this.Header4, bytes, offset + 24);
    Asn1Constants.UInt64ToBe(this.Header5, bytes, offset + 32 /*0x20*/);
    Asn1Constants.UInt64ToBe(this.Header6, bytes, offset + 40);
    this.Reset();
    return 48 /*0x30*/;
  }

  public override void Reset()
  {
    base.Reset();
    this.Header1 = 14680500436340154072UL;
    this.Header2 = 7105036623409894663UL;
    this.Header3 = 10473403895298186519UL;
    this.Header4 = 1526699215303891257UL;
    this.Header5 = 7436329637833083697UL;
    this.Header6 = 10282925794625328401UL;
    this.Header7 = 15784041429090275239UL;
    this.Header8 = 5167115440072839076UL;
  }
}
