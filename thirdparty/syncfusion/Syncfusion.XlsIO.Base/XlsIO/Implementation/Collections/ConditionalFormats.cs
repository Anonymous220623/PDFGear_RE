// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.ConditionalFormats
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections;

public class ConditionalFormats : 
  CollectionBaseEx<IConditionalFormat>,
  IConditionalFormats,
  IEnumerable,
  IParentApplication,
  IOptimizedUpdate
{
  public const int MAXIMUM_CF_NUMBER = 3;
  private CondFMTRecord m_format;
  private CondFmt12Record m_condFMT12;
  private WorksheetImpl m_sheet;
  private RangesOperations m_rangesOperations;
  private int m_cfCount;
  private int m_acfCount;
  private int m_totalCFCount;
  private int m_cfIndex;
  private bool m_futureRecord;
  private bool m_pivot;
  private bool m_isCopying;

  public WorksheetImpl sheet => this.m_sheet;

  public ConditionalFormats(IApplication application, object parent)
    : base(application, parent)
  {
    this.m_format = (CondFMTRecord) BiffRecordFactory.GetRecord(TBIFFRecord.CondFMT);
    this.m_condFMT12 = (CondFmt12Record) BiffRecordFactory.GetRecord(TBIFFRecord.CondFMT12);
    this.FindParent();
    this.m_rangesOperations = new RangesOperations(this.m_format.CellList);
  }

  public ConditionalFormats(IApplication application, object parent, ConditionalFormats toClone)
    : base(application, parent)
  {
    if (toClone == null)
      return;
    if (toClone.m_format != null)
    {
      this.m_format = (CondFMTRecord) toClone.m_format.Clone();
      this.m_rangesOperations = new RangesOperations(this.m_format.CellList);
    }
    if (toClone.m_condFMT12 != null)
    {
      this.m_condFMT12 = (CondFmt12Record) toClone.m_condFMT12.Clone();
      this.m_rangesOperations = new RangesOperations(this.m_condFMT12.CellList);
    }
    int index = 0;
    for (int count = toClone.Count; index < count; ++index)
    {
      ConditionalFormatImpl conditionalFormatImpl = (ConditionalFormatImpl) toClone.List[index];
      if (conditionalFormatImpl != null)
        this.Add(conditionalFormatImpl.Clone((object) this) as IConditionalFormat);
    }
    this.FindParent();
  }

  public ConditionalFormats(
    IApplication application,
    object parent,
    ConditionalFormats toClone,
    bool bCopy)
    : base(application, parent)
  {
    if (toClone == null || !bCopy)
      return;
    if (toClone.m_format != null)
    {
      this.m_format = (CondFMTRecord) toClone.m_format.Clone();
      if (toClone.m_format.CellList.Count > 0)
        this.m_rangesOperations = new RangesOperations(this.m_format.CellList);
    }
    if (toClone.m_condFMT12 != null)
    {
      this.m_condFMT12 = (CondFmt12Record) toClone.m_condFMT12.Clone();
      if (toClone.m_condFMT12.CellList.Count > 0)
        this.m_rangesOperations = new RangesOperations(this.m_condFMT12.CellList);
    }
    int index = 0;
    for (int count = toClone.Count; index < count; ++index)
    {
      ConditionalFormatImpl conditionalFormatImpl = (ConditionalFormatImpl) toClone.List[index];
      if (conditionalFormatImpl != null)
        this.Add(conditionalFormatImpl.Clone((object) this) as IConditionalFormat);
    }
    this.FindParent();
  }

  [CLSCompliant(false)]
  public ConditionalFormats(
    IApplication application,
    object parent,
    CondFMTRecord format,
    IList formats,
    IList CFExRecords)
    : base(application, parent)
  {
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    if (formats == null)
      throw new ArgumentNullException(nameof (formats));
    this.m_format = format;
    if (formats.Count > 0)
    {
      int index = 0;
      for (int count = formats.Count; index < count; ++index)
        this.AddFromRecord(formats[index] as CFRecord);
    }
    if (CFExRecords.Count > 0)
    {
      int index = 0;
      for (int count = CFExRecords.Count; index < count; ++index)
        this.AddFromCFEXRecord(CFExRecords[index] as CFExRecord);
    }
    this.FindParent();
    this.m_rangesOperations = new RangesOperations(this.m_format.CellList);
  }

  [CLSCompliant(false)]
  public ConditionalFormats(
    IApplication application,
    object parent,
    CondFmt12Record format,
    IList formats)
    : base(application, parent)
  {
    if (format == null)
      throw new ArgumentNullException(nameof (format));
    if (formats == null)
      throw new ArgumentNullException(nameof (formats));
    this.m_condFMT12 = format;
    int index = 0;
    for (int count = formats.Count; index < count; ++index)
      this.AddFromCF12Record(formats[index] as CF12Record);
    this.FindParent();
    this.m_rangesOperations = new RangesOperations(this.m_condFMT12.CellList);
  }

  private void FindParent()
  {
    this.m_sheet = this.FindParent(typeof (WorksheetImpl)) as WorksheetImpl;
    if (this.m_sheet == null)
      throw new ArgumentNullException("Parent worksheet.");
  }

  private int MaxCFNumber => int.MaxValue;

  public IConditionalFormat AddCondition()
  {
    if (this.Count >= this.MaxCFNumber)
      throw new ArgumentOutOfRangeException("Too many conditional formats.");
    ConditionalFormatImpl conditionalFormatImpl = new ConditionalFormatImpl(this.Application, (object) this);
    this.Add((IConditionalFormat) conditionalFormatImpl);
    return (IConditionalFormat) conditionalFormatImpl;
  }

  public void Remove() => throw new NotImplementedException();

  public void RemoveAt() => throw new NotImplementedException();

  private void CopyAdvancedConditionalFormatting(ConditionalFormatImpl format)
  {
    format.CF12Record.FormatType = format.FormatType;
    format.CF12Record.Criteria = format.ColorScale;
    format.CF12Record.DataBarImpl = format.DataBar;
    format.CF12Record.IconSetImpl = format.IconSet;
    format.CF12Record.StopIfTrue = format.StopIfTrue;
  }

  [CLSCompliant(false)]
  public int Serialize(OffsetArrayList records, ushort index, int priority)
  {
    int count = this.Count;
    if (count <= 0 || this.m_rangesOperations != null && this.m_rangesOperations.CellList.Count == 0)
      return count;
    this.m_cfCount = 0;
    this.m_acfCount = 0;
    for (int index1 = 0; index1 < count; ++index1)
    {
      ConditionalFormatImpl inner = (ConditionalFormatImpl) this.InnerList[index1];
      if (inner.CF12Record.IsParsed || !inner.CF12Record.IsParsed && (inner.FormatType == ExcelCFType.ColorScale || inner.FormatType == ExcelCFType.DataBar || inner.FormatType == ExcelCFType.IconSet || inner.FormatType == ExcelCFType.AboveBelowAverage && inner.AboveBelowAverage.AverageType.ToString().Contains("StdDev") && this.CellRectangles.Count > 1 || inner.FormatType == ExcelCFType.TopBottom && this.CellRectangles.Count > 1))
        ++this.m_acfCount;
      else
        ++this.m_cfCount;
      ConditionalFormats parent = inner.Parent as ConditionalFormats;
      if (inner.FormatType == ExcelCFType.Unique || inner.FormatType == ExcelCFType.Duplicate)
      {
        bool flag = false;
        string str = "=AND(";
        string[] strArray = parent.CellsList[0].Split(':');
        string cells1 = parent.CellsList[parent.CellsList.Length - 1];
        int index2 = 0;
        foreach (Rectangle cellsRectangle in parent.CellsRectangleList)
        {
          flag = this.CheckIfValidRange(cellsRectangle);
          if (!flag)
          {
            string cells2 = parent.CellsList[index2];
            str = $"{str}COUNTIF({cells2}, {strArray[0]})";
            str = !(cells2 != cells1) ? (inner.FormatType != ExcelCFType.Duplicate ? $"{str} = 1,NOT(ISBLANK({strArray[0]})))" : $"{str} > 1,NOT(ISBLANK({strArray[0]})))") : str + "+";
            ++index2;
          }
          else
            break;
        }
        if (!flag)
          inner.FirstFormula = str;
      }
      else if (inner.FormatType == ExcelCFType.TopBottom && parent.CellsRectangleList.Count == 1)
      {
        bool flag = false;
        string str1 = inner.TopBottom.Type == ExcelCFTopBottomType.Top ? "LARGE(" : "SMALL(";
        string[] strArray = parent.CellsList[0].Split(':');
        string cells3 = parent.CellsList[parent.CellsList.Length - 1];
        foreach (Rectangle cellsRectangle in parent.CellsRectangleList)
        {
          flag = this.CheckIfValidRange(cellsRectangle);
          if (!flag)
          {
            string cells4 = parent.CellsList[0];
            bool percent = inner.TopBottom.Percent;
            if (percent)
            {
              string str2 = $"INT(COUNT({cells4})*{(object) inner.TopBottom.Rank}%)";
              str1 = $"IF({str2}>0,{str1}{cells4},{str2}),";
            }
            else
              str1 = $"{str1}({cells4}),MIN({(object) inner.TopBottom.Rank},COUNT({cells4})))";
            if (inner.TopBottom.Type == ExcelCFTopBottomType.Top)
            {
              if (percent)
                str1 = $"{str1}MAX({cells4}))";
              str1 = $"{str1}<={strArray[0]}";
            }
            else
            {
              if (percent)
                str1 = $"{str1}MIN({cells4}))";
              str1 = $"{str1}>={strArray[0]}";
            }
          }
          else
            break;
        }
        if (!flag)
          inner.FirstFormula = str1;
      }
      else if (inner.FormatType == ExcelCFType.AboveBelowAverage)
      {
        bool flag1 = false;
        string str3 = "AVERAGE(";
        string[] strArray = parent.CellsList[0].Split(':');
        string cells5 = parent.CellsList[parent.CellsList.Length - 1];
        int index3 = 0;
        bool flag2 = inner.AboveBelowAverage.AverageType.ToString().Contains("StdDev");
        foreach (Rectangle cellsRectangle in parent.CellsRectangleList)
        {
          flag1 = this.CheckIfValidRange(cellsRectangle);
          if (!flag1)
          {
            string cells6 = parent.CellsList[index3];
            if (flag2)
            {
              string str4 = string.Empty + cells6;
              if (cells6 != cells5)
              {
                string str5 = str4 + ",";
              }
              else if (inner.AboveBelowAverage.AverageType.ToString().Contains("Below"))
                str3 = $"({strArray[0]}-{str3}{str4}))>=STDEVP({str4})*({(object) inner.AboveBelowAverage.StdDevValue})";
              else
                str3 = $"({strArray[0]}-{str3}{str4}))>=STDEVP({str4})*(-{(object) inner.AboveBelowAverage.StdDevValue})";
            }
            else
            {
              str3 = $"{str3}IF(ISERROR({cells6}), \"\",IF(ISBLANK({cells6}), \"\",{cells6}))";
              if (cells6 != cells5)
              {
                str3 += ",";
              }
              else
              {
                string str6 = strArray[0];
                if (inner.AboveBelowAverage.AverageType.ToString().Contains("Above"))
                  str6 += ">";
                else if (inner.AboveBelowAverage.AverageType.ToString().Contains("Below"))
                  str6 += "<";
                if (inner.AboveBelowAverage.AverageType.ToString().Contains("Equal"))
                  str6 += "=";
                str3 = $"{str6}{str3})";
              }
            }
            ++index3;
          }
          else
            break;
        }
        if (!flag1)
          inner.FirstFormula = str3;
      }
    }
    this.m_totalCFCount = this.m_cfCount + this.m_acfCount;
    CondFmt12Record temp_condfmt12 = (CondFmt12Record) null;
    CondFMTRecord temp_condfmt = (CondFMTRecord) null;
    if (count <= 3)
    {
      for (int index4 = 0; index4 < count; ++index4)
      {
        ConditionalFormatImpl inner = (ConditionalFormatImpl) this.InnerList[index4];
        if (inner.CF12Record.IsParsed || !inner.CF12Record.IsParsed && (inner.FormatType == ExcelCFType.ColorScale || inner.FormatType == ExcelCFType.DataBar || inner.FormatType == ExcelCFType.IconSet || inner.FormatType == ExcelCFType.AboveBelowAverage && inner.AboveBelowAverage.AverageType.ToString().Contains("StdDev") && this.CellRectangles.Count > 1 || inner.FormatType == ExcelCFType.TopBottom && this.CellRectangles.Count > 1))
          temp_condfmt12 = this.SerializeACF(inner, temp_condfmt12, priority, records, index, this.m_acfCount);
        else
          temp_condfmt = this.SerializeCF(inner, temp_condfmt, priority, records, index, index4);
        ++priority;
      }
    }
    else
    {
      for (int index5 = 0; index5 < count; ++index5)
      {
        ConditionalFormatImpl inner = (ConditionalFormatImpl) this.InnerList[index5];
        if (this.m_acfCount == this.m_totalCFCount)
          temp_condfmt12 = this.SerializeACF(inner, temp_condfmt12, priority, records, index, this.m_acfCount);
        else if (this.m_cfCount == this.m_totalCFCount)
          temp_condfmt = this.SerializeCF(inner, temp_condfmt, priority, records, index, index5);
        else if (inner.CF12Record.IsParsed || !inner.CF12Record.IsParsed && (inner.FormatType == ExcelCFType.ColorScale || inner.FormatType == ExcelCFType.DataBar || inner.FormatType == ExcelCFType.IconSet))
          temp_condfmt12 = this.SerializeACF(inner, temp_condfmt12, priority, records, index, this.m_totalCFCount);
        else
          temp_condfmt = this.SerializeCF(inner, temp_condfmt, priority, records, index, index5);
        ++priority;
      }
    }
    foreach (CFExRecord cfExRecord in this.m_sheet.m_dictCFExRecords.Values)
    {
      if (!cfExRecord.IsCFExParsed)
      {
        foreach (CondFMTRecord condFmtRecord in this.m_sheet.m_dictCondFMT.Values)
        {
          Rectangle rectangle = cfExRecord.EncloseRange.GetRectangle();
          if (condFmtRecord.CellList.Contains(rectangle))
            cfExRecord.CondFmtIndex = condFmtRecord.Index;
        }
      }
    }
    return priority;
  }

  private bool CheckIfValidRange(Rectangle rect)
  {
    int num1 = rect.Y + 1;
    int num2 = rect.X + 1;
    int bottom = rect.Bottom;
    int right = rect.Right;
    return num1 < 1 || num1 > this.sheet.Workbook.MaxRowCount || num2 < 1 || num2 > this.sheet.Workbook.MaxColumnCount;
  }

  private CondFmt12Record SerializeACF(
    ConditionalFormatImpl condFormat,
    CondFmt12Record temp_condfmt12,
    int priority,
    OffsetArrayList records,
    ushort index,
    int ruleCount)
  {
    if (this.m_totalCFCount == this.m_acfCount)
    {
      if (!condFormat.CF12Record.IsParsed && (condFormat.FormatType == ExcelCFType.ColorScale || condFormat.FormatType == ExcelCFType.DataBar || condFormat.FormatType == ExcelCFType.IconSet || condFormat.FormatType == ExcelCFType.AboveBelowAverage && condFormat.AboveBelowAverage.AverageType.ToString().Contains("StdDev") && this.CellRectangles.Count > 1 || condFormat.FormatType == ExcelCFType.TopBottom && this.CellRectangles.Count > 1))
      {
        if (temp_condfmt12 == null && this.m_condFMT12 != null)
        {
          this.m_condFMT12.CellList = this.CellRectangles;
          this.m_condFMT12.CellsCount = (ushort) this.CellRectangles.Count;
          this.m_condFMT12.EncloseRange = this.EnclosedRange;
          this.m_condFMT12.Index = index;
          this.m_condFMT12.CF12RecordCount = (ushort) ruleCount;
          records.Add((IBiffStorage) this.m_condFMT12);
          temp_condfmt12 = this.m_condFMT12;
        }
        condFormat.CF12Record.FormatType = condFormat.FormatType;
        condFormat.CF12Record.ComparisonOperator = ExcelComparisonOperator.None;
        condFormat.CF12Record.StopIfTrue = condFormat.StopIfTrue;
        condFormat.CF12Record.Priority = (ushort) priority;
        condFormat.CF12Record.Criteria = condFormat.ColorScale;
        condFormat.CF12Record.DataBarImpl = condFormat.DataBar;
        condFormat.CF12Record.IconSetImpl = condFormat.IconSet;
        if (condFormat.FormatType == ExcelCFType.AboveBelowAverage || condFormat.FormatType == ExcelCFType.TopBottom)
        {
          if (condFormat.FormatType == ExcelCFType.TopBottom)
          {
            condFormat.CF12Record.Template = ConditionalFormatTemplate.Filter;
            condFormat.CF12Record.TopBottomCF12.TopBottom = condFormat.TopBottom;
          }
          else if (condFormat.FormatType == ExcelCFType.AboveBelowAverage)
          {
            if (condFormat.AboveBelowAverage.AverageType == ExcelCFAverageType.EqualOrAbove)
              condFormat.CF12Record.Template = ConditionalFormatTemplate.AboveOrEqualToAverage;
            else if (condFormat.AboveBelowAverage.AverageType == ExcelCFAverageType.EqualOrBelow)
              condFormat.CF12Record.Template = ConditionalFormatTemplate.BelowOrEqualToAverage;
            else if (condFormat.AboveBelowAverage.AverageType == ExcelCFAverageType.Above || condFormat.AboveBelowAverage.AverageType == ExcelCFAverageType.AboveStdDev)
              condFormat.CF12Record.Template = ConditionalFormatTemplate.AboveAverage;
            else if (condFormat.AboveBelowAverage.AverageType == ExcelCFAverageType.Below || condFormat.AboveBelowAverage.AverageType == ExcelCFAverageType.BelowStdDev)
              condFormat.CF12Record.Template = ConditionalFormatTemplate.BelowAverage;
            condFormat.CF12Record.AboveBelowAverageCF12.AboveBelowAverage = condFormat.AboveBelowAverage;
          }
          if (condFormat.BackColorObject.ColorType == ColorType.RGB || condFormat.BackColorObject.ColorType == ColorType.Theme)
            condFormat.CF12Record.Properties.Add(this.m_sheet.ParentWorkbook.CreateExtendedProperty(CellPropertyExtensionType.BackColor, condFormat.BackColorRGB, condFormat.BackColorObject.ColorType));
          if (condFormat.ColorObject.ColorType == ColorType.RGB || condFormat.ColorObject.ColorType == ColorType.Theme)
            condFormat.CF12Record.Properties.Add(this.m_sheet.ParentWorkbook.CreateExtendedProperty(CellPropertyExtensionType.ForeColor, condFormat.ColorRGB, condFormat.ColorObject.ColorType));
          if (condFormat.FontColorObject.ColorType == ColorType.RGB || condFormat.FontColorObject.ColorType == ColorType.Theme)
            condFormat.CF12Record.Properties.Add(this.m_sheet.ParentWorkbook.CreateExtendedProperty(CellPropertyExtensionType.TextColor, condFormat.FontColorRGB, condFormat.FontColorObject.ColorType));
        }
        condFormat.SerializeCF12(records);
      }
      else
      {
        if (temp_condfmt12 == null)
        {
          records.Add((IBiffStorage) this.m_condFMT12);
          temp_condfmt12 = this.m_condFMT12;
        }
        condFormat.CF12Record.Priority = (ushort) priority;
        condFormat.CF12Record.TopBottomCF12.TopBottom = condFormat.TopBottom;
        condFormat.CF12Record.AboveBelowAverageCF12.AboveBelowAverage = condFormat.AboveBelowAverage;
        condFormat.SerializeCF12(records);
      }
    }
    else if (!condFormat.CF12Record.IsParsed && condFormat.CFExRecord.IsCF12Extends != (byte) 1)
    {
      condFormat.CFExRecord.CondFmtIndex = index;
      condFormat.CFExRecord.IsCF12Extends = (byte) 1;
      condFormat.CFExRecord.CF12RecordIfExtends = condFormat.CF12Record;
      condFormat.CFExRecord.CF12RecordIfExtends.FormatType = condFormat.FormatType;
      condFormat.CFExRecord.CF12RecordIfExtends.ComparisonOperator = condFormat.Operator;
      condFormat.CFExRecord.CF12RecordIfExtends.StopIfTrue = condFormat.StopIfTrue;
      condFormat.CFExRecord.CF12RecordIfExtends.Priority = (ushort) priority;
      condFormat.CFExRecord.CF12RecordIfExtends.Criteria = condFormat.ColorScale;
      condFormat.CFExRecord.CF12RecordIfExtends.DataBarImpl = condFormat.DataBar;
      condFormat.CFExRecord.CF12RecordIfExtends.IconSetImpl = condFormat.IconSet;
      this.m_sheet.m_dictCFExRecords.Add(this.m_sheet.m_dictCFExRecords.Count, condFormat.CFExRecord);
    }
    else
      this.m_sheet.m_dictCFExRecords.Add(this.m_sheet.m_dictCFExRecords.Count, condFormat.CFExRecord);
    return temp_condfmt12;
  }

  private CondFMTRecord SerializeCF(
    ConditionalFormatImpl condFormat,
    CondFMTRecord temp_condfmt,
    int priority,
    OffsetArrayList records,
    ushort index,
    int CFIndex)
  {
    if (this.m_totalCFCount == this.m_cfCount && CFIndex < 3 || this.m_totalCFCount != this.m_cfCount && this.m_cfCount <= 3)
    {
      if (!this.m_format.IsParsed)
      {
        if (temp_condfmt == null)
        {
          this.m_format.CellList = this.CellRectangles;
          this.m_format.CellsCount = (ushort) this.CellRectangles.Count;
          this.m_format.EncloseRange = this.EnclosedRange;
          this.m_format.Index = index;
          this.m_format.CFNumber = this.m_cfCount <= 3 ? (ushort) this.m_cfCount : (ushort) 3;
          records.Add((IBiffStorage) this.m_format);
          temp_condfmt = this.m_format;
          this.m_sheet.m_dictCondFMT.Add((int) temp_condfmt.Index, temp_condfmt);
        }
        condFormat.CFExRecord.StopIfTrue = condFormat.StopIfTrue;
        condFormat.CFExRecord.Priority = (ushort) priority;
        condFormat.CFExRecord.CondFmtIndex = this.m_format.Index;
        condFormat.CFExRecord.CFIndex = (ushort) this.m_cfIndex;
        condFormat.CFExRecord.EncloseRange = this.EnclosedRange;
        ++this.m_cfIndex;
        condFormat.Serialize(records);
        if (condFormat.CFExRecord != null)
        {
          if (condFormat.FormatType == ExcelCFType.Unique)
            condFormat.CFExRecord.Template = ConditionalFormatTemplate.UniqueValues;
          else if (condFormat.FormatType == ExcelCFType.Duplicate)
            condFormat.CFExRecord.Template = ConditionalFormatTemplate.DuplicateValues;
          else if (condFormat.FormatType == ExcelCFType.TopBottom)
          {
            condFormat.CFExRecord.Template = ConditionalFormatTemplate.Filter;
            condFormat.CFExRecord.TopBottomCFEx.TopBottom = condFormat.TopBottom;
          }
          else if (condFormat.FormatType == ExcelCFType.AboveBelowAverage)
          {
            if (condFormat.AboveBelowAverage.AverageType == ExcelCFAverageType.EqualOrAbove)
              condFormat.CFExRecord.Template = ConditionalFormatTemplate.AboveOrEqualToAverage;
            else if (condFormat.AboveBelowAverage.AverageType == ExcelCFAverageType.EqualOrBelow)
              condFormat.CFExRecord.Template = ConditionalFormatTemplate.BelowOrEqualToAverage;
            else if (condFormat.AboveBelowAverage.AverageType == ExcelCFAverageType.Above || condFormat.AboveBelowAverage.AverageType == ExcelCFAverageType.AboveStdDev)
              condFormat.CFExRecord.Template = ConditionalFormatTemplate.AboveAverage;
            else if (condFormat.AboveBelowAverage.AverageType == ExcelCFAverageType.Below || condFormat.AboveBelowAverage.AverageType == ExcelCFAverageType.BelowStdDev)
              condFormat.CFExRecord.Template = ConditionalFormatTemplate.BelowAverage;
            condFormat.CFExRecord.AboveBelowAverageCFEx.AboveBelowAverage = condFormat.AboveBelowAverage;
          }
        }
        this.m_sheet.m_dictCFExRecords.Add(this.m_sheet.m_dictCFExRecords.Count, condFormat.CFExRecord);
      }
      else
      {
        if (temp_condfmt == null && !this.m_sheet.m_dictCondFMT.ContainsKey((int) this.m_format.Index))
        {
          records.Add((IBiffStorage) this.m_format);
          temp_condfmt = this.m_format;
          this.m_sheet.m_dictCondFMT.Add((int) temp_condfmt.Index, this.m_format);
        }
        condFormat.Serialize(records);
        if (condFormat.CFExRecord != null)
        {
          if (condFormat.FormatType == ExcelCFType.Unique)
            condFormat.CFExRecord.Template = ConditionalFormatTemplate.UniqueValues;
          else if (condFormat.FormatType == ExcelCFType.Duplicate)
            condFormat.CFExRecord.Template = ConditionalFormatTemplate.DuplicateValues;
          else if (condFormat.FormatType == ExcelCFType.TopBottom)
          {
            condFormat.CFExRecord.Template = ConditionalFormatTemplate.Filter;
            condFormat.CFExRecord.TopBottomCFEx.TopBottom = condFormat.TopBottom;
          }
          else if (condFormat.FormatType == ExcelCFType.AboveBelowAverage)
          {
            if (condFormat.AboveBelowAverage.AverageType == ExcelCFAverageType.EqualOrAbove)
              condFormat.CFExRecord.Template = ConditionalFormatTemplate.AboveOrEqualToAverage;
            else if (condFormat.AboveBelowAverage.AverageType == ExcelCFAverageType.EqualOrBelow)
              condFormat.CFExRecord.Template = ConditionalFormatTemplate.BelowOrEqualToAverage;
            else if (condFormat.AboveBelowAverage.AverageType == ExcelCFAverageType.Above || condFormat.AboveBelowAverage.AverageType == ExcelCFAverageType.AboveStdDev)
              condFormat.CFExRecord.Template = ConditionalFormatTemplate.AboveAverage;
            else if (condFormat.AboveBelowAverage.AverageType == ExcelCFAverageType.Below || condFormat.AboveBelowAverage.AverageType == ExcelCFAverageType.BelowStdDev)
              condFormat.CFExRecord.Template = ConditionalFormatTemplate.BelowAverage;
            condFormat.CFExRecord.AboveBelowAverageCFEx.AboveBelowAverage = condFormat.AboveBelowAverage;
          }
        }
        this.m_sheet.m_dictCFExRecords.Add(this.m_sheet.m_dictCFExRecords.Count, condFormat.CFExRecord);
      }
    }
    else if (!this.m_format.IsParsed)
    {
      condFormat.CFExRecord.CellList = this.CellRectangles;
      condFormat.CFExRecord.CellsCount = (ushort) this.CellRectangles.Count;
      condFormat.CFExRecord.EncloseRange = this.EnclosedRange;
      condFormat.CFExRecord.IsCF12Extends = (byte) 1;
      condFormat.CFExRecord.CF12RecordIfExtends = condFormat.CF12Record;
      condFormat.CFExRecord.CF12RecordIfExtends.FormatType = condFormat.FormatType;
      condFormat.CFExRecord.CF12RecordIfExtends.ComparisonOperator = condFormat.Operator;
      condFormat.CFExRecord.CF12RecordIfExtends.StopIfTrue = condFormat.StopIfTrue;
      condFormat.CFExRecord.CF12RecordIfExtends.Priority = (ushort) priority;
      this.m_sheet.m_dictCFExRecords.Add(this.m_sheet.m_dictCFExRecords.Count, condFormat.CFExRecord);
    }
    else
      this.m_sheet.m_dictCFExRecords.Add(this.m_sheet.m_dictCFExRecords.Count, condFormat.CFExRecord);
    return temp_condfmt;
  }

  [CLSCompliant(false)]
  public void AddFromRecord(CFRecord cf)
  {
    this.Add((IConditionalFormat) new ConditionalFormatImpl(this.Application, (object) this, cf));
  }

  [CLSCompliant(false)]
  public void AddFromCF12Record(CF12Record cf12)
  {
    this.Add((IConditionalFormat) new ConditionalFormatImpl(this.Application, (object) this, cf12)
    {
      FormatType = cf12.FormatType
    });
  }

  [CLSCompliant(false)]
  public void AddFromCFEXRecord(CFExRecord cfEx)
  {
    ConditionalFormatImpl conditionalFormatImpl = new ConditionalFormatImpl(this.Application, (object) this, cfEx);
    if (cfEx.IsCF12Extends == (byte) 1)
      conditionalFormatImpl.FormatType = cfEx.CF12RecordIfExtends.FormatType;
    this.Add((IConditionalFormat) conditionalFormatImpl);
  }

  public bool CompareTo(ConditionalFormats formats)
  {
    if (formats.Count != this.Count)
      return false;
    for (int i = 0; i < this.Count; ++i)
    {
      if (!this.CompareFormats(this[i], formats[i]))
        return false;
    }
    return true;
  }

  public bool CompareFormats(IConditionalFormat firstFormat, IConditionalFormat secondFormat)
  {
    ConditionalFormatImpl conditionalFormatImpl1 = (ConditionalFormatImpl) firstFormat;
    ConditionalFormatImpl conditionalFormatImpl2 = (ConditionalFormatImpl) secondFormat;
    CFRecord record1 = conditionalFormatImpl1.Record;
    CFRecord record2 = conditionalFormatImpl2.Record;
    CF12Record cf12Record1 = conditionalFormatImpl1.CF12Record;
    CF12Record cf12Record2 = conditionalFormatImpl2.CF12Record;
    CFExRecord cfExRecord1 = conditionalFormatImpl1.CFExRecord;
    CFExRecord cfExRecord2 = conditionalFormatImpl2.CFExRecord;
    return record1.Data.Length == record2.Data.Length && cf12Record1.Data.Length == cf12Record2.Data.Length && cfExRecord1.Data.Length == cfExRecord2.Data.Length && BiffRecordRaw.CompareArrays(record1.Data, 0, record2.Data, 0, record1.Length) && BiffRecordRaw.CompareArrays(cf12Record1.Data, 0, cf12Record2.Data, 0, cf12Record1.Length) && BiffRecordRaw.CompareArrays(cfExRecord1.Data, 0, cfExRecord2.Data, 0, cfExRecord1.Length);
  }

  public void AddCells(ConditionalFormats formats)
  {
    if (formats == null)
      return;
    this.AddCells((IList<Rectangle>) formats.m_format.CellList);
  }

  public void AddCellsCondFMT12(ConditionalFormats formats)
  {
    if (formats == null)
      return;
    System.Collections.Generic.List<Rectangle> arrCells = new System.Collections.Generic.List<Rectangle>();
    if (this.m_condFMT12 != null)
      arrCells = formats.m_condFMT12.CellList;
    this.AddCells((IList<Rectangle>) arrCells);
  }

  public bool Contains(Rectangle[] arrRanges) => this.m_rangesOperations.Contains(arrRanges);

  public int ContainsCount(Rectangle range) => this.m_rangesOperations.ContainsCount(range);

  public void AddCells(IList<Rectangle> arrCells)
  {
    if (arrCells == null)
      return;
    int index = 0;
    for (int count = arrCells.Count; index < count; ++index)
      this.AddRange(arrCells[index]);
  }

  public void AddRange(IRange range) => this.m_rangesOperations.AddRange(range);

  public void AddRange(Rectangle rect)
  {
    this.m_rangesOperations.AddRange(rect);
    TAddr taddr = new TAddr();
    if (this.m_format != null)
      taddr = this.m_format.EncloseRange;
    if (this.m_condFMT12 != null)
      taddr = this.m_condFMT12.EncloseRange;
    taddr.FirstCol = Math.Min(rect.Left, taddr.FirstCol);
    taddr.FirstRow = Math.Min(rect.Top, taddr.FirstRow);
    taddr.LastCol = Math.Max(rect.Right, taddr.LastCol);
    taddr.LastRow = Math.Max(rect.Bottom, taddr.LastRow);
    if (this.m_format != null)
      this.m_format.EncloseRange = taddr;
    if (this.m_condFMT12 != null)
      this.m_condFMT12.EncloseRange = taddr;
    this.m_rangesOperations.AddRange(rect);
  }

  public void Remove(Rectangle[] arrRanges) => this.Remove(arrRanges, false);

  internal void Remove(Rectangle[] arrRanges, bool isCF)
  {
    this.m_rangesOperations.Remove(arrRanges, isCF);
  }

  public void ClearCells()
  {
    if (this.m_format != null && this.m_format.CFNumber > (ushort) 0)
    {
      this.m_format.CellList.Clear();
      this.m_format.CellsCount = (ushort) 0;
    }
    if (this.m_condFMT12 == null)
      return;
    this.m_condFMT12.CellList.Clear();
    this.m_condFMT12.CellsCount = (ushort) 0;
  }

  public void ConvertToExcel97to03Version()
  {
    if (this.Count <= 3)
      return;
    this.InnerList.RemoveRange(3, this.Count - 3);
  }

  public ConditionalFormats GetPart(
    int row,
    int column,
    int rowCount,
    int columnCount,
    bool remove,
    int rowIncrement,
    int columnIncrement,
    object newParent)
  {
    if (columnCount - 1 >= 0)
      --columnCount;
    if (rowCount - 1 >= 0)
      --rowCount;
    RangesOperations part1 = this.m_rangesOperations.GetPart(new Rectangle(column - 1, row - 1, columnCount, rowCount), remove, rowIncrement, columnIncrement);
    ConditionalFormats part2 = (ConditionalFormats) null;
    if (part1 != null)
    {
      part2 = (ConditionalFormats) this.Clone(newParent);
      part2.FindParent();
      part2.m_rangesOperations = part1;
      if (this.m_format != null && part2.m_format.CFNumber > (ushort) 0)
      {
        part2.m_format = (CondFMTRecord) CloneUtils.CloneCloneable((ICloneable) this.m_format);
        part2.m_format.CellList = part1.CellList;
      }
      if (this.m_condFMT12 != null)
      {
        part2.m_condFMT12 = (CondFmt12Record) CloneUtils.CloneCloneable((ICloneable) this.m_condFMT12);
        part2.m_condFMT12.CellList = part1.CellList;
      }
    }
    return part2;
  }

  public void MarkUsedReferences(bool[] usedItems)
  {
    System.Collections.Generic.List<IConditionalFormat> innerList = this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      ((ConditionalFormatImpl) innerList[index]).MarkUsedReferences(usedItems);
  }

  public void UpdateReferenceIndexes(int[] arrUpdatedIndexes)
  {
    System.Collections.Generic.List<IConditionalFormat> innerList = this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      ((ConditionalFormatImpl) innerList[index]).UpdateReferenceIndexes(arrUpdatedIndexes);
  }

  public void UpdateFormula(
    int iCurIndex,
    int iSourceIndex,
    Rectangle sourceRect,
    int iDestIndex,
    Rectangle destRect)
  {
    if (this.m_rangesOperations == null || this.m_rangesOperations.CellList.Count == 0)
      return;
    Rectangle cell = this.m_rangesOperations.CellList[0];
    int top = cell.Top;
    int left = cell.Left;
    int i = 0;
    for (int count = this.Count; i < count; ++i)
    {
      (this[i] as ConditionalFormatImpl).IsConditionalFormatCopying = this.IsCopying;
      (this[i] as ConditionalFormatImpl).UpdateFormula(iCurIndex, iSourceIndex, sourceRect, iDestIndex, destRect, top, left);
    }
  }

  public void BeginUpdate() => throw new NotImplementedException();

  public void EndUpdate() => throw new NotImplementedException();

  public override int GetHashCode()
  {
    int count = this.Count;
    int hashCode = count;
    for (int i = 0; i < count; ++i)
      hashCode |= this[i].GetHashCode();
    return hashCode;
  }

  public override bool Equals(object obj)
  {
    if (!(obj is ConditionalFormats conditionalFormats))
      return false;
    int count = this.Count;
    if (count != conditionalFormats.Count)
      return false;
    for (int i = 0; i < count; ++i)
    {
      if (!this[i].Equals((object) conditionalFormats[i]))
        return false;
    }
    return true;
  }

  public override object Clone(object parent)
  {
    ConditionalFormats conditionalFormats = (ConditionalFormats) base.Clone(parent);
    System.Collections.Generic.List<Rectangle> arrCells = new System.Collections.Generic.List<Rectangle>();
    if (this.m_format != null)
    {
      conditionalFormats.m_format = (CondFMTRecord) CloneUtils.CloneCloneable((ICloneable) this.m_format);
      if (conditionalFormats.m_format.CellList.Count > 0)
        arrCells = conditionalFormats.m_format.CellList;
    }
    if (this.m_condFMT12 != null)
    {
      conditionalFormats.m_condFMT12 = (CondFmt12Record) CloneUtils.CloneCloneable((ICloneable) this.m_condFMT12);
      if (conditionalFormats.m_condFMT12.CellList.Count > 0)
        arrCells = conditionalFormats.m_condFMT12.CellList;
    }
    conditionalFormats.m_rangesOperations = new RangesOperations(arrCells);
    return (object) conditionalFormats;
  }

  public bool IsEmpty
  {
    get
    {
      if (this.m_format != null && this.m_format.CFNumber > (ushort) 0)
        return this.m_format.CellList.Count == 0;
      return this.m_condFMT12 == null || this.m_condFMT12.CellList.Count == 0;
    }
  }

  internal bool Pivot
  {
    get => this.m_pivot;
    set => this.m_pivot = value;
  }

  internal bool IsCopying
  {
    get => this.m_isCopying;
    set => this.m_isCopying = value;
  }

  public string Address
  {
    get
    {
      TAddr taddr = new TAddr();
      System.Collections.Generic.List<Rectangle> rectangleList = (System.Collections.Generic.List<Rectangle>) null;
      string address = (string) null;
      char argumentsSeparator = this.m_sheet.AppImplementation.ArgumentsSeparator;
      if (this.m_format != null)
        taddr = this.m_format.EncloseRange;
      else if (this.m_condFMT12 != null)
        taddr = this.m_condFMT12.EncloseRange;
      if (this.m_format != null && this.m_format.CFNumber > (ushort) 0)
        rectangleList = this.m_format.CellList;
      else if (this.m_condFMT12 != null)
        rectangleList = this.m_condFMT12.CellList;
      int length = 0;
      if (rectangleList != null)
        length = rectangleList.Count;
      string[] strArray = new string[length];
      if (rectangleList == null || rectangleList.Count <= 0)
        return RangeImpl.GetAddressLocal(taddr.FirstRow + 1, taddr.FirstCol + 1, taddr.LastRow + 1, taddr.LastCol + 1);
      for (int index = 0; index < length; ++index)
      {
        Rectangle rectangle = rectangleList[index];
        strArray[index] = RangeImpl.GetAddressLocal(rectangle.Y + 1, rectangle.X + 1, rectangle.Bottom + 1, rectangle.Right + 1);
        address += strArray[index];
        if (index != length - 1)
          address += (string) (object) argumentsSeparator;
      }
      return address;
    }
  }

  internal string GetEnclosedRangeAddress(bool isFirstCell)
  {
    TAddr taddr = new TAddr();
    if (this.m_format != null)
      taddr = this.m_format.EncloseRange;
    else if (this.m_condFMT12 != null)
      taddr = this.m_condFMT12.EncloseRange;
    return isFirstCell ? RangeImpl.GetAddressLocal(taddr.FirstRow + 1, taddr.FirstCol + 1, taddr.FirstRow + 1, taddr.FirstCol + 1) : RangeImpl.GetAddressLocal(taddr.FirstRow + 1, taddr.FirstCol + 1, taddr.LastRow + 1, taddr.LastCol + 1);
  }

  public string AddressR1C1
  {
    get
    {
      TAddr taddr = new TAddr();
      System.Collections.Generic.List<Rectangle> rectangleList = (System.Collections.Generic.List<Rectangle>) null;
      string addressR1C1 = (string) null;
      if (this.m_format != null)
        taddr = this.m_format.EncloseRange;
      else if (this.m_condFMT12 != null)
        taddr = this.m_condFMT12.EncloseRange;
      if (this.m_format != null && this.m_format.CFNumber > (ushort) 0)
        rectangleList = this.m_format.CellList;
      else if (this.m_condFMT12 != null)
        rectangleList = this.m_condFMT12.CellList;
      int count = rectangleList.Count;
      string[] strArray = new string[count];
      if (rectangleList.Count > 0)
      {
        for (int index = 0; index < count; ++index)
        {
          Rectangle rectangle = rectangleList[index];
          strArray[index] = $"R{rectangle.Y + 1}C{rectangle.X + 1}:R{rectangle.Bottom + 1}C{rectangle.Right + 1}";
          addressR1C1 += strArray[index];
          if (index != count - 1)
            addressR1C1 += ",";
        }
        return addressR1C1;
      }
      return $"R{taddr.FirstRow + 1}C{taddr.FirstCol + 1}:R{taddr.LastRow + 1}C{taddr.LastCol + 1}";
    }
  }

  [CLSCompliant(false)]
  public TAddr EnclosedRange
  {
    get
    {
      TAddr enclosedRange = new TAddr();
      if (this.m_format != null)
        enclosedRange = this.m_format.EncloseRange;
      if (this.m_condFMT12 != null)
        enclosedRange = this.m_condFMT12.EncloseRange;
      return enclosedRange;
    }
    set
    {
      if (this.m_format != null)
        this.m_format.EncloseRange = value;
      if (this.m_condFMT12 == null)
        return;
      this.m_condFMT12.EncloseRange = value;
    }
  }

  internal System.Collections.Generic.List<Rectangle> CellsRectangleList
  {
    get
    {
      if (this.m_format != null && this.m_format.CFNumber > (ushort) 0)
        return this.m_format.CellList;
      return this.m_condFMT12 != null ? this.m_condFMT12.CellList : new System.Collections.Generic.List<Rectangle>(1);
    }
  }

  public string[] CellsList
  {
    get
    {
      System.Collections.Generic.List<Rectangle> rectangleList = new System.Collections.Generic.List<Rectangle>();
      if (this.m_format != null && this.m_format.CFNumber > (ushort) 0)
        rectangleList = this.m_format.CellList;
      else if (this.m_condFMT12 != null)
        rectangleList = this.m_condFMT12.CellList;
      int count = rectangleList.Count;
      string[] cellsList = new string[count];
      for (int index = 0; index < count; ++index)
      {
        Rectangle rectangle = rectangleList[index];
        cellsList[index] = RangeImpl.GetAddressLocal(rectangle.Y + 1, rectangle.X + 1, rectangle.Bottom + 1, rectangle.Right + 1);
      }
      return cellsList;
    }
  }

  public System.Collections.Generic.List<Rectangle> CellRectangles
  {
    get => this.m_rangesOperations.CellList;
  }

  public CondFMTRecord CondFMTRecord
  {
    get => this.m_format;
    set => this.m_format = value;
  }

  public CondFmt12Record CondFMT12Record
  {
    get => this.m_condFMT12;
    set => this.m_condFMT12 = value;
  }

  public bool IsFutureRecord
  {
    get => this.m_futureRecord;
    set => this.m_futureRecord = value;
  }
}
