// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.TtfMicrosoftCmapSubTable
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal struct TtfMicrosoftCmapSubTable
{
  public ushort Format;
  public ushort Length;
  public ushort Version;
  public ushort SegCountX2;
  public ushort SearchRange;
  public ushort EntrySelector;
  public ushort RangeShift;
  public ushort[] EndCount;
  public ushort ReservedPad;
  public ushort[] StartCount;
  public ushort[] IdDelta;
  public ushort[] IdRangeOffset;
  public ushort[] GlyphID;
}
