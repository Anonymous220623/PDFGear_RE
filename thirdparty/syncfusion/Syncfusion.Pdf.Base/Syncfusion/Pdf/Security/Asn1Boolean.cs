// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.Asn1Boolean
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class Asn1Boolean : Asn1
{
  private bool m_value;

  public Asn1Boolean(bool value)
    : base(Asn1UniversalTags.Boolean)
  {
    this.m_value = value;
  }

  public Asn1Boolean(byte[] bytes)
    : base(Asn1UniversalTags.Boolean)
  {
    this.m_value = bytes[0] == byte.MaxValue;
  }

  private byte[] ToArray()
  {
    return new byte[1]
    {
      this.m_value ? byte.MaxValue : (byte) 0
    };
  }

  public byte[] AsnEncode() => this.Asn1Encode(this.ToArray());

  protected override bool IsEquals(Asn1 asn1Object) => throw new NotImplementedException();

  public override int GetHashCode() => throw new NotImplementedException();

  internal override void Encode(DerStream derOut) => derOut.WriteEncoded(1, this.ToArray());
}
