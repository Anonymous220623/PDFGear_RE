// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Charts.ChartDataPointImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Parser.Biff_Records;
using Syncfusion.OfficeChart.Parser.Biff_Records.Charts;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Charts;

internal class ChartDataPointImpl : CommonObject, IOfficeChartDataPoint, IParentApplication
{
  private ChartDataLabelsImpl m_dataLabels;
  private int m_iIndex;
  private ChartSerieDataFormatImpl m_dataFormat;
  private ChartImpl m_parentChart;
  private bool m_bHasDataPoint;
  private bool m_defaultMarker;
  private bool m_bBubble3D;
  private int m_explosion;
  private bool m_bHasExplosion;
  private bool m_setAsTotal;

  public ChartDataPointImpl(IApplication application, object parent, int index)
    : base(application, parent)
  {
    this.m_iIndex = index;
    if ((parent as ChartDataPointsCollection).Parent is ChartSerieImpl)
    {
      this.m_dataFormat = new ChartSerieDataFormatImpl(application, (object) this);
      this.m_dataFormat.DataFormat.PointNumber = (ushort) this.m_iIndex;
    }
    else
      this.m_dataFormat = (ChartSerieDataFormatImpl) null;
    this.m_parentChart = (ChartImpl) this.FindParent(typeof (ChartImpl));
    if (this.m_parentChart == null)
      throw new Exception("cannot find parent chart.");
  }

  public IOfficeChartDataLabels DataLabels
  {
    get
    {
      this.CreateDataLabels();
      return (IOfficeChartDataLabels) this.m_dataLabels;
    }
    internal set => this.m_dataLabels = value as ChartDataLabelsImpl;
  }

  public IOfficeChartSerieDataFormat DataFormat
  {
    get
    {
      if (this.m_dataFormat == null && (this.Parent as ChartDataPointsCollection).Parent is ChartSerieImpl)
        this.m_dataFormat = new ChartSerieDataFormatImpl(this.Application, (object) this);
      return (IOfficeChartSerieDataFormat) this.m_dataFormat;
    }
  }

  public ChartSerieDataFormatImpl InnerDataFormat
  {
    get => this.m_dataFormat;
    set
    {
      this.m_dataFormat = value;
      if (value == null)
        return;
      value.SetParent((object) this);
      value.SetParents();
    }
  }

  public int Index
  {
    get => this.m_iIndex;
    set => this.m_iIndex = value;
  }

  public ChartSerieDataFormatImpl DataFormatOrNull => this.m_dataFormat;

  public bool IsDefault => this.m_iIndex == (int) ushort.MaxValue;

  public bool HasDataLabels => this.m_dataLabels != null;

  internal bool HasDataPoint
  {
    get => this.m_bHasDataPoint;
    set => this.m_bHasDataPoint = value;
  }

  public bool IsDefaultmarkertype
  {
    get => this.m_defaultMarker;
    set => this.m_defaultMarker = value;
  }

  internal bool Bubble3D
  {
    get => this.m_bBubble3D;
    set => this.m_bBubble3D = value;
  }

  internal int Explosion
  {
    get => this.m_explosion;
    set
    {
      this.m_explosion = value;
      this.m_bHasExplosion = true;
    }
  }

  internal bool HasExplosion => this.m_bHasExplosion;

  public bool SetAsTotal
  {
    get => this.m_setAsTotal;
    set => this.m_setAsTotal = value;
  }

  [CLSCompliant(false)]
  public void SerializeDataLabels(OffsetArrayList records)
  {
    if (this.m_dataLabels == null)
      return;
    this.m_dataLabels.Serialize((IList<IBiffStorage>) records);
  }

  [CLSCompliant(false)]
  public void SerializeDataFormat(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (this.m_dataFormat == null)
      return;
    this.m_dataFormat.UpdateDataFormatInDataPoint();
    this.m_dataFormat.Serialize(records);
  }

  public void SetDataLabels(ChartTextAreaImpl textArea)
  {
    if (textArea == null)
      throw new ArgumentNullException(nameof (textArea));
    this.CreateDataLabels();
    this.m_dataLabels.TextArea = textArea;
  }

