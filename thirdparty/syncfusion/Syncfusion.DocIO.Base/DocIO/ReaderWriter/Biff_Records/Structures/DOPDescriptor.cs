// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures.DOPDescriptor
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.ReaderWriter.Security;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records.Structures;

[CLSCompliant(false)]
internal class DOPDescriptor : BaseWordRecord
{
  private const int DEF_PROTECTION_KEY = 0;
  internal const int DEF_MAX_PASSWORDLEN = 15;
  private const ushort DEF_PASSWORD_CONST = 52811;
  private byte m_nfcEdnRef = 2;
  private byte m_nfcFtnRef;
  private byte m_bepc = 3;
  private int m_nEdn = 1;
  private byte m_rncEdn;
  private int m_nFtn = 1;
  private byte m_rncFtn;
  private bool m_bFacingPage;
  private bool m_bWidowControl = true;
  private bool m_bPMHMainDoc;
  private int m_grfSuppression;
  private byte m_fpc = 1;
  private int m_unused0_7;
  private int m_grpfIhdt;
  private uint m_flagA = 411568305;
  private Copts60 m_copts60;
  private ushort m_dxaTabs = 720;
  private int m_wSpare = 1251;
  private int m_dxaHotZ = 360;
  private int m_cConsecHypLim;
  private int m_wSpare2;
  private uint m_dttmCreated;
  private uint m_dttmRevised;
  private uint m_dttmLastPrint;
  private int m_nRevision;
  private int m_tmEdited;
  private int m_cWords;
  private int m_cCh;
  private int m_cPg;
  private int m_cParas;
  private ushort m_End = 4;
  private ushort m_epc = 4099;
  private int m_cLines;
  private int m_wordsFtnEnd;
  private int m_cChFtnEdn;
  private int m_cPgFtnEdn;
  private int m_cParasFrnEdn;
  private int m_cLinesFtnEdn;
  private uint m_lKeyProtDoc;
  private ushort m_wvkSaved = 801;
  private bool m_shadeFormData = true;
  internal byte[] m_dopLeftData;
  private DateTime m_created = DateTime.Now;
  private DateTime m_revised = DateTime.Now;
  private DateTime m_lastPrinted = DateTime.MinValue;
  private TimeSpan m_editTime = TimeSpan.MinValue;
  private DOP95 m_dop95;
  private DOP97 m_dop97;
  private DOP2000 m_dop2000;
  private DOP2002 m_dop2002;
  private DOP2003 m_dop2003;
  private DOP2007 m_dop2007;

  internal DOP95 Dop95
  {
    get
    {
      if (this.m_dop95 == null)
        this.m_dop95 = new DOP95(this);
      return this.m_dop95;
    }
  }

  internal DOP97 Dop97
  {
    get
    {
      if (this.m_dop97 == null)
        this.m_dop97 = new DOP97(this);
      return this.m_dop97;
    }
  }

  internal DOP2000 Dop2000
  {
    get
    {
      if (this.m_dop2000 == null)
        this.m_dop2000 = new DOP2000(this);
      return this.m_dop2000;
    }
  }

  internal DOP2002 Dop2002
  {
    get
    {
      if (this.m_dop2002 == null)
        this.m_dop2002 = new DOP2002(this);
      return this.m_dop2002;
    }
  }

  internal DOP2003 Dop2003
  {
    get
    {
      if (this.m_dop2003 == null)
        this.m_dop2003 = new DOP2003(this);
      return this.m_dop2003;
    }
  }

  internal DOP2007 Dop2007
  {
    get
    {
      if (this.m_dop2007 == null)
        this.m_dop2007 = new DOP2007(this);
      return this.m_dop2007;
    }
  }

  internal Copts60 Copts60
  {
    get
    {
      if (this.m_copts60 == null)
        this.m_copts60 = new Copts60(this);
      return this.m_copts60;
    }
  }

  internal byte EndnoteNumberFormat
  {
    get => (byte) this.Dop97.NfcEdnRef;
    set
    {
      if (this.Dop97.NfcEdnRef < (ushort) 0 || this.Dop97.NfcEdnRef >= (ushort) 5)
        return;
      this.Dop97.NfcEdnRef = (ushort) value;
    }
  }

