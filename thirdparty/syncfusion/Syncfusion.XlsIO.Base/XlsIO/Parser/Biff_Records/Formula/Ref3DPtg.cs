// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Formula.Ref3DPtg
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using Syncfusion.XlsIO.Implementation.Exceptions;
using System;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Formula;

[Token(FormulaToken.tRef3d3)]
[Preserve(AllMembers = true)]
[Token(FormulaToken.tRef3d1)]
[Token(FormulaToken.tRef3d2)]
[CLSCompliant(false)]
public class Ref3DPtg : RefPtg, IRangeGetter, ISheetReference, IReference
{
  private ushort m_usRefIndex;

  [Preserve]
  public Ref3DPtg()
  {
  }

  [Preserve]
  public Ref3DPtg(DataProvider provider, int offset, ExcelVersion version)
    : base(provider, offset, version)
  {
  }

  [Preserve]
  public Ref3DPtg(string strFormula, IWorkbook parent)
  {
    Match match = FormulaUtil.Cell3DRegex.Match(strFormula);
    if (!match.Success)
      throw new ArgumentException(nameof (strFormula));
    string sheetName = match.Groups["SheetName"].Value;
    string strRow = match.Groups["Row1"].Value;
    this.SetCellA1(match.Groups["Column1"].Value, strRow);
    this.SetSheetIndex(sheetName, parent);
  }

  [Preserve]
  public Ref3DPtg(
    int iCellRow,
    int iCellColumn,
    int iSheetIndex,
    string strRow,
    string strColumn,
    bool bR1C1)
    : base(iCellRow, iCellColumn, strRow, strColumn, bR1C1)
  {
    this.m_usRefIndex = (ushort) iSheetIndex;
  }

  [Preserve]
  public Ref3DPtg(int iSheetIndex, int iRowIndex, int iColIndex, byte options)
    : base(iRowIndex, iColIndex, options)
  {
    this.m_usRefIndex = (ushort) iSheetIndex;
  }

  [Preserve]
  public Ref3DPtg(Ref3DPtg twin)
    : base((RefPtg) twin)
  {
    this.m_usRefIndex = twin.m_usRefIndex;
  }

  public ushort RefIndex
  {
    get => this.m_usRefIndex;
    set => this.m_usRefIndex = value;
  }

  public override int GetSize(ExcelVersion version) => base.GetSize(version) + 2;

  public override string ToString()
  {
    return $"[RefIndex={this.m_usRefIndex.ToString()} {base.ToString()}]";
  }

  public override byte[] ToByteArray(ExcelVersion version)
  {
    byte[] byteArray = base.ToByteArray(version);
    int length = byteArray.Length;
    Buffer.BlockCopy((Array) byteArray, 1, (Array) byteArray, 3, length - 3);
    BitConverter.GetBytes(this.m_usRefIndex).CopyTo((Array) byteArray, 1);
    return byteArray;
  }

  public override string ToString(
    FormulaUtil formulaUtil,
    int iRow,
    int iColumn,
    bool bR1C1,
    NumberFormatInfo numberFormat,
    bool isForSerialization)
  {
    if (formulaUtil == null)
      return this.ToString();
    string sheetName = Ref3DPtg.GetSheetName(formulaUtil.ParentWorkbook, (int) this.m_usRefIndex);
    return (sheetName == null ? string.Empty : (!Area3DPtg.ValidateSheetName(sheetName) ? sheetName + "!" : $"'{sheetName}'!")) + base.ToString(formulaUtil, iRow, iColumn, bR1C1, numberFormat, isForSerialization);
  }

  public string BaseToString(FormulaUtil formulaUtil, int iRow, int iColumn, bool bR1C1)
  {
    return this.ToString(formulaUtil, iRow, iColumn, bR1C1);
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
    if ((int) this.m_usRefIndex != (int) (ushort) iSourceSheetIndex)
      return (Ptg) this.Clone();
    Ref3DPtg ref3Dptg = (Ref3DPtg) base.Offset(iDestSheetIndex, iTokenRow, iTokenColumn, iDestSheetIndex, rectSource, iDestSheetIndex, rectDest, out bChanged, book);
    if (bChanged)
      ref3Dptg.m_usRefIndex = (ushort) iDestSheetIndex;
    return (Ptg) ref3Dptg;
  }

  public override Ptg ConvertSharedToken(IWorkbook parent, int iRow, int iColumn)
  {
    int num1 = parent.Version == ExcelVersion.Excel97to2003 ? this.ColumnIndex : this.ColumnIndex;
    int num2 = parent.Version == ExcelVersion.Excel97to2003 ? this.RowIndex : this.RowIndex;
    int iColIndex = this.IsColumnIndexRelative ? iColumn + num1 : num1;
    int iRowIndex = this.IsRowIndexRelative ? iRow + num2 : num2;
    if (parent.Version == ExcelVersion.Excel97to2003)
    {
      iColIndex = (int) (byte) iColIndex;
      iRowIndex = (int) (ushort) iRowIndex;
    }
    Ptg ptg = (Ptg) new Ref3DPtg((int) this.RefIndex, iRowIndex, iColIndex, this.Options);
    ptg.TokenCode = this.TokenCode;
    return ptg;
  }

