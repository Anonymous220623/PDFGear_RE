// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Wrappers.PdfFileSpecification
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;
using System;

#nullable disable
namespace Patagames.Pdf.Net.Wrappers;

/// <summary>Represents a file sppecification</summary>
/// <remarks>
/// A PDF file can refer to the contents of another file by using a file specification (PDF 1.1), which can take either of two forms:
/// <list type="bullet">
/// <item>A simple file specification gives just the name of the target file in a standard format, independent of the naming conventions of any particular file system. It can take the form of either a string or a dictionary</item>
/// <item>A full file specification includes information related to one or more specific file systems.</item>
/// </list>
/// <para>
/// Although the file designated by a file specification is normally external to the
/// PDF file referring to it, PDF 1.3 permits a copy of the external file to be
/// embedded within the referring PDF file, allowing its contents to be stored or
/// transmitted along with the PDF file. However, embedding a file does not change
/// the presumption that it is external to the PDF file. Consequently, to ensure that
/// the PDF file can be processed correctly, it may be necessary to copy its embedded
/// files back into a local file system.
/// </para>
/// </remarks>
public class PdfFileSpecification : PdfWrapper
{
  private PdfFile _embeddedFile;
  private PdfIndirectList _listOfIndirectObjects;

  private PdfTypeStream _embeddedFileStream
  {
    get
    {
      PdfTypeDictionary pdfTypeDictionary = this.Dictionary["EF"].As<PdfTypeDictionary>();
      if (pdfTypeDictionary.ContainsKey("UF"))
        return pdfTypeDictionary["UF"].As<PdfTypeStream>();
      if (pdfTypeDictionary.ContainsKey("F"))
        return pdfTypeDictionary["F"].As<PdfTypeStream>();
      if (pdfTypeDictionary.ContainsKey("DOS"))
        return pdfTypeDictionary["DOS"].As<PdfTypeStream>();
      if (pdfTypeDictionary.ContainsKey("Unix"))
        return pdfTypeDictionary["Unix"].As<PdfTypeStream>();
      return pdfTypeDictionary.ContainsKey("Mac") ? pdfTypeDictionary["Mac"].As<PdfTypeStream>() : (PdfTypeStream) null;
    }
    set
    {
      PdfTypeDictionary pdfTypeDictionary = this.Dictionary["EF"].As<PdfTypeDictionary>();
      if (value == null)
      {
        if (pdfTypeDictionary.ContainsKey("UF"))
          pdfTypeDictionary.Remove("UF");
        if (pdfTypeDictionary.ContainsKey("F"))
          pdfTypeDictionary.Remove("F");
        if (pdfTypeDictionary.ContainsKey("DOS"))
          pdfTypeDictionary.Remove("DOS");
        if (pdfTypeDictionary.ContainsKey("Unix"))
          pdfTypeDictionary.Remove("Unix");
        if (!pdfTypeDictionary.ContainsKey("Mac"))
          return;
        pdfTypeDictionary.Remove("Mac");
      }
      else
      {
        this._listOfIndirectObjects.Add((PdfTypeBase) value);
        pdfTypeDictionary.SetIndirectAt("UF", this._listOfIndirectObjects, (PdfTypeBase) value);
        pdfTypeDictionary.SetIndirectAt("F", this._listOfIndirectObjects, (PdfTypeBase) value);
        if (pdfTypeDictionary.ContainsKey("DOS"))
          pdfTypeDictionary.Remove("DOS");
        if (pdfTypeDictionary.ContainsKey("Unix"))
          pdfTypeDictionary.Remove("Unix");
        if (!pdfTypeDictionary.ContainsKey("Mac"))
          return;
        pdfTypeDictionary.Remove("Mac");
      }
    }
  }

