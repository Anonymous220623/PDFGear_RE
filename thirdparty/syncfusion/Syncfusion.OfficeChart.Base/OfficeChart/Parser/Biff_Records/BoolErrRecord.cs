// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.BoolErrRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.BoolErr)]
internal class BoolErrRecord : CellPositionBase, IValueHolder
{
  private const int DEF_RECORD_SIZE = 8;
  [BiffRecordPos(6, 1)]
  private byte m_BoolOrError;
  [BiffRecordPos(7, 1)]
  private byte m_IsErrorCode;

  public byte BoolOrError
  {
    get => this.m_BoolOrError;
    set => this.m_BoolOrError = value;
  }

  public bool IsErrorCode
  {
    get => this.m_IsErrorCode == (byte) 1;
    set => this.m_IsErrorCode = value ? (byte) 1 : (byte) 0;
  }

  public override int MinimumRecordSize => 8;

  public override int MaximumRecordSize => 8;

  protected override void ParseCellData(DataProvider provider, int iOffset, OfficeVersion version)
  {
    this.m_BoolOrError = provider.ReadByte(iOffset);
    this.m_IsErrorCode = provider.ReadByte(iOffset + 1);
  }

  protected override void InfillCellData(DataProvider provider, int iOffset, OfficeVersion version)
  {
    provider.WriteByte(iOffset, this.m_BoolOrError);
    provider.WriteByte(iOffset + 1, this.m_IsErrorCode);
  }

  public override int GetStoreSize(OfficeVersion version)
  {
    int storeSize = 8;
    if (version != OfficeVersion.Excel97to2003)
      storeSize += 4;
    return storeSize;
  }

  public static int ReadValue(DataProvider provider, int recordStart, OfficeVersion version)
  {
    recordStart += 10;
    if (version != OfficeVersion.Excel97to2003)
      recordStart += 4;
    return (int) provider.ReadInt16(recordStart);
  }

  public object Value
  {
    get => !this.IsErrorCode ? (object) (this.BoolOrError != (byte) 0) : (object) this.BoolOrError;
    set
    {
      if (value is bool)
      {
        this.IsErrorCode = false;
        this.BoolOrError = (byte) value;
      }
      else
      {
        this.IsErrorCode = true;
        this.BoolOrError = (byte) value;
      }
    }
  }
}
