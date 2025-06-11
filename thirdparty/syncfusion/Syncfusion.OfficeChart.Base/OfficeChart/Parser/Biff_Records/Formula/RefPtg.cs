// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Formula.RefPtg
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

[Token(FormulaToken.tRef3)]
[Preserve(AllMembers = true)]
[Token(FormulaToken.tRef1)]
[Token(FormulaToken.tRef2)]
internal class RefPtg : Ptg, IRangeGetterToken, IRangeGetter, IToken3D, IRectGetter
{
  public const byte RowBitMask = 128 /*0x80*/;
  public const byte ColumnBitMask = 64 /*0x40*/;
  private const char DEF_R1C1_OPEN_BRACKET = '[';
  private const char DEF_R1C1_CLOSE_BRACKET = ']';
  public const string DEF_R1C1_ROW = "R";
  public const string DEF_R1C1_COLUMN = "C";
  public const char DEF_OPEN_BRACKET = '[';
  public const char DEF_CLOSE_BRACKET = ']';
  private int m_iRowIndex;
  private int m_iColumnIndex;
  private byte m_options;

  [Preserve]
  public RefPtg()
  {
  }

  [Preserve]
  public RefPtg(string strCell)
  {
    Match match = FormulaUtil.CellRegex.Match(strCell);
    this.SetCellA1(match.Groups["Column1"].Value, match.Groups["Row1"].Value);
    this.TokenCode = FormulaToken.tRef2;
  }

  [Preserve]
  public RefPtg(DataProvider provider, int offset, OfficeVersion version)
    : base(provider, offset, version)
  {
  }

  [Preserve]
  public RefPtg(int iRowIndex, int iColIndex, byte options)
  {
    this.m_iRowIndex = iRowIndex;
    this.m_iColumnIndex = iColIndex;
    this.m_options = options;
  }

  [Preserve]
  public RefPtg(int iCellRow, int iCellColumn, string strRow, string strColumn, bool bR1C1)
  {
    this.SetCell(iCellRow, iCellColumn, strRow, strColumn, bR1C1);
  }

  [Preserve]
  public RefPtg(RefPtg twin)
  {
    this.m_iRowIndex = twin.RowIndex;
    this.m_iColumnIndex = twin.m_iColumnIndex;
    this.m_options = twin.m_options;
  }

  [CLSCompliant(false)]
  public virtual int RowIndex
  {
    get => this.m_iRowIndex;
    set => this.m_iRowIndex = value;
  }

  public virtual bool IsRowIndexRelative
  {
    get => RefPtg.IsRelative(this.m_options, (byte) 128 /*0x80*/);
    set => this.m_options = RefPtg.SetRelative(this.m_options, (byte) 128 /*0x80*/, value);
  }

  public virtual bool IsColumnIndexRelative
  {
    get => RefPtg.IsRelative(this.m_options, (byte) 64 /*0x40*/);
    set => this.m_options = RefPtg.SetRelative(this.m_options, (byte) 64 /*0x40*/, value);
  }

  public virtual int ColumnIndex
  {
    get => this.m_iColumnIndex;
    set => this.m_iColumnIndex = value;
  }

  protected byte Options
  {
    get => this.m_options;
    set => this.m_options = value;
  }

  public override int GetSize(OfficeVersion version)
  {
    switch (version)
    {
      case OfficeVersion.Excel97to2003:
        return 5;
      case OfficeVersion.Excel2007:
      case OfficeVersion.Excel2010:
      case OfficeVersion.Excel2013:
        return 10;
      default:
        throw new ArgumentOutOfRangeException(nameof (version));
    }
  }

  public override string ToString()
  {
    return RefPtg.GetCellName(0, 0, this.m_iRowIndex, this.m_iColumnIndex, this.IsRowIndexRelative, this.IsColumnIndexRelative, false);
  }

