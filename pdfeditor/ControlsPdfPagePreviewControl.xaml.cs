// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.PdfPagePreviewControl
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using HandyControl.Tools;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using pdfeditor.Services;
using pdfeditor.Utils;
using PDFKit.Utils;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Controls;

[TemplatePart(Name = "PART_ContentImage")]
public partial class PdfPagePreviewControl : Control
{
  private const string PART_ContentImageName = "PART_ContentImage";
  private Image contentImage;
  private ScrollViewer scrollViewer;
  private CancellationTokenSource cts;
  private PdfPagePreviewControl.ViewportContext viewportContext;
  private bool imageUpdating;
  public static readonly DependencyProperty ForceImageSizeProperty = DependencyProperty.Register(nameof (ForceImageSize), typeof (bool), typeof (PdfPagePreviewControl), new PropertyMetadata((object) false, (PropertyChangedCallback) ((s, a) =>
  {
    if (object.Equals(a.NewValue, a.OldValue) || !(s is PdfPagePreviewControl pagePreviewControl2) || !pagePreviewControl2.IsImageLoaded)
      return;
    pagePreviewControl2.TrySetElementSize(true);
  })));
  public static readonly DependencyProperty IsImageLoadedProperty = DependencyProperty.Register(nameof (IsImageLoaded), typeof (bool), typeof (PdfPagePreviewControl), new PropertyMetadata((object) false));
  public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register(nameof (Document), typeof (PdfDocument), typeof (PdfPagePreviewControl), new PropertyMetadata((object) null, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is PdfPagePreviewControl pagePreviewControl4))
      return;
    pagePreviewControl4.UpdateViewport(true);
  })));
  public static readonly DependencyProperty RenderActualRotateProperty = DependencyProperty.Register(nameof (RenderActualRotate), typeof (bool), typeof (PdfPagePreviewControl), new PropertyMetadata((object) false, new PropertyChangedCallback(PdfPagePreviewControl.OnRenderActualRotatePropertyChanged)));
  public static readonly DependencyProperty PageIndexProperty = DependencyProperty.Register(nameof (PageIndex), typeof (int), typeof (PdfPagePreviewControl), new PropertyMetadata((object) -1, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is PdfPagePreviewControl pagePreviewControl6))
      return;
    pagePreviewControl6.UpdateViewport(true);
  })));
  public static readonly DependencyProperty ThumbnailHeightProperty = DependencyProperty.Register(nameof (ThumbnailHeight), typeof (int), typeof (PdfPagePreviewControl), new PropertyMetadata((object) 0, new PropertyChangedCallback(PdfPagePreviewControl.OnThumbnailSizePropertyChanged)));
  public static readonly DependencyProperty ThumbnailWidthProperty = DependencyProperty.Register(nameof (ThumbnailWidth), typeof (int), typeof (PdfPagePreviewControl), new PropertyMetadata((object) 0, new PropertyChangedCallback(PdfPagePreviewControl.OnThumbnailSizePropertyChanged)));
  public static readonly DependencyProperty PausedProperty = DependencyProperty.Register(nameof (Paused), typeof (bool), typeof (PdfPagePreviewControl), new PropertyMetadata((object) false, new PropertyChangedCallback(PdfPagePreviewControl.OnPausedPropertyChanged)));

  static PdfPagePreviewControl()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (PdfPagePreviewControl), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (PdfPagePreviewControl)));
  }

  public Image ContentImage
  {
    get => this.contentImage;
    set
    {
      if (this.contentImage == value)
        return;
      ImageSource source = this.contentImage?.Source;
      if (this.contentImage != null)
        this.contentImage.Source = (ImageSource) null;
      this.contentImage = value;
      if (this.contentImage != null)
        this.contentImage.Source = source;
      this.UpdateViewport(true);
    }
  }

  public ScrollViewer ScrollViewer
  {
    get => this.scrollViewer;
    set
    {
      if (this.scrollViewer == value)
        return;
      if (this.scrollViewer != null)
        this.scrollViewer.ScrollChanged -= new ScrollChangedEventHandler(this.ScrollViewer_ScrollChanged);
      this.scrollViewer = value;
      this.viewportContext = new PdfPagePreviewControl.ViewportContext();
      if (this.scrollViewer != null)
        this.scrollViewer.ScrollChanged += new ScrollChangedEventHandler(this.ScrollViewer_ScrollChanged);
      this.UpdateViewport(true);
    }
  }

  private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
  {
    this.UpdateViewport(true);
  }

  public PdfPagePreviewControl()
  {
    this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.PdfPagePreviewControl_IsVisibleChanged);
    this.IsImageLoaded = false;
    VisualStateManager.GoToState((FrameworkElement) this, "ImageLoading", true);
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.ContentImage = this.GetTemplateChild("PART_ContentImage") as Image;
  }

  private void PdfPagePreviewControl_IsVisibleChanged(
    object sender,
    DependencyPropertyChangedEventArgs e)
  {
    if (this.IsVisible)
    {
      this.ScrollViewer = ScrollUtils.GetScrollViewerFromItemContainer(this.GetItemContainer());
    }
    else
    {
      this.ScrollViewer = (ScrollViewer) null;
      this.cts?.Cancel();
    }
  }

  private void UpdateViewport(bool force = false)
  {
    if (DesignerProperties.GetIsInDesignMode((DependencyObject) this) || this.Paused)
      return;
    bool show = false;
    if (this.IsVisible)
    {
      if (this.ForceImageSize)
        this.TrySetElementSize(force);
      if (this.ScrollViewer != null)
      {
        PdfPagePreviewControl.ViewportContext viewportContext = PdfPagePreviewControl.ViewportContext.Create(this.ScrollViewer);
        if (force)
          this.viewportContext = viewportContext;
        else if (!viewportContext.Changed(this.viewportContext))
          return;
        try
        {
          Grid parent = VisualHelper.GetParent<Grid>((DependencyObject) this);
          Rect rect = parent.TransformToVisual((Visual) this.ScrollViewer).TransformBounds(new Rect(0.0, 0.0, parent.ActualWidth, parent.ActualHeight));
          if (rect.Bottom > -100.0)
          {
            if (rect.Top < this.ScrollViewer.ActualHeight + 100.0)
              show = true;
          }
        }
        catch
        {
        }
      }
    }
    this.TryUpdateImageSource(show, force);
  }

  private PageRotate GetRotate(PdfPage page)
  {
    return this.RenderActualRotate ? page.Rotation : PageRotate.Normal;
  }

  private void TrySetElementSize(bool force = false)
  {
    if (this.ContentImage == null || this.ScrollViewer == null || !double.IsNaN(this.ContentImage.Width) && !force)
      return;
    PdfThumbnailService requiredService = Ioc.Default.GetRequiredService<PdfThumbnailService>();
    if (this.Document == null || this.PageIndex == -1 || this.PageIndex >= this.Document.Pages.Count)
      return;
    PdfPage page = this.Document.Pages[this.PageIndex];
    (int width, int height) = this.GetThumbnailSize();
    Size thumbnailImageSize = requiredService.GetThumbnailImageSize(page, this.GetRotate(page), width, height);
    this.ContentImage.Width = thumbnailImageSize.Width;
    this.ContentImage.Height = thumbnailImageSize.Height;
  }

  private async void TryUpdateImageSource(bool show, bool force = false)
  {
    PdfPagePreviewControl pagePreviewControl = this;
    pagePreviewControl.cts?.Cancel();
    pagePreviewControl.cts = new CancellationTokenSource();
    if (pagePreviewControl.ContentImage == null)
      return;
    if (show)
    {
      if (force || pagePreviewControl.ContentImage.Source == null)
        await pagePreviewControl.TryUpdateImageSourceAsync();
      if (!pagePreviewControl.RenderActualRotate)
        return;
      StrongReferenceMessenger.Default.Unregister<ValueChangedMessage<int>, string>((object) pagePreviewControl, "MESSAGE_PAGE_ROTATE_CHANGED");
      StrongReferenceMessenger.Default.Register<ValueChangedMessage<int>, string>((object) pagePreviewControl, "MESSAGE_PAGE_ROTATE_CHANGED", new MessageHandler<object, ValueChangedMessage<int>>(pagePreviewControl.OnPageRotateChanged));
    }
    else
    {
      pagePreviewControl.ContentImage.Source = (ImageSource) null;
      pagePreviewControl.IsImageLoaded = false;
      VisualStateManager.GoToState((FrameworkElement) pagePreviewControl, "ImageLoading", true);
      StrongReferenceMessenger.Default.Unregister<ValueChangedMessage<int>, string>((object) pagePreviewControl, "MESSAGE_PAGE_ROTATE_CHANGED");
    }
  }

  private void OnPageRotateChanged(object recipient, ValueChangedMessage<int> message)
  {
    int? nullable = message?.Value;
    int pageIndex = this.PageIndex;
    if (!(nullable.GetValueOrDefault() == pageIndex & nullable.HasValue) && (message == null || message.Value != -1))
      return;
    this.UpdateViewport(true);
  }

  private async Task TryUpdateImageSourceAsync()
  {
    PdfPagePreviewControl pagePreviewControl = this;
    if (pagePreviewControl.ContentImage == null)
      return;
    await Task.Delay(10).ConfigureAwait(false);
    if (pagePreviewControl.cts == null || pagePreviewControl.cts.IsCancellationRequested)
    {
      pagePreviewControl.cts = (CancellationTokenSource) null;
    }
    else
    {
      // ISSUE: reference to a compiler-generated method
      await pagePreviewControl.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) new Action(pagePreviewControl.\u003CTryUpdateImageSourceAsync\u003Eb__22_0));
    }
  }

  private Color TryGetBackgroundColor()
  {
    Brush background1 = this.Background;
    if (!(this.Background is SolidColorBrush background2))
      return Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
    int a = (int) Math.Min(Math.Max((byte) ((double) background2.Color.A * background2.Opacity), (byte) 0), byte.MaxValue);
    Color color = background2.Color;
    byte r1 = color.R;
    color = background2.Color;
    byte g1 = color.G;
    color = background2.Color;
    byte b1 = color.B;
    int r2 = (int) r1;
    int g2 = (int) g1;
    int b2 = (int) b1;
    return Color.FromArgb((byte) a, (byte) r2, (byte) g2, (byte) b2);
  }

  private (int width, int height) GetThumbnailSize()
  {
    if (this.Document == null || this.Document.Pages.Count <= this.PageIndex)
      return ();
    int num1 = this.ThumbnailWidth;
    int num2 = this.ThumbnailHeight;
    if (num1 < 0)
      num1 = 0;
    if (num2 < 0)
      num2 = 0;
    if (num1 == 0 && num2 == 0)
      num1 = 150;
    PdfPage page = this.Document.Pages[this.PageIndex];
    if (num1 == 0 && this.GetRotate(page) != PageRotate.Normal)
    {
      FS_SIZEF effectiveSize1 = page.GetEffectiveSize(page.Rotation);
      FS_SIZEF effectiveSize2 = page.GetEffectiveSize();
      num1 = (int) ((double) effectiveSize2.Width * (double) num2 * 1.0 / (double) effectiveSize2.Height);
      num2 = (int) ((double) effectiveSize1.Height * (double) num1 * 1.0 / (double) effectiveSize1.Width);
    }
    return (num1, num2);
  }

  public bool ForceImageSize
  {
    get => (bool) this.GetValue(PdfPagePreviewControl.ForceImageSizeProperty);
    set => this.SetValue(PdfPagePreviewControl.ForceImageSizeProperty, (object) value);
  }

  public bool IsImageLoaded
  {
    get => (bool) this.GetValue(PdfPagePreviewControl.IsImageLoadedProperty);
    set => this.SetValue(PdfPagePreviewControl.IsImageLoadedProperty, (object) value);
  }

  public PdfDocument Document
  {
    get => (PdfDocument) this.GetValue(PdfPagePreviewControl.DocumentProperty);
    set => this.SetValue(PdfPagePreviewControl.DocumentProperty, (object) value);
  }

  public bool RenderActualRotate
  {
    get => (bool) this.GetValue(PdfPagePreviewControl.RenderActualRotateProperty);
    set => this.SetValue(PdfPagePreviewControl.RenderActualRotateProperty, (object) value);
  }

  private static void OnRenderActualRotatePropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if ((bool) e.NewValue == (bool) e.OldValue || !(d is PdfPagePreviewControl pagePreviewControl))
      return;
    int pageIndex = pagePreviewControl.PageIndex;
    PdfPageCollection pages = pagePreviewControl.Document?.Pages;
    if (pages == null || pageIndex < 0 || pageIndex >= pages.Count || pages[pageIndex].Rotation == PageRotate.Normal)
      return;
    pagePreviewControl.UpdateViewport();
  }

  public int PageIndex
  {
    get => (int) this.GetValue(PdfPagePreviewControl.PageIndexProperty);
    set => this.SetValue(PdfPagePreviewControl.PageIndexProperty, (object) value);
  }

  public int ThumbnailHeight
  {
    get => (int) this.GetValue(PdfPagePreviewControl.ThumbnailHeightProperty);
    set => this.SetValue(PdfPagePreviewControl.ThumbnailHeightProperty, (object) value);
  }

  public int ThumbnailWidth
  {
    get => (int) this.GetValue(PdfPagePreviewControl.ThumbnailWidthProperty);
    set => this.SetValue(PdfPagePreviewControl.ThumbnailWidthProperty, (object) value);
  }

  private static async void OnThumbnailSizePropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    PdfPagePreviewControl sender = d as PdfPagePreviewControl;
    if (sender == null)
      return;
    if ((int) e.OldValue != 0)
    {
      CancellationTokenSource cts = new CancellationTokenSource();
      sender.cts?.Cancel();
      sender.cts = cts;
      await sender.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) (() =>
      {
        if (cts.IsCancellationRequested || !sender.IsLoaded)
          return;
        sender.UpdateViewport(true);
      }));
    }
    else
      sender.UpdateViewport(true);
  }

  public bool Paused
  {
    get => (bool) this.GetValue(PdfPagePreviewControl.PausedProperty);
    set => this.SetValue(PdfPagePreviewControl.PausedProperty, (object) value);
  }

  private static void OnPausedPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!object.Equals(e.NewValue, e.OldValue) || !(d is PdfPagePreviewControl pagePreviewControl))
      return;
    if ((bool) e.NewValue)
    {
      if (pagePreviewControl.imageUpdating)
        return;
      pagePreviewControl.UpdateViewport();
    }
    else
    {
      pagePreviewControl.cts?.Cancel();
      pagePreviewControl.cts = (CancellationTokenSource) null;
    }
  }

  private DependencyObject GetItemContainer()
  {
    return (DependencyObject) PdfPagePreviewControl.FindParent<PdfPagePreviewListViewItem>((DependencyObject) this);
  }

  private static T FindParent<T>(DependencyObject element, string name = null, int maxDeep = 20) where T : DependencyObject
  {
    if (maxDeep < 0)
      return default (T);
    if (maxDeep == 0)
      return IsResult(element) ? (T) element : default (T);
    for (int index = 0; index < maxDeep; ++index)
    {
      if (IsResult(element))
        return (T) element;
      element = GetParent(element);
    }
    return default (T);

    static DependencyObject GetParent(DependencyObject _element)
    {
      if (_element is FrameworkElement frameworkElement1)
      {
        DependencyObject parent = frameworkElement1.Parent;
        if (parent != null)
          return parent;
      }
      if (_element is FrameworkContentElement frameworkContentElement1)
      {
        DependencyObject parent = frameworkContentElement1.Parent;
        if (parent != null)
          return parent;
      }
      return VisualTreeHelper.GetParent(_element);
    }

    bool IsResult(DependencyObject _element)
    {
      return _element is T obj && (string.IsNullOrEmpty(name) || obj is FrameworkElement frameworkElement2 && frameworkElement2.Name == name || obj is FrameworkContentElement frameworkContentElement2 && frameworkContentElement2.Name == name);
    }
  }

  private static ScrollViewer GetScrollViewerFromItemContainer(DependencyObject container)
  {
    ItemsControl reference = ItemsControl.ItemsControlFromItemContainer(container);
    return reference != null && VisualTreeHelper.GetChildrenCount((DependencyObject) reference) > 0 && VisualTreeHelper.GetChild((DependencyObject) reference, 0) is FrameworkElement child && VisualTreeHelper.GetChildrenCount((DependencyObject) child) > 0 ? VisualTreeHelper.GetChild((DependencyObject) child, 0) as ScrollViewer : (ScrollViewer) null;
  }

  private struct ViewportContext(double offsetY, double viewportHeight)
  {
    public double OffsetY { get; } = offsetY;

    public double ViewportHeight { get; } = viewportHeight;

    public bool Changed(PdfPagePreviewControl.ViewportContext oldContext)
    {
      return Math.Abs(this.OffsetY - oldContext.OffsetY) > 10.0 || Math.Abs(this.ViewportHeight - oldContext.ViewportHeight) > 10.0;
    }

    public static PdfPagePreviewControl.ViewportContext Create(ScrollViewer scrollViewer)
    {
      return scrollViewer == null ? new PdfPagePreviewControl.ViewportContext() : new PdfPagePreviewControl.ViewportContext(scrollViewer.VerticalOffset, scrollViewer.ViewportHeight);
    }
  }
}
