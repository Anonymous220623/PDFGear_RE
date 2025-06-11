// Decompiled with JetBrains decompiler
// Type: PDFKit.Contents.TextLine
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using PDFKit.Contents.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

#nullable disable
namespace PDFKit.Contents;

public class TextLine
{
  private List<PdfTextObject> textObjects;
  private FS_RECTF? boundingBox;
  private FS_RECTF? boundingBoxFitWithCarets;
  private FS_RECTF? boundingBoxWithoutBellet;
  private bool? isBulletHeader;
  private int charCount = -1;
  private readonly PdfPage page;
  private object locker = new object();

  public TextLine(PdfPage page)
  {
    this.textObjects = new List<PdfTextObject>();
    this.page = page;
  }

  public string LineText
  {
    get
    {
      return this.TextObjects.Aggregate<PdfTextObject, StringBuilder>(new StringBuilder(), (Func<StringBuilder, PdfTextObject, StringBuilder>) ((s, c) => s.Append(c.GetText()))).ToString();
    }
  }

  public System.Collections.Generic.IReadOnlyList<PdfTextObject> TextObjects
  {
    get => (System.Collections.Generic.IReadOnlyList<PdfTextObject>) this.textObjects;
  }

  public int Id { get; internal set; }

  public bool IsRotate { get; internal set; }

  public int CharCount
  {
    get
    {
      lock (this.locker)
      {
        if (this.TextObjects.Count == 0)
          return 0;
        if (this.charCount == -1)
          this.charCount = this.TextObjects.Sum<PdfTextObject>((Func<PdfTextObject, int>) (c => c.CharsCount));
        return this.charCount;
      }
    }
  }

  public bool IsBulletHeader
  {
    get
    {
      lock (this.locker)
      {
        if (this.TextObjects.Count == 0)
          return false;
        if (!this.isBulletHeader.HasValue)
        {
          PdfTextObject textObject = this.TextObjects[0];
          if (textObject.CharsCount > 0)
          {
            if (CharHelper.IsBullet(textObject.GetUnicodeChar(0)))
            {
              this.isBulletHeader = new bool?(true);
            }
            else
            {
              float num1 = 0.0f;
              int num2 = -1;
              for (int index = 0; index < textObject.CharsCount; ++index)
              {
                float kerning;
                textObject.GetCharInfo(index, out int _, out kerning);
                if ((double) num1 < (double) kerning)
                {
                  num1 = kerning;
                  num2 = index;
                }
              }
              if ((double) num1 < -900.0 && num2 >= 0 && num2 < textObject.CharsCount && num2 < 6)
                this.isBulletHeader = new bool?(true);
            }
          }
          if (!this.isBulletHeader.HasValue)
            this.isBulletHeader = new bool?(false);
        }
        return this.isBulletHeader.Value;
      }
    }
  }

  public bool IsFollowingUpon { get; internal set; }

  public FS_RECTF BoundingBoxWithoutBellet
  {
    get
    {
      lock (this.locker)
      {
        if (!this.boundingBoxWithoutBellet.HasValue)
        {
          FS_RECTF boundingBox = this.GetBoundingBox(false);
          if (this.TextObjects.Count > 0)
          {
            FS_RECTF fsRectf = boundingBox;
            PdfTextObject textObject = this.TextObjects[0];
            int charsCount = textObject.CharsCount;
            if (charsCount > 0)
            {
              char unicodeChar = textObject.GetUnicodeChar(0);
              if (unicodeChar != char.MaxValue && CharHelper.IsBullet(unicodeChar))
              {
                if (charsCount == 1)
                {
                  PdfTextObject textObj = (PdfTextObject) null;
                  for (int index = 1; index < charsCount; ++index)
                  {
                    textObj = this.TextObjects[index];
                    if (textObj != null && !string.IsNullOrEmpty(textObj.GetText()))
                      break;
                  }
                  if (textObj != null)
                  {
                    FS_RECTF box = textObj.GetBox();
                    fsRectf.left = box.left;
                  }
                }
                else
                {
                  float[] charPos = textObject.GetCharPos(1);
                  fsRectf.left = charPos[0];
                }
                this.boundingBoxWithoutBellet = new FS_RECTF?(fsRectf);
              }
              else
              {
                float num1 = 0.0f;
                int num2 = -1;
                for (int index = 0; index < charsCount; ++index)
                {
                  int charCode = 0;
                  float kerning = 0.0f;
                  textObject.GetCharInfo(index, out charCode, out kerning);
                  if ((double) kerning < (double) num1)
                  {
                    num1 = kerning;
                    num2 = index;
                  }
                }
                if ((double) num1 < -900.0 && num2 >= 0 && num2 < charsCount - 1 && num2 < 6)
                {
                  float[] charPos = textObject.GetCharPos(num2 + 1);
                  fsRectf.left = charPos[0];
                  this.boundingBoxWithoutBellet = new FS_RECTF?(fsRectf);
                }
              }
            }
          }
          if (!this.boundingBoxWithoutBellet.HasValue)
            this.boundingBoxWithoutBellet = new FS_RECTF?(boundingBox);
        }
        return this.boundingBoxWithoutBellet.Value;
      }
    }
  }

  public bool IsVertWriteMode { get; internal set; }

