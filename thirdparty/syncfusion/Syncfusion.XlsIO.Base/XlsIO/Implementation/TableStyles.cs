// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.TableStyles
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using System;
using System.Collections;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class TableStyles : CollectionBaseEx<ITableStyle>, ITableStyles, IEnumerable
{
  private WorkbookImpl m_workbook;
  private string m_defaultTableStyle;
  private string m_defaultPivotTableStyle;

  public ITableStyle this[string tableStyleName]
  {
    get
    {
      if (!this.Contains(tableStyleName))
        throw new ArgumentException("This table style not exists");
      for (int index = 0; index < this.Count; ++index)
      {
        if ((this.InnerList[index] as TableStyle).Equals(tableStyleName))
          return this.InnerList[index];
      }
      return (ITableStyle) null;
    }
  }

  public new ITableStyle this[int index] => this.InnerList[index];

  internal WorkbookImpl Workbook => this.m_workbook;

  internal string DefaultTablesStyle
  {
    get => this.m_defaultTableStyle;
    set => this.m_defaultTableStyle = value;
  }

  internal string DefaultPivotTableStyle
  {
    get => this.m_defaultPivotTableStyle;
    set => this.m_defaultPivotTableStyle = value;
  }

  public new int Count => base.Count;

  public TableStyles(WorkbookImpl workbook, IApplication application)
    : base(application, (object) workbook)
  {
    this.m_workbook = (WorkbookImpl) this.FindParent(typeof (WorkbookImpl));
  }

  public ITableStyle Add(string tableStyleName)
  {
    if (string.IsNullOrEmpty(tableStyleName))
      throw new ArgumentException("Table style name is empty");
    if (this.Contains(tableStyleName))
      throw new ArgumentException("Table style name is already exists");
    base.Add((ITableStyle) new TableStyle(tableStyleName, this));
    return this.InnerList[this.Count - 1];
  }

  public void Remove(ITableStyle tableStyle) => base.Remove(tableStyle);

  public new void RemoveAt(int index) => base.RemoveAt(index);

  public ITableStyle Add(ITableStyle tableStyle)
  {
    ((TableStyle) tableStyle).TableStyles = this;
    if (this.Contains(tableStyle.Name))
      throw new ArgumentException("Table style name is already exists");
    base.Add(tableStyle);
    return this.InnerList[this.Count - 1];
  }

  public new bool Contains(ITableStyle tableStyle) => this.Contains(tableStyle.Name);

  public bool Contains(string tableStyleName)
  {
    for (int index = 0; index < this.Count; ++index)
    {
      if ((this.InnerList[index] as TableStyle).Equals(tableStyleName))
        return true;
    }
    return false;
  }

  internal void Dispose()
  {
    if (this.InnerList == null)
      return;
    for (int index = 0; index < this.Count; ++index)
    {
      (this.InnerList[index] as TableStyle).Dispose();
      this.InnerList[index] = (ITableStyle) null;
    }
    this.Clear();
  }

  internal TableStyles Clone(WorkbookImpl workbook, IApplication application)
  {
    TableStyles tableStyles = new TableStyles(workbook, application);
    for (int index = 0; index < this.Count; ++index)
    {
      ITableStyle tableStyle = (this.InnerList[index] as TableStyle).Clone(tableStyles);
      tableStyles.InnerList.Add(tableStyle);
    }
    return tableStyles;
  }
}
