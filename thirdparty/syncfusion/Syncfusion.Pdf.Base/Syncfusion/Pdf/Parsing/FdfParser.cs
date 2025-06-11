// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.FdfParser
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class FdfParser
{
  private PdfReader m_reader;
  private PdfParser m_parser;
  private Stream m_stream;
  private FdfObjectCollection m_objects;
  private Dictionary<string, PdfReferenceHolder> m_annotationObjects;
  private Dictionary<string, string> m_groupObjects;

  internal PdfReader Reader => this.m_reader;

  internal PdfParser Parser => this.m_parser;

  internal FdfObjectCollection FdfObjects => this.m_objects;

  internal Dictionary<string, string> GroupedObjects
  {
    get
    {
      if (this.m_groupObjects == null)
        this.m_groupObjects = new Dictionary<string, string>();
      return this.m_groupObjects;
    }
  }

  internal FdfParser(Stream stream)
  {
    this.m_stream = stream;
    this.m_reader = new PdfReader(stream);
    this.m_objects = new FdfObjectCollection();
  }

  internal void ImportAnnotations(PdfLoadedDocument document)
  {
    this.m_annotationObjects = this.GetAnnotationObjects();
    if (!this.GroupAnnotations())
      return;
    foreach (PdfReferenceHolder pdfReferenceHolder in this.m_annotationObjects.Values)
    {
      PdfDictionary dictionary1 = pdfReferenceHolder.Object as PdfDictionary;
      this.ParseDictionary(dictionary1);
      if (dictionary1 != null && dictionary1.Items.Count > 0)
      {
        if (dictionary1.ContainsKey("IRT") && dictionary1["IRT"] is PdfString pdfString && this.GroupedObjects.ContainsKey(pdfString.Value))
        {
          string groupedObject = this.GroupedObjects[pdfString.Value];
          dictionary1["IRT"] = (IPdfPrimitive) this.m_annotationObjects[groupedObject];
        }
        if (dictionary1.ContainsKey("Page") && dictionary1["Page"] is PdfNumber pdfNumber)
        {
          int intValue = pdfNumber.IntValue;
          if (intValue < document.PageCount)
          {
            (document.Pages[intValue] as PdfLoadedPage).importAnnotation = true;
            PdfDictionary dictionary2 = document.Pages[intValue].Dictionary;
            if (dictionary2 != null)
            {
              if (!dictionary2.ContainsKey("Annots"))
                dictionary2["Annots"] = (IPdfPrimitive) new PdfArray();
              IPdfPrimitive pdfPrimitive = dictionary2["Annots"];
              if (((object) (pdfPrimitive as PdfReferenceHolder) != null ? (pdfPrimitive as PdfReferenceHolder).Object : pdfPrimitive) is PdfArray pdfArray)
              {
                pdfArray.Elements.Add((IPdfPrimitive) pdfReferenceHolder);
                pdfArray.MarkChanged();
                dictionary2.Modify();
              }
            }
          }
          dictionary1.Remove("Page");
        }
      }
    }
  }

  private bool GroupAnnotations()
  {
    if (this.m_annotationObjects.Count <= 0)
      return false;
    foreach (KeyValuePair<string, PdfReferenceHolder> annotationObject in this.m_annotationObjects)
    {
      if (annotationObject.Value != (PdfReferenceHolder) null && annotationObject.Value.Object is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey("NM") && pdfDictionary["NM"] is PdfString pdfString && !string.IsNullOrEmpty(pdfString.Value))
      {
        if (this.GroupedObjects.ContainsKey(pdfString.Value))
          this.GroupedObjects[pdfString.Value] = annotationObject.Key;
        else
          this.GroupedObjects.Add(pdfString.Value, annotationObject.Key);
      }
    }
    return true;
  }

  internal void ParseObjectData()
  {
    PdfCrossTable pdfCrossTable = new PdfCrossTable(true, this.m_stream);
    if (pdfCrossTable.CrossTable == null)
      return;
    this.m_parser = pdfCrossTable.CrossTable.Parser;
    long offset = this.CheckFdf();
    if (offset == -1L)
      return;
    long num = this.m_reader.Seek(0L, SeekOrigin.End);
    this.m_reader.SearchBack("trailer");
    this.m_parser.SetOffset(offset);
    this.m_parser.Advance();
    while (this.m_parser.GetNext() == Syncfusion.Pdf.IO.TokenType.Number)
      this.ParseObject(this.m_objects);
    if (this.m_parser.Lexer != null && this.m_parser.Lexer.Text == "trailer")
    {
      IPdfPrimitive pdfPrimitive = this.m_parser.Trailer();
      if (pdfPrimitive != null)
        this.m_objects.Add("trailer", pdfPrimitive);
    }
    this.m_parser.Advance();
    while (this.m_parser.Lexer != null && this.m_parser.Lexer.Position != num && this.m_parser.GetNext() == Syncfusion.Pdf.IO.TokenType.Number)
      this.ParseObject(this.m_objects);
  }

  internal void ParseAnnotationData()
  {
    PdfCrossTable pdfCrossTable = new PdfCrossTable(true, this.m_stream);
    if (pdfCrossTable.CrossTable == null)
      return;
    this.m_parser = pdfCrossTable.CrossTable.Parser;
    long offset = this.CheckFdf();
    if (offset == -1L)
      return;
    long num = this.m_reader.Seek(0L, SeekOrigin.End);
    this.m_reader.SearchBack("trailer");
    this.m_parser.SetOffset(offset);
    this.m_parser.Advance();
    while (this.m_parser.GetNext() == Syncfusion.Pdf.IO.TokenType.Number)
      this.ParseObject(this.m_objects);
    if (this.m_parser.Lexer != null && this.m_parser.Lexer.Text == "trailer")
    {
      IPdfPrimitive pdfPrimitive = this.m_parser.Trailer();
      if (pdfPrimitive != null)
        this.m_objects.Add("trailer", pdfPrimitive);
    }
    this.m_parser.Advance();
    while (this.m_parser.Lexer != null && this.m_parser.Lexer.Position != num && this.m_parser.GetNext() == Syncfusion.Pdf.IO.TokenType.Number)
      this.ParseObject(this.m_objects);
  }

  internal void Dispose()
  {
    this.m_objects.Dispose();
    this.m_reader.Close();
    this.m_reader.Dispose();
    this.m_parser = (PdfParser) null;
    if (this.m_annotationObjects != null)
      this.m_annotationObjects.Clear();
    this.m_annotationObjects = (Dictionary<string, PdfReferenceHolder>) null;
  }

  private long CheckFdf()
  {
    long num1 = 0;
    long num2 = -1;
    long num3 = 0;
    int num4 = 8;
    while (num2 < 0L && num1 < this.m_stream.Length - 1L)
    {
      byte[] numArray = new byte[1024 /*0x0400*/];
      if (num1 - 5L > 0L)
        num1 -= 5L;
      this.m_stream.Position = num1;
      this.m_stream.Read(numArray, 0, numArray.Length);
      num2 = (long) Encoding.Default.GetString(numArray).IndexOf("%FDF-");
      num1 = this.m_stream.Position;
      if (num2 < 0L)
        num3 = num1 - 5L;
      else
        num2 += num3;
    }
    if (num2 < 0L)
      throw new Exception("Cannot import Fdf file. File format has been corrupted");
    this.m_stream.Position = 0L;
    return num2 + (long) num4;
  }

  private void ParseObject(FdfObjectCollection objects)
  {
    FdfObject fdfObject = this.m_parser.ParseObject();
    if (fdfObject.ObjectNumber > 0 && fdfObject.GenerationNumber >= 0)
    {
      string key = $"{(object) fdfObject.ObjectNumber} {(object) fdfObject.GenerationNumber}";
      objects.Add(key, fdfObject.Object);
    }
    this.m_parser.Advance();
  }

  private void ParseDictionary(PdfDictionary dictionary, PdfName key)
  {
    IPdfPrimitive pdfPrimitive = dictionary[key];
    switch (pdfPrimitive)
    {
      case PdfDictionary _:
      case PdfStream _:
        this.ParseDictionary(pdfPrimitive as PdfDictionary);
        break;
      case PdfArray _:
        this.ParseArray(pdfPrimitive as PdfArray);
        break;
      default:
        if ((object) (pdfPrimitive as PdfReferenceHolder) == null)
          break;
        PdfReferenceHolder pdfReferenceHolder1 = pdfPrimitive as PdfReferenceHolder;
        if (!(pdfReferenceHolder1 != (PdfReferenceHolder) null))
          break;
        PdfReference reference = pdfReferenceHolder1.Reference;
        if (!(reference != (PdfReference) null))
          break;
        string key1 = $"{(object) reference.ObjNum} {(object) reference.GenNum}";
        if (this.m_annotationObjects.ContainsKey(key1))
        {
          dictionary[key] = (IPdfPrimitive) this.m_annotationObjects[key1];
          dictionary.Modify();
          break;
        }
        if (this.m_objects.Objects.ContainsKey(key1))
        {
          Dictionary<string, IPdfPrimitive> objects = this.m_objects.Objects;
          if ((object) (objects[key1] as PdfReferenceHolder) != null)
          {
            dictionary[key] = objects[key1];
            dictionary.Modify();
            break;
          }
          if ((object) (objects[key1] as PdfName) != null)
          {
            PdfReferenceHolder pdfReferenceHolder2 = new PdfReferenceHolder((IPdfPrimitive) (objects[key1] as PdfName));
            dictionary[key] = (IPdfPrimitive) pdfReferenceHolder2;
            objects[key1] = (IPdfPrimitive) pdfReferenceHolder2;
            dictionary.Modify();
            break;
          }
          if (objects[key1] is PdfArray)
          {
            PdfArray array = objects[key1] as PdfArray;
            this.ParseArray(array);
            PdfReferenceHolder pdfReferenceHolder3 = new PdfReferenceHolder((IPdfPrimitive) array);
            dictionary[key] = (IPdfPrimitive) pdfReferenceHolder3;
            objects[key1] = (IPdfPrimitive) pdfReferenceHolder3;
            dictionary.Modify();
            break;
          }
          if (objects[key1] is PdfStream)
          {
            PdfStream dictionary1 = objects[key1] as PdfStream;
            this.ParseDictionary((PdfDictionary) dictionary1);
            PdfReferenceHolder pdfReferenceHolder4 = new PdfReferenceHolder((IPdfPrimitive) dictionary1);
            dictionary[key] = (IPdfPrimitive) pdfReferenceHolder4;
            objects[key1] = (IPdfPrimitive) pdfReferenceHolder4;
            dictionary.Modify();
            break;
          }
          if (!(objects[key1] is PdfDictionary))
            break;
          PdfDictionary dictionary2 = objects[key1] as PdfDictionary;
          this.ParseDictionary(dictionary2);
          PdfReferenceHolder pdfReferenceHolder5 = new PdfReferenceHolder((IPdfPrimitive) dictionary2);
          dictionary[key] = (IPdfPrimitive) pdfReferenceHolder5;
          objects[key1] = (IPdfPrimitive) pdfReferenceHolder5;
          dictionary.Modify();
          break;
        }
        dictionary.Remove(key);
        break;
    }
  }

  private void ParseDictionary(PdfDictionary dictionary)
  {
    if (dictionary == null || dictionary.Items.Count <= 0)
      return;
    foreach (PdfName key in this.GetKeys(dictionary))
      this.ParseDictionary(dictionary, key);
  }

  private void ParseArray(PdfArray array)
  {
    if (array == null)
      return;
    int count = array.Elements.Count;
    for (int index = 0; index < count; ++index)
    {
      PdfReferenceHolder pdfReferenceHolder1 = array[index] as PdfReferenceHolder;
      if (pdfReferenceHolder1 != (PdfReferenceHolder) null)
      {
        PdfReference reference = pdfReferenceHolder1.Reference;
        if (reference != (PdfReference) null)
        {
          string key = $"{(object) reference.ObjNum} {(object) reference.GenNum}";
          if (this.m_annotationObjects.ContainsKey(key))
          {
            array.Elements[index] = (IPdfPrimitive) this.m_annotationObjects[key];
            array.MarkChanged();
          }
          else if (this.m_objects.Objects.ContainsKey(key))
          {
            Dictionary<string, IPdfPrimitive> objects = this.m_objects.Objects;
            if ((object) (objects[key] as PdfReferenceHolder) != null)
            {
              array.Elements[index] = objects[key];
              array.MarkChanged();
            }
            else if (objects[key] is PdfDictionary || objects[key] is PdfStream)
            {
              PdfReferenceHolder pdfReferenceHolder2 = new PdfReferenceHolder(objects[key]);
              array.Elements[index] = (IPdfPrimitive) pdfReferenceHolder2;
              objects[key] = (IPdfPrimitive) pdfReferenceHolder2;
              array.MarkChanged();
            }
          }
        }
      }
    }
  }

  private Dictionary<string, PdfReferenceHolder> GetAnnotationObjects()
  {
    Dictionary<string, PdfReferenceHolder> annotationObjects = new Dictionary<string, PdfReferenceHolder>();
    Dictionary<string, IPdfPrimitive> objects = this.m_objects.Objects;
    List<string> stringList = new List<string>();
    if (objects.Count > 0 && objects.ContainsKey("trailer"))
    {
      if (objects["trailer"] is PdfDictionary pdfDictionary2 && pdfDictionary2.ContainsKey("Root"))
      {
        PdfReferenceHolder pdfReferenceHolder1 = pdfDictionary2["Root"] as PdfReferenceHolder;
        if (pdfReferenceHolder1 != (PdfReferenceHolder) null)
        {
          PdfReference reference1 = pdfReferenceHolder1.Reference;
          if (reference1 != (PdfReference) null)
          {
            string key1 = $"{(object) reference1.ObjNum} {(object) reference1.GenNum}";
            if (objects.ContainsKey(key1) && objects[key1] is PdfDictionary pdfDictionary1 && pdfDictionary1.ContainsKey("FDF"))
            {
              if (pdfDictionary1["FDF"] is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey("Annots") && pdfDictionary["Annots"] is PdfArray pdfArray && pdfArray.Count != 0)
              {
                foreach (IPdfPrimitive element in pdfArray.Elements)
                {
                  PdfReferenceHolder pdfReferenceHolder2 = element as PdfReferenceHolder;
                  if (pdfReferenceHolder2 != (PdfReferenceHolder) null)
                  {
                    PdfReference reference2 = pdfReferenceHolder2.Reference;
                    if (reference2 != (PdfReference) null)
                    {
                      string key2 = $"{(object) reference2.ObjNum} {(object) reference2.GenNum}";
                      if (objects.ContainsKey(key2))
                      {
                        annotationObjects.Add(key2, new PdfReferenceHolder(objects[key2]));
                        objects.Remove(key2);
                      }
                    }
                  }
                }
              }
              objects.Remove(key1);
            }
          }
        }
      }
      objects.Remove("trailer");
    }
    return annotationObjects;
  }

  private PdfName[] GetKeys(PdfDictionary dictionary)
  {
    List<PdfName> pdfNameList = new List<PdfName>(dictionary.Keys.Count);
    foreach (PdfName key in dictionary.Keys)
      pdfNameList.Add(key);
    return pdfNameList.ToArray();
  }
}
