// Decompiled with JetBrains decompiler
// Type: PDFLauncher.Utils.FileManager
// Assembly: PDFLauncher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 94FE2CE7-EB91-4B8B-872D-74808A04C1E6
// Assembly location: C:\Program Files\PDFgear\PDFLauncher.exe

using CommomLib.Commom;
using Microsoft.Win32;
using PDFLauncher.Properties;
using System.Diagnostics;
using System.IO;
using System.Windows;

#nullable disable
namespace PDFLauncher.Utils;

public static class FileManager
{
  public static string SelectFileForOpen()
  {
    OpenFileDialog openFileDialog = new OpenFileDialog();
    openFileDialog.Multiselect = false;
    openFileDialog.Filter = CommomLib.Commom.AppManager.GetOpenFileFilter();
    return openFileDialog.ShowDialog().GetValueOrDefault() ? openFileDialog.FileName : (string) null;
  }

  public static string SelectPDFFileForOpen()
  {
    OpenFileDialog openFileDialog = new OpenFileDialog();
    openFileDialog.Multiselect = false;
    openFileDialog.Filter = "PDF documents|*.pdf";
    return openFileDialog.ShowDialog().GetValueOrDefault() ? openFileDialog.FileName : (string) null;
  }

  public static bool OpenOneFile(string file, string action = null)
  {
    if (string.IsNullOrWhiteSpace(file))
      return false;
    if (!File.Exists(file))
    {
      int num = (int) MessageBox.Show(Resources.OpenFileNoExistMsg + file, UtilManager.GetProductName());
      return false;
    }
    if (!CommomLib.Commom.AppManager.IsSupportFileType(file))
    {
      int num = (int) MessageBox.Show(Resources.OpenFileNoSupporttypeMsg + file, UtilManager.GetProductName());
      return false;
    }
    if (new FileInfo(file).Extension.ToLower().Equals(".pdf"))
      CommomLib.Commom.AppManager.OpenEditor(file, action);
    return true;
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
}
