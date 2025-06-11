// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.GetFontCallback
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>Get a handle to a particular font by its internal ID</summary>
/// <param name="pThis">Pointer to the interface structure itself</param>
/// <param name="face">Typeface name. Currently use system local encoding only.</param>
/// <returns>An opaque pointer for font handle.</returns>
/// <remarks>Required only if <see cref="F:Patagames.Pdf.FPDF_SYSFONTINFO.MapFont" /> method is not implemented.
/// If the system mapping not supported, Pdfium SDK will do the font mapping and use this method to get a font handle.</remarks>
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate IntPtr GetFontCallback([MarshalAs(UnmanagedType.LPStruct)] FPDF_SYSFONTINFO pThis, [MarshalAs(UnmanagedType.LPStr)] string face);
