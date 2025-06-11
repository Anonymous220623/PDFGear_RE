// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Formula.ControlPtg
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation;
using System;
using System.Drawing;
using System.Globalization;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Formula;

[CLSCompliant(false)]
[Preserve(AllMembers = true)]
[Token(FormulaToken.tTbl)]
[Token(FormulaToken.tExp)]
internal class ControlPtg : RefPtg
{
  [Preserve]
  public ControlPtg()
  {
  }

  [Preserve]
  public ControlPtg(DataProvider provider, int offset, OfficeVersion version)
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

  public override int GetSize(OfficeVersion version)
  {
    switch (version)
    {
      case OfficeVersion.Excel97to2003:
        return 5;
      case OfficeVersion.Excel2007:
      case OfficeVersion.Excel2010:
      case OfficeVersion.Excel2013:
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

  public override byte[] ToByteArray(OfficeVersion version)
  {
    byte[] byteArray = new byte[this.GetSize(version)];
    byteArray[0] = (byte) this.TokenCode;
    switch (version)
    {
      case OfficeVersion.Excel97to2003:
        BitConverter.GetBytes((ushort) this.RowIndex).CopyTo((Array) byteArray, 1);
        BitConverter.GetBytes((ushort) this.ColumnIndex).CopyTo((Array) byteArray, 3);
        break;
      case OfficeVersion.Excel2007:
      case OfficeVersion.Excel2010:
      case OfficeVersion.Excel2013:
        int index1 = 1;
        BitConverter.GetBytes(this.RowIndex).CopyTo((Array) byteArray, index1);
        int index2 = index1 + 4;
        BitConverter.GetBytes(this.ColumnIndex).CopyTo((Array) byteArray, index2);
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

  public override void InfillPTG(DataProvider provider, ref int offset, OfficeVersion version)
  {
    if (version == OfficeVersion.Excel97to2003)
    {
      this.RowIndex = (int) provider.ReadUInt16(offset);
      offset += 2;
      this.ColumnIndex = (int) (byte) provider.ReadUInt16(offset);
      offset += 2;
    }
    else
    {
      if (version == OfficeVersion.Excel97to2003)
        return;
      this.RowIndex = provider.ReadInt32(offset);
      offset += 4;
      this.ColumnIndex = provider.ReadInt32(offset);
      offset += 4;
    }
  }
}
