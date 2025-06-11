// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.ObjRecords.ftCmo
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.ObjRecords;

[CLSCompliant(false)]
internal class ftCmo : ObjSubRecord
{
  private const int DEF_CHANGE_COLOR_MASK = 256 /*0x0100*/;
  private TObjType m_ObjType;
  private ushort m_usId;
  private ushort m_usOptions;
  private byte[] m_reserved = new byte[12];
  private bool m_bBadLength;

  public bool Locked
  {
    get => ((int) this.m_usOptions & 1) > 0;
    set
    {
      if (value)
        this.m_usOptions |= (ushort) 1;
      else
        this.m_usOptions &= (ushort) 65534;
    }
  }

  public bool Printable
  {
    get => ((int) this.m_usOptions & 16 /*0x10*/) > 0;
    set
    {
      if (value)
        this.m_usOptions |= (ushort) 16 /*0x10*/;
      else
        this.m_usOptions &= (ushort) 65519;
    }
  }

  public bool AutoFill
  {
    get => ((int) this.m_usOptions & 8192 /*0x2000*/) > 0;
    set
    {
      if (value)
        this.m_usOptions |= (ushort) 8192 /*0x2000*/;
      else
        this.m_usOptions &= (ushort) 57343 /*0xDFFF*/;
    }
  }

  public bool AutoLine
  {
    get => ((int) this.m_usOptions & 16384 /*0x4000*/) > 0;
    set
    {
      if (value)
        this.m_usOptions |= (ushort) 16384 /*0x4000*/;
      else
        this.m_usOptions &= (ushort) 49151 /*0xBFFF*/;
    }
  }

  public bool ChangeColor
  {
    get => ((int) this.m_usOptions & 256 /*0x0100*/) > 0;
    set
    {
      if (value)
        this.m_usOptions |= (ushort) 256 /*0x0100*/;
      else
        this.m_usOptions &= (ushort) 65279;
    }
  }

  public ushort ID
  {
    get => this.m_usId;
    set => this.m_usId = value;
  }

  public TObjType ObjectType
  {
    get => this.m_ObjType;
    set => this.m_ObjType = value;
  }

  public byte[] Reserved
  {
    get => this.m_reserved;
    internal set => this.m_reserved = value;
  }

  public ushort Options
  {
    get => this.m_usOptions;
    internal set => this.m_usOptions = value;
  }

  public ftCmo()
    : base(TObjSubRecordType.ftCmo)
  {
  }

  public ftCmo(TObjSubRecordType type, ushort length, byte[] buffer)
    : base(type, length, buffer)
  {
  }

  protected override void Parse(byte[] buffer)
  {
    if (buffer == null)
      throw new ArgumentNullException(nameof (buffer));
    if (buffer.Length == 0)
    {
      this.m_bBadLength = true;
    }
    else
    {
      this.m_ObjType = (TObjType) BitConverter.ToInt16(buffer, 0);
      this.m_usId = BitConverter.ToUInt16(buffer, 2);
      this.m_usOptions = BitConverter.ToUInt16(buffer, 4);
      int index1 = 0;
      int index2 = 6;
      while (index2 < 18)
      {
        this.m_reserved[index1] = buffer[index2];
        ++index2;
        ++index1;
      }
    }
  }

  public override void FillArray(DataProvider provider, int iOffset)
  {
    if (!this.m_bBadLength)
    {
      provider.WriteInt16(iOffset, (short) this.Type);
      iOffset += 2;
      provider.WriteInt16(iOffset, (short) 18);
      iOffset += 2;
      provider.WriteInt16(iOffset, (short) this.m_ObjType);
      iOffset += 2;
      provider.WriteUInt16(iOffset, this.m_usId);
      iOffset += 2;
      provider.WriteUInt16(iOffset, this.m_usOptions);
      iOffset += 2;
      provider.WriteBytes(iOffset, this.m_reserved, 0, this.m_reserved.Length);
    }
    else
    {
      provider.WriteInt16(iOffset, (short) this.Type);
      iOffset += 2;
      provider.WriteInt16(iOffset, (short) 0);
    }
  }

  public override object Clone()
  {
    ftCmo ftCmo = (ftCmo) base.Clone();
    ftCmo.m_reserved = CloneUtils.CloneByteArray(this.m_reserved);
    return (object) ftCmo;
  }

  public override int GetStoreSize(OfficeVersion version) => this.m_bBadLength ? 4 : 22;
}
