// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.AnnotationHitTestHelper
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.Annotations;
using Patagames.Pdf.Net.Wrappers;
using System;
using System.Linq;
using System.Windows;

#nullable disable
namespace PDFKit.Utils;

public static class AnnotationHitTestHelper
{
  public static bool HitTest(PdfAnnotation annot, Point devicePoint)
  {
    if ((PdfWrapper) annot == (PdfWrapper) null)
      return false;
    PdfControl pdfControl = PdfControl.GetPdfControl(annot.Page?.Document);
    Point pagePoint;
    return pdfControl != null && pdfControl.TryGetPagePoint(annot.Page.PageIndex, devicePoint, out pagePoint) && AnnotationHitTestHelper.ProcessAnnotation(annot, pagePoint);
  }

  internal static bool ProcessAnnotation(PdfAnnotation annot, Point pagePoint)
  {
    switch (annot)
    {
      case PdfInkAnnotation annot1:
        return AnnotationHitTestHelper.ProcessInkAnnotation(annot1, pagePoint);
      case PdfLineAnnotation annot2:
        return AnnotationHitTestHelper.ProcessLineAnnotation(annot2, pagePoint);
      case PdfPopupAnnotation annot3:
        return AnnotationHitTestHelper.ProcessPopupAnnotation(annot3, pagePoint);
      case PdfTextMarkupAnnotation annot4:
        return AnnotationHitTestHelper.ProcessTextMarkupAnnotation(annot4, pagePoint);
      case PdfWidgetAnnotation annot5:
        return AnnotationHitTestHelper.ProcessWidgetAnnotation(annot5, pagePoint);
      default:
        return AnnotationHitTestHelper.IsSupportAnnotation(annot) && AnnotationHitTestHelper.ProcessDefaultAnnotation(annot, pagePoint);
    }
  }

  private static bool IsSupportAnnotation(PdfAnnotation annot)
  {
    switch (annot)
    {
      case PdfMarkupAnnotation _:
        if (!(annot is PdfFileAttachmentAnnotation) || !(annot is PdfSoundAnnotation))
          return true;
        break;
      case PdfLinkAnnotation _:
        return true;
    }
    return false;
  }

  private static bool ProcessInkAnnotation(PdfInkAnnotation annot, Point pagePoint)
  {
    int num1;
    if (AnnotationHitTestHelper.Contains(annot.GetRECT(), pagePoint))
    {
      PdfInkPointCollection inkList = annot.InkList;
      // ISSUE: explicit non-virtual call
      num1 = inkList != null ? (__nonvirtual (inkList.Count) > 0 ? 1 : 0) : 0;
    }
    else
      num1 = 0;
    if (num1 != 0)
    {
label_11:
      for (int index1 = 0; index1 < annot.InkList.Count; ++index1)
      {
        PdfLinePointCollection<PdfInkAnnotation> ink = annot.InkList[index1];
        int index2 = 0;
        while (true)
        {
          int num2 = index2;
          int? count = ink?.Count;
          int valueOrDefault = count.GetValueOrDefault();
          if (num2 < valueOrDefault & count.HasValue)
          {
            FS_POINTF lineEnd = ink[index2];
            if (!AnnotationHitTestHelper.Contains(new FS_RECTF(lineEnd.X - 5f, lineEnd.Y + 5f, lineEnd.X + 5f, lineEnd.Y - 5f), pagePoint) && (index2 <= 0 || !AnnotationHitTestHelper.LineHitTestCore(ink[index2 - 1], lineEnd, pagePoint)))
              ++index2;
            else
              break;
          }
          else
            goto label_11;
        }
        return true;
      }
    }
    return false;
  }

  private static bool ProcessLineAnnotation(PdfLineAnnotation annot, Point pagePoint)
  {
    return AnnotationHitTestHelper.Contains(annot.GetRECT(), pagePoint) && annot.Line.Count == 2 && (double) annot.LeaderLineExtension == 0.0 && (double) annot.LeaderLineLenght == 0.0 && (double) annot.LeaderLineOffset == 0.0 && AnnotationHitTestHelper.LineHitTestCore(annot.Line[0], annot.Line[1], pagePoint);
  }

