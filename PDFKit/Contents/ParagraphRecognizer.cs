// Decompiled with JetBrains decompiler
// Type: PDFKit.Contents.ParagraphRecognizer
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using PDFKit.Contents.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace PDFKit.Contents;

public class ParagraphRecognizer
{
  private readonly PdfPage page;
  private readonly PdfPageObjectsCollection objects;
  private readonly bool form;
  private bool isVert = false;
  private int paths = 0;
  private int curParaId = -1;
  private List<Paragraph> paragraphs;

  public ParagraphRecognizer(PdfPage page, PdfPageObjectsCollection objects, bool form)
  {
    this.page = page;
    this.objects = objects;
    this.form = form;
    this.paragraphs = new List<Paragraph>();
    this.Initialize();
  }

  public System.Collections.Generic.IReadOnlyList<Paragraph> Paragraphs
  {
    get => (System.Collections.Generic.IReadOnlyList<Paragraph>) this.paragraphs;
  }

  private void Initialize()
  {
    List<TextLine> pageObjList = this.ParsePageObjList((IEnumerable<PdfPageObject>) this.objects, false);
    int index1 = 0;
    while (index1 < pageObjList.Count)
    {
      TextLine textLine = pageObjList[index1];
      int count = textLine.TextObjects.Count;
      if (count > 0)
      {
        while (textLine.TextObjects[count - 1].GetText().All<char>((Func<char, bool>) (c => c == ' ')))
        {
          textLine.RemoveObj(count - 1);
          --count;
          if (count <= 0)
            break;
        }
      }
      if (count == 0)
      {
        pageObjList.RemoveAt(index1);
        int num = count - 1;
      }
      else
        ++index1;
    }
    for (int index2 = 0; index2 < pageObjList.Count; ++index2)
      this.ProcessLine(pageObjList, index2);
  }

  private List<TextLine> ParsePageObjList(IEnumerable<PdfPageObject> pageObjs, bool form)
  {
    List<TextLine> _lines = new List<TextLine>();
    ParsePageObjListCore(pageObjs, form, _lines);
    return _lines;

    void ParsePageObjListCore(
      IEnumerable<PdfPageObject> _pageObjs,
      bool _form,
      List<TextLine> _lines)
    {
      foreach (PdfPageObject pageObj in _pageObjs)
      {
        if (pageObj != null && pageObj.ObjectType == PageObjectTypes.PDFPAGE_TEXT)
        {
          if (((PdfTextObject) pageObj).CharsCount > 0)
            this.ProcessTextObj((PdfTextObject) pageObj, this.isVert, _lines);
        }
        else if (pageObj != null && pageObj.ObjectType == PageObjectTypes.PDFPAGE_PATH)
          ++this.paths;
        else if (pageObj != null && pageObj.ObjectType == PageObjectTypes.PDFPAGE_FORM)
        {
          PdfFormObject pdfFormObject = (PdfFormObject) pageObj;
          if (pdfFormObject.PageObjects != null && pdfFormObject.PageObjects.Count > 0)
            ParsePageObjListCore((IEnumerable<PdfPageObject>) pdfFormObject.PageObjects, true, _lines);
        }
      }
    }
  }

  private void ProcessTextObj(PdfTextObject textObj, bool isVert, List<TextLine> lines)
  {
    if (textObj.IsRotate())
      return;
    if (lines != null && lines.Count > 0)
    {
      for (int index = lines.Count - 1; index >= 0; --index)
      {
        if (lines[index] != null && lines[index].AddText(textObj, isVert))
          return;
      }
    }
    TextLine textLine = new TextLine(this.page);
    textLine.AddText(textObj, isVert);
    lines.Add(textLine);
  }

  private void ProcessLine(List<TextLine> lines, int index)
  {
    TextLine line = lines[index];
    Paragraph para = this.paragraphs.LastOrDefault<Paragraph>();
    if (para != null && this.AddLineToPara(para, lines, index))
      return;
    Paragraph paragraph = new Paragraph(this.page, this.GetCurParaID());
    paragraph.AddLine(line);
    this.paragraphs.Add(paragraph);
  }

