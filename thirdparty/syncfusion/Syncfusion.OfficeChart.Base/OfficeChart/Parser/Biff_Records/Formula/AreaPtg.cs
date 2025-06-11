// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Formula.AreaPtg
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation;
using System;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Formula;

[Token(FormulaToken.tArea3)]
[Token(FormulaToken.tArea2)]
[CLSCompliant(false)]
[Token(FormulaToken.tArea1)]
[Preserve(AllMembers = true)]
internal class AreaPtg : Ptg, IRangeGetterToken, IRangeGetter, IToken3D, IRectGetter
{
  private int m_iFirstRow;
  private int m_iLastRow;
  private int m_iFirstColumn;
  private byte m_firstOptions;
  private int m_iLastColumn;
  private byte m_lastOptions;

  [Preserve]
  public AreaPtg()
  {
  }

  [Preserve]
  public AreaPtg(string strFormula, IWorkbook book)
  {
    Match match = FormulaUtil.CellRangeRegex.Match(strFormula);
    string column1 = match.Groups["Column1"].Value;
    string row1 = match.Groups["Row1"].Value;
    string column2 = match.Groups["Column2"].Value;
    string row2 = match.Groups["Row2"].Value;
    if (!match.Success)
      throw new ArgumentException();
    this.SetArea(0, 0, row1, column1, row2, column2, false, book);
  }

  [Preserve]
  public AreaPtg(DataProvider provider, int offset, OfficeVersion version)
    : base(provider, offset, version)
  {
  }

  [Preserve]
  public AreaPtg(AreaPtg ptg)
  {
    this.m_iFirstRow = ptg.m_iFirstRow;
    this.m_iLastRow = ptg.m_iLastRow;
    this.m_iFirstColumn = ptg.m_iFirstColumn;
    this.m_firstOptions = ptg.m_firstOptions;
    this.m_iLastColumn = ptg.m_iLastColumn;
    this.m_lastOptions = ptg.m_lastOptions;
  }

  [Preserve]
  public AreaPtg(
    int iFirstRow,
    int iFirstCol,
    int iLastRow,
    int iLastCol,
    byte firstOptions,
    byte lastOptions)
  {
    this.m_iFirstRow = iFirstRow;
    this.m_iLastRow = iLastRow;
    this.m_iFirstColumn = iFirstCol;
    this.m_iLastColumn = iLastCol;
    this.m_firstOptions = firstOptions;
    this.m_lastOptions = lastOptions;
  }

  [Preserve]
  public AreaPtg(
    int iCellRow,
    int iCellColumn,
    string strFirstRow,
    string strFirstColumn,
    string strLastRow,
    string strLastColumn,
    bool bR1C1,
    IWorkbook book)
  {
    this.SetArea(iCellRow, iCellColumn, strFirstRow, strFirstColumn, strLastRow, strLastColumn, bR1C1, book);
  }

  public int FirstRow
  {
    get => this.m_iFirstRow;
    set => this.m_iFirstRow = value;
  }

  public bool IsFirstRowRelative
  {
    get => RefPtg.IsRelative(this.m_firstOptions, (byte) 128 /*0x80*/);
    set
    {
      this.m_firstOptions = RefPtg.SetRelative(this.m_firstOptions, (byte) 128 /*0x80*/, value);
    }
  }

  public bool IsFirstColumnRelative
  {
    get => RefPtg.IsRelative(this.m_firstOptions, (byte) 64 /*0x40*/);
    set => this.m_firstOptions = RefPtg.SetRelative(this.m_firstOptions, (byte) 64 /*0x40*/, value);
  }

  public int FirstColumn
  {
    get => this.m_iFirstColumn;
    set => this.m_iFirstColumn = value;
  }

  public int LastRow
  {
    get => this.m_iLastRow;
    set => this.m_iLastRow = value;
  }

  public bool IsLastRowRelative
  {
    get => RefPtg.IsRelative(this.m_lastOptions, (byte) 128 /*0x80*/);
    set => this.m_lastOptions = RefPtg.SetRelative(this.m_lastOptions, (byte) 128 /*0x80*/, value);
  }

  public bool IsLastColumnRelative
  {
    get => RefPtg.IsRelative(this.m_lastOptions, (byte) 64 /*0x40*/);
    set => this.m_lastOptions = RefPtg.SetRelative(this.m_lastOptions, (byte) 64 /*0x40*/, value);
  }

  public int LastColumn
  {
    get => this.m_iLastColumn;
    set => this.m_iLastColumn = value;
  }

