// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures.DopTypography
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;

[CLSCompliant(false)]
internal class DopTypography
{
  private ushort m_flagsA;
  private ushort m_cchFollowingPunct;
  private ushort m_cchLeadingPunct;
  private byte[] m_rgxchFPunct = new byte[202];
  private byte[] m_rgxchLPunct = new byte[102];

  internal bool KerningPunct
  {
    get => ((int) this.m_flagsA & 1) != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65534 | (value ? 1 : 0));
  }

  internal byte Justification
  {
    get => (byte) (((int) this.m_flagsA & 6) >> 1);
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65529 | (int) value << 1);
  }

  internal byte LevelOfKinsoku
  {
    get => (byte) (((int) this.m_flagsA & 24) >> 3);
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65511 | (int) value << 3);
  }

  internal bool Print2on1
  {
    get => ((int) this.m_flagsA & 32 /*0x20*/) >> 5 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65503 | (value ? 1 : 0) << 5);
  }

  internal byte CustomKsu
  {
    get => (byte) (((int) this.m_flagsA & 896) >> 7);
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 64639 | (int) value << 7);
  }

  internal bool JapaneseUseLevel2
  {
    get => ((int) this.m_flagsA & 1024 /*0x0400*/) >> 10 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 64511 | (value ? 1 : 0) << 10);
  }

  internal ushort CchFollowingPunct
  {
    get => this.m_cchFollowingPunct;
    set => this.m_cchFollowingPunct = value;
  }

  internal ushort CchLeadingPunct
  {
    get => this.m_cchLeadingPunct;
    set => this.m_cchLeadingPunct = value;
  }

  internal byte[] RgxchFPunct
  {
    get => this.m_rgxchFPunct;
    set => this.m_rgxchFPunct = value;
  }

  internal byte[] RgxchLPunct
  {
    get => this.m_rgxchLPunct;
    set => this.m_rgxchLPunct = value;
  }

  internal DopTypography()
  {
  }

  internal void Parse(Stream stream)
  {
    this.m_flagsA = BaseWordRecord.ReadUInt16(stream);
    this.m_cchFollowingPunct = BaseWordRecord.ReadUInt16(stream);
    this.m_cchLeadingPunct = BaseWordRecord.ReadUInt16(stream);
    stream.Read(this.m_rgxchFPunct, 0, this.m_rgxchFPunct.Length);
    stream.Read(this.m_rgxchLPunct, 0, this.m_rgxchLPunct.Length);
  }

  internal void Write(Stream stream)
  {
    BaseWordRecord.WriteUInt16(stream, this.m_flagsA);
    BaseWordRecord.WriteUInt16(stream, this.m_cchFollowingPunct);
    BaseWordRecord.WriteUInt16(stream, this.m_cchLeadingPunct);
    stream.Write(this.m_rgxchFPunct, 0, this.m_rgxchFPunct.Length);
    stream.Write(this.m_rgxchLPunct, 0, this.m_rgxchLPunct.Length);
  }
}
