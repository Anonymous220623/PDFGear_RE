// Decompiled with JetBrains decompiler
// Type: PDFLauncher.Utils.FileInfoUtils
// Assembly: PDFLauncher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 94FE2CE7-EB91-4B8B-872D-74808A04C1E6
// Assembly location: C:\Program Files\PDFgear\PDFLauncher.exe

using System;
using System.IO;

#nullable disable
namespace PDFLauncher.Utils;

public static class FileInfoUtils
{
  public static string GetLastOpenTime(string path)
  {
    return !File.Exists(path) ? "" : new FileInfo(path).LastAccessTime.ToString("yyyy-MM-dd HH:mm:ss");
  }

  public static string GetFileName(string path)
  {
    return string.IsNullOrEmpty(path) ? "" : new FileInfo(path).Name;
  }

  public static string GetFileSize(string FileName)
  {
    if (!File.Exists(FileName))
      return "";
    long length = new FileInfo(FileName).Length;
    double x = 1024.0;
    if (length <= 0L)
      return "0 KB";
    if ((double) length < x)
      return "1 KB";
    if ((double) length < Math.Pow(x, 2.0))
      return ((double) length / x).ToString("f2") + " KB";
    if ((double) length < Math.Pow(x, 3.0))
      return ((double) length / Math.Pow(x, 2.0)).ToString("f2") + " MB";
    return (double) length < Math.Pow(x, 4.0) ? ((double) length / Math.Pow(x, 3.0)).ToString("f2") + " GB" : ((double) length / Math.Pow(x, 4.0)).ToString("f2") + " TB";
  }
}
