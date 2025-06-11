// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.PdfPagePreviewListView
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Controls;

public partial class PdfPagePreviewListView : ListBox
{
  private Panel itemHost;

  static PdfPagePreviewListView()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (PdfPagePreviewListView), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (PdfPagePreviewListView)));
  }

  public PdfPagePreviewListView()
  {
    this.Loaded += new RoutedEventHandler(this.PdfPagePreviewListView_Loaded);
    this.Unloaded += new RoutedEventHandler(this.PdfPagePreviewListView_Unloaded);
    this.SizeChanged += new SizeChangedEventHandler(this.PdfPagePreviewListView_SizeChanged);
  }

  protected virtual double ViewportThreshold => 1870.0;

  protected override bool IsItemItsOwnContainerOverride(object item)
  {
    return item is PdfPagePreviewListViewItem;
  }

  protected override DependencyObject GetContainerForItemOverride()
  {
    return (DependencyObject) new PdfPagePreviewListViewItem();
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.itemHost = (Panel) null;
  }

  private void PdfPagePreviewListView_Loaded(object sender, RoutedEventArgs e)
  {
    if (this.SelectedItem == null)
      return;
    this.ScrollIntoView(this.SelectedItem);
  }

  private void PdfPagePreviewListView_Unloaded(object sender, RoutedEventArgs e)
  {
    this.itemHost = (Panel) null;
  }

  private void PdfPagePreviewListView_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    if (this.itemHost == null && this.GetTemplateChild("ItemPresenter") is ItemsPresenter templateChild && VisualTreeHelper.GetChildrenCount((DependencyObject) templateChild) > 0 && VisualTreeHelper.GetChild((DependencyObject) templateChild, 0) is Panel child)
      this.itemHost = child;
    this.itemHost?.InvalidateMeasure();
  }

  public new void ScrollIntoView(object item)
  {
    if (item == null)
      return;
    ItemContainerGenerator containerGenerator = this.ItemContainerGenerator;
    if (!this.IsVisible || containerGenerator == null)
    {
      base.ScrollIntoView(item);
    }
    else
    {
      FrameworkElement container = containerGenerator.ContainerFromItem(item) as FrameworkElement;
      if (container == null || container.ActualWidth == 0.0 || container.ActualHeight == 0.0)
      {
        VisualStateManager.GoToState((FrameworkElement) this, "Scrolling", true);
        base.ScrollIntoView(item);
        try
        {
          this.Dispatcher.Invoke((Action) (() => VisualStateManager.GoToState((FrameworkElement) this, "NotScrolling", true)), DispatcherPriority.Background);
        }
        catch
        {
        }
      }
      else
      {
        double viewportThreshold = this.ViewportThreshold;
        if (new Rect(-viewportThreshold, -viewportThreshold, this.ActualWidth + viewportThreshold * 2.0, this.ActualHeight + viewportThreshold * 2.0).IntersectsWith(container.TransformToVisual((Visual) this).TransformBounds(new Rect(0.0, 0.0, container.ActualWidth, container.ActualHeight))))
        {
          container?.BringIntoView();
        }
        else
        {
          VisualStateManager.GoToState((FrameworkElement) this, "Scrolling", true);
          container.BringIntoView();
          try
          {
            this.Dispatcher.Invoke((Action) (() =>
            {
              container.BringIntoView();
              VisualStateManager.GoToState((FrameworkElement) this, "NotScrolling", true);
            }), DispatcherPriority.Background);
          }
          catch
          {
          }
        }
      }
    }
  }
}
