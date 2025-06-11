// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.PivotTable.PivotViewFieldsExRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;

[CLSCompliant(false)]
[Biff(TBIFFRecord.PivotViewFieldsEx)]
public class PivotViewFieldsExRecord : BiffRecordRawWithArray
{
  [BiffRecordPos(0, 4)]
  private ushort m_usOptions;
  [BiffRecordPos(0, 0, TFieldType.Bit)]
  private bool m_bShowAllItems;
  [BiffRecordPos(0, 1, TFieldType.Bit)]
  private bool m_bDragToRow;
  [BiffRecordPos(0, 2, TFieldType.Bit)]
  private bool m_bDragToColumn;
  [BiffRecordPos(0, 3, TFieldType.Bit)]
  private bool m_bDragToPage;
  [BiffRecordPos(0, 4, TFieldType.Bit)]
  private bool m_bDragToHide;
  [BiffRecordPos(0, 7, TFieldType.Bit)]
  private bool m_bServerBased;
  [BiffRecordPos(1, 1, TFieldType.Bit)]
  private bool m_bAutoSort;
  [BiffRecordPos(1, 2, TFieldType.Bit)]
  private bool m_bAscendSort;
  [BiffRecordPos(1, 3, TFieldType.Bit)]
  private bool m_bAutoShow;
  [BiffRecordPos(1, 4, TFieldType.Bit)]
  private bool m_bAscendShow;
  [BiffRecordPos(1, 5, TFieldType.Bit)]
  private bool m_bCalculateField;
  [BiffRecordPos(4, 1)]
  private byte m_btReserved;
  [BiffRecordPos(5, 1)]
  private byte m_btItemsNumber = 10;
  [BiffRecordPos(6, 2)]
  private ushort m_usSortIndex;
  [BiffRecordPos(8, 2)]
  private ushort m_usShowIndex;
  [BiffRecordPos(10, 2)]
  private ushort m_usFormat;
  private string m_strSubTotalName;

  public PivotViewFieldsExRecord()
  {
  }

  public PivotViewFieldsExRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public PivotViewFieldsExRecord(int iReserve)
    : base(iReserve)
  {
  }

  public ushort Options => this.m_usOptions;

  public bool IsShowAllItems
  {
    get => this.m_bShowAllItems;
    set => this.m_bShowAllItems = value;
  }

  public bool IsDragToRow
  {
    get => this.m_bDragToRow;
    set => this.m_bDragToRow = value;
  }

  public bool IsDragToColumn
  {
    get => this.m_bDragToColumn;
    set => this.m_bDragToColumn = value;
  }

  public bool IsDragToPage
  {
    get => this.m_bDragToPage;
    set => this.m_bDragToPage = value;
  }

  public bool IsDragToHide
  {
    get => this.m_bDragToHide;
    set => this.m_bDragToHide = value;
  }

  public bool IsServerBased
  {
    get => this.m_bServerBased;
    set => this.m_bServerBased = value;
  }

  public bool IsAutoSort
  {
    get => this.m_bAutoSort;
    set => this.m_bAutoSort = value;
  }

  public bool IsAscendSort
  {
    get => this.m_bAscendSort;
    set => this.m_bAscendSort = value;
  }

  public bool IsAutoShow
  {
    get => this.m_bAutoShow;
    set => this.m_bAutoShow = value;
  }

  public bool IsAscendShow
  {
    get => this.m_bAscendShow;
    set => this.m_bAscendShow = value;
  }

  public bool IsCalculateField
  {
    get => this.m_bCalculateField;
    set => this.m_bCalculateField = value;
  }

  public byte Reserved => this.m_btReserved;

  public byte ItemsNumber
  {
    get => this.m_btItemsNumber;
    set => this.m_btItemsNumber = value;
  }

  public ushort SortIndex
  {
    get => this.m_usSortIndex;
    set => this.m_usSortIndex = value;
  }

  public ushort ShowIndex
  {
    get => this.m_usShowIndex;
    set => this.m_usShowIndex = value;
  }

