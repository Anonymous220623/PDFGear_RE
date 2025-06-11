// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.FFI_SetTextFieldFocusCallback
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>
/// This method will be called when a text field is getting or losing a focus.
/// </summary>
/// <param name="pThis">Pointer to the interface structure itself.</param>
/// <param name="value">The string value of the form field, in UTF-16LE format.</param>
/// <param name="valueLen">The length of the string value, number of characters (not bytes).</param>
/// <param name="is_focus">True if the form field is getting a focus, False for losing a focus.</param>
/// <remarks>required: No. Currently,only support text field and combobox field.</remarks>
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void FFI_SetTextFieldFocusCallback(
  [MarshalAs(UnmanagedType.LPStruct)] FPDF_FORMFILLINFO pThis,
  [MarshalAs(UnmanagedType.LPWStr)] string value,
  [MarshalAs(UnmanagedType.I4)] int valueLen,
  [MarshalAs(UnmanagedType.Bool)] bool is_focus);
