// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfLoadedXfaArea
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

public class PdfLoadedXfaArea : PdfLoadedXfaField
{
  public PdfLoadedXfaFieldCollection Fields
  {
    get => this.m_fields;
    internal set => this.m_fields = value;
  }

  internal PdfLoadedXfaArea()
  {
  }

  internal void Save(
    PdfLoadedXfaArea area,
    XmlWriter dataSetWriter,
    XmlDocument dataSetDoc,
    PdfLoadedXfaForm form)
  {
    foreach (KeyValuePair<string, PdfXfaField> field in area.Fields.FieldCollection)
    {
      if (field.Value is PdfLoadedXfaForm)
      {
        PdfLoadedXfaForm form1 = field.Value as PdfLoadedXfaForm;
        form1.acroForm = form.acroForm;
        form1.SaveSubForms(form1, dataSetWriter);
      }
      if (field.Value is PdfLoadedXfaArea)
      {
        PdfLoadedXfaArea area1 = field.Value as PdfLoadedXfaArea;
        area1.acroForm = form.acroForm;
        bool flag = true;
        if (dataSetDoc != null)
        {
          string[] strArray = area1.nodeName.Split('[');
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
          string xpath = "//" + str1;
          if (dataSetDoc.SelectSingleNode(xpath) == null)
            flag = false;
        }
        if (area1.Name != null && !area1.Name.Contains("#subform") && flag)
          dataSetWriter.WriteStartElement(area1.Name);
        this.Save(area1, dataSetWriter, dataSetDoc, form);
        if (area1.Name != null && !area1.Name.Contains("#subform") && flag)
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
      else if (field.Value is PdfLoadedXfaComboBoxField && field.Value is PdfLoadedXfaComboBoxField xfaComboBoxField)
        xfaComboBoxField.Fill(dataSetWriter, form);
    }
  }

  internal void ReadArea(
    XmlNode subNode,
    PdfLoadedXfaArea area,
    PdfLoadedXfaForm form,
    List<string> fieldsNames,
    List<string> subFormNames,
    XmlDocument dataSetDoc)
  {
    foreach (XmlNode xmlNode in subNode)
    {
      switch (xmlNode.Name)
      {
        case "subform":
          PdfLoadedXfaForm pdfLoadedXfaForm = new PdfLoadedXfaForm();
          pdfLoadedXfaForm.parent = (PdfLoadedXfaField) area;
          pdfLoadedXfaForm.ReadSubFormProperties(xmlNode, pdfLoadedXfaForm, form);
          pdfLoadedXfaForm.currentNode = xmlNode;
          this.SetName((PdfLoadedXfaField) pdfLoadedXfaForm, area.internalSubFormNames, false);
          if (!area.internalSubFormNames.Contains(pdfLoadedXfaForm.m_name))
            area.internalSubFormNames.Add(pdfLoadedXfaForm.m_name);
          if (pdfLoadedXfaForm.isUnNamedSubForm)
          {
            pdfLoadedXfaForm.internalFieldNames = area.internalFieldNames;
            pdfLoadedXfaForm.internalSubFormNames = area.internalSubFormNames;
          }
          pdfLoadedXfaForm.ReadSubForm(xmlNode, pdfLoadedXfaForm, fieldsNames, subFormNames);
          area.Fields.Add((PdfLoadedXfaField) pdfLoadedXfaForm, pdfLoadedXfaForm.m_name);
          area.m_subFormNames.Add(pdfLoadedXfaForm.m_name);
          subFormNames.Add(pdfLoadedXfaForm.m_name);
          if (pdfLoadedXfaForm.isUnNamedSubForm)
          {
            area.internalFieldNames = pdfLoadedXfaForm.internalFieldNames;
            area.internalSubFormNames = pdfLoadedXfaForm.internalSubFormNames;
            continue;
          }
          continue;
        case "field":
          this.ReadField(xmlNode, area, fieldsNames, subFormNames, dataSetDoc);
          continue;
        case "exclGroup":
          PdfLoadedXfaRadioButtonGroup field = new PdfLoadedXfaRadioButtonGroup();
          field.parent = (PdfLoadedXfaField) area;
          field.ReadField(xmlNode, dataSetDoc);
          string fieldName = this.GetFieldName(area.internalFieldNames, field.Name);
          area.Fields.Add((PdfLoadedXfaField) field, fieldName);
          field.nodeName = $"{area.nodeName}.{fieldName}";
          area.m_fieldNames.Add(fieldName);
          fieldsNames.Add(fieldName);
          if (!area.internalFieldNames.Contains(fieldName))
          {
            area.internalFieldNames.Add(fieldName);
            continue;
          }
          continue;
        case nameof (area):
          PdfLoadedXfaArea pdfLoadedXfaArea = new PdfLoadedXfaArea();
          pdfLoadedXfaArea.parent = (PdfLoadedXfaField) area;
          pdfLoadedXfaArea.currentNode = xmlNode;
          this.SetName((PdfLoadedXfaField) pdfLoadedXfaArea, area.internalSubFormNames, true);
          if (!area.internalSubFormNames.Contains(pdfLoadedXfaArea.m_name))
            area.internalSubFormNames.Add(pdfLoadedXfaArea.m_name);
          pdfLoadedXfaArea.internalFieldNames = area.internalFieldNames;
          pdfLoadedXfaArea.internalSubFormNames = area.internalSubFormNames;
          this.ReadArea(xmlNode, pdfLoadedXfaArea, form, fieldsNames, subFormNames, dataSetDoc);
          area.internalFieldNames = pdfLoadedXfaArea.internalFieldNames;
          area.internalSubFormNames = pdfLoadedXfaArea.internalSubFormNames;
          area.Fields.Add((PdfLoadedXfaField) pdfLoadedXfaArea, pdfLoadedXfaArea.m_name);
          area.m_areaNames.Add(pdfLoadedXfaArea.m_name);
          area.internalSubFormNames.Add(pdfLoadedXfaArea.m_name);
          continue;
        default:
          continue;
      }
    }
  }
}
