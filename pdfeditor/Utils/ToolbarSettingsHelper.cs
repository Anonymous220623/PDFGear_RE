// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.ToolbarSettingsHelper
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.AppTheme;
using CommomLib.Commom;
using Microsoft.Toolkit.Mvvm.Input;
using pdfeditor.Models.Menus;
using pdfeditor.Models.Menus.ToolbarSettings;
using pdfeditor.Properties;
using pdfeditor.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace pdfeditor.Utils;

public static class ToolbarSettingsHelper
{
  private static object locker = new object();
  private static ToolbarSettingsHelper.NestedDefaultValues _defaultValues;

  internal static ToolbarSettingsHelper.NestedDefaultValues DefaultValues
  {
    get
    {
      if (ToolbarSettingsHelper._defaultValues == null)
      {
        lock (ToolbarSettingsHelper.locker)
        {
          if (ToolbarSettingsHelper._defaultValues == null)
            ToolbarSettingsHelper._defaultValues = new ToolbarSettingsHelper.NestedDefaultValues();
        }
      }
      return ToolbarSettingsHelper._defaultValues;
    }
  }

  public static ToolbarSettingItemModel CreateColor(
    AnnotationMode mode,
    ContextMenuItemType type,
    Action<ToolbarSettingItemModel> action,
    ImageSource icon)
  {
    string recentKey = ToolbarSettingConfigHelper.BuildRecentColorKey(mode, type);
    return ToolbarSettingsHelper.CreateColor(mode, type, recentKey, action, icon);
  }

  public static ToolbarSettingItemModel CreateColor(
    AnnotationMode mode,
    ContextMenuItemType type,
    string recentKey,
    Action<ToolbarSettingItemModel> action,
    ImageSource icon)
  {
    IReadOnlyList<string> collection = (IReadOnlyList<string>) null;
    string defaultValue = (string) null;
    string str1 = "";
    string str2 = "";
    switch (type)
    {
      case ContextMenuItemType.StrokeColor:
        collection = ToolbarSettingsHelper.DefaultValues.GetStandardStokeColors(mode, out defaultValue);
        str1 = "Color";
        str2 = Resources.LabelColorContent;
        break;
      case ContextMenuItemType.FillColor:
        collection = ToolbarSettingsHelper.DefaultValues.GetStandardFillColors(mode, out defaultValue);
        str1 = "FillColor";
        str2 = Resources.MenuSubFillColorItem;
        break;
      case ContextMenuItemType.FontColor:
        collection = ToolbarSettingsHelper.DefaultValues.GetStandardFontColors(mode, out defaultValue);
        str1 = "FontColor";
        str2 = Resources.MenuSubFontColorItem;
        break;
    }
    if (collection == null)
      return (ToolbarSettingItemModel) null;
    ToolbarSettingItemColorModel color = new ToolbarSettingItemColorModel(type);
    color.Name = str1;
    color.Caption = str2;
    color.Icon = icon;
    color.Command = action != null ? (ICommand) new RelayCommand<ToolbarSettingItemModel>(action) : (ICommand) null;
    color.RecentColorsKey = recentKey;
    color.SelectedValue = (object) defaultValue;
    color.StandardColors = new ObservableCollection<string>((IEnumerable<string>) collection);
    return (ToolbarSettingItemModel) color;
  }

  private static ImageSource CreateIcon(AnnotationMode model)
  {
    ImageSource icon = (ImageSource) null;
    switch (model)
    {
      case AnnotationMode.Line:
        icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/line.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/line.png"));
        break;
      case AnnotationMode.Ink:
        icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/ink.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/ink.png"));
        break;
      case AnnotationMode.Shape:
        icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/shape.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/shape.png"));
        break;
      case AnnotationMode.Highlight:
        icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/highlighttext.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/highlighttext.png"));
        break;
      case AnnotationMode.Underline:
        icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/underline.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/underline.png"));
        break;
      case AnnotationMode.Strike:
        icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/strike.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/strike.png"));
        break;
      case AnnotationMode.HighlightArea:
        icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/highlightarea2.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/highlightarea2.png"));
        break;
      case AnnotationMode.Ellipse:
        icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/ellipse.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/ellipse.png"));
        break;
      case AnnotationMode.Text:
        icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/text.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/text.png"));
        break;
      case AnnotationMode.TextBox:
        icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/textbox.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/textbox.png"));
        break;
    }
    return icon;
  }

