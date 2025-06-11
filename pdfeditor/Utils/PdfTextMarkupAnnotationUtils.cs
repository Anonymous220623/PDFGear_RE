// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.PdfTextMarkupAnnotationUtils
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf;
using Patagames.Pdf.Net;
using PDFKit;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace pdfeditor.Utils;

public static class PdfTextMarkupAnnotationUtils
{
  public static IEnumerable<TextInfo> GetTextInfos(
    PdfDocument document,
    SelectInfo selectInfo,
    bool includeSpaceChar = false)
  {
    if (document != null && selectInfo.StartPage != -1 && selectInfo.StartIndex != -1 && selectInfo.EndPage != -1 && selectInfo.EndIndex != -1)
    {
      if (selectInfo.StartPage > selectInfo.EndPage)
      {
        ref int local1 = ref selectInfo.StartPage;
        ref int local2 = ref selectInfo.StartIndex;
        ref int local3 = ref selectInfo.EndPage;
        ref int local4 = ref selectInfo.EndIndex;
        int endPage = selectInfo.EndPage;
        int endIndex = selectInfo.EndIndex;
        int startPage = selectInfo.StartPage;
        int startIndex = selectInfo.StartIndex;
        local1 = endPage;
        local2 = endIndex;
        local3 = startPage;
        int num = startIndex;
        local4 = num;
      }
      else if (selectInfo.StartPage == selectInfo.EndPage && selectInfo.StartIndex > selectInfo.EndIndex)
      {
        ref int local5 = ref selectInfo.StartIndex;
        ref int local6 = ref selectInfo.EndIndex;
        int endIndex = selectInfo.EndIndex;
        int startIndex = selectInfo.StartIndex;
        local5 = endIndex;
        int num = startIndex;
        local6 = num;
      }
      for (int i = selectInfo.StartPage; i <= selectInfo.EndPage; ++i)
      {
        PdfPage page = document.Pages[i];
        int countChars = page.Text.CountChars;
        if (countChars > 0)
        {
          int num1 = 0;
          if (i == selectInfo.StartPage)
            num1 = selectInfo.StartIndex;
          int num2 = countChars;
          if (i == selectInfo.EndPage)
            num2 = selectInfo.EndIndex + 1 - num1;
          if (num2 > 0)
          {
            if (includeSpaceChar)
            {
              PdfTextInfo textInfo = page.Text.GetTextInfo(num1, num2);
              if (textInfo.Rects != null && textInfo.Rects.Count > 0)
                yield return new TextInfo(i, num1, num1 + num2 - 1, textInfo);
            }
            else
            {
              System.Collections.Generic.IReadOnlyList<FS_RECTF> withoutSpaceCharacter = PdfTextMarkupAnnotationUtils.GetRectsFromTextInfoWithoutSpaceCharacter(page, num1, num2);
              if (withoutSpaceCharacter.Count > 0)
              {
                string text = page.Text.GetText(num1, num2);
                yield return new TextInfo(i, num1, num1 + num2, text, withoutSpaceCharacter);
              }
            }
          }
        }
      }
    }
  }

