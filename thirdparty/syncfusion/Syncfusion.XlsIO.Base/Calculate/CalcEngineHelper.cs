// Decompiled with JetBrains decompiler
// Type: Syncfusion.Calculate.CalcEngineHelper
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.Calculate;

public static class CalcEngineHelper
{
  public static DateTime FromOADate(double doubleOLEValue)
  {
    string[] strArray = doubleOLEValue >= -657435.0 && doubleOLEValue <= 2958465.99999999 ? doubleOLEValue.ToString().Split('.') : throw new ArgumentException("Not an valid OLE value.");
    int int32 = Convert.ToInt32(strArray[0]);
    double num = strArray.Length > 1 ? Convert.ToDouble("." + strArray[1]) : 1.0;
    return DateTime.Parse("1899-12-30 12:0:0 AM").AddDays((double) int32);
  }

  public static double ToOADate(this DateTime inDateTime)
  {
    DateTime dateTime1 = DateTime.Parse("1899-12-30 12:0:0 AM");
    DateTime dateTime2 = DateTime.Parse("9999-12-31 12:0:0 AM");
    if (inDateTime < dateTime1 || inDateTime > dateTime2)
      throw new ArgumentException("Not an Valid OLE Date.");
    double days = (double) (inDateTime - dateTime1).Days;
    TimeSpan timeSpan = new TimeSpan(inDateTime.Hour, inDateTime.Minute, inDateTime.Second);
    double num = ((double) timeSpan.Hours + ((double) timeSpan.Minutes + (double) timeSpan.Seconds / 60.0) / 60.0) / 24.0;
    return days + num;
  }
}
