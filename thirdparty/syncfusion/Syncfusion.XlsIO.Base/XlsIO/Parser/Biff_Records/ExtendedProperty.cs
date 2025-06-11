// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.ExtendedProperty
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

public class ExtendedProperty
{
  public const int MaxTintValue = 32767 /*0x7FFF*/;
  private ushort m_usType;
  private ushort m_propSize;
  private ushort m_colorType = 2;
  private uint m_colorValue;
  private double m_tintAndShade;
  private long reserved;
  private ushort m_fontScheme;
  private ushort m_textIndentationLevel;
  private uint m_gradientType;
  private long m_iAngle;
  private long m_fillToRectLeft;
  private long m_fillToRectRight;
  private long m_fillToRectTop;
  private long m_fillToRectBottom;
  private uint m_gradStopCount;
  private int m_gradColorValue;
  private long m_gradPostition;
  private long m_gradTint;
  private List<Syncfusion.XlsIO.Parser.Biff_Records.GradStops> m_gradstops;

  public CellPropertyExtensionType Type
  {
    get => (CellPropertyExtensionType) this.m_usType;
    set => this.m_usType = (ushort) (byte) value;
  }

  public ushort Size
  {
    get => this.m_propSize;
    set => this.m_propSize = value;
  }

  public ColorType ColorType
  {
    get => (ColorType) this.m_colorType;
    set => this.m_colorType = (ushort) (byte) value;
  }

  public uint ColorValue
  {
    get => this.m_colorValue;
    set => this.m_colorValue = value;
  }

  public double Tint
  {
    get => this.m_tintAndShade;
    set => this.m_tintAndShade = value;
  }

  public long Reserved
  {
    get => this.reserved;
    set => this.reserved = value;
  }

  public FontScheme FontScheme
  {
    get => (FontScheme) this.m_fontScheme;
    set => this.m_fontScheme = (ushort) (byte) value;
  }

  public ushort Indent
  {
    get => this.m_textIndentationLevel;
    set
    {
      this.m_textIndentationLevel = this.m_textIndentationLevel <= (ushort) 250 ? value : throw new ArgumentOutOfRangeException("Indent level", "Text indentation level must be less than or equal to 250");
    }
  }

  public List<Syncfusion.XlsIO.Parser.Biff_Records.GradStops> GradStops
  {
    get => this.m_gradstops;
    set => this.m_gradstops = value;
  }

  public ExtendedProperty() => this.m_gradstops = new List<Syncfusion.XlsIO.Parser.Biff_Records.GradStops>();

  public int ParseExtendedProperty(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_usType = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_propSize = provider.ReadUInt16(iOffset);
    iOffset += 2;
    if (this.Type == CellPropertyExtensionType.GradientFill)
      iOffset = this.ParseGradient(provider, iOffset, version);
    else if (this.Type == CellPropertyExtensionType.FontScheme)
    {
      switch (this.m_propSize)
      {
        case 5:
          this.m_fontScheme = (ushort) provider.ReadByte(iOffset);
          ++iOffset;
          break;
        case 6:
          this.m_fontScheme = provider.ReadUInt16(iOffset);
          iOffset += 2;
          break;
      }
    }
    else if (this.Type == CellPropertyExtensionType.TextIndentationLevel)
    {
      this.m_textIndentationLevel = provider.ReadUInt16(iOffset);
      iOffset += 2;
    }
    else
      iOffset = this.ParseFullColor(provider, iOffset, version);
    return iOffset;
  }

  public int InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_usType);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.Size);
    iOffset += 2;
    if (this.Type == CellPropertyExtensionType.GradientFill)
      iOffset = this.SerializeGradient(provider, iOffset, version);
    else if (this.Type == CellPropertyExtensionType.FontScheme)
    {
      switch (this.m_propSize)
      {
        case 5:
          provider.WriteByte(iOffset, (byte) this.m_fontScheme);
          ++iOffset;
          break;
        case 6:
          provider.WriteUInt16(iOffset, this.m_fontScheme);
          iOffset += 2;
          break;
      }
    }
    else if (this.Type == CellPropertyExtensionType.TextIndentationLevel)
    {
      provider.WriteUInt16(iOffset, this.m_textIndentationLevel);
      iOffset += 2;
    }
    else
      iOffset = this.SerializeFullColor(provider, iOffset, version);
    return iOffset;
  }

  public int ParseFullColor(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_colorType = (ushort) provider.ReadByte(iOffset);
    iOffset += 2;
    this.m_tintAndShade = (double) provider.ReadInt16(iOffset);
    iOffset += 2;
    this.m_colorValue = provider.ReadUInt32(iOffset);
    iOffset += 4;
    this.reserved = provider.ReadInt64(iOffset);
    iOffset += 8;
    return iOffset;
  }

  public int SerializeFullColor(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_colorType);
    iOffset += 2;
    if (this.m_usType == (ushort) 4 && this.m_colorType == (ushort) 3)
      this.m_tintAndShade *= (double) short.MaxValue;
    provider.WriteUInt16(iOffset, (ushort) this.m_tintAndShade);
    iOffset += 2;
    provider.WriteUInt32(iOffset, this.m_colorValue);
    iOffset += 4;
    provider.WriteInt64(iOffset, this.reserved);
    iOffset += 8;
    return iOffset;
  }

  public int ParseGradient(DataProvider provider, int iOffset, ExcelVersion version)
  {
    this.m_gradientType = (uint) provider.ReadByte(iOffset);
    iOffset += 4;
    this.m_iAngle = provider.ReadInt64(iOffset);
    iOffset += 8;
    this.m_fillToRectLeft = provider.ReadInt64(iOffset);
    iOffset += 8;
    this.m_fillToRectRight = provider.ReadInt64(iOffset);
    iOffset += 8;
    this.m_fillToRectTop = provider.ReadInt64(iOffset);
    iOffset += 8;
    this.m_fillToRectBottom = provider.ReadInt64(iOffset);
    iOffset += 8;
    this.m_gradStopCount = provider.ReadUInt32(iOffset);
    iOffset += 4;
    for (int index = 0; (long) index < (long) this.m_gradStopCount; ++index)
    {
      Syncfusion.XlsIO.Parser.Biff_Records.GradStops gradStops = new Syncfusion.XlsIO.Parser.Biff_Records.GradStops();
      iOffset = gradStops.ParseGradStops(provider, iOffset, version);
      this.m_gradstops.Add(gradStops);
    }
    return iOffset;
  }

  public int SerializeGradient(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteByte(iOffset, (byte) this.m_gradientType);
    iOffset += 4;
    provider.WriteInt64(iOffset, this.m_iAngle);
    iOffset += 8;
    provider.WriteInt64(iOffset, this.m_fillToRectLeft);
    iOffset += 8;
    provider.WriteInt64(iOffset, this.m_fillToRectRight);
    iOffset += 8;
    provider.WriteInt64(iOffset, this.m_fillToRectTop);
    iOffset += 8;
    provider.WriteInt64(iOffset, this.m_fillToRectBottom);
    iOffset += 8;
    provider.WriteUInt32(iOffset, this.m_gradStopCount);
    iOffset += 4;
    foreach (Syncfusion.XlsIO.Parser.Biff_Records.GradStops gradStop in this.GradStops)
      iOffset = gradStop.InfillInternalData(provider, iOffset, version);
    return iOffset;
  }
}
