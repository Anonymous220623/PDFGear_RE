// Decompiled with JetBrains decompiler
// Type: CommomLib.ConvertUtils.ConvertUtils
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using CommomLib.Commom;
using CommomLib.Models;
using Syncfusion.DocIO.DLS;
using Syncfusion.OfficeChart;
using Syncfusion.OfficeChartToImageConverter;
using Syncfusion.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace CommomLib.ConvertUtils;

public static class ConvertUtils
{
  private static string _rootTempFolder;

  public static async Task<string> GetTempPDF(
    InsertSourceFileType type,
    string inputPath,
    CancellationToken token)
  {
    string tmpFileFullName = "";
    do
    {
      tmpFileFullName = Path.Combine(CommomLib.ConvertUtils.ConvertUtils.MergeDocumentTempFolder, Guid.NewGuid().ToString("N").Substring(0, 8) ?? "");
    }
    while (File.Exists(tmpFileFullName));
    if (type == InsertSourceFileType.Doc)
    {
      bool flag = false;
      if (CommomLib.ConvertUtils.ConvertUtils.ExtEquals(Path.GetExtension(inputPath), UtilManager.WordExtention))
        flag = await CommomLib.ConvertUtils.ConvertUtils.WordToPDFByRangeAsync(inputPath, tmpFileFullName, 0, 0, (IProgress<double>) null, token);
      if (!flag)
        tmpFileFullName = "";
    }
    else
      tmpFileFullName = "";
    string tempPdf = tmpFileFullName;
    tmpFileFullName = (string) null;
    return tempPdf;
  }

  public static bool ExtEquals(string InputExt, string Exts)
  {
    if (string.IsNullOrWhiteSpace(InputExt))
      return false;
    string lower = InputExt.ToLower();
    string str1 = Exts;
    char[] chArray = new char[1]{ ';' };
    foreach (string str2 in ((IEnumerable<string>) str1.Split(chArray)).ToList<string>())
    {
      if (!string.IsNullOrWhiteSpace(str2))
      {
        string str3 = str2.Replace("*", "");
        if (lower.Equals(str3, StringComparison.CurrentCultureIgnoreCase))
          return true;
      }
    }
    return false;
  }

  private static string RootTempFolder
  {
    get
    {
      if (string.IsNullOrEmpty(CommomLib.ConvertUtils.ConvertUtils._rootTempFolder))
        CommomLib.ConvertUtils.ConvertUtils._rootTempFolder = UtilManager.GetTemporaryPath();
      return CommomLib.ConvertUtils.ConvertUtils._rootTempFolder;
    }
  }

  private static string DocumentTempFolder
  {
    get
    {
      string path = Path.Combine(CommomLib.ConvertUtils.ConvertUtils.RootTempFolder, "Documents");
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
      return path;
    }
  }

  private static string MergeDocumentTempFolder
  {
    get
    {
      string path = Path.Combine(CommomLib.ConvertUtils.ConvertUtils.DocumentTempFolder, "Merge");
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
      return path;
    }
  }

  public static async Task<bool> WordToPDFByRangeAsync(
    string inputFile,
    string outfullName,
    int start,
    int end,
    IProgress<double> progress,
    CancellationToken cancellationToken)
  {
    if (!LicenceManage.SycfusionRegisterLicence())
      return false;
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
        if (!string.IsNullOrWhiteSpace(extension) && extension.ToLower().Equals(".doc", StringComparison.CurrentCultureIgnoreCase))
          wordDocument.Settings.SkipIncrementalSaveValidation = true;
        wordDocument.Open(inputFile);
        wordDocument.ChartToImageConverter = (IOfficeChartToImageConverter) new ChartToImageConverter();
        wordDocument.ChartToImageConverter.ScalingMode = ScalingMode.Normal;
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
}
