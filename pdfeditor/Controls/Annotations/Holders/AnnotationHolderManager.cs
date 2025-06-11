// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Annotations.Holders.AnnotationHolderManager
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.Wrappers;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using PDFKit.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace pdfeditor.Controls.Annotations.Holders;

public class AnnotationHolderManager
{
  private readonly AnnotationCanvas annotationCanvas;
  private Dictionary<Type, IAnnotationHolder> holders;
  private IAnnotationHolder currentHolder;
  private bool selecting;
  private bool holderStateChanging;
  private PdfAnnotation selectedAnnotation;

  public AnnotationHolderManager(AnnotationCanvas annotationCanvas)
  {
    this.annotationCanvas = annotationCanvas;
    this.holders = new Dictionary<Type, IAnnotationHolder>();
    this.holders[typeof (PdfLineAnnotation)] = (IAnnotationHolder) new LineAnnotationHolder(annotationCanvas);
    this.holders[typeof (PdfUnderlineAnnotation)] = (IAnnotationHolder) new UnderlineAnnotationHoder(annotationCanvas);
    this.holders[typeof (PdfStrikeoutAnnotation)] = (IAnnotationHolder) new StrikeoutAnnotationHoder(annotationCanvas);
    this.holders[typeof (PdfHighlightAnnotation)] = (IAnnotationHolder) new HighlightAnnotationHolder(annotationCanvas);
    this.holders[typeof (PdfFreeTextAnnotation)] = (IAnnotationHolder) new FreeTextAnnotationHolder(annotationCanvas);
    this.holders[typeof (PdfInkAnnotation)] = (IAnnotationHolder) new InkAnnotationHolder(annotationCanvas);
    this.holders[typeof (PdfSquareAnnotation)] = (IAnnotationHolder) new SquareAnnotationHolder(annotationCanvas);
    this.holders[typeof (PdfLinkAnnotation)] = (IAnnotationHolder) new LinkAnnotationHolder(annotationCanvas);
    this.holders[typeof (PdfCircleAnnotation)] = (IAnnotationHolder) new CircleAnnotationHolder(annotationCanvas);
    this.holders[typeof (PdfTextAnnotation)] = (IAnnotationHolder) new TextAnnotationHolder(annotationCanvas);
    this.holders[typeof (PdfStampAnnotation)] = (IAnnotationHolder) new StampAnnotationHolder(annotationCanvas);
    this.holders[typeof (PdfWatermarkAnnotation)] = (IAnnotationHolder) new WatermarkAnnotationHolder(annotationCanvas);
    this.holders[typeof (AnnotationHolderManager.AreaHighlightAnnotation)] = (IAnnotationHolder) new HighlightAreaAnnotationHolder(annotationCanvas);
    foreach (KeyValuePair<Type, IAnnotationHolder> holder in this.holders)
    {
      IAnnotationHolder annotationHolder1;
      holder.Deconstruct<Type, IAnnotationHolder>(out Type _, out annotationHolder1);
      IAnnotationHolder annotationHolder2 = annotationHolder1;
      annotationHolder2.StateChanged += new EventHandler(this.Holder_StateChanged);
      annotationHolder2.SelectedAnnotationChanged += new EventHandler(this.Holder_SelectedAnnotationChanged);
    }
  }

  public LineAnnotationHolder Line
  {
    get => (LineAnnotationHolder) this.holders[typeof (PdfLineAnnotation)];
  }

  public UnderlineAnnotationHoder Underline
  {
    get => (UnderlineAnnotationHoder) this.holders[typeof (PdfUnderlineAnnotation)];
  }

  public StrikeoutAnnotationHoder Strikeout
  {
    get => (StrikeoutAnnotationHoder) this.holders[typeof (PdfStrikeoutAnnotation)];
  }

  public HighlightAnnotationHolder Highlight
  {
    get => (HighlightAnnotationHolder) this.holders[typeof (PdfHighlightAnnotation)];
  }

  public FreeTextAnnotationHolder FreeText
  {
    get => (FreeTextAnnotationHolder) this.holders[typeof (PdfFreeTextAnnotation)];
  }

