// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.DateUtils
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Globalization;

#nullable disable
namespace Syncfusion.Windows.Shared;

internal class DateUtils
{
  private DateUtils()
  {
  }

  public static int AddMonth(int month, int param)
  {
    int num1 = 0;
    int num2 = month + param;
    if (num2 > 12)
      num1 = num2 % 12;
    if (num2 < 1)
      num1 = 12 + num2;
    if (num2 >= 1 && num2 <= 12)
      num1 = num2;
    return num1;
  }

  public static DateTime GetFirstDayOfMonth(int year, int month, Calendar calendar)
  {
    return calendar.ToDateTime(year, month, 1, 0, 0, 0, 0);
  }

  public static int[,] GenerateMatrix(
    int month,
    int year,
    DateTimeFormatInfo format,
    Calendar calendar)
  {
    DateTime firstDayOfMonth = DateUtils.GetFirstDayOfMonth(year, month, calendar);
    DayOfWeek dayOfWeek = calendar.GetDayOfWeek(firstDayOfMonth);
    int[,] matrix = new int[6, 7];
    int num1 = (int) dayOfWeek;
    int firstDayOfWeek = (int) format.FirstDayOfWeek;
    int daysInMonth1 = calendar.GetDaysInMonth(year, month);
    int num2 = (6 + num1 - firstDayOfWeek) % 7 + 1;
    int num3 = num2;
    int index1 = 0;
    int num4 = 1;
    for (; index1 < 6; ++index1)
    {
      if (index1 > 0)
        num3 = 0;
      int index2 = num3;
      while (index2 < 7)
      {
        matrix[index1, index2] = num4;
        ++index2;
        ++num4;
      }
    }
    int num5 = num2;
    int index3 = 0;
    int num6 = 0;
    int num7 = 1;
    for (; index3 < 6; ++index3)
    {
      if (index3 > 0)
        num5 = 0;
      int index4 = num5;
      while (index4 < 7)
      {
        if (num6 >= daysInMonth1)
        {
          matrix[index3, index4] = num7;
          ++num7;
        }
        ++index4;
        ++num6;
      }
    }
    if (month == 1)
      --year;
    Date date = new Date(calendar.MinSupportedDateTime, calendar);
    if (!(new Date(year, DateUtils.AddMonth(month, -1), 1) < date))
    {
      int daysInMonth2 = calendar.GetDaysInMonth(year, DateUtils.AddMonth(month, -1));
      int index5 = 0;
      int num8 = daysInMonth2 - num2 + 1;
      while (index5 < num2)
      {
        matrix[0, index5] = num8;
        ++index5;
        ++num8;
      }
    }
    return matrix;
  }
}
