// Decompiled with JetBrains decompiler
// Type: PDFKit.NativeMethods
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.Runtime.InteropServices;
using System.Security;

#nullable disable
namespace PDFKit;

internal static class NativeMethods
{
  [SecurityCritical]
  [SuppressUnmanagedCodeSecurity]
  [DllImport("kernel32.dll")]
  internal static extern IntPtr GlobalFree(IntPtr hMem);

  [SuppressUnmanagedCodeSecurity]
  [SecurityCritical]
  [DllImport("kernel32.dll")]
  internal static extern IntPtr GlobalLock(IntPtr hMem);

  [SuppressUnmanagedCodeSecurity]
  [SecurityCritical]
  [DllImport("kernel32.dll")]
  internal static extern bool GlobalUnlock(IntPtr hMem);

  [SecurityCritical]
  [SuppressUnmanagedCodeSecurity]
  [DllImport("comdlg32.dll", CharSet = CharSet.Auto)]
  internal static extern int PrintDlgEx(IntPtr pdex);

  [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Auto)]
  internal struct DEVMODE
  {
    private const int CCHDEVICENAME = 32 /*0x20*/;
    private const int CCHFORMNAME = 32 /*0x20*/;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32 /*0x20*/)]
    public string dmDeviceName;
    public short dmSpecVersion;
    public short dmDriverVersion;
    public short dmSize;
    public short dmDriverExtra;
    public int dmFields;
    public int dmPositionX;
    public int dmPositionY;
    public int dmDisplayOrientation;
    public int dmDisplayFixedOutput;
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
    public int dmReserved1;
    public int dmReserved2;
    public int dmPanningWidth;
    public int dmPanningHeight;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Auto)]
  internal struct DEVNAMES
  {
    public ushort wDriverOffset;
    public ushort wDeviceOffset;
    public ushort wOutputOffset;
    public ushort wDefault;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Auto)]
  internal class PRINTDLGEX32
  {
    public int lStructSize = Marshal.SizeOf(typeof (NativeMethods.PRINTDLGEX32));
    public IntPtr hwndOwner = IntPtr.Zero;
    public IntPtr hDevMode = IntPtr.Zero;
    public IntPtr hDevNames = IntPtr.Zero;
    public IntPtr hDC = IntPtr.Zero;
    public uint Flags;
    public uint Flags2;
    public uint ExclusionFlags;
    public uint nPageRanges;
    public uint nMaxPageRanges;
    public IntPtr lpPageRanges = IntPtr.Zero;
    public uint nMinPage;
    public uint nMaxPage;
    public uint nCopies;
    public IntPtr hInstance = IntPtr.Zero;
    public IntPtr lpPrintTemplateName = IntPtr.Zero;
    public IntPtr lpCallback = IntPtr.Zero;
    public uint nPropertyPages;
    public IntPtr lphPropertyPages = IntPtr.Zero;
    public uint nStartPage = uint.MaxValue;
    public uint dwResultAction;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 8, CharSet = CharSet.Auto)]
  internal class PRINTDLGEX64
  {
    public int lStructSize = Marshal.SizeOf(typeof (NativeMethods.PRINTDLGEX64));
    public IntPtr hwndOwner = IntPtr.Zero;
    public IntPtr hDevMode = IntPtr.Zero;
    public IntPtr hDevNames = IntPtr.Zero;
    public IntPtr hDC = IntPtr.Zero;
    public uint Flags;
    public uint Flags2;
    public uint ExclusionFlags;
    public uint nPageRanges;
    public uint nMaxPageRanges;
    public IntPtr lpPageRanges = IntPtr.Zero;
    public uint nMinPage;
    public uint nMaxPage;
    public uint nCopies;
    public IntPtr hInstance = IntPtr.Zero;
    public IntPtr lpPrintTemplateName = IntPtr.Zero;
    public IntPtr lpCallback = IntPtr.Zero;
    public uint nPropertyPages;
    public IntPtr lphPropertyPages = IntPtr.Zero;
    public uint nStartPage = uint.MaxValue;
    public uint dwResultAction;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Auto)]
  internal struct PRINTPAGERANGE
  {
    public uint nFromPage;
    public uint nToPage;
  }
}
