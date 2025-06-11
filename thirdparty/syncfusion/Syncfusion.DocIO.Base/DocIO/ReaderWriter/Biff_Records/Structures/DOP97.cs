// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures.DOP97
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;

[CLSCompliant(false)]
internal class DOP97
{
  private ushort m_adt;
  private DopTypography m_doptypography;
  private Dogrid m_dogrid;
  private ushort m_flagsA = 13394;
  private Asumyi m_asumyi;
  private uint m_cChWS;
  private uint m_cChWSWithSubdocs;
  private uint m_grfDocEvents;
  private uint m_flagsM;
  private uint m_cpMaxListCacheMainDoc;
  private ushort m_ilfoLastBulletMain;
  private ushort m_ilfoLastNumberMain;
  private uint m_cDBC;
  private uint m_cDBCWithSubdocs;
  private ushort m_nfcFtnRef;
  private ushort m_nfcEdnRef = 2;
  private ushort m_hpsZoomFontPag;
  private ushort m_dywDispPag;
  private DOPDescriptor m_dopBase;

  internal ushort Adt
  {
    get => this.m_adt;
    set => this.m_adt = value;
  }

  internal DopTypography DopTypography
  {
    get
    {
      if (this.m_doptypography == null)
        this.m_doptypography = new DopTypography();
      return this.m_doptypography;
    }
  }

  internal Dogrid Dogrid
  {
    get
    {
      if (this.m_dogrid == null)
        this.m_dogrid = new Dogrid();
      return this.m_dogrid;
    }
  }

