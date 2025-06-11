// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.PdfLoadedFormFieldCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using Syncfusion.Pdf.Security;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

public class PdfLoadedFormFieldCollection : PdfFieldCollection
{
  private PdfLoadedForm m_form;
  private System.Collections.Generic.List<string> m_fieldNames;
  private System.Collections.Generic.List<string> m_indexedFieldNames;
  private System.Collections.Generic.List<string> m_actualFieldNames;
  private System.Collections.Generic.List<string> m_indexedActualFieldNames;
  internal System.Collections.Generic.List<string> m_addedFieldNames = new System.Collections.Generic.List<string>();
  private bool m_isCreateNewFormField;
  internal bool m_isImported;

  public override PdfField this[int index]
  {
    get
    {
      int count = this.List.Count;
      return count >= 0 && index < count ? this.List[index] as PdfField : throw new IndexOutOfRangeException(nameof (index));
    }
  }

  public new PdfField this[string name]
  {
    get
    {
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      int index = !(name == string.Empty) ? this.GetFieldIndex(name) : throw new ArgumentException("Field name can't be empty");
      return index != -1 ? this[index] : throw new ArgumentException("Incorrect field name");
    }
  }

  public PdfLoadedForm Form
  {
    get => this.m_form;
    set => this.m_form = value;
  }

  public PdfLoadedFormFieldCollection(PdfLoadedForm form)
  {
    this.m_form = form != null ? form : throw new ArgumentException(nameof (form));
    int index = 0;
    for (int count = this.m_form.TerminalFields.Count; index < count; ++index)
    {
      PdfField field = this.GetField(index);
      if (field != null)
        this.DoAdd(field);
    }
  }

  public PdfLoadedFormFieldCollection()
  {
  }

  public bool ValidateSignatures(out System.Collections.Generic.List<PdfSignatureValidationResult> results)
  {
    return this.ValidateSignatures(new PdfSignatureValidationOptions(), out results);
  }

  public bool ValidateSignatures(
    PdfSignatureValidationOptions options,
    out System.Collections.Generic.List<PdfSignatureValidationResult> results)
  {
    bool flag = true;
    results = new System.Collections.Generic.List<PdfSignatureValidationResult>();
    for (int index = 0; index < this.Count; ++index)
    {
      if (this[index] is PdfLoadedSignatureField loadedSignatureField && loadedSignatureField.IsSigned)
      {
        PdfSignatureValidationResult validationResult = loadedSignatureField.ValidateSignature(options);
        if (validationResult != null)
        {
          results.Add(validationResult);
          flag = flag && validationResult.IsSignatureValid;
        }
      }
    }
    if (results.Count == 0)
    {
      results = (System.Collections.Generic.List<PdfSignatureValidationResult>) null;
      flag = false;
    }
    return flag;
  }

  public bool ValidateSignatures(
    X509Certificate2Collection rootCertificates,
    out System.Collections.Generic.List<PdfSignatureValidationResult> results)
  {
    PdfSignatureValidationOptions options = new PdfSignatureValidationOptions();
    return this.ValidateSignatures(rootCertificates, options, out results);
  }

  public bool ValidateSignatures(
    X509Certificate2Collection rootCertificates,
    PdfSignatureValidationOptions options,
    out System.Collections.Generic.List<PdfSignatureValidationResult> results)
  {
    bool flag = true;
    results = new System.Collections.Generic.List<PdfSignatureValidationResult>();
    for (int index = 0; index < this.Count; ++index)
    {
      if (this[index] is PdfLoadedSignatureField loadedSignatureField && loadedSignatureField.IsSigned)
      {
        PdfSignatureValidationResult validationResult = loadedSignatureField.ValidateSignature((X509CertificateCollection) rootCertificates, options);
        if (validationResult != null)
        {
          results.Add(validationResult);
          flag = flag && validationResult.IsSignatureValid;
        }
      }
    }
    if (results.Count == 0)
    {
      results = (System.Collections.Generic.List<PdfSignatureValidationResult>) null;
      flag = false;
    }
    return flag;
  }

