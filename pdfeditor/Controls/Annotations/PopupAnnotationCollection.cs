// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Annotations.PopupAnnotationCollection
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.Wrappers;
using pdfeditor.Utils;
using PDFKit;
using PDFKit.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace pdfeditor.Controls.Annotations;

public class PopupAnnotationCollection : Canvas
{
  private ConcurrentDictionary<PdfPopupAnnotation, PopupAnnotationCollection.PopupAnnotationContext> dict;
  private readonly AnnotationCanvas annotationCanvas;
  private readonly PdfPage page;
  private IReadOnlyDictionary<PdfAnnotation, System.Collections.Generic.IReadOnlyList<PdfMarkupAnnotation>> annotationRepliesDict;

  private PdfViewer PdfViewer => this.annotationCanvas.PdfViewer;

  public PopupAnnotationCollection(AnnotationCanvas annotationCanvas, PdfPage page)
  {
    this.page = page ?? throw new ArgumentNullException(nameof (page));
    this.annotationCanvas = annotationCanvas ?? throw new ArgumentNullException(nameof (annotationCanvas));
    this.dict = new ConcurrentDictionary<PdfPopupAnnotation, PopupAnnotationCollection.PopupAnnotationContext>((IEqualityComparer<PdfPopupAnnotation>) new PopupAnnotationCollection.PdfWrapperCompare());
    this.Loaded += new RoutedEventHandler(this.PopupAnnotationCollection_Loaded);
    this.Unloaded += new RoutedEventHandler(this.PopupAnnotationCollection_Unloaded);
  }

  private void PopupAnnotationCollection_Loaded(object sender, RoutedEventArgs e)
  {
    this.InitPopupHost();
  }

  private void PopupAnnotationCollection_Unloaded(object sender, RoutedEventArgs e)
  {
    this.dict.Clear();
    this.InternalChildren.Clear();
    this.annotationRepliesDict = (IReadOnlyDictionary<PdfAnnotation, System.Collections.Generic.IReadOnlyList<PdfMarkupAnnotation>>) null;
  }

  private void InitPopupHost()
  {
    this.dict.Clear();
    this.InternalChildren.Clear();
    if (this.page.Annots == null)
      return;
    this.annotationRepliesDict = CommetUtils.GetMarkupAnnotationReplies(this.page);
    PdfTextAnnotation selectedAnnotation = this.annotationCanvas?.SelectedAnnotation as PdfTextAnnotation;
    foreach (PdfAnnotation annot in this.page.Annots)
    {
      PdfAnnotation pdfAnnotation = (PdfAnnotation) null;
      if (annot is PdfPopupAnnotation popup)
      {
        try
        {
          pdfAnnotation = popup.Parent;
        }
        catch
        {
        }
      }
      else
        popup = (PdfPopupAnnotation) null;
      if ((PdfWrapper) popup != (PdfWrapper) null && (PdfWrapper) pdfAnnotation != (PdfWrapper) null && (!(popup.Parent is PdfMarkupAnnotation parent) || parent.Relationship == RelationTypes.NonSpecified))
      {
        this.UpdatePosition(this.AddAnnotationCore(popup));
        if ((PdfWrapper) selectedAnnotation == (PdfWrapper) popup.Parent)
          this.SetSelected((PdfAnnotation) popup, true);
      }
    }
    this.UpdateChildrenZIndex();
  }

  private PopupAnnotationCollection.PopupAnnotationContext AddAnnotationCore(
    PdfPopupAnnotation popup)
  {
    if (popup == null)
      throw new ArgumentNullException(nameof (popup));
    if (this.dict.ContainsKey(popup))
      return (PopupAnnotationCollection.PopupAnnotationContext) null;
    System.Collections.Generic.IReadOnlyList<PdfMarkupAnnotation> replies;
    if (this.annotationRepliesDict == null || (PdfWrapper) popup.Parent == (PdfWrapper) null || !this.annotationRepliesDict.TryGetValue(popup.Parent, out replies))
      replies = (System.Collections.Generic.IReadOnlyList<PdfMarkupAnnotation>) null;
    PopupAnnotationCollection.PopupAnnotationContext annotationContext = new PopupAnnotationCollection.PopupAnnotationContext(this.annotationCanvas, popup, replies);
    this.dict[popup] = annotationContext;
    this.InternalChildren.Add((UIElement) annotationContext.AnnotationPopupControl);
    this.InternalChildren.Add((UIElement) annotationContext.RelationshipLine);
    return annotationContext;
  }

