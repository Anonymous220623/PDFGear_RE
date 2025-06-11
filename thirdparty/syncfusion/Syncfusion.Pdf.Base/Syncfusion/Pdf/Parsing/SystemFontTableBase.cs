// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontTableBase
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal abstract class SystemFontTableBase
{
  private readonly SystemFontOpenTypeFontSourceBase fontSource;

  internal long Offset { get; set; }

  protected SystemFontOpenTypeFontReader Reader => this.fontSource.Reader;

  protected SystemFontOpenTypeFontSourceBase FontSource => this.fontSource;

  public SystemFontTableBase(SystemFontOpenTypeFontSourceBase fontSource)
  {
    this.fontSource = fontSource;
  }

  public abstract void Read(SystemFontOpenTypeFontReader reader);

  internal virtual void Write(SystemFontFontWriter writer)
  {
  }

  internal virtual void Import(SystemFontOpenTypeFontReader reader)
  {
  }
}
