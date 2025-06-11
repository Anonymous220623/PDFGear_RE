// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.ColorPicker
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Tools.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Controls;

[TemplatePart(Name = "PART_BorderColor", Type = typeof (Border))]
[TemplatePart(Name = "PART_BorderPicker", Type = typeof (Border))]
[TemplatePart(Name = "PART_BorderDrag", Type = typeof (Border))]
[TemplatePart(Name = "PART_PanelColor", Type = typeof (Panel))]
[TemplatePart(Name = "PART_SliderColor", Type = typeof (Panel))]
[TemplatePart(Name = "PART_SliderOpacity", Type = typeof (Panel))]
[TemplatePart(Name = "PART_PanelRgb", Type = typeof (Panel))]
[TemplatePart(Name = "PART_ButtonDropper", Type = typeof (ToggleButton))]
public class ColorPicker : Control, ISingleOpen, IDisposable
{
  private const string ElementBorderColor = "PART_BorderColor";
  private const string ElementBorderPicker = "PART_BorderPicker";
  private const string ElementBorderDrag = "PART_BorderDrag";
  private const string ElementPanelColor = "PART_PanelColor";
  private const string ElementSliderColor = "PART_SliderColor";
  private const string ElementSliderOpacity = "PART_SliderOpacity";
  private const string ElementPanelRgb = "PART_PanelRgb";
  private const string ElementButtonDropper = "PART_ButtonDropper";
  private ColorDropper _colorDropper;
  private ToggleButton _toggleButtonDropper;
  private Panel _panelRgb;
  private Border _borderColor;
  private Border _borderPicker;
  private Border _borderDrag;
  private Panel _panelColor;
  private Slider _sliderColor;
  private Slider _sliderOpacity;
  private bool _appliedTemplate;
  private bool _disposed;
  private int _colorType;
  private bool _isLoaded;
  private bool _isNeedUpdatePicker = true;
  private bool _isOnDragging;
  private const double ColorPanelWidth = 230.0;
  private const double ColorPanelHeight = 122.0;
  private readonly List<string> _colorPresetList = new List<string>()
  {
    "#f44336",
    "#e91e63",
    "#9c27b0",
    "#673ab7",
    "#3f51b5",
    "#2196f3",
    "#03a9f4",
    "#00bcd4",
    "#009688",
    "#4caf50",
    "#8bc34a",
    "#cddc39",
    "#ffeb3b",
    "#ffc107",
    "#ff9800",
    "#ff5722",
    "#795548",
    "#9e9e9e"
  };
  private readonly List<ColorRange> _colorRangeList = new List<ColorRange>()
  {
    new ColorRange()
    {
      Start = Color.FromRgb(byte.MaxValue, (byte) 0, (byte) 0),
      End = Color.FromRgb(byte.MaxValue, (byte) 0, byte.MaxValue)
    },
    new ColorRange()
    {
      Start = Color.FromRgb(byte.MaxValue, (byte) 0, byte.MaxValue),
      End = Color.FromRgb((byte) 0, (byte) 0, byte.MaxValue)
    },
    new ColorRange()
    {
      Start = Color.FromRgb((byte) 0, (byte) 0, byte.MaxValue),
      End = Color.FromRgb((byte) 0, byte.MaxValue, byte.MaxValue)
    },
    new ColorRange()
    {
      Start = Color.FromRgb((byte) 0, byte.MaxValue, byte.MaxValue),
      End = Color.FromRgb((byte) 0, byte.MaxValue, (byte) 0)
    },
    new ColorRange()
    {
      Start = Color.FromRgb((byte) 0, byte.MaxValue, (byte) 0),
      End = Color.FromRgb(byte.MaxValue, byte.MaxValue, (byte) 0)
    },
    new ColorRange()
    {
      Start = Color.FromRgb(byte.MaxValue, byte.MaxValue, (byte) 0),
      End = Color.FromRgb(byte.MaxValue, (byte) 0, (byte) 0)
    }
  };
  private readonly List<Color> _colorSeparateList = new List<Color>()
  {
    Color.FromRgb(byte.MaxValue, (byte) 0, (byte) 0),
    Color.FromRgb(byte.MaxValue, (byte) 0, byte.MaxValue),
    Color.FromRgb((byte) 0, (byte) 0, byte.MaxValue),
    Color.FromRgb((byte) 0, byte.MaxValue, byte.MaxValue),
    Color.FromRgb((byte) 0, byte.MaxValue, (byte) 0),
    Color.FromRgb(byte.MaxValue, byte.MaxValue, (byte) 0)
  };
  public static readonly RoutedEvent SelectedColorChangedEvent = EventManager.RegisterRoutedEvent("SelectedColorChanged", RoutingStrategy.Bubble, typeof (EventHandler<FunctionEventArgs<Color>>), typeof (ColorPicker));
  public static readonly RoutedEvent ConfirmedEvent = EventManager.RegisterRoutedEvent("Confirmed", RoutingStrategy.Bubble, typeof (EventHandler<FunctionEventArgs<Color>>), typeof (ColorPicker));
  public static readonly RoutedEvent CanceledEvent = EventManager.RegisterRoutedEvent("Canceled", RoutingStrategy.Bubble, typeof (EventHandler), typeof (ColorPicker));
  internal static readonly DependencyProperty ChannelAProperty = DependencyProperty.Register(nameof (ChannelA), typeof (int), typeof (ColorPicker), new PropertyMetadata((object) (int) byte.MaxValue));
  internal static readonly DependencyProperty ChannelRProperty = DependencyProperty.Register(nameof (ChannelR), typeof (int), typeof (ColorPicker), new PropertyMetadata((object) (int) byte.MaxValue));
  internal static readonly DependencyProperty ChannelGProperty = DependencyProperty.Register(nameof (ChannelG), typeof (int), typeof (ColorPicker), new PropertyMetadata((object) (int) byte.MaxValue));
  internal static readonly DependencyProperty ChannelBProperty = DependencyProperty.Register(nameof (ChannelB), typeof (int), typeof (ColorPicker), new PropertyMetadata((object) (int) byte.MaxValue));
  public static readonly DependencyProperty SelectedBrushProperty = DependencyProperty.Register(nameof (SelectedBrush), typeof (SolidColorBrush), typeof (ColorPicker), new PropertyMetadata((object) Brushes.White, new PropertyChangedCallback(ColorPicker.OnSelectedBrushChanged), new CoerceValueCallback(ColorPicker.CoerceSelectedBrush)));
  internal static readonly DependencyProperty SelectedBrushWithoutOpacityProperty = DependencyProperty.Register(nameof (SelectedBrushWithoutOpacity), typeof (SolidColorBrush), typeof (ColorPicker), new PropertyMetadata((object) Brushes.White));
  internal static readonly DependencyProperty BackColorProperty = DependencyProperty.Register(nameof (BackColor), typeof (SolidColorBrush), typeof (ColorPicker), new PropertyMetadata((object) Brushes.Red));
  internal static readonly DependencyProperty ShowListProperty = DependencyProperty.Register(nameof (ShowList), typeof (List<bool>), typeof (ColorPicker), new PropertyMetadata((object) new List<bool>()
  {
    true,
    false,
    false
  }));

