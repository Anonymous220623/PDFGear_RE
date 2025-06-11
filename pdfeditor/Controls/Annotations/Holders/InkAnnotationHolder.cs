// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Annotations.Holders.InkAnnotationHolder
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.Wrappers;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using PDFKit.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace pdfeditor.Controls.Annotations.Holders;

public class InkAnnotationHolder : BaseAnnotationHolder<PdfInkAnnotation>
{
  private FS_POINTF createStartPoint;
  private FS_POINTF createEndPoint;
  private AnnotationFocusControl selectControl;
  private List<FS_POINTF> lineFsPoints;
  private List<Line> newlines;
  private List<Point> linePoints;
  private AnnotationDragControl dragControl;

  public InkAnnotationHolder(AnnotationCanvas annotationCanvas)
    : base(annotationCanvas)
  {
    this.lineFsPoints = new List<FS_POINTF>();
    this.newlines = new List<Line>();
    this.linePoints = new List<Point>();
  }

  public override bool IsTextMarkupAnnotation => false;

  public override void OnPageClientBoundsChanged()
  {
    this.selectControl?.InvalidateVisual();
    if (this.State == AnnotationHolderState.CreatingNew || this.State != AnnotationHolderState.Selected)
      return;
    this.dragControl?.OnPageClientBoundsChanged();
  }

  protected override void OnCancel()
  {
    if (this.dragControl != null)
    {
      this.AnnotationCanvas.Children.Remove((UIElement) this.dragControl);
      this.dragControl = (AnnotationDragControl) null;
    }
    foreach (UIElement newline in this.newlines)
      this.AnnotationCanvas.Children.Remove(newline);
    this.lineFsPoints.Clear();
    this.linePoints.Clear();
    this.createStartPoint = new FS_POINTF();
    this.createEndPoint = new FS_POINTF();
  }

  protected override async Task<System.Collections.Generic.IReadOnlyList<PdfInkAnnotation>> OnCompleteCreateNewAsync()
  {
    InkAnnotationHolder annotationHolder = this;
    if (annotationHolder.newlines != null && annotationHolder.newlines.Count > 0)
    {
      // ISSUE: reference to a compiler-generated method
      annotationHolder.newlines.ForEach(new Action<Line>(annotationHolder.\u003COnCompleteCreateNewAsync\u003Eb__12_0));
    }
    List<FS_POINTF> list = annotationHolder.lineFsPoints.Distinct<FS_POINTF>().ToList<FS_POINTF>();
    PdfInkAnnotation inkAnnot = (PdfInkAnnotation) null;
    if (list.Count > 1)
    {
      float val2_1 = float.MaxValue;
      float val2_2 = float.MaxValue;
      float val2_3 = float.MinValue;
      float val2_4 = float.MinValue;
      for (int index = 0; index < list.Count; ++index)
      {
        FS_POINTF fsPointf = list[index];
        val2_1 = Math.Min(fsPointf.X, val2_1);
        fsPointf = list[index];
        val2_2 = Math.Min(fsPointf.Y, val2_2);
        fsPointf = list[index];
        val2_3 = Math.Max(fsPointf.X, val2_3);
        fsPointf = list[index];
        val2_4 = Math.Max(fsPointf.Y, val2_4);
      }
      if (Math.Sqrt(((double) val2_3 - (double) val2_1) * ((double) val2_3 - (double) val2_1) + ((double) val2_4 - (double) val2_2) * ((double) val2_4 - (double) val2_2)) > 3.0)
      {
        annotationHolder.newlines.Clear();
        MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
        // ISSUE: explicit non-virtual call
        PdfPage page = __nonvirtual (annotationHolder.CurrentPage);
        if (page.Document == null)
          throw new ArgumentException("Document");
        if (page.Annots == null)
          page.CreateAnnotations();
        inkAnnot = new PdfInkAnnotation(page);
        Color color = (Color) ColorConverter.ConvertFromString(requiredService.AnnotationToolbar.AnnotationMenuPropertyAccessor.InkStroke);
        inkAnnot.Color = new FS_COLOR((int) color.A, (int) color.R, (int) color.G, (int) color.B);
        inkAnnot.Opacity = 1f;
        inkAnnot.LineStyle = new PdfBorderStyle();
        inkAnnot.LineStyle.Style = BorderStyles.Solid;
        inkAnnot.LineStyle.Width = requiredService.AnnotationToolbar.AnnotationMenuPropertyAccessor.InkWidth;
        inkAnnot.Text = AnnotationAuthorUtil.GetAuthorName();
        PdfLinePointCollection<PdfInkAnnotation> linePointCollection = new PdfLinePointCollection<PdfInkAnnotation>();
        for (int index = 0; index < list.Count; ++index)
          linePointCollection.Add(list[index]);
        inkAnnot.InkList = new PdfInkPointCollection();
        inkAnnot.InkList.Add(linePointCollection);
        inkAnnot.ModificationDate = DateTimeOffset.Now.ToModificationDateString();
        inkAnnot.CreationDate = inkAnnot.ModificationDate;
        inkAnnot.Flags |= AnnotationFlags.Print;
        page.Annots.Add((PdfAnnotation) inkAnnot);
        await requiredService.OperationManager.TraceAnnotationInsertAsync((PdfAnnotation) inkAnnot);
        await page.TryRedrawPageAsync();
        page = (PdfPage) null;
      }
    }
    annotationHolder.createStartPoint = new FS_POINTF();
    annotationHolder.createEndPoint = new FS_POINTF();
    annotationHolder.lineFsPoints.Clear();
    if (!((PdfWrapper) inkAnnot != (PdfWrapper) null))
      return (System.Collections.Generic.IReadOnlyList<PdfInkAnnotation>) null;
    return (System.Collections.Generic.IReadOnlyList<PdfInkAnnotation>) new PdfInkAnnotation[1]
    {
      inkAnnot
    };
  }

