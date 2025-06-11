// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.FormFieldCollection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class FormFieldCollection : CollectionImpl
{
  private Dictionary<string, WFormField> m_dictionary = new Dictionary<string, WFormField>();

  public WFormField this[int index] => (WFormField) this.InnerList[index];

  public WFormField this[string formFieldName] => this.GetByName(formFieldName);

  internal Dictionary<string, WFormField> FormFieldDictonary => this.m_dictionary;

  internal FormFieldCollection(WTextBody textBody)
    : base(textBody.Document, (OwnerHolder) textBody)
  {
    this.Populate(textBody);
  }

  public bool ContainsName(string itemName) => this.m_dictionary.ContainsKey(itemName);

  internal void CorrectName(string oldName, string newName)
  {
    WFormField wformField = this.m_dictionary[oldName];
    this.m_dictionary.Remove(oldName);
    this.m_dictionary.Add(newName, wformField);
    if (!(this.OwnerBase is WTableCell ownerBase) || ownerBase.OwnerRow == null || ownerBase.OwnerRow.OwnerTable == null)
      return;
    WTextBody ownerTextBody = ownerBase.OwnerRow.OwnerTable.OwnerTextBody;
    if (ownerTextBody == null || !ownerTextBody.IsFormFieldsCreated)
      return;
    ownerTextBody.FormFields.CorrectName(oldName, newName);
  }

  internal void Add(WFormField ff)
  {
    if (!this.InnerList.Contains((object) ff))
      this.InnerList.Add((object) ff);
    if (ff.Name == null || !(ff.Name != string.Empty) || this.m_dictionary.ContainsKey(ff.Name))
      return;
    this.m_dictionary.Add(ff.Name, ff);
  }

  internal void Remove(WFormField ff)
  {
    this.InnerList.Remove((object) ff);
    if (ff.Name == null || !(ff.Name != string.Empty) || !this.m_dictionary.ContainsKey(ff.Name))
      return;
    this.m_dictionary.Remove(ff.Name);
    if (this.m_doc.IsDeletingBookmarkContent)
      return;
    Bookmark byName = this.m_doc.Bookmarks.FindByName(ff.Name);
    if (byName == null)
      return;
    this.m_doc.Bookmarks.Remove(byName);
  }

  private void Populate(WTextBody textBody)
  {
    foreach (TextBodyItem childEntity in (CollectionImpl) textBody.ChildEntities)
    {
      switch (childEntity.EntityType)
      {
        case EntityType.Paragraph:
          this.PopulateFromParagraph((WParagraph) childEntity);
          continue;
        case EntityType.Table:
          this.PopulateFromTable((WTable) childEntity);
          continue;
        default:
          continue;
      }
    }
  }

  private void PopulateFromParagraph(WParagraph para)
  {
    foreach (ParagraphItem ff in (CollectionImpl) para.Items)
    {
      if (ff.EntityType == EntityType.TextFormField || ff.EntityType == EntityType.CheckBox || ff.EntityType == EntityType.DropDownFormField)
        this.Add((WFormField) ff);
      switch (ff.EntityType)
      {
        case EntityType.Comment:
          this.Populate((ff as WComment).TextBody);
          continue;
        case EntityType.Footnote:
          this.Populate((ff as WFootnote).TextBody);
          continue;
        case EntityType.TextBox:
          this.Populate((ff as WTextBox).TextBoxBody);
          continue;
        default:
          continue;
      }
    }
  }

  private void PopulateFromTable(WTable table)
  {
    foreach (WTableRow row in (CollectionImpl) table.Rows)
    {
      foreach (WTextBody cell in (CollectionImpl) row.Cells)
        this.Populate(cell);
    }
  }

  private WFormField GetByName(string formFieldName)
  {
    return this.m_dictionary.ContainsKey(formFieldName) ? this.m_dictionary[formFieldName] : (WFormField) null;
  }

  internal override void Close()
  {
    if (this.m_dictionary != null)
    {
      this.m_dictionary.Clear();
      this.m_dictionary = (Dictionary<string, WFormField>) null;
    }
    base.Close();
  }
}
