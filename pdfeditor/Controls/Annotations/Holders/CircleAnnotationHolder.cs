// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Annotations.Holders.CircleAnnotationHolder
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
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace pdfeditor.Controls.Annotations.Holders;

public class CircleAnnotationHolder : BaseAnnotationHolder<PdfFigureAnnotation>
{
  private Ellipse newCircleControl;
  private FS_POINTF createStartPoint;
  private FS_POINTF createEndPoint;
  public Point startPoint;
  private List<FS_POINTF> circleFsPoints;
  private AnnotationDragEditControl editControl;

  public CircleAnnotationHolder(AnnotationCanvas annotationCanvas)
    : base(annotationCanvas)
  {
    this.circleFsPoints = new List<FS_POINTF>();
  }

  public override bool IsTextMarkupAnnotation => false;

  public override void OnPageClientBoundsChanged()
  {
    if (this.State == AnnotationHolderState.CreatingNew)
    {
      if (this.newCircleControl == null)
        throw new ArgumentException("newCircleControl");
      if (this.CurrentPage == null)
        throw new ArgumentException("CurrentPage");
    }
    else
    {
      if (this.State != AnnotationHolderState.Selected)
        return;
      this.editControl?.OnPageClientBoundsChanged();
    }
  }

  protected override void OnCancel()
  {
    if (this.editControl != null)
    {
      this.AnnotationCanvas.Children.Remove((UIElement) this.editControl);
      this.editControl = (AnnotationDragEditControl) null;
    }
    if (this.newCircleControl != null)
    {
      this.AnnotationCanvas.Children.Remove((UIElement) this.newCircleControl);
      this.newCircleControl = (Ellipse) null;
    }
    this.createStartPoint = new FS_POINTF();
    this.createEndPoint = new FS_POINTF();
  }

