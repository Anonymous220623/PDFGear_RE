// Decompiled with JetBrains decompiler
// Type: pdfeditor.Services.AnnotationTooltipService
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.Wrappers;
using pdfeditor.Controls;
using pdfeditor.Controls.Annotations;
using pdfeditor.Properties;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using PDFKit;
using PDFKit.Utils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Services;

public class AnnotationTooltipService : IDisposable
{
  private PdfViewer pdfViewer;
  private Window window;
  private DispatcherTimer timer;
  private AnnotationToolTip tooltip;
  private bool disposedValue;

  public AnnotationTooltipService(PdfViewer pdfViewer)
  {
    this.pdfViewer = pdfViewer ?? throw new ArgumentNullException(nameof (pdfViewer));
    pdfViewer.Loaded += new RoutedEventHandler(this.PdfViewer_Loaded);
    pdfViewer.Unloaded += new RoutedEventHandler(this.PdfViewer_Unloaded);
    this.tooltip = new AnnotationToolTip();
    this.timer = new DispatcherTimer();
    this.timer.Interval = TimeSpan.FromSeconds(0.5);
    this.timer.Tick += new EventHandler(this.Timer_Tick);
    this.Window = Window.GetWindow((DependencyObject) pdfViewer);
  }

  private Window Window
  {
    get => this.window;
    set
    {
      if (this.window == value)
        return;
      this.timer?.Stop();
      if (this.window != null)
      {
        this.window.PreviewMouseMove -= new MouseEventHandler(this.Window_PreviewMouseMove);
        this.window.GotMouseCapture -= new MouseEventHandler(this.Window_GotMouseCapture);
        this.window.LostMouseCapture -= new MouseEventHandler(this.Window_LostMouseCapture);
      }
      this.window = value;
      if (this.window != null)
      {
        this.window.PreviewMouseMove += new MouseEventHandler(this.Window_PreviewMouseMove);
        this.window.GotMouseCapture += new MouseEventHandler(this.Window_GotMouseCapture);
        this.window.LostMouseCapture += new MouseEventHandler(this.Window_LostMouseCapture);
      }
      this.ResetTimer();
    }
  }

  private void PdfViewer_Loaded(object sender, RoutedEventArgs e)
  {
    this.Window = Window.GetWindow((DependencyObject) this.pdfViewer);
  }

  private void PdfViewer_Unloaded(object sender, RoutedEventArgs e) => this.Window = (Window) null;

  private void Window_PreviewMouseMove(object sender, MouseEventArgs e) => this.ResetTimer();

  private void Window_LostMouseCapture(object sender, MouseEventArgs e) => this.ResetTimer();

  private void Window_GotMouseCapture(object sender, MouseEventArgs e) => this.ResetTimer();

  private void ResetTimer()
  {
    this.HideTooltip();
    if (this.Window != null && Mouse.Captured == null && Mouse.LeftButton == MouseButtonState.Released && Mouse.RightButton == MouseButtonState.Released)
    {
      this.timer.Start();
    }
    else
    {
      if (!this.timer.IsEnabled)
        return;
      this.timer.Stop();
    }
  }

  private void Timer_Tick(object sender, EventArgs e)
  {
    this.timer.Stop();
    this.ShowTooltip();
  }

  private void ShowTooltip()
  {
    if (this.pdfViewer == null)
      return;
    IInputElement directlyOver = Mouse.DirectlyOver;
    switch (directlyOver)
    {
      case PdfViewer _:
      case AnnotationCanvas _:
      case AnnotationFocusControl _:
label_4:
        PdfAnnotation pointAnnotation = this.pdfViewer.GetPointAnnotation(Mouse.GetPosition((IInputElement) this.pdfViewer), out int _);
        string header;
        string content;
        if (!((PdfWrapper) pointAnnotation != (PdfWrapper) null) || !AnnotationTooltipService.TryBuildTooltipContent(pointAnnotation, this.pdfViewer, out header, out content))
          break;
        Rect deviceBounds = pointAnnotation.GetDeviceBounds();
        this.tooltip.PlacementTarget = (UIElement) this.window;
        this.tooltip.PlacementRectangle = deviceBounds;
        this.tooltip.Header = (object) header;
        this.tooltip.Content = (object) content;
        this.tooltip.IsOpen = true;
        this.tooltip.VerticalOffset = 2.0;
        break;
      default:
        if (!AnnotationTooltipService.IsAnnotationControl(directlyOver))
          break;
        goto label_4;
    }
  }

