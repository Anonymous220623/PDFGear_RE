// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Menus.ToolbarSettings.ToolbarSettingItemModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Toolkit.Mvvm.ComponentModel;
using pdfeditor.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Models.Menus.ToolbarSettings;

public class ToolbarSettingItemModel : ObservableObject
{
  private const bool ApplyTransientValueToSelectedValue = true;
  private ToolbarSettingModel parent;
  private object selectedValue;
  private object transientValue;
  private bool inTransientScope;
  private ImageSource icon;
  private string caption;
  private string name;
  private ICommand command;
  private bool customCacheKey;
  private string configCacheKey;

  public ToolbarSettingItemModel(ContextMenuItemType type, string configCacheKey)
  {
    this.Type = type;
    this.configCacheKey = configCacheKey;
    this.customCacheKey = !string.IsNullOrEmpty(configCacheKey);
  }

  public ToolbarSettingItemModel(ContextMenuItemType type)
    : this(type, "")
  {
  }

  public ToolbarSettingModel Parent
  {
    get => this.parent;
    set
    {
      if (!this.SetProperty<ToolbarSettingModel>(ref this.parent, value, nameof (Parent)) || this.customCacheKey)
        return;
      this.configCacheKey = ToolbarSettingConfigHelper.BuildConfigKey(this.Id, this.Type);
    }
  }

  protected object NontransientSelectedValue
  {
    get => this.selectedValue;
    set
    {
      if (object.Equals(this.selectedValue, value))
        return;
      this.selectedValue = value;
      if (this.inTransientScope)
        return;
      this.RaiseSelectedValueChanged();
    }
  }

  public object SelectedValue
  {
    get => !this.inTransientScope ? this.selectedValue : this.transientValue;
    set
    {
      bool flag;
      if (this.inTransientScope)
      {
        flag = !object.Equals(this.transientValue, value);
        if (flag)
          this.transientValue = value;
        this.selectedValue = value;
      }
      else
      {
        flag = !object.Equals(this.selectedValue, value);
        if (flag)
          this.selectedValue = value;
      }
      if (!flag)
        return;
      this.RaiseSelectedValueChanged();
    }
  }

  public ImageSource Icon
  {
    get => this.icon;
    set => this.SetProperty<ImageSource>(ref this.icon, value, nameof (Icon));
  }

  public string Name
  {
    get => this.name;
    set => this.SetProperty<string>(ref this.name, value, nameof (Name));
  }

  public string Caption
  {
    get => this.caption;
    set => this.SetProperty<string>(ref this.caption, value, nameof (Caption));
  }

  public ICommand Command
  {
    get => this.command;
    set => this.SetProperty<ICommand>(ref this.command, value, nameof (Command));
  }

  public bool InTransientScope => this.inTransientScope;

  public ContextMenuItemType Type { get; }

  public ToolbarSettingId Id
  {
    get
    {
      ToolbarSettingId id = this.Parent?.Id;
      return (object) id != null ? id : ToolbarSettingId.None;
    }
  }

  public void ExecuteCommand() => this.Command?.Execute((object) this);

  protected virtual void OnSelectedValueChanged()
  {
  }

  private void RaiseSelectedValueChanged()
  {
    this.OnSelectedValueChanged();
    EventHandler selectedValueChanged = this.SelectedValueChanged;
    if (selectedValueChanged != null)
      selectedValueChanged((object) this, EventArgs.Empty);
    this.OnPropertyChanged("SelectedValue");
  }

  public event EventHandler SelectedValueChanged;

  public async Task SaveConfigAsync()
  {
    string configCacheKey = this.configCacheKey;
    if (string.IsNullOrEmpty(configCacheKey))
      return;
    Dictionary<string, string> dict = new Dictionary<string, string>();
    this.SaveConfigCore(dict);
    await ToolbarSettingConfigHelper.SaveConfigAsync(configCacheKey, dict).ConfigureAwait(false);
  }

  public async Task LoadConfigAsync()
  {
    string configCacheKey = this.configCacheKey;
    if (string.IsNullOrEmpty(configCacheKey))
      ;
    else
    {
      Dictionary<string, string> dict = await ToolbarSettingConfigHelper.LoadConfigAsync(configCacheKey).ConfigureAwait(false);
      if (dict == null)
        ;
      else
        await DispatcherHelper.RunAsync((Action) (() => this.ApplyConfigCore(dict)));
    }
  }

  protected virtual void SaveConfigCore(Dictionary<string, string> dict)
  {
  }

  protected virtual void ApplyConfigCore(Dictionary<string, string> dict)
  {
  }

  public void BeginTransient(object value)
  {
    if (!this.inTransientScope)
    {
      this.inTransientScope = true;
      this.transientValue = value;
      this.OnPropertyChanged("InTransientScope");
      this.OnPropertyChanged("SelectedValue");
      this.RaiseSelectedValueChanged();
    }
    else
      this.SelectedValue = value;
  }

  public void EndTransient()
  {
    if (!this.inTransientScope)
      return;
    this.inTransientScope = false;
    this.transientValue = (object) null;
    this.OnPropertyChanged("InTransientScope");
    this.OnPropertyChanged("SelectedValue");
    this.RaiseSelectedValueChanged();
  }

  public bool ApplyTransient(bool endTransient)
  {
    if (!this.inTransientScope)
      return false;
    this.selectedValue = this.transientValue;
    if (endTransient)
    {
      this.inTransientScope = false;
      this.transientValue = (object) null;
      this.OnPropertyChanged("InTransientScope");
      this.OnPropertyChanged("SelectedValue");
      this.RaiseSelectedValueChanged();
    }
    return true;
  }
}
