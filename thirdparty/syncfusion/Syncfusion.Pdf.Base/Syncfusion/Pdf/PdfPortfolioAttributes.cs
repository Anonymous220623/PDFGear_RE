// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfPortfolioAttributes
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfPortfolioAttributes : IPdfWrapper
{
  private PdfDictionary m_dictionary;
  private string[] m_attributeKeys;
  private Dictionary<string, string> m_attributes = new Dictionary<string, string>();
  private PdfPortfolioSchemaCollection m_schemaAttributes = new PdfPortfolioSchemaCollection();

  public PdfPortfolioAttributes() => this.Initialize();

  internal PdfPortfolioAttributes(PdfDictionary dictionary)
  {
    if (this.m_dictionary != null)
      return;
    this.m_dictionary = dictionary;
    foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in dictionary.Items)
    {
      if (!(keyValuePair.Key.Value == "Type"))
      {
        if (keyValuePair.Value is PdfString)
          this.m_attributes.Add(keyValuePair.Key.Value, (keyValuePair.Value as PdfString).Value);
        else if (keyValuePair.Value is PdfNumber)
          this.m_attributes.Add(keyValuePair.Key.Value, (keyValuePair.Value as PdfNumber).FloatValue.ToString());
      }
    }
  }

  public string[] AttributesKey
  {
    get
    {
      string[] array = new string[this.m_attributes.Count];
      this.m_attributes.Keys.CopyTo(array, 0);
      return array;
    }
  }

  internal PdfPortfolioSchemaCollection SchemaAttributes
  {
    get
    {
      foreach (KeyValuePair<string, string> attribute in this.m_attributes)
        this.m_schemaAttributes.Add(attribute.Key, attribute.Value);
      return this.m_schemaAttributes;
    }
  }

  private void Initialize()
  {
    this.m_dictionary = new PdfDictionary();
    this.m_dictionary.SetProperty("Type", (IPdfPrimitive) new PdfName("CollectionItem"));
  }

  public void AddAttributes(string key, string value)
  {
    if (this.m_attributes.ContainsKey(key) || this.m_dictionary.ContainsKey(key))
      return;
    this.m_attributes.Add(key, value);
    this.m_dictionary.SetProperty(key, (IPdfPrimitive) new PdfString(value));
  }

  public void RemoveAttributes(string key)
  {
    if (!this.m_attributes.ContainsKey(key) || !this.m_dictionary.ContainsKey(key))
      return;
    this.m_attributes.Remove(key);
    this.m_dictionary.Remove(key);
  }

  public PdfPortfolioSchemaCollection GetAttributes() => this.SchemaAttributes;

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_dictionary;
}
