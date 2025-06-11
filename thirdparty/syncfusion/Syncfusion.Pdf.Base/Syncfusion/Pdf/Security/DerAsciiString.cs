// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.DerAsciiString
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class DerAsciiString : DerString
{
  private string m_value;

  internal DerAsciiString(byte[] bytes)
    : this(Encoding.ASCII.GetString(bytes, 0, bytes.Length), false)
  {
  }

  internal DerAsciiString(string value)
    : this(value, false)
  {
  }

  internal DerAsciiString(string value, bool isValid)
  {
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    this.m_value = !isValid || DerAsciiString.IsAsciiString(value) ? value : throw new ArgumentException("Invalid characters found");
  }

  public override string GetString() => this.m_value;

  internal byte[] AsnEncode() => this.Asn1Encode(this.GetOctets());

  internal byte[] GetOctets() => Encoding.UTF8.GetBytes(this.m_value);

  internal override void Encode(DerStream stream) => stream.WriteEncoded(22, this.GetOctets());

  public override int GetHashCode() => this.m_value.GetHashCode();

  protected override bool IsEquals(Asn1 asn1)
  {
    return asn1 is DerAsciiString derAsciiString && this.m_value.Equals(derAsciiString.m_value);
  }

  internal static DerAsciiString GetAsciiString(object obj)
  {
    switch (obj)
    {
      case null:
      case DerAsciiString _:
        return (DerAsciiString) obj;
      default:
        throw new ArgumentException("Invalid entry");
    }
  }

  internal static DerAsciiString GetAsciiString(Asn1Tag tag, bool isExplicit)
  {
    Asn1 asn1 = tag.GetObject();
    return isExplicit || asn1 is DerAsciiString ? DerAsciiString.GetAsciiString((object) asn1) : new DerAsciiString(((Asn1Octet) asn1).GetOctets());
  }

  internal static bool IsAsciiString(string value)
  {
    foreach (char ch in value)
    {
      if (ch > '\u007F')
        return false;
    }
    return true;
  }
}
