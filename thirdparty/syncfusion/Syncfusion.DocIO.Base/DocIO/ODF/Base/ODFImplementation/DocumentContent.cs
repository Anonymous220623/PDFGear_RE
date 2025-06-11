// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.DocumentContent
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class DocumentContent
{
  private List<FontFace> m_fontFaceDecls;
  private AutomaticStyles m_automaticStyles;
  private OBody m_body;

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

  internal AutomaticStyles AutomaticStyles
  {
    get
    {
      if (this.m_automaticStyles == null)
        this.m_automaticStyles = new AutomaticStyles();
      return this.m_automaticStyles;
    }
    set => this.m_automaticStyles = value;
  }

  internal OBody Body
  {
    get
    {
      if (this.m_body == null)
        this.m_body = new OBody();
      return this.m_body;
    }
    set => this.m_body = value;
  }
}
