// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Annotations.Holders.TextMarkupAnnotationHolder`1
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Patagames.Pdf;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using PDFKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

#nullable disable
namespace pdfeditor.Controls.Annotations.Holders;

public abstract class TextMarkupAnnotationHolder<TMarkupAnnotation> : 
  BaseAnnotationHolder<TMarkupAnnotation>
  where TMarkupAnnotation : PdfTextMarkupAnnotation
{
  private SelectInfo m_selectInfo = new SelectInfo()
  {
    StartPage = -1
  };
  private FS_POINTF createStartPoint;
  private FS_POINTF createEndPoint;
  private AnnotationFocusControl selectControl;

  public TextMarkupAnnotationHolder(AnnotationCanvas annotationCanvas)
    : base(annotationCanvas)
  {
  }

  public override bool IsTextMarkupAnnotation => true;

  public override void OnPageClientBoundsChanged() => this.selectControl?.InvalidateVisual();

  protected override void OnCancel()
  {
    this.createStartPoint = new FS_POINTF();
    this.createEndPoint = new FS_POINTF();
    this.m_selectInfo = new SelectInfo() { StartPage = -1 };
    if (this.selectControl == null)
      return;
    this.AnnotationCanvas.Children.Remove((UIElement) this.selectControl);
    this.selectControl = (AnnotationFocusControl) null;
  }

  public abstract System.Collections.Generic.IReadOnlyList<TMarkupAnnotation> CreateAnnotation(
    PdfDocument document,
    SelectInfo selectInfo);

  protected virtual bool CheckPointMoved(FS_POINTF point1, FS_POINTF point2)
  {
    return (double) Math.Abs(point1.X - point2.X) > 10.0 || (double) Math.Abs(point1.Y - point2.Y) > 10.0;
  }

  protected override async Task<System.Collections.Generic.IReadOnlyList<TMarkupAnnotation>> OnCompleteCreateNewAsync()
  {
    TextMarkupAnnotationHolder<TMarkupAnnotation> annotationHolder = this;
    MainViewModel requiredService = Ioc.Default.GetRequiredService<MainViewModel>();
    // ISSUE: explicit non-virtual call
    PdfPage currentPage = __nonvirtual (annotationHolder.CurrentPage);
    if (currentPage.Document == null)
      throw new ArgumentException("Document");
    System.Collections.Generic.IReadOnlyList<TMarkupAnnotation> list = (System.Collections.Generic.IReadOnlyList<TMarkupAnnotation>) null;
    if (annotationHolder.m_selectInfo.StartPage != annotationHolder.m_selectInfo.EndPage || annotationHolder.CheckPointMoved(annotationHolder.createStartPoint, annotationHolder.createEndPoint))
      list = annotationHolder.CreateAnnotation(currentPage.Document, annotationHolder.m_selectInfo);
    if (list != null && list.Count > 0)
    {
      string modificationDateString = DateTimeOffset.Now.ToModificationDateString();
      foreach (TMarkupAnnotation markupAnnotation in (IEnumerable<TMarkupAnnotation>) list)
      {
        markupAnnotation.ModificationDate = modificationDateString;
        markupAnnotation.CreationDate = modificationDateString;
      }
      await requiredService.OperationManager.TraceAnnotationInsertAsync((System.Collections.Generic.IReadOnlyList<PdfAnnotation>) list);
      foreach (PdfPage page in list.Select<TMarkupAnnotation, PdfPage>((Func<TMarkupAnnotation, PdfPage>) (c => c.Page)).Distinct<PdfPage>())
        await page.TryRedrawPageAsync();
    }
    annotationHolder.createStartPoint = new FS_POINTF();
    annotationHolder.createEndPoint = new FS_POINTF();
    System.Collections.Generic.IReadOnlyList<TMarkupAnnotation> newAsync = list;
    list = (System.Collections.Generic.IReadOnlyList<TMarkupAnnotation>) null;
    return newAsync;
  }

  protected override void OnProcessCreateNew(PdfPage page, FS_POINTF pagePoint)
  {
    int pageIndex = page.PageIndex;
    int charIndexAtPos = page.Text.GetCharIndexAtPos(pagePoint.X, pagePoint.Y, 10f, 10f);
    if (pageIndex != -1 && charIndexAtPos != -1)
    {
      this.m_selectInfo.EndPage = pageIndex;
      this.m_selectInfo.EndIndex = charIndexAtPos;
    }
    this.createEndPoint = pagePoint;
  }

  protected override bool OnSelecting(TMarkupAnnotation annotation, bool afterCreate)
  {
    this.selectControl = new AnnotationFocusControl(this.AnnotationCanvas)
    {
      Annotation = (PdfAnnotation) annotation,
      IsTextMarkupFocusVisible = true
    };
    this.AnnotationCanvas.Children.Add((UIElement) this.selectControl);
    return true;
  }

  protected override bool OnStartCreateNew(PdfPage page, FS_POINTF pagePoint)
  {
    int pageIndex = page.PageIndex;
    int charIndexAtPos = page.Text.GetCharIndexAtPos(pagePoint.X, pagePoint.Y, 10f, 10f);
    this.m_selectInfo = new SelectInfo()
    {
      StartPage = pageIndex,
      EndPage = pageIndex,
      StartIndex = charIndexAtPos,
      EndIndex = charIndexAtPos
    };
    this.createStartPoint = pagePoint;
    this.createEndPoint = pagePoint;
    return true;
  }
}
