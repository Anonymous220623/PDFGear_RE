// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Exceptions.UnexpectedTypeException
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Properties;
using System;

#nullable disable
namespace Patagames.Pdf.Net.Exceptions;

/// <summary>
/// This exception is commonly raised when the annotation cannot be parsed.
/// </summary>
/// <summary>
/// Initializes a new instance of the InvalidAnnotationException class.
/// </summary>
/// <param name="expected">Expected type.</param>
/// <param name="actual">Actual type.</param>
public class UnexpectedTypeException(Type expected, Type actual) : PdfiumException(11U, string.Format(Error.err0050, (object) expected.Name, (object) actual.Name))
{
}
