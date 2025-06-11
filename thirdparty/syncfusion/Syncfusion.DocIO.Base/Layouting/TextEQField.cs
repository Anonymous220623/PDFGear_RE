// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.TextEQField
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal class TextEQField : LayoutedEQFields
{
  private string m_text;
  private Font m_font;

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
}
