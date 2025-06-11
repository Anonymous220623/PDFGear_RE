// Decompiled with JetBrains decompiler
// Type: Syncfusion.ExcelToPdfConverter.ExcelToPdfLayoutSetting
// Assembly: Syncfusion.ExcelToPDFConverter.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 4304B189-CB46-46CF-B5C1-2287263DCC93
// Assembly location: C:\Program Files\PDFgear\Syncfusion.ExcelToPdfConverter.Base.dll

using Syncfusion.Pdf;
using Syncfusion.XlsIO;
using Syncfusion.XlsIO.Implementation;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.ExcelToPdfConverter;

internal class ExcelToPdfLayoutSetting
{
  private readonly float _bottomMargin;
  private readonly Syncfusion.ExcelToPdfConverter.ExcelToPdfConverter _excelToPdfConverter;
  private readonly float _leftMargin;
  private readonly float _rightMargin;
  private readonly float _topMargin;

  internal ExcelToPdfLayoutSetting(Syncfusion.ExcelToPdfConverter.ExcelToPdfConverter excelToPdfConverter)
  {
    this._excelToPdfConverter = excelToPdfConverter;
    this._bottomMargin = excelToPdfConverter.BottomMargin;
    this._topMargin = excelToPdfConverter.TopMargin;
    this._leftMargin = excelToPdfConverter.LeftMargin;
    this._rightMargin = excelToPdfConverter.RightMargin;
  }

  internal Syncfusion.ExcelToPdfConverter.ExcelToPdfConverter ExcelToPdf
  {
    get => this._excelToPdfConverter;
  }

  internal void FitSheetOnPage(
    PdfSection pdfSection,
    IPageSetup sheetPageSetup,
    float usedRangeWidth,
    float usedRangeHeight)
  {
    PdfPageSettings pageSettings = pdfSection.PageSettings;
    pageSettings.Size = ExcelToPdfConverterSettings.GetExcelSheetSize(sheetPageSetup.PaperSize, this.ExcelToPdf.ExcelToPdfSettings);
    pageSettings.Orientation = (PdfPageOrientation) Enum.Parse(typeof (PdfPageOrientation), sheetPageSetup.Orientation.ToString(), true);
    this.ExcelToPdf.SheetHeight = usedRangeHeight;
    this.ExcelToPdf.SheetWidth = usedRangeWidth;
  }

  internal void FitAllColumnOnOnePage(
    PdfSection pdfSection,
    IPageSetup sheetPageSetup,
    float usedRangeWidth,
    float usedRangeHeight)
  {
    PdfPageSettings pageSettings = pdfSection.PageSettings;
    pageSettings.Size = ExcelToPdfConverterSettings.GetExcelSheetSize(sheetPageSetup.PaperSize, this.ExcelToPdf.ExcelToPdfSettings);
    pageSettings.Orientation = (PdfPageOrientation) Enum.Parse(typeof (PdfPageOrientation), sheetPageSetup.Orientation.ToString(), true);
    float shWidth = usedRangeWidth + this.ExcelToPdf.ExcelToPdfPagesetup.TitleColumnWidth;
    float num;
    if ((double) shWidth < (double) pageSettings.Size.Width - ((double) this._leftMargin + (double) this._rightMargin))
    {
      shWidth = pageSettings.Size.Width - (this._leftMargin + this._rightMargin);
      num = pageSettings.Size.Height - (this._topMargin + this._bottomMargin);
    }
    else
      num = this.ExcelToPdf.RequiredHeight(pageSettings.Size.Width - (this._leftMargin + this._rightMargin), shWidth, pageSettings.Size.Height - ((double) this.ExcelToPdf.ExcelToPdfPagesetup.TitleColumnWidth > 0.0 ? 0.0f : this._topMargin + this._bottomMargin));
    this.ExcelToPdf.SheetHeight = num;
    this.ExcelToPdf.SheetWidth = shWidth;
  }

