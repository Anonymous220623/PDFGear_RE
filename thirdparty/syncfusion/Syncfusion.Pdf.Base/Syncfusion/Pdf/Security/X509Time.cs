// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.X509Time
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class X509Time : Asn1Encode
{
  internal Asn1 m_time;

  internal X509Time(Asn1 time) => this.m_time = time;

  internal static X509Time GetTime(object obj)
  {
    switch (obj)
    {
      case null:
      case X509Time _:
        return (X509Time) obj;
      case DerUtcTime _:
        return new X509Time((Asn1) obj);
      case GeneralizedTime _:
        return new X509Time((Asn1) obj);
      default:
        throw new ArgumentException("Invalid entry");
    }
  }

  internal string GetTime()
  {
    if (this.m_time is DerUtcTime)
      return ((DerUtcTime) this.m_time).AdjustedTimeString;
    return this.m_time is GeneralizedTime ? ((GeneralizedTime) this.m_time).TimeString : (string) null;
  }

  internal DateTime ToDateTime()
  {
    try
    {
      if (this.m_time is DerUtcTime)
        return ((DerUtcTime) this.m_time).ToAdjustedDateTime();
      return this.m_time is GeneralizedTime ? ((GeneralizedTime) this.m_time).ToDateTime() : DateTime.Now;
    }
    catch (FormatException ex)
    {
      throw new InvalidOperationException("Invalid entry");
    }
  }

  public override Asn1 GetAsn1() => this.m_time;

  public override string ToString() => this.GetTime();
}
