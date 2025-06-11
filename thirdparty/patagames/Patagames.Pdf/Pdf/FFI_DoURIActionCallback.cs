// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.FFI_DoURIActionCallback
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>
/// This action resolves to a uniform resource identifier.
/// </summary>
/// <param name="pThis">Pointer to the interface structure itself.</param>
/// <param name="bsURI">A byte string which indicates the uniform resource identifier, terminated by 0.</param>
/// <remarks>required: No. See the URI actions description of <a href="http://www.adobe.com/content/dam/Adobe/en/devnet/acrobat/pdfs/pdf_reference_1-7.pdf">PDF Reference, Version 1.7</a> for more details.</remarks>
/// <seealso href="http://www.adobe.com/content/dam/Adobe/en/devnet/acrobat/pdfs/pdf_reference_1-7.pdf">PDF Reference 1.7</seealso>
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void FFI_DoURIActionCallback([MarshalAs(UnmanagedType.LPStruct)] FPDF_FORMFILLINFO pThis, [MarshalAs(UnmanagedType.LPStr)] string bsURI);
