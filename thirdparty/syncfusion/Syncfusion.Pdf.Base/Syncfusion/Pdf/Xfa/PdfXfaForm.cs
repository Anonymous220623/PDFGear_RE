// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfXfaForm
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

public class PdfXfaForm : PdfXfaField, ICloneable
{
  private PdfXfaFlowDirection m_flowDirection;
  private PdfXfaFieldCollection m_fields;
  internal XmlWriter m_dataSetWriter;
  private int m_fieldCount = 1;
  internal PdfDocument m_document;
  internal PdfArray m_imageArray = new PdfArray();
  private PdfXfaBorder m_border;
  private float m_width;
  private bool m_readOnly;
  private PdfXfaPage m_xfaPage;
  internal PdfPage m_page;
  internal PdfXfaForm m_parent;
  internal PdfXfaType m_formType;
  internal PdfXfaDocument m_xfaDocument;
  internal PointF m_startPoint = PointF.Empty;
  internal PointF m_currentPoint = PointF.Empty;
  internal SizeF m_maxSize = SizeF.Empty;
  internal string m_name = string.Empty;
  internal PdfFieldCollection m_acroFields = new PdfFieldCollection();
  internal List<string> m_subFormNames = new List<string>();
  internal List<string> m_fieldNames = new List<string>();
  internal SizeF m_size = SizeF.Empty;
  private SizeF m_maximumSize = SizeF.Empty;
  private PointF m_currentPosition = PointF.Empty;
  private SizeF m_pageSize = SizeF.Empty;
  private float m_height;
  private List<float> m_borderHeight = new List<float>();
  private int m_borderCount;
  private PdfXfaPage m_tempPage;
  internal bool m_isReadOnly;

  public PdfXfaFlowDirection FlowDirection
  {
    get => this.m_flowDirection;
    set => this.m_flowDirection = value;
  }

  public PdfXfaBorder Border
  {
    get => this.m_border;
    set
    {
      if (value == null)
        return;
      this.m_border = value;
    }
  }

  public PdfXfaFieldCollection Fields
  {
    get
    {
      if (this.m_fields == null)
        this.m_fields = new PdfXfaFieldCollection();
      return this.m_fields;
    }
    internal set => this.m_fields = value;
  }

  public bool ReadOnly
  {
    get => this.m_readOnly;
    set => this.m_readOnly = value;
  }

  public float Width
  {
    set => this.m_width = value;
    get => this.m_width;
  }

  public PdfXfaPage Page
  {
    get => this.m_xfaPage;
    set
    {
      if (value == null)
        return;
      this.m_xfaPage = value;
    }
  }

  public PdfXfaForm(float width)
  {
    this.FlowDirection = PdfXfaFlowDirection.Vertical;
    this.Border = (PdfXfaBorder) null;
    this.Width = width;
  }

  public PdfXfaForm(string name, float width)
  {
    this.FlowDirection = PdfXfaFlowDirection.Vertical;
    this.Name = name;
    this.Border = (PdfXfaBorder) null;
    this.Width = width;
  }

  public PdfXfaForm(string name, PdfXfaPage xfaPage, float width)
  {
    this.FlowDirection = PdfXfaFlowDirection.Vertical;
    this.m_xfaPage = xfaPage;
    this.Name = name;
    this.Border = (PdfXfaBorder) null;
    this.Width = width;
  }

  public PdfXfaForm(string name, PdfXfaFlowDirection flowDirection, float width)
  {
    this.FlowDirection = flowDirection;
    this.Name = name;
    this.Border = (PdfXfaBorder) null;
    this.Width = width;
  }

  public PdfXfaForm(PdfXfaPage xfaPage, PdfXfaFlowDirection flowDirection, float width)
  {
    this.FlowDirection = flowDirection;
    this.m_xfaPage = xfaPage;
    this.Border = (PdfXfaBorder) null;
    this.Width = width;
  }

  public PdfXfaForm(PdfXfaFlowDirection flowDirection, float width)
  {
    this.FlowDirection = flowDirection;
    this.Border = (PdfXfaBorder) null;
    this.Width = width;
  }

  public PdfXfaForm(
    string name,
    PdfXfaPage xfaPage,
    PdfXfaFlowDirection flowDirection,
    float width)
  {
    this.FlowDirection = flowDirection;
    this.m_xfaPage = xfaPage;
    this.Name = name;
    this.Border = (PdfXfaBorder) null;
    this.Width = width;
  }

  internal PdfXfaForm()
  {
  }

