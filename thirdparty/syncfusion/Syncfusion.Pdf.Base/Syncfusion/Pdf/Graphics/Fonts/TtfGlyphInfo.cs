// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.TtfGlyphInfo
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal struct TtfGlyphInfo : IComparable
{
  public int Index;
  public int Width;
  public int CharCode;

  public bool Empty
  {
    get => this.Index == this.Width && this.Width == this.CharCode && this.CharCode == 0;
  }

  public int CompareTo(object obj) => this.Index - ((TtfGlyphInfo) obj).Index;
}
