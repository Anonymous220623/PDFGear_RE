// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.TagDirectory
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Graphics;

internal class TagDirectory
{
  private string m_name;
  private TagDirectory m_parent;

  internal string Name => this.m_name;

  internal TagDirectory Parent
  {
    get => this.m_parent;
    set => this.m_parent = value;
  }

  internal TagDirectory(string name) => this.m_name = name;
}
