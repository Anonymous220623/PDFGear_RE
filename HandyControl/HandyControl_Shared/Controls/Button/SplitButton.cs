// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.SplitButton
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Data.Enum;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

#nullable disable
namespace HandyControl.Controls;

public class SplitButton : ButtonBase
{
  public static readonly DependencyProperty HitModeProperty = DependencyProperty.Register(nameof (HitMode), typeof (HitMode), typeof (SplitButton), new PropertyMetadata((object) HitMode.Click));
  public static readonly DependencyProperty MaxDropDownHeightProperty = DependencyProperty.Register(nameof (MaxDropDownHeight), typeof (double), typeof (SplitButton), new PropertyMetadata((object) (SystemParameters.PrimaryScreenHeight / 3.0)));
  public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(nameof (IsDropDownOpen), typeof (bool), typeof (SplitButton), new PropertyMetadata(ValueBoxes.FalseBox));
  public static readonly DependencyProperty DropDownContentProperty = DependencyProperty.Register(nameof (DropDownContent), typeof (object), typeof (SplitButton), new PropertyMetadata((object) null));

  public HitMode HitMode
  {
    get => (HitMode) this.GetValue(SplitButton.HitModeProperty);
    set => this.SetValue(SplitButton.HitModeProperty, (object) value);
  }

  public double MaxDropDownHeight
  {
    get => (double) this.GetValue(SplitButton.MaxDropDownHeightProperty);
    set => this.SetValue(SplitButton.MaxDropDownHeightProperty, (object) value);
  }

  public bool IsDropDownOpen
  {
    get => (bool) this.GetValue(SplitButton.IsDropDownOpenProperty);
    set => this.SetValue(SplitButton.IsDropDownOpenProperty, ValueBoxes.BooleanBox(value));
  }

  public object DropDownContent
  {
    get => this.GetValue(SplitButton.DropDownContentProperty);
    set => this.SetValue(SplitButton.DropDownContentProperty, value);
  }

  public SplitButton()
  {
    this.AddHandler(MenuItem.ClickEvent, (Delegate) new RoutedEventHandler(this.ItemsOnClick));
  }

  private void ItemsOnClick(object sender, RoutedEventArgs e)
  {
    if (!(e.OriginalSource is MenuItem))
      return;
    this.SetCurrentValue(SplitButton.IsDropDownOpenProperty, ValueBoxes.FalseBox);
  }

  protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    base.OnPreviewMouseLeftButtonDown(e);
    if (this.HitMode != HitMode.Hover)
      return;
    e.Handled = true;
  }

  protected override void OnMouseEnter(MouseEventArgs e)
  {
    base.OnMouseEnter(e);
    if (this.HitMode != HitMode.Hover)
      return;
    this.SetCurrentValue(SplitButton.IsDropDownOpenProperty, ValueBoxes.TrueBox);
  }
}
