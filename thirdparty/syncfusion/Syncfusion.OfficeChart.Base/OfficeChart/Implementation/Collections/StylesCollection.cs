// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Collections.StylesCollection
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Parser.Biff_Records;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Collections;

internal class StylesCollection : CollectionBaseEx<IStyle>, IStyles, IEnumerable
{
  private Dictionary<StyleImpl, StyleImpl> m_map = new Dictionary<StyleImpl, StyleImpl>();
  private Dictionary<string, StyleImpl> m_dictStyles;
  private Dictionary<int, StyleImpl> m_hashIndexToStyle;
  private WorkbookImpl m_holder;
  private EventHandler m_beforeChange;
  private EventHandler m_afterChange;

  public StylesCollection(IApplication application, object parent)
    : base(application, parent)
  {
    this.m_dictStyles = new Dictionary<string, StyleImpl>();
    this.m_holder = (WorkbookImpl) (this.FindParent(typeof (WorkbookImpl)) ?? throw new ArgumentException("Style collection must be in Workbook object tree."));
    this.m_beforeChange = new EventHandler(this.OnStyleBeforeChange);
    this.m_afterChange = new EventHandler(this.OnStyleAfterChange);
  }

  public IStyle this[string name]
  {
    get
    {
      StyleImpl styleImpl;
      if (!this.m_dictStyles.TryGetValue(name, out styleImpl))
        throw new ArgumentException("Style with specified name does not exist. Name: " + name, "value");
      return (IStyle) styleImpl;
    }
  }

  public IStyle Add(string name, object BasedOn)
  {
    if (name == null)
      throw new ArgumentNullException(nameof (name));
    if (this.m_dictStyles.ContainsKey(name))
      throw new ArgumentException("Name of style must be unique.");
    IStyle style = (IStyle) null;
    if (BasedOn == null)
      style = (IStyle) this.AppImplementation.CreateStyle(this.m_holder, name);
    else if (BasedOn is string)
      style = (IStyle) this.AppImplementation.CreateStyle(this.m_holder, name, (StyleImpl) this[(string) BasedOn]);
    else if (BasedOn is StyleImpl)
      style = (IStyle) this.AppImplementation.CreateStyle(this.m_holder, name, (StyleImpl) BasedOn);
    if (style != null)
      base.Add(style);
    return style;
  }

  public IStyle Add(string name) => this.Add(name, (object) null);

  public IStyles Merge(object Workbook) => this.Merge(Workbook, false);

  public IStyles Merge(object Workbook, bool overwrite)
  {
    if (Workbook == null)
      throw new ArgumentNullException(nameof (Workbook));
    if (!(Workbook is WorkbookImpl))
      throw new ArgumentException("Wrong argument type", nameof (Workbook));
    this.Merge((IWorkbook) Workbook, overwrite ? ExcelStyleMergeOptions.Replace : ExcelStyleMergeOptions.Leave);
    return (IStyles) this;
  }

  public void Remove(string styleName)
  {
    switch (styleName)
    {
      case null:
        break;
      case "":
        break;
      default:
        StyleImpl key;
        this.m_dictStyles.TryGetValue(styleName, out key);
        if (key == null || key.BuiltIn)
          break;
        int xformatIndex = key.XFormatIndex;
        this.m_map.Remove(key);
        this.Remove((IStyle) key);
        this.m_dictStyles.Remove(styleName);
        key.Workbook.RemoveExtenededFormatIndex(xformatIndex);
        break;
    }
  }

  public new void Add(IStyle style)
  {
    string name = style.Name;
    if (this.ContainsName(name))
    {
      StyleImpl styleImpl1 = (StyleImpl) style;
      StyleImpl styleImpl2;
      this.m_dictStyles.TryGetValue(name, out styleImpl2);
      if (styleImpl1.Index == styleImpl2.Index && styleImpl1.BuiltIn == styleImpl2.BuiltIn)
        throw new ArgumentException($"Collection already contains style with same names {style.Name}.");
      if (styleImpl1.BuiltIn)
        this.m_dictStyles[name] = styleImpl1;
      else if (!styleImpl2.BuiltIn && !this.m_holder.IsWorkbookOpening)
        throw new ArgumentException($"Collection already contains style with same names {style.Name}.");
    }
    base.Add(style);
  }