  internal void Save(PdfDocument document, PdfXfaType type)
  {
    this.m_document = document;
    if (this.m_formType == PdfXfaType.Dynamic)
      this.Message();
    this.m_formType = type;
    if (this.m_document.Form.Dictionary.ContainsKey("XFA"))
      return;
    XfaWriter xfaWriter = new XfaWriter();
    PdfStream pdfStream = new PdfStream();
    this.m_dataSetWriter = XmlWriter.Create((Stream) pdfStream.InternalStream, new XmlWriterSettings()
    {
      OmitXmlDeclaration = true,
      Encoding = (Encoding) new UTF8Encoding(false)
    });
    xfaWriter.StartDataSets(this.m_dataSetWriter);
    PdfResources pdfResources = new PdfResources();
    PdfArray primitive = new PdfArray();
    primitive.Add((IPdfPrimitive) new PdfString("preamble"));
    primitive.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) xfaWriter.WritePreamble()));
    primitive.Add((IPdfPrimitive) new PdfString("config"));
    primitive.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) xfaWriter.WriteConfig()));
    primitive.Add((IPdfPrimitive) new PdfString("template"));
    primitive.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) xfaWriter.WriteDocumentTemplate(this)));
    primitive.Add((IPdfPrimitive) new PdfString("datasets"));
    xfaWriter.EndDataSets(this.m_dataSetWriter);
    this.m_dataSetWriter.Close();
    primitive.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfStream));
    primitive.Add((IPdfPrimitive) new PdfString("postamble"));
    primitive.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) xfaWriter.WritePostable()));
    this.m_document.Form.Dictionary.SetProperty("XFA", (IPdfPrimitive) primitive);
    if (this.m_imageArray == null)
      return;
    PdfDictionary pdfDictionary1 = new PdfDictionary();
    PdfDictionary pdfDictionary2 = new PdfDictionary();
    pdfDictionary2.SetProperty("Names", (IPdfPrimitive) this.m_imageArray);
    pdfDictionary1.SetProperty("XFAImages", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary2));
    this.m_document.Catalog.SetProperty("Names", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary1));
  }

  internal void SaveMainForm(XfaWriter xfaWriter)
  {
    xfaWriter.Write.WriteStartElement("subform");
    PdfXfaForm collection = new PdfXfaForm();
    collection.m_name = this.m_xfaDocument.FormName == null || !(this.m_xfaDocument.FormName != "") ? "form1" : this.m_xfaDocument.FormName;
    xfaWriter.Write.WriteAttributeString("name", collection.m_name);
    this.m_dataSetWriter.WriteStartElement(collection.m_name);
    xfaWriter.Write.WriteAttributeString("locale", "en_US");
    xfaWriter.Write.WriteAttributeString("layout", "tb");
    xfaWriter.Write.WriteStartElement("pageSet");
    if (this.m_xfaDocument.Pages.m_pages.Count > 0)
    {
      foreach (PdfXfaPage page in this.m_xfaDocument.Pages.m_pages)
        page.Save(xfaWriter);
    }
    else
      this.m_xfaDocument.Pages.Add().Save(xfaWriter);
    xfaWriter.Write.WriteEndElement();
    if (this.m_xfaDocument.Pages.m_pages.Count > 0)
      this.m_xfaPage = this.m_xfaDocument.Pages.m_pages[0];
    this.AddForm(xfaWriter);
    if (this.m_formType == PdfXfaType.Static)
    {
      collection.m_acroFields.Add(this, collection.GetSubFormName(this.m_name));
      this.m_document.Form.isXfaForm = true;
      this.m_document.Form.Fields.Add(collection, collection.GetSubFormName(collection.m_name));
    }
    this.m_dataSetWriter.WriteEndElement();
    xfaWriter.Write.WriteEndElement();
  }

  internal void AddForm(XfaWriter xfaWriter)
  {
    xfaWriter.Write.WriteStartElement("subform");
    this.m_name = this.Name == null || !(this.Name != "") ? "subform" + xfaWriter.m_subFormFieldCount++.ToString() : this.Name;
    xfaWriter.Write.WriteAttributeString("name", this.m_name);
    this.m_dataSetWriter.WriteStartElement(this.m_name);
    if (this.FlowDirection == PdfXfaFlowDirection.Horizontal)
      xfaWriter.Write.WriteAttributeString("layout", "lr-tb");
    else
      xfaWriter.Write.WriteAttributeString("layout", "tb");
    if ((double) this.Width != 0.0)
      xfaWriter.SetSize(0.0f, this.Width, 0.0f, 0.0f);
    if (this.ReadOnly)
      xfaWriter.Write.WriteAttributeString("access", "readOnly");
    if (this.m_xfaPage != null && this.m_formType == PdfXfaType.Static)
      this.m_page = this.AddPdfPage(this.m_xfaPage);
    this.m_tempPage = this.m_xfaPage;
    if (this.m_formType == PdfXfaType.Static)
    {
      this.m_pageSize = this.m_page.GetClientSize();
      this.ParseSubForm(this);
      this.m_size.Height += this.Margins.Bottom + this.Margins.Top + this.m_maximumSize.Height;
      this.m_currentPosition = this.m_startPoint = this.m_currentPoint = new PointF(this.Margins.Left, this.Margins.Top);
      this.GetBackgroundHeight(this);
      this.m_borderHeight.Add(this.m_currentPoint.Y + this.m_maxSize.Height);
      this.m_currentPoint = this.m_startPoint = this.m_currentPosition = PointF.Empty;
      this.m_maxSize = SizeF.Empty;
      this.m_height = 0.0f;
    }
    xfaWriter.WriteMargins(this.Margins);
    if (this.Border != null)
      xfaWriter.DrawBorder(this.Border);
    if (this.m_formType == PdfXfaType.Static && this.Border != null)
    {
      PdfPen pen = this.Border.GetPen();
      RectangleF rectangleF = new RectangleF();
      rectangleF = new RectangleF(0.0f, 0.0f, (double) this.Width != 0.0 ? this.Width : this.m_size.Width, this.m_borderHeight[0]);
      ++this.m_borderCount;
      if (this.Border.LeftEdge != null || this.Border.RightEdge != null || this.Border.TopEdge != null || this.Border.BottomEdge != null)
      {
        this.m_page.Graphics.DrawRectangle(this.Border.GetBrush(rectangleF), rectangleF);
        if (this.Border.LeftEdge != null)
          this.DrawEdge(this.Border.LeftEdge, rectangleF.Location, new PointF(rectangleF.X, rectangleF.Y + rectangleF.Height), this.m_page);
        if (this.Border.RightEdge != null)
          this.DrawEdge(this.Border.RightEdge, new PointF(rectangleF.X + rectangleF.Width, rectangleF.Y), new PointF(rectangleF.X + rectangleF.Width, rectangleF.Y + rectangleF.Height), this.m_page);
        if (this.Border.TopEdge != null)
          this.DrawEdge(this.Border.TopEdge, rectangleF.Location, new PointF(rectangleF.X + rectangleF.Width, rectangleF.Y), this.m_page);
        if (this.Border.BottomEdge != null)
          this.DrawEdge(this.Border.BottomEdge, new PointF(rectangleF.X, rectangleF.Y + rectangleF.Height), new PointF(rectangleF.X + rectangleF.Width, rectangleF.Y + rectangleF.Height), this.m_page);
      }
      else
        this.m_page.Graphics.DrawRectangle(pen, this.Border.GetBrush(rectangleF), rectangleF);
    }
    this.m_currentPosition = this.m_startPoint = this.m_currentPoint = new PointF(this.Margins.Left, this.Margins.Top);
    this.AddSubForm(this, xfaWriter);
    this.m_dataSetWriter.WriteEndElement();
  }

  private void DrawEdge(PdfXfaEdge edge, PointF startPont, PointF endPoint, PdfPage page)
  {
    page.Graphics.DrawLine(new PdfPen(edge.Color)
    {
      Width = edge.Thickness,
      DashStyle = this.GetPenDashStyle(edge.BorderStyle)
    }, startPont, endPoint);
  }

  private PdfDashStyle GetPenDashStyle(PdfXfaBorderStyle style)
  {
    PdfDashStyle penDashStyle = PdfDashStyle.Solid;
    switch (style)
    {
      case PdfXfaBorderStyle.Dashed:
        penDashStyle = PdfDashStyle.Dash;
        break;
      case PdfXfaBorderStyle.Dotted:
        penDashStyle = PdfDashStyle.Dot;
        break;
      case PdfXfaBorderStyle.DashDot:
        penDashStyle = PdfDashStyle.DashDot;
        break;
      case PdfXfaBorderStyle.DashDotDot:
        penDashStyle = PdfDashStyle.DashDotDot;
        break;
    }
    return penDashStyle;
  }

  internal void AddSubForm(PdfXfaForm subForm, XfaWriter xfaWriter)
  {
    for (int offset = 0; offset < subForm.Fields.Count; ++offset)
    {
      object field1 = (object) subForm.Fields[offset];
      switch (field1)
      {
        case PdfXfaForm _:
          PdfXfaForm pdfXfaForm = field1 as PdfXfaForm;
          pdfXfaForm.m_fieldNames = new List<string>();
          pdfXfaForm.m_name = pdfXfaForm.Name == null || !(pdfXfaForm.Name != "") || !(pdfXfaForm.Name != string.Empty) ? "subform" + xfaWriter.m_subFormFieldCount++.ToString() : pdfXfaForm.Name;
          pdfXfaForm.m_formType = this.m_formType;
          this.m_dataSetWriter.WriteStartElement(pdfXfaForm.m_name);
          pdfXfaForm.m_parent = subForm;
          xfaWriter.Write.WriteStartElement("subform");
          xfaWriter.Write.WriteAttributeString("name", pdfXfaForm.m_name);
          xfaWriter.SetSize(0.0f, pdfXfaForm.Width, 0.0f, 0.0f);
          if (pdfXfaForm.FlowDirection == PdfXfaFlowDirection.Horizontal)
            xfaWriter.Write.WriteAttributeString("layout", "lr-tb");
          else
            xfaWriter.Write.WriteAttributeString("layout", "tb");
          if (pdfXfaForm.ReadOnly)
            xfaWriter.Write.WriteAttributeString("access", "readOnly");
          if (pdfXfaForm.Border != null)
            xfaWriter.DrawBorder(pdfXfaForm.Border);
          xfaWriter.WriteMargins(pdfXfaForm.Margins);
          SizeF sizeF = SizeF.Empty;
          if (this.m_formType == PdfXfaType.Static)
          {
            if (subForm.ReadOnly)
              pdfXfaForm.m_readOnly = true;
            pdfXfaForm.m_startPoint = new PointF(subForm.m_startPoint.X + pdfXfaForm.Margins.Left, subForm.m_startPoint.Y + pdfXfaForm.Margins.Top);
            pdfXfaForm.m_currentPoint = pdfXfaForm.m_currentPosition = pdfXfaForm.m_startPoint;
            sizeF = new SizeF((double) pdfXfaForm.Width != 0.0 ? pdfXfaForm.Width : pdfXfaForm.m_size.Width, pdfXfaForm.m_size.Height);
            if (subForm.FlowDirection == PdfXfaFlowDirection.Horizontal && (double) subForm.Width != 0.0 && (double) subForm.m_currentPoint.X + (double) sizeF.Width > (double) subForm.Width)
            {
              subForm.m_currentPoint.X = subForm.m_currentPosition.X;
              subForm.m_currentPoint.Y += subForm.m_maxSize.Height;
              subForm.m_height += subForm.m_maxSize.Height;
              subForm.m_maxSize.Height = 0.0f;
            }
            pdfXfaForm.m_currentPoint.X += subForm.m_currentPoint.X - subForm.m_startPoint.X;
            if ((double) pdfXfaForm.m_currentPoint.Y < (double) subForm.m_currentPoint.Y)
              pdfXfaForm.m_currentPoint.Y += subForm.m_currentPoint.Y - subForm.m_startPoint.Y;
            if (pdfXfaForm.m_xfaPage != null)
            {
              pdfXfaForm.BreakPage(xfaWriter, pdfXfaForm.m_xfaPage);
              if (this.m_formType == PdfXfaType.Static)
              {
                this.m_page = this.AddPdfPage(pdfXfaForm.m_xfaPage);
                this.m_pageSize = this.m_page.GetClientSize();
                this.SetCurrentPoint(pdfXfaForm);
                pdfXfaForm.m_startPoint = new PointF(subForm.m_startPoint.X + pdfXfaForm.Margins.Left, subForm.m_startPoint.Y + pdfXfaForm.Margins.Top);
                pdfXfaForm.m_currentPoint = pdfXfaForm.m_startPoint;
              }
            }
            pdfXfaForm.m_page = this.m_page;
            pdfXfaForm.m_currentPosition = pdfXfaForm.m_currentPoint;
            if (pdfXfaForm.Border != null)
            {
              if (pdfXfaForm.m_xfaPage != null)
                this.DrawBackground(pdfXfaForm);
              PdfPen pen = pdfXfaForm.Border.GetPen();
              RectangleF rectangleF = new RectangleF();
              float width = pdfXfaForm.Width;
              if ((double) pdfXfaForm.Width == 0.0)
                width = pdfXfaForm.m_size.Width + pdfXfaForm.Margins.Left + pdfXfaForm.Margins.Right;
              rectangleF = new RectangleF(pdfXfaForm.m_currentPoint.X - pdfXfaForm.Margins.Left, pdfXfaForm.m_currentPoint.Y - pdfXfaForm.Margins.Top, width, pdfXfaForm.m_borderHeight[pdfXfaForm.m_borderCount]);
              if ((double) pdfXfaForm.m_startPoint.Y != (double) pdfXfaForm.m_currentPoint.Y)
                rectangleF.Height -= pdfXfaForm.m_currentPoint.Y;
              rectangleF.Height += pdfXfaForm.Margins.Bottom;
              ++pdfXfaForm.m_borderCount;
              if ((double) rectangleF.Height > 0.0)
              {
                if (pdfXfaForm.Border.LeftEdge != null || pdfXfaForm.Border.RightEdge != null || pdfXfaForm.Border.TopEdge != null || pdfXfaForm.Border.BottomEdge != null)
                {
                  this.m_page.Graphics.DrawRectangle(pdfXfaForm.Border.GetBrush(rectangleF), rectangleF);
                  if (pdfXfaForm.Border.LeftEdge != null)
                    this.DrawEdge(pdfXfaForm.Border.LeftEdge, rectangleF.Location, new PointF(rectangleF.X, rectangleF.Y + rectangleF.Height), this.m_page);
                  if (pdfXfaForm.Border.RightEdge != null)
                    this.DrawEdge(pdfXfaForm.Border.RightEdge, new PointF(rectangleF.X + rectangleF.Width, rectangleF.Y), new PointF(rectangleF.X + rectangleF.Width, rectangleF.Y + rectangleF.Height), this.m_page);
                  if (pdfXfaForm.Border.TopEdge != null)
                    this.DrawEdge(pdfXfaForm.Border.TopEdge, rectangleF.Location, new PointF(rectangleF.X + rectangleF.Width, rectangleF.Y), this.m_page);
                  if (pdfXfaForm.Border.BottomEdge != null)
                    this.DrawEdge(pdfXfaForm.Border.BottomEdge, new PointF(rectangleF.X, rectangleF.Y + rectangleF.Height), new PointF(rectangleF.X + rectangleF.Width, rectangleF.Y + rectangleF.Height), this.m_page);
                }
                else
                  this.m_page.Graphics.DrawRectangle(pen, pdfXfaForm.Border.GetBrush(rectangleF), rectangleF);
              }
            }
            else if (pdfXfaForm.m_xfaPage != null)
              this.DrawBackground(pdfXfaForm);
          }
          else if (pdfXfaForm.m_xfaPage != null)
            pdfXfaForm.BreakPage(xfaWriter, pdfXfaForm.m_xfaPage);
          this.AddSubForm(pdfXfaForm, xfaWriter);
          if (this.m_formType == PdfXfaType.Static)
          {
            string subFormName = subForm.GetSubFormName(pdfXfaForm.m_name);
            if (pdfXfaForm.m_acroFields.Count > 0)
              subForm.m_acroFields.Add(pdfXfaForm, subFormName);
            pdfXfaForm.m_isRendered = true;
            subForm.m_currentPoint.X += sizeF.Width;
            if (subForm.FlowDirection == PdfXfaFlowDirection.Vertical)
            {
              subForm.m_currentPoint.X = subForm.m_startPoint.X;
              if (pdfXfaForm.FlowDirection == PdfXfaFlowDirection.Horizontal)
                pdfXfaForm.m_height += pdfXfaForm.m_maxSize.Height + pdfXfaForm.Margins.Bottom + pdfXfaForm.Margins.Top;
              else
                pdfXfaForm.m_height += pdfXfaForm.Margins.Bottom + pdfXfaForm.Margins.Top;
              if ((double) pdfXfaForm.m_height - (double) pdfXfaForm.Margins.Bottom != 0.0)
                subForm.m_currentPoint.Y += pdfXfaForm.m_height;
              else
                subForm.m_currentPoint.Y = pdfXfaForm.m_currentPoint.Y;
              subForm.m_maxSize.Height = 0.0f;
            }
            else
            {
              if (pdfXfaForm.FlowDirection == PdfXfaFlowDirection.Horizontal)
                pdfXfaForm.m_height += pdfXfaForm.m_maxSize.Height + pdfXfaForm.Margins.Bottom + pdfXfaForm.Margins.Top;
              else
                pdfXfaForm.m_height += pdfXfaForm.Margins.Bottom + pdfXfaForm.Margins.Top;
              if ((double) pdfXfaForm.m_height - (double) pdfXfaForm.Margins.Bottom != 0.0)
              {
                if ((double) subForm.m_maxSize.Height < (double) pdfXfaForm.m_height)
                  subForm.m_maxSize.Height = pdfXfaForm.m_height;
              }
              else if ((double) subForm.m_maxSize.Height < (double) pdfXfaForm.m_currentPoint.Y)
                subForm.m_maxSize.Height = pdfXfaForm.m_currentPoint.Y;
            }
          }
          xfaWriter.Write.WriteEndElement();
          this.m_dataSetWriter.WriteEndElement();
          break;
        case PdfXfaTextBoxField _:
          PdfXfaTextBoxField pdfXfaTextBoxField = field1 as PdfXfaTextBoxField;
          pdfXfaTextBoxField.parent = subForm;
          this.m_dataSetWriter.WriteStartElement(pdfXfaTextBoxField.Name);
          this.m_dataSetWriter.WriteString(pdfXfaTextBoxField.Text);
          this.m_dataSetWriter.WriteEndElement();
          pdfXfaTextBoxField.Save(xfaWriter, this.m_formType);
          this.SetFont((PdfDocumentBase) this.m_document, pdfXfaTextBoxField.Font);
          if (this.m_formType == PdfXfaType.Static)
          {
            SizeF size = pdfXfaTextBoxField.GetSize();
            if (subForm.FlowDirection == PdfXfaFlowDirection.Horizontal)
            {
              if ((double) subForm.Width != 0.0 && (double) subForm.m_currentPoint.X + (double) size.Width - (double) subForm.m_currentPosition.X > (double) subForm.Width - ((double) subForm.Margins.Left + (double) subForm.Margins.Right))
              {
                subForm.m_currentPoint.X = subForm.m_currentPosition.X;
                subForm.m_currentPoint.Y += subForm.m_maxSize.Height;
                subForm.m_height += subForm.m_maxSize.Height;
                subForm.m_maxSize.Height = 0.0f;
              }
              if ((double) subForm.m_currentPoint.Y + (double) size.Height > (double) this.m_pageSize.Height - ((double) subForm.Margins.Top + (double) subForm.Margins.Bottom))
              {
                this.m_page = this.m_page.Section.Pages.Add();
                this.m_pageSize = this.m_page.GetClientSize();
                subForm.m_currentPoint = subForm.m_startPoint;
                this.SetCurrentPoint(subForm);
                this.DrawBackground(subForm);
                subForm.m_height = 0.0f;
              }
              subForm.m_acroFields.Add(pdfXfaTextBoxField.SaveAcroForm(this.m_page, new RectangleF(subForm.m_currentPoint, size), subForm.GetFieldName(pdfXfaTextBoxField.Name)));
              subForm.m_currentPoint.X += size.Width;
            }
            else
            {
              subForm.m_currentPoint.X = subForm.m_currentPosition.X;
              if ((double) subForm.m_currentPoint.Y + (double) size.Height > (double) this.m_pageSize.Height - (double) subForm.Margins.Bottom)
              {
                this.m_page = this.m_page.Section.Pages.Add();
                this.m_pageSize = this.m_page.GetClientSize();
                subForm.m_currentPoint = subForm.m_startPoint;
                this.SetCurrentPoint(subForm);
                this.DrawBackground(subForm);
                subForm.m_height = 0.0f;
              }
              subForm.m_acroFields.Add(pdfXfaTextBoxField.SaveAcroForm(this.m_page, new RectangleF(subForm.m_currentPoint, size), subForm.GetFieldName(this.Name)));
              subForm.m_currentPoint.Y += size.Height;
              subForm.m_height += size.Height;
            }
            if ((double) subForm.m_maxSize.Width < (double) size.Width)
              subForm.m_maxSize.Width = size.Width;
            if ((double) subForm.m_maxSize.Height < (double) size.Height)
            {
              subForm.m_maxSize.Height = size.Height;
              break;
            }
            break;
          }
          break;
        case PdfXfaNumericField _:
          PdfXfaNumericField pdfXfaNumericField = field1 as PdfXfaNumericField;
          pdfXfaNumericField.parent = subForm;
          this.m_dataSetWriter.WriteStartElement(pdfXfaNumericField.Name);
          if (!double.IsNaN(pdfXfaNumericField.NumericValue))
          {
            if (pdfXfaNumericField.FieldType == PdfXfaNumericType.Integer)
              pdfXfaNumericField.NumericValue = (double) (int) pdfXfaNumericField.NumericValue;
            this.m_dataSetWriter.WriteString(pdfXfaNumericField.NumericValue.ToString());
          }
          this.m_dataSetWriter.WriteEndElement();
          pdfXfaNumericField.Save(xfaWriter);
          this.SetFont((PdfDocumentBase) this.m_document, pdfXfaNumericField.Font);
          if (this.m_formType == PdfXfaType.Static)
          {
            SizeF size = pdfXfaNumericField.GetSize();
            if (subForm.FlowDirection == PdfXfaFlowDirection.Horizontal)
            {
              if ((double) subForm.Width != 0.0 && (double) subForm.m_currentPoint.X + (double) size.Width - (double) subForm.m_currentPosition.X > (double) subForm.Width - ((double) subForm.Margins.Left + (double) subForm.Margins.Right))
              {
                subForm.m_currentPoint.X = subForm.m_currentPosition.X;
                subForm.m_currentPoint.Y += subForm.m_maxSize.Height;
                subForm.m_height += subForm.m_maxSize.Height;
                subForm.m_maxSize.Height = 0.0f;
              }
              if ((double) subForm.m_currentPoint.Y + (double) size.Height > (double) this.m_pageSize.Height - ((double) subForm.Margins.Top + (double) subForm.Margins.Bottom))
              {
                this.m_page = this.m_page.Section.Pages.Add();
                this.m_pageSize = this.m_page.GetClientSize();
                subForm.m_currentPoint = subForm.m_startPoint;
                this.SetCurrentPoint(subForm);
                this.DrawBackground(subForm);
                subForm.m_height = 0.0f;
              }
              subForm.m_acroFields.Add(pdfXfaNumericField.SaveAcroForm(this.m_page, new RectangleF(subForm.m_currentPoint, size), subForm.GetFieldName(pdfXfaNumericField.Name)));
              subForm.m_currentPoint.X += size.Width;
            }
            else
            {
              subForm.m_currentPoint.X = subForm.m_currentPosition.X;
              if ((double) subForm.m_currentPoint.Y + (double) size.Height > (double) this.m_pageSize.Height - (double) subForm.Margins.Bottom)
              {
                this.m_page = this.m_page.Section.Pages.Add();
                this.m_pageSize = this.m_page.GetClientSize();
                subForm.m_currentPoint = subForm.m_startPoint;
                this.SetCurrentPoint(subForm);
                this.DrawBackground(subForm);
                subForm.m_height = 0.0f;
              }
              subForm.m_acroFields.Add(pdfXfaNumericField.SaveAcroForm(this.m_page, new RectangleF(subForm.m_currentPoint, size), subForm.GetFieldName(this.Name)));
              subForm.m_currentPoint.Y += size.Height;
              subForm.m_height += size.Height;
            }
            if ((double) subForm.m_maxSize.Width < (double) size.Width)
              subForm.m_maxSize.Width = size.Width;
            if ((double) subForm.m_maxSize.Height < (double) size.Height)
            {
              subForm.m_maxSize.Height = size.Height;
              break;
            }
            break;
          }
          break;
        case PdfXfaTextElement _:
          PdfXfaTextElement pdfXfaTextElement = field1 as PdfXfaTextElement;
          pdfXfaTextElement.parent = subForm;
          pdfXfaTextElement.Save(xfaWriter);
          this.SetFont((PdfDocumentBase) this.m_document, pdfXfaTextElement.Font);
          if (this.m_formType == PdfXfaType.Static)
          {
            SizeF size = pdfXfaTextElement.GetSize();
            if (subForm.FlowDirection == PdfXfaFlowDirection.Horizontal)
            {
              if ((double) subForm.Width != 0.0 && (double) subForm.m_currentPoint.X + (double) size.Width - (double) subForm.m_currentPosition.X > (double) subForm.Width - ((double) subForm.Margins.Left + (double) subForm.Margins.Right))
              {
                subForm.m_currentPoint.X = subForm.m_currentPosition.X;
                subForm.m_currentPoint.Y += subForm.m_maxSize.Height;
                subForm.m_height += subForm.m_maxSize.Height;
                subForm.m_maxSize.Height = 0.0f;
              }
              if ((double) subForm.m_currentPoint.Y + (double) size.Height > (double) this.m_pageSize.Height - ((double) subForm.Margins.Top + (double) subForm.Margins.Bottom))
              {
                this.m_page = this.m_page.Section.Pages.Add();
                this.m_pageSize = this.m_page.GetClientSize();
                subForm.m_currentPoint = subForm.m_startPoint;
                this.SetCurrentPoint(subForm);
                this.DrawBackground(subForm);
                subForm.m_height = 0.0f;
              }
              pdfXfaTextElement.SaveAcroForm(this.m_page, new RectangleF(subForm.m_currentPoint, size));
              subForm.m_currentPoint.X += size.Width;
            }
            else
            {
              subForm.m_currentPoint.X = subForm.m_currentPosition.X;
              if ((double) subForm.m_currentPoint.Y + (double) size.Height > (double) this.m_pageSize.Height - (double) subForm.Margins.Bottom)
              {
                this.m_page = this.m_page.Section.Pages.Add();
                this.m_pageSize = this.m_page.GetClientSize();
                subForm.m_currentPoint = subForm.m_startPoint;
                this.SetCurrentPoint(subForm);
                this.DrawBackground(subForm);
                subForm.m_height = 0.0f;
              }
              pdfXfaTextElement.SaveAcroForm(this.m_page, new RectangleF(subForm.m_currentPoint, size));
              subForm.m_currentPoint.Y += size.Height;
              subForm.m_height += size.Height;
            }
            if ((double) subForm.m_maxSize.Width < (double) size.Width)
              subForm.m_maxSize.Width = size.Width;
            if ((double) subForm.m_maxSize.Height < (double) size.Height)
            {
              subForm.m_maxSize.Height = size.Height;
              break;
            }
            break;
          }
          break;
        case PdfXfaImage _:
          string imageName = "ImageReference" + this.m_fieldCount++.ToString();
          PdfXfaImage pdfXfaImage = field1 as PdfXfaImage;
          if (!pdfXfaImage.isBase64Type)
          {
            this.m_imageArray.Add((IPdfPrimitive) new PdfString(imageName));
            this.m_imageArray.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) (field1 as PdfXfaImage).ImageStream));
          }
          pdfXfaImage.parent = subForm;
          pdfXfaImage.Save(this.m_fieldCount++, imageName, xfaWriter);
          if (this.m_formType == PdfXfaType.Static)
          {
            SizeF size = pdfXfaImage.GetSize();
            if (subForm.FlowDirection == PdfXfaFlowDirection.Horizontal)
            {
              if ((double) subForm.Width != 0.0 && (double) subForm.m_currentPoint.X + (double) size.Width - (double) subForm.m_currentPosition.X > (double) subForm.Width - ((double) subForm.Margins.Left + (double) subForm.Margins.Right))
              {
                subForm.m_currentPoint.X = subForm.m_currentPosition.X;
                subForm.m_currentPoint.Y += subForm.m_maxSize.Height;
                subForm.m_height += subForm.m_maxSize.Height;
                subForm.m_maxSize.Height = 0.0f;
              }
              if ((double) subForm.m_currentPoint.Y + (double) size.Height > (double) this.m_pageSize.Height - ((double) subForm.Margins.Top + (double) subForm.Margins.Bottom))
              {
                this.m_page = this.m_page.Section.Pages.Add();
                this.m_pageSize = this.m_page.GetClientSize();
                subForm.m_currentPoint = subForm.m_startPoint;
                this.SetCurrentPoint(subForm);
                this.DrawBackground(subForm);
                subForm.m_height = 0.0f;
              }
              pdfXfaImage.SaveAcroForm(this.m_page, new RectangleF(subForm.m_currentPoint, size));
              subForm.m_currentPoint.X += size.Width;
            }
            else
            {
              subForm.m_currentPoint.X = subForm.m_currentPosition.X;
              if ((double) subForm.m_currentPoint.Y + (double) size.Height > (double) this.m_pageSize.Height - (double) subForm.Margins.Bottom)
              {
                this.m_page = this.m_page.Section.Pages.Add();
                this.m_pageSize = this.m_page.GetClientSize();
                subForm.m_currentPoint = subForm.m_startPoint;
                this.SetCurrentPoint(subForm);
                this.DrawBackground(subForm);
                subForm.m_height = 0.0f;
              }
              pdfXfaImage.SaveAcroForm(this.m_page, new RectangleF(subForm.m_currentPoint, size));
              subForm.m_currentPoint.Y += size.Height;
              subForm.m_height += size.Height;
            }
            if ((double) subForm.m_maxSize.Width < (double) size.Width)
              subForm.m_maxSize.Width = size.Width;
            if ((double) subForm.m_maxSize.Height < (double) size.Height)
            {
              subForm.m_maxSize.Height = size.Height;
              break;
            }
            break;
          }
          break;
        case PdfXfaLine _:
          PdfXfaLine pdfXfaLine = field1 as PdfXfaLine;
          pdfXfaLine.parent = subForm;
          pdfXfaLine.Save(xfaWriter);
          if (this.m_formType == PdfXfaType.Static)
          {
            SizeF size = pdfXfaLine.GetSize();
            if (subForm.FlowDirection == PdfXfaFlowDirection.Horizontal)
            {
              if ((double) subForm.Width != 0.0 && (double) subForm.m_currentPoint.X + (double) size.Width - (double) subForm.m_currentPosition.X > (double) subForm.Width - ((double) subForm.Margins.Left + (double) subForm.Margins.Right))
              {
                subForm.m_currentPoint.X = subForm.m_currentPosition.X;
                subForm.m_currentPoint.Y += subForm.m_maxSize.Height;
                subForm.m_height += subForm.m_maxSize.Height;
                subForm.m_maxSize.Height = 0.0f;
              }
              if ((double) subForm.m_currentPoint.Y + (double) size.Height > (double) this.m_pageSize.Height - ((double) subForm.Margins.Top + (double) subForm.Margins.Bottom))
              {
                this.m_page = this.m_page.Section.Pages.Add();
                this.m_pageSize = this.m_page.GetClientSize();
                subForm.m_currentPoint = subForm.m_startPoint;
                this.SetCurrentPoint(subForm);
                this.DrawBackground(subForm);
                subForm.m_height = 0.0f;
              }
              pdfXfaLine.SaveAcroForm(this.m_page, new RectangleF(subForm.m_currentPoint, size));
              subForm.m_currentPoint.X += size.Width;
            }
            else
            {
              subForm.m_currentPoint.X = subForm.m_currentPosition.X;
              if ((double) subForm.m_currentPoint.Y + (double) size.Height > (double) this.m_pageSize.Height - (double) subForm.Margins.Bottom)
              {
                this.m_page = this.m_page.Section.Pages.Add();
                this.m_pageSize = this.m_page.GetClientSize();
                subForm.m_currentPoint = subForm.m_startPoint;
                this.SetCurrentPoint(subForm);
                this.DrawBackground(subForm);
                subForm.m_height = 0.0f;
              }
              pdfXfaLine.SaveAcroForm(this.m_page, new RectangleF(subForm.m_currentPoint, size));
              subForm.m_currentPoint.Y += size.Height;
              subForm.m_height += size.Height;
            }
            if ((double) subForm.m_maxSize.Width < (double) size.Width)
              subForm.m_maxSize.Width = size.Width;
            if ((double) subForm.m_maxSize.Height < (double) size.Height)
            {
              subForm.m_maxSize.Height = size.Height;
              break;
            }
            break;
          }
          break;
        case PdfXfaCheckBoxField _:
          PdfXfaCheckBoxField xfaCheckBoxField = field1 as PdfXfaCheckBoxField;
          xfaCheckBoxField.parent = subForm;
          this.m_dataSetWriter.WriteStartElement(xfaCheckBoxField.Name);
          if (xfaCheckBoxField.IsChecked)
            this.m_dataSetWriter.WriteString("1");
          this.m_dataSetWriter.WriteEndElement();
          xfaCheckBoxField.Save(xfaWriter);
          this.SetFont((PdfDocumentBase) this.m_document, xfaCheckBoxField.Font);
          if (this.m_formType == PdfXfaType.Static)
          {
            SizeF size = xfaCheckBoxField.GetSize();
            if (subForm.FlowDirection == PdfXfaFlowDirection.Horizontal)
            {
              if ((double) subForm.Width != 0.0 && (double) subForm.m_currentPoint.X + (double) size.Width - (double) subForm.m_currentPosition.X > (double) subForm.Width - ((double) subForm.Margins.Left + (double) subForm.Margins.Right))
              {
                subForm.m_currentPoint.X = subForm.m_currentPosition.X;
                subForm.m_currentPoint.Y += subForm.m_maxSize.Height;
                subForm.m_height += subForm.m_maxSize.Height;
                subForm.m_maxSize.Height = 0.0f;
              }
              if ((double) subForm.m_currentPoint.Y + (double) size.Height > (double) this.m_pageSize.Height - ((double) subForm.Margins.Top + (double) subForm.Margins.Bottom))
              {
                this.m_page = this.m_page.Section.Pages.Add();
                this.m_pageSize = this.m_page.GetClientSize();
                subForm.m_currentPoint = subForm.m_startPoint;
                this.SetCurrentPoint(subForm);
                this.DrawBackground(subForm);
                subForm.m_height = 0.0f;
              }
              subForm.m_acroFields.Add(xfaCheckBoxField.SaveAcroForm(this.m_page, new RectangleF(subForm.m_currentPoint, size), subForm.GetFieldName(xfaCheckBoxField.Name)));
              subForm.m_currentPoint.X += size.Width;
            }
            else
            {
              subForm.m_currentPoint.X = subForm.m_currentPosition.X;
              if ((double) subForm.m_currentPoint.Y + (double) size.Height > (double) this.m_pageSize.Height - (double) subForm.Margins.Bottom)
              {
                this.m_page = this.m_page.Section.Pages.Add();
                this.m_pageSize = this.m_page.GetClientSize();
                subForm.m_currentPoint = subForm.m_startPoint;
                this.SetCurrentPoint(subForm);
                this.DrawBackground(subForm);
                subForm.m_height = 0.0f;
              }
              subForm.m_acroFields.Add(xfaCheckBoxField.SaveAcroForm(this.m_page, new RectangleF(subForm.m_currentPoint, size), subForm.GetFieldName(this.Name)));
              subForm.m_currentPoint.Y += size.Height;
              subForm.m_height += size.Height;
            }
            if ((double) subForm.m_maxSize.Width < (double) size.Width)
              subForm.m_maxSize.Width = size.Width;
            if ((double) subForm.m_maxSize.Height < (double) size.Height)
            {
              subForm.m_maxSize.Height = size.Height;
              break;
            }
            break;
          }
          break;
        case PdfXfaRadioButtonGroup _:
          PdfXfaRadioButtonGroup radioButtonGroup = field1 as PdfXfaRadioButtonGroup;
          string str = "group1";
          if (radioButtonGroup.Name != null && radioButtonGroup.Name != "")
            str = radioButtonGroup.Name;
          radioButtonGroup.parent = subForm;
          this.m_dataSetWriter.WriteStartElement(str);
          radioButtonGroup.Save(xfaWriter);
          if (radioButtonGroup.selectedItem > 0)
            this.m_dataSetWriter.WriteString(radioButtonGroup.selectedItem.ToString());
          this.m_dataSetWriter.WriteEndElement();
          foreach (PdfXfaStyledField radio in (List<PdfXfaRadioButtonField>) radioButtonGroup.m_radioList)
            this.SetFont((PdfDocumentBase) this.m_document, radio.Font);
          if (this.m_formType == PdfXfaType.Static)
          {
            PdfRadioButtonListField field2 = new PdfRadioButtonListField((PdfPageBase) this.m_page, subForm.GetFieldName(str));
            field2.isXfa = true;
            if (radioButtonGroup.ReadOnly || subForm.ReadOnly)
              field2.ReadOnly = true;
            float num = 0.0f;
            if ((double) subForm.Width != 0.0 && (double) subForm.m_currentPoint.X + (double) radioButtonGroup.Size.Width > (double) subForm.Width - ((double) subForm.Margins.Left + (double) subForm.Margins.Right))
            {
              subForm.m_currentPoint.X = subForm.m_startPoint.X;
              subForm.m_currentPoint.Y += subForm.m_maxSize.Height;
              subForm.m_height += subForm.m_maxSize.Height;
              subForm.m_maxSize.Height = 0.0f;
            }
            subForm.m_currentPoint.Y += radioButtonGroup.Margins.Top;
            subForm.m_currentPoint.X += radioButtonGroup.Margins.Left;
            foreach (PdfXfaRadioButtonField radio in (List<PdfXfaRadioButtonField>) radioButtonGroup.m_radioList)
            {
              radio.parent = (PdfXfaField) radioButtonGroup;
              SizeF size = radio.GetSize();
              if (subForm.FlowDirection == PdfXfaFlowDirection.Horizontal)
              {
                if ((double) subForm.Width != 0.0)
                {
                  if ((double) subForm.m_currentPoint.X + (double) size.Width - (double) subForm.m_startPoint.X > (double) subForm.Width - ((double) subForm.Margins.Left + (double) subForm.Margins.Right))
                  {
                    subForm.m_currentPoint.X = subForm.m_startPoint.X;
                    subForm.m_currentPoint.Y += subForm.m_maxSize.Height;
                    subForm.m_maxSize.Height = 0.0f;
                  }
                }
                else if ((double) subForm.m_currentPoint.X + (double) size.Width - (double) subForm.m_startPoint.X > (double) subForm.m_size.Width)
                {
                  subForm.m_currentPoint.X = subForm.m_startPoint.X;
                  subForm.m_currentPoint.Y += subForm.m_maxSize.Height;
                  subForm.m_maxSize.Height = 0.0f;
                }
                if ((double) subForm.m_currentPoint.Y + (double) size.Height > (double) this.m_page.GetClientSize().Height - ((double) subForm.Margins.Top + (double) subForm.Margins.Bottom))
                {
                  this.m_page = this.m_page.Section.Pages.Add();
                  subForm.m_currentPoint = subForm.m_startPoint;
                  this.SetCurrentPoint(subForm);
                  this.DrawBackground(subForm);
                  subForm.m_height = 0.0f;
                }
                field2.Items.Add(radio.SaveAcroForm(this.m_page, new RectangleF(subForm.m_currentPoint, new SizeF(size.Width, size.Height))));
                if (radioButtonGroup.FlowDirection == PdfXfaFlowDirection.Vertical)
                  subForm.m_currentPoint.Y += size.Height;
                else
                  subForm.m_currentPoint.X += size.Width;
              }
              else
              {
                if ((double) subForm.m_currentPoint.Y + (double) size.Height > (double) this.m_page.GetClientSize().Height - ((double) subForm.Margins.Top + (double) subForm.Margins.Bottom))
                {
                  this.m_page = this.m_page.Section.Pages.Add();
                  subForm.m_currentPoint = subForm.m_startPoint;
                  this.SetCurrentPoint(subForm);
                  this.DrawBackground(subForm);
                  subForm.m_height = 0.0f;
                }
                field2.Items.Add(radio.SaveAcroForm(subForm.m_page, new RectangleF(subForm.m_currentPoint, new SizeF(size.Width, size.Height))));
                if (radioButtonGroup.FlowDirection == PdfXfaFlowDirection.Vertical)
                  subForm.m_currentPoint.Y += size.Height;
                else
                  subForm.m_currentPoint.X += size.Width;
              }
              if ((double) subForm.m_maxSize.Width < (double) size.Width)
                subForm.m_maxSize.Width = size.Width;
              if ((double) subForm.m_maxSize.Height < (double) size.Height)
                subForm.m_maxSize.Height = size.Height;
              if ((double) num < (double) size.Width)
                num = size.Width;
            }
            subForm.m_acroFields.Add((PdfField) field2);
            if (radioButtonGroup.FlowDirection == PdfXfaFlowDirection.Vertical)
              subForm.m_currentPoint.X += num;
            if (subForm.FlowDirection == PdfXfaFlowDirection.Horizontal)
              subForm.m_currentPoint.Y -= radioButtonGroup.Size.Height;
            if ((double) subForm.m_maxSize.Height < (double) radioButtonGroup.Size.Height)
              subForm.m_maxSize.Height = radioButtonGroup.Size.Height;
            if (radioButtonGroup.selectedItem > 0)
            {
              field2.SelectedIndex = radioButtonGroup.selectedItem - 1;
              break;
            }
            break;
          }
          break;
        case PdfXfaButtonField _:
          PdfXfaButtonField pdfXfaButtonField = field1 as PdfXfaButtonField;
          pdfXfaButtonField.parent = subForm;
          pdfXfaButtonField.Save(xfaWriter);
          this.SetFont((PdfDocumentBase) this.m_document, pdfXfaButtonField.Font);
          if (this.m_formType == PdfXfaType.Static)
          {
            SizeF size = pdfXfaButtonField.GetSize();
            if (subForm.FlowDirection == PdfXfaFlowDirection.Horizontal)
            {
              if ((double) subForm.Width != 0.0 && (double) subForm.m_currentPoint.X + (double) size.Width - (double) subForm.m_currentPosition.X > (double) subForm.Width - ((double) subForm.Margins.Left + (double) subForm.Margins.Right))
              {
                subForm.m_currentPoint.X = subForm.m_currentPosition.X;
                subForm.m_currentPoint.Y += subForm.m_maxSize.Height;
                subForm.m_height += subForm.m_maxSize.Height;
                subForm.m_maxSize.Height = 0.0f;
              }
              if ((double) subForm.m_currentPoint.Y + (double) size.Height > (double) this.m_pageSize.Height - ((double) subForm.Margins.Top + (double) subForm.Margins.Bottom))
              {
                this.m_page = this.m_page.Section.Pages.Add();
                this.m_pageSize = this.m_page.GetClientSize();
                subForm.m_currentPoint = subForm.m_startPoint;
                this.SetCurrentPoint(subForm);
                this.DrawBackground(subForm);
                subForm.m_height = 0.0f;
              }
              subForm.m_acroFields.Add(pdfXfaButtonField.SaveAcroForm(this.m_page, new RectangleF(subForm.m_currentPoint, size), subForm.GetFieldName(pdfXfaButtonField.Name)));
              subForm.m_currentPoint.X += size.Width;
            }
            else
            {
              subForm.m_currentPoint.X = subForm.m_currentPosition.X;
              if ((double) subForm.m_currentPoint.Y + (double) size.Height > (double) this.m_pageSize.Height - (double) subForm.Margins.Bottom)
              {
                this.m_page = this.m_page.Section.Pages.Add();
                this.m_pageSize = this.m_page.GetClientSize();
                subForm.m_currentPoint = subForm.m_startPoint;
                this.SetCurrentPoint(subForm);
                this.DrawBackground(subForm);
                subForm.m_height = 0.0f;
              }
              subForm.m_acroFields.Add(pdfXfaButtonField.SaveAcroForm(this.m_page, new RectangleF(subForm.m_currentPoint, size), subForm.GetFieldName(this.Name)));
              subForm.m_currentPoint.Y += size.Height;
              subForm.m_height += size.Height;
            }
            if ((double) subForm.m_maxSize.Width < (double) size.Width)
              subForm.m_maxSize.Width = size.Width;
            if ((double) subForm.m_maxSize.Height < (double) size.Height)
            {
              subForm.m_maxSize.Height = size.Height;
              break;
            }
            break;
          }
          break;
        case PdfXfaListBoxField _:
          PdfXfaListBoxField pdfXfaListBoxField = field1 as PdfXfaListBoxField;
          pdfXfaListBoxField.parent = subForm;
          this.m_dataSetWriter.WriteStartElement(pdfXfaListBoxField.Name);
          if (pdfXfaListBoxField.SelectedValue != string.Empty)
            this.m_dataSetWriter.WriteString(pdfXfaListBoxField.SelectedValue);
          else if (pdfXfaListBoxField.SelectedIndex != -1 && pdfXfaListBoxField.SelectedIndex < pdfXfaListBoxField.Items.Count)
            this.m_dataSetWriter.WriteString(pdfXfaListBoxField.Items[pdfXfaListBoxField.SelectedIndex]);
          this.m_dataSetWriter.WriteEndElement();
          pdfXfaListBoxField.Save(xfaWriter);
          this.SetFont((PdfDocumentBase) this.m_document, pdfXfaListBoxField.Font);
          if (this.m_formType == PdfXfaType.Static)
          {
            SizeF size = pdfXfaListBoxField.GetSize();
            if (subForm.FlowDirection == PdfXfaFlowDirection.Horizontal)
            {
              if ((double) subForm.Width != 0.0 && (double) subForm.m_currentPoint.X + (double) size.Width - (double) subForm.m_currentPosition.X > (double) subForm.Width - ((double) subForm.Margins.Left + (double) subForm.Margins.Right))
              {
                subForm.m_currentPoint.X = subForm.m_currentPosition.X;
                subForm.m_currentPoint.Y += subForm.m_maxSize.Height;
                subForm.m_height += subForm.m_maxSize.Height;
                subForm.m_maxSize.Height = 0.0f;
              }
              if ((double) subForm.m_currentPoint.Y + (double) size.Height > (double) this.m_pageSize.Height - ((double) subForm.Margins.Top + (double) subForm.Margins.Bottom))
              {
                this.m_page = this.m_page.Section.Pages.Add();
                this.m_pageSize = this.m_page.GetClientSize();
                subForm.m_currentPoint = subForm.m_startPoint;
                this.SetCurrentPoint(subForm);
                this.DrawBackground(subForm);
                subForm.m_height = 0.0f;
              }
              subForm.m_acroFields.Add(pdfXfaListBoxField.SaveAcroForm(this.m_page, new RectangleF(subForm.m_currentPoint, size), subForm.GetFieldName(pdfXfaListBoxField.Name)));
              subForm.m_currentPoint.X += size.Width;
              subForm.m_height += size.Height;
            }
            else
            {
              subForm.m_currentPoint.X = subForm.m_currentPosition.X;
              if ((double) subForm.m_currentPoint.Y + (double) size.Height > (double) this.m_pageSize.Height - (double) subForm.Margins.Bottom)
              {
                this.m_page = this.m_page.Section.Pages.Add();
                this.m_pageSize = this.m_page.GetClientSize();
                subForm.m_currentPoint = subForm.m_startPoint;
                this.SetCurrentPoint(subForm);
                this.DrawBackground(subForm);
                subForm.m_height = 0.0f;
              }
              subForm.m_acroFields.Add(pdfXfaListBoxField.SaveAcroForm(this.m_page, new RectangleF(subForm.m_currentPoint, size), subForm.GetFieldName(this.Name)));
              subForm.m_currentPoint.Y += size.Height;
            }
            if ((double) subForm.m_maxSize.Width < (double) size.Width)
              subForm.m_maxSize.Width = size.Width;
            if ((double) subForm.m_maxSize.Height < (double) size.Height)
            {
              subForm.m_maxSize.Height = size.Height;
              break;
            }
            break;
          }
          break;
        case PdfXfaComboBoxField _:
          PdfXfaComboBoxField xfaComboBoxField = field1 as PdfXfaComboBoxField;
          xfaComboBoxField.parent = subForm;
          this.m_dataSetWriter.WriteStartElement(xfaComboBoxField.Name);
          if (xfaComboBoxField.SelectedValue != string.Empty)
          {
            if (xfaComboBoxField.Items.Contains(xfaComboBoxField.SelectedValue) || xfaComboBoxField.AllowTextEntry)
              this.m_dataSetWriter.WriteString(xfaComboBoxField.SelectedValue);
          }
          else if (xfaComboBoxField.SelectedIndex != -1 && xfaComboBoxField.SelectedIndex < xfaComboBoxField.Items.Count)
            this.m_dataSetWriter.WriteString(xfaComboBoxField.Items[xfaComboBoxField.SelectedIndex]);
          this.m_dataSetWriter.WriteEndElement();
          xfaComboBoxField.Save(xfaWriter);
          this.SetFont((PdfDocumentBase) this.m_document, xfaComboBoxField.Font);
          if (this.m_formType == PdfXfaType.Static)
          {
            SizeF size = xfaComboBoxField.GetSize();
            if (subForm.FlowDirection == PdfXfaFlowDirection.Horizontal)
            {
              if ((double) subForm.Width != 0.0 && (double) subForm.m_currentPoint.X + (double) size.Width - (double) subForm.m_currentPosition.X > (double) subForm.Width - ((double) subForm.Margins.Left + (double) subForm.Margins.Right))
              {
                subForm.m_currentPoint.X = subForm.m_currentPosition.X;
                subForm.m_currentPoint.Y += subForm.m_maxSize.Height;
                subForm.m_height += subForm.m_maxSize.Height;
                subForm.m_maxSize.Height = 0.0f;
              }
              if ((double) subForm.m_currentPoint.Y + (double) size.Height > (double) this.m_pageSize.Height - ((double) subForm.Margins.Top + (double) subForm.Margins.Bottom))
              {
                this.m_page = this.m_page.Section.Pages.Add();
                this.m_pageSize = this.m_page.GetClientSize();
                subForm.m_currentPoint = subForm.m_startPoint;
                this.SetCurrentPoint(subForm);
                this.DrawBackground(subForm);
                subForm.m_height = 0.0f;
              }
              subForm.m_acroFields.Add(xfaComboBoxField.SaveAcroForm(this.m_page, new RectangleF(subForm.m_currentPoint, size), subForm.GetFieldName(xfaComboBoxField.Name)));
              subForm.m_currentPoint.X += size.Width;
            }
            else
            {
              subForm.m_currentPoint.X = subForm.m_currentPosition.X;
              if ((double) subForm.m_currentPoint.Y + (double) size.Height > (double) this.m_pageSize.Height - (double) subForm.Margins.Bottom)
              {
                this.m_page = this.m_page.Section.Pages.Add();
                this.m_pageSize = this.m_page.GetClientSize();
                subForm.m_currentPoint = subForm.m_startPoint;
                this.SetCurrentPoint(subForm);
                this.DrawBackground(subForm);
                subForm.m_height = 0.0f;
              }
              subForm.m_acroFields.Add(xfaComboBoxField.SaveAcroForm(this.m_page, new RectangleF(subForm.m_currentPoint, size), subForm.GetFieldName(this.Name)));
              subForm.m_currentPoint.Y += size.Height;
              subForm.m_height += size.Height;
            }
            if ((double) subForm.m_maxSize.Width < (double) size.Width)
              subForm.m_maxSize.Width = size.Width;
            if ((double) subForm.m_maxSize.Height < (double) size.Height)
            {
              subForm.m_maxSize.Height = size.Height;
              break;
            }
            break;
          }
          break;
        case PdfXfaDateTimeField _:
          PdfXfaDateTimeField xfaDateTimeField = field1 as PdfXfaDateTimeField;
          xfaDateTimeField.parent = subForm;
          DateTime dateTime = xfaDateTimeField.Value;
          this.m_dataSetWriter.WriteStartElement(xfaDateTimeField.Name);
          if (xfaDateTimeField.isSet)
          {
            if (xfaDateTimeField.Format == PdfXfaDateTimeFormat.Date)
              this.m_dataSetWriter.WriteString(xfaDateTimeField.Value.ToString(xfaWriter.GetDatePattern(xfaDateTimeField.DatePattern)));
            else if (xfaDateTimeField.Format == PdfXfaDateTimeFormat.DateTime)
              this.m_dataSetWriter.WriteString(xfaDateTimeField.Value.ToString(xfaWriter.GetDateTimePattern(xfaDateTimeField.DatePattern, xfaDateTimeField.TimePattern)));
            else if (xfaDateTimeField.Format == PdfXfaDateTimeFormat.Time)
              this.m_dataSetWriter.WriteString(xfaDateTimeField.Value.ToString(xfaWriter.GetTimePattern(xfaDateTimeField.TimePattern)));
          }
          this.m_dataSetWriter.WriteEndElement();
          xfaDateTimeField.Save(xfaWriter);
          this.SetFont((PdfDocumentBase) this.m_document, xfaDateTimeField.Font);
          if (this.m_formType == PdfXfaType.Static)
          {
            SizeF size = xfaDateTimeField.GetSize();
            if (subForm.FlowDirection == PdfXfaFlowDirection.Horizontal)
            {
              if ((double) subForm.Width != 0.0 && (double) subForm.m_currentPoint.X + (double) size.Width - (double) subForm.m_currentPosition.X > (double) subForm.Width - ((double) subForm.Margins.Left + (double) subForm.Margins.Right))
              {
                subForm.m_currentPoint.X = subForm.m_currentPosition.X;
                subForm.m_currentPoint.Y += subForm.m_maxSize.Height;
                subForm.m_height += subForm.m_maxSize.Height;
                subForm.m_maxSize.Height = 0.0f;
              }
              if ((double) subForm.m_currentPoint.Y + (double) size.Height > (double) this.m_pageSize.Height - ((double) subForm.Margins.Top + (double) subForm.Margins.Bottom))
              {
                this.m_page = this.m_page.Section.Pages.Add();
                this.m_pageSize = this.m_page.GetClientSize();
                subForm.m_currentPoint = subForm.m_startPoint;
                this.SetCurrentPoint(subForm);
                this.DrawBackground(subForm);
                subForm.m_height = 0.0f;
              }
              subForm.m_acroFields.Add(xfaDateTimeField.SaveAcroForm(this.m_page, new RectangleF(subForm.m_currentPoint, size), subForm.GetFieldName(xfaDateTimeField.Name)));
              subForm.m_currentPoint.X += size.Width;
            }
            else
            {
              subForm.m_currentPoint.X = subForm.m_currentPosition.X;
              if ((double) subForm.m_currentPoint.Y + (double) size.Height > (double) this.m_pageSize.Height - (double) subForm.Margins.Bottom)
              {
                this.m_page = this.m_page.Section.Pages.Add();
                this.m_pageSize = this.m_page.GetClientSize();
                subForm.m_currentPoint = subForm.m_startPoint;
                this.SetCurrentPoint(subForm);
                this.DrawBackground(subForm);
                subForm.m_height = 0.0f;
              }
              subForm.m_acroFields.Add(xfaDateTimeField.SaveAcroForm(this.m_page, new RectangleF(subForm.m_currentPoint, size), subForm.GetFieldName(this.Name)));
              subForm.m_currentPoint.Y += size.Height;
              subForm.m_height += size.Height;
            }
            if ((double) subForm.m_maxSize.Width < (double) size.Width)
              subForm.m_maxSize.Width = size.Width;
            if ((double) subForm.m_maxSize.Height < (double) size.Height)
            {
              subForm.m_maxSize.Height = size.Height;
              break;
            }
            break;
          }
          break;
        case PdfXfaCircleField _:
          PdfXfaCircleField pdfXfaCircleField = field1 as PdfXfaCircleField;
          pdfXfaCircleField.parent = subForm;
          pdfXfaCircleField.Save(xfaWriter);
          if (this.m_formType == PdfXfaType.Static)
          {
            SizeF size = pdfXfaCircleField.GetSize();
            if (subForm.FlowDirection == PdfXfaFlowDirection.Horizontal)
            {
              if ((double) subForm.Width != 0.0 && (double) subForm.m_currentPoint.X + (double) size.Width - (double) subForm.m_currentPosition.X > (double) subForm.Width - ((double) subForm.Margins.Left + (double) subForm.Margins.Right))
              {
                subForm.m_currentPoint.X = subForm.m_currentPosition.X;
                subForm.m_currentPoint.Y += subForm.m_maxSize.Height;
                subForm.m_height += subForm.m_maxSize.Height;
                subForm.m_maxSize.Height = 0.0f;
              }
              if ((double) subForm.m_currentPoint.Y + (double) size.Height > (double) this.m_pageSize.Height - ((double) subForm.Margins.Top + (double) subForm.Margins.Bottom))
              {
                this.m_page = this.m_page.Section.Pages.Add();
                this.m_pageSize = this.m_page.GetClientSize();
                subForm.m_currentPoint = subForm.m_startPoint;
                this.SetCurrentPoint(subForm);
                this.DrawBackground(subForm);
                subForm.m_height = 0.0f;
              }
              pdfXfaCircleField.SaveAcroForm(this.m_page, new RectangleF(subForm.m_currentPoint, size));
              subForm.m_currentPoint.X += size.Width;
            }
            else
            {
              subForm.m_currentPoint.X = subForm.m_currentPosition.X;
              if ((double) subForm.m_currentPoint.Y + (double) size.Height > (double) this.m_pageSize.Height - (double) subForm.Margins.Bottom)
              {
                this.m_page = this.m_page.Section.Pages.Add();
                this.m_pageSize = this.m_page.GetClientSize();
                subForm.m_currentPoint = subForm.m_startPoint;
                this.SetCurrentPoint(subForm);
                this.DrawBackground(subForm);
                subForm.m_height = 0.0f;
              }
              pdfXfaCircleField.SaveAcroForm(this.m_page, new RectangleF(subForm.m_currentPoint, size));
              subForm.m_currentPoint.Y += size.Height;
              subForm.m_height += size.Height;
            }
            if ((double) subForm.m_maxSize.Width < (double) size.Width)
              subForm.m_maxSize.Width = size.Width;
            if ((double) subForm.m_maxSize.Height < (double) size.Height)
            {
              subForm.m_maxSize.Height = size.Height;
              break;
            }
            break;
          }
          break;
        case PdfXfaRectangleField _:
          PdfXfaRectangleField xfaRectangleField = field1 as PdfXfaRectangleField;
          xfaRectangleField.parent = subForm;
          xfaRectangleField.Save(xfaWriter);
          if (this.m_formType == PdfXfaType.Static)
          {
            SizeF size = xfaRectangleField.GetSize();
            if (subForm.FlowDirection == PdfXfaFlowDirection.Horizontal)
            {
              if ((double) subForm.Width != 0.0 && (double) subForm.m_currentPoint.X + (double) size.Width - (double) subForm.m_currentPosition.X > (double) subForm.Width - ((double) subForm.Margins.Left + (double) subForm.Margins.Right))
              {
                subForm.m_currentPoint.X = subForm.m_currentPosition.X;
                subForm.m_currentPoint.Y += subForm.m_maxSize.Height;
                subForm.m_height += subForm.m_maxSize.Height;
                subForm.m_maxSize.Height = 0.0f;
              }
              if ((double) subForm.m_currentPoint.Y + (double) size.Height > (double) this.m_pageSize.Height - ((double) subForm.Margins.Top + (double) subForm.Margins.Bottom))
              {
                this.m_page = this.m_page.Section.Pages.Add();
                this.m_pageSize = this.m_page.GetClientSize();
                subForm.m_currentPoint = subForm.m_startPoint;
                this.SetCurrentPoint(subForm);
                this.DrawBackground(subForm);
                subForm.m_height = 0.0f;
              }
              xfaRectangleField.SaveAcroForm(this.m_page, new RectangleF(subForm.m_currentPoint, size));
              subForm.m_currentPoint.X += size.Width;
            }
            else
            {
              subForm.m_currentPoint.X = subForm.m_currentPosition.X;
              if ((double) subForm.m_currentPoint.Y + (double) size.Height > (double) this.m_pageSize.Height - (double) subForm.Margins.Bottom)
              {
                this.m_page = this.m_page.Section.Pages.Add();
                this.m_pageSize = this.m_page.GetClientSize();
                subForm.m_currentPoint = subForm.m_startPoint;
                this.SetCurrentPoint(subForm);
                this.DrawBackground(subForm);
                subForm.m_height = 0.0f;
              }
              xfaRectangleField.SaveAcroForm(this.m_page, new RectangleF(subForm.m_currentPoint, size));
              subForm.m_currentPoint.Y += size.Height;
              subForm.m_height += size.Height;
            }
            if ((double) subForm.m_maxSize.Width < (double) size.Width)
              subForm.m_maxSize.Width = size.Width;
            if ((double) subForm.m_maxSize.Height < (double) size.Height)
            {
              subForm.m_maxSize.Height = size.Height;
              break;
            }
            break;
          }
          break;
        default:
          throw new NotImplementedException();
      }
    }
  }

  internal string GetFieldName(string name)
  {
    string fieldName = name + "[0]";
    if (this.m_fieldNames.Count > 0)
    {
      int num = 0;
      for (; this.m_fieldNames.Contains(fieldName); fieldName = $"{name}[{num.ToString()}]")
        ++num;
    }
    this.m_fieldNames.Add(fieldName);
    return fieldName;
  }

  internal string GetSubFormName(string name)
  {
    string subFormName = name + "[0]";
    if (this.m_subFormNames.Count > 0)
    {
      int num = 0;
      for (; this.m_subFormNames.Contains(subFormName); subFormName = $"{name}[{num.ToString()}]")
        ++num;
    }
    this.m_subFormNames.Add(subFormName);
    return subFormName;
  }

  internal void AddSubForm(XfaWriter xfaWriter)
  {
    string empty = string.Empty;
    string localName = this.Name == null || !(this.Name != "") ? "subform" + xfaWriter.m_subFormFieldCount++.ToString() : this.Name;
    this.m_dataSetWriter.WriteStartElement(localName);
    xfaWriter.Write.WriteStartElement("subform");
    xfaWriter.Write.WriteAttributeString("name", localName);
    xfaWriter.SetSize(0.0f, this.Width, 0.0f, 0.0f);
    if (this.FlowDirection == PdfXfaFlowDirection.Horizontal)
      xfaWriter.Write.WriteAttributeString("layout", "lr-tb");
    else
      xfaWriter.Write.WriteAttributeString("layout", "tb");
    if (this.ReadOnly)
      xfaWriter.Write.WriteAttributeString("access", "readOnly");
    if (this.m_xfaPage != null && !this.m_xfaPage.isAdded)
      this.m_xfaPage.Save(xfaWriter);
    xfaWriter.WriteMargins(this.Margins);
    this.m_formType = PdfXfaType.Dynamic;
    this.AddSubForm(this, xfaWriter);
    xfaWriter.Write.WriteEndElement();
    this.m_dataSetWriter.WriteEndElement();
  }

  internal void BreakPage(XfaWriter writer, PdfXfaPage page)
  {
    writer.Write.WriteStartElement("breakBefore");
    writer.Write.WriteAttributeString("targetType", "pageArea");
    writer.Write.WriteAttributeString("target", "Page" + page.pageId.ToString());
    writer.Write.WriteAttributeString("startNew", "1");
    writer.Write.WriteEndElement();
    page.isBreaked = true;
  }

  private void GetBackgroundHeight(PdfXfaForm subForm)
  {
    for (int offset = 0; offset < subForm.Fields.Count; ++offset)
    {
      object field = (object) subForm.Fields[offset];
      if (field is PdfXfaForm)
      {
        PdfXfaForm subForm1 = field as PdfXfaForm;
        subForm1.m_startPoint = new PointF(subForm.m_startPoint.X + subForm1.Margins.Left, subForm.m_startPoint.Y + subForm1.Margins.Top);
        subForm1.m_currentPoint = subForm1.m_currentPosition = subForm1.m_startPoint;
        SizeF sizeF = new SizeF((double) subForm1.Width != 0.0 ? subForm1.Width : subForm1.m_size.Width, subForm1.m_size.Height);
        if (subForm.FlowDirection == PdfXfaFlowDirection.Horizontal && (double) subForm.Width != 0.0 && (double) subForm.m_currentPoint.X + (double) sizeF.Width > (double) subForm.Width)
        {
          subForm.m_currentPoint.X = subForm.m_currentPosition.X;
          subForm.m_currentPoint.Y += subForm.m_maxSize.Height;
          subForm.m_height += subForm.m_maxSize.Height;
          subForm.m_maxSize.Height = 0.0f;
        }
        subForm1.m_currentPoint.X += subForm.m_currentPoint.X - subForm.m_startPoint.X;
        if ((double) subForm1.m_currentPoint.Y < (double) subForm.m_currentPoint.Y)
          subForm1.m_currentPoint.Y += subForm.m_currentPoint.Y - subForm.m_startPoint.Y;
        if (subForm1.m_xfaPage != null && this.m_formType == PdfXfaType.Static)
        {
          this.m_tempPage = this.m_xfaPage;
          subForm1.m_startPoint = new PointF(subForm.m_startPoint.X + subForm1.Margins.Left, subForm.m_startPoint.Y + subForm1.Margins.Top);
          subForm1.m_currentPoint = subForm1.m_startPoint;
          subForm.m_borderHeight.Add(subForm.m_currentPoint.Y + subForm.m_maxSize.Height + subForm.Margins.Bottom);
        }
        subForm1.m_currentPosition = subForm1.m_currentPoint;
        this.GetBackgroundHeight(subForm1);
        if (this.m_formType == PdfXfaType.Static)
        {
          subForm.m_currentPoint.X += sizeF.Width;
          if (subForm.FlowDirection == PdfXfaFlowDirection.Vertical)
          {
            subForm.m_currentPoint.X = subForm.m_startPoint.X;
            if (subForm1.FlowDirection == PdfXfaFlowDirection.Horizontal)
              subForm1.m_height += subForm1.m_maxSize.Height + subForm1.Margins.Bottom + subForm1.Margins.Top;
            else
              subForm1.m_height += subForm1.Margins.Bottom + subForm1.Margins.Top;
            if ((double) subForm1.m_height - (double) subForm1.Margins.Bottom != 0.0)
              subForm.m_currentPoint.Y += subForm1.m_height;
            else
              subForm.m_currentPoint.Y = subForm1.m_currentPoint.Y;
            subForm.m_maxSize.Height = 0.0f;
          }
          else
          {
            if (subForm1.FlowDirection == PdfXfaFlowDirection.Horizontal)
              subForm1.m_height += subForm1.m_maxSize.Height + subForm1.Margins.Bottom + subForm1.Margins.Top;
            else
              subForm1.m_height += subForm1.Margins.Bottom + subForm1.Margins.Top;
            if ((double) subForm1.m_height - (double) subForm1.Margins.Bottom != 0.0)
            {
              if ((double) subForm.m_maxSize.Height < (double) subForm1.m_height)
                subForm.m_maxSize.Height = subForm1.m_height;
            }
            else if ((double) subForm.m_maxSize.Height < (double) subForm1.m_currentPoint.Y)
              subForm.m_maxSize.Height = subForm1.m_currentPoint.Y;
          }
        }
        if (subForm1.FlowDirection == PdfXfaFlowDirection.Horizontal)
          subForm1.m_borderHeight.Add(subForm1.m_currentPoint.Y + subForm1.Margins.Bottom + subForm1.m_maxSize.Height);
        else
          subForm1.m_borderHeight.Add(subForm1.m_currentPoint.Y + subForm1.Margins.Bottom);
        subForm1.m_currentPoint = subForm1.m_startPoint = subForm1.m_currentPosition = PointF.Empty;
        subForm1.m_maxSize = SizeF.Empty;
        subForm1.m_height = 0.0f;
      }
      else
      {
        SizeF sizeF = SizeF.Empty;
        if (field is PdfXfaTextBoxField)
          sizeF = (field as PdfXfaTextBoxField).GetSize();
        else if (field is PdfXfaNumericField)
          sizeF = (field as PdfXfaNumericField).GetSize();
        else if (field is PdfXfaTextElement)
          sizeF = (field as PdfXfaTextElement).GetSize();
        else if (field is PdfXfaCheckBoxField)
          sizeF = (field as PdfXfaCheckBoxField).GetSize();
        else if (field is PdfXfaListBoxField)
          sizeF = (field as PdfXfaListBoxField).GetSize();
        else if (field is PdfXfaComboBoxField)
          sizeF = (field as PdfXfaComboBoxField).GetSize();
        else if (field is PdfXfaButtonField)
          sizeF = (field as PdfXfaButtonField).GetSize();
        else if (field is PdfXfaCircleField)
          sizeF = (field as PdfXfaCircleField).GetSize();
        else if (field is PdfXfaRectangleField)
          sizeF = (field as PdfXfaRectangleField).GetSize();
        else if (field is PdfXfaLine)
          sizeF = (field as PdfXfaLine).GetSize();
        else if (field is PdfXfaDateTimeField)
          sizeF = (field as PdfXfaDateTimeField).GetSize();
        else if (field is PdfXfaImage)
          sizeF = (field as PdfXfaImage).GetSize();
        else if (field is PdfXfaRadioButtonGroup)
          sizeF = (field as PdfXfaRadioButtonGroup).Size;
        if (subForm.FlowDirection == PdfXfaFlowDirection.Horizontal)
        {
          if ((double) subForm.Width != 0.0 && (double) subForm.m_currentPoint.X + (double) sizeF.Width - (double) subForm.m_currentPosition.X > (double) subForm.Width - ((double) subForm.Margins.Left + (double) subForm.Margins.Right))
          {
            subForm.m_currentPoint.X = subForm.m_currentPosition.X;
            subForm.m_currentPoint.Y += subForm.m_maxSize.Height;
            subForm.m_height += subForm.m_maxSize.Height;
            subForm.m_maxSize.Height = 0.0f;
          }
          if ((double) subForm.m_currentPoint.Y + (double) sizeF.Height > (double) this.m_tempPage.GetClientSize().Height - ((double) subForm.Margins.Top + (double) subForm.Margins.Bottom))
          {
            this.SetBackgroundHeight(subForm, subForm.m_currentPoint.Y);
            subForm.m_currentPoint = subForm.m_startPoint;
            this.SetCurrentPoint(subForm);
            subForm.m_height = 0.0f;
          }
          subForm.m_currentPoint.X += sizeF.Width;
        }
        else
        {
          subForm.m_currentPoint.X = subForm.m_currentPosition.X;
          if ((double) subForm.m_currentPoint.Y + (double) sizeF.Height > (double) this.m_tempPage.GetClientSize().Height - (double) subForm.Margins.Bottom)
          {
            this.SetBackgroundHeight(subForm, subForm.m_currentPoint.Y);
            subForm.m_currentPoint = subForm.m_startPoint;
            this.SetCurrentPoint(subForm);
            subForm.m_height = 0.0f;
          }
          subForm.m_currentPoint.Y += sizeF.Height;
          subForm.m_height += sizeF.Height;
        }
        if ((double) subForm.m_maxSize.Width < (double) sizeF.Width)
          subForm.m_maxSize.Width = sizeF.Width;
        if ((double) subForm.m_maxSize.Height < (double) sizeF.Height)
          subForm.m_maxSize.Height = sizeF.Height;
      }
    }
  }

  private void DrawBackground(PdfXfaForm tempForm)
  {
    if (tempForm.m_parent != null)
      this.DrawBackground(tempForm.m_parent);
    if (tempForm.Border == null)
      return;
    PdfPen pen = tempForm.Border.GetPen();
    RectangleF rectangleF = new RectangleF();
    float width = tempForm.Width;
    if ((double) tempForm.Width == 0.0)
      width = tempForm.m_size.Width + tempForm.Margins.Left + tempForm.Margins.Right;
    rectangleF = new RectangleF(tempForm.m_currentPoint.X - tempForm.Margins.Left, tempForm.m_currentPoint.Y - tempForm.Margins.Top, width, tempForm.m_borderHeight[tempForm.m_borderCount] - tempForm.m_currentPoint.Y);
    ++tempForm.m_borderCount;
    if (tempForm.m_parent == null)
      rectangleF.Height += this.m_currentPoint.Y;
    if (tempForm.m_borderHeight.Count == tempForm.m_borderCount && tempForm.m_parent == null)
      rectangleF.Height += tempForm.Margins.Bottom;
    if (tempForm.Border.LeftEdge != null || tempForm.Border.RightEdge != null || tempForm.Border.TopEdge != null || tempForm.Border.BottomEdge != null)
    {
      this.m_page.Graphics.DrawRectangle(tempForm.Border.GetBrush(rectangleF), rectangleF);
      if (tempForm.Border.LeftEdge != null)
        this.DrawEdge(tempForm.Border.LeftEdge, rectangleF.Location, new PointF(rectangleF.X, rectangleF.Y + rectangleF.Height), this.m_page);
      if (tempForm.Border.RightEdge != null)
        this.DrawEdge(tempForm.Border.RightEdge, new PointF(rectangleF.X + rectangleF.Width, rectangleF.Y), new PointF(rectangleF.X + rectangleF.Width, rectangleF.Y + rectangleF.Height), this.m_page);
      if (tempForm.Border.TopEdge != null)
        this.DrawEdge(tempForm.Border.TopEdge, rectangleF.Location, new PointF(rectangleF.X + rectangleF.Width, rectangleF.Y), this.m_page);
      if (tempForm.Border.BottomEdge == null)
        return;
      this.DrawEdge(tempForm.Border.BottomEdge, new PointF(rectangleF.X, rectangleF.Y + rectangleF.Height), new PointF(rectangleF.X + rectangleF.Width, rectangleF.Y + rectangleF.Height), this.m_page);
    }
    else
      this.m_page.Graphics.DrawRectangle(pen, tempForm.Border.GetBrush(rectangleF), rectangleF);
  }

  private PdfPage AddPdfPage(PdfXfaPage xfaPage)
  {
    PdfSection pdfSection = this.m_document.Sections.Add();
    pdfSection.PageSettings.Margins.Left = xfaPage.pageSettings.Margins.Left;
    pdfSection.PageSettings.Margins.Right = xfaPage.pageSettings.Margins.Right;
    pdfSection.PageSettings.Margins.Top = xfaPage.pageSettings.Margins.Top;
    pdfSection.PageSettings.Margins.Bottom = xfaPage.pageSettings.Margins.Bottom;
    pdfSection.PageSettings.Size = new SizeF(xfaPage.pageSettings.PageSize.Width, xfaPage.pageSettings.PageSize.Height);
    if (xfaPage.pageSettings.PageOrientation == PdfXfaPageOrientation.Landscape)
      pdfSection.PageSettings.Orientation = PdfPageOrientation.Landscape;
    return pdfSection.Pages.Add();
  }

  private void SetCurrentPoint(PdfXfaForm subForm)
  {
    if (subForm.m_parent != null)
      this.SetCurrentPoint(subForm.m_parent);
    subForm.m_currentPoint = subForm.m_startPoint;
    subForm.m_height = 0.0f;
  }

  private void SetBackgroundHeight(PdfXfaForm subForm, float height)
  {
    height += subForm.Margins.Bottom;
    if (subForm.m_parent != null)
      this.SetBackgroundHeight(subForm.m_parent, height);
    subForm.m_borderHeight.Add(height);
  }

  private void ParseSubForm(PdfXfaForm subform)
  {
    float num1 = 0.0f;
    if ((double) subform.Width != 0.0)
      num1 = subform.Width - (subform.Margins.Left + subform.Margins.Right);
    for (int offset = 0; offset < subform.Fields.Count; ++offset)
    {
      object field = (object) subform.Fields[offset];
      if (field is PdfXfaForm)
      {
        PdfXfaForm subform1 = field as PdfXfaForm;
        subform1.m_parent = subform;
        subform1.m_size = SizeF.Empty;
        subform1.m_maximumSize = SizeF.Empty;
        this.ParseSubForm(subform1);
        if (subform.FlowDirection == PdfXfaFlowDirection.Vertical)
        {
          subform1.m_size.Height += subform1.m_maximumSize.Height;
          subform1.m_size.Height += subform1.Margins.Top + subform1.Margins.Bottom;
          subform.m_size.Height += subform1.m_size.Height;
          if ((double) subform1.Width != 0.0)
          {
            if ((double) subform.m_size.Width < (double) subform1.Width)
              subform.m_size.Width = subform1.Width;
          }
          else if ((double) subform.m_size.Width < (double) subform1.m_size.Width)
            subform.m_size.Width = subform1.m_size.Width;
        }
        else
        {
          subform1.m_size.Height += subform1.m_maximumSize.Height;
          subform1.m_size.Height += subform1.Margins.Top + subform1.Margins.Bottom;
          if ((double) subform.Width == 0.0)
          {
            subform.m_size.Width += subform1.Width;
            if ((double) subform.m_maximumSize.Height < (double) subform1.m_size.Height)
              subform.m_maximumSize.Height = subform1.m_size.Height;
            if ((double) subform1.Width == 0.0)
              subform.m_size.Width += subform1.m_size.Width;
          }
          else if ((double) subform1.Width != 0.0)
          {
            if ((double) subform.m_size.Width + (double) subform1.Width >= (double) subform.Width)
            {
              subform.m_size.Height += subform1.m_size.Height;
            }
            else
            {
              subform.m_size.Width += subform1.Width;
              if ((double) subform.m_maximumSize.Height < (double) subform1.m_size.Height)
                subform.m_maximumSize.Height = subform1.m_size.Height;
            }
          }
          else
          {
            if ((double) subform.m_maximumSize.Height != 0.0)
            {
              subform.m_size.Height += subform.m_maximumSize.Height;
              subform.m_maximumSize.Height = 0.0f;
            }
            subform.m_size.Height += subform1.m_size.Height;
            if ((double) subform1.m_size.Width < (double) subform1.m_maximumSize.Width)
              subform1.m_size.Width = subform1.m_maximumSize.Width;
          }
        }
      }
      else
      {
        SizeF sizeF = SizeF.Empty;
        if (field is PdfXfaTextBoxField)
          sizeF = (field as PdfXfaTextBoxField).GetSize();
        else if (field is PdfXfaNumericField)
          sizeF = (field as PdfXfaNumericField).GetSize();
        else if (field is PdfXfaTextElement)
          sizeF = (field as PdfXfaTextElement).GetSize();
        else if (field is PdfXfaCheckBoxField)
          sizeF = (field as PdfXfaCheckBoxField).GetSize();
        else if (field is PdfXfaListBoxField)
          sizeF = (field as PdfXfaListBoxField).GetSize();
        else if (field is PdfXfaComboBoxField)
          sizeF = (field as PdfXfaComboBoxField).GetSize();
        else if (field is PdfXfaButtonField)
          sizeF = (field as PdfXfaButtonField).GetSize();
        else if (field is PdfXfaCircleField)
          sizeF = (field as PdfXfaCircleField).GetSize();
        else if (field is PdfXfaRectangleField)
          sizeF = (field as PdfXfaRectangleField).GetSize();
        else if (field is PdfXfaLine)
          sizeF = (field as PdfXfaLine).GetSize();
        else if (field is PdfXfaDateTimeField)
          sizeF = (field as PdfXfaDateTimeField).GetSize();
        else if (field is PdfXfaImage)
          sizeF = (field as PdfXfaImage).GetSize();
        else if (field is PdfXfaRadioButtonGroup)
          sizeF = (field as PdfXfaRadioButtonGroup).Size;
        if (field is PdfXfaRadioButtonGroup)
        {
          PdfXfaRadioButtonGroup radioButtonGroup = field as PdfXfaRadioButtonGroup;
          float num2 = 0.0f;
          float num3 = 0.0f;
          foreach (PdfXfaStyledField radio in (List<PdfXfaRadioButtonField>) radioButtonGroup.m_radioList)
          {
            SizeF size = radio.GetSize();
            if (radioButtonGroup.FlowDirection == PdfXfaFlowDirection.Vertical)
            {
              radioButtonGroup.Size.Height += size.Height;
              if ((double) radioButtonGroup.Size.Width < (double) size.Width)
                radioButtonGroup.Size.Width = size.Width;
            }
            else if ((double) subform.Width == 0.0)
            {
              num3 += size.Width;
              if ((double) num2 < (double) size.Height)
                num2 = size.Height;
            }
            else if ((double) num3 + (double) size.Width > (double) subform.Width)
            {
              radioButtonGroup.Size.Height += num2;
              if ((double) radioButtonGroup.Size.Width < (double) num3)
                radioButtonGroup.Size.Width = num3;
              if ((double) radioButtonGroup.Size.Width < (double) num3)
                radioButtonGroup.Size.Width = num3;
              num3 = size.Width;
            }
            else
            {
              num3 += size.Width;
              if ((double) num2 < (double) size.Height)
                num2 = size.Height;
            }
          }
          if (radioButtonGroup.FlowDirection == PdfXfaFlowDirection.Horizontal)
          {
            radioButtonGroup.Size.Height += num2;
            if ((double) radioButtonGroup.Size.Width < (double) num3)
              radioButtonGroup.Size.Width = num3;
            if ((double) subform.Width == 0.0)
            {
              subform.m_size.Width += radioButtonGroup.Size.Width;
              if ((double) subform.m_maximumSize.Height < (double) radioButtonGroup.Size.Height)
                subform.m_maximumSize.Height = radioButtonGroup.Size.Height;
            }
            else if ((double) subform.m_size.Width + (double) radioButtonGroup.Size.Width > (double) subform.Width)
            {
              subform.m_size.Height += subform.m_maximumSize.Height;
              if ((double) subform.m_maximumSize.Width < (double) subform.m_size.Width)
                subform.m_maximumSize.Width = subform.m_size.Width;
              subform.m_size.Width = radioButtonGroup.Size.Width;
              subform.m_maximumSize.Height = 0.0f;
              if ((double) subform.m_maximumSize.Height < (double) radioButtonGroup.Size.Height)
                subform.m_maximumSize.Height = radioButtonGroup.Size.Height;
            }
            else
            {
              subform.m_size.Width += radioButtonGroup.Size.Width;
              if ((double) subform.m_maximumSize.Height < (double) radioButtonGroup.Size.Height)
                subform.m_maximumSize.Height = radioButtonGroup.Size.Height;
            }
          }
          else
          {
            subform.m_size.Height += radioButtonGroup.Size.Height;
            if ((double) subform.m_maximumSize.Width < (double) radioButtonGroup.Size.Width)
              subform.m_maximumSize.Width = radioButtonGroup.Size.Width;
          }
        }
        else if (subform.FlowDirection == PdfXfaFlowDirection.Vertical)
        {
          subform.m_size.Height += sizeF.Height;
          if ((double) subform.m_maximumSize.Width < (double) sizeF.Width)
            subform.m_maximumSize.Width = sizeF.Width;
        }
        else if ((double) subform.Width == 0.0)
        {
          subform.m_size.Width += sizeF.Width;
          if ((double) subform.m_maximumSize.Height < (double) sizeF.Height)
            subform.m_maximumSize.Height = sizeF.Height;
        }
        else if ((double) subform.m_size.Width + (double) sizeF.Width > (double) num1)
        {
          subform.m_size.Height += subform.m_maximumSize.Height;
          if ((double) subform.m_maximumSize.Width < (double) subform.m_size.Width)
            subform.m_maximumSize.Width = subform.m_size.Width;
          subform.m_size.Width = sizeF.Width;
          subform.m_maximumSize.Height = 0.0f;
          if ((double) subform.m_maximumSize.Height < (double) sizeF.Height)
            subform.m_maximumSize.Height = sizeF.Height;
        }
        else
        {
          subform.m_size.Width += sizeF.Width;
          if ((double) subform.m_maximumSize.Height < (double) sizeF.Height)
            subform.m_maximumSize.Height = sizeF.Height;
        }
      }
    }
  }

  private void Message()
  {
    PdfPage pdfPage = this.m_document.Pages.Add();
    PdfFont font1 = (PdfFont) new PdfStandardFont(PdfFontFamily.TimesRoman, 20f, PdfFontStyle.Bold);
    PdfFont font2 = (PdfFont) new PdfStandardFont(PdfFontFamily.TimesRoman, 12f, PdfFontStyle.Regular);
    pdfPage.Graphics.DrawString("Please wait...", font1, PdfBrushes.Black, new PointF(0.0f, 0.0f));
    pdfPage.Graphics.DrawString("If this message is not eventually replaced by the proper contents of the document, your PDF viewer may not be able to display this type of document.", font2, PdfBrushes.Black, new RectangleF(0.0f, 40f, 515f, 100f));
  }

  public object Clone()
  {
    PdfXfaForm pdfXfaForm = (PdfXfaForm) this.MemberwiseClone();
    pdfXfaForm.Fields = (PdfXfaFieldCollection) this.Fields.Clone();
    pdfXfaForm.m_acroFields = new PdfFieldCollection();
    pdfXfaForm.m_borderHeight = new List<float>();
    return (object) pdfXfaForm;
  }
}
