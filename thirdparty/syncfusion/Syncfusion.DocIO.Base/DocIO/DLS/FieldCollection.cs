// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.FieldCollection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class FieldCollection : CollectionImpl
{
  internal List<string> m_sortedAutoNumFieldIndexes;
  internal List<WField> m_sortedAutoNumFields;
  private Dictionary<char, int> CharValues;

  internal WField this[string name] => this.FindByName(name);

  internal WField this[int index] => this.InnerList[index] as WField;

  internal List<WField> SortedAutoNumFields
  {
    get
    {
      if (this.m_sortedAutoNumFields == null)
        this.m_sortedAutoNumFields = new List<WField>();
      return this.m_sortedAutoNumFields;
    }
    set => this.m_sortedAutoNumFields = value;
  }

  internal List<string> SortedAutoNumFieldIndexes
  {
    get
    {
      if (this.m_sortedAutoNumFieldIndexes == null)
        this.m_sortedAutoNumFieldIndexes = new List<string>();
      return this.m_sortedAutoNumFieldIndexes;
    }
    set => this.m_sortedAutoNumFieldIndexes = value;
  }

  internal FieldCollection(WordDocument doc)
    : base(doc, (OwnerHolder) doc)
  {
  }

  public WField FindByName(string name)
  {
    name.Replace('-', '_');
    for (int index = 0; index < this.InnerList.Count; ++index)
    {
      WField inner = this.InnerList[index] as WField;
      if (inner.FieldValue.Equals(name, StringComparison.CurrentCultureIgnoreCase))
        return inner;
    }
    return (WField) null;
  }

  public void RemoveAt(int index)
  {
    WField inner = this.InnerList[index] as WField;
    if (inner.FieldType == FieldType.FieldAutoNum && this.SortedAutoNumFields.Contains(inner))
    {
      int index1 = this.SortedAutoNumFields.IndexOf(inner);
      this.SortedAutoNumFields.Remove(inner);
      this.SortedAutoNumFieldIndexes.RemoveAt(index1);
    }
    this.Remove(inner);
  }

  public void Remove(WField field)
  {
    if (field.FieldType == FieldType.FieldAutoNum && this.SortedAutoNumFields.Contains(field))
    {
      int index = this.SortedAutoNumFields.IndexOf(field);
      this.SortedAutoNumFields.Remove(field);
      this.SortedAutoNumFieldIndexes.RemoveAt(index);
    }
    this.InnerList.Remove((object) field);
    field.IsAdded = false;
  }

  public void Clear()
  {
    while (this.InnerList.Count > 0)
      this.RemoveAt(this.InnerList.Count - 1);
  }

  internal void Add(WField field)
  {
    if (field.IsAdded)
      return;
    if (field.FieldType == FieldType.FieldAutoNum)
      this.InsertAutoNumFieldInAsc(field);
    this.InnerList.Add((object) field);
    field.IsAdded = true;
  }

  internal void InsertAutoNumFieldInAsc(WField field)
  {
    string hierarchicalIndex = field.GetHierarchicalIndex(string.Empty);
    if (this.SortedAutoNumFieldIndexes.Count == 0 && hierarchicalIndex != null && !hierarchicalIndex.Contains("-"))
    {
      this.SortedAutoNumFieldIndexes.Add(hierarchicalIndex);
      field.OriginalField = (WField) null;
      this.SortedAutoNumFields.Add(field);
    }
    else
    {
      if (hierarchicalIndex == null || hierarchicalIndex.Contains("-"))
        return;
      bool flag = false;
      int index = this.SortedAutoNumFieldIndexes.Count - 1;
      while (index > -1 && !flag)
      {
        if (this.IsNewIndexhasLowHierarchy(this.SortedAutoNumFieldIndexes[index], hierarchicalIndex))
          --index;
        else
          flag = true;
      }
      this.SortedAutoNumFieldIndexes.Insert(index + 1, hierarchicalIndex);
      field.OriginalField = (WField) null;
      this.SortedAutoNumFields.Insert(index + 1, field);
    }
  }

  private bool IsNewIndexhasLowHierarchy(string oldHierarchicalIndex, string newHierarchicalIndex)
  {
    string[] strArray1 = oldHierarchicalIndex.Split(';');
    string[] strArray2 = newHierarchicalIndex.Split(';');
    for (int index = 0; index < strArray1.Length || index < strArray2.Length; ++index)
    {
      if (index < strArray1.Length && index >= strArray2.Length)
        return true;
      if (index >= strArray1.Length && index < strArray2.Length)
        return false;
      if (Convert.ToInt32(strArray2[index]) < Convert.ToInt32(strArray1[index]))
        return true;
      if (Convert.ToInt32(strArray2[index]) > Convert.ToInt32(strArray1[index]))
        return false;
    }
    return false;
  }

  internal string GetAutoNumFieldResult(WField field)
  {
    int num1 = 0;
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    int num2 = field.OriginalField == null ? this.SortedAutoNumFields.IndexOf(field) : this.SortedAutoNumFields.IndexOf(field.OriginalField);
    if (num2 == 0)
      return (num2 + 1).ToString();
    for (int index = num2 - 1; index >= 0; --index)
    {
      if (this.SortedAutoNumFields[index].OwnerParagraph.StyleName == this.SortedAutoNumFields[num2].OwnerParagraph.StyleName && this.SortedAutoNumFields[index].OwnerParagraph.ParagraphFormat.OutlineLevel == this.SortedAutoNumFields[num2].OwnerParagraph.ParagraphFormat.OutlineLevel)
      {
        char seperatorCode = this.GetSeperatorCode(index);
        string numberFormat = this.GetNumberFormat(index);
        string str = this.SortedAutoNumFields[index].Text.TrimEnd(seperatorCode);
        switch (numberFormat)
        {
          case "ALPHABETIC":
          case "alphabetic":
            num1 = this.GetAsNumberFromLetter(str);
            break;
          case "ROMAN":
          case "roman":
            num1 = this.GetAsNumberFromRoman(str);
            break;
          default:
            num1 = Convert.ToInt32(str);
            break;
        }
        if (this.IsBothFieldsInSameParagarph(index, num2, field.OwnerParagraph != null && field.OwnerParagraph.Owner is WTableCell))
        {
          --num1;
          break;
        }
        break;
      }
      if (this.SortedAutoNumFields[index].OwnerParagraph.StyleName.StartsWith("Heading") || this.SortedAutoNumFields[index].OwnerParagraph.ParagraphFormat.OutlineLevel.ToString().StartsWith("Level"))
        return (num1 + 1).ToString();
    }
    return (num1 + 1).ToString();
  }

  private bool IsBothFieldsInSameParagarph(
    int previousIndex,
    int currentIndex,
    bool currentFieldIsInTable)
  {
    string autoNumFieldIndex1 = this.SortedAutoNumFieldIndexes[previousIndex];
    string autoNumFieldIndex2 = this.SortedAutoNumFieldIndexes[currentIndex];
    string[] strArray1 = autoNumFieldIndex1.Split(';');
    string[] strArray2 = autoNumFieldIndex2.Split(';');
    if (strArray1.Length == strArray2.Length && strArray1[0] == strArray2[0])
    {
      if (!currentFieldIsInTable && strArray1[strArray1.Length - 3] == strArray2[strArray2.Length - 3])
        return true;
      if (currentFieldIsInTable)
      {
        for (int index = strArray1.Length - 5; index > 0; --index)
        {
          if (strArray1[index] != strArray2[index])
            return false;
        }
        return true;
      }
    }
    return false;
  }

  private int GetAsNumberFromRoman(string roman)
  {
    if (this.CharValues == null)
    {
      this.CharValues = new Dictionary<char, int>();
      this.CharValues.Add('I', 1);
      this.CharValues.Add('V', 5);
      this.CharValues.Add('X', 10);
      this.CharValues.Add('L', 50);
      this.CharValues.Add('C', 100);
      this.CharValues.Add('D', 500);
      this.CharValues.Add('M', 1000);
    }
    if (roman.Length == 0)
      return 0;
    roman = roman.ToUpper();
    if (roman[0] == '(')
    {
      int num = roman.LastIndexOf(')');
      string roman1 = roman.Substring(1, num - 1);
      string roman2 = roman.Substring(num + 1);
      return 1000 * this.GetAsNumberFromRoman(roman1) + this.GetAsNumberFromRoman(roman2);
    }
    int asNumberFromRoman = 0;
    int num1 = 0;
    for (int index = roman.Length - 1; index >= 0; --index)
    {
      int charValue = this.CharValues[roman[index]];
      if (charValue < num1)
      {
        asNumberFromRoman -= charValue;
      }
      else
      {
        asNumberFromRoman += charValue;
        num1 = charValue;
      }
    }
    return asNumberFromRoman;
  }

  private char GetSeperatorCode(int indexOfField)
  {
    char[] chArray = new char[0];
    string empty = string.Empty;
    string[] strArray = this.SortedAutoNumFields[indexOfField].FieldCode.Split(new char[1]
    {
      ' '
    }, StringSplitOptions.RemoveEmptyEntries);
    return strArray.Length <= 4 ? '.' : strArray[4].ToCharArray()[0];
  }

  private string GetNumberFormat(int indexOfField)
  {
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    string[] strArray = this.SortedAutoNumFields[indexOfField].FieldCode.Split(new string[1]
    {
      " "
    }, StringSplitOptions.RemoveEmptyEntries);
    if (strArray.Length > 2)
      empty1 = strArray[2];
    return empty1;
  }

  private int GetAsNumberFromLetter(string s)
  {
    int numberFromLetter = 0;
    int num1 = 1;
    foreach (char c in s)
    {
      int num2 = (int) char.ToUpper(c) - 64 /*0x40*/;
      if (num1 == 1)
        numberFromLetter += num2;
      if (num1 > 1)
        numberFromLetter += 25 + num2;
      ++num1;
    }
    return numberFromLetter;
  }
}
