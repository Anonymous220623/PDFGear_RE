// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.XFdfDocument
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

public class XFdfDocument
{
  private Dictionary<object, object> table = new Dictionary<object, object>();
  private string PdfFilePath = "";
  private bool isAnnotationExport;
  private PdfLoadedDocument m_document;
  private PdfExportAnnotationCollection m_annotationCollection;
  private Dictionary<string, object> annotationObjects = new Dictionary<string, object>();
  private bool m_skipBorderStyle;
  private List<string> m_annotationAttributes;
  private string isContainsRV = string.Empty;
  internal IPdfPrimitive trailerId;

  internal bool IsExportAnnotations
  {
    get => this.isAnnotationExport;
    set => this.isAnnotationExport = value;
  }

  internal PdfExportAnnotationCollection AnnotationCollection
  {
    get => this.m_annotationCollection;
    set => this.m_annotationCollection = value;
  }

  public XFdfDocument(string filename) => this.PdfFilePath = filename;

  internal void SetFields(object fieldName, object Fieldvalue)
  {
    this.table[fieldName] = Fieldvalue;
  }

  internal void SetFields(object fieldName, object Fieldvalue, string uniqueKey)
  {
    this.table[fieldName] = Fieldvalue;
    this.isContainsRV = uniqueKey;
  }

  internal void Save(Stream stream)
  {
    XmlTextWriter textWriter = new XmlTextWriter(stream, (Encoding) new UTF8Encoding());
    textWriter.Formatting = Formatting.Indented;
    textWriter.WriteStartDocument();
    textWriter.WriteStartElement("xfdf".ToLower());
    textWriter.WriteAttributeString("xmlns", (string) null, (string) null, "http://ns.adobe.com/xfdf/");
    textWriter.WriteAttributeString("xml", "space", (string) null, "preserve");
    if (this.isAnnotationExport)
    {
      textWriter.WriteStartElement("Annots".ToLower());
      if (this.m_document != null)
      {
        if (this.m_annotationCollection != null && this.m_annotationCollection.Count > 0)
        {
          foreach (PdfAnnotation annotation1 in (PdfCollection) this.m_annotationCollection)
          {
            if (annotation1 is PdfLoadedAnnotation annotation2)
            {
              int index = this.m_document.Pages.IndexOf((PdfPageBase) annotation1.LoadedPage);
              this.ExportAnnotationData(annotation2, index, textWriter);
            }
          }
        }
        else
        {
          for (int index = 0; index < this.m_document.PageCount; ++index)
          {
            foreach (PdfAnnotation annotation3 in (PdfCollection) (this.m_document.Pages[index] as PdfLoadedPage).Annotations)
            {
              if (annotation3 is PdfLoadedAnnotation annotation4)
                this.ExportAnnotationData(annotation4, index, textWriter);
            }
          }
        }
      }
    }
    else
    {
      textWriter.WriteStartElement("Fields".ToLower());
      this.WriteFormData(textWriter);
    }
    textWriter.WriteEndElement();
    textWriter.WriteStartElement("f");
    textWriter.WriteAttributeString("href", this.PdfFilePath);
    textWriter.WriteEndElement();
    textWriter.WriteEndElement();
    textWriter.WriteEndDocument();
    textWriter.Flush();
  }

  internal void Save(Stream stream, bool isacrobat)
  {
    if (!isacrobat)
      return;
    XmlTextWriter textWriter = new XmlTextWriter(stream, (Encoding) new UTF8Encoding());
    textWriter.Formatting = Formatting.Indented;
    textWriter.WriteStartDocument();
    textWriter.WriteStartElement("xfdf".ToLower());
    textWriter.WriteAttributeString("xmlns", (string) null, (string) null, "http://ns.adobe.com/xfdf/");
    textWriter.WriteAttributeString("xml", "space", (string) null, "preserve");
    textWriter.WriteStartElement("f");
    textWriter.WriteAttributeString("href", this.PdfFilePath);
    textWriter.WriteEndElement();
    Dictionary<object, object> dictionary = new Dictionary<object, object>();
    bool flag = false;
    Dictionary<object, object> elements = this.GetElements(this.table);
    if (elements.Count > 0)
      textWriter.WriteStartElement("Fields".ToLower());
    foreach (KeyValuePair<object, object> keyValuePair in elements)
    {
      string key = (string) keyValuePair.Key;
      textWriter.WriteStartElement("field");
      textWriter.WriteAttributeString("Name".ToLower(), key.ToString());
      object obj = elements[(object) key];
      if (obj.GetType().Name == "PdfArray")
      {
        foreach (PdfString pdfString in obj as PdfArray)
        {
          textWriter.WriteStartElement("value");
          textWriter.WriteString(pdfString.Value.ToString());
          textWriter.WriteEndElement();
          flag = true;
        }
      }
      if (obj is Dictionary<object, object>)
        this.WriteFieldName((Dictionary<object, object>) obj, textWriter);
      else if (!flag && !keyValuePair.Value.ToString().EndsWith(this.isContainsRV) || !flag && this.isContainsRV == "")
      {
        textWriter.WriteStartElement("value");
        textWriter.WriteString(obj.ToString());
        textWriter.WriteEndElement();
      }
      else if (keyValuePair.Value.ToString().EndsWith(this.isContainsRV) && this.isContainsRV != "")
      {
        textWriter.WriteStartElement("value-richtext");
        string str = keyValuePair.Value.ToString();
        if (str.StartsWith("<?xml version=\"1.0\"?>"))
          str = str.Remove(0, 21);
        string data = str.Remove(str.Length - this.isContainsRV.Length, this.isContainsRV.Length);
        textWriter.WriteRaw(data);
        textWriter.WriteEndElement();
      }
      textWriter.WriteEndElement();
      flag = false;
    }
    if (elements.Count > 0)
      textWriter.WriteEndElement();
    PdfArray trailerId = this.trailerId as PdfArray;
    byte[] bytes1 = Encoding.GetEncoding("windows-1252").GetBytes("");
    byte[] bytes2 = Encoding.GetEncoding("windows-1252").GetBytes("");
    if (trailerId != null && trailerId.Count >= 1)
    {
      bytes1 = Encoding.GetEncoding("windows-1252").GetBytes(new PdfString((trailerId[0] as PdfString).Value)
      {
        Encode = PdfString.ForceEncoding.ASCII
      }.Value);
      bytes2 = Encoding.GetEncoding("windows-1252").GetBytes(new PdfString((trailerId[1] as PdfString).Value)
      {
        Encode = PdfString.ForceEncoding.ASCII
      }.Value);
    }
    textWriter.WriteStartElement("Ids".ToLower());
    textWriter.WriteAttributeString("original", PdfString.BytesToHex(bytes1));
    textWriter.WriteAttributeString("modified", PdfString.BytesToHex(bytes2));
    textWriter.WriteEndElement();
    textWriter.WriteEndElement();
    textWriter.WriteEndDocument();
    textWriter.Flush();
  }

