// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.EvaluateEventArgs
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

[Preserve(AllMembers = true)]
public class EvaluateEventArgs : EventArgs
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
