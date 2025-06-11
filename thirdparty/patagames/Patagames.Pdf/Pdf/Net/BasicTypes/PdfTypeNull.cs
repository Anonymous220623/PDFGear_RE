// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.BasicTypes.PdfTypeNull
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Net.BasicTypes;

/// <summary>Represents the Null type of objects</summary>
/// <remarks>
/// The null object has a type and value that are unequal to those of any other object.
/// There is only one object of type null, denoted by the keyword null.
/// An indirect object reference to a nonexistent object is treated the same as a null object.
/// Specifying the null object as the value of a dictionary entry is equivalent to omitting the entry entirely.
/// </remarks>
/// <summary>
/// Construct new instance of PdfTypeNull class from given Handle
/// </summary>
/// <param name="Handle">A handle to the unmanaged Null object</param>
public class PdfTypeNull(IntPtr Handle) : PdfTypeBase(Handle)
{
  /// <summary>Creates new Null object</summary>
  /// <returns>The instance of a newly created object</returns>
  public static PdfTypeNull Create()
  {
    IntPtr Handle = Pdfium.FPDFNULL_Create();
    return !(Handle == IntPtr.Zero) ? new PdfTypeNull(Handle) : throw Pdfium.ProcessLastError();
  }

  /// <summary>Creates new instance of PdfTypeNull class</summary>
  /// <param name="handle">A handle to the unmanaged Null object</param>
  /// <returns>An instance of PdfTypeNull</returns>
  public static PdfTypeNull Create(IntPtr handle)
  {
    return !(handle == IntPtr.Zero) ? new PdfTypeNull(handle) : throw new ArgumentException();
  }
}
