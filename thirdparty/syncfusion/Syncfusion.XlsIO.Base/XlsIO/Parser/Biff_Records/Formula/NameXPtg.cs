// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Formula.NameXPtg
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using Syncfusion.XlsIO.Implementation.Exceptions;
using System;
using System.Drawing;
using System.Globalization;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Formula;

[Preserve(AllMembers = true)]
[Token(FormulaToken.tNameX1)]
[Token(FormulaToken.tNameX2)]
[Token(FormulaToken.tNameX3)]
[CLSCompliant(false)]
public class NameXPtg : Ptg, ISheetReference, IReference, IRangeGetter
{
  private ushort m_usRefIndex;
  private ushort m_usNameIndex;

  [Preserve]
  public NameXPtg()
  {
  }

  [Preserve]
  public NameXPtg(DataProvider provider, int offset, ExcelVersion version)
    : base(provider, offset, version)
  {
  }

  [Preserve]
  public NameXPtg(string strFormula, IWorkbook parent)
  {
    IName name = parent.Names[strFormula];
    if (name == null)
      throw new ArgumentNullException($"Extern name {strFormula} does not exist");
    this.m_usNameIndex = (ushort) ((name as NameImpl).Index + 1);
    Ptg formulaToken = ((NameImpl) name).Record.FormulaTokens[0];
    switch (formulaToken)
    {
      case Area3DPtg _:
        this.m_usRefIndex = ((Area3DPtg) formulaToken).RefIndex;
        break;
      case Ref3DPtg _:
        this.m_usRefIndex = ((Ref3DPtg) formulaToken).RefIndex;
        break;
    }
  }

  [Preserve]
  public NameXPtg(int iBookIndex, int iNameIndex)
  {
    this.m_usRefIndex = (ushort) iBookIndex;
    this.m_usNameIndex = (ushort) (iNameIndex + 1);
  }

  public ushort NameIndex
  {
    get => this.m_usNameIndex;
    set => this.m_usNameIndex = value;
  }

  public ushort RefIndex
  {
    get => this.m_usRefIndex;
    set => this.m_usRefIndex = value;
  }

  public override int GetSize(ExcelVersion version) => 7;

  public override string ToString(
    FormulaUtil formulaUtil,
    int iRow,
    int iColumn,
    bool bR1C1,
    NumberFormatInfo numberFormat,
    bool isForSerialization)
  {
    return this.ToString(formulaUtil, iRow, iColumn, bR1C1, numberFormat, isForSerialization, (IWorksheet) null);
  }

