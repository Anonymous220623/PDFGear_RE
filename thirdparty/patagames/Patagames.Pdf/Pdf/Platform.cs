// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Platform
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Activation;
using Patagames.Pdf.Enums;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace Patagames.Pdf;

internal class Platform
{
  private const int RTLD_LAZY = 1;
  private const int RTLD_NOW = 2;
  private const int RTLD_LOCAL = 4;
  private const int RTLD_GLOBAL = 8;
  private const int RTLD_NOLOAD = 16 /*0x10*/;
  private const int RTLD_NODELETE = 128 /*0x80*/;
  private const int RTLD_FIRST = 256 /*0x0100*/;
  private static Platforms? _currentPlatform;

  public static Platforms CurrentPlatform
  {
    get
    {
      if (Platform._currentPlatform.HasValue)
        return Platform._currentPlatform.Value;
      string environmentVariable = Environment.GetEnvironmentVariable("windir");
      if (!string.IsNullOrEmpty(environmentVariable) && environmentVariable.Contains("\\") && Directory.Exists(environmentVariable))
        Platform._currentPlatform = new Platforms?(Platforms.Windows);
      else if (File.Exists("/proc/sys/kernel/ostype"))
      {
        if (File.ReadAllText("/proc/sys/kernel/ostype").StartsWith("Linux", StringComparison.OrdinalIgnoreCase))
          Platform._currentPlatform = new Platforms?(Platforms.Linux);
      }
      else if (File.Exists("/System/Library/CoreServices/SystemVersion.plist"))
        Platform._currentPlatform = new Platforms?(Platforms.OSX);
      if (Platform._currentPlatform.HasValue)
        return Platform._currentPlatform.Value;
      switch (Environment.OSVersion.Platform)
      {
        case PlatformID.Unix:
        case (PlatformID) 128 /*0x80*/:
          Platform._currentPlatform = new Platforms?(Platforms.Linux);
          break;
        case PlatformID.MacOSX:
          Platform._currentPlatform = new Platforms?(Platforms.OSX);
          break;
        default:
          Platform._currentPlatform = new Platforms?(Platforms.Windows);
          break;
      }
      return Platform._currentPlatform.Value;
    }
    set => Platform._currentPlatform = new Platforms?(value);
  }

  public static string PdfiumDllName
  {
    get => Platform.CurrentPlatform == Platforms.Windows ? "pdfium.dll" : "libpdfium.dylib";
  }

  public static string IcuDllName
  {
    get => Platform.CurrentPlatform == Platforms.Windows ? "icudt.dll" : "libicudt.dylib";
  }

  [DllImport("User32.dll", EntryPoint = "ToAscii", SetLastError = true)]
  private static extern int ToAsciiWin(
    int uVirtKey,
    int uScanCode,
    byte[] lpbKeyState,
    byte[] lpChar,
    int uFlags);

  [DllImport("User32.dll", EntryPoint = "ToUnicode", SetLastError = true)]
  private static extern int ToUnicodeWin(
    int uVirtKey,
    int uScanCode,
    byte[] lpbKeyState,
    byte[] lpChar,
    int buflen,
    int uFlags);

  [DllImport("User32.dll", EntryPoint = "GetKeyboardState", SetLastError = true)]
  private static extern int GetKeyboardStateWin(byte[] pbKeyState);

  [DllImport("Kernel32.dll", EntryPoint = "LoadLibraryW", SetLastError = true)]
  private static extern IntPtr LoadLibraryWin([MarshalAs(UnmanagedType.LPWStr)] string path);

  [DllImport("libdl.so", EntryPoint = "dlopen")]
  private static extern IntPtr dlopenLinux(string filename, int flags);

  [DllImport("libdl.dylib", EntryPoint = "dlopen")]
  private static extern IntPtr dlopenOCX(string filename, int flags);

  [DllImport("Kernel32.dll", EntryPoint = "FreeLibrary", SetLastError = true)]
  private static extern bool FreeLibraryWin(IntPtr handle);

  [DllImport("libdl.so", EntryPoint = "dlclose")]
  private static extern int dlcloseLinux(IntPtr handle);

  [DllImport("libdl.dylib", EntryPoint = "dlclose")]
  private static extern int dlcloseOCX(IntPtr handle);

  [DllImport("Kernel32.dll", EntryPoint = "GetModuleHandleA", SetLastError = true)]
  private static extern IntPtr GetModuleHandleWin([MarshalAs(UnmanagedType.LPStr)] string name);