  internal void FitAllRowsOnOnePage(
    PdfSection pdfSection,
    IPageSetup sheetPageSetup,
    float usedRangeWidth,
    float usedRangeHeight)
  {
    PdfPageSettings pageSettings = pdfSection.PageSettings;
    pageSettings.Size = ExcelToPdfConverterSettings.GetExcelSheetSize(sheetPageSetup.PaperSize, this.ExcelToPdf.ExcelToPdfSettings);
    pageSettings.Orientation = (PdfPageOrientation) Enum.Parse(typeof (PdfPageOrientation), sheetPageSetup.Orientation.ToString(), true);
    float shHeight = usedRangeHeight + this.ExcelToPdf.ExcelToPdfPagesetup.TitleRowHeight;
    float num;
    if ((double) shHeight < (double) pageSettings.Size.Height - ((double) this._topMargin + (double) this._bottomMargin))
    {
      shHeight = pageSettings.Size.Height - (this._topMargin + this._bottomMargin);
      num = pageSettings.Size.Width - (this._leftMargin + this._rightMargin);
    }
    else
      num = this.ExcelToPdf.RequiredWidth(pageSettings.Size.Width - ((double) this.ExcelToPdf.ExcelToPdfPagesetup.TitleRowHeight > 0.0 ? 0.0f : this._leftMargin + this._rightMargin), shHeight, pageSettings.Size.Height - (this._topMargin + this._bottomMargin));
    this.ExcelToPdf.SheetHeight = shHeight;
    this.ExcelToPdf.SheetWidth = num;
  }

  internal void NoScaling(
    PdfSection pdfSection,
    IPageSetup sheetPageSetup,
    float usedRangeWidth,
    float usedRangeHeight)
  {
    PdfPageSettings pageSettings = pdfSection.PageSettings;
    pageSettings.Size = ExcelToPdfConverterSettings.GetExcelSheetSize(sheetPageSetup.PaperSize, this.ExcelToPdf.ExcelToPdfSettings);
    pageSettings.Orientation = (PdfPageOrientation) Enum.Parse(typeof (PdfPageOrientation), sheetPageSetup.Orientation.ToString(), true);
    this.ExcelToPdf.SheetHeight = pageSettings.Height - (this._topMargin + this._bottomMargin);
    this.ExcelToPdf.SheetWidth = pageSettings.Width - (this._leftMargin + this._rightMargin);
  }

