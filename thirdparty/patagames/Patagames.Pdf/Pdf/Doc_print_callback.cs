// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Doc_print_callback
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>
/// Prints all or a specific number of pages of the document.
/// </summary>
/// <param name="pThis">Pointer to the interface structure itself.</param>
/// <param name="bUI">If true, will cause a UI to be presented to the user to obtain printing information and confirm the action.</param>
/// <param name="nStart">A 0-based index that defines the start of an inclusive range of pages.</param>
/// <param name="nEnd">A 0-based index that defines the end of an inclusive page range.</param>
/// <param name="bSilent">If true, suppresses the cancel dialog box while the document is printing. The default is false.</param>
/// <param name="bShrinkToFit">If true, the page is shrunk (if necessary) to fit within the imageable area of the printed page.</param>
/// <param name="bPrintAsImage">If true, print pages as an image.</param>
/// <param name="bReverse">If true, print from nEnd to nStart.</param>
/// <param name="bAnnotations">If true (the default), annotations are printed.</param>
/// <remarks>Required: Yes.</remarks>
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void Doc_print_callback(
  [MarshalAs(UnmanagedType.LPStruct)] IPDF_JSPLATFORM pThis,
  [MarshalAs(UnmanagedType.Bool)] bool bUI,
  int nStart,
  int nEnd,
  [MarshalAs(UnmanagedType.Bool)] bool bSilent,
  [MarshalAs(UnmanagedType.Bool)] bool bShrinkToFit,
  [MarshalAs(UnmanagedType.Bool)] bool bPrintAsImage,
  [MarshalAs(UnmanagedType.Bool)] bool bReverse,
  [MarshalAs(UnmanagedType.Bool)] bool bAnnotations);
