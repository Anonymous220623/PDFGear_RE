// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.CustomMetadata
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using Syncfusion.Pdf.Xmp;
using System;
using System.Collections;

#nullable disable
namespace Syncfusion.Pdf;

public class CustomMetadata : IEnumerable
{
  private PdfDictionary m_dictionary;
  private XmpMetadata m_xmp;
  private System.Collections.Generic.Dictionary<string, string> m_customMetaDataDictionary = new System.Collections.Generic.Dictionary<string, string>();

  public string this[string key]
  {
    get
    {
      if (key == null)
        throw new ArgumentNullException("key value should not be null");
      if (key.Contains(" "))
        key = key.Replace(" ", "#20");
      return this.m_customMetaDataDictionary[key];
    }
    set
    {
      if (!(key != "Author") || !(key != "Title") || !(key != "Subject") || !(key != "Trapped") || !(key != "Keywords") || !(key != "Producer") || !(key != "CreationDate") || !(key != "ModDate") || !(key != "Creator"))
        throw new PdfException("The Custom property requires a unique name,which must not be on of the standard property names Title,Author,Subject,Keyword,Creator,Producer,CreationDate,ModDate and Trapped");
      this.m_customMetaDataDictionary[key] = value;
      this.Dictionary[key] = (IPdfPrimitive) new PdfString(value);
      if (this.Xmp == null || this.Xmp.CustomSchema == null)
        return;
      if (this.Xmp.CustomSchema.CustomData.ContainsKey(key))
      {
        if (!(value != this.Xmp.CustomSchema[key]))
          return;
        this.m_xmp.CustomSchema[key] = value;
      }
      else
        this.m_xmp.CustomSchema[key] = value;
    }
  }

  public void Remove(string key)
  {
    if (key == null)
      throw new ArgumentNullException("key value should not be null");
    if (key.Contains(" "))
      key = key.Replace(" ", "#20");
    this.m_customMetaDataDictionary.Remove(key);
    this.Dictionary.Remove(key);
    if (this.Xmp == null || this.m_xmp.CustomSchema == null)
      return;
    this.m_xmp.CustomSchema.Remove(key);
  }

  public bool ContainsKey(string key)
  {
    if (key == null)
      throw new ArgumentNullException("key value should not be null");
    if (key.Contains(" "))
      key = key.Replace(" ", "#20");
    return this.m_customMetaDataDictionary.ContainsKey(key);
  }

  public int Count => this.m_customMetaDataDictionary.Count;

  public void Add(string key, string value)
  {
    if (key == null)
      throw new ArgumentNullException("key value should not be null ");
    this.m_customMetaDataDictionary[key] = value != null ? value : throw new ArgumentNullException("Value parmeter should not be null ");
  }

  public IEnumerator GetEnumerator()
  {
    return (IEnumerator) this.m_customMetaDataDictionary.GetEnumerator();
  }

  internal PdfDictionary Dictionary
  {
    get => this.m_dictionary;
    set => this.m_dictionary = value;
  }

  internal XmpMetadata Xmp
  {
    get => this.m_xmp;
    set => this.m_xmp = value;
  }
}
