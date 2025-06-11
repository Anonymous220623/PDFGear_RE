// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.JsonDocument
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class JsonDocument
{
  private string pdfFilePath = string.Empty;
  private PdfLoadedDocument m_document;
  private bool m_skipBorderStyle;
  private Dictionary<string, string> table = new Dictionary<string, string>();
  private PdfExportAnnotationCollection m_annotationCollection;
  private int count = 1;
  private bool flag;
  private string jsonData = "{\"pdfAnnotation\":{ \"0\":{ \"shapeAnnotation\":[";

  internal PdfExportAnnotationCollection AnnotationCollection
  {
    get => this.m_annotationCollection;
    set => this.m_annotationCollection = value;
  }

  internal JsonDocument(string filename) => this.pdfFilePath = filename;

  internal void ExportAnnotations(Stream stream, PdfLoadedDocument document)
  {
    this.m_document = document;
    if (this.m_document == null)
      return;
    if (this.m_annotationCollection != null && this.m_annotationCollection.Count > 0)
    {
      foreach (PdfAnnotation annotation1 in (PdfCollection) this.m_annotationCollection)
      {
        if (annotation1 is PdfLoadedAnnotation annotation2)
        {
          int index = this.m_document.Pages.IndexOf((PdfPageBase) annotation1.LoadedPage);
          this.ExportAnnotationData(stream, annotation2, index, this.count);
          ++this.count;
        }
      }
    }
    else
      this.ExportAnnotationData(this.m_document, this.m_document.PageCount, stream);
  }

  private void ExportAnnotationData(
    Stream stream,
    PdfLoadedAnnotation annotation,
    int index,
    int count)
  {
    if (!this.flag)
    {
      byte[] bytes = Encoding.GetEncoding("UTF-8").GetBytes(this.jsonData);
      stream.Write(bytes, 0, bytes.Length);
    }
    PdfPageBase page = this.m_document.Pages[index];
    this.flag = true;
    switch (annotation)
    {
      case PdfLoadedFileLinkAnnotation _:
      case PdfLoadedTextWebLinkAnnotation _:
      case PdfLoadedDocumentLinkAnnotation _:
      case PdfLoadedUriAnnotation _:
label_4:
        this.jsonData = this.convertToJson(this.table);
        this.jsonData = this.m_annotationCollection.Count <= count ? this.jsonData + "]}}}" : this.jsonData + ",";
        byte[] bytes1 = Encoding.GetEncoding("UTF-8").GetBytes(this.jsonData);
        stream.Write(bytes1, 0, bytes1.Length);
        this.table.Clear();
        break;
      default:
        this.ExportAnnotationData(annotation, index, annotation.Dictionary);
        goto label_4;
    }
  }

  private void ExportAnnotationData(PdfLoadedDocument document, int pageCount, Stream stream)
  {
    string s1 = "{\"pdfAnnotation\":{";
    byte[] bytes1 = Encoding.GetEncoding("UTF-8").GetBytes(s1);
    stream.Write(bytes1, 0, bytes1.Length);
    for (int index = 0; index < pageCount; ++index)
    {
      PdfLoadedPage page = document.Pages[index] as PdfLoadedPage;
      if (page.Annotations.Count > 0)
      {
        string s2 = $"{(index == 0 ? " " : ",")}\"{(object) index}\":{{ \"shapeAnnotation\":[";
        byte[] bytes2 = Encoding.GetEncoding("UTF-8").GetBytes(s2);
        stream.Write(bytes2, 0, bytes2.Length);
      }
      int num = 0;
      foreach (PdfAnnotation annotation1 in (PdfCollection) page.Annotations)
      {
        ++num;
        if (annotation1 is PdfLoadedAnnotation annotation2)
        {
          this.ExportAnnotationData(annotation2, index, annotation2.Dictionary);
          string json = this.convertToJson(this.table);
          if (num < page.Annotations.Count)
            json += ",";
          byte[] bytes3 = Encoding.GetEncoding("UTF-8").GetBytes(json);
          stream.Write(bytes3, 0, bytes3.Length);
          this.table.Clear();
        }
      }
      if (page.Annotations.Count > 0)
      {
        string s3 = "]}";
        byte[] bytes4 = Encoding.GetEncoding("UTF-8").GetBytes(s3);
        stream.Write(bytes4, 0, bytes4.Length);
      }
    }
    string s4 = "}}";
    byte[] bytes5 = Encoding.GetEncoding("UTF-8").GetBytes(s4);
    stream.Write(bytes5, 0, bytes5.Length);
  }

  private void ExportAnnotationData(
    PdfLoadedAnnotation annotation,
    int pageIndex,
    PdfDictionary dictionary)
  {
    bool hasAppearance = false;
    string annotationType = this.GetAnnotationType(dictionary);
    this.m_skipBorderStyle = false;
    if (string.IsNullOrEmpty(annotationType))
      return;
    this.table.Add("type", annotationType);
    this.table.Add("page", pageIndex.ToString());
    switch (annotationType)
    {
      case "Line":
        int[] linePoints = (annotation as PdfLoadedLineAnnotation).LinePoints;
        this.table.Add("start", $"{linePoints[0].ToString()},{linePoints[1].ToString()}");
        this.table.Add("end", $"{linePoints[2].ToString()},{linePoints[3].ToString()}");
        break;
      case "Stamp":
        hasAppearance = true;
        break;
    }
    if (dictionary != null && dictionary.ContainsKey("BE") && dictionary.ContainsKey("BS") && PdfCrossTable.Dereference(dictionary["BE"]) is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey("S"))
      this.m_skipBorderStyle = true;
    this.WriteDictionary(annotation, pageIndex, dictionary, hasAppearance);
  }

  private void WriteDictionary(
    PdfLoadedAnnotation annotation,
    int pageIndex,
    PdfDictionary dictionary,
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
      if (!(key == "P") && !(key == "Parent"))
      {
        IPdfPrimitive dictionary1 = keyValuePair.Value;
        if ((object) (dictionary1 as PdfReferenceHolder) != null)
        {
          if ((dictionary1 as PdfReferenceHolder).Object is PdfDictionary dictionary2)
          {
            switch (key)
            {
              case "BS":
                this.WriteDictionary(annotation, pageIndex, dictionary2, false);
                continue;
              case "BE":
                this.WriteDictionary(annotation, pageIndex, dictionary2, false);
                continue;
              case "IRT":
                if (dictionary2.ContainsKey("NM"))
                {
                  this.table.Add("inreplyto", this.GetValue(dictionary2["NM"]));
                  continue;
                }
                continue;
              default:
                continue;
            }
          }
        }
        else if (dictionary1 is PdfDictionary)
          this.WriteDictionary(annotation, pageIndex, dictionary1 as PdfDictionary, false);
        else if (!flag || flag && key != "S")
          this.WriteAttribute(key, keyValuePair.Value, pageIndex, dictionary);
      }
    }
    if (dictionary.ContainsKey("Sound"))
    {
      IPdfPrimitive pdfPrimitive = dictionary["Sound"];
      if (!(((object) (pdfPrimitive as PdfReferenceHolder) != null ? (pdfPrimitive as PdfReferenceHolder).Object : pdfPrimitive) is PdfStream pdfStream))
        return;
      if (pdfStream.ContainsKey("B"))
        this.table.Add("bits", this.GetValue(pdfStream["B"]));
      if (pdfStream.ContainsKey("C"))
        this.table.Add("channels", this.GetValue(pdfStream["C"]));
      if (pdfStream.ContainsKey("E"))
        this.table.Add("encoding", this.GetValue(pdfStream["E"]));
      if (pdfStream.ContainsKey("R"))
        this.table.Add("rate", this.GetValue(pdfStream["R"]));
      if (pdfStream.Data.Length <= 0)
        return;
      string hex = PdfString.BytesToHex(pdfStream.Data);
      if (string.IsNullOrEmpty(hex))
        return;
      this.table.Add("MODE".ToLower(), "raw");
      this.table.Add("encodings", "hex");
      if (pdfStream.ContainsKey("Length"))
        this.table.Add("Length".ToLower(), this.GetValue(pdfStream["Length"]));
      if (pdfStream.ContainsKey("Filter"))
        this.table.Add("Filter".ToLower(), this.GetValue(pdfStream["Filter"]));
      this.table.Add("data", hex);
    }
    else
    {
      if (!dictionary.ContainsKey("FS"))
        return;
      IPdfPrimitive pdfPrimitive1 = dictionary["FS"];
      if (!(((object) (pdfPrimitive1 as PdfReferenceHolder) != null ? (pdfPrimitive1 as PdfReferenceHolder).Object : pdfPrimitive1) is PdfDictionary pdfDictionary1))
        return;
      if (pdfDictionary1.ContainsKey("F"))
        this.table.Add("file", this.GetValue(pdfDictionary1["F"]));
      if (!pdfDictionary1.ContainsKey("EF"))
        return;
      IPdfPrimitive pdfPrimitive2 = pdfDictionary1["EF"];
      if (!(((object) (pdfPrimitive2 as PdfReferenceHolder) != null ? (pdfPrimitive2 as PdfReferenceHolder).Object : pdfPrimitive2) is PdfDictionary pdfDictionary2) || !pdfDictionary2.ContainsKey("F"))
        return;
      IPdfPrimitive pdfPrimitive3 = pdfDictionary2["F"];
      if (!(((object) (pdfPrimitive3 as PdfReferenceHolder) != null ? (pdfPrimitive3 as PdfReferenceHolder).Object : pdfPrimitive3) is PdfStream pdfStream))
        return;
      if (pdfStream.ContainsKey("Params"))
      {
        IPdfPrimitive pdfPrimitive4 = pdfStream["Params"];
        if (((object) (pdfPrimitive4 as PdfReferenceHolder) != null ? (pdfPrimitive4 as PdfReferenceHolder).Object : pdfPrimitive4) is PdfDictionary pdfDictionary3)
        {
          if (pdfDictionary3.ContainsKey("CreationDate"))
          {
            PdfString dateTimeStringValue = PdfCrossTable.Dereference(pdfDictionary3["CreationDate"]) as PdfString;
            this.table.Add("creation", dictionary.GetDateTime(dateTimeStringValue).ToString());
          }
          if (pdfDictionary3.ContainsKey("ModDate"))
          {
            PdfString dateTimeStringValue = PdfCrossTable.Dereference(pdfDictionary3["CreationDate"]) as PdfString;
            this.table.Add("modification", dictionary.GetDateTime(dateTimeStringValue).ToString());
          }
          if (pdfDictionary3.ContainsKey("Size"))
            this.table.Add("Size".ToLower(), this.GetValue(pdfDictionary3["Size"]));
          if (pdfDictionary3.ContainsKey("CheckSum"))
            this.table.Add("checksum", BitConverter.ToString(Encoding.Default.GetBytes(this.GetValue(pdfDictionary3["CheckSum"]))).Replace("-", ""));
        }
      }
      string hex = PdfString.BytesToHex(pdfStream.Data);
      if (string.IsNullOrEmpty(hex))
        return;
      this.table.Add("MODE".ToLower(), "RAW".ToLower());
      this.table.Add("Encoding".ToLower(), "HEX".ToLower());
      if (pdfStream.ContainsKey("Length"))
        this.table.Add("Length".ToLower(), this.GetValue(pdfStream["Length"]));
      if (pdfStream.ContainsKey("Filter"))
        this.table.Add("Filter".ToLower(), this.GetValue(pdfStream["Filter"]));
      this.table.Add("DATA".ToLower(), hex);
    }
  }

  private string GetAnnotationType(PdfDictionary dictionary)
  {
    string empty = string.Empty;
    if (dictionary.ContainsKey("Subtype"))
    {
      PdfName pdfName = PdfCrossTable.Dereference(dictionary["Subtype"]) as PdfName;
      if (pdfName != (PdfName) null)
        empty = pdfName.Value;
    }
    return empty;
  }

  private void WriteAttribute(
    string key,
    IPdfPrimitive primitive,
    int p,
    PdfDictionary dictionary)
  {
    switch (key)
    {
      case "C":
        string color1 = this.GetColor(primitive);
        if (color1 == null)
          break;
        this.table.Add("color", color1);
        break;
      case "DA":
        this.table.Add("defaultappearance", (PdfCrossTable.Dereference(dictionary["DA"]) as PdfString).Value);
        break;
      case "IC":
        string color2 = this.GetColor(primitive);
        if (color2 == null)
          break;
        this.table.Add("interior-color", color2);
        break;
      case "M":
        PdfString dateTimeStringValue1 = PdfCrossTable.Dereference(dictionary["M"]) as PdfString;
        this.table.Add("date", dictionary.GetDateTime(dateTimeStringValue1).ToString());
        break;
      case "NM":
        this.table.Add("Name".ToLower(), this.GetValue(primitive));
        break;
      case "Name":
        this.table.Add("icon", this.GetValue(primitive));
        break;
      case "Subj":
        this.table.Add("Subject".ToLower(), this.GetValue(primitive));
        break;
      case "T":
        this.table.Add("Title".ToLower(), this.GetValue(primitive));
        break;
      case "Rect":
        string[] strArray1 = this.GetValue(primitive).Split(',');
        this.table.Add(key.ToLower(), this.convertToJson(new Dictionary<string, string>()
        {
          {
            "x",
            strArray1[0]
          },
          {
            "y",
            strArray1[1]
          },
          {
            "width",
            strArray1[2]
          },
          {
            "height",
            strArray1[3]
          }
        }));
        break;
      case "CreationDate":
        PdfString dateTimeStringValue2 = PdfCrossTable.Dereference(dictionary["CreationDate"]) as PdfString;
        DateTime dateTime = dictionary.GetDateTime(dateTimeStringValue2);
        this.table.Add(key.ToLower(), dateTime.ToString());
        break;
      case "Rotate":
        this.table.Add("rotation", this.GetValue(primitive));
        break;
      case "W":
        this.table.Add("Width".ToLower(), this.GetValue(primitive));
        break;
      case "LE":
        if (primitive is PdfArray)
        {
          PdfArray pdfArray = primitive as PdfArray;
          if (pdfArray.Count != 2)
            break;
          this.table.Add("head", this.GetValue(pdfArray.Elements[0]));
          this.table.Add("tail", this.GetValue(pdfArray.Elements[1]));
          break;
        }
        if ((object) (primitive as PdfName) == null)
          break;
        this.table.Add("head", this.GetValue(primitive));
        break;
      case "S":
        switch (this.GetValue(primitive))
        {
          case null:
            return;
          case "D":
            this.table.Add("style", "dash");
            return;
          case "C":
            this.table.Add("style", "cloudy");
            return;
          case "S":
            this.table.Add("style", "solid");
            return;
          case "B":
            this.table.Add("style", "bevelled");
            return;
          case "I":
            this.table.Add("style", "inset");
            return;
          case "U":
            this.table.Add("style", "underline");
            return;
          default:
            return;
        }
      case "D":
        this.table.Add("dashes", this.GetValue(primitive));
        break;
      case "I":
        this.table.Add("intensity", this.GetValue(primitive));
        break;
      case "RD":
        this.table.Add("fringe", this.GetValue(primitive));
        break;
      case "IT":
        this.table.Add(key, this.GetValue(primitive));
        break;
      case "RT":
        this.table.Add("replyType", this.GetValue(primitive).ToLower());
        break;
      case "LL":
        this.table.Add("leaderLength", this.GetValue(primitive));
        break;
      case "LLE":
        this.table.Add("leaderExtend", this.GetValue(primitive));
        break;
      case "Cap":
        this.table.Add("caption", this.GetValue(primitive));
        break;
      case "CP":
        this.table.Add("caption-style", this.GetValue(primitive));
        break;
      case "CL":
        this.table.Add("callout", this.GetValue(primitive));
        break;
      case "QuadPoints":
        this.table.Add("Coords".ToLower(), this.GetValue(primitive));
        break;
      case "CA":
        this.table.Add("opacity", this.GetValue(primitive));
        break;
      case "F":
        if (!(primitive is PdfNumber pdfNumber))
          break;
        string str1 = ((PdfAnnotationFlags) pdfNumber.IntValue).ToString().ToLower().Replace(" ", "");
        this.table.Add("Flags".ToLower(), str1);
        break;
      case "Contents":
        if (!(PdfCrossTable.Dereference(dictionary["Contents"]) is PdfString pdfString1))
          break;
        string str2 = pdfString1.Value;
        if (string.IsNullOrEmpty(str2))
          break;
        this.table.Add("contents", str2);
        break;
      case "InkList":
        Dictionary<string, string> dictionary1 = new Dictionary<string, string>();
        if (!(PdfCrossTable.Dereference(dictionary["InkList"]) is PdfArray pdfArray1) || pdfArray1.Count <= 0)
          break;
        PdfArray[] pdfArrayArray = new PdfArray[pdfArray1.Count];
        int count1 = pdfArray1.Count;
        for (int index = 0; index < pdfArray1.Count; ++index)
        {
          PdfArray pdfArray2 = pdfArray1[index] as PdfArray;
          pdfArrayArray[index] = pdfArray2;
        }
        dictionary1.Add("gesture", this.convertToJsonArray(pdfArrayArray));
        this.table.Add("inklist", this.convertToJson(dictionary1));
        break;
      case "Vertices":
        if (!(PdfCrossTable.Dereference(dictionary["Vertices"]) is PdfArray pdfArray3) || pdfArray3.Count <= 0)
          break;
        int count2 = pdfArray3.Count;
        if (count2 % 2 != 0)
          break;
        string str3 = string.Empty;
        for (int index = 0; index < count2 - 1; ++index)
        {
          if (pdfArray3.Elements[index] is PdfNumber element)
            str3 = str3 + this.GetValue((IPdfPrimitive) element) + (index % 2 != 0 ? ";" : ",");
        }
        if (pdfArray3.Elements[count2 - 1] is PdfNumber element1)
          str3 += this.GetValue((IPdfPrimitive) element1);
        if (string.IsNullOrEmpty(str3))
          break;
        this.table.Add("vertices", str3);
        break;
      case "DS":
        if (!dictionary.ContainsKey("DS"))
          break;
        PdfString pdfString2 = PdfCrossTable.Dereference(dictionary["DS"]) as PdfString;
        Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
        if (pdfString2 != null)
        {
          string str4 = (dictionary["DS"] as PdfString).Value.ToString();
          char[] chArray1 = new char[1]{ ';' };
          foreach (string str5 in str4.Split(chArray1))
          {
            char[] chArray2 = new char[1]{ ':' };
            string[] strArray2 = str5.Split(chArray2);
            dictionary2.Add(strArray2[0], strArray2[1]);
          }
        }
        this.table.Add("defaultStyle", this.convertToJson(dictionary2));
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
      case "RC":
        break;
      case "FS":
        break;
      case "MeasurementTypes":
        break;
      case "GroupNesting":
        break;
      case "ITEx":
        break;
      case "Sound":
        break;
      default:
        this.table.Add(key.ToLower(), this.GetValue(primitive));
        break;
    }
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
          str = this.GetValue(pdfArray.Elements[0]);
          for (int index = 1; index < pdfArray.Elements.Count; ++index)
            str = $"{str},{this.GetValue(pdfArray.Elements[index])}";
          break;
        case PdfNumber _:
          str = (primitive as PdfNumber).FloatValue.ToString();
          break;
      }
    }
    return str;
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

  private string convertToJson(Dictionary<string, string> value)
  {
    int num = 0;
    string str = "{";
    foreach (KeyValuePair<string, string> keyValuePair in value)
    {
      if (keyValuePair.Value.StartsWith("{"))
        str = $"{str}\"{XmlConvert.EncodeName(Convert.ToString(keyValuePair.Key))}\":{Convert.ToString(keyValuePair.Value)}";
      else
        str = $"{str}\"{XmlConvert.EncodeName(Convert.ToString(keyValuePair.Key))}\":\"{Convert.ToString(keyValuePair.Value)}\"";
      if (num < value.Count - 1)
        str += ",";
      ++num;
    }
    return str + "}";
  }

  private string convertToJsonArray(PdfArray[] value)
  {
    string str = "[";
    for (int index = 0; index < value.Length; ++index)
    {
      str = $"{str}[{this.GetValue((IPdfPrimitive) value[index])}]";
      if (index < value.Length - 1)
        str += ",";
    }
    return str + "]";
  }
}
