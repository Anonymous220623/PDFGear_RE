// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.Printer.PrinterDevModeHelper
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Interop;

#nullable disable
namespace pdfeditor.Utils.Printer;

public static class PrinterDevModeHelper
{
  private const int IDOK = 1;
  private const int IDCANCEL = 2;

  public static void SetHdevmode(this PrinterSettings settings, PrintDevModeHandle devmodeHandle)
  {
    if (settings == null || devmodeHandle == null)
      return;
    bool success = false;
    try
    {
      devmodeHandle.DangerousAddRef(ref success);
      settings.DefaultPageSettings.SetHdevmode(devmodeHandle.DangerousGetHandle());
      settings.SetHdevmode(devmodeHandle.DangerousGetHandle());
    }
    finally
    {
      if (success)
        devmodeHandle.DangerousRelease();
    }
  }

  public static PrintDevModeHandle OpenPrinterConfigure(Window window, PrinterSettings settings)
  {
    if (window == null || settings == null)
      return new PrintDevModeHandle(IntPtr.Zero);
    IntPtr handle = new WindowInteropHelper(window).Handle;
    return handle == IntPtr.Zero ? new PrintDevModeHandle(IntPtr.Zero) : PrinterDevModeHelper.OpenPrinterConfigure(handle, settings);
  }

  public static PrintDevModeHandle OpenPrinterConfigure(
    IntPtr parentWindow,
    PrinterSettings settings)
  {
    if (parentWindow == IntPtr.Zero || settings == null)
      return new PrintDevModeHandle(IntPtr.Zero);
    IntPtr hdevmode = settings.GetHdevmode();
    if (hdevmode != IntPtr.Zero)
    {
      PrintDevModeHandle printDevModeHandle = new PrintDevModeHandle(hdevmode);
      try
      {
        PrintDevModeHandle documentProperties = PrinterDevModeHelper.GetDocumentProperties(parentWindow, settings.PrinterName, hdevmode);
        if (documentProperties != null)
        {
          settings.SetHdevmode(documentProperties);
          return documentProperties;
        }
      }
      finally
      {
        printDevModeHandle.Dispose();
      }
    }
    return new PrintDevModeHandle(IntPtr.Zero);
  }

  private static PrintDevModeHandle GetDocumentProperties(
    IntPtr parentWindow,
    string printerName,
    IntPtr hDevModeInput)
  {
    PrinterDevModeHelper.PRINTER_DEFAULTS pd = new PrinterDevModeHelper.PRINTER_DEFAULTS();
    IntPtr phPrinter = IntPtr.Zero;
    try
    {
      if (PrinterDevModeHelper.OpenPrinter(printerName, out phPrinter, ref pd))
      {
        if (hDevModeInput != IntPtr.Zero)
        {
          try
          {
            IntPtr pDevModeInput = PrinterDevModeHelper.GlobalLock(hDevModeInput);
            int cb = PrinterDevModeHelper.DocumentProperties(IntPtr.Zero, phPrinter, printerName, IntPtr.Zero, IntPtr.Zero, 0);
            if (cb > 0)
            {
              IntPtr num = Marshal.AllocHGlobal(cb);
              int fMode = 14;
              if (PrinterDevModeHelper.DocumentProperties(parentWindow, phPrinter, printerName, num, pDevModeInput, fMode) == 1)
                return new PrintDevModeHandle(num);
              Marshal.FreeHGlobal(num);
            }
          }
          finally
          {
            PrinterDevModeHelper.GlobalUnlock(hDevModeInput);
          }
        }
      }
      else
        phPrinter = IntPtr.Zero;
    }
    finally
    {
      if (phPrinter != IntPtr.Zero)
        PrinterDevModeHelper.ClosePrinter(phPrinter);
    }
    return (PrintDevModeHandle) null;
  }

