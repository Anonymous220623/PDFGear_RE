// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Charts.ChartFormatImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Exceptions;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.Charts;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Charts;

public class ChartFormatImpl : CommonObject, IChartFormat, ICloneParent
{
  public const int DEF_BAR_STACKED = -65436;
  private const int DEF_SERIES_NUMBER = 65533;
  private ChartChartFormatRecord m_chartChartFormat;
  private BiffRecordRaw m_serieFormat;
  private Chart3DRecord m_chart3D;
  private ChartFormatLinkRecord m_formatLink;
  private ChartDataLabelsRecord m_dataLabels;
  private ChartChartLineRecord m_chartChartLine;
  private ChartSerieDataFormatImpl m_dataFormat;
  private ChartDropBarImpl m_firstDropBar;
  private ChartDropBarImpl m_secondDropBar;
  private ChartSeriesListRecord m_seriesList;
  private ChartImpl m_chart;
  private ChartParentAxisImpl m_parentAxis;
  private ChartBorderImpl m_serieLine;
  private ChartBorderImpl m_highlowLine;
  private ChartBorderImpl m_dropLine;
  private bool m_isChartExType;

  public bool IsVeryColor
  {
    get => this.IsVaryColor;
    set => this.IsVaryColor = value;
  }

  public bool IsVaryColor
  {
    get => this.ChartChartFormatRecord.IsVaryColor;
    set => this.ChartChartFormatRecord.IsVaryColor = value;
  }

  public IChartSerieDataFormat SerieDataFormat => (IChartSerieDataFormat) this.DataFormat;

  public int Overlap
  {
    get
    {
      if (this.m_chart.IsChart3D)
        throw new NotSupportedException("This property is not supported in 3d chart types");
      return this.BarRecord.Overlap;
    }
    set
    {
      if (!this.m_chart.ParentWorkbook.Loading && this.m_chart.IsChart3D)
        throw new NotSupportedException("This property is not supported in 3d chart types");
      if ((!this.m_chart.ParentWorkbook.Loading && value < -100 || value > 100) && (!(this.FindParent(typeof (WorksheetImpl), true) is WorksheetImpl parent) || !parent.IsParsing))
        throw new ArgumentOutOfRangeException(nameof (value));
      this.BarRecord.Overlap = value;
    }
  }

  public int GapWidth
  {
    get
    {
      return this.m_serieFormat.TypeCode != TBIFFRecord.ChartBar ? (int) this.BoppopRecord.Gap : (int) this.BarRecord.CategoriesSpace;
    }
    set
    {
      if (this.m_serieFormat.TypeCode == TBIFFRecord.ChartBar)
        this.BarRecord.CategoriesSpace = value >= 0 && value <= 500 ? (ushort) value : throw new ArgumentOutOfRangeException(nameof (GapWidth));
      else
        this.BoppopRecord.Gap = value >= 0 && value <= 500 ? (ushort) value : throw new ArgumentOutOfRangeException(nameof (GapWidth));
    }
  }

  public bool IsHorizontalBar
  {
    get => this.BarRecord.IsHorizontalBar;
    set => this.BarRecord.IsHorizontalBar = value;
  }

  public bool StackValuesBar
  {
    get => this.BarRecord.StackValues;
    set => this.BarRecord.StackValues = value;
  }

  public bool ShowAsPercentsBar
  {
    get => this.BarRecord.ShowAsPercents;
    set => this.BarRecord.ShowAsPercents = value;
  }

  public bool HasShadowBar
  {
    get => this.BarRecord.HasShadow;
    set => this.BarRecord.HasShadow = value;
  }

  public bool StackValuesLine
  {
    get => this.LineRecord.StackValues;
    set => this.LineRecord.StackValues = value;
  }

  public bool ShowAsPercentsLine
  {
    get => this.LineRecord.ShowAsPercents;
    set => this.LineRecord.ShowAsPercents = value;
  }

  public bool HasShadowLine
  {
    get => this.LineRecord.HasShadow;
    set => this.LineRecord.HasShadow = value;
  }

  public int FirstSliceAngle
  {
    get => (int) this.PieRecord.StartAngle;
    set
    {
      this.PieRecord.StartAngle = value >= 0 && value <= 360 ? (ushort) value : throw new ArgumentOutOfRangeException("StartAngle");
      if (!this.m_chart.IsChart3D)
        return;
      this.m_chart3D.RotationAngle = (ushort) value;
    }
  }

  public int DoughnutHoleSize
  {
    get => (int) this.PieRecord.DonutHoleSize;
    set
    {
      if (value < 0 || value > 90)
        throw new ArgumentOutOfRangeException("DonutHoleSize");
      if (!this.m_chart.TypeChanging)
      {
        switch (this.m_chart.ChartType)
        {
          case ExcelChartType.Doughnut:
          case ExcelChartType.Doughnut_Exploded:
            break;
          default:
            throw new NotSupportedException("This property is supported only in doughnut chart types");
        }
      }
      this.PieRecord.DonutHoleSize = (ushort) value;
    }
  }

  public bool HasShadowPie
  {
    get => this.PieRecord.HasShadow;
    set => this.PieRecord.HasShadow = value;
  }

  public bool ShowLeaderLines
  {
    get
    {
      if (this.PieRecord != null)
        return this.PieRecord.ShowLeaderLines;
      return this.BoppopRecord == null || this.BoppopRecord.ShowLeaderLines;
    }
    set
    {
      if (this.PieRecord != null)
      {
        this.PieRecord.ShowLeaderLines = value;
      }
      else
      {
        if (this.BoppopRecord == null)
          return;
        this.BoppopRecord.ShowLeaderLines = value;
      }
    }
  }

  public int BubbleScale
  {
    get => (int) this.ScatterRecord.BubleSizeRation;
    set
    {
      if (value < 0 || value > 300)
        throw new ArgumentOutOfRangeException("BubleSizeScale");
      if (!this.m_chart.TypeChanging)
      {
        switch (this.m_chart.ChartType)
        {
          case ExcelChartType.Bubble:
          case ExcelChartType.Bubble_3D:
            break;
          default:
            throw new NotSupportedException("This property supported only in bubble chart types.");
        }
      }
      this.ScatterRecord.BubleSizeRation = (ushort) value;
    }
  }

  public ExcelBubbleSize SizeRepresents
  {
    get => this.ScatterRecord.BubleSize;
    set
    {
      if (!this.m_chart.TypeChanging)
      {
        switch (this.m_chart.ChartType)
        {
          case ExcelChartType.Bubble:
          case ExcelChartType.Bubble_3D:
            break;
          default:
            throw new NotSupportedException("This property is supported only in bubble chart types.");
        }
      }
      this.ScatterRecord.BubleSize = value;
    }
  }

  public bool IsBubbles
  {
    get => this.ScatterRecord.IsBubbles;
    set => this.ScatterRecord.IsBubbles = value;
  }

  public bool ShowNegativeBubbles
  {
    get => this.ScatterRecord.IsShowNegBubbles;
    set
    {
      switch (this.m_chart.ChartType)
      {
        case ExcelChartType.Bubble:
        case ExcelChartType.Bubble_3D:
          this.ScatterRecord.IsShowNegBubbles = value;
          break;
        default:
          throw new NotSupportedException("This property is supported only in bubble chart types.");
      }
    }
  }

  public bool HasShadowScatter
  {
    get => this.ScatterRecord.HasShadow;
    set => this.ScatterRecord.HasShadow = value;
  }

  public bool IsStacked
  {
    get => this.AreaRecord.IsStacked;
    set => this.AreaRecord.IsStacked = value;
  }

  public bool IsCategoryBrokenDown
  {
    get => this.AreaRecord.IsCategoryBrokenDown;
    set => this.AreaRecord.IsCategoryBrokenDown = value;
  }

  public bool IsAreaShadowed
  {
    get => this.AreaRecord.IsAreaShadowed;
    set => this.AreaRecord.IsAreaShadowed = value;
  }

  public bool IsFillSurface
  {
    get => this.SurfaceRecord.IsFillSurface;
    set => this.SurfaceRecord.IsFillSurface = value;
  }

  public bool Is3DPhongShade
  {
    get => this.SurfaceRecord.Is3DPhongShade;
    set => this.SurfaceRecord.Is3DPhongShade = value;
  }

  public bool HasShadowRadar
  {
    get => this.RadarRecord.HasShadow;
    set => this.RadarRecord.HasShadow = value;
  }

  public bool HasRadarAxisLabels
  {
    get
    {
      return this.m_serieFormat.TypeCode != TBIFFRecord.ChartRadar ? this.RadarAreaRecord.IsRadarAxisLabel : this.RadarRecord.IsRadarAxisLabel;
    }
    set
    {
      if (this.m_serieFormat.TypeCode == TBIFFRecord.ChartRadarArea)
        this.RadarAreaRecord.IsRadarAxisLabel = value;
      else
        this.RadarRecord.IsRadarAxisLabel = value;
      if (((WorkbookImpl) this.m_chart.Workbook).Loading)
        return;
      this.ChangeTickLabelPosition(value);
    }
  }

