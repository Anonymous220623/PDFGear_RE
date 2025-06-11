// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.PdfViewerDecorators.AnnotationPopupHolder
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.Wrappers;
using pdfeditor.Controls.Annotations;
using PDFKit.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Controls.PdfViewerDecorators;

public class AnnotationPopupHolder
{
  private readonly AnnotationCanvas annotationCanvas;
  private ConcurrentDictionary<int, PopupAnnotationCollection> annotPanels = new ConcurrentDictionary<int, PopupAnnotationCollection>();

  public AnnotationPopupHolder(AnnotationCanvas annotationCanvas)
  {
    this.annotationCanvas = annotationCanvas ?? throw new ArgumentNullException(nameof (annotationCanvas));
  }

  public AnnotationCanvas AnnotationCanvas => this.annotationCanvas;

  public void ClearAnnotationPopup()
  {
    foreach (KeyValuePair<int, PopupAnnotationCollection> annotPanel in this.annotPanels)
    {
      annotPanel.Value.Loaded -= new RoutedEventHandler(this.PopupCollection_Loaded);
      this.annotationCanvas.Children.Remove((UIElement) annotPanel.Value);
    }
    this.annotPanels.Clear();
  }

  public void InitAnnotationPopup(PdfPage page)
  {
    if (!this.annotationCanvas.IsAnnotationVisible || (page != null ? page.PageIndex : -1) < 0 || this.annotPanels.ContainsKey(page.PageIndex))
      return;
    PopupAnnotationCollection element = new PopupAnnotationCollection(this.annotationCanvas, page);
    this.annotPanels[page.PageIndex] = element;
    Panel.SetZIndex((UIElement) element, 2);
    this.annotationCanvas.Children.Add((UIElement) element);
    element.UpdatePosition();
    PdfAnnotation selectedAnnotation = this.annotationCanvas.HolderManager.SelectedAnnotation;
    if ((PdfWrapper) selectedAnnotation != (PdfWrapper) null && selectedAnnotation.Page == page)
      this.SetPopupSelected(selectedAnnotation, true);
    element.Loaded += new RoutedEventHandler(this.PopupCollection_Loaded);
  }

  public void FlushAnnotationPopup()
  {
    if (this.annotationCanvas.PdfViewer?.Document?.Pages?.CurrentPage == null)
      return;
    this.ClearAnnotationPopup();
    this.InitAnnotationPopup(this.annotationCanvas.PdfViewer?.Document?.Pages?.CurrentPage);
  }

  private void PopupCollection_Loaded(object sender, RoutedEventArgs e)
  {
    this.annotationCanvas.UpdateViewerFlyoutExtendWidth();
  }

  public void SetPopupHovered(PdfAnnotation annot, bool value)
  {
    int key = annot?.Page?.PageIndex ?? -1;
    PopupAnnotationCollection annotationCollection;
    if (key < 0 || !this.annotPanels.TryGetValue(key, out annotationCollection))
      return;
    annotationCollection.SetHovered(annot, value);
  }

  public void SetPopupSelected(PdfAnnotation annot, bool value)
  {
    int key = annot?.Page?.PageIndex ?? -1;
    PopupAnnotationCollection annotationCollection;
    if (key < 0 || !this.annotPanels.TryGetValue(key, out annotationCollection))
      return;
    annotationCollection.SetSelected(annot, value);
  }

  public void UpdatePanelsPosition()
  {
    foreach (PopupAnnotationCollection annotationCollection in (IEnumerable<PopupAnnotationCollection>) this.annotPanels.Values)
      annotationCollection.UpdatePosition();
  }

  public void KillFocus()
  {
    foreach (PopupAnnotationCollection annotationCollection in (IEnumerable<PopupAnnotationCollection>) this.annotPanels.Values)
      annotationCollection.KillFocus();
  }

  public double GetMaxPopupWidth()
  {
    return (double) this.annotPanels.Values.SelectMany<PopupAnnotationCollection, float>((Func<PopupAnnotationCollection, IEnumerable<float>>) (c => c.Children.OfType<AnnotationPopupControl>().Select<AnnotationPopupControl, float>((Func<AnnotationPopupControl, float>) (x => x.Wrapper.Annotation.GetRECT().Width)))).DefaultIfEmpty<float>().Max();
  }

  public bool IsPopupVisible(PdfAnnotation annot)
  {
    PopupAnnotationCollection annotationCollection;
    return this.annotPanels.TryGetValue(annot.Page.PageIndex, out annotationCollection) && annotationCollection.IsPopupVisible(annot);
  }

  public bool TryShowPopup(PdfAnnotation annot)
  {
    PopupAnnotationCollection annotationCollection;
    return this.annotPanels.TryGetValue(annot.Page.PageIndex, out annotationCollection) && !annotationCollection.IsPopupVisible(annot) && annotationCollection.ShowPopup(annot, true);
  }

  public void FocusPopupTextBox(PdfAnnotation annotation, bool afterCreate)
  {
    if (annotation == null)
      throw new ArgumentNullException(nameof (annotation));
    PopupAnnotationCollection panel;
    if (!this.annotPanels.TryGetValue(annotation.Page.PageIndex, out panel))
      return;
    Action method = (Action) (() =>
    {
      panel.TryBringPopupControlIntoView(annotation);
      TextBox popupTextBox = panel.GetPopupTextBox(annotation);
      if (popupTextBox == null)
        return;
      popupTextBox.Focus();
      if (!afterCreate)
        return;
      popupTextBox.SelectAll();
    });
    if (!panel.IsLoaded)
      panel.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) method);
    else
      method();
  }
}
