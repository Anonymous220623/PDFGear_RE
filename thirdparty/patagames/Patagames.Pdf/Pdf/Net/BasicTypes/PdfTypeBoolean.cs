// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.BasicTypes.PdfTypeBoolean
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;

#nullable disable
namespace Patagames.Pdf.Net.BasicTypes;

/// <summary>Represents the Boolean type of objects</summary>
/// <remarks>
/// PDF provides boolean objects identified by the keywords true and false.
/// Boolean objects can be used as the values of array elements and dictionary entries,
/// and can also occur in PostScript calculator functions as the results of boolean
/// and relational operators and as operands to the conditional operators if and ifelse.
/// </remarks>
/// <summary>
/// Construct new instance of PdfTypeBoolean class from given Handle
/// </summary>
/// <param name="Handle">A handle to the unmanaged Boolean object</param>
public class PdfTypeBoolean(IntPtr Handle) : PdfTypeBase(Handle)
{
  /// <summary>Gets or sets the value for this Boolean object</summary>
  public bool Value
  {
    get => Pdfium.FPDFOBJ_GetInteger(this.Handle) == 1;
    set => Pdfium.FPDFOBJ_SetString(this.Handle, value ? "true" : "false");
  }

  /// <summary>Creates new Boolean object</summary>
  /// <param name="bInitialVal">Initial value for this object</param>
  /// <returns>The instance of a newly created object</returns>
  public static PdfTypeBoolean Create(bool bInitialVal)
  {
    IntPtr Handle = Pdfium.FPDFBOOLEAN_Create(bInitialVal);
    return !(Handle == IntPtr.Zero) ? new PdfTypeBoolean(Handle) : throw Pdfium.ProcessLastError();
  }

  /// <summary>Creates new instance of PdfTypeBoolean class</summary>
  /// <param name="handle">A handle to the unmanaged Boolean object</param>
  /// <returns>An instance of PdfTypeBoolean</returns>
  public static PdfTypeBoolean Create(IntPtr handle)
  {
    return !(handle == IntPtr.Zero) ? new PdfTypeBoolean(handle) : throw new ArgumentException();
  }
}