  private void RemoveAnnotationCore(PdfPopupAnnotation popup)
  {
    if (popup == null)
      throw new ArgumentNullException(nameof (popup));
    PopupAnnotationCollection.PopupAnnotationContext annotationContext;
    if (!this.dict.TryRemove(popup, out annotationContext))
      return;
    this.InternalChildren.Remove((UIElement) annotationContext.AnnotationPopupControl);
    this.InternalChildren.Remove((UIElement) annotationContext.RelationshipLine);
  }

  private void UpdateChildrenZIndex()
  {
    if (this.InternalChildren.Count == 0)
      return;
    PdfAnnotationCollection annots = this.page.Annots;
    // ISSUE: explicit non-virtual call
    int num = annots != null ? __nonvirtual (annots.Count) : this.dict.Count * 2;
    foreach (KeyValuePair<PdfPopupAnnotation, PopupAnnotationCollection.PopupAnnotationContext> pair in this.dict)
    {
      PopupAnnotationCollection.PopupAnnotationContext annotationContext1;
      pair.Deconstruct<PdfPopupAnnotation, PopupAnnotationCollection.PopupAnnotationContext>(out PdfPopupAnnotation _, out annotationContext1);
      PopupAnnotationCollection.PopupAnnotationContext annotationContext2 = annotationContext1;
      int annotationIndex = annotationContext2.PopupAnnotationWrapper.AnnotationIndex;
      if (annotationContext2.IsParentSelected)
        annotationIndex += num;
      Panel.SetZIndex((UIElement) annotationContext2.AnnotationPopupControl, annotationIndex * 2);
      Panel.SetZIndex((UIElement) annotationContext2.RelationshipLine, annotationIndex * 2 - 1);
    }
  }

  private void UpdatePosition(
    PopupAnnotationCollection.PopupAnnotationContext context)
  {
    if (context == null)
      return;
    DpiScale dpi = VisualTreeHelper.GetDpi((Visual) this);
    double num1 = (double) context.PopupAnnotationWrapper.Rectangle.Width * dpi.PixelsPerDip;
    double num2 = (double) context.PopupAnnotationWrapper.Rectangle.Height * dpi.PixelsPerDip;
    Point pagePoint = new Point((double) context.PopupAnnotationWrapper.Rectangle.left, (double) context.PopupAnnotationWrapper.Rectangle.top);
    Point clientPoint;
    if (this.PdfViewer.TryGetClientPoint(context.PopupAnnotationWrapper.Page.PageIndex, pagePoint, out clientPoint))
    {
      Canvas.SetLeft((UIElement) context.AnnotationPopupControl, clientPoint.X);
      Canvas.SetTop((UIElement) context.AnnotationPopupControl, clientPoint.Y);
    }
    context.AnnotationPopupControl.Width = num1;
    context.AnnotationPopupControl.Height = num2;
    context.PopupAnnotationWrapper.NotifyAnnotationChanged();
    this.UpdateLine(context);
  }