  public FS_RECTF GetBoundingBox(bool fitWithCarets)
  {
    lock (this.locker)
    {
      if (this.textObjects.Count == 0 && !fitWithCarets && !this.boundingBox.HasValue)
        return new FS_RECTF();
      return !fitWithCarets ? GetBoundingBoxCore() : GetBoundingBoxFitWithCaretsCore();
    }

    FS_RECTF GetBoundingBoxCore()
    {
      if (!this.boundingBox.HasValue)
      {
        if (!this.IsRotate)
        {
          FS_RECTF? nullable = new FS_RECTF?();
          foreach (PdfTextObject textObject in this.textObjects)
          {
            FS_RECTF box = textObject.GetBox();
            if (!box.IsEmpty() && (double) box.left != 0.0 && (double) box.top != 0.0)
              nullable = !nullable.HasValue ? new FS_RECTF?(box) : new FS_RECTF?(nullable.Value.Union(box));
            else if (!nullable.HasValue)
              nullable = new FS_RECTF?(box);
          }
          this.boundingBox = new FS_RECTF?(nullable.GetValueOrDefault());
        }
        else
          this.boundingBox = new FS_RECTF?(new FS_RECTF());
      }
      return this.boundingBox.Value;
    }

    FS_RECTF GetBoundingBoxFitWithCaretsCore()
    {
      if (!this.boundingBoxFitWithCarets.HasValue)
      {
        FS_RECTF boundingBoxCore = GetBoundingBoxCore();
        if (boundingBoxCore.IsEmpty() && (double) boundingBoxCore.left == 0.0 && (double) boundingBoxCore.top == 0.0)
          return boundingBoxCore;
        if (this.textObjects.Count == 0 && !this.IsRotate)
        {
          if (this.IsVertWriteMode)
            boundingBoxCore.bottom = boundingBoxCore.top;
          else
            boundingBoxCore.right = boundingBoxCore.left;
          this.boundingBoxFitWithCarets = new FS_RECTF?(boundingBoxCore);
          return boundingBoxCore;
        }
        float[] charPos = this.textObjects[0].GetCharPos(0);
        float num1 = boundingBoxCore.left - charPos[0];
        if ((double) num1 > 0.0)
          boundingBoxCore.left -= num1;
        PdfTextObject textObject = this.textObjects[this.textObjects.Count - 1];
        float num2 = textObject.GetCharPos(textObject.CharsCount - 1)[2] - boundingBoxCore.right;
        if ((double) num2 > 0.0)
          boundingBoxCore.right += num2;
        this.boundingBoxFitWithCarets = new FS_RECTF?(boundingBoxCore);
      }
      return this.boundingBoxFitWithCarets.Value;
    }
  }

  internal void SetBox(FS_RECTF boundingBox)
  {
    lock (this.locker)
    {
      this.boundingBox = new FS_RECTF?(boundingBox);
      this.GetBoundingBox(true);
    }
  }

  public AlignType Align { get; internal set; }

  public bool ReturnFlag { get; internal set; }

  public void InvalidateBoundingBox()
  {
    lock (this.locker)
    {
      this.boundingBox = new FS_RECTF?();
      this.boundingBoxFitWithCarets = new FS_RECTF?();
      this.boundingBoxWithoutBellet = new FS_RECTF?();
      this.charCount = -1;
    }
  }

  public void Offset(float dx, float dy)
  {
    lock (this.locker)
    {
      foreach (PdfTextObject textObject in this.textObjects)
        textObject.Offset(dx, dy);
      if (this.boundingBox.HasValue)
      {
        FS_RECTF fsRectf = this.boundingBox.Value;
        fsRectf.left += dx;
        fsRectf.right += dx;
        fsRectf.top += dy;
        fsRectf.bottom += dy;
        this.boundingBox = new FS_RECTF?(fsRectf);
      }
      if (!this.boundingBoxFitWithCarets.HasValue)
        return;
      FS_RECTF fsRectf1 = this.boundingBoxFitWithCarets.Value;
      fsRectf1.left += dx;
      fsRectf1.right += dx;
      fsRectf1.top += dy;
      fsRectf1.bottom += dy;
      this.boundingBoxFitWithCarets = new FS_RECTF?(fsRectf1);
    }
  }

  public bool AddText(PdfTextObject textObj, bool vertWrite)
  {
    lock (this.locker)
    {
      if (this.textObjects.Count == 0)
      {
        this.textObjects.Add(textObj);
        this.IsRotate = textObj.IsRotate();
        this.InvalidateBoundingBox();
        return true;
      }
      if (!this.CanContainText(textObj, vertWrite))
        return false;
      this.textObjects.Add(textObj);
      this.InvalidateBoundingBox();
      return true;
    }
  }

  public void InsertText(int index, PdfTextObject textObj)
  {
    lock (this.locker)
    {
      this.textObjects.Insert(index, textObj);
      this.InvalidateBoundingBox();
    }
  }

