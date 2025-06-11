// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WFormField
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public abstract class WFormField : WField
{
  internal const int DEF_VALUE = 25;
  protected FormFieldType m_curFormFieldType;
  private short m_params;
  private string m_title;
  private string m_help;
  private string m_tooltip;
  private string m_macroOnStart;
  private string m_macroOnEnd;
  private bool m_bHasFFData = true;

  public FormFieldType FormFieldType => this.m_curFormFieldType;

  public string Name
  {
    get => this.m_title;
    set
    {
      this.ApplyNewBookmarkName(this.m_title, value);
      this.m_title = value;
    }
  }

  public string Help
  {
    get => this.m_help;
    set
    {
      this.m_help = value;
      this.m_params = (short) BaseWordRecord.SetBitsByMask((int) this.m_params, 128 /*0x80*/, 7, 1);
    }
  }

  public string StatusBarHelp
  {
    get => this.m_tooltip;
    set
    {
      this.m_tooltip = value;
      this.m_params = (short) BaseWordRecord.SetBitsByMask((int) this.m_params, 256 /*0x0100*/, 8, 1);
    }
  }

  public string MacroOnStart
  {
    get => this.m_macroOnStart;
    set => this.m_macroOnStart = value;
  }

  public string MacroOnEnd
  {
    get => this.m_macroOnEnd;
    set => this.m_macroOnEnd = value;
  }

  internal int Value
  {
    get => ((int) this.m_params & 124) >> 2;
    set => this.m_params = (short) ((int) this.m_params & -125 | value << 2);
  }

  internal int Params
  {
    get => (int) this.m_params;
    set => this.m_params = (short) value;
  }

  public bool Enabled
  {
    get => ((int) this.m_params & 512 /*0x0200*/) == 0;
    set
    {
      this.m_params = (short) BaseWordRecord.SetBitsByMask((int) this.m_params, 512 /*0x0200*/, 9, value ? 0 : 1);
    }
  }

  public bool CalculateOnExit
  {
    get => ((int) this.m_params & 16384 /*0x4000*/) == 16384 /*0x4000*/;
    set
    {
      this.m_params = value ? (short) BaseWordRecord.SetBitsByMask((int) this.m_params, 16384 /*0x4000*/, 14, 1) : (short) BaseWordRecord.SetBitsByMask((int) this.m_params, 16384 /*0x4000*/, 14, 0);
    }
  }

  internal bool HasFFData
  {
    get => this.m_bHasFFData;
    set => this.m_bHasFFData = value;
  }

  public WFormField(IWordDocument doc)
    : base(doc)
  {
    this.m_paraItemType = ParagraphItemType.FormField;
    this.m_title = string.Empty;
    this.m_help = string.Empty;
    this.m_tooltip = string.Empty;
    this.m_macroOnStart = string.Empty;
    this.m_macroOnEnd = string.Empty;
  }

  protected WFormField(WFormField formField, IWordDocument doc)
    : this(doc)
  {
    this.Help = formField.Help;
    this.MacroOnEnd = formField.MacroOnEnd;
    this.MacroOnStart = formField.MacroOnStart;
    this.Params = formField.Params;
    this.Name = formField.Name;
    this.StatusBarHelp = formField.StatusBarHelp;
    this.Value = formField.Value;
    this.FieldType = formField.FieldType;
  }

  protected override object CloneImpl()
  {
    WFormField wformField = (WFormField) base.CloneImpl();
    wformField.CharacterFormat.ImportContainer((FormatBase) this.CharacterFormat);
    return (object) wformField;
  }

  protected override void CreateLayoutInfo() => base.CreateLayoutInfo();

  internal override void AttachToParagraph(WParagraph paragraph, int itemPos)
  {
    base.AttachToParagraph(paragraph, itemPos);
    if (this.Document.IsOpening)
      return;
    this.AttachForTextBody(paragraph.Owner as WTextBody);
  }

  internal override void Detach()
  {
    base.Detach();
    this.DetachForTextBody(this.OwnerParagraph.Owner as WTextBody);
  }

  private void AttachForTextBody(WTextBody textBody)
  {
    if (textBody == null || !textBody.IsFormFieldsCreated)
      return;
    textBody.FormFields.Add(this);
    if (!(textBody is WTableCell) || !(textBody.Owner.Owner is WTable owner))
      return;
    this.AttachForTextBody(owner.Owner as WTextBody);
  }

  private void DetachForTextBody(WTextBody textBody)
  {
    if (textBody == null)
      return;
    if (textBody.IsFormFieldsCreated)
      textBody.FormFields.Remove(this);
    if (!(textBody is WTableCell))
      return;
    this.DetachForTextBody(textBody.Owner.Owner.Owner as WTextBody);
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    if (reader.HasAttribute("Params"))
      this.Params = reader.ReadInt("Params");
    if (reader.HasAttribute("Title"))
      this.m_title = reader.ReadString("Title");
    if (reader.HasAttribute("Help"))
      this.m_help = reader.ReadString("Help");
    if (reader.HasAttribute("Tooltip"))
      this.m_tooltip = reader.ReadString("Tooltip");
    if (reader.HasAttribute("MacroOnStart"))
      this.m_macroOnStart = reader.ReadString("MacroOnStart");
    if (!reader.HasAttribute("MacroOnEnd"))
      return;
    this.m_macroOnEnd = reader.ReadString("MacroOnEnd");
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    writer.WriteValue("Params", (int) this.m_params);
    writer.WriteValue("Title", this.m_title);
    writer.WriteValue("Help", this.m_help);
    writer.WriteValue("Tooltip", this.m_tooltip);
    writer.WriteValue("MacroOnStart", this.m_macroOnStart);
    writer.WriteValue("MacroOnEnd", this.m_macroOnEnd);
  }

  private void ApplyNewBookmarkName(string oldName, string newName)
  {
    if (this.Document != null && this.Document.IsOpening || !(this.Owner is WParagraph))
      return;
    this.CheckFormFieldName(newName);
    bool flag = false;
    if (this.Document != null)
      flag = this.ApplyInDocBkmkColl(oldName, newName);
    if (!flag)
      this.ApplyInOwnerParaColl(oldName, newName);
    if (this.OwnerParagraph.OwnerTextBody == null || !this.OwnerParagraph.OwnerTextBody.IsFormFieldsCreated)
      return;
    this.OwnerParagraph.OwnerTextBody.FormFields.CorrectName(oldName, newName);
  }

  private bool ApplyInDocBkmkColl(string oldName, string newName)
  {
    bool flag = false;
    BookmarkCollection bookmarks = this.Document.Bookmarks;
    if (bookmarks.Count > 0)
    {
      Bookmark bookmark = bookmarks[oldName];
      if (bookmark != null && bookmark.BookmarkStart != null && bookmark.BookmarkEnd != null)
      {
        bookmark.BookmarkStart.SetName(newName);
        bookmark.BookmarkEnd.SetName(newName);
        flag = true;
      }
    }
    return flag;
  }

  private void ApplyInOwnerParaColl(string oldName, string newName)
  {
    BookmarkStart bookmarkStart = (BookmarkStart) null;
    BookmarkEnd bookmarkEnd = (BookmarkEnd) null;
    foreach (IParagraphItem paragraphItem in (CollectionImpl) this.OwnerParagraph.Items)
    {
      if (paragraphItem is BookmarkStart && (paragraphItem as BookmarkStart).Name == oldName)
        bookmarkStart = paragraphItem as BookmarkStart;
      else if (paragraphItem is BookmarkEnd && (paragraphItem as BookmarkEnd).Name == oldName)
      {
        bookmarkEnd = paragraphItem as BookmarkEnd;
        if (bookmarkStart != null)
          break;
      }
    }
    if (bookmarkStart == null || bookmarkEnd == null)
      return;
    bookmarkStart.SetName(newName);
    bookmarkEnd.SetName(newName);
  }

  private void CheckFormFieldName(string newName)
  {
    Bookmark bookmark = this.Document.Bookmarks[newName];
    if (bookmark == null)
      return;
    this.Document.Bookmarks.Remove(bookmark);
    foreach (WSection section in (CollectionImpl) this.Document.Sections)
    {
      if (section.Body.FormFields != null)
      {
        WFormField formField = section.Body.FormFields[newName];
        if (formField != null)
          formField.Name = string.Empty;
      }
    }
  }
}
