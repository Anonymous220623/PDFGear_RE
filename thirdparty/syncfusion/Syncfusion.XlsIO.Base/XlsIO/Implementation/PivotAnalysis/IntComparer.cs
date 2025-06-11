// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.IntComparer
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections;
using System.Globalization;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

internal class IntComparer : IComparer
{
  public int Compare(object x, object y)
  {
    NumberStyles style = NumberStyles.Currency;
    int result;
    int.TryParse(x != null ? x.ToString() : "", style, (IFormatProvider) null, out result);
    int num1 = result;
    int.TryParse(y != null ? y.ToString() : "", style, (IFormatProvider) null, out result);
    int num2 = result;
    if (num1 == 0 && num2 == 0)
      return 0;
    if (num2 == 0)
      return 1;
    return num1 == 0 ? -1 : num1.CompareTo(num2);
  }
}
