// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfAttachmentCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using Syncfusion.Pdf.Security;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfAttachmentCollection : PdfCollection, IPdfWrapper
{
  private PdfArray m_array = new PdfArray();
  private PdfDictionary m_dictionary = new PdfDictionary();
  private Dictionary<string, PdfReferenceHolder> dic = new Dictionary<string, PdfReferenceHolder>();
  private System.Collections.Generic.List<string> orderList;
  private int count;
  internal PdfCrossTable m_CrossTable;
  private PdfMainObjectCollection m_objectCollection;
  private PdfDictionary attachmentDictionay;

  public PdfAttachmentCollection()
  {
    this.m_dictionary.SetProperty("Names", (IPdfPrimitive) this.m_array);
  }

  internal PdfAttachmentCollection(PdfDictionary attachmentDictionary, PdfCrossTable table)
  {
    this.m_dictionary = attachmentDictionary;
    this.m_CrossTable = table;
    PdfDictionary pdfDictionary1 = this.m_dictionary["EmbeddedFiles"] as PdfDictionary;
    if ((object) (this.m_dictionary["EmbeddedFiles"] as PdfReferenceHolder) != null && pdfDictionary1 == null)
      pdfDictionary1 = (this.m_dictionary["EmbeddedFiles"] as PdfReferenceHolder).Object as PdfDictionary;
    if (pdfDictionary1 == null)
      return;
    this.m_array = pdfDictionary1["Names"] as PdfArray;
    if (this.m_array == null && pdfDictionary1["Kids"] is PdfArray)
    {
      if (!(pdfDictionary1["Kids"] is PdfArray pdfArray) || pdfArray.Count == 0)
        return;
      for (int index = 0; index < pdfArray.Count; ++index)
      {
        if ((object) (pdfArray[index] as PdfReferenceHolder) != null || pdfArray[index] is PdfDictionary)
        {
          PdfDictionary pdfDictionary2 = pdfArray[index] as PdfDictionary;
          if ((object) (pdfArray[index] as PdfReferenceHolder) != null && pdfDictionary2 == null)
            pdfDictionary2 = (pdfArray[index] as PdfReferenceHolder).Object as PdfDictionary;
          if (pdfDictionary2 != null)
          {
            this.m_array = pdfDictionary2["Names"] as PdfArray;
            if (this.m_array != null)
              this.attachmentInformation(this.m_array);
          }
        }
      }
    }
    else
    {
      if (this.m_array == null)
        return;
      this.attachmentInformation(this.m_array);
    }
  }

  private void attachmentInformation(PdfArray m_array)
  {
    if (m_array.Count == 0)
      return;
    int index1 = 1;
    for (int index2 = 0; index2 < m_array.Count / 2; ++index2)
    {
      if ((object) (m_array[index1] as PdfReferenceHolder) != null || m_array[index1] is PdfDictionary)
      {
        PdfDictionary m = m_array[index1] as PdfDictionary;
        if ((object) (m_array[index1] as PdfReferenceHolder) != null && m == null)
          m = (m_array[index1] as PdfReferenceHolder).Object as PdfDictionary;
        if (m != null)
        {
          PdfStream pdfStream = new PdfStream();
          PdfDictionary pdfDictionary1 = (PdfDictionary) null;
          if (m.ContainsKey("EF"))
          {
            if (m["EF"] is PdfDictionary)
              pdfDictionary1 = m["EF"] as PdfDictionary;
            else if ((object) (m["EF"] as PdfReferenceHolder) != null)
              pdfDictionary1 = (m["EF"] as PdfReferenceHolder).Object as PdfDictionary;
            PdfReferenceHolder pdfReferenceHolder = pdfDictionary1["F"] as PdfReferenceHolder;
            if (pdfReferenceHolder != (PdfReferenceHolder) null)
            {
              PdfReference reference = pdfReferenceHolder.Reference;
              pdfStream = pdfReferenceHolder.Object as PdfStream;
              IPdfDecryptable pdfDecryptable = (IPdfDecryptable) pdfStream;
              if (pdfDecryptable != null && this.m_CrossTable.Encryptor != null && this.m_CrossTable.Encryptor.EncryptOnlyAttachment && reference != (PdfReference) null)
                pdfDecryptable.Decrypt(this.m_CrossTable.Encryptor, reference.ObjNum);
            }
          }
          PdfAttachment pdfAttachment;
          if (pdfStream != null)
          {
            pdfStream.Decompress();
            if (m.ContainsKey("F"))
            {
              pdfAttachment = new PdfAttachment((m["F"] as PdfString).Value, pdfStream.Data);
              PdfDictionary pdfDictionary2 = (PdfDictionary) pdfStream;
              if (pdfDictionary2 != null)
              {
                PdfName pdfName = PdfCrossTable.Dereference(pdfDictionary2["Subtype"]) as PdfName;
                if (pdfName != (PdfName) null)
                  pdfAttachment.MimeType = pdfName.Value.Replace("#23", "#").Replace("#20", " ").Replace("#2F", "/");
              }
              if (pdfDictionary2.ContainsKey("Params") && PdfCrossTable.Dereference(pdfDictionary2["Params"]) is PdfDictionary pdfDictionary3)
              {
                PdfString dateTimeStringValue1 = PdfCrossTable.Dereference(pdfDictionary3["CreationDate"]) as PdfString;
                PdfString dateTimeStringValue2 = PdfCrossTable.Dereference(pdfDictionary3["ModDate"]) as PdfString;
                if (dateTimeStringValue1 != null)
                  pdfAttachment.CreationDate = pdfDictionary3.GetDateTime(dateTimeStringValue1);
                if (dateTimeStringValue2 != null)
                  pdfAttachment.ModificationDate = pdfDictionary3.GetDateTime(dateTimeStringValue2);
              }
              if (m.ContainsKey("AFRelationship"))
              {
                PdfName pdfName = PdfCrossTable.Dereference(m["AFRelationship"]) as PdfName;
                if (pdfName != (PdfName) null)
                  pdfAttachment.Relationship = this.ObtainRelationShip(pdfName.Value);
              }
              if (m.ContainsKey("Desc"))
                pdfAttachment.Description = (m["Desc"] as PdfString).Value;
              if (m.ContainsKey("CI"))
              {
                PdfDictionary dictionary = (PdfDictionary) null;
                if (m["CI"] is PdfDictionary)
                  dictionary = m["CI"] as PdfDictionary;
                else if ((object) (m["CI"] as PdfReferenceHolder) != null)
                  dictionary = (m["CI"] as PdfReferenceHolder).Object as PdfDictionary;
                if (dictionary != null)
                {
                  PdfPortfolioAttributes portfolioAttributes = new PdfPortfolioAttributes(dictionary);
                  pdfAttachment.PortfolioAttributes = portfolioAttributes;
                }
              }
            }
            else
              pdfAttachment = new PdfAttachment((m["Desc"] as PdfString).Value, pdfStream.Data);
          }
          else
            pdfAttachment = !m.ContainsKey("Desc") ? new PdfAttachment((m["F"] as PdfString).Value) : new PdfAttachment((m["Desc"] as PdfString).Value);
          this.List.Add((object) pdfAttachment);
        }
      }
      index1 += 2;
    }
  }

  public PdfAttachment this[int index] => (PdfAttachment) this.List[index];

  public int Add(PdfAttachment attachment)
  {
    if (attachment == null)
      throw new ArgumentNullException(nameof (attachment));
    if (PdfDocument.ConformanceLevel == PdfConformanceLevel.Pdf_A1B || PdfDocument.ConformanceLevel == PdfConformanceLevel.Pdf_A1A || PdfDocument.ConformanceLevel == PdfConformanceLevel.Pdf_A2B || PdfDocument.ConformanceLevel == PdfConformanceLevel.Pdf_A2A || PdfDocument.ConformanceLevel == PdfConformanceLevel.Pdf_A2U)
      throw new PdfConformanceException($"Attachment is not allowed in {PdfDocument.ConformanceLevel.ToString()} Conformance.");
    int num = this.DoAdd(attachment);
    this.m_dictionary.Modify();
    return num;
  }

  public void Insert(int index, PdfAttachment attachment)
  {
    if (attachment == null)
      throw new ArgumentNullException(nameof (attachment));
    this.DoInsert(index, attachment);
  }

  public void Remove(PdfAttachment attachment)
  {
    if (attachment == null)
      throw new ArgumentNullException(nameof (attachment));
    this.DoRemove(attachment);
  }

  public void RemoveAt(int index) => this.DoRemoveAt(index);

  public int IndexOf(PdfAttachment attachment)
  {
    return attachment != null ? this.List.IndexOf((object) attachment) : throw new ArgumentNullException(nameof (attachment));
  }

  public bool Contains(PdfAttachment attachment)
  {
    return attachment != null ? this.List.Contains((object) attachment) : throw new ArgumentNullException(nameof (attachment));
  }

  public void Clear() => this.DoClear();

  private int DoAdd(PdfAttachment attachment)
  {
    string fileName = attachment.FileName;
    string key = !PdfString.IsUnicode(fileName) ? fileName : Encoding.ASCII.GetString(Encoding.Convert(Encoding.Unicode, Encoding.ASCII, Encoding.Unicode.GetBytes(fileName)));
    System.StringComparer ordinal = System.StringComparer.Ordinal;
    if (this.dic.Count == 0 && this.m_array.Count > 0)
    {
      for (int index = 0; index < this.m_array.Count; index += 2)
      {
        if (!this.dic.ContainsKey((this.m_array[index] as PdfString).Value))
          this.dic.Add((this.m_array[index] as PdfString).Value, this.m_array[index + 1] as PdfReferenceHolder);
        else
          this.dic.Add((this.m_array[index] as PdfString).Value + "_copy", this.m_array[index + 1] as PdfReferenceHolder);
      }
    }
    if (!this.dic.ContainsKey(key))
      this.dic.Add(key, new PdfReferenceHolder((IPdfWrapper) attachment));
    else
      this.dic.Add(key + "_copy", new PdfReferenceHolder((IPdfWrapper) attachment));
    this.orderList = new System.Collections.Generic.List<string>((IEnumerable<string>) this.dic.Keys);
    this.orderList.Sort((IComparer<string>) ordinal);
    this.m_array.Clear();
    foreach (string order in this.orderList)
    {
      this.m_array.Add((IPdfPrimitive) new PdfString(order));
      this.m_array.Add((IPdfPrimitive) this.dic[order]);
    }
    this.List.Add((object) attachment);
    return this.List.Count - 1;
  }

  private void DoInsert(int index, PdfAttachment attachment)
  {
    this.m_array.Insert(2 * index, (IPdfPrimitive) new PdfString(attachment.FileName));
    this.m_array.Insert(2 * index + 1, (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) attachment));
    this.List.Insert(index, (object) attachment);
  }

  private void DoRemove(PdfAttachment attachment)
  {
    int num = this.List.IndexOf((object) attachment);
    this.m_array.RemoveAt(2 * num);
    this.attachmentDictionay = PdfCrossTable.Dereference(this.m_array[2 * num]) as PdfDictionary;
    if (this.attachmentDictionay != null)
    {
      this.RemoveAttachementObjects(this.attachmentDictionay);
      this.attachmentDictionay = (PdfDictionary) null;
    }
    this.m_array.RemoveAt(2 * num);
    this.List.Remove((object) attachment);
  }

  private void DoRemoveAt(int index)
  {
    this.m_array.RemoveAt(2 * index);
    this.attachmentDictionay = PdfCrossTable.Dereference(this.m_array[2 * index]) as PdfDictionary;
    if (this.attachmentDictionay != null)
    {
      this.RemoveAttachementObjects(this.attachmentDictionay);
      this.attachmentDictionay = (PdfDictionary) null;
    }
    this.m_array.RemoveAt(2 * index);
    this.List.RemoveAt(index);
  }

  private new void DoClear()
  {
    this.List.Clear();
    if (this.m_CrossTable != null && this.m_CrossTable.Document.PdfObjects != null)
    {
      for (int index = 1; index < this.m_array.Count; index += 2)
      {
        if ((object) (this.m_array[index] as PdfReferenceHolder) != null && PdfCrossTable.Dereference(this.m_array[index]) is PdfDictionary attachmentDictionary)
          this.RemoveAttachementObjects(attachmentDictionary);
      }
    }
    this.m_array.Clear();
  }

  private void RemoveAttachementObjects(PdfDictionary attachmentDictionary)
  {
    if (this.m_CrossTable != null)
      this.m_objectCollection = this.m_CrossTable.Document.PdfObjects;
    if (this.m_objectCollection == null)
      return;
    if (attachmentDictionary != null)
    {
      if (attachmentDictionary.ContainsKey("EF") && PdfCrossTable.Dereference(attachmentDictionary["EF"]) is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey("F") && PdfCrossTable.Dereference(pdfDictionary["F"]) is PdfStream element && this.m_objectCollection.Contains((IPdfPrimitive) element))
        this.m_objectCollection.Remove(this.m_objectCollection.IndexOf((IPdfPrimitive) element));
      if (this.m_objectCollection.Contains((IPdfPrimitive) attachmentDictionary))
        this.m_objectCollection.Remove(this.m_objectCollection.IndexOf((IPdfPrimitive) attachmentDictionary));
    }
    if (this.dic.Count <= 0)
      return;
    this.dic.Clear();
  }

  private PdfAttachmentRelationship ObtainRelationShip(string relation)
  {
    PdfAttachmentRelationship relationShip = PdfAttachmentRelationship.Unspecified;
    switch (relation)
    {
      case "Alternative":
        relationShip = PdfAttachmentRelationship.Alternative;
        break;
      case "Data":
        relationShip = PdfAttachmentRelationship.Data;
        break;
      case "Source":
        relationShip = PdfAttachmentRelationship.Source;
        break;
      case "Supplement":
        relationShip = PdfAttachmentRelationship.Supplement;
        break;
      case "Unspecified":
        relationShip = PdfAttachmentRelationship.Unspecified;
        break;
    }
    return relationShip;
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_dictionary;
}
