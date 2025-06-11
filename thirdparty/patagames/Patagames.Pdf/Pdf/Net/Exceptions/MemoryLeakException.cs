// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Exceptions.MemoryLeakException
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Properties;

#nullable disable
namespace Patagames.Pdf.Net.Exceptions;

/// <summary>
/// The exception that is thrown when the undisposed instance of class was found.
/// </summary>
/// <summary>
/// Initializes a new instance of the MemoryLeakException class.
/// </summary>
/// <param name="className">A class name there memory leak was detected.</param>
public class MemoryLeakException(string className) : PdfiumException(9U, string.Format(Error.err0018, (object) className))
{
}
