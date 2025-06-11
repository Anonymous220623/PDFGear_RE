// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.KeyCode
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Windows.Input;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class KeyCode
{
  public static string KeycodeToChar(Key key, bool isShift)
  {
    switch (key)
    {
      case Key.Space:
        return " ";
      case Key.D0:
        return !isShift ? "0" : ")";
      case Key.D1:
        return !isShift ? "1" : "!";
      case Key.D2:
        return !isShift ? "2" : "@";
      case Key.D3:
        return !isShift ? "3" : "#";
      case Key.D4:
        return !isShift ? "4" : "$";
      case Key.D5:
        return !isShift ? "5" : "%";
      case Key.D6:
        return !isShift ? "6" : "^";
      case Key.D7:
        return !isShift ? "7" : "&";
      case Key.D8:
        return !isShift ? "8" : "*";
      case Key.D9:
        return !isShift ? "9" : "(";
      case Key.Multiply:
        return "*";
      case Key.Add:
        return "+";
      case Key.Separator:
        return "-";
      case Key.Subtract:
        return "-";
      case Key.Decimal:
        return ".";
      case Key.Divide:
        return "/";
      case Key.Oem1:
        return !isShift ? ";" : ":";
      case Key.OemPlus:
        return !isShift ? "=" : "+";
      case Key.OemComma:
        return !isShift ? "," : "<";
      case Key.OemMinus:
        return !isShift ? "-" : "_";
      case Key.OemPeriod:
        return !isShift ? "." : ">";
      case Key.Oem2:
        return !isShift ? "/" : "?";
      case Key.Oem3:
        return !isShift ? "`" : "~";
      case Key.Oem4:
        return !isShift ? "[" : "{";
      case Key.Oem5:
        return !isShift ? "\\" : "|";
      case Key.Oem6:
        return !isShift ? "]" : "}";
      case Key.Oem7:
        return !isShift ? "'" : "\"";
      default:
        return key.ToString();
    }
  }
}
