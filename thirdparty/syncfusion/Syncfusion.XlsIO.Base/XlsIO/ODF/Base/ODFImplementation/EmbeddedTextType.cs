// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODF.Base.ODFImplementation.EmbeddedTextType
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.ODF.Base.ODFImplementation;

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
