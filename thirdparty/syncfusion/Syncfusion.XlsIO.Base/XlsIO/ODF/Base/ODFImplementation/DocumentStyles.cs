// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODF.Base.ODFImplementation.DocumentStyles
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.ODF.Base.ODFImplementation;

internal class DocumentStyles
{
  private List<FontFace> m_fontFaceDecls;
  private CommonStyles m_commmonStyles;
  private AutomaticStyles m_autoStyles;
  private MasterPageCollection m_masterStyles;

  internal List<FontFace> FontFaceDecls
  {
    get
    {
      if (this.m_fontFaceDecls == null)
        this.m_fontFaceDecls = new List<FontFace>();
      return this.m_fontFaceDecls;
    }
    set => this.m_fontFaceDecls = value;
  }

  internal CommonStyles CommmonStyles
  {
    get
    {
      if (this.m_commmonStyles == null)
        this.m_commmonStyles = new CommonStyles();
      return this.m_commmonStyles;
    }
    set => this.m_commmonStyles = value;
  }

  internal AutomaticStyles AutoStyles
  {
    get
    {
      if (this.m_autoStyles == null)
        this.m_autoStyles = new AutomaticStyles();
      return this.m_autoStyles;
    }
    set => this.m_autoStyles = value;
  }

  internal MasterPageCollection MasterStyles
  {
    get
    {
      if (this.m_masterStyles == null)
        this.m_masterStyles = new MasterPageCollection();
      return this.m_masterStyles;
    }
    set => this.m_masterStyles = value;
  }
}
