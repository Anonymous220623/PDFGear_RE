// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Native.KernelApi
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.Pdf.Native;

internal sealed class KernelApi
{
  private KernelApi() => throw new NotImplementedException();

  [DllImport("kernel32.dll")]
  internal static extern uint GetLastError();

  [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
  internal static extern bool GetStringTypeExW(
    uint Locale,
    StringInfoType dwInfoType,
    string lpSrcStr,
    int cchSrc,
    [Out] ushort[] lpCharType);

  [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
  internal static extern bool FileTimeToSystemTime(IntPtr lpFileTime, ref SystemTime lpSystemTime);

  [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
  internal static extern uint FormatMessage(
    FormatMessageFlags dwFlags,
    IntPtr lpSource,
    uint messageId,
    uint dwLanguageId,
    IntPtr lpBuffer,
    uint nSize,
    IntPtr Arguments);
}
