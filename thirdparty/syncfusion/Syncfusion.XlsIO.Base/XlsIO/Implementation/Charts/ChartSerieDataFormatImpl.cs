// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Charts.ChartSerieDataFormatImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.Charts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Charts;

public class ChartSerieDataFormatImpl : 
  CommonObject,
  IChartSerieDataFormat,
  IChartFillBorder,
  IFillColor
{
  private const ushort DEF_NONE_COLOR = 78;
  private const int DEF_MARKER_SIZE_MUL = 20;
  public const int DEF_MARKER_START_COLOR = 24;
  private const string DEF_PIE_START_TYPE = "Pie";
  private const string DEF_DOUGHNUT_START_TYPE = "Doughnut";
  private const string DEF_SURFACE_START_TYPE = "Surface";
  public const string DEF_LINE_START_TYPE = "Line";
  public const string DEF_SCATTER_START_TYPE = "Scatter";
  private const int DEF_MARKER_LINE_SIZE = 60;
  private const int DEF_LINE_SIZE = 5;
  private const int DEF_LINE_COLOR = 8388608 /*0x800000*/;
  private const int DEF_MARKER_INDEX = 32 /*0x20*/;
  private const ExcelKnownColors DEF_MARKER_COLOR_INDEX = ExcelKnownColors.YellowCustom | ExcelKnownColors.BlackCustom;
  public static ExcelChartType[] DEF_SUPPORT_DATAFORMAT_PROPERTIES = new ExcelChartType[28]
  {
    ExcelChartType.Bar_Clustered_3D,
    ExcelChartType.Bar_Stacked_100_3D,
    ExcelChartType.Bar_Stacked_3D,
    ExcelChartType.Column_3D,
    ExcelChartType.Column_Clustered_3D,
    ExcelChartType.Column_Stacked_100_3D,
    ExcelChartType.Column_Stacked_3D,
    ExcelChartType.Cone_Bar_Clustered,
    ExcelChartType.Cone_Bar_Stacked,
    ExcelChartType.Cone_Bar_Stacked_100,
    ExcelChartType.Cone_Clustered,
    ExcelChartType.Cone_Clustered_3D,
    ExcelChartType.Cone_Stacked,
    ExcelChartType.Cone_Stacked_100,
    ExcelChartType.Cylinder_Bar_Clustered,
    ExcelChartType.Cylinder_Bar_Stacked,
    ExcelChartType.Cylinder_Bar_Stacked_100,
    ExcelChartType.Cylinder_Clustered,
    ExcelChartType.Cylinder_Clustered_3D,
    ExcelChartType.Cylinder_Stacked,
    ExcelChartType.Cylinder_Stacked_100,
    ExcelChartType.Pyramid_Bar_Clustered,
    ExcelChartType.Pyramid_Bar_Stacked,
    ExcelChartType.Pyramid_Bar_Stacked_100,
    ExcelChartType.Pyramid_Clustered,
    ExcelChartType.Pyramid_Clustered_3D,
    ExcelChartType.Pyramid_Stacked,
    ExcelChartType.Pyramid_Stacked_100
  };
  private ChartDataFormatRecord m_dataFormat = (ChartDataFormatRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartDataFormat);
  private Chart3DDataFormatRecord m_3DDataFormat;
  private ChartPieFormatRecord m_pieFormat;
  private ThreeDFormatImpl m_3D;
  private ShadowImpl m_shadow;
  private ChartMarkerFormatRecord m_markerFormat;
  private ChartAttachedLabelRecord m_attachedLabel;
  private UnknownRecord m_startBlock;
  private UnknownRecord m_shapePropsStream;
  private UnknownRecord m_endBlock;
  private ChartAttachedLabelLayoutRecord m_attachedLabelLayout;
  private ChartSerFmtRecord m_seriesFormat;
  private ChartDataPointImpl m_dataPoint;
  private ChartSerieImpl m_serie;
  private ChartFormatImpl m_format;
  private ChartImpl m_chart;
  private ChartBorderImpl m_border;
  private ChartInteriorImpl m_interior;
  private bool m_bFormatted;
  private ChartFillImpl m_fill;
  private ColorObject m_markerBackColor;
  private ColorObject m_markerForeColor;
  private GradientStops m_markerGradient;
  private double m_markerTransparency = 1.0;
  private Stream m_markerLineStream;
  private Stream m_markerEffectList;
  private bool m_HasMarkerProperties;
  private bool m_HasBarShape;
  private bool m_bIsParsed;
  private bool m_markerChanged;
  private bool m_showConnectorLines = true;
  private ExcelTreeMapLabelOption m_treeMapLabelOption = ExcelTreeMapLabelOption.Overlapping;
  private BoxAndWhiskerSerieFormat m_boxAndWhsikerFormat;
  private HistogramAxisFormat m_histogramAxisFormat;
  private double m_markerLineWidth = 0.75;
  private bool m_isAutoMarkerColor;

  public ChartSerieDataFormatImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.SetParents();
    this.m_fill = new ChartFillImpl(application, (object) this);
    if (!this.m_chart.ParentWorkbook.Loading)
      this.SetDefault3DDataFormat();
    if (parent is ChartDataPointImpl chartDataPointImpl && chartDataPointImpl.IsDefault)
      this.m_boxAndWhsikerFormat.Options = (byte) 14;
    this.InitializeColors();
  }

  private void InitializeColors()
  {
    this.m_markerForeColor = new ColorObject(ColorExtension.Empty);
    this.m_markerForeColor.AfterChange += new ColorObject.AfterChangeHandler(this.MarkerForeColorChanged);
    this.m_markerBackColor = new ColorObject(ColorExtension.Empty);
    this.m_markerBackColor.AfterChange += new ColorObject.AfterChangeHandler(this.MarkerBackColorChanged);
  }

  internal void SetParents()
  {
    this.m_chart = this.FindParent(typeof (ChartImpl)) as ChartImpl;
    object parent = this.FindParent(new Type[2]
    {
      typeof (ChartSerieImpl),
      typeof (ChartFormatImpl)
    });
    this.m_serie = parent != null && this.m_chart != null ? parent as ChartSerieImpl : throw new ArgumentNullException("Can't find parent objects.");
    this.m_format = parent as ChartFormatImpl;
    this.m_dataPoint = this.FindParent(typeof (ChartDataPointImpl)) as ChartDataPointImpl;
    if (!(this.m_format != (ChartFormatImpl) null) || this.m_chart.TypeChanging)
      return;
    this.UpdateSerieFormat();
  }

  [CLSCompliant(false)]
  public int Parse(IList<BiffRecordRaw> arrData, int iPos)
  {
    if (arrData == null)
      throw new ArgumentNullException(nameof (arrData));
    if (iPos < 0 || iPos > arrData.Count)
      throw new ArgumentOutOfRangeException(nameof (iPos), "Value cannot be less than 0 and greater than arrData.Length");
    BiffRecordRaw biffRecordRaw1 = arrData[iPos];
    biffRecordRaw1.CheckTypeCode(TBIFFRecord.ChartDataFormat);
    this.m_dataFormat = (ChartDataFormatRecord) biffRecordRaw1;
    ++iPos;
    arrData[iPos++].CheckTypeCode(TBIFFRecord.Begin);
    BiffRecordRaw biffRecordRaw2 = arrData[iPos];
    int num = 1;
    while (num > 0)
    {
      switch (biffRecordRaw2.TypeCode)
      {
        case TBIFFRecord.StartBlock:
          this.m_startBlock = (UnknownRecord) biffRecordRaw2;
          break;
        case TBIFFRecord.EndBlock:
          this.m_endBlock = (UnknownRecord) biffRecordRaw2;
          break;
        case TBIFFRecord.ChartAttachedLabelLayout:
          this.m_attachedLabelLayout = (ChartAttachedLabelLayoutRecord) biffRecordRaw2;
          break;
        case TBIFFRecord.ShapePropsStream:
          this.m_shapePropsStream = (UnknownRecord) biffRecordRaw2;
          break;
        case TBIFFRecord.ChartLineFormat:
          this.m_border = new ChartBorderImpl(this.Application, (object) this, (ChartLineFormatRecord) biffRecordRaw2);
          this.m_bFormatted = true;
          break;
        case TBIFFRecord.ChartMarkerFormat:
          this.m_markerFormat = (ChartMarkerFormatRecord) biffRecordRaw2;
          this.m_bFormatted = true;
          if (this.m_markerFormat.MarkerType == ExcelChartMarkerType.None)
          {
            this.HasMarkerProperties = true;
            break;
          }
          break;
        case TBIFFRecord.ChartAreaFormat:
          this.m_interior = new ChartInteriorImpl(this.Application, (object) this, (ChartAreaFormatRecord) biffRecordRaw2);
          this.m_bFormatted = true;
          break;
        case TBIFFRecord.ChartPieFormat:
          this.m_pieFormat = (ChartPieFormatRecord) biffRecordRaw2;
          this.m_bFormatted = true;
          break;
        case TBIFFRecord.ChartAttachedLabel:
          this.m_attachedLabel = (ChartAttachedLabelRecord) biffRecordRaw2;
          break;
        case TBIFFRecord.Begin:
          ++num;
          break;
        case TBIFFRecord.End:
          --num;
          break;
        case TBIFFRecord.ChartSerFmt:
          this.m_seriesFormat = (ChartSerFmtRecord) biffRecordRaw2;
          this.m_bFormatted = true;
          break;
        case TBIFFRecord.Chart3DDataFormat:
          this.m_3DDataFormat = (Chart3DDataFormatRecord) biffRecordRaw2;
          this.m_bFormatted = this.ChackDataRecord(this.m_3DDataFormat);
          if ((object) (this.Parent as ChartFormatImpl) != null)
          {
            using (IEnumerator<IChartSerie> enumerator = this.m_chart.Series.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                ChartSerieDataFormatImpl serieFormat = enumerator.Current.SerieFormat as ChartSerieDataFormatImpl;
                if (!serieFormat.m_bFormatted && serieFormat.m_3DDataFormat != null)
                  serieFormat.m_bFormatted = serieFormat.m_3DDataFormat.DataFormatBase != this.m_3DDataFormat.DataFormatBase || serieFormat.m_3DDataFormat.DataFormatTop != this.m_3DDataFormat.DataFormatTop;
              }
              break;
            }
          }
          break;
        case TBIFFRecord.ChartGelFrame:
          this.m_fill = new ChartFillImpl(this.Application, (object) this, (ChartGelFrameRecord) biffRecordRaw2);
          break;
      }
      ++iPos;
      biffRecordRaw2 = arrData[iPos];
    }
    return iPos;
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (this.m_attachedLabel != null)
      this.UpdateSerieFormat();
    IChartDataLabels dataLabels = this.m_dataPoint == null || !this.m_dataPoint.HasDataLabels ? (IChartDataLabels) null : this.m_dataPoint.DataLabels;
    bool flag = dataLabels != null && (dataLabels.IsSeriesName || dataLabels.IsCategoryName);
    records.Add((IBiffStorage) this.m_dataFormat);
    records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.Begin));
    if (this.m_3DDataFormat != null)
      records.Add((IBiffStorage) this.m_3DDataFormat.Clone());
    if (this.m_border != null)
      this.m_border.Serialize((IList<IBiffStorage>) records);
    else if (flag && this.IsBorderSupported)
    {
      BiffRecordRaw record = BiffRecordFactory.GetRecord(TBIFFRecord.ChartLineFormat);
      records.Add((IBiffStorage) record);
    }
    if (this.m_interior != null)
      this.m_interior.Serialize((IList<IBiffStorage>) records);
    else if (flag && this.IsInteriorSupported)
    {
      BiffRecordRaw record = BiffRecordFactory.GetRecord(TBIFFRecord.ChartAreaFormat);
      records.Add((IBiffStorage) record);
    }
    if (this.m_pieFormat != null)
      records.Add((IBiffStorage) this.m_pieFormat.Clone());
    if (this.m_seriesFormat != null)
      records.Add((IBiffStorage) this.m_seriesFormat.Clone());
    if ((this.m_serie == null || this.m_serie.StartType != "Scatter") && this.IsInteriorSupported)
      this.m_fill.Serialize((IList<IBiffStorage>) records);
    if (this.m_markerFormat != null && this.IsMarkerSupported)
      records.Add((IBiffStorage) this.m_markerFormat.Clone());
    if (this.m_attachedLabel != null)
      records.Add((IBiffStorage) this.m_attachedLabel.Clone());
    if (this.m_startBlock != null)
      records.Add((IBiffStorage) this.m_startBlock.Clone());
    if (this.m_shapePropsStream != null)
      records.Add((IBiffStorage) this.m_shapePropsStream.Clone());
    if (this.m_endBlock != null)
      records.Add((IBiffStorage) this.m_endBlock.Clone());
    records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.End));
  }

  public void SetDefaultValues()
  {
    this.SetFieldsToNull();
    this.m_dataFormat.SeriesIndex = (ushort) this.m_serie.Index;
    this.m_dataFormat.SeriesNumber = (ushort) this.m_serie.Number;
    this.m_3DDataFormat = this.m_serie.Get3DDataFormat();
    ChartImpl innerChart = this.m_serie.InnerChart;
    if (innerChart.IsChartStock && innerChart.Series[innerChart.Series.Count - 1] == this.m_serie)
    {
      this.LineProperties.LinePattern = innerChart.DefaultLinePattern;
      this.m_border.AutoFormat = false;
      this.m_pieFormat = (ChartPieFormatRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartPieFormat);
      this.MarkerFormat.MarkerType = ExcelChartMarkerType.DowJones;
      this.m_markerFormat.LineSize = 60;
    }
    else
    {
      if (this.m_serie.ChartGroup <= 0)
        return;
      this.LineProperties.LineColor = ColorExtension.FromArgb(8388608 /*0x800000*/);
      this.m_pieFormat = (ChartPieFormatRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartPieFormat);
      this.MarkerFormat.MarkerType = ExcelChartMarkerType.Diamond;
      this.m_markerFormat.BorderColorIndex = (ushort) 32 /*0x20*/;
      this.m_markerFormat.FillColorIndex = (ushort) 32 /*0x20*/;
      this.m_markerFormat.LineSize = 100;
      this.m_markerFormat.IsAutoColor = true;
      this.m_border.AutoFormat = true;
    }
  }

  private void SetDefault3DDataFormat()
  {
    this.m_3DDataFormat = (Chart3DDataFormatRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Chart3DDataFormat);
    switch (this.m_chart.ChartType)
    {
      case ExcelChartType.Cylinder_Clustered:
      case ExcelChartType.Cylinder_Stacked:
      case ExcelChartType.Cylinder_Stacked_100:
      case ExcelChartType.Cylinder_Bar_Clustered:
      case ExcelChartType.Cylinder_Bar_Stacked:
      case ExcelChartType.Cylinder_Bar_Stacked_100:
      case ExcelChartType.Cylinder_Clustered_3D:
        this.m_3DDataFormat.DataFormatBase = ExcelBaseFormat.Circle;
        this.m_3DDataFormat.DataFormatTop = ExcelTopFormat.Straight;
        break;
      case ExcelChartType.Cone_Clustered:
      case ExcelChartType.Cone_Stacked:
      case ExcelChartType.Cone_Bar_Clustered:
      case ExcelChartType.Cone_Bar_Stacked:
      case ExcelChartType.Cone_Clustered_3D:
        this.m_3DDataFormat.DataFormatBase = ExcelBaseFormat.Circle;
        this.m_3DDataFormat.DataFormatTop = ExcelTopFormat.Sharp;
        break;
      case ExcelChartType.Cone_Stacked_100:
      case ExcelChartType.Cone_Bar_Stacked_100:
        this.m_3DDataFormat.DataFormatBase = ExcelBaseFormat.Circle;
        this.m_3DDataFormat.DataFormatTop = ExcelTopFormat.Trunc;
        break;
      case ExcelChartType.Pyramid_Clustered:
      case ExcelChartType.Pyramid_Stacked:
      case ExcelChartType.Pyramid_Bar_Clustered:
      case ExcelChartType.Pyramid_Bar_Stacked:
      case ExcelChartType.Pyramid_Clustered_3D:
        this.m_3DDataFormat.DataFormatBase = ExcelBaseFormat.Rectangle;
        this.m_3DDataFormat.DataFormatTop = ExcelTopFormat.Sharp;
        break;
      case ExcelChartType.Pyramid_Stacked_100:
      case ExcelChartType.Pyramid_Bar_Stacked_100:
        this.m_3DDataFormat.DataFormatBase = ExcelBaseFormat.Rectangle;
        this.m_3DDataFormat.DataFormatTop = ExcelTopFormat.Trunc;
        break;
    }
  }

  private void SetFieldsToNull()
  {
    this.m_dataFormat = (ChartDataFormatRecord) null;
    this.m_3DDataFormat = (Chart3DDataFormatRecord) null;
    this.m_border = (ChartBorderImpl) null;
    this.m_interior = (ChartInteriorImpl) null;
    this.m_pieFormat = (ChartPieFormatRecord) null;
    this.m_markerFormat = (ChartMarkerFormatRecord) null;
  }

  public ChartSerieDataFormatImpl Clone(object parent)
  {
    ChartSerieDataFormatImpl serieDataFormatImpl = (ChartSerieDataFormatImpl) this.MemberwiseClone();
    serieDataFormatImpl.SetParent(parent);
    serieDataFormatImpl.SetParents();
    serieDataFormatImpl.m_dataFormat = (ChartDataFormatRecord) CloneUtils.CloneCloneable((ICloneable) this.m_dataFormat);
    serieDataFormatImpl.m_3DDataFormat = (Chart3DDataFormatRecord) CloneUtils.CloneCloneable((ICloneable) this.m_3DDataFormat);
    if (this.m_3D != null)
      serieDataFormatImpl.m_3D = this.m_3D.Clone((object) serieDataFormatImpl);
    if (this.m_border != null)
      serieDataFormatImpl.m_border = this.m_border.Clone((object) serieDataFormatImpl);
    if (this.m_interior != null)
      serieDataFormatImpl.m_interior = this.m_interior.Clone((object) serieDataFormatImpl);
    serieDataFormatImpl.m_pieFormat = (ChartPieFormatRecord) CloneUtils.CloneCloneable((ICloneable) this.m_pieFormat);
    serieDataFormatImpl.m_markerFormat = (ChartMarkerFormatRecord) CloneUtils.CloneCloneable((ICloneable) this.m_markerFormat);
    serieDataFormatImpl.m_attachedLabel = (ChartAttachedLabelRecord) CloneUtils.CloneCloneable((ICloneable) this.m_attachedLabel);
    serieDataFormatImpl.m_attachedLabelLayout = (ChartAttachedLabelLayoutRecord) CloneUtils.CloneCloneable((ICloneable) this.m_attachedLabelLayout);
    serieDataFormatImpl.m_seriesFormat = (ChartSerFmtRecord) CloneUtils.CloneCloneable((ICloneable) this.m_seriesFormat);
    serieDataFormatImpl.m_fill = (ChartFillImpl) this.m_fill.Clone((object) serieDataFormatImpl);
    if (!this.m_chart.TypeChanging && !this.m_chart.IsParsed && this.IsInteriorSupported && serieDataFormatImpl.IsSupportFill)
      this.CopyFillBackForeGroundColorObjects(serieDataFormatImpl);
    serieDataFormatImpl.InitializeColors();
    serieDataFormatImpl.m_markerBackColor.CopyFrom(this.m_markerBackColor, false);
    serieDataFormatImpl.m_markerForeColor.CopyFrom(this.m_markerForeColor, false);
    return serieDataFormatImpl;
  }

  internal void CopyFillBackForeGroundColorObjects(ChartSerieDataFormatImpl result)
  {
    if (this.m_fill == null || result.m_fill == null)
      return;
    if (result.m_fill.ForeColorObject != this.m_fill.ForeColorObject)
      result.m_fill.ForeColorObject.CopyFrom(this.m_fill.ForeColorObject, false);
    if (!(result.m_fill.BackColorObject != this.m_fill.BackColorObject))
      return;
    result.m_fill.BackColorObject.CopyFrom(this.m_fill.BackColorObject, false);
  }

  internal void SetDefaultValuesForSerieRecords()
  {
    this.m_dataFormat.SeriesIndex = (ushort) this.m_serie.Index;
    this.m_dataFormat.SeriesNumber = (ushort) this.m_serie.Number;
    this.m_3DDataFormat = (Chart3DDataFormatRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Chart3DDataFormat);
    this.m_3D = (ThreeDFormatImpl) null;
    this.m_pieFormat = (ChartPieFormatRecord) null;
    this.m_seriesFormat = (ChartSerFmtRecord) null;
  }

  public void UpdateSerieIndex()
  {
    this.m_dataFormat.SeriesIndex = (ushort) this.m_serie.Index;
    this.m_dataFormat.SeriesNumber = (ushort) this.m_serie.Number;
  }

  public void UpdateDataFormatInDataPoint()
  {
    this.m_dataFormat.SeriesIndex = this.ParentSerie != null ? (ushort) this.ParentSerie.Index : throw new ArgumentException("Parent serie");
    this.m_dataFormat.SeriesNumber = (ushort) this.ParentSerie.Number;
  }

  public void ChangeRadarDataFormat(ExcelChartType type)
  {
    if (type == ExcelChartType.Radar)
    {
      this.MarkerForegroundColorIndex = ExcelKnownColors.YellowCustom | ExcelKnownColors.BlackCustom;
      this.MarkerBackgroundColorIndex = ExcelKnownColors.YellowCustom | ExcelKnownColors.BlackCustom;
      this.LineProperties.AutoFormat = true;
      this.m_border.IsAutoLineColor = true;
      this.IsAutoMarker = false;
      this.MarkerStyle = ExcelChartMarkerType.None;
    }
    if (type != ExcelChartType.Radar_Markers || this.ParentChart.Loading)
      return;
    this.LineProperties.AutoFormat = false;
  }

  public void ChangeScatterDataFormat(ExcelChartType type)
  {
    if (type == ExcelChartType.Scatter_Line_Markers)
    {
      this.LineProperties.LinePattern = ExcelChartLinePattern.None;
      this.m_border.AutoFormat = true;
    }
    else
    {
      this.MarkerSize = 5;
      this.MarkerStyle = ExcelChartMarkerType.None;
      this.LineProperties.AutoFormat = true;
      if (type == ExcelChartType.Scatter_SmoothedLine || type == ExcelChartType.Scatter_SmoothedLine_Markers)
        this.IsSmoothedLine = true;
      if (type == ExcelChartType.Scatter_SmoothedLine_Markers || type == ExcelChartType.Scatter_Markers)
      {
        this.MarkerStyle = ExcelChartMarkerType.Diamond;
        this.IsAutoMarker = true;
      }
      if (type != ExcelChartType.Scatter_Markers)
        return;
      this.m_border.LinePattern = ExcelChartLinePattern.None;
      this.m_markerFormat = (ChartMarkerFormatRecord) null;
    }
  }

  public void ChangeLineDataFormat(ExcelChartType type)
  {
    if (type == ExcelChartType.Line || type == ExcelChartType.Line_Stacked || type == ExcelChartType.Line_Stacked_100)
    {
      this.IsAutoMarker = false;
      this.MarkerStyle = ExcelChartMarkerType.None;
    }
    if (type != ExcelChartType.Line_Markers && type != ExcelChartType.Line_Markers_Stacked && type != ExcelChartType.Line_Markers_Stacked_100)
      return;
    this.LineProperties.AutoFormat = false;
  }

  internal void UpdateBarFormat(bool bIsDataTop)
  {
    if (this.m_serie != null)
      return;
    IChartSeries series = this.m_chart.Series;
    int index = 0;
    for (int count = series.Count; index < count; ++index)
    {
      IChartSerieDataFormat dataFormat = series[index].DataPoints.DefaultDataPoint.DataFormat;
      if (bIsDataTop)
        dataFormat.BarShapeTop = this.BarShapeTop;
      else
        dataFormat.BarShapeBase = this.BarShapeBase;
    }
  }

  public int UpdateLineColor()
  {
    ExcelChartType serieType = this.SerieType;
    string startSerieType = ChartFormatImpl.GetStartSerieType(serieType);
    return serieType != ExcelChartType.Radar_Markers && serieType != ExcelChartType.Radar && !(startSerieType == "Line") ? -1 : ChartSerieDataFormatImpl.UpdateColor(this.m_serie, this.m_dataPoint);
  }

  public static int UpdateColor(ChartSerieImpl serie, ChartDataPointImpl dataPoint)
  {
    if (serie == null)
      return 24;
    int num = serie.SerieFormat.CommonSerieOptions.IsVaryColor ? dataPoint.Index : serie.Index;
    return num <= 30 ? num + 24 : (num - 30) % 55 + 7;
  }

  public void UpdateSerieFormat()
  {
    if (this.m_3DDataFormat == null)
      this.m_3DDataFormat = (Chart3DDataFormatRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Chart3DDataFormat);
    if (this.m_pieFormat == null)
      this.m_pieFormat = (ChartPieFormatRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartPieFormat);
    if (this.m_markerFormat == null && !this.m_chart.ParentWorkbook.Loading)
      this.m_markerFormat = (ChartMarkerFormatRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartMarkerFormat);
    if (this.m_seriesFormat == null)
      this.m_seriesFormat = (ChartSerFmtRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartSerFmt);
    bool typeChanging = this.m_chart.TypeChanging;
    bool loading = this.m_chart.ParentWorkbook.Loading;
    ExcelChartType destinationType = this.m_chart.DestinationType;
    bool flag1 = !loading && (typeChanging && ChartSerieDataFormatImpl.GetIsBorderSupported(destinationType) || !typeChanging && this.IsBorderSupported);
    if (this.m_border == null && flag1)
      this.m_border = new ChartBorderImpl(this.Application, (object) this);
    bool flag2 = !loading && (typeChanging && ChartSerieDataFormatImpl.GetIsInteriorSupported(destinationType) || !typeChanging && this.IsInteriorSupported);
    if (this.m_interior == null && flag2)
      this.m_interior = new ChartInteriorImpl(this.Application, (object) this);
    this.m_bFormatted = true;
  }

  private bool ChackDataRecord(Chart3DDataFormatRecord record)
  {
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    return record.DataFormatBase != ExcelBaseFormat.Rectangle || record.DataFormatTop != ExcelTopFormat.Straight;
  }

  public void ClearOnPropertyChange()
  {
    if (this.m_chart.Loading)
      return;
    if (this.m_format != (ChartFormatImpl) null)
    {
      ((ChartSeriesCollection) this.m_chart.Series).ClearDataFormats(this);
    }
    else
    {
      if (this.m_dataPoint == null || this.m_dataPoint.Index != (int) ushort.MaxValue)
        return;
      this.m_dataPoint.ClearDataFormats(this);
    }
  }

  private bool ValidateMarkerProprties()
  {
    ExcelChartType serieType = this.SerieType;
    string startSerieType = ChartFormatImpl.GetStartSerieType(serieType);
    bool flag = startSerieType == "Line" || startSerieType == "Radar" || startSerieType == "Scatter";
    if (serieType == ExcelChartType.Radar || startSerieType == "Scatter")
      this.HasMarkerProperties = true;
    return flag && serieType != ExcelChartType.Line_3D && serieType != ExcelChartType.Radar_Filled;
  }

  internal static bool GetIsInteriorSupported(ExcelChartType chartType)
  {
    string startSerieType = ChartFormatImpl.GetStartSerieType(chartType);
    bool flag = startSerieType == "Line" && chartType != ExcelChartType.Line_3D || chartType == ExcelChartType.Radar || chartType == ExcelChartType.Radar_Markers;
    return !(startSerieType == "Surface") && !(startSerieType == "Scatter") && !flag;
  }

  private static bool GetIsBorderSupported(ExcelChartType chartType) => true;

  internal void MarkerForeColorChanged()
  {
    this.IsAutoMarker = false;
    ExcelKnownColors indexed = this.m_markerForeColor.GetIndexed(this.m_chart.Workbook);
    this.MarkerFormat.BorderColorIndex = (ushort) indexed;
    this.MarkerFormat.IsNotShowBrd = indexed == ExcelKnownColors.None;
    this.m_markerGradient = (GradientStops) null;
    if (this.EffectListStream != null)
      this.IsMarkerChanged = true;
    if (!this.m_chart.ParentWorkbook.Loading)
      this.m_markerLineStream = (Stream) null;
    this.ClearOnPropertyChange();
  }

  internal void MarkerBackColorChanged()
  {
    this.IsAutoMarker = false;
    ExcelKnownColors indexed = this.m_markerBackColor.GetIndexed(this.m_chart.Workbook);
    this.MarkerFormat.FillColorIndex = (ushort) indexed;
    this.MarkerFormat.IsNotShowInt = indexed == ExcelKnownColors.None;
    if (this.EffectListStream != null)
      this.IsMarkerChanged = true;
    this.ClearOnPropertyChange();
  }

  public bool HasLineProperties
  {
    get => this.m_border != null;
    internal set
    {
      if (this.m_border != null || !value)
        return;
      this.m_border = new ChartBorderImpl(this.Application, (object) this);
    }
  }

  public bool HasShadowProperties
  {
    get => this.m_shadow != null;
    internal set
    {
      if (value)
      {
        IShadow shadow = this.Shadow;
      }
      else
        this.m_shadow = (ShadowImpl) null;
    }
  }

  public IThreeDFormat ThreeD
  {
    get
    {
      if (this.m_3D == null)
        this.m_3D = new ThreeDFormatImpl(this.Application, (object) this);
      return (IThreeDFormat) this.m_3D;
    }
  }

  public bool Has3dProperties
  {
    get => this.m_3D != null && !this.m_3D.IsDefault;
    internal set
    {
      if (value)
      {
        IThreeDFormat threeD = this.ThreeD;
      }
      else
        this.m_3D = (ThreeDFormatImpl) null;
    }
  }

  public bool HasInterior
  {
    get
    {
      if (!this.IsInteriorSupported)
        this.m_interior = (ChartInteriorImpl) null;
      return this.m_interior != null;
    }
    internal set
    {
      if (this.m_interior != null || !value)
        return;
      this.m_interior = new ChartInteriorImpl(this.Application, (object) this);
    }
  }

  public IShadow Shadow
  {
    get
    {
      if (this.m_shadow == null)
        this.m_shadow = new ShadowImpl(this.Application, (object) this);
      return (IShadow) this.m_shadow;
    }
  }

  public IChartBorder LineProperties
  {
    get
    {
      if (!this.m_chart.TypeChanging && !this.IsBorderSupported)
        throw new NotSupportedException("This property dosn't support in this chart type");
      if (this.m_chart.ParentWorkbook.Loading)
        this.HasLineProperties = true;
      else
        this.UpdateSerieFormat();
      this.m_bFormatted = true;
      return (IChartBorder) this.m_border;
    }
  }

  public IChartInterior AreaProperties
  {
    get
    {
      if (!this.m_chart.TypeChanging && this.m_chart.IsParsed && !this.IsInteriorSupported)
        throw new NotSupportedException("This property dosn't support in this chart type");
      this.UpdateSerieFormat();
      this.m_bFormatted = true;
      if (!this.m_chart.TypeChanging && this.m_chart.IsParsed && this.m_interior != null && this.m_interior.UseAutomaticFormat)
      {
        this.m_interior.ForegroundColorObject.SetIndexed((ExcelKnownColors) ChartSerieDataFormatImpl.UpdateColor(this.m_serie, this.m_dataPoint));
        this.m_interior.UseAutomaticFormat = true;
      }
      return (IChartInterior) this.m_interior;
    }
  }

  public ExcelBaseFormat BarShapeBase
  {
    get => this.Serie3DDataFormat.DataFormatBase;
    set
    {
      if (value == this.BarShapeBase)
        return;
      bool loading = this.m_chart.Loading;
      if (!loading && this.m_chart.Series.Count != 0 && Array.IndexOf<ExcelChartType>(ChartSerieDataFormatImpl.DEF_SUPPORT_DATAFORMAT_PROPERTIES, this.SerieType) == -1)
        throw new NotSupportedException("This property is not supported in current chart type.");
      this.Serie3DDataFormat.DataFormatBase = value;
      if (!loading)
        this.UpdateBarFormat(false);
      this.m_bFormatted = true;
      this.m_HasBarShape = true;
      this.ClearOnPropertyChange();
    }
  }

  public ExcelTopFormat BarShapeTop
  {
    get => this.Serie3DDataFormat.DataFormatTop;
    set
    {
      if (value == this.BarShapeTop)
        return;
      bool loading = this.m_chart.Loading;
      if (!loading && this.m_chart.Series.Count != 0 && Array.IndexOf<ExcelChartType>(ChartSerieDataFormatImpl.DEF_SUPPORT_DATAFORMAT_PROPERTIES, this.SerieType) == -1)
        throw new NotSupportedException("This property is not supported in current chart type.");
      this.Serie3DDataFormat.DataFormatTop = value;
      if (!loading)
        this.UpdateBarFormat(true);
      this.m_bFormatted = true;
      this.m_HasBarShape = true;
      this.ClearOnPropertyChange();
    }
  }

  public Color MarkerBackgroundColor
  {
    get
    {
      return this.m_chart.Workbook.Version == ExcelVersion.Excel97to2003 ? ColorExtension.FromArgb(this.MarkerFormat.ForeColor) : this.m_markerBackColor.GetRGB(this.m_chart.Workbook);
    }
    set
    {
      this.MarkerFormat.ForeColor = value.ToArgb() & 16777215 /*0xFFFFFF*/;
      this.m_markerBackColor.SetRGB(value);
      if (this.m_dataPoint != null)
        this.m_dataPoint.IsDefaultmarkertype = true;
      this.IsAutoMarkerColor = false;
    }
  }

  public Color MarkerForegroundColor
  {
    get
    {
      return this.m_chart.Workbook.Version == ExcelVersion.Excel97to2003 ? ColorExtension.FromArgb(this.MarkerFormat.BackColor) : this.m_markerForeColor.GetRGB(this.m_chart.Workbook);
    }
    set
    {
      if (((WorkbookImpl) this.m_chart.Workbook).IsCreated || this.Parent is ChartDataPointImpl)
        this.MarkerFormat.FlagOptions |= (byte) 2;
      this.m_markerForeColor.SetRGB(value);
      this.MarkerFormat.BackColor = value.ToArgb() & 16777215 /*0xFFFFFF*/;
      if (this.m_dataPoint == null)
        return;
      this.m_dataPoint.IsDefaultmarkertype = true;
    }
  }

  public ExcelChartMarkerType MarkerStyle
  {
    get
    {
      if (!this.m_chart.Loading && !this.m_chart.TypeChanging && !this.ValidateMarkerProprties())
        throw new NotSupportedException("This property is not supported in this chart type.");
      return this.MarkerFormat.MarkerType;
    }
    set
    {
      if (this.MarkerStyle == value && ((!this.m_chart.IsChartScatter || !this.m_chart.IsChartLine) && !this.m_chart.IsChartSmoothedLine || !this.IsMarker))
        return;
      this.MarkerFormat.MarkerType = value;
      this.IsAutoMarker = false;
      this.HasMarkerProperties = true;
      if (!this.m_chart.TypeChanging)
        this.ClearOnPropertyChange();
      if (this.m_dataPoint == null)
        return;
      this.m_dataPoint.IsDefaultmarkertype = true;
    }
  }

  public ExcelKnownColors MarkerForegroundColorIndex
  {
    get
    {
      if (!this.m_chart.Loading && !this.m_chart.TypeChanging && !this.ValidateMarkerProprties())
        throw new NotSupportedException("This property is not supported in this chart type.");
      return (ExcelKnownColors) this.MarkerFormat.BorderColorIndex;
    }
    set
    {
      if (this.MarkerForegroundColorIndex == value)
        return;
      this.MarkerFormat.FlagOptions |= (byte) 2;
      this.m_markerForeColor.SetIndexed(value);
      if (this.m_dataPoint == null)
        return;
      this.m_dataPoint.IsDefaultmarkertype = true;
    }
  }

  public ExcelKnownColors MarkerBackgroundColorIndex
  {
    get
    {
      if (!this.m_chart.Loading && !this.m_chart.TypeChanging && !this.ValidateMarkerProprties())
        throw new NotSupportedException("This property is not supported in this chart type.");
      return (ExcelKnownColors) this.MarkerFormat.FillColorIndex;
    }
    set
    {
      if (this.MarkerBackgroundColorIndex == value)
        return;
      this.m_markerBackColor.SetIndexed(value);
      if (this.m_dataPoint != null)
        this.m_dataPoint.IsDefaultmarkertype = true;
      this.IsAutoMarkerColor = false;
    }
  }

  public int MarkerSize
  {
    get
    {
      if (!this.m_chart.Loading && !this.m_chart.TypeChanging && !this.ValidateMarkerProprties())
        throw new NotSupportedException("This property is not supported in this chart type.");
      return this.MarkerFormat.LineSize / 20;
    }
    set
    {
      if (value == this.MarkerSize)
        return;
      if (value < 2 || value > 72)
        throw new ArgumentOutOfRangeException(nameof (MarkerSize));
      this.MarkerFormat.LineSize = value * 20;
      if (this.MarkerFormat.IsAutoColor)
        this.MarkerFormat.MarkerType = ExcelChartMarkerType.Square;
      this.IsAutoMarker = false;
      this.ClearOnPropertyChange();
      if (this.m_dataPoint == null)
        return;
      this.m_dataPoint.IsDefaultmarkertype = true;
    }
  }

  public bool IsAutoMarker
  {
    get
    {
      if (!this.m_chart.Loading && !this.m_chart.TypeChanging && !this.ValidateMarkerProprties())
        throw new NotSupportedException("This property is not supported in this chart type.");
      return this.MarkerFormat.IsAutoColor;
    }
    set
    {
      if (value == this.IsAutoMarker)
        return;
      this.MarkerFormat.IsAutoColor = value;
      if (!value)
      {
        int num = ChartSerieDataFormatImpl.UpdateColor(this.m_serie, this.m_dataPoint);
        this.MarkerFormat.FillColorIndex = (ushort) num;
        this.MarkerFormat.BorderColorIndex = (ushort) num;
      }
      if (this.m_chart.TypeChanging)
        return;
      this.ClearOnPropertyChange();
    }
  }

  public bool IsNotShowInt
  {
    get => this.MarkerFormat.IsNotShowInt;
    set => this.MarkerFormat.IsNotShowInt = value;
  }

  public bool IsNotShowBrd
  {
    get => this.MarkerFormat.IsNotShowBrd;
    set => this.MarkerFormat.IsNotShowBrd = value;
  }

  public int Percent
  {
    get
    {
      return this.m_dataPoint != null && this.m_chart.Workbook.Version != ExcelVersion.Excel97to2003 ? this.m_dataPoint.Explosion : (int) this.PieFormat.Percent;
    }
    set
    {
      if (!this.m_chart.TypeChanging)
      {
        string startSerieType = ChartFormatImpl.GetStartSerieType(this.SerieType);
        if (startSerieType != "Pie" && startSerieType != "Doughnut")
          throw new NotSupportedException("This property is not supported in current chart type.");
      }
      this.PieFormat.Percent = (ushort) value;
      if (this.m_dataPoint != null && this.m_chart.Workbook.Version != ExcelVersion.Excel97to2003)
        this.m_dataPoint.Explosion = (int) (ushort) value;
      this.ClearOnPropertyChange();
    }
  }

  public bool IsSmoothedLine
  {
    get => this.SerieFormat.IsSmoothedLine;
    set => this.SerieFormat.IsSmoothedLine = value;
  }

  public bool Is3DBubbles
  {
    get => this.SerieFormat.Is3DBubbles;
    set
    {
      if (this.Is3DBubbles == value)
        return;
      ExcelChartType serieType = this.SerieType;
      if (this.ParentChart.Series.Count != 0 && serieType != ExcelChartType.Bubble && serieType != ExcelChartType.Bubble_3D)
        throw new NotSupportedException("This property is not supported in this chart type.");
      this.SerieFormat.Is3DBubbles = value;
      this.ClearOnPropertyChange();
    }
  }

  public bool IsArShadow
  {
    get => this.SerieFormat.IsArShadow;
    set => this.SerieFormat.IsArShadow = value;
  }

  public bool ShowActiveValue
  {
    get => this.AttachedLabel.ShowActiveValue;
    set => this.AttachedLabel.ShowActiveValue = value;
  }

  public bool ShowPieInPercents
  {
    get => this.AttachedLabel.ShowPieInPercents;
    set => this.AttachedLabel.ShowPieInPercents = value;
  }

  public bool ShowPieCategoryLabel
  {
    get => this.AttachedLabel.ShowPieCategoryLabel;
    set => this.AttachedLabel.ShowPieCategoryLabel = value;
  }

  public bool SmoothLine
  {
    get => this.AttachedLabel.SmoothLine;
    set => this.AttachedLabel.SmoothLine = value;
  }

  public bool ShowCategoryLabel
  {
    get => this.AttachedLabel.ShowCategoryLabel;
    set => this.AttachedLabel.ShowCategoryLabel = value;
  }

  public bool ShowBubble
  {
    get => this.AttachedLabel.ShowBubble;
    set => this.AttachedLabel.ShowBubble = value;
  }

  public IFill Fill
  {
    get
    {
      if (!this.m_chart.TypeChanging && this.m_chart.IsParsed)
      {
        if (!this.IsSupportFill)
          throw new NotSupportedException("This property isn't supported in this chart type");
        this.UpdateSerieFormat();
      }
      return (IFill) this.m_fill;
    }
  }

  public bool IsSupportFill
  {
    get
    {
      ExcelChartType serieType = this.SerieType;
      string startSerieType = ChartFormatImpl.GetStartSerieType(serieType);
      bool flag = startSerieType == "Line" && serieType != ExcelChartType.Line_3D || serieType == ExcelChartType.Radar || serieType == ExcelChartType.Radar_Markers;
      return !(startSerieType == "Surface") && !(startSerieType == "Scatter") && !flag;
    }
  }

  public IChartFormat CommonSerieOptions
  {
    get
    {
      return this.m_serie != null ? (IChartFormat) this.m_serie.GetCommonSerieFormat() : throw new NotSupportedException("Cannot get series options.");
    }
  }

  public bool IsMarkerSupported => this.ValidateMarkerProprties();

  public IChartInterior Interior => this.AreaProperties;

  public bool IsInteriorSupported
  {
    get => ChartSerieDataFormatImpl.GetIsInteriorSupported(this.SerieType);
  }

  public bool IsBorderSupported => ChartSerieDataFormatImpl.GetIsBorderSupported(this.SerieType);

  internal bool HasMarkerProperties
  {
    get => this.m_HasMarkerProperties;
    set => this.m_HasMarkerProperties = value;
  }

  internal bool HasBarShape
  {
    get => this.m_HasBarShape;
    set => this.m_HasBarShape = true;
  }

  internal HistogramAxisFormat HistogramAxisFormatProperty
  {
    get => this.m_histogramAxisFormat;
    set => this.m_histogramAxisFormat = value;
  }

  public bool ShowConnectorLines
  {
    get => this.m_showConnectorLines;
    set => this.m_showConnectorLines = value;
  }

  public ExcelTreeMapLabelOption TreeMapLabelOption
  {
    get => this.m_treeMapLabelOption;
    set => this.m_treeMapLabelOption = value;
  }

  public bool ShowMeanLine
  {
    get => this.m_boxAndWhsikerFormat.ShowMeanLine;
    set => this.m_boxAndWhsikerFormat.ShowMeanLine = value;
  }

  public bool ShowMeanMarkers
  {
    get => this.m_boxAndWhsikerFormat.ShowMeanMarkers;
    set => this.m_boxAndWhsikerFormat.ShowMeanMarkers = value;
  }

  public bool ShowInnerPoints
  {
    get => this.m_boxAndWhsikerFormat.ShowInnerPoints;
    set => this.m_boxAndWhsikerFormat.ShowInnerPoints = value;
  }

  public bool ShowOutlierPoints
  {
    get => this.m_boxAndWhsikerFormat.ShowOutlierPoints;
    set => this.m_boxAndWhsikerFormat.ShowOutlierPoints = value;
  }

  public ExcelQuartileCalculation QuartileCalculationType
  {
    get => this.m_boxAndWhsikerFormat.QuartileCalculationType;
    set => this.m_boxAndWhsikerFormat.QuartileCalculationType = value;
  }

  internal bool IsBinningByCategory
  {
    get => this.m_histogramAxisFormat.IsBinningByCategory;
    set => this.m_histogramAxisFormat.IsBinningByCategory = value;
  }

  internal bool HasAutomaticBins
  {
    get => this.m_histogramAxisFormat.HasAutomaticBins;
    set => this.m_histogramAxisFormat.HasAutomaticBins = value;
  }

  internal int NumberOfBins
  {
    get => this.m_histogramAxisFormat.NumberOfBins;
    set => this.m_histogramAxisFormat.NumberOfBins = value;
  }

  internal double BinWidth
  {
    get => this.m_histogramAxisFormat.BinWidth;
    set => this.m_histogramAxisFormat.BinWidth = value;
  }

  internal double OverflowBinValue
  {
    get => this.m_histogramAxisFormat.OverflowBinValue;
    set => this.m_histogramAxisFormat.OverflowBinValue = value;
  }

  internal double UnderflowBinValue
  {
    get => this.m_histogramAxisFormat.UnderflowBinValue;
    set => this.m_histogramAxisFormat.UnderflowBinValue = value;
  }

  internal bool IsIntervalClosedinLeft
  {
    get => this.m_histogramAxisFormat.IsIntervalClosedinLeft;
    set => this.m_histogramAxisFormat.IsIntervalClosedinLeft = value;
  }

  public ChartSerieImpl ParentSerie => this.m_serie;

  [CLSCompliant(false)]
  public ChartDataFormatRecord DataFormat
  {
    get => this.m_dataFormat;
    set => this.m_dataFormat = value;
  }

  [CLSCompliant(false)]
  public ChartPieFormatRecord PieFormat
  {
    get
    {
      if (this.m_pieFormat == null)
        this.m_pieFormat = (ChartPieFormatRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartPieFormat);
      this.m_bFormatted = true;
      return this.m_pieFormat;
    }
  }

  [CLSCompliant(false)]
  public ChartMarkerFormatRecord MarkerFormat
  {
    get
    {
      if (this.m_markerFormat == null)
      {
        this.m_markerFormat = (ChartMarkerFormatRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartMarkerFormat);
        this.m_bFormatted = true;
        this.m_markerFormat.IsAutoColor = true;
      }
      return this.m_markerFormat;
    }
  }

  [CLSCompliant(false)]
  public Chart3DDataFormatRecord Serie3DDataFormat
  {
    get
    {
      if (this.m_3DDataFormat == null)
        this.m_3DDataFormat = (Chart3DDataFormatRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Chart3DDataFormat);
      return this.m_3DDataFormat;
    }
  }

  [CLSCompliant(false)]
  public ChartSerFmtRecord SerieFormat
  {
    get
    {
      if (this.m_seriesFormat == null)
        this.m_seriesFormat = (ChartSerFmtRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartSerFmt);
      this.m_bFormatted = true;
      return this.m_seriesFormat;
    }
  }

  [CLSCompliant(false)]
  public ChartAttachedLabelRecord AttachedLabel
  {
    get
    {
      if (this.m_attachedLabel == null)
      {
        this.UpdateSerieFormat();
        this.m_attachedLabel = (ChartAttachedLabelRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartAttachedLabel);
      }
      return this.m_attachedLabel;
    }
  }

  [CLSCompliant(false)]
  public ChartAttachedLabelLayoutRecord AttachedLabelLayout
  {
    get
    {
      if (this.m_attachedLabelLayout == null)
        this.m_attachedLabelLayout = (ChartAttachedLabelLayoutRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartAttachedLabelLayout);
      return this.m_attachedLabelLayout;
    }
  }

  public bool ContainsLineProperties => this.m_border != null;

  [CLSCompliant(false)]
  public ChartMarkerFormatRecord MarkerFormatOrNull => this.m_markerFormat;

  [CLSCompliant(false)]
  public Chart3DDataFormatRecord Serie3DdDataFormatOrNull => this.m_3DDataFormat;

  [CLSCompliant(false)]
  public ChartSerFmtRecord SerieFormatOrNull => this.m_seriesFormat;

  [CLSCompliant(false)]
  public ChartPieFormatRecord PieFormatOrNull => this.m_pieFormat;

  public int SeriesNumber
  {
    get => (int) this.m_dataFormat.SeriesNumber;
    set => this.m_dataFormat.SeriesNumber = (ushort) value;
  }

  public bool IsMarker
  {
    get
    {
      return this.MarkerFormatOrNull == null || this.MarkerFormatOrNull.MarkerType != ExcelChartMarkerType.None;
    }
  }

  public bool IsLine
  {
    get
    {
      return this.m_border == null || this.m_border.AutoFormat || this.m_border.LinePattern != ExcelChartLinePattern.None;
    }
  }

  public bool IsSmoothed => this.SerieFormatOrNull != null && this.IsSmoothedLine;

  private ExcelChartType SerieType
  {
    get
    {
      if (this.m_serie != null)
        return this.m_serie.SerieType;
      ChartSeriesCollection series = (ChartSeriesCollection) this.m_chart.Series;
      return series.Count == 0 ? this.ParentChart.ChartType : series.GetTypeByOrder((int) this.m_dataFormat.SeriesIndex);
    }
  }

  public bool IsFormatted
  {
    get => this.m_bFormatted;
    internal set => this.m_bFormatted = value;
  }

  public ChartImpl ParentChart => this.m_chart;

  public ColorObject MarkerBackColorObject => this.m_markerBackColor;

  public ColorObject MarkerForeColorObject => this.m_markerForeColor;

  internal GradientStops MarkerGradient
  {
    get => this.m_markerGradient;
    set => this.m_markerGradient = value;
  }

  public double MarkerTransparency
  {
    get => this.m_markerTransparency;
    set => this.m_markerTransparency = value;
  }

  public Stream MarkerLineStream
  {
    get => this.m_markerLineStream;
    set => this.m_markerLineStream = value;
  }

  internal Stream EffectListStream
  {
    get => this.m_markerEffectList;
    set => this.m_markerEffectList = value;
  }

  internal bool IsParsed
  {
    get => this.m_bIsParsed;
    set => this.m_bIsParsed = value;
  }

  internal bool IsMarkerChanged
  {
    get => this.m_markerChanged;
    set => this.m_markerChanged = value;
  }

  internal double MarkerLineWidth
  {
    get => this.m_markerLineWidth;
    set => this.m_markerLineWidth = value;
  }

  internal bool IsAutoMarkerColor
  {
    get => this.m_isAutoMarkerColor;
    set => this.m_isAutoMarkerColor = value;
  }

  internal bool IsDefault
  {
    get
    {
      return (!this.HasInterior || (this.Interior.UseAutomaticFormat || this.Interior.Pattern == ExcelPattern.None && (this.Fill == null || this.Fill.FillType == ExcelFillType.Pattern || this.Fill.FillType == ExcelFillType.SolidColor)) && this.Interior.Pattern != ExcelPattern.None) && (this.Shadow == null || this.Shadow.ShadowInnerPresets == Excel2007ChartPresetsInner.NoShadow) && this.Shadow.ShadowOuterPresets == Excel2007ChartPresetsOuter.NoShadow && this.Shadow.ShadowPrespectivePresets == Excel2007ChartPresetsPrespective.NoShadow && !this.Shadow.HasCustomShadowStyle && !this.Has3dProperties && (!(this.Shadow is ShadowImpl) || (this.Shadow as ShadowImpl).GlowStream == null && (this.Shadow as ShadowImpl).SoftEdgeRadius == -1) && (this.LineProperties == null || this.LineProperties.AutoFormat);
    }
  }

  public ColorObject ForeGroundColorObject
  {
    get
    {
      return this.AreaProperties == null ? (ColorObject) null : (this.AreaProperties as ChartInteriorImpl).ForegroundColorObject;
    }
  }

  public ColorObject BackGroundColorObject
  {
    get
    {
      return this.AreaProperties == null ? (ColorObject) null : (this.AreaProperties as ChartInteriorImpl).BackgroundColorObject;
    }
  }

  public ExcelPattern Pattern
  {
    get => this.AreaProperties.Pattern;
    set => this.AreaProperties.Pattern = value;
  }

  public bool IsAutomaticFormat
  {
    get => this.AreaProperties.UseAutomaticFormat;
    set => this.AreaProperties.UseAutomaticFormat = value;
  }

  public bool Visible
  {
    get => this.AreaProperties.Pattern != ExcelPattern.None;
    set
    {
      if (value)
      {
        if (this.AreaProperties.Pattern != ExcelPattern.None)
          return;
        this.AreaProperties.Pattern = ExcelPattern.Solid;
      }
      else
        this.AreaProperties.Pattern = ExcelPattern.None;
    }
  }

  internal bool CompareFormat(IChartSerieDataFormat format)
  {
    bool flag = false;
    if (!this.IsSupportFill || this.IsSupportFill && this.Fill.BackColorIndex == format.Fill.BackColorIndex && this.Fill.FillType == format.Fill.FillType && this.Fill.ForeColorIndex == format.Fill.ForeColorIndex && (this.Fill.FillType != ExcelFillType.Gradient || this.Fill.FillType == ExcelFillType.Gradient && this.Fill.GradientColorType == format.Fill.GradientColorType && this.Fill.GradientStyle == format.Fill.GradientStyle && this.Fill.GradientVariant == format.Fill.GradientVariant && (this.Fill.GradientColorType == ExcelGradientColor.OneColor && this.Fill.GradientDegree == format.Fill.GradientDegree || this.Fill.GradientColorType == ExcelGradientColor.Preset && this.Fill.PresetGradientType == format.Fill.PresetGradientType)) && (this.Fill.FillType != ExcelFillType.Pattern || this.Fill.FillType == ExcelFillType.Pattern && this.Fill.Pattern == format.Fill.Pattern) && (this.Fill.FillType != ExcelFillType.Picture || this.Fill.FillType == ExcelFillType.Picture && this.Fill.PictureName == format.Fill.PictureName) && (this.Fill.FillType != ExcelFillType.Texture || this.Fill.FillType == ExcelFillType.Texture && this.Fill.Texture == format.Fill.Texture && (double) this.Fill.TextureHorizontalScale == (double) format.Fill.TextureHorizontalScale && (double) this.Fill.TextureOffsetX == (double) format.Fill.TextureOffsetX && (double) this.Fill.TextureOffsetY == (double) format.Fill.TextureOffsetY && (double) this.Fill.TextureVerticalScale == (double) format.Fill.TextureVerticalScale) && (this.Fill.FillType != ExcelFillType.SolidColor || this.Fill.FillType == ExcelFillType.SolidColor && this.Fill.Transparency == format.Fill.Transparency && (double) this.Fill.TransparencyColor == (double) format.Fill.TransparencyColor) && this.Fill.Visible == format.Fill.Visible)
      flag = true;
    if (format.HasLineProperties && (this.HasLineProperties || ChartFormatImpl.GetStartSerieType(this.SerieType) == "Line"))
    {
      IChartBorder lineProperties1 = format.LineProperties;
      IChartBorder lineProperties2 = this.LineProperties;
      if (lineProperties1.AutoFormat != lineProperties2.AutoFormat || lineProperties1.ColorIndex != lineProperties2.ColorIndex || lineProperties1.DrawTickLabels != lineProperties2.DrawTickLabels || lineProperties1.IsAutoLineColor != lineProperties2.IsAutoLineColor || !(lineProperties1.LineColor == lineProperties2.LineColor) || lineProperties1.LinePattern != lineProperties2.LinePattern || lineProperties1.LineWeight != lineProperties2.LineWeight || lineProperties1.Transparency != lineProperties2.Transparency)
        flag = false;
    }
    if (format.HasInterior && this.HasInterior)
    {
      IChartInterior areaProperties1 = format.AreaProperties;
      IChartInterior areaProperties2 = this.AreaProperties;
      if (areaProperties1.BackgroundColorIndex != areaProperties2.BackgroundColorIndex || areaProperties1.ForegroundColorIndex != areaProperties2.ForegroundColorIndex || areaProperties1.Pattern != areaProperties2.Pattern || areaProperties1.SwapColorsOnNegative != areaProperties2.SwapColorsOnNegative || areaProperties1.UseAutomaticFormat != areaProperties2.UseAutomaticFormat)
        flag = false;
      IChartInterior interior1 = format.Interior;
      IChartInterior interior2 = this.Interior;
      if (interior1.BackgroundColorIndex != interior2.BackgroundColorIndex || interior1.ForegroundColorIndex != interior2.ForegroundColorIndex || interior1.Pattern != interior2.Pattern || interior1.SwapColorsOnNegative != interior2.SwapColorsOnNegative || interior1.UseAutomaticFormat != interior2.UseAutomaticFormat)
        flag = false;
    }
    if (format.HasShadowProperties && this.HasShadowProperties)
    {
      IShadow shadow1 = format.Shadow;
      IShadow shadow2 = this.Shadow;
      if (shadow1.Angle != shadow2.Angle || shadow1.Blur != shadow2.Blur || shadow1.Distance != shadow2.Distance || shadow1.HasCustomShadowStyle != shadow2.HasCustomShadowStyle || !(shadow1.ShadowColor == shadow2.ShadowColor) || shadow1.ShadowInnerPresets != shadow2.ShadowInnerPresets || shadow1.ShadowOuterPresets != shadow2.ShadowOuterPresets || shadow1.ShadowPrespectivePresets != shadow2.ShadowPrespectivePresets || shadow1.Size != shadow2.Size || shadow1.Transparency != shadow2.Transparency)
        flag = false;
    }
    if (format.IsMarkerSupported && this.IsMarkerSupported && (format.IsAutoMarker != this.IsAutoMarker || format.MarkerBackgroundColorIndex != this.MarkerBackgroundColorIndex || format.MarkerForegroundColorIndex != this.MarkerForegroundColorIndex || format.MarkerSize != this.MarkerSize || format.MarkerStyle != this.MarkerStyle || format.ShowMeanMarkers != this.ShowMeanMarkers))
      flag = false;
    if (format.Has3dProperties && this.Has3dProperties)
    {
      IThreeDFormat threeD1 = format.ThreeD;
      IThreeDFormat threeD2 = this.ThreeD;
      if (threeD1.BevelBottom != threeD2.BevelBottom || threeD1.BevelBottomHeight != threeD2.BevelBottomHeight || threeD1.BevelBottomWidth != threeD2.BevelBottomWidth || threeD1.BevelTop != threeD2.BevelTop || threeD1.BevelTopHeight != threeD2.BevelTopHeight || threeD1.BevelTopWidth != threeD2.BevelTopWidth || threeD1.Lighting != threeD2.Lighting || threeD1.Material != threeD2.Material)
        flag = false;
    }
    if (format.BarShapeBase != this.BarShapeBase || format.BarShapeTop != this.BarShapeTop || format.Is3DBubbles != this.Is3DBubbles || format.Percent != this.Percent || format.QuartileCalculationType != this.QuartileCalculationType || format.ShowMeanLine != this.ShowMeanLine || format.ShowInnerPoints != this.ShowInnerPoints || format.ShowOutlierPoints != this.ShowOutlierPoints || format.TreeMapLabelOption != this.TreeMapLabelOption)
      flag = false;
    return flag;
  }

  protected override void OnDispose()
  {
    if (this.m_3D != null)
    {
      this.m_3D.Dispose();
      this.m_3D = (ThreeDFormatImpl) null;
    }
    this.m_3DDataFormat = (Chart3DDataFormatRecord) null;
    this.m_attachedLabel = (ChartAttachedLabelRecord) null;
    this.m_attachedLabelLayout = (ChartAttachedLabelLayoutRecord) null;
    this.m_dataFormat = (ChartDataFormatRecord) null;
    this.m_endBlock = (UnknownRecord) null;
    this.m_startBlock = (UnknownRecord) null;
    this.m_seriesFormat = (ChartSerFmtRecord) null;
    this.m_markerFormat = (ChartMarkerFormatRecord) null;
    if (this.m_markerGradient != null)
    {
      this.m_markerGradient.Clear();
      this.m_markerGradient = (GradientStops) null;
    }
    if (this.m_border != null)
    {
      this.m_border.Dispose();
      this.m_border = (ChartBorderImpl) null;
    }
    if (this.m_fill != null)
    {
      this.m_fill.Dispose();
      this.m_fill = (ChartFillImpl) null;
    }
    if (this.m_format != (ChartFormatImpl) null)
    {
      this.m_format.Dispose();
      this.m_format = (ChartFormatImpl) null;
    }
    if (this.m_interior != null)
    {
      this.m_interior.Dispose();
      this.m_interior = (ChartInteriorImpl) null;
    }
    if (this.m_markerEffectList != null)
      this.m_markerEffectList = (Stream) null;
    if (this.m_markerLineStream != null)
      this.m_markerLineStream = (Stream) null;
    this.m_histogramAxisFormat = (HistogramAxisFormat) null;
  }
}
