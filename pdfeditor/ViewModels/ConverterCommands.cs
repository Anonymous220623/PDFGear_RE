// Decompiled with JetBrains decompiler
// Type: pdfeditor.ViewModels.ConverterCommands
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using Microsoft.Toolkit.Mvvm.Input;
using pdfeditor.Properties;
using System;
using System.IO;

#nullable disable
namespace pdfeditor.ViewModels;

public class ConverterCommands
{
  private readonly MainViewModel mainViewModel;
  private RelayCommand pdfToWordCmd;
  private RelayCommand pdfToExcelCmd;
  private RelayCommand pdfToImageCmd;
  private RelayCommand pdfToJpegCmd;
  private RelayCommand pdfToPPTCmd;
  private RelayCommand pdfToRtfCmd;
  private RelayCommand pdfToTxtCmd;
  private RelayCommand pdfToHtmlCmd;
  private RelayCommand pdfToXmlCmd;
  private RelayCommand compressPDF;
  private RelayCommand wordToPdfCmd;
  private RelayCommand excelToPdfCmd;
  private RelayCommand pptToPdfCmd;
  private RelayCommand imageToPdfCmd;
  private RelayCommand rtfToPdfCmd;
  private RelayCommand txtToPdfCmd;

  public ConverterCommands(MainViewModel mainViewModel) => this.mainViewModel = mainViewModel;

  public RelayCommand PDFToWordCmd
  {
    get
    {
      return this.pdfToWordCmd ?? (this.pdfToWordCmd = new RelayCommand((Action) (() => this.DoPDFToWord()), (Func<bool>) (() => true)));
    }
  }

  public RelayCommand CompressPDF
  {
    get
    {
      return this.compressPDF ?? (this.compressPDF = new RelayCommand((Action) (() => this.DoPDFCompress()), (Func<bool>) (() => true)));
    }
  }

  public RelayCommand PDFToExcelCmd
  {
    get
    {
      return this.pdfToExcelCmd ?? (this.pdfToExcelCmd = new RelayCommand((Action) (() => this.DoPDFToExcel()), (Func<bool>) (() => true)));
    }
  }

  public RelayCommand PDFToImageCmd
  {
    get
    {
      return this.pdfToImageCmd ?? (this.pdfToImageCmd = new RelayCommand((Action) (() => this.DoPDFToImage()), (Func<bool>) (() => true)));
    }
  }

  public RelayCommand PDFToJpegCmd
  {
    get
    {
      return this.pdfToJpegCmd ?? (this.pdfToJpegCmd = new RelayCommand((Action) (() => this.DoPDFToJpeg()), (Func<bool>) (() => true)));
    }
  }

  public RelayCommand PDFToPPTCmd
  {
    get
    {
      return this.pdfToPPTCmd ?? (this.pdfToPPTCmd = new RelayCommand((Action) (() => this.DoPDFToPPT()), (Func<bool>) (() => true)));
    }
  }

  public RelayCommand PDFToRtfCmd
  {
    get
    {
      return this.pdfToRtfCmd ?? (this.pdfToRtfCmd = new RelayCommand((Action) (() => this.DoPDFToRtf()), (Func<bool>) (() => true)));
    }
  }

  public RelayCommand PDFToTxtCmd
  {
    get
    {
      return this.pdfToTxtCmd ?? (this.pdfToTxtCmd = new RelayCommand((Action) (() => this.DoPDFToTxt()), (Func<bool>) (() => true)));
    }
  }

  public RelayCommand PDFToHtmlCmd
  {
    get
    {
      return this.pdfToHtmlCmd ?? (this.pdfToHtmlCmd = new RelayCommand((Action) (() => this.DoPDFToHtml()), (Func<bool>) (() => true)));
    }
  }

  public RelayCommand PDFToXmlCmd
  {
    get
    {
      return this.pdfToXmlCmd ?? (this.pdfToXmlCmd = new RelayCommand((Action) (() => this.DoPDFToXml()), (Func<bool>) (() => true)));
    }
  }

