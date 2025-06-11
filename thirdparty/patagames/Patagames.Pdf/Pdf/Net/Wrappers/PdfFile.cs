// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Wrappers.PdfFile
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;
using System;

#nullable disable
namespace Patagames.Pdf.Net.Wrappers;

/// <summary>Represents a embedded file object</summary>
public class PdfFile : PdfWrapper
{
  /// <summary>Gets underlying stream.</summary>
  public PdfTypeStream Stream { get; private set; }

  /// <summary>
  /// Gets or sets the date and time when the embedded file was created.
  /// </summary>
  /// <remarks>
  /// <para>
  /// PDF defines a standard date format, which closely follows that of the international standard ASN.1 (Abstract Syntax Notation One), defined in ISO/IEC 8824
  /// A date is a string of the form</para>
  /// <para>D:YYYYMMDDHHmmSSOHH'mm'</para>
  /// <para>where YYYY is the year; MM is the month; DD is the day(01–31); HH is the hour(00–23); mm is the minute(00–59); SS is the second(00–59);
  /// O is the relationship of local time to Universal Time(UT), denoted by one of the characters +, −, or Z;
  /// HH followed by ' is the absolute value of the offset from UT in hours (00–23),
  /// mm followed by ' is the absolute value of the offset from UT in minutes (00–59)
  /// </para>
  /// <para>The apostrophe character (') after HH and mm is part of the syntax. All fields after
  /// the year are optional. (The prefix D:, although also optional, is strongly recommended.)
  /// The default values for MM and DD are both 01; all other numerical fields default to zero values. A plus sign (+) as the value of the O field
  /// signifies that local time is later than UT, a minus sign(−) signifies that local time
  /// is earlier than UT, and the letter Z signifies that local time is equal to UT.If no UT
  /// information is specified, the relationship of the specified time to UT is considered
  /// to be unknown.Regardless of whether the time zone is known, the rest of the date
  /// should be specified in local time.</para>
  /// <para>For example, December 20, 2018, at 3:50 PM, U.S. Pacific Standard Time, is represented by the string</para>
  /// <para>D:201812201550−08'00'</para>
  /// </remarks>
  public string CreationDate
  {
    get
    {
      if (!this.IsExists("Params"))
        return (string) null;
      PdfTypeDictionary pdfTypeDictionary = this.Dictionary["Params"].As<PdfTypeDictionary>();
      return pdfTypeDictionary.ContainsKey(nameof (CreationDate)) ? pdfTypeDictionary[nameof (CreationDate)].As<PdfTypeString>().AnsiString : (string) null;
    }
    set
    {
      if (value == null && !this.IsExists("Params"))
        return;
      if (!this.IsExists("Params"))
        this.Dictionary["Params"] = (PdfTypeBase) PdfTypeDictionary.Create();
      PdfTypeDictionary pdfTypeDictionary = this.Dictionary["Params"].As<PdfTypeDictionary>();
      if (value == null && pdfTypeDictionary.ContainsKey(nameof (CreationDate)))
      {
        pdfTypeDictionary.Remove(nameof (CreationDate));
      }
      else
      {
        if (value == null)
          return;
        pdfTypeDictionary[nameof (CreationDate)] = (PdfTypeBase) PdfTypeString.Create(value);
      }
    }
  }

  /// <summary>
  /// Gets or sets the date and time when the embedded file was last modified.
  /// </summary>
  /// <remarks>
  /// Please see remarks section of <see cref="P:Patagames.Pdf.Net.Wrappers.PdfFile.CreationDate" />.
  /// </remarks>
  public string ModificationDate
  {
    get
    {
      if (!this.IsExists("Params"))
        return (string) null;
      PdfTypeDictionary pdfTypeDictionary = this.Dictionary["Params"].As<PdfTypeDictionary>();
      return pdfTypeDictionary.ContainsKey("ModDate") ? pdfTypeDictionary["ModDate"].As<PdfTypeString>().AnsiString : (string) null;
    }
    set
    {
      if (value == null && !this.IsExists("Params"))
        return;
      if (!this.IsExists("Params"))
        this.Dictionary["Params"] = (PdfTypeBase) PdfTypeDictionary.Create();
      PdfTypeDictionary pdfTypeDictionary = this.Dictionary["Params"].As<PdfTypeDictionary>();
      if (value == null && pdfTypeDictionary.ContainsKey("ModDate"))
      {
        pdfTypeDictionary.Remove("ModDate");
      }
      else
      {
        if (value == null)
          return;
        pdfTypeDictionary["ModDate"] = (PdfTypeBase) PdfTypeString.Create(value);
      }
    }
  }

