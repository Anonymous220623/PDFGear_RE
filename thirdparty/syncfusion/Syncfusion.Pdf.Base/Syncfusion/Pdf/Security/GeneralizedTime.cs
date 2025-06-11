// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.GeneralizedTime
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Globalization;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class GeneralizedTime : Asn1
{
  internal const string GMT = "GMT";
  internal const char TZ = 'Z';
  internal const string Day = "dd";
  internal const string Month = "MM";
  internal const string Year = "yyyy";
  internal const string Hours = "HH";
  internal const string Minutes = "mm";
  internal const string Seconds = "ss";
  private readonly string m_time;

  internal string TimeString => this.m_time;

  internal GeneralizedTime(byte[] bytes)
  {
    this.m_time = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
  }

  internal string GetTime()
  {
    if (this.m_time[this.m_time.Length - 1] == 'Z')
      return this.m_time.Substring(0, this.m_time.Length - 1) + "GMT+00:00";
    int num1 = this.m_time.Length - 5;
    switch (this.m_time[num1])
    {
      case '+':
      case '-':
        return $"{this.m_time.Substring(0, num1)}GMT{this.m_time.Substring(num1, 3)}:{this.m_time.Substring(num1 + 3)}";
      default:
        int num2 = this.m_time.Length - 3;
        switch (this.m_time[num2])
        {
          case '+':
          case '-':
            return $"{this.m_time.Substring(0, num2)}GMT{this.m_time.Substring(num2)}:00";
          default:
            char ch = '+';
            TimeSpan timeSpan = TimeZone.CurrentTimeZone.GetUtcOffset(this.ToDateTime());
            if (timeSpan.CompareTo(TimeSpan.Zero) < 0)
            {
              ch = '-';
              timeSpan = timeSpan.Duration();
            }
            int hours = timeSpan.Hours;
            int minutes = timeSpan.Minutes;
            return $"{this.m_time}GMT{(object) ch}{(hours < 10 ? (object) ("0" + hours.ToString()) : (object) hours.ToString())}:{(minutes < 10 ? (object) ("0" + minutes.ToString()) : (object) minutes.ToString())}";
        }
    }
  }

  internal DateTime ToDateTime()
  {
    string time = this.m_time;
    bool flag = false;
    string format;
    if (time.EndsWith("Z"))
      format = this.m_time.IndexOf('.') != 14 ? "yyyyMMddHHmmss\\Z" : $"yyyyMMddHHmmss.{this.FormatString(time.Length - time.IndexOf('.') - 2)}\\Z";
    else if (this.m_time.IndexOf('-') > 0 || this.m_time.IndexOf('+') > 0)
    {
      time = this.GetTime();
      flag = true;
      format = this.m_time.IndexOf('.') != 14 ? "yyyyMMddHHmmss'GMT'zzz" : $"yyyyMMddHHmmss.{this.FormatString(time.IndexOf("GMT") - 1 - time.IndexOf('.'))}'GMT'zzz";
    }
    else
      format = this.m_time.IndexOf('.') != 14 ? "yyyyMMddHHmmss" : "yyyyMMddHHmmss." + this.FormatString(time.Length - 1 - time.IndexOf('.'));
    DateTime exact = DateTime.ParseExact(time, format, (IFormatProvider) DateTimeFormatInfo.InvariantInfo);
    return !flag ? exact : exact.ToUniversalTime();
  }

  private string FormatString(int count)
  {
    StringBuilder stringBuilder = new StringBuilder();
    for (int index = 0; index < count; ++index)
      stringBuilder.Append('f');
    return stringBuilder.ToString();
  }

  internal override void Encode(DerStream dStream)
  {
    dStream.WriteEncoded(24, Encoding.ASCII.GetBytes(this.m_time));
  }

  protected override bool IsEquals(Asn1 asn1Object) => throw new NotImplementedException();

  public override int GetHashCode() => this.m_time.GetHashCode();

  internal static GeneralizedTime GetGeneralizedTime(object obj)
  {
    switch (obj)
    {
      case null:
      case GeneralizedTime _:
        return (GeneralizedTime) obj;
      default:
        return (GeneralizedTime) null;
    }
  }

  internal static GeneralizedTime GetGeneralizedTime(Asn1Tag tag, bool isExplicit)
  {
    Asn1 asn1 = tag.GetObject();
    return isExplicit || asn1 is GeneralizedTime ? GeneralizedTime.GetGeneralizedTime((object) asn1) : new GeneralizedTime(((Asn1Octet) asn1).GetOctets());
  }
}
