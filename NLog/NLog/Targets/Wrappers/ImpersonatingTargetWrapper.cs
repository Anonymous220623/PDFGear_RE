// Decompiled with JetBrains decompiler
// Type: NLog.Targets.Wrappers.ImpersonatingTargetWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;

#nullable disable
namespace NLog.Targets.Wrappers;

[SecuritySafeCritical]
[Target("ImpersonatingWrapper", IsWrapper = true)]
public class ImpersonatingTargetWrapper : WrapperTargetBase
{
  private WindowsIdentity _newIdentity;
  private IntPtr _duplicateTokenHandle = IntPtr.Zero;

  public ImpersonatingTargetWrapper()
    : this((Target) null)
  {
  }

  public ImpersonatingTargetWrapper(string name, Target wrappedTarget)
    : this(wrappedTarget)
  {
    this.Name = name;
  }

  public ImpersonatingTargetWrapper(Target wrappedTarget)
  {
    this.Domain = ".";
    this.LogOnType = SecurityLogOnType.Interactive;
    this.LogOnProvider = LogOnProviderType.Default;
    this.ImpersonationLevel = SecurityImpersonationLevel.Impersonation;
    this.WrappedTarget = wrappedTarget;
  }

  public string UserName { get; set; }

  public string Password { get; set; }

  [DefaultValue(".")]
  public string Domain { get; set; }

  public SecurityLogOnType LogOnType { get; set; }

  public LogOnProviderType LogOnProvider { get; set; }

  public SecurityImpersonationLevel ImpersonationLevel { get; set; }

  [DefaultValue(false)]
  public bool RevertToSelf { get; set; }

  protected override void InitializeTarget()
  {
    if (!this.RevertToSelf)
      this._newIdentity = this.CreateWindowsIdentity(out this._duplicateTokenHandle);
    using (this.DoImpersonate())
      base.InitializeTarget();
  }

  protected override void CloseTarget()
  {
    using (this.DoImpersonate())
      base.CloseTarget();
    if (this._duplicateTokenHandle != IntPtr.Zero)
    {
      NLog.Internal.NativeMethods.CloseHandle(this._duplicateTokenHandle);
      this._duplicateTokenHandle = IntPtr.Zero;
    }
    if (this._newIdentity == null)
      return;
    this._newIdentity.Dispose();
    this._newIdentity = (WindowsIdentity) null;
  }

  protected override void Write(AsyncLogEventInfo logEvent)
  {
    using (this.DoImpersonate())
      this.WrappedTarget.WriteAsyncLogEvent(logEvent);
  }

  [Obsolete("Instead override Write(IList<AsyncLogEventInfo> logEvents. Marked obsolete on NLog 4.5")]
  protected override void Write(AsyncLogEventInfo[] logEvents)
  {
    this.Write((IList<AsyncLogEventInfo>) logEvents);
  }

  protected override void Write(IList<AsyncLogEventInfo> logEvents)
  {
    using (this.DoImpersonate())
      this.WrappedTarget.WriteAsyncLogEvents(logEvents);
  }

  protected override void FlushAsync(AsyncContinuation asyncContinuation)
  {
    using (this.DoImpersonate())
      this.WrappedTarget.Flush(asyncContinuation);
  }

  private IDisposable DoImpersonate()
  {
    return this.RevertToSelf ? (IDisposable) new ImpersonatingTargetWrapper.ContextReverter(WindowsIdentity.Impersonate(IntPtr.Zero)) : (IDisposable) new ImpersonatingTargetWrapper.ContextReverter(this._newIdentity.Impersonate());
  }

  private WindowsIdentity CreateWindowsIdentity(out IntPtr handle)
  {
    IntPtr phToken;
    if (!NLog.Internal.NativeMethods.LogonUser(this.UserName, this.Domain, this.Password, (int) this.LogOnType, (int) this.LogOnProvider, out phToken))
      throw Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());
    if (!NLog.Internal.NativeMethods.DuplicateToken(phToken, (int) this.ImpersonationLevel, out handle))
    {
      NLog.Internal.NativeMethods.CloseHandle(phToken);
      throw Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());
    }
    NLog.Internal.NativeMethods.CloseHandle(phToken);
    return new WindowsIdentity(handle);
  }

  internal sealed class ContextReverter : IDisposable
  {
    private readonly WindowsImpersonationContext _impersonationContext;

    public ContextReverter(
      WindowsImpersonationContext windowsImpersonationContext)
    {
      this._impersonationContext = windowsImpersonationContext;
    }

    public void Dispose() => this._impersonationContext.Undo();
  }
}
