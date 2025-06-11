// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.SubstituteFontEventArgs
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class SubstituteFontEventArgs : EventArgs
{
  private string m_originalFontName;
  private string m_alternateFontName;
  private Stream m_alternateFontStream;

  public string OriginalFontName => this.m_originalFontName;

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

  internal SubstituteFontEventArgs(string orginaleFontName, string alternateFontName)
  {
    this.m_originalFontName = orginaleFontName;
    this.m_alternateFontName = alternateFontName;
  }
}
