// Decompiled with JetBrains decompiler
// Type: HandyControl.Interactivity.AdornerContainer
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Interactivity;

public class AdornerContainer(UIElement adornedElement) : Adorner(adornedElement)
{
  private UIElement _child;

  public UIElement Child
  {
    get => this._child;
    set
    {
      if (value == null)
      {
        this.RemoveVisualChild((Visual) this._child);
        this._child = value;
      }
      else
      {
        this.AddVisualChild((Visual) value);
        this._child = value;
      }
    }
  }

  protected override int VisualChildrenCount => this._child != null ? 1 : 0;

  protected override Size ArrangeOverride(Size finalSize)
  {
    this._child?.Arrange(new Rect(finalSize));
    return finalSize;
  }

  protected override Visual GetVisualChild(int index)
  {
    return index == 0 && this._child != null ? (Visual) this._child : base.GetVisualChild(index);
  }
}
