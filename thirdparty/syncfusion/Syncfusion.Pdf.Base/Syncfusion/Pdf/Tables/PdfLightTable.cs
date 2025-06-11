// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Tables.PdfLightTable
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System;
using System.Collections;
using System.Data;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Tables;

public class PdfLightTable : PdfLayoutElement
{
  private PdfColumnCollection m_columns;
  private PdfRowCollection m_rows;
  internal object m_dataSource;
  private string m_dataMember;
  private PdfLightTableDataSourceType m_dataSourceType;
  private PdfLightTableStyle m_properties;
  private PdfDataSource m_dsParser;
  private bool m_allowRowBreakAcrossPages = true;
  internal PdfLightTableBuiltinStyle m_lightTableBuiltinStyle;
  internal bool m_headerStyle = true;
  internal bool m_bandedRowStyle = true;
  internal bool m_bandedColStyle;
  internal bool m_totalRowStyle;
  internal bool m_firstColumnStyle;
  internal bool m_lastColumnStyle;
  internal bool isBuiltinStyle;
  internal bool isCustomDataSource;
  private bool isColumnProportionalSizing;

  public PdfColumnCollection Columns
  {
    get
    {
      if (this.m_columns == null)
        this.m_columns = this.CreateColumns();
      return this.m_columns;
    }
  }

  public PdfRowCollection Rows
  {
    get
    {
      if (this.m_rows == null)
        this.m_rows = this.CreateRows();
      return this.m_rows;
    }
  }

  public bool ColumnProportionalSizing
  {
    get => this.isColumnProportionalSizing;
    set => this.isColumnProportionalSizing = value;
  }

  public object DataSource
  {
    get => this.m_dataSource;
    set
    {
      this.m_dataSource = value != null ? value : throw new ArgumentNullException(nameof (DataSource));
      this.m_dsParser = this.CreateDataSourceConsumer(value);
      if (this.m_dataSource == null)
        this.m_dataMember = (string) null;
      if (this.DataSourceType == PdfLightTableDataSourceType.TableDirect)
        return;
      this.m_columns = (PdfColumnCollection) null;
    }
  }

  public string DataMember
  {
    get => this.m_dataMember;
    set
    {
      if (!(this.m_dataSource is DataSet))
        return;
      this.m_dataMember = value;
      this.m_dsParser = this.CreateDataSourceConsumer(this.m_dataSource);
    }
  }

  public PdfLightTableDataSourceType DataSourceType
  {
    get => this.m_dataSourceType;
    set => this.m_dataSourceType = value;
  }

  public PdfLightTableStyle Style
  {
    get
    {
      if (this.m_properties == null)
        this.m_properties = new PdfLightTableStyle();
      return this.m_properties;
    }
    set
    {
      this.m_properties = value != null ? value : throw new ArgumentNullException("Properties");
    }
  }

  public bool IgnoreSorting
  {
    get
    {
      bool ignoreSorting = true;
      if (this.m_dsParser != null)
        ignoreSorting = this.m_dsParser.UseSorting;
      return ignoreSorting;
    }
    set
    {
      if (this.m_dsParser == null)
        return;
      this.m_dsParser.UseSorting = !value;
    }
  }

  internal bool RaiseBeginRowLayout => this.BeginRowLayout != null;

  internal bool RaiseEndRowLayout => this.EndRowLayout != null;

  internal bool RaiseBeginCellLayout => this.BeginCellLayout != null;

  internal bool RaiseEndCellLayout => this.EndCellLayout != null;

  public bool AllowRowBreakAcrossPages
  {
    get => this.m_allowRowBreakAcrossPages;
    set => this.m_allowRowBreakAcrossPages = value;
  }

  public event BeginRowLayoutEventHandler BeginRowLayout;

  public event EndRowLayoutEventHandler EndRowLayout;

  public event BeginCellLayoutEventHandler BeginCellLayout;

  public event EndCellLayoutEventHandler EndCellLayout;

  public event QueryNextRowEventHandler QueryNextRow;