  internal byte LvlDop
  {
    get => (byte) (((int) this.m_flagsA & 30) >> 1);
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65505 | (int) value << 1);
  }

  internal bool GramAllDone
  {
    get => ((int) this.m_flagsA & 32 /*0x20*/) >> 5 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65503 | (value ? 1 : 0) << 5);
  }

  internal bool GramAllClean
  {
    get => ((int) this.m_flagsA & 64 /*0x40*/) >> 6 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65471 | (value ? 1 : 0) << 6);
  }

  internal bool SubsetFonts
  {
    get => ((int) this.m_flagsA & 128 /*0x80*/) >> 7 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65407 | (value ? 1 : 0) << 7);
  }

  internal bool HtmlDoc
  {
    get => ((int) this.m_flagsA & 512 /*0x0200*/) >> 9 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 65023 | (value ? 1 : 0) << 9);
  }

  internal bool DiskLvcInvalid
  {
    get => ((int) this.m_flagsA & 1024 /*0x0400*/) >> 10 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 64511 | (value ? 1 : 0) << 10);
  }

  internal bool SnapBorder
  {
    get => ((int) this.m_flagsA & 2048 /*0x0800*/) >> 11 != 0;
    set => this.m_flagsA = (ushort) ((int) this.m_flagsA & 63487 | (value ? 1 : 0) << 11);
  }

  internal bool IncludeHeader
  {
    get => ((int) this.m_flagsA & 4096 /*0x1000*/) >> 12 != 0;
    set
    {
      this.m_flagsA = (ushort) ((int) this.m_flagsA & 61439 /*0xEFFF*/ | (value ? 1 : 0) << 12);
    }
  }

  internal bool IncludeFooter
  {
    get => ((int) this.m_flagsA & 8192 /*0x2000*/) >> 13 != 0;
    set
    {
      this.m_flagsA = (ushort) ((int) this.m_flagsA & 57343 /*0xDFFF*/ | (value ? 1 : 0) << 13);
    }
  }

  internal Asumyi Asumyi
  {
    get
    {
      if (this.m_asumyi == null)
        this.m_asumyi = new Asumyi();
      return this.m_asumyi;
    }
  }

  internal uint CChWS
  {
    get => this.m_cChWS;
    set => this.m_cChWS = value;
  }

  internal uint CChWSWithSubdocs
  {
    get => this.m_cChWSWithSubdocs;
    set => this.m_cChWSWithSubdocs = value;
  }

  internal uint GrfDocEvents
  {
    get => this.m_grfDocEvents;
    set => this.m_grfDocEvents = value;
  }

  internal bool VirusPrompted
  {
    get => ((int) this.m_flagsM & 1) != 0;
    set => this.m_flagsM = (uint) ((long) (this.m_flagsM & 4294967294U) | (value ? 1L : 0L));
  }

  internal bool VirusLoadSafe
  {
    get => (this.m_flagsM & 2U) >> 1 != 0U;
    set
    {
      this.m_flagsM = (uint) ((long) (this.m_flagsM & 4294967293U) | (long) ((value ? 1 : 0) << 1));
    }
  }

  internal uint KeyVirusSession30
  {
    get => (this.m_flagsM & 4294967292U) >> 2;
    set => this.m_flagsM = (uint) ((int) this.m_flagsM & 3 | (int) value << 2);
  }

  internal uint CpMaxListCacheMainDoc
  {
    get => this.m_cpMaxListCacheMainDoc;
    set => this.m_cpMaxListCacheMainDoc = value;
  }

  internal ushort IlfoLastBulletMain
  {
    get => this.m_ilfoLastBulletMain;
    set => this.m_ilfoLastBulletMain = value;
  }

  internal ushort IlfoLastNumberMain
  {
    get => this.m_ilfoLastNumberMain;
    set => this.m_ilfoLastNumberMain = value;
  }

  internal uint CDBC
  {
    get => this.m_cDBC;
    set => this.m_cDBC = value;
  }

  internal uint CDBCWithSubdocs
  {
    get => this.m_cDBCWithSubdocs;
    set => this.m_cDBCWithSubdocs = value;
  }

  internal ushort NfcFtnRef
  {
    get => this.m_nfcFtnRef;
    set => this.m_nfcFtnRef = value;
  }

  internal ushort NfcEdnRef
  {
    get => this.m_nfcEdnRef;
    set => this.m_nfcEdnRef = value;
  }

  internal ushort HpsZoomFontPag
  {
    get => this.m_hpsZoomFontPag;
    set => this.m_hpsZoomFontPag = value;
  }

  internal ushort DywDispPag
  {
    get => this.m_dywDispPag;
    set => this.m_dywDispPag = value;
  }

  internal DOP97(DOPDescriptor dopBase) => this.m_dopBase = dopBase;

  internal void Parse(Stream stream)
  {
    this.m_adt = BaseWordRecord.ReadUInt16(stream);
    this.DopTypography.Parse(stream);
    this.Dogrid.Parse(stream);
    this.m_flagsA = BaseWordRecord.ReadUInt16(stream);
    int num1 = (int) BaseWordRecord.ReadUInt16(stream);
    this.Asumyi.Parse(stream);
    this.m_cChWS = BaseWordRecord.ReadUInt32(stream);
    this.m_cChWSWithSubdocs = BaseWordRecord.ReadUInt32(stream);
    this.m_grfDocEvents = BaseWordRecord.ReadUInt32(stream);
    this.m_flagsM = BaseWordRecord.ReadUInt32(stream);
    byte[] buffer = new byte[30];
    stream.Read(buffer, 0, buffer.Length);
    this.m_cpMaxListCacheMainDoc = BaseWordRecord.ReadUInt32(stream);
    this.m_ilfoLastBulletMain = BaseWordRecord.ReadUInt16(stream);
    this.m_ilfoLastNumberMain = BaseWordRecord.ReadUInt16(stream);
    this.m_cDBC = BaseWordRecord.ReadUInt32(stream);
    this.m_cDBCWithSubdocs = BaseWordRecord.ReadUInt32(stream);
    int num2 = (int) BaseWordRecord.ReadUInt32(stream);
    this.m_nfcFtnRef = BaseWordRecord.ReadUInt16(stream);
    this.m_nfcEdnRef = BaseWordRecord.ReadUInt16(stream);
    this.m_hpsZoomFontPag = BaseWordRecord.ReadUInt16(stream);
    this.m_dywDispPag = BaseWordRecord.ReadUInt16(stream);
  }

  internal void Write(Stream stream)
  {
    BaseWordRecord.WriteUInt16(stream, this.m_adt);
    this.DopTypography.Write(stream);
    this.Dogrid.Write(stream);
    BaseWordRecord.WriteUInt16(stream, this.m_flagsA);
    BaseWordRecord.WriteUInt16(stream, (ushort) 0);
    this.Asumyi.Write(stream);
    BaseWordRecord.WriteUInt32(stream, this.m_cChWS);
    BaseWordRecord.WriteUInt32(stream, this.m_cChWSWithSubdocs);
    BaseWordRecord.WriteUInt32(stream, this.m_grfDocEvents);
    BaseWordRecord.WriteUInt32(stream, this.m_flagsM);
    byte[] buffer = new byte[30];
    stream.Write(buffer, 0, buffer.Length);
    BaseWordRecord.WriteUInt32(stream, this.m_cpMaxListCacheMainDoc);
    BaseWordRecord.WriteUInt16(stream, this.m_ilfoLastBulletMain);
    BaseWordRecord.WriteUInt16(stream, this.m_ilfoLastNumberMain);
    BaseWordRecord.WriteUInt32(stream, this.m_cDBC);
    BaseWordRecord.WriteUInt32(stream, this.m_cDBCWithSubdocs);
    BaseWordRecord.WriteUInt32(stream, 0U);
    BaseWordRecord.WriteUInt16(stream, this.m_nfcFtnRef);
    BaseWordRecord.WriteUInt16(stream, this.m_nfcEdnRef);
    BaseWordRecord.WriteUInt16(stream, this.m_hpsZoomFontPag);
    BaseWordRecord.WriteUInt16(stream, this.m_dywDispPag);
  }
}
