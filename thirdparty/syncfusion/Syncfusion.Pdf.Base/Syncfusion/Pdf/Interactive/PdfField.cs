// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public abstract class PdfField : IPdfWrapper
{
  private string m_name = string.Empty;
  private PdfPageBase m_page;
  private FieldFlags m_flags;
  private PdfForm m_form;
  private string m_mappingName = string.Empty;
  private bool m_export = true;
  private bool m_readOnly;
  private bool m_required;
  private string m_toolTip = string.Empty;
  private PdfDictionary m_dictionary = new PdfDictionary();
  private bool m_flatten;
  private bool m_disableAutoFormat;
  private int m_rotationAngle;
  internal bool isXfa;
  private PdfTag m_tag;
  private int m_tabIndex;
  private int m_annotationIndex;
  private bool m_complexScript;
  private PdfLayer layer;

  public PdfField(PdfPageBase page, string name)
  {
    if (page != null && this is PdfSignatureField)
    {
      PdfSignatureField pdfSignatureField = this as PdfSignatureField;
      switch (page)
      {
        case PdfLoadedPage pdfLoadedPage:
          if (pdfLoadedPage.Document != null)
          {
            pdfSignatureField.m_fieldAutoNaming = pdfLoadedPage.Document.ObtainForm().FieldAutoNaming;
            break;
          }
          break;
        case PdfPage pdfPage:
          if (pdfPage.Document != null)
          {
            pdfSignatureField.m_fieldAutoNaming = pdfPage.Document.ObtainForm().FieldAutoNaming;
            break;
          }
          break;
      }
    }
    this.Initialize();
    this.m_name = name;
    this.m_page = page;
    this.m_dictionary.SetProperty("T", (IPdfPrimitive) new PdfString(name));
  }

  internal PdfField() => this.Initialize();

  public virtual string Name => this.m_name;

  public virtual PdfForm Form => this.m_form;

  public virtual string MappingName
  {
    get => this.m_mappingName;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (MappingName));
      if (!(this.m_mappingName != value))
        return;
      this.m_mappingName = value;
      this.m_dictionary.SetString("TM", this.m_mappingName);
    }
  }

  public virtual bool Export
  {
    get => this.m_export;
    set
    {
      if (this.m_export == value)
        return;
      this.m_export = value;
      if (this.m_export)
        this.Flags -= FieldFlags.NoExport;
      else
        this.Flags |= FieldFlags.NoExport;
    }
  }

  public virtual bool ReadOnly
  {
    get => this.m_readOnly;
    set => this.m_readOnly = value;
  }

  public virtual bool Required
  {
    get => this.m_required;
    set
    {
      if (this.m_required == value)
        return;
      this.m_required = value;
      if (this.m_required)
        this.Flags |= FieldFlags.Required;
      else
        this.Flags -= FieldFlags.Required;
    }
  }

  public virtual string ToolTip
  {
    get => this.m_toolTip;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (ToolTip));
      if (!(this.m_toolTip != value))
        return;
      this.m_toolTip = value;
      this.m_dictionary.SetString("TU", this.m_toolTip);
    }
  }

  public virtual PdfPageBase Page
  {
    get => this.m_page;
    internal set => this.m_page = value;
  }

  internal bool ComplexScript
  {
    get
    {
      return this.Form != null ? this.m_complexScript | this.Form.ComplexScript : this.m_complexScript;
    }
    set => this.m_complexScript = value;
  }

  public bool Flatten
  {
    get
    {
      bool flatten = this.m_flatten;
      if (this.Form != null)
        flatten |= this.Form.Flatten;
      return flatten;
    }
    set => this.m_flatten = value;
  }

  internal virtual FieldFlags Flags
  {
    get => this.m_flags;
    set
    {
      if (this.m_flags == value)
        return;
      this.m_flags = value;
      this.m_dictionary.SetNumber("Ff", (int) this.m_flags);
    }
  }

  internal PdfDictionary Dictionary
  {
    get => this.m_dictionary;
    set => this.m_dictionary = value;
  }

  internal int RotationAngle
  {
    get => this.m_rotationAngle;
    set => this.m_rotationAngle = value;
  }

  public bool DisableAutoFormat
  {
    get
    {
      bool disableAutoFormat = this.m_disableAutoFormat;
      if (this.Form != null)
        disableAutoFormat |= this.Form.DisableAutoFormat;
      return disableAutoFormat;
    }
    set => this.m_disableAutoFormat = value;
  }

  public PdfTag PdfTag
  {
    get => this.m_tag;
    set => this.m_tag = value;
  }

  public int TabIndex
  {
    get
    {
      PdfLoadedStyledField loadedStyledField = this as PdfLoadedStyledField;
      PdfLoadedPage page = this.Page as PdfLoadedPage;
      if (loadedStyledField != null && page != null)
      {
        PdfDictionary widgetAnnotation = loadedStyledField.GetWidgetAnnotation(loadedStyledField.Dictionary, loadedStyledField.CrossTable);
        if (widgetAnnotation != null)
        {
          PdfReference reference = page.CrossTable.GetReference((IPdfPrimitive) widgetAnnotation);
          if (reference != (PdfReference) null)
            this.m_tabIndex = page.AnnotsReference.IndexOf((IPdfPrimitive) reference);
        }
      }
      return this.m_tabIndex;
    }
    set
    {
      this.m_tabIndex = value;
      if (this.Page == null || this.Page.FormFieldsTabOrder != PdfFormFieldsTabOrder.Manual || !(this is PdfLoadedField))
        return;
      PdfLoadedStyledField loadedStyledField = this as PdfLoadedStyledField;
      PdfLoadedPage page = this.Page as PdfLoadedPage;
      PdfAnnotation pdfAnnotation = (PdfAnnotation) new PdfLoadedWidgetAnnotation(loadedStyledField.Dictionary, loadedStyledField.CrossTable, loadedStyledField.Bounds);
      PdfReference reference = page.CrossTable.GetReference(((IPdfWrapper) pdfAnnotation).Element);
      int index = page.AnnotsReference.IndexOf((IPdfPrimitive) reference);
      if (index < 0)
        index = this.AnnotationIndex;
      PdfArray primitive = this.Page.Annotations.Rearrange(reference, this.m_tabIndex, index);
      page.Dictionary.SetProperty("Annots", (IPdfPrimitive) primitive);
    }
  }

  internal int AnnotationIndex
  {
    get => this.m_annotationIndex;
    set => this.m_annotationIndex = value;
  }

  public PdfLayer Layer
  {
    get
    {
      if (this.layer == null)
        this.layer = this.GetDocumentLayer();
      return this.layer;
    }
    set
    {
      this.layer = value;
      if (this.layer != null)
        this.Dictionary.SetProperty("OC", (IPdfPrimitive) this.layer.ReferenceHolder);
      else
        this.Dictionary.Remove("OC");
    }
  }

  internal void SetForm(PdfForm form)
  {
    this.m_form = form;
    this.DefineDefaultAppearance();
  }

  internal virtual void Save()
  {
    bool flag = this.Form != null && this.Form.ReadOnly;
    if (!this.m_readOnly && !flag)
      return;
    this.Flags |= FieldFlags.ReadOnly;
  }

  internal abstract void Draw();

  internal virtual void ApplyName(string name)
  {
    this.m_name = name;
    this.Dictionary.SetProperty("T", (IPdfPrimitive) new PdfString(name));
  }

  internal virtual PdfField Clone(PdfPageBase page)
  {
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    PdfField pdfField = (PdfField) null;
    if (!(page as PdfPage).Section.ParentDocument.EnableMemoryOptimization)
    {
      pdfField = this.MemberwiseClone() as PdfField;
      pdfField.Dictionary = new PdfDictionary(this.Dictionary);
      pdfField.m_page = page;
      if (pdfField is PdfLoadedRadioButtonListField && pdfField is PdfLoadedRadioButtonListField radioButtonListField && radioButtonListField.Items.Count > 0)
      {
        foreach (PdfLoadedFieldItem pdfLoadedFieldItem in (PdfCollection) radioButtonListField.Items)
          pdfLoadedFieldItem.Page = page;
      }
      if (pdfField is PdfLoadedButtonField)
        pdfField.Page = page;
      pdfField.Dictionary["P"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) page);
    }
    else if (this is PdfLoadedField)
    {
      PdfDictionary pdfDictionary = new PdfDictionary(this.Dictionary);
      pdfDictionary.Remove("Parent");
      if (!(this is PdfLoadedSignatureField))
        pdfDictionary.Remove("P");
      pdfDictionary.Remove("Kids");
      PdfDictionary dictionary = pdfDictionary.Clone((page as PdfPage).Section.ParentDocument.CrossTable) as PdfDictionary;
      if (this is PdfLoadedButtonField)
        pdfField = (this as PdfLoadedButtonField).Clone(dictionary, page as PdfPage);
      else if (this is PdfLoadedCheckBoxField)
        pdfField = (this as PdfLoadedCheckBoxField).Clone(dictionary, page as PdfPage);
      else if (this is PdfLoadedComboBoxField)
        pdfField = (this as PdfLoadedComboBoxField).Clone(dictionary, page as PdfPage);
      else if (this is PdfLoadedListBoxField)
        pdfField = (this as PdfLoadedListBoxField).Clone(dictionary, page as PdfPage);
      else if (this is PdfLoadedRadioButtonListField)
        pdfField = (this as PdfLoadedRadioButtonListField).Clone(dictionary, page as PdfPage);
      else if (this is PdfLoadedSignatureField)
        pdfField = (this as PdfLoadedSignatureField).Clone(dictionary, page as PdfPage);
      else if (this is PdfLoadedTextBoxField)
        pdfField = (this as PdfLoadedTextBoxField).Clone(dictionary, page as PdfPage);
      else if (!dictionary.ContainsKey("FT") && this is PdfLoadedStyledField)
        pdfField = (this as PdfLoadedStyledField).Clone(dictionary, page as PdfPage);
      PdfLoadedField pdfLoadedField = this as PdfLoadedField;
      pdfField.DisableAutoFormat = pdfLoadedField.DisableAutoFormat;
      pdfField.Export = pdfLoadedField.Export;
      pdfField.Flags = pdfLoadedField.Flags;
      pdfField.Flatten = pdfLoadedField.Flatten;
      if (pdfField.MappingName != null)
        pdfField.MappingName = pdfLoadedField.MappingName;
      pdfField.Required = pdfLoadedField.Required;
      pdfField.RotationAngle = pdfLoadedField.RotationAngle;
      if (pdfLoadedField.ToolTip != null)
        pdfField.ToolTip = pdfLoadedField.ToolTip;
      if (pdfLoadedField is PdfLoadedTextBoxField loadedTextBoxField && loadedTextBoxField.m_font != null)
        (pdfField as PdfLoadedTextBoxField).Font = loadedTextBoxField.m_font;
    }
    return pdfField;
  }

  protected virtual void DefineDefaultAppearance()
  {
    if (!(this is PdfRadioButtonListField radioButtonListField))
      return;
    for (int index = 0; index < radioButtonListField.Items.Count; ++index)
    {
      PdfRadioButtonListItem radioButtonListItem = radioButtonListField.Items[index];
      if (radioButtonListItem.Font != null)
        this.Form.Resources.Add(radioButtonListField.Items[index].Font, new PdfName(radioButtonListItem.Widget.DefaultAppearance.FontName));
    }
  }

  protected virtual void Initialize()
  {
    this.m_dictionary.BeginSave += new SavePdfPrimitiveEventHandler(this.Dictionary_BeginSave);
  }

  private void Dictionary_BeginSave(object sender, SavePdfPrimitiveEventArgs ars)
  {
    if (!(this is PdfSignatureField))
    {
      this.Save();
    }
    else
    {
      if (this.Dictionary == null || !this.Dictionary.ContainsKey("Kids") || !this.Dictionary.ContainsKey("TU") || !(this.Dictionary["Kids"] is PdfArray pdfArray))
        return;
      for (int index = 0; index < pdfArray.Count; ++index)
      {
        PdfReferenceHolder element = pdfArray.Elements[index] as PdfReferenceHolder;
        if (element != (PdfReferenceHolder) null && element.Object is PdfDictionary pdfDictionary && !pdfDictionary.ContainsKey("TU") && this.Dictionary["TU"] is PdfString pdfString)
          pdfDictionary.SetString("TU", pdfString.Value);
      }
    }
  }

  private PdfLayer GetDocumentLayer()
  {
    if (this.Dictionary.ContainsKey("OC"))
    {
      IPdfPrimitive expectedObject = this.Dictionary["OC"];
      PdfLoadedPage page = this.Page as PdfLoadedPage;
      if (expectedObject != null && page != null && page.Document != null)
      {
        PdfDocumentLayerCollection layers = page.Document.Layers;
        if (layers != null)
          this.IsMatched(layers, expectedObject, page);
      }
    }
    return this.layer;
  }

  private void IsMatched(
    PdfDocumentLayerCollection layerCollection,
    IPdfPrimitive expectedObject,
    PdfLoadedPage page)
  {
    for (int index = 0; index < layerCollection.Count; ++index)
    {
      IPdfPrimitive referenceHolder = (IPdfPrimitive) layerCollection[index].ReferenceHolder;
      if (referenceHolder != null && referenceHolder.Equals((object) expectedObject))
      {
        if (layerCollection[index].Name != null)
        {
          this.layer = layerCollection[index];
          break;
        }
      }
      else if (layerCollection[index].Layers != null && layerCollection[index].Layers.Count > 0)
        this.IsMatched(layerCollection[index].Layers, expectedObject, page);
    }
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_dictionary;
}
