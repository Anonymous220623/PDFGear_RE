// Decompiled with JetBrains decompiler
// Type: NLog.Internal.FileAppenders.BaseMutexFileAppender
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using JetBrains.Annotations;
using NLog.Common;
using System;
using System.IO;
using System.Security;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;

#nullable disable
namespace NLog.Internal.FileAppenders;

[SecuritySafeCritical]
internal abstract class BaseMutexFileAppender : BaseFileAppender
{
  protected BaseMutexFileAppender(string fileName, ICreateFileParameters createParameters)
    : base(fileName, createParameters)
  {
    if (!createParameters.IsArchivingEnabled || !createParameters.ConcurrentWrites)
      return;
    if (PlatformDetector.SupportsSharableMutex)
      this.ArchiveMutex = this.CreateArchiveMutex();
    else
      InternalLogger.Debug("Mutex for file archive not supported");
  }

  [CanBeNull]
  public Mutex ArchiveMutex { get; private set; }

  private Mutex CreateArchiveMutex()
  {
    try
    {
      return this.CreateSharableMutex("FileArchiveLock");
    }
    catch (Exception ex)
    {
      switch (ex)
      {
        case SecurityException _:
        case UnauthorizedAccessException _:
        case NotSupportedException _:
        case NotImplementedException _:
        case PlatformNotSupportedException _:
          InternalLogger.Warn(ex, "Failed to create global archive mutex: {0}", (object) this.FileName);
          return new Mutex();
        default:
          InternalLogger.Error(ex, "Failed to create global archive mutex: {0}", (object) this.FileName);
          if (!ex.MustBeRethrown())
            return new Mutex();
          throw;
      }
    }
  }

  protected override void Dispose(bool disposing)
  {
    base.Dispose(disposing);
    if (!disposing)
      return;
    this.ArchiveMutex?.Close();
  }

  protected Mutex CreateSharableMutex(string mutexNamePrefix)
  {
    if (!PlatformDetector.SupportsSharableMutex)
      throw new NotSupportedException("Creating Mutex not supported");
    return BaseMutexFileAppender.ForceCreateSharableMutex(this.GetMutexName(mutexNamePrefix));
  }

  internal static Mutex ForceCreateSharableMutex(string name)
  {
    MutexSecurity mutexSecurity = new MutexSecurity();
    SecurityIdentifier identity = new SecurityIdentifier(WellKnownSidType.WorldSid, (SecurityIdentifier) null);
    mutexSecurity.AddAccessRule(new MutexAccessRule((IdentityReference) identity, MutexRights.FullControl, AccessControlType.Allow));
    bool createdNew;
    return new Mutex(false, name, out createdNew, mutexSecurity);
  }

  private string GetMutexName(string mutexNamePrefix)
  {
    string s = Path.GetFullPath(this.FileName).ToLowerInvariant().Replace('\\', '_').Replace('/', '_');
    string mutexName = $"Global\\NLog-File{mutexNamePrefix}-{s}";
    if (mutexName.Length <= 260)
      return mutexName;
    string base64String;
    using (MD5 md5 = MD5.Create())
      base64String = Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(s)));
    string str = $"Global\\NLog-File{mutexNamePrefix}-{base64String}";
    int startIndex = s.Length - (260 - str.Length);
    return str + s.Substring(startIndex);
  }
}
