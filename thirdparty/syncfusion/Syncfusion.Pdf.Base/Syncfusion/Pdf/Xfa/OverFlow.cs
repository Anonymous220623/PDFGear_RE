// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.OverFlow
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

internal class OverFlow
{
  private string m_overFlowID = string.Empty;

  internal string OverFlowID
  {
    get => this.m_overFlowID;
    set => this.m_overFlowID = value;
  }

  internal void Read(XmlNode node)
  {
    if (node.Attributes["target"] == null)
      return;
    this.OverFlowID = node.Attributes["target"].Value;
  }
}
