// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.PdfPagePreviewGridViewItem
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace pdfeditor.Controls;

public class PdfPagePreviewGridViewItem : PdfPagePreviewListViewItem
{
  internal static volatile bool draging;
  private Point? lastClickPos;
  private Point? lastClickScreenPos;
  private PdfPagePreviewGridView parentListView;

  static PdfPagePreviewGridViewItem()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (PdfPagePreviewGridViewItem), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (PdfPagePreviewGridViewItem)));
  }

  public PdfPagePreviewGridViewItem()
  {
    this.Loaded += new RoutedEventHandler(this.PdfPagePreviewGridViewItem_Loaded);
    this.Unloaded += new RoutedEventHandler(this.PdfPagePreviewGridViewItem_Unloaded);
  }

  private void PdfPagePreviewGridViewItem_Loaded(object sender, RoutedEventArgs e)
  {
    this.parentListView = ItemsControl.ItemsControlFromItemContainer((DependencyObject) this) as PdfPagePreviewGridView;
  }

  private void PdfPagePreviewGridViewItem_Unloaded(object sender, RoutedEventArgs e)
  {
    this.parentListView = (PdfPagePreviewGridView) null;
  }

  protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    base.OnPreviewMouseLeftButtonDown(e);
    this.lastClickPos = new Point?(e.GetPosition((IInputElement) this));
    this.lastClickScreenPos = new Point?(this.PointToScreen(this.lastClickPos.Value));
  }

  protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    base.OnMouseLeftButtonUp(e);
    this.lastClickPos = new Point?();
    this.lastClickScreenPos = new Point?();
  }

  protected override void OnPreviewMouseMove(MouseEventArgs e)
  {
    base.OnPreviewMouseMove(e);
    if (this.lastClickScreenPos.HasValue && !PdfPagePreviewGridViewItem.draging && e.LeftButton == MouseButtonState.Pressed)
    {
      Point screen = this.PointToScreen(e.GetPosition((IInputElement) this));
      if (Math.Abs(screen.X - this.lastClickScreenPos.Value.X) > 20.0 || Math.Abs(screen.Y - this.lastClickScreenPos.Value.Y) > 20.0)
      {
        this.lastClickPos = new Point?();
        this.lastClickScreenPos = new Point?();
        this.IsSelected = true;
        this.parentListView?.OnItemsDragStart(this);
      }
    }
    if (!PdfPagePreviewGridViewItem.draging)
      return;
    e.Handled = true;
  }

  protected override void OnMouseEnter(MouseEventArgs e)
  {
    Panel.SetZIndex((UIElement) this, 1);
    base.OnMouseEnter(e);
  }

  protected override void OnMouseLeave(MouseEventArgs e)
  {
    Panel.SetZIndex((UIElement) this, 0);
    base.OnMouseLeave(e);
  }

  protected override void OnMouseUp(MouseButtonEventArgs e)
  {
    base.OnMouseUp(e);
    VisualStateManager.GoToState((FrameworkElement) this, "FocusBorderInvisible", true);
  }

  protected override void OnGotFocus(RoutedEventArgs e)
  {
    base.OnGotFocus(e);
    if (this.IsMouseOver && (Mouse.LeftButton == MouseButtonState.Pressed || Mouse.RightButton == MouseButtonState.Pressed || Mouse.MiddleButton == MouseButtonState.Pressed || Mouse.XButton1 == MouseButtonState.Pressed || Mouse.XButton2 == MouseButtonState.Pressed))
      VisualStateManager.GoToState((FrameworkElement) this, "FocusBorderInvisible", true);
    else
      VisualStateManager.GoToState((FrameworkElement) this, "FocusBorderVisible", true);
  }

  protected override void OnLostFocus(RoutedEventArgs e)
  {
    base.OnLostFocus(e);
    VisualStateManager.GoToState((FrameworkElement) this, "FocusBorderInvisible", true);
  }
}