  internal byte FootnoteNumberFormat
  {
    get => (byte) this.Dop97.NfcFtnRef;
    set
    {
      if (this.Dop97.NfcFtnRef < (ushort) 0 || this.Dop97.NfcFtnRef >= (ushort) 5)
        return;
      this.Dop97.NfcFtnRef = (ushort) value;
    }
  }

  internal byte EndnotePosition
  {
    get => this.m_bepc;
    set => this.m_bepc = value;
  }

  internal int InitialEndnoteNumber
  {
    get => this.m_nEdn;
    set => this.m_nEdn = value;
  }

  internal byte RestartIndexForEndnote
  {
    get => this.m_rncEdn;
    set
    {
      if (this.m_rncEdn >= (byte) 3 || this.m_rncEdn < (byte) 0)
        return;
      this.m_rncEdn = value;
    }
  }

  internal int InitialFootnoteNumber
  {
    get => this.m_nFtn;
    set => this.m_nFtn = value;
  }

  internal byte RestartIndexForFootnotes
  {
    get => this.m_rncFtn;
    set
    {
      if (value < (byte) 0 || value >= (byte) 3)
        return;
      this.m_rncFtn = value;
    }
  }

  internal byte FootnotePosition
  {
    get => this.m_fpc;
    set
    {
      if (value < (byte) 0 || value >= (byte) 3)
        return;
      this.m_fpc = value;
    }
  }

  internal ProtectionType ProtectionType
  {
    get
    {
      if (!this.Dop2003.EnforceDocProt || this.Dop2003.DocProtCur == (byte) 7)
        return ProtectionType.NoProtection;
      if (this.ProtEnabled && this.Dop2003.DocProtCur == (byte) 2)
        return ProtectionType.AllowOnlyFormFields;
      if (this.LockAtn)
      {
        if (this.Dop2003.TreatLockAtnAsReadOnly && this.Dop2003.DocProtCur == (byte) 3)
          return ProtectionType.AllowOnlyReading;
        if (this.Dop2003.DocProtCur == (byte) 1)
          return ProtectionType.AllowOnlyComments;
      }
      return this.LockRev && this.Dop2003.DocProtCur == (byte) 0 ? ProtectionType.AllowOnlyRevisions : ProtectionType.NoProtection;
    }
    set
    {
      if (this.LockRev && this.Dop2003.DocProtCur == (byte) 0)
        this.RevMarking = false;
      this.LockAtn = false;
      this.ProtEnabled = false;
      this.LockRev = false;
      this.Dop2003.EnforceDocProt = false;
      this.Dop2003.DocProtCur = (byte) 3;
      switch (value)
      {
        case ProtectionType.NoProtection:
          if (this.ProtectionType == ProtectionType.NoProtection || this.m_lKeyProtDoc != 0U)
            break;
          this.m_lKeyProtDoc = 0U;
          break;
        case ProtectionType.AllowOnlyRevisions:
          this.Dop2003.EnforceDocProt = true;
          this.Dop2003.DocProtCur = (byte) 0;
          this.LockRev = true;
          goto case ProtectionType.NoProtection;
        case ProtectionType.AllowOnlyComments:
          this.Dop2003.EnforceDocProt = true;
          this.Dop2003.DocProtCur = (byte) 1;
          this.LockAtn = true;
          goto case ProtectionType.NoProtection;
        case ProtectionType.AllowOnlyFormFields:
          this.Dop2003.EnforceDocProt = true;
          this.Dop2003.DocProtCur = (byte) 2;
          this.ProtEnabled = true;
          goto case ProtectionType.NoProtection;
        case ProtectionType.AllowOnlyReading:
          this.LockAtn = true;
          this.Dop2003.TreatLockAtnAsReadOnly = true;
          this.Dop2003.EnforceDocProt = true;
          this.Dop2003.DocProtCur = (byte) 3;
          goto case ProtectionType.NoProtection;
        default:
          throw new ArgumentException("Unknown protection specified.");
      }
    }
  }

