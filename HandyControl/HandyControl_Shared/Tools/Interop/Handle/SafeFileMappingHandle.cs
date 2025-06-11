// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Interop.SafeFileMappingHandle
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

#nullable disable
namespace HandyControl.Tools.Interop;

internal sealed class SafeFileMappingHandle : SafeHandleZeroOrMinusOneIsInvalid
{
  [SecurityCritical]
  internal SafeFileMappingHandle(IntPtr handle)
    : base(false)
  {
    this.SetHandle(handle);
  }

  [SecurityCritical]
  [SecuritySafeCritical]
  internal SafeFileMappingHandle()
    : base(true)
  {
  }

  public override bool IsInvalid
  {
    [SecurityCritical, SecuritySafeCritical] get => this.handle == IntPtr.Zero;
  }

  [SecurityCritical]
  [SecuritySafeCritical]
  protected override bool ReleaseHandle()
  {
    new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
    try
    {
      return SafeFileMappingHandle.CloseHandleNoThrow(new HandleRef((object) null, this.handle));
    }
    finally
    {
      CodeAccessPermission.RevertAssert();
    }
  }

  [SecurityCritical]
  public static bool CloseHandleNoThrow(HandleRef handle)
  {
    HandleCollector.Remove((IntPtr) handle, CommonHandles.Kernel);
    return InteropMethods.IntCloseHandle(handle);
  }
}
