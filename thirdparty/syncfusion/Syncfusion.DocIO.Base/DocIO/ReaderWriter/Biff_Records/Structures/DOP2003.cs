// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures.DOP2003
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;

[CLSCompliant(false)]
internal class DOP2003
{
  private ushort m_flagsA;
  private byte m_flagsO = 50;
  private uint m_dxaPageLock;
  private uint m_dyaPageLock;
  private uint m_pctFontLock;
  private byte m_grfitbid;
  private ushort m_ilfoMacAtCleanup;
  private DOPDescriptor m_dopBase;

  internal bool TreatLockAtnAsReadOnly
  {
    get => ((int) this.m_flagsA & 1) != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65534 | (value ? 1 : 0));
  }

  internal bool StyleLock
  {
    get => ((int) this.m_flagsA & 2) >> 1 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65533 | (value ? 1 : 0) << 1);
  }

  internal bool AutoFmtOverride
  {
    get => ((int) this.m_flagsA & 4) >> 2 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65531 | (value ? 1 : 0) << 2);
  }

  internal bool RemoveWordML
  {
    get => ((int) this.m_flagsA & 8) >> 3 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65527 | (value ? 1 : 0) << 3);
  }

  internal bool ApplyCustomXForm
  {
    get => ((int) this.m_flagsA & 16 /*0x10*/) >> 4 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65519 | (value ? 1 : 0) << 4);
  }

  internal bool StyleLockEnforced
  {
    get => ((int) this.m_flagsA & 32 /*0x20*/) >> 5 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65503 | (value ? 1 : 0) << 5);
  }

  internal bool FakeLockAtn
  {
    get => ((int) this.m_flagsA & 64 /*0x40*/) >> 6 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65471 | (value ? 1 : 0) << 6);
  }

  internal bool IgnoreMixedContent
  {
    get => ((int) this.m_flagsA & 128 /*0x80*/) >> 7 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65407 | (value ? 1 : 0) << 7);
  }

  internal bool ShowPlaceholderText
  {
    get => ((int) this.m_flagsA & 256 /*0x0100*/) >> 8 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65279 | (value ? 1 : 0) << 8);
  }

  internal bool Word97Doc
  {
    get => ((int) this.m_flagsA & 1024 /*0x0400*/) >> 10 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 64511 | (value ? 1 : 0) << 10);
  }

  internal bool StyleLockTheme
  {
    get => ((int) this.m_flagsA & 2048 /*0x0800*/) >> 11 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 63487 | (value ? 1 : 0) << 11);
  }

  internal bool StyleLockQFSet
  {
    get => ((int) this.m_flagsA & 4096 /*0x1000*/) >> 12 != 0;
    set
    {
      this.m_flagsA = (ushort) ((int) this.m_flagsA & 61439 /*0xEFFF*/ | (value ? 1 : 0) << 12);
    }
  }

  internal bool ReadingModeInkLockDown
  {
    get => ((int) this.m_flagsO & 1) != 0;
    set => this.m_flagsO = (byte) ((int) this.m_flagsO & 254 | (value ? 1 : 0));
  }

  internal bool AcetateShowInkAtn
  {
    get => ((int) this.m_flagsO & 2) >> 1 != 0;
    set => this.m_flagsO = (byte) ((int) this.m_flagsO & 253 | (value ? 1 : 0) << 1);
  }

  internal bool FilterDttm
  {
    get => ((int) this.m_flagsO & 4) >> 2 != 0;
    set => this.m_flagsO = (byte) ((int) this.m_flagsO & 251 | (value ? 1 : 0) << 2);
  }

  internal bool EnforceDocProt
  {
    get => ((int) this.m_flagsO & 8) >> 3 != 0;
    set => this.m_flagsO = (byte) ((int) this.m_flagsO & 247 | (value ? 1 : 0) << 3);
  }

  internal byte DocProtCur
  {
    get => (byte) (((int) this.m_flagsO & 112 /*0x70*/) >> 4);
    set => this.m_flagsO = (byte) ((int) this.m_flagsO & 143 | (int) value << 4);
  }

  internal bool DispBkSpSaved
  {
    get => ((int) this.m_flagsO & 128 /*0x80*/) >> 7 != 0;
    set
    {
      this.m_flagsO = (byte) ((int) this.m_flagsO & (int) sbyte.MaxValue | (value ? 1 : 0) << 7);
    }
  }

  internal uint DxaPageLock
  {
    get => this.m_dxaPageLock;
    set => this.m_dxaPageLock = value;
  }

  internal uint DyaPageLock
  {
    get => this.m_dyaPageLock;
    set => this.m_dyaPageLock = value;
  }

  internal uint PctFontLock
  {
    get => this.m_pctFontLock;
    set => this.m_pctFontLock = value;
  }

  internal byte Grfitbid
  {
    get => this.m_grfitbid;
    set => this.m_grfitbid = value;
  }

  internal ushort IlfoMacAtCleanup
  {
    get => this.m_ilfoMacAtCleanup;
    set => this.m_ilfoMacAtCleanup = value;
  }

  internal DOP2003(DOPDescriptor dopBase) => this.m_dopBase = dopBase;

  internal void Parse(Stream stream)
  {
    this.m_flagsA = BaseWordRecord.ReadUInt16(stream);
    int num = (int) BaseWordRecord.ReadUInt16(stream);
    this.m_flagsO = (byte) stream.ReadByte();
    stream.ReadByte();
    this.m_dxaPageLock = BaseWordRecord.ReadUInt32(stream);
    this.m_dyaPageLock = BaseWordRecord.ReadUInt32(stream);
    this.m_pctFontLock = BaseWordRecord.ReadUInt32(stream);
    this.m_grfitbid = (byte) stream.ReadByte();
    stream.ReadByte();
    this.m_ilfoMacAtCleanup = BaseWordRecord.ReadUInt16(stream);
  }

  internal void Write(Stream stream)
  {
    BaseWordRecord.WriteUInt16(stream, this.m_flagsA);
    BaseWordRecord.WriteUInt16(stream, (ushort) 0);
    stream.WriteByte(this.m_flagsO);
    stream.WriteByte((byte) 0);
    BaseWordRecord.WriteUInt32(stream, this.m_dxaPageLock);
    BaseWordRecord.WriteUInt32(stream, this.m_dyaPageLock);
    BaseWordRecord.WriteUInt32(stream, this.m_pctFontLock);
    stream.WriteByte(this.m_grfitbid);
    stream.WriteByte((byte) 0);
    BaseWordRecord.WriteUInt16(stream, this.m_ilfoMacAtCleanup);
  }
}
