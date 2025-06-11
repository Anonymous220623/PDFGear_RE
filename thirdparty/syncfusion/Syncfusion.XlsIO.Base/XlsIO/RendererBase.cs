// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.RendererBase
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using Syncfusion.XlsIO.Implementation.Collections;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;

#nullable disable
namespace Syncfusion.XlsIO;

internal abstract class RendererBase
{
  private const string NewLineKey = "\n";
  private IRange _cell;
  private StringFormat _stringFormat;
  private WorkbookImpl _workBookImpl;
  private List<IFont> _richTextFont;
  private List<string> _drawString;
  private float _leftWidth;
  private float _rightWidth;
  private RectangleF _cellRect;

  internal abstract bool IsHfRtfProcess { get; set; }

  internal abstract RectangleF HfImageBounds { get; set; }

  internal void SetWidth(float leftWidth, float rightWidth)
  {
    this._leftWidth = leftWidth;
    this._rightWidth = rightWidth;
  }

  internal RendererBase(
    IRange cell,
    StringFormat stringFormat,
    List<IFont> fonts,
    List<string> drawString,
    WorkbookImpl workbook)
  {
    this._cell = cell;
    this._stringFormat = stringFormat;
    this._workBookImpl = workbook;
    this._richTextFont = fonts;
    this._drawString = drawString;
  }