  public InkAnnotationHolder Ink => (InkAnnotationHolder) this.holders[typeof (PdfInkAnnotation)];

  public SquareAnnotationHolder Square
  {
    get => (SquareAnnotationHolder) this.holders[typeof (PdfSquareAnnotation)];
  }

  public LinkAnnotationHolder Link
  {
    get => (LinkAnnotationHolder) this.holders[typeof (PdfLinkAnnotation)];
  }

  public CircleAnnotationHolder Circle
  {
    get => (CircleAnnotationHolder) this.holders[typeof (PdfCircleAnnotation)];
  }

  public TextAnnotationHolder Text
  {
    get => (TextAnnotationHolder) this.holders[typeof (PdfTextAnnotation)];
  }

  public StampAnnotationHolder Stamp
  {
    get => (StampAnnotationHolder) this.holders[typeof (PdfStampAnnotation)];
  }

  public WatermarkAnnotationHolder Watermark
  {
    get => (WatermarkAnnotationHolder) this.holders[typeof (PdfWatermarkAnnotation)];
  }

  public HighlightAreaAnnotationHolder HighlightArea
  {
    get
    {
      return (HighlightAreaAnnotationHolder) this.holders[typeof (AnnotationHolderManager.AreaHighlightAnnotation)];
    }
  }

  public IAnnotationHolder CurrentHolder
  {
    get
    {
      IAnnotationHolder currentHolder = this.currentHolder;
      return (currentHolder != null ? (currentHolder.State == AnnotationHolderState.None ? 1 : 0) : 0) != 0 ? (IAnnotationHolder) null : this.currentHolder;
    }
    private set
    {
      if (this.currentHolder == value)
        return;
      this.currentHolder = value;
      EventHandler currentHolderChanged = this.CurrentHolderChanged;
      if (currentHolderChanged == null)
        return;
      currentHolderChanged((object) this, EventArgs.Empty);
    }
  }

  public PdfAnnotation SelectedAnnotation
  {
    get => this.selectedAnnotation;
    set
    {
      if (!((PdfWrapper) this.selectedAnnotation != (PdfWrapper) value))
        return;
      this.selectedAnnotation = value;
      EventHandler annotationChanged = this.SelectedAnnotationChanged;
      if (annotationChanged == null)
        return;
      annotationChanged((object) this, EventArgs.Empty);
    }
  }

  public IAnnotationControl SelectedAnnotationControl
  {
    get
    {
      IAnnotationHolder currentHolder = this.CurrentHolder;
      if (currentHolder == null || currentHolder.State != AnnotationHolderState.Selected)
        return (IAnnotationControl) null;
      PdfAnnotation selectedAnnotation = currentHolder.SelectedAnnotation;
      return this.annotationCanvas.Children.OfType<IAnnotationControl>().FirstOrDefault<IAnnotationControl>((Func<IAnnotationControl, bool>) (c => (PdfWrapper) c.Annotation == (PdfWrapper) selectedAnnotation));
    }
  }

  public bool IsAnnotationDoubleClicked(MouseEventArgs e)
  {
    if (this.SelectedAnnotationControl is FrameworkElement annotationControl)
    {
      Point position = e.GetPosition((IInputElement) annotationControl);
      if (annotationControl.InputHitTest(position) == null)
        return false;
      if (!(annotationControl is AnnotationFreeTextEditor annotationFreeTextEditor))
        return true;
      RichTextBox richTextBox = annotationFreeTextEditor.GetRichTextBox();
      return (richTextBox != null ? (!richTextBox.IsReadOnly ? 1 : 0) : 0) == 0;
    }
    PdfAnnotation selectedAnnotation = this.SelectedAnnotation;
    if ((PdfWrapper) selectedAnnotation != (PdfWrapper) null)
    {
      IAnnotationHolder currentHolder = this.CurrentHolder;
      if ((currentHolder != null ? (currentHolder.IsTextMarkupAnnotation ? 1 : 0) : 0) != 0)
      {
        Point position = e.GetPosition((IInputElement) annotationControl);
        return AnnotationHitTestHelper.HitTest(selectedAnnotation, position);
      }
    }
    return false;
  }