  internal Dictionary<object, object> GetElements(Dictionary<object, object> table)
  {
    Dictionary<object, object> elements = new Dictionary<object, object>();
    foreach (KeyValuePair<object, object> keyValuePair in table)
    {
      object key = keyValuePair.Key;
      object obj = keyValuePair.Value;
      Dictionary<object, object> dictionary1 = elements;
      if (key.ToString().Contains("."))
      {
        string[] strArray = key.ToString().Split('.');
        for (int index = 0; index < strArray.Length; ++index)
        {
          if (dictionary1.ContainsKey((object) strArray[index]))
          {
            this.GetElements((Dictionary<object, object>) dictionary1[(object) strArray[index]]);
            dictionary1 = (Dictionary<object, object>) dictionary1[(object) strArray[index]];
          }
          else if (index == strArray.Length - 1)
          {
            dictionary1.Add((object) strArray[index], obj);
          }
          else
          {
            Dictionary<object, object> dictionary2 = new Dictionary<object, object>();
            dictionary1.Add((object) strArray[index], (object) dictionary2);
            dictionary1 = (Dictionary<object, object>) dictionary1[(object) strArray[index]];
          }
        }
      }
      else
        dictionary1.Add(key, obj);
    }
    return elements;
  }

  private void WriteFieldName(Dictionary<object, object> value, XmlTextWriter textWriter)
  {
    foreach (KeyValuePair<object, object> keyValuePair in value)
    {
      if (keyValuePair.Value is Dictionary<object, object>)
      {
        textWriter.WriteStartElement("field");
        textWriter.WriteAttributeString("Name".ToLower(), keyValuePair.Key.ToString());
        this.WriteFieldName((Dictionary<object, object>) keyValuePair.Value, textWriter);
        textWriter.WriteEndElement();
      }
      else
      {
        textWriter.WriteStartElement("field");
        textWriter.WriteAttributeString("Name".ToLower(), keyValuePair.Key.ToString());
        if (keyValuePair.Value.GetType().Name == "PdfArray")
        {
          foreach (PdfString pdfString in keyValuePair.Value as PdfArray)
          {
            textWriter.WriteStartElement(nameof (value));
            textWriter.WriteString(pdfString.Value.ToString());
            textWriter.WriteEndElement();
          }
        }
        else
        {
          if (!keyValuePair.Value.ToString().EndsWith(this.isContainsRV) || this.isContainsRV == "")
          {
            textWriter.WriteStartElement(nameof (value));
            textWriter.WriteString(keyValuePair.Value.ToString());
          }
          else
          {
            textWriter.WriteStartElement("value-richtext");
            string str = keyValuePair.Value.ToString();
            if (str.StartsWith("<?xml version=\"1.0\"?>"))
              str = str.Remove(0, 21);
            string data = str.Remove(str.Length - this.isContainsRV.Length, this.isContainsRV.Length);
            textWriter.WriteRaw(data);
          }
          textWriter.WriteEndElement();
        }
        textWriter.WriteEndElement();
      }
    }
  }

  internal void ExportAnnotations(Stream stream, PdfLoadedDocument document)
  {
    this.m_document = document;
    this.Save(stream);
  }

  private void WriteFormData(XmlTextWriter textWriter)
  {
    foreach (KeyValuePair<object, object> keyValuePair in this.table)
    {
      textWriter.WriteStartElement("field");
      textWriter.WriteAttributeString("Name".ToLower(), keyValuePair.Key.ToString());
      if (keyValuePair.Value.GetType().Name == "PdfArray")
      {
        foreach (PdfString pdfString in keyValuePair.Value as PdfArray)
        {
          textWriter.WriteStartElement("value");
          textWriter.WriteString(pdfString.Value.ToString());
          textWriter.WriteEndElement();
        }
      }
      else
      {
        textWriter.WriteStartElement("value");
        textWriter.WriteString(keyValuePair.Value.ToString());
        textWriter.WriteEndElement();
      }
      textWriter.WriteEndElement();
    }
  }

  private void ExportAnnotationData(
    PdfLoadedAnnotation annotation,
    int index,
    XmlTextWriter textWriter)
  {
    if (annotation.Dictionary == null)
      return;
    switch (annotation)
    {
      case PdfLoadedFileLinkAnnotation _:
        break;
      case PdfLoadedTextWebLinkAnnotation _:
        break;
      case PdfLoadedDocumentLinkAnnotation _:
        break;
      case PdfLoadedUriAnnotation _:
        break;
      default:
        this.WriteAnnotationData(annotation, index, textWriter, annotation.Dictionary);
        break;
    }
  }

