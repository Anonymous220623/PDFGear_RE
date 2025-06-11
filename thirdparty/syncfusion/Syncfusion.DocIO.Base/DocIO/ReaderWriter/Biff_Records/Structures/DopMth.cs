// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures.DopMth
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;

[CLSCompliant(false)]
internal class DopMth
{
  private ushort m_flagsA = 6160;
  private ushort m_ftcMath;
  private uint m_dxaLeftMargin;
  private uint m_dxaRightMargin;
  private uint m_dxaIndentWrapped = 1440;

  internal byte Mthbrk
  {
    get => (byte) ((uint) this.m_flagsA & 3U);
    set => this.m_flagsA = (ushort) ((uint) this.m_flagsA & 65532U | (uint) value);
  }

  internal byte MthbrkSub
  {
    get => (byte) (((int) this.m_flagsA & 12) >> 2);
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65523 | (int) value << 2);
  }

  internal byte Mthbpjc
  {
    get => (byte) (((int) this.m_flagsA & 112 /*0x70*/) >> 4);
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65423 | (int) value << 4);
  }

  internal bool MathSmallFrac
  {
    get => ((int) this.m_flagsA & 256 /*0x0100*/) >> 8 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65279 | (value ? 1 : 0) << 8);
  }

  internal bool MathIntLimUndOvr
  {
    get => ((int) this.m_flagsA & 512 /*0x0200*/) >> 9 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65023 | (value ? 1 : 0) << 9);
  }

  internal bool MathNaryLimUndOvr
  {
    get => ((int) this.m_flagsA & 1024 /*0x0400*/) >> 10 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 64511 | (value ? 1 : 0) << 10);
  }

  internal bool MathWrapAlignLeft
  {
    get => ((int) this.m_flagsA & 2048 /*0x0800*/) >> 11 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 63487 | (value ? 1 : 0) << 11);
  }

  internal bool MathUseDispDefaults
  {
    get => ((int) this.m_flagsA & 4096 /*0x1000*/) >> 12 != 0;
    set
    {
      this.m_flagsA = (ushort) ((int) this.m_flagsA & 61439 /*0xEFFF*/ | (value ? 1 : 0) << 12);
    }
  }

  internal ushort FtcMath
  {
    get => this.m_ftcMath;
    set => this.m_ftcMath = value;
  }

  internal uint DxaLeftMargin
  {
    get => this.m_dxaLeftMargin;
    set => this.m_dxaLeftMargin = value;
  }

  internal uint DxaRightMargin
  {
    get => this.m_dxaRightMargin;
    set => this.m_dxaRightMargin = value;
  }

  internal uint DxaIndentWrapped
  {
    get => this.m_dxaIndentWrapped;
    set => this.m_dxaIndentWrapped = value;
  }

  internal DopMth()
  {
  }

  internal void Parse(Stream stream)
  {
    this.m_flagsA = BaseWordRecord.ReadUInt16(stream);
    int num1 = (int) BaseWordRecord.ReadUInt16(stream);
    this.m_ftcMath = BaseWordRecord.ReadUInt16(stream);
    this.m_dxaLeftMargin = BaseWordRecord.ReadUInt32(stream);
    this.m_dxaRightMargin = BaseWordRecord.ReadUInt32(stream);
    int num2 = (int) BaseWordRecord.ReadUInt32(stream);
    int num3 = (int) BaseWordRecord.ReadUInt32(stream);
    int num4 = (int) BaseWordRecord.ReadUInt32(stream);
    int num5 = (int) BaseWordRecord.ReadUInt32(stream);
    this.m_dxaIndentWrapped = BaseWordRecord.ReadUInt32(stream);
  }

  internal void Write(Stream stream)
  {
    BaseWordRecord.WriteUInt16(stream, this.m_flagsA);
    BaseWordRecord.WriteUInt16(stream, (ushort) 0);
    BaseWordRecord.WriteUInt16(stream, this.m_ftcMath);
    BaseWordRecord.WriteUInt32(stream, this.m_dxaLeftMargin);
    BaseWordRecord.WriteUInt32(stream, this.m_dxaRightMargin);
    BaseWordRecord.WriteUInt32(stream, 120U);
    BaseWordRecord.WriteUInt32(stream, 120U);
    BaseWordRecord.WriteUInt32(stream, 0U);
    BaseWordRecord.WriteUInt32(stream, 0U);
    BaseWordRecord.WriteUInt32(stream, this.m_dxaIndentWrapped);
  }
}
