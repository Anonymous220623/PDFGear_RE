// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.MsofbtSp
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using Syncfusion.DocIO.ReaderWriter.Escher;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;

internal class MsofbtSp : BaseEscherRecord
{
  private int m_shapeId;
  private int m_shapeFlags;

  internal bool IsGroup
  {
    get => BaseWordRecord.GetBitsByMask(this.m_shapeFlags, 1, 0) == 1;
    set => this.m_shapeFlags = BaseWordRecord.SetBitsByMask(this.m_shapeFlags, 1, 0, value ? 1 : 0);
  }

  internal bool IsChild
  {
    get => BaseWordRecord.GetBitsByMask(this.m_shapeFlags, 2, 1) == 1;
    set => this.m_shapeFlags = BaseWordRecord.SetBitsByMask(this.m_shapeFlags, 2, 1, value ? 1 : 0);
  }

  internal bool IsPatriarch
  {
    get => BaseWordRecord.GetBitsByMask(this.m_shapeFlags, 4, 2) == 1;
    set => this.m_shapeFlags = BaseWordRecord.SetBitsByMask(this.m_shapeFlags, 4, 2, value ? 1 : 0);
  }

  internal bool IsDeleted
  {
    get => BaseWordRecord.GetBitsByMask(this.m_shapeFlags, 8, 3) == 1;
    set => this.m_shapeFlags = BaseWordRecord.SetBitsByMask(this.m_shapeFlags, 8, 3, value ? 1 : 0);
  }

  internal bool IsOle
  {
    get => BaseWordRecord.GetBitsByMask(this.m_shapeFlags, 16 /*0x10*/, 4) == 1;
    set
    {
      this.m_shapeFlags = BaseWordRecord.SetBitsByMask(this.m_shapeFlags, 16 /*0x10*/, 4, value ? 1 : 0);
    }
  }

  internal bool HasMaster
  {
    get => BaseWordRecord.GetBitsByMask(this.m_shapeFlags, 32 /*0x20*/, 5) == 1;
    set
    {
      this.m_shapeFlags = BaseWordRecord.SetBitsByMask(this.m_shapeFlags, 32 /*0x20*/, 5, value ? 1 : 0);
    }
  }

  internal bool IsFlippedHor
  {
    get => BaseWordRecord.GetBitsByMask(this.m_shapeFlags, 64 /*0x40*/, 6) == 1;
    set
    {
      this.m_shapeFlags = BaseWordRecord.SetBitsByMask(this.m_shapeFlags, 64 /*0x40*/, 6, value ? 1 : 0);
    }
  }

  internal bool IsFlippedVert
  {
    get => BaseWordRecord.GetBitsByMask(this.m_shapeFlags, 128 /*0x80*/, 7) == 1;
    set
    {
      this.m_shapeFlags = BaseWordRecord.SetBitsByMask(this.m_shapeFlags, 128 /*0x80*/, 7, value ? 1 : 0);
    }
  }

  internal bool IsConnector
  {
    get => BaseWordRecord.GetBitsByMask(this.m_shapeFlags, 256 /*0x0100*/, 8) == 1;
    set
    {
      this.m_shapeFlags = BaseWordRecord.SetBitsByMask(this.m_shapeFlags, 256 /*0x0100*/, 8, value ? 1 : 0);
    }
  }

  internal bool HasAnchor
  {
    get => BaseWordRecord.GetBitsByMask(this.m_shapeFlags, 512 /*0x0200*/, 9) == 1;
    set
    {
      this.m_shapeFlags = BaseWordRecord.SetBitsByMask(this.m_shapeFlags, 512 /*0x0200*/, 9, value ? 1 : 0);
    }
  }

  internal bool IsBackground
  {
    get => BaseWordRecord.GetBitsByMask(this.m_shapeFlags, 1024 /*0x0400*/, 10) == 1;
    set
    {
      this.m_shapeFlags = BaseWordRecord.SetBitsByMask(this.m_shapeFlags, 1024 /*0x0400*/, 10, value ? 1 : 0);
    }
  }

  internal bool HasShapeTypeProperty
  {
    get => BaseWordRecord.GetBitsByMask(this.m_shapeFlags, 2048 /*0x0800*/, 11) == 1;
    set
    {
      this.m_shapeFlags = BaseWordRecord.SetBitsByMask(this.m_shapeFlags, 2048 /*0x0800*/, 11, value ? 1 : 0);
    }
  }

  internal int ShapeId
  {
    get => this.m_shapeId;
    set => this.m_shapeId = value;
  }

  internal EscherShapeType ShapeType
  {
    get => (EscherShapeType) this.Header.Instance;
    set => this.Header.Instance = (int) value;
  }

  internal MsofbtSp(WordDocument doc)
    : base(MSOFBT.msofbtSp, 2, doc)
  {
  }

  protected override void ReadRecordData(Stream stream)
  {
    this.m_shapeId = BaseWordRecord.ReadInt32(stream);
    this.m_shapeFlags = BaseWordRecord.ReadInt32(stream);
  }

  protected override void WriteRecordData(Stream stream)
  {
    BaseWordRecord.WriteInt32(stream, this.m_shapeId);
    BaseWordRecord.WriteInt32(stream, this.m_shapeFlags);
  }

  internal override BaseEscherRecord Clone()
  {
    MsofbtSp msofbtSp = (MsofbtSp) this.MemberwiseClone();
    msofbtSp.m_doc = this.m_doc;
    return (BaseEscherRecord) msofbtSp;
  }
}
