// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WTextBoxCollection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using System;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WTextBoxCollection(IWordDocument doc) : EntityCollection((WordDocument) doc, (Entity) doc), IWTextBoxCollection
{
  private static readonly Type[] TYPES = new Type[1]
  {
    typeof (WTextBox)
  };

  public IWTextBox this[int index] => (IWTextBox) this.InnerList[index];

  protected override Type[] TypesOfElement => WTextBoxCollection.TYPES;

  public int Add(IWTextBox textBox) => this.InnerList.Add((object) textBox);

  protected override OwnerHolder CreateItem(IXDLSContentReader reader)
  {
    return (OwnerHolder) this.Document.CreateParagraphItem(ParagraphItemType.TextBox);
  }

  protected override string GetTagItemName() => "textboxes";
}
