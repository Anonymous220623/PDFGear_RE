// Decompiled with JetBrains decompiler
// Type: Syncfusion.Calculate.FormulaInfo
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.Calculate;

public class FormulaInfo
{
  private string _formulaText;
  private string _formulaValue;
  private string _parsedFormula;
  internal int calcID = -2147483647 /*0x80000001*/;

  public string FormulaText
  {
    get => this._formulaText;
    set => this._formulaText = value;
  }

  public string FormulaValue
  {
    get => this._formulaValue;
    set => this._formulaValue = value;
  }

  public string ParsedFormula
  {
    get => this._parsedFormula;
    set => this._parsedFormula = value;
  }
}
