// Decompiled with JetBrains decompiler
// Type: PDFKit.PdfViewer
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Actions;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.EventArguments;
using Patagames.Pdf.Net.Exceptions;
using Patagames.Pdf.Net.Wrappers;
using PDFKit.Events;
using PDFKit.PdfViewerDecorators;
using PDFKit.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

#nullable disable
namespace PDFKit;

[LicenseProvider]
public class PdfViewer : Control, IScrollInfo, IPdfScrollInfo, IPdfScrollInfoInternal
{
  private bool _preventStackOverflowBugWorkaround = false;
  private SelectInfo _selectInfo = new SelectInfo()
  {
    StartPage = -1
  };
  private SortedDictionary<int, List<HighlightInfo>> _highlightedText = new SortedDictionary<int, List<HighlightInfo>>();
  private SortedDictionary<int, FS_RECTF> _pagesCropBoxInfo = new SortedDictionary<int, FS_RECTF>();
  private bool _mousePressed = false;
  private bool _mousePressedInLink = false;
  private bool _isShowSelection = false;
  private int _onstartPageIndex = 0;
  private Point _panToolInitialScrollPosition;
  private Point _panToolInitialMousePosition;
  private PdfForms _fillForms;
  private List<Rect> _selectedRectangles = new List<Rect>();
  private Pen _pageBorderColorPen = Helpers.CreatePen((Color) PdfViewer.PageBorderColorProperty.DefaultMetadata.DefaultValue);
  private Pen _pageCropBoxColorPen = Helpers.CreatePen((Brush) Brushes.Red);
  private Pen _pageSeparatorColorPen = Helpers.CreatePen((Color) PdfViewer.PageSeparatorColorProperty.DefaultMetadata.DefaultValue);
  private Pen _currentPageHighlightColorPen = Helpers.CreatePen((Color) PdfViewer.CurrentPageHighlightColorProperty.DefaultMetadata.DefaultValue, 4.0);
  private RenderRect[] _renderRects;
  private bool _preferredSelectedPage = false;
  private (int pageIndex, int annotIndex, int annotCount) mouseOverAnnot = (-1, -1, 0);
  private CachedCanvasBitmap cachedCanvasBitmap;
  private MouseWheelHelper mouseWheelHelper;
  private static readonly PanToolCursorHelper panToolCursorHelper = new PanToolCursorHelper();
  private long lastRenderCompletedStamp = -1;
  private int lastRenderingTime = 0;
  private Size _extent = new Size(0.0, 0.0);
  private Size _viewport = new Size(0.0, 0.0);
  private Point _autoScrollPosition = new Point(0.0, 0.0);
  private bool _isProgrammaticallyFocusSetted = false;
  internal PRCollection _prPages = new PRCollection();
  private DispatcherTimer _invalidateTimer = (DispatcherTimer) null;
  private WriteableBitmap _canvasWpfBitmap = (WriteableBitmap) null;
  private bool _loadedByViewer = true;
  private PdfViewer.CaptureInfo _externalDocCapture;
  private Point _scrollPoint;
  private bool _scrollPointSaved;
  private PdfViewer.SmoothSelection _smoothSelection;
  private PdfViewerDecoratorCollection decorators;
  private PdfViewerDecoratorDrawingArgs decoratorDrawingArgs;
  public static readonly DependencyProperty FormsBlendModeProperty = DependencyProperty.Register(nameof (FormsBlendMode), typeof (BlendTypes), typeof (PdfViewer), (PropertyMetadata) new FrameworkPropertyMetadata((object) BlendTypes.FXDIB_BLEND_MULTIPLY, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) => (o as PdfViewer).OnFormsBlendModeChanged(EventArgs.Empty))));
  public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register(nameof (Document), typeof (PdfDocument), typeof (PdfViewer), new PropertyMetadata((object) null, (PropertyChangedCallback) ((o, e) =>
  {
    PdfViewer pdfViewer = o as PdfViewer;
    PdfDocument oldValue = e.OldValue as PdfDocument;
    PdfDocument newValue = e.NewValue as PdfDocument;
    if (oldValue == newValue)
      return;
    int num = oldValue != null ? oldValue.Pages.CurrentIndex : -1;
    pdfViewer._prPages.ReleaseCanvas();
    if (oldValue != null && pdfViewer._loadedByViewer)
    {
      oldValue.Dispose();
      pdfViewer.OnDocumentClosed(EventArgs.Empty);
    }
    else if (oldValue != null && !pdfViewer._loadedByViewer)
    {
      oldValue.Pages.CurrentPageChanged -= new EventHandler(pdfViewer.Pages_CurrentPageChanged);
      oldValue.Pages.PageInserted -= new EventHandler<PageCollectionChangedEventArgs>(pdfViewer.Pages_PageInserted);
      oldValue.Pages.PageDeleted -= new EventHandler<PageCollectionChangedEventArgs>(pdfViewer.Pages_PageDeleted);
      oldValue.Pages.ProgressiveRender -= new EventHandler<ProgressiveRenderEventArgs>(pdfViewer.Pages_ProgressiveRender);
    }
    pdfViewer._extent = new Size(0.0, 0.0);
    pdfViewer._selectInfo = new SelectInfo()
    {
      StartPage = -1
    };
    pdfViewer._highlightedText.Clear();
    pdfViewer._renderRects = (RenderRect[]) null;
    pdfViewer._loadedByViewer = false;
    pdfViewer.ReleaseFillForms(pdfViewer._externalDocCapture);
    pdfViewer.UpdateDocLayout();
    if (newValue != null)
    {
      if (newValue.FormFill != pdfViewer._fillForms)
        pdfViewer._externalDocCapture = pdfViewer.CaptureFillForms(newValue.FormFill);
      newValue.Pages.CurrentPageChanged += new EventHandler(pdfViewer.Pages_CurrentPageChanged);
      newValue.Pages.PageInserted += new EventHandler<PageCollectionChangedEventArgs>(pdfViewer.Pages_PageInserted);
      newValue.Pages.PageDeleted += new EventHandler<PageCollectionChangedEventArgs>(pdfViewer.Pages_PageDeleted);
      newValue.Pages.ProgressiveRender += new EventHandler<ProgressiveRenderEventArgs>(pdfViewer.Pages_ProgressiveRender);
      if (Pdfium.IsFullAPI && newValue.OpenDestination != null)
        pdfViewer._onstartPageIndex = newValue.OpenDestination.PageIndex;
      if (newValue.Pages.CurrentIndex != pdfViewer._onstartPageIndex)
        pdfViewer.SetCurrentPage(pdfViewer._onstartPageIndex);
      else if (newValue.Pages.CurrentIndex != num)
        pdfViewer.OnCurrentPageChanged(EventArgs.Empty);
      if (newValue.Pages.Count > 0)
      {
        if (pdfViewer._onstartPageIndex != 0)
          pdfViewer.ScrollToPage(pdfViewer._onstartPageIndex);
        else
          pdfViewer._autoScrollPosition = new Point(0.0, 0.0);
      }
      pdfViewer._onstartPageIndex = 0;
      PdfPage currentPage = newValue.Pages.CurrentPage;
      if (currentPage?.Annots != null && currentPage.Annots.Count > 0)
      {
        foreach (PdfTextAnnotation text in currentPage.Annots.OfType<PdfTextAnnotation>().Where<PdfTextAnnotation>((Func<PdfTextAnnotation, bool>) (c => c.Relationship == RelationTypes.Reply)))
          text.RegenerateAppearancesAdvance();
      }
    }
    pdfViewer.OnAfterDocumentChanged(EventArgs.Empty);
  }), (CoerceValueCallback) ((dobj, o) =>
  {
    PdfViewer pdfViewer = dobj as PdfViewer;
    PdfDocument document = pdfViewer.Document;
    PdfDocument pdfDocument = o as PdfDocument;
    return document != pdfDocument && (pdfViewer.OnBeforeDocumentChanged(new DocumentClosingEventArgs()) || document != null && pdfViewer._loadedByViewer && pdfViewer.OnDocumentClosing(new DocumentClosingEventArgs())) ? (object) document : (object) pdfDocument;
  })));
  public static readonly DependencyProperty PageBackColorProperty = DependencyProperty.Register(nameof (PageBackColor), typeof (Color), typeof (PdfViewer), (PropertyMetadata) new FrameworkPropertyMetadata((object) Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) => (o as PdfViewer).OnPageBackColorChanged(EventArgs.Empty))));
  public static readonly DependencyProperty PageMarginProperty = DependencyProperty.Register(nameof (PageMargin), typeof (Thickness), typeof (PdfViewer), (PropertyMetadata) new FrameworkPropertyMetadata((object) new Thickness(10.0), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) =>
  {
    (o as PdfViewer).UpdateDocLayout();
    (o as PdfViewer).OnPageMarginChanged(EventArgs.Empty);
  })));
  public new static readonly DependencyProperty PaddingProperty = DependencyProperty.Register(nameof (Padding), typeof (Thickness), typeof (PdfViewer), (PropertyMetadata) new FrameworkPropertyMetadata((object) new Thickness(10.0), FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) => (o as PdfViewer).UpdateDocLayout())));
  public static readonly DependencyProperty PageBorderColorProperty = DependencyProperty.Register(nameof (PageBorderColor), typeof (Color), typeof (PdfViewer), (PropertyMetadata) new FrameworkPropertyMetadata((object) Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 0, (byte) 0), FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) =>
  {
    PdfViewer pdfViewer = o as PdfViewer;
    pdfViewer._pageBorderColorPen = Helpers.CreatePen((Color) e.NewValue);
    pdfViewer.OnPageBorderColorChanged(EventArgs.Empty);
  })));
  public static readonly DependencyProperty SizeModeProperty = DependencyProperty.Register(nameof (SizeMode), typeof (SizeModes), typeof (PdfViewer), (PropertyMetadata) new FrameworkPropertyMetadata((object) SizeModes.FitToWidth, FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) =>
  {
    PdfViewer pdfViewer = o as PdfViewer;
    pdfViewer.UpdateDocLayout();
    pdfViewer.OnSizeModeChanged(EventArgs.Empty);
  })));
  public static readonly DependencyProperty TextSelectColorProperty = DependencyProperty.Register(nameof (TextSelectColor), typeof (Color), typeof (PdfViewer), (PropertyMetadata) new FrameworkPropertyMetadata((object) Color.FromArgb((byte) 70, (byte) 70, (byte) 130, (byte) 180), FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) => (o as PdfViewer).OnTextSelectColorChanged(EventArgs.Empty))));
  public static readonly DependencyProperty FormHighlightColorProperty = DependencyProperty.Register(nameof (FormHighlightColor), typeof (Color), typeof (PdfViewer), (PropertyMetadata) new FrameworkPropertyMetadata((object) Color.FromArgb((byte) 0, byte.MaxValue, byte.MaxValue, byte.MaxValue), FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) =>
  {
    PdfViewer pdfViewer = o as PdfViewer;
    if (pdfViewer._fillForms != null)
      pdfViewer._fillForms.SetHighlightColorEx(FormFieldTypes.FPDF_FORMFIELD_UNKNOWN, Helpers.ToArgb((Color) e.NewValue));
    if (pdfViewer.Document != null && !pdfViewer._loadedByViewer && pdfViewer._externalDocCapture.forms != null)
      pdfViewer._externalDocCapture.forms.SetHighlightColorEx(FormFieldTypes.FPDF_FORMFIELD_UNKNOWN, Helpers.ToArgb((Color) e.NewValue));
    pdfViewer.OnFormHighlightColorChanged(EventArgs.Empty);
  })));
  public static readonly DependencyProperty ZoomProperty = DependencyProperty.Register(nameof (Zoom), typeof (float), typeof (PdfViewer), (PropertyMetadata) new FrameworkPropertyMetadata((object) 1f, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) =>
  {
    PdfViewer pdfViewer = o as PdfViewer;
    if (pdfViewer._preventStackOverflowBugWorkaround)
      return;
    pdfViewer.UpdateDocLayout();
    pdfViewer.OnZoomChanged(EventArgs.Empty);
  })));
  public static readonly DependencyProperty SelectedTextProperty = DependencyProperty.Register(nameof (SelectedText), typeof (string), typeof (PdfViewer), (PropertyMetadata) new FrameworkPropertyMetadata((object) "", FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Journal));
  public static readonly DependencyProperty ViewModeProperty = DependencyProperty.Register(nameof (ViewMode), typeof (ViewModes), typeof (PdfViewer), (PropertyMetadata) new FrameworkPropertyMetadata((object) ViewModes.Vertical, FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) =>
  {
    PdfViewer pdfViewer = o as PdfViewer;
    pdfViewer.UpdateDocLayout();
    pdfViewer.OnViewModeChanged(EventArgs.Empty);
  })));
  public static readonly DependencyProperty PageSeparatorColorProperty = DependencyProperty.Register(nameof (PageSeparatorColor), typeof (Color), typeof (PdfViewer), (PropertyMetadata) new FrameworkPropertyMetadata((object) Color.FromArgb(byte.MaxValue, (byte) 190, (byte) 190, (byte) 190), FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) =>
  {
    PdfViewer pdfViewer = o as PdfViewer;
    pdfViewer._pageSeparatorColorPen = Helpers.CreatePen((Color) e.NewValue);
    pdfViewer.OnPageSeparatorColorChanged(EventArgs.Empty);
  })));
  public static readonly DependencyProperty ShowPageSeparatorProperty = DependencyProperty.Register(nameof (ShowPageSeparator), typeof (bool), typeof (PdfViewer), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) => (o as PdfViewer).OnShowPageSeparatorChanged(EventArgs.Empty))));
  public static readonly DependencyProperty CurrentPageHighlightColorProperty = DependencyProperty.Register(nameof (CurrentPageHighlightColor), typeof (Color), typeof (PdfViewer), (PropertyMetadata) new FrameworkPropertyMetadata((object) Color.FromArgb((byte) 170, (byte) 70, (byte) 130, (byte) 180), FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) =>
  {
    PdfViewer pdfViewer = o as PdfViewer;
    pdfViewer._currentPageHighlightColorPen = Helpers.CreatePen((Color) e.NewValue);
    pdfViewer.OnCurrentPageHighlightColorChanged(EventArgs.Empty);
  })));
  public static readonly DependencyProperty ShowCurrentPageHighlightProperty = DependencyProperty.Register(nameof (ShowCurrentPageHighlight), typeof (bool), typeof (PdfViewer), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) => (o as PdfViewer).OnShowCurrentPageHighlightChanged(EventArgs.Empty))));
  public static readonly DependencyProperty PageVAlignProperty = DependencyProperty.Register(nameof (PageVAlign), typeof (VerticalAlignment), typeof (PdfViewer), (PropertyMetadata) new FrameworkPropertyMetadata((object) VerticalAlignment.Center, FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) =>
  {
    PdfViewer pdfViewer = o as PdfViewer;
    pdfViewer.UpdateDocLayout();
    pdfViewer.OnPageAlignChanged(EventArgs.Empty);
  })));
  public static readonly DependencyProperty PageHAlignProperty = DependencyProperty.Register(nameof (PageHAlign), typeof (HorizontalAlignment), typeof (PdfViewer), (PropertyMetadata) new FrameworkPropertyMetadata((object) HorizontalAlignment.Center, FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) =>
  {
    PdfViewer pdfViewer = o as PdfViewer;
    pdfViewer.UpdateDocLayout();
    pdfViewer.OnPageAlignChanged(EventArgs.Empty);
  })));
  public static readonly DependencyProperty RenderFlagsProperty = DependencyProperty.Register(nameof (RenderFlags), typeof (RenderFlags), typeof (PdfViewer), (PropertyMetadata) new FrameworkPropertyMetadata((object) (RenderFlags.FPDF_LCD_TEXT | RenderFlags.FPDF_NO_CATCH), FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) => (o as PdfViewer).OnRenderFlagsChanged(EventArgs.Empty))));
  public static readonly DependencyProperty TilesCountProperty = DependencyProperty.Register(nameof (TilesCount), typeof (int), typeof (PdfViewer), (PropertyMetadata) new FrameworkPropertyMetadata((object) 2, FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) =>
  {
    PdfViewer pdfViewer = o as PdfViewer;
    pdfViewer.UpdateDocLayout();
    pdfViewer.OnTilesCountChanged(EventArgs.Empty);
  }), (CoerceValueCallback) ((v, o) => (int) o < 2 ? (object) 2 : o)));
  public static readonly DependencyProperty MouseModeProperty = DependencyProperty.Register(nameof (MouseMode), typeof (MouseModes), typeof (PdfViewer), (PropertyMetadata) new FrameworkPropertyMetadata((object) MouseModes.Default, FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) => (o as PdfViewer).OnMouseModeChanged(EventArgs.Empty))));
  public static readonly DependencyProperty ShowLoadingIconProperty = DependencyProperty.Register(nameof (ShowLoadingIcon), typeof (bool), typeof (PdfViewer), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) => (o as PdfViewer).OnShowLoadingIconChanged(EventArgs.Empty))));
  public static readonly DependencyProperty UseProgressiveRenderProperty = DependencyProperty.Register(nameof (UseProgressiveRender), typeof (bool), typeof (PdfViewer), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) =>
  {
    PdfViewer pdfViewer = o as PdfViewer;
    pdfViewer.UpdateDocLayout();
    pdfViewer.OnUseProgressiveRenderChanged(EventArgs.Empty);
  })));
  public static readonly DependencyProperty LoadingIconTextProperty = DependencyProperty.Register(nameof (LoadingIconText), typeof (string), typeof (PdfViewer), (PropertyMetadata) new FrameworkPropertyMetadata((object) "", FrameworkPropertyMetadataOptions.Journal, (PropertyChangedCallback) ((o, e) => (o as PdfViewer).OnLoadingIconTextChanged(EventArgs.Empty))));
  public static readonly DependencyProperty PageAutoDisposeProperty = DependencyProperty.Register(nameof (PageAutoDispose), typeof (bool), typeof (PdfViewer), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, FrameworkPropertyMetadataOptions.Journal));
  public static readonly DependencyProperty OptimizedLoadThresholdProperty = DependencyProperty.Register(nameof (OptimizedLoadThreshold), typeof (int), typeof (PdfViewer), (PropertyMetadata) new FrameworkPropertyMetadata((object) 1000));
  public static readonly DependencyProperty FlyoutExtentWidthProperty = DependencyProperty.Register(nameof (FlyoutExtentWidth), typeof (double), typeof (PdfViewer), new PropertyMetadata((object) 0.0, (PropertyChangedCallback) ((s, a) => ((PdfViewer) s).ScrollOwner?.InvalidateScrollInfo())));
  public static readonly DependencyProperty IsAnnotationVisibleProperty = DependencyProperty.Register(nameof (IsAnnotationVisible), typeof (bool), typeof (PdfViewer), new PropertyMetadata((object) true, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is PdfViewer pdfViewer2))
      return;
    pdfViewer2.cachedCanvasBitmap?.Dispose();
    pdfViewer2.cachedCanvasBitmap = (CachedCanvasBitmap) null;
    pdfViewer2.InvalidateVisual();
  })));
  public static readonly DependencyProperty PageMaskBrushProperty = DependencyProperty.Register(nameof (PageMaskBrush), typeof (Brush), typeof (PdfViewer), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty IsRenderPausedProperty = DependencyProperty.Register(nameof (IsRenderPaused), typeof (bool), typeof (PdfViewer), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.AffectsRender));
  public static readonly DependencyProperty OverrideCursorProperty = DependencyProperty.Register(nameof (OverrideCursor), typeof (Cursor), typeof (PdfViewer), new PropertyMetadata((object) null, (PropertyChangedCallback) ((s, a) =>
  {
    PdfViewer pdfViewer3;
    int num;
    if (a.NewValue != a.OldValue)
    {
      pdfViewer3 = s as PdfViewer;
      num = pdfViewer3 != null ? 1 : 0;
    }
    else
      num = 0;
    if (num == 0)
      return;
    if (a.NewValue is Cursor newValue2)
    {
      pdfViewer3.Cursor = newValue2;
    }
    else
    {
      MouseEventArgs e = new MouseEventArgs(Mouse.PrimaryDevice, 0)
      {
        RoutedEvent = UIElement.MouseMoveEvent,
        Source = (object) pdfViewer3
      };
      pdfViewer3.RaiseEvent((RoutedEventArgs) e);
    }
  })));
  public static readonly DependencyProperty IsLinkAnnotationHighlightedProperty = DependencyProperty.Register(nameof (IsLinkAnnotationHighlighted), typeof (bool), typeof (PdfViewer), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.AffectsRender, (PropertyChangedCallback) ((s, a) =>
  {
    PdfViewer pdfViewer4;
    int num;
    if (!object.Equals(a.NewValue, a.OldValue))
    {
      pdfViewer4 = s as PdfViewer;
      num = pdfViewer4 != null ? 1 : 0;
    }
    else
      num = 0;
    if (num == 0)
      return;
    foreach (PdfLinkAnnotationDecorator annotationDecorator in pdfViewer4.decorators.OfType<PdfLinkAnnotationDecorator>())
      annotationDecorator.IsEnabled = a.NewValue is bool newValue4 && newValue4;
  })));
  public static readonly DependencyProperty LinkAnnotationHighlightBorderColorProperty = DependencyProperty.Register(nameof (LinkAnnotationHighlightBorderColor), typeof (Color), typeof (PdfViewer), (PropertyMetadata) new FrameworkPropertyMetadata((object) Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 0, byte.MaxValue), FrameworkPropertyMetadataOptions.AffectsRender, (PropertyChangedCallback) ((s, a) =>
  {
    PdfViewer pdfViewer5;
    int num;
    if (!object.Equals(a.NewValue, a.OldValue))
    {
      pdfViewer5 = s as PdfViewer;
      num = pdfViewer5 != null ? 1 : 0;
    }
    else
      num = 0;
    if (num == 0)
      return;
    foreach (PdfLinkAnnotationDecorator annotationDecorator in pdfViewer5.decorators.OfType<PdfLinkAnnotationDecorator>())
      annotationDecorator.BorderColor = (Color) a.NewValue;
  })));
  public static readonly DependencyProperty LinkAnnotationHighlightFillColorProperty = DependencyProperty.Register(nameof (LinkAnnotationHighlightFillColor), typeof (Color), typeof (PdfViewer), (PropertyMetadata) new FrameworkPropertyMetadata((object) Colors.Transparent, FrameworkPropertyMetadataOptions.AffectsRender, (PropertyChangedCallback) ((s, a) =>
  {
    PdfViewer pdfViewer6;
    int num;
    if (!object.Equals(a.NewValue, a.OldValue))
    {
      pdfViewer6 = s as PdfViewer;
      num = pdfViewer6 != null ? 1 : 0;
    }
    else
      num = 0;
    if (num == 0)
      return;
    foreach (PdfLinkAnnotationDecorator annotationDecorator in pdfViewer6.decorators.OfType<PdfLinkAnnotationDecorator>())
      annotationDecorator.FillColor = (Color) a.NewValue;
  })));
  public static readonly DependencyProperty IsFillFormHighlightedProperty = DependencyProperty.Register(nameof (IsFillFormHighlighted), typeof (bool), typeof (PdfViewer), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, FrameworkPropertyMetadataOptions.AffectsRender, (PropertyChangedCallback) ((s, a) =>
  {
    PdfViewer pdfViewer7;
    int num;
    if (!object.Equals(a.NewValue, a.OldValue))
    {
      pdfViewer7 = s as PdfViewer;
      num = pdfViewer7 != null ? 1 : 0;
    }
    else
      num = 0;
    if (num == 0)
      return;
    foreach (PdfFormFieldDecorator formFieldDecorator in pdfViewer7.decorators.OfType<PdfFormFieldDecorator>())
      formFieldDecorator.IsEnabled = a.NewValue is bool newValue6 && newValue6;
  })));
  public static readonly DependencyProperty FillFormHighlightFocusedBorderColorProperty = DependencyProperty.Register(nameof (FillFormHighlightFocusedBorderColor), typeof (Color), typeof (PdfViewer), (PropertyMetadata) new FrameworkPropertyMetadata((object) Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 0, (byte) 0), FrameworkPropertyMetadataOptions.AffectsRender, (PropertyChangedCallback) ((s, a) =>
  {
    PdfViewer pdfViewer8;
    int num;
    if (!object.Equals(a.NewValue, a.OldValue))
    {
      pdfViewer8 = s as PdfViewer;
      num = pdfViewer8 != null ? 1 : 0;
    }
    else
      num = 0;
    if (num == 0)
      return;
    foreach (PdfFormFieldDecorator formFieldDecorator in pdfViewer8.decorators.OfType<PdfFormFieldDecorator>())
      formFieldDecorator.FocusBorderColor = (Color) a.NewValue;
  })));
  public static readonly DependencyProperty FillFormHighlightFillColorProperty = DependencyProperty.Register(nameof (FillFormHighlightFillColor), typeof (Color), typeof (PdfViewer), (PropertyMetadata) new FrameworkPropertyMetadata((object) Color.FromArgb((byte) 51, (byte) 85, (byte) 120, byte.MaxValue), FrameworkPropertyMetadataOptions.AffectsRender, (PropertyChangedCallback) ((s, a) =>
  {
    PdfViewer pdfViewer9;
    int num;
    if (!object.Equals(a.NewValue, a.OldValue))
    {
      pdfViewer9 = s as PdfViewer;
      num = pdfViewer9 != null ? 1 : 0;
    }
    else
      num = 0;
    if (num == 0)
      return;
    foreach (PdfFormFieldDecorator formFieldDecorator in pdfViewer9.decorators.OfType<PdfFormFieldDecorator>())
      formFieldDecorator.FillColor = (Color) a.NewValue;
  })));
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

  public event EventHandler<PdfBeforeLinkClickedEventArgs> BeforeLinkClicked;

  public event EventHandler<PdfAfterLinkClickedEventArgs> AfterLinkClicked;

  public event EventHandler RenderFlagsChanged;

  public event EventHandler TilesCountChanged;

  public event EventHandler HighlightedTextChanged;

  public event EventHandler MouseModeChanged;

  public event EventHandler ShowLoadingIconChanged;

  public event EventHandler UseProgressiveRenderChanged;

  public event EventHandler LoadingIconTextChanged;

  public event EventHandler FormsBlendModeChanged;

  public event EventHandler<AnnotationMouseEventArgs> AnnotationMouseEntered;

  public event EventHandler<AnnotationMouseEventArgs> AnnotationMouseExited;

  public event EventHandler<AnnotationMouseEventArgs> AnnotationMouseMoved;

  public event EventHandler<AnnotationMouseClickEventArgs> AnnotationMouseClick;

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
    GAManager.SendEvent("PdfToolBarMain", "DocumentLoaded", "Count", 1L);
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

  protected virtual void OnBeforeLinkClicked(PdfBeforeLinkClickedEventArgs e)
  {
    if (this.BeforeLinkClicked == null)
      return;
    this.BeforeLinkClicked((object) this, e);
  }

  protected virtual void OnAfterLinkClicked(PdfAfterLinkClickedEventArgs e)
  {
    if (this.AfterLinkClicked == null)
      return;
    this.AfterLinkClicked((object) this, e);
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

  protected virtual void OnHighlightedTextChanged(EventArgs e)
  {
    if (this.HighlightedTextChanged == null)
      return;
    this.HighlightedTextChanged((object) this, e);
  }

  protected virtual void OnMouseModeChanged(EventArgs e)
  {
    if (this.MouseModeChanged == null)
      return;
    this.MouseModeChanged((object) this, e);
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

  protected virtual void OnAnnotationMouseEntered(AnnotationMouseEventArgs e)
  {
    EventHandler<AnnotationMouseEventArgs> annotationMouseEntered = this.AnnotationMouseEntered;
    if (annotationMouseEntered == null)
      return;
    annotationMouseEntered((object) this, e);
  }

  protected virtual void OnAnnotationMouseExited(AnnotationMouseEventArgs e)
  {
    EventHandler<AnnotationMouseEventArgs> annotationMouseExited = this.AnnotationMouseExited;
    if (annotationMouseExited == null)
      return;
    annotationMouseExited((object) this, e);
  }

  protected virtual void OnAnnotationMouseMoved(AnnotationMouseEventArgs e)
  {
    EventHandler<AnnotationMouseEventArgs> annotationMouseMoved = this.AnnotationMouseMoved;
    if (annotationMouseMoved == null)
      return;
    annotationMouseMoved((object) this, e);
  }

  protected virtual void OnAnnotationMouseClick(AnnotationMouseClickEventArgs e)
  {
    EventHandler<AnnotationMouseClickEventArgs> annotationMouseClick = this.AnnotationMouseClick;
    if (annotationMouseClick == null)
      return;
    annotationMouseClick((object) this, e);
  }

  public BlendTypes FormsBlendMode
  {
    get => (BlendTypes) this.GetValue(PdfViewer.FormsBlendModeProperty);
    set => this.SetValue(PdfViewer.FormsBlendModeProperty, (object) value);
  }

  public PdfDocument Document
  {
    get => (PdfDocument) this.GetValue(PdfViewer.DocumentProperty);
    set => this.SetValue(PdfViewer.DocumentProperty, (object) value);
  }

  public Color PageBackColor
  {
    get => (Color) this.GetValue(PdfViewer.PageBackColorProperty);
    set => this.SetValue(PdfViewer.PageBackColorProperty, (object) value);
  }

  public Thickness PageMargin
  {
    get => (Thickness) this.GetValue(PdfViewer.PageMarginProperty);
    set => this.SetValue(PdfViewer.PageMarginProperty, (object) value);
  }

  public new Thickness Padding
  {
    get => (Thickness) this.GetValue(PdfViewer.PaddingProperty);
    set => this.SetValue(PdfViewer.PaddingProperty, (object) value);
  }

  public Color PageBorderColor
  {
    get => (Color) this.GetValue(PdfViewer.PageBorderColorProperty);
    set => this.SetValue(PdfViewer.PageBorderColorProperty, (object) value);
  }

  public SizeModes SizeMode
  {
    get => (SizeModes) this.GetValue(PdfViewer.SizeModeProperty);
    set => this.SetValue(PdfViewer.SizeModeProperty, (object) value);
  }

  public Color TextSelectColor
  {
    get => (Color) this.GetValue(PdfViewer.TextSelectColorProperty);
    set => this.SetValue(PdfViewer.TextSelectColorProperty, (object) value);
  }

  public Color FormHighlightColor
  {
    get => (Color) this.GetValue(PdfViewer.FormHighlightColorProperty);
    set => this.SetValue(PdfViewer.FormHighlightColorProperty, (object) value);
  }

  public float Zoom
  {
    get => (float) this.GetValue(PdfViewer.ZoomProperty);
    set => this.SetValue(PdfViewer.ZoomProperty, (object) value);
  }

  public string SelectedText => (string) this.GetValue(PdfViewer.SelectedTextProperty);

  public ViewModes ViewMode
  {
    get => (ViewModes) this.GetValue(PdfViewer.ViewModeProperty);
    set => this.SetValue(PdfViewer.ViewModeProperty, (object) value);
  }

  public Color PageSeparatorColor
  {
    get => (Color) this.GetValue(PdfViewer.PageSeparatorColorProperty);
    set => this.SetValue(PdfViewer.PageSeparatorColorProperty, (object) value);
  }

  public bool ShowPageSeparator
  {
    get => (bool) this.GetValue(PdfViewer.ShowPageSeparatorProperty);
    set => this.SetValue(PdfViewer.ShowPageSeparatorProperty, (object) value);
  }

  public Color CurrentPageHighlightColor
  {
    get => (Color) this.GetValue(PdfViewer.CurrentPageHighlightColorProperty);
    set => this.SetValue(PdfViewer.CurrentPageHighlightColorProperty, (object) value);
  }

  public bool ShowCurrentPageHighlight
  {
    get => (bool) this.GetValue(PdfViewer.ShowCurrentPageHighlightProperty);
    set => this.SetValue(PdfViewer.ShowCurrentPageHighlightProperty, (object) value);
  }

  public VerticalAlignment PageVAlign
  {
    get => (VerticalAlignment) this.GetValue(PdfViewer.PageVAlignProperty);
    set => this.SetValue(PdfViewer.PageVAlignProperty, (object) value);
  }

  public HorizontalAlignment PageHAlign
  {
    get => (HorizontalAlignment) this.GetValue(PdfViewer.PageHAlignProperty);
    set => this.SetValue(PdfViewer.PageHAlignProperty, (object) value);
  }

  public RenderFlags RenderFlags
  {
    get => (RenderFlags) this.GetValue(PdfViewer.RenderFlagsProperty);
    set => this.SetValue(PdfViewer.RenderFlagsProperty, (object) value);
  }

  public int TilesCount
  {
    get => (int) this.GetValue(PdfViewer.TilesCountProperty);
    set => this.SetValue(PdfViewer.TilesCountProperty, (object) value);
  }

  public MouseModes MouseMode
  {
    get => (MouseModes) this.GetValue(PdfViewer.MouseModeProperty);
    set => this.SetValue(PdfViewer.MouseModeProperty, (object) value);
  }

  public bool ShowLoadingIcon
  {
    get => (bool) this.GetValue(PdfViewer.ShowLoadingIconProperty);
    set => this.SetValue(PdfViewer.ShowLoadingIconProperty, (object) value);
  }

  public bool UseProgressiveRender
  {
    get => (bool) this.GetValue(PdfViewer.UseProgressiveRenderProperty);
    set => this.SetValue(PdfViewer.UseProgressiveRenderProperty, (object) value);
  }

  public string LoadingIconText
  {
    get => (string) this.GetValue(PdfViewer.LoadingIconTextProperty);
    set => this.SetValue(PdfViewer.LoadingIconTextProperty, (object) value);
  }

  public bool PageAutoDispose
  {
    get => (bool) this.GetValue(PdfViewer.PageAutoDisposeProperty);
    set => this.SetValue(PdfViewer.PageAutoDisposeProperty, (object) value);
  }

  public int OptimizedLoadThreshold
  {
    get => (int) this.GetValue(PdfViewer.OptimizedLoadThresholdProperty);
    set => this.SetValue(PdfViewer.OptimizedLoadThresholdProperty, (object) value);
  }

  public double FlyoutExtentWidth
  {
    get => (double) this.GetValue(PdfViewer.FlyoutExtentWidthProperty);
    set => this.SetValue(PdfViewer.FlyoutExtentWidthProperty, (object) value);
  }

  public bool IsAnnotationVisible
  {
    get => (bool) this.GetValue(PdfViewer.IsAnnotationVisibleProperty);
    set => this.SetValue(PdfViewer.IsAnnotationVisibleProperty, (object) value);
  }

  public Brush PageMaskBrush
  {
    get => (Brush) this.GetValue(PdfViewer.PageMaskBrushProperty);
    set => this.SetValue(PdfViewer.PageMaskBrushProperty, (object) value);
  }

  public bool IsRenderPaused
  {
    get => (bool) this.GetValue(PdfViewer.IsRenderPausedProperty);
    set => this.SetValue(PdfViewer.IsRenderPausedProperty, (object) value);
  }

  public Cursor OverrideCursor
  {
    get => (Cursor) this.GetValue(PdfViewer.OverrideCursorProperty);
    set => this.SetValue(PdfViewer.OverrideCursorProperty, (object) value);
  }

  public bool IsLinkAnnotationHighlighted
  {
    get => (bool) this.GetValue(PdfViewer.IsLinkAnnotationHighlightedProperty);
    set => this.SetValue(PdfViewer.IsLinkAnnotationHighlightedProperty, (object) value);
  }

  public Color LinkAnnotationHighlightBorderColor
  {
    get => (Color) this.GetValue(PdfViewer.LinkAnnotationHighlightBorderColorProperty);
    set => this.SetValue(PdfViewer.LinkAnnotationHighlightBorderColorProperty, (object) value);
  }

  public Color LinkAnnotationHighlightFillColor
  {
    get => (Color) this.GetValue(PdfViewer.LinkAnnotationHighlightFillColorProperty);
    set => this.SetValue(PdfViewer.LinkAnnotationHighlightFillColorProperty, (object) value);
  }

  public bool IsFillFormHighlighted
  {
    get => (bool) this.GetValue(PdfViewer.IsFillFormHighlightedProperty);
    set => this.SetValue(PdfViewer.IsFillFormHighlightedProperty, (object) value);
  }

  public Color FillFormHighlightFocusedBorderColor
  {
    get => (Color) this.GetValue(PdfViewer.FillFormHighlightFocusedBorderColorProperty);
    set => this.SetValue(PdfViewer.FillFormHighlightFocusedBorderColorProperty, (object) value);
  }

  public Color FillFormHighlightFillColor
  {
    get => (Color) this.GetValue(PdfViewer.FillFormHighlightFillColorProperty);
    set => this.SetValue(PdfViewer.FillFormHighlightFillColorProperty, (object) value);
  }

  public PdfForms FillForms
  {
    get
    {
      return this.Document == null || this.Document.FormFill == null ? this._fillForms : this.Document.FormFill;
    }
  }

  public SelectInfo SelectInfo => this.NormalizeSelectionInfo();

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

  [Obsolete("This property is ignored now", false)]
  [ReadOnly(true)]
  [Browsable(false)]
  public bool AllowSetDocument { get; set; }

  public SortedDictionary<int, List<HighlightInfo>> HighlightedTextInfo => this._highlightedText;

  public int LastRenderingMilliseconds => this.lastRenderingTime;

  public SortedDictionary<int, FS_RECTF> PageCropBoxInfo
  {
    get => this._pagesCropBoxInfo;
    set => this._pagesCropBoxInfo = value;
  }

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

  public void SelectText(SelectInfo SelInfo)
  {
    this.SelectText(SelInfo.StartPage, SelInfo.StartIndex, SelInfo.EndPage, SelInfo.EndIndex);
  }

  public void SelectText(int startPage, int startIndex, int endPage, int endIndex)
  {
    if (this.Document == null)
      return;
    if (startPage > this.Document.Pages.Count - 1)
      startPage = this.Document.Pages.Count - 1;
    if (startPage < 0)
      startPage = 0;
    if (endPage > this.Document.Pages.Count - 1)
      endPage = this.Document.Pages.Count - 1;
    if (endPage < 0)
      endPage = 0;
    int countChars1 = this.Document.Pages[startPage].Text.CountChars;
    int countChars2 = this.Document.Pages[endPage].Text.CountChars;
    if (startIndex > countChars1 - 1)
      startIndex = countChars1 - 1;
    if (startIndex < 0)
      startIndex = 0;
    if (endIndex > countChars2 - 1)
      endIndex = countChars2 - 1;
    if (endIndex < 0)
      endIndex = 0;
    this._selectInfo = new SelectInfo()
    {
      StartPage = startPage,
      StartIndex = startIndex,
      EndPage = endPage,
      EndIndex = endIndex
    };
    this._isShowSelection = true;
    this.InvalidateVisual();
    this.GenerateSelectedTextProperty();
  }

  public void DeselectText()
  {
    this._selectInfo = new SelectInfo() { StartPage = -1 };
    this.InvalidateVisual();
    this.GenerateSelectedTextProperty();
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
      throw new IndexOutOfRangeException(PDFKit.Properties.Resources.err0002);
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
      throw new IndexOutOfRangeException(PDFKit.Properties.Resources.err0002);
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

  public void HighlightText(int pageIndex, HighlightInfo highlightInfo)
  {
    this.HighlightText(pageIndex, highlightInfo.CharIndex, highlightInfo.CharsCount, highlightInfo.Color);
  }

  public void HighlightText(int pageIndex, int charIndex, int charsCount, Color color)
  {
    this.HighlightText(pageIndex, charIndex, charsCount, color, new FS_RECTF());
  }

  public void HighlightText(
    int pageIndex,
    int charIndex,
    int charsCount,
    Color color,
    FS_RECTF inflate)
  {
    if (pageIndex < 0)
      pageIndex = 0;
    if (pageIndex > this.Document.Pages.Count - 1)
      pageIndex = this.Document.Pages.Count - 1;
    IntPtr page = Pdfium.FPDF_LoadPage(this.Document.Handle, pageIndex);
    IntPtr text_page = Pdfium.FPDFText_LoadPage(page);
    int num = Pdfium.FPDFText_CountChars(text_page);
    Pdfium.FPDFText_ClosePage(text_page);
    Pdfium.FPDF_ClosePage(page);
    if (charIndex < 0)
      charIndex = 0;
    if (charIndex > num - 1)
      charIndex = num - 1;
    if (charsCount < 0)
      charsCount = num - charIndex;
    if (charIndex + charsCount > num)
      charsCount = num - 1 - charIndex;
    if (charsCount <= 0)
      return;
    HighlightInfo addingEntry = new HighlightInfo()
    {
      CharIndex = charIndex,
      CharsCount = charsCount,
      Color = color,
      Inflate = inflate
    };
    if (!this._highlightedText.ContainsKey(pageIndex))
    {
      if (color != Helpers.ColorEmpty)
      {
        this._highlightedText.Add(pageIndex, new List<HighlightInfo>());
        this._highlightedText[pageIndex].Add(addingEntry);
      }
    }
    else
    {
      List<HighlightInfo> highlightInfoList = this._highlightedText[pageIndex];
      for (int index1 = highlightInfoList.Count - 1; index1 >= 0; --index1)
      {
        List<HighlightInfo> calcEntries;
        if (this.CalcIntersectEntries(highlightInfoList[index1], addingEntry, out calcEntries))
        {
          if (calcEntries.Count == 0)
          {
            highlightInfoList.RemoveAt(index1);
          }
          else
          {
            for (int index2 = 0; index2 < calcEntries.Count; ++index2)
            {
              if (index2 == 0)
                highlightInfoList[index1] = calcEntries[index2];
              else
                highlightInfoList.Insert(index1, calcEntries[index2]);
            }
          }
        }
      }
      if (color != Helpers.ColorEmpty)
        highlightInfoList.Add(addingEntry);
    }
    this.InvalidateVisual();
    this.OnHighlightedTextChanged(EventArgs.Empty);
  }

  public void RemoveHighlightFromText()
  {
    this._highlightedText.Clear();
    this.InvalidateVisual();
  }

  public void RemoveHighlightFromText(int pageIndex, int charIndex, int charsCount)
  {
    this.HighlightText(pageIndex, charIndex, charsCount, Helpers.ColorEmpty);
  }

  [Obsolete("This method is obsolete. Please use HighlightSelectedText instead", false)]
  public void HilightSelectedText(Color color) => this.HighlightSelectedText(color);

  public void HighlightSelectedText(Color color)
  {
    SelectInfo selectInfo = this.SelectInfo;
    if (selectInfo.StartPage < 0 || selectInfo.StartIndex < 0)
      return;
    for (int startPage = selectInfo.StartPage; startPage <= selectInfo.EndPage; ++startPage)
    {
      int startIndex = startPage == selectInfo.StartPage ? selectInfo.StartIndex : 0;
      int charsCount = startPage == selectInfo.EndPage ? selectInfo.EndIndex + 1 - startIndex : -1;
      this.HighlightText(startPage, startIndex, charsCount, color);
    }
  }

  [Obsolete("This method is obsolete. Please use RemoveHighlightFromSelectedText instead", false)]
  public void RemoveHilightFromSelectedText() => this.RemoveHighlightFromSelectedText();

  public void RemoveHighlightFromSelectedText() => this.HighlightSelectedText(Helpers.ColorEmpty);

  public void UpdateDocLayout()
  {
    this._prPages.ReleaseCanvas();
    this._viewport = new Size(this.ActualWidth, this.ActualHeight);
    this.mouseOverAnnot = (-1, -1, 0);
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

  public List<Int32Rect> GetSelectedRects(int pageIndex)
  {
    return this.GetSelectedRects(pageIndex, this.SelectInfo);
  }

  public List<Int32Rect> GetSelectedRects(int pageIndex, SelectInfo selInfo)
  {
    if (pageIndex < selInfo.StartPage || pageIndex > selInfo.EndPage)
      return new List<Int32Rect>();
    int countChars = this.Document.Pages[pageIndex].Text.CountChars;
    int num = 0;
    if (pageIndex == selInfo.StartPage)
      num = selInfo.StartIndex;
    int len1 = countChars;
    if (pageIndex == selInfo.EndPage)
      len1 = selInfo.EndIndex + 1 - num;
    int s = num + len1;
    int len2 = countChars - s;
    IEnumerable<FS_RECTF> withoutSpaceCharacter1 = this.GetRectsFromTextInfoWithoutSpaceCharacter(pageIndex, num, len1);
    IEnumerable<FS_RECTF> withoutSpaceCharacter2 = this._smoothSelection != PdfViewer.SmoothSelection.ByLine || num <= 0 ? (IEnumerable<FS_RECTF>) null : this.GetRectsFromTextInfoWithoutSpaceCharacter(pageIndex, 0, num);
    IEnumerable<FS_RECTF> withoutSpaceCharacter3 = this._smoothSelection != PdfViewer.SmoothSelection.ByLine || s >= countChars || len2 <= 0 ? (IEnumerable<FS_RECTF>) null : this.GetRectsFromTextInfoWithoutSpaceCharacter(pageIndex, s, len2);
    return this.NormalizeRects(withoutSpaceCharacter1, pageIndex, withoutSpaceCharacter2, withoutSpaceCharacter3);
  }

  public List<Int32Rect> GetHighlightedRects(int pageIndex, HighlightInfo selInfo)
  {
    int countChars = this.Document.Pages[pageIndex].Text.CountChars;
    int charIndex = selInfo.CharIndex;
    int charsCount = selInfo.CharsCount;
    int s = charIndex + charsCount;
    int len = countChars - s;
    IEnumerable<FS_RECTF> withoutSpaceCharacter1 = this.GetRectsFromTextInfoWithoutSpaceCharacter(pageIndex, charIndex, charsCount);
    if (!(selInfo.Inflate == new FS_RECTF()))
      return this.NormalizeRects(withoutSpaceCharacter1, pageIndex, (IEnumerable<FS_RECTF>) null, (IEnumerable<FS_RECTF>) null, selInfo.Inflate);
    IEnumerable<FS_RECTF> withoutSpaceCharacter2 = this._smoothSelection != PdfViewer.SmoothSelection.ByLine || charIndex <= 0 ? (IEnumerable<FS_RECTF>) null : this.GetRectsFromTextInfoWithoutSpaceCharacter(pageIndex, 0, charIndex);
    IEnumerable<FS_RECTF> withoutSpaceCharacter3 = this._smoothSelection != PdfViewer.SmoothSelection.ByLine || s >= countChars || len <= 0 ? (IEnumerable<FS_RECTF>) null : this.GetRectsFromTextInfoWithoutSpaceCharacter(pageIndex, s, len);
    return this.NormalizeRects(withoutSpaceCharacter1, pageIndex, withoutSpaceCharacter2, withoutSpaceCharacter3, selInfo.Inflate);
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
      int num = (int) MessageBox.Show(ex.Message, PDFKit.Properties.Resources.InfoHeader, MessageBoxButton.OK, MessageBoxImage.Asterisk);
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
      int num = (int) MessageBox.Show(ex.Message, PDFKit.Properties.Resources.InfoHeader, MessageBoxButton.OK, MessageBoxImage.Asterisk);
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
      int num = (int) MessageBox.Show(ex.Message, PDFKit.Properties.Resources.InfoHeader, MessageBoxButton.OK, MessageBoxImage.Asterisk);
    }
    finally
    {
      if (this.Document == null && pdfDocument != null)
        pdfDocument.Dispose();
    }
  }

  public void CloseDocument() => this.Document = (PdfDocument) null;

  static PdfViewer()
  {
    Style defaultValue = new Style();
    defaultValue.Setters.Add((SetterBase) new Setter(Control.TemplateProperty, (object) new ControlTemplate()));
    defaultValue.Seal();
    FrameworkElement.FocusVisualStyleProperty.OverrideMetadata(typeof (PdfViewer), (PropertyMetadata) new FrameworkPropertyMetadata((object) defaultValue));
  }

  public PdfViewer()
  {
    this.LoadingIconText = PDFKit.Properties.Resources.LoadingText;
    this.Background = (Brush) SystemColors.ControlDarkBrush;
    this._smoothSelection = PdfViewer.SmoothSelection.ByLine;
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
    this.decorators = new PdfViewerDecoratorCollection(this)
    {
      (IPdfViewerDecorator) new PdfLinkAnnotationDecorator()
      {
        IsEnabled = this.IsLinkAnnotationHighlighted,
        BorderColor = this.LinkAnnotationHighlightBorderColor,
        FillColor = this.LinkAnnotationHighlightFillColor,
        BorderThickness = 1.0
      },
      (IPdfViewerDecorator) new PdfFormFieldDecorator()
      {
        IsEnabled = this.IsFillFormHighlighted,
        FillColor = this.FillFormHighlightFillColor,
        FocusBorderColor = this.FillFormHighlightFocusedBorderColor,
        FocusBorderThickness = 1.0
      }
    };
    this.decoratorDrawingArgs = new PdfViewerDecoratorDrawingArgs()
    {
      Viewer = this
    };
    this.Loaded += (RoutedEventHandler) ((s, a) =>
    {
      this.mouseWheelHelper?.Dispose();
      this.mouseWheelHelper = new MouseWheelHelper((Visual) this);
      this.mouseWheelHelper.Throttled = true;
      this.mouseWheelHelper.MouseTilt += new EventHandler<MouseTiltEventArgs>(this.MouseWheelHelper_MouseTilt);
    });
    this.Unloaded += (RoutedEventHandler) ((s, a) =>
    {
      this.mouseWheelHelper.MouseTilt -= new EventHandler<MouseTiltEventArgs>(this.MouseWheelHelper_MouseTilt);
      this.mouseWheelHelper?.Dispose();
      this.mouseWheelHelper = (MouseWheelHelper) null;
    });
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
    SelectInfo selInfo = this.NormalizeSelectionInfo();
    List<Point> separator = new List<Point>();
    Rect clientRect1 = this.ClientRect;
    int pixels1 = Helpers.UnitsToPixels((DependencyObject) this, clientRect1.Width);
    clientRect1 = this.ClientRect;
    int pixels2 = Helpers.UnitsToPixels((DependencyObject) this, clientRect1.Height);
    if (pixels1 <= 0 || pixels2 <= 0)
      return;
    if (this.lastRenderingTime != int.MaxValue)
      this.lastRenderCompletedStamp = Stopwatch.GetTimestamp();
    this.decoratorDrawingArgs.Reset();
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
          PageDisposeHelper.DisposePage(page);
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
        this.RegenerateAnnots(startPage);
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
          this.decoratorDrawingArgs.Reset();
          this.decoratorDrawingArgs.PdfPage = this.Document.Pages[startPage];
          this.decoratorDrawingArgs.PdfBitmap = pdfBitmap;
          this.decoratorDrawingArgs.PageInViewerActualRect = actualRect;
          this.DrawDecorators(this.decoratorDrawingArgs);
          this.DrawFillForms(pdfBitmap, this.Document.Pages[startPage], actualRect);
          this.DrawFillFormsSelection(pdfBitmap, this._selectedRectangles);
          if (this._highlightedText.ContainsKey(startPage))
            this.DrawTextHighlight(pdfBitmap, this._highlightedText[startPage], startPage);
          this.DrawTextSelection(pdfBitmap, selInfo, startPage);
        }
        this.CalcPageSeparator(actualRect, startPage, ref separator);
      }
    }
    if (allPagesAreRendered)
    {
      long timestamp = Stopwatch.GetTimestamp();
      if (this.lastRenderCompletedStamp != -1L)
        this.lastRenderingTime = flag1 ? (int) ((timestamp - this.lastRenderCompletedStamp) / 10000L) : 0;
      this.lastRenderCompletedStamp = timestamp;
      this.cachedCanvasBitmap?.Dispose();
      this.cachedCanvasBitmap = (CachedCanvasBitmap) null;
      if (this._prPages?.CanvasBitmap != null)
        this.cachedCanvasBitmap = new CachedCanvasBitmap(this._prPages.CanvasBitmap, (IPdfScrollInfo) this, pageRects);
    }
    else
    {
      this.lastRenderingTime = int.MaxValue;
      if (this.cachedCanvasBitmap != null && this.cachedCanvasBitmap.Zoom != (double) this.Zoom)
      {
        this.cachedCanvasBitmap?.Dispose();
        this.cachedCanvasBitmap = (CachedCanvasBitmap) null;
      }
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
        if (this._pagesCropBoxInfo.ContainsKey(startPage))
        {
          FS_RECTF pageRect = this._pagesCropBoxInfo[startPage];
          Rect clientRect2;
          if (this.TryGetClientRect(startPage, pageRect, out clientRect2))
            this.DrawPageCropBox(drawingContext, clientRect2);
        }
        this.DrawFillFormsSelection(drawingContext, this._selectedRectangles);
        if (this._highlightedText.ContainsKey(startPage))
          this.DrawTextHighlight(drawingContext, this._highlightedText[startPage], startPage);
        this.DrawTextSelection(drawingContext, selInfo, startPage);
        this.decoratorDrawingArgs.Reset();
        this.decoratorDrawingArgs.PdfPage = this.Document.Pages[startPage];
        this.decoratorDrawingArgs.DrawingContext = drawingContext;
        this.DrawDecorators(this.decoratorDrawingArgs);
        this.DrawCurrentPageHighlight(drawingContext, startPage, rect);
        this.DrawPageMask(drawingContext, startPage, rect);
      }
    }
    if (!allPagesAreRendered)
      this.StartInvalidateTimer();
    else if ((this.RenderFlags & RenderFlags.FPDF_THUMBNAIL) != 0)
      this._prPages.ReleaseCanvas();
    else if (!this.UseProgressiveRender)
      this._prPages.ReleaseCanvas();
    this._selectedRectangles.Clear();
  }

  protected virtual void RegenerateAnnots(int pageIndex)
  {
    IntPtr handle = this.Document.Pages[pageIndex].Handle;
    if (!Pdfium.IsFullAPI)
      return;
    uint[] annotsWithoutAp = Pdfium.FPDFTOOLS_GetAnnotsWithoutAP(handle);
    if (annotsWithoutAp == null || annotsWithoutAp.Length == 0)
      return;
    PdfAnnotationCollection annots = this.Document.Pages[pageIndex].Annots;
    int count = annots.Count;
    foreach (int index in annotsWithoutAp)
    {
      if (index >= 0 && index < count)
      {
        try
        {
          switch (annots[index])
          {
            case PdfHighlightAnnotation highlight:
              highlight.RegenerateAppearancesWithoutRound();
              break;
            case PdfFreeTextAnnotation annot1:
              annot1.RegenerateAppearancesWithRichText(false);
              break;
            case PdfTextAnnotation text:
              text.RegenerateAppearancesAdvance();
              break;
            case PdfStampAnnotation annot2:
              annot2.RegenerateAppearancesAdvance();
              break;
            case PdfMarkupAnnotation markupAnnotation:
              markupAnnotation.RegenerateAppearances();
              break;
          }
        }
        catch (UnexpectedTypeException ex)
        {
          GAManager.SendEvent("Exception", "UnexpectedTypeException1", ex.Message, 1L);
        }
      }
    }
  }

  protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
  {
    if (e.LeftButton == MouseButtonState.Pressed && this.Document != null)
    {
      Point position = e.GetPosition((IInputElement) this);
      Point pagePoint;
      int page = this.DeviceToPage(position.X, position.Y, out pagePoint);
      if (page >= 0)
      {
        switch (this.MouseMode)
        {
          case MouseModes.Default:
          case MouseModes.SelectTextTool:
            this.ProcessMouseDoubleClickForSelectTextTool(pagePoint, page);
            break;
        }
      }
    }
    base.OnMouseDoubleClick(e);
  }

  protected override void OnMouseDown(MouseButtonEventArgs e)
  {
    Point? page_point = new Point?();
    Point? loc = new Point?();
    int? idx = new int?();
    Action action = (Action) (() =>
    {
      if (page_point.HasValue)
        return;
      loc = new Point?(e.GetPosition((IInputElement) this));
      Point pagePoint;
      idx = new int?(this.DeviceToPage(loc.Value.X, loc.Value.Y, out pagePoint));
      page_point = new Point?(pagePoint);
    });
    this._mousePressedInLink = false;
    if (this.MouseMode != MouseModes.PanTool && this.MouseMode != MouseModes.None && (e.ChangedButton == MouseButton.Left || e.ChangedButton == MouseButton.Right) && this.Document != null)
    {
      action();
      int? nullable = idx;
      int num = 0;
      if (nullable.GetValueOrDefault() >= num & nullable.HasValue && this.ProcessAnnotMouseClickEvent(idx.Value, page_point.Value, e))
        e.Handled = true;
    }
    if (!e.Handled && e.ChangedButton == MouseButton.Left && this.Document != null)
    {
      action();
      int? nullable = idx;
      int num = 0;
      if (nullable.GetValueOrDefault() >= num & nullable.HasValue)
      {
        if (this.MouseMode != MouseModes.PanTool && this.MouseMode != MouseModes.None)
          this.Document.Pages[idx.Value].OnLButtonDown(0, (float) page_point.Value.X, (float) page_point.Value.Y);
        this.SetCurrentPage(idx.Value);
        this._mousePressed = true;
        switch (this.MouseMode)
        {
          case MouseModes.Default:
            this.ProcessMouseDownDefaultTool(page_point.Value, idx.Value);
            this.ProcessMouseDownForSelectTextTool(page_point.Value, idx.Value);
            break;
          case MouseModes.SelectTextTool:
            this.ProcessMouseDownForSelectTextTool(page_point.Value, idx.Value);
            break;
          case MouseModes.PanTool:
            this.ProcessMouseDownPanTool(loc.Value);
            break;
        }
        this.InvalidateVisual();
      }
      else if (this.MouseMode == MouseModes.PanTool)
      {
        Rect rect1 = Rect.Empty;
        Rect clientRect = this.ClientRect;
        for (int startPage = this._startPage; startPage <= this._endPage; ++startPage)
        {
          Rect rect2 = this.CalcActualRect(startPage);
          if (!rect2.IntersectsWith(clientRect))
          {
            if (!rect1.IsEmpty)
              break;
          }
          else
          {
            if (rect1.IsEmpty)
            {
              rect1 = rect2;
            }
            else
            {
              rect1.X = Math.Min(rect1.X, rect2.X);
              rect1.Y = Math.Min(rect1.Y, rect2.Y);
              rect1.Width = Math.Max(rect1.Width, rect2.Right - rect1.X);
              rect1.Height = Math.Max(rect1.Height, rect2.Bottom - rect1.Y);
            }
            if (rect1.Contains(loc.Value))
              break;
          }
        }
        if (rect1.Contains(loc.Value))
        {
          this._mousePressed = true;
          this.ProcessMouseDownPanTool(loc.Value);
        }
      }
    }
    base.OnMouseDown(e);
  }

  private bool ProcessAnnotMouseClickEvent(
    int pageIndex,
    Point pagePoint,
    MouseButtonEventArgs rawEvent)
  {
    PdfAnnotation pdfAnnotation = (PdfAnnotation) null;
    Rect annotBounds;
    if (this.ProcessAnnotMouseMoveEvent(pageIndex, pagePoint, (MouseEventArgs) rawEvent, false, out annotBounds))
    {
      (int num1, int num2, int annotCount) = this.mouseOverAnnot;
      int? count = this.Document.Pages[num1].Annots?.Count;
      int valueOrDefault = count.GetValueOrDefault();
      if (!(annotCount == valueOrDefault & count.HasValue) && num2 >= 0)
        pdfAnnotation = this.Document.Pages[num1].Annots[num2];
    }
    if ((PdfWrapper) pdfAnnotation == (PdfWrapper) null)
      pdfAnnotation = this.GetPointAnnotation(this.Document.Pages[pageIndex], pagePoint, out int _);
    if (!((PdfWrapper) pdfAnnotation != (PdfWrapper) null))
      return false;
    AnnotationMouseClickEventArgs e = new AnnotationMouseClickEventArgs(pdfAnnotation, annotBounds, rawEvent, pageIndex, pagePoint);
    this.OnAnnotationMouseClick(e);
    return e.Handled;
  }

  private bool ProcessAnnotMouseMoveEvent(int pageIndex, Point pagePoint, MouseEventArgs rawEvent)
  {
    return this.ProcessAnnotMouseMoveEvent(pageIndex, pagePoint, rawEvent, true, out Rect _);
  }

  private bool ProcessAnnotMouseMoveEvent(
    int pageIndex,
    Point pagePoint,
    MouseEventArgs rawEvent,
    bool processMoveEvent,
    out Rect annotBounds)
  {
    annotBounds = new Rect();
    (int num1, int num2, int annotCount) = this.mouseOverAnnot;
    int index;
    PdfAnnotation pointAnnotation = this.GetPointAnnotation(this.Document.Pages[pageIndex], pagePoint, out index);
    Rect boundsInControl = new Rect();
    if (num1 != pageIndex || num2 != index)
    {
      if (num1 != -1 && num2 != -1)
      {
        PdfAnnotationCollection annots = this.Document.Pages[num1].Annots;
        // ISSUE: explicit non-virtual call
        if (annots != null && __nonvirtual (annots.Count) == annotCount)
        {
          PdfAnnotation annot = this.Document.Pages[num1].Annots[num2];
          Rect deviceBounds = Extensions.GetDeviceBounds(this, annot);
          this.OnAnnotationMouseExited(new AnnotationMouseEventArgs(annot, deviceBounds, rawEvent, pageIndex, pagePoint));
        }
      }
      int num3 = pageIndex;
      int num4 = index;
      PdfAnnotationCollection annots1 = this.Document.Pages[pageIndex].Annots;
      // ISSUE: explicit non-virtual call
      int count = annots1 != null ? __nonvirtual (annots1.Count) : 0;
      this.mouseOverAnnot = (num3, num4, count);
      if ((PdfWrapper) pointAnnotation != (PdfWrapper) null)
      {
        Rect deviceBounds = Extensions.GetDeviceBounds(this, pointAnnotation);
        this.OnAnnotationMouseEntered(new AnnotationMouseEventArgs(pointAnnotation, deviceBounds, rawEvent, pageIndex, pagePoint));
        annotBounds = deviceBounds;
      }
      return true;
    }
    if (processMoveEvent && (PdfWrapper) pointAnnotation != (PdfWrapper) null)
    {
      AnnotationMouseEventArgs e = new AnnotationMouseEventArgs(pointAnnotation, boundsInControl, rawEvent, pageIndex, pagePoint);
      this.OnAnnotationMouseMoved(e);
      if (e.Handled)
        return true;
    }
    return false;
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
        switch (this.MouseMode)
        {
          case MouseModes.Default:
            CursorTypes cursorTypes1 = this.ProcessMouseMoveForDefaultTool(pagePoint, page);
            CursorTypes cursorTypes2 = this.ProcessMouseMoveForSelectTextTool(pagePoint, page);
            cursor = cursorTypes2 == CursorTypes.Arrow || !this._mousePressed || this._mousePressedInLink ? (cursorTypes1 == CursorTypes.Arrow ? cursorTypes2 : cursorTypes1) : cursorTypes2;
            this.ProcessAnnotMouseMoveEvent(page, pagePoint, e);
            break;
          case MouseModes.SelectTextTool:
            cursor = this.ProcessMouseMoveForSelectTextTool(pagePoint, page);
            break;
          case MouseModes.PanTool:
            panTool = true;
            cursor = this.ProcessMouseMoveForPanTool(position);
            break;
        }
        if (this.OverrideCursor == null)
          this.InternalSetCursor(cursor, panTool);
      }
      else if (this.MouseMode == MouseModes.PanTool)
      {
        if (this._mousePressed && this.IsMouseCaptured)
        {
          bool panTool = true;
          CursorTypes cursor = this.ProcessMouseMoveForPanTool(position);
          if (this.OverrideCursor == null)
            this.InternalSetCursor(cursor, panTool);
        }
        else
          this.Cursor = this.OverrideCursor;
      }
      else
        this.Cursor = this.OverrideCursor;
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
    this._mousePressed = false;
    if (this.Document != null)
    {
      Point position = e.GetPosition((IInputElement) this);
      Point pagePoint;
      int page = this.DeviceToPage(position.X, position.Y, out pagePoint);
      if (this._selectInfo.StartPage >= 0)
        this.GenerateSelectedTextProperty();
      if (page >= 0)
      {
        if (this.MouseMode != MouseModes.PanTool && this.MouseMode != MouseModes.None)
          this.Document.Pages[page].OnLButtonUp(0, (float) pagePoint.X, (float) pagePoint.Y);
        switch (this.MouseMode)
        {
          case MouseModes.Default:
            this.ProcessMouseUpForDefaultTool(pagePoint, page);
            break;
          case MouseModes.PanTool:
            this.ProcessMouseUpPanTool(position);
            break;
        }
      }
      else
        this.Document?.FormFill?.ForceToKillFocus();
    }
    base.OnMouseUp(e);
  }

  protected override void OnLostMouseCapture(MouseEventArgs e)
  {
    if (this.MouseMode == MouseModes.PanTool)
    {
      this._mousePressed = false;
      this.ProcessMouseUpPanTool(e.GetPosition((IInputElement) this));
    }
    base.OnLostMouseCapture(e);
  }

  protected override void OnPreviewKeyDown(KeyEventArgs e)
  {
    if (this.Document != null)
    {
      FWL_VKEYCODE keyCode = (FWL_VKEYCODE) KeyInterop.VirtualKeyFromKey(e.Key);
      KeyboardModifiers modifier = (KeyboardModifiers) 0;
      if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
        modifier |= KeyboardModifiers.ControlKey;
      if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
        modifier |= KeyboardModifiers.ShiftKey;
      if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
        modifier |= KeyboardModifiers.AltKey;
      if (this.Document.Pages.CurrentPage.OnKeyDown(keyCode, modifier))
        e.Handled = true;
    }
    base.OnPreviewKeyDown(e);
  }

  protected override void OnKeyDown(KeyEventArgs e)
  {
    if (this.Document != null)
      ScrollInDirection(e);
    base.OnKeyDown(e);

    void ScrollInDirection(KeyEventArgs args)
    {
      bool flag1 = (args.KeyboardDevice.Modifiers & ModifierKeys.Control) != 0;
      if ((args.KeyboardDevice.Modifiers & ModifierKeys.Alt) != 0)
        return;
      bool flag2 = this.FlowDirection == FlowDirection.RightToLeft;
      switch (args.Key)
      {
        case Key.Prior:
          this.PageUp();
          args.Handled = true;
          break;
        case Key.Next:
          this.PageDown();
          args.Handled = true;
          break;
        case Key.End:
          if (flag1)
            this.ScrollOwner?.ScrollToBottom();
          else
            this.ScrollOwner?.ScrollToRightEnd();
          args.Handled = true;
          break;
        case Key.Home:
          if (flag1)
            this.ScrollOwner?.ScrollToTop();
          else
            this.ScrollOwner?.ScrollToLeftEnd();
          args.Handled = true;
          break;
        case Key.Left:
          if (flag2)
            this.LineRight();
          else
            this.LineLeft();
          args.Handled = true;
          break;
        case Key.Up:
          this.LineUp();
          args.Handled = true;
          break;
        case Key.Right:
          if (flag2)
            this.LineLeft();
          else
            this.LineRight();
          args.Handled = true;
          break;
        case Key.Down:
          this.LineDown();
          args.Handled = true;
          break;
      }
    }
  }

  protected override void OnKeyUp(KeyEventArgs e)
  {
    if (this.Document != null)
    {
      KeyboardModifiers modifier = (KeyboardModifiers) 0;
      if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
        modifier |= KeyboardModifiers.ControlKey;
      if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
        modifier |= KeyboardModifiers.ShiftKey;
      if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
        modifier |= KeyboardModifiers.AltKey;
      this.Document.Pages.CurrentPage.OnKeyUp((FWL_VKEYCODE) e.Key, modifier);
    }
    base.OnKeyUp(e);
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

  protected virtual void DrawTextHighlight(
    PdfBitmap bitmap,
    List<HighlightInfo> entries,
    int pageIndex)
  {
    if (entries == null)
      return;
    foreach (HighlightInfo entry in entries)
    {
      foreach (Int32Rect highlightedRect in this.GetHighlightedRects(pageIndex, entry))
        bitmap.FillRectEx(highlightedRect.X, highlightedRect.Y, highlightedRect.Width, highlightedRect.Height, Helpers.ToArgb(entry.Color), this.FormsBlendMode);
    }
  }

  protected virtual void DrawTextSelection(PdfBitmap bitmap, SelectInfo selInfo, int pageIndex)
  {
    if (selInfo.StartPage < 0 || !this._isShowSelection || pageIndex < selInfo.StartPage || pageIndex > selInfo.EndPage)
      return;
    foreach (Int32Rect selectedRect in this.GetSelectedRects(pageIndex, selInfo))
      bitmap.FillRectEx(selectedRect.X, selectedRect.Y, selectedRect.Width, selectedRect.Height, Helpers.ToArgb(this.TextSelectColor), this.FormsBlendMode);
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

  protected virtual void DrawPageBorder(DrawingContext drawingContext, Rect BBox)
  {
    Helpers.DrawRectangle(drawingContext, this._pageBorderColorPen, BBox);
  }

  protected virtual void DrawPageCropBox(DrawingContext drawingContext, Rect box)
  {
    Helpers.DrawRectangle(drawingContext, this._pageCropBoxColorPen, box);
  }

  protected virtual void DrawFillFormsSelection(
    DrawingContext drawingContext,
    List<Rect> selectedRectangles)
  {
  }

  protected virtual void DrawTextHighlight(
    DrawingContext drawingContext,
    List<HighlightInfo> entries,
    int pageIndex)
  {
  }

  protected virtual void DrawTextSelection(
    DrawingContext drawingContext,
    SelectInfo selInfo,
    int pageIndex)
  {
  }

  private void DrawDecorators(PdfViewerDecoratorDrawingArgs args)
  {
    if (this.Document == null || args.Viewer == null || args.PdfPage == null || args.DrawingContext == null && args.PdfBitmap == null)
      return;
    bool flag1 = args.DrawingContext != null;
    bool flag2 = args.PdfBitmap != null;
    for (int index = 0; index < this.decorators.Count; ++index)
    {
      if (flag2 && this.decorators[index].CanDrawPdfBitmap(args))
        this.decorators[index].DrawPdfBitmap(args);
      if (flag1 && this.decorators[index].CanDrawVisual(args))
        this.decorators[index].DrawVisual(args);
    }
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
    if (panTool)
    {
      if (this._mousePressed)
        this.Cursor = PdfViewer.panToolCursorHelper.HandClose;
      else
        this.Cursor = PdfViewer.panToolCursorHelper.Hand;
    }
    else
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
        default:
          this.Cursor = (Cursor) null;
          break;
      }
    }
  }

  internal bool CanDisposePage(int i)
  {
    return !this._highlightedText.ContainsKey(i) && (this._selectInfo.StartPage < 0 || this._selectInfo.EndPage < 0 || this._selectInfo.StartIndex < 0 || this._selectInfo.EndIndex < 0 || this._selectInfo.StartPage < i || this._selectInfo.EndPage > i);
  }

  internal void ForceRender()
  {
    this.cachedCanvasBitmap?.Dispose();
    this.cachedCanvasBitmap = (CachedCanvasBitmap) null;
    this._prPages?.ReleaseCanvas();
    this.InvalidateVisual();
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

  private void GenerateSelectedTextProperty()
  {
    string str = "";
    if (this.Document != null)
    {
      StringBuilder stringBuilder = new StringBuilder();
      SelectInfo selectInfo = this.NormalizeSelectionInfo();
      if (selectInfo.StartPage >= 0 && selectInfo.StartIndex >= 0)
      {
        for (int startPage = selectInfo.StartPage; startPage <= selectInfo.EndPage; ++startPage)
        {
          if (stringBuilder.Length > 0)
            stringBuilder.Append("\r\n");
          int index = 0;
          if (startPage == selectInfo.StartPage)
            index = selectInfo.StartIndex;
          int count = this.Document.Pages[startPage].Text.CountChars;
          if (startPage == selectInfo.EndPage)
            count = selectInfo.EndIndex + 1 - index;
          stringBuilder.Append(this.Document.Pages[startPage].Text.GetText(index, count));
        }
      }
      str = stringBuilder.ToString();
    }
    this.SetValue(PdfViewer.SelectedTextProperty, (object) str);
    this.OnSelectionChanged(EventArgs.Empty);
  }

  private PdfViewer.CaptureInfo CaptureFillForms(PdfForms fillForms)
  {
    PdfViewer.CaptureInfo captureInfo = new PdfViewer.CaptureInfo();
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

  private void ReleaseFillForms(PdfViewer.CaptureInfo captureInfo)
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
    captureInfo.forms.SetHighlightColorEx(FormFieldTypes.FPDF_FORMFIELD_UNKNOWN, captureInfo.color);
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

  private void ProcessLinkClicked(PdfLink pdfLink, PdfWebLink webLink)
  {
    PdfBeforeLinkClickedEventArgs e = new PdfBeforeLinkClickedEventArgs(webLink, pdfLink);
    this.OnBeforeLinkClicked(e);
    if (e.Cancel)
      return;
    if (pdfLink != null && pdfLink.Action != null)
      this.ProcessAction(pdfLink.Action);
    else if (pdfLink != null && pdfLink.Destination != null)
      this.ProcessDestination(pdfLink.Destination);
    else if (webLink != null)
    {
      try
      {
        Process.Start(webLink.Url);
      }
      catch (Exception ex)
      {
        if (ex is IOException || ex is Win32Exception)
        {
          int num = (int) MessageBox.Show(ex.Message, PDFKit.Properties.Resources.InfoHeader, MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
        else
          throw;
      }
    }
    this.OnAfterLinkClicked(new PdfAfterLinkClickedEventArgs(webLink, pdfLink));
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

  public virtual void ProcessAction(PdfAction pdfAction)
  {
    try
    {
      switch (pdfAction.ActionType - 1UL)
      {
        case ActionTypes.Unknown:
          this.ProcessDestination((pdfAction as PdfGoToAction).Destination);
          break;
        case ActionTypes.CurrentDoc:
          this.ProcesRemoteGotoAction((pdfAction as PdfGoToRAction).Destination, (pdfAction as PdfGoToRAction).FileSpecification);
          break;
        case ActionTypes.ExternalDoc:
          Process.Start(new ProcessStartInfo((pdfAction as PdfUriAction).ActionUrl)
          {
            UseShellExecute = true,
            Verb = "open"
          });
          break;
        case ActionTypes.Thread:
          this.ProcesRemoteGotoAction((pdfAction as PdfGoToEAction).Destination, (pdfAction as PdfGoToEAction).FileSpecification);
          break;
      }
    }
    catch (Exception ex)
    {
      int num1;
      switch (ex)
      {
        case IOException _:
        case UnauthorizedAccessException _:
        case ArgumentNullException _:
        case ArgumentException _:
        case NotSupportedException _:
        case InvalidOperationException _:
        case Win32Exception _:
          num1 = 1;
          break;
        default:
          num1 = ex is NoLicenseException ? 1 : 0;
          break;
      }
      if (num1 != 0)
      {
        int num2 = (int) MessageBox.Show(ex.Message, PDFKit.Properties.Resources.InfoHeader, MessageBoxButton.OK, MessageBoxImage.Asterisk);
      }
      else
        throw;
    }
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

  private SelectInfo NormalizeSelectionInfo()
  {
    SelectInfo selectInfo = this._selectInfo;
    if (selectInfo.StartPage >= 0 && selectInfo.EndPage >= 0)
    {
      if (selectInfo.StartPage > selectInfo.EndPage)
        selectInfo = new SelectInfo()
        {
          StartPage = selectInfo.EndPage,
          EndPage = selectInfo.StartPage,
          StartIndex = selectInfo.EndIndex,
          EndIndex = selectInfo.StartIndex
        };
      else if (selectInfo.StartPage == selectInfo.EndPage && selectInfo.StartIndex > selectInfo.EndIndex)
        selectInfo = new SelectInfo()
        {
          StartPage = selectInfo.StartPage,
          EndPage = selectInfo.EndPage,
          StartIndex = selectInfo.EndIndex,
          EndIndex = selectInfo.StartIndex
        };
    }
    return selectInfo;
  }

  private List<Int32Rect> NormalizeRects(
    IEnumerable<FS_RECTF> rects,
    int pageIndex,
    IEnumerable<FS_RECTF> rectsBefore,
    IEnumerable<FS_RECTF> rectsAfter)
  {
    return this.NormalizeRects(rects, pageIndex, rectsBefore, rectsAfter, new FS_RECTF());
  }

  private List<Int32Rect> NormalizeRects(
    IEnumerable<FS_RECTF> rects,
    int pageIndex,
    IEnumerable<FS_RECTF> rectsBefore,
    IEnumerable<FS_RECTF> rectsAfter,
    FS_RECTF inflate)
  {
    List<Int32Rect> rows = new List<Int32Rect>();
    if (this._smoothSelection == PdfViewer.SmoothSelection.None)
    {
      foreach (FS_RECTF rect in rects)
        rows.Add(this.PageToDevicePixelRect(rect, pageIndex));
      return rows;
    }
    int y;
    int x = y = int.MaxValue;
    int num1;
    int num2 = num1 = int.MinValue;
    float f = float.NaN;
    float num3 = float.NaN;
    foreach (FS_RECTF rect in rects)
    {
      rect.Inflate(inflate);
      float num4 = num3 - f;
      if (float.IsNaN(f))
      {
        f = rect.bottom;
        num3 = rect.top;
      }
      else if ((double) rect.top < (double) f + (double) num4 / 2.0 || (double) rect.bottom > (double) num3 - (double) num4 / 2.0)
      {
        rows.Add(new Int32Rect(x, y, num2 - x, num1 - y));
        f = rect.bottom;
        num3 = rect.top;
        x = y = int.MaxValue;
        num2 = num1 = int.MinValue;
      }
      else
      {
        if ((double) f > (double) rect.bottom)
          f = rect.bottom;
        if ((double) num3 < (double) rect.top)
          num3 = rect.top;
      }
      Int32Rect devicePixelRect = this.PageToDevicePixelRect(rect, pageIndex);
      if (x > devicePixelRect.X)
        x = devicePixelRect.X;
      if (num2 < devicePixelRect.X + devicePixelRect.Width)
        num2 = devicePixelRect.X + devicePixelRect.Width;
      if (y > devicePixelRect.Y)
        y = devicePixelRect.Y;
      if (num1 < devicePixelRect.Y + devicePixelRect.Height)
        num1 = devicePixelRect.Y + devicePixelRect.Height;
    }
    rows.Add(new Int32Rect(x, y, num2 - x, num1 - y));
    if (this._smoothSelection == PdfViewer.SmoothSelection.ByLine && rectsBefore != null)
      this.PadRectagles(pageIndex, rectsBefore, rows, true);
    if (this._smoothSelection == PdfViewer.SmoothSelection.ByLine && rectsAfter != null)
      this.PadRectagles(pageIndex, rectsAfter, rows, false);
    return rows;
  }

  private void PadRectagles(
    int pageIndex,
    IEnumerable<FS_RECTF> padRects,
    List<Int32Rect> rows,
    bool isLeft)
  {
    List<Int32Rect> int32RectList = this.NormalizeRects(padRects, pageIndex, (IEnumerable<FS_RECTF>) null, (IEnumerable<FS_RECTF>) null);
    if (int32RectList.Count <= 0)
      return;
    Int32Rect int32Rect = int32RectList[isLeft ? int32RectList.Count - 1 : 0];
    int num1 = int32Rect.Y + int32Rect.Height;
    Int32Rect row = rows[isLeft ? 0 : rows.Count - 1];
    int y1 = row.Y;
    int num2;
    if (num1 >= y1)
    {
      int y2 = int32Rect.Y;
      row = rows[isLeft ? 0 : rows.Count - 1];
      int y3 = row.Y;
      row = rows[isLeft ? 0 : rows.Count - 1];
      int height = row.Height;
      int num3 = y3 + height;
      num2 = y2 <= num3 ? 1 : 0;
    }
    else
      num2 = 0;
    if (num2 != 0)
    {
      row = rows[isLeft ? 0 : rows.Count - 1];
      int x1 = row.X;
      row = rows[isLeft ? 0 : rows.Count - 1];
      int x2 = row.X;
      row = rows[isLeft ? 0 : rows.Count - 1];
      int width = row.Width;
      int num4 = x2 + width;
      int y4 = int32Rect.Y;
      row = rows[isLeft ? 0 : rows.Count - 1];
      int y5 = row.Y;
      int y6 = Math.Min(y4, y5);
      int val1 = int32Rect.Y + int32Rect.Height;
      row = rows[isLeft ? 0 : rows.Count - 1];
      int y7 = row.Y;
      row = rows[isLeft ? 0 : rows.Count - 1];
      int height = row.Height;
      int val2 = y7 + height;
      int num5 = Math.Max(val1, val2);
      rows[isLeft ? 0 : rows.Count - 1] = new Int32Rect(x1, y6, num4 - x1, num5 - y6);
    }
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
      if (this.Document.Pages.CurrentIndex == index)
        return;
      int startPage = this._startPage;
      int endPage = this._endPage;
      if (PageDisposeHelper.TryFixPageAnnotations(this.Document, index) && this.Document.Pages[index].IsLoaded)
        this.Document.Pages[index].Dispose();
      this.Document.Pages.CurrentIndex = index;
      this.OnCurrentPageChanged(EventArgs.Empty);
      if (this.ViewMode == ViewModes.SinglePage || this.ViewMode == ViewModes.TilesLine)
        this.UpdateScrollBars(new Size(this._renderRects[this._endPage].Right + this.Padding.Right, this._renderRects[this.IdxWithLowestBottom(this._startPage, this._endPage)].Bottom + this.Padding.Bottom));
      if ((this.ViewMode == ViewModes.SinglePage || this.ViewMode == ViewModes.TilesLine) && this._startPage != startPage)
      {
        for (int index1 = startPage; index1 <= endPage; ++index1)
        {
          if (this.PageAutoDispose && this.CanDisposePage(index1))
            PageDisposeHelper.DisposePage(this.Document.Pages[index1]);
        }
      }
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

  private void StartInvalidateTimer()
  {
    if (this._invalidateTimer != null)
      return;
    this._invalidateTimer = new DispatcherTimer();
    this._invalidateTimer.Interval = TimeSpan.FromMilliseconds(10.0);
    this._invalidateTimer.Tick += (EventHandler) ((s, a) =>
    {
      if (!this._prPages.IsNeedContinuePaint)
      {
        this._invalidateTimer.Stop();
        this._invalidateTimer = (DispatcherTimer) null;
      }
      this.InvalidateVisual();
    });
    this._invalidateTimer.Start();
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

  private IEnumerable<FS_RECTF> GetRectsFromTextInfoWithoutSpaceCharacter(
    int pageIndex,
    int s,
    int len)
  {
    List<IEnumerable<FS_RECTF>> fsRectfsList = new List<IEnumerable<FS_RECTF>>();
    int index1 = -1;
    int count = 0;
    for (int index2 = s; index2 < s + len; ++index2)
    {
      if (this.Document.Pages[pageIndex].Text.GetCharacter(index2) == ' ')
      {
        if (index1 >= 0)
        {
          fsRectfsList.Add((IEnumerable<FS_RECTF>) this.Document.Pages[pageIndex].Text.GetTextInfo(index1, count).Rects);
          index1 = -1;
          count = 0;
        }
      }
      else if (index1 == -1)
      {
        index1 = index2;
        count = 1;
      }
      else
        ++count;
    }
    if (index1 >= 0)
      fsRectfsList.Add((IEnumerable<FS_RECTF>) this.Document.Pages[pageIndex].Text.GetTextInfo(index1, count).Rects);
    List<FS_RECTF> withoutSpaceCharacter = new List<FS_RECTF>();
    foreach (IEnumerable<FS_RECTF> collection in fsRectfsList)
      withoutSpaceCharacter.AddRange(collection);
    return (IEnumerable<FS_RECTF>) withoutSpaceCharacter;
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

  protected virtual void OnFormsSetCursor(SetCursorEventArgs e)
  {
    if (this.OverrideCursor != null)
      return;
    this.InternalSetCursor(e.Cursor);
  }

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
    try
    {
      PdfPage currentPage = this.Document.Pages.CurrentPage;
      if (currentPage.Annots != null && currentPage.Annots.Count > 0)
      {
        foreach (PdfTextAnnotation text in currentPage.Annots.OfType<PdfTextAnnotation>().Where<PdfTextAnnotation>((Func<PdfTextAnnotation, bool>) (c => c.Relationship == RelationTypes.Reply)))
          text.RegenerateAppearancesAdvance();
      }
    }
    catch
    {
    }
    if (this.ViewMode == ViewModes.SinglePage || this.ViewMode == ViewModes.TilesLine)
    {
      this._prPages.ReleaseCanvas();
      double right1 = this._renderRects[this._endPage].Right;
      Thickness padding = this.Padding;
      double right2 = padding.Right;
      double width = right1 + right2;
      double bottom1 = this._renderRects[this.IdxWithLowestBottom(this._startPage, this._endPage)].Bottom;
      padding = this.Padding;
      double bottom2 = padding.Bottom;
      double height = bottom1 + bottom2;
      this.UpdateScrollBars(new Size(width, height));
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

  private void ProcessMouseDoubleClickForSelectTextTool(Point page_point, int page_index)
  {
    PdfPage page = this.Document.Pages[page_index];
    int charIndexAtPos = page.Text.GetCharIndexAtPos((float) page_point.X, (float) page_point.Y, 10f, 10f);
    int si;
    int ei;
    if (!this.GetWord(page.Text, charIndexAtPos, out si, out ei))
      return;
    this._selectInfo = new SelectInfo()
    {
      StartPage = page_index,
      EndPage = page_index,
      StartIndex = si,
      EndIndex = ei
    };
    this._isShowSelection = true;
    if (this._selectInfo.StartPage >= 0)
      this.GenerateSelectedTextProperty();
    this.InvalidateVisual();
  }

  private void ProcessMouseDownForSelectTextTool(Point page_point, int page_index)
  {
    this._selectInfo = new SelectInfo()
    {
      StartPage = page_index,
      EndPage = page_index,
      StartIndex = this.Document.Pages[page_index].Text.GetCharIndexAtPos((float) page_point.X, (float) page_point.Y, 10f, 10f),
      EndIndex = -1
    };
    this._isShowSelection = false;
    if (this._selectInfo.StartPage < 0)
      return;
    this.GenerateSelectedTextProperty();
  }

  private CursorTypes ProcessMouseMoveForSelectTextTool(Point page_point, int page_index)
  {
    int charIndexAtPos = this.Document.Pages[page_index].Text.GetCharIndexAtPos((float) page_point.X, (float) page_point.Y, 10f, 10f);
    if (this._mousePressed)
    {
      if (charIndexAtPos >= 0)
      {
        this._selectInfo = new SelectInfo()
        {
          StartPage = this._selectInfo.StartPage,
          EndPage = page_index,
          EndIndex = charIndexAtPos,
          StartIndex = this._selectInfo.StartIndex
        };
        this._mousePressedInLink = false;
        this._isShowSelection = true;
      }
      this.InvalidateVisual();
    }
    if (!this.Document.Pages[page_index].OnMouseMove(0, (float) page_point.X, (float) page_point.Y) && charIndexAtPos >= 0)
      return CursorTypes.VBeam;
    switch (this.Document.FormFill != null ? this.Document.Pages[page_index].GetFormFieldAtPoint((float) page_point.X, (float) page_point.Y) : FormFieldTypes.FPDF_FORMFIELD_NOFIELDS)
    {
      case FormFieldTypes.FPDF_FORMFIELD_PUSHBUTTON:
      case FormFieldTypes.FPDF_FORMFIELD_CHECKBOX:
      case FormFieldTypes.FPDF_FORMFIELD_RADIOBUTTON:
      case FormFieldTypes.FPDF_FORMFIELD_COMBOBOX:
      case FormFieldTypes.FPDF_FORMFIELD_LISTBOX:
        return CursorTypes.Hand;
      case FormFieldTypes.FPDF_FORMFIELD_TEXTFIELD:
        return CursorTypes.VBeam;
      default:
        return CursorTypes.Arrow;
    }
  }

  private void ProcessMouseDownDefaultTool(Point page_point, int page_index)
  {
    if (this.IsMouseCaptured || this.IsLinkAnnotationHighlighted)
      return;
    PdfLink linkAtPoint = this.Document.Pages[page_index].Links.GetLinkAtPoint((float) page_point.X, (float) page_point.Y);
    if (this.Document.Pages[page_index].Text.WebLinks.GetWebLinkAtPoint((float) page_point.X, (float) page_point.Y) != null || linkAtPoint != null)
      this._mousePressedInLink = true;
    else
      this._mousePressedInLink = false;
  }

  private CursorTypes ProcessMouseMoveForDefaultTool(Point page_point, int page_index)
  {
    if (this.IsMouseCaptured || this.IsLinkAnnotationHighlighted)
      return CursorTypes.Arrow;
    PdfLink linkAtPoint = this.Document.Pages[page_index].Links.GetLinkAtPoint((float) page_point.X, (float) page_point.Y);
    return this.Document.Pages[page_index].Text.WebLinks.GetWebLinkAtPoint((float) page_point.X, (float) page_point.Y) != null || linkAtPoint != null ? CursorTypes.Hand : CursorTypes.Arrow;
  }

  private void ProcessMouseUpForDefaultTool(Point page_point, int page_index)
  {
    if (!this._mousePressedInLink || this.IsMouseCaptured || this.IsLinkAnnotationHighlighted)
      return;
    PdfLink linkAtPoint = this.Document.Pages[page_index].Links.GetLinkAtPoint((float) page_point.X, (float) page_point.Y);
    PdfWebLink webLinkAtPoint = this.Document.Pages[page_index].Text.WebLinks.GetWebLinkAtPoint((float) page_point.X, (float) page_point.Y);
    if (webLinkAtPoint != null || linkAtPoint != null)
      this.ProcessLinkClicked(linkAtPoint, webLinkAtPoint);
  }

  public PdfAnnotation GetPointAnnotation(Point clientPoint, out int pageIndex)
  {
    Point pagePoint;
    pageIndex = this.DeviceToPage(clientPoint.X, clientPoint.Y, out pagePoint);
    return pageIndex == -1 ? (PdfAnnotation) null : this.GetPointAnnotation(this.Document.Pages[pageIndex], pagePoint, out int _);
  }

  private PdfAnnotation GetPointAnnotation(PdfPage page, Point pagePoint, out int index)
  {
    index = -1;
    if (page == null || page.Annots == null || page.Annots.Count == 0)
      return (PdfAnnotation) null;
    bool annotationVisible = this.IsAnnotationVisible;
    for (int index1 = page.Annots.Count - 1; index1 >= 0; --index1)
    {
      try
      {
        PdfAnnotation annot = page.Annots[index1];
        if (annotationVisible || !PdfRenderHelper.CanAnnotationHide(annot))
        {
          if (AnnotationHitTestHelper.ProcessAnnotation(annot, pagePoint))
          {
            index = index1;
            return annot;
          }
        }
      }
      catch (UnexpectedTypeException ex)
      {
        GAManager.SendEvent("Exception", "UnexpectedTypeException2", ex.Message, 1L);
      }
    }
    return (PdfAnnotation) null;
  }

  private void ProcessMouseDownPanTool(Point mouse_point)
  {
    this._panToolInitialScrollPosition = this._autoScrollPosition;
    this._panToolInitialMousePosition = mouse_point;
    if (this.OverrideCursor == null)
      this.Cursor = PdfViewer.panToolCursorHelper.HandClose;
    this.CaptureMouse();
  }

  private CursorTypes ProcessMouseMoveForPanTool(Point mouse_point)
  {
    if (!this._mousePressed)
      return CursorTypes.Arrow;
    double num1 = mouse_point.Y - this._panToolInitialMousePosition.Y;
    double num2 = mouse_point.X - this._panToolInitialMousePosition.X;
    this.SetVerticalOffset(-this._panToolInitialScrollPosition.Y - num1);
    this.SetHorizontalOffset(-this._panToolInitialScrollPosition.X - num2);
    return CursorTypes.Arrow;
  }

  private void ProcessMouseUpPanTool(Point mouse_point)
  {
    this.ReleaseMouseCapture();
    if (this.OverrideCursor != null)
      return;
    this.Cursor = PdfViewer.panToolCursorHelper.Hand;
  }

  private struct CaptureInfo
  {
    public PdfForms forms;
    public SynchronizationContext sync;
    public int color;
  }

  private enum SmoothSelection
  {
    None,
    ByCharacter,
    ByLine,
  }
}