  public ExcelPieType PieChartType
  {
    get => this.BoppopRecord.PieChartType;
    set => this.BoppopRecord.PieChartType = value;
  }

  public bool UseDefaultSplitValue
  {
    get => this.BoppopRecord.UseDefaultSplitValue;
    set => this.BoppopRecord.UseDefaultSplitValue = value;
  }

  public ExcelSplitType SplitType
  {
    get => this.BoppopRecord.ChartSplitType;
    set => this.BoppopRecord.ChartSplitType = value;
  }

  public int SplitValue
  {
    get => (int) this.BoppopRecord.SplitPosition;
    set
    {
      if (this.SplitType == ExcelSplitType.Percent)
        this.BoppopRecord.SplitPercent = (ushort) value;
      else
        this.BoppopRecord.SplitPosition = (ushort) value;
      this.UseDefaultSplitValue = false;
    }
  }

  public int SplitPercent
  {
    get => (int) this.BoppopRecord.SplitPercent;
    set => this.BoppopRecord.SplitPercent = (ushort) value;
  }

  public int PieSecondSize
  {
    get => (int) this.BoppopRecord.Pie2Size;
    set
    {
      this.BoppopRecord.Pie2Size = value >= 5 && value <= 200 ? (ushort) value : throw new ArgumentOutOfRangeException(nameof (PieSecondSize));
    }
  }

  public int Gap
  {
    get => (int) this.BoppopRecord.Gap;
    set => this.BoppopRecord.Gap = (ushort) value;
  }

  public int NumSplitValue
  {
    get => this.BoppopRecord.NumSplitValue;
    set => this.BoppopRecord.NumSplitValue = value;
  }

  public bool HasShadowBoppop
  {
    get => this.BoppopRecord.HasShadow;
    set => this.BoppopRecord.HasShadow = value;
  }

  public bool IsSeriesName
  {
    get => this.DataLabelsRecord.IsSeriesName;
    set => this.DataLabelsRecord.IsSeriesName = value;
  }

  public bool IsCategoryName
  {
    get => this.DataLabelsRecord.IsCategoryName;
    set => this.DataLabelsRecord.IsCategoryName = value;
  }

  public bool IsValue
  {
    get => this.DataLabelsRecord.IsValue;
    set => this.DataLabelsRecord.IsValue = value;
  }

  public bool IsPercentage
  {
    get => this.DataLabelsRecord.IsPercentage;
    set => this.DataLabelsRecord.IsPercentage = value;
  }

  public bool IsBubbleSize
  {
    get => this.DataLabelsRecord.IsBubbleSize;
    set => this.DataLabelsRecord.IsBubbleSize = value;
  }

  public int DelimiterLength => this.DataLabelsRecord.DelimiterLength;

  public string Delimiter
  {
    get => this.DataLabelsRecord.Delimiter;
    set => this.DataLabelsRecord.Delimiter = value;
  }

  public ExcelDropLineStyle LineStyle
  {
    get => this.DropLineStyle;
    set => this.DropLineStyle = value;
  }

  public ExcelDropLineStyle DropLineStyle
  {
    get
    {
      if (this.m_chartChartLine == (ChartChartLineRecord) null)
        this.m_chartChartLine = (ChartChartLineRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartChartLine);
      return this.m_chartChartLine.LineStyle;
    }
    set
    {
      this.SetDropLineStyle(value);
      switch (value)
      {
        case ExcelDropLineStyle.Drop:
          this.HasDropLines = true;
          this.HasHighLowLines = false;
          this.HasSeriesLines = false;
          break;
        case ExcelDropLineStyle.HiLow:
          this.HasDropLines = false;
          this.HasHighLowLines = true;
          this.HasSeriesLines = false;
          break;
        case ExcelDropLineStyle.Series:
          this.HasDropLines = false;
          this.HasHighLowLines = false;
          this.HasSeriesLines = true;
          break;
      }
    }
  }

  public IChartDropBar FirstDropBar
  {
    get
    {
      if (this.m_firstDropBar == null)
        this.m_firstDropBar = new ChartDropBarImpl(this.Application, (object) this);
      return (IChartDropBar) this.m_firstDropBar;
    }
  }

  public IChartDropBar SecondDropBar
  {
    get
    {
      if (this.m_secondDropBar == null)
        this.m_secondDropBar = new ChartDropBarImpl(this.Application, (object) this);
      return (IChartDropBar) this.m_secondDropBar;
    }
  }

  public IChartBorder PieSeriesLine
  {
    get
    {
      if (this.m_serieFormat.TypeCode != TBIFFRecord.ChartBoppop && this.m_chart.ChartType != ExcelChartType.Column_Stacked && this.m_chart.ChartType != ExcelChartType.Column_Stacked_100 && this.m_chart.ChartType != ExcelChartType.Bar_Stacked && this.m_chart.ChartType != ExcelChartType.Bar_Stacked_100)
        throw new ArgumentNullException("This property is not supported in this chart type");
      if (this.m_serieLine == null)
        this.m_serieLine = new ChartBorderImpl(this.Application, (object) this);
      return (IChartBorder) this.m_serieLine;
    }
    internal set
    {
      if (!this.HasSeriesLines)
        return;
      this.m_serieLine = (ChartBorderImpl) value;
    }
  }

  public int Rotation
  {
    get => (int) this.Chart3DRecord.RotationAngle;
    set
    {
      this.Chart3DRecord.RotationAngle = value >= 0 && value <= 360 ? (ushort) value : throw new ArgumentOutOfRangeException(nameof (Rotation));
    }
  }

  public bool IsDefaultRotation => this.Chart3DRecord.IsDefaultRotation;

  public int Elevation
  {
    get => (int) this.Chart3DRecord.ElevationAngle;
    set
    {
      this.Chart3DRecord.ElevationAngle = value >= -90 && value <= 90 ? (short) value : throw new ArgumentOutOfRangeException(nameof (Elevation));
    }
  }

  public bool IsDefaultElevation => this.Chart3DRecord.IsDefaultElevation;

  public int Perspective
  {
    get => (int) this.Chart3DRecord.DistanceFromEye;
    set
    {
      this.Chart3DRecord.DistanceFromEye = value >= 0 && value <= 100 ? (ushort) value : throw new ArgumentOutOfRangeException("Elevation");
      this.Chart3DRecord.IsPerspective = true;
    }
  }

  public int HeightPercent
  {
    get => (int) this.Chart3DRecord.Height;
    set
    {
      this.Chart3DRecord.Height = value >= 5 && value <= 500 ? (ushort) value : throw new ArgumentOutOfRangeException("Elevation");
      this.Chart3DRecord.IsAutoScaled = false;
    }
  }

  public int DepthPercent
  {
    get => (int) this.Chart3DRecord.Depth;
    set
    {
      this.Chart3DRecord.Depth = value >= 20 && value <= 2000 ? (ushort) value : throw new ArgumentOutOfRangeException(nameof (DepthPercent));
    }
  }

  public int GapDepth
  {
    get => (int) this.Chart3DRecord.SeriesSpace;
    set
    {
      this.Chart3DRecord.SeriesSpace = value >= 0 && value <= 500 ? (ushort) value : throw new ArgumentOutOfRangeException(nameof (GapDepth));
    }
  }

  public bool RightAngleAxes
  {
    get => !this.Chart3DRecord.IsPerspective;
    set => this.Chart3DRecord.IsPerspective = !value;
  }

  public bool IsClustered
  {
    get => this.Chart3DRecord.IsClustered;
    set => this.Chart3DRecord.IsClustered = value;
  }

  internal bool IsChartExType
  {
    get => this.m_isChartExType;
    set => this.m_isChartExType = value;
  }

  public bool AutoScaling
  {
    get => this.Chart3DRecord.IsAutoScaled;
    set => this.Chart3DRecord.IsAutoScaled = value;
  }

  public bool WallsAndGridlines2D
  {
    get => this.Chart3DRecord.Is2DWalls;
    set => this.Chart3DRecord.Is2DWalls = value;
  }

  private ChartBarRecord BarRecord
  {
    get
    {
      return this.m_serieFormat.TypeCode == TBIFFRecord.ChartBar ? this.m_serieFormat as ChartBarRecord : throw new NotSupportedException("This property is not suported in current chart type.");
    }
  }

  private ChartLineRecord LineRecord
  {
    get
    {
      return this.m_serieFormat.TypeCode == TBIFFRecord.ChartLine ? this.m_serieFormat as ChartLineRecord : throw new NotSupportedException("This property is not suported in current chart type.");
    }
  }

