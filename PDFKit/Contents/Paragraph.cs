// Decompiled with JetBrains decompiler
// Type: PDFKit.Contents.Paragraph
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Net;
using PDFKit.Contents.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

#nullable disable
namespace PDFKit.Contents;

public class Paragraph
{
  private List<TextLine> lines;
  private FS_RECTF? boundingBox;
  private FS_RECTF? boundingBoxFitWithCarets;
  private object locker = new object();

  public Paragraph(PdfPage page, int id)
  {
    this.Id = id;
    this.Page = page;
    this.lines = new List<TextLine>();
  }

  internal PdfPage Page { get; }

  public string ParagraphText
  {
    get
    {
      return this.Lines.Aggregate<TextLine, StringBuilder>(new StringBuilder(), (Func<StringBuilder, TextLine, StringBuilder>) ((s, c) => s.Append(c.LineText))).ToString();
    }
  }

  public int Id { get; }

  public bool IsRotate { get; private set; }

  public System.Collections.Generic.IReadOnlyList<TextLine> Lines
  {
    get => (System.Collections.Generic.IReadOnlyList<TextLine>) this.lines;
  }

  public FS_RECTF GetBoundingBox(bool fitWithCarets)
  {
    lock (this.locker)
    {
      if (this.Lines.Count == 0)
        return new FS_RECTF();
      return !fitWithCarets ? GetBoundingBoxCore() : GetBoundingBoxFitWithCaretsCore();
    }

    FS_RECTF GetBoundingBoxCore()
    {
      if (!this.boundingBox.HasValue)
      {
        FS_RECTF? nullable = new FS_RECTF?();
        foreach (TextLine line in (IEnumerable<TextLine>) this.Lines)
        {
          FS_RECTF boundingBox = line.GetBoundingBox(false);
          if (!boundingBox.IsEmpty() && (double) boundingBox.left != 0.0 && (double) boundingBox.top != 0.0)
            nullable = !nullable.HasValue ? new FS_RECTF?(boundingBox) : new FS_RECTF?(nullable.Value.Union(boundingBox));
          else if (!nullable.HasValue)
            nullable = new FS_RECTF?(boundingBox);
        }
        this.boundingBox = nullable;
      }
      return this.boundingBox.Value;
    }

    FS_RECTF GetBoundingBoxFitWithCaretsCore()
    {
      if (!this.boundingBoxFitWithCarets.HasValue)
      {
        FS_RECTF? nullable = new FS_RECTF?();
        foreach (TextLine line in (IEnumerable<TextLine>) this.Lines)
        {
          FS_RECTF boundingBox = line.GetBoundingBox(true);
          if (!boundingBox.IsEmpty() || (double) boundingBox.left != 0.0 && (double) boundingBox.top != 0.0)
            nullable = !nullable.HasValue ? new FS_RECTF?(boundingBox) : new FS_RECTF?(nullable.Value.Union(boundingBox));
          else if (!nullable.HasValue)
            nullable = new FS_RECTF?(boundingBox);
        }
        this.boundingBoxFitWithCarets = nullable;
      }
      return this.boundingBoxFitWithCarets.Value;
    }
  }

  public FS_RECTF OuterBox
  {
    get
    {
      lock (this.locker)
      {
        FS_RECTF boundingBox = this.GetBoundingBox(true);
        boundingBox.Inflate(new FS_RECTF(2f, 2f, 2f, 2f));
        return boundingBox;
      }
    }
  }

  public bool IsVertWriteMode { get; private set; }

  public bool Ended { get; internal set; }

  public AlignType Align { get; internal set; }

  public void InvalidateBoundingBox(CalcBoundingBoxOrientation orientation = CalcBoundingBoxOrientation.All)
  {
    lock (this.locker)
    {
      if (orientation == CalcBoundingBoxOrientation.All)
      {
        this.boundingBox = new FS_RECTF?();
        this.boundingBoxFitWithCarets = new FS_RECTF?();
      }
      else
      {
        if (this.boundingBox.HasValue)
        {
          FS_RECTF fsRectf = this.boundingBox.Value;
          this.boundingBox = new FS_RECTF?();
          FS_RECTF boundingBox = this.GetBoundingBox(false);
          switch (orientation)
          {
            case CalcBoundingBoxOrientation.Horizontal:
              boundingBox.top = fsRectf.top;
              boundingBox.bottom = fsRectf.bottom;
              break;
            case CalcBoundingBoxOrientation.Vertical:
              boundingBox.left = fsRectf.left;
              boundingBox.right = fsRectf.right;
              break;
          }
          this.boundingBox = new FS_RECTF?(boundingBox);
        }
        if (this.boundingBoxFitWithCarets.HasValue)
        {
          FS_RECTF fsRectf = this.boundingBoxFitWithCarets.Value;
          this.boundingBoxFitWithCarets = new FS_RECTF?();
          FS_RECTF boundingBox = this.GetBoundingBox(true);
          switch (orientation)
          {
            case CalcBoundingBoxOrientation.Horizontal:
              boundingBox.top = fsRectf.top;
              boundingBox.bottom = fsRectf.bottom;
              break;
            case CalcBoundingBoxOrientation.Vertical:
              boundingBox.left = fsRectf.left;
              boundingBox.right = fsRectf.right;
              break;
          }
          this.boundingBoxFitWithCarets = new FS_RECTF?(boundingBox);
        }
      }
    }
  }