  private void WriteAnnotationData(
    PdfLoadedAnnotation annotation,
    int pageIndex,
    XmlTextWriter textWriter,
    PdfDictionary dictionary)
  {
    bool hasAppearance = false;
    string annotationType = this.GetAnnotationType(dictionary);
    this.m_skipBorderStyle = false;
    if (string.IsNullOrEmpty(annotationType))
      return;
    List<string> annotationAttributes = this.m_annotationAttributes;
    this.m_annotationAttributes = new List<string>();
    textWriter.WriteStartElement(annotationType.ToLower());
    textWriter.WriteAttributeString("page", pageIndex.ToString());
    switch (annotationType)
    {
      case "Line":
        float[] linePoint = (annotation as PdfLoadedLineAnnotation).LinePoint;
        textWriter.WriteAttributeString("start", $"{linePoint[0].ToString()},{linePoint[1].ToString()}");
        textWriter.WriteAttributeString("end", $"{linePoint[2].ToString()},{linePoint[3].ToString()}");
        break;
      case "Stamp":
        hasAppearance = true;
        break;
    }
    if (dictionary != null && dictionary.ContainsKey("BE") && dictionary.ContainsKey("BS") && PdfCrossTable.Dereference(dictionary["BE"]) is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey("S"))
      this.m_skipBorderStyle = true;
    this.WriteDictionary(dictionary, pageIndex, textWriter, hasAppearance);
    textWriter.WriteEndElement();
    this.m_annotationAttributes.Clear();
  }

