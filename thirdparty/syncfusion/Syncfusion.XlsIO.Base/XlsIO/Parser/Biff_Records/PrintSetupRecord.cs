// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.PrintSetupRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[Biff(TBIFFRecord.PrintSetup)]
[CLSCompliant(false)]
public class PrintSetupRecord : BiffRecordRaw
{
  public const ushort ErrorBitMask = 3072 /*0x0C00*/;
  public const int ErrorStartBit = 10;
  private const int DEF_RECORD_SIZE = 34;
  [BiffRecordPos(0, 2)]
  private ushort m_usPaperSize = 9;
  [BiffRecordPos(2, 2)]
  private ushort m_usScale = 100;
  [BiffRecordPos(4, 2)]
  private short m_sPageStart = 1;
  [BiffRecordPos(6, 2)]
  private ushort m_usFitWidth = 1;
  [BiffRecordPos(8, 2)]
  private ushort m_usFitHeight = 1;
  [BiffRecordPos(10, 2)]
  private ushort m_usOptions = 4;
  [BiffRecordPos(10, 0, TFieldType.Bit)]
  private bool m_bLeftToRight;
  [BiffRecordPos(10, 1, TFieldType.Bit)]
  private bool m_bNotLandscape = true;
  [BiffRecordPos(10, 2, TFieldType.Bit)]
  private bool m_bNotValidSettings = true;
  [BiffRecordPos(10, 3, TFieldType.Bit)]
  private bool m_bNoColor;
  [BiffRecordPos(10, 4, TFieldType.Bit)]
  private bool m_bDraft;
  [BiffRecordPos(10, 5, TFieldType.Bit)]
  private bool m_bNotes;
  [BiffRecordPos(10, 6, TFieldType.Bit)]
  private bool m_bNoOrientation = true;
  [BiffRecordPos(10, 7, TFieldType.Bit)]
  private bool m_bUsePage;
  [BiffRecordPos(11, 1, TFieldType.Bit)]
  private bool m_bPrintNotes;
  [BiffRecordPos(12, 2)]
  private ushort m_usHResolution = 600;
  [BiffRecordPos(14, 2)]
  private ushort m_usVResolution = 600;
  [BiffRecordPos(16 /*0x10*/, 8, TFieldType.Float)]
  private double m_dbHeaderMargin = 0.5;
  [BiffRecordPos(24, 8, TFieldType.Float)]
  private double m_dbFooterMargin = 0.5;
  [BiffRecordPos(32 /*0x20*/, 2)]
  private ushort m_usCopies = 1;

  public ushort PaperSize
  {
    get => this.m_usPaperSize;
    set
    {
      this.m_usPaperSize = value;
      this.m_bNotValidSettings = false;
    }
  }

  public ushort Scale
  {
    get => this.m_usScale;
    set
    {
      this.m_usScale = value;
      this.m_bNotValidSettings = false;
    }
  }

  public short PageStart
  {
    get => this.m_sPageStart;
    set
    {
      this.m_sPageStart = value;
      this.m_bNotValidSettings = false;
    }
  }

  public ushort FitWidth
  {
    get => this.m_usFitWidth;
    set
    {
      this.m_usFitWidth = value;
      this.m_bNotValidSettings = false;
    }
  }

  public ushort FitHeight
  {
    get => this.m_usFitHeight;
    set
    {
      this.m_usFitHeight = value;
      this.m_bNotValidSettings = false;
    }
  }

  public ushort HResolution
  {
    get => this.m_usHResolution;
    set
    {
      this.m_usHResolution = value;
      this.m_bNotValidSettings = false;
    }
  }

  public ushort VResolution
  {
    get => this.m_usVResolution;
    set
    {
      this.m_usVResolution = value;
      this.m_bNotValidSettings = false;
    }
  }

  public double HeaderMargin
  {
    get => this.m_dbHeaderMargin;
    set
    {
      this.m_dbHeaderMargin = value;
      this.m_bNotValidSettings = false;
    }
  }

  public double FooterMargin
  {
    get => this.m_dbFooterMargin;
    set
    {
      this.m_dbFooterMargin = value;
      this.m_bNotValidSettings = false;
    }
  }

  public ushort Copies
  {
    get => this.m_usCopies;
    set
    {
      this.m_usCopies = value;
      this.m_bNotValidSettings = false;
    }
  }

  public bool IsLeftToRight
  {
    get => this.m_bLeftToRight;
    set
    {
      this.m_bLeftToRight = value;
      this.m_bNotValidSettings = false;
    }
  }

  public bool IsNotLandscape
  {
    get => this.m_bNotLandscape;
    set
    {
      this.m_bNotLandscape = value;
      this.m_bNotValidSettings = false;
    }
  }

  public bool IsNotValidSettings
  {
    get => this.m_bNotValidSettings;
    set => this.m_bNotValidSettings = value;
  }

