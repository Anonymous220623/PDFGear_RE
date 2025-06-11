// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.EvaluateEventArgs
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Parser.Biff_Records.Formula;
using System;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

[Preserve(AllMembers = true)]
internal class EvaluateEventArgs : EventArgs
{
  private IRange m_range;
  private Ptg[] m_FormulaTokens;

  private EvaluateEventArgs()
  {
  }

  public EvaluateEventArgs(IRange range, Ptg[] array)
  {
    this.m_range = range;
    this.m_FormulaTokens = array;
  }

  public IRange Range => this.m_range;

  public Ptg[] PtgArray => this.m_FormulaTokens;

  public static EvaluateEventArgs Empty => new EvaluateEventArgs();
}