  private bool AddLineToPara(Paragraph para, List<TextLine> lines, int lineIndex)
  {
    TextLine line1 = lines[lineIndex];
    if (para.IsRotate != line1.IsRotate)
      return false;
    AlignType align = para.Align;
    TextLine textLine = para.Lines.Last<TextLine>();
    if (!para.IsRotate)
    {
      bool flag = textLine.IsBulletHeader || textLine.IsFollowingUpon;
      FS_RECTF fsRectf1 = flag ? line1.BoundingBoxWithoutBellet : line1.GetBoundingBox(false);
      FS_RECTF fsRectf2 = flag ? textLine.BoundingBoxWithoutBellet : textLine.GetBoundingBox(false);
      FS_RECTF boundingBox1 = para.GetBoundingBox(false);
      if (!para.IsVertWriteMode)
      {
        if ((double) Math.Abs(fsRectf2.bottom - fsRectf1.top) > 30.0)
        {
          para.Ended = true;
          return false;
        }
        if (this.paths < 2000 && !this.form)
        {
          FS_RECTF fsRectf3 = fsRectf2;
          FS_RECTF fsRectf4 = fsRectf1;
          if ((double) fsRectf3.top < (double) fsRectf4.top)
          {
            fsRectf3 = fsRectf1;
            fsRectf4 = fsRectf2;
          }
          foreach (PdfPageObject pdfPageObject in this.objects)
          {
            if ((pdfPageObject != null ? (pdfPageObject.ObjectType != PageObjectTypes.PDFPAGE_TEXT ? 1 : 0) : 1) != 0 && (pdfPageObject == null || pdfPageObject.ObjectType != (PageObjectTypes.PDFPAGE_PATH | PageObjectTypes.PDFPAGE_SHADING)))
            {
              FS_RECTF boundingBox2 = pdfPageObject.BoundingBox;
              if ((double) boundingBox2.top <= (double) fsRectf3.bottom && (double) boundingBox2.bottom >= (double) fsRectf4.top)
                return false;
            }
          }
        }
        if (!ParagraphRecognizer.TreatAsEquals(fsRectf1.left, fsRectf2.left) && !ParagraphRecognizer.TreatAsEquals(fsRectf1.right, fsRectf2.right) && !ParagraphRecognizer.TreatAsEquals(fsRectf1.left + fsRectf1.Width / 2f, fsRectf2.left + fsRectf2.Width / 2f))
        {
          FS_RECTF rect1 = fsRectf1;
          FS_RECTF rect2 = fsRectf2;
          rect1.Inflate(new FS_RECTF(20f, 20f, 20f, 20f));
          rect2.Inflate(new FS_RECTF(20f, 20f, 20f, 20f));
          rect1.Inflate(rect2);
          if (rect1.IsEmpty())
          {
            FS_RECTF fsRectf5 = fsRectf1;
            FS_RECTF fsRectf6 = fsRectf2;
            if ((double) fsRectf5.left > (double) fsRectf6.left)
            {
              fsRectf5 = fsRectf2;
              fsRectf6 = fsRectf1;
            }
            if ((double) Math.Abs(fsRectf5.right - fsRectf6.left) > 30.0)
            {
              para.Ended = true;
              return false;
            }
          }
        }
      }
      if (ParagraphRecognizer.TreatAsEquals(fsRectf1.left, fsRectf2.left))
      {
        if (ParagraphRecognizer.TreatAsEquals(fsRectf1.right, fsRectf2.right))
        {
          int count = para.Lines.Count;
          para.AddLine(line1);
          if (count == 1)
          {
            para.Align = AlignType.AlignAdjust;
            textLine.Align = AlignType.AlignAdjust;
          }
          line1.Align = AlignType.AlignAdjust;
          this.AdjustReturnFlag(line1, para);
          if (flag && !line1.IsBulletHeader)
            line1.IsFollowingUpon = true;
          return true;
        }
        switch (align)
        {
          case AlignType.AlignNone:
            this.AdjustReturnFlag(line1, para);
            int count1 = para.Lines.Count;
            para.AddLine(line1);
            line1.Align = AlignType.AlignLeft;
            if (count1 == 1)
            {
              para.Align = AlignType.AlignLeft;
              textLine.Align = AlignType.AlignLeft;
            }
            if (flag && !line1.IsBulletHeader)
            {
              line1.IsFollowingUpon = true;
              textLine.ReturnFlag = false;
            }
            return true;
          case AlignType.AlignLeft:
            this.AdjustReturnFlag(line1, para);
            float num1 = boundingBox1.bottom - fsRectf1.top;
            line1.Align = AlignType.AlignLeft;
            para.AddLine(line1);
            if (flag && !line1.IsBulletHeader)
            {
              line1.IsFollowingUpon = true;
              textLine.ReturnFlag = false;
            }
            PdfTextObject textObj1 = line1.TextObjects.LastOrDefault<PdfTextObject>();
            if (textObj1 != null && textObj1.IsEndOfPunctuation())
            {
              float num2 = 0.0f;
              if (lineIndex + 1 < lines.Count)
              {
                TextLine line2 = lines[lineIndex + 1];
                if (!para.IsVertWriteMode)
                {
                  FS_RECTF boundingBox3 = line2.GetBoundingBox(false);
                  num2 = fsRectf1.bottom - boundingBox3.top;
                }
                PdfTextObject textObj2 = line1.TextObjects.FirstOrDefault<PdfTextObject>();
                PdfTextObject textObj2_1 = line2.TextObjects.FirstOrDefault<PdfTextObject>();
                if (textObj2 != null && !textObj2.IsSameFontProperties(textObj2_1) && (double) num2 >= (double) num1 + 5.0)
                {
                  line1.ReturnFlag = true;
                  para.Ended = true;
                }
              }
            }
            return true;
          case AlignType.AlignAdjust:
            this.AdjustReturnFlag(line1, para);
            para.AddLine(line1);
            if ((double) fsRectf1.Width > (double) fsRectf2.Width && (double) fsRectf2.Width - (double) fsRectf1.Width > 12.0)
            {
              para.Ended = true;
              line1.ReturnFlag = true;
            }
            textLine.Align = AlignType.AlignAdjust;
            line1.Align = AlignType.AlignAdjust;
            if (flag && !line1.IsBulletHeader)
            {
              line1.IsFollowingUpon = true;
              textLine.ReturnFlag = false;
            }
            return true;
          default:
            return false;
        }
      }
      else
      {
        if (ParagraphRecognizer.TreatAsEquals(fsRectf1.right, fsRectf2.right))
        {
          if (align == AlignType.AlignAdjust || align == AlignType.AlignLeft || align == AlignType.AlignCenter)
          {
            if (textLine.TextObjects.Count >= 2)
            {
              string text = textLine.TextObjects[0].GetText();
              if (text.Length == 1 && text[0] == '•' && ParagraphRecognizer.TreatAsEquals(textLine.TextObjects[1].GetBox().left, fsRectf1.left))
              {
                this.AdjustReturnFlag(line1, para);
                para.AddLine(line1);
                line1.Align = AlignType.AlignAdjust;
                if (para.Lines.Count == 1)
                {
                  para.Align = AlignType.AlignAdjust;
                  textLine.Align = AlignType.AlignAdjust;
                }
                line1.IsFollowingUpon = true;
                return true;
              }
            }
            return false;
          }
          if (align == AlignType.AlignNone)
          {
            if (textLine.TextObjects.Count >= 2)
            {
              string text = textLine.TextObjects[0].GetText();
              if (text.Length == 1 && text[0] == '\u0080' && ParagraphRecognizer.TreatAsEquals(textLine.TextObjects[1].GetBox().left, fsRectf1.left))
              {
                this.AdjustReturnFlag(line1, para);
                para.AddLine(line1);
                line1.Align = AlignType.AlignAdjust;
                if (para.Lines.Count == 1)
                {
                  para.Align = AlignType.AlignAdjust;
                  textLine.Align = AlignType.AlignAdjust;
                }
                line1.IsFollowingUpon = true;
                return true;
              }
            }
            if (lineIndex + 1 < lines.Count)
            {
              TextLine line3 = lines[lineIndex + 1];
              if (!para.IsVertWriteMode)
              {
                FS_RECTF boundingBox4 = line3.GetBoundingBox(false);
                if (ParagraphRecognizer.TreatAsEquals(boundingBox4.left, fsRectf1.left) && (double) (fsRectf2.bottom - fsRectf1.top) >= (double) (fsRectf1.bottom - boundingBox4.top))
                  return false;
              }
            }
            if (para.Lines.Count != 1)
              return false;
            para.AddLine(line1);
            para.Align = AlignType.AlignRight;
            return true;
          }
          this.AdjustReturnFlag(line1, para);
          para.AddLine(line1);
          return true;
        }
        if (ParagraphRecognizer.TreatAsEquals(fsRectf1.left + fsRectf1.Width / 2f, fsRectf2.left + fsRectf2.Width / 2f))
        {
          if (lineIndex + 1 < lines.Count)
          {
            TextLine line4 = lines[lineIndex + 1];
            if (!para.IsVertWriteMode)
            {
              FS_RECTF boundingBox5 = line4.GetBoundingBox(false);
              if (ParagraphRecognizer.TreatAsEquals(boundingBox5.left, fsRectf1.left) && (double) (fsRectf2.bottom - fsRectf1.top) >= (double) (fsRectf1.bottom - boundingBox5.top))
                return false;
            }
          }
          if (para.Lines.Count == 1)
          {
            para.AddLine(line1);
            para.Align = AlignType.AlignCenter;
            return true;
          }
          if (align == AlignType.AlignCenter && lineIndex + 1 < lines.Count)
          {
            TextLine line5 = lines[lineIndex + 1];
            if (!para.IsVertWriteMode)
            {
              FS_RECTF boundingBox6 = line5.GetBoundingBox(false);
              if (ParagraphRecognizer.TreatAsEquals(fsRectf1.left, boundingBox6.left) || ParagraphRecognizer.TreatAsEquals(fsRectf1.right, boundingBox6.left))
                return false;
              this.AdjustReturnFlag(line1, para);
              para.AddLine(line1);
              if (flag && !line1.IsBulletHeader)
              {
                line1.IsFollowingUpon = true;
                textLine.ReturnFlag = false;
              }
              return true;
            }
          }
          return false;
        }
        if (!para.IsVertWriteMode)
        {
          if (align == AlignType.AlignLeft || align == AlignType.AlignAdjust || align == AlignType.AlignNone)
          {
            if (ParagraphRecognizer.TreatAsEquals(fsRectf1.left, boundingBox1.left))
            {
              this.AdjustReturnFlag(line1, para);
              para.AddLine(line1);
              line1.Align = AlignType.AlignLeft;
              return true;
            }
            if (line1.TextObjects.Count > 0)
            {
              float[] charPos1 = line1.TextObjects[0].GetCharPos(0);
              if (textLine.TextObjects.Count > 0)
              {
                foreach (PdfTextObject textObject in (IEnumerable<PdfTextObject>) textLine.TextObjects)
                {
                  for (int index = 0; index < textObject.CharsCount; ++index)
                  {
                    float kerning;
                    textObject.GetCharInfo(index, out int _, out kerning);
                    if ((double) kerning <= 900.0)
                    {
                      float[] charPos2 = textObject.GetCharPos(index);
                      if (ParagraphRecognizer.TreatAsEquals(charPos1[0], charPos2[0]))
                      {
                        if (!line1.TextObjects[0].IsSameFontProperties(textObject))
                          return false;
                        this.AdjustReturnFlag(line1, para);
                        para.AddLine(line1);
                        line1.Align = AlignType.AlignLeft;
                        if (para.Lines.Count == 1)
                          para.Align = AlignType.AlignLeft;
                        line1.IsFollowingUpon = true;
                        return true;
                      }
                    }
                  }
                }
              }
            }
            if (textLine.TextObjects.Count >= 2)
            {
              string text = textLine.TextObjects[0].GetText();
              if (text.Length == 1 && text[0] == '\u0080' && ParagraphRecognizer.TreatAsEquals(textLine.TextObjects[1].GetBox().left, fsRectf1.left))
              {
                para.AddLine(line1);
                line1.Align = AlignType.AlignLeft;
                if (para.Lines.Count == 1)
                  para.Align = AlignType.AlignLeft;
                line1.IsFollowingUpon = true;
              }
            }
          }
          if (align == AlignType.AlignNone && (double) Math.Abs(fsRectf1.top - fsRectf2.bottom) < 20.0 && (double) fsRectf1.left > (double) fsRectf2.left)
          {
            para.AddLine(line1);
            return true;
          }
        }
      }
    }
    return false;
  }

