// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures.DOP2000
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;

[CLSCompliant(false)]
internal class DOP2000
{
  private byte m_ilvlLastBulletMain;
  private byte m_ilvlLastNumberMain;
  private ushort m_istdClickParaType;
  private ushort m_flagsA = 12800;
  private ushort m_flagsJ = 387;
  private Copts m_copts;
  private ushort m_verCompatPre10;
  private ushort m_flagsP = 4160;
  private DOPDescriptor m_dopBase;

  internal byte IlvlLastBulletMain
  {
    get => this.m_ilvlLastBulletMain;
    set => this.m_ilvlLastBulletMain = value;
  }

  internal byte IlvlLastNumberMain
  {
    get => this.m_ilvlLastNumberMain;
    set => this.m_ilvlLastNumberMain = value;
  }

  internal ushort IstdClickParaType
  {
    get => this.m_istdClickParaType;
    set => this.m_istdClickParaType = value;
  }

  internal bool LADAllDone
  {
    get => ((int) this.m_flagsA & 1) != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65534 | (value ? 1 : 0));
  }

  internal bool EnvelopeVis
  {
    get => ((int) this.m_flagsA & 2) >> 1 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65533 | (value ? 1 : 0) << 1);
  }

  internal bool MaybeTentativeListInDoc
  {
    get => ((int) this.m_flagsA & 4) >> 2 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65531 | (value ? 1 : 0) << 2);
  }

  internal bool MaybeFitText
  {
    get => ((int) this.m_flagsA & 8) >> 3 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65527 | (value ? 1 : 0) << 3);
  }

  internal bool FCCAllDone
  {
    get => ((int) this.m_flagsA & 256 /*0x0100*/) >> 8 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65279 | (value ? 1 : 0) << 8);
  }

  internal bool RelyOnCSS_WebOpt
  {
    get => ((int) this.m_flagsA & 512 /*0x0200*/) >> 9 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65023 | (value ? 1 : 0) << 9);
  }

  internal bool RelyOnVML_WebOpt
  {
    get => ((int) this.m_flagsA & 1024 /*0x0400*/) >> 10 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 64511 | (value ? 1 : 0) << 10);
  }

  internal bool AllowPNG_WebOpt
  {
    get => ((int) this.m_flagsA & 2048 /*0x0800*/) >> 11 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 63487 | (value ? 1 : 0) << 11);
  }

  internal byte ScreenSize_WebOpt
  {
    get => (byte) (((int) this.m_flagsA & 61440 /*0xF000*/) >> 12);
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 4095 /*0x0FFF*/ | (int) value << 12);
  }

  internal bool OrganizeInFolder_WebOpt
  {
    get => ((int) this.m_flagsJ & 1) != 0;
    set => this.m_flagsJ = (ushort) ((int) this.m_flagsJ & 65534 | (value ? 1 : 0));
  }

  internal bool UseLongFileNames_WebOpt
  {
    get => ((int) this.m_flagsJ & 2) >> 1 != 0;
    set => this.m_flagsJ = (ushort) ((int) this.m_flagsJ & 65533 | (value ? 1 : 0) << 1);
  }

  internal ushort PixelsPerInch_WebOpt
  {
    get => (ushort) (((int) this.m_flagsJ & 4092) >> 2);
    set => this.m_flagsJ = (ushort) ((int) this.m_flagsJ & 61443 | (int) value << 2);
  }

  internal bool WebOptionsInit
  {
    get => ((int) this.m_flagsJ & 4096 /*0x1000*/) >> 12 != 0;
    set
    {
      this.m_flagsJ = (ushort) ((int) this.m_flagsJ & 61439 /*0xEFFF*/ | (value ? 1 : 0) << 12);
    }
  }

  internal bool MaybeFEL
  {
    get => ((int) this.m_flagsJ & 4096 /*0x1000*/) >> 13 != 0;
    set
    {
      this.m_flagsJ = (ushort) ((int) this.m_flagsJ & 61439 /*0xEFFF*/ | (value ? 1 : 0) << 13);
    }
  }

  internal bool CharLineUnits
  {
    get => ((int) this.m_flagsJ & 4096 /*0x1000*/) >> 14 != 0;
    set
    {
      this.m_flagsJ = (ushort) ((int) this.m_flagsJ & 61439 /*0xEFFF*/ | (value ? 1 : 0) << 14);
    }
  }

  internal Copts Copts
  {
    get
    {
      if (this.m_copts == null)
        this.m_copts = new Copts(this.m_dopBase);
      return this.m_copts;
    }
  }

  internal ushort VerCompatPre10
  {
    get => this.m_verCompatPre10;
    set => this.m_verCompatPre10 = value;
  }

  internal bool NoMargPgvwSaved
  {
    get => ((int) this.m_flagsP & 1) != 0;
    set => this.m_flagsP = (ushort) ((int) this.m_flagsP & 65534 | (value ? 1 : 0));
  }

  internal bool BulletProofed
  {
    get => ((int) this.m_flagsP & 16 /*0x10*/) >> 4 != 0;
    set => this.m_flagsP = (ushort) ((int) this.m_flagsP & 65519 | (value ? 1 : 0) << 4);
  }

  internal bool SaveUim
  {
    get => ((int) this.m_flagsP & 64 /*0x40*/) >> 6 != 0;
    set => this.m_flagsP = (ushort) ((int) this.m_flagsP & 65471 | (value ? 1 : 0) << 6);
  }

  internal bool FilterPrivacy
  {
    get => ((int) this.m_flagsP & 128 /*0x80*/) >> 7 != 0;
    set => this.m_flagsP = (ushort) ((int) this.m_flagsP & 65407 | (value ? 1 : 0) << 7);
  }

  internal bool SeenRepairs
  {
    get => ((int) this.m_flagsP & 512 /*0x0200*/) >> 9 != 0;
    set => this.m_flagsP = (ushort) ((int) this.m_flagsP & 65023 | (value ? 1 : 0) << 9);
  }

  internal bool HasXML
  {
    get => ((int) this.m_flagsP & 1024 /*0x0400*/) >> 10 != 0;
    set => this.m_flagsP = (ushort) ((int) this.m_flagsP & 64511 | (value ? 1 : 0) << 10);
  }

  internal bool ValidateXML
  {
    get => ((int) this.m_flagsP & 4096 /*0x1000*/) >> 12 != 0;
    set
    {
      this.m_flagsP = (ushort) ((int) this.m_flagsP & 61439 /*0xEFFF*/ | (value ? 1 : 0) << 12);
    }
  }

  internal bool SaveInvalidXML
  {
    get => ((int) this.m_flagsP & 8192 /*0x2000*/) >> 13 != 0;
    set
    {
      this.m_flagsP = (ushort) ((int) this.m_flagsP & 57343 /*0xDFFF*/ | (value ? 1 : 0) << 13);
    }
  }

  internal bool ShowXMLErrors
  {
    get => ((int) this.m_flagsP & 16384 /*0x4000*/) >> 14 != 0;
    set
    {
      this.m_flagsP = (ushort) ((int) this.m_flagsP & 49151 /*0xBFFF*/ | (value ? 1 : 0) << 14);
    }
  }

  internal bool AlwaysMergeEmptyNamespace
  {
    get => ((int) this.m_flagsP & 32768 /*0x8000*/) >> 15 != 0;
    set
    {
      this.m_flagsP = (ushort) ((int) this.m_flagsP & (int) short.MaxValue | (value ? 1 : 0) << 15);
    }
  }

  internal DOP2000(DOPDescriptor dopBase) => this.m_dopBase = dopBase;

  internal void Parse(Stream stream)
  {
    this.m_ilvlLastBulletMain = (byte) stream.ReadByte();
    this.m_ilvlLastNumberMain = (byte) stream.ReadByte();
    this.m_istdClickParaType = BaseWordRecord.ReadUInt16(stream);
    this.m_flagsA = BaseWordRecord.ReadUInt16(stream);
    this.m_flagsJ = BaseWordRecord.ReadUInt16(stream);
    this.Copts.Parse(stream);
    this.m_verCompatPre10 = BaseWordRecord.ReadUInt16(stream);
    this.m_flagsP = BaseWordRecord.ReadUInt16(stream);
  }

  internal void Write(Stream stream)
  {
    stream.WriteByte(this.m_ilvlLastBulletMain);
    stream.WriteByte(this.m_ilvlLastNumberMain);
    BaseWordRecord.WriteUInt16(stream, this.m_istdClickParaType);
    BaseWordRecord.WriteUInt16(stream, this.m_flagsA);
    BaseWordRecord.WriteUInt16(stream, this.m_flagsJ);
    this.Copts.Write(stream);
    BaseWordRecord.WriteUInt16(stream, this.m_verCompatPre10);
    BaseWordRecord.WriteUInt16(stream, this.m_flagsP);
  }
}
