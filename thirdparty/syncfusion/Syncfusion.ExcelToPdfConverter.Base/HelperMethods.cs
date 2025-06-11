// Decompiled with JetBrains decompiler
// Type: Syncfusion.ExcelToPdfConverter.HelperMethods
// Assembly: Syncfusion.ExcelToPDFConverter.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 4304B189-CB46-46CF-B5C1-2287263DCC93
// Assembly location: C:\Program Files\PDFgear\Syncfusion.ExcelToPdfConverter.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.XlsIO;
using Syncfusion.XlsIO.Implementation;

#nullable disable
namespace Syncfusion.ExcelToPdfConverter;

internal class HelperMethods
{
  private readonly PageSetupOption _pageSetupOption;
  private readonly PdfUnitConvertor _unitConverter = new PdfUnitConvertor();
  private double[] _arrayValue = new double[2];
  private float _colTitle;
  private float _rowTitle;

  internal HelperMethods(PageSetupOption pageSetUp) => this._pageSetupOption = pageSetUp;

  internal int[] RowBreaker(
    int sheetFirstRow,
    int sheetLastRow,
    float sheetHeight,
    ItemSizeHelper rowHeightGetter,
    out int rowStartIndex,
    ExcelToPdfConverterSettings settings)
  {
    int[] numArray = new int[1048576 /*0x100000*/];
    numArray[0] = sheetFirstRow;
    rowStartIndex = 1;
    float num = 0.0f;
    bool flag1 = false;
    bool flag2 = settings.LayoutOptions == LayoutOptions.CustomScaling && !this._pageSetupOption.PageSetup.IsFitToPage;
    if (flag2)
      this._arrayValue = this.CalculateFontValue(settings);
    for (int itemIndex = sheetFirstRow; itemIndex <= sheetLastRow; ++itemIndex)
    {
      if (flag2)
        num += this._unitConverter.ConvertFromPixels(rowHeightGetter.GetSize(itemIndex), PdfGraphicsUnit.Point) * (float) this._arrayValue[1];
      else
        num += this._unitConverter.ConvertFromPixels(rowHeightGetter.GetSize(itemIndex), PdfGraphicsUnit.Point);
      if ((double) num >= (double) sheetHeight)
      {
        flag1 = false;
        numArray[rowStartIndex] = itemIndex - 1;
        ++rowStartIndex;
        num = 0.0f;
        --itemIndex;
      }
      else if (this._pageSetupOption.HorizontalBreaks.Contains(itemIndex) && (settings.LayoutOptions != LayoutOptions.CustomScaling || !this._pageSetupOption.PageSetup.IsFitToPage || this._pageSetupOption.FitPagesTall <= 1 && this._pageSetupOption.FitPagesWide <= 1) && itemIndex < sheetLastRow)
      {
        flag1 = false;
        numArray[rowStartIndex] = itemIndex;
        ++rowStartIndex;
        num = 0.0f;
      }
      if (!this._pageSetupOption.CheckRowBounds(numArray[rowStartIndex - 1], sheetLastRow) && this._pageSetupOption.HasPrintTitleRows && !flag1 && sheetLastRow > this._pageSetupOption.PrintTitleLastRow)
      {
        if (this._pageSetupOption.PrintTitleLastRow < numArray[rowStartIndex - 1])
        {
          if (flag2)
            num += this._pageSetupOption.TitleRowHeight * (float) this._arrayValue[1];
          else
            num += this._pageSetupOption.TitleRowHeight;
        }
        else if ((double) this._pageSetupOption.TitleRowHeight < (double) sheetHeight)
        {
          sheetFirstRow = (numArray[rowStartIndex - 1] += this._pageSetupOption.PrintTitleLastRow - numArray[rowStartIndex - 1] + 1);
          itemIndex = sheetFirstRow - 1;
          num = this._pageSetupOption.TitleRowHeight;
        }
        else
        {
          for (++itemIndex; itemIndex < sheetLastRow; ++itemIndex)
          {
            if ((!flag2 ? (double) this._unitConverter.ConvertFromPixels(rowHeightGetter.GetSize(itemIndex), PdfGraphicsUnit.Point) : (double) (this._unitConverter.ConvertFromPixels(rowHeightGetter.GetSize(itemIndex), PdfGraphicsUnit.Point) * (float) this._arrayValue[1])) > 0.0)
            {
              numArray[rowStartIndex] += numArray[rowStartIndex - 1] + 1;
              this._pageSetupOption.RowIndexes.Add(rowStartIndex);
              ++rowStartIndex;
            }
          }
        }
        this._pageSetupOption.RowIndexes.Add(rowStartIndex);
        flag1 = true;
      }
    }
    return numArray;
  }

