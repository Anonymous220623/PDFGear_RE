// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.ObjRecords.ObjSubRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.ObjRecords;

internal abstract class ObjSubRecord : ICloneable
{
  protected const int HeaderSize = 4;
  private TObjSubRecordType m_Type;
  private ushort m_usLength;

  public TObjSubRecordType Type => this.m_Type;

  [CLSCompliant(false)]
  public ushort Length
  {
    get => this.m_usLength;
    protected set => this.m_usLength = value;
  }

  private ObjSubRecord()
  {
  }

  protected ObjSubRecord(TObjSubRecordType type) => this.m_Type = type;

  [CLSCompliant(false)]
  protected ObjSubRecord(TObjSubRecordType type, ushort length, byte[] buffer)
  {
    this.m_Type = type;
    this.m_usLength = length;
    this.Parse(buffer);
  }

  protected abstract void Parse(byte[] buffer);

  public virtual void FillArray(DataProvider provider, int iOffset)
  {
    provider.WriteInt16(iOffset, (short) this.Type);
    iOffset += 2;
    ushort num = (ushort) (this.GetStoreSize(OfficeVersion.Excel97to2003) - 4);
    provider.WriteUInt16(iOffset, num);
    iOffset += 2;
    this.Serialize(provider, iOffset);
  }

  protected virtual void Serialize(DataProvider provider, int iOffset)
  {
  }

  public abstract int GetStoreSize(OfficeVersion version);

  public virtual object Clone() => this.MemberwiseClone();
}
