// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.DeviceInfo
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Windows;

#nullable disable
namespace CommomLib.Commom;

public class DeviceInfo
{
  private static object locker = new object();
  private static DeviceInfo.ManufacturerInfo manufacturerInfo;
  private static string cpu;
  private static bool? curProcessElevatedCache;

  public DeviceInfo()
  {
    this.Monitors = (IReadOnlyList<DeviceInfo.MonitorInfo>) DeviceInfo.GetAllMonitorsInfo();
    this.Drives = (IReadOnlyList<DriveInfo>) DriveInfo.GetDrives();
    this.Touch = DeviceInfo.GetDigitizerInfo();
    this.Memory = DeviceInfo.GetMemoryInfo();
  }

  public IReadOnlyList<DeviceInfo.MonitorInfo> Monitors { get; }

  public IReadOnlyList<DriveInfo> Drives { get; }

  public DeviceInfo.DigitizerInfo Touch { get; }

  public string OSVersion => Environment.OSVersion.VersionString;

  public string InstalledLanguage => CultureInfo.InstalledUICulture?.Name ?? "";

  public Architecture OSArchitecture => RuntimeInformation.OSArchitecture;

  public Architecture ProcessArchitecture => RuntimeInformation.ProcessArchitecture;

  public string MachineName => Environment.MachineName;

  public DeviceInfo.ManufacturerInfo Manufacturer => DeviceInfo.GetManufacturerInfo();

  public string CPU => DeviceInfo.GetCPUName();

  public DeviceInfo.MemoryInfo Memory { get; }

  public string FrameworkDescription => RuntimeInformation.FrameworkDescription;

  public bool Elevated => DeviceInfo.IsCurrentProcessElevated();

