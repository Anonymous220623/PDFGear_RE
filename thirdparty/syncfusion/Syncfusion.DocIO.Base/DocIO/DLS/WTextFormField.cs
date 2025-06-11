// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WTextFormField
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using Syncfusion.DocIO.Rendering;
using Syncfusion.Layouting;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WTextFormField : WFormField, ILeafWidget, IWidget
{
  internal const string DEF_TEXT = "     ";
  private TextFormFieldType m_formFieldType;
  private string m_defText;
  private int m_maxLength;
  private string m_strTextFormat;
  private WTextRange m_text;
  private short m_iFieldSeparator;

  public override EntityType EntityType => EntityType.TextFormField;

  public TextFormFieldType Type
  {
    get => this.m_formFieldType;
    set
    {
      this.m_formFieldType = value;
      if (this.m_formFieldType == TextFormFieldType.CurrentDateText)
      {
        this.AppendDateTimeField(FieldType.FieldDate);
      }
      else
      {
        if (this.m_formFieldType != TextFormFieldType.CurrentTimeText)
          return;
        this.AppendDateTimeField(FieldType.FieldTime);
      }
    }
  }

  public string StringFormat
  {
    get => this.m_strTextFormat;
    set => this.m_strTextFormat = value;
  }

  public string DefaultText
  {
    get => this.m_defText;
    set => this.m_defText = value;
  }

  public int MaximumLength
  {
    get => this.m_maxLength;
    set
    {
      if (!this.m_doc.IsOpening && value < this.m_defText.Length && value != 0 && this.Type != TextFormFieldType.Calculation)
        throw new ArgumentOutOfRangeException("MaximumLength is lower than current text length");
      this.m_maxLength = value;
    }
  }

  public WTextRange TextRange
  {
    get
    {
      this.GetTextRangeValue();
      return this.GetFirstTextRange() ?? this.m_text;
    }
    set
    {
      this.m_text = value;
      this.SetTextRangeValue(this.m_text);
    }
  }

  public override string Text
  {
    get
    {
      this.GetTextRangeValue();
      return this.m_text.Text;
    }
    set
    {
      WTextRange firstTextRange = this.GetFirstTextRange();
      if (firstTextRange != null)
      {
        firstTextRange.Text = value;
        this.SetTextRangeValue(firstTextRange);
      }
      else
      {
        this.m_text.Text = value;
        this.SetTextRangeValue(this.m_text);
      }
    }
  }

  public WTextFormField(IWordDocument doc)
    : base(doc)
  {
    this.m_curFormFieldType = FormFieldType.TextInput;
    this.m_paraItemType = ParagraphItemType.TextFormField;
    this.FieldType = FieldType.FieldFormTextInput;
    this.Params = 128 /*0x80*/;
    this.m_defText = string.Empty;
    this.m_text = new WTextRange(doc);
    this.m_strTextFormat = string.Empty;
  }

  internal override void Close()
  {
    if (this.m_text != null)
    {
      this.m_text.Close();
      this.m_text = (WTextRange) null;
    }
    if (this.m_defText != null)
      this.m_defText = (string) null;
    if (this.m_strTextFormat != null)
      this.m_strTextFormat = (string) null;
    base.Close();
  }

  protected override object CloneImpl()
  {
    WTextFormField owner = (WTextFormField) base.CloneImpl();
    owner.m_text = (WTextRange) this.m_text.Clone();
    owner.m_text.SetOwner((OwnerHolder) owner);
    return (object) owner;
  }

  private WTextRange GetFirstTextRange()
  {
    bool flag = false;
    for (int index = 0; index < this.Range.Count; ++index)
    {
      ParagraphItem firstTextRange = this.Range.Items[index] as ParagraphItem;
      if (flag && firstTextRange is WTextRange)
        return firstTextRange as WTextRange;
      if (!flag && firstTextRange is WFieldMark && (firstTextRange as WFieldMark).Type == FieldMarkType.FieldSeparator && this.FieldSeparator == firstTextRange)
        flag = true;
    }
    return (WTextRange) null;
  }

  private void GetTextRangeValue()
  {
    if (this.Range.Count < 1)
      return;
    string str = string.Empty;
    for (int index = 0; index < this.Range.Count; ++index)
      str = !(this.Range.Items[index] is ParagraphItem) ? str + this.UpdateTextBodyItemText(this.Range.Items[index] as Entity) : str + this.UpdateParagraphItemText(this.Range.Items[index] as Entity);
    this.m_text.Text = str;
  }

  private string UpdateTextBodyItemText(Entity entity)
  {
    string empty = string.Empty;
    switch (entity)
    {
      case WParagraph _:
        for (int index = 0; index < (entity as WParagraph).Items.Count; ++index)
        {
          empty += this.UpdateParagraphItemText((Entity) (entity as WParagraph).Items[index]);
          if (this.m_iFieldSeparator == (short) 0)
            return empty;
        }
        break;
      case WTable _:
        empty += this.UpdateTextForTable(entity);
        break;
    }
    return empty;
  }

  private string UpdateTextForTable(Entity entity)
  {
    string empty = string.Empty;
    for (int index1 = 0; index1 < (entity as WTable).Rows.Count; ++index1)
    {
      WTableRow row = (entity as WTable).Rows[index1];
      for (int index2 = 0; index2 < row.Cells.Count; ++index2)
      {
        WTableCell cell = row.Cells[index2];
        for (int index3 = 0; index3 < cell.Items.Count; ++index3)
        {
          empty += this.UpdateTextBodyItemText((Entity) cell.Items[index3]);
          if (this.m_iFieldSeparator == (short) 0)
            return empty;
        }
      }
    }
    return empty;
  }

  private string UpdateParagraphItemText(Entity entity)
  {
    string str = string.Empty;
    if (this.m_iFieldSeparator > (short) 0 && entity is WTextRange)
    {
      str = (entity as WTextRange).Text;
    }
    else
    {
      switch (entity)
      {
        case WFieldMark _ when (entity as WFieldMark).Type == FieldMarkType.FieldSeparator && this.FieldSeparator == entity:
          ++this.m_iFieldSeparator;
          break;
        case WFieldMark _ when (entity as WFieldMark).Type == FieldMarkType.FieldEnd && this.FieldEnd == entity:
          --this.m_iFieldSeparator;
          break;
      }
    }
    return str;
  }

  private void SetTextRangeValue(WTextRange textRange)
  {
    this.RemovePreviousText();
    for (int inOwnerCollection = this.GetIndexInOwnerCollection(); inOwnerCollection < this.OwnerParagraph.Items.Count; ++inOwnerCollection)
    {
      ParagraphItem paragraphItem = this.OwnerParagraph.Items[inOwnerCollection];
      if (paragraphItem is WFieldMark && (paragraphItem as WFieldMark).Type == FieldMarkType.FieldSeparator && this.FieldSeparator == paragraphItem)
      {
        this.OwnerParagraph.Items.Insert(inOwnerCollection + 1, (IEntity) textRange.Clone());
        break;
      }
    }
    this.IsFieldRangeUpdated = false;
  }

  private void RemovePreviousText()
  {
    this.m_iFieldSeparator = (short) 0;
    for (int index = 0; index < this.Range.Count; ++index)
    {
      int count = this.Range.Count;
      if (this.Range.Items[index] is ParagraphItem)
        this.RemoveParagraphItem(this.Range.Items[index] as Entity);
      else
        this.RemoveTextBodyItem(this.Range.Items[index] as Entity);
      if (this.Range.Count < count)
        index -= count - this.Range.Count;
    }
  }

  private void RemoveTextBodyItem(Entity entity)
  {
    switch (entity)
    {
      case WParagraph _:
        for (int index = 0; index < (entity as WParagraph).Items.Count; ++index)
        {
          int count = (entity as WParagraph).Items.Count;
          this.RemoveParagraphItem((Entity) (entity as WParagraph).Items[index]);
          if (this.m_iFieldSeparator == (short) 0)
          {
            this.InsertParagraphItems(entity as WParagraph);
            (entity.Owner as WTextBody).Items.Remove((IEntity) entity);
            break;
          }
          if ((entity as WParagraph).Items.Count < count)
            index -= count - (entity as WParagraph).Items.Count;
        }
        if (this.m_iFieldSeparator < (short) 1 || (entity as WParagraph).Items.Count != 0)
          break;
        (entity.Owner as WTextBody).Items.Remove((IEntity) entity);
        break;
      case WTable _:
        (entity.Owner as WTextBody).Items.Remove((IEntity) entity);
        break;
    }
  }

  private void InsertParagraphItems(WParagraph paragraph)
  {
    for (int inOwnerCollection = this.GetIndexInOwnerCollection(); inOwnerCollection < this.OwnerParagraph.Items.Count; ++inOwnerCollection)
    {
      ParagraphItem paragraphItem = this.OwnerParagraph.Items[inOwnerCollection];
      if (paragraphItem is WFieldMark && (paragraphItem as WFieldMark).Type == FieldMarkType.FieldSeparator)
      {
        int num = 1;
        while (paragraph.Items.Count != 0)
        {
          this.OwnerParagraph.Items.Insert(inOwnerCollection + num, (IEntity) paragraph.Items[0]);
          ++num;
        }
        break;
      }
    }
  }

  private void RemoveParagraphItem(Entity entity)
  {
    switch (entity)
    {
      case WTextRange _:
        if (this.m_iFieldSeparator < (short) 1)
          break;
        (entity as ParagraphItem).OwnerParagraph.Items.Remove((IEntity) entity);
        break;
      case WFieldMark _ when (entity as WFieldMark).Type == FieldMarkType.FieldSeparator:
        ++this.m_iFieldSeparator;
        if (this.m_iFieldSeparator <= (short) 1)
          break;
        (entity as ParagraphItem).OwnerParagraph.Items.Remove((IEntity) entity);
        break;
      case WFieldMark _ when (entity as WFieldMark).Type == FieldMarkType.FieldEnd:
        --this.m_iFieldSeparator;
        if (this.m_iFieldSeparator < (short) 1)
          break;
        (entity as ParagraphItem).OwnerParagraph.Items.Remove((IEntity) entity);
        break;
    }
  }

  private void AppendDateTimeField(FieldType fieldType)
  {
    if (this.FieldSeparator == null || this.FieldSeparator.ParentField != this)
      return;
    int inOwnerCollection = this.FieldSeparator.GetIndexInOwnerCollection();
    WField wfield = new WField((IWordDocument) this.Document);
    wfield.FieldType = fieldType;
    this.OwnerParagraph.ChildEntities.Insert(inOwnerCollection, (IEntity) wfield);
    int index1 = inOwnerCollection + 1;
    WTextRange wtextRange1 = new WTextRange((IWordDocument) this.Document);
    this.OwnerParagraph.ChildEntities.Insert(index1, (IEntity) wtextRange1);
    int index2 = index1 + 1;
    WFieldMark wfieldMark1 = new WFieldMark((IWordDocument) this.m_doc, FieldMarkType.FieldSeparator);
    this.OwnerParagraph.ChildEntities.Insert(index2, (IEntity) wfieldMark1);
    int index3 = index2 + 1;
    WTextRange wtextRange2 = new WTextRange((IWordDocument) this.Document);
    this.OwnerParagraph.ChildEntities.Insert(index3, (IEntity) wtextRange2);
    int index4 = index3 + 1;
    wfield.FieldSeparator = wfieldMark1;
    WFieldMark wfieldMark2 = new WFieldMark((IWordDocument) this.m_doc, FieldMarkType.FieldEnd);
    this.OwnerParagraph.ChildEntities.Insert(index4, (IEntity) wfieldMark2);
    wfield.FieldEnd = wfieldMark2;
    string empty = string.Empty;
    DateTime currentDateTime = WordDocument.DisableDateTimeUpdating ? DateTime.MaxValue : DateTime.Now;
    string text = string.IsNullOrEmpty(this.FormattingString) ? (fieldType == FieldType.FieldDate ? currentDateTime.ToString("d") : currentDateTime.ToString("t")) : this.UpdateDateField(this.FormattingString, currentDateTime);
    wfield.UpdateFieldResult(text);
    this.UpdateFieldResult(text);
  }

  internal void SetTextFormFieldType(TextFormFieldType txtFormFieldType)
  {
    this.m_formFieldType = txtFormFieldType;
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    if (reader.HasAttribute("MaxLength"))
      this.m_maxLength = reader.ReadInt("MaxLength");
    if (reader.HasAttribute("DefaultText"))
      this.m_defText = reader.ReadString("DefaultText");
    if (reader.HasAttribute("StringTextFormat"))
      this.m_strTextFormat = reader.ReadString("StringTextFormat");
    if (!reader.HasAttribute("TextType"))
      return;
    this.m_formFieldType = (TextFormFieldType) reader.ReadEnum("TextType", typeof (TextFormFieldType));
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    writer.WriteValue("MaxLength", this.m_maxLength);
    writer.WriteValue("DefaultText", this.m_defText);
    writer.WriteValue("StringTextFormat", this.m_strTextFormat);
    writer.WriteValue("TextType", (int) this.m_formFieldType);
  }

  protected override void InitXDLSHolder()
  {
    this.XDLSHolder.AddElement("text-range", (object) this.m_text);
  }

  SizeF ILeafWidget.Measure(DrawingContext dc) => new SizeF();

  protected override void CreateLayoutInfo() => base.CreateLayoutInfo();

  internal override void InitLayoutInfo(Entity entity, ref bool isLastTOCEntry)
  {
    this.m_layoutInfo = (ILayoutInfo) null;
    if (this != entity)
      return;
    isLastTOCEntry = true;
  }

  ILayoutInfo IWidget.LayoutInfo
  {
    get
    {
      if (this.m_layoutInfo == null)
        this.CreateLayoutInfo();
      return this.m_layoutInfo;
    }
  }

  void IWidget.InitLayoutInfo() => this.m_layoutInfo = (ILayoutInfo) null;

  void IWidget.InitLayoutInfo(IWidget widget)
  {
  }
}
