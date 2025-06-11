// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing.MsofbtDg
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing;

[Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing.MsoDrawing(MsoRecords.msofbtDg)]
[CLSCompliant(false)]
internal class MsofbtDg : MsoBase
{
  private const int DEF_INSTANCE = 1;
  private const int DEF_RECORD_SIZE = 8;
  [BiffRecordPos(0, 4)]
  private uint m_uiShapesNumber;
  [BiffRecordPos(4, 4)]
  private int m_iLastId;

  public MsofbtDg(MsoBase parent)
    : base(parent)
  {
    this.Instance = 1;
  }

  public MsofbtDg(MsoBase parent, byte[] data, int iOffset)
    : base(parent, data, iOffset)
  {
  }

  public uint ShapesNumber
  {
    get => this.m_uiShapesNumber;
    set => this.m_uiShapesNumber = value;
  }

  public int LastId
  {
    get => this.m_iLastId;
    set => this.m_iLastId = value;
  }

  public override void InfillInternalData(
    Stream stream,
    int iOffset,
    List<int> arrBreaks,
    List<List<BiffRecordRaw>> arrRecords)
  {
    this.m_iLength = 8;
    MsoBase.WriteUInt32(stream, this.m_uiShapesNumber);
    MsoBase.WriteInt32(stream, this.m_iLastId);
  }

  public override void ParseStructure(Stream stream)
  {
    this.m_uiShapesNumber = MsoBase.ReadUInt32(stream);
    this.m_iLastId = MsoBase.ReadInt32(stream);
  }

  public override int GetStoreSize(OfficeVersion version) => 8;
}
