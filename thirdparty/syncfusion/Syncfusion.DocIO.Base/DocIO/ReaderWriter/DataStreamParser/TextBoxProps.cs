// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.TextBoxProps
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser;

[CLSCompliant(false)]
internal class TextBoxProps : BaseProps
{
  private float m_txbxLineWidth;
  private LineDashing m_lineDashing;
  private Color m_fillColor;
  private TextBoxLineStyle m_lineStyle;
  private WrapMode m_wrapMode;
  private float m_txID;
  private bool m_noLine;
  private Color m_lineColor;
  private uint m_leftMargin;
  private uint m_rightMargin;
  private uint m_topMargin;
  private uint m_bottomMargin;
  private byte m_bFlags;

  internal bool NoLine
  {
    get => this.m_noLine;
    set => this.m_noLine = value;
  }

  internal WrapMode WrapText
  {
    get => this.m_wrapMode;
    set => this.m_wrapMode = value;
  }

  internal bool FitShapeToText
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 251 | (value ? 1 : 0) << 2);
  }

  internal TextBoxLineStyle LineStyle
  {
    get => this.m_lineStyle;
    set => this.m_lineStyle = value;
  }

  internal Color FillColor
  {
    get => this.m_fillColor;
    set => this.m_fillColor = value;
  }

  internal Color LineColor
  {
    get => this.m_lineColor;
    set => this.m_lineColor = value;
  }

  internal float TxbxLineWidth
  {
    get => this.m_txbxLineWidth;
    set => this.m_txbxLineWidth = value;
  }

  internal LineDashing LineDashing
  {
    get => this.m_lineDashing;
    set => this.m_lineDashing = value;
  }

  internal float TXID
  {
    get => this.m_txID;
    set => this.m_txID = value;
  }

  internal uint LeftMargin
  {
    get => this.m_leftMargin;
    set => this.m_leftMargin = value;
  }

  internal uint RightMargin
  {
    get => this.m_rightMargin;
    set => this.m_rightMargin = value;
  }

  internal uint TopMargin
  {
    get => this.m_topMargin;
    set => this.m_topMargin = value;
  }

  internal uint BottomMargin
  {
    get => this.m_bottomMargin;
    set => this.m_bottomMargin = value;
  }

  internal TextBoxProps()
  {
    this.m_fillColor = Color.White;
    this.m_lineColor = Color.Black;
    this.m_txbxLineWidth = 0.75f;
    this.RelHrzPos = HorizontalOrigin.Column;
    this.RelVrtPos = VerticalOrigin.Paragraph;
    this.m_lineStyle = TextBoxLineStyle.Simple;
    this.m_lineDashing = LineDashing.Solid;
    this.m_wrapMode = WrapMode.None;
    this.m_leftMargin = uint.MaxValue;
    this.m_rightMargin = uint.MaxValue;
    this.m_topMargin = uint.MaxValue;
    this.m_bottomMargin = uint.MaxValue;
  }
}
