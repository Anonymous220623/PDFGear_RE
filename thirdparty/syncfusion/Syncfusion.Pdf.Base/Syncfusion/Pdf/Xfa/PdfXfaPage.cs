// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfXfaPage
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

public class PdfXfaPage
{
  internal int pageId;
  internal bool isAdded;
  internal bool isBreaked;
  internal PdfXfaPageSettings pageSettings = new PdfXfaPageSettings();

  internal void Save(XfaWriter writer)
  {
    writer.Write.WriteStartElement("pageArea");
    writer.Write.WriteAttributeString("name", "Page" + this.pageId.ToString());
    writer.Write.WriteStartElement("contentArea");
    writer.Write.WriteAttributeString("x", this.pageSettings.Margins.Left.ToString() + "pt");
    writer.Write.WriteAttributeString("y", this.pageSettings.Margins.Top.ToString() + "pt");
    SizeF sizeF = new SizeF(this.pageSettings.PageSize.Width - (this.pageSettings.Margins.Left + this.pageSettings.Margins.Right), this.pageSettings.PageSize.Height - (this.pageSettings.Margins.Top + this.pageSettings.Margins.Bottom));
    if (this.pageSettings.PageOrientation == PdfXfaPageOrientation.Portrait)
    {
      writer.Write.WriteAttributeString("w", sizeF.Width.ToString() + "pt");
      writer.Write.WriteAttributeString("h", sizeF.Height.ToString() + "pt");
    }
    else
    {
      writer.Write.WriteAttributeString("w", sizeF.Height.ToString() + "pt");
      writer.Write.WriteAttributeString("h", sizeF.Width.ToString() + "pt");
    }
    writer.Write.WriteEndElement();
    writer.Write.WriteStartElement("medium");
    writer.Write.WriteAttributeString("short", this.pageSettings.PageSize.Width.ToString() + "pt");
    writer.Write.WriteAttributeString("long", this.pageSettings.PageSize.Height.ToString() + "pt");
    if (this.pageSettings.PageOrientation != PdfXfaPageOrientation.Portrait)
      writer.Write.WriteAttributeString("orientation", this.pageSettings.PageOrientation.ToString().ToLower());
    writer.Write.WriteEndElement();
    writer.Write.WriteEndElement();
    this.isAdded = true;
  }

  public SizeF GetClientSize()
  {
    return this.pageSettings.PageOrientation == PdfXfaPageOrientation.Landscape ? new SizeF(this.pageSettings.PageSize.Height - (this.pageSettings.Margins.Left + this.pageSettings.Margins.Right), this.pageSettings.PageSize.Width - (this.pageSettings.Margins.Top + this.pageSettings.Margins.Bottom)) : new SizeF(this.pageSettings.PageSize.Width - (this.pageSettings.Margins.Left + this.pageSettings.Margins.Right), this.pageSettings.PageSize.Height - (this.pageSettings.Margins.Top + this.pageSettings.Margins.Bottom));
  }
}
