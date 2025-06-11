// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.SubstituteFontEventArgs
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class SubstituteFontEventArgs : EventArgs
{
  private string m_originalFontName;
  private string m_alternateFontName;
  private Stream m_alternateFontStream;
  private FontStyle m_fontStyle;

  [Obsolete("This property has been deprecated. Use the OriginalFontName property of SubstituteFontEventArgs class to get the original font name which need to be substituted.")]
  public string OrignalFontName
  {
    get => this.m_originalFontName;
    internal set => this.m_originalFontName = value;
  }

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

  public FontStyle FontStyle
  {
    get => this.m_fontStyle;
    internal set => this.m_fontStyle = value;
  }

  public Stream AlternateFontStream
  {
    get => this.m_alternateFontStream;
    set => this.m_alternateFontStream = value;
  }

  internal SubstituteFontEventArgs(
    string orignalFontName,
    string alternateFontName,
    FontStyle fontStyle)
  {
    this.OrignalFontName = orignalFontName;
    this.AlternateFontName = alternateFontName;
    this.OriginalFontName = orignalFontName;
    this.FontStyle = fontStyle;
  }
}