  public bool IsNoColor
  {
    get => this.m_bNoColor;
    set
    {
      this.m_bNoColor = value;
      this.m_bNotValidSettings = false;
    }
  }

  public bool IsDraft
  {
    get => this.m_bDraft;
    set
    {
      this.m_bDraft = value;
      this.m_bNotValidSettings = false;
    }
  }

  public bool IsNotes
  {
    get => this.m_bNotes;
    set
    {
      this.m_bNotes = value;
      this.m_bNotValidSettings = false;
    }
  }

  public bool IsNoOrientation
  {
    get => this.m_bNoOrientation;
    set
    {
      this.m_bNoOrientation = value;
      this.m_bNotValidSettings = false;
    }
  }

  public bool IsUsePage
  {
    get => this.m_bUsePage;
    set
    {
      this.m_bUsePage = value;
      this.m_bNotValidSettings = false;
    }
  }

  public bool IsPrintNotesAsDisplayed
  {
    get => this.m_bPrintNotes;
    set
    {
      this.m_bPrintNotes = value;
      this.m_bNotValidSettings = false;
    }
  }

  public ExcelPrintErrors PrintErrors
  {
    get
    {
      return (ExcelPrintErrors) ((int) BiffRecordRaw.GetUInt16BitsByMask(this.m_usOptions, (ushort) 3072 /*0x0C00*/) >> 10);
    }
    set
    {
      BiffRecordRaw.SetUInt16BitsByMask(ref this.m_usOptions, (ushort) 3072 /*0x0C00*/, (ushort) ((uint) value << 10));
      this.m_bNotValidSettings = false;
    }
  }

  public override int MinimumRecordSize => 34;

  public override int MaximumRecordSize => 34;

  public PrintSetupRecord()
  {
  }

  public PrintSetupRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public PrintSetupRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_iLength = 34;
    this.m_usPaperSize = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usScale = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_sPageStart = provider.ReadInt16(iOffset);
    iOffset += 2;
    this.m_usFitWidth = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usFitHeight = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usOptions = provider.ReadUInt16(iOffset);
    this.m_bLeftToRight = provider.ReadBit(iOffset, 0);
    this.m_bNotLandscape = provider.ReadBit(iOffset, 1);
    this.m_bNotValidSettings = provider.ReadBit(iOffset, 2);
    if (this.m_bNotValidSettings)
      this.m_usScale = (ushort) 100;
    this.m_bNoColor = provider.ReadBit(iOffset, 3);
    this.m_bDraft = provider.ReadBit(iOffset, 4);
    this.m_bNotes = provider.ReadBit(iOffset, 5);
    this.m_bNoOrientation = provider.ReadBit(iOffset, 6);
    if (this.m_bNoOrientation || this.m_bNotValidSettings)
      this.m_bNotLandscape = true;
    this.m_bUsePage = provider.ReadBit(iOffset, 7);
    ++iOffset;
    this.m_bPrintNotes = provider.ReadBit(iOffset, 1);
    ++iOffset;
    this.m_usHResolution = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usVResolution = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_dbHeaderMargin = provider.ReadDouble(iOffset);
    iOffset += 8;
    this.m_dbFooterMargin = provider.ReadDouble(iOffset);
    iOffset += 8;
    this.m_usCopies = provider.ReadUInt16(iOffset);
    iOffset += 2;
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_iLength = 34;
    provider.WriteUInt16(iOffset, this.m_usPaperSize);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usScale);
    iOffset += 2;
    provider.WriteInt16(iOffset, this.m_sPageStart);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usFitWidth);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usFitHeight);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usOptions);
    provider.WriteBit(iOffset, this.m_bLeftToRight, 0);
    provider.WriteBit(iOffset, this.m_bNotLandscape, 1);
    provider.WriteBit(iOffset, this.m_bNotValidSettings, 2);
    provider.WriteBit(iOffset, this.m_bNoColor, 3);
    provider.WriteBit(iOffset, this.m_bDraft, 4);
    provider.WriteBit(iOffset, this.m_bNotes, 5);
    provider.WriteBit(iOffset, this.m_bNoOrientation, 6);
    provider.WriteBit(iOffset, this.m_bUsePage, 7);
    ++iOffset;
    provider.WriteBit(iOffset, this.m_bPrintNotes, 1);
    ++iOffset;
    provider.WriteUInt16(iOffset, this.m_usHResolution);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_usVResolution);
    iOffset += 2;
    provider.WriteDouble(iOffset, this.m_dbHeaderMargin);
    iOffset += 8;
    provider.WriteDouble(iOffset, this.m_dbFooterMargin);
    iOffset += 8;
    provider.WriteUInt16(iOffset, this.m_usCopies);
  }

  public override int GetStoreSize(ExcelVersion version) => 34;
}
