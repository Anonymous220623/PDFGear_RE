// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.HotKeys.WindowKeyEventListener
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.Windows;
using System.Windows.Input;

#nullable disable
namespace CommomLib.Commom.HotKeys;

internal class WindowKeyEventListener : IKeyboardEventListener
{
  private object locker = new object();
  private KeyboardEventListener.KeyDownEventHandler onKeyDownEvent;

  public event KeyboardEventListener.KeyDownEventHandler KeyDown
  {
    add
    {
      lock (this.locker)
      {
        this.VerifyAccess();
        this.onKeyDownEvent += value;
        if (this.onKeyDownEvent == null)
          return;
        InputManager.Current.PreNotifyInput -= new NotifyInputEventHandler(this.InputManager_PreNotifyInput);
        InputManager.Current.PreNotifyInput += new NotifyInputEventHandler(this.InputManager_PreNotifyInput);
      }
    }
    remove
    {
      lock (this.locker)
      {
        this.VerifyAccess();
        this.onKeyDownEvent -= value;
        if (this.onKeyDownEvent != null)
          return;
        InputManager.Current.PreNotifyInput -= new NotifyInputEventHandler(this.InputManager_PreNotifyInput);
      }
    }
  }

  private void InputManager_PreNotifyInput(object sender, NotifyInputEventArgs e)
  {
    if (!(e.StagingItem.Input is KeyEventArgs input) || input.RoutedEvent != Keyboard.PreviewKeyDownEvent || !input.IsDown)
      return;
    Key key = input.Key;
    if (key == Key.System)
      key = input.SystemKey;
    int virtualKey = KeyInterop.VirtualKeyFromKey(key);
    KeyboardEventListener.KeyDownEventHandler onKeyDownEvent = this.onKeyDownEvent;
    if (virtualKey == 0 || onKeyDownEvent == null)
      return;
    KeyboardEventListener.KeyDownEventArgs e1 = new KeyboardEventListener.KeyDownEventArgs(virtualKey, key, (ushort) 1, (ushort) KeyboardHelper.GetScanCode(virtualKey), false, input.IsRepeat);
    onKeyDownEvent((object) this, e1);
    input.Handled = e1.Handled;
  }

  private void VerifyAccess()
  {
    (Application.Current?.Dispatcher ?? throw new InvalidOperationException("Dispatcher")).VerifyAccess();
  }
}