  private void UpdateSelectedAnnotation()
  {
    IAnnotationHolder currentHolder = this.CurrentHolder;
    this.SelectedAnnotation = (currentHolder != null ? (currentHolder.State == AnnotationHolderState.Selected ? 1 : 0) : 0) != 0 ? this.CurrentHolder.SelectedAnnotation : (PdfAnnotation) null;
  }

  private void UpdateCurrentHolder()
  {
    this.CurrentHolder = this.holders.Values.FirstOrDefault<IAnnotationHolder>((Func<IAnnotationHolder, bool>) (c => c.State != 0));
  }

  private void Holder_StateChanged(object sender, EventArgs e)
  {
    if (this.holderStateChanging)
      return;
    this.holderStateChanging = true;
    if (sender is IAnnotationHolder annotationHolder)
    {
      if (annotationHolder.State != AnnotationHolderState.None)
      {
        foreach (KeyValuePair<Type, IAnnotationHolder> holder in this.holders)
        {
          IAnnotationHolder annotationHolder1;
          holder.Deconstruct<Type, IAnnotationHolder>(out Type _, out annotationHolder1);
          IAnnotationHolder annotationHolder2 = annotationHolder1;
          if (annotationHolder2 != annotationHolder && annotationHolder2.State != AnnotationHolderState.None)
            annotationHolder2.Cancel();
        }
      }
      if (!this.selecting)
      {
        this.UpdateCurrentHolder();
        this.UpdateSelectedAnnotation();
      }
    }
    this.holderStateChanging = false;
  }

  private void Holder_SelectedAnnotationChanged(object sender, EventArgs e)
  {
    if (this.selecting)
      return;
    this.UpdateCurrentHolder();
    this.UpdateSelectedAnnotation();
  }

  public void Select(PdfAnnotation annotation, bool afterCreate)
  {
    if (this.selecting)
      return;
    PdfAnnotation selectedAnnotation = this.SelectedAnnotation;
    if ((PdfWrapper) selectedAnnotation != (PdfWrapper) null && (PdfWrapper) selectedAnnotation == (PdfWrapper) annotation)
      return;
    this.selecting = true;
    try
    {
      this.CancelAllCore();
      if ((PdfWrapper) annotation != (PdfWrapper) null)
        this.GetHolder(annotation)?.Select(annotation, afterCreate);
      this.UpdateCurrentHolder();
      this.UpdateSelectedAnnotation();
    }
    finally
    {
      this.selecting = false;
    }
  }

  private IAnnotationHolder GetHolder(PdfAnnotation annotation)
  {
    if ((PdfWrapper) annotation != (PdfWrapper) null)
    {
      Type type = annotation.GetType();
      if (type == typeof (PdfHighlightAnnotation))
        return (annotation as PdfHighlightAnnotation).Subject == "AreaHighlight" ? this.holders[typeof (AnnotationHolderManager.AreaHighlightAnnotation)] : this.holders[typeof (PdfHighlightAnnotation)];
      IAnnotationHolder holder;
      if (this.holders.TryGetValue(type, out holder))
        return holder;
    }
    return (IAnnotationHolder) null;
  }

  public void CancelAll()
  {
    this.selecting = true;
    try
    {
      this.CancelAllCore();
      this.UpdateCurrentHolder();
      this.UpdateSelectedAnnotation();
    }
    finally
    {
      this.selecting = false;
    }
  }

  private void CancelAllCore()
  {
    foreach (KeyValuePair<Type, IAnnotationHolder> holder in this.holders)
    {
      IAnnotationHolder annotationHolder1;
      holder.Deconstruct<Type, IAnnotationHolder>(out Type _, out annotationHolder1);
      IAnnotationHolder annotationHolder2 = annotationHolder1;
      if (annotationHolder2.State != AnnotationHolderState.None)
      {
        annotationHolder2.Cancel();
        Ioc.Default.GetRequiredService<MainViewModel>();
      }
    }
  }

