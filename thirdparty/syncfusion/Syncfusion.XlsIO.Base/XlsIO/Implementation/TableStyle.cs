// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.TableStyle
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class TableStyle : ITableStyle
{
  private string m_tableStyleName;
  private Syncfusion.XlsIO.Implementation.TableStyleElements m_tableStyleElements;
  private TableStyles m_tableStyles;

  public string Name
  {
    get => this.m_tableStyleName;
    set => this.m_tableStyleName = value;
  }

  public ITableStyleElements TableStyleElements
  {
    get
    {
      if (this.m_tableStyleElements == null)
        this.m_tableStyleElements = new Syncfusion.XlsIO.Implementation.TableStyleElements(this, this.TableStyles.Application);
      return (ITableStyleElements) this.m_tableStyleElements;
    }
  }

  internal TableStyles TableStyles
  {
    get => this.m_tableStyles;
    set => this.m_tableStyles = value;
  }

  public TableStyle(string tableStyleName, TableStyles tableStyles)
  {
    this.m_tableStyles = tableStyles;
    this.m_tableStyleName = tableStyleName;
  }

  public TableStyle(TableStyles tableStyles) => this.m_tableStyles = tableStyles;

  public override bool Equals(object obj)
  {
    return this.TableStyleElements.Equals((object) ((TableStyle) obj).TableStyleElements);
  }

  internal bool Equals(string tableStyleName) => this.Name.Equals(tableStyleName);

  public void Delete()
  {
    this.TableStyles.Remove((ITableStyle) this);
    this.Dispose();
  }

  public ITableStyle Duplicate()
  {
    TableStyle tableStyle = (TableStyle) this.Clone();
    for (int index = 2; index > 0; ++index)
    {
      string tableStyleName = $"{this.m_tableStyleName} {(object) index}";
      if (!this.TableStyles.Contains(tableStyleName))
      {
        tableStyle.m_tableStyleName = tableStyleName;
        this.TableStyles.Add((ITableStyle) tableStyle);
        break;
      }
    }
    return (ITableStyle) tableStyle;
  }

  public ITableStyle Duplicate(string tableStyleName)
  {
    TableStyle tableStyle = (TableStyle) this.Clone();
    tableStyle.m_tableStyleName = !this.TableStyles.Contains(tableStyleName) ? tableStyleName : throw new ArgumentException("Table style name is already exists");
    this.TableStyles.Add((ITableStyle) tableStyle);
    return (ITableStyle) tableStyle;
  }

  public ITableStyle Clone()
  {
    TableStyle tableStyle = (TableStyle) this.MemberwiseClone();
    if (this.m_tableStyleElements != null)
      tableStyle.m_tableStyleElements = (Syncfusion.XlsIO.Implementation.TableStyleElements) tableStyle.m_tableStyleElements.Clone(tableStyle);
    tableStyle.m_tableStyles = this.TableStyles;
    return (ITableStyle) tableStyle;
  }

  internal ITableStyle Clone(TableStyles tableStyles)
  {
    TableStyle tableStyle = (TableStyle) this.MemberwiseClone();
    if (this.m_tableStyleElements != null)
      tableStyle.m_tableStyleElements = (Syncfusion.XlsIO.Implementation.TableStyleElements) tableStyle.m_tableStyleElements.Clone(tableStyle);
    tableStyle.m_tableStyles = tableStyles;
    return (ITableStyle) tableStyle;
  }

  internal void Dispose()
  {
    if (this.m_tableStyleElements != null)
    {
      this.m_tableStyleElements.Dispose();
      this.m_tableStyleElements = (Syncfusion.XlsIO.Implementation.TableStyleElements) null;
    }
    this.m_tableStyleName = (string) null;
    this.m_tableStyles = (TableStyles) null;
  }
}
