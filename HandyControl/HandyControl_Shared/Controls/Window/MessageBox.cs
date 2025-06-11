// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.MessageBox
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Properties.Langs;
using HandyControl.Tools;
using HandyControl.Tools.Interop;
using System;
using System.ComponentModel;
using System.Media;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Controls;

[TemplatePart(Name = "PART_Panel", Type = typeof (Panel))]
[TemplatePart(Name = "PART_ButtonClose", Type = typeof (Button))]
public sealed class MessageBox : Window
{
  private const string ElementPanel = "PART_Panel";
  private const string ElementButtonClose = "PART_ButtonClose";
  private Button _buttonClose;
  private Panel _panel;
  private MessageBoxResult _messageBoxResult = MessageBoxResult.Cancel;
  private Button _buttonOk;
  private Button _buttonCancel;
  private Button _buttonYes;
  private Button _buttonNo;
  private bool _showOk;
  private bool _showCancel;
  private bool _showYes;
  private bool _showNo;
  private IntPtr _lastActiveWindowIntPtr;
  public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(nameof (Message), typeof (string), typeof (MessageBox), new PropertyMetadata((object) null));
  public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(nameof (Image), typeof (Geometry), typeof (MessageBox), new PropertyMetadata((object) null));
  public static readonly DependencyProperty ImageBrushProperty = DependencyProperty.Register(nameof (ImageBrush), typeof (Brush), typeof (MessageBox), new PropertyMetadata((object) null));
  public static readonly DependencyProperty ShowImageProperty = DependencyProperty.Register(nameof (ShowImage), typeof (bool), typeof (MessageBox), new PropertyMetadata(ValueBoxes.FalseBox));

  public string Message
  {
    get => (string) this.GetValue(MessageBox.MessageProperty);
    set => this.SetValue(MessageBox.MessageProperty, (object) value);
  }

  public Geometry Image
  {
    get => (Geometry) this.GetValue(MessageBox.ImageProperty);
    set => this.SetValue(MessageBox.ImageProperty, (object) value);
  }

  public Brush ImageBrush
  {
    get => (Brush) this.GetValue(MessageBox.ImageBrushProperty);
    set => this.SetValue(MessageBox.ImageBrushProperty, (object) value);
  }

  public bool ShowImage
  {
    get => (bool) this.GetValue(MessageBox.ShowImageProperty);
    set => this.SetValue(MessageBox.ShowImageProperty, ValueBoxes.BooleanBox(value));
  }

