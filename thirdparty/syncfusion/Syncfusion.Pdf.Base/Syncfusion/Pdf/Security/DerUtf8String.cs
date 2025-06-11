// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.DerUtf8String
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class DerUtf8String : DerString
{
  private string m_value;

  internal static DerUtf8String GetUtf8String(object obj)
  {
    switch (obj)
    {
      case null:
      case DerUtf8String _:
        return (DerUtf8String) obj;
      default:
        throw new ArgumentException("Invalid entry");
    }
  }

  internal static DerUtf8String GetUtf8String(Asn1Tag tag, bool isExplicit)
  {
    Asn1 asn1 = tag.GetObject();
    return isExplicit || asn1 is DerUtf8String ? DerUtf8String.GetUtf8String((object) asn1) : new DerUtf8String(Asn1Octet.GetOctetString((object) asn1).GetOctets());
  }

  internal DerUtf8String(byte[] bytes)
    : this(Encoding.UTF8.GetString(bytes, 0, bytes.Length))
  {
  }

  internal DerUtf8String(string value)
  {
    this.m_value = value != null ? value : throw new ArgumentNullException(nameof (value));
  }

  public override string GetString() => this.m_value;

  protected override bool IsEquals(Asn1 asn1)
  {
    return asn1 is DerUtf8String derUtf8String && this.m_value.Equals(derUtf8String.m_value);
  }

  internal override void Encode(DerStream stream)
  {
    stream.WriteEncoded(12, Encoding.UTF8.GetBytes(this.m_value));
  }
}