  public static ImageSource CreateIcon(ContextMenuItemType itemtype)
  {
    ImageSource icon = (ImageSource) null;
    switch (itemtype)
    {
      case ContextMenuItemType.StrokeColor:
        icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/strokecolor.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/strokecolor.png"));
        break;
      case ContextMenuItemType.FillColor:
        icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/fillcolor.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/fillcolor.png"));
        break;
      case ContextMenuItemType.StrokeThickness:
        icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/strokethickness.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/strokethickness.png"));
        break;
      case ContextMenuItemType.FontColor:
        icon = ThemeBitmapImage.CreateBitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/fontcolor.png"), new Uri("pack://application:,,,/Style/DarkModeResources/Annonate/fontcolor.png"));
        break;
    }
    return icon;
  }

  public static ToolbarSettingItemModel CreateCollapsedColor(
    AnnotationMode mode,
    ContextMenuItemType type,
    Action<ToolbarSettingItemModel> action,
    ImageSource icon)
  {
    string recentKey = ToolbarSettingConfigHelper.BuildRecentColorKey(mode, type);
    return ToolbarSettingsHelper.CreateCollapsedColor(mode, type, recentKey, action, icon);
  }

  public static ToolbarSettingItemModel CreateCollapsedColor(
    AnnotationMode mode,
    ContextMenuItemType type,
    string recentKey,
    Action<ToolbarSettingItemModel> action,
    ImageSource icon)
  {
    return ToolbarSettingsHelper.CreateCollapsedColor(ToolbarSettingId.CreateAnnotation(mode), type, recentKey, action, icon);
  }

  public static ToolbarSettingItemModel CreateCollapsedColor(
    ToolbarSettingId id,
    ContextMenuItemType type,
    Action<ToolbarSettingItemModel> action,
    ImageSource icon)
  {
    string recentKey = ToolbarSettingConfigHelper.BuildRecentColorKey(id, type);
    return ToolbarSettingsHelper.CreateCollapsedColor(id, type, recentKey, action, icon);
  }

  public static ToolbarSettingItemModel CreateCollapsedColor(
    ToolbarSettingId id,
    ContextMenuItemType type,
    string recentKey,
    Action<ToolbarSettingItemModel> action,
    ImageSource icon)
  {
    IReadOnlyList<string> collection = (IReadOnlyList<string>) null;
    string defaultValue = (string) null;
    string str1 = "";
    string str2 = "";
    switch (type)
    {
      case ContextMenuItemType.StrokeColor:
        collection = ToolbarSettingsHelper.DefaultValues.GetStandardStokeColors(id.AnnotationMode, out defaultValue);
        str1 = "Color";
        str2 = Resources.LabelColorContent;
        icon = icon ?? (ImageSource) new BitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/linecolor.png"));
        break;
      case ContextMenuItemType.FillColor:
        collection = ToolbarSettingsHelper.DefaultValues.GetStandardFillColors(id.AnnotationMode, out defaultValue);
        str1 = "FillColor";
        str2 = Resources.MenuSubFillColorItem;
        icon = icon ?? (ImageSource) new BitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/linecolor.png"));
        break;
      case ContextMenuItemType.FontColor:
        collection = ToolbarSettingsHelper.DefaultValues.GetStandardFontColors(id.AnnotationMode, out defaultValue);
        str1 = "FontColor";
        str2 = Resources.MenuSubFontColorItem;
        icon = icon ?? (ImageSource) new BitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/linecolor.png"));
        break;
    }
    if (collection == null)
      return (ToolbarSettingItemModel) null;
    ToolbarSettingItemColorCollapseModel collapsedColor = new ToolbarSettingItemColorCollapseModel(type);
    collapsedColor.Name = str1;
    collapsedColor.Caption = str2;
    collapsedColor.Icon = icon;
    collapsedColor.Command = action != null ? (ICommand) new RelayCommand<ToolbarSettingItemModel>(action) : (ICommand) null;
    collapsedColor.RecentColorsKey = recentKey;
    collapsedColor.SelectedValue = (object) defaultValue;
    collapsedColor.StandardColors = new ObservableCollection<string>((IEnumerable<string>) collection);
    return (ToolbarSettingItemModel) collapsedColor;
  }

