// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.OwnerHolder
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

public abstract class OwnerHolder
{
  protected WordDocument m_doc;
  private OwnerHolder m_owner;

  public WordDocument Document => this.m_owner == null ? this.m_doc : this.m_owner.Document;

  internal OwnerHolder OwnerBase => this.m_owner;

  public OwnerHolder()
  {
  }

  public OwnerHolder(WordDocument doc)
    : this(doc, (OwnerHolder) null)
  {
  }

  public OwnerHolder(WordDocument doc, OwnerHolder owner)
  {
    this.m_doc = doc;
    this.m_owner = owner;
  }

  internal void SetOwner(OwnerHolder owner)
  {
    this.m_owner = owner;
    if (owner == null)
      return;
    this.m_doc = owner.Document;
  }

  internal void SetOwner(WordDocument doc, OwnerHolder owner)
  {
    this.m_owner = owner;
    if (owner == null)
      this.m_doc = doc;
    else
      this.m_doc = owner.Document;
  }

  internal virtual void OnStateChange(object sender)
  {
    if (this.m_owner == null)
      return;
    this.m_owner.OnStateChange(sender);
  }

  internal virtual void Close()
  {
    this.m_doc = (WordDocument) null;
    this.m_owner = (OwnerHolder) null;
  }
}
