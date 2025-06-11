// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.HeaderFooter
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class HeaderFooter : WTextBody
{
  private HeaderFooterType m_type;
  private byte m_bFlags;
  private Watermark m_watermark;

  public override EntityType EntityType => EntityType.HeaderFooter;

  internal HeaderFooterType Type
  {
    get => this.m_type;
    set => this.m_type = value;
  }

  internal bool WriteWatermark
  {
    get => !this.LinkToPrevious && ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal Watermark Watermark
  {
    get
    {
      if (this.m_watermark == null)
        this.m_watermark = new Watermark(WatermarkType.NoWatermark);
      return this.m_watermark;
    }
    set
    {
      this.m_watermark = value;
      if (this.m_watermark == null)
        return;
      this.WriteWatermark = true;
      this.m_watermark.SetOwner((OwnerHolder) this);
      if (this.m_watermark is PictureWatermark)
        (this.m_watermark as PictureWatermark).UpdateImage();
      else
        (this.m_watermark as TextWatermark).SetDefaultSize();
    }
  }

  internal Watermark InsertWatermark(WatermarkType type)
  {
    switch (type)
    {
      case WatermarkType.PictureWatermark:
        this.m_watermark = (Watermark) new PictureWatermark(this.Document);
        break;
      case WatermarkType.TextWatermark:
        this.m_watermark = (Watermark) new TextWatermark(this.Document);
        break;
      default:
        this.m_watermark = new Watermark(type);
        break;
    }
    return this.m_watermark;
  }

  public bool LinkToPrevious
  {
    get => this.GetLinkToPreviousValue();
    set
    {
      if (this.LinkToPrevious == value)
        return;
      this.UpdateLinkToPrevious(value);
    }
  }

  internal bool CheckWriteWatermark() => this.WriteWatermark;

  internal HeaderFooter(WSection sec, HeaderFooterType type)
    : base(sec)
  {
    this.m_type = type;
  }

  private bool GetLinkToPreviousValue()
  {
    return this.OwnerBase is WSection ownerBase && ownerBase.Index > 0 && this.Items.Count == 0;
  }

  private void UpdateLinkToPrevious(bool linkToPrevious)
  {
    if (linkToPrevious)
    {
      this.ChildEntities.Clear();
      (this.OwnerBase as WSection).HeadersFooters[this.m_type] = new HeaderFooter(this.OwnerBase as WSection, this.m_type);
    }
    else
    {
      WSection sourceSection = this.FindSourceSection();
      if (sourceSection != null)
      {
        foreach (HeaderFooter headersFooter in sourceSection.HeadersFooters)
        {
          if (this.m_type == headersFooter.m_type && this.CheckShapes(headersFooter))
            headersFooter.m_bodyItems.CloneTo((EntityCollection) this.m_bodyItems);
        }
      }
      if (this.ChildEntities.Count != 0)
        return;
      this.AddParagraph();
    }
  }

  private WSection FindSourceSection()
  {
    WSection previousSibling = (this.OwnerBase as WSection).PreviousSibling as WSection;
    while (previousSibling != null && previousSibling.HeadersFooters.LinkToPrevious)
      previousSibling = previousSibling.PreviousSibling as WSection;
    return previousSibling;
  }

  private bool CheckShapes(HeaderFooter hf)
  {
    foreach (WParagraph paragraph in (IEnumerable) hf.Paragraphs)
    {
      foreach (ParagraphItem paragraphItem in (CollectionImpl) paragraph.Items)
      {
        switch (paragraphItem)
        {
          case WPicture _:
          case WTextBox _:
            paragraphItem.IsCloned = true;
            continue;
          case ShapeObject _:
            return false;
          default:
            continue;
        }
      }
    }
    return true;
  }
}
