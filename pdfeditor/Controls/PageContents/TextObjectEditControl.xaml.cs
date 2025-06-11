// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.PageContents.TextObjectEditControl
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using Patagames.Pdf;
using Patagames.Pdf.Net;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using PDFKit.Utils;
using PDFKit.Utils.PageContents;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

#nullable disable
namespace pdfeditor.Controls.PageContents;

public partial class TextObjectEditControl : UserControl, IComponentConnector
{
  private readonly AnnotationCanvas annotationCanvas;
  private readonly int pageIndex;
  private readonly PdfTextObject textObject;
  private FS_RECTF textObjectBounds;
  private Rect deviceRect;
  private Point? dragStartPoint;
  internal ResizeView ResizeControl;
  internal Rectangle PlaceholderRect;
  private bool _contentLoaded;

  public TextObjectEditControl(
    AnnotationCanvas annotationCanvas,
    int pageIndex,
    PdfTextObject textObject)
  {
    this.InitializeComponent();
    this.annotationCanvas = annotationCanvas ?? throw new ArgumentNullException(nameof (annotationCanvas));
    this.pageIndex = pageIndex;
    this.textObject = textObject ?? throw new ArgumentNullException(nameof (textObject));
    this.textObjectBounds = this.textObject.BoundingBox;
    this.Loaded += new RoutedEventHandler(this.TextObjectEditControl_Loaded);
  }

  public int PageIndex => this.pageIndex;

  public PdfTextObject TextObject => this.textObject;

  private void TextObjectEditControl_Loaded(object sender, RoutedEventArgs e)
  {
    this.UpdatePosition();
  }

  public void UpdatePosition()
  {
    if (!this.IsLoaded)
      return;
    try
    {
      if (this.annotationCanvas.PdfViewer.TryGetClientRect(this.pageIndex, this.textObjectBounds, out this.deviceRect))
      {
        this.Visibility = Visibility.Visible;
        this.Width = this.deviceRect.Width;
        this.Height = this.deviceRect.Height;
        Canvas.SetLeft((UIElement) this, this.deviceRect.Left);
        Canvas.SetTop((UIElement) this, this.deviceRect.Top);
        this.ResizeControl.Width = this.deviceRect.Width;
        this.ResizeControl.Height = this.deviceRect.Height;
        Canvas.SetLeft((UIElement) this.ResizeControl, -1.0);
        Canvas.SetTop((UIElement) this.ResizeControl, -1.0);
        Rectangle placeholderRect = this.PlaceholderRect;
        Thickness borderThickness = this.ResizeControl.BorderThickness;
        double left = 1.0 - borderThickness.Left;
        borderThickness = this.ResizeControl.BorderThickness;
        double top = 1.0 - borderThickness.Top;
        borderThickness = this.ResizeControl.BorderThickness;
        double right = 1.0 - borderThickness.Right;
        borderThickness = this.ResizeControl.BorderThickness;
        double bottom = 1.0 - borderThickness.Bottom;
        Thickness thickness = new Thickness(left, top, right, bottom);
        placeholderRect.Margin = thickness;
        this.PlaceholderRect.Width = this.deviceRect.Width;
        this.PlaceholderRect.Height = this.deviceRect.Height;
        return;
      }
    }
    catch
    {
    }
    this.deviceRect = new Rect();
    this.Visibility = Visibility.Collapsed;
  }

  private async void ResizeControl_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
  {
    TextObjectEditControl objectEditControl = this;
    if (e.ChangedButton != MouseButton.Left || !objectEditControl.IsLoaded)
      return;
    e.Handled = true;
    await objectEditControl.annotationCanvas.TextObjectHolder.EditSelectedTextObjectAsync();
  }

  private void ResizeControl_ResizeDragStarted(
    object sender,
    ResizeViewResizeDragStartedEventArgs e)
  {
    if (!this.IsLoaded)
      return;
    Point clientPoint;
    if (e.Operation.HasFlag((Enum) ResizeViewOperation.Move) && this.annotationCanvas.PdfViewer.TryGetClientPoint(this.pageIndex, this.textObject.Location.ToPoint(), out clientPoint))
    {
      GAManager.SendEvent("TextEditor", "ResizeDragStarted", "Count", 1L);
      this.dragStartPoint = new Point?(clientPoint);
      WriteableBitmap pageObjectImage = PageContentUtils.GetPageObjectImage(this.annotationCanvas.PdfViewer.Document.Pages[this.pageIndex], (PdfPageObject) this.textObject, Color.FromArgb((byte) 204, (byte) 0, (byte) 134, (byte) 237));
      if (pageObjectImage != null)
      {
        ImageBrush imageBrush = new ImageBrush();
        imageBrush.ImageSource = (ImageSource) pageObjectImage;
        imageBrush.Stretch = Stretch.Fill;
        this.PlaceholderRect.Fill = (Brush) imageBrush;
      }
      else
        this.PlaceholderRect.Fill = (Brush) null;
    }
    else
      this.dragStartPoint = new Point?();
  }

  private async void ResizeControl_ResizeDragCompleted(
    object sender,
    ResizeViewResizeDragEventArgs e)
  {
    TextObjectEditControl objectEditControl = this;
    if (!objectEditControl.IsLoaded)
      return;
    objectEditControl.PlaceholderRect.Fill = (Brush) null;
    if (!objectEditControl.dragStartPoint.HasValue)
      return;
    Point point = objectEditControl.dragStartPoint.Value;
    Point clientPoint = new Point(point.X + e.OffsetX, point.Y + e.OffsetY);
    Point pagePoint;
    if (!objectEditControl.annotationCanvas.PdfViewer.TryGetPagePoint(objectEditControl.pageIndex, clientPoint, out pagePoint))
      return;
    PdfPage page = objectEditControl.annotationCanvas.PdfViewer.Document.Pages[objectEditControl.pageIndex];
    if (objectEditControl.DataContext is MainViewModel dataContext)
    {
      objectEditControl.annotationCanvas.TextObjectHolder.CancelTextObject();
      PdfTextObject textObject = await dataContext.OperationManager.MoveTextObjectAsync(page, objectEditControl.textObject, pagePoint.ToPdfPoint());
      objectEditControl.annotationCanvas.TextObjectHolder.SelectTextObject(page, textObject);
    }
    page = (PdfPage) null;
  }

  private void ResizeControl_PreviewMouseUp(object sender, MouseButtonEventArgs e)
  {
    if (!this.IsLoaded || e.ChangedButton != MouseButton.Right)
      return;
    e.Handled = true;
    this.annotationCanvas.TryShowTextObjectContextMenu();
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/pagecontents/textobjecteditcontrol.xaml", UriKind.Relative));
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
    if (connectionId != 1)
    {
      if (connectionId == 2)
        this.PlaceholderRect = (Rectangle) target;
      else
        this._contentLoaded = true;
    }
    else
      this.ResizeControl = (ResizeView) target;
  }
}
