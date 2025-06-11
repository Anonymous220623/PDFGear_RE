// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.AlternateChunk
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.Layouting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class AlternateChunk : TextBodyItem
{
  private string m_targetId;
  private string m_contentPath;
  private string m_contentType;
  private bool m_isParaItem;
  private ImportOptions m_importOption = ImportOptions.UseDestinationStyles;
  private List<Entity> altChunkBookmarks;
  private List<Entity> bkmkCollection = new List<Entity>();

  internal string TargetId
  {
    get => this.m_targetId;
    set => this.m_targetId = value;
  }

  internal string ContentExtension
  {
    get
    {
      return this.m_contentPath != null ? Path.GetExtension(this.m_contentPath).Replace(".", "") : string.Empty;
    }
  }

  internal ImportOptions ImportOption
  {
    get => this.m_importOption;
    set => this.m_importOption = value;
  }

  internal string ContentType
  {
    get => this.m_contentType;
    set => this.m_contentType = value;
  }

  internal string ContentPath
  {
    get => this.m_contentPath;
    set => this.m_contentPath = value;
  }

  internal Stream Stream
  {
    get
    {
      Part part = this.Document.DocxPackage.FindPart("word/" + this.m_contentPath);
      return part != null ? part.DataStream : this.Document.DocxPackage.FindPart(this.m_contentPath).DataStream;
    }
  }

  internal List<Entity> AltChunkBookmarks
  {
    get
    {
      if (this.altChunkBookmarks == null)
        this.altChunkBookmarks = new List<Entity>();
      return this.altChunkBookmarks;
    }
  }

  internal bool IsParagraphItem
  {
    get => this.m_isParaItem;
    set => this.m_isParaItem = value;
  }

  internal AlternateChunk(WordDocument doc)
    : base(doc)
  {
  }

  internal AlternateChunk Clone() => (AlternateChunk) this.CloneImpl();

  protected override object CloneImpl() => (object) (AlternateChunk) base.CloneImpl();

  internal override TextBodyItem GetNextTextBodyItemValue()
  {
    if (this.NextSibling != null)
      return this.NextSibling as TextBodyItem;
    if (this.Owner is WTableCell)
      return (this.Owner as WTableCell).GetNextTextBodyItem();
    if (this.Owner is WTextBody)
    {
      if (this.OwnerTextBody.Owner is WTextBox)
        return (this.OwnerTextBody.Owner as WTextBox).GetNextTextBodyItem();
      if (this.OwnerTextBody.Owner is WSection)
        return this.GetNextInSection(this.OwnerTextBody.Owner as WSection);
    }
    return (TextBodyItem) null;
  }

  internal bool GetFormatType(string extension)
  {
    switch (extension.ToLower())
    {
      case "doc":
      case "dot":
      case "docx":
      case "dotx":
      case "docm":
      case "dotm":
      case "rtf":
      case "txt":
      case "dat":
      case "htm":
      case "html":
      case "xml":
        return true;
      default:
        return false;
    }
  }

  internal void Update()
  {
    int index1 = 0;
    WSection section1 = (WSection) null;
    Entity ownerTextBody1 = this.GetOwnerTextBody((Entity) this);
    if (ownerTextBody1 is WSection)
    {
      section1 = ownerTextBody1 as WSection;
      index1 = section1.GetIndexInOwnerCollection();
    }
    WordDocument wordDocument;
    try
    {
      wordDocument = new WordDocument();
      wordDocument.m_AltChunkOwner = this.Document;
      FormatType formatType1 = FormatType.Docx;
      formatType1 = FormatType.Doc;
      FormatType formatType2 = FormatType.Automatic;
      wordDocument.Open(this.Stream, formatType2, XHTMLValidationType.None);
      wordDocument.UpdateAlternateChunks();
    }
    catch
    {
      return;
    }
    bool importStyles = this.Document.ImportStyles;
    if ((this.ImportOption & ImportOptions.KeepSourceFormatting) != (ImportOptions) 0)
      this.Document.ImportOptions = ImportOptions.KeepSourceFormatting;
    else
      this.Document.ImportStyles = false;
    int inOwnerCollection = this.GetIndexInOwnerCollection();
    WTextBody ownerTextBody2 = this.OwnerTextBody;
    ownerTextBody2.ChildEntities.Remove((IEntity) this);
    int count = ownerTextBody2.ChildEntities.Count;
    WParagraph lastParagraph = wordDocument.LastParagraph;
    if (lastParagraph != null)
      lastParagraph.IsLastItem = true;
    if (wordDocument.Sections.Count == 1)
    {
      WSection section2 = wordDocument.Sections[0];
      WTextBody body = section2.Body;
      switch (ownerTextBody1)
      {
        case HeaderFooter _:
        case WFootnote _:
        case WComment _:
label_14:
          for (int index2 = 0; index2 < body.ChildEntities.Count; ++index2)
          {
            if (index2 == 0 && this.IsParagraphItem && body.ChildEntities[0] is WParagraph)
              this.MergeAltChunkFirstParagraph(ref inOwnerCollection, ownerTextBody2, body);
            else if (index2 == 0 && this.IsParagraphItem && body.ChildEntities[0] is WTable)
              this.MergeAltChunkFirstTable(ref inOwnerCollection, ownerTextBody2, body);
            else
              ownerTextBody2.ChildEntities.Insert(inOwnerCollection + index2, (IEntity) body.ChildEntities[index2].Clone());
          }
          break;
        default:
          if (inOwnerCollection == 0 && section1 != null && ownerTextBody2.Owner is WSection)
          {
            this.UpdateHeaderFooter(section2, ownerTextBody2, section1);
            goto label_14;
          }
          goto label_14;
      }
    }
    else if (wordDocument.Sections.Count > 1)
    {
      int index3 = 0;
      int num1 = inOwnerCollection;
      int num2 = 0;
      for (int index4 = 0; index4 < wordDocument.Sections.Count; ++index4)
      {
        switch (ownerTextBody1)
        {
          case HeaderFooter _:
          case WFootnote _:
          case WComment _:
            WSection section3 = wordDocument.Sections[index4];
            if (index4 > 0)
              num1 += num2;
            for (int index5 = 0; index5 < section3.Body.ChildEntities.Count; ++index5)
            {
              Entity entity = section3.Body.ChildEntities[index5].Clone();
              ownerTextBody2.ChildEntities.Insert(num1 + index5, (IEntity) entity);
            }
            num2 = section3.Body.ChildEntities.Count;
            break;
          default:
            if (index4 == 0)
            {
              WSection section4 = wordDocument.Sections[0];
              WTextBody body = section4.Body;
              if (section1 != null)
                this.UpdateHeaderFooter(section4, ownerTextBody2, section1);
              index3 = inOwnerCollection + body.ChildEntities.Count;
              for (int index6 = 0; index6 < body.ChildEntities.Count; ++index6)
              {
                if (index6 == 0 && this.IsParagraphItem)
                  this.MergeAltChunkFirstParagraph(ref inOwnerCollection, ownerTextBody2, body);
                else
                  ownerTextBody2.ChildEntities.Insert(inOwnerCollection + index6, (IEntity) body.ChildEntities[index6].Clone());
              }
              break;
            }
            this.Document.Sections.Insert(index1 + index4, (IEntity) wordDocument.Sections[index4].Clone());
            if (index4 == wordDocument.Sections.Count - 1)
            {
              WSection section5 = this.Document.Sections[index1];
              WSection section6 = this.Document.Sections[index1 + index4];
              while (section5.Body.ChildEntities.Count > index3)
                section6.Body.ChildEntities.Add((IEntity) section5.Body.ChildEntities[index3]);
              break;
            }
            break;
        }
      }
    }
    this.UpdateBookmarks(ownerTextBody2, count);
    this.Document.ImportOptions = ImportOptions.UseDestinationStyles;
    this.Document.ImportStyles = importStyles;
    wordDocument.Close();
  }

  private void MergeAltChunkFirstTable(
    ref int altchunkIndex,
    WTextBody ownerTextBody,
    WTextBody altChunkTextBody)
  {
    WTableCell childEntity = ((altChunkTextBody.ChildEntities[0] as WTable).ChildEntities[0] as WTableRow).ChildEntities[0] as WTableCell;
    WParagraph wparagraph = ownerTextBody.ChildEntities[altchunkIndex - 1].Clone() as WParagraph;
    if (childEntity.ChildEntities[0] is WParagraph && wparagraph != null)
    {
      for (int index = 0; index < wparagraph.ChildEntities.Count; ++index)
      {
        (childEntity.ChildEntities[0] as WParagraph).ChildEntities.Insert(index, (IEntity) wparagraph.ChildEntities[index]);
        (ownerTextBody.ChildEntities[altchunkIndex - 1] as WParagraph).ChildEntities[index].RemoveSelf();
      }
    }
    ownerTextBody.ChildEntities.Insert(altchunkIndex - 1, (IEntity) altChunkTextBody.ChildEntities[0].Clone());
    --altchunkIndex;
  }

  private void MergeAltChunkFirstParagraph(
    ref int altchunkIndex,
    WTextBody ownerTextBody,
    WTextBody altChunkTextBody)
  {
    WParagraph wparagraph1 = altChunkTextBody.ChildEntities[0].Clone() as WParagraph;
    WParagraph wparagraph2 = ownerTextBody.ChildEntities[altchunkIndex - 1].Clone() as WParagraph;
    (ownerTextBody.ChildEntities[altchunkIndex - 1] as WParagraph).ImportStyle(wparagraph1.ParaStyle);
    wparagraph1.ParagraphFormat.UpdateSourceFormatting((ownerTextBody.ChildEntities[altchunkIndex - 1] as WParagraph).ParagraphFormat);
    wparagraph1.BreakCharacterFormat.UpdateSourceFormatting((ownerTextBody.ChildEntities[altchunkIndex - 1] as WParagraph).BreakCharacterFormat);
    for (int index = 0; index < wparagraph1.ChildEntities.Count; index = index - 1 + 1)
      (ownerTextBody.ChildEntities[altchunkIndex - 1] as WParagraph).ChildEntities.Add((IEntity) wparagraph1.ChildEntities[index]);
    wparagraph2.ClearItems();
    ownerTextBody.ChildEntities.Insert(altchunkIndex, (IEntity) wparagraph2);
    --altchunkIndex;
  }

  private void UpdateBookmarks(WTextBody body, int oldChildCount)
  {
    if (this.altChunkBookmarks != null && this.altChunkBookmarks.Count > 0)
    {
      foreach (Entity altChunkBookmark in this.altChunkBookmarks)
      {
        switch (altChunkBookmark)
        {
          case BookmarkStart _:
            this.bkmkCollection.Add(altChunkBookmark);
            continue;
          case BookmarkEnd _:
            if (!(altChunkBookmark as BookmarkEnd).IsAfterParagraphMark)
            {
              using (List<Entity>.Enumerator enumerator = this.bkmkCollection.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  Entity current = enumerator.Current;
                  if (current is BookmarkStart && (current as BookmarkStart).Name == (altChunkBookmark as BookmarkEnd).Name)
                  {
                    this.bkmkCollection.Remove(current);
                    break;
                  }
                }
                continue;
              }
            }
            break;
        }
        this.bkmkCollection.Add(altChunkBookmark);
      }
      this.altChunkBookmarks.Clear();
    }
    if (this.bkmkCollection.Count <= 0)
      return;
    bool bkmkStartInserted = false;
    bool bkmkEndInserted = false;
    using (List<Entity>.Enumerator enumerator = this.bkmkCollection.GetEnumerator())
    {
label_29:
      while (enumerator.MoveNext())
      {
        Entity current = enumerator.Current;
        switch (current)
        {
          case BookmarkStart _:
            for (int index = this.Index; index < body.ChildEntities.Count; ++index)
            {
              this.InsertBkmkStart(body.ChildEntities[index], current as BookmarkStart, ref bkmkStartInserted);
              if (bkmkStartInserted)
                break;
            }
            continue;
          case BookmarkEnd _:
            (current as BookmarkEnd).IsAfterParagraphMark = false;
            break;
        }
        int index1 = body.ChildEntities.Count - oldChildCount + (this.Index - 1);
        while (true)
        {
          if (index1 < body.ChildEntities.Count && index1 >= 0)
          {
            this.InsertBkmkEnd(body.ChildEntities[index1], current as BookmarkEnd, ref bkmkEndInserted);
            if (!bkmkEndInserted)
              --index1;
            else
              goto label_29;
          }
          else
            goto label_29;
        }
      }
    }
    this.bkmkCollection.Clear();
  }

  private void InsertBkmkStart(Entity item, BookmarkStart bookmark, ref bool bkmkStartInserted)
  {
    switch (item)
    {
      case WParagraph _:
        WParagraph wparagraph = item as WParagraph;
        bookmark.Index = 0;
        wparagraph.Items.Insert(0, (IEntity) bookmark);
        bkmkStartInserted = true;
        break;
      case WTable _:
        IEnumerator enumerator = (item as WTable).Rows[0].Cells[0].ChildEntities.GetEnumerator();
        try
        {
          while (enumerator.MoveNext())
            this.InsertBkmkStart((Entity) enumerator.Current, bookmark, ref bkmkStartInserted);
          break;
        }
        finally
        {
          if (enumerator is IDisposable disposable)
            disposable.Dispose();
        }
    }
  }

  private void InsertBkmkEnd(Entity item, BookmarkEnd bookmark, ref bool bkmkEndInserted)
  {
    switch (item)
    {
      case WParagraph _:
        (item as WParagraph).Items.Add((IEntity) bookmark);
        bkmkEndInserted = true;
        break;
      case WTable _:
        this.InsertBkmkEnd((item as WTable).LastCell.LastParagraph as Entity, bookmark, ref bkmkEndInserted);
        break;
    }
  }

  private void UpdateHeaderFooter(
    WSection altChunkFirstSection,
    WTextBody ownerTextBody,
    WSection section)
  {
    WTextBody wtextBody = (WTextBody) null;
    if (altChunkFirstSection.HeadersFooters.Header != null)
      wtextBody = (WTextBody) altChunkFirstSection.HeadersFooters.Header;
    if (wtextBody != null && wtextBody.ChildEntities.Count > 0)
    {
      if (section != null && section.PreviousHeaderCount != (short) 0)
      {
        while (section.HeadersFooters.Header.ChildEntities.Count > (int) section.PreviousHeaderCount)
          section.HeadersFooters.Header.ChildEntities.RemoveAt((int) section.PreviousHeaderCount);
        section.PreviousHeaderCount = (short) 0;
      }
      else
        section.PreviousHeaderCount = (short) section.HeadersFooters.Header.ChildEntities.Count;
      if (section.HeadersFooters.Header.ChildEntities.Count > 0)
        section.HeadersFooters.Header.AddParagraph();
      for (int index = 0; index < wtextBody.ChildEntities.Count; ++index)
      {
        Entity entity = wtextBody.ChildEntities[index].Clone();
        section.HeadersFooters.Header.ChildEntities.Add((IEntity) entity);
      }
    }
    if (altChunkFirstSection.HeadersFooters.Footer != null)
      wtextBody = (WTextBody) altChunkFirstSection.HeadersFooters.Footer;
    if (wtextBody == null || wtextBody.ChildEntities.Count <= 0)
      return;
    if (section != null && section.PreviousFooterCount != (short) 0)
    {
      while (section.HeadersFooters.Footer.ChildEntities.Count > (int) section.PreviousFooterCount)
        section.HeadersFooters.Footer.ChildEntities.RemoveAt((int) section.PreviousFooterCount);
      section.PreviousFooterCount = (short) 0;
    }
    else
      section.PreviousFooterCount = (short) section.HeadersFooters.Footer.ChildEntities.Count;
    if (section.HeadersFooters.Footer.ChildEntities.Count > 0)
      section.HeadersFooters.Footer.AddParagraph();
    for (int index = 0; index < wtextBody.ChildEntities.Count; ++index)
    {
      Entity entity = wtextBody.ChildEntities[index].Clone();
      section.HeadersFooters.Footer.ChildEntities.Add((IEntity) entity);
    }
  }

  internal override bool CheckDeleteRev() => false;

  internal override void SetChangedPFormat(bool check)
  {
  }

  internal override void SetChangedCFormat(bool check)
  {
  }

  internal override void SetDeleteRev(bool check)
  {
  }

  internal override void SetInsertRev(bool check)
  {
  }

  internal override bool HasTrackedChanges() => false;

  public override int Replace(Regex pattern, string replace) => 1;

  public override int Replace(string given, string replace, bool caseSensitive, bool wholeWord)
  {
    return 0;
  }

  public override int Replace(Regex pattern, TextSelection textSelection) => 0;

  public override int Replace(Regex pattern, TextSelection textSelection, bool saveFormatting) => 0;

  public int Replace(
    string given,
    TextSelection textSelection,
    bool caseSensitive,
    bool wholeWord)
  {
    return 0;
  }

  public int Replace(
    string given,
    TextSelection textSelection,
    bool caseSensitive,
    bool wholeWord,
    bool saveFormatting)
  {
    return 0;
  }

  internal int ReplaceFirst(string given, string replace, bool caseSensitive, bool wholeWord) => 0;

  internal int ReplaceFirst(Regex pattern, string replace) => 0;

  protected override void CreateLayoutInfo() => this.m_layoutInfo = (ILayoutInfo) new LayoutInfo();

  internal override void InitLayoutInfo(Entity entity, ref bool isLastTOCEntry)
  {
    this.m_layoutInfo = (ILayoutInfo) null;
    if (this != entity)
      return;
    isLastTOCEntry = true;
  }

  internal override void RemoveCFormatChanges()
  {
  }

  internal override void RemovePFormatChanges()
  {
  }

  internal override void AcceptCChanges()
  {
  }

  internal override void AcceptPChanges()
  {
  }

  internal override bool CheckChangedCFormat() => false;

  internal override bool CheckInsertRev() => false;

  public override TextSelection Find(Regex pattern) => (TextSelection) null;

  public TextSelection Find(string given, bool caseSensitive, bool wholeWord)
  {
    return (TextSelection) null;
  }

  internal override void MakeChanges(bool acceptChanges)
  {
  }

  internal override TextSelectionList FindAll(Regex pattern) => (TextSelectionList) null;

  internal override void Close()
  {
    this.m_targetId = (string) null;
    this.m_contentPath = (string) null;
    this.m_contentType = (string) null;
    base.Close();
  }

  internal override bool CheckChangedPFormat() => false;

  public override EntityType EntityType => EntityType.AlternateChunk;

  public EntityCollection ChildEntities => (EntityCollection) null;

  internal override void CloneRelationsTo(WordDocument doc, OwnerHolder nextOwner)
  {
    if (doc.DocxPackage == null && this.Document.DocxPackage != null)
      doc.DocxPackage = this.Document.DocxPackage.Clone();
    else if (doc.DocxPackage != null && this.Document.DocxPackage != null)
      this.UpdateXmlParts(doc);
    this.TargetId = "AltChunkId" + nextOwner.Document.AlternateChunkCount.ToString();
    base.CloneRelationsTo(doc, nextOwner);
  }

  private void UpdateXmlParts(WordDocument destination)
  {
    string[] parts = this.ContentPath.Replace("word/", "").Split('/');
    string newValue = this.UpdateXmlPartContainer(this.Document.DocxPackage.FindPartContainer("word/"), destination.DocxPackage.FindPartContainer("word/"), parts, 0);
    if (!(newValue != string.Empty))
      return;
    this.ContentPath = this.ContentPath.Replace(parts[parts.Length - 1], newValue);
  }

  private string UpdateXmlPartContainer(
    PartContainer srcContainer,
    PartContainer destContainer,
    string[] parts,
    int index)
  {
    string str = string.Empty;
    for (int index1 = index; index1 < parts.Length; ++index1)
    {
      if (index1 < parts.Length - 1)
      {
        string key = parts[index1] + "/";
        if (destContainer.XmlPartContainers.ContainsKey(key))
        {
          destContainer = destContainer.XmlPartContainers[key];
          srcContainer = srcContainer.XmlPartContainers[key];
          str = this.UpdateXmlPartContainer(srcContainer, destContainer, parts, index1 + 1);
          break;
        }
        PartContainer partContainer = srcContainer.XmlPartContainers[key].Clone();
        destContainer.XmlPartContainers.Add(key, partContainer);
        break;
      }
      str = this.UpdateXmlPart(srcContainer, destContainer, parts[index1]);
    }
    return str;
  }

  private string UpdateXmlPart(
    PartContainer srcContainer,
    PartContainer destContainer,
    string xmlPart)
  {
    string key = string.Empty;
    if (destContainer.XmlParts.ContainsKey(xmlPart))
    {
      string extension = Path.GetExtension(xmlPart);
      string str = xmlPart.Replace(extension, "");
      string oldValue = str.TrimEnd('0', '1', '2', '3', '4', '5', '6', '7', '8', '9');
      int result = 0;
      int.TryParse(str.Replace(oldValue, ""), out result);
      for (key = oldValue + result.ToString() + extension; destContainer.XmlParts.ContainsKey(key); key = oldValue + result.ToString() + extension)
        ++result;
      Part part = srcContainer.XmlParts[xmlPart].Clone();
      part.Name = key;
      destContainer.XmlParts.Add(key, part);
    }
    else
      destContainer.XmlParts.Add(xmlPart, srcContainer.XmlParts[xmlPart].Clone());
    return key;
  }
}
