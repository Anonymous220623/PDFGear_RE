// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.ConfigHelper
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Properties.Langs;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

#nullable disable
namespace HandyControl.Tools;

public class ConfigHelper : INotifyPropertyChanged
{
  public static ConfigHelper Instance = new Lazy<ConfigHelper>((Func<ConfigHelper>) (() => new ConfigHelper())).Value;
  private XmlLanguage _lang = XmlLanguage.GetLanguage("zh-cn");

  private ConfigHelper()
  {
  }

  public XmlLanguage Lang
  {
    get => this._lang;
    set
    {
      if (this._lang.IetfLanguageTag.Equals(value.IetfLanguageTag))
        return;
      this._lang = value;
      this.OnPropertyChanged(nameof (Lang));
    }
  }

  public void SetLang(string lang)
  {
    LangProvider.Culture = new CultureInfo(lang);
    Application.Current.Dispatcher.Thread.CurrentUICulture = new CultureInfo(lang);
    this.Lang = XmlLanguage.GetLanguage(lang);
  }

  public void SetConfig(HandyControlConfig config)
  {
    this.SetLang(config.Lang);
    this.SetTimelineFrameRate(config.TimelineFrameRate);
  }

  public void SetTimelineFrameRate(int rate)
  {
    Timeline.DesiredFrameRateProperty.OverrideMetadata(typeof (Timeline), (PropertyMetadata) new FrameworkPropertyMetadata((object) rate));
  }

  public void SetWindowDefaultStyle(object resourceKey = null)
  {
    FrameworkPropertyMetadata typeMetadata = resourceKey == null ? new FrameworkPropertyMetadata(Application.Current.FindResource((object) typeof (Window))) : new FrameworkPropertyMetadata(Application.Current.FindResource(resourceKey));
    FrameworkElement.StyleProperty.OverrideMetadata(typeof (Window), (PropertyMetadata) typeMetadata);
  }

  public void SetNavigationWindowDefaultStyle(object resourceKey = null)
  {
    FrameworkPropertyMetadata typeMetadata = resourceKey == null ? new FrameworkPropertyMetadata(Application.Current.FindResource((object) typeof (NavigationWindow))) : new FrameworkPropertyMetadata(Application.Current.FindResource(resourceKey));
    FrameworkElement.StyleProperty.OverrideMetadata(typeof (NavigationWindow), (PropertyMetadata) typeMetadata);
  }

  public event PropertyChangedEventHandler PropertyChanged;

  protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
  {
    PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
    if (propertyChanged == null)
      return;
    propertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
  }
}
