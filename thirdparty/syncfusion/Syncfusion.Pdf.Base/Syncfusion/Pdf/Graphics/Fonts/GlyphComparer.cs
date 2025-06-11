// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.GlyphComparer
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal sealed class GlyphComparer : IComparer<OtfGlyphInfo>
{
  public int Compare(OtfGlyphInfo otg1, OtfGlyphInfo otg2)
  {
    IndicGlyphInfo indicGlyphInfo1 = otg1 is IndicGlyphInfo && otg2 is IndicGlyphInfo ? (IndicGlyphInfo) otg1 : throw new InvalidOperationException();
    IndicGlyphInfo indicGlyphInfo2 = (IndicGlyphInfo) otg2;
    if (indicGlyphInfo1.Position == indicGlyphInfo2.Position)
      return 0;
    return indicGlyphInfo1.Position < indicGlyphInfo2.Position ? -1 : 1;
  }
}
