// Decompiled with JetBrains decompiler
// Type: PDFKit.Contents.Utils.KeyboardHelper
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;

#nullable disable
namespace PDFKit.Contents.Utils;

public static class KeyboardHelper
{
  private static byte[] buffer;
  private static object locker = new object();
  private static StringBuilder sb;
  private const int VK_LSHIFT = 160 /*0xA0*/;
  private const int VK_RSHIFT = 161;
  private const int VK_LCONTROL = 162;
  private const int VK_RCONTROL = 163;
  private const int VK_LMENU = 164;
  private const int VK_RMENU = 165;

  public static bool TryGetCharFromKey(Key key, out char ch)
  {
    int uVirtKey = KeyInterop.VirtualKeyFromKey(key);
    ch = KeyboardHelper.GetUnicodeCharacter((uint) uVirtKey);
    return !char.IsControl(ch);
  }

  private static char GetUnicodeCharacter(uint uVirtKey)
  {
    if (KeyboardHelper.buffer == null)
    {
      lock (KeyboardHelper.locker)
      {
        if (KeyboardHelper.buffer == null)
        {
          KeyboardHelper.buffer = new byte[(int) byte.MaxValue];
          KeyboardHelper.sb = new StringBuilder(8);
        }
      }
    }
    uint wScanCode = KeyboardHelper.MapVirtualKey(uVirtKey, KeyboardHelper.MapType.MAPVK_VK_TO_VSC);
    lock (KeyboardHelper.locker)
    {
      KeyboardHelper.sb.Clear();
      KeyboardHelper.GetKeyboardState(KeyboardHelper.buffer);
      if (KeyboardHelper.ToUnicode(uVirtKey, wScanCode, KeyboardHelper.buffer, KeyboardHelper.sb, KeyboardHelper.sb.Capacity, 0U) == 1)
        return KeyboardHelper.sb[0];
    }
    return char.MinValue;
  }

  [DllImport("user32.dll")]
  private static extern int ToUnicode(
    uint wVirtKey,
    uint wScanCode,
    byte[] lpKeyState,
    [MarshalAs(UnmanagedType.LPWStr), Out] StringBuilder pwszBuff,
    int cchBuff,
    uint wFlags);

  [DllImport("user32.dll")]
  private static extern bool GetKeyboardState(byte[] lpKeyState);

  [DllImport("user32.dll")]
  private static extern uint MapVirtualKey(uint uCode, KeyboardHelper.MapType uMapType);

  private enum MapType : uint
  {
    MAPVK_VK_TO_VSC,
    MAPVK_VSC_TO_VK,
    MAPVK_VK_TO_CHAR,
    MAPVK_VSC_TO_VK_EX,
  }
}
