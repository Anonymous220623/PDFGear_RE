// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Charts.ChartInteriorImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.Charts;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Charts;

public class ChartInteriorImpl : CommonObject, IChartInterior, ICloneParent
{
  private ChartAreaFormatRecord m_area;
  private WorkbookImpl m_book;
  private ChartSerieDataFormatImpl m_serieFormat;
  private ColorObject m_foreColor;
  private ColorObject m_backColor;
  internal static Dictionary<ExcelPattern, ExcelGradientPattern> m_hashPat = new Dictionary<ExcelPattern, ExcelGradientPattern>(18);

  static ChartInteriorImpl()
  {
    ChartInteriorImpl.m_hashPat.Add(ExcelPattern.Percent50, ExcelGradientPattern.Pat_50_Percent);
    ChartInteriorImpl.m_hashPat.Add(ExcelPattern.Percent70, ExcelGradientPattern.Pat_70_Percent);
    ChartInteriorImpl.m_hashPat.Add(ExcelPattern.Percent25, ExcelGradientPattern.Pat_25_Percent);
    ChartInteriorImpl.m_hashPat.Add(ExcelPattern.Percent60, ExcelGradientPattern.Pat_30_Percent);
    ChartInteriorImpl.m_hashPat.Add(ExcelPattern.Percent10, ExcelGradientPattern.Pat_20_Percent);
    ChartInteriorImpl.m_hashPat.Add(ExcelPattern.Percent05, ExcelGradientPattern.Pat_10_Percent);
    for (int key = 5; key < 16 /*0x10*/; ++key)
      ChartInteriorImpl.m_hashPat.Add((ExcelPattern) key, (ExcelGradientPattern) (key + 8));
    ChartInteriorImpl.m_hashPat.Add(ExcelPattern.Gradient, ExcelGradientPattern.Pat_5_Percent);
  }