  internal uint ProtectionKey => this.m_lKeyProtDoc;

  internal bool OddAndEvenPagesHeaderFooter
  {
    get => this.m_bFacingPage;
    set => this.m_bFacingPage = value;
  }

  internal byte ViewType
  {
    get => (byte) ((uint) this.m_wvkSaved & 7U);
    set
    {
      this.m_wvkSaved &= (ushort) 65528;
      this.m_wvkSaved = (ushort) value;
    }
  }

  internal ushort ZoomPercent
  {
    get => (ushort) (((int) this.m_wvkSaved & 4088) >> 3);
    set
    {
      this.m_wvkSaved &= (ushort) 61447;
      this.m_wvkSaved += (ushort) ((uint) value << 3);
    }
  }

  internal byte ZoomType
  {
    get => (byte) (((int) this.m_wvkSaved & 12288 /*0x3000*/) >> 12);
    set
    {
      this.m_wvkSaved &= (ushort) 53247 /*0xCFFF*/;
      this.m_wvkSaved += (ushort) ((uint) value << 12);
    }
  }

  internal ushort DefaultTabWidth
  {
    get => this.m_dxaTabs;
    set => this.m_dxaTabs = value;
  }

  internal int DxaHotZ
  {
    get => this.m_dxaHotZ;
    set => this.m_dxaHotZ = value;
  }

  internal int ConsecHypLim
  {
    get => this.m_cConsecHypLim;
    set => this.m_cConsecHypLim = value;
  }

  internal bool SpellAllDone
  {
    get => (this.m_flagA & 64U /*0x40*/) >> 6 != 0U;
    set
    {
      this.m_flagA = (uint) ((long) (this.m_flagA & 4294967231U) | (long) ((value ? 1 : 0) << 6));
    }
  }

  internal bool SpellAllClean
  {
    get => (this.m_flagA & 128U /*0x80*/) >> 7 != 0U;
    set
    {
      this.m_flagA = (uint) ((long) (this.m_flagA & 4294967167U) | (long) ((value ? 1 : 0) << 7));
    }
  }

  internal bool SpellHideErrors
  {
    get => (this.m_flagA & 256U /*0x0100*/) >> 8 != 0U;
    set
    {
      this.m_flagA = (uint) ((long) (this.m_flagA & 4294967039U) | (long) ((value ? 1 : 0) << 8));
    }
  }

  internal bool GramHideErrors
  {
    get => (this.m_flagA & 512U /*0x0200*/) >> 9 != 0U;
    set
    {
      this.m_flagA = (uint) ((long) (this.m_flagA & 4294966783U) | (long) ((value ? 1 : 0) << 9));
    }
  }

  internal bool LabelDoc
  {
    get => (this.m_flagA & 1024U /*0x0400*/) >> 10 != 0U;
    set
    {
      this.m_flagA = (uint) ((long) (this.m_flagA & 4294966271U) | (long) ((value ? 1 : 0) << 10));
    }
  }

  internal bool HyphCapitals
  {
    get => (this.m_flagA & 2048U /*0x0800*/) >> 11 != 0U;
    set
    {
      this.m_flagA = (uint) ((long) (this.m_flagA & 4294965247U) | (long) ((value ? 1 : 0) << 11));
    }
  }

  internal bool AutoHyphen
  {
    get => (this.m_flagA & 4096U /*0x1000*/) >> 12 != 0U;
    set
    {
      this.m_flagA = (uint) ((long) (this.m_flagA & 4294963199U) | (long) ((value ? 1 : 0) << 12));
    }
  }

  internal bool FormNoFields
  {
    get => (this.m_flagA & 8192U /*0x2000*/) >> 13 != 0U;
    set
    {
      this.m_flagA = (uint) ((long) (this.m_flagA & 4294959103U) | (long) ((value ? 1 : 0) << 13));
    }
  }

  internal bool LinkStyles
  {
    get => (this.m_flagA & 16384U /*0x4000*/) >> 14 != 0U;
    set
    {
      this.m_flagA = (uint) ((long) (this.m_flagA & 4294950911U) | (long) ((value ? 1 : 0) << 14));
    }
  }

