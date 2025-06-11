// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.EmbeddedTextType
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class EmbeddedTextType
{
  private int m_position;
  private string m_content;

  internal int Position
  {
    get => this.m_position;
    set => this.m_position = value;
  }

  internal string Content
  {
    get => this.m_content;
    set => this.m_content = value;
  }
}
