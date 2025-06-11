// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.Behaviors.PdfViewerPageBindingBehavior
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Xaml.Behaviors;
using Patagames.Pdf.Net.EventArguments;
using PDFKit;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Utils.Behaviors;

public class PdfViewerPageBindingBehavior : Behavior<PdfViewer>
{
  private TimeSpan lastScrollTime;
  private ScrollViewer scrollViewer;
  private bool isFirstChange = true;
  private DispatcherTimer timer = new DispatcherTimer()
  {
    Interval = TimeSpan.FromMilliseconds(50.0)
  };
  public static readonly DependencyProperty PageIndexProperty = DependencyProperty.Register(nameof (PageIndex), typeof (int), typeof (PdfViewerPageBindingBehavior), new PropertyMetadata((object) -1, new PropertyChangedCallback(PdfViewerPageBindingBehavior.OnPageIndexPropertyChanged)));
  public static readonly DependencyProperty IsDebounceEnabledProperty = DependencyProperty.Register(nameof (IsDebounceEnabled), typeof (bool), typeof (PdfViewerPageBindingBehavior), new PropertyMetadata((object) true, new PropertyChangedCallback(PdfViewerPageBindingBehavior.OnIsDebounceEnabledPropertyChanged)));

  protected override void OnAttached()
  {
    base.OnAttached();
    this.AssociatedObject.BeforeDocumentChanged += new EventHandler<DocumentClosingEventArgs>(this.AssociatedObject_BeforeDocumentChanged);
    this.AssociatedObject.CurrentPageChanged += new EventHandler(this.AssociatedObject_CurrentPageChanged);
    this.AssociatedObject.ScrollOwnerChanged += new EventHandler(this.AssociatedObject_ScrollOwnerChanged);
    this.UpdateScrollOwner();
    this.PageIndex = this.AssociatedObject.CurrentIndex;
    this.timer.Tick += new EventHandler(this.Timer_Tick);
  }

  protected override void OnDetaching()
  {
    base.OnDetaching();
    this.timer.Tick -= new EventHandler(this.Timer_Tick);
    this.AssociatedObject.BeforeDocumentChanged -= new EventHandler<DocumentClosingEventArgs>(this.AssociatedObject_BeforeDocumentChanged);
    this.AssociatedObject.CurrentPageChanged -= new EventHandler(this.AssociatedObject_CurrentPageChanged);
    this.AssociatedObject.ScrollOwnerChanged -= new EventHandler(this.AssociatedObject_ScrollOwnerChanged);
    this.UpdateScrollOwner();
  }

  private void AssociatedObject_CurrentPageChanged(object sender, EventArgs e)
  {
    PdfViewer associatedObject = this.AssociatedObject;
    if (associatedObject?.Document == null)
      return;
    if (this.isFirstChange)
    {
      this.isFirstChange = false;
      this.timer.Stop();
      this.PageIndex = associatedObject.CurrentIndex;
    }
    else
      this.timer.Start();
  }

  public int PageIndex
  {
    get => (int) this.GetValue(PdfViewerPageBindingBehavior.PageIndexProperty);
    set => this.SetValue(PdfViewerPageBindingBehavior.PageIndexProperty, (object) value);
  }

  private static void OnPageIndexPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (object.Equals(e.NewValue, e.OldValue) || !(d is PdfViewerPageBindingBehavior pageBindingBehavior))
      return;
    pageBindingBehavior.UpdateViewerPageIndex();
  }

  public bool IsDebounceEnabled
  {
    get => (bool) this.GetValue(PdfViewerPageBindingBehavior.IsDebounceEnabledProperty);
    set => this.SetValue(PdfViewerPageBindingBehavior.IsDebounceEnabledProperty, (object) value);
  }

  private static void OnIsDebounceEnabledPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (object.Equals(e.NewValue, e.OldValue) || !(d is PdfViewerPageBindingBehavior pageBindingBehavior) || (bool) e.NewValue)
      return;
    pageBindingBehavior.timer.Stop();
    pageBindingBehavior.Timer_Tick((object) pageBindingBehavior.timer, EventArgs.Empty);
  }

  private void Timer_Tick(object sender, EventArgs e)
  {
    this.timer.Stop();
    PdfViewer associatedObject = this.AssociatedObject;
    if (associatedObject == null)
      return;
    this.PageIndex = associatedObject.CurrentIndex;
  }

  private void UpdateViewerPageIndex()
  {
    if (this.AssociatedObject == null || this.AssociatedObject.CurrentIndex == this.PageIndex)
      return;
    if (!this.IsFromScroll)
      this.AssociatedObject.ScrollToPage(this.PageIndex);
    this.AssociatedObject.CurrentIndex = this.PageIndex;
  }

  private void UpdateScrollOwner(bool detaching = false)
  {
    if (this.scrollViewer != null)
    {
      this.scrollViewer.ScrollChanged -= new ScrollChangedEventHandler(this.ScrollViewer_ScrollChanged);
      this.scrollViewer = (ScrollViewer) null;
    }
    if (!detaching && this.AssociatedObject?.ScrollOwner != null)
    {
      this.scrollViewer = this.AssociatedObject.ScrollOwner;
      this.scrollViewer.ScrollChanged -= new ScrollChangedEventHandler(this.ScrollViewer_ScrollChanged);
    }
    this.lastScrollTime = new TimeSpan();
  }

  private void AssociatedObject_BeforeDocumentChanged(object sender, DocumentClosingEventArgs e)
  {
    this.isFirstChange = true;
  }

  private void AssociatedObject_ScrollOwnerChanged(object sender, EventArgs e)
  {
    this.UpdateScrollOwner();
  }

  private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
  {
    this.lastScrollTime = TimeSpan.FromTicks(Stopwatch.GetTimestamp());
  }

  private bool IsFromScroll
  {
    get => (TimeSpan.FromTicks(Stopwatch.GetTimestamp()) - this.lastScrollTime).Milliseconds < 10;
  }
}
