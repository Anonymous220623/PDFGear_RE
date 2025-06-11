// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Annotations.Holders.TextAnnotationHolder
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using PDFKit;
using PDFKit.Utils;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Controls.Annotations.Holders;

public class TextAnnotationHolder(AnnotationCanvas annotationCanvas) : 
  BaseAnnotationHolder<PdfTextAnnotation>(annotationCanvas)
{
  private FS_POINTF createPoint;
  private AnnotationTextControl editControl;

  public override bool IsTextMarkupAnnotation => false;

  public override void OnPageClientBoundsChanged()
  {
    if (this.State == AnnotationHolderState.CreatingNew)
    {
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
    this.createPoint = new FS_POINTF();
    if (this.editControl == null)
      return;
    this.AnnotationCanvas.Children.Remove((UIElement) this.editControl);
    this.editControl = (AnnotationTextControl) null;
  }

  protected override async Task<System.Collections.Generic.IReadOnlyList<PdfTextAnnotation>> OnCompleteCreateNewAsync()
  {
    TextAnnotationHolder annotationHolder = this;
    PdfTextAnnotation textAnnot = (PdfTextAnnotation) null;
    MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
    requiredService.AnnotationMode = AnnotationMode.None;
    // ISSUE: explicit non-virtual call
    PdfPage page = __nonvirtual (annotationHolder.CurrentPage);
    if (page.Annots == null)
      page.CreateAnnotations();
    textAnnot = new PdfTextAnnotation(page);
    textAnnot.Flags |= AnnotationFlags.Print | AnnotationFlags.NoZoom | AnnotationFlags.NoRotate;
    textAnnot.Color = FS_COLOR.Red;
    textAnnot.Contents = "";
    textAnnot.Opacity = 1f;
    textAnnot.Subject = "";
    textAnnot.Text = AnnotationAuthorUtil.GetAuthorName();
    textAnnot.StandardIconName = IconNames.Note;
    textAnnot.Rectangle = TextAnnotationHolder.GetBounds(annotationHolder.createPoint, textAnnot.StandardIconName);
    textAnnot.ModificationDate = DateTimeOffset.Now.ToModificationDateString();
    textAnnot.CreationDate = textAnnot.ModificationDate;
    PdfPopupAnnotation pdfPopupAnnotation = new PdfPopupAnnotation(page);
    pdfPopupAnnotation.Parent = (PdfAnnotation) textAnnot;
    pdfPopupAnnotation.IsOpen = true;
    // ISSUE: explicit non-virtual call
    pdfPopupAnnotation.Rectangle = TextAnnotationHolder.GetPopupBounds(textAnnot.GetRECT(), __nonvirtual (annotationHolder.AnnotationCanvas)?.PdfViewer, page, 180.0, 140.0);
    textAnnot.Popup = pdfPopupAnnotation;
    textAnnot.RegenerateAppearancesAdvance();
    page.Annots.Add((PdfAnnotation) textAnnot);
    page.Annots.Add((PdfAnnotation) pdfPopupAnnotation);
    // ISSUE: explicit non-virtual call
    __nonvirtual (annotationHolder.AnnotationCanvas).PopupHolder.ClearAnnotationPopup();
    // ISSUE: explicit non-virtual call
    // ISSUE: explicit non-virtual call
    __nonvirtual (annotationHolder.AnnotationCanvas).PopupHolder.InitAnnotationPopup(__nonvirtual (annotationHolder.AnnotationCanvas).PdfViewer.Document?.Pages?.CurrentPage);
    await requiredService.OperationManager.TraceAnnotationInsertAsync((PdfAnnotation) textAnnot);
    await page.TryRedrawPageAsync();
    annotationHolder.createPoint = new FS_POINTF();
    System.Collections.Generic.IReadOnlyList<PdfTextAnnotation> newAsync = (System.Collections.Generic.IReadOnlyList<PdfTextAnnotation>) new PdfTextAnnotation[1]
    {
      textAnnot
    };
    textAnnot = (PdfTextAnnotation) null;
    page = (PdfPage) null;
    return newAsync;
  }

  private static FS_RECTF GetPopupBounds(
    FS_RECTF textAnnotRect,
    PdfViewer viewer,
    PdfPage page,
    double width,
    double height)
  {
    float l = textAnnotRect.right + 40f;
    if (viewer != null && page.Rotation == PageRotate.Normal)
    {
      Rect rect = viewer.CalcActualRect(page.PageIndex);
      if (!rect.IsEmpty && rect != new Rect() && viewer.ViewportWidth / 3.0 * 2.0 > rect.Width)
        l = page.GetEffectiveSize().Width + 8f;
    }
    return new FS_RECTF((double) l, (double) textAnnotRect.top, (double) l + width, (double) textAnnotRect.top - height);
  }

  private static FS_RECTF GetBounds(FS_POINTF point, IconNames icon)
  {
    float num1 = 20f;
    float num2 = 20f;
    switch (icon)
    {
      case IconNames.Note:
        num2 = 17.696f;
        num1 = 20.836f;
        break;
      case IconNames.Comment:
        num2 = 19.414f;
        num1 = 19.414f;
        break;
      case IconNames.Key:
        num2 = 11.208f;
        num1 = 20.036f;
        break;
      case IconNames.Help:
        num2 = 21.71f;
        num1 = 21.712f;
        break;
      case IconNames.NewParagraph:
        num2 = 15.612f;
        num1 = 21.175f;
        break;
      case IconNames.Paragraph:
        num2 = 21.324f;
        num1 = 21.766f;
        break;
      case IconNames.Insert:
        num2 = 19.802f;
        num1 = 22.031f;
        break;
    }
    return new FS_RECTF(point.X, point.Y + num1, point.X + num2, point.Y);
  }

  protected override void OnProcessCreateNew(PdfPage page, FS_POINTF pagePoint)
  {
    if (page != this.CurrentPage)
      return;
    this.createPoint = pagePoint;
  }

  protected override bool OnSelecting(PdfTextAnnotation annotation, bool afterCreate)
  {
    this.editControl = this.editControl == null ? new AnnotationTextControl(annotation, this) : throw new ArgumentException("editControl");
    this.AnnotationCanvas.Children.Add((UIElement) this.editControl);
    this.AnnotationCanvas.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, (Delegate) (() =>
    {
      PdfViewer pdfViewer = this.AnnotationCanvas.PdfViewer;
      PdfPageCollection pages = pdfViewer?.Document?.Pages;
      if (pages != null && pages.CurrentPage != annotation.Page)
        pdfViewer.CurrentIndex = annotation.Page.PageIndex;
      if (!afterCreate)
        return;
      this.AnnotationCanvas.PopupHolder.FocusPopupTextBox((PdfAnnotation) annotation, afterCreate);
    }));
    return true;
  }

  protected override bool OnStartCreateNew(PdfPage page, FS_POINTF pagePoint)
  {
    this.createPoint = pagePoint;
    return true;
  }

  public override bool OnPropertyChanged(string propertyName) => false;
}
