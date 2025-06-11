// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfLoadedXfaField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

public abstract class PdfLoadedXfaField : PdfXfaField
{
  internal XmlNode currentNode;
  internal XmlNode dataSetNode;
  internal string nodeName = string.Empty;
  internal PdfLoadedXfaField parent;
  internal string bindingName = string.Empty;
  internal bool isRendered;
  internal List<string> m_fieldNames = new List<string>();
  internal List<string> m_subFormNames = new List<string>();
  internal List<string> internalFieldNames = new List<string>();
  internal List<string> internalSubFormNames = new List<string>();
  internal bool isUnNamedSubForm;
  internal List<string> m_areaNames = new List<string>();
  internal PdfLoadedXfaFieldCollection m_fields = new PdfLoadedXfaFieldCollection();
  internal PdfLoadedForm acroForm;
  internal string m_name = string.Empty;

  internal string GetFieldName(List<string> fieldNameList, string name)
  {
    string fieldName = name + "[0]";
    if (fieldNameList.Contains(fieldName))
    {
      int num1 = 1;
      string str1 = name;
      int num2 = num1;
      int num3 = num2 + 1;
      string str2 = num2.ToString();
      fieldName = $"{str1}[{str2}]";
      while (fieldNameList.Contains(fieldName))
        fieldName = $"{name}[{num3++.ToString()}]";
    }
    return fieldName;
  }

  internal int GetSameNameFieldsCount(string name)
  {
    int sameNameFieldsCount = 0;
    char[] chArray = new char[1]{ '[' };
    foreach (string internalFieldName in this.internalFieldNames)
    {
      if (internalFieldName.Split(chArray)[0] == name)
        ++sameNameFieldsCount;
    }
    return sameNameFieldsCount;
  }

  internal void SetName(PdfLoadedXfaField field, List<string> subFormNamesCollection, bool isArea)
  {
    if (field.currentNode.Attributes != null && field.currentNode.Attributes["name"] != null)
    {
      field.Name = field.currentNode.Attributes["name"].Value;
    }
    else
    {
      field.isUnNamedSubForm = true;
      field.Name = isArea ? "#area" : "#subform";
    }
    field.m_name = this.GetFieldName(subFormNamesCollection, field.Name);
    if (field.parent != null && !string.IsNullOrEmpty(field.parent.nodeName))
      field.nodeName = $"{field.parent.nodeName}.{field.m_name}";
    else
      field.nodeName = field.m_name;
  }

  internal float ConvertToPoint(string value)
  {
    float point = 0.0f;
    if (value.Contains("pt"))
      point = Convert.ToSingle(value.Trim('p', 't', 'm'), (IFormatProvider) CultureInfo.InvariantCulture);
    else if (value.Contains("cm"))
      point = Convert.ToSingle(value.Trim('p', 't', 'c', 'm'), (IFormatProvider) CultureInfo.InvariantCulture) * 28.3464565f;
    else if (value.Contains("m"))
    {
      float single;
      try
      {
        single = Convert.ToSingle(value.Trim('p', 't', 'm'), (IFormatProvider) CultureInfo.InvariantCulture);
      }
      catch
      {
        single = Convert.ToSingle(Regex.Replace(value, "[^0-9.-]", ""), (IFormatProvider) CultureInfo.InvariantCulture);
      }
      point = single * 2.83464575f;
    }
    else if (value.Contains("in"))
      point = Convert.ToSingle(value.Trim('i', 'n'), (IFormatProvider) CultureInfo.InvariantCulture) * 72f;
    return point;
  }

  internal void ReadMargin(XmlNode node)
  {
    for (int i = 0; i < node.Attributes.Count; ++i)
    {
      if (node.Attributes[i].Name == "bottomInset")
        this.Margins.Bottom = this.ConvertToPoint(node.Attributes[i].Value);
      if (node.Attributes[i].Name == "topInset")
        this.Margins.Top = this.ConvertToPoint(node.Attributes[i].Value);
      if (node.Attributes[i].Name == "leftInset")
        this.Margins.Left = this.ConvertToPoint(node.Attributes[i].Value);
      if (node.Attributes[i].Name == "rightInset")
        this.Margins.Right = this.ConvertToPoint(node.Attributes[i].Value);
    }
  }

