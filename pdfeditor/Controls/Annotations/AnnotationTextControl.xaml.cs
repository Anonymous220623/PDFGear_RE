// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Annotations.AnnotationTextControl
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using pdfeditor.Controls.Annotations.Holders;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using PDFKit;
using PDFKit.Utils;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace pdfeditor.Controls.Annotations;

public partial class AnnotationTextControl : 
  UserControl,
  IAnnotationControl<PdfTextAnnotation>,
  IAnnotationControl,
  IComponentConnector
{
  internal ResizeView DragResizeView;
  private bool _contentLoaded;

  public AnnotationTextControl(PdfTextAnnotation annot, TextAnnotationHolder holder)
  {
    this.InitializeComponent();
    this.Annotation = annot ?? throw new ArgumentNullException(nameof (annot));
    this.Holder = (IAnnotationHolder) (holder ?? throw new ArgumentNullException(nameof (holder)));
    this.Loaded += new RoutedEventHandler(this.AnnotationTextControl_Loaded);
  }

  public PdfTextAnnotation Annotation { get; }

  public IAnnotationHolder Holder { get; }

  public AnnotationCanvas ParentCanvas => this.Parent as AnnotationCanvas;

  PdfAnnotation IAnnotationControl.Annotation => (PdfAnnotation) this.Annotation;

  public void OnPageClientBoundsChanged()
  {
    Rect deviceBounds = this.Annotation.GetDeviceBounds();
    Canvas.SetLeft((UIElement) this, deviceBounds.Left);
    Canvas.SetTop((UIElement) this, deviceBounds.Top);
    this.Width = deviceBounds.Width;
    this.Height = deviceBounds.Height;
    this.DragResizeView.Width = deviceBounds.Width;
    this.DragResizeView.Height = deviceBounds.Height;
  }

  private void AnnotationTextControl_Loaded(object sender, RoutedEventArgs e)
  {
    this.OnPageClientBoundsChanged();
  }

  public bool OnPropertyChanged(string propertyName) => false;

  private void ResizeView_ResizeDragCompleted(object sender, ResizeViewResizeDragEventArgs e)
  {
    if (!(this.DataContext is MainViewModel dataContext))
      return;
    double left = Canvas.GetLeft((UIElement) this);
    double top = Canvas.GetTop((UIElement) this);
    Point point = new Point(left + e.OffsetX, top + e.OffsetY);
    PdfViewer pdfViewer = this.ParentCanvas?.PdfViewer;
    if (pdfViewer == null || !pdfViewer.TryGetPagePoint(this.Annotation.Page.PageIndex, point, out Point _))
      return;
    using (dataContext.OperationManager.TraceAnnotationChange(this.Annotation.Page))
      this.Annotation.TrySetBounds(new Rect(point, new Size(this.DragResizeView.Width, this.DragResizeView.Height)));
    AnnotationHolderManager holderManager = this.ParentCanvas.HolderManager;
    this.Annotation.Page.TryRedrawPageAsync();
    this.ParentCanvas?.PopupHolder.ClearAnnotationPopup();
    this.ParentCanvas?.PopupHolder.InitAnnotationPopup(pdfViewer.CurrentPage);
    PdfPage page = this.Annotation.Page;
    if (!this.Annotation.GetRECT().IntersectsWith(new FS_RECTF(0.0f, page.Height, page.Width, 0.0f)))
      return;
    holderManager.Select((PdfAnnotation) this.Annotation, false);
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/annotations/annotationtextcontrol.xaml", UriKind.Relative));
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
    if (connectionId == 1)
      this.DragResizeView = (ResizeView) target;
    else
      this._contentLoaded = true;
  }
}
