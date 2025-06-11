// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher._MSOFBH
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using Syncfusion.DocIO.ReaderWriter.Escher;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;

internal class _MSOFBH : BaseWordRecord
{
  internal const int DEF_ISCONTAINER = 15;
  private int m_version;
  private int m_instance;
  private int m_type;
  private int m_length;
  internal WordDocument m_doc;

  internal int Instance
  {
    get => this.m_instance;
    set => this.m_instance = value;
  }

  internal bool IsContainer
  {
    get => this.m_version == 15;
    set
    {
      if (!value)
        return;
      this.m_version = 15;
    }
  }

  internal new int Length
  {
    get => this.m_length;
    set => this.m_length = value;
  }

  internal MSOFBT Type
  {
    get => (MSOFBT) this.m_type;
    set => this.m_type = (int) value;
  }

  internal int Version
  {
    get => this.m_version;
    set => this.m_version = value;
  }

  internal _MSOFBH(WordDocument doc) => this.m_doc = doc;

  internal _MSOFBH(Stream stream, WordDocument doc)
  {
    this.m_doc = doc;
    this.Read(stream);
  }

  internal void Read(Stream stream)
  {
    int num = BaseWordRecord.ReadInt32(stream);
    this.m_version = num & 15;
    this.m_instance = (num & 65520) >> 4;
    this.m_type = (int) (((long) num & 4294901760L) >> 16 /*0x10*/);
    this.m_length = BaseWordRecord.ReadInt32(stream);
  }

  internal void Write(Stream stream)
  {
    int num = 0 | this.m_version | this.m_instance << 4 | this.m_type << 16 /*0x10*/;
    BaseWordRecord.WriteInt32(stream, num);
    BaseWordRecord.WriteInt32(stream, this.m_length);
  }

  internal BaseEscherRecord CreateRecordFromHeader()
  {
    switch (this.Type)
    {
      case MSOFBT.msofbtDggContainer:
        return (BaseEscherRecord) new MsofbtDggContainer(this.m_doc);
      case MSOFBT.msofbtBstoreContainer:
        return (BaseEscherRecord) new MsofbtBstoreContainer(this.m_doc);
      case MSOFBT.msofbtDgContainer:
        return (BaseEscherRecord) new MsofbtDgContainer(this.m_doc);
      case MSOFBT.msofbtSpgrContainer:
        return (BaseEscherRecord) new MsofbtSpgrContainer(this.m_doc);
      case MSOFBT.msofbtSpContainer:
        return (BaseEscherRecord) new MsofbtSpContainer(this.m_doc);
      case MSOFBT.msofbtSolverContainer:
        return (BaseEscherRecord) new MsofbtSolverContainer(this.m_doc);
      case MSOFBT.msofbtDgg:
        return (BaseEscherRecord) new MsofbtDgg(this.m_doc);
      case MSOFBT.msofbtBSE:
        return (BaseEscherRecord) new MsofbtBSE(this.m_doc);
      case MSOFBT.msofbtDg:
        return (BaseEscherRecord) new MsofbtDg(this.m_doc);
      case MSOFBT.msofbtSpgr:
        return (BaseEscherRecord) new MsofbtSpgr(this.m_doc);
      case MSOFBT.msofbtSp:
        return (BaseEscherRecord) new MsofbtSp(this.m_doc);
      case MSOFBT.msofbtOPT:
        return (BaseEscherRecord) new MsofbtOPT(this.m_doc);
      case MSOFBT.msofbtClientTextbox:
        return (BaseEscherRecord) new MsofbtClientTextbox(this.m_doc);
      case MSOFBT.msofbtClientAnchor:
        return (BaseEscherRecord) new MsofbtClientAnchor(this.m_doc);
      case MSOFBT.msofbtClientData:
        return (BaseEscherRecord) new MsofbtClientData(this.m_doc);
      case MSOFBT.msofbtBlipEMF:
      case MSOFBT.msofbtBlipWMF:
        return (BaseEscherRecord) new MsofbtMetaFile(this.m_doc);
      case MSOFBT.msofbtBlipJPEG:
      case MSOFBT.msofbtBlipPNG:
      case MSOFBT.msofbtBlipDIB:
        return (BaseEscherRecord) new MsofbtImage(this.m_doc);
      case MSOFBT.msofbtREGROUPItems:
        return (BaseEscherRecord) new MsofbtGeneral(this.m_doc);
      case MSOFBT.msofbtSecondaryFOPT:
        return (BaseEscherRecord) new MsofbtSecondaryFOPT(this.m_doc);
      case MSOFBT.msofbtTertiaryFOPT:
        return (BaseEscherRecord) new MsofbtTertiaryFOPT(this.m_doc);
      default:
        return this.IsContainer ? (BaseEscherRecord) new BaseContainer(this.m_doc) : (BaseEscherRecord) new MsofbtGeneral(this.m_doc);
    }
  }

  internal _MSOFBH Clone()
  {
    _MSOFBH msofbh = (_MSOFBH) this.MemberwiseClone();
    msofbh.m_doc = this.m_doc;
    return msofbh;
  }

  internal static BaseEscherRecord ReadHeaderWithRecord(Stream stream, WordDocument doc)
  {
    _MSOFBH msofbh = new _MSOFBH(stream, doc);
    BaseEscherRecord recordFromHeader = msofbh.CreateRecordFromHeader();
    return !recordFromHeader.ReadRecord(msofbh, stream) ? (BaseEscherRecord) null : recordFromHeader;
  }
}
