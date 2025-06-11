// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WSectionCollection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using System;
using System.Collections;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WSectionCollection : 
  EntityCollection,
  IWSectionCollection,
  IEntityCollectionBase,
  ICollectionBase,
  IEnumerable
{
  private static readonly Type[] DEF_ELEMENT_TYPES = new Type[1]
  {
    typeof (WSection)
  };

  public WSection this[int index] => this.InnerList[index] as WSection;

  protected override Type[] TypesOfElement => WSectionCollection.DEF_ELEMENT_TYPES;

  public WSectionCollection(WordDocument doc)
    : base(doc, (Entity) doc)
  {
  }

  internal WSectionCollection()
    : base((WordDocument) null, (Entity) null)
  {
  }

  public int Add(IWSection section) => this.Add((IEntity) section);

  public int IndexOf(IWSection section) => (section as Entity).Index;

  internal string GetText()
  {
    string empty = string.Empty;
    WParagraph lastParagraph = this.Document.LastParagraph;
    for (int index = 0; index < this.Count; ++index)
    {
      empty += this[index].GetText(lastParagraph);
      if (index < this.Count - 1 && !empty.EndsWith(ControlChar.ParagraphBreak))
        empty += ControlChar.ParagraphBreak;
    }
    return empty;
  }

  protected override OwnerHolder CreateItem(IXDLSContentReader reader)
  {
    return (OwnerHolder) new WSection((IWordDocument) this.Document);
  }

  protected override string GetTagItemName() => "section";
}
