// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.PdfLoadedField
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

public abstract class PdfLoadedField : PdfField
{
  public int ObjectID;
  private PdfCrossTable m_crossTable;
  private bool m_Changed;
  private bool m_fieldChanged;
  private int m_defaultIndex;
  private string m_name;
  private PdfPageBase m_page;
  private PdfLoadedForm m_form;
  internal bool ExportEmptyField;
  internal PdfReferenceHolder m_requiredReference;
  internal bool isAcrobat;

  internal event PdfLoadedField.BeforeNameChangesEventHandler BeforeNameChanges;

  public override string Name
  {
    get
    {
      this.m_name = this.GetFieldName();
      return this.m_name;
    }
  }

  public override string MappingName
  {
    get
    {
      string mappingName = base.MappingName;
      if ((mappingName == null || mappingName == string.Empty) && PdfLoadedField.GetValue(this.Dictionary, this.m_crossTable, "TM", false) is PdfString pdfString)
        mappingName = pdfString.Value;
      return mappingName;
    }
    set
    {
      base.MappingName = value;
      this.Changed = true;
    }
  }

  public override string ToolTip
  {
    get
    {
      PdfString pdfString = PdfLoadedField.GetValue(this.Dictionary, this.m_crossTable, "TU", false) as PdfString;
      string toolTip = (string) null;
      if (pdfString != null)
        toolTip = pdfString.Value;
      return toolTip;
    }
    set
    {
      base.ToolTip = value;
      this.Changed = true;
    }
  }

  public override PdfPageBase Page
  {
    get
    {
      if (this.m_page == null)
        this.m_page = this.GetLoadedPage();
      else if (this.m_page != null && this.m_page is PdfLoadedPage && (this.Changed || this.Form != null && this.Form.Flatten || this.Flatten))
        this.m_page = this.GetLoadedPage();
      return this.m_page;
    }
    internal set => this.m_page = value;
  }

  public override bool ReadOnly
  {
    get => (FieldFlags.ReadOnly & this.Flags) != FieldFlags.Default || this.Form.ReadOnly;
    set
    {
      if (value || this.Form.ReadOnly)
      {
        this.Flags |= FieldFlags.ReadOnly;
      }
      else
      {
        if (this.Flags == FieldFlags.ReadOnly)
        {
          PdfLoadedField pdfLoadedField = this;
          pdfLoadedField.Flags = pdfLoadedField.Flags;
        }
        this.Flags &= ~FieldFlags.ReadOnly;
      }
    }
  }

  public override bool Required
  {
    get => (FieldFlags.Required & this.Flags) != FieldFlags.Default;
    set
    {
      if (value)
        this.Flags |= FieldFlags.Required;
      else
        this.Flags &= ~FieldFlags.Required;
    }
  }

  public override bool Export
  {
    get => (FieldFlags.NoExport & this.Flags) == FieldFlags.Default;
    set
    {
      if (value)
        this.Flags &= ~FieldFlags.NoExport;
      else
        this.Flags |= FieldFlags.NoExport;
    }
  }

  internal override FieldFlags Flags
  {
    get
    {
      FieldFlags flags = base.Flags;
      if (flags == FieldFlags.Default && PdfLoadedField.GetValue(this.Dictionary, this.m_crossTable, "Ff", true) is PdfNumber pdfNumber)
        flags = (FieldFlags) pdfNumber.IntValue;
      return flags;
    }
    set
    {
      base.Flags = value;
      this.Changed = true;
    }
  }

  internal string ActualFieldName
  {
    get
    {
      string actualFieldName = (string) null;
      if (PdfLoadedField.GetValue(this.Dictionary, this.m_crossTable, "T", false) is PdfString pdfString)
        actualFieldName = pdfString.Value;
      return actualFieldName;
    }
  }

  public new PdfForm Form => this.m_form != null ? (PdfForm) this.m_form : base.Form;

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

  internal PdfDictionary Parent
  {
    get
    {
      PdfDictionary parent = (PdfDictionary) null;
      if (this.Dictionary.ContainsKey(nameof (Parent)))
        parent = this.m_crossTable.GetObject(this.Dictionary[nameof (Parent)]) as PdfDictionary;
      return parent;
    }
  }

  internal bool Changed
  {
    get => this.m_Changed;
    set => this.m_Changed = value;
  }

  internal bool FieldChanged
  {
    get => this.m_fieldChanged;
    set => this.m_fieldChanged = value;
  }

  internal int DefaultIndex
  {
    get => this.m_defaultIndex;
    set => this.m_defaultIndex = value;
  }

  internal PdfLoadedField(PdfDictionary dictionary, PdfCrossTable crossTable)
  {
    if (dictionary == null)
      throw new ArgumentNullException(nameof (dictionary));
    if (crossTable == null)
      throw new ArgumentNullException(nameof (crossTable));
    this.Dictionary = dictionary;
    this.m_crossTable = crossTable;
  }

  public void SetName(string name)
  {
    if (name == null)
      throw new ArgumentNullException(nameof (name));
    if (name == string.Empty)
      throw new ArgumentException("The name can't be empty");
    if (this.Name == null || !(this.Name != name))
      return;
    string[] strArray = this.Name.Split('.');
    int length = strArray.Length;
    if (strArray[length - 1] == name)
      return;
    PdfString primitive = new PdfString(name);
    if (this.m_form != null)
      this.BeforeNameChanges(name);
    this.Dictionary.SetProperty("T", (IPdfPrimitive) primitive);
    this.Changed = true;
  }