  protected byte FirstOptions
  {
    get => this.m_firstOptions;
    set => this.m_firstOptions = value;
  }

  protected byte LastOptions
  {
    get => this.m_lastOptions;
    set => this.m_lastOptions = value;
  }

  protected void SetArea(
    int iCellRow,
    int iCellColumn,
    string row1,
    string column1,
    string row2,
    string column2,
    bool bR1C1,
    IWorkbook book)
  {
    bool bRelative;
    int num1 = RefPtg.GetColumnIndex(iCellColumn, column1, bR1C1, out bRelative);
    this.IsFirstColumnRelative = bRelative;
    int num2 = RefPtg.GetRowIndex(iCellRow, row1, bR1C1, out bRelative);
    this.IsFirstRowRelative = bRelative;
    int num3 = RefPtg.GetColumnIndex(iCellColumn, column2, bR1C1, out bRelative);
    this.IsLastColumnRelative = bRelative;
    int num4 = RefPtg.GetRowIndex(iCellRow, row2, bR1C1, out bRelative);
    this.IsLastRowRelative = bRelative;
    if (num2 == -1 && num4 == -1)
    {
      num2 = 0;
      num4 = book.MaxRowCount - 1;
    }
    else if (num1 == -1 && num3 == -1)
    {
      num1 = 0;
      num3 = book.MaxColumnCount - 1;
    }
    this.m_iFirstRow = num2;
    this.m_iLastRow = num4;
    this.m_iFirstColumn = num1;
    this.m_iLastColumn = num3;
  }

  public virtual int CodeToIndex() => AreaPtg.CodeToIndex(this.TokenCode);

  public virtual FormulaToken GetCorrespondingErrorCode()
  {
    return AreaErrorPtg.IndexToCode(this.CodeToIndex());
  }

  protected bool IsWholeRow(IWorkbook book)
  {
    return this.FirstRow == this.LastRow && this.IsWholeRows(book);
  }

  protected bool IsWholeRows(IWorkbook book)
  {
    return book != null && this.IsFirstRowRelative == this.IsLastRowRelative && this.FirstColumn == 0 && this.LastColumn == book.MaxColumnCount - 1;
  }

  protected bool IsWholeColumns(IWorkbook book)
  {
    return book != null && this.IsFirstColumnRelative == this.IsLastColumnRelative && this.FirstRow == 0 && this.LastRow == book.MaxRowCount - 1;
  }

  protected bool IsWholeColumn(IWorkbook book)
  {
    return this.FirstColumn == this.LastColumn && this.IsWholeColumns(book);
  }

  public virtual AreaPtg ConvertToErrorPtg() => (AreaPtg) new AreaErrorPtg(this);

  public override void InfillPTG(DataProvider provider, ref int offset, OfficeVersion version)
  {
    if (version == OfficeVersion.Excel97to2003)
    {
      this.m_iFirstRow = (int) provider.ReadUInt16(offset);
      offset += 2;
      this.m_iLastRow = (int) provider.ReadUInt16(offset);
      offset += 2;
      this.m_iFirstColumn = (int) provider.ReadByte(offset++);
      this.m_firstOptions = provider.ReadByte(offset++);
      this.m_iLastColumn = (int) provider.ReadByte(offset++);
      this.m_lastOptions = provider.ReadByte(offset++);
    }
    else
    {
      if (version == OfficeVersion.Excel97to2003)
        throw new NotImplementedException();
      this.m_iFirstRow = provider.ReadInt32(offset);
      offset += 4;
      this.m_iLastRow = provider.ReadInt32(offset);
      offset += 4;
      this.m_iFirstColumn = provider.ReadInt32(offset);
      offset += 4;
      this.m_firstOptions = provider.ReadByte(offset++);
      this.m_iLastColumn = provider.ReadInt32(offset);
      offset += 4;
      this.m_lastOptions = provider.ReadByte(offset++);
    }
  }

  public static FormulaToken IndexToCode(int index)
  {
    switch (index)
    {
      case 1:
        return FormulaToken.tArea1;
      case 2:
        return FormulaToken.tArea2;
      case 3:
        return FormulaToken.tArea3;
      default:
        throw new ArgumentOutOfRangeException(nameof (index));
    }
  }

  public static int CodeToIndex(FormulaToken code)
  {
    switch (code)
    {
      case FormulaToken.tArea1:
        return 1;
      case FormulaToken.tArea2:
        return 2;
      case FormulaToken.tArea3:
        return 3;
      default:
        throw new ArgumentOutOfRangeException("index");
    }
  }

