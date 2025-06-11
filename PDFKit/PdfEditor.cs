// Decompiled with JetBrains decompiler
// Type: PDFKit.PdfEditor
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.EventArguments;
using Patagames.Pdf.Net.Exceptions;
using Patagames.Pdf.Net.Wrappers;
using PDFKit.Contents;
using PDFKit.Contents.Controls;
using PDFKit.Contents.Operations;
using PDFKit.Contents.Utils;
using PDFKit.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Media;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

#nullable disable
namespace PDFKit;

[LicenseProvider]
public class PdfEditor : Control, IScrollInfo, IPdfScrollInfo, IPdfScrollInfoInternal
{
  private bool _preventStackOverflowBugWorkaround = false;
  private DateTime? _lastPressedTime = new DateTime?();
  private int _onstartPageIndex = 0;
  private PdfForms _fillForms;
  private List<Rect> _selectedRectangles = new List<Rect>();
  private Pen _pageBorderColorPen = Helpers.CreatePen((Color) PdfEditor.PageBorderColorProperty.DefaultMetadata.DefaultValue);
  private Pen _pageCropBoxColorPen = Helpers.CreatePen((Brush) Brushes.Red);
  private Pen _pageSeparatorColorPen = Helpers.CreatePen((Color) PdfEditor.PageSeparatorColorProperty.DefaultMetadata.DefaultValue);
  private Pen _currentPageHighlightColorPen = Helpers.CreatePen((Color) PdfEditor.CurrentPageHighlightColorProperty.DefaultMetadata.DefaultValue, 4.0);
  private Pen _paragraphBorderColorPen = Helpers.CreatePen(Color.FromArgb((byte) 100, (byte) 0, (byte) 0, (byte) 0));
  private Pen _paragraphBorderSelectedColorPen = Helpers.CreatePen(Colors.Black);
  private Brush _paragraphDragingColorBrush = Helpers.CreateBrush(Color.FromArgb((byte) 51, (byte) 24, (byte) 146, byte.MaxValue));
  private RenderRect[] _renderRects;
  private bool _preferredSelectedPage = false;
  private CachedCanvasBitmap cachedCanvasBitmap;
  private MouseWheelHelper mouseWheelHelper;
  private LogicalStructAnalyser[] analysers;
  internal CaretInfo caretInfo;
  private ImeReceiver imeReceiver;
  private HwndSource hwndSource;
  private HwndSourceHook hwndSourceHook;
  internal OperationManager operationManager;
  private PdfEditorKeyboardProcessor keyboardProcessor;
  private TextProperties textProperties;
  private FS_POINTF? selectParagraphMouseDownPoint;
  private FS_POINTF? selectParagraphMouseCurPoint;
  private WeakReference<Window> window;
  private Size _extent = new Size(0.0, 0.0);
  private Size _viewport = new Size(0.0, 0.0);
  private Point _autoScrollPosition = new Point(0.0, 0.0);
  private bool _isProgrammaticallyFocusSetted = false;
  internal PRCollection _prPages = new PRCollection();
  private DispatcherTimer _invalidateTimer = (DispatcherTimer) null;
  private WriteableBitmap _canvasWpfBitmap = (WriteableBitmap) null;
  private bool _loadedByViewer = true;
  private PdfEditor.CaptureInfo _externalDocCapture;
  private Point _scrollPoint;
  private bool _scrollPointSaved;
  public static readonly DependencyProperty FormsBlendModeProperty = DependencyProperty.Register(nameof (FormsBlendMode), typeof (BlendTypes), typeof (PdfEditor), (PropertyMetadata) new FrameworkPropertyMetadata((object) BlendTypes.FXDIB_BLEND_MULTIPLY, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) => (o as PdfEditor).OnFormsBlendModeChanged(EventArgs.Empty))));
  public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register(nameof (Document), typeof (PdfDocument), typeof (PdfEditor), new PropertyMetadata((object) null, (PropertyChangedCallback) ((o, e) =>
  {
    PdfEditor pdfEditor = o as PdfEditor;
    PdfDocument oldValue = e.OldValue as PdfDocument;
    PdfDocument newValue = e.NewValue as PdfDocument;
    if (oldValue == newValue)
      return;
    int num = oldValue != null ? oldValue.Pages.CurrentIndex : -1;
    pdfEditor._prPages.ReleaseCanvas();
    if (oldValue != null && pdfEditor._loadedByViewer)
    {
      oldValue.Dispose();
      pdfEditor.OnDocumentClosed(EventArgs.Empty);
    }
    else if (oldValue != null && !pdfEditor._loadedByViewer)
    {
      oldValue.Pages.CurrentPageChanged -= new EventHandler(pdfEditor.Pages_CurrentPageChanged);
      oldValue.Pages.PageInserted -= new EventHandler<PageCollectionChangedEventArgs>(pdfEditor.Pages_PageInserted);
      oldValue.Pages.PageDeleted -= new EventHandler<PageCollectionChangedEventArgs>(pdfEditor.Pages_PageDeleted);
      oldValue.Pages.ProgressiveRender -= new EventHandler<ProgressiveRenderEventArgs>(pdfEditor.Pages_ProgressiveRender);
    }
    pdfEditor.analysers = (LogicalStructAnalyser[]) null;
    pdfEditor.operationManager?.Clear();
    pdfEditor._extent = new Size(0.0, 0.0);
    pdfEditor._renderRects = (RenderRect[]) null;
    pdfEditor._loadedByViewer = false;
    pdfEditor.ReleaseFillForms(pdfEditor._externalDocCapture);
    pdfEditor.UpdateDocLayout();
    if (newValue != null)
    {
      pdfEditor.analysers = new LogicalStructAnalyser[newValue.Pages.Count];
      if (newValue.FormFill != pdfEditor._fillForms)
        pdfEditor._externalDocCapture = pdfEditor.CaptureFillForms(newValue.FormFill);
      newValue.Pages.CurrentPageChanged += new EventHandler(pdfEditor.Pages_CurrentPageChanged);
      newValue.Pages.PageInserted += new EventHandler<PageCollectionChangedEventArgs>(pdfEditor.Pages_PageInserted);
      newValue.Pages.PageDeleted += new EventHandler<PageCollectionChangedEventArgs>(pdfEditor.Pages_PageDeleted);
      newValue.Pages.ProgressiveRender += new EventHandler<ProgressiveRenderEventArgs>(pdfEditor.Pages_ProgressiveRender);
      if (Pdfium.IsFullAPI && newValue.OpenDestination != null)
        pdfEditor._onstartPageIndex = newValue.OpenDestination.PageIndex;
      if (newValue.Pages.CurrentIndex != pdfEditor._onstartPageIndex)
        pdfEditor.SetCurrentPage(pdfEditor._onstartPageIndex);
      else if (newValue.Pages.CurrentIndex != num)
        pdfEditor.OnCurrentPageChanged(EventArgs.Empty);
      if (newValue.Pages.Count > 0)
      {
        if (pdfEditor._onstartPageIndex != 0)
          pdfEditor.ScrollToPage(pdfEditor._onstartPageIndex);
        else
          pdfEditor._autoScrollPosition = new Point(0.0, 0.0);
      }
      pdfEditor._onstartPageIndex = 0;
      if (newValue.Pages.CurrentIndex >= 0)
        pdfEditor.GetPageStructAnalyser(newValue.Pages.CurrentIndex);
    }
    pdfEditor.OnAfterDocumentChanged(EventArgs.Empty);
  }), (CoerceValueCallback) ((dobj, o) =>
  {
    PdfEditor pdfEditor = dobj as PdfEditor;
    PdfDocument document = pdfEditor.Document;
    PdfDocument pdfDocument = o as PdfDocument;
    return document != pdfDocument && (pdfEditor.OnBeforeDocumentChanged(new DocumentClosingEventArgs()) || document != null && pdfEditor._loadedByViewer && pdfEditor.OnDocumentClosing(new DocumentClosingEventArgs())) ? (object) document : (object) pdfDocument;
  })));
  public static readonly DependencyProperty PageBackColorProperty = DependencyProperty.Register(nameof (PageBackColor), typeof (Color), typeof (PdfEditor), (PropertyMetadata) new FrameworkPropertyMetadata((object) Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) => (o as PdfEditor).OnPageBackColorChanged(EventArgs.Empty))));
  public static readonly DependencyProperty PageMarginProperty = DependencyProperty.Register(nameof (PageMargin), typeof (Thickness), typeof (PdfEditor), (PropertyMetadata) new FrameworkPropertyMetadata((object) new Thickness(10.0), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) =>
  {
    (o as PdfEditor).UpdateDocLayout();
    (o as PdfEditor).OnPageMarginChanged(EventArgs.Empty);
  })));
  public new static readonly DependencyProperty PaddingProperty = DependencyProperty.Register(nameof (Padding), typeof (Thickness), typeof (PdfEditor), (PropertyMetadata) new FrameworkPropertyMetadata((object) new Thickness(10.0), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) => (o as PdfEditor).UpdateDocLayout())));
  public static readonly DependencyProperty PageBorderColorProperty = DependencyProperty.Register(nameof (PageBorderColor), typeof (Color), typeof (PdfEditor), (PropertyMetadata) new FrameworkPropertyMetadata((object) Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 0, (byte) 0), FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) =>
  {
    PdfEditor pdfEditor = o as PdfEditor;
    pdfEditor._pageBorderColorPen = Helpers.CreatePen((Color) e.NewValue);
    pdfEditor.OnPageBorderColorChanged(EventArgs.Empty);
  })));
  public static readonly DependencyProperty SizeModeProperty = DependencyProperty.Register(nameof (SizeMode), typeof (SizeModes), typeof (PdfEditor), (PropertyMetadata) new FrameworkPropertyMetadata((object) SizeModes.FitToWidth, FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) =>
  {
    PdfEditor pdfEditor = o as PdfEditor;
    pdfEditor.UpdateDocLayout();
    pdfEditor.OnSizeModeChanged(EventArgs.Empty);
  })));
  public static readonly DependencyProperty TextSelectColorProperty = DependencyProperty.Register(nameof (TextSelectColor), typeof (Color), typeof (PdfEditor), (PropertyMetadata) new FrameworkPropertyMetadata((object) Color.FromArgb((byte) 70, (byte) 70, (byte) 130, (byte) 180), FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) => (o as PdfEditor).OnTextSelectColorChanged(EventArgs.Empty))));
  public static readonly DependencyProperty FormHighlightColorProperty = DependencyProperty.Register(nameof (FormHighlightColor), typeof (Color), typeof (PdfEditor), (PropertyMetadata) new FrameworkPropertyMetadata((object) Color.FromArgb((byte) 0, byte.MaxValue, byte.MaxValue, byte.MaxValue), FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) =>
  {
    PdfEditor pdfEditor = o as PdfEditor;
    if (pdfEditor._fillForms != null)
      pdfEditor._fillForms.SetHighlightColorEx(FormFieldTypes.FPDF_FORMFIELD_UNKNOWN, Helpers.ToArgb((Color) e.NewValue));
    if (pdfEditor.Document != null && !pdfEditor._loadedByViewer && pdfEditor._externalDocCapture.forms != null)
      pdfEditor._externalDocCapture.forms.SetHighlightColorEx(FormFieldTypes.FPDF_FORMFIELD_UNKNOWN, Helpers.ToArgb((Color) e.NewValue));
    pdfEditor.OnFormHighlightColorChanged(EventArgs.Empty);
  })));
  public static readonly DependencyProperty ZoomProperty = DependencyProperty.Register(nameof (Zoom), typeof (float), typeof (PdfEditor), (PropertyMetadata) new FrameworkPropertyMetadata((object) 1f, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) =>
  {
    PdfEditor pdfEditor = o as PdfEditor;
    if (pdfEditor._preventStackOverflowBugWorkaround)
      return;
    pdfEditor.UpdateDocLayout();
    pdfEditor.OnZoomChanged(EventArgs.Empty);
  })));
  public static readonly DependencyProperty ViewModeProperty = DependencyProperty.Register(nameof (ViewMode), typeof (ViewModes), typeof (PdfEditor), (PropertyMetadata) new FrameworkPropertyMetadata((object) ViewModes.Vertical, FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) =>
  {
    PdfEditor pdfEditor = o as PdfEditor;
    pdfEditor.UpdateDocLayout();
    pdfEditor.OnViewModeChanged(EventArgs.Empty);
  })));
  public static readonly DependencyProperty PageSeparatorColorProperty = DependencyProperty.Register(nameof (PageSeparatorColor), typeof (Color), typeof (PdfEditor), (PropertyMetadata) new FrameworkPropertyMetadata((object) Color.FromArgb(byte.MaxValue, (byte) 190, (byte) 190, (byte) 190), FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) =>
  {
    PdfEditor pdfEditor = o as PdfEditor;
    pdfEditor._pageSeparatorColorPen = Helpers.CreatePen((Color) e.NewValue);
    pdfEditor.OnPageSeparatorColorChanged(EventArgs.Empty);
  })));
  public static readonly DependencyProperty ShowPageSeparatorProperty = DependencyProperty.Register(nameof (ShowPageSeparator), typeof (bool), typeof (PdfEditor), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) => (o as PdfEditor).OnShowPageSeparatorChanged(EventArgs.Empty))));
  public static readonly DependencyProperty CurrentPageHighlightColorProperty = DependencyProperty.Register(nameof (CurrentPageHighlightColor), typeof (Color), typeof (PdfEditor), (PropertyMetadata) new FrameworkPropertyMetadata((object) Color.FromArgb((byte) 170, (byte) 70, (byte) 130, (byte) 180), FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) =>
  {
    PdfEditor pdfEditor = o as PdfEditor;
    pdfEditor._currentPageHighlightColorPen = Helpers.CreatePen((Color) e.NewValue);
    pdfEditor.OnCurrentPageHighlightColorChanged(EventArgs.Empty);
  })));
  public static readonly DependencyProperty ShowCurrentPageHighlightProperty = DependencyProperty.Register(nameof (ShowCurrentPageHighlight), typeof (bool), typeof (PdfEditor), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) => (o as PdfEditor).OnShowCurrentPageHighlightChanged(EventArgs.Empty))));
  public static readonly DependencyProperty PageVAlignProperty = DependencyProperty.Register(nameof (PageVAlign), typeof (VerticalAlignment), typeof (PdfEditor), (PropertyMetadata) new FrameworkPropertyMetadata((object) VerticalAlignment.Center, FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) =>
  {
    PdfEditor pdfEditor = o as PdfEditor;
    pdfEditor.UpdateDocLayout();
    pdfEditor.OnPageAlignChanged(EventArgs.Empty);
  })));
  public static readonly DependencyProperty PageHAlignProperty = DependencyProperty.Register(nameof (PageHAlign), typeof (HorizontalAlignment), typeof (PdfEditor), (PropertyMetadata) new FrameworkPropertyMetadata((object) HorizontalAlignment.Center, FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) =>
  {
    PdfEditor pdfEditor = o as PdfEditor;
    pdfEditor.UpdateDocLayout();
    pdfEditor.OnPageAlignChanged(EventArgs.Empty);
  })));
  public static readonly DependencyProperty RenderFlagsProperty = DependencyProperty.Register(nameof (RenderFlags), typeof (RenderFlags), typeof (PdfEditor), (PropertyMetadata) new FrameworkPropertyMetadata((object) (RenderFlags.FPDF_LCD_TEXT | RenderFlags.FPDF_NO_CATCH), FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) => (o as PdfEditor).OnRenderFlagsChanged(EventArgs.Empty))));
  public static readonly DependencyProperty TilesCountProperty = DependencyProperty.Register(nameof (TilesCount), typeof (int), typeof (PdfEditor), (PropertyMetadata) new FrameworkPropertyMetadata((object) 2, FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) =>
  {
    PdfEditor pdfEditor = o as PdfEditor;
    pdfEditor.UpdateDocLayout();
    pdfEditor.OnTilesCountChanged(EventArgs.Empty);
  }), (CoerceValueCallback) ((v, o) => (int) o < 2 ? (object) 2 : o)));
  public static readonly DependencyProperty ShowLoadingIconProperty = DependencyProperty.Register(nameof (ShowLoadingIcon), typeof (bool), typeof (PdfEditor), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) => (o as PdfEditor).OnShowLoadingIconChanged(EventArgs.Empty))));
  public static readonly DependencyProperty UseProgressiveRenderProperty = DependencyProperty.Register(nameof (UseProgressiveRender), typeof (bool), typeof (PdfEditor), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) =>
  {
    PdfEditor pdfEditor = o as PdfEditor;
    pdfEditor.UpdateDocLayout();
    pdfEditor.OnUseProgressiveRenderChanged(EventArgs.Empty);
  })));
  public static readonly DependencyProperty LoadingIconTextProperty = DependencyProperty.Register(nameof (LoadingIconText), typeof (string), typeof (PdfEditor), (PropertyMetadata) new FrameworkPropertyMetadata((object) "", FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) => (o as PdfEditor).OnLoadingIconTextChanged(EventArgs.Empty))));
  public static readonly DependencyProperty PageAutoDisposeProperty = DependencyProperty.Register(nameof (PageAutoDispose), typeof (bool), typeof (PdfEditor), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, FrameworkPropertyMetadataOptions.Journal));
  public static readonly DependencyProperty OptimizedLoadThresholdProperty = DependencyProperty.Register(nameof (OptimizedLoadThreshold), typeof (int), typeof (PdfEditor), (PropertyMetadata) new FrameworkPropertyMetadata((object) 1000));
  public static readonly DependencyProperty FlyoutExtentWidthProperty = DependencyProperty.Register(nameof (FlyoutExtentWidth), typeof (double), typeof (PdfEditor), new PropertyMetadata((object) 0.0, (PropertyChangedCallback) ((s, a) => ((PdfEditor) s).ScrollOwner?.InvalidateScrollInfo())));
  public static readonly DependencyProperty IsAnnotationVisibleProperty = DependencyProperty.Register(nameof (IsAnnotationVisible), typeof (bool), typeof (PdfEditor), new PropertyMetadata((object) true, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is PdfEditor pdfEditor2))
      return;
    pdfEditor2.cachedCanvasBitmap?.Dispose();
    pdfEditor2.cachedCanvasBitmap = (CachedCanvasBitmap) null;
    pdfEditor2.InvalidateVisual();
  })));
  public static readonly DependencyProperty PageMaskBrushProperty = DependencyProperty.Register(nameof (PageMaskBrush), typeof (Brush), typeof (PdfEditor), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty IsRenderPausedProperty = DependencyProperty.Register(nameof (IsRenderPaused), typeof (bool), typeof (PdfEditor), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty MouseModeProperty = DependencyProperty.Register(nameof (MouseMode), typeof (EditorMouseModes), typeof (PdfEditor), (PropertyMetadata) new FrameworkPropertyMetadata((object) EditorMouseModes.SelectParagraph, FrameworkPropertyMetadataOptions.AffectsRender));
  private bool imeProcessing;
  private const CursorTypes CursorTypesSizeAll = (CursorTypes) 10;
  private ScrollViewer scrollOwner;

  private int _startPage
  {
    get
    {
      if (this.Document == null)
        return 0;
      switch (this.ViewMode)
      {
        case ViewModes.SinglePage:
          return this.Document.Pages.CurrentIndex;
        case ViewModes.TilesLine:
          return this.Document.Pages.CurrentIndex % this.TilesCount == 0 ? this.Document.Pages.CurrentIndex : this.Document.Pages.CurrentIndex - this.Document.Pages.CurrentIndex % this.TilesCount;
        default:
          return 0;
      }
    }
  }

  private int _endPage
  {
    get
    {
      if (this.Document == null)
        return -1;
      switch (this.ViewMode)
      {
        case ViewModes.SinglePage:
          return this.Document.Pages.CurrentIndex;
        case ViewModes.TilesLine:
          return Math.Min(this._startPage + this.TilesCount - 1, this._renderRects != null ? this._renderRects.Length - 1 : -1);
        default:
          return this._renderRects != null ? this._renderRects.Length - 1 : -1;
      }
    }
  }

  int IPdfScrollInfoInternal.StartPage => this._startPage;

  int IPdfScrollInfoInternal.EndPage => this._endPage;

  internal int StartPage => this._startPage;

  internal int EndPage => this._endPage;

  public event EventHandler AfterDocumentChanged;

  public event EventHandler<DocumentClosingEventArgs> BeforeDocumentChanged;

  public event EventHandler DocumentLoaded;

  public event EventHandler<DocumentClosingEventArgs> DocumentClosing;

  public event EventHandler DocumentClosed;

  public event EventHandler SizeModeChanged;

  public event EventHandler PageBackColorChanged;

  public event EventHandler PageMarginChanged;

  public event EventHandler PageBorderColorChanged;

  public event EventHandler TextSelectColorChanged;

  public event EventHandler FormHighlightColorChanged;

  public event EventHandler ZoomChanged;

  public event EventHandler SelectionChanged;

  public event EventHandler ViewModeChanged;

  public event EventHandler PageSeparatorColorChanged;

  public event EventHandler ShowPageSeparatorChanged;

  public event EventHandler CurrentPageChanged;

  public event EventHandler CurrentPageHighlightColorChanged;

  public event EventHandler ShowCurrentPageHighlightChanged;

  public event EventHandler PageAlignChanged;

  public event EventHandler RenderFlagsChanged;

  public event EventHandler TilesCountChanged;

  public event EventHandler ShowLoadingIconChanged;

  public event EventHandler UseProgressiveRenderChanged;

  public event EventHandler LoadingIconTextChanged;

  public event EventHandler FormsBlendModeChanged;

  protected virtual void OnAfterDocumentChanged(EventArgs e)
  {
    if (this.AfterDocumentChanged == null)
      return;
    this.AfterDocumentChanged((object) this, e);
  }

  protected virtual bool OnBeforeDocumentChanged(DocumentClosingEventArgs e)
  {
    if (this.BeforeDocumentChanged != null)
      this.BeforeDocumentChanged((object) this, e);
    return e.Cancel;
  }

  protected virtual void OnDocumentLoaded(EventArgs e)
  {
    if (this.DocumentLoaded == null)
      return;
    this.DocumentLoaded((object) this, e);
  }

  protected virtual bool OnDocumentClosing(DocumentClosingEventArgs e)
  {
    if (this.DocumentClosing != null)
      this.DocumentClosing((object) this, e);
    return e.Cancel;
  }

  protected virtual void OnDocumentClosed(EventArgs e)
  {
    if (this.DocumentClosed == null)
      return;
    this.DocumentClosed((object) this, e);
  }

  protected virtual void OnSizeModeChanged(EventArgs e)
  {
    if (this.SizeModeChanged == null)
      return;
    this.SizeModeChanged((object) this, e);
  }

  protected virtual void OnPageBackColorChanged(EventArgs e)
  {
    if (this.PageBackColorChanged == null)
      return;
    this.PageBackColorChanged((object) this, e);
  }

  protected virtual void OnPageMarginChanged(EventArgs e)
  {
    if (this.PageMarginChanged == null)
      return;
    this.PageMarginChanged((object) this, e);
  }

  protected virtual void OnPageBorderColorChanged(EventArgs e)
  {
    if (this.PageBorderColorChanged == null)
      return;
    this.PageBorderColorChanged((object) this, e);
  }

  protected virtual void OnTextSelectColorChanged(EventArgs e)
  {
    if (this.TextSelectColorChanged == null)
      return;
    this.TextSelectColorChanged((object) this, e);
  }

  protected virtual void OnFormHighlightColorChanged(EventArgs e)
  {
    if (this.FormHighlightColorChanged == null)
      return;
    this.FormHighlightColorChanged((object) this, e);
  }

  protected virtual void OnZoomChanged(EventArgs e)
  {
    if (this.ZoomChanged == null)
      return;
    this.ZoomChanged((object) this, e);
  }

  protected virtual void OnSelectionChanged(EventArgs e)
  {
    if (this.SelectionChanged == null)
      return;
    this.SelectionChanged((object) this, e);
  }

  protected virtual void OnViewModeChanged(EventArgs e)
  {
    if (this.ViewModeChanged == null)
      return;
    this.ViewModeChanged((object) this, e);
  }

  protected virtual void OnPageSeparatorColorChanged(EventArgs e)
  {
    if (this.PageSeparatorColorChanged == null)
      return;
    this.PageSeparatorColorChanged((object) this, e);
  }

  protected virtual void OnShowPageSeparatorChanged(EventArgs e)
  {
    if (this.ShowPageSeparatorChanged == null)
      return;
    this.ShowPageSeparatorChanged((object) this, e);
  }

  protected virtual void OnCurrentPageChanged(EventArgs e)
  {
    if (this.CurrentPageChanged == null)
      return;
    this.CurrentPageChanged((object) this, e);
  }

  protected virtual void OnCurrentPageHighlightColorChanged(EventArgs e)
  {
    if (this.CurrentPageHighlightColorChanged == null)
      return;
    this.CurrentPageHighlightColorChanged((object) this, e);
  }

  protected virtual void OnShowCurrentPageHighlightChanged(EventArgs e)
  {
    if (this.ShowCurrentPageHighlightChanged == null)
      return;
    this.ShowCurrentPageHighlightChanged((object) this, e);
  }

  protected virtual void OnPageAlignChanged(EventArgs e)
  {
    if (this.PageAlignChanged == null)
      return;
    this.PageAlignChanged((object) this, e);
  }

  protected virtual void OnRenderFlagsChanged(EventArgs e)
  {
    if (this.RenderFlagsChanged == null)
      return;
    this.RenderFlagsChanged((object) this, e);
  }

  protected virtual void OnTilesCountChanged(EventArgs e)
  {
    if (this.TilesCountChanged == null)
      return;
    this.TilesCountChanged((object) this, e);
  }

  protected virtual void OnShowLoadingIconChanged(EventArgs e)
  {
    if (this.ShowLoadingIconChanged == null)
      return;
    this.ShowLoadingIconChanged((object) this, e);
  }

  protected virtual void OnUseProgressiveRenderChanged(EventArgs e)
  {
    if (this.UseProgressiveRenderChanged == null)
      return;
    this.UseProgressiveRenderChanged((object) this, e);
  }

  protected virtual void OnLoadingIconTextChanged(EventArgs e)
  {
    if (this.LoadingIconTextChanged == null)
      return;
    this.LoadingIconTextChanged((object) this, e);
  }

  protected virtual void OnFormsBlendModeChanged(EventArgs e)
  {
    if (this.FormsBlendModeChanged == null)
      return;
    this.FormsBlendModeChanged((object) this, e);
  }

  public BlendTypes FormsBlendMode
  {
    get => (BlendTypes) this.GetValue(PdfEditor.FormsBlendModeProperty);
    set => this.SetValue(PdfEditor.FormsBlendModeProperty, (object) value);
  }

  public PdfDocument Document
  {
    get => (PdfDocument) this.GetValue(PdfEditor.DocumentProperty);
    set => this.SetValue(PdfEditor.DocumentProperty, (object) value);
  }

  public Color PageBackColor
  {
    get => (Color) this.GetValue(PdfEditor.PageBackColorProperty);
    set => this.SetValue(PdfEditor.PageBackColorProperty, (object) value);
  }

  public Thickness PageMargin
  {
    get => (Thickness) this.GetValue(PdfEditor.PageMarginProperty);
    set => this.SetValue(PdfEditor.PageMarginProperty, (object) value);
  }

  public new Thickness Padding
  {
    get => (Thickness) this.GetValue(PdfEditor.PaddingProperty);
    set => this.SetValue(PdfEditor.PaddingProperty, (object) value);
  }

  public Color PageBorderColor
  {
    get => (Color) this.GetValue(PdfEditor.PageBorderColorProperty);
    set => this.SetValue(PdfEditor.PageBorderColorProperty, (object) value);
  }

  public SizeModes SizeMode
  {
    get => (SizeModes) this.GetValue(PdfEditor.SizeModeProperty);
    set => this.SetValue(PdfEditor.SizeModeProperty, (object) value);
  }

  public Color TextSelectColor
  {
    get => (Color) this.GetValue(PdfEditor.TextSelectColorProperty);
    set => this.SetValue(PdfEditor.TextSelectColorProperty, (object) value);
  }

  public Color FormHighlightColor
  {
    get => (Color) this.GetValue(PdfEditor.FormHighlightColorProperty);
    set => this.SetValue(PdfEditor.FormHighlightColorProperty, (object) value);
  }

  public float Zoom
  {
    get => (float) this.GetValue(PdfEditor.ZoomProperty);
    set => this.SetValue(PdfEditor.ZoomProperty, (object) value);
  }

  public ViewModes ViewMode
  {
    get => (ViewModes) this.GetValue(PdfEditor.ViewModeProperty);
    set => this.SetValue(PdfEditor.ViewModeProperty, (object) value);
  }

  public Color PageSeparatorColor
  {
    get => (Color) this.GetValue(PdfEditor.PageSeparatorColorProperty);
    set => this.SetValue(PdfEditor.PageSeparatorColorProperty, (object) value);
  }

  public bool ShowPageSeparator
  {
    get => (bool) this.GetValue(PdfEditor.ShowPageSeparatorProperty);
    set => this.SetValue(PdfEditor.ShowPageSeparatorProperty, (object) value);
  }

  public Color CurrentPageHighlightColor
  {
    get => (Color) this.GetValue(PdfEditor.CurrentPageHighlightColorProperty);
    set => this.SetValue(PdfEditor.CurrentPageHighlightColorProperty, (object) value);
  }

  public bool ShowCurrentPageHighlight
  {
    get => (bool) this.GetValue(PdfEditor.ShowCurrentPageHighlightProperty);
    set => this.SetValue(PdfEditor.ShowCurrentPageHighlightProperty, (object) value);
  }

  public VerticalAlignment PageVAlign
  {
    get => (VerticalAlignment) this.GetValue(PdfEditor.PageVAlignProperty);
    set => this.SetValue(PdfEditor.PageVAlignProperty, (object) value);
  }

  public HorizontalAlignment PageHAlign
  {
    get => (HorizontalAlignment) this.GetValue(PdfEditor.PageHAlignProperty);
    set => this.SetValue(PdfEditor.PageHAlignProperty, (object) value);
  }

  public RenderFlags RenderFlags
  {
    get => (RenderFlags) this.GetValue(PdfEditor.RenderFlagsProperty);
    set => this.SetValue(PdfEditor.RenderFlagsProperty, (object) value);
  }

  public int TilesCount
  {
    get => (int) this.GetValue(PdfEditor.TilesCountProperty);
    set => this.SetValue(PdfEditor.TilesCountProperty, (object) value);
  }

  public bool ShowLoadingIcon
  {
    get => (bool) this.GetValue(PdfEditor.ShowLoadingIconProperty);
    set => this.SetValue(PdfEditor.ShowLoadingIconProperty, (object) value);
  }

  public bool UseProgressiveRender
  {
    get => (bool) this.GetValue(PdfEditor.UseProgressiveRenderProperty);
    set => this.SetValue(PdfEditor.UseProgressiveRenderProperty, (object) value);
  }

  public string LoadingIconText
  {
    get => (string) this.GetValue(PdfEditor.LoadingIconTextProperty);
    set => this.SetValue(PdfEditor.LoadingIconTextProperty, (object) value);
  }

  public bool PageAutoDispose
  {
    get => (bool) this.GetValue(PdfEditor.PageAutoDisposeProperty);
    set => this.SetValue(PdfEditor.PageAutoDisposeProperty, (object) value);
  }

  public int OptimizedLoadThreshold
  {
    get => (int) this.GetValue(PdfEditor.OptimizedLoadThresholdProperty);
    set => this.SetValue(PdfEditor.OptimizedLoadThresholdProperty, (object) value);
  }

  public double FlyoutExtentWidth
  {
    get => (double) this.GetValue(PdfEditor.FlyoutExtentWidthProperty);
    set => this.SetValue(PdfEditor.FlyoutExtentWidthProperty, (object) value);
  }

  public bool IsAnnotationVisible
  {
    get => (bool) this.GetValue(PdfEditor.IsAnnotationVisibleProperty);
    set => this.SetValue(PdfEditor.IsAnnotationVisibleProperty, (object) value);
  }

  public Brush PageMaskBrush
  {
    get => (Brush) this.GetValue(PdfEditor.PageMaskBrushProperty);
    set => this.SetValue(PdfEditor.PageMaskBrushProperty, (object) value);
  }

  public bool IsRenderPaused
  {
    get => (bool) this.GetValue(PdfEditor.IsRenderPausedProperty);
    set => this.SetValue(PdfEditor.IsRenderPausedProperty, (object) value);
  }

  public EditorMouseModes MouseMode
  {
    get => (EditorMouseModes) this.GetValue(PdfEditor.MouseModeProperty);
    set => this.SetValue(PdfEditor.MouseModeProperty, (object) value);
  }

  public PdfForms FillForms
  {
    get
    {
      return this.Document == null || this.Document.FormFill == null ? this._fillForms : this.Document.FormFill;
    }
  }

  public int CurrentIndex
  {
    get => this.Document == null ? -1 : this.Document.Pages.CurrentIndex;
    set
    {
      if (this.Document == null)
        return;
      this.Document.Pages.CurrentIndex = value;
    }
  }

  public PdfPage CurrentPage => this.Document.Pages.CurrentPage;

  public TextProperties TextProperties => this.textProperties;

  public void ScrollToPage(int index)
  {
    if (this.Document == null || this.Document.Pages.Count == 0 || index < 0 || index > this.Document.Pages.Count - 1)
      return;
    if (this.ViewMode == ViewModes.SinglePage || this.ViewMode == ViewModes.TilesLine)
    {
      if (index != this.CurrentIndex)
      {
        this.SetCurrentPage(index);
        this._prPages.ReleaseCanvas();
      }
      this.InvalidateVisual();
    }
    else
    {
      Rect rect = this.renderRects(index);
      if (rect.Width == 0.0 || rect.Height == 0.0)
        return;
      this.SetVerticalOffset(rect.Y);
      this.SetHorizontalOffset(rect.X);
    }
  }

  public void ScrollToChar(int charIndex) => this.ScrollToChar(this.CurrentIndex, charIndex);

  public void ScrollToChar(int pageIndex, int charIndex)
  {
    if (this.Document == null || this.Document.Pages.Count == 0 || pageIndex < 0)
      return;
    PdfPage page = this.Document.Pages[pageIndex];
    int countChars = page.Text.CountChars;
    if (charIndex < 0)
      charIndex = 0;
    if (charIndex >= countChars)
      charIndex = countChars - 1;
    if (charIndex < 0)
      return;
    PdfTextInfo textInfo = page.Text.GetTextInfo(charIndex, 1);
    if (textInfo.Rects == null || textInfo.Rects.Count == 0)
      return;
    this.ScrollToPage(pageIndex);
    Point client = this.PageToClient(pageIndex, new Point((double) textInfo.Rects[0].left, (double) textInfo.Rects[0].top));
    Point autoScrollPosition = this._autoScrollPosition;
    this.SetVerticalOffset(client.Y - autoScrollPosition.Y);
    this.SetHorizontalOffset(client.X - autoScrollPosition.X);
  }

  public void ScrollToPoint(int pageIndex, Point pagePoint)
  {
    if (this.Document == null)
      return;
    int count = this.Document.Pages.Count;
    if (count == 0 || pageIndex < 0 || pageIndex > count - 1)
      return;
    this.ScrollToPage(pageIndex);
    Point client = this.PageToClient(pageIndex, pagePoint);
    Point autoScrollPosition = this._autoScrollPosition;
    this.SetVerticalOffset(client.Y - autoScrollPosition.Y);
    this.SetHorizontalOffset(client.X - autoScrollPosition.X);
  }

  public void RotatePage(int pageIndex, PageRotate angle)
  {
    if (this.Document == null)
      return;
    this.Document.Pages[pageIndex].Rotation = angle;
    this.UpdateDocLayout();
  }

  public int PointInPage(Point pt)
  {
    for (int startPage = this._startPage; startPage <= this._endPage; ++startPage)
    {
      if (this.CalcActualRect(startPage).Contains(pt))
        return startPage;
    }
    return -1;
  }

  public Point ClientToPage(int pageIndex, Point pt)
  {
    if (pageIndex < this._startPage || pageIndex > this._endPage)
      throw new IndexOutOfRangeException(nameof (pageIndex));
    PdfPage page = this.Document.Pages[pageIndex];
    Rect rect = this.CalcActualRect(pageIndex);
    double pageX;
    double pageY;
    page.DeviceToPage(Helpers.UnitsToPixels((DependencyObject) this, rect.X), Helpers.UnitsToPixels((DependencyObject) this, rect.Y), Helpers.UnitsToPixels((DependencyObject) this, rect.Width), Helpers.UnitsToPixels((DependencyObject) this, rect.Height), this.PageRotation(page), Helpers.UnitsToPixels((DependencyObject) this, pt.X), Helpers.UnitsToPixels((DependencyObject) this, pt.Y), out pageX, out pageY);
    return new Point(pageX, pageY);
  }

  public Point PageToClient(int pageIndex, Point pt)
  {
    if (pageIndex < this._startPage || pageIndex > this._endPage)
      throw new IndexOutOfRangeException(nameof (pageIndex));
    PdfPage page = this.Document.Pages[pageIndex];
    Rect rect = this.CalcActualRect(pageIndex);
    int deviceX;
    int deviceY;
    page.PageToDevice(Helpers.UnitsToPixels((DependencyObject) this, rect.X), Helpers.UnitsToPixels((DependencyObject) this, rect.Y), Helpers.UnitsToPixels((DependencyObject) this, rect.Width), Helpers.UnitsToPixels((DependencyObject) this, rect.Height), this.PageRotation(page), (float) pt.X, (float) pt.Y, out deviceX, out deviceY);
    return new Point(Helpers.PixelsToUnits((DependencyObject) this, deviceX), Helpers.PixelsToUnits((DependencyObject) this, deviceY));
  }

  public FS_RECTF ClientToPageRect(Rect rect, int pageIndex)
  {
    Point page1 = this.ClientToPage(pageIndex, new Point(rect.Left, rect.Top));
    Point page2 = this.ClientToPage(pageIndex, new Point(rect.Right, rect.Bottom));
    return new FS_RECTF(page1.X < page2.X ? (float) page1.X : (float) page2.X, page1.Y > page2.Y ? (float) page1.Y : (float) page2.Y, page1.X > page2.X ? (float) page1.X : (float) page2.X, page1.Y < page2.Y ? (float) page1.Y : (float) page2.Y);
  }

  public Rect PageToClientRect(FS_RECTF rc, int pageIndex)
  {
    Point device1 = this.PageToDevice((double) rc.left, (double) rc.top, pageIndex);
    Point device2 = this.PageToDevice((double) rc.right, (double) rc.bottom, pageIndex);
    return new Rect(device1.X < device2.X ? device1.X : device2.X, device1.Y < device2.Y ? device1.Y : device2.Y, device1.X > device2.X ? device1.X - device2.X : device2.X - device1.X, device1.Y > device2.Y ? device1.Y - device2.Y : device2.Y - device1.Y);
  }

  public void UpdateDocLayout()
  {
    this._prPages.ReleaseCanvas();
    this._viewport = new Size(this.ActualWidth, this.ActualHeight);
    if (this.Document == null || this.Document.Pages.Count <= 0 || this.ActualWidth < 1E-05 || this.ActualHeight < 1E-05)
    {
      this._renderRects = (RenderRect[]) null;
      this.InvalidateVisual();
    }
    else
    {
      this.SaveScrollPoint();
      this._renderRects = new RenderRect[this.Document.Pages.Count];
      this.CalcPages();
      this.RestoreScrollPoint();
      this.InvalidateVisual();
      if (this.ScrollOwner == null)
        return;
      this.ScrollOwner.InvalidateScrollInfo();
    }
  }

  public void ClearRenderBuffer() => this._prPages.ReleaseCanvas();

  public Rect CalcActualRect(int index)
  {
    if (this._renderRects == null)
      return new Rect();
    Rect rect = this.renderRects(index);
    rect.X += this._autoScrollPosition.X;
    rect.Y += this._autoScrollPosition.Y;
    return rect;
  }

  public bool DeleteSelectedParagraph()
  {
    IPdfParagraph caretParagraph = this.GetCaretParagraph();
    IPdfUndoItem undoItem;
    if (caretParagraph != null && this.analysers[this.caretInfo.PageIndex].DeleteParagraph(caretParagraph, true, out undoItem))
    {
      this.operationManager.AddUndoItem(undoItem);
      this.ForceRender();
      return true;
    }
    this.InvalidateVisual();
    return false;
  }

  public int[] GetChangedPageIndexes()
  {
    return this.Document == null || this.Document.IsDisposed ? Array.Empty<int>() : this.operationManager.GetUndoItemPageIndexes(false);
  }

  public void LoadDocument(string path, string password = null)
  {
    PdfDocument pdfDocument = (PdfDocument) null;
    try
    {
      this.CloseDocument();
      if (this.Document != null)
        return;
      this.Document = pdfDocument = PdfDocument.Load(path, this._fillForms, password);
      if (this.Document == null)
        return;
      this._loadedByViewer = true;
      this.OnDocumentLoaded(EventArgs.Empty);
    }
    catch (NoLicenseException ex)
    {
    }
    finally
    {
      if (this.Document == null && pdfDocument != null)
        pdfDocument.Dispose();
    }
  }

  public void LoadDocument(Stream stream, string password = null)
  {
    PdfDocument pdfDocument = (PdfDocument) null;
    try
    {
      this.CloseDocument();
      if (this.Document != null)
        return;
      this.Document = pdfDocument = PdfDocument.Load(stream, this._fillForms, password);
      if (this.Document == null)
        return;
      this._loadedByViewer = true;
      this.OnDocumentLoaded(EventArgs.Empty);
    }
    catch (NoLicenseException ex)
    {
    }
    finally
    {
      if (this.Document == null && pdfDocument != null)
        pdfDocument.Dispose();
    }
  }

  public void LoadDocument(byte[] pdf, string password = null)
  {
    PdfDocument pdfDocument = (PdfDocument) null;
    try
    {
      this.CloseDocument();
      if (this.Document != null)
        return;
      this.Document = pdfDocument = PdfDocument.Load(pdf, this._fillForms, password);
      if (this.Document == null)
        return;
      this._loadedByViewer = true;
      this.OnDocumentLoaded(EventArgs.Empty);
    }
    catch (NoLicenseException ex)
    {
    }
    finally
    {
      if (this.Document == null && pdfDocument != null)
        pdfDocument.Dispose();
    }
  }

  public void CloseDocument() => this.Document = (PdfDocument) null;

  static PdfEditor()
  {
    Style defaultValue = new Style();
    defaultValue.Setters.Add((SetterBase) new Setter(Control.TemplateProperty, (object) new ControlTemplate()));
    defaultValue.Seal();
    FrameworkElement.FocusVisualStyleProperty.OverrideMetadata(typeof (PdfEditor), (PropertyMetadata) new FrameworkPropertyMetadata((object) defaultValue));
    UIElement.FocusableProperty.OverrideMetadata(typeof (PdfEditor), (PropertyMetadata) new FrameworkPropertyMetadata((object) true));
  }

  public PdfEditor()
  {
    this.LoadingIconText = "Loading";
    this.Background = (Brush) SystemColors.ControlDarkBrush;
    this._prPages.PaintBackground += (EventHandler<EventArgs<Int32Rect>>) ((s, e) =>
    {
      PdfBitmap canvasBitmap = this._prPages.CanvasBitmap;
      Int32Rect int32Rect = e.Value;
      int x = int32Rect.X;
      int32Rect = e.Value;
      int y = int32Rect.Y;
      int32Rect = e.Value;
      int width = int32Rect.Width;
      int32Rect = e.Value;
      int height = int32Rect.Height;
      this.DrawPageBackColor(canvasBitmap, x, y, width, height);
    });
    this._fillForms = new PdfForms();
    this.CaptureFillForms(this._fillForms);
    this.caretInfo = new CaretInfo(this);
    this.keyboardProcessor = new PdfEditorKeyboardProcessor(this);
    this.textProperties = new TextProperties(this);
    this.operationManager = new OperationManager(this);
    this.Loaded += (RoutedEventHandler) ((s, a) =>
    {
      this.mouseWheelHelper?.Dispose();
      this.mouseWheelHelper = new MouseWheelHelper((Visual) this);
      this.mouseWheelHelper.Throttled = true;
      this.mouseWheelHelper.MouseTilt += new EventHandler<MouseTiltEventArgs>(this.MouseWheelHelper_MouseTilt);
      HwndSource hwndSource1 = (HwndSource) PresentationSource.FromVisual((Visual) this);
      if (hwndSource1 == null)
      {
        Window window = Window.GetWindow((DependencyObject) this);
        if (window != null)
        {
          EventHandler sourceInitializedFunc = (EventHandler) null;
          sourceInitializedFunc = (EventHandler) ((_s, _a) =>
          {
            ((Window) _s).SourceInitialized -= sourceInitializedFunc;
            HwndSource hwndSource2 = (HwndSource) PresentationSource.FromVisual((Visual) this);
            if (hwndSource2 == null || !((FrameworkElement) _s).IsLoaded || Window.GetWindow((DependencyObject) this) != _s)
              return;
            this.InitializeImeReceiver(hwndSource2);
          });
          window.SourceInitialized += sourceInitializedFunc;
        }
      }
      else
        this.InitializeImeReceiver(hwndSource1);
      Window target;
      if (this.window != null && this.window.TryGetTarget(out target))
        target.PreviewKeyDown -= new KeyEventHandler(this.Global_PreviewKeyDown);
      this.window = (WeakReference<Window>) null;
      target = Window.GetWindow((DependencyObject) this);
      if (target == null)
        return;
      this.window = new WeakReference<Window>(target);
      target.PreviewKeyDown += new KeyEventHandler(this.Global_PreviewKeyDown);
    });
    this.Unloaded += (RoutedEventHandler) ((s, a) =>
    {
      this.ReleaseImeReceiver();
      this.mouseWheelHelper.MouseTilt -= new EventHandler<MouseTiltEventArgs>(this.MouseWheelHelper_MouseTilt);
      this.mouseWheelHelper?.Dispose();
      this.mouseWheelHelper = (MouseWheelHelper) null;
      Window target;
      if (this.window == null || !this.window.TryGetTarget(out target))
        return;
      target.PreviewKeyDown -= new KeyEventHandler(this.Global_PreviewKeyDown);
    });
  }

  private IntPtr ParentWindowProc(
    IntPtr hwnd,
    int msg,
    IntPtr wParam,
    IntPtr lParam,
    ref bool handled)
  {
    return this.FilterImeMessage(hwnd, msg, wParam, lParam, ref handled);
  }

  private IntPtr FilterImeMessage(
    IntPtr hwnd,
    int msg,
    IntPtr wParam,
    IntPtr lParam,
    ref bool handled)
  {
    if (this.imeReceiver != null && this.IsKeyboardFocused)
    {
      switch (msg)
      {
        case 81:
          this.imeReceiver.ProcessInputLangChangedMessage();
          break;
        case 270:
          this.imeReceiver.CompleteImeCharMessage();
          this.imeProcessing = false;
          break;
        case 271:
          this.imeProcessing = true;
          this.imeReceiver.ProcessImeCompositionMessage();
          break;
        case 646:
          handled = true;
          this.imeReceiver.ProcessImeCharMessage(wParam, lParam);
          break;
      }
    }
    return IntPtr.Zero;
  }

  private void InitializeImeReceiver(HwndSource hwndSource)
  {
    this.ReleaseImeReceiver();
    this.hwndSource = hwndSource;
    if (hwndSource != null)
    {
      this.hwndSourceHook = new HwndSourceHook(this.ParentWindowProc);
      hwndSource.AddHook(this.hwndSourceHook);
      this.imeReceiver = new ImeReceiver(hwndSource.Handle);
      this.imeReceiver.RefreshInputMethodRequested += new EventHandler(this.ImeReceiver_RefreshInputMethodRequested);
      this.imeReceiver.ImeRequested += new ImeReceiverImeRequestedEventHandler(this.ImeReceiver_ImeRequested);
      this.imeReceiver.TextReveived += new ImeReceiverTextReceivedEventHandler(this.ImeReceiver_TextReveived);
    }
    this.UpdateInputMethods();
    this.UpdateImeState(true);
  }

  private void ReleaseImeReceiver()
  {
    this.UpdateImeState(false);
    if (this.hwndSource != null && this.hwndSourceHook != null)
    {
      this.hwndSource.RemoveHook(this.hwndSourceHook);
      this.hwndSource = (HwndSource) null;
    }
    if (this.imeReceiver == null)
      return;
    this.imeReceiver.RefreshInputMethodRequested -= new EventHandler(this.ImeReceiver_RefreshInputMethodRequested);
    this.imeReceiver.ImeRequested -= new ImeReceiverImeRequestedEventHandler(this.ImeReceiver_ImeRequested);
    this.imeReceiver.TextReveived -= new ImeReceiverTextReceivedEventHandler(this.ImeReceiver_TextReveived);
    this.imeReceiver?.Dispose();
    this.imeReceiver = (ImeReceiver) null;
  }

  private void ImeReceiver_RefreshInputMethodRequested(object sender, EventArgs e)
  {
    this.UpdateInputMethods();
  }

  private void ImeReceiver_ImeRequested(ImeReceiver sender, ImeReceiverImeRequestedEventArgs args)
  {
    IPdfParagraph caretParagraph = this.GetCaretParagraph();
    FS_POINTF bottom;
    if (caretParagraph != null && caretParagraph.GetCaretPos(this.caretInfo.Caret, out FS_POINTF _, out bottom))
    {
      Point clientPoint;
      this.TryGetClientPoint(this.caretInfo.PageIndex, bottom.ToPoint(), out clientPoint);
      Point point = this.TransformToVisual(this.hwndSource.RootVisual).Transform(clientPoint);
      double pixelsPerDip = VisualTreeHelper.GetDpi((Visual) this).PixelsPerDip;
      args.CaretPointLeftInPixel = (int) (point.X * pixelsPerDip / SystemParameters.CaretWidth) + 2;
      args.CaretPointTopInPixel = (int) (point.Y * pixelsPerDip / SystemParameters.CaretWidth - 24.0 - 3.0);
      args.FontSize = 18.0;
    }
    else
      this.Dispatcher.InvokeAsync((Action) (() => this.UpdateImeState(false)), DispatcherPriority.Normal);
  }

  private void ImeReceiver_TextReveived(ImeReceiver sender, ImeReceiverTextReceivedEventArgs args)
  {
    Common.WriteLog("keyboardProcessor.InsertText");
    try
    {
      this.keyboardProcessor.InsertText(args.Text);
    }
    catch (Exception ex)
    {
      Common.WriteLog(ex.ToString());
    }
  }

  internal void ForceRender()
  {
    this.cachedCanvasBitmap?.Dispose();
    this.cachedCanvasBitmap = (CachedCanvasBitmap) null;
    this._prPages?.ReleaseCanvas();
    this.InvalidateVisual();
  }

  private void UpdateInputMethods()
  {
    if (InputMethod.GetIsInputMethodEnabled((DependencyObject) this))
      InputMethod.SetIsInputMethodEnabled((DependencyObject) this, false);
    if (InputMethod.GetIsInputMethodSuspended((DependencyObject) this))
      InputMethod.SetIsInputMethodSuspended((DependencyObject) this, false);
    InputMethod.SetIsInputMethodEnabled((DependencyObject) this, true);
    InputMethod.SetIsInputMethodSuspended((DependencyObject) this, true);
  }

  private void UpdateImeState(bool enable)
  {
    if (enable && this.GetCaretParagraph() == null)
      enable = false;
    if (enable)
      return;
    this.imeReceiver?.ProcessLostKeyboardFocus(true);
    this.imeProcessing = false;
  }

  protected override int VisualChildrenCount => 0;

  protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
  {
    this.UpdateDocLayout();
    base.OnRenderSizeChanged(sizeInfo);
  }

  protected override void OnMouseLeave(MouseEventArgs e) => base.OnMouseLeave(e);

  protected override void OnRender(DrawingContext drawingContext)
  {
    Helpers.FillRectangle(drawingContext, this.Background, this.ClientRect);
    if (this.IsRenderPaused || this.Document == null || this._renderRects == null)
      return;
    List<Point> separator = new List<Point>();
    Rect clientRect = this.ClientRect;
    int pixels1 = Helpers.UnitsToPixels((DependencyObject) this, clientRect.Width);
    clientRect = this.ClientRect;
    int pixels2 = Helpers.UnitsToPixels((DependencyObject) this, clientRect.Height);
    if (pixels1 <= 0 || pixels2 <= 0)
      return;
    this._prPages.InitCanvas(new Helpers.Int32Size(pixels1, pixels2));
    bool allPagesAreRendered = true;
    bool flag1 = false;
    bool[] flagArray = new bool[this._endPage + 1];
    PdfBitmap pdfBitmap = (PdfBitmap) null;
    Int32Rect[] pageRects = new Int32Rect[this._endPage - this._startPage + 1];
    for (int startPage = this._startPage; startPage <= this._endPage; ++startPage)
    {
      Rect actualRect = this.CalcActualRect(startPage);
      if (!actualRect.IntersectsWith(this.ClientRect))
      {
        PdfPage page = this.Document.Pages[startPage];
        if (page.IsLoaded && this.PageAutoDispose && this.CanDisposePage(startPage))
        {
          PageDisposeHelper.DisposePage(page);
          this.analysers[startPage] = (LogicalStructAnalyser) null;
        }
      }
      else
      {
        if (PageDisposeHelper.TryFixPageAnnotations(this.Document, startPage))
          this.Document.Pages[startPage].ReloadPage();
        if (!this._renderRects[startPage].IsChecked)
        {
          this.SaveScrollPoint();
          this.CalcPages();
          this.RestoreScrollPoint();
          actualRect = this.CalcActualRect(startPage);
        }
        if (this._prPages.CanvasBitmap == null)
          this._prPages.InitCanvas(new Helpers.Int32Size(pixels1, pixels2));
        bool flag2 = this.DrawPage(this.Document.Pages[startPage], actualRect);
        allPagesAreRendered &= flag2;
        flag1 |= !flag2;
        flagArray[startPage] = !flag2;
        if (pdfBitmap == null)
          pdfBitmap = new PdfBitmap(this._prPages.CanvasSize.Width, this._prPages.CanvasSize.Height, true);
        int pixels3 = Helpers.UnitsToPixels((DependencyObject) this, actualRect.X);
        int pixels4 = Helpers.UnitsToPixels((DependencyObject) this, actualRect.Y);
        int pixels5 = Helpers.UnitsToPixels((DependencyObject) this, actualRect.Width);
        int pixels6 = Helpers.UnitsToPixels((DependencyObject) this, actualRect.Height);
        Pdfium.FPDFBitmap_CompositeBitmap(pdfBitmap.Handle, pixels3, pixels4, pixels5, pixels6, this._prPages.CanvasBitmap.Handle, pixels3, pixels4);
        if (allPagesAreRendered)
          pageRects[startPage - this._startPage] = new Int32Rect(pixels3, pixels4, pixels5, pixels6);
        if (flag2)
        {
          this.DrawFillForms(pdfBitmap, this.Document.Pages[startPage], actualRect);
          this.DrawFillFormsSelection(pdfBitmap, this._selectedRectangles);
          this.DrawTextSelection(pdfBitmap, startPage);
        }
        this.CalcPageSeparator(actualRect, startPage, ref separator);
      }
    }
    if (allPagesAreRendered)
    {
      this.cachedCanvasBitmap?.Dispose();
      this.cachedCanvasBitmap = (CachedCanvasBitmap) null;
      if (this._prPages?.CanvasBitmap != null)
        this.cachedCanvasBitmap = new CachedCanvasBitmap(this._prPages.CanvasBitmap, (IPdfScrollInfo) this, pageRects);
    }
    else if (this.cachedCanvasBitmap != null && this.cachedCanvasBitmap.Zoom != (double) this.Zoom)
    {
      this.cachedCanvasBitmap?.Dispose();
      this.cachedCanvasBitmap = (CachedCanvasBitmap) null;
    }
    this.DrawRenderedPagesToDevice(drawingContext, this._prPages.CanvasBitmap, pdfBitmap, this._prPages.CanvasSize.Width, this._prPages.CanvasSize.Height, allPagesAreRendered);
    pdfBitmap?.Dispose();
    this.DrawPageSeparators(drawingContext, ref separator);
    for (int startPage = this._startPage; startPage <= this._endPage; ++startPage)
    {
      Rect rect = this.CalcActualRect(startPage);
      if (rect.IntersectsWith(this.ClientRect))
      {
        if (this.ShowLoadingIcon && flagArray[startPage])
          this.DrawLoadingIcon(drawingContext, this.Document.Pages[startPage], rect);
        this.DrawPageBorder(drawingContext, rect);
        this.DrawCurrentPageHighlight(drawingContext, startPage, rect);
        this.DrawParagraphBorder(drawingContext, startPage, rect);
        this.DrawCaret(drawingContext, rect);
        this.DrawPageMask(drawingContext, startPage, rect);
      }
    }
    if (!allPagesAreRendered || this.IsCaretVisible())
      this.StartInvalidateTimer(!allPagesAreRendered);
    else if ((this.RenderFlags & RenderFlags.FPDF_THUMBNAIL) != 0)
      this._prPages.ReleaseCanvas();
    else if (!this.UseProgressiveRender)
      this._prPages.ReleaseCanvas();
    this._selectedRectangles.Clear();
  }

  protected override void OnMouseDoubleClick(MouseButtonEventArgs e) => base.OnMouseDoubleClick(e);

  protected override void OnMouseDown(MouseButtonEventArgs e)
  {
    if (!this.IsFocused)
    {
      this._isProgrammaticallyFocusSetted = true;
      this.Focus();
    }
    if (!e.Handled && e.ChangedButton == MouseButton.Left && this.Document != null)
    {
      Point position = e.GetPosition((IInputElement) this);
      Point pagePoint;
      int page = this.DeviceToPage(position.X, position.Y, out pagePoint);
      Point page_point = pagePoint;
      if (page >= 0)
      {
        this.SetCurrentPage(page);
        this._lastPressedTime = new DateTime?(DateTime.Now);
        if (this.MouseMode == EditorMouseModes.Default)
          this.ProcessMouseDownForSelectTextTool(page_point, page);
        else if (this.MouseMode == EditorMouseModes.SelectParagraph)
          this.ProcessMouseDownForSelectParagraph(page_point, page);
        this.InvalidateVisual();
        e.Handled = true;
      }
    }
    base.OnMouseDown(e);
  }

  protected override void OnMouseMove(MouseEventArgs e)
  {
    if (this.Document != null)
    {
      Point position = e.GetPosition((IInputElement) this);
      Point pagePoint;
      int page = this.DeviceToPage(position.X, position.Y, out pagePoint);
      if (page >= 0)
      {
        CursorTypes cursor = CursorTypes.Arrow;
        bool panTool = false;
        if (this.MouseMode == EditorMouseModes.Default)
          cursor = this.ProcessMouseMoveForSelectTextTool(pagePoint, page);
        else if (this.MouseMode == EditorMouseModes.SelectParagraph)
          cursor = this.ProcessMouseMoveForSelectParagraph(pagePoint, page);
        this.InternalSetCursor(cursor, panTool);
      }
      else
        this.Cursor = (Cursor) null;
    }
    base.OnMouseMove(e);
  }

  protected override void OnMouseUp(MouseButtonEventArgs e)
  {
    if (!this.IsFocused)
    {
      this._isProgrammaticallyFocusSetted = true;
      Keyboard.Focus((IInputElement) this);
    }
    try
    {
      DateTime? lastPressedTime = this._lastPressedTime;
      this._lastPressedTime = new DateTime?();
      if (this.Document != null)
      {
        Point position = e.GetPosition((IInputElement) this);
        Point pagePoint;
        int page = this.DeviceToPage(position.X, position.Y, out pagePoint);
        if (page >= 0)
        {
          if (this.MouseMode == EditorMouseModes.SelectParagraph && e.ChangedButton == MouseButton.Left)
          {
            DateTime now = DateTime.Now;
            TimeSpan timeSpan;
            int num1;
            if (lastPressedTime.HasValue)
            {
              timeSpan = now - lastPressedTime.Value;
              num1 = timeSpan.TotalSeconds < 0.5 ? 1 : 0;
            }
            else
              num1 = 0;
            bool flag1 = num1 != 0;
            int num2;
            if (lastPressedTime.HasValue)
            {
              timeSpan = now - lastPressedTime.Value;
              num2 = timeSpan.TotalSeconds > 1.0 ? 1 : 0;
            }
            else
              num2 = 0;
            bool flag2 = num2 != 0;
            bool flag3 = false;
            bool flag4 = true;
            if (this.selectParagraphMouseDownPoint.HasValue && this.selectParagraphMouseCurPoint.HasValue)
            {
              FS_POINTF fsPointf = this.selectParagraphMouseDownPoint.Value;
              double x1 = (double) fsPointf.X;
              fsPointf = this.selectParagraphMouseCurPoint.Value;
              double x2 = (double) fsPointf.X;
              float num3 = Math.Abs((float) (x1 - x2));
              fsPointf = this.selectParagraphMouseDownPoint.Value;
              double y1 = (double) fsPointf.Y;
              fsPointf = this.selectParagraphMouseCurPoint.Value;
              double y2 = (double) fsPointf.Y;
              float num4 = Math.Abs((float) (y1 - y2));
              if ((double) num3 > 5.0 || (double) num4 > 5.0)
                flag4 = false;
              if ((double) num3 >= 10.0 || (double) num4 >= 10.0)
                flag3 = true;
            }
            if (((!flag1 ? 0 : (!flag2 ? 1 : 0)) & (flag4 ? 1 : 0)) != 0)
            {
              this.selectParagraphMouseDownPoint = new FS_POINTF?();
              this.ProcessMouseUpForSelectParagraph(pagePoint, page);
              this.ProcessMouseDoubleClickForSelectTextTool(pagePoint, page);
            }
            else
              this.ProcessMouseUpForSelectParagraph(pagePoint, page);
          }
          else if (e.ChangedButton == MouseButton.Right)
          {
            this.selectParagraphMouseDownPoint = new FS_POINTF?();
            this.selectParagraphMouseCurPoint = new FS_POINTF?();
            this.Cursor = (Cursor) null;
            this.InvalidateVisual();
          }
        }
        else
        {
          this.selectParagraphMouseDownPoint = new FS_POINTF?();
          this.selectParagraphMouseCurPoint = new FS_POINTF?();
          this.Document?.FormFill?.ForceToKillFocus();
          this.Cursor = (Cursor) null;
          this.InvalidateVisual();
        }
      }
    }
    finally
    {
      this.ReleaseMouseCapture();
    }
    base.OnMouseUp(e);
  }

  protected override void OnLostMouseCapture(MouseEventArgs e)
  {
    base.OnLostMouseCapture(e);
    this.selectParagraphMouseCurPoint = new FS_POINTF?();
    this.selectParagraphMouseDownPoint = new FS_POINTF?();
  }

  protected override void OnKeyDown(KeyEventArgs e)
  {
    if (this.Document != null && !e.Handled)
      ScrollInDirection(e);
    base.OnKeyDown(e);

    void ScrollInDirection(KeyEventArgs args)
    {
      bool flag1 = (args.KeyboardDevice.Modifiers & ModifierKeys.Control) != 0;
      if ((args.KeyboardDevice.Modifiers & ModifierKeys.Alt) != 0)
        return;
      bool flag2 = this.FlowDirection == FlowDirection.RightToLeft;
      args.Handled = true;
      switch (args.Key)
      {
        case Key.Prior:
          this.PageUp();
          break;
        case Key.Next:
          this.PageDown();
          break;
        case Key.End:
          if (flag1)
          {
            ScrollViewer scrollOwner = this.ScrollOwner;
            if (scrollOwner != null)
            {
              scrollOwner.ScrollToBottom();
              break;
            }
            break;
          }
          ScrollViewer scrollOwner1 = this.ScrollOwner;
          if (scrollOwner1 != null)
          {
            scrollOwner1.ScrollToRightEnd();
            break;
          }
          break;
        case Key.Home:
          if (flag1)
          {
            ScrollViewer scrollOwner2 = this.ScrollOwner;
            if (scrollOwner2 != null)
            {
              scrollOwner2.ScrollToTop();
              break;
            }
            break;
          }
          ScrollViewer scrollOwner3 = this.ScrollOwner;
          if (scrollOwner3 != null)
          {
            scrollOwner3.ScrollToLeftEnd();
            break;
          }
          break;
        case Key.Left:
          if (flag2)
          {
            this.LineRight();
            break;
          }
          this.LineLeft();
          break;
        case Key.Up:
          this.LineUp();
          break;
        case Key.Right:
          if (flag2)
          {
            this.LineLeft();
            break;
          }
          this.LineRight();
          break;
        case Key.Down:
          this.LineDown();
          break;
        default:
          args.Handled = false;
          break;
      }
    }
  }

  protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
  {
    base.OnGotKeyboardFocus(e);
    if (e.NewFocus != this)
      return;
    if (this.GetCaretParagraph() != null)
      this.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) (() =>
      {
        this.UpdateImeState(false);
        this.imeReceiver?.ProcessGotKeyboardFocus();
        this.ForceRender();
      }));
    else
      this.ForceRender();
  }

  protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
  {
    base.OnLostKeyboardFocus(e);
    if (e.NewFocus == this)
      return;
    this.UpdateImeState(false);
  }

  protected virtual void DrawPageBackColor(PdfBitmap bitmap, int x, int y, int width, int height)
  {
    bitmap.FillRectEx(x, y, width, height, Helpers.ToArgb(this.PageBackColor));
  }

  protected virtual bool DrawPage(PdfPage page, Rect actualRect)
  {
    if (actualRect.Width <= 0.0 || actualRect.Height <= 0.0)
      return true;
    int pixels1 = Helpers.UnitsToPixels((DependencyObject) this, actualRect.Width);
    int pixels2 = Helpers.UnitsToPixels((DependencyObject) this, actualRect.Height);
    if (pixels1 <= 0 || pixels2 <= 0)
      return true;
    Int32Rect pageRect = new Int32Rect(Helpers.UnitsToPixels((DependencyObject) this, actualRect.X), Helpers.UnitsToPixels((DependencyObject) this, actualRect.Y), pixels1, pixels2);
    lock (page)
    {
      using (PdfRenderHelper.CreateHideFlagContext(page, this.IsAnnotationVisible))
        return this._prPages.RenderPage(page, pageRect, this.PageRotation(page), this.RenderFlags, this.UseProgressiveRender);
    }
  }

  protected virtual void DrawFillForms(PdfBitmap bitmap, PdfPage page, Rect actualRect)
  {
    int pixels1 = Helpers.UnitsToPixels((DependencyObject) this, actualRect.X);
    int pixels2 = Helpers.UnitsToPixels((DependencyObject) this, actualRect.Y);
    int pixels3 = Helpers.UnitsToPixels((DependencyObject) this, actualRect.Width);
    int pixels4 = Helpers.UnitsToPixels((DependencyObject) this, actualRect.Height);
    lock (page)
    {
      using (PdfRenderHelper.CreateHideFlagContext(page, this.IsAnnotationVisible))
        page.RenderForms(bitmap, pixels1, pixels2, pixels3, pixels4, this.PageRotation(page), this.RenderFlags);
    }
  }

  protected virtual void DrawFillFormsSelection(PdfBitmap bitmap, List<Rect> selectedRectangles)
  {
    if (selectedRectangles == null)
      return;
    foreach (Rect selectedRectangle in selectedRectangles)
    {
      int pixels1 = Helpers.UnitsToPixels((DependencyObject) this, selectedRectangle.X);
      int pixels2 = Helpers.UnitsToPixels((DependencyObject) this, selectedRectangle.Y);
      int pixels3 = Helpers.UnitsToPixels((DependencyObject) this, selectedRectangle.Width);
      int pixels4 = Helpers.UnitsToPixels((DependencyObject) this, selectedRectangle.Height);
      bitmap.FillRectEx(pixels1, pixels2, pixels3, pixels4, Helpers.ToArgb(this.TextSelectColor), this.FormsBlendMode);
    }
  }

  protected void DrawTextSelection(PdfBitmap bitmap, int pageIndex)
  {
    if (this.MouseMode != EditorMouseModes.Default || pageIndex != this.caretInfo.PageIndex || this.caretInfo.Caret < 0 || this.caretInfo.EndCaret < 0)
      return;
    IPdfParagraph caretParagraph = this.GetCaretParagraph();
    if (caretParagraph != null)
    {
      int caret = this.caretInfo.Caret;
      int endCaret = this.caretInfo.EndCaret;
      foreach (FS_RECTF rc in (IEnumerable<FS_RECTF>) caretParagraph.GetSelectedBoxs(Math.Min(caret, endCaret), Math.Max(caret, endCaret)))
      {
        Int32Rect devicePixelRect = this.PageToDevicePixelRect(rc, pageIndex);
        int x = devicePixelRect.X;
        int y = devicePixelRect.Y;
        int width = devicePixelRect.Width;
        int height = devicePixelRect.Height;
        bitmap.FillRectEx(x, y, width, height, Helpers.ToArgb(this.TextSelectColor), BlendTypes.FXDIB_BLEND_MULTIPLY);
      }
    }
  }

  protected virtual void DrawLoadingIcon(
    DrawingContext drawingContext,
    PdfPage page,
    Rect actualRect)
  {
    FormattedText formattedText = new FormattedText(this.LoadingIconText, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Tahoma"), 14.0, (Brush) Brushes.Black, Helpers.GetPixelSize((DependencyObject) this))
    {
      MaxTextWidth = actualRect.Width,
      MaxTextHeight = actualRect.Height,
      TextAlignment = TextAlignment.Left
    };
    double x = (actualRect.Width - formattedText.Width) / 2.0 + actualRect.X;
    if (x < actualRect.X)
      x = actualRect.X;
    double y = (actualRect.Height - formattedText.Height) / 2.0 + actualRect.Y;
    if (y < actualRect.Y)
      y = actualRect.Y;
    drawingContext.DrawText(formattedText, new Point(x, y));
  }

  protected virtual void DrawParagraphBorder(
    DrawingContext drawingContext,
    int pageIndex,
    Rect actualRect)
  {
    lock (this.analysers)
    {
      if (this.analysers[pageIndex] == null)
        return;
      for (int index = 0; index < this.analysers[pageIndex].ParagraphsCount; ++index)
      {
        FS_RECTF bbox = this.analysers[pageIndex].GetParagraph(index).GetBBox();
        Rect clientRect;
        if (this.TryGetClientRect(pageIndex, bbox, out clientRect))
        {
          if (this.MouseMode == EditorMouseModes.Default && pageIndex == this.caretInfo.PageIndex && index == this.caretInfo.ParagraphIndex)
            drawingContext.DrawRectangle((Brush) null, this._paragraphBorderSelectedColorPen, clientRect);
          else
            drawingContext.DrawRectangle((Brush) null, this._paragraphBorderColorPen, clientRect);
          if (this.MouseMode == EditorMouseModes.SelectParagraph && pageIndex == this.caretInfo.PageIndex && index == this.caretInfo.ParagraphIndex && this.selectParagraphMouseDownPoint.HasValue && this.selectParagraphMouseCurPoint.HasValue)
          {
            FS_POINTF point1 = this.selectParagraphMouseDownPoint.Value;
            FS_POINTF point2 = this.selectParagraphMouseCurPoint.Value;
            Point clientPoint1;
            Point clientPoint2;
            if (this.TryGetClientPoint(pageIndex, point1.ToPoint(), out clientPoint1) && this.TryGetClientPoint(pageIndex, point2.ToPoint(), out clientPoint2))
            {
              double offsetX = clientPoint2.X - clientPoint1.X;
              double offsetY = clientPoint2.Y - clientPoint1.Y;
              if (offsetX != 0.0 || offsetY != 0.0)
              {
                Rect rectangle = clientRect;
                rectangle.Offset(offsetX, offsetY);
                drawingContext.DrawRectangle(this._paragraphDragingColorBrush, this._paragraphBorderColorPen, rectangle);
              }
            }
          }
        }
      }
    }
  }

  protected virtual void DrawCaret(DrawingContext drawingContext, Rect actualRect)
  {
    if (!this.IsCaretVisible())
      return;
    lock (this.caretInfo)
    {
      if (Stopwatch.GetTimestamp() / 5000000L % 2L == 1L)
      {
        Rect caretRect = this.caretInfo.GetCaretRect(actualRect);
        if (!caretRect.IsEmpty)
          drawingContext.DrawRectangle((Brush) Brushes.Black, (Pen) null, caretRect);
      }
    }
  }

  protected virtual void DrawPageBorder(DrawingContext drawingContext, Rect BBox)
  {
    Helpers.DrawRectangle(drawingContext, this._pageBorderColorPen, BBox);
  }

  protected virtual void DrawCurrentPageHighlight(
    DrawingContext drawingContext,
    int pageIndex,
    Rect actualRect)
  {
    if (!this.ShowCurrentPageHighlight || pageIndex != this.Document.Pages.CurrentIndex)
      return;
    double num = this._currentPageHighlightColorPen.Thickness / 2.0;
    actualRect.Inflate(num, num);
    Helpers.DrawRectangle(drawingContext, this._currentPageHighlightColorPen, actualRect);
  }

  protected virtual void DrawPageMask(
    DrawingContext drawingContext,
    int pageIndex,
    Rect actualRect)
  {
    Brush pageMaskBrush = this.PageMaskBrush;
    if (pageMaskBrush == null || pageMaskBrush is SolidColorBrush solidColorBrush && solidColorBrush.Color.A == (byte) 0)
      return;
    drawingContext.DrawRectangle(pageMaskBrush, (Pen) null, actualRect);
  }

  protected virtual void DrawRenderedPagesToDevice(
    DrawingContext drawingContext,
    PdfBitmap canvasBitmap,
    PdfBitmap formsBitmap,
    int canvasWidth,
    int canvasHeight,
    bool allPagesAreRendered)
  {
    int stride = this._prPages.CanvasBitmap.Stride;
    int dpi = Helpers.GetDpi((DependencyObject) this);
    if (this._canvasWpfBitmap == null || this._canvasWpfBitmap.PixelWidth != canvasWidth || this._canvasWpfBitmap.PixelHeight != canvasHeight)
      this._canvasWpfBitmap = new WriteableBitmap(canvasWidth, canvasHeight, (double) dpi, (double) dpi, PixelFormats.Bgra32, (BitmapPalette) null);
    if (allPagesAreRendered || this.cachedCanvasBitmap == null)
      this._canvasWpfBitmap.WritePixels(new Int32Rect(0, 0, canvasWidth, canvasHeight), (formsBitmap ?? canvasBitmap).Buffer, stride * canvasHeight, stride, 0, 0);
    else
      this.cachedCanvasBitmap.WriteToDeviceBitmap(this._canvasWpfBitmap, formsBitmap ?? canvasBitmap, (IPdfScrollInfo) this);
    Point pixelOffset = Helpers.GetPixelOffset((UIElement) this);
    Helpers.DrawImageUnscaled((DependencyObject) this, drawingContext, this._canvasWpfBitmap, pixelOffset.X, pixelOffset.Y);
  }

  protected virtual void DrawPageSeparators(
    DrawingContext drawingContext,
    ref List<Point> separator)
  {
    if (separator == null || !this.ShowPageSeparator)
      return;
    for (int index = 0; index < separator.Count; index += 2)
      drawingContext.DrawLine(this._pageSeparatorColorPen, separator[index], separator[index + 1]);
  }

  protected virtual void InternalSetCursor(CursorTypes cursor, bool panTool = false)
  {
    switch (cursor)
    {
      case CursorTypes.NESW:
        this.Cursor = Cursors.SizeNESW;
        break;
      case CursorTypes.NWSE:
        this.Cursor = Cursors.SizeNWSE;
        break;
      case CursorTypes.VBeam:
        this.Cursor = Cursors.IBeam;
        break;
      case CursorTypes.HBeam:
        this.Cursor = Cursors.IBeam;
        break;
      case CursorTypes.Hand:
        this.Cursor = Cursors.Hand;
        break;
      case (CursorTypes) 10:
        this.Cursor = Cursors.SizeAll;
        break;
      default:
        this.Cursor = (Cursor) null;
        break;
    }
  }

  private bool IsCaretVisible()
  {
    if (!this.IsKeyboardFocused || this.imeProcessing || this.MouseMode != 0)
      return false;
    (int startPage, int endPage) = this.GetVisiblePageRange();
    if (this.caretInfo.PageIndex >= startPage && this.caretInfo.PageIndex <= endPage && this.caretInfo.ParagraphIndex != -1 && this.caretInfo.Caret >= 0)
    {
      lock (this.analysers)
      {
        if (this.analysers[this.caretInfo.PageIndex] != null)
          return this.analysers[this.caretInfo.PageIndex].GetParagraph(this.caretInfo.ParagraphIndex).GetCaretPos(this.caretInfo.Caret, out FS_POINTF _, out FS_POINTF _);
      }
    }
    return false;
  }

  private void ResetCaretInfo(int pageIndex)
  {
    if (this.caretInfo.PageIndex == pageIndex && pageIndex != -1)
      return;
    this.caretInfo.PageIndex = pageIndex;
    this.caretInfo.ParagraphIndex = -1;
    this.caretInfo.Caret = 0;
    this.caretInfo.EndCaret = -1;
    this.caretInfo.RaiseCaretChanged();
    this.GetPageStructAnalyser(pageIndex);
  }

  internal IPdfParagraph GetCaretParagraph()
  {
    int pageIndex = this.caretInfo.PageIndex;
    int paragraphIndex = this.caretInfo.ParagraphIndex;
    if (this.analysers == null)
      return (IPdfParagraph) null;
    lock (this.analysers)
    {
      if (pageIndex >= 0 && pageIndex < this.analysers.Length && paragraphIndex >= 0 && this.analysers[pageIndex] != null)
        return this.analysers[pageIndex].GetParagraph(paragraphIndex);
    }
    return (IPdfParagraph) null;
  }

  internal void UpdateCurrentParagraphCarets()
  {
    IPdfParagraph caretParagraph = this.GetCaretParagraph();
    if (caretParagraph == null)
      return;
    int caret = this.caretInfo.Caret;
    int endCaret = this.caretInfo.EndCaret;
    if (endCaret == -1)
      caretParagraph.SetCurrentCarets(caret, -1);
    else
      caretParagraph.SetCurrentCarets(Math.Min(caret, endCaret), Math.Max(caret, endCaret));
  }

  internal void UpdateCaretInfo()
  {
    IPdfParagraph caretParagraph = this.GetCaretParagraph();
    if (caretParagraph != null)
    {
      int start;
      int end;
      if (!caretParagraph.GetCurrentCaretsRange(out start, out end))
        return;
      if (end != -1)
      {
        if (this.caretInfo.Caret == start)
          this.caretInfo.EndCaret = end;
        else if (this.caretInfo.Caret == end)
          this.caretInfo.Caret = start;
        else if (this.caretInfo.EndCaret == start)
          this.caretInfo.Caret = end;
        else if (this.caretInfo.EndCaret == end)
        {
          this.caretInfo.Caret = start;
        }
        else
        {
          this.caretInfo.Caret = start;
          this.caretInfo.EndCaret = end;
        }
      }
      else
      {
        this.caretInfo.Caret = start;
        this.caretInfo.EndCaret = end;
      }
      this.caretInfo.RaiseCaretChanged();
    }
    else
      this.ResetCaretInfo(-1);
  }

  internal LogicalStructAnalyser GetPageStructAnalyser(int pageIndex)
  {
    if (this.Document == null)
      return (LogicalStructAnalyser) null;
    lock (this.analysers)
    {
      if (pageIndex < 0 || pageIndex >= this.analysers.Length)
        return (LogicalStructAnalyser) null;
      if (this.analysers[pageIndex] != null)
        return this.analysers[pageIndex];
      (int startPage, int endPage) = this.GetVisiblePageRange();
      if (pageIndex >= startPage && pageIndex <= endPage)
        this.analysers[pageIndex] = new LogicalStructAnalyser(this.Document.Pages[pageIndex]);
      return this.analysers[pageIndex];
    }
  }

  internal bool CanDisposePage(int i)
  {
    OperationManager operationManager = this.operationManager;
    return operationManager == null || !operationManager.ContainsPageIndex(i);
  }

  private void SaveScrollPoint()
  {
    this._scrollPointSaved = false;
    if (this._renderRects == null)
      return;
    this._scrollPoint = this.ClientToPage(this.CurrentIndex, new Point(0.0, 0.0));
    this._scrollPointSaved = true;
  }

  private void RestoreScrollPoint()
  {
    if (!this._scrollPointSaved)
      return;
    this.ScrollToPoint(this.CurrentIndex, this._scrollPoint);
  }

  private PdfEditor.CaptureInfo CaptureFillForms(PdfForms fillForms)
  {
    PdfEditor.CaptureInfo captureInfo = new PdfEditor.CaptureInfo();
    if (fillForms == null)
      return captureInfo;
    captureInfo.forms = fillForms;
    captureInfo.sync = fillForms.SynchronizationContext;
    fillForms.SynchronizationContext = SynchronizationContext.Current;
    captureInfo.color = fillForms.SetHighlightColorEx(FormFieldTypes.FPDF_FORMFIELD_UNKNOWN, Helpers.ToArgb(this.FormHighlightColor));
    fillForms.AppBeep += new EventHandler<EventArgs<BeepTypes>>(this.FormsAppBeep);
    fillForms.DoGotoAction += new EventHandler<DoGotoActionEventArgs>(this.FormsDoGotoAction);
    fillForms.DoNamedAction += new EventHandler<EventArgs<string>>(this.FormsDoNamedAction);
    fillForms.GotoPage += new EventHandler<EventArgs<int>>(this.FormsGotoPage);
    fillForms.Invalidate += new EventHandler<InvalidatePageEventArgs>(this.FormsInvalidate);
    fillForms.OutputSelectedRect += new EventHandler<InvalidatePageEventArgs>(this.FormsOutputSelectedRect);
    fillForms.SetCursor += new EventHandler<SetCursorEventArgs>(this.FormsSetCursor);
    return captureInfo;
  }

  private void ReleaseFillForms(PdfEditor.CaptureInfo captureInfo)
  {
    if (captureInfo.forms == null)
      return;
    captureInfo.forms.AppBeep -= new EventHandler<EventArgs<BeepTypes>>(this.FormsAppBeep);
    captureInfo.forms.DoGotoAction -= new EventHandler<DoGotoActionEventArgs>(this.FormsDoGotoAction);
    captureInfo.forms.DoNamedAction -= new EventHandler<EventArgs<string>>(this.FormsDoNamedAction);
    captureInfo.forms.GotoPage -= new EventHandler<EventArgs<int>>(this.FormsGotoPage);
    captureInfo.forms.Invalidate -= new EventHandler<InvalidatePageEventArgs>(this.FormsInvalidate);
    captureInfo.forms.OutputSelectedRect -= new EventHandler<InvalidatePageEventArgs>(this.FormsOutputSelectedRect);
    captureInfo.forms.SetCursor -= new EventHandler<SetCursorEventArgs>(this.FormsSetCursor);
    captureInfo.forms.SynchronizationContext = captureInfo.sync;
  }

  private void CalcAndSetCurrentPage()
  {
    if (this.Document == null)
      return;
    int index = this.CalcCurrentPage();
    if (index >= 0)
      this.SetCurrentPage(index);
    this.InvalidateVisual();
  }

  public virtual void ProcessDestination(PdfDestination pdfDestination)
  {
    if (pdfDestination == null)
      return;
    float num1 = pdfDestination.Left.GetValueOrDefault();
    float num2 = pdfDestination.Top.GetValueOrDefault();
    float valueOrDefault1 = pdfDestination.Right.GetValueOrDefault();
    float valueOrDefault2 = pdfDestination.Bottom.GetValueOrDefault();
    float valueOrDefault3 = pdfDestination.Zoom.GetValueOrDefault();
    DestinationTypes destinationType = pdfDestination.DestinationType;
    float num3 = Math.Max(num1, valueOrDefault1) - Math.Min(num1, valueOrDefault1);
    float num4 = Math.Max(num2, valueOrDefault2) - Math.Min(num2, valueOrDefault2);
    float num5 = (float) (this.ActualWidth - SystemParameters.VerticalScrollBarWidth);
    float num6 = (float) (this.ActualHeight - SystemParameters.HorizontalScrollBarHeight);
    if (destinationType == DestinationTypes.FitB || destinationType == DestinationTypes.FitBV || destinationType == DestinationTypes.FitBH)
    {
      FS_RECTF fsRectf = this.Document.Pages[pdfDestination.PageIndex].PageObjects.CalcuateBoundingBox();
      num1 = destinationType == DestinationTypes.FitBV ? num1 : fsRectf.left;
      num2 = destinationType == DestinationTypes.FitBH ? num2 : fsRectf.top;
      num3 = Math.Max(fsRectf.left, fsRectf.right) - Math.Min(fsRectf.left, fsRectf.right);
      num4 = Math.Max(fsRectf.top, fsRectf.bottom) - Math.Min(fsRectf.top, fsRectf.bottom);
    }
    switch (destinationType)
    {
      case DestinationTypes.XYZ:
        if ((double) valueOrDefault3 > 0.0)
        {
          this.SizeMode = SizeModes.Zoom;
          this.Zoom = valueOrDefault3;
        }
        this.ScrollToPoint(pdfDestination.PageIndex, new Point((double) num1, (double) num2));
        break;
      case DestinationTypes.Fit:
        this.SizeMode = SizeModes.FitToSize;
        this.ScrollToPage(pdfDestination.PageIndex);
        break;
      case DestinationTypes.FitH:
        this.SizeMode = SizeModes.FitToWidth;
        this.ScrollToPoint(pdfDestination.PageIndex, new Point(0.0, (double) num2));
        break;
      case DestinationTypes.FitV:
        this.SizeMode = SizeModes.FitToHeight;
        this.ScrollToPoint(pdfDestination.PageIndex, new Point((double) num1, 0.0));
        break;
      case DestinationTypes.FitR:
      case DestinationTypes.FitB:
        float val1 = num5 / (float) ((double) num3 / 72.0 * 96.0);
        float val2 = num6 / (float) ((double) num4 / 72.0 * 96.0);
        this.SizeMode = SizeModes.Zoom;
        this.Zoom = Math.Min(val1, val2);
        this.ScrollToPoint(pdfDestination.PageIndex, new Point((double) num1, (double) num2));
        break;
      case DestinationTypes.FitBH:
        float num7 = num5 / (float) ((double) num3 / 72.0 * 96.0);
        this.SizeMode = SizeModes.Zoom;
        this.Zoom = num7;
        this.ScrollToPoint(pdfDestination.PageIndex, new Point((double) num1, (double) num2));
        break;
      case DestinationTypes.FitBV:
        float num8 = num6 / (float) ((double) num4 / 72.0 * 96.0);
        this.SizeMode = SizeModes.Zoom;
        this.Zoom = num8;
        this.ScrollToPoint(pdfDestination.PageIndex, new Point((double) num1, (double) num2));
        break;
    }
    this.InvalidateVisual();
  }

  private void ProcesRemoteGotoAction(
    PdfDestination destination,
    PdfFileSpecification fileSpecification)
  {
    if ((PdfWrapper) fileSpecification == (PdfWrapper) null)
      throw new ArgumentNullException(nameof (fileSpecification));
    if ((PdfWrapper) fileSpecification.EmbeddedFile != (PdfWrapper) null)
    {
      if (fileSpecification.EmbeddedFile.Stream == null)
        throw new ArgumentNullException("EmbeddedFile.Stream");
      this.LoadDocument(fileSpecification.EmbeddedFile.Stream.DecodedData);
    }
    else
    {
      if (fileSpecification.FileName == null)
        throw new ArgumentNullException("EmbeddedFile.FileName");
      this.LoadDocument(fileSpecification.FileName);
    }
    if (destination != null && destination.Name != null && this.Document.NamedDestinations[destination.Name] != null)
      this.ProcessDestination(this.Document.NamedDestinations[destination.Name]);
    else
      this.ProcessDestination(destination);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private int CalcCurrentPage()
  {
    return this.ViewMode == ViewModes.TilesHorizontal || this.ViewMode == ViewModes.TilesLine || this.ViewMode == ViewModes.TilesVertical ? this.CalcCurrentPageDoubleTrackCore() : this.CalcCurrentPageCore();
  }

  private int CalcCurrentPageDoubleTrackCore()
  {
    int num1 = -1;
    double num2 = 0.0;
    int? currentIndex = this.Document?.Pages?.CurrentIndex;
    Rect? nullable1 = new Rect?();
    Rect? nullable2 = new Rect?();
    for (int startPage = this._startPage; startPage <= this._endPage; startPage += 2)
    {
      Rect rect1 = this.renderRects(startPage);
      Rect? nullable3 = startPage + 1 < this.Document.Pages.Count ? new Rect?(this.renderRects(startPage + 1)) : new Rect?();
      rect1.X += this._autoScrollPosition.X;
      rect1.Y += this._autoScrollPosition.Y;
      Rect rect2 = rect1;
      if (nullable3.HasValue)
      {
        Rect rect3 = nullable3.Value;
        rect3.X += this._autoScrollPosition.X;
        rect3.Y += this._autoScrollPosition.Y;
        nullable3 = new Rect?(rect3);
        rect2 = new Rect(new Point(Math.Min(rect1.Left, rect3.Left), Math.Min(rect1.Top, rect3.Top)), new Point(Math.Max(rect1.Right, rect3.Right), Math.Max(rect1.Bottom, rect3.Bottom)));
      }
      if (rect2.IntersectsWith(this.ClientRect))
      {
        if (this._preferredSelectedPage && currentIndex.HasValue)
        {
          if (startPage == currentIndex.Value)
            return startPage;
          if (startPage + 1 == currentIndex.Value)
            return startPage + 1;
        }
        rect2.Intersect(this.ClientRect);
        double num3 = rect2.Width * rect2.Height;
        if (num2 < num3)
        {
          nullable1 = new Rect?(rect1);
          nullable2 = nullable3;
          num2 = num3;
          num1 = startPage;
        }
      }
    }
    int index = num1;
    if (!IsFirst(this._startPage, index))
      --index;
    if (index + 1 < this.Document.Pages.Count)
    {
      double v1 = 0.0;
      double v2 = 0.0;
      if (nullable1.HasValue)
      {
        Rect rect = nullable1.Value;
        rect.Intersect(this.ClientRect);
        if (!rect.IsEmpty)
          v1 = rect.Width * rect.Height;
      }
      if (nullable2.HasValue)
      {
        Rect rect = nullable2.Value;
        rect.Intersect(this.ClientRect);
        if (!rect.IsEmpty)
          v2 = rect.Width * rect.Height;
      }
      num1 = !currentIndex.HasValue || currentIndex.Value != index && currentIndex.Value != index + 1 ? (DoubleUtil.GreaterThan(v1, v2) ? index : index + 1) : (this._extent.Width > this.ViewportWidth && this.HorizontalOffset <= 1.0 ? index : (currentIndex.Value != index ? (currentIndex.Value != index + 1 ? (!currentIndex.HasValue || !AreClose(v1, v2, 0.1) ? (DoubleUtil.GreaterThan(v1, v2) ? index : index + 1) : currentIndex.Value) : (v2 > v1 / 4.0 ? index + 1 : index)) : (v1 > v2 / 4.0 ? index : index + 1)));
    }
    return num1;

    static bool IsFirst(int start, int index) => (index - start) % 2 == 0;

    static bool AreClose(double v1, double v2, double maxDiff)
    {
      return Math.Abs(v1 - v2) < Math.Abs(maxDiff);
    }
  }

  private int CalcCurrentPageCore()
  {
    int num1 = -1;
    double num2 = 0.0;
    int? currentIndex = this.Document?.Pages?.CurrentIndex;
    for (int startPage = this._startPage; startPage <= this._endPage; ++startPage)
    {
      PdfPage page = this.Document.Pages[startPage];
      Rect rect = this.renderRects(startPage);
      rect.X += this._autoScrollPosition.X;
      rect.Y += this._autoScrollPosition.Y;
      if (rect.IntersectsWith(this.ClientRect))
      {
        if (this._preferredSelectedPage && currentIndex.HasValue && startPage == currentIndex.Value)
          return startPage;
        rect.Intersect(this.ClientRect);
        double num3 = rect.Width * rect.Height;
        if (num2 < num3)
        {
          num2 = num3;
          num1 = startPage;
        }
      }
    }
    return num1;
  }

  private void CalcPageSeparator(Rect actualRect, int pageIndex, ref List<Point> separator)
  {
    if (!this.ShowPageSeparator || pageIndex == this._endPage || this.ViewMode == ViewModes.SinglePage)
      return;
    switch (this.ViewMode)
    {
      case ViewModes.Vertical:
        List<Point> pointList1 = separator;
        double x1 = actualRect.X;
        double bottom1 = actualRect.Bottom;
        Thickness pageMargin1 = this.PageMargin;
        double bottom2 = pageMargin1.Bottom;
        double y1 = bottom1 + bottom2;
        Point point1 = new Point(x1, y1);
        pointList1.Add(point1);
        List<Point> pointList2 = separator;
        double right1 = actualRect.Right;
        double bottom3 = actualRect.Bottom;
        pageMargin1 = this.PageMargin;
        double bottom4 = pageMargin1.Bottom;
        double y2 = bottom3 + bottom4;
        Point point2 = new Point(right1, y2);
        pointList2.Add(point2);
        break;
      case ViewModes.Horizontal:
      case ViewModes.TilesLine:
        List<Point> pointList3 = separator;
        double right2 = actualRect.Right;
        Thickness pageMargin2 = this.PageMargin;
        double right3 = pageMargin2.Right;
        Point point3 = new Point(right2 + right3, actualRect.Top);
        pointList3.Add(point3);
        List<Point> pointList4 = separator;
        double right4 = actualRect.Right;
        pageMargin2 = this.PageMargin;
        double right5 = pageMargin2.Right;
        Point point4 = new Point(right4 + right5, actualRect.Bottom);
        pointList4.Add(point4);
        break;
      case ViewModes.TilesVertical:
        Thickness pageMargin3;
        if ((pageIndex + 1) % this.TilesCount != 0)
        {
          List<Point> pointList5 = separator;
          double right6 = actualRect.Right;
          pageMargin3 = this.PageMargin;
          double right7 = pageMargin3.Right;
          Point point5 = new Point(right6 + right7, actualRect.Top);
          pointList5.Add(point5);
          List<Point> pointList6 = separator;
          double right8 = actualRect.Right;
          pageMargin3 = this.PageMargin;
          double right9 = pageMargin3.Right;
          Point point6 = new Point(right8 + right9, actualRect.Bottom);
          pointList6.Add(point6);
        }
        if (pageIndex > this._endPage - this.TilesCount)
          break;
        List<Point> pointList7 = separator;
        double x2 = actualRect.X;
        double bottom5 = actualRect.Bottom;
        pageMargin3 = this.PageMargin;
        double bottom6 = pageMargin3.Bottom;
        double y3 = bottom5 + bottom6;
        Point point7 = new Point(x2, y3);
        pointList7.Add(point7);
        List<Point> pointList8 = separator;
        double right10 = actualRect.Right;
        double bottom7 = actualRect.Bottom;
        pageMargin3 = this.PageMargin;
        double bottom8 = pageMargin3.Bottom;
        double y4 = bottom7 + bottom8;
        Point point8 = new Point(right10, y4);
        pointList8.Add(point8);
        break;
      case ViewModes.TilesHorizontal:
        Thickness pageMargin4;
        if (pageIndex <= this._endPage - this.TilesCount)
        {
          List<Point> pointList9 = separator;
          double right11 = actualRect.Right;
          pageMargin4 = this.PageMargin;
          double right12 = pageMargin4.Right;
          Point point9 = new Point(right11 + right12, actualRect.Top);
          pointList9.Add(point9);
          List<Point> pointList10 = separator;
          double right13 = actualRect.Right;
          pageMargin4 = this.PageMargin;
          double right14 = pageMargin4.Right;
          Point point10 = new Point(right13 + right14, actualRect.Bottom);
          pointList10.Add(point10);
        }
        if ((pageIndex + 1) % this.TilesCount == 0)
          break;
        List<Point> pointList11 = separator;
        double x3 = actualRect.X;
        double bottom9 = actualRect.Bottom;
        pageMargin4 = this.PageMargin;
        double bottom10 = pageMargin4.Bottom;
        double y5 = bottom9 + bottom10;
        Point point11 = new Point(x3, y5);
        pointList11.Add(point11);
        List<Point> pointList12 = separator;
        double right15 = actualRect.Right;
        double bottom11 = actualRect.Bottom;
        pageMargin4 = this.PageMargin;
        double bottom12 = pageMargin4.Bottom;
        double y6 = bottom11 + bottom12;
        Point point12 = new Point(right15, y6);
        pointList12.Add(point12);
        break;
    }
  }

  private bool GetRenderRectEx(ref Rect rrect, int processedPage)
  {
    if (this._renderRects.Length < this.OptimizedLoadThreshold || processedPage == 0)
    {
      rrect = this.GetRenderRect(processedPage);
      return true;
    }
    if (!this._renderRects[processedPage].IsChecked)
      return false;
    rrect = new Rect(this._renderRects[processedPage].X, this._renderRects[processedPage].Y, this._renderRects[processedPage].Width, this._renderRects[processedPage].Height);
    return true;
  }

  private Rect GetRenderRect(int index)
  {
    Size renderSize = this.GetRenderSize(index);
    return Helpers.CreateRect(this.GetRenderLocation(renderSize), renderSize);
  }

  private Point GetRenderLocation(Size size)
  {
    Size size1 = this.ClientRect.Size;
    if (this.ScrollOwner != null)
    {
      double num1 = this.ScrollOwner.ActualWidth - SystemParameters.VerticalScrollBarWidth;
      double num2 = this.ScrollOwner.ActualHeight - SystemParameters.HorizontalScrollBarHeight;
      size1 = new Size(num1 < 1E-06 ? 1E-06 : num1, num2 < 1E-06 ? 1E-06 : num2);
    }
    double num3 = 0.0 + this.Padding.Left;
    double num4 = 0.0 + this.Padding.Top;
    double num5 = (size1.Width - Helpers.ThicknessHorizontal(this.Padding) - size.Width) / 2.0;
    Thickness padding = this.Padding;
    double left1 = padding.Left;
    double num6 = num5 + left1;
    double num7 = (size1.Height - Helpers.ThicknessVertical(this.Padding) - size.Height) / 2.0;
    padding = this.Padding;
    double top1 = padding.Top;
    double num8 = num7 + top1;
    double num9 = size1.Width - Helpers.ThicknessHorizontal(this.Padding) - size.Width;
    padding = this.Padding;
    double left2 = padding.Left;
    double num10 = num9 + left2;
    double num11 = size1.Height - Helpers.ThicknessVertical(this.Padding) - size.Height;
    padding = this.Padding;
    double top2 = padding.Top;
    double num12 = num11 + top2;
    double num13 = num6;
    padding = this.Padding;
    double left3 = padding.Left;
    if (num13 < left3)
    {
      padding = this.Padding;
      num6 = padding.Left;
    }
    double num14 = num8;
    padding = this.Padding;
    double top3 = padding.Top;
    if (num14 < top3)
    {
      padding = this.Padding;
      num8 = padding.Top;
    }
    double num15 = num10;
    padding = this.Padding;
    double left4 = padding.Left;
    if (num15 < left4)
    {
      padding = this.Padding;
      num10 = padding.Left;
    }
    double num16 = num12;
    padding = this.Padding;
    double top4 = padding.Top;
    if (num16 < top4)
    {
      padding = this.Padding;
      num12 = padding.Top;
    }
    double x = num6;
    double y = num8;
    switch (this.PageVAlign)
    {
      case VerticalAlignment.Top:
        y = num4;
        break;
      case VerticalAlignment.Bottom:
        y = num12;
        break;
    }
    switch (this.PageHAlign)
    {
      case HorizontalAlignment.Left:
        x = num3;
        break;
      case HorizontalAlignment.Right:
        x = num10;
        break;
    }
    return new Point(x, y);
  }

  private Size GetRenderSize(int index)
  {
    Size size = this.ClientRect.Size;
    if (this.ScrollOwner != null)
    {
      double num1 = this.ScrollOwner.ActualWidth - SystemParameters.VerticalScrollBarWidth;
      double num2 = this.ScrollOwner.ActualHeight - SystemParameters.HorizontalScrollBarHeight;
      size = new Size(num1 < 1E-06 ? 1E-06 : num1, num2 < 1E-06 ? 1E-06 : num2);
    }
    double width1;
    double height;
    Pdfium.FPDF_GetPageSizeByIndex(this.Document.Handle, index, out width1, out height);
    double w = width1 * this._actualSizeFactor();
    double h = height * this._actualSizeFactor();
    double width2 = size.Width;
    double num = h * width2 / w;
    Size renderSize;
    switch (this.ViewMode)
    {
      case ViewModes.TilesVertical:
      case ViewModes.TilesLine:
        renderSize = this.CalcAppropriateSize(w, h, size.Width / (double) this.TilesCount - Helpers.ThicknessHorizontal(this.Padding), size.Height - Helpers.ThicknessVertical(this.Padding));
        break;
      case ViewModes.TilesHorizontal:
        renderSize = this.CalcAppropriateSize(w, h, size.Width - Helpers.ThicknessHorizontal(this.Padding), size.Height / (double) this.TilesCount - Helpers.ThicknessVertical(this.Padding));
        break;
      default:
        renderSize = this.CalcAppropriateSize(w, h, size.Width - Helpers.ThicknessHorizontal(this.Padding), size.Height - Helpers.ThicknessVertical(this.Padding));
        break;
    }
    if (this.SizeMode != SizeModes.Zoom)
    {
      try
      {
        this._preventStackOverflowBugWorkaround = true;
        this.Zoom = (float) (renderSize.Width / w);
      }
      finally
      {
        this._preventStackOverflowBugWorkaround = false;
      }
    }
    return renderSize;
  }

  internal double _actualSizeFactor()
  {
    double dpi = (double) Helpers.GetDpi((DependencyObject) this);
    return dpi / 72.0 / (dpi / 96.0) / Helpers.GetPixelSize((DependencyObject) this);
  }

  private Size CalcAppropriateSize(double w, double h, double fitWidth, double fitHeight)
  {
    if (fitWidth < 0.0)
      fitWidth = 0.0;
    if (fitHeight < 0.0)
      fitHeight = 0.0;
    double val1_1 = fitWidth;
    double val1_2 = h * val1_1 / w;
    switch (this.SizeMode)
    {
      case SizeModes.FitToSize:
        val1_2 = fitHeight;
        val1_1 = w * val1_2 / h;
        if (val1_1 > fitWidth)
        {
          val1_1 = fitWidth;
          val1_2 = h * val1_1 / w;
          break;
        }
        break;
      case SizeModes.FitToHeight:
        val1_2 = fitHeight;
        val1_1 = w * val1_2 / h;
        break;
      case SizeModes.Zoom:
        val1_1 = w * (double) this.Zoom;
        val1_2 = h * (double) this.Zoom;
        break;
    }
    return new Size(Math.Max(val1_1, 0.0), Math.Max(val1_2, 0.0));
  }

  private void AlignVertical(int from = 0, int to = -1)
  {
    if (to == -1)
      to = this._renderRects.Length;
    if (this._renderRects[to - 1].Bottom + this.Padding.Bottom >= this.ClientRect.Size.Height)
      return;
    double num = this.GetRenderLocation(new Size(0.0, this._renderRects[to - 1].Bottom - this.Padding.Bottom)).Y - this._renderRects[from].Y;
    for (int index = from; index < to; ++index)
      this._renderRects[index].Y += num;
  }

  private void AlignHorizontal(int from = 0, int to = -1)
  {
    if (to == -1)
      to = this._renderRects.Length;
    if (this._renderRects[to - 1].Right + this.Padding.Right >= this.ClientRect.Size.Width)
      return;
    double num = this.GetRenderLocation(new Size(this._renderRects[to - 1].Right - this.Padding.Right, 0.0)).X - this._renderRects[from].X;
    for (int index = from; index < to; ++index)
      this._renderRects[index].X += num;
  }

  public int DeviceToPage(double x, double y, out Point pagePoint)
  {
    for (int startPage = this._startPage; startPage <= this._endPage; ++startPage)
    {
      RenderRect renderRect = this._renderRects[startPage];
      renderRect.X += this._autoScrollPosition.X;
      renderRect.Y += this._autoScrollPosition.Y;
      if (renderRect.Contains(x, y))
      {
        double pageX;
        double pageY;
        this.Document.Pages[startPage].DeviceToPage((int) renderRect.X, (int) renderRect.Y, (int) renderRect.Width, (int) renderRect.Height, this.PageRotation(this.Document.Pages[startPage]), (int) x, (int) y, out pageX, out pageY);
        pagePoint = new Point(pageX, pageY);
        return startPage;
      }
    }
    pagePoint = new Point(0.0, 0.0);
    return -1;
  }

  private Point PageToDevice(double x, double y, int pageIndex)
  {
    Rect rect = this.CalcActualRect(pageIndex);
    int deviceX;
    int deviceY;
    this.Document.Pages[pageIndex].PageToDevice(Helpers.UnitsToPixels((DependencyObject) this, rect.X), Helpers.UnitsToPixels((DependencyObject) this, rect.Y), Helpers.UnitsToPixels((DependencyObject) this, rect.Width), Helpers.UnitsToPixels((DependencyObject) this, rect.Height), this.PageRotation(this.Document.Pages[pageIndex]), (float) x, (float) y, out deviceX, out deviceY);
    return new Point(Helpers.PixelsToUnits((DependencyObject) this, deviceX), Helpers.PixelsToUnits((DependencyObject) this, deviceY));
  }

  private Int32Rect PageToDevicePixelRect(FS_RECTF rc, int pageIndex)
  {
    Point device1 = this.PageToDevice((double) rc.left, (double) rc.top, pageIndex);
    Point device2 = this.PageToDevice((double) rc.right, (double) rc.bottom, pageIndex);
    return new Int32Rect(Helpers.UnitsToPixels((DependencyObject) this, device1.X < device2.X ? device1.X : device2.X), Helpers.UnitsToPixels((DependencyObject) this, device1.Y < device2.Y ? device1.Y : device2.Y), Helpers.UnitsToPixels((DependencyObject) this, device1.X > device2.X ? device1.X - device2.X : device2.X - device1.X), Helpers.UnitsToPixels((DependencyObject) this, device1.Y > device2.Y ? device1.Y - device2.Y : device2.Y - device1.Y));
  }

  private PageRotate PageRotation(PdfPage pdfPage)
  {
    int num = pdfPage.Rotation - pdfPage.OriginalRotation;
    if (num < 0)
      num = 4 + num;
    return (PageRotate) num;
  }

  private Size CalcVertical()
  {
    double num = 0.0;
    Rect rrect = Rect.Empty;
    for (int index = 0; index < this._renderRects.Length; ++index)
    {
      bool isChecked = this.GetRenderRectEx(ref rrect, index);
      while (true)
      {
        this._renderRects[index] = Helpers.CreateRenderRect(rrect.X, index > 0 ? this._renderRects[index - 1].Bottom + Helpers.ThicknessVertical(this.PageMargin) : this.Padding.Top, rrect.Width, rrect.Height, isChecked);
        if (!isChecked && this.CalcActualRect(index).IntersectsWith(this.ClientRect))
        {
          isChecked = true;
          rrect = this.GetRenderRect(index);
        }
        else
          break;
      }
      if (num < this._renderRects[index].Right)
        num = this._renderRects[index].Right;
    }
    this.AlignVertical();
    return Helpers.CreateSize(num + this.Padding.Right, this._renderRects[this._renderRects.Length - 1].Bottom + this.Padding.Bottom);
  }

  private Size CalcTilesVertical()
  {
    Rect rrect = Rect.Empty;
    double num1 = 0.0;
    double num2 = 0.0;
    for (int from = 0; from < this._renderRects.Length; from += this.TilesCount)
    {
      int index1 = from;
      int index2;
      for (index2 = from; index2 < from + this.TilesCount && index2 < this._renderRects.Length; ++index2)
      {
        bool isChecked = this.GetRenderRectEx(ref rrect, index2);
        while (true)
        {
          this._renderRects[index2] = new RenderRect((index2 - from) % this.TilesCount != 0 ? this._renderRects[index2 - 1].Right + Helpers.ThicknessHorizontal(this.PageMargin) : this.Padding.Left, from > 0 ? num1 + Helpers.ThicknessVertical(this.PageMargin) : this.Padding.Top, rrect.Width, rrect.Height, isChecked);
          if (!isChecked && this.CalcActualRect(index2).IntersectsWith(this.ClientRect))
          {
            isChecked = true;
            rrect = this.GetRenderRect(index2);
          }
          else
            break;
        }
        if (this._renderRects[index1].Bottom < this._renderRects[index2].Bottom)
          index1 = index2;
      }
      this.AlignHorizontal(from, index2);
      num1 = this._renderRects[index1].Bottom;
      if (num2 < this._renderRects[index2 - 1].Right)
        num2 = this._renderRects[index2 - 1].Right;
    }
    this.AlignVertical();
    double num3 = num2;
    Thickness padding = this.Padding;
    double right = padding.Right;
    double width = num3 + right;
    double num4 = num1;
    padding = this.Padding;
    double bottom = padding.Bottom;
    double height = num4 + bottom;
    return new Size(width, height);
  }

  private Size CalcTilesHorizontal()
  {
    Rect rrect = Rect.Empty;
    double num1 = 0.0;
    double num2 = 0.0;
    for (int from = 0; from < this._renderRects.Length; from += this.TilesCount)
    {
      int index1 = from;
      int index2;
      for (index2 = from; index2 < from + this.TilesCount && index2 < this._renderRects.Length; ++index2)
      {
        bool isChecked = this.GetRenderRectEx(ref rrect, index2);
        while (true)
        {
          this._renderRects[index2] = new RenderRect(from > 0 ? num2 + Helpers.ThicknessHorizontal(this.PageMargin) : this.Padding.Left, (index2 - from) % this.TilesCount != 0 ? this._renderRects[index2 - 1].Bottom + Helpers.ThicknessVertical(this.PageMargin) : this.Padding.Top, rrect.Width, rrect.Height, isChecked);
          if (!isChecked && this.CalcActualRect(index2).IntersectsWith(this.ClientRect))
          {
            isChecked = true;
            rrect = this.GetRenderRect(index2);
          }
          else
            break;
        }
        if (this._renderRects[index1].Right < this._renderRects[index2].Right)
          index1 = index2;
      }
      this.AlignVertical(from, index2);
      num2 = this._renderRects[index1].Right;
      if (num1 < this._renderRects[index2 - 1].Bottom)
        num1 = this._renderRects[index2 - 1].Bottom;
    }
    this.AlignHorizontal();
    double num3 = num2;
    Thickness padding = this.Padding;
    double right = padding.Right;
    double width = num3 + right;
    double num4 = num1;
    padding = this.Padding;
    double bottom = padding.Bottom;
    double height = num4 + bottom;
    return new Size(width, height);
  }

  private Size CalcHorizontal()
  {
    double num = 0.0;
    Rect rrect = Rect.Empty;
    for (int index = 0; index < this._renderRects.Length; ++index)
    {
      bool isChecked = this.GetRenderRectEx(ref rrect, index);
      while (true)
      {
        this._renderRects[index] = Helpers.CreateRenderRect(index > 0 ? this._renderRects[index - 1].Right + Helpers.ThicknessHorizontal(this.PageMargin) : this.Padding.Left, rrect.Y, rrect.Width, rrect.Height, isChecked);
        if (!isChecked && this.CalcActualRect(index).IntersectsWith(this.ClientRect))
        {
          isChecked = true;
          rrect = this.GetRenderRect(index);
        }
        else
          break;
      }
      if (num < this._renderRects[index].Bottom)
        num = this._renderRects[index].Bottom;
    }
    this.AlignHorizontal();
    return Helpers.CreateSize(this._renderRects[this._renderRects.Length - 1].Right + this.Padding.Right, num + this.Padding.Bottom);
  }

  private Size CalcSingle()
  {
    Size size = Helpers.CreateSize(0.0, 0.0);
    Rect rrect = Rect.Empty;
    for (int index = 0; index < this._renderRects.Length; ++index)
    {
      bool isChecked = true;
      if (index == this.CurrentIndex)
        rrect = this.GetRenderRect(index);
      else
        isChecked = this.GetRenderRectEx(ref rrect, index);
      this._renderRects[index] = Helpers.CreateRenderRect(rrect.X, rrect.Y, rrect.Width, rrect.Height, isChecked);
      if (index == this.Document.Pages.CurrentIndex)
        size = Helpers.CreateSize(rrect.Width + Helpers.ThicknessHorizontal(this.Padding), rrect.Height + Helpers.ThicknessVertical(this.Padding));
    }
    return size;
  }

  private Size CalcTilesLine()
  {
    int num = this.Document?.Pages?.CurrentIndex ?? -1;
    Size size = new Size(0.0, 0.0);
    Rect rrect = Rect.Empty;
    if (num == -1)
      return size;
    for (int index = 0; index < this._renderRects.Length; ++index)
    {
      bool isChecked = true;
      if (index >= this._startPage && index <= this._endPage)
        rrect = this.GetRenderRect(index);
      else
        isChecked = this.GetRenderRectEx(ref rrect, index);
      this._renderRects[index] = new RenderRect(index % this.TilesCount == 0 ? this.Padding.Left : this._renderRects[index - 1].Right + Helpers.ThicknessHorizontal(this.PageMargin), rrect.Y, rrect.Width, rrect.Height, isChecked);
      if (index % this.TilesCount == this.TilesCount - 1 || index == this._renderRects.Length - 1)
      {
        this.AlignHorizontal(index - index % this.TilesCount, index + 1);
        if (index == this._endPage)
          size = new Size(this._renderRects[index].Right + this.Padding.Right, this._renderRects[this.IdxWithLowestBottom(index - index % this.TilesCount, index)].Bottom + this.Padding.Bottom);
      }
    }
    return size;
  }

  private Rect renderRects(int index)
  {
    return this._renderRects != null ? this._renderRects[index].Rect() : new Rect(0.0, 0.0, 0.0, 0.0);
  }

  private void SetCurrentPage(int index)
  {
    try
    {
      this.Document.Pages.CurrentPageChanged -= new EventHandler(this.Pages_CurrentPageChanged);
      if (this.Document.Pages.CurrentIndex != index)
      {
        int startPage = this._startPage;
        int endPage = this._endPage;
        this.Document.Pages.CurrentIndex = index;
        this.OnCurrentPageChanged(EventArgs.Empty);
        if (this._renderRects != null)
        {
          if (this.ViewMode == ViewModes.SinglePage || this.ViewMode == ViewModes.TilesLine)
            this.UpdateScrollBars(new Size(this._renderRects[this._endPage].Right + this.Padding.Right, this._renderRects[this.IdxWithLowestBottom(this._startPage, this._endPage)].Bottom + this.Padding.Bottom));
          if ((this.ViewMode == ViewModes.SinglePage || this.ViewMode == ViewModes.TilesLine) && this._startPage != startPage)
          {
            for (int index1 = startPage; index1 <= endPage; ++index1)
            {
              if (this.PageAutoDispose && this.CanDisposePage(index1))
              {
                PageDisposeHelper.DisposePage(this.Document.Pages[index1]);
                this.analysers[index1] = (LogicalStructAnalyser) null;
              }
            }
          }
        }
      }
      this.ResetCaretInfo(index);
    }
    finally
    {
      this.Document.Pages.CurrentPageChanged += new EventHandler(this.Pages_CurrentPageChanged);
    }
  }

  private bool CalcIntersectEntries(
    HighlightInfo existEntry,
    HighlightInfo addingEntry,
    out List<HighlightInfo> calcEntries)
  {
    calcEntries = new List<HighlightInfo>();
    int charIndex1 = existEntry.CharIndex;
    int num1 = existEntry.CharIndex + existEntry.CharsCount - 1;
    int charIndex2 = addingEntry.CharIndex;
    int num2 = addingEntry.CharIndex + addingEntry.CharsCount - 1;
    if (charIndex1 < charIndex2 && num1 >= charIndex2 && num1 <= num2)
    {
      calcEntries.Add(new HighlightInfo()
      {
        CharIndex = charIndex1,
        CharsCount = charIndex2 - charIndex1,
        Color = existEntry.Color
      });
      return true;
    }
    if (charIndex1 >= charIndex2 && charIndex1 <= num2 && num1 > num2)
    {
      calcEntries.Add(new HighlightInfo()
      {
        CharIndex = num2 + 1,
        CharsCount = num1 - num2,
        Color = existEntry.Color
      });
      return true;
    }
    if (charIndex1 >= charIndex2 && num1 <= num2)
      return true;
    if (charIndex1 >= charIndex2 || num1 <= num2)
      return false;
    calcEntries.Add(new HighlightInfo()
    {
      CharIndex = charIndex1,
      CharsCount = charIndex2 - charIndex1,
      Color = existEntry.Color
    });
    calcEntries.Add(new HighlightInfo()
    {
      CharIndex = num2 + 1,
      CharsCount = num1 - num2,
      Color = existEntry.Color
    });
    return true;
  }

  private void CalcPages()
  {
    Size size;
    switch (this.ViewMode)
    {
      case ViewModes.Vertical:
        size = this.CalcVertical();
        break;
      case ViewModes.Horizontal:
        size = this.CalcHorizontal();
        break;
      case ViewModes.TilesVertical:
        size = this.CalcTilesVertical();
        break;
      case ViewModes.TilesHorizontal:
        size = this.CalcTilesHorizontal();
        break;
      case ViewModes.TilesLine:
        size = this.CalcTilesLine();
        break;
      default:
        size = this.CalcSingle();
        break;
    }
    this.UpdateScrollBars(size);
    if (this.SizeMode == SizeModes.Zoom)
      return;
    this.OnZoomChanged(EventArgs.Empty);
  }

  private void UpdateScrollBars(Size size)
  {
    if (size.Width == 0.0 || size.Height == 0.0)
      return;
    this._extent = size;
    this._viewport = new Size(this.ActualWidth, this.ActualHeight);
    if (this.ScrollOwner != null)
      this.ScrollOwner.InvalidateScrollInfo();
  }

  private bool GetWord(PdfText text, int ci, out int si, out int ei)
  {
    si = ei = ci;
    if (text == null || ci < 0)
      return false;
    for (int index = ci - 1; index >= 0; --index)
    {
      char character = text.GetCharacter(index);
      if (!char.IsSeparator(character) && !char.IsPunctuation(character) && !char.IsControl(character) && !char.IsWhiteSpace(character) && character != '\r' && character != '\n')
        si = index;
      else
        break;
    }
    int countChars = text.CountChars;
    for (int index = ci + 1; index < countChars; ++index)
    {
      char character = text.GetCharacter(index);
      if (!char.IsSeparator(character) && !char.IsPunctuation(character) && !char.IsControl(character) && !char.IsWhiteSpace(character) && character != '\r' && character != '\n')
        ei = index;
      else
        break;
    }
    return true;
  }

  private void StartInvalidateTimer(bool force)
  {
    if (this._invalidateTimer != null && this._invalidateTimer.Interval.TotalMilliseconds == 10.0)
      return;
    if (this._invalidateTimer == null)
    {
      this._invalidateTimer = new DispatcherTimer(DispatcherPriority.Input);
      this._invalidateTimer.Interval = TimeSpan.FromMilliseconds(force ? 10.0 : 50.0);
      this._invalidateTimer.Tick += (EventHandler) ((s, a) =>
      {
        if (this._prPages.IsNeedContinuePaint)
        {
          if (this._invalidateTimer.Interval.TotalMilliseconds != 10.0)
          {
            this._invalidateTimer.Stop();
            this._invalidateTimer.Interval = TimeSpan.FromMilliseconds(10.0);
            this._invalidateTimer.Start();
          }
        }
        else if (this.IsCaretVisible())
        {
          if (this._invalidateTimer.Interval.TotalMilliseconds != 50.0)
          {
            this._invalidateTimer.Stop();
            this._invalidateTimer.Interval = TimeSpan.FromMilliseconds(50.0);
            this._invalidateTimer.Start();
          }
        }
        else
        {
          this._invalidateTimer.Stop();
          this._invalidateTimer = (DispatcherTimer) null;
        }
        this.InvalidateVisual();
      });
      this._invalidateTimer.Start();
    }
    else
    {
      if (!force)
        return;
      this._invalidateTimer.Stop();
      this._invalidateTimer.Start();
    }
  }

  private int IdxWithLowestBottom(int from, int to)
  {
    int index1 = from;
    for (int index2 = from + 1; index2 <= to; ++index2)
    {
      if (this._renderRects[index1].Bottom < this._renderRects[index2].Bottom)
        index1 = index2;
    }
    return index1;
  }

  protected virtual void OnFormsInvalidate(InvalidatePageEventArgs e) => this.InvalidateVisual();

  protected virtual void OnFormsGotoPage(EventArgs<int> e)
  {
    if (this.Document == null)
      return;
    this.SetCurrentPage(e.Value);
    this.ScrollToPage(e.Value);
  }

  protected virtual void OnFormsDoNamedAction(EventArgs<string> e)
  {
    if (this.Document == null)
      return;
    PdfDestination byName = this.Document.NamedDestinations.GetByName(e.Value);
    if (byName == null)
      return;
    this.SetCurrentPage(byName.PageIndex);
    this.ScrollToPage(byName.PageIndex);
  }

  protected virtual void OnFormsDoGotoAction(DoGotoActionEventArgs e)
  {
    if (this.Document == null)
    {
      this._onstartPageIndex = e.PageIndex;
    }
    else
    {
      this.SetCurrentPage(e.PageIndex);
      this.ScrollToPage(e.PageIndex);
    }
  }

  protected virtual void OnFormsAppBeep(EventArgs<BeepTypes> e)
  {
    switch (e.Value)
    {
      case BeepTypes.Error:
        SystemSounds.Asterisk.Play();
        break;
      case BeepTypes.Warning:
        SystemSounds.Exclamation.Play();
        break;
      case BeepTypes.Question:
        SystemSounds.Question.Play();
        break;
      case BeepTypes.Status:
        SystemSounds.Beep.Play();
        break;
      case BeepTypes.Default:
        SystemSounds.Beep.Play();
        break;
      default:
        SystemSounds.Beep.Play();
        break;
    }
  }

  protected virtual void OnFormsOutputSelectedRect(InvalidatePageEventArgs e)
  {
    if (this.Document == null)
      return;
    int pageIndex = this.Document.Pages.GetPageIndex(e.Page);
    Point device1 = this.PageToDevice((double) e.Rect.left, (double) e.Rect.top, pageIndex);
    Point device2 = this.PageToDevice((double) e.Rect.right, (double) e.Rect.bottom, pageIndex);
    double num1 = device2.X - device1.X;
    double num2 = device2.Y - device1.Y;
    this._selectedRectangles.Add(Helpers.CreateRect(device1.X + (num1 < 0.0 ? num1 : 0.0), device1.Y + (num2 < 0.0 ? num2 : 0.0), num1 < 0.0 ? -num1 : num1, num2 < 0.0 ? -num2 : num2));
  }

  protected virtual void OnFormsSetCursor(SetCursorEventArgs e) => this.InternalSetCursor(e.Cursor);

  private void FormsInvalidate(object sender, InvalidatePageEventArgs e)
  {
    this.OnFormsInvalidate(e);
  }

  private void FormsGotoPage(object sender, EventArgs<int> e) => this.OnFormsGotoPage(e);

  private void FormsDoNamedAction(object sender, EventArgs<string> e)
  {
    this.OnFormsDoNamedAction(e);
  }

  private void FormsDoGotoAction(object sender, DoGotoActionEventArgs e)
  {
    this.OnFormsDoGotoAction(e);
  }

  private void FormsAppBeep(object sender, EventArgs<BeepTypes> e) => this.OnFormsAppBeep(e);

  private void FormsOutputSelectedRect(object sender, InvalidatePageEventArgs e)
  {
    this.OnFormsOutputSelectedRect(e);
  }

  private void FormsSetCursor(object sender, SetCursorEventArgs e) => this.OnFormsSetCursor(e);

  private void Pages_ProgressiveRender(object sender, ProgressiveRenderEventArgs e)
  {
    e.NeedPause = this._prPages.IsNeedPause(sender as PdfPage);
  }

  private void Pages_CurrentPageChanged(object sender, EventArgs e)
  {
    if (this.ViewMode == ViewModes.SinglePage || this.ViewMode == ViewModes.TilesLine)
    {
      this._prPages.ReleaseCanvas();
      this.UpdateScrollBars(new Size(this._renderRects[this._endPage].Right + this.Padding.Right, this._renderRects[this.IdxWithLowestBottom(this._startPage, this._endPage)].Bottom + this.Padding.Bottom));
    }
    this.OnCurrentPageChanged(EventArgs.Empty);
    this.InvalidateVisual();
  }

  private void Pages_PageInserted(object sender, PageCollectionChangedEventArgs e)
  {
    this.UpdateDocLayout();
  }

  private void Pages_PageDeleted(object sender, PageCollectionChangedEventArgs e)
  {
    this.UpdateDocLayout();
  }

  private void Global_PreviewKeyDown(object sender, KeyEventArgs e)
  {
    if (this.Document == null || this.keyboardProcessor == null || this.operationManager == null)
      return;
    ProcessEditKey(e);

    void ProcessEditKey(KeyEventArgs args)
    {
      try
      {
        args.Handled = this.keyboardProcessor.OnKeyDown(args.Key);
        if (!args.Handled)
          return;
        Common.WriteLog($"keyboardProcessor.OnKeyDown {args.Key}");
      }
      catch (Exception ex)
      {
        Common.WriteLog(ex.ToString());
      }
    }
  }

  private Rect ClientRect => new Rect(0.0, 0.0, this.ViewportWidth, this.ViewportHeight);

  public bool CanVerticallyScroll { get; set; }

  public bool CanHorizontallyScroll { get; set; }

  public ScrollViewer ScrollOwner
  {
    get => this.scrollOwner;
    set
    {
      this.scrollOwner = value;
      EventHandler scrollOwnerChanged = this.ScrollOwnerChanged;
      if (scrollOwnerChanged == null)
        return;
      scrollOwnerChanged((object) this, EventArgs.Empty);
    }
  }

  public event EventHandler ScrollOwnerChanged;

  public double ExtentWidth => this._extent.Width + this.FlyoutExtentWidth;

  public double ExtentHeight => this._extent.Height;

  public double ViewportWidth => this._viewport.Width;

  public double ViewportHeight => this._viewport.Height;

  public double HorizontalOffset => -this._autoScrollPosition.X;

  public double VerticalOffset => -this._autoScrollPosition.Y;

  public void SetVerticalOffset(double offset)
  {
    if (offset < 0.0 || this._viewport.Height >= this._extent.Height)
      offset = 0.0;
    else if (offset + this._viewport.Height >= this._extent.Height)
      offset = this._extent.Height - this._viewport.Height;
    double y = this._autoScrollPosition.Y;
    this._autoScrollPosition.Y = -offset;
    this.CalcAndSetCurrentPage();
    if (y != this._autoScrollPosition.Y)
      this._prPages.ReleaseCanvas();
    if (this.ScrollOwner == null)
      return;
    this.ScrollOwner.InvalidateScrollInfo();
  }

  public void SetHorizontalOffset(double offset)
  {
    if (offset < 0.0 || this._viewport.Width >= this.ExtentWidth)
      offset = 0.0;
    else if (offset + this._viewport.Width >= this.ExtentWidth)
      offset = this.ExtentWidth - this._viewport.Width;
    double x = this._autoScrollPosition.X;
    this._autoScrollPosition.X = -offset;
    this.CalcAndSetCurrentPage();
    if (x != this._autoScrollPosition.X)
      this._prPages.ReleaseCanvas();
    if (this.ScrollOwner == null)
      return;
    this.ScrollOwner.InvalidateScrollInfo();
  }

  public Rect MakeVisible(Visual visual, Rect rectangle)
  {
    if (this._isProgrammaticallyFocusSetted)
    {
      this._isProgrammaticallyFocusSetted = false;
      return rectangle;
    }
    this.SetHorizontalOffset(rectangle.X - this._autoScrollPosition.X);
    this.SetVerticalOffset(rectangle.Y - this._autoScrollPosition.Y);
    return new Rect(this._autoScrollPosition.X, this._autoScrollPosition.Y, this.ViewportWidth, this.ViewportHeight);
  }

  public void LineUp()
  {
    if (this.TryMovePrevPage(Orientation.Vertical))
      return;
    this.SetVerticalOffset(this.VerticalOffset - this._viewport.Height / 10.0);
  }

  public void LineDown()
  {
    if (this.TryMoveNextPage(Orientation.Vertical))
      return;
    this.SetVerticalOffset(this.VerticalOffset + this._viewport.Height / 10.0);
  }

  public void LineLeft()
  {
    if (this.TryMovePrevPage(Orientation.Horizontal))
      return;
    this.SetHorizontalOffset(this.HorizontalOffset - this._viewport.Width / 10.0);
  }

  public void LineRight()
  {
    if (this.TryMoveNextPage(Orientation.Horizontal))
      return;
    this.SetHorizontalOffset(this.HorizontalOffset + this._viewport.Width / 10.0);
  }

  public void PageUp()
  {
    if (this.TryMovePrevPage(Orientation.Vertical))
      return;
    this.SetVerticalOffset(this.VerticalOffset - this._viewport.Height * 1.0);
  }

  public void PageDown()
  {
    if (this.TryMoveNextPage(Orientation.Vertical))
      return;
    this.SetVerticalOffset(this.VerticalOffset + this._viewport.Height * 1.0);
  }

  public void PageLeft()
  {
    if (this.TryMovePrevPage(Orientation.Horizontal))
      return;
    this.SetHorizontalOffset(this.HorizontalOffset - this._viewport.Width * 1.0);
  }

  public void PageRight()
  {
    if (this.TryMoveNextPage(Orientation.Horizontal))
      return;
    this.SetHorizontalOffset(this.HorizontalOffset + this._viewport.Width * 1.0);
  }

  public void MouseWheelUp()
  {
    if (this.TryMovePrevPage(Orientation.Vertical))
      return;
    if (this.ScrollOwner.ComputedVerticalScrollBarVisibility == Visibility.Visible)
      this.SetVerticalOffset(this.VerticalOffset - this._viewport.Height / 10.0 * 1.0);
    else if (this.ScrollOwner.ComputedHorizontalScrollBarVisibility == Visibility.Visible)
      this.SetHorizontalOffset(this.HorizontalOffset - this._viewport.Width / 10.0 * 1.0);
    else
      this.SetVerticalOffset(this.VerticalOffset - this._viewport.Height / 10.0 * 1.0);
  }

  public void MouseWheelDown()
  {
    if (this.TryMoveNextPage(Orientation.Vertical))
      return;
    if (this.ScrollOwner.ComputedVerticalScrollBarVisibility == Visibility.Visible)
      this.SetVerticalOffset(this.VerticalOffset + this._viewport.Height / 10.0 * 1.0);
    else if (this.ScrollOwner.ComputedHorizontalScrollBarVisibility == Visibility.Visible)
      this.SetHorizontalOffset(this.HorizontalOffset + this._viewport.Width / 10.0 * 1.0);
    else
      this.SetVerticalOffset(this.VerticalOffset + this._viewport.Height / 10.0 * 1.0);
  }

  public void MouseWheelLeft()
  {
    if (this.TryMovePrevPage(Orientation.Horizontal) || this.ScrollOwner.ComputedHorizontalScrollBarVisibility != Visibility.Visible)
      return;
    this.SetHorizontalOffset(this.HorizontalOffset - this._viewport.Width / 10.0 * 1.0);
  }

  public void MouseWheelRight()
  {
    if (this.TryMoveNextPage(Orientation.Horizontal) || this.ScrollOwner.ComputedHorizontalScrollBarVisibility != Visibility.Visible)
      return;
    this.SetHorizontalOffset(this.HorizontalOffset + this._viewport.Width / 10.0 * 1.0);
  }

  private bool TryMoveNextPage(Orientation orientation)
  {
    if (this.ViewMode != ViewModes.SinglePage && this.ViewMode != ViewModes.TilesLine || this.Document == null || this.ScrollOwner == null)
      return false;
    int currentIndex = this.CurrentIndex;
    if (currentIndex >= this.Document.Pages.Count - 1)
      return false;
    if (orientation == Orientation.Horizontal)
    {
      if (this.ScrollOwner.HorizontalOffset < this.ScrollOwner.ScrollableWidth)
        return false;
    }
    else if (this.ScrollOwner.VerticalOffset < this.ScrollOwner.ScrollableHeight)
      return false;
    int index = currentIndex + 1;
    this._preferredSelectedPage = true;
    this.ScrollToPage(index);
    Rect rect = this.renderRects(index);
    if (rect.Width != 0.0 && rect.Height != 0.0)
    {
      this.SetVerticalOffset(rect.Y);
      this.SetHorizontalOffset(rect.X);
    }
    this._preferredSelectedPage = false;
    return true;
  }

  private bool TryMovePrevPage(Orientation orientation)
  {
    if (this.ViewMode != ViewModes.SinglePage && this.ViewMode != ViewModes.TilesLine || this.Document == null || this.ScrollOwner == null)
      return false;
    int currentIndex = this.CurrentIndex;
    if (currentIndex <= 0)
      return false;
    if (orientation == Orientation.Horizontal)
    {
      if (this.ScrollOwner.HorizontalOffset > 0.0)
        return false;
    }
    else if (this.ScrollOwner.VerticalOffset > 0.0)
      return false;
    int index = currentIndex - 1;
    this._preferredSelectedPage = true;
    this.ScrollToPage(index);
    Rect rect = this.renderRects(index);
    if (rect.Width != 0.0 && rect.Height != 0.0)
    {
      this.SetVerticalOffset(rect.Y);
      this.SetHorizontalOffset(rect.X);
    }
    this._preferredSelectedPage = false;
    return true;
  }

  private void MouseWheelHelper_MouseTilt(object sender, MouseTiltEventArgs e)
  {
    if (e.Delta > 0)
      this.MouseWheelRight();
    else
      this.MouseWheelLeft();
    e.Handled = true;
  }

  private void ProcessMouseDownForSelectParagraph(Point page_point, int page_index)
  {
    LogicalStructAnalyser pageStructAnalyser = this.GetPageStructAnalyser(page_index);
    FS_POINTF pdfPoint = page_point.ToPdfPoint();
    this.selectParagraphMouseDownPoint = new FS_POINTF?(pdfPoint);
    this.selectParagraphMouseCurPoint = new FS_POINTF?(pdfPoint);
    for (int index = 0; index < pageStructAnalyser.ParagraphsCount; ++index)
    {
      if (pageStructAnalyser.GetParagraph(index).GetBBox().Contains(pdfPoint))
      {
        this.caretInfo.Caret = 0;
        this.caretInfo.EndCaret = -1;
        this.caretInfo.ParagraphIndex = index;
        this.caretInfo.RaiseCaretChanged();
        this.UpdateCurrentParagraphCarets();
        this.InvalidateVisual();
        if (this.CaptureMouse())
          return;
        this.selectParagraphMouseDownPoint = new FS_POINTF?();
        this.selectParagraphMouseCurPoint = new FS_POINTF?();
        return;
      }
    }
    this.caretInfo.Caret = 0;
    this.caretInfo.EndCaret = -1;
    this.caretInfo.ParagraphIndex = -1;
    this.caretInfo.RaiseCaretChanged();
    this.selectParagraphMouseDownPoint = new FS_POINTF?();
    this.selectParagraphMouseCurPoint = new FS_POINTF?();
    this.InvalidateVisual();
  }

  private CursorTypes ProcessMouseMoveForSelectParagraph(Point page_point, int page_index)
  {
    bool flag = Mouse.LeftButton == MouseButtonState.Pressed;
    FS_POINTF pdfPoint = page_point.ToPdfPoint();
    if (!flag)
    {
      LogicalStructAnalyser pageStructAnalyser = this.GetPageStructAnalyser(page_index);
      for (int index = 0; index < pageStructAnalyser.ParagraphsCount; ++index)
      {
        if (pageStructAnalyser.GetParagraph(index).GetBBox().Contains(pdfPoint))
          return (CursorTypes) 10;
      }
    }
    else if (this.selectParagraphMouseDownPoint.HasValue && this.GetCaretParagraph() != null)
    {
      this.selectParagraphMouseCurPoint = new FS_POINTF?(pdfPoint);
      this.InvalidateVisual();
      return (CursorTypes) 10;
    }
    return CursorTypes.Arrow;
  }

  private void ProcessMouseUpForSelectParagraph(Point page_point, int page_index)
  {
    if (this.selectParagraphMouseDownPoint.HasValue)
    {
      FS_POINTF fsPointf = this.selectParagraphMouseDownPoint.Value;
      Point point = page_point;
      double dx = point.X - (double) fsPointf.X;
      double dy = point.Y - (double) fsPointf.Y;
      IPdfParagraph caretParagraph = this.GetCaretParagraph();
      if (caretParagraph != null)
      {
        IPdfUndoItem undoItem;
        caretParagraph.Offset((float) dx, (float) dy, true, out undoItem);
        if (undoItem != null)
          this.operationManager.AddUndoItem(undoItem);
      }
    }
    this.selectParagraphMouseDownPoint = new FS_POINTF?();
    this.selectParagraphMouseCurPoint = new FS_POINTF?();
    this.ForceRender();
  }

  private void ProcessMouseDoubleClickForSelectTextTool(Point page_point, int page_index)
  {
    if (this.MouseMode != EditorMouseModes.SelectParagraph || this.GetCaretParagraph() == null)
      return;
    this.MouseMode = EditorMouseModes.Default;
    this.InternalSetCursor(CursorTypes.VBeam);
    this.ProcessMouseDownForSelectTextTool(page_point, page_index);
  }

  private void ProcessMouseDownForSelectTextTool(Point page_point, int page_index)
  {
    LogicalStructAnalyser pageStructAnalyser = this.GetPageStructAnalyser(page_index);
    for (int index = 0; index < pageStructAnalyser.ParagraphsCount; ++index)
    {
      IPdfParagraph paragraph = pageStructAnalyser.GetParagraph(index);
      FS_RECTF bbox = paragraph.GetBBox();
      if (page_point.X >= (double) bbox.left && page_point.X <= (double) bbox.right && page_point.Y >= (double) bbox.bottom && page_point.Y <= (double) bbox.top)
      {
        this.caretInfo.Caret = paragraph.GetCaretAt(page_point.ToPdfPoint());
        this.caretInfo.EndCaret = -1;
        this.caretInfo.ParagraphIndex = index;
        this.caretInfo.RaiseCaretChanged();
        this.UpdateCurrentParagraphCarets();
        if (this.IsKeyboardFocused && !this.imeProcessing)
        {
          this.UpdateImeState(false);
          this.imeReceiver.ProcessGotKeyboardFocus();
        }
        this.ForceRender();
        return;
      }
    }
    this.MouseMode = EditorMouseModes.SelectParagraph;
  }

  private CursorTypes ProcessMouseMoveForSelectTextTool(Point page_point, int page_index)
  {
    int charIndexAtPos = this.Document.Pages[page_index].Text.GetCharIndexAtPos((float) page_point.X, (float) page_point.Y, 10f, 10f);
    if (this._lastPressedTime.HasValue)
    {
      IPdfParagraph caretParagraph = this.GetCaretParagraph();
      if (caretParagraph != null)
      {
        int caretAt = caretParagraph.GetCaretAt(page_point.ToPdfPoint());
        if (caretAt >= 0)
        {
          this.caretInfo.EndCaret = caretAt;
          this.caretInfo.RaiseCaretChanged();
          this.UpdateCurrentParagraphCarets();
          this.InvalidateVisual();
        }
      }
    }
    return !this.Document.Pages[page_index].OnMouseMove(0, (float) page_point.X, (float) page_point.Y) && charIndexAtPos >= 0 ? CursorTypes.VBeam : CursorTypes.Arrow;
  }

  private struct CaptureInfo
  {
    public PdfForms forms;
    public SynchronizationContext sync;
    public int color;
  }
}