  internal PdfTextObject InsertText(
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
    {
      PdfFont font1 = font;
      float dx = 0.0f;
      float dy = 0.0f;
      int charsCount1 = textObj.CharsCount;
      int length = text.Length;
      BoldItalicFlags boldItalicFlags = BoldItalicFlags.None;
      if (font.IsBold() || textObj.IsBold())
        boldItalicFlags |= BoldItalicFlags.Bold;
      if (font.IsItalic() || textObj.IsItalic())
        boldItalicFlags |= BoldItalicFlags.Italic;
      bool flag1 = underlineStrikeout >= 2;
      bool flag2 = underlineStrikeout == 1 || underlineStrikeout == 3;
      PdfTextObject pdfTextObject1;
      PdfTextObject pdfTextObject2;
      if (boldItalicFlags == boldItalic && textObj.IsSameFontProperties(font, fontSize, textColor, strokeColor, BoldItalicFlags.None))
      {
        FS_RECTF box1 = textObj.GetBox();
        float width1 = box1.Width;
        float height1 = box1.Height;
        if (!this.IsRotate)
          ;
        int[] sourceArray1 = new int[length];
        for (int index = 0; index < length; ++index)
          sourceArray1[index] = font.ToCharCode(text[index]);
        int[] sourceArray2 = new int[charsCount1];
        float[] sourceArray3 = new float[charsCount1];
        for (int index = 0; index < charsCount1; ++index)
        {
          int charCode;
          float kerning;
          textObj.GetCharInfo(index, out charCode, out kerning);
          sourceArray2[index] = charCode;
          sourceArray3[index] = kerning;
        }
        int countChars = charsCount1 + length;
        int[] numArray1 = new int[countChars];
        float[] numArray2 = new float[countChars];
        if (insertAt == 0)
        {
          Array.Copy((Array) sourceArray1, (Array) numArray1, length);
          Array.Copy((Array) sourceArray2, 0, (Array) numArray1, length, charsCount1);
          Array.Copy((Array) sourceArray3, 0, (Array) numArray2, length, charsCount1);
        }
        else if (insertAt >= charsCount1)
        {
          Array.Copy((Array) sourceArray2, (Array) numArray1, charsCount1);
          Array.Copy((Array) sourceArray3, (Array) numArray2, charsCount1);
          Array.Copy((Array) sourceArray1, 0, (Array) numArray1, charsCount1, length);
        }
        else
        {
          Array.Copy((Array) sourceArray2, (Array) numArray1, insertAt);
          Array.Copy((Array) sourceArray3, (Array) numArray2, insertAt);
          Array.Copy((Array) sourceArray1, 0, (Array) numArray1, insertAt, length);
          Array.Copy((Array) sourceArray2, insertAt, (Array) numArray1, insertAt + length, charsCount1 - insertAt);
          Array.Copy((Array) sourceArray3, insertAt, (Array) numArray2, insertAt + length, charsCount1 - insertAt);
        }
        Pdfium.FPDFTextObj_SetEmpty(textObj.Handle);
        Pdfium.FPDFTextObj_SetText(textObj.Handle, countChars, numArray1, numArray2);
        textObj.RecalcPositionData();
        FS_RECTF box2 = textObj.GetBox();
        float width2 = box2.Width;
        float height2 = box2.Height;
        dx = width2 - width1;
        dy = height2 - height1;
        pdfTextObject1 = textObj;
        pdfTextObject2 = textObj;
      }
      else
      {
        bool bold = (boldItalic & BoldItalicFlags.Bold) != 0;
        bool italic = (boldItalic & BoldItalicFlags.Italic) != 0;
        if (bold != font.IsBold() || italic != font.IsItalic())
        {
          PdfFont font2 = TextObjectExtensions.LoadBoldItalicSubstFont(this.page.Document, font, textObj.GetCharset(cultureInfo), bold, italic);
          if (font2 != null && (bold && font2.IsBold() || !bold && !font2.IsBold()) && (italic && font2.IsItalic() || !italic && !font2.IsItalic()))
          {
            font = font2;
            boldItalic = BoldItalicFlags.None;
          }
        }
        if (bold && font1.IsBold())
          boldItalic &= ~BoldItalicFlags.Bold;
        if (italic && font1.IsItalic())
          boldItalic &= ~BoldItalicFlags.Italic;
        this.textObjects.IndexOf(textObj);
        PdfTextObject textObject = TextLine.CreateTextObject(text, font, fontSize, textColor, strokeColor, boldItalic, underlineStrikeout);
        FS_POINTF ascentPoint;
        FS_POINTF descentPoint;
        textObj.GetCharPos(insertAt, out ascentPoint, out descentPoint);
        textObject.Location = new FS_POINTF(descentPoint.X, textObj.Location.Y);
        int index = textObj.Container.IndexOf((PdfPageObject) textObj);
        if (insertAt >= charsCount1)
          ++index;
        else if (insertAt != 0)
        {
          this.SplitAt(textObj, insertAt, false).RecalcPositionData();
          ++index;
        }
        textObj.Container.Insert(index, (PdfPageObject) textObject);
        this.textObjects.Insert(this.textObjects.IndexOf(textObj) + (insertAt == 0 ? 0 : 1), textObject);
        if (!flag2)
          ;
        if (!flag1)
          ;
        if (script > ScriptEnum.Normal)
          TextLine.UpdateScripts(this, textObj, insertAt, script, textObject);
        textObject.RecalcPositionData();
        FS_RECTF box3 = textObject.GetBox();
        float num = 0.0f;
        if (!this.IsRotate)
        {
          if (!this.IsVertWriteMode)
          {
            FS_RECTF box4 = textObj.GetBox();
            if (insertAt == 0)
            {
              num = box4.left - ascentPoint.X;
            }
            else
            {
              int charsCount2 = textObj.CharsCount;
              if (!textObj.CharIsBlankSpace(charsCount2 - 1))
                num = box3.left - box4.right;
            }
          }
          dx = box3.Width + num;
          dy = box3.Height;
        }
        pdfTextObject1 = textObject;
        pdfTextObject2 = textObject;
      }
      bool flag3 = true;
      PdfTextObject textObj1 = (PdfTextObject) null;
      for (int index = this.textObjects.Count - 1; index >= 0; --index)
      {
        PdfTextObject textObject = this.textObjects[index];
        if (textObject == pdfTextObject1)
        {
          if (textObj1 != null && !this.IsRotate)
          {
            FS_RECTF box5 = textObj1.GetBox();
            FS_RECTF box6 = textObject.GetBox();
            if (this.IsVertWriteMode)
            {
              if ((double) box5.top <= (double) box6.bottom)
                flag3 = false;
            }
            else if ((double) box5.left >= (double) box6.right)
              flag3 = false;
            break;
          }
          break;
        }
        textObj1 = textObject;
      }
      if (flag3)
      {
        for (int index = this.textObjects.Count - 1; index >= 0; --index)
        {
          PdfTextObject textObject = this.textObjects[index];
          if (textObject != pdfTextObject1)
          {
            if (this.IsVertWriteMode)
              textObject.Offset(0.0f, dy);
            else
              textObject.Offset(dx, 0.0f);
          }
          else
            break;
        }
        this.InvalidateBoundingBox();
      }
      return pdfTextObject2;
    }
  }

  internal PdfTextObject InsertTextToEmptyLine(
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
    {
      PdfTextObject textObject = TextLine.CreateTextObject(text, font, fontSize, textColor, strokeColor, boldItalic, underlineStrikeout);
      FS_RECTF boundingBox = this.GetBoundingBox(false);
      textObject.Location = new FS_POINTF(boundingBox.left, boundingBox.bottom);
      textObject.RecalcPositionData();
      container.Insert(insertAt, (PdfPageObject) textObject);
      this.textObjects.Insert(0, textObject);
      this.InvalidateBoundingBox();
      return textObject;
    }
  }

