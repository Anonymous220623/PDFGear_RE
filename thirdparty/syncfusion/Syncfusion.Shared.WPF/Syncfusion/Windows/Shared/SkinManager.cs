// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.SkinManager
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class SkinManager : DependencyObject
{
  public static readonly DependencyProperty ActiveColorSchemeProperty = DependencyProperty.RegisterAttached("ActiveColorScheme", typeof (Brush), typeof (SkinManager), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(SkinManager.OnActiveColorSchemeChanged)));

  public static void RemoveDictionaryIfExist(
    FrameworkElement element,
    ResourceDictionary dictionary)
  {
    if (element == null)
      return;
    for (int index = 0; index < element.Resources.MergedDictionaries.Count; ++index)
    {
      if (element.Resources.MergedDictionaries[index].Source == dictionary.Source)
      {
        try
        {
          element.Resources.MergedDictionaries.RemoveAt(index);
        }
        catch
        {
        }
        --index;
      }
    }
  }

  internal static SkinTypeAttribute GetSkinAttribute(FrameworkElement element, string currentskin)
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

  private static void OnActiveColorSchemeChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (e.NewValue is Brush newValue && d is Window)
      (d as Window).Loaded += new RoutedEventHandler(SkinManager.win_Loaded);
    string visualStyle = SkinStorage.GetVisualStyle(d);
    if (newValue == null || !(newValue is SolidColorBrush))
      return;
    Color color = (newValue as SolidColorBrush).Color;
    if (!(d is Visual))
      return;
    foreach (FrameworkElement element1 in VisualUtils.EnumLogicalChildrenOfType(d, typeof (FrameworkElement)))
    {
      if (element1 != null)
      {
        if (element1 is Popup && (element1 as Popup).Child != null)
        {
          IEnumerable<DependencyObject> dependencyObjects = VisualUtils.EnumLogicalChildrenOfType((DependencyObject) (element1 as Popup).Child, typeof (FrameworkElement));
          if (dependencyObjects != null)
          {
            foreach (FrameworkElement element2 in dependencyObjects)
            {
              if (element2 is ScrollViewer)
              {
                SkinTypeAttribute skinAttribute = SkinManager.GetSkinAttribute(element2, visualStyle);
                if (skinAttribute != null)
                {
                  int skinVisualStyle = (int) skinAttribute.SkinVisualStyle;
                  string xamlResource = skinAttribute.XamlResource;
                  Type type = skinAttribute.Type;
                  ResourceDictionary dictionary = SkinColorScheme.ApplyCustomColorScheme(new ResourceDictionary()
                  {
                    Source = new Uri(xamlResource, UriKind.RelativeOrAbsolute)
                  }, color);
                  SkinManager.RemoveDictionaryIfExist(element2, dictionary);
                  element2.Resources.MergedDictionaries.Add(dictionary);
                }
                SkinStorage.SetVisualStyle((DependencyObject) element2, visualStyle);
              }
            }
          }
        }
        if (element1 is ScrollViewer && element1 != null && (element1 as ScrollViewer).Content is Visual)
        {
          IEnumerable<DependencyObject> dependencyObjects = VisualUtils.EnumLogicalChildrenOfType((element1 as ScrollViewer).Content as DependencyObject, typeof (FrameworkElement));
          if (dependencyObjects != null)
          {
            foreach (FrameworkElement element3 in dependencyObjects)
            {
              if (element3 != null)
              {
                SkinTypeAttribute skinAttribute = SkinManager.GetSkinAttribute(element3, visualStyle);
                if (skinAttribute != null)
                {
                  int skinVisualStyle = (int) skinAttribute.SkinVisualStyle;
                  string xamlResource = skinAttribute.XamlResource;
                  Type type = skinAttribute.Type;
                  ResourceDictionary dictionary = SkinColorScheme.ApplyCustomColorScheme(new ResourceDictionary()
                  {
                    Source = new Uri(xamlResource, UriKind.RelativeOrAbsolute)
                  }, color);
                  try
                  {
                    SkinManager.RemoveDictionaryIfExist(element3, dictionary);
                    element3.Resources.MergedDictionaries.Add(dictionary);
                  }
                  catch
                  {
                  }
                }
                SkinStorage.SetVisualStyle((DependencyObject) element3, visualStyle);
              }
            }
          }
        }
        SkinTypeAttribute skinAttribute1 = SkinManager.GetSkinAttribute(element1, visualStyle);
        if (skinAttribute1 != null)
        {
          int skinVisualStyle = (int) skinAttribute1.SkinVisualStyle;
          string xamlResource = skinAttribute1.XamlResource;
          Type type = skinAttribute1.Type;
          ResourceDictionary dictionary = SkinColorScheme.ApplyCustomColorScheme(new ResourceDictionary()
          {
            Source = new Uri(xamlResource, UriKind.RelativeOrAbsolute)
          }, color);
          SkinManager.RemoveDictionaryIfExist(element1, dictionary);
          element1.Resources.MergedDictionaries.Add(dictionary);
        }
        SkinStorage.SetVisualStyle((DependencyObject) element1, visualStyle);
      }
    }
    if (!(d is FrameworkElement))
      return;
    SkinTypeAttribute skinAttribute2 = SkinManager.GetSkinAttribute(d as FrameworkElement, visualStyle);
    if (skinAttribute2 == null)
      return;
    int skinVisualStyle1 = (int) skinAttribute2.SkinVisualStyle;
    string xamlResource1 = skinAttribute2.XamlResource;
    Type type1 = skinAttribute2.Type;
    ResourceDictionary dictionary1 = SkinColorScheme.ApplyCustomColorScheme(new ResourceDictionary()
    {
      Source = new Uri(xamlResource1, UriKind.RelativeOrAbsolute)
    }, color);
    SkinManager.RemoveDictionaryIfExist(d as FrameworkElement, dictionary1);
    (d as FrameworkElement).Resources.MergedDictionaries.Add(dictionary1);
  }

  private static void win_Loaded(object sender, RoutedEventArgs e)
  {
    SolidColorBrush activeColorScheme = SkinManager.GetActiveColorScheme((DependencyObject) sender);
    if (activeColorScheme == null)
      return;
    SkinManager.ApplyActiveColorToMSControls(sender as DependencyObject, activeColorScheme);
  }

  [TypeConverter(typeof (BrushConverter))]
  public static SolidColorBrush GetActiveColorScheme(DependencyObject obj)
  {
    return (SolidColorBrush) obj.GetValue(SkinManager.ActiveColorSchemeProperty);
  }

  private static void ApplyActiveColorToMSControls(DependencyObject obj, SolidColorBrush value)
  {
    string visualStyle = SkinStorage.GetVisualStyle(obj);
    if (!(obj is FrameworkElement frameworkElement))
      return;
    if (!(frameworkElement.Parent is Window))
    {
      if (!(frameworkElement.Parent is ChromelessWindow))
      {
        switch (obj)
        {
          case Window _:
          case ChromelessWindow _:
          case UIElement _:
            break;
          default:
            return;
        }
      }
    }
    try
    {
      ResourceDictionary controlDictionary = SkinStorage.GetMSControlDictionary(visualStyle);
      frameworkElement.Resources.MergedDictionaries.Remove(controlDictionary);
      ResourceDictionary dictionary = SkinColorScheme.ApplyCustomColorScheme(controlDictionary, value.Color);
      SkinManager.RemoveDictionaryIfExist(obj as FrameworkElement, dictionary);
      frameworkElement.Resources.MergedDictionaries.Add(dictionary);
    }
    catch
    {
    }
  }

  public static void SetActiveColorScheme(DependencyObject obj, SolidColorBrush value)
  {
    obj.ClearValue(SkinManager.ActiveColorSchemeProperty);
    obj.SetValue(SkinManager.ActiveColorSchemeProperty, (object) value);
    SkinManager.ApplyActiveColorToMSControls(obj, value);
  }
}
