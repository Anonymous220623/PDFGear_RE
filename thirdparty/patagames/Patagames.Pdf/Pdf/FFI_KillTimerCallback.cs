// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.FFI_KillTimerCallback
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>
/// This method kills the timer event identified by nIDEvent, set by an earlier call to <see cref="F:Patagames.Pdf.FPDF_FORMFILLINFO.FFI_SetTimer" />.
/// </summary>
/// <param name="pThis">Pointer to the interface structure itself.</param>
/// <param name="nTimerID">The timer ID return by <see cref="F:Patagames.Pdf.FPDF_FORMFILLINFO.FFI_SetTimer" /> function.</param>
/// <remarks>Required: Yes.</remarks>
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void FFI_KillTimerCallback([MarshalAs(UnmanagedType.LPStruct)] FPDF_FORMFILLINFO pThis, int nTimerID);
