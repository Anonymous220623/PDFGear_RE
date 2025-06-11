// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfResources
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.ColorSpace;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf;

internal class PdfResources : PdfDictionary
{
  private Dictionary<IPdfPrimitive, PdfName> m_names;
  private PdfDictionary m_properties = new PdfDictionary();
  private string m_originalFontName;
  private PdfDocument m_document;
  private int m_imageCounter;
  private string m_imageName = "Im";
  private int m_fontCounter;
  private string m_fontName = "F";
  private int m_colorSpaceCounter;
  private string m_colorSpaceName = "Cs";
  private int m_brushCounter;
  private string m_brushName = "Br";
  private int m_templateCounter;
  private string m_templateName = "Tp";
  private int m_transparencyCounter;
  private string m_transparencyName = "Tr";
  private int m_dColorSpaceCounter;
  private string m_dColorSpaceName = "Dc";

  private Dictionary<IPdfPrimitive, PdfName> Names => this.ObtainNames();

  internal string OriginalFontName
  {
    get => this.m_originalFontName;
    set => this.m_originalFontName = value;
  }

  internal PdfDocument Document
  {
    get => this.m_document;
    set => this.m_document = value;
  }

  internal PdfResources()
  {
  }

  internal PdfResources(PdfDictionary baseDictionary)
    : base(baseDictionary)
  {
  }

  internal PdfName GetName(IPdfWrapper obj)
  {
    PdfTemplate pdfTemplate = obj as PdfTemplate;
    IPdfPrimitive key = obj != null ? obj.Element : throw new ArgumentNullException(nameof (obj));
    PdfName name1 = (PdfName) null;
    if (obj is PdfImage && this.Document != null && (obj as PdfImage).InternalImage is Bitmap)
    {
      if (this.Names.ContainsKey(key))
        name1 = this.Names[key];
      else
        name1 = new PdfName(!PdfDocument.m_resourceNaming ? this.GenerateName() : this.GenerateName(obj));
    }
    else
    {
      if (this.Names.ContainsKey(key))
        name1 = this.Names[key];
      if (this.m_originalFontName != null)
      {
        PdfName pdfName = new PdfName(this.m_originalFontName);
        foreach (KeyValuePair<IPdfPrimitive, PdfName> name2 in this.Names)
        {
          if (name2.Value == pdfName)
          {
            name1 = name2.Value;
            this.m_originalFontName = (string) null;
            break;
          }
        }
        if (name1 == (PdfName) null)
        {
          name1 = new PdfName(!PdfDocument.m_resourceNaming ? this.GenerateName() : this.GenerateName(obj));
          this.Names[key] = name1;
          this.Add(obj, name1);
        }
      }
      else if (name1 == (PdfName) null)
      {
        name1 = new PdfName(!PdfDocument.m_resourceNaming ? (pdfTemplate == null ? this.GenerateName() : (pdfTemplate.CustomPdfTemplateName == null ? this.GenerateName() : pdfTemplate.CustomPdfTemplateName)) : this.GenerateName(obj));
        this.Names[key] = name1;
        this.Add(obj, name1);
      }
    }
    return name1;
  }