  internal bool RevMarking
  {
    get => (this.m_flagA & 32768U /*0x8000*/) >> 15 != 0U;
    set
    {
      this.m_flagA = (uint) ((long) (this.m_flagA & 4294934527U) | (long) ((value ? 1 : 0) << 15));
    }
  }

  internal bool ExactCWords
  {
    get => (this.m_flagA & 131072U /*0x020000*/) >> 17 != 0U;
    set
    {
      this.m_flagA = (uint) ((long) (this.m_flagA & 4294836223U) | (long) ((value ? 1 : 0) << 17));
    }
  }

  internal bool PagHidden
  {
    get => (this.m_flagA & 262144U /*0x040000*/) >> 18 != 0U;
    set
    {
      this.m_flagA = (uint) ((long) (this.m_flagA & 4294705151U) | (long) ((value ? 1 : 0) << 18));
    }
  }

  internal bool PagResults
  {
    get => (this.m_flagA & 524288U /*0x080000*/) >> 19 != 0U;
    set
    {
      this.m_flagA = (uint) ((long) (this.m_flagA & 4294443007U) | (long) ((value ? 1 : 0) << 19));
    }
  }

  internal bool LockAtn
  {
    get => (this.m_flagA & 1048576U /*0x100000*/) >> 20 != 0U;
    set
    {
      this.m_flagA = (uint) ((long) (this.m_flagA & 4293918719U) | (long) ((value ? 1 : 0) << 20));
    }
  }

  internal bool MirrorMargins
  {
    get => (this.m_flagA & 2097152U /*0x200000*/) >> 21 != 0U;
    set
    {
      this.m_flagA = (uint) ((long) (this.m_flagA & 4293918719U) | (long) ((value ? 1 : 0) << 21));
    }
  }

  internal bool Word97Compat
  {
    get => (this.m_flagA & 4194304U /*0x400000*/) >> 22 != 0U;
    set
    {
      this.m_flagA = (uint) ((long) (this.m_flagA & 4290772991U) | (long) ((value ? 1 : 0) << 22));
    }
  }

  internal bool ProtEnabled
  {
    get => (this.m_flagA & 33554432U /*0x02000000*/) >> 25 != 0U;
    set
    {
      this.m_flagA = (uint) ((long) (this.m_flagA & 4261412863U) | (long) ((value ? 1 : 0) << 25));
    }
  }

  internal bool DispFormFldSel
  {
    get => (this.m_flagA & 67108864U /*0x04000000*/) >> 26 != 0U;
    set
    {
      this.m_flagA = (uint) ((long) (this.m_flagA & 4227858431U) | (long) ((value ? 1 : 0) << 26));
    }
  }

  internal bool RMView
  {
    get => (this.m_flagA & 134217728U /*0x08000000*/) >> 27 != 0U;
    set
    {
      this.m_flagA = (uint) ((long) (this.m_flagA & 4160749567U) | (long) ((value ? 1 : 0) << 27));
    }
  }

  internal bool RMPrint
  {
    get => (this.m_flagA & 268435456U /*0x10000000*/) >> 28 != 0U;
    set
    {
      this.m_flagA = (uint) ((long) (this.m_flagA & 4026531839U /*0xEFFFFFFF*/) | (long) ((value ? 1 : 0) << 28));
    }
  }

  internal bool LockVbaProj
  {
    get => (this.m_flagA & 536870912U /*0x20000000*/) >> 29 != 0U;
    set
    {
      this.m_flagA = (uint) ((long) (this.m_flagA & 3758096383U /*0xDFFFFFFF*/) | (long) ((value ? 1 : 0) << 29));
    }
  }

  internal bool LockRev
  {
    get => (this.m_flagA & 1073741824U /*0x40000000*/) >> 30 != 0U;
    set
    {
      this.m_flagA = (uint) ((long) (this.m_flagA & 3221225471U /*0xBFFFFFFF*/) | (long) ((value ? 1 : 0) << 30));
    }
  }

