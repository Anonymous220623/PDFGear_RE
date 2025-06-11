// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.BeforeSelectionChangeCallback
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>
/// This delegate invoked by SDK before field's selection is changed.
/// </summary>
/// <param name="pThis">Pointer to the interface structure itself</param>
/// <param name="field">Handle to Field object</param>
/// <param name="value">Field's value</param>
/// <returns>0 to accept changes, -1 to cancel</returns>
/// <remarks>Required: Yes.
/// <para>Delegate is called by the list boxes when it lose focus.</para>
/// </remarks>
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate int BeforeSelectionChangeCallback(
  [MarshalAs(UnmanagedType.LPStruct)] FPDF_FORMFILLNOTIFY pThis,
  IntPtr field,
  [MarshalAs(UnmanagedType.LPWStr)] string value);
