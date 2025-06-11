// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.Native.Win32
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

#nullable enable
namespace NAPS2.Wia.Native;

internal class Win32
{
  [DllImport("propsys.dll")]
  public static extern uint PropVariantGetElementCount(in PROPVARIANT propvar);

  [DllImport("propsys.dll", PreserveSig = false)]
  public static extern void PropVariantGetInt32Elem(
    in PROPVARIANT propvar,
    uint iElem,
    out int pnVal);

  [DllImport("propsys.dll", PreserveSig = false)]
  public static extern void PropVariantToInt32(in PROPVARIANT propvarIn, out int plRet);

  [DllImport("propsys.dll", PreserveSig = false)]
  public static extern void InitPropVariantFromInt32(int lVal, out PROPVARIANT ppropvar);

  [DllImport("propsys.dll", PreserveSig = false)]
  public static extern void PropVariantToBSTR(in PROPVARIANT propvar, [MarshalAs(UnmanagedType.BStr)] out string pbstrOut);

  [DllImport("propsys.dll", PreserveSig = false)]
  public static extern void PropVariantToInt32VectorAlloc(
    in PROPVARIANT propvar,
    [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] out int[] pprgn,
    out uint pcElem);

  [DllImport("ole32.dll", PreserveSig = false)]
  public static extern void PropVariantClear(in PROPVARIANT pvar);

  [DllImport("shlwapi.dll")]
  public static extern IStream SHCreateMemStream([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[]? pInit, uint cbInit);
}