  private static bool LineHitTestCore(FS_POINTF lineStart, FS_POINTF lineEnd, Point point)
  {
    if ((double) lineStart.X == (double) lineEnd.X && (double) lineStart.Y == (double) lineEnd.Y)
      return new Rect((double) lineStart.X - 4.0, (double) lineStart.Y - 4.0, 8.0, 8.0).Contains(point.X, point.Y);
    if ((double) lineStart.X == (double) lineEnd.X)
      return new Rect((double) lineStart.X - 4.0, (double) Math.Min(lineStart.Y, lineEnd.Y) - 4.0, 8.0, (double) Math.Abs(lineStart.Y - lineEnd.Y) + 8.0).Contains(point.X, point.Y);
    float num1 = (float) (((double) lineStart.Y - (double) lineEnd.Y) / ((double) lineStart.X - (double) lineEnd.X));
    float num2 = lineStart.Y - num1 * lineStart.X;
    double val1 = (double) num1 * (point.X - 4.0) + (double) num2;
    double val2 = (double) num1 * (point.X + 4.0) + (double) num2;
    return point.Y >= Math.Min(val1, val2) - 4.0 && point.Y <= Math.Max(val1, val2) + 4.0;
  }

  private static bool ProcessTextMarkupAnnotation(PdfTextMarkupAnnotation annot, Point pagePoint)
  {
    return annot.QuadPoints != null && annot.QuadPoints.Count > 0 && annot.QuadPoints.Any<FS_QUADPOINTSF>((Func<FS_QUADPOINTSF, bool>) (c => AnnotationHitTestHelper.Contains(c, pagePoint)));
  }

  private static bool ProcessWidgetAnnotation(PdfWidgetAnnotation annot, Point pagePoint) => false;

  private static bool ProcessPopupAnnotation(PdfPopupAnnotation annot, Point pagePoint) => false;

  private static bool ProcessDefaultAnnotation(PdfAnnotation annot, Point pagePoint)
  {
    if (!annot.Flags.HasFlag((Enum) AnnotationFlags.NoZoom))
      return AnnotationHitTestHelper.Contains(annot.GetRECT(), pagePoint);
    PdfControl pdfControl = PdfControl.GetPdfControl(annot.Page?.Document);
    if (pdfControl == null)
      return false;
    Rect deviceBounds = Extensions.GetDeviceBounds(pdfControl.Viewer, annot);
    Point clientPoint;
    return pdfControl.TryGetClientPoint(annot.Page.PageIndex, pagePoint, out clientPoint) && deviceBounds.Contains(clientPoint);
  }

  internal static bool Contains(FS_RECTF rect, Point point)
  {
    return (double) rect.left <= point.X && (double) rect.right >= point.X && (double) rect.top >= point.Y && (double) rect.bottom <= point.Y;
  }

  internal static bool IntersecteWith(FS_RECTF rect1, FS_RECTF rect2)
  {
    float num1 = Math.Max(rect1.left, rect2.left);
    float num2 = Math.Min(rect1.top, rect2.top);
    float num3 = Math.Min(rect1.right, rect2.right);
    float num4 = Math.Max(rect1.bottom, rect2.bottom);
    return (double) num1 < (double) num3 && (double) num4 < (double) num2;
  }

  private static bool Contains(FS_QUADPOINTSF quadPoints, Point point)
  {
    float num1 = Math.Min(Math.Min(quadPoints.x1, quadPoints.x2), Math.Min(quadPoints.x3, quadPoints.x4));
    float num2 = Math.Max(Math.Max(quadPoints.x1, quadPoints.x2), Math.Max(quadPoints.x3, quadPoints.x4));
    float num3 = Math.Max(Math.Max(quadPoints.y1, quadPoints.y2), Math.Max(quadPoints.y3, quadPoints.y4));
    float num4 = Math.Min(Math.Min(quadPoints.y1, quadPoints.y2), Math.Min(quadPoints.y3, quadPoints.y4));
    return (double) num1 <= point.X && (double) num2 >= point.X && (double) num3 >= point.Y && (double) num4 <= point.Y;
  }
}
