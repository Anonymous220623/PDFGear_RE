// Decompiled with JetBrains decompiler
// Type: CommomLib.AppTheme.SystemThemeListener
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using Microsoft.Win32;
using System;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Threading;

#nullable disable
namespace CommomLib.AppTheme;

public class SystemThemeListener : IDisposable
{
  private bool disposedValue;
  private Dispatcher dispatcher;
  internal const uint DarkThemeColorRef = 1973790 /*0x1E1E1E*/;
  internal const uint LightThemeColorRef = 16777215 /*0xFFFFFF*/;

  public SystemThemeListener()
  {
    SystemThemeListener.StaticListener.AddRef();
    WeakEventManager<SystemThemeListener.StaticListener, EventArgs>.AddHandler((SystemThemeListener.StaticListener) null, "ActualSystemThemeChanged", new EventHandler<EventArgs>(this.OnSystemThemeChanged));
    WeakEventManager<SystemThemeListener.StaticListener, EventArgs>.AddHandler((SystemThemeListener.StaticListener) null, "ActualAppThemeChanged", new EventHandler<EventArgs>(this.OnAppThemeChanged));
    WeakEventManager<SystemThemeListener.StaticListener, EventArgs>.AddHandler((SystemThemeListener.StaticListener) null, "AccentColorChanged", new EventHandler<EventArgs>(this.OnAccentColorChanged));
  }

  public SystemThemeListener(Dispatcher dispatcher)
    : this()
  {
    this.dispatcher = dispatcher;
  }

  public SystemThemeListener.ActualTheme ActualSystemTheme
  {
    get => SystemThemeListener.StaticListener.ActualSystemTheme;
  }

  public SystemThemeListener.ActualTheme ActualAppTheme
  {
    get => SystemThemeListener.StaticListener.ActualAppTheme;
  }

  public System.Windows.Media.Color AccentColor => SystemThemeListener.StaticListener.AccentColor;

  internal System.Windows.Media.Color DarkThemeColor
  {
    get => System.Windows.Media.Color.FromArgb(byte.MaxValue, (byte) 30, (byte) 30, (byte) 30);
  }

  internal System.Windows.Media.Color LightThemeColor
  {
    get => System.Windows.Media.Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
  }

  internal uint CurrentThemeColorRef
  {
    get
    {
      return this.ActualAppTheme != SystemThemeListener.ActualTheme.Light ? 1973790U /*0x1E1E1E*/ : 16777215U /*0xFFFFFF*/;
    }
  }

  internal System.Windows.Media.Color CurrentThemeColor
  {
    get
    {
      return this.ActualAppTheme != SystemThemeListener.ActualTheme.Light ? this.DarkThemeColor : this.LightThemeColor;
    }
  }

  public event EventHandler ActualSystemThemeChanged;

  public event EventHandler ActualAppThemeChanged;

  public event EventHandler AccentColorChanged;

  private void OnSystemThemeChanged(object sender, EventArgs e)
  {
    this.EnqueueOrRun((Action) (() =>
    {
      EventHandler systemThemeChanged = this.ActualSystemThemeChanged;
      if (systemThemeChanged == null)
        return;
      systemThemeChanged((object) this, e);
    }));
  }

  private void OnAppThemeChanged(object sender, EventArgs e)
  {
    this.EnqueueOrRun((Action) (() =>
    {
      EventHandler actualAppThemeChanged = this.ActualAppThemeChanged;
      if (actualAppThemeChanged == null)
        return;
      actualAppThemeChanged((object) this, e);
    }));
  }

  private void OnAccentColorChanged(object sender, EventArgs e)
  {
    this.EnqueueOrRun((Action) (() =>
    {
      EventHandler accentColorChanged = this.AccentColorChanged;
      if (accentColorChanged == null)
        return;
      accentColorChanged((object) this, e);
    }));
  }

  private void EnqueueOrRun(Action action)
  {
    if (action == null)
      return;
    Dispatcher dispatcher = this.dispatcher;
    if (dispatcher != null)
      dispatcher.InvokeAsync(action);
    else
      action();
  }

