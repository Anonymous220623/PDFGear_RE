// Decompiled with JetBrains decompiler
// Type: PDFKit.PdfControl
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Net;
using PDFKit.Contents.Controls;
using PDFKit.Utils;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

#nullable disable
namespace PDFKit;

public partial class PdfControl : UserControl, IPdfScrollInfoInternal, IComponentConnector
{
  private static List<PdfControl.PdfControlCache> docCache;
  private PdfControlPageIndexBinding viewerPageIdxBinding;
  private PdfControlPageIndexBinding editorPageIdxBinding;
  private int[] lastChangedPages;
  public static readonly DependencyProperty ViewerProperty;
  private static readonly DependencyPropertyKey ViewerPropertyKey;
  public static readonly DependencyProperty EditorProperty;
  private static readonly DependencyPropertyKey EditorPropertyKey;
  public static readonly DependencyProperty CanEditorRedoProperty;
  private static readonly DependencyPropertyKey CanEditorRedoPropertyKey;
  public static readonly DependencyProperty CanEditorUndoProperty;
  private static readonly DependencyPropertyKey CanEditorUndoPropertyKey;
  public static readonly DependencyProperty IsEditingProperty = DependencyProperty.Register(nameof (IsEditing), typeof (bool), typeof (PdfControl), new PropertyMetadata((object) false, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is PdfControl pdfControl2) || object.Equals(a.NewValue, a.OldValue))
      return;
    pdfControl2.UpdateEditMode();
  })));
  public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register(nameof (Document), typeof (PdfDocument), typeof (PdfControl), new PropertyMetadata((object) null, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is PdfControl pdfControl4) || a.NewValue == a.OldValue)
      return;
    lock (PdfControl.docCache)
    {
      if (a.OldValue is PdfDocument oldValue2)
      {
        for (int index = 0; index < PdfControl.docCache.Count; ++index)
        {
          if (PdfControl.docCache[index].Document == oldValue2)
          {
            PdfControl.docCache.RemoveAt(index);
            break;
          }
        }
      }
      if (a.NewValue is PdfDocument newValue2)
        PdfControl.docCache.Add(new PdfControl.PdfControlCache(pdfControl4, newValue2));
      PdfControl.ClearInvalidItems();
    }
    pdfControl4.UpdateDocument();
  })));
  private static readonly DependencyPropertyKey ActualEditingPropertyKey;
  public static readonly DependencyProperty ActualEditingProperty;
  public static readonly DependencyProperty IsAnnotationVisibleProperty = DependencyProperty.Register(nameof (IsAnnotationVisible), typeof (bool), typeof (PdfControl), new PropertyMetadata((object) true, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is PdfControl pdfControl6) || object.Equals(a.NewValue, a.OldValue) || !(a.NewValue is bool newValue4))
      return;
    pdfControl6._Viewer.IsAnnotationVisible = newValue4;
    pdfControl6._Editor.IsAnnotationVisible = newValue4;
  })));
  public static readonly DependencyProperty IsRenderPausedProperty = DependencyProperty.Register(nameof (IsRenderPaused), typeof (bool), typeof (PdfControl), new PropertyMetadata((object) false, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is PdfControl pdfControl8) || object.Equals(a.NewValue, a.OldValue) || !(a.NewValue is bool newValue6))
      return;
    pdfControl8._Viewer.IsRenderPaused = newValue6;
    pdfControl8._Editor.IsRenderPaused = newValue6;
  })));
  public static readonly DependencyProperty PageBackgroundProperty = DependencyProperty.Register(nameof (PageBackground), typeof (Brush), typeof (PdfControl), new PropertyMetadata((object) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 201, (byte) 201, (byte) 201)), (PropertyChangedCallback) ((s, a) =>
  {
    Brush newValue7;
    int num;
    if (s is PdfControl pdfControl10 && !object.Equals(a.NewValue, a.OldValue))
    {
      newValue7 = a.NewValue as Brush;
      num = newValue7 != null ? 1 : 0;
    }
    else
      num = 0;
    if (num == 0)
      return;
    pdfControl10._Viewer.Background = newValue7;
    pdfControl10._Editor.Background = newValue7;
  })));
  public static readonly DependencyProperty PageMaskBrushProperty = DependencyProperty.Register(nameof (PageMaskBrush), typeof (Brush), typeof (PdfControl), new PropertyMetadata((object) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 201, (byte) 201, (byte) 201)), (PropertyChangedCallback) ((s, a) =>
  {
    Brush newValue8;
    int num;
    if (s is PdfControl pdfControl12 && !object.Equals(a.NewValue, a.OldValue))
    {
      newValue8 = a.NewValue as Brush;
      num = newValue8 != null ? 1 : 0;
    }
    else
      num = 0;
    if (num == 0)
      return;
    pdfControl12._Viewer.PageMaskBrush = newValue8;
    pdfControl12._Editor.PageMaskBrush = newValue8;
  })));
  public static readonly DependencyProperty CurrentPageHighlightColorProperty = DependencyProperty.Register(nameof (CurrentPageHighlightColor), typeof (Color), typeof (PdfControl), new PropertyMetadata((object) Color.FromArgb((byte) 102, (byte) 0, (byte) 0, (byte) 0), (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is PdfControl pdfControl14) || object.Equals(a.NewValue, a.OldValue) || !(a.NewValue is Color newValue10))
      return;
    pdfControl14._Viewer.CurrentPageHighlightColor = newValue10;
    pdfControl14._Editor.CurrentPageHighlightColor = newValue10;
  })));
  public static readonly DependencyProperty PagePaddingProperty = DependencyProperty.Register(nameof (PagePadding), typeof (Thickness), typeof (PdfControl), new PropertyMetadata((object) new Thickness(10.0), (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is PdfControl pdfControl16) || object.Equals(a.NewValue, a.OldValue) || !(a.NewValue is Thickness newValue12))
      return;
    pdfControl16._Viewer.Padding = newValue12;
    pdfControl16._Editor.Padding = newValue12;
  })));
  public static readonly DependencyProperty ViewerMouseModeProperty = DependencyProperty.Register(nameof (ViewerMouseMode), typeof (MouseModes), typeof (PdfControl), new PropertyMetadata((object) MouseModes.Default, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is PdfControl pdfControl18) || object.Equals(a.NewValue, a.OldValue) || !(a.NewValue is MouseModes newValue14))
      return;
    pdfControl18._Viewer.MouseMode = newValue14;
  })));
  public static readonly DependencyProperty EditorMouseModeProperty = DependencyProperty.Register(nameof (EditorMouseMode), typeof (EditorMouseModes), typeof (PdfControl), new PropertyMetadata((object) EditorMouseModes.SelectParagraph, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is PdfControl pdfControl20) || object.Equals(a.NewValue, a.OldValue) || !(a.NewValue is EditorMouseModes newValue16))
      return;
    pdfControl20._Editor.MouseMode = newValue16;
  })));
  public static readonly DependencyProperty PageIndexProperty = DependencyProperty.Register(nameof (PageIndex), typeof (int), typeof (PdfControl), new PropertyMetadata((object) -1, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is PdfControl pdfControl22) || object.Equals(a.NewValue, a.OldValue))
      return;
    if (pdfControl22.IsEditing)
    {
      if (pdfControl22.editorPageIdxBinding != null)
        pdfControl22.editorPageIdxBinding.PageIndex = (int) a.NewValue;
    }
    else if (pdfControl22.viewerPageIdxBinding != null)
      pdfControl22.viewerPageIdxBinding.PageIndex = (int) a.NewValue;
  })));
  public static readonly DependencyProperty ViewModeProperty = DependencyProperty.Register(nameof (ViewMode), typeof (ViewModes), typeof (PdfControl), new PropertyMetadata((object) ViewModes.Vertical, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is PdfControl pdfControl24) || object.Equals(a.NewValue, a.OldValue) || !(a.NewValue is ViewModes newValue18))
      return;
    pdfControl24._Viewer.ViewMode = newValue18;
    pdfControl24._Editor.ViewMode = newValue18;
    pdfControl24.Zoom = pdfControl24.GetCurrentPdfScrollInfo().Zoom;
  })));
  public static readonly DependencyProperty SizeModeProperty = DependencyProperty.Register(nameof (SizeMode), typeof (SizeModes), typeof (PdfControl), new PropertyMetadata((object) SizeModes.Zoom, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is PdfControl pdfControl26) || object.Equals(a.NewValue, a.OldValue) || !(a.NewValue is SizeModes newValue20))
      return;
    pdfControl26._Viewer.SizeMode = newValue20;
    pdfControl26._Editor.SizeMode = newValue20;
    pdfControl26.Zoom = pdfControl26.GetCurrentPdfScrollInfo().Zoom;
  })));
  public static readonly DependencyProperty ZoomProperty = DependencyProperty.Register(nameof (Zoom), typeof (float), typeof (PdfControl), new PropertyMetadata((object) 1f, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is PdfControl pdfControl28) || object.Equals(a.NewValue, a.OldValue))
      return;
    pdfControl28._Viewer.Zoom = (float) a.NewValue;
    pdfControl28._Editor.Zoom = (float) a.NewValue;
  })));
  private static readonly DependencyProperty PdfControlProperty = DependencyProperty.RegisterAttached("Helper", typeof (PdfControl), typeof (PdfControl), new PropertyMetadata((PropertyChangedCallback) null));
  internal PdfViewerScrollViewer _ViewerScroll;
  internal PdfViewer _Viewer;
  internal PdfViewerScrollViewer _EditorScroll;
  internal PdfEditor _Editor;
  private bool _contentLoaded;

  static PdfControl()
  {
    PdfControl.ActualEditingPropertyKey = DependencyProperty.RegisterReadOnly(nameof (ActualEditing), typeof (bool), typeof (PdfControl), new PropertyMetadata((object) false));
    PdfControl.ActualEditingProperty = PdfControl.ActualEditingPropertyKey.DependencyProperty;
    PdfControl.ViewerPropertyKey = DependencyProperty.RegisterReadOnly(nameof (Viewer), typeof (PdfViewer), typeof (PdfControl), new PropertyMetadata((PropertyChangedCallback) null));
    PdfControl.ViewerProperty = PdfControl.ViewerPropertyKey.DependencyProperty;
    PdfControl.EditorPropertyKey = DependencyProperty.RegisterReadOnly(nameof (Editor), typeof (PdfEditor), typeof (PdfControl), new PropertyMetadata((PropertyChangedCallback) null));
    PdfControl.EditorProperty = PdfControl.EditorPropertyKey.DependencyProperty;
    PdfControl.CanEditorUndoPropertyKey = DependencyProperty.RegisterReadOnly(nameof (CanEditorUndo), typeof (bool), typeof (PdfControl), new PropertyMetadata((object) false));
    PdfControl.CanEditorUndoProperty = PdfControl.CanEditorUndoPropertyKey.DependencyProperty;
    PdfControl.CanEditorRedoPropertyKey = DependencyProperty.RegisterReadOnly(nameof (CanEditorRedo), typeof (bool), typeof (PdfControl), new PropertyMetadata((object) false));
    PdfControl.CanEditorRedoProperty = PdfControl.CanEditorRedoPropertyKey.DependencyProperty;
    PdfControl.docCache = new List<PdfControl.PdfControlCache>();
  }

  public PdfControl()
  {
    this.InitializeComponent();
    this.SetValue(PdfControl.PdfControlProperty, (object) this);
    this.SetValue(PdfControl.ViewerPropertyKey, (object) this._Viewer);
    this.SetValue(PdfControl.EditorPropertyKey, (object) this._Editor);
    this._Viewer.SetValue(PdfControl.PdfControlProperty, (object) this);
    this._Editor.SetValue(PdfControl.PdfControlProperty, (object) this);
    this._Viewer.IsAnnotationVisible = this.IsAnnotationVisible;
    this._Editor.IsAnnotationVisible = this.IsAnnotationVisible;
    this._Viewer.IsRenderPaused = this.IsRenderPaused;
    this._Editor.IsRenderPaused = this.IsRenderPaused;
    this._Viewer.Background = this.PageBackground;
    this._Editor.Background = this.PageBackground;
    this._Viewer.PageMaskBrush = this.PageMaskBrush;
    this._Editor.PageMaskBrush = this.PageMaskBrush;
    this._Viewer.CurrentPageHighlightColor = this.CurrentPageHighlightColor;
    this._Editor.CurrentPageHighlightColor = this.CurrentPageHighlightColor;
    this._Viewer.Padding = this.PagePadding;
    this._Editor.Padding = this.PagePadding;
    this._Viewer.ViewMode = this.ViewMode;
    this._Editor.ViewMode = this.ViewMode;
    this._Viewer.SizeMode = this.SizeMode;
    this._Editor.SizeMode = this.SizeMode;
    this._Viewer.Zoom = this.Zoom;
    this._Editor.Zoom = this.Zoom;
    this._Viewer.MouseMode = this.ViewerMouseMode;
    this._Editor.MouseMode = this.EditorMouseMode;
    this._Viewer.CurrentIndex = this.PageIndex;
    this._Viewer.ViewModeChanged += new EventHandler(this.InnerControl_ViewModeChanged);
    this._Editor.ViewModeChanged += new EventHandler(this.InnerControl_ViewModeChanged);
    this._Viewer.SizeModeChanged += new EventHandler(this.InnerControl_SizeModeChanged);
    this._Editor.SizeModeChanged += new EventHandler(this.InnerControl_SizeModeChanged);
    this._Viewer.ZoomChanged += new EventHandler(this.InnerControl_ZoomChanged);
    this._Editor.ZoomChanged += new EventHandler(this.InnerControl_ZoomChanged);
    this.viewerPageIdxBinding = (PdfControlPageIndexBinding) new PdfViewerPageIndexBinding(this._Viewer);
    this.editorPageIdxBinding = (PdfControlPageIndexBinding) new PdfEditorPageIndexBinding(this._Editor);
    this._Editor.operationManager.StateChanged += (EventHandler) ((s, a) =>
    {
      this.SetValue(PdfControl.CanEditorUndoPropertyKey, (object) this._Editor.operationManager.CanUndo);
      this.SetValue(PdfControl.CanEditorRedoPropertyKey, (object) this._Editor.operationManager.CanRedo);
      EventHandler undoStateChanged = this.EditorUndoStateChanged;
      if (undoStateChanged == null)
        return;
      undoStateChanged((object) this, EventArgs.Empty);
    });
    this.Loaded += new RoutedEventHandler(this.PdfControl_Loaded);
  }

  private void PdfControl_Loaded(object sender, RoutedEventArgs e) => this.UpdateEditMode();

  private void InnerControl_ZoomChanged(object sender, EventArgs e)
  {
    IPdfScrollInfoInternal currentPdfScrollInfo = this.GetCurrentPdfScrollInfo();
    if (sender != currentPdfScrollInfo)
      return;
    this.SetCurrentValue(PdfControl.ZoomProperty, (object) currentPdfScrollInfo.Zoom);
  }

  private void InnerControl_SizeModeChanged(object sender, EventArgs e)
  {
    IPdfScrollInfoInternal currentPdfScrollInfo = this.GetCurrentPdfScrollInfo();
    if (sender != currentPdfScrollInfo)
      return;
    switch (currentPdfScrollInfo)
    {
      case PdfViewer pdfViewer:
        this.SetCurrentValue(PdfControl.SizeModeProperty, (object) pdfViewer.SizeMode);
        break;
      case PdfEditor pdfEditor:
        this.SetCurrentValue(PdfControl.SizeModeProperty, (object) pdfEditor.SizeMode);
        break;
    }
  }

  private void InnerControl_ViewModeChanged(object sender, EventArgs e)
  {
    IPdfScrollInfoInternal currentPdfScrollInfo = this.GetCurrentPdfScrollInfo();
    if (sender != currentPdfScrollInfo)
      return;
    switch (currentPdfScrollInfo)
    {
      case PdfViewer pdfViewer:
        this.SetCurrentValue(PdfControl.ViewModeProperty, (object) pdfViewer.ViewMode);
        break;
      case PdfEditor pdfEditor:
        this.SetCurrentValue(PdfControl.ViewModeProperty, (object) pdfEditor.ViewMode);
        break;
    }
  }

  public PdfViewer Viewer => (PdfViewer) this.GetValue(PdfControl.ViewerProperty);

  public PdfEditor Editor => (PdfEditor) this.GetValue(PdfControl.EditorProperty);

  public bool CanEditorRedo => (bool) this.GetValue(PdfControl.CanEditorRedoProperty);

  public bool CanEditorUndo => (bool) this.GetValue(PdfControl.CanEditorUndoProperty);

  public bool IsEditing
  {
    get => (bool) this.GetValue(PdfControl.IsEditingProperty);
    set => this.SetValue(PdfControl.IsEditingProperty, (object) value);
  }

  public PdfDocument Document
  {
    get => (PdfDocument) this.GetValue(PdfControl.DocumentProperty);
    set => this.SetValue(PdfControl.DocumentProperty, (object) value);
  }

  public bool ActualEditing
  {
    get => (bool) this.GetValue(PdfControl.ActualEditingProperty);
    private set => this.SetValue(PdfControl.ActualEditingPropertyKey, (object) value);
  }

  public bool IsAnnotationVisible
  {
    get => (bool) this.GetValue(PdfControl.IsAnnotationVisibleProperty);
    set => this.SetValue(PdfControl.IsAnnotationVisibleProperty, (object) value);
  }

  public bool IsRenderPaused
  {
    get => (bool) this.GetValue(PdfControl.IsRenderPausedProperty);
    set => this.SetValue(PdfControl.IsRenderPausedProperty, (object) value);
  }

  public Brush PageBackground
  {
    get => (Brush) this.GetValue(PdfControl.PageBackgroundProperty);
    set => this.SetValue(PdfControl.PageBackgroundProperty, (object) value);
  }

  public Brush PageMaskBrush
  {
    get => (Brush) this.GetValue(PdfControl.PageMaskBrushProperty);
    set => this.SetValue(PdfControl.PageMaskBrushProperty, (object) value);
  }

  public Color CurrentPageHighlightColor
  {
    get => (Color) this.GetValue(PdfControl.CurrentPageHighlightColorProperty);
    set => this.SetValue(PdfControl.CurrentPageHighlightColorProperty, (object) value);
  }

  public Thickness PagePadding
  {
    get => (Thickness) this.GetValue(PdfControl.PagePaddingProperty);
    set => this.SetValue(PdfControl.PagePaddingProperty, (object) value);
  }

  public MouseModes ViewerMouseMode
  {
    get => (MouseModes) this.GetValue(PdfControl.ViewerMouseModeProperty);
    set => this.SetValue(PdfControl.ViewerMouseModeProperty, (object) value);
  }

  public EditorMouseModes EditorMouseMode
  {
    get => (EditorMouseModes) this.GetValue(PdfControl.EditorMouseModeProperty);
    set => this.SetValue(PdfControl.EditorMouseModeProperty, (object) value);
  }

  public int PageIndex
  {
    get => (int) this.GetValue(PdfControl.PageIndexProperty);
    set => this.SetValue(PdfControl.PageIndexProperty, (object) value);
  }

  public ViewModes ViewMode
  {
    get => (ViewModes) this.GetValue(PdfControl.ViewModeProperty);
    set => this.SetValue(PdfControl.ViewModeProperty, (object) value);
  }

  public SizeModes SizeMode
  {
    get => (SizeModes) this.GetValue(PdfControl.SizeModeProperty);
    set => this.SetValue(PdfControl.SizeModeProperty, (object) value);
  }

  public float Zoom
  {
    get => (float) this.GetValue(PdfControl.ZoomProperty);
    set => this.SetValue(PdfControl.ZoomProperty, (object) value);
  }

  public ScrollViewer ScrollViewer
  {
    get => !this.IsEditing ? (ScrollViewer) this._ViewerScroll : (ScrollViewer) this._EditorScroll;
  }

  private void UpdateEditMode()
  {
    ScrollAnchorPointUtils.PdfViewerScrollSnapshot scrollSnapshot = ScrollAnchorPointUtils.CreateScrollSnapshot(this);
    int pageIndex = this.PageIndex;
    PdfControlPageIndexBinding pageIndexBinding1 = this.ActualEditing ? this.editorPageIdxBinding : this.viewerPageIdxBinding;
    if (pageIndexBinding1 != null)
      pageIndexBinding1.PageIndexChanged -= new EventHandler(this.PageIndexBinding_PageIndexChanged);
    this.Viewer.ZoomChanged -= new EventHandler(this.InternalZoomChanged);
    this.Editor.ZoomChanged -= new EventHandler(this.InternalZoomChanged);
    if (this.IsEditing)
    {
      if (this.Viewer != null)
        this.Viewer.Document = (PdfDocument) null;
      this._EditorScroll.Visibility = Visibility.Visible;
      this._ViewerScroll.Visibility = Visibility.Collapsed;
      if (this.Document != null)
        this.Document.Pages.CurrentIndex = pageIndex;
      if (this.Editor != null)
        this.Editor.Document = this.Document;
    }
    else
    {
      (int startPage, int endPage) = this.GetVisiblePageRange();
      this.lastChangedPages = this.Editor?.GetChangedPageIndexes();
      if (this.Editor != null)
        this.Editor.Document = (PdfDocument) null;
      if (startPage != -1 && endPage != -1)
      {
        for (int index = startPage; index <= endPage; ++index)
          this.Document.Pages[index].Dispose();
      }
      this._EditorScroll.Visibility = Visibility.Collapsed;
      this._ViewerScroll.Visibility = Visibility.Visible;
      if (this.Document != null)
        this.Document.Pages.CurrentIndex = pageIndex;
      if (this.Viewer != null)
        this.Viewer.Document = this.Document;
    }
    this.UpdateLayout();
    this.ActualEditing = this.IsEditing;
    ScrollAnchorPointUtils.ApplyScrollSnapshot(this, scrollSnapshot);
    PdfControlPageIndexBinding pageIndexBinding2 = this.ActualEditing ? this.editorPageIdxBinding : this.viewerPageIdxBinding;
    if (pageIndexBinding2 != null)
      pageIndexBinding2.PageIndexChanged += new EventHandler(this.PageIndexBinding_PageIndexChanged);
    this.Zoom = this.GetCurrentPdfScrollInfo().Zoom;
    if (this.IsEditing)
      this.Editor.ZoomChanged += new EventHandler(this.InternalZoomChanged);
    else
      this.Viewer.ZoomChanged += new EventHandler(this.InternalZoomChanged);
  }

  private void PageIndexBinding_PageIndexChanged(object sender, EventArgs e)
  {
    this.PageIndex = ((PdfControlPageIndexBinding) sender).PageIndex;
  }

  private void InternalZoomChanged(object sender, EventArgs e)
  {
    this.Zoom = this.GetCurrentPdfScrollInfo().Zoom;
  }

  private void UpdateDocument()
  {
    if (this.IsEditing)
    {
      if (this.Editor != null)
        this.Editor.Document = this.Document;
      if (this.Viewer != null)
        this.Viewer.Document = (PdfDocument) null;
    }
    else
    {
      if (this.Viewer != null)
        this.Viewer.Document = this.Document;
      if (this.Editor != null)
        this.Editor.Document = (PdfDocument) null;
    }
    this.Zoom = this.GetCurrentPdfScrollInfo().Zoom;
  }

  public event ScrollChangedEventHandler ScrollChanged;

  private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
  {
    ScrollChangedEventHandler scrollChanged = this.ScrollChanged;
    if (scrollChanged == null)
      return;
    scrollChanged((object) this, e);
  }

  private IPdfScrollInfoInternal GetCurrentPdfScrollInfo()
  {
    return this.ActualEditing ? (IPdfScrollInfoInternal) this.Editor : (IPdfScrollInfoInternal) this.Viewer;
  }

  public int[] GetChangedPageIndexes()
  {
    return this.IsEditing ? this.Editor?.GetChangedPageIndexes() ?? Array.Empty<int>() : this.lastChangedPages ?? Array.Empty<int>();
  }

  public void UpdateDocLayout()
  {
    if (this.IsEditing)
      this.Editor?.UpdateDocLayout();
    else
      this.Viewer?.UpdateDocLayout();
  }

  public void Redraw(bool force)
  {
    if (force)
    {
      this.Viewer?.ForceRender();
      this.Editor?.ForceRender();
    }
    else
    {
      this.Viewer?.InvalidateVisual();
      this.Editor?.InvalidateVisual();
    }
  }

  public int StartPage => this.GetCurrentPdfScrollInfo().StartPage;

  public int EndPage => this.GetCurrentPdfScrollInfo().EndPage;

  public bool CanVerticallyScroll => this.GetCurrentPdfScrollInfo().CanVerticallyScroll;

  public bool CanHorizontallyScroll => this.GetCurrentPdfScrollInfo().CanHorizontallyScroll;

  public double ExtentWidth => this.GetCurrentPdfScrollInfo().ExtentWidth;

  public double ExtentHeight => this.GetCurrentPdfScrollInfo().ExtentHeight;

  public double ViewportWidth => this.GetCurrentPdfScrollInfo().ViewportWidth;

  public double ViewportHeight => this.GetCurrentPdfScrollInfo().ViewportHeight;

  public double HorizontalOffset => this.GetCurrentPdfScrollInfo().HorizontalOffset;

  public double VerticalOffset => this.GetCurrentPdfScrollInfo().VerticalOffset;

  public Rect CalcActualRect(int index) => this.GetCurrentPdfScrollInfo().CalcActualRect(index);

  public Point ClientToPage(int pageIndex, Point pt)
  {
    return this.GetCurrentPdfScrollInfo().ClientToPage(pageIndex, pt);
  }

  public FS_RECTF ClientToPageRect(Rect rect, int pageIndex)
  {
    return this.GetCurrentPdfScrollInfo().ClientToPageRect(rect, pageIndex);
  }

  public Point PageToClient(int pageIndex, Point pt)
  {
    return this.GetCurrentPdfScrollInfo().PageToClient(pageIndex, pt);
  }

  public Rect PageToClientRect(FS_RECTF rc, int pageIndex)
  {
    return this.GetCurrentPdfScrollInfo().PageToClientRect(rc, pageIndex);
  }

  public void ScrollToPage(int index) => this.GetCurrentPdfScrollInfo().ScrollToPage(index);

  public int DeviceToPage(double x, double y, out Point pagePoint)
  {
    return this.GetCurrentPdfScrollInfo().DeviceToPage(x, y, out pagePoint);
  }

  public static PdfControl GetPdfControl(IPdfScrollInfoInternal obj)
  {
    switch (obj)
    {
      case PdfControl pdfControl:
        return pdfControl;
      case DependencyObject dependencyObject:
        return dependencyObject.GetValue(PdfControl.PdfControlProperty) as PdfControl;
      default:
        return (PdfControl) null;
    }
  }

  public static PdfControl GetPdfControl(PdfDocument doc)
  {
    if (doc == null)
      return (PdfControl) null;
    lock (PdfControl.docCache)
    {
      PdfControl.PdfControlCache pdfControlCache = PdfControl.docCache.FirstOrDefault<PdfControl.PdfControlCache>((Func<PdfControl.PdfControlCache, bool>) (c => c.Document == doc));
      PdfControl.ClearInvalidItems();
      return pdfControlCache?.PdfControl;
    }
  }

  private static void ClearInvalidItems()
  {
    lock (PdfControl.docCache)
    {
      for (int index = PdfControl.docCache.Count - 1; index >= 0; --index)
      {
        PdfControl.PdfControlCache pdfControlCache = PdfControl.docCache[index];
        if (pdfControlCache.Document == null || pdfControlCache.PdfControl == null)
          PdfControl.docCache.RemoveAt(index);
      }
    }
  }

  public void RedoEditor()
  {
    try
    {
      this._Editor.operationManager.Redo();
      this._Editor.UpdateCaretInfo();
      this.Redraw(true);
    }
    catch (Exception ex)
    {
      Common.WriteLog(ex.ToString());
    }
  }

  public void UndoEditor()
  {
    try
    {
      this._Editor.operationManager.Undo();
      this._Editor.UpdateCaretInfo();
      this.Redraw(true);
    }
    catch (Exception ex)
    {
      Common.WriteLog(ex.ToString());
    }
  }

  public void ClearEditorUndoStack() => this._Editor.operationManager.Clear();

  public event EventHandler EditorUndoStateChanged;

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "9.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/PDFKit;component/pdfcontrol.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "9.0.0.0")]
  internal Delegate _CreateDelegate(Type delegateType, string handler)
  {
    return Delegate.CreateDelegate(delegateType, (object) this, handler);
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "9.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this._ViewerScroll = (PdfViewerScrollViewer) target;
        break;
      case 2:
        this._Viewer = (PdfViewer) target;
        break;
      case 3:
        this._EditorScroll = (PdfViewerScrollViewer) target;
        break;
      case 4:
        this._Editor = (PdfEditor) target;
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }

  private class PdfControlCache : IEquatable<PdfControl.PdfControlCache>
  {
    private int _hashCode;
    private WeakReference<PdfControl> weakPdfViewer;
    private readonly WeakReference<PdfDocument> weakDocument;

    public PdfControlCache(PdfControl pdfControl, PdfDocument document)
    {
      this.weakPdfViewer = pdfControl != null ? new WeakReference<PdfControl>(pdfControl) : throw new ArgumentException(nameof (pdfControl));
      this.weakDocument = new WeakReference<PdfDocument>(document);
      this._hashCode = HashCode.Combine<PdfControl, PdfDocument>(pdfControl, document);
    }

    public PdfControl PdfControl
    {
      get
      {
        PdfControl target;
        return this.weakPdfViewer.TryGetTarget(out target) ? target : (PdfControl) null;
      }
    }

    public PdfDocument Document
    {
      get
      {
        PdfDocument target;
        return this.weakDocument.TryGetTarget(out target) ? target : (PdfDocument) null;
      }
    }

    public bool Equals(PdfControl.PdfControlCache other)
    {
      return other.PdfControl == this.PdfControl && other.Document == this.Document;
    }

    public override bool Equals(object obj)
    {
      return obj is PdfControl.PdfControlCache other && this.Equals(other);
    }

    public override int GetHashCode() => this._hashCode;
  }
}
