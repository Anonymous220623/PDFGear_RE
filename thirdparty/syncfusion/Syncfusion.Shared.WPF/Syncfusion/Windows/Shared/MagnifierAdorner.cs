// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.MagnifierAdorner
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class MagnifierAdorner : Adorner
{
  private Magnifier mMagnifier;
  private Point mMousePoint;

  public MagnifierAdorner(UIElement element, Magnifier magnifier)
    : base(element)
  {
    this.mMagnifier = magnifier;
    this.AddVisualChild((Visual) magnifier);
    magnifier.TargetElementChanging += new CoerceValueCallback(this.Magnifier_TargetElementChanging);
    InputManager.Current.PostProcessInput += new ProcessInputEventHandler(this.Current_PostProcessInput);
    this.UpdateMagnifierViewbox();
  }

  private object Magnifier_TargetElementChanging(DependencyObject d, object baseValue)
  {
    if (baseValue as UIElement != this.mMagnifier.TargetElement)
    {
      this.mMagnifier.TargetElementChanging -= new CoerceValueCallback(this.Magnifier_TargetElementChanging);
      this.RemoveVisualChild((Visual) this.mMagnifier);
    }
    return baseValue;
  }

  private void Current_PostProcessInput(object sender, ProcessInputEventArgs e)
  {
    Point position = Mouse.GetPosition((IInputElement) this);
    if (!(this.mMousePoint != position))
      return;
    this.mMousePoint = position;
    this.UpdateMagnifierViewbox();
    this.InvalidateArrange();
  }

  internal void UpdateMagnifierViewbox()
  {
    this.mMagnifier.Viewbox = new Rect(this.CalculateMagnifierViewboxLocation(), this.mMagnifier.Viewbox.Size);
  }

  private Point CalculateMagnifierViewboxLocation()
  {
    double num1 = 0.0;
    double num2 = 0.0;
    if (this.mMagnifier.ActualTargetElement != null)
    {
      Point position1 = Mouse.GetPosition((IInputElement) this);
      Point position2 = Mouse.GetPosition((IInputElement) this.AdornedElement);
      num1 = position2.X - position1.X;
      num2 = position2.Y - position1.Y;
      if (this.mMagnifier.ActualTargetElement is FrameworkElement actualTargetElement)
      {
        if (actualTargetElement.Parent is Panel parent)
        {
          Point position3 = Mouse.GetPosition((IInputElement) parent);
          num1 += position3.X - position2.X;
          num2 += position3.Y - position2.Y;
        }
        else
        {
          num1 += actualTargetElement.Margin.Left;
          num2 += actualTargetElement.Margin.Top;
        }
      }
    }
    return new Point(this.mMousePoint.X - this.mMagnifier.Viewbox.Width / 2.0 + num1, this.mMousePoint.Y - this.mMagnifier.Viewbox.Height / 2.0 + num2);
  }

  internal void DisconnectMagnifier()
  {
    if (this.mMagnifier == null)
      return;
    this.RemoveVisualChild((Visual) this.mMagnifier);
  }

  protected override Visual GetVisualChild(int index) => (Visual) this.mMagnifier;

  protected override int VisualChildrenCount => 1;

  protected override Size MeasureOverride(Size constraint)
  {
    this.mMagnifier.Measure(constraint);
    return base.MeasureOverride(constraint);
  }

  protected override Size ArrangeOverride(Size finalSize)
  {
    this.mMagnifier.Arrange(new Rect(this.mMousePoint.X - this.mMagnifier.CurrentSize.Width / 2.0, this.mMousePoint.Y - this.mMagnifier.CurrentSize.Height / 2.0, this.mMagnifier.CurrentSize.Width, this.mMagnifier.CurrentSize.Height));
    return base.ArrangeOverride(finalSize);
  }
}
