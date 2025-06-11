// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfLoadedXfaForm
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

public class PdfLoadedXfaForm : PdfLoadedXfaField
{
  private PdfDictionary m_catalog;
  private Dictionary<string, PdfStream> m_xfaArray = new Dictionary<string, PdfStream>();
  private PdfArray m_imageArray = new PdfArray();
  private PdfLoadedDocument m_loadedDocument;
  private List<PdfLoadedXfaField> tryGetfields;
  private int m_fieldCount = 1;
  internal XmlWriter dataSetWriter;
  internal XmlDocument xmlDoc;
  internal XmlDocument dataSetDoc;
  private int count;
  private int nodeCount;
  private List<string> m_completefieldNames = new List<string>();
  private float m_width;
  private float m_height;
  private PointF m_location;
  private PdfXfaVisibility m_visibility;
  private bool m_readOnly;
  internal SizeF m_size = SizeF.Empty;
  private PdfLoadedXfaFlowDirection m_flowDirection;
  internal PdfDocument fDocument;
  internal bool is_modified;
  internal PointF currentPoint = PointF.Empty;
  internal float maxHeight;
  internal float maxWidth;
  internal float extraSize;
  internal List<float> columnWidths = new List<float>();
  internal bool isLocationPresent;
  internal PointF startPoint = PointF.Empty;
  internal float trackingHeight;
  private PdfXfaBorder m_border;
  private bool isXfaImport;
  internal SizeF bgSize = SizeF.Empty;
  internal List<float> backgroundHeight = new List<float>();
  internal int bgHeightCounter;
  internal PdfLoadedXfaPageBreak PageBreak;
  internal PointF cStartPoint = PointF.Empty;

  internal PdfLoadedXfaFlowDirection FlowDirection
  {
    get => this.m_flowDirection;
    set => this.m_flowDirection = value;
  }

  internal PdfXfaBorder Border
  {
    get => this.m_border;
    set
    {
      if (value == null)
        return;
      this.m_border = value;
    }
  }

  public bool ReadOnly
  {
    get => this.m_readOnly;
    set => this.m_readOnly = value;
  }

  public new PdfXfaVisibility Visibility
  {
    get => this.m_visibility;
    set => this.m_visibility = value;
  }

  public float Width
  {
    get => this.m_width;
    set => this.m_width = value;
  }

  public float Height
  {
    get => this.m_height;
    set => this.m_height = value;
  }

  public PointF Location
  {
    get => this.m_location;
    set => this.m_location = value;
  }

  public PdfLoadedXfaFieldCollection Fields
  {
    get => this.m_fields;
    set => this.m_fields = value;
  }

  internal PdfDictionary Catalog
  {
    get => this.m_catalog;
    set => this.m_catalog = value;
  }

  internal Dictionary<string, PdfStream> XFAArray
  {
    get => this.m_xfaArray;
    set => this.m_xfaArray = value;
  }

  public string[] FieldNames => this.m_fieldNames.ToArray();

  public string[] SubFormNames => this.m_subFormNames.ToArray();

  public string[] CompleteFieldNames => this.m_completefieldNames.ToArray();

  public string[] AreaNames => this.m_areaNames.ToArray();

  internal void Load(PdfCatalog catalog)
  {
    this.Catalog = (PdfDictionary) catalog;
    PdfDictionary pdfDictionary1 = (PdfDictionary) catalog;
    if (pdfDictionary1 != null && pdfDictionary1.ContainsKey("AcroForm"))
    {
      PdfDictionary pdfDictionary2 = (PdfDictionary) null;
      if ((object) (pdfDictionary1["AcroForm"] as PdfReferenceHolder) != null)
        pdfDictionary2 = (pdfDictionary1["AcroForm"] as PdfReferenceHolder).Object as PdfDictionary;
      else if (pdfDictionary1["AcroForm"] is PdfDictionary)
        pdfDictionary2 = pdfDictionary1["AcroForm"] as PdfDictionary;
      if (pdfDictionary2 != null && pdfDictionary2.ContainsKey("XFA") && pdfDictionary2["XFA"] is PdfArray pdfArray)
      {
        for (int index = 0; index < pdfArray.Count; index += 2)
        {
          if (pdfArray[index] is PdfString)
          {
            string str = (pdfArray[index] as PdfString).Value;
            PdfStream pdfStream = (pdfArray[index + 1] as PdfReferenceHolder).Object as PdfStream;
            if (str == "template" || str == "datasets")
            {
              pdfStream.Clone(pdfStream.CrossTable);
              this.XFAArray.Add(str, pdfStream.ClonedObject as PdfStream);
            }
            else if (!this.XFAArray.ContainsKey(str))
              this.XFAArray.Add(str, pdfStream);
            else
              this.XFAArray.Add(this.GetName(str), pdfStream);
          }
        }
      }
    }
    if (this.m_xfaArray.Count <= 0)
      return;
    PdfStream xfa1 = this.m_xfaArray["template"];
    xfa1.Decompress();
    xfa1.InternalStream.Position = 0L;
    this.xmlDoc = new XmlDocument();
    this.xmlDoc.Load((Stream) xfa1.InternalStream);
    if (this.m_xfaArray.ContainsKey("datasets"))
    {
      PdfStream xfa2 = this.m_xfaArray["datasets"];
      xfa2.Decompress();
      this.dataSetDoc = new XmlDocument();
      this.dataSetDoc.LoadXml(Encoding.UTF8.GetString(xfa2.InternalStream.GetBuffer()));
    }
    this.Fields.parent = (PdfLoadedXfaField) this;
    this.ReadForm();
  }