  private PdfTypeArray _relatedFilesArray
  {
    get
    {
      PdfTypeDictionary pdfTypeDictionary = this.Dictionary["RF"].As<PdfTypeDictionary>();
      if (pdfTypeDictionary.ContainsKey("UF"))
        return pdfTypeDictionary["UF"].As<PdfTypeArray>();
      if (pdfTypeDictionary.ContainsKey("F"))
        return pdfTypeDictionary["F"].As<PdfTypeArray>();
      if (pdfTypeDictionary.ContainsKey("DOS"))
        return pdfTypeDictionary["DOS"].As<PdfTypeArray>();
      if (pdfTypeDictionary.ContainsKey("Unix"))
        return pdfTypeDictionary["Unix"].As<PdfTypeArray>();
      return pdfTypeDictionary.ContainsKey("Mac") ? pdfTypeDictionary["Mac"].As<PdfTypeArray>() : (PdfTypeArray) null;
    }
    set
    {
      PdfTypeDictionary pdfTypeDictionary = this.Dictionary["RF"].As<PdfTypeDictionary>();
      if (value == null)
      {
        if (pdfTypeDictionary.ContainsKey("UF"))
          pdfTypeDictionary.Remove("UF");
        if (pdfTypeDictionary.ContainsKey("F"))
          pdfTypeDictionary.Remove("F");
        if (pdfTypeDictionary.ContainsKey("DOS"))
          pdfTypeDictionary.Remove("DOS");
        if (pdfTypeDictionary.ContainsKey("Unix"))
          pdfTypeDictionary.Remove("Unix");
        if (!pdfTypeDictionary.ContainsKey("Mac"))
          return;
        pdfTypeDictionary.Remove("Mac");
      }
      else
      {
        this._listOfIndirectObjects.Add((PdfTypeBase) value);
        pdfTypeDictionary.SetIndirectAt("UF", this._listOfIndirectObjects, (PdfTypeBase) value);
        pdfTypeDictionary.SetIndirectAt("F", this._listOfIndirectObjects, (PdfTypeBase) value);
        if (pdfTypeDictionary.ContainsKey("DOS"))
          pdfTypeDictionary.Remove("DOS");
        if (pdfTypeDictionary.ContainsKey("Unix"))
          pdfTypeDictionary.Remove("Unix");
        if (!pdfTypeDictionary.ContainsKey("Mac"))
          return;
        pdfTypeDictionary.Remove("Mac");
      }
    }
  }

  private string _fileName
  {
    get
    {
      if (this.IsExists("UF"))
        return this.Dictionary["UF"].As<PdfTypeString>().UnicodeString;
      if (this.IsExists("F"))
        return this.Dictionary["F"].As<PdfTypeString>().UnicodeString;
      if (this.IsExists("DOS"))
        return this.Dictionary["DOS"].As<PdfTypeString>().AnsiString;
      if (this.IsExists("Unix"))
        return this.Dictionary["Unix"].As<PdfTypeString>().AnsiString;
      return this.IsExists("Mac") ? this.Dictionary["Mac"].As<PdfTypeString>().AnsiString : (string) null;
    }
    set
    {
      if (value == null)
      {
        if (this.Dictionary.ContainsKey("UF"))
          this.Dictionary.Remove("UF");
        if (this.Dictionary.ContainsKey("F"))
          this.Dictionary.Remove("F");
        if (this.Dictionary.ContainsKey("DOS"))
          this.Dictionary.Remove("DOS");
        if (this.Dictionary.ContainsKey("Unix"))
          this.Dictionary.Remove("Unix");
        if (!this.Dictionary.ContainsKey("Mac"))
          return;
        this.Dictionary.Remove("Mac");
      }
      else
      {
        this.Dictionary["UF"] = (PdfTypeBase) PdfTypeString.Create(value, true);
        this.Dictionary["F"] = (PdfTypeBase) PdfTypeString.Create(value);
        if (this.Dictionary.ContainsKey("DOS"))
          this.Dictionary.Remove("DOS");
        if (this.Dictionary.ContainsKey("Unix"))
          this.Dictionary.Remove("Unix");
        if (!this.Dictionary.ContainsKey("Mac"))
          return;
        this.Dictionary.Remove("Mac");
      }
    }
  }

  /// <summary>
  /// Gets or sets the name of the file system to be used to interpret this file specification.
  /// </summary>
  /// <remarks>
  /// If value is not null, all other properties are interpreted by the designated file system.
  /// PDF defines only one standard file system name, URL; an application or plug-in extension
  /// can register other names. This
  /// Property is independent of the <see cref="P:Patagames.Pdf.Net.Wrappers.PdfFileSpecification.FileName" /> property.
  /// </remarks>
  private string _fileSystem
  {
    get => !this.IsExists("FS") ? (string) null : this.Dictionary["FS"].As<PdfTypeName>().Value;
    set
    {
      if (value == null && this.Dictionary.ContainsKey("FS"))
      {
        this.Dictionary.Remove("FS");
      }
      else
      {
        if (value == null)
          return;
        this.Dictionary["FS"] = (PdfTypeBase) PdfTypeName.Create(value);
      }
    }
  }

