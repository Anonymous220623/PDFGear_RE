// Decompiled with JetBrains decompiler
// Type: pdfeditor.AutoSaveRestore.AutoSaveManager
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using CommomLib.Config.ConfigModels;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using pdfeditor.Models;
using pdfeditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

#nullable disable
namespace pdfeditor.AutoSaveRestore;

public class AutoSaveManager
{
  private static AutoSaveManager m_stance;
  private Timer savetimer;
  private bool isEnabled;
  private int autoSaveIntervalMinus;
  public static string SaveDir = Path.Combine(AppDataHelper.LocalFolder, "BackUp");
  public static string MutexOperationID = "d672d331-57aa-4230-b3ce-d0488770978f";

  private MainViewModel vm => Ioc.Default.GetRequiredService<MainViewModel>();

  public event EventHandler SaveStarted;

  public event EventHandler SaveCompleted;

  public string LastOperationVersion { get; set; }

  public bool CanSaveByOperationManager { get; set; }

  public AutoSaveManager()
  {
    this.savetimer = new Timer(new TimerCallback(this.ToSave), (object) null, -1, 1000);
    this.LastOperationVersion = "";
    this.CanSaveByOperationManager = true;
  }

  public static AutoSaveManager GetInstance()
  {
    if (AutoSaveManager.m_stance == null)
      AutoSaveManager.m_stance = new AutoSaveManager();
    return AutoSaveManager.m_stance;
  }

  private void ToSave(object filename)
  {
    if (!this.vm.CanSave || this.LastOperationVersion == this.vm.OperationManager?.Version || !this.CanSaveByOperationManager)
      return;
    EventHandler saveStarted = this.SaveStarted;
    if (saveStarted != null)
      saveStarted((object) this, EventArgs.Empty);
    string saveDir = AutoSaveManager.SaveDir;
    DocumentWrapper documentWrapper = this.vm.DocumentWrapper;
    PdfDocument document = documentWrapper.Document;
    if (string.IsNullOrEmpty(documentWrapper?.DocumentPath))
      return;
    FileInfo fileInfo = new FileInfo(documentWrapper?.DocumentPath);
    string str = fileInfo.Name;
    if (!string.IsNullOrEmpty(fileInfo.Extension))
      str = str.Substring(0, str.Length - fileInfo.Extension.Length);
    if (!Directory.Exists(saveDir))
      Directory.CreateDirectory(saveDir);
    string lower1;
    string path1;
    do
    {
      lower1 = Guid.NewGuid().ToString("N").ToLower();
      string path2 = $"{str}_{lower1}" + ".data";
      path1 = Path.Combine(saveDir, path2);
    }
    while (File.Exists(path1));
    string path3 = path1;
    int id = Process.GetCurrentProcess().Id;
    this.DelSameFileByProcess(id);
    AutoSaveFileModel model = new AutoSaveFileModel()
    {
      Guid = lower1,
      SoruceFileFullName = fileInfo.FullName,
      LastSaveTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
      FileName = fileInfo.Name,
      CreatePid = id,
      TempFileName = path3
    };
    try
    {
      using (FileStream fileStream = File.OpenWrite(path3))
      {
        fileStream.Seek(0L, SeekOrigin.Begin);
        document.Save((Stream) fileStream, SaveFlags.NoIncremental);
        fileStream.SetLength(fileStream.Position);
      }
    }
    catch (PathTooLongException ex)
    {
      string lower2 = Guid.NewGuid().ToString("N").ToLower();
      string path4 = Path.Combine(saveDir, lower2) + ".data";
      model.TempFileName = path4;
      model.Guid = lower2;
      using (FileStream fileStream = File.OpenWrite(path4))
      {
        fileStream.Seek(0L, SeekOrigin.Begin);
        document.Save((Stream) fileStream, SaveFlags.NoIncremental);
        fileStream.SetLength(fileStream.Position);
      }
    }
    catch (DirectoryNotFoundException ex)
    {
      string lower3 = Guid.NewGuid().ToString("N").ToLower();
      string path5 = Path.Combine(saveDir, lower3) + ".data";
      model.TempFileName = path5;
      model.Guid = lower3;
      using (FileStream fileStream = File.OpenWrite(path5))
      {
        fileStream.Seek(0L, SeekOrigin.Begin);
        document.Save((Stream) fileStream, SaveFlags.NoIncremental);
        fileStream.SetLength(fileStream.Position);
      }
    }
    catch (Exception ex)
    {
      GAManager.SendEvent("Exception", "AutoSave", $"{ex.GetType().Name}, {ex.Message}", 1L);
    }
    finally
    {
      ConfigManager.SetAutoSaveFileAsync(model);
      EventHandler saveCompleted = this.SaveCompleted;
      if (saveCompleted != null)
        saveCompleted((object) this, EventArgs.Empty);
      this.LastOperationVersion = this.vm.OperationManager?.Version;
    }
  }

  private void DelSameFileByProcess(int pid)
  {
    if (!Directory.Exists(AutoSaveManager.SaveDir))
      return;
    FileInfo[] files = new DirectoryInfo(AutoSaveManager.SaveDir).GetFiles();
    if (files.Length == 0)
      return;
    List<AutoSaveFileModel> result = ConfigManager.GetAutoSaveFilesAsync(new CancellationToken()).GetAwaiter().GetResult();
    if (result == null)
      return;
    for (int index = 0; index < files.Length; ++index)
    {
      FileInfo f = files[index];
      AutoSaveFileModel autoSaveFileModel = result.Find((Predicate<AutoSaveFileModel>) (c => c.TempFileName == f.FullName));
      if (autoSaveFileModel != null && f.Name.Contains(autoSaveFileModel.Guid) && pid == autoSaveFileModel.CreatePid)
      {
        f.Delete();
        ConfigManager.DelAutoSaveFileAsync(autoSaveFileModel.Guid, new int?(pid));
      }
    }
  }

  public void Start(int mins)
  {
    if (!this.vm.AutoSaveModel.IsAuto)
      return;
    this.autoSaveIntervalMinus = mins;
    this.isEnabled = true;
    this.savetimer.Change(0, 60000 * mins);
  }

  public void Stop()
  {
    this.isEnabled = false;
    this.savetimer.Change(-1, 1000);
  }

  public void TrySaveImmediately()
  {
    int num = this.isEnabled ? 1 : 0;
    this.Stop();
    this.ToSave((object) null);
    if (num == 0)
      return;
    this.Start(this.autoSaveIntervalMinus);
  }
}
