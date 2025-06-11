// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.AddInFunctionImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;
using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class AddInFunctionImpl : CommonObject, IAddInFunction, IParentApplication, ICloneParent
{
  private int m_iBookIndex;
  private int m_iNameIndex;
  private WorkbookImpl m_book;

  public AddInFunctionImpl(
    IApplication application,
    object parent,
    int iBookIndex,
    int iNameIndex)
    : base(application, parent)
  {
    this.m_iBookIndex = iBookIndex;
    this.m_iNameIndex = iNameIndex;
    this.SetParents();
  }

  private void SetParents()
  {
    this.m_book = this.FindParent(typeof (WorkbookImpl)) as WorkbookImpl;
    if (this.m_book == null)
      throw new ArgumentNullException("m_book");
  }

  public int BookIndex
  {
    get => this.m_iBookIndex;
    set => this.m_iBookIndex = value;
  }

  public int NameIndex
  {
    get => this.m_iNameIndex;
    set => this.m_iNameIndex = value;
  }

  public string Name
  {
    get
    {
      return this.BookIndex != -1 ? this.m_book.ExternWorkbooks[this.BookIndex].ExternNames[this.NameIndex].Name : this.m_book.InnerNamesColection[this.NameIndex].Name;
    }
  }

  public object Clone(object parent)
  {
    AddInFunctionImpl addInFunctionImpl = (AddInFunctionImpl) this.MemberwiseClone();
    addInFunctionImpl.SetParent(parent);
    addInFunctionImpl.SetParents();
    return (object) addInFunctionImpl;
  }
}
