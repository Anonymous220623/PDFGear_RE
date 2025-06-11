// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Annotations.Holders.BaseAnnotationHolder`1
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.Wrappers;
using pdfeditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable disable
namespace pdfeditor.Controls.Annotations.Holders;

public abstract class BaseAnnotationHolder<TCreateAnnotReturn> : IAnnotationHolder where TCreateAnnotReturn : PdfAnnotation
{
  private AnnotationHolderState state;
  private PdfAnnotation currentAnnotation;

  public BaseAnnotationHolder(AnnotationCanvas annotationCanvas)
  {
    this.AnnotationCanvas = annotationCanvas;
  }

  public abstract bool IsTextMarkupAnnotation { get; }

  public AnnotationCanvas AnnotationCanvas { get; }

  public PdfPage CurrentPage { get; protected set; }

  public PdfAnnotation SelectedAnnotation
  {
    get => this.currentAnnotation;
    private set
    {
      if (!((PdfWrapper) this.currentAnnotation != (PdfWrapper) value))
        return;
      this.currentAnnotation = value;
      EventHandler annotationChanged = this.SelectedAnnotationChanged;
      if (annotationChanged == null)
        return;
      annotationChanged((object) this, EventArgs.Empty);
    }
  }

  public AnnotationHolderState State
  {
    get => this.state;
    protected set
    {
      if (this.state == value)
        return;
      this.state = value;
      EventHandler stateChanged = this.StateChanged;
      if (stateChanged == null)
        return;
      stateChanged((object) this, EventArgs.Empty);
    }
  }

  public virtual void StartCreateNew(PdfPage page, FS_POINTF pagePoint)
  {
    if (this.State != AnnotationHolderState.None)
      throw new ArgumentException(this.State.ToString());
    if (this.CurrentPage != null)
      throw new ArgumentException("CurrentPage");
    this.State = AnnotationHolderState.CreateNewStarting;
    this.CurrentPage = page;
    bool flag = false;
    try
    {
      flag = this.OnStartCreateNew(page, pagePoint);
    }
    finally
    {
      if (flag)
      {
        this.State = AnnotationHolderState.CreatingNew;
      }
      else
      {
        this.State = AnnotationHolderState.None;
        this.CurrentPage = (PdfPage) null;
      }
    }
  }

  public virtual void ProcessCreateNew(PdfPage page, FS_POINTF pagePoint)
  {
    if (this.State != AnnotationHolderState.CreatingNew)
      throw new ArgumentException(this.State.ToString());
    if (this.CurrentPage == null)
      throw new ArgumentException("CurrentPage");
    this.OnProcessCreateNew(page, pagePoint);
  }

  async Task<System.Collections.Generic.IReadOnlyList<PdfAnnotation>> IAnnotationHolder.CompleteCreateNewAsync()
  {
    return (System.Collections.Generic.IReadOnlyList<PdfAnnotation>) await this.CompleteCreateNewAsync();
  }