  public override string ToString(
    FormulaUtil formulaUtil,
    int iRow,
    int iColumn,
    bool bR1C1,
    NumberFormatInfo numberFormat,
    bool isForSerialization)
  {
    return RefPtg.GetCellName(iRow, iColumn, this.RowIndex, this.ColumnIndex, this.IsRowIndexRelative, this.IsColumnIndexRelative, bR1C1);
  }

  public override byte[] ToByteArray(OfficeVersion version)
  {
    byte[] byteArray = base.ToByteArray(version);
    int index1 = 1;
    int index2;
    if (version == OfficeVersion.Excel97to2003)
    {
      if (this.m_iRowIndex > (int) ushort.MaxValue || this.m_iColumnIndex > (int) byte.MaxValue)
      {
        FormulaToken correspondingErrorCode = this.GetCorrespondingErrorCode();
        byteArray[0] = (byte) correspondingErrorCode;
      }
      BitConverter.GetBytes((ushort) this.m_iRowIndex).CopyTo((Array) byteArray, index1);
      int index3 = index1 + 2;
      byteArray[index3] = (byte) this.m_iColumnIndex;
      index2 = index3 + 1;
    }
    else
    {
      if (version == OfficeVersion.Excel97to2003)
        throw new ArgumentOutOfRangeException(nameof (version));
      BitConverter.GetBytes(this.m_iRowIndex).CopyTo((Array) byteArray, index1);
      int index4 = index1 + 4;
      BitConverter.GetBytes(this.m_iColumnIndex).CopyTo((Array) byteArray, index4);
      index2 = index4 + 4;
    }
    byteArray[index2] = this.m_options;
    return byteArray;
  }

  protected void SetCell(
    int iCellRow,
    int iCellColumn,
    string strRow,
    string strColumn,
    bool bR1C1)
  {
    if (bR1C1)
      this.SetCellR1C1(iCellRow, iCellColumn, strColumn, strRow);
    else
      this.SetCellA1(strColumn, strRow);
  }

  protected void SetCellA1(string strColumn, string strRow)
  {
    switch (strRow)
    {
      case null:
        throw new ArgumentNullException(nameof (strRow));
      case "":
        throw new ArgumentException("strRow - string cannot be empty");
      default:
        switch (strColumn)
        {
          case null:
            throw new ArgumentNullException(nameof (strColumn));
          case "":
            throw new ArgumentException("strColumn - string cannot be empty");
          default:
            bool bRelative;
            this.m_iColumnIndex = RefPtg.GetColumnIndex(0, strColumn, false, out bRelative);
            this.IsColumnIndexRelative = bRelative;
            this.m_iRowIndex = RefPtg.GetRowIndex(0, strRow, false, out bRelative);
            this.IsRowIndexRelative = bRelative;
            return;
        }
    }
  }

  protected void SetCellR1C1(int iCellRow, int iCellColumn, string column, string row)
  {
    bool bRelative;
    this.m_iColumnIndex = RefPtg.GetR1C1Index(iCellColumn, column, out bRelative);
    this.IsColumnIndexRelative = bRelative;
    this.m_iRowIndex = RefPtg.GetR1C1Index(iCellRow, row, out bRelative);
    this.IsRowIndexRelative = bRelative;
  }

  public static int GetR1C1Index(int iIndex, string strValue, out bool bRelative)
  {
    bRelative = false;
    if (strValue == null)
      return -1;
    int length1 = strValue.Length;
    if (length1 < 2)
    {
      bRelative = true;
      return iIndex;
    }
    strValue = strValue.Substring(1);
    int num1 = length1 - 1;
    if (strValue[0] == '[' && strValue[num1 - 1] == ']')
    {
      int length2 = num1 - 2;
      strValue = strValue.Substring(1, length2);
      bRelative = true;
    }
    int num2 = int.Parse(strValue);
    return !bRelative ? num2 - 1 : iIndex + num2;
  }