  public override int GetSize(OfficeVersion version)
  {
    switch (version)
    {
      case OfficeVersion.Excel97to2003:
        return 9;
      case OfficeVersion.Excel2007:
      case OfficeVersion.Excel2010:
      case OfficeVersion.Excel2013:
        return 19;
      default:
        throw new ArgumentOutOfRangeException(nameof (version));
    }
  }

  public override string ToString(
    FormulaUtil formulaUtil,
    int iRow,
    int iColumn,
    bool bR1C1,
    NumberFormatInfo numberFormat,
    bool isForSerialization)
  {
    WorkbookImpl parentWorkbook = formulaUtil != null ? (WorkbookImpl) formulaUtil.ParentWorkbook : (WorkbookImpl) null;
    bool flag1 = this.IsWholeRows((IWorkbook) parentWorkbook);
    bool flag2 = this.IsWholeColumns((IWorkbook) parentWorkbook);
    if (flag1 && bR1C1)
      return $"{RefPtg.GetR1C1Name(iRow, "R", this.FirstRow, this.IsFirstRowRelative)}:{RefPtg.GetR1C1Name(iRow, "R", this.LastRow, this.IsFirstRowRelative)}";
    if (flag2 && bR1C1)
      return $"{RefPtg.GetR1C1Name(iColumn, "C", this.FirstColumn, this.IsFirstColumnRelative)}:{RefPtg.GetR1C1Name(iColumn, "C", this.LastColumn, this.IsFirstColumnRelative)}";
    if (flag1)
    {
      (this.FirstRow + 1).ToString();
      return $"${(this.FirstRow + 1).ToString()}:${(this.LastRow + 1).ToString()}";
    }
    if (!flag2)
      return $"{RefPtg.GetCellName(iRow, iColumn, this.FirstRow, this.FirstColumn, this.IsFirstRowRelative, this.IsFirstColumnRelative, bR1C1)}:{RefPtg.GetCellName(iRow, iColumn, this.LastRow, this.LastColumn, this.IsLastRowRelative, this.IsLastColumnRelative, bR1C1)}";
    RangeImpl.GetColumnName(this.FirstColumn + 1);
    return $"${RangeImpl.GetColumnName(this.FirstColumn + 1)}:${RangeImpl.GetColumnName(this.LastColumn + 1)}";
  }

  public override byte[] ToByteArray(OfficeVersion version)
  {
    byte[] byteArray = base.ToByteArray(version);
    int index1 = 1;
    if (version == OfficeVersion.Excel97to2003)
    {
      if (this.m_iFirstRow > (int) ushort.MaxValue || this.m_iLastRow > (int) ushort.MaxValue || this.m_iFirstColumn > (int) byte.MaxValue || this.m_iLastColumn > (int) byte.MaxValue)
      {
        FormulaToken correspondingErrorCode = this.GetCorrespondingErrorCode();
        byteArray[0] = (byte) correspondingErrorCode;
      }
      BitConverter.GetBytes((ushort) this.m_iFirstRow).CopyTo((Array) byteArray, index1);
      int index2 = index1 + 2;
      BitConverter.GetBytes((ushort) this.m_iLastRow).CopyTo((Array) byteArray, index2);
      int num1 = index2 + 2;
      byte[] numArray1 = byteArray;
      int index3 = num1;
      int num2 = index3 + 1;
      int iFirstColumn = (int) (byte) this.m_iFirstColumn;
      numArray1[index3] = (byte) iFirstColumn;
      byte[] numArray2 = byteArray;
      int index4 = num2;
      int num3 = index4 + 1;
      int firstOptions = (int) this.m_firstOptions;
      numArray2[index4] = (byte) firstOptions;
      byte[] numArray3 = byteArray;
      int index5 = num3;
      index1 = index5 + 1;
      int iLastColumn = (int) (byte) this.m_iLastColumn;
      numArray3[index5] = (byte) iLastColumn;
    }
    else if (version != OfficeVersion.Excel97to2003)
    {
      BitConverter.GetBytes(this.m_iFirstRow).CopyTo((Array) byteArray, index1);
      int index6 = index1 + 4;
      BitConverter.GetBytes(this.m_iLastRow).CopyTo((Array) byteArray, index6);
      int index7 = index6 + 4;
      BitConverter.GetBytes(this.m_iFirstColumn).CopyTo((Array) byteArray, index7);
      int num = index7 + 4;
      byte[] numArray = byteArray;
      int index8 = num;
      int index9 = index8 + 1;
      int firstOptions = (int) this.m_firstOptions;
      numArray[index8] = (byte) firstOptions;
      BitConverter.GetBytes(this.m_iLastColumn).CopyTo((Array) byteArray, index9);
      index1 = index9 + 4;
    }
    byteArray[index1] = this.m_lastOptions;
    return byteArray;
  }

