// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Formula.NameXPtg
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation;
using Syncfusion.OfficeChart.Implementation.Exceptions;
using System;
using System.Drawing;
using System.Globalization;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Formula;

[CLSCompliant(false)]
[Preserve(AllMembers = true)]
[Token(FormulaToken.tNameX1)]
[Token(FormulaToken.tNameX2)]
[Token(FormulaToken.tNameX3)]
internal class NameXPtg : Ptg, ISheetReference, IReference, IRangeGetter
{
  private ushort m_usRefIndex;
  private ushort m_usNameIndex;

  [Preserve]
  public NameXPtg()
  {
  }

  [Preserve]
  public NameXPtg(DataProvider provider, int offset, OfficeVersion version)
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

  public override int GetSize(OfficeVersion version) => 7;

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
    if (parentWorkbook.IsLocalReference((int) this.m_usRefIndex))
    {
      if ((formulaUtil.ParentWorkbook as WorkbookImpl).InnerNamesColection.Count <= (int) this.m_usNameIndex - 1 || this.m_usNameIndex < (ushort) 1)
        throw new ParseException();
      IName name = (formulaUtil.ParentWorkbook as WorkbookImpl).InnerNamesColection[(int) this.m_usNameIndex - 1];
      IWorksheet worksheet = name.Worksheet;
      return worksheet != sheet && worksheet != null ? $"'{worksheet.Name}'!{name.Name}" : name.Name;
    }
    int bookIndex = parentWorkbook.GetBookIndex((int) this.m_usRefIndex);
    ExternWorkbookImpl externWorkbook = parentWorkbook.ExternWorkbooks[bookIndex];
    ExternNameImpl externName = externWorkbook.ExternNames[(int) this.m_usNameIndex - 1];
    if (parentWorkbook.Version == OfficeVersion.Excel97to2003 || externWorkbook.URL == null || !isForSerialization)
    {
      if (Area3DPtg.ValidateSheetName(externName.Name))
        return $"'{externWorkbook.URL}'!{externName.Name}";
      return externWorkbook.URL == null ? externName.Name : $"'{externWorkbook.URL}'!{externName.Name}";
    }
    return externWorkbook.IsAddInFunctions && externWorkbook.Worksheets.Count == 0 ? $"'{externWorkbook.URL}'!'{externName.Name}'" : $"{$"[{(object) (bookIndex + 1)}]"}!{externName.Name}";
  }

  public string BaseToString(FormulaUtil formulaUtil, int iRow, int iColumn, bool bR1C1)
  {
    return this.ToString(formulaUtil, iRow, iColumn, bR1C1);
  }

  public override byte[] ToByteArray(OfficeVersion version)
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
    return (IRange) workbookImpl.Names[(int) this.NameIndex - 1];
  }

  public Rectangle GetRectangle() => throw new NotSupportedException();

  public Ptg UpdateRectangle(Rectangle rectangle) => throw new NotSupportedException();

  public override void InfillPTG(DataProvider provider, ref int offset, OfficeVersion version)
  {
    this.m_usRefIndex = provider.ReadUInt16(offset);
    offset += 2;
    this.m_usNameIndex = provider.ReadUInt16(offset);
    offset += this.GetSize(version) - 3;
  }
}
