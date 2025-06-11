// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfLoadedAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public abstract class PdfLoadedAnnotation : PdfAnnotation
{
  public int ObjectID;
  private PdfCrossTable m_crossTable;
  private bool m_Changed;
  private int m_defaultIndex;
  private string m_fileName;
  private PdfLoadedPage m_loadedpage;
  private string m_annotationID;
  private PdfAnnotation m_loadedPopup;

  internal event PdfLoadedAnnotation.BeforeNameChangesEventHandler BeforeNameChanges;

  internal bool Changed
  {
    get => this.m_Changed;
    set => this.m_Changed = value;
  }

  internal PdfCrossTable CrossTable
  {
    get => this.m_crossTable;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (CrossTable));
      if (this.m_crossTable == value)
        return;
      this.m_crossTable = value;
    }
  }

  internal virtual PdfAnnotation Popup
  {
    get => this.m_loadedPopup;
    set
    {
      if (value != null && this.ValidPopup(value.Dictionary, true))
      {
        if (this.Page != null && value is PdfPopupAnnotation || value is PdfLoadedPopupAnnotation)
        {
          if (this.m_loadedPopup != null)
            this.RemoveAnnoationFromPage((PdfPageBase) this.Page, this.m_loadedPopup);
          this.m_loadedPopup = value;
          bool changed = this.Dictionary.Changed;
          this.m_loadedPopup.Dictionary.SetProperty("Parent", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this));
          this.Dictionary.SetProperty(nameof (Popup), (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_loadedPopup));
          if (changed || !(value is PdfLoadedPopupAnnotation))
            return;
          this.m_loadedPopup.Dictionary.FreezeChanges((object) this.m_loadedPopup.Dictionary);
          this.Dictionary.FreezeChanges((object) this.Dictionary);
        }
        else
          this.m_loadedPopup = value;
      }
      else
      {
        if (value != null || this.m_loadedPopup == null || this.Page == null)
          return;
        this.RemoveAnnoationFromPage((PdfPageBase) this.Page, this.m_loadedPopup);
        this.m_loadedPopup = value;
      }
    }
  }

  internal PdfLoadedAnnotation(PdfDictionary dictionary, PdfCrossTable crossTable)
  {
    if (dictionary == null)
      throw new ArgumentNullException(nameof (dictionary));
    if (crossTable == null)
      throw new ArgumentNullException(nameof (crossTable));
    this.Dictionary = dictionary;
    this.m_crossTable = crossTable;
    PdfName pdfName = (PdfName) null;
    if (this.Dictionary.ContainsKey("Subtype"))
      pdfName = this.Dictionary.Items[new PdfName("Subtype")] as PdfName;
    if (!(pdfName != (PdfName) null) || !(pdfName.Value == "Circle") && !(pdfName.Value == "Square") && !(pdfName.Value == "Line") && !(pdfName.Value == "Polygon") && !(pdfName.Value == "Ink") && !(pdfName.Value == "FreeText") && !(pdfName.Value == "Highlight") && !(pdfName.Value == "Underline") && !(pdfName.Value == "StrikeOut") && !(pdfName.Value == "PolyLine") && !(pdfName.Value == "Text") && !(pdfName.Value == "Stamp") && !(pdfName.Value == "Redact"))
      return;
    this.CrossTable.Document.Catalog.BeginSave += new SavePdfPrimitiveEventHandler(((PdfAnnotation) this).Dictionary_BeginSave);
    this.CrossTable.Document.Catalog.Modify();
  }

  public PdfLoadedPage Page
  {
    get => this.m_loadedpage;
    set => this.m_loadedpage = value;
  }

  public void SetText(string text)
  {
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    if (text == string.Empty)
      throw new ArgumentException("The text can't be empty");
    if (!(this.Text != text))
      return;
    PdfString pdfString = new PdfString(text);
    this.Dictionary.SetString("T", text);
    this.Changed = true;
  }

  public List<string> GetValues(string name)
  {
    List<string> values = new List<string>();
    PdfName key = new PdfName(name);
    PdfArray pdfArray = this.Dictionary.ContainsKey(key) ? PdfCrossTable.Dereference(this.Dictionary[key]) as PdfArray : throw new PdfException(name + " key is not found");
    PdfString pdfString1 = this.Dictionary[key] as PdfString;
    PdfName pdfName1 = this.Dictionary[key] as PdfName;
    if (pdfArray != null)
    {
      for (int index = 0; index < pdfArray.Count; ++index)
      {
        if (pdfArray[index] is PdfString)
        {
          PdfString pdfString2 = pdfArray[index] as PdfString;
          values.Add(pdfString2.Value);
        }
        else if (pdfArray[index] is PdfNumber)
        {
          PdfNumber pdfNumber = pdfArray[index] as PdfNumber;
          values.Add(pdfNumber.FloatValue.ToString());
        }
        else if ((object) (pdfArray[index] as PdfName) != null)
        {
          PdfName pdfName2 = pdfArray[index] as PdfName;
          values.Add(pdfName2.Value);
        }
      }
    }
    else if (pdfString1 != null)
    {
      values.Add(pdfString1.Value);
    }
    else
    {
      if (!(pdfName1 != (PdfName) null))
        throw new PdfException(name + " key is not found");
      values.Add(pdfName1.Value);
    }
    return values;
  }

  public new void SetValues(string key, string value)
  {
    if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
      return;
    this.Dictionary.SetProperty(key, (IPdfPrimitive) new PdfString(value));
  }

  internal static IPdfPrimitive SearchInParents(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    string value)
  {
    IPdfPrimitive pdfPrimitive = (IPdfPrimitive) null;
    PdfDictionary pdfDictionary = dictionary;
    while (pdfPrimitive == null && pdfDictionary != null)
    {
      if (pdfDictionary.ContainsKey(value))
        pdfPrimitive = crossTable.GetObject(pdfDictionary[value]);
      else
        pdfDictionary = !pdfDictionary.ContainsKey("Parent") ? (PdfDictionary) null : crossTable.GetObject(pdfDictionary["Parent"]) as PdfDictionary;
    }
    return pdfPrimitive;
  }

  internal static IPdfPrimitive GetValue(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    string value,
    bool inheritable)
  {
    IPdfPrimitive pdfPrimitive = (IPdfPrimitive) null;
    if (dictionary.ContainsKey(value))
      pdfPrimitive = crossTable.GetObject(dictionary[value]);
    else if (inheritable)
      pdfPrimitive = PdfLoadedAnnotation.SearchInParents(dictionary, crossTable, value);
    return pdfPrimitive;
  }

  internal PdfDictionary GetWidgetAnnotation(PdfDictionary dictionary, PdfCrossTable crossTable)
  {
    PdfDictionary widgetAnnotation = (PdfDictionary) null;
    if (dictionary.ContainsKey("Kids"))
    {
      PdfArray pdfArray = crossTable.GetObject(dictionary["Kids"]) as PdfArray;
      PdfReference reference = crossTable.GetReference(pdfArray[this.m_defaultIndex]);
      widgetAnnotation = crossTable.GetObject((IPdfPrimitive) reference) as PdfDictionary;
    }
    if (dictionary.ContainsKey("Subtype") && (this.CrossTable.GetObject(dictionary["Subtype"]) as PdfName).Value == "Widget")
      widgetAnnotation = dictionary;
    return widgetAnnotation;
  }

  internal override void ApplyText(string text) => this.SetText(text);

  internal virtual void BeginSave()
  {
  }

  internal void ExportText(Stream stream, ref int objectid)
  {
    bool flag = false;
    pdfArray = (PdfArray) null;
    if (this.Dictionary.ContainsKey("Kids") && this.CrossTable.GetObject(this.Dictionary["Kids"]) is PdfArray pdfArray)
    {
      for (int index = 0; index < pdfArray.Count; ++index)
        flag = flag || pdfArray[index] is PdfLoadedAnnotation;
    }
    PdfString pdfString = PdfLoadedAnnotation.GetValue(this.Dictionary, this.CrossTable, "Contents", true) as PdfString;
    string text1 = "";
    if (pdfString != null)
      text1 = pdfString.Value;
    if (PdfLoadedAnnotation.validateString(text1) && !flag)
      return;
    if (flag)
    {
      for (int index = 0; index < pdfArray.Count; ++index)
      {
        if (pdfArray[index] is PdfLoadedAnnotation loadedAnnotation)
          loadedAnnotation.ExportText(stream, ref objectid);
      }
      this.ObjectID = objectid;
      ++objectid;
      StringBuilder stringBuilder = new StringBuilder();
      byte[] bytes1 = Encoding.GetEncoding("windows-1252").GetBytes(new PdfString(text1)
      {
        Encode = PdfString.ForceEncoding.ASCII
      }.Value);
      stringBuilder.AppendFormat("{0} 0 obj<</T <{1}> /Kids [", (object) this.ObjectID, (object) PdfString.BytesToHex(bytes1));
      for (int index = 0; index < pdfArray.Count; ++index)
      {
        if (pdfArray[index] is PdfLoadedAnnotation loadedAnnotation && loadedAnnotation.ObjectID != 0)
          stringBuilder.AppendFormat("{0} 0 R ", (object) loadedAnnotation.ObjectID);
      }
      stringBuilder.Append("]>>endobj\n");
      byte[] bytes2 = Encoding.GetEncoding("windows-1252").GetBytes(new PdfString(stringBuilder.ToString())
      {
        Encode = PdfString.ForceEncoding.ASCII
      }.Value);
      stream.Write(bytes2, 0, bytes2.Length);
    }
    else
    {
      this.ObjectID = objectid;
      ++objectid;
      string str;
      if (this.GetType().Name == "PdfLoadedCheckBoxField" || this.GetType().Name == "PdfLoadedRadioButtonListField")
        str = "/" + text1;
      else
        str = $"<{PdfString.BytesToHex(Encoding.GetEncoding("windows-1252").GetBytes(new PdfString(text1)
        {
          Encode = PdfString.ForceEncoding.ASCII
        }.Value))}>";
      StringBuilder stringBuilder = new StringBuilder();
      byte[] bytes3 = Encoding.GetEncoding("windows-1252").GetBytes(new PdfString(this.Text)
      {
        Encode = PdfString.ForceEncoding.ASCII
      }.Value);
      stringBuilder.AppendFormat("{0} 0 obj<</T <{1}> /Contents {2} >>endobj\n", (object) this.ObjectID, (object) PdfString.BytesToHex(bytes3), (object) str);
      byte[] bytes4 = Encoding.GetEncoding("windows-1252").GetBytes(new PdfString(stringBuilder.ToString())
      {
        Encode = PdfString.ForceEncoding.ASCII
      }.Value);
      stream.Write(bytes4, 0, bytes4.Length);
    }
  }

  internal static bool validateString(string text1) => text1 == null || text1.Length == 0;

  internal List<string> ExportAnnotation(
    ref PdfWriter writer,
    ref int currentID,
    List<string> annotID,
    int pageIndex,
    bool hasAppearance)
  {
    string str = " 0 obj\r\n";
    string text = "\r\nendobj\r\n";
    PdfDictionary dictionary1 = this.Dictionary;
    this.m_annotationID = currentID.ToString();
    writer.Write($"{(object) currentID}{str}<<");
    System.Collections.Generic.Dictionary<int, IPdfPrimitive> dictionaries = new System.Collections.Generic.Dictionary<int, IPdfPrimitive>();
    List<int> streamReferences = new List<int>();
    annotID.Add(this.m_annotationID);
    dictionary1.Items.Add(new PdfName("Page"), (IPdfPrimitive) new PdfNumber(pageIndex));
    this.GetEntriesInDictionary(ref dictionaries, ref streamReferences, ref currentID, dictionary1, writer, hasAppearance);
    dictionary1.Remove("Page");
    writer.Write(">>" + text);
    while (dictionaries.Count > 0)
    {
      foreach (int key in new List<int>((IEnumerable<int>) dictionaries.Keys))
      {
        if (dictionaries[key] is PdfDictionary)
        {
          if (dictionaries[key] is PdfDictionary dictionary2)
          {
            if (dictionary2.ContainsKey("Type"))
            {
              PdfName pdfName = dictionary2["Type"] as PdfName;
              if (pdfName != (PdfName) null && pdfName.Value == "Annot")
              {
                annotID.Add(key.ToString());
                dictionary2.Items.Add(new PdfName("Page"), (IPdfPrimitive) new PdfNumber(pageIndex));
              }
            }
            writer.Write($"{(object) key}{str}<<");
            this.GetEntriesInDictionary(ref dictionaries, ref streamReferences, ref currentID, dictionary2, writer, hasAppearance);
            if (dictionary2.ContainsKey("Page"))
              dictionary2.Remove("Page");
            writer.Write(">>");
            if (streamReferences.Contains(key))
              this.AppendStream(dictionaries[key] as PdfStream, writer);
            writer.Write(text);
          }
        }
        else if ((object) (dictionaries[key] as PdfName) != null)
        {
          PdfName pdfName = dictionaries[key] as PdfName;
          if (pdfName != (PdfName) null)
            writer.Write(key.ToString() + str + pdfName.ToString() + text);
        }
        else if (dictionaries[key] is PdfArray)
        {
          if (dictionaries[key] is PdfArray array)
          {
            writer.Write(key.ToString() + str);
            this.AppendArrayElements(array, writer, ref currentID, hasAppearance, ref dictionaries, ref streamReferences);
            writer.Write(text);
          }
        }
        else if (dictionaries[key] is PdfBoolean)
        {
          if (dictionaries[key] is PdfBoolean pdfBoolean)
            writer.Write(key.ToString() + str + (pdfBoolean.Value ? (object) "true" : (object) "false") + text);
        }
        else if (dictionaries[key] is PdfString && dictionaries[key] is PdfString pdfString)
          writer.Write($"{(object) key}{str}({this.GetFormattedString(pdfString.Value)}){text}");
        dictionaries.Remove(key);
      }
    }
    ++currentID;
    return annotID;
  }

  private void GetEntriesInDictionary(
    ref System.Collections.Generic.Dictionary<int, IPdfPrimitive> dictionaries,
    ref List<int> streamReferences,
    ref int currentID,
    PdfDictionary dictionary,
    PdfWriter writer,
    bool hasAppearance)
  {
    bool isStream = false;
    foreach (PdfName key in dictionary.Keys)
    {
      if (hasAppearance || !(key.Value == "AP"))
      {
        if (key.Value != "P")
          writer.Write(key.ToString());
        if (key.Value == "Sound" || key.Value == "F" || hasAppearance)
          isStream = true;
        IPdfPrimitive pdfPrimitive = dictionary[key];
        if (pdfPrimitive is PdfString)
          writer.Write($"({this.GetFormattedString((pdfPrimitive as PdfString).Value)})");
        else if ((object) (pdfPrimitive as PdfName) != null)
        {
          writer.Write((pdfPrimitive as PdfName).ToString());
        }
        else
        {
          switch (pdfPrimitive)
          {
            case PdfArray _:
              this.AppendArrayElements(pdfPrimitive as PdfArray, writer, ref currentID, isStream, ref dictionaries, ref streamReferences);
              break;
            case PdfNumber _:
              writer.Write(" " + (object) (pdfPrimitive as PdfNumber).FloatValue);
              break;
            case PdfBoolean _:
              writer.Write(" " + ((pdfPrimitive as PdfBoolean).Value ? "true" : "false"));
              break;
            case PdfDictionary _:
              writer.Write("<<");
              this.GetEntriesInDictionary(ref dictionaries, ref streamReferences, ref currentID, pdfPrimitive as PdfDictionary, writer, hasAppearance);
              writer.Write(">>");
              break;
            default:
              if ((object) (pdfPrimitive as PdfReferenceHolder) != null)
              {
                int num = (this.Page.Document as PdfLoadedDocument).Pages.IndexOf((PdfPageBase) this.Page);
                if (key.Value == "Parent")
                {
                  writer.Write($" {this.m_annotationID} 0 R");
                  writer.Write("/Page " + (object) num);
                  break;
                }
                if (key.Value == "IRT")
                {
                  PdfReferenceHolder pdfReferenceHolder = pdfPrimitive as PdfReferenceHolder;
                  if (pdfReferenceHolder != (PdfReferenceHolder) null && pdfReferenceHolder.Object is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey("NM") && pdfDictionary["NM"] is PdfString pdfString)
                  {
                    writer.Write($"({this.GetFormattedString(pdfString.Value)})");
                    break;
                  }
                  break;
                }
                if (key.Value != "P")
                {
                  PdfReferenceHolder pdfReferenceHolder = pdfPrimitive as PdfReferenceHolder;
                  if (pdfReferenceHolder != (PdfReferenceHolder) null)
                  {
                    ++currentID;
                    writer.Write($" {(object) currentID} 0 R");
                    if (isStream)
                      streamReferences.Add(currentID);
                    dictionaries.Add(currentID, pdfReferenceHolder.Object);
                    break;
                  }
                  break;
                }
                break;
              }
              break;
          }
        }
        isStream = false;
      }
    }
  }

  private void AppendStream(PdfStream stream, PdfWriter writer)
  {
    if (stream == null || stream.Data == null || stream.Data.Length <= 0)
      return;
    writer.Write("stream\r\n");
    writer.Write(stream.Data);
    writer.Write("\r\nendstream");
  }

  private void AppendElement(
    IPdfPrimitive element,
    PdfWriter writer,
    ref int currentID,
    bool isStream,
    ref System.Collections.Generic.Dictionary<int, IPdfPrimitive> dictionaries,
    ref List<int> streamReferences)
  {
    if (element is PdfNumber)
      writer.Write((element as PdfNumber).FloatValue);
    else if ((object) (element as PdfName) != null)
      writer.Write((element as PdfName).ToString());
    else if (element is PdfString)
      writer.Write($"({this.GetFormattedString((element as PdfString).Value)})");
    else if (element is PdfBoolean)
      writer.Write((element as PdfBoolean).Value ? "true" : "false");
    else if ((object) (element as PdfReferenceHolder) != null)
    {
      PdfReferenceHolder pdfReferenceHolder = element as PdfReferenceHolder;
      ++currentID;
      writer.Write(currentID.ToString() + " 0 R");
      if (isStream)
        streamReferences.Add(currentID);
      dictionaries.Add(currentID, pdfReferenceHolder.Object);
    }
    else
    {
      switch (element)
      {
        case PdfArray _:
          this.AppendArrayElements(element as PdfArray, writer, ref currentID, isStream, ref dictionaries, ref streamReferences);
          break;
        case PdfDictionary _:
          writer.Write("<<");
          this.GetEntriesInDictionary(ref dictionaries, ref streamReferences, ref currentID, element as PdfDictionary, writer, isStream);
          writer.Write(">>");
          break;
      }
    }
  }

  private void AppendArrayElements(
    PdfArray array,
    PdfWriter writer,
    ref int currentID,
    bool isStream,
    ref System.Collections.Generic.Dictionary<int, IPdfPrimitive> dictionaries,
    ref List<int> streamReferences)
  {
    writer.Write("[");
    if (array != null && array.Elements.Count > 0)
    {
      int count = array.Elements.Count;
      for (int index = 0; index < count; ++index)
      {
        IPdfPrimitive element = array.Elements[index];
        if (index != 0 && (element is PdfNumber || (object) (element as PdfReferenceHolder) != null || element is PdfBoolean))
          writer.Write(" ");
        this.AppendElement(element, writer, ref currentID, isStream, ref dictionaries, ref streamReferences);
      }
    }
    writer.Write("]");
  }

  private string GetFormattedString(string value)
  {
    string formattedString = "";
    foreach (int num in value)
    {
      if (num == 40 || num == 41)
        formattedString += "\\";
      if (num == 13 || num == 10)
      {
        if (num == 13)
          formattedString += "\\r";
        if (num == 10)
          formattedString += "\\n";
      }
      else
        formattedString += (string) (object) (char) num;
    }
    return formattedString;
  }

  internal bool IsPopup => this.ValidPopup(this.Dictionary, false);

  internal bool ValidPopup(PdfDictionary dictionary, bool isSupportedPopup)
  {
    if (dictionary != null && dictionary.ContainsKey("Subtype"))
    {
      PdfName pdfName1 = PdfCrossTable.Dereference(dictionary["Subtype"]) as PdfName;
      if (pdfName1 != (PdfName) null)
      {
        bool flag = pdfName1.Value == "Popup";
        if (flag && isSupportedPopup && this.Dictionary != null && this.Dictionary.ContainsKey("Subtype"))
        {
          PdfName pdfName2 = PdfCrossTable.Dereference(this.Dictionary["Subtype"]) as PdfName;
          if (pdfName2 != (PdfName) null && (pdfName2.Value == "FreeText" || pdfName2.Value == "Sound" || pdfName2.Value == "FileAttachment"))
            return false;
        }
        return flag;
      }
    }
    return false;
  }

  internal delegate void BeforeNameChangesEventHandler(string name);
}