  internal void CustomScaling(
    PdfSection pdfSection,
    IPageSetup sheetPageSetup,
    float usedRangeWidth,
    float usedRangeHeight,
    IRange[] printAreas)
  {
    PdfPageSettings pageSettings = pdfSection.PageSettings;
    pageSettings.Size = ExcelToPdfConverterSettings.GetExcelSheetSize(sheetPageSetup.PaperSize, this.ExcelToPdf.ExcelToPdfSettings);
    pageSettings.Orientation = (PdfPageOrientation) Enum.Parse(typeof (PdfPageOrientation), sheetPageSetup.Orientation.ToString(), true);
    PageSetupBaseImpl pageSetupBaseImpl = sheetPageSetup as PageSetupBaseImpl;
    SizeF sizeF = new SizeF(pageSettings.Width, pageSettings.Height);
    float num1;
    float num2;
    if (pageSetupBaseImpl.IsFitToPage)
    {
      float num3 = this._topMargin + this._bottomMargin;
      float num4 = this._leftMargin + this._rightMargin;
      float shWidth = pageSettings.Width - num4;
      float shHeight = pageSettings.Height - num3;
      if (pageSetupBaseImpl.FitToPagesTall == 1 && pageSetupBaseImpl.FitToPagesWide == 0)
      {
        if ((double) shHeight < (double) usedRangeHeight)
          shHeight = usedRangeHeight;
        num1 = this.ExcelToPdf.RequiredWidth(pageSettings.Size.Width, shHeight + num3, pageSettings.Size.Height) + 3f;
        num2 = shHeight + 3f;
      }
      else if (pageSetupBaseImpl.FitToPagesTall == 0 && pageSetupBaseImpl.FitToPagesWide == 1)
      {
        if ((double) shWidth < (double) usedRangeWidth)
          shWidth = usedRangeWidth;
        num2 = this.ExcelToPdf.RequiredHeight(pageSettings.Size.Width - num4, shWidth, pageSettings.Size.Height - num3) + 3f;
        num1 = shWidth + 3f;
      }
      else
      {
        int num5 = pageSetupBaseImpl.FitToPagesWide == 0 ? 1 : pageSetupBaseImpl.FitToPagesWide;
        int num6 = pageSetupBaseImpl.FitToPagesTall == 0 ? 1 : pageSetupBaseImpl.FitToPagesTall;
        int num7 = num5.CompareTo(num6);
        if (num7 == 0)
        {
          if ((double) (shWidth * (float) num5) < (double) usedRangeWidth)
            shWidth = (usedRangeWidth + num4 * (float) num5) / (float) num5;
          shHeight = this.ExcelToPdf.RequiredHeight(pageSettings.Size.Width, shWidth, pageSettings.Size.Height);
          if ((double) (shHeight * (float) num6) < (double) usedRangeHeight + (double) num3)
          {
            shHeight = (usedRangeHeight + num3 * (float) num6) / (float) num6;
            shWidth = this.ExcelToPdf.RequiredWidth(pageSettings.Size.Width, shHeight, pageSettings.Size.Height);
          }
        }
        else if (num7 > 0)
        {
          if ((double) (shHeight * (float) num6) < (double) usedRangeHeight + (double) num3)
            shHeight = (usedRangeHeight + num3 * (float) num6) / (float) num6;
          float num8 = this.ExcelToPdf.RequiredWidth(pageSettings.Size.Width, shHeight, pageSettings.Size.Height) * (float) num5;
          if ((double) num8 < (double) usedRangeWidth + (double) num4)
          {
            shWidth = (usedRangeWidth + num4 * (float) num5) / (float) num5;
            shHeight = this.ExcelToPdf.RequiredHeight(pageSettings.Size.Width, shWidth, pageSettings.Size.Height);
          }
          else
            shWidth = num8 / (float) num5;
        }
        else
        {
          if ((double) (shWidth * (float) num5) < (double) usedRangeWidth + (double) num4)
            shWidth = (usedRangeWidth + num4 * (float) num5) / (float) num5;
          float num9 = this.ExcelToPdf.RequiredHeight(pageSettings.Size.Width, shWidth, pageSettings.Size.Height) * (float) num6;
          if ((double) num9 < (double) usedRangeHeight + (double) num3)
          {
            shHeight = (usedRangeHeight + num3 * (float) num6) / (float) num6;
            shWidth = this.ExcelToPdf.RequiredWidth(pageSettings.Size.Width, shHeight, pageSettings.Size.Height);
          }
          else
            shHeight = num9 / (float) num6;
          if (num5 == 1 && ((double) usedRangeHeight + (double) num3 < (double) pageSettings.Height || (double) usedRangeWidth + (double) num4 <= (double) pageSettings.Width))
            shHeight = pageSettings.Height;
        }
        num2 = (float) ((double) shHeight - (double) num3 + 3.0);
        num1 = (float) ((double) shWidth - (double) num4 + 3.0);
      }
    }
    else
    {
      float num10 = pageSettings.Height - (this._topMargin + this._bottomMargin);
      float num11 = pageSettings.Width - (this._leftMargin + this._rightMargin);
      num2 = num10 + 1.5f;
      num1 = num11 + 1.5f;
    }
    this.ExcelToPdf.SheetHeight = num2;
    this.ExcelToPdf.SheetWidth = num1;
  }
}
