// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing.MsofbtSplitMenuColors
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing;

[CLSCompliant(false)]
[Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing.MsoDrawing(MsoRecords.msofbtSplitMenuColors)]
internal class MsofbtSplitMenuColors : MsoBase
{
  private const int RecordSize = 16 /*0x10*/;
  [BiffRecordPos(0, 4, true)]
  private int m_iFillColor;
  [BiffRecordPos(4, 4, true)]
  private int m_iLineColor;
  [BiffRecordPos(8, 4, true)]
  private int m_iShadowColor;
  [BiffRecordPos(12, 4, true)]
  private int m_i3DColor;

  public int FillColor
  {
    get => this.m_iFillColor;
    set => this.m_iFillColor = value;
  }

  public int LineColor
  {
    get => this.m_iLineColor;
    set => this.m_iLineColor = value;
  }

  public int ShadowColor
  {
    get => this.m_iShadowColor;
    set => this.m_iShadowColor = value;
  }

  public int Color3D
  {
    get => this.m_i3DColor;
    set => this.m_i3DColor = value;
  }

  public MsofbtSplitMenuColors(MsoBase parent)
    : base(parent)
  {
  }

  public MsofbtSplitMenuColors(MsoBase parent, byte[] data, int iOffset)
    : base(parent, data, iOffset)
  {
  }

  public override void InfillInternalData(
    Stream stream,
    int iOffset,
    List<int> arrBreaks,
    List<List<BiffRecordRaw>> arrRecords)
  {
    MsoBase.WriteInt32(stream, this.m_iFillColor);
    MsoBase.WriteInt32(stream, this.m_iLineColor);
    MsoBase.WriteInt32(stream, this.m_iShadowColor);
    MsoBase.WriteInt32(stream, this.m_i3DColor);
    this.m_iLength = 16 /*0x10*/;
  }

  public override void ParseStructure(Stream stream)
  {
    this.m_iFillColor = MsoBase.ReadInt32(stream);
    this.m_iLineColor = MsoBase.ReadInt32(stream);
    this.m_iShadowColor = MsoBase.ReadInt32(stream);
    this.m_i3DColor = MsoBase.ReadInt32(stream);
  }
}