  public event QueryColumnCountEventHandler QueryColumnCount;

  public event QueryRowCountEventHandler QueryRowCount;

  public void Draw(PdfGraphics graphics, PointF location, float width)
  {
    this.Draw(graphics, location.X, location.Y, width);
  }

  public void Draw(PdfGraphics graphics, float x, float y, float width)
  {
    RectangleF bounds = new RectangleF(x, y, width, 0.0f);
    this.Draw(graphics, bounds);
  }

  public void Draw(PdfGraphics graphics, RectangleF bounds)
  {
    if (this.m_dataSourceType == PdfLightTableDataSourceType.TableDirect)
      this.DataSource = this.FillData();
    new LightTableLayouter(this).Layout(graphics, bounds);
  }

  public PdfLightTableLayoutResult Draw(PdfPage page, PointF location)
  {
    if (this.m_dataSourceType == PdfLightTableDataSourceType.TableDirect)
      this.DataSource = this.FillData();
    return (PdfLightTableLayoutResult) base.Draw(page, location);
  }

  public PdfLightTableLayoutResult Draw(
    PdfPage page,
    PointF location,
    PdfLightTableLayoutFormat format)
  {
    if (this.m_dataSourceType == PdfLightTableDataSourceType.TableDirect)
      this.DataSource = this.FillData();
    return (PdfLightTableLayoutResult) this.Draw(page, location, (PdfLayoutFormat) format);
  }

  public PdfLightTableLayoutResult Draw(PdfPage page, RectangleF bounds)
  {
    if (this.m_dataSourceType == PdfLightTableDataSourceType.TableDirect)
      this.DataSource = this.FillData();
    return (PdfLightTableLayoutResult) base.Draw(page, bounds);
  }

  public PdfLightTableLayoutResult Draw(
    PdfPage page,
    RectangleF bounds,
    PdfLightTableLayoutFormat format)
  {
    if (this.m_dataSourceType == PdfLightTableDataSourceType.TableDirect)
      this.DataSource = this.FillData();
    return (PdfLightTableLayoutResult) this.Draw(page, bounds, (PdfLayoutFormat) format);
  }

  public PdfLightTableLayoutResult Draw(PdfPage page, float x, float y)
  {
    if (this.m_dataSourceType == PdfLightTableDataSourceType.TableDirect)
      this.DataSource = this.FillData();
    return (PdfLightTableLayoutResult) base.Draw(page, x, y);
  }

  public PdfLightTableLayoutResult Draw(
    PdfPage page,
    float x,
    float y,
    PdfLightTableLayoutFormat format)
  {
    if (this.m_dataSourceType == PdfLightTableDataSourceType.TableDirect)
      this.DataSource = this.FillData();
    return (PdfLightTableLayoutResult) this.Draw(page, x, y, (PdfLayoutFormat) format);
  }

  public PdfLightTableLayoutResult Draw(PdfPage page, float x, float y, float width)
  {
    return this.Draw(page, x, y, width, (PdfLightTableLayoutFormat) null);
  }

  public PdfLightTableLayoutResult Draw(
    PdfPage page,
    float x,
    float y,
    float width,
    PdfLightTableLayoutFormat format)
  {
    if (this.m_dataSourceType == PdfLightTableDataSourceType.TableDirect)
      this.DataSource = this.FillData();
    RectangleF layoutRectangle = new RectangleF(x, y, width, 0.0f);
    return (PdfLightTableLayoutResult) this.Draw(page, layoutRectangle, (PdfLayoutFormat) format);
  }

  public void ApplyBuiltinStyle(PdfLightTableBuiltinStyle tableStyle)
  {
    this.isBuiltinStyle = true;
    this.m_lightTableBuiltinStyle = tableStyle;
  }

