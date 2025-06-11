// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.FFI_DoGoToActionCallback
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>
/// This action changes the view to a specified destination.
/// </summary>
/// <param name="pThis">Pointer to the interface structure itself.</param>
/// <param name="nPageIndex">The index of the PDF page.</param>
/// <param name="zoomMode">The zoom mode for viewing page.See ZoomTypes. </param>
/// <param name="fPosArray">The float array which carries the position info</param>
/// <param name="sizeofArray">The size of float array.</param>
/// <remarks>Required: No. See the Destinations description of <a href="http://www.adobe.com/content/dam/Adobe/en/devnet/acrobat/pdfs/pdf_reference_1-7.pdf">PDF Reference, Version 1.7</a> in 8.2.1 for more details.</remarks>
/// <seealso href="http://www.adobe.com/content/dam/Adobe/en/devnet/acrobat/pdfs/pdf_reference_1-7.pdf">PDF Reference 1.7</seealso>
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void FFI_DoGoToActionCallback(
  [MarshalAs(UnmanagedType.LPStruct)] FPDF_FORMFILLINFO pThis,
  int nPageIndex,
  ZoomTypes zoomMode,
  [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4, ArraySubType = UnmanagedType.R4)] float[] fPosArray,
  int sizeofArray);
