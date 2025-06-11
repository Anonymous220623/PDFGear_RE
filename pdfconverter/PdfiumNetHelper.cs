// Decompiled with JetBrains decompiler
// Type: pdfconverter.PdfiumNetHelper
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using Patagames.Pdf.Net.BasicTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace pdfconverter;

public static class PdfiumNetHelper
{
  public static async Task<int> GetPageCountAsync(string filePath, string pwd)
  {
    if (string.IsNullOrEmpty(filePath))
      throw new ArgumentException(nameof (filePath));
    return await Task.Run<int>((Func<int>) (() =>
    {
      using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
      {
        using (PdfDocument pdfDocument = PdfDocument.Load((Stream) fileStream, password: pwd))
          return pdfDocument.Pages.Count;
      }
    })).ConfigureAwait(false);
  }

  public static async Task<bool> MergeAsync(
    System.Collections.Generic.IReadOnlyList<PdfiumPdfRange> inputFiles,
    string outputFile,
    IProgress<double> progress,
    CancellationToken cancellationToken)
  {
    cancellationToken.ThrowIfCancellationRequested();
    progress?.Report(0.0);
    if (string.IsNullOrEmpty(outputFile) || inputFiles == null || inputFiles.Count == 0)
    {
      progress?.Report(1.0);
      return false;
    }
    int num = 0;
    using (PdfDocument outputDoc = PdfDocument.CreateNew())
    {
      for (int index = 0; index < inputFiles.Count; ++index)
      {
        cancellationToken.ThrowIfCancellationRequested();
        PdfiumPdfRange inputFile = inputFiles[index];
        if (inputFile != null && !string.IsNullOrEmpty(inputFile.FilePath))
        {
          if (inputFile.EndPageIndex >= inputFile.StartPageIndex)
          {
            try
            {
              string password = inputFile.Password == "" ? (string) null : inputFile.Password;
              using (FileStream fileStream = new FileStream(inputFile.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
              {
                using (PdfDocument pdfDocument = PdfDocument.Load((Stream) fileStream, password: password))
                {
                  PdfiumNetHelper.TryFixResource(pdfDocument, inputFile.StartPageIndex, inputFile.EndPageIndex);
                  outputDoc.Pages.ImportPages(pdfDocument, $"{inputFile.StartPageIndex + 1}-{inputFile.EndPageIndex + 1}", outputDoc.Pages.Count);
                }
              }
              ++num;
            }
            catch
            {
            }
          }
        }
        progress?.Report(0.9 / (double) inputFiles.Count * (double) (index + 1));
      }
      cancellationToken.ThrowIfCancellationRequested();
      if (num != 0)
      {
        try
        {
          if (File.Exists(outputFile))
            File.Delete(outputFile);
          cancellationToken.ThrowIfCancellationRequested();
          using (FileStream outputStream = File.OpenWrite(outputFile))
          {
            outputDoc.Save((Stream) outputStream, SaveFlags.NoIncremental);
            await outputStream.FlushAsync(cancellationToken);
          }
          progress?.Report(1.0);
          return true;
        }
        catch
        {
        }
      }
    }
    progress?.Report(1.0);
    return false;
  }

  public static async Task<PdfiumNetHelper.SplitResult> SplitByRangeAsync(
    string inputFile,
    string password,
    string outputFolder,
    string splitRanges,
    IProgress<double> progress,
    CancellationToken cancellationToken)
  {
    cancellationToken.ThrowIfCancellationRequested();
    progress?.Report(0.0);
    if (string.IsNullOrEmpty(inputFile) || string.IsNullOrEmpty(outputFolder) || string.IsNullOrEmpty(splitRanges))
    {
      progress?.Report(1.0);
      return new PdfiumNetHelper.SplitResult(false, (System.Collections.Generic.IReadOnlyList<string>) null);
    }
    int[][] pageIndexes;
    if (!PageRangeHelper.TryParsePageRange2(splitRanges, out pageIndexes, out int _))
    {
      progress?.Report(1.0);
      return new PdfiumNetHelper.SplitResult(false, (System.Collections.Generic.IReadOnlyList<string>) null);
    }
    if (pageIndexes == null || pageIndexes.Length == 0 || ((IEnumerable<int[]>) pageIndexes).All<int[]>((Func<int[], bool>) (c => c.Length == 0)))
    {
      progress?.Report(1.0);
      return new PdfiumNetHelper.SplitResult(false, (System.Collections.Generic.IReadOnlyList<string>) null);
    }
    try
    {
      string password1 = password == "" ? (string) null : password;
      using (FileStream stream = new FileStream(inputFile, FileMode.Open, FileAccess.Read, FileShare.Read))
      {
        using (PdfDocument doc = PdfDocument.Load((Stream) stream, password: password1))
        {
          int[] array = ((IEnumerable<int[]>) pageIndexes).SelectMany<int[], int>((Func<int[], IEnumerable<int>>) (c => (IEnumerable<int>) c)).OrderBy<int, int>((Func<int, int>) (c => c)).ToArray<int>();
          PdfiumNetHelper.TryFixResource(doc, array[0], array[array.Length - 1]);
          FileInfo fileInfo = new FileInfo(inputFile);
          string fileNameWithoutExt = fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length);
          if (string.IsNullOrEmpty(fileNameWithoutExt))
            fileNameWithoutExt = "PDFgear Split";
          return await PdfiumNetHelper.SplitCore(doc, fileNameWithoutExt, outputFolder, pageIndexes, progress, cancellationToken);
        }
      }
    }
    catch (Exception ex) when (!(ex is OperationCanceledException))
    {
    }
    progress?.Report(1.0);
    return new PdfiumNetHelper.SplitResult(false, (System.Collections.Generic.IReadOnlyList<string>) null);
  }

  public static async Task<PdfiumNetHelper.SplitResult> SplitByMaxPageCountAsync(
    string inputFile,
    string password,
    string outputFolder,
    int maxPageCount,
    IProgress<double> progress,
    CancellationToken cancellationToken)
  {
    cancellationToken.ThrowIfCancellationRequested();
    progress?.Report(0.0);
    if (maxPageCount <= 0)
    {
      progress?.Report(1.0);
      return new PdfiumNetHelper.SplitResult(false, (System.Collections.Generic.IReadOnlyList<string>) null);
    }
    if (string.IsNullOrEmpty(inputFile) || string.IsNullOrEmpty(outputFolder))
    {
      progress?.Report(1.0);
      return new PdfiumNetHelper.SplitResult(false, (System.Collections.Generic.IReadOnlyList<string>) null);
    }
    try
    {
      string password1 = password == "" ? (string) null : password;
      using (FileStream stream = new FileStream(inputFile, FileMode.Open, FileAccess.Read, FileShare.Read))
      {
        using (PdfDocument doc = PdfDocument.Load((Stream) stream, password: password1))
        {
          PdfiumNetHelper.TryFixResource(doc, 0, doc.Pages.Count - 1);
          FileInfo fileInfo = new FileInfo(inputFile);
          string fileNameWithoutExt = fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length);
          if (string.IsNullOrEmpty(fileNameWithoutExt))
            fileNameWithoutExt = "PDFgear Split";
          List<int[]> numArrayList = new List<int[]>();
          for (int index = 0; index < doc.Pages.Count; index += maxPageCount)
          {
            int start = index;
            int num = Math.Min(index + maxPageCount - 1, doc.Pages.Count - 1);
            $"{start + 1}-{num + 1}";
            numArrayList.Add(Enumerable.Range(start, num - start + 1).ToArray<int>());
          }
          return await PdfiumNetHelper.SplitCore(doc, fileNameWithoutExt, outputFolder, numArrayList.ToArray(), progress, cancellationToken);
        }
      }
    }
    catch (Exception ex) when (!(ex is OperationCanceledException))
    {
    }
    progress?.Report(1.0);
    return new PdfiumNetHelper.SplitResult(false, (System.Collections.Generic.IReadOnlyList<string>) null);
  }

