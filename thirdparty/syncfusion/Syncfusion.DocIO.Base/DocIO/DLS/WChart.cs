// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WChart
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.Compression.Zip;
using Syncfusion.DocIO.Rendering;
using Syncfusion.Layouting;
using Syncfusion.OfficeChart;
using Syncfusion.OfficeChart.Implementation;
using Syncfusion.OfficeChart.Implementation.Charts;
using Syncfusion.OfficeChart.Implementation.XmlSerialization;
using System.Collections;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WChart : ShapeBase, IEntity, ILeafWidget, IWidget
{
  private IOfficeChart m_officeChart;
  private string m_externalDataPath = string.Empty;
  private string m_internalDataPath = string.Empty;
  private string m_userShapes = string.Empty;
  private bool m_isExternalRelation;

  public IOfficeChart OfficeChart
  {
    get
    {
      if (this.m_officeChart == null)
        this.m_officeChart = (IOfficeChart) new ChartImpl();
      ((WorkbookImpl) (this.m_officeChart as ChartImpl).Workbook).IsWorkbookOpening = this.Document.IsOpening;
      return this.m_officeChart;
    }
    internal set => this.m_officeChart = value;
  }

  internal WorkbookImpl Workbook => (this.OfficeChart as ChartImpl).Workbook as WorkbookImpl;

  internal string InternalDataPath
  {
    get => this.m_internalDataPath;
    set => this.m_internalDataPath = value;
  }

  internal string UserShapes
  {
    get => this.m_userShapes;
    set => this.m_userShapes = value;
  }

  internal bool IsExternalRelation
  {
    get => this.m_isExternalRelation;
    set => this.m_isExternalRelation = value;
  }

  public OfficeChartType ChartType
  {
    get => this.OfficeChart.ChartType;
    set => this.OfficeChart.ChartType = value;
  }

  public string ExternalDataPath
  {
    get => this.m_externalDataPath;
    set
    {
      this.m_externalDataPath = value;
      this.IsExternalRelation = true;
    }
  }

  public IOfficeDataRange DataRange
  {
    get => this.OfficeChart.DataRange;
    set => this.OfficeChart.DataRange = value;
  }

  public bool IsSeriesInRows
  {
    get => this.OfficeChart.IsSeriesInRows;
    set => this.OfficeChart.IsSeriesInRows = value;
  }

  public string ChartTitle
  {
    get => this.OfficeChart.ChartTitle;
    set => this.OfficeChart.ChartTitle = value;
  }

  public IOfficeChartTextArea ChartTitleArea => this.OfficeChart.ChartTitleArea;

  public IOfficeChartSeries Series => this.OfficeChart.Series;

  public IOfficeChartCategoryAxis PrimaryCategoryAxis => this.OfficeChart.PrimaryCategoryAxis;

  public IOfficeChartValueAxis PrimaryValueAxis => this.OfficeChart.PrimaryValueAxis;

  public IOfficeChartSeriesAxis PrimarySeriesAxis => this.OfficeChart.PrimarySerieAxis;

  public IOfficeChartCategoryAxis SecondaryCategoryAxis => this.OfficeChart.SecondaryCategoryAxis;

  public IOfficeChartValueAxis SecondaryValueAxis => this.OfficeChart.SecondaryValueAxis;

  public IOfficeChartFrameFormat ChartArea => this.OfficeChart.ChartArea;

  public IOfficeChartFrameFormat PlotArea => this.OfficeChart.PlotArea;

  public IOfficeChartWallOrFloor Walls => this.OfficeChart.Walls;

  public IOfficeChartWallOrFloor SideWall => this.OfficeChart.SideWall;

  public IOfficeChartWallOrFloor BackWall => this.OfficeChart.BackWall;

  public IOfficeChartWallOrFloor Floor => this.OfficeChart.Floor;

  public IOfficeChartDataTable DataTable => this.OfficeChart.DataTable;

  public bool HasDataTable
  {
    get => this.OfficeChart.HasDataTable;
    set => this.OfficeChart.HasDataTable = value;
  }

  public IOfficeChartLegend Legend => this.OfficeChart.Legend;

  public bool HasLegend
  {
    get => this.OfficeChart.HasLegend;
    set => this.OfficeChart.HasLegend = value;
  }

  public int Rotation
  {
    get => this.OfficeChart.Rotation;
    set => this.OfficeChart.Rotation = value;
  }

  public int Elevation
  {
    get => this.OfficeChart.Elevation;
    set => this.OfficeChart.Elevation = value;
  }

  public int Perspective
  {
    get => this.OfficeChart.Perspective;
    set => this.OfficeChart.Perspective = value;
  }

  public int HeightPercent
  {
    get => this.OfficeChart.HeightPercent;
    set => this.OfficeChart.HeightPercent = value;
  }

  public int DepthPercent
  {
    get => this.OfficeChart.DepthPercent;
    set => this.OfficeChart.DepthPercent = value;
  }

  public int GapDepth
  {
    get => this.OfficeChart.GapDepth;
    set => this.OfficeChart.GapDepth = value;
  }

  public bool RightAngleAxes
  {
    get => this.OfficeChart.RightAngleAxes;
    set => this.OfficeChart.RightAngleAxes = value;
  }

  public bool AutoScaling
  {
    get => this.OfficeChart.AutoScaling;
    set => this.OfficeChart.AutoScaling = value;
  }

  public bool HasPlotArea
  {
    get => this.OfficeChart.HasPlotArea;
    set => this.OfficeChart.HasPlotArea = value;
  }

  public OfficeChartPlotEmpty DisplayBlanksAs
  {
    get => this.OfficeChart.DisplayBlanksAs;
    set => this.OfficeChart.DisplayBlanksAs = value;
  }

  public bool PlotVisibleOnly
  {
    get => this.OfficeChart.PlotVisibleOnly;
    set => this.OfficeChart.PlotVisibleOnly = value;
  }

  public IOfficeChartCategories Categories => this.OfficeChart.Categories;

  public OfficeSeriesNameLevel SeriesNameLevel
  {
    get => this.OfficeChart.SeriesNameLevel;
    set => this.OfficeChart.SeriesNameLevel = value;
  }

  public OfficeCategoriesLabelLevel CategoryLabelLevel
  {
    get => this.OfficeChart.CategoryLabelLevel;
    set => this.OfficeChart.CategoryLabelLevel = value;
  }

  public IOfficeChartData ChartData => this.OfficeChart.ChartData;

  public override EntityType EntityType => EntityType.Chart;

  internal override void AttachToParagraph(WParagraph paragraph, int itemPos)
  {
    base.AttachToParagraph(paragraph, itemPos);
    if (!this.DeepDetached)
      this.IsCloned = false;
    else
      this.IsCloned = true;
    if (this.GetTextWrappingStyle() == TextWrappingStyle.Inline)
      return;
    this.Document.FloatingItems.Add((Entity) this);
  }

  internal override void Detach()
  {
    base.Detach();
    this.Document.FloatingItems.Remove((Entity) this);
  }

  protected override object CloneImpl()
  {
    WChart wchart = (WChart) base.CloneImpl();
    wchart.OfficeChart = (this.OfficeChart as ChartImpl).Clone();
    wchart.IsCloned = true;
    return (object) wchart;
  }

  internal override void CloneRelationsTo(WordDocument doc, OwnerHolder nextOwner)
  {
    base.CloneRelationsTo(doc, nextOwner);
  }

  internal override void Close()
  {
    if (this.m_officeChart != null)
    {
      (this.m_officeChart as ChartImpl).Close();
      this.m_officeChart = (IOfficeChart) null;
    }
    base.Close();
  }

  public WChart(WordDocument doc)
    : base(doc)
  {
    this.m_officeChart = (IOfficeChart) new ChartImpl();
    this.Workbook.IsWorkbookOpening = doc.IsOpening;
    (this.m_officeChart as ChartImpl).SetDefaultProperties();
    this.CreateDataHolder();
    this.WrapFormat.SetTextWrappingStyleValue(TextWrappingStyle.Inline);
    this.VerticalAlignment = ShapeVerticalAlignment.None;
    this.HorizontalAlignment = ShapeHorizontalAlignment.None;
    this.m_charFormat = new WCharacterFormat((IWordDocument) doc, (Entity) this);
  }

  internal void CreateDataHolder()
  {
    if (this.Workbook.DataHolder == null)
    {
      this.Workbook.DataHolder = new FileDataHolder(this.Workbook);
      ZipArchiveItem zipArchiveItem = new ZipArchive().AddItem(string.Empty, (Stream) null, false, FileAttributes.Archive);
      if (this.Workbook.ActiveSheet != null)
      {
        (this.Workbook.ActiveSheet as WorksheetImpl).DataHolder = new WorksheetDataHolder(this.Workbook.DataHolder, zipArchiveItem);
        (this.Workbook.ActiveSheet as WorksheetImpl).IsParsed = true;
      }
    }
    if (this.Workbook.ActiveSheet == null)
      return;
    (this.m_officeChart as ChartImpl).DataHolder = (this.Workbook.ActiveSheet as WorksheetImpl).DataHolder;
  }

  internal void SetDataRange(int sheetNumber, string dataRange)
  {
    (this.m_officeChart as ChartImpl).DataIRange = (this.Workbook.Worksheets[sheetNumber - 1] as WorksheetImpl).Range[dataRange];
  }

  internal void InitializeOfficeChartToImageConverter()
  {
    (this.OfficeChart as ChartImpl).Application.ChartToImageConverter = this.Document.ChartToImageConverter;
  }

  public void SetChartData(object[][] data) => this.OfficeChart.SetChartData(data);

  public void SetDataRange(object[][] data, int rowIndex, int columnIndex)
  {
    this.OfficeChart.SetDataRange(data, rowIndex, columnIndex);
  }

  public void SetDataRange(IEnumerable enumerable, int rowIndex, int columnIndex)
  {
    this.OfficeChart.SetDataRange(enumerable, rowIndex, columnIndex);
  }

  public void Refresh()
  {
    if (this.m_officeChart == null)
      return;
    (this.m_officeChart as ChartImpl).Refresh();
  }

  protected override void CreateLayoutInfo()
  {
    this.m_layoutInfo = (ILayoutInfo) new LayoutInfo(ChildrenLayoutDirection.Horizontal);
    WParagraph wparagraph = this.OwnerParagraph;
    if (this.Owner is InlineContentControl || this.Owner is XmlParagraphItem)
      wparagraph = this.GetOwnerParagraphValue();
    if (wparagraph.IsInCell && ((IWidget) wparagraph).LayoutInfo.IsClipped)
      this.m_layoutInfo.IsClipped = true;
    if (this.WrapFormat.TextWrappingStyle != TextWrappingStyle.Inline)
      this.m_layoutInfo.IsSkipBottomAlign = true;
    if (this.ParaItemCharFormat.HasValue(53))
      this.m_layoutInfo.IsSkip = true;
    if (!this.IsDeleteRevision || this.Document.RevisionOptions.ShowDeletedText)
      return;
    this.m_layoutInfo.IsSkip = true;
  }

  internal override void InitLayoutInfo(Entity entity, ref bool isLastTOCEntry)
  {
    this.m_layoutInfo = (ILayoutInfo) null;
    if (this != entity)
      return;
    isLastTOCEntry = true;
  }

  SizeF ILeafWidget.Measure(DrawingContext dc) => new SizeF(this.Width, this.Height);
}
