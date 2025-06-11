// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PdfRichTextStrings.RichTextBoxUtils
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.Wrappers;
using PDFKit.Utils.PageContents;
using PDFKit.Utils.PdfRichTextStrings.HtmlToXaml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;

#nullable disable
namespace PDFKit.Utils.PdfRichTextStrings;

public static class RichTextBoxUtils
{
  public static async Task<List<PdfPathObject>> CreatePdfTextBoundsAsync(
    RichTextBox rtb,
    PdfDocument document,
    FS_RECTF rect,
    float[] innerRectangle,
    CancellationToken cancellationToken = default (CancellationToken))
  {
    FS_RECTF rect2 = AnnotDrawingHelper.GetInnerRectangle(rect, innerRectangle);
    System.Collections.Generic.IReadOnlyList<Run> runs = RichTextBoxUtils.GetAllRuns(rtb.Document);
    System.Collections.Generic.IReadOnlyList<TextPointerLineInfo> lines = RichTextBoxUtils.GetLineInfos(rtb);
    System.Collections.Generic.IReadOnlyList<TextSegmentInfo> ss = await RichTextBoxUtils.GetSegmentInfosAsync(rtb, runs, lines, cancellationToken);
    double pixelsPerDip = VisualTreeHelper.GetDpi((Visual) rtb).PixelsPerDip;
    FS_COLOR[] brushes = new FS_COLOR[4]
    {
      new FS_COLOR((int) byte.MaxValue, 74, 183, 226),
      new FS_COLOR((int) byte.MaxValue, 254, 246, 129),
      new FS_COLOR((int) byte.MaxValue, 249, 97, 107),
      new FS_COLOR((int) byte.MaxValue, 66, 234, 89)
    };
    List<PdfPathObject> l = new List<PdfPathObject>();
    for (int i = 0; i < ss.Count; ++i)
    {
      TextSegmentInfo segment = ss[i];
      double left1 = (double) rect.left;
      Rect tightBounds = segment.TightBounds;
      double num1 = tightBounds.Left / 96.0 * 72.0;
      float left = (float) (left1 + num1);
      double num2 = (double) rect.bottom + (double) rect.Height;
      tightBounds = segment.TightBounds;
      double num3 = tightBounds.Top / 96.0 * 72.0;
      float top = (float) (num2 - num3);
      tightBounds = segment.TightBounds;
      float width = (float) (tightBounds.Width / 96.0 * 72.0);
      tightBounds = segment.TightBounds;
      float height = (float) (tightBounds.Height / 96.0 * 72.0);
      FS_RECTF segmentRect = new FS_RECTF(left, top, left + width, top - height);
      l.AddRange((IEnumerable<PdfPathObject>) CreateCore(brushes[i % 4], segmentRect));
      segment = new TextSegmentInfo();
      segmentRect = new FS_RECTF();
    }
    List<PdfPathObject> pdfTextBoundsAsync = l;
    runs = (System.Collections.Generic.IReadOnlyList<Run>) null;
    lines = (System.Collections.Generic.IReadOnlyList<TextPointerLineInfo>) null;
    ss = (System.Collections.Generic.IReadOnlyList<TextSegmentInfo>) null;
    brushes = (FS_COLOR[]) null;
    l = (List<PdfPathObject>) null;
    return pdfTextBoundsAsync;

    static List<PdfPathObject> CreateCore(FS_COLOR fillColor, FS_RECTF r)
    {
      return AnnotDrawingHelper.CreateSquare(new FS_COLOR(0), fillColor, 0.0f, (float[]) null, BorderStyles.Solid, BorderEffects.None, 0, r);
    }
  }

