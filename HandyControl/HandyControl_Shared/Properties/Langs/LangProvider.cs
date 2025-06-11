// Decompiled with JetBrains decompiler
// Type: HandyControl.Properties.Langs.LangProvider
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Tools;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

#nullable disable
namespace HandyControl.Properties.Langs;

public class LangProvider : INotifyPropertyChanged
{
  private static string CultureInfoStr;

  internal static LangProvider Instance { get; } = ResourceHelper.GetResourceInternal<LangProvider>("Langs");

  internal static CultureInfo Culture
  {
    get => Lang.Culture;
    set
    {
      if (value == null || object.Equals((object) LangProvider.CultureInfoStr, (object) value.EnglishName))
        return;
      Lang.Culture = value;
      LangProvider.CultureInfoStr = value.EnglishName;
      LangProvider.Instance.UpdateLangs();
    }
  }

  public static string GetLang(string key)
  {
    return Lang.ResourceManager.GetString(key, LangProvider.Culture);
  }

  public static void SetLang(
    DependencyObject dependencyObject,
    DependencyProperty dependencyProperty,
    string key)
  {
    BindingOperations.SetBinding(dependencyObject, dependencyProperty, (BindingBase) new Binding(key)
    {
      Source = (object) LangProvider.Instance,
      Mode = BindingMode.OneWay
    });
  }

  private void UpdateLangs()
  {
    this.OnPropertyChanged("All");
    this.OnPropertyChanged("Am");
    this.OnPropertyChanged("Cancel");
    this.OnPropertyChanged("Clear");
    this.OnPropertyChanged("Close");
    this.OnPropertyChanged("CloseAll");
    this.OnPropertyChanged("CloseOther");
    this.OnPropertyChanged("Confirm");
    this.OnPropertyChanged("ErrorImgPath");
    this.OnPropertyChanged("ErrorImgSize");
    this.OnPropertyChanged("Find");
    this.OnPropertyChanged("FormatError");
    this.OnPropertyChanged("Interval10m");
    this.OnPropertyChanged("Interval1h");
    this.OnPropertyChanged("Interval1m");
    this.OnPropertyChanged("Interval2h");
    this.OnPropertyChanged("Interval30m");
    this.OnPropertyChanged("Interval30s");
    this.OnPropertyChanged("Interval5m");
    this.OnPropertyChanged("IsNecessary");
    this.OnPropertyChanged("Jump");
    this.OnPropertyChanged("LangComment");
    this.OnPropertyChanged("Miscellaneous");
    this.OnPropertyChanged("NextPage");
    this.OnPropertyChanged("No");
    this.OnPropertyChanged("NoData");
    this.OnPropertyChanged("OutOfRange");
    this.OnPropertyChanged("PageMode");
    this.OnPropertyChanged("Pm");
    this.OnPropertyChanged("PngImg");
    this.OnPropertyChanged("PreviousPage");
    this.OnPropertyChanged("ScrollMode");
    this.OnPropertyChanged("Tip");
    this.OnPropertyChanged("TooLarge");
    this.OnPropertyChanged("TwoPageMode");
    this.OnPropertyChanged("Unknown");
    this.OnPropertyChanged("UnknownSize");
    this.OnPropertyChanged("Yes");
    this.OnPropertyChanged("ZoomIn");
    this.OnPropertyChanged("ZoomOut");
  }

  public string All => Lang.All;

  public string Am => Lang.Am;

  public string Cancel => Lang.Cancel;

  public string Clear => Lang.Clear;

  public string Close => Lang.Close;

  public string CloseAll => Lang.CloseAll;

  public string CloseOther => Lang.CloseOther;

  public string Confirm => Lang.Confirm;

  public string ErrorImgPath => Lang.ErrorImgPath;

  public string ErrorImgSize => Lang.ErrorImgSize;

  public string Find => Lang.Find;

  public string FormatError => Lang.FormatError;

  public string Interval10m => Lang.Interval10m;

  public string Interval1h => Lang.Interval1h;

  public string Interval1m => Lang.Interval1m;

  public string Interval2h => Lang.Interval2h;

  public string Interval30m => Lang.Interval30m;

  public string Interval30s => Lang.Interval30s;

  public string Interval5m => Lang.Interval5m;

  public string IsNecessary => Lang.IsNecessary;

  public string Jump => Lang.Jump;

  public string LangComment => Lang.LangComment;

  public string Miscellaneous => Lang.Miscellaneous;

  public string NextPage => Lang.NextPage;

  public string No => Lang.No;

  public string NoData => Lang.NoData;

  public string OutOfRange => Lang.OutOfRange;

  public string PageMode => Lang.PageMode;

  public string Pm => Lang.Pm;

  public string PngImg => Lang.PngImg;

  public string PreviousPage => Lang.PreviousPage;

  public string ScrollMode => Lang.ScrollMode;

  public string Tip => Lang.Tip;

  public string TooLarge => Lang.TooLarge;

  public string TwoPageMode => Lang.TwoPageMode;

  public string Unknown => Lang.Unknown;

  public string UnknownSize => Lang.UnknownSize;

  public string Yes => Lang.Yes;

  public string ZoomIn => Lang.ZoomIn;

  public string ZoomOut => Lang.ZoomOut;

  public event PropertyChangedEventHandler PropertyChanged;

  protected virtual void OnPropertyChanged(string propertyName)
  {
    PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
    if (propertyChanged == null)
      return;
    propertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
  }
}
