// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.ObjRecords.ftSbsFormula
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using System;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.ObjRecords;

internal class ftSbsFormula : ObjSubRecord, IFormulaRecord
{
  private const int RowIndexOffset = 7;
  private const int ColumnIndexOffset = 9;
  private Ptg[] m_formula;

  public ftSbsFormula()
    : base(TObjSubRecordType.ftSbsFormula)
  {
  }

  public ftSbsFormula(TObjSubRecordType type, ushort length, byte[] buffer)
    : base(type, length, buffer)
  {
  }

  public Ptg[] Formula
  {
    get => this.m_formula;
    set => this.m_formula = value;
  }

  protected override void Parse(byte[] buffer)
  {
    int startIndex1 = 0;
    int int16 = (int) BitConverter.ToInt16(buffer, startIndex1);
    int startIndex2 = startIndex1 + 2;
    BitConverter.ToInt32(buffer, startIndex2);
    int offset = startIndex2 + 4;
    this.m_formula = FormulaUtil.ParseExpression((DataProvider) new ByteArrayDataProvider(buffer), offset, int16, out int _, ExcelVersion.Excel97to2003);
  }

  protected override void Serialize(DataProvider provider, int iOffset)
  {
    byte[] byteArray = FormulaUtil.PtgArrayToByteArray(this.m_formula, ExcelVersion.Excel97to2003);
    int length = byteArray.Length;
    provider.WriteInt16(iOffset, (short) length);
    iOffset += 2;
    provider.WriteInt32(iOffset, 0);
    iOffset += 4;
    provider.WriteBytes(iOffset, byteArray);
    iOffset += length;
    provider.WriteByte(iOffset, (byte) 0);
  }

  public override int GetStoreSize(ExcelVersion version)
  {
    return DVRecord.GetFormulaSize(this.m_formula, version, true) + 4 + 2 + 4 + 1;
  }
}