  private void UpdateLine(
    PopupAnnotationCollection.PopupAnnotationContext context)
  {
    if (this.annotationCanvas.PdfViewer == null)
      return;
    PdfAnnotation parent = context.PopupAnnotationWrapper.Annotation.Parent;
    if ((PdfWrapper) parent == (PdfWrapper) null)
      return;
    Rect deviceBounds = parent.GetDeviceBounds();
    Point point1 = new Point(deviceBounds.Left + deviceBounds.Width / 2.0, deviceBounds.Top + deviceBounds.Height / 2.0);
    Point point2 = new Point(Canvas.GetLeft((UIElement) context.AnnotationPopupControl), Canvas.GetTop((UIElement) context.AnnotationPopupControl));
    context.RelationshipLine.X1 = point1.X;
    context.RelationshipLine.Y1 = point1.Y;
    context.RelationshipLine.X2 = point2.X;
    context.RelationshipLine.Y2 = point2.Y;
    if (!(context.RelationshipLine.Stroke is SolidColorBrush stroke))
      return;
    stroke.Color = context.PopupAnnotationWrapper.BackgroundColor;
  }

  public void AddAnnotation(PdfPopupAnnotation popup)
  {
    this.AddAnnotationCore(popup);
    this.UpdateChildrenZIndex();
    if (!(this.annotationCanvas?.SelectedAnnotation is PdfTextAnnotation selectedAnnotation) || !((PdfWrapper) selectedAnnotation == (PdfWrapper) popup.Parent))
      return;
    this.SetSelected((PdfAnnotation) popup, true);
  }

  public void RemoveAnnotation(PdfPopupAnnotation popup)
  {
    this.RemoveAnnotationCore(popup);
    this.UpdateChildrenZIndex();
  }

  private PdfPopupAnnotation GetPopupAnnotation(PdfAnnotation annotation)
  {
    switch (annotation)
    {
      case PdfPopupAnnotation key:
        int? pageIndex1 = key.Page?.PageIndex;
        int pageIndex2 = this.page.PageIndex;
        if (!(pageIndex1.GetValueOrDefault() == pageIndex2 & pageIndex1.HasValue))
          throw new ArgumentException("Page");
        return !this.dict.TryGetValue(key, out PopupAnnotationCollection.PopupAnnotationContext _) ? (PdfPopupAnnotation) null : key;
      case PdfMarkupAnnotation markupAnnotation:
        if ((PdfWrapper) markupAnnotation.Popup != (PdfWrapper) null)
          return this.GetPopupAnnotation((PdfAnnotation) markupAnnotation.Popup);
        break;
    }
    return (PdfPopupAnnotation) null;
  }

  public bool IsPopupVisible(PdfAnnotation annotation)
  {
    PdfPopupAnnotation popupAnnotation = this.GetPopupAnnotation(annotation);
    PopupAnnotationCollection.PopupAnnotationContext annotationContext;
    return (PdfWrapper) popupAnnotation != (PdfWrapper) null && this.dict.TryGetValue(popupAnnotation, out annotationContext) && annotationContext.PopupAnnotationWrapper.IsOpen;
  }

  public bool ShowPopup(PdfAnnotation annotation, bool bringIntoView)
  {
    PdfPopupAnnotation popupAnnotation = this.GetPopupAnnotation(annotation);
    PopupAnnotationCollection.PopupAnnotationContext context;
    if (!((PdfWrapper) popupAnnotation != (PdfWrapper) null) || !this.dict.TryGetValue(popupAnnotation, out context))
      return false;
    this.UpdatePosition(context);
    object dataContext = this.annotationCanvas.DataContext;
    context.PopupAnnotationWrapper.IsOpen = true;
    context.UpdateLineVisible();
    if (bringIntoView)
      this.TryBringPopupControlIntoView((PdfAnnotation) popupAnnotation);
    return true;
  }

  public void HidePopup(PdfAnnotation annotation)
  {
    PdfPopupAnnotation popupAnnotation = this.GetPopupAnnotation(annotation);
    PopupAnnotationCollection.PopupAnnotationContext annotationContext;
    if (!((PdfWrapper) popupAnnotation != (PdfWrapper) null) || !this.dict.TryGetValue(popupAnnotation, out annotationContext))
      return;
    object dataContext = this.annotationCanvas.DataContext;
    annotationContext.PopupAnnotationWrapper.IsOpen = false;
    annotationContext.UpdateLineVisible();
  }

