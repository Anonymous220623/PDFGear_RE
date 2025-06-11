// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.DerUtcTime
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Globalization;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class DerUtcTime : Asn1
{
  private string m_time;

  internal static DerUtcTime GetUtcTime(object obj)
  {
    switch (obj)
    {
      case null:
      case DerUtcTime _:
        return (DerUtcTime) obj;
      default:
        throw new ArgumentException("Invalid entry");
    }
  }

  internal static DerUtcTime GetInstance(Asn1Tag tag, bool isExplicit)
  {
    Asn1 asn1 = tag.GetObject();
    return isExplicit || asn1 is DerUtcTime ? DerUtcTime.GetUtcTime((object) asn1) : new DerUtcTime(((Asn1Octet) asn1).GetOctets());
  }

  internal DerUtcTime(string time)
  {
    this.m_time = time != null ? time : throw new ArgumentNullException(nameof (time));
    try
    {
      this.ToDateTime();
    }
    catch (FormatException ex)
    {
      throw new ArgumentException("Invalid date format : " + ex.Message);
    }
  }

  internal DerUtcTime(DateTime time) => this.m_time = time.ToString("yyMMddHHmmss") + "Z";

  internal DerUtcTime(byte[] bytes)
  {
    this.m_time = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
  }

  public DateTime ToDateTime()
  {
    return DateTime.ParseExact(this.TimeString, "yyMMddHHmmss'GMT'zzz", (IFormatProvider) DateTimeFormatInfo.InvariantInfo).ToUniversalTime();
  }

  internal DateTime ToAdjustedDateTime()
  {
    return DateTime.ParseExact(this.AdjustedTimeString, "yyyyMMddHHmmss'GMT'zzz", (IFormatProvider) DateTimeFormatInfo.InvariantInfo).ToUniversalTime();
  }

  private byte[] GetBytes() => Encoding.ASCII.GetBytes(this.m_time);

  internal override void Encode(DerStream stream) => stream.WriteEncoded(23, this.GetBytes());

  protected override bool IsEquals(Asn1 asn1)
  {
    return asn1 is DerUtcTime derUtcTime && this.m_time.Equals(derUtcTime.m_time);
  }

  public override int GetHashCode() => this.m_time.GetHashCode();

  public override string ToString() => this.m_time;

  internal string AdjustedTimeString
  {
    get
    {
      string timeString = this.TimeString;
      return (timeString[0] < '5' ? "20" : "19") + timeString;
    }
  }

  internal string TimeString
  {
    get
    {
      if (this.m_time.IndexOf('-') < 0 && this.m_time.IndexOf('+') < 0)
        return this.m_time.Length == 11 ? this.m_time.Substring(0, 10) + "00GMT+00:00" : this.m_time.Substring(0, 12) + "GMT+00:00";
      int num = this.m_time.IndexOf('-');
      if (num < 0)
        num = this.m_time.IndexOf('+');
      string time = this.m_time;
      if (num == this.m_time.Length - 3)
        time += "00";
      return num == 10 ? $"{time.Substring(0, 10)}00GMT{time.Substring(10, 3)}:{time.Substring(13, 2)}" : $"{time.Substring(0, 12)}GMT{time.Substring(12, 3)}:{time.Substring(15, 2)}";
    }
  }
}
