// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.DecimalComparer
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections;
using System.Globalization;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

internal class DecimalComparer : IComparer
{
  public int Compare(object x, object y)
  {
    NumberStyles style = NumberStyles.Currency;
    Decimal result;
    Decimal.TryParse(x != null ? x.ToString() : "", style, (IFormatProvider) null, out result);
    Decimal num1 = result;
    Decimal.TryParse(y != null ? y.ToString() : "", style, (IFormatProvider) null, out result);
    Decimal num2 = result;
    if (num1 == 0.0M && num2 == 0.0M)
      return 0;
    if (num2 == 0.0M)
      return 1;
    return num1 == 0.0M ? -1 : num1.CompareTo(num2);
  }
}