  public void Add(IStyle style, bool bReplace)
  {
    if (this.ContainsName(style.Name))
    {
      if (!bReplace)
        return;
      int index = 0;
      for (int count = this.List.Count; index < count; ++index)
      {
        if (this.List[index].Name == style.Name)
        {
          this.List[index] = style;
          break;
        }
      }
    }
    else
      base.Add(style);
  }

  public bool Contains(string name)
  {
    switch (name)
    {
      case null:
        throw new ArgumentNullException(nameof (name));
      case "":
        throw new ArgumentException("name - string cannot be empty.");
      default:
        return this.ContainsName(name);
    }
  }

  public IStyle ContainsSameStyle(IStyle style)
  {
    StyleImpl key = style != null ? style as StyleImpl : throw new ArgumentNullException(nameof (style));
    key.NotCompareNames = true;
    style = (IStyle) this.m_map[key];
    key.NotCompareNames = false;
    return style;
  }

  public static bool CompareStyles(IStyle source, IStyle destination)
  {
    return source.Color == destination.Color && source.PatternColor == destination.PatternColor && source.FillPattern == destination.FillPattern && source.NumberFormat == destination.NumberFormat && source.FormulaHidden == destination.FormulaHidden && source.HorizontalAlignment == destination.HorizontalAlignment && source.VerticalAlignment == destination.VerticalAlignment && source.WrapText == destination.WrapText && source.Font.Equals((object) destination.Font) && source.IncludeAlignment == destination.IncludeAlignment && source.IncludeBorder == destination.IncludeBorder && source.IncludeFont == destination.IncludeFont && source.IncludeNumberFormat == destination.IncludeNumberFormat && source.IncludePatterns == destination.IncludePatterns && source.IncludeProtection == destination.IncludeProtection && source.IndentLevel == destination.IndentLevel && StylesCollection.CompareBorders(source.Borders, destination.Borders) && source.Locked == destination.Locked && source.ShrinkToFit == destination.ShrinkToFit;
  }

  public static bool CompareBorders(IBorders source, IBorders destination)
  {
    return StylesCollection.CompareBorder(source[OfficeBordersIndex.EdgeBottom], destination[OfficeBordersIndex.EdgeBottom]) && StylesCollection.CompareBorder(source[OfficeBordersIndex.EdgeLeft], destination[OfficeBordersIndex.EdgeLeft]) && StylesCollection.CompareBorder(source[OfficeBordersIndex.EdgeRight], destination[OfficeBordersIndex.EdgeRight]) && StylesCollection.CompareBorder(source[OfficeBordersIndex.EdgeTop], destination[OfficeBordersIndex.EdgeTop]) && StylesCollection.CompareBorder(source[OfficeBordersIndex.DiagonalDown], destination[OfficeBordersIndex.DiagonalDown]) && StylesCollection.CompareBorder(source[OfficeBordersIndex.DiagonalUp], destination[OfficeBordersIndex.DiagonalUp]);
  }

  public static bool CompareBorder(IBorder source, IBorder destination)
  {
    return source.ColorObject == destination.ColorObject && source.LineStyle == destination.LineStyle && source.ShowDiagonalLine == destination.ShowDiagonalLine;
  }

  public Dictionary<string, string> Merge(IWorkbook workbook, ExcelStyleMergeOptions option)
  {
    return this.Merge(workbook, option, out Dictionary<int, int> _, out Dictionary<int, int> _);
  }

