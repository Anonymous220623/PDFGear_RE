// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Charts.ChartLegendImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.Charts;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Charts;

[CLSCompliant(false)]
public class ChartLegendImpl : CommonObject, IChartLegend
{
  private const int DEF_POSITION = 5;
  private const double DEF_SPRC = 4000.0;
  private ChartLegendRecord m_serieLegend;
  private ChartPosRecord m_pos;
  private ChartAttachedLabelLayoutRecord m_attachedLabelLayout;
  private ChartTextAreaImpl m_text;
  private ChartFrameFormatImpl m_frame;
  private bool m_includeInLayout = true;
  private ChartImpl m_parentChart;
  private ChartLegendEntriesColl m_collEntries;
  private IChartLayout m_layout;
  private ChartParagraphType m_paraType;
  private UnknownRecord m_legendTextPropsStream;
  private bool m_IsDefaultTextSettings;
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
            this.m_attachedLabelLayout = (ChartAttachedLabelLayoutRecord) biffRecordRaw;
          ChartManualLayoutImpl manualLayout = this.Layout.ManualLayout as ChartManualLayoutImpl;
          manualLayout.AttachedLabelLayout = this.m_attachedLabelLayout;
          if (this.m_attachedLabelLayout != null && this.m_attachedLabelLayout.X > 0.0 && this.m_attachedLabelLayout.Y > 0.0 && this.m_attachedLabelLayout.WXMode != LayoutModes.auto && this.m_attachedLabelLayout.WYMode != LayoutModes.auto)
          {
            manualLayout.FlagOptions = (byte) 3;
            if (this.m_attachedLabelLayout.Dx > 0.0 && this.m_attachedLabelLayout.Dy > 0.0)
            {
              manualLayout.FlagOptions = (byte) 15;
              break;
            }
            break;
          }
          break;
        case TBIFFRecord.ChartTextPropsStream:
          this.m_legendTextPropsStream = (UnknownRecord) data[iPos];
          break;
        case TBIFFRecord.ChartText:
          this.m_text = new ChartTextAreaImpl(this.Application, (object) this);
          iPos = this.m_text.Parse(data, iPos) - 1;
          this.IsChartTextArea = true;
          this.IsDefaultTextSettings = true;
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
    if (this.m_serieLegend.Position != ExcelLegendPosition.NotDocked || this.m_serieLegend.AutoPosition)
      return;
    if (this.m_attachedLabelLayout == null)
    {
      this.X = this.m_serieLegend.X;
      this.Y = this.m_serieLegend.Y;
      this.Width = this.m_serieLegend.Width;
      this.Height = this.m_serieLegend.Height;
    }
    else
    {
      if (this.m_attachedLabelLayout.X != 0.0 || this.m_attachedLabelLayout.Y != 0.0 || this.m_attachedLabelLayout.Dx != 0.0 || this.m_attachedLabelLayout.Dy != 0.0)
        return;
      this.m_serieLegend.Position = (ExcelLegendPosition) (((int) this.m_attachedLabelLayout.AutoLayoutType & 14) >> 1);
    }
  }

  internal void LegendStyleProperty(int index, object value)
  {
    if (this.m_collEntries == null || this.m_collEntries.HashEntries.Count == 0)
      return;
    bool flag = false;
    ExcelKnownColors excelKnownColors = ExcelKnownColors.None;
    Color color = Color.FromArgb(0, 0, 0, 0);
    ExcelUnderline excelUnderline = ExcelUnderline.None;
    string str = (string) null;
    ExcelFontVertialAlignment vertialAlignment = ExcelFontVertialAlignment.Baseline;
    double num = 0.0;
    switch (index)
    {
      case 0:
      case 3:
      case 4:
      case 5:
      case 7:
      case 8:
      case 9:
        flag = (bool) value;
        break;
      case 1:
        excelKnownColors = (ExcelKnownColors) value;
        break;
      case 2:
        color = (Color) value;
        break;
      case 6:
        num = (double) value;
        break;
      case 10:
        excelUnderline = (ExcelUnderline) value;
        break;
      case 11:
        str = (string) value;
        break;
      case 12:
        vertialAlignment = (ExcelFontVertialAlignment) value;
        break;
    }
    for (int iIndex = 0; iIndex < this.m_collEntries.Count; ++iIndex)
    {
      IChartLegendEntry collEntry = this.m_collEntries[iIndex];
      if (collEntry == null)
        break;
      switch (index)
      {
        case 0:
          if (collEntry.TextArea != null)
          {
            collEntry.TextArea.Bold = flag;
            break;
          }
          break;
        case 1:
          if (collEntry.TextArea != null)
          {
            collEntry.TextArea.Color = excelKnownColors;
            break;
          }
          break;
        case 2:
          if (collEntry.TextArea != null)
          {
            collEntry.TextArea.RGBColor = color;
            break;
          }
          break;
        case 3:
          if (collEntry.TextArea != null)
          {
            collEntry.TextArea.Italic = flag;
            break;
          }
          break;
        case 4:
          if (collEntry.TextArea != null)
          {
            collEntry.TextArea.MacOSOutlineFont = flag;
            break;
          }
          break;
        case 5:
          if (collEntry.TextArea != null)
          {
            collEntry.TextArea.MacOSShadow = flag;
            break;
          }
          break;
        case 6:
          if (collEntry.TextArea != null)
          {
            collEntry.TextArea.Size = num;
            break;
          }
          break;
        case 7:
          if (collEntry.TextArea != null)
          {
            collEntry.TextArea.Strikethrough = flag;
            break;
          }
          break;
        case 8:
          if (collEntry.TextArea != null)
          {
            collEntry.TextArea.Subscript = flag;
            break;
          }
          break;
        case 9:
          if (collEntry.TextArea != null)
          {
            collEntry.TextArea.Superscript = flag;
            break;
          }
          break;
        case 10:
          if (collEntry.TextArea != null)
          {
            collEntry.TextArea.Underline = excelUnderline;
            break;
          }
          break;
        case 11:
          if (collEntry.TextArea != null)
          {
            collEntry.TextArea.FontName = str;
            break;
          }
          break;
        case 12:
          if (collEntry.TextArea != null)
          {
            collEntry.TextArea.VerticalAlignment = vertialAlignment;
            break;
          }
          break;
      }
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
    this.m_attachedLabelLayout = (this.Layout.ManualLayout as ChartManualLayoutImpl).GetAttachedLabelLayoutRecord();
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

  public IChartFrameFormat FrameFormat
  {
    get
    {
      if (this.m_frame == null)
      {
        this.m_frame = new ChartFrameFormatImpl(this.Application, (object) this, true, false, true);
        this.m_frame.Interior.UseAutomaticFormat = true;
        if (this.m_parentChart.Workbook.Version != ExcelVersion.Excel97to2003)
          this.m_frame.HasLineProperties = false;
      }
      return (IChartFrameFormat) this.m_frame;
    }
  }

  public IChartTextArea TextArea
  {
    get
    {
      if (this.m_text == null)
        this.m_text = new ChartTextAreaImpl(this.Application, (object) this);
      if (!this.IsChartTextArea)
        this.m_text.ParagraphType = ChartParagraphType.CustomDefault;
      this.IsDefaultTextSettings = false;
      return (IChartTextArea) this.m_text;
    }
  }

  public bool IncludeInLayout
  {
    get => this.m_includeInLayout;
    set
    {
      if (value == this.m_includeInLayout)
        return;
      this.m_includeInLayout = value;
    }
  }

  public int X
  {
    get => (int) (this.Layout.ManualLayout.Left * 4000.0);
    set
    {
      this.SetCustomPosition();
      this.LegendRecord.X = value;
      this.PositionRecord.X1 = value;
      this.Layout.ManualLayout.Left = (double) this.LegendRecord.X / 4000.0;
      this.Layout.LeftMode = LayoutModes.edge;
    }
  }

  public int Y
  {
    get => (int) (this.Layout.ManualLayout.Top * 4000.0);
    set
    {
      this.SetCustomPosition();
      this.LegendRecord.Y = value;
      this.PositionRecord.Y1 = value;
      this.Layout.ManualLayout.Top = (double) this.LegendRecord.Y / 4000.0;
      this.Layout.TopMode = LayoutModes.edge;
    }
  }

  public ExcelLegendPosition Position
  {
    get => this.LegendRecord.Position;
    set
    {
      if (value == ExcelLegendPosition.NotDocked)
      {
        this.SetCustomPosition();
      }
      else
      {
        if (!(this.m_parentChart.Workbook as WorkbookImpl).Loading && !this.m_parentChart.IsParsing)
          this.m_layout = (IChartLayout) null;
        this.SetDefPosition();
        this.IsVerticalLegend = value != ExcelLegendPosition.Bottom && value != ExcelLegendPosition.Top;
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
    get => (int) (this.Layout.ManualLayout.Width * 4000.0);
    set
    {
      this.LegendRecord.Width = value;
      this.Layout.ManualLayout.Width = (double) this.LegendRecord.Width / 4000.0;
    }
  }

  public int Height
  {
    get => (int) (this.Layout.ManualLayout.Height * 4000.0);
    set
    {
      this.LegendRecord.Height = value;
      this.Layout.ManualLayout.Height = (double) this.LegendRecord.Height / 4000.0;
    }
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

  public IChartLayout Layout
  {
    get
    {
      if (this.m_layout == null)
        this.m_layout = (IChartLayout) new ChartLayoutImpl(this.Application, (object) this, (object) this.m_parentChart);
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
    return parent1;
  }

  public void Clear()
  {
    this.m_frame.Clear();
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
    this.LegendRecord.Position = ExcelLegendPosition.NotDocked;
    this.m_parentChart.ChartProperties.IsAlwaysAutoPlotArea = true;
  }
}
