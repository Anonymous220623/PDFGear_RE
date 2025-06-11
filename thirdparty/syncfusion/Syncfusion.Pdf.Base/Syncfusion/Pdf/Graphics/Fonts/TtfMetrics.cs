// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.TtfMetrics
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Native;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal struct TtfMetrics
{
  public int LineGap;
  public bool ContainsCFF;
  public bool IsSymbol;
  public RECT FontBox;
  public bool IsFixedPitch;
  public float ItalicAngle;
  public string PostScriptName;
  public string FontFamily;
  public float CapHeight;
  public float Leading;
  public float MacAscent;
  public float MacDescent;
  public float WinDescent;
  public float WinAscent;
  public float StemV;
  public int[] WidthTable;
  public int MacStyle;
  public float SubScriptSizeFactor;
  public float SuperscriptSizeFactor;

  public bool IsItalic => (this.MacStyle & 2) != 0;

  public bool IsBold => (this.MacStyle & 1) != 0;
}
