// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.Empty
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace HandyControl.Controls;

public class Empty : ContentControl
{
  public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(nameof (Description), typeof (object), typeof (Empty), new PropertyMetadata((object) null));
  public static readonly DependencyProperty LogoProperty = DependencyProperty.Register(nameof (Logo), typeof (object), typeof (Empty), new PropertyMetadata((object) null));
  public static readonly DependencyProperty ShowEmptyProperty = DependencyProperty.RegisterAttached("ShowEmpty", typeof (bool), typeof (Empty), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits));

  public object Description
  {
    get => this.GetValue(Empty.DescriptionProperty);
    set => this.SetValue(Empty.DescriptionProperty, value);
  }

  public object Logo
  {
    get => this.GetValue(Empty.LogoProperty);
    set => this.SetValue(Empty.LogoProperty, value);
  }

  public static void SetShowEmpty(DependencyObject element, bool value)
  {
    element.SetValue(Empty.ShowEmptyProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetShowEmpty(DependencyObject element)
  {
    return (bool) element.GetValue(Empty.ShowEmptyProperty);
  }
}
