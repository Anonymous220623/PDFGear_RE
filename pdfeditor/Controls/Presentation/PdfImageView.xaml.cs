// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Presentation.PdfImageView
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using pdfeditor.Utils;
using PDFKit.Utils;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace pdfeditor.Controls.Presentation;

public partial class PdfImageView : UserControl, IComponentConnector
{
  private CancellationTokenSource cts;
  public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register(nameof (Document), typeof (PdfDocument), typeof (PdfImageView), new PropertyMetadata((object) null, new PropertyChangedCallback(PdfImageView.OnDocumentPropertyChanged)));
  public static readonly DependencyProperty PageIndexProperty = DependencyProperty.Register(nameof (PageIndex), typeof (int), typeof (PdfImageView), new PropertyMetadata((object) -1, new PropertyChangedCallback(PdfImageView.OnPageIndexPropertyChanged)));
  internal Image MainImage;
  private bool _contentLoaded;

  public PdfImageView()
  {
    this.InitializeComponent();
    this.Loaded += new RoutedEventHandler(this.PdfImageView_Loaded);
    this.Unloaded += new RoutedEventHandler(this.PdfImageView_Unloaded);
    this.SizeChanged += new SizeChangedEventHandler(this.PdfImageView_SizeChanged);
    this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.PdfImageView_IsVisibleChanged);
  }

  private void PdfImageView_Loaded(object sender, RoutedEventArgs e) => this.UpdateImage();

  private void PdfImageView_Unloaded(object sender, RoutedEventArgs e)
  {
    this.UpdateImage();
    this.cts = (CancellationTokenSource) null;
  }

  private void PdfImageView_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    this.UpdateImage();
  }

  private async void PdfImageView_IsVisibleChanged(
    object sender,
    DependencyPropertyChangedEventArgs e)
  {
    try
    {
      CancellationTokenSource cts = this.cts;
      await Task.Delay(20, cts != null ? cts.Token : new CancellationToken());
      this.UpdateImage();
    }
    catch (OperationCanceledException ex)
    {
    }
  }

  public PdfDocument Document
  {
    get => (PdfDocument) this.GetValue(PdfImageView.DocumentProperty);
    set => this.SetValue(PdfImageView.DocumentProperty, (object) value);
  }

  private static void OnDocumentPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (e.NewValue == e.OldValue || !(d is PdfImageView pdfImageView))
      return;
    pdfImageView.UpdateImage();
  }

  public int PageIndex
  {
    get => (int) this.GetValue(PdfImageView.PageIndexProperty);
    set => this.SetValue(PdfImageView.PageIndexProperty, (object) value);
  }

  private static void OnPageIndexPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (object.Equals(e.NewValue, e.OldValue) || !(d is PdfImageView pdfImageView))
      return;
    pdfImageView.UpdateImage();
  }

  private async void UpdateImage()
  {
    PdfImageView pdfImageView = this;
    pdfImageView.cts?.Cancel();
    pdfImageView.cts = new CancellationTokenSource();
    if (!pdfImageView.IsLoaded || !pdfImageView.IsVisible)
    {
      pdfImageView.MainImage.Source = (ImageSource) null;
    }
    else
    {
      try
      {
        WriteableBitmap pdfImageAsync = await pdfImageView.GetPdfImageAsync(pdfImageView.PageIndex);
        if (pdfImageAsync == null)
          return;
        pdfImageView.MainImage.Source = (ImageSource) pdfImageAsync;
      }
      catch (OperationCanceledException ex)
      {
      }
    }
  }

  private async Task<WriteableBitmap> GetPdfImageAsync(int pageIndex)
  {
    PdfImageView pdfImageView = this;
    if (pageIndex < 0 || pdfImageView.Document == null || pageIndex >= pdfImageView.Document.Pages.Count || !pdfImageView.IsLoaded || !pdfImageView.IsVisible || pdfImageView.cts == null)
      return (WriteableBitmap) null;
    CancellationToken token = pdfImageView.cts.Token;
    double actualWidth = pdfImageView.ActualWidth;
    double actualHeight = pdfImageView.ActualHeight;
    DpiScale dpiScale = VisualTreeHelper.GetDpi((Visual) pdfImageView);
    if (actualWidth <= 1.0 || actualHeight <= 1.0)
      return (WriteableBitmap) null;
    double widthPixel = actualWidth * dpiScale.PixelsPerDip;
    double heightPixel = actualHeight * dpiScale.PixelsPerDip;
    PdfDocument document = pdfImageView.Document;
    PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(document);
    bool showAnnot = pdfControl == null || pdfControl.IsAnnotationVisible;
    return await Task.Run<WriteableBitmap>(CommomLib.Commom.TaskExceptionHelper.ExceptionBoundary<WriteableBitmap>((Func<Task<WriteableBitmap>>) (async () =>
    {
      try
      {
        token.ThrowIfCancellationRequested();
        IntPtr pageHandle = Pdfium.FPDF_LoadPage(document.Handle, pageIndex);
        if (pageHandle != IntPtr.Zero)
        {
          try
          {
            PageRotate rotation = Pdfium.FPDFPage_GetRotation(pageHandle);
            FS_SIZEF effectiveSize = PdfLocationUtils.GetEffectiveSize(document.Handle, pageHandle, pageIndex, rotation);
            double a1 = widthPixel;
            double a2 = widthPixel / (double) effectiveSize.Width * (double) effectiveSize.Height;
            if (a2 > heightPixel)
            {
              a2 = heightPixel;
              a1 = heightPixel / (double) effectiveSize.Height * (double) effectiveSize.Width;
            }
            using (PdfBitmap pdfBitmap = new PdfBitmap((int) Math.Ceiling(a1), (int) Math.Ceiling(a2), BitmapFormats.FXDIB_Argb))
            {
              pdfBitmap.FillRectEx(0, 0, pdfBitmap.Width, pdfBitmap.Height, -1);
              Pdfium.FPDF_RenderPageBitmap(pdfBitmap.Handle, pageHandle, 0, 0, pdfBitmap.Width, pdfBitmap.Height, PageRotate.Normal, showAnnot ? RenderFlags.FPDF_ANNOT : RenderFlags.FPDF_NONE);
              return await pdfBitmap.ToWriteableBitmapAsync((uint) dpiScale.PixelsPerInchX, (uint) dpiScale.PixelsPerInchX, token);
            }
          }
          finally
          {
            Pdfium.FPDF_ClosePage(pageHandle);
          }
        }
      }
      catch (OperationCanceledException ex)
      {
      }
      return (WriteableBitmap) null;
    })), token);
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/presentation/pdfimageview.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    if (connectionId == 1)
      this.MainImage = (Image) target;
    else
      this._contentLoaded = true;
  }
}
