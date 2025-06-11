// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.FileWatcherHelper
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

#nullable disable
namespace CommomLib.Commom;

public class FileWatcherHelper : INotifyPropertyChanged
{
  public const bool FeatureEnabled = false;
  private const string FileWatcherMessengerWindowCaption = "FileWatcher_37CCD8B0-9B92-435E-88A1-79102B13E510";
  private static StartupTaskHelper startupTaskHelper = new StartupTaskHelper();
  private const int WM_COPYDATA = 74;
  private static CancellationTokenSource stateCts;
  private static object stateLocker = new object();
  private static FileWatcherHelper instance;
  private static object locker = new object();
  private bool isEnabled;

  public static FileWatcherHelper Instance
  {
    get
    {
      if (FileWatcherHelper.instance == null)
      {
        lock (FileWatcherHelper.locker)
        {
          if (FileWatcherHelper.instance == null)
            FileWatcherHelper.instance = new FileWatcherHelper();
        }
      }
      return FileWatcherHelper.instance;
    }
  }

  private FileWatcherHelper()
  {
    this.isEnabled = FileWatcherHelper.startupTaskHelper.IsStartupTaskEnabled;
  }

  public event PropertyChangedEventHandler PropertyChanged;

  public bool IsEnabled
  {
    get => this.isEnabled;
    set
    {
      if (this.isEnabled == value)
        return;
      this.isEnabled = value;
      GAManager.SendEvent("FileWatcher", "SettingsCheck", this.IsEnabled.ToString(), 1L);
      this.UpdateState();
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(nameof (IsEnabled)));
    }
  }

  public void Stop()
  {
    try
    {
      IntPtr window = FileWatcherHelper.FindWindow((string) null, "FileWatcher_37CCD8B0-9B92-435E-88A1-79102B13E510");
      if (!(window != IntPtr.Zero))
        return;
      string str = "0";
      try
      {
        str = $"{(long) new WindowInteropHelper(Application.Current.MainWindow).Handle}";
      }
      catch
      {
      }
      string message = JsonConvert.SerializeObject((object) new FileWatcherHelper.MessageData()
      {
        From = str,
        Type = "exit"
      }, Formatting.None);
      FileWatcherHelper.SendCopyDataMessage(window, message);
    }
    catch
    {
    }
  }

  public bool TryRestart()
  {
    if (this.IsEnabled)
    {
      IntPtr window = FileWatcherHelper.FindWindow((string) null, "FileWatcher_37CCD8B0-9B92-435E-88A1-79102B13E510");
      if (window != IntPtr.Zero)
      {
        string str = "0";
        try
        {
          str = $"{(long) new WindowInteropHelper(Application.Current.MainWindow).Handle}";
        }
        catch
        {
        }
        string message = JsonConvert.SerializeObject((object) new FileWatcherHelper.MessageData()
        {
          From = str,
          Type = "restart"
        }, Formatting.None);
        FileWatcherHelper.SendCopyDataMessage(window, message);
      }
    }
    return false;
  }

  public async void UpdateState(int delayMillis = 700)
  {
    CancellationTokenSource tokenSource = (CancellationTokenSource) null;
    lock (FileWatcherHelper.stateLocker)
    {
      FileWatcherHelper.stateCts?.Cancel();
      tokenSource = new CancellationTokenSource();
      FileWatcherHelper.stateCts = tokenSource;
    }
    try
    {
      if (delayMillis > 0)
        await Task.Delay(delayMillis, tokenSource.Token);
      if (tokenSource.IsCancellationRequested)
      {
        tokenSource = (CancellationTokenSource) null;
      }
      else
      {
        this.UpdateStateCore();
        tokenSource = (CancellationTokenSource) null;
      }
    }
    catch
    {
      tokenSource = (CancellationTokenSource) null;
    }
  }

  private void UpdateStateCore()
  {
    IntPtr window = FileWatcherHelper.FindWindow((string) null, "FileWatcher_37CCD8B0-9B92-435E-88A1-79102B13E510");
    if (this.IsEnabled)
    {
      FileWatcherHelper.startupTaskHelper.Enable();
      this.isEnabled = FileWatcherHelper.startupTaskHelper.IsStartupTaskEnabled;
      if (window == IntPtr.Zero)
      {
        string rootDirectoryFile = AppManager.GetRootDirectoryFile("FileWatcher.exe");
        if (File.Exists(rootDirectoryFile))
          Process.Start(new ProcessStartInfo(rootDirectoryFile));
      }
    }
    else
    {
      FileWatcherHelper.startupTaskHelper.Disable();
      this.isEnabled = FileWatcherHelper.startupTaskHelper.IsStartupTaskEnabled;
      if (window != IntPtr.Zero)
      {
        string str = "0";
        try
        {
          str = $"{(long) new WindowInteropHelper(Application.Current.MainWindow).Handle}";
        }
        catch
        {
        }
        string message = JsonConvert.SerializeObject((object) new FileWatcherHelper.MessageData()
        {
          From = str,
          Type = "exit"
        }, Formatting.None);
        FileWatcherHelper.SendCopyDataMessage(window, message);
      }
    }
    PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
    if (propertyChanged == null)
      return;
    propertyChanged((object) this, new PropertyChangedEventArgs("IsEnabled"));
  }

  public void Refresh()
  {
    IntPtr window = FileWatcherHelper.FindWindow((string) null, "FileWatcher_37CCD8B0-9B92-435E-88A1-79102B13E510");
    FileWatcherHelper.startupTaskHelper.Refresh();
    this.isEnabled = FileWatcherHelper.startupTaskHelper.IsStartupTaskEnabled;
    if (this.isEnabled != (window != IntPtr.Zero))
      this.UpdateState();
    PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
    if (propertyChanged == null)
      return;
    propertyChanged((object) this, new PropertyChangedEventArgs("IsEnabled"));
  }

  private static void SendCopyDataMessage(IntPtr hwnd, string message)
  {
    if (hwnd == IntPtr.Zero)
      return;
    FileWatcherHelper.COPYDATASTRUCT lParam = new FileWatcherHelper.COPYDATASTRUCT()
    {
      cbData = message.Length + 1,
      lpData = message + "\0"
    };
    FileWatcherHelper.SendMessage(hwnd, 74, IntPtr.Zero, ref lParam);
  }

  [DllImport("user32.dll", SetLastError = true)]
  private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

  [DllImport("user32.dll", SetLastError = true)]
  private static extern IntPtr SendMessage(
    IntPtr hWnd,
    int Msg,
    IntPtr wParam,
    ref FileWatcherHelper.COPYDATASTRUCT lParam);

  private class MessageData
  {
    public string From { get; set; }

    public string Type { get; set; }

    public string Msg { get; set; }
  }

  private struct COPYDATASTRUCT
  {
    public IntPtr dwData;
    public int cbData;
    [MarshalAs(UnmanagedType.LPStr)]
    public string lpData;
  }
}
