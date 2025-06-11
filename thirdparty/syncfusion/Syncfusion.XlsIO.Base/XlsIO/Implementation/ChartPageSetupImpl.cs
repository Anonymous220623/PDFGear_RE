// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.ChartPageSetupImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.Charts;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class ChartPageSetupImpl : 
  PageSetupBaseImpl,
  IChartPageSetup,
  IPageSetupBase,
  IParentApplication
{
  private PrintedChartSizeRecord m_chartSize = (PrintedChartSizeRecord) BiffRecordFactory.GetRecord(TBIFFRecord.PrintedChartSize);

  public bool FitToPagesTall
  {
    get => this.m_setup.FitHeight != (ushort) 0;
    set
    {
      ushort num = value ? (ushort) 1 : (ushort) 0;
      if ((int) this.m_setup.FitHeight == (int) num)
        return;
      this.m_setup.FitHeight = num;
      this.SetChanged();
    }
  }

  public bool FitToPagesWide
  {
    get => this.m_setup.FitWidth != (ushort) 0;
    set
    {
      ushort num = value ? (ushort) 1 : (ushort) 0;
      if ((int) this.m_setup.FitWidth == (int) num)
        return;
      this.m_setup.FitWidth = num;
      this.SetChanged();
    }
  }

  public ChartPageSetupImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.FindParents();
  }

  [CLSCompliant(false)]
  public ChartPageSetupImpl(IApplication application, object parent, BiffReader reader)
    : base(application, parent)
  {
    this.FindParents();
    this.Parse(reader);
  }

  [CLSCompliant(false)]
  public ChartPageSetupImpl(
    IApplication application,
    object parent,
    IList<BiffRecordRaw> data,
    ref int position)
    : base(application, parent)
  {
    this.FindParents();
    position = this.Parse(data, position);
  }

  public ChartPageSetupImpl(
    IApplication application,
    object parent,
    List<BiffRecordRaw> data,
    ref int position)
    : base(application, parent)
  {
    this.FindParents();
    position = this.Parse((IList<BiffRecordRaw>) data, position);
  }

  [CLSCompliant(false)]
  protected override bool ParseRecord(BiffRecordRaw record)
  {
    bool record1 = base.ParseRecord(record);
    if (!record1)
    {
      record1 = true;
      if (record.TypeCode == TBIFFRecord.PrintedChartSize)
        this.m_chartSize = (PrintedChartSizeRecord) record;
      else
        record1 = false;
    }
    return record1;
  }

  [CLSCompliant(false)]
  public void Parse(BiffReader reader) => throw new NotImplementedException();

  [CLSCompliant(false)]
  protected override void SerializeEndRecords(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    if (this.m_chartSize == null)
      throw new ArgumentNullException("m_chartSize");
    base.SerializeEndRecords(records);
    records.Add((IBiffStorage) this.m_chartSize.Clone());
  }

  public ChartPageSetupImpl Clone(object parent)
  {
    ChartPageSetupImpl chartPageSetupImpl = (ChartPageSetupImpl) base.Clone(parent);
    this.m_chartSize = (PrintedChartSizeRecord) CloneUtils.CloneCloneable((ICloneable) this.m_chartSize);
    this.m_setup = (PrintSetupRecord) CloneUtils.CloneCloneable((ICloneable) this.m_setup);
    this.m_unknown = (PrinterSettingsRecord) CloneUtils.CloneCloneable((ICloneable) this.m_unknown);
    return chartPageSetupImpl;
  }
}