  private static System.Collections.Generic.IReadOnlyList<FS_RECTF> GetRectsFromTextInfoWithoutSpaceCharacter(
    PdfPage page,
    int s,
    int len)
  {
    int num = page != null ? page.PageIndex : throw new ArgumentNullException(nameof (page));
    List<IEnumerable<FS_RECTF>> fsRectfsList = new List<IEnumerable<FS_RECTF>>();
    int index1 = -1;
    int count = 0;
    for (int index2 = s; index2 < s + len; ++index2)
    {
      if (page.Text.GetCharacter(index2) == ' ')
      {
        if (index1 >= 0)
        {
          fsRectfsList.Add((IEnumerable<FS_RECTF>) page.Text.GetTextInfo(index1, count).Rects);
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
      fsRectfsList.Add((IEnumerable<FS_RECTF>) page.Text.GetTextInfo(index1, count).Rects);
    List<FS_RECTF> withoutSpaceCharacter = new List<FS_RECTF>();
    foreach (IEnumerable<FS_RECTF> collection in fsRectfsList)
      withoutSpaceCharacter.AddRange(collection);
    return (System.Collections.Generic.IReadOnlyList<FS_RECTF>) withoutSpaceCharacter;
  }

  public static System.Collections.Generic.IReadOnlyList<FS_RECTF> GetNormalizedRects(
    PdfViewer viewer,
    TextInfo textInfo,
    bool normalizeRects = false,
    bool removeOverlap = false)
  {
    if (viewer == null)
      throw new ArgumentNullException(nameof (viewer));
    if (textInfo == null)
      throw new ArgumentNullException(nameof (textInfo));
    PdfPage page = (viewer.Document ?? throw new ArgumentNullException("viewer.Document")).Pages[textInfo.PageIndex];
    return (System.Collections.Generic.IReadOnlyList<FS_RECTF>) PdfTextMarkupAnnotationUtils.GetNormalizedRow(textInfo.Rects, (double) page.Width / 40.0, normalizeRects, removeOverlap).SelectMany<PdfTextMarkupAnnotationUtils.NormalizedRow, FS_RECTF, FS_RECTF>((Func<PdfTextMarkupAnnotationUtils.NormalizedRow, IEnumerable<FS_RECTF>>) (c => (IEnumerable<FS_RECTF>) c.Rects), (Func<PdfTextMarkupAnnotationUtils.NormalizedRow, FS_RECTF, FS_RECTF>) ((s, c) => c)).ToArray<FS_RECTF>();
  }

  private static System.Collections.Generic.IReadOnlyList<PdfTextMarkupAnnotationUtils.NormalizedRow> GetNormalizedRow(
    System.Collections.Generic.IReadOnlyList<FS_RECTF> rects,
    double minSpaceWidth,
    bool normalizeRects,
    bool removeOverlap)
  {
    if (rects == null)
      return (System.Collections.Generic.IReadOnlyList<PdfTextMarkupAnnotationUtils.NormalizedRow>) Array.Empty<PdfTextMarkupAnnotationUtils.NormalizedRow>();
    rects = (System.Collections.Generic.IReadOnlyList<FS_RECTF>) rects.Where<FS_RECTF>((Func<FS_RECTF, bool>) (c => (double) c.Width > 0.0 && (double) c.Height > 0.0)).ToArray<FS_RECTF>();
    if (rects.Count == 1)
    {
      PdfTextMarkupAnnotationUtils.NormalizedRow normalizedRow = new PdfTextMarkupAnnotationUtils.NormalizedRow(rects[0], minSpaceWidth);
      normalizedRow.Complete(normalizeRects);
      return (System.Collections.Generic.IReadOnlyList<PdfTextMarkupAnnotationUtils.NormalizedRow>) new PdfTextMarkupAnnotationUtils.NormalizedRow[1]
      {
        normalizedRow
      };
    }
    List<PdfTextMarkupAnnotationUtils.NormalizedRow> normalizedRow1 = new List<PdfTextMarkupAnnotationUtils.NormalizedRow>();
    PdfTextMarkupAnnotationUtils.NormalizedRow normalizedRow2 = new PdfTextMarkupAnnotationUtils.NormalizedRow(rects[0], minSpaceWidth);
    normalizedRow1.Add(normalizedRow2);
    for (int index = 1; index < rects.Count; ++index)
    {
      FS_RECTF rect1 = rects[index];
      FS_RECTF rect2 = rects[index - 1];
      bool flag;
      if ((double) rect2.right <= (double) rect1.right)
      {
        if (normalizedRow2.RawRects.Count == 1)
        {
          if ((double) rect1.left - (double) rect2.right > minSpaceWidth)
          {
            float height = normalizedRow2.Height;
            flag = (double) rect1.top < (double) normalizedRow2.Bottom + (double) height / 2.0 || (double) rect1.bottom > (double) normalizedRow2.Top - (double) height / 2.0;
          }
          else
          {
            double num = (double) Math.Max(normalizedRow2.Height, rect1.Height);
            flag = (double) rect1.Height >= 1.6 ? (double) rect1.top <= (double) normalizedRow2.Bottom || (double) rect1.bottom >= (double) normalizedRow2.Top : (double) rect1.top <= (double) normalizedRow2.Bottom - 2.0 || (double) rect1.bottom >= (double) normalizedRow2.Top + 2.0;
          }
        }
        else if ((double) rect1.Height < (double) normalizedRow2.Height)
        {
          double height = (double) rect1.Height;
          flag = (double) rect1.top <= (double) normalizedRow2.Bottom || (double) rect1.bottom >= (double) normalizedRow2.Top;
        }
        else
        {
          float height = normalizedRow2.Height;
          flag = (double) rect1.top < (double) normalizedRow2.Bottom + (double) height / 2.0 || (double) rect1.bottom > (double) normalizedRow2.Top - (double) height / 2.0;
        }
      }
      else
        flag = true;
      if (flag)
      {
        if (!removeOverlap)
          normalizedRow2.Complete(normalizeRects);
        normalizedRow2 = new PdfTextMarkupAnnotationUtils.NormalizedRow(rect1, minSpaceWidth);
        normalizedRow1.Add(normalizedRow2);
      }
      else
        normalizedRow2.Add(rect1);
    }
    if (removeOverlap)
    {
      for (int index = 1; index < normalizedRow1.Count; ++index)
      {
        PdfTextMarkupAnnotationUtils.NormalizedRow normalizedRow3 = normalizedRow1[index];
        PdfTextMarkupAnnotationUtils.NormalizedRow normalizedRow4 = normalizedRow1[index - 1];
        if ((double) normalizedRow3.Top > (double) normalizedRow4.Bottom && (double) normalizedRow3.Bottom < (double) normalizedRow4.Bottom && (double) Math.Min(normalizedRow3.Top, normalizedRow4.Top) - (double) Math.Max(normalizedRow3.Bottom, normalizedRow4.Bottom) < (double) Math.Min(normalizedRow3.Height, normalizedRow4.Height) / 2.0)
          normalizedRow3.Top = Math.Max(normalizedRow3.Bottom, normalizedRow4.Bottom);
        normalizedRow3.Complete(normalizeRects);
      }
    }
    return (System.Collections.Generic.IReadOnlyList<PdfTextMarkupAnnotationUtils.NormalizedRow>) normalizedRow1;
  }

  public static FS_RECTF ToPdfRect(this FS_QUADPOINTSF quadPoints)
  {
    double l = (double) Math.Min(Math.Min(quadPoints.x1, quadPoints.x2), Math.Min(quadPoints.x3, quadPoints.x4));
    float num1 = Math.Max(Math.Max(quadPoints.x1, quadPoints.x2), Math.Max(quadPoints.x3, quadPoints.x4));
    float num2 = Math.Max(Math.Max(quadPoints.y1, quadPoints.y2), Math.Max(quadPoints.y3, quadPoints.y4));
    float num3 = Math.Min(Math.Min(quadPoints.y1, quadPoints.y2), Math.Min(quadPoints.y3, quadPoints.y4));
    double t = (double) num2;
    double r = (double) num1;
    double b = (double) num3;
    return new FS_RECTF((float) l, (float) t, (float) r, (float) b);
  }

  public static FS_QUADPOINTSF ToQuadPoints(this FS_RECTF rect)
  {
    return rect.ToQuadPoints(0.0f, 0.0f, 0.0f, 0.0f);
  }

  public static FS_QUADPOINTSF ToQuadPoints(this FS_RECTF rect, float padding)
  {
    return rect.ToQuadPoints(padding, padding, padding, padding);
  }

  public static FS_QUADPOINTSF ToQuadPoints(
    this FS_RECTF rect,
    float leftRightPadding = 0.0f,
    float topBottomPadding = 0.0f)
  {
    return rect.ToQuadPoints(leftRightPadding, topBottomPadding, leftRightPadding, topBottomPadding);
  }

  public static FS_QUADPOINTSF ToQuadPoints(
    this FS_RECTF rect,
    float leftPadding = 0.0f,
    float topPadding = 0.0f,
    float rightPadding = 0.0f,
    float bottomPadding = 0.0f)
  {
    double x1 = (double) rect.left - (double) leftPadding;
    float num1 = rect.top + topPadding;
    float num2 = rect.right + rightPadding;
    float num3 = rect.top + topPadding;
    float num4 = rect.left - leftPadding;
    float num5 = rect.bottom - bottomPadding;
    float num6 = rect.right + rightPadding;
    float num7 = rect.bottom - bottomPadding;
    double y1 = (double) num1;
    double x2 = (double) num2;
    double y2 = (double) num3;
    double x3 = (double) num4;
    double y3 = (double) num5;
    double x4 = (double) num6;
    double y4 = (double) num7;
    return new FS_QUADPOINTSF((float) x1, (float) y1, (float) x2, (float) y2, (float) x3, (float) y3, (float) x4, (float) y4);
  }

  private class NormalizedRow
  {
    private readonly double minSpaceWidth;
    private List<FS_RECTF> rawRects = new List<FS_RECTF>();
    private List<FS_RECTF> rects = new List<FS_RECTF>();
    private bool completed;

    public NormalizedRow(FS_RECTF rc, double minSpaceWidth)
    {
      this.minSpaceWidth = minSpaceWidth;
      this.Add(rc);
    }

    public System.Collections.Generic.IReadOnlyList<FS_RECTF> RawRects
    {
      get => (System.Collections.Generic.IReadOnlyList<FS_RECTF>) this.rawRects;
    }

    public System.Collections.Generic.IReadOnlyList<FS_RECTF> Rects
    {
      get => (System.Collections.Generic.IReadOnlyList<FS_RECTF>) this.rects;
    }

    public float Top { get; set; } = float.MinValue;

    public float Bottom { get; set; } = float.MaxValue;

    public float Height => this.Top - this.Bottom;

    public bool Completed => this.completed;

    public void Add(FS_RECTF rect)
    {
      if (this.completed)
        throw new ArgumentException("Completed");
      this.rawRects.Add(rect);
      if (this.rects.Count == 0)
      {
        this.rects.Add(rect);
      }
      else
      {
        FS_RECTF fsRectf1 = this.rects.Last<FS_RECTF>();
        if ((double) rect.left - (double) fsRectf1.right < this.minSpaceWidth)
        {
          FS_RECTF fsRectf2 = this.rects.Last<FS_RECTF>();
          float l = Math.Min(rect.left, fsRectf2.left);
          float r = Math.Max(rect.right, fsRectf2.right);
          float t = Math.Max(rect.top, fsRectf2.top);
          float b = Math.Min(rect.bottom, fsRectf2.bottom);
          this.rects[this.rects.Count - 1] = new FS_RECTF(l, t, r, b);
        }
        else
          this.rects.Add(rect);
      }
      this.Top = Math.Max(rect.top, this.Top);
      this.Bottom = Math.Min(rect.bottom, this.Bottom);
    }

    public void Complete(bool normalizeRects)
    {
      this.completed = true;
      if (!normalizeRects)
        return;
      for (int index = 0; index < this.rects.Count; ++index)
      {
        FS_RECTF rect = this.rects[index];
        this.rects[index] = new FS_RECTF(rect.left, this.Top, rect.right, this.Bottom);
      }
    }
  }
}