  private PdfField GetField(int index)
  {
    PdfDictionary terminalField = this.m_form.TerminalFields[index];
    PdfCrossTable crossTable = this.m_form.CrossTable;
    PdfField field = (PdfField) null;
    PdfName name = PdfLoadedField.GetValue(terminalField, crossTable, "FT", true) as PdfName;
    PdfLoadedFieldTypes loadedFieldTypes = PdfLoadedFieldTypes.Null;
    if (name != (PdfName) null)
      loadedFieldTypes = this.GetFieldType(name, terminalField, crossTable);
    switch (loadedFieldTypes)
    {
      case PdfLoadedFieldTypes.PushButton:
        field = this.CreatePushButton(terminalField, crossTable);
        break;
      case PdfLoadedFieldTypes.CheckBox:
        field = this.CreateCheckBox(terminalField, crossTable);
        break;
      case PdfLoadedFieldTypes.RadioButton:
        field = this.CreateRadioButton(terminalField, crossTable);
        break;
      case PdfLoadedFieldTypes.TextField:
        field = this.CreateTextField(terminalField, crossTable);
        break;
      case PdfLoadedFieldTypes.ListBox:
        field = this.CreateListBox(terminalField, crossTable);
        break;
      case PdfLoadedFieldTypes.ComboBox:
        field = this.CreateComboBox(terminalField, crossTable);
        break;
      case PdfLoadedFieldTypes.SignatureField:
        field = this.CreateSignatureField(terminalField, crossTable);
        break;
      case PdfLoadedFieldTypes.Null:
        field = (PdfField) new PdfLoadedStyledField(terminalField, crossTable);
        field.SetForm((PdfForm) this.Form);
        break;
    }
    if (field is PdfLoadedField pdfLoadedField)
    {
      pdfLoadedField.SetForm((PdfForm) this.Form);
      pdfLoadedField.BeforeNameChanges += new PdfLoadedField.BeforeNameChangesEventHandler(this.ldField_NameChanded);
    }
    return field;
  }

  private PdfField GetField(PdfDictionary dictionary)
  {
    PdfCrossTable crossTable = this.m_form.CrossTable;
    PdfField field = (PdfField) null;
    PdfName name = PdfLoadedField.GetValue(dictionary, crossTable, "FT", true) as PdfName;
    PdfLoadedFieldTypes loadedFieldTypes = PdfLoadedFieldTypes.Null;
    if (name != (PdfName) null)
      loadedFieldTypes = this.GetFieldType(name, dictionary, crossTable);
    switch (loadedFieldTypes)
    {
      case PdfLoadedFieldTypes.PushButton:
        field = this.CreatePushButton(dictionary, crossTable);
        break;
      case PdfLoadedFieldTypes.CheckBox:
        field = this.CreateCheckBox(dictionary, crossTable);
        break;
      case PdfLoadedFieldTypes.RadioButton:
        field = this.CreateRadioButton(dictionary, crossTable);
        break;
      case PdfLoadedFieldTypes.TextField:
        field = this.CreateTextField(dictionary, crossTable);
        break;
      case PdfLoadedFieldTypes.ListBox:
        field = this.CreateListBox(dictionary, crossTable);
        break;
      case PdfLoadedFieldTypes.ComboBox:
        field = this.CreateComboBox(dictionary, crossTable);
        break;
      case PdfLoadedFieldTypes.SignatureField:
        field = this.CreateSignatureField(dictionary, crossTable);
        break;
      case PdfLoadedFieldTypes.Null:
        field = (PdfField) new PdfLoadedStyledField(dictionary, crossTable);
        field.SetForm((PdfForm) this.Form);
        break;
    }
    if (field is PdfLoadedField pdfLoadedField)
    {
      pdfLoadedField.SetForm((PdfForm) this.Form);
      pdfLoadedField.BeforeNameChanges += new PdfLoadedField.BeforeNameChangesEventHandler(this.ldField_NameChanded);
    }
    return field;
  }

  private PdfField CreateSignatureField(PdfDictionary dictionary, PdfCrossTable crossTable)
  {
    PdfLoadedField signatureField = (PdfLoadedField) new PdfLoadedSignatureField(dictionary, crossTable);
    signatureField.SetForm((PdfForm) this.Form);
    return (PdfField) signatureField;
  }

  private PdfField CreateListBox(PdfDictionary dictionary, PdfCrossTable crossTable)
  {
    PdfLoadedField listBox = (PdfLoadedField) new PdfLoadedListBoxField(dictionary, crossTable);
    listBox.SetForm((PdfForm) this.Form);
    return (PdfField) listBox;
  }

  private PdfField CreateComboBox(PdfDictionary dictionary, PdfCrossTable crossTable)
  {
    PdfLoadedField comboBox = (PdfLoadedField) new PdfLoadedComboBoxField(dictionary, crossTable);
    comboBox.SetForm((PdfForm) this.Form);
    return (PdfField) comboBox;
  }