  private void AdjustReturnFlag(TextLine line, Paragraph para)
  {
    TextLine line1 = para.Lines.LastOrDefault<TextLine>();
    if (para.IsRotate || para.IsVertWriteMode)
      return;
    if (line1 != null)
    {
      if (line1.IsBulletHeader && line.IsBulletHeader)
      {
        line1.ReturnFlag = true;
        line1.IsFollowingUpon = false;
        return;
      }
      if (line.IsBulletHeader && line1.IsFollowingUpon)
      {
        line1.ReturnFlag = true;
        line.IsFollowingUpon = false;
      }
    }
    if (ParagraphRecognizer.IsParaEndLine(line))
    {
      line.ReturnFlag = true;
    }
    else
    {
      if (line1.TextObjects.Count == 0 || line.TextObjects.Count == 0)
        return;
      PdfTextObject textObject1 = line1.TextObjects[line1.TextObjects.Count - 1];
      PdfFont font1 = textObject1.Font;
      PdfTextObject textObject2 = line.TextObjects[0];
      PdfFont font2 = textObject2.Font;
      bool flag1 = font1.IsBold() && font2.IsBold() || !font1.IsBold() && !font2.IsBold();
      bool flag2 = font1.IsItalic() && font2.IsItalic() || !font1.IsItalic() && !font2.IsItalic();
      bool flag3 = (double) textObject1.FontSize == (double) textObject2.FontSize;
      if (!flag1 || !flag2 || !flag3)
      {
        line1.ReturnFlag = true;
      }
      else
      {
        if (ParagraphRecognizer.IsParaEndLine(line1))
          return;
        int upperCharIndex;
        if (ParagraphRecognizer.HeaderCharIsUpper(line, out upperCharIndex) && upperCharIndex == 0)
          line1.ReturnFlag = true;
        if (para.Lines.Count > 1)
        {
          TextLine line2 = para.Lines[para.Lines.Count - 2];
          if (line2.TextObjects.Count > 0)
          {
            PdfTextObject textObject3 = line2.TextObjects[0];
            PdfTextObject textObject4 = line2.TextObjects[0];
            float y1 = textObject3.Location.Y;
            FS_POINTF location = textObject4.Location;
            float y2 = location.Y;
            location = textObject2.Location;
            float y3 = location.Y;
            float num1 = y1 - y2;
            float num2 = y2 - y3;
            if ((double) num1 > 0.0 && (double) num2 > 0.0 && (double) Math.Abs(num1 - num2) > 0.25)
            {
              if ((double) num2 < (double) num1)
                line2.ReturnFlag = true;
              else if ((double) num2 > (double) num1)
                line1.ReturnFlag = true;
            }
          }
        }
      }
    }
  }

