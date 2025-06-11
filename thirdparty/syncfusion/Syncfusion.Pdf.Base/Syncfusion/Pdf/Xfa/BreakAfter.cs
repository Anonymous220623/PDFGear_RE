// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.BreakAfter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

internal class BreakAfter
{
  private string m_afterTargetID = string.Empty;
  private bool m_isStartNew;
  private PdfXfaTargetType m_targetType = PdfXfaTargetType.PageArea;

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

  internal PdfXfaTargetType TargetType
  {
    get => this.m_targetType;
    set => this.m_targetType = value;
  }

  internal void Read(XmlNode node)
  {
    if (node.Attributes["target"] != null)
      this.AfterTargetID = node.Attributes["target"].Value;
    if (node.Attributes["targetType"] != null)
    {
      switch (node.Attributes["targetType"].Value)
      {
        case "pageArea":
          this.TargetType = PdfXfaTargetType.PageArea;
          break;
        case "contentArea":
          this.TargetType = PdfXfaTargetType.ContentArea;
          break;
      }
    }
    if (node.Attributes["startNew"] == null)
      return;
    string s = node.Attributes["startNew"].Value;
    if (s == null || !(s != string.Empty) || int.Parse(s) != 1)
      return;
    this.IsStartNew = true;
  }
}
