// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfLoadedXfaFieldCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

public class PdfLoadedXfaFieldCollection : IEnumerable
{
  private Dictionary<string, PdfXfaField> m_fieldCollection;
  internal PdfLoadedXfaField parent;
  private Dictionary<string, PdfXfaField> m_completeFields = new Dictionary<string, PdfXfaField>();

  internal Dictionary<string, PdfXfaField> CompleteFields
  {
    get => this.m_completeFields;
    set => this.m_completeFields = value;
  }

  public PdfLoadedXfaField this[string fieldName]
  {
    internal set
    {
      if (!this.m_fieldCollection.ContainsKey(fieldName))
        return;
      this.m_fieldCollection[fieldName] = (PdfXfaField) value;
    }
    get
    {
      return this.m_fieldCollection.ContainsKey(fieldName) ? this.m_fieldCollection[fieldName] as PdfLoadedXfaField : (PdfLoadedXfaField) null;
    }
  }

  internal PdfXfaField this[int index]
  {
    get
    {
      List<PdfXfaField> pdfXfaFieldList = new List<PdfXfaField>();
      foreach (KeyValuePair<string, PdfXfaField> field in this.m_fieldCollection)
        pdfXfaFieldList.Add(field.Value);
      return pdfXfaFieldList[index];
    }
  }

  internal Dictionary<string, PdfXfaField> FieldCollection => this.m_fieldCollection;

  public int Count => this.m_fieldCollection.Count;

  public PdfLoadedXfaFieldCollection()
  {
    this.m_fieldCollection = new Dictionary<string, PdfXfaField>();
  }

  internal string Add(PdfLoadedXfaField xfaField)
  {
    string key = xfaField.Name + "[0]";
    if (xfaField != null && xfaField.Name != null && (this.m_fieldCollection.ContainsKey(key) || this.CompleteFields.ContainsKey(key)))
    {
      int num = 0;
      for (; this.m_fieldCollection.ContainsKey(key) || this.CompleteFields.ContainsKey(key); key = $"{xfaField.Name}[{num.ToString()}]")
        ++num;
      this.m_fieldCollection.Add(key, (PdfXfaField) xfaField);
    }
    else if (xfaField != null && xfaField.Name != null && !this.m_fieldCollection.ContainsKey(key))
      this.m_fieldCollection.Add(key, (PdfXfaField) xfaField);
    this.CompleteFields.Add(key, (PdfXfaField) xfaField);
    return key;
  }

  internal void Add(PdfLoadedXfaField field, string fieldName)
  {
    if (this.m_fieldCollection.ContainsKey(fieldName) || this.CompleteFields.ContainsKey(fieldName))
      return;
    this.m_fieldCollection.Add(fieldName, (PdfXfaField) field);
    this.CompleteFields.Add(fieldName, (PdfXfaField) field);
  }

  private string GetName(string name)
  {
    string key = name + "[0]";
    if (this.m_fieldCollection.ContainsKey(key))
    {
      int num = 0;
      for (; this.m_fieldCollection.ContainsKey(key); key = $"{name}[{num.ToString()}]")
        ++num;
    }
    return key;
  }

  internal void AddStaticFields(PdfLoadedXfaField xfaField, string fieldName)
  {
    if (this.CompleteFields.ContainsKey(fieldName))
      return;
    this.CompleteFields.Add(fieldName, (PdfXfaField) xfaField);
  }

  internal void AddStaticFields(PdfLoadedXfaField xfaField)
  {
    string key = xfaField.Name + "[0]";
    if (xfaField != null && xfaField.Name != null && this.CompleteFields.ContainsKey(key))
    {
      int num = 0;
      for (; this.CompleteFields.ContainsKey(key); key = $"{xfaField.Name}[{num.ToString()}]")
        ++num;
      this.CompleteFields.Add(key, (PdfXfaField) xfaField);
    }
    else
    {
      if (xfaField == null || xfaField.Name == null || this.CompleteFields.ContainsKey(key))
        return;
      this.CompleteFields.Add(key, (PdfXfaField) xfaField);
    }
  }

  public void Add(PdfXfaField xfaField)
  {
    if (xfaField is PdfXfaRadioButtonField)
      throw new PdfException("Can't add single radio button, need to add the radio button in group (PdfXfaRadioButtonGroup).");
    if (xfaField is PdfXfaForm)
    {
      PdfXfaForm pdfXfaForm = xfaField as PdfXfaForm;
      if (pdfXfaForm.Name != string.Empty)
      {
        string name = this.GetName(pdfXfaForm.Name);
        if (this.parent != null)
          this.parent.m_subFormNames.Add(name);
        this.m_fieldCollection.Add(name, pdfXfaForm.Clone() as PdfXfaField);
      }
      else
      {
        string name = this.GetName("subform");
        if (this.parent != null)
          this.parent.m_subFormNames.Add(name);
        this.m_fieldCollection.Add(name, pdfXfaForm.Clone() as PdfXfaField);
      }
    }
    else if (xfaField.Name != string.Empty)
    {
      string name = this.GetName(xfaField.Name);
      if (this.parent != null)
        this.parent.m_fieldNames.Add(name);
      this.m_fieldCollection.Add(this.GetName(xfaField.Name), xfaField);
    }
    else
    {
      switch (xfaField)
      {
        case PdfXfaTextElement _:
          string name1 = this.GetName("textElement");
          if (this.parent != null)
            this.parent.m_fieldNames.Add(name1);
          this.m_fieldCollection.Add(this.GetName(xfaField.Name), xfaField);
          break;
        case PdfXfaLine _:
          string name2 = this.GetName("line");
          if (this.parent != null)
            this.parent.m_fieldNames.Add(name2);
          this.m_fieldCollection.Add(this.GetName(xfaField.Name), xfaField);
          break;
        default:
          throw new PdfException("Field name is invalid.");
      }
    }
  }

  public void Remove(PdfLoadedXfaField lField)
  {
    lField.currentNode.ParentNode.RemoveChild(lField.currentNode);
    string[] strArray = lField.nodeName.Split('.');
    if (lField.parent != null)
    {
      lField.parent.m_fields.m_fieldCollection.Remove(strArray[strArray.Length - 1]);
    }
    else
    {
      lField.currentNode = (XmlNode) null;
      if (!this.m_fieldCollection.ContainsValue((PdfXfaField) lField))
        return;
      foreach (KeyValuePair<string, PdfXfaField> field in this.m_fieldCollection)
      {
        if (field.Value == lField)
          this.m_fieldCollection.Remove(field.Key);
      }
    }
  }

  public void RemoveAt(int index)
  {
    int num = 0;
    foreach (KeyValuePair<string, PdfXfaField> field in this.m_fieldCollection)
    {
      if (num == index)
      {
        this.m_fieldCollection.Remove(field.Key);
        break;
      }
      ++num;
    }
  }

  public void Clear()
  {
    if (this.parent != null)
      this.parent.currentNode.RemoveAll();
    this.m_fieldCollection.Clear();
  }

  public IEnumerator GetEnumerator() => (IEnumerator) this.m_fieldCollection.Values.GetEnumerator();
}
