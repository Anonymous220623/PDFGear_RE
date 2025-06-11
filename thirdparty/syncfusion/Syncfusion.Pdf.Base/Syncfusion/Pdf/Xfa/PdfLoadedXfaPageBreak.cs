// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfLoadedXfaPageBreak
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

internal class PdfLoadedXfaPageBreak
{
  private BreakBefore m_beforeBreak;
  private BreakAfter m_afterBreak;
  private OverFlow m_overflow;
  private string m_beforeTargetID = string.Empty;
  private string m_afterTargetID = string.Empty;
  private bool m_isStartNew;

  internal bool IsStartNew
  {
    get => this.m_isStartNew;
    set => this.m_isStartNew = value;
  }

  internal string AfterTargetID
  {
    get => this.m_afterTargetID;
    set => this.m_afterTargetID = value;
  }

  internal string BeforeTargetID
  {
    get => this.m_beforeTargetID;
    set => this.m_beforeTargetID = value;
  }

  internal OverFlow Overflow
  {
    get => this.m_overflow;
    set => this.m_overflow = value;
  }

  internal BreakBefore BeforeBreak
  {
    get => this.m_beforeBreak;
    set => this.m_beforeBreak = value;
  }

  internal BreakAfter AfterBreak
  {
    get => this.m_afterBreak;
    set => this.m_afterBreak = value;
  }

  internal void Read(XmlNode node)
  {
    if (node.Name == "break")
    {
      if (node.Attributes["beforeTarget"] != null)
        this.BeforeTargetID = node.Attributes["beforeTarget"].Value.Replace("#", "");
      else if (node.Attributes["afterTarget"] != null)
        this.AfterTargetID = node.Attributes["afterTarget"].Value.Replace("#", "");
      if (node.Attributes["startNew"] == null)
        return;
      string s = node.Attributes["startNew"].Value;
      if (s == null || !(s != string.Empty) || int.Parse(s) != 1)
        return;
      this.IsStartNew = true;
    }
    else if (node.Name == "overflow")
    {
      this.Overflow = new OverFlow();
      this.Overflow.Read(node);
    }
    else if (node.Name == "breakBefore")
    {
      this.BeforeBreak = new BreakBefore();
      this.BeforeBreak.Read(node);
    }
    else
    {
      if (!(node.Name == "breakAfter"))
        return;
      this.AfterBreak = new BreakAfter();
      this.AfterBreak.Read(node);
    }
  }
}
