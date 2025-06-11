// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Charts.PresentationChart
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Compression.Zip;
using Syncfusion.OfficeChart;
using Syncfusion.OfficeChart.Implementation;
using Syncfusion.OfficeChart.Implementation.Charts;
using Syncfusion.OfficeChart.Implementation.XmlReaders;
using Syncfusion.OfficeChart.Implementation.XmlSerialization;
using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.SlideImplementation;
using System.Collections;
using System.IO;

#nullable disable
namespace Syncfusion.Presentation.Charts;

internal class PresentationChart : Shape, IPresentationChart
{
  private ChartImpl _chartImpl;
  private string _relationId;
  private Syncfusion.Presentation.RelationCollection _topRelation;
  private bool _isParsedChart;
  private bool _useExcelDataRange;
  private bool _chartDataRangeSet;
  private int _workSheetIndex;
  private bool _ischartEx;
  private Stream m_embeddedWorkbookStream;

  internal PresentationChart(BaseSlide baseSlide)
    : this(baseSlide, false)
  {
  }

  internal PresentationChart(BaseSlide baseSlide, bool isWorkbookOpen)
    : base(ShapeType.Chart, baseSlide)
  {
    this._chartDataRangeSet = true;
    this._chartImpl = new ChartImpl();
    ((WorkbookImpl) this._chartImpl.Workbook).IsWorkbookOpening = isWorkbookOpen;
    Excel2007Parser.m_isPresentation = true;
    this._chartImpl.SetDefaultProperties();
    this.DrawingType = DrawingType.Chart;
    this._topRelation = new Syncfusion.Presentation.RelationCollection();
    this.CreateDataHolder();
  }

  public IOfficeChart OfficeChart => (IOfficeChart) this.GetChartImpl();

  public OfficeChartType ChartType
  {
    get => this._chartImpl.ChartType;
    set => this._chartImpl.ChartType = value;
  }

  internal bool IsChartEx
  {
    set => this._ischartEx = value;
    get => this._ischartEx;
  }

  public IOfficeDataRange DataRange
  {
    get => this._chartImpl.DataRange;
    set => this._chartImpl.DataRange = value;
  }

  public bool IsSeriesInRows
  {
    get => this._chartImpl.IsSeriesInRows;
    set => this._chartImpl.IsSeriesInRows = value;
  }

  public string ChartTitle
  {
    get => this._chartImpl.ChartTitle;
    set => this._chartImpl.ChartTitle = value;
  }

  public IOfficeChartTextArea ChartTitleArea => this._chartImpl.ChartTitleArea;

  public IOfficeChartPageSetup PageSetup => this._chartImpl.PageSetup;

  public double XPos
  {
    get => this._chartImpl.XPos;
    set
    {
      this._chartImpl.XPos = value;
      this.Left = this._chartImpl.XPos;
    }
  }

  public double YPos
  {
    get => this._chartImpl.YPos;
    set
    {
      this._chartImpl.YPos = value;
      this.Top = this._chartImpl.YPos;
    }
  }

  public IOfficeChartSeries Series => this._chartImpl.Series;

  public IOfficeChartCategoryAxis PrimaryCategoryAxis => this._chartImpl.PrimaryCategoryAxis;

  public IOfficeChartValueAxis PrimaryValueAxis => this._chartImpl.PrimaryValueAxis;

  public IOfficeChartSeriesAxis PrimarySerieAxis => this._chartImpl.PrimarySerieAxis;

  public IOfficeChartCategoryAxis SecondaryCategoryAxis => this._chartImpl.SecondaryCategoryAxis;

  public IOfficeChartValueAxis SecondaryValueAxis => this._chartImpl.SecondaryValueAxis;

  public IOfficeChartFrameFormat ChartArea => this._chartImpl.ChartArea;

  public IOfficeChartFrameFormat PlotArea => this._chartImpl.PlotArea;

  public IOfficeChartWallOrFloor Walls => this._chartImpl.Walls;

  public IOfficeChartWallOrFloor SideWall => this._chartImpl.SideWall;

  public IOfficeChartWallOrFloor BackWall => this._chartImpl.BackWall;

