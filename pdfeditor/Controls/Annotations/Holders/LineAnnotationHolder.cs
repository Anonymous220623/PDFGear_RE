// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Annotations.Holders.LineAnnotationHolder
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
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace pdfeditor.Controls.Annotations.Holders;

public class LineAnnotationHolder(AnnotationCanvas annotationCanvas) : 
  BaseAnnotationHolder<PdfLineAnnotation>(annotationCanvas)
{
  private Line newLineControl;
  private FS_POINTF createStartPoint;
  private FS_POINTF createEndPoint;
  private AnnotationLineControl editLineControl;

  public override bool IsTextMarkupAnnotation => false;

  public override void OnPageClientBoundsChanged()
  {
    if (this.State == AnnotationHolderState.CreatingNew)
    {
      if (this.newLineControl == null)
        throw new ArgumentException("newLineControl");
      if (this.CurrentPage == null)
        throw new ArgumentException("CurrentPage");
      Point clientPoint1;
      Point clientPoint2;
      if (!this.AnnotationCanvas.PdfViewer.TryGetClientPoint(this.CurrentPage.PageIndex, this.createStartPoint.ToPoint(), out clientPoint1) || !this.AnnotationCanvas.PdfViewer.TryGetClientPoint(this.CurrentPage.PageIndex, this.createEndPoint.ToPoint(), out clientPoint2))
        return;
      this.newLineControl.X1 = clientPoint1.X;
      this.newLineControl.Y1 = clientPoint1.Y;
      this.newLineControl.X2 = clientPoint2.X;
      this.newLineControl.Y2 = clientPoint2.Y;
    }
    else
    {
      if (this.State != AnnotationHolderState.Selected)
        return;
      this.editLineControl?.OnPageClientBoundsChanged();
    }
  }

  protected override void OnCancel()
  {
    if (this.newLineControl != null)
    {
      this.AnnotationCanvas.Children.Remove((UIElement) this.newLineControl);
      this.newLineControl = (Line) null;
    }
    this.createStartPoint = new FS_POINTF();
    this.createEndPoint = new FS_POINTF();
    if (this.editLineControl == null)
      return;
    if (this.editLineControl.IsMouseCaptured)
      this.editLineControl.ReleaseMouseCapture();
    this.AnnotationCanvas.Children.Remove((UIElement) this.editLineControl);
    this.editLineControl = (AnnotationLineControl) null;
  }

  protected override async Task<System.Collections.Generic.IReadOnlyList<PdfLineAnnotation>> OnCompleteCreateNewAsync()
  {
    LineAnnotationHolder annotationHolder = this;
    // ISSUE: explicit non-virtual call
    __nonvirtual (annotationHolder.AnnotationCanvas).Children.Remove((UIElement) annotationHolder.newLineControl);
    PdfLineAnnotation newLineAnnot = (PdfLineAnnotation) null;
    MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
    // ISSUE: explicit non-virtual call
    PdfPage page = __nonvirtual (annotationHolder.CurrentPage);
    if (page.Annots == null)
      page.CreateAnnotations();
    if ((double) Math.Abs(annotationHolder.createStartPoint.X - annotationHolder.createEndPoint.X) > 10.0 || (double) Math.Abs(annotationHolder.createStartPoint.Y - annotationHolder.createEndPoint.Y) > 10.0)
    {
      newLineAnnot = new PdfLineAnnotation(page);
      Color color = (Color) ColorConverter.ConvertFromString(requiredService.AnnotationToolbar.AnnotationMenuPropertyAccessor.LineStroke);
      newLineAnnot.Color = new FS_COLOR((int) color.A, (int) color.R, (int) color.G, (int) color.B);
      newLineAnnot.CaptionPosition = CaptionPositions.Inline;
      newLineAnnot.Line = new PdfLinePointCollection<PdfLineAnnotation>();
      newLineAnnot.Line.Add(new FS_POINTF(annotationHolder.createStartPoint.X, annotationHolder.createStartPoint.Y));
      newLineAnnot.Line.Add(new FS_POINTF(annotationHolder.createEndPoint.X, annotationHolder.createEndPoint.Y));
      newLineAnnot.LineStyle = new PdfBorderStyle();
      newLineAnnot.LineStyle.Width = requiredService.AnnotationToolbar.AnnotationMenuPropertyAccessor.LineWidth;
      newLineAnnot.Text = AnnotationAuthorUtil.GetAuthorName();
      if (requiredService.AnnotationMode == AnnotationMode.Arrow)
        newLineAnnot.LineEnding = new PdfLineEndingCollection(LineEndingStyles.None, LineEndingStyles.OpenArrow);
      newLineAnnot.Cap = false;
      newLineAnnot.ModificationDate = DateTimeOffset.Now.ToModificationDateString();
      newLineAnnot.CreationDate = newLineAnnot.ModificationDate;
      newLineAnnot.Flags |= AnnotationFlags.Print;
      page.Annots.Add((PdfAnnotation) newLineAnnot);
      await requiredService.OperationManager.TraceAnnotationInsertAsync((PdfAnnotation) newLineAnnot);
      await page.TryRedrawPageAsync();
    }
    annotationHolder.newLineControl = (Line) null;
    annotationHolder.createStartPoint = new FS_POINTF();
    annotationHolder.createEndPoint = new FS_POINTF();
    if (!((PdfWrapper) newLineAnnot != (PdfWrapper) null))
      return (System.Collections.Generic.IReadOnlyList<PdfLineAnnotation>) null;
    return (System.Collections.Generic.IReadOnlyList<PdfLineAnnotation>) new PdfLineAnnotation[1]
    {
      newLineAnnot
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
    this.newLineControl.X2 = clientPoint.X;
    this.newLineControl.Y2 = clientPoint.Y;
  }

  private Line CreateLine(Point startPoint)
  {
    MainViewModel dataContext = this.AnnotationCanvas.DataContext as MainViewModel;
    SolidColorBrush solidColorBrush = Brushes.Transparent;
    if (!string.IsNullOrEmpty(dataContext.AnnotationToolbar.AnnotationMenuPropertyAccessor.LineStroke))
    {
      try
      {
        solidColorBrush = new SolidColorBrush((Color) ColorConverter.ConvertFromString(dataContext.AnnotationToolbar.AnnotationMenuPropertyAccessor.LineStroke));
      }
      catch
      {
      }
    }
    Line line = new Line();
    line.Stroke = (Brush) solidColorBrush;
    line.StrokeThickness = (double) dataContext.AnnotationToolbar.AnnotationMenuPropertyAccessor.LineWidth;
    line.X1 = startPoint.X;
    line.Y1 = startPoint.Y;
    line.X2 = startPoint.X;
    line.Y2 = startPoint.Y;
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
    this.newLineControl = this.CreateLine(clientPoint);
    this.AnnotationCanvas.Children.Add((UIElement) this.newLineControl);
    return true;
  }

  protected override bool OnSelecting(PdfLineAnnotation annotation, bool afterCreate)
  {
    this.editLineControl = this.editLineControl == null ? new AnnotationLineControl(annotation, (IAnnotationHolder) this) : throw new ArgumentException("editLineControl");
    this.AnnotationCanvas.Children.Add((UIElement) this.editLineControl);
    return true;
  }

  public override bool OnPropertyChanged(string propertyName)
  {
    if (!(propertyName == "LineStroke") && !(propertyName == "LineWidth"))
      return false;
    AnnotationLineControl editLineControl = this.editLineControl;
    // ISSUE: explicit non-virtual call
    return editLineControl != null && __nonvirtual (editLineControl.OnPropertyChanged(propertyName));
  }
}
