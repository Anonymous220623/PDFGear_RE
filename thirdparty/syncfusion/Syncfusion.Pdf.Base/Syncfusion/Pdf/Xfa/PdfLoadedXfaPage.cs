// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfLoadedXfaPage
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System;
using System.Drawing;
using System.Globalization;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

internal class PdfLoadedXfaPage
{
  private PdfPage m_currentPage;
  internal PdfXfaPageSettings pageSettings = new PdfXfaPageSettings();
  internal PdfLoadedXfaForm m_loadedXfaForm = new PdfLoadedXfaForm();
  internal PdfTemplate pageSetTemplate;
  internal string Name = string.Empty;
  internal string Id = string.Empty;
  internal PdfDocument document;
  internal PdfXfaFlattener flattener;
  internal bool isSet;
  private float m_headerTemplateHeight;

  internal PdfPage CurrentPage
  {
    get
    {
      if (this.m_currentPage == null)
      {
        this.isSet = true;
        this.AddPdfPage();
      }
      else
        this.isSet = false;
      return this.m_currentPage;
    }
  }

  internal SizeF GetClientSize()
  {
    return this.pageSettings.PageOrientation == PdfXfaPageOrientation.Landscape ? new SizeF(this.pageSettings.PageSize.Height - (this.pageSettings.Margins.Left + this.pageSettings.Margins.Right), this.pageSettings.PageSize.Width - (this.pageSettings.Margins.Top + this.pageSettings.Margins.Bottom)) : new SizeF(this.pageSettings.PageSize.Width - (this.pageSettings.Margins.Left + this.pageSettings.Margins.Right), this.pageSettings.PageSize.Height - (this.pageSettings.Margins.Top + this.pageSettings.Margins.Bottom));
  }

