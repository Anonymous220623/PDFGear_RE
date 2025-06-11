// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.BasicTypes.PdfTypeUnknown
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using System;

#nullable disable
namespace Patagames.Pdf.Net.BasicTypes;

/// <summary>Represents the unknown (unsuported) type of objects</summary>
/// <summary>
/// Construct new instance of PdfTypeUnknown class from given Handle
/// </summary>
/// <param name="Handle">Handle to the unmanaged object</param>
public class PdfTypeUnknown(IntPtr Handle) : PdfTypeBase(Handle)
{
  private PdfTypeBase _direct;
  private IndirectObjectTypes _directType;
  private PdfTypeDictionary _dictionary;
  private PdfTypeArray _array;

  /// <summary>Gets the inderect object</summary>
  public PdfTypeBase Direct
  {
    get
    {
      IntPtr direct = Pdfium.FPDFOBJ_GetDirect(this.Handle);
      if (direct == IntPtr.Zero)
      {
        this._direct = (PdfTypeBase) null;
        return (PdfTypeBase) null;
      }
      IndirectObjectTypes type = Pdfium.FPDFOBJ_GetType(direct);
      if (this._direct != null && this._direct.Handle == direct && this._directType == type)
        return this._direct;
      this._direct = PdfTypeBase.Create(direct);
      this._directType = type;
      return this._direct;
    }
  }

  /// <summary>
  /// Gets a non-unicode string representation of the specified object.
  /// </summary>
  public string AnsiString
  {
    get => Pdfium.FPDFOBJ_GetString(this.Handle);
    set => Pdfium.FPDFOBJ_SetString(this.Handle, value);
  }

  /// <summary>
  /// Gets a unicode string representation of the specified object.
  /// </summary>
  public string UnicodeString => Pdfium.FPDFOBJ_GetUnicodeText(this.Handle);

  /// <summary>
  /// Gets a floating-point representation of the specified object.
  /// </summary>
  public float Number => Pdfium.FPDFOBJ_GetNumber(this.Handle);

  /// <summary>
  /// Gets an integer representation of the specified object.
  /// </summary>
  public int Integer => Pdfium.FPDFOBJ_GetInteger(this.Handle);

  /// <summary>
  /// Gets a Dictionary representation of the specified object.
  /// </summary>
  public PdfTypeDictionary Dictionary
  {
    get
    {
      IntPtr dict = Pdfium.FPDFOBJ_GetDict(this.Handle);
      if (dict == IntPtr.Zero)
        return (PdfTypeDictionary) null;
      if (this._dictionary != null && this._dictionary.Handle == dict)
        return this._dictionary;
      this._dictionary = new PdfTypeDictionary(dict);
      return this._dictionary;
    }
  }

  /// <summary>Gets an Array representation of the specified object.</summary>
  public PdfTypeArray Array
  {
    get
    {
      IntPtr array = Pdfium.FPDFOBJ_GetArray(this.Handle);
      if (array == IntPtr.Zero)
        return (PdfTypeArray) null;
      if (this._array != null && this._array.Handle == array)
        return this._array;
      this._array = new PdfTypeArray(array);
      return this._array;
    }
  }
}
