// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.ShellHelper
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using MS.WindowsAPICodePack.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

#nullable disable
namespace CommomLib.Commom;

public static class ShellHelper
{
  private const string CLSID_TaskbandPin = "90AA3A4E-1CBA-4233-B8BB-535773D48449";
  private const string CLSID_ShellLink = "00021401-0000-0000-C000-000000000046";
  private const string IID_IPinnedList3 = "0DD79AE2-D156-45D4-9EEB-3B549769E940";
  private const string IID_IShellLink = "000214F9-0000-0000-C000-000000000046";
  private static Type CTaskbandPin_Type;
  private static Type CShellLink_Type;
  private static Func<ShellObject, IntPtr> shellObjectPidlGetter;

  public static bool UpdateFilePinState(
    bool pinning,
    string fileName,
    string displayName,
    string amuid)
  {
    if (string.IsNullOrEmpty(fileName))
      return false;
    try
    {
      if (!File.Exists(fileName))
        return false;
      if (string.IsNullOrEmpty(amuid) && string.IsNullOrEmpty(displayName))
      {
        using (ShellFile shellFile = ShellFile.FromFilePath(fileName))
          return ShellHelper.UpdatePinState(pinning, ShellHelper.GetShellObjectPIDL((ShellObject) shellFile));
      }
      if (ShellHelper.CShellLink_Type == (Type) null)
        ShellHelper.CShellLink_Type = Type.GetTypeFromCLSID(new Guid("00021401-0000-0000-C000-000000000046"), false);
      if (ShellHelper.CShellLink_Type != (Type) null)
      {
        ShellHelper.IShellLink instance = (ShellHelper.IShellLink) Activator.CreateInstance(ShellHelper.CShellLink_Type);
        instance.SetPath(fileName);
        ShellHelper.IPropertyStore propertyStore1 = (ShellHelper.IPropertyStore) instance;
        Guid guid = typeof (ShellHelper.IPropertyStore).GUID;
        if (!string.IsNullOrEmpty(amuid))
        {
          ShellHelper.IPropertyStore propertyStore2 = propertyStore1;
          PropertyKey id = SystemProperties.System.AppUserModel.ID;
          ref PropertyKey local = ref id;
          PropVariant pv = new PropVariant(amuid);
          int num = (int) propertyStore2.SetValue(ref local, pv);
        }
        if (!string.IsNullOrEmpty(displayName))
        {
          ShellHelper.IPropertyStore propertyStore3 = propertyStore1;
          PropertyKey itemNameDisplay = SystemProperties.System.ItemNameDisplay;
          ref PropertyKey local = ref itemNameDisplay;
          PropVariant pv = new PropVariant(displayName);
          int num = (int) propertyStore3.SetValue(ref local, pv);
        }
        int num1 = (int) propertyStore1.Commit();
        string str1 = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N").Substring(0, 8));
        Directory.CreateDirectory(str1);
        try
        {
          HashSet<char> invalidChars = new HashSet<char>(((IEnumerable<char>) Path.GetInvalidFileNameChars()).Distinct<char>());
          displayName = new string(displayName.Where<char>((Func<char, bool>) (c => !invalidChars.Contains(c))).ToArray<char>());
          if (displayName.Length == 0)
            displayName = Path.GetFileNameWithoutExtension(fileName);
          string str2 = Path.Combine(str1, displayName + ".lnk");
          File.Delete(str2);
          ((ShellHelper.IPersistFile) instance).Save(str2, true);
          using (ShellFile shellFile = ShellFile.FromFilePath(str2))
            return ShellHelper.UpdatePinState(pinning, ShellHelper.GetShellObjectPIDL((ShellObject) shellFile));
        }
        finally
        {
          Directory.Delete(str1, true);
        }
      }
    }
    catch
    {
    }
    return false;
  }

  private static bool UpdatePinState(bool pinning, IntPtr pidl)
  {
    bool result = false;
    Thread thread = new Thread(new ThreadStart(Run));
    thread.IsBackground = true;
    thread.SetApartmentState(ApartmentState.STA);
    thread.Start();
    thread.Join();
    return result;

    void Run() => result = ShellHelper.UpdatePinStateCore(pinning, pidl);
  }

  private static bool UpdatePinStateCore(bool pinning, IntPtr pidl)
  {
    if (pidl == IntPtr.Zero)
      return false;
    if (ShellHelper.CTaskbandPin_Type == (Type) null)
      ShellHelper.CTaskbandPin_Type = Type.GetTypeFromCLSID(new Guid("90AA3A4E-1CBA-4233-B8BB-535773D48449"), false);
    if (ShellHelper.CTaskbandPin_Type != (Type) null)
    {
      try
      {
        ShellHelper.IPinnedList3 instance = (ShellHelper.IPinnedList3) Activator.CreateInstance(ShellHelper.CTaskbandPin_Type);
        if (instance != null)
          return pinning ? instance.Modify(IntPtr.Zero, pidl, ShellHelper.PLMC.PLMC_EXPLORER) >= 0 : instance.Modify(pidl, IntPtr.Zero, ShellHelper.PLMC.PLMC_EXPLORER) >= 0;
      }
      catch
      {
      }
    }
    return false;
  }