  internal void ReadPage(XmlNode node, PdfLoadedXfaForm lForm)
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    float num3 = 0.0f;
    float num4 = 0.0f;
    float width = 0.0f;
    float height = 0.0f;
    string empty = string.Empty;
    if (node.Attributes["name"] != null)
      this.Name = node.Attributes["name"].Value;
    if (node.Attributes["id"] != null)
      this.Id = node.Attributes["id"].Value;
    if (node["contentArea"] != null)
    {
      XmlNode xmlNode = (XmlNode) node["contentArea"];
      if (xmlNode.Attributes["x"] != null)
        num1 = this.ConvertToPoint(xmlNode.Attributes["x"].Value);
      if (xmlNode.Attributes["y"] != null)
        num2 = this.ConvertToPoint(xmlNode.Attributes["y"].Value);
      if (xmlNode.Attributes["w"] != null)
        num3 = this.ConvertToPoint(xmlNode.Attributes["w"].Value);
      if (xmlNode.Attributes["h"] != null)
        num4 = this.ConvertToPoint(xmlNode.Attributes["h"].Value);
    }
    if (node["medium"] != null)
    {
      width = this.ConvertToPoint(node["medium"].Attributes["short"].Value);
      height = this.ConvertToPoint(node["medium"].Attributes["long"].Value);
      if (node["medium"].Attributes["orientation"] != null && node["medium"].Attributes["orientation"].Value == "landscape")
        this.pageSettings.PageOrientation = PdfXfaPageOrientation.Landscape;
    }
    if ((double) width > 0.0 && (double) height > 0.0)
    {
      this.pageSettings.PageSize = new SizeF(width, height);
      this.pageSettings.Margins.Left = num1;
      this.pageSettings.Margins.Top = num2;
      if (this.pageSettings.PageOrientation == PdfXfaPageOrientation.Landscape)
      {
        this.pageSettings.Margins.Bottom = width - (num4 + num2);
        this.pageSettings.Margins.Right = height - (num3 + num1);
      }
      else
      {
        this.pageSettings.Margins.Bottom = height - (num4 + num2);
        this.pageSettings.Margins.Right = width - (num3 + num1);
      }
    }
    this.m_loadedXfaForm.parent = (PdfLoadedXfaField) lForm;
    this.m_loadedXfaForm.Name = lForm.Name;
    this.m_loadedXfaForm.nodeName = lForm.nodeName;
    this.m_loadedXfaForm.dataSetDoc = lForm.dataSetDoc;
    this.m_loadedXfaForm.currentNode = node;
    this.m_loadedXfaForm.ReadSubForm(node, this.m_loadedXfaForm, this.m_loadedXfaForm.m_fieldNames, this.m_loadedXfaForm.m_subFormNames);
    SizeF size = this.pageSettings.PageSize;
    if (this.pageSettings.PageOrientation == PdfXfaPageOrientation.Landscape && (double) size.Height > (double) size.Width || this.pageSettings.PageOrientation == PdfXfaPageOrientation.Portrait && (double) size.Width > (double) size.Height)
      size = new SizeF(size.Height, size.Width);
    this.pageSetTemplate = new PdfTemplate(size);
  }

  internal void AddPdfPage()
  {
    PdfSection pdfSection = this.document.Sections.Add();
    pdfSection.PageSettings.Margins.Left = this.pageSettings.Margins.Left;
    pdfSection.PageSettings.Margins.Right = this.pageSettings.Margins.Right;
    pdfSection.PageSettings.Margins.Top = 0.0f;
    pdfSection.PageSettings.Margins.Bottom = 0.0f;
    SizeF pageSize = this.pageSettings.PageSize;
    if (this.pageSettings.PageOrientation == PdfXfaPageOrientation.Landscape)
    {
      pdfSection.PageSettings.Orientation = PdfPageOrientation.Landscape;
      pdfSection.PageSettings.Size = (double) pageSize.Height <= (double) pageSize.Width ? pageSize : new SizeF(pageSize.Height, pageSize.Width);
    }
    else
      pdfSection.PageSettings.Size = (double) pageSize.Width <= (double) pageSize.Height ? pageSize : new SizeF(pageSize.Height, pageSize.Width);
    this.flattener.ParseSubform(this.m_loadedXfaForm);
    this.flattener.DrawPageSet(this.m_loadedXfaForm, this.pageSetTemplate);
    this.m_currentPage = pdfSection.Pages.Add();
    if (this.m_loadedXfaForm == null)
      return;
    PdfPageTemplateElement pageTemplateElement1 = new PdfPageTemplateElement(new SizeF(this.m_currentPage.GetClientSize().Width, this.pageSettings.Margins.Top));
    pageTemplateElement1.Graphics.DrawPdfTemplate(this.pageSetTemplate, new PointF(-this.pageSettings.Margins.Left, 0.0f));
    pdfSection.Template.Top = pageTemplateElement1;
    this.m_headerTemplateHeight = pageTemplateElement1.Height;
    PdfPageTemplateElement pageTemplateElement2 = new PdfPageTemplateElement(this.pageSetTemplate.Size.Width, this.pageSettings.Margins.Bottom);
    pageTemplateElement2.Graphics.Save();
    pageTemplateElement2.Graphics.SetClip(new RectangleF(0.0f, 0.0f, this.pageSetTemplate.Width, this.pageSetTemplate.Height));
    this.pageSetTemplate.Draw(pageTemplateElement2.Graphics, new PointF(-this.pageSettings.Margins.Left, (float) -((double) pdfSection.PageSettings.Size.Height - (double) this.pageSettings.Margins.Bottom)));
    pageTemplateElement2.Graphics.Restore();
    pdfSection.Template.Bottom = pageTemplateElement2;
  }

  internal void DrawPageBackgroundTemplate(PdfPage currentPage)
  {
    currentPage.Graphics.Save();
    currentPage.Graphics.SetClip(new RectangleF(new PointF(0.0f, 0.0f), this.m_currentPage.GetClientSize()));
    currentPage.Graphics.DrawPdfTemplate(this.pageSetTemplate, new PointF(-this.pageSettings.Margins.Left, -this.m_headerTemplateHeight));
    currentPage.Graphics.Restore();
  }

  internal float ConvertToPoint(string value)
  {
    float point = 0.0f;
    if (value.Contains("pt"))
      point = Convert.ToSingle(value.Trim('p', 't', 'm'), (IFormatProvider) CultureInfo.InvariantCulture);
    else if (value.Contains("m"))
      point = Convert.ToSingle(value.Trim('p', 't', 'm'), (IFormatProvider) CultureInfo.InvariantCulture) * 2.83464575f;
    else if (value.Contains("in"))
      point = Convert.ToSingle(value.Trim('i', 'n'), (IFormatProvider) CultureInfo.InvariantCulture) * 72f;
    return point;
  }
}
