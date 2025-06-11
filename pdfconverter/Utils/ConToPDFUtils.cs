// Decompiled with JetBrains decompiler
// Type: pdfconverter.Utils.ConToPDFUtils
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using CommomLib.Commom;
using pdfconverter.Models;
using pdfconverter.Properties;
using pdfconverter.Views;
using Syncfusion.DocIO.DLS;
using Syncfusion.ExcelToPdfConverter;
using Syncfusion.OfficeChart;
using Syncfusion.OfficeChartToImageConverter;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Presentation;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

#nullable disable
namespace pdfconverter.Utils;

public static class ConToPDFUtils
{
  public static async Task<bool> WordToPDFByRangeAsync(
    string inputFile,
    string outfullName,
    int start,
    int end,
    string password,
    IProgress<double> progress,
    CancellationToken cancellationToken)
  {
    cancellationToken.ThrowIfCancellationRequested();
    progress?.Report(0.0);
    if (string.IsNullOrEmpty(inputFile) || string.IsNullOrEmpty(outfullName))
    {
      progress?.Report(1.0);
      return false;
    }
    try
    {
      await Task.Run(TaskExceptionHelper.ExceptionBoundary((Action) (() =>
      {
        WordDocument wordDocument = new WordDocument();
        string extension = Path.GetExtension(inputFile);
        if (!string.IsNullOrWhiteSpace(extension) && extension.Equals(".doc", StringComparison.CurrentCultureIgnoreCase))
          wordDocument.Settings.SkipIncrementalSaveValidation = true;
        wordDocument.Open(inputFile, Syncfusion.DocIO.FormatType.Automatic, password);
        wordDocument.ChartToImageConverter = (IOfficeChartToImageConverter) new ChartToImageConverter();
        wordDocument.ChartToImageConverter.ScalingMode = Syncfusion.OfficeChart.ScalingMode.Normal;
        int num = end - start + 1;
        for (int index = 1; index < start; ++index)
          wordDocument.ChildEntities.RemoveAt(0);
        for (int index = num; index < end; ++index)
          wordDocument.ChildEntities.RemoveAt(wordDocument.ChildEntities.Count - 1);
        Syncfusion.DocToPDFConverter.DocToPDFConverter docToPdfConverter = new Syncfusion.DocToPDFConverter.DocToPDFConverter();
        PdfDocument pdf = docToPdfConverter.ConvertToPDF(wordDocument);
        pdf.Save(outfullName);
        pdf.Close(true);
        pdf.Dispose();
        wordDocument.Close();
        wordDocument.Dispose();
        docToPdfConverter.Dispose();
        GC.Collect();
      })));
      return true;
    }
    catch (Exception ex) when (!(ex is OperationCanceledException))
    {
    }
    progress?.Report(1.0);
    return false;
  }

  public static async Task<bool> RTFToPDFByRangeAsync(
    string inputFile,
    string outfullName,
    int start,
    int end,
    IProgress<double> progress,
    CancellationToken cancellationToken)
  {
    cancellationToken.ThrowIfCancellationRequested();
    progress?.Report(0.0);
    if (string.IsNullOrEmpty(inputFile))
    {
      progress?.Report(1.0);
      return false;
    }
    try
    {
      await Task.Run(TaskExceptionHelper.ExceptionBoundary((Action) (() =>
      {
        WordDocument wordDocument = new WordDocument(inputFile, Syncfusion.DocIO.FormatType.Txt);
        PdfDocument pdf = new Syncfusion.DocToPDFConverter.DocToPDFConverter()
        {
          Settings = {
            EmbedCompleteFonts = true
          }
        }.ConvertToPDF(wordDocument);
        pdf.Save(outfullName);
        pdf.Close(true);
        wordDocument.Close();
      })));
      return true;
    }
    catch (Exception ex) when (!(ex is OperationCanceledException))
    {
    }
    progress?.Report(1.0);
    return false;
  }

