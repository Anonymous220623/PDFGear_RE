// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.DerOctet
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class DerOctet : Asn1Octet
{
  internal DerOctet(byte[] bytes)
    : base(bytes)
  {
  }

  internal DerOctet(Syncfusion.Pdf.Security.Asn1Encode asn1)
    : base(asn1)
  {
  }

  internal override void Encode(DerStream stream) => stream.WriteEncoded(4, this.m_value);
}
