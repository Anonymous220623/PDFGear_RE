// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.CommetControl
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using pdfeditor.Models.Commets;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using PDFKit;
using PDFKit.Utils;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Controls;

[TemplatePart(Name = "PART_CommetTreeView", Type = typeof (CommetTreeView))]
public partial class CommetControl : Control
{
  private const string CommetTreeViewName = "PART_CommetTreeView";
  private bool innerSet;
  private CommetTreeView commetTreeView;
  public static readonly DependencyProperty AllPageCommetsProperty = DependencyProperty.Register(nameof (AllPageCommets), typeof (AllPageCommetCollectionView), typeof (CommetControl), new PropertyMetadata((object) null, new PropertyChangedCallback(CommetControl.OnAllPageCommetsPropertyChanged)));
  public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register(nameof (Document), typeof (PdfDocument), typeof (CommetControl), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty SelectedAnnotationProperty = DependencyProperty.Register(nameof (SelectedAnnotation), typeof (PdfAnnotation), typeof (CommetControl), new PropertyMetadata((object) null, new PropertyChangedCallback(CommetControl.OnSelectedAnnotationPropertyChanged)));

  static CommetControl()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (CommetControl), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (CommetControl)));
  }

  public CommetControl()
  {
    this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.CommetControl_IsVisibleChanged);
  }

  private CommetTreeView CommetTreeView
  {
    get => this.commetTreeView;
    set
    {
      if (this.commetTreeView == value)
        return;
      if (this.commetTreeView != null)
      {
        this.commetTreeView.ItemsSource = (IEnumerable) null;
        this.commetTreeView.SelectedItemChanged -= new RoutedPropertyChangedEventHandler<object>(this.CommetTreeView_SelectedItemChanged);
      }
      this.commetTreeView = value;
      if (this.commetTreeView == null)
        return;
      this.commetTreeView.ItemsSource = (IEnumerable) this.AllPageCommets;
      this.commetTreeView.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(this.CommetTreeView_SelectedItemChanged);
    }
  }

  private void CommetTreeView_SelectedItemChanged(
    object sender,
    RoutedPropertyChangedEventArgs<object> e)
  {
    if (this.innerSet || this.Document == null || !(e.NewValue is CommetModel newValue))
      return;
    PdfPage page = this.Document.Pages[newValue.Annotation.PageIndex];
    if (page.Annots == null)
      page.CreateAnnotations();
    if (newValue.Annotation.AnnotIndex >= page.Annots.Count)
      return;
    PdfAnnotation selectAnnot = page.Annots[newValue.Annotation.AnnotIndex];
    PdfMarkupAnnotation markup = selectAnnot as PdfMarkupAnnotation;
    if (markup != null)
    {
      if (markup.Relationship == RelationTypes.Reply)
        return;
      PDFKit.PdfControl viewer = PDFKit.PdfControl.GetPdfControl(this.Document);
      if (viewer != null)
      {
        if (viewer.DataContext is MainViewModel dataContext && !dataContext.IsAnnotationVisible)
          dataContext.IsAnnotationVisible = true;
        FS_RECTF rect = markup.GetRECT();
        bool flag = false;
        if (viewer.PageIndex != page.PageIndex)
        {
          flag = true;
          viewer.ScrollToPage(page.PageIndex);
        }
        (float width, float height) = page.GetEffectiveSize();
        if ((double) rect.left < (double) width && (double) rect.right > 0.0 && (double) rect.top > 0.0 && (double) rect.bottom < (double) height)
        {
          if (flag)
            this.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) (() => this.ScrollToAnnotation(viewer?.Viewer, markup)));
          else
            this.ScrollToAnnotation(viewer?.Viewer, markup);
        }
      }
    }
    this.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) (() =>
    {
      try
      {
        this.SelectedAnnotation = selectAnnot;
      }
      catch
      {
      }
    }));
  }

  private void ScrollToAnnotation(PdfViewer viewer, PdfMarkupAnnotation markup)
  {
    try
    {
      Rect deviceBounds = markup.GetDeviceBounds();
      if (deviceBounds.Left > viewer.ActualWidth || deviceBounds.Right < 0.0)
      {
        if (deviceBounds.Left > viewer.ActualWidth)
          viewer.ScrollOwner.ScrollToHorizontalOffset(viewer.ScrollOwner.HorizontalOffset + (deviceBounds.Right - viewer.ActualWidth));
        else
          viewer.ScrollOwner.ScrollToHorizontalOffset(viewer.ScrollOwner.HorizontalOffset + deviceBounds.Left);
      }
      if (deviceBounds.Top <= viewer.ActualHeight && deviceBounds.Bottom >= 0.0)
        return;
      if (deviceBounds.Top > viewer.ActualHeight)
        viewer.ScrollOwner.ScrollToVerticalOffset(viewer.ScrollOwner.VerticalOffset + (deviceBounds.Bottom - viewer.ActualHeight));
      else
        viewer.ScrollOwner.ScrollToVerticalOffset(viewer.ScrollOwner.VerticalOffset + deviceBounds.Top);
    }
    catch
    {
      PdfPage page = markup.Page;
      if (viewer.CurrentIndex == page.PageIndex)
        return;
      viewer.ScrollToPage(page.PageIndex);
    }
  }

  public void ExpandAll()
  {
    if (this.AllPageCommets == null)
      return;
    PdfViewer pdfViewer = (PdfViewer) null;
    if (this.IsVisible && this.Document != null)
      pdfViewer = PDFKit.PdfControl.GetPdfControl(this.Document)?.Viewer;
    int num = pdfViewer != null ? pdfViewer.CurrentIndex : -1;
    PageCommetCollection commetCollection = (PageCommetCollection) null;
    bool flag = false;
    foreach (PageCommetCollection allPageCommet in (Collection<PageCommetCollection>) this.AllPageCommets)
    {
      allPageCommet.IsExpanded = true;
      if (!flag)
      {
        if (allPageCommet.PageIndex <= num)
          commetCollection = allPageCommet;
        else
          flag = true;
      }
    }
    if (this.CommetTreeView.SelectedItem is CommetModel selectedItem)
    {
      this.CommetTreeView.ScrollIntoViewAsync((ITreeViewNode) selectedItem);
    }
    else
    {
      if (commetCollection == null || this.CommetTreeView == null || !this.IsVisible)
        return;
      this.CommetTreeView.ScrollIntoViewAsync((ITreeViewNode) commetCollection);
    }
  }

  public void CollapseAll()
  {
    this.SelectedAnnotation = (PdfAnnotation) null;
    if (this.CommetTreeView != null)
    {
      foreach (object obj in (IEnumerable) this.CommetTreeView.Items)
      {
        if (this.CommetTreeView.ItemContainerGenerator?.ContainerFromItem(obj) is TreeViewItem treeViewItem)
          treeViewItem.IsExpanded = false;
      }
    }
    if (this.AllPageCommets == null)
      return;
    PdfViewer pdfViewer = (PdfViewer) null;
    if (this.IsVisible && this.Document != null)
      pdfViewer = PDFKit.PdfControl.GetPdfControl(this.Document)?.Viewer;
    int num = pdfViewer != null ? pdfViewer.CurrentIndex : -1;
    PageCommetCollection commetCollection = (PageCommetCollection) null;
    bool flag = false;
    foreach (PageCommetCollection allPageCommet in (Collection<PageCommetCollection>) this.AllPageCommets)
    {
      allPageCommet.IsExpanded = false;
      if (!flag)
      {
        if (allPageCommet.PageIndex <= num)
          commetCollection = allPageCommet;
        else
          flag = true;
      }
    }
    if (commetCollection == null || this.CommetTreeView == null || !this.IsVisible)
      return;
    ScrollIntoViewAsync((TreeView) this.CommetTreeView, commetCollection);

    static async void ScrollIntoViewAsync(TreeView view, PageCommetCollection item)
    {
      await view.ScrollIntoViewAsync((ITreeViewNode) item);
      item.IsExpanded = false;
    }
  }

  private void CommetControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
  {
    if (!this.IsVisible)
      return;
    this.SyncSelectedAnnotation();
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.CommetTreeView = this.GetTemplateChild("PART_CommetTreeView") as CommetTreeView;
  }

  public AllPageCommetCollectionView AllPageCommets
  {
    get => (AllPageCommetCollectionView) this.GetValue(CommetControl.AllPageCommetsProperty);
    set => this.SetValue(CommetControl.AllPageCommetsProperty, (object) value);
  }

  private static void OnAllPageCommetsPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (e.NewValue == e.OldValue || !(d is CommetControl commetControl) || commetControl.CommetTreeView == null)
      return;
    commetControl.CommetTreeView.ItemsSource = e.NewValue as IEnumerable;
  }

  public PdfDocument Document
  {
    get => (PdfDocument) this.GetValue(CommetControl.DocumentProperty);
    set => this.SetValue(CommetControl.DocumentProperty, (object) value);
  }

  public PdfAnnotation SelectedAnnotation
  {
    get => (PdfAnnotation) this.GetValue(CommetControl.SelectedAnnotationProperty);
    set => this.SetValue(CommetControl.SelectedAnnotationProperty, (object) value);
  }

  private static void OnSelectedAnnotationPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (e.NewValue == e.OldValue || !(d is CommetControl commetControl))
      return;
    commetControl.SyncSelectedAnnotation();
  }

  private void SyncSelectedAnnotation()
  {
    if (this.AllPageCommets == null)
      return;
    this.innerSet = true;
    try
    {
      bool flag = false;
      PdfAnnotation selectedAnnotation = this.SelectedAnnotation;
      if (selectedAnnotation != null)
      {
        foreach (PageCommetCollection allPageCommet in (Collection<PageCommetCollection>) this.AllPageCommets)
        {
          if (allPageCommet.PageIndex == selectedAnnotation.Page.PageIndex)
          {
            int idx = selectedAnnotation.Page.Annots.IndexOf(selectedAnnotation);
            CommetModel model = allPageCommet.FirstOrDefault<CommetModel>((Func<CommetModel, bool>) (c => c.Annotation.AnnotIndex == idx));
            if (model != null)
            {
              model.IsSelected = true;
              flag = true;
              if (this.CommetTreeView != null)
              {
                this.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) (async () =>
                {
                  CommetTreeView commetTreeView = this.CommetTreeView;
                  if ((commetTreeView != null ? (commetTreeView.IsLoaded ? 1 : 0) : 0) == 0)
                    return;
                  await this.CommetTreeView.ScrollIntoViewAsync((ITreeViewNode) model);
                }));
                break;
              }
              break;
            }
          }
        }
      }
      if (flag || !(this.CommetTreeView?.SelectedItem is CommetModel selectedItem))
        return;
      selectedItem.IsSelected = false;
    }
    finally
    {
      this.innerSet = false;
    }
  }
}
