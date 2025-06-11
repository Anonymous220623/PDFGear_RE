// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.DateComparer
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections;
using System.Globalization;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

internal class DateComparer : IComparer
{
  private string[] formats = new string[29]
  {
    "M/d/yyyy h:mm:ss tt",
    "M/d/yyyy h:mm tt",
    "hh:mm",
    "MM/dd/yyyy hh:mm:ss",
    "M/d/yyyy h:mm:ss",
    "dd-MM-yyyy",
    "DD-MM-yyyy",
    "dd-mm-yyyy",
    "dd-MM-YYYY",
    "DD-MM-YYYY",
    "M/d/yyyy hh:mm tt",
    "M/d/yyyy hh tt",
    "MM-dd-yyyy ",
    "M/d/yyyy h:mm",
    "M/d/yyyy h:mm",
    "m",
    "d",
    "MM/DD/YYYY",
    "mm/dd/yyyy",
    "m/d/yy",
    "MM/dd/yyyy hh:mm",
    "M/dd/yyyy hh:mm",
    "MMMM",
    "MMM",
    "MM",
    "M",
    "yyyy",
    "yy",
    "y"
  };

  public int Compare(object x, object y)
  {
    if (x == null && y == null)
      return 0;
    if (y == null)
      return 1;
    if (x == null)
      return -1;
    DateTime result1;
    DateTime result2;
    return DateTime.TryParseExact(x.ToString(), this.formats, (IFormatProvider) CultureInfo.CurrentUICulture, DateTimeStyles.None, out result1) && DateTime.TryParseExact(y.ToString(), this.formats, (IFormatProvider) CultureInfo.CurrentUICulture, DateTimeStyles.None, out result2) ? DateTime.Compare(result1, result2) : 0;
  }
}
