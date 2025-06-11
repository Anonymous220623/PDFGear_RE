// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.BasicTypes.PdfTypeNumber
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Net.BasicTypes;

/// <summary>Represents the Numeric type of objects</summary>
/// <remarks>
/// PDF provides two types of numeric objects: integer and real.
/// Integer objects represent mathematical integers within a certain interval centered at 0.
/// Real objectsapproximate mathematical real numbers, but with limited range and precision;
/// they are typically represented in fixed-point form rather than floating-point form.
/// The range and precision of numbers are limited by the internal representations used in the computer
/// on which the PDF consumer application is running.
/// </remarks>
/// <summary>
/// Construct new instance of PdfTypeNumber class from given Handle
/// </summary>
/// <param name="Handle">A handle to the unmanaged Number object</param>
public class PdfTypeNumber(IntPtr Handle) : PdfTypeBase(Handle)
{
  /// <summary>Gets or sets the value for this Number object</summary>
  public int IntValue
  {
    get => Pdfium.FPDFOBJ_GetInteger(this.Handle);
    set => Pdfium.FPDFOBJ_SetString(this.Handle, value.ToString());
  }

  /// <summary>Gets or sets the value for this Number object</summary>
  public float FloatValue
  {
    get => Pdfium.FPDFOBJ_GetNumber(this.Handle);
    set
    {
      Pdfium.FPDFOBJ_SetString(this.Handle, value.ToString().Replace(",", ".").Replace(" ", ""));
    }
  }

  /// <summary>
  /// Gets a boolean value that indicates whether a Number is an integer.
  /// </summary>
  public bool IsInteger => Pdfium.FPDFNUMBER_IsInteger(this.Handle);

  /// <summary>Creates new Number object</summary>
  /// <param name="initialVal">Initial value for this object</param>
  /// <returns>The instance of a newly created object</returns>
  public static PdfTypeNumber Create(int initialVal)
  {
    IntPtr Handle = Pdfium.FPDFNUMBER_CreateInt(initialVal);
    return !(Handle == IntPtr.Zero) ? new PdfTypeNumber(Handle) : throw Pdfium.ProcessLastError();
  }

  /// <summary>Creates new Number object</summary>
  /// <param name="initialVal">Initial value for this object</param>
  /// <returns>The instance of a newly created object</returns>
  public static PdfTypeNumber Create(float initialVal)
  {
    IntPtr Handle = Pdfium.FPDFNUMBER_CreateFloat(initialVal);
    return !(Handle == IntPtr.Zero) ? new PdfTypeNumber(Handle) : throw Pdfium.ProcessLastError();
  }

  /// <summary>Creates new instance of PdfTypeNumber class</summary>
  /// <param name="handle">A handle to the unmanaged Number object</param>
  /// <returns>An instance of PdfTypeNumber</returns>
  public static PdfTypeNumber Create(IntPtr handle)
  {
    return !(handle == IntPtr.Zero) ? new PdfTypeNumber(handle) : throw new ArgumentException();
  }
}
