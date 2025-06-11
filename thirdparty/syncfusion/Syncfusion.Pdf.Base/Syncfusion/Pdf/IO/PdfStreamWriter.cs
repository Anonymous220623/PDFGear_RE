// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.IO.PdfStreamWriter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.ColorSpace;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections;
using System.Drawing;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.IO;

internal class PdfStreamWriter : IPdfWriter
{
  private PdfStream m_stream;

  public PdfStreamWriter(PdfStream stream)
  {
    this.m_stream = stream != null ? stream : throw new ArgumentNullException(nameof (stream));
  }

  public void ModifyTM(PdfTransformationMatrix matrix)
  {
    this.m_stream.Write(matrix.ToString());
    this.m_stream.Write(" ");
    this.WriteOperator("Tm");
  }

  public void SetFont(PdfFont font, string name, float size)
  {
    this.SetFont(font, new PdfName(name), size);
  }

  public void SetFont(PdfFont font, PdfName name, float size)
  {
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    this.m_stream.Write(name.ToString());
    this.m_stream.Write(" ");
    this.m_stream.Write(PdfNumber.FloatToString(size));
    this.m_stream.Write(" ");
    this.WriteOperator("Tf");
  }

  public void SetColorSpace(PdfColorSpaces colorspace, PdfName name)
  {
    if (colorspace == null)
      throw new ArgumentNullException("Color Space");
    this.m_stream.Write(name.ToString());
    this.m_stream.Write(" ");
    this.WriteOperator("CS");
    this.m_stream.Write(name.ToString());
    this.m_stream.Write(" ");
    this.WriteOperator("cs");
  }

  public void SetCharacterSpacing(float charSpacing)
  {
    this.m_stream.Write(PdfNumber.FloatToString(charSpacing));
    this.m_stream.Write(" ");
    this.m_stream.Write("Tc");
    this.m_stream.Write("\r\n");
  }

  public void SetWordSpacing(float wordSpacing)
  {
    this.m_stream.Write(PdfNumber.FloatToString(wordSpacing));
    this.m_stream.Write(" ");
    this.WriteOperator("Tw");
  }

  public void SetHorizontalScaling(float scalingFactor)
  {
    this.m_stream.Write(PdfNumber.FloatToString(scalingFactor));
    this.m_stream.Write(" ");
    this.WriteOperator("Tz");
  }

  public void SetLeading(float leading)
  {
    this.m_stream.Write(PdfNumber.FloatToString(leading));
    this.m_stream.Write(" ");
    this.WriteOperator("TL");
  }

  public void SetTextRenderingMode(TextRenderingMode renderingMode)
  {
    this.m_stream.Write(((int) renderingMode).ToString());
    this.m_stream.Write(" ");
    this.WriteOperator("Tr");
  }

  public void SetTextRise(float rise)
  {
    this.m_stream.Write(PdfNumber.FloatToString(rise));
    this.m_stream.Write(" ");
    this.WriteOperator("Ts");
  }

  internal void SetTextScaling(float textScaling)
  {
    this.m_stream.Write(PdfNumber.FloatToString(textScaling));
    this.m_stream.Write(" ");
    this.WriteOperator("Tz");
  }

  public void StartNextLine() => this.WriteOperator("T*");

  public void StartNextLine(PointF point)
  {
    this.WritePoint(point);
    this.WriteOperator("Td");
  }

  public void StartNextLine(float x, float y)
  {
    this.WritePoint(x, y);
    this.WriteOperator("Td");
  }

  public void StartLineAndSetLeading(PointF point)
  {
    this.WritePoint(point);
    this.WriteOperator("TD");
  }

  public void StartLineAndSetLeading(float x, float y)
  {
    this.WritePoint(x, y);
    this.WriteOperator("TD");
  }

  public void ShowText(byte[] text, bool hex)
  {
    this.CheckTextParam(text);
    this.WriteText(text, hex);
    this.WriteOperator("Tj");
  }

  public void ShowText(string text, bool hex)
  {
    this.CheckTextParam(text);
    this.WriteText(text, hex);
    this.WriteOperator("Tj");
  }

  public void ShowText(PdfString text)
  {
    this.CheckTextParam(text);
    this.WriteText(text);
    this.WriteOperator("Tj");
  }

