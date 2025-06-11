// Decompiled with JetBrains decompiler
// Type: pdfconverter.Utils.TempFileUtils
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using CommomLib.Commom;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace pdfconverter.Utils;

public static class TempFileUtils
{
  private static readonly string TempFolder = Path.Combine(AppDataHelper.TemporaryFolder, "ConvertOnline");

  public static async Task<string> SplitFileInRangeAsync(string Filepath, int from, int to)
  {
    if (!Directory.Exists(TempFileUtils.TempFolder))
      Directory.CreateDirectory(TempFileUtils.TempFolder);
    string splitRanges = $"{from}-{to}";
    CancellationToken cancellationToken;
    PdfiumNetHelper.SplitResult splitResult = await PdfiumNetHelper.SplitByRangeAsync(Filepath, (string) null, TempFileUtils.TempFolder, splitRanges, (IProgress<double>) null, cancellationToken);
    if (!splitResult.Success)
      return "";
    string path2 = splitResult.OutputFiles.FirstOrDefault<string>();
    return Path.Combine(TempFileUtils.TempFolder, path2);
  }

  public static string GetTempFileName(string Filepath, int from, int to)
  {
    if (!Directory.Exists(TempFileUtils.TempFolder))
      Directory.CreateDirectory(TempFileUtils.TempFolder);
    string str = $"{from}-{to}";
    string path2 = Path.Combine(TempFileUtils.TempFolder, $"{Path.GetFileNameWithoutExtension(Filepath).Trim()} [{str}].pdf");
    return Path.Combine(TempFileUtils.TempFolder, path2);
  }
}