  /// <summary>Gets or sets a file specification string.</summary>
  /// <remarks>
  /// A file specification string of the form described in section “File Specification Strings”
  /// or (if the file system is URL) a uniform resource locator, as described in Section “URL Specifications”
  /// in remarks of <see cref="T:Patagames.Pdf.Net.Wrappers.PdfFileSpecification" /> reference.
  /// </remarks>
  public string FileName
  {
    get => this._fileName;
    set => this._fileName = value;
  }

  /// <summary>
  /// Gets or sets a flag indicating that the file name should be interpreted as an URL
  /// </summary>
  public bool IsUrl
  {
    get => !(this._fileSystem ?? "").Equals("URL", StringComparison.OrdinalIgnoreCase);
    set => this._fileSystem = value ? "URL" : (string) null;
  }

  /// <summary>
  /// Gets or sets a flag indicating whether the file referenced by the this file specification is volatile (changes frequently with time).
  /// </summary>
  /// <remarks>
  /// If the value is true, applications should
  /// never cache a copy of the file.For example, a movie annotation referencing a URL to
  /// a live video camera could set this flag to true to notify the application that it should
  /// reacquire the movie each time it is played.Default value: false.
  /// </remarks>
  public bool IsVolatile
  {
    get => this.IsExists("V") && this.Dictionary["V"].As<PdfTypeBoolean>().Value;
    set
    {
      if (!value && this.Dictionary.ContainsKey("V"))
      {
        this.Dictionary.Remove("V");
      }
      else
      {
        if (!value)
          return;
        this.Dictionary["V"] = (PdfTypeBase) PdfTypeBoolean.Create(value);
      }
    }
  }

  /// <summary>
  /// Gets or sets descriptive text associated with the file specification.
  /// </summary>
  public string Description
  {
    get
    {
      return !this.IsExists("Desc") ? (string) null : this.Dictionary["Desc"].As<PdfTypeString>().UnicodeString;
    }
    set
    {
      if (value == null && this.Dictionary.ContainsKey("Desc"))
      {
        this.Dictionary.Remove("Desc");
      }
      else
      {
        if (value == null)
          return;
        this.Dictionary["Desc"] = (PdfTypeBase) PdfTypeString.Create(value);
      }
    }
  }

  /// <summary>Gets or sets embedded file.</summary>
  public PdfFile EmbeddedFile
  {
    get
    {
      if (!this.IsExists("EF"))
        return (PdfFile) null;
      if ((PdfWrapper) this._embeddedFile == (PdfWrapper) null || this._embeddedFile.Stream.IsDisposed)
      {
        PdfTypeStream embeddedFileStream = this._embeddedFileStream;
        if (embeddedFileStream == null)
          return (PdfFile) null;
        this._embeddedFile = new PdfFile(embeddedFileStream);
      }
      return this._embeddedFile;
    }
    set
    {
      if ((PdfWrapper) value == (PdfWrapper) null && this.Dictionary.ContainsKey("EF"))
        this.Dictionary.Remove("EF");
      else if ((PdfWrapper) value != (PdfWrapper) null)
      {
        if (!this.Dictionary.ContainsKey("EF"))
          this.Dictionary["EF"] = (PdfTypeBase) PdfTypeDictionary.Create();
        this._embeddedFileStream = value.Stream;
      }
      this._embeddedFile = value;
    }
  }

  /// <summary>
  /// Creates new instance of <see cref="T:Patagames.Pdf.Net.Wrappers.PdfFileSpecification" />.
  /// </summary>
  /// <param name="document">Instance of PdfDocument class with which this dictionary is associated</param>
  public PdfFileSpecification(PdfDocument document)
  {
    if (document == null)
      throw new ArgumentNullException(nameof (document));
    this.Dictionary["Type"] = (PdfTypeBase) PdfTypeName.Create("Filespec");
    this._listOfIndirectObjects = PdfIndirectList.FromPdfDocument(document);
  }

  /// <summary>
  /// Creates a new instance of <see cref="T:Patagames.Pdf.Net.Wrappers.PdfFileSpecification" /> and initialize it with specified dictionary
  /// </summary>
  /// <param name="document">Instance of PdfDocument class with which this dictionary is associated</param>
  /// <param name="dictionary">The dictionary or indirect dictionary</param>
  public PdfFileSpecification(PdfDocument document, PdfTypeBase dictionary)
    : base(dictionary)
  {
    this._listOfIndirectObjects = PdfIndirectList.FromPdfDocument(document);
  }
}