  public IOfficeChartWallOrFloor Floor => this._chartImpl.Floor;

  public IOfficeChartDataTable DataTable => this._chartImpl.DataTable;

  public bool HasDataTable
  {
    get => this._chartImpl.HasDataTable;
    set => this._chartImpl.HasDataTable = value;
  }

  public IOfficeChartLegend Legend => this._chartImpl.Legend;

  public bool HasLegend
  {
    get => this._chartImpl.HasLegend;
    set => this._chartImpl.HasLegend = value;
  }

  public new int Rotation
  {
    get => this._chartImpl.Rotation;
    set => this._chartImpl.Rotation = value;
  }

  public int Elevation
  {
    get => this._chartImpl.Elevation;
    set => this._chartImpl.Elevation = value;
  }

  public int Perspective
  {
    get => this._chartImpl.Perspective;
    set => this._chartImpl.Perspective = value;
  }

  public int HeightPercent
  {
    get => this._chartImpl.HeightPercent;
    set => this._chartImpl.HeightPercent = value;
  }

  public int DepthPercent
  {
    get => this._chartImpl.DepthPercent;
    set => this._chartImpl.DepthPercent = value;
  }

  public int GapDepth
  {
    get => this._chartImpl.GapDepth;
    set => this._chartImpl.GapDepth = value;
  }

  public bool RightAngleAxes
  {
    get => this._chartImpl.RightAngleAxes;
    set => this._chartImpl.RightAngleAxes = value;
  }

  public bool AutoScaling
  {
    get => this._chartImpl.AutoScaling;
    set => this._chartImpl.AutoScaling = value;
  }

  public bool WallsAndGridlines2D
  {
    get => this._chartImpl.WallsAndGridlines2D;
    set => this._chartImpl.WallsAndGridlines2D = value;
  }

  public bool HasPlotArea
  {
    get => this._chartImpl.HasPlotArea;
    set => this._chartImpl.HasPlotArea = value;
  }

  public OfficeChartPlotEmpty DisplayBlanksAs
  {
    get => this._chartImpl.DisplayBlanksAs;
    set => this._chartImpl.DisplayBlanksAs = value;
  }

  public bool PlotVisibleOnly
  {
    get => this._chartImpl.PlotVisibleOnly;
    set => this._chartImpl.PlotVisibleOnly = value;
  }

  public bool SizeWithWindow
  {
    get => this._chartImpl.SizeWithWindow;
    set => this._chartImpl.SizeWithWindow = value;
  }

  public OfficeChartType PivotChartType
  {
    get => this._chartImpl.PivotChartType;
    set => this._chartImpl.PivotChartType = value;
  }

  public bool ShowAllFieldButtons
  {
    get => this._chartImpl.ShowAllFieldButtons;
    set => this._chartImpl.ShowAllFieldButtons = value;
  }

  public bool ShowValueFieldButtons
  {
    get => this._chartImpl.ShowValueFieldButtons;
    set => this._chartImpl.ShowValueFieldButtons = value;
  }

  public bool ShowAxisFieldButtons
  {
    get => this._chartImpl.ShowAxisFieldButtons;
    set => this._chartImpl.ShowAxisFieldButtons = value;
  }

  public bool ShowLegendFieldButtons
  {
    get => this._chartImpl.ShowLegendFieldButtons;
    set => this._chartImpl.ShowLegendFieldButtons = value;
  }

  public bool ShowReportFilterFieldButtons
  {
    get => this._chartImpl.ShowReportFilterFieldButtons;
    set => this._chartImpl.ShowReportFilterFieldButtons = value;
  }

  public IOfficeChartCategories Categories => this._chartImpl.Categories;

  public OfficeSeriesNameLevel SeriesNameLevel
  {
    get => this._chartImpl.SeriesNameLevel;
    set => this._chartImpl.SeriesNameLevel = value;
  }

  public OfficeCategoriesLabelLevel CategoryLabelLevel
  {
    get => this._chartImpl.CategoryLabelLevel;
    set => this._chartImpl.CategoryLabelLevel = value;
  }

  public int Style
  {
    get => this._chartImpl.Style;
    set => this._chartImpl.Style = value;
  }