  internal void ReadMargin(XmlNode node, PdfMargins margins)
  {
    if (node.Attributes.Count > 0)
    {
      for (int i = 0; i < node.Attributes.Count; ++i)
      {
        if (node.Attributes[i].Name == "bottomInset")
          margins.Bottom = this.ConvertToPoint(node.Attributes[i].Value);
        if (node.Attributes[i].Name == "topInset")
          margins.Top = this.ConvertToPoint(node.Attributes[i].Value);
        if (node.Attributes[i].Name == "leftInset")
          margins.Left = this.ConvertToPoint(node.Attributes[i].Value);
        if (node.Attributes[i].Name == "rightInset")
          margins.Right = this.ConvertToPoint(node.Attributes[i].Value);
      }
    }
    else
    {
      margins.Left = 2f;
      margins.Right = 2f;
    }
  }

  internal string ReadBinding(XmlNode node)
  {
    string str = string.Empty;
    if (node.Attributes["ref"] != null)
      str = node.Attributes["ref"].Value.Replace("$record.", "").Replace("[*]", "");
    return str;
  }

  internal void SetSize(XmlNode node, string attribute, float value)
  {
    if (node.Attributes[attribute] != null)
    {
      node.Attributes[attribute].Value = value.ToString() + "pt";
    }
    else
    {
      XmlAttribute attribute1 = node.OwnerDocument.CreateAttribute(attribute);
      attribute1.Value = value.ToString() + "pt";
      node.Attributes.Append(attribute1);
    }
  }

