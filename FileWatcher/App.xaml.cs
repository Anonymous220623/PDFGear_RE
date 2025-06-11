// Decompiled with JetBrains decompiler
// Type: FileWatcher.App
// Assembly: FileWatcher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 60845E26-CA8C-4ACF-A930-D757CD9B4993
// Assembly location: C:\Program Files\PDFgear\FileWatcher.exe

using CommomLib.AppTheme;
using CommomLib.Commom;
using FileWatcher.Views;
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;

#nullable disable
namespace FileWatcher;

public partial class App : System.Windows.Application
{
  private NotifyIcon notifyIcon;
  private Watcher watcher;
  private CancellationTokenSource openNotifyWindowCancellationToken;
  private object openNotifyWindowlocker = new object();
  private WindowMessageListener msgListener;
  private SystemThemeListener systemThemeListener;
  private bool _contentLoaded;

  internal Watcher Watcher => this.watcher;

  public static App Current => (App) System.Windows.Application.Current;

  public App()
  {
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
    this.Dispatcher.InvokeAsync((Action) (() => ThemeResourceDictionary.GetForCurrentApp().Theme = this.GetCurrentActualAppTheme()));
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

  protected override void OnStartup(StartupEventArgs e)
  {
    base.OnStartup(e);
    ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
    contextMenuStrip.Items.Add((ToolStripItem) new ToolStripMenuItem("Settings"));
    contextMenuStrip.Items.Add((ToolStripItem) new ToolStripMenuItem("Exit"));
    ThemeResourceDictionary.GetForCurrentApp().Theme = this.GetCurrentActualAppTheme();
    contextMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(this.ContextMenu_ItemClicked);
    this.watcher = new Watcher();
    this.watcher.FileCreated += new WatcherFileCreatedEventHandler(this.Watcher_FileCreated);
    this.watcher.FileRenamed += new WatcherFileRenamedEventHandler(this.Watcher_FileRenamed);
    this.UpdateListenFolders();
    this.msgListener = new WindowMessageListener();
    this.msgListener.MessageReceived += new MessageReceivedEventHandler(this.MsgListener_MessageReceived);
    GAManager2.SendEvent("FileWatcher", "FileWatcherServiceOn", "Count", 1L);
  }

  public void UpdateListenFolders()
  {
    if (this.Watcher == null)
      return;
    this.Watcher.Clear();
    foreach (string listenFolder in SettingsHelper.ListenFolders)
    {
      string path = "";
      switch (listenFolder)
      {
        case "Desktop":
          path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
          break;
        case "Downloads":
          path = KnownFolders.GetPath(KnownFolder.Downloads);
          break;
        case "Documents":
          path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
          break;
      }
      this.Watcher.AddPath(path, "*.pdf", true);
    }
  }

  private void MsgListener_MessageReceived(object sender, MessageReceivedEventArgs e)
  {
    switch (e.MessageData.Type)
    {
      case "exit":
        System.Windows.Application.Current.Shutdown();
        break;
      case "restart":
        SingleInstance.TryReleaseMutex();
        Process.Start(Process.GetCurrentProcess().MainModule.FileName);
        System.Windows.Application.Current.Shutdown();
        break;
    }
  }

  private void Watcher_FileCreated(Watcher sender, WatcherFileCreatedEventArgs args)
  {
    this.ProcessFile(args.CreatedFile);
  }

  private void Watcher_FileRenamed(Watcher sender, WatcherFileRenamedEventArgs args)
  {
    try
    {
      string oldFileName = args.OldFileName;
      string newFileName = args.NewFileName;
      if (!newFileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase) || oldFileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
        return;
      this.ProcessFile(newFileName);
    }
    catch
    {
    }
  }

  private void ProcessFile(string fileName)
  {
    if (string.IsNullOrEmpty(fileName))
      return;
    CancellationTokenSource tokenSource = (CancellationTokenSource) null;
    lock (this.openNotifyWindowlocker)
    {
      this.openNotifyWindowCancellationToken?.Cancel();
      tokenSource = new CancellationTokenSource();
      this.openNotifyWindowCancellationToken = tokenSource;
    }
    this.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) (async () =>
    {
      try
      {
        await Task.Delay(1000, tokenSource.Token);
      }
      catch
      {
      }
      if (tokenSource.IsCancellationRequested)
        return;
      try
      {
        if (this.Windows.OfType<NotifyView>().Count<NotifyView>() != 0 || NotificationHelper.IsFocusAssistEnabled || !File.Exists(fileName) || (File.GetAttributes(fileName) & (FileAttributes.Hidden | FileAttributes.System | FileAttributes.Temporary | FileAttributes.Offline)) != (FileAttributes) 0)
          return;
        using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
          if (fileStream.Length == 0L)
          {
            GAManager2.SendEvent("FileWatcher", "NotifyMsgSkipLen", "Count", 1L);
            return;
          }
        }
        new NotifyView(fileName).Show();
        GAManager2.SendEvent("FileWatcher", "NotifyMsg", "Count", 1L);
      }
      catch
      {
        GAManager2.SendEvent("FileWatcher", "NotifyMsgSkipExcept", "Count", 1L);
      }
    }));
  }

  private void ContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
  {
    ToolStripItem clickedItem = e.ClickedItem;
    if (clickedItem == null)
      return;
    switch (clickedItem.Text)
    {
      case "Settings":
        SettingsView settingsView = this.Windows.OfType<SettingsView>().FirstOrDefault<SettingsView>() ?? new SettingsView();
        settingsView.Show();
        settingsView.Activate();
        break;
      case "Exit":
        System.Windows.Application.Current.Shutdown();
        break;
    }
  }

  protected override void OnExit(ExitEventArgs e)
  {
    SingleInstance.TryReleaseMutex();
    this.msgListener.MessageReceived -= new MessageReceivedEventHandler(this.MsgListener_MessageReceived);
    this.msgListener.DestroyHandle();
    this.msgListener = (WindowMessageListener) null;
    this.watcher?.Dispose();
    this.watcher = (Watcher) null;
    base.OnExit(e);
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    System.Windows.Application.LoadComponent((object) this, new Uri("/FileWatcher;component/app.xaml", UriKind.Relative));
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
