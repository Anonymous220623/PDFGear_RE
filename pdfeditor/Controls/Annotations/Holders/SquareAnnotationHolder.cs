// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Annotations.Holders.SquareAnnotationHolder
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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace pdfeditor.Controls.Annotations.Holders;

public class SquareAnnotationHolder(AnnotationCanvas annotationCanvas) : 
  BaseAnnotationHolder<PdfFigureAnnotation>(annotationCanvas)
{
  private Rectangle newSquareControl;
  private FS_POINTF createStartPoint;
  private FS_POINTF createEndPoint;
  public Point startPoint;
  private AnnotationDragEditControl editControl;

  public override bool IsTextMarkupAnnotation => false;

  public override void OnPageClientBoundsChanged()
  {
    if (this.State == AnnotationHolderState.CreatingNew)
    {
      if (this.newSquareControl == null)
        throw new ArgumentException("newSquareControl");
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
    if (this.newSquareControl != null)
    {
      this.AnnotationCanvas.Children.Remove((UIElement) this.newSquareControl);
      this.newSquareControl = (Rectangle) null;
    }
    this.createStartPoint = new FS_POINTF();
    this.createEndPoint = new FS_POINTF();
  }

  protected override async Task<System.Collections.Generic.IReadOnlyList<PdfFigureAnnotation>> OnCompleteCreateNewAsync()
  {
    SquareAnnotationHolder annotationHolder = this;
    // ISSUE: explicit non-virtual call
    __nonvirtual (annotationHolder.AnnotationCanvas).Children.Remove((UIElement) annotationHolder.newSquareControl);
    MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
    // ISSUE: explicit non-virtual call
    PdfPage page = __nonvirtual (annotationHolder.CurrentPage);
    if (page.Annots == null)
      page.CreateAnnotations();
    if ((double) Math.Abs(annotationHolder.createStartPoint.X - annotationHolder.createEndPoint.X) <= 10.0 && (double) Math.Abs(annotationHolder.createStartPoint.Y - annotationHolder.createEndPoint.Y) <= 10.0)
      return (System.Collections.Generic.IReadOnlyList<PdfFigureAnnotation>) null;
    PdfFigureAnnotation squareAnnot = (PdfFigureAnnotation) new PdfSquareAnnotation(page);
    squareAnnot.Subject = "Rectangle";
    Color color1 = (Color) ColorConverter.ConvertFromString(requiredService.AnnotationToolbar.AnnotationMenuPropertyAccessor.ShapeStroke);
    squareAnnot.Color = new FS_COLOR((int) color1.A, (int) color1.R, (int) color1.G, (int) color1.B);
    squareAnnot.Opacity = 1f;
    squareAnnot.Text = AnnotationAuthorUtil.GetAuthorName();
    Color color2 = (Color) ColorConverter.ConvertFromString(requiredService.AnnotationToolbar.AnnotationMenuPropertyAccessor.ShapeFill);
    squareAnnot.InteriorColor = new FS_COLOR((int) color2.A, (int) color2.R, (int) color2.G, (int) color2.B);
    squareAnnot.Rectangle = new FS_RECTF(Math.Min(annotationHolder.createStartPoint.X, annotationHolder.createEndPoint.X), Math.Max(annotationHolder.createStartPoint.Y, annotationHolder.createEndPoint.Y), Math.Max(annotationHolder.createStartPoint.X, annotationHolder.createEndPoint.X), Math.Min(annotationHolder.createStartPoint.Y, annotationHolder.createEndPoint.Y));
    squareAnnot.BorderStyle = new PdfBorderStyle();
    squareAnnot.BorderStyle.Style = BorderStyles.Solid;
    squareAnnot.BorderStyle.Width = requiredService.AnnotationToolbar.AnnotationMenuPropertyAccessor.ShapeThickness;
    squareAnnot.ModificationDate = DateTimeOffset.Now.ToModificationDateString();
    squareAnnot.CreationDate = squareAnnot.ModificationDate;
    squareAnnot.Flags |= AnnotationFlags.Print;
    page.Annots.Add((PdfAnnotation) squareAnnot);
    await requiredService.OperationManager.TraceAnnotationInsertAsync((PdfAnnotation) squareAnnot);
    await page.TryRedrawPageAsync();
    annotationHolder.createStartPoint = new FS_POINTF();
    annotationHolder.createEndPoint = new FS_POINTF();
    if (!((PdfWrapper) squareAnnot != (PdfWrapper) null))
      return (System.Collections.Generic.IReadOnlyList<PdfFigureAnnotation>) null;
    return (System.Collections.Generic.IReadOnlyList<PdfFigureAnnotation>) new PdfFigureAnnotation[1]
    {
      squareAnnot
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
    double length1 = Math.Min(clientPoint.X, this.startPoint.X);
    double length2 = Math.Min(clientPoint.Y, this.startPoint.Y);
    double num1 = Math.Max(clientPoint.X, this.startPoint.X) - length1;
    double num2 = Math.Max(clientPoint.Y, this.startPoint.Y) - length2;
    if (this.newSquareControl == null)
      return;
    this.newSquareControl.Width = num1;
    this.newSquareControl.Height = num2;
    Canvas.SetLeft((UIElement) this.newSquareControl, length1);
    Canvas.SetTop((UIElement) this.newSquareControl, length2);
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
    MainViewModel dataContext = this.AnnotationCanvas.DataContext as MainViewModel;
    Color color = (Color) ColorConverter.ConvertFromString(dataContext.AnnotationToolbar.AnnotationMenuPropertyAccessor.ShapeStroke);
    Rectangle rectangle = new Rectangle();
    rectangle.Stroke = (Brush) new SolidColorBrush(color);
    rectangle.StrokeThickness = (double) dataContext.AnnotationToolbar.AnnotationMenuPropertyAccessor.ShapeThickness;
    this.newSquareControl = rectangle;
    Canvas.SetLeft((UIElement) this.newSquareControl, this.startPoint.X);
    Canvas.SetTop((UIElement) this.newSquareControl, this.startPoint.Y);
    this.AnnotationCanvas.Children.Add((UIElement) this.newSquareControl);
    return true;
  }

  public override bool OnPropertyChanged(string propertyName)
  {
    if (!(propertyName == "ShapeFill") && !(propertyName == "ShapeStroke") && !(propertyName == "ShapeThickness"))
      return false;
    AnnotationDragEditControl editControl = this.editControl;
    // ISSUE: explicit non-virtual call
    return editControl != null && __nonvirtual (editControl.OnPropertyChanged(propertyName));
  }
}