  public ushort NumberFormat
  {
    get => this.m_usFormat;
    set => this.m_usFormat = value;
  }

  public string SubTotalName
  {
    get => this.m_strSubTotalName;
    set => this.m_strSubTotalName = value;
  }

  public override void ParseStructure()
  {
    this.m_usOptions = this.GetUInt16(0);
    this.m_bShowAllItems = this.GetBit(0, 0);
    this.m_bDragToRow = this.GetBit(0, 1);
    this.m_bDragToColumn = this.GetBit(0, 2);
    this.m_bDragToPage = this.GetBit(0, 3);
    this.m_bDragToHide = this.GetBit(0, 4);
    this.m_bServerBased = this.GetBit(0, 7);
    this.m_bAutoSort = this.GetBit(1, 1);
    this.m_bAscendSort = this.GetBit(1, 2);
    this.m_bAutoShow = this.GetBit(1, 3);
    this.m_bAscendShow = this.GetBit(1, 4);
    this.m_bCalculateField = this.GetBit(1, 5);
    this.m_btReserved = this.GetByte(2);
    this.m_btItemsNumber = this.GetByte(3);
    this.m_usSortIndex = this.GetUInt16(4);
    this.m_usShowIndex = this.GetUInt16(6);
    this.m_usFormat = this.GetUInt16(8);
    int offset = 10;
    if (this.m_iLength <= offset)
      return;
    int int16 = (int) this.GetInt16(offset);
    int index1 = offset + 2 + 8;
    int iLength = this.m_iLength;
    if (int16 <= 0)
      return;
    byte num = this.m_data[index1];
    int index2 = index1 + 1;
    Encoding encoding = num != (byte) 0 ? Encoding.Unicode : Encoding.Default;
    if (num != (byte) 0)
      int16 *= 2;
    this.m_strSubTotalName = encoding.GetString(this.m_data, index2, int16);
  }

  public override void InfillInternalData(ExcelVersion version)
  {
    this.m_iLength = 10;
    this.m_data = new byte[this.m_iLength];
    this.SetUInt16(0, this.m_usOptions);
    this.SetBit(0, this.m_bShowAllItems, 0);
    this.SetBit(0, this.m_bDragToRow, 1);
    this.SetBit(0, this.m_bDragToColumn, 2);
    this.SetBit(0, this.m_bDragToPage, 3);
    this.SetBit(0, this.m_bDragToHide, 4);
    this.SetBit(0, this.m_bServerBased, 7);
    this.SetBit(1, this.m_bAutoSort, 1);
    this.SetBit(1, this.m_bAscendSort, 2);
    this.SetBit(1, this.m_bAutoShow, 3);
    this.SetBit(1, this.m_bAscendShow, 4);
    this.SetBit(1, this.m_bCalculateField, 5);
    this.SetByte(2, this.m_btReserved);
    this.SetByte(3, this.m_btItemsNumber);
    this.SetUInt16(4, this.m_usSortIndex);
    this.SetUInt16(6, this.m_usShowIndex);
    this.SetUInt16(8, this.m_usFormat);
    int length = this.m_strSubTotalName != null ? this.m_strSubTotalName.Length : 0;
    this.AutoGrowData = true;
    if (length == 0)
      this.SetUInt16(this.m_iLength, ushort.MaxValue);
    else
      this.SetUInt16(this.m_iLength, (ushort) length);
    this.m_iLength += 2;
    this.SetInt32(this.m_iLength, 0);
    this.m_iLength += 4;
    this.SetInt32(this.m_iLength, 0);
    this.m_iLength += 4;
    if (this.m_strSubTotalName == null || this.m_strSubTotalName.Length <= 0)
      return;
    this.SetByte(this.m_iLength, (byte) 1);
    ++this.m_iLength;
    byte[] bytes = Encoding.Unicode.GetBytes(this.m_strSubTotalName);
    this.SetBytes(this.m_iLength, bytes);
    this.m_iLength += bytes.Length;
  }
}