  internal PdfTextObject InsertTextAtBlank(
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
    {
      if (this.textObjects.Count == 0)
        return (PdfTextObject) null;
      int index1 = -1;
      int index2 = -1;
      PdfPageObjectsCollection objectsCollection = (PdfPageObjectsCollection) null;
      if (!this.IsRotate)
      {
        for (int index3 = this.textObjects.Count - 1; index3 >= 0; --index3)
        {
          PdfTextObject textObject = this.textObjects[index3];
          FS_RECTF box = textObject.GetBox();
          if (!this.IsVertWriteMode)
          {
            if ((double) box.left <= (double) x)
            {
              index1 = index3;
              objectsCollection = textObject.Container;
              index2 = objectsCollection.IndexOf((PdfPageObject) textObject);
              break;
            }
          }
          else if ((double) box.top >= (double) y)
          {
            index1 = index3;
            objectsCollection = textObject.Container;
            index2 = objectsCollection.IndexOf((PdfPageObject) textObject);
            break;
          }
        }
      }
      if (index1 == -1)
      {
        index1 = this.textObjects.Count;
        objectsCollection = this.textObjects[this.textObjects.Count - 1].Container;
        index2 = objectsCollection.Count;
      }
      PdfTextObject textObject1 = TextLine.CreateTextObject(text, font, fontSize, textColor, strokeColor, boldItalic, underlineStrikeout);
      objectsCollection.Insert(index2, (PdfPageObject) textObject1);
      this.textObjects.Insert(index1, textObject1);
      textObject1.Location = new FS_POINTF(x, y);
      textObject1.RecalcPositionData();
      if (!this.IsRotate)
      {
        FS_RECTF box = textObject1.GetBox();
        float width = box.Width;
        float height = box.Height;
        for (int index4 = index1 + 1; index4 < this.textObjects.Count; ++index4)
        {
          PdfTextObject textObject2 = this.textObjects[index4];
          if (!this.IsVertWriteMode)
            textObject2.Offset(width, 0.0f);
          else
            textObject2.Offset(0.0f, height);
        }
      }
      this.InvalidateBoundingBox();
      return textObject1;
    }
  }

  public void RemoveText(PdfTextObject textObj, int startCharIndex, int endCharIndex)
  {
    lock (this.locker)
    {
      FS_RECTF box = textObj.GetBox();
      bool totalAndRemoved;
      textObj.DeleteText(startCharIndex, endCharIndex, out totalAndRemoved);
      float num = !totalAndRemoved ? textObj.GetBox().Width - box.Width : -box.Width;
      for (int index = this.textObjects.Count - 1; index >= 0; --index)
      {
        PdfTextObject textObject = this.textObjects[index];
        if (textObject == textObj)
        {
          if (totalAndRemoved)
          {
            this.textObjects.RemoveAt(index);
            break;
          }
          break;
        }
        if (this.IsVertWriteMode)
          textObject.Offset(0.0f, num);
        else
          textObject.Offset(num, 0.0f);
      }
      this.InvalidateBoundingBox();
    }
  }

  public void RemoveText(
    PdfTextObject textStart,
    int startCharIndex,
    PdfTextObject textEnd,
    int endCharIndex)
  {
    lock (this.locker)
    {
      if (textStart == textEnd)
      {
        this.RemoveText(textStart, startCharIndex, endCharIndex);
      }
      else
      {
        bool flag = false;
        PdfTextObject textObj1 = (PdfTextObject) null;
        PdfTextObject textObj2 = (PdfTextObject) null;
        for (int index1 = this.textObjects.Count - 1; index1 >= 0; --index1)
        {
          PdfTextObject textObject = this.textObjects[index1];
          if (!flag)
            flag = textObject == textEnd;
          if (flag)
          {
            if (textObject == textEnd)
            {
              if (endCharIndex == textObject.CharsCount - 1)
              {
                textObject.Container.Remove((PdfPageObject) textObject);
                this.textObjects.Remove(textObject);
                if (index1 < this.textObjects.Count)
                  textObj2 = this.textObjects[index1];
              }
              else
              {
                PdfTextObject newTextObj;
                TextObjectExtensions.SplitTextObj(textObject, endCharIndex + 1, false, out newTextObj);
                textObject.Container.Insert(textObject.Container.IndexOf((PdfPageObject) textObject) + 1, (PdfPageObject) newTextObj);
                newTextObj.RecalcPositionData();
                this.textObjects.Insert(index1 + 1, newTextObj);
                textObject.Container.Remove((PdfPageObject) textObject);
                this.textObjects.RemoveAt(index1);
                textObj2 = newTextObj;
              }
            }
            else
            {
              if (textObject == textStart)
              {
                if (startCharIndex == 0)
                {
                  textObject.Container.Remove((PdfPageObject) textObject);
                  this.textObjects.Remove(textObject);
                  int index2 = index1 - 1;
                  if (index2 >= 0 && index2 < this.textObjects.Count)
                  {
                    textObj1 = this.textObjects[index2];
                    break;
                  }
                  break;
                }
                textObject.GetBox();
                TextObjectExtensions.SplitTextObj(textObject, startCharIndex, false, out PdfTextObject _);
                textObj1 = textStart;
                break;
              }
              textObject.Container.Remove((PdfPageObject) textObject);
              this.textObjects.RemoveAt(index1);
            }
          }
        }
        if (textObj1 != null && textObj2 != null)
        {
          FS_RECTF box1 = textObj1.GetBox();
          FS_RECTF box2 = textObj2.GetBox();
          if (!this.IsRotate && !this.IsVertWriteMode)
          {
            float num = box2.left - box1.right;
            for (int index3 = 0; index3 < this.textObjects.Count; ++index3)
            {
              if (this.textObjects[index3] == textObj2)
              {
                for (int index4 = index3; index4 < this.textObjects.Count; ++index4)
                  this.textObjects[index4].Offset(-num, 0.0f);
                break;
              }
            }
          }
        }
        this.InvalidateBoundingBox();
      }
    }
  }

  public void RemoveObj(int index)
  {
    lock (this.locker)
    {
      if (index < 0 || index >= this.textObjects.Count)
        return;
      this.textObjects.RemoveAt(index);
      if (this.textObjects.Count > 0)
        this.InvalidateBoundingBox();
    }
  }

  public bool IsLineHeader(PdfTextObject textObj, int charIndex)
  {
    lock (this.locker)
      return this.textObjects.Count == 0 && textObj == null || (this.textObjects.Count <= 0 || this.textObjects[0] == textObj) && charIndex == 0;
  }

