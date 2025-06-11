// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.GetFaceNameCallback
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>Get face name from a font handle</summary>
/// <param name="pThis">Pointer to the interface structure itself</param>
/// <param name="hFont">Font handle returned by <see cref="F:Patagames.Pdf.FPDF_SYSFONTINFO.MapFont" /> or <see cref="F:Patagames.Pdf.FPDF_SYSFONTINFO.GetFont" /> method</param>
/// <param name="buffer">The buffer receiving the face name. Can be NULL if not provided</param>
/// <param name="buf_size">Buffer size, can be zero if not provided</param>
/// <returns>Number of bytes needed, if buffer not provided or not large enough, or number of bytes written into buffer otherwise.</returns>
/// <remarks>Required: No.</remarks>
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
[return: MarshalAs(UnmanagedType.U4)]
public delegate uint GetFaceNameCallback(
  [MarshalAs(UnmanagedType.LPStruct)] FPDF_SYSFONTINFO pThis,
  IntPtr hFont,
  [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3), In, Out] byte[] buffer,
  [MarshalAs(UnmanagedType.U4)] uint buf_size);
