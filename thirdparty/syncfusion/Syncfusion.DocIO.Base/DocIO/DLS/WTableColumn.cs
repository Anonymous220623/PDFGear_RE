// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WTableColumn
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class WTableColumn
{
  private float m_preferredwidth;
  internal float m_endOffset;
  private ColumnSizeInfo sizeInfo;
  private bool m_hasMaximumWordWidth;

  internal WTableColumn() => this.sizeInfo = new ColumnSizeInfo();

  internal float PreferredWidth
  {
    get => this.m_preferredwidth;
    set => this.m_preferredwidth = value;
  }

  internal float EndOffset
  {
    get => this.m_endOffset;
    set => this.m_endOffset = value;
  }

  internal float MinimumWordWidth
  {
    get => this.sizeInfo.MinimumWordWidth;
    set => this.sizeInfo.MinimumWordWidth = value;
  }

  internal float MaximumWordWidth
  {
    get => this.sizeInfo.MaximumWordWidth;
    set => this.sizeInfo.MaximumWordWidth = value;
  }

  internal float MinimumWidth
  {
    get => this.sizeInfo.MinimumWidth;
    set => this.sizeInfo.MinimumWidth = value;
  }

  internal bool HasMaximumWordWidth
  {
    get => this.m_hasMaximumWordWidth;
    set => this.m_hasMaximumWordWidth = value;
  }

  internal float MaxParaWidth
  {
    get => this.sizeInfo.MaxParaWidth;
    set => this.sizeInfo.MaxParaWidth = value;
  }

  internal WTableColumn Clone()
  {
    return new WTableColumn()
    {
      PreferredWidth = this.PreferredWidth,
      MinimumWidth = this.MinimumWidth,
      MinimumWordWidth = this.MinimumWordWidth,
      MaximumWordWidth = this.MaximumWordWidth
    };
  }

  internal void Dispose() => this.sizeInfo = (ColumnSizeInfo) null;
}