  private PdfField CreateTextField(PdfDictionary dictionary, PdfCrossTable crossTable)
  {
    PdfLoadedField textField = (PdfLoadedField) new PdfLoadedTextBoxField(dictionary, crossTable);
    textField.SetForm((PdfForm) this.Form);
    return (PdfField) textField;
  }

  private PdfField CreateRadioButton(PdfDictionary dictionary, PdfCrossTable crossTable)
  {
    PdfLoadedField radioButton = (PdfLoadedField) new PdfLoadedRadioButtonListField(dictionary, crossTable);
    radioButton.SetForm((PdfForm) this.Form);
    return (PdfField) radioButton;
  }

  internal void CreateFormFieldsFromWidgets(int startFormFieldIndex)
  {
    for (int index = startFormFieldIndex; index < this.m_form.TerminalFields.Count; ++index)
    {
      PdfField field = this.GetField(index);
      if (field != null)
        this.DoAdd(field);
    }
    if (this.m_form.m_widgetDictionary == null)
      return;
    foreach (KeyValuePair<string, System.Collections.Generic.List<PdfDictionary>> widget in this.m_form.m_widgetDictionary)
    {
      if (widget.Value.Count > 1)
      {
        PdfField field = this.GetField(widget.Value[0]);
        PdfLoadedRadioButtonListField radioButtonListField1 = field as PdfLoadedRadioButtonListField;
        for (int index1 = 1; index1 < widget.Value.Count; ++index1)
        {
          field = this.GetField(widget.Value[index1]);
          if (field != null && field is PdfLoadedRadioButtonListField radioButtonListField2)
          {
            for (int index2 = 0; index2 < radioButtonListField2.Items.Count; ++index2)
              radioButtonListField1?.Items.Add(radioButtonListField2.Items[index2]);
          }
        }
        this.m_form.TerminalFields.Add(field.Dictionary);
        if (field != null)
          this.DoAdd(field);
      }
      else
      {
        this.m_form.TerminalFields.Add(widget.Value[0]);
        PdfField field = this.GetField(this.m_form.TerminalFields.Count - 1);
        if (field != null)
          this.DoAdd(field);
      }
    }
  }

  private PdfField CreateCheckBox(PdfDictionary dictionary, PdfCrossTable crossTable)
  {
    PdfLoadedField checkBox = (PdfLoadedField) new PdfLoadedCheckBoxField(dictionary, crossTable);
    checkBox.SetForm((PdfForm) this.Form);
    return (PdfField) checkBox;
  }

  private PdfField CreatePushButton(PdfDictionary dictionary, PdfCrossTable crossTable)
  {
    PdfLoadedField pushButton = (PdfLoadedField) new PdfLoadedButtonField(dictionary, crossTable);
    pushButton.SetForm((PdfForm) this.Form);
    return (PdfField) pushButton;
  }

  internal PdfLoadedFieldTypes GetFieldType(
    PdfName name,
    PdfDictionary dictionary,
    PdfCrossTable crossTable)
  {
    string str = name.Value;
    PdfLoadedFieldTypes fieldType = PdfLoadedFieldTypes.Null;
    PdfNumber pdfNumber = PdfLoadedField.GetValue(dictionary, crossTable, "Ff", true) as PdfNumber;
    int num = 0;
    if (pdfNumber != null)
      num = pdfNumber.IntValue;
    switch (str.ToLower())
    {
      case "btn":
        fieldType = (num & 65536 /*0x010000*/) == 0 ? ((num & 32768 /*0x8000*/) == 0 ? PdfLoadedFieldTypes.CheckBox : PdfLoadedFieldTypes.RadioButton) : PdfLoadedFieldTypes.PushButton;
        break;
      case "tx":
        fieldType = PdfLoadedFieldTypes.TextField;
        break;
      case "ch":
        fieldType = (num & 131072 /*0x020000*/) == 0 ? PdfLoadedFieldTypes.ListBox : PdfLoadedFieldTypes.ComboBox;
        break;
      case "sig":
        fieldType = PdfLoadedFieldTypes.SignatureField;
        break;
    }
    return fieldType;
  }

