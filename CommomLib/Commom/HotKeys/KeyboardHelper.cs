// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.HotKeys.KeyboardHelper
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Input;

#nullable disable
namespace CommomLib.Commom.HotKeys;

internal static class KeyboardHelper
{
  private static readonly IReadOnlyDictionary<int, string> staticKeyNames = (IReadOnlyDictionary<int, string>) new Dictionary<int, string>()
  {
    [33] = "Page Up",
    [34] = "Page Down",
    [36] = "Home",
    [35] = "End",
    [37] = "←",
    [39] = "→",
    [38] = "↑",
    [40] = "↓",
    [106] = "Num *",
    [107] = "+",
    [109] = "-",
    [110] = "Num .",
    [111] = "Num /",
    [93] = "Menu",
    [13] = "Enter",
    [32 /*0x20*/] = "Space",
    [8] = "Backspace",
    [45] = "Insert",
    [46] = "Delete"
  };
  private static readonly IReadOnlyDictionary<int, string> oemKeyDefaultNames = (IReadOnlyDictionary<int, string>) new Dictionary<int, string>()
  {
    [187] = "+",
    [188] = ",",
    [189] = "-",
    [190] = ".",
    [186] = ";",
    [191] = "?",
    [192 /*0xC0*/] = "~",
    [219] = "[",
    [220] = "\\",
    [221] = "]",
    [222] = "'"
  };

  internal static bool HasName(int virtualKey)
  {
    return KeyboardHelper.staticKeyNames.ContainsKey(virtualKey) || KeyboardHelper.oemKeyDefaultNames.ContainsKey(virtualKey) || KeyboardHelper.IsControlKey(virtualKey) || KeyboardHelper.IsAltKey(virtualKey) || KeyboardHelper.IsShiftKey(virtualKey) || KeyboardHelper.IsWinKey(virtualKey) || KeyboardHelper.IsNumberKey(virtualKey, out int _) || KeyboardHelper.IsAlphabetKey(virtualKey, out char _) || KeyboardHelper.IsFKey(virtualKey, out string _);
  }

  internal static int NormalizeKey(int virtualKey)
  {
    if (KeyboardHelper.IsShiftKey(virtualKey))
      return 16 /*0x10*/;
    if (KeyboardHelper.IsControlKey(virtualKey))
      return 17;
    return KeyboardHelper.IsAltKey(virtualKey) ? 18 : virtualKey;
  }

  internal static string GetName(ModifierKeys modifierKey)
  {
    switch (modifierKey)
    {
      case ModifierKeys.Alt:
        return KeyboardHelper.GetName(18);
      case ModifierKeys.Control:
        return KeyboardHelper.GetName(17);
      case ModifierKeys.Shift:
        return KeyboardHelper.GetName(16 /*0x10*/);
      case ModifierKeys.Windows:
        return KeyboardHelper.GetName(91);
      default:
        return (string) null;
    }
  }

