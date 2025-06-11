// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.ScriptFeature
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class ScriptFeature
{
  private string m_name;
  private bool m_isComplete;
  private int m_mask;

  internal string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }

  internal bool IsComplete
  {
    get => this.m_isComplete;
    set => this.m_isComplete = value;
  }

  internal int Mask
  {
    get => this.m_mask;
    set => this.m_mask = value;
  }

  internal ScriptFeature(string name, bool complete, int mask)
  {
    this.Name = name;
    this.IsComplete = complete;
    this.Mask = mask;
  }
}
