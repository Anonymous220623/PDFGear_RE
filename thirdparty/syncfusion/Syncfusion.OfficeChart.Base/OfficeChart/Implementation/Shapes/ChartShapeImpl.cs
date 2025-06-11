// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Shapes.ChartShapeImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Charts;
using Syncfusion.OfficeChart.Parser;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing;
using Syncfusion.OfficeChart.Parser.Biff_Records.ObjRecords;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Shapes;

internal class ChartShapeImpl : 
  ShapeImpl,
  IOfficeChartShape,
  IShape,
  IOfficeChart,
  IParentApplication
{
  private const int DEF_SHAPE_INSTANCE = 201;
  private const int DEF_SHAPE_VERSION = 2;
  private const int DEF_OPTIONS_VERSION = 3;
  private const int DEF_OPTIONS_INSTANCE = 8;
  private const uint DEF_LOCK_GROUPING_VALUE = 17039620;
  private const uint DEF_LINECOLOR = 134217805 /*0x0800004D*/;
  private const uint DEF_NOLINEDRAWDASH = 524296 /*0x080008*/;
  private const uint DEF_SHADOWOBSCURED = 131072 /*0x020000*/;
  private const uint DEF_FORECOLOR = 134217806 /*0x0800004E*/;
  private const uint DEF_BACKCOLOR = 134217805 /*0x0800004D*/;
  private ChartImpl m_chart;
  private int m_iTopRow;
  private int m_iBottomRow;
  private int m_iLeftColumn;
  private int m_iRightColumn;
  private WorksheetBaseImpl m_worksheet;
  private int m_offsetX;
  private int m_offsetY;
  private int m_extentsX;
  private int m_extentsY;
  private ChartCategoryCollection m_categories;

  public ChartShapeImpl(
    IApplication application,
    object parent,
    ChartShapeImpl instance,
    Dictionary<string, string> hashNewNames,
    Dictionary<int, int> dicFontIndexes)
    : base(application, parent, (ShapeImpl) instance)
  {
    this.m_chart = instance.m_chart.Clone(hashNewNames, (object) this, dicFontIndexes);
    this.m_bIsDisposed = instance.m_bIsDisposed;
    this.m_iBottomRow = instance.m_iBottomRow;
    this.m_iLeftColumn = instance.m_iLeftColumn;
    this.m_iRightColumn = instance.m_iRightColumn;
    this.m_iTopRow = instance.m_iTopRow;
  }

  public ChartShapeImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.m_chart = new ChartImpl(application, (object) this);
    this.ShapeType = OfficeShapeType.Chart;
    this.BottomRow = 20;
    this.RightColumn = 10;
    this.m_bSupportOptions = false;
  }

  [CLSCompliant(false)]
  public ChartShapeImpl(
    IApplication application,
    object parent,
    MsofbtSpContainer container,
    OfficeParseOptions options)
    : base(application, parent, container, options)
  {
    this.ShapeType = OfficeShapeType.Chart;
    this.m_bSupportOptions = false;
  }

  public ChartImpl ChartObject => this.m_chart;

  internal int OffsetX
  {
    get => this.m_offsetX;
    set => this.m_offsetX = value;
  }

  public void Refresh() => this.m_chart.Refresh();

  internal int OffsetY
  {
    get => this.m_offsetY;
    set => this.m_offsetY = value;
  }

  internal int ExtentsX
  {
    get => this.m_extentsX;
    set => this.m_extentsX = value;
  }

  internal int ExtentsY
  {
    get => this.m_extentsY;
    set => this.m_extentsY = value;
  }

  public IOfficeChartData ChartData => this.m_chart.ChartData;

  public int Rotation
  {
    get => this.m_chart.Rotation;
    set => this.m_chart.Rotation = value;
  }

  public OfficeSeriesNameLevel SeriesNameLevel
  {
    get => this.m_chart.SeriesNameLevel;
    set => this.m_chart.SeriesNameLevel = value;
  }

  public int Style
  {
    get => this.m_chart.Style;
    set => this.m_chart.Style = value;
  }

  public OfficeCategoriesLabelLevel CategoryLabelLevel
  {
    get => this.m_chart.CategoryLabelLevel;
    set => this.m_chart.CategoryLabelLevel = value;
  }

  public IOfficeChartCategories Categories => this.m_chart.Categories;

  public int Elevation
  {
    get => this.m_chart.Elevation;
    set => this.m_chart.Elevation = value;
  }

  public int Perspective
  {
    get => this.m_chart.Perspective;
    set => this.m_chart.Perspective = value;
  }

  public int HeightPercent
  {
    get => this.m_chart.HeightPercent;
    set => this.m_chart.HeightPercent = value;
  }

  public int DepthPercent
  {
    get => this.m_chart.DepthPercent;
    set => this.m_chart.DepthPercent = value;
  }

  public int GapDepth
  {
    get => this.m_chart.GapDepth;
    set => this.m_chart.GapDepth = value;
  }

  public bool RightAngleAxes
  {
    get => this.m_chart.RightAngleAxes;
    set => this.m_chart.RightAngleAxes = value;
  }

  public bool AutoScaling
  {
    get => this.m_chart.AutoScaling;
    set => this.m_chart.AutoScaling = value;
  }

  public bool WallsAndGridlines2D
  {
    get => this.m_chart.WallsAndGridlines2D;
    set => this.m_chart.WallsAndGridlines2D = value;
  }

  public IShapes Shapes => this.m_chart.Shapes;

  public OfficeChartType PivotChartType
  {
    get => this.m_chart.PivotChartType;
    set => this.m_chart.PivotChartType = value;
  }

  public bool ShowAllFieldButtons
  {
    get => this.m_chart.ShowAllFieldButtons;
    set => this.m_chart.ShowAllFieldButtons = value;
  }

  public bool ShowValueFieldButtons
  {
    get => this.m_chart.ShowValueFieldButtons;
    set => this.m_chart.ShowValueFieldButtons = value;
  }

  public bool ShowAxisFieldButtons
  {
    get => this.m_chart.ShowAxisFieldButtons;
    set => this.m_chart.ShowAxisFieldButtons = value;
  }

  public bool ShowLegendFieldButtons
  {
    get => this.m_chart.ShowLegendFieldButtons;
    set => this.m_chart.ShowLegendFieldButtons = value;
  }

  public bool ShowReportFilterFieldButtons
  {
    get => this.m_chart.ShowReportFilterFieldButtons;
    set => this.m_chart.ShowReportFilterFieldButtons = value;
  }

  public OfficeChartType ChartType
  {
    get => this.m_chart.ChartType;
    set => this.m_chart.ChartType = value;
  }

  public IOfficeDataRange DataRange
  {
    get => this.m_chart.DataRange;
    set
    {
    }
  }

  public bool IsSeriesInRows
  {
    get => this.m_chart.IsSeriesInRows;
    set => this.m_chart.IsSeriesInRows = value;
  }

  public string ChartTitle
  {
    get => this.m_chart.ChartTitle;
    set => this.m_chart.ChartTitle = value;
  }

  public IOfficeChartTextArea ChartTitleArea => this.m_chart.ChartTitleArea;

  public string CategoryAxisTitle
  {
    get => this.m_chart.CategoryAxisTitle;
    set => this.m_chart.CategoryAxisTitle = value;
  }

  public string ValueAxisTitle
  {
    get => this.m_chart.ValueAxisTitle;
    set => this.m_chart.ValueAxisTitle = value;
  }

  public string SecondaryCategoryAxisTitle
  {
    get => this.m_chart.SecondaryCategoryAxisTitle;
    set => this.m_chart.SecondaryCategoryAxisTitle = value;
  }

  public string SecondaryValueAxisTitle
  {
    get => this.m_chart.SecondaryValueAxisTitle;
    set => this.m_chart.SecondaryValueAxisTitle = value;
  }

  public string SeriesAxisTitle
  {
    get => this.m_chart.SeriesAxisTitle;
    set => this.m_chart.SeriesAxisTitle = value;
  }

  public IOfficeChartPageSetup PageSetup => this.m_chart.PageSetup;

  public double XPos
  {
    get => this.m_chart.XPos;
    set => this.m_chart.XPos = value;
  }

  public double YPos
  {
    get => this.m_chart.YPos;
    set => this.m_chart.YPos = value;
  }

  double IOfficeChart.Width
  {
    get => this.m_chart.Width;
    set => this.m_chart.Width = value;
  }

  double IOfficeChart.Height
  {
    get => this.m_chart.Height;
    set => this.m_chart.Height = value;
  }

  public IOfficeChartSeries Series => this.m_chart.Series;

  public IOfficeChartCategoryAxis PrimaryCategoryAxis => this.m_chart.PrimaryCategoryAxis;

  public IOfficeChartValueAxis PrimaryValueAxis => this.m_chart.PrimaryValueAxis;

  public IOfficeChartSeriesAxis PrimarySerieAxis => this.m_chart.PrimarySerieAxis;

  public IOfficeChartCategoryAxis SecondaryCategoryAxis => this.m_chart.SecondaryCategoryAxis;

  public IOfficeChartValueAxis SecondaryValueAxis => this.m_chart.SecondaryValueAxis;

  public IOfficeChartFrameFormat ChartArea => this.m_chart.ChartArea;

  public IOfficeChartFrameFormat PlotArea => this.m_chart.PlotArea;

  public ChartFormatCollection PrimaryFormats => this.m_chart.PrimaryFormats;

  public ChartFormatCollection SecondaryFormats => this.m_chart.SecondaryFormats;

  public IOfficeChartShapes Charts => throw new NotSupportedException();

  public OfficeKnownColors TabColor
  {
    get => throw new NotSupportedException();
    set => throw new NotSupportedException();
  }

  public bool IsRightToLeft
  {
    get => this.m_chart.IsRightToLeft;
    set => this.m_chart.IsRightToLeft = value;
  }

  public Color TabColorRGB
  {
    get => throw new NotSupportedException();
    set => throw new NotSupportedException();
  }

  public IOfficeChartWallOrFloor Walls => this.m_chart.Walls;

  public IOfficeChartWallOrFloor SideWall => this.m_chart.SideWall;

  public IOfficeChartWallOrFloor BackWall => this.m_chart.Walls;

  public IOfficeChartWallOrFloor Floor => this.m_chart.Floor;

  public IOfficeChartDataTable DataTable => this.m_chart.DataTable;

  public bool IsSelected => throw new NotSupportedException();

  public bool HasDataTable
  {
    get => this.m_chart.HasDataTable;
    set => this.m_chart.HasDataTable = value;
  }

  public bool HasLegend
  {
    get => this.m_chart.HasLegend;
    set => this.m_chart.HasLegend = value;
  }

  public bool HasTitle
  {
    get => this.m_chart.HasTitle;
    set => this.m_chart.HasTitle = value;
  }

  public IOfficeChartLegend Legend => this.m_chart.Legend;

  public bool HasPlotArea
  {
    get => this.m_chart.HasPlotArea;
    set => this.m_chart.HasPlotArea = value;
  }

  public int TabIndex
  {
    get => throw new NotSupportedException("This property is not supported for embedded charts.");
  }

  public OfficeWorksheetVisibility Visibility
  {
    get => throw new NotSupportedException();
    set => throw new NotSupportedException();
  }

  public void Activate() => throw new NotSupportedException();

  public void Select() => throw new NotSupportedException();

  public void Unselect() => throw new NotSupportedException();

  public OfficeChartPlotEmpty DisplayBlanksAs
  {
    get => this.m_chart.DisplayBlanksAs;
    set => this.m_chart.DisplayBlanksAs = value;
  }

  public bool PlotVisibleOnly
  {
    get => this.m_chart.PlotVisibleOnly;
    set => this.m_chart.PlotVisibleOnly = value;
  }

  public bool SizeWithWindow
  {
    get => this.m_chart.SizeWithWindow;
    set => this.m_chart.SizeWithWindow = value;
  }

  public ITextBoxes TextBoxes => this.m_chart.TextBoxes;

  public string CodeName => this.m_chart.CodeName;

  public bool ProtectContents => throw new NotSupportedException();

  public bool ProtectDrawingObjects => throw new NotSupportedException();

  public bool ProtectScenarios => throw new NotSupportedException();

  public OfficeSheetProtection Protection => throw new NotSupportedException();

  public bool IsPasswordProtected => throw new NotSupportedException();

  public void Protect(string password) => throw new NotSupportedException();

  public void Protect(string password, OfficeSheetProtection options)
  {
    throw new NotSupportedException();
  }

  public void Unprotect(string password) => throw new NotSupportedException();

  public void SaveAsImage(Stream imageAsStream)
  {
    (this.Application.ChartToImageConverter ?? throw new ArgumentException("IApplication.ChartToImageConverter must be instantiated")).SaveAsImage((IOfficeChart) this, imageAsStream);
  }

  public override IShape Clone(
    object parent,
    Dictionary<string, string> hashNewNames,
    Dictionary<int, int> dicFontIndexes,
    bool addToCollections)
  {
    ChartShapeImpl newShape = new ChartShapeImpl(this.Application, parent, this, hashNewNames, dicFontIndexes);
    WorksheetBaseImpl parent1 = CommonObject.FindParent(newShape.Parent, typeof (WorksheetBaseImpl), true) as WorksheetBaseImpl;
    if (addToCollections)
      parent1.InnerShapes.AddShape((ShapeImpl) newShape);
    return (IShape) newShape;
  }

  public override void UpdateFormula(
    int iCurIndex,
    int iSourceIndex,
    Rectangle sourceRect,
    int iDestIndex,
    Rectangle destRect)
  {
    this.m_chart.UpdateFormula(iCurIndex, iSourceIndex, sourceRect, iDestIndex, destRect);
  }

  public override void RegisterInSubCollection()
  {
    this.m_shapes.WorksheetBase.InnerCharts.InnerAddChart((IOfficeChartShape) this);
  }

  protected override void OnPrepareForSerialization()
  {
    if (this.m_shape == null)
      this.m_shape = (MsofbtSp) MsoFactory.GetRecord(MsoRecords.msofbtSp);
    this.m_shape.Version = 2;
    this.m_shape.Instance = 201;
    this.m_shape.IsHaveAnchor = true;
    this.m_shape.IsHaveSpt = true;
  }

  [CLSCompliant(false)]
  protected override void ParseClientData(MsofbtClientData clientData, OfficeParseOptions options)
  {
    base.ParseClientData(clientData, options);
    int iPos = 1;
    this.m_chart = new ChartImpl(this.Application, (object) this, (IList) clientData.AdditionalData, ref iPos, options);
    if ((options & OfficeParseOptions.DoNotParseCharts) != OfficeParseOptions.Default)
      return;
    this.m_chart.Parse();
  }

  [CLSCompliant(false)]
  protected override void SerializeShape(MsofbtSpgrContainer spgrContainer)
  {
    if (spgrContainer == null)
      throw new ArgumentNullException(nameof (spgrContainer));
    MsofbtSpContainer record1 = (MsofbtSpContainer) MsoFactory.GetRecord(MsoRecords.msofbtSpContainer);
    MsofbtClientAnchor itemToAdd1 = (MsofbtClientAnchor) MsoFactory.GetRecord(MsoRecords.msofbtClientAnchor);
    MsofbtClientData record2 = (MsofbtClientData) MsoFactory.GetRecord(MsoRecords.msofbtClientData);
    OffsetArrayList records = new OffsetArrayList();
    ftCmo record3;
    if (this.Obj == null)
    {
      OBJRecord record4 = (OBJRecord) BiffRecordFactory.GetRecord(TBIFFRecord.OBJ);
      record3 = new ftCmo();
      record3.ObjectType = TObjType.otChart;
      record3.Printable = true;
      ftEnd record5 = new ftEnd();
      record4.AddSubRecord((ObjSubRecord) record3);
      record4.AddSubRecord((ObjSubRecord) record5);
      this.SetObject(record4);
    }
    else
      record3 = this.Obj.RecordsList[0] as ftCmo;
    record3.ID = this.OldObjId > 0U ? (ushort) this.OldObjId : (ushort) this.ParentWorkbook.CurrentObjectId;
    record2.AddRecord((BiffRecordRaw) this.Obj);
    this.m_chart.EMUWidth = ApplicationImpl.ConvertFromPixel((double) this.Width, MeasureUnits.Point);
    this.m_chart.EMUHeight = ApplicationImpl.ConvertFromPixel((double) this.Height, MeasureUnits.Point);
    this.m_chart.Serialize(records);
    record2.AddRecordRange((IList) records);
    if (this.ClientAnchor == null)
    {
      itemToAdd1.Options = (ushort) 3;
      itemToAdd1.LeftColumn = this.m_iLeftColumn;
      itemToAdd1.RightColumn = this.m_iRightColumn;
      itemToAdd1.TopRow = this.m_iTopRow;
      itemToAdd1.BottomRow = this.m_iBottomRow;
      itemToAdd1.LeftOffset = 0;
      itemToAdd1.RightOffset = 0;
      itemToAdd1.TopOffset = 0;
      itemToAdd1.BottomOffset = 0;
    }
    else
      itemToAdd1 = this.ClientAnchor;
    record1.AddItem((MsoBase) this.m_shape);
    MsofbtOPT itemToAdd2 = this.SerializeOptions((MsoBase) record1);
    itemToAdd2.Version = 3;
    itemToAdd2.Instance = 8;
    if (itemToAdd2.Properties.Length > 0)
      record1.AddItem((MsoBase) itemToAdd2);
    record1.AddItem((MsoBase) itemToAdd1);
    record1.AddItem((MsoBase) record2);
    spgrContainer.AddItem((MsoBase) record1);
  }

  [CLSCompliant(false)]
  public override void ParseClientAnchor(MsofbtClientAnchor clientAnchor)
  {
    base.ParseClientAnchor(clientAnchor);
    this.m_iBottomRow = clientAnchor.BottomRow;
    this.m_iTopRow = clientAnchor.TopRow;
    this.m_iLeftColumn = clientAnchor.LeftColumn;
    this.m_iRightColumn = clientAnchor.RightColumn;
  }

  [CLSCompliant(false)]
  protected override MsofbtOPT SerializeOptions(MsoBase parent)
  {
    if (!this.m_bUpdateLineFill && this.m_options != null)
      return this.m_options;
    MsofbtOPT options = base.SerializeOptions(parent);
    this.SerializeSizeTextToFit(options);
    this.SerializeOptionSorted(options, MsoOptions.ForeColor, 134217806U /*0x0800004E*/);
    this.SerializeOptionSorted(options, MsoOptions.BackColor, 134217805U /*0x0800004D*/);
    this.SerializeHitTest(options);
    this.SerializeOptionSorted(options, MsoOptions.LineColor, 134217805U /*0x0800004D*/);
    this.SerializeOptionSorted(options, MsoOptions.NoLineDrawDash, 524296U /*0x080008*/);
    this.SerializeOptionSorted(options, MsoOptions.ShadowObscured, 131072U /*0x020000*/);
    this.SerializeShapeName(options);
    return options;
  }

  [CLSCompliant(false)]
  protected override MsofbtOPT CreateDefaultOptions()
  {
    MsofbtOPT defaultOptions = base.CreateDefaultOptions();
    defaultOptions.Version = 3;
    defaultOptions.Instance = 8;
    this.SerializeOption(defaultOptions, MsoOptions.LockAgainstGrouping, 17039620U);
    return defaultOptions;
  }

  protected override void SetParents()
  {
    base.SetParents();
    this.m_worksheet = this.m_shapes.WorksheetBase;
    this.m_worksheet.InnerCharts.InnerAddChart((IOfficeChartShape) this);
  }

  public static implicit operator WorksheetBaseImpl(ChartShapeImpl chartShape)
  {
    return (WorksheetBaseImpl) chartShape.ChartObject;
  }

  public void SetChartData(object[][] data) => this.ChartData.SetChartData(data);

  public void SetDataRange(object[][] data, int rowIndex, int columnIndex)
  {
    this.ChartData.SetDataRange(data, rowIndex, columnIndex);
  }

  public void SetDataRange(IEnumerable enumerable, int rowIndex, int columnIndex)
  {
    this.ChartData.SetDataRange(enumerable, rowIndex, columnIndex);
  }
}