  internal static string GetName(int virtualKey)
  {
    string name1;
    if (KeyboardHelper.staticKeyNames.TryGetValue(virtualKey, out name1))
      return name1;
    if (KeyboardHelper.oemKeyDefaultNames.TryGetValue(virtualKey, out name1))
    {
      string name2;
      return KeyboardHelper.TryGetSystemKeyName(virtualKey, out name2) && (virtualKey != 107 || !(name2 == "=")) ? name2 : name1;
    }
    if (KeyboardHelper.IsControlKey(virtualKey))
      return "Ctrl";
    if (KeyboardHelper.IsAltKey(virtualKey))
      return "Alt";
    if (KeyboardHelper.IsShiftKey(virtualKey))
      return "Shift";
    if (KeyboardHelper.IsWinKey(virtualKey))
      return "Win";
    int num;
    if (KeyboardHelper.IsNumberKey(virtualKey, out num))
    {
      string name3;
      return KeyboardHelper.TryGetSystemKeyName(virtualKey, out name3) ? name3 : $"Num {num}";
    }
    char ch;
    if (KeyboardHelper.IsAlphabetKey(virtualKey, out ch))
    {
      string name4;
      return KeyboardHelper.TryGetSystemKeyName(virtualKey, out name4) ? name4 : $"{ch}";
    }
    string str;
    if (!KeyboardHelper.IsFKey(virtualKey, out str))
      return (string) null;
    string name5;
    return KeyboardHelper.TryGetSystemKeyName(virtualKey, out name5) ? name5 : str ?? "";
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static bool IsWinKey(int virtualKey) => virtualKey == 91 || virtualKey == 92;

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static bool IsModifierKey(int virtualKey)
  {
    return KeyboardHelper.IsControlKey(virtualKey) || KeyboardHelper.IsShiftKey(virtualKey) || KeyboardHelper.IsAltKey(virtualKey) || KeyboardHelper.IsWinKey(virtualKey);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static bool IsControlKey(int virtualKey)
  {
    return virtualKey == 17 || virtualKey == 162 || virtualKey == 163;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static bool IsShiftKey(int virtualKey)
  {
    return virtualKey == 16 /*0x10*/ || virtualKey == 160 /*0xA0*/ || virtualKey == 161;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static bool IsAltKey(int virtualKey)
  {
    return virtualKey == 18 || virtualKey == 164 || virtualKey == 165;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static bool IsNumberKey(int virtualKey, out int value)
  {
    value = 0;
    if (virtualKey >= 48 /*0x30*/ && virtualKey <= 57)
    {
      value = virtualKey - 48 /*0x30*/;
      return true;
    }
    if (virtualKey < 96 /*0x60*/ || virtualKey > 105)
      return false;
    value = virtualKey - 96 /*0x60*/;
    return true;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static bool IsAlphabetKey(int virtualKey, out char value)
  {
    value = char.MinValue;
    if (virtualKey < 65 || virtualKey > 90)
      return false;
    value = (char) (virtualKey - 65 + 65);
    return true;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static bool IsFKey(int virtualKey, out string value)
  {
    value = string.Empty;
    if (virtualKey < 112 /*0x70*/ || virtualKey > 123)
      return false;
    value = "F" + (virtualKey - 112 /*0x70*/ + 1).ToString();
    return true;
  }

  internal static ModifierKeys GetModifierKeyState()
  {
    ModifierKeys modifierKeyState = ModifierKeys.None;
    if (((int) NativeMethods.GetKeyState(160 /*0xA0*/) & 32768 /*0x8000*/) == 32768 /*0x8000*/ || ((int) NativeMethods.GetKeyState(161) & 32768 /*0x8000*/) == 32768 /*0x8000*/)
      modifierKeyState |= ModifierKeys.Shift;
    if (((int) NativeMethods.GetKeyState(162) & 32768 /*0x8000*/) == 32768 /*0x8000*/ || ((int) NativeMethods.GetKeyState(163) & 32768 /*0x8000*/) == 32768 /*0x8000*/)
      modifierKeyState |= ModifierKeys.Control;
    if (((int) NativeMethods.GetKeyState(164) & 32768 /*0x8000*/) == 32768 /*0x8000*/ || ((int) NativeMethods.GetKeyState(165) & 32768 /*0x8000*/) == 32768 /*0x8000*/)
      modifierKeyState |= ModifierKeys.Alt;
    if (((int) NativeMethods.GetKeyState(91) & 32768 /*0x8000*/) == 32768 /*0x8000*/ || ((int) NativeMethods.GetKeyState(92) & 32768 /*0x8000*/) == 32768 /*0x8000*/)
      modifierKeyState |= ModifierKeys.Windows;
    return modifierKeyState;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  internal static uint GetScanCode(int virtualKey)
  {
    return NativeMethods.MapVirtualKey((uint) virtualKey, NativeMethods.MAPVK.MAPVK_VK_TO_VSC);
  }

  private static unsafe bool TryGetSystemKeyName(int virtualKey, out string name)
  {
    name = (string) null;
    uint scanCode = KeyboardHelper.GetScanCode(virtualKey);
    char* lpString = stackalloc char[65];
    int keyNameText = NativeMethods.GetKeyNameText((IntPtr) (long) (scanCode << 16 /*0x10*/), lpString, 64 /*0x40*/);
    if (keyNameText <= 0)
      return false;
    name = new string(lpString, 0, keyNameText);
    if (virtualKey == 192 /*0xC0*/ && name == "`")
      name = "~";
    return true;
  }

  private static class VirtualKeys
  {
    internal const int VK_LBUTTON = 1;
    internal const int VK_RBUTTON = 2;
    internal const int VK_CANCEL = 3;
    internal const int VK_MBUTTON = 4;
    internal const int VK_XBUTTON1 = 5;
    internal const int VK_XBUTTON2 = 6;
    internal const int VK_BACK = 8;
    internal const int VK_TAB = 9;
    internal const int VK_CLEAR = 12;
    internal const int VK_RETURN = 13;
    internal const int VK_SHIFT = 16 /*0x10*/;
    internal const int VK_CONTROL = 17;
    internal const int VK_MENU = 18;
    internal const int VK_PAUSE = 19;
    internal const int VK_CAPITAL = 20;
    internal const int VK_KANA = 21;
    internal const int VK_HANGUEL = 21;
    internal const int VK_HANGUL = 21;
    internal const int VK_IME_ON = 22;
    internal const int VK_JUNJA = 23;
    internal const int VK_FINAL = 24;
    internal const int VK_HANJA = 25;
    internal const int VK_KANJI = 25;
    internal const int VK_IME_OFF = 26;
    internal const int VK_ESCAPE = 27;
    internal const int VK_CONVERT = 28;
    internal const int VK_NONCONVERT = 29;
    internal const int VK_ACCEPT = 30;
    internal const int VK_MODECHANGE = 31 /*0x1F*/;
    internal const int VK_SPACE = 32 /*0x20*/;
    internal const int VK_PRIOR = 33;
    internal const int VK_NEXT = 34;
    internal const int VK_END = 35;
    internal const int VK_HOME = 36;
    internal const int VK_LEFT = 37;
    internal const int VK_UP = 38;
    internal const int VK_RIGHT = 39;
    internal const int VK_DOWN = 40;
    internal const int VK_SELECT = 41;
    internal const int VK_PRINT = 42;
    internal const int VK_EXECUTE = 43;
    internal const int VK_SNAPSHOT = 44;
    internal const int VK_INSERT = 45;
    internal const int VK_DELETE = 46;
    internal const int VK_HELP = 47;
    internal const int VK_0 = 48 /*0x30*/;
    internal const int VK_1 = 49;
    internal const int VK_2 = 50;
    internal const int VK_3 = 51;
    internal const int VK_4 = 52;
    internal const int VK_5 = 53;
    internal const int VK_6 = 54;
    internal const int VK_7 = 55;
    internal const int VK_8 = 56;
    internal const int VK_9 = 57;
    internal const int VK_A = 65;
    internal const int VK_B = 66;
    internal const int VK_C = 67;
    internal const int VK_D = 68;
    internal const int VK_E = 69;
    internal const int VK_F = 70;
    internal const int VK_G = 71;
    internal const int VK_H = 72;
    internal const int VK_I = 73;
    internal const int VK_J = 74;
    internal const int VK_K = 75;
    internal const int VK_L = 76;
    internal const int VK_M = 77;
    internal const int VK_N = 78;
    internal const int VK_O = 79;
    internal const int VK_P = 80 /*0x50*/;
    internal const int VK_Q = 81;
    internal const int VK_R = 82;
    internal const int VK_S = 83;
    internal const int VK_T = 84;
    internal const int VK_U = 85;
    internal const int VK_V = 86;
    internal const int VK_W = 87;
    internal const int VK_X = 88;
    internal const int VK_Y = 89;
    internal const int VK_Z = 90;
    internal const int VK_LWIN = 91;
    internal const int VK_RWIN = 92;
    internal const int VK_APPS = 93;
    internal const int VK_SLEEP = 95;
    internal const int VK_NUMPAD0 = 96 /*0x60*/;
    internal const int VK_NUMPAD1 = 97;
    internal const int VK_NUMPAD2 = 98;
    internal const int VK_NUMPAD3 = 99;
    internal const int VK_NUMPAD4 = 100;
    internal const int VK_NUMPAD5 = 101;
    internal const int VK_NUMPAD6 = 102;
    internal const int VK_NUMPAD7 = 103;
    internal const int VK_NUMPAD8 = 104;
    internal const int VK_NUMPAD9 = 105;
    internal const int VK_MULTIPLY = 106;
    internal const int VK_ADD = 107;
    internal const int VK_SEPARATOR = 108;
    internal const int VK_SUBTRACT = 109;
    internal const int VK_DECIMAL = 110;
    internal const int VK_DIVIDE = 111;
    internal const int VK_F1 = 112 /*0x70*/;
    internal const int VK_F2 = 113;
    internal const int VK_F3 = 114;
    internal const int VK_F4 = 115;
    internal const int VK_F5 = 116;
    internal const int VK_F6 = 117;
    internal const int VK_F7 = 118;
    internal const int VK_F8 = 119;
    internal const int VK_F9 = 120;
    internal const int VK_F10 = 121;
    internal const int VK_F11 = 122;
    internal const int VK_F12 = 123;
    internal const int VK_F13 = 124;
    internal const int VK_F14 = 125;
    internal const int VK_F15 = 126;
    internal const int VK_F16 = 127 /*0x7F*/;
    internal const int VK_F17 = 128 /*0x80*/;
    internal const int VK_F18 = 129;
    internal const int VK_F19 = 130;
    internal const int VK_F20 = 131;
    internal const int VK_F21 = 132;
    internal const int VK_F22 = 133;
    internal const int VK_F23 = 134;
    internal const int VK_F24 = 135;
    internal const int VK_NUMLOCK = 144 /*0x90*/;
    internal const int VK_SCROLL = 145;
    internal const int VK_OEM_NEC_EQUAL = 146;
    internal const int VK_OEM_FJ_JISHO = 146;
    internal const int VK_OEM_FJ_MASSHOU = 147;
    internal const int VK_OEM_FJ_TOUROKU = 148;
    internal const int VK_OEM_FJ_LOYA = 149;
    internal const int VK_OEM_FJ_ROYA = 150;
    internal const int VK_LSHIFT = 160 /*0xA0*/;
    internal const int VK_RSHIFT = 161;
    internal const int VK_LCONTROL = 162;
    internal const int VK_RCONTROL = 163;
    internal const int VK_LMENU = 164;
    internal const int VK_RMENU = 165;
    internal const int VK_BROWSER_BACK = 166;
    internal const int VK_BROWSER_FORWARD = 167;
    internal const int VK_BROWSER_REFRESH = 168;
    internal const int VK_BROWSER_STOP = 169;
    internal const int VK_BROWSER_SEARCH = 170;
    internal const int VK_BROWSER_FAVORITES = 171;
    internal const int VK_BROWSER_HOME = 172;
    internal const int VK_VOLUME_MUTE = 173;
    internal const int VK_VOLUME_DOWN = 174;
    internal const int VK_VOLUME_UP = 175;
    internal const int VK_MEDIA_NEXT_TRACK = 176 /*0xB0*/;
    internal const int VK_MEDIA_PREV_TRACK = 177;
    internal const int VK_MEDIA_STOP = 178;
    internal const int VK_MEDIA_PLAY_PAUSE = 179;
    internal const int VK_LAUNCH_MAIL = 180;
    internal const int VK_LAUNCH_MEDIA_SELECT = 181;
    internal const int VK_LAUNCH_APP1 = 182;
    internal const int VK_LAUNCH_APP2 = 183;
    internal const int VK_OEM_1 = 186;
    internal const int VK_OEM_PLUS = 187;
    internal const int VK_OEM_COMMA = 188;
    internal const int VK_OEM_MINUS = 189;
    internal const int VK_OEM_PERIOD = 190;
    internal const int VK_OEM_2 = 191;
    internal const int VK_OEM_3 = 192 /*0xC0*/;
    internal const int VK_GAMEPAD_A = 195;
    internal const int VK_GAMEPAD_B = 196;
    internal const int VK_GAMEPAD_X = 197;
    internal const int VK_GAMEPAD_Y = 198;
    internal const int VK_GAMEPAD_RIGHT_SHOULDER = 199;
    internal const int VK_GAMEPAD_LEFT_SHOULDER = 200;
    internal const int VK_GAMEPAD_LEFT_TRIGGER = 201;
    internal const int VK_GAMEPAD_RIGHT_TRIGGER = 202;
    internal const int VK_GAMEPAD_DPAD_UP = 203;
    internal const int VK_GAMEPAD_DPAD_DOWN = 204;
    internal const int VK_GAMEPAD_DPAD_LEFT = 205;
    internal const int VK_GAMEPAD_DPAD_RIGHT = 206;
    internal const int VK_GAMEPAD_MENU = 207;
    internal const int VK_GAMEPAD_VIEW = 208 /*0xD0*/;
    internal const int VK_GAMEPAD_LEFT_THUMBSTICK_BUTTON = 209;
    internal const int VK_GAMEPAD_RIGHT_THUMBSTICK_BUTTON = 210;
    internal const int VK_GAMEPAD_LEFT_THUMBSTICK_UP = 211;
    internal const int VK_GAMEPAD_LEFT_THUMBSTICK_DOWN = 212;
    internal const int VK_GAMEPAD_LEFT_THUMBSTICK_RIGHT = 213;
    internal const int VK_GAMEPAD_LEFT_THUMBSTICK_LEFT = 214;
    internal const int VK_GAMEPAD_RIGHT_THUMBSTICK_UP = 215;
    internal const int VK_GAMEPAD_RIGHT_THUMBSTICK_DOWN = 216;
    internal const int VK_GAMEPAD_RIGHT_THUMBSTICK_RIGHT = 217;
    internal const int VK_GAMEPAD_RIGHT_THUMBSTICK_LEFT = 218;
    internal const int VK_OEM_4 = 219;
    internal const int VK_OEM_5 = 220;
    internal const int VK_OEM_6 = 221;
    internal const int VK_OEM_7 = 222;
    internal const int VK_OEM_8 = 223;
    internal const int VK_OEM_AX = 225;
    internal const int VK_OEM_102 = 226;
    internal const int VK_PROCESSKEY = 229;
    internal const int VK_ICO_CLEAR = 230;
    internal const int VK_PACKET = 231;
    internal const int VK_OEM_RESET = 233;
    internal const int VK_OEM_JUMP = 234;
    internal const int VK_OEM_PA1 = 235;
    internal const int VK_OEM_PA2 = 236;
    internal const int VK_OEM_PA3 = 237;
    internal const int VK_OEM_WSCTRL = 238;
    internal const int VK_OEM_CUSEL = 239;
    internal const int VK_OEM_ATTN = 240 /*0xF0*/;
    internal const int VK_OEM_FINISH = 241;
    internal const int VK_OEM_COPY = 242;
    internal const int VK_OEM_AUTO = 243;
    internal const int VK_OEM_ENLW = 244;
    internal const int VK_OEM_BACKTAB = 245;
    internal const int VK_ATTN = 246;
    internal const int VK_CRSEL = 247;
    internal const int VK_EXSEL = 248;
    internal const int VK_EREOF = 249;
    internal const int VK_PLAY = 250;
    internal const int VK_ZOOM = 251;
    internal const int VK_NONAME = 252;
    internal const int VK_PA1 = 253;
    internal const int VK_OEM_CLEAR = 254;
  }
}