  public void ShowText(PdfArray formattedText)
  {
    if (formattedText == null)
      throw new ArgumentNullException(nameof (formattedText));
    formattedText.Save((IPdfWriter) this);
    this.WriteOperator("TJ");
  }

  internal void ShowFormatedText(string text)
  {
    this.m_stream.Write(text);
    this.WriteOperator("TJ");
  }

  public void ShowNextLineText(byte[] text, bool hex)
  {
    this.CheckTextParam(text);
    this.WriteText(text, hex);
    this.WriteOperator("'");
  }

  public void ShowNextLineText(string text, bool hex)
  {
    this.CheckTextParam(text);
    this.WriteText(text, hex);
    this.WriteOperator("'");
  }

  public void ShowNextLineText(PdfString text)
  {
    this.CheckTextParam(text);
    this.WriteText(text);
    this.WriteOperator("'");
  }

  public void ShowNextLineTextWithSpacings(
    float wordSpacing,
    float charSpacing,
    byte[] text,
    bool hex)
  {
    this.CheckTextParam(text);
    this.WritePoint(wordSpacing, charSpacing);
    this.WriteText(text, hex);
    this.WriteOperator("\"");
  }

  public void ShowNextLineTextWithSpacings(
    float wordSpacing,
    float charSpacing,
    string text,
    bool hex)
  {
    this.CheckTextParam(text);
    this.WritePoint(wordSpacing, charSpacing);
    this.WriteText(text, hex);
    this.WriteOperator("\"");
  }

  public void ShowNextLineTextWithSpacings(float wordSpacing, float charSpacing, PdfString text)
  {
    this.CheckTextParam(text);
    this.WritePoint(wordSpacing, charSpacing);
    this.WriteText(text);
    this.WriteOperator("\"");
  }

  public void ShowText(IList formatting)
  {
    if (formatting == null)
      throw new ArgumentNullException(nameof (formatting));
    this.m_stream.Write("[");
    foreach (object text in (IEnumerable) formatting)
    {
      switch (text)
      {
        case null:
          throw new ArgumentException("Invalid formatting", nameof (formatting));
        case PdfNumber _:
          this.m_stream.Write((text as PdfNumber).IntValue.ToString());
          this.m_stream.Write(" ");
          continue;
        case int _:
          this.m_stream.Write(text.ToString());
          this.m_stream.Write(" ");
          continue;
        default:
          this.WriteText(text);
          continue;
      }
    }
    this.m_stream.Write("]");
    this.WriteOperator("TJ");
  }

  public void BeginText() => this.WriteOperator("BT");

  internal void WriteTag(string tag)
  {
    this.m_stream.Write(tag);
    this.m_stream.Write("\r\n");
  }

  public void EndText() => this.WriteOperator("ET");

  public void BeginMarkupSequence(string name)
  {
    if (name == null || name == string.Empty)
      throw new ArgumentNullException(nameof (name));
    this.m_stream.Write("/");
    this.m_stream.Write(name);
    this.m_stream.Write(" ");
    this.WriteOperator("BMC");
  }

  public void BeginMarkupSequence(PdfName name)
  {
    if (name == (PdfName) null)
      throw new ArgumentNullException(nameof (name));
    this.m_stream.Write(name.ToString());
    this.m_stream.Write(" ");
    this.WriteOperator("BMC");
  }

  public void EndMarkupSequence() => this.WriteOperator("EMC");

  public void WriteComment(string comment)
  {
    if (comment == null || comment.Length <= 0)
      return;
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("%");
    stringBuilder.Append(" ");
    stringBuilder.Append(comment);
    this.WriteOperator(stringBuilder.ToString());
  }

  public void BeginPath(PointF startPoint) => this.BeginPath(startPoint.X, startPoint.Y);

  public void BeginPath(float x, float y)
  {
    this.WritePoint(x, y);
    this.WriteOperator("m");
  }

  public void AppendBezierSegment(PointF p1, PointF p2, PointF p3)
  {
    this.AppendBezierSegment(p1.X, p1.Y, p2.X, p2.Y, p3.X, p3.Y);
  }

  public void AppendBezierSegment(float x1, float y1, float x2, float y2, float x3, float y3)
  {
    this.WritePoint(x1, y1);
    this.WritePoint(x2, y2);
    this.WritePoint(x3, y3);
    this.WriteOperator("c");
  }

