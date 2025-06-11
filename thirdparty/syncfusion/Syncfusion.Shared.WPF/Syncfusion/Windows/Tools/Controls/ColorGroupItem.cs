// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.Controls.ColorGroupItem
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Windows.Shared;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Tools.Controls;

public class ColorGroupItem : Control, IDisposable
{
  public Collection<ColorGroupItem> ColorGroupItemsCollection = new Collection<ColorGroupItem>();
  public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof (Color), typeof (Brush), typeof (ColorGroupItem), new PropertyMetadata((object) new SolidColorBrush(Colors.Black)));
  public static readonly DependencyProperty VariantsProperty = DependencyProperty.Register(nameof (Variants), typeof (bool), typeof (ColorGroupItem), new PropertyMetadata((object) false, new PropertyChangedCallback(ColorGroupItem.IsVariantsChanged)));
  public new static readonly DependencyProperty WidthProperty = DependencyProperty.Register(nameof (BorderWidth), typeof (double), typeof (ColorGroupItem), new PropertyMetadata((object) 0.0));
  public new static readonly DependencyProperty HeightProperty = DependencyProperty.Register(nameof (BorderHeight), typeof (double), typeof (ColorGroupItem), new PropertyMetadata((object) 0.0));
  public static readonly DependencyProperty ColorNameProperty = DependencyProperty.Register(nameof (ColorName), typeof (string), typeof (ColorGroupItem), new PropertyMetadata((object) "hi"));
  public static readonly DependencyProperty BorderMarginProperty = DependencyProperty.Register(nameof (BorderMargin), typeof (Thickness), typeof (ColorGroupItem), new PropertyMetadata((object) new Thickness(0.0, 0.0, 0.0, 0.0)));
  public static readonly DependencyProperty ItemMarginProperty = DependencyProperty.Register(nameof (ItemMargin), typeof (Thickness), typeof (ColorGroupItem), new PropertyMetadata((object) new Thickness(2.0, 0.0, 2.0, 0.0)));
  public new static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register(nameof (BorderThick), typeof (Thickness), typeof (ColorGroupItem), new PropertyMetadata((object) new Thickness(1.0, 1.0, 1.0, 1.0)));
  public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(nameof (IsSelected), typeof (bool), typeof (ColorGroupItem), new PropertyMetadata((object) false));
  internal string colorname;
  internal ColorGroup item;
  internal bool setFocus;
  internal bool IsMouseOver;
  internal FrameworkElement colorGroupItemBorder;

  public string ColorName
  {
    get => (string) this.GetValue(ColorGroupItem.ColorNameProperty);
    set => this.SetValue(ColorGroupItem.ColorNameProperty, (object) value);
  }

  public bool Variants
  {
    get => (bool) this.GetValue(ColorGroupItem.VariantsProperty);
    set => this.SetValue(ColorGroupItem.VariantsProperty, (object) value);
  }

  public Brush Color
  {
    get => (Brush) this.GetValue(ColorGroupItem.ColorProperty);
    set => this.SetValue(ColorGroupItem.ColorProperty, (object) value);
  }

  public Thickness BorderMargin
  {
    get => (Thickness) this.GetValue(ColorGroupItem.BorderMarginProperty);
    set => this.SetValue(ColorGroupItem.BorderMarginProperty, (object) value);
  }

  public double BorderWidth
  {
    get => (double) this.GetValue(ColorGroupItem.WidthProperty);
    set => this.SetValue(ColorGroupItem.WidthProperty, (object) value);
  }

  public double BorderHeight
  {
    get => (double) this.GetValue(ColorGroupItem.HeightProperty);
    set => this.SetValue(ColorGroupItem.HeightProperty, (object) value);
  }

  public Thickness BorderThick
  {
    get => (Thickness) this.GetValue(ColorGroupItem.BorderThicknessProperty);
    set => this.SetValue(ColorGroupItem.BorderThicknessProperty, (object) value);
  }

  public Thickness ItemMargin
  {
    get => (Thickness) this.GetValue(ColorGroupItem.ItemMarginProperty);
    set => this.SetValue(ColorGroupItem.ItemMarginProperty, (object) value);
  }

  public bool IsSelected
  {
    get => (bool) this.GetValue(ColorGroupItem.IsSelectedProperty);
    set => this.SetValue(ColorGroupItem.IsSelectedProperty, (object) value);
  }

  public ColorGroupItem() => this.DefaultStyleKey = (object) typeof (ColorGroupItem);

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    if (this.colorGroupItemBorder != null)
    {
      this.colorGroupItemBorder.MouseEnter -= new MouseEventHandler(this.BorderMouseMove);
      this.colorGroupItemBorder.MouseLeave -= new MouseEventHandler(this.ColorGroupItemBorderMouseLeave);
      this.colorGroupItemBorder.MouseLeftButtonDown -= new MouseButtonEventHandler(this.ColorGroupItemBorderMouseLeftButtonDown);
      this.colorGroupItemBorder.KeyDown += new KeyEventHandler(this.ColorGroupItemBorder_KeyDown);
    }
    if (this.GetTemplateChild("Ic1") is ItemsControl templateChild)
      templateChild.ItemsSource = (IEnumerable) this.ColorGroupItemsCollection;
    this.colorGroupItemBorder = this.GetTemplateChild("ItemBorder") as FrameworkElement;
    if (this.colorGroupItemBorder != null)
    {
      this.colorGroupItemBorder.MouseEnter += new MouseEventHandler(this.BorderMouseMove);
      this.colorGroupItemBorder.MouseLeave += new MouseEventHandler(this.ColorGroupItemBorderMouseLeave);
      this.colorGroupItemBorder.MouseLeftButtonDown += new MouseButtonEventHandler(this.ColorGroupItemBorderMouseLeftButtonDown);
    }
    if (VisualUtils.FindAncestor((Visual) this, typeof (ColorPickerPalette)) is ColorPickerPalette ancestor1)
    {
      BindingUtils.SetBinding((DependencyObject) this, (object) ancestor1, ColorGroupItem.HeightProperty, (object) ColorPickerPalette.BorderHeightProperty);
      BindingUtils.SetBinding((DependencyObject) this, (object) ancestor1, ColorGroupItem.WidthProperty, (object) ColorPickerPalette.BorderWidthProperty);
    }
    if (ancestor1 == null && VisualUtils.FindAncestor((Visual) this, typeof (ColorGroup)) is ColorGroup ancestor2 && ancestor2.colorpicker != null)
    {
      ColorPickerPalette colorpicker = ancestor2.colorpicker;
      BindingUtils.SetBinding((DependencyObject) this, (object) colorpicker, ColorGroupItem.HeightProperty, (object) ColorPickerPalette.BorderHeightProperty);
      BindingUtils.SetBinding((DependencyObject) this, (object) colorpicker, ColorGroupItem.WidthProperty, (object) ColorPickerPalette.BorderWidthProperty);
    }
    if (!this.setFocus)
      return;
    Keyboard.Focus((IInputElement) this.colorGroupItemBorder);
    this.setFocus = false;
  }

  private void ColorGroupItemBorder_KeyDown(object sender, KeyEventArgs e)
  {
    if (e.Key != Key.Return)
      return;
    this.SelectColor(sender as FrameworkElement);
  }

  internal static ColorGroup GetBrushEditParentFromChildren(FrameworkElement element)
  {
    parentFromChildren = (ColorGroup) null;
    if (element != null && !(element is ColorGroup parentFromChildren))
    {
      while (element != null)
      {
        element = VisualTreeHelper.GetParent((DependencyObject) element) as FrameworkElement;
        if (element is ColorGroup)
        {
          parentFromChildren = (ColorGroup) element;
          break;
        }
      }
    }
    return parentFromChildren;
  }

  internal Brush Lighten(System.Windows.Media.Color inColor, int level, out string name, string color)
  {
    byte a = inColor.A;
    byte r = inColor.R;
    byte g = inColor.G;
    byte b = inColor.B;
    double num1 = (double) ((int) byte.MaxValue - (int) r);
    double num2 = (double) ((int) byte.MaxValue - (int) g);
    double num3 = (double) ((int) byte.MaxValue - (int) b);
    name = string.Empty;
    if (r == byte.MaxValue && g == byte.MaxValue && b == byte.MaxValue)
    {
      if (level == 1)
      {
        int num4;
        b = (byte) (num4 = (int) (byte) ((double) r - (double) r * 0.05));
        g = (byte) num4;
        r = (byte) num4;
        name = $"{color}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} 5%";
      }
      if (level == 2)
      {
        int num5;
        b = (byte) (num5 = (int) (byte) ((double) r - (double) r * 0.15));
        g = (byte) num5;
        r = (byte) num5;
        name = $"{color}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} 15%";
      }
      if (level == 3)
      {
        int num6;
        b = (byte) (num6 = (int) (byte) ((double) r - (double) r * 0.25));
        g = (byte) num6;
        r = (byte) num6;
        name = $"{color}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} 25%";
      }
      if (level == 4)
      {
        int num7;
        b = (byte) (num7 = (int) (byte) ((double) r - (double) r * 0.35));
        g = (byte) num7;
        r = (byte) num7;
        name = $"{color}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} 35%";
      }
      if (level == 5)
      {
        int num8;
        b = (byte) (num8 = (int) (byte) ((double) r - (double) r * 0.5));
        g = (byte) num8;
        r = (byte) num8;
        name = $"{color}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} 50%";
      }
      return (Brush) new SolidColorBrush(System.Windows.Media.Color.FromArgb(a, r, g, b));
    }
    if (r == (byte) 0 && g == (byte) 0 && b == (byte) 0)
    {
      if (level == 1)
      {
        int maxValue;
        b = (byte) (maxValue = (int) sbyte.MaxValue);
        g = (byte) maxValue;
        r = (byte) maxValue;
        name = $"{color}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "LighterText")} 50%";
      }
      if (level == 2)
      {
        int num9;
        b = (byte) (num9 = 89);
        g = (byte) num9;
        r = (byte) num9;
        name = $"{color}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "LighterText")} 35%";
      }
      if (level == 3)
      {
        int num10;
        b = (byte) (num10 = 63 /*0x3F*/);
        g = (byte) num10;
        r = (byte) num10;
        name = $"{color}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "LighterText")} 25%";
      }
      if (level == 4)
      {
        int num11;
        b = (byte) (num11 = 38);
        g = (byte) num11;
        r = (byte) num11;
        name = $"{color}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "LighterText")} 15%";
      }
      if (level == 5)
      {
        int num12;
        b = (byte) (num12 = 12);
        g = (byte) num12;
        r = (byte) num12;
        name = $"{color}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "LighterText")} 5%";
      }
      return (Brush) new SolidColorBrush(System.Windows.Media.Color.FromArgb(a, r, g, b));
    }
    if (r >= (byte) 220 && g >= (byte) 220 && b >= (byte) 220 || (int) r + (int) g + (int) b > 690)
    {
      if (level == 1)
      {
        r = (byte) ((double) r - num1);
        g = (byte) ((double) g - num2);
        b = (byte) ((double) b - num3);
        name = $"{color}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} 10%";
      }
      if (level == 2)
      {
        r = (byte) ((double) r - num1 * 2.5);
        g = (byte) ((double) g - num2 * 2.5);
        b = (byte) ((double) b - num3 * 2.5);
        name = $"{color}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} 25%";
      }
      if (level == 3)
      {
        r = (byte) ((double) r - num1 * 5.0);
        g = (byte) ((double) g - num2 * 5.0);
        b = (byte) ((double) b - num3 * 5.0);
        name = $"{color}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} 50%";
      }
      if (level == 4)
      {
        r = (byte) (((double) r - num1 * 5.0) / 2.0);
        g = (byte) (((double) g - num2 * 5.0) / 2.0);
        b = (byte) (((double) b - num3 * 5.0) / 2.0);
        name = $"{color}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} 75%";
      }
      if (level == 5)
      {
        r = (byte) (((double) r - num1 * 5.0) / 6.0);
        g = (byte) (((double) g - num2 * 5.0) / 6.0);
        b = (byte) (((double) b - num3 * 5.0) / 6.0);
        name = $"{color}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} 90%";
      }
      if (inColor.R == byte.MaxValue)
        r = byte.MaxValue;
      if (inColor.G == byte.MaxValue)
        g = byte.MaxValue;
      if (inColor.B == byte.MaxValue)
        b = byte.MaxValue;
    }
    else
    {
      if (level == 1)
      {
        r = (byte) ((double) r + num1 * 0.8);
        g = (byte) ((double) g + num2 * 0.8);
        b = (byte) ((double) b + num3 * 0.8);
        name = $"{color}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "LighterText")} 80%";
      }
      if (level == 2)
      {
        r = (byte) ((double) r + num1 * 0.6);
        g = (byte) ((double) g + num2 * 0.6);
        b = (byte) ((double) b + num3 * 0.6);
        name = $"{color}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "LighterText")} 60%";
      }
      if (level == 3)
      {
        r = (byte) ((double) r + num1 * 0.4);
        g = (byte) ((double) g + num2 * 0.4);
        b = (byte) ((double) b + num3 * 0.4);
        name = $"{color}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "LighterText")} 40%";
      }
      if (level == 4)
      {
        r -= (byte) ((uint) r / 4U);
        g -= (byte) ((uint) g / 4U);
        b -= (byte) ((uint) b / 4U);
        name = $"{color}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} 25%";
      }
      if (level == 5)
      {
        double num13 = (double) ((int) r - (int) r / 4);
        double num14 = (double) ((int) g - (int) g / 4);
        double num15 = (double) ((int) b - (int) b / 4);
        r = (byte) (num13 - num13 / 3.0);
        g = (byte) (num14 - num14 / 3.0);
        b = (byte) (num15 - num15 / 3.0);
        name = $"{color}, {SharedLocalizationResourceAccessor.Instance.GetString(CultureInfo.CurrentUICulture, "DarkerText")} 50%";
      }
    }
    return (Brush) new SolidColorBrush(System.Windows.Media.Color.FromArgb(a, r, g, b));
  }

  private static void IsVariantsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
  {
    ((ColorGroupItem) o).IsVariantsChanged(e);
  }

  protected virtual void IsVariantsChanged(DependencyPropertyChangedEventArgs e)
  {
    if (!this.Variants)
      return;
    System.Windows.Media.Color color = ((SolidColorBrush) this.Color).Color;
    this.ColorGroupItemsCollection.Add(new ColorGroupItem()
    {
      Color = this.Lighten(color, 1, out this.colorname, this.ColorName),
      Variants = false,
      ColorName = this.colorname,
      BorderThick = new Thickness(1.0, 1.0, 1.0, 0.0)
    });
    this.ColorGroupItemsCollection.Add(new ColorGroupItem()
    {
      Color = this.Lighten(color, 2, out this.colorname, this.ColorName),
      Variants = false,
      ColorName = this.colorname,
      BorderThick = new Thickness(1.0, 0.0, 1.0, 0.0)
    });
    this.ColorGroupItemsCollection.Add(new ColorGroupItem()
    {
      Color = this.Lighten(color, 3, out this.colorname, this.ColorName),
      Variants = false,
      ColorName = this.colorname,
      BorderThick = new Thickness(1.0, 0.0, 1.0, 0.0)
    });
    this.ColorGroupItemsCollection.Add(new ColorGroupItem()
    {
      Color = this.Lighten(color, 4, out this.colorname, this.ColorName),
      Variants = false,
      ColorName = this.colorname,
      BorderThick = new Thickness(1.0, 0.0, 1.0, 0.0)
    });
    this.ColorGroupItemsCollection.Add(new ColorGroupItem()
    {
      Color = this.Lighten(color, 5, out this.colorname, this.ColorName),
      Variants = false,
      ColorName = this.colorname,
      BorderThick = new Thickness(1.0, 0.0, 1.0, 1.0)
    });
  }

  private void ColorGroupItemBorderMouseLeave(object sender, MouseEventArgs e)
  {
    this.IsMouseOver = false;
  }

  private void ColorGroupItemBorder_LostFocus(object sender, RoutedEventArgs e)
  {
    this.IsSelected = false;
  }

  private void ColorGroupItemBorderMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    FrameworkElement frameworkElement = sender as FrameworkElement;
    Keyboard.Focus((IInputElement) frameworkElement);
    this.SelectColor(frameworkElement);
  }

  internal void FocusBorder()
  {
    this.colorGroupItemBorder = this.GetTemplateChild("ColorPickerBorder") as FrameworkElement;
    if (this.colorGroupItemBorder == null)
      return;
    this.colorGroupItemBorder.Focus();
  }

  protected override void OnGotFocus(RoutedEventArgs e) => this.FocusBorder();

  internal void SelectColor(FrameworkElement border)
  {
    this.IsMouseOver = false;
    this.IsSelected = true;
    this.item = ColorGroupItem.GetBrushEditParentFromChildren(border);
    if (this.item.colorpicker.SelectedItem != null)
      this.item.colorpicker.SelectedItem.IsSelected = false;
    this.IsMouseOver = false;
    this.IsSelected = true;
    if (this.item.colorpicker.IsSelected)
      this.item.colorpicker.IsSelected = false;
    if (this.item.colorpicker.Popup != null)
      this.item.colorpicker.Popup.IsOpen = false;
    this.item.colorpicker.UnwiredEvents();
    this.item.colorpicker.IsChecked = false;
    this.item.colorpicker.SelectedItem = this;
    if (this.item.colorpicker.SelectedMoreColor != null)
      this.item.colorpicker.SelectedMoreColor = (PolygonItem) null;
    this.item.colorpicker.ColorName = this.ColorName;
    this.item.colorpicker.Color = ((SolidColorBrush) this.Color).Color;
    this.item.colorpicker.RaiseCommand();
  }

  private void r_MouseLeave(object sender, MouseEventArgs e) => this.IsMouseOver = false;

  private void BorderMouseMove(object sender, MouseEventArgs e) => this.IsMouseOver = true;

  internal void Dispose(bool disposing)
  {
    if (this.colorGroupItemBorder != null)
    {
      this.colorGroupItemBorder.MouseEnter -= new MouseEventHandler(this.BorderMouseMove);
      this.colorGroupItemBorder.MouseLeave -= new MouseEventHandler(this.ColorGroupItemBorderMouseLeave);
      this.colorGroupItemBorder.MouseLeftButtonDown -= new MouseButtonEventHandler(this.ColorGroupItemBorderMouseLeftButtonDown);
    }
    if (this.ColorGroupItemsCollection != null)
    {
      for (int index = 0; index > this.ColorGroupItemsCollection.Count; ++index)
        this.ColorGroupItemsCollection[index].Dispose();
      this.ColorGroupItemsCollection.Clear();
      this.ColorGroupItemsCollection = (Collection<ColorGroupItem>) null;
    }
    this.colorname = (string) null;
    if (this.item == null)
      return;
    this.item = (ColorGroup) null;
  }

  public void Dispose() => this.Dispose(true);
}