  public static async Task<List<PdfTextObject>> CreatePdfTextObjectsAsync(
    RichTextBox rtb,
    PdfDocument document,
    float padding,
    FS_RECTF rect,
    float[] innerRectangle,
    CancellationToken cancellationToken = default (CancellationToken))
  {
    FS_RECTF rect2 = AnnotDrawingHelper.GetInnerRectangle(rect, innerRectangle);
    System.Collections.Generic.IReadOnlyList<Run> runs = RichTextBoxUtils.GetAllRuns(rtb.Document);
    System.Collections.Generic.IReadOnlyList<TextPointerLineInfo> lines = RichTextBoxUtils.GetLineInfos(rtb);
    System.Collections.Generic.IReadOnlyList<TextSegmentInfo> ss = await RichTextBoxUtils.GetSegmentInfosAsync(rtb, runs, lines, cancellationToken);
    List<PdfTextObject> list = ss.SelectMany<TextSegmentInfo, PdfTextObject>((Func<TextSegmentInfo, IEnumerable<PdfTextObject>>) (c => (IEnumerable<PdfTextObject>) AnnotDrawingHelper.CreatePdfTextObjects(document, c.Run, c.Text, c.Bounds, c.Baseline, padding, rect2))).ToList<PdfTextObject>();
    runs = (System.Collections.Generic.IReadOnlyList<Run>) null;
    lines = (System.Collections.Generic.IReadOnlyList<TextPointerLineInfo>) null;
    ss = (System.Collections.Generic.IReadOnlyList<TextSegmentInfo>) null;
    return list;
  }

  public static System.Collections.Generic.IReadOnlyList<PdfPathObject> CreateTextPdfPathObjects(
    RichTextBox rtb,
    PdfDocument document,
    float padding,
    FS_RECTF rect,
    float[] innerRectangle)
  {
    FS_RECTF innerRectangle1 = AnnotDrawingHelper.GetInnerRectangle(rect, innerRectangle);
    FrameworkElement reference = (FrameworkElement) null;
    if (VisualTreeHelper.GetChildrenCount((DependencyObject) rtb) > 0)
      reference = (FrameworkElement) VisualTreeHelper.GetChild((DependencyObject) rtb, 0);
    if (reference == null)
    {
      rtb.ApplyTemplate();
      rtb.UpdateLayout();
      reference = (FrameworkElement) VisualTreeHelper.GetChild((DependencyObject) rtb, 0);
    }
    if (reference == null)
      return (System.Collections.Generic.IReadOnlyList<PdfPathObject>) new List<PdfPathObject>();
    // ISSUE: explicit non-virtual call
    FrameworkElement content = VisualTreeHelper.GetChild((DependencyObject) reference, 0) is ScrollViewer child ? (FrameworkElement) __nonvirtual (child.Content) : (FrameworkElement) (object) null;
    Visual visual = (Visual) null;
    if (content != null)
      visual = (Visual) VisualTreeHelper.GetChild((DependencyObject) content, 0);
    return PdfPathObjectUtils.CreateFromVisual(visual, new FS_POINTF(innerRectangle1.left + padding, innerRectangle1.bottom + padding));
  }

  public static RichTextBox CreateRichTextBox(PdfFreeTextAnnotation annot)
  {
    if ((PdfWrapper) annot == (PdfWrapper) null)
      return (RichTextBox) null;
    PdfDefaultAppearance pdfFontStyle;
    if (!PdfDefaultAppearance.TryParse(annot.DefaultAppearance, out pdfFontStyle))
      pdfFontStyle = PdfDefaultAppearance.Default;
    if (annot.Color != FS_COLOR.Empty)
      pdfFontStyle.FillColor = annot.Color;
    PdfRichTextStyle richTextStyle;
    if (!PdfRichTextStyle.TryParse(annot.DefaultStyle, out richTextStyle))
      richTextStyle = pdfFontStyle.ToRichTextStyle();
    PdfRichTextString pdfRichTextString;
    if (!PdfRichTextString.TryParse(annot.RichText, richTextStyle, out pdfRichTextString, annot.Name))
      pdfRichTextString = (PdfRichTextString) null;
    return RichTextBoxUtils.CreateRichTextBox(annot, pdfRichTextString, pdfFontStyle, richTextStyle);
  }

