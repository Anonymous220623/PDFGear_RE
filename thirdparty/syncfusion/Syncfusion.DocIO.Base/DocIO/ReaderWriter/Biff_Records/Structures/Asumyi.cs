// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures.Asumyi
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;

[CLSCompliant(false)]
internal class Asumyi
{
  private byte m_flagsA;
  private ushort m_wDlgLevel = 25;
  private uint m_lHighestLevel;
  private uint m_lCurrentLevel;

  internal bool Valid
  {
    get => ((int) this.m_flagsA & 1) != 0;
    set => this.m_flagsA = (byte) ((int) this.m_flagsA & 254 | (value ? 1 : 0));
  }

  internal bool View
  {
    get => ((int) this.m_flagsA & 2) >> 1 != 0;
    set => this.m_flagsA = (byte) ((int) this.m_flagsA & 253 | (value ? 1 : 0) << 1);
  }

  internal byte ViewBy
  {
    get => (byte) (((int) this.m_flagsA & 12) >> 2);
    set => this.m_flagsA = (byte) ((int) this.m_flagsA & 243 | (int) value << 2);
  }

  internal bool UpdateProps
  {
    get => ((int) this.m_flagsA & 16 /*0x10*/) >> 4 != 0;
    set => this.m_flagsA = (byte) ((int) this.m_flagsA & 239 | (value ? 1 : 0) << 4);
  }

  internal ushort WDlgLevel
  {
    get => this.m_wDlgLevel;
    set => this.m_wDlgLevel = value;
  }

  internal uint LHighestLevel
  {
    get => this.m_lHighestLevel;
    set => this.m_lHighestLevel = value;
  }

  internal uint LCurrentLevel
  {
    get => this.m_lCurrentLevel;
    set => this.m_lCurrentLevel = value;
  }

  internal Asumyi()
  {
  }

  internal void Parse(Stream stream)
  {
    this.m_flagsA = (byte) stream.ReadByte();
    stream.ReadByte();
    this.m_wDlgLevel = BaseWordRecord.ReadUInt16(stream);
    this.m_lHighestLevel = BaseWordRecord.ReadUInt32(stream);
    this.m_lCurrentLevel = BaseWordRecord.ReadUInt32(stream);
  }

  internal void Write(Stream stream)
  {
    stream.WriteByte(this.m_flagsA);
    stream.WriteByte((byte) 0);
    BaseWordRecord.WriteUInt16(stream, this.m_wDlgLevel);
    BaseWordRecord.WriteUInt32(stream, this.m_lHighestLevel);
    BaseWordRecord.WriteUInt32(stream, this.m_lCurrentLevel);
  }
}
