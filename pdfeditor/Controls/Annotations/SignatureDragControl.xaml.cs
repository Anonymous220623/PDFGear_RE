// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Annotations.SignatureDragControl
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.BasicTypes;
using pdfeditor.Controls.Annotations.Holders;
using pdfeditor.Controls.Signature;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using pdfeditor.Views;
using PDFKit;
using PDFKit.Utils;
using PDFKit.Utils.StampUtils;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Controls.Annotations;

public partial class SignatureDragControl : 
  UserControl,
  IAnnotationControl<PdfStampAnnotation>,
  IAnnotationControl,
  IComponentConnector
{
  public static List<PdfStampAnnotation> ApplySignatures;
  public static readonly DependencyProperty IsApplyProperty = DependencyProperty.Register(nameof (IsApply), typeof (bool), typeof (SignatureDragControl), new PropertyMetadata((object) false));
  internal Canvas LayoutRoot;
  internal System.Windows.Shapes.Rectangle AnnotationDrag;
  internal ResizeView DragResizeView;
  internal Border Border1;
  internal StackPanel OperationPanel;
  internal Button Btn_Embed;
  internal Button Btn_Embed_InBatch;
  internal Button Btn_Delete;
  internal Button Btn_Apply;
  internal Button Btn_Delete_InBatch;
  private bool _contentLoaded;

  public bool IsApply
  {
    get => (bool) this.GetValue(SignatureDragControl.IsApplyProperty);
    set => this.SetValue(SignatureDragControl.IsApplyProperty, (object) value);
  }

  public SignatureDragControl(PdfStampAnnotation annot, IAnnotationHolder holder)
  {
    this.InitializeComponent();
    this.Annotation = annot;
    this.Holder = holder;
    SignatureDragControl.ApplySignatures = new List<PdfStampAnnotation>();
    this.Loaded += new RoutedEventHandler(this.SignatureDragControl_Loaded);
  }

  private void SignatureDragControl_Loaded(object sender, RoutedEventArgs e)
  {
    this.OnPageClientBoundsChanged();
    if (!this.Annotation.Dictionary.ContainsKey("ApplyId"))
      return;
    this.IsApply = this.Annotation.Dictionary["ApplyId"].As<PdfTypeString>().UnicodeString.Trim().Length > 0;
  }

  public PdfStampAnnotation Annotation { get; }

  public IAnnotationHolder Holder { get; }

  public AnnotationCanvas ParentCanvas => (AnnotationCanvas) this.Parent;

  PdfAnnotation IAnnotationControl.Annotation => (PdfAnnotation) this.Annotation;

  public void OnPageClientBoundsChanged()
  {
    object dataContext = this.DataContext;
    Rect clientRect;
    if (!this.ParentCanvas.PdfViewer.TryGetClientRect(this.Annotation.Page.PageIndex, this.Annotation.GetRECT(), out clientRect))
      return;
    this.AnnotationDrag.Width = clientRect.Width;
    this.AnnotationDrag.Height = clientRect.Height;
    this.LayoutRoot.Width = clientRect.Width;
    this.LayoutRoot.Height = clientRect.Height;
    Canvas.SetLeft((UIElement) this, clientRect.Left);
    Canvas.SetTop((UIElement) this, clientRect.Top);
    this.ResetDraggers();
  }

  public bool OnPropertyChanged(string propertyName) => false;

  private void ResetDraggers()
  {
    this.DragResizeView.Width = this.LayoutRoot.ActualWidth;
    this.DragResizeView.Height = this.LayoutRoot.ActualHeight;
  }

  private void ResizeView_ResizeDragStarted(object sender, ResizeViewResizeDragStartedEventArgs e)
  {
    if (e.Operation == ResizeViewOperation.Move)
    {
      this.DragResizeView.DragPlaceholderFill = (System.Windows.Media.Brush) new SolidColorBrush(System.Windows.Media.Color.FromArgb((byte) 51, (byte) 0, (byte) 122, (byte) 204));
      this.DragResizeView.BorderBrush = (System.Windows.Media.Brush) System.Windows.Media.Brushes.Transparent;
    }
    else
    {
      this.DragResizeView.DragPlaceholderFill = (System.Windows.Media.Brush) System.Windows.Media.Brushes.Transparent;
      this.DragResizeView.BorderBrush = (System.Windows.Media.Brush) new SolidColorBrush(System.Windows.Media.Color.FromArgb((byte) 51, (byte) 0, (byte) 122, (byte) 204));
    }
  }

  private async void ResizeView_ResizeDragCompleted(object sender, ResizeViewResizeDragEventArgs e)
  {
    SignatureDragControl element = this;
    element.DragResizeView.BorderBrush = (System.Windows.Media.Brush) System.Windows.Media.Brushes.Transparent;
    Canvas.SetLeft((UIElement) element.AnnotationDrag, 0.0);
    Canvas.SetTop((UIElement) element.AnnotationDrag, 0.0);
    if (!(element.DataContext is MainViewModel dataContext))
      return;
    double left = Canvas.GetLeft((UIElement) element);
    double top = Canvas.GetTop((UIElement) element);
    element.LayoutRoot.Width = e.NewSize.Width;
    element.LayoutRoot.Height = e.NewSize.Height;
    double length1 = left + e.OffsetX;
    double length2 = top + e.OffsetY;
    Canvas.SetLeft((UIElement) element, length1);
    Canvas.SetTop((UIElement) element, length2);
    // ISSUE: explicit non-virtual call
    PdfViewer pdfViewer = __nonvirtual (element.ParentCanvas).PdfViewer;
    // ISSUE: explicit non-virtual call
    PdfPage page = __nonvirtual (element.Annotation)?.Page;
    if (pdfViewer == null || page == null)
      return;
    // ISSUE: explicit non-virtual call
    PdfPageObjectsCollection normalAppearance = __nonvirtual (element.Annotation).NormalAppearance;
    PdfImageObject pdfImageObject = normalAppearance != null ? normalAppearance.OfType<PdfImageObject>().FirstOrDefault<PdfImageObject>() : (PdfImageObject) null;
    FS_RECTF? newRect = element.GetNewRect();
    if (newRect.HasValue)
    {
      PdfTypeArray pdfTypeArray = PdfTypeArray.Create();
      pdfTypeArray.AddReal(newRect.Value.Width);
      pdfTypeArray.AddReal(newRect.Value.Height);
      // ISSUE: explicit non-virtual call
      __nonvirtual (element.Annotation).Dictionary["ChangeSize"] = (PdfTypeBase) pdfTypeArray;
      // ISSUE: explicit non-virtual call
      using (dataContext.OperationManager.TraceAnnotationChange(__nonvirtual (element.Annotation).Page))
      {
        // ISSUE: explicit non-virtual call
        __nonvirtual (element.Annotation).Opacity = 1f;
        // ISSUE: explicit non-virtual call
        __nonvirtual (element.Annotation).Rectangle = newRect.Value;
      }
    }
    dataContext.IsSaveBySignature = true;
    if (pdfImageObject != null)
    {
      await page.TryRedrawPageAsync();
      // ISSUE: reference to a compiler-generated method
      element.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) new Action(element.\u003CResizeView_ResizeDragCompleted\u003Eb__21_0));
    }
    else
    {
      // ISSUE: explicit non-virtual call
      __nonvirtual (element.Annotation).TryRedrawAnnotation();
      // ISSUE: explicit non-virtual call
      __nonvirtual (element.OnPageClientBoundsChanged());
    }
  }

  public static WriteableBitmap BitmapToWriteableBitmap(Bitmap src, int newwidth, int newheight)
  {
    WriteableBitmap dst = SignatureDragControl.CreateCompatibleWriteableBitmap(src);
    System.Drawing.Imaging.PixelFormat srcPixelFormat = src.PixelFormat;
    if (dst == null)
    {
      dst = new WriteableBitmap(newwidth, newheight, 96.0, 96.0, PixelFormats.Bgra32, (BitmapPalette) null);
      srcPixelFormat = System.Drawing.Imaging.PixelFormat.Format32bppArgb;
    }
    SignatureDragControl.BitmapCopyToWriteableBitmap(src, dst, new System.Drawing.Rectangle(0, 0, newwidth, newheight), 0, 0, srcPixelFormat);
    return dst;
  }

  public static WriteableBitmap CreateCompatibleWriteableBitmap(Bitmap src)
  {
    System.Windows.Media.PixelFormat pixelFormat;
    switch (src.PixelFormat)
    {
      case System.Drawing.Imaging.PixelFormat.Format16bppRgb555:
        pixelFormat = PixelFormats.Bgr555;
        break;
      case System.Drawing.Imaging.PixelFormat.Format16bppRgb565:
        pixelFormat = PixelFormats.Bgr565;
        break;
      case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
        pixelFormat = PixelFormats.Bgr24;
        break;
      case System.Drawing.Imaging.PixelFormat.Format32bppRgb:
        pixelFormat = PixelFormats.Bgr32;
        break;
      case System.Drawing.Imaging.PixelFormat.Format32bppPArgb:
        pixelFormat = PixelFormats.Pbgra32;
        break;
      case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
        pixelFormat = PixelFormats.Bgra32;
        break;
      default:
        return (WriteableBitmap) null;
    }
    return new WriteableBitmap(src.Width, src.Height, 0.0, 0.0, pixelFormat, (BitmapPalette) null);
  }

  public static void BitmapCopyToWriteableBitmap(
    Bitmap src,
    WriteableBitmap dst,
    System.Drawing.Rectangle srcRect,
    int destinationX,
    int destinationY,
    System.Drawing.Imaging.PixelFormat srcPixelFormat)
  {
    BitmapData bitmapdata = src.LockBits(new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), src.Size), ImageLockMode.ReadOnly, srcPixelFormat);
    dst.WritePixels(new Int32Rect(srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height), bitmapdata.Scan0, bitmapdata.Height * bitmapdata.Stride, bitmapdata.Stride, destinationX, destinationY);
    src.UnlockBits(bitmapdata);
  }

  public static BitmapSource GetBitmapSource(Bitmap bmp)
  {
    using (MemoryStream bitmapStream = new MemoryStream())
    {
      bmp.Save((Stream) bitmapStream, ImageFormat.Png);
      return (BitmapSource) BitmapFrame.Create((Stream) bitmapStream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
    }
  }

  private FS_RECTF? GetNewRect()
  {
    PdfViewer pdfViewer = this.ParentCanvas?.PdfViewer;
    double left = Canvas.GetLeft((UIElement) this);
    double top = Canvas.GetTop((UIElement) this);
    double width = this.LayoutRoot.Width;
    double height = this.LayoutRoot.Height;
    if (width == 0.0 || height == 0.0)
      return new FS_RECTF?();
    FS_RECTF pageRect;
    return pdfViewer.TryGetPageRect(this.Annotation.Page.PageIndex, new Rect(left, top, width, height), out pageRect) ? new FS_RECTF?(pageRect) : new FS_RECTF?();
  }

  private void LayoutRoot_SizeChanged(object sender, SizeChangedEventArgs e)
  {
    this.ResetDraggers();
  }

  private async void Btn_Embed_Click(object sender, RoutedEventArgs e)
  {
    CommomLib.Commom.GAManager.SendEvent("PdfStampAnnotationSignature", "Flatten", "Begin", 1L);
    SignatureEmbedConfirmWin signatureEmbedConfirmWin = new SignatureEmbedConfirmWin(EmbedType.Single);
    signatureEmbedConfirmWin.ShowDialog();
    bool? dialogResult = signatureEmbedConfirmWin.DialogResult;
    bool flag = false;
    if (dialogResult.GetValueOrDefault() == flag & dialogResult.HasValue)
      return;
    try
    {
      CommomLib.Commom.GAManager.SendEvent("PdfStampAnnotationSignature", "Flatten", "Start", 1L);
      MainViewModel vm = Ioc.Default.GetRequiredService<MainViewModel>();
      PDFKit.PdfControl viewer = PDFKit.PdfControl.GetPdfControl(vm.Document);
      PdfObjectExtensions.GetAnnotationHolderManager(viewer);
      this.Annotation.Dictionary["Embed"] = (PdfTypeBase) PdfTypeBoolean.Create(true);
      this.Annotation.DeleteAnnotation();
      vm.PageEditors?.NotifyPageAnnotationChanged(this.Annotation.Page.PageIndex);
      int num = await StampUtil.FlattenAnnotationAsync((PdfAnnotation) this.Annotation) ? 1 : 0;
      vm.SetCanSaveFlag("FlattenSignature", true);
      await viewer.TryRedrawVisiblePageAsync();
      CommomLib.Commom.GAManager.SendEvent("PdfStampAnnotationSignature", "Flatten", "Done", 1L);
      vm = (MainViewModel) null;
      viewer = (PDFKit.PdfControl) null;
    }
    catch (Exception ex)
    {
    }
  }

  private async void Btn_Embed_InBatch_Click(object sender, RoutedEventArgs e)
  {
    CommomLib.Commom.GAManager.SendEvent("PdfStampAnnotationSignature", "BatchFlatten", "Begin", 1L);
    SignatureEmbedConfirmWin signatureEmbedConfirmWin = new SignatureEmbedConfirmWin(EmbedType.InBatch);
    signatureEmbedConfirmWin.ShowDialog();
    bool? dialogResult = signatureEmbedConfirmWin.DialogResult;
    bool flag = false;
    if (dialogResult.GetValueOrDefault() == flag & dialogResult.HasValue)
      return;
    try
    {
      if (!this.Annotation.Dictionary.ContainsKey("ApplyRange") || !this.Annotation.Dictionary.ContainsKey("ApplyId"))
        return;
      CommomLib.Commom.GAManager.SendEvent("PdfStampAnnotationSignature", "BatchFlatten", "Start", 1L);
      PdfTypeBase[] array = this.Annotation.Dictionary["ApplyRange"].As<PdfTypeArray>().ToArray<PdfTypeBase>();
      string applyId = this.Annotation.Dictionary["ApplyId"].As<PdfTypeString>().UnicodeString;
      int[] pageIndex = new int[array.Length];
      for (int index = 0; index < array.Length; ++index)
        pageIndex[index] = (array[index] as PdfTypeNumber).IntValue;
      MainViewModel vm = Ioc.Default.GetRequiredService<MainViewModel>();
      if (this.Annotation.Dictionary.ContainsKey("ImgSource"))
      {
        string unicodeString = this.Annotation.Dictionary["ImgSource"].As<PdfTypeString>().UnicodeString;
      }
      ProgressUtils.ShowProgressBar((Func<ProgressUtils.ProgressAction, Task>) (async c =>
      {
        Progress<double> progress = new Progress<double>();
        progress.ProgressChanged += (EventHandler<double>) ((s, a) => c.Report(a));
        await this.ConvertSignatureObj(vm.Document, pageIndex, applyId, (IProgress<double>) progress);
        c.Complete();
      }), (string) null, (object) pdfeditor.Properties.Resources.WinSignatureFlattenProcess, false, (Window) App.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>());
      PDFKit.PdfControl viewer = PDFKit.PdfControl.GetPdfControl(vm.Document);
      vm.SetCanSaveFlag("FlattenSignature", true);
      await this.Annotation.Page.TryRedrawPageAsync();
      await viewer.TryRedrawVisiblePageAsync();
      CommomLib.Commom.GAManager.SendEvent("PdfStampAnnotationSignature", "BatchFlatten", "Done", 1L);
      viewer = (PDFKit.PdfControl) null;
    }
    catch (Exception ex)
    {
    }
  }

  private async void Btn_Delete_Click(object sender, RoutedEventArgs e)
  {
    CommomLib.Commom.GAManager.SendEvent("PdfStampAnnotationSignature", "Delete", "Count", 1L);
    await PdfObjectExtensions.GetAnnotationHolderManager(PDFKit.PdfControl.GetPdfControl(Ioc.Default.GetRequiredService<MainViewModel>().Document)).DeleteAnnotationAsync((PdfAnnotation) this.Annotation);
  }

  private async void Btn_Delete_InBatch_Click(object sender, RoutedEventArgs e)
  {
    CommomLib.Commom.GAManager.SendEvent("PdfStampAnnotationSignature", "BatchDelete", "Count", 1L);
    SignatureClearConfirmWin signatureClearConfirmWin = new SignatureClearConfirmWin();
    signatureClearConfirmWin.ShowDialog();
    bool? dialogResult = signatureClearConfirmWin.DialogResult;
    bool flag = false;
    MainViewModel vm;
    if (dialogResult.GetValueOrDefault() == flag & dialogResult.HasValue)
    {
      vm = (MainViewModel) null;
    }
    else
    {
      vm = Ioc.Default.GetRequiredService<MainViewModel>();
      await PdfObjectExtensions.GetAnnotationHolderManager(PDFKit.PdfControl.GetPdfControl(vm.Document)).DeleteAnnotationAsync((PdfAnnotation) this.Annotation);
      if (!this.Annotation.Dictionary.ContainsKey("ApplyRange"))
        vm = (MainViewModel) null;
      else if (!this.Annotation.Dictionary.ContainsKey("ApplyId"))
      {
        vm = (MainViewModel) null;
      }
      else
      {
        PdfTypeBase[] array = this.Annotation.Dictionary["ApplyRange"].As<PdfTypeArray>().ToArray<PdfTypeBase>();
        string applyId = this.Annotation.Dictionary["ApplyId"].As<PdfTypeString>().UnicodeString;
        int[] pageIndex = new int[array.Length];
        for (int index = 0; index < array.Length; ++index)
          pageIndex[index] = (array[index] as PdfTypeNumber).IntValue;
        List<PdfStampAnnotation> annotations = this.TraceRemoveAll(pageIndex);
        if (annotations != null && annotations.Count > 0)
          await vm.OperationManager.TraceAnnotationRemoveAsync((System.Collections.Generic.IReadOnlyList<PdfAnnotation>) annotations);
        await this.ReomveSignatureAsync(vm.Document, pageIndex, applyId);
        applyId = (string) null;
        pageIndex = (int[]) null;
        vm = (MainViewModel) null;
      }
    }
  }

  private List<PdfStampAnnotation> TraceRemoveAll(int[] pageIndexs)
  {
    PdfDocument document = Ioc.Default.GetRequiredService<MainViewModel>().Document;
    List<PdfStampAnnotation> pdfStampAnnotationList = new List<PdfStampAnnotation>();
    for (int index = 0; index < pageIndexs.Length; ++index)
    {
      int num = pageIndexs[index];
      IntPtr zero = IntPtr.Zero;
      IntPtr handle = Pdfium.FPDF_LoadPage(document.Handle, num);
      if (handle != IntPtr.Zero)
      {
        PdfAnnotationCollection annots = PdfPage.FromHandle(document, handle, num).Annots;
        List<PdfStampAnnotation> list = annots != null ? annots.OfType<PdfStampAnnotation>().ToList<PdfStampAnnotation>() : (List<PdfStampAnnotation>) null;
        if (list != null && list.Count > 0)
          pdfStampAnnotationList.AddRange((IEnumerable<PdfStampAnnotation>) list);
      }
    }
    return pdfStampAnnotationList;
  }

  private async void Btn_Apply_Click(object sender, RoutedEventArgs e)
  {
    CommomLib.Commom.GAManager.SendEvent("PdfStampAnnotationSignature", "ApplyMultiPages", "Begin", 1L);
    SignatureApplyPageWin signatureApplyPageWin = new SignatureApplyPageWin(this.Annotation.Page.PageIndex);
    if (!signatureApplyPageWin.ShowDialog().GetValueOrDefault())
      ;
    else if (signatureApplyPageWin.ApplyPageIndex == null)
      ;
    else
    {
      MainViewModel vm = Ioc.Default.GetRequiredService<MainViewModel>();
      int[] pageIndexs = signatureApplyPageWin.ApplyPageIndex;
      PdfTypeArray rangeArr = PdfTypeArray.Create();
      ((IEnumerable<int>) pageIndexs).ToList<int>().ForEach((Action<int>) (i => rangeArr.Add((PdfTypeBase) PdfTypeNumber.Create(i))));
      IAnnotationHolder currentHolder = this.Holder.AnnotationCanvas.HolderManager.CurrentHolder;
      string imgSource = this.Annotation.Dictionary.ContainsKey("ImgSource") ? this.Annotation.Dictionary["ImgSource"].As<PdfTypeString>().UnicodeString : string.Empty;
      byte[] imgSource2 = (byte[]) null;
      bool isRemoveBg = this.Annotation.Dictionary.ContainsKey("IsRemoveBg") && this.Annotation.Dictionary["IsRemoveBg"].As<PdfTypeBoolean>().Value;
      PdfTypeBase[] array = this.Annotation.Dictionary.ContainsKey("ChangeSize") ? this.Annotation.Dictionary["ChangeSize"].As<PdfTypeArray>().ToArray<PdfTypeBase>() : (PdfTypeBase[]) null;
      System.Windows.Size changeSize2 = new System.Windows.Size();
      if (array != null)
      {
        float[] numArray = new float[array.Length];
        for (int index = 0; index < array.Length; ++index)
          numArray[index] = (array[index] as PdfTypeNumber).FloatValue;
        if (numArray.Length == 2)
          changeSize2 = new System.Windows.Size((double) numArray[0], (double) numArray[1]);
      }
      this.Annotation.Dictionary["ApplyRange"] = (PdfTypeBase) rangeArr;
      string applyId = Guid.NewGuid().ToString().ToLower();
      this.Annotation.Dictionary["ApplyId"] = (PdfTypeBase) PdfTypeString.Create(applyId);
      this.IsApply = true;
      CommomLib.Commom.GAManager.SendEvent("PdfStampAnnotationSignature", "ApplyMultiPages", "Start", 1L);
      ProgressUtils.ShowProgressBar((Func<ProgressUtils.ProgressAction, Task>) (async c =>
      {
        Progress<double> progress = new Progress<double>();
        progress.ProgressChanged += (EventHandler<double>) ((s, a) => c.Report(a));
        int num = await this.GenerateSignatureAsync(vm.Document, pageIndexs, imgSource, imgSource2, applyId, (IProgress<double>) progress, isRemoveBg, changeSize2) ? 1 : 0;
        c.Complete();
      }), (string) null, (object) pdfeditor.Properties.Resources.WinSignatureFlattenApplying, false, (Window) App.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>());
      await PDFKit.PdfControl.GetPdfControl(vm.Document).TryRedrawVisiblePageAsync();
      if (SignatureDragControl.ApplySignatures.Count > 0)
        await vm.OperationManager.TraceAnnotationInsertAsync((System.Collections.Generic.IReadOnlyList<PdfAnnotation>) SignatureDragControl.ApplySignatures);
      CommomLib.Commom.GAManager.SendEvent("PdfStampAnnotationSignature", "ApplyMultiPages", "Done", 1L);
    }
  }

  private async Task<bool> GenerateSignatureAsync(
    PdfDocument doc,
    int[] pageIndexs,
    string imgSource,
    byte[] imgsSource2,
    string applyId,
    IProgress<double> progress,
    bool isRemoveBg = false,
    System.Windows.Size changeSize = default (System.Windows.Size))
  {
    if (pageIndexs == null || pageIndexs.Length == 0)
      return false;
    Func<PdfPage, PdfStampAnnotation> createFunc2 = this.CreateImageStampFunc2(pageIndexs, imgSource, imgsSource2, applyId, isRemoveBg, changeSize);
    PDFKit.PdfControl pdfControl = PDFKit.PdfControl.GetPdfControl(doc);
    (int num1, int num2) = pdfControl != null ? pdfControl.GetVisiblePageRange() : (-1, -1);
    if (createFunc2 == null)
      return false;
    progress?.Report(0.0);
    SignatureDragControl.ApplySignatures.Clear();
    await Task.Run(CommomLib.Commom.TaskExceptionHelper.ExceptionBoundary((Action) (() =>
    {
      for (int index = 0; index < pageIndexs.Length; ++index)
      {
        int num3 = pageIndexs[index];
        if (num3 != this.Annotation.Page.PageIndex)
        {
          IntPtr num4 = IntPtr.Zero;
          PdfPage page = (PdfPage) null;
          try
          {
            num4 = Pdfium.FPDF_LoadPage(doc.Handle, num3);
            if (num4 != IntPtr.Zero)
            {
              page = PdfPage.FromHandle(doc, num4, num3);
              PdfStampAnnotation pdfStampAnnotation = (PdfStampAnnotation) DispatcherHelper.UIDispatcher.Invoke((Delegate) createFunc2, (object) page);
              SignatureDragControl.ApplySignatures.Add(pdfStampAnnotation);
            }
          }
          finally
          {
            if (page != null && (page.PageIndex > num2 || page.PageIndex < num1))
              PageDisposeHelper.DisposePage(page);
            if (num4 != IntPtr.Zero)
              Pdfium.FPDF_ClosePage(num4);
          }
          progress?.Report(1.0 / (double) pageIndexs.Length * (double) (index + 1));
        }
      }
      progress?.Report(1.0);
    }))).ConfigureAwait(false);
    return true;
  }

  private Func<PdfPage, PdfStampAnnotation> CreateImageStampFunc2(
    int[] pageIndex,
    string imgSource,
    byte[] imgSource2,
    string applyId,
    bool isRemove,
    System.Windows.Size changeSize = default (System.Windows.Size))
  {
    MainViewModel vm = Ioc.Default.GetRequiredService<MainViewModel>();
    return (Func<PdfPage, PdfStampAnnotation>) (p =>
    {
      ImageStampModel imageStampModel = new ImageStampModel()
      {
        ImageFilePath = imgSource
      };
      WriteableBitmap writeableBitmap1 = (WriteableBitmap) null;
      try
      {
        writeableBitmap1 = new WriteableBitmap((BitmapSource) new BitmapImage(new Uri(imgSource))
        {
          CacheOption = BitmapCacheOption.None
        });
      }
      catch (Exception ex)
      {
      }
      imageStampModel.StampImageSource = (BitmapSource) writeableBitmap1;
      PdfPage currentPage = vm.Document.Pages.CurrentPage;
      FS_RECTF effectiveBox = vm.Document.Pages.CurrentPage.GetEffectiveBox(currentPage.Rotation);
      Rect clientRect;
      this.ParentCanvas?.PdfViewer.TryGetClientRect(currentPage.PageIndex, effectiveBox, out clientRect);
      double num = clientRect.Width / (double) effectiveBox.Width;
      System.Windows.Size signaturePageSize = SignatureDragControl.GetSignaturePageSize(writeableBitmap1.Width, writeableBitmap1.Height, effectiveBox);
      System.Windows.Size size1 = new System.Windows.Size(signaturePageSize.Width * num, signaturePageSize.Height * num);
      imageStampModel.ImageWidth = size1.Width;
      imageStampModel.ImageHeight = size1.Height;
      imageStampModel.PageSize = new FS_SIZEF(signaturePageSize.Width, signaturePageSize.Height);
      PdfStampAnnotation imageStampFunc2 = (PdfStampAnnotation) null;
      WriteableBitmap writeableBitmap2 = (WriteableBitmap) null;
      FS_RECTF rect1 = this.Annotation.GetRECT();
      if (imageStampModel?.StampImageSource != null)
        writeableBitmap2 = !(imageStampModel.StampImageSource.Format == PixelFormats.Bgra32) ? new WriteableBitmap((BitmapSource) new FormatConvertedBitmap(imageStampModel.StampImageSource, PixelFormats.Bgra32, (BitmapPalette) null, 0.0)) : new WriteableBitmap(imageStampModel.StampImageSource);
      System.Windows.Size size2 = new System.Windows.Size((double) imageStampModel.PageSize.Width, (double) imageStampModel.PageSize.Height);
      using (PdfBitmap bitmap = new PdfBitmap(writeableBitmap2.PixelWidth, writeableBitmap2.PixelHeight, true))
      {
        int bufferSize = bitmap.Stride * bitmap.Height;
        writeableBitmap2.CopyPixels(new Int32Rect(0, 0, writeableBitmap2.PixelWidth, writeableBitmap2.PixelHeight), bitmap.Buffer, bufferSize, bitmap.Stride);
        PdfImageObject pdfImageObject = PdfImageObject.Create(vm.Document, bitmap, rect1.left, rect1.top - (float) size2.Height);
        FS_MATRIX fsMatrix = new FS_MATRIX((float) size2.Width, 0.0f, 0.0f, (float) size2.Height, rect1.left, rect1.top - (float) size2.Height);
        if (changeSize != new System.Windows.Size())
          fsMatrix = new FS_MATRIX((float) changeSize.Width, 0.0f, 0.0f, (float) changeSize.Height, rect1.left, rect1.top - (float) changeSize.Height);
        pdfImageObject.Matrix = fsMatrix;
        imageStampFunc2 = new PdfStampAnnotation(p, "", rect1.left, rect1.top, FS_COLOR.SteelBlue);
        PdfTypeArray rangeArr = PdfTypeArray.Create();
        ((IEnumerable<int>) pageIndex).ToList<int>().ForEach((Action<int>) (i => rangeArr.Add((PdfTypeBase) PdfTypeNumber.Create(i))));
        if (isRemove)
        {
          pdfImageObject.BlendMode = BlendTypes.FXDIB_BLEND_MULTIPLY;
          imageStampFunc2.Dictionary["IsRemoveBg"] = (PdfTypeBase) PdfTypeBoolean.Create(true);
        }
        imageStampFunc2.Dictionary["ApplyRange"] = (PdfTypeBase) rangeArr;
        imageStampFunc2.Dictionary["ApplyId"] = (PdfTypeBase) PdfTypeString.Create(applyId);
        imageStampFunc2.Flags |= AnnotationFlags.Print;
        imageStampFunc2.NormalAppearance.Clear();
        imageStampFunc2.NormalAppearance.Add((PdfPageObject) pdfImageObject);
        imageStampFunc2.Subject = "Signature";
        imageStampFunc2.ModificationDate = DateTimeOffset.Now.ToModificationDateString();
        imageStampFunc2.CreationDate = imageStampFunc2.ModificationDate;
        imageStampFunc2.Text = AnnotationAuthorUtil.GetAuthorName();
        imageStampFunc2.GenerateAppearance(AppearanceStreamModes.Normal);
        FS_RECTF rect2 = imageStampFunc2.GetRECT();
        FS_RECTF rotatedRect = PdfRotateUtils.GetRotatedRect(rect2, new FS_POINTF(rect2.left, rect2.top), p.Rotation);
        if (p.Rotation != PageRotate.Normal)
          imageStampFunc2.Dictionary["Rotate"] = (PdfTypeBase) PdfTypeNumber.Create((int) p.Rotation * 90);
        imageStampFunc2.Rectangle = rotatedRect;
        imageStampFunc2.RegenerateAppearancesAdvance();
        if (p.Annots == null)
          p.CreateAnnotations();
        p.Annots.Add((PdfAnnotation) imageStampFunc2);
        vm.IsSaveBySignature = true;
        vm.PageEditors?.NotifyPageAnnotationChanged(p.PageIndex);
      }
      return imageStampFunc2;
    });
  }

  private async Task ReomveSignatureAsync(PdfDocument doc, int[] pageIndexs, string applyId)
  {
    PDFKit.PdfControl viewer;
    if (pageIndexs == null)
      viewer = (PDFKit.PdfControl) null;
    else if (pageIndexs.Length == 0)
    {
      viewer = (PDFKit.PdfControl) null;
    }
    else
    {
      viewer = PDFKit.PdfControl.GetPdfControl(doc);
      PDFKit.PdfControl scrollInfo = viewer;
      (int num1, int num2) = scrollInfo != null ? scrollInfo.GetVisiblePageRange() : (-1, -1);
      for (int p = 0; p < pageIndexs.Length; ++p)
      {
        int num3 = pageIndexs[p];
        IntPtr pageHandle = IntPtr.Zero;
        PdfPage page = (PdfPage) null;
        try
        {
          pageHandle = Pdfium.FPDF_LoadPage(doc.Handle, num3);
          if (pageHandle != IntPtr.Zero)
          {
            page = PdfPage.FromHandle(doc, pageHandle, num3);
            await this.RemoveImageStampFuncAsync(page, applyId);
          }
        }
        finally
        {
          if (page != null && (page.PageIndex > num2 || page.PageIndex < num1))
            PageDisposeHelper.DisposePage(page);
          if (pageHandle != IntPtr.Zero)
            Pdfium.FPDF_ClosePage(pageHandle);
        }
        page = (PdfPage) null;
      }
      if (viewer == null)
      {
        viewer = (PDFKit.PdfControl) null;
      }
      else
      {
        await viewer.TryRedrawVisiblePageAsync();
        viewer = (PDFKit.PdfControl) null;
      }
    }
  }

  private async Task RemoveImageStampFuncAsync(PdfPage page, string applyId)
  {
    await PdfAnnotationExtensions.WaitForAnnotationGenerateAsync();
    MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
    PdfAnnotationCollection annots = page.Annots;
    List<PdfStampAnnotation> list = annots != null ? annots.OfType<PdfStampAnnotation>().Where<PdfStampAnnotation>((Func<PdfStampAnnotation, bool>) (x => x.Subject == "Signature" && x.Dictionary.ContainsKey("ApplyId") && x.Dictionary["ApplyId"].As<PdfTypeString>().UnicodeString == applyId)).ToList<PdfStampAnnotation>() : (List<PdfStampAnnotation>) null;
    if (list == null)
      ;
    else if (list.Count <= 0)
      ;
    else
    {
      foreach (PdfAnnotation annot in list)
        annot.DeleteAnnotation();
      requiredService.PageEditors?.NotifyPageAnnotationChanged(page.PageIndex);
      await page.TryRedrawPageAsync();
    }
  }

  private async Task ConvertSignatureObj(
    PdfDocument doc,
    int[] pageIndexs,
    string applyId,
    IProgress<double> progress)
  {
    if (pageIndexs == null || pageIndexs.Length == 0)
      return;
    progress?.Report(0.0);
    await Task.Run(CommomLib.Commom.TaskExceptionHelper.ExceptionBoundary((Func<Task>) (async () =>
    {
      for (int p = 0; p < pageIndexs.Length; ++p)
      {
        int num = pageIndexs[p];
        IntPtr handle = Pdfium.FPDF_LoadPage(doc.Handle, num);
        if (handle != IntPtr.Zero)
          await this.AddEmbedSignatureObjAsync(PdfPage.FromHandle(doc, handle, num), applyId);
        progress?.Report(1.0 / (double) pageIndexs.Length * (double) (p + 1));
      }
      progress?.Report(1.0);
    }))).ConfigureAwait(false);
  }

  private async Task AddEmbedSignatureObjAsync(PdfPage page, string applyId)
  {
    MainViewModel vm = Ioc.Default.GetRequiredService<MainViewModel>();
    PdfAnnotationCollection annots = page.Annots;
    List<PdfStampAnnotation> imgStamps = annots != null ? annots.OfType<PdfStampAnnotation>().Where<PdfStampAnnotation>((Func<PdfStampAnnotation, bool>) (x => x.Subject == "Signature" && x.Dictionary.ContainsKey("ApplyId") && x.Dictionary["ApplyId"].As<PdfTypeString>().UnicodeString == applyId)).ToList<PdfStampAnnotation>() : (List<PdfStampAnnotation>) null;
    if (imgStamps == null)
      imgStamps = (List<PdfStampAnnotation>) null;
    else if (imgStamps.Count <= 0)
    {
      imgStamps = (List<PdfStampAnnotation>) null;
    }
    else
    {
      for (int i = 0; i < imgStamps.Count; ++i)
      {
        PdfStampAnnotation annot = imgStamps[i];
        annot.Dictionary["Embed"] = (PdfTypeBase) PdfTypeBoolean.Create(true);
        annot.DeleteAnnotation();
        Application.Current.Dispatcher.Invoke((Action) (() => vm.PageEditors?.NotifyPageAnnotationChanged(page.PageIndex)));
        int num = await StampUtil.FlattenAnnotationAsync((PdfAnnotation) annot) ? 1 : 0;
      }
      imgStamps = (List<PdfStampAnnotation>) null;
    }
  }

  private static System.Windows.Size GetSignaturePageSize(
    double bitmapWidth,
    double bitmapHeight,
    FS_RECTF pageBounds)
  {
    System.Windows.Size signaturePageSize = new System.Windows.Size(bitmapWidth * 96.0 / 72.0, bitmapHeight * 96.0 / 72.0);
    if (signaturePageSize.Width != 200.0)
    {
      double num = 200.0 / signaturePageSize.Width;
      signaturePageSize = new System.Windows.Size(200.0, signaturePageSize.Height * num);
    }
    if (signaturePageSize.Height > (double) pageBounds.Height / 2.0)
    {
      double num = signaturePageSize.Height / ((double) pageBounds.Height / 2.0);
      signaturePageSize = new System.Windows.Size(signaturePageSize.Width / num, (double) pageBounds.Height / 2.0);
    }
    return signaturePageSize;
  }

  public static byte[] intToBytes(int[] src, int offset)
  {
    byte[] bytes = new byte[src.Length * 4];
    for (int index = 0; index < src.Length; ++index)
    {
      bytes[offset + 3] = (byte) (src[index] >> 24 & (int) byte.MaxValue);
      bytes[offset + 2] = (byte) (src[index] >> 16 /*0x10*/ & (int) byte.MaxValue);
      bytes[offset + 1] = (byte) (src[index] >> 8 & (int) byte.MaxValue);
      bytes[offset] = (byte) (src[index] & (int) byte.MaxValue);
      offset += 4;
    }
    return bytes;
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/annotations/signaturedragcontrol.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  internal Delegate _CreateDelegate(Type delegateType, string handler)
  {
    return Delegate.CreateDelegate(delegateType, (object) this, handler);
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.LayoutRoot = (Canvas) target;
        this.LayoutRoot.SizeChanged += new SizeChangedEventHandler(this.LayoutRoot_SizeChanged);
        break;
      case 2:
        this.AnnotationDrag = (System.Windows.Shapes.Rectangle) target;
        break;
      case 3:
        this.DragResizeView = (ResizeView) target;
        break;
      case 4:
        this.Border1 = (Border) target;
        break;
      case 5:
        this.OperationPanel = (StackPanel) target;
        break;
      case 6:
        this.Btn_Embed = (Button) target;
        this.Btn_Embed.Click += new RoutedEventHandler(this.Btn_Embed_Click);
        break;
      case 7:
        this.Btn_Embed_InBatch = (Button) target;
        this.Btn_Embed_InBatch.Click += new RoutedEventHandler(this.Btn_Embed_InBatch_Click);
        break;
      case 8:
        this.Btn_Delete = (Button) target;
        this.Btn_Delete.Click += new RoutedEventHandler(this.Btn_Delete_Click);
        break;
      case 9:
        this.Btn_Apply = (Button) target;
        this.Btn_Apply.Click += new RoutedEventHandler(this.Btn_Apply_Click);
        break;
      case 10:
        this.Btn_Delete_InBatch = (Button) target;
        this.Btn_Delete_InBatch.Click += new RoutedEventHandler(this.Btn_Delete_InBatch_Click);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