  public static async Task<bool> PPTToPDFByRangeAsync(
    string inputFile,
    string outfullName,
    int start,
    int end,
    string password,
    IProgress<double> progress,
    CancellationToken cancellationToken)
  {
    cancellationToken.ThrowIfCancellationRequested();
    progress?.Report(0.0);
    if (string.IsNullOrEmpty(inputFile))
    {
      progress?.Report(1.0);
      return false;
    }
    try
    {
      await Task.Run(TaskExceptionHelper.ExceptionBoundary((Action) (() =>
      {
        IPresentation presentation = Syncfusion.Presentation.Presentation.Open(inputFile, password);
        presentation.ChartToImageConverter = (IOfficeChartToImageConverter) new ChartToImageConverter();
        PdfDocument pdfDocument = Syncfusion.PresentationToPdfConverter.PresentationToPdfConverter.Convert(presentation);
        pdfDocument.Save(outfullName);
        pdfDocument.Close(true);
        presentation.Close();
      })));
      return true;
    }
    catch (Exception ex) when (!(ex is OperationCanceledException))
    {
    }
    progress?.Report(1.0);
    return false;
  }

  public static async Task<bool> ExcelToPDFByRangeAsync(
    string inputFile,
    string outfullName,
    int start,
    int end,
    string password,
    IProgress<double> progress,
    CancellationToken cancellationToken,
    bool fitWitdh = true)
  {
    cancellationToken.ThrowIfCancellationRequested();
    progress?.Report(0.0);
    if (string.IsNullOrEmpty(inputFile))
    {
      progress?.Report(1.0);
      return false;
    }
    try
    {
      await Task.Run(TaskExceptionHelper.ExceptionBoundary((Action) (() =>
      {
        using (Syncfusion.XlsIO.ExcelEngine excelEngine = new Syncfusion.XlsIO.ExcelEngine())
        {
          Syncfusion.XlsIO.IApplication excel = excelEngine.Excel;
          excel.DefaultVersion = ExcelVersion.Excel2013;
          Syncfusion.XlsIO.IWorkbook workbook = excel.Workbooks.Open(inputFile, ExcelParseOptions.Default, false, password, ExcelOpenType.Automatic);
          foreach (Syncfusion.XlsIO.IWorksheet worksheet in (IEnumerable<Syncfusion.XlsIO.IWorksheet>) workbook.Worksheets)
            worksheet.PageSetup.PrintComments = ExcelPrintLocation.PrintSheetEnd;
          Syncfusion.ExcelToPdfConverter.ExcelToPdfConverter excelToPdfConverter = new Syncfusion.ExcelToPdfConverter.ExcelToPdfConverter(workbook);
          PdfDocument pdfDocument = new PdfDocument();
          ExcelToPdfConverterSettings converterSettings = new ExcelToPdfConverterSettings()
          {
            LayoutOptions = !fitWitdh ? LayoutOptions.NoScaling : LayoutOptions.FitAllColumnsOnOnePage
          };
          excelToPdfConverter.Convert(converterSettings).Save(outfullName);
        }
      })));
      return true;
    }
    catch (Exception ex) when (!(ex is OperationCanceledException))
    {
    }
    progress?.Report(1.0);
    return false;
  }

  public static async Task<bool> ImageToSiglePDFByRangeAsync(
    List<ToPDFFileItem> inputFiles,
    string outfullName,
    PageSizeItem sizeItem,
    PdfMargins margins,
    IProgress<double> progress,
    CancellationToken cancellationToken)
  {
    cancellationToken.ThrowIfCancellationRequested();
    progress?.Report(0.0);
    SizeF size = ParaConvert.GetPdfPagesize(sizeItem);
    bool MapSource = (double) size.Width == 0.0 || (double) size.Height == 0.0;
    bool HadSetPageSetting = false;
    if (inputFiles.Count == 0 || string.IsNullOrEmpty(outfullName))
    {
      progress?.Report(1.0);
      return false;
    }
    try
    {
      await Task.Run(TaskExceptionHelper.ExceptionBoundary((Action) (() =>
      {
        PdfDocument pdfDocument = new PdfDocument();
        foreach (ToPDFFileItem inputFile in inputFiles)
        {
          PdfBitmap image = new PdfBitmap(inputFile.FilePath);
          SizeF sizeF = image.PhysicalDimension;
          float height1 = sizeF.Height;
          sizeF = image.PhysicalDimension;
          float width1 = sizeF.Width;
          if (MapSource)
          {
            PdfSection pdfSection = pdfDocument.Sections.Add();
            pdfSection.PageSettings.Rotate = PdfPageRotateAngle.RotateAngle0;
            pdfSection.PageSettings.Size = new SizeF(width1 + margins.Left + margins.Right, height1 + margins.Top + margins.Bottom);
            pdfSection.PageSettings.Margins = margins;
            pdfSection.Pages.Add().Graphics.DrawImage((PdfImage) image, 0.0f, 0.0f, width1, height1);
          }
          else
          {
            if (!HadSetPageSetting)
            {
              pdfDocument.PageSettings.Margins = margins;
              pdfDocument.PageSettings.Size = size;
              HadSetPageSetting = true;
            }
            PdfPage pdfPage = pdfDocument.Pages.Add();
            PdfGraphics graphics = pdfPage.Graphics;
            sizeF = pdfPage.GetClientSize();
            float width2 = sizeF.Width;
            sizeF = pdfPage.GetClientSize();
            float height2 = sizeF.Height;
            float num = Math.Min(width2 / width1, height2 / height1);
            if ((double) num < 1.0)
            {
              height1 *= num;
              width1 *= num;
            }
            float x = (float) (((double) width2 - (double) width1) / 2.0);
            float y = (float) (((double) height2 - (double) height1) / 2.0);
            graphics.DrawImage((PdfImage) image, x, y, width1, height1);
          }
          image.Dispose();
        }
        pdfDocument.Save(outfullName);
        pdfDocument.Close(true);
      })));
      return true;
    }
    catch (Exception ex) when (!(ex is OperationCanceledException))
    {
    }
    progress?.Report(1.0);
    return false;
  }

