// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.AutoSaveManager
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using CommomLib.Config.ConfigModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

#nullable disable
namespace CommomLib.Commom;

public class AutoSaveManager
{
  public static bool IsNeedShowRecover()
  {
    string path = Path.Combine(AppDataHelper.LocalFolder, "BackUp");
    if (!Directory.Exists(path))
      return false;
    List<FileInfo> list1 = ((IEnumerable<FileInfo>) new DirectoryInfo(path).GetFiles("*.data")).ToList<FileInfo>();
    if (list1.Count <= 0)
      return false;
    List<FileInfo> source = new List<FileInfo>();
    for (int index = 0; index < list1.Count; ++index)
    {
      FileInfo fileInfo = list1[index];
      int startIndex = fileInfo.Name.LastIndexOf("_") + 1;
      int length = fileInfo.Name.LastIndexOf(".") - startIndex;
      if (ConfigManager.GetAutoSaveFileModelAsync(new CancellationToken(), fileInfo.Name.Substring(startIndex, length)).GetAwaiter().GetResult() == null)
        fileInfo.Delete();
      else
        source.Add(fileInfo);
    }
    List<string> list2 = source.Select<FileInfo, string>((Func<FileInfo, string>) (f => f.Name)).ToList<string>();
    return source.Count > 0 && !AutoSaveManager.GetAutoSaveFileByUnUsingAll(list2);
  }

  public static void OpenRecover()
  {
    ProcessManager.RunProcess(Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\', '/', ' ')).FullName, "pdfrecover", "pdfrecover.exe"), "");
  }

  public static List<Process> GetPdfeditorProcesss()
  {
    return ((IEnumerable<Process>) Process.GetProcessesByName("pdfeditor")).ToList<Process>();
  }

  private static bool GetAutoSaveFileByUnUsingAll(List<string> fileNames)
  {
    List<AutoSaveFileModel> result = ConfigManager.GetAutoSaveFilesAsync(new CancellationToken()).GetAwaiter().GetResult();
    List<Process> pdfeditorProcesss = AutoSaveManager.GetPdfeditorProcesss();
    int num = 0;
    if (result != null && pdfeditorProcesss != null)
    {
      for (int index = 0; index < result.Count; ++index)
      {
        AutoSaveFileModel configFile = result[index];
        if (pdfeditorProcesss.Find((Predicate<Process>) (p => p.Id == configFile.CreatePid)) != null && fileNames.Find((Predicate<string>) (f => f.Contains(configFile.Guid))) != null)
          ++num;
      }
    }
    return num == fileNames.Count;
  }

  public static void DelTempFileByCloseExe(int pId, string filefullName)
  {
    string path = Path.Combine(AppDataHelper.LocalFolder, "BackUp");
    if (!Directory.Exists(path))
      return;
    FileInfo[] files = new DirectoryInfo(path).GetFiles();
    if (files.Length == 0)
      return;
    List<AutoSaveFileModel> result = ConfigManager.GetAutoSaveFilesAsync(new CancellationToken()).GetAwaiter().GetResult();
    if (result == null)
      return;
    for (int index = 0; index < files.Length; ++index)
    {
      FileInfo f = files[index];
      AutoSaveFileModel autoSaveFileModel = result.Find((Predicate<AutoSaveFileModel>) (x => x.TempFileName == f.FullName));
      if (autoSaveFileModel != null && f.Name.Contains(autoSaveFileModel.Guid) && pId == autoSaveFileModel.CreatePid && autoSaveFileModel.SoruceFileFullName == filefullName)
      {
        f.Delete();
        ConfigManager.DelAutoSaveFileAsync(autoSaveFileModel.Guid, new int?(pId)).GetAwaiter().GetResult();
      }
    }
  }
}
