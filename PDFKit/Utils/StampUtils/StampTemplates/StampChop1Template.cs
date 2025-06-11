// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.StampUtils.StampTemplates.StampChop1Template
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.Wrappers;
using PDFKit.Utils.PageContents;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace PDFKit.Utils.StampUtils.StampTemplates;

internal class StampChop1Template : StampTemplateBase
{
  internal override bool IsMatch(PdfStampAnnotation annotation, PDFExtStampDictionary extDict)
  {
    return !((PdfWrapper) annotation == (PdfWrapper) null) && extDict != null && extDict.Type == "Stamp" && extDict.Template == "Chop1";
  }

  internal override bool RegenerateAppearances(
    PdfStampAnnotation annotation,
    PDFExtStampDictionary extDict)
  {
    if ((PdfWrapper) annotation == (PdfWrapper) null)
      return false;
    FS_RECTF rect = annotation.GetRECT();
    annotation.Rectangle = rect;
    FS_COLOR color = annotation.Color;
    Color.FromArgb((byte) Math.Min((float) byte.MaxValue, Math.Max(0.0f, (float) color.A * annotation.Opacity)), (byte) Math.Min((int) byte.MaxValue, Math.Max(0, color.R)), (byte) Math.Min((int) byte.MaxValue, Math.Max(0, color.G)), (byte) Math.Min((int) byte.MaxValue, Math.Max(0, color.B)));
    if ((double) rect.Width < 2.0)
      rect.right = rect.left + 2f;
    if ((double) rect.Height < 2.0)
      rect.bottom = rect.top - 2f;
    Visual previewVisual = this.CreatePreviewVisual(StampAnnotationData.Create(annotation, extDict));
    if (previewVisual == null)
      return false;
    System.Collections.Generic.IReadOnlyList<PdfPathObject> fromVisual = PdfPathObjectUtils.CreateFromVisual(previewVisual, new FS_POINTF(rect.left, rect.bottom), 72.0, true);
    annotation.Rectangle = rect;
    annotation.NormalAppearance?.Clear();
    annotation.CreateEmptyAppearance(AppearanceStreamModes.Normal);
    for (int index = 0; index < fromVisual.Count; ++index)
      annotation.NormalAppearance.Add((PdfPageObject) fromVisual[index]);
    annotation.GenerateAppearance(AppearanceStreamModes.Normal);
    return true;
  }

