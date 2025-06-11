// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PdfRichTextStrings.TextSegmentInfo
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System.Windows;
using System.Windows.Documents;

#nullable disable
namespace PDFKit.Utils.PdfRichTextStrings;

internal struct TextSegmentInfo
{
  public TextSegmentInfo(
    Run run,
    TextRange range,
    Rect bounds,
    Rect tightBounds,
    int lineIndex,
    double baseline)
  {
    this.Run = run;
    this.Range = range;
    this.Bounds = bounds;
    this.TightBounds = tightBounds;
    this.LineIndex = lineIndex;
    this.Text = range.Text ?? "";
    this.Baseline = baseline;
    this.HasVisibleElement = !string.IsNullOrWhiteSpace(this.Text);
  }

  public TextRange Range { get; }

  public Rect Bounds { get; }

  public Rect TightBounds { get; }

  public double Baseline { get; set; }

  public Run Run { get; }

  public int LineIndex { get; }

  public string Text { get; }

  public string TextUnicode => PdfRichTextStringHelper.GetUnicodeText(this.Text);

  public bool HasVisibleElement { get; }
}