  /// <summary>
  /// Gets or sets a 16-byte string that is the checksum of the bytes of the uncompressed embedded file.
  /// </summary>
  /// <remarks>
  /// The checksum is calculated by applying the standard MD5 message-digest algorithm (described in Internet RFC 1321, The MD5
  /// Message-Digest Algorithm) to the bytes of the embedded file stream.
  /// </remarks>
  public string CheckSum
  {
    get
    {
      if (!this.IsExists("Params"))
        return (string) null;
      PdfTypeDictionary pdfTypeDictionary = this.Dictionary["Params"].As<PdfTypeDictionary>();
      return pdfTypeDictionary.ContainsKey(nameof (CheckSum)) ? pdfTypeDictionary[nameof (CheckSum)].As<PdfTypeString>().AnsiString : (string) null;
    }
    set
    {
      if (value == null && !this.IsExists("Params"))
        return;
      if (!this.IsExists("Params"))
        this.Dictionary["Params"] = (PdfTypeBase) PdfTypeDictionary.Create();
      PdfTypeDictionary pdfTypeDictionary = this.Dictionary["Params"].As<PdfTypeDictionary>();
      if (value == null && pdfTypeDictionary.ContainsKey(nameof (CheckSum)))
      {
        pdfTypeDictionary.Remove(nameof (CheckSum));
      }
      else
      {
        if (value == null)
          return;
        pdfTypeDictionary[nameof (CheckSum)] = (PdfTypeBase) PdfTypeString.Create(value);
      }
    }
  }

  /// <summary>Gets or sets size of the embedded file, in bytes.</summary>
  public int FileSize
  {
    get
    {
      if (!this.IsExists("Params"))
        return 0;
      PdfTypeDictionary pdfTypeDictionary = this.Dictionary["Params"].As<PdfTypeDictionary>();
      return pdfTypeDictionary.ContainsKey("Size") ? pdfTypeDictionary["Size"].As<PdfTypeNumber>().IntValue : 0;
    }
    set
    {
      if (value == 0 && !this.IsExists("Params"))
        return;
      if (!this.IsExists("Params"))
        this.Dictionary["Params"] = (PdfTypeBase) PdfTypeDictionary.Create();
      PdfTypeDictionary pdfTypeDictionary = this.Dictionary["Params"].As<PdfTypeDictionary>();
      if (value == 0 && pdfTypeDictionary.ContainsKey("Size"))
      {
        pdfTypeDictionary.Remove("Size");
      }
      else
      {
        if (value == 0)
          return;
        pdfTypeDictionary["Size"] = (PdfTypeBase) PdfTypeNumber.Create(value);
      }
    }
  }

  /// <summary>
  /// Creates new instance of <see cref="T:Patagames.Pdf.Net.Wrappers.PdfFile" />.
  /// </summary>
  /// <param name="fileContent">The contents of the file that should be embedded</param>
  public PdfFile(byte[] fileContent)
  {
    this.Dictionary["Type"] = (PdfTypeBase) PdfTypeName.Create("EmbeddedFile");
    this.Stream = PdfTypeStream.Create();
    this.Stream.Init(fileContent, this.Dictionary);
    this.FileSize = fileContent.Length;
  }

  /// <summary>
  /// Creates a new instance of <see cref="T:Patagames.Pdf.Net.Wrappers.PdfFile" /> and initialize it with specified steeam
  /// </summary>
  /// <param name="stream">The dictionary or indirect dictionary</param>
  public PdfFile(PdfTypeStream stream)
    : base((PdfTypeBase) stream.As<PdfTypeStream>().Dictionary)
  {
    this.Stream = stream != null ? stream.As<PdfTypeStream>() : throw new ArgumentNullException(nameof (stream));
  }

  /// <summary>
  /// Releases all resources used by the <see cref="T:Patagames.Pdf.Net.Wrappers.PdfFile" />.
  /// </summary>
  /// <param name="disposing">true for SuppressFinalize</param>
  protected override void Dispose(bool disposing)
  {
    this.Stream.Dispose();
    if (!disposing)
      return;
    GC.SuppressFinalize((object) this);
  }
}
