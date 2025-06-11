// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.TimerCallback
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>
/// Declares of a pointer type to the callback function for the <see cref="F:Patagames.Pdf.FPDF_FORMFILLINFO.FFI_SetTimer" /> method.
/// </summary>
/// <param name="idEvent">dentifier of the timer.</param>
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void TimerCallback(int idEvent);
