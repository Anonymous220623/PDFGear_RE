// Decompiled with JetBrains decompiler
// Type: RegExt.FileAssociations.DefaultAppHelper
// Assembly: RegExt, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: DBF16820-DB7E-4C29-8C11-DD0B94851B7F
// Assembly location: C:\Program Files\PDFgear\RegExt.exe

using CommomLib.Commom;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;

#nullable disable
namespace RegExt.FileAssociations;

internal static class DefaultAppHelper
{
  public static string GetDefaultAppProgId(string ext) => AppIdHelper.GetDefaultAppProgId(ext);

  public static void ResetDefaultApp(string ext)
  {
    if (string.IsNullOrEmpty(ext))
      return;
    ext = ext.Trim();
    string str1 = !ext.Contains(".") ? "Software\\Microsoft\\Windows\\Shell\\Associations\\UrlAssociations\\" + ext : "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\" + ext;
    try
    {
      string str2 = $"\\Registry\\User\\{WindowsIdentity.GetCurrent().User.ToString()}\\{str1}";
      RegistryBatchHelper.DeleteRegistry(new string[1]
      {
        str2 + "\\UserChoice"
      });
      RegistryBatchHelper.DeleteRegistry(new string[1]
      {
        str2 + "\\UserChoiceLatest\\ProgId"
      });
      RegistryBatchHelper.DeleteRegistry(new string[1]
      {
        str2 + "\\UserChoiceLatest"
      });
    }
    catch
    {
    }
  }

