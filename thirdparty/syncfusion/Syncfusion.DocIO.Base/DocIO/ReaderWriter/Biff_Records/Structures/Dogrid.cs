// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures.Dogrid
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;

[CLSCompliant(false)]
internal class Dogrid
{
  private ushort m_xaGrid = 1701;
  private ushort m_yaGrid = 1984;
  private ushort m_dxaGrid = 180;
  private ushort m_dyaGrid = 180;
  private byte m_flagsA = 1;
  private byte m_flagsB = 129;

  internal ushort XaGrid
  {
    get => this.m_xaGrid;
    set => this.m_xaGrid = value;
  }

  internal ushort YaGrid
  {
    get => this.m_yaGrid;
    set => this.m_yaGrid = value;
  }

  internal ushort DxaGrid
  {
    get => this.m_dxaGrid;
    set => this.m_dxaGrid = value;
  }

  internal ushort DyaGrid
  {
    get => this.m_dyaGrid;
    set => this.m_dyaGrid = value;
  }

  internal byte DyGridDisplay
  {
    get => (byte) ((uint) this.m_flagsA & (uint) sbyte.MaxValue);
    set => this.m_flagsA = (byte) ((uint) this.m_flagsA & 128U /*0x80*/ | (uint) value);
  }

  internal byte DxGridDisplay
  {
    get => (byte) ((uint) this.m_flagsB & (uint) sbyte.MaxValue);
    set => this.m_flagsB = (byte) ((uint) this.m_flagsB & 128U /*0x80*/ | (uint) value);
  }

  internal bool FollowMargins
  {
    get => ((int) this.m_flagsB & 128 /*0x80*/) >> 7 != 0;
    set
    {
      this.m_flagsB = (byte) ((int) this.m_flagsB & (int) sbyte.MaxValue | (value ? 1 : 0) << 7);
    }
  }

  internal Dogrid()
  {
  }

  internal void Parse(Stream stream)
  {
    this.m_xaGrid = BaseWordRecord.ReadUInt16(stream);
    this.m_yaGrid = BaseWordRecord.ReadUInt16(stream);
    this.m_dxaGrid = BaseWordRecord.ReadUInt16(stream);
    this.m_dyaGrid = BaseWordRecord.ReadUInt16(stream);
    this.m_flagsA = (byte) stream.ReadByte();
    this.m_flagsB = (byte) stream.ReadByte();
  }

  internal void Write(Stream stream)
  {
    BaseWordRecord.WriteUInt16(stream, this.m_xaGrid);
    BaseWordRecord.WriteUInt16(stream, this.m_yaGrid);
    BaseWordRecord.WriteUInt16(stream, this.m_dxaGrid);
    BaseWordRecord.WriteUInt16(stream, this.m_dyaGrid);
    stream.WriteByte(this.m_flagsA);
    stream.WriteByte(this.m_flagsB);
  }
}
