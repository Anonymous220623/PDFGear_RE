// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.DocProperties
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class DocProperties
{
  private IWordDocument m_document;
  private DocumentVersion m_version;
  private Hyphenation m_hyphenation;

  public bool FormFieldShading
  {
    get => (this.m_document as WordDocument).DOP.FormFieldShading;
    set => (this.m_document as WordDocument).DOP.FormFieldShading = value;
  }

  public DocumentVersion Version => this.m_version;

  public Hyphenation Hyphenation
  {
    get
    {
      if (this.m_hyphenation == null)
        this.m_hyphenation = new Hyphenation(this.m_document);
      return this.m_hyphenation;
    }
  }

  internal DocProperties(IWordDocument document) => this.m_document = document;

  internal void SetVersion(DocumentVersion version) => this.m_version = version;

  internal void Close() => this.m_document = (IWordDocument) null;
}
