// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.HotKeys.HotKeyModel
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

#nullable disable
namespace CommomLib.Commom.HotKeys;

public class HotKeyModel : INotifyPropertyChanged
{
  private readonly HotKeyItem defaultHotKey;
  private HotKeyItem hotKeyItem;
  private string displayName;
  private bool isRegistered;
  private bool isReadOnly;
  private ICommand command;
  private bool isVisible = true;
  private bool isEnabled = true;
  private bool allowRepeat;

  internal HotKeyModel(string name)
    : this(name, new HotKeyItem())
  {
  }

  internal HotKeyModel(string name, HotKeyItem defaultHotKey)
  {
    this.Name = name;
    this.defaultHotKey = defaultHotKey;
    WeakReference<HotKeyModel> weakThis = new WeakReference<HotKeyModel>(this);
    EventHandler<HotKeyInvokedEventArgs> func1 = (EventHandler<HotKeyInvokedEventArgs>) null;
    EventHandler<HotKeyChangedEventArgs> func2 = (EventHandler<HotKeyChangedEventArgs>) null;
    func1 = (EventHandler<HotKeyInvokedEventArgs>) ((_sender, _e) =>
    {
      HotKeyModel target;
      if (weakThis.TryGetTarget(out target))
        target.OnHotKeyInvoked(_sender, _e);
      else
        HotKeyListener.HotKeyInvoked -= func1;
    });
    func2 = (EventHandler<HotKeyChangedEventArgs>) ((_sender, _e) =>
    {
      HotKeyModel target;
      if (weakThis.TryGetTarget(out target))
        target.OnHotKeyChanged(_sender, _e);
      else
        HotKeyListener.HotKeyChanged -= func2;
    });
    HotKeyListener.HotKeyInvoked += func1;
    HotKeyListener.HotKeyChanged += func2;
    this.LoadSetting();
  }

  public string Name { get; }

  public string DisplayName
  {
    get => this.displayName;
    set
    {
      if (object.Equals((object) this.displayName, (object) value))
        return;
      this.displayName = value;
      this.OnPropertyChanged(nameof (DisplayName));
    }
  }

  public bool IsRegistered
  {
    get => this.isRegistered;
    private set
    {
      if (this.isRegistered == value)
        return;
      this.isRegistered = value;
      this.OnPropertyChanged(nameof (IsRegistered));
    }
  }

  public bool IsReadOnly
  {
    get => this.isReadOnly;
    set
    {
      if (this.isReadOnly == value)
        return;
      this.isReadOnly = value;
      this.OnPropertyChanged(nameof (IsReadOnly));
    }
  }

  public bool IsVisible
  {
    get => this.isVisible;
    set
    {
      if (this.isVisible == value)
        return;
      this.isVisible = value;
      this.OnPropertyChanged(nameof (IsVisible));
    }
  }

  public bool IsEnabled
  {
    get => this.isEnabled;
    set
    {
      if (this.isEnabled == value)
        return;
      this.isEnabled = value;
      this.OnPropertyChanged(nameof (IsEnabled));
    }
  }

  public bool AllowRepeat
  {
    get => this.allowRepeat;
    set
    {
      if (this.allowRepeat == value)
        return;
      this.allowRepeat = value;
      this.OnPropertyChanged(nameof (AllowRepeat));
    }
  }

  public HotKeyItem HotKeyItem
  {
    get => this.hotKeyItem;
    set
    {
      if (object.Equals((object) this.hotKeyItem, (object) value))
        return;
      HotKeyListener.Unset(this.Name);
      this.hotKeyItem = value;
      if (HotKeyManager.ContainsKey(this.Name))
      {
        if (this.hotKeyItem.IsSupported)
          this.SaveSetting();
        this.IsRegistered = HotKeyListener.Set(this.hotKeyItem, this.Name);
      }
      else
        this.IsRegistered = false;
      this.OnPropertyChanged(nameof (HotKeyItem));
      this.OnPropertyChanged("HotKeyDisplayText");
    }
  }

  public ICommand Command
  {
    get => this.command;
    set
    {
      if (object.Equals((object) this.command, (object) value))
        return;
      this.command = value;
    }
  }

  internal bool IsHotKeyModelEnabled(bool isRepeat)
  {
    return this.IsEnabled && (!isRepeat || this.AllowRepeat);
  }

  internal void ResetToDefault()
  {
    HotKeyListener.Unset(this.Name);
    this.hotKeyItem = this.defaultHotKey;
    this.isRegistered = HotKeyManager.ContainsKey(this.Name) && HotKeyListener.Set(this.hotKeyItem, this.Name);
    this.OnPropertyChanged("HotKeyItem");
    this.OnPropertyChanged("IsRegistered");
    this.OnPropertyChanged("HotKeyDisplayText");
  }

  public string HotKeyDisplayText => this.HotKeyItem.ToString();

  private void OnPropertyChanged([CallerMemberName] string propertyName = null)
  {
    PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
    if (propertyChanged == null)
      return;
    propertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
  }

  public event PropertyChangedEventHandler PropertyChanged;

  public event EventHandler<HotKeyInvokedEventArgs> HotKeyInvoked;

  private void OnHotKeyInvoked(object sender, HotKeyInvokedEventArgs e)
  {
    if (!(e.HotKeyName == this.Name) || !this.isRegistered || !HotKeyManager.ContainsKey(this.Name) || !this.IsHotKeyModelEnabled(e.IsRepeat))
      return;
    ICommand command = this.Command;
    if (command != null)
    {
      e.Handled = true;
      command.Execute((object) null);
    }
    EventHandler<HotKeyInvokedEventArgs> hotKeyInvoked = this.HotKeyInvoked;
    if (hotKeyInvoked == null)
      return;
    hotKeyInvoked((object) this, e);
  }

  private void OnHotKeyChanged(object sender, HotKeyChangedEventArgs e)
  {
    if (e.Action == HotKeyChangedAction.Clear)
    {
      this.isRegistered = false;
      this.hotKeyItem = new HotKeyItem();
    }
    else if (e.Action == HotKeyChangedAction.Unset)
    {
      if (e.Name == this.Name)
      {
        this.isRegistered = false;
        this.hotKeyItem = new HotKeyItem();
      }
      else if (e.HotKey == this.hotKeyItem)
        this.isRegistered = HotKeyManager.ContainsKey(this.Name) && HotKeyListener.Set(this.hotKeyItem, this.Name);
    }
    else if (e.Action == HotKeyChangedAction.Set)
    {
      if (e.Name == this.Name)
      {
        this.isRegistered = true;
        this.hotKeyItem = e.HotKey;
      }
      else if (e.HotKey == this.hotKeyItem)
        this.isRegistered = false;
    }
    this.OnPropertyChanged("HotKeyItem");
    this.OnPropertyChanged("IsRegistered");
    this.OnPropertyChanged("HotKeyDisplayText");
  }

  private void LoadSetting()
  {
    string str = "HOTKEY_" + this.Name;
    HotKeyItem hotKeyItem = new HotKeyItem();
    if (!hotKeyItem.IsSupported)
      hotKeyItem = this.defaultHotKey;
    if (!hotKeyItem.IsSupported)
      return;
    this.hotKeyItem = hotKeyItem;
    this.isRegistered = HotKeyListener.Set(this.hotKeyItem, this.Name);
    this.OnPropertyChanged("HotKeyItem");
    this.OnPropertyChanged("IsRegistered");
    this.OnPropertyChanged("HotKeyDisplayText");
  }

  private void SaveSetting()
  {
  }
}
