// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.BoundSheetRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Security;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[Biff(TBIFFRecord.BoundSheet)]
[CLSCompliant(false)]
public class BoundSheetRecord : BiffRecordRaw
{
  [BiffRecordPos(0, 4, true)]
  private int m_iBOFPosition = 1394;
  [BiffRecordPos(4, 1, true)]
  private byte m_Visibility;
  [BiffRecordPos(5, 1, true)]
  private byte m_SheetType;
  [BiffRecordPos(6, TFieldType.String)]
  private string m_strSheetName = "Sheet1";
  private int m_iSheetIndex = -1;
  private BOFRecord m_bof;

  public int BOFPosition
  {
    get => this.m_iBOFPosition;
    set => this.m_iBOFPosition = value;
  }

  public string SheetName
  {
    get => this.m_strSheetName;
    set => this.m_strSheetName = value;
  }

  public int SheetIndex
  {
    get => this.m_iSheetIndex;
    set => this.m_iSheetIndex = value;
  }

  public BoundSheetRecord.SheetType BoundSheetType
  {
    get => (BoundSheetRecord.SheetType) this.m_SheetType;
    set => this.m_SheetType = (byte) value;
  }

  public WorksheetVisibility Visibility
  {
    get => (WorksheetVisibility) this.m_Visibility;
    set => this.m_Visibility = (byte) value;
  }

  public override int MinimumRecordSize => 8;

  public BOFRecord BOF
  {
    get => this.m_bof;
    set => this.m_bof = value;
  }

  public override int StartDecodingOffset => 4;

  public BoundSheetRecord()
  {
  }

  public BoundSheetRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public BoundSheetRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_iBOFPosition = provider.ReadInt32(iOffset);
    this.m_Visibility = provider.ReadByte(iOffset + 4);
    this.m_SheetType = provider.ReadByte(iOffset + 5);
    this.m_strSheetName = provider.ReadString8Bit(iOffset + 6, out int _);
    this.InternalDataIntegrityCheck();
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    int num = iOffset;
    provider.WriteInt32(iOffset, this.m_iBOFPosition);
    provider.WriteByte(iOffset + 4, this.m_Visibility);
    provider.WriteByte(iOffset + 5, this.m_SheetType);
    iOffset += 6;
    provider.WriteString8BitUpdateOffset(ref iOffset, this.m_strSheetName);
    this.m_iLength = iOffset - num;
  }

  private void InternalDataIntegrityCheck()
  {
  }

  public override int FillStream(
    BinaryWriter writer,
    DataProvider provider,
    IEncryptor encryptor,
    int streamPosition)
  {
    this.m_iBOFPosition = (int) this.m_bof.StreamPos;
    return base.FillStream(writer, provider, encryptor, streamPosition);
  }

  public override int GetStoreSize(ExcelVersion version)
  {
    return 8 + Encoding.Unicode.GetByteCount(this.m_strSheetName);
  }

  public enum SheetType
  {
    Worksheet = 0,
    Chart = 2,
    VisualBasicModule = 6,
  }
}
