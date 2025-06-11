// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.TimeBar
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Properties.Langs;
using HandyControl.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace HandyControl.Controls;

[TemplatePart(Name = "PART_BorderTop", Type = typeof (Border))]
[TemplatePart(Name = "PART_TextBlockMove", Type = typeof (TextBlock))]
[TemplatePart(Name = "PART_TextBlockSelected", Type = typeof (TextBlock))]
[TemplatePart(Name = "PART_CanvasSpe", Type = typeof (Canvas))]
[TemplatePart(Name = "PART_Hotspots", Type = typeof (Panel))]
public class TimeBar : Control
{
  private const string ElementBorderTop = "PART_BorderTop";
  private const string ElementTextBlockMove = "PART_TextBlockMove";
  private const string ElementTextBlockSelected = "PART_TextBlockSelected";
  private const string ElementCanvasSpe = "PART_CanvasSpe";
  private const string ElementHotspots = "PART_Hotspots";
  private Border _borderTop;
  private TextBlock _textBlockMove;
  private TextBlock _textBlockSelected;
  private Canvas _canvasSpe;
  private Panel _panelHotspots;
  public static readonly DependencyProperty HotspotsBrushProperty = DependencyProperty.Register(nameof (HotspotsBrush), typeof (Brush), typeof (TimeBar), new PropertyMetadata((object) null));
  public static readonly DependencyProperty ShowSpeStrProperty = DependencyProperty.Register(nameof (ShowSpeStr), typeof (bool), typeof (TimeBar), new PropertyMetadata(ValueBoxes.FalseBox));
  public static readonly DependencyProperty TimeFormatProperty = DependencyProperty.Register(nameof (TimeFormat), typeof (string), typeof (TimeBar), new PropertyMetadata((object) "yyyy-MM-dd HH:mm:ss"));
  internal static readonly DependencyProperty SpeStrProperty = DependencyProperty.Register(nameof (SpeStr), typeof (string), typeof (TimeBar), new PropertyMetadata((object) Lang.Interval1h));
  public static readonly DependencyProperty SelectedTimeProperty = DependencyProperty.Register(nameof (SelectedTime), typeof (DateTime), typeof (TimeBar), new PropertyMetadata((object) new DateTime(), new PropertyChangedCallback(TimeBar.OnSelectedTimeChanged)));
  public static readonly RoutedEvent TimeChangedEvent = EventManager.RegisterRoutedEvent("TimeChanged", RoutingStrategy.Bubble, typeof (EventHandler<FunctionEventArgs<DateTime>>), typeof (TimeBar));
  private readonly List<SpeTextBlock> _speBlockList = new List<SpeTextBlock>();
  private readonly DateTime _starTime;
  private readonly List<int> _timeSpeList = new List<int>()
  {
    7200000,
    3600000,
    1800000,
    600000,
    300000,
    60000,
    30000
  };
  private bool _borderTopIsMouseLeftButtonDown;
  private bool _isDragging;
  private double _itemWidth;
  private DateTime _mouseDownTime;
  private int _speCount = 13;
  private int _speIndex = 1;
  private double _tempOffsetX;
  private double _totalOffsetX;
  private readonly bool _isLoaded;
  private readonly SortedSet<DateTimeRange> _dateTimeRanges;