  internal bool EmbedFonts
  {
    get => (this.m_flagA & 2147483648U /*0x80000000*/) >> 31 /*0x1F*/ != 0U;
    set
    {
      this.m_flagA = (uint) ((long) (this.m_flagA & (uint) int.MaxValue) | (long) ((value ? 1 : 0) << 31 /*0x1F*/));
    }
  }

  internal byte[] DopInternalData
  {
    get => this.m_dopLeftData;
    set => this.m_dopLeftData = value;
  }

  internal bool FormFieldShading
  {
    get => this.m_shadeFormData;
    set => this.m_shadeFormData = value;
  }

  internal bool GutterAtTop
  {
    get => (this.m_flagA & 8U) >> 3 != 0U;
    set
    {
      this.m_flagA = (uint) ((long) (this.m_flagA & 4294967287U) | (long) ((value ? 1 : 0) << 3));
    }
  }

  internal DOPDescriptor()
  {
  }

  internal DOPDescriptor(Stream stream, int dopStart, int dopLength, bool isTemplate)
    : this()
  {
    stream.Position = (long) dopStart;
    int num1 = (int) BaseWordRecord.ReadUInt16(stream);
    this.m_bFacingPage = (num1 & 1) != 0;
    this.m_bWidowControl = (num1 & 2) != 0;
    this.m_bPMHMainDoc = (num1 & 4) != 0;
    this.m_grfSuppression = (num1 & 24) >> 3;
    this.m_fpc = (byte) ((num1 & 96 /*0x60*/) >> 5);
    this.m_unused0_7 = (num1 & 128 /*0x80*/) >> 7;
    this.m_grpfIhdt = (num1 & 65280) >> 8;
    int num2 = (int) BaseWordRecord.ReadUInt16(stream);
    this.m_rncFtn = (byte) (num2 & 3);
    this.m_nFtn = (num2 & 65532) >> 2;
    this.m_flagA = BaseWordRecord.ReadUInt32(stream);
    this.Copts60.Parse(stream);
    this.m_dxaTabs = BaseWordRecord.ReadUInt16(stream);
    this.m_wSpare = (int) BaseWordRecord.ReadUInt16(stream);
    this.m_dxaHotZ = (int) BaseWordRecord.ReadUInt16(stream);
    this.m_cConsecHypLim = (int) BaseWordRecord.ReadUInt16(stream);
    this.m_wSpare2 = (int) BaseWordRecord.ReadUInt16(stream);
    this.m_dttmCreated = BaseWordRecord.ReadUInt32(stream);
    this.m_created = !isTemplate ? this.ParseDateTime(this.m_dttmCreated) : DateTime.Now;
    this.m_dttmRevised = BaseWordRecord.ReadUInt32(stream);
    this.m_revised = !isTemplate ? this.ParseDateTime(this.m_dttmRevised) : DateTime.Now;
    this.m_dttmLastPrint = BaseWordRecord.ReadUInt32(stream);
    this.m_lastPrinted = !isTemplate ? this.ParseDateTime(this.m_dttmLastPrint) : DateTime.Now;
    this.m_nRevision = (int) BaseWordRecord.ReadInt16(stream);
    this.m_tmEdited = BaseWordRecord.ReadInt32(stream);
    this.m_editTime = TimeSpan.FromMinutes((double) this.m_tmEdited);
    this.m_cWords = BaseWordRecord.ReadInt32(stream);
    this.m_cCh = BaseWordRecord.ReadInt32(stream);
    this.m_cPg = (int) BaseWordRecord.ReadInt16(stream);
    this.m_cParas = BaseWordRecord.ReadInt32(stream);
    this.m_End = BaseWordRecord.ReadUInt16(stream);
    this.m_rncEdn = (byte) ((uint) this.m_End & 3U);
    this.m_nEdn = ((int) this.m_End & 65532) >> 2;
    this.m_epc = BaseWordRecord.ReadUInt16(stream);
    this.m_nfcFtnRef = (byte) (((int) this.m_epc & 60) >> 2);
    this.m_nfcEdnRef = (byte) (((int) this.m_epc & 960) >> 6);
    this.m_bepc = (byte) ((uint) this.m_epc & 3U);
    this.m_shadeFormData = ((int) this.m_epc & 4096 /*0x1000*/) == 4096 /*0x1000*/;
    this.m_cLines = BaseWordRecord.ReadInt32(stream);
    this.m_wordsFtnEnd = BaseWordRecord.ReadInt32(stream);
    this.m_cChFtnEdn = BaseWordRecord.ReadInt32(stream);
    this.m_cPgFtnEdn = (int) BaseWordRecord.ReadInt16(stream);
    this.m_cParasFrnEdn = BaseWordRecord.ReadInt32(stream);
    this.m_cLinesFtnEdn = BaseWordRecord.ReadInt32(stream);
    this.m_lKeyProtDoc = BaseWordRecord.ReadUInt32(stream);
    this.m_wvkSaved = BaseWordRecord.ReadUInt16(stream);
    this.GutterAtTop = ((int) this.m_wvkSaved & 32768 /*0x8000*/) != 0;
    if (dopLength >= 88)
      this.Dop95.Parse(stream);
    if (dopLength >= 500)
      this.Dop97.Parse(stream);
    if (dopLength >= 544)
      this.Dop2000.Parse(stream);
    if (dopLength >= 594)
      this.Dop2002.Parse(stream);
    if (dopLength >= 616)
      this.Dop2003.Parse(stream);
    else if (this.LockAtn)
    {
      this.Dop2003.EnforceDocProt = true;
      this.Dop2003.DocProtCur = (byte) 1;
    }
    else if (this.ProtEnabled)
    {
      this.Dop2003.EnforceDocProt = true;
      this.Dop2003.DocProtCur = (byte) 2;
    }
    else if (this.LockRev)
    {
      this.Dop2003.EnforceDocProt = true;
      this.Dop2003.DocProtCur = (byte) 0;
    }
    if (dopLength > 674)
      this.Dop2007.Parse(stream);
    if (dopLength <= (int) stream.Position - dopStart)
      return;
    int count = dopLength - ((int) stream.Position - dopStart);
    this.m_dopLeftData = new byte[count];
    stream.Read(this.m_dopLeftData, 0, count);
  }

