// Decompiled with JetBrains decompiler
// Type: CommomLib.AppTheme.ThemeResourceDictionary
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Threading;

#nullable disable
namespace CommomLib.AppTheme;

public class ThemeResourceDictionary : ResourceDictionary
{
  private ThemeResourceObservableDictionary themeResources;
  private string theme = "";
  private string actualTheme = "";
  private bool internalSet;
  private bool followMainThemeResourceTheme;
  private ResourceDictionary currentResource;

  public ThemeResourceDictionary()
  {
    this.themeResources = new ThemeResourceObservableDictionary();
    this.themeResources.ResetRequested += new EventHandler(this.ThemeResources_ResetRequested);
    this.themeResources.CollectionChanged += new NotifyCollectionChangedEventHandler(this.ThemeResources_CollectionChanged);
    if (this.MergedDictionaries is INotifyCollectionChanged mergedDictionaries)
      mergedDictionaries.CollectionChanged += new NotifyCollectionChangedEventHandler(this.MergedDictionaries_CollectionChanged);
    ThemeResourceDictionary.MainThemeResourceDictionaryHelper.OnNewThemeResourceDictionaryCreated();
  }

  public IDictionary ThemeResources => (IDictionary) this.themeResources;

  public string Theme
  {
    get => this.theme;
    set
    {
      if (!(this.theme != value))
        return;
      this.UpdateTheme(value);
    }
  }

  public string ActualTheme => this.actualTheme;

  public bool FollowMainThemeResourceTheme
  {
    get => this.followMainThemeResourceTheme;
    set
    {
      if (this.followMainThemeResourceTheme == value)
        return;
      this.followMainThemeResourceTheme = value;
      ThemeResourceDictionary.MainThemeResourceDictionaryHelper.MainThemeResourceDictionaryThemeChanged -= new EventHandler(this.OnMainResourceThemeChanged);
      if (!value)
        return;
      ThemeResourceDictionary.MainThemeResourceDictionaryHelper.MainThemeResourceDictionaryThemeChanged += new EventHandler(this.OnMainResourceThemeChanged);
      ThemeResourceDictionary forCurrentApp = ThemeResourceDictionary.GetForCurrentApp();
      if (forCurrentApp == null)
        return;
      this.Theme = forCurrentApp.Theme;
    }
  }

  private void ThemeResources_ResetRequested(object sender, EventArgs e)
  {
    lock (this.themeResources)
    {
      foreach (KeyValuePair<string, ResourceDictionary> themeResource in this.themeResources)
        this.RemoveResource(themeResource.Key, themeResource.Value);
    }
  }