  public void UpdatePosition()
  {
    foreach (KeyValuePair<PdfPopupAnnotation, PopupAnnotationCollection.PopupAnnotationContext> pair in this.dict)
    {
      PopupAnnotationCollection.PopupAnnotationContext annotationContext;
      pair.Deconstruct<PdfPopupAnnotation, PopupAnnotationCollection.PopupAnnotationContext>(out PdfPopupAnnotation _, out annotationContext);
      PopupAnnotationCollection.PopupAnnotationContext context = annotationContext;
      if (context.PopupAnnotationWrapper.IsOpen)
        this.UpdatePosition(context);
    }
  }

  public void SetHovered(PdfAnnotation pdfAnnotation, bool value)
  {
    PdfPopupAnnotation popupAnnotation = this.GetPopupAnnotation(pdfAnnotation);
    PopupAnnotationCollection.PopupAnnotationContext context;
    if (!((PdfWrapper) popupAnnotation != (PdfWrapper) null) || !this.dict.TryGetValue(popupAnnotation, out context))
      return;
    if (value)
      this.UpdatePosition(context);
    if ((PdfWrapper) pdfAnnotation == (PdfWrapper) popupAnnotation)
    {
      context.IsParentHovered = value;
    }
    else
    {
      if (!((PdfWrapper) pdfAnnotation == (PdfWrapper) popupAnnotation.Parent))
        return;
      context.IsPopupHovered = value;
    }
  }

  public void SetSelected(PdfAnnotation pdfAnnotation, bool value)
  {
    PdfPopupAnnotation popupAnnotation = this.GetPopupAnnotation(pdfAnnotation);
    PopupAnnotationCollection.PopupAnnotationContext context;
    if (!((PdfWrapper) popupAnnotation != (PdfWrapper) null) || !this.dict.TryGetValue(popupAnnotation, out context))
      return;
    if (value)
      this.UpdatePosition(context);
    context.IsParentSelected = value;
    this.UpdateChildrenZIndex();
  }

  public TextBox GetPopupTextBox(PdfAnnotation pdfAnnotation)
  {
    PdfPopupAnnotation popupAnnotation = this.GetPopupAnnotation(pdfAnnotation);
    PopupAnnotationCollection.PopupAnnotationContext annotationContext;
    return (PdfWrapper) popupAnnotation != (PdfWrapper) null && this.dict.TryGetValue(popupAnnotation, out annotationContext) && annotationContext.AnnotationPopupControl?.Content is FrameworkElement content && content.FindName("TextContentBox") is TextBox name ? name : (TextBox) null;
  }

  public void TryBringPopupControlIntoView(PdfAnnotation pdfAnnotation)
  {
    if (this.annotationCanvas.ActualWidth == 0.0 || this.annotationCanvas.ActualHeight == 0.0)
      return;
    ScrollViewer scrollOwner = this.annotationCanvas.PdfViewer.ScrollOwner;
    if (scrollOwner == null)
      return;
    PdfPopupAnnotation popupAnnotation = this.GetPopupAnnotation(pdfAnnotation);
    PopupAnnotationCollection.PopupAnnotationContext annotationContext;
    if (!((PdfWrapper) popupAnnotation != (PdfWrapper) null) || !this.dict.TryGetValue(popupAnnotation, out annotationContext))
      return;
    AnnotationPopupControl annotationPopupControl = annotationContext.AnnotationPopupControl;
    if (annotationPopupControl == null || annotationPopupControl.ActualWidth == 0.0 && annotationPopupControl.ActualHeight == 0.0)
      return;
    Rect rect = annotationPopupControl.TransformToVisual((Visual) this.annotationCanvas).TransformBounds(new Rect(0.0, 0.0, annotationPopupControl.ActualWidth + 8.0, annotationPopupControl.ActualHeight));
    if (rect.Left < 0.0 || rect.Right > this.annotationCanvas.ActualWidth)
    {
      double num = rect.Left >= 0.0 ? rect.Right - this.annotationCanvas.ActualWidth : rect.Left;
      double offset = scrollOwner.HorizontalOffset + num;
      scrollOwner.ScrollToHorizontalOffset(offset);
    }
    if (rect.Top >= 0.0 && rect.Bottom <= this.annotationCanvas.ActualHeight)
      return;
    double num1 = rect.Top >= 0.0 ? rect.Bottom - this.annotationCanvas.ActualHeight : rect.Top;
    double offset1 = scrollOwner.VerticalOffset + num1;
    scrollOwner.ScrollToVerticalOffset(offset1);
  }