  internal override Visual CreatePreviewVisual(StampAnnotationData data)
  {
    if (data.ContentDictionary == null || data.ContentDictionary.Count == 0)
      return (Visual) null;
    string text1 = "";
    string text2 = "";
    string text3 = "";
    string str1 = "";
    string str2 = "";
    string format = "";
    string name = "";
    data.ContentDictionary.TryGetValue("Content1", out text1);
    data.ContentDictionary.TryGetValue("Content2", out str1);
    data.ContentDictionary.TryGetValue("Content2Type", out str2);
    data.ContentDictionary.TryGetValue("Content3", out text3);
    data.ContentDictionary.TryGetValue("TimeFormat", out format);
    data.ContentDictionary.TryGetValue("Locale", out name);
    switch (str2)
    {
      case "CreateTime":
        try
        {
          if (data.CreateDate.HasValue)
          {
            CultureInfo cultureInfo = CultureInfo.InvariantCulture;
            try
            {
              cultureInfo = CultureInfo.GetCultureInfo(name);
            }
            catch
            {
            }
            text2 = data.CreateDate.Value.ToString(format, (IFormatProvider) cultureInfo);
            break;
          }
          break;
        }
        catch
        {
          break;
        }
      case "Text":
        text2 = str1;
        break;
    }
    DrawingVisual previewVisual = new DrawingVisual();
    SolidColorBrush _brush = new SolidColorBrush(data.Color);
    using (DrawingContext _drawingContext = previewVisual.RenderOpen())
    {
      _drawingContext.DrawRectangle((Brush) null, new Pen((Brush) Brushes.Transparent, 0.0), new Rect(0.0, 0.0, data.Width, data.Height));
      double thickness = Math.Min(data.Width, data.Height) / 50.0;
      double num1 = Math.Min(data.Width, data.Height) / 2.0 - thickness;
      Point point = new Point(data.Width / 2.0, data.Height / 2.0);
      double _radius = num1 - thickness / 2.0;
      _drawingContext.DrawEllipse((Brush) null, new Pen((Brush) _brush, thickness), point, num1, num1);
      Rect _rect1 = new Rect();
      Rect _rect2 = new Rect();
      Rect _rect3 = new Rect();
      Geometry _textGeometry = (Geometry) null;
      Geometry textGeometry1;
      Geometry textGeometry2;
      if (string.IsNullOrEmpty(text2))
      {
        double num2 = point.Y - _radius / 2.0;
        double y = point.Y;
        double num3 = point.Y + _radius / 2.0;
        (double x1, double x2, double dist) result = GetResult(point, _radius, num2);
        _rect1 = new Rect(result.x1, num2, result.dist, y - num2);
        _rect3 = new Rect(result.x1, y, result.dist, num3 - y);
        textGeometry1 = StampChop1Template.GetTextGeometry(text1);
        textGeometry2 = StampChop1Template.GetTextGeometry(text3);
      }
      else
      {
        double num4 = point.Y - _radius / 3.0 * 2.0;
        double num5 = point.Y - _radius / 5.0;
        double y = point.Y + _radius / 5.0;
        double _y = point.Y + _radius / 3.0 * 2.0;
        (double x1, double x2, double dist) result1 = GetResult(point, _radius, num4);
        (double x1, double x2, double dist) result2 = GetResult(point, _radius, num5);
        (double x1, double x2, double dist) result3 = GetResult(point, _radius, _y);
        _rect1 = new Rect(result1.x1, num4, result1.dist, num5 - num4);
        _rect2 = new Rect(result2.x1, num5, result2.dist, y - num5);
        _rect3 = new Rect(result3.x1, y, result3.dist, _y - y);
        textGeometry1 = StampChop1Template.GetTextGeometry(text1);
        _textGeometry = StampChop1Template.GetTextGeometry(text2);
        textGeometry2 = StampChop1Template.GetTextGeometry(text3);
      }
      if (textGeometry1 != null)
        DrawText(_drawingContext, textGeometry1, _rect1, (Brush) _brush);
      if (_textGeometry != null)
        DrawText(_drawingContext, _textGeometry, _rect2, (Brush) _brush);
      if (textGeometry2 != null)
        DrawText(_drawingContext, textGeometry2, _rect3, (Brush) _brush);
    }
    return (Visual) previewVisual;

    static (double x1, double x2, double dist) GetResult(
      Point _centerPoint,
      double _radius,
      double _y)
    {
      double num = Math.Sqrt(_radius * _radius - (_y - _centerPoint.Y) * (_y - _centerPoint.Y));
      return (_centerPoint.X - num, _centerPoint.X + num, num * 2.0);
    }

    static void DrawText(
      DrawingContext _drawingContext,
      Geometry _textGeometry,
      Rect _rect,
      Brush _brush)
    {
      Rect bounds = _textGeometry.Bounds;
      double num1 = Math.Min(_rect.Width / bounds.Width, _rect.Height / bounds.Height);
      double num2 = bounds.Width * num1;
      double num3 = bounds.Height * num1;
      Matrix identity = Matrix.Identity;
      identity.Translate(-bounds.Left, -bounds.Top);
      identity.Scale(num1, num1);
      identity.Translate(_rect.Left + (_rect.Width - num2) / 2.0, _rect.Top + (_rect.Height - num3) / 2.0);
      _textGeometry.Transform = (Transform) new MatrixTransform(identity);
      _drawingContext.DrawGeometry(_brush, (Pen) null, _textGeometry);
    }
  }

  private static Geometry GetTextGeometry(string text)
  {
    Typeface typeface = new Typeface(new FontFamily("#Global Sans Serif"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal);
    Geometry geometry = new FormattedText(text, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight, typeface, 1.0, (Brush) Brushes.Black, 96.0).BuildGeometry(new Point());
    Rect bounds = geometry.Bounds;
    Matrix identity = Matrix.Identity;
    identity.Translate(-bounds.X, -bounds.Y);
    geometry.Transform = (Transform) new MatrixTransform(identity);
    return (Geometry) PathGeometry.CreateFromGeometry(geometry);
  }
}