  protected override void OnProcessCreateNew(PdfPage page, FS_POINTF pagePoint)
  {
    if (page != this.CurrentPage)
      return;
    this.createEndPoint = pagePoint;
    Point clientPoint;
    if (!this.AnnotationCanvas.PdfViewer.TryGetClientPoint(page.PageIndex, pagePoint.ToPoint(), out clientPoint))
      return;
    this.lineFsPoints.Add(pagePoint);
    this.linePoints.Add(clientPoint);
    List<Point> list = this.linePoints.Distinct<Point>().ToList<Point>();
    int num = list.Count<Point>();
    if (!(clientPoint != list.ElementAt<Point>(0)) || list.Count < 2)
      return;
    Point point = list[num - 2];
    double x = point.X;
    point = list[num - 2];
    double y = point.Y;
    Line line = this.CreateLine(new Point(x, y), clientPoint);
    this.newlines.Add(line);
    this.AnnotationCanvas.Children.Add((UIElement) line);
  }

  private Line CreateLine(Point startPoint, Point endPoint)
  {
    MainViewModel dataContext = this.AnnotationCanvas.DataContext as MainViewModel;
    SolidColorBrush solidColorBrush = Brushes.Transparent;
    if (!string.IsNullOrEmpty(dataContext.AnnotationToolbar.AnnotationMenuPropertyAccessor.InkStroke))
    {
      try
      {
        solidColorBrush = new SolidColorBrush((Color) ColorConverter.ConvertFromString(dataContext.AnnotationToolbar.AnnotationMenuPropertyAccessor.InkStroke));
      }
      catch
      {
      }
    }
    Line line = new Line();
    line.Stroke = (Brush) solidColorBrush;
    line.StrokeThickness = (double) dataContext.AnnotationToolbar.AnnotationMenuPropertyAccessor.InkWidth;
    line.X1 = startPoint.X;
    line.Y1 = startPoint.Y;
    line.X2 = endPoint.X;
    line.Y2 = endPoint.Y;
    line.IsHitTestVisible = false;
    return line;
  }

  protected override bool OnStartCreateNew(PdfPage page, FS_POINTF pagePoint)
  {
    this.createStartPoint = pagePoint;
    this.createEndPoint = pagePoint;
    Point clientPoint;
    if (!this.AnnotationCanvas.PdfViewer.TryGetClientPoint(page.PageIndex, pagePoint.ToPoint(), out clientPoint))
      return false;
    this.lineFsPoints.Clear();
    this.linePoints.Clear();
    this.linePoints.Add(clientPoint);
    this.lineFsPoints.Add(pagePoint);
    return true;
  }

  protected override bool OnSelecting(PdfInkAnnotation annotation, bool afterCreate)
  {
    this.dragControl = new AnnotationDragControl(annotation, (IAnnotationHolder) this);
    this.AnnotationCanvas.Children.Add((UIElement) this.dragControl);
    return true;
  }

  public override bool OnPropertyChanged(string propertyName)
  {
    if (!(propertyName == "InkStroke") && !(propertyName == "InkWidth"))
      return false;
    AnnotationDragControl dragControl = this.dragControl;
    // ISSUE: explicit non-virtual call
    return dragControl != null && __nonvirtual (dragControl.OnPropertyChanged(propertyName));
  }
}
