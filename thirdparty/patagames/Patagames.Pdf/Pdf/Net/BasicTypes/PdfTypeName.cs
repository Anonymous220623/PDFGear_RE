// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.BasicTypes.PdfTypeName
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Net.BasicTypes;

/// <summary>Represents the Name type of objects</summary>
/// <remarks>
/// A name object is an atomic symbol uniquely defined by a sequence of characters.
/// Uniquely defined means that any two name objects made up of the same sequence of characters
/// are identically the same object.
/// Atomic means that a name has no internal structure; although it is defined by a sequence of characters,
/// those characters are not considered elements of the name.
/// <para>
/// The name may include any regular characters, but not delimiter or white-space characters.
/// Uppercase and lowercase letters are considered distinct: 'A' and 'a' are different names.
/// </para>
/// <para>Beginning with PDF 1.2, any character except null (character code 0) may be included in a name
/// by writing its 2-digit hexadecimal code, preceded by the number sign character (#).
/// This syntax is required to represent any of the delimiter or white-space characters or the number sign character itself;
/// it is recommended but not required for characters whose codes are outside the range 33 (!) to 126 (~).
/// </para>
/// <para>The length of a name is subject to an implementation limit;
/// The limit applies to the number of characters in the name’s internal representation. For example, the name A#20B has free characters (A, space, B), not five.</para>
/// <note type="note">PDF does not prescribe what UTF-8 sequence to choose for representing any given piece of externally specified text as a name object.
/// In some cases, multiple UTF-8 sequences could represent the same logical text.
/// Name objects defined by different sequences of bytes constitute distinct name objects in PDF,
/// even though the UTF-8 sequences might have identical external interpretations.</note>
/// </remarks>
/// <summary>
/// Construct new instance of PdfTypeName class from given Handle
/// </summary>
/// <param name="Handle">A handle to the unmanaged Name object</param>
public class PdfTypeName(IntPtr Handle) : PdfTypeBase(Handle)
{
  /// <summary>Gets or sets the value for this Name object</summary>
  public string Value
  {
    get => Pdfium.FPDFOBJ_GetString(this.Handle);
    set => Pdfium.FPDFOBJ_SetString(this.Handle, value);
  }

  /// <summary>Creates new Name object</summary>
  /// <param name="initialVal">Initial value for this object</param>
  /// <returns>The instance of a newly created object</returns>
  public static PdfTypeName Create(string initialVal)
  {
    IntPtr Handle = Pdfium.FPDFNAME_Create(initialVal);
    return !(Handle == IntPtr.Zero) ? new PdfTypeName(Handle) : throw Pdfium.ProcessLastError();
  }

  /// <summary>Creates new instance of PdfTypeName class</summary>
  /// <param name="handle">A handle to the unmanaged Name object</param>
  /// <returns>An instance of PdfTypeName</returns>
  public static PdfTypeName Create(IntPtr handle)
  {
    return !(handle == IntPtr.Zero) ? new PdfTypeName(handle) : throw new ArgumentException();
  }
}
