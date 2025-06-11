// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotTables.PivotFieldItem
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotTables;

internal class PivotFieldItem : IPivotFieldItem
{
  private bool b_Visible = true;
  private PivotFieldImpl m_Parent;
  private string m_name;
  private PivotItemOptions m_itemOptions;

  public string Text
  {
    get => this.m_itemOptions != null ? this.m_itemOptions.UserCaption : this.m_name;
    set
    {
      if (!(value != this.m_name))
        return;
      if (this.m_itemOptions == null)
        this.m_itemOptions = new PivotItemOptions();
      this.m_itemOptions.UserCaption = value;
    }
  }

  internal string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }

  public PivotFieldImpl Parent
  {
    get => this.m_Parent;
    set => this.m_Parent = value;
  }

  public bool Visible
  {
    get => this.b_Visible;
    set
    {
      if (!value)
        ++this.Parent.m_iItemInvisibleCount;
      if (this.Parent.m_iItemInvisibleCount >= this.Parent.Items.Count)
        throw new ArgumentNullException("All the items cannot be set invisible");
      foreach (KeyValuePair<int, PivotItemOptions> itemOption in this.Parent.ItemOptions)
      {
        if (itemOption.Value != null)
          itemOption.Value.IsHidden = false;
      }
      this.Parent.IsMultiSelected = true;
      this.b_Visible = value;
    }
  }

  public int Position
  {
    get => this.Parent.GetPosition(this);
    set
    {
      this.Parent.SortType = new PivotFieldSortType?(PivotFieldSortType.Manual);
      this.Parent.SetPosition(this, value);
    }
  }

  internal PivotItemOptions ItemOptions
  {
    get
    {
      if (this.m_itemOptions == null)
        this.m_itemOptions = new PivotItemOptions();
      return this.m_itemOptions;
    }
    set => this.m_itemOptions = value;
  }
}
