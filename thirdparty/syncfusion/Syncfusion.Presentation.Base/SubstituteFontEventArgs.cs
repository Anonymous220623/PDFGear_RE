// Decompiled with JetBrains decompiler
// Type: SubstituteFontEventArgs
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System;
using System.Drawing;
using System.IO;

#nullable disable
public class SubstituteFontEventArgs : EventArgs
{
  private string m_originalFontName;
  private string m_alternateFontName;
  private Stream m_alternateFontStream;
  private FontStyle m_fontStyle;

  public string OriginalFontName
  {
    get => this.m_originalFontName;
    internal set => this.m_originalFontName = value;
  }

  public string AlternateFontName
  {
    get => this.m_alternateFontName;
    set => this.m_alternateFontName = value;
  }

  public Stream AlternateFontStream
  {
    get => this.m_alternateFontStream;
    set => this.m_alternateFontStream = value;
  }

  public FontStyle FontStyle
  {
    get => this.m_fontStyle;
    internal set => this.m_fontStyle = value;
  }

  internal SubstituteFontEventArgs(
    string originalFontName,
    string alternateFontName,
    FontStyle fontStyle)
  {
    this.OriginalFontName = originalFontName;
    this.AlternateFontName = alternateFontName;
    this.FontStyle = fontStyle;
  }
}
