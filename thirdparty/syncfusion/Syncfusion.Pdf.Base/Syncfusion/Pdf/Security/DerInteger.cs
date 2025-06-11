// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.DerInteger
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class DerInteger : Asn1
{
  internal byte[] m_value;

  internal Number Value => new Number(this.m_value);

  internal Number PositiveValue => new Number(1, this.m_value);

  internal DerInteger(int value) => this.m_value = Number.ValueOf((long) value).ToByteArray();

  internal DerInteger(Number value)
  {
    this.m_value = value != null ? value.ToByteArray() : throw new ArgumentNullException(nameof (value));
  }

  internal DerInteger(byte[] bytes) => this.m_value = bytes;

  internal override void Encode(DerStream stream) => stream.WriteEncoded(2, this.m_value);

  public override int GetHashCode() => Asn1Constants.GetHashCode(this.m_value);

  protected override bool IsEquals(Asn1 asn1)
  {
    return asn1 is DerInteger derInteger && Asn1Constants.AreEqual(this.m_value, derInteger.m_value);
  }

  public override string ToString() => this.Value.ToString();

  internal static DerInteger GetNumber(object obj)
  {
    switch (obj)
    {
      case null:
      case DerInteger _:
        return (DerInteger) obj;
      default:
        throw new ArgumentException("Invalid entry");
    }
  }

  internal static DerInteger GetNumber(Asn1Tag tag, bool isExplicit)
  {
    Asn1 asn1 = tag != null ? tag.GetObject() : throw new ArgumentNullException(nameof (tag));
    return isExplicit || asn1 is DerInteger ? DerInteger.GetNumber((object) asn1) : new DerInteger(Asn1Octet.GetOctetString((object) asn1).GetOctets());
  }
}
