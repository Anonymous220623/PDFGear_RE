// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.BuiltInDocumentProperties
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.CompoundFile.XlsIO;
using Syncfusion.CompoundFile.XlsIO.Native;
using Syncfusion.CompoundFile.XlsIO.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections;

public class BuiltInDocumentProperties(IApplication application, object parent) : 
  CollectionBaseEx<DocumentPropertyImpl>(application, parent),
  IBuiltInDocumentProperties
{
  private const STGM DEF_PROPERTY_STORAGE_OPTIONS = STGM.STGM_READWRITE | STGM.STGM_SHARE_EXCLUSIVE | STGM.STGM_CREATE;
  public static readonly Guid GuidSummary = new Guid("F29F85E0-4FF9-1068-AB91-08002B27B3D9");
  public static readonly Guid GuidDocument = new Guid("D5CDD502-2E9C-101B-9397-08002B2CF9AE");
  private Dictionary<int, DocumentPropertyImpl> m_documentHash = new Dictionary<int, DocumentPropertyImpl>();
  private Dictionary<int, DocumentPropertyImpl> m_summaryHash = new Dictionary<int, DocumentPropertyImpl>();

  public IDocumentProperty this[ExcelBuiltInProperty index]
  {
    get
    {
      int key = (int) index;
      IDictionary dictionary = this.GetDictionary(index);
      if (dictionary.Contains((object) key))
        return (IDocumentProperty) dictionary[(object) key];
      DocumentPropertyImpl documentPropertyImpl = new DocumentPropertyImpl((BuiltInProperty) index, (object) null);
      this.Add(documentPropertyImpl);
      return (IDocumentProperty) documentPropertyImpl;
    }
  }

  public IDocumentProperty this[int iIndex]
  {
    get
    {
      return iIndex >= 0 && iIndex <= this.Count - 1 ? (IDocumentProperty) this.List[iIndex] : throw new ArgumentOutOfRangeException(nameof (iIndex), "Value cannot be less than 0 and greater than than Count - 1.");
    }
  }

  public bool Contains(ExcelBuiltInProperty index)
  {
    int key = (int) index;
    return this.GetDictionary(index).Contains((object) key);
  }

  public string Title
  {
    get
    {
      return !this.Contains(ExcelBuiltInProperty.Title) ? (string) null : this[ExcelBuiltInProperty.Title].Text;
    }
    set => this[ExcelBuiltInProperty.Title].Text = value;
  }

  public string Subject
  {
    get
    {
      return !this.Contains(ExcelBuiltInProperty.Subject) ? (string) null : this[ExcelBuiltInProperty.Subject].Text;
    }
    set => this[ExcelBuiltInProperty.Subject].Text = value;
  }

  public string Author
  {
    get
    {
      return !this.Contains(ExcelBuiltInProperty.Author) ? (string) null : this[ExcelBuiltInProperty.Author].Text;
    }
    set => this[ExcelBuiltInProperty.Author].Text = value;
  }

  public string Keywords
  {
    get
    {
      return !this.Contains(ExcelBuiltInProperty.Keywords) ? (string) null : this[ExcelBuiltInProperty.Keywords].Text;
    }
    set => this[ExcelBuiltInProperty.Keywords].Text = value;
  }

  public string Comments
  {
    get
    {
      return !this.Contains(ExcelBuiltInProperty.Comments) ? (string) null : this[ExcelBuiltInProperty.Comments].Text;
    }
    set => this[ExcelBuiltInProperty.Comments].Text = value;
  }

  public string Template
  {
    get
    {
      return !this.Contains(ExcelBuiltInProperty.Template) ? (string) null : this[ExcelBuiltInProperty.Template].Text;
    }
    set => this[ExcelBuiltInProperty.Template].Text = value;
  }

  public string LastAuthor
  {
    get
    {
      return !this.Contains(ExcelBuiltInProperty.LastAuthor) ? (string) null : this[ExcelBuiltInProperty.LastAuthor].Text;
    }
    set => this[ExcelBuiltInProperty.LastAuthor].Text = value;
  }

  public string RevisionNumber
  {
    get
    {
      return !this.Contains(ExcelBuiltInProperty.RevisionNumber) ? (string) null : this[ExcelBuiltInProperty.RevisionNumber].Text;
    }
    set => this[ExcelBuiltInProperty.RevisionNumber].Text = value;
  }

  public TimeSpan EditTime
  {
    get
    {
      return !this.Contains(ExcelBuiltInProperty.EditTime) ? TimeSpan.MinValue : this[ExcelBuiltInProperty.EditTime].TimeSpan;
    }
    set => this[ExcelBuiltInProperty.EditTime].TimeSpan = value;
  }

  public DateTime LastPrinted
  {
    get
    {
      return !this.Contains(ExcelBuiltInProperty.LastPrinted) ? DateTime.MinValue : this[ExcelBuiltInProperty.LastPrinted].DateTime;
    }
    set => this[ExcelBuiltInProperty.LastPrinted].DateTime = value;
  }

  public DateTime CreationDate
  {
    get
    {
      return !this.Contains(ExcelBuiltInProperty.CreationDate) ? DateTime.Now : this[ExcelBuiltInProperty.CreationDate].DateTime;
    }
    set => this[ExcelBuiltInProperty.CreationDate].DateTime = value;
  }

  public DateTime LastSaveDate
  {
    get
    {
      return !this.Contains(ExcelBuiltInProperty.LastSaveDate) ? DateTime.Now : this[ExcelBuiltInProperty.LastSaveDate].DateTime;
    }
    set => this[ExcelBuiltInProperty.LastSaveDate].DateTime = value;
  }

  public int PageCount
  {
    get
    {
      return !this.Contains(ExcelBuiltInProperty.PageCount) ? int.MinValue : this[ExcelBuiltInProperty.PageCount].Int32;
    }
    set => this[ExcelBuiltInProperty.PageCount].Int32 = value;
  }

  public int WordCount
  {
    get
    {
      return !this.Contains(ExcelBuiltInProperty.WordCount) ? int.MinValue : this[ExcelBuiltInProperty.WordCount].Int32;
    }
    set => this[ExcelBuiltInProperty.WordCount].Int32 = value;
  }

  public int CharCount
  {
    get
    {
      return !this.Contains(ExcelBuiltInProperty.CharCount) ? int.MinValue : this[ExcelBuiltInProperty.CharCount].Int32;
    }
    set => this[ExcelBuiltInProperty.CharCount].Int32 = value;
  }

  public string ApplicationName
  {
    get
    {
      return !this.Contains(ExcelBuiltInProperty.ApplicationName) ? (string) null : this[ExcelBuiltInProperty.ApplicationName].Text;
    }
    set => this[ExcelBuiltInProperty.ApplicationName].Text = value;
  }

  internal bool HasHeadingPair
  {
    get
    {
      return this.Contains(ExcelBuiltInProperty.HeadingPair) && this[ExcelBuiltInProperty.HeadingPair].Value != null;
    }
    set => this[ExcelBuiltInProperty.HeadingPair].Value = (object) value;
  }

  public int Security
  {
    get
    {
      return !this.Contains(ExcelBuiltInProperty.Security) ? int.MinValue : this[ExcelBuiltInProperty.Security].Int32;
    }
    set => this[ExcelBuiltInProperty.Security].Int32 = value;
  }

  public string Category
  {
    get
    {
      return !this.Contains(ExcelBuiltInProperty.Category) ? (string) null : this[ExcelBuiltInProperty.Category].Text;
    }
    set => this[ExcelBuiltInProperty.Category].Text = value;
  }

  public string PresentationTarget
  {
    get
    {
      return !this.Contains(ExcelBuiltInProperty.PresentationTarget) ? (string) null : this[ExcelBuiltInProperty.PresentationTarget].Text;
    }
    set => this[ExcelBuiltInProperty.PresentationTarget].Text = value;
  }

  public int ByteCount
  {
    get
    {
      return !this.Contains(ExcelBuiltInProperty.ByteCount) ? int.MinValue : this[ExcelBuiltInProperty.ByteCount].Int32;
    }
    set => this[ExcelBuiltInProperty.ByteCount].Int32 = value;
  }

  public int LineCount
  {
    get
    {
      return !this.Contains(ExcelBuiltInProperty.LineCount) ? int.MinValue : this[ExcelBuiltInProperty.LineCount].Int32;
    }
    set => this[ExcelBuiltInProperty.LineCount].Int32 = value;
  }

  public int ParagraphCount
  {
    get
    {
      return !this.Contains(ExcelBuiltInProperty.PageCount) ? int.MinValue : this[ExcelBuiltInProperty.ParagraphCount].Int32;
    }
    set => this[ExcelBuiltInProperty.ParagraphCount].Int32 = value;
  }

  public int SlideCount
  {
    get
    {
      return !this.Contains(ExcelBuiltInProperty.SlideCount) ? int.MinValue : this[ExcelBuiltInProperty.SlideCount].Int32;
    }
    set => this[ExcelBuiltInProperty.SlideCount].Int32 = value;
  }

  public int NoteCount
  {
    get
    {
      return !this.Contains(ExcelBuiltInProperty.NoteCount) ? int.MinValue : this[ExcelBuiltInProperty.NoteCount].Int32;
    }
    set => this[ExcelBuiltInProperty.NoteCount].Int32 = value;
  }

  public int HiddenCount
  {
    get
    {
      return !this.Contains(ExcelBuiltInProperty.HiddenCount) ? int.MinValue : this[ExcelBuiltInProperty.HiddenCount].Int32;
    }
    set => this[ExcelBuiltInProperty.HiddenCount].Int32 = value;
  }

  public int MultimediaClipCount
  {
    get
    {
      return !this.Contains(ExcelBuiltInProperty.MultimediaClipCount) ? int.MinValue : this[ExcelBuiltInProperty.MultimediaClipCount].Int32;
    }
    set => this[ExcelBuiltInProperty.MultimediaClipCount].Int32 = value;
  }

  public bool ScaleCrop
  {
    get
    {
      return this.Contains(ExcelBuiltInProperty.ScaleCrop) && this[ExcelBuiltInProperty.ScaleCrop].Boolean;
    }
    set => this[ExcelBuiltInProperty.ScaleCrop].Boolean = value;
  }

  public string Manager
  {
    get
    {
      return !this.Contains(ExcelBuiltInProperty.Manager) ? (string) null : this[ExcelBuiltInProperty.Manager].Text;
    }
    set => this[ExcelBuiltInProperty.Manager].Text = value;
  }

  public string Company
  {
    get
    {
      return !this.Contains(ExcelBuiltInProperty.Company) ? (string) null : this[ExcelBuiltInProperty.Company].Text;
    }
    set => this[ExcelBuiltInProperty.Company].Text = value;
  }

  public bool LinksDirty
  {
    get
    {
      return this.Contains(ExcelBuiltInProperty.LinksDirty) && this[ExcelBuiltInProperty.LinksDirty].Boolean;
    }
    set => this[ExcelBuiltInProperty.LinksDirty].Boolean = value;
  }

  private IDictionary GetDictionary(ExcelBuiltInProperty propertyId)
  {
    bool bSummary;
    DocumentPropertyImpl.CorrectIndex((BuiltInProperty) propertyId, out bSummary);
    return !bSummary ? (IDictionary) this.m_documentHash : (IDictionary) this.m_summaryHash;
  }

  protected override void OnClearComplete()
  {
    this.m_documentHash.Clear();
    this.m_summaryHash.Clear();
  }

  [CLSCompliant(false)]
  public void Parse(DocumentPropertyCollection properties)
  {
    System.Collections.Generic.List<PropertySection> sections = properties.Sections;
    int index = 0;
    for (int count = sections.Count; index < count; ++index)
    {
      PropertySection section = sections[index];
      if (section.Id == BuiltInDocumentProperties.GuidSummary)
        BuiltInDocumentProperties.ReadProperties(section, (IDictionary) this.m_summaryHash, this.InnerList, true, true);
      else if (section.Id == BuiltInDocumentProperties.GuidDocument)
        BuiltInDocumentProperties.ReadProperties(section, (IDictionary) this.m_documentHash, this.InnerList, false, true);
    }
  }

  public static void ReadProperties(
    PropertySection section,
    IDictionary dicProperties,
    System.Collections.Generic.List<DocumentPropertyImpl> lstProperties,
    bool bSummary,
    bool bBuiltIn)
  {
    Dictionary<int, DocumentPropertyImpl> dictionary = (Dictionary<int, DocumentPropertyImpl>) null;
    if (!bBuiltIn)
      dictionary = new Dictionary<int, DocumentPropertyImpl>();
    System.Collections.Generic.List<PropertyData> properties = section.Properties;
    int index = 0;
    for (int count = properties.Count; index < count; ++index)
    {
      PropertyData variant = properties[index];
      if (variant.IsLinkToSource)
      {
        int parentId = variant.ParentId;
        dictionary[parentId].SetLinkSource((IPropertyData) variant);
      }
      else
      {
        DocumentPropertyImpl documentPropertyImpl = new DocumentPropertyImpl((IPropertyData) variant, bSummary);
        object key = bBuiltIn ? (object) (int) documentPropertyImpl.PropertyId : (object) documentPropertyImpl.Name;
        if (!bBuiltIn)
          dictionary.Add(variant.Id, documentPropertyImpl);
        bool flag = dicProperties.Contains(key);
        if (documentPropertyImpl.PropertyId == BuiltInProperty.HeadingPair)
          documentPropertyImpl.Boolean = true;
        dicProperties[key] = (object) documentPropertyImpl;
        if (!flag)
          lstProperties.Add(documentPropertyImpl);
      }
    }
  }

  public static void WriteProperties(PropertySection section, ICollection values)
  {
    int iPropertyId = 2;
    foreach (DocumentPropertyImpl property in (IEnumerable) values)
    {
      if (property.PropertyId != BuiltInProperty.HeadingPair)
      {
        PropertyData propertyData = BuiltInDocumentProperties.ConvertToPropertyData(property, iPropertyId);
        section.Properties.Add(propertyData);
        ++iPropertyId;
      }
    }
  }

  private static PropertyData ConvertToPropertyData(DocumentPropertyImpl property, int iPropertyId)
  {
    PropertyData variant = new PropertyData();
    property.FillPropVariant((IPropertyData) variant, iPropertyId);
    if (property.InternalName != null)
      variant.Name = property.InternalName;
    return variant;
  }

  [CLSCompliant(false)]
  public void Serialize(IPropertySetStorage setProp)
  {
    if (this.ApplicationName != null && this.ApplicationName != string.Empty)
      this.ApplicationName = "Essential XlsIO";
    BuiltInDocumentProperties.WriteProperties(setProp, BuiltInDocumentProperties.GuidSummary, (ICollection) this.m_summaryHash.Values);
    BuiltInDocumentProperties.WriteProperties(setProp, BuiltInDocumentProperties.GuidDocument, (ICollection) this.m_documentHash.Values);
  }

  [CLSCompliant(false)]
  public void Parse(IPropertySetStorage setProp)
  {
    BuiltInDocumentProperties.ReadProperties(setProp, BuiltInDocumentProperties.GuidSummary, (IDictionary) this.m_summaryHash, (IList<DocumentPropertyImpl>) this.InnerList, true, true);
    BuiltInDocumentProperties.ReadProperties(setProp, BuiltInDocumentProperties.GuidDocument, (IDictionary) this.m_documentHash, (IList<DocumentPropertyImpl>) this.InnerList, false, true);
  }

  [CLSCompliant(false)]
  public static void WriteProperties(
    IPropertySetStorage setProp,
    Guid guid,
    ICollection colProperties)
  {
    if (setProp == null)
      throw new ArgumentNullException(nameof (setProp));
    if (colProperties == null)
      throw new ArgumentNullException(nameof (colProperties));
    IPropertyStorage ppprstg = (IPropertyStorage) null;
    Guid empty = Guid.Empty;
    short codePage = (short) Encoding.Default.CodePage;
    try
    {
      setProp.Create(ref guid, ref empty, 0U, STGM.STGM_READWRITE | STGM.STGM_SHARE_EXCLUSIVE | STGM.STGM_CREATE, out ppprstg);
      using (PropVariant variant = new PropVariant())
      {
        variant.Id = 1;
        variant.SetValue((object) codePage, PropertyType.Int16);
        variant.Write(ppprstg);
        int iPropertyId = 2;
        foreach (DocumentPropertyImpl colProperty in (IEnumerable) colProperties)
        {
          if (colProperty.PropertyId != BuiltInProperty.HeadingPair)
          {
            colProperty.Write(ppprstg, variant, iPropertyId);
            ++iPropertyId;
          }
        }
      }
    }
    catch (Exception ex)
    {
    }
    finally
    {
      if (ppprstg != null)
      {
        ppprstg.Commit(STGC.STGC_DEFAULT);
        Marshal.FinalReleaseComObject((object) ppprstg);
      }
    }
  }

  [CLSCompliant(false)]
  public static void ReadProperties(
    IPropertySetStorage setProp,
    Guid guid,
    IDictionary dicProperties,
    IList<DocumentPropertyImpl> lstProperties,
    bool bSummary,
    bool bBuiltIn)
  {
    if (setProp == null)
      throw new ArgumentNullException(nameof (setProp));
    if (dicProperties == null)
      throw new ArgumentNullException(nameof (dicProperties));
    IPropertyStorage ppprstg = (IPropertyStorage) null;
    IEnumSTATPROPSTG ppenum = (IEnumSTATPROPSTG) null;
    Dictionary<int, DocumentPropertyImpl> dictionary = (Dictionary<int, DocumentPropertyImpl>) null;
    if (!bBuiltIn)
      dictionary = new Dictionary<int, DocumentPropertyImpl>();
    if (setProp.Open(ref guid, STGM.STGM_SHARE_EXCLUSIVE, out ppprstg) != 0)
      return;
    try
    {
      ppprstg.Enum(out ppenum);
      while (true)
      {
        int pceltFetched = 0;
        tagSTATPROPSTG rgelt = new tagSTATPROPSTG();
        ppenum.Next(1, ref rgelt, out pceltFetched);
        if (pceltFetched != 0)
        {
          using (PropVariant variant = new PropVariant(rgelt, ppprstg, bBuiltIn))
          {
            if (variant.IsLinkToSource)
            {
              int parentId = variant.ParentId;
              dictionary[parentId].SetLinkSource((IPropertyData) variant);
            }
            else
            {
              DocumentPropertyImpl documentPropertyImpl = new DocumentPropertyImpl((IPropertyData) variant, bSummary);
              object key = bBuiltIn ? (object) (int) documentPropertyImpl.PropertyId : (object) documentPropertyImpl.Name;
              if (!bBuiltIn)
                dictionary.Add((int) rgelt.propid, documentPropertyImpl);
              bool flag = dicProperties.Contains(key);
              dicProperties[key] = (object) documentPropertyImpl;
              if (!flag)
                lstProperties.Add(documentPropertyImpl);
            }
          }
        }
        else
          break;
      }
    }
    finally
    {
      if (ppprstg != null)
        Marshal.FinalReleaseComObject((object) ppprstg);
      if (ppenum != null)
        Marshal.FinalReleaseComObject((object) ppenum);
    }
  }

  protected override void OnInsertComplete(int index, DocumentPropertyImpl value)
  {
    base.OnInsertComplete(index, value);
    DocumentPropertyImpl documentPropertyImpl = value;
    ExcelBuiltInProperty propertyId = (ExcelBuiltInProperty) documentPropertyImpl.PropertyId;
    this.GetDictionary(propertyId).Add((object) (int) propertyId, (object) documentPropertyImpl);
  }

  public void Serialize(PropertySection summarySection, PropertySection documentSection)
  {
    BuiltInDocumentProperties.WriteProperties(summarySection, (ICollection) this.m_summaryHash.Values);
    BuiltInDocumentProperties.WriteProperties(documentSection, (ICollection) this.m_documentHash.Values);
  }
}