  public void AppendBezierSegment(PointF p2, PointF p3, bool useFirstPoint)
  {
    this.AppendBezierSegment(p2.X, p2.Y, p3.X, p3.Y, useFirstPoint);
  }

  public void AppendBezierSegment(float x2, float y2, float x3, float y3, bool useFirstPoint)
  {
    this.WritePoint(x2, y2);
    this.WritePoint(x3, y3);
    if (useFirstPoint)
      this.WriteOperator("y");
    else
      this.WriteOperator("v");
  }

  public void AppendLineSegment(PointF point) => this.AppendLineSegment(point.X, point.Y);

  public void AppendLineSegment(float x, float y)
  {
    this.WritePoint(x, y);
    this.WriteOperator("l");
  }

  public void AppendRectangle(RectangleF rect)
  {
    this.AppendRectangle(rect.X, rect.Y, rect.Width, rect.Height);
  }

  public void AppendRectangle(float x, float y, float width, float height)
  {
    this.WritePoint(x, y);
    this.WritePoint(width, height);
    this.WriteOperator("re");
  }

  public void ClosePath() => this.WriteOperator("h");

  public void CloseSubPath() => this.WriteOperator("h");

  public void FillPath(bool useEvenOddRule)
  {
    this.m_stream.Write("f");
    if (useEvenOddRule)
      this.m_stream.Write("*");
    this.m_stream.Write("\r\n");
  }

  public void StrokePath() => this.WriteOperator("S");

  public void FillStrokePath(bool useEvenOddRule)
  {
    this.m_stream.Write("B");
    if (useEvenOddRule)
      this.m_stream.Write("*");
    this.m_stream.Write("\r\n");
  }

  public void CloseStrokePath() => this.WriteOperator("s");

  public void CloseFillStrokePath(bool useEvenOddRule)
  {
    this.m_stream.Write("b");
    if (useEvenOddRule)
      this.m_stream.Write("*");
    this.m_stream.Write("\r\n");
  }

  public void CloseFillPath(bool useEvenOddRule)
  {
    this.WriteOperator("h");
    this.m_stream.Write("f");
    if (useEvenOddRule)
      this.m_stream.Write("*");
    this.m_stream.Write("\r\n");
  }

  public void ClipPath(bool useEvenOddRule)
  {
    this.m_stream.Write("W");
    if (useEvenOddRule)
      this.m_stream.Write("*");
    this.m_stream.Write(" ");
    this.m_stream.Write("n");
    this.m_stream.Write("\r\n");
  }

  public void EndPath() => this.WriteOperator("n");

  public void SaveGraphicsState() => this.WriteOperator("q");

  public void RestoreGraphicsState() => this.WriteOperator("Q");

  public void ModifyCTM(PdfTransformationMatrix matrix)
  {
    if (matrix == null)
      throw new ArgumentNullException(nameof (matrix));
    this.m_stream.Write(matrix.ToString());
    this.m_stream.Write(" ");
    this.WriteOperator("cm");
  }

  public void SetLineWidth(float width)
  {
    this.m_stream.Write(PdfNumber.FloatToString(width));
    this.m_stream.Write(" ");
    this.WriteOperator("w");
  }

  public void SetLineCap(PdfLineCap lineCapStyle)
  {
    this.m_stream.Write(((int) lineCapStyle).ToString());
    this.m_stream.Write(" ");
    this.WriteOperator("J");
  }

  public void SetLineJoin(PdfLineJoin lineJoinStyle)
  {
    this.m_stream.Write(((int) lineJoinStyle).ToString());
    this.m_stream.Write(" ");
    this.WriteOperator("j");
  }

  public void SetMiterLimit(float miterLimit)
  {
    this.m_stream.Write(PdfNumber.FloatToString(miterLimit));
    this.m_stream.Write(" ");
    this.WriteOperator("M");
  }

  public void SetLineDashPattern(float[] pattern, float patternOffset)
  {
    this.SetLineDashPattern(new PdfArray(pattern), new PdfNumber(patternOffset));
  }

  private void SetLineDashPattern(PdfArray pattern, PdfNumber patternOffset)
  {
    pattern.Save((IPdfWriter) this);
    this.m_stream.Write(" ");
    patternOffset.Save((IPdfWriter) this);
    this.m_stream.Write(" ");
    this.WriteOperator("d");
  }

