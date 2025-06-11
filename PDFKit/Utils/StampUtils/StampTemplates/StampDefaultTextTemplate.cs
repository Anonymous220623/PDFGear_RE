// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.StampUtils.StampTemplates.StampDefaultTextTemplate
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using PDFKit.Utils.PageContents;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace PDFKit.Utils.StampUtils.StampTemplates;

internal class StampDefaultTextTemplate : StampTemplateBase
{
  internal override bool IsMatch(PdfStampAnnotation annotation, PDFExtStampDictionary extDict)
  {
    if (annotation == null)
      return false;
    if (extDict == null)
    {
      if (annotation.StandardIconName != StampIconNames.Extended)
        return true;
      if (!string.IsNullOrEmpty(annotation.ExtendedIconName))
        return annotation.NormalAppearance == null || annotation.NormalAppearance.All<PdfPageObject>((Func<PdfPageObject, bool>) (c => c.ObjectType != PageObjectTypes.PDFPAGE_IMAGE));
    }
    else if (extDict.Type == "Stamp" && (string.IsNullOrEmpty(extDict.Template) || extDict.Template == "Default"))
      return true;
    return false;
  }

  internal override bool RegenerateAppearances(
    PdfStampAnnotation annotation,
    PDFExtStampDictionary extDict)
  {
    if (annotation == null || string.IsNullOrEmpty(this.GetText(annotation, extDict)))
      return false;
    FS_RECTF rect = annotation.GetRECT();
    annotation.Rectangle = rect;
    Visual previewVisual = this.CreatePreviewVisual(StampAnnotationData.Create(annotation, extDict));
    if (previewVisual == null)
      return false;
    System.Collections.Generic.IReadOnlyList<PdfPathObject> fromVisual = PdfPathObjectUtils.CreateFromVisual(previewVisual, new FS_POINTF(rect.left, rect.bottom), 72.0);
    annotation.Rectangle = rect;
    annotation.NormalAppearance?.Clear();
    annotation.CreateEmptyAppearance(AppearanceStreamModes.Normal);
    for (int index = 0; index < fromVisual.Count; ++index)
      annotation.NormalAppearance.Add((PdfPageObject) fromVisual[index]);
    annotation.GenerateAppearance(AppearanceStreamModes.Normal);
    return true;
  }

  internal override string GetText(PdfStampAnnotation annotation, PDFExtStampDictionary extDict)
  {
    Dictionary<string, string> contentDictionary = this.GetContentDictionary(annotation, extDict);
    return StampDefaultTextTemplate.GetDefaultText(annotation, extDict, contentDictionary);
  }

  internal static string GetDefaultText(
    PdfStampAnnotation annotation,
    PDFExtStampDictionary extDict,
    Dictionary<string, string> contentDict)
  {
    if (extDict != null && extDict.Type == "Stamp" && (string.IsNullOrEmpty(extDict.Template) || extDict.Template == "Default"))
    {
      string str;
      return contentDict != null && contentDict.TryGetValue("ContentText", out str) && !string.IsNullOrEmpty(str) ? str : annotation.ExtendedIconName;
    }
    if (annotation.StandardIconName == StampIconNames.Extended && !string.IsNullOrEmpty(annotation.ExtendedIconName))
      return annotation.ExtendedIconName;
    return annotation.NormalAppearance != null ? annotation.NormalAppearance.Where<PdfPageObject>((Func<PdfPageObject, bool>) (x => x.ObjectType == PageObjectTypes.PDFPAGE_TEXT)).OfType<PdfTextObject>().Aggregate<PdfTextObject, StringBuilder>(new StringBuilder(), (Func<StringBuilder, PdfTextObject, StringBuilder>) ((x, y) => x.Append(y.TextUnicode))).ToString() ?? "" : "";
  }

