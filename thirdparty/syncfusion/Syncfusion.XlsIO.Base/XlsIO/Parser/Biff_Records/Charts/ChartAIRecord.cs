// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartAIRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using Syncfusion.XlsIO.Parser.Biff_Records.Formula;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.ChartAI)]
[CLSCompliant(false)]
public class ChartAIRecord : BiffRecordRaw
{
  [BiffRecordPos(0, 1)]
  private byte m_id;
  [BiffRecordPos(1, 1)]
  private byte m_ReferenceType;
  [BiffRecordPos(2, 2)]
  private ushort m_usOptions;
  [BiffRecordPos(4, 2)]
  private ushort m_usNumIndex;
  [BiffRecordPos(6, 2)]
  private ushort m_usFormulaSize;
  [BiffRecordPos(2, 1, TFieldType.Bit)]
  private bool m_bCustomNumberFormat;
  private Ptg[] m_arrExpression;

  public ChartAIRecord.LinkIndex IndexIdentifier
  {
    get => (ChartAIRecord.LinkIndex) this.m_id;
    set => this.m_id = (byte) value;
  }

  public ChartAIRecord.ReferenceType Reference
  {
    get => (ChartAIRecord.ReferenceType) this.m_ReferenceType;
    set => this.m_ReferenceType = (byte) value;
  }

  public ushort Options => this.m_usOptions;

  public ushort NumberFormatIndex
  {
    get => this.m_usNumIndex;
    set
    {
      if ((int) value == (int) this.m_usNumIndex)
        return;
      this.m_usNumIndex = value;
    }
  }

  public ushort FormulaSize
  {
    get => this.m_usFormulaSize;
    set
    {
      if ((int) value == (int) this.m_usFormulaSize)
        return;
      this.m_usFormulaSize = value;
    }
  }

  public bool IsCustomNumberFormat
  {
    get => this.m_bCustomNumberFormat;
    set => this.m_bCustomNumberFormat = value;
  }

  public Ptg[] ParsedExpression
  {
    get => this.m_arrExpression;
    set => this.m_arrExpression = value;
  }

  public ChartAIRecord()
  {
  }

  public ChartAIRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartAIRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override int GetStoreSize(ExcelVersion version)
  {
    int storeSize = 8;
    if (this.m_arrExpression != null)
    {
      int index = 0;
      for (int length = this.m_arrExpression.Length; index < length; ++index)
      {
        Ptg ptg = this.m_arrExpression[index];
        storeSize += ptg.GetSize(version);
        if (ptg is IAdditionalData additionalData)
          storeSize += additionalData.AdditionalDataSize;
      }
    }
    return storeSize;
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_id = provider.ReadByte(iOffset);
    ++iOffset;
    this.m_ReferenceType = provider.ReadByte(iOffset);
    ++iOffset;
    this.m_usOptions = provider.ReadUInt16(iOffset);
    this.m_bCustomNumberFormat = provider.ReadBit(iOffset, 0);
    iOffset += 2;
    this.m_usNumIndex = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usFormulaSize = provider.ReadUInt16(iOffset);
    iOffset += 2;
    if (this.m_usFormulaSize > (ushort) 0)
      this.m_arrExpression = FormulaUtil.ParseExpression(provider, iOffset, (int) this.m_usFormulaSize, out int _, version);
    else
      this.m_arrExpression = (Ptg[]) null;
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_usOptions &= (ushort) 1;
    this.m_usFormulaSize = (ushort) 0;
    byte[] numArray = (byte[]) null;
    if (this.m_arrExpression != null && this.m_arrExpression.Length > 0)
    {
      numArray = FormulaUtil.PtgArrayToByteArray(this.m_arrExpression, version);
      this.m_usFormulaSize = (ushort) numArray.Length;
    }
    provider.WriteByte(iOffset, this.m_id);
    ++iOffset;
    provider.WriteByte(iOffset, this.m_ReferenceType);
    ++iOffset;
    provider.WriteUInt16(iOffset, this.m_usOptions);
    provider.WriteBit(iOffset, this.m_bCustomNumberFormat, 0);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usNumIndex);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usFormulaSize);
    iOffset += 2;
    this.m_iLength = 8;
    if (numArray == null)
      return;
    provider.WriteBytes(iOffset, numArray, 0, (int) this.m_usFormulaSize);
    this.m_iLength += (int) this.m_usFormulaSize;
  }

  public override object Clone()
  {
    ChartAIRecord chartAiRecord = (ChartAIRecord) base.Clone();
    if (this.m_arrExpression == null)
      return (object) chartAiRecord;
    int length = this.m_arrExpression.Length;
    chartAiRecord.m_arrExpression = new Ptg[length];
    for (int index = 0; index < length; ++index)
      chartAiRecord.m_arrExpression[index] = (Ptg) CloneUtils.CloneCloneable((ICloneable) this.m_arrExpression[index]);
    return (object) chartAiRecord;
  }

  public enum LinkIndex
  {
    LinkToTitleOrText,
    LinkToValues,
    LinkToCategories,
    LinkToBubbles,
  }

  public enum ReferenceType
  {
    DefaultCategories,
    EnteredDirectly,
    Worksheet,
    NotUsed,
    ErrorReported,
  }
}