  internal int[] ColumnBreaker(
    int sheetFirstColumn,
    int sheetLastColumn,
    float sheetWidth,
    ItemSizeHelper columnWidthGetter,
    out int columnStartIndex,
    ExcelToPdfConverterSettings settings)
  {
    int[] numArray = new int[16384 /*0x4000*/];
    float num1 = 0.0f;
    numArray[0] = sheetFirstColumn;
    columnStartIndex = 1;
    bool flag1 = true;
    bool flag2 = settings.LayoutOptions == LayoutOptions.CustomScaling && !this._pageSetupOption.PageSetup.IsFitToPage;
    if (flag2)
      this._arrayValue = this.CalculateFontValue(settings);
    this._pageSetupOption.ColumnIndexes.Clear();
    int num2 = sheetFirstColumn;
    int num3 = 0;
    for (; num2 <= sheetLastColumn; ++num2)
    {
      if (flag1 && this._pageSetupOption.HasPrintTitleColumns && this._pageSetupOption.CheckColumnBounds(num2, sheetLastColumn))
      {
        if (flag2)
          num1 += this._colTitle;
        else
          num1 += this._pageSetupOption.TitleColumnWidth;
        this._pageSetupOption.ColumnIndexes.Add(columnStartIndex);
        if (num2 >= this._pageSetupOption.PrintTitleFirstColumn && num2 <= this._pageSetupOption.PrintTitleLastColumn)
        {
          bool flag3 = false;
          if ((double) num1 >= (double) sheetWidth)
          {
            float num4 = 0.0f;
            for (int titleFirstColumn = this._pageSetupOption.PrintTitleFirstColumn; titleFirstColumn <= this._pageSetupOption.PrintTitleLastColumn; ++titleFirstColumn)
            {
              if (flag2)
                num4 += this._unitConverter.ConvertFromPixels((float) this._pageSetupOption.Worksheet.GetColumnWidthInPixels(titleFirstColumn), PdfGraphicsUnit.Point) * (float) this._arrayValue[0];
              else
                num4 += this._unitConverter.ConvertFromPixels(columnWidthGetter.GetSize(titleFirstColumn), PdfGraphicsUnit.Point);
              if ((double) num4 > (double) sheetWidth)
              {
                if (titleFirstColumn > num2)
                {
                  numArray[columnStartIndex] = titleFirstColumn - 1;
                  flag1 = true;
                  ++columnStartIndex;
                  num1 = 0.0f;
                  num2 = titleFirstColumn - 1;
                  flag3 = true;
                  break;
                }
                break;
              }
            }
          }
          else
          {
            num2 = this._pageSetupOption.PrintTitleLastColumn;
            num1 = 0.0f;
            flag3 = true;
          }
          if (flag3)
            continue;
        }
      }
      if (flag2)
        num1 += this._unitConverter.ConvertFromPixels(columnWidthGetter.GetSize(num2), PdfGraphicsUnit.Point) * (float) this._arrayValue[0];
      else
        num1 += this._unitConverter.ConvertFromPixels(columnWidthGetter.GetSize(num2), PdfGraphicsUnit.Point);
      if ((double) num1 > (double) sheetWidth)
      {
        if (flag2)
          num1 -= this._unitConverter.ConvertFromPixels(columnWidthGetter.GetSize(num2), PdfGraphicsUnit.Point) * (float) this._arrayValue[0];
        else
          num1 -= this._unitConverter.ConvertFromPixels(columnWidthGetter.GetSize(num2), PdfGraphicsUnit.Point);
        if ((int) num1 == 0 || (double) num1 >= (double) sheetWidth || flag1 && numArray[columnStartIndex - 1] == num2 - 1)
        {
          if (num2 < sheetLastColumn)
            numArray[columnStartIndex] = num2;
          else
            continue;
        }
        else if (num2 > sheetFirstColumn)
        {
          numArray[columnStartIndex] = num2 - 1;
          --num2;
        }
        flag1 = true;
        ++columnStartIndex;
        num1 = 0.0f;
        ++num3;
      }
      else if (this._pageSetupOption.VerticalBreaks.Contains(num2) && (settings.LayoutOptions != LayoutOptions.CustomScaling || !this._pageSetupOption.PageSetup.IsFitToPage || this._pageSetupOption.FitPagesTall <= 1 && this._pageSetupOption.FitPagesWide <= 1) && num2 < sheetLastColumn)
      {
        numArray[columnStartIndex] = num2;
        ++columnStartIndex;
        flag1 = true;
        num1 = 0.0f;
      }
      else
        flag1 = false;
    }
    return numArray;
  }

