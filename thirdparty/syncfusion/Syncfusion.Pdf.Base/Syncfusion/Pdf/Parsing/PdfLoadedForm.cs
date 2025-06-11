// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.PdfLoadedForm
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using Syncfusion.Pdf.Security;
using Syncfusion.Pdf.Xfa;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

public class PdfLoadedForm : PdfForm
{
  private PdfLoadedFormFieldCollection m_fields;
  private ExportFormSettings m_formSettings = new ExportFormSettings();
  private ImportFormSettings m_settings = new ImportFormSettings();
  private PdfCrossTable m_crossTable;
  private List<PdfDictionary> m_terminalFields = new List<PdfDictionary>();
  private bool m_isModified;
  private bool m_isXFAForm;
  private bool m_hasFieldFontRetrieved;
  internal bool isUR3;
  private PdfLoadedXfaForm m_loadedXfa;
  internal System.Collections.Generic.Dictionary<string, List<PdfDictionary>> m_widgetDictionary;
  private bool m_formHasKids;
  private bool m_exportEmptyFields;
  private System.Collections.Generic.Dictionary<string, PdfDictionary> m_fdfFields;
  private System.Collections.Generic.Dictionary<string, List<string>> m_xmlFields;
  private System.Collections.Generic.Dictionary<string, List<string>> m_xdfdFields;
  private string uniquekey = string.Empty;
  private System.Collections.Generic.Dictionary<string, string> fdfRichTextTable = new System.Collections.Generic.Dictionary<string, string>();
  private System.Collections.Generic.Dictionary<string, string> m_xfdfRichText = new System.Collections.Generic.Dictionary<string, string>();

  internal PdfLoadedXfaForm LoadedXfa
  {
    get
    {
      if (this.m_loadedXfa == null)
        this.m_loadedXfa = new PdfLoadedXfaForm();
      return this.m_loadedXfa;
    }
    set => this.m_loadedXfa = value;
  }

  public PdfLoadedFormFieldCollection Fields
  {
    get
    {
      if (this.m_fields == null)
        this.m_fields = new PdfLoadedFormFieldCollection(this);
      return this.m_fields;
    }
  }

  public bool EnableXfaFormFill
  {
    get => this.m_enableXfaFormfill;
    set
    {
      this.m_enableXfaFormfill = value;
      if (!value || this.m_loadedXfa != null || !this.m_isXFAForm)
        return;
      this.m_loadedXfa = this.LoadedXfa;
      this.m_loadedXfa.Load(this.CrossTable.Document.Catalog);
      this.LoadedXfa = this.m_loadedXfa;
    }
  }

  public bool ExportEmptyFields
  {
    get => this.m_exportEmptyFields;
    set => this.m_exportEmptyFields = value;
  }

  public override bool ReadOnly
  {
    get => base.ReadOnly;
    set
    {
      base.ReadOnly = value;
      foreach (PdfField field in (PdfCollection) this.Fields)
        field.ReadOnly = value;
    }
  }

  internal override SignatureFlags SignatureFlags
  {
    get => base.SignatureFlags;
    set
    {
      base.SignatureFlags = value;
      this.IsModified = true;
      this.Dictionary.SetNumber("SigFlags", (int) value);
    }
  }

  internal override bool NeedAppearances
  {
    get => base.NeedAppearances;
    set
    {
      base.NeedAppearances = value;
      this.IsModified = true;
    }
  }

  internal override PdfResources Resources
  {
    get => base.Resources;
    set
    {
      base.Resources = value;
      this.IsModified = true;
      this.Dictionary.SetProperty("DR", (IPdfPrimitive) value);
    }
  }

  internal bool IsModified
  {
    get => this.m_isModified;
    set => this.m_isModified = value;
  }

  internal PdfCrossTable CrossTable => this.m_crossTable;

  internal List<PdfDictionary> TerminalFields
  {
    get => this.m_terminalFields;
    set => this.m_terminalFields = value;
  }

  internal bool IsXFAForm
  {
    get => this.m_isXFAForm;
    set => this.m_isXFAForm = value;
  }

  internal bool IsFormContainsKids
  {
    get => this.m_formHasKids;
    set => this.m_formHasKids = value;
  }

  internal PdfLoadedForm(PdfDictionary formDictionary, PdfCrossTable crossTable)
    : this(crossTable)
  {
    this.Initialize(formDictionary, crossTable);
  }

  internal PdfLoadedForm(PdfCrossTable crossTable)
  {
    this.m_crossTable = crossTable;
    if (crossTable.DocumentCatalog != null && crossTable.DocumentCatalog.ContainsKey("AcroForm") && PdfCrossTable.Dereference(crossTable.DocumentCatalog["AcroForm"]) is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey(nameof (NeedAppearances)))
      this.Dictionary.SetBoolean(nameof (NeedAppearances), this.NeedAppearances);
    this.CrossTable.Document.Catalog.BeginSave += new SavePdfPrimitiveEventHandler(((PdfForm) this).Dictionary_BeginSave);
    this.CrossTable.Document.Catalog.Modify();
    if (!crossTable.Document.Catalog.ContainsKey("Perms"))
      return;
    this.CheckPerms(crossTable.Document.Catalog);
  }

  internal PdfField GetField(string nodeName)
  {
    foreach (PdfField field in (PdfCollection) this.Fields)
    {
      if (field.Name.Replace("\\", string.Empty) == nodeName)
        return field;
    }
    return (PdfField) null;
  }

  private void CheckPerms(PdfCatalog catalog)
  {
    PdfDictionary pdfDictionary = (PdfDictionary) null;
    if (catalog["Perms"] is PdfDictionary)
      pdfDictionary = catalog["Perms"] as PdfDictionary;
    else if ((object) (catalog["Perms"] as PdfReferenceHolder) != null)
      pdfDictionary = (catalog["Perms"] as PdfReferenceHolder).Object as PdfDictionary;
    if (!pdfDictionary.ContainsKey("UR3"))
      return;
    this.isUR3 = true;
  }

  private void Initialize(PdfDictionary formDictionary, PdfCrossTable crossTable)
  {
    if (formDictionary == null)
      throw new ArgumentNullException("dictionary");
    if (crossTable == null)
      throw new ArgumentNullException(nameof (crossTable));
    this.Dictionary = formDictionary;
    if (this.Dictionary.ContainsKey("XFA"))
      this.m_isXFAForm = true;
    this.CreateFields();
    if (this.Dictionary.ContainsKey("NeedAppearances"))
    {
      base.NeedAppearances = (this.m_crossTable.GetObject(this.Dictionary["NeedAppearances"]) as PdfBoolean).Value;
      this.SetAppearanceDictionary = true;
    }
    else
      this.SetAppearanceDictionary = false;
    if (this.Dictionary.ContainsKey("SigFlags"))
      base.SignatureFlags = (SignatureFlags) (this.m_crossTable.GetObject(this.Dictionary["SigFlags"]) as PdfNumber).IntValue;
    if (!this.Dictionary.ContainsKey("DR") || !(PdfCrossTable.Dereference(this.Dictionary["DR"]) is PdfDictionary baseDictionary))
      return;
    PdfResources pdfResources = new PdfResources(baseDictionary);
    this.Resources = pdfResources;
    base.Resources = pdfResources;
  }

  private void CreateFields()
  {
    PdfArray fields = (PdfArray) null;
    if (this.Dictionary.ContainsKey("Fields"))
      fields = this.m_crossTable.GetObject(this.Dictionary["Fields"]) as PdfArray;
    int num = 0;
    Stack<PdfLoadedForm.NodeInfo> nodeInfoStack = new Stack<PdfLoadedForm.NodeInfo>();
    while (fields != null)
    {
      for (; num < fields.Count; ++num)
      {
        PdfDictionary pdfDictionary1 = this.m_crossTable.GetObject(fields[num]) as PdfDictionary;
        kids = (PdfArray) null;
        if (pdfDictionary1 != null && pdfDictionary1.ContainsKey("Kids") && this.m_crossTable.GetObject(pdfDictionary1["Kids"]) is PdfArray kids)
        {
          for (int index = 0; index < kids.Count; ++index)
          {
            if (PdfCrossTable.Dereference(kids[index]) is PdfDictionary pdfDictionary2 && !pdfDictionary2.ContainsKey("Parent"))
              pdfDictionary2["Parent"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary1);
          }
        }
        if (kids == null)
        {
          if (pdfDictionary1 != null && !this.m_terminalFields.Contains(pdfDictionary1))
            this.m_terminalFields.Add(pdfDictionary1);
        }
        else if (!pdfDictionary1.ContainsKey("FT") || this.IsNode(kids))
        {
          PdfLoadedForm.NodeInfo nodeInfo = new PdfLoadedForm.NodeInfo(fields, num);
          nodeInfoStack.Push(nodeInfo);
          this.IsFormContainsKids = true;
          num = -1;
          fields = kids;
        }
        else
          this.m_terminalFields.Add(pdfDictionary1);
      }
      if (nodeInfoStack.Count == 0)
        break;
      PdfLoadedForm.NodeInfo nodeInfo1 = nodeInfoStack.Pop();
      fields = nodeInfo1.Fields;
      num = nodeInfo1.Count + 1;
    }
  }

  private bool IsNode(PdfArray kids)
  {
    bool flag = false;
    if (kids.Count >= 1 && this.m_crossTable.GetObject(kids[0]) is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey("Subtype") && (this.m_crossTable.GetObject(pdfDictionary["Subtype"]) as PdfName).Value != "Widget")
      flag = true;
    return flag;
  }

  public void ExportData(string fileName, DataFormat dataFormat, string formName)
  {
    FileStream fileStream = (FileStream) null;
    try
    {
      fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
      this.ExportData((Stream) fileStream, dataFormat, formName);
    }
    catch
    {
      throw;
    }
    finally
    {
      fileStream?.Close();
    }
  }

  public void ExportData(string fileName, ExportFormSettings settings)
  {
    this.m_formSettings = settings;
    this.m_formSettings.AsPerSpecification = true;
    this.ExportData(fileName, this.m_formSettings.DataFormat, this.m_formSettings.FormName);
  }

  public void ExportData(Stream stream, DataFormat dataFormat, string formName)
  {
    if (dataFormat == DataFormat.Xml)
      this.ExportDataXML(stream);
    if (dataFormat == DataFormat.Fdf)
      this.ExportDataFDF(stream, formName);
    if (dataFormat == DataFormat.XFdf)
      this.ExportDataXFDF(stream, formName);
    if (dataFormat != DataFormat.Json)
      return;
    this.ExportDataJSON(stream);
  }

  public void ExportData(Stream stream, ExportFormSettings settings)
  {
    this.m_formSettings = settings;
    this.m_formSettings.AsPerSpecification = true;
    this.ExportData(stream, this.m_formSettings.DataFormat, this.m_formSettings.FormName);
  }

  public void FlattenFields()
  {
    bool flatten = this.Flatten;
    this.Flatten = true;
    this.FlttenFormFields();
    if (this.m_fields.Count == 0 && !(this.CrossTable.Document as PdfLoadedDocument).m_isXfaDocument)
    {
      int index = this.CrossTable.PdfObjects.IndexOf((IPdfPrimitive) this.Dictionary);
      this.Dictionary.Clear();
      if (index != -1)
        this.CrossTable.PdfObjects.Remove(index);
    }
    else if (this.SetAppearanceDictionary && (this.Dictionary.ContainsKey("NeedAppearances") || base.SignatureFlags == SignatureFlags.None && this.NeedAppearances))
      this.Dictionary.SetBoolean("NeedAppearances", this.NeedAppearances);
    this.Flatten = flatten;
  }

