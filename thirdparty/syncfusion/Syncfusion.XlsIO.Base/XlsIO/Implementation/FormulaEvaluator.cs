// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.FormulaEvaluator
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class FormulaEvaluator
{
  public object TryGetValue(Ptg[] formula, IWorksheet sheet)
  {
    return this.TryGetValue(formula, sheet, (string) null);
  }

  internal object TryGetValue(Ptg[] formula, IWorksheet sheet, string formulaString)
  {
    if (formula == null || formula.Length == 0)
      return (object) null;
    if (formula.Length == 1)
      return this.GetSingleTokenResult(formula[0], sheet);
    if (formula.Length == 2 && formula[1].TokenCode == FormulaToken.tPercent)
      return (object) (Convert.ToDouble(this.GetSingleTokenResult(formula[0], sheet)) / 100.0);
    return formula.Length >= 2 && formulaString != null && sheet.CalcEngine != null ? (object) sheet.CalcEngine.ParseAndComputeFormula(formulaString) : (object) null;
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
      case FormulaToken.tName1:
      case FormulaToken.tRef1:
      case FormulaToken.tName2:
      case FormulaToken.tRef2:
      case FormulaToken.tName3:
      case FormulaToken.tRef3:
        IRange range = (IRange) null;
        if (token is RefPtg)
          range = (token as RefPtg).GetRange(sheet.Workbook, sheet);
        else if (token is NamePtg)
        {
          range = (IRange) ((token as NamePtg).GetRange(sheet.Workbook, sheet) as NameImpl);
          if (range != null && range is NameImpl)
            range = range.Worksheet[(range as NameImpl).AddressLocal];
        }
        if (range != null)
        {
          if (range.HasFormula)
          {
            switch ((sheet as WorksheetImpl).GetCellType(range.Row, range.Column, true))
            {
              case WorksheetImpl.TRangeValueType.Error | WorksheetImpl.TRangeValueType.Formula:
                singleTokenResult = (object) (sheet as WorksheetImpl).GetFormulaErrorValue(range.Row, range.Column);
                break;
              case WorksheetImpl.TRangeValueType.Boolean | WorksheetImpl.TRangeValueType.Formula:
                singleTokenResult = (object) (sheet as WorksheetImpl).GetFormulaBoolValue(range.Row, range.Column);
                break;
              case WorksheetImpl.TRangeValueType.Number | WorksheetImpl.TRangeValueType.Formula:
                singleTokenResult = (object) (sheet as WorksheetImpl).GetFormulaNumberValue(range.Row, range.Column);
                break;
              case WorksheetImpl.TRangeValueType.Formula | WorksheetImpl.TRangeValueType.String:
                singleTokenResult = (object) (sheet as WorksheetImpl).GetFormulaStringValue(range.Row, range.Column);
                break;
            }
          }
          else
          {
            if ((range as RangeImpl).IsSingleCell)
            {
              singleTokenResult = range.Value2;
              break;
            }
            break;
          }
        }
        else
          break;
        break;
    }
    return singleTokenResult;
  }

  internal bool CheckFomula(Ptg[] tokens)
  {
    for (int index = 0; index < tokens.Length; ++index)
    {
      Ptg token = tokens[index];
      if (token.TokenCode == FormulaToken.tFunctionVar2 || token.TokenCode == FormulaToken.tRef1 || token.TokenCode == FormulaToken.tRef2 || token.TokenCode == FormulaToken.tRef3)
        return true;
    }
    return false;
  }
}