  public override Ptg Offset(int iRowOffset, int iColumnOffset, WorkbookImpl book)
  {
    RefPtg refPtg = (RefPtg) base.Offset(iRowOffset, iColumnOffset, book);
    int num1 = this.IsRowIndexRelative ? this.RowIndex + iRowOffset : this.RowIndex;
    int num2 = this.IsColumnIndexRelative ? this.ColumnIndex + iColumnOffset : this.ColumnIndex;
    if (num1 < 0 || num1 > book.MaxRowCount - 1 || num2 < 0 || num2 > book.MaxColumnCount - 1)
      return FormulaUtil.CreatePtg(this.GetCorrespondingErrorCode(), this.ToString(book.FormulaUtil), (IWorkbook) book);
    refPtg.RowIndex = num1;
    refPtg.ColumnIndex = num2;
    return (Ptg) refPtg;
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
    bChanged = false;
    RefPtg result = (RefPtg) base.Offset(iCurSheetIndex, iTokenRow, iTokenColumn, iSourceSheetIndex, rectSource, iDestSheetIndex, rectDest, out bChanged, book);
    int iRowOffset = rectDest.Top - rectSource.Top;
    int iColOffset = rectDest.Left - rectSource.Left;
    int rowIndex = this.RowIndex;
    int columnIndex = this.ColumnIndex;
    if (iCurSheetIndex == iSourceSheetIndex)
    {
      if (Ptg.RectangleContains(rectSource, rowIndex, columnIndex))
      {
        int iRowIndex = rowIndex + iRowOffset;
        int iColIndex = columnIndex + iColOffset;
        return result.UpdateReferencedCell(iCurSheetIndex, iDestSheetIndex, iRowIndex, iColIndex, ref bChanged, book);
      }
      if (Ptg.RectangleContains(rectDest, rowIndex, columnIndex))
        return this.ConvertToError();
    }
    else if (iCurSheetIndex == iDestSheetIndex && iSourceSheetIndex != iDestSheetIndex && Ptg.RectangleContains(rectDest, iTokenRow, iTokenColumn))
    {
      bChanged = true;
      return this.MoveIntoDifferentSheet(result, iSourceSheetIndex, rectSource, iDestSheetIndex, iRowOffset, iColOffset, book);
    }
    return (Ptg) result;
  }

  protected virtual Ptg MoveIntoDifferentSheet(
    RefPtg result,
    int iSourceSheetIndex,
    Rectangle rectSource,
    int iDestSheetIndex,
    int iRowOffset,
    int iColOffset,
    WorkbookImpl book)
  {
    int num = iSourceSheetIndex;
    bool flag = this.ReferencedCellMoved(rectSource);
    int rowIndex = result.RowIndex;
    int columnIndex = result.ColumnIndex;
    if (flag)
    {
      num = iDestSheetIndex;
      rowIndex += iRowOffset;
      columnIndex += iColOffset;
    }
    int code = (int) Ref3DPtg.IndexToCode(this.CodeToIndex());
    object[] objArray = new object[4]
    {
      (object) num,
      (object) rowIndex,
      (object) columnIndex,
      (object) this.m_options
    };
    return (Ptg) (result = (RefPtg) FormulaUtil.CreatePtg((FormulaToken) code, objArray));
  }

  private bool ReferencedCellMoved(Rectangle rectSource)
  {
    int rowIndex = this.RowIndex;
    int columnIndex = this.ColumnIndex;
    return Ptg.RectangleContains(rectSource, rowIndex, columnIndex);
  }

  public virtual int CodeToIndex() => RefPtg.CodeToIndex(this.TokenCode);

  public virtual FormulaToken GetCorrespondingErrorCode()
  {
    return RefErrorPtg.IndexToCode(this.CodeToIndex());
  }

  private Ptg UpdateReferencedCell(
    int iCurSheetIndex,
    int iDestSheetIndex,
    int iRowIndex,
    int iColIndex,
    ref bool bChanged,
    WorkbookImpl book)
  {
    if (iRowIndex < 0 || iRowIndex > book.MaxRowCount - 1 || iColIndex < 0 || iColIndex > book.MaxColumnCount - 1)
      return FormulaUtil.CreatePtg(this.GetCorrespondingErrorCode(), this.ToString());
    if (iCurSheetIndex == iDestSheetIndex)
    {
      this.RowIndex = iRowIndex;
      this.ColumnIndex = iColIndex;
      bChanged = true;
      return (Ptg) this;
    }
    bChanged = true;
    return FormulaUtil.CreatePtg(Ref3DPtg.IndexToCode(this.CodeToIndex()), (object) iDestSheetIndex, (object) iRowIndex, (object) iColIndex, (object) this.m_options);
  }

