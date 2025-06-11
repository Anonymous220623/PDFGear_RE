// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.CustomXmlPart
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class CustomXmlPart : CommonObject, ICustomXmlPart
{
  private CustomXmlSchemaCollection m_schemacollection = new CustomXmlSchemaCollection();
  private string m_Id = "";
  private byte[] m_data;
  private WorkbookImpl m_book;
  private WorksheetImpl m_worksheet;
  private int m_index = -1;

  public CustomXmlPart(IApplication application, object parent, string id, int index)
    : this(application, parent, id, index, false)
  {
  }

  public CustomXmlPart(
    IApplication application,
    object parent,
    string id,
    int index,
    bool bIsLocal)
    : this(application, parent)
  {
    this.m_index = index;
    this.m_Id = id;
  }

  public CustomXmlPart(IApplication application, object parent)
    : base(application, parent)
  {
    this.SetParents();
  }

  public CustomXmlPart(
    IApplication application,
    object parent,
    string id,
    ICustomXmlSchemaCollection schemas,
    int index)
    : this(application, parent, id, schemas, index, false)
  {
  }

  public CustomXmlPart(
    IApplication application,
    object parent,
    string id,
    byte[] data,
    int index)
    : this(application, parent, id, data, index, false)
  {
  }

  public CustomXmlPart(
    IApplication application,
    object parent,
    string id,
    ICustomXmlSchemaCollection customXmlSchemaCollection,
    int index,
    bool bIsLocal)
    : this(application, parent)
  {
    this.m_index = index;
    this.SetParents();
    this.m_Id = id;
    this.Schemas = customXmlSchemaCollection;
  }

  public CustomXmlPart(
    IApplication application,
    object parent,
    string id,
    byte[] data,
    int index,
    bool bIsLocal)
    : this(application, parent)
  {
    this.m_index = index;
    this.m_data = data;
    this.SetParents();
    this.m_Id = id;
  }

  public override void Dispose()
  {
    base.Dispose();
    this.m_book = (WorkbookImpl) null;
  }

  public byte[] Data
  {
    get => this.m_data;
    set => this.m_data = value;
  }

  public string Id
  {
    get => this.m_Id;
    set => this.m_Id = value;
  }

  public ICustomXmlSchemaCollection Schemas
  {
    get => (ICustomXmlSchemaCollection) this.m_schemacollection;
    set => this.m_schemacollection = value as CustomXmlSchemaCollection;
  }

  public ICustomXmlPart Clone()
  {
    CustomXmlPart customXmlPart = (CustomXmlPart) this.MemberwiseClone();
    customXmlPart.m_schemacollection = (CustomXmlSchemaCollection) this.m_schemacollection.Clone();
    return (ICustomXmlPart) customXmlPart;
  }

  public void SetIndex(int index) => this.SetIndex(index, true);

  public void SetIndex(int index, bool bRaiseEvent)
  {
    if (index == this.m_index)
      return;
    this.m_index = index;
  }

  private void SetParents()
  {
    this.m_worksheet = this.FindParent(typeof (WorksheetImpl)) as WorksheetImpl;
    if (this.m_worksheet != null)
    {
      this.m_book = this.m_worksheet.Workbook as WorkbookImpl;
    }
    else
    {
      this.m_book = this.FindParent(typeof (WorkbookImpl)) as WorkbookImpl;
      if (this.m_book == null)
        throw new ArgumentNullException("IName has no parent workbook");
    }
  }
}
