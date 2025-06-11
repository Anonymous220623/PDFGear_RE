// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing.MsofbtDgContainer
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation;
using System;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing;

[Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing.MsoDrawing(MsoRecords.msofbtDgContainer)]
[CLSCompliant(false)]
internal class MsofbtDgContainer : MsoContainerBase
{
  private const int DEF_VERSION = 15;

  public MsofbtDgContainer(MsoBase parent)
    : base(parent)
  {
    this.Version = 15;
  }

  public MsofbtDgContainer(MsoBase parent, byte[] data, int iOffset)
    : base(parent, data, iOffset)
  {
  }

  public MsofbtDgContainer(
    MsoBase parent,
    byte[] data,
    int iOffset,
    GetNextMsoDrawingData dataGetter)
    : base(parent, data, iOffset, dataGetter)
  {
  }
}
