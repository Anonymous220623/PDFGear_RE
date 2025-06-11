// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Tables.QueryNextRowEventArgs
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Tables;

public class QueryNextRowEventArgs : EventArgs
{
  private string[] m_rowData;
  private int m_columnCount;
  private int m_rowIndex;

  public string[] RowData
  {
    get => this.m_rowData;
    set
    {
      if (this.m_columnCount != 0 && value != null && value.Length != this.m_columnCount)
        throw new ArgumentException("The data array is not of the proper length.", nameof (RowData));
      this.m_rowData = value;
    }
  }

  public int ColumnCount => this.m_columnCount;

  public int RowIndex => this.m_rowIndex;

  internal QueryNextRowEventArgs(int columnCount, int rowIndex)
  {
    this.m_columnCount = columnCount >= 0 ? columnCount : throw new ArgumentOutOfRangeException(nameof (columnCount));
    this.m_rowIndex = rowIndex;
  }
}
