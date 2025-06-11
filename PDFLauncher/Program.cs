// Decompiled with JetBrains decompiler
// Type: PDFLauncher.Program
// Assembly: PDFLauncher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 94FE2CE7-EB91-4B8B-872D-74808A04C1E6
// Assembly location: C:\Program Files\PDFgear\PDFLauncher.exe

using CommomLib.Commom;
using CommomLib.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using PDFLauncher.Utils;
using PDFLauncher.ViewModels;
using System;
using System.IO;
using System.Threading;

#nullable enable
namespace PDFLauncher;

public static class Program
{
  internal static bool NeedShowRecover { get; private set; }

  internal static 
  #nullable disable
  Program.OpenFileResult OpenFileOnLaunchResult { get; private set; }

  internal static bool AppLaunchEventsResult { get; private set; }

  [STAThread]
  public static void Main(string[] args)
  {
    SqliteUtils.InitializeDatabase();
    CultureInfoUtils.Initialize();
    Program.NeedShowRecover = AutoSaveManager.IsNeedShowRecover();
    if (!Program.NeedShowRecover)
      Program.OpenFileOnLaunch(args);
    Program.AppLaunchEventsResult = App.AppLaunchEvents();
    if (Program.OpenFileOnLaunchResult != null && Program.OpenFileOnLaunchResult.FileOpened && Program.AppLaunchEventsResult)
    {
      Thread.Sleep(5000);
    }
    else
    {
      AppIdHelper.RegisterAppUserModelId();
      ServiceCollection services = new ServiceCollection();
      services.AddSingleton<MainViewModel>((Func<IServiceProvider, MainViewModel>) (_ => new MainViewModel()));
      services.AddSingleton<RecoverViewModel>((Func<IServiceProvider, RecoverViewModel>) (_ => new RecoverViewModel()));
      Ioc.Default.ConfigureServices((IServiceProvider) services.BuildServiceProvider());
      App app = new App();
      app.InitializeComponent();
      app.Run();
    }
  }

  internal static void OpenFileOnLaunch(string[] args)
  {
    if (args == null || args.Length == 0)
      return;
    string str1 = args[0];
    string empty = string.Empty;
    string str2 = str1;
    if (string.IsNullOrEmpty(str2))
      return;
    Program.OpenFileOnLaunchResult = new Program.OpenFileResult(new FileInfo(str2), FileManager.OpenOneFile(str2));
  }

  internal class OpenFileResult
  {
    public OpenFileResult(FileInfo fileInfo, bool fileOpened)
    {
      this.FileInfo = fileInfo;
      this.FileOpened = fileOpened;
    }

    public FileInfo FileInfo { get; }

    public bool FileOpened { get; }
  }
}