  private bool IsNeedUpdateInfo { get; set; } = true;

  public event EventHandler<FunctionEventArgs<Color>> SelectedColorChanged
  {
    add => this.AddHandler(ColorPicker.SelectedColorChangedEvent, (Delegate) value);
    remove => this.RemoveHandler(ColorPicker.SelectedColorChangedEvent, (Delegate) value);
  }

  public event EventHandler<FunctionEventArgs<Color>> Confirmed
  {
    add => this.AddHandler(ColorPicker.ConfirmedEvent, (Delegate) value);
    remove => this.RemoveHandler(ColorPicker.ConfirmedEvent, (Delegate) value);
  }

  public event EventHandler Canceled
  {
    add => this.AddHandler(ColorPicker.CanceledEvent, (Delegate) value);
    remove => this.RemoveHandler(ColorPicker.CanceledEvent, (Delegate) value);
  }

  internal int ChannelA
  {
    get => (int) this.GetValue(ColorPicker.ChannelAProperty);
    set => this.SetValue(ColorPicker.ChannelAProperty, (object) value);
  }

  internal int ChannelR
  {
    get => (int) this.GetValue(ColorPicker.ChannelRProperty);
    set => this.SetValue(ColorPicker.ChannelRProperty, (object) value);
  }

  internal int ChannelG
  {
    get => (int) this.GetValue(ColorPicker.ChannelGProperty);
    set => this.SetValue(ColorPicker.ChannelGProperty, (object) value);
  }

