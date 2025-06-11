// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfFormFieldCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfFormFieldCollection : PdfFieldCollection
{
  private PdfForm m_form;

  internal PdfForm Form
  {
    get => this.m_form;
    set => this.m_form = value != null ? value : throw new ArgumentNullException("form");
  }

  protected override int DoAdd(PdfField field)
  {
    field.SetForm(this.Form);
    string empty = string.Empty;
    string name = !(field is PdfLoadedField) ? field.Name : (field as PdfLoadedField).ActualFieldName;
    if (string.IsNullOrEmpty(name))
      name = Guid.NewGuid().ToString();
    this.m_form.FieldNames.Add(name);
    if (this.m_form.FieldAutoNaming)
    {
      string correctName = this.m_form.GetCorrectName(name);
      field.ApplyName(correctName);
    }
    else
    {
      if (this.Count <= 0)
        return base.DoAdd(field);
      foreach (PdfField pdfField in (PdfCollection) this)
      {
        if (pdfField.Name == field.Name && field is PdfTextBoxField && pdfField is PdfTextBoxField)
        {
          (field as PdfTextBoxField).Widget.Dictionary?.Remove("Parent");
          (field as PdfTextBoxField).Widget.Parent = pdfField;
          if (field is PdfStyledField)
          {
            PdfStyledField pdfStyledField = field as PdfStyledField;
            if (field.Page is PdfPage)
              (field.Page as PdfPage).Annotations.Add((PdfAnnotation) pdfStyledField.Widget);
            else if (field.Page is PdfLoadedPage)
              (field.Page as PdfLoadedPage).Annotations.Add((PdfAnnotation) pdfStyledField.Widget);
          }
          if (!(pdfField as PdfTextBoxField).m_array.Contains((IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) (pdfField as PdfTextBoxField).Widget)))
            (pdfField as PdfTextBoxField).m_array.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) (pdfField as PdfTextBoxField).Widget));
          (pdfField as PdfTextBoxField).m_array.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) (field as PdfTextBoxField).Widget));
          pdfField.Dictionary.SetProperty("Kids", (IPdfPrimitive) (pdfField as PdfTextBoxField).m_array);
          (pdfField as PdfTextBoxField).fieldItems.Add(field);
          return this.Count - 1;
        }
      }
    }
    return base.DoAdd(field);
  }

  protected override void DoInsert(int index, PdfField field)
  {
    if (!this.IsValidName(field.Name))
      throw new PdfDocumentException(string.Format(this.c_exisingFieldException, (object) field.Name));
    field.SetForm(this.Form);
    base.DoInsert(index, field);
  }

  protected override void DoRemove(PdfField field)
  {
    field.SetForm((PdfForm) null);
    base.DoRemove(field);
  }

  protected override void DoRemoveAt(int index)
  {
    ((PdfField) this.Items[index]).SetForm((PdfForm) null);
    base.DoRemoveAt(index);
  }

  protected override void DoClear()
  {
    foreach (PdfField field in (PdfCollection) this)
    {
      this.m_form.DeleteFromPages(field);
      this.m_form.DeleteAnnotation(field);
      field.Page = (PdfPageBase) null;
      field.Dictionary.Clear();
      field.SetForm((PdfForm) null);
    }
    base.DoClear();
  }

  private bool IsValidName(string name) => this.m_form.FieldNames.Contains(name);
}