  public Task DeleteAnnotationAsync(PdfAnnotation annotation, bool batchDeletion = false)
  {
    return this.DeleteAnnotationsAsync((System.Collections.Generic.IReadOnlyList<PdfAnnotation>) new PdfAnnotation[1]
    {
      annotation
    }, batchDeletion);
  }

  public async Task DeleteAnnotationsAsync(
    System.Collections.Generic.IReadOnlyList<PdfAnnotation> annotations,
    bool batchDeletion = false)
  {
    AnnotationHolderManager annotationHolderManager = this;
    if (annotations == null)
      throw new ArgumentNullException(nameof (annotations));
    List<PdfAnnotation> deleteList;
    PdfDocument doc;
    if (annotations.Count == 0)
    {
      deleteList = (List<PdfAnnotation>) null;
      doc = (PdfDocument) null;
      vm = (MainViewModel) null;
    }
    else
    {
      deleteList = new List<PdfAnnotation>();
      doc = (PdfDocument) null;
      for (int index = 0; index < annotations.Count; ++index)
      {
        if (annotations[index]?.Page?.Document == null)
          throw new ArgumentNullException(nameof (annotations));
        if (doc == null)
          doc = annotations[index]?.Page?.Document;
        else if (annotations[index]?.Page?.Document != doc)
          throw new ArgumentNullException(nameof (annotations));
        if (annotations[index].Page.Annots != null && annotations[index].Page.Annots.Contains(annotations[index]))
          deleteList.Add(annotations[index]);
      }
      if (deleteList.Count == 0)
      {
        deleteList = (List<PdfAnnotation>) null;
        doc = (PdfDocument) null;
        vm = (MainViewModel) null;
      }
      else if (!(annotationHolderManager.annotationCanvas.DataContext is MainViewModel vm))
      {
        deleteList = (List<PdfAnnotation>) null;
        doc = (PdfDocument) null;
        vm = (MainViewModel) null;
      }
      else
      {
        annotationHolderManager.SelectedAnnotation = (PdfAnnotation) null;
        await PdfAnnotationExtensions.WaitForAnnotationGenerateAsync();
        // ISSUE: reference to a compiler-generated method
        await vm.OperationManager.TraceAnnotationRemoveAsync((System.Collections.Generic.IReadOnlyList<PdfAnnotation>) deleteList.Where<PdfAnnotation>(new Func<PdfAnnotation, bool>(annotationHolderManager.\u003CDeleteAnnotationsAsync\u003Eb__51_0)).ToArray<PdfAnnotation>());
        annotationHolderManager.annotationCanvas.PopupHolder.ClearAnnotationPopup();
        HashSet<int> intSet = new HashSet<int>();
        for (int index = 0; index < deleteList.Count; ++index)
        {
          deleteList[index].DeleteAnnotation();
          if (!batchDeletion && intSet.Add(deleteList[index].Page.PageIndex))
            vm.PageEditors?.NotifyPageAnnotationChanged(deleteList[index].Page.PageIndex);
        }
        PdfPage currentPage = doc.Pages.CurrentPage;
        if (currentPage == null)
        {
          deleteList = (List<PdfAnnotation>) null;
          doc = (PdfDocument) null;
          vm = (MainViewModel) null;
        }
        else
        {
          annotationHolderManager.annotationCanvas.PopupHolder.InitAnnotationPopup(currentPage);
          await currentPage.TryRedrawPageAsync();
          deleteList = (List<PdfAnnotation>) null;
          doc = (PdfDocument) null;
          vm = (MainViewModel) null;
        }
      }
    }
  }

  private bool IsEmbedSignature(PdfAnnotation annotation)
  {
    return annotation is PdfStampAnnotation pdfStampAnnotation && pdfStampAnnotation.Subject == "Signature" && pdfStampAnnotation.Dictionary.ContainsKey("Embed");
  }

