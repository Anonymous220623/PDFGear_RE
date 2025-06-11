// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.ContentControlProperties
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using Syncfusion.Office;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class ContentControlProperties : XDLSSerializableBase
{
  internal const byte AppearanceKey = 0;
  internal const byte ColorKey = 1;
  internal const byte CheckedKey = 2;
  internal const byte CalendarTypeKey = 3;
  internal const byte DisplayFormatKey = 4;
  internal const byte DisplayLocaleKey = 5;
  internal const byte StorageFormatKey = 6;
  internal const byte LockContentControlKey = 7;
  internal const byte LockContentskey = 8;
  internal const byte IsTemporarykey = 9;
  internal const byte MultilineKey = 10;
  internal const byte IsShowingPlaceHolderTextKey = 11;
  internal const byte CheckedStateKey = 12;
  internal const byte UnCheckedStateKey = 13;
  private string m_title = string.Empty;
  private XmlMapping m_xmlMapping;
  private DocPartList m_DocPartList;
  private DocPartObj m_DocPartObj;
  private ContentControlListItems m_sdtListItem;
  private ContentControlType m_contentControlType;
  private string m_Label;
  private WCharacterFormat m_CharacterFormat;
  private uint m_TabIndex;
  private string m_Tag;
  private byte m_bFlags;
  private ContentRepeatingType m_ContentRepeatingType;
  private string m_id;
  private CalendarType m_calendarType;
  private string m_dateFormat;
  private LocaleIDs m_LID;
  private string m_fullDate;
  private string m_placeHolderDocPartId;
  private ContentControlDateStorageFormat m_dateStorage;
  private ContentControlAppearance m_appearance;
  private ushort m_flag;
  private Color m_color;
  private CheckBoxState m_checkedState;
  private CheckBoxState m_unCheckedState;
  private byte m_bFlagA;
  private Entity m_owner;

  public CheckBoxState CheckedState
  {
    get => this.m_checkedState;
    set
    {
      if (this.Document != null && !this.Document.IsOpening && !this.Document.IsCloning && this.Type != ContentControlType.CheckBox)
        throw new Exception("Checked property is available only for CheckBox content control");
      this.m_flag = (ushort) ((int) this.m_flag & 61439 /*0xEFFF*/ | 4096 /*0x1000*/);
      this.m_checkedState = value;
      if (!this.IsChecked || this.Document.IsOpening || this.Document.IsCloning)
        return;
      this.ChangeCheckboxState(this.IsChecked);
    }
  }

  public CheckBoxState UncheckedState
  {
    get => this.m_unCheckedState;
    set
    {
      if (this.Document != null && !this.Document.IsOpening && !this.Document.IsCloning && this.Type != ContentControlType.CheckBox)
        throw new Exception("Checked property is available only for CheckBox content control");
      this.m_unCheckedState = value;
      this.m_flag = (ushort) ((int) this.m_flag & 57343 /*0xDFFF*/ | 8192 /*0x2000*/);
      if (this.IsChecked || this.Document.IsOpening || this.Document.IsCloning)
        return;
      this.ChangeCheckboxState(this.IsChecked);
    }
  }

  internal string ID
  {
    get => this.m_id;
    set => this.m_id = value;
  }

  public string DateDisplayFormat
  {
    get => this.m_dateFormat;
    set
    {
      if (this.Document != null && !this.Document.IsOpening && this.Type != ContentControlType.Date)
        throw new Exception("DateDisplayFormat property is available for Date content control alone");
      this.m_flag = (ushort) ((int) this.m_flag & 65519 | 16 /*0x10*/);
      this.m_dateFormat = value;
    }
  }

  public LocaleIDs DateDisplayLocale
  {
    get => this.m_LID;
    set
    {
      if (this.Document != null && !this.Document.IsOpening && this.Type != ContentControlType.Date)
        throw new Exception("DateDisplayLocale property is available for Date content control alone");
      this.m_flag = (ushort) ((int) this.m_flag & 65503 | 32 /*0x20*/);
      this.m_LID = value;
    }
  }

  internal string FullDate
  {
    get => this.m_fullDate;
    set => this.m_fullDate = value;
  }

  internal string PlaceHolderDocPartId
  {
    get => this.m_placeHolderDocPartId;
    set => this.m_placeHolderDocPartId = value;
  }

  public ContentControlDateStorageFormat DateStorageFormat
  {
    get => this.m_dateStorage;
    set
    {
      if (this.Document != null && !this.Document.IsOpening && this.Type != ContentControlType.Date)
        throw new Exception("DateStorageFormat property is available for Date content control alone");
      this.m_flag = (ushort) ((int) this.m_flag & 65471 | 64 /*0x40*/);
      this.m_dateStorage = value;
    }
  }

  public CalendarType DateCalendarType
  {
    get => this.m_calendarType;
    set
    {
      if (this.Document != null && !this.Document.IsOpening && this.Type != ContentControlType.Date)
        throw new Exception("DateCalendarType property is available for Date content control alone");
      this.m_flag = (ushort) ((int) this.m_flag & 65527 | 8);
      this.m_calendarType = value;
    }
  }

  public bool IsChecked
  {
    get => ((int) this.m_bFlags & 128 /*0x80*/) >> 7 != 0;
    set
    {
      if (this.Document != null && !this.Document.IsOpening && this.Type != ContentControlType.CheckBox)
        throw new Exception("Checked property is available only for CheckBox content control");
      this.m_flag = (ushort) ((int) this.m_flag & 65531 | 4);
      this.m_bFlags = (byte) ((int) this.m_bFlags & (int) sbyte.MaxValue | (value ? 1 : 0) << 7);
      if (this.Document.IsOpening)
        return;
      this.ChangeCheckboxState(value);
    }
  }

  public bool LockContentControl
  {
    get => ((int) this.m_bFlags & 64 /*0x40*/) >> 6 != 0;
    set
    {
      if (value && this.IsTemporary)
        this.IsTemporary = false;
      this.m_flag = (ushort) ((int) this.m_flag & 65407 | 128 /*0x80*/);
      this.m_bFlags = (byte) ((int) this.m_bFlags & 191 | (value ? 1 : 0) << 6);
    }
  }

  public bool LockContents
  {
    get => ((int) this.m_bFlags & 32 /*0x20*/) >> 5 != 0;
    set
    {
      this.m_flag = (ushort) ((int) this.m_flag & 65279 | 256 /*0x0100*/);
      this.m_bFlags = (byte) ((int) this.m_bFlags & 223 | (value ? 1 : 0) << 5);
    }
  }

  public bool Multiline
  {
    get => ((int) this.m_bFlags & 16 /*0x10*/) >> 4 != 0;
    set
    {
      if (this.Document != null && !this.Document.IsOpening && !this.Document.IsCloning && (this.Type == ContentControlType.RichText || this.Type == ContentControlType.RepeatingSection || this.Type == ContentControlType.BuildingBlockGallery))
        throw new Exception($"Multiline property is not available for {this.Type.ToString()} content control");
      this.m_flag = (ushort) ((int) this.m_flag & 64511 | 1024 /*0x0400*/);
      this.m_bFlags = (byte) ((int) this.m_bFlags & 239 | (value ? 1 : 0) << 4);
    }
  }

  public ContentControlAppearance Appearance
  {
    get => this.m_appearance;
    set
    {
      this.m_flag = (ushort) ((int) this.m_flag & 65534 | 1);
      this.m_appearance = value;
    }
  }

  public Color Color
  {
    get => this.m_color;
    set
    {
      this.m_flag = (ushort) ((int) this.m_flag & 65533 | 2);
      this.m_color = value;
    }
  }

  public string Title
  {
    get => this.m_title;
    set => this.m_title = value;
  }

  internal bool Bibliograph
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal bool Citation
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  internal bool Unlocked
  {
    get => ((int) this.m_bFlagA & 2) >> 1 != 0;
    set => this.m_bFlagA = (byte) ((int) this.m_bFlagA & 253 | (value ? 1 : 0) << 1);
  }

  public ContentControlListItems ContentControlListItems
  {
    get
    {
      if (this.Document != null && !this.Document.IsOpening && this.Type != ContentControlType.ComboBox && this.Type != ContentControlType.DropDownList)
        throw new Exception("ContentControlListItems property is available only for ComboBox or DropDownList content controls");
      if (this.m_sdtListItem == null)
        this.m_sdtListItem = new ContentControlListItems();
      return this.m_sdtListItem;
    }
  }

  public XmlMapping XmlMapping
  {
    get
    {
      if (this.m_xmlMapping == null)
        this.m_xmlMapping = new XmlMapping(this.Owner);
      return this.m_xmlMapping;
    }
    internal set => this.m_xmlMapping = value;
  }

  internal Entity Owner => this.m_owner;

  internal DocPartList DocPartList
  {
    get => this.m_DocPartList;
    set => this.m_DocPartList = value;
  }

  internal DocPartObj DocPartObj
  {
    get => this.m_DocPartObj;
    set => this.m_DocPartObj = value;
  }

  public ContentControlType Type
  {
    get => this.m_contentControlType;
    internal set => this.m_contentControlType = value;
  }

  internal string Label
  {
    get => this.m_Label;
    set => this.m_Label = value;
  }

  internal WCharacterFormat CharacterFormat => this.m_CharacterFormat;

  public bool HasPlaceHolderText
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    internal set
    {
      this.m_flag = (ushort) ((int) this.m_flag & 63487 | 2048 /*0x0800*/);
      this.m_bFlags = (byte) ((int) this.m_bFlags & 251 | (value ? 1 : 0) << 2);
    }
  }

  internal uint TabIndex
  {
    get => this.m_TabIndex;
    set => this.m_TabIndex = value;
  }

  public string Tag
  {
    get => this.m_Tag;
    set => this.m_Tag = value;
  }

  public bool IsTemporary
  {
    get => ((int) this.m_bFlags & 8) >> 3 != 0;
    set
    {
      if (value && this.LockContentControl)
        this.LockContentControl = false;
      this.m_flag = (ushort) ((int) this.m_flag & 65023 | 512 /*0x0200*/);
      this.m_bFlags = (byte) ((int) this.m_bFlags & 247 | (value ? 1 : 0) << 3);
    }
  }

  internal ContentRepeatingType ContentRepeatingType
  {
    get => this.m_ContentRepeatingType;
    set => this.m_ContentRepeatingType = value;
  }

  internal ContentControlProperties(WordDocument doc, Entity ownerEntity)
    : base(doc, (Entity) null)
  {
    this.m_CharacterFormat = new WCharacterFormat((IWordDocument) doc);
    this.m_checkedState = new CheckBoxState();
    this.m_checkedState.ContentControlProperties = this;
    this.m_unCheckedState = new CheckBoxState();
    this.m_unCheckedState.ContentControlProperties = this;
    this.m_owner = ownerEntity;
  }

  internal ContentControlProperties(WordDocument doc)
    : base(doc, (Entity) null)
  {
    this.m_CharacterFormat = new WCharacterFormat((IWordDocument) doc);
    this.m_checkedState = new CheckBoxState();
    this.m_checkedState.ContentControlProperties = this;
    this.m_unCheckedState = new CheckBoxState();
    this.m_unCheckedState.ContentControlProperties = this;
  }

  internal void ChangeCheckboxState(bool isChecked)
  {
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    string text;
    string font;
    if (isChecked)
    {
      text = this.m_checkedState.Value;
      font = this.m_checkedState.Font;
    }
    else
    {
      text = this.m_unCheckedState.Value;
      font = this.m_unCheckedState.Font;
    }
    if (this.m_owner is InlineContentControl owner)
    {
      if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(font))
        return;
      if (owner.ParagraphItems.Count != 0 && owner.ParagraphItems[0] is WTextRange)
      {
        (owner.ParagraphItems[0] as WTextRange).Text = text;
        (owner.ParagraphItems[0] as WTextRange).CharacterFormat.FontName = font;
      }
      else
        owner.ParagraphItems.Add((IEntity) new WTextRange((IWordDocument) this.Document)
        {
          Text = text,
          CharacterFormat = {
            FontName = font
          }
        });
    }
    else if (this.Owner is CellContentControl)
    {
      this.InsertParagraph((WTextBody) (this.Owner as CellContentControl).OwnerCell, text, font);
    }
    else
    {
      if (!(this.Owner is BlockContentControl))
        return;
      this.InsertParagraph((this.Owner as BlockContentControl).TextBody, text, font);
    }
  }

  private void InsertParagraph(WTextBody textBody, string text, string fontName)
  {
    if (textBody.ChildEntities.Count > 0 && textBody.ChildEntities[0] is WParagraph)
    {
      bool flag = false;
      WParagraph childEntity1 = textBody.ChildEntities[0] as WParagraph;
      for (int index = 0; index < childEntity1.ChildEntities.Count; ++index)
      {
        if (childEntity1.ChildEntities[index] is WTextRange)
        {
          WTextRange childEntity2 = childEntity1.ChildEntities[index] as WTextRange;
          childEntity2.Text = text;
          childEntity2.CharacterFormat.FontName = fontName;
          flag = true;
          break;
        }
      }
      if (flag)
        return;
      childEntity1.ChildEntities.Insert(0, (IEntity) new WTextRange((IWordDocument) this.Document)
      {
        Text = text,
        CharacterFormat = {
          FontName = fontName
        }
      });
    }
    else
    {
      WParagraph wparagraph = new WParagraph((IWordDocument) this.Document);
      wparagraph.ChildEntities.Insert(0, (IEntity) new WTextRange((IWordDocument) this.Document)
      {
        Text = text,
        CharacterFormat = {
          FontName = fontName
        }
      });
      textBody.ChildEntities.Insert(0, (IEntity) wparagraph);
    }
  }

  internal bool HasKey(int propertyKey)
  {
    return ((int) this.m_flag & (int) (ushort) Math.Pow(2.0, (double) propertyKey)) >> propertyKey != 0;
  }

  internal ContentControlProperties Clone() => (ContentControlProperties) this.CloneImpl();

  protected new object CloneImpl()
  {
    ContentControlProperties controlProperties = (ContentControlProperties) this.MemberwiseClone();
    if (this.m_sdtListItem != null)
    {
      controlProperties.m_sdtListItem = new ContentControlListItems();
      controlProperties.m_sdtListItem = this.m_sdtListItem.Clone();
    }
    if (this.m_CharacterFormat != null)
    {
      controlProperties.m_CharacterFormat = new WCharacterFormat((IWordDocument) this.m_CharacterFormat.Document);
      controlProperties.m_CharacterFormat.ImportContainer((FormatBase) this.CharacterFormat);
      controlProperties.m_CharacterFormat.CopyProperties((FormatBase) this.CharacterFormat);
    }
    return (object) controlProperties;
  }

  internal new void CloneRelationsTo(WordDocument doc, OwnerHolder nextOwner)
  {
    if (this.m_CharacterFormat == null || string.IsNullOrEmpty(this.m_CharacterFormat.CharStyleName))
      return;
    if (this.m_CharacterFormat != null)
      this.m_CharacterFormat.CloneRelationsTo(doc, nextOwner);
    this.m_CharacterFormat.SetOwner((OwnerHolder) doc);
  }

  internal void SetOwnerContentControl(Entity owner) => this.m_owner = owner;

  internal new void Close()
  {
    if (this.m_xmlMapping != null)
      this.m_xmlMapping = (XmlMapping) null;
    if (this.m_DocPartList != null)
      this.m_DocPartList = (DocPartList) null;
    if (this.m_DocPartObj != null)
      this.m_DocPartObj = (DocPartObj) null;
    if (this.m_sdtListItem != null)
    {
      this.m_sdtListItem.Close();
      this.m_sdtListItem = (ContentControlListItems) null;
    }
    if (this.m_CharacterFormat == null)
      return;
    this.m_CharacterFormat.Close();
    this.m_CharacterFormat = (WCharacterFormat) null;
  }
}