  private static async Task<PdfiumNetHelper.SplitResult> SplitCore(
    PdfDocument doc,
    string fileNameWithoutExt,
    string outputFolder,
    int[][] pageIndexes,
    IProgress<double> progress,
    CancellationToken cancellationToken)
  {
    if (doc == null || string.IsNullOrEmpty(fileNameWithoutExt) || string.IsNullOrEmpty(outputFolder) || pageIndexes == null || pageIndexes.Length == 0 || ((IEnumerable<int[]>) pageIndexes).All<int[]>((Func<int[], bool>) (c => c.Length == 0)))
      return new PdfiumNetHelper.SplitResult(false, (System.Collections.Generic.IReadOnlyList<string>) null);
    cancellationToken.ThrowIfCancellationRequested();
    List<string> outputFileList = new List<string>();
    for (int i = 0; i < pageIndexes.Length; ++i)
    {
      int[] array = ((IEnumerable<int>) pageIndexes[i]).Where<int>((Func<int, bool>) (c => c >= 0 && c < doc.Pages.Count)).ToArray<int>();
      if (array.Length != 0)
      {
        try
        {
          string outputFullPath;
          using (PdfDocument tmpDoc = PdfDocument.CreateNew())
          {
            string range = ((IEnumerable<int>) array).ConvertToRange();
            tmpDoc.Pages.ImportPages(doc, range, 0);
            outputFullPath = Path.Combine(outputFolder, $"{fileNameWithoutExt} [{range}].pdf");
            if (File.Exists(outputFullPath))
              File.Delete(outputFullPath);
            cancellationToken.ThrowIfCancellationRequested();
            using (FileStream tmpStream = File.OpenWrite(outputFullPath))
            {
              tmpDoc.Save((Stream) tmpStream, SaveFlags.NoIncremental);
              await tmpStream.FlushAsync(cancellationToken);
            }
          }
          outputFileList.Add(outputFullPath);
          outputFullPath = (string) null;
        }
        catch
        {
        }
      }
      progress?.Report(1.0 / (double) pageIndexes.Length * (double) (i + 1));
    }
    return new PdfiumNetHelper.SplitResult(outputFileList.Count > 0, (System.Collections.Generic.IReadOnlyList<string>) outputFileList);
  }