  public bool IsLineTail(PdfTextObject textObj, int charIndex)
  {
    lock (this.locker)
    {
      if (this.textObjects.Count == 0)
        return false;
      PdfTextObject textObject = this.textObjects[this.textObjects.Count - 1];
      return textObject == textObj && charIndex >= textObject.CharsCount;
    }
  }

  public FS_POINTF GetTailCharPos()
  {
    lock (this.locker)
    {
      bool flag = this.CharCount == 0;
      if (!flag)
        flag = this.textObjects[this.textObjects.Count - 1].CharsCount == 0;
      if (flag)
      {
        FS_RECTF boundingBox = this.GetBoundingBox(true);
        return new FS_POINTF(boundingBox.right, boundingBox.bottom);
      }
      PdfTextObject textObject = this.textObjects[this.textObjects.Count - 1];
      return new FS_POINTF(textObject.GetBox().right, textObject.Location.Y);
    }
  }

  public bool ExtractFrom(TextLine srcLine, int startIndex, int endIndex, bool addSpaceAtTail)
  {
    lock (this.locker)
    {
      FS_POINTF tailCharPos = this.GetTailCharPos();
      float num = 0.0f;
      for (int index = startIndex; index <= endIndex; ++index)
      {
        PdfTextObject textObject1 = srcLine.TextObjects[index];
        float width = 0.0f;
        bool flag1 = false;
        bool flag2 = this.NeedSpaceAtTail();
        if (addSpaceAtTail & flag2 && this.TextObjects.Count > 0)
        {
          PdfTextObject textObject2 = this.TextObjects[this.TextObjects.Count - 1];
          if (!textObject2.CharIsBlankSpace(0))
          {
            PdfFont font = textObject2.Font;
            if (font != null)
            {
              flag1 = font.ContainsChar(' ', false);
              if (!textObject2.CharIsBlankSpace(textObject2.CharsCount - 1) & flag1)
              {
                textObject2.AppendSpace();
                Pdfium.FPDFTextObj_GetSpaceCharWidth(textObject2.Handle, out width);
                width *= (float) textObject2.MatrixFromPage2().M11;
              }
            }
          }
        }
        if (((!addSpaceAtTail ? 0 : (!flag1 ? 1 : 0)) & (flag2 ? 1 : 0)) != 0)
        {
          tailCharPos.X += 4f;
          width = 4f;
        }
        FS_RECTF box = textObject1.GetBox();
        if (startIndex == index)
          num = tailCharPos.X - box.left + width;
        float x = box.left + num;
        textObject1.Location = new FS_POINTF(x, tailCharPos.Y);
        this.textObjects.Add(textObject1);
      }
      for (int index = endIndex; index >= startIndex; --index)
        srcLine.RemoveObj(index);
      if (this.TextObjects.Count > 0)
        this.InvalidateBoundingBox();
      return true;
    }
  }

  internal System.Collections.Generic.IReadOnlyList<SplitToken> GetSymbolAndSpacePos()
  {
    lock (this.locker)
    {
      if (this.textObjects.Count == 0)
        return (System.Collections.Generic.IReadOnlyList<SplitToken>) Array.Empty<SplitToken>();
      List<SplitToken> symbolAndSpacePos = new List<SplitToken>();
      for (int index = 0; index < this.textObjects.Count; ++index)
      {
        PdfTextObject textObject1 = this.textObjects[index];
        foreach (int symbolAndSpacePo in textObject1.GetSymbolAndSpacePos())
        {
          SplitToken splitToken = new SplitToken()
          {
            ObjectIndex = index,
            SplitCharAt = symbolAndSpacePo,
            TextObject = textObject1,
            SplitNewTextObject = (PdfTextObject) null
          };
          symbolAndSpacePos.Add(splitToken);
        }
        if (index < this.textObjects.Count - 1)
        {
          PdfTextObject textObject2 = this.textObjects[index + 1];
          if (!this.IsRotate)
          {
            float[] charPos1 = textObject1.GetCharPos(textObject1.CharsCount - 1);
            float[] charPos2 = textObject2.GetCharPos(0);
            float spaceCharWidth = textObject1.GetSpaceCharWidth();
            if (!this.IsVertWriteMode && (double) charPos2[0] - (double) charPos1[2] >= (double) spaceCharWidth / 2.0)
            {
              SplitToken splitToken = new SplitToken()
              {
                ObjectIndex = index + 1,
                SplitCharAt = 0,
                TextObject = textObject2,
                SplitNewTextObject = (PdfTextObject) null
              };
              symbolAndSpacePos.Add(splitToken);
            }
          }
        }
      }
      return (System.Collections.Generic.IReadOnlyList<SplitToken>) symbolAndSpacePos;
    }
  }

  public PdfTextObject SplitAt(PdfTextObject textObj, int splitAtIndex, bool skipIfSpace)
  {
    lock (this.locker)
    {
      PdfPageObjectsCollection container = textObj.Container;
      PdfTextObject newTextObj;
      if (!TextObjectExtensions.SplitTextObj(textObj, splitAtIndex, skipIfSpace, out newTextObj))
        return (PdfTextObject) null;
      int index = container.IndexOf((PdfPageObject) textObj) + 1;
      container.Insert(index, (PdfPageObject) newTextObj);
      this.textObjects.Insert(this.textObjects.IndexOf(textObj) + 1, newTextObj);
      return newTextObj;
    }
  }

  public TextLine Clone()
  {
    lock (this.locker)
      return new TextLine(this.page)
      {
        textObjects = this.textObjects.Select<PdfTextObject, PdfTextObject>((Func<PdfTextObject, PdfTextObject>) (c => (PdfTextObject) c.Clone())).ToList<PdfTextObject>(),
        boundingBox = this.boundingBox,
        boundingBoxFitWithCarets = this.boundingBoxFitWithCarets,
        isBulletHeader = this.isBulletHeader,
        charCount = this.charCount,
        Id = this.Id,
        IsFollowingUpon = this.IsFollowingUpon,
        boundingBoxWithoutBellet = this.boundingBoxWithoutBellet,
        IsVertWriteMode = this.IsVertWriteMode,
        Align = this.Align,
        ReturnFlag = this.ReturnFlag
      };
  }