  public void SaveAsImage(Stream imageAsStream) => this.GetChartImpl().SaveAsImage(imageAsStream);

  public IOfficeChartData ChartData => this._chartImpl.ChartData;

  public void SetChartData(object[][] data) => this._chartImpl.SetChartData(data);

  internal void SetDataRange(int sheetNumber, string dataRange)
  {
    this._workSheetIndex = sheetNumber - 1;
    WorksheetImpl worksheet = this.Workbook.Worksheets[this._workSheetIndex] as WorksheetImpl;
    this._chartImpl.ActiveSheetIndex = this._workSheetIndex;
    (this._chartImpl.Workbook as WorkbookImpl).ActiveSheet = (IWorksheet) worksheet;
    this._chartImpl.DataIRange = worksheet.Range[dataRange];
    this._chartDataRangeSet = true;
  }

  public void SetDataRange(object[][] data, int rowIndex, int columnIndex)
  {
    this._chartImpl.SetDataRange(data, rowIndex, columnIndex);
  }

  public void SetDataRange(IEnumerable enumerable, int rowIndex, int columnIndex)
  {
    this._chartImpl.SetDataRange(enumerable, rowIndex, columnIndex);
  }

  internal bool UseExcelDataRange
  {
    get => this._useExcelDataRange;
    set => this._useExcelDataRange = value;
  }

  public void Refresh()
  {
    this.UseExcelDataRange = true;
    this._chartImpl.Refresh();
  }

  internal bool ChartDataRangeSet
  {
    get => this._chartDataRangeSet;
    set => this._chartDataRangeSet = value;
  }

  internal int WorkSheetIndex
  {
    get => this._workSheetIndex;
    set => this._workSheetIndex = value;
  }

  internal string RelationId
  {
    get => this._relationId;
    set => this._relationId = value;
  }

  internal Syncfusion.Presentation.RelationCollection TopRelation
  {
    get => this._topRelation;
    set => this._topRelation = value;
  }

  internal WorkbookImpl Workbook => this._chartImpl.Workbook as WorkbookImpl;

  private WorksheetImpl Worksheet => this.Workbook.ActiveSheet as WorksheetImpl;

  internal bool IsParsedChart
  {
    get => this._isParsedChart;
    set => this._isParsedChart = value;
  }

  internal void CreateDataHolder()
  {
    if (this.Workbook.DataHolder == null)
    {
      this.Workbook.DataHolder = new Syncfusion.OfficeChart.Implementation.XmlSerialization.FileDataHolder(this.Workbook);
      this.Worksheet.DataHolder = new WorksheetDataHolder(this.Workbook.DataHolder, new ZipArchive().AddItem(string.Empty, (Stream) null, false, FileAttributes.Archive));
      this.Worksheet.IsParsed = true;
    }
    this._chartImpl.DataHolder = this.Worksheet.DataHolder;
  }

  internal ChartImpl GetChartImpl()
  {
    if (this._chartImpl.Application.ChartToImageConverter == null)
      this._chartImpl.Application.ChartToImageConverter = this.BaseSlide.Presentation.ChartToImageConverter;
    return this._chartImpl;
  }

  public override ISlideItem Clone()
  {
    PresentationChart presentationChart = (PresentationChart) this.MemberwiseClone();
    this.Clone((Shape) presentationChart);
    if (this._chartImpl != null)
      presentationChart._chartImpl = (ChartImpl) this._chartImpl.Clone();
    if (this.m_embeddedWorkbookStream != null)
      presentationChart.m_embeddedWorkbookStream = CloneUtils.CloneStream(this.m_embeddedWorkbookStream);
    presentationChart._topRelation = this._topRelation.Clone();
    return (ISlideItem) presentationChart;
  }

  internal override void Close()
  {
    if (this._chartImpl != null)
    {
      this._chartImpl.Close();
      this._chartImpl = (ChartImpl) null;
    }
    this._topRelation.Close();
    base.Close();
  }

  internal Stream EmbeddedWorkbookStream
  {
    get => this.m_embeddedWorkbookStream;
    set => this.m_embeddedWorkbookStream = value;
  }
}
