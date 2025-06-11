// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.Growl
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Properties.Langs;
using HandyControl.Tools;
using HandyControl.Tools.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

#nullable disable
namespace HandyControl.Controls;

[TemplatePart(Name = "PART_PanelMore", Type = typeof (Panel))]
[TemplatePart(Name = "PART_GridMain", Type = typeof (Grid))]
[TemplatePart(Name = "PART_ButtonClose", Type = typeof (Button))]
public class Growl : Control
{
  private const string ElementPanelMore = "PART_PanelMore";
  private const string ElementGridMain = "PART_GridMain";
  private const string ElementButtonClose = "PART_ButtonClose";
  private const int MinWaitTime = 2;
  public static readonly DependencyProperty GrowlParentProperty = DependencyProperty.RegisterAttached("GrowlParent", typeof (bool), typeof (Growl), new PropertyMetadata(ValueBoxes.FalseBox, (PropertyChangedCallback) ((o, args) =>
  {
    if (!(bool) args.NewValue || !(o is Panel panel2))
      return;
    Growl.SetGrowlPanel(panel2);
  })));
  public static readonly DependencyProperty ShowModeProperty = DependencyProperty.RegisterAttached("ShowMode", typeof (GrowlShowMode), typeof (Growl), (PropertyMetadata) new FrameworkPropertyMetadata((object) GrowlShowMode.Prepend, FrameworkPropertyMetadataOptions.Inherits));
  public static readonly DependencyProperty ShowDateTimeProperty = DependencyProperty.Register(nameof (ShowDateTime), typeof (bool), typeof (Growl), new PropertyMetadata(ValueBoxes.TrueBox));
  public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(nameof (Message), typeof (string), typeof (Growl), new PropertyMetadata((object) null));
  public static readonly DependencyProperty TimeProperty = DependencyProperty.Register(nameof (Time), typeof (DateTime), typeof (Growl), new PropertyMetadata((object) new DateTime()));
  public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof (Icon), typeof (Geometry), typeof (Growl), new PropertyMetadata((object) null));
  public static readonly DependencyProperty IconBrushProperty = DependencyProperty.Register(nameof (IconBrush), typeof (Brush), typeof (Growl), new PropertyMetadata((object) null));
  public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(nameof (Type), typeof (InfoType), typeof (Growl), new PropertyMetadata((object) InfoType.Success));
  public static readonly DependencyProperty TokenProperty = DependencyProperty.RegisterAttached("Token", typeof (string), typeof (Growl), new PropertyMetadata((object) null, new PropertyChangedCallback(Growl.OnTokenChanged)));
  internal static readonly DependencyProperty CancelStrProperty = DependencyProperty.Register(nameof (CancelStr), typeof (string), typeof (Growl), new PropertyMetadata((object) null));
  internal static readonly DependencyProperty ConfirmStrProperty = DependencyProperty.Register(nameof (ConfirmStr), typeof (string), typeof (Growl), new PropertyMetadata((object) null));
  private static readonly DependencyProperty IsCreatedAutomaticallyProperty = DependencyProperty.RegisterAttached("IsCreatedAutomatically", typeof (bool), typeof (Growl), new PropertyMetadata(ValueBoxes.FalseBox));
  private static GrowlWindow GrowlWindow;
  private static readonly Dictionary<string, Panel> PanelDic = new Dictionary<string, Panel>();
  private Panel _panelMore;
  private Grid _gridMain;
  private Button _buttonClose;
  private bool _showCloseButton;
  private bool _staysOpen;
  private int _waitTime = 6;
  private int _tickCount;
  private DispatcherTimer _timerClose;

  public static Panel GrowlPanel { get; set; }

  public InfoType Type
  {
    get => (InfoType) this.GetValue(Growl.TypeProperty);
    set => this.SetValue(Growl.TypeProperty, (object) value);
  }

  public bool ShowDateTime
  {
    get => (bool) this.GetValue(Growl.ShowDateTimeProperty);
    set => this.SetValue(Growl.ShowDateTimeProperty, ValueBoxes.BooleanBox(value));
  }

  public string Message
  {
    get => (string) this.GetValue(Growl.MessageProperty);
    set => this.SetValue(Growl.MessageProperty, (object) value);
  }

  public DateTime Time
  {
    get => (DateTime) this.GetValue(Growl.TimeProperty);
    set => this.SetValue(Growl.TimeProperty, (object) value);
  }

  public Geometry Icon
  {
    get => (Geometry) this.GetValue(Growl.IconProperty);
    set => this.SetValue(Growl.IconProperty, (object) value);
  }

  public Brush IconBrush
  {
    get => (Brush) this.GetValue(Growl.IconBrushProperty);
    set => this.SetValue(Growl.IconBrushProperty, (object) value);
  }

  internal string CancelStr
  {
    get => (string) this.GetValue(Growl.CancelStrProperty);
    set => this.SetValue(Growl.CancelStrProperty, (object) value);
  }

  internal string ConfirmStr
  {
    get => (string) this.GetValue(Growl.ConfirmStrProperty);
    set => this.SetValue(Growl.ConfirmStrProperty, (object) value);
  }

  private Func<bool, bool> ActionBeforeClose { get; set; }

  public Growl()
  {
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Close, new ExecutedRoutedEventHandler(this.ButtonClose_OnClick)));
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Cancel, new ExecutedRoutedEventHandler(this.ButtonCancel_OnClick)));
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Confirm, new ExecutedRoutedEventHandler(this.ButtonOk_OnClick)));
  }

  public static void Register(string token, Panel panel)
  {
    if (string.IsNullOrEmpty(token) || panel == null)
      return;
    Growl.PanelDic[token] = panel;
    Growl.InitGrowlPanel(panel);
  }

  public static void Unregister(string token, Panel panel)
  {
    if (string.IsNullOrEmpty(token) || panel == null || !Growl.PanelDic.ContainsKey(token) || Growl.PanelDic[token] != panel)
      return;
    Growl.PanelDic.Remove(token);
    panel.ContextMenu = (ContextMenu) null;
    panel.SetCurrentValue(PanelElement.FluidMoveBehaviorProperty, DependencyProperty.UnsetValue);
  }

  public static void Unregister(Panel panel)
  {
    if (panel == null)
      return;
    KeyValuePair<string, Panel> keyValuePair = Growl.PanelDic.FirstOrDefault<KeyValuePair<string, Panel>>((Func<KeyValuePair<string, Panel>, bool>) (item => panel == item.Value));
    if (string.IsNullOrEmpty(keyValuePair.Key))
      return;
    Growl.PanelDic.Remove(keyValuePair.Key);
    panel.ContextMenu = (ContextMenu) null;
    panel.SetCurrentValue(PanelElement.FluidMoveBehaviorProperty, DependencyProperty.UnsetValue);
  }

  public static void Unregister(string token)
  {
    if (string.IsNullOrEmpty(token) || !Growl.PanelDic.ContainsKey(token))
      return;
    Panel panel = Growl.PanelDic[token];
    Growl.PanelDic.Remove(token);
    panel.ContextMenu = (ContextMenu) null;
    panel.SetCurrentValue(PanelElement.FluidMoveBehaviorProperty, DependencyProperty.UnsetValue);
  }

  protected override void OnMouseEnter(MouseEventArgs e)
  {
    base.OnMouseEnter(e);
    this._buttonClose.Show(this._showCloseButton);
  }

  protected override void OnMouseLeave(MouseEventArgs e)
  {
    base.OnMouseLeave(e);
    this._buttonClose.Collapse();
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this._panelMore = this.GetTemplateChild("PART_PanelMore") as Panel;
    this._gridMain = this.GetTemplateChild("PART_GridMain") as Grid;
    this._buttonClose = this.GetTemplateChild("PART_ButtonClose") as Button;
    this.CheckNull();
    this.Update();
  }

  private void CheckNull()
  {
    if (this._panelMore == null || this._gridMain == null || this._buttonClose == null)
      throw new Exception();
  }

  private static void OnTokenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is Panel panel))
      return;
    if (e.NewValue == null)
      Growl.Unregister(panel);
    else
      Growl.Register(e.NewValue.ToString(), panel);
  }

  public static void SetToken(DependencyObject element, string value)
  {
    element.SetValue(Growl.TokenProperty, (object) value);
  }

  public static string GetToken(DependencyObject element)
  {
    return (string) element.GetValue(Growl.TokenProperty);
  }

  public static void SetShowMode(DependencyObject element, GrowlShowMode value)
  {
    element.SetValue(Growl.ShowModeProperty, (object) value);
  }

  public static GrowlShowMode GetShowMode(DependencyObject element)
  {
    return (GrowlShowMode) element.GetValue(Growl.ShowModeProperty);
  }

  public static void SetGrowlParent(DependencyObject element, bool value)
  {
    element.SetValue(Growl.GrowlParentProperty, ValueBoxes.BooleanBox(value));
  }

  public static bool GetGrowlParent(DependencyObject element)
  {
    return (bool) element.GetValue(Growl.GrowlParentProperty);
  }

  private static void SetIsCreatedAutomatically(DependencyObject element, bool value)
  {
    element.SetValue(Growl.IsCreatedAutomaticallyProperty, ValueBoxes.BooleanBox(value));
  }

  private static bool GetIsCreatedAutomatically(DependencyObject element)
  {
    return (bool) element.GetValue(Growl.IsCreatedAutomaticallyProperty);
  }

  private void StartTimer()
  {
    this._timerClose = new DispatcherTimer()
    {
      Interval = TimeSpan.FromSeconds(1.0)
    };
    this._timerClose.Tick += (EventHandler) ((_param1, _param2) =>
    {
      if (this.IsMouseOver)
      {
        this._tickCount = 0;
      }
      else
      {
        ++this._tickCount;
        if (this._tickCount < this._waitTime)
          return;
        this.Close(true);
      }
    });
    this._timerClose.Start();
  }

  private static void SetGrowlPanel(Panel panel)
  {
    Growl.GrowlPanel = panel;
    Growl.InitGrowlPanel(panel);
  }

  private static void InitGrowlPanel(Panel panel)
  {
    if (panel == null)
      return;
    MenuItem newItem = new MenuItem();
    LangProvider.SetLang((DependencyObject) newItem, HeaderedItemsControl.HeaderProperty, LangKeys.Clear);
    newItem.Click += (RoutedEventHandler) ((s, e) =>
    {
      foreach (Growl growl in panel.Children.OfType<Growl>())
        growl.Close(false);
    });
    Panel panel1 = panel;
    ContextMenu contextMenu = new ContextMenu();
    contextMenu.Items.Add((object) newItem);
    panel1.ContextMenu = contextMenu;
    PanelElement.SetFluidMoveBehavior((DependencyObject) panel, HandyControl.Tools.ResourceHelper.GetResourceInternal<FluidMoveBehavior>("BehaviorXY400"));
  }

  private void Update()
  {
    if (DesignerHelper.IsInDesignMode)
      return;
    if (this.Type == InfoType.Ask)
    {
      this._panelMore.IsEnabled = true;
      this._panelMore.Show();
    }
    TranslateTransform translateTransform = new TranslateTransform()
    {
      X = this.FlowDirection == FlowDirection.LeftToRight ? this.MaxWidth : -this.MaxWidth
    };
    this._gridMain.RenderTransform = (Transform) translateTransform;
    translateTransform.BeginAnimation(TranslateTransform.XProperty, (AnimationTimeline) AnimationHelper.CreateAnimation(0.0));
    if (this._staysOpen)
      return;
    this.StartTimer();
  }

  private static void ShowInternal(Panel panel, UIElement growl)
  {
    if (panel == null)
      return;
    if (Growl.GetShowMode((DependencyObject) panel) == GrowlShowMode.Prepend)
      panel.Children.Insert(0, growl);
    else
      panel.Children.Add(growl);
  }

  private static void ShowGlobal(GrowlInfo growlInfo)
  {
    Application.Current.Dispatcher?.Invoke((Action) (() =>
    {
      if (Growl.GrowlWindow == null)
      {
        Growl.GrowlWindow = new GrowlWindow();
        Growl.GrowlWindow.Show();
        Growl.InitGrowlPanel(Growl.GrowlWindow.GrowlPanel);
        Growl.GrowlWindow.Init();
      }
      Growl.GrowlWindow.Show(true);
      Growl growl = new Growl()
      {
        Message = growlInfo.Message,
        Time = DateTime.Now,
        Icon = HandyControl.Tools.ResourceHelper.GetResource<Geometry>(growlInfo.IconKey) ?? growlInfo.Icon,
        IconBrush = HandyControl.Tools.ResourceHelper.GetResource<Brush>(growlInfo.IconBrushKey) ?? growlInfo.IconBrush,
        _showCloseButton = growlInfo.ShowCloseButton,
        ActionBeforeClose = growlInfo.ActionBeforeClose,
        _staysOpen = growlInfo.StaysOpen,
        ShowDateTime = growlInfo.ShowDateTime,
        ConfirmStr = growlInfo.ConfirmStr,
        CancelStr = growlInfo.CancelStr,
        Type = growlInfo.Type,
        _waitTime = Math.Max(growlInfo.WaitTime, 2),
        FlowDirection = growlInfo.FlowDirection
      };
      Growl.ShowInternal(Growl.GrowlWindow.GrowlPanel, (UIElement) growl);
    }));
  }

  private static void Show(GrowlInfo growlInfo)
  {
    (Application.Current.Dispatcher ?? growlInfo.Dispatcher)?.Invoke((Action) (() =>
    {
      Growl growl = new Growl()
      {
        Message = growlInfo.Message,
        Time = DateTime.Now,
        Icon = HandyControl.Tools.ResourceHelper.GetResource<Geometry>(growlInfo.IconKey) ?? growlInfo.Icon,
        IconBrush = HandyControl.Tools.ResourceHelper.GetResource<Brush>(growlInfo.IconBrushKey) ?? growlInfo.IconBrush,
        _showCloseButton = growlInfo.ShowCloseButton,
        ActionBeforeClose = growlInfo.ActionBeforeClose,
        _staysOpen = growlInfo.StaysOpen,
        ShowDateTime = growlInfo.ShowDateTime,
        ConfirmStr = growlInfo.ConfirmStr,
        CancelStr = growlInfo.CancelStr,
        Type = growlInfo.Type,
        _waitTime = Math.Max(growlInfo.WaitTime, 2)
      };
      if (!string.IsNullOrEmpty(growlInfo.Token))
      {
        Panel panel;
        if (!Growl.PanelDic.TryGetValue(growlInfo.Token, out panel))
          return;
        Growl.ShowInternal(panel, (UIElement) growl);
      }
      else
      {
        if (Growl.GrowlPanel == null)
          Growl.GrowlPanel = Growl.CreateDefaultPanel();
        Growl.ShowInternal(Growl.GrowlPanel, (UIElement) growl);
      }
    }));
  }

  private static Panel CreateDefaultPanel()
  {
    AdornerDecorator child = VisualHelper.GetChild<AdornerDecorator>((DependencyObject) WindowHelper.GetActiveWindow());
    if (child != null)
    {
      AdornerLayer adornerLayer = child.AdornerLayer;
      if (adornerLayer != null)
      {
        StackPanel stackPanel = new StackPanel();
        stackPanel.VerticalAlignment = VerticalAlignment.Top;
        StackPanel element = stackPanel;
        Growl.InitGrowlPanel((Panel) element);
        Growl.SetIsCreatedAutomatically((DependencyObject) element, true);
        ScrollViewer scrollViewer1 = new ScrollViewer();
        scrollViewer1.HorizontalAlignment = HorizontalAlignment.Right;
        scrollViewer1.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
        scrollViewer1.IsInertiaEnabled = true;
        scrollViewer1.IsPenetrating = true;
        scrollViewer1.Content = (object) element;
        ScrollViewer scrollViewer2 = scrollViewer1;
        AdornerContainer adornerContainer = new AdornerContainer((UIElement) adornerLayer)
        {
          Child = (UIElement) scrollViewer2
        };
        adornerLayer.Add((Adorner) adornerContainer);
        return (Panel) element;
      }
    }
    return (Panel) null;
  }

  private static void RemoveDefaultPanel(Panel panel)
  {
    AdornerDecorator child = VisualHelper.GetChild<AdornerDecorator>((DependencyObject) WindowHelper.GetActiveWindow());
    if (child == null)
      return;
    AdornerLayer adornerLayer = child.AdornerLayer;
    Adorner parent = VisualHelper.GetParent<Adorner>((DependencyObject) panel);
    if (parent == null || adornerLayer == null)
      return;
    adornerLayer.Remove(parent);
  }

  private static void InitGrowlInfo(ref GrowlInfo growlInfo, InfoType infoType)
  {
    if (growlInfo == null)
      throw new ArgumentNullException(nameof (growlInfo));
    growlInfo.Type = infoType;
    string str;
    switch (infoType)
    {
      case InfoType.Success:
        if (!growlInfo.IsCustom)
        {
          growlInfo.IconKey = "SuccessGeometry";
          growlInfo.IconBrushKey = "SuccessBrush";
          break;
        }
        GrowlInfo growlInfo1 = growlInfo;
        if (growlInfo1.IconKey == null)
          growlInfo1.IconKey = str = "SuccessGeometry";
        GrowlInfo growlInfo2 = growlInfo;
        if (growlInfo2.IconBrushKey != null)
          break;
        growlInfo2.IconBrushKey = str = "SuccessBrush";
        break;
      case InfoType.Info:
        if (!growlInfo.IsCustom)
        {
          growlInfo.IconKey = "InfoGeometry";
          growlInfo.IconBrushKey = "InfoBrush";
          break;
        }
        GrowlInfo growlInfo3 = growlInfo;
        if (growlInfo3.IconKey == null)
          growlInfo3.IconKey = str = "InfoGeometry";
        GrowlInfo growlInfo4 = growlInfo;
        if (growlInfo4.IconBrushKey != null)
          break;
        growlInfo4.IconBrushKey = str = "InfoBrush";
        break;
      case InfoType.Warning:
        if (!growlInfo.IsCustom)
        {
          growlInfo.IconKey = "WarningGeometry";
          growlInfo.IconBrushKey = "WarningBrush";
          break;
        }
        GrowlInfo growlInfo5 = growlInfo;
        if (growlInfo5.IconKey == null)
          growlInfo5.IconKey = str = "WarningGeometry";
        GrowlInfo growlInfo6 = growlInfo;
        if (growlInfo6.IconBrushKey != null)
          break;
        growlInfo6.IconBrushKey = str = "WarningBrush";
        break;
      case InfoType.Error:
        if (!growlInfo.IsCustom)
        {
          growlInfo.IconKey = "ErrorGeometry";
          growlInfo.IconBrushKey = "DangerBrush";
          growlInfo.StaysOpen = true;
          break;
        }
        GrowlInfo growlInfo7 = growlInfo;
        if (growlInfo7.IconKey == null)
          growlInfo7.IconKey = str = "ErrorGeometry";
        GrowlInfo growlInfo8 = growlInfo;
        if (growlInfo8.IconBrushKey != null)
          break;
        growlInfo8.IconBrushKey = str = "DangerBrush";
        break;
      case InfoType.Fatal:
        if (!growlInfo.IsCustom)
        {
          growlInfo.IconKey = "FatalGeometry";
          growlInfo.IconBrushKey = "PrimaryTextBrush";
          growlInfo.StaysOpen = true;
          growlInfo.ShowCloseButton = false;
          break;
        }
        GrowlInfo growlInfo9 = growlInfo;
        if (growlInfo9.IconKey == null)
          growlInfo9.IconKey = str = "FatalGeometry";
        GrowlInfo growlInfo10 = growlInfo;
        if (growlInfo10.IconBrushKey != null)
          break;
        growlInfo10.IconBrushKey = str = "PrimaryTextBrush";
        break;
      case InfoType.Ask:
        growlInfo.StaysOpen = true;
        growlInfo.ShowCloseButton = false;
        if (!growlInfo.IsCustom)
        {
          growlInfo.IconKey = "AskGeometry";
          growlInfo.IconBrushKey = "AccentBrush";
          break;
        }
        GrowlInfo growlInfo11 = growlInfo;
        if (growlInfo11.IconKey == null)
          growlInfo11.IconKey = str = "AskGeometry";
        GrowlInfo growlInfo12 = growlInfo;
        if (growlInfo12.IconBrushKey != null)
          break;
        growlInfo12.IconBrushKey = str = "AccentBrush";
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof (infoType), (object) infoType, (string) null);
    }
  }

  public static void Success(string message, string token = "")
  {
    Growl.Success(new GrowlInfo()
    {
      Message = message,
      Token = token
    });
  }

  public static void Success(GrowlInfo growlInfo)
  {
    Growl.InitGrowlInfo(ref growlInfo, InfoType.Success);
    Growl.Show(growlInfo);
  }

  public static void SuccessGlobal(string message)
  {
    Growl.SuccessGlobal(new GrowlInfo()
    {
      Message = message
    });
  }

  public static void SuccessGlobal(GrowlInfo growlInfo)
  {
    Growl.InitGrowlInfo(ref growlInfo, InfoType.Success);
    Growl.ShowGlobal(growlInfo);
  }

  public static void Info(string message, string token = "")
  {
    Growl.Info(new GrowlInfo()
    {
      Message = message,
      Token = token
    });
  }

  public static void Info(GrowlInfo growlInfo)
  {
    Growl.InitGrowlInfo(ref growlInfo, InfoType.Info);
    Growl.Show(growlInfo);
  }

  public static void InfoGlobal(string message)
  {
    Growl.InfoGlobal(new GrowlInfo() { Message = message });
  }

  public static void InfoGlobal(GrowlInfo growlInfo)
  {
    Growl.InitGrowlInfo(ref growlInfo, InfoType.Info);
    Growl.ShowGlobal(growlInfo);
  }

  public static void Warning(string message, string token = "")
  {
    Growl.Warning(new GrowlInfo()
    {
      Message = message,
      Token = token
    });
  }

  public static void Warning(GrowlInfo growlInfo)
  {
    Growl.InitGrowlInfo(ref growlInfo, InfoType.Warning);
    Growl.Show(growlInfo);
  }

  public static void WarningGlobal(string message)
  {
    Growl.WarningGlobal(new GrowlInfo()
    {
      Message = message
    });
  }

  public static void WarningGlobal(GrowlInfo growlInfo)
  {
    Growl.InitGrowlInfo(ref growlInfo, InfoType.Warning);
    Growl.ShowGlobal(growlInfo);
  }

  public static void Error(string message, string token = "")
  {
    Growl.Error(new GrowlInfo()
    {
      Message = message,
      Token = token
    });
  }

  public static void Error(GrowlInfo growlInfo)
  {
    Growl.InitGrowlInfo(ref growlInfo, InfoType.Error);
    Growl.Show(growlInfo);
  }

  public static void ErrorGlobal(string message)
  {
    Growl.ErrorGlobal(new GrowlInfo() { Message = message });
  }

  public static void ErrorGlobal(GrowlInfo growlInfo)
  {
    Growl.InitGrowlInfo(ref growlInfo, InfoType.Error);
    Growl.ShowGlobal(growlInfo);
  }

  public static void Fatal(string message, string token = "")
  {
    Growl.Fatal(new GrowlInfo()
    {
      Message = message,
      Token = token
    });
  }

  public static void Fatal(GrowlInfo growlInfo)
  {
    Growl.InitGrowlInfo(ref growlInfo, InfoType.Fatal);
    Growl.Show(growlInfo);
  }

  public static void FatalGlobal(string message)
  {
    Growl.FatalGlobal(new GrowlInfo() { Message = message });
  }

  public static void FatalGlobal(GrowlInfo growlInfo)
  {
    Growl.InitGrowlInfo(ref growlInfo, InfoType.Fatal);
    Growl.ShowGlobal(growlInfo);
  }

  public static void Ask(string message, Func<bool, bool> actionBeforeClose, string token = "")
  {
    Growl.Ask(new GrowlInfo()
    {
      Message = message,
      ActionBeforeClose = actionBeforeClose,
      Token = token
    });
  }

  public static void Ask(GrowlInfo growlInfo)
  {
    Growl.InitGrowlInfo(ref growlInfo, InfoType.Ask);
    Growl.Show(growlInfo);
  }

  public static void AskGlobal(string message, Func<bool, bool> actionBeforeClose)
  {
    Growl.AskGlobal(new GrowlInfo()
    {
      Message = message,
      ActionBeforeClose = actionBeforeClose
    });
  }

  public static void AskGlobal(GrowlInfo growlInfo)
  {
    Growl.InitGrowlInfo(ref growlInfo, InfoType.Ask);
    Growl.ShowGlobal(growlInfo);
  }

  private void ButtonClose_OnClick(object sender, RoutedEventArgs e) => this.Close(false);

  private void Close(bool invokeParam)
  {
    Func<bool, bool> actionBeforeClose = this.ActionBeforeClose;
    if ((actionBeforeClose != null ? (!actionBeforeClose(invokeParam) ? 1 : 0) : 0) != 0)
      return;
    this._timerClose?.Stop();
    TranslateTransform translateTransform = new TranslateTransform();
    this._gridMain.RenderTransform = (Transform) translateTransform;
    DoubleAnimation animation = AnimationHelper.CreateAnimation(this.FlowDirection == FlowDirection.LeftToRight ? this.ActualWidth : -this.ActualWidth);
    animation.Completed += (EventHandler) ((s, e) =>
    {
      if (!(this.Parent is Panel parent2))
        return;
      parent2.Children.Remove((UIElement) this);
      if (Growl.GrowlWindow != null)
      {
        if (Growl.GrowlWindow.GrowlPanel == null || Growl.GrowlWindow.GrowlPanel.Children.Count != 0)
          return;
        Growl.GrowlWindow.Close();
        Growl.GrowlWindow = (GrowlWindow) null;
      }
      else
      {
        if (Growl.GrowlPanel == null || Growl.GrowlPanel.Children.Count != 0 || !Growl.GetIsCreatedAutomatically((DependencyObject) Growl.GrowlPanel))
          return;
        Growl.RemoveDefaultPanel(Growl.GrowlPanel);
        Growl.GrowlPanel = (Panel) null;
      }
    });
    translateTransform.BeginAnimation(TranslateTransform.XProperty, (AnimationTimeline) animation);
  }

  public static void Clear(string token = "")
  {
    if (!string.IsNullOrEmpty(token))
    {
      Panel panel;
      if (!Growl.PanelDic.TryGetValue(token, out panel))
        return;
      Growl.Clear(panel);
    }
    else
      Growl.Clear(Growl.GrowlPanel);
  }

  private static void Clear(Panel panel) => panel?.Children.Clear();

  public static void ClearGlobal()
  {
    if (Growl.GrowlWindow == null)
      return;
    Growl.Clear(Growl.GrowlWindow.GrowlPanel);
    Growl.GrowlWindow.Close();
    Growl.GrowlWindow = (GrowlWindow) null;
  }

  private void ButtonCancel_OnClick(object sender, RoutedEventArgs e) => this.Close(false);

  private void ButtonOk_OnClick(object sender, RoutedEventArgs e) => this.Close(true);
}
