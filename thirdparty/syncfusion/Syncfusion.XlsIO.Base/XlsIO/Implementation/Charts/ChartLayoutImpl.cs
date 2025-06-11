// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Charts.ChartLayoutImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Charts;

public class ChartLayoutImpl : CommonObject, IChartLayout, IParentApplication
{
  protected ChartImpl m_book;
  protected object m_Parent;
  protected IChart m_chart;
  protected IShape m_chartShape;
  protected IChartManualLayout m_manualLayout;

  public ChartLayoutImpl(IApplication application, object parent, object chartObject)
    : this(application, parent, false, false, true)
  {
    if (!(chartObject is IChart))
      return;
    this.m_chart = (IChart) chartObject;
    if (this.m_chart.Parent == null || !(this.m_chart.Parent is IShape))
      return;
    this.m_chartShape = (IShape) this.m_chart.Parent;
  }

  public ChartLayoutImpl(IApplication application, object parent, bool bSetDefaults)
    : this(application, parent, false, false, bSetDefaults)
  {
  }

  public ChartLayoutImpl(
    IApplication application,
    object parent,
    bool bAutoSize,
    bool bIsInteriorGrey,
    bool bSetDefaults)
    : base(application, parent)
  {
    this.SetParents(parent);
    if (this.Workbook.Loading || !bSetDefaults)
      return;
    this.SetDefaultValues();
  }

  public ChartLayoutImpl(
    IApplication application,
    object parent,
    IList<BiffRecordRaw> data,
    ref int iPos)
    : base(application, parent)
  {
    this.SetParents(parent);
    this.Parse(data, ref iPos);
  }

  private void SetParents(object parent)
  {
    this.m_book = this.FindParent(typeof (ChartImpl)) as ChartImpl;
    this.m_Parent = parent;
    if (this.m_book == null)
      throw new ArgumentNullException("Can't find parent chart");
  }

  public WorkbookImpl Workbook => this.m_book.InnerWorkbook;

  public new object Parent => this.m_Parent;

  public IChartManualLayout ManualLayout
  {
    get
    {
      if (this.m_manualLayout == null)
        this.m_manualLayout = (IChartManualLayout) new ChartManualLayoutImpl(this.Workbook.Application, (object) this);
      return this.m_manualLayout;
    }
    set => this.m_manualLayout = value;
  }

  public LayoutTargets LayoutTarget
  {
    get => this.ManualLayout.LayoutTarget;
    set => this.ManualLayout.LayoutTarget = value;
  }

  public LayoutModes LeftMode
  {
    get => this.ManualLayout.LeftMode;
    set => this.ManualLayout.LeftMode = value;
  }

  public LayoutModes TopMode
  {
    get => this.ManualLayout.TopMode;
    set => this.ManualLayout.TopMode = value;
  }

  public double Left
  {
    get
    {
      return this.m_chart == null ? this.ManualLayout.Left : this.ManualLayout.Left * (this.m_chartShape != null ? (double) this.m_chartShape.Width : this.m_chart.Width);
    }
    set
    {
      if (this.m_chart != null)
        this.ManualLayout.Left = value / (this.m_chartShape != null ? (double) this.m_chartShape.Width : this.m_chart.Width);
      else
        this.ManualLayout.Left = value;
    }
  }

  public double Top
  {
    get
    {
      return this.m_chart == null ? this.ManualLayout.Top : this.ManualLayout.Top * (this.m_chartShape != null ? (double) this.m_chartShape.Height : this.m_chart.Height);
    }
    set
    {
      if (this.m_chart != null)
        this.ManualLayout.Top = value / (this.m_chartShape != null ? (double) this.m_chartShape.Height : this.m_chart.Height);
      else
        this.ManualLayout.Top = value;
    }
  }

  public LayoutModes WidthMode
  {
    get => this.ManualLayout.WidthMode;
    set => this.ManualLayout.WidthMode = value;
  }

  public LayoutModes HeightMode
  {
    get => this.ManualLayout.HeightMode;
    set => this.ManualLayout.HeightMode = value;
  }

  public double Width
  {
    get
    {
      return this.m_chart == null ? this.ManualLayout.Width : this.ManualLayout.Width * (this.m_chartShape != null ? (double) this.m_chartShape.Width : this.m_chart.Width);
    }
    set
    {
      if (this.m_chart != null)
        this.ManualLayout.Width = value / (this.m_chartShape != null ? (double) this.m_chartShape.Width : this.m_chart.Width);
      else
        this.ManualLayout.Width = value;
    }
  }

  public double Height
  {
    get
    {
      return this.m_chart == null ? this.ManualLayout.Height : this.ManualLayout.Height * (this.m_chartShape != null ? (double) this.m_chartShape.Height : this.m_chart.Height);
    }
    set
    {
      if (this.m_chart != null)
        this.ManualLayout.Height = value / (this.m_chartShape != null ? (double) this.m_chartShape.Height : this.m_chart.Height);
      else
        this.ManualLayout.Height = value;
    }
  }

  [CLSCompliant(false)]
  public void Parse(IList<BiffRecordRaw> data, ref int iPos)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    if (iPos < 0 || iPos > data.Count)
      throw new ArgumentOutOfRangeException(nameof (iPos), "Value cannot be less than 0 and greater than data.Count");
    this.UnwrapRecord(data[iPos]).CheckTypeCode(TBIFFRecord.ChartFrame);
  }

  [CLSCompliant(false)]
  protected virtual bool CheckBegin(BiffRecordRaw record)
  {
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    return record.TypeCode == TBIFFRecord.Begin;
  }

  [CLSCompliant(false)]
  protected virtual void ParseRecord(BiffRecordRaw record, ref int iBeginCounter)
  {
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    switch (record.TypeCode)
    {
      case TBIFFRecord.Begin:
        ++iBeginCounter;
        break;
      case TBIFFRecord.End:
        --iBeginCounter;
        break;
    }
  }

  [CLSCompliant(false)]
  public void Serialize(IList<IBiffStorage> records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
  }

  [CLSCompliant(false)]
  protected virtual void SerializeRecord(IList<IBiffStorage> list, BiffRecordRaw record)
  {
    if (list == null)
      throw new ArgumentNullException(nameof (list));
    if (record == null)
      throw new ArgumentNullException(nameof (record));
    list.Add((IBiffStorage) record.Clone());
  }

  [CLSCompliant(false)]
  protected virtual BiffRecordRaw UnwrapRecord(BiffRecordRaw record) => record;

  public void SetDefaultValues()
  {
  }

  internal bool IsManualLayout => this.m_manualLayout != null;
}
