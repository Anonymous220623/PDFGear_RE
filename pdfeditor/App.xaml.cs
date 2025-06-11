// Decompiled with JetBrains decompiler
// Type: pdfeditor.App
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.AppTheme;
using CommomLib.Commom;
using CommomLib.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Xaml.Behaviors.Layout;
using Patagames.Pdf.Net;
using pdfeditor.Services;
using pdfeditor.Utils;
using pdfeditor.ViewModels;
using pdfeditor.Views;
using PDFKit;
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

#nullable enable
namespace pdfeditor;

public partial class App : Application
{
  private 
  #nullable disable
  SystemThemeListener systemThemeListener;
  private object lastException;
  private bool _contentLoaded;

  public static App Current => (App) Application.Current;

  public AppHotKeyManager AppHotKeyManager { get; private set; }

  public App()
  {
    AppIdHelper.RegisterAppUserModelId();
    SqliteUtils.InitializeDatabase();
    Common.SetAppDataFolder(UtilManager.GetAppDataPath());
    Common.Initialize(CultureInfoUtils.ActualAppLanguage, new Func<string>(UtilManager.GetProductName), new Func<string>(UtilManager.GetAppVersion), new Action<string, string, string, long>(CommomLib.Commom.GAManager.SendEvent), new Action<string>(CommomLib.Commom.Log.WriteLog));
    CultureInfoUtils.Initialize();
    pdfeditor.Properties.Resources.Culture = CultureInfo.CurrentUICulture;
    CommomLib.Properties.Resources.Culture = CultureInfo.CurrentUICulture;
    this.systemThemeListener = new SystemThemeListener(this.Dispatcher);
    this.systemThemeListener.ActualAppThemeChanged += (EventHandler) ((s, a) => ThemeResourceDictionary.GetForCurrentApp().Theme = this.GetCurrentActualAppTheme());
    WindowThemeHelper.Initialize();
    ProcessMessageHelper.RegisterMessageName("PDFgearAutoSave");
    ProcessMessageHelper.MessageReceived += new EventHandler<ProcessMessageReceivedEventArgs>(this.ProcessMessageHelper_MessageReceived);
  }

  private void ProcessMessageHelper_MessageReceived(
    object sender,
    ProcessMessageReceivedEventArgs e)
  {
    CommomLib.Commom.Log.WriteLog(e.Message);
    if (e.Message == "UpdateTheme")
    {
      this.Dispatcher.InvokeAsync((Action) (() => ThemeResourceDictionary.GetForCurrentApp().Theme = this.GetCurrentActualAppTheme()));
    }
    else
    {
      if (!(e.Message == "PDFgearAutoSave"))
        return;
      this.Dispatcher.InvokeAsync((Action) (() => pdfeditor.AutoSaveRestore.AutoSaveManager.GetInstance().TrySaveImmediately()), DispatcherPriority.Send);
    }
  }