  public static async Task<bool> ImageToMultiPDFByRangeAsync(
    ToPDFFileItem item,
    PageSizeItem sizeItem,
    PdfMargins margins,
    IProgress<double> progress,
    CancellationToken cancellationToken)
  {
    cancellationToken.ThrowIfCancellationRequested();
    progress?.Report(0.0);
    SizeF size = ParaConvert.GetPdfPagesize(sizeItem);
    bool MapSource = (double) size.Width == 0.0 || (double) size.Height == 0.0;
    if (!File.Exists(item.FilePath))
    {
      progress?.Report(1.0);
      return false;
    }
    try
    {
      await Task.Run(TaskExceptionHelper.ExceptionBoundary((Action) (() =>
      {
        PdfDocument pdfDocument = new PdfDocument();
        PdfBitmap image = new PdfBitmap(item.FilePath);
        float height1 = image.PhysicalDimension.Height;
        float width1 = image.PhysicalDimension.Width;
        if (MapSource)
        {
          PdfSection pdfSection = pdfDocument.Sections.Add();
          pdfSection.PageSettings.Rotate = PdfPageRotateAngle.RotateAngle0;
          pdfSection.PageSettings.Size = new SizeF(width1 + margins.Left + margins.Right, height1 + margins.Top + margins.Bottom);
          pdfSection.PageSettings.Margins = margins;
          pdfSection.Pages.Add().Graphics.DrawImage((PdfImage) image, 0.0f, 0.0f, width1, height1);
        }
        else
        {
          pdfDocument.PageSettings.Margins = margins;
          pdfDocument.PageSettings.Size = size;
          PdfPage pdfPage = pdfDocument.Pages.Add();
          PdfGraphics graphics = pdfPage.Graphics;
          float width2 = pdfPage.GetClientSize().Width;
          float height2 = pdfPage.GetClientSize().Height;
          float num = Math.Min(width2 / width1, height2 / height1);
          if ((double) num < 1.0)
          {
            height1 *= num;
            width1 *= num;
          }
          float x = (float) (((double) width2 - (double) width1) / 2.0);
          float y = (float) (((double) height2 - (double) height1) / 2.0);
          graphics.DrawImage((PdfImage) image, x, y, width1, height1);
        }
        image.Dispose();
        pdfDocument.Save(item.OutputPath);
        pdfDocument.Close(true);
      })));
      return true;
    }
    catch (Exception ex) when (!(ex is OperationCanceledException))
    {
    }
    progress?.Report(1.0);
    return false;
  }

  public static bool? CheckAccess(string inputeFile)
  {
    try
    {
      FileStream fileStream = new FileStream(inputeFile, FileMode.Open, FileAccess.Read);
      fileStream.Close();
      fileStream.Dispose();
      return new bool?(true);
    }
    catch (Exception ex)
    {
      return ex.HResult == -2147024864 /*0x80070020*/ ? new bool?(false) : new bool?();
    }
  }

