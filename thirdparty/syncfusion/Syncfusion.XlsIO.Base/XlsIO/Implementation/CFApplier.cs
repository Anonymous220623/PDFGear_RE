// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.CFApplier
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.Calculate;
using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class CFApplier
{
  private const string m_trueString = "TRUE";
  private FormulaEvaluator m_evaluator = new FormulaEvaluator();
  private StringComparer m_comparer = new StringComparer();
  private Rectangle m_range;
  private List<Rectangle> m_ranges;
  private uint m_cfApplied;
  private bool m_calculationEnabled;
  private int m_columnDifference;
  private int m_rowDifference;
  private Rectangle m_firstRange;
  private int m_firstRowDifference;
  private Dictionary<string, string> m_calcValues;
  private Dictionary<IConditionalFormat, string> m_Formulas;
  private Dictionary<TopBottomImpl, List<long>> m_top10Cells;
  private Dictionary<long, double> m_cellValues;
  private Dictionary<IConditionalFormats, List<double>> m_aboveAverageValues;
  private double m_minValue;
  private double m_maxValue;
  private bool m_isNegative;
  private bool m_isColorApplied;

  internal bool IsColorApplied
  {
    get => this.m_isColorApplied;
    set => this.m_isColorApplied = value;
  }

  internal bool IsNegative
  {
    get => this.m_isNegative;
    set => this.m_isNegative = value;
  }

  internal double MinValue
  {
    get => this.m_minValue;
    set => this.m_minValue = value;
  }

  internal double MaxValue
  {
    get => this.m_maxValue;
    set => this.m_maxValue = value;
  }

  internal Dictionary<string, string> CalcValues
  {
    get
    {
      if (this.m_calcValues == null)
        this.m_calcValues = new Dictionary<string, string>();
      return this.m_calcValues;
    }
    set => this.m_calcValues = value;
  }

  internal bool CalculationEnabled
  {
    get => this.m_calculationEnabled;
    set => this.m_calculationEnabled = value;
  }

  internal int FirstRowDifference
  {
    get => this.m_firstRowDifference;
    set => this.m_firstRowDifference = value;
  }

  internal Rectangle FirstRange
  {
    get => this.m_firstRange;
    set => this.m_firstRange = value;
  }

  internal int ColumnDifference
  {
    get => this.m_columnDifference;
    set => this.m_columnDifference = value;
  }

  internal int RowDifference
  {
    get => this.m_rowDifference;
    set => this.m_rowDifference = value;
  }

  internal Dictionary<TopBottomImpl, List<long>> Top10Cells
  {
    get => this.m_top10Cells;
    set => this.m_top10Cells = value;
  }

  internal Dictionary<long, double> CellValues
  {
    get => this.m_cellValues;
    set => this.m_cellValues = value;
  }

  internal Dictionary<IConditionalFormats, List<double>> AboveAverageValues
  {
    get => this.m_aboveAverageValues;
    set => this.m_aboveAverageValues = value;
  }

  internal void SetRange(Rectangle range) => this.m_range = range;

  internal void SetRanges(List<Rectangle> ranges) => this.m_ranges = ranges;

  public ExtendedFormatImpl ApplyCF(IRange cell, ExtendedFormatImpl xf)
  {
    ExtendedFormatImpl extendedFormatImpl = (ExtendedFormatImpl) null;
    this.m_cfApplied = 0U;
    if ((cell as RangeImpl).HasConditionFormats)
    {
      IConditionalFormats conditionalFormats = cell.ConditionalFormats;
      int index = 0;
      for (int count = conditionalFormats.Count; index < count && extendedFormatImpl == null; ++index)
      {
        extendedFormatImpl = this.CheckAndApplyCondition(conditionalFormats[index], cell, xf);
        if (this.MaxValue != 0.0)
          this.MaxValue = 0.0;
        if (this.MinValue != 0.0)
          this.MaxValue = 0.0;
        if (this.IsNegative)
          this.IsNegative = false;
        if (this.IsColorApplied)
          this.IsColorApplied = false;
      }
    }
    return extendedFormatImpl ?? xf;
  }

  internal ExtendedFormatImpl ApplyCFNumberFormats(IRange cell, ExtendedFormatImpl xf)
  {
    ExtendedFormatImpl extendedFormatImpl1 = (ExtendedFormatImpl) null;
    ExtendedFormatImpl extendedFormatImpl2 = (ExtendedFormatImpl) null;
    IConditionalFormats conditionalFormats = cell.ConditionalFormats;
    for (int index = 0; index <= conditionalFormats.Count - 1; ++index)
    {
      IConditionalFormat condition = conditionalFormats[index];
      if (condition is ConditionalFormatWrapper)
        condition = (IConditionalFormat) (condition as ConditionalFormatWrapper).GetCondition();
      if (condition.NumberFormat != "General" || extendedFormatImpl1 != null || condition.FormatType == ExcelCFType.IconSet)
        extendedFormatImpl1 = this.CheckAndApplyCondition(condition, cell, extendedFormatImpl1 ?? xf);
      if (condition.StopIfTrue && extendedFormatImpl1 != null)
        return extendedFormatImpl1;
      if (extendedFormatImpl1 == null && extendedFormatImpl2 != null)
        extendedFormatImpl1 = extendedFormatImpl2;
      extendedFormatImpl2 = extendedFormatImpl1;
      if (this.MaxValue != 0.0)
        this.MaxValue = 0.0;
      if (this.MinValue != 0.0)
        this.MinValue = 0.0;
      if (this.IsNegative)
        this.IsNegative = false;
      if (this.IsColorApplied)
        this.IsColorApplied = false;
    }
    return extendedFormatImpl1 ?? xf;
  }

  internal ExtendedFormatImpl MergeCF(
    ConditionalFormats format,
    ExtendedFormatImpl xf,
    IRange cell)
  {
    ExtendedFormatImpl xf1 = (ExtendedFormatImpl) null;
    ExtendedFormatImpl extendedFormatImpl = (ExtendedFormatImpl) null;
    List<IConditionalFormat> conditionalFormatList = new List<IConditionalFormat>();
    bool flag = false;
    for (int i1 = 0; i1 < format.Count; ++i1)
    {
      for (int i2 = i1 + 1; i2 < format.Count; ++i2)
      {
        if ((format[i1] as ConditionalFormatImpl).Priority < (format[i2] as ConditionalFormatImpl).Priority)
        {
          IConditionalFormat conditionalFormat = format[i1];
          format[i1] = format[i2];
          format[i2] = conditionalFormat;
        }
      }
    }
    for (int i = 0; i < format.Count; ++i)
    {
      if ((format[i] as ConditionalFormatImpl).Priority == 0)
      {
        flag = true;
        conditionalFormatList.Add(format[i]);
      }
    }
    if (flag && conditionalFormatList.Count > 0)
      conditionalFormatList.Reverse();
    for (int i = 0; i < format.Count; ++i)
    {
      if ((format[i] as ConditionalFormatImpl).Priority != 0)
        conditionalFormatList.Add(format[i]);
    }
    for (int index = conditionalFormatList.Count - 1; index >= 0; --index)
    {
      IConditionalFormat format1 = conditionalFormatList[index];
      if (xf1 == null)
      {
        xf1 = this.CheckAndApplyCondition(format1, cell, xf);
      }
      else
      {
        extendedFormatImpl = xf1;
        xf1 = this.CheckAndApplyCondition(format1, cell, xf1) ?? extendedFormatImpl;
      }
      if (format1.StopIfTrue && xf1 != null && xf1 != extendedFormatImpl)
        return xf1;
    }
    return xf1 ?? xf;
  }

  private ExtendedFormatImpl CheckAndApplyCondition(
    IConditionalFormat format,
    IRange cell,
    ExtendedFormatImpl xf)
  {
    ExtendedFormatImpl extendedFormatImpl = (ExtendedFormatImpl) null;
    int lastRow1 = cell.Worksheet.UsedRange.LastRow;
    int lastColumn1 = cell.Worksheet.UsedRange.LastColumn;
    int lastRow2 = 0;
    int lastColumn2 = 0;
    format = format is ConditionalFormatImpl ? (IConditionalFormat) (format as ConditionalFormatImpl) : (IConditionalFormat) (format as ConditionalFormatWrapper).GetCondition();
    string separator = cell.Worksheet.Application.ArgumentsSeparator.ToString();
    if (format is ConditionalFormatImpl && (format as ConditionalFormatImpl).IsNegativePriority)
      return xf;
    this.m_cfApplied = !(format is ConditionalFormatImpl) || (format as ConditionalFormatImpl).Priority >= xf.CFPriority || (format as ConditionalFormatImpl).Priority == 0 ? xf.CFApplied : 0U;
    switch (format.FormatType)
    {
      case ExcelCFType.CellValue:
        extendedFormatImpl = this.CheckAndApplyConditionValue(format, cell, xf);
        break;
      case ExcelCFType.Formula:
        extendedFormatImpl = this.CheckAndApplyConditionFormula(format, cell, xf);
        break;
      case ExcelCFType.ColorScale:
        if (this.m_ranges != null)
        {
          List<string> stringList = new List<string>();
          foreach (Rectangle range in this.m_ranges)
          {
            this.UpdateConditionalFormatRange(range.Top, range.Left, range.Bottom, range.Right, lastRow1, lastColumn1, out lastRow2, out lastColumn2);
            stringList.Add(RangeImpl.GetAddressLocal(range.Top, range.Left, lastRow2, lastColumn2));
          }
          extendedFormatImpl = this.ConditionalFormatColorScale(format.ColorScale, cell, string.Join(separator, stringList.ToArray()), xf);
          break;
        }
        this.UpdateConditionalFormatRange(this.m_range.Top, this.m_range.Left, this.m_range.Bottom, this.m_range.Right, lastRow1, lastColumn1, out lastRow2, out lastColumn2);
        extendedFormatImpl = this.ConditionalFormatColorScale(format.ColorScale, cell, RangeImpl.GetAddressLocal(this.m_range.Top, this.m_range.Left, lastRow2, lastColumn2), xf);
        break;
      case ExcelCFType.DataBar:
        if (this.m_ranges != null)
        {
          List<string> stringList = new List<string>();
          foreach (Rectangle range in this.m_ranges)
          {
            this.UpdateConditionalFormatRange(range.Top, range.Left, range.Bottom, range.Right, lastRow1, lastColumn1, out lastRow2, out lastColumn2);
            stringList.Add(RangeImpl.GetAddressLocal(range.Top, range.Left, lastRow2, lastColumn2));
          }
          extendedFormatImpl = (ExtendedFormatImpl) this.ApplyDataBar(cell, format.DataBar, string.Join(separator, stringList.ToArray()), xf);
          break;
        }
        this.UpdateConditionalFormatRange(this.m_range.Top, this.m_range.Left, this.m_range.Bottom, this.m_range.Right, lastRow1, lastColumn1, out lastRow2, out lastColumn2);
        extendedFormatImpl = (ExtendedFormatImpl) this.ApplyDataBar(cell, format.DataBar, RangeImpl.GetAddressLocal(this.m_range.Top, this.m_range.Left, lastRow2, lastColumn2), xf);
        break;
      case ExcelCFType.IconSet:
        if (this.m_ranges != null)
        {
          List<string> stringList = new List<string>();
          foreach (Rectangle range in this.m_ranges)
          {
            this.UpdateConditionalFormatRange(range.Top, range.Left, range.Bottom, range.Right, lastRow1, lastColumn1, out lastRow2, out lastColumn2);
            stringList.Add(RangeImpl.GetAddressLocal(range.Top, range.Left, lastRow2, lastColumn2));
          }
          extendedFormatImpl = (ExtendedFormatImpl) this.ApplyIconSet(cell, format.IconSet, string.Join(separator, stringList.ToArray()), xf);
        }
        else
        {
          this.UpdateConditionalFormatRange(this.m_range.Top, this.m_range.Left, this.m_range.Bottom, this.m_range.Right, lastRow1, lastColumn1, out lastRow2, out lastColumn2);
          extendedFormatImpl = (ExtendedFormatImpl) this.ApplyIconSet(cell, format.IconSet, RangeImpl.GetAddressLocal(this.m_range.Top, this.m_range.Left, lastRow2, lastColumn2), xf);
        }
        if (extendedFormatImpl is ExtendedFormatStandAlone && (extendedFormatImpl as ExtendedFormatStandAlone).IconName != null)
        {
          (extendedFormatImpl as ExtendedFormatStandAlone).AdvancedCFIcon = (Image) this.GetBitMap((extendedFormatImpl as ExtendedFormatStandAlone).IconName);
          break;
        }
        break;
      case ExcelCFType.Blank:
        WorksheetImpl.TRangeValueType type1;
        object textFromCellType1 = (cell.Worksheet as WorksheetImpl).GetTextFromCellType(cell.Row, cell.Column, out type1);
        if (type1 == WorksheetImpl.TRangeValueType.Blank || textFromCellType1 != null && textFromCellType1 is string && textFromCellType1.ToString().Trim().Length == 0)
        {
          extendedFormatImpl = this.ApplyCondition(format, xf);
          break;
        }
        break;
      case ExcelCFType.NoBlank:
        WorksheetImpl.TRangeValueType type2;
        object textFromCellType2 = (cell.Worksheet as WorksheetImpl).GetTextFromCellType(cell.Row, cell.Column, out type2);
        if (type2 != WorksheetImpl.TRangeValueType.Blank && textFromCellType2 != null && (!(textFromCellType2 is string) || textFromCellType2.ToString().Trim().Length != 0))
        {
          extendedFormatImpl = this.ApplyCondition(format, xf);
          break;
        }
        break;
      case ExcelCFType.SpecificText:
        extendedFormatImpl = this.CheckAndApplySpecificText(format, cell, xf);
        break;
      case ExcelCFType.ContainsErrors:
        switch ((cell.Worksheet as WorksheetImpl).GetCellType(cell.Row, cell.Column, true))
        {
          case WorksheetImpl.TRangeValueType.Error:
          case WorksheetImpl.TRangeValueType.Error | WorksheetImpl.TRangeValueType.Formula:
            extendedFormatImpl = this.ApplyCondition(format, xf);
            break;
        }
        break;
      case ExcelCFType.NotContainsErrors:
        switch ((cell.Worksheet as WorksheetImpl).GetCellType(cell.Row, cell.Column, true))
        {
          case WorksheetImpl.TRangeValueType.Error:
          case WorksheetImpl.TRangeValueType.Error | WorksheetImpl.TRangeValueType.Formula:
            break;
          default:
            extendedFormatImpl = this.ApplyCondition(format, xf);
            break;
        }
        break;
      case ExcelCFType.Duplicate:
      case ExcelCFType.Unique:
        extendedFormatImpl = this.ConditionalFormatUniqueDuplicateValue(format, cell, xf);
        break;
      case ExcelCFType.TopBottom:
        extendedFormatImpl = this.ConditionalFormatTopBottom(format, cell, xf);
        break;
      case ExcelCFType.AboveBelowAverage:
        extendedFormatImpl = this.ConditionalFormatAboveAverage(format, cell, xf);
        break;
      default:
        string description = $"Unsupported conditional format type: \"{(object) format.FormatType}\" is applied in the cell \"{cell.Address}\"";
        (cell.Worksheet.Workbook as WorkbookImpl).RaiseWarning(description, WarningType.ConditionalFormatting);
        extendedFormatImpl = (ExtendedFormatImpl) null;
        break;
    }
    if (extendedFormatImpl != null)
    {
      extendedFormatImpl.CFApplied = this.m_cfApplied;
      extendedFormatImpl.CFPriority = !(format is ConditionalFormatImpl) || xf.CFPriority != 0 && (format as ConditionalFormatImpl).Priority >= xf.CFPriority || this.m_cfApplied <= 0U ? xf.CFPriority : (format as ConditionalFormatImpl).Priority;
    }
    return extendedFormatImpl;
  }

  private void UpdateConditionalFormatRange(
    int top,
    int left,
    int bottom,
    int right,
    int usedRangeLastRow,
    int usedRangeLastColumn,
    out int lastRow,
    out int lastColumn)
  {
    lastRow = 0;
    lastColumn = 0;
    lastRow = bottom <= usedRangeLastRow ? bottom : usedRangeLastRow;
    lastColumn = right <= usedRangeLastColumn ? right : usedRangeLastColumn;
    if (lastRow < top)
      lastRow = bottom;
    if (lastColumn >= left)
      return;
    lastColumn = right;
  }

  private ExtendedFormatImpl ConditionalFormatUniqueDuplicateValue(
    IConditionalFormat format,
    IRange cell,
    ExtendedFormatImpl xf)
  {
    ExtendedFormatImpl extendedFormatImpl = (ExtendedFormatImpl) null;
    if (format != null && !cell.IsBlank)
    {
      format = format is ConditionalFormatImpl ? (IConditionalFormat) (format as ConditionalFormatImpl) : (IConditionalFormat) (format as ConditionalFormatWrapper).GetCondition();
      bool flag = this.HasDuplicate(format.Parent as ConditionalFormats, cell);
      if (format.FormatType == ExcelCFType.Unique && !flag)
        extendedFormatImpl = this.ApplyCondition(format, xf);
      else if (format.FormatType == ExcelCFType.Duplicate && flag)
        extendedFormatImpl = this.ApplyCondition(format, xf);
    }
    return extendedFormatImpl;
  }

  private bool HasDuplicate(ConditionalFormats parentCF, IRange cell)
  {
    WorksheetImpl sheet = parentCF.sheet;
    object textFromCellType1 = parentCF.sheet.GetTextFromCellType(cell.Row, cell.Column, out WorksheetImpl.TRangeValueType _);
    if (textFromCellType1 != null)
    {
      int row1 = cell.Row;
      int column1 = cell.Column;
      for (int index = 0; index < parentCF.CellsRectangleList.Count; ++index)
      {
        Rectangle cellsRectangle = parentCF.CellsRectangleList[index];
        int num1 = cellsRectangle.Y + 1;
        int num2 = cellsRectangle.X + 1;
        int num3 = cellsRectangle.Bottom + 1;
        int num4 = cellsRectangle.Right + 1;
        for (int row2 = num1; row2 <= num3; ++row2)
        {
          for (int column2 = num2; column2 <= num4; ++column2)
          {
            object textFromCellType2 = sheet.GetTextFromCellType(row2, column2, out WorksheetImpl.TRangeValueType _);
            if (textFromCellType2 != null && (row1 != row2 || column1 != column2) && (textFromCellType2.Equals(textFromCellType1) || textFromCellType2.ToString().Equals(textFromCellType1.ToString(), StringComparison.OrdinalIgnoreCase)))
              return true;
          }
        }
      }
    }
    return false;
  }

  private ExtendedFormatImpl CheckAndApplyConditionValue(
    IConditionalFormat format,
    IRange cell,
    ExtendedFormatImpl xf)
  {
    IInternalConditionalFormat conditionalFormat = format as IInternalConditionalFormat;
    IWorksheet worksheet = cell.Worksheet;
    object obj1 = (object) null;
    object obj2 = (object) null;
    if (!(cell.Worksheet.Workbook as WorkbookImpl).EnabledCalcEngine)
    {
      if (conditionalFormat.FirstFormula != null)
      {
        Ptg[] firstFormulaPtgs = conditionalFormat.FirstFormulaPtgs;
      }
      else if (conditionalFormat.SecondFormula != null)
      {
        Ptg[] secondFormulaPtgs = conditionalFormat.SecondFormulaPtgs;
      }
      else
        goto label_6;
      this.m_calculationEnabled = true;
      cell.Worksheet.EnableSheetCalculations();
      (cell.Worksheet.Workbook as WorkbookImpl).CalcEngineMemberValuesOnSheet(false);
    }
label_6:
    bool flag1 = conditionalFormat.FirstFormulaPtgs != null && conditionalFormat.FirstFormulaPtgs.Length >= 2;
    bool flag2 = conditionalFormat.SecondFormulaPtgs != null && conditionalFormat.SecondFormulaPtgs.Length >= 2;
    string str1;
    Ptg[] firstFormulaPtgs1;
    if (conditionalFormat.FirstFormulaPtgs != null && !string.IsNullOrEmpty(conditionalFormat.FirstFormula) && this.m_evaluator.CheckFomula(conditionalFormat.FirstFormulaPtgs))
    {
      bool bResult = false;
      str1 = this.UpdateConditionFormula(format, cell, xf, conditionalFormat.FirstFormula, ref bResult);
      firstFormulaPtgs1 = (format as ConditionalFormatImpl).Workbook.FormulaUtil.ParseString(str1);
    }
    else
    {
      str1 = conditionalFormat.FirstFormula;
      firstFormulaPtgs1 = conditionalFormat.FirstFormulaPtgs;
    }
    string str2;
    Ptg[] secondFormulaPtgs1;
    if (conditionalFormat.SecondFormulaPtgs != null && !string.IsNullOrEmpty(conditionalFormat.SecondFormula) && this.m_evaluator.CheckFomula(conditionalFormat.SecondFormulaPtgs))
    {
      bool bResult = false;
      str2 = this.UpdateConditionFormula(format, cell, xf, conditionalFormat.SecondFormula, ref bResult);
      secondFormulaPtgs1 = (format as ConditionalFormatImpl).Workbook.FormulaUtil.ParseString(str2);
    }
    else
    {
      str2 = conditionalFormat.SecondFormula;
      secondFormulaPtgs1 = conditionalFormat.SecondFormulaPtgs;
    }
    if (!flag1 || !this.HasErrorPtg(firstFormulaPtgs1))
    {
      if (flag1 && this.CalculationEnabled && obj1 != null && this.CalcValues.ContainsKey(str1))
      {
        obj1 = (object) this.CalcValues[str1];
      }
      else
      {
        obj1 = this.m_evaluator.TryGetValue(firstFormulaPtgs1, worksheet, str1);
        if (flag1 && this.CalculationEnabled && obj1 != null && !this.CalcValues.ContainsKey(str1))
          this.CalcValues.Add(str1, obj1.ToString());
      }
    }
    if (!flag2 || !this.HasErrorPtg(secondFormulaPtgs1))
    {
      if (flag2 && this.CalculationEnabled && obj2 != null && this.CalcValues.ContainsKey(str2))
      {
        obj2 = (object) this.CalcValues[str2];
      }
      else
      {
        obj2 = this.m_evaluator.TryGetValue(secondFormulaPtgs1, worksheet, str2);
        if (flag2 && this.CalculationEnabled && obj2 != null && !this.CalcValues.ContainsKey(str2))
          this.CalcValues.Add(str2, obj2.ToString());
      }
    }
    bool flag3 = false;
    switch (format.Operator)
    {
      case ExcelComparisonOperator.Between:
        flag3 = !cell.IsBlank || !(obj1 is double) || !(obj2 is double) ? (!cell.IsBlank || !(obj1 is bool) ? (!cell.IsBlank || !(obj1 is string) ? this.CheckBetween(cell, obj1, obj2) : this.CompareString(string.Empty, obj1) > 0 && this.CompareString(string.Empty, obj2) < 0) : this.CompareBoolean(false, obj1) >= 0 || this.CompareBoolean(false, obj2) >= 0) : this.CompareDouble(0.0, obj1) >= 0 && this.CompareDouble(0.0, obj2) <= 0;
        break;
      case ExcelComparisonOperator.NotBetween:
        flag3 = !cell.IsBlank || !(obj1 is double) || !(obj2 is double) ? (!cell.IsBlank || !(obj1 is bool) ? (!cell.IsBlank || !(obj1 is string) ? this.CheckNotBetween(cell, obj1, obj2) : this.CompareString(string.Empty, obj1) <= 0 || this.CompareString(string.Empty, obj2) >= 0) : this.CompareBoolean(false, obj1) < 0 && this.CompareBoolean(false, obj2) < 0) : this.CompareDouble(0.0, obj1) <= 0 || this.CompareDouble(0.0, obj2) >= 0;
        break;
      case ExcelComparisonOperator.Equal:
        flag3 = !cell.IsBlank || !(obj1 is double) ? (!cell.IsBlank || !(obj1 is bool) ? (!cell.IsBlank || !(obj1 is string) ? this.CheckEqual(cell, obj1) : this.CompareString(string.Empty, obj1) == 0) : this.CompareBoolean(false, obj1) == 0) : this.CompareDouble(0.0, obj1) == 0;
        if (!flag3)
        {
          flag3 = obj1 is string && cell.Text != null && cell.Text.Equals(obj1.ToString(), StringComparison.OrdinalIgnoreCase);
          break;
        }
        break;
      case ExcelComparisonOperator.NotEqual:
        flag3 = !cell.IsBlank || !(obj1 is double) && (obj1 == null || !double.TryParse(obj1.ToString(), out double _)) ? (!cell.IsBlank || !(obj1 is bool) ? (!cell.IsBlank || !(obj1 is string) ? this.CheckNotEqual(cell, obj1) : this.CompareString(string.Empty, obj1) != 0) : this.CompareBoolean(false, obj1) != 0) : this.CompareDouble(0.0, obj1) != 0;
        break;
      case ExcelComparisonOperator.Greater:
        flag3 = !cell.IsBlank || !(obj1 is double) ? (!cell.IsBlank || !(obj1 is bool) ? (!cell.IsBlank || !(obj1 is string) ? this.CheckGreater(cell, obj1) : this.CompareString(string.Empty, obj1) > 0) : this.CompareBoolean(false, obj1) > 0) : this.CompareDouble(0.0, obj1) > 0;
        break;
      case ExcelComparisonOperator.Less:
        flag3 = !cell.IsBlank || !(obj1 is double) ? (!cell.IsBlank || !(obj1 is bool) ? (!cell.IsBlank || !(obj1 is string) ? this.CheckLess(cell, obj1) : this.CompareString(string.Empty, obj1) < 0) : this.CompareBoolean(false, obj1) < 0) : this.CompareDouble(0.0, obj1) < 0;
        break;
      case ExcelComparisonOperator.GreaterOrEqual:
        flag3 = !cell.IsBlank || !(obj1 is double) ? (!cell.IsBlank || !(obj1 is bool) ? (!cell.IsBlank || !(obj1 is string) ? this.CheckGreaterOrEqual(cell, obj1) : this.CompareString(string.Empty, obj1) >= 0) : this.CompareBoolean(false, obj1) >= 0) : this.CompareDouble(0.0, obj1) >= 0;
        break;
      case ExcelComparisonOperator.LessOrEqual:
        flag3 = !cell.IsBlank || !(obj1 is double) ? (!cell.IsBlank || !(obj1 is bool) ? (!cell.IsBlank || !(obj1 is string) ? this.CheckLessOrEqual(cell, obj1) : this.CompareString(string.Empty, obj1) <= 0) : this.CompareBoolean(false, obj1) <= 0) : this.CompareDouble(0.0, obj1) <= 0;
        break;
    }
    ExtendedFormatImpl extendedFormatImpl = (ExtendedFormatImpl) null;
    if (flag3)
      extendedFormatImpl = this.ApplyCondition(format, xf);
    return extendedFormatImpl;
  }

  private bool CheckBetween(IRange cell, object value1, object value2)
  {
    return value1 != null && value2 != null && this.CheckGreaterOrEqual(cell, value1) && this.CheckLessOrEqual(cell, value2);
  }

  private bool CheckEqual(IRange cell, object value)
  {
    return value != null && this.Compare(cell, value) == 0;
  }

  private bool CheckGreater(IRange cell, object value)
  {
    return value != null && (this.Compare(cell, value) > 0 || value is double && cell.HasString);
  }

  private bool CheckGreaterOrEqual(IRange cell, object value)
  {
    return value != null && this.Compare(cell, value) >= 0;
  }

  private bool CheckLess(IRange cell, object value)
  {
    if (value == null)
      return false;
    int num = this.Compare(cell, value);
    return num != int.MinValue && num < 0;
  }

  private bool CheckLessOrEqual(IRange cell, object value)
  {
    if (value == null)
      return false;
    int num = this.Compare(cell, value);
    if (num != int.MinValue && num <= 0)
      return true;
    return num == int.MinValue && cell.IsBlank;
  }

  private bool CheckNotBetween(IRange cell, object value1, object value2)
  {
    if (value1 == null || value2 == null)
      return false;
    return this.CheckLess(cell, value1) || this.CheckGreater(cell, value2);
  }

  private bool CheckNotEqual(IRange cell, object value)
  {
    return value != null && this.Compare(cell, value) != 0;
  }

  private int Compare(IRange cell, object value)
  {
    if (value == null)
      return int.MinValue;
    int num = int.MinValue;
    switch ((cell.Worksheet as WorksheetImpl).GetCellType(cell.Row, cell.Column, false))
    {
      case WorksheetImpl.TRangeValueType.Boolean:
        num = this.CompareBoolean(cell.Boolean, value);
        break;
      case WorksheetImpl.TRangeValueType.Number:
        num = this.CompareDouble(cell.Number, value);
        break;
      case WorksheetImpl.TRangeValueType.Formula:
        if (!this.CalculationEnabled)
          RangeImpl.UpdateCellValue((object) cell.Worksheet, cell.Column, cell.Row, true);
        switch ((cell.Worksheet as WorksheetImpl).GetCellType(cell.Row, cell.Column, true))
        {
          case WorksheetImpl.TRangeValueType.Boolean | WorksheetImpl.TRangeValueType.Formula:
            num = this.CompareBoolean(cell.FormulaBoolValue, value);
            break;
          case WorksheetImpl.TRangeValueType.Number | WorksheetImpl.TRangeValueType.Formula:
            num = this.CompareDouble(cell.FormulaNumberValue, value);
            break;
          case WorksheetImpl.TRangeValueType.Formula | WorksheetImpl.TRangeValueType.String:
            num = this.CompareString(cell.FormulaStringValue, value);
            break;
        }
        break;
      case WorksheetImpl.TRangeValueType.String:
        num = this.CompareString(cell.Text, value);
        break;
    }
    return num;
  }

  private int CompareDouble(double number, object value)
  {
    switch (value)
    {
      case string _:
        double result = 0.0;
        if (!double.TryParse(value.ToString(), out result))
          return int.MinValue;
        if (value.ToString().Length > 15)
          number = double.Parse(number.ToString());
        return number.CompareTo(result);
      case double num:
        return number.CompareTo(num);
      default:
        return int.MinValue;
    }
  }

  private int CompareString(string text, object value)
  {
    return !(value is string y) ? int.MinValue : this.m_comparer.Compare((object) text, (object) y);
  }

  private int CompareBoolean(bool boolean, object value)
  {
    switch (value)
    {
      case string _:
        bool result = false;
        return bool.TryParse(value.ToString(), out result) ? boolean.CompareTo(result) : int.MinValue;
      case bool flag:
        return boolean.CompareTo(flag);
      default:
        return int.MinValue;
    }
  }

  private ExtendedFormatImpl CheckAndApplyConditionFormula(
    IConditionalFormat format,
    IRange cell,
    ExtendedFormatImpl xf)
  {
    ExtendedFormatImpl extendedFormatImpl = (ExtendedFormatImpl) null;
    string formatFirstFormula = (string) null;
    if (this.m_Formulas == null)
      this.m_Formulas = new Dictionary<IConditionalFormat, string>();
    if (!this.m_Formulas.TryGetValue(format, out formatFirstFormula))
    {
      formatFirstFormula = format.FirstFormula;
      this.m_Formulas.Add(format, formatFirstFormula);
    }
    if (format.FormatType == ExcelCFType.Formula)
      extendedFormatImpl = this.CheckAndApplyConditionFormula(format, cell, xf, formatFirstFormula);
    return extendedFormatImpl;
  }

  private ExtendedFormatImpl CheckAndApplyConditionFormula(
    IConditionalFormat format,
    IRange cell,
    ExtendedFormatImpl xf,
    string formatFirstFormula)
  {
    ExtendedFormatImpl extendedFormatImpl = (ExtendedFormatImpl) null;
    if (!(cell.Worksheet.Workbook as WorkbookImpl).EnabledCalcEngine || (cell.Worksheet as WorksheetImpl).CalcEngine == null)
    {
      this.m_calculationEnabled = true;
      cell.Worksheet.EnableSheetCalculations();
      (cell.Worksheet.Workbook as WorkbookImpl).CalcEngineMemberValuesOnSheet(false);
    }
    bool bResult = false;
    string str1 = this.UpdateConditionFormula(format, cell, xf, formatFirstFormula, ref bResult);
    if (!bResult)
    {
      string str2;
      if (this.CalcValues.ContainsKey(str1))
      {
        str2 = this.CalcValues[str1];
      }
      else
      {
        bool flag = false;
        string name = (string) null;
        if (format.FormatType == ExcelCFType.SpecificText)
        {
          if (str1.Trim().StartsWith("RIGHT"))
          {
            string[] strArray = str1.Trim().Split('=', '>', '+', '-', '*', '/', '<', ',', '(', ')', '.', '!', (cell.Worksheet.Workbook as WorkbookImpl).Application.ArgumentsSeparator);
            if (strArray.Length == 7)
            {
              name = strArray[strArray.Length - 1];
              flag = char.IsLetter(name[0]) && char.IsNumber(name[name.Length - 1]);
            }
            if (flag && cell.Worksheet[name].IsBlank)
            {
              str2 = "TRUE";
            }
            else
            {
              str2 = cell.Worksheet.CalcEngine.ParseAndComputeFormula(str1);
              this.CalcValues.Add(str1, str2);
            }
          }
          else if (str1.Trim().StartsWith("LEFT"))
          {
            string[] strArray = str1.Trim().Split('=', '>', '+', '-', '*', '/', '<', ',', '(', ')', '.', '!', (cell.Worksheet.Workbook as WorkbookImpl).Application.ArgumentsSeparator);
            if (strArray.Length == 7)
            {
              name = strArray[strArray.Length - 1];
              flag = char.IsLetter(name[0]) && char.IsNumber(name[name.Length - 1]);
            }
            if (flag && cell.Worksheet[name].IsBlank)
            {
              str2 = "TRUE";
            }
            else
            {
              str2 = cell.Worksheet.CalcEngine.ParseAndComputeFormula(str1);
              this.CalcValues.Add(str1, str2);
            }
          }
          else
          {
            str2 = cell.Worksheet.CalcEngine.ParseAndComputeFormula(str1);
            this.CalcValues.Add(str1, str2);
          }
        }
        else
        {
          str2 = cell.Worksheet.CalcEngine.ParseAndComputeFormula(str1);
          this.CalcValues.Add(str1, str2);
        }
      }
      if (str2 == "TRUE")
        extendedFormatImpl = this.ApplyCondition(format, xf);
    }
    return extendedFormatImpl;
  }

  internal string UpdateConditionFormula(
    IConditionalFormat format,
    IRange cell,
    ExtendedFormatImpl xf,
    string formatFirstFormula,
    ref bool bResult)
  {
    cell.Worksheet.CalcEngine.RethrowParseExceptions = false;
    string str1 = formatFirstFormula;
    bool flag = false;
    foreach (Ptg firstFormulaPtg in (format as IInternalConditionalFormat).FirstFormulaPtgs)
    {
      if (firstFormulaPtg.TokenCode == FormulaToken.tArea1 && !firstFormulaPtg.ToString().StartsWith("$"))
        flag = true;
    }
    if (!str1.Contains('$'.ToString()) || flag)
    {
      ConditionalFormatImpl conditionalFormatImpl = (ConditionalFormatImpl) null;
      if (format is ConditionalFormatWrapper)
        conditionalFormatImpl = (format as ConditionalFormatWrapper).GetCondition();
      else if (format != null)
        conditionalFormatImpl = format as ConditionalFormatImpl;
      if (conditionalFormatImpl != null)
      {
        ConditionalFormats parent = (ConditionalFormats) conditionalFormatImpl.Parent;
        string[] strArray = parent.Address.Split(':');
        if (parent.CellsList.Length <= 1 && strArray.Length <= 2 || flag)
        {
          WorkbookImpl workbook = cell.Worksheet.Workbook as WorkbookImpl;
          WorksheetImpl worksheet = cell.Worksheet as WorksheetImpl;
          bool throwOnUnknownNames = workbook.ThrowOnUnknownNames;
          workbook.ThrowOnUnknownNames = false;
          Ptg[] ptgArray = workbook.FormulaUtil.ParseString(format.FirstFormulaR1C1, (IWorksheet) worksheet, (Dictionary<string, string>) null, cell.Row - 1, cell.Column - 1, true);
          workbook.ThrowOnUnknownNames = throwOnUnknownNames;
          bResult = this.HasErrorPtg(ptgArray);
          str1 = worksheet.GetFormula(cell.Row - 1, cell.Column - 1, ptgArray, false, workbook.FormulaUtil, false);
          if (str1[0].ToString() == "=")
            str1 = str1.Substring(1, str1.Length - 1);
          flag = true;
        }
      }
    }
    WorksheetImpl worksheet1 = cell.Worksheet as WorksheetImpl;
    if (formatFirstFormula.Equals(str1) && !flag)
    {
      str1 = str1.Trim(' ');
      string str2 = str1;
      char[] chArray1 = new char[13]
      {
        '=',
        '>',
        '+',
        '-',
        '*',
        '/',
        '<',
        ',',
        '(',
        ')',
        '.',
        '!',
        (cell.Worksheet.Workbook as WorkbookImpl).Application.ArgumentsSeparator
      };
      foreach (string str3 in str2.Split(chArray1))
      {
        char[] chArray2 = new char[1]{ ' ' };
        string str4 = str3.Trim(chArray2);
        if (!str4.Equals(string.Empty))
        {
          if (!str4.Contains('$'.ToString()) && char.IsLetter(str4[0]) && char.IsNumber(str4[str4.Length - 1]) && !Enum.IsDefined(typeof (ExcelFunction), (object) str4))
          {
            Rectangle rectangleFromRangeString = worksheet1.GetRectangleFromRangeString(str4);
            if (this.m_firstRange != this.m_range && cell.Row == this.m_range.Top && cell.Column == this.m_range.Left)
            {
              this.m_columnDifference = cell.Column - this.m_firstRange.Left;
              this.m_rowDifference = cell.Row - this.m_firstRange.Top;
            }
            Rectangle rectangle = new Rectangle(rectangleFromRangeString.Left + (cell.Column - this.m_range.Left) + this.m_columnDifference, rectangleFromRangeString.Top + (cell.Row - this.m_range.Top) + this.m_rowDifference + this.m_firstRowDifference, rectangleFromRangeString.Right - rectangleFromRangeString.Left, rectangleFromRangeString.Bottom - rectangleFromRangeString.Top);
            str1 = str1.Replace(str4, RangeImpl.GetAddressLocal(rectangle.Top, rectangle.Left, rectangle.Bottom, rectangle.Right));
          }
          else if (str4.Contains(":") && !char.IsNumber(str4[str4.Length - 1]) && !str4.StartsWith("\""))
          {
            string newValue = str4.Substring(0, str4.IndexOf(':')) + "1";
            str1 = str1.Replace(str4, newValue);
          }
          else if (str4.Contains('$'.ToString()) && (char.IsLetter(str4[0]) || char.IsLetter(str4[1])))
          {
            Rectangle rectangleFromRangeString = worksheet1.GetRectangleFromRangeString(str4);
            Rectangle rectangle = !str4.StartsWith('$'.ToString()) ? new Rectangle(cell.Column, rectangleFromRangeString.Bottom, 0, 0) : (str4.Substring(1, str4.Length - 1).Contains('$'.ToString()) ? rectangleFromRangeString : new Rectangle(rectangleFromRangeString.Left, cell.Row, 0, 0));
            str1 = str1.Replace(str4, RangeImpl.GetAddressLocal(rectangle.Top, rectangle.Left, rectangle.Bottom, rectangle.Right));
          }
        }
      }
    }
    if (str1.ToUpper().Contains("ROW()"))
      str1 = str1.ToUpper().Replace("ROW()", cell.Row.ToString());
    if (str1.ToUpper().Contains("COLUMN()"))
      str1 = str1.ToUpper().Replace("COLUMN()", cell.Column.ToString());
    return str1;
  }

  private bool HasErrorPtg(Ptg[] Ptgs)
  {
    foreach (Ptg ptg in Ptgs)
    {
      switch (ptg)
      {
        case ErrorPtg _:
        case AreaErrorPtg _:
        case AreaError3DPtg _:
        case RefErrorPtg _:
        case RefError3dPtg _:
          return true;
        default:
          continue;
      }
    }
    return false;
  }

  private ExtendedFormatImpl CheckAndApplySpecificText(
    IConditionalFormat format,
    IRange cell,
    ExtendedFormatImpl xf)
  {
    bool flag1 = false;
    IWorksheet worksheet = cell.Worksheet;
    ConditionalFormatImpl conditionalFormatImpl = format as ConditionalFormatImpl;
    string str = string.Empty;
    ExtendedFormatImpl extendedFormatImpl = (ExtendedFormatImpl) null;
    if (string.IsNullOrEmpty(format.Text))
    {
      if (conditionalFormatImpl.Range != null)
      {
        object textFromCellType = (conditionalFormatImpl.Range.Worksheet as WorksheetImpl).GetTextFromCellType(conditionalFormatImpl.Range.Row, conditionalFormatImpl.Range.Column, out WorksheetImpl.TRangeValueType _);
        str = textFromCellType == null ? string.Empty : textFromCellType.ToString();
      }
      else if (!string.IsNullOrEmpty(format.FirstFormula))
      {
        extendedFormatImpl = this.CheckAndApplyConditionFormula(format, cell, xf, format.FirstFormula);
        flag1 = true;
      }
    }
    else
      str = format.Text;
    bool flag2 = false;
    if (!flag1)
    {
      switch (format.Operator)
      {
        case ExcelComparisonOperator.BeginsWith:
          flag2 = this.CheckBeginsWith(cell, str);
          break;
        case ExcelComparisonOperator.ContainsText:
          flag2 = this.CheckContains(cell, (object) str);
          break;
        case ExcelComparisonOperator.EndsWith:
          flag2 = this.CheckEndsWith(cell, str);
          break;
        case ExcelComparisonOperator.NotContainsText:
          flag2 = this.CheckNotContains(cell, str);
          break;
      }
    }
    if (flag2)
      extendedFormatImpl = this.ApplyCondition(format, xf);
    return extendedFormatImpl;
  }

  private bool CheckContains(IRange cell, object value)
  {
    object textFromCellType = (cell.Worksheet as WorksheetImpl).GetTextFromCellType(cell.Row, cell.Column, out WorksheetImpl.TRangeValueType _);
    return textFromCellType != null && value != null && textFromCellType.ToString().IndexOf(value.ToString(), StringComparison.OrdinalIgnoreCase) >= 0;
  }

  private bool CheckNotContains(IRange cell, string value)
  {
    object textFromCellType = (cell.Worksheet as WorksheetImpl).GetTextFromCellType(cell.Row, cell.Column, out WorksheetImpl.TRangeValueType _);
    return textFromCellType != null && value != null && textFromCellType.ToString().IndexOf(value.ToString(), StringComparison.OrdinalIgnoreCase) < 0;
  }

  private bool CheckBeginsWith(IRange cell, string value)
  {
    object textFromCellType = (cell.Worksheet as WorksheetImpl).GetTextFromCellType(cell.Row, cell.Column, out WorksheetImpl.TRangeValueType _);
    return textFromCellType != null && textFromCellType.ToString().StartsWith(value, StringComparison.OrdinalIgnoreCase);
  }

  private bool CheckEndsWith(IRange cell, string value)
  {
    object textFromCellType = (cell.Worksheet as WorksheetImpl).GetTextFromCellType(cell.Row, cell.Column, out WorksheetImpl.TRangeValueType _);
    return textFromCellType != null && textFromCellType.ToString().EndsWith(value, StringComparison.OrdinalIgnoreCase);
  }

  private ExtendedFormatImpl ConditionalFormatTopBottom(
    IConditionalFormat format,
    IRange cell,
    ExtendedFormatImpl xf)
  {
    ExtendedFormatImpl extendedFormatImpl = (ExtendedFormatImpl) null;
    this.GetCellValues(format, cell);
    this.ExtractTop10Cells(format, cell);
    List<long> longList;
    if (format != null && this.m_top10Cells.TryGetValue(format.TopBottom as TopBottomImpl, out longList) && longList.Contains(RangeImpl.GetCellIndex(cell.Column, cell.Row)))
    {
      format = format is ConditionalFormatImpl ? (IConditionalFormat) (format as ConditionalFormatImpl) : (IConditionalFormat) (format as ConditionalFormatWrapper).GetCondition();
      extendedFormatImpl = this.ApplyCondition(format, xf);
    }
    return extendedFormatImpl;
  }

  private ExtendedFormatImpl ConditionalFormatAboveAverage(
    IConditionalFormat format,
    IRange cell,
    ExtendedFormatImpl xf)
  {
    ExtendedFormatImpl extendedFormatImpl = (ExtendedFormatImpl) null;
    this.GetCellValues(format, cell);
    if (this.m_aboveAverageValues == null)
    {
      this.m_aboveAverageValues = new Dictionary<IConditionalFormats, List<double>>();
      List<double> doubleList = new List<double>();
      double average = this.GetAverage(new List<double>((IEnumerable<double>) this.m_cellValues.Values).ToArray());
      double standardDeviation = this.GetStandardDeviation(new List<double>((IEnumerable<double>) this.m_cellValues.Values).ToArray());
      doubleList.Add(average);
      doubleList.Add(standardDeviation);
      this.m_aboveAverageValues.Add((IConditionalFormats) (format.Parent as ConditionalFormats), doubleList);
    }
    double num1;
    if (format != null && this.m_cellValues.TryGetValue(RangeImpl.GetCellIndex(cell.Column, cell.Row), out num1))
    {
      List<double> doubleList;
      this.m_aboveAverageValues.TryGetValue((IConditionalFormats) (format.Parent as ConditionalFormats), out doubleList);
      double num2 = doubleList[0];
      bool flag;
      if (format.AboveBelowAverage.AverageType.ToString().Contains("StdDev"))
      {
        double num3 = doubleList[1];
        flag = !format.AboveBelowAverage.AverageType.ToString().Contains("Below") ? num1 > num2 + (double) format.AboveBelowAverage.StdDevValue * num3 : num1 < num2 - (double) format.AboveBelowAverage.StdDevValue * num3;
      }
      else
        flag = !format.AboveBelowAverage.AverageType.ToString().Contains("Equal") ? (!format.AboveBelowAverage.AverageType.ToString().Contains("Below") ? num1 > num2 : num1 < num2) : (!format.AboveBelowAverage.AverageType.ToString().Contains("Below") ? num1 >= num2 : num1 <= num2);
      if (flag)
      {
        format = format is ConditionalFormatImpl ? (IConditionalFormat) (format as ConditionalFormatImpl) : (IConditionalFormat) (format as ConditionalFormatWrapper).GetCondition();
        extendedFormatImpl = this.ApplyCondition(format, xf);
      }
    }
    return extendedFormatImpl;
  }

  private void GetCellValues(IConditionalFormat format, IRange cell)
  {
    if (this.m_cellValues != null)
      return;
    this.m_cellValues = new Dictionary<long, double>();
    ConditionalFormats parent = format.Parent as ConditionalFormats;
    WorksheetImpl sheet = parent.sheet;
    bool flag = false;
    if (sheet.CalcEngine == null)
    {
      cell.Worksheet.EnableSheetCalculations();
      flag = true;
    }
    int num1 = 0;
    for (int index1 = 0; index1 < parent.CellsRectangleList.Count; ++index1)
    {
      Rectangle cellsRectangle = parent.CellsRectangleList[index1];
      int num2 = cellsRectangle.Y + 1;
      int num3 = cellsRectangle.X + 1;
      int num4 = cellsRectangle.Bottom + 1;
      int num5 = cellsRectangle.Right + 1;
      IMigrantRange migrantRange = sheet.MigrantRange;
      for (int index2 = num2; index2 <= num4; ++index2)
      {
        for (int index3 = num3; index3 <= num5; ++index3)
        {
          migrantRange.ResetRowColumn(index2, index3);
          string calculatedValue = migrantRange.CalculatedValue;
          ++num1;
          double result;
          if (double.TryParse(calculatedValue, out result) || sheet.GetCellType(index2, index3, false) == WorksheetImpl.TRangeValueType.Number)
          {
            result = sheet.GetCellType(index2, index3, false) == WorksheetImpl.TRangeValueType.Number ? sheet.GetNumber(index2, index3) : result;
            this.m_cellValues.Add(RangeImpl.GetCellIndex(index3, index2), result);
          }
        }
      }
    }
    if (!flag)
      return;
    sheet.DisableSheetCalculations();
  }

  private void ExtractTop10Cells(IConditionalFormat format, IRange cell)
  {
    bool flag = this.m_top10Cells == null;
    TopBottomImpl topBottom = format.TopBottom as TopBottomImpl;
    if (!flag)
    {
      flag = true;
      foreach (KeyValuePair<TopBottomImpl, List<long>> top10Cell in this.m_top10Cells)
      {
        if (top10Cell.Key == topBottom)
        {
          flag = false;
          break;
        }
      }
    }
    if (!flag)
      return;
    if (this.m_top10Cells == null)
      this.m_top10Cells = new Dictionary<TopBottomImpl, List<long>>();
    List<long> longList1 = new List<long>();
    List<long> longList2 = new List<long>((IEnumerable<long>) this.m_cellValues.Keys);
    List<double> doubleList = new List<double>((IEnumerable<double>) this.m_cellValues.Values);
    for (int index1 = 0; index1 < doubleList.Count; ++index1)
    {
      for (int index2 = index1 + 1; index2 < doubleList.Count; ++index2)
      {
        if ((format.TopBottom.Type == ExcelCFTopBottomType.Top ? (doubleList[index1] < doubleList[index2] ? 1 : 0) : (doubleList[index1] > doubleList[index2] ? 1 : 0)) != 0)
        {
          double num1 = doubleList[index1];
          doubleList[index1] = doubleList[index2];
          doubleList[index2] = num1;
          long num2 = longList2[index1];
          longList2[index1] = longList2[index2];
          longList2[index2] = num2;
        }
      }
    }
    double num = !format.TopBottom.Percent ? (double) format.TopBottom.Rank : (double) (int) ((double) this.m_cellValues.Count * ((double) format.TopBottom.Rank / 100.0));
    if (num == 0.0)
      num = 1.0;
    for (int index = 0; index < longList2.Count && ((double) index < num || doubleList[index - 1] == doubleList[index]); ++index)
      longList1.Add(longList2[index]);
    if (longList1.Count <= 0 || this.m_top10Cells.ContainsKey(format.TopBottom as TopBottomImpl))
      return;
    this.m_top10Cells.Add(topBottom, longList1);
  }

  private double GetStandardDeviation(double[] values)
  {
    return Math.Sqrt(this.GetVariance(values, this.GetAverage(values)));
  }

  private double GetAverage(double[] values)
  {
    double num = 0.0;
    for (int index = 0; index < values.Length; ++index)
      num += values[index];
    return num / (double) values.Length;
  }

  private double GetVariance(double[] values, double mean)
  {
    double num1 = 0.0;
    for (int index = 0; index < values.Length; ++index)
    {
      double num2 = values[index] - mean;
      num1 += num2 * num2;
    }
    return num1 / (double) values.Length;
  }

  private ExtendedFormatImpl ApplyCondition(IConditionalFormat condition, ExtendedFormatImpl xf)
  {
    ExtendedFormatImpl xf1 = (ExtendedFormatImpl) new ExtendedFormatStandAlone(xf);
    this.UpdateFill(condition, xf1);
    this.UpdateFont(condition, xf1);
    this.UpdateBorders(condition, xf1);
    this.UpdateNumberFormat(condition, xf1);
    return xf1;
  }

  private ExtendedFormatStandAlone ApplyCondition(
    string iconName,
    ExcelIconSetType iconSet,
    int iconId,
    ExtendedFormatImpl xf,
    bool showIcon)
  {
    ExtendedFormatStandAlone formatStandAlone = new ExtendedFormatStandAlone(xf);
    if ((this.m_cfApplied & 524288U /*0x080000*/) >> 19 != 1U)
    {
      formatStandAlone.ShowIconOnly = showIcon;
      formatStandAlone.IconId = iconId;
      formatStandAlone.IconSet = iconSet;
      formatStandAlone.IconName = iconName;
      formatStandAlone.HasIconSet = true;
      this.m_cfApplied |= 524288U /*0x080000*/;
    }
    return formatStandAlone;
  }

  private void UpdateFont(IConditionalFormat condition, ExtendedFormatImpl xf)
  {
    xf.Font.BeginUpdate();
    this.UpdateFontFormat(condition, xf);
    this.UpdateFontColor(condition, xf);
    xf.Font.EndUpdate();
  }

  private void UpdateNumberFormat(IConditionalFormat condition, ExtendedFormatImpl xf)
  {
    if ((!(condition is ConditionalFormatImpl) ? ((condition as ConditionalFormatWrapper).HasNumberFormatPresent ? 1 : 0) : (!(condition as ConditionalFormatImpl).HasNumberFormatPresent ? 0 : ((this.m_cfApplied & 262144U /*0x040000*/) >> 18 != 1U ? 1 : 0))) != 0)
    {
      xf.NumberFormat = condition.NumberFormat;
      this.m_cfApplied |= 262144U /*0x040000*/;
    }
    else
    {
      if ((!(condition is ConditionalFormatImpl) ? ((condition as ConditionalFormatWrapper).HasNumberFormatPresent ? 1 : 0) : ((condition as ConditionalFormatImpl).HasNumberFormatPresent ? 1 : 0)) != 0 || !condition.StopIfTrue)
        return;
      this.m_cfApplied |= 262144U /*0x040000*/;
    }
  }

  private void UpdateFill(IConditionalFormat condition, ExtendedFormatImpl xf)
  {
    this.UpdatePatternFormat(condition, xf);
    this.UpdatePatternColor(condition, xf);
    this.UpdateBackgroundColor(condition, xf);
  }

  private void UpdatePatternColor(IConditionalFormat condition, ExtendedFormatImpl xf)
  {
    if (condition.IsPatternColorPresent && (this.m_cfApplied & 2U) >> 1 != 1U)
    {
      xf.PatternColor = condition.ColorRGB;
      this.m_cfApplied |= 2U;
    }
    else
    {
      if (condition.IsPatternColorPresent || !condition.StopIfTrue)
        return;
      this.m_cfApplied |= 2U;
    }
  }

  private void UpdateBackgroundColor(IConditionalFormat condition, ExtendedFormatImpl xf)
  {
    if (condition.IsBackgroundColorPresent && (this.m_cfApplied & 4U) >> 2 != 1U)
    {
      xf.Color = condition.BackColorRGB;
      this.m_cfApplied |= 4U;
    }
    else
    {
      if (condition.IsBackgroundColorPresent || !condition.StopIfTrue)
        return;
      this.m_cfApplied |= 4U;
    }
  }

  private void UpdateFontColor(IConditionalFormat condition, ExtendedFormatImpl xf)
  {
    if (condition.IsFontColorPresent && (this.m_cfApplied & 512U /*0x0200*/) >> 9 != 1U)
    {
      xf.Font.RGBColor = condition.FontColorRGB;
      this.m_cfApplied |= 512U /*0x0200*/;
    }
    else
    {
      if (condition.IsFontColorPresent || !condition.StopIfTrue)
        return;
      this.m_cfApplied |= 512U /*0x0200*/;
    }
  }

  private void UpdatePatternFormat(IConditionalFormat condition, ExtendedFormatImpl xf)
  {
    if (condition.IsPatternFormatPresent && ((int) this.m_cfApplied & 1) != 1)
    {
      xf.FillPattern = condition.FillPattern;
      this.m_cfApplied |= 1U;
    }
    else
    {
      if (condition.IsPatternFormatPresent || !condition.StopIfTrue)
        return;
      this.m_cfApplied |= 1U;
    }
  }

  private void UpdateFontFormat(IConditionalFormat condition, ExtendedFormatImpl xf)
  {
    if (condition.IsFontFormatPresent)
    {
      IFont font = xf.Font;
      if ((this.m_cfApplied & 8U) >> 3 != 1U)
      {
        font.Bold = condition.IsBold;
        this.m_cfApplied |= 8U;
      }
      if ((this.m_cfApplied & 16U /*0x10*/) >> 4 != 1U)
      {
        font.Italic = condition.IsItalic;
        this.m_cfApplied |= 16U /*0x10*/;
      }
      if ((this.m_cfApplied & 32U /*0x20*/) >> 5 != 1U)
      {
        font.Strikethrough = condition.IsStrikeThrough;
        this.m_cfApplied |= 32U /*0x20*/;
      }
      if ((this.m_cfApplied & 64U /*0x40*/) >> 6 != 1U)
      {
        font.Subscript = condition.IsSubScript;
        this.m_cfApplied |= 64U /*0x40*/;
      }
      if ((this.m_cfApplied & 128U /*0x80*/) >> 7 != 1U)
      {
        font.Superscript = condition.IsSuperScript;
        this.m_cfApplied |= 128U /*0x80*/;
      }
      if ((this.m_cfApplied & 256U /*0x0100*/) >> 8 == 1U)
        return;
      font.Underline = condition.Underline;
      this.m_cfApplied |= 256U /*0x0100*/;
    }
    else
    {
      if (!condition.StopIfTrue)
        return;
      this.m_cfApplied |= 504U;
    }
  }

  private void UpdateBorders(IConditionalFormat condition, ExtendedFormatImpl xf)
  {
    if (condition.IsBorderFormatPresent)
    {
      if (condition.IsLeftBorderModified)
      {
        if ((this.m_cfApplied & 1024U /*0x0400*/) >> 10 != 1U)
        {
          xf.LeftBorderColor.SetRGB(condition.LeftBorderColorRGB);
          this.m_cfApplied |= 1024U /*0x0400*/;
        }
        if ((this.m_cfApplied & 2048U /*0x0800*/) >> 11 != 1U)
        {
          xf.LeftBorderLineStyle = condition.LeftBorderStyle;
          this.m_cfApplied |= 2048U /*0x0800*/;
        }
      }
      else if (condition.StopIfTrue)
        this.m_cfApplied |= 3072U /*0x0C00*/;
      if (condition.IsRightBorderModified)
      {
        if ((this.m_cfApplied & 4096U /*0x1000*/) >> 12 != 1U)
        {
          xf.RightBorderColor.SetRGB(condition.RightBorderColorRGB);
          this.m_cfApplied |= 4096U /*0x1000*/;
        }
        if ((this.m_cfApplied & 8192U /*0x2000*/) >> 13 != 1U)
        {
          xf.RightBorderLineStyle = condition.RightBorderStyle;
          this.m_cfApplied |= 8192U /*0x2000*/;
        }
      }
      else if (condition.StopIfTrue)
        this.m_cfApplied |= 12288U /*0x3000*/;
      if (condition.IsTopBorderModified)
      {
        if ((this.m_cfApplied & 16384U /*0x4000*/) >> 14 != 1U)
        {
          xf.TopBorderColor.SetRGB(condition.TopBorderColorRGB);
          this.m_cfApplied |= 16384U /*0x4000*/;
        }
        if ((this.m_cfApplied & 32768U /*0x8000*/) >> 15 != 1U)
        {
          xf.TopBorderLineStyle = condition.TopBorderStyle;
          this.m_cfApplied |= 32768U /*0x8000*/;
        }
      }
      else if (condition.StopIfTrue)
        this.m_cfApplied |= 49152U /*0xC000*/;
      if (condition.IsBottomBorderModified)
      {
        if ((this.m_cfApplied & 65536U /*0x010000*/) >> 16 /*0x10*/ != 1U)
        {
          xf.BottomBorderColor.SetRGB(condition.BottomBorderColorRGB);
          this.m_cfApplied |= 65536U /*0x010000*/;
        }
        if ((this.m_cfApplied & 131072U /*0x020000*/) >> 17 == 1U)
          return;
        xf.BottomBorderLineStyle = condition.BottomBorderStyle;
        this.m_cfApplied |= 131072U /*0x020000*/;
      }
      else
      {
        if (!condition.StopIfTrue)
          return;
        this.m_cfApplied |= 196608U /*0x030000*/;
      }
    }
    else
    {
      if (!condition.StopIfTrue)
        return;
      this.m_cfApplied |= 261120U;
    }
  }

  internal ExtendedFormatStandAlone ApplyIconSet(
    IRange cell,
    IIconSet iconSet,
    string formatRange,
    ExtendedFormatImpl xf)
  {
    IconSetImpl iconSetImpl = iconSet as IconSetImpl;
    IConditionValue iconCriterion1 = iconSetImpl.IconCriteria[1];
    IConditionValue iconCriterion2 = iconSetImpl.IconCriteria[2];
    bool isGreaterThan1 = false;
    bool isGreaterThan2 = false;
    bool isGreaterThan3 = false;
    bool isGreaterThan4 = false;
    double num1 = 0.0;
    double num2 = 0.0;
    bool reverseOrder = iconSetImpl.ReverseOrder;
    ExcelIconSetType iconSet1 = iconSetImpl.IconSet;
    int iconId = -1;
    string iconName = (string) null;
    if (iconSetImpl.IconCriteria.Count == 5)
    {
      num1 = this.GetIconSetConditionValue(cell, iconSetImpl.IconCriteria[3], formatRange, out isGreaterThan3);
      num2 = this.GetIconSetConditionValue(cell, iconSetImpl.IconCriteria[4], formatRange, out isGreaterThan4);
    }
    else if (iconSetImpl.IconCriteria.Count == 4)
      num1 = this.GetIconSetConditionValue(cell, iconSetImpl.IconCriteria[3], formatRange, out isGreaterThan3);
    double setConditionValue1 = this.GetIconSetConditionValue(cell, iconCriterion1, formatRange, out isGreaterThan1);
    double setConditionValue2 = this.GetIconSetConditionValue(cell, iconCriterion2, formatRange, out isGreaterThan2);
    switch (iconSetImpl.IsCustom ? -1 : (int) iconSetImpl.IconSet)
    {
      case 0:
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue2) && !isGreaterThan2 || this.CheckGreater(cell, (object) setConditionValue2) && isGreaterThan2 || this.CheckCriteria(iconSetImpl.IconCriteria[2]))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_GreenUpArrow.png";
            iconId = 2;
            break;
          }
          iconName = "CF_IS_RedDownArrow.png";
          iconId = 0;
          break;
        }
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue1) && !isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckGreater(cell, (object) setConditionValue1) && isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckCriteria(iconSetImpl.IconCriteria[1]))
        {
          iconName = "CF_IS_YellowSideArrow.png";
          iconId = 1;
          break;
        }
        if (this.CheckLess(cell, (object) setConditionValue1) && !isGreaterThan1 || this.CheckLessOrEqual(cell, (object) setConditionValue1) && isGreaterThan1)
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_RedDownArrow.png";
            iconId = 0;
            break;
          }
          iconName = "CF_IS_GreenUpArrow.png";
          iconId = 2;
          break;
        }
        break;
      case 1:
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue2) && !isGreaterThan2 || this.CheckGreater(cell, (object) setConditionValue2) && isGreaterThan2 || this.CheckCriteria(iconSetImpl.IconCriteria[2]))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_GrayUpArrow.png";
            iconId = 2;
            break;
          }
          iconName = "CF_IS_GrayDownArrow.png";
          iconId = 0;
          break;
        }
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue1) && !isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckGreater(cell, (object) setConditionValue1) && isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckCriteria(iconSetImpl.IconCriteria[1]))
        {
          iconName = "CF_IS_GraySideArrow.png";
          iconId = 1;
          break;
        }
        if (this.CheckLess(cell, (object) setConditionValue1) && !isGreaterThan1 || this.CheckLessOrEqual(cell, (object) setConditionValue1) && isGreaterThan1)
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_GrayDownArrow.png";
            iconId = 0;
            break;
          }
          iconName = "CF_IS_GrayUpArrow.png";
          iconId = 2;
          break;
        }
        break;
      case 2:
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue2) && !isGreaterThan2 || this.CheckGreater(cell, (object) setConditionValue2) && isGreaterThan2 || this.CheckCriteria(iconSetImpl.IconCriteria[2]))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_GreenFlag.png";
            iconId = 2;
            break;
          }
          iconName = "CF_IS_RedFlag.png";
          iconId = 0;
          break;
        }
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue1) && !isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckGreater(cell, (object) setConditionValue1) && isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckCriteria(iconSetImpl.IconCriteria[1]))
        {
          iconName = "CF_IS_YellowFlag.png";
          iconId = 1;
          break;
        }
        if (this.CheckLess(cell, (object) setConditionValue1) && !isGreaterThan1 || this.CheckLessOrEqual(cell, (object) setConditionValue1) && isGreaterThan1)
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_RedFlag.png";
            iconId = 0;
            break;
          }
          iconName = "CF_IS_GreenFlag.png";
          iconId = 2;
          break;
        }
        break;
      case 3:
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue2) && !isGreaterThan2 || this.CheckGreater(cell, (object) setConditionValue2) && isGreaterThan2 || this.CheckCriteria(iconSetImpl.IconCriteria[2]))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_GreenCircle.png";
            iconId = 2;
            break;
          }
          iconName = "CF_IS_RedCircleWithBorder.png";
          iconId = 0;
          break;
        }
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue1) && !isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckGreater(cell, (object) setConditionValue1) && isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckCriteria(iconSetImpl.IconCriteria[1]))
        {
          iconName = "CF_IS_YellowCircle.png";
          iconId = 1;
          break;
        }
        if (this.CheckLess(cell, (object) setConditionValue1) && !isGreaterThan1 || this.CheckLessOrEqual(cell, (object) setConditionValue1) && isGreaterThan1)
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_RedCircleWithBorder.png";
            iconId = 0;
            break;
          }
          iconName = "CF_IS_GreenCircle.png";
          iconId = 2;
          break;
        }
        break;
      case 4:
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue2) && !isGreaterThan2 || this.CheckGreater(cell, (object) setConditionValue2) && isGreaterThan2 || this.CheckCriteria(iconSetImpl.IconCriteria[2]))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_GreenTrafficLight.png";
            iconId = 2;
            break;
          }
          iconName = "CF_IS_RedTrafficLight.png";
          iconId = 0;
          break;
        }
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue1) && !isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckGreater(cell, (object) setConditionValue1) && isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckCriteria(iconSetImpl.IconCriteria[1]))
        {
          iconName = "CF_IS_YellowTrafficLight.png";
          iconId = 1;
          break;
        }
        if (this.CheckLess(cell, (object) setConditionValue1) && !isGreaterThan1 || this.CheckLessOrEqual(cell, (object) setConditionValue1) && isGreaterThan1)
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_RedTrafficLight.png";
            iconId = 0;
            break;
          }
          iconName = "CF_IS_GreenTrafficLight.png";
          iconId = 2;
          break;
        }
        break;
      case 5:
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue2) && !isGreaterThan2 || this.CheckGreater(cell, (object) setConditionValue2) && isGreaterThan2 || this.CheckCriteria(iconSetImpl.IconCriteria[2]))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_GreenCircle.png";
            iconId = 2;
            break;
          }
          iconName = "CF_IS_RedDiamond.png";
          iconId = 0;
          break;
        }
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue1) && !isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckGreater(cell, (object) setConditionValue1) && isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckCriteria(iconSetImpl.IconCriteria[1]))
        {
          iconName = "CF_IS_YellowTriangle.png";
          iconId = 1;
          break;
        }
        if (this.CheckLess(cell, (object) setConditionValue1) && !isGreaterThan1 || this.CheckLessOrEqual(cell, (object) setConditionValue1) && isGreaterThan1)
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_RedDiamond.png";
            iconId = 0;
            break;
          }
          iconName = "CF_IS_GreenCircle.png";
          iconId = 2;
          break;
        }
        break;
      case 6:
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue2) && !isGreaterThan2 || this.CheckGreater(cell, (object) setConditionValue2) && isGreaterThan2 || this.CheckCriteria(iconSetImpl.IconCriteria[2]) && this.CheckGreaterOrEqual(cell, (object) 0.0))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_GreenCheckSymbol.png";
            iconId = 2;
            break;
          }
          iconName = "CF_IS_RedCrossSymbol.png";
          iconId = 0;
          break;
        }
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue1) && !isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckGreater(cell, (object) setConditionValue1) && isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckCriteria(iconSetImpl.IconCriteria[1]) && this.CheckGreaterOrEqual(cell, (object) 0.0))
        {
          iconName = "CF_IS_YellowExclamationSymbol.png";
          iconId = 1;
          break;
        }
        if ((!this.CheckCriteria(iconSetImpl.IconCriteria[0]) ? (!this.CheckLess(cell, (object) setConditionValue1) || isGreaterThan1 ? (!this.CheckLessOrEqual(cell, (object) setConditionValue1) ? 0 : (isGreaterThan1 ? 1 : 0)) : 1) : (this.CheckGreaterOrEqual(cell, (object) 0.0) ? 1 : 0)) != 0)
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_RedCrossSymbol.png";
            iconId = 0;
            break;
          }
          iconName = "CF_IS_GreenCheckSymbol.png";
          iconId = 2;
          break;
        }
        if (this.CheckLess(cell, (object) 0.0))
          return new ExtendedFormatStandAlone(xf)
          {
            ShowIconOnly = iconSetImpl.ShowIconOnly
          };
        break;
      case 7:
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue2) && !isGreaterThan2 || this.CheckGreater(cell, (object) setConditionValue2) && isGreaterThan2 || this.CheckCriteria(iconSetImpl.IconCriteria[2]))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_GreenCheck.png";
            iconId = 2;
            break;
          }
          iconName = "CF_IS_RedCross.png";
          iconId = 0;
          break;
        }
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue1) && !isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckGreater(cell, (object) setConditionValue1) && isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckCriteria(iconSetImpl.IconCriteria[1]))
        {
          iconName = "CF_IS_YellowExclamation.png";
          iconId = 1;
          break;
        }
        if (this.CheckLess(cell, (object) setConditionValue1) && !isGreaterThan1 || this.CheckLessOrEqual(cell, (object) setConditionValue1) && isGreaterThan1)
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_RedCross.png";
            iconId = 0;
            break;
          }
          iconName = "CF_IS_GreenCheck.png";
          iconId = 2;
          break;
        }
        break;
      case 8:
        if (this.CheckGreaterOrEqual(cell, (object) num1) && !isGreaterThan3 || this.CheckGreater(cell, (object) num1) && isGreaterThan3 || this.CheckCriteria(iconSetImpl.IconCriteria[3]))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_GreenUpArrow.png";
            iconId = 3;
            break;
          }
          iconName = "CF_IS_RedDownArrow.png";
          iconId = 0;
          break;
        }
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue2) && !isGreaterThan2 && this.CompareValues(isGreaterThan3, cell, num1) || this.CheckGreater(cell, (object) setConditionValue2) && isGreaterThan2 && this.CompareValues(isGreaterThan3, cell, num1) || this.CheckCriteria(iconSetImpl.IconCriteria[2]))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_YellowUpInclineArrow.png";
            iconId = 2;
            break;
          }
          iconName = "CF_IS_YellowDownInclineArrow.png";
          iconId = 1;
          break;
        }
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue1) && !isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckGreater(cell, (object) setConditionValue1) && isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckCriteria(iconSetImpl.IconCriteria[1]))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_YellowDownInclineArrow.png";
            iconId = 1;
            break;
          }
          iconName = "CF_IS_YellowUpInclineArrow.png";
          iconId = 2;
          break;
        }
        if (this.CheckLess(cell, (object) setConditionValue1) && !isGreaterThan1 || this.CheckLessOrEqual(cell, (object) setConditionValue1) && isGreaterThan1)
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_RedDownArrow.png";
            iconId = 0;
            break;
          }
          iconName = "CF_IS_GreenUpArrow.png";
          iconId = 3;
          break;
        }
        break;
      case 9:
        if (this.CheckGreaterOrEqual(cell, (object) num1) && !isGreaterThan3 || this.CheckGreater(cell, (object) num1) && isGreaterThan3 || this.CheckCriteria(iconSetImpl.IconCriteria[3]))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_GrayUpArrow.png";
            iconId = 3;
            break;
          }
          iconName = "CF_IS_GrayDownArrow.png";
          iconId = 0;
          break;
        }
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue2) && !isGreaterThan2 && this.CompareValues(isGreaterThan3, cell, num1) || this.CheckGreater(cell, (object) setConditionValue2) && isGreaterThan2 && this.CompareValues(isGreaterThan3, cell, num1) || this.CheckCriteria(iconSetImpl.IconCriteria[2]))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_GrayUpInclineArrow.png";
            iconId = 2;
            break;
          }
          iconName = "CF_IS_GrayDownInclineArrow.png";
          iconId = 1;
          break;
        }
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue1) && !isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckGreater(cell, (object) setConditionValue1) && isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckCriteria(iconSetImpl.IconCriteria[1]))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_GrayDownInclineArrow.png";
            iconId = 1;
            break;
          }
          iconName = "CF_IS_GrayUpInclineArrow.png";
          iconId = 2;
          break;
        }
        if (this.CheckLess(cell, (object) setConditionValue1) && !isGreaterThan1 || this.CheckLessOrEqual(cell, (object) setConditionValue1) && isGreaterThan1)
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_GrayDownArrow.png";
            iconId = 0;
            break;
          }
          iconName = "CF_IS_GrayUpArrow.png";
          iconId = 3;
          break;
        }
        break;
      case 10:
        if (this.CheckGreaterOrEqual(cell, (object) num1) && !isGreaterThan3 || this.CheckGreater(cell, (object) num1) && isGreaterThan3 || this.CheckCriteria(iconSetImpl.IconCriteria[3]))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_RedCircle.png";
            iconId = 3;
            break;
          }
          iconName = "CF_IS_BlackCircle.png";
          iconId = 0;
          break;
        }
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue2) && !isGreaterThan2 && this.CompareValues(isGreaterThan3, cell, num1) || this.CheckGreater(cell, (object) setConditionValue2) && isGreaterThan2 && this.CompareValues(isGreaterThan3, cell, num1) || this.CheckCriteria(iconSetImpl.IconCriteria[2]))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_PinkCircle.png";
            iconId = 2;
            break;
          }
          iconName = "CF_IS_GrayCircle.png";
          iconId = 1;
          break;
        }
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue1) && !isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckGreater(cell, (object) setConditionValue1) && isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckCriteria(iconSetImpl.IconCriteria[1]))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_GrayCircle.png";
            iconId = 1;
            break;
          }
          iconName = "CF_IS_PinkCircle.png";
          iconId = 2;
          break;
        }
        if (this.CheckLess(cell, (object) setConditionValue1) && !isGreaterThan1 || this.CheckLessOrEqual(cell, (object) setConditionValue1) && isGreaterThan1)
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_BlackCircle.png";
            iconId = 0;
            break;
          }
          iconName = "CF_IS_RedCircle.png";
          iconId = 3;
          break;
        }
        break;
      case 11:
        if (this.CheckGreaterOrEqual(cell, (object) num1) && !isGreaterThan3 || this.CheckGreater(cell, (object) num1) && isGreaterThan3 || this.CheckCriteria(iconSetImpl.IconCriteria[3]))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_SignalWithFourFillBars.png";
            iconId = 3;
            break;
          }
          iconName = "CF_IS_SignalWithOneFillBar.png";
          iconId = 0;
          break;
        }
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue2) && !isGreaterThan2 && this.CompareValues(isGreaterThan3, cell, num1) || this.CheckGreater(cell, (object) setConditionValue2) && isGreaterThan2 && this.CompareValues(isGreaterThan3, cell, num1) || this.CheckCriteria(iconSetImpl.IconCriteria[2]))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_SignalWithThreeFillBars.png";
            iconId = 2;
            break;
          }
          iconName = "CF_IS_SignalWithTwoFillBars.png";
          iconId = 1;
          break;
        }
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue1) && !isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckGreater(cell, (object) setConditionValue1) && isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckCriteria(iconSetImpl.IconCriteria[1]))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_SignalWithTwoFillBars.png";
            iconId = 1;
            break;
          }
          iconName = "CF_IS_SignalWithThreeFillBars.png";
          iconId = 2;
          break;
        }
        if (this.CheckLess(cell, (object) setConditionValue1) && !isGreaterThan1 || this.CheckLessOrEqual(cell, (object) setConditionValue1) && isGreaterThan1)
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_SignalWithOneFillBar.png";
            iconId = 0;
            break;
          }
          iconName = "CF_IS_SignalWithFourFillBars.png";
          iconId = 3;
          break;
        }
        break;
      case 12:
        if (this.CheckGreaterOrEqual(cell, (object) num1) && !isGreaterThan3 || this.CheckGreater(cell, (object) num1) && isGreaterThan3 || this.CheckCriteria(iconSetImpl.IconCriteria[3]))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_GreenCircle.png";
            iconId = 3;
            break;
          }
          iconName = "CF_IS_BlackCircleWithBorder.png";
          iconId = 0;
          break;
        }
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue2) && !isGreaterThan2 && this.CompareValues(isGreaterThan3, cell, num1) || this.CheckGreater(cell, (object) setConditionValue2) && isGreaterThan2 && this.CompareValues(isGreaterThan3, cell, num1) || this.CheckCriteria(iconSetImpl.IconCriteria[2]))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_YellowCircle.png";
            iconId = 2;
            break;
          }
          iconName = "CF_IS_RedCircleWithBorder.png";
          iconId = 1;
          break;
        }
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue1) && !isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckGreater(cell, (object) setConditionValue1) && isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckCriteria(iconSetImpl.IconCriteria[1]))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_RedCircleWithBorder.png";
            iconId = 1;
            break;
          }
          iconName = "CF_IS_YellowCircle.png";
          iconId = 2;
          break;
        }
        if (this.CheckLess(cell, (object) setConditionValue1) && !isGreaterThan1 || this.CheckLessOrEqual(cell, (object) setConditionValue1) && isGreaterThan1)
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_BlackCircleWithBorder.png";
            iconId = 0;
            break;
          }
          iconName = "CF_IS_GreenCircle.png";
          iconId = 3;
          break;
        }
        break;
      case 13:
        if (this.CheckGreaterOrEqual(cell, (object) num2) && !isGreaterThan4 || this.CheckGreater(cell, (object) num2) && isGreaterThan4 || this.CheckCriteria(iconSetImpl.IconCriteria[4]))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_GreenUpArrow.png";
            iconId = 4;
            break;
          }
          iconName = "CF_IS_RedDownArrow.png";
          iconId = 0;
          break;
        }
        if (this.CheckGreaterOrEqual(cell, (object) num1) && !isGreaterThan3 && this.CompareValues(isGreaterThan4, cell, num2) || this.CheckGreater(cell, (object) num1) && isGreaterThan3 && this.CompareValues(isGreaterThan4, cell, num2) || this.CheckCriteria(iconSetImpl.IconCriteria[3]))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_YellowUpInclineArrow.png";
            iconId = 3;
            break;
          }
          iconName = "CF_IS_YellowDownInclineArrow.png";
          iconId = 1;
          break;
        }
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue2) && !isGreaterThan2 && this.CompareValues(isGreaterThan3, cell, num1) || this.CheckGreater(cell, (object) setConditionValue2) && isGreaterThan2 && this.CompareValues(isGreaterThan3, cell, num1) || this.CheckCriteria(iconSetImpl.IconCriteria[2]))
        {
          iconName = "CF_IS_YellowSideArrow.png";
          iconId = 2;
          break;
        }
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue1) && !isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckGreater(cell, (object) setConditionValue1) && isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckCriteria(iconSetImpl.IconCriteria[1]))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_YellowDownInclineArrow.png";
            iconId = 1;
            break;
          }
          iconName = "CF_IS_YellowUpInclineArrow.png";
          iconId = 3;
          break;
        }
        if (this.CheckLess(cell, (object) setConditionValue1) && !isGreaterThan1 || this.CheckLessOrEqual(cell, (object) setConditionValue1) && isGreaterThan1)
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_RedDownArrow.png";
            iconId = 0;
            break;
          }
          iconName = "CF_IS_GreenUpArrow.png";
          iconId = 4;
          break;
        }
        break;
      case 14:
        if (this.CheckGreaterOrEqual(cell, (object) num2) && !isGreaterThan4 || this.CheckGreater(cell, (object) num2) && isGreaterThan4 || this.CheckCriteria(iconSetImpl.IconCriteria[4]))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_GrayUpArrow.png";
            iconId = 4;
            break;
          }
          iconName = "CF_IS_GrayDownArrow.png";
          iconId = 0;
          break;
        }
        if (this.CheckGreaterOrEqual(cell, (object) num1) && !isGreaterThan3 && this.CompareValues(isGreaterThan4, cell, num2) || this.CheckGreater(cell, (object) num1) && isGreaterThan3 && this.CompareValues(isGreaterThan4, cell, num2) || this.CheckCriteria(iconSetImpl.IconCriteria[3]))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_GrayUpInclineArrow.png";
            iconId = 3;
            break;
          }
          iconName = "CF_IS_GrayDownInclineArrow.png";
          iconId = 1;
          break;
        }
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue2) && !isGreaterThan2 && this.CompareValues(isGreaterThan3, cell, num1) || this.CheckGreater(cell, (object) setConditionValue2) && isGreaterThan2 && this.CompareValues(isGreaterThan3, cell, num1) || this.CheckCriteria(iconSetImpl.IconCriteria[2]))
        {
          iconName = "CF_IS_GraySideArrow.png";
          iconId = 2;
          break;
        }
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue1) && !isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckGreater(cell, (object) setConditionValue1) && isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckCriteria(iconSetImpl.IconCriteria[1]))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_GrayDownInclineArrow.png";
            iconId = 1;
            break;
          }
          iconName = "CF_IS_GrayUpInclineArrow.png";
          iconId = 3;
          break;
        }
        if (this.CheckLess(cell, (object) setConditionValue1) && !isGreaterThan1 || this.CheckLessOrEqual(cell, (object) setConditionValue1) && isGreaterThan1)
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_GrayDownArrow.png";
            iconId = 0;
            break;
          }
          iconName = "CF_IS_GrayUpArrow.png";
          iconId = 4;
          break;
        }
        break;
      case 15:
        if (this.CheckGreaterOrEqual(cell, (object) num2) && !isGreaterThan4 || this.CheckGreater(cell, (object) num2) && isGreaterThan4 || this.CheckCriteria(iconSetImpl.IconCriteria[4]))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_SignalWithFourFillBars.png";
            iconId = 4;
            break;
          }
          iconName = "CF_IS_SignalWithNoFillBars.png";
          iconId = 0;
          break;
        }
        if (this.CheckGreaterOrEqual(cell, (object) num1) && !isGreaterThan3 && this.CompareValues(isGreaterThan4, cell, num2) || this.CheckGreater(cell, (object) num1) && isGreaterThan3 && this.CompareValues(isGreaterThan4, cell, num2) || this.CheckCriteria(iconSetImpl.IconCriteria[3]))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_SignalWithThreeFillBars.png";
            iconId = 3;
            break;
          }
          iconName = "CF_IS_SignalWithOneFillBar.png";
          iconId = 1;
          break;
        }
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue2) && !isGreaterThan2 && this.CompareValues(isGreaterThan3, cell, num1) || this.CheckGreater(cell, (object) setConditionValue2) && isGreaterThan2 && this.CompareValues(isGreaterThan3, cell, num1) || this.CheckCriteria(iconSetImpl.IconCriteria[2]))
        {
          iconName = "CF_IS_SignalWithTwoFillBars.png";
          iconId = 2;
          break;
        }
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue1) && !isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckGreater(cell, (object) setConditionValue1) && isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckCriteria(iconSetImpl.IconCriteria[1]))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_SignalWithOneFillBar.png";
            iconId = 1;
            break;
          }
          iconName = "CF_IS_SignalWithThreeFillBars.png";
          iconId = 3;
          break;
        }
        if (this.CheckLess(cell, (object) setConditionValue1) && !isGreaterThan1 || this.CheckLessOrEqual(cell, (object) setConditionValue1) && isGreaterThan1)
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_SignalWithNoFillBars.png";
            iconId = 0;
            break;
          }
          iconName = "CF_IS_SignalWithFourFillBars.png";
          iconId = 4;
          break;
        }
        break;
      case 16 /*0x10*/:
        if (this.CheckGreaterOrEqual(cell, (object) num2) && !isGreaterThan4 || this.CheckGreater(cell, (object) num2) && isGreaterThan4 || this.CheckCriteria(iconSetImpl.IconCriteria[4]))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_BlackCircle.png";
            iconId = 4;
            break;
          }
          iconName = "CF_IS_CircleAllWQuarters.png";
          iconId = 0;
          break;
        }
        if (this.CheckGreaterOrEqual(cell, (object) num1) && !isGreaterThan3 && this.CompareValues(isGreaterThan4, cell, num2) || this.CheckGreater(cell, (object) num1) && isGreaterThan3 && this.CompareValues(isGreaterThan4, cell, num2) || this.CheckCriteria(iconSetImpl.IconCriteria[3]))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_CircleOneWQuarter.png";
            iconId = 3;
            break;
          }
          iconName = "CF_IS_CircleThreeWQuarters.png";
          iconId = 1;
          break;
        }
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue2) && !isGreaterThan2 && this.CompareValues(isGreaterThan3, cell, num1) || this.CheckGreater(cell, (object) setConditionValue2) && isGreaterThan2 && this.CompareValues(isGreaterThan3, cell, num1) || this.CheckCriteria(iconSetImpl.IconCriteria[2]))
        {
          iconName = "CF_IS_CircleTwoWQuarters.png";
          iconId = 2;
          break;
        }
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue1) && !isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckGreater(cell, (object) setConditionValue1) && isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckCriteria(iconSetImpl.IconCriteria[1]))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_CircleThreeWQuarters.png";
            iconId = 1;
            break;
          }
          iconName = "CF_IS_CircleOneWQuarter.png";
          iconId = 3;
          break;
        }
        if (this.CheckLess(cell, (object) setConditionValue1) && !isGreaterThan1 || this.CheckLessOrEqual(cell, (object) setConditionValue1) && isGreaterThan1)
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_CircleAllWQuarters.png";
            iconId = 0;
            break;
          }
          iconName = "CF_IS_BlackCircle.png";
          iconId = 4;
          break;
        }
        break;
      case 17:
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue2) && !isGreaterThan2 || this.CheckGreater(cell, (object) setConditionValue2) && isGreaterThan2 || this.CheckCriteria(iconSetImpl.IconCriteria[2]))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_GoldStar.png";
            iconId = 2;
            break;
          }
          iconName = "CF_IS_SilverStar.png";
          iconId = 0;
          break;
        }
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue1) && !isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckGreater(cell, (object) setConditionValue1) && isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckCriteria(iconSetImpl.IconCriteria[1]))
        {
          iconName = "CF_IS_HalfGoldStar.png";
          iconId = 1;
          break;
        }
        if (this.CheckLess(cell, (object) setConditionValue1) && !isGreaterThan1 || this.CheckLessOrEqual(cell, (object) setConditionValue1) && isGreaterThan1)
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_SilverStar.png";
            iconId = 0;
            break;
          }
          iconName = "CF_IS_GoldStar.png";
          iconId = 2;
          break;
        }
        break;
      case 18:
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue2) && !isGreaterThan2 || this.CheckGreater(cell, (object) setConditionValue2) && isGreaterThan2 || this.CheckCriteria(iconSetImpl.IconCriteria[2]))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_GreenUpTriangle.png";
            iconId = 2;
            break;
          }
          iconName = "CF_IS_RedDownTriangle.png";
          iconId = 0;
          break;
        }
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue1) && !isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckGreater(cell, (object) setConditionValue1) && isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckCriteria(iconSetImpl.IconCriteria[1]))
        {
          iconName = "CF_IS_YellowDash.png";
          iconId = 1;
          break;
        }
        if (this.CheckLess(cell, (object) setConditionValue1) && !isGreaterThan1 || this.CheckLessOrEqual(cell, (object) setConditionValue1) && isGreaterThan1)
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_RedDownTriangle.png";
            iconId = 0;
            break;
          }
          iconName = "CF_IS_GreenUpTriangle.png";
          iconId = 2;
          break;
        }
        break;
      case 19:
        if (this.CheckGreaterOrEqual(cell, (object) num2) && !isGreaterThan4 || this.CheckGreater(cell, (object) num2) && isGreaterThan4 || this.CheckCriteria(iconSetImpl.IconCriteria[4]))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_FourFilledBoxes.png";
            iconId = 4;
            break;
          }
          iconName = "CF_IS_ZeroFilledBoxes.png";
          iconId = 0;
          break;
        }
        if (this.CheckGreaterOrEqual(cell, (object) num1) && !isGreaterThan3 && this.CompareValues(isGreaterThan4, cell, num2) || this.CheckGreater(cell, (object) num1) && isGreaterThan3 && this.CompareValues(isGreaterThan4, cell, num2) || this.CheckCriteria(iconSetImpl.IconCriteria[3]))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_ThreeFilledBoxes.png";
            iconId = 3;
            break;
          }
          iconName = "CF_IS_OneFilledBox.png";
          iconId = 1;
          break;
        }
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue2) && !isGreaterThan2 && this.CompareValues(isGreaterThan3, cell, num1) || this.CheckGreater(cell, (object) setConditionValue2) && isGreaterThan2 && this.CompareValues(isGreaterThan3, cell, num1) || this.CheckCriteria(iconSetImpl.IconCriteria[2]))
        {
          iconName = "CF_IS_TwoFilledBoxes.png";
          iconId = 2;
          break;
        }
        if (this.CheckGreaterOrEqual(cell, (object) setConditionValue1) && !isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckGreater(cell, (object) setConditionValue1) && isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckCriteria(iconSetImpl.IconCriteria[1]))
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_OneFilledBox.png";
            iconId = 1;
            break;
          }
          iconName = "CF_IS_ThreeFilledBoxes.png";
          iconId = 3;
          break;
        }
        if (this.CheckLess(cell, (object) setConditionValue1) && !isGreaterThan1 || this.CheckLessOrEqual(cell, (object) setConditionValue1) && isGreaterThan1)
        {
          if (!reverseOrder)
          {
            iconName = "CF_IS_ZeroFilledBoxes.png";
            iconId = 0;
            break;
          }
          iconName = "CF_IS_FourFilledBoxes.png";
          iconId = 4;
          break;
        }
        break;
      default:
        IConditionValue conditionValue = (IConditionValue) null;
        switch (iconSetImpl.IconCriteria.Count)
        {
          case 3:
            if (this.CheckGreaterOrEqual(cell, (object) setConditionValue2) && !isGreaterThan2 || this.CheckGreater(cell, (object) setConditionValue2) && isGreaterThan2 || this.CheckCriteria(iconSetImpl.IconCriteria[2]))
            {
              conditionValue = reverseOrder ? iconSetImpl.IconCriteria[0] : iconSetImpl.IconCriteria[2];
              break;
            }
            if (this.CheckGreaterOrEqual(cell, (object) setConditionValue1) && !isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckGreater(cell, (object) setConditionValue1) && isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckCriteria(iconSetImpl.IconCriteria[1]))
            {
              conditionValue = iconSetImpl.IconCriteria[1];
              break;
            }
            if (this.CheckLess(cell, (object) setConditionValue1) && !isGreaterThan1 || this.CheckLessOrEqual(cell, (object) setConditionValue1) && isGreaterThan1)
            {
              conditionValue = reverseOrder ? iconSetImpl.IconCriteria[2] : iconSetImpl.IconCriteria[0];
              break;
            }
            break;
          case 4:
            if (this.CheckGreaterOrEqual(cell, (object) num1) && !isGreaterThan3 || this.CheckGreater(cell, (object) num1) && isGreaterThan3 || this.CheckCriteria(iconSetImpl.IconCriteria[3]))
            {
              conditionValue = reverseOrder ? iconSetImpl.IconCriteria[0] : iconSetImpl.IconCriteria[3];
              break;
            }
            if (this.CheckGreaterOrEqual(cell, (object) setConditionValue2) && this.CompareValues(isGreaterThan3, cell, num1) && !isGreaterThan2 || this.CheckGreater(cell, (object) setConditionValue2) && isGreaterThan2 && this.CompareValues(isGreaterThan3, cell, num1) || this.CheckCriteria(iconSetImpl.IconCriteria[2]))
            {
              conditionValue = reverseOrder ? iconSetImpl.IconCriteria[1] : iconSetImpl.IconCriteria[2];
              break;
            }
            if (this.CheckGreaterOrEqual(cell, (object) setConditionValue1) && !isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckGreater(cell, (object) setConditionValue1) && isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckCriteria(iconSetImpl.IconCriteria[1]))
            {
              conditionValue = reverseOrder ? iconSetImpl.IconCriteria[2] : iconSetImpl.IconCriteria[1];
              break;
            }
            if (this.CheckLess(cell, (object) setConditionValue1) && !isGreaterThan1 || this.CheckLessOrEqual(cell, (object) setConditionValue1) && isGreaterThan1)
            {
              conditionValue = reverseOrder ? iconSetImpl.IconCriteria[3] : iconSetImpl.IconCriteria[0];
              break;
            }
            break;
          case 5:
            if (this.CheckGreaterOrEqual(cell, (object) num2) && !isGreaterThan4 || this.CheckGreater(cell, (object) num2) && isGreaterThan4 || this.CheckCriteria(iconSetImpl.IconCriteria[4]))
            {
              conditionValue = reverseOrder ? iconSetImpl.IconCriteria[0] : iconSetImpl.IconCriteria[4];
              break;
            }
            if (this.CheckGreaterOrEqual(cell, (object) num1) && !isGreaterThan3 && this.CompareValues(isGreaterThan4, cell, num2) || this.CheckGreater(cell, (object) num1) && isGreaterThan3 && this.CompareValues(isGreaterThan4, cell, num2) || this.CheckCriteria(iconSetImpl.IconCriteria[3]))
            {
              conditionValue = reverseOrder ? iconSetImpl.IconCriteria[1] : iconSetImpl.IconCriteria[3];
              break;
            }
            if (this.CheckGreaterOrEqual(cell, (object) setConditionValue2) && !isGreaterThan2 && this.CompareValues(isGreaterThan3, cell, num1) || this.CheckGreater(cell, (object) setConditionValue2) && isGreaterThan2 && this.CompareValues(isGreaterThan3, cell, num1) || this.CheckCriteria(iconSetImpl.IconCriteria[2]))
            {
              conditionValue = iconSetImpl.IconCriteria[2];
              break;
            }
            if (this.CheckGreaterOrEqual(cell, (object) setConditionValue1) && !isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckGreater(cell, (object) setConditionValue1) && isGreaterThan1 && this.CompareValues(isGreaterThan2, cell, setConditionValue2) || this.CheckCriteria(iconSetImpl.IconCriteria[1]))
            {
              conditionValue = reverseOrder ? iconSetImpl.IconCriteria[3] : iconSetImpl.IconCriteria[1];
              break;
            }
            if (this.CheckLess(cell, (object) setConditionValue1) && !isGreaterThan1 || this.CheckLessOrEqual(cell, (object) setConditionValue1) && isGreaterThan1)
            {
              conditionValue = reverseOrder ? iconSetImpl.IconCriteria[4] : iconSetImpl.IconCriteria[0];
              break;
            }
            break;
        }
        if (conditionValue is IconConditionValue)
        {
          IconConditionValue iconConditionValue = conditionValue as IconConditionValue;
          iconName = this.MapIconIdToImage(iconConditionValue);
          iconSet1 = iconConditionValue.IconSet;
          iconId = iconConditionValue.Index;
          break;
        }
        break;
    }
    return iconName != null ? this.ApplyCondition(iconName, iconSet1, iconId, xf, iconSetImpl.ShowIconOnly) : (ExtendedFormatStandAlone) null;
  }

  private bool CompareValues(bool operatorValue, IRange cell, double value)
  {
    return !operatorValue ? this.CheckLess(cell, (object) value) : this.CheckLessOrEqual(cell, (object) value);
  }

  private string MapIconIdToImage(IconConditionValue iconConditionValue)
  {
    switch ($"{(object) iconConditionValue.IconSet}_{(object) iconConditionValue.Index}")
    {
      case "ThreeArrows_2":
        return "CF_IS_GreenUpArrow.png";
      case "ThreeArrows_1":
        return "CF_IS_YellowSideArrow.png";
      case "ThreeArrows_0":
        return "CF_IS_RedDownArrow.png";
      case "ThreeTriangles_2":
        return "CF_IS_GreenUpTriangle.png";
      case "ThreeTriangles_1":
        return "CF_IS_YellowDash.png";
      case "ThreeTriangles_0":
        return "CF_IS_RedDownTriangle.png";
      case "ThreeArrowsGray_2":
        return "CF_IS_GrayUpArrow.png";
      case "ThreeArrowsGray_1":
        return "CF_IS_GraySideArrow.png";
      case "ThreeArrowsGray_0":
        return "CF_IS_GrayDownArrow.png";
      case "ThreeFlags_2":
        return "CF_IS_GreenFlag.png";
      case "ThreeFlags_1":
        return "CF_IS_YellowFlag.png";
      case "ThreeFlags_0":
        return "CF_IS_RedFlag.png";
      case "ThreeSigns_2":
        return "CF_IS_GreenCircle.png";
      case "ThreeSigns_1":
        return "CF_IS_YellowTriangle.png";
      case "ThreeSigns_0":
        return "CF_IS_RedDiamond.png";
      case "ThreeSymbols_2":
        return "CF_IS_GreenCheckSymbol.png";
      case "ThreeSymbols_1":
        return "CF_IS_YellowExclamationSymbol.png";
      case "ThreeSymbols_0":
        return "CF_IS_RedCrossSymbol.png";
      case "ThreeSymbols2_2":
        return "CF_IS_GreenCheck.png";
      case "ThreeSymbols2_1":
        return "CF_IS_YellowExclamation.png";
      case "ThreeSymbols2_0":
        return "CF_IS_RedCross.png";
      case "ThreeTrafficLights1_2":
        return "CF_IS_GreenCircle.png";
      case "ThreeTrafficLights1_1":
        return "CF_IS_YellowCircle.png";
      case "ThreeTrafficLights1_0":
        return "CF_IS_RedCircleWithBorder.png";
      case "ThreeTrafficLights2_2":
        return "CF_IS_GreenTrafficLight.png";
      case "ThreeTrafficLights2_1":
        return "CF_IS_YellowTrafficLight.png";
      case "ThreeTrafficLights2_0":
        return "CF_IS_RedTrafficLight.png";
      case "ThreeStars_2":
        return "CF_IS_GoldStar.png";
      case "ThreeStars_1":
        return "CF_IS_HalfGoldStar.png";
      case "ThreeStars_0":
        return "CF_IS_SilverStar.png";
      case "FourArrows_3":
        return "CF_IS_GreenUpArrow.png";
      case "FourArrows_2":
        return "CF_IS_YellowUpInclineArrow.png";
      case "FourArrows_1":
        return "CF_IS_YellowDownInclineArrow.png";
      case "FourArrows_0":
        return "CF_IS_RedDownArrow.png";
      case "FourArrowsGray_3":
        return "CF_IS_GrayUpArrow.png";
      case "FourArrowsGray_2":
        return "CF_IS_GrayUpInclineArrow.png";
      case "FourArrowsGray_1":
        return "CF_IS_GrayDownInclineArrow.png";
      case "FourArrowsGray_0":
        return "CF_IS_GrayDownArrow.png";
      case "FourTrafficLights_3":
        return "CF_IS_GreenCircle.png";
      case "FourTrafficLights_2":
        return "CF_IS_YellowCircle.png";
      case "FourTrafficLights_1":
        return "CF_IS_RedCircleWithBorder.png";
      case "FourTrafficLights_0":
        return "CF_IS_BlackCircleWithBorder.png";
      case "FourRedToBlack_3":
        return "CF_IS_RedCircle.png";
      case "FourRedToBlack_2":
        return "CF_IS_PinkCircle.png";
      case "FourRedToBlack_1":
        return "CF_IS_GrayCircle.png";
      case "FourRedToBlack_0":
        return "CF_IS_BlackCircle.png";
      case "FourRating_3":
        return "CF_IS_SignalWithFourFillBars.png";
      case "FourRating_2":
        return "CF_IS_SignalWithThreeFillBars.png";
      case "FourRating_1":
        return "CF_IS_SignalWithTwoFillBars.png";
      case "FourRating_0":
        return "CF_IS_SignalWithOneFillBar.png";
      case "FiveArrows_4":
        return "CF_IS_GreenUpArrow.png";
      case "FiveArrows_3":
        return "CF_IS_YellowUpInclineArrow.png";
      case "FiveArrows_2":
        return "CF_IS_YellowSideArrow.png";
      case "FiveArrows_1":
        return "CF_IS_YellowDownInclineArrow.png";
      case "FiveArrows_0":
        return "CF_IS_RedDownArrow.png";
      case "FiveArrowsGray_4":
        return "CF_IS_GrayUpArrow.png";
      case "FiveArrowsGray_3":
        return "CF_IS_GrayUpInclineArrow.png";
      case "FiveArrowsGray_2":
        return "CF_IS_GraySideArrow.png";
      case "FiveArrowsGray_1":
        return "CF_IS_GrayDownInclineArrow.png";
      case "FiveArrowsGray_0":
        return "CF_IS_GrayDownArrow.png";
      case "FiveRating_4":
        return "CF_IS_SignalWithFourFillBars.png";
      case "FiveRating_3":
        return "CF_IS_SignalWithThreeFillBars.png";
      case "FiveRating_2":
        return "CF_IS_SignalWithTwoFillBars.png";
      case "FiveRating_1":
        return "CF_IS_SignalWithOneFillBar.png";
      case "FiveRating_0":
        return "CF_IS_SignalWithNoFillBars.png";
      case "FiveQuarters_4":
        return "CF_IS_BlackCircle.png";
      case "FiveQuarters_3":
        return "CF_IS_CircleOneWQuarter.png";
      case "FiveQuarters_2":
        return "CF_IS_CircleTwoWQuarters.png";
      case "FiveQuarters_1":
        return "CF_IS_CircleThreeWQuarters.png";
      case "FiveQuarters_0":
        return "CF_IS_CircleAllWQuarters.png";
      case "FiveBoxes_4":
        return "CF_IS_FourFilledBoxes.png";
      case "FiveBoxes_3":
        return "CF_IS_ThreeFilledBoxes.png";
      case "FiveBoxes_2":
        return "CF_IS_TwoFilledBoxes.png";
      case "FiveBoxes_1":
        return "CF_IS_OneFilledBox.png";
      case "FiveBoxes_0":
        return "CF_IS_ZeroFilledBoxes.png";
      default:
        return (string) null;
    }
  }

  private bool CheckCriteria(IConditionValue conditionValue)
  {
    return conditionValue.Type == ConditionValueType.HighestValue || conditionValue.Type == ConditionValueType.LowestValue;
  }

  private double GetIconSetConditionValue(
    IRange cell,
    IConditionValue condition,
    string formatRange,
    out bool isGreaterThan)
  {
    double result = 0.0;
    isGreaterThan = false;
    bool flag = false;
    IWorksheet worksheet = cell.Worksheet;
    if (condition != null)
    {
      if (worksheet.CalcEngine == null)
      {
        cell.Worksheet.EnableSheetCalculations();
        flag = true;
      }
      this.TryGetPointValue(cell, condition, formatRange, out result);
      if (flag)
        cell.Worksheet.DisableSheetCalculations();
      isGreaterThan = condition.Operator == ConditionalFormatOperator.GreaterThan;
    }
    return result;
  }

  private bool TryGetPointValue(
    IRange cell,
    IConditionValue condition,
    string range,
    out double result)
  {
    double num1 = -1.0;
    if (!string.IsNullOrEmpty(condition.Value))
    {
      string empty = string.Empty;
      string input = condition.Value;
      if (input.Contains("."))
        input = input.Replace(".", cell.Application.DecimalSeparator);
      string s;
      if (Regex.IsMatch(input, "([$][A-Z][$][1-9])|([$][A-Z][1-9])|([A-Z][$][1-9])|([A-Z][1-9])", RegexOptions.IgnorePatternWhitespace))
      {
        CalcEngine calcEngine = cell.Worksheet.CalcEngine;
        calcEngine.cell = cell.AddressLocal;
        s = calcEngine.ComputedValue(calcEngine.Parse("=" + input));
      }
      else
        s = input;
      double result1;
      if (!double.TryParse(s, out result1))
      {
        result = 0.0;
        return false;
      }
      num1 = result1;
    }
    if (condition.Type == ConditionValueType.Number || condition.Type == ConditionValueType.Formula)
    {
      result = num1;
      return true;
    }
    if (condition.Type == ConditionValueType.HighestValue)
    {
      if (this.MaxValue == 0.0)
      {
        this.TryGetMaxValue(cell, range, out result);
        this.MaxValue = result;
      }
      else
        result = this.MaxValue;
      return true;
    }
    if (condition.Type == ConditionValueType.LowestValue)
    {
      if (this.MinValue == 0.0)
      {
        this.TryGetMinValue(cell, range, out result);
        this.MinValue = result;
      }
      else
        result = this.MinValue;
      return true;
    }
    if (condition.Type == ConditionValueType.Percent)
    {
      if (num1 > 100.0)
      {
        result = num1;
        return true;
      }
      double result2 = 0.0;
      double result3 = 0.0;
      if (this.MaxValue == 0.0)
      {
        this.TryGetMaxValue(cell, range, out result2);
        this.MaxValue = result2;
      }
      else
        result2 = this.MaxValue;
      if (this.MinValue == 0.0)
      {
        this.TryGetMinValue(cell, range, out result3);
        this.MinValue = result3;
      }
      else
        result3 = this.MinValue;
      double num2 = result2 - result3;
      double num3 = num1 / 100.0;
      result = num2 * num3 + result3;
      return true;
    }
    if (condition.Type == ConditionValueType.Percentile)
    {
      if (num1 > 100.0)
      {
        result = num1;
        return true;
      }
      string oldValue = cell.Worksheet.Application.ArgumentsSeparator.ToString();
      string str = range.Replace(oldValue, ":");
      double num4 = num1 / 100.0;
      return double.TryParse(cell.Worksheet.CalcEngine.ComputePercentile(str + oldValue + (object) num4), out result);
    }
    result = 0.0;
    return false;
  }

  private void TryGetMinValue(IRange cell, string range, out double result)
  {
    double.TryParse(cell.Worksheet.CalcEngine.ComputeMin(range), out result);
  }

  private void TryGetMaxValue(IRange cell, string range, out double result)
  {
    double.TryParse(cell.Worksheet.CalcEngine.ComputeMax(range), out result);
  }

  private Bitmap GetBitMap(string iconName)
  {
    return new Bitmap(typeof (XlsIOBaseAssembly).Assembly.GetManifestResourceStream("Syncfusion.XlsIO.CF_IconSets." + iconName));
  }

  internal ExtendedFormatImpl ConditionalFormatColorScale(
    IColorScale colorScale,
    IRange cell,
    string formatRange,
    ExtendedFormatImpl xf)
  {
    ExtendedFormatImpl extendedFormatImpl = (ExtendedFormatImpl) null;
    ColorScaleImpl colorScaleImpl = colorScale as ColorScaleImpl;
    IColorConditionValue criterion1 = colorScaleImpl.Criteria[0];
    IColorConditionValue criterion2 = colorScaleImpl.Criteria[1];
    bool isGreaterThan1 = false;
    bool isGreaterThan2 = false;
    bool isGreaterThan3 = false;
    double num = 0.0;
    Color formatColorRgb1 = criterion1.FormatColorRGB;
    Color formatColorRgb2 = criterion2.FormatColorRGB;
    Color color1 = Color.FromArgb(0, 0, 0, 0);
    Color color2 = Color.FromArgb(0, 0, 0, 0);
    WorksheetImpl worksheet = cell.Worksheet as WorksheetImpl;
    bool flag = false;
    if (worksheet.CalcEngine == null)
    {
      cell.Worksheet.EnableSheetCalculations();
      flag = true;
    }
    if (double.TryParse(cell.CalculatedValue, out double _) || (cell as RangeImpl).CellType == RangeImpl.TCellType.Number)
    {
      if (colorScaleImpl.Criteria.Count == 3)
      {
        IColorConditionValue criterion3 = colorScaleImpl.Criteria[2];
        color1 = criterion3.FormatColorRGB;
        num = this.GetColorConditionValue(cell, (IConditionValue) criterion3, formatRange, out isGreaterThan3);
      }
      double colorConditionValue1 = this.GetColorConditionValue(cell, (IConditionValue) criterion1, formatRange, out isGreaterThan1);
      double colorConditionValue2 = this.GetColorConditionValue(cell, (IConditionValue) criterion2, formatRange, out isGreaterThan2);
      if (colorScaleImpl.Criteria.Count == 2 && cell.CalculatedValue != "")
      {
        if (this.CheckGreaterOrEqual(cell, (object) colorConditionValue2))
          extendedFormatImpl = this.ApplyCondition(formatColorRgb2, xf);
        else if (this.CheckLessOrEqual(cell, (object) colorConditionValue1))
          extendedFormatImpl = this.ApplyCondition(formatColorRgb1, xf);
        else if (this.CheckBetween(cell, (object) colorConditionValue1, (object) colorConditionValue2))
        {
          if (Convert.ToDouble(cell.CalculatedValue) < colorConditionValue2)
            color2 = this.GetBlendedTwoColor((Convert.ToDouble(cell.CalculatedValue) - colorConditionValue1) / (colorConditionValue2 - colorConditionValue1) * 100.0, formatColorRgb1, formatColorRgb2);
          extendedFormatImpl = this.ApplyCondition(color2, xf);
        }
      }
      if (colorScaleImpl.Criteria.Count == 3 && cell.CalculatedValue != "")
      {
        if (this.CheckGreaterOrEqual(cell, (object) num))
          extendedFormatImpl = this.ApplyCondition(color1, xf);
        else if (this.CheckLessOrEqual(cell, (object) colorConditionValue1))
          extendedFormatImpl = this.ApplyCondition(formatColorRgb1, xf);
        else if (this.CheckEqual(cell, (object) colorConditionValue2))
          extendedFormatImpl = this.ApplyCondition(formatColorRgb2, xf);
        else if (this.CheckBetween(cell, (object) colorConditionValue1, (object) colorConditionValue2))
        {
          if (Convert.ToDouble(cell.CalculatedValue) < colorConditionValue2)
            color2 = this.GetBlendedTwoColor((Convert.ToDouble(cell.CalculatedValue) - colorConditionValue1) / (colorConditionValue2 - colorConditionValue1) * 100.0, formatColorRgb1, formatColorRgb2);
          extendedFormatImpl = this.ApplyCondition(color2, xf);
        }
        else if (this.CheckBetween(cell, (object) colorConditionValue2, (object) num))
        {
          if (Convert.ToDouble(cell.CalculatedValue) < num)
            color2 = this.GetBlendedTwoColor((Convert.ToDouble(cell.CalculatedValue) - colorConditionValue2) / (num - colorConditionValue2) * 100.0, formatColorRgb2, color1);
          extendedFormatImpl = this.ApplyCondition(color2, xf);
        }
      }
      if (flag)
        cell.Worksheet.DisableSheetCalculations();
    }
    return extendedFormatImpl;
  }

  private double GetColorConditionValue(
    IRange cell,
    IConditionValue condition,
    string formatRange,
    out bool isGreaterThan)
  {
    double result = 0.0;
    isGreaterThan = false;
    bool flag = false;
    IWorksheet worksheet = cell.Worksheet;
    if (condition != null)
    {
      if (worksheet.CalcEngine == null)
      {
        cell.Worksheet.EnableSheetCalculations();
        flag = true;
      }
      this.TryGetValue(cell, condition, formatRange, out result);
      if (flag)
        cell.Worksheet.DisableSheetCalculations();
      isGreaterThan = condition.Operator == ConditionalFormatOperator.GreaterThan;
    }
    return result;
  }

  private bool TryGetValue(
    IRange cell,
    IConditionValue condition,
    string range,
    out double result)
  {
    double num1 = -1.0;
    if (!string.IsNullOrEmpty(condition.Value))
    {
      string empty = string.Empty;
      string str = condition.Value;
      if (str.Contains("."))
        str = str.Replace(".", cell.Application.DecimalSeparator);
      CalcEngine calcEngine = cell.Worksheet.CalcEngine;
      double result1;
      if (!double.TryParse(calcEngine.ComputedValue(calcEngine.Parse("=" + str)), out result1))
      {
        result = 0.0;
        return false;
      }
      num1 = result1;
    }
    if (condition.Type == ConditionValueType.Number || condition.Type == ConditionValueType.Formula)
    {
      result = num1;
      return true;
    }
    if (condition.Type == ConditionValueType.HighestValue)
    {
      if (this.MaxValue == 0.0)
      {
        this.TryGetMaxValue(cell, range, out result);
        this.MaxValue = result;
      }
      else
        result = this.MaxValue;
      return true;
    }
    if (condition.Type == ConditionValueType.LowestValue)
    {
      if (this.MinValue == 0.0)
      {
        this.TryGetMinValue(cell, range, out result);
        this.MinValue = result;
      }
      else
        result = this.MinValue;
      return true;
    }
    if (condition.Type == ConditionValueType.Percent)
    {
      if (num1 > 100.0)
      {
        result = num1;
        return true;
      }
      double result2 = 0.0;
      double result3 = 0.0;
      if (this.MaxValue == 0.0)
      {
        this.TryGetMaxValue(cell, range, out result2);
        this.MaxValue = result2;
      }
      else
        result2 = this.MaxValue;
      if (this.MinValue == 0.0)
      {
        this.TryGetMinValue(cell, range, out result3);
        this.MinValue = result3;
      }
      else
        result3 = this.MinValue;
      double num2 = result2 - result3;
      double num3 = num1 / 100.0;
      result = num2 * num3 + result3;
      return true;
    }
    if (condition.Type == ConditionValueType.Percentile)
    {
      if (num1 > 100.0)
      {
        result = num1;
        return true;
      }
      string oldValue = cell.Worksheet.Application.ArgumentsSeparator.ToString();
      string str = range.Replace(oldValue, ":");
      double num4 = num1 / 100.0;
      return double.TryParse(cell.Worksheet.CalcEngine.ComputePercentile(str + oldValue + (object) num4), out result);
    }
    result = 0.0;
    return false;
  }

  private ExtendedFormatImpl ApplyCondition(Color color, ExtendedFormatImpl xf)
  {
    ExtendedFormatImpl extendedFormatImpl = (ExtendedFormatImpl) new ExtendedFormatStandAlone(xf);
    if (!this.IsColorApplied && (this.m_cfApplied & 4U) >> 2 != 1U)
    {
      extendedFormatImpl.Color = color;
      this.m_cfApplied |= 4U;
      this.m_isColorApplied = true;
    }
    return extendedFormatImpl;
  }

  private Color GetBlendedTwoColor(double increasePercent, Color color1, Color color2)
  {
    return increasePercent < 100.0 ? this.Interpolate(color1, color2, increasePercent / 100.0) : this.Interpolate(color1, color2, (increasePercent - 100.0) / 100.0);
  }

  private Color Interpolate(Color color1, Color color2, double fraction)
  {
    double a1 = this.Interpolate((double) color1.A, (double) color2.A, fraction);
    double a2 = this.Interpolate((double) color1.R, (double) color2.R, fraction);
    double a3 = this.Interpolate((double) color1.G, (double) color2.G, fraction);
    double a4 = this.Interpolate((double) color1.B, (double) color2.B, fraction);
    return Color.FromArgb((int) Convert.ToByte(Math.Round(a1)), (int) Convert.ToByte(Math.Round(a2)), (int) Convert.ToByte(Math.Round(a3)), (int) Convert.ToByte(Math.Round(a4)));
  }

  private double Interpolate(double d1, double d2, double fraction)
  {
    double num = d1 + (d2 - d1) * fraction;
    if (num < 0.0)
      num *= -1.0;
    return num;
  }

  internal ExtendedFormatStandAlone ApplyDataBar(
    IRange cell,
    IDataBar dataBar,
    string formatRange,
    ExtendedFormatImpl xf)
  {
    ExtendedFormatStandAlone formatStandAlone = (ExtendedFormatStandAlone) null;
    WorksheetImpl worksheet = cell.Worksheet as WorksheetImpl;
    bool flag1 = false;
    if (worksheet.CalcEngine == null)
    {
      cell.Worksheet.EnableSheetCalculations();
      flag1 = true;
    }
    double result1;
    if (double.TryParse(cell.CalculatedValue, out result1) || (cell as RangeImpl).CellType == RangeImpl.TCellType.Number)
    {
      result1 = (cell as RangeImpl).CellType == RangeImpl.TCellType.Number ? worksheet.GetNumber(cell.Row, cell.Column) : result1;
      DataBarImpl dataBarImpl = dataBar as DataBarImpl;
      IConditionValue minPoint = dataBarImpl.MinPoint;
      IConditionValue maxPoint = dataBarImpl.MaxPoint;
      int percentMax = dataBarImpl.PercentMax;
      int percentMin = dataBarImpl.PercentMin;
      bool showValue = dataBarImpl.ShowValue;
      Color barAxisColor = dataBarImpl.BarAxisColor;
      Color borderColor = dataBarImpl.BorderColor;
      Color negativeBorderColor = dataBarImpl.NegativeBorderColor;
      Color negativeFillColor = dataBarImpl.NegativeFillColor;
      Color barColor = dataBarImpl.BarColor;
      DataBarDirection dataBarDirection = dataBarImpl.DataBarDirection;
      DataBarAxisPosition dataBarAxisPosition = dataBarImpl.DataBarAxisPosition;
      bool isGreaterThan1 = false;
      bool isGreaterThan2 = false;
      int num = dataBarImpl.HasGradientFill ? 1 : 0;
      bool hasBorder = dataBarImpl.HasBorder;
      if (this.MinValue == 0.0)
      {
        double result2;
        this.IsNegative = double.TryParse(cell.Worksheet.CalcEngine.ComputeMin(formatRange), out result2);
        this.MinValue = result2;
      }
      else if (!this.IsNegative)
        this.IsNegative = this.MinValue < 0.0;
      bool isNegativeBar = this.IsNegative && this.MinValue < 0.0;
      int flag2 = isNegativeBar ? -1 : 0;
      double barConditionValue1 = this.GetDataBarConditionValue(cell, minPoint, formatRange, out isGreaterThan1, flag2);
      int flag3 = 1;
      double barConditionValue2 = this.GetDataBarConditionValue(cell, maxPoint, formatRange, out isGreaterThan2, flag3);
      double negativeBarPoint = 0.0;
      if (dataBarAxisPosition == DataBarAxisPosition.none)
        isNegativeBar = false;
      if (dataBarAxisPosition == DataBarAxisPosition.middle)
        negativeBarPoint = 0.5;
      if (barConditionValue1 < 0.0 && barConditionValue2 < 0.0 || barConditionValue1 > 0.0 && barConditionValue2 > 0.0 && isNegativeBar)
        isNegativeBar = false;
      if (barConditionValue1 < 0.0 && barConditionValue2 > 0.0 && dataBarAxisPosition != DataBarAxisPosition.middle && dataBarAxisPosition != DataBarAxisPosition.none || barConditionValue2 < 0.0 && barConditionValue1 > 0.0 && dataBarAxisPosition != DataBarAxisPosition.middle && dataBarAxisPosition != DataBarAxisPosition.none)
      {
        negativeBarPoint = !isNegativeBar || dataBarAxisPosition != DataBarAxisPosition.automatic || dataBarDirection != DataBarDirection.rightToLeft ? Math.Abs(barConditionValue1) / (Math.Abs(barConditionValue1) + Math.Abs(barConditionValue2)) : Math.Abs(barConditionValue2) / (Math.Abs(barConditionValue2) + Math.Abs(barConditionValue1));
        if (barConditionValue1 > barConditionValue2)
          negativeBarPoint = 1.0 - negativeBarPoint;
      }
      double _dataBarPercent;
      if (barConditionValue1 == 0.0 && barConditionValue2 == 0.0)
      {
        isNegativeBar = true;
        negativeBarPoint = 0.5;
        _dataBarPercent = result1 >= 0.0 ? 1.0 : -1.0;
      }
      else
        _dataBarPercent = this.DataBarLength(result1, barConditionValue1, barConditionValue2, minPoint, isNegativeBar, dataBar);
      if (flag1)
        cell.Worksheet.DisableSheetCalculations();
      formatStandAlone = this.ApplyCondition(barColor, xf, _dataBarPercent, hasBorder, dataBarDirection, borderColor, showValue, isNegativeBar, negativeBarPoint, barAxisColor, negativeFillColor, negativeBorderColor, result1, barConditionValue1, barConditionValue2, dataBar.DataBarAxisPosition);
    }
    return formatStandAlone;
  }

  private double DataBarLength(
    double cellValue,
    double min,
    double max,
    IConditionValue condition,
    bool isNegativeBar,
    IDataBar dataBar)
  {
    DataBarAxisPosition dataBarAxisPosition = (dataBar as DataBarImpl).DataBarAxisPosition;
    double num1 = 0.0;
    double num2 = 1.0;
    if (min > max && isNegativeBar)
    {
      if (cellValue < max)
        cellValue = max;
      else if (cellValue > min)
        cellValue = min;
    }
    else if (max < 0.0 && min < 0.0 && !isNegativeBar)
    {
      if (cellValue < min)
        cellValue = min;
    }
    else if (cellValue < min)
      cellValue = min;
    else if (cellValue > max)
      cellValue = max;
    double num3;
    if (isNegativeBar)
    {
      if (min > max)
        num3 = cellValue >= 0.0 ? num1 + cellValue / min * (num2 - num1) : -(num1 + -cellValue / max * (num1 - num2));
      else if (cellValue < 0.0)
      {
        num3 = dataBarAxisPosition != DataBarAxisPosition.middle ? -(num1 + -cellValue / min * (num1 - num2)) : (Math.Abs(min) >= Math.Abs(max) ? -(num1 + -cellValue / min * (num1 - num2)) : -(num1 + cellValue / max * (num1 - num2)));
        if (dataBarAxisPosition != DataBarAxisPosition.none && dataBar.DataBarDirection == DataBarDirection.rightToLeft)
          num3 = Math.Abs(num3);
      }
      else
      {
        num3 = dataBarAxisPosition != DataBarAxisPosition.middle ? num1 + cellValue / max * (num2 - num1) : (Math.Abs(min) >= Math.Abs(max) ? -(num1 + -cellValue / min * (num1 - num2)) : num1 + cellValue / max * (num2 - num1));
        if (dataBarAxisPosition != DataBarAxisPosition.none && dataBar.DataBarDirection == DataBarDirection.rightToLeft)
          num3 = -num3;
      }
    }
    else
      num3 = max >= 0.0 || min >= 0.0 || cellValue >= 0.0 ? (max >= 0.0 || min >= 0.0 || cellValue <= 0.0 ? num1 + (cellValue - min) / (max - min) * (num2 - num1) : 0.0) : -(num1 + -cellValue / min * (num1 - num2));
    return num3;
  }

  private ExtendedFormatStandAlone ApplyCondition(
    Color barColor,
    ExtendedFormatImpl xf,
    double _dataBarPercent,
    bool hasBorder,
    DataBarDirection dataBarDirection,
    Color borderColor,
    bool showValue,
    bool isNegativeBar,
    double negativeBarPoint,
    Color barAxisColor,
    Color negativeFillColor,
    Color negativeBorderColor,
    double cellValue,
    double minValue,
    double maxValue,
    DataBarAxisPosition dataBarAxisPosition)
  {
    ExtendedFormatStandAlone formatStandAlone = new ExtendedFormatStandAlone(xf);
    formatStandAlone.HasDataBar = true;
    formatStandAlone.MinValue = minValue;
    formatStandAlone.MaxValue = maxValue;
    formatStandAlone.DataBarPercent = _dataBarPercent;
    formatStandAlone.Color = barColor;
    if (hasBorder)
    {
      formatStandAlone.HasDataBarBorder = hasBorder;
      formatStandAlone.DataBarBorderColor = borderColor;
      formatStandAlone.NegativeDataBarBorderColor = negativeBorderColor;
    }
    formatStandAlone.DataBarDirection = dataBarDirection;
    formatStandAlone.ShowValue = showValue;
    if (negativeBarPoint != -1.0)
    {
      formatStandAlone.NegativeBarPoint = negativeBarPoint;
      formatStandAlone.BarAxisColor = barAxisColor;
    }
    if (isNegativeBar)
    {
      formatStandAlone.IsNegativeBar = isNegativeBar;
      formatStandAlone.NegativeBarPoint = negativeBarPoint;
      formatStandAlone.BarAxisColor = barAxisColor;
      formatStandAlone.NegativeFillColor = negativeFillColor;
      formatStandAlone.NegativeDataBarBorderColor = negativeBorderColor;
    }
    if (cellValue < 0.0)
    {
      formatStandAlone.NegativeFillColor = negativeFillColor;
      formatStandAlone.NegativeDataBarBorderColor = negativeBorderColor;
    }
    if (hasBorder && dataBarAxisPosition != DataBarAxisPosition.none && isNegativeBar && dataBarDirection == DataBarDirection.rightToLeft)
    {
      formatStandAlone.DataBarBorderColor = negativeBorderColor;
      formatStandAlone.NegativeDataBarBorderColor = borderColor;
    }
    if (hasBorder && dataBarAxisPosition == DataBarAxisPosition.none && cellValue < 0.0)
      formatStandAlone.DataBarBorderColor = negativeBorderColor;
    return formatStandAlone;
  }

  private double GetDataBarConditionValue(
    IRange cell,
    IConditionValue condition,
    string formatRange,
    out bool isGreaterThan,
    int flag)
  {
    double result = 0.0;
    isGreaterThan = false;
    IWorksheet worksheet = cell.Worksheet;
    if (condition != null)
    {
      this.TryGetPointValue(cell, condition, formatRange, out result, flag);
      isGreaterThan = condition.Operator == ConditionalFormatOperator.GreaterThan;
    }
    return result;
  }

  private bool TryGetPointValue(
    IRange cell,
    IConditionValue condition,
    string range,
    out double result,
    int flag)
  {
    double num1 = -1.0;
    if (!string.IsNullOrEmpty(condition.Value))
    {
      string str = condition.Value;
      if (str.Contains("."))
        str = str.Replace(".", cell.Application.DecimalSeparator);
      CalcEngine calcEngine = cell.Worksheet.CalcEngine;
      string empty = string.Empty;
      string s;
      if (Regex.IsMatch(str, "([$][A-Z][$][1-9])|([$][A-Z][1-9])|([A-Z][$][1-9])|([A-Z][1-9])", RegexOptions.IgnorePatternWhitespace))
      {
        calcEngine.cell = cell.AddressLocal;
        s = calcEngine.ComputedValue(calcEngine.Parse("=" + str));
      }
      else
        s = calcEngine.ComputeValue(str);
      double result1;
      if (!double.TryParse(s, out result1))
      {
        result = 0.0;
        return false;
      }
      num1 = result1;
    }
    if (condition.Type == ConditionValueType.Automatic)
    {
      switch (flag)
      {
        case -1:
          if (this.MinValue == 0.0)
          {
            this.TryGetMinValue(cell, range, out result);
            this.MinValue = result;
            break;
          }
          result = this.MinValue;
          break;
        case 1:
          if (this.MaxValue == 0.0)
          {
            this.TryGetMaxValue(cell, range, out result);
            this.MaxValue = result;
            break;
          }
          result = this.MaxValue;
          break;
        default:
          result = 0.0;
          return false;
      }
      return true;
    }
    if (condition.Type == ConditionValueType.Number || condition.Type == ConditionValueType.Formula)
    {
      result = num1;
      return true;
    }
    if (condition.Type == ConditionValueType.HighestValue)
    {
      if (this.MaxValue == 0.0)
      {
        this.TryGetMaxValue(cell, range, out result);
        this.MaxValue = result;
      }
      else
        result = this.MaxValue;
      return true;
    }
    if (condition.Type == ConditionValueType.LowestValue)
    {
      if (this.MinValue == 0.0)
      {
        this.TryGetMinValue(cell, range, out result);
        this.MinValue = result;
      }
      else
        result = this.MinValue;
      return true;
    }
    if (condition.Type == ConditionValueType.Percent)
    {
      if (num1 > 100.0)
      {
        result = num1;
        return true;
      }
      double result2 = 0.0;
      double result3 = 0.0;
      if (this.MaxValue == 0.0)
      {
        this.TryGetMaxValue(cell, range, out result2);
        this.MaxValue = result2;
      }
      else
        result2 = this.MaxValue;
      if (this.MinValue == 0.0)
      {
        this.TryGetMinValue(cell, range, out result3);
        this.MinValue = result3;
      }
      else
        result3 = this.MinValue;
      double num2 = result2 - result3;
      double num3 = num1 / 100.0;
      result = num2 * num3 + result3;
      return true;
    }
    if (condition.Type == ConditionValueType.Percentile)
    {
      if (num1 > 100.0)
      {
        result = num1;
        return true;
      }
      string oldValue = cell.Worksheet.Application.ArgumentsSeparator.ToString();
      string str = range.Replace(oldValue, ":");
      double num4 = num1 / 100.0;
      return double.TryParse(cell.Worksheet.CalcEngine.ComputePercentile(str + oldValue + (object) num4), out result);
    }
    result = 0.0;
    return false;
  }
}
