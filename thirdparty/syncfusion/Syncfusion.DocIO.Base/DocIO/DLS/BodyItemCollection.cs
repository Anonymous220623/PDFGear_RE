// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.BodyItemCollection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using System;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class BodyItemCollection : EntityCollection
{
  private static readonly Type[] DEF_ELEMENT_TYPES = new Type[4]
  {
    typeof (WTable),
    typeof (WParagraph),
    typeof (BlockContentControl),
    typeof (AlternateChunk)
  };

  internal TextBodyItem this[int index] => (TextBodyItem) base[index];

  protected override Type[] TypesOfElement => BodyItemCollection.DEF_ELEMENT_TYPES;

  public BodyItemCollection(WTextBody body)
    : base(body.Document, (Entity) body)
  {
  }

  internal BodyItemCollection(WordDocument doc)
    : base(doc, (Entity) null)
  {
  }

  protected override string GetTagItemName() => "item";

  protected override OwnerHolder CreateItem(IXDLSContentReader reader)
  {
    switch (reader.GetAttributeValue("type"))
    {
      case "Table":
        return (OwnerHolder) new WTable((IWordDocument) this.Document);
      default:
        return (OwnerHolder) new WParagraph((IWordDocument) this.Document);
    }
  }
}
