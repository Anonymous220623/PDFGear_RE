// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotTables.PivotTableLayout
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotTables;

public class PivotTableLayout : List<List<PivotValueCollections>>
{
  private List<List<PivotValueCollections>> pivotValueCollections = new List<List<PivotValueCollections>>();
  public int maxRowCount;
  public int maxColumnCount;

  public PivotValueCollections this[int rowIndex, int colIndex]
  {
    get => this.pivotValueCollections[rowIndex][colIndex];
    set
    {
      if (rowIndex > this.pivotValueCollections.Count - 1)
      {
        int num = rowIndex - this.pivotValueCollections.Count;
        for (int index = 0; index <= num; ++index)
          this.pivotValueCollections.Add(new List<PivotValueCollections>());
      }
      if (this.maxColumnCount < colIndex)
        this.maxColumnCount = colIndex;
      if (rowIndex > this.maxRowCount)
        this.maxRowCount = rowIndex;
      if (this.pivotValueCollections[rowIndex].Count <= colIndex)
      {
        if (colIndex - this.pivotValueCollections[rowIndex].Count > 0)
        {
          for (int count = this.pivotValueCollections[rowIndex].Count; count <= colIndex; ++count)
          {
            PivotValueCollections valueCollections = new PivotValueCollections();
            valueCollections.PivotTablePartStyle = value.PivotTablePartStyle;
            if (this.pivotValueCollections[rowIndex].Count == 0)
              valueCollections.PivotTablePartStyle = PivotTableParts.WholeTable | PivotTableParts.FirstColumn | PivotTableParts.HeaderRow;
            if (count != colIndex)
              this.pivotValueCollections[rowIndex].Add(valueCollections);
            else
              this.pivotValueCollections[rowIndex].Add(value);
          }
        }
        else
          this.pivotValueCollections[rowIndex].Add(value);
      }
      else
        this.pivotValueCollections[rowIndex][colIndex] = value;
    }
  }

  public new List<PivotValueCollections> this[int rowIndex] => this.pivotValueCollections[rowIndex];
}
