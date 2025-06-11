// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.CustomDocumentProperties
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation;

internal class CustomDocumentProperties : ICustomDocumentProperties
{
  public const string CustomGuidString = "D5CDD505-2E9C-101B-9397-08002B2CF9AE";
  public static readonly Guid GuidCustom = new Guid("D5CDD505-2E9C-101B-9397-08002B2CF9AE");
  private Dictionary<string, DocumentPropertyImpl> _propertiesHash = new Dictionary<string, DocumentPropertyImpl>();
  private Syncfusion.Presentation.Presentation _presentation;
  private List<DocumentPropertyImpl> _list;

  public CustomDocumentProperties(Syncfusion.Presentation.Presentation presentaiton)
  {
    this._presentation = presentaiton;
    this._list = new List<DocumentPropertyImpl>();
  }

  public IDocumentProperty this[string strName]
  {
    get
    {
      IDocumentProperty property = this.GetProperty(strName);
      return !this._propertiesHash.ContainsKey(strName) ? (IDocumentProperty) null : property;
    }
  }

  public int Count => this._list.Count;

  public void Clear() => this._list.Clear();

  public IDocumentProperty this[int iIndex]
  {
    get
    {
      if (iIndex < 0 || iIndex > this._list.Count - 1)
        throw new ArgumentOutOfRangeException(nameof (iIndex), "Value cannot be less than 0 and greater than than Count - 1.");
      return (IDocumentProperty) this._list[iIndex];
    }
  }

  public IDocumentProperty GetProperty(string strName)
  {
    DocumentPropertyImpl property;
    this._propertiesHash.TryGetValue(strName, out property);
    return (IDocumentProperty) property;
  }

  internal List<DocumentPropertyImpl> GetDocumentPropertyList() => this._list;

  public void Remove(string strName)
  {
    DocumentPropertyImpl documentPropertyImpl;
    if (!this._propertiesHash.TryGetValue(strName, out documentPropertyImpl))
      return;
    this._propertiesHash.Remove(strName);
    this._list.Remove(documentPropertyImpl);
  }

  public IDocumentProperty Add(string strName)
  {
    DocumentPropertyImpl documentPropertyImpl = new DocumentPropertyImpl(strName, (object) null);
    this._propertiesHash.Add(strName, documentPropertyImpl);
    this._list.Add(documentPropertyImpl);
    return (IDocumentProperty) documentPropertyImpl;
  }

  public bool Contains(string strName) => this._propertiesHash.ContainsKey(strName);

  internal void Close()
  {
    if (this._list == null)
      return;
    this._list.Clear();
    this._list = (List<DocumentPropertyImpl>) null;
  }

  public CustomDocumentProperties Clone()
  {
    CustomDocumentProperties documentProperties = (CustomDocumentProperties) this.MemberwiseClone();
    if (this._list != null)
      documentProperties._list = this.CloneDocumentProperty();
    documentProperties._propertiesHash = this.CloneHash();
    return documentProperties;
  }

  private Dictionary<string, DocumentPropertyImpl> CloneHash()
  {
    Dictionary<string, DocumentPropertyImpl> dictionary = new Dictionary<string, DocumentPropertyImpl>();
    foreach (KeyValuePair<string, DocumentPropertyImpl> keyValuePair in this._propertiesHash)
    {
      string key = keyValuePair.Key;
      DocumentPropertyImpl documentPropertyImpl = (DocumentPropertyImpl) keyValuePair.Value.Clone();
      dictionary.Add(key, documentPropertyImpl);
    }
    return dictionary;
  }

  private List<DocumentPropertyImpl> CloneDocumentProperty()
  {
    List<DocumentPropertyImpl> documentPropertyImplList = new List<DocumentPropertyImpl>();
    foreach (DocumentPropertyImpl documentPropertyImpl in this._list)
      documentPropertyImplList.Add((DocumentPropertyImpl) documentPropertyImpl.Clone());
    return documentPropertyImplList;
  }

  internal void SetParent(Syncfusion.Presentation.Presentation presentation)
  {
    this._presentation = presentation;
  }
}