  internal Dictionary<IPdfPrimitive, PdfName> ObtainNames()
  {
    if (this.m_names == null)
      this.m_names = new Dictionary<IPdfPrimitive, PdfName>();
    IPdfPrimitive pdfPrimitive = this["Font"];
    if (pdfPrimitive != null)
    {
      PdfReferenceHolder pdfReferenceHolder = pdfPrimitive as PdfReferenceHolder;
      PdfDictionary pdfDictionary = pdfPrimitive as PdfDictionary;
      if (pdfReferenceHolder != (PdfReferenceHolder) null)
        pdfDictionary = PdfCrossTable.Dereference(pdfPrimitive) as PdfDictionary;
      if (pdfDictionary != null)
      {
        foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in pdfDictionary.Items)
        {
          IPdfPrimitive key1 = PdfCrossTable.Dereference(keyValuePair.Value);
          if (!this.m_names.ContainsValue(keyValuePair.Key))
          {
            PdfName key2 = keyValuePair.Key;
            this.m_names[key1] = key2;
          }
        }
      }
    }
    return this.m_names;
  }

  internal void RequireProcSet(string procSetName)
  {
    if (procSetName == null)
      throw new ArgumentNullException(nameof (procSetName));
    if (!(this["ProcSet"] is PdfArray pdfArray))
    {
      pdfArray = new PdfArray();
      this["ProcSet"] = (IPdfPrimitive) pdfArray;
    }
    PdfName element = new PdfName(procSetName);
    if (pdfArray.Contains((IPdfPrimitive) element))
      return;
    pdfArray.Add((IPdfPrimitive) element);
  }

  private string GenerateName() => Guid.NewGuid().ToString();

  private string GenerateName(IPdfWrapper obj)
  {
    switch (obj)
    {
      case PdfFont _:
        return this.SetResourceName(this.m_fontName, this.FetchResourceDictionary("Font"), this.m_fontCounter, out this.m_fontCounter);
      case PdfTemplate _:
        return this.SetResourceName(this.m_templateName, this.FetchResourceDictionary("XObject"), this.m_templateCounter, out this.m_templateCounter);
      case PdfImage _:
        return this.SetResourceName(this.m_imageName, this.FetchResourceDictionary("XObject"), this.m_imageCounter, out this.m_imageCounter);
      case PdfBrush _:
        return this.SetResourceName(this.m_brushName, this.FetchResourceDictionary("Pattern"), this.m_brushCounter, out this.m_brushCounter);
      case PdfTransparency _:
        return this.SetResourceName(this.m_transparencyName, this.FetchResourceDictionary("ExtGState"), this.m_transparencyCounter, out this.m_transparencyCounter);
      case PdfColorSpaces _:
        return this.SetResourceName(this.m_colorSpaceName, this.FetchResourceDictionary("ColorSpace"), this.m_colorSpaceCounter, out this.m_colorSpaceCounter);
      case PdfDictionary _:
        return this.SetResourceName(this.m_dColorSpaceName, this.FetchResourceDictionary("ColorSpace"), this.m_dColorSpaceCounter, out this.m_dColorSpaceCounter);
      default:
        return Guid.NewGuid().ToString();
    }
  }

  private string SetResourceName(
    string name,
    PdfDictionary resourceDictionary,
    int counter,
    out int updatedCounter)
  {
    updatedCounter = counter;
    string key = name + (object) updatedCounter;
    if (resourceDictionary != null)
    {
      for (; resourceDictionary.ContainsKey(key); key = name + (object) updatedCounter)
        ++updatedCounter;
    }
    ++updatedCounter;
    return key;
  }

  private PdfDictionary FetchResourceDictionary(string DictionaryProperty)
  {
    return (object) (this[DictionaryProperty] as PdfReferenceHolder) == null ? this[DictionaryProperty] as PdfDictionary : (this[DictionaryProperty] as PdfReferenceHolder).Object as PdfDictionary;
  }

  private void Add(IPdfWrapper obj, PdfName name)
  {
    switch (obj)
    {
      case PdfFont font:
        this.Add(font, name);
        break;
      case PdfTemplate template:
        this.Add(template, name);
        break;
      case PdfImage image:
        this.Add(image, name);
        break;
      case PdfBrush brush:
        this.Add(brush, name);
        break;
      case PdfTransparency transparancy:
        this.Add(transparancy, name);
        break;
      case PdfColorSpaces color:
        this.Add(color, name);
        break;
      case PdfDictionary _:
        this.Add(color, name);
        break;
    }
  }

  internal void Add(PdfFont font, PdfName name)
  {
    IPdfPrimitive pdfPrimitive = this["Font"];
    PdfDictionary pdfDictionary;
    if (pdfPrimitive != null)
    {
      PdfReferenceHolder pdfReferenceHolder = pdfPrimitive as PdfReferenceHolder;
      pdfDictionary = pdfPrimitive as PdfDictionary;
      if (pdfReferenceHolder != (PdfReferenceHolder) null)
        pdfDictionary = PdfCrossTable.Dereference(pdfPrimitive) as PdfDictionary;
    }
    else
    {
      pdfDictionary = new PdfDictionary();
      this["Font"] = (IPdfPrimitive) pdfDictionary;
    }
    pdfDictionary[name] = (IPdfPrimitive) new PdfReferenceHolder(((IPdfWrapper) font).Element);
  }

  internal void AddProperties(string layerid, PdfReferenceHolder reff)
  {
    this.m_properties[layerid] = (IPdfPrimitive) reff;
    this["Properties"] = (IPdfPrimitive) this.m_properties;
  }

  private void Add(PdfTemplate template, PdfName name)
  {
    PdfDictionary pdfDictionary = (object) (this["XObject"] as PdfReferenceHolder) == null ? this["XObject"] as PdfDictionary : (this["XObject"] as PdfReferenceHolder).Object as PdfDictionary;
    if (pdfDictionary == null)
    {
      pdfDictionary = new PdfDictionary();
      this["XObject"] = (IPdfPrimitive) pdfDictionary;
    }
    pdfDictionary[name] = (IPdfPrimitive) new PdfReferenceHolder(((IPdfWrapper) template).Element);
  }

  private void Add(PdfImage image, PdfName name)
  {
    PdfDictionary pdfDictionary1 = this["XObject"] as PdfDictionary;
    PdfReferenceHolder pdfReferenceHolder = this["XObject"] as PdfReferenceHolder;
    PdfDictionary pdfDictionary2 = new PdfDictionary();
    if (pdfDictionary1 == null && pdfReferenceHolder != (PdfReferenceHolder) null)
      pdfDictionary2 = pdfReferenceHolder.Object as PdfDictionary;
    if (pdfDictionary1 == null)
    {
      pdfDictionary1 = new PdfDictionary();
      this["XObject"] = (IPdfPrimitive) pdfDictionary1;
      foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in pdfDictionary2.Items)
        pdfDictionary1[keyValuePair.Key] = (IPdfPrimitive) (keyValuePair.Value as PdfReferenceHolder);
    }
    pdfDictionary1[name] = (IPdfPrimitive) new PdfReferenceHolder(((IPdfWrapper) image).Element);
  }

  private void Add(PdfBrush brush, PdfName name)
  {
    IPdfPrimitive element = (brush as IPdfWrapper).Element;
    if (element == null)
      return;
    if (!(this["Pattern"] is PdfDictionary pdfDictionary))
    {
      pdfDictionary = new PdfDictionary();
      this["Pattern"] = (IPdfPrimitive) pdfDictionary;
    }
    pdfDictionary[name] = (IPdfPrimitive) new PdfReferenceHolder(element);
  }

  private void Add(PdfTransparency transparancy, PdfName name)
  {
    IPdfPrimitive element = ((IPdfWrapper) transparancy).Element;
    if (element == null)
      return;
    PdfDictionary pdfDictionary = (PdfDictionary) null;
    if (this["ExtGState"] is PdfDictionary)
      pdfDictionary = this["ExtGState"] as PdfDictionary;
    else if ((object) (this["ExtGState"] as PdfReferenceHolder) != null)
      pdfDictionary = (this["ExtGState"] as PdfReferenceHolder).Object as PdfDictionary;
    if (pdfDictionary == null)
    {
      pdfDictionary = new PdfDictionary();
      this["ExtGState"] = (IPdfPrimitive) pdfDictionary;
    }
    pdfDictionary[name] = (IPdfPrimitive) new PdfReferenceHolder(element);
  }

  internal void Add(PdfColorSpaces color, PdfName name)
  {
    IPdfPrimitive pdfPrimitive = this["ColorSpace"];
    PdfDictionary pdfDictionary;
    if (pdfPrimitive != null)
    {
      PdfReferenceHolder pdfReferenceHolder = pdfPrimitive as PdfReferenceHolder;
      pdfDictionary = pdfPrimitive as PdfDictionary;
      if (pdfReferenceHolder != (PdfReferenceHolder) null)
        pdfDictionary = PdfCrossTable.Dereference(pdfPrimitive) as PdfDictionary;
    }
    else
    {
      pdfDictionary = new PdfDictionary();
      this["ColorSpace"] = (IPdfPrimitive) pdfDictionary;
    }
    pdfDictionary[name] = (IPdfPrimitive) new PdfReferenceHolder(((IPdfWrapper) color).Element);
  }

  internal void Add(PdfDictionary color, PdfName name)
  {
    IPdfPrimitive pdfPrimitive = this["ColorSpace"];
    PdfDictionary pdfDictionary;
    if (pdfPrimitive != null)
    {
      PdfReferenceHolder pdfReferenceHolder = pdfPrimitive as PdfReferenceHolder;
      pdfDictionary = pdfPrimitive as PdfDictionary;
      if (pdfReferenceHolder != (PdfReferenceHolder) null)
        pdfDictionary = PdfCrossTable.Dereference(pdfPrimitive) as PdfDictionary;
    }
    else
    {
      pdfDictionary = new PdfDictionary();
      this["ColorSpace"] = (IPdfPrimitive) pdfDictionary;
    }
    pdfDictionary[name] = (IPdfPrimitive) new PdfReferenceHolder(((IPdfWrapper) color).Element);
  }

  internal void RemoveFont(string name)
  {
    IPdfPrimitive key = (IPdfPrimitive) null;
    if (this.m_names != null)
    {
      foreach (KeyValuePair<IPdfPrimitive, PdfName> name1 in this.m_names)
      {
        if (name1.Value == new PdfName(name))
        {
          key = name1.Key;
          break;
        }
      }
    }
    if (key == null)
      return;
    this.m_names.Remove(key);
  }
}
