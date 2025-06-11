// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.SimpleStackPanel
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace HandyControl.Controls;

public class SimpleStackPanel : Panel
{
  public static readonly DependencyProperty OrientationProperty = StackPanel.OrientationProperty.AddOwner(typeof (SimpleStackPanel), (PropertyMetadata) new FrameworkPropertyMetadata((object) Orientation.Vertical, FrameworkPropertyMetadataOptions.AffectsMeasure));

  public Orientation Orientation
  {
    get => (Orientation) this.GetValue(SimpleStackPanel.OrientationProperty);
    set => this.SetValue(SimpleStackPanel.OrientationProperty, (object) value);
  }

  protected override Size MeasureOverride(Size constraint)
  {
    Size size = new Size();
    UIElementCollection internalChildren = this.InternalChildren;
    Size availableSize = constraint;
    if (this.Orientation == Orientation.Horizontal)
    {
      availableSize.Width = double.PositiveInfinity;
      int index = 0;
      for (int count = internalChildren.Count; index < count; ++index)
      {
        UIElement uiElement = internalChildren[index];
        if (uiElement != null)
        {
          uiElement.Measure(availableSize);
          Size desiredSize = uiElement.DesiredSize;
          size.Width += desiredSize.Width;
          size.Height = Math.Max(size.Height, desiredSize.Height);
        }
      }
    }
    else
    {
      availableSize.Height = double.PositiveInfinity;
      int index = 0;
      for (int count = internalChildren.Count; index < count; ++index)
      {
        UIElement uiElement = internalChildren[index];
        if (uiElement != null)
        {
          uiElement.Measure(availableSize);
          Size desiredSize = uiElement.DesiredSize;
          size.Width = Math.Max(size.Width, desiredSize.Width);
          size.Height += desiredSize.Height;
        }
      }
    }
    return size;
  }

  protected override Size ArrangeOverride(Size arrangeSize)
  {
    UIElementCollection internalChildren = this.InternalChildren;
    Rect finalRect = new Rect(arrangeSize);
    double num = 0.0;
    if (this.Orientation == Orientation.Horizontal)
    {
      int index = 0;
      for (int count = internalChildren.Count; index < count; ++index)
      {
        UIElement uiElement = internalChildren[index];
        if (uiElement != null)
        {
          finalRect.X += num;
          num = uiElement.DesiredSize.Width;
          finalRect.Width = num;
          finalRect.Height = Math.Max(arrangeSize.Height, uiElement.DesiredSize.Height);
          uiElement.Arrange(finalRect);
        }
      }
    }
    else
    {
      int index = 0;
      for (int count = internalChildren.Count; index < count; ++index)
      {
        UIElement uiElement = internalChildren[index];
        if (uiElement != null)
        {
          finalRect.Y += num;
          num = uiElement.DesiredSize.Height;
          finalRect.Height = num;
          finalRect.Width = Math.Max(arrangeSize.Width, uiElement.DesiredSize.Width);
          uiElement.Arrange(finalRect);
        }
      }
    }
    return arrangeSize;
  }
}