  public RelayCommand WordToPDFCmd
  {
    get
    {
      return this.wordToPdfCmd ?? (this.wordToPdfCmd = new RelayCommand((Action) (() => this.DoWordToPDF()), (Func<bool>) (() => true)));
    }
  }

  public RelayCommand ExcelToPDFCmd
  {
    get
    {
      return this.excelToPdfCmd ?? (this.excelToPdfCmd = new RelayCommand((Action) (() => this.DoExcelToPDF()), (Func<bool>) (() => true)));
    }
  }

  public RelayCommand PPTToPDFCmd
  {
    get
    {
      return this.pptToPdfCmd ?? (this.pptToPdfCmd = new RelayCommand((Action) (() => this.DoPPTToPDF()), (Func<bool>) (() => true)));
    }
  }

  public RelayCommand ImageToPDFCmd
  {
    get
    {
      return this.imageToPdfCmd ?? (this.imageToPdfCmd = new RelayCommand((Action) (() => this.DoImageToPDF()), (Func<bool>) (() => true)));
    }
  }

  public RelayCommand RtfToPDFCmd
  {
    get
    {
      return this.rtfToPdfCmd ?? (this.rtfToPdfCmd = new RelayCommand((Action) (() => this.DoRtfToPDF()), (Func<bool>) (() => true)));
    }
  }

  public RelayCommand TxtToPDFCmd
  {
    get
    {
      return this.txtToPdfCmd ?? (this.txtToPdfCmd = new RelayCommand((Action) (() => this.DoTxtToPDF()), (Func<bool>) (() => true)));
    }
  }

  public void DoPDFToWord(string from = null)
  {
    if (this.mainViewModel.CanSave)
    {
      int num = (int) ModernMessageBox.Show(Resources.SaveDocBeforeConvertMsg, UtilManager.GetProductName());
    }
    else
      AppManager.OpenPDFConverterFromPdf(ConvFromPDFType.PDFToWord, this.mainViewModel.Password, new string[1]
      {
        this.mainViewModel.DocumentWrapper?.DocumentPath
      });
  }

  public void DoPDFCompress(ConverterCommands.CompressMode? mode = null)
  {
    if (this.mainViewModel.CanSave)
    {
      int num = (int) ModernMessageBox.Show(Resources.SaveDocBeforeCompressMsg, UtilManager.GetProductName());
    }
    else
    {
      if (mode.HasValue)
      {
        try
        {
          string str = Path.Combine(AppDataHelper.LocalCacheFolder, "TmpSetting");
          if (!Directory.Exists(str))
            Directory.CreateDirectory(str);
          string path = Path.Combine(str, "compress");
          if (File.Exists(path))
            File.Delete(path);
          File.WriteAllText(path, ((int) mode.Value).ToString());
        }
        catch
        {
        }
      }
      AppManager.OpenPDFConverterToPdf(ConvToPDFType.CompressPDF, this.mainViewModel.Password, new string[1]
      {
        this.mainViewModel.DocumentWrapper?.DocumentPath
      });
    }
  }

  public void DoPDFToExcel()
  {
    if (this.mainViewModel.CanSave)
    {
      int num = (int) ModernMessageBox.Show(Resources.SaveDocBeforeConvertMsg, UtilManager.GetProductName());
    }
    else
      AppManager.OpenPDFConverterFromPdf(ConvFromPDFType.PDFToExcel, this.mainViewModel.Password, new string[1]
      {
        this.mainViewModel.DocumentWrapper?.DocumentPath
      });
  }

  public void DoPDFToImage()
  {
    if (this.mainViewModel.CanSave)
    {
      int num = (int) ModernMessageBox.Show(Resources.SaveDocBeforeConvertMsg, UtilManager.GetProductName());
    }
    else
      AppManager.OpenPDFConverterFromPdf(ConvFromPDFType.PDFToPng, this.mainViewModel.Password, new string[1]
      {
        this.mainViewModel.DocumentWrapper?.DocumentPath
      });
  }