  public override Ptg Offset(int iRowOffset, int iColumnOffset, WorkbookImpl book)
  {
    AreaPtg areaPtg = (AreaPtg) base.Offset(iRowOffset, iColumnOffset, book);
    int num1 = this.IsFirstRowRelative ? this.FirstRow + iRowOffset : this.FirstRow;
    int num2 = this.IsFirstColumnRelative ? this.FirstColumn + iColumnOffset : this.FirstColumn;
    int num3 = this.IsLastRowRelative ? this.LastRow + iRowOffset : this.LastRow;
    int num4 = this.IsLastColumnRelative ? this.LastColumn + iColumnOffset : this.LastColumn;
    if (num1 < 0 || num1 > book.MaxRowCount - 1 || num2 < 0 || num2 > book.MaxColumnCount - 1 || num3 < 0 || num3 > book.MaxRowCount - 1 || num4 < 0 || num4 > book.MaxColumnCount - 1)
      return FormulaUtil.CreatePtg(this.GetCorrespondingErrorCode(), (object) this);
    areaPtg.FirstRow = num1;
    areaPtg.FirstColumn = num2;
    areaPtg.LastRow = num3;
    areaPtg.LastColumn = num4;
    return (Ptg) areaPtg;
  }

  public override Ptg Offset(
    int iCurSheetIndex,
    int iTokenRow,
    int iTokenColumn,
    int iSourceSheetIndex,
    Rectangle rectSource,
    int iDestSheetIndex,
    Rectangle rectDest,
    out bool bChanged,
    WorkbookImpl book)
  {
    AreaPtg result = (AreaPtg) base.Offset(iCurSheetIndex, iTokenRow, iTokenColumn, iSourceSheetIndex, rectSource, iDestSheetIndex, rectDest, out bChanged, book);
    if (iCurSheetIndex == iSourceSheetIndex)
      return this.MoveReferencedArea(iSourceSheetIndex, rectSource, iDestSheetIndex, rectDest, ref bChanged, book);
    if (iCurSheetIndex != iDestSheetIndex || iSourceSheetIndex == iDestSheetIndex || !Ptg.RectangleContains(rectDest, iTokenRow, iTokenColumn))
      return (Ptg) result;
    int iRowOffset = rectDest.Top - rectSource.Top;
    int iColOffset = rectDest.Left - rectSource.Left;
    bChanged = true;
    return result.MoveIntoDifferentSheet(result, iSourceSheetIndex, rectSource, iDestSheetIndex, iRowOffset, iColOffset);
  }

  public override Ptg ConvertPtgToNPtg(IWorkbook parent, int iRow, int iColumn)
  {
    AreaNPtg ptg = (AreaNPtg) FormulaUtil.CreatePtg(AreaNPtg.IndexToCode(this.CodeToIndex()));
    bool flag1 = this.IsWholeRows(parent);
    bool flag2 = this.IsWholeColumns(parent);
    int num1 = !this.IsFirstRowRelative || flag2 ? this.FirstRow : this.FirstRow - iRow;
    short num2 = !this.IsFirstColumnRelative || flag1 ? (short) this.FirstColumn : (short) (this.FirstColumn - iColumn);
    int num3 = !this.IsLastRowRelative || flag2 ? this.LastRow : this.LastRow - iRow;
    short num4 = !this.IsLastColumnRelative || flag1 ? (short) this.LastColumn : (short) (this.LastColumn - iColumn);
    ptg.FirstRow = num1;
    ptg.FirstColumn = num2;
    ptg.LastRow = num3;
    ptg.LastColumn = num4;
    ptg.FirstOptions = this.FirstOptions;
    ptg.LastOptions = this.LastOptions;
    return (Ptg) ptg;
  }

