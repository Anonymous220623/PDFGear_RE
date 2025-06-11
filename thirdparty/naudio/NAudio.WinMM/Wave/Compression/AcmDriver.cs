// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.Compression.AcmDriver
// Assembly: NAudio.WinMM, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: F178471D-A6CB-4E6B-9DBA-D166E9B46BDD
// Assembly location: D:\PDFGear\bin\NAudio.WinMM.dll

using NAudio.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

#nullable disable
namespace NAudio.Wave.Compression;

public class AcmDriver : IDisposable
{
  private static List<AcmDriver> drivers;
  private AcmDriverDetails details;
  private IntPtr driverId;
  private IntPtr driverHandle;
  private List<AcmFormatTag> formatTags;
  private List<AcmFormat> tempFormatsList;
  private IntPtr localDllHandle;

  public static bool IsCodecInstalled(string shortName)
  {
    foreach (AcmDriver enumerateAcmDriver in AcmDriver.EnumerateAcmDrivers())
    {
      if (enumerateAcmDriver.ShortName == shortName)
        return true;
    }
    return false;
  }

  public static AcmDriver AddLocalDriver(string driverFile)
  {
    IntPtr num = NativeMethods.LoadLibrary(driverFile);
    IntPtr driverFunctionAddress = !(num == IntPtr.Zero) ? NativeMethods.GetProcAddress(num, "DriverProc") : throw new ArgumentException("Failed to load driver file");
    if (driverFunctionAddress == IntPtr.Zero)
    {
      NativeMethods.FreeLibrary(num);
      throw new ArgumentException("Failed to discover DriverProc");
    }
    IntPtr driverHandle;
    MmResult result = AcmInterop.acmDriverAdd(out driverHandle, num, driverFunctionAddress, 0, AcmDriverAddFlags.Function);
    if (result != MmResult.NoError)
    {
      NativeMethods.FreeLibrary(num);
      throw new MmException(result, "acmDriverAdd");
    }
    AcmDriver acmDriver = new AcmDriver(driverHandle);
    if (string.IsNullOrEmpty(acmDriver.details.longName))
    {
      acmDriver.details.longName = "Local driver: " + Path.GetFileName(driverFile);
      acmDriver.localDllHandle = num;
    }
    return acmDriver;
  }

  public static void RemoveLocalDriver(AcmDriver localDriver)
  {
    if (localDriver.localDllHandle == IntPtr.Zero)
      throw new ArgumentException("Please pass in the AcmDriver returned by the AddLocalDriver method");
    int result = (int) AcmInterop.acmDriverRemove(localDriver.driverId, 0);
    NativeMethods.FreeLibrary(localDriver.localDllHandle);
    MmException.Try((MmResult) result, "acmDriverRemove");
  }