  internal int ChannelB
  {
    get => (int) this.GetValue(ColorPicker.ChannelBProperty);
    set => this.SetValue(ColorPicker.ChannelBProperty, (object) value);
  }

  private static object CoerceSelectedBrush(DependencyObject d, object basevalue)
  {
    return !(basevalue is SolidColorBrush) ? (object) Brushes.White : basevalue;
  }

  private static void OnSelectedBrushChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ColorPicker source = (ColorPicker) d;
    SolidColorBrush newValue = (SolidColorBrush) e.NewValue;
    if (source.IsNeedUpdateInfo)
    {
      source.IsNeedUpdateInfo = false;
      source.ChannelR = (int) newValue.Color.R;
      source.ChannelG = (int) newValue.Color.G;
      source.ChannelB = (int) newValue.Color.B;
      source.ChannelA = (int) newValue.Color.A;
      source.IsNeedUpdateInfo = true;
    }
    source.UpdateStatus(newValue.Color);
    ColorPicker colorPicker = source;
    int r = (int) newValue.Color.R;
    Color color = newValue.Color;
    int g = (int) color.G;
    color = newValue.Color;
    int b = (int) color.B;
    SolidColorBrush solidColorBrush = new SolidColorBrush(Color.FromRgb((byte) r, (byte) g, (byte) b));
    colorPicker.SelectedBrushWithoutOpacity = solidColorBrush;
    source.RaiseEvent((RoutedEventArgs) new FunctionEventArgs<Color>(ColorPicker.SelectedColorChangedEvent, (object) source)
    {
      Info = newValue.Color
    });
  }

  public SolidColorBrush SelectedBrush
  {
    get => (SolidColorBrush) this.GetValue(ColorPicker.SelectedBrushProperty);
    set => this.SetValue(ColorPicker.SelectedBrushProperty, (object) value);
  }

  internal SolidColorBrush SelectedBrushWithoutOpacity
  {
    get => (SolidColorBrush) this.GetValue(ColorPicker.SelectedBrushWithoutOpacityProperty);
    set => this.SetValue(ColorPicker.SelectedBrushWithoutOpacityProperty, (object) value);
  }

  internal SolidColorBrush BackColor
  {
    get => (SolidColorBrush) this.GetValue(ColorPicker.BackColorProperty);
    set => this.SetValue(ColorPicker.BackColorProperty, (object) value);
  }

  internal List<bool> ShowList
  {
    get => (List<bool>) this.GetValue(ColorPicker.ShowListProperty);
    set => this.SetValue(ColorPicker.ShowListProperty, (object) value);
  }

  private int ColorType
  {
    get => this._colorType;
    set
    {
      this._colorType = value >= 0 ? (value <= 1 ? value : 0) : 1;
      List<bool> boolList = new List<bool>();
      for (int index = 0; index < 2; ++index)
        boolList.Add(false);
      boolList[this._colorType] = true;
      this.ShowList = boolList;
    }
  }

  ~ColorPicker() => this.Dispose(false);

  public ColorPicker()
  {
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Confirm, new ExecutedRoutedEventHandler(this.ButtonConfirm_OnClick)));
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Switch, new ExecutedRoutedEventHandler(this.ButtonSwitch_OnClick)));
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Cancel, new ExecutedRoutedEventHandler(this.ButtonCancel_OnClick)));
    this.Loaded += (RoutedEventHandler) ((s, e) =>
    {
      if (this._isLoaded)
        return;
      this.Init();
      this._isLoaded = true;
    });
  }

  public override void OnApplyTemplate()
  {
    this._appliedTemplate = false;
    if (this._sliderColor != null)
      this._sliderColor.ValueChanged -= new RoutedPropertyChangedEventHandler<double>(this.SliderColor_OnValueChanged);
    if (this._sliderOpacity != null)
      this._sliderOpacity.ValueChanged -= new RoutedPropertyChangedEventHandler<double>(this.SliderOpacity_OnValueChanged);
    if (this._toggleButtonDropper != null)
      this._toggleButtonDropper.Click -= new RoutedEventHandler(this.ToggleButtonDropper_Click);
    this._panelColor?.Children.Clear();
    this._panelRgb?.RemoveHandler(NumericUpDown.ValueChangedEvent, (Delegate) new EventHandler<FunctionEventArgs<double>>(this.NumericUpDownRgb_OnValueChanged));
    base.OnApplyTemplate();
    this._borderColor = this.GetTemplateChild("PART_BorderColor") as Border;
    this._borderDrag = this.GetTemplateChild("PART_BorderDrag") as Border;
    this._borderPicker = this.GetTemplateChild("PART_BorderPicker") as Border;
    this._panelColor = this.GetTemplateChild("PART_PanelColor") as Panel;
    this._sliderColor = this.GetTemplateChild("PART_SliderColor") as Slider;
    this._sliderOpacity = this.GetTemplateChild("PART_SliderOpacity") as Slider;
    this._panelRgb = this.GetTemplateChild("PART_PanelRgb") as Panel;
    this._toggleButtonDropper = this.GetTemplateChild("PART_ButtonDropper") as ToggleButton;
    if (this._sliderColor != null)
      this._sliderColor.ValueChanged += new RoutedPropertyChangedEventHandler<double>(this.SliderColor_OnValueChanged);
    if (this._sliderOpacity != null)
      this._sliderOpacity.ValueChanged += new RoutedPropertyChangedEventHandler<double>(this.SliderOpacity_OnValueChanged);
    if (this._borderDrag != null)
    {
      MouseDragElementBehavior dragElementBehavior = new MouseDragElementBehavior();
      dragElementBehavior.DragFinished += new MouseEventHandler(this.MouseDragElementBehavior_OnDragFinished);
      dragElementBehavior.DragBegun += new MouseEventHandler(this.MouseDragElementBehavior_OnDragging);
      dragElementBehavior.Dragging += new MouseEventHandler(this.MouseDragElementBehavior_OnDragging);
      Interaction.GetBehaviors((DependencyObject) this._borderDrag).Add((Behavior) dragElementBehavior);
    }
    if (this._toggleButtonDropper != null)
      this._toggleButtonDropper.Click += new RoutedEventHandler(this.ToggleButtonDropper_Click);
    this._appliedTemplate = true;
    if (this._isLoaded)
      this.Init();
    this._panelRgb?.AddHandler(NumericUpDown.ValueChangedEvent, (Delegate) new EventHandler<FunctionEventArgs<double>>(this.NumericUpDownRgb_OnValueChanged));
  }

  private void Init()
  {
    if (this._panelColor == null)
      return;
    this.UpdateStatus(this.SelectedBrush.Color);
    this._panelColor.Children.Clear();
    foreach (string colorPreset in this._colorPresetList)
      this._panelColor.Children.Add((UIElement) this.CreateColorButton(colorPreset));
  }

  private Button CreateColorButton(string colorStr)
  {
    SolidColorBrush brush = new SolidColorBrush((Color) (ColorConverter.ConvertFromString(colorStr) ?? (object) new Color()));
    Button colorButton = new Button();
    colorButton.Margin = new Thickness(6.0);
    colorButton.Style = HandyControl.Tools.ResourceHelper.GetResourceInternal<Style>("ButtonCustom");
    Border border = new Border();
    border.Background = (Brush) brush;
    border.Width = 12.0;
    border.Height = 12.0;
    border.CornerRadius = new CornerRadius(2.0);
    colorButton.Content = (object) border;
    colorButton.Click += (RoutedEventHandler) ((s, e) =>
    {
      this.SelectedBrush = brush;
      this.ChannelA = (int) byte.MaxValue;
    });
    return colorButton;
  }

  private void UpdateStatus(Color color)
  {
    if (this._isOnDragging || this._sliderColor == null)
      return;
    byte r = color.R;
    byte g = color.G;
    byte b = color.B;
    List<byte> source = new List<byte>() { r, g, b };
    byte num1 = source.Max<byte>();
    byte num2 = source.Min<byte>();
    if ((int) num2 == (int) num1)
    {
      if ((int) r != (int) g || (int) b != (int) g)
      {
        this.BackColor = Brushes.Red;
        this.IsNeedUpdateInfo = false;
        if (!this._sliderColor.IsMouseOver && !this._sliderOpacity.IsMouseOver)
          this._sliderColor.Value = 0.0;
        this.IsNeedUpdateInfo = true;
      }
    }
    else
    {
      int index1 = source.IndexOf(num1);
      int index2 = source.IndexOf(num2);
      int index3 = 3 - index1 - index2;
      if (index3 == 3)
      {
        this.BackColor = Brushes.Red;
        this.IsNeedUpdateInfo = false;
        if (!this._sliderColor.IsMouseOver && !this._sliderOpacity.IsMouseOver)
          this._sliderColor.Value = 0.0;
        this.IsNeedUpdateInfo = true;
      }
      else
      {
        byte num3 = source[index3];
        source[index1] = byte.MaxValue;
        source[index2] = (byte) 0;
        byte num4 = (byte) ((double) ((int) byte.MaxValue * ((int) num2 - (int) num3)) / (double) ((int) num2 - (int) num1));
        source[index3] = num4;
        this.BackColor = new SolidColorBrush(Color.FromRgb(source[0], source[1], source[2]));
        source[index3] = (byte) 0;
        int num5 = this._colorSeparateList.IndexOf(Color.FromRgb(source[0], source[1], source[2]));
        int num6 = 0;
        int num7;
        if (num5 < 5 && num5 > 0)
        {
          List<byte> list1 = this._colorSeparateList[num5 + 1].ToList();
          List<byte> list2 = this._colorSeparateList[num5 - 1].ToList();
          int index4 = index2;
          if (list1[index4] > (byte) 0)
          {
            int num8 = (int) list2[index3];
            num6 = 1;
            int num9 = (int) num4;
            num7 = num8 - num9;
          }
          else
            num7 = (int) num4;
        }
        else if (num5 == 0)
        {
          num7 = (int) num4;
          if (index2 == 2)
          {
            num7 = (int) byte.MaxValue - (int) num4;
            num6 = -5;
          }
        }
        else
          num7 = (int) byte.MaxValue - (int) num4;
        double num10 = (double) num7 / (double) byte.MaxValue;
        double num11 = (double) (num5 - num6) + num10;
        this.IsNeedUpdateInfo = false;
        if (!this._sliderColor.IsMouseOver && !this._sliderOpacity.IsMouseOver)
          this._sliderColor.Value = num11;
        this.IsNeedUpdateInfo = true;
      }
    }
    Matrix matrix = this._borderPicker.RenderTransform.Value;
    double offsetX = num1 == (byte) 0 ? 0.0 : (1.0 - (double) num2 / (double) num1) * 230.0;
    double offsetY = (1.0 - (double) num1 / (double) byte.MaxValue) * 122.0;
    if (!this._isNeedUpdatePicker)
      return;
    this._borderPicker.RenderTransform = (Transform) new MatrixTransform(matrix.M11, matrix.M12, matrix.M21, matrix.M22, offsetX, offsetY);
  }

  private void SliderColor_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
  {
    if (!this._appliedTemplate || !this.IsNeedUpdateInfo)
      return;
    int index = Math.Min(5, (int) Math.Floor(e.NewValue));
    double range = e.NewValue - (double) index;
    this.BackColor = new SolidColorBrush(this._colorRangeList[index].GetColor(range));
    Matrix matrix = this._borderPicker.RenderTransform.Value;
    this._isNeedUpdatePicker = false;
    this.UpdateColorWhenDrag(new Point(matrix.OffsetX, matrix.OffsetY));
    this._isNeedUpdatePicker = true;
  }

  private void SliderOpacity_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
  {
    if (!this._appliedTemplate || !this.IsNeedUpdateInfo)
      return;
    Color color = this.SelectedBrush.Color;
    this.SelectedBrush = new SolidColorBrush(Color.FromArgb((byte) this._sliderOpacity.Value, color.R, color.G, color.B));
  }

  private void MouseDragElementBehavior_OnDragging(object sender, MouseEventArgs e)
  {
    Point position = e.GetPosition((IInputElement) this._borderColor);
    this._isOnDragging = true;
    this.UpdateColorWhenDrag(position);
    this._isOnDragging = false;
  }

  private void UpdateColorWhenDrag(Point p)
  {
    Matrix matrix = this._borderPicker.RenderTransform.Value;
    if (p.X < 0.0)
      p.X = 0.0;
    else if (p.X > 230.0)
      p.X = 230.0;
    if (p.Y < 0.0)
      p.Y = 0.0;
    else if (p.Y > 122.0)
      p.Y = 122.0;
    if (this._isNeedUpdatePicker)
      this._borderPicker.RenderTransform = (Transform) new MatrixTransform(matrix.M11, matrix.M12, matrix.M21, matrix.M22, p.X, p.Y);
    double num1 = p.X / 230.0;
    double num2 = 1.0 - p.Y / 122.0;
    Color color1 = Color.FromRgb((byte) ((double) byte.MaxValue * num2), (byte) ((double) byte.MaxValue * num2), (byte) ((double) byte.MaxValue * num2));
    int r = (int) (byte) ((double) this.BackColor.Color.R * num2);
    Color color2 = this.BackColor.Color;
    int g = (int) (byte) ((double) color2.G * num2);
    color2 = this.BackColor.Color;
    int b = (int) (byte) ((double) color2.B * num2);
    Color color3 = Color.FromRgb((byte) r, (byte) g, (byte) b);
    int num3 = (int) color1.R - (int) color3.R;
    int num4 = (int) color1.G - (int) color3.G;
    int num5 = (int) color1.B - (int) color3.B;
    this.SelectedBrush = new SolidColorBrush(Color.FromArgb((byte) this._sliderOpacity.Value, (byte) ((double) color1.R - (double) num3 * num1), (byte) ((double) color1.G - (double) num4 * num1), (byte) ((double) color1.B - (double) num5 * num1)));
  }

  private void MouseDragElementBehavior_OnDragFinished(object sender, MouseEventArgs e)
  {
    this._borderDrag.RenderTransform = (Transform) new MatrixTransform();
  }

  private void ButtonSwitch_OnClick(object sender, RoutedEventArgs e) => ++this.ColorType;

  private void NumericUpDownRgb_OnValueChanged(object sender, FunctionEventArgs<double> e)
  {
    if (!this._appliedTemplate || !this.IsNeedUpdateInfo || !(e.OriginalSource is NumericUpDown originalSource) || !(originalSource.Tag is string tag))
      return;
    Color color = this.SelectedBrush.Color;
    this.IsNeedUpdateInfo = false;
    SolidColorBrush solidColorBrush;
    switch (tag)
    {
      case "R":
        solidColorBrush = new SolidColorBrush(Color.FromArgb(color.A, (byte) e.Info, color.G, color.B));
        break;
      case "G":
        solidColorBrush = new SolidColorBrush(Color.FromArgb(color.A, color.R, (byte) e.Info, color.B));
        break;
      case "B":
        solidColorBrush = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, (byte) e.Info));
        break;
      default:
        solidColorBrush = this.SelectedBrush;
        break;
    }
    this.SelectedBrush = solidColorBrush;
    this.IsNeedUpdateInfo = true;
  }

  private void ButtonConfirm_OnClick(object sender, RoutedEventArgs e)
  {
    this.RaiseEvent((RoutedEventArgs) new FunctionEventArgs<Color>(ColorPicker.ConfirmedEvent, (object) this)
    {
      Info = this.SelectedBrush.Color
    });
  }

  private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
  {
    this.RaiseEvent(new RoutedEventArgs(ColorPicker.CanceledEvent));
  }

  private void ToggleButtonDropper_Click(object sender, RoutedEventArgs e)
  {
    if (this._colorDropper == null)
      this._colorDropper = new ColorDropper(this);
    this._colorDropper.Update(this._toggleButtonDropper.IsChecked.Value);
  }

  protected virtual void Dispose(bool disposing)
  {
    if (this._disposed)
      return;
    this.Dispatcher.BeginInvoke((Delegate) (() =>
    {
      this._colorDropper?.Update(false);
      System.Windows.Window.GetWindow((DependencyObject) this)?.Close();
    }));
    this._disposed = true;
  }

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }

  public bool CanDispose => true;
}
