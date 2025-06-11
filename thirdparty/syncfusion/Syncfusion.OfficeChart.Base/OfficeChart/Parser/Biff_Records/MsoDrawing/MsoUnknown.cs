// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing.MsoUnknown
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing;

[CLSCompliant(false)]
[Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing.MsoDrawing(MsoRecords.msoUnknown)]
internal class MsoUnknown : MsoBase
{
  public MsoUnknown(MsoBase parent)
    : base(parent)
  {
  }

  public MsoUnknown(MsoBase parent, byte[] data, int iOffset)
    : base(parent, data, iOffset)
  {
  }

  public override void ParseStructure(Stream stream)
  {
    if (this.m_iLength <= 0)
      return;
    this.m_data = new byte[this.m_iLength];
    stream.Read(this.m_data, 0, this.m_iLength);
  }

  public override void InfillInternalData(
    Stream stream,
    int iOffset,
    List<int> arrBreaks,
    List<List<BiffRecordRaw>> arrRecords)
  {
    if (this.m_iLength <= 0)
      return;
    stream.Write(this.m_data, 0, this.m_iLength);
  }

  public override bool NeedDataArray => true;
}