  private static void TryFixResource(PdfDocument doc, int startPage, int endPage)
  {
    if (doc == null || startPage < 0 || endPage < 0)
      return;
    endPage = Math.Min(endPage, doc.Pages.Count - 1);
    if (endPage < startPage)
      return;
    try
    {
      for (int index = startPage; index <= endPage; ++index)
      {
        try
        {
          PdfPage page = doc.Pages[index];
          if (!page.Dictionary.ContainsKey("Resources"))
          {
            PdfTypeBase parentResources = PdfiumNetHelper.FindParentResources(page);
            if (parentResources != null)
              page.Dictionary["Resources"] = PdfiumNetHelper.DeepClone(parentResources);
          }
          page.Dispose();
        }
        catch
        {
        }
      }
    }
    catch
    {
    }
  }

  private static PdfTypeBase FindParentResources(PdfPage page)
  {
    if (page?.Dictionary == null)
      return (PdfTypeBase) null;
    for (PdfTypeDictionary parentPagesNode = GetParentPagesNode(page.Dictionary); parentPagesNode != null; parentPagesNode = GetParentPagesNode(parentPagesNode))
    {
      PdfTypeBase resources = GetResources((PdfTypeBase) parentPagesNode);
      if (resources != null)
        return resources;
    }
    return (PdfTypeBase) null;

    static PdfTypeDictionary GetParentPagesNode(PdfTypeDictionary _dict)
    {
      PdfTypeBase pdfTypeBase;
      return _dict.TryGetValue("Parent", out pdfTypeBase) && pdfTypeBase.Is<PdfTypeDictionary>() ? pdfTypeBase.As<PdfTypeDictionary>() : (PdfTypeDictionary) null;
    }

    static PdfTypeBase GetResources(PdfTypeBase _pagesNode)
    {
      PdfTypeBase pdfTypeBase;
      return _pagesNode.Is<PdfTypeDictionary>() && _pagesNode.As<PdfTypeDictionary>().TryGetValue("Resources", out pdfTypeBase) && pdfTypeBase.Is<PdfTypeDictionary>() ? pdfTypeBase : (PdfTypeBase) null;
    }
  }

  private static PdfTypeBase DeepClone(PdfTypeBase obj)
  {
    if (obj == null)
      return (PdfTypeBase) null;
    if (obj is PdfTypeIndirect pdfTypeIndirect)
      return pdfTypeIndirect.Clone();
    if (obj.Is<PdfTypeBoolean>() || obj.Is<PdfTypeName>() || obj.Is<PdfTypeNull>() || obj.Is<PdfTypeNumber>() || obj.Is<PdfTypeString>() || obj.Is<PdfTypeUnknown>() || obj.Is<PdfTypeStream>())
      return obj.Clone();
    if (obj.Is<PdfTypeArray>())
    {
      PdfTypeArray pdfTypeArray = PdfTypeArray.Create();
      foreach (PdfTypeBase a in obj.As<PdfTypeArray>())
        pdfTypeArray.Add(PdfiumNetHelper.DeepClone(a));
      return (PdfTypeBase) pdfTypeArray;
    }
    if (!obj.Is<PdfTypeDictionary>())
      return (PdfTypeBase) null;
    PdfTypeDictionary pdfTypeDictionary = PdfTypeDictionary.Create();
    foreach (KeyValuePair<string, PdfTypeBase> a in obj.As<PdfTypeDictionary>())
      pdfTypeDictionary[a.Key] = PdfiumNetHelper.DeepClone(a.Value);
    return (PdfTypeBase) pdfTypeDictionary;
  }

  public class SplitResult
  {
    public SplitResult(bool success, System.Collections.Generic.IReadOnlyList<string> outputFiles)
    {
      this.Success = success;
      this.OutputFiles = (System.Collections.Generic.IReadOnlyList<string>) ((object) outputFiles ?? (object) Array.Empty<string>());
    }

    public bool Success { get; }

    public System.Collections.Generic.IReadOnlyList<string> OutputFiles { get; }
  }
}
