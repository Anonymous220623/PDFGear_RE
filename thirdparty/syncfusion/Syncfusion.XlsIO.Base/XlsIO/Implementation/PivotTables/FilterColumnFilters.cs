// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotTables.FilterColumnFilters
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotTables;

internal class FilterColumnFilters
{
  private List<string> Values = new List<string>();

  public string this[int index]
  {
    get
    {
      return this.Values.Count > 0 ? this.Values[index] : throw new ArgumentException("Specified index is not valid");
    }
  }

  public int Count() => this.Values.Count;

  public void Add(string Value) => this.Values.Add(Value);
}