  public AreaPtg ConvertFullRowColumnAreaPtgs(bool bFromExcel07To97)
  {
    int iRows1;
    int iColumns1;
    UtilityMethods.GetMaxRowColumnCount(out iRows1, out iColumns1, OfficeVersion.Excel2007);
    int iRows2;
    int iColumns2;
    UtilityMethods.GetMaxRowColumnCount(out iRows2, out iColumns2, OfficeVersion.Excel97to2003);
    if (bFromExcel07To97)
    {
      if (this.FirstColumn == 0 && this.LastColumn == iColumns1 - 1)
        this.LastColumn = iColumns2 - 1;
      else if (this.FirstRow == 0 && this.LastRow == iRows1 - 1)
        this.LastRow = iRows2 - 1;
      else if (this.FirstColumn > iColumns2 || this.LastColumn > iColumns2 || this.FirstRow > iRows2 || this.LastRow > iRows2)
        return this.ConvertToErrorPtg();
    }
    else if (this.FirstColumn == 0 && this.LastColumn == iColumns2 - 1)
      this.LastColumn = iColumns1 - 1;
    else if (this.FirstRow == 0 && this.LastRow == iRows2 - 1)
      this.LastRow = iRows1 - 1;
    return this;
  }

  private Ptg MoveIntoDifferentSheet(
    AreaPtg result,
    int iSourceSheetIndex,
    Rectangle rectSource,
    int iDestSheetIndex,
    int iRowOffset,
    int iColOffset)
  {
    int num = iSourceSheetIndex;
    bool flag = this.ReferencedAreaMoved(rectSource);
    int firstRow = result.FirstRow;
    int firstColumn = result.FirstColumn;
    int lastRow = result.LastRow;
    int lastColumn = result.LastColumn;
    if (flag)
    {
      num = iDestSheetIndex;
      firstRow += iRowOffset;
      firstColumn += iColOffset;
      lastRow += iRowOffset;
      lastColumn += iColOffset;
    }
    return FormulaUtil.CreatePtg(Area3DPtg.IndexToCode(this.CodeToIndex()), (object) num, (object) firstRow, (object) firstColumn, (object) lastRow, (object) lastColumn, (object) this.m_firstOptions, (object) this.m_lastOptions);
  }

  private bool ReferencedAreaMoved(Rectangle rectSource)
  {
    return Ptg.RectangleContains(rectSource, this.FirstRow, this.FirstColumn) && Ptg.RectangleContains(rectSource, this.LastRow, this.LastColumn);
  }

  private Ptg UpdateReferencedArea(
    int iCurSheetIndex,
    int iDestSheetIndex,
    int iRowOffset,
    int iColOffset,
    ref bool bChanged,
    WorkbookImpl book)
  {
    if (this.m_iLastRow + iRowOffset < 0 || this.m_iFirstColumn + iColOffset < 0 || this.m_iLastRow + iRowOffset > book.MaxRowCount - 1 || this.m_iLastColumn + iColOffset > book.MaxColumnCount - 1)
      return FormulaUtil.CreatePtg(this.GetCorrespondingErrorCode(), this.ToString(book.FormulaUtil), (IWorkbook) book);
    if (iCurSheetIndex == iDestSheetIndex)
    {
      this.m_iFirstRow += iRowOffset;
      this.m_iLastRow += iRowOffset;
      this.m_iFirstColumn += iColOffset;
      this.m_iLastColumn += iColOffset;
      bChanged = true;
      return (Ptg) this;
    }
    bChanged = true;
    return FormulaUtil.CreatePtg(Area3DPtg.IndexToCode(this.CodeToIndex()), (object) iDestSheetIndex, (object) (this.m_iFirstRow + iRowOffset), (object) (this.m_iFirstColumn + iColOffset), (object) (this.m_iLastRow + iRowOffset), (object) (this.m_iLastColumn + iColOffset), (object) this.m_firstOptions, (object) this.m_lastOptions);
  }

  private Ptg UpdateFirstCell(
    int iCurSheetIndex,
    int iDestSheetIndex,
    int iRowOffset,
    int iColOffset,
    ref bool bChanged,
    IWorkbook book)
  {
    int num1 = this.m_iFirstRow + iRowOffset;
    int num2 = this.m_iFirstColumn + iColOffset;
    if (num1 < 0 || num2 < 0 || num1 > book.MaxRowCount - 1 || num2 > book.MaxColumnCount - 1)
      return FormulaUtil.CreatePtg(this.GetCorrespondingErrorCode(), this.ToString());
    if (num1 > this.m_iLastRow || num2 > this.m_iLastColumn)
      return (Ptg) this;
    if (iCurSheetIndex == iDestSheetIndex)
    {
      this.m_iFirstRow = num1;
      this.m_iFirstColumn = num2;
      bChanged = true;
      return (Ptg) this;
    }
    bChanged = true;
    return FormulaUtil.CreatePtg(Area3DPtg.IndexToCode(this.CodeToIndex()), (object) iDestSheetIndex, (object) num1, (object) num2, (object) this.m_iLastRow, (object) this.m_iLastColumn, (object) this.m_firstOptions, (object) this.m_lastOptions);
  }

