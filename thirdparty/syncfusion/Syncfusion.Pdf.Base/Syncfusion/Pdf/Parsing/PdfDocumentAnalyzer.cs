// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.PdfDocumentAnalyzer
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

public class PdfDocumentAnalyzer
{
  private Stream m_stream;
  private string m_password;
  private List<PdfException> result;
  private List<long> parsedObjNum = new List<long>();

  public PdfDocumentAnalyzer(string fileName)
  {
    if (fileName == null)
      throw new ArgumentNullException(nameof (fileName));
    this.m_stream = File.Exists(fileName) ? (Stream) new FileStream(fileName, FileMode.Open, FileAccess.Read) : throw new ArgumentException("File doesn't exist", nameof (fileName));
  }

  public PdfDocumentAnalyzer(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException("file");
    this.m_stream = stream.Length != 0L ? stream : throw new PdfException("Contents of file stream is empty");
  }

  public PdfDocumentAnalyzer(string fileName, string password)
  {
    if (fileName == null)
      throw new ArgumentNullException(nameof (fileName));
    this.m_stream = File.Exists(fileName) ? (Stream) new FileStream(fileName, FileMode.Open, FileAccess.Read) : throw new ArgumentException("File doesn't exist", nameof (fileName));
    this.m_password = password;
  }

  public PdfDocumentAnalyzer(Stream stream, string password)
  {
    if (stream == null)
      throw new ArgumentNullException("file");
    this.m_stream = stream.Length != 0L ? stream : throw new PdfException("Contents of file stream is empty");
    this.m_password = password;
  }

  public SyntaxAnalyzerResult AnalyzeSyntax()
  {
    SyntaxAnalyzerResult syntaxAnalyzerResult = new SyntaxAnalyzerResult();
    PdfLoadedDocument pdfLoadedDocument = (PdfLoadedDocument) null;
    try
    {
      pdfLoadedDocument = new PdfLoadedDocument(this.m_stream, this.m_password, out this.result);
    }
    catch (Exception ex)
    {
      this.result.Add(new PdfException(ex.Message));
    }
    if (pdfLoadedDocument != null)
    {
      PdfDictionary catalog = (PdfDictionary) pdfLoadedDocument.Catalog;
      if (catalog != null)
      {
        try
        {
          if (!catalog.ContainsKey("Pages"))
            throw new PdfException("The document has no page tree node");
          PdfName pdfName = PdfCrossTable.Dereference(catalog["Pages"]) is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey("Type") ? PdfCrossTable.Dereference(pdfDictionary["Type"]) as PdfName : throw new PdfException("The document has no page tree node");
          if (pdfName != (PdfName) null && pdfName.Value != "Pages")
            throw new PdfException("Type of PDF page tree node is invalid");
          this.ParseDictionary(catalog);
        }
        catch (Exception ex)
        {
          this.result.Add(new PdfException(ex.Message));
        }
      }
      else
        this.result.Add(new PdfException("The document has no calatog object"));
    }
    if (this.result.Count == 0)
      return syntaxAnalyzerResult;
    syntaxAnalyzerResult.IsCorrupted = true;
    syntaxAnalyzerResult.Errors = this.result;
    return syntaxAnalyzerResult;
  }

  private void ParseDictionary(PdfDictionary dictionary)
  {
    foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in dictionary.Items)
    {
      if (!(keyValuePair.Key.Value == "Parent") && !(keyValuePair.Key.Value == "First") && !(keyValuePair.Key.Value == "Last") && !(keyValuePair.Key.Value == "Next") && !(keyValuePair.Key.Value == "Prev") && !(keyValuePair.Key.Value == "P") && !(keyValuePair.Key.Value == "Dest") && !(keyValuePair.Key.Value == "Pg") && !(keyValuePair.Key.Value == "Data") && !(keyValuePair.Key.Value == "Reference") && !(keyValuePair.Key.Value == "K") && !(keyValuePair.Key.Value == "D") && !(keyValuePair.Key.Value == "T") && !(keyValuePair.Key.Value == "N") && !(keyValuePair.Key.Value == "V"))
      {
        PdfDictionary dictionary1 = keyValuePair.Value as PdfDictionary;
        PdfReferenceHolder holder = keyValuePair.Value as PdfReferenceHolder;
        PdfArray array = keyValuePair.Value as PdfArray;
        if (dictionary1 != null)
          this.ParseDictionary(dictionary1);
        else if (holder != (PdfReferenceHolder) null)
          this.ParseReferenceHolder(holder);
        else if (array != null)
          this.ParseArray(array);
      }
    }
  }

  private void ParseReferenceHolder(PdfReferenceHolder holder)
  {
    try
    {
      if (this.parsedObjNum.Count > 0 && this.parsedObjNum.Contains(holder.Reference.ObjNum))
        return;
      this.parsedObjNum.Add(holder.Reference.ObjNum);
      IPdfPrimitive pdfPrimitive = holder.Object;
      if (pdfPrimitive is PdfDictionary)
        this.ParseDictionary(pdfPrimitive as PdfDictionary);
      else if ((object) (pdfPrimitive as PdfReferenceHolder) != null)
      {
        this.ParseReferenceHolder(pdfPrimitive as PdfReferenceHolder);
      }
      else
      {
        if (!(pdfPrimitive is PdfArray))
          return;
        this.ParseArray(pdfPrimitive as PdfArray);
      }
    }
    catch (Exception ex)
    {
      this.result.Add(new PdfException(ex.Message.ToString()));
    }
  }

  private void ParseArray(PdfArray array)
  {
    foreach (IPdfPrimitive pdfPrimitive in array)
    {
      PdfDictionary dictionary = pdfPrimitive as PdfDictionary;
      PdfReferenceHolder holder = pdfPrimitive as PdfReferenceHolder;
      PdfArray array1 = pdfPrimitive as PdfArray;
      if (dictionary != null)
        this.ParseDictionary(dictionary);
      else if (holder != (PdfReferenceHolder) null)
        this.ParseReferenceHolder(holder);
      else if (array1 != null)
        this.ParseArray(array1);
    }
  }

  public void Close()
  {
    if (this.m_stream != null)
      this.m_stream.Dispose();
    this.parsedObjNum.Clear();
  }
}
