// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Doc_gotoPage_callback
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>Jump to a specified page.</summary>
/// <param name="pThis">Pointer to the interface structure itself</param>
/// <param name="nPageNum">The specified page number, zero for the first page.</param>
/// <remarks>Required: Yes.</remarks>
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void Doc_gotoPage_callback([MarshalAs(UnmanagedType.LPStruct)] IPDF_JSPLATFORM pThis, int nPageNum);
