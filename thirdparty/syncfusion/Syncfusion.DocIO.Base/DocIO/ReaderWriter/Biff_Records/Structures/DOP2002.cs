// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures.DOP2002
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;

[CLSCompliant(false)]
internal class DOP2002
{
  private ushort m_flagsA = 61449;
  private ushort m_istdTableDflt = 4095 /*0x0FFF*/;
  private ushort m_verCompat;
  private ushort m_grfFmtFilter = 20516;
  private ushort m_iFolioPages;
  private int m_cpgText = 1252;
  private uint m_cpMinRMText;
  private uint m_cpMinRMFtn;
  private uint m_cpMinRMHdd;
  private uint m_cpMinRMAtn;
  private uint m_cpMinRMEdn;
  private uint m_cpMinRmTxbx;
  private uint m_cpMinRmHdrTxbx;
  private uint m_rsidRoot;
  private DOPDescriptor m_dopBase;

  internal bool DoNotEmbedSystemFont
  {
    get => ((int) this.m_flagsA & 1) != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65534 | (value ? 1 : 0));
  }

  internal bool WordCompat
  {
    get => ((int) this.m_flagsA & 2) >> 1 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65533 | (value ? 1 : 0) << 1);
  }

  internal bool LiveRecover
  {
    get => ((int) this.m_flagsA & 4) >> 2 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65531 | (value ? 1 : 0) << 2);
  }

  internal bool EmbedFactoids
  {
    get => ((int) this.m_flagsA & 8) >> 3 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65527 | (value ? 1 : 0) << 3);
  }

  internal bool FactoidXML
  {
    get => ((int) this.m_flagsA & 16 /*0x10*/) >> 4 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65519 | (value ? 1 : 0) << 4);
  }

  internal bool FactoidAllDone
  {
    get => ((int) this.m_flagsA & 32 /*0x20*/) >> 5 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65503 | (value ? 1 : 0) << 5);
  }

  internal bool FolioPrint
  {
    get => ((int) this.m_flagsA & 64 /*0x40*/) >> 6 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65471 | (value ? 1 : 0) << 6);
  }

  internal bool ReverseFolio
  {
    get => ((int) this.m_flagsA & 128 /*0x80*/) >> 7 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65407 | (value ? 1 : 0) << 7);
  }

  internal byte TextLineEnding
  {
    get => (byte) (((int) this.m_flagsA & 1792 /*0x0700*/) >> 8);
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 63743 | (int) value << 8);
  }

  internal bool HideFcc
  {
    get => ((int) this.m_flagsA & 2048 /*0x0800*/) >> 11 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 63487 | (value ? 1 : 0) << 11);
  }

  internal bool AcetateShowMarkup
  {
    get => ((int) this.m_flagsA & 4096 /*0x1000*/) >> 12 != 0;
    set
    {
      this.m_flagsA = (ushort) ((int) this.m_flagsA & 61439 /*0xEFFF*/ | (value ? 1 : 0) << 12);
    }
  }

  internal bool AcetateShowAtn
  {
    get => ((int) this.m_flagsA & 8192 /*0x2000*/) >> 13 != 0;
    set
    {
      this.m_flagsA = (ushort) ((int) this.m_flagsA & 57343 /*0xDFFF*/ | (value ? 1 : 0) << 13);
    }
  }

  internal bool AcetateShowInsDel
  {
    get => ((int) this.m_flagsA & 16384 /*0x4000*/) >> 14 != 0;
    set
    {
      this.m_flagsA = (ushort) ((int) this.m_flagsA & 49151 /*0xBFFF*/ | (value ? 1 : 0) << 14);
    }
  }

  internal bool AcetateShowProps
  {
    get => ((int) this.m_flagsA & 32768 /*0x8000*/) >> 15 != 0;
    set
    {
      this.m_flagsA = (ushort) ((int) this.m_flagsA & (int) short.MaxValue | (value ? 1 : 0) << 15);
    }
  }

  internal ushort IstdTableDflt
  {
    get => this.m_istdTableDflt;
    set => this.m_istdTableDflt = value;
  }

  internal ushort VerCompat
  {
    get => this.m_verCompat;
    set => this.m_verCompat = value;
  }

  internal ushort GrfFmtFilter
  {
    get => this.m_grfFmtFilter;
    set => this.m_grfFmtFilter = value;
  }

  internal ushort IFolioPages
  {
    get => this.m_iFolioPages;
    set => this.m_iFolioPages = value;
  }

  internal int CpgText
  {
    get => this.m_cpgText;
    set => this.m_cpgText = value;
  }

  internal uint CpMinRMText
  {
    get => this.m_cpMinRMText;
    set => this.m_cpMinRMText = value;
  }

  internal uint CpMinRMFtn
  {
    get => this.m_cpMinRMFtn;
    set => this.m_cpMinRMFtn = value;
  }

  internal uint CpMinRMHdd
  {
    get => this.m_cpMinRMHdd;
    set => this.m_cpMinRMHdd = value;
  }

  internal uint CpMinRMAtn
  {
    get => this.m_cpMinRMAtn;
    set => this.m_cpMinRMAtn = value;
  }

  internal uint CpMinRMEdn
  {
    get => this.m_cpMinRMEdn;
    set => this.m_cpMinRMEdn = value;
  }

  internal uint CpMinRmTxbx
  {
    get => this.m_cpMinRmTxbx;
    set => this.m_cpMinRmTxbx = value;
  }

  internal uint CpMinRmHdrTxbx
  {
    get => this.m_cpMinRmHdrTxbx;
    set => this.m_cpMinRmHdrTxbx = value;
  }

  internal DOP2002(DOPDescriptor dopBase) => this.m_dopBase = dopBase;

  internal void Parse(Stream stream)
  {
    BaseWordRecord.ReadInt32(stream);
    this.m_flagsA = BaseWordRecord.ReadUInt16(stream);
    this.m_istdTableDflt = BaseWordRecord.ReadUInt16(stream);
    this.m_verCompat = BaseWordRecord.ReadUInt16(stream);
    this.m_grfFmtFilter = BaseWordRecord.ReadUInt16(stream);
    this.m_iFolioPages = BaseWordRecord.ReadUInt16(stream);
    this.m_cpgText = BaseWordRecord.ReadInt32(stream);
    this.m_cpMinRMText = BaseWordRecord.ReadUInt32(stream);
    this.m_cpMinRMFtn = BaseWordRecord.ReadUInt32(stream);
    this.m_cpMinRMHdd = BaseWordRecord.ReadUInt32(stream);
    this.m_cpMinRMAtn = BaseWordRecord.ReadUInt32(stream);
    this.m_cpMinRMEdn = BaseWordRecord.ReadUInt32(stream);
    this.m_cpMinRmTxbx = BaseWordRecord.ReadUInt32(stream);
    this.m_cpMinRmHdrTxbx = BaseWordRecord.ReadUInt32(stream);
    this.m_rsidRoot = BaseWordRecord.ReadUInt32(stream);
  }

  internal void Write(Stream stream)
  {
    BaseWordRecord.WriteInt32(stream, 0);
    BaseWordRecord.WriteUInt16(stream, this.m_flagsA);
    BaseWordRecord.WriteUInt16(stream, this.m_istdTableDflt);
    BaseWordRecord.WriteUInt16(stream, this.m_verCompat);
    BaseWordRecord.WriteUInt16(stream, this.m_grfFmtFilter);
    BaseWordRecord.WriteUInt16(stream, this.m_iFolioPages);
    BaseWordRecord.WriteInt32(stream, this.m_cpgText);
    BaseWordRecord.WriteUInt32(stream, this.m_cpMinRMText);
    BaseWordRecord.WriteUInt32(stream, this.m_cpMinRMFtn);
    BaseWordRecord.WriteUInt32(stream, this.m_cpMinRMHdd);
    BaseWordRecord.WriteUInt32(stream, this.m_cpMinRMAtn);
    BaseWordRecord.WriteUInt32(stream, this.m_cpMinRMEdn);
    BaseWordRecord.WriteUInt32(stream, this.m_cpMinRmTxbx);
    BaseWordRecord.WriteUInt32(stream, this.m_cpMinRmHdrTxbx);
    BaseWordRecord.WriteUInt32(stream, this.m_rsidRoot);
  }
}