  protected virtual void Dispose(bool disposing)
  {
    if (this.disposedValue)
      return;
    if (disposing)
    {
      WeakEventManager<SystemThemeListener.StaticListener, EventArgs>.RemoveHandler((SystemThemeListener.StaticListener) null, "ActualSystemThemeChanged", new EventHandler<EventArgs>(this.OnSystemThemeChanged));
      WeakEventManager<SystemThemeListener.StaticListener, EventArgs>.RemoveHandler((SystemThemeListener.StaticListener) null, "ActualAppThemeChanged", new EventHandler<EventArgs>(this.OnAppThemeChanged));
      WeakEventManager<SystemThemeListener.StaticListener, EventArgs>.RemoveHandler((SystemThemeListener.StaticListener) null, "AccentColorChanged", new EventHandler<EventArgs>(this.OnAccentColorChanged));
    }
    SystemThemeListener.StaticListener.ReleaseRef();
    this.disposedValue = true;
  }

  ~SystemThemeListener() => this.Dispose(false);

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }

  private class StaticListener
  {
    private static int globalRefCount = 0;
    private static object locker = new object();
    private static object updateLocker = new object();
    private static System.Timers.Timer timer;
    private static readonly bool SupportAppTheme = Environment.OSVersion.Version >= new Version(10, 0, 18362, 0);
    private static SystemThemeListener.ActualTheme DefaultTheme = SystemThemeListener.ActualTheme.Light;
    private static System.Windows.Media.Color DefaultAccentColor = System.Windows.Media.Color.FromArgb(byte.MaxValue, (byte) 239, (byte) 91, (byte) 82);
    private static SystemThemeListener.ActualTheme actualSystemTheme;
    private static SystemThemeListener.ActualTheme actualAppTheme;
    private static System.Windows.Media.Color accentColor;

    public static SystemThemeListener.ActualTheme ActualSystemTheme
    {
      get => SystemThemeListener.StaticListener.actualSystemTheme;
    }

    public static SystemThemeListener.ActualTheme ActualAppTheme
    {
      get => SystemThemeListener.StaticListener.actualAppTheme;
    }

    public static System.Windows.Media.Color AccentColor
    {
      get => SystemThemeListener.StaticListener.accentColor;
    }

    public static event EventHandler ActualSystemThemeChanged;

    public static event EventHandler ActualAppThemeChanged;

    public static event EventHandler AccentColorChanged;

    private static void SystemEvents_UserPreferenceChanged(
      object sender,
      UserPreferenceChangedEventArgs e)
    {
      if (e.Category != UserPreferenceCategory.General)
        return;
      SystemThemeListener.StaticListener.timer?.Stop();
      SystemThemeListener.StaticListener.timer?.Start();
    }

    private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
    {
      SystemThemeListener.StaticListener.UpdateThemeValues(true);
    }

    private static void UpdateThemeValues(bool invokeEvents)
    {
      SystemThemeListener.ActualTheme actualSystemTheme;
      SystemThemeListener.ActualTheme actualAppTheme;
      System.Windows.Media.Color accentColor;
      SystemThemeListener.ActualTheme actualTheme1;
      SystemThemeListener.ActualTheme actualTheme2;
      System.Windows.Media.Color color;
      lock (SystemThemeListener.StaticListener.updateLocker)
      {
        actualSystemTheme = SystemThemeListener.StaticListener.actualSystemTheme;
        actualAppTheme = SystemThemeListener.StaticListener.actualAppTheme;
        accentColor = SystemThemeListener.StaticListener.accentColor;
        SystemThemeListener.ActualTheme? currentActualTheme = SystemThemeListener.StaticListener.GetCurrentActualTheme("SystemUsesLightTheme");
        actualTheme1 = (SystemThemeListener.ActualTheme) ((int) currentActualTheme ?? (int) SystemThemeListener.StaticListener.DefaultTheme);
        actualTheme2 = actualTheme1;
        if (SystemThemeListener.StaticListener.SupportAppTheme)
        {
          currentActualTheme = SystemThemeListener.StaticListener.GetCurrentActualTheme("AppsUseLightTheme");
          actualTheme2 = (SystemThemeListener.ActualTheme) ((int) currentActualTheme ?? (int) SystemThemeListener.StaticListener.DefaultTheme);
        }
        color = SystemThemeListener.StaticListener.GetCurrentAccentColor() ?? SystemThemeListener.StaticListener.DefaultAccentColor;
      }
      if (actualSystemTheme != actualTheme1)
      {
        SystemThemeListener.StaticListener.actualSystemTheme = actualTheme1;
        if (invokeEvents)
        {
          EventHandler systemThemeChanged = SystemThemeListener.StaticListener.ActualSystemThemeChanged;
          if (systemThemeChanged != null)
            systemThemeChanged((object) null, EventArgs.Empty);
        }
      }
      if (actualAppTheme != actualTheme2)
      {
        SystemThemeListener.StaticListener.actualAppTheme = actualTheme2;
        if (invokeEvents)
        {
          EventHandler actualAppThemeChanged = SystemThemeListener.StaticListener.ActualAppThemeChanged;
          if (actualAppThemeChanged != null)
            actualAppThemeChanged((object) null, EventArgs.Empty);
        }
      }
      if (!(accentColor != color))
        return;
      SystemThemeListener.StaticListener.accentColor = color;
      if (!invokeEvents)
        return;
      EventHandler accentColorChanged = SystemThemeListener.StaticListener.AccentColorChanged;
      if (accentColorChanged == null)
        return;
      accentColorChanged((object) null, EventArgs.Empty);
    }

    private static SystemThemeListener.ActualTheme? GetCurrentActualTheme(string name)
    {
      try
      {
        using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize"))
        {
          if (registryKey != null)
          {
            if (registryKey.GetValue(name) == null)
              return new SystemThemeListener.ActualTheme?();
            return Convert.ToInt32(registryKey.GetValue(name)) != 0 ? new SystemThemeListener.ActualTheme?(SystemThemeListener.ActualTheme.Light) : new SystemThemeListener.ActualTheme?(SystemThemeListener.ActualTheme.Dark);
          }
        }
      }
      catch
      {
      }
      return new SystemThemeListener.ActualTheme?();
    }

    private static System.Windows.Media.Color? GetCurrentAccentColor()
    {
      try
      {
        using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\DWM"))
        {
          if (registryKey != null)
          {
            object obj = registryKey.GetValue("AccentColor");
            if (obj == null)
              return new System.Windows.Media.Color?();
            int int32 = Convert.ToInt32(obj);
            return new System.Windows.Media.Color?(System.Windows.Media.Color.FromArgb((byte) (int32 >> 24), (byte) int32, (byte) (int32 >> 8), (byte) (int32 >> 16 /*0x10*/)));
          }
        }
      }
      catch
      {
      }
      return new System.Windows.Media.Color?();
    }

    private static System.Windows.Media.Color ConvertToColor(int value)
    {
      return System.Windows.Media.Color.FromArgb((byte) (value >> 24), (byte) (value >> 16 /*0x10*/), (byte) (value >> 8), (byte) value);
    }

    public static void AddRef()
    {
      int globalRefCount;
      int num;
      do
      {
        globalRefCount = SystemThemeListener.StaticListener.globalRefCount;
        num = SystemThemeListener.StaticListener.globalRefCount + 1;
      }
      while (Interlocked.CompareExchange(ref SystemThemeListener.StaticListener.globalRefCount, num, globalRefCount) != globalRefCount);
      if (num != 1)
        return;
      lock (SystemThemeListener.StaticListener.locker)
      {
        if (SystemThemeListener.StaticListener.timer == null)
        {
          SystemThemeListener.StaticListener.timer = new System.Timers.Timer()
          {
            Interval = 500.0,
            AutoReset = false
          };
          SystemThemeListener.StaticListener.timer.Elapsed += new ElapsedEventHandler(SystemThemeListener.StaticListener.Timer_Elapsed);
        }
        else
          SystemThemeListener.StaticListener.timer.Stop();
        SystemThemeListener.StaticListener.UpdateThemeValues(false);
        SystemEvents.UserPreferenceChanged += new UserPreferenceChangedEventHandler(SystemThemeListener.StaticListener.SystemEvents_UserPreferenceChanged);
      }
    }

    public static void ReleaseRef()
    {
      int globalRefCount;
      int num;
      do
      {
        globalRefCount = SystemThemeListener.StaticListener.globalRefCount;
        num = SystemThemeListener.StaticListener.globalRefCount - 1;
      }
      while (Interlocked.CompareExchange(ref SystemThemeListener.StaticListener.globalRefCount, num, globalRefCount) != globalRefCount);
      if (num != 0)
        return;
      lock (SystemThemeListener.StaticListener.locker)
      {
        if (SystemThemeListener.StaticListener.timer != null)
        {
          SystemThemeListener.StaticListener.timer?.Stop();
          SystemThemeListener.StaticListener.timer.Elapsed -= new ElapsedEventHandler(SystemThemeListener.StaticListener.Timer_Elapsed);
          SystemThemeListener.StaticListener.timer.Dispose();
        }
        SystemEvents.UserPreferenceChanged -= new UserPreferenceChangedEventHandler(SystemThemeListener.StaticListener.SystemEvents_UserPreferenceChanged);
      }
    }
  }

  public enum ActualTheme
  {
    Dark,
    Light,
  }
}
