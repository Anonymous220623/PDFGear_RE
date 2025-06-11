// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.WiaDeviceManager
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

using NAPS2.Wia.Native;
using System;
using System.Collections.Generic;
using System.IO;

#nullable enable
namespace NAPS2.Wia;

public class WiaDeviceManager : NativeWiaObject
{
  private const int SCANNER_DEVICE_TYPE = 1;
  private const int SELECT_DEVICE_NODEFAULT = 1;

  public WiaDeviceManager()
    : this(WiaVersion.Default)
  {
  }

  public WiaDeviceManager(WiaVersion version)
    : base(version)
  {
    IntPtr deviceManager;
    WiaException.Check(this.Version == WiaVersion.Wia10 ? NativeWiaMethods.GetDeviceManager1(out deviceManager) : NativeWiaMethods.GetDeviceManager2(out deviceManager));
    this.Handle = deviceManager;
  }

  protected internal WiaDeviceManager(WiaVersion version, IntPtr handle)
    : base(version, handle)
  {
  }

  public IEnumerable<WiaDeviceInfo> GetDeviceInfos()
  {
    List<WiaDeviceInfo> result = new List<WiaDeviceInfo>();
    WiaException.Check(this.Version == WiaVersion.Wia10 ? NativeWiaMethods.EnumerateDevices1(this.Handle, (Action<IntPtr>) (x => result.Add(new WiaDeviceInfo(this.Version, x)))) : NativeWiaMethods.EnumerateDevices2(this.Handle, (Action<IntPtr>) (x => result.Add(new WiaDeviceInfo(this.Version, x)))));
    return (IEnumerable<WiaDeviceInfo>) result;
  }

  public WiaDevice FindDevice(string deviceID)
  {
    IntPtr device;
    WiaException.Check(this.Version == WiaVersion.Wia10 ? NativeWiaMethods.GetDevice1(this.Handle, deviceID, out device) : NativeWiaMethods.GetDevice2(this.Handle, deviceID, out device));
    return new WiaDevice(this.Version, device);
  }

  public WiaDevice? PromptForDevice(IntPtr parentWindowHandle = default (IntPtr))
  {
    string deviceId;
    IntPtr device;
    uint hresult = this.Version == WiaVersion.Wia10 ? NativeWiaMethods.SelectDevice1(this.Handle, parentWindowHandle, 1, 1, out deviceId, out device) : NativeWiaMethods.SelectDevice2(this.Handle, parentWindowHandle, 1, 1, out deviceId, out device);
    if (hresult == 1U)
      return (WiaDevice) null;
    WiaException.Check(hresult);
    return new WiaDevice(this.Version, device);
  }

  public string[]? PromptForImage(WiaDevice device, IntPtr parentWindowHandle = default (IntPtr), string? tempFolder = null)
  {
    if (tempFolder == null)
      tempFolder = Path.GetTempPath();
    string randomFileName = Path.GetRandomFileName();
    IntPtr zero = IntPtr.Zero;
    int numFiles = 0;
    string[] filePaths = new string[0];
    uint hresult = this.Version == WiaVersion.Wia10 ? NativeWiaMethods.GetImage1(this.Handle, parentWindowHandle, 1, 0, 0, Path.Combine(tempFolder, randomFileName), IntPtr.Zero) : NativeWiaMethods.GetImage2(this.Handle, 0, device.Id(), parentWindowHandle, tempFolder, randomFileName, ref numFiles, ref filePaths, ref zero);
    if (hresult == 1U)
      return (string[]) null;
    WiaException.Check(hresult);
    string[] strArray = filePaths;
    if (strArray != null)
      return strArray;
    return new string[1]
    {
      Path.Combine(tempFolder, randomFileName)
    };
  }
}
