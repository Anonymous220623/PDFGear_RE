// Decompiled with JetBrains decompiler
// Type: Syncfusion.Calculate.FormulaComputedEventArgs
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.Calculate;

internal class FormulaComputedEventArgs : EventArgs
{
  private string formula;
  private string computedValue;
  private string cell;
  private bool isInnerFormula;
  private bool handled;

  internal FormulaComputedEventArgs(
    string formula,
    string computedValue,
    string cell,
    bool isInnerFormula)
  {
    this.formula = formula;
    this.computedValue = computedValue;
    this.cell = cell;
    this.isInnerFormula = isInnerFormula;
  }

  internal string Formula => this.formula;

  internal string ComputedValue
  {
    get => this.computedValue;
    set => this.computedValue = value;
  }

  internal string Cell => this.cell;

  internal bool IsInnerFormula => this.isInnerFormula;

  internal bool Handled
  {
    get => this.handled;
    set => this.handled = value;
  }
}
