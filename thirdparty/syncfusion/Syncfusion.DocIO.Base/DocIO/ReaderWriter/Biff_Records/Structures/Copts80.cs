// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures.Copts80
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;

[CLSCompliant(false)]
internal class Copts80
{
  private ushort m_flags = 16 /*0x10*/;
  private DOPDescriptor m_dopBase;

  internal Copts60 Copts60 => this.m_dopBase.Copts60;

  internal bool SuppressTopSpacingMac5
  {
    get => ((int) this.m_flags & 1) != 0;
    set => this.m_flags = (ushort) ((int) this.m_flags & 65534 | (value ? 1 : 0));
  }

  internal bool TruncDxaExpand
  {
    get => ((int) this.m_flags & 2) >> 1 != 0;
    set => this.m_flags = (ushort) ((int) this.m_flags & 65533 | (value ? 1 : 0) << 1);
  }

  internal bool PrintBodyBeforeHdr
  {
    get => ((int) this.m_flags & 4) >> 2 != 0;
    set => this.m_flags = (ushort) ((int) this.m_flags & 65531 | (value ? 1 : 0) << 2);
  }

  internal bool NoExtLeading
  {
    get => ((int) this.m_flags & 8) >> 3 != 0;
    set => this.m_flags = (ushort) ((int) this.m_flags & 65527 | (value ? 1 : 0) << 3);
  }

  internal bool DontMakeSpaceForUL
  {
    get => ((int) this.m_flags & 16 /*0x10*/) >> 4 != 0;
    set => this.m_flags = (ushort) ((int) this.m_flags & 65519 | (value ? 1 : 0) << 4);
  }

  internal bool MWSmallCaps
  {
    get => ((int) this.m_flags & 32 /*0x20*/) >> 5 != 0;
    set => this.m_flags = (ushort) ((int) this.m_flags & 65503 | (value ? 1 : 0) << 5);
  }

  internal bool F2ptExtLeadingOnly
  {
    get => ((int) this.m_flags & 64 /*0x40*/) >> 6 != 0;
    set => this.m_flags = (ushort) ((int) this.m_flags & 65471 | (value ? 1 : 0) << 6);
  }

  internal bool TruncFontHeight
  {
    get => ((int) this.m_flags & 128 /*0x80*/) >> 7 != 0;
    set => this.m_flags = (ushort) ((int) this.m_flags & 65407 | (value ? 1 : 0) << 7);
  }

  internal bool SubOnSize
  {
    get => ((int) this.m_flags & 256 /*0x0100*/) >> 8 != 0;
    set => this.m_flags = (ushort) ((int) this.m_flags & 65279 | (value ? 1 : 0) << 8);
  }

  internal bool LineWrapLikeWord6
  {
    get => ((int) this.m_flags & 512 /*0x0200*/) >> 9 != 0;
    set => this.m_flags = (ushort) ((int) this.m_flags & 65023 | (value ? 1 : 0) << 9);
  }

  internal bool WW6BorderRules
  {
    get => ((int) this.m_flags & 1024 /*0x0400*/) >> 10 != 0;
    set => this.m_flags = (ushort) ((int) this.m_flags & 64511 | (value ? 1 : 0) << 10);
  }

  internal bool ExactOnTop
  {
    get => ((int) this.m_flags & 2048 /*0x0800*/) >> 11 != 0;
    set => this.m_flags = (ushort) ((int) this.m_flags & 63487 | (value ? 1 : 0) << 11);
  }

  internal bool ExtraAfter
  {
    get => ((int) this.m_flags & 4096 /*0x1000*/) >> 12 != 0;
    set => this.m_flags = (ushort) ((int) this.m_flags & 61439 /*0xEFFF*/ | (value ? 1 : 0) << 12);
  }

  internal bool WPSpace
  {
    get => ((int) this.m_flags & 8192 /*0x2000*/) >> 13 != 0;
    set => this.m_flags = (ushort) ((int) this.m_flags & 57343 /*0xDFFF*/ | (value ? 1 : 0) << 13);
  }

  internal bool WPJust
  {
    get => ((int) this.m_flags & 16384 /*0x4000*/) >> 14 != 0;
    set => this.m_flags = (ushort) ((int) this.m_flags & 49151 /*0xBFFF*/ | (value ? 1 : 0) << 14);
  }

  internal bool PrintMet
  {
    get => ((int) this.m_flags & 32768 /*0x8000*/) >> 15 != 0;
    set
    {
      this.m_flags = (ushort) ((int) this.m_flags & (int) short.MaxValue | (value ? 1 : 0) << 15);
    }
  }

  internal Copts80(DOPDescriptor dopBase) => this.m_dopBase = dopBase;

  internal void Parse(Stream stream)
  {
    this.Copts60.Parse(stream);
    this.m_flags = BaseWordRecord.ReadUInt16(stream);
  }

  internal void Write(Stream stream)
  {
    this.Copts60.Write(stream);
    BaseWordRecord.WriteUInt16(stream, this.m_flags);
  }
}