  public static ToolbarSettingItemModel CreateStrokeThickness(
    AnnotationMode mode,
    Action<ToolbarSettingItemModel> action,
    ImageSource icon)
  {
    float defaultValue;
    IReadOnlyList<float> strokeThicknesses = ToolbarSettingsHelper.DefaultValues.GetStandardStrokeThicknesses(mode, out defaultValue);
    if (strokeThicknesses == null)
      return (ToolbarSettingItemModel) null;
    ToolbarSettingItemStrokeThicknessModel strokeThickness = new ToolbarSettingItemStrokeThicknessModel(ContextMenuItemType.StrokeThickness);
    strokeThickness.Name = "Thickness";
    strokeThickness.Caption = Resources.MenuSubThicknessItem;
    strokeThickness.Icon = icon ?? (ImageSource) new BitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/linewidth.png"));
    strokeThickness.Command = action != null ? (ICommand) new RelayCommand<ToolbarSettingItemModel>(action) : (ICommand) null;
    strokeThickness.SelectedValue = (object) defaultValue;
    strokeThickness.StandardItems = new ObservableCollection<float>((IEnumerable<float>) strokeThicknesses);
    return (ToolbarSettingItemModel) strokeThickness;
  }

  public static ToolbarSettingItemModel CreateFontSize(
    AnnotationMode mode,
    Action<ToolbarSettingItemModel> action,
    ImageSource icon)
  {
    float defaultValue;
    IReadOnlyList<float> standardFontSizes = ToolbarSettingsHelper.DefaultValues.GetStandardFontSizes(mode, out defaultValue);
    if (standardFontSizes == null)
      return (ToolbarSettingItemModel) null;
    ToolbarSettingItemFontSizeModel fontSize = new ToolbarSettingItemFontSizeModel(ContextMenuItemType.FontSize);
    fontSize.Name = "FontSize";
    fontSize.Caption = Resources.MenuSubFontSizeItem;
    fontSize.Icon = icon ?? (ImageSource) new BitmapImage(new Uri("pack://application:,,,/Style/Resources/Annonate/linecolor.png"));
    fontSize.Command = action != null ? (ICommand) new RelayCommand<ToolbarSettingItemModel>(action) : (ICommand) null;
    fontSize.SelectedValue = (object) defaultValue;
    fontSize.StandardItems = new ObservableCollection<float>((IEnumerable<float>) standardFontSizes);
    return (ToolbarSettingItemModel) fontSize;
  }

  public static ToolbarSettingItemModel CreateAnnotationModeIcon(AnnotationMode mode)
  {
    ToolbarSettingItemIconModel annotationModeIcon = new ToolbarSettingItemIconModel();
    annotationModeIcon.Name = "Icon";
    annotationModeIcon.Icon = ToolbarSettingsHelper.CreateIcon(mode);
    return (ToolbarSettingItemModel) annotationModeIcon;
  }

  public static ToolbarSettingItemIconModel CreateMenuItemTypeModelIcon(ContextMenuItemType itemtype)
  {
    ToolbarSettingItemIconModel itemTypeModelIcon = new ToolbarSettingItemIconModel();
    itemTypeModelIcon.Name = "Icon";
    itemTypeModelIcon.Caption = itemtype.ToString();
    itemTypeModelIcon.Icon = ToolbarSettingsHelper.CreateIcon(itemtype);
    return itemTypeModelIcon;
  }

  public static ToolbarSettingItemModel CreateExitEdit(Action<ToolbarSettingItemModel> action)
  {
    ToolbarSettingItemExitModel exitEdit = new ToolbarSettingItemExitModel();
    exitEdit.Name = "Exit";
    exitEdit.Command = action != null ? (ICommand) new RelayCommand<ToolbarSettingItemModel>(action) : (ICommand) null;
    return (ToolbarSettingItemModel) exitEdit;
  }

