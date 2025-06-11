// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.Asn1Integer
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class Asn1Integer : Asn1
{
  private long m_value;
  private byte[] m_bytes;

  public Asn1Integer(long value)
    : base(Asn1UniversalTags.Integer)
  {
    this.m_value = value;
  }

  public Asn1Integer(byte[] value)
    : base(Asn1UniversalTags.Integer)
  {
    this.m_bytes = value;
  }

  private byte[] ToArray()
  {
    if (this.m_value >= (long) byte.MaxValue)
      return BitConverter.GetBytes(this.m_value);
    return new byte[1]{ (byte) this.m_value };
  }

  public byte[] AsnEncode() => this.Asn1Encode(this.ToArray());

  protected override bool IsEquals(Asn1 asn1Object) => throw new NotImplementedException();

  public override int GetHashCode() => throw new NotImplementedException();

  internal override void Encode(DerStream stream) => stream.WriteEncoded(2, this.m_bytes);
}
