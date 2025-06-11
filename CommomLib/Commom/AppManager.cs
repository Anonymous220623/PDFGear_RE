// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.AppManager
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using CommomLib.IAP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

#nullable disable
namespace CommomLib.Commom;

public static class AppManager
{
  private static string[] pdfExtArray = new string[1]
  {
    ".pdf"
  };
  private static Dictionary<string, string[]> supportedFileTypeDict = new Dictionary<string, string[]>()
  {
    {
      "PDF documents",
      AppManager.pdfExtArray
    }
  };

  internal static string GetRootDirectory() => AppDomain.CurrentDomain.BaseDirectory;

  internal static string GetRootDirectoryFile(params string[] filePath)
  {
    if (filePath == null || filePath.Length == 0)
      return AppManager.GetRootDirectory();
    List<string> list = ((IEnumerable<string>) filePath).Select<string, string>((Func<string, string>) (c => c.Trim())).Where<string>((Func<string, bool>) (c => !string.IsNullOrEmpty(c))).ToList<string>();
    if (list.Count == 0)
      return AppManager.GetRootDirectory();
    list.Insert(0, AppManager.GetRootDirectory());
    return Path.Combine(list.ToArray());
  }

  public static void OpenEditor(string fullFileName, string action = null)
  {
    string rootDirectoryFile = AppManager.GetRootDirectoryFile("pdfeditor.exe");
    string str = $"\"{fullFileName}\"";
    if (!string.IsNullOrEmpty(action))
      str = $"{str} -action {action.Trim()}";
    string Parameters = str;
    ProcessManager.RunProcess(rootDirectoryFile, Parameters);
  }

  private static void OpenConverterCore(
    string appTag,
    string type,
    string[] files,
    string password)
  {
    if (string.IsNullOrEmpty(appTag) || string.IsNullOrEmpty(type))
      return;
    string rootDirectoryFile = AppManager.GetRootDirectoryFile("pdfconverter.exe");
    DocsPathUtils.WriteFilesPathJson(type, (object) files, (object) password);
    GAManager.SendEvent("PDFConverterApp", type, "Main", 1L);
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append('"').Append(appTag).Append('"').Append(' ');
    stringBuilder.Append('"').Append(type).Append('"').Append(' ');
    string localJsonPath = DocsPathUtils.GetLocalJsonPath();
    if (!string.IsNullOrEmpty(localJsonPath))
      stringBuilder.Append('"').Append(localJsonPath).Append('"');
    else
      --stringBuilder.Length;
    string Parameters = stringBuilder.ToString();
    ProcessManager.RunProcess(rootDirectoryFile, Parameters);
  }

  public static void OpenPDFConverterFromPdf(ConvFromPDFType type, params string[] files)
  {
    AppManager.OpenPDFConverterFromPdf(type.ToString(), (string) null, files);
  }

  public static void OpenPDFConverterFromPdf(ConvFromPDFType type, string password, string[] files)
  {
    AppManager.OpenPDFConverterFromPdf(type.ToString(), password, files);
  }

  public static void OpenPDFConverterFromPdf(string type, string password, params string[] files)
  {
    if (!IAPHelper.IsPaidUser && !ConfigManager.GetTest())
      IAPHelper.ShowActivationWindow(type, ".pdf");
    else
      AppManager.OpenConverterCore("app1", type, files, password);
  }

  public static void OpenPDFConverterToPdf(ConvToPDFType type, params string[] files)
  {
    AppManager.OpenPDFConverterToPdf(type.ToString(), files);
  }

  public static void OpenPDFConverterToPdf(ConvToPDFType type, string password, string[] files)
  {
    AppManager.OpenPDFConverterToPdf(type.ToString(), password, files);
  }

  public static void OpenPDFConverterToPdf(string type, params string[] files)
  {
    AppManager.OpenPDFConverterToPdf(type, (string) null, files);
  }

  public static void OpenPDFConverterToPdf(string type, string password, string[] files)
  {
    if (((IEnumerable<string>) new string[3]
    {
      "MergePDF",
      "SplitPDF",
      "CompressPDF"
    }).Contains<string>(type) && !IAPHelper.IsPaidUser && !ConfigManager.GetTest())
      IAPHelper.ShowActivationWindow(type, ".pdf");
    else
      AppManager.OpenConverterCore("app2", type.ToString(), files, password);
  }

  public static bool IsSupportFileType(string file)
  {
    FileInfo fileInfo = new FileInfo(file);
    if (fileInfo == null)
      return false;
    string lower = fileInfo.Extension.ToLower();
    foreach (string[] array in AppManager.supportedFileTypeDict.Values)
    {
      if (Array.IndexOf<string>(array, lower) > -1)
        return true;
    }
    return false;
  }

  public static string GetOpenFileFilter()
  {
    string openFileFilter = "";
    string str1 = "";
    foreach (KeyValuePair<string, string[]> keyValuePair in AppManager.supportedFileTypeDict)
    {
      string key = keyValuePair.Key;
      string str2 = "";
      foreach (string str3 in keyValuePair.Value)
      {
        if (!string.IsNullOrWhiteSpace(str2))
          str2 += ";";
        str2 = $"{str2}*{str3}";
      }
      if (!string.IsNullOrWhiteSpace(openFileFilter))
        openFileFilter += "|";
      openFileFilter = $"{openFileFilter}{key}|{str2}";
      if (!string.IsNullOrWhiteSpace(str1))
        str1 += ";";
      str1 += str2;
    }
    return openFileFilter;
  }

  public static string[] GetDefaultFileExts()
  {
    return ((IEnumerable<string>) AppManager.pdfExtArray).Distinct<string>().ToArray<string>();
  }

  public static void RegisterFileAssociations(bool setAsDefault)
  {
    string rootDirectoryFile = AppManager.GetRootDirectoryFile("RegExt.exe");
    StringBuilder stringBuilder = new StringBuilder();
    if (!AppIdHelper.HasUserChoiceLatest)
    {
      stringBuilder.Append(" -setAsDefault");
      if (!setAsDefault)
        stringBuilder.Append(" -resetDefault");
    }
    string Parameters = stringBuilder.ToString();
    ProcessManager.RunProcess(rootDirectoryFile, Parameters);
  }
}
