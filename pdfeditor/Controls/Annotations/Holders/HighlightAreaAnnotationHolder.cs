// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Annotations.Holders.HighlightAreaAnnotationHolder
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

public class HighlightAreaAnnotationHolder(AnnotationCanvas annotationCanvas) : 
  BaseAnnotationHolder<PdfHighlightAnnotation>(annotationCanvas)
{
  private Rectangle newAreaControl;
  private FS_POINTF createStartPoint;
  private FS_POINTF createEndPoint;
  public Point startPoint;
  private AnnotationFocusControl selectControl;

  public override bool IsTextMarkupAnnotation => true;

  public override void OnPageClientBoundsChanged() => this.selectControl?.InvalidateVisual();

  public override bool OnPropertyChanged(string propertyName)
  {
    if (this.SelectedAnnotation is PdfHighlightAnnotation selectedAnnotation && propertyName == "HighlightAreaStroke")
    {
      MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
      using (requiredService.OperationManager.TraceAnnotationChange(selectedAnnotation.Page))
      {
        FS_COLOR pdfColor = ((Color) ColorConverter.ConvertFromString(requiredService.AnnotationToolbar.AnnotationMenuPropertyAccessor.HighlightAreaStroke)).ToPdfColor();
        selectedAnnotation.Color = pdfColor;
      }
      selectedAnnotation.TryRedrawAnnotation();
    }
    return false;
  }

  protected override void OnCancel()
  {
    if (this.selectControl != null)
    {
      this.AnnotationCanvas.Children.Remove((UIElement) this.selectControl);
      this.selectControl = (AnnotationFocusControl) null;
    }
    if (this.newAreaControl != null)
    {
      this.AnnotationCanvas.Children.Remove((UIElement) this.newAreaControl);
      this.newAreaControl = (Rectangle) null;
    }
    this.createStartPoint = new FS_POINTF();
    this.createEndPoint = new FS_POINTF();
  }

  protected override async Task<System.Collections.Generic.IReadOnlyList<PdfHighlightAnnotation>> OnCompleteCreateNewAsync()
  {
    HighlightAreaAnnotationHolder annotationHolder = this;
    MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
    // ISSUE: explicit non-virtual call
    PdfPage page = __nonvirtual (annotationHolder.CurrentPage);
    if (page.Annots == null)
      page.CreateAnnotations();
    try
    {
      if ((double) Math.Abs(annotationHolder.createStartPoint.X - annotationHolder.createEndPoint.X) <= 10.0 && (double) Math.Abs(annotationHolder.createStartPoint.Y - annotationHolder.createEndPoint.Y) <= 10.0)
        return (System.Collections.Generic.IReadOnlyList<PdfHighlightAnnotation>) null;
      PdfHighlightAnnotation highlight = new PdfHighlightAnnotation(page);
      highlight.Subject = "AreaHighlight";
      highlight.Text = AnnotationAuthorUtil.GetAuthorName();
      Color color = (Color) ColorConverter.ConvertFromString(requiredService.AnnotationToolbar.AnnotationMenuPropertyAccessor.HighlightAreaStroke);
      highlight.Color = new FS_COLOR((int) color.A, (int) color.R, (int) color.G, (int) color.B);
      FS_RECTF rect = new FS_RECTF(Math.Min(annotationHolder.createStartPoint.X, annotationHolder.createEndPoint.X), Math.Max(annotationHolder.createStartPoint.Y, annotationHolder.createEndPoint.Y), Math.Max(annotationHolder.createStartPoint.X, annotationHolder.createEndPoint.X), Math.Min(annotationHolder.createStartPoint.Y, annotationHolder.createEndPoint.Y));
      highlight.QuadPoints = new PdfQuadPointsCollection()
      {
        rect.ToQuadPoints()
      };
      highlight.Flags |= AnnotationFlags.Print;
      highlight.ModificationDate = DateTimeOffset.Now.ToModificationDateString();
      highlight.CreationDate = highlight.ModificationDate;
      if (page.Annots != null)
      {
        page.Annots.Add((PdfAnnotation) highlight);
        highlight.RegenerateAppearancesWithoutRound();
      }
      await requiredService.OperationManager.TraceAnnotationInsertAsync((PdfAnnotation) highlight);
      await page.TryRedrawPageAsync();
      annotationHolder.createStartPoint = new FS_POINTF();
      annotationHolder.createEndPoint = new FS_POINTF();
      if (!((PdfWrapper) highlight != (PdfWrapper) null))
        return (System.Collections.Generic.IReadOnlyList<PdfHighlightAnnotation>) null;
      return (System.Collections.Generic.IReadOnlyList<PdfHighlightAnnotation>) new PdfHighlightAnnotation[1]
      {
        highlight
      };
    }
    finally
    {
      if (annotationHolder.selectControl != null)
      {
        // ISSUE: explicit non-virtual call
        __nonvirtual (annotationHolder.AnnotationCanvas).Children.Remove((UIElement) annotationHolder.selectControl);
        annotationHolder.selectControl = (AnnotationFocusControl) null;
      }
      if (annotationHolder.newAreaControl != null)
      {
        // ISSUE: explicit non-virtual call
        __nonvirtual (annotationHolder.AnnotationCanvas).Children.Remove((UIElement) annotationHolder.newAreaControl);
        annotationHolder.newAreaControl = (Rectangle) null;
      }
    }
  }

  protected override void OnProcessCreateNew(PdfPage page, FS_POINTF pagePoint)
  {
    if (page != this.CurrentPage)
      return;
    this.createEndPoint = pagePoint;
    Point clientPoint;
    if (!this.AnnotationCanvas.PdfViewer.TryGetClientPoint(page.PageIndex, pagePoint.ToPoint(), out clientPoint))
      return;
    this.AnnotationCanvas.PdfViewer.DeselectText();
    double length1 = Math.Min(clientPoint.X, this.startPoint.X);
    double length2 = Math.Min(clientPoint.Y, this.startPoint.Y);
    double num1 = Math.Max(clientPoint.X, this.startPoint.X) - length1;
    double num2 = Math.Max(clientPoint.Y, this.startPoint.Y) - length2;
    if (this.newAreaControl == null)
      return;
    this.newAreaControl.Width = num1;
    this.newAreaControl.Height = num2;
    Canvas.SetLeft((UIElement) this.newAreaControl, length1);
    Canvas.SetTop((UIElement) this.newAreaControl, length2);
  }

  protected override bool OnSelecting(PdfHighlightAnnotation annotation, bool afterCreate)
  {
    this.selectControl = new AnnotationFocusControl(this.AnnotationCanvas)
    {
      Annotation = (PdfAnnotation) annotation,
      IsTextMarkupFocusVisible = !afterCreate
    };
    this.AnnotationCanvas.Children.Add((UIElement) this.selectControl);
    return true;
  }

  protected override bool OnStartCreateNew(PdfPage page, FS_POINTF pagePoint)
  {
    if (this.newAreaControl != null)
      this.AnnotationCanvas.Children.Remove((UIElement) this.newAreaControl);
    this.createStartPoint = pagePoint;
    this.createEndPoint = pagePoint;
    Point clientPoint;
    if (!this.AnnotationCanvas.PdfViewer.TryGetClientPoint(page.PageIndex, pagePoint.ToPoint(), out clientPoint))
      return false;
    this.startPoint = clientPoint;
    object dataContext = this.AnnotationCanvas.DataContext;
    Color color = (Color) ColorConverter.ConvertFromString("#000000");
    Rectangle rectangle = new Rectangle();
    rectangle.Stroke = (Brush) new SolidColorBrush(color);
    rectangle.StrokeThickness = 1.0;
    this.newAreaControl = rectangle;
    Canvas.SetLeft((UIElement) this.newAreaControl, this.startPoint.X);
    Canvas.SetTop((UIElement) this.newAreaControl, this.startPoint.Y);
    this.AnnotationCanvas.Children.Add((UIElement) this.newAreaControl);
    return true;
  }
}
