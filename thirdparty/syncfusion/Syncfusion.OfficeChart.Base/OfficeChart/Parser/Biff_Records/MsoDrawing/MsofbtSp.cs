// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing.MsofbtSp
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing;

[CLSCompliant(false)]
[Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing.MsoDrawing(MsoRecords.msofbtSp)]
internal class MsofbtSp : MsoBase
{
  private const int DEF_VERSION = 2;
  private const int DEF_RECORD_SIZE = 8;
  [BiffRecordPos(0, 4, true)]
  private int m_iShapeId;
  [BiffRecordPos(4, 4)]
  private uint m_uiOptions;
  [BiffRecordPos(4, 0, TFieldType.Bit)]
  private bool m_bGroup;
  [BiffRecordPos(4, 1, TFieldType.Bit)]
  private bool m_bChild;
  [BiffRecordPos(4, 2, TFieldType.Bit)]
  private bool m_bPatriarch;
  [BiffRecordPos(4, 3, TFieldType.Bit)]
  private bool m_bDeleted;
  [BiffRecordPos(4, 4, TFieldType.Bit)]
  private bool m_bOleShape;
  [BiffRecordPos(4, 5, TFieldType.Bit)]
  private bool m_bHaveMaster;
  [BiffRecordPos(4, 6, TFieldType.Bit)]
  private bool m_bFlipH;
  [BiffRecordPos(4, 7, TFieldType.Bit)]
  private bool m_bFlipV;
  [BiffRecordPos(5, 0, TFieldType.Bit)]
  private bool m_bConnector;
  [BiffRecordPos(5, 1, TFieldType.Bit)]
  private bool m_bHaveAnchor;
  [BiffRecordPos(5, 2, TFieldType.Bit)]
  private bool m_bBackground;
  [BiffRecordPos(5, 3, TFieldType.Bit)]
  private bool m_bHaveSpt;

  public int ShapeId
  {
    get => this.m_iShapeId;
    set => this.m_iShapeId = value;
  }

  public uint Options => this.m_uiOptions;

  public bool IsGroup
  {
    get => this.m_bGroup;
    set => this.m_bGroup = value;
  }

  public bool IsChild
  {
    get => this.m_bChild;
    set => this.m_bChild = value;
  }

  public bool IsPatriarch
  {
    get => this.m_bPatriarch;
    set => this.m_bPatriarch = value;
  }

  public bool IsDeleted
  {
    get => this.m_bDeleted;
    set => this.m_bDeleted = value;
  }

  public bool IsOleShape
  {
    get => this.m_bOleShape;
    set => this.m_bOleShape = value;
  }

  public bool IsHaveMaster
  {
    get => this.m_bHaveMaster;
    set => this.m_bHaveMaster = value;
  }

  public bool IsFlipH
  {
    get => this.m_bFlipH;
    set => this.m_bFlipH = value;
  }

  public bool IsFlipV
  {
    get => this.m_bFlipV;
    set => this.m_bFlipV = value;
  }

  public bool IsConnector
  {
    get => this.m_bConnector;
    set => this.m_bConnector = value;
  }

  public bool IsHaveAnchor
  {
    get => this.m_bHaveAnchor;
    set => this.m_bHaveAnchor = value;
  }

  public bool IsBackground
  {
    get => this.m_bBackground;
    set => this.m_bBackground = value;
  }

  public bool IsHaveSpt
  {
    get => this.m_bHaveSpt;
    set => this.m_bHaveSpt = value;
  }

  public MsofbtSp(MsoBase parent)
    : base(parent)
  {
    this.Version = 2;
  }

  public MsofbtSp(MsoBase parent, byte[] data, int iOffset)
    : base(parent, data, iOffset)
  {
  }

  public override void InfillInternalData(
    Stream stream,
    int iOffset,
    List<int> arrBreaks,
    List<List<BiffRecordRaw>> arrRecords)
  {
    MsoBase.WriteInt32(stream, this.m_iShapeId);
    this.SetBitInVar(ref this.m_uiOptions, this.m_bGroup, 0);
    this.SetBitInVar(ref this.m_uiOptions, this.m_bChild, 1);
    this.SetBitInVar(ref this.m_uiOptions, this.m_bPatriarch, 2);
    this.SetBitInVar(ref this.m_uiOptions, this.m_bDeleted, 3);
    this.SetBitInVar(ref this.m_uiOptions, this.m_bOleShape, 4);
    this.SetBitInVar(ref this.m_uiOptions, this.m_bHaveMaster, 5);
    this.SetBitInVar(ref this.m_uiOptions, this.m_bFlipH, 6);
    this.SetBitInVar(ref this.m_uiOptions, this.m_bFlipV, 7);
    this.SetBitInVar(ref this.m_uiOptions, this.m_bConnector, 8);
    this.SetBitInVar(ref this.m_uiOptions, this.m_bHaveAnchor, 9);
    this.SetBitInVar(ref this.m_uiOptions, this.m_bBackground, 10);
    this.SetBitInVar(ref this.m_uiOptions, this.m_bHaveSpt, 11);
    MsoBase.WriteUInt32(stream, this.m_uiOptions);
    this.m_iLength = 8;
  }

  public override void ParseStructure(Stream stream)
  {
    this.m_iShapeId = MsoBase.ReadInt32(stream);
    this.m_uiOptions = MsoBase.ReadUInt32(stream);
    this.m_bGroup = BiffRecordRaw.GetBitFromVar(this.m_uiOptions, 0);
    this.m_bChild = BiffRecordRaw.GetBitFromVar(this.m_uiOptions, 1);
    this.m_bPatriarch = BiffRecordRaw.GetBitFromVar(this.m_uiOptions, 2);
    this.m_bDeleted = BiffRecordRaw.GetBitFromVar(this.m_uiOptions, 3);
    this.m_bOleShape = BiffRecordRaw.GetBitFromVar(this.m_uiOptions, 4);
    this.m_bHaveMaster = BiffRecordRaw.GetBitFromVar(this.m_uiOptions, 5);
    this.m_bFlipH = BiffRecordRaw.GetBitFromVar(this.m_uiOptions, 6);
    this.m_bFlipV = BiffRecordRaw.GetBitFromVar(this.m_uiOptions, 7);
    this.m_bConnector = BiffRecordRaw.GetBitFromVar(this.m_uiOptions, 8);
    this.m_bHaveAnchor = BiffRecordRaw.GetBitFromVar(this.m_uiOptions, 9);
    this.m_bBackground = BiffRecordRaw.GetBitFromVar(this.m_uiOptions, 10);
    this.m_bHaveSpt = BiffRecordRaw.GetBitFromVar(this.m_uiOptions, 11);
  }

  public override int GetStoreSize(OfficeVersion version) => 8;
}
