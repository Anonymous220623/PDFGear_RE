// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Annotations.AnnotationLineControl
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.Wrappers;
using pdfeditor.Controls.Annotations.Holders;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using PDFKit.Utils;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace pdfeditor.Controls.Annotations;

public partial class AnnotationLineControl : 
  UserControl,
  IAnnotationControl<PdfLineAnnotation>,
  IAnnotationControl,
  IComponentConnector
{
  private FS_POINTF point1;
  private FS_POINTF point2;
  private Color color;
  private float rawThickness;
  private Cursor pointCursor;
  private bool isPressed;
  private FS_POINTF startPoint;
  internal Canvas LayoutRoot;
  internal Line ContentLine;
  internal Line DraggerLine;
  internal Ellipse Point1Rect;
  internal Ellipse Point2Rect;
  private bool _contentLoaded;

  public AnnotationLineControl(PdfLineAnnotation annot, IAnnotationHolder holder)
  {
    this.InitializeComponent();
    this.Annotation = annot;
    this.Holder = holder;
    this.point1 = annot.Line[0];
    this.point2 = annot.Line[1];
    FS_COLOR color = annot.Color;
    int a = (int) (byte) color.A;
    color = annot.Color;
    int r = (int) (byte) color.R;
    color = annot.Color;
    int g = (int) (byte) color.G;
    color = annot.Color;
    int b = (int) (byte) color.B;
    this.color = Color.FromArgb((byte) a, (byte) r, (byte) g, (byte) b);
    PdfBorderStyle lineStyle = annot.LineStyle;
    this.rawThickness = lineStyle != null ? lineStyle.Width : 1f;
    this.ContentLine.DataContext = (object) this;
    this.Foreground = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 24, (byte) 146, byte.MaxValue));
    this.pointCursor = CursorHelper.CreateCursor(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Style\\\\Resources\\\\pointmove.png"));
    this.Point1Rect.Cursor = this.pointCursor;
    this.Point2Rect.Cursor = this.pointCursor;
  }

  public PdfLineAnnotation Annotation { get; }

  public AnnotationCanvas ParentCanvas => (AnnotationCanvas) this.Parent;

  PdfAnnotation IAnnotationControl.Annotation => (PdfAnnotation) this.Annotation;

  public IAnnotationHolder Holder { get; }

  public void OnPageClientBoundsChanged()
  {
    Point? device1 = this.PageToDevice((double) this.point1.X, (double) this.point1.Y);
    Point? device2 = this.PageToDevice((double) this.point2.X, (double) this.point2.Y);
    if (!device1.HasValue || !device2.HasValue)
      return;
    Point point1 = device1.Value;
    Point point2 = device2.Value;
    Point point3 = new Point(Math.Min(point1.X, point2.X), Math.Min(point1.Y, point2.Y));
    Point point4 = new Point(point1.X - point3.X, point1.Y - point3.Y);
    Point point5 = new Point(point2.X - point3.X, point2.Y - point3.Y);
    double rawThickness = (double) this.rawThickness;
    double zoom = (double) this.ParentCanvas.PdfViewer.Zoom;
    this.ContentLine.X1 = point4.X;
    this.ContentLine.Y1 = point4.Y;
    this.ContentLine.X2 = point5.X;
    this.ContentLine.Y2 = point5.Y;
    this.DraggerLine.X1 = point4.X;
    this.DraggerLine.Y1 = point4.Y;
    this.DraggerLine.X2 = point5.X;
    this.DraggerLine.Y2 = point5.Y;
    this.ContentLine.StrokeThickness = 1.0;
    Canvas.SetLeft((UIElement) this.Point1Rect, point4.X - 4.0);
    Canvas.SetTop((UIElement) this.Point1Rect, point4.Y - 4.0);
    Canvas.SetLeft((UIElement) this.Point2Rect, point5.X - 4.0);
    Canvas.SetTop((UIElement) this.Point2Rect, point5.Y - 4.0);
    this.LayoutRoot.Width = Math.Abs(point4.X - point5.X);
    this.LayoutRoot.Height = Math.Abs(point4.Y - point5.Y);
    Canvas.SetLeft((UIElement) this, point3.X);
    Canvas.SetTop((UIElement) this, point3.Y);
  }

  private void Point1Rect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    if (!this.Point1Rect.CaptureMouse())
      return;
    this.isPressed = true;
    Window.GetWindow((DependencyObject) this).MouseMove += new MouseEventHandler(this.Point1Rect_MouseMove);
    Window.GetWindow((DependencyObject) this).MouseLeftButtonUp += new MouseButtonEventHandler(this.Point1Rect_MouseLeftButtonUp);
    Point? page = this.DeviceToPage(e.GetPosition((IInputElement) this.ParentCanvas));
    if (!page.HasValue)
      return;
    Point point = page.Value;
    double x = point.X;
    point = page.Value;
    double y = point.Y;
    this.point1 = new FS_POINTF(x, y);
    this.OnPageClientBoundsChanged();
  }

  private void Point1Rect_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    if (!(this.DataContext is MainViewModel dataContext))
      return;
    Window.GetWindow((DependencyObject) this).MouseMove -= new MouseEventHandler(this.Point1Rect_MouseMove);
    Window.GetWindow((DependencyObject) this).MouseLeftButtonUp -= new MouseButtonEventHandler(this.Point1Rect_MouseLeftButtonUp);
    this.Point1Rect.ReleaseMouseCapture();
    this.isPressed = false;
    this.ContentLine.Opacity = 0.0;
    Point? page1 = this.DeviceToPage(e.GetPosition((IInputElement) this.ParentCanvas));
    if (!page1.HasValue)
      return;
    this.point1 = new FS_POINTF(page1.Value.X, page1.Value.Y);
    this.OnPageClientBoundsChanged();
    using (dataContext.OperationManager.TraceAnnotationChange(this.Annotation.Page))
      this.Annotation.Line[0] = this.point1;
    this.Annotation.TryRedrawAnnotation();
    PdfPage page2 = this.Annotation.Page;
    if (this.Annotation.GetRECT().IntersectsWith(new FS_RECTF(0.0f, page2.Height, page2.Width, 0.0f)))
      return;
    this.Holder.Cancel();
  }

  private void Point1Rect_MouseMove(object sender, MouseEventArgs e)
  {
    if (!this.isPressed)
      return;
    this.ContentLine.Opacity = 1.0;
    Point? page = this.DeviceToPage(e.GetPosition((IInputElement) this.ParentCanvas));
    if (!page.HasValue)
      return;
    this.point1 = new FS_POINTF(page.Value.X, page.Value.Y);
    this.OnPageClientBoundsChanged();
  }

  private void Point2Rect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    if (!this.Point2Rect.CaptureMouse())
      return;
    this.isPressed = true;
    Window.GetWindow((DependencyObject) this).MouseMove += new MouseEventHandler(this.Point2Rect_MouseMove);
    Window.GetWindow((DependencyObject) this).MouseLeftButtonUp += new MouseButtonEventHandler(this.Point2Rect_MouseLeftButtonUp);
    Point? page = this.DeviceToPage(e.GetPosition((IInputElement) this.ParentCanvas));
    if (!page.HasValue)
      return;
    Point point = page.Value;
    double x = point.X;
    point = page.Value;
    double y = point.Y;
    this.point2 = new FS_POINTF(x, y);
    this.OnPageClientBoundsChanged();
  }

  private void Point2Rect_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    if (!(this.DataContext is MainViewModel dataContext))
      return;
    Window.GetWindow((DependencyObject) this).MouseMove -= new MouseEventHandler(this.Point2Rect_MouseMove);
    Window.GetWindow((DependencyObject) this).MouseLeftButtonUp -= new MouseButtonEventHandler(this.Point2Rect_MouseLeftButtonUp);
    this.Point2Rect.ReleaseMouseCapture();
    this.isPressed = false;
    this.ContentLine.Opacity = 0.0;
    Point? page1 = this.DeviceToPage(e.GetPosition((IInputElement) this.ParentCanvas));
    if (!page1.HasValue)
      return;
    this.point2 = new FS_POINTF(page1.Value.X, page1.Value.Y);
    this.OnPageClientBoundsChanged();
    using (dataContext.OperationManager.TraceAnnotationChange(this.Annotation.Page))
      this.Annotation.Line[1] = this.point2;
    this.Annotation.TryRedrawAnnotation();
    PdfPage page2 = this.Annotation.Page;
    if (this.Annotation.GetRECT().IntersectsWith(new FS_RECTF(0.0f, page2.Height, page2.Width, 0.0f)))
      return;
    this.Holder.Cancel();
  }

  private void Point2Rect_MouseMove(object sender, MouseEventArgs e)
  {
    if (!this.isPressed)
      return;
    this.ContentLine.Opacity = 1.0;
    Point? page = this.DeviceToPage(e.GetPosition((IInputElement) this.ParentCanvas));
    if (!page.HasValue)
      return;
    this.point2 = new FS_POINTF(page.Value.X, page.Value.Y);
    this.OnPageClientBoundsChanged();
  }

  private void DraggerLine_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    if (!this.DraggerLine.CaptureMouse())
      return;
    this.isPressed = true;
    Window.GetWindow((DependencyObject) this).MouseMove += new MouseEventHandler(this.DraggerLine_MouseMove);
    Window.GetWindow((DependencyObject) this).MouseLeftButtonUp += new MouseButtonEventHandler(this.DraggerLine_MouseLeftButtonUp);
    Point? page = this.DeviceToPage(e.GetPosition((IInputElement) this.ParentCanvas));
    if (!page.HasValue)
      return;
    Point point = page.Value;
    double x = point.X;
    point = page.Value;
    double y = point.Y;
    this.startPoint = new FS_POINTF(x, y);
  }

  private void DraggerLine_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    if (!(this.DataContext is MainViewModel dataContext))
      return;
    Window.GetWindow((DependencyObject) this).MouseMove -= new MouseEventHandler(this.DraggerLine_MouseMove);
    Window.GetWindow((DependencyObject) this).MouseLeftButtonUp -= new MouseButtonEventHandler(this.DraggerLine_MouseLeftButtonUp);
    this.DraggerLine.ReleaseMouseCapture();
    this.isPressed = false;
    this.ContentLine.Opacity = 0.0;
    Point? page1 = this.DeviceToPage(e.GetPosition((IInputElement) this.ParentCanvas));
    if (!page1.HasValue)
      return;
    FS_POINTF fsPointf = new FS_POINTF(page1.Value.X, page1.Value.Y);
    float num1 = fsPointf.X - this.startPoint.X;
    float num2 = fsPointf.Y - this.startPoint.Y;
    this.startPoint = new FS_POINTF();
    this.point1.X += num1;
    this.point1.Y += num2;
    this.point2.X += num1;
    this.point2.Y += num2;
    this.OnPageClientBoundsChanged();
    using (dataContext.OperationManager.TraceAnnotationChange(this.Annotation.Page))
    {
      this.Annotation.Line[0] = this.point1;
      this.Annotation.Line[1] = this.point2;
    }
    this.Annotation.TryRedrawAnnotation();
    PdfPage page2 = this.Annotation.Page;
    if (this.Annotation.GetRECT().IntersectsWith(new FS_RECTF(0.0f, page2.Height, page2.Width, 0.0f)))
      return;
    this.Holder.Cancel();
  }

  private void DraggerLine_MouseMove(object sender, MouseEventArgs e)
  {
    if (!this.isPressed)
      return;
    this.ContentLine.Opacity = 1.0;
    Point? page = this.DeviceToPage(e.GetPosition((IInputElement) this.ParentCanvas));
    if (!page.HasValue)
      return;
    FS_POINTF fsPointf = new FS_POINTF(page.Value.X, page.Value.Y);
    float num1 = fsPointf.X - this.startPoint.X;
    float num2 = fsPointf.Y - this.startPoint.Y;
    this.startPoint = fsPointf;
    this.point1.X += num1;
    this.point1.Y += num2;
    this.point2.X += num1;
    this.point2.Y += num2;
    this.OnPageClientBoundsChanged();
  }

  public bool OnPropertyChanged(string propertyName)
  {
    if (!(this.DataContext is MainViewModel dataContext) || !(propertyName == "LineStroke") && !(propertyName == "LineWidth"))
      return false;
    using (dataContext.OperationManager.TraceAnnotationChange(this.Annotation.Page))
    {
      if (propertyName == "LineWidth")
      {
        if ((PdfWrapper) this.Annotation.LineStyle == (PdfWrapper) null)
          this.Annotation.LineStyle = new PdfBorderStyle();
        this.Annotation.LineStyle.Width = dataContext.AnnotationToolbar.AnnotationMenuPropertyAccessor.LineWidth;
      }
      if (propertyName == "LineStroke")
        this.Annotation.Color = ((Color) ColorConverter.ConvertFromString(dataContext.AnnotationToolbar.AnnotationMenuPropertyAccessor.LineStroke)).ToPdfColor();
      this.Annotation.RegenerateAppearances();
    }
    this.Annotation.TryRedrawAnnotation();
    return true;
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/annotations/annotationlinecontrol.xaml", UriKind.Relative));
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
        break;
      case 2:
        this.ContentLine = (Line) target;
        break;
      case 3:
        this.DraggerLine = (Line) target;
        this.DraggerLine.MouseLeftButtonDown += new MouseButtonEventHandler(this.DraggerLine_MouseLeftButtonDown);
        break;
      case 4:
        this.Point1Rect = (Ellipse) target;
        this.Point1Rect.MouseLeftButtonDown += new MouseButtonEventHandler(this.Point1Rect_MouseLeftButtonDown);
        break;
      case 5:
        this.Point2Rect = (Ellipse) target;
        this.Point2Rect.MouseLeftButtonDown += new MouseButtonEventHandler(this.Point2Rect_MouseLeftButtonDown);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
