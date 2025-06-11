// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.ODocument
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class ODocument
{
  private OBody m_body;
  private Dictionary<string, ImageRecord> m_documentImages;
  private List<OListStyle> m_listStyles;
  private List<ODFStyle> m_tocStyles;

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

  internal Dictionary<string, ImageRecord> DocumentImages
  {
    get
    {
      if (this.m_documentImages == null)
        this.m_documentImages = new Dictionary<string, ImageRecord>();
      return this.m_documentImages;
    }
  }

  internal List<OListStyle> ListStyles
  {
    get
    {
      if (this.m_listStyles == null)
        this.m_listStyles = new List<OListStyle>();
      return this.m_listStyles;
    }
  }

  internal List<ODFStyle> TOCStyles
  {
    get
    {
      if (this.m_tocStyles == null)
        this.m_tocStyles = new List<ODFStyle>();
      return this.m_tocStyles;
    }
    set => this.m_tocStyles = value;
  }

  internal void Close()
  {
    if (this.m_body != null)
    {
      this.m_body.Close();
      this.m_body = (OBody) null;
    }
    if (this.m_documentImages != null)
    {
      this.m_documentImages.Clear();
      this.m_documentImages = (Dictionary<string, ImageRecord>) null;
    }
    if (this.m_listStyles != null)
    {
      foreach (OListStyle listStyle in this.m_listStyles)
        listStyle.Close();
      this.m_listStyles.Clear();
      this.m_listStyles = (List<OListStyle>) null;
    }
    if (this.m_tocStyles == null)
      return;
    foreach (ODFStyle tocStyle in this.m_tocStyles)
      tocStyle.Close();
    this.m_tocStyles.Clear();
    this.m_tocStyles = (List<ODFStyle>) null;
  }
}
