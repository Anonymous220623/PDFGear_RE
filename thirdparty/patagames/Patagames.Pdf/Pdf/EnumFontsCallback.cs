// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.EnumFontsCallback
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>Enumerate all fonts installed on the system</summary>
/// <param name="pThis">Pointer to the interface structure itself</param>
/// <param name="pMapper">An opaque pointer to internal font mapper, used when calling <see cref="M:Patagames.Pdf.Pdfium.FPDF_AddInstalledFont(System.IntPtr,System.String,Patagames.Pdf.Enums.CharacterSetTypes)" /></param>
/// <remarks>Required: No. Implementation should call <see cref="M:Patagames.Pdf.Pdfium.FPDF_AddInstalledFont(System.IntPtr,System.String,Patagames.Pdf.Enums.CharacterSetTypes)" /> function for each font found.
/// Only TrueType/OpenType and Type1 fonts are accepted by Pdfium SDK.</remarks>
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void EnumFontsCallback([MarshalAs(UnmanagedType.LPStruct)] FPDF_SYSFONTINFO pThis, IntPtr pMapper);
