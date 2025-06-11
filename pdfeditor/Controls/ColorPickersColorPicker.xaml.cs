// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.ColorPickers.ColorPicker
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Controls.ColorPickers;

public partial class ColorPicker : System.Windows.Controls.Control
{
  private FrameworkElement colorFamilyListContainer;
  private FrameworkElement standardColorListContainer;
  private FrameworkElement recentColorListContainer;
  private System.Windows.Controls.Button defaultColorButton;
  private System.Windows.Controls.Button moreColorButton;
  private System.Windows.Controls.Button transparentColorButton;
  public static readonly DependencyProperty TemplateSettingsProperty;
  private static readonly DependencyPropertyKey TemplateSettingsPropertyKey = DependencyProperty.RegisterReadOnly(nameof (TemplateSettings), typeof (ColorPickerTemplateSettings), typeof (ColorPicker), new PropertyMetadata((object) null, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is ColorPicker colorPicker2) || object.Equals(a.OldValue, a.NewValue))
      return;
    if (a.OldValue is ColorPickerTemplateSettings oldValue2)
    {
      oldValue2.TransparentColorEnabledChanged -= new EventHandler(colorPicker2.TemplateSettings_TransparentColorEnabledChanged);
      BindingOperations.ClearBinding((DependencyObject) oldValue2, ColorPickerTemplateSettings.RecentColorsKeyProperty);
    }
    if (a.NewValue is ColorPickerTemplateSettings newValue2)
    {
      newValue2.TransparentColorEnabledChanged += new EventHandler(colorPicker2.TemplateSettings_TransparentColorEnabledChanged);
      BindingOperations.SetBinding((DependencyObject) newValue2, ColorPickerTemplateSettings.RecentColorsKeyProperty, (BindingBase) new System.Windows.Data.Binding(nameof (RecentColorsKey))
      {
        Source = (object) colorPicker2
      });
    }
    colorPicker2.UpdateTransparentColorEnabledState();
  })));
  public static readonly DependencyProperty RecentColorsKeyProperty = DependencyProperty.Register(nameof (RecentColorsKey), typeof (string), typeof (ColorPicker), new PropertyMetadata((object) null, (PropertyChangedCallback) ((s, a) =>
  {
    ColorPicker sender = s as ColorPicker;
    if (sender == null || object.Equals(a.NewValue, a.OldValue))
      return;
    sender.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) (() => sender.UpdateRecentStates()));
  })));
  public static readonly DependencyProperty StandardColorsProperty = DependencyProperty.Register(nameof (StandardColors), typeof (object), typeof (ColorPicker), new PropertyMetadata((object) null, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is ColorPicker colorPicker4) || object.Equals(a.NewValue, a.OldValue))
      return;
    colorPicker4.UpdateStandardStates();
  })));
  public static readonly DependencyProperty DefaultColorProperty = DependencyProperty.Register(nameof (DefaultColor), typeof (System.Windows.Media.Color?), typeof (ColorPicker), new PropertyMetadata((object) null, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is ColorPicker colorPicker6) || object.Equals(a.NewValue, a.OldValue))
      return;
    colorPicker6.UpdateDefaultColor();
  })));
  public static readonly DependencyProperty IsMoreColorEnabledProperty = DependencyProperty.Register(nameof (IsMoreColorEnabled), typeof (bool), typeof (ColorPicker), new PropertyMetadata((object) true, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is ColorPicker colorPicker8) || object.Equals(a.NewValue, a.OldValue))
      return;
    colorPicker8.UpdateMoreColorStates();
  })));
  public static readonly RoutedEvent ItemClickEvent = EventManager.RegisterRoutedEvent("ItemClick", RoutingStrategy.Bubble, typeof (EventHandler<ColorPickerItemClickEventArgs>), typeof (ColorPicker));

  static ColorPicker()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (ColorPicker), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (ColorPicker)));
    ColorPicker.TemplateSettingsProperty = ColorPicker.TemplateSettingsPropertyKey.DependencyProperty;
  }

  public ColorPicker() => this.TemplateSettings = new ColorPickerTemplateSettings();

  private FrameworkElement ColorFamilyListContainer
  {
    get => this.colorFamilyListContainer;
    set
    {
      if (this.colorFamilyListContainer == value)
        return;
      if (this.colorFamilyListContainer != null)
        this.colorFamilyListContainer.RemoveHandler(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, (Delegate) new RoutedEventHandler(this.ColorFamilyItem_Click));
      this.colorFamilyListContainer = value;
      if (this.colorFamilyListContainer == null)
        return;
      this.colorFamilyListContainer.AddHandler(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, (Delegate) new RoutedEventHandler(this.ColorFamilyItem_Click));
    }
  }

  private FrameworkElement StandardColorListContainer
  {
    get => this.standardColorListContainer;
    set
    {
      if (this.standardColorListContainer == value)
        return;
      if (this.standardColorListContainer != null)
        this.standardColorListContainer.RemoveHandler(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, (Delegate) new RoutedEventHandler(this.StandardColorListItem_Click));
      this.standardColorListContainer = value;
      if (this.standardColorListContainer == null)
        return;
      this.standardColorListContainer.AddHandler(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, (Delegate) new RoutedEventHandler(this.StandardColorListItem_Click));
    }
  }

  private FrameworkElement RecentColorListContainer
  {
    get => this.recentColorListContainer;
    set
    {
      if (this.recentColorListContainer == value)
        return;
      if (this.recentColorListContainer != null)
        this.recentColorListContainer.RemoveHandler(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, (Delegate) new RoutedEventHandler(this.RecentColorListItem_Click));
      this.recentColorListContainer = value;
      if (this.recentColorListContainer == null)
        return;
      this.recentColorListContainer.AddHandler(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, (Delegate) new RoutedEventHandler(this.RecentColorListItem_Click));
    }
  }

  private System.Windows.Controls.Button DefaultColorButton
  {
    get => this.defaultColorButton;
    set
    {
      if (this.defaultColorButton == value)
        return;
      if (this.defaultColorButton != null)
        this.defaultColorButton.Click -= new RoutedEventHandler(this.DefaultColorButton_Click);
      this.defaultColorButton = value;
      if (this.defaultColorButton != null)
        this.defaultColorButton.Click += new RoutedEventHandler(this.DefaultColorButton_Click);
      this.UpdateDefaultColor();
    }
  }

  private System.Windows.Controls.Button MoreColorButton
  {
    get => this.moreColorButton;
    set
    {
      if (this.moreColorButton == value)
        return;
      if (this.moreColorButton != null)
        this.moreColorButton.Click -= new RoutedEventHandler(this.MoreColorButton_Click);
      this.moreColorButton = value;
      if (this.moreColorButton == null)
        return;
      this.moreColorButton.Click += new RoutedEventHandler(this.MoreColorButton_Click);
    }
  }

  private System.Windows.Controls.Button TransparentColorButton
  {
    get => this.transparentColorButton;
    set
    {
      if (this.transparentColorButton == value)
        return;
      if (this.transparentColorButton != null)
        this.transparentColorButton.Click -= new RoutedEventHandler(this.TransparentColorButton_Click);
      this.transparentColorButton = value;
      if (this.transparentColorButton == null)
        return;
      this.transparentColorButton.Click += new RoutedEventHandler(this.TransparentColorButton_Click);
    }
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.ColorFamilyListContainer = this.GetTemplateChild("ColorFamilyListContainer") as FrameworkElement;
    this.StandardColorListContainer = this.GetTemplateChild("StandardColorListContainer") as FrameworkElement;
    this.RecentColorListContainer = this.GetTemplateChild("RecentColorListContainer") as FrameworkElement;
    this.DefaultColorButton = this.GetTemplateChild("DefaultColorButton") as System.Windows.Controls.Button;
    this.MoreColorButton = this.GetTemplateChild("MoreColorButton") as System.Windows.Controls.Button;
    this.TransparentColorButton = this.GetTemplateChild("TransparentColorButton") as System.Windows.Controls.Button;
    this.UpdateStandardStates();
    this.UpdateRecentStates();
    this.UpdateMoreColorStates();
    this.UpdateTransparentColorEnabledState();
  }

  public ColorPickerTemplateSettings TemplateSettings
  {
    get => (ColorPickerTemplateSettings) this.GetValue(ColorPicker.TemplateSettingsProperty);
    private set => this.SetValue(ColorPicker.TemplateSettingsPropertyKey, (object) value);
  }

  public string RecentColorsKey
  {
    get => (string) this.GetValue(ColorPicker.RecentColorsKeyProperty);
    set => this.SetValue(ColorPicker.RecentColorsKeyProperty, (object) value);
  }

  public object StandardColors
  {
    get => this.GetValue(ColorPicker.StandardColorsProperty);
    set => this.SetValue(ColorPicker.StandardColorsProperty, value);
  }

  public System.Windows.Media.Color? DefaultColor
  {
    get => (System.Windows.Media.Color?) this.GetValue(ColorPicker.DefaultColorProperty);
    set => this.SetValue(ColorPicker.DefaultColorProperty, (object) value);
  }

  public bool IsMoreColorEnabled
  {
    get => (bool) this.GetValue(ColorPicker.IsMoreColorEnabledProperty);
    set => this.SetValue(ColorPicker.IsMoreColorEnabledProperty, (object) value);
  }

  private void UpdateDefaultColor()
  {
    if (this.DefaultColorButton == null)
      return;
    System.Windows.Media.Color? defaultColor = this.DefaultColor;
    if (defaultColor.HasValue)
    {
      if (this.DefaultColorButton.FindName("Indicator") is System.Windows.Shapes.Rectangle name)
        name.Fill = (System.Windows.Media.Brush) new SolidColorBrush(defaultColor.Value);
      this.DefaultColorButton.Visibility = Visibility.Visible;
    }
    else
    {
      if (this.DefaultColorButton.FindName("Indicator") is System.Windows.Shapes.Rectangle name)
        name.Fill = (System.Windows.Media.Brush) null;
      this.DefaultColorButton.Visibility = Visibility.Collapsed;
    }
  }

  private void UpdateStandardStates()
  {
    if (this.StandardColors == null || this.StandardColors is ICollection standardColors1 && standardColors1.Count == 0)
    {
      VisualStateManager.GoToState((FrameworkElement) this, "StandardColorInvisible", true);
    }
    else
    {
      List<ColorValue> list = this.StandardColors is ICollection standardColors ? standardColors.OfType<ColorValue>().ToList<ColorValue>() : (List<ColorValue>) null;
      bool flag = false;
      for (int index = list.Count - 1; index >= 0; --index)
      {
        if (list[index] == null || list[index].Color.A == (byte) 0)
        {
          flag = true;
          list.RemoveAt(index);
        }
      }
      this.TemplateSettings.IsTransparentColorEnabled = flag;
      this.TemplateSettings.StandardColors = (IList<ColorValue>) list;
      VisualStateManager.GoToState((FrameworkElement) this, "StandardColorVisible", true);
    }
  }

  private void UpdateRecentStates()
  {
    IReadOnlyList<ColorValue> recentColors = this.TemplateSettings?.RecentColors;
    if (recentColors == null || recentColors.Count == 0)
      VisualStateManager.GoToState((FrameworkElement) this, "RecentColorInvisible", true);
    else
      VisualStateManager.GoToState((FrameworkElement) this, "RecentColorVisible", true);
  }

  private void UpdateMoreColorStates()
  {
    if (this.IsMoreColorEnabled)
      VisualStateManager.GoToState((FrameworkElement) this, "MoreColorVisible", true);
    else
      VisualStateManager.GoToState((FrameworkElement) this, "MoreColorInvisible", true);
  }

  private void UpdateTransparentColorEnabledState()
  {
    ColorPickerTemplateSettings templateSettings = this.TemplateSettings;
    if ((templateSettings != null ? (templateSettings.IsTransparentColorEnabled ? 1 : 0) : 0) != 0)
      VisualStateManager.GoToState((FrameworkElement) this, "TransparentColorVisible", true);
    else
      VisualStateManager.GoToState((FrameworkElement) this, "TransparentColorInvisible", true);
  }

  private void TemplateSettings_TransparentColorEnabledChanged(object sender, EventArgs e)
  {
    this.UpdateTransparentColorEnabledState();
  }

  private void DefaultColorButton_Click(object sender, RoutedEventArgs e)
  {
    System.Windows.Media.Color? defaultColor = this.DefaultColor;
    if (!defaultColor.HasValue)
      return;
    this.RaiseItemClickEvent((ColorValue) defaultColor.Value);
  }

  private void ColorFamilyItem_Click(object sender, RoutedEventArgs e)
  {
    if (!(e.OriginalSource is FrameworkElement originalSource) || !(originalSource.DataContext is ColorValue dataContext))
      return;
    this.RaiseItemClickEvent(dataContext);
  }

  private void StandardColorListItem_Click(object sender, RoutedEventArgs e)
  {
    if (!(e.OriginalSource is FrameworkElement originalSource) || !(originalSource.DataContext is ColorValue dataContext))
      return;
    this.RaiseItemClickEvent(dataContext);
  }

  private void RecentColorListItem_Click(object sender, RoutedEventArgs e)
  {
    if (!(e.OriginalSource is FrameworkElement originalSource) || !(originalSource.DataContext is ColorValue dataContext))
      return;
    this.RaiseItemClickEvent(dataContext);
  }

  private void MoreColorButton_Click(object sender, RoutedEventArgs e)
  {
    using (ColorDialog dialog = new ColorDialog())
    {
      dialog.AllowFullOpen = true;
      dialog.FullOpen = true;
      dialog.AnyColor = true;
      dialog.SolidColorOnly = false;
      if (this.StandardColors is ICollection standardColors)
      {
        List<ColorValue> list = standardColors.OfType<ColorValue>().ToList<ColorValue>();
        if (list.Count != 0)
          dialog.CustomColors = list.Select<ColorValue, int>((Func<ColorValue, int>) (c => c.Color.ToColorDialogValue())).ToArray<int>();
      }
      System.Windows.Media.Color? defaultColor = this.DefaultColor;
      if (defaultColor.HasValue)
        dialog.Color = System.Drawing.Color.FromArgb((int) defaultColor.Value.A, (int) defaultColor.Value.R, (int) defaultColor.Value.G, (int) defaultColor.Value.B);
      ColorPicker.WindowWrapper wrapper = ColorPicker.WindowWrapper.Create((Visual) this);
      DialogResult res = DialogResult.Cancel;
      DispatcherFrame frame = new DispatcherFrame();
      frame.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) (() =>
      {
        try
        {
          res = dialog.ShowDialog((System.Windows.Forms.IWin32Window) wrapper);
        }
        finally
        {
          frame.Continue = false;
        }
      }));
      Dispatcher.PushFrame(frame);
      if (res != DialogResult.OK)
        return;
      System.Drawing.Color color = dialog.Color;
      int a = (int) color.A;
      color = dialog.Color;
      int r = (int) color.R;
      color = dialog.Color;
      int g = (int) color.G;
      color = dialog.Color;
      int b = (int) color.B;
      this.RaiseItemClickEvent(new ColorValue(System.Windows.Media.Color.FromArgb((byte) a, (byte) r, (byte) g, (byte) b), true));
    }
  }

  private void TransparentColorButton_Click(object sender, RoutedEventArgs e)
  {
    this.RaiseItemClickEvent(new ColorValue(Colors.Transparent));
  }

  private void RaiseItemClickEvent(ColorValue item)
  {
    if (item == null)
      return;
    this.OnItemClick(item);
    if (!string.IsNullOrEmpty(this.RecentColorsKey) && item.Color.A != (byte) 0)
    {
      object[] objArray = new object[4]
      {
        (object) item.Color.A,
        null,
        null,
        null
      };
      System.Windows.Media.Color color = item.Color;
      objArray[1] = (object) color.R;
      color = item.Color;
      objArray[2] = (object) color.G;
      color = item.Color;
      objArray[3] = (object) color.B;
      if (ConfigManager.AddColorPickerRecentColorsAsync(this.RecentColorsKey, string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", objArray)).GetAwaiter().GetResult())
      {
        this.TemplateSettings?.UpdateRecentColors();
        this.UpdateRecentStates();
      }
    }
    this.RaiseEvent((RoutedEventArgs) new ColorPickerItemClickEventArgs((object) this, item));
  }

  public event EventHandler<ColorPickerItemClickEventArgs> ItemClick
  {
    add => this.AddHandler(ColorPicker.ItemClickEvent, (Delegate) value);
    remove => this.RemoveHandler(ColorPicker.ItemClickEvent, (Delegate) value);
  }

  protected virtual void OnItemClick(ColorValue item)
  {
  }

  private class WindowWrapper : System.Windows.Forms.IWin32Window
  {
    public IntPtr Handle { get; set; }

    public static ColorPicker.WindowWrapper Create(Visual visual)
    {
      if (visual == null)
        return (ColorPicker.WindowWrapper) null;
      DependencyObject reference = (visual is Popup popup1 ? (DependencyObject) popup1.PlacementTarget : (DependencyObject) null) ?? (visual is FrameworkElement frameworkElement1 ? frameworkElement1.Parent : (DependencyObject) null) ?? VisualTreeHelper.GetParent((DependencyObject) visual);
      while (true)
      {
        DependencyObject dependencyObject;
        switch (reference)
        {
          case null:
            goto label_11;
          case Window window:
            goto label_3;
          case Popup popup2:
            dependencyObject = (DependencyObject) popup2.PlacementTarget;
            break;
          default:
            dependencyObject = (DependencyObject) null;
            break;
        }
        if (dependencyObject == null)
          dependencyObject = (reference is FrameworkElement frameworkElement2 ? frameworkElement2.Parent : (DependencyObject) null) ?? VisualTreeHelper.GetParent(reference);
        reference = dependencyObject;
      }
label_3:
      WindowInteropHelper windowInteropHelper = new WindowInteropHelper(window);
      if (windowInteropHelper.Handle != IntPtr.Zero)
        return new ColorPicker.WindowWrapper()
        {
          Handle = windowInteropHelper.Handle
        };
label_11:
      return (ColorPicker.WindowWrapper) null;
    }
  }
}
