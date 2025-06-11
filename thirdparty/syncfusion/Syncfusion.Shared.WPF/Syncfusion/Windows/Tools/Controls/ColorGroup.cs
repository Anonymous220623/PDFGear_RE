// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.Controls.ColorGroup
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.Windows.Tools.Controls;

public class ColorGroup : Control, IDisposable
{
  public ColorPickerPalette colorpicker;
  public static readonly DependencyProperty HeaderNameProperty = DependencyProperty.Register(nameof (HeaderName), typeof (string), typeof (ColorGroup), new PropertyMetadata(new PropertyChangedCallback(ColorGroup.IsHeaderChanged)));
  public static readonly DependencyProperty HeaderVisibilityProperty = DependencyProperty.Register(nameof (HeaderVisibility), typeof (Visibility), typeof (ColorGroup), new PropertyMetadata((object) Visibility.Visible));
  internal static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof (color), typeof (Brush), typeof (ColorGroup), new PropertyMetadata(new PropertyChangedCallback(ColorGroup.IsColorChanged)));
  public static readonly DependencyProperty DataSourceProperty = DependencyProperty.Register(nameof (DataSource), typeof (ObservableCollection<ColorGroupItem>), typeof (ColorGroup), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty PanelVisibilityProperty = DependencyProperty.Register(nameof (PanelVisibility), typeof (Visibility), typeof (ColorGroup), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty ThemeHeaderForeGroundProperty = DependencyProperty.Register(nameof (ThemeHeaderForeGround), typeof (Brush), typeof (ColorGroup), new PropertyMetadata((object) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 50, (byte) 21, (byte) 110))));
  public static readonly DependencyProperty ThemeHeaderBackGroundProperty = DependencyProperty.Register(nameof (ThemeHeaderBackGround), typeof (Brush), typeof (ColorGroup), new PropertyMetadata((object) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 221, (byte) 231, (byte) 238))));
  public new static readonly DependencyProperty NameProperty = DependencyProperty.Register(nameof (ColorName), typeof (string), typeof (ColorGroup), new PropertyMetadata((object) "hi"));
  internal ItemsControl colorGroupItemsControl;
  public Rectangle cgHeaderName;
  public TextBlock cgHeaderTextBox;

  public Visibility PanelVisibility
  {
    get => (Visibility) this.GetValue(ColorGroup.PanelVisibilityProperty);
    set => this.SetValue(ColorGroup.PanelVisibilityProperty, (object) value);
  }

  public Visibility HeaderVisibility
  {
    get => (Visibility) this.GetValue(ColorGroup.HeaderVisibilityProperty);
    set => this.SetValue(ColorGroup.HeaderVisibilityProperty, (object) value);
  }

  public Brush color
  {
    get => (Brush) this.GetValue(ColorGroup.ColorProperty);
    set => this.SetValue(ColorGroup.ColorProperty, (object) value);
  }

  public Brush ThemeHeaderForeGround
  {
    get => (Brush) this.GetValue(ColorGroup.ThemeHeaderForeGroundProperty);
    set => this.SetValue(ColorGroup.ThemeHeaderForeGroundProperty, (object) value);
  }

  public Brush ThemeHeaderBackGround
  {
    get => (Brush) this.GetValue(ColorGroup.ThemeHeaderBackGroundProperty);
    set => this.SetValue(ColorGroup.ThemeHeaderBackGroundProperty, (object) value);
  }

  public ObservableCollection<ColorGroupItem> DataSource
  {
    get => (ObservableCollection<ColorGroupItem>) this.GetValue(ColorGroup.DataSourceProperty);
    set => this.SetValue(ColorGroup.DataSourceProperty, (object) value);
  }

  public string ColorName
  {
    get => (string) this.GetValue(ColorGroup.NameProperty);
    set => this.SetValue(ColorGroup.NameProperty, (object) value);
  }

  public string HeaderName
  {
    get => (string) this.GetValue(ColorGroup.HeaderNameProperty);
    set => this.SetValue(ColorGroup.HeaderNameProperty, (object) value);
  }

  public ColorGroup() => this.DefaultStyleKey = (object) typeof (ColorGroup);

  public override void OnApplyTemplate()
  {
    this.colorGroupItemsControl = this.GetTemplateChild("Ic") as ItemsControl;
    base.OnApplyTemplate();
    this.cgHeaderName = this.GetTemplateChild("CGHeaderName") as Rectangle;
    this.cgHeaderTextBox = this.GetTemplateChild("CGTextBox") as TextBlock;
    if (this.cgHeaderName != null)
      this.cgHeaderName.MouseLeftButtonDown += new MouseButtonEventHandler(this.cgHeaderName_MouseLeftButtonDown);
    if (this.cgHeaderTextBox == null)
      return;
    this.cgHeaderTextBox.MouseLeftButtonDown += new MouseButtonEventHandler(this.cgHeaderName_MouseLeftButtonDown);
  }

  public void cgHeaderName_MouseLeftButtonDown(object sender, MouseEventArgs args)
  {
    if (this.colorpicker == null || this.colorpicker.Popup == null)
      return;
    if (this.colorpicker.Mode != PickerMode.Palette)
      this.colorpicker.Popup.IsOpen = true;
    this.colorpicker.updownclick = true;
  }

  private static void IsHeaderChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
  {
    ((ColorGroup) o).IsHeaderChanged(e);
  }

  protected virtual void IsHeaderChanged(DependencyPropertyChangedEventArgs e)
  {
  }

  private static void IsColorChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
  {
    ((ColorGroup) o).IsColorChanged(e);
  }

  protected virtual void IsColorChanged(DependencyPropertyChangedEventArgs e)
  {
    this.colorpicker.ColorName = this.ColorName;
    this.colorpicker.Color = ((SolidColorBrush) this.color).Color;
  }

  internal void Dispose(bool disposing)
  {
    if (!disposing)
      return;
    if (this.colorGroupItemsControl != null)
    {
      for (int index = 0; index < this.colorGroupItemsControl.Items.Count; ++index)
      {
        if (this.colorGroupItemsControl.Items[index] is ColorGroupItem)
          (this.colorGroupItemsControl.Items[index] as ColorGroupItem).Dispose();
      }
      this.colorGroupItemsControl.ItemsSource = (IEnumerable) new ObservableCollection<object>();
      if (this.colorGroupItemsControl == null)
        this.colorGroupItemsControl.Items.Clear();
    }
    if (this.cgHeaderName != null)
    {
      this.cgHeaderName.MouseLeftButtonDown -= new MouseButtonEventHandler(this.cgHeaderName_MouseLeftButtonDown);
      this.cgHeaderName = (Rectangle) null;
    }
    if (this.cgHeaderTextBox != null)
    {
      this.cgHeaderTextBox.MouseLeftButtonDown -= new MouseButtonEventHandler(this.cgHeaderName_MouseLeftButtonDown);
      this.cgHeaderTextBox = (TextBlock) null;
    }
    if (this.DataSource != null)
    {
      for (int index = 0; index < this.DataSource.Count; ++index)
        this.DataSource[index].Dispose();
      this.DataSource.Clear();
      this.DataSource = (ObservableCollection<ColorGroupItem>) null;
    }
    if (this.colorpicker != null)
      this.colorpicker = (ColorPickerPalette) null;
    BindingOperations.ClearAllBindings((DependencyObject) this);
  }

  public void Dispose() => this.Dispose(true);
}
