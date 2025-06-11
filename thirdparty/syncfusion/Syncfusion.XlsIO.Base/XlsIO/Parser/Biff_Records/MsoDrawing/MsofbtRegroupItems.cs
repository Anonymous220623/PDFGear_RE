// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing.MsofbtRegroupItems
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing;

[Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing.MsoDrawing(MsoRecords.msofbtRegroupItems)]
[CLSCompliant(false)]
public class MsofbtRegroupItems : MsoBase
{
  private byte[] m_arrData;

  public MsofbtRegroupItems(MsoBase parent)
    : base(parent)
  {
  }

  public MsofbtRegroupItems(MsoBase parent, byte[] data, int iOffset)
    : base(parent, data, iOffset)
  {
  }

  public override void InfillInternalData(
    Stream stream,
    int iOffset,
    List<int> arrBreaks,
    List<List<BiffRecordRaw>> arrRecords)
  {
    this.m_iLength = this.m_arrData != null ? this.m_arrData.Length : 0;
    if (this.m_iLength <= 0)
      return;
    stream.Write(this.m_arrData, 0, this.m_iLength);
  }

  public override void ParseStructure(Stream stream)
  {
    if (this.m_iLength <= 0)
      return;
    this.m_arrData = new byte[this.m_iLength];
    stream.Read(this.m_arrData, 0, this.m_iLength);
  }
}