  internal void UpdateDateTime(
    BuiltinDocumentProperties builtInDocumnetProperties)
  {
    this.m_created = new DateTime(builtInDocumnetProperties.CreateDate.Ticks);
    this.m_revised = new DateTime(builtInDocumnetProperties.LastSaveDate.Ticks);
    this.m_lastPrinted = new DateTime(builtInDocumnetProperties.LastPrinted.Ticks);
    this.m_editTime = builtInDocumnetProperties.TotalEditingTime;
  }

  internal uint Write(Stream stream)
  {
    this.m_bWidowControl = false;
    long position = stream.Position;
    int num1 = 0 | (this.m_bFacingPage ? 1 : 0) | (this.m_bWidowControl ? 2 : 0) | (this.m_bPMHMainDoc ? 4 : 0) | this.m_grfSuppression << 3 | (int) this.m_fpc << 5 | this.m_unused0_7 << 7 | this.m_grpfIhdt << 8;
    BaseWordRecord.WriteUInt16(stream, (ushort) num1);
    int num2 = 0 | (int) this.m_rncFtn | this.m_nFtn << 2;
    BaseWordRecord.WriteUInt16(stream, (ushort) num2);
    if (this.ProtectionType != ProtectionType.AllowOnlyFormFields)
      this.DispFormFldSel = false;
    if (!this.ProtEnabled)
      this.FormNoFields = false;
    BaseWordRecord.WriteUInt32(stream, this.m_flagA);
    this.Copts60.Write(stream);
    BaseWordRecord.WriteUInt16(stream, this.m_dxaTabs);
    BaseWordRecord.WriteUInt16(stream, (ushort) this.m_wSpare);
    BaseWordRecord.WriteUInt16(stream, (ushort) this.m_dxaHotZ);
    BaseWordRecord.WriteUInt16(stream, (ushort) this.m_cConsecHypLim);
    BaseWordRecord.WriteUInt16(stream, (ushort) this.m_wSpare2);
    BaseWordRecord.WriteUInt32(stream, this.SetDateTime(this.m_created));
    BaseWordRecord.WriteUInt32(stream, this.SetDateTime(this.m_revised));
    if (this.m_lastPrinted != DateTime.MinValue)
      BaseWordRecord.WriteUInt32(stream, this.SetDateTime(this.m_lastPrinted));
    else
      BaseWordRecord.WriteInt32(stream, 0);
    BaseWordRecord.WriteInt16(stream, (short) this.m_nRevision);
    if (this.m_editTime != TimeSpan.MinValue)
      this.m_tmEdited = (int) this.m_editTime.TotalMinutes;
    BaseWordRecord.WriteInt32(stream, this.m_tmEdited);
    BaseWordRecord.WriteInt32(stream, this.m_cWords);
    BaseWordRecord.WriteInt32(stream, this.m_cCh);
    BaseWordRecord.WriteInt16(stream, (short) this.m_cPg);
    BaseWordRecord.WriteInt32(stream, this.m_cParas);
    this.m_End = (ushort) 0;
    this.m_End |= (ushort) this.m_rncEdn;
    this.m_End |= (ushort) (this.m_nEdn << 2);
    BaseWordRecord.WriteUInt16(stream, this.m_End);
    this.m_epc = (ushort) BaseWordRecord.SetBitsByMask((int) this.m_epc, 3, (int) this.m_bepc);
    this.m_epc = (ushort) BaseWordRecord.SetBitsByMask((int) this.m_epc, 60, (int) this.m_nfcFtnRef);
    this.m_epc = (ushort) BaseWordRecord.SetBitsByMask((int) this.m_epc, 960, (int) this.m_nfcEdnRef);
    if (this.m_shadeFormData)
      this.m_epc |= (ushort) 4096 /*0x1000*/;
    else
      this.m_epc = (ushort) BaseWordRecord.SetBitsByMask((int) this.m_epc, 4096 /*0x1000*/, 0);
    BaseWordRecord.WriteUInt16(stream, this.m_epc);
    BaseWordRecord.WriteInt32(stream, this.m_cLines);
    BaseWordRecord.WriteInt32(stream, this.m_wordsFtnEnd);
    BaseWordRecord.WriteInt32(stream, this.m_cChFtnEdn);
    BaseWordRecord.WriteInt16(stream, (short) this.m_cPgFtnEdn);
    BaseWordRecord.WriteInt32(stream, this.m_cParasFrnEdn);
    BaseWordRecord.WriteInt32(stream, this.m_cLinesFtnEdn);
    BaseWordRecord.WriteUInt32(stream, this.m_lKeyProtDoc);
    if (this.GutterAtTop)
      this.m_wvkSaved |= (ushort) 32768 /*0x8000*/;
    BaseWordRecord.WriteUInt16(stream, this.m_wvkSaved);
    this.Dop95.Write(stream);
    this.Dop97.Write(stream);
    this.Dop2000.Write(stream);
    this.Dop2002.Write(stream);
    this.Dop2003.Write(stream);
    this.Dop2007.Write(stream);
    if (this.m_dopLeftData != null)
      stream.Write(this.m_dopLeftData, 0, this.m_dopLeftData.Length);
    return (uint) (stream.Position - position);
  }

