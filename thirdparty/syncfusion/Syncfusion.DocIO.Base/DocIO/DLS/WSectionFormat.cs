// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WSectionFormat
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class WSectionFormat : FormatBase
{
  private const float DEF_DISTANCE_BETWEEN_COLUMNS = 36f;
  internal const int BreakcodeKey = 1;
  internal const int TextDirectionKey = 2;
  internal const int PageSetupKey = 3;
  internal const int ChangedFormatKey = 4;
  internal const int FormatChangeAuthorNameKey = 5;
  internal const int FormatChangeDateTimeKey = 6;
  internal ColumnCollection m_columns;
  internal ColumnCollection m_sectFormattingColumnCollection;

  internal ColumnCollection SectFormattingColumnCollection
  {
    get => this.m_sectFormattingColumnCollection;
    set => this.m_sectFormattingColumnCollection = value;
  }

  internal WPageSetup PageSetup
  {
    get => (WPageSetup) this.GetPropertyValue(3);
    set => this.SetPropertyValue(3, (object) value);
  }

  internal SectionBreakCode BreakCode
  {
    get => (SectionBreakCode) this.GetPropertyValue(1);
    set => this.SetPropertyValue(1, (object) value);
  }

  internal ColumnCollection Columns => this.m_columns;

  internal DocTextDirection TextDirection
  {
    get => (DocTextDirection) this.GetPropertyValue(2);
    set => this.SetPropertyValue(2, (object) value);
  }

  internal bool IsChangedFormat
  {
    get => (bool) this.GetPropertyValue(4);
    set
    {
      if (!value)
        return;
      this.SetPropertyValue(4, (object) value);
    }
  }

  internal string FormatChangeAuthorName
  {
    get => (string) this.GetPropertyValue(5);
    set => this.SetPropertyValue(5, (object) value);
  }

  internal DateTime FormatChangeDateTime
  {
    get => (DateTime) this.GetPropertyValue(6);
    set => this.SetPropertyValue(6, (object) value);
  }

  internal WSectionFormat(WSection section)
    : base((IWordDocument) section.Document, (Entity) section)
  {
  }

  internal WSectionFormat Clone()
  {
    WSectionFormat wsectionFormat = new WSectionFormat(this.OwnerBase as WSection);
    wsectionFormat.ImportContainer((FormatBase) this);
    wsectionFormat.CopyProperties((FormatBase) this);
    return wsectionFormat;
  }

  internal object GetPropertyValue(int propKey) => this[propKey];

  internal void SetPropertyValue(int propKey, object value) => this[propKey] = value;

  protected override object GetDefValue(int key)
  {
    switch (key)
    {
      case 1:
        return (object) SectionBreakCode.NewPage;
      case 2:
        return (object) DocTextDirection.LeftToRight;
      case 3:
        return (object) new WPageSetup(this.OwnerBase as WSection);
      case 4:
        return (object) false;
      case 5:
        return (object) string.Empty;
      case 6:
        return (object) DateTime.MinValue;
      default:
        throw new ArgumentException("key not found");
    }
  }

  protected override FormatBase GetDefComposite(int key)
  {
    return key == 3 ? this.GetDefComposite(3, (FormatBase) new WPageSetup(this.OwnerBase as WSection)) : (FormatBase) null;
  }

  internal bool Compare(WSectionFormat sectionFormat)
  {
    if (!this.Compare(1, (FormatBase) sectionFormat) || !this.Compare(2, (FormatBase) sectionFormat) || this.PageSetup != null && sectionFormat.PageSetup != null && !this.PageSetup.Compare(sectionFormat.PageSetup) || this.Columns.Count != this.Columns.Count)
      return false;
    if (this.Columns.Count > 0 && this.Columns.Count > 0)
    {
      for (int index = 0; index < this.Columns.Count; ++index)
      {
        if (this.Columns[index].Compare(this.Columns[index]))
          return false;
      }
    }
    return true;
  }

  internal override void Close()
  {
    if (this.PageSetup != null)
    {
      this.PageSetup.Close();
      this.PageSetup = (WPageSetup) null;
    }
    if (this.Columns != null)
    {
      this.Columns.Close();
      this.m_columns = (ColumnCollection) null;
    }
    base.Close();
  }
}