  public override string ToString(
    FormulaUtil formulaUtil,
    int row,
    int col,
    bool bR1C1,
    NumberFormatInfo numberInfo,
    bool isForSerialization,
    IWorksheet sheet)
  {
    if (formulaUtil == null)
      return $"( ExternNameIndex = {this.m_usNameIndex}, RefIndex = {this.m_usRefIndex} )";
    WorkbookImpl parentWorkbook = (WorkbookImpl) formulaUtil.ParentWorkbook;
    string sheetNameByReference1 = (formulaUtil.ParentWorkbook as WorkbookImpl).GetSheetNameByReference((int) this.m_usRefIndex, false);
    if (parentWorkbook.IsLocalReference((int) this.m_usRefIndex))
    {
      if (parentWorkbook.InnerNamesColection.Count <= (int) this.m_usNameIndex - 1 || this.m_usNameIndex < (ushort) 1)
        throw new ParseException();
      IName nameByIndex = parentWorkbook.InnerNamesColection.GetNameByIndex((int) this.m_usNameIndex - 1);
      string workbookName;
      if (parentWorkbook.FullFileName != null)
      {
        string[] strArray1 = parentWorkbook.FullFileName.Split('/');
        string[] strArray2 = strArray1[strArray1.Length - 1].Split('\\');
        workbookName = strArray2[strArray2.Length - 1];
      }
      else
        workbookName = parentWorkbook.GetWorkbookName(parentWorkbook);
      if ((nameByIndex as NameImpl).IsDeleted && sheetNameByReference1 == "#REF")
        return $"'{workbookName}'!{nameByIndex.Name}";
      IWorksheet worksheet = nameByIndex.Worksheet;
      if (worksheet == null && parentWorkbook.ExternSheet.Refs[(int) this.m_usRefIndex].FirstSheet != (ushort) 65534)
        return nameByIndex.Name;
      if (worksheet == null && parentWorkbook.ExternSheet.Refs[(int) this.m_usRefIndex].FirstSheet == (ushort) 65534 && parentWorkbook.Saving)
        return $"'{"[0]"}'!{nameByIndex.Name}";
      return worksheet == null && parentWorkbook.ExternSheet.Refs[(int) this.m_usRefIndex].FirstSheet == (ushort) 65534 ? $"'{workbookName}'!{nameByIndex.Name}" : $"'{worksheet.Name}'!{nameByIndex.Name}";
    }
    int bookIndex = parentWorkbook.GetBookIndex((int) this.m_usRefIndex);
    int num = 0;
    for (int index = bookIndex - 1; index >= 0; --index)
    {
      ExternWorkbookImpl externWorkbook = parentWorkbook.ExternWorkbooks[index];
      if (externWorkbook.IsInternalReference || string.IsNullOrEmpty(externWorkbook.URL))
        ++num;
    }
    ExternWorkbookImpl externWorkbook1 = parentWorkbook.ExternWorkbooks[bookIndex];
    ExternNameImpl externName = externWorkbook1.ExternNames[(int) this.m_usNameIndex - 1];
    string sheetNameByReference2 = (formulaUtil.ParentWorkbook as WorkbookImpl).GetSheetNameByReference((int) this.m_usRefIndex, false, true);
    if (parentWorkbook.Version == ExcelVersion.Excel97to2003 || externWorkbook1.URL == null || !isForSerialization)
    {
      if (Area3DPtg.ValidateSheetName(externName.Name))
        return $"'{externWorkbook1.URL}'!{externName.Name}";
      if (externWorkbook1.URL == null)
        return externName.Name;
      return sheetNameByReference2 != null ? $"'{sheetNameByReference2}'!{externName.Name}" : $"'{externWorkbook1.URL}'!{externName.Name}";
    }
    if (externWorkbook1.IsAddInFunctions && externWorkbook1.Worksheets.Count == 0)
      return $"'{externWorkbook1.URL}'!'{externName.Name}'";
    return sheetNameByReference2 != null ? $"{sheetNameByReference2}!{externName.Name}" : $"{$"[{(object) (bookIndex - num + 1)}]"}!{externName.Name}";
  }

  public string BaseToString(FormulaUtil formulaUtil, int iRow, int iColumn, bool bR1C1)
  {
    return this.ToString(formulaUtil, iRow, iColumn, bR1C1);
  }

  public override byte[] ToByteArray(ExcelVersion version)
  {
    byte[] byteArray = base.ToByteArray(version);
    BitConverter.GetBytes(this.m_usRefIndex).CopyTo((Array) byteArray, 1);
    BitConverter.GetBytes(this.m_usNameIndex).CopyTo((Array) byteArray, 3);
    return byteArray;
  }

  public static FormulaToken IndexToCode(int index)
  {
    switch (index)
    {
      case 1:
        return FormulaToken.tNameX1;
      case 2:
        return FormulaToken.tNameX2;
      case 3:
        return FormulaToken.tNameX3;
      default:
        throw new ArgumentOutOfRangeException(nameof (index));
    }
  }

  public IRange GetRange(IWorkbook book, IWorksheet sheet)
  {
    WorkbookImpl workbookImpl = book != null ? (WorkbookImpl) book : throw new ArgumentNullException(nameof (book));
    workbookImpl.CheckForInternalReference((int) this.RefIndex);
    return (IRange) workbookImpl.InnerNamesColection[(int) this.NameIndex - 1];
  }

  public Rectangle GetRectangle() => throw new NotSupportedException();

  public Ptg UpdateRectangle(Rectangle rectangle) => throw new NotSupportedException();

  public override void InfillPTG(DataProvider provider, ref int offset, ExcelVersion version)
  {
    this.m_usRefIndex = provider.ReadUInt16(offset);
    offset += 2;
    this.m_usNameIndex = provider.ReadUInt16(offset);
    offset += this.GetSize(version) - 3;
  }
}