  public void SetColorRenderingIntent(ColorIntent intent)
  {
    this.m_stream.Write("/");
    this.m_stream.Write(intent.ToString());
    this.m_stream.Write(" ");
    this.WriteOperator("ri");
  }

  public void SetFlatnessTolerance(int tolerance)
  {
    this.m_stream.Write(PdfNumber.FloatToString((float) tolerance));
    this.m_stream.Write(" ");
    this.WriteOperator("i");
  }

  public void SetGraphicsState(PdfName dictionaryName)
  {
    if (dictionaryName == (PdfName) null || dictionaryName == (object) string.Empty)
      throw new ArgumentNullException(nameof (dictionaryName));
    this.m_stream.Write(dictionaryName.ToString());
    this.m_stream.Write(" ");
    this.WriteOperator("gs");
  }

  public void SetGraphicsState(string dictionaryName)
  {
    if (dictionaryName == null || dictionaryName == string.Empty)
      throw new ArgumentNullException(nameof (dictionaryName));
    this.m_stream.Write("/");
    this.m_stream.Write(dictionaryName);
    this.m_stream.Write(" ");
    this.WriteOperator("gs");
  }

  public void SetColorSpace(string name, bool forStroking)
  {
    this.SetColorSpace(new PdfName(name), forStroking);
  }

  public void SetColorSpace(PdfName name, bool forStroking)
  {
    if (name == (PdfName) null)
      throw new ArgumentNullException(nameof (name));
    string text = forStroking ? "CS" : "cs";
    this.m_stream.Write(name.ToString());
    this.m_stream.Write(" ");
    this.m_stream.Write(text);
    this.m_stream.Write("\r\n");
  }

  public void SetColorAndSpace(PdfColor color, PdfColorSpace colorSpace, bool forStroking)
  {
    if (color.IsEmpty)
      return;
    this.m_stream.Write(color.ToString(colorSpace, forStroking));
    this.m_stream.Write("\r\n");
  }

  public void SetColorAndSpace(
    PdfColor color,
    PdfColorSpace colorSpace,
    bool forStroking,
    bool check)
  {
    if (color.IsEmpty)
      return;
    this.m_stream.Write(color.CalToString(colorSpace, forStroking));
    this.m_stream.Write("\r\n");
  }

  public void SetColorAndSpace(
    PdfColor color,
    PdfColorSpace colorSpace,
    bool forStroking,
    bool check,
    bool iccbased)
  {
    if (color.IsEmpty)
      return;
    this.m_stream.Write(color.IccColorToString(colorSpace, forStroking));
    this.m_stream.Write("\r\n");
  }

  public void SetColorAndSpace(
    PdfColor color,
    PdfColorSpace colorSpace,
    bool forStroking,
    bool check,
    bool iccbased,
    bool indexed)
  {
    if (color.IsEmpty)
      return;
    this.m_stream.Write(color.IndexedToString(forStroking));
    this.m_stream.Write("\r\n");
  }

  public void SetColor(PdfColor color, PdfColorSpace currentSpace, bool forStroking)
  {
    string opcode = forStroking ? "SC" : "sc";
    switch (currentSpace)
    {
      case PdfColorSpace.RGB:
        this.m_stream.Write(PdfNumber.FloatToString(color.Red));
        this.m_stream.Write(" ");
        this.m_stream.Write(PdfNumber.FloatToString(color.Green));
        this.m_stream.Write(" ");
        this.m_stream.Write(PdfNumber.FloatToString(color.Blue));
        this.m_stream.Write(" ");
        break;
      case PdfColorSpace.CMYK:
        this.m_stream.Write(PdfNumber.FloatToString(color.C));
        this.m_stream.Write(" ");
        this.m_stream.Write(PdfNumber.FloatToString(color.M));
        this.m_stream.Write(" ");
        this.m_stream.Write(PdfNumber.FloatToString(color.Y));
        this.m_stream.Write(" ");
        this.m_stream.Write(PdfNumber.FloatToString(color.K));
        this.m_stream.Write(" ");
        break;
      case PdfColorSpace.GrayScale:
        this.m_stream.Write(PdfNumber.FloatToString(color.Gray));
        break;
      default:
        throw new ArgumentException("Unknown current color space");
    }
    this.WriteOperator(opcode);
  }