  [DllImport("Gdi32.dll", EntryPoint = "SetWorldTransform", SetLastError = true)]
  private static extern void SetWorldTransformWin(IntPtr hdc, [MarshalAs(UnmanagedType.LPStruct)] FS_MATRIX pMatrix);

  [DllImport("Gdi32.dll", EntryPoint = "IntersectClipRect", SetLastError = true)]
  private static extern int IntersectClipRectWin(
    IntPtr hdc,
    int left,
    int top,
    int right,
    int bottom);

  [DllImport("kernel32.dll", EntryPoint = "SetDllDirectoryW", CharSet = CharSet.Unicode, SetLastError = true)]
  [return: MarshalAs(UnmanagedType.Bool)]
  private static extern bool SetDllDirectoryWin(string lpPathName);

  [DllImport("kernel32.dll", EntryPoint = "GetModuleFileNameW", SetLastError = true)]
  private static extern int GetModuleFileNameWin(
    [In] IntPtr hModule,
    [MarshalAs(UnmanagedType.LPWStr), Out] StringBuilder lpFilename,
    [MarshalAs(UnmanagedType.U4), In] int nSize);

  [DllImport("libdl.dylib", EntryPoint = "dladdr")]
  private static extern int dladdrOCX(IntPtr hModule, out Platform.Dl_info info);

  [DllImport("Kernel32.dll", EntryPoint = "GetVolumeInformationW", CharSet = CharSet.Unicode, SetLastError = true)]
  [return: MarshalAs(UnmanagedType.Bool)]
  private static extern bool GetVolumeInformationWin(
    string rootPathName,
    string volumeNameBuffer,
    int volumeNameSize,
    out uint volumeSerialNumber,
    out uint maximumComponentLength,
    out FileSystemFeature fileSystemFlags,
    string fileSystemNameBuffer,
    int nFileSystemNameSize);

  [DllImport("Advapi32.dll", EntryPoint = "RegOpenKeyExW", CharSet = CharSet.Unicode, SetLastError = true)]
  private static extern uint RegOpenKeyExWin(
    UIntPtr hKey,
    string lpSubKey,
    uint ulOptions,
    int samDesired,
    out int phkResult);

  [DllImport("Advapi32.dll", EntryPoint = "RegCloseKey")]
  private static extern uint RegCloseKeyWin(int hKey);

  [DllImport("advapi32.dll", EntryPoint = "RegQueryValueEx")]
  private static extern int RegQueryValueExWin(
    int hKey,
    string lpValueName,
    int lpReserved,
    ref uint lpType,
    StringBuilder lpData,
    ref uint lpcbData);

  [DllImport("advapi32.dll", EntryPoint = "RegQueryValueEx")]
  private static extern int RegQueryValueExWin(
    int hKey,
    string lpValueName,
    int lpReserved,
    ref uint lpType,
    byte[] lpData,
    ref uint lpcbData);

  public static IntPtr LoadLibrary(string path)
  {
    if ((path ?? "").Trim() == "")
      return IntPtr.Zero;
    switch (Platform.CurrentPlatform)
    {
      case Platforms.Linux:
        return Platform.dlopenLinux(path, 2);
      case Platforms.OSX:
        return Platform.dlopenOCX(path, 2);
      default:
        return Platform.LoadLibraryWin(path);
    }
  }

  public static bool FreeLibrary(IntPtr handle)
  {
    if (handle == IntPtr.Zero)
      return false;
    switch (Platform.CurrentPlatform)
    {
      case Platforms.Linux:
        return Platform.dlcloseLinux(handle) == 0;
      case Platforms.OSX:
        return Platform.dlcloseOCX(handle) == 0;
      default:
        return Platform.FreeLibraryWin(handle);
    }
  }

  public static IntPtr GetModuleHandle(string path)
  {
    IntPtr zero = IntPtr.Zero;
    switch (Platform.CurrentPlatform)
    {
      case Platforms.Linux:
        IntPtr handle1 = Platform.dlopenLinux(path, 18);
        if (handle1 != IntPtr.Zero)
          Platform.dlcloseLinux(handle1);
        return handle1;
      case Platforms.OSX:
        IntPtr handle2 = Platform.dlopenOCX(path, 18);
        if (handle2 != IntPtr.Zero)
          Platform.dlcloseOCX(handle2);
        return handle2;
      default:
        return Platform.GetModuleHandleWin(path);
    }
  }

  public static bool IsLibraryLoaded(string path)
  {
    switch (Platform.CurrentPlatform)
    {
      case Platforms.Linux:
      case Platforms.OSX:
        return Platform.GetModuleHandle(path) != IntPtr.Zero;
      default:
        return Platform.GetModuleHandleWin(path) != IntPtr.Zero;
    }
  }