  internal void Save(PdfLoadedDocument document)
  {
    this.acroForm = document.Form;
    if (this.acroForm != null)
      this.acroForm.SetDefaultAppearance(false);
    if (this.m_xfaArray.Count <= 0)
      return;
    this.m_loadedDocument = document;
    PdfStream pdfStream1 = new PdfStream();
    this.dataSetWriter = XmlWriter.Create((Stream) pdfStream1.InternalStream, new XmlWriterSettings()
    {
      Indent = true,
      OmitXmlDeclaration = true,
      Encoding = (Encoding) new UTF8Encoding(false)
    });
    this.dataSetWriter.WriteStartElement("xfa", "datasets", "http://www.xfa.org/schema/xfa-data/1.0/");
    this.dataSetWriter.WriteStartElement("xfa", "data", (string) null);
    this.SaveForm();
    this.dataSetWriter.WriteEndElement();
    this.dataSetWriter.WriteEndElement();
    this.dataSetWriter.Close();
    if (this.m_xfaArray.ContainsKey("datasets"))
    {
      if (this.isXfaImport)
      {
        pdfStream1.Dispose();
        pdfStream1 = new PdfStream();
        this.dataSetDoc.Save((Stream) pdfStream1.InternalStream);
      }
      this.m_xfaArray["datasets"] = pdfStream1;
    }
    else
    {
      Dictionary<string, PdfStream> dictionary = new Dictionary<string, PdfStream>();
      foreach (KeyValuePair<string, PdfStream> xfa in this.m_xfaArray)
      {
        dictionary.Add(xfa.Key, xfa.Value);
        if (xfa.Key == "template")
          dictionary.Add("datasets", pdfStream1);
      }
      this.m_xfaArray = dictionary;
    }
    if (this.Catalog.ContainsKey("Perms"))
    {
      PdfDictionary pdfDictionary = (object) (this.Catalog["Perms"] as PdfReferenceHolder) == null ? this.Catalog["Perms"] as PdfDictionary : (this.Catalog["Perms"] as PdfReferenceHolder).Object as PdfDictionary;
      if (!pdfDictionary.ContainsKey("UR3"))
      {
        PdfStream pdfStream2 = new PdfStream();
        pdfStream2.Write(this.xmlDoc.InnerXml);
        this.m_xfaArray["template"] = pdfStream2;
      }
      else if (this.is_modified)
      {
        pdfDictionary.Remove("UR3");
        PdfStream pdfStream3 = new PdfStream();
        pdfStream3.Write(this.xmlDoc.InnerXml);
        this.m_xfaArray["template"] = pdfStream3;
      }
    }
    else
    {
      PdfStream pdfStream4 = new PdfStream();
      pdfStream4.Write(this.xmlDoc.InnerXml);
      this.m_xfaArray["template"] = pdfStream4;
      byte[] array = pdfStream4.InternalStream.ToArray();
      Encoding.UTF8.GetString(array, 0, array.Length);
    }
    PdfDictionary primitive1 = new PdfDictionary();
    primitive1.SetProperty("Names", (IPdfPrimitive) this.m_imageArray);
    if (!this.m_loadedDocument.Catalog.ContainsKey("Names"))
    {
      PdfDictionary pdfDictionary = new PdfDictionary();
      pdfDictionary.SetProperty("XFAImages", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) primitive1));
      this.m_loadedDocument.Catalog.SetProperty("Names", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary));
    }
    else
    {
      PdfDictionary pdfDictionary = (this.m_catalog["Names"] as PdfReferenceHolder).Object as PdfDictionary;
      if (pdfDictionary.ContainsKey("XFAImages"))
      {
        PdfArray pdfArray = ((pdfDictionary["XFAImages"] as PdfReferenceHolder).Object as PdfDictionary)["Names"] as PdfArray;
        for (int index = 0; index < this.m_imageArray.Count; ++index)
          pdfArray.Add(this.m_imageArray[index]);
      }
      else if (this.m_imageArray.Count > 0)
        pdfDictionary.SetProperty("XFAImages", (IPdfPrimitive) primitive1);
    }
    if (!this.m_loadedDocument.Form.Dictionary.ContainsKey("XFA"))
      return;
    PdfResources pdfResources = new PdfResources();
    PdfArray primitive2 = new PdfArray();
    foreach (KeyValuePair<string, PdfStream> xfa in this.m_xfaArray)
    {
      if (xfa.Key.Contains("[") && xfa.Key.Contains("]"))
        primitive2.Add((IPdfPrimitive) new PdfString(xfa.Key.Split('[')[0]));
      else
        primitive2.Add((IPdfPrimitive) new PdfString(xfa.Key));
      primitive2.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) xfa.Value));
    }
    PdfDictionary catalog = this.m_catalog;
    if (catalog == null || !catalog.ContainsKey("AcroForm"))
      return;
    PdfDictionary pdfDictionary1 = (PdfDictionary) null;
    if ((object) (catalog["AcroForm"] as PdfReferenceHolder) != null)
    {
      PdfReferenceHolder pdfReferenceHolder = catalog["AcroForm"] as PdfReferenceHolder;
      pdfReferenceHolder.IsSaving = true;
      pdfDictionary1 = pdfReferenceHolder.Object as PdfDictionary;
    }
    else if (catalog["AcroForm"] is PdfDictionary)
      pdfDictionary1 = catalog["AcroForm"] as PdfDictionary;
    if (pdfDictionary1 == null || !pdfDictionary1.ContainsKey("XFA"))
      return;
    pdfDictionary1.Remove("XFA");
    pdfDictionary1.SetProperty("XFA", (IPdfPrimitive) primitive2);
  }

  internal void Save(
    bool flatten,
    PdfLoadedDocument document,
    PdfLoadedXfaDocument loadedXfaDocument)
  {
    if (this.xmlDoc == null)
      return;
    new PdfXfaFlattener(document).Flatten(this.xmlDoc, this, this.fDocument);
  }

  internal void SaveForm()
  {
    this.dataSetWriter.WriteStartElement(this.Name);
    this.SaveAttributes(this);
    this.SaveSubForms(this, this.dataSetWriter);
    this.dataSetWriter.WriteEndElement();
  }

  internal void SaveSubForms(PdfLoadedXfaForm form, XmlWriter dataSetWriter)
  {
    foreach (KeyValuePair<string, PdfXfaField> field in form.Fields.FieldCollection)
    {
      if (field.Value is PdfLoadedXfaForm)
      {
        PdfLoadedXfaForm pdfLoadedXfaForm = field.Value as PdfLoadedXfaForm;
        pdfLoadedXfaForm.acroForm = form.acroForm;
        this.SaveAttributes(pdfLoadedXfaForm);
        bool flag = true;
        if (this.dataSetDoc != null)
        {
          string[] strArray = pdfLoadedXfaForm.nodeName.Split('[');
          string str1 = string.Empty;
          foreach (string str2 in strArray)
          {
            if (str2.Contains("]"))
            {
              int startIndex = str2.IndexOf(']') + 2;
              if (str2.Length > startIndex)
                str1 = $"{str1}/{str2.Substring(startIndex)}";
            }
            else
              str1 += str2;
          }
          while (str1.Contains("#"))
          {
            int startIndex1 = str1.IndexOf("#");
            if (startIndex1 != -1)
            {
              string str3 = str1.Substring(0, startIndex1 - 1);
              string str4 = str1.Substring(startIndex1);
              int startIndex2 = str4.IndexOf("/");
              string str5 = string.Empty;
              if (startIndex2 != -1)
                str5 = str4.Substring(startIndex2);
              str1 = (str3 + str5).TrimEnd('/');
            }
          }
          if (this.dataSetDoc.SelectSingleNode("//" + str1) == null)
            flag = false;
        }
        if (!string.IsNullOrEmpty(pdfLoadedXfaForm.Name) && !pdfLoadedXfaForm.Name.Contains("#subform") && flag)
          dataSetWriter.WriteStartElement(pdfLoadedXfaForm.Name);
        pdfLoadedXfaForm.dataSetDoc = form.dataSetDoc;
        form.SaveSubForms(pdfLoadedXfaForm, dataSetWriter);
        if (!string.IsNullOrEmpty(pdfLoadedXfaForm.Name) && !pdfLoadedXfaForm.Name.Contains("#subform") && flag)
          dataSetWriter.WriteEndElement();
      }
      else if (field.Value is PdfLoadedXfaArea)
      {
        PdfLoadedXfaArea area = field.Value as PdfLoadedXfaArea;
        area.acroForm = form.acroForm;
        bool flag = true;
        if (this.dataSetDoc != null)
        {
          string[] strArray = area.nodeName.Split('[');
          string str6 = string.Empty;
          foreach (string str7 in strArray)
          {
            if (str7.Contains("]"))
            {
              int startIndex = str7.IndexOf(']') + 2;
              if (str7.Length > startIndex)
                str6 = $"{str6}/{str7.Substring(startIndex)}";
            }
            else
              str6 += str7;
          }
          while (str6.Contains("#"))
          {
            int startIndex3 = str6.IndexOf("#");
            if (startIndex3 != -1)
            {
              string str8 = str6.Substring(0, startIndex3 - 1);
              string str9 = str6.Substring(startIndex3);
              int startIndex4 = str9.IndexOf("/");
              string str10 = string.Empty;
              if (startIndex4 != -1)
                str10 = str9.Substring(startIndex4);
              str6 = (str8 + str10).TrimEnd('/');
            }
          }
          if (this.dataSetDoc.SelectSingleNode("//" + str6) == null)
            flag = false;
        }
        if (!string.IsNullOrEmpty(area.Name) && !area.Name.Contains("#area") && flag)
          dataSetWriter.WriteStartElement(area.Name);
        area.Save(area, dataSetWriter, this.dataSetDoc, form);
        if (!string.IsNullOrEmpty(area.Name) && !area.Name.Contains("#area") && flag)
          dataSetWriter.WriteEndElement();
      }
      else if (field.Value is PdfLoadedXfaTextBoxField)
      {
        if (field.Value is PdfLoadedXfaTextBoxField loadedXfaTextBoxField)
          loadedXfaTextBoxField.Fill(dataSetWriter, form);
      }
      else if (field.Value is PdfLoadedXfaNumericField)
      {
        if (field.Value is PdfLoadedXfaNumericField loadedXfaNumericField)
          loadedXfaNumericField.Fill(dataSetWriter, form);
      }
      else if (field.Value is PdfLoadedXfaDateTimeField)
      {
        if (field.Value is PdfLoadedXfaDateTimeField xfaDateTimeField)
          xfaDateTimeField.Fill(dataSetWriter, form);
      }
      else if (field.Value is PdfLoadedXfaCheckBoxField)
      {
        if (field.Value is PdfLoadedXfaCheckBoxField xfaCheckBoxField)
          xfaCheckBoxField.Fill(dataSetWriter, form);
      }
      else if (field.Value is PdfLoadedXfaRadioButtonGroup)
      {
        if (field.Value is PdfLoadedXfaRadioButtonGroup radioButtonGroup)
          radioButtonGroup.Fill(dataSetWriter, form);
      }
      else if (field.Value is PdfLoadedXfaListBoxField)
      {
        if (field.Value is PdfLoadedXfaListBoxField loadedXfaListBoxField)
          loadedXfaListBoxField.Fill(dataSetWriter, form);
      }
      else if (field.Value is PdfLoadedXfaComboBoxField)
      {
        if (field.Value is PdfLoadedXfaComboBoxField xfaComboBoxField)
          xfaComboBoxField.Fill(dataSetWriter, form);
      }
      else if (field.Value != null && !(field.Value is PdfLoadedXfaField))
        this.SaveNewXfaItems(form, field.Value);
    }
  }

  internal List<PdfLoadedXfaCheckBoxField> GetSameNameFields(string name, PdfLoadedXfaForm form)
  {
    List<PdfLoadedXfaCheckBoxField> sameNameFields = new List<PdfLoadedXfaCheckBoxField>();
    foreach (string fieldName in form.FieldNames)
    {
      if (fieldName.Contains(name) && form.Fields[fieldName] is PdfLoadedXfaCheckBoxField field)
        sameNameFields.Add(field);
    }
    return sameNameFields;
  }

  internal void ReadForm()
  {
    XmlNode xmlNode = this.xmlDoc.GetElementsByTagName("subform")[0];
    this.ReadSubFormProperties(xmlNode, this, (PdfLoadedXfaForm) null);
    this.currentNode = xmlNode;
    this.SetName((PdfLoadedXfaField) this, this.internalSubFormNames, false);
    this.ReadSubForm(xmlNode, this, this.m_fieldNames, this.m_subFormNames);
    this.GetCompeleteFieldNames(this);
  }

  private void ReadColumnWidth(XmlNode node, PdfLoadedXfaForm subform)
  {
    string str1 = node.Attributes["columnWidths"].Value;
    if (!(str1 != string.Empty))
      return;
    string str2 = str1;
    char[] chArray = new char[1]{ ' ' };
    foreach (string str3 in str2.Split(chArray))
      subform.columnWidths.Add(this.ConvertToPoint(str3));
  }

  internal void ReadSubFormProperties(XmlNode node, PdfLoadedXfaForm ff, PdfLoadedXfaForm form)
  {
    if (node.Attributes["x"] != null)
    {
      ff.Location = new PointF(this.ConvertToPoint(node.Attributes["x"].Value), ff.Location.Y);
      this.isLocationPresent = true;
    }
    if (node.Attributes["y"] != null)
    {
      ff.Location = new PointF(ff.Location.X, this.ConvertToPoint(node.Attributes["y"].Value));
      this.isLocationPresent = true;
    }
    if (node.Attributes["w"] != null)
      ff.Width = this.ConvertToPoint(node.Attributes["w"].Value);
    if (node.Attributes["h"] != null)
      ff.Height = this.ConvertToPoint(node.Attributes["h"].Value);
    if (node.Attributes["layout"] != null)
    {
      switch (node.Attributes["layout"].Value)
      {
        case "tb":
          ff.FlowDirection = PdfLoadedXfaFlowDirection.TopToBottom;
          break;
        case "lr-tb":
          ff.FlowDirection = PdfLoadedXfaFlowDirection.LeftToRight;
          break;
        case "rl-tb":
          ff.FlowDirection = PdfLoadedXfaFlowDirection.RightToLeft;
          break;
        case "row":
          ff.FlowDirection = PdfLoadedXfaFlowDirection.Row;
          break;
        case "table":
          ff.FlowDirection = PdfLoadedXfaFlowDirection.Table;
          break;
        default:
          ff.FlowDirection = PdfLoadedXfaFlowDirection.None;
          break;
      }
    }
    if (node.Attributes["columnWidths"] != null)
      this.ReadColumnWidth(node, ff);
    if (node["bind"] != null)
    {
      if (form != null && form.bindingName != string.Empty)
        ff.bindingName = $"{form.bindingName}.{this.ReadBinding((XmlNode) node["bind"])}";
      else
        ff.bindingName = this.ReadBinding((XmlNode) node["bind"]);
    }
    if (node["margin"] != null)
      ff.ReadMargin((XmlNode) node["margin"]);
    if (node["border"] == null)
      return;
    ff.m_border = new PdfXfaBorder();
    ff.m_border.Read((XmlNode) node["border"]);
  }

  internal void ReadSubForm(
    XmlNode subNode,
    PdfLoadedXfaForm form,
    List<string> fieldsNames,
    List<string> subFormNames)
  {
    foreach (XmlNode xmlNode in subNode)
    {
      switch (xmlNode.Name)
      {
        case "subform":
        case "subformSet":
          PdfLoadedXfaForm pdfLoadedXfaForm = new PdfLoadedXfaForm();
          pdfLoadedXfaForm.Fields.parent = (PdfLoadedXfaField) pdfLoadedXfaForm;
          pdfLoadedXfaForm.parent = (PdfLoadedXfaField) form;
          pdfLoadedXfaForm.currentNode = xmlNode;
          this.SetName((PdfLoadedXfaField) pdfLoadedXfaForm, form.internalSubFormNames, false);
          this.ReadSubFormProperties(xmlNode, pdfLoadedXfaForm, form);
          if (!form.internalSubFormNames.Contains(pdfLoadedXfaForm.m_name))
            form.internalSubFormNames.Add(pdfLoadedXfaForm.m_name);
          if (pdfLoadedXfaForm.isUnNamedSubForm)
          {
            pdfLoadedXfaForm.internalFieldNames = form.internalFieldNames;
            pdfLoadedXfaForm.internalSubFormNames = form.internalSubFormNames;
          }
          this.ReadSubForm(xmlNode, pdfLoadedXfaForm, fieldsNames, subFormNames);
          form.Fields.Add((PdfLoadedXfaField) pdfLoadedXfaForm, pdfLoadedXfaForm.m_name);
          form.m_subFormNames.Add(pdfLoadedXfaForm.m_name);
          if (form != this)
            subFormNames.Add(pdfLoadedXfaForm.m_name);
          if (pdfLoadedXfaForm.isUnNamedSubForm)
          {
            form.internalFieldNames = pdfLoadedXfaForm.internalFieldNames;
            form.internalSubFormNames = pdfLoadedXfaForm.internalSubFormNames;
            continue;
          }
          continue;
        case "field":
          this.ReadField(xmlNode, form, fieldsNames, subFormNames, this.dataSetDoc);
          continue;
        case "draw":
          this.ReadStaticField(xmlNode, form, this.dataSetDoc);
          continue;
        case "exclGroup":
          PdfLoadedXfaRadioButtonGroup field = new PdfLoadedXfaRadioButtonGroup();
          field.parent = (PdfLoadedXfaField) form;
          field.ReadField(xmlNode, this.dataSetDoc);
          string fieldName = this.GetFieldName(form.internalFieldNames, field.Name);
          form.Fields.Add((PdfLoadedXfaField) field, fieldName);
          form.m_fieldNames.Add(fieldName);
          PdfLoadedXfaRadioButtonGroup radioButtonGroup = field;
          radioButtonGroup.nodeName = $"{radioButtonGroup.nodeName}.{fieldName}";
          fieldsNames.Add(fieldName);
          if (!form.internalFieldNames.Contains(fieldName))
          {
            form.internalFieldNames.Add(fieldName);
            continue;
          }
          continue;
        case "area":
          PdfLoadedXfaArea pdfLoadedXfaArea = new PdfLoadedXfaArea();
          pdfLoadedXfaArea.Fields.parent = (PdfLoadedXfaField) pdfLoadedXfaArea;
          pdfLoadedXfaArea.parent = (PdfLoadedXfaField) form;
          pdfLoadedXfaArea.currentNode = xmlNode;
          this.SetName((PdfLoadedXfaField) pdfLoadedXfaArea, form.internalSubFormNames, true);
          if (!form.m_subFormNames.Contains(pdfLoadedXfaArea.m_name))
            form.internalSubFormNames.Add(pdfLoadedXfaArea.m_name);
          pdfLoadedXfaArea.internalFieldNames = form.internalFieldNames;
          pdfLoadedXfaArea.internalSubFormNames = form.internalSubFormNames;
          pdfLoadedXfaArea.ReadArea(xmlNode, pdfLoadedXfaArea, form, fieldsNames, subFormNames, this.dataSetDoc);
          form.internalFieldNames = pdfLoadedXfaArea.internalFieldNames;
          form.internalSubFormNames = pdfLoadedXfaArea.internalSubFormNames;
          form.Fields.Add((PdfLoadedXfaField) pdfLoadedXfaArea, pdfLoadedXfaArea.m_name);
          form.m_areaNames.Add(pdfLoadedXfaArea.m_name);
          continue;
        case "break":
        case "overflow":
        case "breakBefore":
        case "breakAfter":
          form.PageBreak = new PdfLoadedXfaPageBreak();
          form.PageBreak.Read(xmlNode);
          continue;
        default:
          continue;
      }
    }
  }

  public PdfLoadedXfaField[] TryGetFieldsByName(string name)
  {
    this.tryGetfields = new List<PdfLoadedXfaField>();
    this.GetFields(name, this, false);
    this.is_modified = true;
    return this.tryGetfields.ToArray();
  }

  [Obsolete("This method has been deprecated. Use TryGetFieldByCompleteName(string name) to get field by using complete name")]
  public PdfLoadedXfaField TryGetFieldByCompeleteName(string name)
  {
    name = name.Replace("].", "]/");
    string[] strArray = name.Split('/');
    PdfLoadedXfaField fieldByCompeleteName = (PdfLoadedXfaField) null;
    PdfLoadedXfaField pdfLoadedXfaField = (PdfLoadedXfaField) this;
    this.is_modified = true;
    for (int index = 1; index < strArray.Length; ++index)
    {
      PdfLoadedXfaField[] fieldsByName = (pdfLoadedXfaField as PdfLoadedXfaForm).TryGetFieldsByName(strArray[index], true);
      if (fieldsByName.Length <= 0)
        return (PdfLoadedXfaField) null;
      if (!(fieldsByName[0] is PdfLoadedXfaForm))
        return fieldsByName[0];
      pdfLoadedXfaField = (PdfLoadedXfaField) (fieldsByName[0] as PdfLoadedXfaForm);
    }
    return fieldByCompeleteName;
  }

  public PdfLoadedXfaField TryGetFieldByCompleteName(string name)
  {
    name = name.Replace("].", "]/");
    string[] strArray = name.Split('/');
    PdfLoadedXfaField fieldByCompleteName = (PdfLoadedXfaField) null;
    PdfLoadedXfaField pdfLoadedXfaField = (PdfLoadedXfaField) this;
    this.is_modified = true;
    for (int index = 1; index < strArray.Length; ++index)
    {
      PdfLoadedXfaField[] fieldsByName = (pdfLoadedXfaField as PdfLoadedXfaForm).TryGetFieldsByName(strArray[index], true);
      if (fieldsByName.Length <= 0)
        return (PdfLoadedXfaField) null;
      if (!(fieldsByName[0] is PdfLoadedXfaForm))
        return fieldsByName[0];
      pdfLoadedXfaField = (PdfLoadedXfaField) (fieldsByName[0] as PdfLoadedXfaForm);
    }
    return fieldByCompleteName;
  }

  public void ImportXfaData(string fileName)
  {
    if (!File.Exists(fileName))
      throw new FileNotFoundException(fileName);
    this.isXfaImport = true;
    this.dataSetDoc = new XmlDocument();
    FileStream inStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
    this.dataSetDoc.Load((Stream) inStream);
    inStream.Close();
    inStream.Dispose();
  }

  public void ImportXfaData(Stream stream)
  {
    if (stream == null || this.dataSetDoc == null)
      return;
    this.isXfaImport = true;
    this.dataSetDoc.Load(stream);
  }

  public void ExportXfaData(string fileName)
  {
    if (this.dataSetDoc == null)
      return;
    this.dataSetDoc.Save(fileName);
  }

  public void ExportXfaData(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    if (this.dataSetDoc == null)
      return;
    this.dataSetDoc.Save(stream);
  }

  private string GetName(string name)
  {
    int num = 0;
    string key;
    do
    {
      key = $"{name}[{num++.ToString()}]";
    }
    while (this.XFAArray.ContainsKey(key));
    return key;
  }

  private void SaveAttributes(PdfLoadedXfaForm lForm)
  {
    if (!lForm.ReadOnly)
      return;
    if (lForm.currentNode.Attributes["access"] != null)
    {
      lForm.currentNode.Attributes["access"].Value = "readOnly";
    }
    else
    {
      XmlAttribute attribute = this.xmlDoc.CreateAttribute("access");
      attribute.InnerText = "readOnly";
      lForm.currentNode.Attributes.Append(attribute);
    }
  }

  private void GetCompeleteFieldNames(PdfLoadedXfaForm form)
  {
    foreach (KeyValuePair<string, PdfXfaField> field in form.Fields.FieldCollection)
    {
      if (field.Value is PdfLoadedXfaForm)
        this.GetCompeleteFieldNames(field.Value as PdfLoadedXfaForm);
      else if (field.Value is PdfLoadedXfaArea)
        this.GetCompeleteFieldNames(field.Value as PdfLoadedXfaArea);
      else
        this.m_completefieldNames.Add((field.Value as PdfLoadedXfaField).nodeName);
    }
  }

  private void GetCompeleteFieldNames(PdfLoadedXfaArea form)
  {
    foreach (KeyValuePair<string, PdfXfaField> field in form.Fields.FieldCollection)
    {
      if (field.Value is PdfLoadedXfaForm)
        this.GetCompeleteFieldNames(field.Value as PdfLoadedXfaForm);
      else if (field.Value is PdfLoadedXfaArea)
        this.GetCompeleteFieldNames(field.Value as PdfLoadedXfaArea);
      else
        this.m_completefieldNames.Add((field.Value as PdfLoadedXfaField).nodeName);
    }
  }

  private void GetFields(string name, PdfLoadedXfaForm form, bool isTryGetFlag)
  {
    foreach (KeyValuePair<string, PdfXfaField> field in form.Fields.FieldCollection)
    {
      if (field.Key == name)
        this.tryGetfields.Add(field.Value as PdfLoadedXfaField);
      if (field.Value is PdfLoadedXfaForm && !isTryGetFlag)
      {
        PdfLoadedXfaForm form1 = field.Value as PdfLoadedXfaForm;
        if (form1.Name == name)
          this.tryGetfields.Add((PdfLoadedXfaField) form1);
        this.GetFields(name, form1, isTryGetFlag);
      }
    }
  }

  private void SaveNewXfaItems(PdfLoadedXfaForm loadedXfa, PdfXfaField field)
  {
    XfaWriter xfaWriter = new XfaWriter();
    XmlWriterSettings settings = new XmlWriterSettings();
    settings.Encoding = (Encoding) new UTF8Encoding(false);
    settings.OmitXmlDeclaration = true;
    settings.Indent = true;
    MemoryStream output = new MemoryStream();
    xfaWriter.Write = XmlWriter.Create((Stream) output, settings);
    switch (field)
    {
      case PdfXfaImage _:
        PdfXfaImage pdfXfaImage = field as PdfXfaImage;
        string imageName = Guid.NewGuid().ToString();
        if (!pdfXfaImage.isBase64Type)
        {
          this.m_imageArray.Add((IPdfPrimitive) new PdfString(imageName));
          this.m_imageArray.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfXfaImage.ImageStream));
        }
        pdfXfaImage.Save(this.m_fieldCount++, imageName, xfaWriter);
        break;
      case PdfXfaTextBoxField _:
        PdfXfaTextBoxField pdfXfaTextBoxField = field as PdfXfaTextBoxField;
        this.dataSetWriter.WriteStartElement(pdfXfaTextBoxField.Name);
        this.dataSetWriter.WriteString(pdfXfaTextBoxField.Text);
        this.dataSetWriter.WriteEndElement();
        pdfXfaTextBoxField.Save(xfaWriter);
        break;
      case PdfXfaForm _:
        PdfXfaForm pdfXfaForm = field as PdfXfaForm;
        pdfXfaForm.m_dataSetWriter = this.dataSetWriter;
        pdfXfaForm.AddSubForm(xfaWriter);
        break;
      case PdfXfaCheckBoxField _:
        PdfXfaCheckBoxField xfaCheckBoxField = field as PdfXfaCheckBoxField;
        this.dataSetWriter.WriteStartElement(xfaCheckBoxField.Name);
        if (xfaCheckBoxField.IsChecked)
          this.dataSetWriter.WriteString("1");
        else
          this.dataSetWriter.WriteString("0");
        this.dataSetWriter.WriteEndElement();
        xfaCheckBoxField.Save(xfaWriter);
        break;
      case PdfXfaRadioButtonGroup _:
        PdfXfaRadioButtonGroup radioButtonGroup = field as PdfXfaRadioButtonGroup;
        this.dataSetWriter.WriteStartElement(radioButtonGroup.Name);
        radioButtonGroup.Save(xfaWriter);
        if (radioButtonGroup.selectedItem > 0)
          this.dataSetWriter.WriteString(radioButtonGroup.selectedItem.ToString());
        this.dataSetWriter.WriteEndElement();
        break;
      case PdfXfaComboBoxField _:
        PdfXfaComboBoxField xfaComboBoxField = field as PdfXfaComboBoxField;
        this.dataSetWriter.WriteStartElement(xfaComboBoxField.Name);
        if (xfaComboBoxField.SelectedValue != null)
        {
          if (xfaComboBoxField.Items.Contains(xfaComboBoxField.SelectedValue))
            this.dataSetWriter.WriteString(xfaComboBoxField.SelectedValue);
        }
        else if (xfaComboBoxField.SelectedIndex > 0 && xfaComboBoxField.SelectedIndex - 1 <= xfaComboBoxField.Items.Count)
          this.dataSetWriter.WriteString(xfaComboBoxField.Items[xfaComboBoxField.SelectedIndex - 1]);
        this.dataSetWriter.WriteEndElement();
        xfaComboBoxField.Save(xfaWriter);
        break;
      case PdfXfaListBoxField _:
        PdfXfaListBoxField pdfXfaListBoxField = field as PdfXfaListBoxField;
        this.dataSetWriter.WriteStartElement(pdfXfaListBoxField.Name);
        if (pdfXfaListBoxField.SelectedValue != null)
        {
          if (pdfXfaListBoxField.Items.Contains(pdfXfaListBoxField.SelectedValue))
            this.dataSetWriter.WriteString(pdfXfaListBoxField.SelectedValue);
        }
        else if (pdfXfaListBoxField.SelectedIndex > 0 && pdfXfaListBoxField.SelectedIndex - 1 <= pdfXfaListBoxField.Items.Count)
          this.dataSetWriter.WriteString(pdfXfaListBoxField.Items[pdfXfaListBoxField.SelectedIndex - 1]);
        this.dataSetWriter.WriteEndElement();
        pdfXfaListBoxField.Save(xfaWriter);
        break;
      case PdfXfaCircleField _:
        (field as PdfXfaCircleField).Save(xfaWriter);
        break;
      case PdfXfaRectangleField _:
        (field as PdfXfaRectangleField).Save(xfaWriter);
        break;
      case PdfXfaLine _:
        (field as PdfXfaLine).Save(xfaWriter);
        break;
      case PdfXfaDateTimeField _:
        PdfXfaDateTimeField xfaDateTimeField = field as PdfXfaDateTimeField;
        DateTime dateTime = xfaDateTimeField.Value;
        this.dataSetWriter.WriteStartElement(xfaDateTimeField.Name);
        if (xfaDateTimeField.Format == PdfXfaDateTimeFormat.Date)
          this.dataSetWriter.WriteString(xfaDateTimeField.Value.ToString(xfaWriter.GetDatePattern(xfaDateTimeField.DatePattern)));
        else if (xfaDateTimeField.Format == PdfXfaDateTimeFormat.DateTime)
          this.dataSetWriter.WriteString(xfaDateTimeField.Value.ToString(xfaWriter.GetDateTimePattern(xfaDateTimeField.DatePattern, xfaDateTimeField.TimePattern)));
        else if (xfaDateTimeField.Format == PdfXfaDateTimeFormat.Time)
          this.dataSetWriter.WriteString(xfaDateTimeField.Value.ToString(xfaWriter.GetTimePattern(xfaDateTimeField.TimePattern)));
        this.dataSetWriter.WriteEndElement();
        xfaDateTimeField.Save(xfaWriter);
        break;
      case PdfXfaNumericField _:
        PdfXfaNumericField pdfXfaNumericField = field as PdfXfaNumericField;
        this.dataSetWriter.WriteStartElement(pdfXfaNumericField.Name);
        if (!double.IsNaN(pdfXfaNumericField.NumericValue))
        {
          if (pdfXfaNumericField.FieldType == PdfXfaNumericType.Integer)
            pdfXfaNumericField.NumericValue = (double) (int) pdfXfaNumericField.NumericValue;
          this.dataSetWriter.WriteString(pdfXfaNumericField.NumericValue.ToString());
        }
        this.dataSetWriter.WriteEndElement();
        pdfXfaNumericField.Save(xfaWriter);
        break;
      case PdfXfaTextElement _:
        (field as PdfXfaTextElement).Save(xfaWriter);
        break;
      case PdfXfaButtonField _:
        (field as PdfXfaButtonField).Save(xfaWriter);
        break;
    }
    xfaWriter.Write.Close();
    XmlDocumentFragment documentFragment = this.xmlDoc.CreateDocumentFragment();
    documentFragment.InnerXml = Encoding.Default.GetString(output.GetBuffer());
    loadedXfa.currentNode.AppendChild((XmlNode) documentFragment);
  }

  private PdfLoadedXfaField[] TryGetFieldsByName(string name, bool isTryGetFlag)
  {
    this.tryGetfields = new List<PdfLoadedXfaField>();
    this.GetFields(name, this, isTryGetFlag);
    this.is_modified = true;
    return this.tryGetfields.ToArray();
  }
}