  public void ApplyBuiltinStyle(
    PdfLightTableBuiltinStyle lightTableStyle,
    PdfLightTableBuiltinStyleSettings lightTableSetting)
  {
    this.m_headerStyle = lightTableSetting.ApplyStyleForHeaderRow;
    this.m_totalRowStyle = lightTableSetting.ApplyStyleForLastRow;
    this.m_lastColumnStyle = lightTableSetting.ApplyStyleForLastColumn;
    this.m_firstColumnStyle = lightTableSetting.ApplyStyleForFirstColumn;
    this.m_bandedColStyle = lightTableSetting.ApplyStyleForBandedColumns;
    this.m_bandedRowStyle = lightTableSetting.ApplyStyleForBandedRows;
    this.ApplyBuiltinStyle(lightTableStyle);
  }

  public override void Draw(PdfGraphics graphics, float x, float y)
  {
    SizeF clientSize = graphics.ClientSize;
    clientSize.Width -= x;
    clientSize.Height -= y;
    this.Draw(graphics, x, y, clientSize.Width);
  }

  protected override PdfLayoutResult Layout(PdfLayoutParams param)
  {
    if ((double) param.Bounds.Width < 0.0)
      throw new ArgumentOutOfRangeException("Width");
    if (this.DataSource == null && this.m_dataSourceType == PdfLightTableDataSourceType.TableDirect)
      this.DataSource = this.FillData();
    return new LightTableLayouter(this).Layout(param);
  }

  protected override void DrawInternal(PdfGraphics graphics)
  {
    new LightTableLayouter(this).Layout(graphics, PointF.Empty);
  }

  internal void OnBeginRowLayout(BeginRowLayoutEventArgs args)
  {
    if (!this.RaiseBeginRowLayout)
      return;
    this.BeginRowLayout((object) this, args);
  }

  internal void OnEndRowLayout(EndRowLayoutEventArgs args)
  {
    if (!this.RaiseEndRowLayout)
      return;
    this.EndRowLayout((object) this, args);
  }

  internal void OnBeginCellLayout(BeginCellLayoutEventArgs args)
  {
    if (!this.RaiseBeginCellLayout)
      return;
    this.BeginCellLayout((object) this, args);
  }

  internal void OnEndCellLayout(EndCellLayoutEventArgs args)
  {
    if (!this.RaiseEndCellLayout)
      return;
    this.EndCellLayout((object) this, args);
  }

  internal string[] GetNextRow(ref int index)
  {
    string[] strArray = new string[0];
    string[] nextRow;
    if (this.m_dsParser != null)
      nextRow = this.m_dsParser.GetRow(ref index);
    else if (this.QueryNextRow != null)
    {
      nextRow = this.OnGetNextRow(index);
    }
    else
    {
      nextRow = (string[]) null;
      if (this.Rows.Count > index && this.Rows[index].Values != null)
      {
        if (this.Rows[index].Values.Length != 0)
        {
          int length = this.Rows[index].Values.Length;
          nextRow = new string[length];
          for (int index1 = 0; index1 < length; ++index1)
            nextRow[index1] = this.Rows[index].Values[index1].ToString();
        }
      }
      else
        nextRow = this.OnGetNextRow(index);
    }
    return nextRow;
  }

  internal string[] GetColumnCaptions() => this.GetColumnsCaption();

  private PdfDataSource CreateDataSourceConsumer(object value)
  {
    Array array = value as Array;
    DataSet dataSet = value as DataSet;
    DataColumn column = value as DataColumn;
    DataTable table = value as DataTable;
    DataView view = value as DataView;
    IEnumerable customSource = value as IEnumerable;
    PdfDataSource dataSourceConsumer = (PdfDataSource) null;
    this.isCustomDataSource = false;
    if (array != null)
      dataSourceConsumer = new PdfDataSource(array);
    else if (column != null)
      dataSourceConsumer = new PdfDataSource(column);
    else if (table != null)
      dataSourceConsumer = new PdfDataSource(table);
    else if (view != null)
      dataSourceConsumer = new PdfDataSource(view);
    else if (dataSet != null)
      dataSourceConsumer = new PdfDataSource(dataSet, this.m_dataMember);
    else if (customSource != null)
    {
      dataSourceConsumer = new PdfDataSource(customSource);
      this.isCustomDataSource = true;
    }
    return dataSourceConsumer;
  }

