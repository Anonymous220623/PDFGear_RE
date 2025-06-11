// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.AfterFormImportDataCallback
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>
/// This delegate invoked by SDK after forms data is imported.
/// </summary>
/// <param name="pThis">Pointer to the interface structure itself</param>
/// <param name="intForm">Handle to Interactive Forms object</param>
/// <remarks>Required: Yes.
/// <para>Delegate is called by <see cref="M:Patagames.Pdf.Net.PdfInteractiveForms.ImportFromFdf(Patagames.Pdf.Net.FdfDocument)" /> and by <see cref="M:Patagames.Pdf.Pdfium.FPDFInterForm_ImportFromFDF(System.IntPtr,System.IntPtr,System.Boolean)" /></para>
/// </remarks>
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void AfterFormImportDataCallback([MarshalAs(UnmanagedType.LPStruct)] FPDF_FORMFILLNOTIFY pThis, IntPtr intForm);