  protected override async Task<System.Collections.Generic.IReadOnlyList<PdfFigureAnnotation>> OnCompleteCreateNewAsync()
  {
    CircleAnnotationHolder annotationHolder = this;
    // ISSUE: explicit non-virtual call
    __nonvirtual (annotationHolder.AnnotationCanvas).Children.Remove((UIElement) annotationHolder.newCircleControl);
    MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
    // ISSUE: explicit non-virtual call
    PdfPage page = __nonvirtual (annotationHolder.CurrentPage);
    if (page.Annots == null)
      page.CreateAnnotations();
    if ((double) Math.Abs(annotationHolder.createStartPoint.X - annotationHolder.createEndPoint.X) <= 10.0 && (double) Math.Abs(annotationHolder.createStartPoint.Y - annotationHolder.createEndPoint.Y) <= 10.0)
      return (System.Collections.Generic.IReadOnlyList<PdfFigureAnnotation>) null;
    PdfFigureAnnotation circleAnnot = (PdfFigureAnnotation) new PdfCircleAnnotation(page);
    circleAnnot.Subject = "Ellipse";
    List<FS_POINTF> list = annotationHolder.circleFsPoints.Distinct<FS_POINTF>().ToList<FS_POINTF>();
    int num = list.Count<FS_POINTF>();
    if (list.Count > 0)
    {
      FS_POINTF fsPointf1;
      ref FS_POINTF local1 = ref fsPointf1;
      FS_POINTF fsPointf2 = list[0];
      double x1 = (double) fsPointf2.X;
      fsPointf2 = list[0];
      double y1 = (double) fsPointf2.Y;
      local1 = new FS_POINTF((float) x1, (float) y1);
      FS_POINTF fsPointf3;
      ref FS_POINTF local2 = ref fsPointf3;
      fsPointf2 = list[num - 1];
      double x2 = (double) fsPointf2.X;
      fsPointf2 = list[num - 1];
      double y2 = (double) fsPointf2.Y;
      local2 = new FS_POINTF((float) x2, (float) y2);
      fsPointf2 = list[0];
      double x3 = (double) fsPointf2.X;
      fsPointf2 = list[num - 1];
      double y3 = (double) fsPointf2.Y;
      FS_POINTF fsPointf4 = new FS_POINTF((float) x3, (float) y3);
      fsPointf2 = list[num - 1];
      double x4 = (double) fsPointf2.X;
      fsPointf2 = list[0];
      double y4 = (double) fsPointf2.Y;
      FS_POINTF fsPointf5 = new FS_POINTF((float) x4, (float) y4);
      circleAnnot.Text = AnnotationAuthorUtil.GetAuthorName();
      Color color1 = (Color) ColorConverter.ConvertFromString(requiredService.AnnotationToolbar.AnnotationMenuPropertyAccessor.EllipseStroke);
      FS_COLOR fsColor = new FS_COLOR((int) color1.A, (int) color1.R, (int) color1.G, (int) color1.B);
      Color color2 = (Color) ColorConverter.ConvertFromString(requiredService.AnnotationToolbar.AnnotationMenuPropertyAccessor.EllipseFill);
      circleAnnot.InteriorColor = new FS_COLOR((int) color2.A, (int) color2.R, (int) color2.G, (int) color2.B);
      circleAnnot.Color = fsColor;
      circleAnnot.Opacity = 1f;
      if ((double) fsPointf1.X < (double) fsPointf3.X)
      {
        if ((double) fsPointf1.Y > (double) fsPointf3.Y)
          circleAnnot.Rectangle = new FS_RECTF(fsPointf1.X, fsPointf1.Y, fsPointf3.X, fsPointf3.Y);
        else
          circleAnnot.Rectangle = new FS_RECTF(fsPointf1.X, fsPointf3.Y, fsPointf3.X, fsPointf1.Y);
      }
      else if ((double) fsPointf1.Y < (double) fsPointf3.Y)
        circleAnnot.Rectangle = new FS_RECTF(fsPointf3.X, fsPointf3.Y, fsPointf1.X, fsPointf1.Y);
      else
        circleAnnot.Rectangle = new FS_RECTF(fsPointf3.X, fsPointf1.Y, fsPointf1.X, fsPointf3.Y);
      circleAnnot.BorderStyle = new PdfBorderStyle();
      circleAnnot.BorderStyle.Style = BorderStyles.Solid;
      circleAnnot.BorderStyle.Width = requiredService.AnnotationToolbar.AnnotationMenuPropertyAccessor.EllipseThickness;
      circleAnnot.ModificationDate = DateTimeOffset.Now.ToModificationDateString();
      circleAnnot.CreationDate = circleAnnot.ModificationDate;
      circleAnnot.Flags |= AnnotationFlags.Print;
      page.Annots.Add((PdfAnnotation) circleAnnot);
      await requiredService.OperationManager.TraceAnnotationInsertAsync((PdfAnnotation) circleAnnot);
      await page.TryRedrawPageAsync();
    }
    annotationHolder.createStartPoint = new FS_POINTF();
    annotationHolder.createEndPoint = new FS_POINTF();
    annotationHolder.circleFsPoints.Clear();
    if (!((PdfWrapper) circleAnnot != (PdfWrapper) null))
      return (System.Collections.Generic.IReadOnlyList<PdfFigureAnnotation>) null;
    return (System.Collections.Generic.IReadOnlyList<PdfFigureAnnotation>) new PdfFigureAnnotation[1]
    {
      circleAnnot
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
    this.circleFsPoints.Add(pagePoint);
    double length1 = Math.Min(clientPoint.X, this.startPoint.X);
    double length2 = Math.Min(clientPoint.Y, this.startPoint.Y);
    double num1 = Math.Max(clientPoint.X, this.startPoint.X) - length1;
    double num2 = Math.Max(clientPoint.Y, this.startPoint.Y) - length2;
    if (this.newCircleControl == null)
      return;
    this.newCircleControl.Width = num1;
    this.newCircleControl.Height = num2;
    Canvas.SetLeft((UIElement) this.newCircleControl, length1);
    Canvas.SetTop((UIElement) this.newCircleControl, length2);
  }

  protected override bool OnSelecting(PdfFigureAnnotation annotation, bool afterCreate)
  {
    this.editControl = new AnnotationDragEditControl(annotation, (IAnnotationHolder) this);
    this.AnnotationCanvas.Children.Add((UIElement) this.editControl);
    return true;
  }

  protected override bool OnStartCreateNew(PdfPage page, FS_POINTF pagePoint)
  {
    this.createStartPoint = pagePoint;
    this.createEndPoint = pagePoint;
    Point clientPoint;
    if (!this.AnnotationCanvas.PdfViewer.TryGetClientPoint(page.PageIndex, pagePoint.ToPoint(), out clientPoint))
      return false;
    this.startPoint = clientPoint;
    this.circleFsPoints.Clear();
    MainViewModel dataContext = this.AnnotationCanvas.DataContext as MainViewModel;
    Color color = (Color) ColorConverter.ConvertFromString(dataContext.AnnotationToolbar.AnnotationMenuPropertyAccessor.EllipseStroke);
    Ellipse ellipse = new Ellipse();
    ellipse.Stroke = (Brush) new SolidColorBrush(color);
    ellipse.StrokeThickness = (double) dataContext.AnnotationToolbar.AnnotationMenuPropertyAccessor.EllipseThickness;
    this.newCircleControl = ellipse;
    Canvas.SetLeft((UIElement) this.newCircleControl, this.startPoint.X);
    Canvas.SetTop((UIElement) this.newCircleControl, this.startPoint.Y);
    this.AnnotationCanvas.Children.Add((UIElement) this.newCircleControl);
    return true;
  }

  public override bool OnPropertyChanged(string propertyName)
  {
    if (!(propertyName == "EllipseFill") && !(propertyName == "EllipseStroke") && !(propertyName == "EllipseThickness"))
      return false;
    AnnotationDragEditControl editControl = this.editControl;
    // ISSUE: explicit non-virtual call
    return editControl != null && __nonvirtual (editControl.OnPropertyChanged(propertyName));
  }
}
