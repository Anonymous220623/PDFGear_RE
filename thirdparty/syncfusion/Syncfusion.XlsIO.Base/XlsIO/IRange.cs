// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IRange
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IRange : IParentApplication, IEnumerable<IRange>, IEnumerable
{
  string Address { get; }

  string AddressLocal { get; }

  string AddressGlobal { get; }

  string AddressR1C1 { get; }

  string AddressR1C1Local { get; }

  bool Boolean { get; set; }

  IBorders Borders { get; }

  IRange[] Cells { get; }

  int Column { get; }

  int ColumnGroupLevel { get; }

  double ColumnWidth { get; set; }

  int Count { get; }

  DateTime DateTime { get; set; }

  string DisplayText { get; }

  IRange End { get; }

  IRange EntireColumn { get; }

  IRange EntireRow { get; }

  string Error { get; set; }

  string Formula { get; set; }

  string FormulaArray { get; set; }

  string FormulaArrayR1C1 { get; set; }

  bool FormulaHidden { get; set; }

  DateTime FormulaDateTime { get; set; }

  string FormulaR1C1 { get; set; }

  bool FormulaBoolValue { get; set; }

  string FormulaErrorValue { get; set; }

  bool HasDataValidation { get; }

  bool HasBoolean { get; }

  bool HasDateTime { get; }

  bool HasFormula { get; }

  bool HasFormulaArray { get; }

  bool HasNumber { get; }

  bool HasRichText { get; }

  bool HasString { get; }

  bool HasStyle { get; }

  ExcelHAlign HorizontalAlignment { get; set; }

  IHyperLinks Hyperlinks { get; }

  int IndentLevel { get; set; }

  bool IsBlank { get; }

  bool IsBoolean { get; }

  bool IsError { get; }

  bool IsGroupedByColumn { get; }

  bool IsGroupedByRow { get; }

  bool IsInitialized { get; }

  int LastColumn { get; }

  int LastRow { get; }

  double Number { get; set; }

  string NumberFormat { get; set; }

  int Row { get; }

  int RowGroupLevel { get; }

  double RowHeight { get; set; }

  IRange[] Rows { get; }

  IRange[] Columns { get; }

  IStyle CellStyle { get; set; }

  string CellStyleName { get; set; }

  string Text { get; set; }

  TimeSpan TimeSpan { get; set; }

  string Value { get; set; }

  string CalculatedValue { get; }

  object Value2 { get; set; }

  ExcelVAlign VerticalAlignment { get; set; }

  IWorksheet Worksheet { get; }

  IRange this[int row, int column] { get; set; }

  IRange this[int row, int column, int lastRow, int lastColumn] { get; }

  IRange this[string name] { get; }

  IRange this[string name, bool IsR1C1Notation] { get; }

  IConditionalFormats ConditionalFormats { get; }

  IDataValidation DataValidation { get; }

  string FormulaStringValue { get; set; }

  double FormulaNumberValue { get; set; }

  bool HasFormulaBoolValue { get; }

  bool HasFormulaErrorValue { get; }

  bool HasFormulaDateTime { get; }

  bool HasFormulaNumberValue { get; }

  bool HasFormulaStringValue { get; }

  ICommentShape Comment { get; }

  IRichTextString RichText { get; }

  bool IsMerged { get; }

  IRange MergeArea { get; }

  bool WrapText { get; set; }

  bool HasExternalFormula { get; }

  ExcelIgnoreError IgnoreErrorOptions { get; set; }

  bool? IsStringsPreserved { get; set; }

  BuiltInStyles? BuiltInStyle { get; set; }

  string HtmlString { get; set; }

  IRange Activate();

  IRange Activate(bool scroll);

  IRange Group(ExcelGroupBy groupBy);

  IRange Group(ExcelGroupBy groupBy, bool bCollapsed);

  void SubTotal(int groupBy, ConsolidationFunction function, int[] totalList);

  void SubTotal(
    int groupBy,
    ConsolidationFunction function,
    int[] totalList,
    bool replace,
    bool pageBreaks,
    bool summaryBelowData);

  void SubTotal(
    int[] groupBy,
    ConsolidationFunction function,
    int[] totalList,
    bool replace,
    bool pageBreaks,
    bool summaryBelowData);

  double Sum();

  double Sum(bool considerDateAsNumber);

  double Average();

  double Average(bool considerDateAsNumber);

  double Min();

  double Min(bool considerDateAsNumber);

  double Max();

  double Max(bool considerDateAsNumber);

  IRange Trim();

  void Merge();

  void Merge(bool clearCells);

  IRange Ungroup(ExcelGroupBy groupBy);

  void UnMerge();

  void FreezePanes();

  void Clear();

  void Clear(bool isClearFormat);

  void Clear(ExcelClearOptions option);

  void Clear(ExcelMoveDirection direction);

  void Clear(ExcelMoveDirection direction, ExcelCopyRangeOptions options);

  void MoveTo(IRange destination);

  IRange CopyTo(IRange destination);

  IRange CopyTo(IRange destination, ExcelCopyRangeOptions options);

  IRange CopyTo(IRange destination, bool pasteLink);

  IRange CopyTo(IRange destination, ExcelCopyRangeOptions options, bool skipBlanks);

  IRange IntersectWith(IRange range);

  IRange MergeWith(IRange range);

  void AutofitRows();

  void AutofitColumns();

  ICommentShape AddComment();

  IRange FindFirst(string findValue, ExcelFindType flags);

  IRange FindFirst(double findValue, ExcelFindType flags);

  IRange FindFirst(bool findValue);

  IRange FindFirst(DateTime findValue);

  IRange FindFirst(TimeSpan findValue);

  IRange[] FindAll(string findValue, ExcelFindType flags);

  IRange[] FindAll(double findValue, ExcelFindType flags);

  IRange[] FindAll(bool findValue);

  IRange[] FindAll(DateTime findValue);

  IRange[] FindAll(TimeSpan findValue);

  void Replace(string oldValue, string newValue);

  void Replace(string oldValue, string newValue, ExcelFindOptions findOptions);

  void Replace(string oldValue, double newValue);

  void Replace(string oldValue, DateTime newValue);

  void Replace(string oldValue, string[] newValues, bool isVertical);

  void Replace(string oldValue, int[] newValues, bool isVertical);

  void Replace(string oldValue, double[] newValues, bool isVertical);

  void Replace(string oldValue, DataTable newValues, bool isFieldNamesShown);

  void Replace(string oldValue, DataColumn newValues, bool isFieldNamesShown);

  void BorderAround();

  void BorderAround(ExcelLineStyle borderLine);

  void BorderAround(ExcelLineStyle borderLine, Color borderColor);

  void BorderAround(ExcelLineStyle borderLine, ExcelKnownColors borderColor);

  void BorderInside();

  void BorderInside(ExcelLineStyle borderLine);

  void BorderInside(ExcelLineStyle borderLine, Color borderColor);

  void BorderInside(ExcelLineStyle borderLine, ExcelKnownColors borderColor);

  void BorderNone();

  void CollapseGroup(ExcelGroupBy groupBy);

  void ExpandGroup(ExcelGroupBy groupBy);

  void ExpandGroup(ExcelGroupBy groupBy, ExpandCollapseFlags flags);

  IRange[] GetDependents();

  IRange[] GetDependents(bool isEntireWorkbook);

  IRange[] GetDirectDependents();

  IRange[] GetDirectDependents(bool isEntireWorkbook);

  IRange[] GetPrecedents();

  IRange[] GetPrecedents(bool isEntireWorkbook);

  IRange[] GetDirectPrecedents();

  IRange[] GetDirectPrecedents(bool isEntireWorkbook);

  IRange Offset(int rowOffset, int columnOffset);

  IRange Resize(int rowSize, int columnSize);
}
