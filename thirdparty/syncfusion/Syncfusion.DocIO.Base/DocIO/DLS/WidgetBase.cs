// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WidgetBase
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.Layouting;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public abstract class WidgetBase : Entity, IWidget
{
  internal ILayoutInfo m_layoutInfo;

  ILayoutInfo IWidget.LayoutInfo
  {
    get
    {
      if (this.m_layoutInfo == null)
        this.CreateLayoutInfo();
      return this.m_layoutInfo;
    }
  }

  public WidgetBase(WordDocument doc, Entity owner)
    : base(doc, owner)
  {
  }

  void IWidget.InitLayoutInfo() => this.m_layoutInfo = (ILayoutInfo) null;

  void IWidget.InitLayoutInfo(IWidget widget)
  {
  }

  protected abstract void CreateLayoutInfo();
}