  private void CreateDataLabels()
  {
    if (this.m_dataLabels != null)
      return;
    this.m_dataLabels = new ChartDataLabelsImpl(this.Application, (object) this, this.Index);
  }

  public object Clone(
    object parent,
    Dictionary<int, int> dicFontIndexes,
    Dictionary<string, string> dicNewSheetNames)
  {
    ChartDataPointImpl parent1 = new ChartDataPointImpl(this.Application, parent, this.m_iIndex);
    if (this.m_dataLabels != null)
      parent1.m_dataLabels = (ChartDataLabelsImpl) this.m_dataLabels.Clone((object) parent1, dicFontIndexes, dicNewSheetNames);
    if (this.m_dataFormat != null)
      parent1.m_dataFormat = this.m_dataFormat.Clone((object) parent1);
    parent1.m_setAsTotal = this.m_setAsTotal;
    return (object) parent1;
  }

  public void UpdateSerieIndex()
  {
    if (this.m_dataLabels != null)
      this.m_dataLabels.UpdateSerieIndex();
    if (this.m_dataFormat == null)
      return;
    this.m_dataFormat.UpdateSerieIndex();
  }

  public void ChangeChartStockHigh_Low_CloseType()
  {
    this.DataFormat.MarkerStyle = OfficeChartMarkerType.DowJones;
    this.m_dataFormat.IsAutoMarker = false;
    this.m_dataFormat.MarkerForegroundColorIndex = OfficeKnownColors.Turquoise | OfficeKnownColors.BlackCustom;
    this.m_dataFormat.MarkerBackgroundColorIndex = OfficeKnownColors.Turquoise | OfficeKnownColors.BlackCustom;
    this.m_dataFormat.LineProperties.LinePattern = OfficeChartLinePattern.None;
    this.m_dataFormat.LineProperties.LineWeight = OfficeChartLineWeight.Hairline;
    this.m_dataFormat.LineProperties.ColorIndex = OfficeKnownColors.YellowCustom | OfficeKnownColors.BlackCustom;
  }

  public void ChangeChartStockVolume_High_Low_CloseType()
  {
    this.DataFormat.MarkerStyle = OfficeChartMarkerType.DowJones;
    this.m_dataFormat.IsAutoMarker = false;
    this.m_dataFormat.MarkerForegroundColorIndex = OfficeKnownColors.YellowCustom | OfficeKnownColors.BlackCustom;
    this.m_dataFormat.MarkerBackgroundColorIndex = OfficeKnownColors.YellowCustom | OfficeKnownColors.BlackCustom;
    OfficeChartType destinationType = this.m_parentChart.DestinationType;
    this.m_parentChart.DestinationType = OfficeChartType.Line;
    this.m_dataFormat.LineProperties.LinePattern = OfficeChartLinePattern.None;
    this.m_parentChart.DestinationType = destinationType;
  }

  public void ChangeIntimateBuble(OfficeChartType typeToChange)
  {
    if ((this.m_parentChart.Workbook as WorkbookImpl).IsWorkbookOpening)
      return;
    this.DataFormat.LineProperties.LinePattern = OfficeChartLinePattern.Solid;
    this.DataFormat.Is3DBubbles = typeToChange != OfficeChartType.Bubble;
  }

  public void CloneDataFormat(ChartSerieDataFormatImpl serieFormat)
  {
    if (serieFormat == null || this.m_dataFormat != null && this.m_dataFormat.IsFormatted)
      return;
    ChartDataFormatRecord dataFormat = this.m_dataFormat.DataFormat;
    this.m_dataFormat = serieFormat.Clone((object) this);
    this.m_dataFormat.DataFormat = dataFormat;
  }

  public void ClearDataFormats(ChartSerieDataFormatImpl format)
  {
    if (this.m_dataFormat == null || !this.m_dataFormat.IsFormatted)
      return;
    ChartDataFormatRecord dataFormat = this.m_dataFormat.DataFormat;
    this.m_dataFormat = format.Clone((object) this);
    this.m_dataFormat.DataFormat = dataFormat;
  }
}
