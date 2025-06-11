// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfAttachment
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.Wrappers;
using System;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>Represents attachment of document.</summary>
public class PdfAttachment
{
  /// <summary>Gets the name of the attachment.</summary>
  public string Name { get; private set; }

  /// <summary>Gets the content of the attachment.</summary>
  public byte[] Content
  {
    get
    {
      return (PdfWrapper) this.FileSpecification == (PdfWrapper) null || (PdfWrapper) this.FileSpecification.EmbeddedFile == (PdfWrapper) null || this.FileSpecification.EmbeddedFile.Stream == null ? (byte[]) null : this.FileSpecification.EmbeddedFile.Stream.DecodedData;
    }
  }

  /// <summary>Gets a file specification described the attachment.</summary>
  public PdfFileSpecification FileSpecification { get; private set; }

  /// <summary>
  /// Initialize new instance of <see cref="T:Patagames.Pdf.Net.PdfAttachment" /> class.
  /// </summary>
  /// <param name="name">The name of the attachment.</param>
  /// <param name="spec">A file specification described the attachment.</param>
  /// <exception cref="T:System.ArgumentNullException"><paramref name="name" /> is null.</exception>
  public PdfAttachment(string name, PdfFileSpecification spec)
  {
    this.Name = name != null ? name : throw new ArgumentNullException(nameof (name));
    this.FileSpecification = spec;
  }

  /// <summary>Create a new attachment.</summary>
  /// <param name="document">The document with which the attachment is associated.</param>
  /// <param name="fileName">The name of the attachment.</param>
  /// <param name="fileContent">The content of the attachment.</param>
  /// <exception cref="T:System.ArgumentNullException"><paramref name="document" />, <paramref name="fileContent" /> or <paramref name="fileName" /> is null.</exception>
  public PdfAttachment(PdfDocument document, string fileName, byte[] fileContent)
  {
    if (fileName == null)
      throw new ArgumentNullException(nameof (fileName));
    if (fileContent == null)
      throw new ArgumentNullException(nameof (fileContent));
    this.FileSpecification = new PdfFileSpecification(document);
    this.FileSpecification.FileName = fileName;
    this.FileSpecification.EmbeddedFile = new PdfFile(fileContent);
    this.Name = fileName;
  }
}
