// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.PivotTable.LineItem
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;

[CLSCompliant(false)]
public class LineItem : ICloneable
{
  private const int DEF_FIXEDPART_SIZE = 8;
  private const int DEF_BIT_MULTIDATANAME = 0;
  private const int DEF_BIT_SUBTOTAL = 9;
  private const int DEF_BIT_BLOCK = 10;
  private const int DEF_BIT_GRAND = 11;
  private const int DEF_BIT_MULTIDATAONAXIS = 12;
  private ushort m_usIdenticalItemsCount;
  private ushort m_usItemType;
  private ushort m_usMaxIndex;
  private ushort m_usOptions;
  private ushort[] m_arrIndexes;

  public ushort IdenticalItemsCount
  {
    get => this.m_usIdenticalItemsCount;
    set => this.m_usIdenticalItemsCount = value;
  }

  public ushort ItemType
  {
    get => this.m_usItemType;
    set => this.m_usItemType = value;
  }

  public ushort MaxIndex => this.m_usMaxIndex;

  public ushort Options => this.m_usOptions;

  public ushort[] Indexes
  {
    get => this.m_arrIndexes;
    set
    {
      this.m_arrIndexes = value != null ? value : throw new ArgumentNullException(nameof (value));
      this.m_usMaxIndex = (ushort) (this.m_arrIndexes.Length - 1);
    }
  }

  public bool IsMultiDataName
  {
    get => BiffRecordRaw.GetBitFromVar(this.m_usOptions, 0);
    set => this.m_usOptions = (ushort) BiffRecordRaw.SetBit((int) this.m_usOptions, 0, value);
  }

  public bool IsSubtotal
  {
    get => BiffRecordRaw.GetBitFromVar(this.m_usOptions, 9);
    set => this.m_usOptions = (ushort) BiffRecordRaw.SetBit((int) this.m_usOptions, 9, value);
  }

  public bool IsBlock
  {
    get => BiffRecordRaw.GetBitFromVar(this.m_usOptions, 10);
    set => this.m_usOptions = (ushort) BiffRecordRaw.SetBit((int) this.m_usOptions, 10, value);
  }

  public bool IsGrand
  {
    get => BiffRecordRaw.GetBitFromVar(this.m_usOptions, 11);
    set => this.m_usOptions = (ushort) BiffRecordRaw.SetBit((int) this.m_usOptions, 11, value);
  }

  public bool IsMultiDataOnAxis
  {
    get => BiffRecordRaw.GetBitFromVar(this.m_usOptions, 12);
    set => this.m_usOptions = (ushort) BiffRecordRaw.SetBit((int) this.m_usOptions, 12, value);
  }

  public int Length => 8 + this.m_arrIndexes.Length * 2;

  public int Parse(DataProvider provider, int iOffset, int iFieldsCount)
  {
    this.m_usIdenticalItemsCount = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usItemType = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usMaxIndex = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usOptions = provider.ReadUInt16(iOffset);
    iOffset += 2;
    int length = iFieldsCount;
    this.m_arrIndexes = new ushort[length];
    int index = 0;
    while (index < length)
    {
      this.m_arrIndexes[index] = provider.ReadUInt16(iOffset);
      ++index;
      iOffset += 2;
    }
    return length * 2 + 8;
  }

  public int Serialize(byte[] arrData, int iOffset)
  {
    BiffRecordRaw.SetUInt16(arrData, iOffset, this.m_usIdenticalItemsCount);
    iOffset += 2;
    BiffRecordRaw.SetUInt16(arrData, iOffset, this.m_usItemType);
    iOffset += 2;
    BiffRecordRaw.SetUInt16(arrData, iOffset, this.m_usMaxIndex);
    iOffset += 2;
    BiffRecordRaw.SetUInt16(arrData, iOffset, this.m_usOptions);
    iOffset += 2;
    int length = this.m_arrIndexes.Length;
    if (length > 0)
      Buffer.BlockCopy((Array) this.m_arrIndexes, 0, (Array) arrData, iOffset, length * 2);
    return length * 2 + 8;
  }

  public object Clone()
  {
    LineItem lineItem = (LineItem) this.MemberwiseClone();
    lineItem.m_arrIndexes = CloneUtils.CloneUshortArray(this.m_arrIndexes);
    return (object) lineItem;
  }

  public enum LineItemType
  {
    Data,
    Default,
    Sum,
    CountA,
    Count,
    Average,
    Max,
    Min,
    Product,
    Stdev,
    StdevP,
    Var,
    VarP,
    GrandTotal,
  }
}
