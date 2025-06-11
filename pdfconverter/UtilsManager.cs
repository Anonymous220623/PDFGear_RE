// Decompiled with JetBrains decompiler
// Type: pdfconverter.UtilsManager
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using System;
using System.Diagnostics;
using System.IO;

#nullable disable
namespace pdfconverter;

internal class UtilsManager
{
  public static void OpenFile(string file)
  {
    if (string.IsNullOrWhiteSpace(file))
      return;
    try
    {
      Process.Start(file);
    }
    catch
    {
    }
  }

  public static void OpenFileInExplore(string file, bool isFile)
  {
    if (string.IsNullOrWhiteSpace(file))
      return;
    try
    {
      char[] chArray = new char[3]{ '\\', '/', ' ' };
      Process.Start("explorer.exe", $"/select, \"{file.TrimEnd(chArray)}\"");
    }
    catch
    {
    }
  }

  public static void OpenFolderInExplore(string folder)
  {
    if (string.IsNullOrWhiteSpace(folder))
      return;
    try
    {
      char[] chArray = new char[3]{ '\\', '/', ' ' };
      Process.Start("explorer.exe", folder.TrimEnd(chArray));
    }
    catch
    {
    }
  }

  public static bool IsPDFFile(string file)
  {
    if (string.IsNullOrWhiteSpace(file))
      return false;
    string extension = Path.GetExtension(file);
    return !string.IsNullOrWhiteSpace(extension) && extension.Equals(".pdf", StringComparison.CurrentCultureIgnoreCase);
  }

  public static bool IsnotSupportFile(string file, string[] supportExt)
  {
    if (string.IsNullOrWhiteSpace(file))
      return true;
    string extension = Path.GetExtension(file);
    return string.IsNullOrWhiteSpace(extension) || Array.IndexOf<string>(supportExt, extension.ToLower()) < 0;
  }

  public static string getValidFileName(string path, string fileName, string extention = ".pdf")
  {
    if (!string.IsNullOrWhiteSpace(path) && !string.IsNullOrWhiteSpace(fileName))
    {
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
      string validFileName = fileName;
      for (int index = 2; index < 100; ++index)
      {
        string path1 = $"{path}\\{validFileName}{extention}";
        if (!Directory.Exists(path1) && !File.Exists(path1))
          return validFileName;
        validFileName = fileName + $" {index}";
      }
    }
    return "";
  }

  public static string getPDFFileName(string path, string fileName)
  {
    if (!string.IsNullOrWhiteSpace(path) && !string.IsNullOrWhiteSpace(fileName))
    {
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
      string withoutExtension = Path.GetFileNameWithoutExtension(fileName);
      string str = withoutExtension;
      for (int index = 2; index < 100; ++index)
      {
        string path1 = $"{path}\\{str}.pdf";
        if (!File.Exists(path1))
          return path1;
        str = withoutExtension + $" {index}";
      }
    }
    return "";
  }

  public static string getPDFResult(string path, string fileName)
  {
    if (!string.IsNullOrWhiteSpace(path) && !string.IsNullOrWhiteSpace(fileName))
    {
      string withoutExtension = Path.GetFileNameWithoutExtension(fileName);
      string str1 = withoutExtension;
      for (int index = 2; index < 100; ++index)
      {
        if (File.Exists($"{path}\\{str1}.pdf"))
        {
          str1 = withoutExtension + $" {index}";
        }
        else
        {
          string str2 = index == 3 ? withoutExtension : withoutExtension + $" {index - 2}";
          return $"{path}\\{str2}.pdf";
        }
      }
    }
    return "";
  }

  public static string getValidFolder(string path, string foldName)
  {
    if (!string.IsNullOrWhiteSpace(path))
    {
      if (string.IsNullOrWhiteSpace(foldName))
        foldName = "PDF Files";
      string validFolder = foldName.Trim();
      for (int index = 1; index < 100; ++index)
      {
        string path1 = $"{path}\\{validFolder}";
        if (!Directory.Exists(path1) && !File.Exists(path1))
          return validFolder;
        validFolder = foldName + $" {index}";
      }
    }
    return "";
  }
}
