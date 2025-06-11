// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontTrueTypeCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontTrueTypeCollection
{
  private readonly SystemFontOpenTypeFontReader reader;
  private SystemFontTCCHeader header;

  internal SystemFontOpenTypeFontReader Reader => this.reader;

  public IEnumerable<SystemFontOpenTypeFontSourceBase> Fonts
  {
    get => (IEnumerable<SystemFontOpenTypeFontSourceBase>) this.header.Fonts;
  }

  public SystemFontTrueTypeCollection(SystemFontOpenTypeFontReader reader) => this.reader = reader;

  public void Initialzie()
  {
    this.header = new SystemFontTCCHeader(this);
    this.header.Read(this.Reader);
  }
}
