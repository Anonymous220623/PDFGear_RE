// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Screenshots.DrawControlExtensions
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace pdfeditor.Controls.Screenshots;

public static class DrawControlExtensions
{
  public static void Select(this UIElement element)
  {
    if (element == null)
      return;
    if (element is Border border && border.Child is TextBox)
      border.BorderThickness = new Thickness(1.0);
    else
      AdornerLayer.GetAdornerLayer((Visual) element).Add((Adorner) new SelectedAdorner(element));
  }

  public static void UnSelect(this UIElement element)
  {
    if (element == null)
      return;
    if (element is Border border && border.Child is TextBox)
    {
      border.BorderThickness = new Thickness(0.0);
      Keyboard.ClearFocus();
      element.Focus();
    }
    else
    {
      AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer((Visual) element);
      if (adornerLayer == null)
        return;
      Adorner[] adorners = adornerLayer.GetAdorners(element);
      if (adorners == null)
        return;
      for (int index = adorners.Length - 1; index >= 0; --index)
        adornerLayer.Remove(adorners[index]);
    }
  }

  public static void Zoom(this UIElement element, double zoomFactor)
  {
    FrameworkElement element1 = element as FrameworkElement;
    switch (element1)
    {
      case Polyline polyline:
        for (int index = 0; index < polyline.Points.Count; ++index)
        {
          Point point = polyline.Points[index];
          point.X *= zoomFactor;
          point.Y *= zoomFactor;
          polyline.Points[index] = point;
        }
        goto label_7;
      case Border border:
        if (border.Child is TextBox)
          break;
        goto default;
      default:
        element1.Height = element1.ActualHeight * zoomFactor;
        element1.Width = element1.ActualWidth * zoomFactor;
        break;
    }
    double left = Canvas.GetLeft((UIElement) element1);
    double top = Canvas.GetTop((UIElement) element1);
    Canvas.SetLeft((UIElement) element1, left * zoomFactor);
    Canvas.SetTop((UIElement) element1, top * zoomFactor);
label_7:
    if (!(element1.RenderTransform is TransformGroup renderTransform) || renderTransform.Children.Count <= 0)
      return;
    TransformGroup transformGroup = new TransformGroup();
    foreach (Transform child in renderTransform.Children)
    {
      if (child is RotateTransform rotateTransform)
      {
        transformGroup.Children.Add((Transform) rotateTransform);
      }
      else
      {
        TranslateTransform translateTransform = (TranslateTransform) child;
        translateTransform.X *= zoomFactor;
        translateTransform.Y *= zoomFactor;
        transformGroup.Children.Add((Transform) translateTransform);
      }
    }
    element.RenderTransform = (Transform) transformGroup;
  }
}
