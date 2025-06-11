// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.TimeSpanEdit
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Licensing;
using Syncfusion.Windows.Shared.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

[SkinType(SkinVisualStyle = Skin.Blend, Type = typeof (TimeSpanEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/TimeSpanEdit/Themes/BlendStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Black, Type = typeof (TimeSpanEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/TimeSpanEdit/Themes/Office2010BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Silver, Type = typeof (TimeSpanEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/TimeSpanEdit/Themes/Office2010SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Transparent, Type = typeof (TimeSpanEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/TimeSpanEdit/Themes/TransparentStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.VS2010, Type = typeof (TimeSpanEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/TimeSpanEdit/Themes/VS2010Style.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Blue, Type = typeof (TimeSpanEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/TimeSpanEdit/Themes/Office2010BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Blue, Type = typeof (TimeSpanEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/TimeSpanEdit/Themes/Office2007BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Default, Type = typeof (TimeSpanEdit), XamlResource = "/Syncfusion.Shared.Wpf;component/Controls/TimeSpanEdit/Themes/generic.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Black, Type = typeof (TimeSpanEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/TimeSpanEdit/Themes/Office2007BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Silver, Type = typeof (TimeSpanEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/TimeSpanEdit/Themes/Office2007SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Metro, Type = typeof (TimeSpanEdit), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/TimeSpanEdit/Themes/MetroStyle.xaml")]
public class TimeSpanEdit : TextBox, IDisposable
{
  private AdornerLayer aLayer;
  private TextBoxSelectionAdorner txtSelectionAdorner1;
  private ExtendedScrollingAdorner vAdorner;
  private Dictionary<int, char> tPosition = new Dictionary<int, char>();
  private Dictionary<int, int> tLength = new Dictionary<int, int>();
  private Dictionary<int, int> tStart = new Dictionary<int, int>();
  private bool isSelectionChanged;
  private bool selectionStartChanged;
  private int selectionStart;
  private ICommand upCommand;
  private ICommand downCommand;
  private RepeatButton upButton;
  private RepeatButton downButton;
  private bool isDaysVisibile;
  private bool isHoursVisible;
  private bool isMinutesVisible;
  private bool isSecondsVisible;
  private bool isAppendDigit;
  private bool isSpinButtonPressed;
  private string keyCatch = string.Empty;
  private bool isMinMaxValidate;
  private string tempTimeSpan;
  public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof (Value), typeof (TimeSpan?), typeof (TimeSpanEdit), new PropertyMetadata((object) new TimeSpan(0, 0, 0, 0, 0), new PropertyChangedCallback(TimeSpanEdit.OnValueChanged)));
  public static readonly DependencyProperty StepIntervalProperty = DependencyProperty.Register(nameof (StepInterval), typeof (TimeSpan), typeof (TimeSpanEdit), new PropertyMetadata((object) new TimeSpan(1, 1, 1, 1)));
  public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(nameof (MinValue), typeof (TimeSpan), typeof (TimeSpanEdit), new PropertyMetadata((object) TimeSpan.MinValue, new PropertyChangedCallback(TimeSpanEdit.OnMinValueChanged)));
  public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(nameof (MaxValue), typeof (TimeSpan), typeof (TimeSpanEdit), new PropertyMetadata((object) TimeSpan.MaxValue, new PropertyChangedCallback(TimeSpanEdit.OnMaxValueChanged)));
  public static readonly DependencyProperty FormatProperty = DependencyProperty.Register(nameof (Format), typeof (string), typeof (TimeSpanEdit), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(TimeSpanEdit.OnFormatStringChanged)));
  public static readonly DependencyProperty NullStringProperty = DependencyProperty.Register(nameof (NullString), typeof (string), typeof (TimeSpanEdit), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(TimeSpanEdit.OnNullStringChanged)));
  public static readonly DependencyProperty ShowArrowButtonsProperty = DependencyProperty.Register(nameof (ShowArrowButtons), typeof (bool), typeof (TimeSpanEdit), new PropertyMetadata((object) true));
  public static readonly DependencyProperty IncrementOnScrollingProperty = DependencyProperty.Register(nameof (IncrementOnScrolling), typeof (bool), typeof (TimeSpanEdit), new PropertyMetadata((object) true));
  public static readonly DependencyProperty AllowNullProperty = DependencyProperty.Register(nameof (AllowNull), typeof (bool), typeof (TimeSpanEdit), new PropertyMetadata((object) true, new PropertyChangedCallback(TimeSpanEdit.OnAllowNullChanged)));
  public static readonly DependencyProperty EnableTouchProperty = DependencyProperty.Register(nameof (EnableTouch), typeof (bool), typeof (TimeSpanEdit), new PropertyMetadata((object) false, new PropertyChangedCallback(TimeSpanEdit.OnEnableTouchChanged)));
  public static readonly DependencyProperty EnableExtendedScrollingProperty = DependencyProperty.Register(nameof (EnableExtendedScrolling), typeof (bool), typeof (TimeSpanEdit), new PropertyMetadata((object) false, new PropertyChangedCallback(TimeSpanEdit.OnEnableExtendedScrollingChanged)));

  public TimeSpanEdit()
  {
    this.DefaultStyleKey = (object) typeof (TimeSpanEdit);
    this.Loaded += new RoutedEventHandler(this.TimeSpanEdit_Loaded);
    this.CommandBindings.Add(new CommandBinding((ICommand) EditorCommands.Clear, new ExecutedRoutedEventHandler(this.ExecuteClearCommand), new CanExecuteRoutedEventHandler(this.CanExecuteClearCommand)));
    FusionLicenseProvider.GetLicenseType(Platform.WPF);
  }

  private void TimeSpanEdit_Loaded(object sender, RoutedEventArgs e)
  {
    this.keyCatch = string.Empty;
    if (this.Format == string.Empty)
      this.Format = "d.h:m:s";
    if (this.EnableExtendedScrolling)
    {
      if (this.aLayer == null)
        this.aLayer = AdornerLayer.GetAdornerLayer((Visual) this);
      if (this.aLayer != null)
      {
        this.vAdorner = new ExtendedScrollingAdorner((UIElement) this);
        this.aLayer.Add((Adorner) this.vAdorner);
      }
    }
    if (!this.EnableTouch)
      return;
    if (this.aLayer == null)
      this.aLayer = AdornerLayer.GetAdornerLayer((Visual) this);
    if (this.aLayer == null)
      return;
    this.txtSelectionAdorner1 = new TextBoxSelectionAdorner((UIElement) this);
    this.aLayer.Add((Adorner) this.txtSelectionAdorner1);
  }

  private void ExecuteClearCommand(object sender, ExecutedRoutedEventArgs e) => this.Clear();

  private void CanExecuteClearCommand(object sender, CanExecuteRoutedEventArgs e)
  {
    e.CanExecute = this.Text.Length > 0;
  }

  public void Dispose()
  {
    this.Loaded -= new RoutedEventHandler(this.TimeSpanEdit_Loaded);
    if (this.upCommand != null)
      this.upCommand = (ICommand) null;
    if (this.downCommand != null)
      this.downCommand = (ICommand) null;
    if (this.upButton != null)
      this.upButton = (RepeatButton) null;
    if (this.downButton != null)
      this.downButton = (RepeatButton) null;
    if (this.keyCatch != null)
      this.keyCatch = (string) null;
    if (this.tPosition != null)
    {
      this.tPosition.Clear();
      this.tPosition = (Dictionary<int, char>) null;
    }
    if (this.tLength != null)
    {
      this.tLength.Clear();
      this.tLength = (Dictionary<int, int>) null;
    }
    if (this.tStart != null)
    {
      this.tStart.Clear();
      this.tStart = (Dictionary<int, int>) null;
    }
    this.CommandBindings.Clear();
  }

  private TimeSpanEdit.TimeSpanElements SelectedSpan { get; set; }

  public TimeSpan? Value
  {
    get => (TimeSpan?) this.GetValue(TimeSpanEdit.ValueProperty);
    set => this.SetValue(TimeSpanEdit.ValueProperty, (object) value);
  }

  public TimeSpan MaxValue
  {
    get => (TimeSpan) this.GetValue(TimeSpanEdit.MaxValueProperty);
    set => this.SetValue(TimeSpanEdit.MaxValueProperty, (object) value);
  }

  public TimeSpan MinValue
  {
    get => (TimeSpan) this.GetValue(TimeSpanEdit.MinValueProperty);
    set => this.SetValue(TimeSpanEdit.MinValueProperty, (object) value);
  }

  public TimeSpan StepInterval
  {
    get => (TimeSpan) this.GetValue(TimeSpanEdit.StepIntervalProperty);
    set => this.SetValue(TimeSpanEdit.StepIntervalProperty, (object) value);
  }

  public string Format
  {
    get => (string) this.GetValue(TimeSpanEdit.FormatProperty);
    set => this.SetValue(TimeSpanEdit.FormatProperty, (object) value);
  }

  public string NullString
  {
    get => (string) this.GetValue(TimeSpanEdit.NullStringProperty);
    set => this.SetValue(TimeSpanEdit.NullStringProperty, (object) value);
  }

  public bool ShowArrowButtons
  {
    get => (bool) this.GetValue(TimeSpanEdit.ShowArrowButtonsProperty);
    set => this.SetValue(TimeSpanEdit.ShowArrowButtonsProperty, (object) value);
  }

  public bool IncrementOnScrolling
  {
    get => (bool) this.GetValue(TimeSpanEdit.IncrementOnScrollingProperty);
    set => this.SetValue(TimeSpanEdit.IncrementOnScrollingProperty, (object) value);
  }

  public bool AllowNull
  {
    get => (bool) this.GetValue(TimeSpanEdit.AllowNullProperty);
    set => this.SetValue(TimeSpanEdit.AllowNullProperty, (object) value);
  }

  public event PropertyChangedCallback ValueChanged;

  protected override AutomationPeer OnCreateAutomationPeer()
  {
    return (AutomationPeer) new TimeSpanEditAutomationPeer(this);
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.upButton = (RepeatButton) this.GetTemplateChild("upbutton");
    this.downButton = (RepeatButton) this.GetTemplateChild("downbutton");
    if (this.upCommand != null)
      AutomationProperties.SetName((DependencyObject) this.upButton, "UpButton");
    if (this.downCommand == null)
      return;
    AutomationProperties.SetName((DependencyObject) this.downButton, "DownButton");
  }

  protected override void OnContextMenuOpening(ContextMenuEventArgs e)
  {
    e.Handled = true;
    base.OnContextMenuOpening(e);
  }

  protected override void OnMouseWheel(MouseWheelEventArgs e)
  {
    if (this.IncrementOnScrolling)
    {
      if (e.Delta > 0)
        this.IncreaseorDecreaseSpanValue(true);
      if (e.Delta < 0)
        this.IncreaseorDecreaseSpanValue(false);
      if (this.tPosition.ContainsKey(this.SelectionStart))
      {
        this.Select(this.tStart[this.SelectionStart], this.tLength[this.SelectionStart]);
      }
      else
      {
        this.isSelectionChanged = false;
        this.HandleLeft();
      }
    }
    base.OnMouseWheel(e);
  }

  protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    this.isSelectionChanged = false;
    this.keyCatch = string.Empty;
    this.OnMouseLeftButtonDown(e);
  }

  protected override void OnPreviewKeyDown(KeyEventArgs e)
  {
    if (this.IsReadOnly)
      return;
    bool flag = false;
    this.isSelectionChanged = false;
    switch (e.Key)
    {
      case Key.Tab:
        e.Handled = false;
        goto case Key.Return;
      case Key.Return:
        if (!flag)
          break;
        this.OnKeyDown(e);
        break;
      case Key.End:
      case Key.A:
      case Key.LeftShift:
      case Key.RightShift:
        if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
        {
          this.SelectAll();
          goto case Key.Return;
        }
        e.Handled = true;
        this.keyCatch = string.Empty;
        goto case Key.Return;
      case Key.Left:
        if (this.Value.HasValue && this.Text != this.NullString)
        {
          this.HandleLeft();
          e.Handled = true;
          goto case Key.Return;
        }
        flag = true;
        goto case Key.Return;
      case Key.Up:
        if (this.Value.HasValue)
        {
          this.IncreaseorDecreaseSpanValue(true);
          this.keyCatch = string.Empty;
          e.Handled = true;
          goto case Key.Return;
        }
        goto case Key.Return;
      case Key.Right:
        if (this.Value.HasValue && this.Text != this.NullString)
        {
          e.Handled = this.HandleRight();
          goto case Key.Return;
        }
        flag = true;
        goto case Key.Return;
      case Key.Down:
        if (this.Value.HasValue)
        {
          this.IncreaseorDecreaseSpanValue(false);
          this.keyCatch = string.Empty;
          e.Handled = true;
          goto case Key.Return;
        }
        goto case Key.Return;
      case Key.Delete:
        if (this.Value.HasValue)
        {
          this.keyCatch = string.Empty;
          if (this.SelectionLength == this.Text.Length)
          {
            if (this.AllowNull)
            {
              this.Text = this.NullString;
              this.Value = new TimeSpan?();
            }
            else
              this.Value = new TimeSpan?(new TimeSpan(0, 0, 0, 0, 0));
          }
          else
            this.AppendDigit(0);
          e.Handled = true;
          goto case Key.Return;
        }
        goto case Key.Return;
      case Key.D0:
        this.AppendDigit(0);
        e.Handled = true;
        goto case Key.Return;
      case Key.D1:
        this.AppendDigit(1);
        e.Handled = true;
        goto case Key.Return;
      case Key.D2:
        this.AppendDigit(2);
        e.Handled = true;
        goto case Key.Return;
      case Key.D3:
        this.AppendDigit(3);
        e.Handled = true;
        goto case Key.Return;
      case Key.D4:
        this.AppendDigit(4);
        e.Handled = true;
        goto case Key.Return;
      case Key.D5:
        this.AppendDigit(5);
        e.Handled = true;
        goto case Key.Return;
      case Key.D6:
        this.AppendDigit(6);
        e.Handled = true;
        goto case Key.Return;
      case Key.D7:
        this.AppendDigit(7);
        e.Handled = true;
        goto case Key.Return;
      case Key.D8:
        this.AppendDigit(8);
        e.Handled = true;
        goto case Key.Return;
      case Key.D9:
        this.AppendDigit(9);
        e.Handled = true;
        goto case Key.Return;
      case Key.NumPad0:
        this.AppendDigit(0);
        e.Handled = true;
        goto case Key.Return;
      case Key.NumPad1:
        this.AppendDigit(1);
        e.Handled = true;
        goto case Key.Return;
      case Key.NumPad2:
        this.AppendDigit(2);
        e.Handled = true;
        goto case Key.Return;
      case Key.NumPad3:
        this.AppendDigit(3);
        e.Handled = true;
        goto case Key.Return;
      case Key.NumPad4:
        this.AppendDigit(4);
        e.Handled = true;
        goto case Key.Return;
      case Key.NumPad5:
        this.AppendDigit(5);
        e.Handled = true;
        goto case Key.Return;
      case Key.NumPad6:
        this.AppendDigit(6);
        e.Handled = true;
        goto case Key.Return;
      case Key.NumPad7:
        this.AppendDigit(7);
        e.Handled = true;
        goto case Key.Return;
      case Key.NumPad8:
        this.AppendDigit(8);
        e.Handled = true;
        goto case Key.Return;
      case Key.NumPad9:
        this.AppendDigit(9);
        e.Handled = true;
        goto case Key.Return;
      default:
        if (!Keyboard.IsKeyDown(Key.F4))
          e.Handled = true;
        this.keyCatch = string.Empty;
        goto case Key.Return;
    }
  }

  public ICommand UpCommand
  {
    get
    {
      if (this.upCommand == null)
        this.upCommand = (ICommand) new DelegateCommand((Action<object>) (param => this.UpExecute()), (Predicate<object>) (param => this.UpCanExecute()));
      return this.upCommand;
    }
  }

  public ICommand DownCommand
  {
    get
    {
      if (this.downCommand == null)
        this.downCommand = (ICommand) new DelegateCommand((Action<object>) (param => this.DownExecute()), (Predicate<object>) (param => this.DownCanExecute()));
      return this.downCommand;
    }
  }

  private bool HandleRight()
  {
    int num = this.SelectionStart;
    if (this.tPosition.ContainsKey(this.SelectionStart))
      num = this.SelectionStart + this.tLength[this.SelectionStart];
    for (int index = num; index < this.Text.Length; ++index)
    {
      if (this.tPosition.ContainsKey(index + 1))
      {
        this.selectionStartChanged = true;
        this.selectionStart = index + 1;
        this.SelectionStart = index + 1;
        return true;
      }
    }
    this.keyCatch = string.Empty;
    return false;
  }

  private void HandleLeft()
  {
    for (int key = this.SelectionStart - 1; key >= 0; --key)
    {
      if (this.tPosition.ContainsKey(key))
      {
        this.selectionStartChanged = true;
        this.selectionStart = this.tStart[key];
        this.Select(this.tStart[key], this.tLength[key]);
        this.SelectionStart = this.tStart[key];
        break;
      }
    }
    this.keyCatch = string.Empty;
  }

  protected override void OnGotFocus(RoutedEventArgs e)
  {
    if (Keyboard.IsKeyDown(Key.Tab))
      this.SelectAll();
    else
      this.UpdateSelectedSpan();
    base.OnGotFocus(e);
  }

  protected override void OnSelectionChanged(RoutedEventArgs e)
  {
    if (!this.isSelectionChanged && this.SelectionStart == 0 && this.SelectionLength == this.Text.Length)
    {
      this.isSelectionChanged = true;
      this.SelectAll();
    }
    else if (!this.isSelectionChanged && !this.isAppendDigit && !this.isSpinButtonPressed)
    {
      if (this.selectionStartChanged && this.tPosition.ContainsKey(this.selectionStart))
      {
        this.selectionStartChanged = false;
        this.isSelectionChanged = true;
        this.Select(this.tStart[this.selectionStart], this.tLength[this.selectionStart]);
        this.SelectionStart = this.tStart[this.selectionStart];
      }
      if (this.tPosition.ContainsKey(this.SelectionStart))
      {
        switch (this.tPosition[this.SelectionStart])
        {
          case 'd':
            this.SelectedSpan = TimeSpanEdit.TimeSpanElements.Days;
            break;
          case 'h':
            this.SelectedSpan = TimeSpanEdit.TimeSpanElements.Hours;
            break;
          case 'm':
            this.SelectedSpan = TimeSpanEdit.TimeSpanElements.Minutes;
            break;
          case 's':
            this.SelectedSpan = TimeSpanEdit.TimeSpanElements.Seconds;
            break;
          case 'z':
            this.SelectedSpan = TimeSpanEdit.TimeSpanElements.MilliSeconds;
            break;
        }
        this.isSelectionChanged = true;
        this.Select(this.tStart[this.SelectionStart], this.tLength[this.SelectionStart]);
        this.SelectionStart = this.tStart[this.SelectionStart];
      }
      else
      {
        this.isSelectionChanged = true;
        this.Select(this.SelectionStart, 0);
      }
    }
    base.OnSelectionChanged(e);
  }

  private void UpdateSelectedSpan()
  {
    if (this.tPosition == null || this.tPosition.Count <= this.SelectionStart || !this.tPosition.ContainsKey(this.SelectionStart))
      return;
    switch (this.tPosition[this.SelectionStart])
    {
      case 'd':
        this.SelectedSpan = TimeSpanEdit.TimeSpanElements.Days;
        break;
      case 'h':
        this.SelectedSpan = TimeSpanEdit.TimeSpanElements.Hours;
        break;
      case 'm':
        this.SelectedSpan = TimeSpanEdit.TimeSpanElements.Minutes;
        break;
      case 's':
        this.SelectedSpan = TimeSpanEdit.TimeSpanElements.Seconds;
        break;
      case 'z':
        this.SelectedSpan = TimeSpanEdit.TimeSpanElements.MilliSeconds;
        break;
    }
  }

  private void AppendDigit(int val)
  {
    this.UpdateSelectedSpan();
    this.keyCatch += val.ToString();
    int result1 = 0;
    int.TryParse(this.keyCatch, out result1);
    TimeSpan timeSpan1 = new TimeSpan();
    TimeSpan timeSpan2 = new TimeSpan();
    this.isSelectionChanged = false;
    this.isAppendDigit = true;
    if (this.Value.HasValue && this.keyCatch.Length < 9)
    {
      TimeSpan timeSpan3 = this.Value.Value;
      switch (this.SelectedSpan)
      {
        case TimeSpanEdit.TimeSpanElements.Days:
          try
          {
            if (TimeSpan.TryParse(this.keyCatch, out TimeSpan _))
            {
              timeSpan1 = new TimeSpan(result1, timeSpan3.Hours, timeSpan3.Minutes, timeSpan3.Seconds, timeSpan3.Milliseconds);
              break;
            }
            timeSpan1 = new TimeSpan();
            this.keyCatch = string.Empty;
            break;
          }
          catch
          {
            break;
          }
        case TimeSpanEdit.TimeSpanElements.Hours:
          if (this.isDaysVisibile)
          {
            this.tempTimeSpan = timeSpan3.Hours.ToString() + (object) val;
            int result2;
            int.TryParse(this.tempTimeSpan, out result2);
            if (result2 >= 24 && timeSpan3.Hours.ToString().Length > 1)
            {
              this.tempTimeSpan = timeSpan3.Hours.ToString().Substring(1, timeSpan3.Hours.ToString().Length - 1) + (object) val;
              int.TryParse(this.tempTimeSpan, out result2);
              this.keyCatch = string.Empty;
            }
            if (result2 < 24)
            {
              timeSpan1 = new TimeSpan(timeSpan3.Days, result2, timeSpan3.Minutes, timeSpan3.Seconds, timeSpan3.Milliseconds);
            }
            else
            {
              timeSpan1 = new TimeSpan(timeSpan3.Days, val, timeSpan3.Minutes, timeSpan3.Seconds, timeSpan3.Milliseconds);
              this.keyCatch = string.Empty;
            }
            this.tempTimeSpan = string.Empty;
            result2 = 0;
            break;
          }
          if (this.isMinMaxValidate)
          {
            this.tempTimeSpan = timeSpan3.Minutes > 0 | timeSpan3.Seconds > 0 || timeSpan3.Milliseconds > 0 ? ((int) Math.Ceiling(timeSpan3.TotalMinutes / 60.0) - 1).ToString() + (object) val : ((int) Math.Ceiling(timeSpan3.TotalMinutes / 60.0)).ToString() + (object) val;
            int result3;
            int.TryParse(this.tempTimeSpan, out result3);
            if ((double) result3 > this.MaxValue.TotalHours)
            {
              this.tempTimeSpan = this.tempTimeSpan.ToString().Substring(1, this.tempTimeSpan.ToString().Length - 1);
              int.TryParse(this.tempTimeSpan, out result3);
              this.keyCatch = string.Empty;
            }
            if ((double) result3 <= this.MaxValue.TotalHours && ((double) result3 != this.MaxValue.TotalHours || timeSpan3.Minutes <= 0 && timeSpan3.Seconds <= 0 && timeSpan3.Milliseconds <= 0))
            {
              timeSpan1 = new TimeSpan(result3, timeSpan3.Minutes, timeSpan3.Seconds);
            }
            else
            {
              timeSpan1 = new TimeSpan(val, timeSpan3.Minutes, timeSpan3.Seconds);
              this.keyCatch = string.Empty;
            }
            this.tempTimeSpan = string.Empty;
            result3 = 0;
            break;
          }
          timeSpan1 = new TimeSpan(0, result1, timeSpan3.Minutes, timeSpan3.Seconds, timeSpan3.Milliseconds);
          if (this.keyCatch.Length >= 8)
          {
            this.keyCatch = string.Empty;
            break;
          }
          break;
        case TimeSpanEdit.TimeSpanElements.Minutes:
          if (this.isHoursVisible)
          {
            this.tempTimeSpan = timeSpan3.Minutes.ToString() + (object) val;
            int result4;
            int.TryParse(this.tempTimeSpan, out result4);
            if (result4 >= 60 && timeSpan3.Minutes.ToString().Length > 1)
            {
              this.tempTimeSpan = timeSpan3.Minutes.ToString().Substring(1, timeSpan3.Minutes.ToString().Length - 1) + (object) val;
              int.TryParse(this.tempTimeSpan, out result4);
              this.keyCatch = string.Empty;
            }
            if (result4 < 60)
            {
              timeSpan1 = new TimeSpan(timeSpan3.Days, timeSpan3.Hours, result4, timeSpan3.Seconds, timeSpan3.Milliseconds);
            }
            else
            {
              timeSpan1 = new TimeSpan(timeSpan3.Days, timeSpan3.Hours, val, timeSpan3.Seconds, timeSpan3.Milliseconds);
              this.keyCatch = string.Empty;
            }
            this.tempTimeSpan = string.Empty;
            result4 = 0;
            break;
          }
          if (this.isMinMaxValidate)
          {
            this.tempTimeSpan = timeSpan3.Seconds > 0 || timeSpan3.Milliseconds > 0 ? ((int) Math.Ceiling(timeSpan3.TotalSeconds / 60.0) - 1).ToString() + (object) val : ((int) Math.Ceiling(timeSpan3.TotalSeconds / 60.0)).ToString() + (object) val;
            int result5;
            int.TryParse(this.tempTimeSpan, out result5);
            if ((double) result5 > this.MaxValue.TotalMinutes)
            {
              this.tempTimeSpan = this.tempTimeSpan.ToString().Substring(1, this.tempTimeSpan.ToString().Length - 1);
              int.TryParse(this.tempTimeSpan, out result5);
              this.keyCatch = string.Empty;
            }
            if ((double) result5 <= this.MaxValue.TotalMinutes && ((double) result5 != this.MaxValue.TotalMinutes || timeSpan3.Seconds <= 0 && timeSpan3.Milliseconds <= 0))
            {
              timeSpan1 = new TimeSpan(0, result5, timeSpan3.Seconds);
            }
            else
            {
              timeSpan1 = new TimeSpan(0, val, timeSpan3.Seconds);
              this.keyCatch = string.Empty;
            }
            this.tempTimeSpan = string.Empty;
            result5 = 0;
            break;
          }
          timeSpan1 = new TimeSpan(0, 0, result1, timeSpan3.Seconds, timeSpan3.Milliseconds);
          if (this.keyCatch.Length >= 8)
          {
            this.keyCatch = string.Empty;
            break;
          }
          break;
        case TimeSpanEdit.TimeSpanElements.Seconds:
          if (this.isMinutesVisible)
          {
            this.tempTimeSpan = timeSpan3.Seconds.ToString() + (object) val;
            int result6;
            int.TryParse(this.tempTimeSpan, out result6);
            if (result6 >= 60 && timeSpan3.Seconds.ToString().Length > 1)
            {
              this.tempTimeSpan = timeSpan3.Seconds.ToString().Substring(1, timeSpan3.Seconds.ToString().Length - 1) + (object) val;
              int.TryParse(this.tempTimeSpan, out result6);
              this.keyCatch = string.Empty;
            }
            if (result6 < 60)
            {
              timeSpan1 = new TimeSpan(timeSpan3.Days, timeSpan3.Hours, timeSpan3.Minutes, result6, timeSpan3.Milliseconds);
            }
            else
            {
              timeSpan1 = new TimeSpan(timeSpan3.Days, timeSpan3.Hours, timeSpan3.Minutes, val, timeSpan3.Milliseconds);
              this.keyCatch = string.Empty;
            }
            this.tempTimeSpan = string.Empty;
            result6 = 0;
            break;
          }
          if (this.isMinMaxValidate)
          {
            this.tempTimeSpan = timeSpan3.Milliseconds <= 0 ? ((int) Math.Ceiling(timeSpan3.TotalMilliseconds / 1000.0)).ToString() + (object) val : ((int) Math.Ceiling(timeSpan3.TotalMilliseconds / 1000.0) - 1).ToString() + (object) val;
            int result7;
            int.TryParse(this.tempTimeSpan, out result7);
            if ((double) result7 > this.MaxValue.TotalSeconds)
            {
              this.tempTimeSpan = this.tempTimeSpan.ToString().Substring(1, this.tempTimeSpan.ToString().Length - 1);
              int.TryParse(this.tempTimeSpan, out result7);
              this.keyCatch = string.Empty;
            }
            if ((double) result7 <= this.MaxValue.TotalSeconds && ((double) result7 != this.MaxValue.TotalMinutes || timeSpan3.Seconds <= 0))
            {
              timeSpan1 = new TimeSpan(0, 0, result7);
            }
            else
            {
              timeSpan1 = new TimeSpan(0, 0, val);
              this.keyCatch = string.Empty;
            }
            this.tempTimeSpan = string.Empty;
            result7 = 0;
            break;
          }
          timeSpan1 = new TimeSpan(0, 0, 0, result1, timeSpan3.Milliseconds);
          if (this.keyCatch.Length >= 8)
          {
            this.keyCatch = string.Empty;
            break;
          }
          break;
        case TimeSpanEdit.TimeSpanElements.MilliSeconds:
          if (this.isSecondsVisible)
          {
            this.tempTimeSpan = timeSpan3.Milliseconds.ToString() + (object) val;
            int result8;
            int.TryParse(this.tempTimeSpan, out result8);
            if (result8 > 999 && timeSpan3.Milliseconds.ToString().Length > 1)
            {
              this.tempTimeSpan = timeSpan3.Milliseconds.ToString().Substring(1, timeSpan3.Milliseconds.ToString().Length - 1) + (object) val;
              int.TryParse(this.tempTimeSpan, out result8);
              this.keyCatch = string.Empty;
            }
            if (result8 < 999)
            {
              timeSpan1 = new TimeSpan(timeSpan3.Days, timeSpan3.Hours, timeSpan3.Minutes, timeSpan3.Seconds, result8);
            }
            else
            {
              timeSpan1 = new TimeSpan(timeSpan3.Days, timeSpan3.Hours, timeSpan3.Minutes, timeSpan3.Seconds, val);
              this.keyCatch = string.Empty;
            }
            this.tempTimeSpan = string.Empty;
            result8 = 0;
            break;
          }
          if (this.isMinMaxValidate)
          {
            this.tempTimeSpan = timeSpan3.TotalMilliseconds.ToString() + (object) val;
            int result9;
            int.TryParse(this.tempTimeSpan, out result9);
            if ((double) result9 > this.MaxValue.TotalMilliseconds)
            {
              this.tempTimeSpan = this.tempTimeSpan.ToString().Substring(1, this.tempTimeSpan.ToString().Length - 1);
              int.TryParse(this.tempTimeSpan, out result9);
              this.keyCatch = string.Empty;
            }
            if ((double) result9 <= this.MaxValue.TotalMilliseconds)
            {
              timeSpan1 = new TimeSpan(0, 0, 0, 0, result9);
            }
            else
            {
              timeSpan1 = new TimeSpan(0, 0, 0, 0, val);
              this.keyCatch = string.Empty;
            }
            this.tempTimeSpan = string.Empty;
            result9 = 0;
            break;
          }
          timeSpan1 = new TimeSpan(0, 0, 0, 0, result1);
          this.keyCatch = string.Empty;
          break;
      }
      if (timeSpan1.Days >= this.MinValue.Days && timeSpan1.Days <= this.MaxValue.Days || timeSpan1.Hours >= this.MinValue.Hours && timeSpan1.Hours <= this.MaxValue.Hours || timeSpan1.Minutes >= this.MinValue.Minutes && timeSpan1.Minutes <= this.MaxValue.Minutes || timeSpan1.Seconds >= this.MinValue.Seconds && timeSpan1.Seconds <= this.MaxValue.Seconds || timeSpan1.Milliseconds >= this.MinValue.Milliseconds && timeSpan1.Milliseconds <= this.MaxValue.Milliseconds)
        this.Value = new TimeSpan?(timeSpan1);
      else
        this.keyCatch = string.Empty;
    }
    else
    {
      this.Value = new TimeSpan?(this.MinValue);
      this.keyCatch = string.Empty;
    }
    this.isAppendDigit = false;
    if (!this.tPosition.ContainsKey(this.SelectionStart))
      return;
    this.Select(this.tStart[this.SelectionStart], this.tLength[this.SelectionStart]);
  }

  private void IncreaseorDecreaseSpanValue(bool isUp)
  {
    if (!this.Value.HasValue || this.IsReadOnly)
      return;
    int days = 0;
    int hours = 0;
    int minutes = 0;
    int seconds = 0;
    int milliseconds = 0;
    switch (this.SelectedSpan)
    {
      case TimeSpanEdit.TimeSpanElements.Days:
        days = this.StepInterval.Days;
        break;
      case TimeSpanEdit.TimeSpanElements.Hours:
        hours = this.StepInterval.Hours;
        break;
      case TimeSpanEdit.TimeSpanElements.Minutes:
        minutes = this.StepInterval.Minutes;
        break;
      case TimeSpanEdit.TimeSpanElements.Seconds:
        seconds = this.StepInterval.Seconds;
        break;
      case TimeSpanEdit.TimeSpanElements.MilliSeconds:
        milliseconds = this.StepInterval.Milliseconds;
        break;
    }
    TimeSpan ts = new TimeSpan(days, hours, minutes, seconds, milliseconds);
    if (isUp)
      this.Value = new TimeSpan?(this.Value.Value.Add(ts) > this.MaxValue ? this.MaxValue : this.Value.Value.Add(ts));
    else
      this.Value = new TimeSpan?(this.Value.Value.Subtract(ts) < this.MinValue ? this.MinValue : this.Value.Value.Subtract(ts));
  }

  internal void UpExecute()
  {
    this.isSpinButtonPressed = true;
    if (!this.Value.HasValue)
    {
      this.Value = new TimeSpan?(this.MaxValue);
    }
    else
    {
      this.IncreaseorDecreaseSpanValue(true);
      this.selectionStartChanged = true;
      if (this.tPosition.ContainsKey(this.SelectionStart))
        this.Select(this.tStart[this.SelectionStart], this.tLength[this.SelectionStart]);
      this.selectionStartChanged = false;
    }
    this.isSpinButtonPressed = false;
  }

  internal void DownExecute()
  {
    this.isSpinButtonPressed = true;
    if (!this.Value.HasValue)
    {
      this.Value = new TimeSpan?(this.MinValue);
    }
    else
    {
      this.IncreaseorDecreaseSpanValue(false);
      this.selectionStartChanged = true;
      if (this.tPosition.ContainsKey(this.SelectionStart))
        this.Select(this.tStart[this.SelectionStart], this.tLength[this.SelectionStart]);
      this.selectionStartChanged = false;
    }
    this.isSpinButtonPressed = false;
  }

  private bool UpCanExecute()
  {
    if (!this.Value.HasValue)
      return true;
    TimeSpan? nullable = this.Value;
    TimeSpan maxValue = this.MaxValue;
    return (nullable.HasValue ? (nullable.GetValueOrDefault() <= maxValue ? 1 : 0) : 0) != 0;
  }

  private bool DownCanExecute()
  {
    if (!this.Value.HasValue)
      return true;
    TimeSpan? nullable = this.Value;
    TimeSpan minValue = this.MinValue;
    return (nullable.HasValue ? (nullable.GetValueOrDefault() >= minValue ? 1 : 0) : 0) != 0;
  }

  public static void OnNullStringChanged(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs args)
  {
    (sender as TimeSpanEdit).OnNullStringChanged(args);
  }

  private void OnNullStringChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.Value.HasValue)
      return;
    this.Text = this.NullString;
  }

  public static void OnMinValueChanged(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs args)
  {
    (sender as TimeSpanEdit).OnMinValueChanged(args);
  }

  private void OnMaxValueChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.MinValue > this.MaxValue)
      throw new InvalidOperationException("MaxValue should not be lesser than MinValue");
    TimeSpan? nullable = this.Value;
    TimeSpan maxValue = this.MaxValue;
    if ((nullable.HasValue ? (nullable.GetValueOrDefault() > maxValue ? 1 : 0) : 0) != 0)
      this.Value = new TimeSpan?(this.MaxValue);
    this.isMinMaxValidate = true;
  }

  public static void OnMaxValueChanged(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs args)
  {
    (sender as TimeSpanEdit).OnMaxValueChanged(args);
  }

  private void OnMinValueChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.MaxValue < this.MinValue)
      throw new InvalidOperationException("MinValue should not be greater than MaxValue");
    TimeSpan? nullable = this.Value;
    TimeSpan minValue = this.MinValue;
    if ((nullable.HasValue ? (nullable.GetValueOrDefault() < minValue ? 1 : 0) : 0) == 0)
      return;
    this.Value = new TimeSpan?(this.MinValue);
  }

  public static void OnFormatStringChanged(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs args)
  {
    (sender as TimeSpanEdit).OnFormatStringChanged(args);
  }

  private void OnFormatStringChanged(DependencyPropertyChangedEventArgs args)
  {
    this.CreateDisplayText();
  }

  public static void OnAllowNullChanged(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs args)
  {
    (sender as TimeSpanEdit).OnAllowNullChanged(args);
  }

  private void OnAllowNullChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.Value.HasValue)
      return;
    this.Text = this.NullString;
  }

  public static void OnValueChanged(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs args)
  {
    (sender as TimeSpanEdit).OnValueChanged(args);
  }

  private void OnValueChanged(DependencyPropertyChangedEventArgs args)
  {
    TimeSpan? nullable1 = this.Value;
    TimeSpan maxValue = this.MaxValue;
    if ((nullable1.HasValue ? (nullable1.GetValueOrDefault() > maxValue ? 1 : 0) : 0) != 0)
    {
      this.Value = new TimeSpan?(this.MaxValue);
    }
    else
    {
      TimeSpan? nullable2 = this.Value;
      TimeSpan minValue = this.MinValue;
      if ((nullable2.HasValue ? (nullable2.GetValueOrDefault() < minValue ? 1 : 0) : 0) != 0)
        this.Value = new TimeSpan?(this.MinValue);
      else if (!this.Value.HasValue && !this.AllowNull)
      {
        this.Value = new TimeSpan?(new TimeSpan(0, 0, 0, 0));
      }
      else
      {
        this.CreateDisplayText();
        if (this.ValueChanged == null)
          return;
        this.ValueChanged((DependencyObject) this, args);
      }
    }
  }

  private void CreateDisplayText()
  {
    this.UpdateSelectedSpan();
    TimeSpan timeSpan1 = new TimeSpan();
    if (this.Value.HasValue && this.Format != string.Empty)
    {
      TimeSpan timeSpan2 = this.Value.Value;
      string str1 = this.Format.ToString();
      string str2 = string.Empty;
      this.tPosition.Clear();
      this.tLength.Clear();
      this.tStart.Clear();
      if (this.Format != string.Empty)
      {
        str1.Split('\'');
        bool flag1 = false;
        bool flag2 = false;
        for (int index1 = 0; index1 < this.Format.Length; ++index1)
        {
          char ch = str1[index1];
          if (ch == '\'')
            flag1 = !flag1;
          else if (flag1 && ch != '\'')
          {
            str2 += (string) (object) ch;
          }
          else
          {
            if (index1 + 1 < this.Format.Length && (int) str1[index1 + 1] == (int) str1[index1])
            {
              ++index1;
              flag2 = true;
            }
            int num1;
            switch (ch)
            {
              case 'd':
                try
                {
                  for (int index2 = 0; index2 < timeSpan2.Days.ToString().Length + 1; ++index2)
                  {
                    this.tPosition.Add(str2.Length + index2, 'd');
                    num1 = timeSpan2.Days;
                    if (num1.ToString().Length <= 1 && flag2)
                    {
                      string str3 = $"{timeSpan2.Days:00}";
                      if (!string.IsNullOrEmpty(str3))
                        this.tLength.Add(str2.Length + index2, str3.Length);
                    }
                    else
                      this.tLength.Add(str2.Length + index2, timeSpan2.Days.ToString().Length);
                    this.tStart.Add(str2.Length + index2, str2.Length);
                    this.isDaysVisibile = true;
                  }
                }
                catch
                {
                }
                str2 = !flag2 ? str2 + timeSpan2.Days.ToString() : str2 + timeSpan2.ToString("dd");
                flag2 = false;
                continue;
              case 'h':
                try
                {
                  for (int index3 = 0; index3 < timeSpan2.Hours.ToString().Length + 1; ++index3)
                  {
                    this.tPosition.Add(str2.Length + index3, 'h');
                    if (this.isDaysVisibile)
                    {
                      if (timeSpan2.Hours.ToString().Length <= 1 && flag2)
                      {
                        string str4 = $"{timeSpan2.Hours:00}";
                        if (!string.IsNullOrEmpty(str4))
                          this.tLength.Add(str2.Length + index3, str4.Length);
                      }
                      else
                        this.tLength.Add(str2.Length + index3, timeSpan2.Hours.ToString().Length);
                    }
                    else if (((int) timeSpan2.TotalHours).ToString().Length <= 1 && flag2)
                    {
                      string str5 = $"{timeSpan2.TotalHours:00}";
                      if (!string.IsNullOrEmpty(str5))
                        this.tLength.Add(str2.Length + index3, str5.Length);
                    }
                    else
                      this.tLength.Add(str2.Length + index3, ((int) timeSpan2.TotalHours).ToString().Length);
                    this.tStart.Add(str2.Length + index3, str2.Length);
                    this.isHoursVisible = true;
                  }
                }
                catch
                {
                }
                str2 = !this.isDaysVisibile ? (((int) timeSpan2.TotalHours).ToString().Length >= 2 || !flag2 ? str2 + ((int) timeSpan2.TotalHours).ToString() : str2 + (object) 0 + ((int) timeSpan2.TotalHours).ToString()) : (!flag2 ? str2 + timeSpan2.Hours.ToString() : str2 + timeSpan2.ToString("hh"));
                flag2 = false;
                continue;
              case 'm':
                try
                {
                  for (int index4 = 0; index4 < timeSpan2.Minutes.ToString().Length + 1; ++index4)
                  {
                    this.tPosition.Add(str2.Length + index4, 'm');
                    if (this.isHoursVisible)
                    {
                      if (timeSpan2.Minutes.ToString().Length <= 1 && flag2)
                      {
                        string str6 = $"{timeSpan2.Minutes:00}";
                        if (!string.IsNullOrEmpty(str6))
                          this.tLength.Add(str2.Length + index4, str6.Length);
                      }
                      else
                        this.tLength.Add(str2.Length + index4, timeSpan2.Minutes.ToString().Length);
                    }
                    else if (((int) timeSpan2.TotalMinutes).ToString().Length <= 1 && flag2)
                    {
                      string str7 = $"{timeSpan2.TotalMinutes:00}";
                      if (!string.IsNullOrEmpty(str7))
                        this.tLength.Add(str2.Length + index4, str7.Length);
                    }
                    else
                      this.tLength.Add(str2.Length + index4, ((int) timeSpan2.TotalMinutes).ToString().Length);
                    this.tStart.Add(str2.Length + index4, str2.Length);
                    this.isMinutesVisible = true;
                  }
                }
                catch
                {
                }
                str2 = !this.isHoursVisible ? (((int) timeSpan2.TotalMinutes).ToString().Length >= 2 || !flag2 ? str2 + ((int) timeSpan2.TotalMinutes).ToString() : str2 + (object) 0 + ((int) timeSpan2.TotalMinutes).ToString()) : (!flag2 ? str2 + timeSpan2.Minutes.ToString() : str2 + timeSpan2.ToString("mm"));
                flag2 = false;
                continue;
              case 's':
                try
                {
                  for (int index5 = 0; index5 < timeSpan2.Seconds.ToString().Length + 1; ++index5)
                  {
                    this.tPosition.Add(str2.Length + index5, 's');
                    if (this.isMinutesVisible)
                    {
                      if (timeSpan2.Seconds.ToString().Length <= 1 && flag2)
                      {
                        string str8 = $"{timeSpan2.Seconds:00}";
                        if (!string.IsNullOrEmpty(str8))
                          this.tLength.Add(str2.Length + index5, str8.Length);
                      }
                      else
                        this.tLength.Add(str2.Length + index5, timeSpan2.Seconds.ToString().Length);
                    }
                    else if (((int) timeSpan2.TotalSeconds).ToString().Length <= 1 && flag2)
                    {
                      string str9 = $"{timeSpan2.TotalSeconds:00}";
                      if (!string.IsNullOrEmpty(str9))
                        this.tLength.Add(str2.Length + index5, str9.Length);
                    }
                    else
                      this.tLength.Add(str2.Length + index5, ((int) timeSpan2.TotalSeconds).ToString().Length);
                    this.tStart.Add(str2.Length + index5, str2.Length);
                    this.isSecondsVisible = true;
                  }
                }
                catch
                {
                }
                str2 = !this.isMinutesVisible ? (((int) timeSpan2.TotalSeconds).ToString().Length >= 2 || !flag2 ? str2 + ((int) timeSpan2.TotalSeconds).ToString() : str2 + (object) 0 + ((int) timeSpan2.TotalSeconds).ToString()) : (!flag2 ? str2 + timeSpan2.Seconds.ToString() : str2 + timeSpan2.ToString("ss"));
                flag2 = false;
                continue;
              case 'z':
                try
                {
                  int num2 = 0;
                  while (true)
                  {
                    int num3 = num2;
                    num1 = timeSpan2.Milliseconds;
                    int num4 = num1.ToString().Length + 1;
                    if (num3 < num4)
                    {
                      this.tPosition.Add(str2.Length + num2, 'z');
                      if (this.isSecondsVisible)
                        this.tLength.Add(str2.Length + num2, timeSpan2.Milliseconds.ToString().Length);
                      else
                        this.tLength.Add(str2.Length + num2, ((int) timeSpan2.TotalMilliseconds).ToString().Length);
                      this.tStart.Add(str2.Length + num2, str2.Length);
                      ++num2;
                    }
                    else
                      break;
                  }
                }
                catch
                {
                }
                if (this.isSecondsVisible)
                {
                  if (flag2)
                  {
                    string empty = string.Empty;
                    for (int index6 = index1; index6 <= this.Format.Length; ++index6)
                    {
                      if ('z' == str1[index1])
                        empty += "f";
                    }
                    index1 += empty.Length;
                    str2 += timeSpan2.ToString(empty);
                  }
                  else
                  {
                    num1 = timeSpan2.Milliseconds;
                    string str10 = num1.ToString();
                    str2 += str10.Length > 0 ? str10[0].ToString() : str10;
                  }
                }
                else
                  str2 += ((int) timeSpan2.TotalMilliseconds).ToString();
                flag2 = false;
                continue;
              default:
                str2 += (string) (object) ch;
                continue;
            }
          }
        }
      }
      int selectionStart = this.SelectionStart;
      this.selectionStart = this.SelectionStart;
      this.selectionStartChanged = true;
      int num;
      if (!this.tPosition.ContainsKey(selectionStart))
      {
        string selectedSpan = this.SelectedSpan.ToString().ToLower();
        num = !(selectedSpan == "milliseconds") ? this.tPosition.FirstOrDefault<KeyValuePair<int, char>>((Func<KeyValuePair<int, char>, bool>) (x => (int) x.Value == (int) selectedSpan[0])).Key : this.tPosition.FirstOrDefault<KeyValuePair<int, char>>((Func<KeyValuePair<int, char>, bool>) (x => x.Value == 'z')).Key;
        this.selectionStart = num;
      }
      else
      {
        string selectedSpan = this.SelectedSpan.ToString().ToLower();
        num = !(selectedSpan == "milliseconds") ? this.tPosition.FirstOrDefault<KeyValuePair<int, char>>((Func<KeyValuePair<int, char>, bool>) (x => (int) x.Value == (int) selectedSpan[0])).Key : this.tPosition.FirstOrDefault<KeyValuePair<int, char>>((Func<KeyValuePair<int, char>, bool>) (x => x.Value == 'z')).Key;
        this.selectionStart = num;
      }
      this.Text = str2;
      this.selectionStartChanged = false;
      this.SelectionStart = num;
    }
    else
      this.Text = this.NullString;
  }

  public bool EnableTouch
  {
    get => (bool) this.GetValue(TimeSpanEdit.EnableTouchProperty);
    set => this.SetValue(TimeSpanEdit.EnableTouchProperty, (object) value);
  }

  public static void OnEnableTouchChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    if (!(obj is TimeSpanEdit))
      return;
    (obj as TimeSpanEdit).OnEnableTouchChanged(args);
  }

  private void OnEnableTouchChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.aLayer == null)
      this.aLayer = AdornerLayer.GetAdornerLayer((Visual) this);
    if ((bool) args.NewValue)
    {
      if (this.aLayer == null)
        return;
      this.txtSelectionAdorner1 = new TextBoxSelectionAdorner((UIElement) this);
      this.aLayer.Add((Adorner) this.txtSelectionAdorner1);
    }
    else
    {
      if (this.aLayer == null)
        return;
      this.aLayer.Remove((Adorner) this.txtSelectionAdorner1);
    }
  }

  public bool EnableExtendedScrolling
  {
    get => (bool) this.GetValue(TimeSpanEdit.EnableExtendedScrollingProperty);
    set => this.SetValue(TimeSpanEdit.EnableExtendedScrollingProperty, (object) value);
  }

  public static void OnEnableExtendedScrollingChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    if (!(obj is TimeSpanEdit))
      return;
    (obj as TimeSpanEdit).OnEnableExtendedScrollingChanged(args);
  }

  private void OnEnableExtendedScrollingChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.aLayer == null)
      this.aLayer = AdornerLayer.GetAdornerLayer((Visual) this);
    if ((bool) args.NewValue)
    {
      if (this.aLayer == null)
        return;
      this.vAdorner = new ExtendedScrollingAdorner((UIElement) this);
      this.aLayer.Add((Adorner) this.vAdorner);
    }
    else
    {
      if (this.aLayer == null)
        return;
      this.aLayer.Remove((Adorner) this.vAdorner);
    }
  }

  private enum TimeSpanElements
  {
    Days,
    Hours,
    Minutes,
    Seconds,
    MilliSeconds,
  }
}
