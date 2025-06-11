// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Controls.KeyboardHelper
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Windows.Input;

#nullable disable
namespace Syncfusion.Windows.Controls;

internal static class KeyboardHelper
{
  public static void GetMetaKeyState(out bool ctrl, out bool shift)
  {
    ctrl = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
    shift = (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
  }
}
