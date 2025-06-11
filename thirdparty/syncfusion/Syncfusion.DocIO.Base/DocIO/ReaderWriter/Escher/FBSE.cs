// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Escher.FBSE
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Escher;

internal class FBSE : BaseWordRecord
{
  private MSOBlipType m_btWin32;
  private MSOBlipType m_btMacOS;
  private byte[] m_rgbUid;
  private ushort m_tag;
  private uint m_size;
  private uint m_cRef;
  private uint m_foDelay;
  private MSOBlipUsage m_usage;
  private byte m_cbName;
  private byte m_unused2;
  private byte m_unused3;

  public FBSE() => this.m_rgbUid = new byte[16 /*0x10*/];

  public MSOBlipType Win32
  {
    get => this.m_btWin32;
    set => this.m_btWin32 = value;
  }

  public MSOBlipType MacOS
  {
    get => this.m_btMacOS;
    set => this.m_btMacOS = value;
  }

  public byte[] Uid
  {
    get => this.m_rgbUid;
    set => this.m_rgbUid = value;
  }

  public ushort Tag
  {
    get => this.m_tag;
    set => this.m_tag = value;
  }

  public uint Size
  {
    get => this.m_size;
    set => this.m_size = value;
  }

  public uint Ref
  {
    get => this.m_cRef;
    set => this.m_cRef = value;
  }

  public uint Delay
  {
    get => this.m_foDelay;
    set => this.m_foDelay = value;
  }

  public MSOBlipUsage Usage
  {
    get => this.m_usage;
    set => this.m_usage = value;
  }

  public byte Name
  {
    get => this.m_cbName;
    set => this.m_cbName = value;
  }

  public byte Unused2
  {
    set => this.m_unused2 = value;
  }

  public byte Unused3
  {
    set => this.m_unused3 = value;
  }

  public void Read(Stream stream)
  {
    this.m_btWin32 = (MSOBlipType) stream.ReadByte();
    this.m_btMacOS = (MSOBlipType) stream.ReadByte();
    this.m_rgbUid = this.ReadBytes(stream, 16 /*0x10*/);
    this.m_tag = BaseWordRecord.ReadUInt16(stream);
    this.m_size = BaseWordRecord.ReadUInt32(stream);
    this.m_cRef = BaseWordRecord.ReadUInt32(stream);
    this.m_foDelay = BaseWordRecord.ReadUInt32(stream);
    this.m_usage = (MSOBlipUsage) stream.ReadByte();
    this.m_cbName = (byte) stream.ReadByte();
    this.m_unused2 = (byte) stream.ReadByte();
    this.m_unused3 = (byte) stream.ReadByte();
  }

  public void Write(Stream stream)
  {
    stream.WriteByte((byte) this.m_btWin32);
    stream.WriteByte((byte) this.m_btMacOS);
    for (int index = 0; index < 16 /*0x10*/; ++index)
      stream.WriteByte(this.m_rgbUid[index]);
    BaseWordRecord.WriteUInt16(stream, this.m_tag);
    BaseWordRecord.WriteUInt32(stream, this.m_size);
    BaseWordRecord.WriteUInt32(stream, this.m_cRef);
    BaseWordRecord.WriteUInt32(stream, this.m_foDelay);
    stream.WriteByte((byte) this.m_usage);
    stream.WriteByte(this.m_cbName);
    stream.WriteByte(this.m_unused2);
    stream.WriteByte(this.m_unused3);
  }
}