  public override Ptg ConvertPtgToNPtg(IWorkbook parent, int iRow, int iColumn)
  {
    int num1 = this.IsColumnIndexRelative ? this.ColumnIndex - iColumn : this.ColumnIndex;
    int num2 = this.IsRowIndexRelative ? this.RowIndex - iRow : this.RowIndex;
    RefNPtg ptg = (RefNPtg) FormulaUtil.CreatePtg(RefNPtg.IndexToCode(this.CodeToIndex()));
    if (parent.Version == OfficeVersion.Excel97to2003)
    {
      ptg.RowIndex = num2;
      ptg.ColumnIndex = num1;
    }
    else
    {
      RefPtg refPtg = (RefPtg) ptg;
      refPtg.RowIndex = num2;
      refPtg.ColumnIndex = num1;
    }
    ptg.Options = this.Options;
    return (Ptg) ptg;
  }

  public static bool IsRelative(byte Options, byte mask) => ((int) Options & (int) mask) != 0;

  public static byte SetRelative(byte Options, byte mask, bool value)
  {
    Options &= ~mask;
    if (value)
      Options += mask;
    return Options;
  }

  [CLSCompliant(false)]
  public static string GetCellName(
    int iCurCellRow,
    int iCurCellColumn,
    int row,
    int column,
    bool bRowRelative,
    bool bColumnRelative,
    bool bR1C1)
  {
    return !bR1C1 ? RefPtg.GetA1CellName(column, row, bColumnRelative, bRowRelative) : RefPtg.GetR1C1CellName(iCurCellRow, iCurCellColumn, row, column, bRowRelative, bColumnRelative);
  }

  private static string GetA1CellName(
    int column,
    int row,
    bool isColumnRelative,
    bool isRowRelative)
  {
    string str1 = RangeImpl.GetColumnName(column + 1);
    string str2 = (row + 1).ToString();
    if (!isRowRelative)
      str2 = "$" + str2;
    if (!isColumnRelative)
      str1 = "$" + str1;
    return str1 + str2;
  }

  public static string GetRCCellName(int column, int row)
  {
    return $"R{(row == 0 ? "" : $"[{(object) row}]")}C{(column == 0 ? "" : $"[{(object) column}]")}";
  }

  public static int GetColumnIndex(
    int iCellColumn,
    string columnName,
    bool bR1C1,
    out bool bRelative)
  {
    return !bR1C1 ? RefPtg.GetA1ColumnIndex(columnName, out bRelative) : RefPtg.GetR1C1Index(iCellColumn, columnName, out bRelative);
  }

  public static int GetA1ColumnIndex(string columnName, out bool bRelative)
  {
    switch (columnName)
    {
      case null:
        throw new ArgumentNullException(nameof (columnName));
      case "":
        throw new ArgumentException("columnName - string cannot be empty.");
      default:
        bRelative = columnName[0] != '$';
        if (!bRelative)
          columnName = columnName.Substring(1);
        return RangeImpl.GetColumnIndex(columnName) - 1;
    }
  }

  public static int GetRowIndex(int iCellRow, string strRowName, bool bR1C1, out bool bRelative)
  {
    return !bR1C1 ? RefPtg.GetA1RowIndex(strRowName, out bRelative) : RefPtg.GetR1C1Index(iCellRow, strRowName, out bRelative);
  }