  private static bool IsAnnotationControl(IInputElement element)
  {
    FrameworkElement reference = element as FrameworkElement;
    while (true)
    {
      switch (reference)
      {
        case null:
          goto label_9;
        case IAnnotationControl _:
          goto label_1;
        case AnnotationCanvas _:
          goto label_2;
        case RichTextBox _:
          goto label_3;
        case TextBox _:
          goto label_4;
        default:
          if (!(reference.Parent is FrameworkElement parent))
            parent = VisualTreeHelper.GetParent((DependencyObject) reference) as FrameworkElement;
          reference = parent;
          continue;
      }
    }
label_1:
    return true;
label_2:
    return false;
label_3:
    return false;
label_4:
    return false;
label_9:
    return false;
  }

  public void HideTooltip() => this.tooltip.IsOpen = false;

  private static bool TryBuildTooltipContent(
    PdfAnnotation annot,
    PdfViewer pdfViewer,
    out string header,
    out string content)
  {
    header = (string) null;
    content = (string) null;
    if ((PdfWrapper) annot == (PdfWrapper) null)
      return false;
    bool flag = false;
    MainViewModel dataContext = pdfViewer.DataContext as MainViewModel;
    switch (annot)
    {
      case PdfMarkupAnnotation markupAnnotation:
        PdfPopupAnnotation popup = markupAnnotation.Popup;
        if (popup != null)
          flag = !popup.IsOpen;
        else if (annot is PdfTextAnnotation)
          flag = true;
        else if (annot is PdfFileAttachmentAnnotation attachmentAnnotation)
        {
          try
          {
            string fileName = attachmentAnnotation.FileSpecification?.FileName;
            header = Resources.AnnotationFileAttachment;
            content = fileName;
            return true;
          }
          catch
          {
          }
          return false;
        }
        if (flag && !string.IsNullOrEmpty(markupAnnotation.Contents))
        {
          if (!string.IsNullOrEmpty(markupAnnotation.Text))
          {
            header = markupAnnotation.Text?.Trim() ?? "";
            content = markupAnnotation.Contents?.Trim() ?? "";
          }
          else
          {
            header = "";
            content = annot.Contents?.Trim() ?? "";
          }
          return true;
        }
        break;
      case PdfLinkAnnotation pdfLinkAnnotation:
        if (!dataContext.AnnotationToolbar.LinkButtonModel.IsChecked)
        {
          PdfAction action1 = pdfLinkAnnotation.Link.Action;
          if ((action1 != null ? (action1.ActionType == ActionTypes.Uri ? 1 : 0) : 0) == 0)
          {
            PdfAction action2 = pdfLinkAnnotation.Link.Action;
            if ((action2 != null ? (action2.ActionType == ActionTypes.Application ? 1 : 0) : 0) == 0)
              break;
          }
          string linkUrlOrFileName = LinkAnnotationUtils.GetLinkUrlOrFileName(pdfLinkAnnotation.Link);
          if (linkUrlOrFileName != null)
          {
            content = linkUrlOrFileName;
            return true;
          }
          break;
        }
        break;
    }
    return false;
  }

  protected virtual void Dispose(bool disposing)
  {
    if (this.disposedValue)
      return;
    if (disposing)
    {
      this.Window = (Window) null;
      this.timer?.Stop();
      this.timer = (DispatcherTimer) null;
      if (this.tooltip != null)
      {
        this.tooltip.IsOpen = false;
        this.tooltip = (AnnotationToolTip) null;
      }
      if (this.pdfViewer != null)
      {
        this.pdfViewer.Loaded -= new RoutedEventHandler(this.PdfViewer_Loaded);
        this.pdfViewer.Unloaded -= new RoutedEventHandler(this.PdfViewer_Unloaded);
        this.pdfViewer = (PdfViewer) null;
      }
    }
    this.disposedValue = true;
  }

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }
}