  [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
  [Bindable(true)]
  public Collection<DateTimeRange> Hotspots { get; }

  public Brush HotspotsBrush
  {
    get => (Brush) this.GetValue(TimeBar.HotspotsBrushProperty);
    set => this.SetValue(TimeBar.HotspotsBrushProperty, (object) value);
  }

  public bool ShowSpeStr
  {
    get => (bool) this.GetValue(TimeBar.ShowSpeStrProperty);
    set => this.SetValue(TimeBar.ShowSpeStrProperty, ValueBoxes.BooleanBox(value));
  }

  public string TimeFormat
  {
    get => (string) this.GetValue(TimeBar.TimeFormatProperty);
    set => this.SetValue(TimeBar.TimeFormatProperty, (object) value);
  }

  internal string SpeStr
  {
    get => (string) this.GetValue(TimeBar.SpeStrProperty);
    set => this.SetValue(TimeBar.SpeStrProperty, (object) value);
  }

  private static void OnSelectedTimeChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is TimeBar timeBar) || timeBar._textBlockSelected == null)
      return;
    timeBar.OnSelectedTimeChanged((DateTime) e.NewValue);
  }

  public DateTime SelectedTime
  {
    get => (DateTime) this.GetValue(TimeBar.SelectedTimeProperty);
    set => this.SetValue(TimeBar.SelectedTimeProperty, (object) value);
  }

  private void OnSelectedTimeChanged(DateTime time)
  {
    this._textBlockSelected.Text = time.ToString(this.TimeFormat);
    if (!this._isDragging && !this._borderTopIsMouseLeftButtonDown)
      this._totalOffsetX = (this._starTime - this.SelectedTime).TotalMilliseconds / (double) this._timeSpeList[this.SpeIndex] * this._itemWidth;
    this.UpdateSpeBlock();
    this.UpdateMouseFollowBlockPos();
  }

  public TimeBar()
  {
    this._starTime = DateTime.Now;
    this.SelectedTime = new DateTime(this._starTime.Year, this._starTime.Month, this._starTime.Day, 0, 0, 0);
    this._starTime = this.SelectedTime;
    this._isLoaded = true;
    ObservableCollection<DateTimeRange> observableCollection = new ObservableCollection<DateTimeRange>();
    this._dateTimeRanges = new SortedSet<DateTimeRange>(ComparerGenerator.GetComparer<DateTimeRange>());
    observableCollection.CollectionChanged += new NotifyCollectionChangedEventHandler(this.Items_CollectionChanged);
    this.Hotspots = (Collection<DateTimeRange>) observableCollection;
  }

  private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    if (e.Action == NotifyCollectionChangedAction.Add)
    {
      foreach (DateTimeRange newItem in (IEnumerable) e.NewItems)
        this._dateTimeRanges.Add(newItem);
    }
    else if (e.Action == NotifyCollectionChangedAction.Remove)
    {
      foreach (DateTimeRange oldItem in (IEnumerable) e.OldItems)
        this._dateTimeRanges.Remove(oldItem);
    }
    else if (e.Action == NotifyCollectionChangedAction.Replace)
    {
      foreach (DateTimeRange oldItem in (IEnumerable) e.OldItems)
        this._dateTimeRanges.Remove(oldItem);
      foreach (DateTimeRange newItem in (IEnumerable) e.NewItems)
        this._dateTimeRanges.Add(newItem);
    }
    else
    {
      if (e.Action != NotifyCollectionChangedAction.Reset)
        return;
      this._dateTimeRanges.Clear();
    }
  }

  public override void OnApplyTemplate()
  {
    if (this._borderTop != null)
    {
      this._borderTop.MouseLeftButtonDown -= new MouseButtonEventHandler(this.BorderTop_OnMouseLeftButtonDown);
      this._borderTop.PreviewMouseLeftButtonUp -= new MouseButtonEventHandler(this.BorderTop_OnPreviewMouseLeftButtonUp);
    }
    base.OnApplyTemplate();
    this._borderTop = this.GetTemplateChild("PART_BorderTop") as Border;
    this._textBlockMove = this.GetTemplateChild("PART_TextBlockMove") as TextBlock;
    this._textBlockSelected = this.GetTemplateChild("PART_TextBlockSelected") as TextBlock;
    this._canvasSpe = this.GetTemplateChild("PART_CanvasSpe") as Canvas;
    this._panelHotspots = this.GetTemplateChild("PART_Hotspots") as Panel;
    this.CheckNull();
    this._borderTop.MouseLeftButtonDown += new MouseButtonEventHandler(this.BorderTop_OnMouseLeftButtonDown);
    this._borderTop.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(this.BorderTop_OnPreviewMouseLeftButtonUp);
    MouseDragElementBehaviorEx elementBehaviorEx = new MouseDragElementBehaviorEx()
    {
      LockY = true
    };
    elementBehaviorEx.DragBegun += new MouseEventHandler(this.DragElementBehavior_OnDragBegun);
    elementBehaviorEx.Dragging += new MouseEventHandler(this.MouseDragElementBehavior_OnDragging);
    elementBehaviorEx.DragFinished += new MouseEventHandler(this.MouseDragElementBehavior_OnDragFinished);
    Interaction.GetBehaviors((DependencyObject) this._borderTop).Add((Behavior) elementBehaviorEx);
    if (this._isLoaded)
      this.Update();
    this._textBlockSelected.Text = this.SelectedTime.ToString(this.TimeFormat);
  }

  private void CheckNull()
  {
    if (this._borderTop == null || this._textBlockMove == null || this._textBlockSelected == null || this._canvasSpe == null)
      throw new Exception();
  }

  public int SpeIndex
  {
    get => this._speIndex;
    private set
    {
      if (this._speIndex == value)
        return;
      if (value < 0)
      {
        this.SpeStr = Lang.Interval2h;
        this._speIndex = 0;
      }
      else if (value > 6)
      {
        this.SpeStr = Lang.Interval30s;
        this._speIndex = 6;
      }
      else
      {
        this.SetSpeTimeFormat("HH:mm");
        switch (value)
        {
          case 0:
            this.SpeStr = Lang.Interval2h;
            break;
          case 1:
            this.SpeStr = Lang.Interval1h;
            break;
          case 2:
            this.SpeStr = Lang.Interval30m;
            break;
          case 3:
            this.SpeStr = Lang.Interval10m;
            break;
          case 4:
            this.SpeStr = Lang.Interval5m;
            break;
          case 5:
            this.SpeStr = Lang.Interval1m;
            break;
          case 6:
            this.SetSpeTimeFormat("HH:mm:ss");
            this.SpeStr = Lang.Interval30s;
            break;
        }
        this._speIndex = value;
      }
    }
  }

  public event EventHandler<FunctionEventArgs<DateTime>> TimeChanged
  {
    add => this.AddHandler(TimeBar.TimeChangedEvent, (Delegate) value);
    remove => this.RemoveHandler(TimeBar.TimeChangedEvent, (Delegate) value);
  }

  private void SetSpeTimeFormat(string format)
  {
    foreach (SpeTextBlock speBlock in this._speBlockList)
      speBlock.TimeFormat = format;
  }

  private void UpdateSpeBlock()
  {
    double num1 = (this._totalOffsetX + this._tempOffsetX) % this._itemWidth;
    for (int index = 0; index < this._speCount; ++index)
    {
      SpeTextBlock speBlock = this._speBlockList[index];
      speBlock.MoveX(num1 + (this._itemWidth - speBlock.Width) / 2.0);
    }
    int num2 = num1 <= 0.0 ? this._speCount / 2 : this._speCount / 2 - 1;
    for (int index = 0; index < this._speCount; ++index)
      this._speBlockList[index].Time = this.TimeConvert(this.SelectedTime).AddMilliseconds((double) ((index - num2) * this._timeSpeList[this._speIndex]));
    if (this._panelHotspots == null || !this._dateTimeRanges.Any<DateTimeRange>())
      return;
    this.UpdateHotspots();
  }

  private DateTime TimeConvert(DateTime time)
  {
    DateTime dateTime;
    switch (this._speIndex)
    {
      case 0:
        dateTime = new DateTime(time.Year, time.Month, time.Day, time.Hour / 2 * 2, 0, 0);
        break;
      case 1:
        dateTime = new DateTime(time.Year, time.Month, time.Day, time.Hour, 0, 0);
        break;
      case 2:
        dateTime = new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute / 30 * 30, 0);
        break;
      case 3:
        dateTime = new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute / 10 * 10, 0);
        break;
      case 4:
        dateTime = new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute / 5 * 5, 0);
        break;
      case 5:
        dateTime = new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, 0);
        break;
      case 6:
        dateTime = new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second / 30 * 30);
        break;
      default:
        dateTime = time;
        break;
    }
    return dateTime;
  }

  protected override void OnMouseWheel(MouseWheelEventArgs e)
  {
    base.OnMouseWheel(e);
    if (Mouse.LeftButton == MouseButtonState.Pressed)
      return;
    this.SpeIndex += e.Delta > 0 ? 1 : -1;
    this._totalOffsetX = (this._starTime - this.SelectedTime).TotalMilliseconds / (double) this._timeSpeList[this.SpeIndex] * this._itemWidth;
    this.UpdateSpeBlock();
    this.UpdateMouseFollowBlockPos();
    e.Handled = true;
  }

  private void MouseDragElementBehavior_OnDragging(object sender, MouseEventArgs e)
  {
    this._isDragging = true;
    this._tempOffsetX = this._borderTop.RenderTransform.Value.OffsetX;
    this.SelectedTime = this._mouseDownTime - TimeSpan.FromMilliseconds(this._tempOffsetX / this._itemWidth * (double) this._timeSpeList[this._speIndex]);
    this._borderTopIsMouseLeftButtonDown = false;
  }

  private void MouseDragElementBehavior_OnDragFinished(object sender, MouseEventArgs e)
  {
    this._tempOffsetX = 0.0;
    this._totalOffsetX = (this._totalOffsetX + this._borderTop.RenderTransform.Value.OffsetX) % this.ActualWidth;
    this._borderTop.RenderTransform = (Transform) new TranslateTransform();
    this.RaiseEvent((RoutedEventArgs) new FunctionEventArgs<DateTime>(TimeBar.TimeChangedEvent, (object) this)
    {
      Info = this.SelectedTime
    });
    this._isDragging = false;
  }

  private void DragElementBehavior_OnDragBegun(object sender, MouseEventArgs e)
  {
    this._mouseDownTime = this.SelectedTime;
  }

  protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
  {
    base.OnRenderSizeChanged(sizeInfo);
    this.Update();
  }

  private void Update()
  {
    if (this._canvasSpe == null)
      return;
    this._speBlockList.Clear();
    this._canvasSpe.Children.Clear();
    this._speCount = (int) (this.ActualWidth / 800.0 * 9.0) | 1;
    double itemWidth = this._itemWidth;
    this._itemWidth = this.ActualWidth / (double) this._speCount;
    this._totalOffsetX = this._itemWidth / itemWidth * this._totalOffsetX % this.ActualWidth;
    if (double.IsNaN(this._totalOffsetX))
      this._totalOffsetX = 0.0;
    double d = (this._totalOffsetX + this._tempOffsetX) % this._itemWidth;
    int num = d <= 0.0 || double.IsNaN(d) ? this._speCount / 2 : this._speCount / 2 - 1;
    for (int index = 0; index < this._speCount; ++index)
    {
      SpeTextBlock speTextBlock = new SpeTextBlock();
      speTextBlock.Time = this.TimeConvert(this.SelectedTime).AddMilliseconds((double) ((index - num) * this._timeSpeList[this._speIndex]));
      speTextBlock.TextAlignment = TextAlignment.Center;
      speTextBlock.TimeFormat = "HH:mm";
      SpeTextBlock element = speTextBlock;
      this._speBlockList.Add(element);
      this._canvasSpe.Children.Add((UIElement) element);
    }
    if (this._speIndex == 6)
      this.SetSpeTimeFormat("HH:mm:ss");
    this.ShowSpeStr = this.ActualWidth > 320.0;
    for (int index = 0; index < this._speCount; ++index)
    {
      SpeTextBlock speBlock = this._speBlockList[index];
      speBlock.X = this._itemWidth * (double) index;
      speBlock.MoveX((this._itemWidth - speBlock.Width) / 2.0);
    }
    this.UpdateSpeBlock();
    this.UpdateMouseFollowBlockPos();
  }

  protected override void OnMouseMove(MouseEventArgs e)
  {
    base.OnMouseMove(e);
    this.UpdateMouseFollowBlockPos();
  }

  private void UpdateMouseFollowBlockPos()
  {
    Point position = Mouse.GetPosition((IInputElement) this);
    double d = (position.X - this.ActualWidth / 2.0) / this._itemWidth * (double) this._timeSpeList[this._speIndex];
    if (double.IsNaN(d) || double.IsInfinity(d))
      return;
    this._textBlockMove.Text = d < 0.0 ? (this.SelectedTime - TimeSpan.FromMilliseconds(-d)).ToString(this.TimeFormat) : (this.SelectedTime + TimeSpan.FromMilliseconds(d)).ToString(this.TimeFormat);
    this._textBlockMove.Margin = new Thickness(position.X - this._textBlockMove.ActualWidth / 2.0, 2.0, 0.0, 0.0);
  }

  private void UpdateHotspots()
  {
    double num = this.ActualWidth * 0.5 / this._itemWidth * (double) this._timeSpeList[this._speIndex];
    if (double.IsNaN(num) || double.IsInfinity(num))
      return;
    this._panelHotspots.Children.Clear();
    foreach (UIElement element in this.GetHotspotsRectangle(num))
      this._panelHotspots.Children.Add(element);
  }

  private void BorderTop_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    this._borderTopIsMouseLeftButtonDown = true;
  }

  private void BorderTop_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    if (!this._borderTopIsMouseLeftButtonDown)
      return;
    this._borderTopIsMouseLeftButtonDown = false;
    this._tempOffsetX = this.ActualWidth / 2.0 - Mouse.GetPosition((IInputElement) this).X;
    this.SelectedTime -= TimeSpan.FromMilliseconds(this._tempOffsetX / this._itemWidth * (double) this._timeSpeList[this._speIndex]);
    this._totalOffsetX = (this._totalOffsetX + this._tempOffsetX) % this.ActualWidth;
    this._tempOffsetX = 0.0;
    this.UpdateMouseFollowBlockPos();
    this.RaiseEvent((RoutedEventArgs) new FunctionEventArgs<DateTime>(TimeBar.TimeChangedEvent, (object) this)
    {
      Info = this.SelectedTime
    });
  }

  private IEnumerable<Rectangle> GetHotspotsRectangle(double mlliseconds)
  {
    TimeBar timeBar = this;
    TimeSpan timeSpan = TimeSpan.FromMilliseconds(mlliseconds);
    DateTime selectedTime = timeBar.SelectedTime;
    DateTime start = selectedTime - timeSpan;
    DateTime start1 = selectedTime + timeSpan;
    SortedSet<DateTimeRange> viewBetween = timeBar._dateTimeRanges.GetViewBetween(new DateTimeRange(start), new DateTimeRange(start1));
    double unitLength = timeBar.ActualWidth / mlliseconds * 0.5;
    foreach (DateTimeRange dateTimeRange in viewBetween)
    {
      double num = dateTimeRange.TotalMilliseconds * unitLength;
      double left = (dateTimeRange.Start - start).TotalMilliseconds * unitLength;
      Rectangle rectangle = new Rectangle();
      rectangle.Fill = timeBar.HotspotsBrush;
      rectangle.Height = 4.0;
      rectangle.Width = num;
      rectangle.Margin = new Thickness(left, 0.0, 0.0, 0.0);
      rectangle.HorizontalAlignment = HorizontalAlignment.Left;
      yield return rectangle;
    }
  }
}