  public static int GetA1RowIndex(string strRowName, out bool bRelative)
  {
    switch (strRowName)
    {
      case null:
        throw new ArgumentNullException(nameof (strRowName));
      case "":
        throw new ArgumentException("strRowName - string cannot be empty.");
      default:
        bRelative = strRowName[0] != '$';
        if (!bRelative)
          strRowName = strRowName.Substring(1);
        return int.Parse(strRowName) - 1;
    }
  }

  public static FormulaToken IndexToCode(int index)
  {
    switch (index)
    {
      case 1:
        return FormulaToken.tRef1;
      case 2:
        return FormulaToken.tRef2;
      case 3:
        return FormulaToken.tRef3;
      default:
        throw new ArgumentOutOfRangeException(nameof (index));
    }
  }

  public static int CodeToIndex(FormulaToken token)
  {
    switch (token)
    {
      case FormulaToken.tRef1:
      case FormulaToken.tRefErr1:
        return 1;
      case FormulaToken.tRef2:
      case FormulaToken.tRefErr2:
        return 2;
      case FormulaToken.tRef3:
      case FormulaToken.tRefErr3:
        return 3;
      default:
        throw new ArgumentOutOfRangeException("index");
    }
  }

  public static string GetR1C1CellName(
    int iCurRow,
    int iCurColumn,
    int row,
    int column,
    bool bRowRelative,
    bool bColumnRelative)
  {
    return RefPtg.GetR1C1Name(iCurRow, "R", row, bRowRelative) + RefPtg.GetR1C1Name(iCurColumn, "C", column, bColumnRelative);
  }

  public static string GetR1C1Name(int iCurIndex, string strStart, int iIndex, bool bIsRelative)
  {
    if (strStart == null)
      throw new ArgumentNullException(nameof (strStart));
    if (bIsRelative)
    {
      iIndex -= iCurIndex;
      if (iIndex == 0)
        return strStart;
    }
    string str = strStart;
    if (bIsRelative)
      str += (string) (object) '[';
    else
      ++iIndex;
    string r1C1Name = str + iIndex.ToString();
    if (bIsRelative)
      r1C1Name += (string) (object) ']';
    return r1C1Name;
  }

  public IRange GetRange(IWorkbook book, IWorksheet sheet)
  {
    if (sheet == null)
      throw new ArgumentNullException(nameof (sheet));
    return sheet.Range[this.m_iRowIndex + 1, this.m_iColumnIndex + 1];
  }

  public Rectangle GetRectangle()
  {
    return Rectangle.FromLTRB(this.ColumnIndex, this.RowIndex, this.ColumnIndex, this.RowIndex);
  }

  public Ptg UpdateRectangle(Rectangle rectangle)
  {
    RefPtg refPtg = (RefPtg) this.Clone();
    refPtg.ColumnIndex = rectangle.Left;
    refPtg.RowIndex = rectangle.Top;
    return (Ptg) refPtg;
  }

  public virtual Ptg Get3DToken(int iSheetReference)
  {
    FormulaToken code = Ref3DPtg.IndexToCode(RefPtg.CodeToIndex(this.TokenCode));
    Ptg ptg = (Ptg) new Ref3DPtg(iSheetReference, this.RowIndex, this.ColumnIndex, this.Options);
    ptg.TokenCode = code;
    return ptg;
  }

  public virtual Ptg ConvertToError()
  {
    return FormulaUtil.CreatePtg(this.GetCorrespondingErrorCode(), (object) this);
  }

  public override void InfillPTG(DataProvider provider, ref int offset, OfficeVersion version)
  {
    if (version == OfficeVersion.Excel97to2003)
    {
      this.m_iRowIndex = (int) provider.ReadUInt16(offset);
      offset += 2;
      this.m_iColumnIndex = (int) provider.ReadByte(offset++);
      this.m_options = provider.ReadByte(offset++);
    }
    else
    {
      if (version == OfficeVersion.Excel97to2003)
        throw new NotImplementedException();
      this.m_iRowIndex = provider.ReadInt32(offset);
      offset += 4;
      this.m_iColumnIndex = provider.ReadInt32(offset);
      offset += 4;
      this.m_options = provider.ReadByte(offset++);
    }
  }
}
