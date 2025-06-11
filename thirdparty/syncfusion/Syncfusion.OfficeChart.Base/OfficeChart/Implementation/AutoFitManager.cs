// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.AutoFitManager
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class AutoFitManager : IDisposable
{
  private const double DEF_AUTO_FILTER_WIDTH = 1.363;
  private const string DROPDOWNSYMBOL = "AA";
  private Graphics m_graphics;
  private RangeImpl m_rangeImpl;
  private WorksheetImpl m_worksheet;
  private WorkbookImpl m_book;
  private int m_row;
  private int m_column;
  private int m_lastRow;
  private int m_lastColumn;

  internal AutoFitManager(int row, int column, int lastRow, int lastColumn, RangeImpl rangeImpl)
  {
    this.m_row = row;
    this.m_column = column;
    this.m_lastRow = lastRow;
    this.m_lastColumn = lastColumn;
    this.m_rangeImpl = rangeImpl;
    this.m_worksheet = rangeImpl.Worksheet as WorksheetImpl;
    this.m_book = rangeImpl.Workbook;
    using (Bitmap bitmap = new Bitmap(100, 2000))
      this.m_graphics = Graphics.FromImage((Image) bitmap);
  }

  internal AutoFitManager()
  {
    using (Bitmap bitmap = new Bitmap(100, 2000))
      this.m_graphics = Graphics.FromImage((Image) bitmap);
  }

  internal IWorksheet Worksheet => (IWorksheet) this.m_worksheet;

  internal void MeasureToFitColumn()
  {
    int num1 = 14;
    int row = this.m_row;
    int lastRow = this.m_lastRow;
    int column = this.m_column;
    int lastColumn = this.m_lastColumn;
    Dictionary<int, IList<object>> dictionary1 = new Dictionary<int, IList<object>>();
    Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
    RectangleF rectF = new RectangleF(0.0f, 0.0f, 1800f, 100f);
    WorksheetImpl worksheet = this.m_worksheet;
    IMigrantRange migrantRange = this.m_worksheet.MigrantRange;
    for (int index1 = row; index1 <= lastRow; ++index1)
    {
      for (int index2 = column; index2 <= lastColumn; ++index2)
      {
        ushort xfIndex = (ushort) this.m_worksheet.GetXFIndex(index1, index2);
        int numberFormatIndex = this.m_book.GetExtFormat((int) xfIndex).NumberFormatIndex;
        IOfficeFont innerFont = this.m_book.InnerFonts[this.m_book.InnerExtFormats[(int) xfIndex].FontIndex];
        FormatImpl innerFormat = this.m_book.InnerFormats[numberFormatIndex];
        IStyle cellStyle = this.m_worksheet.InnerGetCellStyle(index2, index1, (int) xfIndex, this.m_rangeImpl[index1, index2] as RangeImpl);
        int num4 = 0;
        if (!RangeImpl.IsMergedCell(worksheet.MergeCells, index1, index2, false, ref num4) || num4 != 0)
        {
          if (!dictionary2.ContainsKey(index2))
            dictionary2.Add(index2, 0);
          string displayText = this.m_rangeImpl.GetDisplayText(index1, index2, innerFormat);
          switch (displayText)
          {
            case null:
            case "":
              continue;
            default:
              migrantRange.ResetRowColumn(index1, index2);
              bool flag = migrantRange.CellStyle.WrapText || migrantRange.WrapText;
              if ((cellStyle.Rotation == 0 || cellStyle.Rotation == (int) byte.MaxValue) && !flag)
              {
                if ((dictionary1.ContainsKey(index2) ? dictionary1[index2] : (IList<object>) null) == null)
                {
                  IList<object> objectList = (IList<object>) new List<object>();
                  dictionary1.Add(index2, objectList);
                }
                if (cellStyle.HorizontalAlignment == OfficeHAlign.HAlignCenterAcrossSelection)
                {
                  IRange range = this.m_rangeImpl[index1, index2++];
                  if (index2 == range.Column + 1)
                    continue;
                  continue;
                }
                continue;
              }
              if (flag)
              {
                int columnWidthInPixels = this.m_worksheet.GetColumnWidthInPixels(index2);
                string[] strArray = displayText.Split(' ', '\n');
                int num2 = 0;
                for (int index3 = 0; index3 < strArray.Length; ++index3)
                {
                  string strText = strArray[index3].ToString();
                  if (strText.Length > 0)
                  {
                    if (this.MeasureCharacterRanges(cellStyle, strText, num1, rectF) < columnWidthInPixels)
                    {
                      for (int index4 = index3 + 1; index4 < strArray.Length; ++index4)
                      {
                        index3 = index4;
                        strText = $"{strText}{" "}{strArray[index4]}";
                        int num3 = this.MeasureCharacterRanges(cellStyle, strText, num1, rectF);
                        if (num3 < columnWidthInPixels)
                        {
                          if (num3 > num2)
                          {
                            num2 = num3;
                            index4 = strArray.Length;
                          }
                        }
                        else
                        {
                          index3 = index4 - 1;
                          index4 = strArray.Length;
                        }
                      }
                    }
                    else
                    {
                      num2 = columnWidthInPixels;
                      index3 = strArray.Length;
                    }
                  }
                  dictionary2[index2] = num2;
                }
                continue;
              }
              int num5 = this.MeasureCharacterRanges(cellStyle, displayText, num1, rectF);
              if ((dictionary2.ContainsKey(index2) ? dictionary2[index2] : 0) < num5)
              {
                dictionary2[index2] = num5;
                continue;
              }
              continue;
          }
        }
      }
    }
    IDictionaryEnumerator enumerator1 = (IDictionaryEnumerator) dictionary1.GetEnumerator();
    while (enumerator1.MoveNext())
    {
      IList<object> objectList = (IList<object>) enumerator1.Value;
      int key = (int) enumerator1.Key;
      int num6 = 0;
      for (int index = 0; index < objectList.Count; ++index)
      {
        int num7 = this.MeasureCharacterRanges((AutoFitManager.StyleWithText) objectList[index], num1, rectF);
        if (num6 < num7)
          num6 = num7;
      }
      int num8 = dictionary2.ContainsKey(key) ? dictionary2[key] : 0;
      if (num6 > num8)
        dictionary2[key] = num6;
    }
    IDictionaryEnumerator enumerator2 = (IDictionaryEnumerator) dictionary2.GetEnumerator();
    while (enumerator2.MoveNext())
    {
      int num9 = (int) enumerator2.Value;
      if (num9 != 0)
        worksheet.SetColumnWidthInPixels((int) enumerator2.Key, num9);
    }
  }

  private Font CreateFont(string fontName, float size, FontStyle fontStyle)
  {
    try
    {
      return new Font(fontName, size, fontStyle);
    }
    catch
    {
      return new Font(fontName, size);
    }
  }

  private int MeasureCharacterRanges(
    AutoFitManager.StyleWithText styleWithText,
    int paramNum,
    RectangleF rectF)
  {
    int num1 = 0;
    using (Font font = this.CreateFont(styleWithText.fontName, (float) styleWithText.size, styleWithText.style))
    {
      for (int index = 0; index < styleWithText.strValues.Count; ++index)
      {
        int num2 = (int) ((double) this.MeasureString(styleWithText.strValues[index], font, rectF, false).Width + 0.05) + paramNum;
        if (num2 > 100)
          ++num2;
        if (num1 < num2)
          num1 = num2;
      }
    }
    return num1;
  }

  private StringFormat CreateStringFormat(string text, bool isAutoFitRow)
  {
    StringFormat stringFormat;
    if (isAutoFitRow)
    {
      CharacterRange[] ranges = new CharacterRange[1]
      {
        new CharacterRange(0, text.Length)
      };
      stringFormat = new StringFormat();
      stringFormat.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
      stringFormat.Trimming = StringTrimming.None;
      stringFormat.SetMeasurableCharacterRanges(ranges);
    }
    else
    {
      CharacterRange[] ranges = new CharacterRange[1]
      {
        new CharacterRange(0, text.Length)
      };
      stringFormat = new StringFormat();
      stringFormat.SetMeasurableCharacterRanges(ranges);
    }
    return stringFormat;
  }

  private RectangleF MeasureString(string text, Font font, RectangleF rectF, bool isAutoFitRow)
  {
    StringFormat stringFormat = this.CreateStringFormat(text, isAutoFitRow);
    return this.m_graphics.MeasureCharacterRanges(text, font, rectF, stringFormat)[0].GetBounds(this.m_graphics);
  }

  private int MeasureCharacterRanges(IStyle style, string strText, int num, RectangleF rectF)
  {
    FontStyle fontStyle = FontStyle.Regular;
    double size = 10.0;
    string fontName = "Arial";
    IOfficeFont font1 = style.Font;
    if (font1 != null)
    {
      if (font1.Bold)
        fontStyle |= FontStyle.Bold;
      if (font1.Italic)
        fontStyle |= FontStyle.Italic;
      if (font1.Strikethrough)
        fontStyle |= FontStyle.Strikeout;
      if (font1.Underline != OfficeUnderline.None)
        fontStyle |= FontStyle.Underline;
      fontName = font1.FontName;
      size = font1.Size;
    }
    Font font2;
    try
    {
      font2 = this.CreateFont(fontName, (float) size, fontStyle);
    }
    catch
    {
      font2 = this.CreateFont(fontName, (float) size, fontStyle);
    }
    if (style.Rotation == 90)
      return (int) ((double) this.GetFontHeight(font2) * 1.1 + 0.5) + 6;
    RectangleF rectangleF = this.MeasureString(strText, font2, rectF, false);
    if (style.Rotation == 0 || style.Rotation == (int) byte.MaxValue)
    {
      int num1 = (int) ((double) rectangleF.Width + 0.5) + num;
      if (num1 > 100)
        ++num1;
      return num1;
    }
    int num2 = (int) ((double) rectangleF.Width + 0.5) + num;
    int num3 = (int) ((double) this.GetFontHeight(font2) * 1.1 + 0.5);
    double num4 = Math.PI * (double) Math.Abs(style.Rotation) / 180.0;
    font2.Dispose();
    return (int) ((double) num2 * Math.Cos(num4) + (double) num3 * Math.Sin(num4) + 6.5);
  }

  private static void SortTextToFit(
    IList<object> list,
    FontImpl fontImpl,
    string strText,
    bool AutoFilter)
  {
    FontStyle fontStyle = FontStyle.Regular;
    double num = 10.0;
    string str = "Arial";
    IOfficeFont officeFont = (IOfficeFont) fontImpl;
    if (officeFont != null)
    {
      if (officeFont.Bold)
        fontStyle |= FontStyle.Bold;
      if (officeFont.Italic)
        fontStyle |= FontStyle.Italic;
      if (officeFont.Strikethrough)
        fontStyle |= FontStyle.Strikeout;
      if (officeFont.Underline != OfficeUnderline.None)
        fontStyle |= FontStyle.Underline;
      str = officeFont.FontName;
      num = officeFont.Size;
    }
    for (int index1 = 0; index1 < list.Count; ++index1)
    {
      AutoFitManager.StyleWithText styleWithText = (AutoFitManager.StyleWithText) list[index1];
      if (styleWithText.fontName == str && styleWithText.size == num && styleWithText.style == fontStyle)
      {
        for (int index2 = 0; index2 < styleWithText.strValues.Count; ++index2)
        {
          if (styleWithText.strValues[index2].Length < strText.Length)
          {
            styleWithText.strValues.Insert(index2, strText);
            if (styleWithText.strValues.Count <= 5)
              return;
            styleWithText.strValues.RemoveAt(5);
            return;
          }
        }
        if (styleWithText.strValues.Count >= 5)
          return;
        styleWithText.strValues.Add(strText);
        return;
      }
    }
    list.Add((object) new AutoFitManager.StyleWithText()
    {
      fontName = str,
      size = (!AutoFilter ? num : num + 1.363),
      style = fontStyle,
      strValues = {
        strText
      }
    });
  }

  internal int CalculateWrappedCell(
    ExtendedFormatImpl format,
    string stringValue,
    int columnWidth,
    ApplicationImpl applicationImpl)
  {
    int num1 = 19;
    switch (stringValue)
    {
      case null:
      case "":
        return 0;
      default:
        IOfficeFont font = format.Font;
        int num2 = (int) ((double) (stringValue.Length / 406) * font.Size + (double) (2 * Convert.ToInt32(font.Bold || font.Italic)));
        int columnWidth1 = num2 < columnWidth ? columnWidth : num2;
        return this.MeasureCell(format, stringValue, (float) columnWidth1, num1, true);
    }
  }

  private int MeasureCell(
    ExtendedFormatImpl format,
    string stringValue,
    float columnWidth,
    int num,
    bool isString)
  {
    IOfficeFont font1 = format.Font;
    double size = font1.Size;
    FontStyle fontStyle = FontStyle.Regular;
    if (stringValue[stringValue.Length - 1] == '\n')
      stringValue += "a";
    if (font1.Bold)
      fontStyle |= FontStyle.Bold;
    if (font1.Italic)
      fontStyle |= FontStyle.Italic;
    if (font1.Strikethrough)
      fontStyle |= FontStyle.Strikeout;
    if (font1.Underline != OfficeUnderline.None)
      fontStyle |= FontStyle.Underline;
    if (isString && font1.FontName == "Times New Roman")
      stringValue = this.ModifySepicalChar(stringValue);
    using (Font font2 = this.CreateFont(font1.FontName, (float) size, fontStyle))
    {
      float x = 0.0f;
      float y = 0.0f;
      float height1 = 600f;
      if (!format.WrapText)
        columnWidth = 600f;
      else if ((double) columnWidth < 100.0)
      {
        switch (format.HorizontalAlignment)
        {
          case OfficeHAlign.HAlignLeft:
          case OfficeHAlign.HAlignRight:
            --columnWidth;
            break;
        }
      }
      else
        columnWidth -= 2f;
      RectangleF rectF = new RectangleF(x, y, columnWidth, height1);
      RectangleF rectangleF = this.MeasureString(stringValue, font2, rectF, true);
      int num1 = (int) ((double) rectangleF.Height * 1.1 + 0.5);
      if (font1.Size >= 20.0 || num1 > 100)
        ++num1;
      if (format.WrapText)
      {
        if (size >= 10.0)
          return num1;
        int fontHeight = this.CalculateFontHeight(font1);
        float height2 = rectangleF.Height;
        if ((double) height2 > 100.0)
          ++height2;
        int num2 = (int) Math.Ceiling((double) height2 * 1.0 / (double) fontHeight);
        if ((double) height2 > 100.0)
          num2 = (int) ((double) height2 * 1.0 / (double) fontHeight) + 1;
        if (num2 == 1)
          return this.CalculateFontHeightFromGraphics(font1);
        StringBuilder stringBuilder = new StringBuilder();
        for (int index = 0; index < num2; ++index)
          stringBuilder.Append("\n0");
        return this.MeasureFontSize(format, stringBuilder.ToString(), columnWidth);
      }
      int num3 = Math.Abs(format.Rotation);
      if (num3 == 90)
        return (int) ((double) rectangleF.Width + 0.5) + num;
      int num4 = (int) ((double) rectangleF.Width + 0.5) + num;
      int num5 = (int) ((double) this.GetFontHeight(font2) * 1.1 + 0.5);
      return (int) ((double) num4 * Math.Sin(Math.PI * (double) num3 / 180.0) + (double) num5 * Math.Cos(Math.PI * (double) num3 / 180.0) + 6.5);
    }
  }

  private int CalculateFontHeightFromGraphics(IOfficeFont font)
  {
    double size = font.Size;
    FontStyle fontStyle = FontStyle.Regular;
    if (font.Bold)
      fontStyle |= FontStyle.Bold;
    if (font.Italic)
      fontStyle |= FontStyle.Italic;
    if (font.Strikethrough)
      fontStyle |= FontStyle.Strikeout;
    if (font.Underline != OfficeUnderline.None)
      fontStyle |= FontStyle.Underline;
    using (Font font1 = this.CreateFont(font.FontName, (float) size, fontStyle))
    {
      int heightFromGraphics = (int) ((double) this.GetFontHeight(font1) * 1.1 + 0.5);
      if (font.Size >= 20.0 || heightFromGraphics > 100 || font.Size == 12.0 && font.Bold)
        ++heightFromGraphics;
      if (font.Size == 8.0)
        heightFromGraphics += 2;
      else if (font.Size < 10.0)
        ++heightFromGraphics;
      return heightFromGraphics;
    }
  }

  private int CalculateFontHeight(IOfficeFont font)
  {
    double size = font.Size;
    FontStyle fontStyle = FontStyle.Regular;
    if (font.Bold)
      fontStyle |= FontStyle.Bold;
    if (font.Italic)
      fontStyle |= FontStyle.Italic;
    if (font.Strikethrough)
      fontStyle |= FontStyle.Strikeout;
    if (font.Underline != OfficeUnderline.None)
      fontStyle |= FontStyle.Underline;
    using (Font font1 = this.CreateFont(font.FontName, (float) size, fontStyle))
      return (int) Math.Ceiling((double) this.GetFontHeight(font1));
  }

  private float GetFontHeight(Font font) => font.GetHeight(this.m_graphics);

  private int MeasureFontSize(
    ExtendedFormatImpl extendedFromat,
    string stringValue,
    float columnWidth)
  {
    if (stringValue == "")
      return 0;
    double size = extendedFromat.Font.Size;
    string fontName;
    if (((fontName = extendedFromat.Font.FontName) == null || fontName != "Calibri") && size < 10.0)
    {
      size = (double) (int) (size * 1.1 + 0.5);
      if (size > 10.0)
        size = 10.0;
    }
    using (Font font = this.CreateFont(extendedFromat.Font.FontName, (float) size, FontStyle.Regular))
    {
      float x = 0.0f;
      float y = 0.0f;
      float height = 600f;
      if (!extendedFromat.WrapText)
        columnWidth = 600f;
      RectangleF rectF = new RectangleF(x, y, columnWidth, height);
      return (int) ((double) this.MeasureString(stringValue, font, rectF, true).Height * 1.1 + 0.5);
    }
  }

  private string ModifySepicalChar(string stringValue)
  {
    StringBuilder stringBuilder = new StringBuilder();
    char[] charArray = stringValue.ToCharArray();
    for (int index = 0; index < stringValue.Length; ++index)
    {
      switch (charArray[index])
      {
        case ' ':
          if (index != 0)
          {
            switch (charArray[index - 1])
            {
              case '%':
              case '&':
                stringBuilder.Append(charArray[index]);
                break;
            }
          }
          stringBuilder.Append(charArray[index]);
          break;
        case '/':
          stringBuilder.Append('W');
          break;
        default:
          stringBuilder.Append(charArray[index]);
          break;
      }
    }
    return stringBuilder.ToString();
  }

  public void Dispose()
  {
    this.m_graphics.Dispose();
    this.m_worksheet = (WorksheetImpl) null;
    this.m_rangeImpl = (RangeImpl) null;
    this.m_book = (WorkbookImpl) null;
  }

  private class StyleWithText
  {
    internal string fontName;
    internal double size;
    internal FontStyle style;
    internal List<string> strValues;

    internal StyleWithText() => this.strValues = new List<string>();
  }
}
