// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Doc_submitForm_callback
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>Send the form data to a specified URL.</summary>
/// <param name="pThis">Pointer to the interface structure itself</param>
/// <param name="formData">Pointer to the data buffer to be sent.</param>
/// <param name="length">The size,in bytes, of the buffer pointed by formData parameter.</param>
/// <param name="Url">The URL to send to.</param>
/// <remarks>Required: Yes.</remarks>
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void Doc_submitForm_callback(
  [MarshalAs(UnmanagedType.LPStruct)] IPDF_JSPLATFORM pThis,
  [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] formData,
  int length,
  [MarshalAs(UnmanagedType.LPWStr)] string Url);