  [SuppressUnmanagedCodeSecurity]
  [DllImport("winspool.Drv", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
  private static extern bool OpenPrinter(
    [MarshalAs(UnmanagedType.LPTStr)] string printerName,
    out IntPtr phPrinter,
    ref PrinterDevModeHelper.PRINTER_DEFAULTS pd);

  [SuppressUnmanagedCodeSecurity]
  [DllImport("winspool.Drv", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
  private static extern bool ClosePrinter(IntPtr phPrinter);

  [DllImport("winspool.drv")]
  private static extern int PrinterProperties(IntPtr hwnd, IntPtr hPrinter);

  [DllImport("winspool.drv", EntryPoint = "DocumentPropertiesW", CharSet = CharSet.Unicode, SetLastError = true, BestFitMapping = false)]
  private static extern int DocumentProperties(
    IntPtr hwnd,
    IntPtr hPrinter,
    [MarshalAs(UnmanagedType.LPWStr)] string pDeviceName,
    IntPtr pDevModeOutput,
    IntPtr pDevModeInput,
    int fMode);

  [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  private static extern IntPtr GlobalLock(IntPtr handle);

  [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  private static extern bool GlobalUnlock(IntPtr handle);

  private struct PRINTER_DEFAULTS
  {
    public IntPtr pDatatype;
    public IntPtr pDevMode;
    public int DesiredAccess;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
  private class DEVMODE
  {
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32 /*0x20*/)]
    public string dmDeviceName;
    public short dmSpecVersion;
    public short dmDriverVersion;
    public short dmSize;
    public short dmDriverExtra;
    public int dmFields;
    public short dmOrientation;
    public short dmPaperSize;
    public short dmPaperLength;
    public short dmPaperWidth;
    public short dmScale;
    public short dmCopies;
    public short dmDefaultSource;
    public short dmPrintQuality;
    public short dmColor;
    public short dmDuplex;
    public short dmYResolution;
    public short dmTTOption;
    public short dmCollate;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32 /*0x20*/)]
    public string dmFormName;
    public short dmLogPixels;
    public int dmBitsPerPel;
    public int dmPelsWidth;
    public int dmPelsHeight;
    public int dmDisplayFlags;
    public int dmDisplayFrequency;
    public int dmICMMethod;
    public int dmICMIntent;
    public int dmMediaType;
    public int dmDitherType;
    public int dmICCManufacturer;
    public int dmICCModel;
    public int dmPanningWidth;
    public int dmPanningHeight;

    public override string ToString()
    {
      return $"[DEVMODE: dmDeviceName={this.dmDeviceName}, dmSpecVersion={this.dmSpecVersion.ToString()}, dmDriverVersion={this.dmDriverVersion.ToString()}, dmSize={this.dmSize.ToString()}, dmDriverExtra={this.dmDriverExtra.ToString()}, dmFields={this.dmFields.ToString()}, dmOrientation={this.dmOrientation.ToString()}, dmPaperSize={this.dmPaperSize.ToString()}, dmPaperLength={this.dmPaperLength.ToString()}, dmPaperWidth={this.dmPaperWidth.ToString()}, dmScale={this.dmScale.ToString()}, dmCopies={this.dmCopies.ToString()}, dmDefaultSource={this.dmDefaultSource.ToString()}, dmPrintQuality={this.dmPrintQuality.ToString()}, dmColor={this.dmColor.ToString()}, dmDuplex={this.dmDuplex.ToString()}, dmYResolution={this.dmYResolution.ToString()}, dmTTOption={this.dmTTOption.ToString()}, dmCollate={this.dmCollate.ToString()}, dmFormName={this.dmFormName}, dmLogPixels={this.dmLogPixels.ToString()}, dmBitsPerPel={this.dmBitsPerPel.ToString()}, dmPelsWidth={this.dmPelsWidth.ToString()}, dmPelsHeight={this.dmPelsHeight.ToString()}, dmDisplayFlags={this.dmDisplayFlags.ToString()}, dmDisplayFrequency={this.dmDisplayFrequency.ToString()}, dmICMMethod={this.dmICMMethod.ToString()}, dmICMIntent={this.dmICMIntent.ToString()}, dmMediaType={this.dmMediaType.ToString()}, dmDitherType={this.dmDitherType.ToString()}, dmICCManufacturer={this.dmICCManufacturer.ToString()}, dmICCModel={this.dmICCModel.ToString()}, dmPanningWidth={this.dmPanningWidth.ToString()}, dmPanningHeight={this.dmPanningHeight.ToString()}]";
    }
  }

  private enum DM
  {
    DMDUP_UNKNOWN,
    DMDUP_SIMPLEX,
    DMDUP_VERTICAL,
    DMDUP_HORIZONTAL,
  }

  [Flags]
  private enum DocumentPropertiesMode
  {
    DM_OUT_BUFFER = 2,
    DM_IN_PROMPT = 4,
    DM_IN_BUFFER = 8,
  }

  private struct POINTL
  {
    public int x;
    public int y;
  }
}