  public Dictionary<string, string> Merge(
    IWorkbook workbook,
    ExcelStyleMergeOptions option,
    out Dictionary<int, int> dicFontIndexes,
    out Dictionary<int, int> hashExtFormatIndexes)
  {
    WorkbookImpl workbookImpl = workbook is WorkbookImpl ? (WorkbookImpl) workbook : throw new ArgumentException("Wrong argument type", "Workbook");
    hashExtFormatIndexes = (Dictionary<int, int>) null;
    dicFontIndexes = (Dictionary<int, int>) null;
    if (workbookImpl == this.m_holder)
      return (Dictionary<string, string>) null;
    StylesCollection innerStyles = workbookImpl.InnerStyles;
    Dictionary<string, string> dictionary = new Dictionary<string, string>();
    hashExtFormatIndexes = this.m_holder.InnerExtFormats.Merge((IList<ExtendedFormatImpl>) workbookImpl.InnerExtFormats, out dicFontIndexes);
    int i = 0;
    for (int count = innerStyles.Count; i < count; ++i)
    {
      StyleImpl styleImpl = innerStyles[i] as StyleImpl;
      string key = styleImpl.Name;
      bool flag = this.m_dictStyles.ContainsKey(key);
      bool builtInCustomized = styleImpl.IsBuiltInCustomized;
      bool builtIn = styleImpl.BuiltIn;
      if (!builtIn || builtIn && builtInCustomized)
      {
        switch (option)
        {
          case ExcelStyleMergeOptions.Leave:
            key = (string) null;
            goto case ExcelStyleMergeOptions.Replace;
          case ExcelStyleMergeOptions.Replace:
            if (key != null)
            {
              StyleRecord style = (StyleRecord) styleImpl.Record.Clone();
              int extendedFormatIndex = (int) style.ExtendedFormatIndex;
              style.ExtendedFormatIndex = (ushort) hashExtFormatIndexes[extendedFormatIndex];
              style.StyleName = key;
              this.Add(style);
              continue;
            }
            continue;
          case ExcelStyleMergeOptions.CreateDiffName:
            key = CollectionBaseEx<IStyle>.GenerateDefaultName((ICollection<IStyle>) this, key + "_");
            dictionary.Add(key, key);
            goto case ExcelStyleMergeOptions.Replace;
          default:
            throw new ArgumentOutOfRangeException(nameof (option));
        }
      }
      else if (builtIn && !flag)
      {
        dictionary.Add(key, key);
        if (key != null)
        {
          StyleRecord style = (StyleRecord) styleImpl.Record.Clone();
          int extendedFormatIndex = (int) style.ExtendedFormatIndex;
          style.ExtendedFormatIndex = (ushort) hashExtFormatIndexes[extendedFormatIndex];
          style.StyleName = key;
          this.Add(style);
        }
      }
    }
    return dictionary;
  }

  public string GenerateDefaultName(string strStart)
  {
    return CollectionBaseEx<IStyle>.GenerateDefaultName(strStart, (ICollection) this.m_dictStyles.Values);
  }

  public string GenerateDefaultName(
    string strStart,
    Dictionary<string, StyleRecord> hashNamesInFile)
  {
    return CollectionBaseEx<IStyle>.GenerateDefaultName(strStart, (ICollection) this.m_dictStyles.Values, (ICollection) hashNamesInFile.Keys);
  }

  public StyleImpl CreateBuiltInStyle(string strName)
  {
    switch (strName)
    {
      case null:
        throw new ArgumentNullException(nameof (strName));
      case "":
        throw new ArgumentException("strName - string cannot be empty");
      default:
        StyleImpl style = this.AppImplementation.CreateStyle(this.m_holder, strName, true);
        base.Add((IStyle) style);
        return style;
    }
  }

  public StyleImpl GetByXFIndex(int index)
  {
    if (this.m_hashIndexToStyle != null && this.m_hashIndexToStyle.ContainsKey(index))
      return this.m_hashIndexToStyle[index];
    int i = 0;
    for (int count = this.Count; i < count; ++i)
    {
      StyleImpl byXfIndex = this[i] as StyleImpl;
      if (byXfIndex.Index == index)
        return byXfIndex;
    }
    return (StyleImpl) null;
  }

  public void UpdateStyleRecords()
  {
    System.Collections.Generic.List<IStyle> innerList = this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
      ((StyleImpl) innerList[index]).UpdateStyleRecord();
  }

  internal IStyle Find(string styleName) => this[styleName];

  internal bool ContainsName(string styleName) => this.m_dictStyles.ContainsKey(styleName);

