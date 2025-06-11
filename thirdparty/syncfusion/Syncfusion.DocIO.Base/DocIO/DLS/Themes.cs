// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Themes
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections.Generic;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class Themes
{
  private IWordDocument m_document;
  private FormatScheme m_fmtScheme;
  private FontScheme m_fontScheme;
  private Dictionary<string, Color> m_schemeColor;
  private string m_colorSchemeName;
  internal Dictionary<string, Stream> m_docxProps;

  internal Dictionary<string, Color> SchemeColor
  {
    get
    {
      if (this.m_schemeColor == null)
        this.m_schemeColor = new Dictionary<string, Color>();
      return this.m_schemeColor;
    }
  }

  internal FormatScheme FmtScheme
  {
    get
    {
      if (this.m_fmtScheme == null)
        this.m_fmtScheme = new FormatScheme();
      return this.m_fmtScheme;
    }
    set => this.m_fmtScheme = value;
  }

  internal FontScheme FontScheme
  {
    get
    {
      if (this.m_fontScheme == null)
        this.m_fontScheme = new FontScheme();
      return this.m_fontScheme;
    }
    set => this.m_fontScheme = value;
  }

  internal Dictionary<string, Stream> DocxProps
  {
    get
    {
      if (this.m_docxProps == null)
        this.m_docxProps = new Dictionary<string, Stream>();
      return this.m_docxProps;
    }
  }

  internal string ColorSchemeName
  {
    get => this.m_colorSchemeName;
    set => this.m_colorSchemeName = value;
  }

  internal Themes(IWordDocument doc)
  {
    this.m_document = doc;
    this.m_schemeColor = new Dictionary<string, Color>();
    this.m_fmtScheme = new FormatScheme();
    this.m_fontScheme = new FontScheme();
  }

  internal void Close()
  {
    this.m_document = (IWordDocument) null;
    if (this.m_fmtScheme != null)
    {
      this.m_fmtScheme.Close();
      this.m_fmtScheme = (FormatScheme) null;
    }
    if (this.m_fontScheme != null)
    {
      this.m_fontScheme.Close();
      this.m_fontScheme = (FontScheme) null;
    }
    if (this.m_schemeColor == null)
      return;
    this.m_schemeColor.Clear();
    this.m_schemeColor = (Dictionary<string, Color>) null;
  }
}
