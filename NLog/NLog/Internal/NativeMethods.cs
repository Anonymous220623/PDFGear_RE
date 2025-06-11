// Decompiled with JetBrains decompiler
// Type: NLog.Internal.NativeMethods
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace NLog.Internal;

internal static class NativeMethods
{
  [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
  [return: MarshalAs(UnmanagedType.Bool)]
  internal static extern bool LogonUser(
    string pszUsername,
    string pszDomain,
    string pszPassword,
    int dwLogonType,
    int dwLogonProvider,
    out IntPtr phToken);

  [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
  [return: MarshalAs(UnmanagedType.Bool)]
  internal static extern bool CloseHandle(IntPtr handle);

  [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  [return: MarshalAs(UnmanagedType.Bool)]
  internal static extern bool DuplicateToken(
    IntPtr existingTokenHandle,
    int impersonationLevel,
    out IntPtr duplicateTokenHandle);

  [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
  internal static extern void OutputDebugString(string message);

  [DllImport("kernel32.dll")]
  [return: MarshalAs(UnmanagedType.Bool)]
  internal static extern bool QueryPerformanceCounter(out ulong lpPerformanceCount);

  [DllImport("kernel32.dll")]
  [return: MarshalAs(UnmanagedType.Bool)]
  internal static extern bool QueryPerformanceFrequency(out ulong lpPerformanceFrequency);

  [DllImport("kernel32.dll")]
  internal static extern int GetCurrentProcessId();

  [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
  internal static extern uint GetModuleFileName(
    [In] IntPtr hModule,
    [Out] StringBuilder lpFilename,
    [MarshalAs(UnmanagedType.U4), In] int nSize);
}