  public static ToolbarSettingItemModel CreateImageExitEdit(Action<ToolbarSettingItemModel> action)
  {
    return ToolbarSettingsHelper.CreateImageExitEdit((string) null, action);
  }

  public static ToolbarSettingItemModel CreateImageExitEdit(
    string text,
    Action<ToolbarSettingItemModel> action)
  {
    ToolbarSettingItemImageExitModel imageExitEdit = new ToolbarSettingItemImageExitModel(text);
    imageExitEdit.Text = text;
    imageExitEdit.Name = "Exit";
    imageExitEdit.Command = action != null ? (ICommand) new RelayCommand<ToolbarSettingItemModel>(action) : (ICommand) null;
    return (ToolbarSettingItemModel) imageExitEdit;
  }

  public static ToolbarSettingItemModel CreateText(string text)
  {
    ToolBarSettingTextBlock text1 = new ToolBarSettingTextBlock(text);
    text1.Text = text;
    text1.Name = "Text";
    return (ToolbarSettingItemModel) text1;
  }

  public static ToolbarSettingItemApplyToDefaultModel CreateApplyToDefault()
  {
    ToolbarSettingItemApplyToDefaultModel applyToDefault = new ToolbarSettingItemApplyToDefaultModel();
    applyToDefault.Name = "ApplyToDefault";
    return applyToDefault;
  }

  public static ToolbarSettingItemModel CreteEreserState(
    AnnotationMode mode,
    string Name,
    bool Ischeck,
    Action<ToolbarSettingItemModel> action)
  {
    int eraserSize = ConfigManager.GetEraserSize();
    ToolbarSettingInkEraserModel settingInkEraserModel = new ToolbarSettingInkEraserModel();
    settingInkEraserModel.Name = Name;
    settingInkEraserModel.IsCheckable = true;
    settingInkEraserModel.IsChecked = Ischeck;
    settingInkEraserModel.Command = action != null ? (ICommand) new RelayCommand<ToolbarSettingItemModel>(action) : (ICommand) null;
    settingInkEraserModel.Caption = Resources.InkToolbarEraserBtn;
    settingInkEraserModel.SelectSize = eraserSize;
    return (ToolbarSettingItemModel) settingInkEraserModel;
  }

  internal class NestedDefaultValues
  {
    private Dictionary<ContextMenuItemType, IDictionary> allValues;

    public NestedDefaultValues()
    {
      this.allValues = new Dictionary<ContextMenuItemType, IDictionary>();
      this.InitStandardStrokeColors();
      this.InitStandardFillColors();
      this.InitStandardStrokeThicknesses();
      this.InitStandardFontSizes();
      this.InitFontColors();
    }

    public IReadOnlyList<string> GetStandardStokeColors(
      AnnotationMode mode,
      out string defaultValue)
    {
      return this.GetValues<string>(mode, ContextMenuItemType.StrokeColor, out defaultValue);
    }

    public IReadOnlyList<string> GetStandardFillColors(AnnotationMode mode, out string defaultValue)
    {
      return this.GetValues<string>(mode, ContextMenuItemType.FillColor, out defaultValue);
    }

    public IReadOnlyList<float> GetStandardStrokeThicknesses(
      AnnotationMode mode,
      out float defaultValue)
    {
      return this.GetValues<float>(mode, ContextMenuItemType.StrokeThickness, out defaultValue);
    }

    public IReadOnlyList<float> GetStandardFontSizes(AnnotationMode mode, out float defaultValue)
    {
      return this.GetValues<float>(mode, ContextMenuItemType.FontSize, out defaultValue);
    }

    public IReadOnlyList<string> GetStandardFontColors(AnnotationMode mode, out string defaultValue)
    {
      return this.GetValues<string>(mode, ContextMenuItemType.FontColor, out defaultValue);
    }

