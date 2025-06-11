// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.AxleCanvas
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace HandyControl.Controls;

public class AxleCanvas : Canvas
{
  public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof (Orientation), typeof (Orientation), typeof (AxleCanvas), new PropertyMetadata((object) Orientation.Horizontal));

  public Orientation Orientation
  {
    get => (Orientation) this.GetValue(AxleCanvas.OrientationProperty);
    set => this.SetValue(AxleCanvas.OrientationProperty, (object) value);
  }

  protected override Size ArrangeOverride(Size arrangeSize)
  {
    foreach (UIElement internalChild in this.InternalChildren)
    {
      if (internalChild != null)
      {
        double x = 0.0;
        double y = 0.0;
        if (this.Orientation == Orientation.Horizontal)
        {
          x = (arrangeSize.Width - internalChild.DesiredSize.Width) / 2.0;
          double top = Canvas.GetTop(internalChild);
          if (!double.IsNaN(top))
          {
            y = top;
          }
          else
          {
            double bottom = Canvas.GetBottom(internalChild);
            if (!double.IsNaN(bottom))
              y = arrangeSize.Height - internalChild.DesiredSize.Height - bottom;
          }
        }
        else
        {
          y = (arrangeSize.Height - internalChild.DesiredSize.Height) / 2.0;
          double left = Canvas.GetLeft(internalChild);
          if (!double.IsNaN(left))
          {
            x = left;
          }
          else
          {
            double right = Canvas.GetRight(internalChild);
            if (!double.IsNaN(right))
              x = arrangeSize.Width - internalChild.DesiredSize.Width - right;
          }
        }
        internalChild.Arrange(new Rect(new Point(x, y), internalChild.DesiredSize));
      }
    }
    return arrangeSize;
  }
}
