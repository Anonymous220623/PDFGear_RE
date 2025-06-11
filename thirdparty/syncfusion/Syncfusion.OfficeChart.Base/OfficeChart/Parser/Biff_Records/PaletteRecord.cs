// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.PaletteRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.Palette)]
internal class PaletteRecord : BiffRecordRaw
{
  [BiffRecordPos(0, 2)]
  private ushort m_usColorsCount = 56;
  private PaletteRecord.TColor[] m_arrColor;

  public ushort ColorsCount => this.m_usColorsCount;

  public PaletteRecord.TColor[] Colors
  {
    get => this.m_arrColor;
    set
    {
      this.m_arrColor = value;
      this.m_usColorsCount = value != null ? (ushort) value.Length : (ushort) 0;
    }
  }

  public override int MinimumRecordSize => 2;

  public override int MaximumRecordSize => 226;

  public PaletteRecord()
  {
  }

  public PaletteRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public PaletteRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    this.m_usColorsCount = provider.ReadUInt16(iOffset);
    this.m_arrColor = new PaletteRecord.TColor[(int) this.m_usColorsCount];
    iOffset += 2;
    int index = 0;
    while (index < (int) this.m_usColorsCount)
    {
      this.m_arrColor[index].R = provider.ReadByte(iOffset);
      this.m_arrColor[index].G = provider.ReadByte(iOffset + 1);
      this.m_arrColor[index].B = provider.ReadByte(iOffset + 2);
      this.m_arrColor[index].A = provider.ReadByte(iOffset + 3);
      ++index;
      iOffset += 4;
    }
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usColorsCount);
    this.m_iLength = 2;
    for (int index = 0; index < (int) this.m_usColorsCount; ++index)
    {
      provider.WriteByte(iOffset + this.m_iLength, this.m_arrColor[index].R);
      ++this.m_iLength;
      provider.WriteByte(iOffset + this.m_iLength, this.m_arrColor[index].G);
      ++this.m_iLength;
      provider.WriteByte(iOffset + this.m_iLength, this.m_arrColor[index].B);
      ++this.m_iLength;
      provider.WriteByte(iOffset + this.m_iLength, this.m_arrColor[index].A);
      ++this.m_iLength;
    }
  }

  public override int GetStoreSize(OfficeVersion version) => 2 + (int) this.m_usColorsCount * 4;

  public struct TColor
  {
    public byte R;
    public byte G;
    public byte B;
    public byte A;

    public override string ToString()
    {
      return Color.FromArgb((int) this.A, (int) this.R, (int) this.G, (int) this.B).ToString();
    }
  }
}
