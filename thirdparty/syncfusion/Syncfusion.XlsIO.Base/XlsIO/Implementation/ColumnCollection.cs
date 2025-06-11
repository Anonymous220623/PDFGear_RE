// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.ColumnCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class ColumnCollection : List<Column>
{
  internal Column column;
  private double defaultWidth;
  private WorksheetImpl workSheet;

  internal double Width
  {
    get => this.defaultWidth;
    set => this.defaultWidth = value;
  }

  internal ColumnCollection(WorksheetImpl workSheet, double defaultWidth)
  {
    this.workSheet = workSheet;
    this.defaultWidth = defaultWidth;
  }

  public Column GetColumnByIndex(int index) => this[index];

  internal Column GetOrCreateColumn()
  {
    if (this.column == null)
      this.column = new Column(0, this.workSheet, this.defaultWidth);
    return this.column;
  }

  public double GetWidth(int colIndex, bool isDefaultWidth)
  {
    if (this.workSheet != null && this.workSheet.Columns != null && this.workSheet.Columns.Length > colIndex)
      return this.workSheet.Columns[colIndex].ColumnWidth;
    if (this.column == null || this.column.Index > colIndex)
      return this.defaultWidth;
    return isDefaultWidth || !this.column.IsHidden ? this.column.defaultWidth : 0.0;
  }

  public int GetWidth(int minCol, int maxCol, bool isDefaultWidth, bool isLayout)
  {
    bool flag = this.workSheet.View == SheetView.PageLayout;
    double num1 = !isLayout || !flag ? 1.0 : 1.05;
    int num2 = 0;
    for (int index = minCol; index <= maxCol; ++index)
    {
      double width = this.defaultWidth;
      if (this.column != null && this.column.Index <= index)
        width = !isDefaultWidth ? (this.column.IsHidden ? 0.0 : this.column.defaultWidth) : this.column.defaultWidth;
      num2 += WorksheetImpl.CharacterWidth(width, this.workSheet.GetAppImpl());
    }
    return (int) ((double) num2 * num1 + 0.5);
  }

  internal Column AddColumn(int index)
  {
    Column column = new Column((int) (short) index, this.workSheet, this.defaultWidth);
    this.Add(column);
    return column;
  }

  public bool GetColumnIndex(int columnIndex, out int arrIndex)
  {
    if (this.Count == 0)
    {
      arrIndex = 0;
      return false;
    }
    int num1 = 0;
    int num2 = this.Count - 1;
    int index = 0;
    Column column = (Column) null;
    while (num1 <= num2)
    {
      index = (num1 + num2) / 2;
      column = this[index];
      if (column.Index == columnIndex)
      {
        arrIndex = index;
        return true;
      }
      if (column.Index < columnIndex)
        num1 = index + 1;
      else
        num2 = index - 1;
    }
    arrIndex = column.Index >= columnIndex ? index : index + 1;
    return false;
  }
}