  internal void ReadField(
    XmlNode node,
    PdfLoadedXfaForm form,
    List<string> fieldNames,
    List<string> subFormNames,
    XmlDocument dataSetDoc)
  {
    if (node["ui"] == null)
      return;
    if (node["ui"]["textEdit"] != null || node["ui"]["passwordEdit"] != null)
    {
      PdfLoadedXfaTextBoxField field = new PdfLoadedXfaTextBoxField();
      field.parent = (PdfLoadedXfaField) form;
      field.Read(node, dataSetDoc);
      string fieldName = this.GetFieldName(form.internalFieldNames, field.Name);
      form.Fields.Add((PdfLoadedXfaField) field, fieldName);
      PdfLoadedXfaTextBoxField loadedXfaTextBoxField = field;
      loadedXfaTextBoxField.nodeName = $"{loadedXfaTextBoxField.nodeName}.{fieldName}";
      fieldNames.Add(fieldName);
      if (form != this)
        form.m_fieldNames.Add(fieldName);
      if (form.internalFieldNames.Contains(fieldName))
        return;
      form.internalFieldNames.Add(fieldName);
    }
    else if (node["ui"]["numericEdit"] != null)
    {
      PdfLoadedXfaNumericField field = new PdfLoadedXfaNumericField();
      field.parent = (PdfLoadedXfaField) form;
      field.Read(node, dataSetDoc);
      string fieldName = this.GetFieldName(form.internalFieldNames, field.Name);
      form.Fields.Add((PdfLoadedXfaField) field, fieldName);
      PdfLoadedXfaNumericField loadedXfaNumericField = field;
      loadedXfaNumericField.nodeName = $"{loadedXfaNumericField.nodeName}.{fieldName}";
      fieldNames.Add(fieldName);
      if (form != this)
        form.m_fieldNames.Add(fieldName);
      if (form.internalFieldNames.Contains(fieldName))
        return;
      form.internalFieldNames.Add(fieldName);
    }
    else if (node["ui"]["checkButton"] != null)
    {
      PdfLoadedXfaCheckBoxField field = new PdfLoadedXfaCheckBoxField();
      field.parent = (PdfLoadedXfaField) form;
      field.Read(node, dataSetDoc);
      string fieldName = this.GetFieldName(form.internalFieldNames, field.Name);
      form.Fields.Add((PdfLoadedXfaField) field, fieldName);
      PdfLoadedXfaCheckBoxField xfaCheckBoxField = field;
      xfaCheckBoxField.nodeName = $"{xfaCheckBoxField.nodeName}.{fieldName}";
      fieldNames.Add(fieldName);
      if (form != this)
        form.m_fieldNames.Add(fieldName);
      if (form.internalFieldNames.Contains(fieldName))
        return;
      form.internalFieldNames.Add(fieldName);
    }
    else if (node["ui"]["choiceList"] != null)
    {
      XmlAttributeCollection attributes = node["ui"]["choiceList"].Attributes;
      if (attributes["open"] != null)
      {
        switch (attributes["open"].Value.ToLower())
        {
          case "always":
          case "multiselect":
          case "usercontrol":
            PdfLoadedXfaListBoxField field1 = new PdfLoadedXfaListBoxField();
            field1.parent = (PdfLoadedXfaField) form;
            field1.ReadField(node, dataSetDoc);
            string fieldName1 = this.GetFieldName(form.internalFieldNames, field1.Name);
            form.Fields.Add((PdfLoadedXfaField) field1, fieldName1);
            PdfLoadedXfaListBoxField loadedXfaListBoxField = field1;
            loadedXfaListBoxField.nodeName = $"{loadedXfaListBoxField.nodeName}.{fieldName1}";
            fieldNames.Add(fieldName1);
            if (form != this)
              form.m_fieldNames.Add(fieldName1);
            if (form.internalFieldNames.Contains(fieldName1))
              break;
            form.internalFieldNames.Add(fieldName1);
            break;
          case "onentry":
            PdfLoadedXfaComboBoxField field2 = new PdfLoadedXfaComboBoxField();
            field2.parent = (PdfLoadedXfaField) form;
            field2.ReadField(node, dataSetDoc);
            string fieldName2 = this.GetFieldName(form.internalFieldNames, field2.Name);
            form.Fields.Add((PdfLoadedXfaField) field2, fieldName2);
            PdfLoadedXfaComboBoxField xfaComboBoxField1 = field2;
            xfaComboBoxField1.nodeName = $"{xfaComboBoxField1.nodeName}.{fieldName2}";
            fieldNames.Add(fieldName2);
            if (form != this)
              form.m_fieldNames.Add(fieldName2);
            if (form.internalFieldNames.Contains(fieldName2))
              break;
            form.internalFieldNames.Add(fieldName2);
            break;
        }
      }
      else
      {
        PdfLoadedXfaComboBoxField field3 = new PdfLoadedXfaComboBoxField();
        field3.parent = (PdfLoadedXfaField) form;
        field3.ReadField(node, dataSetDoc);
        string fieldName3 = this.GetFieldName(form.internalFieldNames, field3.Name);
        form.Fields.Add((PdfLoadedXfaField) field3, fieldName3);
        PdfLoadedXfaComboBoxField xfaComboBoxField2 = field3;
        xfaComboBoxField2.nodeName = $"{xfaComboBoxField2.nodeName}.{fieldName3}";
        fieldNames.Add(fieldName3);
        if (form != this)
          form.m_fieldNames.Add(fieldName3);
        if (form.internalFieldNames.Contains(fieldName3))
          return;
        form.internalFieldNames.Add(fieldName3);
      }
    }
    else if (node["ui"]["dateTimeEdit"] != null)
    {
      PdfLoadedXfaDateTimeField field = new PdfLoadedXfaDateTimeField();
      field.parent = (PdfLoadedXfaField) form;
      field.Read(node, dataSetDoc);
      string fieldName = this.GetFieldName(form.internalFieldNames, field.Name);
      form.Fields.Add((PdfLoadedXfaField) field, fieldName);
      PdfLoadedXfaDateTimeField xfaDateTimeField = field;
      xfaDateTimeField.nodeName = $"{xfaDateTimeField.nodeName}.{fieldName}";
      fieldNames.Add(fieldName);
      if (form != this)
        form.m_fieldNames.Add(fieldName);
      if (form.internalFieldNames.Contains(fieldName))
        return;
      form.internalFieldNames.Add(fieldName);
    }
    else if (node["ui"]["button"] != null)
    {
      PdfLoadedXfaButtonField xfaField = new PdfLoadedXfaButtonField();
      xfaField.parent = (PdfLoadedXfaField) form;
      xfaField.Read(node);
      string fieldName = this.GetFieldName(form.internalFieldNames, xfaField.Name);
      form.Fields.AddStaticFields((PdfLoadedXfaField) xfaField, fieldName);
      if (form.internalFieldNames.Contains(fieldName))
        return;
      form.internalFieldNames.Add(fieldName);
    }
    else
    {
      if (node["ui"]["imageEdit"] == null)
        return;
      PdfLoadedXfaImage xfaField = new PdfLoadedXfaImage();
      xfaField.parent = (PdfLoadedXfaField) form;
      xfaField.ReadField(node, dataSetDoc);
      string fieldName = this.GetFieldName(form.internalFieldNames, xfaField.Name);
      form.Fields.AddStaticFields((PdfLoadedXfaField) xfaField, fieldName);
      if (form.internalFieldNames.Contains(fieldName))
        return;
      form.internalFieldNames.Add(fieldName);
    }
  }