  public void KillFocus()
  {
    foreach (PopupAnnotationCollection.PopupAnnotationContext annotationContext in (IEnumerable<PopupAnnotationCollection.PopupAnnotationContext>) this.dict.Values)
    {
      if (annotationContext.AnnotationPopupControl.IsKeyboardFocusWithin)
        annotationContext.AnnotationPopupControl.Apply();
    }
  }

  private class PdfWrapperCompare : IEqualityComparer<PdfWrapper>
  {
    public bool Equals(PdfWrapper x, PdfWrapper y)
    {
      if (x == y)
        return true;
      return (object) x != null && x.Equals(y);
    }

    public int GetHashCode(PdfWrapper obj)
    {
      return obj == (PdfWrapper) null ? int.MinValue : (int) (long) obj.Dictionary.Handle;
    }
  }

  private class PopupAnnotationContext
  {
    private bool isParentSelected;
    private bool isParentHovered;
    private bool isPopupHovered;

    public PopupAnnotationContext(
      AnnotationCanvas annotationCanvas,
      PdfPopupAnnotation annotation,
      System.Collections.Generic.IReadOnlyList<PdfMarkupAnnotation> replies)
    {
      PopupAnnotationWrapper wrapper = new PopupAnnotationWrapper(annotation);
      if (replies != null && replies.Count > 0)
        wrapper.Replies = new ObservableCollection<PopupAnnotationReplyWrapper>(replies.Select<PdfMarkupAnnotation, PopupAnnotationReplyWrapper>((Func<PdfMarkupAnnotation, PopupAnnotationReplyWrapper>) (c => new PopupAnnotationReplyWrapper(c))));
      AnnotationPopupControl annotationPopupControl = new AnnotationPopupControl(annotationCanvas, wrapper);
      Line line1 = new Line();
      SolidColorBrush solidColorBrush = new SolidColorBrush(wrapper.BackgroundColor);
      solidColorBrush.Opacity = 0.5;
      line1.Stroke = (Brush) solidColorBrush;
      line1.StrokeThickness = 1.0;
      line1.IsHitTestVisible = false;
      line1.Opacity = 0.0;
      Line line2 = line1;
      this.PopupAnnotationWrapper = wrapper;
      this.AnnotationPopupControl = annotationPopupControl;
      this.RelationshipLine = line2;
    }

    public PopupAnnotationWrapper PopupAnnotationWrapper { get; }

    public AnnotationPopupControl AnnotationPopupControl { get; }

    public Line RelationshipLine { get; }

    public bool IsParentSelected
    {
      get => this.isParentSelected;
      set
      {
        this.isParentSelected = value;
        this.isParentHovered = false;
        this.UpdateLineVisible();
      }
    }

    public bool IsParentHovered
    {
      get => this.isParentHovered;
      set
      {
        this.isParentHovered = value;
        this.UpdateLineVisible();
      }
    }

    public bool IsPopupHovered
    {
      get => this.isPopupHovered;
      set
      {
        this.isPopupHovered = value;
        this.UpdateLineVisible();
      }
    }

    public void UpdateLineVisible()
    {
      if (this.PopupAnnotationWrapper.IsOpen && (this.IsParentSelected || this.IsParentHovered || this.IsPopupHovered))
        this.RelationshipLine.Opacity = 1.0;
      else
        this.RelationshipLine.Opacity = 0.0;
    }
  }
}