  public static bool CheckPassword(string inputeFile, ref string password, Window window)
  {
    bool flag = false;
    string fileName = Path.GetFileName(inputeFile);
    do
    {
      try
      {
        new PdfLoadedDocument(inputeFile, password).Dispose();
        return true;
      }
      catch (Exception ex)
      {
        if (ex.Message == "Can't open an encrypted document. The password is invalid.")
        {
          if (flag)
          {
            int num = (int) ModernMessageBox.Show(Resources.OpenDocByIncorrectPwdMsg, "PDFgear");
          }
          flag = ConToPDFUtils.OnPasswordRequested(window, fileName, out password);
        }
        else
        {
          int num = (int) ModernMessageBox.Show(Resources.WinPwdLoadFailed.Replace("XXX", fileName), "PDFgear");
          return false;
        }
      }
    }
    while (flag);
    return false;
  }

  public static bool CheckWordPassword(string inputeFile, out string password, Window window)
  {
    bool flag = false;
    string fileName = Path.GetFileName(inputeFile);
    password = "";
    do
    {
      try
      {
        new WordDocument(inputeFile, Syncfusion.DocIO.FormatType.Automatic, password).Dispose();
        return true;
      }
      catch (Exception ex)
      {
        if (ex.Message == $"Specified password \"{password}\" is incorrect!")
        {
          if (flag)
          {
            int num = (int) ModernMessageBox.Show(Resources.OpenDocByIncorrectPwdMsg, "PDFgear");
          }
          flag = ConToPDFUtils.OnPasswordRequested(window, fileName, out password);
        }
        else
        {
          int num = (int) ModernMessageBox.Show(Resources.WinPwdLoadFailed.Replace("XXX", fileName), "PDFgear");
          return false;
        }
      }
    }
    while (flag);
    return false;
  }

  public static bool CheckExcelPassword(string inputeFile, out string password, Window window)
  {
    bool flag = false;
    password = "";
    string fileName = Path.GetFileName(inputeFile);
    do
    {
      try
      {
        Syncfusion.XlsIO.ExcelEngine excelEngine = new Syncfusion.XlsIO.ExcelEngine();
        Syncfusion.XlsIO.IApplication excel = excelEngine.Excel;
        excel.DefaultVersion = ExcelVersion.Excel2013;
        excel.Workbooks.Open(inputeFile, ExcelParseOptions.Default, false, password, ExcelOpenType.Automatic);
        excelEngine.Dispose();
        return true;
      }
      catch (Exception ex)
      {
        if (ex.Message.StartsWith("Wrong password"))
        {
          if (flag)
          {
            int num = (int) ModernMessageBox.Show(Resources.OpenDocByIncorrectPwdMsg, "PDFgear");
          }
          flag = ConToPDFUtils.OnPasswordRequested(window, fileName, out password);
        }
        else
        {
          int num = (int) ModernMessageBox.Show(Resources.WinPwdLoadFailed.Replace("XXX", fileName), "PDFgear");
          return false;
        }
      }
    }
    while (flag);
    return false;
  }

  public static bool CheckPPTPassword(string inputeFile, out string password, Window window)
  {
    bool flag = false;
    password = "";
    string fileName = Path.GetFileName(inputeFile);
    do
    {
      try
      {
        Syncfusion.Presentation.Presentation.Open(inputeFile, password).Dispose();
        return true;
      }
      catch (Exception ex)
      {
        if (ex.Message == $"Specified password \"{password}\" is incorrect!")
        {
          if (flag)
          {
            int num = (int) ModernMessageBox.Show(Resources.OpenDocByIncorrectPwdMsg, "PDFgear");
          }
          flag = ConToPDFUtils.OnPasswordRequested(window, fileName, out password);
        }
        else
        {
          int num = (int) ModernMessageBox.Show(Resources.WinPwdLoadFailed.Replace("XXX", fileName), "PDFgear");
          return false;
        }
      }
    }
    while (flag);
    return false;
  }

  private static bool OnPasswordRequested(Window window, string fileName, out string password)
  {
    EnterPasswordDialog enterPasswordDialog = new EnterPasswordDialog(fileName);
    if (window != null && window.IsVisible)
    {
      enterPasswordDialog.Owner = window;
      enterPasswordDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
    }
    else
      enterPasswordDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
    bool? nullable = enterPasswordDialog.ShowDialog();
    password = enterPasswordDialog.Password;
    return nullable.GetValueOrDefault();
  }
}
