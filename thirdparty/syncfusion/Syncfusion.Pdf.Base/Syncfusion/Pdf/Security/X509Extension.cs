// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.X509Extension
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class X509Extension
{
  internal bool m_critical;
  internal Asn1Octet m_value;

  internal X509Extension(bool critical, Asn1Octet value)
  {
    this.m_critical = critical;
    this.m_value = value;
  }

  internal bool IsCritical => this.m_critical;

  internal Asn1Octet Value => this.m_value;

  internal Asn1Encode GetParsedValue() => (Asn1Encode) X509Extension.ConvertValueToObject(this);

  public override int GetHashCode()
  {
    int asn1Hash = this.Value.GetAsn1Hash();
    return !this.IsCritical ? ~asn1Hash : asn1Hash;
  }

  public override bool Equals(object obj)
  {
    return obj is X509Extension x509Extension && this.Value.Equals((object) x509Extension.Value) && this.IsCritical == x509Extension.IsCritical;
  }

  public static Asn1 ConvertValueToObject(X509Extension ext)
  {
    try
    {
      return Asn1.FromByteArray(ext.Value.GetOctets());
    }
    catch (Exception ex)
    {
      throw new ArgumentException("can't convert extension", ex);
    }
  }
}