  private object FillData() => this.FillDataValue();

  private PdfColumnCollection CreateColumns()
  {
    return this.CreateColumnCollection(new PdfColumnCollection());
  }

  private PdfRowCollection CreateRows() => this.CreateRowCollection(new PdfRowCollection());

  private PdfColumnCollection CreateColumnCollection(PdfColumnCollection columns)
  {
    int num = this.m_dsParser != null ? this.m_dsParser.ColumnCount : this.OnGetColumnNumber();
    for (int index = 0; index < num; ++index)
    {
      PdfColumn column = new PdfColumn(10f);
      columns.Add(column);
    }
    return columns;
  }

  private object FillDataValue()
  {
    try
    {
      DataTable dataTable = new DataTable();
      for (int index = 0; index < this.Columns.Count; ++index)
      {
        if (this.Columns[index].ColumnName != null)
          dataTable.Columns.Add(this.Columns[index].ColumnName);
        else
          dataTable.Columns.Add(string.Empty);
        this.Columns[index].m_dataSourceType = this.m_dataSourceType;
        this.Columns[index].Width = this.Columns[index].Width;
        this.Columns[index].StringFormat = this.Columns[index].StringFormat;
      }
      foreach (PdfRow row in (PdfCollection) this.Rows)
      {
        if (row.Values != null)
          dataTable.Rows.Add(row.Values);
      }
      return (object) dataTable;
    }
    catch (Exception ex)
    {
      throw new PdfException("Please check whether the number of rows matches the column count.", ex);
    }
  }

  private PdfRowCollection CreateRowCollection(PdfRowCollection rows)
  {
    int num = this.m_dsParser != null ? this.m_dsParser.RowCount : this.OnGetRowNumber();
    for (int index = 0; index < num; ++index)
    {
      PdfRow row = new PdfRow();
      rows.Add(row);
    }
    return rows;
  }

  private string[] GetColumnsCaption()
  {
    PdfColumnCollection columns = this.Columns;
    string[] columnsCaption = this.m_dsParser != null ? this.m_dsParser.ColumnCaptions : (string[]) null;
    if (this.m_dsParser != null)
    {
      for (int index = 0; index < this.m_dsParser.ColumnCount; ++index)
      {
        if (columns[index].ColumnName != null)
        {
          if (columnsCaption == null)
            columnsCaption = new string[this.m_dsParser.ColumnCount];
          columnsCaption[index] = columns[index].ColumnName;
        }
      }
    }
    else if (columns != null)
    {
      for (int index = 0; index < columns.Count; ++index)
      {
        if (columns[index].ColumnName != null)
        {
          if (columnsCaption == null)
            columnsCaption = new string[columns.Count];
          columnsCaption[index] = columns[index].ColumnName;
        }
      }
    }
    return columnsCaption;
  }

  private string[] OnGetNextRow(int rowIndex)
  {
    string[] nextRow = (string[]) null;
    if (this.QueryNextRow != null)
    {
      QueryNextRowEventArgs args = new QueryNextRowEventArgs(this.Columns.Count, rowIndex);
      if (this.Rows != null && this.Rows.Count > rowIndex || this.Rows.Count == 0)
        this.QueryNextRow((object) this, args);
      nextRow = args.RowData;
    }
    return nextRow;
  }

  private int OnGetColumnNumber()
  {
    int num = 0;
    if (this.QueryColumnCount != null)
    {
      QueryColumnCountEventArgs args = new QueryColumnCountEventArgs();
      this.QueryColumnCount((object) this, args);
      num = args.ColumnCount;
    }
    return num >= 0 ? num : throw new PdfLightTableException("There is no columns.");
  }

  private int OnGetRowNumber()
  {
    int num = 0;
    if (this.QueryRowCount != null)
    {
      QueryRowCountEventArgs args = new QueryRowCountEventArgs();
      this.QueryRowCount((object) this, args);
      num = args.RowCount;
    }
    return num >= 0 ? num : throw new PdfLightTableException("There is no Rows.");
  }
}
