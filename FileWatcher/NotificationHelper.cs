// Decompiled with JetBrains decompiler
// Type: FileWatcher.NotificationHelper
// Assembly: FileWatcher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 60845E26-CA8C-4ACF-A930-D757CD9B4993
// Assembly location: C:\Program Files\PDFgear\FileWatcher.exe

using CommomLib.Commom;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace FileWatcher;

public static class NotificationHelper
{
  private static readonly Lazy<bool> isFocusAssistSupportedLazy = new Lazy<bool>((Func<bool>) (() => Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version >= new Version(10, 0, 17134, 0)), true);

  public static bool AcceptsNotifications
  {
    get
    {
      UserNotificationState userNotificationState;
      return NotificationHelper.SHQueryUserNotificationState(out userNotificationState) == 0 && userNotificationState == UserNotificationState.AcceptsNotifications;
    }
  }

  public static bool IsFocusAssistSupported => NotificationHelper.isFocusAssistSupportedLazy.Value;

  public static bool IsFocusAssistEnabled
  {
    get
    {
      FocusAssistResult focusAssist = NotificationHelper.FocusAssist;
      return focusAssist != FocusAssistResult.OFF && focusAssist != FocusAssistResult.NOT_SUPPORTED;
    }
  }

  public static FocusAssistResult FocusAssist
  {
    get
    {
      if (!NotificationHelper.IsFocusAssistSupported)
        return FocusAssistResult.NOT_SUPPORTED;
      NotificationHelper.WNF_STATE_NAME structure = new NotificationHelper.WNF_STATE_NAME(2747210869U, 226690622U);
      uint nBufferSize = (uint) Marshal.SizeOf(typeof (IntPtr));
      IntPtr num = IntPtr.Zero;
      try
      {
        num = Marshal.AllocHGlobal(Marshal.SizeOf(typeof (NotificationHelper.WNF_STATE_NAME)));
        Marshal.StructureToPtr<NotificationHelper.WNF_STATE_NAME>(structure, num, false);
        IntPtr pBuffer;
        if (NotificationHelper.NtQueryWnfStateData(num, IntPtr.Zero, IntPtr.Zero, out uint _, out pBuffer, ref nBufferSize) == 0U)
          return (FocusAssistResult) (int) pBuffer;
      }
      catch (Exception ex)
      {
        GAManager2.SendEvent("Exception", nameof (FocusAssist), ex.Message, 1L);
      }
      finally
      {
        if (num != IntPtr.Zero)
          Marshal.FreeHGlobal(num);
      }
      return FocusAssistResult.FAILED;
    }
  }

  [DllImport("ntdll.dll", SetLastError = true)]
  private static extern uint NtQueryWnfStateData(
    IntPtr pStateName,
    IntPtr pTypeId,
    IntPtr pExplicitScope,
    out uint nChangeStamp,
    out IntPtr pBuffer,
    ref uint nBufferSize);

  [DllImport("shell32.dll")]
  private static extern int SHQueryUserNotificationState(
    out UserNotificationState userNotificationState);

  private struct WNF_TYPE_ID
  {
    public Guid TypeId;
  }

  private struct WNF_STATE_NAME
  {
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    public uint[] Data;

    public WNF_STATE_NAME(uint Data1, uint Data2)
      : this()
    {
      this.Data = new uint[2]{ Data1, Data2 };
    }
  }
}
