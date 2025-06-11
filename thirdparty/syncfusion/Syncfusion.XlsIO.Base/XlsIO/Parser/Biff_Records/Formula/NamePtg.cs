// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Formula.NamePtg
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using Syncfusion.XlsIO.Implementation.Collections;
using Syncfusion.XlsIO.Implementation.Exceptions;
using System;
using System.Drawing;
using System.Globalization;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Formula;

[CLSCompliant(false)]
[Token(FormulaToken.tName1)]
[Token(FormulaToken.tName2)]
[Token(FormulaToken.tName3)]
[Preserve(AllMembers = true)]
public class NamePtg : Ptg, IRangeGetter
{
  private int m_usIndex;

  [Preserve]
  public NamePtg()
  {
  }

  [Preserve]
  public NamePtg(DataProvider provider, int offset, ExcelVersion version)
    : base(provider, offset, version)
  {
  }

  [Preserve]
  public NamePtg(string strFormula, IWorkbook parent)
  {
    IName name = parent.Names[strFormula];
    if (name == null)
      throw new ArgumentNullException($"Extern name {strFormula} does not exist");
    this.m_usIndex = (name as NameImpl).Index + 1;
  }

  [Preserve]
  public NamePtg(string strFormula, IWorkbook book, IWorksheet sheet)
  {
    IName name;
    if (sheet.Names.Contains(strFormula))
    {
      name = sheet.Names[strFormula];
    }
    else
    {
      if (!book.Names.Contains(strFormula))
        throw new ArgumentException("Unknown name", strFormula);
      name = book.Names[strFormula];
    }
    this.m_usIndex = (name as NameImpl).Index + 1;
  }

  [Preserve]
  public NamePtg(int iNameIndex) => this.m_usIndex = iNameIndex + 1;

  public override int GetSize(ExcelVersion version) => 5;

  public ushort ExternNameIndex
  {
    get => (ushort) this.m_usIndex;
    set => this.m_usIndex = (int) value;
  }

  internal int ExternNameIndexInt
  {
    get => this.m_usIndex;
    set => this.m_usIndex = value;
  }

  public override string ToString() => $"( NameIndex = {this.m_usIndex.ToString()} )";

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
    WorkbookNamesCollection names = formulaUtil.ParentWorkbook.Names as WorkbookNamesCollection;
    if (names.Count <= this.m_usIndex - 1 || this.m_usIndex < 1)
      throw new ParseException();
    IName nameByIndex = names.GetNameByIndex(this.m_usIndex - 1);
    string name = nameByIndex.Name;
    if (isForSerialization && (nameByIndex as NameImpl).m_isTableNamedRange && !nameByIndex.Name.EndsWith("]"))
      name += "[]";
    return name;
  }

  public override byte[] ToByteArray(ExcelVersion version)
  {
    byte[] byteArray = base.ToByteArray(version);
    BitConverter.GetBytes(this.m_usIndex).CopyTo((Array) byteArray, 1);
    return byteArray;
  }

  public static FormulaToken IndexToCode(int index)
  {
    switch (index)
    {
      case 1:
        return FormulaToken.tName1;
      case 2:
        return FormulaToken.tName2;
      case 3:
        return FormulaToken.tName3;
      default:
        throw new ArgumentOutOfRangeException(nameof (index));
    }
  }

  public IRange GetRange(IWorkbook book, IWorksheet sheet)
  {
    if (book == null)
      throw new ArgumentNullException(nameof (book));
    return (book.Names as WorkbookNamesCollection)[this.ExternNameIndexInt - 1] as IRange;
  }

  public Rectangle GetRectangle() => throw new NotSupportedException();

  public override void InfillPTG(DataProvider provider, ref int offset, ExcelVersion version)
  {
    this.m_usIndex = provider.ReadInt32(offset);
    offset += this.GetSize(version) - 1;
  }
}