  public void DoPDFToPPT()
  {
    if (this.mainViewModel.CanSave)
    {
      int num = (int) ModernMessageBox.Show(Resources.SaveDocBeforeConvertMsg, UtilManager.GetProductName());
    }
    else
      AppManager.OpenPDFConverterFromPdf(ConvFromPDFType.PDFToPPT, this.mainViewModel.Password, new string[1]
      {
        this.mainViewModel.DocumentWrapper?.DocumentPath
      });
  }

  public void DoPDFToJpeg()
  {
    if (this.mainViewModel.CanSave)
    {
      int num = (int) ModernMessageBox.Show(Resources.SaveDocBeforeConvertMsg, UtilManager.GetProductName());
    }
    else
      AppManager.OpenPDFConverterFromPdf(ConvFromPDFType.PDFToJpg, this.mainViewModel.Password, new string[1]
      {
        this.mainViewModel.DocumentWrapper?.DocumentPath
      });
  }

  public void DoPDFToRtf()
  {
    if (this.mainViewModel.CanSave)
    {
      int num = (int) ModernMessageBox.Show(Resources.SaveDocBeforeConvertMsg, UtilManager.GetProductName());
    }
    else
      AppManager.OpenPDFConverterFromPdf(ConvFromPDFType.PDFToRTF, this.mainViewModel.Password, new string[1]
      {
        this.mainViewModel.DocumentWrapper?.DocumentPath
      });
  }

  public void DoPDFToTxt()
  {
    if (this.mainViewModel.CanSave)
    {
      int num = (int) ModernMessageBox.Show(Resources.SaveDocBeforeConvertMsg, UtilManager.GetProductName());
    }
    else
      AppManager.OpenPDFConverterFromPdf(ConvFromPDFType.PDFToTxt, this.mainViewModel.Password, new string[1]
      {
        this.mainViewModel.DocumentWrapper?.DocumentPath
      });
  }

  public void DoPDFToHtml()
  {
    if (this.mainViewModel.CanSave)
    {
      int num = (int) ModernMessageBox.Show(Resources.SaveDocBeforeConvertMsg, UtilManager.GetProductName());
    }
    else
      AppManager.OpenPDFConverterFromPdf(ConvFromPDFType.PDFToHtml, this.mainViewModel.Password, new string[1]
      {
        this.mainViewModel.DocumentWrapper?.DocumentPath
      });
  }

  public void DoPDFToXml()
  {
    if (this.mainViewModel.CanSave)
    {
      int num = (int) ModernMessageBox.Show(Resources.SaveDocBeforeConvertMsg, UtilManager.GetProductName());
    }
    else
      AppManager.OpenPDFConverterFromPdf(ConvFromPDFType.PDFToXml, this.mainViewModel.Password, new string[1]
      {
        this.mainViewModel.DocumentWrapper?.DocumentPath
      });
  }

  public void DoWordToPDF()
  {
    AppManager.OpenPDFConverterToPdf(ConvToPDFType.WordToPDF, (string[]) null);
  }

  public void DoExcelToPDF()
  {
    AppManager.OpenPDFConverterToPdf(ConvToPDFType.ExcelToPDF, (string[]) null);
  }

  public void DoPPTToPDF()
  {
    AppManager.OpenPDFConverterToPdf(ConvToPDFType.PPTToPDF, (string[]) null);
  }

  public void DoImageToPDF()
  {
    AppManager.OpenPDFConverterToPdf(ConvToPDFType.ImageToPDF, (string[]) null);
  }

  public void DoRtfToPDF()
  {
    AppManager.OpenPDFConverterToPdf(ConvToPDFType.RtfToPDF, (string[]) null);
  }

  public void DoTxtToPDF()
  {
    AppManager.OpenPDFConverterToPdf(ConvToPDFType.TxtToPDF, (string[]) null);
  }

  public enum CompressMode
  {
    High,
    Medium,
    Low,
  }
}
