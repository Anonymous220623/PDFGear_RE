// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.DoubleComparer
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections;
using System.Globalization;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

internal class DoubleComparer : IComparer
{
  public int Compare(object x, object y)
  {
    NumberStyles style = NumberStyles.Currency;
    double result;
    double.TryParse(x != null ? x.ToString() : "", style, (IFormatProvider) null, out result);
    double num1 = x != null ? (x.ToString().Contains("%") ? double.Parse(x.ToString().Substring(0, x.ToString().Length - 1)) / 100.0 : (!x.ToString().ToUpper().Contains("E") || !(x.GetType() != typeof (string)) ? result : double.Parse(x.ToString(), (IFormatProvider) CultureInfo.InvariantCulture))) : result;
    double.TryParse(y != null ? y.ToString() : "", style, (IFormatProvider) null, out result);
    double num2 = y != null ? (y.ToString().Contains("%") ? double.Parse(y.ToString().Substring(0, y.ToString().Length - 1)) / 100.0 : (!y.ToString().ToUpper().Contains("E") || !(y.GetType() != typeof (string)) ? result : double.Parse(y.ToString(), (IFormatProvider) CultureInfo.InvariantCulture))) : result;
    if (num1 == 0.0 && num2 == 0.0)
      return 0;
    if (num2 == 0.0)
      return 1;
    return num1 == 0.0 ? -1 : num1.CompareTo(num2);
  }
}