  internal int GetCurParaID() => ++this.curParaId;

  private static bool IsParaEndLine(TextLine line)
  {
    if (line == null || line.CharCount == 0)
      return false;
    for (int index1 = line.TextObjects.Count - 1; index1 >= 0; --index1)
    {
      PdfTextObject textObject = line.TextObjects[index1];
      bool flag = true;
      for (int index2 = textObject.CharsCount - 1; index2 >= 0; --index2)
      {
        char unicodeChar = textObject.GetUnicodeChar(index2);
        if (unicodeChar != ' ')
        {
          flag = false;
          if (CharHelper.CharIsLineEndPunctuation(unicodeChar))
            return true;
          break;
        }
      }
      if (!flag)
        break;
    }
    return false;
  }

  private static bool HeaderCharIsUpper(TextLine line, out int upperCharIndex)
  {
    upperCharIndex = -1;
    if (line == null || line.CharCount == 0)
      return false;
    PdfTextObject pdfTextObject = line.TextObjects.FirstOrDefault<PdfTextObject>();
    if (pdfTextObject.CharsCount == 0)
      return false;
    char ch1 = char.MinValue;
    char ch2;
    if (pdfTextObject.Font != null)
    {
      int charCode1;
      float num1;
      float num2;
      pdfTextObject.GetCharInfo(0, out charCode1, out num1, out num2);
      ch2 = pdfTextObject.Font.ToUnicode(charCode1);
      if (pdfTextObject.CharsCount > 1)
      {
        int charCode2;
        pdfTextObject.GetCharInfo(1, out charCode2, out num2, out num1);
        ch1 = pdfTextObject.Font.ToUnicode(charCode2);
      }
    }
    else
    {
      int charCode;
      float num3;
      float num4;
      pdfTextObject.GetCharInfo(0, out charCode, out num3, out num4);
      ch2 = (char) charCode;
      if (pdfTextObject.CharsCount > 1)
      {
        pdfTextObject.GetCharInfo(1, out int _, out num4, out num3);
        ch1 = (char) charCode;
      }
    }
    if (ch2 < 'A' || ch2 > 'Z')
      return false;
    int num = ch1 < 'A' ? 0 : (ch1 <= 'Z' ? 1 : 0);
    upperCharIndex = num == 0 ? 0 : 1;
    return true;
  }

  private static bool TreatAsEquals(float left, float right)
  {
    return (double) Math.Abs(left - right) < 4.0;
  }

  internal void InsertParagraph(int index, Paragraph paragraph)
  {
    this.paragraphs.Insert(index, paragraph);
  }

  internal void RemoveParagraph(int index)
  {
    this.paragraphs[index].RemoveAllLines();
    this.paragraphs.RemoveAt(index);
  }
}
