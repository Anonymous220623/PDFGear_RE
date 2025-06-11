// Decompiled with JetBrains decompiler
// Type: pdfconverter.ConvertManager
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using CommomLib.Commom;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using pdfconverter.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

#nullable disable
namespace pdfconverter;

internal class ConvertManager
{
  public static readonly Dictionary<OCRLanguageID, string> OCRLanguages = new Dictionary<OCRLanguageID, string>(31 /*0x1F*/)
  {
    {
      OCRLanguageID.Arabic,
      "Arabic"
    },
    {
      OCRLanguageID.Bulgarian,
      "Bulgarian"
    },
    {
      OCRLanguageID.Catalan,
      "Catalan"
    },
    {
      OCRLanguageID.ChineseSimplified,
      "Chinese_Simplified"
    },
    {
      OCRLanguageID.ChineseTraditional,
      "Chinese_Traditional"
    },
    {
      OCRLanguageID.Croatian,
      "Croatian"
    },
    {
      OCRLanguageID.Czech,
      "Czech"
    },
    {
      OCRLanguageID.Danish,
      "Danish"
    },
    {
      OCRLanguageID.Dutch,
      "Dutch"
    },
    {
      OCRLanguageID.English,
      "English"
    },
    {
      OCRLanguageID.Estonian,
      "Estonian"
    },
    {
      OCRLanguageID.Finish,
      "Finish"
    },
    {
      OCRLanguageID.French,
      "French"
    },
    {
      OCRLanguageID.German,
      "German"
    },
    {
      OCRLanguageID.Hungarian,
      "Hungarian"
    },
    {
      OCRLanguageID.Indonesian,
      "Indonesian"
    },
    {
      OCRLanguageID.Italian,
      "Italian"
    },
    {
      OCRLanguageID.Japanese,
      "Japanese"
    },
    {
      OCRLanguageID.Korean,
      "Korean"
    },
    {
      OCRLanguageID.Latvian,
      "Latvian"
    },
    {
      OCRLanguageID.Lithuanian,
      "Lithuanian"
    },
    {
      OCRLanguageID.Norwegian,
      "Norwegian"
    },
    {
      OCRLanguageID.Polish,
      "Polish"
    },
    {
      OCRLanguageID.Portuguese,
      "Portuguese"
    },
    {
      OCRLanguageID.Romanian,
      "Romanian"
    },
    {
      OCRLanguageID.Russian,
      "Russian"
    },
    {
      OCRLanguageID.Slovak,
      "Slovak"
    },
    {
      OCRLanguageID.Slovenian,
      "Slovenian"
    },
    {
      OCRLanguageID.Spanish,
      "Spanish"
    },
    {
      OCRLanguageID.Swedish,
      "Swedish"
    },
    {
      OCRLanguageID.Turkish,
      "Turkish"
    }
  };
  public static readonly Dictionary<OCRLanguageID, string> OCROnlineLanguages = new Dictionary<OCRLanguageID, string>(26)
  {
    {
      OCRLanguageID.Bulgarian,
      "BG_BG"
    },
    {
      OCRLanguageID.ChineseSimplified,
      "ZH_CN"
    },
    {
      OCRLanguageID.ChineseTraditional,
      "ZH_HK"
    },
    {
      OCRLanguageID.Croatian,
      "HR_HR"
    },
    {
      OCRLanguageID.Czech,
      "CS_CZ"
    },
    {
      OCRLanguageID.Danish,
      "DA_DK"
    },
    {
      OCRLanguageID.English,
      "EN_US"
    },
    {
      OCRLanguageID.Estonian,
      "ET_EE"
    },
    {
      OCRLanguageID.Finish,
      "FI_FI"
    },
    {
      OCRLanguageID.French,
      "FR_FR"
    },
    {
      OCRLanguageID.German,
      "DE_DE"
    },
    {
      OCRLanguageID.Hungarian,
      "HU_HU"
    },
    {
      OCRLanguageID.Italian,
      "IT_IT"
    },
    {
      OCRLanguageID.Japanese,
      "JA_JP"
    },
    {
      OCRLanguageID.Korean,
      "KO_KR"
    },
    {
      OCRLanguageID.Latvian,
      "LV_LV"
    },
    {
      OCRLanguageID.Lithuanian,
      "LT_LT"
    },
    {
      OCRLanguageID.Norwegian,
      "NO_NO"
    },
    {
      OCRLanguageID.Polish,
      "PL_PL"
    },
    {
      OCRLanguageID.Romanian,
      "RO_RO"
    },
    {
      OCRLanguageID.Russian,
      "RU_RU"
    },
    {
      OCRLanguageID.Slovak,
      "SK_SK"
    },
    {
      OCRLanguageID.Slovenian,
      "SL_SI"
    },
    {
      OCRLanguageID.Spanish,
      "ES_ES"
    },
    {
      OCRLanguageID.Swedish,
      "SV_SE"
    },
    {
      OCRLanguageID.Turkish,
      "TR_TR"
    }
  };
  public static readonly Dictionary<OCRLanguageID, string> OCRLanguagesL10n = new Dictionary<OCRLanguageID, string>(31 /*0x1F*/)
  {
    {
      OCRLanguageID.Arabic,
      "عربى"
    },
    {
      OCRLanguageID.Bulgarian,
      "български"
    },
    {
      OCRLanguageID.Catalan,
      "Català"
    },
    {
      OCRLanguageID.ChineseSimplified,
      "中文 (简体)"
    },
    {
      OCRLanguageID.ChineseTraditional,
      "中文 (繁體)"
    },
    {
      OCRLanguageID.Croatian,
      "Hrvatski"
    },
    {
      OCRLanguageID.Czech,
      "čeština"
    },
    {
      OCRLanguageID.Danish,
      "dansk"
    },
    {
      OCRLanguageID.Dutch,
      "Nederlands"
    },
    {
      OCRLanguageID.English,
      "English"
    },
    {
      OCRLanguageID.Estonian,
      "Eesti"
    },
    {
      OCRLanguageID.Finish,
      "Suomi"
    },
    {
      OCRLanguageID.French,
      "français"
    },
    {
      OCRLanguageID.German,
      "Deutsch"
    },
    {
      OCRLanguageID.Hungarian,
      "Magyar"
    },
    {
      OCRLanguageID.Indonesian,
      "bahasa Indonesia"
    },
    {
      OCRLanguageID.Italian,
      "Italiano"
    },
    {
      OCRLanguageID.Japanese,
      "日本語"
    },
    {
      OCRLanguageID.Korean,
      "한국어"
    },
    {
      OCRLanguageID.Latvian,
      "latviešu"
    },
    {
      OCRLanguageID.Lithuanian,
      "Lietuvių"
    },
    {
      OCRLanguageID.Norwegian,
      "norsk"
    },
    {
      OCRLanguageID.Polish,
      "Polski"
    },
    {
      OCRLanguageID.Portuguese,
      "Português"
    },
    {
      OCRLanguageID.Romanian,
      "Română"
    },
    {
      OCRLanguageID.Russian,
      "русский"
    },
    {
      OCRLanguageID.Slovak,
      "Slovenčina"
    },
    {
      OCRLanguageID.Slovenian,
      "Slovenski"
    },
    {
      OCRLanguageID.Spanish,
      "Español"
    },
    {
      OCRLanguageID.Swedish,
      "svenska"
    },
    {
      OCRLanguageID.Turkish,
      "Türkçe"
    }
  };

