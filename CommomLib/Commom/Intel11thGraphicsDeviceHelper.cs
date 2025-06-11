// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.Intel11thGraphicsDeviceHelper
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;

#nullable disable
namespace CommomLib.Commom;

public class Intel11thGraphicsDeviceHelper
{
  private static IReadOnlyList<IReadOnlyDictionary<string, string>> infos;
  private static object locker = new object();
  private Version driverVersion2;

  static Intel11thGraphicsDeviceHelper()
  {
    SystemEvents.DisplaySettingsChanged += new EventHandler(Intel11thGraphicsDeviceHelper.SystemEvents_DisplaySettingsChanged);
  }

  private static void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e)
  {
    if (Intel11thGraphicsDeviceHelper.infos == null)
      return;
    lock (Intel11thGraphicsDeviceHelper.locker)
    {
      if (Intel11thGraphicsDeviceHelper.infos == null)
        return;
      Intel11thGraphicsDeviceHelper.infos = (IReadOnlyList<IReadOnlyDictionary<string, string>>) null;
    }
  }

  private Intel11thGraphicsDeviceHelper(
    string friendlyName,
    string deviceId,
    string driverDate,
    string driverVersion)
  {
    this.FriendlyName = friendlyName;
    this.DeviceId = deviceId;
    this.DriverDate = driverDate;
    this.DriverVersion = driverVersion;
  }

  public string FriendlyName { get; }

  public string DeviceId { get; }

  public string DriverDate { get; }

  public string DriverVersion { get; }

  public Version DriverVersion2
  {
    get
    {
      if (this.driverVersion2 == (Version) null && !Version.TryParse(this.DriverVersion, out this.driverVersion2))
        this.driverVersion2 = new Version(0, 0, 0, 0);
      return this.driverVersion2;
    }
  }

  public static Intel11thGraphicsDeviceHelper[] Create()
  {
    IReadOnlyList<IReadOnlyDictionary<string, string>> infos = Intel11thGraphicsDeviceHelper.infos;
    if (Intel11thGraphicsDeviceHelper.infos == null)
    {
      lock (Intel11thGraphicsDeviceHelper.locker)
      {
        if (Intel11thGraphicsDeviceHelper.infos == null)
        {
          Intel11thGraphicsDeviceHelper.infos = (IReadOnlyList<IReadOnlyDictionary<string, string>>) Intel11thGraphicsDeviceHelper.TryGetAllInfo().OfType<IReadOnlyDictionary<string, string>>().ToArray<IReadOnlyDictionary<string, string>>();
          infos = Intel11thGraphicsDeviceHelper.infos;
        }
      }
    }
    return infos.Select<IReadOnlyDictionary<string, string>, Intel11thGraphicsDeviceHelper>((Func<IReadOnlyDictionary<string, string>, Intel11thGraphicsDeviceHelper>) (c => new Intel11thGraphicsDeviceHelper(c["Caption"], c["DeviceID"], c["DriverDate"], c["DriverVersion"]))).ToArray<Intel11thGraphicsDeviceHelper>();
  }

  private static Dictionary<string, string>[] TryGetAllInfo()
  {
    string cpuName = DeviceInfo.GetCPUName();
    if (string.IsNullOrEmpty(cpuName))
      return Array.Empty<Dictionary<string, string>>();
    string lowerInvariant1 = cpuName.ToLowerInvariant();
    if (!lowerInvariant1.Contains("intel"))
      return Array.Empty<Dictionary<string, string>>();
    if (!lowerInvariant1.Contains("11th") && !lowerInvariant1.Contains("-11"))
      return Array.Empty<Dictionary<string, string>>();
    List<Dictionary<string, string>> dictionaryList = new List<Dictionary<string, string>>();
    try
    {
      using (ManagementClass managementClass = new ManagementClass("root\\CIMV2", "Win32_VideoController", new ObjectGetOptions()))
      {
        ManagementObjectCollection instances = managementClass.GetInstances();
        if (instances.Count > 0)
        {
          foreach (ManagementBaseObject mo in instances)
          {
            string managementObjectValue = DeviceInfo.GetManagementObjectValue(mo, "Caption");
            if (!string.IsNullOrEmpty(managementObjectValue))
            {
              string lowerInvariant2 = managementObjectValue.ToLowerInvariant();
              if (lowerInvariant2.Contains("intel") && lowerInvariant2.Contains("graphics"))
                dictionaryList.Add(new Dictionary<string, string>()
                {
                  ["Caption"] = managementObjectValue,
                  ["DeviceID"] = DeviceInfo.GetManagementObjectValue(mo, "DeviceID"),
                  ["DriverDate"] = DeviceInfo.GetManagementObjectValue(mo, "DriverDate"),
                  ["DriverVersion"] = DeviceInfo.GetManagementObjectValue(mo, "DriverVersion")
                });
            }
          }
        }
      }
    }
    catch
    {
    }
    return dictionaryList.ToArray();
  }
}
