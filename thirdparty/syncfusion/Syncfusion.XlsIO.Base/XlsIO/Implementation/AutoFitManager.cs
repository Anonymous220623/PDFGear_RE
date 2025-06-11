// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.AutoFitManager
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class AutoFitManager : IDisposable
{
  private const int DEF_AUTO_FILTER_WIDTH = 8;
  private const double DEF_AUTO_FILTER_FONT_SIZE = 1.363;
  private const string DROPDOWNSYMBOL = "AA";
  private Graphics m_graphics;
  private RangeImpl m_rangeImpl;
  private WorksheetImpl m_worksheet;
  private WorkbookImpl m_book;
  private int m_row;
  private int m_column;
  private int m_lastRow;
  private int m_lastColumn;
  private ApplicationImpl m_application;

  internal AutoFitManager(int row, int column, int lastRow, int lastColumn, RangeImpl rangeImpl)
  {
    this.m_row = row;
    this.m_column = column;
    this.m_lastRow = lastRow;
    this.m_lastColumn = lastColumn;
    this.m_rangeImpl = rangeImpl;
    this.m_worksheet = rangeImpl.Worksheet as WorksheetImpl;
    this.m_book = rangeImpl.Workbook;
    this.m_application = this.m_worksheet.AppImplementation;
    using (Image image = (Image) new Bitmap(1, 1))
      this.m_graphics = Graphics.FromImage(image);
  }

  internal AutoFitManager(ApplicationImpl appImpl)
  {
    this.m_application = appImpl;
    using (Image image = (Image) new Bitmap(1, 1))
      this.m_graphics = Graphics.FromImage(image);
  }

  internal AutoFitManager(WorksheetImpl worksheet)
  {
    this.m_worksheet = worksheet;
    this.m_application = worksheet.AppImplementation;
    using (Image image = (Image) new Bitmap(1, 1))
      this.m_graphics = Graphics.FromImage(image);
  }

  internal IWorksheet Worksheet => (IWorksheet) this.m_worksheet;

  internal void MeasureToFitColumn()
  {
    int num1 = 14;
    int row = this.m_row;
    int lastRow = this.m_lastRow;
    int column = this.m_column;
    int lastColumn = this.m_lastColumn;
    bool flag1 = false;
    Dictionary<int, IList<object>> dictionary1 = new Dictionary<int, IList<object>>();
    Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
    RectangleF rectF = new RectangleF(0.0f, 0.0f, 1800f, 100f);
    IRange filterRange = this.m_worksheet.AutoFilters.FilterRange;
    WorksheetImpl worksheet = this.m_worksheet;
    IMigrantRange migrantRange = this.m_worksheet.MigrantRange;
    if (worksheet.ListObjects.Count > 0)
    {
      IListObject listObject = this.Worksheet.ListObjects[0];
      IRange location = listObject.Location;
      if (location.Row >= row && location.LastRow <= lastRow && location.Column <= column && location.LastColumn >= lastColumn && listObject.ShowTotals)
        num1 += 6;
    }
    if (worksheet.ListObjects.Count > 0)
    {
      foreach (IListObject listObject in (IEnumerable<IListObject>) worksheet.ListObjects)
      {
        IRange location = listObject.Location;
        if (location.Row >= row && location.LastRow <= lastRow && location.Column <= column && location.LastColumn >= lastColumn)
          flag1 = true;
      }
    }
    for (int index1 = row; index1 <= lastRow; ++index1)
    {
      for (int index2 = column; index2 <= lastColumn; ++index2)
      {
        ExtendedFormatsCollection innerExtFormats = this.m_book.InnerExtFormats;
        ushort xfIndex = (ushort) this.m_worksheet.GetXFIndex(index1, index2);
        int numberFormatIndex = this.m_book.GetExtFormat((int) xfIndex).NumberFormatIndex;
        ExtendedFormatImpl format = innerExtFormats[(int) xfIndex];
        FontImpl innerFont = this.m_book.InnerFonts[format.FontIndex] as FontImpl;
        FormatImpl innerFormat = this.m_book.InnerFormats[numberFormatIndex];
        migrantRange.ResetRowColumn(index1, index2);
        MigrantRangeImpl migrantRangeImpl = migrantRange as MigrantRangeImpl;
        IStyle cellStyle = this.m_worksheet.InnerGetCellStyle(index2, index1, (int) xfIndex, migrantRange as RangeImpl);
        int num4 = 0;
        bool bAutofitText = migrantRangeImpl.m_bAutofitText;
        if (!RangeImpl.IsMergedCell(worksheet.MergeCells, index1, index2, false, ref num4) || num4 != 0)
        {
          if (!bAutofitText)
            migrantRangeImpl.m_bAutofitText = true;
          if (!dictionary2.ContainsKey(index2))
            dictionary2.Add(index2, 0);
          string str = migrantRangeImpl.DisplayText;
          switch (str)
          {
            case null:
            case "":
              continue;
            default:
              if (flag1 && worksheet.ListObjects.Count > 0)
              {
                foreach (IListObject listObject in (IEnumerable<IListObject>) worksheet.ListObjects)
                {
                  IRange location = listObject.Location;
                  if (location.Row == index1 && location.LastRow <= lastRow && location.Column <= index2 && location.LastColumn >= index2)
                    str = $"{str}{"AA"}";
                }
              }
              migrantRange.ResetRowColumn(index1, index2);
              bool flag2 = cellStyle.WrapText || migrantRange.WrapText;
              if (!flag2)
                str = str.Replace("\n", string.Empty);
              if ((cellStyle.Rotation == 0 || cellStyle.Rotation == (int) byte.MaxValue) && !flag2)
              {
                IList<object> list = dictionary1.ContainsKey(index2) ? dictionary1[index2] : (IList<object>) null;
                if (list == null)
                {
                  list = (IList<object>) new List<object>();
                  dictionary1.Add(index2, list);
                }
                ExcelHAlign horizontalAlignment = cellStyle.HorizontalAlignment;
                if (horizontalAlignment == ExcelHAlign.HAlignCenterAcrossSelection)
                {
                  IRange range = this.m_rangeImpl.Worksheet[index1, index2++];
                  if (index2 != range.Column + 1)
                    continue;
                }
                if (filterRange != null && filterRange.Row == index1 && index2 >= filterRange.Column && index2 <= filterRange.LastColumn)
                {
                  AutoFitManager.SortTextToFit(list, innerFont, str, true, horizontalAlignment);
                  break;
                }
                AutoFitManager.SortTextToFit(list, innerFont, str, false, horizontalAlignment);
                break;
              }
              if (flag2)
              {
                int columnWidthInPixels = this.m_worksheet.GetColumnWidthInPixels(index2);
                double num2 = ApplicationImpl.ConvertFromPixel((double) this.CalculateWrappedCell(format, str, columnWidthInPixels, this.m_application), MeasureUnits.Point);
                RowStorage rowStorage = (migrantRange as RangeImpl).RowStorage;
                int num3 = rowStorage != null ? (int) rowStorage.Height / 20 : 0;
                bool flag3 = rowStorage != null && rowStorage.IsBadFontHeight;
                string[] strArray1 = str.Split('\n');
                string[] strArray2 = strArray1[strArray1.Length - 1].Split(' ');
                string[] strArray3 = new string[strArray1.Length - 1 + strArray2.Length];
                for (int index3 = 0; index3 < strArray1.Length - 1; ++index3)
                  strArray3[index3] = strArray1[index3] + "\n";
                int index4 = strArray1.Length - 1;
                for (int index5 = 0; index5 < strArray2.Length; ++index5)
                {
                  strArray3[index4] = strArray2[index5];
                  ++index4;
                }
                int num5 = 0;
                for (int index6 = 0; index6 < strArray3.Length; ++index6)
                {
                  string strText = strArray3[index6].ToString();
                  if (strText.Length > 0)
                  {
                    int num6 = this.MeasureCharacterRanges(cellStyle, strText, num1, rectF);
                    WorksheetImpl.TRangeValueType cellType = this.m_worksheet.GetCellType(migrantRangeImpl.Row, migrantRangeImpl.Column, true);
                    bool flag4 = cellType == WorksheetImpl.TRangeValueType.Number || cellType == (WorksheetImpl.TRangeValueType.Number | WorksheetImpl.TRangeValueType.Formula);
                    if (num6 < columnWidthInPixels || flag4)
                    {
                      for (int index7 = index6 + 1; index7 < strArray3.Length || index7 == 1; ++index7)
                      {
                        index6 = index7;
                        if (strArray3.Length != 1)
                        {
                          if (!strText.EndsWith("\n"))
                            strText = $"{strText}{" "}{strArray3[index7]}";
                          else
                            --index6;
                        }
                        int num7 = this.MeasureCharacterRanges(cellStyle, strText, num1, rectF);
                        if (strArray1.Length == 1 && num7 > num5 && flag3 && (double) num3 >= this.m_worksheet.StandardHeight && (double) num3 <= num2)
                          num5 = num7;
                        else if (num7 < columnWidthInPixels || flag4)
                        {
                          if (num7 > num5)
                          {
                            num5 = num7;
                            index7 = strArray3.Length;
                          }
                        }
                        else
                        {
                          index6 = index7 - 1;
                          num5 = columnWidthInPixels;
                          index7 = strArray3.Length;
                        }
                      }
                    }
                    else if (cellStyle.Rotation == 0 || cellStyle.Rotation == (int) byte.MaxValue)
                    {
                      num5 = strArray1.Length != 1 || num6 <= num5 || !flag3 || (double) num3 < this.m_worksheet.StandardHeight || (double) num3 > num2 ? columnWidthInPixels : num6;
                      index6 = strArray3.Length;
                    }
                    else if (num6 > num5)
                      num5 = num6;
                  }
                  if (num5 > dictionary2[index2])
                    dictionary2[index2] = num5;
                }
                break;
              }
              int num8 = this.MeasureCharacterRanges(cellStyle, str, num1, rectF);
              if ((dictionary2.ContainsKey(index2) ? dictionary2[index2] : 0) < num8)
              {
                dictionary2[index2] = num8;
                break;
              }
              break;
          }
        }
        migrantRangeImpl.m_bAutofitText = false;
      }
    }
    IDictionaryEnumerator enumerator1 = (IDictionaryEnumerator) dictionary1.GetEnumerator();
    while (enumerator1.MoveNext())
    {
      IList<object> objectList = (IList<object>) enumerator1.Value;
      int key = (int) enumerator1.Key;
      int num9 = 0;
      for (int index = 0; index < objectList.Count; ++index)
      {
        int num10 = this.MeasureCharacterRanges((AutoFitManager.StyleWithText) objectList[index], num1, rectF, key);
        if (num9 < num10)
          num9 = num10;
      }
      int num11 = dictionary2.ContainsKey(key) ? dictionary2[key] : 0;
      if (num9 > num11)
        dictionary2[key] = num9;
    }
    IDictionaryEnumerator enumerator2 = (IDictionaryEnumerator) dictionary2.GetEnumerator();
    while (enumerator2.MoveNext())
    {
      int num12 = (int) enumerator2.Value;
      if (num12 != 0)
        worksheet.SetColumnWidthInPixels((int) enumerator2.Key, num12, true);
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
    RectangleF rectF,
    int column)
  {
    int num1 = 0;
    using (Font font = this.CreateFont(styleWithText.fontName, (float) styleWithText.size, styleWithText.style))
    {
      int num2 = 0;
      double indentLevel = this.GetIndentLevel(column);
      for (int index1 = 0; index1 < styleWithText.strValues.Count; ++index1)
      {
        string text = styleWithText.strValues[index1];
        double num3 = indentLevel;
        if (this.m_rangeImpl != null && num3 > 0.0)
        {
          if (num3 < 10.0)
          {
            num2 = (int) num3 * 9;
            text += "0";
          }
          else
          {
            double num4 = text.Length >= (int) byte.MaxValue - text.Length ? ((double) text.Length >= (double) byte.MaxValue - num3 ? (double) (text.Length * 2) - num3 - 9.0 : num3 * 2.55 + 9.0) : num3 * 2.55;
            for (int index2 = 1; index2 <= (int) num4; ++index2)
              text = " " + text;
          }
        }
        int num5 = (int) ((double) this.MeasureString(text, font, rectF, false).Width + 0.05) + paramNum;
        if (num2 > 0)
          num5 += num2 - paramNum;
        if (this.Worksheet.AutoFilters.Count > 0 && this.Worksheet[this.Worksheet.AutoFilters.FilterRange.Row, column].Value == text && !styleWithText.fontSizeIncreased)
          num5 += 8;
        if (num5 > 100)
          ++num5;
        if (num1 < num5)
          num1 = num5;
      }
    }
    return num1;
  }

  private double GetIndentLevel(int column)
  {
    if (this.m_rangeImpl.IsSingleCell)
      return (double) this.m_rangeImpl.IndentLevel;
    double indentLevel = (double) this.m_rangeImpl.IndentLevel;
    MigrantRangeImpl migrantRangeImpl = new MigrantRangeImpl(this.m_worksheet.Application, (IWorksheet) this.m_worksheet);
    int firstRow = this.m_rangeImpl.FirstRow;
    int lastRow = this.m_rangeImpl.LastRow;
    for (int iRow = firstRow; iRow <= lastRow; ++iRow)
    {
      migrantRangeImpl.ResetRowColumn(iRow, column);
      if (indentLevel < (double) migrantRangeImpl.CellStyle.IndentLevel)
        indentLevel = (double) migrantRangeImpl.CellStyle.IndentLevel;
    }
    return indentLevel;
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

  internal RectangleF MeasureString(string text, Font font, RectangleF rectF, bool isAutoFitRow)
  {
    short num = 32000;
    string text1 = text;
    bool flag = false;
    if (text.Length > (int) num)
    {
      text1 = text.Substring(0, (int) num);
      flag = true;
    }
    StringFormat stringFormat1 = this.CreateStringFormat(text1, isAutoFitRow);
    RectangleF bounds1 = this.m_graphics.MeasureCharacterRanges(text1, font, rectF, stringFormat1)[0].GetBounds(this.m_graphics);
    if (flag && (double) bounds1.Width < (double) rectF.Width && (double) bounds1.Height < (double) rectF.Height)
    {
      string text2 = text.Substring((int) num, (text.Length > (int) short.MaxValue ? 32766 : text.Length - 1) - (int) num);
      StringFormat stringFormat2 = this.CreateStringFormat(text2, isAutoFitRow);
      RectangleF bounds2 = this.m_graphics.MeasureCharacterRanges(text2, font, rectF, stringFormat2)[0].GetBounds(this.m_graphics);
      if (isAutoFitRow)
        bounds1.Height += bounds2.Height;
      else
        bounds1.Width += bounds2.Width;
    }
    if ((double) bounds1.Width > 1790.0)
      bounds1.Width = 1790f;
    if ((double) bounds1.Height > 546.0)
      bounds1.Height = 546f;
    return bounds1;
  }

  private int MeasureCharacterRanges(IStyle style, string strText, int num, RectangleF rectF)
  {
    FontStyle fontStyle = FontStyle.Regular;
    double size = 10.0;
    string fontName = "Arial";
    IFont font1 = style.Font;
    if (font1 != null)
    {
      if (font1.Bold)
        fontStyle |= FontStyle.Bold;
      if (font1.Italic)
        fontStyle |= FontStyle.Italic;
      if (font1.Strikethrough)
        fontStyle |= FontStyle.Strikeout;
      if (font1.Underline != ExcelUnderline.None)
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
    bool AutoFilter,
    ExcelHAlign alignment)
  {
    FontStyle fontStyle = FontStyle.Regular;
    double num = 10.0;
    string str = "Arial";
    IFont font = (IFont) fontImpl;
    if (font != null)
    {
      if (font.Bold)
        fontStyle |= FontStyle.Bold;
      if (font.Italic)
        fontStyle |= FontStyle.Italic;
      if (font.Strikethrough)
        fontStyle |= FontStyle.Strikeout;
      if (font.Underline != ExcelUnderline.None)
        fontStyle |= FontStyle.Underline;
      str = font.FontName;
      num = font.Size;
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
    AutoFitManager.StyleWithText styleWithText1 = new AutoFitManager.StyleWithText();
    styleWithText1.fontName = str;
    if (AutoFilter && alignment != ExcelHAlign.HAlignLeft && alignment != ExcelHAlign.HAlignGeneral)
    {
      styleWithText1.size = num + 1.363;
      styleWithText1.fontSizeIncreased = true;
    }
    else
      styleWithText1.size = num;
    styleWithText1.style = fontStyle;
    styleWithText1.strValues.Add(strText);
    list.Add((object) styleWithText1);
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
        IFont font = format.Font;
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
    IFont font1 = format.Font;
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
    if (font1.Underline != ExcelUnderline.None)
      fontStyle |= FontStyle.Underline;
    if (isString && font1.FontName == "Times New Roman")
      stringValue = this.ModifySepicalChar(stringValue);
    using (Font font2 = this.CreateFont(font1.FontName, (float) size, fontStyle))
    {
      float x = 0.0f;
      float y = 0.0f;
      float height1 = 600f;
      if (!format.WrapText && !format.IsVerticalText)
        columnWidth = 600f;
      else if ((double) columnWidth < 100.0)
      {
        switch (format.HorizontalAlignment)
        {
          case ExcelHAlign.HAlignLeft:
          case ExcelHAlign.HAlignRight:
            --columnWidth;
            break;
        }
      }
      else
        columnWidth -= 2f;
      RectangleF rectF = new RectangleF(x, y, columnWidth, height1);
      RectangleF rectangleF = this.MeasureString(stringValue, font2, rectF, true);
      int num1 = font1.Size > 10.0 ? (int) ((double) rectangleF.Height * 1.1 + 0.5) : (int) Math.Ceiling((double) rectangleF.Height);
      if (font1.Size >= 20.0 || num1 > 100)
        ++num1;
      if (format.WrapText || format.IsVerticalText)
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
        {
          stringBuilder.Append("0");
          if (index + 1 < num2)
            stringBuilder.Append("\n");
        }
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

  private int CalculateFontHeightFromGraphics(IFont font)
  {
    double size = font.Size;
    FontStyle fontStyle = FontStyle.Regular;
    if (font.Bold)
      fontStyle |= FontStyle.Bold;
    if (font.Italic)
      fontStyle |= FontStyle.Italic;
    if (font.Strikethrough)
      fontStyle |= FontStyle.Strikeout;
    if (font.Underline != ExcelUnderline.None)
      fontStyle |= FontStyle.Underline;
    using (Font font1 = this.CreateFont(font.FontName, (float) size, fontStyle))
    {
      int heightFromGraphics = (int) Math.Ceiling((double) this.GetFontHeight(font1));
      if (font.Size >= 20.0 || heightFromGraphics > 100 || font.Size == 12.0 && font.Bold)
        ++heightFromGraphics;
      if (font.Size == 8.0)
        heightFromGraphics += 2;
      else if (font.Size < 10.0)
        ++heightFromGraphics;
      return heightFromGraphics;
    }
  }

  private int CalculateFontHeight(IFont font)
  {
    double size = font.Size;
    FontStyle fontStyle = FontStyle.Regular;
    if (font.Bold)
      fontStyle |= FontStyle.Bold;
    if (font.Italic)
      fontStyle |= FontStyle.Italic;
    if (font.Strikethrough)
      fontStyle |= FontStyle.Strikeout;
    if (font.Underline != ExcelUnderline.None)
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
    internal bool fontSizeIncreased;

    internal StyleWithText() => this.strValues = new List<string>();
  }
}