  private void ThemeResources_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    string theme = this.theme;
    if (e.Action == NotifyCollectionChangedAction.Add)
    {
      foreach (KeyValuePair<string, ResourceDictionary> keyValuePair in e.NewItems.OfType<KeyValuePair<string, ResourceDictionary>>())
        this.AddResource(keyValuePair.Key, keyValuePair.Value);
    }
    else if (e.Action == NotifyCollectionChangedAction.Replace)
    {
      IEnumerable<KeyValuePair<string, ResourceDictionary>> keyValuePairs1 = e.OldItems.OfType<KeyValuePair<string, ResourceDictionary>>();
      IEnumerable<KeyValuePair<string, ResourceDictionary>> keyValuePairs2 = e.NewItems.OfType<KeyValuePair<string, ResourceDictionary>>();
      foreach (KeyValuePair<string, ResourceDictionary> keyValuePair in keyValuePairs1)
        this.RemoveResource(keyValuePair.Key, keyValuePair.Value);
      foreach (KeyValuePair<string, ResourceDictionary> keyValuePair in keyValuePairs2)
        this.AddResource(keyValuePair.Key, keyValuePair.Value);
    }
    else
    {
      if (e.Action != NotifyCollectionChangedAction.Remove)
        return;
      foreach (KeyValuePair<string, ResourceDictionary> keyValuePair in e.OldItems.OfType<KeyValuePair<string, ResourceDictionary>>())
        this.RemoveResource(keyValuePair.Key, keyValuePair.Value);
    }
  }

  private void MergedDictionaries_CollectionChanged(
    object sender,
    NotifyCollectionChangedEventArgs e)
  {
    if (!this.internalSet)
      throw new NotSupportedException();
  }

  private void AddResource(string key, ResourceDictionary resource)
  {
    if (resource == null)
      return;
    lock (this.themeResources)
    {
      if (!(this.theme == key))
        return;
      this.UpdateTheme(key);
    }
  }

  private void RemoveResource(string key, ResourceDictionary resource)
  {
    if (resource == null)
      return;
    lock (this.themeResources)
    {
      if (!(this.theme == key))
        return;
      this.UpdateTheme("");
      this.theme = key;
    }
  }

  private void UpdateTheme(string key)
  {
    if (key == null)
      throw new ArgumentNullException((string) null, nameof (key));
    string theme = this.theme;
    try
    {
      this.internalSet = true;
      lock (this.themeResources)
      {
        string actualTheme = this.actualTheme;
        ResourceDictionary currentResource = this.currentResource;
        ResourceDictionary resourceDictionary;
        this.themeResources.TryGetValue(key, out resourceDictionary);
        this.theme = key;
        if (resourceDictionary != null)
        {
          this.actualTheme = key;
          this.currentResource = resourceDictionary;
        }
        else
        {
          this.actualTheme = "";
          this.currentResource = (ResourceDictionary) null;
        }
        try
        {
          if (actualTheme != this.actualTheme)
            this.OnActualThemeChanged(actualTheme, this.actualTheme);
        }
        finally
        {
          if (currentResource != resourceDictionary)
          {
            if (currentResource != null)
              this.MergedDictionaries.Remove(currentResource);
            if (resourceDictionary != null)
              this.MergedDictionaries.Insert(0, resourceDictionary);
          }
        }
      }
    }
    finally
    {
      this.internalSet = false;
    }
    if (!(theme != this.theme))
      return;
    this.OnThemeChanged();
  }

  internal event EventHandler ThemeChanged;

  public event EventHandler<ActualThemeChangedEventArgs> ActualThemeChanged;

  private void OnActualThemeChanged(string oldTheme, string newTheme)
  {
    EventHandler<ActualThemeChangedEventArgs> actualThemeChanged = this.ActualThemeChanged;
    if (actualThemeChanged == null)
      return;
    actualThemeChanged((object) this, new ActualThemeChangedEventArgs(oldTheme, newTheme));
  }

  private void OnThemeChanged()
  {
    EventHandler themeChanged = this.ThemeChanged;
    if (themeChanged == null)
      return;
    themeChanged((object) this, EventArgs.Empty);
  }

  private void OnMainResourceThemeChanged(object sender, EventArgs e)
  {
    ThemeResourceDictionary forCurrentApp = ThemeResourceDictionary.GetForCurrentApp();
    if (forCurrentApp == null)
      return;
    this.Theme = forCurrentApp.Theme;
  }

  public static ResourceDictionary GetCurrentResource(ThemeResourceDictionary resourceDictionary)
  {
    return resourceDictionary?.currentResource;
  }

  public static ThemeResourceDictionary GetForCurrentApp()
  {
    return ThemeResourceDictionary.MainThemeResourceDictionaryHelper.MainThemeResourceDictionary;
  }

  public static ThemeResourceDictionary GetForApp(Application app)
  {
    return ThemeResourceDictionary.GetFromResource(app?.Resources, true);
  }

  public static ThemeResourceDictionary GetFromResource(ResourceDictionary resourceDictionary)
  {
    return ThemeResourceDictionary.GetFromResource(resourceDictionary, false);
  }

  private static ThemeResourceDictionary GetFromResource(
    ResourceDictionary resourceDictionary,
    bool onlyMainResource)
  {
    if (resourceDictionary == null)
      return (ThemeResourceDictionary) null;
    if (resourceDictionary is ThemeResourceDictionary fromResource1 && (!onlyMainResource || !fromResource1.FollowMainThemeResourceTheme))
      return fromResource1;
    foreach (ResourceDictionary mergedDictionary in resourceDictionary.MergedDictionaries)
    {
      if (mergedDictionary is ThemeResourceDictionary fromResource3)
      {
        if (!onlyMainResource || !fromResource3.FollowMainThemeResourceTheme)
          return fromResource3;
      }
      else
      {
        ThemeResourceDictionary fromResource2 = ThemeResourceDictionary.GetFromResource(mergedDictionary, onlyMainResource);
        if (fromResource2 != null)
          return fromResource2;
      }
    }
    return (ThemeResourceDictionary) null;
  }

  internal static class MainThemeResourceDictionaryHelper
  {
    private static object locker = new object();
    private static Dispatcher dispatcher;
    private static EventHandler _MainThemeResourceDictionaryThemeChanged;
    private static ThemeResourceDictionary mainThemeResourceDictionary;
    private static Timer timer;

    internal static event EventHandler MainThemeResourceDictionaryThemeChanged
    {
      add
      {
        ThemeResourceDictionary.MainThemeResourceDictionaryHelper.ThemeChangedWeakEventManager.AddHandler(value);
      }
      remove
      {
        ThemeResourceDictionary.MainThemeResourceDictionaryHelper.ThemeChangedWeakEventManager.RemoveHandler(value);
      }
    }

    internal static ThemeResourceDictionary MainThemeResourceDictionary
    {
      get
      {
        if (ThemeResourceDictionary.MainThemeResourceDictionaryHelper.mainThemeResourceDictionary == null)
        {
          lock (ThemeResourceDictionary.MainThemeResourceDictionaryHelper.locker)
          {
            if (ThemeResourceDictionary.MainThemeResourceDictionaryHelper.mainThemeResourceDictionary == null)
              ThemeResourceDictionary.MainThemeResourceDictionaryHelper.MainThemeResourceDictionary = ThemeResourceDictionary.GetForApp(Application.Current);
          }
        }
        return ThemeResourceDictionary.MainThemeResourceDictionaryHelper.mainThemeResourceDictionary;
      }
      private set
      {
        ThemeResourceDictionary.MainThemeResourceDictionaryHelper.timer?.Stop();
        if (ThemeResourceDictionary.MainThemeResourceDictionaryHelper.mainThemeResourceDictionary == value)
          return;
        lock (ThemeResourceDictionary.MainThemeResourceDictionaryHelper.locker)
        {
          string str = "";
          if (ThemeResourceDictionary.MainThemeResourceDictionaryHelper.mainThemeResourceDictionary != null)
          {
            str = ThemeResourceDictionary.MainThemeResourceDictionaryHelper.mainThemeResourceDictionary.Theme;
            ThemeResourceDictionary.MainThemeResourceDictionaryHelper.mainThemeResourceDictionary.ThemeChanged -= new EventHandler(ThemeResourceDictionary.MainThemeResourceDictionaryHelper.MainThemeResourceDictionary_ThemeChanged);
          }
          ThemeResourceDictionary.MainThemeResourceDictionaryHelper.mainThemeResourceDictionary = value;
          if (ThemeResourceDictionary.MainThemeResourceDictionaryHelper.mainThemeResourceDictionary != null)
            ThemeResourceDictionary.MainThemeResourceDictionaryHelper.mainThemeResourceDictionary.ThemeChanged += new EventHandler(ThemeResourceDictionary.MainThemeResourceDictionaryHelper.MainThemeResourceDictionary_ThemeChanged);
          if (!(value?.Theme != str))
            return;
          EventHandler dictionaryThemeChanged = ThemeResourceDictionary.MainThemeResourceDictionaryHelper._MainThemeResourceDictionaryThemeChanged;
          if (dictionaryThemeChanged == null)
            return;
          dictionaryThemeChanged((object) null, EventArgs.Empty);
        }
      }
    }

    private static void MainThemeResourceDictionary_ThemeChanged(object sender, EventArgs e)
    {
      EventHandler dictionaryThemeChanged = ThemeResourceDictionary.MainThemeResourceDictionaryHelper._MainThemeResourceDictionaryThemeChanged;
      if (dictionaryThemeChanged == null)
        return;
      dictionaryThemeChanged((object) null, EventArgs.Empty);
    }

    internal static async void OnNewThemeResourceDictionaryCreated()
    {
      if (await ThemeResourceDictionary.MainThemeResourceDictionaryHelper.UpdateThemeResourceDictionaryInternal())
        return;
      if (ThemeResourceDictionary.MainThemeResourceDictionaryHelper.timer == null)
      {
        lock (ThemeResourceDictionary.MainThemeResourceDictionaryHelper.locker)
        {
          if (ThemeResourceDictionary.MainThemeResourceDictionaryHelper.timer == null)
          {
            ThemeResourceDictionary.MainThemeResourceDictionaryHelper.timer = new Timer()
            {
              Interval = 1000.0,
              AutoReset = false
            };
            ThemeResourceDictionary.MainThemeResourceDictionaryHelper.timer.Elapsed += new ElapsedEventHandler(ThemeResourceDictionary.MainThemeResourceDictionaryHelper.Timer_Elapsed);
          }
        }
      }
      ThemeResourceDictionary.MainThemeResourceDictionaryHelper.timer.Start();
    }

    private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
    {
      ThemeResourceDictionary.MainThemeResourceDictionaryHelper.UpdateThemeResourceDictionaryInternal();
    }

    private static async Task<bool> UpdateThemeResourceDictionaryInternal()
    {
      ThemeResourceDictionary.MainThemeResourceDictionaryHelper.timer?.Stop();
      if (ThemeResourceDictionary.MainThemeResourceDictionaryHelper.dispatcher == null)
      {
        lock (ThemeResourceDictionary.MainThemeResourceDictionaryHelper.locker)
        {
          if (ThemeResourceDictionary.MainThemeResourceDictionaryHelper.dispatcher == null)
            ThemeResourceDictionary.MainThemeResourceDictionaryHelper.dispatcher = Application.Current.Dispatcher;
        }
      }
      return await ThemeResourceDictionary.MainThemeResourceDictionaryHelper.dispatcher.InvokeAsync<bool>((Func<bool>) (() =>
      {
        if (ThemeResourceDictionary.MainThemeResourceDictionaryHelper.dispatcher.HasShutdownStarted)
          return true;
        lock (ThemeResourceDictionary.MainThemeResourceDictionaryHelper.locker)
        {
          ThemeResourceDictionary forApp = ThemeResourceDictionary.GetForApp(Application.Current);
          if (forApp != ThemeResourceDictionary.MainThemeResourceDictionaryHelper.mainThemeResourceDictionary)
          {
            ThemeResourceDictionary.MainThemeResourceDictionaryHelper.MainThemeResourceDictionary = forApp;
            return true;
          }
        }
        return false;
      }), DispatcherPriority.Normal);
    }

    private class ThemeChangedWeakEventManager : WeakEventManager
    {
      private ThemeChangedWeakEventManager()
      {
      }

      public static void AddHandler(EventHandler handler)
      {
        if (handler == null)
          throw new ArgumentNullException(nameof (handler));
        ThemeResourceDictionary.MainThemeResourceDictionaryHelper.ThemeChangedWeakEventManager.CurrentManager.ProtectedAddHandler((object) null, (Delegate) handler);
      }

      public static void RemoveHandler(EventHandler handler)
      {
        if (handler == null)
          throw new ArgumentNullException(nameof (handler));
        ThemeResourceDictionary.MainThemeResourceDictionaryHelper.ThemeChangedWeakEventManager.CurrentManager.ProtectedRemoveHandler((object) null, (Delegate) handler);
      }

      private static ThemeResourceDictionary.MainThemeResourceDictionaryHelper.ThemeChangedWeakEventManager CurrentManager
      {
        get
        {
          Type managerType = typeof (ThemeResourceDictionary.MainThemeResourceDictionaryHelper.ThemeChangedWeakEventManager);
          ThemeResourceDictionary.MainThemeResourceDictionaryHelper.ThemeChangedWeakEventManager manager = (ThemeResourceDictionary.MainThemeResourceDictionaryHelper.ThemeChangedWeakEventManager) WeakEventManager.GetCurrentManager(managerType);
          if (manager == null)
          {
            manager = new ThemeResourceDictionary.MainThemeResourceDictionaryHelper.ThemeChangedWeakEventManager();
            WeakEventManager.SetCurrentManager(managerType, (WeakEventManager) manager);
          }
          return manager;
        }
      }

      protected override WeakEventManager.ListenerList NewListenerList()
      {
        return new WeakEventManager.ListenerList();
      }

      protected override void StartListening(object source)
      {
        ThemeResourceDictionary.MainThemeResourceDictionaryHelper._MainThemeResourceDictionaryThemeChanged += new EventHandler(this.OnSomeEvent);
      }

      protected override void StopListening(object source)
      {
        ThemeResourceDictionary.MainThemeResourceDictionaryHelper._MainThemeResourceDictionaryThemeChanged -= new EventHandler(this.OnSomeEvent);
      }

      private void OnSomeEvent(object sender, EventArgs e) => this.DeliverEvent(sender, e);
    }
  }
}