  public static RichTextBox CreateRichTextBox(
    PdfFreeTextAnnotation annot,
    PdfRichTextString pdfRichTextString,
    PdfDefaultAppearance da,
    PdfRichTextStyle ds)
  {
    if ((PdfWrapper) annot == (PdfWrapper) null)
      return (RichTextBox) null;
    FS_RECTF rect = annot.GetRECT();
    PageRotate adjustRotate;
    annot.GetRotate(out PageRotate? _, out adjustRotate);
    FS_RECTF originalRectangle = PdfRotateUtils.GetOriginalRectangle(rect, adjustRotate);
    PdfBorderStyle borderStyle = annot.BorderStyle;
    float num = borderStyle != null ? borderStyle.Width : 1f;
    FS_RECTF fsRectf = new FS_RECTF(originalRectangle.left + num, originalRectangle.top - num, originalRectangle.right - num, originalRectangle.bottom + num);
    RichTextBox rtb = RichTextBoxUtils.CreateRichTextBox(pdfRichTextString, new FS_RECTF?(fsRectf));
    if (rtb == null)
    {
      rtb = new RichTextBox();
      rtb.AppendText(annot.Contents ?? "");
      rtb.SelectAll();
      Color? color = ds.Color;
      if (color.HasValue)
      {
        TextSelection selection = rtb.Selection;
        DependencyProperty foregroundProperty = TextElement.ForegroundProperty;
        color = ds.Color;
        SolidColorBrush solidColorBrush = new SolidColorBrush(color.Value);
        selection.ApplyPropertyValue(foregroundProperty, (object) solidColorBrush);
      }
      rtb.Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, (object) new FontFamily(da.FontFamily));
      rtb.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, (object) ((double) da.FontSize * 96.0 / 72.0));
    }
    if (rtb != null)
    {
      rtb.BorderThickness = new Thickness();
      if (rtb.Document != null)
        rtb.Document.PagePadding = new Thickness();
      rtb.Width = (double) Math.Max(0.0f, (float) ((double) fsRectf.Width / 72.0 * 96.0));
      rtb.Height = (double) Math.Max(0.0f, (float) ((double) fsRectf.Height / 72.0 * 96.0));
      RichTextBoxUtils.UpdateRichTextBoxLayout(rtb);
    }
    return rtb;
  }

  public static RichTextBox CreateRichTextBox(
    PdfRichTextString pdfRichTextString,
    FS_RECTF? annotRectangle = null)
  {
    if (pdfRichTextString?.Text == null)
      return (RichTextBox) null;
    try
    {
      FlowDocument flowDocument = XamlReader.Parse(HtmlToXamlConverter.ConvertHtmlToXaml(pdfRichTextString.ToString(true), true).Replace("<Paragraph", "<Paragraph Margin=\"0\"")) as FlowDocument;
      RichTextBox rtb = new RichTextBox();
      string str1 = PdfFontUtils.TryGetDefaultDocumentFont(CultureInfo.CurrentCulture);
      if (string.IsNullOrEmpty(str1))
        str1 = "#GLOBAL USER INTERFACE";
      PdfRichTextStyle defaultStyle = pdfRichTextString.DefaultStyle;
      string str2;
      if (!string.IsNullOrEmpty(defaultStyle.FontFamily))
      {
        defaultStyle = pdfRichTextString.DefaultStyle;
        str2 = defaultStyle.FontFamily;
      }
      else
        str2 = str1;
      string familyName = str2;
      rtb.FontFamily = new FontFamily(familyName);
      defaultStyle = pdfRichTextString.DefaultStyle;
      float? fontSize = defaultStyle.FontSize;
      double valueOrDefault;
      if (!fontSize.HasValue)
      {
        defaultStyle = PdfRichTextStyle.Default;
        valueOrDefault = (double) defaultStyle.FontSize.Value;
      }
      else
        valueOrDefault = (double) fontSize.GetValueOrDefault();
      float num = (float) valueOrDefault;
      rtb.FontSize = (double) num / 72.0 * 96.0;
      defaultStyle = pdfRichTextString.DefaultStyle;
      PdfRichTextStyleFontStretch? fontStretch = defaultStyle.FontStretch;
      if (fontStretch.HasValue)
      {
        switch (fontStretch.GetValueOrDefault())
        {
          case PdfRichTextStyleFontStretch.UltraCondensed:
            rtb.FontStretch = FontStretches.UltraCondensed;
            break;
          case PdfRichTextStyleFontStretch.ExtraCondensed:
            rtb.FontStretch = FontStretches.ExtraCondensed;
            break;
          case PdfRichTextStyleFontStretch.Condensed:
            rtb.FontStretch = FontStretches.Condensed;
            break;
          case PdfRichTextStyleFontStretch.SemiCondensed:
            rtb.FontStretch = FontStretches.SemiCondensed;
            break;
          case PdfRichTextStyleFontStretch.Normal:
            rtb.FontStretch = FontStretches.Normal;
            break;
          case PdfRichTextStyleFontStretch.SemiExpanded:
            rtb.FontStretch = FontStretches.SemiExpanded;
            break;
          case PdfRichTextStyleFontStretch.Expanded:
            rtb.FontStretch = FontStretches.Expanded;
            break;
          case PdfRichTextStyleFontStretch.ExtraExpanded:
            rtb.FontStretch = FontStretches.ExtraExpanded;
            break;
          case PdfRichTextStyleFontStretch.AndultraExpanded:
            rtb.FontStretch = FontStretches.ExtraExpanded;
            break;
        }
      }
      defaultStyle = pdfRichTextString.DefaultStyle;
      PdfRichTextStyleFontWeight? fontWeight1 = defaultStyle.FontWeight;
      if (fontWeight1.HasValue)
      {
        switch (fontWeight1.GetValueOrDefault())
        {
          case PdfRichTextStyleFontWeight.Bold:
            rtb.FontWeight = FontWeights.Bold;
            goto label_26;
          case PdfRichTextStyleFontWeight.W100:
          case PdfRichTextStyleFontWeight.W200:
          case PdfRichTextStyleFontWeight.W300:
          case PdfRichTextStyleFontWeight.W400:
          case PdfRichTextStyleFontWeight.W500:
          case PdfRichTextStyleFontWeight.W600:
          case PdfRichTextStyleFontWeight.W700:
          case PdfRichTextStyleFontWeight.W800:
          case PdfRichTextStyleFontWeight.W900:
            RichTextBox richTextBox = rtb;
            FontWeightConverter fontWeightConverter = new FontWeightConverter();
            defaultStyle = pdfRichTextString.DefaultStyle;
            string text = ((int) defaultStyle.FontWeight.Value).ToString();
            System.Windows.FontWeight fontWeight2 = (System.Windows.FontWeight) fontWeightConverter.ConvertFromString(text);
            richTextBox.FontWeight = fontWeight2;
            goto label_26;
        }
      }
      rtb.FontWeight = FontWeights.Normal;
label_26:
      rtb.Padding = new Thickness();
      rtb.MinWidth = 1.0;
      rtb.MinHeight = 0.0;
      rtb.Document = flowDocument;
      double val1_1 = 1.0;
      double val1_2 = 1.0;
      if (annotRectangle.HasValue)
      {
        val1_1 = Math.Max(val1_1, (double) annotRectangle.Value.Width / 72.0 * 96.0);
        val1_2 = Math.Max(val1_2, (double) annotRectangle.Value.Height / 72.0 * 96.0);
      }
      rtb.Width = val1_1;
      rtb.Height = val1_2;
      RichTextBoxUtils.UpdateRichTextBoxLayout(rtb);
      return rtb;
    }
    catch
    {
    }
    return (RichTextBox) null;
  }

  public static async Task ExtendRichTextBoxAsync(
    RichTextBox rtb,
    bool isTypeWriter,
    CancellationToken cancellationToken = default (CancellationToken))
  {
    if (rtb == null)
      throw new ArgumentNullException(nameof (rtb));
    if (((double.IsNaN(rtb.Width) ? 1 : (double.IsNaN(rtb.Height) ? 1 : 0)) | (isTypeWriter ? 1 : 0)) != 0 && rtb != null)
    {
      if (rtb.Document != null)
        rtb.Document.PagePadding = new Thickness();
      if (double.IsNaN(rtb.Width))
        rtb.Width = 1.0;
      if (double.IsNaN(rtb.Height) | isTypeWriter)
        rtb.Height = 1.0;
      RichTextBoxUtils.UpdateRichTextBoxLayout(rtb);
    }
    sv = (ScrollViewer) null;
    for (int i = 0; i < 15; ++i)
    {
      FrameworkElement child = VisualTreeHelper.GetChild((DependencyObject) rtb, 0) as FrameworkElement;
      if (!(child?.FindName("PART_ContentHost") is ScrollViewer sv))
      {
        RichTextBoxUtils.UpdateRichTextBoxLayout(rtb);
        await Task.Delay(1, cancellationToken);
        child = (FrameworkElement) null;
      }
      else
        break;
    }
    string contentText;
    if (sv == null)
    {
      sv = (ScrollViewer) null;
      contentText = (string) null;
    }
    else
    {
      sv.UpdateLayout();
      double oldContentWidth = sv.Content is FrameworkElement content ? content.ActualWidth : 0.0;
      contentText = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd).Text;
      double minWidth = 2.0;
      if (!string.IsNullOrEmpty(contentText))
      {
        System.Collections.Generic.IReadOnlyList<Run> runs = RichTextBoxUtils.GetAllRuns(rtb.Document);
        double maxFontSize = runs.DefaultIfEmpty<Run>().Max<Run>((Func<Run, double>) (c => !string.IsNullOrEmpty(c?.Text) ? c.FontSize : 0.0));
        minWidth = Math.Max(maxFontSize, minWidth);
        runs = (System.Collections.Generic.IReadOnlyList<Run>) null;
      }
      oldContentWidth = Math.Max(sv.ExtentWidth, Math.Max(oldContentWidth, minWidth));
      if (oldContentWidth <= rtb.ActualWidth && sv.ExtentHeight <= rtb.ActualHeight)
      {
        sv = (ScrollViewer) null;
        contentText = (string) null;
      }
      else
      {
        if (oldContentWidth > rtb.ActualWidth)
          rtb.Width = oldContentWidth;
        if (sv.ExtentHeight > rtb.ActualHeight)
          rtb.Height = sv.ExtentHeight;
        RichTextBoxUtils.UpdateRichTextBoxLayout(rtb);
        sv = (ScrollViewer) null;
        contentText = (string) null;
      }
    }
  }

  public static void ExtendRichTextBox(RichTextBox rtb, bool isTypeWriter)
  {
    if (rtb == null)
      throw new ArgumentNullException(nameof (rtb));
    if (((double.IsNaN(rtb.Width) ? 1 : (double.IsNaN(rtb.Height) ? 1 : 0)) | (isTypeWriter ? 1 : 0)) != 0 && rtb != null)
    {
      if (rtb.Document != null)
        rtb.Document.PagePadding = new Thickness();
      if (double.IsNaN(rtb.Width))
        rtb.Width = 1.0;
      if (double.IsNaN(rtb.Height) | isTypeWriter)
        rtb.Height = 1.0;
      RichTextBoxUtils.UpdateRichTextBoxLayout(rtb);
    }
    if ((VisualTreeHelper.GetChild((DependencyObject) rtb, 0) is FrameworkElement child1 ? child1.FindName("PART_ContentHost") : (object) null) is ScrollViewer name)
    {
      rtb.ApplyTemplate();
      rtb.UpdateLayout();
      name = (VisualTreeHelper.GetChild((DependencyObject) rtb, 0) is FrameworkElement child ? child.FindName("PART_ContentHost") : (object) null) as ScrollViewer;
    }
    if (name == null)
      return;
    name.UpdateLayout();
    double val1 = name.Content is FrameworkElement content ? content.ActualWidth : 0.0;
    string text = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd).Text;
    double val2 = 2.0;
    if (!string.IsNullOrEmpty(text))
      val2 = Math.Max(RichTextBoxUtils.GetAllRuns(rtb.Document).DefaultIfEmpty<Run>().Max<Run>((Func<Run, double>) (c => !string.IsNullOrEmpty(c?.Text) ? c.FontSize : 0.0)), val2);
    double num = Math.Max(name.ExtentWidth, Math.Max(val1, val2));
    if (num <= rtb.ActualWidth && name.ExtentHeight <= rtb.ActualHeight)
      return;
    if (num > rtb.ActualWidth)
      rtb.Width = num;
    if (name.ExtentHeight > rtb.ActualHeight)
      rtb.Height = name.ExtentHeight;
    RichTextBoxUtils.UpdateRichTextBoxLayout(rtb);
  }

  private static System.Collections.Generic.IReadOnlyList<TextPointerLineInfo> GetLineInfos(
    RichTextBox rtb)
  {
    TextPointerLineInfo? nullable = TextPointerLineInfo.CreateFirstLine(rtb);
    if (!nullable.HasValue)
      return (System.Collections.Generic.IReadOnlyList<TextPointerLineInfo>) Array.Empty<TextPointerLineInfo>();
    List<TextPointerLineInfo> lineInfos = new List<TextPointerLineInfo>();
    TextPointer contentEnd = rtb.Document.ContentEnd;
    do
    {
      lineInfos.Add(nullable.Value);
      nullable = nullable.Value.GetNextLineInfo(contentEnd);
    }
    while (nullable.HasValue);
    return (System.Collections.Generic.IReadOnlyList<TextPointerLineInfo>) lineInfos;
  }

  private static async Task<System.Collections.Generic.IReadOnlyList<TextSegmentInfo>> GetSegmentInfosAsync(
    RichTextBox rtb,
    System.Collections.Generic.IReadOnlyList<Run> runs,
    System.Collections.Generic.IReadOnlyList<TextPointerLineInfo> lines,
    CancellationToken cancellationToken = default (CancellationToken))
  {
    cancellationToken.ThrowIfCancellationRequested();
    if (runs == null || runs.Count == 0 || lines == null || lines.Count == 0)
      return (System.Collections.Generic.IReadOnlyList<TextSegmentInfo>) Array.Empty<TextSegmentInfo>();
    TextViewProxy textView = TextViewProxy.Create(rtb);
    if (textView == null)
      return (System.Collections.Generic.IReadOnlyList<TextSegmentInfo>) Array.Empty<TextSegmentInfo>();
    for (int i = 0; i < 15 && !textView.Validate(); ++i)
    {
      cancellationToken.ThrowIfCancellationRequested();
      await Task.Delay(1, cancellationToken);
    }
    DpiScale scale = VisualTreeHelper.GetDpi((Visual) rtb);
    double pixelsPerDip = scale.PixelsPerDip;
    List<TextSegmentInfo> list = new List<TextSegmentInfo>();
    int runIdx = 0;
    int lineIdx = 0;
    while (runIdx < runs.Count && lineIdx < lines.Count)
    {
      cancellationToken.ThrowIfCancellationRequested();
      Run r = runs[runIdx];
      TextPointerLineInfo l = lines[lineIdx];
      int c = r.ElementEnd.CompareTo(l.LineEnd);
      if (c <= 0)
      {
        TextPointer start = r.ElementStart.CompareTo(l.LineStart) < 0 ? l.LineStart : r.ElementStart;
        TextRange range = new TextRange(start, r.ElementEnd);
        if (!string.IsNullOrEmpty(range.Text))
        {
          double baseline;
          Rect bounds;
          Rect tightBounds = RichTextBoxUtils.GetTightBounds(textView, r, range, pixelsPerDip, out baseline, out bounds);
          if (!tightBounds.IsEmpty)
          {
            TextSegmentInfo info = new TextSegmentInfo(r, range, bounds, tightBounds, l.LineIndex, baseline);
            list.Add(info);
            info = new TextSegmentInfo();
          }
          tightBounds = new Rect();
          bounds = new Rect();
        }
        ++runIdx;
        if (c == 0)
          ++lineIdx;
        start = (TextPointer) null;
        range = (TextRange) null;
      }
      else
      {
        if (r.ElementStart.CompareTo(l.LineEnd) < 0)
        {
          TextRange range = r.ElementStart.CompareTo(l.LineStart) >= 0 ? new TextRange(r.ElementStart, l.LineEnd) : new TextRange(l.LineStart, l.LineEnd);
          if (!string.IsNullOrEmpty(range.Text))
          {
            double baseline;
            Rect bounds;
            Rect tightBounds = RichTextBoxUtils.GetTightBounds(textView, r, range, pixelsPerDip, out baseline, out bounds);
            if (!tightBounds.IsEmpty)
            {
              TextSegmentInfo info = new TextSegmentInfo(r, range, bounds, tightBounds, l.LineIndex, baseline);
              list.Add(info);
              info = new TextSegmentInfo();
            }
            tightBounds = new Rect();
            bounds = new Rect();
          }
          range = (TextRange) null;
        }
        ++lineIdx;
      }
      r = (Run) null;
      l = new TextPointerLineInfo();
    }
    double maxBaseline = 0.0;
    int currentLineIndex = -1;
    int lineStartIndex = 0;
    TextSegmentInfo[] arr = list.ToArray();
    TextSegmentInfo textSegmentInfo;
    for (int i = 0; i < arr.Length; ++i)
    {
      if (arr[i].LineIndex != currentLineIndex)
      {
        for (int j = lineStartIndex; j < i; ++j)
          arr[j].Baseline = maxBaseline;
        textSegmentInfo = list[i];
        currentLineIndex = textSegmentInfo.LineIndex;
        textSegmentInfo = list[i];
        double num;
        if (!textSegmentInfo.HasVisibleElement)
        {
          num = 0.0;
        }
        else
        {
          textSegmentInfo = list[i];
          num = textSegmentInfo.Baseline;
        }
        maxBaseline = num;
        lineStartIndex = i;
      }
      else
      {
        double val1 = maxBaseline;
        textSegmentInfo = list[i];
        double val2;
        if (!textSegmentInfo.HasVisibleElement)
        {
          val2 = 0.0;
        }
        else
        {
          textSegmentInfo = list[i];
          val2 = textSegmentInfo.Baseline;
        }
        maxBaseline = Math.Max(val1, val2);
      }
    }
    if (lineStartIndex < arr.Length - 1)
    {
      for (int j = lineStartIndex; j < arr.Length; ++j)
        arr[j].Baseline = maxBaseline;
    }
    return (System.Collections.Generic.IReadOnlyList<TextSegmentInfo>) arr;
  }

  private static Rect GetTightBounds(
    TextViewProxy textView,
    Run run,
    TextRange range,
    double pixelsPerDip,
    out double baseline,
    out Rect charInLineBounds)
  {
    Geometry fromTextPositions = textView.GetTightBoundingGeometryFromTextPositions(range.Start, range.End);
    baseline = 0.0;
    charInLineBounds = Rect.Empty;
    if (fromTextPositions == null)
      return Rect.Empty;
    Rect bounds = fromTextPositions.Bounds;
    string text = range.Text;
    Typeface typeface = new Typeface(run.FontFamily, run.FontStyle, run.FontWeight, run.FontStretch);
    FormattedText formattedText = new FormattedText(text, run.Language.GetSpecificCulture(), run.FlowDirection, typeface, run.FontSize, run.Foreground, pixelsPerDip);
    baseline = formattedText.Baseline;
    charInLineBounds = bounds;
    return new Rect(bounds.Left, bounds.Top + (bounds.Height - formattedText.Height), formattedText.Width, formattedText.Height);
  }

  private static System.Collections.Generic.IReadOnlyList<Run> GetAllRuns(FlowDocument document)
  {
    if (document == null || document.Blocks.Count == 0)
      return (System.Collections.Generic.IReadOnlyList<Run>) Array.Empty<Run>();
    List<Run> allRuns = new List<Run>();
    foreach (Block block in (TextElementCollection<Block>) document.Blocks)
      allRuns.AddRange(Flatten((TextElement) block));
    return (System.Collections.Generic.IReadOnlyList<Run>) allRuns;

    static IEnumerable<Run> Flatten(TextElement element)
    {
      switch (element)
      {
        case Section section:
          return section.Blocks.SelectMany<Block, Run>((Func<Block, IEnumerable<Run>>) (c => Flatten((TextElement) c))).OfType<Run>();
        case AnchoredBlock anchoredBlock:
          return anchoredBlock.Blocks.SelectMany<Block, Run>((Func<Block, IEnumerable<Run>>) (c => Flatten((TextElement) c))).OfType<Run>();
        case Paragraph paragraph:
          return paragraph.Inlines.SelectMany<Inline, Run>((Func<Inline, IEnumerable<Run>>) (c => Flatten((TextElement) c))).OfType<Run>();
        case Span span:
          return span.Inlines.SelectMany<Inline, Run>((Func<Inline, IEnumerable<Run>>) (c => Flatten((TextElement) c))).OfType<Run>();
        case Run element:
          return Enumerable.Repeat<Run>(element, 1);
        default:
          return Enumerable.Empty<Run>();
      }
    }
  }

  private static void UpdateRichTextBoxLayout(RichTextBox rtb)
  {
    double num1 = rtb != null ? Canvas.GetLeft((UIElement) rtb) : throw new ArgumentNullException(nameof (rtb));
    double num2 = Canvas.GetTop((UIElement) rtb);
    if (double.IsNaN(num1))
      num1 = 0.0;
    if (double.IsNaN(num2))
      num2 = 0.0;
    FrameworkElement frameworkElement1 = (FrameworkElement) null;
    if (VisualTreeHelper.GetChildrenCount((DependencyObject) rtb) > 0)
      frameworkElement1 = (FrameworkElement) VisualTreeHelper.GetChild((DependencyObject) rtb, 0);
    if (frameworkElement1 == null)
      rtb.ApplyTemplate();
    rtb.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
    rtb.Arrange(new Rect(num1, num2, rtb.DesiredSize.Width, rtb.DesiredSize.Height));
    rtb.UpdateLayout();
    if (frameworkElement1 != null)
      return;
    FrameworkElement child = (FrameworkElement) VisualTreeHelper.GetChild((DependencyObject) rtb, 0);
    if (child != null)
    {
      child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
      FrameworkElement frameworkElement2 = child;
      double x = num1;
      double y = num2;
      Size desiredSize = rtb.DesiredSize;
      double width = desiredSize.Width;
      desiredSize = rtb.DesiredSize;
      double height = desiredSize.Height;
      Rect finalRect = new Rect(x, y, width, height);
      frameworkElement2.Arrange(finalRect);
      child.UpdateLayout();
    }
  }
}
