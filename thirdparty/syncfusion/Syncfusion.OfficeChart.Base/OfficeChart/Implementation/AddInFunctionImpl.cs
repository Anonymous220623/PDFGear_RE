// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.AddInFunctionImpl
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Interfaces;
using System;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class AddInFunctionImpl : CommonObject, IAddInFunction, IParentApplication, ICloneParent
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

  public string Name => (string) null;

  public object Clone(object parent)
  {
    AddInFunctionImpl addInFunctionImpl = (AddInFunctionImpl) this.MemberwiseClone();
    addInFunctionImpl.SetParent(parent);
    addInFunctionImpl.SetParents();
    return (object) addInFunctionImpl;
  }
}