  private Ptg UpdateLastCell(
    int iCurSheetIndex,
    int iDestSheetIndex,
    int iRowOffset,
    int iColOffset,
    ref bool bChanged,
    IWorkbook book)
  {
    int num1 = this.m_iLastRow + iRowOffset;
    int num2 = this.m_iLastColumn + iColOffset;
    if (num1 < 0 || num2 < 0 || num1 > book.MaxRowCount - 1 || num2 > book.MaxColumnCount - 1)
      return FormulaUtil.CreatePtg(this.GetCorrespondingErrorCode(), this.ToString());
    if (num1 < this.m_iFirstRow || num2 < this.m_iFirstColumn)
      return (Ptg) this;
    if (iCurSheetIndex == iDestSheetIndex)
    {
      this.m_iLastRow = num1;
      this.m_iLastColumn = num2;
      bChanged = true;
      return (Ptg) this;
    }
    bChanged = true;
    return FormulaUtil.CreatePtg(Area3DPtg.IndexToCode(this.CodeToIndex()), (object) iDestSheetIndex, (object) this.m_iFirstRow, (object) this.m_iFirstColumn, (object) num1, (object) num2, (object) this.m_firstOptions, (object) this.m_lastOptions);
  }

  private bool FullFirstRowMove(Rectangle rectSource)
  {
    return Ptg.RectangleContains(rectSource, this.FirstRow, this.FirstColumn) && Ptg.RectangleContains(rectSource, this.FirstRow, this.LastColumn);
  }

  private bool FullLastRowMove(Rectangle rectSource)
  {
    return Ptg.RectangleContains(rectSource, this.LastRow, this.FirstColumn) && Ptg.RectangleContains(rectSource, this.LastRow, this.LastColumn);
  }

  private bool FullFirstColMove(Rectangle rectSource)
  {
    return Ptg.RectangleContains(rectSource, this.FirstRow, this.FirstColumn) && Ptg.RectangleContains(rectSource, this.LastRow, this.FirstColumn);
  }

  private bool FullLastColMove(Rectangle rectSource)
  {
    return Ptg.RectangleContains(rectSource, this.FirstRow, this.LastColumn) && Ptg.RectangleContains(rectSource, this.LastRow, this.LastColumn);
  }

  private Ptg MoveReferencedArea(
    int iSourceSheetIndex,
    Rectangle rectSource,
    int iDestSheetIndex,
    Rectangle rectDest,
    ref bool bChanged,
    WorkbookImpl book)
  {
    int iRowOffset = rectDest.Top - rectSource.Top;
    int num = rectSource.Top - rectDest.Top;
    int iColOffset = rectDest.Left - rectSource.Left;
    if (iRowOffset == 0 && iColOffset == 0 && iSourceSheetIndex == iDestSheetIndex)
      return (Ptg) this;
    rectSource = Rectangle.FromLTRB(rectSource.Left, rectSource.Top, this.m_iLastColumn, this.m_iLastRow);
    rectDest = Rectangle.FromLTRB(rectDest.Left, rectDest.Top, this.m_iLastColumn, this.m_iLastRow + 1);
    bool flag1 = iRowOffset <= 0 ? Ptg.RectangleContains(rectDest, this.FirstRow, this.FirstColumn) : Ptg.RectangleContains(rectSource, this.FirstRow, this.FirstColumn);
    bool flag2 = num <= 0 ? Ptg.RectangleContains(rectSource, this.LastRow, this.LastColumn) : Ptg.RectangleContains(rectDest, this.LastRow, this.LastColumn);
    if (this.m_iLastRow >= book.MaxRowCount - 1 || this.m_iLastColumn >= book.MaxColumnCount - 1)
    {
      flag1 = false;
      flag2 = false;
    }
    if (!flag1 && !flag2)
    {
      if (book.MaxRowCount - 1 == this.LastRow && this.FirstRow == 0)
        return (Ptg) this;
      Rectangle rect = Rectangle.FromLTRB(this.FirstColumn, this.FirstRow, this.LastColumn, this.LastRow);
      bool flag3 = Ptg.RectangleContains(rect, rectDest.Top, rect.Left);
      bool flag4 = Ptg.RectangleContains(rect, rectDest.Bottom, rect.Right);
      if (!flag3 || !flag4)
        return (Ptg) this;
    }
    if (iColOffset == 0 && iSourceSheetIndex == iDestSheetIndex)
      return this.VerticalMove(iSourceSheetIndex, rectSource, iRowOffset, rectDest, ref bChanged, book);
    if (iRowOffset == 0 && iSourceSheetIndex == iDestSheetIndex)
      return this.HorizontalMove(iSourceSheetIndex, rectSource, iColOffset, rectDest, ref bChanged, book);
    return flag1 || flag2 ? this.UpdateReferencedArea(iSourceSheetIndex, iDestSheetIndex, iRowOffset, iColOffset, ref bChanged, book) : (Ptg) this;
  }

