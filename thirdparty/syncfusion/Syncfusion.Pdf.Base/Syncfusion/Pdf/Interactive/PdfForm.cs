// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfForm
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using Syncfusion.Pdf.Security;
using Syncfusion.Pdf.Xfa;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfForm : IPdfWrapper
{
  private PdfFormFieldCollection m_fields = new PdfFormFieldCollection();
  private PdfResources m_resources;
  private bool m_readOnly;
  private SignatureFlags m_signatureFlags;
  private PdfDictionary m_dictionary = new PdfDictionary();
  private bool m_needAppearances = true;
  private bool m_flatten;
  private bool m_changeName = true;
  private List<string> m_fieldName = new List<string>();
  private List<string> m_fieldNames = new List<string>();
  private bool m_isXFA;
  private bool m_disableAutoFormat;
  internal System.Collections.Generic.Dictionary<PdfDictionary, PdfPageBase> m_pageMap = new System.Collections.Generic.Dictionary<PdfDictionary, PdfPageBase>();
  private bool m_setAppearanceDictionary;
  private bool m_isDefaultEncoding = true;
  private bool m_isDefaultAppearance;
  internal bool isXfaForm;
  private bool m_complexScript;
  internal bool m_enableXfaFormfill;
  private PdfXfaDocument m_xfa;

  public PdfForm()
  {
    this.m_fields.Form = this;
    this.m_dictionary.SetProperty(nameof (Fields), (IPdfWrapper) this.m_fields);
    if (!(this.m_fields.Form is PdfLoadedForm))
      this.m_dictionary.BeginSave += new SavePdfPrimitiveEventHandler(this.Dictionary_BeginSave);
    this.m_setAppearanceDictionary = true;
  }

  internal PdfXfaDocument Xfa
  {
    get => this.m_xfa;
    set => this.m_xfa = value;
  }

  internal bool IsDefaultAppearance
  {
    get => this.m_isDefaultAppearance;
    set => this.m_isDefaultAppearance = value;
  }

  public bool ComplexScript
  {
    get => this.m_complexScript;
    set => this.m_complexScript = value;
  }

  public bool IsDefaultEncoding
  {
    get => this.m_isDefaultEncoding;
    set => this.m_isDefaultEncoding = value;
  }

  internal List<string> FieldNames => this.m_fieldName;

  internal bool IsXFA
  {
    get => this.m_isXFA;
    set => this.m_isXFA = true;
  }

  public PdfFormFieldCollection Fields => this.m_fields;

  public bool Flatten
  {
    get => this.m_flatten;
    set => this.m_flatten = value;
  }

  public virtual bool ReadOnly
  {
    get => this.m_readOnly;
    set => this.m_readOnly = value;
  }

  public bool FieldAutoNaming
  {
    get => this.m_changeName;
    set => this.m_changeName = value;
  }

  internal virtual bool NeedAppearances
  {
    get => this.m_needAppearances;
    set
    {
      if (this.m_needAppearances == value)
        return;
      this.m_needAppearances = value;
    }
  }

  internal virtual SignatureFlags SignatureFlags
  {
    get => this.m_signatureFlags;
    set
    {
      if (this.m_signatureFlags == value)
        return;
      this.m_signatureFlags = value;
      this.m_dictionary.SetNumber("SigFlags", (int) this.m_signatureFlags);
    }
  }

  internal virtual PdfResources Resources
  {
    get
    {
      if (this.m_resources == null)
      {
        this.m_resources = new PdfResources();
        this.m_dictionary.SetProperty("DR", (IPdfPrimitive) this.m_resources);
      }
      return this.m_resources;
    }
    set => this.m_resources = value != null ? value : throw new ArgumentNullException("resources");
  }

  internal virtual PdfDictionary Dictionary
  {
    get => this.m_dictionary;
    set
    {
      this.m_dictionary = value != null ? value : throw new ArgumentNullException(nameof (Dictionary));
    }
  }

  public bool DisableAutoFormat
  {
    get => this.m_disableAutoFormat;
    set => this.m_disableAutoFormat = value;
  }

  internal bool SetAppearanceDictionary
  {
    get => this.m_setAppearanceDictionary;
    set => this.m_setAppearanceDictionary = value;
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_dictionary;

  internal virtual void Dictionary_BeginSave(object sender, SavePdfPrimitiveEventArgs ars)
  {
    if (this.m_signatureFlags != SignatureFlags.None)
      this.NeedAppearances = false;
    if (!this.isXfaForm)
      this.CheckFlatten();
    if (this.m_fields.Count <= 0 || !this.SetAppearanceDictionary || this.SignatureFlags != SignatureFlags.None)
      return;
    this.m_dictionary.SetBoolean("NeedAppearances", this.m_needAppearances);
  }

  internal virtual void Clear()
  {
    if (this.m_fields != null)
    {
      this.m_fields.Clear();
      this.m_fields = (PdfFormFieldCollection) null;
    }
    if (this.m_dictionary != null)
    {
      this.m_dictionary.Clear();
      this.m_dictionary = (PdfDictionary) null;
    }
    this.m_fieldName.Clear();
    this.m_fieldNames.Clear();
    this.m_pageMap.Clear();
  }

  private void CheckFlatten()
  {
    for (int index = 0; index < this.m_fields.Count; ++index)
    {
      PdfField field = this.m_fields[index];
      if (field.DisableAutoFormat && field.Dictionary.ContainsKey("AA"))
        field.Dictionary.Remove("AA");
      if (field.Flatten)
      {
        int num = 0;
        PdfDictionary dictionary = field.Dictionary;
        if (dictionary.ContainsKey("F"))
          num = (dictionary["F"] as PdfNumber).IntValue;
        if (num != 6)
        {
          this.AddFieldResourcesToPage(field);
          field.Draw();
          this.m_fields.Remove(field);
          this.DeleteFromPages(field);
          this.DeleteAnnotation(field);
          --index;
        }
      }
      else if (field is PdfLoadedField)
      {
        if (field is PdfLoadedTextBoxField && (field.Dictionary.ContainsKey("AP") || (field as PdfLoadedTextBoxField).Items.Count > 0))
        {
          if (this.IsXFA && field.Dictionary.ContainsKey("MK"))
          {
            if (!(field.Dictionary["MK"] is PdfDictionary pdfDictionary))
              pdfDictionary = (field.Dictionary["MK"] as PdfReferenceHolder).Object as PdfDictionary;
            if (pdfDictionary.ContainsKey("BG"))
              pdfDictionary.Remove("BG");
          }
          if (!this.IsXFA)
            (field as PdfLoadedField).BeginSave();
        }
        if (field is PdfLoadedField && this.SignatureFlags == SignatureFlags.None && field is PdfLoadedSignatureField)
          field.Save();
        else if (field is PdfLoadedField && !this.ReadOnly)
          (field as PdfLoadedField).BeginSave();
        else
          field.Save();
      }
    }
  }

  private void AddFieldResourcesToPage(PdfField field)
  {
    PdfResources resources1 = field.Form.Resources;
    if (!resources1.ContainsKey("Font"))
      return;
    PdfDictionary pdfDictionary1 = (PdfDictionary) null;
    if (resources1["Font"] as PdfReferenceHolder != (PdfReferenceHolder) null)
      pdfDictionary1 = (resources1["Font"] as PdfReferenceHolder).Object as PdfDictionary;
    else if (resources1["Font"] is PdfDictionary)
      pdfDictionary1 = resources1["Font"] as PdfDictionary;
    foreach (PdfName key in pdfDictionary1.Keys)
    {
      PdfResources resources2 = field.Page.GetResources();
      PdfDictionary pdfDictionary2 = (PdfDictionary) null;
      if (resources2["Font"] is PdfDictionary)
        pdfDictionary2 = resources2["Font"] as PdfDictionary;
      else if (resources2["Font"] as PdfReferenceHolder != (PdfReferenceHolder) null)
        pdfDictionary2 = (resources2["Font"] as PdfReferenceHolder).Object as PdfDictionary;
      if (pdfDictionary2 == null || !pdfDictionary2.ContainsKey(key))
      {
        PdfReferenceHolder pdfReferenceHolder = pdfDictionary1[key] as PdfReferenceHolder;
        if (pdfDictionary2 == null)
          resources2["Font"] = (IPdfPrimitive) new PdfDictionary()
          {
            Items = {
              {
                key,
                (IPdfPrimitive) pdfReferenceHolder
              }
            }
          };
        else
          pdfDictionary2.Items.Add(key, (IPdfPrimitive) pdfReferenceHolder);
      }
    }
  }

  internal void DeleteFromPages(PdfField field)
  {
    PdfDictionary dictionary = field.Dictionary;
    if (dictionary.ContainsKey("Kids"))
    {
      PdfArray pdfArray = dictionary["Kids"] as PdfArray;
      int index1 = 0;
      for (int count = pdfArray.Count; index1 < count; ++index1)
      {
        PdfReferenceHolder element = pdfArray[index1] as PdfReferenceHolder;
        PdfDictionary pdfDictionary = ((element.Object as PdfDictionary)["P"] as PdfReferenceHolder).Object as PdfDictionary;
        if (pdfDictionary.ContainsKey("Annots"))
        {
          if ((object) (pdfDictionary["Annots"] as PdfReferenceHolder) != null)
          {
            PdfArray primitive = (pdfDictionary["Annots"] as PdfReferenceHolder).Object as PdfArray;
            primitive.Remove((IPdfPrimitive) element);
            primitive.MarkChanged();
            pdfDictionary.SetProperty("Annots", (IPdfPrimitive) primitive);
          }
          else if (pdfDictionary["Annots"] is PdfArray)
          {
            if (field.Page != null && field.Page is PdfPage)
            {
              PdfPage page = field.Page as PdfPage;
              PdfAnnotationCollection annotations = page.Annotations;
              if (annotations != null)
              {
                int index2 = annotations.Annotations.IndexOf((IPdfPrimitive) element);
                if (index2 >= 0 && index2 < annotations.Count)
                  page.Annotations.RemoveAt(index2);
              }
            }
            PdfArray primitive = pdfDictionary["Annots"] as PdfArray;
            primitive.Remove((IPdfPrimitive) element);
            primitive.MarkChanged();
            pdfDictionary.SetProperty("Annots", (IPdfPrimitive) primitive);
          }
        }
      }
    }
    else
    {
      PdfDictionary pdfDictionary = (!dictionary.ContainsKey("P") ? new PdfReferenceHolder((IPdfPrimitive) field.Page.Dictionary) : dictionary["P"] as PdfReferenceHolder).Object as PdfDictionary;
      if (!pdfDictionary.ContainsKey("Annots"))
        return;
      PdfArray primitive = pdfDictionary["Annots"] as PdfArray;
      primitive.Remove((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) dictionary));
      primitive.MarkChanged();
      pdfDictionary.SetProperty("Annots", (IPdfPrimitive) primitive);
    }
  }

  internal void DeleteAnnotation(PdfField field)
  {
    PdfDictionary dictionary = field.Dictionary;
    if (!dictionary.ContainsKey("Kids"))
      return;
    PdfArray primitive = dictionary["Kids"] as PdfArray;
    primitive.Clear();
    dictionary.SetProperty("Kids", (IPdfPrimitive) primitive);
  }

  internal virtual string GetCorrectName(string name)
  {
    string correctName = name;
    this.m_fieldNames.Add(correctName);
    if (this.m_fieldName.Contains(name))
    {
      int num = this.m_fieldName.IndexOf(name);
      int index = this.m_fieldName.LastIndexOf(name);
      if (num != index)
      {
        string[] strArray = Guid.NewGuid().ToString().Split('-');
        correctName = $"{name}_{strArray[4]}";
        this.m_fieldName.RemoveAt(index);
        this.m_fieldName.Add(correctName);
      }
    }
    return correctName;
  }

  public void SetDefaultAppearance(bool applyDefault)
  {
    this.NeedAppearances = applyDefault;
    this.SetAppearanceDictionary = true;
    this.IsDefaultAppearance = true;
  }
}
