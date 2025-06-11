// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.TextBodyItem
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public abstract class TextBodyItem : WidgetBase, ITextBodyItem, IEntity
{
  public WTextBody OwnerTextBody => this.Owner as WTextBody;

  public bool IsInsertRevision => this.CheckInsertRev();

  public bool IsDeleteRevision => this.CheckDeleteRev();

  internal bool IsChangedCFormat => this.CheckChangedCFormat();

  internal bool IsChangedPFormat => this.CheckChangedPFormat();

  internal TextBodyItem NextTextBodyItem => this.GetNextTextBodyItemValue();

  public TextBodyItem(WordDocument doc)
    : base(doc, (Entity) null)
  {
  }

  public abstract TextSelection Find(Regex pattern);

  public abstract int Replace(Regex pattern, string replace);

  public abstract int Replace(string given, string replace, bool caseSensitive, bool wholeWord);

  public abstract int Replace(Regex pattern, TextSelection textSelection);

  public abstract int Replace(Regex pattern, TextSelection textSelection, bool saveFormatting);

  internal abstract TextSelectionList FindAll(Regex pattern);

  internal abstract TextBodyItem GetNextTextBodyItemValue();

  internal abstract void MakeChanges(bool acceptChanges);

  internal abstract bool CheckInsertRev();

  internal abstract bool CheckDeleteRev();

  internal abstract bool CheckChangedCFormat();

  internal abstract bool CheckChangedPFormat();

  internal abstract void AcceptCChanges();

  internal abstract void AcceptPChanges();

  internal abstract void RemoveCFormatChanges();

  internal abstract void RemovePFormatChanges();

  internal abstract bool HasTrackedChanges();

  internal abstract void SetChangedCFormat(bool check);

  internal abstract void SetChangedPFormat(bool check);

  internal abstract void SetDeleteRev(bool check);

  internal abstract void SetInsertRev(bool check);

  protected TextBodyItem GetNextInSection(WSection section)
  {
    if (section == null)
      return (TextBodyItem) null;
    return section.NextSibling is WSection nextSibling && nextSibling.Body.Items.Count > 0 ? nextSibling.Body.Items[0] : (TextBodyItem) null;
  }
}
