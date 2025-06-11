// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfFieldCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using Syncfusion.Pdf.Xfa;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfFieldCollection : PdfCollection, IPdfWrapper
{
  internal string c_exisingFieldException = "The field with '{0}' name already exists";
  private PdfArray m_array = new PdfArray();
  private Dictionary<string, int> m_fieldNames;

  public virtual PdfField this[int index] => (PdfField) this.List[index];

  public PdfField this[string name]
  {
    get
    {
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      int index = !(name == string.Empty) ? this.GetFieldIndex(name) : throw new ArgumentException("Field name can't be empty");
      return index != -1 ? this[index] : throw new ArgumentException("Incorrect field name");
    }
  }

  internal PdfArray Items => this.m_array;

  public int Add(PdfField field)
  {
    return field != null ? this.DoAdd(field) : throw new ArgumentNullException(nameof (field));
  }

  internal void Add(PdfXfaForm collection, string subformName)
  {
    if (collection == null)
      return;
    this.DoAdd(collection, subformName);
  }

  public void Insert(int index, PdfField field)
  {
    if (field == null)
      throw new ArgumentNullException(nameof (field));
    this.DoInsert(index, field);
  }

  public bool Contains(PdfField field) => this.List.Contains((object) field);

  public int IndexOf(PdfField field)
  {
    return field != null ? this.List.IndexOf((object) field) : throw new ArgumentNullException(nameof (field));
  }

  public void Remove(PdfField field)
  {
    if (field == null)
      throw new ArgumentNullException(nameof (field));
    this.DoRemove(field);
  }

  public void RemoveAt(int index) => this.DoRemoveAt(index);

  public void Clear() => this.DoClear();

  internal int Add(PdfField field, PdfPageBase newPage)
  {
    PdfField field1 = (PdfField) null;
    if (field is PdfLoadedField)
      field1 = this.InsertLoadedField(field as PdfLoadedField, newPage);
    int num = this.DoAdd(field1);
    if (field is PdfLoadedField && field1.ReadOnly != (field as PdfLoadedField).ReadOnly)
      field1.ReadOnly = (field as PdfLoadedField).ReadOnly;
    return num;
  }

  protected virtual void DoAdd(PdfXfaForm form, string subformName)
  {
    PdfReferenceHolder element = new PdfReferenceHolder((IPdfPrimitive) new PdfDictionary()
    {
      Items = {
        {
          new PdfName("Kids"),
          (IPdfPrimitive) form.m_acroFields.Items
        },
        {
          new PdfName("T"),
          (IPdfPrimitive) new PdfString(subformName)
        }
      }
    });
    this.m_array.Add((IPdfPrimitive) element);
    foreach (PdfReferenceHolder pdfReferenceHolder in form.m_acroFields.Items)
    {
      if (pdfReferenceHolder != (PdfReferenceHolder) null)
      {
        PdfDictionary pdfDictionary = pdfReferenceHolder.Object as PdfDictionary;
        if (pdfDictionary.ContainsKey(new PdfName("Kids")) && pdfDictionary.ContainsKey(new PdfName("T")))
          pdfDictionary.Items.Add(new PdfName("Parent"), (IPdfPrimitive) element);
      }
    }
    this.List.Add((object) form.m_acroFields.Items);
  }

  protected virtual int DoAdd(PdfField field)
  {
    switch (field)
    {
      case PdfStyledField _:
        PdfStyledField pdfStyledField = field as PdfStyledField;
        if (field.Page is PdfPage)
        {
          (field.Page as PdfPage).Annotations.Add((PdfAnnotation) pdfStyledField.Widget);
          break;
        }
        if (field.Page is PdfLoadedPage)
        {
          (field.Page as PdfLoadedPage).Annotations.Add((PdfAnnotation) pdfStyledField.Widget);
          break;
        }
        break;
      case PdfSignatureField _:
        PdfSignatureField pdfSignatureField = field as PdfSignatureField;
        if (field.Page is PdfPage)
        {
          PdfPage page = field.Page as PdfPage;
          if (!page.Annotations.Contains((PdfAnnotation) pdfSignatureField.Widget))
          {
            page.Annotations.Add((PdfAnnotation) pdfSignatureField.Widget);
            break;
          }
          break;
        }
        if (field.Page is PdfLoadedPage)
        {
          PdfLoadedPage page = field.Page as PdfLoadedPage;
          if (!page.Annotations.Contains((PdfAnnotation) pdfSignatureField.Widget))
          {
            page.Annotations.Add((PdfAnnotation) pdfSignatureField.Widget);
            break;
          }
          break;
        }
        break;
    }
    this.m_array.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) field));
    this.List.Add((object) field);
    if (field.Dictionary != null && field.Dictionary.ContainsKey("Kids") && field.Dictionary["Kids"] is PdfArray pdfArray)
    {
      for (int index = 0; index < pdfArray.Count; ++index)
      {
        PdfReferenceHolder element = pdfArray.Elements[index] as PdfReferenceHolder;
        if (element != (PdfReferenceHolder) null)
        {
          PdfDictionary annotDictionary = element.Object as PdfDictionary;
          PdfStructTreeRoot structTreeRoot = PdfCatalog.StructTreeRoot;
          if (annotDictionary != null && structTreeRoot != null)
          {
            if (field.PdfTag != null && field.PdfTag is PdfStructureElement)
              structTreeRoot.Add(field.PdfTag as PdfStructureElement, field.Page, annotDictionary);
            else
              structTreeRoot.Add(new PdfStructureElement(PdfTagType.Form), field.Page, annotDictionary);
          }
        }
      }
    }
    field.AnnotationIndex = this.List.Count - 1;
    return this.List.Count - 1;
  }

  protected virtual void DoInsert(int index, PdfField field)
  {
    this.m_array.Insert(index, (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) field));
    this.List.Insert(index, (object) field);
  }

  protected virtual void DoRemove(PdfField field)
  {
    this.m_array.RemoveAt(this.List.IndexOf((object) field));
    this.List.Remove((object) field);
  }

  protected virtual void DoRemoveAt(int index)
  {
    this.m_array.RemoveAt(index);
    this.List.RemoveAt(index);
  }

  protected new virtual void DoClear()
  {
    this.m_array.Clear();
    this.List.Clear();
  }

  internal bool RemoveContainingFieldItems(
    PdfDictionary fieldDictionary,
    PdfReferenceHolder pageReferenceHolder,
    out bool removeField)
  {
    bool flag = false;
    removeField = false;
    if (fieldDictionary.ContainsKey("Kids") && fieldDictionary["Kids"] is PdfArray field)
    {
      for (int index = field.Count - 1; index >= 0; --index)
      {
        PdfReferenceHolder pdfReferenceHolder = (PdfReferenceHolder) null;
        PdfDictionary pdfDictionary = (object) (field[index] as PdfReferenceHolder) == null ? field[index] as PdfDictionary : (field[index] as PdfReferenceHolder).Object as PdfDictionary;
        if (pdfDictionary != null && pdfDictionary.ContainsKey("P"))
          pdfReferenceHolder = pdfDictionary["P"] as PdfReferenceHolder;
        if (pdfReferenceHolder != (PdfReferenceHolder) null && pdfReferenceHolder == pageReferenceHolder)
        {
          field.RemoveAt(index);
          field.MarkChanged();
          flag = true;
        }
      }
      if (field.Count == 0)
        removeField = true;
    }
    return flag;
  }

  private PdfField InsertLoadedField(PdfLoadedField field, PdfPageBase newPage)
  {
    if (!(newPage as PdfPage).Section.ParentDocument.EnableMemoryOptimization)
    {
      PdfDictionary dictionary1 = field.Dictionary;
      PdfDictionary dictionary2 = field.Page.Dictionary;
      PdfDictionary dictionary3 = newPage.Dictionary;
      field = field.Clone(newPage) as PdfLoadedField;
      PdfArray array1 = field.CrossTable.GetObject(dictionary1["Kids"]) as PdfArray;
      PdfArray array2 = field.CrossTable.GetObject(dictionary2["Annots"]) as PdfArray;
      PdfArray newArray = field.CrossTable.GetObject(dictionary3["Annots"]) as PdfArray;
      if (array1 != null)
      {
        PdfArray kidsArray = new PdfArray(array1);
        field.Dictionary["Kids"] = (IPdfPrimitive) kidsArray;
        this.UpdateReferences(kidsArray, array2, newArray, (PdfField) field);
        field.Dictionary.Remove("P");
      }
      else
      {
        PdfReferenceHolder element = new PdfReferenceHolder((IPdfPrimitive) dictionary1);
        int index = array2.IndexOf((IPdfPrimitive) element);
        if (index >= 0)
          field.Dictionary = PdfCrossTable.Dereference(newArray[index]) as PdfDictionary;
      }
      return (PdfField) field;
    }
    int num1 = 0;
    if (newPage.Dictionary.ContainsKey("Annots"))
    {
      num1 = newPage.ObtainAnnotations().Count;
    }
    else
    {
      PdfArray primitive = new PdfArray();
      newPage.Dictionary.SetProperty("Annots", (IPdfPrimitive) primitive);
    }
    PdfField wrapper1 = field.Clone(newPage);
    if (wrapper1 is PdfLoadedTextBoxField)
    {
      PdfDictionary dictionary = (wrapper1 as PdfLoadedTextBoxField).Dictionary;
      if (dictionary.ContainsKey("V"))
      {
        if (dictionary["V"] is PdfString)
          (dictionary["V"] as PdfString).IsFormField = true;
        else if ((object) (dictionary["V"] as PdfReferenceHolder) != null)
          ((dictionary["V"] as PdfReferenceHolder).Object as PdfString).IsFormField = true;
      }
    }
    bool flag1 = false;
    if (wrapper1 is PdfLoadedSignatureField)
      flag1 = true;
    if (field.CrossTable.GetObject(field.Dictionary["Kids"]) is PdfArray pdfArray1 && pdfArray1.Count > 0 && !flag1)
    {
      PdfCrossTable crossTable = (newPage as PdfPage).Section.ParentDocument.CrossTable;
      if (wrapper1 is PdfLoadedCheckBoxField)
      {
        PdfLoadedCheckBoxField loadedCheckBoxField = wrapper1 as PdfLoadedCheckBoxField;
        if (loadedCheckBoxField.Items.Count > 0)
          loadedCheckBoxField.Items.DoClear();
      }
      for (int index1 = 0; index1 < pdfArray1.Count; ++index1)
      {
        PdfDictionary dictionary4 = PdfCrossTable.Dereference(pdfArray1[index1]) as PdfDictionary;
        PdfDictionary pdfDictionary = new PdfDictionary(dictionary4);
        PdfName key1 = new PdfName("Parent");
        PdfName key2 = new PdfName("P");
        pdfDictionary.Remove(key1);
        pdfDictionary.Remove(key2);
        PdfDictionary dictionary5 = pdfDictionary.Clone(crossTable) as PdfDictionary;
        dictionary5[key1] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) wrapper1);
        bool flag2 = false;
        PdfPageBase wrapper2;
        if (dictionary4.ContainsKey(key2))
        {
          if (PdfCrossTable.Dereference(dictionary4[key2]) is PdfDictionary key3 && field.CrossTable.PageCorrespondance.ContainsKey((IPdfPrimitive) key3) && field.CrossTable.PageCorrespondance[(IPdfPrimitive) key3] != null)
          {
            wrapper2 = field.CrossTable.PageCorrespondance[(IPdfPrimitive) key3] as PdfPageBase;
            if (wrapper2 == newPage)
            {
              dictionary5[key2] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) wrapper2);
              flag2 = true;
            }
            else
              continue;
          }
          else
            continue;
        }
        else
        {
          dictionary5[key2] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) newPage);
          wrapper2 = newPage;
          PdfDictionary dictionary6 = field.Page.Dictionary;
          if (dictionary6 != null && field.CrossTable.GetObject(dictionary6["Annots"]) is PdfArray pdfArray && pdfArray.Contains(pdfArray1[index1]))
            flag2 = true;
        }
        if (flag2)
        {
          PdfLoadedFieldItem loadedItem = (wrapper1 as PdfLoadedField).CreateLoadedItem(dictionary5);
          if (loadedItem != null && loadedItem.Page != null && wrapper2 != null)
            loadedItem.Page = wrapper2;
          PdfArray annotations = newPage.ObtainAnnotations();
          if (num1 < annotations.Count)
          {
            for (int index2 = annotations.Count - 1; index2 >= num1; --index2)
              annotations.RemoveAt(index2);
          }
          annotations.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) dictionary5));
          ++num1;
        }
      }
    }
    else
    {
      PdfArray annotations = newPage.ObtainAnnotations();
      if (num1 < annotations.Count)
      {
        for (int index = annotations.Count - 1; index >= num1; --index)
          annotations.RemoveAt(index);
      }
      annotations.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) wrapper1));
      int num2 = num1 + 1;
    }
    return wrapper1;
  }

  private void UpdateReferences(
    PdfArray kidsArray,
    PdfArray array,
    PdfArray newArray,
    PdfField field)
  {
    if (kidsArray == null)
      return;
    PdfLoadedField pdfLoadedField = field as PdfLoadedField;
    int index1 = 0;
    for (int count = kidsArray.Count; index1 < count; ++index1)
    {
      PdfReferenceHolder kids = kidsArray[index1] as PdfReferenceHolder;
      if (array != null)
      {
        int index2 = array.IndexOf((IPdfPrimitive) kids);
        if (index2 < 0 && pdfLoadedField != null)
        {
          PdfLoadedPageCollection pages = (pdfLoadedField.CrossTable.Document as PdfLoadedDocument).Pages;
          Dictionary<IPdfPrimitive, object> pageCorrespondance = pdfLoadedField.CrossTable.PageCorrespondance;
          for (int index3 = 0; index3 < pages.Count; ++index3)
          {
            PdfPageBase pdfPageBase1 = pages[index3];
            if (pageCorrespondance[((IPdfWrapper) pdfPageBase1).Element] is PdfPageBase pdfPageBase3)
            {
              newArray = pdfLoadedField.CrossTable.GetObject(pdfPageBase3.Dictionary["Annots"]) as PdfArray;
              if (PdfCrossTable.Dereference(kidsArray[index1]) is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey("P") && PdfCrossTable.Dereference(pdfDictionary["P"]) is PdfDictionary key)
              {
                array = pdfLoadedField.CrossTable.GetObject(key["Annots"]) as PdfArray;
                if (pdfLoadedField.CrossTable.PageCorrespondance[(IPdfPrimitive) key] is PdfPageBase pdfPageBase2 && pdfPageBase2 == pdfPageBase3 && array != null && newArray != null && array.Count == newArray.Count)
                {
                  index2 = array.IndexOf((IPdfPrimitive) kids);
                  if (index2 >= 0)
                    break;
                }
              }
            }
          }
        }
        if (index2 >= 0 && newArray != null && index2 < newArray.Count)
        {
          IPdfPrimitive element = newArray[index2];
          kidsArray.RemoveAt(index1);
          kidsArray.Insert(index1, element);
          if (PdfCrossTable.Dereference(element) is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey("Parent"))
            pdfDictionary["Parent"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) field);
        }
      }
    }
  }

  private int GetFieldIndex(string name)
  {
    int num = -1;
    if (this.m_fieldNames == null)
    {
      this.m_fieldNames = new Dictionary<string, int>();
      foreach (PdfField pdfField in this.List)
      {
        ++num;
        this.m_fieldNames.Add(pdfField.Name.Split('[')[0], num);
      }
    }
    int fieldIndex = -1;
    this.m_fieldNames.TryGetValue(name, out fieldIndex);
    return fieldIndex;
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_array;
}
