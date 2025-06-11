// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.BinaryList
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

public class BinaryList : List<IComparable>
{
  public int AddIfUnique(IComparable o) => this.AddIfUnique(o, false);

  internal int AddIfUnique(IComparable o, bool ShouldRefreshKeys)
  {
    int num = -1;
    if (o != null)
    {
      KeysCalculationValues calculationValues1 = o as KeysCalculationValues;
      if (ShouldRefreshKeys && calculationValues1 != null && !calculationValues1.Keys.All<IComparable>((Func<IComparable, bool>) (x => x.ToString() == " ")))
      {
        IComparable comparable = (IComparable) new KeysCalculationValues()
        {
          Keys = new List<IComparable>()
        };
        KeysCalculationValues calculationValues2 = comparable as KeysCalculationValues;
        foreach (IComparable key in calculationValues1.Keys)
          calculationValues2.Keys.Add(key);
        if (calculationValues2 != null)
        {
          for (int index = 0; calculationValues2.Keys != null && index < calculationValues2.Keys.Count; ++index)
          {
            if (calculationValues2.Keys[index].ToString() == " ")
              calculationValues2.Keys[index] = (IComparable) null;
          }
        }
        num = this.BinarySearch(comparable);
      }
      else
        num = this.BinarySearch(o);
      if (num < 0)
        this.Insert(-num - 1, o);
    }
    return num;
  }
}