  internal static IPdfPrimitive SearchInParents(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    string value)
  {
    IPdfPrimitive pdfPrimitive = (IPdfPrimitive) null;
    pdfDictionary = dictionary;
    while (pdfPrimitive == null && pdfDictionary != null)
    {
      if (pdfDictionary.ContainsKey(value))
        pdfPrimitive = crossTable.GetObject(pdfDictionary[value]);
      else if (pdfDictionary.ContainsKey("Parent"))
      {
        if (crossTable.GetObject(pdfDictionary["Parent"]) is PdfDictionary pdfDictionary && !pdfDictionary.ContainsKey(value) && value == "Ff")
          break;
      }
      else
        pdfDictionary = (PdfDictionary) null;
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
      pdfPrimitive = PdfLoadedField.SearchInParents(dictionary, crossTable, value);
    if (pdfPrimitive != null && pdfPrimitive is PdfString)
    {
      bool flag = false;
      if (crossTable.Document != null && crossTable.Document is PdfLoadedDocument)
        flag = (crossTable.Document as PdfLoadedDocument).WasEncrypted;
      if (flag)
      {
        PdfReference reference = crossTable.GetReference((IPdfPrimitive) dictionary);
        if (reference != (PdfReference) null)
          (pdfPrimitive as PdfString).Decrypt(crossTable.Encryptor, reference.ObjNum);
      }
    }
    return pdfPrimitive;
  }

  internal PdfDictionary GetWidgetAnnotation(PdfDictionary dictionary, PdfCrossTable crossTable)
  {
    PdfDictionary widgetAnnotation = (PdfDictionary) null;
    if (dictionary.ContainsKey("Kids"))
    {
      PdfArray pdfArray = crossTable.GetObject(dictionary["Kids"]) as PdfArray;
      if (pdfArray.Count > 0)
      {
        PdfReference reference = crossTable.GetReference(pdfArray[this.m_defaultIndex]);
        widgetAnnotation = crossTable.GetObject((IPdfPrimitive) reference) as PdfDictionary;
      }
    }
    else if (dictionary.ContainsKey("Subtype") && (this.CrossTable.GetObject(dictionary["Subtype"]) as PdfName).Value == "Widget")
      widgetAnnotation = dictionary;
    if (widgetAnnotation == null)
      widgetAnnotation = dictionary;
    return widgetAnnotation;
  }

  internal List<PdfDictionary> GetWidgetAnnotations(
    PdfDictionary dictionary,
    PdfCrossTable crossTable)
  {
    List<PdfDictionary> widgetAnnotations = new List<PdfDictionary>();
    if (dictionary.ContainsKey("Kids"))
    {
      if (crossTable.GetObject(dictionary["Kids"]) is PdfArray pdfArray && pdfArray.Count > 0)
      {
        foreach (IPdfPrimitive pdfPrimitive in pdfArray)
        {
          PdfReference reference = crossTable.GetReference(pdfPrimitive);
          if (reference != (PdfReference) null && crossTable.GetObject((IPdfPrimitive) reference) is PdfDictionary pdfDictionary)
            widgetAnnotations.Add(pdfDictionary);
        }
      }
    }
    else if (dictionary.ContainsKey("Subtype"))
    {
      PdfName pdfName = this.CrossTable.GetObject(dictionary["Subtype"]) as PdfName;
      if (pdfName != (PdfName) null && pdfName.Value == "Widget")
        widgetAnnotations.Add(dictionary);
    }
    if (widgetAnnotations.Count == 0)
      widgetAnnotations.Add(dictionary);
    return widgetAnnotations;
  }

  internal PdfHighlightMode GetHighLight(PdfDictionary dictionary, PdfCrossTable crossTable)
  {
    PdfName pdfName = (PdfName) null;
    if (dictionary.ContainsKey("Kids"))
    {
      PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(dictionary, crossTable);
      if (widgetAnnotation.ContainsKey("H"))
        pdfName = crossTable.GetObject(widgetAnnotation["H"]) as PdfName;
    }
    else if (dictionary.ContainsKey("H"))
      pdfName = crossTable.GetObject(dictionary["H"]) as PdfName;
    PdfHighlightMode highLight = PdfHighlightMode.NoHighlighting;
    if (pdfName != (PdfName) null)
    {
      switch (pdfName.Value)
      {
        case "I":
          highLight = PdfHighlightMode.Invert;
          break;
        case "O":
          highLight = PdfHighlightMode.Outline;
          break;
        case "P":
          highLight = PdfHighlightMode.Push;
          break;
      }
    }
    return highLight;
  }

  internal abstract override void Draw();

  internal abstract PdfLoadedFieldItem CreateLoadedItem(PdfDictionary dictionary);

  internal override void ApplyName(string name) => this.SetName(name);

  internal virtual void BeginSave()
  {
  }

  private PdfPageBase GetLoadedPage()
  {
    PdfPageBase loadedPage = base.Page;
    if (loadedPage == null)
    {
      PdfLoadedDocument document = this.CrossTable.Document as PdfLoadedDocument;
      PdfDictionary widget = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable) ?? this.Dictionary;
      if (widget.ContainsKey("P") && !(PdfCrossTable.Dereference(widget["P"]) is PdfNull))
      {
        if (this.CrossTable.GetObject(widget["P"]) is PdfDictionary dic)
          loadedPage = document.Pages.GetPage(dic);
      }
      else
        loadedPage = this.FindWidgetPageReference(widget, document);
      if (loadedPage == null && this.Dictionary.ContainsKey("Kids") && this.CrossTable.GetObject(this.Dictionary["Kids"]) is PdfArray pdfArray)
      {
        for (int index = 0; index < pdfArray.Count; ++index)
          loadedPage = this.FindWidgetPageReference(PdfCrossTable.Dereference(pdfArray[index]) as PdfDictionary, document);
      }
    }
    return loadedPage;
  }

  private PdfPageBase FindWidgetPageReference(PdfDictionary widget, PdfLoadedDocument doc)
  {
    PdfPageBase widgetPageReference = (PdfPageBase) null;
    PdfReference reference = this.CrossTable.GetReference((IPdfPrimitive) widget);
    foreach (PdfPageBase page in doc.Pages)
    {
      PdfArray annotations = page.ObtainAnnotations();
      if (annotations != null)
      {
        for (int index = 0; index < annotations.Count; ++index)
        {
          PdfReferenceHolder pdfReferenceHolder = annotations[index] as PdfReferenceHolder;
          if (pdfReferenceHolder.Reference == reference)
          {
            widgetPageReference = page;
            return widgetPageReference;
          }
          if (this.m_requiredReference != (PdfReferenceHolder) null && this.m_requiredReference.Reference == pdfReferenceHolder.Reference)
          {
            widgetPageReference = page;
            return widgetPageReference;
          }
        }
      }
    }
    return widgetPageReference;
  }

