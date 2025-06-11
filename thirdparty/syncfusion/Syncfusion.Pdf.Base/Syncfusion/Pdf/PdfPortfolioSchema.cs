// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfPortfolioSchema
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfPortfolioSchema : IPdfWrapper
{
  private PdfDictionary m_dictionary = new PdfDictionary();
  private PdfPortfolioSchemaField m_schemaField;
  private string[] fieldkeys;
  private Dictionary<string, PdfPortfolioSchemaField> m_fieldCollections = new Dictionary<string, PdfPortfolioSchemaField>();

  public string[] FieldKeys
  {
    get
    {
      string[] array = new string[this.m_fieldCollections.Count];
      this.m_fieldCollections.Keys.CopyTo(array, 0);
      return array;
    }
  }

  public PdfPortfolioSchema() => this.Initialize();

  internal PdfPortfolioSchema(PdfDictionary schemaDictionary)
  {
    this.m_dictionary = schemaDictionary;
    if (this.m_dictionary == null)
      return;
    foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in this.m_dictionary.Items)
    {
      if (!(keyValuePair.Key.Value == "Type"))
      {
        PdfDictionary schemaField = (PdfDictionary) null;
        if (this.m_dictionary[keyValuePair.Key] is PdfDictionary)
          schemaField = this.m_dictionary[keyValuePair.Key] as PdfDictionary;
        else if ((object) (this.m_dictionary[keyValuePair.Key] as PdfReferenceHolder) != null)
          schemaField = (this.m_dictionary[keyValuePair.Key] as PdfReferenceHolder).Object as PdfDictionary;
        if (schemaField != null)
        {
          this.m_schemaField = new PdfPortfolioSchemaField(schemaField);
          if (this.m_schemaField != null)
            this.m_fieldCollections.Add(this.m_schemaField.Name, this.m_schemaField);
        }
      }
    }
  }

  public void AddSchemaField(PdfPortfolioSchemaField field)
  {
    if (this.m_fieldCollections.ContainsKey(field.Name) || this.m_dictionary.ContainsKey(field.Name))
      return;
    this.m_fieldCollections.Add(field.Name, field);
    this.m_dictionary.SetProperty(field.Name, (IPdfWrapper) field);
  }

  public void RemoveField(string key)
  {
    if (!this.m_fieldCollections.ContainsKey(key) || !this.m_dictionary.ContainsKey(key))
      return;
    this.m_fieldCollections.Remove(key);
    this.m_dictionary.Remove(key);
  }

  public Dictionary<string, PdfPortfolioSchemaField> GetSchemaField() => this.m_fieldCollections;

  private void Initialize()
  {
    this.m_dictionary.SetProperty("Type", (IPdfPrimitive) new PdfName("CollectionSchema"));
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_dictionary;
}
