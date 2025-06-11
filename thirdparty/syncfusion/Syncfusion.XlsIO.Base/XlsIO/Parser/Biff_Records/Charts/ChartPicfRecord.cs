// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.Charts.ChartPicfRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.Charts;

[CLSCompliant(false)]
[Biff(TBIFFRecord.ChartPicf)]
public class ChartPicfRecord : BiffRecordRaw
{
  public const int DefaultRecordSize = 14;
  [BiffRecordPos(0, 2)]
  private ushort m_usPictureType;
  [BiffRecordPos(2, 2)]
  private ushort m_usImageFormat;
  [BiffRecordPos(4, 1)]
  private byte m_Environment;
  [BiffRecordPos(5, 1)]
  private byte m_usOptions;
  [BiffRecordPos(5, 0, TFieldType.Bit)]
  private bool m_bFormatOnly;
  [BiffRecordPos(5, 1, TFieldType.Bit)]
  private bool m_bPictureTopBottom;
  [BiffRecordPos(5, 2, TFieldType.Bit)]
  private bool m_bPictureBackFront;
  [BiffRecordPos(5, 3, TFieldType.Bit)]
  private bool m_bPictureSides;
  [BiffRecordPos(6, 8, TFieldType.Float)]
  private double m_numScale;

  public ChartPicfRecord.TPicture PictureType
  {
    get => (ChartPicfRecord.TPicture) this.m_usPictureType;
    set => this.m_usPictureType = (ushort) value;
  }

  public ChartPicfRecord.TImageFormat ImageFormat
  {
    get => (ChartPicfRecord.TImageFormat) this.m_usImageFormat;
    set => this.m_usImageFormat = (ushort) value;
  }

  public ChartPicfRecord.TEnvironment Environment
  {
    get => (ChartPicfRecord.TEnvironment) this.m_Environment;
    set => this.m_Environment = (byte) value;
  }

  public byte Options => this.m_usOptions;

  public bool IsFormatOnly
  {
    get => this.m_bFormatOnly;
    set => this.m_bFormatOnly = value;
  }

  public bool IsPictureTopBottom
  {
    get => this.m_bPictureTopBottom;
    set => this.m_bPictureTopBottom = value;
  }

  public bool IsPictureBackFront
  {
    get => this.m_bPictureBackFront;
    set => this.m_bPictureBackFront = value;
  }

  public bool IsPictureSides
  {
    get => this.m_bPictureSides;
    set => this.m_bPictureSides = value;
  }

  public double Scale
  {
    get => this.m_numScale;
    set => this.m_numScale = value;
  }

  public override int MinimumRecordSize => 14;

  public override int MaximumRecordSize => 14;

  public ChartPicfRecord()
  {
  }

  public ChartPicfRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ChartPicfRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_usPictureType = provider.ReadUInt16(iOffset);
    this.m_usImageFormat = provider.ReadUInt16(iOffset + 2);
    this.m_Environment = provider.ReadByte(iOffset + 4);
    this.m_usOptions = provider.ReadByte(iOffset + 5);
    this.m_bFormatOnly = provider.ReadBit(iOffset + 5, 0);
    this.m_bPictureTopBottom = provider.ReadBit(iOffset + 5, 1);
    this.m_bPictureBackFront = provider.ReadBit(iOffset + 5, 2);
    this.m_bPictureSides = provider.ReadBit(iOffset + 5, 3);
    this.m_numScale = provider.ReadDouble(iOffset + 6);
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usPictureType);
    provider.WriteUInt16(iOffset + 2, this.m_usImageFormat);
    provider.WriteByte(iOffset + 4, this.m_Environment);
    provider.WriteByte(iOffset + 5, this.m_usOptions);
    provider.WriteBit(iOffset + 5, this.m_bFormatOnly, 0);
    provider.WriteBit(iOffset + 5, this.m_bPictureTopBottom, 1);
    provider.WriteBit(iOffset + 5, this.m_bPictureBackFront, 2);
    provider.WriteBit(iOffset + 5, this.m_bPictureSides, 3);
    provider.WriteDouble(iOffset + 6, this.m_numScale);
    this.m_iLength = 14;
  }

  [Flags]
  public enum TPicture
  {
    Stretched = 1,
    Stacked = 2,
  }

  public enum TImageFormat
  {
    MacintoshPICT = 2,
    WindowsMetafile = 2,
    WindowsBitmap = 9,
  }

  public enum TEnvironment
  {
    Windows = 1,
    Macintosh = 2,
  }
}
