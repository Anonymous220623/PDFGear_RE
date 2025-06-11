// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WCommentFormat
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using System;
using System.Collections;
using System.Globalization;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WCommentFormat : XDLSSerializableBase
{
  private string m_strUser = "";
  private string m_strUserInitials = "";
  private int m_iBookmarkStartOffset = -1;
  private int m_iBookmarkEndOffset = -1;
  private string m_iTagBkmk = "";
  private int m_iPosition;
  private DateTime date;

  public DateTime DateTime
  {
    get => this.date;
    set => this.date = value;
  }

  public string UserInitials
  {
    get => this.m_strUserInitials;
    set
    {
      this.m_strUserInitials = value.Length <= 9 ? value : throw new ArgumentOutOfRangeException(nameof (UserInitials), "Users initials length must be less than 10 symbols.");
    }
  }

  public string User
  {
    get => this.m_strUser;
    set => this.m_strUser = value;
  }

  internal int BookmarkStartOffset
  {
    get => this.m_iBookmarkStartOffset;
    set => this.m_iBookmarkStartOffset = value;
  }

  internal int BookmarkEndOffset
  {
    get => this.m_iBookmarkEndOffset;
    set => this.m_iBookmarkEndOffset = value;
  }

  internal string TagBkmk
  {
    get => this.m_iTagBkmk;
    set => this.m_iTagBkmk = value;
  }

  internal int Position
  {
    get => this.m_iPosition;
    set => this.m_iPosition = value;
  }

  internal int StartTextPos => this.m_iPosition - this.m_iBookmarkStartOffset;

  public WCommentFormat()
    : base((WordDocument) null, (Entity) null)
  {
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    if (this.m_strUserInitials != "")
      writer.WriteValue("UserInitials", this.m_strUserInitials);
    if (this.m_strUser != "")
      writer.WriteValue("User", this.m_strUser);
    if (this.m_iBookmarkStartOffset != -1)
      writer.WriteValue("BookmarkStartPos", this.m_iBookmarkStartOffset);
    if (this.m_iBookmarkEndOffset != -1)
      writer.WriteValue("BookmarkEndPos", this.m_iBookmarkEndOffset);
    if (!(this.m_iTagBkmk != ""))
      return;
    writer.WriteValue("TagBkmk", this.m_iTagBkmk);
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    if (reader.HasAttribute("User"))
      this.m_strUser = reader.ReadString("User");
    if (reader.HasAttribute("UserInitials"))
      this.m_strUserInitials = reader.ReadString("UserInitials");
    if (reader.HasAttribute("BookmarkStartPos"))
      this.m_iBookmarkStartOffset = reader.ReadInt("BookmarkStartPos");
    if (reader.HasAttribute("BookmarkEndPos"))
      this.m_iBookmarkEndOffset = reader.ReadInt("BookmarkEndPos");
    if (!reader.HasAttribute("TagBkmk"))
      return;
    this.m_iTagBkmk = reader.ReadInt("TagBkmk").ToString();
  }

  public WCommentFormat Clone(IWordDocument doc)
  {
    WCommentFormat wcommentFormat = new WCommentFormat();
    wcommentFormat.m_strUserInitials = this.m_strUserInitials;
    wcommentFormat.m_strUser = this.m_strUser;
    wcommentFormat.m_iBookmarkEndOffset = this.m_iBookmarkEndOffset;
    wcommentFormat.m_iBookmarkStartOffset = this.m_iBookmarkStartOffset;
    if (doc == this.Document || this.FindTagBkmk(doc, this.m_iTagBkmk))
    {
      int result = 0;
      int.TryParse(this.m_iTagBkmk, NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result);
      wcommentFormat.m_iTagBkmk = Convert.ToString(TagIdRandomizer.GetId(result));
    }
    else
      wcommentFormat.m_iTagBkmk = this.m_iTagBkmk;
    return wcommentFormat;
  }

  private bool FindTagBkmk(IWordDocument doc, string tagBkmk)
  {
    foreach (WSection section in (CollectionImpl) doc.Sections)
    {
      if (this.IsCommentExists(section.Body, tagBkmk))
        return true;
    }
    int result = 0;
    int.TryParse(this.m_iTagBkmk, NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result);
    return TagIdRandomizer.ChangedIds.ContainsKey(result);
  }

  private bool IsCommentExists(WTextBody body, string tagBkmk)
  {
    foreach (WParagraph paragraph in (IEnumerable) body.Paragraphs)
    {
      foreach (IParagraphItem paragraphItem in (CollectionImpl) paragraph.Items)
      {
        if (paragraphItem is WComment wcomment && wcomment.Format.TagBkmk == tagBkmk)
          return true;
      }
    }
    foreach (WTable table in (IEnumerable) body.Tables)
    {
      foreach (WTableRow row in (CollectionImpl) table.Rows)
      {
        foreach (WTextBody cell in (CollectionImpl) row.Cells)
        {
          if (this.IsCommentExists(cell, tagBkmk))
            return true;
        }
      }
    }
    return false;
  }

  internal void UpdateTagBkmk()
  {
    this.m_iTagBkmk = Convert.ToString(TagIdRandomizer.Instance.Next());
  }
}
