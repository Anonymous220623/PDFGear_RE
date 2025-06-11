// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.SkinStorage
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class SkinStorage : DependencyObject
{
  private const string DefaultName = "Default";
  private static Brush metrobrush = (Brush) new SolidColorBrush((Color) ColorConverter.ConvertFromString("#FF119EDA"));
  private static Brush metrohoverbrush = (Brush) new SolidColorBrush((Color) ColorConverter.ConvertFromString("#FFD8D8D9"));
  private static Brush metroforegroundbrush = (Brush) new SolidColorBrush((Color) ColorConverter.ConvertFromString("#FF333333"));
  private static Brush metroborderbrush = (Brush) new SolidColorBrush((Color) ColorConverter.ConvertFromString("#FFCCCCCC"));
  private static Brush metrofocusedbrush = (Brush) new SolidColorBrush((Color) ColorConverter.ConvertFromString("#FF2ABFF1"));
  private static Brush metrobackgroundbrush = (Brush) new SolidColorBrush((Color) ColorConverter.ConvertFromString("#FFEBEBEB"));
  private static Brush metropanelbackgroundbrush = (Brush) Brushes.White;
  private static Brush metrohighlightedforegroundbrush = (Brush) Brushes.White;
  private static FontFamily metrofontfamily = new FontFamily("Segoe UI");
  public static readonly DependencyProperty MetroBrushProperty = DependencyProperty.RegisterAttached("MetroBrush", typeof (Brush), typeof (SkinStorage), (PropertyMetadata) new FrameworkPropertyMetadata((object) SkinStorage.metrobrush, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(SkinStorage.OnMetroBrushChanged)));
  public static readonly DependencyProperty MetroForegroundBrushProperty = DependencyProperty.RegisterAttached("MetroForegroundBrush", typeof (Brush), typeof (SkinStorage), (PropertyMetadata) new FrameworkPropertyMetadata((object) SkinStorage.metroforegroundbrush, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(SkinStorage.OnMetroBrushChanged)));
  public static readonly DependencyProperty MetroHoverBrushProperty = DependencyProperty.RegisterAttached("MetroHoverBrush", typeof (Brush), typeof (SkinStorage), (PropertyMetadata) new FrameworkPropertyMetadata((object) SkinStorage.metrohoverbrush, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(SkinStorage.OnMetroBrushChanged)));
  public static readonly DependencyProperty MetroBorderBrushProperty = DependencyProperty.RegisterAttached("MetroBorderBrush", typeof (Brush), typeof (SkinStorage), (PropertyMetadata) new FrameworkPropertyMetadata((object) SkinStorage.metroborderbrush, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(SkinStorage.OnMetroBrushChanged)));
  public static readonly DependencyProperty MetroFocusedBorderBrushProperty = DependencyProperty.RegisterAttached("MetroFocusedBorderBrush", typeof (Brush), typeof (SkinStorage), (PropertyMetadata) new FrameworkPropertyMetadata((object) SkinStorage.metrofocusedbrush, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(SkinStorage.OnMetroBrushChanged)));
  public static readonly DependencyProperty MetroBackgroundBrushProperty = DependencyProperty.RegisterAttached("MetroBackgroundBrush", typeof (Brush), typeof (SkinStorage), (PropertyMetadata) new FrameworkPropertyMetadata((object) SkinStorage.metrobackgroundbrush, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(SkinStorage.OnMetroBrushChanged)));
  public static readonly DependencyProperty MetroFontFamilyProperty = DependencyProperty.RegisterAttached("MetroFontFamily", typeof (FontFamily), typeof (SkinStorage), (PropertyMetadata) new FrameworkPropertyMetadata((object) SkinStorage.metrofontfamily, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(SkinStorage.OnMetroBrushChanged)));
  public static readonly DependencyProperty MetroPanelBackgroundBrushProperty = DependencyProperty.RegisterAttached("MetroPanelBackgroundBrush", typeof (Brush), typeof (SkinStorage), (PropertyMetadata) new FrameworkPropertyMetadata((object) SkinStorage.metropanelbackgroundbrush, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(SkinStorage.OnMetroBrushChanged)));
  public static readonly DependencyProperty MetroHighlightedForegroundBrushProperty = DependencyProperty.RegisterAttached("MetroHighlightedForegroundBrush", typeof (Brush), typeof (SkinStorage), (PropertyMetadata) new FrameworkPropertyMetadata((object) SkinStorage.metrohighlightedforegroundbrush, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(SkinStorage.OnMetroBrushChanged)));
  [Browsable(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  public static string ClassicThemeXAMLPath = "/Syncfusion.Shared.WPF.Classic;component/SkinManager/";
  [EditorBrowsable(EditorBrowsableState.Never)]
  [Browsable(false)]
  public static string SharedThemeXAMLPath = "/Syncfusion.Shared.WPF;component/SkinManager/";
  private static bool IsUserControlPresent;
  private static bool IsPageControlPresent;
  private static ObservableCollection<FrameworkElement> Root;
  private static bool IsSkinNotChanged = false;
  private static bool windowflag = false;
  internal static readonly DependencyProperty MergedDictionaryPathProperty = DependencyProperty.RegisterAttached("MergedDictionaryPath", typeof (ObservableCollection<string>), typeof (SkinStorage), (PropertyMetadata) new FrameworkPropertyMetadata((object) new ObservableCollection<string>()));
  public static readonly DependencyProperty EnableOptimizationProperty = DependencyProperty.RegisterAttached("EnableOptimization", typeof (bool), typeof (SkinStorage), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(SkinStorage.OnEnableOptimizationChanged)));
  public static readonly DependencyProperty VisualStyleProperty = DependencyProperty.RegisterAttached("VisualStyle", typeof (string), typeof (SkinStorage), (PropertyMetadata) new FrameworkPropertyMetadata((object) "Default", FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(SkinStorage.OnVisualStyleChanged)));
  public static readonly DependencyProperty OverrideVisualStyleProperty = DependencyProperty.RegisterAttached("OverrideVisualStyle", typeof (bool), typeof (SkinStorage), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(SkinStorage.OnOverrideVisualStyleChanged)));
  internal static readonly DependencyProperty IsMSDictionaryMergedProperty = DependencyProperty.RegisterAttached("IsMSDictionaryMerged", typeof (bool), typeof (SkinStorage), (PropertyMetadata) new UIPropertyMetadata((object) false));
  public static readonly DependencyProperty EnableTouchProperty = DependencyProperty.RegisterAttached("EnableTouch", typeof (bool), typeof (SkinStorage), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.Inherits));
  internal static readonly DependencyProperty IsDictionaryMergedProperty = DependencyProperty.RegisterAttached("IsDictionaryMerged", typeof (bool), typeof (SkinStorage), (PropertyMetadata) new UIPropertyMetadata((object) false));
  [Browsable(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  private static bool isThemeChangeNotNeeded = false;

  public static Brush GetMetroBrush(DependencyObject obj)
  {
    return (Brush) obj.GetValue(SkinStorage.MetroBrushProperty);
  }

  public static void SetMetroBrush(DependencyObject obj, Brush value)
  {
    obj.ClearValue(SkinStorage.MetroBrushProperty);
    obj.SetValue(SkinStorage.MetroBrushProperty, (object) value);
  }

  public static Brush GetMetroForegroundBrush(DependencyObject obj)
  {
    return (Brush) obj.GetValue(SkinStorage.MetroForegroundBrushProperty);
  }

  public static void SetMetroForegroundBrush(DependencyObject obj, Brush value)
  {
    obj.ClearValue(SkinStorage.MetroForegroundBrushProperty);
    obj.SetValue(SkinStorage.MetroForegroundBrushProperty, (object) value);
  }

  public static Brush GetMetroHoverBrush(DependencyObject obj)
  {
    return (Brush) obj.GetValue(SkinStorage.MetroHoverBrushProperty);
  }

  public static void SetMetroHoverBrush(DependencyObject obj, Brush value)
  {
    obj.ClearValue(SkinStorage.MetroHoverBrushProperty);
    obj.SetValue(SkinStorage.MetroHoverBrushProperty, (object) value);
  }

  public static Brush GetMetroBorderBrush(DependencyObject obj)
  {
    return (Brush) obj.GetValue(SkinStorage.MetroBorderBrushProperty);
  }

  public static void SetMetroBorderBrush(DependencyObject obj, Brush value)
  {
    obj.ClearValue(SkinStorage.MetroBorderBrushProperty);
    obj.SetValue(SkinStorage.MetroBorderBrushProperty, (object) value);
  }

  public static Brush GetMetroFocusedBorderBrush(DependencyObject obj)
  {
    return (Brush) obj.GetValue(SkinStorage.MetroFocusedBorderBrushProperty);
  }

  public static void SetMetroFocusedBorderBrush(DependencyObject obj, Brush value)
  {
    obj.ClearValue(SkinStorage.MetroFocusedBorderBrushProperty);
    obj.SetValue(SkinStorage.MetroFocusedBorderBrushProperty, (object) value);
  }

  public static Brush GetMetroBackgroundBrush(DependencyObject obj)
  {
    return (Brush) obj.GetValue(SkinStorage.MetroBackgroundBrushProperty);
  }

  public static void SetMetroBackgroundBrush(DependencyObject obj, Brush value)
  {
    obj.ClearValue(SkinStorage.MetroBackgroundBrushProperty);
    obj.SetValue(SkinStorage.MetroBackgroundBrushProperty, (object) value);
  }

  public static Brush GetMetroPanelBackgroundBrush(DependencyObject obj)
  {
    return (Brush) obj.GetValue(SkinStorage.MetroPanelBackgroundBrushProperty);
  }

  public static void SetMetroPanelBackgroundBrush(DependencyObject obj, Brush value)
  {
    obj.ClearValue(SkinStorage.MetroPanelBackgroundBrushProperty);
    obj.SetValue(SkinStorage.MetroPanelBackgroundBrushProperty, (object) value);
  }

  public static Brush GetMetroHighlightedForegroundBrush(DependencyObject obj)
  {
    return (Brush) obj.GetValue(SkinStorage.MetroHighlightedForegroundBrushProperty);
  }

  public static void SetMetroHighlightedForegroundBrush(DependencyObject obj, Brush value)
  {
    obj.ClearValue(SkinStorage.MetroHighlightedForegroundBrushProperty);
    obj.SetValue(SkinStorage.MetroHighlightedForegroundBrushProperty, (object) value);
  }

  public static FontFamily GetMetroFontFamily(DependencyObject obj)
  {
    return (FontFamily) obj.GetValue(SkinStorage.MetroFontFamilyProperty);
  }

  public static void SetMetroFontFamily(DependencyObject obj, FontFamily value)
  {
    obj.ClearValue(SkinStorage.MetroFontFamilyProperty);
    obj.SetValue(SkinStorage.MetroFontFamilyProperty, (object) value);
  }

  public static void MergeMetroBrush(ResourceDictionary metroskindictionary, DependencyObject obj)
  {
    ResourceDictionary resourceDictionary = metroskindictionary;
    try
    {
      if (SkinStorage.GetMetroBrush(obj) != null)
        resourceDictionary[(object) "MetroBrush"] = (object) SkinStorage.GetMetroBrush(obj);
      if (SkinStorage.GetMetroHoverBrush(obj) != null)
        resourceDictionary[(object) "MetroHoverBrush"] = (object) SkinStorage.GetMetroHoverBrush(obj);
      if (SkinStorage.GetMetroForegroundBrush(obj) != null)
        resourceDictionary[(object) "MetroForegroundBrush"] = (object) SkinStorage.GetMetroForegroundBrush(obj);
      if (SkinStorage.GetMetroFontFamily(obj) != null)
        resourceDictionary[(object) "MetroFontFamily"] = (object) SkinStorage.GetMetroFontFamily(obj);
      if (SkinStorage.GetMetroBorderBrush(obj) != null)
        resourceDictionary[(object) "MetroBorderBrush"] = (object) SkinStorage.GetMetroBorderBrush(obj);
      if (SkinStorage.GetMetroFocusedBorderBrush(obj) != null)
        resourceDictionary[(object) "MetroFocusedBorderBrush"] = (object) SkinStorage.GetMetroFocusedBorderBrush(obj);
      if (SkinStorage.GetMetroBackgroundBrush(obj) != null)
        resourceDictionary[(object) "MetroBackgroundBrush"] = (object) SkinStorage.GetMetroBackgroundBrush(obj);
      if (SkinStorage.GetMetroPanelBackgroundBrush(obj) != null)
        resourceDictionary[(object) "MetroPanelBackgroundBrush"] = (object) SkinStorage.GetMetroPanelBackgroundBrush(obj);
      if (SkinStorage.GetMetroHighlightedForegroundBrush(obj) == null)
        return;
      resourceDictionary[(object) "MetroHighlightedForegroundBrush"] = (object) SkinStorage.GetMetroHighlightedForegroundBrush(obj);
    }
    catch
    {
    }
  }

  private static void MergeMetroBrushDictionaries(
    FrameworkElement element,
    ResourceDictionary dictionary)
  {
    SkinTypeAttribute skinAttribute = SkinStorage.GetSkinAttribute(element, "Metro");
    if (skinAttribute == null)
      return;
    ResourceDictionary resourceDictionary = new ResourceDictionary();
    resourceDictionary.Source = new Uri(skinAttribute.XamlResource, UriKind.RelativeOrAbsolute);
    for (int index = 0; index < resourceDictionary.MergedDictionaries.Count; ++index)
    {
      if (resourceDictionary.MergedDictionaries[index].Source == dictionary.Source)
      {
        try
        {
          resourceDictionary.MergedDictionaries.RemoveAt(index);
        }
        catch
        {
        }
        --index;
      }
    }
    SkinManager.RemoveDictionaryIfExist(element, dictionary);
    element.Resources.MergedDictionaries.Add(dictionary);
  }

  private static void ApplyMetroBrush(DependencyObject obj)
  {
    ResourceDictionary resourceDictionary = new ResourceDictionary();
    resourceDictionary.Source = new Uri(SkinStorage.SharedThemeXAMLPath + "MetroThemeBrushes.xaml", UriKind.RelativeOrAbsolute);
    SkinStorage.MergeMetroBrush(resourceDictionary, obj);
    if (obj.GetType().ToString().StartsWith("Syncfusion"))
    {
      SkinStorage.MergeMetroBrushDictionaries(obj as FrameworkElement, resourceDictionary);
    }
    else
    {
      if (!(obj is FrameworkElement))
        return;
      SkinManager.RemoveDictionaryIfExist(obj as FrameworkElement, resourceDictionary);
      (obj as FrameworkElement).Resources.MergedDictionaries.Add(resourceDictionary);
    }
  }

  private static void OnMetroBrushChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs e)
  {
    if (!SkinStorage.GetEnableOptimization(obj))
    {
      if (obj == null)
        return;
      if (obj is FrameworkElement frameworkElement && !frameworkElement.IsLoaded)
        frameworkElement.Loaded += new RoutedEventHandler(SkinStorage.element_Loaded1);
      SkinStorage.ApplyMetroBrush(obj);
    }
    else
    {
      FrameworkElement frameworkElement = obj as FrameworkElement;
      int count;
      if (frameworkElement == null || !(SkinStorage.GetVisualStyle((DependencyObject) frameworkElement) == "Metro") || !SkinStorage.IsApply(frameworkElement) || (count = frameworkElement.Resources.MergedDictionaries.Count) <= 0)
        return;
      SkinStorage.UpdateMetroBrush(frameworkElement, frameworkElement.Resources.MergedDictionaries, count);
    }
  }

  private static void UpdateMetroBrush(
    FrameworkElement element,
    Collection<ResourceDictionary> MergedDictionaries,
    int count)
  {
    for (int index = 0; index < count; ++index)
    {
      ResourceDictionary mergedDictionary = MergedDictionaries[index];
      if (mergedDictionary.Source != (Uri) null && mergedDictionary.Source.ToString().Contains("SkinManager/MetroThemeBrushes.xaml"))
      {
        SkinStorage.MergeMetroBrush(mergedDictionary, (DependencyObject) element);
        if (SkinStorage.GetEnableOptimization((DependencyObject) element))
          break;
        if (MergedDictionaries.Contains(mergedDictionary))
          MergedDictionaries.Remove(mergedDictionary);
        MergedDictionaries.Add(mergedDictionary);
        break;
      }
      if (mergedDictionary.MergedDictionaries.Count > 0)
        SkinStorage.UpdateMetroBrush(element, mergedDictionary.MergedDictionaries, mergedDictionary.MergedDictionaries.Count);
    }
  }

  private static void element_Loaded1(object sender, RoutedEventArgs e)
  {
    SkinStorage.ApplyMetroBrush(sender as DependencyObject);
    (sender as FrameworkElement).Loaded -= new RoutedEventHandler(SkinStorage.element_Loaded1);
  }

  private static void RemoveDictionaryIfExist(
    FrameworkElement element,
    ResourceDictionary dictionary)
  {
    if (element == null)
      return;
    for (int index = 0; index < element.Resources.MergedDictionaries.Count; ++index)
    {
      if (element.Resources.MergedDictionaries[index].Source == dictionary.Source)
      {
        element.Resources.MergedDictionaries.RemoveAt(index);
        --index;
      }
    }
  }

  public static string GetVisualStyle(DependencyObject obj)
  {
    return (string) obj.GetValue(SkinStorage.VisualStyleProperty);
  }

  public static bool GetOverrideVisualStyle(DependencyObject obj)
  {
    return (bool) obj.GetValue(SkinStorage.OverrideVisualStyleProperty);
  }

  public static void SetVisualStyle(DependencyObject obj, string value)
  {
    SkinStorage.IsThemeChangeNotNeeded = false;
    if (obj != null && SkinStorage.GetEnableOptimization(obj))
      SkinStorage.ResetResources(obj, value);
    obj?.SetValue(SkinStorage.VisualStyleProperty, (object) value);
    if (!(obj is ISkinStylePropagator))
      return;
    ((ISkinStylePropagator) obj).OnStyleChanged(value);
  }

  public static void SetOverrideVisualStyle(DependencyObject obj, bool value)
  {
    obj.SetValue(SkinStorage.OverrideVisualStyleProperty, (object) value);
  }

  internal static void SetMergedDictionaryPath(DependencyObject obj, string value)
  {
    obj.SetValue(SkinStorage.MergedDictionaryPathProperty, (object) value);
  }

  internal static ObservableCollection<string> GetMergedDictionaryPath(DependencyObject obj)
  {
    return (ObservableCollection<string>) obj.GetValue(SkinStorage.MergedDictionaryPathProperty);
  }

  public static bool GetEnableOptimization(DependencyObject obj)
  {
    return (bool) obj.GetValue(SkinStorage.EnableOptimizationProperty);
  }

  public static void SetEnableOptimization(DependencyObject obj, bool value)
  {
    obj.SetValue(SkinStorage.EnableOptimizationProperty, (object) value);
  }

  public static bool GetEnableTouch(DependencyObject obj)
  {
    return (bool) obj.GetValue(SkinStorage.EnableTouchProperty);
  }

  public static void SetEnableTouch(DependencyObject obj, bool value)
  {
    obj.SetValue(SkinStorage.EnableTouchProperty, (object) value);
  }

  internal static bool GetIsDictionaryMerged(DependencyObject obj)
  {
    return (bool) obj.GetValue(SkinStorage.IsDictionaryMergedProperty);
  }

  internal static void SetIsDictionaryMerged(DependencyObject obj, bool value)
  {
    obj.SetValue(SkinStorage.IsDictionaryMergedProperty, (object) value);
  }

  internal static bool GetIsMSDictionaryMerged(DependencyObject obj)
  {
    return (bool) obj.GetValue(SkinStorage.IsMSDictionaryMergedProperty);
  }

  internal static void SetIsMSDictionaryMerged(DependencyObject obj, bool value)
  {
    obj.SetValue(SkinStorage.IsMSDictionaryMergedProperty, (object) value);
  }

  public static bool IsThemeChangeNotNeeded
  {
    get => SkinStorage.isThemeChangeNotNeeded;
    set => SkinStorage.isThemeChangeNotNeeded = value;
  }

  private static void OnEnableOptimizationChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs e)
  {
    FrameworkElement element = obj as FrameworkElement;
    if (SkinStorage.IsSkinNotChanged || element == null || SkinStorage.Root != null || !(element is Window) && (!(element is UserControl) || !(element.GetType().Name != "Splitter")) && !(element is ChromelessWindow) && !(element is Page))
      return;
    SkinStorage.Root = new ObservableCollection<FrameworkElement>();
    SkinStorage.windowflag = true;
    if (element is UserControl)
      SkinStorage.IsUserControlPresent = true;
    if (element is Page)
      SkinStorage.IsPageControlPresent = true;
    SkinStorage.ClearDictionary(element);
    ResourceDictionary controlDictionary = SkinStorage.GetMSControlDictionary(SkinStorage.GetVisualStyle((DependencyObject) element));
    SkinStorage.MergeDictionaryIntoElement(element, controlDictionary);
    if (SkinStorage.Root.Contains(element))
      return;
    element.Unloaded += new RoutedEventHandler(SkinStorage.element_Unloaded);
    SkinStorage.Root.Add(element);
    element.Loaded += new RoutedEventHandler(SkinStorage.element_Loaded);
  }

  private static void OnVisualStyleChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs e)
  {
    string str1 = string.Empty;
    if (e.NewValue != null)
      str1 = e.NewValue as string;
    if (obj == null)
      return;
    FrameworkElement element = obj as FrameworkElement;
    if (SkinStorage.GetEnableOptimization(obj))
      obj.SetValue(SkinStorage.VisualStyleProperty, (object) str1);
    if ((bool) obj.GetValue(SkinStorage.OverrideVisualStyleProperty))
      return;
    if (!SkinStorage.GetEnableOptimization(obj))
    {
      SkinStorage.GetVisualStyle(obj);
      if (element == null || SkinStorage.IsThemeChangeNotNeeded)
        return;
      if ((element.Parent == null || !(element.Parent is Window) || SkinStorage.windowflag) && (element.Parent == null || !(element.Parent is ChromelessWindow) || SkinStorage.windowflag))
      {
        switch (obj)
        {
          case Window _:
          case ChromelessWindow _:
          case UserControl _:
          case Page _:
            break;
          default:
            goto label_22;
        }
      }
      if (obj is Window || obj is ChromelessWindow)
        SkinStorage.windowflag = true;
      DependencyObject dependencyObject = LogicalTreeHelper.GetParent(obj) ?? VisualTreeHelper.GetParent(obj);
      string skin = SkinStorage.GetVisualStyle(obj).ToString();
      string str2 = dependencyObject == null ? string.Empty : SkinStorage.GetVisualStyle(dependencyObject).ToString();
      try
      {
        ResourceDictionary controlDictionary = SkinStorage.GetMSControlDictionary(skin);
        if (str2 != skin)
        {
          SkinStorage.SetIsDictionaryMerged(obj, true);
          SkinStorage.RemoveDictionaryIfExist(element, controlDictionary);
          if (element.Resources != null)
            element.Resources.MergedDictionaries.Add(controlDictionary);
        }
        else if (SkinStorage.GetIsDictionaryMerged(obj))
        {
          SkinStorage.RemoveDictionaryIfExist(element, controlDictionary);
          if (element.Resources != null)
            element.Resources.MergedDictionaries.Add(controlDictionary);
        }
      }
      catch
      {
      }
label_22:
      if (!SkinStorage.IsApply(obj as FrameworkElement))
        return;
      SkinStorage.ApplySkin(obj, SkinStorage.GetVisualStyle(obj));
    }
    else
      SkinStorage.ApplyOptimization(obj, e);
  }

  private static void OnOverrideVisualStyleChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs e)
  {
    if (!e.NewValue.Equals((object) true) || !(obj is FrameworkElement frameworkElement))
      return;
    for (int index = 0; index < frameworkElement.Resources.MergedDictionaries.Count; ++index)
    {
      if (frameworkElement.Resources.MergedDictionaries[index].Source != (Uri) null && (frameworkElement.Resources.MergedDictionaries[index].Source.OriginalString.Contains("/Syncfusion.Shared.WPF;") || frameworkElement.Resources.MergedDictionaries[index].Source.OriginalString.Contains("/Syncfusion.Tools.WPF;") || frameworkElement.Resources.MergedDictionaries[index].Source.OriginalString.Contains("/Syncfusion.Tools.WPF.Classic;") || frameworkElement.Resources.MergedDictionaries[index].Source.OriginalString.Contains("/Syncfusion.Tools.WPF.Classic;")))
      {
        frameworkElement.Resources.MergedDictionaries.RemoveAt(index);
        --index;
      }
    }
  }

  private static SkinTypeAttribute GetSkinAttribute(FrameworkElement element, string currentskin)
  {
    SkinTypeAttribute skinAttribute = (SkinTypeAttribute) null;
    if (element != null)
    {
      foreach (object customAttribute in element.GetType().GetCustomAttributes(true))
      {
        if (customAttribute is SkinTypeAttribute skinTypeAttribute && skinTypeAttribute.SkinVisualStyle.ToString() == currentskin)
        {
          skinAttribute = skinTypeAttribute;
          break;
        }
      }
    }
    return skinAttribute;
  }

  private static SkinTypeAttribute GetSkinAttribute(Control element, string currentskin)
  {
    SkinTypeAttribute skinAttribute = (SkinTypeAttribute) null;
    if (element != null)
    {
      foreach (object customAttribute in element.GetType().GetCustomAttributes(true))
      {
        if (customAttribute is SkinTypeAttribute skinTypeAttribute && skinTypeAttribute.SkinVisualStyle.ToString() == currentskin)
        {
          skinAttribute = skinTypeAttribute;
          break;
        }
      }
    }
    return skinAttribute;
  }

  private static void ApplySkin(DependencyObject obj, string style)
  {
    if (!(obj is Visual))
      return;
    FrameworkElement frameworkElement1 = obj as FrameworkElement;
    if (obj is ContextMenu)
    {
      ResourceDictionary dictionary = SkinStorage.MergeDic(new ResourceDictionary(), style);
      SkinStorage.RemoveDictionaryIfExist(frameworkElement1, dictionary);
      if ((obj as ContextMenu).Style == null)
        frameworkElement1.Resources.MergedDictionaries.Add(dictionary);
    }
    foreach (FrameworkElement frameworkElement2 in VisualUtils.EnumLogicalChildrenOfType(obj, typeof (FrameworkElement)))
    {
      if (frameworkElement2 != null && SkinStorage.IsApply(frameworkElement2))
      {
        if (frameworkElement2 is Popup && (frameworkElement2 as Popup).Child != null)
        {
          IEnumerable<Visual> fe = VisualUtils.EnumChildrenOfType((Visual) (frameworkElement2 as Popup).Child, typeof (FrameworkElement));
          if (fe != null)
            SkinStorage.ControlIterate(fe, style);
        }
        if (frameworkElement2 is ScrollViewer && frameworkElement2 != null && (frameworkElement2 as ScrollViewer).Content is Visual)
        {
          IEnumerable<Visual> fe = VisualUtils.EnumChildrenOfType((frameworkElement2 as ScrollViewer).Content as Visual, typeof (FrameworkElement));
          if (fe != null)
            SkinStorage.ControlIterate(fe, style);
        }
        SkinStorage.OuterControlIterate(frameworkElement2, style);
        if (frameworkElement2.GetType().FullName.Contains("Syncfusion"))
          SkinStorage.SetVisualStyle((DependencyObject) frameworkElement2, style);
      }
    }
    if (frameworkElement1 == null)
      return;
    SkinStorage.OuterControlIterate(frameworkElement1, style);
  }

  private static bool IsMSDictionaryMrgedInParent(FrameworkElement fe)
  {
    return (VisualUtils.FindLogicalAncestor((Visual) fe, typeof (Window)) != null || VisualUtils.FindLogicalAncestor((Visual) fe, typeof (UserControl)) != null || VisualUtils.FindLogicalAncestor((Visual) fe, typeof (Page)) != null || VisualUtils.FindLogicalAncestor((Visual) fe, typeof (ChromelessWindow)) != null) && SkinStorage.GetVisualStyle((DependencyObject) fe) == SkinStorage.GetVisualStyle(fe.Parent) && !SkinStorage.GetIsMSDictionaryMerged((DependencyObject) fe);
  }

  private static void OuterControlIterate(FrameworkElement fe, string style)
  {
    SkinTypeAttribute skinTypeAttribute = (SkinTypeAttribute) null;
    if (!SkinStorage.GetOverrideVisualStyle((DependencyObject) fe))
      skinTypeAttribute = SkinStorage.GetSkinAttribute(fe, style);
    if (skinTypeAttribute != null)
    {
      int skinVisualStyle = (int) skinTypeAttribute.SkinVisualStyle;
      string xamlResource = skinTypeAttribute.XamlResource;
      Type type = skinTypeAttribute.Type;
      ResourceDictionary resourceDictionary = new ResourceDictionary();
      try
      {
        resourceDictionary.Source = new Uri(xamlResource, UriKind.RelativeOrAbsolute);
        if (!SkinStorage.IsMSDictionaryMrgedInParent(fe))
        {
          SkinStorage.SetIsMSDictionaryMerged((DependencyObject) fe, true);
          resourceDictionary = SkinStorage.MergeDic(resourceDictionary, style);
        }
        try
        {
          SkinStorage.RemoveDictionaryIfExist(fe, resourceDictionary);
          fe.Resources.MergedDictionaries.Add(resourceDictionary);
        }
        catch
        {
        }
      }
      catch (FileNotFoundException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw;
      }
    }
    if (!DesignerProperties.GetIsInDesignMode((DependencyObject) fe) || fe.GetType().FullName.Contains("Syncfusion"))
      return;
    string skin = SkinStorage.GetVisualStyle((DependencyObject) fe).ToString();
    ResourceDictionary controlDictionary = SkinStorage.GetMSControlDictionary(skin);
    if (skin == null)
      return;
    SkinStorage.SetIsDictionaryMerged((DependencyObject) fe, true);
    SkinStorage.RemoveDictionaryIfExist(fe, controlDictionary);
    if (fe.Resources == null)
      return;
    fe.Resources.MergedDictionaries.Add(controlDictionary);
  }

  private static void ControlIterate(IEnumerable<Visual> fe, string style)
  {
    foreach (FrameworkElement frameworkElement in fe)
    {
      if (frameworkElement != null && SkinStorage.IsApply(frameworkElement))
      {
        SkinTypeAttribute skinAttribute = SkinStorage.GetSkinAttribute(frameworkElement, style);
        if (skinAttribute != null)
        {
          int skinVisualStyle = (int) skinAttribute.SkinVisualStyle;
          string xamlResource = skinAttribute.XamlResource;
          Type type = skinAttribute.Type;
          ResourceDictionary dictionary = SkinStorage.MergeDic(new ResourceDictionary()
          {
            Source = new Uri(xamlResource, UriKind.RelativeOrAbsolute)
          }, style);
          try
          {
            SkinStorage.RemoveDictionaryIfExist(frameworkElement, dictionary);
            frameworkElement.Resources.MergedDictionaries.Add(dictionary);
          }
          catch
          {
          }
        }
      }
    }
  }

  private static bool IsApply(FrameworkElement frameSkin)
  {
    switch (frameSkin)
    {
      case Panel _:
      case Image _:
      case Decorator _:
      case Shape _:
        return false;
      default:
        return true;
    }
  }

  private static ResourceDictionary MergeDic(ResourceDictionary rd, string skin)
  {
    ResourceDictionary controlDictionary = SkinStorage.GetMSControlDictionary(skin);
    rd.MergedDictionaries.Add(controlDictionary);
    return rd;
  }

  private static void ApplyOptimization(DependencyObject obj, DependencyPropertyChangedEventArgs e)
  {
    FrameworkElement frameworkElement1 = obj as FrameworkElement;
    string newValue = e.NewValue as string;
    if (SkinStorage.IsSkinNotChanged || frameworkElement1 == null)
      return;
    if (frameworkElement1 is Window || frameworkElement1 is UserControl && frameworkElement1.GetType().Name != "Splitter" || frameworkElement1 is ChromelessWindow || frameworkElement1 is Page)
    {
      if (SkinStorage.Root == null)
        SkinStorage.Root = new ObservableCollection<FrameworkElement>();
      SkinStorage.windowflag = true;
      if (frameworkElement1 is UserControl)
        SkinStorage.IsUserControlPresent = true;
      if (frameworkElement1 is Page)
        SkinStorage.IsPageControlPresent = true;
      SkinStorage.ClearDictionary(frameworkElement1);
      ResourceDictionary controlDictionary = SkinStorage.GetMSControlDictionary(SkinStorage.GetVisualStyle((DependencyObject) frameworkElement1));
      SkinStorage.MergeDictionaryIntoElement(frameworkElement1, controlDictionary);
      if (frameworkElement1 != null && !SkinStorage.Root.Contains(frameworkElement1))
      {
        string visualStyle = SkinStorage.GetVisualStyle((DependencyObject) frameworkElement1);
        if (SkinStorage.GetEnableOptimization((DependencyObject) frameworkElement1))
        {
          foreach (FrameworkElement frameworkElement2 in VisualUtils.EnumLogicalChildrenOfType((DependencyObject) frameworkElement1, typeof (FrameworkElement)))
          {
            if (frameworkElement2 != null && frameworkElement2.GetType().FullName.Contains("Syncfusion"))
            {
              SkinTypeAttribute skinAttribute = SkinStorage.GetSkinAttribute(frameworkElement2, visualStyle);
              if (skinAttribute != null)
              {
                int skinVisualStyle = (int) skinAttribute.SkinVisualStyle;
                string xamlResource = skinAttribute.XamlResource;
                Type type = skinAttribute.Type;
                ResourceDictionary rd = new ResourceDictionary();
                rd.Source = new Uri(xamlResource, UriKind.RelativeOrAbsolute);
                if (!SkinStorage.IsMSDictionaryMrgedInParent(frameworkElement2))
                {
                  SkinStorage.SetIsMSDictionaryMerged((DependencyObject) frameworkElement2, true);
                  rd = SkinStorage.MergeDic(rd, visualStyle);
                }
                if (frameworkElement1 != null && frameworkElement1.Resources != null && frameworkElement1.Resources.MergedDictionaries != null && !frameworkElement1.Resources.MergedDictionaries.Contains(rd))
                  frameworkElement1.Resources.MergedDictionaries.Add(rd);
              }
            }
          }
        }
        SkinStorage.Root.Add(frameworkElement1);
        frameworkElement1.Loaded += new RoutedEventHandler(SkinStorage.element_Loaded);
      }
    }
    if (frameworkElement1 == null || !SkinStorage.IsApply(frameworkElement1))
      return;
    string currentskin = SkinStorage.GetVisualStyle(obj).ToString();
    SkinTypeAttribute skinAttribute1 = SkinStorage.GetSkinAttribute(frameworkElement1, currentskin);
    if (SkinStorage.windowflag)
    {
      if (skinAttribute1 != null)
      {
        try
        {
          string xamlResource = skinAttribute1.XamlResource;
          ResourceDictionary Dict = new ResourceDictionary();
          Dict.Source = new Uri(xamlResource, UriKind.RelativeOrAbsolute);
          FrameworkElement element = (FrameworkElement) null;
          if (element == null && SkinStorage.IsUserControlPresent)
            element = VisualUtils.FindSomeParent(frameworkElement1, typeof (UserControl));
          if (element == null && SkinStorage.IsPageControlPresent)
            element = VisualUtils.FindSomeParent(frameworkElement1, typeof (Page));
          switch (frameworkElement1)
          {
            case Window _:
            case UserControl _:
            case Page _:
              element = frameworkElement1;
              break;
          }
          if (element == null)
            element = VisualUtils.FindSomeParent(frameworkElement1, typeof (Window));
          if (element != null)
          {
            if (SkinStorage.Root.Contains(element))
            {
              SkinStorage.MergeDictionaryIntoElement(element, Dict);
              return;
            }
            if (SkinStorage.Root[0] == null)
              return;
            SkinStorage.MergeDictionaryIntoElement(SkinStorage.Root[0], Dict);
            return;
          }
          if (SkinStorage.Root[0] == null)
            return;
          SkinStorage.MergeDictionaryIntoElement(SkinStorage.Root[0], Dict);
          return;
        }
        catch (Exception ex)
        {
          return;
        }
      }
    }
    if (skinAttribute1 == null)
      return;
    if (!(skinAttribute1.XamlResource != string.Empty))
      return;
    try
    {
      FrameworkElement element = obj as FrameworkElement;
      ResourceDictionary Dict = new ResourceDictionary();
      string xamlResource = skinAttribute1.XamlResource;
      if (element.Resources.MergedDictionaries.Count == 0)
        element.ClearMergedDictionaryPath();
      Dict.Source = new Uri(xamlResource, UriKind.RelativeOrAbsolute);
      ResourceDictionary controlDictionary = SkinStorage.GetMSControlDictionary(SkinStorage.GetVisualStyle((DependencyObject) element));
      SkinStorage.MergeDictionaryIntoElement(element, controlDictionary);
      SkinStorage.MergeDictionaryIntoElement(element, Dict);
    }
    catch (Exception ex)
    {
    }
  }

  private static void element_Loaded(object sender, RoutedEventArgs e)
  {
    FrameworkElement frameworkElement = sender as FrameworkElement;
    frameworkElement.Loaded -= new RoutedEventHandler(SkinStorage.element_Loaded);
    ResourceDictionary controlDictionary = SkinStorage.GetMSControlDictionary(SkinStorage.GetVisualStyle((DependencyObject) frameworkElement));
    frameworkElement.Resources.MergedDictionaries.Add(controlDictionary);
  }

  private static void ResetResources(DependencyObject obj, string appliedskin)
  {
    FrameworkElement element = obj as FrameworkElement;
    try
    {
      if (!(SkinStorage.GetVisualStyle((DependencyObject) element).ToString() != appliedskin))
        return;
      SkinStorage.IsThemeChangeNotNeeded = false;
      SkinStorage.IsSkinNotChanged = false;
      SkinStorage.ClearDictionary(element);
      ResourceDictionary controlDictionary = SkinStorage.GetMSControlDictionary(appliedskin);
      element.Resources.MergedDictionaries.Add(controlDictionary);
      element.AddIntoMergedDictionaryPath(controlDictionary.Source == (Uri) null ? "<Resouce Not found>" : controlDictionary.Source.ToString());
    }
    catch (Exception ex)
    {
    }
  }

  private static bool ResolveClassicAssemblyReference()
  {
    if (Type.GetType("Syncfusion.Windows.Shared.VistaWindow,Syncfusion.Shared.WPF.Classic") == (Type) null)
      throw new FileNotFoundException("Could not load assembly Syncfusion.Shared.WPF.Classic, Add reference to Syncfusion.Shared.WPF.Classic assembly.", "Syncfusion.Shared.WPF.Classic");
    return true;
  }

  internal static ResourceDictionary GetMSControlDictionary(string skin)
  {
    SkinStorage.ResolveClassicAssemblyReference();
    ResourceDictionary controlDictionary = new ResourceDictionary();
    switch (skin)
    {
      case "Blend":
        controlDictionary.Source = new Uri(SkinStorage.ClassicThemeXAMLPath + "BlendStyle.xaml", UriKind.RelativeOrAbsolute);
        break;
      case "Office2007Blue":
        controlDictionary.Source = new Uri(SkinStorage.ClassicThemeXAMLPath + "Office2007BlueStyle.xaml", UriKind.RelativeOrAbsolute);
        break;
      case "Office2007Black":
        controlDictionary.Source = new Uri(SkinStorage.ClassicThemeXAMLPath + "Office2007BlackStyle.xaml", UriKind.RelativeOrAbsolute);
        break;
      case "Office2007Silver":
        controlDictionary.Source = new Uri(SkinStorage.ClassicThemeXAMLPath + "Office2007SilverStyle.xaml", UriKind.RelativeOrAbsolute);
        break;
      case "Office2003":
        controlDictionary.Source = new Uri(SkinStorage.ClassicThemeXAMLPath + "Office2003Style.xaml", UriKind.RelativeOrAbsolute);
        break;
      case "SyncOrange":
        controlDictionary.Source = new Uri(SkinStorage.ClassicThemeXAMLPath + "SyncOrangeStyle.xaml", UriKind.RelativeOrAbsolute);
        break;
      case "ShinyRed":
        controlDictionary.Source = new Uri(SkinStorage.ClassicThemeXAMLPath + "ShinyRedStyle.xaml", UriKind.RelativeOrAbsolute);
        break;
      case "ShinyBlue":
        controlDictionary.Source = new Uri(SkinStorage.ClassicThemeXAMLPath + "ShinyBlueStyle.xaml", UriKind.RelativeOrAbsolute);
        break;
      case "Default":
        controlDictionary.Source = new Uri(SkinStorage.SharedThemeXAMLPath + "DefaultStyle.xaml", UriKind.RelativeOrAbsolute);
        break;
      case "Office2010Blue":
        controlDictionary.Source = new Uri(SkinStorage.ClassicThemeXAMLPath + "Office2010BlueStyle.xaml", UriKind.RelativeOrAbsolute);
        break;
      case "Office2010Black":
        controlDictionary.Source = new Uri(SkinStorage.ClassicThemeXAMLPath + "Office2010BlackStyle.xaml", UriKind.RelativeOrAbsolute);
        break;
      case "Office2010Silver":
        controlDictionary.Source = new Uri(SkinStorage.ClassicThemeXAMLPath + "Office2010SilverStyle.xaml", UriKind.RelativeOrAbsolute);
        break;
      case "VS2010":
        controlDictionary.Source = new Uri(SkinStorage.ClassicThemeXAMLPath + "VS2010Style.xaml", UriKind.RelativeOrAbsolute);
        break;
      case "Metro":
        controlDictionary.Source = new Uri(SkinStorage.ClassicThemeXAMLPath + "MetroStyle.xaml", UriKind.RelativeOrAbsolute);
        break;
      case "Transparent":
        controlDictionary.Source = new Uri(SkinStorage.ClassicThemeXAMLPath + "TransparentStyle.xaml", UriKind.RelativeOrAbsolute);
        break;
      case "Office2013":
        controlDictionary.Source = new Uri(SkinStorage.ClassicThemeXAMLPath + "Office2013Style.xaml", UriKind.RelativeOrAbsolute);
        break;
    }
    return controlDictionary;
  }

  private static void MergeDictionaryIntoElement(FrameworkElement element, ResourceDictionary Dict)
  {
    if (!(Dict.Source != (Uri) null) || element.IsContainsInMergedDictionaryPath(Dict.Source.ToString()))
      return;
    element.Resources.MergedDictionaries.Add(Dict);
    element.AddIntoMergedDictionaryPath(Dict.Source.ToString());
  }

  private static void ClearDictionary(FrameworkElement element)
  {
    if (element.Resources.MergedDictionaries.Count == 0 || element.GetMergedDictionaryPath().Count == 0)
      return;
    Collection<ResourceDictionary> collection = new Collection<ResourceDictionary>();
    foreach (ResourceDictionary mergedDictionary in element.Resources.MergedDictionaries)
      collection.Add(mergedDictionary);
    int count = collection.Count;
    for (int index = 0; index <= count - 1; ++index)
    {
      ResourceDictionary resourceDictionary = collection[index];
      if (resourceDictionary.Source != (Uri) null && element.GetMergedDictionaryPath().Contains(resourceDictionary.Source.ToString()))
      {
        SkinStorage.IsSkinNotChanged = true;
        element.Resources.MergedDictionaries.Remove(resourceDictionary);
        SkinStorage.IsSkinNotChanged = false;
        element.GetMergedDictionaryPath().Remove(resourceDictionary.Source.ToString());
      }
    }
    collection.Clear();
  }

  private static void element_Unloaded(object sender, RoutedEventArgs e)
  {
    FrameworkElement element = sender as FrameworkElement;
    element.Unloaded -= new RoutedEventHandler(SkinStorage.element_Unloaded);
    element.ClearMergedDictionaryPath();
    SkinStorage.Root.Remove(sender as FrameworkElement);
  }
}