  private MessageBox()
  {
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Confirm, (ExecutedRoutedEventHandler) ((s, e) =>
    {
      this._messageBoxResult = MessageBoxResult.OK;
      this.Close();
    }), (CanExecuteRoutedEventHandler) ((s, e) => e.CanExecute = this._showOk)));
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Cancel, (ExecutedRoutedEventHandler) ((s, e) =>
    {
      this._messageBoxResult = MessageBoxResult.Cancel;
      this.Close();
    }), (CanExecuteRoutedEventHandler) ((s, e) => e.CanExecute = this._showCancel)));
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Yes, (ExecutedRoutedEventHandler) ((s, e) =>
    {
      this._messageBoxResult = MessageBoxResult.Yes;
      this.Close();
    }), (CanExecuteRoutedEventHandler) ((s, e) => e.CanExecute = this._showYes)));
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.No, (ExecutedRoutedEventHandler) ((s, e) =>
    {
      this._messageBoxResult = MessageBoxResult.No;
      this.Close();
    }), (CanExecuteRoutedEventHandler) ((s, e) => e.CanExecute = this._showNo)));
  }

  protected override void OnSourceInitialized(EventArgs e)
  {
    if (this._showYes && !this._showCancel)
    {
      IntPtr systemMenu = InteropMethods.GetSystemMenu(WindowHelper.GetHandle(this), false);
      if (systemMenu != IntPtr.Zero)
        InteropMethods.EnableMenuItem(systemMenu, 61536, 1);
      if (this._buttonClose != null)
        this._buttonClose.IsEnabled = false;
    }
    base.OnSourceInitialized(e);
    this._lastActiveWindowIntPtr = InteropMethods.GetForegroundWindow();
    this.Activate();
  }

  protected override void OnClosed(EventArgs e)
  {
    InteropMethods.SetForegroundWindow(this._lastActiveWindowIntPtr);
    base.OnClosed(e);
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this._panel = this.GetTemplateChild("PART_Panel") as Panel;
    if (this._panel != null)
    {
      if (this._buttonOk != null)
        this._panel.Children.Add((UIElement) this._buttonOk);
      if (this._buttonYes != null)
        this._panel.Children.Add((UIElement) this._buttonYes);
      if (this._buttonNo != null)
        this._panel.Children.Add((UIElement) this._buttonNo);
      if (this._buttonCancel != null)
        this._panel.Children.Add((UIElement) this._buttonCancel);
    }
    this._buttonClose = this.GetTemplateChild("PART_ButtonClose") as Button;
    if (this._buttonClose == null)
      return;
    this._buttonClose.Click += new RoutedEventHandler(this.ButtonClose_Click);
  }

  private void ButtonClose_Click(object sender, RoutedEventArgs e) => this.Close();

  protected override void OnPreviewKeyDown(KeyEventArgs e)
  {
    if (e.Key == Key.System && e.SystemKey == Key.F4)
    {
      e.Handled = true;
    }
    else
    {
      if (e.KeyboardDevice.Modifiers != ModifierKeys.Control || e.Key != Key.C)
        return;
      StringBuilder stringBuilder = new StringBuilder();
      string str = new string('-', 27);
      stringBuilder.Append(str);
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append(this.Title);
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append(str);
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append(this.Message);
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append(str);
      stringBuilder.Append(Environment.NewLine);
      if (this._showOk)
      {
        stringBuilder.Append(Lang.Confirm);
        stringBuilder.Append("   ");
      }
      if (this._showYes)
      {
        stringBuilder.Append(Lang.Yes);
        stringBuilder.Append("   ");
      }
      if (this._showNo)
      {
        stringBuilder.Append(Lang.No);
        stringBuilder.Append("   ");
      }
      if (this._showCancel)
      {
        stringBuilder.Append(Lang.Cancel);
        stringBuilder.Append("   ");
      }
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append(str);
      stringBuilder.Append(Environment.NewLine);
      try
      {
        Clipboard.SetDataObject((object) stringBuilder.ToString());
      }
      catch
      {
      }
    }
  }

  public static MessageBoxResult Success(string messageBoxText, string caption = null)
  {
    MessageBox messageBox = (MessageBox) null;
    Application.Current.Dispatcher.Invoke((Action) (() =>
    {
      messageBox = MessageBox.CreateMessageBox((System.Windows.Window) null, messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK);
      MessageBox.SetButtonStatus(messageBox, MessageBoxButton.OK, MessageBoxResult.OK);
      messageBox.ShowImage = true;
      messageBox.Image = HandyControl.Tools.ResourceHelper.GetResourceInternal<Geometry>("SuccessGeometry");
      messageBox.ImageBrush = HandyControl.Tools.ResourceHelper.GetResourceInternal<Brush>("SuccessBrush");
      SystemSounds.Asterisk.Play();
      messageBox.ShowDialog();
    }));
    return messageBox._messageBoxResult;
  }

  public static MessageBoxResult Info(string messageBoxText, string caption = null)
  {
    MessageBox messageBox = (MessageBox) null;
    Application.Current.Dispatcher.Invoke((Action) (() =>
    {
      messageBox = MessageBox.CreateMessageBox((System.Windows.Window) null, messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.OK);
      MessageBox.SetButtonStatus(messageBox, MessageBoxButton.OK, MessageBoxResult.OK);
      MessageBox.SetImage(messageBox, MessageBoxImage.Asterisk);
      SystemSounds.Asterisk.Play();
      messageBox.ShowDialog();
    }));
    return messageBox._messageBoxResult;
  }

  public static MessageBoxResult Warning(string messageBoxText, string caption = null)
  {
    MessageBox messageBox = (MessageBox) null;
    Application.Current.Dispatcher.Invoke((Action) (() =>
    {
      messageBox = MessageBox.CreateMessageBox((System.Windows.Window) null, messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK);
      MessageBox.SetButtonStatus(messageBox, MessageBoxButton.OK, MessageBoxResult.OK);
      MessageBox.SetImage(messageBox, MessageBoxImage.Exclamation);
      SystemSounds.Asterisk.Play();
      messageBox.ShowDialog();
    }));
    return messageBox._messageBoxResult;
  }

  public static MessageBoxResult Error(string messageBoxText, string caption = null)
  {
    MessageBox messageBox = (MessageBox) null;
    Application.Current.Dispatcher.Invoke((Action) (() =>
    {
      messageBox = MessageBox.CreateMessageBox((System.Windows.Window) null, messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.Hand, MessageBoxResult.OK);
      MessageBox.SetButtonStatus(messageBox, MessageBoxButton.OK, MessageBoxResult.OK);
      MessageBox.SetImage(messageBox, MessageBoxImage.Hand);
      SystemSounds.Asterisk.Play();
      messageBox.ShowDialog();
    }));
    return messageBox._messageBoxResult;
  }

  public static MessageBoxResult Fatal(string messageBoxText, string caption = null)
  {
    MessageBox messageBox = (MessageBox) null;
    Application.Current.Dispatcher.Invoke((Action) (() =>
    {
      messageBox = MessageBox.CreateMessageBox((System.Windows.Window) null, messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK);
      MessageBox.SetButtonStatus(messageBox, MessageBoxButton.OK, MessageBoxResult.OK);
      messageBox.ShowImage = true;
      messageBox.Image = HandyControl.Tools.ResourceHelper.GetResourceInternal<Geometry>("FatalGeometry");
      messageBox.ImageBrush = HandyControl.Tools.ResourceHelper.GetResourceInternal<Brush>("PrimaryTextBrush");
      SystemSounds.Asterisk.Play();
      messageBox.ShowDialog();
    }));
    return messageBox._messageBoxResult;
  }

  public static MessageBoxResult Ask(string messageBoxText, string caption = null)
  {
    MessageBox messageBox = (MessageBox) null;
    Application.Current.Dispatcher.Invoke((Action) (() =>
    {
      messageBox = MessageBox.CreateMessageBox((System.Windows.Window) null, messageBoxText, caption, MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.Cancel);
      MessageBox.SetButtonStatus(messageBox, MessageBoxButton.OKCancel, MessageBoxResult.Cancel);
      MessageBox.SetImage(messageBox, MessageBoxImage.Question);
      SystemSounds.Asterisk.Play();
      messageBox.ShowDialog();
    }));
    return messageBox._messageBoxResult;
  }

  public static MessageBoxResult Show(MessageBoxInfo info)
  {
    MessageBox messageBox = (MessageBox) null;
    Application.Current.Dispatcher.Invoke((Action) (() =>
    {
      messageBox = MessageBox.CreateMessageBox((System.Windows.Window) null, info.Message, info.Caption, info.Button, MessageBoxImage.None, info.DefaultResult);
      MessageBox.SetButtonStatus(messageBox, info.Button, info.DefaultResult);
      if (!string.IsNullOrEmpty(info.IconKey))
      {
        messageBox.ShowImage = true;
        messageBox.Image = HandyControl.Tools.ResourceHelper.GetResource<Geometry>(info.IconKey) ?? info.Icon;
        messageBox.ImageBrush = HandyControl.Tools.ResourceHelper.GetResource<Brush>(info.IconBrushKey) ?? info.IconBrush;
      }
      if (info.StyleKey != null)
        messageBox.Style = HandyControl.Tools.ResourceHelper.GetResource<Style>(info.StyleKey) ?? info.Style;
      SystemSounds.Asterisk.Play();
      messageBox.ShowDialog();
    }));
    return messageBox._messageBoxResult;
  }

  public static MessageBoxResult Show(
    string messageBoxText,
    string caption = null,
    MessageBoxButton button = MessageBoxButton.OK,
    MessageBoxImage icon = MessageBoxImage.None,
    MessageBoxResult defaultResult = MessageBoxResult.None)
  {
    return MessageBox.Show((System.Windows.Window) null, messageBoxText, caption, button, icon, defaultResult);
  }

  public static MessageBoxResult Show(
    System.Windows.Window owner,
    string messageBoxText,
    string caption = null,
    MessageBoxButton button = MessageBoxButton.OK,
    MessageBoxImage icon = MessageBoxImage.None,
    MessageBoxResult defaultResult = MessageBoxResult.None)
  {
    MessageBox messageBox = (MessageBox) null;
    Application.Current.Dispatcher.Invoke((Action) (() =>
    {
      messageBox = MessageBox.CreateMessageBox(owner, messageBoxText, caption, button, icon, defaultResult);
      MessageBox.SetButtonStatus(messageBox, button, defaultResult);
      MessageBox.SetImage(messageBox, icon);
      SystemSounds.Asterisk.Play();
      messageBox.ShowDialog();
    }));
    return messageBox._messageBoxResult;
  }

  private static MessageBox CreateMessageBox(
    System.Windows.Window owner,
    string messageBoxText,
    string caption,
    MessageBoxButton button,
    MessageBoxImage icon,
    MessageBoxResult defaultResult)
  {
    if (!MessageBox.IsValidMessageBoxButton(button))
      throw new InvalidEnumArgumentException(nameof (button), (int) button, typeof (MessageBoxButton));
    if (!MessageBox.IsValidMessageBoxImage(icon))
      throw new InvalidEnumArgumentException(nameof (icon), (int) icon, typeof (MessageBoxImage));
    if (!MessageBox.IsValidMessageBoxResult(defaultResult))
      throw new InvalidEnumArgumentException(nameof (defaultResult), (int) defaultResult, typeof (MessageBoxResult));
    System.Windows.Window window = owner ?? WindowHelper.GetActiveWindow();
    bool flag = window == null;
    MessageBox messageBox = new MessageBox();
    messageBox.Message = messageBoxText;
    messageBox.Owner = window;
    messageBox.WindowStartupLocation = flag ? WindowStartupLocation.CenterScreen : WindowStartupLocation.CenterOwner;
    messageBox.ShowTitle = true;
    messageBox.Title = caption ?? string.Empty;
    messageBox.Topmost = flag;
    messageBox._messageBoxResult = defaultResult;
    return messageBox;
  }

  private static void SetButtonStatus(
    MessageBox messageBox,
    MessageBoxButton messageBoxButton,
    MessageBoxResult defaultResult)
  {
    switch (messageBoxButton)
    {
      case MessageBoxButton.OK:
        messageBox._messageBoxResult = MessageBoxResult.Yes;
        messageBox._showOk = true;
        MessageBox messageBox1 = messageBox;
        Button button1 = new Button();
        button1.IsCancel = true;
        button1.IsDefault = true;
        button1.Content = (object) Lang.Confirm;
        button1.Command = (ICommand) ControlCommands.Confirm;
        button1.Style = HandyControl.Tools.ResourceHelper.GetResourceInternal<Style>("MessageBoxButtonStyle");
        messageBox1._buttonOk = button1;
        break;
      case MessageBoxButton.OKCancel:
        messageBox._messageBoxResult = MessageBoxResult.Cancel;
        messageBox._showOk = true;
        MessageBox messageBox2 = messageBox;
        Button button2 = new Button();
        button2.Content = (object) Lang.Confirm;
        button2.Command = (ICommand) ControlCommands.Confirm;
        messageBox2._buttonOk = button2;
        messageBox._showCancel = true;
        MessageBox messageBox3 = messageBox;
        Button button3 = new Button();
        button3.IsCancel = true;
        button3.Content = (object) Lang.Cancel;
        button3.Command = (ICommand) ControlCommands.Cancel;
        messageBox3._buttonCancel = button3;
        if (defaultResult == MessageBoxResult.Cancel)
        {
          messageBox._buttonOk.Style = HandyControl.Tools.ResourceHelper.GetResourceInternal<Style>("MessageBoxButtonStyle");
          messageBox._buttonCancel.Style = HandyControl.Tools.ResourceHelper.GetResourceInternal<Style>("MessageBoxPrimaryButtonStyle");
          messageBox._buttonCancel.IsDefault = true;
          break;
        }
        messageBox._buttonOk.Style = HandyControl.Tools.ResourceHelper.GetResourceInternal<Style>("MessageBoxPrimaryButtonStyle");
        messageBox._buttonOk.IsDefault = true;
        messageBox._buttonCancel.Style = HandyControl.Tools.ResourceHelper.GetResourceInternal<Style>("MessageBoxButtonStyle");
        break;
      case MessageBoxButton.YesNoCancel:
        messageBox._messageBoxResult = MessageBoxResult.Cancel;
        messageBox._showYes = true;
        MessageBox messageBox4 = messageBox;
        Button button4 = new Button();
        button4.Content = (object) Lang.Yes;
        button4.Command = (ICommand) ControlCommands.Yes;
        messageBox4._buttonYes = button4;
        messageBox._showNo = true;
        MessageBox messageBox5 = messageBox;
        Button button5 = new Button();
        button5.Content = (object) Lang.No;
        button5.Command = (ICommand) ControlCommands.No;
        messageBox5._buttonNo = button5;
        messageBox._showCancel = true;
        MessageBox messageBox6 = messageBox;
        Button button6 = new Button();
        button6.IsCancel = true;
        button6.Content = (object) Lang.Cancel;
        button6.Command = (ICommand) ControlCommands.Cancel;
        messageBox6._buttonCancel = button6;
        if (defaultResult == MessageBoxResult.No)
        {
          messageBox._buttonYes.Style = HandyControl.Tools.ResourceHelper.GetResourceInternal<Style>("MessageBoxButtonStyle");
          messageBox._buttonNo.Style = HandyControl.Tools.ResourceHelper.GetResourceInternal<Style>("MessageBoxPrimaryButtonStyle");
          messageBox._buttonNo.IsDefault = true;
          messageBox._buttonCancel.Style = HandyControl.Tools.ResourceHelper.GetResourceInternal<Style>("MessageBoxButtonStyle");
          break;
        }
        if (defaultResult == MessageBoxResult.Cancel)
        {
          messageBox._buttonYes.Style = HandyControl.Tools.ResourceHelper.GetResourceInternal<Style>("MessageBoxButtonStyle");
          messageBox._buttonNo.Style = HandyControl.Tools.ResourceHelper.GetResourceInternal<Style>("MessageBoxButtonStyle");
          messageBox._buttonCancel.Style = HandyControl.Tools.ResourceHelper.GetResourceInternal<Style>("MessageBoxPrimaryButtonStyle");
          messageBox._buttonCancel.IsDefault = true;
          break;
        }
        messageBox._buttonYes.Style = HandyControl.Tools.ResourceHelper.GetResourceInternal<Style>("MessageBoxPrimaryButtonStyle");
        messageBox._buttonYes.IsDefault = true;
        messageBox._buttonNo.Style = HandyControl.Tools.ResourceHelper.GetResourceInternal<Style>("MessageBoxButtonStyle");
        messageBox._buttonCancel.Style = HandyControl.Tools.ResourceHelper.GetResourceInternal<Style>("MessageBoxButtonStyle");
        break;
      case MessageBoxButton.YesNo:
        messageBox._messageBoxResult = MessageBoxResult.Cancel;
        messageBox._showYes = true;
        MessageBox messageBox7 = messageBox;
        Button button7 = new Button();
        button7.Content = (object) Lang.Yes;
        button7.Command = (ICommand) ControlCommands.Yes;
        messageBox7._buttonYes = button7;
        messageBox._showNo = true;
        MessageBox messageBox8 = messageBox;
        Button button8 = new Button();
        button8.Content = (object) Lang.No;
        button8.Command = (ICommand) ControlCommands.No;
        messageBox8._buttonNo = button8;
        if (defaultResult == MessageBoxResult.No)
        {
          messageBox._buttonYes.Style = HandyControl.Tools.ResourceHelper.GetResourceInternal<Style>("MessageBoxButtonStyle");
          messageBox._buttonNo.Style = HandyControl.Tools.ResourceHelper.GetResourceInternal<Style>("MessageBoxPrimaryButtonStyle");
          messageBox._buttonNo.IsDefault = true;
          break;
        }
        messageBox._buttonYes.Style = HandyControl.Tools.ResourceHelper.GetResourceInternal<Style>("MessageBoxPrimaryButtonStyle");
        messageBox._buttonYes.IsDefault = true;
        messageBox._buttonNo.Style = HandyControl.Tools.ResourceHelper.GetResourceInternal<Style>("MessageBoxButtonStyle");
        break;
    }
  }

  private static void SetImage(MessageBox messageBox, MessageBoxImage messageBoxImage)
  {
    string key1 = string.Empty;
    string key2 = string.Empty;
    switch (messageBoxImage)
    {
      case MessageBoxImage.Hand:
        key1 = "ErrorGeometry";
        key2 = "DangerBrush";
        break;
      case MessageBoxImage.Question:
        key1 = "AskGeometry";
        key2 = "AccentBrush";
        break;
      case MessageBoxImage.Exclamation:
        key1 = "WarningGeometry";
        key2 = "WarningBrush";
        break;
      case MessageBoxImage.Asterisk:
        key1 = "InfoGeometry";
        key2 = "InfoBrush";
        break;
    }
    if (string.IsNullOrEmpty(key1))
      return;
    messageBox.ShowImage = true;
    messageBox.Image = HandyControl.Tools.ResourceHelper.GetResourceInternal<Geometry>(key1);
    messageBox.ImageBrush = HandyControl.Tools.ResourceHelper.GetResourceInternal<Brush>(key2);
  }

  private static bool IsValidMessageBoxButton(MessageBoxButton value)
  {
    return value == MessageBoxButton.OK || value == MessageBoxButton.OKCancel || value == MessageBoxButton.YesNo || value == MessageBoxButton.YesNoCancel;
  }

  private static bool IsValidMessageBoxImage(MessageBoxImage value)
  {
    return value == MessageBoxImage.Asterisk || value == MessageBoxImage.Hand || value == MessageBoxImage.Exclamation || value == MessageBoxImage.Hand || value == MessageBoxImage.Asterisk || value == MessageBoxImage.None || value == MessageBoxImage.Question || value == MessageBoxImage.Hand || value == MessageBoxImage.Exclamation;
  }

  private static bool IsValidMessageBoxResult(MessageBoxResult value)
  {
    return value == MessageBoxResult.Cancel || value == MessageBoxResult.No || value == MessageBoxResult.None || value == MessageBoxResult.OK || value == MessageBoxResult.Yes;
  }
}