  private void PrintTitlesWidth()
  {
    if (!this._pageSetupOption.HasPrintTitleColumns)
      return;
    this._colTitle = 0.0f;
    for (int titleFirstColumn = this._pageSetupOption.PrintTitleFirstColumn; titleFirstColumn <= this._pageSetupOption.PrintTitleLastColumn; ++titleFirstColumn)
      this._colTitle += this._unitConverter.ConvertFromPixels((float) this._pageSetupOption.Worksheet.GetColumnWidthInPixels(titleFirstColumn), PdfGraphicsUnit.Point) * (float) this._arrayValue[0];
  }

  private void PrintTitlesHeight()
  {
    if (!this._pageSetupOption.HasPrintTitleRows)
      return;
    this._rowTitle = 0.0f;
    for (int printTitleFirstRow = this._pageSetupOption.PrintTitleFirstRow; printTitleFirstRow <= this._pageSetupOption.PrintTitleLastRow; ++printTitleFirstRow)
      this._rowTitle += (float) this._pageSetupOption.Worksheet.GetRowHeight(printTitleFirstRow) * (float) this._arrayValue[1];
  }

  internal double[] CalculateFontValue(ExcelToPdfConverterSettings settings)
  {
    double[] numArray1 = new double[2]{ 1.0, 1.0 };
    double[] numArray2 = new double[2]{ 1.0, 1.0 };
    double[] numArray3 = new double[2];
    double num = (double) this._pageSetupOption.Worksheet.PageSetup.Zoom / 100.0;
    if (settings.LayoutOptions == LayoutOptions.NoScaling)
      num = 1.0;
    return new double[2]
    {
      numArray1[0] * numArray2[0] * num,
      numArray1[1] * numArray2[1] * num
    };
  }