  internal bool NeedSpaceAtTail()
  {
    lock (this.locker)
    {
      for (int index = this.TextObjects.Count - 1; index >= 0; --index)
      {
        PdfTextObject textObject = this.TextObjects[index];
        int charsCount = textObject.CharsCount;
        if (charsCount > 0)
        {
          char unicodeChar = textObject.GetUnicodeChar(charsCount - 1);
          int num;
          switch (unicodeChar)
          {
            case '\t':
            case ' ':
              num = 1;
              break;
            default:
              if (!CharHelper.IsPunctuation((int) unicodeChar))
              {
                num = !CharHelper.IsCJK(unicodeChar) ? 1 : 0;
                break;
              }
              goto case '\t';
          }
          return num != 0;
        }
      }
      return false;
    }
  }

  private bool CanContainText(PdfTextObject textObj, bool vertWrite)
  {
    lock (this.locker)
    {
      if (this.TextObjects.Count == 0)
        return false;
      PdfTextObject textObject = this.TextObjects[this.TextObjects.Count - 1];
      FS_POINTF location1 = textObject.Location;
      FS_POINTF location2 = textObj.Location;
      FS_RECTF withoutTransform1 = textObject.GetBoundingBoxWithoutTransform();
      FS_RECTF withoutTransform2 = textObj.GetBoundingBoxWithoutTransform();
      float num1 = withoutTransform2.left - withoutTransform1.right;
      float num2 = 5f;
      bool flag = vertWrite ? (double) Math.Abs(location1.X - location2.X) < (double) num2 : (double) Math.Abs(location1.Y - location2.Y) < (double) num2;
      if (!flag && !this.IsRotate && !textObj.IsRotate())
        flag = (double) withoutTransform1.top <= (double) withoutTransform2.top && (double) withoutTransform1.bottom >= (double) withoutTransform2.bottom || (double) withoutTransform2.top <= (double) withoutTransform1.top && (double) withoutTransform2.bottom >= (double) withoutTransform1.bottom;
      if (flag && (!textObj.IsSameFontProperties(textObject) || vertWrite || (double) Math.Abs(num1) <= 50.0))
      {
        if (TextLine.IsFollowing(textObject, textObj))
          return true;
      }
      return false;
    }
  }

  private static bool IsFollowing(PdfTextObject lastObj, PdfTextObject textObj)
  {
    if (lastObj?.Container == null || textObj?.Container == null || lastObj.Container != textObj.Container)
      return false;
    int num = lastObj.Container.IndexOf((PdfPageObject) lastObj);
    if (textObj.Container.IndexOf((PdfPageObject) textObj) - 1 == num)
      return true;
    for (int index = num + 1; index < textObj.Container.Count; ++index)
    {
      PdfPageObject textObj1 = textObj.Container[index];
      if (textObj1.ObjectType == PageObjectTypes.PDFPAGE_TEXT && !((PdfTextObject) textObj1).IsBlankText())
        return textObj1.Handle == textObj.Handle;
    }
    return false;
  }

