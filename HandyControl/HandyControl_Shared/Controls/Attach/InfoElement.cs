// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.InfoElement
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System.Windows;

#nullable disable
namespace HandyControl.Controls;

public class InfoElement : TitleElement
{
  public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.RegisterAttached("Placeholder", typeof (string), typeof (InfoElement), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.Inherits));
  public static readonly DependencyProperty NecessaryProperty = DependencyProperty.RegisterAttached("Necessary", typeof (bool), typeof (InfoElement), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits));
  public static readonly DependencyProperty SymbolProperty = DependencyProperty.RegisterAttached("Symbol", typeof (string), typeof (InfoElement), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.Inherits));
  public static readonly DependencyProperty ContentHeightProperty = DependencyProperty.RegisterAttached("ContentHeight", typeof (double), typeof (InfoElement), (PropertyMetadata) new FrameworkPropertyMetadata((object) 28.0, FrameworkPropertyMetadataOptions.Inherits));
  public static readonly DependencyProperty MinContentHeightProperty = DependencyProperty.RegisterAttached("MinContentHeight", typeof (double), typeof (InfoElement), new PropertyMetadata((object) 28.0));
  public static readonly DependencyProperty MaxContentHeightProperty = DependencyProperty.RegisterAttached("MaxContentHeight", typeof (double), typeof (InfoElement), new PropertyMetadata((object) double.PositiveInfinity));
  public static readonly DependencyProperty RegexPatternProperty = DependencyProperty.RegisterAttached("RegexPattern", typeof (string), typeof (InfoElement), new PropertyMetadata((object) null));
  public static readonly DependencyProperty ShowClearButtonProperty = DependencyProperty.RegisterAttached("ShowClearButton", typeof (bool), typeof (InfoElement), new PropertyMetadata(ValueBoxes.FalseBox));
  public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.RegisterAttached("IsReadOnly", typeof (bool), typeof (InfoElement), new PropertyMetadata(ValueBoxes.FalseBox));

  public static void SetPlaceholder(DependencyObject element, string value)
  {
    element.SetValue(InfoElement.PlaceholderProperty, (object) value);
  }

  public static string GetPlaceholder(DependencyObject element)
  {
    return (string) element.GetValue(InfoElement.PlaceholderProperty);
  }

  public static void SetNecessary(DependencyObject element, bool value)
  {
    element.SetValue(InfoElement.NecessaryProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetNecessary(DependencyObject element)
  {
    return (bool) element.GetValue(InfoElement.NecessaryProperty);
  }

  public static void SetSymbol(DependencyObject element, string value)
  {
    element.SetValue(InfoElement.SymbolProperty, (object) value);
  }

  public static string GetSymbol(DependencyObject element)
  {
    return (string) element.GetValue(InfoElement.SymbolProperty);
  }

  public static void SetContentHeight(DependencyObject element, double value)
  {
    element.SetValue(InfoElement.ContentHeightProperty, (object) value);
  }

  public static double GetContentHeight(DependencyObject element)
  {
    return (double) element.GetValue(InfoElement.ContentHeightProperty);
  }

  public static void SetMinContentHeight(DependencyObject element, double value)
  {
    element.SetValue(InfoElement.MinContentHeightProperty, (object) value);
  }

  public static double GetMinContentHeight(DependencyObject element)
  {
    return (double) element.GetValue(InfoElement.MinContentHeightProperty);
  }

  public static void SetMaxContentHeight(DependencyObject element, double value)
  {
    element.SetValue(InfoElement.MaxContentHeightProperty, (object) value);
  }

  public static double GetMaxContentHeight(DependencyObject element)
  {
    return (double) element.GetValue(InfoElement.MaxContentHeightProperty);
  }

  public static void SetRegexPattern(DependencyObject element, string value)
  {
    element.SetValue(InfoElement.RegexPatternProperty, (object) value);
  }

  public static string GetRegexPattern(DependencyObject element)
  {
    return (string) element.GetValue(InfoElement.RegexPatternProperty);
  }

  public static void SetShowClearButton(DependencyObject element, bool value)
  {
    element.SetValue(InfoElement.ShowClearButtonProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetShowClearButton(DependencyObject element)
  {
    return (bool) element.GetValue(InfoElement.ShowClearButtonProperty);
  }

  public static void SetIsReadOnly(DependencyObject element, bool value)
  {
    element.SetValue(InfoElement.IsReadOnlyProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetIsReadOnly(DependencyObject element)
  {
    return (bool) element.GetValue(InfoElement.IsReadOnlyProperty);
  }
}
