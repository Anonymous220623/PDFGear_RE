// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Formula.NamePtg
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation;
using Syncfusion.OfficeChart.Implementation.Collections;
using Syncfusion.OfficeChart.Implementation.Exceptions;
using System;
using System.Drawing;
using System.Globalization;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Formula;

[Preserve(AllMembers = true)]
[CLSCompliant(false)]
[Token(FormulaToken.tName1)]
[Token(FormulaToken.tName2)]
[Token(FormulaToken.tName3)]
internal class NamePtg : Ptg, IRangeGetter
{
  private ushort m_usIndex;

  [Preserve]
  public NamePtg()
  {
  }

  [Preserve]
  public NamePtg(DataProvider provider, int offset, OfficeVersion version)
    : base(provider, offset, version)
  {
  }

  [Preserve]
  public NamePtg(string strFormula, IWorkbook parent)
  {
    IName name = parent.Names[strFormula];
    if (name == null)
      throw new ArgumentNullException($"Extern name {strFormula} does not exist");
    this.m_usIndex = (ushort) ((name as NameImpl).Index + 1);
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
    this.m_usIndex = (ushort) ((name as NameImpl).Index + 1);
  }

  [Preserve]
  public NamePtg(int iNameIndex) => this.m_usIndex = (ushort) (iNameIndex + 1);

  public override int GetSize(OfficeVersion version) => 5;

  public ushort ExternNameIndex
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
    if (names.Count <= (int) this.m_usIndex - 1 || this.m_usIndex < (ushort) 1)
      throw new ParseException();
    return names.GetNameByIndex((int) this.m_usIndex - 1).Name;
  }

  public override byte[] ToByteArray(OfficeVersion version)
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
    return book.Names[(int) this.ExternNameIndex - 1] as IRange;
  }

  public Rectangle GetRectangle() => throw new NotSupportedException();

  public override void InfillPTG(DataProvider provider, ref int offset, OfficeVersion version)
  {
    this.m_usIndex = provider.ReadUInt16(offset);
    offset += this.GetSize(version) - 1;
  }
}