  internal void ExportField(XmlTextWriter textWriter)
  {
    PdfName pdfName1 = PdfLoadedField.GetValue(this.Dictionary, this.CrossTable, "FT", true) as PdfName;
    PdfDictionary font = (PdfDictionary) null;
    string name = this.Name;
    this.GetEncodedFontDictionary(this.Dictionary, out font);
    if (font != null)
      name = this.UpdateEncodedValue(name, font);
    if (!(pdfName1 != (PdfName) null))
      return;
    switch (pdfName1.Value)
    {
      case "Tx":
        if (!(PdfLoadedField.GetValue(this.Dictionary, this.CrossTable, "V", true) is PdfString pdfString1) && !this.ExportEmptyField)
          break;
        textWriter.WriteStartElement(XmlConvert.EncodeName(name), "");
        if (pdfString1 != null)
        {
          pdfString1.Value = this.UpdateEncodedValue(pdfString1.Value, font);
          textWriter.WriteString(pdfString1.Value);
        }
        else if (this.ExportEmptyField)
          textWriter.WriteString("");
        textWriter.WriteEndElement();
        break;
      case "Ch":
        IPdfPrimitive buttonFieldPrimitive1 = PdfLoadedField.GetValue(this.Dictionary, this.CrossTable, "V", true);
        string text = (string) null;
        if (buttonFieldPrimitive1 != null)
          text = this.GetExportValue(this, buttonFieldPrimitive1);
        if (buttonFieldPrimitive1 == null && this.Dictionary.ContainsKey("I"))
          text = !(this.GetType().Name == "PdfLoadedListBoxField") ? (this as PdfLoadedComboBoxField).SelectedValue : (this as PdfLoadedListBoxField).SelectedValue[0];
        if (string.IsNullOrEmpty(text) && !this.ExportEmptyField)
          break;
        textWriter.WriteStartElement(XmlConvert.EncodeName(this.Name), "");
        if (!string.IsNullOrEmpty(text))
          textWriter.WriteString(text);
        else if (this.ExportEmptyField)
          textWriter.WriteString("");
        textWriter.WriteEndElement();
        break;
      case "Btn":
        IPdfPrimitive buttonFieldPrimitive2 = PdfLoadedField.GetValue(this.Dictionary, this.CrossTable, "V", true);
        PdfLoadedRadioButtonListField radioButtonListField = (PdfLoadedRadioButtonListField) null;
        if (buttonFieldPrimitive2 != null)
        {
          string exportValue = this.GetExportValue(this, buttonFieldPrimitive2);
          if (this is PdfLoadedRadioButtonListField)
            radioButtonListField = this as PdfLoadedRadioButtonListField;
          if (radioButtonListField != null && radioButtonListField.SelectedIndex == -1)
          {
            textWriter.WriteStartElement(XmlConvert.EncodeName(this.Name), "");
            if (this.ExportEmptyField)
              textWriter.WriteString("");
            else
              textWriter.WriteString("Off");
            textWriter.WriteEndElement();
            break;
          }
          if (this.Dictionary != null && this.Dictionary.ContainsKey("Opt"))
          {
            PdfArray pdfArray = PdfCrossTable.Dereference(this.Dictionary["Opt"]) as PdfArray;
            int result = 0;
            try
            {
              int.TryParse(exportValue, out result);
              if (pdfArray != null)
              {
                PdfString pdfString2 = radioButtonListField == null ? PdfCrossTable.Dereference(pdfArray[result]) as PdfString : PdfCrossTable.Dereference(pdfArray[radioButtonListField.SelectedIndex]) as PdfString;
                if (pdfString2 != null)
                  exportValue = pdfString2.Value;
              }
            }
            catch
            {
            }
            if (string.IsNullOrEmpty(exportValue) && !this.ExportEmptyField)
              break;
            textWriter.WriteStartElement(XmlConvert.EncodeName(this.Name), "");
            if (exportValue != null)
              textWriter.WriteString(exportValue);
            else if (this.ExportEmptyField)
              textWriter.WriteString("");
            textWriter.WriteEndElement();
            break;
          }
          if (!string.IsNullOrEmpty(exportValue) || this.ExportEmptyField)
          {
            textWriter.WriteStartElement(XmlConvert.EncodeName(this.Name), "");
            if (exportValue != null)
              textWriter.WriteString(exportValue);
            else if (this.ExportEmptyField)
              textWriter.WriteString("");
            textWriter.WriteEndElement();
            break;
          }
          if (!(this is PdfLoadedCheckBoxField))
            break;
          textWriter.WriteStartElement(XmlConvert.EncodeName(this.Name), "");
          if (this.ExportEmptyField)
            textWriter.WriteString("");
          else
            textWriter.WriteString("Off");
          textWriter.WriteEndElement();
          break;
        }
        if (this is PdfLoadedRadioButtonListField)
        {
          textWriter.WriteStartElement(XmlConvert.EncodeName(this.Name), "");
          textWriter.WriteString(this.GetAppearanceStateValue(this));
          textWriter.WriteEndElement();
          break;
        }
        PdfName pdfName2 = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable)["AS"] as PdfName;
        if (!(pdfName2 != (PdfName) null) && !this.ExportEmptyField)
          break;
        textWriter.WriteStartElement(XmlConvert.EncodeName(this.Name), "");
        if (pdfName2 != (PdfName) null)
          textWriter.WriteString(pdfName2.Value);
        else
          textWriter.WriteString("");
        textWriter.WriteEndElement();
        break;
    }
  }

  internal void ExportField(
    XmlTextWriter textWriter,
    System.Collections.Generic.Dictionary<object, object> table,
    string uniquekey)
  {
    PdfName pdfName1 = PdfLoadedField.GetValue(this.Dictionary, this.CrossTable, "FT", true) as PdfName;
    bool flag = false;
    string fieldName = this.Name;
    PdfDictionary font = (PdfDictionary) null;
    this.GetEncodedFontDictionary(this.Dictionary, out font);
    if (font != null)
      fieldName = this.UpdateEncodedValue(fieldName, font);
    switch (pdfName1.Value)
    {
      case "Tx":
        PdfString pdfString1 = PdfLoadedField.GetValue(this.Dictionary, this.CrossTable, "V", true) as PdfString;
        if (this.Dictionary.ContainsKey("RV"))
          fieldName += uniquekey;
        if (pdfString1 == null || flag)
          break;
        pdfString1.Value = this.UpdateEncodedValue(pdfString1.Value, font);
        this.SetFields((object) fieldName, (object) this.ReplaceCRtoLF(pdfString1.Value), table);
        break;
      case "Ch":
        PdfName pdfName2 = PdfLoadedField.GetValue(this.Dictionary, this.CrossTable, "V", true) as PdfName;
        string empty = string.Empty;
        if (pdfName2 != (PdfName) null)
        {
          pdfName2.Value = this.UpdateEncodedValue(pdfName2.Value, font);
          this.SetFields((object) fieldName, (object) pdfName2.Value, table);
          break;
        }
        if (PdfLoadedField.GetValue(this.Dictionary, this.CrossTable, "V", true) is PdfString pdfString2)
        {
          pdfString2.Value = this.UpdateEncodedValue(pdfString2.Value, font);
          this.SetFields((object) fieldName, (object) pdfString2.Value, table);
          break;
        }
        if (PdfLoadedField.GetValue(this.Dictionary, this.CrossTable, "V", true) is PdfArray Fieldvalue1)
          this.SetFields((object) fieldName, (object) Fieldvalue1, table);
        if (Fieldvalue1 != null || !this.Dictionary.ContainsKey("I"))
          break;
        string Fieldvalue2 = this.UpdateEncodedValue(!(this.GetType().Name == "PdfLoadedListBoxField") ? (this as PdfLoadedComboBoxField).SelectedValue : (this as PdfLoadedListBoxField).SelectedValue[0], font);
        this.SetFields((object) fieldName, (object) Fieldvalue2, table);
        break;
      case "Btn":
        IPdfPrimitive buttonFieldPrimitive = PdfLoadedField.GetValue(this.Dictionary, this.CrossTable, "V", true);
        PdfLoadedRadioButtonListField radioButtonListField = (PdfLoadedRadioButtonListField) null;
        if (buttonFieldPrimitive != null)
        {
          string exportValue = this.GetExportValue(this, buttonFieldPrimitive);
          if (!string.IsNullOrEmpty(exportValue))
          {
            string str = this.HexToString(exportValue);
            if (this is PdfLoadedRadioButtonListField)
              radioButtonListField = this as PdfLoadedRadioButtonListField;
            if (!this.Dictionary.ContainsKey("Opt") || radioButtonListField != null && radioButtonListField.SelectedIndex == -1)
            {
              this.SetFields((object) fieldName, (object) str, table);
              break;
            }
            if (this.Dictionary == null || !this.Dictionary.ContainsKey("Opt"))
              break;
            PdfArray pdfArray = PdfCrossTable.Dereference(this.Dictionary["Opt"]) as PdfArray;
            int result = 0;
            try
            {
              int.TryParse(str, out result);
              if (pdfArray != null)
              {
                PdfString pdfString3 = radioButtonListField == null ? PdfCrossTable.Dereference(pdfArray[result]) as PdfString : PdfCrossTable.Dereference(pdfArray[radioButtonListField.SelectedIndex]) as PdfString;
                if (pdfString3 != null)
                  str = pdfString3.Value;
              }
              if (string.IsNullOrEmpty(str))
                break;
              string Fieldvalue3 = this.UpdateEncodedValue(str, font);
              this.SetFields((object) fieldName, (object) Fieldvalue3, table);
              break;
            }
            catch
            {
              break;
            }
          }
          else
          {
            switch (this)
            {
              case PdfLoadedRadioButtonListField _:
              case PdfLoadedCheckBoxField _:
                this.SetFields((object) fieldName, (object) "Off", table);
                return;
              default:
                return;
            }
          }
        }
        else
        {
          if (this is PdfLoadedRadioButtonListField)
          {
            this.SetFields((object) fieldName, (object) this.GetAppearanceStateValue(this), table);
            break;
          }
          PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
          if (widgetAnnotation == null)
            break;
          PdfName pdfName3 = widgetAnnotation["AS"] as PdfName;
          if (!(pdfName3 != (PdfName) null))
            break;
          this.SetFields((object) fieldName, (object) pdfName3.Value, table);
          break;
        }
    }
  }

  private string HexToString(string text)
  {
    StringBuilder stringBuilder = new StringBuilder();
    for (int index = 0; index < text.Length; ++index)
    {
      if (text[index] == '#')
      {
        string s = text.Substring(index + 1, 2);
        int num = int.Parse(s, NumberStyles.HexNumber);
        if (num > (int) sbyte.MaxValue && num < (int) byte.MaxValue)
          stringBuilder.Append("%" + s);
        else
          stringBuilder.Append((char) num);
        index += 2;
      }
      else
        stringBuilder.Append(text[index]);
    }
    return stringBuilder.ToString();
  }

  private string ReplaceCRtoLF(string value)
  {
    if (this is PdfLoadedTextBoxField loadedTextBoxField && !loadedTextBoxField.Multiline)
      return value;
    value = value.Replace("\r\n", "\r");
    return value.Replace("\r", "\n");
  }

  internal string GetExportValue(PdfLoadedField field, IPdfPrimitive buttonFieldPrimitive)
  {
    string exportValue = (string) null;
    PdfName pdfName = buttonFieldPrimitive as PdfName;
    if (pdfName != (PdfName) null)
      exportValue = pdfName.Value;
    else if (buttonFieldPrimitive is PdfString pdfString)
      exportValue = pdfString.Value;
    if (exportValue != null && field is PdfLoadedRadioButtonListField radioButtonListField)
    {
      PdfLoadedRadioButtonItem selectedItem = radioButtonListField.SelectedItem;
      if (selectedItem != null && selectedItem.Value == exportValue && !string.IsNullOrEmpty(selectedItem.Value))
        exportValue = selectedItem.Value;
    }
    return exportValue;
  }

  internal string GetAppearanceStateValue(PdfLoadedField field)
  {
    List<PdfDictionary> widgetAnnotations = field.GetWidgetAnnotations(field.Dictionary, field.CrossTable);
    string appearanceStateValue = (string) null;
    if (widgetAnnotations != null)
    {
      foreach (PdfDictionary pdfDictionary in widgetAnnotations)
      {
        PdfName pdfName = pdfDictionary["AS"] as PdfName;
        if (pdfName != (PdfName) null && pdfName.Value != "Off")
          appearanceStateValue = pdfName.Value;
      }
    }
    if (appearanceStateValue == null && this.ExportEmptyField)
      appearanceStateValue = "";
    else if (appearanceStateValue == null)
      appearanceStateValue = "Off";
    return appearanceStateValue;
  }

  internal void ExportField(Stream stream, ref int objectid)
  {
    bool flag = false;
    pdfArray1 = (PdfArray) null;
    if (this.Dictionary.ContainsKey("Kids") && this.CrossTable.GetObject(this.Dictionary["Kids"]) is PdfArray pdfArray1)
    {
      for (int index = 0; index < pdfArray1.Count; ++index)
        flag = flag || pdfArray1[index] is PdfLoadedField;
    }
    PdfName pdfName1 = PdfLoadedField.GetValue(this.Dictionary, this.CrossTable, "FT", true) as PdfName;
    string text1 = "";
    if (pdfName1 != (PdfName) null)
    {
      switch (pdfName1.Value)
      {
        case "Tx":
          if (PdfLoadedField.GetValue(this.Dictionary, this.CrossTable, "V", true) is PdfString pdfString1)
          {
            text1 = pdfString1.Value;
            break;
          }
          break;
        case "Ch":
          IPdfPrimitive buttonFieldPrimitive1 = PdfLoadedField.GetValue(this.Dictionary, this.CrossTable, "V", true);
          string str1 = (string) null;
          if (buttonFieldPrimitive1 != null)
            str1 = this.GetExportValue(this, buttonFieldPrimitive1);
          if (buttonFieldPrimitive1 == null && this.Dictionary.ContainsKey("I"))
            str1 = !(this.GetType().Name == "PdfLoadedListBoxField") ? (this as PdfLoadedComboBoxField).SelectedValue : (this as PdfLoadedListBoxField).SelectedValue[0];
          if (!string.IsNullOrEmpty(str1))
          {
            text1 = str1;
            break;
          }
          break;
        case "Btn":
          IPdfPrimitive buttonFieldPrimitive2 = PdfLoadedField.GetValue(this.Dictionary, this.CrossTable, "V", true);
          PdfLoadedRadioButtonListField radioButtonListField = (PdfLoadedRadioButtonListField) null;
          if (buttonFieldPrimitive2 != null)
          {
            string exportValue = this.GetExportValue(this, buttonFieldPrimitive2);
            if (this is PdfLoadedRadioButtonListField)
              radioButtonListField = this as PdfLoadedRadioButtonListField;
            if (radioButtonListField != null && radioButtonListField.SelectedIndex == -1)
            {
              if (!this.ExportEmptyField)
              {
                text1 = "Off";
                break;
              }
              break;
            }
            if (this.Dictionary != null && this.Dictionary.ContainsKey("Opt"))
            {
              PdfArray pdfArray2 = PdfCrossTable.Dereference(this.Dictionary["Opt"]) as PdfArray;
              int result = 0;
              try
              {
                int.TryParse(exportValue, out result);
                if (pdfArray2 != null)
                {
                  PdfString pdfString2 = radioButtonListField == null ? PdfCrossTable.Dereference(pdfArray2[result]) as PdfString : PdfCrossTable.Dereference(pdfArray2[radioButtonListField.SelectedIndex]) as PdfString;
                  if (pdfString2 != null)
                    exportValue = pdfString2.Value;
                }
              }
              catch
              {
              }
              if (!string.IsNullOrEmpty(exportValue))
              {
                text1 = exportValue;
                break;
              }
              break;
            }
            if (!string.IsNullOrEmpty(exportValue))
            {
              text1 = exportValue;
              break;
            }
            if (this is PdfLoadedCheckBoxField && !this.ExportEmptyField)
            {
              text1 = "Off";
              break;
            }
            break;
          }
          if (this is PdfLoadedRadioButtonListField)
          {
            text1 = this.GetAppearanceStateValue(this);
            break;
          }
          PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
          if (widgetAnnotation != null)
          {
            PdfName pdfName2 = widgetAnnotation["AS"] as PdfName;
            if (pdfName2 != (PdfName) null)
            {
              text1 = pdfName2.Value;
              break;
            }
            break;
          }
          break;
      }
    }
    if (PdfLoadedField.validateString(text1) && !this.ExportEmptyField && !flag)
      return;
    if (flag)
    {
      for (int index = 0; index < pdfArray1.Count; ++index)
      {
        if (pdfArray1[index] is PdfLoadedField pdfLoadedField && pdfLoadedField.Export)
          pdfLoadedField.ExportField(stream, ref objectid);
      }
      this.ObjectID = objectid;
      ++objectid;
      StringBuilder stringBuilder = new StringBuilder();
      byte[] bytes1 = Encoding.GetEncoding("windows-1252").GetBytes(new PdfString(text1)
      {
        Encode = PdfString.ForceEncoding.ASCII
      }.Value);
      stringBuilder.AppendFormat("{0} 0 obj<</T <{1}> /Kids [", (object) this.ObjectID, (object) PdfString.BytesToHex(bytes1));
      for (int index = 0; index < pdfArray1.Count; ++index)
      {
        if (pdfArray1[index] is PdfLoadedField pdfLoadedField && pdfLoadedField.Export && pdfLoadedField.ObjectID != 0)
          stringBuilder.AppendFormat("{0} 0 R ", (object) pdfLoadedField.ObjectID);
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
      string str2;
      if (this.GetType().Name == "PdfLoadedCheckBoxField" || this.GetType().Name == "PdfLoadedRadioButtonListField")
        str2 = "/" + text1;
      else
        str2 = $"<{PdfString.BytesToHex(Encoding.GetEncoding("windows-1252").GetBytes(new PdfString(text1)
        {
          Encode = PdfString.ForceEncoding.ASCII
        }.Value))}>";
      StringBuilder stringBuilder = new StringBuilder();
      byte[] bytes3 = Encoding.GetEncoding("windows-1252").GetBytes(new PdfString(this.Name)
      {
        Encode = PdfString.ForceEncoding.ASCII
      }.Value);
      stringBuilder.AppendFormat("{0} 0 obj<</T <{1}> /V {2} >>endobj\n", (object) this.ObjectID, (object) PdfString.BytesToHex(bytes3), (object) str2);
      byte[] bytes4 = Encoding.GetEncoding("windows-1252").GetBytes(new PdfString(stringBuilder.ToString())
      {
        Encode = PdfString.ForceEncoding.ASCII
      }.Value);
      stream.Write(bytes4, 0, bytes4.Length);
    }
  }

  internal void ExportField(System.Collections.Generic.Dictionary<string, object> table)
  {
    PdfName pdfName1 = PdfLoadedField.GetValue(this.Dictionary, this.CrossTable, "FT", true) as PdfName;
    string str1 = "";
    if (!(pdfName1 != (PdfName) null))
      return;
    switch (pdfName1.Value)
    {
      case "Tx":
        List<PdfString> Fieldvalue1 = new List<PdfString>();
        if (this.Dictionary.ContainsKey("RV"))
        {
          PdfString pdfString = PdfLoadedField.GetValue(this.Dictionary, this.CrossTable, "RV", true) as PdfString;
          Fieldvalue1.Add(pdfString);
        }
        if (!(PdfLoadedField.GetValue(this.Dictionary, this.CrossTable, "V", true) is PdfString pdfString1))
          break;
        string str2 = pdfString1.Value;
        Fieldvalue1.Add(new PdfString(str2));
        this.SetFields(this.Name, (object) Fieldvalue1, table);
        break;
      case "Ch":
        IPdfPrimitive buttonFieldPrimitive1 = PdfLoadedField.GetValue(this.Dictionary, this.CrossTable, "V", true);
        if (buttonFieldPrimitive1 != null)
        {
          PdfArray Fieldvalue2 = buttonFieldPrimitive1 as PdfArray;
          string exportValue = this.GetExportValue(this, buttonFieldPrimitive1);
          if (!string.IsNullOrEmpty(exportValue))
          {
            this.SetFields(this.Name, (object) new PdfString(exportValue), table);
            break;
          }
          if (Fieldvalue2 == null)
            break;
          this.SetFields(this.Name, (object) Fieldvalue2, table);
          break;
        }
        if (buttonFieldPrimitive1 != null || !this.Dictionary.ContainsKey("I"))
          break;
        this.SetFields(this.Name, (object) new PdfString(!(this.GetType().Name == "PdfLoadedListBoxField") ? (this as PdfLoadedComboBoxField).SelectedValue : (this as PdfLoadedListBoxField).SelectedValue[0]), table);
        break;
      case "Btn":
        IPdfPrimitive buttonFieldPrimitive2 = PdfLoadedField.GetValue(this.Dictionary, this.CrossTable, "V", true);
        PdfLoadedRadioButtonListField radioButtonListField = (PdfLoadedRadioButtonListField) null;
        if (buttonFieldPrimitive2 != null)
        {
          string exportValue = this.GetExportValue(this, buttonFieldPrimitive2);
          if (this is PdfLoadedRadioButtonListField)
            radioButtonListField = this as PdfLoadedRadioButtonListField;
          if (radioButtonListField != null && radioButtonListField.SelectedIndex == -1)
          {
            this.SetFields(this.Name, (object) new PdfString("Off"), table);
            break;
          }
          if (this.Dictionary != null && this.Dictionary.ContainsKey("Opt"))
          {
            PdfArray pdfArray = PdfCrossTable.Dereference(this.Dictionary["Opt"]) as PdfArray;
            int result = 0;
            try
            {
              int.TryParse(exportValue, out result);
              if (pdfArray != null)
              {
                PdfString pdfString2 = radioButtonListField == null ? PdfCrossTable.Dereference(pdfArray[result]) as PdfString : PdfCrossTable.Dereference(pdfArray[radioButtonListField.SelectedIndex]) as PdfString;
                if (pdfString2 != null)
                  exportValue = pdfString2.Value;
              }
              if (string.IsNullOrEmpty(exportValue))
                break;
              this.SetFields(this.Name, (object) new PdfString(exportValue), table);
              break;
            }
            catch
            {
              break;
            }
          }
          else
          {
            if (!string.IsNullOrEmpty(exportValue))
            {
              this.SetFields(this.Name, (object) new PdfName(exportValue), table);
              break;
            }
            if (!(this is PdfLoadedCheckBoxField))
              break;
            this.SetFields(this.Name, (object) new PdfString("Off"), table);
            break;
          }
        }
        else
        {
          if (this is PdfLoadedRadioButtonListField)
          {
            str1 = this.GetAppearanceStateValue(this);
          }
          else
          {
            PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(this.Dictionary, this.CrossTable);
            if (widgetAnnotation != null)
            {
              PdfName pdfName2 = widgetAnnotation["AS"] as PdfName;
              if (pdfName2 != (PdfName) null)
                str1 = pdfName2.Value;
            }
          }
          if (string.IsNullOrEmpty(str1))
            break;
          this.SetFields(this.Name, (object) new PdfName(str1), table);
          break;
        }
    }
  }

  internal void SetFields(object fieldName, object Fieldvalue, System.Collections.Generic.Dictionary<object, object> table)
  {
    table[fieldName] = Fieldvalue;
  }

  internal void SetFields(string fieldName, object Fieldvalue, System.Collections.Generic.Dictionary<string, object> table)
  {
    table[fieldName] = Fieldvalue;
  }

  internal void ImportFieldValue(object FieldValue)
  {
    PdfName pdfName = PdfLoadedField.GetValue(this.Dictionary, this.CrossTable, "FT", true) as PdfName;
    string str = FieldValue as string;
    strArray = (string[]) null;
    if (str == null && FieldValue is string[] strArray)
      str = strArray[0];
    if (str == null)
      return;
    switch (pdfName.Value)
    {
      case "Tx":
        if (str == null)
          break;
        (this as PdfLoadedTextBoxField).Text = str;
        break;
      case "Ch":
        if (this.GetType().Name == "PdfLoadedListBoxField")
        {
          PdfLoadedListBoxField loadedListBoxField = this as PdfLoadedListBoxField;
          if (strArray != null)
          {
            loadedListBoxField.SelectedValue = strArray;
            break;
          }
          loadedListBoxField.SelectedValue = new string[1]
          {
            str.Split(',')[0]
          };
          break;
        }
        if (!(this.GetType().Name == "PdfLoadedComboBoxField"))
          break;
        PdfLoadedComboBoxField loadedComboBoxField = this as PdfLoadedComboBoxField;
        if (this.isAcrobat && loadedComboBoxField != null && loadedComboBoxField.Dictionary.ContainsKey("AP"))
          loadedComboBoxField.Dictionary.Remove("AP");
        loadedComboBoxField.SelectedValue = str;
        break;
      case "Btn":
        if (this is PdfLoadedCheckBoxField loadedCheckBoxField)
        {
          if (str.ToUpper() == "off".ToUpper() || str.ToUpper() == "no".ToUpper())
          {
            loadedCheckBoxField.Checked = false;
            break;
          }
          if (this.ContainsExportValue(str, loadedCheckBoxField.Dictionary))
          {
            loadedCheckBoxField.Checked = true;
            break;
          }
          loadedCheckBoxField.Checked = false;
          break;
        }
        if (!(this.GetType().Name == "PdfLoadedRadioButtonListField"))
          break;
        PdfLoadedRadioButtonListField radioButtonListField = this as PdfLoadedRadioButtonListField;
        if (str.Contains("%"))
          str = str.Replace("%", "#");
        else if (!str.Contains("#"))
          str = PdfName.EncodeName(str);
        if (radioButtonListField.Dictionary.ContainsKey("Opt") && PdfCrossTable.Dereference(radioButtonListField.Dictionary["Opt"]) is PdfArray pdfArray)
        {
          for (int index = 0; index < pdfArray.Count; ++index)
          {
            if (PdfCrossTable.Dereference(pdfArray[index]) is PdfString pdfString && pdfString.Value == str)
            {
              str = index.ToString();
              break;
            }
          }
        }
        radioButtonListField.SelectedValue = str;
        break;
    }
  }

  private bool ContainsExportValue(string value, PdfDictionary dictionary)
  {
    bool flag = false;
    PdfDictionary pdfDictionary1 = new PdfDictionary();
    PdfDictionary pdfDictionary2 = new PdfDictionary();
    PdfDictionary pdfDictionary3 = new PdfDictionary();
    if (dictionary.ContainsKey("Kids"))
    {
      if (this.CrossTable.GetObject(dictionary["Kids"]) is PdfArray pdfArray1)
      {
        for (int index = 0; index < pdfArray1.Count; ++index)
        {
          this.DefaultIndex = index;
          PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(dictionary, this.CrossTable);
          if (widgetAnnotation != null && widgetAnnotation.ContainsKey("AP") && this.CrossTable.GetObject(widgetAnnotation["AP"]) is PdfDictionary pdfDictionary4 && pdfDictionary4.ContainsKey("N") && PdfCrossTable.Dereference(pdfDictionary4["N"]) is PdfDictionary pdfDictionary5 && pdfDictionary5.ContainsKey(value))
            return true;
        }
      }
    }
    else
    {
      PdfDictionary widgetAnnotation = this.GetWidgetAnnotation(dictionary, this.CrossTable);
      if (widgetAnnotation != null && widgetAnnotation.ContainsKey("AP") && this.CrossTable.GetObject(widgetAnnotation["AP"]) is PdfDictionary pdfDictionary6 && pdfDictionary6.ContainsKey("N") && PdfCrossTable.Dereference(pdfDictionary6["N"]) is PdfDictionary pdfDictionary7 && pdfDictionary7.ContainsKey(value))
        flag = true;
      if (this.isAcrobat && !flag && widgetAnnotation.ContainsKey("AS"))
      {
        string str = (this.CrossTable.GetObject(widgetAnnotation["AS"]) as PdfName).Value;
        if (str == "Off" && widgetAnnotation.ContainsKey("Opt"))
        {
          PdfArray pdfArray2 = PdfCrossTable.Dereference(widgetAnnotation["Opt"]) as PdfArray;
          for (int index = 0; index < pdfArray2.Count; ++index)
          {
            if ((pdfArray2[index] as PdfString).Value == value)
              flag = true;
          }
        }
        else if (str != "Off")
          flag = true;
      }
    }
    return flag;
  }

  internal static bool validateString(string text1) => text1 == null || text1.Length == 0;

  internal string GetFieldName()
  {
    string fieldName = (string) null;
    PdfString pdfString1 = (PdfString) null;
    if (!this.Dictionary.ContainsKey("Parent"))
    {
      pdfString1 = PdfLoadedField.GetValue(this.Dictionary, this.m_crossTable, "T", false) as PdfString;
    }
    else
    {
      dictionary = this.m_crossTable.GetObject(this.Dictionary["Parent"]) as PdfDictionary;
      while (dictionary.ContainsKey("Parent"))
      {
        if (dictionary.ContainsKey("T"))
          fieldName = fieldName == null ? (PdfLoadedField.GetValue(dictionary, this.m_crossTable, "T", false) as PdfString).Value : $"{(PdfLoadedField.GetValue(dictionary, this.m_crossTable, "T", false) as PdfString).Value}.{fieldName}";
        if (this.m_crossTable.GetObject(dictionary["Parent"]) is PdfDictionary dictionary && !dictionary.ContainsKey("T"))
          break;
      }
      if (dictionary.ContainsKey("T"))
      {
        fieldName = fieldName == null ? (PdfLoadedField.GetValue(dictionary, this.m_crossTable, "T", false) as PdfString).Value : $"{(PdfLoadedField.GetValue(dictionary, this.m_crossTable, "T", false) as PdfString).Value}.{fieldName}";
        if (PdfLoadedField.GetValue(this.Dictionary, this.m_crossTable, "T", false) is PdfString pdfString2)
          fieldName += $".{pdfString2.Value}";
      }
      else if (this.Dictionary.ContainsKey("T"))
        pdfString1 = PdfLoadedField.GetValue(this.Dictionary, this.m_crossTable, "T", false) as PdfString;
    }
    PdfReference reference = this.m_crossTable.GetReference((IPdfPrimitive) this.Dictionary);
    bool flag = false;
    if (this.m_crossTable.Document != null && this.m_crossTable.Document is PdfLoadedDocument)
      flag = (this.m_crossTable.Document as PdfLoadedDocument).WasEncrypted;
    if (flag && reference != (PdfReference) null && pdfString1 != null)
      pdfString1.Decrypt(this.CrossTable.Encryptor, reference.ObjNum);
    if (pdfString1 != null)
      fieldName = pdfString1.Value;
    return fieldName;
  }

  internal string ReplaceNotUsedCharacters(
    string input,
    System.Collections.Generic.Dictionary<string, string> encodingDifference)
  {
    char[] charArray = input.ToCharArray();
    string str = string.Empty;
    foreach (char ch in charArray)
    {
      int num = (int) ch;
      str = !encodingDifference.ContainsKey(num.ToString()) ? str + (object) ch : str + encodingDifference[num.ToString()];
    }
    return str;
  }

  internal string UpdateEncodedValue(string value, PdfDictionary font)
  {
    string input = value;
    bool flag = false;
    string str;
    if (this.CrossTable != null && this.CrossTable.m_pdfDocumentEncoding != null)
    {
      FontStructure fontStructure = new FontStructure((IPdfPrimitive) this.CrossTable.m_pdfDocumentEncoding);
      return str = this.ReplaceNotUsedCharacters(input, fontStructure.DifferencesDictionary);
    }
    if (this.CrossTable != null && this.CrossTable.DocumentCatalog != null)
    {
      if (PdfCrossTable.Dereference(this.CrossTable.DocumentCatalog["AcroForm"]) is PdfDictionary pdfDictionary1 && pdfDictionary1.ContainsKey("DR") && PdfCrossTable.Dereference(pdfDictionary1["DR"]) is PdfDictionary pdfDictionary2 && pdfDictionary2.ContainsKey("Encoding") && PdfCrossTable.Dereference(pdfDictionary2["Encoding"]) is PdfDictionary pdfDictionary3 && pdfDictionary3.ContainsKey("PDFDocEncoding") && PdfCrossTable.Dereference(pdfDictionary3["PDFDocEncoding"]) is PdfDictionary pdfDictionary4 && pdfDictionary4.ContainsKey("Differences"))
      {
        PdfDictionary pdfDictionary = new PdfDictionary();
        pdfDictionary["Differences"] = pdfDictionary4["Differences"];
        PdfDictionary fontDictionary = new PdfDictionary();
        fontDictionary["Subtype"] = (IPdfPrimitive) new PdfName("Type1");
        fontDictionary["Encoding"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary);
        FontStructure fontStructure = new FontStructure((IPdfPrimitive) fontDictionary);
        if (fontStructure != null && fontStructure.DifferencesDictionary.Count > 0)
        {
          this.CrossTable.m_pdfDocumentEncoding = fontDictionary;
          return str = this.ReplaceNotUsedCharacters(input, fontStructure.DifferencesDictionary);
        }
      }
      if (!flag && value != null && font != null && font.Items != null && font.Items.Count > 0)
      {
        foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in font.Items)
        {
          FontStructure fontStructure = new FontStructure((IPdfPrimitive) (PdfCrossTable.Dereference(keyValuePair.Value) as PdfDictionary));
          if (fontStructure != null && fontStructure.DifferencesDictionary.Count > 0)
            input = this.ReplaceNotUsedCharacters(input, fontStructure.DifferencesDictionary);
        }
      }
    }
    return input;
  }

  internal void GetEncodedFontDictionary(PdfDictionary fieldDictionary, out PdfDictionary font)
  {
    PdfArray pdfArray = (PdfArray) null;
    font = (PdfDictionary) null;
    if (!fieldDictionary.ContainsKey("AP") && fieldDictionary.ContainsKey("Kids"))
      pdfArray = PdfCrossTable.Dereference(fieldDictionary["Kids"]) as PdfArray;
    if (!fieldDictionary.ContainsKey("AP") && pdfArray == null)
      return;
    PdfDictionary pdfDictionary1 = (PdfDictionary) null;
    if (pdfArray != null && pdfArray.Count > 0)
    {
      if (PdfCrossTable.Dereference(pdfArray[0]) is PdfDictionary pdfDictionary2 && pdfDictionary2.ContainsKey("AP"))
        pdfDictionary1 = PdfCrossTable.Dereference(pdfDictionary2["AP"]) as PdfDictionary;
    }
    else
      pdfDictionary1 = PdfCrossTable.Dereference(this.Dictionary["AP"]) as PdfDictionary;
    if (pdfDictionary1 == null || !pdfDictionary1.ContainsKey("N") || !(PdfCrossTable.Dereference(pdfDictionary1["N"]) is PdfDictionary pdfDictionary3) || !pdfDictionary3.ContainsKey("Resources") || !(PdfCrossTable.Dereference(pdfDictionary3["Resources"]) is PdfDictionary pdfDictionary4) || !pdfDictionary4.ContainsKey("Font"))
      return;
    font = PdfCrossTable.Dereference(pdfDictionary4["Font"]) as PdfDictionary;
  }

  internal delegate void BeforeNameChangesEventHandler(string name);
}
