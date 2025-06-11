// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.RowContentControl
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class RowContentControl : IRowContentControl
{
  private ContentControlProperties m_controlProperties;
  private WCharacterFormat m_BreakCharacterFormat;
  private WTableRow m_ownerRow;

  public ContentControlProperties ContentControlProperties
  {
    get => this.m_controlProperties;
    set => this.m_controlProperties = value;
  }

  public WCharacterFormat BreakCharacterFormat
  {
    get => this.m_BreakCharacterFormat;
    set => this.m_BreakCharacterFormat = value;
  }

  internal WTableRow OwnerRow
  {
    get => this.m_ownerRow;
    set => this.m_ownerRow = value;
  }

  public RowContentControl(WordDocument document)
  {
    this.m_controlProperties = new ContentControlProperties(document);
    this.m_BreakCharacterFormat = new WCharacterFormat((IWordDocument) document);
  }

  internal void Close()
  {
    if (this.m_controlProperties != null)
    {
      this.m_controlProperties.Close();
      this.m_controlProperties = (ContentControlProperties) null;
    }
    if (this.m_BreakCharacterFormat == null)
      return;
    this.m_BreakCharacterFormat.Close();
    this.m_BreakCharacterFormat = (WCharacterFormat) null;
  }
}