  public static void SetDefaultApp(string progId, string ext)
  {
    if (string.IsNullOrEmpty(progId) || string.IsNullOrEmpty(ext))
      return;
    progId = progId.Trim();
    ext = ext.Trim();
    DefaultAppHelper.WriteRequiredApplicationAssociationToasts(progId, ext);
    if (ext.Contains("."))
      DefaultAppHelper.ResetUserChoice("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\" + ext, progId, ext);
    else
      DefaultAppHelper.ResetUserChoice("Software\\Microsoft\\Windows\\Shell\\Associations\\UrlAssociations\\" + ext, progId, ext);
  }

  private static void ResetUserChoice(string keyPath, string progId, string ext)
  {
    try
    {
      string str = $"\\Registry\\User\\{WindowsIdentity.GetCurrent().User.ToString()}\\{keyPath}";
      RegistryBatchHelper.DeleteRegistry(new string[1]
      {
        str + "\\UserChoice"
      });
      string hash = DefaultAppHashHelper.GetHash(progId, ext);
      RegistryBatchHelper.SetRegistryKeyValue(str + "\\UserChoice", new Dictionary<string, string>()
      {
        ["Hash"] = hash,
        ["ProgId"] = progId
      });
    }
    catch
    {
    }
  }

  private static void WriteRequiredApplicationAssociationToasts(string progId, string ext)
  {
    DefaultAppHelper.WriteRequiredApplicationAssociationToasts((IReadOnlyList<(string, string)>) new (string, string)[1]
    {
      (progId, ext)
    });
  }

  private static void WriteRequiredApplicationAssociationToasts(
    IReadOnlyList<(string progId, string ext)> exts)
  {
    List<(RegistryKey, IReadOnlyList<string>)> valueTupleList = new List<(RegistryKey, IReadOnlyList<string>)>();
    foreach ((string progId, string ext) ext in (IEnumerable<(string progId, string ext)>) exts)
    {
      RegistryKey registryKey;
      IReadOnlyList<string> apps;
      if (DefaultAppHelper.GetRequiredApplicationAssociationToasts(ext.progId, ext.ext, out registryKey, out apps))
        valueTupleList.Add((registryKey, apps));
    }
    try
    {
      for (int index1 = 0; index1 < valueTupleList.Count; ++index1)
      {
        for (int index2 = 0; index2 < valueTupleList[index1].Item2.Count; ++index2)
          valueTupleList[index1].Item1.SetValue(valueTupleList[index1].Item2[index2], (object) 0);
      }
    }
    catch
    {
    }
  }

  private static bool GetRequiredApplicationAssociationToasts(
    string progId,
    string ext,
    out RegistryKey registryKey,
    out IReadOnlyList<string> apps)
  {
    registryKey = (RegistryKey) null;
    apps = (IReadOnlyList<string>) null;
    try
    {
      registryKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default).OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\ApplicationAssociationToasts", true);
    }
    catch
    {
    }
    if (registryKey == null)
      return false;
    List<string> source = new List<string>();
    source.Add(progId);
    RegistryKey baseKey1 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default);
    RegistryKey baseKey2 = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default);
    source.AddRange(DefaultAppHelper.GetAppsFromClasses(baseKey1, ext));
    source.AddRange(DefaultAppHelper.GetAppsFromClasses(baseKey2, ext));
    source.AddRange(DefaultAppHelper.GetAppsFromSoftwareCapabilities(baseKey1, ext));
    source.AddRange(DefaultAppHelper.GetAppsFromSoftwareCapabilities(baseKey2, ext));
    source.AddRange(DefaultAppHelper.GetStartMenuInternetApps(baseKey1, ext));
    source.AddRange(DefaultAppHelper.GetStartMenuInternetApps(baseKey2, ext));
    baseKey1?.Dispose();
    baseKey2?.Dispose();
    source.AddRange(DefaultAppHelper.GetAppsFromAppModel(ext));
    try
    {
      for (int index = source.Count - 1; index >= 0; --index)
      {
        string str = source[index];
        if (string.IsNullOrEmpty(str))
        {
          source.RemoveAt(index);
        }
        else
        {
          string name = $"{str}_{ext}";
          if (registryKey.GetValue(name) != null)
            source.RemoveAt(index);
          else if (index == source.Count + 1)
            source[index] = name;
          else
            source.Add(name);
        }
      }
    }
    catch (Exception ex)
    {
    }
    List<string> list = source.Distinct<string>().ToList<string>();
    if (list.Count == 0)
    {
      registryKey.Dispose();
      return false;
    }
    apps = (IReadOnlyList<string>) list;
    return true;
  }

  private static IEnumerable<string> GetStartMenuInternetApps(RegistryKey baseKey, string ext)
  {
    List<string> menuInternetApps = new List<string>();
    try
    {
      RegistryKey registryKey = baseKey?.OpenSubKey("SOFTWARE\\Clients\\StartMenuInternet");
      if (registryKey != null)
      {
        foreach (string subKeyName in registryKey.GetSubKeyNames())
        {
          if (!string.IsNullOrEmpty(registryKey.OpenSubKey($"{subKeyName}\\Capabilities\\{(ext.Contains(".") ? "FileAssociations" : "URLAssociations")}")?.GetValue(ext) as string))
            menuInternetApps.Add(subKeyName);
        }
      }
    }
    catch
    {
    }
    return (IEnumerable<string>) menuInternetApps;
  }

  private static IEnumerable<string> GetAppsFromAppModel(string ext)
  {
    List<string> appsFromAppModel = new List<string>();
    try
    {
      using (RegistryKey registryKey1 = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default))
      {
        using (RegistryKey registryKey2 = registryKey1.OpenSubKey("Software\\Classes\\Local Settings\\Software\\Microsoft\\Windows\\CurrentVersion\\AppModel\\Repository\\Packages"))
        {
          if (registryKey2 != null)
          {
            foreach (string subKeyName in registryKey2.GetSubKeyNames())
            {
              try
              {
                using (RegistryKey registryKey3 = registryKey2.OpenSubKey(subKeyName))
                {
                  if (registryKey3 != null)
                  {
                    string[] subKeyNames = registryKey3.GetSubKeyNames();
                    string str1 = "";
                    if (subKeyNames.Length == 1)
                      str1 = subKeyNames[0];
                    else if (str1.Contains("App"))
                      str1 = "App";
                    else if (subKeyNames.Length > 1)
                      str1 = subKeyNames[0];
                    string name = str1 + "\\Capabilities\\FileAssociations";
                    if (!ext.Contains<char>('.'))
                      name = str1 + "\\Capabilities\\URLAssociations";
                    using (RegistryKey registryKey4 = registryKey3.OpenSubKey(name))
                    {
                      if (registryKey4 != null)
                      {
                        foreach (string valueName in registryKey4.GetValueNames())
                        {
                          if (string.Equals(valueName, ext, StringComparison.OrdinalIgnoreCase) && registryKey4.GetValue(valueName) is string str2)
                            appsFromAppModel.Add(str2);
                        }
                      }
                    }
                  }
                }
              }
              catch
              {
              }
            }
          }
        }
      }
    }
    catch
    {
    }
    return (IEnumerable<string>) appsFromAppModel;
  }

  private static IEnumerable<string> GetAppsFromClasses(RegistryKey baseKey, string ext)
  {
    List<string> appsFromClasses = new List<string>();
    try
    {
      if (baseKey.OpenSubKey("SOFTWARE\\Classes\\" + ext)?.GetValue("") is string str)
        appsFromClasses.Add(str);
    }
    catch
    {
    }
    try
    {
      RegistryKey registryKey = baseKey.OpenSubKey($"SOFTWARE\\Classes\\{ext}\\OpenWithList");
      IEnumerable<string> collection = registryKey != null ? ((IEnumerable<string>) registryKey.GetSubKeyNames()).Select<string, string>((Func<string, string>) (c => "Applications\\" + c)) : (IEnumerable<string>) null;
      if (collection != null)
        appsFromClasses.AddRange(collection);
    }
    catch
    {
    }
    try
    {
      string[] valueNames = baseKey.OpenSubKey($"SOFTWARE\\Classes\\{ext}\\OpenWithProgids")?.GetValueNames();
      if (valueNames != null)
        appsFromClasses.AddRange((IEnumerable<string>) valueNames);
    }
    catch
    {
    }
    return (IEnumerable<string>) appsFromClasses;
  }

  private static IEnumerable<string> GetAppsFromSoftwareCapabilities(
    RegistryKey baseKey,
    string ext)
  {
    List<string> softwareCapabilities = new List<string>();
    try
    {
      using (RegistryKey registryKey1 = baseKey.OpenSubKey("SOFTWARE"))
      {
        foreach (string subKeyName in registryKey1.GetSubKeyNames())
        {
          try
          {
            string name = $"{subKeyName}\\Capabilities\\{(ext.Contains(".") ? "FileAssociations" : "URLAssociations")}";
            using (RegistryKey registryKey2 = registryKey1.OpenSubKey(name))
            {
              if (registryKey2 != null)
              {
                foreach (string valueName in registryKey2.GetValueNames())
                {
                  if (string.Equals(valueName, ext, StringComparison.OrdinalIgnoreCase) && registryKey2.GetValue(valueName) is string str)
                    softwareCapabilities.Add(str);
                }
              }
            }
          }
          catch
          {
          }
        }
      }
    }
    catch
    {
    }
    return (IEnumerable<string>) softwareCapabilities;
  }

  private static void RemoveUserChoiceKey(string key)
  {
    try
    {
      DefaultAppHelper.DeleteKey(key);
    }
    catch
    {
    }
  }

  private static void SetUserChoiceKeyAccessControl(
    RegistryKey extensionKey,
    string userChoiceKeyName = "UserChoice")
  {
    using (RegistryKey registryKey = extensionKey.OpenSubKey(userChoiceKeyName, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.ChangePermissions))
    {
      if (registryKey == null)
        return;
      string name = WindowsIdentity.GetCurrent().Name;
      RegistrySecurity accessControl = registryKey.GetAccessControl();
      foreach (RegistryAccessRule accessRule in (ReadOnlyCollectionBase) accessControl.GetAccessRules(true, true, typeof (NTAccount)))
      {
        if (accessRule.IdentityReference.Value == name && accessRule.AccessControlType == AccessControlType.Deny)
          accessControl.RemoveAccessRuleSpecific(accessRule);
      }
      registryKey.SetAccessControl(accessControl);
    }
  }

  public static void Refresh()
  {
    DefaultAppHelper.SHChangeNotify(134217728 /*0x08000000*/, 0, IntPtr.Zero, IntPtr.Zero);
  }

  private static void DeleteKey(string key)
  {
    UIntPtr hkResult = UIntPtr.Zero;
    DefaultAppHelper.RegOpenKeyEx((UIntPtr) 2147483649U /*0x80000001*/, key, 0, 131097, out hkResult);
    int num = (int) DefaultAppHelper.RegDeleteKey((UIntPtr) 2147483649U /*0x80000001*/, key);
  }

  [DllImport("Shell32.dll")]
  private static extern int SHChangeNotify(int eventId, int flags, IntPtr item1, IntPtr item2);

  [DllImport("advapi32.dll", SetLastError = true)]
  private static extern int RegOpenKeyEx(
    UIntPtr hKey,
    string subKey,
    int ulOptions,
    int samDesired,
    out UIntPtr hkResult);

  [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
  private static extern uint RegDeleteKey(UIntPtr hKey, string subKey);
}
