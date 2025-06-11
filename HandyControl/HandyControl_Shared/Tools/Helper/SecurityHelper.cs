// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.SecurityHelper
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.Security;
using System.Security.Permissions;

#nullable disable
namespace HandyControl.Tools;

internal class SecurityHelper
{
  private static UIPermission _allWindowsUIPermission;

  [SecurityCritical]
  internal static void DemandUIWindowPermission()
  {
    if (SecurityHelper._allWindowsUIPermission == null)
      SecurityHelper._allWindowsUIPermission = new UIPermission(UIPermissionWindow.AllWindows);
    SecurityHelper._allWindowsUIPermission.Demand();
  }
}