  public async Task BatchDeleteAnnotationsAsync(
    System.Collections.Generic.IReadOnlyList<PdfAnnotation> annotations,
    IProgress<double> progress,
    CancellationToken cancellationToken)
  {
    if (annotations == null)
      throw new ArgumentNullException(nameof (annotations));
    List<PdfAnnotation> deleteList;
    PdfDocument doc;
    if (annotations.Count == 0)
    {
      deleteList = (List<PdfAnnotation>) null;
      doc = (PdfDocument) null;
    }
    else
    {
      deleteList = new List<PdfAnnotation>();
      doc = (PdfDocument) null;
      for (int index = 0; index < annotations.Count; ++index)
      {
        if (annotations[index]?.Page?.Document == null)
          throw new ArgumentNullException(nameof (annotations));
        if (doc == null)
          doc = annotations[index]?.Page?.Document;
        else if (annotations[index]?.Page?.Document != doc)
          throw new ArgumentNullException(nameof (annotations));
        if (annotations[index].Page.Annots != null && annotations[index].Page.Annots.Contains(annotations[index]))
          deleteList.Add(annotations[index]);
      }
      if (deleteList.Count == 0)
      {
        deleteList = (List<PdfAnnotation>) null;
        doc = (PdfDocument) null;
      }
      else if (!(this.annotationCanvas.DataContext is MainViewModel))
      {
        deleteList = (List<PdfAnnotation>) null;
        doc = (PdfDocument) null;
      }
      else
      {
        this.SelectedAnnotation = (PdfAnnotation) null;
        await PdfAnnotationExtensions.WaitForAnnotationGenerateAsync();
        this.annotationCanvas.PopupHolder.ClearAnnotationPopup();
        HashSet<int> intSet = new HashSet<int>();
        progress?.Report(0.0);
        for (int i = 0; i < deleteList.Count; ++i)
        {
          deleteList[i].DeleteAnnotation();
          progress?.Report(((double) i + 1.0) / (double) deleteList.Count);
          if (i % 10 == 0)
            await Task.Delay(1);
        }
        PdfPage currentPage = doc.Pages.CurrentPage;
        if (currentPage == null)
        {
          deleteList = (List<PdfAnnotation>) null;
          doc = (PdfDocument) null;
        }
        else
        {
          this.annotationCanvas.PopupHolder.InitAnnotationPopup(currentPage);
          await currentPage.TryRedrawPageAsync();
          deleteList = (List<PdfAnnotation>) null;
          doc = (PdfDocument) null;
        }
      }
    }
  }

  public bool OnPropertyChanged(string propertyName)
  {
    return this.OnPropertyChanged(propertyName, out int _);
  }

  public bool OnPropertyChanged(string propertyName, out int pageIndex)
  {
    pageIndex = -1;
    if (this.CurrentHolder != null && this.CurrentHolder.State == AnnotationHolderState.Selected)
    {
      int num = this.CurrentHolder?.SelectedAnnotation?.Page?.PageIndex ?? -1;
      if (this.CurrentHolder.OnPropertyChanged(propertyName))
      {
        pageIndex = num;
        this.annotationCanvas.PopupHolder.ClearAnnotationPopup();
        PdfPage page = this.CurrentHolder.CurrentPage ?? this.annotationCanvas.PdfViewer?.Document?.Pages?.CurrentPage;
        if (page != null)
          this.annotationCanvas.PopupHolder.InitAnnotationPopup(page);
        if (this.annotationCanvas.DataContext is MainViewModel dataContext)
          dataContext.PageEditors?.NotifyPageAnnotationChanged(pageIndex);
        return true;
      }
    }
    return false;
  }

  public async Task WaitForCancelCompletedAsync()
  {
    await PdfAnnotationExtensions.WaitForAnnotationGenerateAsync();
  }

  public void OnPageClientBoundsChanged()
  {
    if (this.CurrentHolder?.CurrentPage == null)
      return;
    this.CurrentHolder?.OnPageClientBoundsChanged();
  }

  public event EventHandler CurrentHolderChanged;

  public event EventHandler SelectedAnnotationChanged;

  private class AreaHighlightAnnotation
  {
  }
}
