// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.FilterItemsCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

[Serializable]
public class FilterItemsCollection : List<FilterItemElement>
{
  private const char formatMarker = '\u009D';
  private bool isSuspendUpdate;
  private string m_Name = string.Empty;
  private bool showSubTotal = true;
  private bool allowRunTimeGroupByField = true;

  public FilterItemsCollection()
  {
    this.AllFilterItem = new FilterItemElement();
    this.AllFilterItem.Key = "(All)";
    this.FilteredValues = new List<string>();
    this.AllFilterItem.PropertyChanged += new PropertyChangedEventHandler(this.PropertyChanged);
    this.Add(this.AllFilterItem);
  }

  public PropertyDescriptor FilterProperty { get; set; }

  public FilterItemElement AllFilterItem { get; set; }

  public IComparer Comparer { get; set; }

  public string Name
  {
    get
    {
      if (string.IsNullOrEmpty(this.m_Name) && this.FilterProperty != null)
        this.m_Name = this.FilterProperty.Name;
      return this.m_Name;
    }
    set
    {
      if (this.FilterProperty != null && this.FilterProperty.Name == value)
        this.m_Name = this.FilterProperty.Name;
      else
        this.m_Name = value;
    }
  }

  public string DisplayHeader { get; set; }

  public string FieldCaption { get; set; }

  public string Format { get; set; }

  public bool ShowSubTotal
  {
    get => this.showSubTotal;
    set => this.showSubTotal = value;
  }

  public bool AllowRunTimeGroupByField
  {
    get => this.allowRunTimeGroupByField;
    set
    {
      if (this.allowRunTimeGroupByField == value)
        return;
      this.allowRunTimeGroupByField = value;
    }
  }

  public List<string> FilteredValues { get; set; }

  public int AddIfUnique(FilterItemElement filterItemElement)
  {
    int num = -1;
    if (filterItemElement != null)
    {
      num = this.BinarySearch(filterItemElement);
      if (num < 0)
      {
        this.Insert(-num - 1, filterItemElement);
        filterItemElement.PropertyChanged += new PropertyChangedEventHandler(this.PropertyChanged);
      }
    }
    return num;
  }

  public void AcceptChanges()
  {
    foreach (FilterItemElement filterItemElement in (List<FilterItemElement>) this)
    {
      bool? selectedState = filterItemElement.SelectedState;
      bool? isSelected = filterItemElement.IsSelected;
      if ((selectedState.GetValueOrDefault() != isSelected.GetValueOrDefault() ? 1 : (selectedState.HasValue != isSelected.HasValue ? 1 : 0)) != 0)
        filterItemElement.IsChanged = new bool?(false);
      filterItemElement.AcceptChanges();
    }
  }

  public void RejectChanges()
  {
    foreach (FilterItemElement filterItemElement in (List<FilterItemElement>) this)
      filterItemElement.RejectChanges();
  }

  public void SetName(string name) => this.Name = name;

  public void AddWireEvent(FilterItemElement element)
  {
    this.Add(element);
    element.PropertyChanged += new PropertyChangedEventHandler(this.PropertyChanged);
  }

