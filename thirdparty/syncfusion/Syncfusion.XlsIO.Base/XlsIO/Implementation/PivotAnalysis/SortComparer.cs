// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.SortComparer
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

public class SortComparer : IComparer<object>
{
  private List<string> propertyNames;
  private GetValueDelegate GetValue;
  private List<string> formats;
  private List<IComparer> comparers;

  public SortComparer(
    List<string> propertyNames,
    List<string> formats,
    List<IComparer> comparers,
    GetValueDelegate GetValue)
  {
    this.propertyNames = propertyNames;
    this.GetValue = GetValue;
    this.formats = formats;
    this.comparers = comparers;
  }

  public int Compare(object x, object y)
  {
    int num = 0;
    for (int index = 0; index < this.propertyNames.Count; ++index)
    {
      IComparable x1 = this.GetValue(x, this.propertyNames[index]);
      IComparable y1 = this.GetValue(y, this.propertyNames[index]);
      if (x1 == null && y1 != null)
      {
        num = -1;
      }
      else
      {
        if (this.formats != null && this.formats[index] != null && this.formats[index].Length > 0)
        {
          if (x1 != null)
            x1 = (IComparable) string.Format((IFormatProvider) CultureInfo.CurrentUICulture, this.formats[index], (object) x1);
          if (y1 != null)
            y1 = (IComparable) string.Format((IFormatProvider) CultureInfo.CurrentUICulture, this.formats[index], (object) y1);
        }
        num = this.comparers == null || this.comparers[index] == null ? (x1 != null ? x1.CompareTo((object) y1) : 0) : this.comparers[index].Compare((object) x1, (object) y1);
      }
      if (num != 0)
        break;
    }
    return num;
  }
}
