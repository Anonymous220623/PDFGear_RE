// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.SplitWidgetContainer
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using System;

#nullable disable
namespace Syncfusion.Layouting;

internal class SplitWidgetContainer : IWidgetContainer, IWidget
{
  private IWidgetContainer m_container;
  internal IWidget m_currentChild;
  private int m_firstIndex;
  private IWidgetContainer m_realWidgetConatiner;

  public SplitWidgetContainer(IWidgetContainer container)
  {
    this.m_container = container;
    this.m_currentChild = (IWidget) null;
    this.m_firstIndex = this.m_container.Count;
    this.m_realWidgetConatiner = container is SplitWidgetContainer ? (container as SplitWidgetContainer).RealWidgetContainer : container;
  }

  public SplitWidgetContainer(IWidgetContainer container, IWidget currentChild, int firstIndex)
  {
    if (container == null)
      throw new ArgumentNullException(nameof (container));
    if (currentChild == null)
      throw new ArgumentNullException(nameof (currentChild));
    if (firstIndex < 0)
      throw new ArgumentOutOfRangeException(nameof (firstIndex), (object) firstIndex, "Value can not be less 0");
    this.m_container = container;
    this.m_currentChild = currentChild;
    this.m_firstIndex = firstIndex;
    this.m_realWidgetConatiner = container is SplitWidgetContainer ? (container as SplitWidgetContainer).RealWidgetContainer : container;
  }

  public IWidgetContainer RealWidgetContainer => this.m_realWidgetConatiner;

  public ILayoutInfo LayoutInfo => this.m_container.LayoutInfo;

  void IWidget.InitLayoutInfo()
  {
  }

  void IWidget.InitLayoutInfo(IWidget widget)
  {
    int firstIndex = this.WidgetInnerCollection.InnerList.IndexOf((object) (widget as Entity));
    if (firstIndex > 0)
      this.m_currentChild = (IWidget) new SplitWidgetContainer(this.RealWidgetContainer, widget, firstIndex);
    else
      this.m_currentChild = widget;
  }

  public int Count => this.m_container.Count - this.m_firstIndex;

  public IWidget this[int index]
  {
    get => index == 0 ? this.m_currentChild : this.m_container[index + this.m_firstIndex];
  }

  public EntityCollection WidgetInnerCollection => this.m_container.WidgetInnerCollection;
}
