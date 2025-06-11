// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.JsonParser
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
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class JsonParser
{
  private PdfLoadedDocument m_document;
  private string annotation = string.Empty;
  private string beginLineStyle = string.Empty;
  private string endLineStyle = string.Empty;
  private string values = string.Empty;
  private bool isBasicStyle = true;
  private string style = string.Empty;
  private Dictionary<string, string> dataStream = new Dictionary<string, string>();
  private Dictionary<string, string> fields = new Dictionary<string, string>();
  private List<PdfDictionary> m_groupHolders = new List<PdfDictionary>();
  private Dictionary<string, PdfReferenceHolder> m_groupReferences = new Dictionary<string, PdfReferenceHolder>();

  internal JsonParser(Stream stream, PdfLoadedDocument document) => this.m_document = document;

  internal void ImportAnnotationData(Stream stream)
  {
    if (stream == null)
      return;
    string key = (string) null;
    string str = (string) null;
    stream.Position = 0L;
    PdfReader reader = new PdfReader(stream);
    string nextJsonToken1 = reader.GetNextJsonToken();
    reader.Position = 0L;
    for (; nextJsonToken1 != null && nextJsonToken1 != string.Empty; nextJsonToken1 = reader.GetNextJsonToken())
    {
      if (nextJsonToken1 == "type")
      {
        for (; nextJsonToken1 != "}"; nextJsonToken1 = reader.GetNextJsonToken())
        {
          if (nextJsonToken1 != "{" && nextJsonToken1 != "}" && nextJsonToken1 != "\"" && nextJsonToken1 != ",")
          {
            key = nextJsonToken1;
            do
              ;
            while (reader.GetNextJsonToken() != ":");
            string nextJsonToken2 = reader.GetNextJsonToken();
            if (nextJsonToken2 == "{")
            {
              str = this.getJsonObject(nextJsonToken2, key, reader);
            }
            else
            {
              string nextJsonToken3 = reader.GetNextJsonToken();
              string empty = string.Empty;
              for (; nextJsonToken3 != "\""; nextJsonToken3 = reader.GetNextJsonToken())
                empty += nextJsonToken3;
              str = empty;
            }
          }
          if (key != null && str != null)
          {
            this.fields.Add(key, str);
            key = (string) null;
            str = (string) null;
          }
        }
        if (this.fields.Count > 0)
        {
          this.parseAnnotationData(this.fields);
          this.fields.Clear();
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
    stream.Dispose();
  }

  private void parseAnnotationData(Dictionary<string, string> annotData)
  {
    int result = -1;
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    foreach (KeyValuePair<string, string> keyValuePair in annotData)
    {
      if (keyValuePair.Key == "page")
        empty1 = keyValuePair.Value;
      if (keyValuePair.Key == "type")
        empty2 = keyValuePair.Value;
    }
    int.TryParse(empty1, out result);
    if (result < 0 || result >= this.m_document.Pages.Count)
      return;
    (this.m_document.Pages[result] as PdfLoadedPage).importAnnotation = true;
    PdfDictionary annotationData = this.GetAnnotationData(empty2, result, annotData);
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
    if (!(((object) (pdfPrimitive as PdfReferenceHolder) != null ? (pdfPrimitive as PdfReferenceHolder).Object : pdfPrimitive) is PdfArray pdfArray))
      return;
    pdfArray.Elements.Add((IPdfPrimitive) holder);
    pdfArray.MarkChanged();
  }

  private PdfDictionary GetAnnotationData(
    string type,
    int pageindex,
    Dictionary<string, string> key_Value)
  {
    PdfDictionary annotDictionary = new PdfDictionary();
    annotDictionary.SetName("Type", "Annot");
    bool flag = true;
    switch (type.ToLower())
    {
      case "line":
        annotDictionary.SetName("Subtype", "Line");
        break;
      case "circle":
        annotDictionary.SetName("Subtype", "Circle");
        break;
      case "square":
        annotDictionary.SetName("Subtype", "Square");
        break;
      case "polyline":
        annotDictionary.SetName("Subtype", "PolyLine");
        break;
      case "polygon":
        annotDictionary.SetName("Subtype", "Polygon");
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
      case "redact":
        annotDictionary.SetName("Subtype", "Redact");
        this.annotation = "redact";
        break;
      case "caret":
        annotDictionary.SetName("Subtype", "Caret");
        break;
      default:
        flag = false;
        break;
    }
    if (flag)
      this.AddAnnotationData(annotDictionary, key_Value, pageindex);
    return annotDictionary;
  }

  private void AddAnnotationData(
    PdfDictionary annotDictionary,
    Dictionary<string, string> key_Value,
    int index)
  {
    List<float> linePoints = new List<float>();
    PdfDictionary borderEffectDictionary = new PdfDictionary();
    PdfDictionary borderStyleDictionary = new PdfDictionary();
    foreach (KeyValuePair<string, string> keyValuePair in key_Value)
    {
      string str1 = keyValuePair.Value;
      switch (keyValuePair.Key.ToLower())
      {
        case "start":
        case "end":
          this.AddLinePoints(linePoints, keyValuePair.Value);
          if (linePoints.Count == 4)
          {
            annotDictionary.SetProperty("L", (IPdfPrimitive) new PdfArray(linePoints.ToArray()));
            linePoints.Clear();
            linePoints = (List<float>) null;
            continue;
          }
          continue;
        case "state":
          this.AddString(annotDictionary, "State", str1);
          continue;
        case "statemodel":
          this.AddString(annotDictionary, "StateModel", str1);
          continue;
        case "replytype":
          if (str1 == "group")
          {
            annotDictionary.SetName("RT", "Group");
            continue;
          }
          continue;
        case "inreplyto":
          this.AddString(annotDictionary, "IRT", str1);
          continue;
        case "dashes":
        case "width":
        case "intensity":
        case "style":
          this.AddBorderStyle(keyValuePair.Key, keyValuePair.Value, borderEffectDictionary, borderStyleDictionary);
          continue;
        case "rect":
          float[] floatPoints = this.ObtainFloatPoints(str1);
          if (floatPoints != null && floatPoints.Length == 4)
          {
            annotDictionary.SetProperty("Rect", (IPdfPrimitive) new PdfArray(floatPoints));
            continue;
          }
          continue;
        case "color":
          PdfColor pdfColor1 = new PdfColor(ColorTranslator.FromHtml(str1));
          annotDictionary.SetProperty("C", (IPdfPrimitive) pdfColor1.ToArray());
          continue;
        case "oc":
          if (this.annotation == "redact")
          {
            PdfColor pdfColor2 = new PdfColor(ColorTranslator.FromHtml(str1));
            annotDictionary.SetProperty("OC", (IPdfPrimitive) pdfColor2.ToArray());
            continue;
          }
          continue;
        case "interior-color":
          PdfColor pdfColor3 = new PdfColor(ColorTranslator.FromHtml(str1));
          annotDictionary.SetProperty("IC", (IPdfPrimitive) pdfColor3.ToArray());
          continue;
        case "date":
          this.AddString(annotDictionary, "M", str1);
          continue;
        case "creationdate":
          this.AddString(annotDictionary, "CreationDate", str1);
          continue;
        case "name":
          this.AddString(annotDictionary, "NM", str1);
          continue;
        case "icon":
          if (!string.IsNullOrEmpty(str1))
          {
            annotDictionary.SetName("Name", str1);
            continue;
          }
          continue;
        case "subject":
          this.AddString(annotDictionary, "Subj", str1);
          continue;
        case "title":
          this.AddString(annotDictionary, "T", str1);
          continue;
        case "rotation":
          this.AddInt(annotDictionary, "Rotate", str1);
          continue;
        case "fringe":
          this.AddFloatPoints(annotDictionary, this.ObtainFloatPoints(str1), "RD");
          continue;
        case "it":
          if (!string.IsNullOrEmpty(str1))
          {
            annotDictionary.SetName("IT", str1);
            continue;
          }
          continue;
        case "leaderlength":
          this.AddFloat(annotDictionary, "LL", str1);
          continue;
        case "leaderextend":
          float result1;
          if (float.TryParse(str1, out result1))
          {
            annotDictionary.SetNumber("LLE", result1);
            continue;
          }
          continue;
        case "caption":
          if (!string.IsNullOrEmpty(str1))
          {
            annotDictionary.SetBoolean("Cap", str1.ToLower() == "yes");
            continue;
          }
          continue;
        case "caption-style":
          if (!string.IsNullOrEmpty(str1))
          {
            annotDictionary.SetName("CP", str1);
            continue;
          }
          continue;
        case "callout":
          this.AddFloatPoints(annotDictionary, this.ObtainFloatPoints(str1), "CL");
          continue;
        case "coords":
          this.AddFloatPoints(annotDictionary, this.ObtainFloatPoints(str1), "QuadPoints");
          continue;
        case "border":
          this.AddFloatPoints(annotDictionary, this.ObtainFloatPoints(str1), "Border");
          continue;
        case "opacity":
          float result2;
          if (float.TryParse(str1, out result2))
          {
            annotDictionary.SetNumber("CA", result2);
            continue;
          }
          continue;
        case "defaultstyle":
          this.AddString(annotDictionary, "DS", keyValuePair.Value);
          continue;
        case "defaultappearance":
          this.AddString(annotDictionary, "DA", keyValuePair.Value);
          continue;
        case "flags":
          if (!string.IsNullOrEmpty(str1))
          {
            PdfAnnotationFlags pdfAnnotationFlags1 = PdfAnnotationFlags.Default;
            if (str1.Contains(","))
            {
              string[] strArray = str1.Split(',');
              for (int index1 = 0; index1 < strArray.Length; ++index1)
              {
                PdfAnnotationFlags pdfAnnotationFlags2 = this.MapAnnotationFlags(strArray[index1]);
                if (index1 == 0)
                  pdfAnnotationFlags1 = pdfAnnotationFlags2;
                else
                  pdfAnnotationFlags1 |= pdfAnnotationFlags2;
              }
            }
            else
              pdfAnnotationFlags1 = this.MapAnnotationFlags(str1);
            annotDictionary.SetNumber("F", (int) pdfAnnotationFlags1);
            continue;
          }
          continue;
        case "open":
          if (!string.IsNullOrEmpty(str1) && annotDictionary != null)
          {
            annotDictionary.SetBoolean("Open", str1 == "true" || str1 == "yes");
            continue;
          }
          continue;
        case "repeat":
          if (!string.IsNullOrEmpty(str1) && annotDictionary != null)
          {
            annotDictionary.SetBoolean("Repeat", str1 == "true" || str1 == "yes");
            continue;
          }
          continue;
        case "overlaytext":
          annotDictionary.SetString("OverlayText", str1);
          continue;
        case "contents":
          string str2 = keyValuePair.Value;
          if (!string.IsNullOrEmpty(str2))
          {
            annotDictionary.SetString("Contents", str2);
            continue;
          }
          continue;
        case "q":
          int result3;
          if (int.TryParse(str1, out result3))
          {
            annotDictionary.SetNumber("Q", result3);
            continue;
          }
          continue;
        case "inklist":
          PdfArray primitive = new PdfArray();
          string str3 = keyValuePair.Value.Replace("gesture:[", "").Replace("[", "");
          string[] separator = new string[2]{ "]", "];" };
          foreach (string str4 in str3.Split(separator, StringSplitOptions.None))
          {
            char[] chArray = new char[2]{ ',', ';' };
            string[] strArray = str4.Split(chArray);
            if (strArray != null && strArray.Length > 0)
            {
              List<float> collection = new List<float>();
              foreach (string str5 in strArray)
                this.AddFloatPoints(collection, str5);
              if (collection.Count > 0 && collection.Count % 2 == 0)
                primitive.Add((IPdfPrimitive) new PdfArray(collection.ToArray()));
              collection.Clear();
            }
          }
          annotDictionary.SetProperty("InkList", (IPdfPrimitive) primitive);
          continue;
        case "head":
          this.beginLineStyle = this.MapLineEndingStyle(keyValuePair.Value).ToString();
          continue;
        case "tail":
          this.endLineStyle = this.MapLineEndingStyle(keyValuePair.Value).ToString();
          continue;
        case "creation":
        case "modification":
        case "file":
        case "bits":
        case "channels":
        case "encoding":
        case "rate":
        case "length":
        case "filter":
        case "mode":
        case "size":
          this.dataStream.Add(keyValuePair.Key, keyValuePair.Value);
          continue;
        case "data":
          this.values = keyValuePair.Value;
          continue;
        case "vertices":
          if (!string.IsNullOrEmpty(keyValuePair.Value))
          {
            string[] strArray = str1.Split(',', ';');
            if (strArray != null && strArray.Length > 0)
            {
              List<float> collection = new List<float>();
              foreach (string str6 in strArray)
                this.AddFloatPoints(collection, str6);
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
        default:
          continue;
      }
    }
    if (!string.IsNullOrEmpty(this.beginLineStyle))
    {
      if (!string.IsNullOrEmpty(this.endLineStyle))
        annotDictionary.SetProperty("LE", (IPdfPrimitive) new PdfArray()
        {
          (IPdfPrimitive) new PdfName(this.beginLineStyle),
          (IPdfPrimitive) new PdfName(this.endLineStyle)
        });
      else
        annotDictionary.SetName("LE", this.beginLineStyle);
    }
    else if (!string.IsNullOrEmpty(this.endLineStyle))
      annotDictionary.SetName("LE", this.beginLineStyle);
    if (borderStyleDictionary.Count > 0)
    {
      borderStyleDictionary.SetProperty("Type", (IPdfPrimitive) new PdfName("Border"));
      annotDictionary.SetProperty("BS", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) borderStyleDictionary));
    }
    if (borderEffectDictionary.Count > 0)
      annotDictionary.SetProperty("BE", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) borderEffectDictionary));
    this.AddStreamData(this.dataStream, annotDictionary, this.values);
  }

  private void AddStreamData(
    Dictionary<string, string> dataValues,
    PdfDictionary annotDictionary,
    string values)
  {
    string str = (annotDictionary["Subtype"] as PdfName).Value;
    byte[] bytes = this.GetBytes(values);
    if (bytes == null || bytes.Length <= 0)
      return;
    switch (str)
    {
      case "sound":
        PdfStream dictionary1 = new PdfStream();
        dictionary1.SetName("Type", "Sound");
        foreach (KeyValuePair<string, string> dataValue in dataValues)
        {
          switch (dataValue.Key)
          {
            case "bits":
            case "rate":
            case "channels":
              this.AddInt((PdfDictionary) dictionary1, dataValue.Key, dataValue.Value);
              continue;
            case "encoding":
              if (!string.IsNullOrEmpty(dataValue.Value))
              {
                dictionary1.SetName("E", dataValue.Value);
                continue;
              }
              continue;
            case "filter":
              dictionary1.AddFilter("FlateDecode");
              continue;
            default:
              continue;
          }
        }
        dictionary1.Data = bytes;
        annotDictionary.SetProperty("Sound", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) dictionary1));
        break;
      case "FileAttachment":
        PdfDictionary dictionary2 = new PdfDictionary();
        PdfStream pdfStream = new PdfStream();
        PdfDictionary pdfDictionary = new PdfDictionary();
        dictionary2.SetName("Type", "Filespec");
        foreach (KeyValuePair<string, string> dataValue in dataValues)
        {
          switch (dataValue.Key)
          {
            case "file":
              this.AddString(dictionary2, "F", dataValue.Value);
              this.AddString(dictionary2, "UF", dataValue.Value);
              continue;
            case "size":
              int result;
              if (int.TryParse(dataValue.Value, out result))
              {
                pdfDictionary.SetNumber("Size", result);
                pdfStream.SetNumber("DL", result);
                continue;
              }
              continue;
            case "creation":
              this.AddString(pdfDictionary, "creation", "CreationDate");
              continue;
            case "modification":
              this.AddString(pdfDictionary, "modification", "ModDate");
              continue;
            default:
              continue;
          }
        }
        pdfStream.SetProperty("Params", (IPdfPrimitive) pdfDictionary);
        pdfStream.Data = bytes;
        pdfStream.AddFilter("FlateDecode");
        PdfDictionary primitive = new PdfDictionary();
        primitive.SetProperty("F", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfStream));
        dictionary2.SetProperty("EF", (IPdfPrimitive) primitive);
        annotDictionary.SetProperty("FS", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) dictionary2));
        break;
    }
  }

  private void AddInt(PdfDictionary dictionary, string key, string value)
  {
    int result;
    if (!int.TryParse(value, out result))
      return;
    dictionary.SetNumber(key, result);
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
          if (float.TryParse(s, out result))
            floatList.Add(result);
        }
      }
      else
      {
        float result;
        if (float.TryParse(value, out result))
          floatList.Add(result);
      }
    }
    return floatList.Count <= 0 ? (float[]) null : floatList.ToArray();
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
    if (!float.TryParse(value, out result))
      return;
    collection.Add(result);
  }

  private void AddFloat(PdfDictionary dictionary, string key, string value)
  {
    float result;
    if (!float.TryParse(value, out result))
      return;
    dictionary.SetNumber(key, result);
  }

  private void AddString(PdfDictionary dictionary, string key, string value)
  {
    if (string.IsNullOrEmpty(value))
      return;
    dictionary.SetString(key, value);
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
      if (float.TryParse(s, out result))
        linePoints.Add(result);
    }
  }

  private byte[] GetBytes(string hex) => new PdfString().HexToBytes(hex);

  private void AddArrayElement(PdfArray array, IPdfPrimitive primitive)
  {
    if (primitive == null)
      return;
    array.Add(primitive);
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

  private void AddBorderStyle(
    string key,
    string value,
    PdfDictionary borderEffectDictionary,
    PdfDictionary borderStyleDictionary)
  {
    switch (value)
    {
      case "dash":
        this.style = "D";
        break;
      case "solid":
        this.style = "S";
        break;
      case "bevelled":
        this.style = "B";
        break;
      case "inset":
        this.style = "I";
        break;
      case "underline":
        this.style = "U";
        break;
      case "cloudy":
        this.style = "C";
        this.isBasicStyle = false;
        break;
    }
    float result1;
    if (key == "width" && float.TryParse(value, out result1))
      borderStyleDictionary.SetNumber("W", result1);
    float result2;
    if (key == "intensity" && float.TryParse(value, out result2))
      borderEffectDictionary.SetNumber("I", result2);
    if (!string.IsNullOrEmpty(this.style))
      (this.isBasicStyle ? borderStyleDictionary : borderEffectDictionary).SetName("S", this.style);
    if (!(key == "dashes"))
      return;
    float[] floatPoints = this.ObtainFloatPoints(value);
    if (floatPoints == null || floatPoints.Length <= 0)
      return;
    borderStyleDictionary.SetProperty("D", (IPdfPrimitive) new PdfArray(floatPoints));
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

  private string getJsonObject(string token, string key, PdfReader reader)
  {
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    string jsonObject;
    if (key == "rect")
    {
      while (token != "}")
      {
        while (token != ":")
          token = reader.GetNextJsonToken();
        token = reader.GetNextJsonToken();
        token = reader.GetNextJsonToken();
        empty1 += token;
        token = reader.GetNextJsonToken();
        token = reader.GetNextJsonToken();
        if (token == ",")
        {
          empty1 += token;
          token = reader.GetNextJsonToken();
        }
      }
      jsonObject = empty1;
    }
    else
    {
      for (token = reader.GetNextJsonToken(); token != "}"; token = reader.GetNextJsonToken())
      {
        if (token != "\"")
        {
          if (token == ",")
            token = ";";
          empty1 += token;
        }
      }
      jsonObject = empty1;
    }
    return jsonObject;
  }
}
