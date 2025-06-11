// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.TableStyleElements
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using System;
using System.Collections;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class TableStyleElements : 
  CollectionBaseEx<ITableStyleElement>,
  ITableStyleElements,
  IEnumerable
{
  private TableStyle m_tableStyle;

  internal TableStyle TableStyle => this.m_tableStyle;

  public new int Count => base.Count;

  public ITableStyleElement this[ExcelTableStyleElementType tableStyleElementType]
  {
    get
    {
      if (!this.Contains(tableStyleElementType))
        throw new ArgumentException("This Table Style Element not Exists");
      for (int index = 0; index < this.Count; ++index)
      {
        if ((this.InnerList[index] as TableStyleElement).Equals(tableStyleElementType))
          return this.InnerList[index];
      }
      return (ITableStyleElement) null;
    }
  }

  public new ITableStyleElement this[int index] => this.InnerList[index];

  public TableStyleElements(TableStyle tableStyle, IApplication application)
    : base(application, (object) tableStyle)
  {
    this.m_tableStyle = tableStyle;
  }

  public ITableStyleElement Add(ExcelTableStyleElementType tableStyleElementType)
  {
    if (this.Contains(tableStyleElementType))
    {
      for (int index = 0; index < this.Count; ++index)
      {
        if ((this.InnerList[index] as TableStyleElement).Equals(tableStyleElementType))
          return this.InnerList[index];
      }
      return (ITableStyleElement) null;
    }
    base.Add((ITableStyleElement) new TableStyleElement(tableStyleElementType, this));
    return this.InnerList[this.Count - 1];
  }

  public override bool Equals(object obj)
  {
    TableStyleElements tableStyleElements = (TableStyleElements) obj;
    if (this.Count != tableStyleElements.Count)
      return false;
    for (int index = 0; index < tableStyleElements.Count; ++index)
    {
      if (!this.InnerList[index].Equals((object) tableStyleElements.InnerList[index]))
        return false;
    }
    return true;
  }

  public ITableStyleElement Add(ITableStyleElement tableStyleElement)
  {
    TableStyleElement tableStyleElement1 = (TableStyleElement) tableStyleElement;
    tableStyleElement1.TableStyleElements = this;
    if (string.IsNullOrEmpty(tableStyleElement1.TableStyleElementName))
    {
      if (this.Contains(tableStyleElement1.TableStyleElementType))
      {
        for (int index = 0; index < this.Count; ++index)
        {
          if (this.InnerList[index].Equals((object) tableStyleElement1.TableStyleElementType))
            return this.InnerList[index];
        }
        return (ITableStyleElement) null;
      }
      base.Add((ITableStyleElement) tableStyleElement1);
      return this.InnerList[this.Count - 1];
    }
    base.Add((ITableStyleElement) tableStyleElement1);
    return this.InnerList[this.Count - 1];
  }

  public void Remove(ITableStyleElement tableStyleElement) => base.Remove(tableStyleElement);

  public new void RemoveAt(int index) => base.RemoveAt(index);

  internal object Clone(TableStyle tableStyle)
  {
    TableStyleElements tableStyleElements = new TableStyleElements(tableStyle, tableStyle.TableStyles.Application);
    for (int index = 0; index < this.Count; ++index)
    {
      ITableStyleElement tableStyleElement = (ITableStyleElement) (this.InnerList[index] as TableStyleElement).Clone(tableStyleElements);
      tableStyleElements.InnerList.Add(tableStyleElement);
    }
    return (object) tableStyleElements;
  }

  internal void Dispose()
  {
    if (this.InnerList != null)
    {
      for (int index = 0; index < this.Count; ++index)
      {
        (this.InnerList[index] as TableStyleElement).Dispose();
        this.InnerList[index] = (ITableStyleElement) null;
      }
      this.Clear();
    }
    this.m_tableStyle = (TableStyle) null;
  }

  public new bool Contains(ITableStyleElement tableStyleElement)
  {
    return base.Contains(tableStyleElement);
  }

  public bool Contains(ExcelTableStyleElementType tableStyleElementType)
  {
    for (int index = 0; index < this.Count; ++index)
    {
      if ((this.InnerList[index] as TableStyleElement).Equals(tableStyleElementType))
        return true;
    }
    return false;
  }

  internal void Add(string tableStyleElementName)
  {
    base.Add((ITableStyleElement) new TableStyleElement(tableStyleElementName, this));
  }
}