  public static string selectPDFFile()
  {
    string str = "";
    OpenFileDialog openFileDialog = new OpenFileDialog();
    openFileDialog.Multiselect = false;
    openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
    openFileDialog.Filter = "Portable Document Format(*.pdf)|*.pdf";
    if (openFileDialog.ShowDialog().GetValueOrDefault())
      str = openFileDialog.FileName;
    return str;
  }

  public static string[] selectMultiPDFFiles()
  {
    OpenFileDialog openFileDialog = new OpenFileDialog();
    openFileDialog.Multiselect = true;
    openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
    openFileDialog.Filter = "Portable Document Format(*.pdf)|*.pdf";
    return openFileDialog.ShowDialog().GetValueOrDefault() ? openFileDialog.FileNames : (string[]) null;
  }

  public static string[] selectMultiFiles(string typeName, string extention)
  {
    OpenFileDialog openFileDialog = new OpenFileDialog();
    openFileDialog.Multiselect = true;
    openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
    openFileDialog.Filter = $"{typeName}(*{extention})|*{extention}";
    return openFileDialog.ShowDialog().GetValueOrDefault() ? openFileDialog.FileNames : (string[]) null;
  }

  public static string selectOutputFolder(string defaultPath)
  {
    string str1 = "";
    string str2 = string.IsNullOrWhiteSpace(defaultPath) || !Directory.Exists(defaultPath) ? Environment.GetFolderPath(Environment.SpecialFolder.MyComputer) : defaultPath;
    CommonOpenFileDialog commonOpenFileDialog = new CommonOpenFileDialog();
    commonOpenFileDialog.IsFolderPicker = true;
    commonOpenFileDialog.InitialDirectory = str2;
    if (commonOpenFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
      str1 = commonOpenFileDialog.FileName;
    return str1;
  }

  public static bool IsFileAdded(ObservableCollection<ConvertFileItem> list, string file)
  {
    if (list.Count > 0)
    {
      foreach (ConvertFileItem convertFileItem in (Collection<ConvertFileItem>) list)
      {
        if (convertFileItem.convertFile.Equals(file, StringComparison.CurrentCulture))
          return true;
      }
    }
    return false;
  }

  public static FileCovertType parseConvertType(string typeStr)
  {
    FileCovertType convertType = FileCovertType.Invalid;
    if (typeStr.Equals("pdftoword", StringComparison.CurrentCulture))
      convertType = FileCovertType.PDFToWord;
    else if (typeStr.Equals("pdftortf", StringComparison.CurrentCulture))
      convertType = FileCovertType.PDFToRtf;
    else if (typeStr.Equals("pdftoxls", StringComparison.CurrentCulture))
      convertType = FileCovertType.PDFToXls;
    else if (typeStr.Equals("pdftohtml", StringComparison.CurrentCulture))
      convertType = FileCovertType.PDFToHtml;
    else if (typeStr.Equals("pdftoxml", StringComparison.CurrentCulture))
      convertType = FileCovertType.PDFToXml;
    else if (typeStr.Equals("pdftotext", StringComparison.CurrentCulture))
      convertType = FileCovertType.PDFToText;
    else if (typeStr.Equals("pdftopng", StringComparison.CurrentCulture))
      convertType = FileCovertType.PDFToPng;
    else if (typeStr.Equals("pdftojpeg", StringComparison.CurrentCulture))
      convertType = FileCovertType.PDFToJpeg;
    return convertType;
  }

  public static string getTitle(ConvFromPDFType type)
  {
    string title = "";
    switch (type)
    {
      case ConvFromPDFType.PDFToWord:
        title = Resources.PDFtoWordText;
        break;
      case ConvFromPDFType.PDFToExcel:
        title = Resources.PDFtoExcelText;
        break;
      case ConvFromPDFType.PDFToPng:
        title = Resources.PDFtoPngText;
        break;
      case ConvFromPDFType.PDFToJpg:
        title = Resources.PDFtoJpegText;
        break;
      case ConvFromPDFType.PDFToTxt:
        title = Resources.PDFtoTextText;
        break;
      case ConvFromPDFType.PDFToHtml:
        title = Resources.PDFtoWebText;
        break;
      case ConvFromPDFType.PDFToXml:
        title = Resources.PDFtoXMLText;
        break;
      case ConvFromPDFType.PDFToRTF:
        title = Resources.PDFtoRTFText;
        break;
      case ConvFromPDFType.PDFToPPT:
        title = Resources.PDFtoPPTText;
        break;
    }
    return title;
  }

  public static string getOutputExt(OutputFormat format)
  {
    string outputExt = "";
    switch (format)
    {
      case OutputFormat.Docx:
        outputExt = ".docx";
        break;
      case OutputFormat.Rtf:
        outputExt = ".rtf";
        break;
      case OutputFormat.Xls:
        outputExt = ".xls";
        break;
      case OutputFormat.Html:
        outputExt = ".html";
        break;
      case OutputFormat.Xml:
        outputExt = ".xml";
        break;
      case OutputFormat.Text:
        outputExt = ".txt";
        break;
      case OutputFormat.Png:
        outputExt = ".png";
        break;
      case OutputFormat.Jpeg:
        outputExt = ".jpeg";
        break;
      case OutputFormat.Ppt:
        outputExt = ".pptx";
        break;
    }
    return outputExt;
  }

  public static OCRLanguageID GetOCRLanguageID()
  {
    int key = ConfigManager.GetOCRLanguageID();
    int num = ((IEnumerable<string>) Enum.GetNames(OCRLanguageID.Arabic.GetType())).Count<string>();
    if (key < 0 || key >= num)
      key = 9;
    return !ConvertManager.OCRLanguages.ContainsKey((OCRLanguageID) key) ? OCRLanguageID.English : (OCRLanguageID) key;
  }

  public static string GetOCROnlineLanguage()
  {
    int key = ConfigManager.GetOCRLanguageID();
    int num = ((IEnumerable<string>) Enum.GetNames(OCRLanguageID.Arabic.GetType())).Count<string>();
    if (key < 0 || key >= num)
      key = 9;
    return !ConvertManager.OCROnlineLanguages.ContainsKey((OCRLanguageID) key) ? string.Empty : ConvertManager.OCROnlineLanguages[(OCRLanguageID) key];
  }

  public static string getOCRLanguage()
  {
    OCRLanguageID ocrLanguageId = ConvertManager.GetOCRLanguageID();
    return $"Languages/{ConvertManager.OCRLanguages[ocrLanguageId]}";
  }

  public static string getOCRLanguageL10N()
  {
    OCRLanguageID ocrLanguageId = ConvertManager.GetOCRLanguageID();
    return ConvertManager.OCRLanguagesL10n[ocrLanguageId];
  }
}
