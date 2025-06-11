// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WParagraphCollection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WParagraphCollection(BodyItemCollection bodyItems) : 
  EntitySubsetCollection((EntityCollection) bodyItems, EntityType.Paragraph),
  IWParagraphCollection,
  IEntityCollectionBase,
  ICollectionBase,
  IEnumerable
{
  public WParagraph this[int index]
  {
    get
    {
      this.ClearIndexes();
      return (WParagraph) this.GetByIndex(index);
    }
  }

  internal ITextBody OwnerTextBody => this.Owner as ITextBody;

  public int Add(IWParagraph paragraph)
  {
    this.Document.EnsureParagraphStyle(paragraph);
    return this.InternalAdd((Entity) paragraph);
  }

  public bool Contains(IWParagraph paragraph) => this.InternalContains((Entity) paragraph);

  public void Insert(int index, IWParagraph paragraph)
  {
    this.Document.EnsureParagraphStyle(paragraph);
    this.InternalInsert(index, (Entity) paragraph);
  }

  public int IndexOf(IWParagraph paragraph) => this.InternalIndexOf((Entity) paragraph);

  public void Remove(IWParagraph paragraph) => this.InternalRemove((Entity) paragraph);

  public void RemoveAt(int index) => this.InternalRemoveAt(index);
}
