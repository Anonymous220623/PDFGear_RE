// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.ObjRecords.ftMacro
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using System;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.ObjRecords;

public class ftMacro : ObjSubRecord
{
  private Ptg[] m_arrTokens;

  public Ptg[] Tokens
  {
    get => this.m_arrTokens;
    set => this.m_arrTokens = value;
  }

  public ftMacro()
    : base(TObjSubRecordType.ftMacro)
  {
  }

  [CLSCompliant(false)]
  public ftMacro(ushort length, byte[] buffer)
    : base(TObjSubRecordType.ftMacro, length, buffer)
  {
  }

  protected override void Parse(byte[] buffer)
  {
    int startIndex = 0;
    int uint16 = (int) BitConverter.ToUInt16(buffer, startIndex);
    int offset = startIndex + 2 + 4;
    this.m_arrTokens = FormulaUtil.ParseExpression((DataProvider) new ByteArrayDataProvider(buffer), offset, uint16, out int _, ExcelVersion.Excel97to2003);
  }

  protected override void Serialize(DataProvider provider, int iOffset)
  {
    byte[] byteArray = FormulaUtil.PtgArrayToByteArray(this.m_arrTokens, ExcelVersion.Excel97to2003);
    int length = byteArray.Length;
    provider.WriteUInt16(iOffset, (ushort) length);
    iOffset += 2;
    provider.WriteInt32(iOffset, 0);
    iOffset += 4;
    provider.WriteBytes(iOffset, byteArray);
  }

  public override int GetStoreSize(ExcelVersion version)
  {
    int storeSize = DVRecord.GetFormulaSize(this.m_arrTokens, version, true) + 4 + 2 + 4;
    if (storeSize % 2 != 0)
      ++storeSize;
    return storeSize;
  }

  public override object Clone()
  {
    ftMacro ftMacro = (ftMacro) base.Clone();
    ftMacro.m_arrTokens = CloneUtils.ClonePtgArray(this.m_arrTokens);
    return (object) ftMacro;
  }
}
