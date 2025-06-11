// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.ObjRecords.ftMacro
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation;
using Syncfusion.OfficeChart.Parser.Biff_Records.Formula;
using System;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.ObjRecords;

internal class ftMacro : ObjSubRecord
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
    this.m_arrTokens = FormulaUtil.ParseExpression((DataProvider) new ByteArrayDataProvider(buffer), offset, uint16, out int _, OfficeVersion.Excel97to2003);
  }

  protected override void Serialize(DataProvider provider, int iOffset)
  {
    byte[] byteArray = FormulaUtil.PtgArrayToByteArray(this.m_arrTokens, OfficeVersion.Excel97to2003);
    int length = byteArray.Length;
    provider.WriteUInt16(iOffset, (ushort) length);
    iOffset += 2;
    provider.WriteInt32(iOffset, 0);
    iOffset += 4;
    provider.WriteBytes(iOffset, byteArray);
  }

  public override int GetStoreSize(OfficeVersion version)
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