  private static PdfTextObject CreateTextObject(
    string text,
    PdfFont font,
    float fontSize,
    FS_COLOR textColor,
    FS_COLOR strokeColor,
    BoldItalicFlags boldItalic,
    int underlineStrikeout)
  {
    bool flag1 = (boldItalic & BoldItalicFlags.Bold) > BoldItalicFlags.None;
    bool flag2 = (boldItalic & BoldItalicFlags.Italic) > BoldItalicFlags.None;
    IntPtr handle = Pdfium.FPDFPageObj_Create(PageObjectTypes.PDFPAGE_TEXT);
    if (!(handle != IntPtr.Zero))
      return (PdfTextObject) null;
    PdfTextObject textObject = (PdfTextObject) PdfPageObject.Create(handle);
    textObject.Font = font;
    textObject.FontSize = fontSize;
    textObject.CharSpacing = 0.0f;
    textObject.WordSpacing = 0.0f;
    textObject.RenderMode = TextRenderingModes.Fill;
    textObject.Matrix = new FS_MATRIX(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
    if (flag1)
      textObject.RenderMode = TextRenderingModes.FillThenStroke;
    if (flag2)
      textObject.Matrix = new FS_MATRIX(1f, 0.333f, 0.0f, 1f, 0.0f, 0.0f);
    textObject.FillColor = textColor;
    textObject.StrokeColor = strokeColor;
    textObject.TextUnicode = text;
    return textObject;
  }

  private static void UpdateScripts(
    TextLine textLine,
    PdfTextObject textObj,
    int insertAt,
    ScriptEnum script,
    PdfTextObject splitedObj)
  {
    switch (script)
    {
      case ScriptEnum.SuperScript:
        switch (textObj.GetScript())
        {
          case ScriptEnum.SuperScript:
            FS_POINTF location1 = textObj.Location;
            splitedObj.Location = location1;
            splitedObj.SetScript(ScriptEnum.SuperScript);
            int charsCount1 = textObj.CharsCount;
            float num1 = textObj.FontSize / 0.62f;
            splitedObj.FontSize = num1;
            return;
          case ScriptEnum.SubScript:
            return;
          default:
            return;
        }
      case ScriptEnum.SubScript:
        switch (textObj.GetScript())
        {
          case ScriptEnum.SubScript:
            FS_POINTF location2 = textObj.Location;
            splitedObj.Location = location2;
            splitedObj.SetScript(ScriptEnum.SuperScript);
            int charsCount2 = textObj.CharsCount;
            float num2 = textObj.FontSize / 0.62f;
            splitedObj.FontSize = num2;
            break;
        }
        break;
    }
  }

  internal void CombinAtHeader(
    TextLine line,
    bool addSpace,
    float charSpaceCJK,
    out float ascent,
    out float descent)
  {
    lock (this.locker)
    {
      ascent = 0.0f;
      descent = 0.0f;
      FS_RECTF boundingBox1 = this.GetBoundingBox(false);
      int count1 = this.TextObjects.Count;
      int count2 = line.TextObjects.Count;
      if (count1 == 0)
      {
        if (this.IsRotate)
          return;
        line.GetBoundingBox(true);
        FS_RECTF boundingBox2 = this.GetBoundingBox(false);
        float num = boundingBox2.left;
        float bottom = boundingBox2.bottom;
        for (int index = 0; index < count2; ++index)
        {
          PdfTextObject textObject = line.textObjects[index];
          if (!this.IsRotate)
          {
            FS_RECTF box = textObject.GetBox();
            if (!this.IsVertWriteMode)
            {
              float dx = num - box.left;
              float dy = bottom - box.bottom;
              textObject.Offset(dx, dy);
              num = textObject.GetBox().right;
            }
          }
          this.InsertText(this.TextObjects.Count, textObject);
        }
        for (int index = count2 - 1; index >= 0; --index)
          line.RemoveObj(index);
      }
      else
      {
        this.GetBoundingBox(false);
        FS_POINTF location1 = this.TextObjects[0].Location;
        float num1 = 0.0f;
        if (!this.IsRotate)
        {
          PdfTextObject textObject1 = line.TextObjects[count2 - 1];
          int charsCount = textObject1.CharsCount;
          if (!textObject1.CharIsBlankSpace(charsCount - 1) & addSpace)
          {
            if (textObject1.IsCJK() || textObject1.CharIsCJK(charsCount - 1))
            {
              float num2 = charSpaceCJK;
              if ((double) num2 <= 0.0)
              {
                if (charsCount > 1)
                {
                  float[] charPos = textObject1.GetCharPos(charsCount - 2);
                  num2 = textObject1.GetCharPos(charsCount - 1)[0] - charPos[2];
                }
                else if (count1 > 0)
                {
                  for (int index = 0; index < count1; ++index)
                  {
                    PdfTextObject textObject2 = this.TextObjects[index];
                    if (textObject2.CharsCount > 1)
                    {
                      float[] charPos = textObject2.GetCharPos(0);
                      num2 = textObject2.GetCharPos(1)[0] - charPos[2];
                      break;
                    }
                  }
                }
              }
              if ((double) num2 <= 0.0)
                num1 = 0.1f * textObject1.FontSize;
            }
            else
              textObject1.AppendSpace();
          }
          this.Offset(line.GetBoundingBox(false).Width, 0.0f);
        }
        if (!this.IsRotate && !this.IsVertWriteMode)
        {
          FS_POINTF location2 = line.TextObjects[0].Location;
          float dx = location1.X - location2.X;
          float dy = location1.Y - location2.Y;
          line.Offset(dx, dy);
          for (int index = count2 - 1; index >= 0; --index)
            this.InsertText(0, line.TextObjects[index]);
        }
        this.InvalidateBoundingBox();
        if (!this.IsRotate)
        {
          FS_RECTF fsRectf = boundingBox1;
          FS_RECTF boundingBox3 = this.GetBoundingBox(false);
          if (!this.IsVertWriteMode)
          {
            ascent = fsRectf.top - boundingBox3.top;
            descent = boundingBox3.bottom - fsRectf.bottom;
          }
        }
        for (int index = count2 - 1; index >= 0; --index)
          line.RemoveObj(index);
      }
    }
  }

  internal float SetFontInfo(
    PdfTextObject startTextObj,
    int startCharIdx,
    PdfTextObject endTextObj,
    int endCharIdx,
    PdfFont font,
    float? fontSize)
  {
    lock (this.locker)
    {
      FS_RECTF boundingBox1 = this.GetBoundingBox(false);
      float num1 = 0.0f;
      if (endTextObj == startTextObj)
      {
        if (font != null && !TextLine.CanSetFont(startTextObj, font))
          return 0.0f;
        this.SetFontInfoCore(startTextObj, startCharIdx, endCharIdx, font, fontSize);
        if (!this.IsRotate)
        {
          FS_RECTF boundingBox2 = this.GetBoundingBox(false);
          num1 = !this.IsVertWriteMode ? boundingBox1.top - boundingBox2.top : boundingBox1.right - boundingBox2.right;
        }
        return num1;
      }
      float num2 = 0.0f;
      List<float> floatList = new List<float>();
      int num3 = this.textObjects.IndexOf(startTextObj);
      for (int index = num3; index < this.textObjects.Count; ++index)
      {
        PdfTextObject textObject = this.textObjects[index];
        if (font == null || TextLine.CanSetFont(textObject, font))
        {
          if (textObject == startTextObj)
          {
            if (startCharIdx != 0)
            {
              this.SplitAt(textObject, startCharIdx, false);
              floatList.Add(0.0f);
              ++index;
              textObject = this.textObjects[index];
            }
          }
          else if (textObject == endTextObj)
            this.SplitAt(textObject, startCharIdx, false);
          FS_RECTF box = textObject.GetBox();
          if (font != null)
            textObject.SetFont(font);
          if (fontSize.HasValue)
            textObject.SetFontSize(fontSize.Value);
          float num4 = textObject.GetBox().Width - box.Width;
          num2 += num4;
          floatList.Add(num2);
          if (textObject == endTextObj)
            break;
        }
      }
      if (floatList.Count == 0)
        return 0.0f;
      int index1 = num3 + 1;
      int num5 = 0;
      while (index1 < this.textObjects.Count)
      {
        PdfTextObject textObject = this.textObjects[index1];
        int index2 = num5 < floatList.Count ? num5 : floatList.Count - 1;
        float num6 = floatList[index2];
        if (this.IsVertWriteMode)
          textObject.Offset(0.0f, num6);
        else
          textObject.Offset(num6, 0.0f);
        ++index1;
        ++num5;
      }
      this.InvalidateBoundingBox();
      FS_RECTF boundingBox3 = this.GetBoundingBox(false);
      return !this.IsVertWriteMode ? boundingBox1.top - boundingBox3.top : boundingBox1.right - boundingBox3.right;
    }
  }

  private void SetFontInfoCore(
    PdfTextObject textObj,
    int startCharIdx,
    int endCharIdx,
    PdfFont font,
    float? fontSize)
  {
    lock (this.locker)
    {
      PdfTextObject textObj1 = (PdfTextObject) null;
      int charsCount = textObj.CharsCount;
      if (startCharIdx == 0 && endCharIdx == charsCount - 1)
        textObj1 = textObj;
      else if (startCharIdx == endCharIdx)
      {
        if (startCharIdx == 0)
          textObj1 = this.SplitAt(textObj, startCharIdx + 1, false);
        else if (endCharIdx == charsCount - 1)
          textObj1 = this.SplitAt(textObj, endCharIdx, false);
      }
      else if (startCharIdx == 0)
      {
        this.SplitAt(textObj, endCharIdx, false);
        textObj1 = textObj;
      }
      else if (endCharIdx == charsCount - 1)
      {
        textObj1 = this.SplitAt(textObj, startCharIdx, false);
      }
      else
      {
        this.SplitAt(textObj, endCharIdx, false);
        textObj1 = this.SplitAt(textObj, startCharIdx, false);
      }
      FS_RECTF box1 = textObj1.GetBox();
      if (font != null)
        textObj1.SetFont(font);
      if (fontSize.HasValue)
        textObj1.SetFontSize(fontSize.Value);
      FS_RECTF box2 = textObj1.GetBox();
      float num1 = box2.Width - box1.Width;
      float num2 = box1.top - box2.top;
      if (this.IsVertWriteMode)
      {
        num1 = box2.Height - box1.Height;
        num2 = box1.right - box2.right;
      }
      for (int index = this.textObjects.Count - 1; index >= 0; --index)
      {
        PdfTextObject textObject = this.textObjects[index];
        if (textObject != textObj1)
        {
          if (this.IsVertWriteMode)
            textObject.Offset(0.0f, num1);
          else
            textObject.Offset(num1, 0.0f);
        }
        else
          break;
      }
      this.InvalidateBoundingBox();
    }
  }

  internal void SetBoldItalic(
    PdfTextObject startTextObj,
    int startCharIdx,
    PdfTextObject endTextObj,
    int endCharIdx,
    bool bold,
    bool italic,
    CultureInfo cultureInfo = null)
  {
    lock (this.locker)
    {
      if (endTextObj == startTextObj)
      {
        this.SetBoldItalicCore(startTextObj, startCharIdx, endCharIdx, bold, italic, cultureInfo);
      }
      else
      {
        List<float> floatList = new List<float>();
        float num1 = 0.0f;
        int count = this.textObjects.Count;
        int num2 = this.textObjects.IndexOf(startTextObj);
        for (int index = num2; index < count; ++index)
        {
          PdfTextObject textObject = this.textObjects[index];
          int charsCount = textObject.CharsCount;
          if (textObject == startTextObj && startCharIdx == 0)
          {
            textObject.SetBold(this.page.Document, bold, cultureInfo);
            textObject.SetItalic(this.page.Document, italic, cultureInfo);
            floatList.Add(0.0f);
          }
          else
          {
            if (textObject == endTextObj && endCharIdx < textObject.CharsCount - 1)
              this.SplitAt(textObject, endCharIdx, false);
            FS_RECTF box = textObject.GetBox();
            textObject.SetBold(this.page.Document, bold, cultureInfo);
            textObject.SetItalic(this.page.Document, italic, cultureInfo);
            float num3 = textObject.GetBox().Width - box.Width;
            num1 += num3;
            floatList.Add(num1);
          }
          if (textObject == endTextObj)
            break;
        }
        int index1 = num2 + 1;
        int num4 = 0;
        while (index1 < this.textObjects.Count)
        {
          PdfTextObject textObject = this.textObjects[index1];
          int index2 = num4 < floatList.Count ? num4 : floatList.Count - 1;
          float num5 = floatList[index2];
          if (this.IsVertWriteMode)
            textObject.Offset(0.0f, num5);
          else
            textObject.Offset(num5, 0.0f);
          ++index1;
          ++num4;
        }
        this.InvalidateBoundingBox();
      }
    }
  }

  internal void SetBoldItalicCore(
    PdfTextObject textObj,
    int startCharIdx,
    int endCharIdx,
    bool bold,
    bool italic,
    CultureInfo cultureInfo = null)
  {
    lock (this.locker)
    {
      PdfTextObject textObj1 = (PdfTextObject) null;
      int charsCount = textObj.CharsCount;
      if (startCharIdx == 0 && endCharIdx == charsCount - 1)
        textObj1 = textObj;
      else if (startCharIdx == endCharIdx)
      {
        if (startCharIdx == 0)
          textObj1 = this.SplitAt(textObj, startCharIdx + 1, false);
        else if (endCharIdx == charsCount - 1)
          textObj1 = this.SplitAt(textObj, endCharIdx, false);
      }
      else if (startCharIdx == 0)
      {
        this.SplitAt(textObj, endCharIdx, false);
        textObj1 = textObj;
      }
      else if (endCharIdx == charsCount - 1)
      {
        textObj1 = this.SplitAt(textObj, startCharIdx, false);
      }
      else
      {
        this.SplitAt(textObj, endCharIdx, false);
        textObj1 = this.SplitAt(textObj, startCharIdx, false);
      }
      FS_RECTF box1 = textObj1.GetBox();
      textObj1.SetBold(this.page.Document, bold, cultureInfo);
      textObj1.SetItalic(this.page.Document, italic, cultureInfo);
      FS_RECTF box2 = textObj1.GetBox();
      float num1 = box2.Width - box1.Width;
      float num2 = box1.top - box2.top;
      if (this.IsVertWriteMode)
      {
        num1 = box2.Height - box1.Height;
        num2 = box1.right - box2.right;
      }
      for (int index = this.textObjects.Count - 1; index >= 0; --index)
      {
        PdfTextObject textObject = this.textObjects[index];
        if (textObject != textObj1)
        {
          if (this.IsVertWriteMode)
            textObject.Offset(0.0f, num1);
          else
            textObject.Offset(num1, 0.0f);
        }
        else
          break;
      }
      this.InvalidateBoundingBox();
    }
  }

  private static bool CanSetFont(PdfTextObject textObj, PdfFont font)
  {
    int charsCount = textObj.CharsCount;
    if (charsCount > 0)
    {
      PdfFont font1 = textObj.Font;
      for (int index = 0; index < charsCount; ++index)
      {
        int charCode;
        textObj.GetCharInfo(index, out charCode, out float _);
        if (font1 != null)
        {
          char unicode = font.ToUnicode(charCode);
          if (font.ToCharCode(unicode) == -1)
            return false;
        }
      }
    }
    return true;
  }
}
