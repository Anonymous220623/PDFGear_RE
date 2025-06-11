// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Collections.CustomXmlPartCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Collections;

public class CustomXmlPartCollection : 
  CollectionBaseEx<ICustomXmlPart>,
  ICustomXmlPartCollection,
  IEnumerable
{
  private Dictionary<string, ICustomXmlPart> m_propertiesHash = new Dictionary<string, ICustomXmlPart>();
  private WorkbookImpl m_book;
  private object m_parent;

  public CustomXmlPartCollection(IApplication application, object parent)
    : base(application, parent)
  {
    this.SetParent();
    this.m_parent = parent;
  }

  int ICustomXmlPartCollection.Count => this.List.Count;

  public new ICustomXmlPart this[int index]
  {
    get
    {
      if (index < 0 || index >= this.List.Count)
        throw new ArgumentOutOfRangeException($"index is {index}, Count is {this.List.Count}");
      return this.List[index];
    }
  }

  public new int Count => this.List.Count;

  public ICustomXmlPartCollection Clone() => (ICustomXmlPartCollection) this.Clone(this.m_parent);

  public ICustomXmlPart GetById(string id)
  {
    if (id == null)
      throw new ArgumentNullException(nameof (id));
    return this.m_propertiesHash.ContainsKey(id) ? this.m_propertiesHash[id] : (ICustomXmlPart) null;
  }

  void ICustomXmlPartCollection.RemoveAt(int index)
  {
    ICustomXmlPart customXmlPart = index >= 0 && index <= this.Count - 1 ? this.List[index] : throw new ArgumentOutOfRangeException(nameof (index), "Value cannot be less than 0 and greater than Count - 1.");
    this.List.RemoveAt(index);
    this.m_propertiesHash.Remove(customXmlPart.Id);
  }

  void ICustomXmlPartCollection.Clear()
  {
    this.List.Clear();
    this.m_propertiesHash.Clear();
  }

  public ICustomXmlPart Add(ICustomXmlPart customXmlPart)
  {
    if (this.m_propertiesHash.ContainsKey((customXmlPart as CustomXmlPart).Id) && !this.m_book.Loading)
      throw new ArgumentException("ID of the CustomXmlPart object must be unique.");
    this.AddLocal(customXmlPart, true);
    return customXmlPart;
  }

  public ICustomXmlPart Add(string id, byte[] XmlData)
  {
    if (id == null)
      throw new ArgumentNullException(nameof (id));
    if (XmlData == null)
      throw new ArgumentNullException(nameof (XmlData));
    if (id.Length == 0)
      throw new ArgumentException(nameof (id));
    if (XmlData.Length == 0)
      throw new ArgumentNullException(nameof (XmlData));
    CustomXmlPart customXmlPart = new CustomXmlPart(this.Application, (object) this, id, XmlData, this.List.Count);
    this.Add((ICustomXmlPart) customXmlPart);
    return (ICustomXmlPart) customXmlPart;
  }

  public ICustomXmlPart Add(string id)
  {
    switch (id)
    {
      case null:
        throw new ArgumentNullException("ID");
      case "":
        throw new ArgumentException("ID");
      default:
        CustomXmlPart customXmlPart = new CustomXmlPart(this.Application, (object) this.m_book, id, this.List.Count);
        this.Add((ICustomXmlPart) customXmlPart);
        return (ICustomXmlPart) customXmlPart;
    }
  }

  public void AddLocal(ICustomXmlPart name, bool bAddInGlobalNamesHash)
  {
    if (bAddInGlobalNamesHash)
      base.Add(name);
    else
      this.InnerList.Add(name);
  }

  protected override void OnInsertComplete(int index, ICustomXmlPart customXmlPart)
  {
    CustomXmlPart customXmlPart1 = (CustomXmlPart) customXmlPart;
    base.OnInsertComplete(index, customXmlPart);
    this.m_propertiesHash[customXmlPart1.Id] = (ICustomXmlPart) customXmlPart1;
  }

  private void SetParent()
  {
    this.m_book = this.FindParent(typeof (WorkbookImpl)) as WorkbookImpl;
    if (this.m_book == null)
      throw new ArgumentNullException("NamesCollection has no parent Workbook.");
  }

  public void Remove(string id)
  {
    ICustomXmlPart customXmlPart;
    if (!this.m_propertiesHash.TryGetValue(id, out customXmlPart))
      return;
    this.m_propertiesHash.Remove(id);
    this.List.Remove(customXmlPart);
  }
}