  private void ExportDataXFDF(Stream stream, string formName)
  {
    XFdfDocument xfdfDocument = new XFdfDocument(formName);
    this.uniquekey = Guid.NewGuid().ToString();
    for (int index = 0; index < this.Fields.Count; ++index)
    {
      PdfLoadedField field = (PdfLoadedField) this.Fields[index];
      if (field.Export)
      {
        field.ExportEmptyField = this.ExportEmptyFields;
        PdfName pdfName1 = PdfLoadedField.GetValue(field.Dictionary, field.CrossTable, "FT", true) as PdfName;
        PdfDictionary font = (PdfDictionary) null;
        string fieldName = field.Name;
        field.GetEncodedFontDictionary(field.Dictionary, out font);
        if (font != null)
          fieldName = field.UpdateEncodedValue(fieldName, font);
        if (pdfName1 != (PdfName) null)
        {
          switch (pdfName1.Value)
          {
            case "Tx":
              PdfString pdfString1 = PdfLoadedField.GetValue(field.Dictionary, field.CrossTable, "V", true) as PdfString;
              if (this.m_formSettings.AsPerSpecification)
              {
                if (field.Dictionary.ContainsKey("RV"))
                {
                  if (PdfLoadedField.GetValue(field.Dictionary, field.CrossTable, "RV", true) is PdfString pdfString2)
                  {
                    pdfString2.Value += this.uniquekey;
                    xfdfDocument.SetFields((object) fieldName, (object) pdfString2.Value, this.uniquekey);
                    continue;
                  }
                  continue;
                }
                if (PdfLoadedField.GetValue(field.Dictionary, field.CrossTable, "V", true) is PdfString pdfString3)
                {
                  pdfString3.Value = field.UpdateEncodedValue(pdfString3.Value, font);
                  string str1 = pdfString3.Value;
                  if (field is PdfLoadedTextBoxField loadedTextBoxField && loadedTextBoxField.Multiline)
                  {
                    string str2 = str1.Replace("\n", "").Replace("\r", "\r\n");
                    pdfString3.Value = str2;
                  }
                  xfdfDocument.SetFields((object) fieldName, (object) pdfString3.Value);
                  continue;
                }
                continue;
              }
              if (pdfString1 != null)
              {
                pdfString1.Value = field.UpdateEncodedValue(pdfString1.Value, font);
                xfdfDocument.SetFields((object) fieldName, (object) pdfString1.Value);
                continue;
              }
              if (this.ExportEmptyFields)
              {
                xfdfDocument.SetFields((object) fieldName, (object) "");
                continue;
              }
              continue;
            case "Ch":
              if (this.m_formSettings.AsPerSpecification)
              {
                string empty = string.Empty;
                if (field.GetType().Name == "PdfLoadedListBoxField")
                {
                  if (PdfLoadedField.GetValue(field.Dictionary, field.CrossTable, "V", true) is PdfArray Fieldvalue1)
                  {
                    xfdfDocument.SetFields((object) fieldName, (object) Fieldvalue1);
                    continue;
                  }
                  if (PdfLoadedField.GetValue(field.Dictionary, field.CrossTable, "V", true) is PdfString pdfString4)
                  {
                    pdfString4.Value = field.UpdateEncodedValue(pdfString4.Value, font);
                    xfdfDocument.SetFields((object) fieldName, (object) pdfString4.Value);
                    continue;
                  }
                  if (pdfString4 == null && field.Dictionary.ContainsKey("I"))
                  {
                    string str = (field as PdfLoadedListBoxField).SelectedValue[0];
                    string Fieldvalue2 = field.UpdateEncodedValue(str, font);
                    xfdfDocument.SetFields((object) fieldName, (object) Fieldvalue2);
                    continue;
                  }
                  continue;
                }
                PdfName pdfName2 = PdfLoadedField.GetValue(field.Dictionary, field.CrossTable, "V", true) as PdfName;
                if (pdfName2 != (PdfName) null)
                {
                  xfdfDocument.SetFields((object) fieldName, (object) pdfName2.Value);
                  continue;
                }
                if (PdfLoadedField.GetValue(field.Dictionary, field.CrossTable, "V", true) is PdfString pdfString5)
                {
                  pdfString5.Value = field.UpdateEncodedValue(pdfString5.Value, font);
                  xfdfDocument.SetFields((object) fieldName, (object) pdfString5.Value);
                  continue;
                }
                if (pdfString5 == null && field.Dictionary.ContainsKey("I"))
                {
                  string selectedValue = (field as PdfLoadedComboBoxField).SelectedValue;
                  string Fieldvalue = field.UpdateEncodedValue(selectedValue, font);
                  xfdfDocument.SetFields((object) fieldName, (object) Fieldvalue);
                  continue;
                }
                continue;
              }
              IPdfPrimitive fieldPrimitive = PdfLoadedField.GetValue(field.Dictionary, field.CrossTable, "V", true);
              string str3 = (string) null;
              if (fieldPrimitive != null)
                str3 = this.GetExportValue(fieldPrimitive);
              if (field.GetType().Name == "PdfLoadedListBoxField")
              {
                if (fieldPrimitive == null && field.Dictionary.ContainsKey("I"))
                  str3 = (field as PdfLoadedListBoxField).SelectedValue[0];
                if (!string.IsNullOrEmpty(str3))
                {
                  string Fieldvalue = field.UpdateEncodedValue(str3, font);
                  xfdfDocument.SetFields((object) fieldName, (object) Fieldvalue);
                  continue;
                }
                if (this.ExportEmptyFields)
                {
                  xfdfDocument.SetFields((object) fieldName, (object) "");
                  continue;
                }
                continue;
              }
              if (fieldPrimitive == null && field.Dictionary.ContainsKey("I"))
                str3 = (field as PdfLoadedComboBoxField).SelectedValue;
              if (!string.IsNullOrEmpty(str3))
              {
                string Fieldvalue = field.UpdateEncodedValue(str3, font);
                xfdfDocument.SetFields((object) fieldName, (object) Fieldvalue);
                continue;
              }
              if (this.ExportEmptyFields)
              {
                xfdfDocument.SetFields((object) fieldName, (object) "");
                continue;
              }
              continue;
            case "Btn":
              IPdfPrimitive buttonFieldPrimitive = PdfLoadedField.GetValue(field.Dictionary, field.CrossTable, "V", true);
              PdfLoadedRadioButtonListField radioButtonListField = (PdfLoadedRadioButtonListField) null;
              if (buttonFieldPrimitive != null)
              {
                string exportValue = field.GetExportValue(field, buttonFieldPrimitive);
                if (!string.IsNullOrEmpty(exportValue))
                {
                  if (this.m_formSettings.AsPerSpecification)
                    exportValue = this.HexToString(exportValue);
                  if (field is PdfLoadedRadioButtonListField)
                    radioButtonListField = field as PdfLoadedRadioButtonListField;
                  if (!field.Dictionary.ContainsKey("Opt") || radioButtonListField != null && radioButtonListField.SelectedIndex == -1)
                  {
                    string Fieldvalue = field.UpdateEncodedValue(exportValue, font);
                    xfdfDocument.SetFields((object) fieldName, (object) Fieldvalue);
                    continue;
                  }
                  if (field.Dictionary != null && field.Dictionary.ContainsKey("Opt"))
                  {
                    PdfArray pdfArray = PdfCrossTable.Dereference(field.Dictionary["Opt"]) as PdfArray;
                    int result = 0;
                    try
                    {
                      int.TryParse(exportValue, out result);
                      if (pdfArray != null)
                      {
                        PdfString pdfString6 = radioButtonListField == null ? PdfCrossTable.Dereference(pdfArray[result]) as PdfString : PdfCrossTable.Dereference(pdfArray[radioButtonListField.SelectedIndex]) as PdfString;
                        if (pdfString6 != null)
                          exportValue = pdfString6.Value;
                      }
                      if (!string.IsNullOrEmpty(exportValue))
                      {
                        string Fieldvalue = field.UpdateEncodedValue(exportValue, font);
                        xfdfDocument.SetFields((object) fieldName, (object) Fieldvalue);
                        continue;
                      }
                      continue;
                    }
                    catch
                    {
                      continue;
                    }
                  }
                  else
                    continue;
                }
                else
                {
                  switch (field)
                  {
                    case PdfLoadedRadioButtonListField _:
                    case PdfLoadedCheckBoxField _:
                      if (this.ExportEmptyFields)
                      {
                        xfdfDocument.SetFields((object) fieldName, (object) "");
                        continue;
                      }
                      xfdfDocument.SetFields((object) fieldName, (object) "Off");
                      continue;
                    default:
                      continue;
                  }
                }
              }
              else
              {
                if (field is PdfLoadedRadioButtonListField)
                {
                  xfdfDocument.SetFields((object) fieldName, (object) field.GetAppearanceStateValue(field));
                  continue;
                }
                PdfDictionary widgetAnnotation = field.GetWidgetAnnotation(field.Dictionary, field.CrossTable);
                if (widgetAnnotation != null)
                {
                  PdfName pdfName3 = widgetAnnotation["AS"] as PdfName;
                  if (pdfName3 != (PdfName) null)
                  {
                    xfdfDocument.SetFields((object) fieldName, (object) pdfName3.Value);
                    continue;
                  }
                  if (this.ExportEmptyFields)
                  {
                    xfdfDocument.SetFields((object) fieldName, (object) "");
                    continue;
                  }
                  continue;
                }
                continue;
              }
            default:
              continue;
          }
        }
      }
    }
    if (this.m_formSettings.AsPerSpecification)
    {
      xfdfDocument.trailerId = this.CrossTable.Trailer["ID"];
      xfdfDocument.Save(stream, this.m_formSettings.AsPerSpecification);
    }
    else
      xfdfDocument.Save(stream);
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

  internal PdfLoadedXfaField GetXfaField(string fieldName)
  {
    return this.LoadedXfa.TryGetFieldByCompleteName(fieldName);
  }

  private void ExportDataFDF(Stream stream, string formName)
  {
    BinaryWriter binaryWriter = new BinaryWriter(stream);
    int objectid = 1;
    System.Collections.Generic.Dictionary<string, object> table = new System.Collections.Generic.Dictionary<string, object>();
    System.Collections.Generic.Dictionary<string, object> map = new System.Collections.Generic.Dictionary<string, object>();
    if (this.m_formSettings.AsPerSpecification)
    {
      PdfString pdfString1 = new PdfString("%FDF-1.2\r");
      pdfString1.Encode = PdfString.ForceEncoding.ASCII;
      byte[] bytes1 = Encoding.GetEncoding("windows-1252").GetBytes(pdfString1.Value);
      stream.Write(bytes1, 0, bytes1.Length);
      PdfString pdfString2 = new PdfString("%âãÏÓ\r\n");
      pdfString1.Encode = PdfString.ForceEncoding.ASCII;
      byte[] bytes2 = Encoding.GetEncoding("windows-1252").GetBytes(pdfString2.Value);
      stream.Write(bytes2, 0, bytes2.Length);
      StringBuilder stringBuilder1 = new StringBuilder();
      stringBuilder1.AppendFormat("1 0 obj\r");
      stringBuilder1.AppendFormat("<</FDF<</F(");
      byte[] bytes3 = Encoding.GetEncoding("windows-1252").GetBytes(new PdfString(stringBuilder1.ToString())
      {
        Encode = PdfString.ForceEncoding.ASCII
      }.Value);
      stream.Write(bytes3, 0, bytes3.Length);
      byte[] bytes4 = Encoding.GetEncoding("windows-1252").GetBytes(new PdfString(formName)
      {
        Encode = PdfString.ForceEncoding.ASCII
      }.Value);
      stream.Write(bytes4, 0, bytes4.Length);
      StringBuilder stringBuilder2 = new StringBuilder();
      stringBuilder2.AppendFormat(")");
      byte[] bytes5 = Encoding.GetEncoding("windows-1252").GetBytes(new PdfString(stringBuilder2.ToString())
      {
        Encode = PdfString.ForceEncoding.ASCII
      }.Value);
      stream.Write(bytes5, 0, bytes5.Length);
    }
    else
    {
      byte[] bytes = Encoding.GetEncoding("windows-1252").GetBytes(new PdfString("%FDF-1.2\n")
      {
        Encode = PdfString.ForceEncoding.ASCII
      }.Value);
      stream.Write(bytes, 0, bytes.Length);
    }
    for (int index = 0; index < this.Fields.Count; ++index)
    {
      PdfLoadedField field = (PdfLoadedField) this.Fields[index];
      field.ExportEmptyField = this.ExportEmptyFields;
      if (field.Export)
      {
        if (this.m_formSettings.AsPerSpecification)
          field.ExportField(table);
        else
          field.ExportField(stream, ref objectid);
      }
    }
    if (this.m_formSettings.AsPerSpecification)
    {
      if (table.Count > 0)
        map = this.GetElements(table);
      PdfArray array = this.GroupFieldNames(map);
      PdfWriter writer = new PdfWriter(stream);
      if (array.Count > 0)
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendFormat("/Fields");
        byte[] bytes = Encoding.GetEncoding("windows-1252").GetBytes(new PdfString(stringBuilder.ToString())
        {
          Encode = PdfString.ForceEncoding.ASCII
        }.Value);
        stream.Write(bytes, 0, bytes.Length);
        this.AppendArrayElements(array, writer);
      }
      StringBuilder stringBuilder3 = new StringBuilder();
      stringBuilder3.Append("/ID[");
      if (this.CrossTable.Trailer["ID"] is PdfArray pdfArray)
      {
        for (int index = 0; index < pdfArray.Count; ++index)
        {
          byte[] bytes = Encoding.GetEncoding("windows-1252").GetBytes(new PdfString((pdfArray[index] as PdfString).Value)
          {
            Encode = PdfString.ForceEncoding.ASCII
          }.Value);
          stringBuilder3.AppendFormat("<{0}>", (object) PdfString.BytesToHex(bytes));
        }
      }
      byte[] bytes6 = Encoding.GetEncoding("windows-1252").GetBytes(new PdfString(stringBuilder3.ToString())
      {
        Encode = PdfString.ForceEncoding.ASCII
      }.Value);
      stream.Write(bytes6, 0, bytes6.Length);
      stream.Flush();
      StringBuilder stringBuilder4 = new StringBuilder();
      stringBuilder4.Append("]");
      stringBuilder4.AppendFormat("/UF({0})>>", (object) formName);
      stringBuilder4.Append("/Type/Catalog>>\r");
      stringBuilder4.Append("endobj\r");
      stringBuilder4.Append("trailer\r\n");
      stringBuilder4.AppendFormat("<</Root 1 0 R>>\r\n");
      stringBuilder4.AppendFormat("%%EOF\r\n");
      byte[] bytes7 = Encoding.GetEncoding("windows-1252").GetBytes(new PdfString(stringBuilder4.ToString())
      {
        Encode = PdfString.ForceEncoding.ASCII
      }.Value);
      stream.Write(bytes7, 0, bytes7.Length);
      stream.Flush();
    }
    else
    {
      StringBuilder stringBuilder = new StringBuilder();
      byte[] bytes8 = Encoding.GetEncoding("windows-1252").GetBytes(new PdfString(formName)
      {
        Encode = PdfString.ForceEncoding.ASCII
      }.Value);
      stringBuilder.AppendFormat("{0} 0 obj<</F <{1}>  /Fields [", (object) objectid, (object) PdfString.BytesToHex(bytes8));
      for (int index = 0; index < this.Fields.Count; ++index)
      {
        PdfLoadedField field = (PdfLoadedField) this.Fields[index];
        if (field.Export && field.ObjectID != 0)
          stringBuilder.AppendFormat("{0} 0 R ", (object) field.ObjectID);
      }
      stringBuilder.Append("]>>endobj\n");
      stringBuilder.AppendFormat("{0} 0 obj<</Version /1.4 /FDF {1} 0 R>>endobj\n", (object) (objectid + 1), (object) objectid);
      stringBuilder.AppendFormat("trailer\n<</Root {0} 0 R>>\n", (object) (objectid + 1));
      byte[] bytes9 = Encoding.GetEncoding("windows-1252").GetBytes(new PdfString(stringBuilder.ToString())
      {
        Encode = PdfString.ForceEncoding.ASCII
      }.Value);
      stream.Write(bytes9, 0, bytes9.Length);
      stream.Flush();
    }
  }

