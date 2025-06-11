// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.DerNull
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class DerNull : Asn1Null
{
  internal static readonly DerNull Value = new DerNull(0);
  private byte[] m_bytes = new byte[0];

  internal DerNull(int value)
  {
  }

  internal override void Encode(DerStream stream) => stream.WriteEncoded(5, this.m_bytes);

  protected override bool IsEquals(Asn1 asn1) => asn1 is DerNull;

  public override int GetHashCode() => -1;
}
