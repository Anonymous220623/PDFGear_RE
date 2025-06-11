// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.Asn1Null
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal abstract class Asn1Null : Asn1
{
  internal Asn1Null()
    : base(Asn1UniversalTags.Null)
  {
  }

  internal static Asn1Null GetInstance() => (Asn1Null) new DerNull(0);

  private byte[] ToArray() => new byte[0];

  internal byte[] AsnEncode() => this.Asn1Encode(this.ToArray());

  public override string ToString() => "NULL";

  internal override void Encode(DerStream derOut) => derOut.WriteEncoded(5, this.ToArray());
}