  internal void DrawRTFText(
    RectangleF cellRect,
    RectangleF adjacentRect,
    bool isShape,
    bool isWrapText,
    bool isHorizontalTextOverflow,
    bool isVerticalTextOverflow,
    bool isChartShape,
    bool isHeaderFooter)
  {
    this._cellRect = cellRect;
    if (this._cell != null && (this._cell.VerticalAlignment == ExcelVAlign.VAlignJustify || (this._cell as RangeImpl).ExtendedFormat != null && (this._cell as RangeImpl).ExtendedFormat.Rotation == (int) byte.MaxValue))
      this._stringFormat.Trimming = StringTrimming.Word;
    List<LineInfoImpl> lineInfoCollection = new List<LineInfoImpl>();
    List<TextInfoImpl> textInfoCollection = new List<TextInfoImpl>();
    float usedWidth = 0.0f;
    float usedHeight = 0.0f;
    float maxHeight = 0.0f;
    float maxAscent = 0.0f;
    for (int index1 = 0; index1 < this._drawString.Count; ++index1)
    {
      string str1 = this._drawString[index1];
      IFont font = this._richTextFont[index1];
      bool flag = this.CheckUnicode(str1);
      string str2 = font.FontName;
      if (flag)
      {
        if (font is FontImpl)
          str2 = this.SwitchFonts(str1, (font as FontImpl).CharSet, str2);
        if (font is FontWrapper)
          str2 = this.SwitchFonts(str1, (byte) (font as FontWrapper).CharSet, str2);
      }
      Font systemFont = this.GetSystemFont(font, str2);
      SizeF size = this.MeasureString(str1, systemFont);
      float ascent1 = this.FindAscent(str1, systemFont);
      if (str1.Contains("\n"))
      {
        if (this._stringFormat.Trimming != StringTrimming.Word)
        {
          if (!str1.Equals("\n"))
          {
            this._drawString[index1] = str1.Replace("\n", string.Empty);
            --index1;
          }
        }
        else if (str1.Equals("\n"))
        {
          if ((double) size.Height > (double) maxHeight)
            maxHeight = size.Height;
          this.LayoutNewLine(adjacentRect, ref usedHeight, lineInfoCollection, ref textInfoCollection, ref usedWidth, ref maxAscent, ref maxHeight, true, string.Empty, isShape, isHeaderFooter);
          if (index1 + 1 == this._drawString.Count && (double) size.Height > (double) maxHeight)
            maxHeight = size.Height;
        }
        else
        {
          this._drawString.RemoveAt(index1);
          this._richTextFont.RemoveAt(index1);
          int startIndex = 0;
          int num1 = 0;
          while (str1.IndexOf("\n", startIndex) > -1)
          {
            int num2 = str1.IndexOf("\n", startIndex);
            string str3 = str1.Substring(startIndex, num2 - startIndex == 0 ? 1 : num2 - startIndex);
            startIndex += str3.Length;
            this._drawString.Insert(index1 + num1, str3);
            this._richTextFont.Insert(index1 + num1++, font);
          }
          if (startIndex < str1.Length)
          {
            string str4 = str1.Substring(startIndex, str1.Length - startIndex);
            this._drawString.Insert(index1 + num1, str4);
            this._richTextFont.Insert(index1 + num1, font);
          }
          --index1;
        }
      }
      else
      {
        if (this.IsHfRtfProcess && str1.Contains("HeaderFooterImage"))
        {
          string[] strArray = str1.Split(':');
          size.Width = float.Parse(strArray[1], (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat);
          size.Height = ascent1 = float.Parse(strArray[2], (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat);
        }
        if ((double) size.Width <= (double) adjacentRect.Width - (double) usedWidth || this._stringFormat.Trimming != StringTrimming.Word || this.IsHfRtfProcess && str1.Contains("HeaderFooterImage") || !isWrapText)
        {
          TextInfoImpl textInfoImpl = this.LayoutText(adjacentRect, ref usedWidth, usedHeight, font, size, ascent1, ref maxHeight, ref maxAscent, str1, str2, isShape, isHeaderFooter);
          textInfoImpl.Length = str1.Length;
          textInfoCollection.Add(textInfoImpl);
        }
        else
        {
          if (!str1.TrimStart(' ').TrimEnd(' ').Contains(" "))
          {
            if (!str1.TrimStart(' ').TrimEnd(' ').Contains("-") && !str1.EndsWith(" "))
            {
              this.SplitByCharacter(adjacentRect, ref usedHeight, lineInfoCollection, ref textInfoCollection, ref usedWidth, ref maxAscent, ref maxHeight, str1, font, ref size, ascent1, 0, str1, str2, isShape, isHeaderFooter);
              goto label_47;
            }
          }
          int currPosition = 0;
          while (currPosition < str1.Length)
          {
            int index2 = this.GetSpaceIndexBeforeText(str1, currPosition);
            int num = str1.IndexOf('-', currPosition);
            if (num >= 0 && (num < index2 || index2 == -1))
              index2 = num;
            string str5;
            if (index2 >= 0)
              str5 = str1.Substring(currPosition, index2 + 1 - currPosition);
            else
              str5 = str1.Substring(currPosition, str1.Length - currPosition).TrimEnd(' ');
            string text1 = str5;
            size = this.MeasureString(text1, systemFont);
            float ascent2 = this.FindAscent(text1, systemFont);
            if ((double) size.Width <= (double) adjacentRect.Width - (double) usedWidth || this._workBookImpl.IsNullOrWhiteSpace(text1))
            {
              this.LayoutSplittedText(adjacentRect, usedHeight, textInfoCollection, ref usedWidth, ref maxAscent, ref maxHeight, font, str1, ref size, ascent2, ref currPosition, index2, str2);
            }
            else
            {
              if (index2 > 0)
              {
                string text2 = str1.Substring(currPosition, index2 + 1 - currPosition).TrimEnd(' ');
                if (text2 == string.Empty)
                {
                  currPosition = index2 + 1;
                  continue;
                }
                SizeF sizeF = this.MeasureString(text2, systemFont);
                ascent2 = this.FindAscent(text2, systemFont);
                if ((double) sizeF.Width <= (double) adjacentRect.Width - (double) usedWidth)
                {
                  this.LayoutSplittedText(adjacentRect, usedHeight, textInfoCollection, ref usedWidth, ref maxAscent, ref maxHeight, font, str1, ref size, ascent2, ref currPosition, index2, str2);
                  continue;
                }
              }
              if ((double) size.Width <= (double) adjacentRect.Width && (double) adjacentRect.Width - (double) usedWidth > 0.0)
              {
                this.LayoutNewLine(adjacentRect, ref usedHeight, lineInfoCollection, ref textInfoCollection, ref usedWidth, ref maxAscent, ref maxHeight, false, text1, isShape, isHeaderFooter);
                this.LayoutSplittedText(adjacentRect, usedHeight, textInfoCollection, ref usedWidth, ref maxAscent, ref maxHeight, font, str1, ref size, ascent2, ref currPosition, index2, str2);
              }
              else
              {
                this.SplitByCharacter(adjacentRect, ref usedHeight, lineInfoCollection, ref textInfoCollection, ref usedWidth, ref maxAscent, ref maxHeight, text1, font, ref size, ascent2, currPosition, str1, str2, isShape, isHeaderFooter);
                currPosition += text1.Length;
              }
            }
          }
        }
label_47:
        systemFont.Dispose();
      }
    }
    this.LayoutNewLine(adjacentRect, ref usedHeight, lineInfoCollection, ref textInfoCollection, ref usedWidth, ref maxAscent, ref maxHeight, true, string.Empty, isShape, isHeaderFooter);
    float y = 0.0f;
    if (this._cell != null && this._cell.VerticalAlignment == ExcelVAlign.VAlignJustify)
    {
      if (lineInfoCollection.Count > 1 && (double) adjacentRect.Height > (double) usedHeight)
      {
        float num = (adjacentRect.Height - usedHeight) / (float) (lineInfoCollection.Count - 1);
        for (int index = 0; index < lineInfoCollection.Count; ++index)
        {
          if (index != 0)
          {
            foreach (TextInfoImpl textInfo in lineInfoCollection[index].TextInfoCollection)
              textInfo.Y += num * (float) index;
          }
        }
        y = 0.0f;
      }
    }
    else
    {
      if (this._cell != null)
      {
        double num3 = this._workBookImpl.GetCellScaledWidthHeight(this._cell.Worksheet)[1];
        double scaledHeight = this._workBookImpl.GetScaledHeight(this._cell.CellStyle.Font.FontName, this._cell.CellStyle.Font.Size, this._cell.Worksheet);
        double num4 = (double) adjacentRect.Height / num3;
        RowStorage rowStorage = (this._cell as RangeImpl).RowStorage;
        if (rowStorage != null && !rowStorage.IsBadFontHeight)
        {
          if (rowStorage.IsSpaceAboveRow)
            num4 -= 0.5;
          if (rowStorage.IsSpaceBelowRow)
            num4 -= 0.5;
        }
        usedHeight = this.UpdateTextHeight((float) (num4 * scaledHeight), lineInfoCollection, false);
      }
      else if (isShape && isWrapText && !isVerticalTextOverflow)
        usedHeight = this.UpdateTextHeight(adjacentRect.Height, lineInfoCollection, true);
      switch (this._stringFormat.LineAlignment)
      {
        case StringAlignment.Center:
          y = (float) (((double) adjacentRect.Height - (double) usedHeight) / 2.0);
          break;
        case StringAlignment.Far:
          y = adjacentRect.Height - usedHeight;
          break;
      }
    }
    usedWidth = lineInfoCollection[lineInfoCollection.Count - 1].Width;
    for (int index = 0; index < lineInfoCollection.Count; ++index)
    {
      if ((double) lineInfoCollection[index].Width > (double) usedWidth)
        usedWidth = lineInfoCollection[index].Width;
    }
    if (!isChartShape)
      this.InitializeStringFormat();
    float width = lineInfoCollection[0].Width;
    RectangleF bounds = new RectangleF();
    if (this._stringFormat.Alignment == StringAlignment.Center && this._stringFormat.Trimming != StringTrimming.Word && ((double) this._leftWidth != 0.0 || (double) this._rightWidth != 0.0))
      this.DrawTextTemplate(adjacentRect, lineInfoCollection, y);
    else if (isShape && isHorizontalTextOverflow && !isWrapText)
    {
      bounds = new RectangleF(adjacentRect.X, adjacentRect.Y, usedWidth, adjacentRect.Height);
      this.DrawTextTemplate(bounds, lineInfoCollection, y);
    }
    else if (isShape && isVerticalTextOverflow && isWrapText && (double) usedHeight > (double) adjacentRect.Height)
    {
      foreach (LineInfoImpl lineInfoImpl in lineInfoCollection)
      {
        foreach (TextInfoImpl textInfo in lineInfoImpl.TextInfoCollection)
        {
          textInfo.Y += y;
          this.DrawString(textInfo, this._stringFormat);
        }
      }
    }
    else if (isShape && !isWrapText && !isHorizontalTextOverflow && !isVerticalTextOverflow && (double) adjacentRect.Width <= (double) usedWidth)
      this.DrawTextTemplate(adjacentRect, lineInfoCollection, y);
    else if (this._stringFormat.Trimming == StringTrimming.Word || (double) adjacentRect.Width >= (double) width)
    {
      if ((double) adjacentRect.Height >= (double) usedHeight)
      {
        foreach (LineInfoImpl lineInfoImpl in lineInfoCollection)
        {
          foreach (TextInfoImpl textInfo in lineInfoImpl.TextInfoCollection)
          {
            textInfo.Y += y;
            if (this.IsHfRtfProcess && textInfo.Text.Contains("HeaderFooterImage"))
              this.HfImageBounds = textInfo.Bounds;
            else
              this.DrawString(textInfo, this._stringFormat);
          }
        }
      }
      else
      {
        if (isShape && lineInfoCollection.Count > 0 && lineInfoCollection.Count == 1)
        {
          foreach (LineInfoImpl lineInfoImpl in lineInfoCollection)
          {
            foreach (TextInfoImpl textInfo in lineInfoImpl.TextInfoCollection)
            {
              if (textInfo.Font.Underline != ExcelUnderline.None)
                adjacentRect.Height += 0.5f;
            }
          }
        }
        this.DrawTextTemplate(adjacentRect, lineInfoCollection, y);
      }
    }
    else
    {
      switch (this._stringFormat.LineAlignment)
      {
        case StringAlignment.Near:
          adjacentRect.Width += cellRect.X - adjacentRect.X;
          break;
        case StringAlignment.Far:
          adjacentRect.Width += adjacentRect.X - cellRect.X;
          adjacentRect.X = cellRect.X;
          break;
      }
      this.DrawTextTemplate(adjacentRect, lineInfoCollection, y);
    }
    this._leftWidth = 0.0f;
    this._rightWidth = 0.0f;
    if (this._richTextFont != null)
    {
      this._richTextFont.Clear();
      this._richTextFont = (List<IFont>) null;
    }
    if (this._drawString != null)
    {
      this._drawString.Clear();
      this._drawString = (List<string>) null;
    }
    if (lineInfoCollection != null)
    {
      foreach (LineInfoImpl lineInfoImpl in lineInfoCollection)
        lineInfoImpl.Dispose();
      lineInfoCollection.Clear();
    }
    if (textInfoCollection == null)
      return;
    foreach (TextInfoImpl textInfoImpl in textInfoCollection)
      textInfoImpl.Dispose();
    textInfoCollection.Clear();
  }

  internal float UpdateTextHeight(
    float height,
    List<LineInfoImpl> lineInfoCollection,
    bool isShape)
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    if (isShape)
    {
      int index1;
      for (index1 = 0; index1 < lineInfoCollection.Count; ++index1)
      {
        num1 += lineInfoCollection[index1].Height;
        if ((double) height + 4.0 >= (double) num1)
          num2 = num1;
        else
          break;
      }
      if (index1 == 0)
      {
        num2 = lineInfoCollection[0].Height;
        index1 = 1;
      }
      if (index1 < lineInfoCollection.Count)
      {
        int index2 = index1;
        while (index2 < lineInfoCollection.Count)
          lineInfoCollection.RemoveAt(index2);
      }
    }
    else
    {
      int index3;
      for (index3 = 0; index3 < lineInfoCollection.Count; ++index3)
      {
        num1 += lineInfoCollection[index3].Height;
        if ((double) height <= (double) num1)
        {
          num2 = num1;
          ++index3;
          break;
        }
        num2 = num1;
      }
      if (index3 < lineInfoCollection.Count)
      {
        int index4 = index3;
        while (index4 < lineInfoCollection.Count)
          lineInfoCollection.RemoveAt(index4);
      }
    }
    return num2;
  }

  internal abstract bool CheckUnicode(string text);

  internal abstract string SwitchFonts(string text, byte charSet, string fontName);

  internal abstract void InitializeStringFormat();

  internal abstract void DrawString(TextInfoImpl textInfo, StringFormat stringFormat);

  internal abstract SizeF MeasureString(string text, Font systemFont);

  internal abstract string CheckPdfFont(Font sysFont, string testString);

  internal virtual float FindAscent(string text, Font font)
  {
    int emHeight = font.FontFamily.GetEmHeight(font.Style);
    int cellAscent = font.FontFamily.GetCellAscent(font.Style);
    return font.SizeInPoints * (float) cellAscent / (float) emHeight;
  }

  internal void LayoutNewLine(
    RectangleF shapeBounds,
    ref float usedHeight,
    List<LineInfoImpl> lineInfoCollection,
    ref List<TextInfoImpl> textInfoCollection,
    ref float usedWidth,
    ref float maxAscent,
    ref float maxHeight,
    bool isLastLine,
    string text,
    bool isShape,
    bool isHeaderFooter)
  {
    LineInfoImpl lineInfoImpl = new LineInfoImpl();
    lineInfoImpl.TextInfoCollection = textInfoCollection;
    lineInfoCollection.Add(lineInfoImpl);
    this.LayoutXYPosition(usedWidth, maxAscent, shapeBounds.Width, textInfoCollection, isLastLine, ref maxHeight, ref usedHeight, isShape, isHeaderFooter);
    if (textInfoCollection.Count == 0)
      lineInfoImpl.Height = maxHeight;
    textInfoCollection = new List<TextInfoImpl>();
    usedHeight += maxHeight;
    usedWidth = 0.0f;
    maxHeight = 0.0f;
    maxAscent = 0.0f;
    this.CheckPreviousElement(lineInfoCollection, ref textInfoCollection, text, ref usedWidth, usedHeight, shapeBounds, isShape, isHeaderFooter);
  }

  private void LayoutXYPosition(
    float usedWidth,
    float maxAscent,
    float shapeWidth,
    List<TextInfoImpl> textInfoCollection,
    bool isLastLine,
    ref float maxHeight,
    ref float usedHeight,
    bool isShape,
    bool isHeaderFooter)
  {
    float x1 = 0.0f;
    float y = 0.0f;
    if (this.IsJustify())
    {
      if (textInfoCollection.Count <= 0)
        return;
      if (!isLastLine)
        this.LineJustify(ref usedWidth, maxAscent, shapeWidth, textInfoCollection);
      else
        this.UpdateXYPosition(maxAscent, (IEnumerable<TextInfoImpl>) textInfoCollection, x1, y, isShape, isHeaderFooter, ref maxHeight);
    }
    else
    {
      switch (this._stringFormat.Alignment)
      {
        case StringAlignment.Near:
          this.UpdateXYPosition(maxAscent, (IEnumerable<TextInfoImpl>) textInfoCollection, x1, y, isShape, isHeaderFooter, ref maxHeight);
          break;
        case StringAlignment.Center:
          usedWidth -= this.RemoveWhiteSpaces(textInfoCollection);
          float x2 = !this.IsHfRtfProcess ? (this._cell == null || this._cell.HorizontalAlignment != ExcelHAlign.HAlignCenterAcrossSelection ? this._leftWidth + (float) (((double) this._cellRect.Width - (double) usedWidth) / 2.0) : (float) (((double) shapeWidth - (double) usedWidth) / 2.0)) : (float) (((double) shapeWidth - (double) usedWidth) / 2.0);
          this.UpdateXYPosition(maxAscent, (IEnumerable<TextInfoImpl>) textInfoCollection, x2, y, isShape, isHeaderFooter, ref maxHeight);
          break;
        case StringAlignment.Far:
          usedWidth -= this.RemoveWhiteSpaces(textInfoCollection);
          float x3 = shapeWidth - usedWidth;
          this.UpdateXYPosition(maxAscent, (IEnumerable<TextInfoImpl>) textInfoCollection, x3, y, isShape, isHeaderFooter, ref maxHeight);
          break;
      }
    }
  }

  internal abstract bool IsJustify();

  private void CheckPreviousElement(
    List<LineInfoImpl> lineInfoCollection,
    ref List<TextInfoImpl> currTextInfoCollection,
    string text,
    ref float usedWidth,
    float usedHeight,
    RectangleF shapeBounds,
    bool isShape,
    bool isHeaderFooter)
  {
    if (lineInfoCollection.Count <= 0 || !(text != string.Empty) || text.StartsWith(" "))
      return;
    LineInfoImpl lineInfo = lineInfoCollection[lineInfoCollection.Count - 1];
    List<TextInfoImpl> textInfoCollection = lineInfo.TextInfoCollection;
    string text1 = lineInfo.Text;
    if (text1 == string.Empty || !text1.Contains(" ") || text1.EndsWith(" "))
      return;
    int num1 = 0;
    float num2 = 0.0f;
    for (int index = textInfoCollection.Count - 1; index >= 0; --index)
    {
      TextInfoImpl textInfo = textInfoCollection[index];
      string text2 = textInfo.Text;
      if (text2 != string.Empty && !text2.Contains(" ") && !text2.EndsWith("-"))
      {
        ++num1;
      }
      else
      {
        if (text2.TrimEnd(' ').Contains(" ") && !text2.EndsWith(" "))
        {
          string str = text2.TrimEnd(' ');
          int num3 = str.LastIndexOf(' ') + 1;
          TextInfoImpl textInfoImpl = new TextInfoImpl(textInfo.GetOriginalText());
          textInfo.CopyTo(textInfoImpl);
          textInfoImpl.Position = num3;
          textInfoImpl.Length = str.Length - num3;
          textInfo.Length = num3;
          textInfo.Width = this.MeasureString(textInfo.Text, this.GetSystemFont(textInfo)).Width;
          textInfoImpl.X = shapeBounds.Left + usedWidth;
          textInfoImpl.Y = shapeBounds.Top + usedHeight;
          SizeF sizeF = this.MeasureString(textInfoImpl.Text, this.GetSystemFont(textInfoImpl));
          usedWidth += num2 = textInfoImpl.Width = sizeF.Width;
          textInfoImpl.Height = sizeF.Height;
          currTextInfoCollection.Add(textInfoImpl);
          break;
        }
        if (text2.Equals(string.Empty))
          ++num1;
        else
          break;
      }
    }
    int index1 = textInfoCollection.Count - num1;
    while (textInfoCollection.Count != index1)
    {
      TextInfoImpl textInfoImpl = textInfoCollection[index1];
      textInfoCollection.RemoveAt(index1);
      currTextInfoCollection.Add(textInfoImpl);
      textInfoImpl.X = shapeBounds.Left + usedWidth;
      textInfoImpl.Y = shapeBounds.Top + usedHeight + this.AlignSuperOrSubScript(textInfoImpl.Text, textInfoImpl.Font, isShape, isHeaderFooter);
      usedWidth += textInfoImpl.Width;
      num2 += textInfoImpl.Width;
    }
    if ((double) num2 == 0.0)
      return;
    switch (this._stringFormat.Alignment)
    {
      case StringAlignment.Center:
        float num4 = num2 + this.RemoveWhiteSpaces(textInfoCollection);
        this.UpdateXPosition((IEnumerable<TextInfoImpl>) textInfoCollection, num4 / 2f);
        break;
      case StringAlignment.Far:
        float x = num2 + this.RemoveWhiteSpaces(textInfoCollection);
        this.UpdateXPosition((IEnumerable<TextInfoImpl>) textInfoCollection, x);
        break;
      default:
        if (this._cell == null || this._cell.HorizontalAlignment != ExcelHAlign.HAlignJustify)
          break;
        string empty = string.Empty;
        float num5 = -this.RemoveWhiteSpaces(textInfoCollection);
        foreach (TextInfoImpl textInfoImpl in textInfoCollection)
        {
          if (textInfoImpl.Text != null)
            empty += textInfoImpl.Text;
          num5 += textInfoImpl.Width;
        }
        int num6 = empty.TrimEnd(' ').Length - empty.Replace(" ", string.Empty).Length;
        for (int index2 = 0; index2 < textInfoCollection.Count; ++index2)
          RendererBase.UpdateLineJustifyPosition(textInfoCollection, (shapeBounds.Width - num5) / (float) num6, index2);
        break;
    }
  }

  internal abstract void DrawTextTemplate(
    RectangleF bounds,
    List<LineInfoImpl> lineInfoCollection,
    float y);

  private float AlignSuperOrSubScript(string text, IFont font, bool isShape, bool isHeaderFooter)
  {
    float num1 = 0.0f;
    float num2 = 1.35f;
    if (font.Superscript)
    {
      Font font1 = this._workBookImpl.GetFont(font);
      float ascent = this.FindAscent(text, font1);
      int height = font1.Height;
      num1 = isShape || isHeaderFooter ? (float) -(((double) num1 + (double) ascent + ((double) height / (double) num2 - (double) height)) / 1.5) : (float) -((double) num1 + (double) ascent + ((double) height / (double) num2 - (double) height));
    }
    else if (font.Subscript)
    {
      Font font2 = this._workBookImpl.GetFont(font);
      float ascent = this.FindAscent(text, font2);
      int height = font2.Height;
      num1 = (float) (((double) num1 + (double) ascent + ((double) height / (double) num2 - (double) height)) / 3.0);
    }
    return num1;
  }

  internal Color NormalizeColor(Color color)
  {
    return color.A == (byte) 0 ? Color.FromArgb((int) byte.MaxValue, (int) color.R, (int) color.G, (int) color.B) : color;
  }

  private TextInfoImpl LayoutText(
    RectangleF adjacentRect,
    ref float usedWidth,
    float usedHeight,
    IFont font,
    SizeF size,
    float ascent,
    ref float maxHeight,
    ref float maxAscent,
    string text,
    string unicodeFont,
    bool isShape,
    bool isHeaderFooter)
  {
    float x = adjacentRect.Left + usedWidth;
    float y = adjacentRect.Top + usedHeight + this.AlignSuperOrSubScript(text, font, isShape, isHeaderFooter);
    float width = size.Width;
    float height = size.Height;
    usedWidth += size.Width;
    if ((double) size.Height > (double) maxHeight)
      maxHeight = size.Height;
    if ((double) ascent > (double) maxAscent)
      maxAscent = ascent;
    return new TextInfoImpl(text)
    {
      Bounds = new RectangleF(x, y, width, height),
      Ascent = ascent,
      Font = font,
      UnicodeFont = unicodeFont
    };
  }

  private int GetSpaceIndexBeforeText(string text, int startIndex)
  {
    int index = text.IndexOf(' ', startIndex);
    if (index < 0 || index == text.Length)
      return index;
    while (text[index] == ' ')
    {
      ++index;
      if (index == text.Length)
        break;
    }
    return index - 1;
  }

  private void LayoutSplittedText(
    RectangleF shapeBounds,
    float usedHeight,
    List<TextInfoImpl> textInfoCollection,
    ref float usedWidth,
    ref float maxAscent,
    ref float maxHeight,
    IFont font,
    string text,
    ref SizeF size,
    float ascent,
    ref int currPosition,
    int index,
    string unicodeFont)
  {
    TextInfoImpl textInfoImpl = this.LayoutText(shapeBounds, ref usedWidth, usedHeight, font, size, ascent, ref maxHeight, ref maxAscent, text, unicodeFont, false, false);
    textInfoImpl.Position = currPosition;
    textInfoCollection.Add(textInfoImpl);
    if (index > -1)
    {
      textInfoImpl.Length = index + 1 - currPosition;
      currPosition = index + 1;
    }
    else
    {
      textInfoImpl.Length = text.Length - currPosition;
      currPosition = text.Length;
    }
  }

  private void SplitByCharacter(
    RectangleF shapeBounds,
    ref float usedHeight,
    List<LineInfoImpl> lineInfoCollection,
    ref List<TextInfoImpl> textInfoCollection,
    ref float usedWidth,
    ref float maxAscent,
    ref float maxHeight,
    string text,
    IFont font,
    ref SizeF size,
    float ascent,
    int currPosition,
    string originalText,
    string unicodeFont,
    bool isShape,
    bool isHeaderFooter)
  {
    int textIndexAfterSpace = this.GetTextIndexAfterSpace(text.TrimEnd(' '), 0);
    int length = 1;
    float num = 0.0f;
    string text1 = string.Empty;
    if ((double) usedWidth > 0.0 && textInfoCollection.Count > 1 && (double) size.Width <= (double) shapeBounds.Width)
      this.LayoutNewLine(shapeBounds, ref usedHeight, lineInfoCollection, ref textInfoCollection, ref usedWidth, ref maxAscent, ref maxHeight, false, text, isShape, isHeaderFooter);
    while (textIndexAfterSpace + length <= text.Length)
    {
      text1 = text.Substring(textIndexAfterSpace, length);
      size = this.MeasureString(text1, this.GetSystemFont(font, unicodeFont));
      if ((double) size.Width > (double) shapeBounds.Width - (double) usedWidth)
      {
        if (text1.Length > 1)
          size.Width = num;
        TextInfoImpl textInfo = this.LayoutText(shapeBounds, ref usedWidth, usedHeight, font, size, ascent, ref maxHeight, ref maxAscent, originalText, unicodeFont, isShape, isHeaderFooter);
        textInfo.Position = textIndexAfterSpace + currPosition;
        if ((double) shapeBounds.Width - (double) usedWidth < 0.0)
        {
          textInfo.Length = length;
          textInfo.Width = this.MeasureString(text1, this.GetSystemFont(textInfo)).Width;
          ascent = this.FindAscent(text, this.GetSystemFont(textInfo));
        }
        else
          textInfo.Length = (double) usedWidth != 0.0 ? length - 1 : length;
        textInfoCollection.Add(textInfo);
        if (text1.Length == 1 && textInfoCollection.Count == 1)
          textIndexAfterSpace += length;
        else
          textIndexAfterSpace += length - 1;
        length = 1;
        this.LayoutNewLine(shapeBounds, ref usedHeight, lineInfoCollection, ref textInfoCollection, ref usedWidth, ref maxAscent, ref maxHeight, false, text1[text1.Length - 1].ToString((IFormatProvider) CultureInfo.InvariantCulture), isShape, isHeaderFooter);
        if (textInfoCollection.Count == 1)
        {
          textInfoCollection = new List<TextInfoImpl>();
          usedWidth = 0.0f;
          textIndexAfterSpace = this.GetTextIndexAfterSpace(text.TrimEnd(' '), 0);
        }
        text1 = string.Empty;
      }
      else
      {
        num = size.Width;
        ++length;
      }
    }
    if (!(text1 != string.Empty))
      return;
    TextInfoImpl textInfoImpl = this.LayoutText(shapeBounds, ref usedWidth, usedHeight, font, size, ascent, ref maxHeight, ref maxAscent, originalText, unicodeFont, isShape, isHeaderFooter);
    textInfoImpl.Position = textIndexAfterSpace + currPosition;
    textInfoImpl.Length = length - 1;
    textInfoCollection.Add(textInfoImpl);
  }

  private int GetTextIndexAfterSpace(string text, int startIndex)
  {
    int index = text.IndexOf(' ', startIndex) + 1;
    if (index == 0 || index == text.Length)
      return index;
    while (text[index] == ' ')
    {
      ++index;
      if (index == text.Length)
        break;
    }
    return index;
  }

  private void UpdateXYPosition(
    float maxAscent,
    IEnumerable<TextInfoImpl> textInfoCollection,
    float x,
    float y,
    bool isShape,
    bool isHeaderFooter,
    ref float maxHeight)
  {
    foreach (TextInfoImpl textInfo in textInfoCollection)
    {
      textInfo.X += x;
      if ((double) maxAscent == (double) textInfo.Ascent && textInfo.Font.Superscript && (isShape || isHeaderFooter))
      {
        textInfo.Font.Superscript = false;
        Font systemFont = this.GetSystemFont(textInfo.Font, textInfo.Font.FontName);
        float ascent = this.FindAscent(textInfo.Text, systemFont);
        if (isHeaderFooter)
        {
          SizeF sizeF = this.MeasureString(textInfo.Text, systemFont);
          if ((double) maxHeight < (double) sizeF.Height)
            maxHeight = sizeF.Height;
        }
        textInfo.Font.Superscript = true;
        textInfo.Y += ascent - textInfo.Ascent + y;
      }
      else if ((double) maxAscent == (double) textInfo.Ascent && textInfo.Font.Subscript && isHeaderFooter)
      {
        textInfo.Font.Subscript = false;
        Font systemFont = this.GetSystemFont(textInfo.Font, textInfo.Font.FontName);
        if (isHeaderFooter)
        {
          SizeF sizeF = this.MeasureString(textInfo.Text, systemFont);
          if ((double) maxHeight < (double) sizeF.Height)
            maxHeight = sizeF.Height;
        }
        float ascent = this.FindAscent(textInfo.Text, systemFont);
        textInfo.Font.Subscript = true;
        textInfo.Y += (float) ((double) ascent - (double) textInfo.Ascent + (double) y - 1.0);
      }
      else
        textInfo.Y += maxAscent - textInfo.Ascent + y;
    }
  }

  private void LineJustify(
    ref float usedWidth,
    float maxAscent,
    float shapeWidth,
    List<TextInfoImpl> textInfoCollection)
  {
    List<TextInfoImpl> collection = new List<TextInfoImpl>();
    float x1 = textInfoCollection[0].Bounds.X;
    string empty = string.Empty;
    foreach (TextInfoImpl textInfo in textInfoCollection)
    {
      string originalText = textInfo.GetOriginalText();
      int startIndex1 = textInfo.Position;
      int position = textInfo.Position;
      if (originalText.Substring(position, originalText.Length - position).Contains(" "))
      {
        while (startIndex1 < textInfo.Length + textInfo.Position)
        {
          int startIndex2 = startIndex1;
          startIndex1 = this.GetTextIndexAfterSpace(originalText, startIndex1);
          int index = originalText.IndexOf('-', startIndex2) + 1;
          while (index > 0 && index < originalText.Length && originalText[index].ToString() == " ")
            ++index;
          if (index > 0 && (index < startIndex1 || startIndex1 == 0))
            startIndex1 = index;
          if (startIndex1 != 0 || position != originalText.Length)
          {
            if (startIndex1 == 0)
              startIndex1 = originalText.Length;
            if (startIndex1 <= originalText.Length)
            {
              if (originalText.Substring(position, startIndex1 - position).TrimEnd(' ').StartsWith(" ") && !string.IsNullOrEmpty(originalText))
              {
                position += originalText.Length - originalText.TrimStart(' ').Length;
              }
              else
              {
                TextInfoImpl destination = new TextInfoImpl(textInfo.GetOriginalText());
                string text = originalText.Substring(position, startIndex1 - position);
                destination.Position = position;
                destination.Length = startIndex1 - position;
                position += destination.Length;
                SizeF sizeF = this.MeasureString(text, this.GetSystemFont(textInfo));
                destination.Bounds = new RectangleF(x1, textInfo.Bounds.Y, sizeF.Width, sizeF.Height);
                textInfo.CopyTo(destination);
                collection.Add(destination);
                empty += text;
                x1 += sizeF.Width;
              }
            }
          }
          else
            break;
        }
      }
      else
      {
        empty += textInfo.Text;
        collection.Add(textInfo);
        x1 += textInfo.Width;
      }
    }
    textInfoCollection.Clear();
    textInfoCollection.AddRange((IEnumerable<TextInfoImpl>) collection);
    usedWidth -= this.RemoveWhiteSpaces(textInfoCollection);
    int num = empty.TrimEnd(' ').Length - empty.Replace(" ", "").Length;
    float x2 = (shapeWidth - usedWidth) / (float) num;
    for (int index = 0; index < textInfoCollection.Count; ++index)
    {
      TextInfoImpl textInfo = textInfoCollection[index];
      textInfo.Y += maxAscent - textInfo.Ascent;
      RendererBase.UpdateLineJustifyPosition(textInfoCollection, x2, index);
    }
  }

  private float RemoveWhiteSpaces(List<TextInfoImpl> textInfoCollection)
  {
    float num = 0.0f;
    if (textInfoCollection.Count == 0)
      return num;
    TextInfoImpl textInfo = textInfoCollection[textInfoCollection.Count - 1];
    string empty = string.Empty;
    for (int index = textInfo.Text.Length - 1; index >= 0; --index)
    {
      char ch = textInfo.Text[index];
      if (ch == ' ')
        empty += ch.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      else
        break;
    }
    if (!empty.Equals(string.Empty))
      num = this.MeasureString(empty, this.GetSystemFont(textInfo)).Width;
    return num;
  }

  private static void UpdateLineJustifyPosition(
    List<TextInfoImpl> textInfoCollection,
    float x,
    int index)
  {
    if (index == 0)
      return;
    if (textInfoCollection[index - 1].Text.EndsWith(" "))
      textInfoCollection[index].X = textInfoCollection[index - 1].Bounds.Right + x;
    else
      textInfoCollection[index].X = textInfoCollection[index - 1].Bounds.Right;
  }

  private void UpdateXPosition(IEnumerable<TextInfoImpl> textInfoCollection, float x)
  {
    foreach (TextInfoImpl textInfo in textInfoCollection)
      textInfo.X += x;
  }

  internal Font GetSystemFont(TextInfoImpl textInfo)
  {
    Font systemFont = this._workBookImpl.GetSystemFont(textInfo.Font, string.IsNullOrEmpty(textInfo.UnicodeFont) ? textInfo.Font.FontName : textInfo.UnicodeFont);
    if (systemFont.Name != textInfo.Font.FontName && systemFont.Name == "Microsoft Sans Serif")
      systemFont = this._workBookImpl.GetSystemFont(textInfo.Font, this._workBookImpl.StandardFont);
    return systemFont;
  }

  internal Font GetSystemFont(IFont font, string fontName)
  {
    Font systemFont = this._workBookImpl.GetSystemFont(font, fontName);
    if (systemFont.Name != font.FontName && systemFont.Name == "Microsoft Sans Serif")
      systemFont = this._workBookImpl.GetSystemFont(font, this._workBookImpl.StandardFont);
    return systemFont;
  }
}
