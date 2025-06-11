// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.VisiblePanelItem
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Windows;

#nullable disable
namespace Syncfusion.Windows.Shared;

internal class VisiblePanelItem
{
  private UIElement _Child;
  private int _Index;

  public VisiblePanelItem(UIElement child, int index)
  {
    if (index < 0)
      return;
    this.Child = child;
    this.Index = index;
  }

  public UIElement Child
  {
    get => this._Child;
    set => this._Child = value;
  }

  public int Index
  {
    get => this._Index;
    set => this._Index = value;
  }

  public override bool Equals(object obj)
  {
    return obj is VisiblePanelItem visiblePanelItem && this.Index == visiblePanelItem.Index;
  }

  public override int GetHashCode() => base.GetHashCode();
}
