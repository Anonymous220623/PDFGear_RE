// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Annotations.AnnotationDragControl
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
using PDFKit;
using PDFKit.Utils;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
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

public partial class AnnotationDragControl : 
  UserControl,
  IAnnotationControl<PdfInkAnnotation>,
  IAnnotationControl,
  IComponentConnector
{
  private FS_POINTF startPoint;
  private List<Point> pc;
  private List<List<Point>> array;
  private List<PointCollection> offsetPoints;
  internal Canvas LayoutRoot;
  internal Rectangle AnnotationDrag;
  internal ResizeView DragResizeView;
  internal Canvas AnotationDataCanvas;
  private bool _contentLoaded;

  public AnnotationDragControl(PdfInkAnnotation annot, IAnnotationHolder holder)
  {
    this.InitializeComponent();
    this.Annotation = annot;
    this.Holder = holder;
    this.pc = new List<Point>();
    this.array = new List<List<Point>>();
    this.offsetPoints = new List<PointCollection>();
  }

  public PdfInkAnnotation Annotation { get; }

  public IAnnotationHolder Holder { get; }

  public AnnotationCanvas ParentCanvas => (AnnotationCanvas) this.Parent;

  PdfAnnotation IAnnotationControl.Annotation => (PdfAnnotation) this.Annotation;

  public void OnPageClientBoundsChanged()
  {
    if (!((PdfWrapper) this.Annotation != (PdfWrapper) null) || this.Annotation.InkList == null || this.Annotation.InkList.Count <= 0)
      return;
    double num1 = double.MaxValue;
    double num2 = double.MinValue;
    double num3 = double.MinValue;
    double num4 = double.MaxValue;
    this.pc.Clear();
    this.offsetPoints.Clear();
    this.array.Clear();
    this.Annotation.Opacity = 1f;
    PdfInkPointCollection inkList = this.Annotation.InkList;
    if (inkList != null && inkList.Count > 0)
    {
      for (int index = 0; index < inkList.Count; ++index)
      {
        List<Point> pointList = new List<Point>();
        foreach (FS_POINTF fsPointf in inkList[index])
        {
          num1 = Math.Min((double) fsPointf.X, num1);
          num2 = Math.Max((double) fsPointf.X, num2);
          num3 = Math.Max((double) fsPointf.Y, num3);
          num4 = Math.Min((double) fsPointf.Y, num4);
          Point clientPoint;
          if (this.ParentCanvas.PdfViewer.TryGetClientPoint(this.Annotation.Page.PageIndex, new Point((double) fsPointf.X, (double) fsPointf.Y), out clientPoint))
            pointList.Add(clientPoint);
        }
        this.array.Add(pointList);
      }
    }
    Rect clientRect;
    if (num2 < num1 || num3 < num4 || !this.ParentCanvas.PdfViewer.TryGetClientRect(this.Annotation.Page.PageIndex, new FS_RECTF(num1, num3, num2, num4), out clientRect))
      return;
    this.AnnotationDrag.Width = clientRect.Width + 6.0;
    this.AnnotationDrag.Height = clientRect.Height + 12.0;
    this.LayoutRoot.Width = this.AnnotationDrag.Width;
    this.LayoutRoot.Height = this.AnnotationDrag.Height;
    Canvas.SetLeft((UIElement) this, clientRect.Left - 3.0);
    Canvas.SetTop((UIElement) this, clientRect.Top - 6.0);
    this.DragResizeView.Width = clientRect.Width + 6.0;
    this.DragResizeView.Height = clientRect.Height + 6.0;
    double left = Canvas.GetLeft((UIElement) this);
    double top = Canvas.GetTop((UIElement) this);
    for (int index = 0; index < this.array.Count; ++index)
    {
      this.offsetPoints.Add(new PointCollection());
      foreach (Point point1 in this.array[index])
      {
        Point point2 = new Point(point1.X - left, Math.Abs(point1.Y - top));
        this.offsetPoints[index].Add(point2);
      }
      Polyline polyline = new Polyline();
      polyline.Stroke = (Brush) new SolidColorBrush((Color) ColorConverter.ConvertFromString("#1892ff"));
      polyline.StrokeThickness = 1.0;
      polyline.Cursor = Cursors.SizeAll;
      polyline.SnapsToDevicePixels = false;
      Polyline element = polyline;
      if (this.offsetPoints[index].Count > 0)
        element.Points = this.offsetPoints[index];
      this.AnotationDataCanvas.Children.Add((UIElement) element);
    }
  }

  private void ResizeView_ResizeDragStarted(object sender, ResizeViewResizeDragStartedEventArgs e)
  {
    this.startPoint = new FS_POINTF(this.Annotation.GetRECT().left, this.Annotation.GetRECT().top);
  }

  private void ResizeView_ResizeDragCompleted(object sender, ResizeViewResizeDragEventArgs e)
  {
    if (!(this.DataContext is MainViewModel dataContext))
      return;
    double left = Canvas.GetLeft((UIElement) this);
    double top = Canvas.GetTop((UIElement) this);
    double length1 = left + e.OffsetX;
    double length2 = top + e.OffsetY;
    Canvas.SetLeft((UIElement) this, length1);
    Canvas.SetTop((UIElement) this, length2);
    PdfViewer pdfViewer = this.ParentCanvas.PdfViewer;
    PdfPage page = this.Annotation?.Page;
    if (pdfViewer == null || page == null || this.Annotation.InkList == null || this.Annotation.InkList.Count == 0)
      return;
    using (dataContext.OperationManager.TraceAnnotationChange(this.Annotation.Page))
    {
      PdfInkPointCollection inkPointCollection = new PdfInkPointCollection();
      foreach (PdfLinePointCollection<PdfInkAnnotation> ink in this.Annotation.InkList)
      {
        if (ink != null)
        {
          this.startPoint = new FS_POINTF();
          PdfLinePointCollection<PdfInkAnnotation> linePointCollection = new PdfLinePointCollection<PdfInkAnnotation>();
          for (int index = 0; index < ink.Count; ++index)
          {
            Point clientPoint;
            if (pdfViewer.TryGetClientPoint(page.PageIndex, ink[index].ToPoint(), out clientPoint))
            {
              clientPoint.X += e.OffsetX;
              clientPoint.Y += e.OffsetY;
              Point pagePoint;
              if (pdfViewer.TryGetPagePoint(page.PageIndex, clientPoint, out pagePoint))
                linePointCollection.Add(pagePoint.ToPdfPoint());
            }
          }
          inkPointCollection.Add(linePointCollection);
        }
      }
      this.Annotation.InkList.Clear();
      this.Annotation.InkList = inkPointCollection;
    }
    if ((double) this.Annotation.Opacity == 0.0)
      this.Annotation.Opacity = 0.01f;
    this.Annotation.TryRedrawAnnotation();
    this.OnPageClientBoundsChanged();
    if (this.Annotation.GetRECT().IntersectsWith(new FS_RECTF(0.0f, page.Height, page.Width, 0.0f)))
      return;
    this.Holder.Cancel();
  }

  public bool OnPropertyChanged(string propertyName)
  {
    if (!(this.DataContext is MainViewModel dataContext) || !(propertyName == "InkStroke") && !(propertyName == "InkWidth"))
      return false;
    using (dataContext.OperationManager.TraceAnnotationChange(this.Annotation.Page))
    {
      if (propertyName == "InkWidth")
      {
        if ((PdfWrapper) this.Annotation.LineStyle == (PdfWrapper) null)
          this.Annotation.LineStyle = new PdfBorderStyle();
        this.Annotation.LineStyle.Width = dataContext.AnnotationToolbar.AnnotationMenuPropertyAccessor.InkWidth;
      }
      if (propertyName == "InkStroke")
        this.Annotation.Color = ((Color) ColorConverter.ConvertFromString(dataContext.AnnotationToolbar.AnnotationMenuPropertyAccessor.InkStroke)).ToPdfColor();
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
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/annotations/annotationdragcontrol.xaml", UriKind.Relative));
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
        break;
      case 2:
        this.AnnotationDrag = (Rectangle) target;
        break;
      case 3:
        this.DragResizeView = (ResizeView) target;
        break;
      case 4:
        this.AnotationDataCanvas = (Canvas) target;
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
