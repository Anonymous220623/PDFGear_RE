// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.MessageBoxHelper.ButtonTextHelper
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

#nullable disable
namespace CommomLib.Commom.MessageBoxHelper;

public static class ButtonTextHelper
{
  internal const uint IDS_OK = 800;
  internal const uint IDS_CANCEL = 801;
  internal const uint IDS_YES = 805;
  internal const uint IDS_NO = 806;
  private const string OK_STRING = "OK";
  private const string CANCEL_STRING = "Cancel";
  private const string YES_STRING = "&Yes";
  private const string NO_STRING = "&No";
  private static Dictionary<ushort, Dictionary<uint, string>> cache = new Dictionary<ushort, Dictionary<uint, string>>();

  public static string GetText(MessageBoxResult res, CultureInfo cultureInfo = null, bool enableAccessKey = false)
  {
    if (cultureInfo == null || cultureInfo.LCID == CultureInfo.InvariantCulture.LCID)
      cultureInfo = CultureInfo.CurrentUICulture;
    ButtonTextHelper.TryInitLangCache((ushort) cultureInfo.LCID);
    Dictionary<uint, string> dictionary = ButtonTextHelper.cache[(ushort) cultureInfo.LCID];
    uint key;
    switch (res)
    {
      case MessageBoxResult.Cancel:
        key = 801U;
        break;
      case MessageBoxResult.Yes:
        key = 805U;
        break;
      case MessageBoxResult.No:
        key = 806U;
        break;
      default:
        key = 800U;
        break;
    }
    string str;
    if (!dictionary.TryGetValue(key, out str))
      return string.Empty;
    return enableAccessKey ? str.Replace("&", "_") : str;
  }

  private static void TryInitLangCache(ushort lcid)
  {
    Dictionary<uint, string> dictionary1;
    if (ButtonTextHelper.cache.TryGetValue(lcid, out dictionary1))
      return;
    lock (typeof (ButtonTextHelper))
    {
      if (ButtonTextHelper.cache.TryGetValue(lcid, out dictionary1))
        return;
      Dictionary<uint, string> dictionary2 = new Dictionary<uint, string>();
      ButtonTextHelper.cache[lcid] = dictionary2;
      IntPtr user32Module = ButtonTextHelper.GetUser32Module();
      try
      {
        dictionary2[800U] = ButtonTextHelper.GetStringResource(user32Module, 800U, lcid);
        dictionary2[801U] = ButtonTextHelper.GetStringResource(user32Module, 801U, lcid);
        dictionary2[805U] = ButtonTextHelper.GetStringResource(user32Module, 805U, lcid);
        dictionary2[806U] = ButtonTextHelper.GetStringResource(user32Module, 806U, lcid);
      }
      finally
      {
        ButtonTextHelper.FreeLibrary(user32Module);
      }
    }
  }

  private static string GetStringResource(IntPtr hInstance, uint uiStringID, ushort lcid)
  {
    string stringResource = "";
    try
    {
      stringResource = ButtonTextHelper.GetStringResourceCore(hInstance, uiStringID, lcid);
    }
    catch
    {
    }
    if (string.IsNullOrEmpty(stringResource))
      stringResource = ButtonTextHelper.GetStringResourceFallback(hInstance, uiStringID);
    return stringResource;
  }

  private static string GetStringResourceCore(IntPtr hInstance, uint uiStringID, ushort lcid)
  {
    IntPtr resourceEx = ButtonTextHelper.FindResourceEx(hInstance, 6U, uiStringID / 16U /*0x10*/ + 1U, lcid);
    if (resourceEx != IntPtr.Zero)
    {
      uint size = ButtonTextHelper.SizeofResource(hInstance, resourceEx);
      IntPtr hRes = ButtonTextHelper.LoadResource(hInstance, resourceEx);
      if (hRes != IntPtr.Zero)
        return ButtonTextHelper.GetResourceStringFromBundle(uiStringID, hRes, (int) size);
    }
    return "";
  }

  private static string GetResourceStringFromBundle(uint uId, IntPtr hRes, int size)
  {
    uint num1 = uId & 15U;
    byte[] numArray = new byte[size];
    Marshal.Copy(hRes, numArray, 0, size);
    int index1 = 0;
    for (int index2 = 0; (long) index2 < (long) num1; ++index2)
    {
      byte num2 = numArray[index1];
      index1 += ((int) num2 + 1) * 2;
    }
    byte num3 = numArray[index1];
    return Encoding.Unicode.GetString(numArray.AsSpan<byte>(index1 + 2, (int) num3 * 2).ToArray());
  }

  private static string GetStringResourceFallback(IntPtr hInstance, uint uiStringID)
  {
    string resourceFallback = "";
    try
    {
      StringBuilder lpBuffer = new StringBuilder((int) byte.MaxValue);
      ButtonTextHelper.LoadString(hInstance, uiStringID, lpBuffer, lpBuffer.Capacity + 1);
      resourceFallback = lpBuffer.ToString();
    }
    catch
    {
    }
    if (string.IsNullOrEmpty(resourceFallback))
    {
      switch (uiStringID)
      {
        case 800:
          resourceFallback = "OK";
          break;
        case 801:
          resourceFallback = "Cancel";
          break;
        case 805:
          resourceFallback = "&Yes";
          break;
        case 806:
          resourceFallback = "&No";
          break;
      }
    }
    return resourceFallback;
  }

  private static IntPtr GetUser32Module()
  {
    return ButtonTextHelper.LoadLibraryEx("user32.dll", IntPtr.Zero, ButtonTextHelper.LoadLibraryFlags.LOAD_LIBRARY_AS_DATAFILE);
  }

  [DllImport("kernel32.dll", SetLastError = true)]
  private static extern IntPtr LoadLibraryEx(
    string lpFileName,
    IntPtr hReservedNull,
    ButtonTextHelper.LoadLibraryFlags dwFlags);

  [DllImport("kernel32.dll", SetLastError = true)]
  [return: MarshalAs(UnmanagedType.Bool)]
  private static extern bool FreeLibrary(IntPtr hModule);

  [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
  private static extern IntPtr FindResourceEx(
    IntPtr hModule,
    uint lpType,
    uint lpName,
    ushort wLanguage);

  [DllImport("kernel32.dll", SetLastError = true)]
  private static extern IntPtr LoadResource(IntPtr hModule, IntPtr hResInfo);

  [DllImport("kernel32.dll", SetLastError = true)]
  private static extern uint SizeofResource(IntPtr hModule, IntPtr hResInfo);

  [DllImport("user32.dll", CharSet = CharSet.Auto)]
  private static extern int LoadString(
    IntPtr hInstance,
    uint uID,
    StringBuilder lpBuffer,
    int nBufferMax);

  [Flags]
  private enum LoadLibraryFlags : uint
  {
    None = 0,
    DONT_RESOLVE_DLL_REFERENCES = 1,
    LOAD_IGNORE_CODE_AUTHZ_LEVEL = 16, // 0x00000010
    LOAD_LIBRARY_AS_DATAFILE = 2,
    LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE = 64, // 0x00000040
    LOAD_LIBRARY_AS_IMAGE_RESOURCE = 32, // 0x00000020
    LOAD_LIBRARY_SEARCH_APPLICATION_DIR = 512, // 0x00000200
    LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 4096, // 0x00001000
    LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR = 256, // 0x00000100
    LOAD_LIBRARY_SEARCH_SYSTEM32 = 2048, // 0x00000800
    LOAD_LIBRARY_SEARCH_USER_DIRS = 1024, // 0x00000400
    LOAD_WITH_ALTERED_SEARCH_PATH = 8,
  }
}
