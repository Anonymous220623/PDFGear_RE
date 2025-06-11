// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Endnote
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class Endnote
{
  private WTextBody m_separator;
  private WTextBody m_continuationSeparator;
  private WTextBody m_continuationNotice;
  private WordDocument m_ownerDoc;

  public WTextBody Separator
  {
    get
    {
      if (this.m_separator == null || this.m_separator.ChildEntities.Count == 0 && !this.m_ownerDoc.IsOpening && !this.m_ownerDoc.IsCloning)
      {
        this.m_separator = new WTextBody(this.m_ownerDoc, (Entity) null);
        this.m_separator.AddParagraph().AppendText('\u0003'.ToString()).CharacterFormat.Special = true;
      }
      return this.m_separator;
    }
    set
    {
      this.m_separator = value;
      if (this.m_separator == null)
        return;
      this.m_separator.SetOwner(this.m_ownerDoc, (OwnerHolder) null);
    }
  }

  public WTextBody ContinuationSeparator
  {
    get
    {
      if (this.m_continuationSeparator == null || this.m_continuationSeparator.ChildEntities.Count == 0 && !this.m_ownerDoc.IsOpening && !this.m_ownerDoc.IsCloning)
      {
        this.m_continuationSeparator = new WTextBody(this.m_ownerDoc, (Entity) null);
        this.m_continuationSeparator.AddParagraph().AppendText('\u0004'.ToString()).CharacterFormat.Special = true;
      }
      return this.m_continuationSeparator;
    }
    set
    {
      this.m_continuationSeparator = value;
      if (this.m_continuationSeparator == null)
        return;
      this.m_continuationSeparator.SetOwner(this.m_ownerDoc, (OwnerHolder) null);
    }
  }

  public WTextBody ContinuationNotice
  {
    get
    {
      if (this.m_continuationNotice == null)
        this.m_continuationNotice = new WTextBody(this.m_ownerDoc, (Entity) null);
      return this.m_continuationNotice;
    }
    set
    {
      this.m_continuationNotice = value;
      if (this.m_continuationNotice == null)
        return;
      this.m_continuationNotice.SetOwner(this.m_ownerDoc, (OwnerHolder) null);
    }
  }

  public Endnote(WordDocument document) => this.m_ownerDoc = document;

  internal Endnote(Endnote endnote)
  {
    this.m_separator = endnote.Separator.Clone() as WTextBody;
    this.m_continuationSeparator = endnote.ContinuationSeparator.Clone() as WTextBody;
    this.m_continuationNotice = endnote.ContinuationNotice.Clone() as WTextBody;
  }

  public Endnote Clone() => new Endnote(this);

  internal void SetOwner(WordDocument document)
  {
    this.m_ownerDoc = document;
    if (this.m_separator != null)
      this.m_separator.SetOwner(this.m_ownerDoc, (OwnerHolder) null);
    if (this.m_continuationSeparator != null)
      this.m_continuationSeparator.SetOwner(this.m_ownerDoc, (OwnerHolder) null);
    if (this.m_continuationNotice == null)
      return;
    this.m_continuationNotice.SetOwner(this.m_ownerDoc, (OwnerHolder) null);
  }

  internal void Close()
  {
    if (this.m_separator != null)
    {
      this.m_separator.Close();
      this.m_separator = (WTextBody) null;
    }
    if (this.m_continuationSeparator != null)
    {
      this.m_continuationSeparator.Close();
      this.m_continuationSeparator = (WTextBody) null;
    }
    if (this.m_continuationNotice != null)
    {
      this.m_continuationNotice.Close();
      this.m_continuationNotice = (WTextBody) null;
    }
    this.m_ownerDoc = (WordDocument) null;
  }
}
