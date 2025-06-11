// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.LayoutedStringWidget
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal class LayoutedStringWidget
{
  private RectangleF m_bounds;
  private string m_text;
  private Font m_font;
  private bool m_isStretchable;

  internal LayoutedStringWidget()
  {
  }

  internal LayoutedStringWidget(LayoutedStringWidget srcWidget)
  {
    this.Bounds = srcWidget.Bounds;
    this.Text = srcWidget.Text;
    this.Font = srcWidget.Font;
    this.IsStretchable = srcWidget.IsStretchable;
  }

  internal RectangleF Bounds
  {
    get => this.m_bounds;
    set => this.m_bounds = value;
  }

  internal string Text
  {
    get => this.m_text;
    set => this.m_text = value;
  }

  internal Font Font
  {
    get => this.m_font;
    set => this.m_font = value;
  }

  internal bool IsStretchable
  {
    get => this.m_isStretchable;
    set => this.m_isStretchable = value;
  }

  public void ShiftXYPosition(float xPosition, float yPosition)
  {
    this.Bounds = new RectangleF(this.Bounds.X + xPosition, this.Bounds.Y + yPosition, this.Bounds.Width, this.Bounds.Height);
  }

  public void Dispose()
  {
    this.m_text = (string) null;
    if (this.m_font == null)
      return;
    this.m_font.Dispose();
    this.m_font = (Font) null;
  }
}