  public static bool SetDllDirectory(string path) => Platform.SetDllDirectoryWin(path);

  public static string GetModuleFileName(IntPtr hModule)
  {
    switch (Platform.CurrentPlatform)
    {
      case Platforms.Linux:
      case Platforms.OSX:
        Platform.dladdrOCX(hModule, out Platform.Dl_info _);
        return (string) null;
      default:
        StringBuilder lpFilename = new StringBuilder(502);
        int moduleFileNameWin = Platform.GetModuleFileNameWin(hModule, lpFilename, 502);
        return moduleFileNameWin > 0 && moduleFileNameWin < 502 ? lpFilename.ToString() : (string) null;
    }
  }

  public static void SetWorldTransform(IntPtr hdc, FS_MATRIX pMatrix)
  {
    Platform.SetWorldTransformWin(hdc, pMatrix);
  }

  public static int IntersectClipRect(IntPtr hdc, int left, int top, int right, int bottom)
  {
    return Platform.IntersectClipRectWin(hdc, left, top, right, bottom);
  }

  public static int ToAscii(
    int uVirtKey,
    int uScanCode,
    byte[] lpbKeyState,
    byte[] lpChar,
    int uFlags)
  {
    return Platform.ToAsciiWin(uVirtKey, uScanCode, lpbKeyState, lpChar, uFlags);
  }

  public static int ToUnicode(
    int uVirtKey,
    int uScanCode,
    byte[] lpbKeyState,
    byte[] lpChar,
    int buflen,
    int uFlags)
  {
    return Platform.ToUnicodeWin(uVirtKey, uScanCode, lpbKeyState, lpChar, buflen, uFlags);
  }

  public static int GetKeyboardState(byte[] pbKeyState) => Platform.GetKeyboardStateWin(pbKeyState);

  public static bool GetVolumeInformation(
    string rootPathName,
    string volumeNameBuffer,
    int volumeNameSize,
    out uint volumeSerialNumber,
    out uint maximumComponentLength,
    out FileSystemFeature fileSystemFlags,
    string fileSystemNameBuffer,
    int nFileSystemNameSize)
  {
    return Platform.GetVolumeInformationWin(rootPathName, volumeNameBuffer, volumeNameSize, out volumeSerialNumber, out maximumComponentLength, out fileSystemFlags, fileSystemNameBuffer, nFileSystemNameSize);
  }

  public static uint RegOpenKeyEx(
    UIntPtr hKey,
    string lpSubKey,
    uint ulOptions,
    int samDesired,
    out int phkResult)
  {
    if (Platform.CurrentPlatform == Platforms.Windows)
      return Platform.RegOpenKeyExWin(hKey, lpSubKey, ulOptions, samDesired, out phkResult);
    phkResult = 0;
    return (uint) sbyte.MaxValue;
  }

  public static uint RegCloseKey(int hKey)
  {
    return Platform.CurrentPlatform == Platforms.Windows ? Platform.RegCloseKeyWin(hKey) : (uint) sbyte.MaxValue;
  }

  public static int RegQueryValueEx(
    int hKey,
    string lpValueName,
    int lpReserved,
    ref uint lpType,
    StringBuilder lpData,
    ref uint lpcbData)
  {
    return Platform.CurrentPlatform == Platforms.Windows ? Platform.RegQueryValueExWin(hKey, lpValueName, lpReserved, ref lpType, lpData, ref lpcbData) : (int) sbyte.MaxValue;
  }

  public static int RegQueryValueEx(
    int hKey,
    string lpValueName,
    int lpReserved,
    ref uint lpType,
    byte[] lpData,
    ref uint lpcbData)
  {
    return Platform.CurrentPlatform == Platforms.Windows ? Platform.RegQueryValueExWin(hKey, lpValueName, lpReserved, ref lpType, lpData, ref lpcbData) : (int) sbyte.MaxValue;
  }

  public static string GetUnicode(byte[] data, bool isNullTerminatedData = true)
  {
    return Encoding.Unicode.GetString(data, 0, data.Length - (isNullTerminatedData ? 2 : 0));
  }

  private struct Dl_info
  {
    [MarshalAs(UnmanagedType.LPStr)]
    private string dli_fname;
    private IntPtr dli_fbase;
    [MarshalAs(UnmanagedType.LPStr)]
    private string dli_sname;
    private IntPtr dli_saddr;
  }
}