    private IReadOnlyList<T> GetValues<T>(
      AnnotationMode mode,
      ContextMenuItemType type,
      out T defaultValue)
    {
      defaultValue = default (T);
      IDictionary dictionary1;
      if (!this.allValues.TryGetValue(type, out dictionary1) || !(dictionary1 is Dictionary<AnnotationMode, IReadOnlyList<ToolbarSettingsHelper.ValueProxy<T>>> dictionary2))
        return (IReadOnlyList<T>) null;
      IReadOnlyList<ToolbarSettingsHelper.ValueProxy<T>> valueProxyList;
      if (!dictionary2.TryGetValue(mode, out valueProxyList))
        valueProxyList = dictionary2[AnnotationMode.None];
      bool flag = false;
      List<T> values = new List<T>(valueProxyList.Count);
      for (int index = 0; index < valueProxyList.Count; ++index)
      {
        values.Add(valueProxyList[index].Value);
        if (!flag && valueProxyList[index].IsDefault)
        {
          defaultValue = valueProxyList[index].Value;
          flag = true;
        }
      }
      if (!flag && values.Count > 0)
        defaultValue = values[0];
      return (IReadOnlyList<T>) values;
    }

    private void InitStandardStrokeColors()
    {
      Dictionary<AnnotationMode, IReadOnlyList<ToolbarSettingsHelper.ValueProxy<string>>> dictionary = new Dictionary<AnnotationMode, IReadOnlyList<ToolbarSettingsHelper.ValueProxy<string>>>()
      {
        [AnnotationMode.None] = (IReadOnlyList<ToolbarSettingsHelper.ValueProxy<string>>) new ToolbarSettingsHelper.ValueProxy<string>[9]
        {
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FFFFFFFF"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FF000000"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FFFB302F", true),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FFFD9927"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FFFAFF00"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FFA5DE50"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FF43D9EF"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FF52AAEC"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FF9573E4")
        },
        [AnnotationMode.Highlight] = (IReadOnlyList<ToolbarSettingsHelper.ValueProxy<string>>) new ToolbarSettingsHelper.ValueProxy<string>[5]
        {
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FB302F"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FFA800"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FFF400", true),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#7AF256"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#21A0FF")
        }
      };
      dictionary[AnnotationMode.HighlightArea] = dictionary[AnnotationMode.Highlight];
      dictionary[AnnotationMode.Underline] = (IReadOnlyList<ToolbarSettingsHelper.ValueProxy<string>>) new ToolbarSettingsHelper.ValueProxy<string>[5]
      {
        ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FB302F", true),
        ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FFA800"),
        ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FFF400"),
        ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#7AF256"),
        ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#21A0FF")
      };
      dictionary[AnnotationMode.Strike] = dictionary[AnnotationMode.Underline];
      dictionary[AnnotationMode.Ink] = dictionary[AnnotationMode.Underline];
      this.allValues[ContextMenuItemType.StrokeColor] = (IDictionary) dictionary;
    }

    private void InitStandardFillColors()
    {
      Dictionary<AnnotationMode, IReadOnlyList<ToolbarSettingsHelper.ValueProxy<string>>> dictionary = new Dictionary<AnnotationMode, IReadOnlyList<ToolbarSettingsHelper.ValueProxy<string>>>()
      {
        [AnnotationMode.None] = (IReadOnlyList<ToolbarSettingsHelper.ValueProxy<string>>) new ToolbarSettingsHelper.ValueProxy<string>[9]
        {
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FFFFFFFF"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FF000000"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FFFB302F", true),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FFFD9927"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FFFAFF00"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FFA5DE50"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FF43D9EF"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FF52AAEC"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FF9573E4")
        },
        [AnnotationMode.Shape] = (IReadOnlyList<ToolbarSettingsHelper.ValueProxy<string>>) new ToolbarSettingsHelper.ValueProxy<string>[10]
        {
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#00FFFFFF", true),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FFFFFFFF"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FF000000"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FFFB302F"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FFFD9927"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FFFAFF00"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FFA5DE50"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FF43D9EF"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FF52AAEC"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FF9573E4")
        },
        [AnnotationMode.Link] = (IReadOnlyList<ToolbarSettingsHelper.ValueProxy<string>>) new ToolbarSettingsHelper.ValueProxy<string>[10]
        {
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#00FFFFFF", true),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FFFFFFFF"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FF000000"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FFFB302F"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FFFD9927"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FFFAFF00"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FFA5DE50"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FF43D9EF"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FF52AAEC"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FF9573E4")
        }
      };
      dictionary[AnnotationMode.Ellipse] = dictionary[AnnotationMode.Shape];
      dictionary[AnnotationMode.TextBox] = (IReadOnlyList<ToolbarSettingsHelper.ValueProxy<string>>) new ToolbarSettingsHelper.ValueProxy<string>[9]
      {
        ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FFFFFFFF", true),
        ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FF000000"),
        ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FFFB302F"),
        ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FFFD9927"),
        ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FFFAFF00"),
        ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FFA5DE50"),
        ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FF43D9EF"),
        ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FF52AAEC"),
        ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FF9573E4")
      };
      this.allValues[ContextMenuItemType.FillColor] = (IDictionary) dictionary;
    }