  public static bool ShowFormatChooseDialog(
    IntPtr ownerWindowHandle,
    string windowTitle,
    AcmFormatEnumFlags enumFlags,
    WaveFormat enumFormat,
    out WaveFormat selectedFormat,
    out string selectedFormatDescription,
    out string selectedFormatTagDescription)
  {
    AcmFormatChoose formatChoose = new AcmFormatChoose();
    formatChoose.structureSize = Marshal.SizeOf<AcmFormatChoose>(formatChoose);
    formatChoose.styleFlags = AcmFormatChooseStyleFlags.None;
    formatChoose.ownerWindowHandle = ownerWindowHandle;
    int cb = 200;
    formatChoose.selectedWaveFormatPointer = Marshal.AllocHGlobal(cb);
    formatChoose.selectedWaveFormatByteSize = cb;
    formatChoose.title = windowTitle;
    formatChoose.name = (string) null;
    formatChoose.formatEnumFlags = enumFlags;
    formatChoose.waveFormatEnumPointer = IntPtr.Zero;
    if (enumFormat != null)
    {
      IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf<WaveFormat>(enumFormat));
      Marshal.StructureToPtr<WaveFormat>(enumFormat, ptr, false);
      formatChoose.waveFormatEnumPointer = ptr;
    }
    formatChoose.instanceHandle = IntPtr.Zero;
    formatChoose.templateName = (string) null;
    MmResult result = AcmInterop.acmFormatChoose(ref formatChoose);
    selectedFormat = (WaveFormat) null;
    selectedFormatDescription = (string) null;
    selectedFormatTagDescription = (string) null;
    if (result == MmResult.NoError)
    {
      selectedFormat = WaveFormat.MarshalFromPtr(formatChoose.selectedWaveFormatPointer);
      selectedFormatDescription = formatChoose.formatDescription;
      selectedFormatTagDescription = formatChoose.formatTagDescription;
    }
    Marshal.FreeHGlobal(formatChoose.waveFormatEnumPointer);
    Marshal.FreeHGlobal(formatChoose.selectedWaveFormatPointer);
    if (result != MmResult.AcmCancelled && result != MmResult.NoError)
      throw new MmException(result, "acmFormatChoose");
    return result == MmResult.NoError;
  }

  public int MaxFormatSize
  {
    get
    {
      int output;
      MmException.Try(AcmInterop.acmMetrics(this.driverHandle, AcmMetrics.MaxSizeFormat, out output), "acmMetrics");
      return output;
    }
  }

  public static AcmDriver FindByShortName(string shortName)
  {
    foreach (AcmDriver enumerateAcmDriver in AcmDriver.EnumerateAcmDrivers())
    {
      if (enumerateAcmDriver.ShortName == shortName)
        return enumerateAcmDriver;
    }
    return (AcmDriver) null;
  }

  public static IEnumerable<AcmDriver> EnumerateAcmDrivers()
  {
    AcmDriver.drivers = new List<AcmDriver>();
    MmException.Try(AcmInterop.acmDriverEnum(new AcmInterop.AcmDriverEnumCallback(AcmDriver.DriverEnumCallback), IntPtr.Zero, (AcmDriverEnumFlags) 0), "acmDriverEnum");
    return (IEnumerable<AcmDriver>) AcmDriver.drivers;
  }

  private static bool DriverEnumCallback(
    IntPtr hAcmDriver,
    IntPtr dwInstance,
    AcmDriverDetailsSupportFlags flags)
  {
    AcmDriver.drivers.Add(new AcmDriver(hAcmDriver));
    return true;
  }

  private AcmDriver(IntPtr hAcmDriver)
  {
    this.driverId = hAcmDriver;
    this.details = new AcmDriverDetails();
    this.details.structureSize = Marshal.SizeOf<AcmDriverDetails>(this.details);
    MmException.Try(AcmInterop.acmDriverDetails(hAcmDriver, ref this.details, 0), "acmDriverDetails");
  }

  public string ShortName => this.details.shortName;

  public string LongName => this.details.longName;

  public IntPtr DriverId => this.driverId;

  public override string ToString() => this.LongName;

  public IEnumerable<AcmFormatTag> FormatTags
  {
    get
    {
      if (this.formatTags == null)
      {
        if (this.driverHandle == IntPtr.Zero)
          throw new InvalidOperationException("Driver must be opened first");
        this.formatTags = new List<AcmFormatTag>();
        AcmFormatTagDetails formatTagDetails = new AcmFormatTagDetails();
        formatTagDetails.structureSize = Marshal.SizeOf<AcmFormatTagDetails>(formatTagDetails);
        MmException.Try(AcmInterop.acmFormatTagEnum(this.driverHandle, ref formatTagDetails, new AcmInterop.AcmFormatTagEnumCallback(this.AcmFormatTagEnumCallback), IntPtr.Zero, 0), "acmFormatTagEnum");
      }
      return (IEnumerable<AcmFormatTag>) this.formatTags;
    }
  }

  public IEnumerable<AcmFormat> GetFormats(AcmFormatTag formatTag)
  {
    if (this.driverHandle == IntPtr.Zero)
      throw new InvalidOperationException("Driver must be opened first");
    this.tempFormatsList = new List<AcmFormat>();
    AcmFormatDetails formatDetails = new AcmFormatDetails();
    formatDetails.structSize = Marshal.SizeOf<AcmFormatDetails>(formatDetails);
    formatDetails.waveFormatByteSize = 1024 /*0x0400*/;
    formatDetails.waveFormatPointer = Marshal.AllocHGlobal(formatDetails.waveFormatByteSize);
    formatDetails.formatTag = (int) formatTag.FormatTag;
    int result = (int) AcmInterop.acmFormatEnum(this.driverHandle, ref formatDetails, new AcmInterop.AcmFormatEnumCallback(this.AcmFormatEnumCallback), IntPtr.Zero, AcmFormatEnumFlags.None);
    Marshal.FreeHGlobal(formatDetails.waveFormatPointer);
    MmException.Try((MmResult) result, "acmFormatEnum");
    return (IEnumerable<AcmFormat>) this.tempFormatsList;
  }

  public void Open()
  {
    if (!(this.driverHandle == IntPtr.Zero))
      return;
    MmException.Try(AcmInterop.acmDriverOpen(out this.driverHandle, this.DriverId, 0), "acmDriverOpen");
  }

  public void Close()
  {
    if (!(this.driverHandle != IntPtr.Zero))
      return;
    MmException.Try(AcmInterop.acmDriverClose(this.driverHandle, 0), "acmDriverClose");
    this.driverHandle = IntPtr.Zero;
  }

  private bool AcmFormatTagEnumCallback(
    IntPtr hAcmDriverId,
    ref AcmFormatTagDetails formatTagDetails,
    IntPtr dwInstance,
    AcmDriverDetailsSupportFlags flags)
  {
    this.formatTags.Add(new AcmFormatTag(formatTagDetails));
    return true;
  }

  private bool AcmFormatEnumCallback(
    IntPtr hAcmDriverId,
    ref AcmFormatDetails formatDetails,
    IntPtr dwInstance,
    AcmDriverDetailsSupportFlags flags)
  {
    this.tempFormatsList.Add(new AcmFormat(formatDetails));
    return true;
  }

  public void Dispose()
  {
    if (!(this.driverHandle != IntPtr.Zero))
      return;
    this.Close();
    GC.SuppressFinalize((object) this);
  }
}
