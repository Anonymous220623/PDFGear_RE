// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.BlurWindow
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Tools;
using HandyControl.Tools.Helper;
using HandyControl.Tools.Interop;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace HandyControl.Controls;

public class BlurWindow : Window
{
  protected override void OnSourceInitialized(EventArgs e)
  {
    base.OnSourceInitialized(e);
    if (!(SystemHelper.GetSystemVersionInfo() >= SystemVersionInfo.Windows10_1903))
      return;
    this.GetHwndSource()?.AddHook(new System.Windows.Interop.HwndSourceHook(this.HwndSourceHook));
  }

  private IntPtr HwndSourceHook(
    IntPtr hwnd,
    int msg,
    IntPtr wparam,
    IntPtr lparam,
    ref bool handled)
  {
    switch (msg)
    {
      case 561:
        BlurWindow.EnableBlur((Window) this, false);
        break;
      case 562:
        BlurWindow.EnableBlur((Window) this, true);
        break;
    }
    return IntPtr.Zero;
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    BlurWindow.EnableBlur((Window) this, true);
  }

  private static void EnableBlur(Window window, bool isEnabled)
  {
    SystemVersionInfo systemVersionInfo = SystemHelper.GetSystemVersionInfo();
    InteropValues.ACCENTPOLICY structure = new InteropValues.ACCENTPOLICY();
    int cb = Marshal.SizeOf<InteropValues.ACCENTPOLICY>(structure);
    structure.AccentFlags = 2;
    structure.AccentState = !isEnabled ? InteropValues.ACCENTSTATE.ACCENT_ENABLE_BLURBEHIND : (!(systemVersionInfo >= SystemVersionInfo.Windows10_1809) ? (!(systemVersionInfo >= SystemVersionInfo.Windows10) ? InteropValues.ACCENTSTATE.ACCENT_ENABLE_TRANSPARENTGRADIENT : InteropValues.ACCENTSTATE.ACCENT_ENABLE_BLURBEHIND) : InteropValues.ACCENTSTATE.ACCENT_ENABLE_ACRYLICBLURBEHIND);
    structure.GradientColor = HandyControl.Tools.ResourceHelper.GetResource<uint>("BlurGradientValue");
    IntPtr num = Marshal.AllocHGlobal(cb);
    Marshal.StructureToPtr<InteropValues.ACCENTPOLICY>(structure, num, false);
    InteropValues.WINCOMPATTRDATA data = new InteropValues.WINCOMPATTRDATA()
    {
      Attribute = InteropValues.WINDOWCOMPOSITIONATTRIB.WCA_ACCENT_POLICY,
      DataSize = cb,
      Data = num
    };
    InteropMethods.Gdip.SetWindowCompositionAttribute(WindowHelper.GetHandle(window), ref data);
    Marshal.FreeHGlobal(num);
  }
}