  public virtual async Task<System.Collections.Generic.IReadOnlyList<TCreateAnnotReturn>> CompleteCreateNewAsync()
  {
    if (this.CurrentPage == null)
      throw new ArgumentException("CurrentPage");
    this.State = AnnotationHolderState.CreateNewCompleting;
    System.Collections.Generic.IReadOnlyList<TCreateAnnotReturn> newAsync1;
    try
    {
      System.Collections.Generic.IReadOnlyList<TCreateAnnotReturn> newAsync2 = await this.OnCompleteCreateNewAsync();
      if (newAsync2 != null)
      {
        if (this.AnnotationCanvas.DataContext is MainViewModel dataContext)
        {
          foreach (TCreateAnnotReturn createAnnotReturn in (IEnumerable<TCreateAnnotReturn>) newAsync2)
          {
            if ((object) createAnnotReturn is PdfMarkupAnnotation && dataContext != null)
              dataContext.PageEditors?.NotifyPageAnnotationChanged(createAnnotReturn.Page.PageIndex);
          }
        }
        try
        {
          TCreateAnnotReturn createAnnotReturn = newAsync2.Count > 0 ? newAsync2[0] : default (TCreateAnnotReturn);
          if (createAnnotReturn is PdfHighlightAnnotation highlightAnnotation)
          {
            if (!string.IsNullOrWhiteSpace(highlightAnnotation.Subject) && highlightAnnotation.Subject == "AreaHighlight")
              GAManager.SendEvent("AnnotationAction", "PdfAreaHighlightAnnotation", "New", 1L);
            else
              GAManager.SendEvent("AnnotationAction", "PdfHighlightAnnotation", "New", 1L);
          }
          if ((object) createAnnotReturn is PdfStrikeoutAnnotation)
            GAManager.SendEvent("AnnotationAction", "PdfStrikeoutAnnotation", "New", 1L);
          if ((object) createAnnotReturn is PdfUnderlineAnnotation)
            GAManager.SendEvent("AnnotationAction", "PdfUnderlineAnnotation", "New", 1L);
          if ((object) createAnnotReturn is PdfLineAnnotation)
            GAManager.SendEvent("AnnotationAction", "PdfLineAnnotation", "New", 1L);
          if ((object) createAnnotReturn is PdfSquareAnnotation)
            GAManager.SendEvent("AnnotationAction", "PdfSquareAnnotation", "New", 1L);
          if ((object) createAnnotReturn is PdfCircleAnnotation)
            GAManager.SendEvent("AnnotationAction", "PdfCircleAnnotation", "New", 1L);
          if ((object) createAnnotReturn is PdfInkAnnotation)
            GAManager.SendEvent("AnnotationAction", "PdfInkAnnotation", "New", 1L);
          if ((object) createAnnotReturn is PdfLinkAnnotation)
            GAManager.SendEvent("AnnotationAction", "PdfLinkAnnotation", "New", 1L);
          if (createAnnotReturn is PdfFreeTextAnnotation freeTextAnnotation)
          {
            if (freeTextAnnotation.Intent == AnnotationIntent.FreeTextTypeWriter)
              GAManager.SendEvent("AnnotationAction", "PdfFreeTextAnnotationTransparent", "New", 1L);
            else
              GAManager.SendEvent("AnnotationAction", "PdfFreeTextAnnotation", "New", 1L);
          }
          if ((object) createAnnotReturn is PdfTextAnnotation)
            GAManager.SendEvent("AnnotationAction", "PdfTextAnnotation", "New", 1L);
          if (createAnnotReturn is PdfStampAnnotation pdfStampAnnotation)
          {
            if (!string.IsNullOrWhiteSpace(pdfStampAnnotation.Subject) && pdfStampAnnotation.Subject == "Signature")
              GAManager.SendEvent("AnnotationAction", "PdfStampAnnotationSignature", "New", 1L);
            else if (!string.IsNullOrWhiteSpace(pdfStampAnnotation.Subject) && pdfStampAnnotation.Subject == "FormControl")
              GAManager.SendEvent("AnnotationAction", "PdfStampAnnotationForm", "New", 1L);
            else
              GAManager.SendEvent("AnnotationAction", "PdfStampAnnotation", "New", 1L);
          }
        }
        catch
        {
        }
      }
      newAsync1 = newAsync2;
    }
    finally
    {
      this.State = AnnotationHolderState.None;
      this.CurrentPage = (PdfPage) null;
    }
    return newAsync1;
  }

  public void Select(PdfAnnotation annotation, bool afterCreate)
  {
    if (!(annotation is TCreateAnnotReturn))
      throw new ArgumentException(nameof (annotation));
    if (this.State != AnnotationHolderState.None)
      throw new ArgumentException(this.State.ToString());
    this.CurrentPage = annotation != null ? annotation.Page : throw new ArgumentNullException(nameof (annotation));
    bool flag = false;
    try
    {
      flag = this.OnSelecting((TCreateAnnotReturn) annotation, afterCreate);
    }
    finally
    {
      if (flag)
      {
        this.State = AnnotationHolderState.Selected;
        this.SelectedAnnotation = annotation;
      }
      else
      {
        this.State = AnnotationHolderState.None;
        this.SelectedAnnotation = (PdfAnnotation) null;
        this.CurrentPage = (PdfPage) null;
      }
    }
  }

  public virtual void Cancel()
  {
    if (this.AnnotationCanvas.Dispatcher.CheckAccess())
    {
      CancelCore();
    }
    else
    {
      // ISSUE: method pointer
      this.AnnotationCanvas.Dispatcher.Invoke(new Action((object) this, __methodptr(\u003CCancel\u003Eg__CancelCore\u007C23_0)));
    }

    void CancelCore()
    {
      this.OnCancel();
      this.State = AnnotationHolderState.None;
      this.CurrentPage = (PdfPage) null;
      this.SelectedAnnotation = (PdfAnnotation) null;
      EventHandler canceled = this.Canceled;
      if (canceled == null)
        return;
      canceled((object) this, EventArgs.Empty);
    }
  }

  protected abstract bool OnStartCreateNew(PdfPage page, FS_POINTF pagePoint);

  protected abstract void OnProcessCreateNew(PdfPage page, FS_POINTF pagePoint);

  protected abstract Task<System.Collections.Generic.IReadOnlyList<TCreateAnnotReturn>> OnCompleteCreateNewAsync();

  public abstract void OnPageClientBoundsChanged();

  protected abstract void OnCancel();

  protected abstract bool OnSelecting(TCreateAnnotReturn annotation, bool afterCreate);

  public abstract bool OnPropertyChanged(string propertyName);

  public event EventHandler Canceled;

  public event EventHandler StateChanged;

  public event EventHandler SelectedAnnotationChanged;
}
