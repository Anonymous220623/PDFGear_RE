// Decompiled with JetBrains decompiler
// Type: CommomLib.Controls.HotKeyEditBox
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using CommomLib.Commom.HotKeys;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

#nullable disable
namespace CommomLib.Controls;

public partial class HotKeyEditBox : TextBox
{
  private HashSet<Key> pressedKey = new HashSet<Key>();
  private HotKeyItem? tempHotKey;
  private Window window;
  private Button ClearButton;
  public static readonly DependencyProperty HotKeyProperty = DependencyProperty.Register(nameof (HotKey), typeof (HotKeyItem), typeof (HotKeyEditBox), (PropertyMetadata) new FrameworkPropertyMetadata((object) new HotKeyItem(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is HotKeyEditBox hotKeyEditBox2) || object.Equals(a.NewValue, a.OldValue))
      return;
    hotKeyEditBox2.UpdateDisplayText();
  })));
  public static readonly DependencyProperty InvalidHotKeyDisplayTextProperty = DependencyProperty.Register(nameof (InvalidHotKeyDisplayText), typeof (string), typeof (HotKeyEditBox), new PropertyMetadata((object) "", (PropertyChangedCallback) ((s, a) =>
  {
    if (!(s is HotKeyEditBox hotKeyEditBox4) || object.Equals(a.NewValue, a.OldValue))
      return;
    hotKeyEditBox4.UpdateDisplayText();
  })));

  static HotKeyEditBox()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (HotKeyEditBox), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (HotKeyEditBox)));
    TextBoxBase.IsUndoEnabledProperty.OverrideMetadata(typeof (HotKeyEditBox), (PropertyMetadata) new FrameworkPropertyMetadata((object) false));
    TextBoxBase.SelectionOpacityProperty.OverrideMetadata(typeof (HotKeyEditBox), (PropertyMetadata) new FrameworkPropertyMetadata((object) 0.0));
  }

  public HotKeyEditBox()
  {
    this.Loaded += (RoutedEventHandler) ((s, a) =>
    {
      if (!this.IsLoaded)
        return;
      if (this.window != null)
        this.window.Deactivated -= new EventHandler(this.Window_Deactivated);
      this.window = Window.GetWindow((DependencyObject) this);
      if (this.window == null)
        return;
      this.window.Deactivated += new EventHandler(this.Window_Deactivated);
    });
    this.Unloaded += (RoutedEventHandler) ((s, a) =>
    {
      if (this.IsLoaded || this.window == null)
        return;
      this.window.Deactivated -= new EventHandler(this.Window_Deactivated);
      this.window = (Window) null;
    });
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    if (this.ClearButton != null)
      this.ClearButton.Click -= new RoutedEventHandler(this.ClearButton_Click);
    this.ClearButton = this.GetTemplateChild("ClearButton") as Button;
    if (this.ClearButton == null)
      return;
    this.ClearButton.Click += new RoutedEventHandler(this.ClearButton_Click);
    this.UpdateClearButtonState();
  }

  public HotKeyItem HotKey
  {
    get => (HotKeyItem) this.GetValue(HotKeyEditBox.HotKeyProperty);
    set => this.SetValue(HotKeyEditBox.HotKeyProperty, (object) value);
  }

  public string InvalidHotKeyDisplayText
  {
    get => (string) this.GetValue(HotKeyEditBox.InvalidHotKeyDisplayTextProperty);
    set => this.SetValue(HotKeyEditBox.InvalidHotKeyDisplayTextProperty, (object) value);
  }

  protected override void OnPreviewKeyDown(KeyEventArgs e)
  {
    e.Handled = true;
    if (this.IsReadOnly)
      return;
    base.OnPreviewKeyDown(e);
    Key key = e.Key == Key.System ? e.SystemKey : e.Key;
    if (key == Key.Escape)
    {
      this.Cancel();
    }
    else
    {
      int virtualKey = KeyInterop.VirtualKeyFromKey(key);
      if (KeyboardHelper.IsModifierKey(virtualKey))
        virtualKey = 0;
      if (virtualKey == 0 && this.tempHotKey.HasValue)
        virtualKey = this.tempHotKey.Value.VirtualKey;
      this.tempHotKey = new HotKeyItem?(new HotKeyItem(virtualKey, KeyboardHelper.GetModifierKeyState()));
      lock (this.pressedKey)
        this.pressedKey.Add(key);
      this.UpdateDisplayText();
    }
  }

  protected override void OnPreviewKeyUp(KeyEventArgs e)
  {
    e.Handled = true;
    if (this.IsReadOnly)
    {
      this.Cancel();
    }
    else
    {
      base.OnPreviewKeyUp(e);
      Key key = e.Key == Key.System ? e.SystemKey : e.Key;
      lock (this.pressedKey)
      {
        if (this.pressedKey.Count == 0)
          return;
        this.pressedKey.Remove(key);
      }
      if (this.tempHotKey.HasValue && this.tempHotKey.Value.IsSupported)
      {
        HotKeyItem hotKeyItem = this.tempHotKey.Value;
        this.tempHotKey = new HotKeyItem?();
        this.HotKey = hotKeyItem;
        this.UpdateDisplayText();
      }
      else
      {
        if (this.pressedKey.Count != 0)
          return;
        this.Cancel();
      }
    }
  }

  protected override void OnGotFocus(RoutedEventArgs e)
  {
    base.OnGotFocus(e);
    InputManager.Current.PreNotifyInput -= new NotifyInputEventHandler(this.InputManager_PreNotifyInput);
    InputManager.Current.PreNotifyInput += new NotifyInputEventHandler(this.InputManager_PreNotifyInput);
    this.UpdateClearButtonState();
  }

  protected override void OnLostFocus(RoutedEventArgs e)
  {
    base.OnLostFocus(e);
    InputManager.Current.PreNotifyInput -= new NotifyInputEventHandler(this.InputManager_PreNotifyInput);
    this.Cancel();
  }

  protected override void OnContextMenuOpening(ContextMenuEventArgs e) => e.Handled = true;

  protected override void OnQueryCursor(QueryCursorEventArgs e)
  {
    e.Cursor = this.Cursor ?? Cursors.Arrow;
    e.Handled = true;
  }

  private void Cancel(bool forceKillFocus = false)
  {
    lock (this.pressedKey)
      this.pressedKey.Clear();
    this.tempHotKey = new HotKeyItem?();
    Keyboard.ClearFocus();
    if (forceKillFocus)
      this.ForceKillFocus((Func<UIElement, bool>) (c => !(c is HotKeyEditBox)));
    this.UpdateDisplayText();
  }

  private void ClearHotKey()
  {
    this.Cancel();
    this.HotKey = new HotKeyItem();
    this.UpdateDisplayText();
  }

  private void ClearButton_Click(object sender, RoutedEventArgs e) => this.ClearHotKey();

  private void InputManager_PreNotifyInput(object sender, NotifyInputEventArgs e)
  {
    if (e.StagingItem.Input.RoutedEvent != Mouse.PreviewMouseDownEvent || this.InputHitTest(((MouseEventArgs) e.StagingItem.Input).GetPosition((IInputElement) this)) != null)
      return;
    this.Cancel(true);
  }

  private void Window_Deactivated(object sender, EventArgs e) => this.Cancel(true);

  private void UpdateDisplayText()
  {
    HotKeyItem? tempHotKey = this.tempHotKey;
    if (tempHotKey.HasValue)
    {
      StringBuilder stringBuilder = new StringBuilder();
      HotKeyItem hotKeyItem = tempHotKey.Value;
      if ((hotKeyItem.Modifiers & ModifierKeys.Control) != ModifierKeys.None)
        stringBuilder.Append(KeyboardHelper.GetName(ModifierKeys.Control)).Append(" + ");
      hotKeyItem = tempHotKey.Value;
      if ((hotKeyItem.Modifiers & ModifierKeys.Shift) != ModifierKeys.None)
        stringBuilder.Append(KeyboardHelper.GetName(ModifierKeys.Shift)).Append(" + ");
      hotKeyItem = tempHotKey.Value;
      if ((hotKeyItem.Modifiers & ModifierKeys.Alt) != ModifierKeys.None)
        stringBuilder.Append(KeyboardHelper.GetName(ModifierKeys.Alt)).Append(" + ");
      hotKeyItem = tempHotKey.Value;
      if ((hotKeyItem.Modifiers & ModifierKeys.Windows) != ModifierKeys.None)
        stringBuilder.Append(KeyboardHelper.GetName(ModifierKeys.Windows)).Append(" + ");
      hotKeyItem = tempHotKey.Value;
      if (!KeyboardHelper.IsModifierKey(hotKeyItem.VirtualKey))
      {
        hotKeyItem = tempHotKey.Value;
        string name = KeyboardHelper.GetName(hotKeyItem.VirtualKey);
        if (!string.IsNullOrEmpty(name))
          stringBuilder.Append(name);
      }
      this.Text = stringBuilder.ToString();
      this.UpdateClearButtonState();
    }
    else
    {
      HotKeyItem hotKey = this.HotKey;
      if (hotKey.IsSupported)
        this.Text = hotKey.ToString(HotKeyItem.FormatType.Normal);
      else
        this.Text = this.InvalidHotKeyDisplayText;
      this.UpdateClearButtonState();
    }
  }

  private void UpdateClearButtonState()
  {
    if (this.ClearButton == null)
      return;
    if (this.IsReadOnly || !this.IsKeyboardFocusWithin)
      this.ClearButton.Visibility = Visibility.Collapsed;
    else if (this.tempHotKey.HasValue)
      this.ClearButton.Visibility = Visibility.Visible;
    else if (this.HotKey.IsSupported)
      this.ClearButton.Visibility = Visibility.Visible;
    else
      this.ClearButton.Visibility = Visibility.Collapsed;
  }
}
