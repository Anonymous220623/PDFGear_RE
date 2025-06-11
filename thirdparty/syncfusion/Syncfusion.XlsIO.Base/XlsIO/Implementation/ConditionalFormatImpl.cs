// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.ConditionalFormatImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class ConditionalFormatImpl : 
  CommonObject,
  IInternalConditionalFormat,
  IConditionalFormat,
  IParentApplication,
  IOptimizedUpdate,
  ICloneParent
{
  private const uint DEF_NOT_CONTAIN_FONT_COLOR = 4294967295 /*0xFFFFFFFF*/;
  private const string DefaultBlankFormula = "LEN(TRIM({0}))=0";
  private const string DefaultNoBlankFormula = "LEN(TRIM({0}))>0";
  private const string DefaultErrorFormula = "ISERROR({0})";
  private const string DefaultNotErrorFormula = "NOT(ISERROR({0}))";
  private const string DefaultBeginsWithFormula = "LEFT({0},LEN({1}))={1}";
  private const string DefaultEndsWithFormula = "RIGHT({0},LEN({1}))={1}";
  private const string DefaultContainsTextFormula = "NOT(ISERROR(SEARCH({0},{1})))";
  private const string DefaultNotContainsTextFormula = "ISERROR(SEARCH({0},{1}))";
  private const string DefaultYesterdayTimePeriodFormula = "FLOOR({0},1)=TODAY()-1";
  private const string DefaultTodayTimePeriodFormula = "FLOOR({0},1)=TODAY()";
  private const string DefaultTomorrowTimePeriodFormula = "FLOOR({0},1)=TODAY()+1";
  private const string DefaultLastSevenDaysTimePeriodFormula = "AND(TODAY()-FLOOR({0},1)<=6,FLOOR({0},1)<=TODAY())";
  private const string DefaultLastWeekTimePeriodFormula = "AND(TODAY()-ROUNDDOWN({0},0)>=(WEEKDAY(TODAY())),TODAY()-ROUNDDOWN({0},0)<(WEEKDAY(TODAY())+7))";
  private const string DefaultThisWeekTimePeriodFormula = "AND(TODAY()-ROUNDDOWN({0},0)<=WEEKDAY(TODAY())-1,ROUNDDOWN({0},0)-TODAY()<=7-WEEKDAY(TODAY()))";
  private const string DefaultNextWeekTimePeriodFormula = "AND(ROUNDDOWN({0},0)-TODAY()>(7-WEEKDAY(TODAY())),ROUNDDOWN({0},0)-TODAY()<(15-WEEKDAY(TODAY())))";
  private const string DefaultLastMonthTimePeriodFormula = "AND(MONTH({0})=MONTH(EDATE(TODAY(),0-1)),YEAR({0})=YEAR(EDATE(TODAY(),0-1)))";
  private const string DefaultThisMonthTimePeriodFormula = "AND(MONTH({0})=MONTH(TODAY()),YEAR({0})=YEAR(TODAY()))";
  private const string DefaultNextMonthTimePeriodFormula = "AND(MONTH({0})=MONTH(EDATE(TODAY(),0+1)),YEAR({0})=YEAR(EDATE(TODAY(),0+1)))";
  private bool m_isFormula;
  private CFRecord m_formatRecord;
  private CFExRecord m_cfExRecord;
  private int m_startdxf;
  private CF12Record m_cf12Record;
  private WorkbookImpl m_book;
  private ColorObject m_color;
  private ColorObject m_backColor;
  private ColorObject m_topBorderColor;
  private ColorObject m_bottomBorderColor;
  private ColorObject m_leftBorderColor;
  private ColorObject m_rightBorderColor;
  private ColorObject m_fontColor;
  private DataBarImpl m_dataBar;
  private IconSetImpl m_iconSet;
  private ColorScaleImpl m_colorScale;
  private string m_asteriskRange;
  private IRange m_range;
  private string m_text;
  private string m_rangeReference;
  internal string m_customFunction = string.Empty;
  private bool m_cfHasExtensionList;
  private bool m_IsDxfPatternNone;
  internal string ST_GUID;
  private CFTimePeriods m_CFTimePeriod;
  private string m_CFRuleID;
  private TopBottomImpl m_topBottom;
  private AboveBelowAverageImpl m_aboveBelowAverage;
  private bool m_isNegativePriority;
  private bool m_isConditionalFormatCopying;

  public ConditionalFormatImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.SetParents();
    this.m_formatRecord = (CFRecord) BiffRecordFactory.GetRecord(TBIFFRecord.CF);
    this.InitializeColors();
    this.m_cf12Record = (CF12Record) BiffRecordFactory.GetRecord(TBIFFRecord.CF12);
    this.m_cfExRecord = (CFExRecord) BiffRecordFactory.GetRecord(TBIFFRecord.CFEx);
  }

  [CLSCompliant(false)]
  public ConditionalFormatImpl(
    IApplication application,
    object parent,
    BiffRecordRaw[] data,
    ref int iPos)
    : this(application, parent)
  {
    this.Parse(data, ref iPos);
  }

  [CLSCompliant(false)]
  public ConditionalFormatImpl(IApplication application, object parent, CFRecord cf)
    : this(application, parent)
  {
    this.m_formatRecord = (CFRecord) cf.Clone();
    this.ParseRecord();
  }

  [CLSCompliant(false)]
  public ConditionalFormatImpl(IApplication application, object parent, CF12Record cf12)
    : this(application, parent)
  {
    this.m_cf12Record = (CF12Record) cf12.Clone();
    this.UpdateCFRecordProperties(this.m_cf12Record.Properties, this);
  }

  [CLSCompliant(false)]
  public ConditionalFormatImpl(IApplication application, object parent, CFExRecord cfEx)
    : this(application, parent)
  {
    this.m_cfExRecord = (CFExRecord) cfEx.Clone();
    this.UpdateCFRecordProperties(this.m_cfExRecord.Properties, this);
  }

  [CLSCompliant(false)]
  public void Parse(BiffRecordRaw[] data, ref int iPos)
  {
    data[iPos].CheckTypeCode(TBIFFRecord.CF);
    this.m_formatRecord = (CFRecord) data[iPos];
    this.ParseRecord();
    data[iPos].CheckTypeCode(TBIFFRecord.CF12);
    this.m_cf12Record = (CF12Record) data[iPos];
    data[iPos].CheckTypeCode(TBIFFRecord.CFEx);
    this.m_cfExRecord = (CFExRecord) data[iPos];
    ++iPos;
  }

  private void InitializeColors()
  {
    this.m_color = new ColorObject(ExcelKnownColors.BlackCustom);
    this.m_color.AfterChange += new ColorObject.AfterChangeHandler(this.UpdateColor);
    this.m_backColor = new ColorObject(ExcelKnownColors.None);
    this.m_backColor.AfterChange += new ColorObject.AfterChangeHandler(this.UpdateBackColor);
    this.m_topBorderColor = new ColorObject(ExcelKnownColors.Black);
    this.m_topBorderColor.AfterChange += new ColorObject.AfterChangeHandler(this.UpdateTopBorderColor);
    this.m_bottomBorderColor = new ColorObject(ExcelKnownColors.Black);
    this.m_bottomBorderColor.AfterChange += new ColorObject.AfterChangeHandler(this.UpdateBottomBorderColor);
    this.m_leftBorderColor = new ColorObject(ExcelKnownColors.Black);
    this.m_leftBorderColor.AfterChange += new ColorObject.AfterChangeHandler(this.UpdateLeftBorderColor);
    this.m_rightBorderColor = new ColorObject(ExcelKnownColors.Black);
    this.m_rightBorderColor.AfterChange += new ColorObject.AfterChangeHandler(this.UpdateRightBorderColor);
    this.m_fontColor = new ColorObject(ExcelKnownColors.Black);
    this.m_fontColor.AfterChange += new ColorObject.AfterChangeHandler(this.UpdateFontColor);
  }

  private void UpdateColorObjects()
  {
    this.m_color.SetIndexedNoEvent((ExcelKnownColors) this.m_formatRecord.PatternColorIndex);
    this.m_backColor.SetIndexedNoEvent((ExcelKnownColors) this.m_formatRecord.PatternBackColor);
    this.m_topBorderColor.SetIndexedNoEvent((ExcelKnownColors) this.m_formatRecord.TopBorderColorIndex);
    this.m_bottomBorderColor.SetIndexedNoEvent((ExcelKnownColors) this.m_formatRecord.BottomBorderColorIndex);
    this.m_leftBorderColor.SetIndexedNoEvent((ExcelKnownColors) this.m_formatRecord.LeftBorderColorIndex);
    this.m_rightBorderColor.SetIndexedNoEvent((ExcelKnownColors) this.m_formatRecord.RightBorderColorIndex);
    this.m_fontColor.SetIndexedNoEvent((ExcelKnownColors) this.m_formatRecord.FontColorIndex);
  }

  private void ZeroRecordColors()
  {
  }

  private void ParseRecord() => this.UpdateColorObjects();

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    this.UpdateColors();
    records.Add((IBiffStorage) this.m_formatRecord);
    this.ZeroRecordColors();
  }

  [CLSCompliant(false)]
  public void SerializeCF12(OffsetArrayList records)
  {
    records.Add((IBiffStorage) this.m_cf12Record);
  }

  internal void UpdateFontColor()
  {
    this.m_formatRecord.FontColorIndex = (uint) (ushort) this.m_fontColor.GetIndexed((IWorkbook) this.m_book);
    this.m_formatRecord.IsFontFormatPresent = true;
  }

  internal void UpdateColor()
  {
    this.m_formatRecord.PatternColorIndex = (ushort) this.m_color.GetIndexed((IWorkbook) this.m_book);
    this.m_formatRecord.IsPatternColorModified = true;
    this.m_formatRecord.IsPatternFormatPresent = true;
  }

  internal void UpdateBackColor()
  {
    this.m_formatRecord.PatternBackColor = (ushort) this.m_backColor.GetIndexed((IWorkbook) this.m_book);
    this.m_formatRecord.IsPatternBackColorModified = true;
    this.m_formatRecord.IsPatternFormatPresent = true;
  }

  internal void UpdateLeftBorderColor()
  {
    this.m_formatRecord.LeftBorderColorIndex = (uint) this.m_leftBorderColor.GetIndexed((IWorkbook) this.m_book);
    this.IsLeftBorderModified = true;
  }

  internal void UpdateRightBorderColor()
  {
    this.m_formatRecord.RightBorderColorIndex = (uint) this.m_rightBorderColor.GetIndexed((IWorkbook) this.m_book);
    this.IsRightBorderModified = true;
  }

  internal void UpdateTopBorderColor()
  {
    this.m_formatRecord.TopBorderColorIndex = (uint) this.m_topBorderColor.GetIndexed((IWorkbook) this.m_book);
    this.IsTopBorderModified = true;
  }

  internal void UpdateBottomBorderColor()
  {
    this.m_formatRecord.BottomBorderColorIndex = (uint) this.m_bottomBorderColor.GetIndexed((IWorkbook) this.m_book);
    this.IsBottomBorderModified = true;
  }

  public void MarkUsedReferences(bool[] usedItems)
  {
    FormulaUtil.MarkUsedReferences(this.m_formatRecord.FirstFormulaPtgs, usedItems);
    FormulaUtil.MarkUsedReferences(this.m_formatRecord.SecondFormulaPtgs, usedItems);
  }

  public void UpdateReferenceIndexes(int[] arrUpdatedIndexes)
  {
    Ptg[] firstFormulaPtgs = this.m_formatRecord.FirstFormulaPtgs;
    if (FormulaUtil.UpdateReferenceIndexes(firstFormulaPtgs, arrUpdatedIndexes))
      this.m_formatRecord.FirstFormulaPtgs = firstFormulaPtgs;
    Ptg[] secondFormulaPtgs = this.m_formatRecord.SecondFormulaPtgs;
    if (!FormulaUtil.UpdateReferenceIndexes(secondFormulaPtgs, arrUpdatedIndexes))
      return;
    this.m_formatRecord.SecondFormulaPtgs = secondFormulaPtgs;
  }

  private void SetParents()
  {
    this.m_book = (WorkbookImpl) (this.FindParent(typeof (WorkbookImpl)) ?? throw new ArgumentNullException("Can't find parent workbook"));
  }

  internal void SetCFExRecord(CFExRecord cfEx)
  {
    this.m_cfExRecord = (CFExRecord) cfEx.Clone();
    this.UpdateCFRecordProperties(this.m_cfExRecord.Properties, this);
  }

  internal void UpdateCFRecordProperties(
    List<ExtendedProperty> properties,
    ConditionalFormatImpl format)
  {
    if (properties.Count <= 0)
      return;
    for (int index = 0; index < properties.Count; ++index)
    {
      ExtendedProperty property = properties[index];
      System.Drawing.Color argb = this.m_book.ConvertRGBAToARGB(this.m_book.UIntToColor(property.ColorValue));
      ColorType colorType = properties[index].ColorType;
      switch (properties[index].Type)
      {
        case CellPropertyExtensionType.ForeColor:
          if (colorType == ColorType.Theme)
          {
            property.Tint /= (double) short.MaxValue;
            format.ColorObject.SetTheme((int) property.ColorValue, (IWorkbook) this.Workbook, property.Tint);
          }
          if (colorType == ColorType.RGB)
          {
            format.ColorObject.ColorType = property.ColorType;
            format.ColorObject.SetRGB(argb);
            break;
          }
          break;
        case CellPropertyExtensionType.BackColor:
          switch (colorType)
          {
            case ColorType.RGB:
              format.BackColorObject.ColorType = property.ColorType;
              format.BackColorObject.SetRGB(argb);
              continue;
            case ColorType.Theme:
              property.Tint /= (double) short.MaxValue;
              format.BackColorObject.SetTheme((int) property.ColorValue, (IWorkbook) this.Workbook, property.Tint);
              continue;
            default:
              continue;
          }
        case CellPropertyExtensionType.TopBorderColor:
          if (colorType == ColorType.RGB)
          {
            format.TopBorderColorObject.ColorType = property.ColorType;
            format.TopBorderColorObject.SetRGB(argb);
            break;
          }
          break;
        case CellPropertyExtensionType.BottomBorderColor:
          if (colorType == ColorType.RGB)
          {
            format.BottomBorderColorObject.ColorType = property.ColorType;
            format.BottomBorderColorObject.SetRGB(argb);
            break;
          }
          break;
        case CellPropertyExtensionType.LeftBorderColor:
          if (colorType == ColorType.RGB)
          {
            format.LeftBorderColorObject.ColorType = property.ColorType;
            format.LeftBorderColorObject.SetRGB(argb);
            break;
          }
          break;
        case CellPropertyExtensionType.RightBorderColor:
          if (colorType == ColorType.RGB)
          {
            format.RightBorderColorObject.ColorType = property.ColorType;
            format.RightBorderColorObject.SetRGB(argb);
            break;
          }
          break;
        case CellPropertyExtensionType.TextColor:
          switch (colorType)
          {
            case ColorType.RGB:
              format.FontColorObject.ColorType = property.ColorType;
              format.FontColorObject.SetRGB(argb);
              continue;
            case ColorType.Theme:
              property.Tint /= (double) short.MaxValue;
              format.FontColorObject.SetTheme((int) property.ColorValue, (IWorkbook) this.Workbook, property.Tint);
              continue;
            default:
              continue;
          }
      }
    }
  }

  public ExcelKnownColors LeftBorderColor
  {
    get => this.m_leftBorderColor.GetIndexed((IWorkbook) this.m_book);
    set
    {
      value = BorderImpl.NormalizeColor(value);
      this.m_leftBorderColor.SetIndexed(value);
    }
  }

  public System.Drawing.Color LeftBorderColorRGB
  {
    get => this.m_leftBorderColor.GetRGB((IWorkbook) this.m_book);
    set => this.m_leftBorderColor.SetRGB(value, (IWorkbook) this.m_book);
  }

  public ExcelLineStyle LeftBorderStyle
  {
    get => this.m_formatRecord.LeftBorderStyle;
    set
    {
      this.m_formatRecord.LeftBorderStyle = value;
      this.IsLeftBorderModified = true;
      this.m_formatRecord.IsBorderFormatPresent = true;
    }
  }

  public ExcelKnownColors RightBorderColor
  {
    get => this.m_rightBorderColor.GetIndexed((IWorkbook) this.m_book);
    set
    {
      value = BorderImpl.NormalizeColor(value);
      this.m_rightBorderColor.SetIndexed(value);
    }
  }

  public System.Drawing.Color RightBorderColorRGB
  {
    get => this.m_rightBorderColor.GetRGB((IWorkbook) this.m_book);
    set => this.m_rightBorderColor.SetRGB(value, (IWorkbook) this.m_book);
  }

  public ExcelLineStyle RightBorderStyle
  {
    get => this.m_formatRecord.RightBorderStyle;
    set
    {
      this.m_formatRecord.RightBorderStyle = value;
      this.IsRightBorderModified = true;
      this.m_formatRecord.IsBorderFormatPresent = true;
    }
  }

  public ExcelKnownColors TopBorderColor
  {
    get => this.m_topBorderColor.GetIndexed((IWorkbook) this.m_book);
    set
    {
      value = BorderImpl.NormalizeColor(value);
      this.m_topBorderColor.SetIndexed(value);
    }
  }

  public System.Drawing.Color TopBorderColorRGB
  {
    get => this.m_topBorderColor.GetRGB((IWorkbook) this.m_book);
    set => this.m_topBorderColor.SetRGB(value, (IWorkbook) this.m_book);
  }

  public ExcelLineStyle TopBorderStyle
  {
    get => this.m_formatRecord.TopBorderStyle;
    set
    {
      this.m_formatRecord.TopBorderStyle = value;
      this.IsTopBorderModified = true;
      this.m_formatRecord.IsBorderFormatPresent = true;
    }
  }

  public ExcelKnownColors BottomBorderColor
  {
    get => this.m_bottomBorderColor.GetIndexed((IWorkbook) this.m_book);
    set
    {
      value = BorderImpl.NormalizeColor(value);
      this.m_bottomBorderColor.SetIndexed(value);
    }
  }

  public System.Drawing.Color BottomBorderColorRGB
  {
    get => this.m_bottomBorderColor.GetRGB((IWorkbook) this.m_book);
    set => this.m_bottomBorderColor.SetRGB(value, (IWorkbook) this.m_book);
  }

  public ExcelLineStyle BottomBorderStyle
  {
    get => this.m_formatRecord.BottomBorderStyle;
    set
    {
      this.m_formatRecord.BottomBorderStyle = value;
      this.IsBottomBorderModified = true;
      this.m_formatRecord.IsBorderFormatPresent = true;
    }
  }

  public string FirstFormula
  {
    get => this.GetCondtionalFormatForumula(true);
    set
    {
      if (value[0] == '=')
        value = value.Substring(1);
      if (this.Workbook.Version >= ExcelVersion.Excel2010 && this.Text == null && !this.CFHasExtensionList && this.FormatType == ExcelCFType.SpecificText && value.Length < 3)
      {
        string str = value.ToString();
        string empty = string.Empty;
        RangeImpl range = (this.Parent as ConditionalFormats).sheet.Range[str.ToString()] as RangeImpl;
        this.m_range = (IRange) range;
        value = this.SetSpecificTextFormula(this.Operator, range);
      }
      if (!(this.FirstFormula != value))
        return;
      DataValidationImpl.RegisterFunctions(true);
      DataValidationImpl.RegisterFunctions(true);
      Ptg[] ptgs = this.m_book.FormulaUtil.ParseString(value);
      DataValidationImpl.RegisterFunctions(false);
      DataValidationImpl.RegisterFunctions(false);
      Ptg[] nptg = this.ConvertPtgToNPtg(ptgs);
      this.m_formatRecord.FirstFormulaPtgs = nptg;
      if (this.FormatType == ExcelCFType.ColorScale || this.FormatType == ExcelCFType.DataBar || this.FormatType == ExcelCFType.IconSet)
        return;
      this.m_cf12Record.FirstFormulaPtgs = nptg;
    }
  }

  internal bool IsFormula
  {
    get => this.m_isFormula;
    set => this.m_isFormula = value;
  }

  internal Ptg[] ConvertPtgToNPtg(Ptg[] ptgs)
  {
    if (ptgs != null)
    {
      for (int index = 0; index < ptgs.Length; ++index)
      {
        if (ptgs[index] is RefPtg)
        {
          RefPtg ptg = ptgs[index] as RefPtg;
          if (ptg.IsRowIndexRelative || ptg.IsColumnIndexRelative)
          {
            IRange range = (this.FindParent(typeof (WorksheetImpl)) as WorksheetImpl)[(this.Parent as ConditionalFormats).GetEnclosedRangeAddress(true)];
            if (!(ptgs[index] is RefNPtg))
            {
              ptgs[index] = ptg.ConvertPtgToNPtg((IWorkbook) this.Workbook, range.Row - 1, range.Column - 1);
            }
            else
            {
              if (ptg.IsRowIndexRelative)
                (ptgs[index] as RefNPtg).RowIndex -= range.Row - 1;
              if (ptg.IsColumnIndexRelative)
                (ptgs[index] as RefNPtg).ColumnIndex -= range.Column - 1;
            }
          }
        }
        else if (ptgs[index] is AreaPtg)
        {
          AreaPtg ptg = ptgs[index] as AreaPtg;
          if (ptg.IsFirstRowRelative || ptg.IsLastRowRelative || ptg.IsFirstColumnRelative || ptg.IsLastColumnRelative)
          {
            IRange range = (this.FindParent(typeof (WorksheetImpl)) as WorksheetImpl)[(this.Parent as ConditionalFormats).GetEnclosedRangeAddress(true)];
            if (!(ptgs[index] is AreaNPtg))
            {
              ptgs[index] = ptg.ConvertPtgToNPtg((IWorkbook) this.Workbook, range.Row - 1, range.Column - 1);
            }
            else
            {
              (ptgs[index] as AreaPtg).FirstRow -= range.Row - 1;
              (ptgs[index] as AreaPtg).LastRow -= range.Row - 1;
            }
          }
        }
      }
    }
    return ptgs;
  }

  private string GetCondtionalFormatForumula(bool isFirstOrSecond)
  {
    string condtionalFormatForumula = this.GetFirstSecondFormula(this.m_book.FormulaUtil, isFirstOrSecond);
    if (condtionalFormatForumula != null && condtionalFormatForumula.Contains("_xlfn."))
      condtionalFormatForumula = condtionalFormatForumula.Replace("_xlfn.", string.Empty);
    return condtionalFormatForumula;
  }

  public string FirstFormulaR1C1
  {
    get
    {
      Ptg[] firstFormulaPtgs = this.m_formatRecord.FirstFormulaPtgs;
      if (firstFormulaPtgs == null)
        return (string) null;
      ConditionalFormats parent = (ConditionalFormats) this.Parent;
      return this.m_book.FormulaUtil.ParsePtgArray(firstFormulaPtgs, parent.EnclosedRange.FirstRow, parent.EnclosedRange.FirstCol, true, false);
    }
    set
    {
      if (value[0] == '=')
        value = value.Substring(1);
      if (!(this.FirstFormulaR1C1 != value))
        return;
      DataValidationImpl.RegisterFunctions(true);
      Ptg[] ptgArray;
      if (this.m_range == null)
      {
        ConditionalFormats parent = (ConditionalFormats) this.Parent;
        ptgArray = this.m_book.FormulaUtil.ParseString(value, (IWorksheet) (this.FindParent(typeof (WorksheetImpl)) as WorksheetImpl), (Dictionary<string, string>) null, parent.CellRectangles[0].Y, parent.CellRectangles[0].X, true);
      }
      else
        ptgArray = this.m_book.FormulaUtil.ParseString(value, this.m_range.Worksheet, (Dictionary<string, string>) null, this.m_range.Row - 1, this.m_range.Column - 1, true);
      DataValidationImpl.RegisterFunctions(false);
      this.m_formatRecord.FirstFormulaPtgs = ptgArray;
    }
  }

  public string SecondFormula
  {
    get => this.GetCondtionalFormatForumula(false);
    set
    {
      if (value[0] == '=')
        value = value.Substring(1);
      if (!(this.SecondFormula != value))
        return;
      Ptg[] nptg = this.ConvertPtgToNPtg(this.m_book.FormulaUtil.ParseString(value));
      this.m_formatRecord.SecondFormulaPtgs = nptg;
      if (this.FormatType == ExcelCFType.ColorScale || this.FormatType == ExcelCFType.DataBar || this.FormatType == ExcelCFType.IconSet)
        return;
      this.m_cf12Record.SecondFormulaPtgs = nptg;
    }
  }

  public string SecondFormulaR1C1
  {
    get
    {
      Ptg[] secondFormulaPtgs = this.m_formatRecord.SecondFormulaPtgs;
      return secondFormulaPtgs == null ? (string) null : this.m_book.FormulaUtil.ParsePtgArray(secondFormulaPtgs, 0, 0, true, false);
    }
    set
    {
      if (value[0] == '=')
        value = value.Substring(1);
      if (!(this.SecondFormulaR1C1 != value))
        return;
      Ptg[] ptgArray;
      if (this.m_range == null)
      {
        ConditionalFormats parent = (ConditionalFormats) this.Parent;
        ptgArray = this.m_book.FormulaUtil.ParseString(value, (IWorksheet) (this.FindParent(typeof (WorksheetImpl)) as WorksheetImpl), (Dictionary<string, string>) null, parent.CellRectangles[0].Y, parent.CellRectangles[0].X, true);
      }
      else
        ptgArray = this.m_book.FormulaUtil.ParseString(value, this.m_range.Worksheet, (Dictionary<string, string>) null, this.m_range.Row - 1, this.m_range.Column - 1, true);
      this.m_formatRecord.SecondFormulaPtgs = ptgArray;
    }
  }

  public ExcelCFType FormatType
  {
    get => this.m_formatRecord.FormatType;
    set
    {
      if (this.m_formatRecord.FormatType == value)
        return;
      this.m_formatRecord.FormatType = value;
      this.m_dataBar = (DataBarImpl) null;
      this.m_iconSet = (IconSetImpl) null;
      this.m_colorScale = (ColorScaleImpl) null;
      this.m_topBottom = (TopBottomImpl) null;
      this.m_aboveBelowAverage = (AboveBelowAverageImpl) null;
      switch (value)
      {
        case ExcelCFType.CellValue:
          this.Operator = ExcelComparisonOperator.Between;
          break;
        case ExcelCFType.Formula:
          this.Operator = ExcelComparisonOperator.None;
          break;
        case ExcelCFType.ColorScale:
          this.m_colorScale = new ColorScaleImpl();
          if (this.CF12Record.FormatType != ExcelCFType.ColorScale)
            break;
          this.m_colorScale = (ColorScaleImpl) this.CF12Record.ColorScaleCF12.ColorScaleImpl;
          break;
        case ExcelCFType.DataBar:
          this.m_dataBar = new DataBarImpl();
          if (this.CF12Record.FormatType == ExcelCFType.DataBar)
            this.m_dataBar = (DataBarImpl) this.CF12Record.DataBarCF12.DataBarImpl;
          this.m_dataBar.HasExtensionList = true;
          break;
        case ExcelCFType.IconSet:
          this.m_iconSet = new IconSetImpl();
          if (this.CF12Record.FormatType != ExcelCFType.IconSet)
            break;
          this.m_iconSet = (IconSetImpl) this.CF12Record.IconSetCF12.IconsetImpl;
          break;
        case ExcelCFType.Blank:
          ConditionalFormats parent1 = this.Parent as ConditionalFormats;
          this.FirstFormula = $"LEN(TRIM({parent1.GetEnclosedRangeAddress(parent1.CellsList.Length > 1)}))=0";
          break;
        case ExcelCFType.NoBlank:
          ConditionalFormats parent2 = this.Parent as ConditionalFormats;
          this.FirstFormula = $"LEN(TRIM({parent2.GetEnclosedRangeAddress(parent2.CellsList.Length > 1)}))>0";
          break;
        case ExcelCFType.SpecificText:
          this.Operator = ExcelComparisonOperator.ContainsText;
          break;
        case ExcelCFType.ContainsErrors:
          ConditionalFormats parent3 = this.Parent as ConditionalFormats;
          this.FirstFormula = $"ISERROR({parent3.GetEnclosedRangeAddress(parent3.CellsList.Length > 1)})";
          break;
        case ExcelCFType.NotContainsErrors:
          ConditionalFormats parent4 = this.Parent as ConditionalFormats;
          this.FirstFormula = $"NOT(ISERROR({parent4.GetEnclosedRangeAddress(parent4.CellsList.Length > 1)}))";
          break;
        case ExcelCFType.TimePeriod:
          this.Operator = ExcelComparisonOperator.None;
          this.FirstFormula = this.SetTimePeriodFormula(this.m_CFTimePeriod);
          break;
        case ExcelCFType.TopBottom:
          this.m_topBottom = new TopBottomImpl();
          if (this.CF12Record.FormatType == ExcelCFType.TopBottom)
          {
            this.m_topBottom = (TopBottomImpl) this.CF12Record.TopBottomCF12.TopBottom;
            break;
          }
          if (this.CFExRecord.Template != ConditionalFormatTemplate.Filter)
            break;
          this.m_topBottom = (TopBottomImpl) this.CFExRecord.TopBottomCFEx.TopBottom;
          break;
        case ExcelCFType.AboveBelowAverage:
          this.m_aboveBelowAverage = new AboveBelowAverageImpl();
          if (this.CF12Record.FormatType == ExcelCFType.AboveBelowAverage)
            this.m_aboveBelowAverage = (AboveBelowAverageImpl) this.CF12Record.AboveBelowAverageCF12.AboveBelowAverage;
          if (this.CFExRecord.Template != ConditionalFormatTemplate.AboveAverage && this.CFExRecord.Template != ConditionalFormatTemplate.AboveOrEqualToAverage && this.CFExRecord.Template != ConditionalFormatTemplate.BelowAverage && this.CFExRecord.Template != ConditionalFormatTemplate.BelowOrEqualToAverage)
            break;
          this.m_aboveBelowAverage = (AboveBelowAverageImpl) this.CFExRecord.AboveBelowAverageCFEx.AboveBelowAverage;
          break;
      }
    }
  }

  public CFTimePeriods TimePeriodType
  {
    get => this.m_CFTimePeriod;
    set
    {
      if (this.FormatType == ExcelCFType.TimePeriod)
        this.m_CFTimePeriod = value;
      this.FirstFormula = this.SetTimePeriodFormula(this.m_CFTimePeriod);
    }
  }

  internal string CFRuleID
  {
    get => this.m_CFRuleID;
    set => this.m_CFRuleID = value;
  }

  public ExcelComparisonOperator Operator
  {
    get => this.m_formatRecord.ComparisonOperator;
    set => this.m_formatRecord.ComparisonOperator = value;
  }

  public bool IsBold
  {
    get => this.m_formatRecord.FontWeight >= (ushort) 700;
    set
    {
      this.m_formatRecord.FontWeight = !value ? (ushort) 400 : (ushort) 700;
      this.m_formatRecord.IsFontFormatPresent = true;
      this.m_formatRecord.IsFontStyleModified = true;
    }
  }

  public bool IsItalic
  {
    get => this.m_formatRecord.FontPosture;
    set
    {
      this.m_formatRecord.FontPosture = value;
      this.m_formatRecord.IsFontFormatPresent = true;
      this.m_formatRecord.IsFontStyleModified = true;
    }
  }

  public ExcelKnownColors FontColor
  {
    get => this.m_fontColor.GetIndexed((IWorkbook) this.m_book);
    set => this.m_fontColor.SetIndexed(value);
  }

  public System.Drawing.Color FontColorRGB
  {
    get => this.m_fontColor.GetRGB((IWorkbook) this.m_book);
    set => this.m_fontColor.SetRGB(value);
  }

  public ExcelUnderline Underline
  {
    get => this.m_formatRecord.FontUnderline;
    set
    {
      this.m_formatRecord.FontUnderline = value;
      this.m_formatRecord.IsFontFormatPresent = true;
      this.m_formatRecord.IsFontUnderlineModified = true;
    }
  }

  public bool IsStrikeThrough
  {
    get => this.m_formatRecord.FontCancellation;
    set
    {
      this.m_formatRecord.FontCancellation = value;
      this.m_formatRecord.IsFontFormatPresent = true;
      this.m_formatRecord.IsFontCancellationModified = true;
    }
  }

  public bool IsSuperScript
  {
    get => this.m_formatRecord.FontEscapment == ExcelFontVertialAlignment.Superscript;
    set
    {
      if (value)
        this.m_formatRecord.FontEscapment = ExcelFontVertialAlignment.Superscript;
      else if (this.IsSuperScript)
        this.m_formatRecord.FontEscapment = ExcelFontVertialAlignment.Baseline;
      this.m_formatRecord.IsFontEscapmentModified = true;
      this.m_formatRecord.IsFontFormatPresent = true;
    }
  }

  public bool IsSubScript
  {
    get => this.m_formatRecord.FontEscapment == ExcelFontVertialAlignment.Subscript;
    set
    {
      if (value)
        this.m_formatRecord.FontEscapment = ExcelFontVertialAlignment.Subscript;
      else if (this.IsSubScript)
        this.m_formatRecord.FontEscapment = ExcelFontVertialAlignment.Baseline;
      this.m_formatRecord.IsFontEscapmentModified = true;
      this.m_formatRecord.IsFontFormatPresent = true;
    }
  }

  public ExcelKnownColors Color
  {
    get => this.m_color.GetIndexed((IWorkbook) this.m_book);
    set => this.m_color.SetIndexed(value);
  }

  public System.Drawing.Color ColorRGB
  {
    get => this.m_color.GetRGB((IWorkbook) this.m_book);
    set => this.m_color.SetRGB(value, (IWorkbook) this.m_book);
  }

  public ExcelKnownColors BackColor
  {
    get => this.m_backColor.GetIndexed((IWorkbook) this.m_book);
    set => this.m_backColor.SetIndexed(value);
  }

  public System.Drawing.Color BackColorRGB
  {
    get => this.m_backColor.GetRGB((IWorkbook) this.m_book);
    set => this.m_backColor.SetRGB(value, (IWorkbook) this.m_book);
  }

  public ExcelPattern FillPattern
  {
    get => this.m_formatRecord.PatternStyle;
    set
    {
      if (value == this.m_formatRecord.PatternStyle)
        return;
      this.m_formatRecord.PatternStyle = value;
      this.m_formatRecord.IsPatternStyleModified = true;
      this.m_formatRecord.IsPatternFormatPresent = true;
    }
  }

  public bool IsFontFormatPresent
  {
    get => this.m_formatRecord.IsFontFormatPresent;
    set => this.m_formatRecord.IsFontFormatPresent = value;
  }

  public bool IsBorderFormatPresent
  {
    get => this.m_formatRecord.IsBorderFormatPresent;
    set => this.m_formatRecord.IsBorderFormatPresent = value;
  }

  public bool IsPatternFormatPresent
  {
    get => this.m_formatRecord.IsPatternFormatPresent;
    set => this.m_formatRecord.IsPatternFormatPresent = value;
  }

  public bool IsFontColorPresent
  {
    get => this.m_formatRecord.FontColorIndex != uint.MaxValue;
    set
    {
      if (value)
        this.m_formatRecord.FontColorIndex = 0U;
      else
        this.m_formatRecord.FontColorIndex = uint.MaxValue;
    }
  }

  public bool IsPatternColorPresent
  {
    get => this.m_formatRecord.IsPatternColorModified;
    set
    {
      this.m_formatRecord.IsPatternColorModified = value;
      if (!value)
        return;
      this.m_formatRecord.IsPatternFormatPresent = value;
    }
  }

  public bool IsBackgroundColorPresent
  {
    get => this.m_formatRecord.IsPatternBackColorModified;
    set
    {
      this.m_formatRecord.IsPatternBackColorModified = value;
      if (!value)
        return;
      this.m_formatRecord.IsPatternFormatPresent = value;
    }
  }

  public bool HasNumberFormatPresent
  {
    get => this.m_formatRecord.IsNumberFormatModified;
    set
    {
      this.m_formatRecord.IsNumberFormatModified = value;
      if (!value)
        return;
      this.m_formatRecord.IsNumberFormatPresent = value;
    }
  }

  internal bool CFHasExtensionList
  {
    get
    {
      if (this.ST_GUID == null)
        this.ST_GUID = $"{{{Guid.NewGuid().ToString()}}}";
      return this.m_cfHasExtensionList;
    }
    set
    {
      this.m_cfHasExtensionList = value;
      if (this.ST_GUID != null)
        return;
      this.ST_GUID = $"{{{Guid.NewGuid().ToString()}}}";
    }
  }

  public bool IsLeftBorderModified
  {
    get => this.m_formatRecord.IsLeftBorderModified;
    set
    {
      this.m_formatRecord.IsLeftBorderModified = value;
      if (!value)
        return;
      this.m_formatRecord.IsBorderFormatPresent = value;
    }
  }

  public bool IsRightBorderModified
  {
    get => this.m_formatRecord.IsRightBorderModified;
    set
    {
      this.m_formatRecord.IsRightBorderModified = value;
      if (!value)
        return;
      this.m_formatRecord.IsBorderFormatPresent = value;
    }
  }

  public bool IsTopBorderModified
  {
    get => this.m_formatRecord.IsTopBorderModified;
    set
    {
      this.m_formatRecord.IsTopBorderModified = value;
      if (!value)
        return;
      this.m_formatRecord.IsBorderFormatPresent = value;
    }
  }

  public bool IsBottomBorderModified
  {
    get => this.m_formatRecord.IsBottomBorderModified;
    set
    {
      this.m_formatRecord.IsBottomBorderModified = value;
      if (!value)
        return;
      this.m_formatRecord.IsBorderFormatPresent = value;
    }
  }

  public ushort NumberFormatIndex
  {
    get => this.m_formatRecord.NumberFormatIndex;
    set
    {
      if (!this.m_book.InnerFormats.Contains((int) value))
        throw new ArgumentOutOfRangeException("Unknown format index");
      this.HasNumberFormatPresent = true;
      this.m_formatRecord.NumberFormatIndex = value;
    }
  }

  public string NumberFormat
  {
    get => this.m_book.InnerFormats[(int) this.NumberFormatIndex].FormatString;
    set => this.NumberFormatIndex = (ushort) this.m_book.InnerFormats.FindOrCreateFormat(value);
  }

  public string Text
  {
    get => this.m_text;
    set
    {
      this.m_text = value != null && !(value == string.Empty) ? value : throw new ArgumentNullException("Argument cannot be null or empty.");
      if (this.FormatType != ExcelCFType.SpecificText)
        return;
      this.SetSpecificTextString(this.Operator, value);
    }
  }

  internal string AsteriskRange
  {
    get => this.m_asteriskRange;
    set => this.m_asteriskRange = value;
  }

  public bool StopIfTrue
  {
    get => this.m_cfExRecord.StopIfTrue;
    set => this.m_cfExRecord.StopIfTrue = value;
  }

  internal int StartDxf
  {
    get => this.m_startdxf;
    set => this.m_startdxf = value;
  }

  internal bool IsNegativePriority => this.m_isNegativePriority;

  internal bool IsConditionalFormatCopying
  {
    get => this.m_isConditionalFormatCopying;
    set => this.m_isConditionalFormatCopying = value;
  }

  internal int Priority
  {
    get => (int) this.m_cfExRecord.Priority;
    set
    {
      this.m_isNegativePriority = value == -1;
      this.m_cfExRecord.Priority = (ushort) value;
    }
  }

  public ConditionalFormatTemplate Template
  {
    get => this.m_cf12Record.Template;
    set => this.m_cf12Record.Template = value;
  }

  public IDataBar DataBar => (IDataBar) this.m_dataBar;

  public IIconSet IconSet => (IIconSet) this.m_iconSet;

  public IColorScale ColorScale => (IColorScale) this.m_colorScale;

  public ITopBottom TopBottom => (ITopBottom) this.m_topBottom;

  public IAboveBelowAverage AboveBelowAverage => (IAboveBelowAverage) this.m_aboveBelowAverage;

  [CLSCompliant(false)]
  public CFRecord Record => this.m_formatRecord;

  [CLSCompliant(false)]
  public CF12Record CF12Record => this.m_cf12Record;

  [CLSCompliant(false)]
  public CFExRecord CFExRecord => this.m_cfExRecord;

  public WorkbookImpl Workbook => this.m_book;

  internal DataBarImpl InnerDataBar => this.m_dataBar;

  internal string RangeRefernce
  {
    get => this.m_rangeReference;
    set => this.m_rangeReference = value;
  }

  internal IRange Range
  {
    get => this.m_range;
    set => this.m_range = value;
  }

  internal bool IsDxfPatternNone
  {
    get => this.m_IsDxfPatternNone;
    set => this.m_IsDxfPatternNone = value;
  }

  public void SetSpecificTextString(ExcelComparisonOperator compOperator, string value)
  {
    ConditionalFormats parent = this.Parent as ConditionalFormats;
    this.m_cfExRecord.Template = ConditionalFormatTemplate.ContainsText;
    this.AppImplementation.IsFormulaParsed = false;
    this.m_book.SetSeparators(Convert.ToChar(this.AppImplementation.CheckAndApplySeperators().TextInfo.ListSeparator), this.AppImplementation.RowSeparator);
    switch (compOperator)
    {
      case ExcelComparisonOperator.BeginsWith:
        this.FirstFormula = string.Format("LEFT({0},LEN({1}))={1}", (object) parent.GetEnclosedRangeAddress(parent.CellsList.Length > 1), (object) $"\"{value.ToString()}\"");
        this.m_cfExRecord.m_cfExTextParam.TextRuleType = CFTextRuleType.TextBeginsWith;
        break;
      case ExcelComparisonOperator.ContainsText:
        if (this.Workbook.Version == ExcelVersion.Excel97to2003 && parent.EnclosedRange.LastRow + 1 > 65522)
        {
          this.FirstFormula = $"NOT(ISERROR(SEARCH({$"\"{value.ToString()}\""},{RangeImpl.GetAddressLocal(parent.EnclosedRange.FirstRow + 1, parent.EnclosedRange.FirstCol + 1, parent.EnclosedRange.FirstRow + 1, parent.EnclosedRange.FirstCol + 1)})))";
          this.IsFormula = true;
        }
        else
          this.FirstFormula = $"NOT(ISERROR(SEARCH({$"\"{value.ToString()}\""},{parent.GetEnclosedRangeAddress(parent.CellsList.Length > 1)})))";
        this.m_cfExRecord.m_cfExTextParam.TextRuleType = CFTextRuleType.TextContains;
        break;
      case ExcelComparisonOperator.EndsWith:
        this.FirstFormula = string.Format("RIGHT({0},LEN({1}))={1}", (object) parent.GetEnclosedRangeAddress(parent.CellsList.Length > 1), (object) $"\"{value.ToString()}\"");
        this.m_cfExRecord.m_cfExTextParam.TextRuleType = CFTextRuleType.TextEndsWith;
        break;
      case ExcelComparisonOperator.NotContainsText:
        if (this.Workbook.Version == ExcelVersion.Excel97to2003 && parent.EnclosedRange.LastRow + 1 > 65522)
        {
          this.FirstFormula = $"ISERROR(SEARCH({$"\"{value.ToString()}\""},{RangeImpl.GetAddressLocal(parent.EnclosedRange.FirstRow + 1, parent.EnclosedRange.FirstCol + 1, parent.EnclosedRange.FirstRow + 1, parent.EnclosedRange.FirstCol + 1)}))";
          this.IsFormula = true;
        }
        else
          this.FirstFormula = $"ISERROR(SEARCH({$"\"{value.ToString()}\""},{parent.GetEnclosedRangeAddress(parent.CellsList.Length > 1)}))";
        this.m_cfExRecord.m_cfExTextParam.TextRuleType = CFTextRuleType.TextNotContains;
        break;
      default:
        this.Operator = ExcelComparisonOperator.ContainsText;
        this.FirstFormula = $"NOT(ISERROR(SEARCH({$"\"{value.ToString()}\""},{parent.GetEnclosedRangeAddress(parent.CellsList.Length > 1)})))";
        this.m_cfExRecord.m_cfExTextParam.TextRuleType = CFTextRuleType.TextContains;
        break;
    }
    this.AppImplementation.IsFormulaParsed = true;
    this.m_book.SetSeparators(Convert.ToChar(this.AppImplementation.CheckAndApplySeperators().TextInfo.ListSeparator), this.AppImplementation.RowSeparator);
  }

  public string SetSpecificTextFormula(ExcelComparisonOperator compOperator, RangeImpl range)
  {
    ConditionalFormats parent = this.Parent as ConditionalFormats;
    string empty = string.Empty;
    this.AppImplementation.IsFormulaParsed = false;
    this.m_book.SetSeparators(Convert.ToChar(this.AppImplementation.CheckAndApplySeperators().TextInfo.ListSeparator), this.AppImplementation.RowSeparator);
    string str;
    switch (compOperator)
    {
      case ExcelComparisonOperator.BeginsWith:
        str = string.Format("LEFT({0},LEN({1}))={1}", (object) parent.GetEnclosedRangeAddress(parent.CellsList.Length > 1), (object) range.AddressGlobalWithoutSheetName.ToString());
        this.CFHasExtensionList = true;
        break;
      case ExcelComparisonOperator.ContainsText:
        str = $"NOT(ISERROR(SEARCH({range.AddressGlobalWithoutSheetName.ToString()},{parent.GetEnclosedRangeAddress(parent.CellsList.Length > 1)})))";
        this.CFHasExtensionList = true;
        break;
      case ExcelComparisonOperator.EndsWith:
        str = string.Format("RIGHT({0},LEN({1}))={1}", (object) parent.GetEnclosedRangeAddress(parent.CellsList.Length > 1), (object) range.AddressGlobalWithoutSheetName.ToString());
        this.CFHasExtensionList = true;
        break;
      case ExcelComparisonOperator.NotContainsText:
        str = $"ISERROR(SEARCH({range.AddressGlobalWithoutSheetName.ToString()},{parent.GetEnclosedRangeAddress(parent.CellsList.Length > 1)}))";
        this.CFHasExtensionList = true;
        break;
      default:
        throw new ArgumentException("Invalid Opertor:", nameof (compOperator));
    }
    this.AppImplementation.IsFormulaParsed = true;
    this.m_book.SetSeparators(Convert.ToChar(this.AppImplementation.CheckAndApplySeperators().TextInfo.ListSeparator), this.AppImplementation.RowSeparator);
    return str;
  }

  private string SetTimePeriodFormula(CFTimePeriods cfTimePeriods)
  {
    string empty = string.Empty;
    ConditionalFormats parent = this.Parent as ConditionalFormats;
    string str;
    switch (cfTimePeriods)
    {
      case CFTimePeriods.Today:
        str = $"FLOOR({parent.GetEnclosedRangeAddress(parent.CellsList.Length > 1)},1)=TODAY()";
        this.m_cfExRecord.m_cfExDateParam.DateComparisonOperator = (ushort) 15;
        this.m_cfExRecord.Template = ConditionalFormatTemplate.Today;
        break;
      case CFTimePeriods.Yesterday:
        str = $"FLOOR({parent.GetEnclosedRangeAddress(parent.CellsList.Length > 1)},1)=TODAY()-1";
        this.m_cfExRecord.m_cfExDateParam.DateComparisonOperator = (ushort) 17;
        this.m_cfExRecord.Template = ConditionalFormatTemplate.Yesterday;
        break;
      case CFTimePeriods.Tomorrow:
        str = $"FLOOR({parent.GetEnclosedRangeAddress(parent.CellsList.Length > 1)},1)=TODAY()+1";
        this.m_cfExRecord.m_cfExDateParam.DateComparisonOperator = (ushort) 16 /*0x10*/;
        this.m_cfExRecord.Template = ConditionalFormatTemplate.Tomorrow;
        break;
      case CFTimePeriods.Last7Days:
        str = string.Format("AND(TODAY()-FLOOR({0},1)<=6,FLOOR({0},1)<=TODAY())", (object) parent.GetEnclosedRangeAddress(parent.CellsList.Length > 1));
        this.m_cfExRecord.m_cfExDateParam.DateComparisonOperator = (ushort) 18;
        this.m_cfExRecord.Template = ConditionalFormatTemplate.Last7Days;
        break;
      case CFTimePeriods.ThisMonth:
        str = string.Format("AND(MONTH({0})=MONTH(TODAY()),YEAR({0})=YEAR(TODAY()))", (object) parent.GetEnclosedRangeAddress(parent.CellsList.Length > 1));
        this.m_cfExRecord.m_cfExDateParam.DateComparisonOperator = (ushort) 24;
        this.m_cfExRecord.Template = ConditionalFormatTemplate.ThisMonth;
        break;
      case CFTimePeriods.LastMonth:
        str = string.Format("AND(MONTH({0})=MONTH(EDATE(TODAY(),0-1)),YEAR({0})=YEAR(EDATE(TODAY(),0-1)))", (object) parent.GetEnclosedRangeAddress(parent.CellsList.Length > 1));
        this.m_cfExRecord.m_cfExDateParam.DateComparisonOperator = (ushort) 19;
        this.m_cfExRecord.Template = ConditionalFormatTemplate.LastMonth;
        break;
      case CFTimePeriods.NextMonth:
        str = string.Format("AND(MONTH({0})=MONTH(EDATE(TODAY(),0+1)),YEAR({0})=YEAR(EDATE(TODAY(),0+1)))", (object) parent.GetEnclosedRangeAddress(parent.CellsList.Length > 1));
        this.m_cfExRecord.m_cfExDateParam.DateComparisonOperator = (ushort) 20;
        this.m_cfExRecord.Template = ConditionalFormatTemplate.NextMonth;
        break;
      case CFTimePeriods.ThisWeek:
        str = string.Format("AND(TODAY()-ROUNDDOWN({0},0)<=WEEKDAY(TODAY())-1,ROUNDDOWN({0},0)-TODAY()<=7-WEEKDAY(TODAY()))", (object) parent.GetEnclosedRangeAddress(parent.CellsList.Length > 1));
        this.m_cfExRecord.m_cfExDateParam.DateComparisonOperator = (ushort) 21;
        this.m_cfExRecord.Template = ConditionalFormatTemplate.ThisWeek;
        break;
      case CFTimePeriods.LastWeek:
        str = string.Format("AND(TODAY()-ROUNDDOWN({0},0)>=(WEEKDAY(TODAY())),TODAY()-ROUNDDOWN({0},0)<(WEEKDAY(TODAY())+7))", (object) parent.GetEnclosedRangeAddress(parent.CellsList.Length > 1));
        this.m_cfExRecord.m_cfExDateParam.DateComparisonOperator = (ushort) 23;
        this.m_cfExRecord.Template = ConditionalFormatTemplate.LastWeek;
        break;
      case CFTimePeriods.NextWeek:
        str = string.Format("AND(ROUNDDOWN({0},0)-TODAY()>(7-WEEKDAY(TODAY())),ROUNDDOWN({0},0)-TODAY()<(15-WEEKDAY(TODAY())))", (object) parent.GetEnclosedRangeAddress(parent.CellsList.Length > 1));
        this.m_cfExRecord.m_cfExDateParam.DateComparisonOperator = (ushort) 22;
        this.m_cfExRecord.Template = ConditionalFormatTemplate.NextWeek;
        break;
      default:
        throw new ArgumentException("Invalid time period type:", nameof (cfTimePeriods));
    }
    return str;
  }

  public void SetFirstSecondFormula(
    FormulaUtil formulaUtil,
    string strFormula,
    bool bIsFirstFormula)
  {
    bool isFormulaParsed = this.AppImplementation.IsFormulaParsed;
    this.AppImplementation.IsFormulaParsed = false;
    this.m_book.SetSeparators(Convert.ToChar(this.AppImplementation.CheckAndApplySeperators().TextInfo.ListSeparator), this.AppImplementation.RowSeparator);
    IWorksheet sheet = (IWorksheet) null;
    if (this.Parent is ConditionalFormats parent1 && parent1.Parent is WorksheetConditionalFormats parent2)
      sheet = (IWorksheet) (parent2.Parent as WorksheetImpl);
    Ptg[] ptgArray = sheet == null ? formulaUtil.ParseString(strFormula) : formulaUtil.ParseString(strFormula, sheet, (Dictionary<string, string>) null);
    if (bIsFirstFormula)
      this.m_formatRecord.FirstFormulaPtgs = ptgArray;
    else
      this.m_formatRecord.SecondFormulaPtgs = ptgArray;
    this.AppImplementation.IsFormulaParsed = isFormulaParsed;
    this.m_book.SetSeparators(Convert.ToChar(this.AppImplementation.CheckAndApplySeperators().TextInfo.ListSeparator), this.AppImplementation.RowSeparator);
  }

  public string GetFirstSecondFormula(FormulaUtil formulaUtil, bool bIsFirstFormula)
  {
    return this.GetFirstSecondFormula(formulaUtil, bIsFirstFormula, false);
  }

  internal string GetFirstSecondFormula(
    FormulaUtil formulaUtil,
    bool bIsFirstFormula,
    bool isForserialization)
  {
    Ptg[] ptgs = bIsFirstFormula ? this.m_formatRecord.FirstFormulaPtgs : this.m_formatRecord.SecondFormulaPtgs;
    if (ptgs == null)
      return (string) null;
    object parent = this.Parent;
    Rectangle rectangleFromRangeString = (this.FindParent(typeof (WorksheetImpl)) as WorksheetImpl).GetRectangleFromRangeString((this.Parent as ConditionalFormats).GetEnclosedRangeAddress(true));
    bool flag1 = false;
    for (int index = 0; index < ptgs.Length; ++index)
    {
      if (ptgs[index] is IRangeGetter)
      {
        flag1 = true;
        break;
      }
    }
    string ptgArray = formulaUtil.ParsePtgArray(ptgs, rectangleFromRangeString.Top, rectangleFromRangeString.Left, this.m_book.CalculationOptions.R1C1ReferenceMode, isForserialization);
    DateTime result;
    if (!flag1 && DateTime.TryParseExact(ptgArray, this.Workbook.DateTimePatterns, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
      return result.ToOADate().ToString();
    bool flag2 = false;
    foreach (Ptg ptg in ptgs)
    {
      if (ptg.TokenCode == FormulaToken.tName1 || ptg.TokenCode == FormulaToken.tName2 || ptg.TokenCode == FormulaToken.tName3 || ptg.TokenCode == FormulaToken.tNameX1 || ptg.TokenCode == FormulaToken.tNameX2 || ptg.TokenCode == FormulaToken.tNameX3)
      {
        flag2 = true;
        break;
      }
    }
    return flag2 && ptgArray != null && ptgArray != string.Empty && this.FormatType == ExcelCFType.CellValue && !double.TryParse(ptgArray, out double _) && ptgArray[0].ToString() != "\"" && ptgArray[ptgArray.Length - 1].ToString() != "\"" ? $"\"{ptgArray}\"" : ptgArray;
  }

  private Rectangle GetCellRectangle(ConditionalFormats parentFormats)
  {
    if (parentFormats != null && parentFormats.CellsList.Length > 0)
    {
      string minimumRange = this.GetMinimumRange(parentFormats.CellsList);
      for (int index = 0; index < parentFormats.CellsList.Length; ++index)
      {
        IRange range = this.Workbook.ActiveSheet[parentFormats.CellsList[index]];
        if (this.Range.Row >= range.Row && this.Range.Column >= range.Column || this.Range.LastRow <= range.LastRow && this.Range.LastColumn <= range.LastColumn)
          return new Rectangle(this.Workbook.ActiveSheet[minimumRange].Column, this.Workbook.ActiveSheet[minimumRange].Row, 0, 0);
      }
    }
    return new Rectangle(0, 0, 0, 0);
  }

  private string GetMinimumRange(string[] sortedList)
  {
    List<string> stringList = new List<string>();
    foreach (string sorted in sortedList)
    {
      if (sorted.Contains(":"))
        stringList.AddRange((IEnumerable<string>) sorted.Split(':'));
      else
        stringList.Add(sorted);
    }
    for (int index1 = 1; index1 < stringList.Count; ++index1)
    {
      string str = stringList[index1];
      IRange range = this.Workbook.ActiveSheet[stringList[index1]];
      int index2;
      for (index2 = index1; index2 > 0 && this.Workbook.ActiveSheet[stringList[index2 - 1]].Row <= range.Row && this.Workbook.ActiveSheet[stringList[index2 - 1]].Column <= range.Column; --index2)
        stringList[index2] = stringList[index2 - 1];
      stringList[index2] = str;
    }
    return stringList[stringList.Count - 1];
  }

  public void BeginUpdate() => throw new NotImplementedException();

  public void EndUpdate() => throw new NotImplementedException();

  private void UpdateColors()
  {
    this.m_formatRecord.PatternColorIndex = (ushort) this.m_color.GetIndexed((IWorkbook) this.m_book);
    this.m_formatRecord.PatternBackColor = (ushort) this.m_backColor.GetIndexed((IWorkbook) this.m_book);
    this.m_formatRecord.TopBorderColorIndex = (uint) (ushort) this.m_topBorderColor.GetIndexed((IWorkbook) this.m_book);
    this.m_formatRecord.BottomBorderColorIndex = (uint) (ushort) this.m_bottomBorderColor.GetIndexed((IWorkbook) this.m_book);
    this.m_formatRecord.LeftBorderColorIndex = (uint) (ushort) this.m_leftBorderColor.GetIndexed((IWorkbook) this.m_book);
    this.m_formatRecord.RightBorderColorIndex = (uint) (ushort) this.m_rightBorderColor.GetIndexed((IWorkbook) this.m_book);
  }

  public void UpdateFormula(
    int iCurIndex,
    int iSourceIndex,
    Rectangle sourceRect,
    int iDestIndex,
    Rectangle destRect,
    int row,
    int column)
  {
    Ptg[] firstFormulaPtgs = this.m_formatRecord.FirstFormulaPtgs;
    if (firstFormulaPtgs != null && firstFormulaPtgs.Length > 0)
    {
      Ptg[] ptgArray;
      if (!this.IsRelativeFormula(firstFormulaPtgs) || !this.IsConditionalFormatCopying)
      {
        ptgArray = this.m_book.FormulaUtil.UpdateFormula(firstFormulaPtgs, iCurIndex, iSourceIndex, sourceRect, iDestIndex, destRect, row, column);
      }
      else
      {
        int x = -sourceRect.X;
        sourceRect.Offset(x, 0);
        destRect.Offset(x, 0);
        ptgArray = this.m_book.FormulaUtil.UpdateFormula(firstFormulaPtgs, iCurIndex, iSourceIndex, sourceRect, iDestIndex, destRect, row, column);
        sourceRect.Offset(-x, 0);
        destRect.Offset(-x, 0);
      }
      this.m_formatRecord.FirstFormulaPtgs = ptgArray;
    }
    Ptg[] secondFormulaPtgs = this.m_formatRecord.SecondFormulaPtgs;
    if (secondFormulaPtgs == null || secondFormulaPtgs.Length <= 0)
      return;
    Ptg[] ptgArray1;
    if (!this.IsRelativeFormula(secondFormulaPtgs) || !this.IsConditionalFormatCopying)
    {
      ptgArray1 = this.m_book.FormulaUtil.UpdateFormula(secondFormulaPtgs, iCurIndex, iSourceIndex, sourceRect, iDestIndex, destRect, row, column);
    }
    else
    {
      int x = -sourceRect.X;
      sourceRect.Offset(x, 0);
      destRect.Offset(x, 0);
      ptgArray1 = this.m_book.FormulaUtil.UpdateFormula(secondFormulaPtgs, iCurIndex, iSourceIndex, sourceRect, iDestIndex, destRect, row, column);
    }
    this.m_formatRecord.SecondFormulaPtgs = ptgArray1;
  }

  private bool IsRelativeFormula(Ptg[] formula)
  {
    foreach (Ptg ptg in formula)
    {
      string str = Regex.Replace(ptg.ToString(), "\\d", "");
      if (ptg.TokenCode == FormulaToken.tRef2 && (!ptg.ToString().Contains("$") || !str.EndsWith("$")))
        return true;
    }
    return false;
  }

  public override int GetHashCode()
  {
    return this.m_formatRecord.GetHashCode() ^ this.m_cf12Record.GetHashCode() ^ this.m_cfExRecord.GetHashCode();
  }

  public override bool Equals(object obj)
  {
    return obj is ConditionalFormatImpl conditionalFormatImpl && this.m_formatRecord.Equals((object) conditionalFormatImpl.m_formatRecord) && this.m_cfExRecord.Equals((object) conditionalFormatImpl.CFExRecord) && this.m_cf12Record.Equals((object) conditionalFormatImpl.m_cf12Record) && this.m_color == conditionalFormatImpl.m_color && this.m_backColor == conditionalFormatImpl.m_backColor && this.m_topBorderColor == conditionalFormatImpl.m_topBorderColor && this.m_bottomBorderColor == conditionalFormatImpl.m_bottomBorderColor && this.m_leftBorderColor == conditionalFormatImpl.m_leftBorderColor && this.m_rightBorderColor == conditionalFormatImpl.m_rightBorderColor && this.m_fontColor == conditionalFormatImpl.m_fontColor && this.m_dataBar == conditionalFormatImpl.m_dataBar && this.m_iconSet == conditionalFormatImpl.m_iconSet && this.m_topBottom == conditionalFormatImpl.m_topBottom && this.m_aboveBelowAverage == conditionalFormatImpl.m_aboveBelowAverage && this.m_colorScale == conditionalFormatImpl.m_colorScale && this.Priority == conditionalFormatImpl.Priority;
  }

  public object Clone(object parent)
  {
    ConditionalFormatImpl conditionalFormatImpl = (ConditionalFormatImpl) this.MemberwiseClone();
    conditionalFormatImpl.SetParent(parent);
    conditionalFormatImpl.SetParents();
    conditionalFormatImpl.m_formatRecord = (CFRecord) CloneUtils.CloneCloneable((ICloneable) this.m_formatRecord);
    int orCreateFormat = conditionalFormatImpl.Workbook.InnerFormats.FindOrCreateFormat(this.NumberFormat);
    conditionalFormatImpl.Workbook.InnerFormats.Add(orCreateFormat, this.NumberFormat);
    conditionalFormatImpl.NumberFormatIndex = (ushort) orCreateFormat;
    conditionalFormatImpl.InitializeColors();
    conditionalFormatImpl.m_color.CopyFrom(this.m_color, false);
    conditionalFormatImpl.m_backColor.CopyFrom(this.m_backColor, false);
    conditionalFormatImpl.m_fontColor.CopyFrom(this.m_fontColor, false);
    conditionalFormatImpl.m_leftBorderColor.CopyFrom(this.m_leftBorderColor, false);
    conditionalFormatImpl.m_rightBorderColor.CopyFrom(this.m_rightBorderColor, false);
    conditionalFormatImpl.m_topBorderColor.CopyFrom(this.m_topBorderColor, false);
    conditionalFormatImpl.m_bottomBorderColor.CopyFrom(this.m_bottomBorderColor, false);
    conditionalFormatImpl.m_formatRecord = (CFRecord) CloneUtils.CloneCloneable((ICloneable) this.m_formatRecord);
    conditionalFormatImpl.m_cf12Record = (CF12Record) CloneUtils.CloneCloneable((ICloneable) this.m_cf12Record);
    conditionalFormatImpl.m_cfExRecord = (CFExRecord) CloneUtils.CloneCloneable((ICloneable) this.m_cfExRecord);
    conditionalFormatImpl.m_dataBar = (DataBarImpl) CloneUtils.CloneCloneable((ICloneable) this.m_dataBar);
    conditionalFormatImpl.m_iconSet = (IconSetImpl) CloneUtils.CloneCloneable((ICloneable) this.m_iconSet);
    if (this.m_aboveBelowAverage != (AboveBelowAverageImpl) null)
      conditionalFormatImpl.m_aboveBelowAverage = this.m_aboveBelowAverage.Clone();
    if (this.m_topBottom != (TopBottomImpl) null)
      conditionalFormatImpl.m_topBottom = this.m_topBottom.Clone();
    return (object) conditionalFormatImpl;
  }

  public ColorObject ColorObject => this.m_color;

  public ColorObject BackColorObject => this.m_backColor;

  public ColorObject TopBorderColorObject => this.m_topBorderColor;

  public ColorObject BottomBorderColorObject => this.m_bottomBorderColor;

  public ColorObject LeftBorderColorObject => this.m_leftBorderColor;

  public ColorObject RightBorderColorObject => this.m_rightBorderColor;

  public ColorObject FontColorObject => this.m_fontColor;

  public bool IsPatternStyleModified
  {
    get => this.m_formatRecord.IsPatternStyleModified;
    set => this.m_formatRecord.IsPatternStyleModified = value;
  }

  Ptg[] IInternalConditionalFormat.FirstFormulaPtgs => this.m_formatRecord.FirstFormulaPtgs;

  Ptg[] IInternalConditionalFormat.SecondFormulaPtgs => this.m_formatRecord.SecondFormulaPtgs;

  internal void ClearAll()
  {
    if (this.m_fontColor != (ColorObject) null)
      this.m_fontColor.Dispose();
    if (this.m_color != (ColorObject) null)
      this.m_color.Dispose();
    if (this.m_cf12Record != null)
      this.m_cf12Record.ClearAll();
    this.m_cfExRecord.ClearAll();
    this.m_backColor.Dispose();
    this.m_fontColor.Dispose();
    this.m_book = (WorkbookImpl) null;
  }
}