  [CLSCompliant(false)]
  public StyleImpl Add(StyleRecord style)
  {
    if (style == null)
      throw new ArgumentNullException(nameof (style));
    StyleImpl style1 = this.AppImplementation.CreateStyle(this.m_holder, style);
    this.Add((IStyle) style1);
    return style1;
  }

  public override object Clone(object parent)
  {
    StylesCollection parent1 = parent != null ? new StylesCollection(this.Application, parent) : throw new ArgumentNullException(nameof (parent));
    System.Collections.Generic.List<IStyle> innerList = this.InnerList;
    int index = 0;
    for (int count = innerList.Count; index < count; ++index)
    {
      StyleImpl styleImpl = (StyleImpl) ((CommonWrapper) innerList[index]).Clone((object) parent1);
      parent1.Add((IStyle) styleImpl);
    }
    return (object) parent1;
  }

  public Dictionary<StyleImpl, StyleImpl> Map => this.m_map;

  protected override void OnClearComplete()
  {
    this.m_dictStyles.Clear();
    this.m_map.Clear();
    if (this.m_hashIndexToStyle != null)
      this.m_hashIndexToStyle.Clear();
    base.OnClearComplete();
  }

  protected override void OnInsertComplete(int index, IStyle value)
  {
    if (!this.m_dictStyles.ContainsKey(value.Name))
      this.m_dictStyles[value.Name] = (StyleImpl) value;
    StyleImpl key = (StyleImpl) value;
    if (this.m_hashIndexToStyle != null)
      this.m_hashIndexToStyle[key.Index] = key;
    this.m_map.Add(key, key);
    key.BeforeChange += this.m_beforeChange;
    key.AfterChange += this.m_afterChange;
    base.OnInsertComplete(index, value);
  }

  protected override void OnRemoveComplete(int index, IStyle value)
  {
    StyleImpl key = (StyleImpl) value;
    this.m_dictStyles.Remove(key.Name);
    if (this.m_hashIndexToStyle != null)
      this.m_hashIndexToStyle.Remove(key.Index);
    this.m_map.Remove(key);
    key.BeforeChange -= this.m_beforeChange;
    key.AfterChange -= this.m_afterChange;
    base.OnRemoveComplete(index, value);
  }

  protected override void OnSetComplete(int index, IStyle oldValue, IStyle newValue)
  {
    StyleImpl key1 = (StyleImpl) oldValue;
    StyleImpl key2 = (StyleImpl) newValue;
    if (this.m_hashIndexToStyle != null)
    {
      this.m_hashIndexToStyle.Remove(key1.Index);
      this.m_hashIndexToStyle[key2.Index] = key2;
    }
    this.m_dictStyles.Remove(key1.Name);
    this.m_map.Remove(key1);
    this.m_map.Add(key2, key2);
    if (this.m_dictStyles.ContainsKey(key2.Name))
      throw new ArgumentException("Collection cannot contain two styles with same name.");
    this.m_dictStyles[key2.Name] = key2;
    base.OnSetComplete(index, oldValue, newValue);
  }

  private void OnStyleBeforeChange(object sender, EventArgs args)
  {
    if (sender == null)
      throw new ArgumentNullException(nameof (sender));
    if (args == null)
      throw new ArgumentNullException(nameof (args));
    StyleImpl key = (StyleImpl) sender;
    this.m_map.Remove(key);
    key.BeforeChange -= this.m_beforeChange;
  }

  private void OnStyleAfterChange(object sender, EventArgs args)
  {
    if (sender == null)
      throw new ArgumentNullException(nameof (sender));
    if (args == null)
      throw new ArgumentNullException(nameof (args));
    StyleImpl key = (StyleImpl) sender;
    this.m_map.Add(key, key);
    key.BeforeChange += this.m_beforeChange;
  }

  internal void ClearStylesHash()
  {
    this.m_hashIndexToStyle = (Dictionary<int, StyleImpl>) null;
    this.m_map = (Dictionary<StyleImpl, StyleImpl>) null;
    this.m_dictStyles = (Dictionary<string, StyleImpl>) null;
  }

  internal void Dispose()
  {
    foreach (StyleImpl inner in this.InnerList)
      inner.Dispose();
  }
}
