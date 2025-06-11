// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Annotations.Holders.LinkAnnotationHolder
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Actions;
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

public class LinkAnnotationHolder(AnnotationCanvas annotationCanvas) : 
  BaseAnnotationHolder<PdfLinkAnnotation>(annotationCanvas)
{
  private Rectangle newSquareControl;
  private FS_POINTF createStartPoint;
  private FS_POINTF createEndPoint;
  public Point startPoint;
  private AnnotationDragEditControl editControl;
  private int Page;
  private string Filepath;
  private string Linkurl;

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
      this.editControl?.OnPageClientLinkBoundsChanged();
    }
  }

  protected override void OnCancel()
  {
    if (this.newSquareControl != null)
    {
      this.AnnotationCanvas.Children.Remove((UIElement) this.newSquareControl);
      this.newSquareControl = (Rectangle) null;
    }
    if (this.editControl != null)
    {
      this.AnnotationCanvas.Children.Remove((UIElement) this.editControl);
      this.editControl = (AnnotationDragEditControl) null;
    }
    this.createStartPoint = new FS_POINTF();
    this.createEndPoint = new FS_POINTF();
  }

  protected override async Task<System.Collections.Generic.IReadOnlyList<PdfLinkAnnotation>> OnCompleteCreateNewAsync()
  {
    LinkAnnotationHolder annotationHolder = this;
    // ISSUE: explicit non-virtual call
    __nonvirtual (annotationHolder.AnnotationCanvas).Children.Remove((UIElement) annotationHolder.newSquareControl);
    if ((double) Math.Abs(annotationHolder.createStartPoint.X - annotationHolder.createEndPoint.X) > 5.0 || (double) Math.Abs(annotationHolder.createStartPoint.Y - annotationHolder.createEndPoint.Y) > 5.0)
    {
      // ISSUE: explicit non-virtual call
      MainViewModel dataContext = __nonvirtual (annotationHolder.AnnotationCanvas).DataContext as MainViewModel;
      LinkEditWindows linkEditWindows = new LinkEditWindows(dataContext.Document);
      linkEditWindows.Owner = App.Current.MainWindow;
      linkEditWindows.WindowStartupLocation = linkEditWindows.Owner != null ? WindowStartupLocation.CenterOwner : WindowStartupLocation.CenterScreen;
      bool? nullable = linkEditWindows.ShowDialog();
      MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
      // ISSUE: explicit non-virtual call
      PdfPage page = __nonvirtual (annotationHolder.CurrentPage);
      if (page.Annots == null)
        page.CreateAnnotations();
      if (nullable.GetValueOrDefault())
      {
        PdfLinkAnnotation LinkAnnot = new PdfLinkAnnotation(page);
        if (linkEditWindows.SelectedType == LinkSelect.ToPage)
        {
          annotationHolder.Page = linkEditWindows.Page - 1;
          PdfDestination xyz = PdfDestination.CreateXYZ(dataContext.Document, annotationHolder.Page, top: new float?(dataContext.Document.Pages[annotationHolder.Page].Height));
          LinkAnnot.Link.Action = (PdfAction) new PdfGoToAction(dataContext.Document, xyz);
        }
        else if (linkEditWindows.SelectedType == LinkSelect.ToWeb)
          LinkAnnot.Link.Action = (PdfAction) new PdfUriAction(dataContext.Document, linkEditWindows.UrlFilePath);
        else if (linkEditWindows.SelectedType == LinkSelect.ToFile)
          LinkAnnot.Link.Action = (PdfAction) new PdfLaunchAction(dataContext.Document, new PdfFileSpecification(dataContext.Document)
          {
            FileName = linkEditWindows.FileDiaoligFiePath
          });
        Color color = (Color) ColorConverter.ConvertFromString(linkEditWindows.SelectedFontground);
        FS_COLOR fsColor = new FS_COLOR((int) color.A, (int) color.R, (int) color.G, (int) color.B);
        float num;
        if (!linkEditWindows.rectangleVis)
        {
          num = 0.0f;
        }
        else
        {
          LinkAnnot.Color = fsColor;
          num = linkEditWindows.BorderWidth;
        }
        LinkAnnot.Rectangle = new FS_RECTF(Math.Min(annotationHolder.createStartPoint.X, annotationHolder.createEndPoint.X), Math.Max(annotationHolder.createStartPoint.Y, annotationHolder.createEndPoint.Y), Math.Max(annotationHolder.createStartPoint.X, annotationHolder.createEndPoint.X), Math.Min(annotationHolder.createStartPoint.Y, annotationHolder.createEndPoint.Y));
        LinkAnnot.ModificationDate = DateTimeOffset.Now.ToModificationDateString();
        LinkAnnot.Flags |= AnnotationFlags.Print;
        LinkAnnot.SetBorderStyle(new PdfBorderStyle()
        {
          Width = num,
          Style = linkEditWindows.BorderStyles,
          DashPattern = new float[2]{ 2f, 4f }
        });
        page.Annots.Add((PdfAnnotation) LinkAnnot);
        await requiredService.OperationManager.TraceAnnotationInsertAsync((PdfAnnotation) LinkAnnot);
        await page.TryRedrawPageAsync();
        annotationHolder.createStartPoint = new FS_POINTF();
        annotationHolder.createEndPoint = new FS_POINTF();
        if (!((PdfWrapper) LinkAnnot != (PdfWrapper) null))
          return (System.Collections.Generic.IReadOnlyList<PdfLinkAnnotation>) null;
        return (System.Collections.Generic.IReadOnlyList<PdfLinkAnnotation>) new PdfLinkAnnotation[1]
        {
          LinkAnnot
        };
      }
      page = (PdfPage) null;
    }
    return (System.Collections.Generic.IReadOnlyList<PdfLinkAnnotation>) null;
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

  protected override bool OnStartCreateNew(PdfPage page, FS_POINTF pagePoint)
  {
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

  protected override bool OnSelecting(PdfLinkAnnotation annotation, bool afterCreate)
  {
    try
    {
      this.editControl = new AnnotationDragEditControl(annotation, (IAnnotationHolder) this);
      this.AnnotationCanvas.Children.Add((UIElement) this.editControl);
      return true;
    }
    catch (Exception ex)
    {
      return false;
    }
  }
}
