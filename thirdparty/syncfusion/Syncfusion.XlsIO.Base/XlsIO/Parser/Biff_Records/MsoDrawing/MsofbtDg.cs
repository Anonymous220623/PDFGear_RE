// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing.MsofbtDg
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing;

[Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing.MsoDrawing(MsoRecords.msofbtDg)]
[CLSCompliant(false)]
public class MsofbtDg : MsoBase
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

  public override int GetStoreSize(ExcelVersion version) => 8;
}