  internal DOPDescriptor Clone() => (DOPDescriptor) this.MemberwiseClone();

  internal void SetProtection(ProtectionType type, string password)
  {
    this.ProtectionType = type;
    if (string.IsNullOrEmpty(password))
    {
      this.m_lKeyProtDoc = 0U;
    }
    else
    {
      if (type == ProtectionType.NoProtection)
        return;
      this.m_lKeyProtDoc = WordDecryptor.GetPasswordHash(password);
    }
  }

  private DateTime ParseDateTime(uint dateTime)
  {
    if (dateTime == 0U)
      return DateTime.MinValue;
    ushort num1 = (ushort) (dateTime & (uint) ushort.MaxValue);
    ushort num2 = (ushort) ((dateTime & 4294901760U) >> 16 /*0x10*/);
    int minute = (int) num1 & 63 /*0x3F*/;
    if (minute < 0 || minute > 59)
      return DateTime.Now;
    int hour = ((int) num1 & 1984) >> 6;
    if (hour < 0 || hour > 23)
      return DateTime.Now;
    int day = ((int) num1 & 63488) >> 11;
    if (day < 1 || day > 31 /*0x1F*/)
      return DateTime.Now;
    int month = (int) num2 & 15;
    if (month < 1 || month > 12)
      return DateTime.Now;
    int year = (((int) num2 & 8176) >> 4) + 1900;
    return year < 1900 || year > 2411 ? DateTime.Now : new DateTime(year, month, day, hour, minute, 0, 0);
  }