  protected override int DoAdd(PdfField field)
  {
    if (field == null)
      throw new ArgumentNullException(nameof (field));
    field.SetForm((PdfForm) this.m_form);
    bool flag = false;
    PdfArray primitive1 = !this.m_form.Dictionary.ContainsKey("Fields") ? new PdfArray() : this.m_form.CrossTable.GetObject(this.m_form.Dictionary["Fields"]) as PdfArray;
    if (field.Dictionary.Items.ContainsKey(new PdfName("Parent")) && !this.m_isImported)
      flag = true;
    PdfReferenceHolder element1 = new PdfReferenceHolder((IPdfWrapper) field);
    if (!primitive1.Contains((IPdfPrimitive) element1))
    {
      if (this.IsValidName(field.Name) && !flag)
      {
        primitive1.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) field));
        this.m_form.Dictionary.SetProperty("Fields", (IPdfPrimitive) primitive1);
      }
      else if (this.m_form.FieldAutoNaming && !flag)
      {
        string correctName = this.GetCorrectName(field.Name);
        field.ApplyName(correctName);
        primitive1.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) field));
        this.m_form.Dictionary.SetProperty("Fields", (IPdfPrimitive) primitive1);
      }
      else
      {
        if (this.IsValidName(field.Name) && flag || this.m_form.FieldAutoNaming && flag)
        {
          this.m_addedFieldNames.Add(field.Name);
          return base.DoAdd(field);
        }
        if (this.Count <= 0)
          return base.DoAdd(field);
        foreach (PdfField pdfField in (PdfCollection) this)
        {
          if (pdfField.Name == field.Name)
          {
            if (field is PdfTextBoxField && (pdfField is PdfTextBoxField || pdfField is PdfLoadedTextBoxField))
            {
              if (pdfField is PdfLoadedTextBoxField)
              {
                PdfArray pdfArray1 = pdfField.Dictionary.Items[new PdfName("Kids")] as PdfArray;
                (field as PdfTextBoxField).Widget.Dictionary?.Remove("Parent");
                (field as PdfTextBoxField).Widget.Parent = pdfField;
                if (field.Dictionary.Items[new PdfName("Kids")] is PdfArray pdfArray2)
                {
                  foreach (PdfReferenceHolder element2 in pdfArray2.Elements)
                  {
                    if (element2 != (PdfReferenceHolder) null && pdfArray1 != null)
                      pdfArray1.Add((IPdfPrimitive) element2);
                  }
                }
              }
              else
              {
                (field as PdfTextBoxField).Widget.Dictionary?.Remove("Parent");
                (field as PdfTextBoxField).Widget.Parent = pdfField;
                if (field is PdfTextBoxField)
                {
                  PdfTextBoxField pdfTextBoxField = field as PdfTextBoxField;
                  if (field.Page is PdfPage)
                    (field.Page as PdfPage).Annotations.Add((PdfAnnotation) pdfTextBoxField.Widget);
                  else if (field.Page is PdfLoadedPage)
                    (field.Page as PdfLoadedPage).Annotations.Add((PdfAnnotation) pdfTextBoxField.Widget);
                }
                if (!(pdfField as PdfTextBoxField).m_array.Contains((IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) (pdfField as PdfTextBoxField).Widget)))
                  (pdfField as PdfTextBoxField).m_array.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) (pdfField as PdfTextBoxField).Widget));
                (pdfField as PdfTextBoxField).m_array.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) (field as PdfTextBoxField).Widget));
                pdfField.Dictionary.SetProperty("Kids", (IPdfPrimitive) (pdfField as PdfTextBoxField).m_array);
                (pdfField as PdfTextBoxField).fieldItems.Add(field);
              }
              return this.Count - 1;
            }
            if (field is PdfSignatureField)
            {
              PdfSignatureField pdfSignatureField = field as PdfSignatureField;
              PdfDictionary dictionary = pdfSignatureField.Widget.Dictionary;
              if (dictionary != null && dictionary.ContainsKey("Parent"))
                dictionary.Remove("Parent");
              pdfSignatureField.Widget.Parent = pdfField;
              PdfArray primitive2 = (PdfArray) null;
              PdfArray pdfArray = (PdfArray) null;
              if (pdfField.Dictionary.ContainsKey("Kids"))
                primitive2 = pdfField.Dictionary.Items[new PdfName("Kids")] as PdfArray;
              if (field.Dictionary.ContainsKey("Kids"))
                pdfArray = field.Dictionary.Items[new PdfName("Kids")] as PdfArray;
              if (pdfArray != null)
              {
                if (primitive2 == null)
                  primitive2 = new PdfArray();
                foreach (PdfReferenceHolder element3 in pdfArray.Elements)
                {
                  if (element3 != (PdfReferenceHolder) null)
                    primitive2.Add((IPdfPrimitive) element3);
                }
              }
              pdfField.Dictionary.SetProperty("Kids", (IPdfPrimitive) primitive2);
              pdfSignatureField.m_SkipKidsCertificate = true;
              if (field.Page is PdfPage)
              {
                PdfPage page = field.Page as PdfPage;
                if (!page.Annotations.Contains((PdfAnnotation) pdfSignatureField.Widget))
                  page.Annotations.Add((PdfAnnotation) pdfSignatureField.Widget);
              }
              else if (field.Page is PdfLoadedPage)
              {
                PdfLoadedPage page = field.Page as PdfLoadedPage;
                if (!page.Annotations.Contains((PdfAnnotation) pdfSignatureField.Widget))
                  page.Annotations.Add((PdfAnnotation) pdfSignatureField.Widget);
              }
              return this.Count - 1;
            }
          }
        }
      }
    }
    this.m_addedFieldNames.Add(field.Name);
    return base.DoAdd(field);
  }

  protected override void DoInsert(int index, PdfField field)
  {
    if (index < 0 || index > this.List.Count)
      throw new IndexOutOfRangeException();
    if (field == null)
      throw new ArgumentNullException(nameof (field));
    field.SetForm((PdfForm) this.m_form);
    PdfReferenceHolder element = new PdfReferenceHolder((IPdfWrapper) field);
    if (!(field is PdfLoadedField))
    {
      PdfArray primitive = !this.m_form.Dictionary.ContainsKey("Fields") ? new PdfArray() : this.m_form.CrossTable.GetObject(this.m_form.Dictionary["Fields"]) as PdfArray;
      if (this.m_form.IsFormContainsKids)
      {
        if (this.m_form.Fields.List[index] is PdfField)
        {
          PdfField wrapper = this.m_form.Fields.List[index] as PdfField;
          PdfReferenceHolder pdfReferenceHolder = new PdfReferenceHolder((IPdfWrapper) wrapper);
          if (wrapper.Dictionary.Items.ContainsKey(new PdfName("Parent")))
          {
            PdfArray pdfArray = ((wrapper.Dictionary["Parent"] as PdfReferenceHolder).Object as PdfDictionary).Items[new PdfName("Kids")] as PdfArray;
            for (int index1 = 0; index1 < pdfArray.Count; ++index1)
            {
              if ((pdfArray[index1] as PdfReferenceHolder).Equals((object) pdfReferenceHolder))
              {
                pdfArray.RemoveAt(index1);
                pdfArray.Insert(index1, (IPdfPrimitive) element);
              }
            }
          }
        }
      }
      else
        primitive.Insert(index, (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) field));
      this.m_form.Dictionary.SetProperty("Fields", (IPdfPrimitive) primitive);
    }
    base.DoInsert(index, field);
  }

  protected override void DoRemove(PdfField field)
  {
    if (field == null)
      throw new ArgumentNullException(nameof (field));
    this.m_form.RemoveFromDictionaries(field);
    if (this.m_fieldNames != null)
      this.m_fieldNames.Remove(field.Name);
    if (this.m_addedFieldNames != null)
      this.m_addedFieldNames.Remove(field.Name);
    if (this.m_indexedFieldNames != null)
      this.m_indexedFieldNames.Remove(field.Name);
    base.DoRemove(field);
  }

  protected override void DoRemoveAt(int index)
  {
    if (index < 0 || index > this.List.Count)
      throw new IndexOutOfRangeException();
    PdfField field = this.List[index] as PdfField;
    if (field is PdfLoadedField)
    {
      this.m_form.RemoveFromDictionaries(field);
      if (this.m_fieldNames != null)
        this.m_fieldNames.Remove(field.Name);
      if (this.m_addedFieldNames != null)
        this.m_addedFieldNames.Remove(field.Name);
      if (this.m_indexedFieldNames != null)
        this.m_indexedFieldNames.Remove(field.Name);
    }
    base.DoRemoveAt(index);
  }

  internal void RemoveContainingField(PdfReferenceHolder pageReferenceHolder)
  {
    for (int index = this.Items.Count - 1; index >= 0; --index)
    {
      if ((this.Items[index] as PdfReferenceHolder).Object is PdfDictionary fieldDictionary)
      {
        if (fieldDictionary.ContainsKey("P"))
        {
          PdfReferenceHolder pdfReferenceHolder = fieldDictionary["P"] as PdfReferenceHolder;
          if (pdfReferenceHolder != (PdfReferenceHolder) null && pdfReferenceHolder == pageReferenceHolder)
            this.DoRemoveAt(index);
        }
        else
        {
          bool removeField;
          if (fieldDictionary.ContainsKey("Kids") && this.RemoveContainingFieldItems(fieldDictionary, pageReferenceHolder, out removeField) && this.List[index] is PdfLoadedField)
          {
            if (removeField)
              this.DoRemoveAt(index);
            else
              (this.List[index] as PdfLoadedField).BeginSave();
          }
        }
      }
    }
  }

  protected override void DoClear()
  {
    int index = 0;
    for (int count = this.List.Count; index < count; ++index)
    {
      if (this.List[index] is PdfLoadedField field)
        this.m_form.RemoveFromDictionaries((PdfField) field);
    }
    this.m_addedFieldNames.Clear();
    this.m_form.TerminalFields.Clear();
    base.DoClear();
  }

  internal bool IsValidName(string name) => !this.m_addedFieldNames.Contains(name);

  internal string GetCorrectName(string name)
  {
    System.Collections.Generic.List<string> stringList = new System.Collections.Generic.List<string>();
    foreach (PdfField pdfField in this.List)
      stringList.Add(pdfField.Name);
    string correctName = name;
    int num = 0;
    while (stringList.IndexOf(correctName) != -1)
    {
      correctName = name + (object) num;
      ++num;
    }
    return correctName;
  }

  internal void AddFieldDictionary(PdfDictionary field)
  {
    if (field == null)
      throw new ArgumentNullException(nameof (field));
    this.List.Add((object) field);
    this.Items.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) field));
  }

  private void ldField_NameChanded(string name)
  {
    if (!this.IsValidName(name))
      throw new ArgumentException("Field with the same name already exist");
  }

  private int GetFieldIndex(string name)
  {
    int fieldIndex = -1;
    if (this.m_fieldNames == null)
    {
      this.m_fieldNames = new System.Collections.Generic.List<string>();
      this.m_indexedFieldNames = new System.Collections.Generic.List<string>();
      foreach (PdfField pdfField in this.List)
      {
        this.m_fieldNames.Add(pdfField.Name);
        if (pdfField.Name != null)
          this.m_indexedFieldNames.Add(pdfField.Name.Split('[')[0]);
      }
    }
    if (this.m_fieldNames.Contains(name))
      fieldIndex = this.m_fieldNames.IndexOf(name);
    else if (this.m_indexedFieldNames.Contains(name))
      fieldIndex = this.m_indexedFieldNames.IndexOf(name);
    if (fieldIndex < 0)
    {
      if (this.m_actualFieldNames == null)
      {
        this.m_actualFieldNames = new System.Collections.Generic.List<string>();
        this.m_indexedActualFieldNames = new System.Collections.Generic.List<string>();
        foreach (PdfLoadedField pdfLoadedField in this.List)
        {
          this.m_actualFieldNames.Add(pdfLoadedField.ActualFieldName);
          this.m_indexedActualFieldNames.Add(pdfLoadedField.ActualFieldName.Split('[')[0]);
        }
      }
      if (this.m_actualFieldNames.Contains(name))
        fieldIndex = this.m_actualFieldNames.IndexOf(name);
      else if (this.m_indexedActualFieldNames.Contains(name))
        fieldIndex = this.m_indexedActualFieldNames.IndexOf(name);
    }
    return fieldIndex;
  }

  private PdfField GetNamedField(string name)
  {
    PdfField namedField = (PdfField) null;
    foreach (PdfField pdfField in this.List)
    {
      if (pdfField.Name == name)
        namedField = pdfField;
    }
    return namedField;
  }

  public bool TryGetField(string fieldName, out PdfLoadedField field)
  {
    field = (PdfLoadedField) null;
    int fieldIndex = this.GetFieldIndex(fieldName);
    if (fieldIndex <= -1)
      return false;
    field = this.List[fieldIndex] as PdfLoadedField;
    return true;
  }

  public bool TryGetValue(string fieldName, out string fieldValue)
  {
    fieldValue = string.Empty;
    int fieldIndex = this.GetFieldIndex(fieldName);
    if (fieldIndex <= -1)
      return false;
    fieldValue = (this.List[fieldIndex] as PdfLoadedTextBoxField).Text;
    return true;
  }
}