  internal void ReadStaticField(XmlNode node, PdfLoadedXfaForm form, XmlDocument dataSetDoc)
  {
    if (node["ui"] != null && node["ui"]["textEdit"] != null)
    {
      PdfLoadedXfaTextElement xfaField = new PdfLoadedXfaTextElement();
      xfaField.parent = (PdfLoadedXfaField) form;
      xfaField.Read(node);
      string fieldName = this.GetFieldName(form.internalFieldNames, xfaField.Name);
      form.Fields.AddStaticFields((PdfLoadedXfaField) xfaField, fieldName);
      if (form.internalFieldNames.Contains(fieldName))
        return;
      form.internalFieldNames.Add(fieldName);
    }
    else if (node["ui"] != null && node["ui"]["imageEdit"] != null)
    {
      PdfLoadedXfaImage xfaField = new PdfLoadedXfaImage();
      xfaField.parent = (PdfLoadedXfaField) form;
      xfaField.ReadField(node, dataSetDoc);
      string fieldName = this.GetFieldName(form.internalFieldNames, xfaField.Name);
      form.Fields.AddStaticFields((PdfLoadedXfaField) xfaField, fieldName);
      if (form.internalFieldNames.Contains(fieldName))
        return;
      form.internalFieldNames.Add(fieldName);
    }
    else if (node["value"] != null && node["value"]["line"] != null)
    {
      PdfLoadedXfaLine xfaField = new PdfLoadedXfaLine();
      xfaField.parent = (PdfLoadedXfaField) form;
      xfaField.ReadField(node);
      string fieldName = this.GetFieldName(form.internalFieldNames, xfaField.Name);
      form.Fields.AddStaticFields((PdfLoadedXfaField) xfaField, fieldName);
      if (form.internalFieldNames.Contains(fieldName))
        return;
      form.internalFieldNames.Add(fieldName);
    }
    else if (node["value"] != null && node["value"]["rectangle"] != null)
    {
      PdfLoadedXfaRectangleField xfaField = new PdfLoadedXfaRectangleField();
      xfaField.parent = (PdfLoadedXfaField) form;
      xfaField.ReadField(node);
      string fieldName = this.GetFieldName(form.internalFieldNames, xfaField.Name);
      form.Fields.AddStaticFields((PdfLoadedXfaField) xfaField, fieldName);
      if (form.internalFieldNames.Contains(fieldName))
        return;
      form.internalFieldNames.Add(fieldName);
    }
    else
    {
      if (node["value"] == null || node["value"]["arc"] == null)
        return;
      PdfLoadedXfaCircleField xfaField = new PdfLoadedXfaCircleField();
      xfaField.parent = (PdfLoadedXfaField) form;
      xfaField.ReadField(node);
      string fieldName = this.GetFieldName(form.internalFieldNames, xfaField.Name);
      form.Fields.AddStaticFields((PdfLoadedXfaField) xfaField, fieldName);
      if (form.internalFieldNames.Contains(fieldName))
        return;
      form.internalFieldNames.Add(fieldName);
    }
  }

