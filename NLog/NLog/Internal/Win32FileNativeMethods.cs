// Decompiled with JetBrains decompiler
// Type: NLog.Internal.Win32FileNativeMethods
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using Microsoft.Win32.SafeHandles;
using NLog.Targets;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace NLog.Internal;

internal static class Win32FileNativeMethods
{
  public const int FILE_SHARE_READ = 1;
  public const int FILE_SHARE_WRITE = 2;
  public const int FILE_SHARE_DELETE = 4;

  [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
  public static extern SafeFileHandle CreateFile(
    string lpFileName,
    Win32FileNativeMethods.FileAccess dwDesiredAccess,
    int dwShareMode,
    IntPtr lpSecurityAttributes,
    Win32FileNativeMethods.CreationDisposition dwCreationDisposition,
    Win32FileAttributes dwFlagsAndAttributes,
    IntPtr hTemplateFile);

  [Flags]
  public enum FileAccess : uint
  {
    GenericRead = 2147483648, // 0x80000000
    GenericWrite = 1073741824, // 0x40000000
    GenericExecute = 536870912, // 0x20000000
    GenericAll = 268435456, // 0x10000000
  }

  public enum CreationDisposition : uint
  {
    New = 1,
    CreateAlways = 2,
    OpenExisting = 3,
    OpenAlways = 4,
    TruncateExisting = 5,
  }
}