  public void AddLine(TextLine line)
  {
    lock (this.locker)
    {
      FS_RECTF boundingBox1 = line.GetBoundingBox(false);
      for (int index = 0; index < this.lines.Count; ++index)
      {
        FS_RECTF boundingBox2 = this.lines[index].GetBoundingBox(false);
        if ((double) boundingBox1.top >= (double) boundingBox2.top)
        {
          this.lines.Insert(index, line);
          this.InvalidateBoundingBox();
          return;
        }
      }
      this.lines.Add(line);
      this.InvalidateBoundingBox();
    }
  }

  public void InsertLine(int index, TextLine line)
  {
    lock (this.locker)
      this.lines.Insert(index, line);
  }

  public void RemoveLine(int index)
  {
    lock (this.locker)
    {
      if (index < 0 || index >= this.lines.Count)
        return;
      this.RemoveLine(this.lines[index]);
    }
  }

  public void RemoveLine(TextLine line)
  {
    lock (this.locker)
    {
      if (!this.lines.Remove(line))
        return;
      if (line.TextObjects.Count > 0)
      {
        PdfTextObject textStart = line.TextObjects.FirstOrDefault<PdfTextObject>();
        PdfTextObject textEnd = line.TextObjects.LastOrDefault<PdfTextObject>();
        line.RemoveText(textStart, 0, textEnd, textEnd.CharsCount - 1);
      }
      this.InvalidateBoundingBox();
    }
  }

  internal PdfTextObject InsertText(
    TextLine line,
    PdfTextObject textObj,
    int insertAt,
    string text,
    PdfFont font,
    float fontSize,
    FS_COLOR textColor,
    FS_COLOR strokeColor,
    BoldItalicFlags boldItalic,
    int underlineStrikeout,
    ScriptEnum script,
    CultureInfo cultureInfo = null)
  {
    lock (this.locker)
      return line.InsertText(textObj, insertAt, text, font, fontSize, textColor, strokeColor, boldItalic, underlineStrikeout, script, cultureInfo);
  }

  internal PdfTextObject InsertText(
    TextLine line,
    float x,
    float y,
    string text,
    PdfFont font,
    float fontSize,
    FS_COLOR textColor,
    FS_COLOR strokeColor,
    BoldItalicFlags boldItalic,
    int underlineStrikeout,
    ScriptEnum script)
  {
    lock (this.locker)
      return line.InsertTextAtBlank(x, y, text, font, fontSize, textColor, strokeColor, boldItalic, underlineStrikeout, script);
  }

  internal PdfTextObject InsertTextToEmptyLine(
    TextLine line,
    PdfPageObjectsCollection container,
    int insertAt,
    string text,
    PdfFont font,
    float fontSize,
    FS_COLOR textColor,
    FS_COLOR strokeColor,
    BoldItalicFlags boldItalic,
    int underlineStrikeout,
    ScriptEnum script)
  {
    lock (this.locker)
      return line.InsertTextToEmptyLine(container, insertAt, text, font, fontSize, textColor, strokeColor, boldItalic, underlineStrikeout, script);
  }

  internal void OffsetLinesVert(float offsetY, TextLine line)
  {
    lock (this.locker)
    {
      int num = this.lines.IndexOf(line);
      if (num <= -1)
        return;
      for (int index = num; index < this.lines.Count; ++index)
        this.lines[index].Offset(0.0f, offsetY);
      this.InvalidateBoundingBox(CalcBoundingBoxOrientation.Vertical);
    }
  }

  internal void NewEmpty(float x, float y, float width, float height)
  {
    lock (this.locker)
    {
      this.AddLine(new TextLine(this.Page));
      FS_RECTF fsRectf = new FS_RECTF(x, y + height, x + width, y);
      this.boundingBox = new FS_RECTF?(fsRectf);
      this.boundingBoxFitWithCarets = new FS_RECTF?(fsRectf);
    }
  }

  internal void RemoveAllLines()
  {
    lock (this.locker)
    {
      for (int index = this.lines.Count - 1; index >= 0; --index)
      {
        TextLine line = this.lines[index];
        if (line.TextObjects.Count > 0)
        {
          PdfTextObject textObject1 = line.TextObjects[0];
          PdfTextObject textObject2 = line.TextObjects[line.TextObjects.Count - 1];
          line.RemoveText(textObject1, 0, textObject2, textObject2.CharsCount - 1);
        }
        this.lines.RemoveAt(index);
      }
    }
  }

  internal Paragraph Clone()
  {
    lock (this.locker)
    {
      Paragraph paragraph = new Paragraph(this.Page, this.Id);
      for (int index = 0; index < this.Lines.Count; ++index)
        paragraph.AddLine(this.Lines[index].Clone());
      paragraph.Align = this.Align;
      paragraph.IsVertWriteMode = this.IsVertWriteMode;
      paragraph.IsRotate = this.IsRotate;
      return paragraph;
    }
  }
}