  private ChartPieRecord PieRecord
  {
    get
    {
      if (this.m_serieFormat.TypeCode == TBIFFRecord.ChartBoppop)
        return (ChartPieRecord) null;
      if (this.m_serieFormat.TypeCode != TBIFFRecord.ChartPie && !(this.m_chart.Workbook as WorkbookImpl).Loading)
        throw new NotSupportedException("This property is not suported in current chart type.");
      return this.m_serieFormat as ChartPieRecord;
    }
  }

  private ChartScatterRecord ScatterRecord
  {
    get
    {
      return this.m_serieFormat.TypeCode == TBIFFRecord.ChartScatter ? this.m_serieFormat as ChartScatterRecord : throw new NotSupportedException("This property is not suported in current chart type.");
    }
  }

  private ChartAreaRecord AreaRecord
  {
    get
    {
      return this.m_serieFormat.TypeCode == TBIFFRecord.ChartArea ? this.m_serieFormat as ChartAreaRecord : throw new NotSupportedException("This property is not suported in current chart type.");
    }
  }

  private ChartSurfaceRecord SurfaceRecord
  {
    get
    {
      return this.m_serieFormat.TypeCode == TBIFFRecord.ChartSurface ? this.m_serieFormat as ChartSurfaceRecord : throw new NotSupportedException("This property is not suported in current chart type.");
    }
  }

  private ChartRadarRecord RadarRecord
  {
    get
    {
      return this.m_serieFormat.TypeCode == TBIFFRecord.ChartRadar ? this.m_serieFormat as ChartRadarRecord : throw new NotSupportedException("This property is not suported in current chart type.");
    }
  }

  private ChartRadarAreaRecord RadarAreaRecord
  {
    get
    {
      return this.m_serieFormat.TypeCode == TBIFFRecord.ChartRadarArea ? this.m_serieFormat as ChartRadarAreaRecord : throw new NotSupportedException("This property is not suported in current chart type.");
    }
  }

  private ChartBoppopRecord BoppopRecord
  {
    get
    {
      return this.m_serieFormat.TypeCode == TBIFFRecord.ChartBoppop ? this.m_serieFormat as ChartBoppopRecord : throw new NotSupportedException("This property is not suported in current chart type.");
    }
  }

  private ChartDataLabelsRecord DataLabelsRecord
  {
    get
    {
      if (this.m_dataLabels == (ChartDataLabelsRecord) null)
        this.m_dataLabels = (ChartDataLabelsRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartDataLabels);
      return this.m_dataLabels;
    }
  }

  private ChartSerieDataFormatImpl DataFormat
  {
    get
    {
      if (this.m_dataFormat == null)
        this.m_dataFormat = new ChartSerieDataFormatImpl(this.Application, (object) this);
      return this.m_dataFormat;
    }
  }

  private ChartChartFormatRecord ChartChartFormatRecord
  {
    get
    {
      if (this.m_chartChartFormat == null)
        this.m_chartChartFormat = (ChartChartFormatRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartChartFormat);
      return this.m_chartChartFormat;
    }
  }

  private Chart3DRecord Chart3DRecord
  {
    get
    {
      if (this.m_chart3D == (Chart3DRecord) null)
        this.m_chart3D = (Chart3DRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Chart3D);
      return this.m_chart3D;
    }
  }

  public bool IsPrimaryAxis => this.m_parentAxis.IsPrimary;

  public bool IsChartChartLine => this.m_chartChartLine != (ChartChartLineRecord) null;

  public bool IsChartLineFormat
  {
    get => this.m_highlowLine != null || this.m_dropLine != null || this.m_serieLine != null;
  }

  public bool IsDropBar => this.m_firstDropBar != null;

  [CLSCompliant(false)]
  public BiffRecordRaw SerieFormat
  {
    get
    {
      return this.m_serieFormat != null ? this.m_serieFormat : throw new ArgumentNullException("m_serieFormat");
    }
  }

  public int DrawingZOrder
  {
    get => (int) this.ChartChartFormatRecord.DrawingZOrder;
    set
    {
      if ((int) this.ChartChartFormatRecord.DrawingZOrder == value)
        return;
      this.ChartChartFormatRecord.DrawingZOrder = (ushort) value;
    }
  }

  public TBIFFRecord FormatRecordType => this.m_serieFormat.TypeCode;

  public bool Is3D => this.m_chart3D != (Chart3DRecord) null;

  public ChartSerieDataFormatImpl DataFormatOrNull => this.m_dataFormat;

  public bool IsMarker => this.m_dataFormat == null || this.m_dataFormat.IsMarker;

  public bool IsLine => this.m_dataFormat == null || this.m_dataFormat.IsLine;

  public bool IsSmoothed => this.m_dataFormat != null && this.m_dataFormat.IsSmoothed;

  internal IChartBorder HighLowLineProperties
  {
    get => this.HighLowLines;
    set => this.HighLowLines = value;
  }

  public IChartBorder HighLowLines
  {
    get
    {
      if (this.m_serieFormat.TypeCode != TBIFFRecord.ChartLine || this.Is3D)
        throw new ArgumentNullException("This property is not supported in this chart type");
      if (this.m_highlowLine == null)
        this.m_highlowLine = new ChartBorderImpl(this.Application, (object) this);
      return (IChartBorder) this.m_highlowLine;
    }
    internal set
    {
      if (!this.HasHighLowLines)
        return;
      this.m_highlowLine = (ChartBorderImpl) value;
    }
  }

  public IChartBorder DropLines
  {
    get
    {
      if (this.m_serieFormat.TypeCode != TBIFFRecord.ChartLine && this.m_serieFormat.TypeCode != TBIFFRecord.ChartArea)
        throw new ArgumentNullException("This property is not supported in this chart type");
      if (this.m_dropLine == null)
        this.m_dropLine = new ChartBorderImpl(this.Application, (object) this);
      return (IChartBorder) this.m_dropLine;
    }
    internal set
    {
      if (!this.HasDropLines)
        return;
      this.m_dropLine = (ChartBorderImpl) value;
    }
  }

  public bool HasDropLines
  {
    get
    {
      return this.m_chartChartLine != (ChartChartLineRecord) null && this.m_chartChartLine.HasDropLine;
    }
    set
    {
      if (this.m_chartChartLine != (ChartChartLineRecord) null)
        this.m_chartChartLine.HasDropLine = value;
      if (!value)
        return;
      this.SetDropLineStyle(ExcelDropLineStyle.Drop);
    }
  }

  public bool HasHighLowLines
  {
    get
    {
      return this.m_chartChartLine != (ChartChartLineRecord) null && this.m_chartChartLine.HasHighLowLine;
    }
    set
    {
      if (this.m_chartChartLine != (ChartChartLineRecord) null)
        this.m_chartChartLine.HasHighLowLine = value;
      if (!value)
        return;
      this.SetDropLineStyle(ExcelDropLineStyle.HiLow);
    }
  }

  public bool HasSeriesLines
  {
    get
    {
      return this.m_chartChartLine != (ChartChartLineRecord) null && this.m_chartChartLine.HasSeriesLine;
    }
    set
    {
      if (this.m_chartChartLine != (ChartChartLineRecord) null)
        this.m_chartChartLine.HasSeriesLine = value;
      if (!value)
        return;
      this.SetDropLineStyle(ExcelDropLineStyle.Series);
    }
  }