  public override string ToString()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.AppendLine("[OS]");
    try
    {
      stringBuilder.Append("OSVersion: ").AppendLine(this.OSVersion);
      stringBuilder.Append("InstalledLanguage: ").AppendLine(this.InstalledLanguage);
      stringBuilder.Append("FrameworkDescription: ").AppendLine(this.FrameworkDescription);
    }
    catch
    {
    }
    try
    {
      stringBuilder.Append("OSArchitecture: ").AppendLine(this.OSArchitecture.ToString());
    }
    catch
    {
    }
    try
    {
      stringBuilder.Append("ProcessArchitecture: ").AppendLine(this.ProcessArchitecture.ToString());
    }
    catch
    {
    }
    if (this.Manufacturer != null)
    {
      try
      {
        stringBuilder.AppendLine(this.Manufacturer.ToString());
      }
      catch
      {
      }
    }
    stringBuilder.Append("CPU: ").AppendLine(this.CPU);
    stringBuilder.AppendLine().AppendLine("[Process]");
    stringBuilder.AppendLine(this.Elevated ? "Administrator" : "User");
    stringBuilder.AppendLine().AppendLine("[Memory]");
    if (this.Memory != null)
      stringBuilder.AppendLine(this.Memory.ToString());
    stringBuilder.AppendLine().AppendLine("[Drives]");
    stringBuilder.AppendLine(this.DrivesToString());
    stringBuilder.AppendLine().AppendLine("[Screen]");
    if (this.Monitors != null)
    {
      stringBuilder.Append("Count: ").Append(this.Monitors.Count).AppendLine();
      for (int index = 0; index < this.Monitors.Count; ++index)
      {
        DeviceInfo.MonitorInfo monitor = this.Monitors[index];
        stringBuilder.AppendLine(monitor.ToString());
        if (index < this.Monitors.Count - 1)
          stringBuilder.AppendLine();
      }
    }
    stringBuilder.AppendLine().AppendLine("[Touch]");
    if (this.Touch != null)
      stringBuilder.AppendLine(this.Touch.ToString());
    return stringBuilder.ToString();
  }

  private string DrivesToString()
  {
    if (this.Drives == null || this.Drives.Count == 0)
      return string.Empty;
    StringBuilder stringBuilder = new StringBuilder();
    try
    {
      IReadOnlyList<DriveInfo> drives = this.Drives;
      for (int index = 0; index < drives.Count; ++index)
      {
        DriveInfo driveInfo = drives[index];
        if (driveInfo.IsReady)
        {
          double num1 = (double) driveInfo.TotalSize / 1024.0 / 1024.0;
          double num2 = (double) driveInfo.TotalFreeSpace / 1024.0 / 1024.0;
          stringBuilder.AppendFormat("{0}:[{2}], {3}MB/{4}MB, {1}% Free", (object) driveInfo.Name, (object) $"{num2 / num1 * 100.0:F2}", (object) driveInfo.DriveFormat, (object) (long) num2, (object) (long) num1);
          if (index < drives.Count - 1)
            stringBuilder.AppendLine();
        }
      }
    }
    catch
    {
    }
    return stringBuilder.ToString();
  }

  private static DeviceInfo.DigitizerInfo GetDigitizerInfo()
  {
    int digitizerMaxTouchCount = DeviceInfo.GetDigitizerMaxTouchCount(DeviceInfo.GetDigitizerFlag());
    return DeviceInfo.DigitizerInfo.CreateDigitizerInfo(DeviceInfo.GetDigitizerFlag(), digitizerMaxTouchCount);
  }

  internal static string GetCPUName()
  {
    if (string.IsNullOrEmpty(DeviceInfo.cpu))
    {
      lock (DeviceInfo.locker)
      {
        if (string.IsNullOrEmpty(DeviceInfo.cpu))
        {
          string str = "";
          try
          {
            using (ManagementClass managementClass = new ManagementClass("Win32_Processor"))
            {
              ManagementObjectCollection instances = managementClass.GetInstances();
              if (instances.Count > 0)
              {
                foreach (ManagementBaseObject mo in instances)
                {
                  if (string.IsNullOrEmpty(str))
                    str = DeviceInfo.GetManagementObjectValue(mo, "Name");
                }
              }
            }
          }
          catch
          {
          }
          if (string.IsNullOrEmpty(str))
            str = "Unknown";
          DeviceInfo.cpu = str;
        }
      }
    }
    return DeviceInfo.cpu;
  }

  private static DeviceInfo.MemoryInfo GetMemoryInfo()
  {
    DeviceInfo.MEMORYSTATUSEX meminfo = new DeviceInfo.MEMORYSTATUSEX();
    return DeviceInfo.GlobalMemoryStatusEx(meminfo) ? new DeviceInfo.MemoryInfo((double) meminfo.dwMemoryLoad * 0.01, meminfo.ullTotalPhys, meminfo.ullAvailPhys) : (DeviceInfo.MemoryInfo) null;
  }

  private static DeviceInfo.ManufacturerInfo GetManufacturerInfo()
  {
    if (DeviceInfo.manufacturerInfo == null)
    {
      lock (DeviceInfo.locker)
      {
        if (DeviceInfo.manufacturerInfo == null)
        {
          string manufacturer = "";
          string systemFamily = "";
          string model = "";
          try
          {
            using (ManagementClass managementClass = new ManagementClass("Win32_ComputerSystem"))
            {
              ManagementObjectCollection instances = managementClass.GetInstances();
              if (instances.Count > 0)
              {
                foreach (ManagementBaseObject mo in instances)
                {
                  if (string.IsNullOrEmpty(manufacturer))
                    manufacturer = DeviceInfo.GetManagementObjectValue(mo, "Manufacturer");
                  if (string.IsNullOrEmpty(systemFamily))
                    systemFamily = DeviceInfo.GetManagementObjectValue(mo, "SystemFamily");
                  if (string.IsNullOrEmpty(model))
                    model = DeviceInfo.GetManagementObjectValue(mo, "Model");
                }
              }
            }
          }
          catch
          {
          }
          DeviceInfo.manufacturerInfo = new DeviceInfo.ManufacturerInfo(manufacturer, systemFamily, model);
        }
      }
    }
    return DeviceInfo.manufacturerInfo;
  }

  internal static string GetManagementObjectValue(ManagementBaseObject mo, string propName)
  {
    if (mo != null)
    {
      if (!string.IsNullOrEmpty(propName))
      {
        try
        {
          switch (mo[propName])
          {
            case string managementObjectValue:
              return managementObjectValue;
            case string[] strArray:
              if (strArray.Length != 0)
                return string.Join(", ", strArray);
              break;
          }
        }
        catch
        {
        }
        return string.Empty;
      }
    }
    return string.Empty;
  }

  private static DeviceInfo.MONITORINFOEXW CreateMonitorInfoEX()
  {
    return new DeviceInfo.MONITORINFOEXW()
    {
      cbSize = (uint) sizeof (DeviceInfo.MONITORINFOEXW)
    };
  }

  private static bool MultiMonitorSupport() => DeviceInfo.GetSystemMetrics(80 /*0x50*/) != 0;

  private static DeviceInfo.SM_DIGITIZER_FLAG GetDigitizerFlag()
  {
    return (DeviceInfo.SM_DIGITIZER_FLAG) DeviceInfo.GetSystemMetrics(94);
  }

  private static int GetDigitizerMaxTouchCount(DeviceInfo.SM_DIGITIZER_FLAG flags)
  {
    return (flags & DeviceInfo.SM_DIGITIZER_FLAG.NID_READY) != DeviceInfo.SM_DIGITIZER_FLAG.TABLET_CONFIG_NONE && (flags & DeviceInfo.SM_DIGITIZER_FLAG.NID_MULTI_INPUT) != DeviceInfo.SM_DIGITIZER_FLAG.TABLET_CONFIG_NONE ? DeviceInfo.GetSystemMetrics(95) : 0;
  }

  private static unsafe DeviceInfo.MonitorInfo[] GetAllMonitorsInfo()
  {
    List<DeviceInfo.MonitorInfo> list = new List<DeviceInfo.MonitorInfo>();
    DeviceInfo.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, new DeviceInfo.EnumMonitorsDelegate(_EnumMonitors), IntPtr.Zero);
    return list.ToArray();

    unsafe bool _EnumMonitors(
      IntPtr hMonitor,
      IntPtr hdcMonitor,
      ref DeviceInfo._RECT lprcMonitor,
      IntPtr dwData)
    {
      try
      {
        if (hMonitor != IntPtr.Zero)
        {
          DeviceInfo.MONITORINFOEXW monitorInfoEx = DeviceInfo.CreateMonitorInfoEX();
          if (DeviceInfo.GetMonitorInfoW(hMonitor, ref monitorInfoEx) > 0)
          {
            uint dpiX;
            uint dpiY;
            int dpiForMonitor = (int) DeviceInfo.GetDpiForMonitor(hMonitor, DeviceInfo.MONITOR_DPI_TYPE.MDT_EFFECTIVE_DPI, out dpiX, out dpiY);
            string empty = string.Empty;
            string deviceName;
            try
            {
              deviceName = new string(monitorInfoEx.szDevice);
            }
            catch
            {
              deviceName = string.Empty;
            }
            list.Add(new DeviceInfo.MonitorInfo((Rect) monitorInfoEx.rcMonitor, (Rect) monitorInfoEx.rcWork, (monitorInfoEx.dwFlags & DeviceInfo.MONITORINFOF.PRIMARY) > DeviceInfo.MONITORINFOF.None, deviceName, (int) dpiX, (int) dpiY));
          }
        }
      }
      catch
      {
      }
      return true;
    }
  }

  private static bool IsCurrentProcessElevated()
  {
    if (!DeviceInfo.curProcessElevatedCache.HasValue)
    {
      try
      {
        DeviceInfo.curProcessElevatedCache = new bool?(new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator));
      }
      catch
      {
        DeviceInfo.curProcessElevatedCache = new bool?();
      }
    }
    return DeviceInfo.curProcessElevatedCache.GetValueOrDefault();
  }

  [DllImport("user32.dll")]
  private static extern bool EnumDisplayMonitors(
    IntPtr hdc,
    IntPtr lprcClip,
    DeviceInfo.EnumMonitorsDelegate lpfnEnum,
    IntPtr dwData);

  [DllImport("user32")]
  private static extern int GetSystemMetrics(int nIndex);

  [DllImport("user32")]
  private static extern int GetMonitorInfoW(IntPtr hMonitor, ref DeviceInfo.MONITORINFOEXW lpmi);

  [DllImport("shcore", CharSet = CharSet.Auto, SetLastError = true)]
  private static extern uint GetDpiForMonitor(
    IntPtr hMonitor,
    DeviceInfo.MONITOR_DPI_TYPE dpiType,
    out uint dpiX,
    out uint dpiY);

  [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
  private static extern bool GlobalMemoryStatusEx(DeviceInfo.MEMORYSTATUSEX meminfo);

  public class DigitizerInfo
  {
    private static object nonTouchInstanceLocker = new object();
    private static DeviceInfo.DigitizerInfo nonTouchInstance;

    internal DigitizerInfo(DeviceInfo.SM_DIGITIZER_FLAG flags, int maxTouchCount)
      : this((flags & DeviceInfo.SM_DIGITIZER_FLAG.NID_READY) > DeviceInfo.SM_DIGITIZER_FLAG.TABLET_CONFIG_NONE, (flags & DeviceInfo.SM_DIGITIZER_FLAG.NID_INTEGRATED_TOUCH) > DeviceInfo.SM_DIGITIZER_FLAG.TABLET_CONFIG_NONE, (flags & DeviceInfo.SM_DIGITIZER_FLAG.NID_EXTERNAL_TOUCH) > DeviceInfo.SM_DIGITIZER_FLAG.TABLET_CONFIG_NONE, (flags & DeviceInfo.SM_DIGITIZER_FLAG.NID_INTEGRATED_PEN) > DeviceInfo.SM_DIGITIZER_FLAG.TABLET_CONFIG_NONE, (flags & DeviceInfo.SM_DIGITIZER_FLAG.NID_EXTERNAL_TOUCH) > DeviceInfo.SM_DIGITIZER_FLAG.TABLET_CONFIG_NONE, (flags & DeviceInfo.SM_DIGITIZER_FLAG.NID_MULTI_INPUT) > DeviceInfo.SM_DIGITIZER_FLAG.TABLET_CONFIG_NONE, maxTouchCount)
    {
    }

    internal DigitizerInfo(
      bool isTouchSupported,
      bool integratedTouch,
      bool externalTouch,
      bool integratedPen,
      bool externalPen,
      bool multipleTouch,
      int maxTouchCount)
    {
      this.IsTouchSupported = isTouchSupported;
      this.IntegratedTouch = integratedTouch;
      this.ExternalTouch = externalTouch;
      this.IntegratedPen = integratedPen;
      this.ExternalPen = externalPen;
      this.MultipleTouch = multipleTouch;
      this.MaxTouchCount = maxTouchCount;
    }

    public bool IsTouchSupported { get; }

    public bool IntegratedTouch { get; }

    public bool ExternalTouch { get; }

    public bool IntegratedPen { get; }

    public bool ExternalPen { get; }

    public bool MultipleTouch { get; }

    public int MaxTouchCount { get; }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("IsTouchSupported: ").Append(this.IsTouchSupported).AppendLine();
      stringBuilder.Append("IntegratedTouch: ").Append(this.IntegratedTouch).AppendLine();
      stringBuilder.Append("ExternalTouch: ").Append(this.ExternalTouch).AppendLine();
      stringBuilder.Append("IntegratedPen: ").Append(this.IntegratedPen).AppendLine();
      stringBuilder.Append("ExternalPen: ").Append(this.ExternalPen).AppendLine();
      stringBuilder.Append("MultipleTouch: ").Append(this.MultipleTouch).AppendLine();
      stringBuilder.Append("MaxTouchCount: ").Append(this.MaxTouchCount).AppendLine();
      return stringBuilder.ToString();
    }

    internal static DeviceInfo.DigitizerInfo CreateDigitizerInfo(
      DeviceInfo.SM_DIGITIZER_FLAG flags,
      int maxTouchCount)
    {
      if ((flags & DeviceInfo.SM_DIGITIZER_FLAG.NID_READY) != DeviceInfo.SM_DIGITIZER_FLAG.TABLET_CONFIG_NONE)
        return new DeviceInfo.DigitizerInfo(flags, maxTouchCount);
      if (DeviceInfo.DigitizerInfo.nonTouchInstance == null)
      {
        lock (DeviceInfo.DigitizerInfo.nonTouchInstanceLocker)
        {
          if (DeviceInfo.DigitizerInfo.nonTouchInstance == null)
            DeviceInfo.DigitizerInfo.nonTouchInstance = new DeviceInfo.DigitizerInfo(DeviceInfo.SM_DIGITIZER_FLAG.TABLET_CONFIG_NONE, 0);
        }
      }
      return DeviceInfo.DigitizerInfo.nonTouchInstance;
    }
  }

  public class MonitorInfo
  {
    internal MonitorInfo(
      Rect bounds,
      Rect workingArea,
      bool primary,
      string deviceName,
      int dpiX,
      int dpiY)
    {
      this.Bounds = bounds;
      this.WorkingArea = workingArea;
      this.Primary = primary;
      this.DeviceName = deviceName;
      this.DpiX = dpiX;
      this.DpiY = dpiY;
    }

    public Rect Bounds { get; }

    public Rect WorkingArea { get; }

    public bool Primary { get; }

    public string DeviceName { get; }

    public int DpiX { get; }

    public int DpiY { get; }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("DeviceName: ").Append(this.DeviceName);
      if (this.Primary)
        stringBuilder.AppendLine(", Primary");
      else
        stringBuilder.AppendLine();
      stringBuilder.Append("Bounds: ").AppendLine(this.Bounds.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      stringBuilder.Append("WorkingArea: ").AppendLine(this.WorkingArea.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      stringBuilder.Append("Dpi: ").Append(this.DpiX).Append(", ").Append(this.DpiY);
      return stringBuilder.ToString();
    }
  }

  public class ManufacturerInfo
  {
    public ManufacturerInfo(string manufacturer, string systemFamily, string model)
    {
      this.Manufacturer = manufacturer;
      this.SystemFamily = systemFamily;
      this.Model = model;
    }

    public string Manufacturer { get; }

    public string SystemFamily { get; }

    public string Model { get; }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("Manufacturer: ").AppendLine(this.Manufacturer);
      stringBuilder.Append("SystemFamily: ").AppendLine(this.SystemFamily);
      stringBuilder.Append("Model: ").Append(this.Model);
      return stringBuilder.ToString();
    }
  }

  public class MemoryInfo
  {
    public MemoryInfo(double memoryLoadPrecent, ulong totalPhysical, ulong availablePhysical)
    {
      this.MemoryLoadPrecent = memoryLoadPrecent;
      this.TotalPhysical = totalPhysical;
      this.AvailablePhysical = availablePhysical;
    }

    public double MemoryLoadPrecent { get; }

    public ulong TotalPhysical { get; }

    public ulong AvailablePhysical { get; }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("MemoryLoadPrecent: ").AppendFormat("{0:0.##}", (object) (this.MemoryLoadPrecent * 100.0)).Append('%').AppendLine();
      stringBuilder.Append("TotalPhysical: ").AppendFormat("{0:0.##}", (object) ((double) this.TotalPhysical * 1.0 / 1024.0 / 1024.0 / 1024.0)).AppendLine("GB");
      stringBuilder.Append("AvailablePhysical: ").AppendFormat("{0:0.##}", (object) ((double) this.AvailablePhysical * 1.0 / 1024.0 / 1024.0 / 1024.0)).Append("GB");
      return stringBuilder.ToString();
    }
  }

  private delegate bool EnumMonitorsDelegate(
    IntPtr hMonitor,
    IntPtr hdcMonitor,
    ref DeviceInfo._RECT lprcMonitor,
    IntPtr dwData);

  [Flags]
  public enum SM_DIGITIZER_FLAG
  {
    TABLET_CONFIG_NONE = 0,
    NID_INTEGRATED_TOUCH = 1,
    NID_EXTERNAL_TOUCH = 2,
    NID_INTEGRATED_PEN = 4,
    NID_EXTERNAL_PEN = 8,
    NID_MULTI_INPUT = 64, // 0x00000040
    NID_READY = 128, // 0x00000080
  }

  private enum MONITOR_DPI_TYPE
  {
    MDT_DEFAULT = 0,
    MDT_EFFECTIVE_DPI = 0,
    MDT_ANGULAR_DPI = 1,
    MDT_RAW_DPI = 2,
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
  private class MEMORYSTATUSEX
  {
    public uint dwLength;
    public uint dwMemoryLoad;
    public ulong ullTotalPhys;
    public ulong ullAvailPhys;
    public ulong ullTotalPageFile;
    public ulong ullAvailPageFile;
    public ulong ullTotalVirtual;
    public ulong ullAvailVirtual;
    public ulong ullAvailExtendedVirtual;

    public MEMORYSTATUSEX()
    {
      this.dwLength = (uint) Marshal.SizeOf(typeof (DeviceInfo.MEMORYSTATUSEX));
    }
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  private struct MONITORINFOEXW
  {
    public uint cbSize;
    public DeviceInfo._RECT rcMonitor;
    public DeviceInfo._RECT rcWork;
    public DeviceInfo.MONITORINFOF dwFlags;
    public unsafe fixed char szDevice[32];
  }

  [Flags]
  public enum MONITORINFOF : uint
  {
    None = 0,
    PRIMARY = 1,
  }

  private struct _RECT(int left, int top, int right, int bottom)
  {
    public int left = left;
    public int top = top;
    public int right = right;
    public int bottom = bottom;

    public int X => this.left;

    public int Y => this.top;

    public int Width => this.right - this.left;

    public int Height => this.bottom - this.top;

    public Size Size => new Size((double) this.Width, (double) this.Height);

    public override string ToString()
    {
      return $"{{{this.left}, {this.top}, {this.right}, {this.bottom} (LTRB)}}";
    }

    public static implicit operator Rect(DeviceInfo._RECT rc)
    {
      return new Rect((double) rc.left, (double) rc.top, (double) rc.Width, (double) rc.Height);
    }
  }
}
