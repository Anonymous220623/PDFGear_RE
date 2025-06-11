// Decompiled with JetBrains decompiler
// Type: NAudio.Wave.Compression.AcmInterop
// Assembly: NAudio.WinMM, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: F178471D-A6CB-4E6B-9DBA-D166E9B46BDD
// Assembly location: D:\PDFGear\bin\NAudio.WinMM.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace NAudio.Wave.Compression;

internal class AcmInterop
{
  [DllImport("msacm32.dll")]
  public static extern MmResult acmDriverAdd(
    out IntPtr driverHandle,
    IntPtr driverModule,
    IntPtr driverFunctionAddress,
    int priority,
    AcmDriverAddFlags flags);

  [DllImport("msacm32.dll")]
  public static extern MmResult acmDriverRemove(IntPtr driverHandle, int removeFlags);

  [DllImport("Msacm32.dll")]
  public static extern MmResult acmDriverClose(IntPtr hAcmDriver, int closeFlags);

  [DllImport("Msacm32.dll")]
  public static extern MmResult acmDriverEnum(
    AcmInterop.AcmDriverEnumCallback fnCallback,
    IntPtr dwInstance,
    AcmDriverEnumFlags flags);

  [DllImport("Msacm32.dll")]
  public static extern MmResult acmDriverDetails(
    IntPtr hAcmDriver,
    ref AcmDriverDetails driverDetails,
    int reserved);

  [DllImport("Msacm32.dll")]
  public static extern MmResult acmDriverOpen(
    out IntPtr pAcmDriver,
    IntPtr hAcmDriverId,
    int openFlags);

  [DllImport("Msacm32.dll", EntryPoint = "acmFormatChooseW")]
  public static extern MmResult acmFormatChoose(ref AcmFormatChoose formatChoose);

  [DllImport("Msacm32.dll")]
  public static extern MmResult acmFormatEnum(
    IntPtr hAcmDriver,
    ref AcmFormatDetails formatDetails,
    AcmInterop.AcmFormatEnumCallback callback,
    IntPtr instance,
    AcmFormatEnumFlags flags);

  [DllImport("Msacm32.dll", EntryPoint = "acmFormatSuggest")]
  public static extern MmResult acmFormatSuggest2(
    IntPtr hAcmDriver,
    IntPtr sourceFormatPointer,
    IntPtr destFormatPointer,
    int sizeDestFormat,
    AcmFormatSuggestFlags suggestFlags);

  [DllImport("Msacm32.dll")]
  public static extern MmResult acmFormatTagEnum(
    IntPtr hAcmDriver,
    ref AcmFormatTagDetails formatTagDetails,
    AcmInterop.AcmFormatTagEnumCallback callback,
    IntPtr instance,
    int reserved);

  [DllImport("Msacm32.dll")]
  public static extern MmResult acmMetrics(IntPtr hAcmObject, AcmMetrics metric, out int output);

  [DllImport("Msacm32.dll", EntryPoint = "acmStreamOpen")]
  public static extern MmResult acmStreamOpen2(
    out IntPtr hAcmStream,
    IntPtr hAcmDriver,
    IntPtr sourceFormatPointer,
    IntPtr destFormatPointer,
    [In] WaveFilter waveFilter,
    IntPtr callback,
    IntPtr instance,
    AcmStreamOpenFlags openFlags);

  [DllImport("Msacm32.dll")]
  public static extern MmResult acmStreamClose(IntPtr hAcmStream, int closeFlags);

  [DllImport("Msacm32.dll")]
  public static extern MmResult acmStreamConvert(
    IntPtr hAcmStream,
    [In, Out] AcmStreamHeaderStruct streamHeader,
    AcmStreamConvertFlags streamConvertFlags);

  [DllImport("Msacm32.dll")]
  public static extern MmResult acmStreamPrepareHeader(
    IntPtr hAcmStream,
    [In, Out] AcmStreamHeaderStruct streamHeader,
    int prepareFlags);

  [DllImport("Msacm32.dll")]
  public static extern MmResult acmStreamReset(IntPtr hAcmStream, int resetFlags);

  [DllImport("Msacm32.dll")]
  public static extern MmResult acmStreamSize(
    IntPtr hAcmStream,
    int inputBufferSize,
    out int outputBufferSize,
    AcmStreamSizeFlags flags);

  [DllImport("Msacm32.dll")]
  public static extern MmResult acmStreamUnprepareHeader(
    IntPtr hAcmStream,
    [In, Out] AcmStreamHeaderStruct streamHeader,
    int flags);

  public delegate bool AcmDriverEnumCallback(
    IntPtr hAcmDriverId,
    IntPtr instance,
    AcmDriverDetailsSupportFlags flags);

  public delegate bool AcmFormatEnumCallback(
    IntPtr hAcmDriverId,
    ref AcmFormatDetails formatDetails,
    IntPtr dwInstance,
    AcmDriverDetailsSupportFlags flags);

  public delegate bool AcmFormatTagEnumCallback(
    IntPtr hAcmDriverId,
    ref AcmFormatTagDetails formatTagDetails,
    IntPtr dwInstance,
    AcmDriverDetailsSupportFlags flags);

  public delegate bool AcmFormatChooseHookProc(
    IntPtr windowHandle,
    int message,
    IntPtr wParam,
    IntPtr lParam);
}