  private Ptg VerticalMove(
    int iSourceSheetIndex,
    Rectangle rectSource,
    int iRowOffset,
    Rectangle rectDest,
    ref bool bChanged,
    WorkbookImpl book)
  {
    bool flag1 = iRowOffset < 0 ? this.FullFirstRowMove(rectSource) : this.FullFirstRowMove(rectDest);
    if (rectSource.X <= rectDest.X && iRowOffset > 0 && !flag1)
    {
      bool flag2 = !this.FullFirstRowMove(rectDest);
    }
    bool flag3 = this.m_iFirstRow >= rectSource.Y;
    bool flag4 = iRowOffset < 0 ? this.FullLastRowMove(rectDest) : this.FullLastRowMove(rectSource);
    Rectangle a = Rectangle.FromLTRB(this.FirstColumn, this.FirstRow, this.LastColumn, this.LastRow);
    if (flag3 && flag4)
      return this.UpdateReferencedArea(iSourceSheetIndex, iSourceSheetIndex, iRowOffset, 0, ref bChanged, book);
    if (flag3)
      return this.FirstRowVerticalMove(iSourceSheetIndex, iRowOffset, rectSource, rectDest, ref bChanged, (IWorkbook) book);
    if (flag4)
      return this.LastRowVerticalMove(iSourceSheetIndex, iRowOffset, rectSource, rectDest, ref bChanged, (IWorkbook) book);
    if (!Rectangle.Intersect(a, rectDest).IsEmpty)
    {
      if (this.LastRow == rectDest.Bottom || this.FirstRow == rectDest.Top)
        return (Ptg) this;
      if (iRowOffset < 0)
        this.LastRow = (int) (ushort) (rectDest.Top - 1);
      else
        this.FirstRow = (int) (ushort) (rectDest.Bottom + 1);
    }
    return (Ptg) this;
  }

  private Ptg FirstRowVerticalMove(
    int iSourceSheetIndex,
    int iRowOffset,
    Rectangle rectSource,
    Rectangle rectDest,
    ref bool bChanged,
    IWorkbook book)
  {
    if (iRowOffset < 0)
      return this.UpdateFirstCell(iSourceSheetIndex, iSourceSheetIndex, iRowOffset, 0, ref bChanged, book);
    if (this.FirstRow + iRowOffset > this.LastRow)
      return (Ptg) this;
    this.FirstRow = rectDest.Top > rectSource.Bottom ? (int) (ushort) (this.FirstRow + iRowOffset) : (int) (ushort) (rectSource.Top + iRowOffset);
    return (Ptg) this;
  }

  private Ptg LastRowVerticalMove(
    int iSourceSheetIndex,
    int iRowOffset,
    Rectangle rectSource,
    Rectangle rectDest,
    ref bool bChanged,
    IWorkbook book)
  {
    if (iRowOffset > 0)
      return this.UpdateLastCell(iSourceSheetIndex, iSourceSheetIndex, iRowOffset, 0, ref bChanged, book);
    if (this.LastRow + iRowOffset >= this.FirstRow)
      this.LastRow = (int) (ushort) (this.LastRow + iRowOffset);
    return (Ptg) this;
  }