  public void SetColourWithPattern(IList colours, PdfName patternName, bool forStroking)
  {
    if (colours != null)
    {
      int index = 0;
      for (int count = colours.Count; index < count; ++index)
      {
        this.m_stream.Write(colours[index].ToString());
        this.m_stream.Write(" ");
      }
    }
    if (patternName != (PdfName) null)
    {
      this.m_stream.Write(patternName.ToString());
      this.m_stream.Write(" ");
    }
    if (forStroking)
      this.WriteOperator("SCN");
    else
      this.WriteOperator("scn");
  }

  public void ExecuteObject(string name) => this.ExecuteObject(new PdfName(name));

  public void ExecuteObject(PdfName name)
  {
    this.m_stream.Write(name.ToString());
    this.m_stream.Write(" ");
    this.WriteOperator("Do");
  }

  internal PdfStream GetStream() => this.m_stream;

  internal void Clear() => this.m_stream.Clear();

  private void WritePoint(PointF point) => this.WritePoint(point.X, point.Y);

  private void WritePoint(float x, float y)
  {
    this.m_stream.Write(PdfNumber.FloatToString(x));
    this.m_stream.Write(" ");
    y = PdfGraphics.UpdateY(y);
    this.m_stream.Write(PdfNumber.FloatToString(y));
    this.m_stream.Write(" ");
  }

  private void WriteText(object text)
  {
    switch (text)
    {
      case PdfString _:
        this.WriteText(text as PdfString);
        break;
      case string _:
        this.WriteText((object) (text as string));
        break;
      case byte[] _:
        this.WriteText((object) (text as byte[]));
        break;
      default:
        throw new ArgumentException("Unknown text format", nameof (text));
    }
  }

  private void WriteText(byte[] text, bool hex)
  {
    char symbol1;
    char symbol2;
    if (hex)
    {
      symbol1 = "<>"[0];
      symbol2 = "<>"[1];
    }
    else
    {
      symbol1 = "()"[0];
      symbol2 = "()"[1];
    }
    this.m_stream.Write(symbol1);
    if (hex)
      this.m_stream.Write(PdfString.BytesToHex(text));
    else
      this.m_stream.Write(text);
    this.m_stream.Write(symbol2);
  }

  private void WriteText(string text, bool hex)
  {
    char symbol1;
    char symbol2;
    if (hex)
    {
      symbol1 = "<>"[0];
      symbol2 = "<>"[1];
    }
    else
    {
      symbol1 = "()"[0];
      symbol2 = "()"[1];
    }
    this.m_stream.Write(symbol1);
    this.m_stream.Write(text);
    this.m_stream.Write(symbol2);
  }

  private void WriteText(PdfString text)
  {
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    this.m_stream.Write(text.PdfEncode((PdfDocumentBase) null));
  }

  private void WriteOperator(string opcode)
  {
    this.m_stream.Write(opcode);
    this.m_stream.Write("\r\n");
  }

  private void CheckTextParam(byte[] text)
  {
    if (text == null)
      throw new ArgumentNullException(nameof (text));
  }

  private void CheckTextParam(string text)
  {
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    if (text == string.Empty)
      throw new ArgumentException("The text can't be an empty string", nameof (text));
  }

  private void CheckTextParam(PdfString text)
  {
    if (text == null)
      throw new ArgumentNullException(nameof (text));
  }

  public long Position
  {
    get => this.m_stream.InternalStream.Position;
    set => throw new Exception("The method or operation is not implemented.");
  }

  public long Length => this.m_stream.InternalStream.Length;

  public PdfDocumentBase Document
  {
    get => (PdfDocumentBase) null;
    set => throw new Exception("The method or operation is not implemented.");
  }

  public void Write(IPdfPrimitive pdfObject) => pdfObject.Save((IPdfWriter) this);

  public void Write(long number) => this.m_stream.Write(number.ToString());

  public void Write(float number) => this.m_stream.Write(PdfNumber.FloatToString(number));

  public void Write(string text) => this.m_stream.Write(text);

  public void Write(char[] text) => this.m_stream.Write(new string(text));

  public void Write(byte[] data) => this.m_stream.Write(data);
}
