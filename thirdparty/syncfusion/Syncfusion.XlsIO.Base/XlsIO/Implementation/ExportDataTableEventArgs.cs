// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.ExportDataTableEventArgs
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class ExportDataTableEventArgs : EventArgs
{
  private int m_excelRowIndex;
  private int m_excelColumnIndex;
  private int m_DataTableColumnIndex;
  private object m_excelValue;
  private ExportDataTableActions m_exportDataTableAction;
  private IRange m_cellRange;
  private Type m_columnType;
  private object m_dataTableValue;

  public int DataTableColumnIndex
  {
    get => this.m_DataTableColumnIndex;
    internal set => this.m_DataTableColumnIndex = value;
  }

  public Type ColumnType
  {
    get => this.m_columnType;
    internal set => this.m_columnType = value;
  }

  public IRange CellRange
  {
    get => this.m_cellRange;
    internal set => this.m_cellRange = value;
  }

  public int ExcelRowIndex
  {
    get => this.m_excelRowIndex;
    internal set => this.m_excelRowIndex = value;
  }

  public int ExcelColumnIndex
  {
    get => this.m_excelColumnIndex;
    internal set => this.m_excelColumnIndex = value;
  }

  public object ExcelValue
  {
    get => this.m_excelValue;
    internal set => this.m_excelValue = value;
  }

  public object DataTableValue
  {
    get => this.m_dataTableValue;
    set => this.m_dataTableValue = value;
  }

  public ExportDataTableActions ExportDataTableAction
  {
    get => this.m_exportDataTableAction;
    set => this.m_exportDataTableAction = value;
  }

  internal ExportDataTableEventArgs()
  {
  }
}