  public ChartFormatImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.SetParents();
    this.m_chartChartFormat = (ChartChartFormatRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartChartFormat);
    this.m_serieFormat = BiffRecordFactory.GetRecord(TBIFFRecord.ChartBar);
  }

  public void SetParents()
  {
    this.m_parentAxis = (ChartParentAxisImpl) this.FindParent(typeof (ChartParentAxisImpl));
    this.m_chart = this.m_parentAxis != null ? this.m_parentAxis.m_parentChart : throw new ArgumentNullException("Cannot find parent axis object.");
  }

  [CLSCompliant(false)]
  public void Parse(IList<BiffRecordRaw> data, ref int iPos)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    data[iPos].CheckTypeCode(TBIFFRecord.ChartChartFormat);
    this.m_chartChartFormat = (ChartChartFormatRecord) data[iPos];
    ++iPos;
    data[iPos].CheckTypeCode(TBIFFRecord.Begin);
    ++iPos;
    int num1 = 1;
    int num2 = 0;
    while (num1 != 0)
    {
      BiffRecordRaw line = data[iPos];
      switch (line.TypeCode)
      {
        case TBIFFRecord.ChartDataLabels:
          this.m_dataLabels = (ChartDataLabelsRecord) line;
          break;
        case TBIFFRecord.ChartDataFormat:
          this.m_dataFormat = new ChartSerieDataFormatImpl(this.Application, (object) this);
          iPos = this.m_dataFormat.Parse(data, iPos) - 1;
          break;
        case TBIFFRecord.ChartLineFormat:
          if (this.DropLineStyle == ExcelDropLineStyle.Series)
          {
            this.m_serieLine = new ChartBorderImpl(this.Application, (object) this, (ChartLineFormatRecord) line);
            break;
          }
          if (this.DropLineStyle == ExcelDropLineStyle.HiLow)
          {
            this.m_highlowLine = new ChartBorderImpl(this.Application, (object) this, (ChartLineFormatRecord) line);
            break;
          }
          if (this.DropLineStyle == ExcelDropLineStyle.Drop)
          {
            this.m_dropLine = new ChartBorderImpl(this.Application, (object) this, (ChartLineFormatRecord) line);
            break;
          }
          break;
        case TBIFFRecord.ChartLegend:
          this.m_chart.ParseLegend(data, ref iPos);
          --iPos;
          break;
        case TBIFFRecord.ChartSeriesList:
          this.m_seriesList = (ChartSeriesListRecord) line;
          break;
        case TBIFFRecord.ChartBar:
        case TBIFFRecord.ChartLine:
        case TBIFFRecord.ChartPie:
        case TBIFFRecord.ChartArea:
        case TBIFFRecord.ChartScatter:
        case TBIFFRecord.ChartRadar:
        case TBIFFRecord.ChartSurface:
        case TBIFFRecord.ChartRadarArea:
        case TBIFFRecord.ChartBoppop:
          this.m_serieFormat = line;
          break;
        case TBIFFRecord.ChartChartLine:
          this.m_chartChartLine = (ChartChartLineRecord) line;
          break;
        case TBIFFRecord.ChartFormatLink:
          this.m_formatLink = (ChartFormatLinkRecord) line;
          break;
        case TBIFFRecord.Begin:
          iPos = BiffRecordRaw.SkipBeginEndBlock(data, iPos) - 1;
          break;
        case TBIFFRecord.End:
          --num1;
          break;
        case TBIFFRecord.Chart3D:
          this.m_chart3D = (Chart3DRecord) line;
          break;
        case TBIFFRecord.ChartDropBar:
          if (num2 > 1)
            throw new ParseException("Find more then two ChartBarRecords.");
          ChartDropBarImpl chartDropBarImpl = new ChartDropBarImpl(this.Application, (object) this);
          chartDropBarImpl.Parse(data, ref iPos);
          if (num2 == 0)
            this.m_firstDropBar = chartDropBarImpl;
          else
            this.m_secondDropBar = chartDropBarImpl;
          ++num2;
          break;
      }
      ++iPos;
    }
    if (!(this.m_chartChartLine != (ChartChartLineRecord) null) || this.m_chartChartLine.LineStyle != ExcelDropLineStyle.HiLow || this.m_dataFormat == null || !this.m_dataFormat.HasLineProperties || this.m_dataFormat.LineProperties.AutoFormat || this.m_dataFormat.LineProperties.LinePattern != ExcelChartLinePattern.None)
      return;
    this.m_chart.IsStock = true;
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    records.Add((IBiffStorage) this.m_chartChartFormat.Clone());
    records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.Begin));
    records.Add((IBiffStorage) this.m_serieFormat.Clone());
    if (this.m_formatLink != null)
      records.Add((IBiffStorage) this.m_formatLink.Clone());
    if (this.m_seriesList != (ChartSeriesListRecord) null)
      records.Add((IBiffStorage) this.m_seriesList.Clone());
    if (this.m_chart3D != (Chart3DRecord) null)
      records.Add((IBiffStorage) this.m_chart3D.Clone());
    if (this.DrawingZOrder == 0)
      this.m_chart.SerializeLegend(records);
    if (this.m_firstDropBar != null)
      this.m_firstDropBar.Serialize(records);
    if (this.m_secondDropBar != null)
      this.m_secondDropBar.Serialize(records);
    if (this.m_chartChartLine != (ChartChartLineRecord) null)
      records.Add((IBiffStorage) this.m_chartChartLine.Clone());
    if (this.m_serieLine != null)
      this.m_serieLine.Serialize((IList<IBiffStorage>) records);
    if (this.m_highlowLine != null)
      this.m_highlowLine.Serialize((IList<IBiffStorage>) records);
    if (this.m_dropLine != null)
      this.m_dropLine.Serialize((IList<IBiffStorage>) records);
    if (this.m_dataFormat != null)
      this.m_dataFormat.Serialize(records);
    if (this.m_dataLabels != (ChartDataLabelsRecord) null)
      records.Add((IBiffStorage) this.m_dataLabels.Clone());
    records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.End));
  }

  public static string GetStartSerieType(ExcelChartType type)
  {
    if (type == ExcelChartType.PieOfPie)
      return "Pie";
    string str = type.ToString();
    int length = str.IndexOf('_');
    return length == -1 ? str : str.Substring(0, length);
  }

  public void ChangeChartType(ExcelChartType type, bool isSeriesCreation)
  {
    this.ChangeSerieType(type, isSeriesCreation);
  }

  private void SetDropLineStyle(ExcelDropLineStyle value)
  {
    if (this.m_chartChartLine == (ChartChartLineRecord) null)
      this.m_chartChartLine = (ChartChartLineRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartChartLine);
    this.m_chartChartLine.LineStyle = value;
  }

  private void SetNullForAllRecords()
  {
    this.m_chart3D = (Chart3DRecord) null;
    this.m_chartChartLine = (ChartChartLineRecord) null;
    this.m_serieLine = (ChartBorderImpl) null;
    this.m_highlowLine = (ChartBorderImpl) null;
    this.m_dropLine = (ChartBorderImpl) null;
    this.m_dataFormat = (ChartSerieDataFormatImpl) null;
    this.m_dataLabels = (ChartDataLabelsRecord) null;
    this.m_firstDropBar = (ChartDropBarImpl) null;
    this.m_secondDropBar = (ChartDropBarImpl) null;
    this.m_seriesList = (ChartSeriesListRecord) null;
  }

  private void ChangeChartStockLine()
  {
    ExcelChartType destinationType = this.m_chart.DestinationType;
    this.m_chart.DestinationType = ExcelChartType.Line;
    this.m_serieFormat = BiffRecordFactory.GetRecord(TBIFFRecord.ChartLine);
    this.m_serieLine = new ChartBorderImpl(this.Application, (object) this);
    this.m_highlowLine = new ChartBorderImpl(this.Application, (object) this);
    this.m_dropLine = new ChartBorderImpl(this.Application, (object) this);
    this.m_serieLine.LineWeight = ExcelChartLineWeight.Hairline;
    this.m_highlowLine.LineWeight = ExcelChartLineWeight.Hairline;
    this.m_dropLine.LineWeight = ExcelChartLineWeight.Hairline;
    this.m_serieLine.ColorIndex = ExcelKnownColors.Turquoise | ExcelKnownColors.BlackCustom;
    this.m_highlowLine.ColorIndex = ExcelKnownColors.Turquoise | ExcelKnownColors.BlackCustom;
    this.m_dropLine.ColorIndex = ExcelKnownColors.Turquoise | ExcelKnownColors.BlackCustom;
    this.DropLineStyle = ExcelDropLineStyle.HiLow;
    this.HasHighLowLines = true;
    IChartBorder lineProperties = this.SerieDataFormat.LineProperties;
    lineProperties.LineWeight = ExcelChartLineWeight.Hairline;
    lineProperties.ColorIndex = ExcelKnownColors.Turquoise | ExcelKnownColors.BlackCustom;
    lineProperties.LinePattern = ExcelChartLinePattern.None;
    this.m_dataFormat.SeriesNumber = 65533;
    this.m_dataFormat.MarkerStyle = ExcelChartMarkerType.None;
    this.m_dataFormat.MarkerForegroundColorIndex = ExcelKnownColors.YellowCustom | ExcelKnownColors.BlackCustom;
    this.m_dataFormat.MarkerBackgroundColorIndex = ExcelKnownColors.YellowCustom | ExcelKnownColors.BlackCustom;
    this.m_dataFormat.IsAutoMarker = false;
    this.m_chart.PrimaryCategoryAxis.IsBetween = true;
    IChartGridLine majorGridLines = this.m_chart.PrimaryValueAxis.MajorGridLines;
    this.m_chart.DestinationType = destinationType;
  }

  public void ChangeChartStockHigh_Low_CloseType()
  {
    ExcelChartType destinationType = this.m_chart.DestinationType;
    this.m_chart.DestinationType = ExcelChartType.Line;
    this.ChangeChartStockLine();
    ((ChartDataPointImpl) this.m_chart.Series[2].DataPoints.DefaultDataPoint).ChangeChartStockHigh_Low_CloseType();
    this.m_chart.DestinationType = destinationType;
  }

  public void ChangeChartStockOpen_High_Low_CloseType()
  {
    this.ChangeChartStockLine();
    this.FirstDropBar.Gap = 150;
    IChartBorder lineProperties = this.m_firstDropBar.LineProperties;
    lineProperties.LinePattern = ExcelChartLinePattern.Solid;
    lineProperties.LineWeight = ExcelChartLineWeight.Hairline;
    IChartInterior interior1 = this.m_firstDropBar.Interior;
    interior1.Pattern = ExcelPattern.Solid;
    lineProperties.ColorIndex = ExcelKnownColors.Turquoise | ExcelKnownColors.BlackCustom;
    lineProperties.AutoFormat = true;
    interior1.ForegroundColorIndex = ExcelKnownColors.WhiteCustom;
    interior1.BackgroundColorIndex = ExcelKnownColors.Custom0;
    this.m_secondDropBar = this.m_firstDropBar.Clone((object) this);
    IChartInterior interior2 = this.m_secondDropBar.Interior;
    interior2.ForegroundColorIndex = ExcelKnownColors.Custom0;
    interior2.BackgroundColorIndex = ExcelKnownColors.WhiteCustom;
  }

  public void ChangeChartStockVolume_High_Low_CloseTypeFirst()
  {
    this.m_serieFormat = BiffRecordFactory.GetRecord(TBIFFRecord.ChartBar);
    this.IsVaryColor = false;
  }

  public void ChangeChartStockVolume_High_Low_CloseTypeSecond()
  {
    this.ChangeChartStockLine();
    ushort[] numArray = new ushort[4]
    {
      (ushort) 1,
      (ushort) 2,
      (ushort) 3,
      (ushort) 4
    };
    this.m_seriesList = (ChartSeriesListRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartSeriesList);
    this.m_seriesList.Series = numArray;
    this.m_chart.SecondaryParentAxis.UpdateSecondaryAxis(true);
    for (int index = 1; index < 4; ++index)
    {
      ChartSerieImpl chartSerieImpl = (ChartSerieImpl) this.m_chart.Series[index];
      chartSerieImpl.ChartGroup = 1;
      if (index == 3)
        ((ChartDataPointImpl) chartSerieImpl.DataPoints.DefaultDataPoint).ChangeChartStockVolume_High_Low_CloseType();
    }
  }

  public void ChangeChartStockVolume_Open_High_Low_CloseType()
  {
    this.ChangeChartStockOpen_High_Low_CloseType();
    this.FirstDropBar.Gap = 100;
    this.SecondDropBar.Gap = 100;
    IChartInterior interior = this.m_firstDropBar.Interior;
    interior.ForegroundColorIndex = ExcelKnownColors.WhiteCustom;
    interior.BackgroundColorIndex = ExcelKnownColors.Custom0;
    ExcelChartType destinationType = this.m_chart.DestinationType;
    this.m_chart.DestinationType = ExcelChartType.Line;
    ((ChartSerieDataFormatImpl) this.SerieDataFormat).SeriesNumber = 65533;
    this.m_chart.DestinationType = destinationType;
    this.SecondDropBar.Interior.BackgroundColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
    this.m_seriesList = (ChartSeriesListRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartSeriesList);
    this.m_seriesList.Series = new ushort[5]
    {
      (ushort) 1,
      (ushort) 2,
      (ushort) 3,
      (ushort) 4,
      (ushort) 5
    };
    for (int index = 1; index < 5; ++index)
      ((ChartSerieImpl) this.m_chart.Series[index]).ChartGroup = 1;
  }

  public void ChangeSerieType(ExcelChartType type, bool isSeriesCreation)
  {
    if (this.m_isChartExType)
      this.m_isChartExType = false;
    switch (type)
    {
      case ExcelChartType.Column_Clustered:
      case ExcelChartType.Column_Clustered_3D:
      case ExcelChartType.Column_3D:
      case ExcelChartType.Bar_Clustered:
      case ExcelChartType.Bar_Clustered_3D:
        this.ChangeSerieBarClustered(type);
        break;
      case ExcelChartType.Column_Stacked:
      case ExcelChartType.Column_Stacked_100:
      case ExcelChartType.Column_Stacked_3D:
      case ExcelChartType.Column_Stacked_100_3D:
      case ExcelChartType.Bar_Stacked:
      case ExcelChartType.Bar_Stacked_100:
      case ExcelChartType.Bar_Stacked_3D:
      case ExcelChartType.Bar_Stacked_100_3D:
        this.ChangeSerieBarStacked(type);
        break;
      case ExcelChartType.Line:
      case ExcelChartType.Line_Stacked:
      case ExcelChartType.Line_Stacked_100:
      case ExcelChartType.Line_Markers:
      case ExcelChartType.Line_Markers_Stacked:
      case ExcelChartType.Line_Markers_Stacked_100:
      case ExcelChartType.Line_3D:
        this.ChangeSerieLine(type);
        break;
      case ExcelChartType.Pie:
      case ExcelChartType.Pie_3D:
      case ExcelChartType.PieOfPie:
      case ExcelChartType.Pie_Exploded:
      case ExcelChartType.Pie_Exploded_3D:
      case ExcelChartType.Pie_Bar:
        this.ChangeSeriePie(type);
        break;
      case ExcelChartType.Scatter_Markers:
      case ExcelChartType.Scatter_SmoothedLine_Markers:
      case ExcelChartType.Scatter_SmoothedLine:
      case ExcelChartType.Scatter_Line_Markers:
      case ExcelChartType.Scatter_Line:
        this.ChangeSerieScatter(type);
        break;
      case ExcelChartType.Area:
      case ExcelChartType.Area_Stacked:
      case ExcelChartType.Area_Stacked_100:
      case ExcelChartType.Area_3D:
      case ExcelChartType.Area_Stacked_3D:
      case ExcelChartType.Area_Stacked_100_3D:
        this.ChangeSerieArea(type);
        break;
      case ExcelChartType.Doughnut:
      case ExcelChartType.Doughnut_Exploded:
        this.ChangeSerieDoughnut(type);
        break;
      case ExcelChartType.Radar:
      case ExcelChartType.Radar_Markers:
      case ExcelChartType.Radar_Filled:
        this.ChangeSerieRadar(type);
        break;
      case ExcelChartType.Surface_3D:
      case ExcelChartType.Surface_NoColor_3D:
      case ExcelChartType.Surface_Contour:
      case ExcelChartType.Surface_NoColor_Contour:
        this.ChangeSerieSurface(type, isSeriesCreation);
        break;
      case ExcelChartType.Bubble:
      case ExcelChartType.Bubble_3D:
        this.ChangeSerieBuble(type, isSeriesCreation);
        break;
      case ExcelChartType.Cylinder_Clustered:
      case ExcelChartType.Cylinder_Stacked:
      case ExcelChartType.Cylinder_Stacked_100:
      case ExcelChartType.Cylinder_Bar_Clustered:
      case ExcelChartType.Cylinder_Bar_Stacked:
      case ExcelChartType.Cylinder_Bar_Stacked_100:
      case ExcelChartType.Cylinder_Clustered_3D:
      case ExcelChartType.Cone_Clustered:
      case ExcelChartType.Cone_Stacked:
      case ExcelChartType.Cone_Stacked_100:
      case ExcelChartType.Cone_Bar_Clustered:
      case ExcelChartType.Cone_Bar_Stacked:
      case ExcelChartType.Cone_Bar_Stacked_100:
      case ExcelChartType.Cone_Clustered_3D:
      case ExcelChartType.Pyramid_Clustered:
      case ExcelChartType.Pyramid_Stacked:
      case ExcelChartType.Pyramid_Stacked_100:
      case ExcelChartType.Pyramid_Bar_Clustered:
      case ExcelChartType.Pyramid_Bar_Stacked:
      case ExcelChartType.Pyramid_Bar_Stacked_100:
      case ExcelChartType.Pyramid_Clustered_3D:
        this.ChangeSerieConeCylinderPyramyd(type);
        break;
      case ExcelChartType.Funnel:
      case ExcelChartType.WaterFall:
      case ExcelChartType.BoxAndWhisker:
      case ExcelChartType.Histogram:
      case ExcelChartType.Pareto:
      case ExcelChartType.TreeMap:
      case ExcelChartType.SunBurst:
        this.SetNullForAllRecords();
        this.m_serieFormat = BiffRecordFactory.GetRecord(TBIFFRecord.ChartBar);
        this.m_isChartExType = true;
        break;
      default:
        throw new NotSupportedException("Cannot change serie type.");
    }
    if (this.m_isChartExType)
      return;
    string startSerieType = ChartFormatImpl.GetStartSerieType(type);
    if (this.m_chart.ParentWorkbook.Loading)
      return;
    this.m_chart.PrimaryCategoryAxis.IsBetween = !(startSerieType == "Area") && !(startSerieType == "Surface");
  }

  private void ChangeSerieDoughnut(ExcelChartType type)
  {
    this.SetNullForAllRecords();
    this.m_serieFormat = BiffRecordFactory.GetRecord(TBIFFRecord.ChartPie);
    this.m_chartChartFormat.IsVaryColor = true;
    this.DoughnutHoleSize = 50;
    if (type != ExcelChartType.Doughnut_Exploded)
      return;
    this.SerieDataFormat.Percent = 25;
  }

  private void ChangeSerieBuble(ExcelChartType type, bool isSeriesCreation)
  {
    this.SetNullForAllRecords();
    this.m_serieFormat = BiffRecordFactory.GetRecord(TBIFFRecord.ChartScatter);
    this.SizeRepresents = ExcelBubbleSize.Area;
    this.BubbleScale = 100;
    this.IsBubbles = true;
    if (type == ExcelChartType.Bubble_3D)
      this.SerieDataFormat.Is3DBubbles = true;
    if (isSeriesCreation)
      return;
    this.UpdateBubbleSeries(this.m_chart.Series);
  }

  private void UpdateBubbleSeries(IChartSeries series)
  {
    if (series == null)
      throw new ArgumentNullException(nameof (series));
    int count = this.m_chart.Series.Count;
    for (int index = 0; index < series.Count - 1; index = index - 1 + 2)
    {
      ChartSerieImpl chartSerieImpl = (ChartSerieImpl) series[index];
      IChartSerie chartSerie = series[index + 1];
      chartSerieImpl.Bubbles = chartSerie.Values;
      chartSerieImpl.Index = index;
      chartSerieImpl.Number = index;
      series.RemoveAt(index + 1);
    }
    if (count % 2 == 0)
      return;
    IChartSerie chartSerie1 = series[0];
    IRange values = chartSerie1.Values;
    int length = chartSerie1.Values != null ? Math.Max(values.LastRow - values.Row + 1, values.LastColumn - values.Column + 1) : chartSerie1.EnteredDirectlyValues.Length;
    object[] objArray = new object[length];
    for (int index = 0; index < length; ++index)
      objArray[index] = (object) 0;
    int num = series.Count - 1;
    ChartSerieImpl chartSerieImpl1 = (ChartSerieImpl) series[series.Count - 1];
    chartSerieImpl1.EnteredDirectlyBubbles = objArray;
    chartSerieImpl1.Index = num;
    chartSerieImpl1.Number = num;
  }

  private void ChangeSerieSurface(ExcelChartType type, bool isSeriesCreation)
  {
    if (this.m_chart.Series.Count < 2 && !isSeriesCreation && !this.m_chart.ParentWorkbook.Loading)
      throw new ArgumentException("Cannot change type. Chart cannot contain less then 2 series.");
    this.SetNullForAllRecords();
    this.m_serieFormat = BiffRecordFactory.GetRecord(TBIFFRecord.ChartSurface);
    this.RightAngleAxes = false;
    if (type == ExcelChartType.Surface_3D || type == ExcelChartType.Surface_Contour)
      this.IsFillSurface = true;
    if (type != ExcelChartType.Surface_NoColor_Contour && type != ExcelChartType.Surface_Contour)
      return;
    this.Rotation = 0;
    this.Elevation = 90;
    this.Perspective = 0;
    this.IsVaryColor = false;
  }

  private void ChangeSerieRadar(ExcelChartType type)
  {
    this.SetNullForAllRecords();
    if (type == ExcelChartType.Radar_Filled)
    {
      ChartRadarAreaRecord record = (ChartRadarAreaRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartRadarArea);
      record.IsRadarAxisLabel = true;
      this.m_serieFormat = (BiffRecordRaw) record;
      this.IsCategoryName = true;
    }
    else
    {
      ChartRadarRecord record = (ChartRadarRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartRadar);
      record.IsRadarAxisLabel = true;
      this.m_serieFormat = (BiffRecordRaw) record;
      if (type != ExcelChartType.Radar)
        return;
      this.HasRadarAxisLabels = true;
      ((ChartSerieDataFormatImpl) this.SerieDataFormat).ChangeRadarDataFormat(type);
    }
  }

  private void ChangeSerieBarClustered(ExcelChartType type)
  {
    this.SetNullForAllRecords();
    this.m_serieFormat = BiffRecordFactory.GetRecord(TBIFFRecord.ChartBar);
    if (type == ExcelChartType.Column_Clustered_3D || type == ExcelChartType.Bar_Clustered_3D)
      this.IsClustered = true;
    if ((type == ExcelChartType.Column_Clustered_3D || type == ExcelChartType.Column_3D || type == ExcelChartType.Bar_Clustered_3D || type == ExcelChartType.Bar_Stacked_100_3D || type == ExcelChartType.Bar_Stacked_3D || type == ExcelChartType.Column_Stacked_100_3D || type == ExcelChartType.Column_Stacked_3D) && this.DataFormat != null)
    {
      this.DataFormat.BarShapeBase = ExcelBaseFormat.Rectangle;
      this.DataFormat.BarShapeTop = ExcelTopFormat.Straight;
    }
    if (type == ExcelChartType.Column_3D)
    {
      this.RightAngleAxes = false;
    }
    else
    {
      if (type.ToString().IndexOf("Bar_") < 0)
        return;
      this.IsHorizontalBar = true;
    }
  }

  private void ChangeSerieBarStacked(ExcelChartType type)
  {
    this.ChangeSerieBarClustered(type);
    this.StackValuesBar = true;
    this.BarRecord.Overlap = -65436;
    if (this.m_chart != null)
      this.m_chart.OverLap = -65436;
    if (type == ExcelChartType.Column_Stacked_100 || type == ExcelChartType.Bar_Stacked_100)
    {
      this.ShowAsPercentsBar = true;
    }
    else
    {
      switch (type)
      {
        case ExcelChartType.Column_Stacked_3D:
        case ExcelChartType.Bar_Stacked_3D:
          this.m_chart3D = (Chart3DRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Chart3D);
          break;
        case ExcelChartType.Column_Stacked_100_3D:
        case ExcelChartType.Bar_Stacked_100_3D:
          this.m_chart3D = (Chart3DRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Chart3D);
          this.ShowAsPercentsBar = true;
          break;
      }
    }
  }

  private void ChangeSerieLine(ExcelChartType type)
  {
    if (!this.m_chart.ParentWorkbook.Loading)
      this.SetNullForAllRecords();
    this.m_serieFormat = BiffRecordFactory.GetRecord(TBIFFRecord.ChartLine);
    if (type == ExcelChartType.Line_Markers)
      return;
    if (type == ExcelChartType.Line_3D)
    {
      this.m_chart3D = (Chart3DRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Chart3D);
      this.RightAngleAxes = false;
    }
    else
    {
      if (type == ExcelChartType.Line_Markers_Stacked || type == ExcelChartType.Line_Stacked)
        this.StackValuesLine = true;
      if (type == ExcelChartType.Line_Markers_Stacked_100 || type == ExcelChartType.Line_Stacked_100)
      {
        this.StackValuesLine = true;
        this.ShowAsPercentsLine = true;
      }
      if (type != ExcelChartType.Line && type != ExcelChartType.Line_Stacked && type != ExcelChartType.Line_Stacked_100)
        return;
      ((ChartSerieDataFormatImpl) this.SerieDataFormat).ChangeLineDataFormat(type);
    }
  }

  private void ChangeSeriePie(ExcelChartType type)
  {
    this.SetNullForAllRecords();
    this.IsVaryColor = true;
    this.m_serieFormat = BiffRecordFactory.GetRecord(TBIFFRecord.ChartPie);
    if (type == ExcelChartType.Pie_3D || type == ExcelChartType.Pie_Exploded_3D)
      this.m_chart3D = (Chart3DRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Chart3D);
    if (type == ExcelChartType.Pie_Exploded || type == ExcelChartType.Pie_Exploded_3D)
      this.SerieDataFormat.Percent = 25;
    if (type != ExcelChartType.Pie_Bar && type != ExcelChartType.PieOfPie)
      return;
    this.m_serieFormat = BiffRecordFactory.GetRecord(TBIFFRecord.ChartBoppop);
    this.UseDefaultSplitValue = true;
    this.PieChartType = ExcelPieType.Bar;
    this.PieSecondSize = 75;
    this.Gap = 100;
    this.HasSeriesLines = true;
    this.m_serieLine = new ChartBorderImpl(this.Application, (object) this);
    if (type != ExcelChartType.PieOfPie)
      return;
    this.PieChartType = ExcelPieType.Pie;
  }

  private void ChangeSerieArea(ExcelChartType type)
  {
    this.SetNullForAllRecords();
    this.m_serieFormat = BiffRecordFactory.GetRecord(TBIFFRecord.ChartArea);
    if (type == ExcelChartType.Area_3D || type == ExcelChartType.Area_Stacked_3D || type == ExcelChartType.Area_Stacked_100_3D)
      this.m_chart3D = (Chart3DRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Chart3D);
    if (type == ExcelChartType.Area_Stacked || type == ExcelChartType.Area_Stacked_3D)
      this.IsStacked = true;
    if (type == ExcelChartType.Area_Stacked_100 || type == ExcelChartType.Area_Stacked_100_3D)
    {
      this.IsStacked = true;
      this.IsCategoryBrokenDown = true;
    }
    if (type != ExcelChartType.Area_3D)
      return;
    this.RightAngleAxes = false;
  }

  private void ChangeTickLabelPosition(bool value)
  {
    if (this.Parent == this.m_chart.PrimaryFormats && this.m_chart.PrimaryCategoryAxis != null)
    {
      this.m_chart.PrimaryCategoryAxis.TickLabelPosition = value ? ExcelTickLabelPosition.TickLabelPosition_NextToAxis : ExcelTickLabelPosition.TickLabelPosition_None;
    }
    else
    {
      if (this.Parent != this.m_chart.SecondaryFormats || this.m_chart.SecondaryCategoryAxis == null || !this.m_chart.IsSecondaryCategoryAxisAvail)
        return;
      this.m_chart.SecondaryCategoryAxis.TickLabelPosition = value ? ExcelTickLabelPosition.TickLabelPosition_NextToAxis : ExcelTickLabelPosition.TickLabelPosition_None;
    }
  }

  private void ChangeSerieScatter(ExcelChartType type)
  {
    this.SetNullForAllRecords();
    this.m_serieFormat = BiffRecordFactory.GetRecord(TBIFFRecord.ChartScatter);
    this.SizeRepresents = ExcelBubbleSize.Area;
    this.BubbleScale = 100;
    if (type == ExcelChartType.Scatter_Line_Markers)
      return;
    ((ChartSerieDataFormatImpl) this.SerieDataFormat).ChangeScatterDataFormat(type);
  }

  private void ChangeSerieConeCylinderPyramyd(ExcelChartType type)
  {
    switch (type - 52)
    {
      case ExcelChartType.Column_Clustered:
      case ExcelChartType.Bar_Clustered:
      case ExcelChartType.Line_Stacked:
        this.ChangeSerieBarClustered(ExcelChartType.Column_Clustered_3D);
        break;
      case ExcelChartType.Column_Stacked:
      case ExcelChartType.Bar_Stacked:
      case ExcelChartType.Line_Stacked_100:
        this.ChangeSerieBarStacked(ExcelChartType.Column_Stacked_3D);
        break;
      case ExcelChartType.Column_Stacked_100:
      case ExcelChartType.Bar_Stacked_100:
      case ExcelChartType.Line_Markers:
        this.ChangeSerieBarStacked(ExcelChartType.Column_Stacked_100_3D);
        break;
      case ExcelChartType.Column_Clustered_3D:
      case ExcelChartType.Bar_Clustered_3D:
      case ExcelChartType.Line_Markers_Stacked:
        this.ChangeSerieBarClustered(ExcelChartType.Bar_Clustered_3D);
        break;
      case ExcelChartType.Column_Stacked_3D:
      case ExcelChartType.Bar_Stacked_3D:
      case ExcelChartType.Line_Markers_Stacked_100:
        this.ChangeSerieBarStacked(ExcelChartType.Bar_Stacked_3D);
        break;
      case ExcelChartType.Column_Stacked_100_3D:
      case ExcelChartType.Bar_Stacked_100_3D:
      case ExcelChartType.Line_3D:
        this.ChangeSerieBarStacked(ExcelChartType.Bar_Stacked_100_3D);
        break;
      case ExcelChartType.Column_3D:
      case ExcelChartType.Line:
      case ExcelChartType.Pie:
        this.ChangeSerieBarClustered(ExcelChartType.Column_3D);
        break;
    }
    ExcelBaseFormat baseFormat;
    ExcelTopFormat topFormat;
    switch (type - 52)
    {
      case ExcelChartType.Column_Clustered:
      case ExcelChartType.Column_Stacked:
      case ExcelChartType.Column_Stacked_100:
      case ExcelChartType.Column_Clustered_3D:
      case ExcelChartType.Column_Stacked_3D:
      case ExcelChartType.Column_Stacked_100_3D:
      case ExcelChartType.Column_3D:
        baseFormat = ExcelBaseFormat.Circle;
        topFormat = ExcelTopFormat.Straight;
        break;
      case ExcelChartType.Bar_Clustered:
      case ExcelChartType.Bar_Stacked:
      case ExcelChartType.Bar_Stacked_100:
      case ExcelChartType.Bar_Clustered_3D:
      case ExcelChartType.Bar_Stacked_3D:
      case ExcelChartType.Bar_Stacked_100_3D:
      case ExcelChartType.Line:
        baseFormat = ExcelBaseFormat.Circle;
        topFormat = ExcelTopFormat.Sharp;
        break;
      case ExcelChartType.Line_Stacked:
      case ExcelChartType.Line_Stacked_100:
      case ExcelChartType.Line_Markers:
      case ExcelChartType.Line_Markers_Stacked:
      case ExcelChartType.Line_Markers_Stacked_100:
      case ExcelChartType.Line_3D:
      case ExcelChartType.Pie:
        baseFormat = ExcelBaseFormat.Rectangle;
        topFormat = ExcelTopFormat.Sharp;
        break;
      default:
        throw new ArgumentException(nameof (type));
    }
    ((ChartSeriesCollection) this.m_chart.Series).UpdateDataPointForCylConePurChartType(baseFormat, topFormat);
    this.SerieDataFormat.BarShapeBase = baseFormat;
    this.SerieDataFormat.BarShapeTop = topFormat;
  }

  public object Clone(object parent)
  {
    ChartFormatImpl parent1 = (ChartFormatImpl) this.MemberwiseClone();
    parent1.SetParent(parent);
    parent1.SetParents();
    parent1.m_chartChartFormat = (ChartChartFormatRecord) CloneUtils.CloneCloneable((ICloneable) this.m_chartChartFormat);
    parent1.m_serieFormat = (BiffRecordRaw) CloneUtils.CloneCloneable((ICloneable) this.m_serieFormat);
    parent1.m_chart3D = (Chart3DRecord) CloneUtils.CloneCloneable((ICloneable) this.m_chart3D);
    parent1.m_formatLink = (ChartFormatLinkRecord) CloneUtils.CloneCloneable((ICloneable) this.m_formatLink);
    parent1.m_dataLabels = (ChartDataLabelsRecord) CloneUtils.CloneCloneable((ICloneable) this.m_dataLabels);
    parent1.m_chartChartLine = (ChartChartLineRecord) CloneUtils.CloneCloneable((ICloneable) this.m_chartChartLine);
    if (this.m_serieLine != null)
      parent1.m_serieLine = this.m_serieLine.Clone((object) parent1);
    if (this.m_highlowLine != null)
      parent1.m_highlowLine = this.m_highlowLine.Clone((object) parent1);
    if (this.m_dropLine != null)
      parent1.m_dropLine = this.m_dropLine.Clone((object) parent1);
    parent1.m_seriesList = (ChartSeriesListRecord) CloneUtils.CloneCloneable((ICloneable) this.m_seriesList);
    if (this.m_firstDropBar != null)
      parent1.m_firstDropBar = this.m_firstDropBar.Clone((object) parent1);
    if (this.m_secondDropBar != null)
      parent1.m_secondDropBar = this.m_secondDropBar.Clone((object) parent1);
    if (this.m_dataFormat != null)
      parent1.m_dataFormat = this.m_dataFormat.Clone((object) parent1);
    return (object) parent1;
  }

  public static bool operator ==(ChartFormatImpl format1, ChartFormatImpl format2)
  {
    if (object.Equals((object) format1, (object) null) && object.Equals((object) format2, (object) null))
      return true;
    if (object.Equals((object) format1, (object) null) || object.Equals((object) format2, (object) null) || format1.m_serieFormat.TypeCode != format2.m_serieFormat.TypeCode)
      return false;
    int storeSize1 = format1.m_serieFormat.GetStoreSize(ExcelVersion.Excel97to2003);
    int storeSize2 = format1.m_serieFormat.GetStoreSize(ExcelVersion.Excel97to2003);
    if (storeSize1 != storeSize2)
      return false;
    ByteArrayDataProvider provider1 = new ByteArrayDataProvider(new byte[storeSize1]);
    format1.m_serieFormat.InfillInternalData((DataProvider) provider1, 0, ExcelVersion.Excel97to2003);
    ByteArrayDataProvider provider2 = new ByteArrayDataProvider(new byte[storeSize2]);
    format2.m_serieFormat.InfillInternalData((DataProvider) provider2, 0, ExcelVersion.Excel97to2003);
    return BiffRecordRaw.CompareArrays(provider1.InternalBuffer, provider2.InternalBuffer) && format1.m_chartChartFormat.EqualsWithoutOrder(format2.m_chartChartFormat) && format1.m_chart3D == format2.m_chart3D && format1.m_seriesList == format2.m_seriesList && format1.m_chartChartLine == format2.m_chartChartLine && format1.m_dataLabels == format2.m_dataLabels;
  }

  public static bool operator !=(ChartFormatImpl format1, ChartFormatImpl format2)
  {
    return !(format1 == format2);
  }

  internal void InitializeStockFormat()
  {
    if (this.m_highlowLine == null)
    {
      this.m_highlowLine = new ChartBorderImpl(this.Application, (object) this);
      this.m_highlowLine.LineWeight = ExcelChartLineWeight.Hairline;
      this.m_highlowLine.ColorIndex = ExcelKnownColors.Turquoise | ExcelKnownColors.BlackCustom;
      this.m_highlowLine.AutoFormat = true;
    }
    IChartBorder lineProperties1 = this.SerieDataFormat.LineProperties;
    lineProperties1.LineWeight = ExcelChartLineWeight.Hairline;
    lineProperties1.LinePattern = ExcelChartLinePattern.None;
    lineProperties1.ColorIndex = ExcelKnownColors.Turquoise | ExcelKnownColors.BlackCustom;
    if (!this.IsDropBar || this.m_firstDropBar.HasLineProperties)
      return;
    IChartBorder lineProperties2 = this.m_firstDropBar.LineProperties;
    lineProperties2.LinePattern = ExcelChartLinePattern.Solid;
    lineProperties2.LineWeight = ExcelChartLineWeight.Hairline;
    IChartInterior interior1 = this.m_firstDropBar.Interior;
    interior1.Pattern = ExcelPattern.Solid;
    lineProperties2.ColorIndex = ExcelKnownColors.Turquoise | ExcelKnownColors.BlackCustom;
    lineProperties2.AutoFormat = true;
    interior1.ForegroundColorIndex = ExcelKnownColors.WhiteCustom;
    interior1.BackgroundColorIndex = ExcelKnownColors.Custom0;
    interior1.UseAutomaticFormat = true;
    IChartInterior interior2 = this.m_secondDropBar.Interior;
    interior2.ForegroundColorIndex = ExcelKnownColors.Custom0;
    interior2.BackgroundColorIndex = ExcelKnownColors.WhiteCustom;
    interior2.UseAutomaticFormat = true;
  }

  internal void CloneDeletedFormat(object parent, ref ChartFormatImpl format, bool cloneDataFormat)
  {
    if (!cloneDataFormat && format == (ChartFormatImpl) null)
    {
      format = (ChartFormatImpl) this.MemberwiseClone();
      format.SetParent(parent);
      format.SetParents();
      format.m_chartChartFormat = (ChartChartFormatRecord) CloneUtils.CloneCloneable((ICloneable) this.m_chartChartFormat);
      format.m_serieFormat = (BiffRecordRaw) CloneUtils.CloneCloneable((ICloneable) this.m_serieFormat);
      format.m_chart3D = (Chart3DRecord) CloneUtils.CloneCloneable((ICloneable) this.m_chart3D);
      format.m_formatLink = (ChartFormatLinkRecord) CloneUtils.CloneCloneable((ICloneable) this.m_formatLink);
      format.m_dataLabels = (ChartDataLabelsRecord) CloneUtils.CloneCloneable((ICloneable) this.m_dataLabels);
      format.m_chartChartLine = (ChartChartLineRecord) CloneUtils.CloneCloneable((ICloneable) this.m_chartChartLine);
      if (this.m_serieLine != null)
        format.m_serieLine = this.m_serieLine.Clone((object) format);
      if (this.m_highlowLine != null)
        format.m_highlowLine = this.m_highlowLine.Clone((object) format);
      if (this.m_dropLine != null)
        format.m_dropLine = this.m_dropLine.Clone((object) format);
      format.m_seriesList = (ChartSeriesListRecord) CloneUtils.CloneCloneable((ICloneable) this.m_seriesList);
      if (this.m_firstDropBar != null)
        format.m_firstDropBar = this.m_firstDropBar.Clone((object) format);
      if (this.m_secondDropBar != null)
        format.m_secondDropBar = this.m_secondDropBar.Clone((object) format);
      format.m_dataFormat = (ChartSerieDataFormatImpl) null;
    }
    else
    {
      if (this.m_dataFormat == null)
        return;
      format.m_dataFormat = this.m_dataFormat.Clone((object) format);
    }
  }

  internal ExcelChartType CheckAndApplyChartType()
  {
    ExcelChartType excelChartType = ExcelChartType.Column_Clustered;
    string str1 = "";
    switch (this.FormatRecordType)
    {
      case TBIFFRecord.ChartBar:
        string str2;
        if (this.DataFormatOrNull != null && this.DataFormatOrNull.Serie3DdDataFormatOrNull != null)
        {
          if (this.DataFormatOrNull.BarShapeBase == ExcelBaseFormat.Circle)
          {
            str2 = this.DataFormatOrNull.BarShapeTop == ExcelTopFormat.Straight ? "Cylinder" : "Cone";
            if (this.IsHorizontalBar)
              str2 += "_Bar";
          }
          else if (this.DataFormatOrNull.BarShapeTop == ExcelTopFormat.Straight)
          {
            str2 = this.IsHorizontalBar ? "Bar" : "Column";
          }
          else
          {
            str2 = "Pyramid";
            if (this.IsHorizontalBar)
              str2 += "_Bar";
          }
        }
        else
          str2 = str1 + (this.IsHorizontalBar ? "Bar" : "Column");
        bool flag = str2.IndexOf("Cone") != -1 || str2.IndexOf("Cylinder") != -1 || str2.IndexOf("Pyramid") != -1;
        if (str2 == "Column" && this.Is3D && !this.RightAngleAxes && !this.IsClustered && !this.StackValuesBar)
        {
          str1 = str2 + "_3D";
        }
        else
        {
          str1 = !this.StackValuesBar ? str2 + "_Clustered" : str2 + "_Stacked";
          if (this.ShowAsPercentsBar)
            str1 += "_100";
          if (!flag && this.Is3D)
            str1 += "_3D";
        }
        if (flag && !this.IsClustered && !this.StackValuesBar)
        {
          str1 += "_3D";
          break;
        }
        break;
      case TBIFFRecord.ChartLine:
        if (this.Is3D)
        {
          str1 = "Line_3D";
          break;
        }
        str1 += "Line";
        if (this.IsMarker)
          str1 += "_Markers";
        if (this.StackValuesLine)
          str1 += "_Stacked";
        if (this.ShowAsPercentsLine)
        {
          str1 += "_100";
          break;
        }
        break;
      case TBIFFRecord.ChartPie:
        str1 = this.DoughnutHoleSize != 0 ? "Doughnut" : "Pie";
        if (this.DataFormatOrNull != null && this.DataFormatOrNull.PieFormat != null && this.DataFormatOrNull.PieFormat.Percent > (ushort) 0)
          str1 += "_Exploded";
        if (this.Is3D)
        {
          str1 += "_3D";
          break;
        }
        break;
      case TBIFFRecord.ChartArea:
        if (this.Is3D && !this.IsStacked)
        {
          str1 = "Area_3D";
          break;
        }
        str1 += "Area";
        if (this.IsStacked)
          str1 += "_Stacked";
        if (this.IsCategoryBrokenDown)
          str1 += "_100";
        if (this.Is3D)
        {
          str1 += "_3D";
          break;
        }
        break;
      case TBIFFRecord.ChartScatter:
        if (this.IsBubbles)
        {
          str1 = this.DataFormatOrNull == null || !this.DataFormatOrNull.Is3DBubbles ? "Bubble" : "Bubble_3D";
          break;
        }
        string str3 = "Scatter";
        str1 = !this.IsSmoothed ? str3 + "_Line" : str3 + "_SmoothedLine";
        if (this.IsMarker)
        {
          str1 += "_Markers";
          break;
        }
        break;
      case TBIFFRecord.ChartRadar:
        str1 = "Radar";
        if (this.IsMarker)
        {
          str1 += "_Markers";
          break;
        }
        break;
      case TBIFFRecord.ChartSurface:
        string str4 = "Surface";
        if (!this.IsFillSurface)
          str4 += "_NoColor";
        str1 = this.Rotation != 0 || this.Elevation != 90 || this.Perspective != 0 ? str4 + "_3D" : str4 + "_Contour";
        break;
      case TBIFFRecord.ChartRadarArea:
        str1 = "Radar_Filled";
        break;
      case TBIFFRecord.ChartBoppop:
        switch (this.PieChartType)
        {
          case ExcelPieType.Normal:
            str1 = "Pie";
            break;
          case ExcelPieType.Pie:
            str1 = "PieOfPie";
            break;
          case ExcelPieType.Bar:
            str1 = "Pie_Bar";
            break;
        }
        break;
    }
    if (str1 != "")
      excelChartType = (ExcelChartType) Enum.Parse(typeof (ExcelChartType), str1, true);
    return excelChartType;
  }
}