  private double[] GetFontStyleValue(IWorksheet sheet)
  {
    IFont font = sheet.Workbook.Styles["Normal"].Font;
    double num1 = 1.0;
    double num2 = 1.0;
    if (font.FontName == "Times New Roman")
    {
      switch (font.Size)
      {
        case 10.0:
          num1 = 377.0 / 337.0;
          num2 = 143.0 / 140.0;
          break;
        case 11.0:
          num1 = 270.0 / 257.0;
          num2 = 302.0 / 321.0;
          break;
        case 12.0:
          num1 = 682.0 / 675.0;
          num2 = 387.0 / 410.0;
          break;
      }
    }
    else if (!(font.FontName == "Calibri") || font.Size != 10.0)
    {
      if (font.FontName == "Calibri" && font.Size == 12.0)
      {
        num1 = 1.0;
        num2 = 43.0 / 42.0;
      }
      else if (!(font.FontName == "Calibri") || font.Size != 11.0)
      {
        if (!font.FontName.StartsWith("ï\u00BC\u00ADï\u00BC\u00B3") || font.Size != 9.0)
        {
          if (!font.FontName.StartsWith("ï\u00BC\u00ADï\u00BC\u00B3") || font.Size != 10.0)
          {
            if (!font.FontName.StartsWith("ï\u00BC\u00ADï\u00BC\u00B3") || font.Size != 11.0)
            {
              if (!font.FontName.StartsWith("ï\u00BC\u00ADï\u00BC\u00B3") || font.Size != 12.0)
              {
                if (!(font.FontName == "Arial") || font.Size != 9.0)
                {
                  if (font.FontName == "Arial" && font.Size == 10.0)
                  {
                    num1 = 504.0 / 479.0;
                    num2 = 181.0 / 187.0;
                  }
                  else if (!(font.FontName == "Arial") || font.Size != 11.0)
                  {
                    if (font.FontName == "Arial" && font.Size == 8.0)
                    {
                      num1 = 443.0 / 449.0;
                      num2 = 29.0 / 32.0;
                    }
                    else if (font.FontName == "Geneva" && font.Size == 9.0)
                    {
                      num1 = 455.0 / 421.0;
                      num2 = 287.0 / 272.0;
                    }
                    else if (font.FontName == "Verdana" && font.Size == 10.0)
                    {
                      num1 = 305.0 / 289.0;
                      num2 = 293.0 / 289.0;
                    }
                    else if (font.FontName == "Verdana" && font.Size == 8.0)
                    {
                      num1 = 587.0 / 611.0;
                      num2 = 109.0 / 116.0;
                    }
                    else if (font.FontName == "Arial MT" && font.Size == 12.0)
                    {
                      num1 = 240.0 / 241.0;
                      num2 = 237.0 / 241.0;
                    }
                    else if (font.FontName == "Tahoma" && font.Size == 10.0)
                    {
                      num1 = 197.0 / 192.0;
                      num2 = 1.0;
                    }
                    else if (font.FontName == "MS Sans Serif" && font.Size == 8.0)
                    {
                      num1 = 281.0 / 312.0;
                      num2 = 148.0 / 155.0;
                    }
                    else
                    {
                      switch (sheet.PageSetup.PrintQuality)
                      {
                        case 96 /*0x60*/:
                          num1 = 649.0 / 648.0;
                          num2 = 930.0 / 929.0;
                          break;
                        case 100:
                          num1 = 649.0 / 593.0;
                          num2 = 465.0 / 484.0;
                          break;
                        case 120:
                          num1 = 649.0 / 632.0;
                          num2 = 930.0 / 941.0;
                          break;
                        case 200:
                          num1 = 649.0 / 592.0;
                          num2 = 930.0 / 941.0;
                          break;
                        case 600:
                          num1 = 59.0 / 56.0;
                          num2 = 465.0 / 479.0;
                          break;
                        default:
                          num1 = 649.0 / 617.0;
                          num2 = 930.0 / 949.0;
                          break;
                      }
                    }
                  }
                  else
                  {
                    switch (sheet.PageSetup.PrintQuality)
                    {
                      case 96 /*0x60*/:
                        num1 = 649.0 / 648.0;
                        num2 = 310.0 / 327.0;
                        break;
                      case 100:
                        num1 = 649.0 / 676.0;
                        num2 = 465.0 / 511.0;
                        break;
                      case 120:
                        num1 = 1.0;
                        num2 = 31.0 / 32.0;
                        break;
                      case 200:
                        num1 = 649.0 / 636.0;
                        num2 = 465.0 / 484.0;
                        break;
                      case 600:
                        num1 = 649.0 / 636.0;
                        num2 = 930.0 / 967.0;
                        break;
                      default:
                        num1 = 649.0 / 623.0;
                        num2 = 310.0 / 317.0;
                        break;
                    }
                  }
                }
                else
                {
                  switch (sheet.PageSetup.PrintQuality)
                  {
                    case 96 /*0x60*/:
                      num1 = 649.0 / 648.0;
                      num2 = 930.0 / 929.0;
                      break;
                    case 100:
                      num1 = 649.0 / 676.0;
                      num2 = 930.0 / 911.0;
                      break;
                    case 120:
                      num1 = 649.0 / 711.0;
                      num2 = 0.90029041626331074;
                      break;
                    case 200:
                      num1 = 649.0 / 676.0;
                      num2 = 0.93;
                      break;
                    case 600:
                      num1 = 128.0 / 135.0;
                      num2 = 310.0 / 333.0;
                      break;
                    default:
                      num1 = 649.0 / 675.0;
                      num2 = 465.0 / 484.0;
                      break;
                  }
                }
              }
              else
              {
                switch (sheet.PageSetup.PrintQuality)
                {
                  case 96 /*0x60*/:
                    num1 = 649.0 / 648.0;
                    num2 = 930.0 / 929.0;
                    break;
                  case 100:
                    num1 = 649.0 / 601.0;
                    num2 = 93.0 / 92.0;
                    break;
                  case 120:
                    num1 = 1.0;
                    num2 = 31.0 / 32.0;
                    break;
                  case 200:
                    num1 = 649.0 / 636.0;
                    num2 = 186.0 / 199.0;
                    break;
                  case 600:
                    num1 = 649.0 / 648.0;
                    num2 = 0.92721834496510469;
                    break;
                  default:
                    num1 = 649.0 / 648.0;
                    num2 = 186.0 / 197.0;
                    break;
                }
              }
            }
            else
            {
              switch (sheet.PageSetup.PrintQuality)
              {
                case 96 /*0x60*/:
                  num1 = 649.0 / 648.0;
                  num2 = 930.0 / 929.0;
                  break;
                case 100:
                  num1 = 649.0 / 676.0;
                  num2 = 465.0 / 484.0;
                  break;
                case 120:
                  num1 = 649.0 / 721.0;
                  num2 = 155.0 / 166.0;
                  break;
                case 200:
                  num1 = 649.0 / 676.0;
                  num2 = 155.0 / 166.0;
                  break;
                case 600:
                  num1 = 59.0 / 64.0;
                  num2 = 465.0 / 512.0;
                  break;
                default:
                  num1 = 59.0 / 64.0;
                  num2 = 62.0 / 67.0;
                  break;
              }
            }
          }
          else
          {
            switch (sheet.PageSetup.PrintQuality)
            {
              case 96 /*0x60*/:
                num1 = 649.0 / 648.0;
                num2 = 930.0 / 929.0;
                break;
              case 100:
                num1 = 649.0 / 676.0;
                num2 = 930.0 / 911.0;
                break;
              case 120:
                num1 = 649.0 / 632.0;
                num2 = 1.0;
                break;
              case 200:
                num1 = 649.0 / 676.0;
                num2 = 465.0 / 484.0;
                break;
              case 600:
                num1 = 649.0 / 675.0;
                num2 = 310.0 / 333.0;
                break;
              default:
                num1 = 649.0 / 675.0;
                num2 = 465.0 / 484.0;
                break;
            }
          }
        }
        else
        {
          switch (sheet.PageSetup.PrintQuality)
          {
            case 96 /*0x60*/:
              num1 = 649.0 / 648.0;
              num2 = 930.0 / 929.0;
              break;
            case 100:
              num1 = 649.0 / 582.0;
              num2 = 465.0 / 454.0;
              break;
            case 120:
              num1 = 649.0 / 606.0;
              num2 = 310.0 / 323.0;
              break;
            case 200:
              num1 = 649.0 / 625.0;
              num2 = 155.0 / 167.0;
              break;
            case 600:
              num1 = 649.0 / 639.0;
              num2 = 465.0 / 512.0;
              break;
            default:
              num1 = 649.0 / 640.0;
              num2 = 31.0 / 33.0;
              break;
          }
        }
      }
      else
      {
        switch (sheet.PageSetup.PrintQuality)
        {
          case 96 /*0x60*/:
            num1 = 649.0 / 648.0;
            num2 = 155.0 / 163.0;
            break;
          case 100:
            num1 = 649.0 / 676.0;
            num2 = 0.91265947006869474;
            break;
          case 120:
            num1 = 649.0 / 632.0;
            num2 = 310.0 / 337.0;
            break;
          case 200:
            num1 = 649.0 / 592.0;
            num2 = 465.0 / 484.0;
            break;
          case 600:
            num1 = 275.0 / 257.0;
            num2 = 29.0 / 30.0;
            break;
          default:
            num1 = 649.0 / 617.0;
            num2 = 465.0 / 484.0;
            break;
        }
      }
    }
    else
    {
      switch (sheet.PageSetup.PrintQuality)
      {
        case 96 /*0x60*/:
          num1 = 649.0 / 648.0;
          num2 = 310.0 / 329.0;
          break;
        case 100:
          num1 = 649.0 / 676.0;
          num2 = 62.0 / 61.0;
          break;
        case 120:
          num1 = 649.0 / 632.0;
          num2 = 465.0 / 449.0;
          break;
        case 200:
          num1 = 649.0 / 676.0;
          num2 = 93.0 / 89.0;
          break;
        case 600:
          num1 = 649.0 / 675.0;
          num2 = 186.0 / 181.0;
          break;
        default:
          num1 = 649.0 / 675.0;
          num2 = 310.0 / 299.0;
          break;
      }
    }
    return new double[2]{ num1, num2 };
  }
}