  private Ptg HorizontalMove(
    int iSourceSheetIndex,
    Rectangle rectSource,
    int iColOffset,
    Rectangle rectDest,
    ref bool bChanged,
    WorkbookImpl book)
  {
    bool flag1 = this.FullFirstColMove(rectSource);
    bool flag2 = this.FullLastColMove(rectSource);
    Rectangle rectangle = Rectangle.FromLTRB(this.FirstColumn, this.FirstRow, this.LastColumn, this.LastRow);
    if (flag1 && flag2)
      return this.UpdateReferencedArea(iSourceSheetIndex, iSourceSheetIndex, 0, iColOffset, ref bChanged, book);
    if (flag1)
      return this.FirstColumnHorizontalMove(iSourceSheetIndex, iColOffset, rectSource, rectDest, ref bChanged, (IWorkbook) book);
    if (flag2)
      return this.LastColumnHorizontalMove(iSourceSheetIndex, iColOffset, rectSource, rectDest, ref bChanged, (IWorkbook) book);
    if (!flag1 && !flag2)
      return (Ptg) this;
    if (!Rectangle.Intersect(rectangle, rectDest).IsEmpty)
    {
      if (this.InsideRectangle(rectDest, rectangle) && this.OutsideRectangle(rectSource, rectangle))
        return (Ptg) this.ConvertToErrorPtg();
      if (this.LastColumn == rectDest.Right || this.FirstColumn == rectDest.Left)
        return (Ptg) this;
      if (iColOffset < 0)
        this.LastColumn = (int) (byte) (rectDest.Left - 1);
      else
        this.FirstColumn = (int) (byte) (rectDest.Right + 1);
    }
    return (Ptg) this;
  }

  private bool OutsideRectangle(Rectangle owner, Rectangle toCheck)
  {
    return owner.Top > toCheck.Bottom || owner.Bottom < toCheck.Top || owner.Left > toCheck.Right || owner.Right < toCheck.Left;
  }

  private bool InsideRectangle(Rectangle owner, Rectangle toCheck)
  {
    return owner.Left <= toCheck.Left && owner.Right >= toCheck.Right && owner.Top <= toCheck.Top && owner.Bottom >= toCheck.Bottom;
  }

  private Ptg FirstColumnHorizontalMove(
    int iSourceSheetIndex,
    int iColOffset,
    Rectangle rectSource,
    Rectangle rectDest,
    ref bool bChanged,
    IWorkbook book)
  {
    if (iColOffset < 0)
      return this.UpdateFirstCell(iSourceSheetIndex, iSourceSheetIndex, 0, iColOffset, ref bChanged, book);
    if (this.FirstColumn + iColOffset > this.LastColumn)
      return (Ptg) this;
    this.FirstColumn = rectDest.Left > rectSource.Right ? (int) (byte) (this.FirstColumn + iColOffset) : (int) (byte) (rectSource.Right + 1);
    return (Ptg) this;
  }

  private Ptg LastColumnHorizontalMove(
    int iSourceSheetIndex,
    int iColOffset,
    Rectangle rectSource,
    Rectangle rectDest,
    ref bool bChanged,
    IWorkbook book)
  {
    if (iColOffset > 0)
      return this.UpdateLastCell(iSourceSheetIndex, iSourceSheetIndex, 0, iColOffset, ref bChanged, book);
    if (this.LastColumn + iColOffset >= this.FirstColumn)
      this.LastColumn = rectDest.Right > rectSource.Left ? (int) (byte) (this.LastColumn + iColOffset) : (int) (byte) (rectSource.Left + 1);
    return (Ptg) this;
  }

  public IRange GetRange(IWorkbook book, IWorksheet sheet)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    if (this.FirstColumn > this.LastColumn)
    {
      int lastColumn = this.LastColumn;
      this.LastColumn = this.FirstColumn;
      this.FirstColumn = lastColumn;
    }
    return sheet[this.FirstRow + 1, this.FirstColumn + 1, this.LastRow + 1, this.LastColumn + 1];
  }

  public Rectangle GetRectangle()
  {
    return Rectangle.FromLTRB(this.FirstColumn, this.FirstRow, this.LastColumn, this.LastRow);
  }

  public Ptg UpdateRectangle(Rectangle rectangle)
  {
    AreaPtg areaPtg = (AreaPtg) this.Clone();
    areaPtg.FirstColumn = rectangle.Left;
    areaPtg.LastColumn = rectangle.Right;
    areaPtg.FirstRow = rectangle.Top;
    areaPtg.LastRow = rectangle.Bottom;
    return (Ptg) areaPtg;
  }

  public virtual Ptg ConvertToError()
  {
    return FormulaUtil.CreatePtg(this.GetCorrespondingErrorCode(), (object) this);
  }

  public Ptg Get3DToken(int iSheetReference)
  {
    FormulaToken code = Area3DPtg.IndexToCode(AreaPtg.CodeToIndex(this.TokenCode));
    Ptg ptg = (Ptg) new Area3DPtg(iSheetReference, this.FirstRow, this.FirstColumn, this.LastRow, this.LastColumn, this.FirstOptions, this.LastOptions);
    ptg.TokenCode = code;
    return ptg;
  }
}
