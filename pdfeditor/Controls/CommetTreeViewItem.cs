// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.CommetTreeViewItem
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Controls;

internal class CommetTreeViewItem : TreeViewItem
{
  private ToggleButton expander;

  static CommetTreeViewItem()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (CommetTreeViewItem), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (CommetTreeViewItem)));
    EventManager.RegisterClassHandler(typeof (CommetTreeViewItem), FrameworkElement.RequestBringIntoViewEvent, (Delegate) new RequestBringIntoViewEventHandler(CommetTreeViewItem.OnRequestBringIntoView));
  }

  private static void OnRequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
  {
    if (e.TargetObject != sender)
      return;
    e.Handled = true;
    ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer((DependencyObject) sender);
    FrameworkElement child = VisualTreeHelper.GetChildrenCount((DependencyObject) sender) > 0 ? VisualTreeHelper.GetChild((DependencyObject) sender, 0) as FrameworkElement : (FrameworkElement) null;
    if (child == null)
      return;
    Rect targetRectangle = new Rect(0.0, 0.0, child.ActualWidth, child.ActualHeight);
    if (itemsControl != null)
    {
      ref Rect local = ref targetRectangle;
      double left1 = targetRectangle.Left;
      Thickness margin = itemsControl.Margin;
      double left2 = margin.Left;
      double x = left1 - left2;
      double top = targetRectangle.Top;
      double width1 = targetRectangle.Width;
      margin = itemsControl.Margin;
      double left3 = margin.Left;
      double width2 = width1 + left3;
      double height = targetRectangle.Height;
      local = new Rect(x, top, width2, height);
    }
    child.BringIntoView(targetRectangle);
  }

  public CommetTreeViewItem()
  {
    this.SizeChanged += new SizeChangedEventHandler(this.CommetTreeViewItem_SizeChanged);
    this.DataContextChanged += new DependencyPropertyChangedEventHandler(this.CommetTreeViewItem_DataContextChanged);
  }

  private ToggleButton Expander
  {
    get => this.expander;
    set
    {
      if (this.expander == value)
        return;
      if (this.expander != null)
        this.expander.SizeChanged -= new SizeChangedEventHandler(this.OnChildSizeChanged);
      this.expander = value;
      if (this.expander == null)
        return;
      this.expander.SizeChanged += new SizeChangedEventHandler(this.OnChildSizeChanged);
    }
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.Expander = this.GetTemplateChild("Expander") as ToggleButton;
  }

  private void CommetTreeViewItem_DataContextChanged(
    object sender,
    DependencyPropertyChangedEventArgs e)
  {
  }

  private void OnChildSizeChanged(object sender, SizeChangedEventArgs e)
  {
  }

  private void CommetTreeViewItem_SizeChanged(object sender, SizeChangedEventArgs e)
  {
  }

  protected override bool IsItemItsOwnContainerOverride(object item) => item is CommetTreeViewItem;

  protected override DependencyObject GetContainerForItemOverride()
  {
    return (DependencyObject) new CommetTreeViewItem();
  }

  protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
  {
    if (element is CommetTreeViewItem commetTreeViewItem && commetTreeViewItem.IsSelected)
      commetTreeViewItem.BringIntoView();
    base.PrepareContainerForItemOverride(element, item);
  }
}