  internal string ReadField(
    XmlNode node,
    PdfLoadedXfaArea form,
    List<string> fieldNames,
    List<string> subFormNames,
    XmlDocument dataSetDoc)
  {
    string fieldName = string.Empty;
    if (node["ui"] != null)
    {
      if (node["ui"]["textEdit"] != null || node["ui"]["passwordEdit"] != null)
      {
        PdfLoadedXfaTextBoxField field = new PdfLoadedXfaTextBoxField();
        field.parent = (PdfLoadedXfaField) form;
        field.Read(node, dataSetDoc);
        fieldName = this.GetFieldName(form.internalFieldNames, field.Name);
        form.Fields.Add((PdfLoadedXfaField) field, fieldName);
        PdfLoadedXfaTextBoxField loadedXfaTextBoxField = field;
        loadedXfaTextBoxField.nodeName = $"{loadedXfaTextBoxField.nodeName}.{fieldName}";
        fieldNames.Add(fieldName);
        if (form != this)
          form.m_fieldNames.Add(fieldName);
        if (!form.internalFieldNames.Contains(fieldName))
          form.internalFieldNames.Add(fieldName);
      }
      else if (node["ui"]["numericEdit"] != null)
      {
        PdfLoadedXfaNumericField field = new PdfLoadedXfaNumericField();
        field.parent = (PdfLoadedXfaField) form;
        field.Read(node, dataSetDoc);
        fieldName = this.GetFieldName(form.internalFieldNames, field.Name);
        form.Fields.Add((PdfLoadedXfaField) field, fieldName);
        PdfLoadedXfaNumericField loadedXfaNumericField = field;
        loadedXfaNumericField.nodeName = $"{loadedXfaNumericField.nodeName}.{fieldName}";
        fieldNames.Add(fieldName);
        if (form != this)
          form.m_fieldNames.Add(fieldName);
        if (!form.internalFieldNames.Contains(fieldName))
          form.internalFieldNames.Add(fieldName);
      }
      else if (node["ui"]["checkButton"] != null)
      {
        PdfLoadedXfaCheckBoxField field = new PdfLoadedXfaCheckBoxField();
        field.parent = (PdfLoadedXfaField) form;
        field.Read(node, dataSetDoc);
        fieldName = this.GetFieldName(form.internalFieldNames, field.Name);
        form.Fields.Add((PdfLoadedXfaField) field, fieldName);
        PdfLoadedXfaCheckBoxField xfaCheckBoxField = field;
        xfaCheckBoxField.nodeName = $"{xfaCheckBoxField.nodeName}.{fieldName}";
        fieldNames.Add(fieldName);
        if (form != this)
          form.m_fieldNames.Add(fieldName);
        if (!form.internalFieldNames.Contains(fieldName))
          form.internalFieldNames.Add(fieldName);
      }
      else if (node["ui"]["choiceList"] != null)
      {
        XmlAttributeCollection attributes = node["ui"]["choiceList"].Attributes;
        if (attributes["open"] != null)
        {
          switch (attributes["open"].Value.ToLower())
          {
            case "always":
            case "multiselect":
            case "usercontrol":
              PdfLoadedXfaListBoxField field1 = new PdfLoadedXfaListBoxField();
              field1.parent = (PdfLoadedXfaField) form;
              field1.ReadField(node, dataSetDoc);
              fieldName = this.GetFieldName(form.internalFieldNames, field1.Name);
              form.Fields.Add((PdfLoadedXfaField) field1, fieldName);
              PdfLoadedXfaListBoxField loadedXfaListBoxField = field1;
              loadedXfaListBoxField.nodeName = $"{loadedXfaListBoxField.nodeName}.{fieldName}";
              fieldNames.Add(fieldName);
              if (form != this)
                form.m_fieldNames.Add(fieldName);
              if (!form.internalFieldNames.Contains(fieldName))
              {
                form.internalFieldNames.Add(fieldName);
                break;
              }
              break;
            case "onentry":
              PdfLoadedXfaComboBoxField field2 = new PdfLoadedXfaComboBoxField();
              field2.parent = (PdfLoadedXfaField) form;
              field2.ReadField(node, dataSetDoc);
              fieldName = this.GetFieldName(form.internalFieldNames, field2.Name);
              form.Fields.Add((PdfLoadedXfaField) field2, fieldName);
              PdfLoadedXfaComboBoxField xfaComboBoxField1 = field2;
              xfaComboBoxField1.nodeName = $"{xfaComboBoxField1.nodeName}.{fieldName}";
              fieldNames.Add(fieldName);
              if (form != this)
                form.m_fieldNames.Add(fieldName);
              if (!form.internalFieldNames.Contains(fieldName))
              {
                form.internalFieldNames.Add(fieldName);
                break;
              }
              break;
          }
        }
        else
        {
          PdfLoadedXfaComboBoxField field3 = new PdfLoadedXfaComboBoxField();
          field3.parent = (PdfLoadedXfaField) form;
          field3.ReadField(node, dataSetDoc);
          fieldName = this.GetFieldName(form.internalFieldNames, field3.Name);
          form.Fields.Add((PdfLoadedXfaField) field3, fieldName);
          PdfLoadedXfaComboBoxField xfaComboBoxField2 = field3;
          xfaComboBoxField2.nodeName = $"{xfaComboBoxField2.nodeName}.{fieldName}";
          fieldNames.Add(fieldName);
          if (form != this)
            form.m_fieldNames.Add(fieldName);
          if (!form.internalFieldNames.Contains(fieldName))
            form.internalFieldNames.Add(fieldName);
        }
      }
      else if (node["ui"]["dateTimeEdit"] != null)
      {
        PdfLoadedXfaDateTimeField field = new PdfLoadedXfaDateTimeField();
        field.parent = (PdfLoadedXfaField) form;
        field.Read(node, dataSetDoc);
        fieldName = this.GetFieldName(form.internalFieldNames, field.Name);
        form.Fields.Add((PdfLoadedXfaField) field, fieldName);
        PdfLoadedXfaDateTimeField xfaDateTimeField = field;
        xfaDateTimeField.nodeName = $"{xfaDateTimeField.nodeName}.{fieldName}";
        fieldNames.Add(fieldName);
        if (form != this)
          form.m_fieldNames.Add(fieldName);
        if (!form.internalFieldNames.Contains(fieldName))
          form.internalFieldNames.Add(fieldName);
      }
    }
    return fieldName;
  }
}
