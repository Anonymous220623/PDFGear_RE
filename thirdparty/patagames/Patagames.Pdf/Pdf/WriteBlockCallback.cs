// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.WriteBlockCallback
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>Output a block of data in your custom way.</summary>
/// <param name="pThis">Pointer to the structure itself</param>
/// <param name="buffer">Pointer to a buffer to output</param>
/// <param name="buflen">The size of the buffer</param>
/// <returns>Should be non-zero if successful, zero for error</returns>
/// <remarks>Required: Yes. Called by function <see cref="M:Patagames.Pdf.Pdfium.FPDF_SaveAsCopy(System.IntPtr,Patagames.Pdf.FPDF_FILEWRITE,Patagames.Pdf.Enums.SaveFlags)" /> and <see cref="M:Patagames.Pdf.Pdfium.FPDF_SaveWithVersion(System.IntPtr,Patagames.Pdf.FPDF_FILEWRITE,Patagames.Pdf.Enums.SaveFlags,System.Int32)" /></remarks>
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
[return: MarshalAs(UnmanagedType.Bool)]
public delegate bool WriteBlockCallback([MarshalAs(UnmanagedType.LPStruct)] FPDF_FILEWRITE pThis, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] buffer, int buflen);