  public string GetCurrentActualAppTheme()
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
    LaunchUtils.Initialize(e);
    DispatcherHelper.Initialize();
    ThemeResourceDictionary forCurrentApp = ThemeResourceDictionary.GetForCurrentApp();
    forCurrentApp.Theme = app.GetCurrentActualAppTheme();
    // ISSUE: reference to a compiler-generated method
    forCurrentApp.ActualThemeChanged += new EventHandler<ActualThemeChangedEventArgs>(app.\u003COnStartup\u003Eb__10_0);
    AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(app.CurrentDomain_UnhandledException);
    Application.Current.Dispatcher.UnhandledException += new DispatcherUnhandledExceptionEventHandler(app.Dispatcher_UnhandledException);
    CommomLib.Commom.TaskExceptionHelper.UnhandledException += new CommomLib.Commom.TaskUnhandledExceptionEventHandler(app.TaskExceptionHelper_UnhandledException);
    // ISSUE: reference to a compiler-generated method
    app.\u003C\u003En__0(e);
    ConfigManager.GetOriginVersion();
    ConfigManager.GetInstallDate();
    ConfigManager.setAppLaunchCount(ConfigManager.getAppLaunchCount() + 1L);
    IAPUtils.GetIAPProductsAsync();
    if (!string.IsNullOrWhiteSpace(SDKUtils.GetLibPath()))
      PdfCommon.Initialize(SDKUtils.GetLinceseKey(), SDKUtils.GetLibPath());
    Ioc.Default.ConfigureServices(App.ConfigureServices(e));
    RenderUtils.Init();
    app.AppHotKeyManager = new AppHotKeyManager();
    // ISSUE: reference to a compiler-generated method
    await app.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) new Action(app.\u003COnStartup\u003Eb__10_1));
  }

  private void ThemeChangedFadeOut()
  {
    if (!(this.MainWindow is MainView mainWindow) || !(mainWindow.Content is FrameworkElement content))
      return;
    AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer((Visual) content);
    if (adornerLayer == null)
      return;
    DpiScale dpi = VisualTreeHelper.GetDpi((Visual) mainWindow);
    RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int) (content.ActualWidth * dpi.PixelsPerDip), (int) (content.ActualHeight * dpi.PixelsPerDip), dpi.PixelsPerInchY, dpi.PixelsPerInchY, PixelFormats.Pbgra32);
    renderTargetBitmap.Render((Visual) content);
    Image image1 = new Image();
    image1.Width = content.ActualWidth;
    image1.Height = content.ActualHeight;
    image1.Source = (ImageSource) renderTargetBitmap;
    image1.Stretch = Stretch.None;
    Image image2 = image1;
    AdornerContainer adornerContainer1 = new AdornerContainer((UIElement) content);
    adornerContainer1.Width = content.ActualWidth;
    adornerContainer1.Height = content.ActualHeight;
    adornerContainer1.Child = (UIElement) image2;
    AdornerContainer adornerContainer = adornerContainer1;
    adornerLayer.Add((Adorner) adornerContainer);
    DoubleAnimation animation = new DoubleAnimation(0.0, (Duration) TimeSpan.FromSeconds(0.15));
    animation.Completed += (EventHandler) ((s1, a1) => adornerLayer.Remove((Adorner) adornerContainer));
    image2.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline) animation);
  }

  private void Dispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
  {
    if (e.Exception == this.lastException)
      return;
    this.lastException = (object) e.Exception;
    this.LogException((object) e.Exception);
  }

  private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
  {
    if (e.ExceptionObject == this.lastException)
      return;
    this.lastException = e.ExceptionObject;
    this.LogException(e.ExceptionObject);
  }

  private void TaskExceptionHelper_UnhandledException(CommomLib.Commom.TaskUnhandledExceptionEventArgs e)
  {
    if (e.ExceptionObject == this.lastException || e.ExceptionObject is OperationCanceledException)
      return;
    this.lastException = e.ExceptionObject;
    this.LogException(e.ExceptionObject);
  }

  private void LogException(object exceptionObj)
  {
    string logStr = "";
    if (exceptionObj is Exception exception)
    {
      logStr = exception.CreateUnhandledExceptionMessage();
      CommomLib.Commom.GAManager.SendEvent("Exception", "UnhandledException", $"{exception.GetType().Name}, {exception.Message}", 1L);
    }
    else if (exceptionObj != null)
      logStr = exceptionObj.ToString();
    CommomLib.Commom.Log.WriteLog(logStr);
  }

  private static IServiceProvider ConfigureServices(StartupEventArgs e)
  {
    ServiceCollection services = new ServiceCollection();
    string filePath = string.Empty;
    if (e.Args != null && e.Args.Length != 0 && e.Args[0] != "CreateNewFile")
      filePath = e.Args[0];
    services.AddSingleton<PdfThumbnailService>();
    services.AddSingleton<MainViewModel>((Func<IServiceProvider, MainViewModel>) (_ => new MainViewModel(filePath)));
    services.AddSingleton<AppSettingsViewModel>((Func<IServiceProvider, AppSettingsViewModel>) (_ => new AppSettingsViewModel()));
    return (IServiceProvider) services.BuildServiceProvider();
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    this.StartupUri = new Uri("/Views/MainView.xaml", UriKind.Relative);
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/app.xaml", UriKind.Relative));
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