  private uint SetDateTime(DateTime dt)
  {
    return (uint) dt.Minute | (uint) (dt.Hour << 6) | (uint) (dt.Day << 11) | (uint) (dt.Month << 16 /*0x10*/) | (uint) (dt.Year - 1900 << 20) | this.ConvertDayOfWeek(dt.DayOfWeek) << 29;
  }

  private uint ConvertDayOfWeek(DayOfWeek dow)
  {
    switch (dow)
    {
      case DayOfWeek.Monday:
        return 1;
      case DayOfWeek.Tuesday:
        return 2;
      case DayOfWeek.Wednesday:
        return 3;
      case DayOfWeek.Thursday:
        return 4;
      case DayOfWeek.Friday:
        return 5;
      case DayOfWeek.Saturday:
        return 6;
      default:
        return 0;
    }
  }

  [CLSCompliant(false)]
  internal static ushort GetPasswordHash(string password)
  {
    if (password == null)
      throw new ArgumentNullException(nameof (password));
    if (password.Length > 15)
      throw new ArgumentOutOfRangeException("Length of the password can't be more than " + 15.ToString());
    ushort num = 0;
    int index = 0;
    for (int length = password.Length; index < length; ++index)
    {
      ushort uint16FromBits = DOPDescriptor.GetUInt16FromBits(DOPDescriptor.RotateBits(DOPDescriptor.GetCharBits15(password[index]), index + 1));
      num ^= uint16FromBits;
    }
    return (ushort) ((int) num ^ password.Length ^ 52811);
  }

  private static bool[] GetCharBits15(char charToConvert)
  {
    bool[] charBits15 = new bool[15];
    ushort uint16 = Convert.ToUInt16(charToConvert);
    ushort num = 1;
    for (int index = 0; index < 15; ++index)
    {
      charBits15[index] = ((int) uint16 & (int) num) == (int) num;
      num <<= 1;
    }
    return charBits15;
  }

  private static ushort GetUInt16FromBits(bool[] bits)
  {
    if (bits == null)
      throw new ArgumentNullException(nameof (bits));
    if (bits.Length > 16 /*0x10*/)
      throw new ArgumentOutOfRangeException("There can't be more than 16 bits");
    ushort uint16FromBits = 0;
    ushort num = 1;
    int index = 0;
    for (int length = bits.Length; index < length; ++index)
    {
      if (bits[index])
        uint16FromBits += num;
      num <<= 1;
    }
    return uint16FromBits;
  }

  private static bool[] RotateBits(bool[] bits, int count)
  {
    if (bits == null)
      throw new ArgumentNullException(nameof (bits));
    if (bits.Length == 0)
      return bits;
    if (count < 0)
      throw new ArgumentOutOfRangeException("count can't be less than zero");
    bool[] flagArray = new bool[bits.Length];
    int index1 = 0;
    for (int length = bits.Length; index1 < length; ++index1)
    {
      int index2 = (index1 + count) % length;
      flagArray[index2] = bits[index1];
    }
    return flagArray;
  }

  internal static int Round(int value, int degree)
  {
    if (degree == 0)
      throw new ArgumentOutOfRangeException("degree can't be 0");
    int num = value % degree;
    return value - num + degree;
  }
}
