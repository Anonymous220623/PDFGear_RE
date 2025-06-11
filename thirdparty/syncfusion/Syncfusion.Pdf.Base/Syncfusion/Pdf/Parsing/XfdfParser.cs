// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.XfdfParser
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class XfdfParser
{
  private PdfLoadedDocument m_document;
  private Stream m_stream;
  private XmlDocument m_xmlDocument;
  private string m_richTextPrefix = "<?xml version=\"1.0\"?>";
  private Dictionary<string, PdfReferenceHolder> m_groupReferences = new Dictionary<string, PdfReferenceHolder>();
  private List<PdfDictionary> m_groupHolders = new List<PdfDictionary>();

  internal XfdfParser(Stream stream, PdfLoadedDocument document)
  {
    this.m_document = document != null ? document : throw new ArgumentNullException(nameof (document));
    this.m_stream = stream;
    this.m_xmlDocument = new XmlDocument();
    this.m_xmlDocument.Load(stream);
  }

  internal void ParseAndImportAnnotationData()
  {
    if (this.m_xmlDocument == null || this.m_xmlDocument.NodeType != XmlNodeType.Document)
      return;
    XmlElement documentElement = this.m_xmlDocument.DocumentElement;
    if (documentElement != null && documentElement.NodeType == XmlNodeType.Element)
    {
      this.CheckXfdf(documentElement);
      XmlNodeList elementsByTagName = documentElement.GetElementsByTagName("Annots".ToLower());
      if (elementsByTagName != null && elementsByTagName.Count > 0)
      {
        foreach (XmlNode xmlNode in elementsByTagName)
        {
          foreach (XmlNode chileElement in xmlNode)
          {
            if (chileElement.NodeType == XmlNodeType.Element)
              this.ParseAnnotationData(chileElement as XmlElement);
          }
        }
      }
    }
    if (this.m_groupHolders.Count > 0)
    {
      foreach (PdfDictionary groupHolder in this.m_groupHolders)
      {
        if (groupHolder["IRT"] is PdfString pdfString && !string.IsNullOrEmpty(pdfString.Value))
        {
          if (this.m_groupReferences.ContainsKey(pdfString.Value))
            groupHolder["IRT"] = (IPdfPrimitive) this.m_groupReferences[pdfString.Value];
          else
            groupHolder.Remove("IRT");
        }
      }
    }
    this.m_groupReferences.Clear();
    this.m_groupHolders.Clear();
  }

  private void ParseAnnotationData(XmlElement chileElement)
  {
    int result = -1;
    if (chileElement == null || !chileElement.HasAttributes || !chileElement.HasAttribute("Page".ToLower()))
      return;
    XmlAttribute attribute = chileElement.Attributes["Page".ToLower()];
    if (attribute == null || string.IsNullOrEmpty(attribute.Value))
      return;
    int.TryParse(attribute.Value, out result);
    if (result < 0 || result >= this.m_document.Pages.Count)
      return;
    (this.m_document.Pages[result] as PdfLoadedPage).importAnnotation = true;
    PdfDictionary annotationData = this.GetAnnotationData(chileElement, result);
    if (annotationData.Count <= 0)
      return;
    PdfReferenceHolder holder = new PdfReferenceHolder((IPdfPrimitive) annotationData);
    if (annotationData.ContainsKey("NM"))
      this.AddReferenceToGroup(holder, annotationData);
    PdfDictionary dictionary = this.m_document.Pages[result].Dictionary;
    if (dictionary == null)
      return;
    if (!dictionary.ContainsKey("Annots"))
      dictionary["Annots"] = (IPdfPrimitive) new PdfArray();
    IPdfPrimitive pdfPrimitive = dictionary["Annots"];
    if (!(((object) (pdfPrimitive as PdfReferenceHolder) != null ? (pdfPrimitive as PdfReferenceHolder).Object : pdfPrimitive) is PdfArray annots))
      return;
    annots.Elements.Add((IPdfPrimitive) holder);
    this.HandlePopUp(annots, holder, annotationData);
    annots.MarkChanged();
  }

  private PdfStream GetStream(XmlElement element)
  {
    PdfStream appearance = new PdfStream();
    if (element.HasChildNodes)
    {
      foreach (XmlElement element1 in (XmlNode) element)
        this.GetAppearance((PdfDictionary) appearance, element1);
    }
    return appearance;
  }

  private PdfDictionary GetDictionary(XmlElement element)
  {
    PdfDictionary appearance = new PdfDictionary();
    if (element.HasChildNodes)
    {
      foreach (XmlElement element1 in (XmlNode) element)
        this.GetAppearance(appearance, element1);
    }
    return appearance;
  }

  private PdfArray GetArray(XmlElement element)
  {
    PdfArray array = new PdfArray();
    if (element.HasChildNodes)
    {
      foreach (XmlElement element1 in (XmlNode) element)
        this.AddArrayElements(array, element1);
    }
    return array;
  }

  private PdfNumber GetFixed(XmlElement element)
  {
    float result;
    return element.HasAttribute("VAL") && float.TryParse(element.Attributes["VAL"].Value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result) ? new PdfNumber(result) : (PdfNumber) null;
  }

  private PdfNumber GetInt(XmlElement element)
  {
    int result;
    return element.HasAttribute("VAL") && int.TryParse(element.Attributes["VAL"].Value, out result) ? new PdfNumber(result) : (PdfNumber) null;
  }

  private PdfString GetString(XmlElement element)
  {
    return element.HasAttribute("VAL") ? new PdfString(element.Attributes["VAL"].Value) : (PdfString) null;
  }

  private PdfName GetName(XmlElement element)
  {
    return element.HasAttribute("VAL") ? new PdfName(element.Attributes["VAL"].Value) : (PdfName) null;
  }

  private PdfBoolean GetBoolean(XmlElement element)
  {
    return element.HasAttribute("VAL") ? new PdfBoolean(element.GetAttribute("VAL").ToLower() == "true") : (PdfBoolean) null;
  }

  private byte[] GetData(XmlElement element)
  {
    if (!string.IsNullOrEmpty(element.InnerText) && element.HasAttribute("MODE") && element.HasAttribute("ENCODING"))
    {
      string attribute1 = element.GetAttribute("MODE");
      string attribute2 = element.GetAttribute("ENCODING");
      if (attribute1 != null && attribute2 != null)
      {
        if (attribute1 == "FILTERED" && attribute2 == "ASCII")
          return Encoding.Default.GetBytes(element.InnerText);
        if (attribute1 == "RAW" && attribute2 == "HEX")
          return this.GetBytes(element.InnerText);
      }
    }
    return (byte[]) null;
  }

  private PdfDictionary GetAppearance(PdfDictionary appearance, XmlElement element)
  {
    if (element != null)
    {
      switch (element.Name)
      {
        case "STREAM":
          PdfStream stream = this.GetStream(element);
          if (stream != null)
          {
            this.AddKey((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) stream), appearance, element);
            break;
          }
          break;
        case "DICT":
          PdfDictionary dictionary = this.GetDictionary(element);
          if (dictionary != null)
          {
            this.AddKey((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) dictionary), appearance, element);
            break;
          }
          break;
        case "ARRAY":
          this.AddKey((IPdfPrimitive) this.GetArray(element), appearance, element);
          break;
        case "FIXED":
          this.AddKey((IPdfPrimitive) this.GetFixed(element), appearance, element);
          break;
        case "INT":
          this.AddKey((IPdfPrimitive) this.GetInt(element), appearance, element);
          break;
        case "STRING":
          this.AddKey((IPdfPrimitive) this.GetString(element), appearance, element);
          break;
        case "NAME":
          this.AddKey((IPdfPrimitive) this.GetName(element), appearance, element);
          break;
        case "BOOL":
          this.AddKey((IPdfPrimitive) this.GetBoolean(element), appearance, element);
          break;
        case "DATA":
          byte[] data = this.GetData(element);
          if (data != null && data.Length > 0 && appearance is PdfStream pdfStream)
          {
            pdfStream.Data = data;
            if (!pdfStream.ContainsKey("Type") && !pdfStream.ContainsKey("Subtype"))
              pdfStream.Decompress();
            bool flag = false;
            if (pdfStream.ContainsKey("Subtype"))
            {
              PdfName pdfName = pdfStream["Subtype"] as PdfName;
              if (pdfName != (PdfName) null && pdfName.Value == "Image")
                flag = true;
            }
            if (flag)
            {
              pdfStream.Compress = false;
              break;
            }
            if (pdfStream.ContainsKey("Length"))
              pdfStream.Remove("Length");
            if (pdfStream.ContainsKey("Filter"))
            {
              pdfStream.Remove("Filter");
              break;
            }
            break;
          }
          break;
      }
    }
    return appearance;
  }

  private void AddBorderStyle(PdfDictionary annotDictionary, XmlElement element)
  {
    PdfDictionary pdfDictionary1 = new PdfDictionary();
    PdfDictionary pdfDictionary2 = new PdfDictionary();
    float result1;
    if (element.HasAttribute("Width".ToLower()) && float.TryParse(element.Attributes["Width".ToLower()].Value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result1))
      pdfDictionary2.SetNumber("W", result1);
    bool flag = true;
    if (element.HasAttribute("style"))
    {
      string name = string.Empty;
      switch (element.Attributes["style"].Value)
      {
        case "dash":
          name = "D";
          break;
        case "solid":
          name = "S";
          break;
        case "bevelled":
          name = "B";
          break;
        case "inset":
          name = "I";
          break;
        case "underline":
          name = "U";
          break;
        case "cloudy":
          name = "C";
          flag = false;
          break;
      }
      if (!string.IsNullOrEmpty(name))
      {
        (flag ? pdfDictionary2 : pdfDictionary1).SetName("S", name);
        if (!flag && element.HasAttribute("intensity"))
        {
          float result2;
          if (float.TryParse(element.Attributes["intensity"].Value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result2))
            pdfDictionary1.SetNumber("I", result2);
        }
        else if (element.HasAttribute("dashes"))
        {
          float[] floatPoints = this.ObtainFloatPoints(element.Attributes["dashes"].Value);
          if (floatPoints != null && floatPoints.Length > 0)
            pdfDictionary2.SetProperty("D", (IPdfPrimitive) new PdfArray(floatPoints));
        }
      }
    }
    if (pdfDictionary1.Count > 0)
    {
      annotDictionary.SetProperty("BE", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary1));
    }
    else
    {
      pdfDictionary1.Clear();
      pdfDictionary1.IsSaving = false;
    }
    if (pdfDictionary2.Count > 0)
    {
      pdfDictionary2.SetProperty("Type", (IPdfPrimitive) new PdfName("Border"));
      annotDictionary.SetProperty("BS", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary2));
    }
    else
    {
      pdfDictionary2.Clear();
      pdfDictionary2.IsSaving = false;
    }
  }

  private void AddMeasureDictionary(PdfDictionary annotDictionary, XmlElement element)
  {
    element.GetElementsByTagName("Measure".ToLower());
    xmlElement = (XmlElement) null;
    XmlElement element1 = (XmlElement) null;
    XmlElement element2 = (XmlElement) null;
    XmlElement element3 = (XmlElement) null;
    foreach (XmlNode childNode in element.ChildNodes)
    {
      if (childNode.NodeType == XmlNodeType.Element && childNode is XmlElement xmlElement)
      {
        if (xmlElement.Name == "measure")
          break;
      }
    }
    PdfDictionary pdfDictionary1 = new PdfDictionary();
    PdfArray pdfArray1 = new PdfArray();
    PdfArray pdfArray2 = new PdfArray();
    PdfArray pdfArray3 = new PdfArray();
    PdfDictionary pdfDictionary2 = new PdfDictionary();
    PdfDictionary pdfDictionary3 = new PdfDictionary();
    PdfDictionary pdfDictionary4 = new PdfDictionary();
    pdfDictionary1.Items.Add(new PdfName("A"), (IPdfPrimitive) pdfArray2);
    pdfDictionary1.Items.Add(new PdfName("D"), (IPdfPrimitive) pdfArray1);
    pdfDictionary1.Items.Add(new PdfName("X"), (IPdfPrimitive) pdfArray3);
    if (xmlElement != null)
    {
      pdfDictionary1.SetName("Type", "Measure");
      if (xmlElement.HasAttribute("rateValue"))
      {
        XmlAttribute attribute = xmlElement.Attributes["rateValue"];
        if (attribute != null)
          pdfDictionary1.SetString("R", attribute.Value);
      }
      foreach (XmlNode childNode in xmlElement.ChildNodes)
      {
        if (childNode.NodeType == XmlNodeType.Element)
        {
          if (childNode is XmlElement xmlElement && xmlElement.Name == "area")
            element1 = xmlElement;
          if (xmlElement != null && xmlElement.Name == "distance")
            element2 = xmlElement;
          if (xmlElement != null && xmlElement.Name == "xformat")
            element3 = xmlElement;
        }
      }
    }
    if (element3 != null)
    {
      this.AddElements(element3, pdfDictionary4);
      pdfArray3.Add((IPdfPrimitive) pdfDictionary4);
    }
    if (element1 != null)
    {
      this.AddElements(element1, pdfDictionary3);
      pdfArray2.Add((IPdfPrimitive) pdfDictionary3);
    }
    if (element2 != null)
    {
      this.AddElements(element2, pdfDictionary2);
      pdfArray1.Add((IPdfPrimitive) pdfDictionary2);
    }
    if (pdfDictionary1.Items.Count <= 0 || !pdfDictionary1.ContainsKey("Type"))
      return;
    annotDictionary.Items.Add(new PdfName("Measure"), (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary1));
  }

  private void ParseInnerElements(PdfDictionary annotDictionary, XmlElement element, int pageIndex)
  {
    if (!element.HasChildNodes)
      return;
    foreach (XmlNode childNode1 in element.ChildNodes)
    {
      if (childNode1.NodeType == XmlNodeType.Element)
      {
        switch (childNode1.Name.ToLower())
        {
          case "popup":
            XmlElement element1 = childNode1 as XmlElement;
            if (element1.HasAttributes)
            {
              PdfDictionary annotationData = this.GetAnnotationData(element1, pageIndex);
              if (annotationData.Count > 0)
              {
                PdfReferenceHolder pdfReferenceHolder = new PdfReferenceHolder((IPdfPrimitive) annotationData);
                annotDictionary.SetProperty("Popup", (IPdfPrimitive) pdfReferenceHolder);
                if (annotationData.ContainsKey("NM"))
                {
                  this.AddReferenceToGroup(pdfReferenceHolder, annotationData);
                  continue;
                }
                continue;
              }
              continue;
            }
            continue;
          case "contents":
            string innerXml = childNode1.InnerXml;
            if (!string.IsNullOrEmpty(innerXml))
            {
              string str = innerXml.Replace("&lt;", "<").Replace("&gt;", ">");
              annotDictionary.SetString("Contents", str);
              continue;
            }
            continue;
          case "contents-richtext":
            if (!string.IsNullOrEmpty(childNode1.InnerXml))
            {
              annotDictionary.SetString("RC", this.m_richTextPrefix + childNode1.InnerXml);
              continue;
            }
            continue;
          case "defaultstyle":
            this.AddString(annotDictionary, "DS", childNode1.InnerText);
            continue;
          case "defaultappearance":
            this.AddString(annotDictionary, "DA", childNode1.InnerText);
            continue;
          case "vertices":
            if (!string.IsNullOrEmpty(childNode1.InnerText))
            {
              string[] strArray = childNode1.InnerText.Split(',', ';');
              if (strArray != null && strArray.Length > 0)
              {
                List<float> collection = new List<float>();
                foreach (string str in strArray)
                  this.AddFloatPoints(collection, str);
                if (collection.Count > 0 && collection.Count % 2 == 0)
                {
                  annotDictionary.SetProperty("Vertices", (IPdfPrimitive) new PdfArray(collection.ToArray()));
                  continue;
                }
                continue;
              }
              continue;
            }
            continue;
          case "appearance":
            this.AddAppearanceData(childNode1, annotDictionary);
            continue;
          case "inklist":
            if (childNode1.HasChildNodes)
            {
              PdfArray primitive = new PdfArray();
              foreach (XmlNode childNode2 in childNode1.ChildNodes)
              {
                if (childNode2.NodeType == XmlNodeType.Element && childNode2 is XmlElement xmlElement && xmlElement.Name.ToLower() == "gesture" && !string.IsNullOrEmpty(xmlElement.InnerText))
                {
                  string[] strArray = xmlElement.InnerText.Split(',', ';');
                  if (strArray != null && strArray.Length > 0)
                  {
                    List<float> collection = new List<float>();
                    foreach (string str in strArray)
                      this.AddFloatPoints(collection, str);
                    if (collection.Count > 0 && collection.Count % 2 == 0)
                      primitive.Add((IPdfPrimitive) new PdfArray(collection.ToArray()));
                    collection.Clear();
                  }
                }
              }
              annotDictionary.SetProperty("InkList", (IPdfPrimitive) primitive);
              continue;
            }
            continue;
          case "data":
            this.AddStreamData(childNode1, annotDictionary, element);
            continue;
          case "imagedata":
            this.AddImageToAppearance(annotDictionary, element);
            continue;
          default:
            continue;
        }
      }
    }
  }

  private void AddAppearanceData(XmlNode childNode, PdfDictionary annotDictionary)
  {
    if (string.IsNullOrEmpty(childNode.InnerText))
      return;
    byte[] buffer = Convert.FromBase64String(childNode.InnerText);
    if (buffer == null || buffer.Length <= 0)
      return;
    Stream inStream = (Stream) new MemoryStream(buffer);
    XmlDocument xmlDocument = new XmlDocument();
    xmlDocument.Load(inStream);
    if (xmlDocument == null || !xmlDocument.HasChildNodes)
      return;
    XmlNodeList childNodes = xmlDocument.ChildNodes;
    if (childNodes.Count != 1 || !(childNodes[0] is XmlElement xmlElement) || !(xmlElement.Name == "DICT") || !xmlElement.HasAttribute("KEY") || !(xmlElement.Attributes["KEY"].Value == "AP") || !xmlElement.HasChildNodes)
      return;
    PdfDictionary pdfDictionary = new PdfDictionary();
    foreach (XmlElement childNode1 in xmlElement.ChildNodes)
      this.GetAppearance(pdfDictionary, childNode1);
    if (pdfDictionary.Count <= 0)
      return;
    annotDictionary.SetProperty("AP", (IPdfPrimitive) pdfDictionary);
  }

  private void AddStreamData(XmlNode childNode, PdfDictionary annotDictionary, XmlElement element)
  {
    if (string.IsNullOrEmpty(childNode.InnerText))
      return;
    byte[] bytes = this.GetBytes(childNode.InnerText);
    if (bytes == null || bytes.Length <= 0 || !annotDictionary.ContainsKey("Subtype"))
      return;
    switch ((annotDictionary["Subtype"] as PdfName).Value)
    {
      case "FileAttachment":
        PdfDictionary dictionary1 = new PdfDictionary();
        dictionary1.SetName("Type", "Filespec");
        this.AddString(dictionary1, element, "file", "F");
        this.AddString(dictionary1, element, "file", "UF");
        PdfStream dictionary2 = new PdfStream();
        PdfDictionary pdfDictionary = new PdfDictionary();
        int result;
        if (element.HasAttribute("Size".ToLower()) && int.TryParse(element.GetAttribute("Size".ToLower()), out result))
        {
          pdfDictionary.SetNumber("Size", result);
          dictionary2.SetNumber("DL", result);
        }
        this.AddString(pdfDictionary, element, "modification", "ModDate");
        this.AddString(pdfDictionary, element, "creation", "CreationDate");
        dictionary2.SetProperty("Params", (IPdfPrimitive) pdfDictionary);
        this.AddString((PdfDictionary) dictionary2, element, "mimetype", "Subtype");
        dictionary2.Data = bytes;
        dictionary2.AddFilter("FlateDecode");
        PdfDictionary primitive = new PdfDictionary();
        primitive.SetProperty("F", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) dictionary2));
        dictionary1.SetProperty("EF", (IPdfPrimitive) primitive);
        annotDictionary.SetProperty("FS", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) dictionary1));
        break;
      case "Sound":
        PdfStream dictionary3 = new PdfStream();
        dictionary3.SetName("Type", "Sound");
        this.AddNumber((PdfDictionary) dictionary3, element, "bits", "B");
        this.AddNumber((PdfDictionary) dictionary3, element, "rate", "R");
        this.AddNumber((PdfDictionary) dictionary3, element, "channels", "C");
        if (element.HasAttribute("Encoding".ToLower()))
        {
          string attribute = element.GetAttribute("Encoding".ToLower());
          if (!string.IsNullOrEmpty(attribute))
            dictionary3.SetName("E", attribute);
        }
        dictionary3.Data = bytes;
        if (element.HasAttribute("Filter".ToLower()))
          dictionary3.AddFilter("FlateDecode");
        annotDictionary.SetProperty("Sound", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) dictionary3));
        break;
    }
  }

  private void AddNumber(
    PdfDictionary dictionary,
    XmlElement element,
    string attributeName,
    string key)
  {
    if (!element.HasAttribute(attributeName))
      return;
    this.AddInt(dictionary, key, element.GetAttribute(attributeName));
  }

  private void AddString(
    PdfDictionary dictionary,
    XmlElement element,
    string attributeName,
    string key)
  {
    if (!element.HasAttribute(attributeName))
      return;
    this.AddString(dictionary, key, element.GetAttribute(attributeName));
  }

  private void AddKey(IPdfPrimitive primitive, PdfDictionary dictionary, XmlElement element)
  {
    if (primitive == null || !element.HasAttribute("KEY"))
      return;
    dictionary.SetProperty(element.GetAttribute("KEY"), primitive);
  }

  private void AddReferenceToGroup(PdfReferenceHolder holder, PdfDictionary dictionary)
  {
    if (!(dictionary["NM"] is PdfString pdfString) || string.IsNullOrEmpty(pdfString.Value))
      return;
    if (this.m_groupReferences.ContainsKey(pdfString.Value))
      this.m_groupReferences[pdfString.Value] = holder;
    else
      this.m_groupReferences.Add(pdfString.Value, holder);
    if (!dictionary.ContainsKey("IRT"))
      return;
    this.m_groupHolders.Add(dictionary);
  }

  private void AddLineEndStyle(XmlElement element, PdfDictionary annotDictionary)
  {
    string empty1 = string.Empty;
    if (element.HasAttribute("head"))
      empty1 = this.MapLineEndingStyle(element.Attributes["head"].Value).ToString();
    string empty2 = string.Empty;
    if (element.HasAttribute("tail"))
      empty2 = this.MapLineEndingStyle(element.Attributes["tail"].Value).ToString();
    if (!string.IsNullOrEmpty(empty1))
    {
      if (!string.IsNullOrEmpty(empty2))
        annotDictionary.SetProperty("LE", (IPdfPrimitive) new PdfArray()
        {
          (IPdfPrimitive) new PdfName(empty1),
          (IPdfPrimitive) new PdfName(empty2)
        });
      else
        annotDictionary.SetName("LE", empty1);
    }
    else
    {
      if (string.IsNullOrEmpty(empty2))
        return;
      annotDictionary.SetName("LE", empty1);
    }
  }

  private void AddAnnotationData(PdfDictionary annotDictionary, XmlElement element, int pageIndex)
  {
    this.AddBorderStyle(annotDictionary, element);
    this.ApplyAttributeValues(annotDictionary, element.Attributes);
    this.ParseInnerElements(annotDictionary, element, pageIndex);
    this.AddMeasureDictionary(annotDictionary, element);
  }

  private void AddLinePoints(List<float> linePoints, string value)
  {
    if (linePoints == null || string.IsNullOrEmpty(value) || !value.Contains(","))
      return;
    string str = value;
    char[] chArray = new char[1]{ ',' };
    foreach (string s in str.Split(chArray))
    {
      float result;
      if (float.TryParse(s, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result))
        linePoints.Add(result);
    }
  }

  private float[] ObtainFloatPoints(string value)
  {
    List<float> floatList = new List<float>();
    if (!string.IsNullOrEmpty(value))
    {
      if (value.Contains(","))
      {
        string str = value;
        char[] chArray = new char[1]{ ',' };
        foreach (string s in str.Split(chArray))
        {
          float result;
          if (float.TryParse(s, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result))
            floatList.Add(result);
        }
      }
      else
      {
        float result;
        if (float.TryParse(value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result))
          floatList.Add(result);
      }
    }
    return floatList.Count <= 0 ? (float[]) null : floatList.ToArray();
  }

  private string GetFormatedString(string value)
  {
    if (!string.IsNullOrEmpty(value))
    {
      value = value.Replace("&lt;", "<");
      value = value.Replace("&gt;", ">");
    }
    return value;
  }

  private void HandlePopUp(
    PdfArray annots,
    PdfReferenceHolder holder,
    PdfDictionary annotDictionary)
  {
    if (!annotDictionary.ContainsKey("Popup"))
      return;
    PdfReferenceHolder annot = annotDictionary["Popup"] as PdfReferenceHolder;
    if (!(annot != (PdfReferenceHolder) null))
      return;
    (annot.Object as PdfDictionary).SetProperty("Parent", (IPdfPrimitive) holder);
    annots.Add((IPdfPrimitive) annot);
  }

  private void CheckXfdf(XmlElement element)
  {
    if (element.Name.ToLower() != "xfdf")
      throw new Exception("Cannot import XFdf file. File format has been corrupted");
  }

  private void AddArrayElements(PdfArray array, XmlElement element)
  {
    if (element == null)
      return;
    switch (element.Name)
    {
      case "STREAM":
        PdfStream stream = this.GetStream(element);
        if (stream == null)
          break;
        this.AddArrayElement(array, (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) stream));
        break;
      case "DICT":
        PdfDictionary dictionary = this.GetDictionary(element);
        if (dictionary == null)
          break;
        this.AddArrayElement(array, (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) dictionary));
        break;
      case "ARRAY":
        PdfArray array1 = this.GetArray(element);
        this.AddArrayElement(array, (IPdfPrimitive) array1);
        break;
      case "FIXED":
        PdfNumber primitive1 = this.GetFixed(element);
        this.AddArrayElement(array, (IPdfPrimitive) primitive1);
        break;
      case "INT":
        PdfNumber primitive2 = this.GetInt(element);
        this.AddArrayElement(array, (IPdfPrimitive) primitive2);
        break;
      case "NAME":
        PdfName name = this.GetName(element);
        this.AddArrayElement(array, (IPdfPrimitive) name);
        break;
      case "BOOL":
        PdfBoolean boolean = this.GetBoolean(element);
        this.AddArrayElement(array, (IPdfPrimitive) boolean);
        break;
    }
  }

  private void ApplyAttributeValues(
    PdfDictionary annotDictionary,
    XmlAttributeCollection collection)
  {
    foreach (XmlAttribute xmlAttribute in (XmlNamedNodeMap) collection)
    {
      string str = xmlAttribute.Value;
      switch (xmlAttribute.Name.ToLower())
      {
        case "state":
          this.AddString(annotDictionary, "State", str);
          continue;
        case "statemodel":
          this.AddString(annotDictionary, "StateModel", str);
          continue;
        case "replytype":
          if (str == "group")
          {
            annotDictionary.SetName("RT", "Group");
            continue;
          }
          continue;
        case "inreplyto":
          this.AddString(annotDictionary, "IRT", str);
          continue;
        case "rect":
          float[] floatPoints = this.ObtainFloatPoints(str);
          if (floatPoints != null && floatPoints.Length == 4)
          {
            annotDictionary.SetProperty("Rect", (IPdfPrimitive) new PdfArray(floatPoints));
            continue;
          }
          continue;
        case "color":
          PdfColor pdfColor1 = new PdfColor(ColorTranslator.FromHtml(str));
          annotDictionary.SetProperty("C", (IPdfPrimitive) pdfColor1.ToArray());
          continue;
        case "interior-color":
          PdfColor pdfColor2 = new PdfColor(ColorTranslator.FromHtml(str));
          annotDictionary.SetProperty("IC", (IPdfPrimitive) pdfColor2.ToArray());
          continue;
        case "date":
          this.AddString(annotDictionary, "M", str);
          continue;
        case "creationdate":
          this.AddString(annotDictionary, "CreationDate", str);
          continue;
        case "name":
          this.AddString(annotDictionary, "NM", str);
          continue;
        case "icon":
          if (!string.IsNullOrEmpty(str))
          {
            annotDictionary.SetName("Name", str);
            continue;
          }
          continue;
        case "subject":
          this.AddString(annotDictionary, "Subj", this.GetFormatedString(str));
          continue;
        case "title":
          this.AddString(annotDictionary, "T", this.GetFormatedString(str));
          continue;
        case "rotation":
          this.AddInt(annotDictionary, "Rotate", str);
          continue;
        case "fringe":
          this.AddFloatPoints(annotDictionary, this.ObtainFloatPoints(str), "RD");
          continue;
        case "it":
          if (!string.IsNullOrEmpty(str))
          {
            annotDictionary.SetName("IT", str);
            continue;
          }
          continue;
        case "leaderlength":
          this.AddFloat(annotDictionary, "LL", str);
          continue;
        case "leaderextend":
          float result1;
          if (float.TryParse(str, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result1))
          {
            annotDictionary.SetNumber("LLE", result1);
            continue;
          }
          continue;
        case "caption":
          if (!string.IsNullOrEmpty(str))
          {
            annotDictionary.SetBoolean("Cap", str.ToLower() == "yes");
            continue;
          }
          continue;
        case "caption-style":
          if (!string.IsNullOrEmpty(str))
          {
            annotDictionary.SetName("CP", str);
            continue;
          }
          continue;
        case "callout":
          this.AddFloatPoints(annotDictionary, this.ObtainFloatPoints(str), "CL");
          continue;
        case "coords":
          this.AddFloatPoints(annotDictionary, this.ObtainFloatPoints(str), "QuadPoints");
          continue;
        case "border":
          this.AddFloatPoints(annotDictionary, this.ObtainFloatPoints(str), "Border");
          continue;
        case "opacity":
          float result2;
          if (float.TryParse(str, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result2))
          {
            annotDictionary.SetNumber("CA", result2);
            continue;
          }
          continue;
        case "flags":
          if (!string.IsNullOrEmpty(str))
          {
            PdfAnnotationFlags pdfAnnotationFlags1 = PdfAnnotationFlags.Default;
            if (str.Contains(","))
            {
              string[] strArray = str.Split(',');
              for (int index = 0; index < strArray.Length; ++index)
              {
                PdfAnnotationFlags pdfAnnotationFlags2 = this.MapAnnotationFlags(strArray[index]);
                if (index == 0)
                  pdfAnnotationFlags1 = pdfAnnotationFlags2;
                else
                  pdfAnnotationFlags1 |= pdfAnnotationFlags2;
              }
            }
            else
              pdfAnnotationFlags1 = this.MapAnnotationFlags(str);
            annotDictionary.SetNumber("F", (int) pdfAnnotationFlags1);
            continue;
          }
          continue;
        case "open":
          if (!string.IsNullOrEmpty(str) && annotDictionary != null)
          {
            annotDictionary.SetBoolean("Open", str == "true" || str == "yes");
            continue;
          }
          continue;
        case "calibrate":
          this.AddString(annotDictionary, "Calibrate", str);
          continue;
        case "customdata":
          this.AddString(annotDictionary, "CustomData", str);
          continue;
        default:
          continue;
      }
    }
  }

  private PdfDictionary GetAnnotationData(XmlElement element, int pageIndex)
  {
    PdfDictionary annotDictionary = new PdfDictionary();
    annotDictionary.SetName("Type", "Annot");
    bool flag = true;
    XmlAttributeCollection attributes = element.Attributes;
    switch (element.Name.ToLower())
    {
      case "line":
        annotDictionary.SetName("Subtype", "Line");
        if (element.HasAttribute("start") && element.HasAttribute("end"))
        {
          List<float> linePoints = new List<float>();
          XmlAttribute xmlAttribute1 = attributes["start"];
          this.AddLinePoints(linePoints, xmlAttribute1.Value);
          XmlAttribute xmlAttribute2 = attributes["end"];
          this.AddLinePoints(linePoints, xmlAttribute2.Value);
          if (linePoints.Count == 4)
            annotDictionary.SetProperty("L", (IPdfPrimitive) new PdfArray(linePoints.ToArray()));
          linePoints.Clear();
        }
        this.AddLineEndStyle(element, annotDictionary);
        break;
      case "circle":
        annotDictionary.SetName("Subtype", "Circle");
        break;
      case "square":
        annotDictionary.SetName("Subtype", "Square");
        break;
      case "polyline":
        annotDictionary.SetName("Subtype", "PolyLine");
        this.AddLineEndStyle(element, annotDictionary);
        break;
      case "polygon":
        annotDictionary.SetName("Subtype", "Polygon");
        this.AddLineEndStyle(element, annotDictionary);
        break;
      case "ink":
        annotDictionary.SetName("Subtype", "Ink");
        break;
      case "popup":
        annotDictionary.SetName("Subtype", "Popup");
        break;
      case "text":
        annotDictionary.SetName("Subtype", "Text");
        break;
      case "freetext":
        annotDictionary.SetName("Subtype", "FreeText");
        this.AddLineEndStyle(element, annotDictionary);
        break;
      case "stamp":
        annotDictionary.SetName("Subtype", "Stamp");
        break;
      case "highlight":
        annotDictionary.SetName("Subtype", "Highlight");
        break;
      case "squiggly":
        annotDictionary.SetName("Subtype", "Squiggly");
        break;
      case "underline":
        annotDictionary.SetName("Subtype", "Underline");
        break;
      case "strikeout":
        annotDictionary.SetName("Subtype", "StrikeOut");
        break;
      case "fileattachment":
        annotDictionary.SetName("Subtype", "FileAttachment");
        break;
      case "sound":
        annotDictionary.SetName("Subtype", "Sound");
        break;
      case "caret":
        annotDictionary.SetName("Subtype", "Caret");
        break;
      default:
        flag = false;
        break;
    }
    if (flag)
      this.AddAnnotationData(annotDictionary, element, pageIndex);
    return annotDictionary;
  }

  private void AddImageToAppearance(PdfDictionary annotDictionary, XmlElement element)
  {
    MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(element.InnerText.Replace("data:image/png;base64,", string.Empty).Replace("data:image/jpg;base64,", string.Empty).Replace("data:image/bmp;base64,", string.Empty)));
    PdfImage image = (PdfImage) new PdfBitmap((Stream) memoryStream);
    memoryStream.Dispose();
    PdfArray pdfArray = PdfCrossTable.Dereference(annotDictionary["Rect"]) as PdfArray;
    float x = 0.0f;
    float y = 0.0f;
    float floatValue1 = (pdfArray[2] as PdfNumber).FloatValue;
    float floatValue2 = (pdfArray[3] as PdfNumber).FloatValue;
    PdfTemplate wrapper = new PdfTemplate(new RectangleF(x, y, floatValue1, floatValue2));
    this.SetMatrix((PdfDictionary) wrapper.m_content);
    wrapper.Graphics.DrawImage(image, x, y, floatValue1, floatValue2);
    PdfReferenceHolder primitive1 = new PdfReferenceHolder((IPdfWrapper) wrapper);
    PdfDictionary primitive2 = new PdfDictionary();
    primitive2.SetProperty("N", (IPdfPrimitive) primitive1);
    annotDictionary.SetProperty("AP", (IPdfPrimitive) primitive2);
  }

  private void SetMatrix(PdfDictionary template)
  {
    float[] numArray = new float[0];
    if (!(PdfCrossTable.Dereference(template["BBox"]) is PdfArray pdfArray))
      return;
    float[] array = new float[6]
    {
      1f,
      0.0f,
      0.0f,
      1f,
      -(pdfArray[0] as PdfNumber).FloatValue,
      -(pdfArray[1] as PdfNumber).FloatValue
    };
    template["Matrix"] = (IPdfPrimitive) new PdfArray(array);
  }

  private PdfLineEndingStyle MapLineEndingStyle(string style)
  {
    PdfLineEndingStyle pdfLineEndingStyle;
    switch (style.ToLower())
    {
      case "butt":
        pdfLineEndingStyle = PdfLineEndingStyle.Butt;
        break;
      case "circle":
        pdfLineEndingStyle = PdfLineEndingStyle.Circle;
        break;
      case "closedarrow":
        pdfLineEndingStyle = PdfLineEndingStyle.ClosedArrow;
        break;
      case "diamond":
        pdfLineEndingStyle = PdfLineEndingStyle.Diamond;
        break;
      case "openarrow":
        pdfLineEndingStyle = PdfLineEndingStyle.OpenArrow;
        break;
      case "rclosedarrow":
        pdfLineEndingStyle = PdfLineEndingStyle.RClosedArrow;
        break;
      case "ropenarrow":
        pdfLineEndingStyle = PdfLineEndingStyle.ROpenArrow;
        break;
      case "slash":
        pdfLineEndingStyle = PdfLineEndingStyle.Slash;
        break;
      case "square":
        pdfLineEndingStyle = PdfLineEndingStyle.Square;
        break;
      default:
        pdfLineEndingStyle = PdfLineEndingStyle.None;
        break;
    }
    return pdfLineEndingStyle;
  }

  private PdfAnnotationFlags MapAnnotationFlags(string flag)
  {
    PdfAnnotationFlags pdfAnnotationFlags;
    switch (flag.ToLower())
    {
      case "hidden":
        pdfAnnotationFlags = PdfAnnotationFlags.Hidden;
        break;
      case "invisible":
        pdfAnnotationFlags = PdfAnnotationFlags.Invisible;
        break;
      case "locked":
        pdfAnnotationFlags = PdfAnnotationFlags.Locked;
        break;
      case "norotate":
        pdfAnnotationFlags = PdfAnnotationFlags.NoRotate;
        break;
      case "noview":
        pdfAnnotationFlags = PdfAnnotationFlags.NoView;
        break;
      case "nozoom":
        pdfAnnotationFlags = PdfAnnotationFlags.NoZoom;
        break;
      case "print":
        pdfAnnotationFlags = PdfAnnotationFlags.Print;
        break;
      case "readonly":
        pdfAnnotationFlags = PdfAnnotationFlags.ReadOnly;
        break;
      case "togglenoview":
        pdfAnnotationFlags = PdfAnnotationFlags.ToggleNoView;
        break;
      default:
        pdfAnnotationFlags = PdfAnnotationFlags.Default;
        break;
    }
    return pdfAnnotationFlags;
  }

  private void AddFloatPoints(PdfDictionary dictionary, float[] points, string key)
  {
    if (points == null || points.Length <= 0)
      return;
    dictionary.SetProperty(key, (IPdfPrimitive) new PdfArray(points));
  }

  private void AddFloatPoints(List<float> collection, string value)
  {
    float result;
    if (!float.TryParse(value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result))
      return;
    collection.Add(result);
  }

  private void AddFloat(PdfDictionary dictionary, string key, string value)
  {
    float result;
    if (!float.TryParse(value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result))
      return;
    dictionary.SetNumber(key, result);
  }

  private void AddInt(PdfDictionary dictionary, string key, string value)
  {
    int result;
    if (!int.TryParse(value, out result))
      return;
    dictionary.SetNumber(key, result);
  }

  private void AddString(PdfDictionary dictionary, string key, string value)
  {
    if (string.IsNullOrEmpty(value))
      return;
    dictionary.SetString(key, value);
  }

  private byte[] GetBytes(string hex) => new PdfString().HexToBytes(hex);

  private void AddArrayElement(PdfArray array, IPdfPrimitive primitive)
  {
    if (primitive == null)
      return;
    array.Add(primitive);
  }

  private void AddElements(XmlElement element, PdfDictionary dictionary)
  {
    float result1;
    if (element.HasAttribute("d") && float.TryParse(element.Attributes["d"].Value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result1))
      dictionary.Items.Add(new PdfName("D"), (IPdfPrimitive) new PdfNumber(result1));
    float result2;
    if (element.HasAttribute("C".ToLower()) && float.TryParse(element.Attributes["C".ToLower()].Value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result2))
      dictionary.Items.Add(new PdfName("C"), (IPdfPrimitive) new PdfNumber(result2));
    if (element.HasAttribute("rt"))
    {
      XmlAttribute attribute = element.Attributes["rt"];
      if (attribute != null)
        dictionary.Items.Add(new PdfName("RT"), (IPdfPrimitive) new PdfString(attribute.Value));
    }
    if (element.HasAttribute("rd"))
    {
      XmlAttribute attribute = element.Attributes["rd"];
      if (attribute != null)
        dictionary.Items.Add(new PdfName("RD"), (IPdfPrimitive) new PdfString(attribute.Value));
    }
    if (element.HasAttribute("SS".ToLower()))
    {
      XmlAttribute attribute = element.Attributes["SS".ToLower()];
      if (attribute != null)
        dictionary.Items.Add(new PdfName("SS"), (IPdfPrimitive) new PdfString(attribute.Value));
    }
    if (element.HasAttribute("U".ToLower()))
    {
      XmlAttribute attribute = element.Attributes["U".ToLower()];
      if (attribute != null)
        dictionary.Items.Add(new PdfName("U"), (IPdfPrimitive) new PdfString(attribute.Value));
    }
    if (element.HasAttribute("F".ToLower()))
    {
      XmlAttribute attribute = element.Attributes["F".ToLower()];
      if (attribute != null)
        dictionary.Items.Add(new PdfName("F"), (IPdfPrimitive) new PdfName(attribute.Value));
    }
    if (!element.HasAttribute("FD".ToLower()))
      return;
    XmlAttribute attribute1 = element.Attributes["FD".ToLower()];
    if (attribute1 == null)
      return;
    dictionary.Items.Add(new PdfName("FD"), (IPdfPrimitive) new PdfBoolean(attribute1.Value == "yes"));
  }
}
