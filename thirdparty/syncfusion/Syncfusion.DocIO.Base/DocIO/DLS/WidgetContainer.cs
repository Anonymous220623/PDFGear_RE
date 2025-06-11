// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WidgetContainer
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.Layouting;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public abstract class WidgetContainer : WidgetBase, IWidgetContainer, IWidget
{
  public int Count => this.WidgetCollection.Count;

  IWidget IWidgetContainer.this[int index] => this.WidgetCollection[index] as IWidget;

  protected abstract IEntityCollectionBase WidgetCollection { get; }

  public EntityCollection WidgetInnerCollection => this.WidgetCollection as EntityCollection;

  public WidgetContainer(WordDocument doc, Entity owner)
    : base(doc, owner)
  {
  }

  internal override void Close()
  {
    base.Close();
    if (this.m_layoutInfo == null)
      return;
    this.m_layoutInfo = (ILayoutInfo) null;
  }
}
