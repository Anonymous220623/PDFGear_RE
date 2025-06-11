// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.CalculationOptionsImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class CalculationOptionsImpl(IApplication application, object parent) : 
  CommonObject(application, parent),
  ICalculationOptions,
  IParentApplication,
  ICloneParent
{
  public static TBIFFRecord[] DEF_CORRECT_CODES = new TBIFFRecord[6]
  {
    TBIFFRecord.CalcMode,
    TBIFFRecord.CalCount,
    TBIFFRecord.RefMode,
    TBIFFRecord.Iteration,
    TBIFFRecord.Delta,
    TBIFFRecord.SaveRecalc
  };
  private CalcModeRecord m_calcMode = (CalcModeRecord) BiffRecordFactory.GetRecord(TBIFFRecord.CalcMode);
  private CalcCountRecord m_calcCount = (CalcCountRecord) BiffRecordFactory.GetRecord(TBIFFRecord.CalCount);
  private RefModeRecord m_refMode = (RefModeRecord) BiffRecordFactory.GetRecord(TBIFFRecord.RefMode);
  private IterationRecord m_iteration = (IterationRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Iteration);
  private DeltaRecord m_delta = (DeltaRecord) BiffRecordFactory.GetRecord(TBIFFRecord.Delta);
  private SaveRecalcRecord m_saveRecalc = (SaveRecalcRecord) BiffRecordFactory.GetRecord(TBIFFRecord.SaveRecalc);

  [CLSCompliant(false)]
  public CalculationOptionsImpl(
    IApplication application,
    object parent,
    BiffRecordRaw[] data,
    int iPos)
    : this(application, parent)
  {
    this.Parse((IList) data, iPos);
  }

  public int Parse(IList data, int iPos)
  {
    int num = data != null ? data.Count : throw new ArgumentNullException(nameof (data));
    if (iPos < 0 || iPos >= num)
      throw new ArgumentOutOfRangeException(nameof (iPos), "Value cannot be less than 0 and greater than data.Length");
    for (; iPos < num; ++iPos)
    {
      BiffRecordRaw biffRecordRaw = (BiffRecordRaw) data[iPos];
      switch (biffRecordRaw.TypeCode)
      {
        case TBIFFRecord.CalCount:
          this.m_calcCount = (CalcCountRecord) biffRecordRaw;
          break;
        case TBIFFRecord.CalcMode:
          this.m_calcMode = (CalcModeRecord) biffRecordRaw;
          break;
        case TBIFFRecord.RefMode:
          this.m_refMode = (RefModeRecord) biffRecordRaw;
          break;
        case TBIFFRecord.Delta:
          this.m_delta = (DeltaRecord) biffRecordRaw;
          break;
        case TBIFFRecord.Iteration:
          this.m_iteration = (IterationRecord) biffRecordRaw;
          break;
        case TBIFFRecord.SaveRecalc:
          this.m_saveRecalc = (SaveRecalcRecord) biffRecordRaw;
          break;
        default:
          return iPos;
      }
    }
    return iPos;
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    records.Add(this.m_calcMode.Clone());
    records.Add(this.m_calcCount.Clone());
    records.Add(this.m_refMode.Clone());
    records.Add(this.m_iteration.Clone());
    records.Add(this.m_delta.Clone());
    records.Add(this.m_saveRecalc.Clone());
  }

  public int MaximumIteration
  {
    get => (int) this.m_calcCount.Iterations;
    set => this.m_calcCount.Iterations = (ushort) value;
  }

  public ExcelCalculationMode CalculationMode
  {
    get => this.m_calcMode.CalculationMode;
    set => this.m_calcMode.CalculationMode = value;
  }

  public bool RecalcOnSave
  {
    get => this.m_saveRecalc.RecalcOnSave == (ushort) 1;
    set => this.m_saveRecalc.RecalcOnSave = value ? (ushort) 1 : (ushort) 0;
  }

  public double MaximumChange
  {
    get => this.m_delta.MaxChange;
    set => this.m_delta.MaxChange = value;
  }

  public bool IsIterationEnabled
  {
    get => this.m_iteration.IsIteration == (ushort) 1;
    set => this.m_iteration.IsIteration = value ? (ushort) 1 : (ushort) 0;
  }

  public bool R1C1ReferenceMode
  {
    get => this.m_refMode.IsA1ReferenceMode == (ushort) 0;
    set => this.m_refMode.IsA1ReferenceMode = value ? (ushort) 0 : (ushort) 1;
  }

  public object Clone(object parent)
  {
    CalculationOptionsImpl calculationOptionsImpl = (CalculationOptionsImpl) this.MemberwiseClone();
    calculationOptionsImpl.SetParent(parent);
    calculationOptionsImpl.m_calcMode = (CalcModeRecord) CloneUtils.CloneCloneable((ICloneable) this.m_calcMode);
    calculationOptionsImpl.m_calcCount = (CalcCountRecord) CloneUtils.CloneCloneable((ICloneable) this.m_calcCount);
    calculationOptionsImpl.m_refMode = (RefModeRecord) CloneUtils.CloneCloneable((ICloneable) this.m_refMode);
    calculationOptionsImpl.m_iteration = (IterationRecord) CloneUtils.CloneCloneable((ICloneable) this.m_iteration);
    calculationOptionsImpl.m_delta = (DeltaRecord) CloneUtils.CloneCloneable((ICloneable) this.m_delta);
    calculationOptionsImpl.m_saveRecalc = (SaveRecalcRecord) CloneUtils.CloneCloneable((ICloneable) this.m_saveRecalc);
    return (object) calculationOptionsImpl;
  }
}
