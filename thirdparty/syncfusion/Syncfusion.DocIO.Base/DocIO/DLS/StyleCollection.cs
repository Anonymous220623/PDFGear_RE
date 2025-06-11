// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.StyleCollection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class StyleCollection : 
  XDLSSerializableCollection,
  IStyleCollection,
  ICollectionBase,
  IEnumerable
{
  internal string m_FixedIndex13StyleName = string.Empty;
  internal string m_FixedIndex14StyleName = string.Empty;
  private byte m_bFlags;

  public IStyle this[int index] => (IStyle) this.InnerList[index];

  public bool FixedIndex13HasStyle
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  public bool FixedIndex14HasStyle
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  public string FixedIndex13StyleName
  {
    get => this.m_FixedIndex13StyleName;
    set => this.m_FixedIndex13StyleName = value;
  }

  public string FixedIndex14StyleName
  {
    get => this.m_FixedIndex14StyleName;
    set => this.m_FixedIndex14StyleName = value;
  }

  internal StyleCollection(WordDocument doc)
    : base(doc, (OwnerHolder) doc)
  {
  }

  public int Add(IStyle style)
  {
    XDLSSerializableBase serializableBase = style != null ? (XDLSSerializableBase) style : throw new ArgumentNullException(nameof (style));
    serializableBase.CloneRelationsTo(this.Document, (OwnerHolder) null);
    serializableBase.SetOwner((OwnerHolder) this.Document);
    if (string.IsNullOrEmpty(style.Name.Trim()))
      this.UpdateAlternateStyleName(style);
    return this.InnerList.Add((object) style);
  }

  public IStyle FindByName(string name) => StyleCollection.FindStyleByName(this.InnerList, name);

  public IStyle FindByName(string name, StyleType styleType)
  {
    return StyleCollection.FindStyleByName(this.InnerList, name, styleType);
  }

  internal IStyle FindByName(
    string name,
    StyleType styleType,
    ref List<string> styelNames,
    ref bool isDiffTypeStyleFound)
  {
    return this.FindStyleByName(this.InnerList, name, styleType, ref styelNames, ref isDiffTypeStyleFound);
  }

  public IStyle FindById(int styleId) => StyleCollection.FindStyleById(this.InnerList, styleId);

  internal override void CloneToImpl(CollectionImpl coll)
  {
    StyleCollection styleCollection = coll as StyleCollection;
    int index = 0;
    for (int count = this.InnerList.Count; index < count; ++index)
    {
      IStyle inner = this.InnerList[index] as IStyle;
      styleCollection.Add(inner.Clone());
    }
  }

  internal void Remove(IStyle style) => this.InnerList.Remove((object) style);

  private void UpdateAlternateStyleName(IStyle style)
  {
    int count = this.InnerList.Count;
    string styleName;
    for (styleName = "a" + (object) count; this.IsSameNameExists(styleName); styleName = "a" + (object) count)
      ++count;
    style.Name = styleName;
  }

  private bool IsSameNameExists(string styleName)
  {
    foreach (IStyle inner in (IEnumerable) this.InnerList)
    {
      if (inner.Name == styleName)
        return true;
    }
    return false;
  }

  internal static IStyle FindStyleByName(IList styles, string name)
  {
    IStyle styleByName = (IStyle) null;
    for (int index = 0; index < styles.Count; ++index)
    {
      if (styles[index] is IStyle style && style.Name == name)
        styleByName = style;
    }
    return styleByName;
  }

  internal static IStyle FindStyleByName(IList styles, string name, bool findFirstStyle)
  {
    IStyle styleByName = (IStyle) null;
    for (int index = 0; index < styles.Count; ++index)
    {
      if (styles[index] is IStyle style && style.Name == name)
      {
        styleByName = style;
        break;
      }
    }
    return styleByName;
  }

  internal IStyle FindFirstStyleByName(string name)
  {
    return StyleCollection.FindStyleByName(this.InnerList, name, true);
  }

  internal Style FindStyleById(string name)
  {
    IList innerList = this.InnerList;
    Style styleById = (Style) null;
    for (int index = 0; index < innerList.Count; ++index)
    {
      if (innerList[index] is Style style && style.StyleIDName == name)
        styleById = style;
    }
    return styleById;
  }

  internal static IStyle FindStyleByName(IList styles, string name, StyleType styleType)
  {
    if (name == null)
      return (IStyle) null;
    IStyle styleByName = (IStyle) null;
    for (int index = 0; index < styles.Count; ++index)
    {
      if (styles[index] is Style style && style.Name == name && style.StyleType == styleType)
        styleByName = (IStyle) style;
    }
    return styleByName;
  }

  internal IStyle FindStyleByName(
    IList styles,
    string name,
    StyleType styleType,
    ref List<string> styleNames,
    ref bool isDiffTypeStyleFound)
  {
    if (name == null)
      return (IStyle) null;
    IStyle styleByName = (IStyle) null;
    for (int index = 0; index < styles.Count; ++index)
    {
      if (styles[index] is Style style)
      {
        if (style.Name.StartsWithExt(name + "_"))
          styleNames.Add(style.Name);
        if (style.Name == name)
        {
          if (style.StyleType == styleType)
            styleByName = (IStyle) style;
          else
            isDiffTypeStyleFound = true;
          styleNames.Add(style.Name);
        }
      }
    }
    return styleByName;
  }

  internal static IStyle FindStyleById(IList styles, int styleId)
  {
    IStyle styleById = (IStyle) null;
    for (int index = 0; index < styles.Count; ++index)
    {
      if (styles[index] is Style style && style.StyleId == styleId)
        styleById = (IStyle) style;
    }
    return styleById;
  }

  protected override OwnerHolder CreateItem(IXDLSContentReader reader)
  {
    switch (reader.GetAttributeValue("type"))
    {
      case "CharacterStyle":
        return (OwnerHolder) new WCharacterStyle(this.Document);
      default:
        return (OwnerHolder) new WParagraphStyle((IWordDocument) this.Document);
    }
  }

  protected override string GetTagItemName() => "style";
}
