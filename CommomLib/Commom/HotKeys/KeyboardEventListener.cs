// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.HotKeys.KeyboardEventListener
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.Windows.Input;

#nullable disable
namespace CommomLib.Commom.HotKeys;

internal class KeyboardEventListener
{
  private static object locker = new object();
  private static IKeyboardEventListener windowKeyEventListener;
  private static IKeyboardEventListener hookKeyEventListener;

  public static IKeyboardEventListener WindowKeyEventListener
  {
    get
    {
      if (KeyboardEventListener.windowKeyEventListener == null)
      {
        lock (KeyboardEventListener.locker)
        {
          if (KeyboardEventListener.windowKeyEventListener == null)
            KeyboardEventListener.windowKeyEventListener = (IKeyboardEventListener) new CommomLib.Commom.HotKeys.WindowKeyEventListener();
        }
      }
      return KeyboardEventListener.windowKeyEventListener;
    }
  }

  public static IKeyboardEventListener HookKeyEventListener
  {
    get
    {
      if (KeyboardEventListener.hookKeyEventListener == null)
      {
        lock (KeyboardEventListener.locker)
        {
          if (KeyboardEventListener.hookKeyEventListener == null)
            KeyboardEventListener.hookKeyEventListener = (IKeyboardEventListener) new CommomLib.Commom.HotKeys.HookKeyEventListener();
        }
      }
      return KeyboardEventListener.hookKeyEventListener;
    }
  }

  public delegate void KeyDownEventHandler(object sender, KeyboardEventListener.KeyDownEventArgs e);

  public class KeyDownEventArgs : EventArgs
  {
    internal KeyDownEventArgs(
      int virtualKey,
      Key key,
      ushort count,
      ushort scanCode,
      bool extendKey,
      bool prevKeyPressed)
    {
      this.VirtualKey = virtualKey;
      this.Key = key;
      this.Count = count;
      this.ScanCode = scanCode;
      this.ExtendKey = extendKey;
      this.PrevKeyPressed = prevKeyPressed;
    }

    public int VirtualKey { get; }

    public Key Key { get; }

    public ushort Count { get; }

    public ushort ScanCode { get; }

    public bool ExtendKey { get; }

    public bool PrevKeyPressed { get; }

    public ModifierKeys Modifier => KeyboardHelper.GetModifierKeyState();

    public bool Handled { get; set; }

    public override string ToString()
    {
      return $"VirtualKey: 0x{this.VirtualKey:X2}, Key: {this.Key}, Count: {this.Count}, ScanCode: {this.ScanCode}, ExtendKey: {this.ExtendKey}, PrevKeyPressed: {this.PrevKeyPressed}";
    }
  }
}