  internal override Visual CreatePreviewVisual(StampAnnotationData data)
  {
    if (data == null)
      return (Visual) null;
    string content = data.Content;
    double width1 = data.Width;
    double height1 = data.Height;
    Color color = data.Color;
    if (width1 <= 2.0 || height1 <= 2.0)
      return (Visual) null;
    string format = "";
    string name = "";
    string s = "";
    string textToFormat = "";
    double thickness = 2.0;
    data.ContentDictionary?.TryGetValue("TimeFormat", out format);
    data.ContentDictionary?.TryGetValue("Locale", out name);
    data.ContentDictionary?.TryGetValue("BorderThickness", out s);
    double result;
    if (!string.IsNullOrEmpty(s) && double.TryParse(s, out result))
      thickness = result;
    if (!string.IsNullOrEmpty(format) && data.CreateDate.HasValue)
    {
      CultureInfo cultureInfo = CultureInfo.InvariantCulture;
      try
      {
        cultureInfo = CultureInfo.GetCultureInfo(name);
      }
      catch
      {
      }
      try
      {
        textToFormat = data.CreateDate.Value.ToString(format, (IFormatProvider) cultureInfo);
      }
      catch
      {
        textToFormat = data.CreateDate.Value.ToString((IFormatProvider) cultureInfo);
      }
    }
    double num1 = Math.Min(width1, height1) / 5.0;
    double num2 = num1;
    if (!string.IsNullOrEmpty(textToFormat))
      num1 /= 2.0;
    double width2 = width1 - num1 * 2.0 - 4.0;
    double height2 = height1 - num1 * 2.0 - 4.0;
    Geometry geometry1 = (Geometry) null;
    Rect rect1 = new Rect(0.0, 0.0, 0.0, 0.0);
    Geometry geometry2 = (Geometry) null;
    Rect rect2 = new Rect(0.0, 0.0, 0.0, 0.0);
    if (width2 > 0.0 && height2 > 0.0)
    {
      Typeface typeface = new Typeface(new FontFamily("#Global Sans Serif"), FontStyles.Italic, FontWeights.Bold, FontStretches.Normal);
      geometry1 = new FormattedText(content, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight, typeface, 1.0, (Brush) Brushes.Black, 96.0).BuildGeometry(new Point());
      rect1 = geometry1.Bounds;
      if (rect1.Width == 0.0 || rect1.Height == 0.0)
      {
        geometry1 = (Geometry) null;
        rect1 = new Rect(0.0, 0.0, 0.0, 0.0);
      }
      if (!string.IsNullOrEmpty(textToFormat))
      {
        geometry2 = new FormattedText(textToFormat, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight, typeface, 1.0, (Brush) Brushes.Black, 96.0).BuildGeometry(new Point());
        rect2 = geometry2.Bounds;
        if (rect2.Width == 0.0 || rect2.Height == 0.0)
        {
          geometry2 = (Geometry) null;
          rect2 = new Rect(0.0, 0.0, 0.0, 0.0);
        }
      }
    }
    SolidColorBrush solidColorBrush = new SolidColorBrush(color);
    DrawingVisual previewVisual = new DrawingVisual();
    using (DrawingContext drawingContext = previewVisual.RenderOpen())
    {
      Rect rectangle = new Rect(2.0, 2.0, width1 - 4.0, height1 - 4.0);
      drawingContext.DrawRoundedRectangle((Brush) Brushes.Transparent, new Pen((Brush) solidColorBrush, thickness), rectangle, num2, num2);
      if (!string.IsNullOrEmpty(content) && geometry1 != null)
      {
        Rect rect3 = new Rect(num1 + 2.0, num1 + 2.0, width2, height2);
        double num3 = Math.Min(rect3.Width / rect1.Width, rect3.Height / rect1.Height);
        double num4 = rect3.Height;
        if (geometry2 != null)
        {
          double num5 = num1 / 2.0;
          num4 = (rect3.Height - num5) / 3.0 * 2.0;
          double num6 = rect3.Height - num5 - num4;
          num3 = Math.Min(rect3.Width / rect1.Width, num4 / rect1.Height);
          double num7 = Math.Min(rect3.Width / rect2.Width, num6 / rect2.Height);
          double num8 = rect2.Width * num7;
          double num9 = rect2.Height * num7;
          Matrix identity = Matrix.Identity;
          identity.Translate(-rect2.Left, -rect2.Top);
          identity.Scale(num7, num7);
          identity.Translate(rect3.Left + (rect3.Width - num8) / 2.0, rect3.Top + num4 + num5 + (num6 - num9) / 2.0);
          geometry2.Transform = (Transform) new MatrixTransform(identity);
          drawingContext.DrawGeometry((Brush) solidColorBrush, (Pen) null, geometry2);
        }
        double num10 = rect1.Width * num3;
        double num11 = rect1.Height * num3;
        Matrix identity1 = Matrix.Identity;
        identity1.Translate(-rect1.Left, -rect1.Top);
        identity1.Scale(num3, num3);
        identity1.Translate(rect3.Left + (rect3.Width - num10) / 2.0, rect3.Top + (num4 - num11) / 2.0);
        geometry1.Transform = (Transform) new MatrixTransform(identity1);
        drawingContext.DrawGeometry((Brush) solidColorBrush, (Pen) null, geometry1);
      }
    }
    return (Visual) previewVisual;
  }
}
