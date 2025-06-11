// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Formula.ControlPtg
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System;
using System.Drawing;
using System.Globalization;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Formula;

[Token(FormulaToken.tExp)]
[CLSCompliant(false)]
[Preserve(AllMembers = true)]
[Token(FormulaToken.tTbl)]
public class ControlPtg : RefPtg
{
  [Preserve]
  public ControlPtg()
  {
  }

  [Preserve]
  public ControlPtg(DataProvider provider, int offset, ExcelVersion version)
    : base(provider, offset, version)
  {
  }

  [Preserve]
  public ControlPtg(int iRow, int iColumn)
  {
    this.RowIndex = iRow;
    this.ColumnIndex = iColumn;
  }

  public override bool IsColumnIndexRelative
  {
    get => true;
    set => throw new NotSupportedException();
  }

  public override bool IsRowIndexRelative
  {
    get => true;
    set => throw new NotSupportedException();
  }

  public override int GetSize(ExcelVersion version)
  {
    switch (version)
    {
      case ExcelVersion.Excel97to2003:
        return 5;
      case ExcelVersion.Excel2007:
      case ExcelVersion.Excel2010:
      case ExcelVersion.Excel2013:
      case ExcelVersion.Excel2016:
      case ExcelVersion.Xlsx:
        return 9;
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
    return $"( ControlToken {RangeImpl.GetCellName(this.ColumnIndex + 1, this.RowIndex + 1)})";
  }

  public override byte[] ToByteArray(ExcelVersion version)
  {
    byte[] byteArray = new byte[this.GetSize(version)];
    byteArray[0] = (byte) this.TokenCode;
    switch (version)
    {
      case ExcelVersion.Excel97to2003:
        BitConverter.GetBytes((ushort) this.RowIndex).CopyTo((Array) byteArray, 1);
        BitConverter.GetBytes((ushort) this.ColumnIndex).CopyTo((Array) byteArray, 3);
        break;
      case ExcelVersion.Excel2007:
      case ExcelVersion.Excel2010:
      case ExcelVersion.Excel2013:
      case ExcelVersion.Excel2016:
      case ExcelVersion.Xlsx:
        int index1 = 1;
        BitConverter.GetBytes(this.RowIndex).CopyTo((Array) byteArray, index1);
        int index2 = index1 + 4;
        BitConverter.GetBytes((short) this.ColumnIndex).CopyTo((Array) byteArray, index2);
        int num = index2 + 2;
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof (version));
    }
    return byteArray;
  }

  public override FormulaToken GetCorrespondingErrorCode() => FormulaToken.tRef2;

  protected override Ptg MoveIntoDifferentSheet(
    RefPtg result,
    int iSourceSheetIndex,
    Rectangle rectSource,
    int iDestSheetIndex,
    int iRowOffset,
    int iColOffset,
    WorkbookImpl book)
  {
    return this.Offset(iRowOffset, iColOffset, book);
  }

  public override void InfillPTG(DataProvider provider, ref int offset, ExcelVersion version)
  {
    if (version == ExcelVersion.Excel97to2003)
    {
      this.RowIndex = (int) provider.ReadUInt16(offset);
      offset += 2;
      this.ColumnIndex = (int) (byte) provider.ReadUInt16(offset);
      offset += 2;
    }
    else
    {
      if (version == ExcelVersion.Excel97to2003)
        return;
      this.RowIndex = provider.ReadInt32(offset);
      offset += 4;
      this.ColumnIndex = provider.ReadInt32(offset);
      offset += 4;
    }
  }
}
