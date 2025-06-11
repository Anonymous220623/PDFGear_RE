// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.Type1FontSource
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class Type1FontSource : FontSource
{
  public override string FontFamily => throw new NotImplementedException();

  public override bool IsBold => throw new NotImplementedException();

  public override bool IsItalic => throw new NotImplementedException();

  public override short Ascender => throw new NotImplementedException();

  public override short Descender => throw new NotImplementedException();

  public BaseType1Font Font { get; private set; }

  public Type1FontSource(byte[] data) => this.Initialize(data);

  public override void GetGlyphName(Glyph glyph)
  {
    ushort intValue = (ushort) glyph.CharId.IntValue;
    glyph.Name = this.Font.GetGlyphName(intValue);
  }

  private void Initialize(byte[] data)
  {
    FontInterpreter fontInterpreter = new FontInterpreter();
    fontInterpreter.Execute(Type1FontReader.StripData(data));
    this.Font = Countable.FirstOrDefault<BaseType1Font>((IEnumerable<BaseType1Font>) fontInterpreter.Fonts.Values);
  }

  public override void GetAdvancedWidth(Glyph glyph)
  {
    glyph.AdvancedWidth = (double) this.Font.GetAdvancedWidth(glyph) / 1000.0;
  }

  public override void GetGlyphOutlines(Glyph glyph, double fontSize)
  {
    this.Font.GetGlyphOutlines(glyph, fontSize);
  }
}
