// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.FFI_SetTimerCallback
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>
/// This method installs a system timer. A time-out value is specified,
/// and every time a time-out occurs, the system passes a message to
/// the TimerProc callback function.
/// </summary>
/// <param name="pThis">Pointer to the interface structure itself.</param>
/// <param name="uElapse">Specifies the time-out value, in milliseconds.</param>
/// <param name="lpTimerFunc">A pointer to the callback function-TimerCallback.</param>
/// <returns>The timer identifier of the new timer if the function is successful.
/// An application passes this value to the <see cref="F:Patagames.Pdf.FPDF_FORMFILLINFO.FFI_KillTimer" /> method to kill
/// the timer. Nonzero if it is successful; otherwise, it is zero.</returns>
/// <remarks>Required: Yes.</remarks>
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate int FFI_SetTimerCallback(
  [MarshalAs(UnmanagedType.LPStruct)] FPDF_FORMFILLINFO pThis,
  int uElapse,
  TimerCallback lpTimerFunc);
