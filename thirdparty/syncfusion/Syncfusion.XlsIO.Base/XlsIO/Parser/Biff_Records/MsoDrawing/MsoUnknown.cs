// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing.MsoUnknown
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing;

[CLSCompliant(false)]
[Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing.MsoDrawing(MsoRecords.msoUnknown)]
public class MsoUnknown : MsoBase
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