    private void InitStandardStrokeThicknesses()
    {
      this.allValues[ContextMenuItemType.StrokeThickness] = (IDictionary) new Dictionary<AnnotationMode, IReadOnlyList<ToolbarSettingsHelper.ValueProxy<float>>>()
      {
        [AnnotationMode.None] = (IReadOnlyList<ToolbarSettingsHelper.ValueProxy<float>>) ((IEnumerable<float>) new float[14]
        {
          0.25f,
          0.5f,
          1f,
          2f,
          3f,
          4f,
          5f,
          6f,
          7f,
          8f,
          9f,
          10f,
          11f,
          12f
        }).Select<float, ToolbarSettingsHelper.ValueProxy<float>>((Func<float, ToolbarSettingsHelper.ValueProxy<float>>) (c => ToolbarSettingsHelper.NestedDefaultValues.Value<float>(c, (double) c == 1.0))).ToArray<ToolbarSettingsHelper.ValueProxy<float>>()
      };
    }

    private void InitStandardFontSizes()
    {
      this.allValues[ContextMenuItemType.FontSize] = (IDictionary) new Dictionary<AnnotationMode, IReadOnlyList<ToolbarSettingsHelper.ValueProxy<float>>>()
      {
        [AnnotationMode.None] = (IReadOnlyList<ToolbarSettingsHelper.ValueProxy<float>>) ((IEnumerable<float>) new float[18]
        {
          2f,
          4f,
          6f,
          8f,
          10f,
          12f,
          14f,
          16f,
          18f,
          20f,
          22f,
          24f,
          26f,
          28f,
          36f,
          48f,
          56f,
          72f
        }).Select<float, ToolbarSettingsHelper.ValueProxy<float>>((Func<float, ToolbarSettingsHelper.ValueProxy<float>>) (c => ToolbarSettingsHelper.NestedDefaultValues.Value<float>(c, (double) c == 12.0))).ToArray<ToolbarSettingsHelper.ValueProxy<float>>()
      };
    }

    private void InitFontColors()
    {
      this.allValues[ContextMenuItemType.FontColor] = (IDictionary) new Dictionary<AnnotationMode, IReadOnlyList<ToolbarSettingsHelper.ValueProxy<string>>>()
      {
        [AnnotationMode.None] = (IReadOnlyList<ToolbarSettingsHelper.ValueProxy<string>>) new ToolbarSettingsHelper.ValueProxy<string>[9]
        {
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FFFFFFFF"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FF000000", true),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FFFB302F"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FFFD9927"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FFFAFF00"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FFA5DE50"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FF43D9EF"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FF52AAEC"),
          ToolbarSettingsHelper.NestedDefaultValues.Value<string>("#FF9573E4")
        }
      };
    }

    private static ToolbarSettingsHelper.ValueProxy<T> Value<T>(T value, bool isDefault)
    {
      return new ToolbarSettingsHelper.ValueProxy<T>(value, isDefault);
    }

    private static ToolbarSettingsHelper.ValueProxy<T> Value<T>(T value)
    {
      return ToolbarSettingsHelper.NestedDefaultValues.Value<T>(value, false);
    }
  }

  private class ValueProxy<T>
  {
    public ValueProxy(T value, bool isDefault)
    {
      this.Value = value;
      this.IsDefault = isDefault;
    }

    public T Value { get; }

    public bool IsDefault { get; }
  }
}
