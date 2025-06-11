// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.TextInfo
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf;
using Patagames.Pdf.Net;
using System.Linq;

#nullable disable
namespace pdfeditor.Utils;

public class TextInfo
{
  public TextInfo(int pageIndex, int startIndex, int endIndex, PdfTextInfo pdfTextInfo)
    : this(pageIndex, startIndex, endIndex, pdfTextInfo.Text, (System.Collections.Generic.IReadOnlyList<FS_RECTF>) pdfTextInfo.Rects.ToArray<FS_RECTF>())
  {
  }

  public TextInfo(
    int pageIndex,
    int startIndex,
    int endIndex,
    string text,
    System.Collections.Generic.IReadOnlyList<FS_RECTF> rects)
  {
    this.PageIndex = pageIndex;
    this.StartIndex = startIndex;
    this.EndIndex = endIndex;
    this.Text = text;
    this.Rects = (System.Collections.Generic.IReadOnlyList<FS_RECTF>) rects.ToArray<FS_RECTF>();
  }

  public int PageIndex { get; }

  public int StartIndex { get; }

  public int EndIndex { get; }

  public string Text { get; }

  public System.Collections.Generic.IReadOnlyList<FS_RECTF> Rects { get; }
}
