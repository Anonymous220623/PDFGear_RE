// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Charts.ChartLegendImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Parser;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using Syncfusion.OfficeChart.Parser.Biff_Records.Charts;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Charts;

[CLSCompliant(false)]
internal class ChartLegendImpl : CommonObject, IOfficeChartLegend
{
  private const int DEF_POSITION = 5;
  private ChartLegendRecord m_serieLegend;
  private ChartPosRecord m_pos;
  private ChartAttachedLabelLayoutRecord m_attachedLabelLayout;
  private ChartTextAreaImpl m_text;
  private ChartFrameFormatImpl m_frame;
  private bool m_includeInLayout = true;
  private ChartImpl m_parentChart;
  private ChartLegendEntriesColl m_collEntries;
  private IOfficeChartLayout m_layout;
  private ChartParagraphType m_paraType;
  private UnknownRecord m_legendTextPropsStream;
  private bool m_IsDefaultTextSettings = true;
  private bool m_IsChartTextArea;
  private ushort m_chartExPosition;

  public ChartLegendImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.m_serieLegend = (ChartLegendRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartLegend);
    this.m_pos = (ChartPosRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartPos);
    this.m_pos.TopLeft = (ushort) 5;
    this.m_text = new ChartTextAreaImpl(application, (object) this);
    this.m_collEntries = new ChartLegendEntriesColl(application, parent);
    this.SetParents();
    this.m_paraType = ChartParagraphType.Default;
  }

  private void SetParents()
  {
    this.m_parentChart = (ChartImpl) this.FindParent(typeof (ChartImpl));
    if (this.m_parentChart == null)
      throw new ApplicationException("Can't find parent object.");
  }

  public void Parse(IList<BiffRecordRaw> data, ref int iPos)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    data[iPos].CheckTypeCode(TBIFFRecord.ChartLegend);
    this.m_serieLegend = (ChartLegendRecord) data[iPos];
    iPos += 2;
    int num = 1;
    while (num != 0)
    {
      BiffRecordRaw biffRecordRaw = data[iPos];
      switch (biffRecordRaw.TypeCode)
      {
        case TBIFFRecord.ChartAttachedLabelLayout:
          if (this.m_attachedLabelLayout == null)
            this.m_attachedLabelLayout = (this.Layout.ManualLayout as ChartManualLayoutImpl).AttachedLabelLayout;
          this.m_attachedLabelLayout = (ChartAttachedLabelLayoutRecord) biffRecordRaw;
          break;
        case TBIFFRecord.ChartTextPropsStream:
          this.m_legendTextPropsStream = (UnknownRecord) data[iPos];
          break;
        case TBIFFRecord.ChartText:
          this.m_text = new ChartTextAreaImpl(this.Application, (object) this);
          iPos = this.m_text.Parse(data, iPos) - 1;
          this.IsChartTextArea = true;
          this.m_text.ParagraphType = ChartParagraphType.CustomDefault;
          break;
        case TBIFFRecord.ChartFrame:
          this.m_frame = new ChartFrameFormatImpl(this.Application, (object) this, false);
          this.m_frame.Parse(data, ref iPos);
          --iPos;
          break;
        case TBIFFRecord.Begin:
          iPos = BiffRecordRaw.SkipBeginEndBlock(data, iPos);
          break;
        case TBIFFRecord.End:
          --num;
          break;
        case TBIFFRecord.ChartPos:
          this.m_pos = (ChartPosRecord) biffRecordRaw;
          break;
      }
      ++iPos;
    }
  }

  public void Serialize(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    records.Add((IBiffStorage) this.m_serieLegend.Clone());
    records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.Begin));
    if (this.m_pos != null)
      records.Add((IBiffStorage) this.m_pos.Clone());
    if (this.m_text != null)
      this.m_text.Serialize((IList<IBiffStorage>) records, true);
    if (this.m_frame != null)
      this.m_frame.Serialize((IList<IBiffStorage>) records);
    if (this.m_attachedLabelLayout != null)
      this.SerializeRecord((IList<IBiffStorage>) records, (BiffRecordRaw) this.m_attachedLabelLayout);
    if (this.m_legendTextPropsStream != null)
      records.Add((IBiffStorage) this.m_legendTextPropsStream);
    records.Add((IBiffStorage) BiffRecordFactory.GetRecord(TBIFFRecord.End));
  }

  [CLSCompliant(false)]
  protected virtual void SerializeRecord(IList<IBiffStorage> records, BiffRecordRaw record)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (record == null)
      return;
    records.Add((IBiffStorage) record.Clone());
  }

  public IOfficeChartFrameFormat FrameFormat
  {
    get
    {
      if (this.m_frame == null)
      {
        this.m_frame = new ChartFrameFormatImpl(this.Application, (object) this, true, false, true);
        this.m_frame.Interior.UseAutomaticFormat = true;
        if (this.m_parentChart.Workbook.Version != OfficeVersion.Excel97to2003)
          this.m_frame.HasLineProperties = false;
      }
      return (IOfficeChartFrameFormat) this.m_frame;
    }
  }

  public IOfficeChartTextArea TextArea
  {
    get
    {
      if (this.m_text == null)
        this.m_text = new ChartTextAreaImpl(this.Application, (object) this);
      if (!this.IsChartTextArea)
        this.m_text.ParagraphType = ChartParagraphType.CustomDefault;
      return (IOfficeChartTextArea) this.m_text;
    }
  }

  public bool IncludeInLayout
  {
    get => this.m_includeInLayout;
    set
    {
      if (this.m_parentChart.Workbook.Version == OfficeVersion.Excel97to2003)
        throw new ArgumentException("This property is not supported for the current workbook version");
      if (value == this.m_includeInLayout)
        return;
      this.m_includeInLayout = value;
    }
  }

  public int X
  {
    get => this.LegendRecord.X;
    set
    {
      this.SetCustomPosition();
      this.LegendRecord.X = value;
      this.m_layout.Left = (double) this.LegendRecord.X;
      this.PositionRecord.X1 = value;
    }
  }

  public int Y
  {
    get => this.LegendRecord.Y;
    set
    {
      this.SetCustomPosition();
      this.LegendRecord.Y = value;
      this.m_layout.Top = (double) this.LegendRecord.Y;
      this.PositionRecord.Y1 = value;
    }
  }

  public OfficeLegendPosition Position
  {
    get => this.LegendRecord.Position;
    set
    {
      if (value == OfficeLegendPosition.NotDocked)
      {
        this.SetCustomPosition();
      }
      else
      {
        this.SetDefPosition();
        this.IsVerticalLegend = value != OfficeLegendPosition.Bottom && value != OfficeLegendPosition.Top;
        this.LegendRecord.Position = value;
      }
    }
  }

  public bool IsVerticalLegend
  {
    get => this.LegendRecord.IsVerticalLegend;
    set => this.LegendRecord.IsVerticalLegend = value;
  }

  public IChartLegendEntries LegendEntries => (IChartLegendEntries) this.m_collEntries;

  internal bool IsDefaultTextSettings
  {
    get => this.m_IsDefaultTextSettings;
    set => this.m_IsDefaultTextSettings = value;
  }

  internal bool IsChartTextArea
  {
    get => this.m_IsChartTextArea;
    set => this.m_IsChartTextArea = value;
  }

  public int Width
  {
    get => this.LegendRecord.Width;
    set => this.LegendRecord.Width = value;
  }

  public int Height
  {
    get => this.LegendRecord.Height;
    set => this.LegendRecord.Height = value;
  }

  public bool ContainsDataTable
  {
    get => this.LegendRecord.ContainsDataTable;
    set => this.LegendRecord.ContainsDataTable = value;
  }

  public ExcelLegendSpacing Spacing
  {
    get => this.LegendRecord.Spacing;
    set => this.LegendRecord.Spacing = value;
  }

  public bool AutoPosition
  {
    get => this.LegendRecord.AutoPosition;
    set => this.LegendRecord.AutoPosition = value;
  }

  public bool AutoSeries
  {
    get => this.LegendRecord.AutoSeries;
    set => this.LegendRecord.AutoSeries = value;
  }

  public bool AutoPositionX
  {
    get => this.LegendRecord.AutoPositionX;
    set => this.LegendRecord.AutoPositionX = value;
  }

  public bool AutoPositionY
  {
    get => this.LegendRecord.AutoPositionY;
    set => this.LegendRecord.AutoPositionY = value;
  }

  public IOfficeChartLayout Layout
  {
    get
    {
      if (this.m_layout == null)
        this.m_layout = (IOfficeChartLayout) new ChartLayoutImpl(this.Application, (object) this, (object) this.m_parentChart);
      return this.m_layout;
    }
    set => this.m_layout = value;
  }

  public ChartParagraphType ParagraphType
  {
    get => this.m_paraType;
    set => this.m_paraType = value;
  }

  public ChartAttachedLabelLayoutRecord AttachedLabelLayout
  {
    get
    {
      if (this.m_attachedLabelLayout == null)
        this.m_attachedLabelLayout = (ChartAttachedLabelLayoutRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartAttachedLabelLayout);
      return this.m_attachedLabelLayout;
    }
  }

  internal ushort ChartExPosition
  {
    get => this.m_chartExPosition;
    set => this.m_chartExPosition = value;
  }

  private ChartLegendRecord LegendRecord
  {
    get
    {
      if (this.m_serieLegend == null)
        this.m_serieLegend = (ChartLegendRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartLegend);
      return this.m_serieLegend;
    }
  }

  private ChartPosRecord PositionRecord
  {
    get
    {
      if (this.m_pos == null)
        this.m_pos = (ChartPosRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartPos);
      return this.m_pos;
    }
  }

  public ChartLegendImpl Clone(
    object parent,
    Dictionary<int, int> dicFontIndexes,
    Dictionary<string, string> dicNewSheetNames)
  {
    ChartLegendImpl parent1 = (ChartLegendImpl) this.MemberwiseClone();
    parent1.SetParent(parent);
    parent1.SetParents();
    parent1.m_serieLegend = (ChartLegendRecord) CloneUtils.CloneCloneable((ICloneable) this.m_serieLegend);
    parent1.m_pos = (ChartPosRecord) CloneUtils.CloneCloneable((ICloneable) this.m_pos);
    parent1.m_collEntries = this.m_collEntries.Clone((object) parent1, dicFontIndexes, dicNewSheetNames);
    if (this.m_frame != null)
      parent1.m_frame = this.m_frame.Clone((object) parent1);
    if (this.m_text != null)
      parent1.m_text = (ChartTextAreaImpl) this.m_text.Clone((object) parent1, dicFontIndexes, dicNewSheetNames);
    if (this.m_layout != null)
      parent1.m_layout = this.m_layout;
    parent1.m_paraType = this.m_paraType;
    if (this.m_legendTextPropsStream != null)
      parent1.m_legendTextPropsStream = (UnknownRecord) this.m_legendTextPropsStream.Clone();
    if (this.m_attachedLabelLayout != null)
      parent1.m_attachedLabelLayout = (ChartAttachedLabelLayoutRecord) this.m_attachedLabelLayout.Clone();
    return parent1;
  }

  public void Clear()
  {
    if (this.m_frame != null)
      this.m_frame.Clear();
    if (this.m_collEntries != null)
      this.m_collEntries.Clear();
    this.m_pos = (ChartPosRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartPos);
    this.m_serieLegend = (ChartLegendRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartLegend);
  }

  public void Delete() => this.m_parentChart.HasLegend = false;

  private void SetDefPosition()
  {
    this.AutoPosition = true;
    this.AutoPositionX = true;
    this.AutoPositionY = true;
    this.LegendRecord.X = 0;
    this.LegendRecord.Y = 0;
    this.LegendRecord.Width = 0;
    this.LegendRecord.Height = 0;
    this.PositionRecord.X1 = 0;
    this.PositionRecord.X2 = 0;
    this.PositionRecord.Y1 = 0;
    this.PositionRecord.Y2 = 0;
    this.m_parentChart.ChartProperties.IsAlwaysAutoPlotArea = false;
  }

  private void SetCustomPosition()
  {
    this.AutoPosition = false;
    this.AutoPositionX = false;
    this.AutoPositionY = false;
    this.LegendRecord.Position = OfficeLegendPosition.NotDocked;
    this.m_parentChart.ChartProperties.IsAlwaysAutoPlotArea = true;
  }
}
