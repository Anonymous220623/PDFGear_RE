// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.Charts.ChartAlrunsRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records.Charts;

[Biff(TBIFFRecord.ChartAlruns)]
[CLSCompliant(false)]
internal class ChartAlrunsRecord : BiffRecordRaw
{
  [BiffRecordPos(0, 2)]
  private ushort m_usQuantity;
  private ChartAlrunsRecord.TRuns[] m_array = new ChartAlrunsRecord.TRuns[0];

  public ushort Quantity
  {
    get => this.m_usQuantity;
    set
    {
      if ((int) value == (int) this.m_usQuantity)
        return;
      this.m_usQuantity = value;
    }
  }

  public ChartAlrunsRecord.TRuns[] Runs
  {
    get => this.m_array;
    set
    {
      this.m_array = value != null ? value : throw new ArgumentNullException(nameof (value));
      this.m_usQuantity = (ushort) this.m_array.Length;
    }
  }

  public ChartAlrunsRecord()
  {
  }

  public ChartAlrunsRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartAlrunsRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_usQuantity = provider.ReadUInt16(iOffset);
    this.m_array = new ChartAlrunsRecord.TRuns[(int) this.m_usQuantity];
    int iOffset1 = iOffset + 2;
    int index = 0;
    while (index < (int) this.m_usQuantity)
    {
      this.m_array[index] = new ChartAlrunsRecord.TRuns(provider.ReadUInt16(iOffset1), provider.ReadUInt16(iOffset1 + 2));
      ++index;
      iOffset1 += 4;
    }
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usQuantity);
    int iOffset1 = iOffset + 2;
    int index = 0;
    while (index < (int) this.m_usQuantity)
    {
      provider.WriteUInt16(iOffset1, this.m_array[index].FirstCharIndex);
      provider.WriteUInt16(iOffset1 + 2, this.m_array[index].FontIndex);
      ++index;
      iOffset1 += 4;
    }
    this.m_iLength = iOffset1;
  }

  public override int GetStoreSize(OfficeVersion version) => 2 + this.m_array.Length * 4;

  public override object Clone()
  {
    ChartAlrunsRecord chartAlrunsRecord = (ChartAlrunsRecord) base.Clone();
    if (this.m_array == null)
      return (object) chartAlrunsRecord;
    int length = this.m_array.Length;
    chartAlrunsRecord.m_array = new ChartAlrunsRecord.TRuns[length];
    for (int index = 0; index < length; ++index)
      chartAlrunsRecord.m_array[index] = (ChartAlrunsRecord.TRuns) CloneUtils.CloneCloneable((ICloneable) this.m_array[index]);
    return (object) chartAlrunsRecord;
  }

  public class TRuns : ICloneable
  {
    internal const int Size = 4;
    private ushort m_usFirstChar;
    private ushort m_usFontIndex;
    private bool m_newParagraphStart;

    public ushort FirstCharIndex
    {
      get => this.m_usFirstChar;
      set => this.m_usFirstChar = value;
    }

    public ushort FontIndex
    {
      get => this.m_usFontIndex;
      set => this.m_usFontIndex = value;
    }

    internal bool HasNewParagarphStart
    {
      get => this.m_newParagraphStart;
      set => this.m_newParagraphStart = value;
    }

    public TRuns(ushort firstChar, ushort fontIndex)
    {
      this.m_usFirstChar = firstChar;
      this.m_usFontIndex = fontIndex;
    }

    public object Clone() => this.MemberwiseClone();
  }
}
