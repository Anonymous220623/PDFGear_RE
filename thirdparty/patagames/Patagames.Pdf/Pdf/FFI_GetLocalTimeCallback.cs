// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.FFI_GetLocalTimeCallback
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>
/// This method receives the current local time on the system.
/// </summary>
/// <param name="pThis">Pointer to the interface structure itself.</param>
/// <returns><see cref="T:Patagames.Pdf.FPDF_SYSTEMTIME" /> structure filled by current time</returns>
/// <remarks>Required: Yes.</remarks>
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
[return: MarshalAs(UnmanagedType.Struct)]
public delegate FPDF_SYSTEMTIME FFI_GetLocalTimeCallback([MarshalAs(UnmanagedType.LPStruct)] FPDF_FORMFILLINFO pThis);
