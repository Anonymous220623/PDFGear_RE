// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.PivotCellInfos
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

public class PivotCellInfos : List<List<PivotCellInfo>>
{
  public PivotCellInfos()
  {
  }

  public PivotCellInfos(int rowCount, int colCount)
  {
    this.Capacity = rowCount;
    for (int index = 0; index < rowCount; ++index)
      this.Add(new List<PivotCellInfo>((IEnumerable<PivotCellInfo>) new PivotCellInfo[colCount]));
  }

  public PivotCellInfo this[int rowIndex, int colIndex]
  {
    get => this[rowIndex][colIndex];
    set => this[rowIndex][colIndex] = value;
  }

  public int GetLength(int index) => index != 0 ? this[0].Count : this.Count;
}
