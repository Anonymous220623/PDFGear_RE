// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.KeysCalculationValues
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

internal class KeysCalculationValues : IComparable
{
  private List<object> rawValues = new List<object>();

  public List<object> RawValues => this.rawValues;

  public List<IComparable> Keys { get; set; }

  public List<SummaryBase> Values { get; set; }

  public IComparer[] Comparers { get; set; }

  public int CompareTo(object obj)
  {
    int num = 0;
    if (this.Keys != null)
    {
      int index = 0;
      if (obj is KeysCalculationValues calculationValues)
      {
        for (; num == 0 && index < this.Keys.Count && calculationValues.Keys.Count > index; ++index)
          num = this.Comparers == null || this.Comparers[index] == null ? (!(this.Keys[index].ToString() != " ") || this.Keys[index] == null ? (!(this.Keys[index].ToString() == " ") ? this.Keys[index].CompareTo((object) calculationValues.Keys[index]) : (calculationValues.Keys[index] == null || calculationValues.Keys[index].ToString() == " " ? 0 : -1)) : (calculationValues.Keys[index] != null ? this.Keys[index].CompareTo((object) calculationValues.Keys[index]) : -1)) : this.Comparers[index].Compare((object) this.Keys[index], (object) calculationValues.Keys[index]);
      }
    }
    return num;
  }

  public override string ToString()
  {
    string empty = string.Empty;
    foreach (IComparable key in this.Keys)
      empty += string.IsNullOrEmpty(empty) ? key.ToString() : "-" + key.ToString();
    return empty;
  }
}
