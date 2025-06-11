// Decompiled with JetBrains decompiler
// Type: pdfconverter.App
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using CommomLib.AppTheme;
using CommomLib.Commom;
using CommomLib.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Patagames.Pdf.Net;
using pdfconverter.ViewModels;
using pdfconverter.Views;
using Syncfusion.Licensing;
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows;

#nullable disable
namespace pdfconverter;

public partial class App : Application
{
  public static object convertType = (object) ConvToPDFType.ImageToPDF;
  public static string[] selectedFile;
  public static string[] seletedPassword;
  private SystemThemeListener systemThemeListener;
  private bool _contentLoaded;

  public App()
  {
    AppIdHelper.RegisterAppUserModelId();
    SqliteUtils.InitializeDatabase();
    CultureInfoUtils.Initialize();
    pdfconverter.Properties.Resources.Culture = CultureInfo.CurrentUICulture;
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
    if (!(e.Message == "UpdateTheme"))
      return;
    this.Dispatcher.InvokeAsync((System.Action) (() => ThemeResourceDictionary.GetForCurrentApp().Theme = this.GetCurrentActualAppTheme()));
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
    base.OnStartup(e);
    ThemeResourceDictionary.GetForCurrentApp().Theme = this.GetCurrentActualAppTheme();
    if (e.Args.Length > 2)
    {
      string str1 = e.Args[0];
      if (str1.Equals("app1", StringComparison.OrdinalIgnoreCase))
      {
        string str2 = e.Args[1];
        try
        {
          App.convertType = Enum.Parse(typeof (ConvFromPDFType), str2);
        }
        catch
        {
          int num = (int) MessageBox.Show("Failed to launch the application.", UtilManager.GetProductName());
          Application.Current.Shutdown();
          return;
        }
      }
      else if (str1.Equals("app2", StringComparison.OrdinalIgnoreCase))
      {
        string str3 = e.Args[1];
        try
        {
          App.convertType = Enum.Parse(typeof (ConvToPDFType), str3);
        }
        catch
        {
          int num = (int) MessageBox.Show("Failed to launch the application.", UtilManager.GetProductName());
          Application.Current.Shutdown();
          return;
        }
      }
      else
      {
        int num = (int) MessageBox.Show("Failed to launch the application.", UtilManager.GetProductName());
        Application.Current.Shutdown();
        return;
      }
      if (e.Args.Length > 2 && File.Exists(e.Args[2]))
      {
        FilesArgsModel filesArgsModel = DocsPathUtils.ReadFilesPathJson(e.Args[2]);
        if (filesArgsModel != null)
        {
          App.selectedFile = new string[filesArgsModel.FilesPath.Count];
          App.seletedPassword = new string[filesArgsModel.FilesPath.Count];
          for (int index = 0; index < filesArgsModel.FilesPath.Count; ++index)
          {
            App.selectedFile[index] = filesArgsModel.FilesPath[index].FilePath;
            App.seletedPassword[index] = filesArgsModel.FilesPath[index].Password == null ? "" : filesArgsModel.FilesPath[index].Password;
          }
        }
      }
    }
    if (!string.IsNullOrWhiteSpace(SDKUtils.GetLibPath()))
    {
      PdfCommon.Initialize(SDKUtils.GetLinceseKey(), SDKUtils.GetLibPath());
      GAManager.SendEvent(nameof (App), "LaunchConverter", "Count", 1L);
    }
    else
      GAManager.SendEvent(nameof (App), "LaunchConverter", "Error", 1L);
    SyncfusionLicenseProvider.RegisterLicense("NTQ0NTIxQDMxMzkyZTMzMmUzMEFOQnNrMC9aSWFVRHdYdTN3UFJPMDZKTTJaZ0UzV21LMG9BcXRGOVRvUTg9");
    SqliteUtils.InitializeDatabase();
    switch (App.convertType)
    {
      case ConvFromPDFType _:
        new pdfconverter.MainWindow().Show();
        break;
      case ConvToPDFType _:
        ServiceCollection services = new ServiceCollection();
        services.AddSingleton<MainWindow2ViewModel>(new MainWindow2ViewModel());
        services.AddSingleton<SplitPDFUCViewModel>(new SplitPDFUCViewModel());
        services.AddSingleton<MergePDFUCViewModel>(new MergePDFUCViewModel());
        services.AddSingleton<WordToPDFUCViewModel>(new WordToPDFUCViewModel());
        services.AddSingleton<ExcelToPDFUCViewModel>(new ExcelToPDFUCViewModel());
        services.AddSingleton<ImageToPDFUCViewModel>(new ImageToPDFUCViewModel());
        services.AddSingleton<RTFToPDFUCViewModel>(new RTFToPDFUCViewModel());
        services.AddSingleton<TXTToPDFUCViewModel>(new TXTToPDFUCViewModel());
        services.AddSingleton<PPTToPDFUCViewModel>(new PPTToPDFUCViewModel());
        services.AddSingleton<CompressPDFUCViewModel>(new CompressPDFUCViewModel());
        Ioc.Default.ConfigureServices((IServiceProvider) services.BuildServiceProvider());
        new MainWindow2().Show();
        if (App.selectedFile != null && App.selectedFile.Length != 0)
        {
          this.FileListInMainWindows2();
          break;
        }
        break;
      default:
        Application.Current.Shutdown();
        break;
    }
    if (await UpdateHelper.EndOfServicing())
    {
      Application.Current.Shutdown();
    }
    else
    {
      if (!UpdateHelper.ShouldShowUpdateDialog())
        return;
      int num = await UpdateHelper.UpdateAndExit() ? 1 : 0;
    }
  }

  private void Application_Startup(object sender, StartupEventArgs e)
  {
  }

  private void FileListInMainWindows2()
  {
    switch ((ConvToPDFType) App.convertType)
    {
      case ConvToPDFType.MergePDF:
        for (int index = 0; index <= App.selectedFile.Length - 1; ++index)
          Ioc.Default.GetRequiredService<MergePDFUCViewModel>().AddOneFileToMergeList(App.selectedFile[index], App.seletedPassword[index]);
        break;
      case ConvToPDFType.SplitPDF:
        for (int index = 0; index <= App.selectedFile.Length - 1; ++index)
          Ioc.Default.GetRequiredService<SplitPDFUCViewModel>().AddOneFileToSplitList(App.selectedFile[index], App.seletedPassword[index]);
        break;
      case ConvToPDFType.CompressPDF:
        for (int index = 0; index <= App.selectedFile.Length - 1; ++index)
          Ioc.Default.GetRequiredService<CompressPDFUCViewModel>().AddOneFileToFileList(App.selectedFile[index], App.seletedPassword[index]);
        break;
    }
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    this.Startup += new StartupEventHandler(this.Application_Startup);
    Application.LoadComponent((object) this, new Uri("/pdfconverter;component/app.xaml", UriKind.Relative));
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