  private void WriteDictionary(
    PdfDictionary dictionary,
    int pageIndex,
    XmlTextWriter textWriter,
    bool hasAppearance)
  {
    bool flag = false;
    if (dictionary.ContainsKey("Type"))
    {
      PdfName pdfName = PdfCrossTable.Dereference(dictionary["Type"]) as PdfName;
      if (pdfName != (PdfName) null && pdfName.Value == "Border" && this.m_skipBorderStyle)
        flag = true;
    }
    foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in dictionary.Items)
    {
      string key = keyValuePair.Key.Value;
      if ((hasAppearance || !(key == "AP")) && !(key == "P") && !(key == "Parent"))
      {
        IPdfPrimitive dictionary1 = keyValuePair.Value;
        if ((object) (dictionary1 as PdfReferenceHolder) != null)
        {
          if ((dictionary1 as PdfReferenceHolder).Object is PdfDictionary dictionary2)
          {
            switch (key)
            {
              case "BS":
                this.WriteDictionary(dictionary2, pageIndex, textWriter, false);
                continue;
              case "BE":
                this.WriteDictionary(dictionary2, pageIndex, textWriter, false);
                continue;
              case "IRT":
                if (dictionary2.ContainsKey("NM"))
                {
                  textWriter.WriteAttributeString("inreplyto", this.GetValue(dictionary2["NM"]));
                  continue;
                }
                continue;
              default:
                continue;
            }
          }
        }
        else if (dictionary1 is PdfDictionary)
          this.WriteDictionary(dictionary1 as PdfDictionary, pageIndex, textWriter, false);
        else if (!flag || flag && key != "S")
          this.WriteAttribute(textWriter, key, keyValuePair.Value);
      }
    }
    if (hasAppearance && dictionary.ContainsKey("AP"))
    {
      MemoryStream appearanceString = this.GetAppearanceString(dictionary["AP"]);
      if (appearanceString != null && appearanceString.Length > 0L && appearanceString.CanRead && appearanceString.CanSeek)
      {
        textWriter.WriteStartElement("appearance");
        textWriter.WriteBase64(appearanceString.ToArray(), 0, (int) appearanceString.Length);
        textWriter.WriteEndElement();
      }
    }
    if (dictionary.ContainsKey("Measure"))
      this.ExportMeasureDictionary(dictionary, textWriter);
    if (dictionary.ContainsKey("Sound"))
    {
      IPdfPrimitive pdfPrimitive = dictionary["Sound"];
      if (((object) (pdfPrimitive as PdfReferenceHolder) != null ? (pdfPrimitive as PdfReferenceHolder).Object : pdfPrimitive) is PdfStream pdfStream)
      {
        if (pdfStream.ContainsKey("B"))
          textWriter.WriteAttributeString("bits", this.GetValue(pdfStream["B"]));
        if (pdfStream.ContainsKey("C"))
          textWriter.WriteAttributeString("channels", this.GetValue(pdfStream["C"]));
        if (pdfStream.ContainsKey("E"))
          textWriter.WriteAttributeString("encoding", this.GetValue(pdfStream["E"]));
        if (pdfStream.ContainsKey("R"))
          textWriter.WriteAttributeString("rate", this.GetValue(pdfStream["R"]));
        if (pdfStream.Data.Length > 0)
        {
          string hex = PdfString.BytesToHex(pdfStream.Data);
          if (!string.IsNullOrEmpty(hex))
          {
            textWriter.WriteStartElement("data");
            textWriter.WriteAttributeString("MODE", "raw");
            textWriter.WriteAttributeString("Encoding".ToLower(), "hex");
            if (pdfStream.ContainsKey("Length"))
              textWriter.WriteAttributeString("Length".ToLower(), this.GetValue(pdfStream["Length"]));
            if (pdfStream.ContainsKey("Filter"))
              textWriter.WriteAttributeString("Filter".ToLower(), this.GetValue(pdfStream["Filter"]));
            textWriter.WriteRaw(hex);
            textWriter.WriteEndElement();
          }
        }
      }
    }
    else if (dictionary.ContainsKey("FS"))
    {
      IPdfPrimitive pdfPrimitive1 = dictionary["FS"];
      if (((object) (pdfPrimitive1 as PdfReferenceHolder) != null ? (pdfPrimitive1 as PdfReferenceHolder).Object : pdfPrimitive1) is PdfDictionary pdfDictionary2)
      {
        if (pdfDictionary2.ContainsKey("F"))
          textWriter.WriteAttributeString("file", this.GetValue(pdfDictionary2["F"]));
        if (pdfDictionary2.ContainsKey("EF"))
        {
          IPdfPrimitive pdfPrimitive2 = pdfDictionary2["EF"];
          if (((object) (pdfPrimitive2 as PdfReferenceHolder) != null ? (pdfPrimitive2 as PdfReferenceHolder).Object : pdfPrimitive2) is PdfDictionary pdfDictionary1 && pdfDictionary1.ContainsKey("F"))
          {
            IPdfPrimitive pdfPrimitive3 = pdfDictionary1["F"];
            if (((object) (pdfPrimitive3 as PdfReferenceHolder) != null ? (pdfPrimitive3 as PdfReferenceHolder).Object : pdfPrimitive3) is PdfStream pdfStream)
            {
              if (pdfStream.ContainsKey("Params"))
              {
                IPdfPrimitive pdfPrimitive4 = pdfStream["Params"];
                if (((object) (pdfPrimitive4 as PdfReferenceHolder) != null ? (pdfPrimitive4 as PdfReferenceHolder).Object : pdfPrimitive4) is PdfDictionary pdfDictionary)
                {
                  if (pdfDictionary.ContainsKey("CreationDate"))
                    textWriter.WriteAttributeString("creation", this.GetValue(pdfDictionary["CreationDate"]));
                  if (pdfDictionary.ContainsKey("ModDate"))
                    textWriter.WriteAttributeString("modification", this.GetValue(pdfDictionary["ModDate"]));
                  if (pdfDictionary.ContainsKey("Size"))
                    textWriter.WriteAttributeString("Size".ToLower(), this.GetValue(pdfDictionary["Size"]));
                  if (pdfDictionary.ContainsKey("CheckSum"))
                  {
                    string str = BitConverter.ToString(Encoding.Default.GetBytes(this.GetValue(pdfDictionary["CheckSum"]))).Replace("-", "");
                    textWriter.WriteAttributeString("checksum", str);
                  }
                }
              }
              string hex = PdfString.BytesToHex(pdfStream.Data);
              if (!string.IsNullOrEmpty(hex))
              {
                textWriter.WriteStartElement("DATA".ToLower());
                textWriter.WriteAttributeString("MODE", "RAW".ToLower());
                textWriter.WriteAttributeString("Encoding".ToLower(), "HEX".ToLower());
                if (pdfStream.ContainsKey("Length"))
                  textWriter.WriteAttributeString("Length".ToLower(), this.GetValue(pdfStream["Length"]));
                if (pdfStream.ContainsKey("Filter"))
                  textWriter.WriteAttributeString("Filter".ToLower(), this.GetValue(pdfStream["Filter"]));
                textWriter.WriteRaw(hex);
                textWriter.WriteEndElement();
              }
            }
          }
        }
      }
    }
    if (dictionary.ContainsKey("Vertices"))
    {
      textWriter.WriteStartElement("Vertices".ToLower());
      if (dictionary["Vertices"] is PdfArray pdfArray && pdfArray.Count > 0)
      {
        int count = pdfArray.Count;
        if (count % 2 == 0)
        {
          string data = string.Empty;
          for (int index = 0; index < count - 1; ++index)
          {
            if (pdfArray.Elements[index] is PdfNumber element)
              data = data + this.GetValue((IPdfPrimitive) element) + (index % 2 != 0 ? ";" : ",");
          }
          if (pdfArray.Elements[count - 1] is PdfNumber element1)
            data += this.GetValue((IPdfPrimitive) element1);
          if (!string.IsNullOrEmpty(data))
            textWriter.WriteRaw(data);
        }
      }
      textWriter.WriteEndElement();
    }
    if (dictionary.ContainsKey("Popup") && (dictionary["Popup"] as PdfReferenceHolder).Object is PdfDictionary dictionary3)
      this.WriteAnnotationData((PdfLoadedAnnotation) null, pageIndex, textWriter, dictionary3);
    if (dictionary.ContainsKey("DA") && dictionary["DA"] is PdfString pdfString1)
      this.WriteRawData(textWriter, "defaultappearance", pdfString1.Value);
    if (dictionary.ContainsKey("DS") && dictionary["DS"] is PdfString pdfString2)
      this.WriteRawData(textWriter, "defaultstyle", pdfString2.Value);
    if (dictionary.ContainsKey("InkList") && dictionary["InkList"] is PdfArray pdfArray1 && pdfArray1.Count > 0)
    {
      textWriter.WriteStartElement("InkList".ToLower());
      for (int index = 0; index < pdfArray1.Count; ++index)
      {
        PdfArray primitive = pdfArray1[index] as PdfArray;
        textWriter.WriteElementString("gesture", this.GetValue((IPdfPrimitive) primitive));
      }
      textWriter.WriteEndElement();
    }
    if (dictionary.ContainsKey("RC"))
    {
      string str = (dictionary["RC"] as PdfString).Value;
      int startIndex = str.IndexOf("<body");
      if (startIndex > 0)
        str = str.Substring(startIndex);
      this.WriteRawData(textWriter, "contents-richtext", str);
    }
    if (!dictionary.ContainsKey("Contents") || !(dictionary["Contents"] is PdfString pdfString3))
      return;
    string str1 = pdfString3.Value.Replace("<", "&lt;");
    if (string.IsNullOrEmpty(str1))
      return;
    this.WriteRawData(textWriter, "Contents".ToLower(), str1);
  }

  private MemoryStream GetAppearanceString(IPdfPrimitive primitive)
  {
    MemoryStream w = new MemoryStream();
    XmlTextWriter textWriter = new XmlTextWriter((Stream) w, (Encoding) new UTF8Encoding());
    textWriter.Formatting = Formatting.Indented;
    textWriter.WriteStartElement("DICT");
    textWriter.WriteAttributeString("KEY", "AP");
    if (((object) (primitive as PdfReferenceHolder) != null ? (primitive as PdfReferenceHolder).Object : primitive) is PdfDictionary dictionary)
      this.WriteAppearanceDictionary(textWriter, dictionary);
    textWriter.WriteEndElement();
    textWriter.Flush();
    return w;
  }

  private void WriteAppearanceDictionary(XmlTextWriter textWriter, PdfDictionary dictionary)
  {
    if (dictionary == null || dictionary.Count <= 0)
      return;
    foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in dictionary.Items)
      this.WriteObject(textWriter, keyValuePair.Key.Value, keyValuePair.Value);
  }

  private void WriteObject(XmlTextWriter textWriter, string key, IPdfPrimitive primitive)
  {
    if (primitive == null)
      return;
    switch (primitive.GetType().ToString())
    {
      case "Syncfusion.Pdf.Primitives.PdfReferenceHolder":
        PdfReferenceHolder pdfReferenceHolder = primitive as PdfReferenceHolder;
        this.WriteObject(textWriter, key, pdfReferenceHolder.Object);
        break;
      case "Syncfusion.Pdf.Primitives.PdfDictionary":
        PdfDictionary dictionary1 = primitive as PdfDictionary;
        textWriter.WriteStartElement("DICT");
        textWriter.WriteAttributeString("KEY", key);
        this.WriteAppearanceDictionary(textWriter, dictionary1);
        textWriter.WriteEndElement();
        break;
      case "Syncfusion.Pdf.Primitives.PdfStream":
        PdfStream dictionary2 = (primitive as PdfStream).Clone(this.m_document.CrossTable) as PdfStream;
        if (dictionary2.Data.Length <= 0)
          break;
        textWriter.WriteStartElement("STREAM");
        textWriter.WriteAttributeString("KEY", key);
        textWriter.WriteAttributeString("DEFINE", "");
        this.WriteAppearanceDictionary(textWriter, (PdfDictionary) dictionary2);
        textWriter.WriteStartElement("DATA");
        if (dictionary2.ContainsKey("Subtype") && "Image" == this.GetValue(dictionary2["Subtype"]))
        {
          textWriter.WriteAttributeString("MODE", "RAW");
          textWriter.WriteAttributeString("Encoding".ToUpper(), "HEX");
          string data = BitConverter.ToString(dictionary2.Data).Replace("-", "");
          if (!string.IsNullOrEmpty(data))
            textWriter.WriteRaw(data);
        }
        else if (!dictionary2.ContainsKey("Type") && !dictionary2.ContainsKey("Subtype"))
        {
          textWriter.WriteAttributeString("MODE", "RAW");
          textWriter.WriteAttributeString("Encoding".ToUpper(), "HEX");
          string data = BitConverter.ToString(dictionary2.Data).Replace("-", "");
          if (!string.IsNullOrEmpty(data))
            textWriter.WriteRaw(data);
        }
        else
        {
          textWriter.WriteAttributeString("MODE", "FILTERED");
          textWriter.WriteAttributeString("Encoding".ToUpper(), "ASCII");
          string str = Encoding.Default.GetString(dictionary2.GetDecompressedData());
          if (!string.IsNullOrEmpty(str))
            textWriter.WriteRaw(this.GetFormatedString(str));
        }
        textWriter.WriteEndElement();
        textWriter.WriteEndElement();
        break;
      case "Syncfusion.Pdf.Primitives.PdfBoolean":
        PdfBoolean pdfBoolean = primitive as PdfBoolean;
        textWriter.WriteStartElement("BOOL");
        textWriter.WriteAttributeString("KEY", key);
        textWriter.WriteAttributeString("VAL", pdfBoolean.Value ? "true" : "false");
        textWriter.WriteEndElement();
        break;
      case "Syncfusion.Pdf.Primitives.PdfName":
        PdfName pdfName = primitive as PdfName;
        textWriter.WriteStartElement("NAME");
        textWriter.WriteAttributeString("KEY", key);
        textWriter.WriteAttributeString("VAL", pdfName.Value);
        textWriter.WriteEndElement();
        break;
      case "Syncfusion.Pdf.Primitives.PdfString":
        PdfString pdfString = primitive as PdfString;
        textWriter.WriteStartElement("STRING");
        textWriter.WriteAttributeString("KEY", key);
        textWriter.WriteAttributeString("VAL", pdfString.Value);
        textWriter.WriteEndElement();
        break;
      case "Syncfusion.Pdf.Primitives.PdfNumber":
        PdfNumber pdfNumber = primitive as PdfNumber;
        if (pdfNumber.IsInteger)
        {
          textWriter.WriteStartElement("INT");
          textWriter.WriteAttributeString("KEY", key);
          textWriter.WriteAttributeString("VAL", pdfNumber.IntValue.ToString());
        }
        else if (pdfNumber.IsLong)
        {
          textWriter.WriteStartElement("INT");
          textWriter.WriteAttributeString("KEY", key);
          textWriter.WriteAttributeString("VAL", pdfNumber.LongValue.ToString());
        }
        else
        {
          textWriter.WriteStartElement("FIXED");
          textWriter.WriteAttributeString("KEY", key);
          string str = Math.Round((double) pdfNumber.FloatValue, 6).ToString();
          if (!str.Contains("."))
            str += ".000000";
          textWriter.WriteAttributeString("VAL", str);
        }
        textWriter.WriteEndElement();
        break;
      case "Syncfusion.Pdf.Primitives.PdfNull":
        textWriter.WriteStartElement("NULL");
        textWriter.WriteAttributeString("KEY", key);
        textWriter.WriteEndElement();
        break;
      case "Syncfusion.Pdf.Primitives.PdfArray":
        textWriter.WriteStartElement("ARRAY");
        textWriter.WriteAttributeString("KEY", key);
        this.WriteArray(textWriter, primitive as PdfArray);
        textWriter.WriteEndElement();
        break;
    }
  }

  private void WriteArray(XmlTextWriter textWriter, PdfArray array)
  {
    foreach (IPdfPrimitive element in array.Elements)
      this.WriteArrayElement(textWriter, element);
  }

  private void WriteArrayElement(XmlTextWriter textWriter, IPdfPrimitive element)
  {
    switch (element.GetType().ToString())
    {
      case "Syncfusion.Pdf.Primitives.PdfArray":
        textWriter.WriteStartElement("ARRAY");
        this.WriteArray(textWriter, element as PdfArray);
        textWriter.WriteEndElement();
        break;
      case "Syncfusion.Pdf.Primitives.PdfName":
        PdfName pdfName = element as PdfName;
        textWriter.WriteStartElement("NAME");
        textWriter.WriteAttributeString("VAL", pdfName.Value);
        textWriter.WriteEndElement();
        break;
      case "Syncfusion.Pdf.Primitives.PdfString":
        PdfString pdfString = element as PdfString;
        textWriter.WriteStartElement("STRING");
        textWriter.WriteAttributeString("VAL", pdfString.Value);
        textWriter.WriteEndElement();
        break;
      case "Syncfusion.Pdf.Primitives.PdfNumber":
        PdfNumber pdfNumber = element as PdfNumber;
        if (pdfNumber.IsInteger)
        {
          textWriter.WriteStartElement("INT");
          textWriter.WriteAttributeString("VAL", pdfNumber.IntValue.ToString());
        }
        else if (pdfNumber.IsLong)
        {
          textWriter.WriteStartElement("INT");
          textWriter.WriteAttributeString("VAL", pdfNumber.LongValue.ToString());
        }
        else
        {
          textWriter.WriteStartElement("FIXED");
          string str = Math.Round((double) pdfNumber.FloatValue, 6).ToString();
          if (!str.Contains("."))
            str += ".000000";
          textWriter.WriteAttributeString("VAL", str);
        }
        textWriter.WriteEndElement();
        break;
      case "Syncfusion.Pdf.Primitives.PdfBoolean":
        PdfBoolean pdfBoolean = element as PdfBoolean;
        textWriter.WriteStartElement("BOOL");
        textWriter.WriteAttributeString("VAL", pdfBoolean.Value ? "true" : "false");
        textWriter.WriteEndElement();
        break;
      case "Syncfusion.Pdf.Primitives.PdfDictionary":
        PdfDictionary dictionary1 = element as PdfDictionary;
        textWriter.WriteStartElement("DICT");
        this.WriteAppearanceDictionary(textWriter, dictionary1);
        textWriter.WriteEndElement();
        break;
      case "Syncfusion.Pdf.Primitives.PdfStream":
        PdfStream dictionary2 = (element as PdfStream).Clone(this.m_document.CrossTable) as PdfStream;
        if (dictionary2.Data.Length <= 0)
          break;
        textWriter.WriteStartElement("STREAM");
        textWriter.WriteAttributeString("DEFINE", "");
        this.WriteAppearanceDictionary(textWriter, (PdfDictionary) dictionary2);
        textWriter.WriteStartElement("DATA");
        if (dictionary2.ContainsKey("Subtype") && "Image" == this.GetValue(dictionary2["Subtype"]))
        {
          textWriter.WriteAttributeString("MODE", "RAW");
          textWriter.WriteAttributeString("Encoding".ToUpper(), "HEX");
          string data = BitConverter.ToString(dictionary2.Data).Replace("-", "");
          if (!string.IsNullOrEmpty(data))
            textWriter.WriteRaw(data);
        }
        else
        {
          textWriter.WriteAttributeString("MODE", "FILTERED");
          textWriter.WriteAttributeString("Encoding".ToUpper(), "ASCII");
          string str = Encoding.Default.GetString(dictionary2.GetDecompressedData());
          if (!string.IsNullOrEmpty(str))
            textWriter.WriteRaw(this.GetFormatedString(str));
        }
        textWriter.WriteEndElement();
        textWriter.WriteEndElement();
        break;
      case "Syncfusion.Pdf.Primitives.PdfReferenceHolder":
        PdfReferenceHolder pdfReferenceHolder = element as PdfReferenceHolder;
        if (pdfReferenceHolder.Object == null)
          break;
        this.WriteArrayElement(textWriter, pdfReferenceHolder.Object);
        break;
    }
  }

  private void WriteAttribute(XmlTextWriter textWriter, string key, IPdfPrimitive primitive)
  {
    if (this.m_annotationAttributes == null || this.m_annotationAttributes.Contains(key))
      return;
    switch (key)
    {
      case "C":
        string color1 = this.GetColor(primitive);
        if (primitive is PdfNumber)
        {
          string str = this.GetValue(primitive);
          if (!string.IsNullOrEmpty(str) && !this.m_annotationAttributes.Contains("c"))
          {
            textWriter.WriteAttributeString("c", str);
            this.m_annotationAttributes.Add("c");
          }
        }
        if (string.IsNullOrEmpty(color1) || this.m_annotationAttributes.Contains("color"))
          break;
        textWriter.WriteAttributeString("color", color1);
        this.m_annotationAttributes.Add("color");
        break;
      case "IC":
        string color2 = this.GetColor(primitive);
        if (string.IsNullOrEmpty(color2) || this.m_annotationAttributes.Contains("interior-color"))
          break;
        textWriter.WriteAttributeString("interior-color", color2);
        this.m_annotationAttributes.Add("interior-color");
        break;
      case "M":
        if (this.m_annotationAttributes.Contains("date"))
          break;
        textWriter.WriteAttributeString("date", this.GetValue(primitive));
        this.m_annotationAttributes.Add("date");
        break;
      case "NM":
        if (this.m_annotationAttributes.Contains("Name".ToLower()))
          break;
        textWriter.WriteAttributeString("Name".ToLower(), this.GetValue(primitive));
        this.m_annotationAttributes.Add("Name".ToLower());
        break;
      case "Name":
        if (this.m_annotationAttributes.Contains("icon"))
          break;
        textWriter.WriteAttributeString("icon", this.GetValue(primitive));
        this.m_annotationAttributes.Add("icon");
        break;
      case "Subj":
        if (this.m_annotationAttributes.Contains("Subject".ToLower()))
          break;
        textWriter.WriteAttributeString("Subject".ToLower(), this.GetValue(primitive));
        this.m_annotationAttributes.Add("Subject".ToLower());
        break;
      case "T":
        if (this.m_annotationAttributes.Contains("Title".ToLower()))
          break;
        textWriter.WriteAttributeString("Title".ToLower(), this.GetValue(primitive));
        this.m_annotationAttributes.Add("Title".ToLower());
        break;
      case "Rect":
      case "CreationDate":
        if (this.m_annotationAttributes.Contains(key.ToLower()))
          break;
        textWriter.WriteAttributeString(key.ToLower(), this.GetValue(primitive));
        this.m_annotationAttributes.Add(key.ToLower());
        break;
      case "Rotate":
        if (this.m_annotationAttributes.Contains("rotation"))
          break;
        textWriter.WriteAttributeString("rotation", this.GetValue(primitive));
        this.m_annotationAttributes.Add("rotation");
        break;
      case "W":
        if (this.m_annotationAttributes.Contains("Width".ToLower()))
          break;
        textWriter.WriteAttributeString("Width".ToLower(), this.GetValue(primitive));
        this.m_annotationAttributes.Add("Width".ToLower());
        break;
      case "LE":
        if (primitive is PdfArray)
        {
          PdfArray pdfArray = primitive as PdfArray;
          if (pdfArray.Count != 2)
            break;
          textWriter.WriteAttributeString("head", this.GetValue(pdfArray.Elements[0]));
          textWriter.WriteAttributeString("tail", this.GetValue(pdfArray.Elements[1]));
          break;
        }
        if ((object) (primitive as PdfName) == null || this.m_annotationAttributes.Contains("head"))
          break;
        textWriter.WriteAttributeString("head", this.GetValue(primitive));
        this.m_annotationAttributes.Add("head");
        break;
      case "S":
        if (this.m_annotationAttributes.Contains("style"))
          break;
        switch (this.GetValue(primitive))
        {
          case "D":
            textWriter.WriteAttributeString("style", "dash");
            break;
          case "C":
            textWriter.WriteAttributeString("style", "cloudy");
            break;
          case "S":
            textWriter.WriteAttributeString("style", "solid");
            break;
          case "B":
            textWriter.WriteAttributeString("style", "bevelled");
            break;
          case "I":
            textWriter.WriteAttributeString("style", "inset");
            break;
          case "U":
            textWriter.WriteAttributeString("style", "underline");
            break;
        }
        this.m_annotationAttributes.Add("style");
        break;
      case "D":
        if (this.m_annotationAttributes.Contains("dashes"))
          break;
        textWriter.WriteAttributeString("dashes", this.GetValue(primitive));
        this.m_annotationAttributes.Add("dashes");
        break;
      case "I":
        if (this.m_annotationAttributes.Contains("intensity"))
          break;
        textWriter.WriteAttributeString("intensity", this.GetValue(primitive));
        this.m_annotationAttributes.Add("intensity");
        break;
      case "RD":
        if (this.m_annotationAttributes.Contains("fringe"))
          break;
        textWriter.WriteAttributeString("fringe", this.GetValue(primitive));
        this.m_annotationAttributes.Add("fringe");
        break;
      case "IT":
        if (this.m_annotationAttributes.Contains(key))
          break;
        textWriter.WriteAttributeString(key, this.GetValue(primitive));
        this.m_annotationAttributes.Add(key);
        break;
      case "RT":
        if (this.m_annotationAttributes.Contains("replyType"))
          break;
        textWriter.WriteAttributeString("replyType", this.GetValue(primitive).ToLower());
        this.m_annotationAttributes.Add("replyType");
        break;
      case "LL":
        if (this.m_annotationAttributes.Contains("leaderLength"))
          break;
        textWriter.WriteAttributeString("leaderLength", this.GetValue(primitive));
        this.m_annotationAttributes.Add("leaderLength");
        break;
      case "LLE":
        if (this.m_annotationAttributes.Contains("leaderExtend"))
          break;
        textWriter.WriteAttributeString("leaderExtend", this.GetValue(primitive));
        this.m_annotationAttributes.Add("leaderExtend");
        break;
      case "Cap":
        if (this.m_annotationAttributes.Contains("caption"))
          break;
        textWriter.WriteAttributeString("caption", this.GetValue(primitive));
        this.m_annotationAttributes.Add("caption");
        break;
      case "CP":
        if (this.m_annotationAttributes.Contains("caption-style"))
          break;
        textWriter.WriteAttributeString("caption-style", this.GetValue(primitive));
        this.m_annotationAttributes.Add("caption-style");
        break;
      case "CL":
        if (this.m_annotationAttributes.Contains("callout"))
          break;
        textWriter.WriteAttributeString("callout", this.GetValue(primitive));
        this.m_annotationAttributes.Add("callout");
        break;
      case "FD":
        if (this.m_annotationAttributes.Contains(key.ToLower()))
          break;
        textWriter.WriteAttributeString(key.ToLower(), this.GetValue(primitive));
        this.m_annotationAttributes.Add(key.ToLower());
        break;
      case "QuadPoints":
        if (this.m_annotationAttributes.Contains("Coords".ToLower()))
          break;
        textWriter.WriteAttributeString("Coords".ToLower(), this.GetValue(primitive));
        this.m_annotationAttributes.Add("Coords".ToLower());
        break;
      case "CA":
        if (this.m_annotationAttributes.Contains("opacity"))
          break;
        textWriter.WriteAttributeString("opacity", this.GetValue(primitive));
        this.m_annotationAttributes.Add("opacity");
        break;
      case "F":
        if (!(primitive is PdfNumber pdfNumber) || this.m_annotationAttributes.Contains("Flags".ToLower()))
          break;
        string str1 = ((PdfAnnotationFlags) pdfNumber.IntValue).ToString().ToLower().Replace(" ", "");
        textWriter.WriteAttributeString("Flags".ToLower(), str1);
        this.m_annotationAttributes.Add("Flags".ToLower());
        break;
      case "InkList":
        break;
      case "Type":
        break;
      case "Subtype":
        break;
      case "P":
        break;
      case "Parent":
        break;
      case "L":
        break;
      case "Contents":
        break;
      case "RC":
        break;
      case "DA":
        break;
      case "DS":
        break;
      case "FS":
        break;
      case "MeasurementTypes":
        break;
      case "Vertices":
        break;
      case "GroupNesting":
        break;
      case "ITEx":
        break;
      default:
        if (this.m_annotationAttributes.Contains(key.ToLower()))
          break;
        textWriter.WriteAttributeString(key.ToLower(), this.GetValue(primitive));
        this.m_annotationAttributes.Add(key.ToLower());
        break;
    }
  }

  private void WriteRawData(XmlTextWriter textWriter, string name, string value)
  {
    if (string.IsNullOrEmpty(value))
      return;
    textWriter.WriteStartElement(name);
    textWriter.WriteRaw(value);
    textWriter.WriteEndElement();
  }

  private string GetColor(IPdfPrimitive primitive)
  {
    string color = string.Empty;
    if (primitive != null && primitive is PdfArray pdfArray && pdfArray.Count >= 3)
    {
      string str1 = Convert.ToInt32((pdfArray.Elements[0] as PdfNumber).FloatValue * (float) byte.MaxValue).ToString("X");
      string str2 = Convert.ToInt32((pdfArray.Elements[1] as PdfNumber).FloatValue * (float) byte.MaxValue).ToString("X");
      string str3 = Convert.ToInt32((pdfArray.Elements[2] as PdfNumber).FloatValue * (float) byte.MaxValue).ToString("X");
      color = $"#{(str1.Length == 1 ? "0" + str1 : str1)}{(str2.Length == 1 ? "0" + str2 : str2)}{(str3.Length == 1 ? "0" + str3 : str3)}";
    }
    return color;
  }

  private string GetValue(IPdfPrimitive primitive)
  {
    string str = string.Empty;
    if ((object) (primitive as PdfName) != null)
    {
      str = (primitive as PdfName).Value;
    }
    else
    {
      switch (primitive)
      {
        case PdfBoolean _:
          str = (primitive as PdfBoolean).Value ? "yes" : "no";
          break;
        case PdfString _:
          str = (primitive as PdfString).Value;
          break;
        case PdfArray _:
          PdfArray pdfArray = primitive as PdfArray;
          if (pdfArray.Elements.Count > 0)
            str = this.GetValue(pdfArray.Elements[0]);
          for (int index = 1; index < pdfArray.Elements.Count; ++index)
            str = $"{str},{this.GetValue(pdfArray.Elements[index])}";
          break;
        case PdfNumber _:
          str = (primitive as PdfNumber).FloatValue.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          break;
      }
    }
    return str;
  }

  private string GetAnnotationType(PdfDictionary dictionary)
  {
    string empty = string.Empty;
    if (dictionary.ContainsKey("Subtype"))
    {
      PdfName pdfName = dictionary["Subtype"] as PdfName;
      if (pdfName != (PdfName) null)
        empty = pdfName.Value;
    }
    return empty;
  }

  private string GetFormatedString(string value)
  {
    value = value.Replace("<", "&lt;");
    value = value.Replace(">", "&gt;");
    value = value.Replace("&", "&amp;");
    return value;
  }

  private void ExportMeasureDictionary(PdfDictionary dictionary, XmlTextWriter textWriter)
  {
    textWriter.WriteStartElement("measure");
    IPdfPrimitive pdfPrimitive = dictionary["Measure"];
    if (((object) (pdfPrimitive as PdfReferenceHolder) != null ? (pdfPrimitive as PdfReferenceHolder).Object : pdfPrimitive) is PdfDictionary pdfDictionary)
    {
      if (pdfDictionary.ContainsKey("R"))
        textWriter.WriteAttributeString("rateValue", this.GetValue(pdfDictionary["R"]));
      if (pdfDictionary.ContainsKey("A"))
      {
        IPdfPrimitive element = (pdfDictionary["A"] as PdfArray).Elements[0];
        PdfDictionary measurementDetails = ((object) (element as PdfReferenceHolder) != null ? (element as PdfReferenceHolder).Object : element) as PdfDictionary;
        textWriter.WriteStartElement("area");
        this.ExportMeasureFormatDetails(measurementDetails, textWriter);
        textWriter.WriteEndElement();
      }
      if (pdfDictionary.ContainsKey("D"))
      {
        IPdfPrimitive element = (pdfDictionary["D"] as PdfArray).Elements[0];
        PdfDictionary measurementDetails = ((object) (element as PdfReferenceHolder) != null ? (element as PdfReferenceHolder).Object : element) as PdfDictionary;
        textWriter.WriteStartElement("distance");
        this.ExportMeasureFormatDetails(measurementDetails, textWriter);
        textWriter.WriteEndElement();
      }
      if (pdfDictionary.ContainsKey("X"))
      {
        IPdfPrimitive element = (pdfDictionary["X"] as PdfArray).Elements[0];
        PdfDictionary measurementDetails = ((object) (element as PdfReferenceHolder) != null ? (element as PdfReferenceHolder).Object : element) as PdfDictionary;
        textWriter.WriteStartElement("xformat");
        this.ExportMeasureFormatDetails(measurementDetails, textWriter);
        textWriter.WriteEndElement();
      }
    }
    textWriter.WriteEndElement();
  }

  private void ExportMeasureFormatDetails(
    PdfDictionary measurementDetails,
    XmlTextWriter textWriter)
  {
    if (measurementDetails.ContainsKey("C"))
      textWriter.WriteAttributeString("c", this.GetValue(measurementDetails["C"]));
    if (measurementDetails.ContainsKey("F"))
      textWriter.WriteAttributeString("f", this.GetValue(measurementDetails["F"]));
    if (measurementDetails.ContainsKey("D"))
      textWriter.WriteAttributeString("d", this.GetValue(measurementDetails["D"]));
    if (measurementDetails.ContainsKey("RD"))
      textWriter.WriteAttributeString("rd", this.GetValue(measurementDetails["RD"]));
    if (measurementDetails.ContainsKey("U"))
      textWriter.WriteAttributeString("u", this.GetValue(measurementDetails["U"]));
    if (measurementDetails.ContainsKey("RT"))
      textWriter.WriteAttributeString("rt", this.GetValue(measurementDetails["RT"]));
    if (measurementDetails.ContainsKey("SS"))
      textWriter.WriteAttributeString("ss", this.GetValue(measurementDetails["SS"]));
    if (!measurementDetails.ContainsKey("FD"))
      return;
    textWriter.WriteAttributeString("fd", this.GetValue(measurementDetails["FD"]));
  }
}
