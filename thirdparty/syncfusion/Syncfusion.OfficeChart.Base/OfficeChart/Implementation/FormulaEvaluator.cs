// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.FormulaEvaluator
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Parser.Biff_Records.Formula;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class FormulaEvaluator
{
  public object TryGetValue(Ptg[] formula, IWorksheet sheet)
  {
    if (formula == null || formula.Length == 0)
      return (object) null;
    return formula.Length == 1 ? this.GetSingleTokenResult(formula[0], sheet) : (object) null;
  }

  private object GetSingleTokenResult(Ptg token, IWorksheet sheet)
  {
    object singleTokenResult = (object) null;
    switch (token.TokenCode)
    {
      case FormulaToken.tStringConstant:
        singleTokenResult = (object) (token as StringConstantPtg).Value;
        break;
      case FormulaToken.tBoolean:
        singleTokenResult = (object) (token as BooleanPtg).Value;
        break;
      case FormulaToken.tInteger:
        singleTokenResult = (object) (double) (token as IntegerPtg).Value;
        break;
      case FormulaToken.tNumber:
        singleTokenResult = (object) (token as DoublePtg).Value;
        break;
      case FormulaToken.tRef1:
      case FormulaToken.tRef2:
      case FormulaToken.tRef3:
        IRange range = (token as RefPtg).GetRange(sheet.Workbook, sheet);
        if ((range as RangeImpl).IsSingleCell || range.HasFormula)
        {
          singleTokenResult = range.Value2;
          break;
        }
        break;
    }
    return singleTokenResult;
  }
}
