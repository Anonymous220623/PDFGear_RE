// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.Date
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Globalization;

#nullable disable
namespace Syncfusion.Windows.Shared;

public struct Date : IEquatable<Date>, IComparable<Date>
{
  private int m_year;
  private int m_month;
  private int m_day;

  public int Year
  {
    get => this.m_year;
    set => this.m_year = value;
  }

  public int Month
  {
    get => this.m_month;
    set => this.m_month = value;
  }

  public int Day
  {
    get => this.m_day;
    set => this.m_day = value;
  }

  public Date(int year, int month, int day)
  {
    this.m_year = year;
    this.m_month = month;
    this.m_day = day;
  }

  public Date(DateTime date, Calendar calendar)
  {
    this.m_year = calendar.GetYear(date);
    this.m_month = calendar.GetMonth(date);
    this.m_day = calendar.GetDayOfMonth(date);
  }

  public DateTime ToDateTime(Calendar calendar)
  {
    if (calendar == null)
      throw new ArgumentNullException("Calendar object cannot be null");
    if (this.m_year == 0 || this.m_month == 0 || this.m_day == 0)
    {
      if (this.m_day == 0 && this.m_year != 0 && this.m_month != 0)
        return new DateTime(this.m_year, this.m_month, 1, calendar);
      return this.m_month == 0 && this.m_year != 0 && this.m_day != 0 ? new DateTime(this.m_year, 1, this.m_day, calendar) : new DateTime();
    }
    return this.m_year > 9999 ? new DateTime(9999, this.m_month, this.m_day, calendar) : new DateTime(this.m_year, this.m_month, this.m_day, calendar);
  }

  public Date AddMonthToDate(int month)
  {
    Date date = this;
    int num1 = 0;
    int num2;
    if (Math.Abs(month) > 12)
    {
      num1 = month / 12;
      num2 = month % 12;
    }
    else
      num2 = month;
    int num3 = date.Month + num2;
    date.Year += num1;
    if (num3 > 12)
    {
      date.Year += num3 / 12;
      date.Month = num3 % 12;
    }
    if (num3 < 1)
    {
      date.Month = 12 + num3;
      --date.Year;
    }
    if (num3 >= 1 && num3 <= 12)
      date.Month = num3;
    date.Day = month > 0 ? 1 : 31 /*0x1F*/;
    return date;
  }

  public override bool Equals(object obj) => obj is Date date && this == date;

  public override string ToString()
  {
    return $"{this.Day.ToString()} {this.Month.ToString()} {this.Year.ToString()}";
  }

  public override int GetHashCode() => this.m_day ^ this.m_month ^ this.m_year;

  public static bool operator ==(Date a, Date b)
  {
    return a.Year == b.Year && a.Month == b.Month && a.Day == b.Day;
  }

  public static bool operator !=(Date a, Date b) => !(a == b);

  public static bool operator >(Date a, Date b)
  {
    if (a.Year > b.Year)
      return true;
    if (a.Year < b.Year)
      return false;
    if (a.Month > b.Month)
      return true;
    if (a.Month < b.Month)
      return false;
    if (a.Day > b.Day)
      return true;
    return a.Day < b.Day && false;
  }

  public static bool operator <(Date a, Date b) => !(a > b) && a != b;

  public static bool operator >=(Date a, Date b) => a > b || a == b;

  public static bool operator <=(Date a, Date b) => a < b || a == b;

  public bool Equals(Date other) => this == other;

  public int CompareTo(Date other)
  {
    if (this < other)
      return -1;
    return this > other ? 1 : 0;
  }
}