  private void AppendArrayElements(PdfArray array, PdfWriter writer)
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
        this.AppendElement(element, writer);
      }
    }
    writer.Write("]");
  }

  private void AppendElement(IPdfPrimitive element, PdfWriter writer)
  {
    if (element is PdfNumber)
      writer.Write((element as PdfNumber).FloatValue);
    else if ((object) (element as PdfName) != null)
    {
      writer.Write((element as PdfName).ToString());
    }
    else
    {
      switch (element)
      {
        case PdfString _:
          byte[] preamble = Encoding.BigEndianUnicode.GetPreamble();
          string s = (element as PdfString).Value;
          bool flag = this.NonAsciiCheck(s);
          byte[] bytes1 = Encoding.GetEncoding("windows-1252").GetBytes("(");
          writer.m_stream.Write(bytes1, 0, bytes1.Length);
          byte[] buffer = !flag ? PdfString.EscapeSymbols(Encoding.GetEncoding("windows-1252").GetBytes(s), true) : this.EscapeSymbols(Encoding.BigEndianUnicode.GetBytes(s));
          if (flag)
            writer.m_stream.Write(preamble, 0, preamble.Length);
          writer.m_stream.Write(buffer, 0, buffer.Length);
          byte[] bytes2 = Encoding.GetEncoding("windows-1252").GetBytes(")");
          writer.m_stream.Write(bytes2, 0, bytes2.Length);
          break;
        case PdfBoolean _:
          writer.Write((element as PdfBoolean).Value ? "true" : "false");
          break;
        case PdfArray _:
          this.AppendArrayElements(element as PdfArray, writer);
          break;
        case PdfDictionary _:
          writer.Write("<<");
          this.GetEntriesInDictionary(element as PdfDictionary, writer);
          writer.Write(">>");
          break;
      }
    }
  }

  private void GetEntriesInDictionary(PdfDictionary dictionary, PdfWriter writer)
  {
    foreach (PdfName key in dictionary.Keys)
    {
      writer.Write(key.ToString());
      IPdfPrimitive pdfPrimitive = dictionary[key];
      if (pdfPrimitive is PdfString)
      {
        byte[] preamble = Encoding.BigEndianUnicode.GetPreamble();
        string str = (pdfPrimitive as PdfString).Value;
        bool flag = this.NonAsciiCheck(str);
        byte[] bytes1 = Encoding.GetEncoding("windows-1252").GetBytes("(");
        writer.m_stream.Write(bytes1, 0, bytes1.Length);
        byte[] buffer = !flag ? PdfString.EscapeSymbols(PdfString.StringToByte(str), true) : this.EscapeSymbols(Encoding.BigEndianUnicode.GetBytes(str));
        if (flag)
          writer.m_stream.Write(preamble, 0, preamble.Length);
        writer.m_stream.Write(buffer, 0, buffer.Length);
        byte[] bytes2 = Encoding.GetEncoding("windows-1252").GetBytes(")");
        writer.m_stream.Write(bytes2, 0, bytes2.Length);
      }
      else if ((object) (pdfPrimitive as PdfName) != null)
      {
        writer.Write((pdfPrimitive as PdfName).ToString());
      }
      else
      {
        switch (pdfPrimitive)
        {
          case PdfArray _:
            this.AppendArrayElements(pdfPrimitive as PdfArray, writer);
            continue;
          case PdfNumber _:
            writer.Write(" " + (object) (pdfPrimitive as PdfNumber).FloatValue);
            continue;
          case PdfBoolean _:
            writer.Write(" " + ((pdfPrimitive as PdfBoolean).Value ? "true" : "false"));
            continue;
          case PdfDictionary _:
            writer.Write("<<");
            this.GetEntriesInDictionary(pdfPrimitive as PdfDictionary, writer);
            writer.Write(">>");
            continue;
          default:
            continue;
        }
      }
    }
  }

  private bool NonAsciiCheck(string value)
  {
    foreach (char ch in value)
    {
      if (ch > 'ÿ')
        return true;
    }
    return false;
  }

  internal byte[] EscapeSymbols(byte[] data)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    MemoryStream memoryStream = new MemoryStream();
    int index = 0;
    for (int length = data.Length; index < length; ++index)
    {
      byte num = data[index];
      switch (num)
      {
        case 10:
          memoryStream.WriteByte((byte) 92);
          memoryStream.WriteByte((byte) 110);
          break;
        case 13:
          memoryStream.WriteByte((byte) 92);
          memoryStream.WriteByte((byte) 114);
          break;
        case 40:
        case 41:
          memoryStream.WriteByte((byte) 92);
          memoryStream.WriteByte(num);
          break;
        case 92:
          memoryStream.WriteByte((byte) 92);
          memoryStream.WriteByte(num);
          break;
        default:
          memoryStream.WriteByte(num);
          break;
      }
    }
    byte[] bytes = PdfStream.StreamToBytes((Stream) memoryStream);
    memoryStream.Dispose();
    return bytes;
  }

  private string GetFormattedString(string value)
  {
    string formattedString = "";
    foreach (int num in value)
    {
      switch (num)
      {
        case 40:
        case 41:
          formattedString += "\\";
          break;
      }
      formattedString += (string) (object) (char) num;
    }
    return formattedString;
  }

  private PdfArray GroupFieldNames(System.Collections.Generic.Dictionary<string, object> map)
  {
    PdfArray pdfArray = new PdfArray();
    foreach (KeyValuePair<string, object> keyValuePair in map)
    {
      string key = keyValuePair.Key;
      object obj = keyValuePair.Value;
      PdfDictionary element = new PdfDictionary();
      element.SetProperty("T", (IPdfPrimitive) new PdfString(key));
      if (obj is System.Collections.Generic.Dictionary<string, object>)
        element.SetProperty("Kids", (IPdfPrimitive) this.GroupFieldNames((System.Collections.Generic.Dictionary<string, object>) obj));
      else if (obj is List<PdfString> pdfStringList && pdfStringList.Count > 1)
      {
        PdfString primitive1 = pdfStringList[0];
        PdfString primitive2 = pdfStringList[1];
        element.SetProperty("RV", (IPdfPrimitive) primitive1);
        element.SetProperty("V", (IPdfPrimitive) primitive2);
      }
      else
      {
        if (pdfStringList != null)
          obj = (object) pdfStringList[0];
        if (obj is PdfString)
          element.SetProperty("V", (IPdfPrimitive) (obj as PdfString));
        if ((object) (obj as PdfName) != null)
          element.SetProperty("V", (IPdfPrimitive) (obj as PdfName));
        if (obj is PdfArray)
          element.SetProperty("V", (IPdfPrimitive) (obj as PdfArray));
      }
      pdfArray.Add((IPdfPrimitive) element);
    }
    return pdfArray;
  }

  internal System.Collections.Generic.Dictionary<string, object> GetElements(
    System.Collections.Generic.Dictionary<string, object> table)
  {
    System.Collections.Generic.Dictionary<string, object> elements = new System.Collections.Generic.Dictionary<string, object>();
    foreach (KeyValuePair<string, object> keyValuePair in table)
    {
      object key = (object) keyValuePair.Key;
      object obj = keyValuePair.Value;
      System.Collections.Generic.Dictionary<string, object> dictionary1 = elements;
      if (key.ToString().Contains("."))
      {
        string[] strArray = key.ToString().Split('.');
        for (int index = 0; index < strArray.Length; ++index)
        {
          if (dictionary1.ContainsKey(strArray[index]))
          {
            this.GetElements((System.Collections.Generic.Dictionary<string, object>) dictionary1[strArray[index]]);
            dictionary1 = (System.Collections.Generic.Dictionary<string, object>) dictionary1[strArray[index]];
          }
          else if (index == strArray.Length - 1)
          {
            dictionary1.Add(strArray[index], obj);
          }
          else
          {
            System.Collections.Generic.Dictionary<string, object> dictionary2 = new System.Collections.Generic.Dictionary<string, object>();
            dictionary1.Add(strArray[index], (object) dictionary2);
            dictionary1 = (System.Collections.Generic.Dictionary<string, object>) dictionary1[strArray[index]];
          }
        }
      }
      else
        dictionary1.Add(key as string, obj);
    }
    return elements;
  }

  internal void ExportDataXML(Stream stream)
  {
    XmlTextWriter textWriter = new XmlTextWriter(stream, (Encoding) new UTF8Encoding());
    textWriter.Formatting = Formatting.Indented;
    textWriter.WriteStartDocument();
    if (this.m_formSettings.AsPerSpecification)
    {
      textWriter.WriteStartElement("fields", "");
      textWriter.WriteAttributeString("xmlns", "xfdf", (string) null, "http://ns.adobe.com/xfdf-transition/");
    }
    else
      textWriter.WriteStartElement("Fields", "");
    System.Collections.Generic.Dictionary<object, object> table = new System.Collections.Generic.Dictionary<object, object>();
    System.Collections.Generic.Dictionary<object, object> dictionary = new System.Collections.Generic.Dictionary<object, object>();
    this.uniquekey = Guid.NewGuid().ToString();
    for (int index = 0; index < this.Fields.Count; ++index)
    {
      PdfLoadedField field = (PdfLoadedField) this.Fields[index];
      if (field.Export)
      {
        field.ExportEmptyField = this.ExportEmptyFields;
        if (this.m_formSettings.AsPerSpecification)
          field.ExportField(textWriter, table, this.uniquekey);
        else
          field.ExportField(textWriter);
      }
    }
    if (this.m_formSettings.AsPerSpecification && table.Count > 0)
    {
      System.Collections.Generic.Dictionary<object, object> elements = this.GetElements(table);
      foreach (KeyValuePair<object, object> keyValuePair in elements)
      {
        string str = (string) keyValuePair.Key;
        bool flag1 = false;
        if (str.EndsWith(this.uniquekey))
        {
          str = str.Remove(str.Length - this.uniquekey.Length, this.uniquekey.Length);
          flag1 = true;
        }
        object obj = elements[(object) keyValuePair.Key.ToString()];
        if (obj is System.Collections.Generic.Dictionary<object, object>)
        {
          if (!XmlReader.IsName(str.ToString()) || str.ToString().Contains(":") || str.ToString().Contains(" "))
          {
            if (str.ToString().Contains(" ") && XmlReader.IsName(str.ToString().Replace(" ", "")) && !str.ToString().Contains(":"))
            {
              textWriter.WriteStartElement(str.ToString().Replace(" ", ""));
              if (!flag1)
                textWriter.WriteAttributeString("xfdf", "original", (string) null, str.ToString());
            }
            else
            {
              textWriter.WriteStartElement("group");
              if (!flag1)
                textWriter.WriteAttributeString("xfdf", "original", (string) null, str);
            }
          }
          else
            textWriter.WriteStartElement(str.ToString());
          if (flag1)
          {
            textWriter.WriteAttributeString("xmlns", "xfa", (string) null, "http://www.xfa.org/schema/xfa-data/1.0/");
            textWriter.WriteAttributeString("xfa", "contentType", (string) null, "text/html");
            textWriter.WriteAttributeString("xfdf", "original", (string) null, str);
          }
          this.WriteFieldName((System.Collections.Generic.Dictionary<object, object>) obj, textWriter);
          textWriter.WriteEndElement();
        }
        else if (obj.GetType().Name == "PdfArray")
        {
          PdfArray pdfArray = obj as PdfArray;
          if (str.ToString().Contains(" ") && XmlReader.IsName(str.ToString().Replace(" ", "")) && !str.ToString().Contains(":"))
          {
            textWriter.WriteStartElement(str.ToString().Replace(" ", ""));
            textWriter.WriteAttributeString("xfdf", "original", (string) null, str.ToString());
          }
          else if (!XmlReader.IsName(str.ToString()) || str.ToString().Contains(":"))
          {
            textWriter.WriteStartElement("field");
            textWriter.WriteAttributeString("xfdf", "original", (string) null, str.ToString());
          }
          else
            textWriter.WriteStartElement(str.ToString());
          foreach (PdfString pdfString in pdfArray)
          {
            textWriter.WriteStartElement("value");
            textWriter.WriteString(pdfString.Value.ToString());
            textWriter.WriteEndElement();
          }
          textWriter.WriteEndElement();
        }
        else
        {
          bool flag2 = false;
          if (!XmlReader.IsName(str.ToString()) || str.ToString().Contains(":"))
          {
            if (str.ToString().Contains(" ") && XmlReader.IsName(str.ToString().Replace(" ", "")) && !str.ToString().Contains(":"))
              textWriter.WriteStartElement(str.ToString().Replace(" ", ""));
            else
              textWriter.WriteStartElement("field");
            if (!flag1)
              textWriter.WriteAttributeString("xfdf", "original", (string) null, str.ToString());
          }
          else if (str.ToString().Contains(" "))
          {
            textWriter.WriteStartElement(str.ToString().Replace(" ", ""));
            if (!flag1)
              textWriter.WriteAttributeString("xfdf", "original", (string) null, str.ToString());
          }
          else
          {
            textWriter.WriteStartElement(str.ToString());
            flag2 = true;
          }
          if (flag1)
          {
            textWriter.WriteAttributeString("xmlns", "xfa", (string) null, "http://www.xfa.org/schema/xfa-data/1.0/");
            textWriter.WriteAttributeString("xfa", "contentType", (string) null, "text/html");
            if (!flag2)
              textWriter.WriteAttributeString("xfdf", "original", (string) null, str);
          }
          textWriter.WriteString(obj.ToString());
          textWriter.WriteEndElement();
        }
      }
    }
    textWriter.WriteEndElement();
    textWriter.Flush();
  }

  private void WriteFieldName(System.Collections.Generic.Dictionary<object, object> value, XmlTextWriter textWriter)
  {
    foreach (KeyValuePair<object, object> keyValuePair in value)
    {
      string str = keyValuePair.Key.ToString();
      bool flag1 = false;
      if (str.EndsWith(this.uniquekey))
      {
        str = str.Remove(str.Length - this.uniquekey.Length, this.uniquekey.Length);
        flag1 = true;
      }
      if (keyValuePair.Value is System.Collections.Generic.Dictionary<object, object>)
      {
        if (!XmlReader.IsName(str.ToString()) || str.ToString().Contains(":") || str.ToString().Contains(" "))
        {
          if (str.ToString().Contains(" ") && XmlReader.IsName(str.ToString().Replace(" ", "")) && !str.ToString().Contains(":"))
          {
            textWriter.WriteStartElement(str.ToString().Replace(" ", ""));
            if (!flag1)
              textWriter.WriteAttributeString("xfdf", "original", (string) null, str.ToString());
          }
          else
          {
            textWriter.WriteStartElement("group");
            if (!flag1)
              textWriter.WriteAttributeString("xfdf", "original", (string) null, str);
          }
        }
        else
          textWriter.WriteStartElement(str.ToString());
        if (flag1)
        {
          textWriter.WriteAttributeString("xmlns", "xfa", (string) null, "http://www.xfa.org/schema/xfa-data/1.0/");
          textWriter.WriteAttributeString("xfa", "contentType", (string) null, "text/html");
          textWriter.WriteAttributeString("xfdf", "original", (string) null, str);
        }
        this.WriteFieldName((System.Collections.Generic.Dictionary<object, object>) keyValuePair.Value, textWriter);
        textWriter.WriteEndElement();
      }
      else if (keyValuePair.Value.GetType().Name == "PdfArray")
      {
        PdfArray pdfArray = keyValuePair.Value as PdfArray;
        if (str.ToString().Contains(" ") && XmlReader.IsName(str.ToString().Replace(" ", "")) && !str.ToString().Contains(":"))
        {
          textWriter.WriteStartElement(str.ToString().Replace(" ", ""));
          textWriter.WriteAttributeString("xfdf", "original", (string) null, str.ToString());
        }
        else if (!XmlReader.IsName(str.ToString()) || str.ToString().Contains(":"))
        {
          textWriter.WriteStartElement("field");
          textWriter.WriteAttributeString("xfdf", "original", (string) null, str.ToString());
        }
        else
          textWriter.WriteStartElement(str.ToString());
        foreach (PdfString pdfString in pdfArray)
        {
          textWriter.WriteStartElement(nameof (value));
          textWriter.WriteString(pdfString.Value.ToString());
          textWriter.WriteEndElement();
        }
        textWriter.WriteEndElement();
      }
      else
      {
        bool flag2 = false;
        if (!XmlReader.IsName(str.ToString()) || str.ToString().Contains(":"))
        {
          if (str.ToString().Contains(" ") && XmlReader.IsName(str.ToString().Replace(" ", "")) && !str.ToString().Contains(":"))
            textWriter.WriteStartElement(str.ToString().Replace(" ", ""));
          else
            textWriter.WriteStartElement("field");
          if (!flag1)
            textWriter.WriteAttributeString("xfdf", "original", (string) null, str.ToString());
        }
        else if (str.ToString().Contains(" "))
        {
          textWriter.WriteStartElement(str.ToString().Replace(" ", ""));
          if (!flag1)
            textWriter.WriteAttributeString("xfdf", "original", (string) null, str.ToString());
        }
        else
        {
          textWriter.WriteStartElement(str.ToString());
          flag2 = true;
        }
        if (flag1)
        {
          textWriter.WriteAttributeString("xmlns", "xfa", (string) null, "http://www.xfa.org/schema/xfa-data/1.0/");
          textWriter.WriteAttributeString("xfa", "contentType", (string) null, "text/html");
          if (!flag2)
            textWriter.WriteAttributeString("xfdf", "original", (string) null, str);
        }
        textWriter.WriteString(keyValuePair.Value.ToString());
        textWriter.WriteEndElement();
      }
    }
  }

  private bool HasEscapeCharacter(string text)
  {
    if (text != null & text.Length > 0 && char.IsDigit(text.ToCharArray()[0]))
      return true;
    string[] strArray = new string[20]
    {
      "?",
      "!",
      "/",
      "\\",
      "#",
      "$",
      "+",
      "{",
      "}",
      "(",
      ")",
      "[",
      "]",
      "*",
      "&",
      "^",
      "<",
      ">",
      "@",
      "%"
    };
    foreach (string str in strArray)
    {
      if (text.Contains(str))
        return true;
    }
    return false;
  }

  internal System.Collections.Generic.Dictionary<object, object> GetElements(
    System.Collections.Generic.Dictionary<object, object> table)
  {
    System.Collections.Generic.Dictionary<object, object> elements = new System.Collections.Generic.Dictionary<object, object>();
    foreach (KeyValuePair<object, object> keyValuePair in table)
    {
      object key = keyValuePair.Key;
      object obj = keyValuePair.Value;
      System.Collections.Generic.Dictionary<object, object> dictionary1 = elements;
      if (key.ToString().Contains("."))
      {
        string[] strArray = key.ToString().Split('.');
        for (int index = 0; index < strArray.Length; ++index)
        {
          if (dictionary1.ContainsKey((object) strArray[index]))
          {
            this.GetElements((System.Collections.Generic.Dictionary<object, object>) dictionary1[(object) strArray[index]]);
            dictionary1 = (System.Collections.Generic.Dictionary<object, object>) dictionary1[(object) strArray[index]];
          }
          else if (index == strArray.Length - 1)
          {
            dictionary1.Add((object) strArray[index], obj);
          }
          else
          {
            System.Collections.Generic.Dictionary<object, object> dictionary2 = new System.Collections.Generic.Dictionary<object, object>();
            dictionary1.Add((object) strArray[index], (object) dictionary2);
            dictionary1 = (System.Collections.Generic.Dictionary<object, object>) dictionary1[(object) strArray[index]];
          }
        }
      }
      else
        dictionary1.Add(key, obj);
    }
    return elements;
  }

  private void fieldname(System.Collections.Generic.Dictionary<object, object> value, XmlTextWriter textWriter)
  {
    foreach (KeyValuePair<object, object> keyValuePair in value)
    {
      string str = keyValuePair.Key.ToString();
      bool flag = false;
      if (str.EndsWith(this.uniquekey))
      {
        str = str.Remove(str.Length - this.uniquekey.Length, this.uniquekey.Length);
        flag = true;
      }
      if (keyValuePair.Value is System.Collections.Generic.Dictionary<object, object>)
      {
        if (this.HasEscapeCharacter(str))
        {
          textWriter.WriteStartElement("field");
          if (!flag)
          {
            textWriter.WriteAttributeString("xfdf", "original", (string) null, str);
          }
          else
          {
            textWriter.WriteAttributeString("xmlns", "xfa", (string) null, "http://www.xfa.org/schema/xfa-data/1.0/");
            textWriter.WriteAttributeString("xfa", "contentType", (string) null, "text/html");
            textWriter.WriteAttributeString("xfdf", "original", (string) null, str);
          }
        }
        else if (str.Contains(" "))
        {
          textWriter.WriteStartElement(keyValuePair.Key.ToString().Replace(" ", ""));
          if (!flag)
          {
            textWriter.WriteAttributeString("xfdf", "original", (string) null, str);
          }
          else
          {
            textWriter.WriteAttributeString("xmlns", "xfa", (string) null, "http://www.xfa.org/schema/xfa-data/1.0/");
            textWriter.WriteAttributeString("xfa", "contentType", (string) null, "text/html");
            textWriter.WriteAttributeString("xfdf", "original", (string) null, str);
          }
        }
        else
        {
          textWriter.WriteStartElement(str, "");
          if (flag)
          {
            textWriter.WriteAttributeString("xmlns", "xfa", (string) null, "http://www.xfa.org/schema/xfa-data/1.0/");
            textWriter.WriteAttributeString("xfa", "contentType", (string) null, "text/html");
          }
        }
        this.fieldname((System.Collections.Generic.Dictionary<object, object>) keyValuePair.Value, textWriter);
        textWriter.WriteEndElement();
      }
      else
      {
        if (this.HasEscapeCharacter(str))
        {
          textWriter.WriteStartElement("field");
          if (!flag)
          {
            textWriter.WriteAttributeString("xfdf", "original", (string) null, str);
          }
          else
          {
            textWriter.WriteAttributeString("xmlns", "xfa", (string) null, "http://www.xfa.org/schema/xfa-data/1.0/");
            textWriter.WriteAttributeString("xfa", "contentType", (string) null, "text/html");
            textWriter.WriteAttributeString("xfdf", "original", (string) null, str);
          }
        }
        else if (str.Contains(" "))
        {
          textWriter.WriteStartElement(keyValuePair.Key.ToString().Replace(" ", ""));
          if (!flag)
          {
            textWriter.WriteAttributeString("xfdf", "original", (string) null, str);
          }
          else
          {
            textWriter.WriteAttributeString("xmlns", "xfa", (string) null, "http://www.xfa.org/schema/xfa-data/1.0/");
            textWriter.WriteAttributeString("xfa", "contentType", (string) null, "text/html");
            textWriter.WriteAttributeString("xfdf", "original", (string) null, str);
          }
        }
        else
        {
          textWriter.WriteStartElement(str, "");
          if (flag)
          {
            textWriter.WriteAttributeString("xmlns", "xfa", (string) null, "http://www.xfa.org/schema/xfa-data/1.0/");
            textWriter.WriteAttributeString("xfa", "contentType", (string) null, "text/html");
          }
        }
        textWriter.WriteString(keyValuePair.Value.ToString());
        textWriter.WriteEndElement();
      }
    }
  }

  internal void ExportDataJSON(Stream stream)
  {
    System.Collections.Generic.Dictionary<string, List<string>> dictionary = new System.Collections.Generic.Dictionary<string, List<string>>();
    for (int index = 0; index < this.Fields.Count; ++index)
    {
      PdfLoadedField field = (PdfLoadedField) this.Fields[index];
      if (field.Export)
      {
        field.ExportEmptyField = this.ExportEmptyFields;
        PdfName pdfName1 = PdfLoadedField.GetValue(field.Dictionary, field.CrossTable, "FT", true) as PdfName;
        if (pdfName1 != (PdfName) null)
        {
          switch (pdfName1.Value)
          {
            case "Tx":
              if (PdfLoadedField.GetValue(field.Dictionary, field.CrossTable, "V", true) is PdfString pdfString1)
              {
                dictionary.Add(field.Name, new List<string>()
                {
                  pdfString1.Value
                });
                continue;
              }
              if (this.ExportEmptyFields)
              {
                dictionary.Add(field.Name, new List<string>()
                {
                  ""
                });
                continue;
              }
              continue;
            case "Ch":
              IPdfPrimitive fieldPrimitive = PdfLoadedField.GetValue(field.Dictionary, field.CrossTable, "V", true);
              string str = (string) null;
              if (fieldPrimitive != null)
                str = this.GetExportValue(fieldPrimitive);
              if (field.GetType().Name == "PdfLoadedListBoxField")
              {
                if (fieldPrimitive == null && field.Dictionary.ContainsKey("I"))
                  str = (field as PdfLoadedListBoxField).SelectedValue[0];
                if (!string.IsNullOrEmpty(str))
                  dictionary.Add(field.Name, new List<string>()
                  {
                    str
                  });
                else if (this.ExportEmptyFields)
                  dictionary.Add(field.Name, new List<string>()
                  {
                    ""
                  });
              }
              else
              {
                if (fieldPrimitive == null && field.Dictionary.ContainsKey("I"))
                  str = (field as PdfLoadedComboBoxField).SelectedValue;
                if (!string.IsNullOrEmpty(str))
                  dictionary.Add(field.Name, new List<string>()
                  {
                    str
                  });
                else if (this.ExportEmptyFields)
                  dictionary.Add(field.Name, new List<string>()
                  {
                    ""
                  });
              }
              if (field is PdfLoadedListBoxField)
              {
                IEnumerator enumerator = (field as PdfLoadedListBoxField).Values.GetEnumerator();
                try
                {
                  while (enumerator.MoveNext())
                  {
                    PdfLoadedListItem current = (PdfLoadedListItem) enumerator.Current;
                    if (str != current.Value)
                      dictionary[field.Name].Add(current.Value);
                  }
                  continue;
                }
                finally
                {
                  if (enumerator is IDisposable disposable)
                    disposable.Dispose();
                }
              }
              else
                continue;
            case "Btn":
              IPdfPrimitive buttonFieldPrimitive = PdfLoadedField.GetValue(field.Dictionary, field.CrossTable, "V", true);
              PdfLoadedRadioButtonListField radioButtonListField = (PdfLoadedRadioButtonListField) null;
              if (buttonFieldPrimitive != null)
              {
                string exportValue = field.GetExportValue(field, buttonFieldPrimitive);
                if (field is PdfLoadedRadioButtonListField)
                  radioButtonListField = field as PdfLoadedRadioButtonListField;
                if (radioButtonListField != null && radioButtonListField.SelectedIndex == -1)
                {
                  dictionary.Add(field.Name, new List<string>()
                  {
                    "Off"
                  });
                  continue;
                }
                if (field.Dictionary != null && field.Dictionary.ContainsKey("Opt"))
                {
                  PdfArray pdfArray = PdfCrossTable.Dereference(field.Dictionary["Opt"]) as PdfArray;
                  int result = 0;
                  try
                  {
                    int.TryParse(exportValue, out result);
                    if (pdfArray != null)
                    {
                      if (PdfCrossTable.Dereference(pdfArray[radioButtonListField.SelectedIndex]) is PdfString pdfString2)
                        exportValue = pdfString2.Value;
                    }
                  }
                  catch
                  {
                  }
                  if (!string.IsNullOrEmpty(exportValue))
                  {
                    dictionary.Add(field.Name, new List<string>()
                    {
                      exportValue
                    });
                    continue;
                  }
                  continue;
                }
                if (!string.IsNullOrEmpty(exportValue))
                {
                  dictionary.Add(field.Name, new List<string>()
                  {
                    exportValue
                  });
                  continue;
                }
                if (field is PdfLoadedCheckBoxField)
                {
                  if (this.ExportEmptyFields)
                  {
                    dictionary.Add(field.Name, new List<string>()
                    {
                      ""
                    });
                    continue;
                  }
                  dictionary.Add(field.Name, new List<string>()
                  {
                    "Off"
                  });
                  continue;
                }
                continue;
              }
              if (field is PdfLoadedRadioButtonListField)
              {
                dictionary.Add(field.Name, new List<string>()
                {
                  field.GetAppearanceStateValue(field)
                });
                continue;
              }
              PdfDictionary widgetAnnotation = field.GetWidgetAnnotation(field.Dictionary, field.CrossTable);
              if (widgetAnnotation != null)
              {
                PdfName pdfName2 = widgetAnnotation["AS"] as PdfName;
                if (pdfName2 != (PdfName) null)
                {
                  dictionary.Add(field.Name, new List<string>()
                  {
                    pdfName2.Value
                  });
                  continue;
                }
                if (this.ExportEmptyFields)
                {
                  dictionary.Add(field.Name, new List<string>()
                  {
                    ""
                  });
                  continue;
                }
                continue;
              }
              continue;
            default:
              continue;
          }
        }
      }
    }
    byte[] bytes1 = Encoding.GetEncoding("UTF-8").GetBytes("{");
    stream.Write(bytes1, 0, bytes1.Length);
    int num = 0;
    foreach (KeyValuePair<string, List<string>> keyValuePair in dictionary)
    {
      string s = $"\"{XmlConvert.EncodeName(Convert.ToString(keyValuePair.Key))}\":\"{XmlConvert.EncodeName(string.Join(",", keyValuePair.Value.ToArray()))}\"";
      if (num > 0)
        s = "," + s;
      byte[] bytes2 = Encoding.GetEncoding("UTF-8").GetBytes(s);
      stream.Write(bytes2, 0, bytes2.Length);
      ++num;
    }
    byte[] bytes3 = Encoding.GetEncoding("UTF-8").GetBytes("}");
    stream.Write(bytes3, 0, bytes3.Length);
  }

  private string GetExportValue(IPdfPrimitive fieldPrimitive)
  {
    string exportValue = (string) null;
    if ((object) (fieldPrimitive as PdfName) != null)
    {
      exportValue = (fieldPrimitive as PdfName).Value;
    }
    else
    {
      switch (fieldPrimitive)
      {
        case PdfString _:
          exportValue = (fieldPrimitive as PdfString).Value;
          break;
        case PdfArray _:
          IEnumerator enumerator = (fieldPrimitive as PdfArray).GetEnumerator();
          try
          {
            while (enumerator.MoveNext())
            {
              IPdfPrimitive current = (IPdfPrimitive) enumerator.Current;
              if ((object) (current as PdfName) != null)
                return (current as PdfName).Value;
              if (current is PdfString)
                return (current as PdfString).Value;
            }
            break;
          }
          finally
          {
            if (enumerator is IDisposable disposable)
              disposable.Dispose();
          }
      }
    }
    return exportValue;
  }

  internal void OnValidate(string nodeName)
  {
    if (nodeName.StartsWith("XML"))
      throw new Exception("Element type names may not start with XML");
    if (nodeName.StartsWith("_"))
      throw new Exception("Element type names must start with a letter or underscore");
    if (!char.IsLetter(nodeName[0]) && !char.IsNumber(nodeName[0]))
      throw new Exception("Element type names must start with a letter or underscore");
  }

  internal override void Dictionary_BeginSave(object sender, SavePdfPrimitiveEventArgs ars)
  {
    this.FlttenFormFields();
    if (this.m_fields.Count == 0 && !(ars.Writer.Document as PdfLoadedDocument).m_isXfaDocument)
    {
      int index = this.CrossTable.PdfObjects.IndexOf((IPdfPrimitive) this.Dictionary);
      this.Dictionary.Clear();
      (sender as PdfDictionary).Remove("AcroForm");
      if (index != -1)
        this.CrossTable.PdfObjects.Remove(index);
    }
    else if (this.SetAppearanceDictionary && (this.Dictionary.ContainsKey("NeedAppearances") || base.SignatureFlags == SignatureFlags.None && this.NeedAppearances))
      this.Dictionary.SetBoolean("NeedAppearances", this.NeedAppearances);
    bool flag = this.EnableXfaFormFill;
    if (!flag && ars.Writer.Document is PdfLoadedDocument document)
      flag = document.m_isXfaDocument;
    if (flag)
      return;
    this.Dictionary.Remove("XFA");
  }

  private void FlttenFormFields()
  {
    int index1 = 0;
    if (base.SignatureFlags != SignatureFlags.None)
    {
      if (!this.IsDefaultAppearance)
        this.NeedAppearances = false;
      if (this.Dictionary.ContainsKey("NeedAppearances"))
        this.Dictionary.SetBoolean("NeedAppearances", this.NeedAppearances);
    }
    for (; index1 < this.Fields.Count; ++index1)
    {
      if (this.Fields[index1] is PdfLoadedField field1 && field1.DisableAutoFormat && field1.Dictionary.ContainsKey("AA"))
      {
        field1.Dictionary.Remove("AA");
        field1.BeginSave();
      }
      if (field1 != null && field1.Dictionary != null)
      {
        int num = 0;
        PdfDictionary dictionary = field1.Dictionary;
        bool flag = false;
        if (!dictionary.ContainsKey("AP") && this.IsDefaultAppearance && !this.NeedAppearances && !field1.Changed)
          flag = true;
        if (dictionary.ContainsKey("F") && PdfCrossTable.Dereference(dictionary["F"]) is PdfNumber pdfNumber)
          num = pdfNumber.IntValue;
        PdfArray pdfArray = (PdfArray) null;
        if (field1.Dictionary.ContainsKey("Kids"))
          pdfArray = PdfCrossTable.Dereference(field1.Dictionary["Kids"]) as PdfArray;
        if (field1.Flatten && num != 6)
        {
          if (field1.Page != null || pdfArray != null)
            field1.Draw();
          this.Fields.Remove((PdfField) field1);
          int index2 = this.CrossTable.PdfObjects.IndexOf((IPdfPrimitive) field1.Dictionary);
          if (index2 != -1)
            this.CrossTable.PdfObjects.Remove(index2);
          --index1;
        }
        else if (field1.Changed || flag)
          field1.BeginSave();
      }
      else
      {
        PdfField field2 = this.Fields[index1];
        if (field2.Flatten)
        {
          this.Fields.Remove(field2);
          field2.Draw();
          --index1;
        }
        else
          field2.Save();
      }
    }
  }

  internal override void Clear()
  {
    if (this.m_fields != null)
      this.m_fields.Clear();
    if (this.m_pageMap != null)
      this.m_pageMap.Clear();
    if (this.m_terminalFields != null)
      this.m_terminalFields.Clear();
    this.Dictionary.Clear();
  }

  internal void RemoveFromDictionaries(PdfField field)
  {
    if (this.m_fields != null && this.m_fields.Count > 0)
    {
      PdfName key1 = new PdfName("Fields");
      PdfArray primitive = this.m_crossTable.GetObject(this.Dictionary[key1]) as PdfArray;
      PdfReferenceHolder element = new PdfReferenceHolder((IPdfPrimitive) field.Dictionary);
      primitive.Remove((IPdfPrimitive) element);
      primitive.MarkChanged();
      if (!this.IsFormContainsKids || !field.Dictionary.Items.ContainsKey(new PdfName("Parent")))
      {
        for (int index1 = 0; index1 < primitive.Count; ++index1)
        {
          PdfDictionary pdfDictionary = PdfCrossTable.Dereference(this.CrossTable.GetObject(primitive[index1])) as PdfDictionary;
          PdfName key2 = new PdfName("Kids");
          if (pdfDictionary != null && pdfDictionary.ContainsKey(key2))
          {
            PdfArray pdfArray = this.CrossTable.GetObject(pdfDictionary[key2]) as PdfArray;
            pdfArray.Remove((IPdfPrimitive) element);
            for (int index2 = 0; index2 < pdfArray.Count; ++index2)
            {
              if (pdfArray.Elements[index2] is PdfNull)
              {
                pdfArray.RemoveAt(index2);
                index2 = -1;
              }
            }
          }
        }
      }
      else if (field.Dictionary.Items.ContainsKey(new PdfName("Parent")))
      {
        PdfArray pdfArray = ((field.Dictionary["Parent"] as PdfReferenceHolder).Object as PdfDictionary).Items[new PdfName("Kids")] as PdfArray;
        for (int index = 0; index < pdfArray.Count; ++index)
        {
          if ((pdfArray[index] as PdfReferenceHolder).Equals((object) element))
            pdfArray.Remove((IPdfPrimitive) element);
        }
      }
      this.Dictionary.SetProperty(key1, (IPdfPrimitive) primitive);
    }
    if (!(field is PdfLoadedField))
      return;
    this.DeleteFromPages(field);
    this.DeleteAnnottation(field);
  }

  internal new void DeleteFromPages(PdfField field)
  {
    PdfDictionary dictionary = field.Dictionary;
    PdfName key1 = new PdfName("Kids");
    PdfName key2 = new PdfName("Annots");
    PdfName key3 = new PdfName("P");
    if (dictionary.ContainsKey(key1))
    {
      PdfArray pdfArray1 = this.CrossTable.GetObject(dictionary[key1]) as PdfArray;
      int index1 = 0;
      for (int count = pdfArray1.Count; index1 < count; ++index1)
      {
        PdfReferenceHolder pdfReferenceHolder = pdfArray1[index1] as PdfReferenceHolder;
        PdfDictionary pdfDictionary1 = this.CrossTable.GetObject((IPdfPrimitive) pdfReferenceHolder) as PdfDictionary;
        PdfReference pointer = (PdfReference) null;
        if (pdfDictionary1.ContainsKey(key3) && !(pdfDictionary1["P"] is PdfNull))
          pointer = this.CrossTable.GetReference(pdfDictionary1[key3]);
        else if (dictionary.ContainsKey(key3) && !(dictionary["P"] is PdfNull))
          pointer = this.CrossTable.GetReference(dictionary[key3]);
        else if (field.Page != null)
          pointer = this.CrossTable.GetReference((IPdfPrimitive) field.Page.Dictionary);
        if (this.CrossTable.GetObject((IPdfPrimitive) pointer) is PdfDictionary pdfDictionary2 && pdfDictionary2.ContainsKey(key2))
        {
          PdfArray primitive = this.CrossTable.GetObject(pdfDictionary2[key2]) as PdfArray;
          primitive.Remove((IPdfPrimitive) pdfReferenceHolder);
          primitive.MarkChanged();
          pdfDictionary2.SetProperty(key2, (IPdfPrimitive) primitive);
          int index2 = this.m_crossTable.PdfObjects.IndexOf(this.m_crossTable.GetObject((IPdfPrimitive) pdfDictionary1));
          if (index2 != -1)
            this.m_crossTable.PdfObjects.Remove(index2);
        }
        else if (field is PdfLoadedField && pdfReferenceHolder != (PdfReferenceHolder) null)
        {
          PdfLoadedField pdfLoadedField = field as PdfLoadedField;
          pdfLoadedField.m_requiredReference = pdfReferenceHolder;
          if (field.Page != null && field.Page.Dictionary.ContainsKey(key2))
          {
            PdfArray pdfArray2 = this.CrossTable.GetObject(field.Page.Dictionary[key2]) as PdfArray;
            pdfArray2.Remove((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary1));
            pdfArray2.MarkChanged();
          }
          if (this.CrossTable.PdfObjects.Contains((IPdfPrimitive) pdfDictionary1))
            this.CrossTable.PdfObjects.Remove(this.CrossTable.PdfObjects.IndexOf((IPdfPrimitive) pdfDictionary1));
          pdfLoadedField.m_requiredReference = (PdfReferenceHolder) null;
        }
      }
    }
    else
    {
      PdfReference pointer = (PdfReference) null;
      if (dictionary.ContainsKey(key3) && !(dictionary["P"] is PdfNull))
        pointer = this.CrossTable.GetReference(dictionary[key3]);
      else if (field.Page != null)
        pointer = this.CrossTable.GetReference((IPdfPrimitive) field.Page.Dictionary);
      if (this.CrossTable.GetObject((IPdfPrimitive) pointer) is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey(key2))
      {
        PdfArray primitive = this.CrossTable.GetObject(pdfDictionary[key2]) as PdfArray;
        primitive.Remove((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) dictionary));
        primitive.MarkChanged();
        pdfDictionary.SetProperty(key2, (IPdfPrimitive) primitive);
      }
      else if (field.Page != null)
      {
        PdfPageBase page = field.Page;
        if (page.Dictionary.ContainsKey(key2))
        {
          PdfArray pdfArray = this.CrossTable.GetObject(page.Dictionary[key2]) as PdfArray;
          pdfArray.Remove((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) dictionary));
          pdfArray.MarkChanged();
        }
      }
    }
    if (field.Page == null || !(field.Page is PdfLoadedPage page1) || page1.Document == null)
      return;
    PdfCatalog catalog = page1.Document.Catalog;
    if (catalog == null || !catalog.ContainsKey("Perms") || !(PdfCrossTable.Dereference(catalog["Perms"]) is PdfDictionary pdfDictionary3) || !pdfDictionary3.ContainsKey("DocMDP"))
      return;
    PdfReferenceHolder pdfReferenceHolder1 = pdfDictionary3["DocMDP"] as PdfReferenceHolder;
    if (!dictionary.ContainsKey("V"))
      return;
    PdfReferenceHolder pdfReferenceHolder2 = dictionary["V"] as PdfReferenceHolder;
    if (!(pdfReferenceHolder2 != (PdfReferenceHolder) null) || !(pdfReferenceHolder1 != (PdfReferenceHolder) null) || !(pdfReferenceHolder2.Reference != (PdfReference) null) || !(pdfReferenceHolder1.Reference != (PdfReference) null) || !(pdfReferenceHolder2.Reference == pdfReferenceHolder1.Reference))
      return;
    pdfDictionary3.Remove("DocMDP");
  }

  internal void DeleteAnnottation(PdfField field)
  {
    PdfDictionary dictionary = field.Dictionary;
    PdfName key = new PdfName("Kids");
    if (!dictionary.ContainsKey(key))
      return;
    PdfArray primitive = this.m_crossTable.GetObject(dictionary[key]) as PdfArray;
    primitive.Clear();
    dictionary.SetProperty(key, (IPdfPrimitive) primitive);
  }

  internal override string GetCorrectName(string name)
  {
    List<string> stringList = new List<string>();
    for (int index = 0; index < this.Fields.Count; ++index)
      stringList.Add(this.Fields[index].Name);
    string correctName = name;
    int num = 0;
    while (stringList.IndexOf(correctName) != -1)
    {
      correctName = name + (object) num;
      ++num;
    }
    return correctName;
  }

  public void ImportData(string fileName, DataFormat dataFormat)
  {
    if (this.m_settings.IgnoreErrors)
      this.ImportDataField(fileName, dataFormat, true);
    else
      this.ImportDataField(fileName, dataFormat, false);
  }

  public void ImportData(string fileName, ImportFormSettings settings)
  {
    this.m_settings = settings;
    this.ImportData(fileName, this.m_settings.DataFormat);
  }

  public void ImportData(byte[] array, DataFormat dataFormat)
  {
    MemoryStream fileName = array != null ? new MemoryStream(array) : throw new ArgumentNullException(nameof (array));
    try
    {
      this.ImportData((Stream) fileName, dataFormat, false);
    }
    catch
    {
      throw;
    }
    finally
    {
      fileName?.Close();
    }
  }

  public PdfLoadedFieldImportError[] ImportData(
    string fileName,
    DataFormat dataFormat,
    bool errorFlag)
  {
    return this.ImportDataField(fileName, dataFormat, errorFlag);
  }

  public PdfLoadedFieldImportError[] ImportData(
    byte[] array,
    DataFormat dataFormat,
    bool errorFlag)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    return this.ImportData((Stream) new MemoryStream(array), dataFormat, errorFlag);
  }

  private PdfLoadedFieldImportError[] ImportDataField(
    string fileName,
    DataFormat dataFormat,
    bool continueImportOnError)
  {
    FileStream fileName1 = (FileStream) null;
    try
    {
      fileName1 = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
      return this.ImportData((Stream) fileName1, dataFormat, continueImportOnError);
    }
    catch
    {
      throw;
    }
    finally
    {
      fileName1?.Close();
    }
  }

  private PdfLoadedFieldImportError[] ImportData(
    Stream fileName,
    DataFormat dataFormat,
    bool continueImportOnError)
  {
    switch (dataFormat)
    {
      case DataFormat.Xml:
        return this.ImportDataXML(fileName, continueImportOnError);
      case DataFormat.Fdf:
        return this.ImportDataFDF(fileName, continueImportOnError);
      case DataFormat.XFdf:
        return this.ImportDataXFDF(fileName, continueImportOnError);
      default:
        return (PdfLoadedFieldImportError[]) null;
    }
  }

  public void ImportDataJson(string fileName)
  {
    try
    {
      this.ImportDataJson((Stream) new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read));
    }
    catch
    {
      throw;
    }
  }

  public void ImportDataJson(byte[] array)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    this.ImportDataJson((Stream) new MemoryStream(array));
  }

  public void ImportDataJson(Stream stream)
  {
    if (stream == null)
      return;
    string name1 = (string) null;
    string name2 = (string) null;
    System.Collections.Generic.Dictionary<string, string> dictionary = new System.Collections.Generic.Dictionary<string, string>();
    StreamReader streamReader = new StreamReader(stream);
    PdfReader pdfReader = new PdfReader(stream);
    string nextJsonToken1 = pdfReader.GetNextJsonToken();
    pdfReader.Position = 0L;
    for (; nextJsonToken1 != null && nextJsonToken1 != string.Empty; nextJsonToken1 = pdfReader.GetNextJsonToken())
    {
      if (nextJsonToken1 != "{" && nextJsonToken1 != "}" && nextJsonToken1 != "\"" && nextJsonToken1 != ",")
      {
        name1 = nextJsonToken1;
        do
          ;
        while (pdfReader.GetNextJsonToken() != ":");
        do
          ;
        while (pdfReader.GetNextJsonToken() != "\"");
        string nextJsonToken2 = pdfReader.GetNextJsonToken();
        if (nextJsonToken2 != "\"")
          name2 = nextJsonToken2;
      }
      if (name1 != null && name2 != null)
      {
        dictionary.Add(XmlConvert.DecodeName(name1), XmlConvert.DecodeName(name2));
        name1 = (string) null;
        name2 = (string) null;
      }
    }
    PdfLoadedField field = (PdfLoadedField) null;
    foreach (KeyValuePair<string, string> keyValuePair in dictionary)
    {
      this.Fields.TryGetField(keyValuePair.Key, out field);
      field?.ImportFieldValue((object) keyValuePair.Value);
    }
    stream.Dispose();
  }

  public void ImportDataFDF(Stream stream)
  {
    if (this.m_settings.IgnoreErrors)
      this.ImportDataFDF(stream, true);
    else
      this.ImportDataFDF(stream, false);
  }

  public void ImportDataFDF(byte[] array)
  {
    MemoryStream memoryStream = array != null ? new MemoryStream(array) : throw new ArgumentNullException(nameof (array));
    if (this.m_settings.IgnoreErrors)
      this.ImportDataFDF((Stream) memoryStream, true);
    else
      this.ImportDataFDF((Stream) memoryStream, false);
  }

  public PdfLoadedFieldImportError[] ImportDataFDF(Stream stream, bool continueImportOnError)
  {
    PdfReader reader = new PdfReader(stream);
    reader.Position = 0L;
    if (reader.GetNextToken().StartsWith("%") && !reader.GetNextToken().StartsWith("FDF-"))
      throw new Exception("The source is not a valid FDF file because it does not start with\"%FDF-\"");
    reader.GetNextToken();
    string nextToken = reader.GetNextToken();
    if (nextToken != null && nextToken != "âãÏÓ" && nextToken != "Ã¢Ã£Ã\u008FÃ\u0093")
      this.m_settings.AsPerSpecification = false;
    if (!this.m_settings.AsPerSpecification)
    {
      Hashtable table = new Hashtable();
      ArrayList arrayList = new ArrayList();
      string fieldName = "";
      for (string token = reader.GetNextToken(); token != null && token != string.Empty; token = reader.GetNextToken())
      {
        if (token.ToUpper() == "T")
          fieldName = this.GetFieldName(reader, out token);
        if (token.ToUpper() == "V")
          this.GetFieldValue(reader, token, fieldName, table);
      }
      PdfLoadedField field = (PdfLoadedField) null;
      foreach (DictionaryEntry dictionaryEntry in table)
      {
        try
        {
          field = (PdfLoadedField) this.Fields[dictionaryEntry.Key.ToString()];
          field?.ImportFieldValue(dictionaryEntry.Value);
        }
        catch (Exception ex)
        {
          if (!continueImportOnError)
            throw;
          PdfLoadedFieldImportError fieldImportError = new PdfLoadedFieldImportError(field, ex);
          arrayList.Add((object) fieldImportError);
        }
      }
      return arrayList.Count == 0 ? (PdfLoadedFieldImportError[]) null : (PdfLoadedFieldImportError[]) arrayList.ToArray(typeof (PdfLoadedFieldImportError));
    }
    PdfLoadedField field1 = (PdfLoadedField) null;
    ArrayList arrayList1 = new ArrayList();
    FdfParser fdfParser = new FdfParser(stream);
    fdfParser.ParseObjectData();
    this.m_fdfFields = new System.Collections.Generic.Dictionary<string, PdfDictionary>();
    System.Collections.Generic.Dictionary<string, IPdfPrimitive> objects = fdfParser.FdfObjects.Objects;
    if (objects != null && objects.Count > 0 && objects.ContainsKey("trailer"))
    {
      if (objects["trailer"] is PdfDictionary pdfDictionary2 && pdfDictionary2.ContainsKey("Root"))
      {
        PdfReferenceHolder pdfReferenceHolder = pdfDictionary2["Root"] as PdfReferenceHolder;
        if (pdfReferenceHolder != (PdfReferenceHolder) null)
        {
          PdfReference reference = pdfReferenceHolder.Reference;
          if (reference != (PdfReference) null)
          {
            string key = $"{(object) reference.ObjNum} {(object) reference.GenNum}";
            if (objects.ContainsKey(key) && objects[key] is PdfDictionary pdfDictionary1 && pdfDictionary1.ContainsKey("FDF"))
            {
              if (pdfDictionary1["FDF"] is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey("Fields"))
              {
                PdfArray primitive = pdfDictionary["Fields"] as PdfArray;
                PdfDictionary kidNodes = new PdfDictionary();
                kidNodes.SetProperty("Kids", (IPdfPrimitive) primitive);
                this.ReadFDFFields(kidNodes, "");
              }
              objects.Remove(key);
            }
          }
        }
      }
      objects.Remove("trailer");
    }
    foreach (KeyValuePair<string, PdfDictionary> fdfField in this.m_fdfFields)
    {
      try
      {
        this.Fields.TryGetField(fdfField.Key.ToString(), out field1);
        if (field1 != null)
        {
          field1.isAcrobat = true;
          if (fdfField.Value.ContainsKey("RV"))
            field1.Dictionary.SetProperty("RV", fdfField.Value["RV"]);
          if (fdfField.Value["V"] is PdfString pdfString1)
            field1.ImportFieldValue((object) pdfString1.Value);
          else if (fdfField.Value["V"] is PdfArray pdfArray)
          {
            string[] FieldValue = new string[pdfArray.Count];
            for (int index = 0; index < pdfArray.Count; ++index)
            {
              if (pdfArray[index] is PdfString pdfString)
                FieldValue[index] = pdfString.Value;
            }
            field1.ImportFieldValue((object) FieldValue);
          }
          else
          {
            PdfName pdfName = fdfField.Value["V"] as PdfName;
            if (pdfName != (PdfName) null)
              field1.ImportFieldValue((object) pdfName.Value);
          }
        }
      }
      catch (Exception ex)
      {
        if (!continueImportOnError)
          throw;
        PdfLoadedFieldImportError fieldImportError = new PdfLoadedFieldImportError(field1, ex);
        arrayList1.Add((object) fieldImportError);
      }
    }
    fdfParser.Dispose();
    this.m_fdfFields.Clear();
    return arrayList1.Count == 0 ? (PdfLoadedFieldImportError[]) null : (PdfLoadedFieldImportError[]) arrayList1.ToArray(typeof (PdfLoadedFieldImportError));
  }

  private void ReadFDFFields(PdfDictionary kidNodes, string name)
  {
    if (!(kidNodes["Kids"] is PdfArray kidNode) || kidNode.Count == 0)
    {
      if (name.Length > 0)
        name = name.Substring(1);
      this.m_fdfFields[name] = kidNodes;
    }
    else
    {
      kidNodes.Remove("Kids");
      for (int index = 0; index < kidNode.Count; ++index)
      {
        PdfDictionary pdfDictionary1 = new PdfDictionary();
        pdfDictionary1 = kidNodes;
        PdfDictionary pdfDictionary2 = kidNode[index] as PdfDictionary;
        PdfString pdfString = pdfDictionary2["T"] as PdfString;
        string name1 = name;
        if (pdfString != null)
          name1 = $"{name1}.{pdfString.Value}";
        PdfDictionary kidNodes1 = pdfDictionary2;
        kidNodes1.Remove("T");
        this.ReadFDFFields(kidNodes1, name1);
      }
    }
  }

  private string GetFieldName(PdfReader reader, out string token)
  {
    string fieldName = "";
    token = reader.GetNextToken();
    if (!string.IsNullOrEmpty(token))
    {
      if (token == "<")
      {
        token = reader.GetNextToken();
        if (!string.IsNullOrEmpty(token) && token != ">")
        {
          byte[] bytes = new PdfString().HexToBytes(token);
          token = PdfString.ByteToString(bytes);
          fieldName = token;
        }
      }
      else
      {
        token = reader.GetNextToken();
        if (!string.IsNullOrEmpty(token))
        {
          string str = " ";
          while (str != ")")
          {
            str = reader.GetNextToken();
            if (!string.IsNullOrEmpty(str) && str != ")")
              token = $"{token} {str}";
          }
          fieldName = token;
          token = str;
        }
      }
    }
    return fieldName;
  }

  private void GetFieldValue(PdfReader reader, string token, string fieldName, Hashtable table)
  {
    token = reader.GetNextToken();
    if (string.IsNullOrEmpty(token))
      return;
    if (token == "[")
    {
      token = reader.GetNextToken();
      if (string.IsNullOrEmpty(token))
        return;
      List<string> fieldValues = new List<string>();
      while (token != "]")
        token = this.GetFieldValue(reader, token, true, table, fieldName, fieldValues);
      if (table.ContainsKey((object) fieldName) || fieldValues.Count <= 0)
        return;
      table.Add((object) fieldName, (object) fieldValues.ToArray());
    }
    else
      this.GetFieldValue(reader, token, false, table, fieldName, (List<string>) null);
  }

  private string GetFieldValue(
    PdfReader reader,
    string token,
    bool isMultiSelect,
    Hashtable table,
    string fieldName,
    List<string> fieldValues)
  {
    if (token == "<")
    {
      token = reader.GetNextToken();
      if (!string.IsNullOrEmpty(token) && token != ">")
      {
        token = PdfString.ByteToString(new PdfString().HexToBytes(token));
        if (isMultiSelect)
          fieldValues.Add(token);
        else if (!table.ContainsKey((object) fieldName))
          table.Add((object) fieldName, (object) token);
      }
    }
    else if (isMultiSelect)
    {
      for (; token != ">" && token != ")" && token != "]"; token = reader.GetNextToken())
      {
        if (!string.IsNullOrEmpty(token) && (token == "/" || token != ")"))
        {
          token = reader.GetNextToken();
          if (!string.IsNullOrEmpty(token) && token != ">" && token != ")")
          {
            string str = " ";
            while (str != ")" && str != ">")
            {
              str = reader.GetNextToken();
              if (!string.IsNullOrEmpty(str) && str != ">" && str != ")" && str != "/")
                token = $"{token} {str}";
              fieldValues.Add(token);
            }
          }
        }
      }
    }
    else
    {
      for (; token != ">" && token != ")"; token = reader.GetNextToken())
      {
        if (!string.IsNullOrEmpty(token) && (token == "/" || token != ")"))
        {
          token = reader.GetNextToken();
          if (!string.IsNullOrEmpty(token) && token != ">" && token != ")")
          {
            string str = " ";
            while (str != ")" && str != ">")
            {
              str = reader.GetNextToken();
              if (!string.IsNullOrEmpty(str) && str != ">" && str != ")" && str != "/")
                token = $"{token} {str}";
            }
            if (!table.ContainsKey((object) fieldName))
              table.Add((object) fieldName, (object) token);
          }
        }
      }
    }
    return token;
  }

  public void HighlightFields(bool highlight)
  {
    this.CrossTable.Document.Catalog["OpenAction"] = (IPdfPrimitive) new PdfDictionary()
    {
      ["Type"] = (IPdfPrimitive) new PdfName("Action"),
      ["S"] = (IPdfPrimitive) new PdfName("JavaScript"),
      ["JS"] = (!highlight ? (IPdfPrimitive) new PdfString("app.runtimeHighlight = false;") : (IPdfPrimitive) new PdfString("app.runtimeHighlight = true;"))
    };
    this.CrossTable.Document.Catalog.Modify();
  }

  public bool OnlyHexInString(string test) => Regex.IsMatch(test, "\\A\\b[0-9a-fA-F]+\\b\\Z");

  private PdfLoadedFieldImportError[] ImportDataXML(Stream stream, bool continueImportOnError)
  {
    XmlDocument xmlDocument = new XmlDocument();
    xmlDocument.Load(stream);
    if (xmlDocument.DocumentElement.LocalName.ToUpper() != "fields".ToUpper())
      throw new ArgumentException("The XML form data stream is not valid");
    if (xmlDocument.DocumentElement.LocalName != "fields")
      this.m_settings.AsPerSpecification = false;
    this.m_xmlFields = new System.Collections.Generic.Dictionary<string, List<string>>();
    ArrayList list = new ArrayList();
    this.ImportXMLData(xmlDocument.DocumentElement.ChildNodes, continueImportOnError, list);
    if (this.m_settings.AsPerSpecification)
    {
      System.Collections.Generic.Dictionary<string, PdfLoadedField> dictionary = new System.Collections.Generic.Dictionary<string, PdfLoadedField>();
      for (int index = 0; index < this.Fields.Count; ++index)
      {
        PdfLoadedField field = this.Fields[index] as PdfLoadedField;
        string name = field.Name;
        string key = field.UpdateEncodedValue(name, (PdfDictionary) null);
        dictionary[key] = field;
      }
      foreach (KeyValuePair<string, List<string>> xmlField in this.m_xmlFields)
      {
        string str = xmlField.Key;
        object FieldValue = (object) xmlField.Value[0];
        if (xmlField.Value.Count > 1)
        {
          str = str.Remove(str.Length - 6, 6);
          string[] strArray = new string[xmlField.Value.Count];
          for (int index = 0; index < xmlField.Value.Count; ++index)
            strArray[index] = xmlField.Value[index];
          FieldValue = (object) strArray;
        }
        PdfLoadedField field = (PdfLoadedField) null;
        try
        {
          if (!this.Fields.TryGetField(str, out field) && dictionary.ContainsKey(str))
            field = dictionary[str];
          if (field != null)
          {
            field.isAcrobat = true;
            field.ImportFieldValue(FieldValue);
          }
        }
        catch (Exception ex)
        {
          if (str.Contains(".value"))
          {
            string name = str.Substring(0, str.Length - 6);
            try
            {
              field = (PdfLoadedField) this.Fields[name];
              if (field != null)
              {
                field.isAcrobat = true;
                field.ImportFieldValue(FieldValue);
              }
            }
            catch
            {
              if (!continueImportOnError)
                throw;
              PdfLoadedFieldImportError fieldImportError = new PdfLoadedFieldImportError(field, ex);
              list.Add((object) fieldImportError);
            }
          }
          else
          {
            if (!continueImportOnError)
              throw;
            PdfLoadedFieldImportError fieldImportError = new PdfLoadedFieldImportError(field, ex);
            list.Add((object) fieldImportError);
          }
        }
      }
      this.m_xmlFields.Clear();
    }
    return list.Count == 0 ? (PdfLoadedFieldImportError[]) null : (PdfLoadedFieldImportError[]) list.ToArray(typeof (PdfLoadedFieldImportError));
  }

  private void ImportXMLData(XmlNodeList xmlnode, bool continueImportOnError, ArrayList list)
  {
    for (int i = 0; i < xmlnode.Count; ++i)
    {
      if (xmlnode[i] is XmlText xmlText)
      {
        string data = xmlText.Data;
        XmlNode parentNode = xmlText.ParentNode;
        string str = "";
        for (; parentNode.LocalName.ToUpper() != "fields".ToUpper(); parentNode = parentNode.ParentNode)
        {
          if (str.Length > 0)
            str = "." + str;
          str = !this.m_settings.AsPerSpecification ? parentNode.LocalName + str : (parentNode.Attributes["xfdf:original"] == null || !(parentNode.Attributes["xfdf:original"].Value != "") ? parentNode.LocalName + str : parentNode.Attributes["xfdf:original"].Value + str);
        }
        PdfLoadedField field = (PdfLoadedField) null;
        try
        {
          if (this.m_settings.AsPerSpecification)
          {
            if (this.m_xmlFields.ContainsKey(str))
              this.m_xmlFields[str].Add(data);
            else
              this.m_xmlFields[str] = new List<string>()
              {
                data
              };
          }
          else
          {
            field = (PdfLoadedField) this.Fields[XmlConvert.DecodeName(str)];
            field?.ImportFieldValue((object) data);
          }
        }
        catch (Exception ex)
        {
          if (!continueImportOnError)
            throw;
          PdfLoadedFieldImportError fieldImportError = new PdfLoadedFieldImportError(field, ex);
          list.Add((object) fieldImportError);
        }
      }
      if (xmlnode[i].ChildNodes != null)
        this.ImportXMLData(xmlnode[i].ChildNodes, continueImportOnError, list);
    }
  }

  public void ImportDataXFDF(string fileName)
  {
    this.ImportDataXFDF((Stream) new FileStream(fileName, FileMode.Open), false);
  }

  public void ImportDataXFDF(byte[] array)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    this.ImportDataXFDF((Stream) new MemoryStream(array));
  }

  public void ImportDataXFDF(Stream stream)
  {
    if (stream == null)
      return;
    if (this.m_settings.IgnoreErrors)
      this.ImportDataXFDF(stream, true);
    else
      this.ImportDataXFDF(stream, false);
  }

  private PdfLoadedFieldImportError[] ImportDataXFDF(Stream stream, bool continueImportOnError)
  {
    stream.Position = 0L;
    ArrayList arrayList = new ArrayList();
    XmlDocument xmlDocument = new XmlDocument();
    xmlDocument.Load(stream);
    this.m_xdfdFields = new System.Collections.Generic.Dictionary<string, List<string>>();
    XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
    namespaceManager.AddNamespace("xfdf", "http://ns.adobe.com/xfdf/");
    XmlElement documentElement = xmlDocument.DocumentElement;
    XmlNode xmlNode1 = documentElement.SelectSingleNode("xfdf:f", namespaceManager);
    if (xmlNode1 != null && xmlNode1.Attributes["href"] != null)
      xmlNode1.Attributes["href"].Value.Trim();
    if (documentElement.SelectSingleNode("xfdf:ids", namespaceManager) == null)
      this.m_settings.AsPerSpecification = false;
    XmlNode xmlNode2 = documentElement.SelectSingleNode("xfdf:fields", namespaceManager);
    if (xmlNode2 != null)
      this.ImportXFDFNodes(xmlNode2.ChildNodes, namespaceManager);
    System.Collections.Generic.Dictionary<string, PdfLoadedField> dictionary = new System.Collections.Generic.Dictionary<string, PdfLoadedField>();
    for (int index = 0; index < this.Fields.Count; ++index)
    {
      PdfLoadedField field = this.Fields[index] as PdfLoadedField;
      string name = field.Name;
      string key = field.UpdateEncodedValue(name, (PdfDictionary) null);
      dictionary[key] = field;
    }
    foreach (KeyValuePair<string, List<string>> xdfdField in this.m_xdfdFields)
    {
      string key = xdfdField.Key;
      string str = (string) null;
      if (this.m_xfdfRichText != null && this.m_xfdfRichText.Count > 0 && this.m_xfdfRichText.ContainsKey(key))
        str = this.m_xfdfRichText[key];
      object FieldValue = (object) xdfdField.Value[0];
      if (xdfdField.Value.Count > 1)
      {
        string[] strArray = new string[xdfdField.Value.Count];
        for (int index = 0; index < xdfdField.Value.Count; ++index)
          strArray[index] = xdfdField.Value[index];
        FieldValue = (object) strArray;
      }
      PdfLoadedField field = (PdfLoadedField) null;
      try
      {
        if (!this.Fields.TryGetField(key, out field) && dictionary.ContainsKey(key))
          field = dictionary[key];
        if (field != null)
        {
          if (str != null)
            field.Dictionary.SetProperty("RV", (IPdfPrimitive) new PdfString(str));
          field.isAcrobat = this.m_settings.AsPerSpecification;
          field.ImportFieldValue(FieldValue);
        }
      }
      catch (Exception ex)
      {
        if (!continueImportOnError)
          throw;
        PdfLoadedFieldImportError fieldImportError = new PdfLoadedFieldImportError(field, ex);
        arrayList.Add((object) fieldImportError);
      }
    }
    return arrayList.Count == 0 ? (PdfLoadedFieldImportError[]) null : (PdfLoadedFieldImportError[]) arrayList.ToArray(typeof (PdfLoadedFieldImportError));
  }

  private void ImportXFDFNodes(XmlNodeList xmlKidNode, XmlNamespaceManager xmlNamespace)
  {
    for (int i1 = 0; i1 < xmlKidNode.Count; ++i1)
    {
      string str1 = "";
      XmlNode xmlNode1 = xmlKidNode[i1];
      if (xmlNode1.NodeType == XmlNodeType.Element)
      {
        if (xmlNode1.Attributes["name"] != null)
          str1 = xmlNode1.Attributes["name"].Value.Trim();
        if (str1.Length > 0)
        {
          XmlNodeList xmlKidNode1 = xmlNode1.SelectNodes("xfdf:field", xmlNamespace);
          if (xmlKidNode1.Count > 0)
          {
            this.ImportXFDFNodes(xmlKidNode1, xmlNamespace);
          }
          else
          {
            XmlNode xmlNode2 = xmlNode1.SelectSingleNode("xfdf:value", xmlNamespace);
            XmlNodeList xmlNodeList = xmlNode1.SelectNodes("xfdf:value", xmlNamespace);
            if (xmlNode2 != null)
            {
              XmlNode xmlNode3 = xmlNode1;
              string str2 = "";
              for (; xmlNode3.LocalName != "fields"; xmlNode3 = xmlNode3.ParentNode)
              {
                if (str2.Length > 0)
                  str2 = "." + str2;
                str2 = xmlNode3.Attributes["name"] == null || !(xmlNode3.Attributes["name"].Value != "") ? xmlNode3.LocalName + str2 : xmlNode3.Attributes["name"].Value + str2;
              }
              string key = str2;
              if (this.m_xdfdFields.ContainsKey(key))
              {
                this.m_xdfdFields[key].Add(xmlNode2.InnerText);
              }
              else
              {
                List<string> stringList = new List<string>();
                if (xmlNodeList != null && xmlNodeList.Count > 1)
                {
                  for (int i2 = 0; i2 < xmlNodeList.Count; ++i2)
                  {
                    XmlNode xmlNode4 = xmlNodeList[i2];
                    stringList.Add(xmlNode4.InnerText);
                  }
                  this.m_xdfdFields[key] = stringList;
                }
                else
                {
                  stringList.Add(xmlNode2.InnerText);
                  this.m_xdfdFields[key] = stringList;
                }
              }
            }
            else
            {
              XmlNode xmlNode5 = xmlNode1.SelectSingleNode("xfdf:value-richtext", xmlNamespace);
              if (xmlNode5 != null)
              {
                XmlNode xmlNode6 = xmlNode1;
                string str3 = "";
                for (; xmlNode6.LocalName != "fields"; xmlNode6 = xmlNode6.ParentNode)
                {
                  if (str3.Length > 0)
                    str3 = "." + str3;
                  str3 = xmlNode6.Attributes["name"] == null || !(xmlNode6.Attributes["name"].Value != "") ? xmlNode6.LocalName + str3 : xmlNode6.Attributes["name"].Value + str3;
                }
                string key = str3;
                string str4 = xmlNode5.InnerText;
                XmlNode childNode = xmlNode5.ChildNodes[0];
                if (childNode != null)
                {
                  XmlNodeList childNodes = childNode.ChildNodes;
                  if (childNodes != null)
                  {
                    string str5 = string.Empty;
                    for (int i3 = 0; i3 < childNodes.Count; ++i3)
                    {
                      XmlNode xmlNode7 = childNodes[i3];
                      str5 = $"{str5}{xmlNode7.InnerText}\r";
                    }
                    str4 = str5.Length <= 0 ? xmlNode5.InnerText : str5.Remove(str5.Length - 1, 1);
                  }
                }
                if (this.m_xdfdFields.ContainsKey(key))
                  this.m_xdfdFields[key].Add(str4);
                else
                  this.m_xdfdFields[key] = new List<string>()
                  {
                    str4
                  };
                if (!this.m_xfdfRichText.ContainsKey(key))
                  this.m_xfdfRichText[key] = xmlNode5.InnerXml;
              }
            }
          }
        }
      }
    }
  }

  public void ImportData(Stream stream, ImportFormSettings settings)
  {
    this.m_settings = settings;
    if (this.m_settings.DataFormat == DataFormat.Fdf)
      this.ImportDataFDF(stream);
    if (this.m_settings.DataFormat == DataFormat.XFdf)
      this.ImportDataXFDF(stream);
    if (this.m_settings.DataFormat != DataFormat.Xml)
      return;
    if (this.m_settings.IgnoreErrors)
      this.ImportDataXML(stream, true);
    else
      this.ImportDataXML(stream, false);
  }

  private class NodeInfo
  {
    private int m_count;
    private PdfArray m_fields;

    internal PdfArray Fields
    {
      get => this.m_fields;
      set => this.m_fields = value;
    }

    internal int Count
    {
      get => this.m_count;
      set => this.m_count = value;
    }

    internal NodeInfo(PdfArray fields, int count)
    {
      this.m_fields = fields;
      this.m_count = count;
    }
  }
}
