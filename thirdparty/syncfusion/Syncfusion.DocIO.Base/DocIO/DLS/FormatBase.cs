// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.FormatBase
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public abstract class FormatBase : XDLSSerializableBase
{
  private const byte MAXPARENTLEVEL = 4;
  private const byte MAXKEY = 128 /*0x80*/;
  protected Dictionary<int, object> m_propertiesHash;
  protected Dictionary<int, object> m_oldPropertiesHash;
  private FormatBase m_baseFormat;
  private FormatBase m_parentFormat;
  private byte m_compositeKey;
  private byte m_parentLevel;
  private byte m_bFlags = 1;
  internal SinglePropertyModifierArray m_unParsedSprms;
  internal SinglePropertyModifierArray m_sprms;
  private List<Revision> m_revisions;

  internal bool IsDefault
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set
    {
      if (value)
        return;
      this.MarkNoDefault();
    }
  }

  internal bool IsFormattingChange
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  internal Dictionary<int, object> PropertiesHash
  {
    get
    {
      if (this.m_propertiesHash == null)
        this.m_propertiesHash = new Dictionary<int, object>();
      return this.m_propertiesHash;
    }
  }

  internal Dictionary<int, object> OldPropertiesHash
  {
    get
    {
      if (this.m_oldPropertiesHash == null)
        this.m_oldPropertiesHash = new Dictionary<int, object>();
      return this.m_oldPropertiesHash;
    }
  }

  internal FormatBase BaseFormat
  {
    get => this.m_baseFormat;
    set => this.m_baseFormat = value;
  }

  protected object this[int key]
  {
    get
    {
      int fullKey = this.GetFullKey(key);
      object obj = this.PropertiesHash.ContainsKey(fullKey) ? this.PropertiesHash[fullKey] : (object) this.GetDefComposite(key);
      if (obj == null && this.BaseFormat != null && this.BaseFormat.m_propertiesHash != null)
        obj = this.GetBaseFormatValue(key);
      if (this.CheckCharacterStyle(key))
        obj = (this as WCharacterFormat).CharStyle.CharacterFormat[key];
      if (obj == null)
        obj = this.GetDefValue(key);
      return obj;
    }
    set
    {
      int fullKey = this.GetFullKey(key);
      if (this.IsFormattingChange)
        this.OldPropertiesHash[fullKey] = value;
      else
        this.PropertiesHash[fullKey] = value;
      this.IsDefault = false;
      this.OnChange(this, key);
    }
  }

  internal FormatBase ParentFormat => this.m_parentFormat;

  internal List<Revision> Revisions
  {
    get
    {
      if (this.m_revisions == null)
        this.m_revisions = new List<Revision>();
      return this.m_revisions;
    }
  }

  public FormatBase()
    : this((IWordDocument) null)
  {
  }

  internal FormatBase(IWordDocument doc, bool isTextBox)
    : this(doc)
  {
    this.m_propertiesHash = (Dictionary<int, object>) null;
    this.m_baseFormat = (FormatBase) null;
    this.m_parentFormat = (FormatBase) null;
    this.m_sprms = (SinglePropertyModifierArray) null;
  }

  public FormatBase(IWordDocument doc)
    : this(doc, (Entity) null)
  {
  }

  public FormatBase(IWordDocument doc, Entity owner)
    : base(doc as WordDocument, owner)
  {
    this.m_propertiesHash = new Dictionary<int, object>();
    this.m_oldPropertiesHash = new Dictionary<int, object>();
  }

  public FormatBase(FormatBase parentFormat, int parentKey)
    : this((IWordDocument) null)
  {
    if ((int) parentFormat.m_parentLevel + 1 >= 4)
      throw new ArgumentOutOfRangeException("offset");
    if (parentKey > 128 /*0x80*/)
      throw new ArgumentOutOfRangeException(nameof (parentKey));
    this.m_propertiesHash = parentFormat.PropertiesHash;
    this.m_compositeKey = (byte) parentKey;
    this.m_parentFormat = parentFormat;
    this.m_parentLevel = (byte) ((uint) parentFormat.m_parentLevel + 1U);
  }

  public FormatBase(FormatBase parent, int parentKey, int parentOffset)
    : this(parent, parentKey)
  {
  }

  protected internal void ImportContainer(FormatBase format)
  {
    switch (format)
    {
      case WParagraphFormat _:
      case WCharacterFormat _:
      case RowFormat _:
label_2:
        this.EnsureComposites();
        this.IsDefault = false;
        this.ImportMembers(format);
        break;
      default:
        this.CopyProperties(format);
        goto label_2;
    }
  }

  protected virtual void ImportMembers(FormatBase format)
  {
  }

  internal virtual void ApplyBase(FormatBase baseFormat) => this.m_baseFormat = baseFormat;

  public bool HasKey(int key)
  {
    return this.PropertiesHash != null && this.PropertiesHash.ContainsKey(this.GetFullKey(key));
  }

  public bool HasBoolKey(int key)
  {
    return this.PropertiesHash != null && this.PropertiesHash.ContainsKey(key) && (bool) this.PropertiesHash[key];
  }

  public virtual void ClearFormatting()
  {
    if (this.m_propertiesHash != null)
      this.m_propertiesHash.Clear();
    if (this.m_sprms == null)
      return;
    this.m_sprms.Clear();
  }

  protected abstract object GetDefValue(int key);

  protected virtual FormatBase GetDefComposite(int key) => (FormatBase) null;

  protected virtual void OnChange(FormatBase format, int propKey)
  {
    if (this.m_parentFormat == null)
      return;
    this.ParentFormat.OnChange(format, propKey);
  }

  internal virtual bool HasValue(int propertyKey) => false;

  internal virtual int GetSprmOption(int propertyKey) => int.MaxValue;

  internal override void Close()
  {
    base.Close();
    if (this.m_propertiesHash != null)
    {
      this.m_propertiesHash.Clear();
      this.m_propertiesHash = (Dictionary<int, object>) null;
    }
    if (this.m_sprms != null)
    {
      this.m_sprms.Close();
      this.m_sprms = (SinglePropertyModifierArray) null;
    }
    if (this.m_unParsedSprms != null)
    {
      this.m_unParsedSprms.Close();
      this.m_unParsedSprms = (SinglePropertyModifierArray) null;
    }
    if (this.m_baseFormat != null)
      this.m_baseFormat = (FormatBase) null;
    if (this.m_parentFormat != null)
      this.m_parentFormat = (FormatBase) null;
    if (this.m_revisions == null)
      return;
    this.m_revisions.Clear();
    this.m_revisions = (List<Revision>) null;
  }

  protected internal virtual void EnsureComposites()
  {
  }

  protected void EnsureComposites(params int[] keys)
  {
    foreach (int key in keys)
    {
      int fullKey = this.GetFullKey(key);
      FormatBase formatBase = this.PropertiesHash == null || !this.PropertiesHash.ContainsKey(fullKey) ? this.GetDefComposite(key) : this.PropertiesHash[fullKey] as FormatBase;
      formatBase.EnsureComposites();
      formatBase.IsDefault = false;
    }
  }

  protected int GetBaseKey(int key)
  {
    int key1 = (int) this.m_compositeKey;
    if (this.m_parentLevel > (byte) 1)
      key1 = this.m_parentFormat.GetFullKey(key1);
    return key - (key1 << 8);
  }

  protected int GetFullKey(int key)
  {
    if (key > 128 /*0x80*/)
      throw new ArgumentOutOfRangeException(nameof (key));
    int key1 = (int) this.m_compositeKey;
    if (this.m_parentLevel > (byte) 1)
      key1 = this.m_parentFormat.GetFullKey(key1);
    return (key1 << 8) + key;
  }

  protected FormatBase GetDefComposite(int key, FormatBase value)
  {
    int fullKey = this.GetFullKey(key);
    this.PropertiesHash[fullKey] = (object) value;
    if (this.BaseFormat != null && this.BaseFormat.PropertiesHash != null)
    {
      FormatBase baseFormat = !this.BaseFormat.PropertiesHash.ContainsKey(fullKey) ? this.BaseFormat[fullKey] as FormatBase : this.BaseFormat.PropertiesHash[fullKey] as FormatBase;
      value.ApplyBase(baseFormat);
    }
    return value;
  }

  private void MarkNoDefault()
  {
    this.m_bFlags &= (byte) 254;
    if (this.m_parentFormat == null)
      return;
    this.m_parentFormat.IsDefault = false;
  }

  internal virtual void RemoveChanges()
  {
    if (this.m_sprms == null)
      return;
    int changeOption = this.GetChangeOption();
    if (changeOption == 0)
      return;
    SinglePropertyModifierRecord sprm = this.m_sprms[changeOption];
    if (sprm == null)
      return;
    for (int index = this.m_sprms.Modifiers.IndexOf(sprm); index < this.m_sprms.Modifiers.Count; index = index - 1 + 1)
      this.m_sprms.Modifiers.RemoveAt(index);
    if (this.m_propertiesHash == null)
      return;
    this.m_propertiesHash.Clear();
  }

  internal virtual void AcceptChanges()
  {
    if (this.m_sprms == null || this.m_sprms.Length == 0)
      return;
    int changeOption = this.GetChangeOption();
    if (changeOption == 0)
      return;
    SinglePropertyModifierRecord sprm = this.m_sprms.TryGetSprm(changeOption);
    if (sprm == null)
      return;
    int styleChangeOption = this.GetStyleChangeOption();
    if (this.m_sprms.Contain(styleChangeOption))
    {
      int num = this.m_sprms.Modifiers.IndexOf(sprm);
      for (int index = 0; index <= num; ++index)
        this.m_sprms.Modifiers.RemoveAt(0);
      if (this is WParagraphFormat)
      {
        if (this.m_sprms.Contain(17931))
          this.m_sprms.RemoveValue(styleChangeOption);
      }
      else
        this.m_sprms.RemoveValue(styleChangeOption);
    }
    else
    {
      int num = this.m_sprms.Modifiers.IndexOf(sprm) + 1;
      if (num < this.m_sprms.Count)
      {
        List<SinglePropertyModifierRecord> propertyModifierRecordList = new List<SinglePropertyModifierRecord>();
        int sprmIndex = num;
        for (int count = this.m_sprms.Count; sprmIndex < count; ++sprmIndex)
          propertyModifierRecordList.Add(this.m_sprms.GetSprmByIndex(sprmIndex));
        foreach (SinglePropertyModifierRecord modifier in propertyModifierRecordList)
        {
          this.m_sprms.RemoveValue((int) modifier.Options);
          this.m_sprms.Add(modifier);
        }
      }
      this.m_sprms.RemoveValue(changeOption);
    }
    this.PropertiesHash.Clear();
  }

  private int GetStyleChangeOption()
  {
    switch (this)
    {
      case WCharacterFormat _:
        return 18992;
      case WParagraphFormat _:
        return 17920;
      default:
        return 0;
    }
  }

  private int GetChangeOption()
  {
    switch (this)
    {
      case WCharacterFormat _:
        return 10883;
      case WParagraphFormat _:
        return 9828;
      case RowFormat _:
        return 13928;
      default:
        return 0;
    }
  }

  internal virtual void RemovePositioning()
  {
  }

  internal Stream CloneStream(Stream input)
  {
    MemoryStream memoryStream = new MemoryStream();
    byte[] buffer = new byte[input.Length];
    input.Seek(0L, SeekOrigin.Begin);
    int count = input.Read(buffer, 0, buffer.Length);
    if (count > 0)
      memoryStream.Write(buffer, 0, count);
    return (Stream) memoryStream;
  }

  internal bool CompareProperties(FormatBase format)
  {
    bool flag = false;
    if (this.PropertiesHash.Count > 0 || format.PropertiesHash.Count > 0)
    {
      if (this.PropertiesHash.Count == 0 && format.PropertiesHash.Count > 0)
      {
        foreach (KeyValuePair<int, object> keyValuePair in format.PropertiesHash)
          this.PropertiesHash.Add(keyValuePair.Key, keyValuePair.Value);
        foreach (KeyValuePair<int, object> keyValuePair in this.PropertiesHash)
        {
          if (keyValuePair.Value is FormatBase)
          {
            (keyValuePair.Value as FormatBase).SetOwner((OwnerHolder) this);
            if (format is WParagraphFormat && keyValuePair.Value is Borders)
              (keyValuePair.Value as Borders).ApplyBase((FormatBase) (this.BaseFormat as WParagraphFormat).Borders);
            if (format is RowFormat && keyValuePair.Value is Borders)
              (keyValuePair.Value as Borders).ApplyBase((FormatBase) (this.BaseFormat as RowFormat).Borders);
            if (format is RowFormat && keyValuePair.Value is Paddings)
              (keyValuePair.Value as Paddings).ApplyBase((FormatBase) (this.BaseFormat as RowFormat).Paddings);
            if (format is RowFormat && keyValuePair.Value is RowFormat.TablePositioning)
              (keyValuePair.Value as RowFormat.TablePositioning).ApplyBase((FormatBase) (this.BaseFormat as RowFormat).Positioning);
          }
        }
        return false;
      }
      if (this.PropertiesHash.Count > 0 && format.PropertiesHash.Count == 0)
      {
        foreach (KeyValuePair<int, object> keyValuePair in this.PropertiesHash)
          this.OldPropertiesHash.Add(keyValuePair.Key, keyValuePair.Value);
        this.PropertiesHash.Clear();
        return false;
      }
      foreach (KeyValuePair<int, object> keyValuePair in format.PropertiesHash)
      {
        if (this.PropertiesHash.ContainsKey(keyValuePair.Key))
        {
          object format1 = format.PropertiesHash[keyValuePair.Key];
          if (format1 is FormatBase)
          {
            if (!(this.PropertiesHash[keyValuePair.Key] as FormatBase).CompareProperties(format1 as FormatBase))
            {
              flag = true;
              break;
            }
          }
          else if (this.PropertiesHash[keyValuePair.Key] != format1)
            ;
        }
        else
        {
          flag = true;
          break;
        }
      }
    }
    if (!flag)
      return true;
    foreach (KeyValuePair<int, object> keyValuePair in this.PropertiesHash)
      this.OldPropertiesHash.Add(keyValuePair.Key, keyValuePair.Value);
    this.PropertiesHash.Clear();
    foreach (KeyValuePair<int, object> keyValuePair in format.PropertiesHash)
      this.PropertiesHash.Add(keyValuePair.Key, keyValuePair.Value);
    foreach (KeyValuePair<int, object> keyValuePair in this.PropertiesHash)
    {
      if (keyValuePair.Value is FormatBase)
      {
        (keyValuePair.Value as FormatBase).SetOwner((OwnerHolder) this);
        if (format is WParagraphFormat && keyValuePair.Value is Borders)
          (keyValuePair.Value as Borders).ApplyBase((FormatBase) (this.BaseFormat as WParagraphFormat).Borders);
        if (format is RowFormat && keyValuePair.Value is Borders)
          (keyValuePair.Value as Borders).ApplyBase((FormatBase) (this.BaseFormat as RowFormat).Borders);
        if (format is RowFormat && keyValuePair.Value is Paddings)
          (keyValuePair.Value as Paddings).ApplyBase((FormatBase) (this.BaseFormat as RowFormat).Paddings);
        if (format is RowFormat && keyValuePair.Value is RowFormat.TablePositioning)
          (keyValuePair.Value as RowFormat.TablePositioning).ApplyBase((FormatBase) (this.BaseFormat as RowFormat).Positioning);
      }
    }
    return false;
  }

  internal bool Compare(int propertyKey, FormatBase format)
  {
    if (this.PropertiesHash.ContainsKey(propertyKey) && format.PropertiesHash.ContainsKey(propertyKey))
    {
      if (!this.Compare(this.PropertiesHash[propertyKey], format.PropertiesHash[propertyKey]))
        return false;
    }
    else if (!this.PropertiesHash.ContainsKey(propertyKey) && format.PropertiesHash.ContainsKey(propertyKey) || this.PropertiesHash.ContainsKey(propertyKey) && !format.PropertiesHash.ContainsKey(propertyKey))
      return false;
    return true;
  }

  private bool Compare(object value, object currentValue)
  {
    if (value == null && currentValue != null || value != null && currentValue == null)
      return false;
    Type type1 = value.GetType();
    Type type2 = currentValue.GetType();
    if (type1.Name != type2.Name)
      return false;
    switch (type1.Name.ToLower())
    {
      case "single":
        if ((double) (float) value != (double) (float) currentValue)
          return false;
        break;
      case "boolean":
        if ((bool) value != (bool) currentValue)
          return false;
        break;
      case "toggleoperand":
        if ((ToggleOperand) value != (ToggleOperand) currentValue)
          return false;
        break;
      case "string":
        if ((string) value != (string) currentValue)
          return false;
        break;
      case "subsuperscript":
        if ((SubSuperScript) value != (SubSuperScript) currentValue)
          return false;
        break;
      case "numberspacingtype":
        if ((NumberSpacingType) value != (NumberSpacingType) currentValue)
          return false;
        break;
      case "color":
        if (this.GetARGBCode((Color) value) != this.GetARGBCode((Color) currentValue))
          return false;
        break;
      case "texturestyle":
        if ((TextureStyle) value != (TextureStyle) currentValue)
          return false;
        break;
      case "int16":
        if ((int) (short) value != (int) (short) currentValue)
          return false;
        break;
      case "outlinelevel":
        if ((OutlineLevel) value != (OutlineLevel) currentValue)
          return false;
        break;
      case "framewrapmode":
        if ((FrameWrapMode) value != (FrameWrapMode) currentValue)
          return false;
        break;
      case "horizontalalignment":
        if ((HorizontalAlignment) value != (HorizontalAlignment) currentValue)
          return false;
        break;
      case "linespacingrule":
        if ((LineSpacingRule) value != (LineSpacingRule) currentValue)
          return false;
        break;
      case "borderstyle":
        if ((BorderStyle) value != (BorderStyle) currentValue)
          return false;
        break;
      case "underlinestyle":
        if ((UnderlineStyle) value != (UnderlineStyle) currentValue)
          return false;
        break;
    }
    return true;
  }

  private string GetARGBCode(Color color)
  {
    return color.A.ToString("X2") + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
  }

  internal void CopyProperties(FormatBase format)
  {
    int num = 0;
    int parentLevel = 0;
    if (!(format is RowFormat.TablePositioning))
    {
      num = (int) format.m_compositeKey;
      parentLevel = (int) format.m_parentLevel;
      if (parentLevel > 1)
        num = format.m_parentFormat.GetFullKey((int) format.m_compositeKey);
    }
    int[] array1 = new int[this.m_propertiesHash.Count];
    this.m_propertiesHash.Keys.CopyTo(array1, 0);
    for (int index = 0; index < array1.Length; ++index)
    {
      int key = array1[index];
      if (!(this.m_propertiesHash[key] is FormatBase) && (num == 0 || this.GetCompositeParentKey(key, parentLevel) == num))
        this.m_propertiesHash.Remove(key);
    }
    IDictionaryEnumerator enumerator1 = (IDictionaryEnumerator) format.PropertiesHash.GetEnumerator();
    while (enumerator1.MoveNext())
    {
      if (!(enumerator1.Value is FormatBase) && (num == 0 || this.GetCompositeParentKey((int) enumerator1.Key, parentLevel) == num))
        this.m_propertiesHash.Add((int) enumerator1.Key, enumerator1.Value);
    }
    int[] array2 = new int[this.m_oldPropertiesHash.Count];
    this.m_oldPropertiesHash.Keys.CopyTo(array2, 0);
    for (int index = 0; index < array2.Length; ++index)
    {
      int key = array2[index];
      if (!(this.m_oldPropertiesHash[key] is FormatBase) && (num == 0 || this.GetCompositeParentKey(key, parentLevel) == num))
        this.m_oldPropertiesHash.Remove(key);
    }
    IDictionaryEnumerator enumerator2 = (IDictionaryEnumerator) format.OldPropertiesHash.GetEnumerator();
    while (enumerator2.MoveNext())
    {
      if (!(enumerator2.Value is FormatBase) && (!(this.OwnerBase is WTableRow) || !(format.OwnerBase is WTable)) && (num == 0 || this.GetCompositeParentKey((int) enumerator2.Key, parentLevel) == num))
        this.m_oldPropertiesHash.Add((int) enumerator2.Key, enumerator2.Value);
    }
    if (!(this is WListFormat) || !(format is WListFormat))
      return;
    (this as WListFormat).IsListRemoved = (format as WListFormat).IsListRemoved;
    (this as WListFormat).IsEmptyList = (format as WListFormat).IsEmptyList;
  }

  internal void CopyCharacterFormatRevision(FormatBase format)
  {
    if (!(this is WCharacterFormat) || !(format is WCharacterFormat))
      return;
    (this as WCharacterFormat).m_revisions = (format as WCharacterFormat).m_revisions;
  }

  internal void CopyRowFormatRevisions(FormatBase format)
  {
    if (!(this is RowFormat) || !(format is RowFormat))
      return;
    (this as RowFormat).m_revisions = (format as RowFormat).m_revisions;
  }

  internal void CopyCellFormatRevisions(FormatBase format)
  {
    if (!(this is CellFormat) || !(format is CellFormat))
      return;
    (this as CellFormat).m_revisions = (format as CellFormat).m_revisions;
  }

  private int GetCompositeParentKey(int key, int parentLevel)
  {
    if (key > int.MaxValue)
      throw new ArgumentOutOfRangeException("Key should be less than or equal 4294967295");
    int num1 = parentLevel;
    int num2 = (int) Math.Pow(2.0, (double) (num1 * 8)) - 1;
    if (key <= num2)
      return key;
    int num3;
    do
    {
      ++num1;
      num3 = (int) Math.Pow(2.0, (double) (num1 * 8)) - 1;
    }
    while (key > num3);
    return key >> (num1 - parentLevel) * 8;
  }

  internal void UpdateProperties(FormatBase format)
  {
    this.m_propertiesHash = format.PropertiesHash;
  }

  internal void CopyFormat(FormatBase format)
  {
    foreach (KeyValuePair<int, object> keyValuePair in format.PropertiesHash)
    {
      if (!(keyValuePair.Value is Borders) && !(keyValuePair.Value is Border) && !(keyValuePair.Value is Paddings) && !(keyValuePair.Value is RowFormat.TablePositioning))
      {
        if (this.PropertiesHash.ContainsKey(keyValuePair.Key))
          this.PropertiesHash[keyValuePair.Key] = keyValuePair.Value;
        else
          this.PropertiesHash.Add(keyValuePair.Key, keyValuePair.Value);
      }
    }
  }

  private bool CheckCharacterStyle(int key)
  {
    int fullKey = this.GetFullKey(key);
    return this is WCharacterFormat && !this.PropertiesHash.ContainsKey(fullKey) && (this as WCharacterFormat).CharStyle != null && (this as WCharacterFormat).CharStyle.CharacterFormat[key] != null && (this as WCharacterFormat).CharStyle.CharacterFormat.HasValue(key);
  }

  private object GetBaseFormatValue(int key)
  {
    object baseFormatValue = this.BaseFormat[key];
    switch (this)
    {
      case WCharacterFormat _ when (this as WCharacterFormat).TableStyleCharacterFormat != null && !(baseFormatValue is bool):
      case WParagraphFormat _ when (this as WParagraphFormat).TableStyleParagraphFormat != null:
        FormatBase baseFormat = this.BaseFormat;
        for (int fullKey = this.GetFullKey(key); !baseFormat.PropertiesHash.ContainsKey(fullKey); baseFormat = baseFormat.BaseFormat)
        {
          if (baseFormat.BaseFormat == null)
            return this is WCharacterFormat ? (this as WCharacterFormat).TableStyleCharacterFormat[key] : (this as WParagraphFormat).TableStyleParagraphFormat[key];
        }
        if (!(this is WParagraphFormat) || (this as WParagraphFormat).TableStyleParagraphFormat == null || key != 2 && key != 5)
          return baseFormatValue;
        WListFormat wlistFormat1 = (WListFormat) null;
        if ((this as WParagraphFormat).OwnerBase is WParagraph)
          wlistFormat1 = ((this as WParagraphFormat).OwnerBase as WParagraph).ListFormat;
        else if ((this as WParagraphFormat).OwnerBase is WParagraphStyle)
          wlistFormat1 = ((this as WParagraphFormat).OwnerBase as WParagraphStyle).ListFormat;
        return wlistFormat1 != null && wlistFormat1.IsEmptyList ? (object) null : baseFormatValue;
      case WParagraphFormat _ when key == 2 || key == 5:
        WListFormat wlistFormat2 = (WListFormat) null;
        if ((this as WParagraphFormat).OwnerBase is WParagraph)
          wlistFormat2 = ((this as WParagraphFormat).OwnerBase as WParagraph).ListFormat;
        else if ((this as WParagraphFormat).OwnerBase is WParagraphStyle)
          wlistFormat2 = ((this as WParagraphFormat).OwnerBase as WParagraphStyle).ListFormat;
        return wlistFormat2 != null && wlistFormat2.IsEmptyList ? (object) null : baseFormatValue;
      default:
        return baseFormatValue;
    }
  }

  internal DateTime ParseDTTM(int value)
  {
    DateTime dttm = new DateTime();
    if ((value >> 29 & 7) <= 6)
    {
      int minute = value & 63 /*0x3F*/;
      if (minute > 59)
        return dttm;
      int hour = value >> 6 & 31 /*0x1F*/;
      if (hour > 23)
        return dttm;
      int day = value >> 11 & 31 /*0x1F*/;
      if (day > 31 /*0x1F*/ || day == 0)
        return dttm;
      int month = value >> 16 /*0x10*/ & 15;
      if (month > 12)
        return dttm;
      dttm = new DateTime(1900 + (value >> 20 & 511 /*0x01FF*/), month, day, hour, minute, 0);
    }
    return dttm;
  }

  internal int GetDTTMIntValue(DateTime dt)
  {
    if (dt == DateTime.MinValue)
      dt = new DateTime(1900, 1, 1, 0, 0, 0);
    return Convert.ToInt32(string.Empty + Convert.ToString((int) dt.DayOfWeek, 2).PadLeft(3, '0') + Convert.ToString(dt.Year - 1900, 2).PadLeft(9, '0') + Convert.ToString(dt.Month, 2).PadLeft(4, '0') + Convert.ToString(dt.Day, 2).PadLeft(5, '0') + Convert.ToString(dt.Hour, 2).PadLeft(5, '0') + Convert.ToString(dt.Minute, 2).PadLeft(6, '0'), 2);
  }

  internal bool CompareArray(byte[] buffer1, byte[] buffer2)
  {
    bool flag = true;
    for (int index = 0; index < buffer1.Length; ++index)
    {
      if ((int) buffer1[index] != (int) buffer2[index])
      {
        flag = false;
        break;
      }
    }
    return flag;
  }

  internal object GetKeyValue(Dictionary<int, object> propertyHash, int key)
  {
    if (propertyHash.ContainsKey(key))
      return propertyHash[key];
    try
    {
      return this.GetDefValue(key);
    }
    catch (Exception ex)
    {
      return (object) null;
    }
  }
}
