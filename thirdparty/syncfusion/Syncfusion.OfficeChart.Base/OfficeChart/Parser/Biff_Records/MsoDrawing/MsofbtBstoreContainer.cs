// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing.MsofbtBstoreContainer
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing;

[Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing.MsoDrawing(MsoRecords.msofbtBstoreContainer)]
[CLSCompliant(false)]
internal class MsofbtBstoreContainer : MsoContainerBase
{
  private const int DEF_VERSION = 15;
  private const int DEF_INSTANCE = 1;

  public MsofbtBstoreContainer(MsoBase parent)
    : base(parent)
  {
    this.Version = 15;
    this.Instance = 1;
  }

  public MsofbtBstoreContainer(MsoBase parent, byte[] data, int iOffset)
    : base(parent, data, iOffset)
  {
  }

  public MsofbtBstoreContainer(
    MsoBase parent,
    byte[] data,
    int iOffset,
    GetNextMsoDrawingData dataGetter)
    : base(parent, data, iOffset, dataGetter)
  {
  }

  protected override void OnDispose()
  {
    if (this.Items.Length > 0)
    {
      int length = this.Items.Length;
      for (int index = 0; index < length; ++index)
        this.Items[index].Dispose();
    }
    base.OnDispose();
  }

  public override void InfillInternalData(
    Stream stream,
    int iOffset,
    List<int> arrBreaks,
    List<List<BiffRecordRaw>> arrRecords)
  {
    this.Instance = this.Items.Length;
    base.InfillInternalData(stream, iOffset, arrBreaks, arrRecords);
  }
}