  public string GetFilterExpressionForDataView()
  {
    if (this.Count <= 0)
      return string.Empty;
    StringBuilder stringBuilder1 = new StringBuilder();
    StringBuilder stringBuilder2 = new StringBuilder();
    bool flag = false;
    this.FilteredValues.Clear();
    if (this.Count < 700)
    {
      foreach (FilterItemElement filterItemElement in (List<FilterItemElement>) this)
      {
        if (filterItemElement != this.AllFilterItem)
        {
          if (filterItemElement.IsSelected.Value)
          {
            if (flag)
              stringBuilder1.Append(" OR ");
            else
              flag = true;
            if (!string.IsNullOrEmpty(filterItemElement.Key))
              stringBuilder1.Append($"[{this.HandleBracketsInColumnNamesInFilters(this.FilterProperty.Name)}] = '{(filterItemElement.Key.Contains("'") ? (object) filterItemElement.Key.Replace("'", "''") : (object) filterItemElement.Key)}'");
            else if (string.IsNullOrEmpty(filterItemElement.Key) && this.FilterProperty.PropertyType != typeof (string))
              stringBuilder1.Append(this.FilterProperty.Name + " = null");
            else
              stringBuilder1.Append(string.Format("IsNull([{0}], 'Null Column') = 'Null Column' OR [{0}] = '{1}'", (object) this.HandleBracketsInColumnNamesInFilters(this.FilterProperty.Name), filterItemElement.Key.Contains("'") ? (object) filterItemElement.Key.Replace("'", "''") : (object) filterItemElement.Key));
            if (!this.FilteredValues.Contains(filterItemElement.Key))
              this.FilteredValues.Add(filterItemElement.Key);
          }
          else if (this.FilteredValues.Contains(filterItemElement.Key))
            this.FilteredValues.Remove(filterItemElement.Key);
        }
      }
    }
    else
    {
      stringBuilder1.Append($"[{this.HandleBracketsInColumnNamesInFilters(this.FilterProperty.Name)}]" + " IN");
      for (int index = 0; index < this.Count; ++index)
      {
        FilterItemElement filterItemElement = this[index];
        if (filterItemElement != this.AllFilterItem)
        {
          if (filterItemElement.IsSelected.Value)
          {
            if (!string.IsNullOrEmpty(filterItemElement.Key) && !stringBuilder1.ToString().Contains("("))
              stringBuilder1.Append($"{"("}'{(filterItemElement.Key.Contains("'") ? (object) filterItemElement.Key.Replace("'", "''") : (object) filterItemElement.Key)}'");
            else if (!string.IsNullOrEmpty(filterItemElement.Key) && stringBuilder1.ToString().Contains("("))
            {
              stringBuilder1.Append(",");
              stringBuilder1.Append($"'{(filterItemElement.Key.Contains("'") ? (object) filterItemElement.Key.Replace("'", "''") : (object) filterItemElement.Key)}'");
            }
            else
              stringBuilder2.Append("OR " + $"[{this.HandleBracketsInColumnNamesInFilters(this.FilterProperty.Name)}] IS NULL");
            if (!this.FilteredValues.Contains(filterItemElement.Key))
              this.FilteredValues.Add(filterItemElement.Key);
          }
          else if (this.FilteredValues.Contains(filterItemElement.Key))
            this.FilteredValues.Remove(filterItemElement.Key);
        }
      }
      stringBuilder1.Append(")");
      if (stringBuilder2.Length > 0)
        stringBuilder1.Append(" " + (object) stringBuilder2);
    }
    return stringBuilder1.ToString();
  }

  public string GetFilterExpression(bool IEnumerableSource)
  {
    if (IEnumerableSource)
    {
      if (this.Count <= 0)
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      bool flag = false;
      this.FilteredValues.Clear();
      foreach (FilterItemElement filterItemElement in (List<FilterItemElement>) this)
      {
        if (filterItemElement != this.AllFilterItem)
        {
          if (filterItemElement.IsSelected.HasValue && filterItemElement.IsSelected.Value)
          {
            if (flag)
              stringBuilder.Append(" || ");
            else
              flag = true;
            if (this.FilterProperty is ExpressionPropertyDescriptor)
            {
              ExpressionPropertyDescriptor filterProperty = this.FilterProperty as ExpressionPropertyDescriptor;
              string format = filterProperty.Format;
              int num;
              if (format != null && (num = format.IndexOf(":")) > -1)
              {
                string str = format.Substring(num + 1).Replace("}", "");
                stringBuilder.Append($"({filterProperty.Expression} ToString {str}) = {(object) '\u009D'}{filterItemElement.Key}{(object) '\u009D'}");
              }
              else
                stringBuilder.Append($"{filterProperty.Expression} = {filterItemElement.Key}");
            }
            else
            {
              if (filterItemElement.Key.Contains(" "))
                filterItemElement.Key = filterItemElement.Key.Replace(" ", '\u0081'.ToString());
              if (string.IsNullOrEmpty(filterItemElement.Key) && Nullable.GetUnderlyingType(this.FilterProperty.PropertyType) != (Type) null && this.FilterProperty.PropertyType != typeof (string))
                stringBuilder.Append(this.FilterProperty.Name + " = null");
              else if (string.IsNullOrEmpty(filterItemElement.Key) && this.FilterProperty.PropertyType != typeof (string))
                stringBuilder.Append($"{this.FilterProperty.Name} = {(object) 0}");
              else if (this.FilterProperty.PropertyType == typeof (double) && filterItemElement.Key.GetType() == typeof (string))
              {
                NumberStyles style = NumberStyles.Currency;
                double result;
                double.TryParse(filterItemElement.Key, style, (IFormatProvider) null, out result);
                stringBuilder.Append($"{this.FilterProperty.Name} = {(object) result}");
              }
              else
                stringBuilder.Append($"{this.FilterProperty.Name} = {filterItemElement.Key}");
            }
            if (!this.FilteredValues.Contains(filterItemElement.Key))
              this.FilteredValues.Add(filterItemElement.Key);
          }
          else if (this.FilteredValues.Contains(filterItemElement.Key))
            this.FilteredValues.Remove(filterItemElement.Key);
        }
      }
      return stringBuilder.ToString();
    }
    if (this.Count <= 0)
      return string.Empty;
    StringBuilder stringBuilder1 = new StringBuilder();
    bool flag1 = false;
    this.FilteredValues.Clear();
    foreach (FilterItemElement filterItemElement in (List<FilterItemElement>) this)
    {
      if (filterItemElement != this.AllFilterItem)
      {
        if (filterItemElement.IsSelected.Value)
        {
          if (flag1)
            stringBuilder1.Append(" OR ");
          else
            flag1 = true;
          if (this.FilterProperty is ExpressionPropertyDescriptor)
          {
            ExpressionPropertyDescriptor filterProperty = this.FilterProperty as ExpressionPropertyDescriptor;
            stringBuilder1.Append($"{filterProperty.Expression} = '{filterItemElement.Key}'");
          }
          else
            stringBuilder1.Append($"{this.FilterProperty.Name} = '{filterItemElement.Key}'");
          if (!this.FilteredValues.Contains(filterItemElement.Key))
            this.FilteredValues.Add(filterItemElement.Key);
        }
        else if (this.FilteredValues.Contains(filterItemElement.Key))
          this.FilteredValues.Remove(filterItemElement.Key);
      }
    }
    return stringBuilder1.ToString();
  }

  public string GetFilterItem()
  {
    if (this.Count <= 0)
      return string.Empty;
    StringBuilder stringBuilder = new StringBuilder();
    foreach (FilterItemElement filterItemElement in (List<FilterItemElement>) this)
    {
      if (filterItemElement != this.AllFilterItem && filterItemElement.IsSelected.Value)
        return filterItemElement.Key;
    }
    return stringBuilder.ToString();
  }

  public string GetFilterExpression(
    bool IEnumerableSource,
    FilterItemsCollection filterItemsCollection,
    string format)
  {
    if (IEnumerableSource)
    {
      if (this.Count <= 0)
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      this.FilteredValues.Clear();
      foreach (FilterItemElement filterItemElement in (List<FilterItemElement>) this)
      {
        bool flag = true;
        if (filterItemElement != this.AllFilterItem)
        {
          if (filterItemElement.IsSelected.Value)
          {
            for (int index = 0; index < filterItemsCollection.Count; ++index)
            {
              if (this.FilterProperty is ExpressionPropertyDescriptor)
              {
                ExpressionPropertyDescriptor filterProperty = this.FilterProperty as ExpressionPropertyDescriptor;
                stringBuilder.Append($"{filterProperty.Expression} = '{filterItemsCollection[index].Key}'");
                if (flag)
                  stringBuilder.Append(" || ");
              }
              else if (filterItemsCollection[index] != filterItemsCollection.AllFilterItem)
              {
                DateTime dateTime = Convert.ToDateTime(filterItemsCollection[index].Key);
                if (string.Format(format == null || format.Length <= 0 ? (string) null : $"{{0:{format}}}", (object) dateTime) == filterItemElement.Key)
                {
                  stringBuilder.Append($"{this.FilterProperty.Name} = {filterItemsCollection[index].Key}");
                  if (flag)
                    stringBuilder.Append(" || ");
                }
              }
              if (!this.FilteredValues.Contains(filterItemElement.Key))
                this.FilteredValues.Add(filterItemElement.Key);
            }
          }
          else if (this.FilteredValues.Contains(filterItemElement.Key))
            this.FilteredValues.Remove(filterItemElement.Key);
        }
      }
      if (stringBuilder.ToString().EndsWith(" || "))
        stringBuilder.Remove(stringBuilder.Length - 4, 4);
      return stringBuilder.ToString();
    }
    if (this.Count <= 0)
      return string.Empty;
    StringBuilder stringBuilder1 = new StringBuilder();
    bool flag1 = false;
    this.FilteredValues.Clear();
    foreach (FilterItemElement filterItemElement in (List<FilterItemElement>) this)
    {
      if (filterItemElement != this.AllFilterItem)
      {
        if (filterItemElement.IsSelected.Value)
        {
          for (int index = 0; index < filterItemsCollection.Count; ++index)
          {
            if (this.FilterProperty is ExpressionPropertyDescriptor)
            {
              ExpressionPropertyDescriptor filterProperty = this.FilterProperty as ExpressionPropertyDescriptor;
              stringBuilder1.Append($"{filterProperty.Expression} = '{filterItemsCollection[index].Key}'");
              if (flag1)
                stringBuilder1.Append(" OR ");
            }
            else
            {
              stringBuilder1.Append($"{this.FilterProperty.Name} = '{filterItemsCollection[index].Key}'");
              if (flag1)
                stringBuilder1.Append(" OR ");
            }
            if (!this.FilteredValues.Contains(filterItemElement.Key))
              this.FilteredValues.Add(filterItemElement.Key);
          }
        }
        else if (this.FilteredValues.Contains(filterItemElement.Key))
          this.FilteredValues.Remove(filterItemElement.Key);
      }
    }
    if (stringBuilder1.ToString().EndsWith(" OR "))
      stringBuilder1.Remove(stringBuilder1.Length - 4, 4);
    return stringBuilder1.ToString();
  }

  internal string GetExpressionForVisibleRecords(bool IEnumerableSource)
  {
    if (IEnumerableSource)
    {
      if (this.Count <= 0)
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      bool flag = false;
      this.FilteredValues.Clear();
      foreach (FilterItemElement filterItemElement in (List<FilterItemElement>) this)
      {
        if (filterItemElement != this.AllFilterItem && filterItemElement.IsSelected.HasValue && filterItemElement.IsSelected.Value)
        {
          if (flag)
            stringBuilder.Append(" || ");
          else
            flag = true;
          if (this.FilterProperty is ExpressionPropertyDescriptor)
          {
            ExpressionPropertyDescriptor filterProperty = this.FilterProperty as ExpressionPropertyDescriptor;
            string format = filterProperty.Format;
            int num;
            if (format != null && (num = format.IndexOf(":")) > -1)
            {
              string str = format.Substring(num + 1).Replace("}", "");
              stringBuilder.Append($"({filterProperty.Expression} ToString {str}) = {(object) '\u009D'}{filterItemElement.Key}{(object) '\u009D'}");
            }
            else
              stringBuilder.Append($"{filterProperty.Expression} = {filterItemElement.Key}");
          }
          else
          {
            if (filterItemElement.Key.Contains(" "))
              filterItemElement.Key = filterItemElement.Key.Replace(" ", '\u0081'.ToString());
            if (string.IsNullOrEmpty(filterItemElement.Key) && Nullable.GetUnderlyingType(this.FilterProperty.PropertyType) != (Type) null && this.FilterProperty.PropertyType != typeof (string))
              stringBuilder.Append(this.FilterProperty.Name + " = null");
            else if (string.IsNullOrEmpty(filterItemElement.Key) && this.FilterProperty.PropertyType != typeof (string))
              stringBuilder.Append($"{this.FilterProperty.Name} = {(object) 0}");
            else if (this.FilterProperty.PropertyType == typeof (double) && filterItemElement.Key.GetType() == typeof (string))
            {
              NumberStyles style = NumberStyles.Currency;
              double result;
              double.TryParse(filterItemElement.Key, style, (IFormatProvider) null, out result);
              stringBuilder.Append($"{this.FilterProperty.Name} = {(object) result}");
            }
            else
              stringBuilder.Append($"{this.FilterProperty.Name} = {filterItemElement.Key}");
          }
        }
      }
      return stringBuilder.ToString();
    }
    if (this.Count <= 0)
      return string.Empty;
    StringBuilder stringBuilder1 = new StringBuilder();
    bool flag1 = false;
    this.FilteredValues.Clear();
    foreach (FilterItemElement filterItemElement in (List<FilterItemElement>) this)
    {
      if (filterItemElement != this.AllFilterItem && filterItemElement.IsSelected.Value)
      {
        if (flag1)
          stringBuilder1.Append(" OR ");
        else
          flag1 = true;
        if (this.FilterProperty is ExpressionPropertyDescriptor)
        {
          ExpressionPropertyDescriptor filterProperty = this.FilterProperty as ExpressionPropertyDescriptor;
          stringBuilder1.Append($"{filterProperty.Expression} = '{filterItemElement.Key}'");
        }
        else
          stringBuilder1.Append($"{this.FilterProperty.Name} = '{filterItemElement.Key}'");
      }
    }
    return stringBuilder1.ToString();
  }

  internal void Dispose()
  {
    if (this.FilteredValues != null)
    {
      this.FilteredValues.Clear();
      this.FilteredValues = (List<string>) null;
    }
    if (this.AllFilterItem != null)
    {
      this.AllFilterItem.PropertyChanged -= new PropertyChangedEventHandler(this.PropertyChanged);
      this.AllFilterItem = (FilterItemElement) null;
    }
    this.Comparer = (IComparer) null;
    this.DisplayHeader = (string) null;
    this.FilterProperty = (PropertyDescriptor) null;
  }

  private string HandleBracketsInColumnNamesInFilters(string name)
  {
    if (this.AllFilterItem == null)
      return (string) null;
    name = name.Replace("\\", "\\\\");
    name = name.Replace("]", "\\]");
    return name;
  }

  private void PropertyChanged(object sender, PropertyChangedEventArgs e)
  {
    if (!(e.PropertyName == "IsSelected") || this.isSuspendUpdate)
      return;
    FilterItemElement filterItemElement1 = (FilterItemElement) sender;
    if (filterItemElement1.Key == "(All)" && !this.isSuspendUpdate || filterItemElement1.IsAllFilter)
    {
      this.isSuspendUpdate = true;
      foreach (FilterItemElement filterItemElement2 in (List<FilterItemElement>) this)
      {
        if (filterItemElement2.Key != "(All)")
          filterItemElement2.IsSelected = filterItemElement1.IsSelected;
      }
      this.isSuspendUpdate = false;
    }
    else
    {
      this.isSuspendUpdate = true;
      if (this.Count - 1 == this.Where<FilterItemElement>((Func<FilterItemElement, bool>) (i =>
      {
        bool? isSelected = i.IsSelected;
        return (isSelected.GetValueOrDefault() ? 0 : (isSelected.HasValue ? 1 : 0)) != 0 && i.Key != "(All)" && !i.IsAllFilter;
      })).Count<FilterItemElement>())
        this.AllFilterItem.IsSelected = new bool?(false);
      else if (this.Count - 1 == this.Where<FilterItemElement>((Func<FilterItemElement, bool>) (i =>
      {
        bool? isSelected = i.IsSelected;
        return (!isSelected.GetValueOrDefault() ? 0 : (isSelected.HasValue ? 1 : 0)) != 0 && i.Key != "(All)" && !i.IsAllFilter;
      })).Count<FilterItemElement>())
      {
        this.AllFilterItem.IsSelected = new bool?(true);
      }
      else
      {
        this.AllFilterItem.IsSelected = new bool?();
        this.SeletedState();
      }
      this.isSuspendUpdate = false;
    }
  }

  private bool? SeletedState()
  {
    if (this.Count<FilterItemElement>() <= 1)
      return new bool?(true);
    bool? isSelected1 = this[1].IsSelected;
    foreach (FilterItemElement filterItemElement in (List<FilterItemElement>) this)
    {
      if (filterItemElement != this.AllFilterItem)
      {
        bool? isSelected2 = filterItemElement.IsSelected;
        bool? nullable = isSelected1;
        if ((isSelected2.GetValueOrDefault() != nullable.GetValueOrDefault() ? 1 : (isSelected2.HasValue != nullable.HasValue ? 1 : 0)) != 0)
          return new bool?(true);
      }
    }
    return isSelected1;
  }
}
