// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.BoolErrRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[Biff(TBIFFRecord.BoolErr)]
[CLSCompliant(false)]
public class BoolErrRecord : CellPositionBase, IValueHolder
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

  protected override void ParseCellData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_BoolOrError = provider.ReadByte(iOffset);
    this.m_IsErrorCode = provider.ReadByte(iOffset + 1);
  }

  protected override void InfillCellData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteByte(iOffset, this.m_BoolOrError);
    provider.WriteByte(iOffset + 1, this.m_IsErrorCode);
  }

  public override int GetStoreSize(ExcelVersion version)
  {
    int storeSize = 8;
    if (version != ExcelVersion.Excel97to2003)
      storeSize += 4;
    return storeSize;
  }

  public static int ReadValue(DataProvider provider, int recordStart, ExcelVersion version)
  {
    recordStart += 10;
    if (version != ExcelVersion.Excel97to2003)
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
