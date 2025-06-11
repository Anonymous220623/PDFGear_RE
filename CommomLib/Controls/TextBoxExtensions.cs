// Decompiled with JetBrains decompiler
// Type: CommomLib.Controls.TextBoxExtensions
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace CommomLib.Controls;

public static class TextBoxExtensions
{
  public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.RegisterAttached("Placeholder", typeof (string), typeof (TextBoxExtensions), new PropertyMetadata((object) string.Empty));
  public static readonly DependencyProperty PlaceholderForegroundProperty = DependencyProperty.RegisterAttached("PlaceholderForeground", typeof (Brush), typeof (TextBoxExtensions), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty PlaceholderFontSizeProperty = DependencyProperty.RegisterAttached("PlaceholderFontSize", typeof (double), typeof (TextBoxExtensions), new PropertyMetadata((object) 12.0));
  public static readonly DependencyProperty PlaceholderFontFamilyProperty = DependencyProperty.RegisterAttached("PlaceholderFontFamily", typeof (FontFamily), typeof (TextBoxExtensions), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty PlaceholderPaddingProperty = DependencyProperty.RegisterAttached("PlaceholderPadding", typeof (Thickness), typeof (TextBoxExtensions), new PropertyMetadata((object) new Thickness(0.0)));
  private static readonly DependencyPropertyKey IsEmptyPropertyKey = DependencyProperty.RegisterAttachedReadOnly("IsEmpty", typeof (bool), typeof (TextBoxExtensions), (PropertyMetadata) new FrameworkPropertyMetadata((object) false));
  internal static readonly DependencyProperty IsEmptyTracerEnabledProperty = DependencyProperty.RegisterAttached("IsEmptyTracerEnabled", typeof (bool), typeof (TextBoxExtensions), new PropertyMetadata((object) false, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is Control sender2) || object.Equals(a.NewValue, a.OldValue))
      return;
    TextBox source1 = sender2 as TextBox;
    PasswordBox source2 = sender2 as PasswordBox;
    if (source1 == null && source2 == null)
      return;
    if (source1 != null)
      WeakEventManager<TextBox, TextChangedEventArgs>.RemoveHandler(source1, "TextChanged", new EventHandler<TextChangedEventArgs>(TextBoxExtensions.OnTextChanged));
    if (source2 != null)
      WeakEventManager<PasswordBox, RoutedEventArgs>.RemoveHandler(source2, "PasswordChanged", new EventHandler<RoutedEventArgs>(TextBoxExtensions.OnTextChanged));
    if (a.NewValue is bool newValue2 && newValue2)
    {
      TextBoxExtensions.OnTextChanged((object) sender2, (RoutedEventArgs) null);
      if (source1 != null)
        WeakEventManager<TextBox, TextChangedEventArgs>.AddHandler(source1, "TextChanged", new EventHandler<TextChangedEventArgs>(TextBoxExtensions.OnTextChanged));
      if (source2 == null)
        return;
      WeakEventManager<PasswordBox, RoutedEventArgs>.AddHandler(source2, "PasswordChanged", new EventHandler<RoutedEventArgs>(TextBoxExtensions.OnTextChanged));
    }
    else
      TextBoxExtensions.SetIsEmpty(sender2, false);
  })));
  public static readonly DependencyProperty IsOnlyNumberProperty = DependencyProperty.RegisterAttached("IsOnlyNumber", typeof (bool), typeof (TextBox), new PropertyMetadata((object) false, (PropertyChangedCallback) ((s, e) =>
  {
    if (!(s is TextBox source4))
      return;
    source4.SetValue(InputMethod.IsInputMethodEnabledProperty, (object) !(bool) e.NewValue);
    WeakEventManager<TextBox, TextCompositionEventArgs>.RemoveHandler(source4, "PreviewTextInput", new EventHandler<TextCompositionEventArgs>(TextBoxExtensions.TxtInput));
    if (!(bool) e.NewValue)
      return;
    WeakEventManager<TextBox, TextCompositionEventArgs>.AddHandler(source4, "PreviewTextInput", new EventHandler<TextCompositionEventArgs>(TextBoxExtensions.TxtInput));
  })));

  public static string GetPlaceholder(Control obj)
  {
    return (string) obj.GetValue(TextBoxExtensions.PlaceholderProperty);
  }

  public static void SetPlaceholder(Control obj, string value)
  {
    obj.SetValue(TextBoxExtensions.PlaceholderProperty, (object) value);
  }

  public static Brush GetPlaceholderForeground(Control obj)
  {
    return (Brush) obj.GetValue(TextBoxExtensions.PlaceholderForegroundProperty);
  }

  public static void SetPlaceholderForeground(Control obj, Brush value)
  {
    obj.SetValue(TextBoxExtensions.PlaceholderForegroundProperty, (object) value);
  }

  public static double GetPlaceholderFontSize(Control obj)
  {
    return (double) obj.GetValue(TextBoxExtensions.PlaceholderFontSizeProperty);
  }

  public static void SetPlaceholderFontSize(Control obj, double value)
  {
    obj.SetValue(TextBoxExtensions.PlaceholderFontSizeProperty, (object) value);
  }

  public static FontFamily GetPlaceholderFontFamily(Control obj)
  {
    return (FontFamily) obj.GetValue(TextBoxExtensions.PlaceholderFontFamilyProperty);
  }

  public static void SetPlaceholderFontFamily(Control obj, FontFamily value)
  {
    obj.SetValue(TextBoxExtensions.PlaceholderFontFamilyProperty, (object) value);
  }

  public static Thickness GetPlaceholderPadding(DependencyObject obj)
  {
    return (Thickness) obj.GetValue(TextBoxExtensions.PlaceholderPaddingProperty);
  }

  public static void SetPlaceholderPadding(DependencyObject obj, Thickness value)
  {
    obj.SetValue(TextBoxExtensions.PlaceholderPaddingProperty, (object) value);
  }

  public static bool GetIsEmpty(Control obj)
  {
    return (bool) obj.GetValue(TextBoxExtensions.IsEmptyProperty);
  }

  private static void SetIsEmpty(Control obj, bool value)
  {
    obj.SetValue(TextBoxExtensions.IsEmptyPropertyKey, (object) value);
  }

  public static DependencyProperty IsEmptyProperty
  {
    get => TextBoxExtensions.IsEmptyPropertyKey.DependencyProperty;
  }

  internal static bool GetIsEmptyTracerEnabled(Control obj)
  {
    return (bool) obj.GetValue(TextBoxExtensions.IsEmptyTracerEnabledProperty);
  }

  internal static void SetIsEmptyTracerEnabled(Control obj, bool value)
  {
    obj.SetValue(TextBoxExtensions.IsEmptyTracerEnabledProperty, (object) value);
  }

  private static void OnTextChanged(object sender, RoutedEventArgs e)
  {
    if (sender is TextBox textBox)
      TextBoxExtensions.SetIsEmpty((Control) textBox, string.IsNullOrEmpty(textBox.Text));
    if (!(sender is PasswordBox passwordBox))
      return;
    TextBoxExtensions.SetIsEmpty((Control) passwordBox, passwordBox.SecurePassword.Length == 0);
  }

  public static bool GetIsOnlyNumber(DependencyObject obj)
  {
    return (bool) obj.GetValue(TextBoxExtensions.IsOnlyNumberProperty);
  }

  public static void SetIsOnlyNumber(DependencyObject obj, bool value)
  {
    obj.SetValue(TextBoxExtensions.IsOnlyNumberProperty, (object) value);
  }

  private static void TxtInput(object sender, TextCompositionEventArgs e)
  {
    e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
  }
}
