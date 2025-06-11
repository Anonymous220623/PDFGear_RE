// Decompiled with JetBrains decompiler
// Type: PDFLauncher.App
// Assembly: PDFLauncher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 94FE2CE7-EB91-4B8B-872D-74808A04C1E6
// Assembly location: C:\Program Files\PDFgear\PDFLauncher.exe

using CommomLib.AppTheme;
using CommomLib.Commom;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using PDFLauncher.Utils;
using PDFLauncher.ViewModels;
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

#nullable disable
namespace PDFLauncher;

public partial class App : Application
{
  public AppLifeWindow appLifeWin;
  private SystemThemeListener systemThemeListener;
  private bool _contentLoaded;

  public App()
  {
    PDFLauncher.Properties.Resources.Culture = CultureInfo.CurrentUICulture;
    CommomLib.Properties.Resources.Culture = CultureInfo.CurrentUICulture;
    this.systemThemeListener = new SystemThemeListener(this.Dispatcher);
    this.systemThemeListener.ActualAppThemeChanged += (EventHandler) ((s, a) => ThemeResourceDictionary.GetForCurrentApp().Theme = this.GetCurrentActualAppTheme());
    WindowThemeHelper.Initialize();
    ProcessMessageHelper.MessageReceived += new EventHandler<ProcessMessageReceivedEventArgs>(this.ProcessMessageHelper_MessageReceived);
  }

  private void ProcessMessageHelper_MessageReceived(
    object sender,
    ProcessMessageReceivedEventArgs e)
  {
    if (e.Message == "UpdateTheme")
    {
      this.Dispatcher.InvokeAsync((Action) (() => ThemeResourceDictionary.GetForCurrentApp().Theme = this.GetCurrentActualAppTheme()));
    }
    else
    {
      if (!(e.Message == "UpdateFileHistory"))
        return;
      this.Dispatcher.InvokeAsync((Action) (() => Ioc.Default.GetRequiredService<MainViewModel>().ReadHistory()));
    }
  }

  private string GetCurrentActualAppTheme()
  {
    switch (ConfigManager.GetCurrentAppTheme()?.ToLowerInvariant())
    {
      case "dark":
        return "Dark";
      case "auto":
        return this.systemThemeListener.ActualAppTheme != SystemThemeListener.ActualTheme.Light ? "Dark" : "Light";
      default:
        return "Light";
    }
  }

  protected override async void OnStartup(StartupEventArgs e)
  {
    App app = this;
    int sleepInt = 3000;
    // ISSUE: reference to a compiler-generated method
    app.\u003C\u003En__0(e);
    UpdateHelper.RemoveUpdateDialogShownFlag();
    ThemeResourceDictionary.GetForCurrentApp().Theme = app.GetCurrentActualAppTheme();
    if (!Program.AppLaunchEventsResult)
    {
      App.OpenUpdateAdConfigWindow();
    }
    else
    {
      int num1 = IAPUtils.NeedToShowIAP() ? 1 : 0;
      if (Program.NeedShowRecover)
      {
        app.appLifeWin = new AppLifeWindow();
        PDFLauncher.Utils.AppManager.ShowRecoverWindows();
      }
      if (Program.OpenFileOnLaunchResult == null)
        Program.OpenFileOnLaunch(e.Args);
      if (Program.OpenFileOnLaunchResult == null || !Program.OpenFileOnLaunchResult.FileOpened)
      {
        sleepInt = 500;
        PDFLauncher.MainWindow mainWindow = new PDFLauncher.MainWindow();
        mainWindow.Show();
        Application.Current.MainWindow = (Window) mainWindow;
        if (app.appLifeWin != null)
        {
          app.appLifeWin.Close();
          app.appLifeWin = (AppLifeWindow) null;
        }
      }
      App.OpenUpdateAdConfigWindow();
      if (num1 != 0)
        await app.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Delegate) (async () =>
        {
          await Task.Delay(sleepInt);
          IAPUtils.ShowPurchaseWindows("Startup", Program.OpenFileOnLaunchResult?.FileInfo.Extension ?? "default");
        }));
      if (app.appLifeWin != null)
        app.Dispatcher.Invoke((Action) (() => this.appLifeWin.Close()));
      app.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) (async () =>
      {
        FileWatcherHelper.Instance.UpdateState();
        if (Program.OpenFileOnLaunchResult != null && Program.OpenFileOnLaunchResult.FileOpened)
          return;
        if (await UpdateHelper.EndOfServicing())
        {
          this.Shutdown();
        }
        else
        {
          int num2 = await UpdateHelper.UpdateAndExit() ? 1 : 0;
        }
      }));
    }
  }

  public static bool AppLaunchEvents(string sourceFrom = "Launch")
  {
    GAManager.SendEvent(nameof (App), "Launch", "count", 1L);
    ConfigManager.GetOriginVersion();
    ConfigManager.GetInstallDate();
    IAPUtils.GetIAPProductsAsync();
    return true;
  }

  public static void OpenUpdateAdConfigWindow() => new UpdateAdConfigWindow().Show();

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/PDFLauncher;component/app.xaml", UriKind.Relative));
  }

  [STAThread]
  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public static void Main()
  {
    App app = new App();
    app.InitializeComponent();
    app.Run();
  }
}
