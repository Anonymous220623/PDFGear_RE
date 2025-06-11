// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing.MsofbtAnchor
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing;

[Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing.MsoDrawing(MsoRecords.msofbtAnchor)]
[CLSCompliant(false)]
internal class MsofbtAnchor : MsoBase
{
  private const int DEF_RECORD_SIZE = 16 /*0x10*/;
  [BiffRecordPos(0, 4, true)]
  private int m_iLeft;
  [BiffRecordPos(4, 4, true)]
  private int m_iTop;
  [BiffRecordPos(8, 4, true)]
  private int m_iRight;
  [BiffRecordPos(12, 4, true)]
  private int m_iBottom;

  public int Left
  {
    get => this.m_iLeft;
    set => this.m_iLeft = value;
  }

  public int Top
  {
    get => this.m_iTop;
    set => this.m_iTop = value;
  }

  public int Right
  {
    get => this.m_iRight;
    set => this.m_iRight = value;
  }

  public int Bottom
  {
    get => this.m_iBottom;
    set => this.m_iBottom = value;
  }

  public MsofbtAnchor(MsoBase parent)
    : base(parent)
  {
  }

  public MsofbtAnchor(MsoBase parent, byte[] data, int iOffset)
    : base(parent, data, iOffset)
  {
  }

  public override void InfillInternalData(
    Stream stream,
    int iOffset,
    List<int> arrBreaks,
    List<List<BiffRecordRaw>> arrRecords)
  {
    this.m_iLength = 16 /*0x10*/;
    MsoBase.WriteInt32(stream, this.m_iLeft);
    MsoBase.WriteInt32(stream, this.m_iTop);
    MsoBase.WriteInt32(stream, this.m_iRight);
    MsoBase.WriteInt32(stream, this.m_iBottom);
  }

  public override void ParseStructure(Stream stream)
  {
    this.m_iLeft = MsoBase.ReadInt32(stream);
    this.m_iTop = MsoBase.ReadInt32(stream);
    this.m_iRight = MsoBase.ReadInt32(stream);
    this.m_iBottom = MsoBase.ReadInt32(stream);
  }

  public override int GetStoreSize(OfficeVersion version) => 16 /*0x10*/;
}