  public override Ptg ConvertPtgToNPtg(IWorkbook parent, int iRow, int iColumn)
  {
    int num1 = this.IsColumnIndexRelative ? this.ColumnIndex - iColumn : this.ColumnIndex;
    int num2 = this.IsRowIndexRelative ? this.RowIndex - iRow : this.RowIndex;
    RefN3DPtg ptg = (RefN3DPtg) FormulaUtil.CreatePtg(RefN3DPtg.IndexToCode(this.CodeToIndex()));
    ptg.RefIndex = this.RefIndex;
    if (parent.Version == ExcelVersion.Excel97to2003)
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
    switch (ptg.TokenCode)
    {
      case FormulaToken.tRefN3d1:
        ptg.TokenCode = FormulaToken.tRef3d1;
        break;
      case FormulaToken.tRefN3d2:
        ptg.TokenCode = FormulaToken.tRef3d2;
        break;
      case FormulaToken.tRefN3d3:
        ptg.TokenCode = FormulaToken.tRef3d3;
        break;
    }
    ptg.SetOptions(this.Options);
    return (Ptg) ptg;
  }

  internal Ptg ConvertPtgToNPtg()
  {
    RefN3DPtg ptg = (RefN3DPtg) FormulaUtil.CreatePtg(RefN3DPtg.IndexToCode(this.CodeToIndex()));
    ptg.RefIndex = this.RefIndex;
    ptg.RowIndex = this.RowIndex;
    ptg.ColumnIndex = this.ColumnIndex;
    switch (ptg.TokenCode)
    {
      case FormulaToken.tRefN3d1:
        ptg.TokenCode = FormulaToken.tRef3d1;
        break;
      case FormulaToken.tRefN3d2:
        ptg.TokenCode = FormulaToken.tRef3d2;
        break;
      case FormulaToken.tRefN3d3:
        ptg.TokenCode = FormulaToken.tRef3d3;
        break;
    }
    ptg.SetOptions(this.Options);
    return (Ptg) ptg;
  }

  public override int CodeToIndex() => Ref3DPtg.CodeToIndex(this.TokenCode);

  public override FormulaToken GetCorrespondingErrorCode()
  {
    return RefError3dPtg.IndexToCode(this.CodeToIndex());
  }

  public static string GetSheetName(IWorkbook book, int refIndex)
  {
    return ((WorkbookImpl) book)?.GetSheetNameByReference(refIndex, false, true)?.Replace("'", "''");
  }

  protected void SetSheetIndex(string sheetName, IWorkbook parent)
  {
    WorkbookImpl workbookImpl = (WorkbookImpl) parent;
    if (sheetName[0] == '\'')
    {
      if (sheetName[sheetName.Length - 1] == '\'')
        sheetName = sheetName.Substring(1, sheetName.Length - 2);
    }
    try
    {
      this.m_usRefIndex = (ushort) workbookImpl.AddSheetReference(sheetName);
    }
    catch (ArgumentException ex)
    {
      throw new ParseException();
    }
  }

  public new static FormulaToken IndexToCode(int index)
  {
    switch (index)
    {
      case 1:
        return FormulaToken.tRef3d1;
      case 2:
        return FormulaToken.tRef3d2;
      case 3:
        return FormulaToken.tRef3d3;
      default:
        throw new ArgumentOutOfRangeException(nameof (index));
    }
  }

  public new static int CodeToIndex(FormulaToken token)
  {
    switch (token)
    {
      case FormulaToken.tRef3d1:
        return 1;
      case FormulaToken.tRef3d2:
        return 2;
      case FormulaToken.tRef3d3:
        return 3;
      default:
        throw new ArgumentOutOfRangeException("index");
    }
  }

  public new IRange GetRange(IWorkbook book, IWorksheet sheet)
  {
    WorkbookImpl workbookImpl = book != null ? (WorkbookImpl) book : throw new ArgumentNullException(nameof (book));
    IRange range = (IRange) null;
    if (!workbookImpl.IsExternalReference((int) this.m_usRefIndex))
    {
      sheet = workbookImpl.GetSheetByReference((int) this.m_usRefIndex, false);
      if (sheet != null)
        range = sheet[this.RowIndex + 1, this.ColumnIndex + 1];
    }
    else
      range = (IRange) new ExternalRange(workbookImpl.GetExternSheet((int) this.m_usRefIndex), this.RowIndex + 1, this.ColumnIndex + 1);
    return range;
  }

  public override void InfillPTG(DataProvider provider, ref int offset, ExcelVersion version)
  {
    this.m_usRefIndex = provider.ReadUInt16(offset);
    offset += 2;
    base.InfillPTG(provider, ref offset, version);
  }
}
