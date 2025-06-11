// Decompiled with JetBrains decompiler
// Type: pdfconverter.Controls.DragDropAdorner
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

#nullable disable
namespace pdfconverter.Controls;

public class DragDropAdorner : Adorner
{
  private FrameworkElement mDraggedElement;

  public DragDropAdorner(UIElement parent)
    : base(parent)
  {
    this.IsHitTestVisible = false;
    this.mDraggedElement = parent as FrameworkElement;
  }

  protected override void OnRender(DrawingContext drawingContext)
  {
    base.OnRender(drawingContext);
    if (this.mDraggedElement == null)
      return;
    Win32.POINT point1 = new Win32.POINT();
    if (!Win32.GetCursorPos(ref point1))
      return;
    Point point2 = this.PointFromScreen(new Point((double) point1.X, (double) point1.Y));
    this.mDraggedElement.PointToScreen(new Point());
    Rect rectangle = new Rect(point2.X, point2.Y - this.mDraggedElement.ActualHeight / 2.0, this.mDraggedElement.ActualWidth, this.mDraggedElement.ActualHeight);
    drawingContext.PushOpacity(0.5);
    if (this.mDraggedElement.TryFindResource((object) SystemColors.HighlightBrushKey) is Brush resource)
      drawingContext.DrawRectangle(resource, new Pen((Brush) Brushes.Red, 0.0), rectangle);
    drawingContext.DrawRectangle((Brush) new VisualBrush((Visual) this.mDraggedElement), new Pen((Brush) Brushes.Transparent, 0.0), rectangle);
    drawingContext.Pop();
  }
}