  private static IntPtr GetShellObjectPIDL(ShellObject shellObject)
  {
    if (shellObject == (ShellObject) null)
      return IntPtr.Zero;
    if (ShellHelper.shellObjectPidlGetter == null)
    {
      try
      {
        ShellHelper.shellObjectPidlGetter = TypeHelper.CreateFieldOrPropertyGetter<ShellObject, IntPtr>("PIDL", BindingFlags.Instance | BindingFlags.NonPublic);
      }
      catch
      {
      }
      if (ShellHelper.shellObjectPidlGetter == null)
        ShellHelper.shellObjectPidlGetter = (Func<ShellObject, IntPtr>) (_ => IntPtr.Zero);
    }
    try
    {
      return ShellHelper.shellObjectPidlGetter(shellObject);
    }
    catch
    {
    }
    return IntPtr.Zero;
  }

  private enum PLMC
  {
    PLMC_EXPLORER = 4,
  }

  [Flags]
  private enum GetPropertyStoreOptions
  {
    Default = 0,
    HandlePropertiesOnly = 1,
    ReadWrite = 2,
    Temporary = 4,
    FastPropertiesOnly = 8,
    OpensLowItem = 16, // 0x00000010
    DelayCreation = 32, // 0x00000020
    BestEffort = 64, // 0x00000040
    MaskValid = 255, // 0x000000FF
  }

  [Guid("0DD79AE2-D156-45D4-9EEB-3B549769E940")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  private interface IPinnedList3
  {
    int Placeholder1();

    int Placeholder2();

    int Placeholder3();

    int Placeholder4();

    int Placeholder5();

    int Placeholder6();

    int Placeholder7();

    int Placeholder8();

    int Placeholder9();

    int Placeholder10();

    int Placeholder11();

    int Placeholder12();

    int Placeholder13();

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int Modify(IntPtr unpin, IntPtr pin, ShellHelper.PLMC caller);
  }

  [Guid("000214F9-0000-0000-C000-000000000046")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  private interface IShellLink
  {
    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetPath([MarshalAs(UnmanagedType.LPWStr)] out string pszFile, int cch, ref IntPtr pfd, uint fFlags);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetIDList(out IntPtr ppidl);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int SetIDList(IntPtr ppidl);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetDescription([MarshalAs(UnmanagedType.LPWStr)] out string pszName, int cch);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] out string pszDir, int cch);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetArguments([MarshalAs(UnmanagedType.LPWStr)] out string pszArgs, int cch);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetHotkey(out ushort pwHotkey);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int SetHotkey(ushort wHotkey);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetShowCmd(out int piShowCmd);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int SetShowCmd(int iShowCmd);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetIconLocation([MarshalAs(UnmanagedType.LPWStr)] out string pszIconPath, int cch, out int piIcon);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, uint dwReserved);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int Resolve(IntPtr hwnd, uint fFlags);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
  }

  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [Guid("886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99")]
  [ComImport]
  private interface IPropertyStore
  {
    [MethodImpl(MethodImplOptions.InternalCall)]
    HResult GetCount(out uint propertyCount);

    [MethodImpl(MethodImplOptions.InternalCall)]
    HResult GetAt([In] uint propertyIndex, out PropertyKey key);

    [MethodImpl(MethodImplOptions.InternalCall)]
    HResult GetValue([In] ref PropertyKey key, [Out] PropVariant pv);

    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
    HResult SetValue([In] ref PropertyKey key, [In] PropVariant pv);

    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
    HResult Commit();
  }

  [Guid("0000010b-0000-0000-C000-000000000046")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  public interface IPersistFile
  {
    void GetClassID(out Guid pClassID);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int IsDirty();

    [MethodImpl(MethodImplOptions.PreserveSig)]
    void Load([MarshalAs(UnmanagedType.LPWStr), In] string pszFileName, uint dwMode);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    void Save([MarshalAs(UnmanagedType.LPWStr), In] string pszFileName, [MarshalAs(UnmanagedType.Bool), In] bool fRemember);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    void SaveCompleted([MarshalAs(UnmanagedType.LPWStr), In] string pszFileName);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    void GetCurFile([MarshalAs(UnmanagedType.LPWStr), In] string ppszFileName);
  }
}
