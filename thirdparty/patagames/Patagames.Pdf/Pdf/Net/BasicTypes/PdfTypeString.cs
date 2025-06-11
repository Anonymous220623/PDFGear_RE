// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.BasicTypes.PdfTypeString
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Net.BasicTypes;

/// <summary>Represents the String type of objects</summary>
/// <remarks>
/// A string object consists of a series of bytes—unsigned integer values in the range 0 to 255.
/// String objects are not integer objects, but are stored in a more compact format.
/// The length of a string may be subject to implementation limits.
/// <para>String objects can be written in two ways:</para>
/// <list type="bullet">
/// <item>As a sequence of literal characters</item>
/// <item>As hexadecimal data. e.g. 4E6F762073686D6F7A206B6120706F702E</item>
/// </list>
/// </remarks>
/// <summary>
/// Construct new instance of PdfTypeString class from given Handle
/// </summary>
/// <param name="Handle">A handle to the unmanaged String object</param>
public class PdfTypeString(IntPtr Handle) : PdfTypeBase(Handle)
{
  /// <summary>
  /// Gets a boolean value that indicates whether a String is a hex.
  /// </summary>
  public bool IsHex => Pdfium.FPDFSTRING_IsHex(this.Handle);

  /// <summary>Gets or sets the non unicode string</summary>
  public string AnsiString
  {
    get => Pdfium.FPDFOBJ_GetString(this.Handle);
    set => Pdfium.FPDFOBJ_SetString(this.Handle, value);
  }

  /// <summary>Gets the unicode string</summary>
  public string UnicodeString => Pdfium.FPDFOBJ_GetUnicodeText(this.Handle);

  /// <summary>
  /// Create new String object and initialise it with string
  /// </summary>
  /// <param name="initialVal">Initial value for this object</param>
  /// <param name="bUnicode">Indicates what the given string is a unicode string.</param>
  /// <param name="bHex">Indicates whether a value is a HEX. Used for non unicode strings only.</param>
  /// <returns>The instance of a newly created object</returns>
  public static PdfTypeString Create(string initialVal = null, bool bUnicode = false, bool bHex = false)
  {
    IntPtr zero = IntPtr.Zero;
    IntPtr Handle = initialVal != null ? (!bUnicode ? Pdfium.FPDFSTRING_CreateChar(initialVal, bHex) : Pdfium.FPDFSTRING_CreateUnicode(initialVal)) : Pdfium.FPDFSTRING_CreateEmpty();
    return !(Handle == IntPtr.Zero) ? new PdfTypeString(Handle) : throw Pdfium.ProcessLastError();
  }

  /// <summary>Creates new instance of PdfTypeString class</summary>
  /// <param name="handle">A handle to the unmanaged String object</param>
  /// <returns>An instance of PdfTypeString</returns>
  public static PdfTypeString Create(IntPtr handle)
  {
    return !(handle == IntPtr.Zero) ? new PdfTypeString(handle) : throw new ArgumentException();
  }
}
