// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.ObjRecords.ftPictFmla
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.ObjRecords;

[CLSCompliant(false)]
internal class ftPictFmla : ObjSubRecord
{
  private const int FormulaStart = 14;
  private static readonly byte[] DefaultHeader = new byte[14]
  {
    (byte) 30,
    (byte) 0,
    (byte) 5,
    (byte) 0,
    (byte) 12,
    (byte) 151,
    (byte) 65,
    (byte) 7,
    (byte) 2,
    (byte) 8,
    (byte) 8,
    (byte) 232,
    (byte) 7,
    (byte) 3
  };
  private static readonly byte[] DefaultFooter = new byte[16 /*0x10*/]
  {
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 68,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0
  };
  private byte[] m_arrHeader;
  private byte[] m_arrFooter;
  private string m_strFormula;

  public string Formula
  {
    get => this.m_strFormula;
    set => this.m_strFormula = value;
  }

  public ftPictFmla()
    : base(TObjSubRecordType.ftPictFmla)
  {
    int length1 = ftPictFmla.DefaultHeader.Length;
    this.m_arrHeader = new byte[length1];
    Buffer.BlockCopy((Array) ftPictFmla.DefaultHeader, 0, (Array) this.m_arrHeader, 0, length1);
    int length2 = ftPictFmla.DefaultFooter.Length;
    this.m_arrFooter = new byte[length2];
    Buffer.BlockCopy((Array) ftPictFmla.DefaultFooter, 0, (Array) this.m_arrFooter, 0, length2);
  }

  public ftPictFmla(TObjSubRecordType type, ushort length, byte[] buffer)
    : base(type, length, buffer)
  {
  }

  protected override void Parse(byte[] buffer)
  {
    this.m_arrHeader = new byte[14];
    Buffer.BlockCopy((Array) buffer, 0, (Array) this.m_arrHeader, 0, 14);
    int offset = 14;
    this.m_strFormula = BiffRecordRaw.GetString16BitUpdateOffset(buffer, ref offset);
    int count = buffer.Length - offset;
    this.m_arrFooter = new byte[count];
    Buffer.BlockCopy((Array) buffer, offset, (Array) this.m_arrFooter, 0, count);
  }

  public override void FillArray(DataProvider provider, int iOffset)
  {
    provider.WriteInt16(iOffset, (short) this.Type);
    iOffset += 2;
    int num = this.GetStoreSize(OfficeVersion.Excel97to2003) - 4;
    provider.WriteInt16(iOffset, (short) num);
    iOffset += 2;
    int length1 = this.m_arrHeader.Length;
    provider.WriteBytes(iOffset, this.m_arrHeader, 0, length1);
    iOffset += length1;
    iOffset += provider.WriteString16Bit(iOffset, this.m_strFormula, false);
    int length2 = this.m_arrFooter.Length;
    provider.WriteBytes(iOffset, this.m_arrFooter, 0, length2);
    iOffset += length2;
  }

  public override object Clone()
  {
    ftPictFmla ftPictFmla = (ftPictFmla) base.Clone();
    ftPictFmla.m_arrHeader = CloneUtils.CloneByteArray(this.m_arrHeader);
    ftPictFmla.m_arrFooter = CloneUtils.CloneByteArray(this.m_arrFooter);
    return (object) ftPictFmla;
  }

  public override int GetStoreSize(OfficeVersion version)
  {
    return 4 + this.m_arrFooter.Length + this.m_arrHeader.Length + 3 + this.m_strFormula.Length;
  }
}