  public ChartInteriorImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.m_area = (ChartAreaFormatRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartAreaFormat);
    this.SetParents();
  }

  [CLSCompliant(false)]
  public ChartInteriorImpl(IApplication application, object parent, ChartAreaFormatRecord area)
    : base(application, parent)
  {
    this.m_area = area != null ? area : throw new ArgumentNullException(nameof (area));
    this.SetParents();
  }

  public ChartInteriorImpl(
    IApplication application,
    object parent,
    IList<BiffRecordRaw> data,
    ref int iPos)
    : base(application, parent)
  {
    this.Parse(data, ref iPos);
    this.SetParents();
  }

  private void SetParents()
  {
    this.m_book = (WorkbookImpl) this.FindParent(typeof (WorkbookImpl));
    this.m_serieFormat = this.FindParent(typeof (ChartSerieDataFormatImpl)) as ChartSerieDataFormatImpl;
    if (this.m_book == null)
      throw new ApplicationException("cannot find parent object");
    this.m_foreColor = new ColorObject(this.m_area.ForegroundColorIndex);
    this.m_foreColor.AfterChange += new ColorObject.AfterChangeHandler(this.UpdateForeColor);
    this.m_backColor = new ColorObject(this.m_area.BackgroundColorIndex);
    this.m_backColor.AfterChange += new ColorObject.AfterChangeHandler(this.UpdateBackColor);
  }

  public void Parse(IList<BiffRecordRaw> data, ref int iPos)
  {
    BiffRecordRaw biffRecordRaw = data != null ? data[iPos] : throw new ArgumentNullException(nameof (data));
    biffRecordRaw.CheckTypeCode(TBIFFRecord.ChartAreaFormat);
    this.m_area = (ChartAreaFormatRecord) biffRecordRaw;
    ++iPos;
  }

  [CLSCompliant(false)]
  public void Serialize(IList<IBiffStorage> records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (this.m_area == null)
      return;
    records.Add((IBiffStorage) this.m_area.Clone());
  }

  public ColorObject ForegroundColorObject => this.m_foreColor;

  public ColorObject BackgroundColorObject => this.m_backColor;

  public Color ForegroundColor
  {
    get => this.m_foreColor.GetRGB((IWorkbook) this.m_book);
    set => this.m_foreColor.SetRGB(value, (IWorkbook) this.m_book);
  }

  public Color BackgroundColor
  {
    get => this.m_backColor.GetRGB((IWorkbook) this.m_book);
    set => this.m_backColor.SetRGB(value, (IWorkbook) this.m_book);
  }

  public ExcelPattern Pattern
  {
    get => !this.UseAutomaticFormat ? this.m_area.Pattern : ExcelPattern.Solid;
    set
    {
      if (this.Pattern == value)
        return;
      int num = (int) value;
      IFill fill = (this.Parent as IFillColor).Fill;
      if (num < 2)
      {
        if (this.Pattern > ExcelPattern.Solid)
          fill.Solid();
      }
      else
        fill.Patterned(ChartInteriorImpl.m_hashPat[value]);
      this.UseAutomaticFormat = false;
      this.m_area.Pattern = value;
    }
  }

  public ExcelKnownColors ForegroundColorIndex
  {
    get => this.m_foreColor.GetIndexed((IWorkbook) this.m_book);
    set => this.m_foreColor.SetIndexed(value);
  }

  public ExcelKnownColors BackgroundColorIndex
  {
    get => this.m_backColor.GetIndexed((IWorkbook) this.m_book);
    set => this.m_backColor.SetIndexed(value);
  }

  public bool UseAutomaticFormat
  {
    get => this.m_area.UseAutomaticFormat;
    set
    {
      if (value == this.UseAutomaticFormat)
        return;
      this.m_area.UseAutomaticFormat = value;
      if (value || this.m_area.Pattern != ExcelPattern.None)
        return;
      this.m_area.Pattern = ExcelPattern.Solid;
    }
  }

  public bool SwapColorsOnNegative
  {
    get => this.m_area.SwapColorsOnNegative;
    set => this.m_area.SwapColorsOnNegative = value;
  }

  internal void UpdateForeColor()
  {
    this.m_area.ForegroundColorIndex = this.ForegroundColorIndex;
    this.m_area.ForegroundColor = this.ForegroundColor.ToArgb() & 16777215 /*0xFFFFFF*/;
    this.UseAutomaticFormat = false;
    (this.Parent as IFillColor).Visible = true;
  }

  internal void UpdateBackColor()
  {
    this.m_area.BackgroundColorIndex = this.BackgroundColorIndex;
    this.m_area.BackgroundColor = this.BackgroundColor;
    this.UseAutomaticFormat = false;
    (this.Parent as IFillColor).Visible = true;
  }

  public void InitForFrameFormat(bool bIsAutoSize, bool bIs3DChart, bool bIsInteriorGray)
  {
    this.InitForFrameFormat(bIsAutoSize, bIs3DChart, bIsInteriorGray, false);
  }

  public void InitForFrameFormat(
    bool bIsAutoSize,
    bool bIs3DChart,
    bool bIsInteriorGray,
    bool bIsGray50)
  {
    this.m_area.Pattern = ExcelPattern.Solid;
    this.m_area.UseAutomaticFormat = bIs3DChart;
    this.m_area.SwapColorsOnNegative = false;
    this.m_area.ForegroundColorIndex = bIsInteriorGray ? ExcelKnownColors.Grey_25_percent : ExcelKnownColors.White;
    this.m_area.BackgroundColorIndex = bIsAutoSize ? ExcelKnownColors.Turquoise | ExcelKnownColors.BlackCustom : ExcelKnownColors.YellowCustom | ExcelKnownColors.BlackCustom;
    if (!bIsGray50)
      return;
    this.m_area.ForegroundColorIndex = ExcelKnownColors.Grey_50_percent;
  }

  public ChartInteriorImpl Clone(object parent)
  {
    if (parent == null)
      throw new ArgumentNullException(nameof (parent));
    ChartInteriorImpl chartInteriorImpl = (ChartInteriorImpl) this.MemberwiseClone();
    chartInteriorImpl.m_area = (ChartAreaFormatRecord) CloneUtils.CloneCloneable((ICloneable) this.m_area);
    chartInteriorImpl.SetParent(parent);
    chartInteriorImpl.SetParents();
    return chartInteriorImpl;
  }

  object ICloneParent.Clone(object parent) => (object) this.Clone(parent);
}
