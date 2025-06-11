// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfXfaFlattener
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

internal class PdfXfaFlattener
{
  private PdfDocument m_document;
  private PdfLoadedDocument m_loadedDocument;
  private PdfPage m_currentPage;
  private PointF m_currentPoint = PointF.Empty;
  private float m_maxHeight;
  private SizeF m_clinetSize = SizeF.Empty;
  private Dictionary<string, PdfLoadedXfaPage> m_pageCollection = new Dictionary<string, PdfLoadedXfaPage>();
  private string m_nextPageId = string.Empty;
  private string m_nextBgPageId = string.Empty;
  private PdfLoadedXfaPage m_xfaPage;
  private bool m_skipFPB;

  internal PdfXfaFlattener(PdfLoadedDocument ldoc) => this.m_loadedDocument = ldoc;

  private float GetColumnWidths(List<float> columnWidth)
  {
    float columnWidths = 0.0f;
    foreach (float num in columnWidth)
      columnWidths += num;
    return columnWidths;
  }

  private PdfLoadedXfaPage GetPageByID(string id)
  {
    PdfLoadedXfaPage pageById = (PdfLoadedXfaPage) null;
    foreach (KeyValuePair<string, PdfLoadedXfaPage> page in this.m_pageCollection)
    {
      if (page.Value.Id == id)
      {
        pageById = page.Value;
        break;
      }
    }
    return pageById;
  }

  internal SizeF ParseSubform(PdfLoadedXfaForm subform)
  {
    SizeF sizeF = SizeF.Empty;
    SizeF empty = SizeF.Empty;
    int num = 0;
    bool flag = true;
    foreach (KeyValuePair<string, PdfXfaField> completeField in subform.Fields.CompleteFields)
    {
      PdfLoadedXfaField pdfLoadedXfaField = completeField.Value as PdfLoadedXfaField;
      switch (pdfLoadedXfaField)
      {
        case PdfLoadedXfaForm _:
          PdfLoadedXfaForm subform1 = pdfLoadedXfaField as PdfLoadedXfaForm;
          subform1.parent = (PdfLoadedXfaField) subform;
          sizeF = this.ParseSubform(subform1);
          if (subform1.FlowDirection == PdfLoadedXfaFlowDirection.Row && (subform1.parent as PdfLoadedXfaForm).columnWidths.Count > 0)
          {
            sizeF.Width = this.GetColumnWidths((subform1.parent as PdfLoadedXfaForm).columnWidths);
            subform1.m_size.Width = sizeF.Width;
          }
          if (subform1.FlowDirection == PdfLoadedXfaFlowDirection.None && subform1.currentNode.Name != "subformSet" && subform1.parent != null && subform1.parent.currentNode != null && subform1.parent.currentNode.Name != "subformSet")
            sizeF = new SizeF(subform1.Width, subform1.Height);
          else if (subform1.FlowDirection == PdfLoadedXfaFlowDirection.None && subform1.currentNode.Name != "subformSet")
          {
            sizeF = new SizeF(subform1.Width, subform1.Height);
          }
          else
          {
            sizeF.Height += subform1.Margins.Top + subform1.Margins.Bottom;
            if ((double) subform1.Width == 0.0)
              sizeF.Width += subform1.Margins.Left + subform1.Margins.Right;
            if ((subform1.FlowDirection == PdfLoadedXfaFlowDirection.LeftToRight || subform1.FlowDirection == PdfLoadedXfaFlowDirection.RightToLeft) && (double) subform1.Width > 0.0 && (double) sizeF.Width > (double) subform1.Width)
              sizeF.Width = subform1.Width;
          }
          if ((double) subform1.m_size.Height == 0.0)
          {
            if ((double) sizeF.Height == 0.0 && subform1.Fields.Count > 0)
            {
              sizeF.Height = this.m_maxHeight + subform1.Margins.Top + subform1.Margins.Bottom;
              this.m_maxHeight = 0.0f;
            }
            subform1.m_size = sizeF;
          }
          if ((double) subform1.Width > 0.0 && (double) subform1.Width > (double) subform1.m_size.Width)
            subform1.m_size.Width = subform1.Width;
          subform1.m_size = sizeF;
          break;
        case PdfLoadedXfaTextBoxField _:
          PdfLoadedXfaTextBoxField loadedXfaTextBoxField = pdfLoadedXfaField as PdfLoadedXfaTextBoxField;
          sizeF = loadedXfaTextBoxField.Visibility == PdfXfaVisibility.Hidden ? SizeF.Empty : loadedXfaTextBoxField.GetFieldSize();
          break;
        case PdfLoadedXfaNumericField _:
          PdfLoadedXfaNumericField loadedXfaNumericField = pdfLoadedXfaField as PdfLoadedXfaNumericField;
          sizeF = loadedXfaNumericField.Visibility == PdfXfaVisibility.Hidden ? SizeF.Empty : loadedXfaNumericField.GetNumericFieldSize();
          break;
        case PdfLoadedXfaTextElement _:
          PdfLoadedXfaTextElement loadedXfaTextElement = pdfLoadedXfaField as PdfLoadedXfaTextElement;
          sizeF = loadedXfaTextElement.Visibility == PdfXfaVisibility.Hidden ? SizeF.Empty : loadedXfaTextElement.GetSize();
          break;
        case PdfLoadedXfaCheckBoxField _:
          PdfLoadedXfaCheckBoxField xfaCheckBoxField = pdfLoadedXfaField as PdfLoadedXfaCheckBoxField;
          sizeF = xfaCheckBoxField.Visibility == PdfXfaVisibility.Hidden ? SizeF.Empty : xfaCheckBoxField.GetSize();
          break;
        case PdfLoadedXfaListBoxField _:
          PdfLoadedXfaListBoxField loadedXfaListBoxField = pdfLoadedXfaField as PdfLoadedXfaListBoxField;
          sizeF = loadedXfaListBoxField.Visibility == PdfXfaVisibility.Hidden ? SizeF.Empty : loadedXfaListBoxField.GetSize();
          break;
        case PdfLoadedXfaComboBoxField _:
          PdfLoadedXfaComboBoxField xfaComboBoxField = pdfLoadedXfaField as PdfLoadedXfaComboBoxField;
          sizeF = xfaComboBoxField.Visibility == PdfXfaVisibility.Hidden ? SizeF.Empty : xfaComboBoxField.GetSize();
          break;
        case PdfLoadedXfaButtonField _:
          PdfLoadedXfaButtonField loadedXfaButtonField = pdfLoadedXfaField as PdfLoadedXfaButtonField;
          sizeF = loadedXfaButtonField.Visibility == PdfXfaVisibility.Hidden ? SizeF.Empty : loadedXfaButtonField.GetSize();
          break;
        case PdfLoadedXfaCircleField _:
          PdfLoadedXfaCircleField loadedXfaCircleField = pdfLoadedXfaField as PdfLoadedXfaCircleField;
          sizeF = loadedXfaCircleField.Visibility == PdfXfaVisibility.Hidden ? SizeF.Empty : loadedXfaCircleField.GetSize();
          break;
        case PdfLoadedXfaRectangleField _:
          PdfLoadedXfaRectangleField xfaRectangleField = pdfLoadedXfaField as PdfLoadedXfaRectangleField;
          if (xfaRectangleField.Visibility != PdfXfaVisibility.Hidden)
          {
            sizeF = xfaRectangleField.GetSize();
            break;
          }
          break;
        case PdfLoadedXfaLine _:
          PdfLoadedXfaLine pdfLoadedXfaLine = pdfLoadedXfaField as PdfLoadedXfaLine;
          sizeF = pdfLoadedXfaLine.Visibility == PdfXfaVisibility.Hidden ? SizeF.Empty : pdfLoadedXfaLine.GetSize();
          break;
        case PdfLoadedXfaDateTimeField _:
          PdfLoadedXfaDateTimeField xfaDateTimeField = pdfLoadedXfaField as PdfLoadedXfaDateTimeField;
          sizeF = xfaDateTimeField.Visibility == PdfXfaVisibility.Hidden ? SizeF.Empty : xfaDateTimeField.GetFieldSize();
          break;
        case PdfLoadedXfaImage _:
          PdfLoadedXfaImage pdfLoadedXfaImage = pdfLoadedXfaField as PdfLoadedXfaImage;
          sizeF = pdfLoadedXfaImage.Visibility == PdfXfaVisibility.Hidden ? SizeF.Empty : pdfLoadedXfaImage.GetSize();
          break;
        case PdfLoadedXfaRadioButtonGroup _:
          PdfLoadedXfaRadioButtonGroup radioButtonGroup = pdfLoadedXfaField as PdfLoadedXfaRadioButtonGroup;
          if (radioButtonGroup.Visibility != PdfXfaVisibility.Hidden)
          {
            sizeF = radioButtonGroup.GetSize();
            break;
          }
          break;
      }
      if ((double) sizeF.Width == 0.0 && subform.FlowDirection == PdfLoadedXfaFlowDirection.Row && (subform.parent as PdfLoadedXfaForm).columnWidths.Count > 0)
        sizeF.Width = (subform.parent as PdfLoadedXfaForm).columnWidths[num++];
      if (subform.FlowDirection == PdfLoadedXfaFlowDirection.TopToBottom)
      {
        subform.m_size.Height += sizeF.Height;
        if ((double) subform.m_size.Width < (double) sizeF.Width && (double) subform.Width != (double) subform.m_size.Width)
          subform.m_size.Width = sizeF.Width;
      }
      else if (subform.FlowDirection == PdfLoadedXfaFlowDirection.LeftToRight || subform.FlowDirection == PdfLoadedXfaFlowDirection.RightToLeft)
      {
        if ((double) subform.m_size.Width + (double) sizeF.Width > (double) subform.Width - ((double) subform.Margins.Left + (double) subform.Margins.Right))
        {
          subform.m_size.Width = sizeF.Width;
          subform.m_size.Height += empty.Height;
          empty.Height = 0.0f;
        }
        else
          subform.m_size.Width += sizeF.Width;
      }
      else if (subform.FlowDirection == PdfLoadedXfaFlowDirection.None && (double) subform.Width == 0.0 && (double) subform.Height == 0.0 && subform.currentNode != null && subform.currentNode.Name != "subformSet")
      {
        subform.m_size.Width = sizeF.Width;
        subform.m_size.Height = sizeF.Height;
      }
      else if (subform.currentNode != null && subform.currentNode.Name == "subformSet" && subform.FlowDirection == PdfLoadedXfaFlowDirection.None && (double) subform.Width == 0.0 && (double) subform.Height == 0.0)
      {
        if ((double) subform.m_size.Width < (double) sizeF.Width)
          subform.m_size.Width = sizeF.Width;
        subform.m_size.Height += sizeF.Height;
      }
      else if (subform.FlowDirection == PdfLoadedXfaFlowDirection.Row)
      {
        if ((double) subform.m_size.Height < (double) sizeF.Height)
          subform.m_size.Height = sizeF.Height;
      }
      else if (subform.FlowDirection == PdfLoadedXfaFlowDirection.Table)
      {
        if ((double) subform.m_size.Width < (double) sizeF.Width)
          subform.m_size.Width = sizeF.Width;
        subform.m_size.Height += sizeF.Height;
      }
      if ((double) empty.Height < (double) sizeF.Height)
        empty.Height = sizeF.Height;
      if ((double) empty.Width < (double) sizeF.Width)
        empty.Width = sizeF.Width;
      if (flag && subform.currentNode.Name != "subformSet" && subform.FlowDirection == PdfLoadedXfaFlowDirection.None && subform.parent != null && subform.parent.parent == null)
      {
        this.m_maxHeight = 0.0f;
        flag = false;
      }
      if ((double) this.m_maxHeight < (double) sizeF.Height)
        this.m_maxHeight = sizeF.Height;
    }
    if (subform.FlowDirection == PdfLoadedXfaFlowDirection.LeftToRight || subform.FlowDirection == PdfLoadedXfaFlowDirection.RightToLeft)
    {
      if ((double) subform.m_size.Width < (double) empty.Width)
        subform.m_size.Width = empty.Width;
      subform.m_size.Height += empty.Height;
    }
    if ((double) subform.Width == 0.0 && (double) subform.m_size.Width == 0.0)
      subform.m_size.Width = empty.Width;
    subform.bgSize = subform.m_size;
    if ((double) subform.Width > (double) subform.m_size.Width)
    {
      subform.m_size.Width = subform.Width;
      subform.bgSize.Width = subform.Width;
    }
    return subform.m_size;
  }

  internal void Flatten(XmlDocument xmlDoc, PdfLoadedXfaForm lForm, PdfDocument document)
  {
    this.m_document = document;
    XmlNodeList elementsByTagName = xmlDoc.GetElementsByTagName("pageSet");
    string key = string.Empty;
    int num = 0;
    foreach (XmlNode childNode in elementsByTagName[0].ChildNodes)
    {
      if (childNode != null && childNode.Name == "pageArea")
      {
        PdfLoadedXfaPage pdfLoadedXfaPage = new PdfLoadedXfaPage();
        pdfLoadedXfaPage.ReadPage(childNode, lForm);
        pdfLoadedXfaPage.flattener = this;
        pdfLoadedXfaPage.document = this.m_document;
        this.m_pageCollection.Add(pdfLoadedXfaPage.Name, pdfLoadedXfaPage);
        if (num == 0)
          key = pdfLoadedXfaPage.Name;
        ++num;
      }
    }
    Dictionary<string, PdfLoadedXfaPage>.Enumerator enumerator = this.m_pageCollection.GetEnumerator();
    enumerator.MoveNext();
    this.m_xfaPage = enumerator.Current.Value;
    this.ParseSubform(lForm);
    this.m_clinetSize = this.m_xfaPage.GetClientSize();
    this.CalculateBackgroundHeight(lForm);
    this.m_xfaPage = enumerator.Current.Value;
    this.ResetForm(lForm);
    this.m_maxHeight = 0.0f;
    this.m_currentPoint = PointF.Empty;
    this.m_currentPage = this.m_pageCollection[key].CurrentPage;
    this.m_pageCollection[key].DrawPageBackgroundTemplate(this.m_currentPage);
    this.m_clinetSize = this.m_currentPage.GetClientSize();
    this.m_skipFPB = true;
    this.DrawSubForm(lForm);
  }

  private void ResetForm(PdfLoadedXfaForm subform)
  {
    foreach (KeyValuePair<string, PdfXfaField> completeField in subform.Fields.CompleteFields)
    {
      if (completeField.Value is PdfLoadedXfaForm)
        this.ResetForm(completeField.Value as PdfLoadedXfaForm);
      subform.currentPoint = PointF.Empty;
      subform.cStartPoint = PointF.Empty;
      subform.extraSize = 0.0f;
      subform.trackingHeight = 0.0f;
      subform.startPoint = PointF.Empty;
      subform.maxWidth = 0.0f;
      subform.maxHeight = 0.0f;
    }
  }

  private void DrawField(PdfLoadedXfaField field, PdfGraphics graphics, RectangleF bounds)
  {
    switch (field)
    {
      case PdfLoadedXfaTextElement _:
        PdfLoadedXfaTextElement loadedXfaTextElement = field as PdfLoadedXfaTextElement;
        if (loadedXfaTextElement.Visibility == PdfXfaVisibility.Hidden || loadedXfaTextElement.Visibility == PdfXfaVisibility.Invisible)
          break;
        loadedXfaTextElement.DrawTextElement(graphics, bounds);
        break;
      case PdfLoadedXfaTextBoxField _:
        PdfLoadedXfaTextBoxField loadedXfaTextBoxField = field as PdfLoadedXfaTextBoxField;
        if (loadedXfaTextBoxField.Visibility == PdfXfaVisibility.Hidden || loadedXfaTextBoxField.Visibility == PdfXfaVisibility.Invisible)
          break;
        loadedXfaTextBoxField.DrawField(graphics, bounds);
        break;
      case PdfLoadedXfaNumericField _:
        PdfLoadedXfaNumericField loadedXfaNumericField = field as PdfLoadedXfaNumericField;
        if (loadedXfaNumericField.Visibility == PdfXfaVisibility.Hidden || loadedXfaNumericField.Visibility == PdfXfaVisibility.Invisible)
          break;
        loadedXfaNumericField.DrawField(graphics, bounds);
        break;
      case PdfLoadedXfaDateTimeField _:
        PdfLoadedXfaDateTimeField xfaDateTimeField = field as PdfLoadedXfaDateTimeField;
        if (xfaDateTimeField.Visibility == PdfXfaVisibility.Hidden || xfaDateTimeField.Visibility == PdfXfaVisibility.Invisible)
          break;
        xfaDateTimeField.DrawField(graphics, bounds);
        break;
      case PdfLoadedXfaListBoxField _:
        PdfLoadedXfaListBoxField loadedXfaListBoxField = field as PdfLoadedXfaListBoxField;
        if (loadedXfaListBoxField.Visibility == PdfXfaVisibility.Hidden || loadedXfaListBoxField.Visibility == PdfXfaVisibility.Invisible)
          break;
        loadedXfaListBoxField.DrawListBoxField(graphics, bounds);
        break;
      case PdfLoadedXfaComboBoxField _:
        PdfLoadedXfaComboBoxField xfaComboBoxField = field as PdfLoadedXfaComboBoxField;
        if (xfaComboBoxField.Visibility == PdfXfaVisibility.Hidden || xfaComboBoxField.Visibility == PdfXfaVisibility.Invisible)
          break;
        xfaComboBoxField.DrawField(graphics, bounds);
        break;
      case PdfLoadedXfaButtonField _:
        PdfLoadedXfaButtonField loadedXfaButtonField = field as PdfLoadedXfaButtonField;
        if (loadedXfaButtonField.Visibility == PdfXfaVisibility.Hidden || loadedXfaButtonField.Visibility == PdfXfaVisibility.Invisible)
          break;
        loadedXfaButtonField.DrawField(graphics, bounds);
        break;
      case PdfLoadedXfaLine _:
        PdfLoadedXfaLine pdfLoadedXfaLine = field as PdfLoadedXfaLine;
        if (pdfLoadedXfaLine.Visibility == PdfXfaVisibility.Hidden || pdfLoadedXfaLine.Visibility == PdfXfaVisibility.Invisible)
          break;
        pdfLoadedXfaLine.DrawLine(graphics, bounds);
        break;
      case PdfLoadedXfaImage _:
        PdfLoadedXfaImage pdfLoadedXfaImage = field as PdfLoadedXfaImage;
        if (pdfLoadedXfaImage.Visibility == PdfXfaVisibility.Hidden || pdfLoadedXfaImage.Visibility == PdfXfaVisibility.Invisible)
          break;
        pdfLoadedXfaImage.DrawImage(graphics, bounds, this.m_loadedDocument);
        break;
      case PdfLoadedXfaCheckBoxField _:
        PdfLoadedXfaCheckBoxField xfaCheckBoxField = field as PdfLoadedXfaCheckBoxField;
        if (xfaCheckBoxField.Visibility == PdfXfaVisibility.Hidden || xfaCheckBoxField.Visibility == PdfXfaVisibility.Invisible)
          break;
        xfaCheckBoxField.DrawField(graphics, bounds);
        break;
      case PdfLoadedXfaRectangleField _:
        PdfLoadedXfaRectangleField xfaRectangleField = field as PdfLoadedXfaRectangleField;
        if (xfaRectangleField.Visibility == PdfXfaVisibility.Hidden || xfaRectangleField.Visibility == PdfXfaVisibility.Invisible)
          break;
        xfaRectangleField.DrawRectangle(graphics, bounds);
        break;
      case PdfLoadedXfaRadioButtonGroup _:
        PdfLoadedXfaRadioButtonGroup radioButtonGroup = field as PdfLoadedXfaRadioButtonGroup;
        if (radioButtonGroup.Visibility == PdfXfaVisibility.Hidden || radioButtonGroup.Visibility == PdfXfaVisibility.Invisible)
          break;
        radioButtonGroup.DrawRadiButtonGroup(graphics, bounds);
        break;
      case PdfLoadedXfaCircleField _:
        PdfLoadedXfaCircleField loadedXfaCircleField = field as PdfLoadedXfaCircleField;
        if (loadedXfaCircleField.Visibility == PdfXfaVisibility.Hidden || loadedXfaCircleField.Visibility == PdfXfaVisibility.Invisible)
          break;
        loadedXfaCircleField.DrawCircle(graphics, bounds);
        break;
    }
  }

  private void DrawTabeBorder(PdfLoadedXfaForm subform, PointF currentPoint)
  {
    int count = subform.Fields.CompleteFields.Count;
    PdfPen pen = new PdfPen(Color.Black, 1f);
    float x1 = currentPoint.X;
    float y1 = currentPoint.Y;
    this.m_currentPage.Graphics.DrawLine(pen, x1, y1, subform.m_size.Width, y1);
    float num = y1 + subform.m_size.Height;
    this.m_currentPage.Graphics.DrawLine(pen, x1, num, subform.m_size.Width, num);
    float x2 = currentPoint.X;
    float y2 = currentPoint.Y;
    this.m_currentPage.Graphics.DrawLine(pen, x2, y2, x2, currentPoint.Y + subform.m_size.Height);
    for (int index = 0; index < (subform.parent as PdfLoadedXfaForm).columnWidths.Count; ++index)
    {
      x2 += (subform.parent as PdfLoadedXfaForm).columnWidths[index];
      this.m_currentPage.Graphics.DrawLine(pen, x2, currentPoint.Y, x2, currentPoint.Y + subform.m_size.Height);
    }
  }

  private void SetCurrentPosition(PdfLoadedXfaForm subForm, float trackingSize)
  {
    if (subForm.FlowDirection == PdfLoadedXfaFlowDirection.LeftToRight || subForm.FlowDirection == PdfLoadedXfaFlowDirection.RightToLeft || subForm.FlowDirection == PdfLoadedXfaFlowDirection.Row)
      subForm.trackingHeight += subForm.maxHeight;
    float trackingSize1 = subForm.trackingHeight + trackingSize;
    subForm.m_size.Height -= trackingSize1;
    subForm.currentPoint = subForm.startPoint;
    subForm.trackingHeight = 0.0f;
    subForm.maxHeight = 0.0f;
    subForm.cStartPoint = subForm.startPoint;
    if (subForm.parent == null)
      return;
    this.SetCurrentPosition(subForm.parent as PdfLoadedXfaForm, trackingSize1);
  }

  private void SetCurrentBackgroundHeight(PdfLoadedXfaForm subForm, float trackingSize)
  {
    if (subForm.FlowDirection == PdfLoadedXfaFlowDirection.LeftToRight || subForm.FlowDirection == PdfLoadedXfaFlowDirection.RightToLeft || subForm.FlowDirection == PdfLoadedXfaFlowDirection.Row)
      subForm.trackingHeight += subForm.maxHeight;
    float trackingSize1 = subForm.trackingHeight + trackingSize + subForm.Margins.Bottom;
    subForm.backgroundHeight.Add(trackingSize1);
    subForm.bgSize.Height -= trackingSize1;
    subForm.currentPoint = subForm.startPoint;
    subForm.trackingHeight = 0.0f;
    subForm.cStartPoint = subForm.startPoint;
    subForm.maxHeight = 0.0f;
    if (subForm.parent == null)
      return;
    this.SetCurrentBackgroundHeight(subForm.parent as PdfLoadedXfaForm, trackingSize1);
  }

  private void DrawBackground(PdfLoadedXfaForm tempForm, bool isRepeat)
  {
    if (tempForm.parent != null && isRepeat)
      this.DrawBackground(tempForm.parent as PdfLoadedXfaForm, true);
    if (tempForm.Border == null || tempForm.Border.Visibility == PdfXfaVisibility.Hidden)
      return;
    PdfPen pen = tempForm.Border.GetPen();
    RectangleF rectangleF = new RectangleF();
    float width = tempForm.Width;
    if ((double) tempForm.Width == 0.0)
      width = tempForm.m_size.Width + tempForm.Margins.Left + tempForm.Margins.Right;
    float height = tempForm.backgroundHeight.Count <= tempForm.bgHeightCounter ? tempForm.bgSize.Height : tempForm.backgroundHeight[tempForm.bgHeightCounter++];
    rectangleF = new RectangleF(tempForm.currentPoint.X - tempForm.Margins.Left, tempForm.currentPoint.Y - tempForm.Margins.Top, width, height);
    if (tempForm.parent == null)
      rectangleF.Height += this.m_currentPoint.Y;
    if (tempForm.parent == null)
      rectangleF.Height += tempForm.Margins.Bottom;
    if (tempForm.Border.LeftEdge != null && tempForm.Border.RightEdge == null && tempForm.Border.TopEdge == null && tempForm.Border.BottomEdge == null)
    {
      if (tempForm.Border.LeftEdge.Visibility != PdfXfaVisibility.Hidden)
        this.m_currentPage.Graphics.DrawRectangle(new PdfPen(tempForm.Border.LeftEdge.Color, tempForm.Border.LeftEdge.Thickness), tempForm.Border.GetBrush(rectangleF), rectangleF);
      else
        this.m_currentPage.Graphics.DrawRectangle(tempForm.Border.GetBrush(rectangleF), rectangleF);
    }
    else if (tempForm.Border.LeftEdge != null || tempForm.Border.RightEdge != null || tempForm.Border.TopEdge != null || tempForm.Border.BottomEdge != null)
    {
      this.m_currentPage.Graphics.DrawRectangle(tempForm.Border.GetBrush(rectangleF), rectangleF);
      if (tempForm.Border.LeftEdge != null && tempForm.Border.LeftEdge.Visibility != PdfXfaVisibility.Hidden)
        this.DrawEdge(tempForm.Border.LeftEdge, rectangleF.Location, new PointF(rectangleF.X, rectangleF.Y + rectangleF.Height), this.m_currentPage);
      if (tempForm.Border.RightEdge != null && tempForm.Border.RightEdge.Visibility != PdfXfaVisibility.Hidden)
        this.DrawEdge(tempForm.Border.RightEdge, new PointF(rectangleF.X + rectangleF.Width, rectangleF.Y), new PointF(rectangleF.X + rectangleF.Width, rectangleF.Y + rectangleF.Height), this.m_currentPage);
      if (tempForm.Border.TopEdge != null && tempForm.Border.TopEdge.Visibility != PdfXfaVisibility.Hidden)
        this.DrawEdge(tempForm.Border.TopEdge, rectangleF.Location, new PointF(rectangleF.X + rectangleF.Width, rectangleF.Y), this.m_currentPage);
      if (tempForm.Border.BottomEdge == null || tempForm.Border.BottomEdge.Visibility == PdfXfaVisibility.Hidden)
        return;
      this.DrawEdge(tempForm.Border.BottomEdge, new PointF(rectangleF.X, rectangleF.Y + rectangleF.Height), new PointF(rectangleF.X + rectangleF.Width, rectangleF.Y + rectangleF.Height), this.m_currentPage);
    }
    else
      this.m_currentPage.Graphics.DrawRectangle(pen, tempForm.Border.GetBrush(rectangleF), rectangleF);
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

  private void DrawSubForm(PdfLoadedXfaForm subform)
  {
    int num = 0;
    if (subform.PageBreak != null)
    {
      PdfLoadedXfaPageBreak pageBreak = subform.PageBreak;
      if (pageBreak.Overflow != null && pageBreak.Overflow.OverFlowID != string.Empty)
        this.m_nextPageId = subform.PageBreak.Overflow.OverFlowID;
      else if (pageBreak.BeforeBreak != null)
        this.m_nextPageId = pageBreak.BeforeBreak.BeforeTargetID;
      else if (pageBreak.BeforeTargetID != string.Empty)
        this.m_nextPageId = pageBreak.BeforeTargetID;
    }
    foreach (KeyValuePair<string, PdfXfaField> completeField in subform.Fields.CompleteFields)
    {
      if (completeField.Value is PdfLoadedXfaForm)
      {
        PdfLoadedXfaForm pdfLoadedXfaForm = completeField.Value as PdfLoadedXfaForm;
        if (pdfLoadedXfaForm.columnWidths.Count > 0)
          pdfLoadedXfaForm.m_size.Width = this.GetColumnWidths(pdfLoadedXfaForm.columnWidths);
        pdfLoadedXfaForm.startPoint = new PointF(subform.startPoint.X + pdfLoadedXfaForm.Margins.Left, subform.startPoint.Y + pdfLoadedXfaForm.Margins.Top);
        SizeF sizeF = new SizeF((double) pdfLoadedXfaForm.Width > 0.0 ? pdfLoadedXfaForm.Width : pdfLoadedXfaForm.m_size.Width, (double) pdfLoadedXfaForm.Height > 0.0 ? pdfLoadedXfaForm.Height : pdfLoadedXfaForm.m_size.Height);
        if (subform.FlowDirection == PdfLoadedXfaFlowDirection.None)
        {
          if (subform.currentNode.Name == "subformSet" && (double) subform.currentPoint.X + (double) sizeF.Width >= (double) subform.m_size.Width - ((double) subform.Margins.Right + (double) subform.Margins.Left))
          {
            subform.currentPoint.X = subform.startPoint.X;
            subform.currentPoint.Y += subform.maxHeight;
            subform.trackingHeight += subform.maxHeight;
            subform.maxHeight = 0.0f;
          }
        }
        else if ((subform.FlowDirection == PdfLoadedXfaFlowDirection.LeftToRight || subform.FlowDirection == PdfLoadedXfaFlowDirection.RightToLeft) && (double) subform.currentPoint.X + (double) sizeF.Width > (double) subform.m_size.Width - ((double) subform.Margins.Right + (double) subform.Margins.Left))
        {
          subform.currentPoint.X = subform.startPoint.X;
          subform.currentPoint.Y += subform.maxHeight;
          subform.trackingHeight += subform.maxHeight;
          subform.maxHeight = 0.0f;
        }
        bool flag1 = false;
        bool flag2 = false;
        bool flag3 = false;
        if ((double) subform.currentPoint.Y + (double) pdfLoadedXfaForm.m_size.Height > (double) this.m_clinetSize.Height - ((double) subform.Margins.Top + (double) subform.Margins.Bottom) && pdfLoadedXfaForm.currentNode["keep"] != null && pdfLoadedXfaForm.FlowDirection != PdfLoadedXfaFlowDirection.Table && pdfLoadedXfaForm.currentNode["keep"].Attributes["intact"] != null && pdfLoadedXfaForm.currentNode["keep"].Attributes["intact"].Value == "contentArea")
          flag1 = true;
        if (pdfLoadedXfaForm.FlowDirection == PdfLoadedXfaFlowDirection.Table && (double) subform.currentPoint.Y + (double) pdfLoadedXfaForm.m_size.Height > (double) this.m_clinetSize.Height - ((double) subform.Margins.Top + (double) subform.Margins.Bottom))
          flag2 = true;
        if (pdfLoadedXfaForm.PageBreak != null && !this.m_skipFPB)
        {
          string str = string.Empty;
          bool flag4 = false;
          PdfLoadedXfaPageBreak pageBreak = pdfLoadedXfaForm.PageBreak;
          if (pageBreak.BeforeBreak != null)
          {
            str = pageBreak.BeforeBreak.BeforeTargetID;
            if (pageBreak.BeforeBreak.IsStartNew)
              flag4 = true;
          }
          else if (pageBreak.BeforeTargetID != string.Empty)
            str = pageBreak.BeforeTargetID;
          if (str != string.Empty || flag4)
          {
            PdfLoadedXfaPage pdfLoadedXfaPage = !this.m_pageCollection.ContainsKey(str) ? this.GetPageByID(str) : this.m_pageCollection[str];
            if (pdfLoadedXfaPage != null)
            {
              PdfPage currentPage = pdfLoadedXfaPage.CurrentPage;
              this.m_currentPage = !pdfLoadedXfaPage.isSet ? currentPage.Section.Pages.Add() : currentPage;
              pdfLoadedXfaPage.DrawPageBackgroundTemplate(this.m_currentPage);
              this.m_xfaPage = pdfLoadedXfaPage;
            }
            else
            {
              this.m_currentPage = this.m_currentPage.Section.Add();
              this.m_xfaPage.DrawPageBackgroundTemplate(this.m_currentPage);
            }
            this.m_clinetSize = this.m_currentPage.GetClientSize();
            subform.currentPoint = subform.startPoint;
            if (subform.parent != null)
              this.SetCurrentPosition(subform.parent as PdfLoadedXfaForm, subform.trackingHeight);
            this.DrawBackground(subform, true);
            subform.m_size.Height -= subform.trackingHeight;
            subform.trackingHeight = 0.0f;
            subform.maxHeight = 0.0f;
            flag3 = true;
          }
        }
        else if (flag1 || flag2)
        {
          PdfLoadedXfaPage pdfLoadedXfaPage = (PdfLoadedXfaPage) null;
          if (this.m_nextPageId != string.Empty)
          {
            pdfLoadedXfaPage = !this.m_pageCollection.ContainsKey(this.m_nextPageId) ? this.GetPageByID(this.m_nextPageId) : this.m_pageCollection[this.m_nextPageId];
            this.m_nextPageId = string.Empty;
          }
          if (pdfLoadedXfaPage != null)
          {
            PdfPage currentPage = pdfLoadedXfaPage.CurrentPage;
            this.m_currentPage = !pdfLoadedXfaPage.isSet ? currentPage.Section.Pages.Add() : currentPage;
            pdfLoadedXfaPage.DrawPageBackgroundTemplate(this.m_currentPage);
            this.m_xfaPage = pdfLoadedXfaPage;
          }
          else
          {
            this.m_currentPage = this.m_currentPage.Section.Add();
            this.m_xfaPage.DrawPageBackgroundTemplate(this.m_currentPage);
          }
          this.m_clinetSize = this.m_currentPage.GetClientSize();
          subform.currentPoint = subform.startPoint;
          if (subform.parent != null)
            this.SetCurrentPosition(subform.parent as PdfLoadedXfaForm, subform.trackingHeight);
          this.DrawBackground(subform, true);
          subform.m_size.Height -= subform.trackingHeight;
          subform.trackingHeight = 0.0f;
          subform.maxHeight = 0.0f;
          flag3 = true;
        }
        pdfLoadedXfaForm.currentPoint = !flag3 ? new PointF(pdfLoadedXfaForm.startPoint.X + subform.currentPoint.X, pdfLoadedXfaForm.startPoint.Y + subform.currentPoint.Y) : new PointF(pdfLoadedXfaForm.startPoint.X, pdfLoadedXfaForm.startPoint.Y);
        if (subform.FlowDirection == PdfLoadedXfaFlowDirection.None && subform.currentNode.Name != "subformSet")
        {
          pdfLoadedXfaForm.currentPoint.X += pdfLoadedXfaForm.Location.X;
          pdfLoadedXfaForm.currentPoint.Y += pdfLoadedXfaForm.Location.Y;
        }
        pdfLoadedXfaForm.cStartPoint.X = pdfLoadedXfaForm.currentPoint.X - pdfLoadedXfaForm.Margins.Left;
        pdfLoadedXfaForm.cStartPoint.Y = pdfLoadedXfaForm.currentPoint.Y - pdfLoadedXfaForm.Margins.Top;
        this.m_skipFPB = false;
        this.DrawBackground(pdfLoadedXfaForm, false);
        this.DrawSubForm(pdfLoadedXfaForm);
        if (pdfLoadedXfaForm.FlowDirection == PdfLoadedXfaFlowDirection.Row)
          this.DrawTabeBorder(pdfLoadedXfaForm, subform.currentPoint);
        if (pdfLoadedXfaForm.FlowDirection == PdfLoadedXfaFlowDirection.None && pdfLoadedXfaForm.currentNode.Name != "subformSet")
          pdfLoadedXfaForm.m_size = (double) pdfLoadedXfaForm.Height != 0.0 ? new SizeF(pdfLoadedXfaForm.Width, pdfLoadedXfaForm.Height) : (pdfLoadedXfaForm.currentNode.Attributes["minH"] == null ? new SizeF(pdfLoadedXfaForm.Width, pdfLoadedXfaForm.m_size.Height) : new SizeF(pdfLoadedXfaForm.Width, pdfLoadedXfaForm.ConvertToPoint(pdfLoadedXfaForm.currentNode.Attributes["minH"].Value)));
        if (subform.FlowDirection == PdfLoadedXfaFlowDirection.None)
        {
          if (subform.currentNode.Name == "subformSet")
          {
            if ((double) subform.currentPoint.X + (double) sizeF.Width > (double) subform.m_size.Width - ((double) subform.Margins.Right + (double) subform.Margins.Left))
            {
              subform.currentPoint.X = subform.startPoint.X;
              subform.currentPoint.Y += subform.maxHeight;
              subform.trackingHeight += subform.maxHeight;
              subform.maxHeight = 0.0f;
            }
            else
              subform.currentPoint.X += pdfLoadedXfaForm.m_size.Width;
          }
        }
        else if (subform.FlowDirection == PdfLoadedXfaFlowDirection.LeftToRight || subform.FlowDirection == PdfLoadedXfaFlowDirection.RightToLeft)
        {
          if ((int) ((double) subform.currentPoint.X + (double) sizeF.Width) > (int) ((double) subform.m_size.Width - ((double) subform.Margins.Right + (double) subform.Margins.Left)))
          {
            subform.currentPoint.X = subform.startPoint.X;
            subform.currentPoint.Y += subform.maxHeight;
            subform.trackingHeight += subform.maxHeight;
            subform.maxHeight = 0.0f;
          }
          else
            subform.currentPoint.X += sizeF.Width;
        }
        else
        {
          subform.currentPoint.Y += pdfLoadedXfaForm.m_size.Height;
          subform.trackingHeight += pdfLoadedXfaForm.m_size.Height;
        }
        if ((double) subform.maxHeight < (double) pdfLoadedXfaForm.m_size.Height)
          subform.maxHeight = pdfLoadedXfaForm.m_size.Height;
        if ((double) subform.maxWidth < (double) pdfLoadedXfaForm.m_size.Width)
          subform.maxWidth = pdfLoadedXfaForm.m_size.Width;
      }
      else if (completeField.Value is PdfLoadedXfaStyledField)
      {
        PdfLoadedXfaStyledField field = completeField.Value as PdfLoadedXfaStyledField;
        if (subform.FlowDirection == PdfLoadedXfaFlowDirection.Row)
          field.Width = (subform.parent as PdfLoadedXfaForm).columnWidths[num++];
        if ((double) field.Height == 0.0)
        {
          if (field is PdfLoadedXfaTextBoxField)
          {
            PdfLoadedXfaTextBoxField loadedXfaTextBoxField = field as PdfLoadedXfaTextBoxField;
            SizeF fieldSize = loadedXfaTextBoxField.GetFieldSize();
            loadedXfaTextBoxField.Height = fieldSize.Height;
          }
          else
            field.GetSize();
        }
        if (field.Visibility != PdfXfaVisibility.Hidden)
        {
          if (subform.FlowDirection == PdfLoadedXfaFlowDirection.None)
          {
            if ((int) ((double) subform.currentPoint.Y + ((double) field.Location.Y - (double) subform.extraSize) + (double) field.Height) > (int) ((double) this.m_clinetSize.Height - (double) subform.Margins.Bottom))
            {
              PdfLoadedXfaPage pdfLoadedXfaPage = (PdfLoadedXfaPage) null;
              if (this.m_nextPageId != string.Empty)
              {
                pdfLoadedXfaPage = !this.m_pageCollection.ContainsKey(this.m_nextPageId) ? this.GetPageByID(this.m_nextPageId) : this.m_pageCollection[this.m_nextPageId];
                this.m_nextPageId = string.Empty;
              }
              if (pdfLoadedXfaPage != null)
              {
                PdfPage currentPage = pdfLoadedXfaPage.CurrentPage;
                this.m_currentPage = !pdfLoadedXfaPage.isSet ? currentPage.Section.Pages.Add() : currentPage;
                pdfLoadedXfaPage.DrawPageBackgroundTemplate(this.m_currentPage);
                this.m_xfaPage = pdfLoadedXfaPage;
              }
              else
              {
                this.m_currentPage = this.m_currentPage.Section.Add();
                this.m_xfaPage.DrawPageBackgroundTemplate(this.m_currentPage);
              }
              this.m_clinetSize = this.m_currentPage.GetClientSize();
              subform.currentPoint = subform.startPoint;
              subform.extraSize = subform.trackingHeight;
              if (subform.parent != null)
                this.SetCurrentPosition(subform.parent as PdfLoadedXfaForm, subform.trackingHeight);
              this.DrawBackground(subform, true);
              subform.m_size.Height -= subform.trackingHeight;
              subform.trackingHeight = 0.0f;
              subform.maxHeight = 0.0f;
            }
            this.DrawField((PdfLoadedXfaField) field, this.m_currentPage.Graphics, new RectangleF(new PointF(field.Location.X + subform.currentPoint.X, field.Location.Y - subform.extraSize + subform.currentPoint.Y), new SizeF(field.GetSize().Width, field.GetSize().Height)));
            subform.trackingHeight = field.Location.Y + field.GetSize().Height;
          }
          else if (subform.FlowDirection == PdfLoadedXfaFlowDirection.TopToBottom)
          {
            if ((double) subform.currentPoint.Y + (double) field.GetSize().Height > (double) this.m_clinetSize.Height - (double) subform.Margins.Bottom)
            {
              PdfLoadedXfaPage pdfLoadedXfaPage = (PdfLoadedXfaPage) null;
              if (this.m_nextPageId != string.Empty)
              {
                pdfLoadedXfaPage = !this.m_pageCollection.ContainsKey(this.m_nextPageId) ? this.GetPageByID(this.m_nextPageId) : this.m_pageCollection[this.m_nextPageId];
                this.m_nextPageId = string.Empty;
              }
              if (pdfLoadedXfaPage != null)
              {
                PdfPage currentPage = pdfLoadedXfaPage.CurrentPage;
                this.m_currentPage = !pdfLoadedXfaPage.isSet ? currentPage.Section.Pages.Add() : currentPage;
                pdfLoadedXfaPage.DrawPageBackgroundTemplate(this.m_currentPage);
                this.m_xfaPage = pdfLoadedXfaPage;
              }
              else
              {
                this.m_currentPage = this.m_currentPage.Section.Add();
                this.m_xfaPage.DrawPageBackgroundTemplate(this.m_currentPage);
              }
              this.m_clinetSize = this.m_currentPage.GetClientSize();
              subform.currentPoint = subform.startPoint;
              if (subform.parent != null)
                this.SetCurrentPosition(subform.parent as PdfLoadedXfaForm, subform.trackingHeight);
              this.DrawBackground(subform, true);
              subform.m_size.Height -= subform.trackingHeight;
              subform.trackingHeight = 0.0f;
              subform.maxHeight = 0.0f;
            }
            this.DrawField((PdfLoadedXfaField) field, this.m_currentPage.Graphics, new RectangleF(subform.currentPoint, new SizeF(field.GetSize().Width, field.GetSize().Height)));
            subform.currentPoint.Y += field.GetSize().Height;
            subform.trackingHeight += field.GetSize().Height;
          }
          else if (subform.FlowDirection == PdfLoadedXfaFlowDirection.LeftToRight || subform.FlowDirection == PdfLoadedXfaFlowDirection.RightToLeft || subform.FlowDirection == PdfLoadedXfaFlowDirection.Row)
          {
            if ((double) (subform.currentPoint.X + field.GetSize().Width - subform.cStartPoint.X) > (double) (subform.m_size.Width - subform.Margins.Right))
            {
              subform.currentPoint.X = subform.cStartPoint.X + subform.startPoint.X;
              subform.currentPoint.Y += subform.maxHeight;
              subform.trackingHeight += subform.maxHeight;
              subform.maxHeight = 0.0f;
            }
            if ((double) subform.currentPoint.Y + (double) field.GetSize().Height > (double) this.m_clinetSize.Height - (double) subform.Margins.Bottom)
            {
              PdfLoadedXfaPage pdfLoadedXfaPage = (PdfLoadedXfaPage) null;
              if (this.m_nextPageId != string.Empty)
              {
                pdfLoadedXfaPage = !this.m_pageCollection.ContainsKey(this.m_nextPageId) ? this.GetPageByID(this.m_nextPageId) : this.m_pageCollection[this.m_nextPageId];
                this.m_nextPageId = string.Empty;
              }
              if (pdfLoadedXfaPage != null)
              {
                PdfPage currentPage = pdfLoadedXfaPage.CurrentPage;
                this.m_currentPage = !pdfLoadedXfaPage.isSet ? currentPage.Section.Pages.Add() : currentPage;
                pdfLoadedXfaPage.DrawPageBackgroundTemplate(this.m_currentPage);
                this.m_xfaPage = pdfLoadedXfaPage;
              }
              else
              {
                this.m_currentPage = this.m_currentPage.Section.Add();
                this.m_xfaPage.DrawPageBackgroundTemplate(this.m_currentPage);
              }
              this.m_clinetSize = this.m_currentPage.GetClientSize();
              subform.currentPoint = subform.startPoint;
              if (subform.parent != null)
                this.SetCurrentPosition(subform.parent as PdfLoadedXfaForm, subform.trackingHeight);
              this.DrawBackground(subform, true);
              subform.m_size.Height -= subform.trackingHeight;
              subform.trackingHeight = 0.0f;
              subform.maxHeight = 0.0f;
            }
            this.DrawField((PdfLoadedXfaField) field, this.m_currentPage.Graphics, new RectangleF(subform.currentPoint, new SizeF(field.GetSize().Width, field.GetSize().Height)));
            subform.currentPoint.X += field.GetSize().Width;
          }
          if ((double) subform.maxHeight < (double) field.GetSize().Height)
            subform.maxHeight = field.GetSize().Height;
          if ((double) subform.maxWidth < (double) field.GetSize().Width)
            subform.maxWidth = field.GetSize().Width;
        }
      }
      else if (completeField.Value is PdfLoadedXfaRadioButtonGroup)
      {
        PdfLoadedXfaRadioButtonGroup field = completeField.Value as PdfLoadedXfaRadioButtonGroup;
        if (subform.FlowDirection == PdfLoadedXfaFlowDirection.Row)
          field.Width = (subform.parent as PdfLoadedXfaForm).columnWidths[num++];
        if (field.Visibility != PdfXfaVisibility.Hidden)
        {
          if (subform.FlowDirection == PdfLoadedXfaFlowDirection.None)
          {
            if ((double) subform.currentPoint.Y + (double) field.Location.Y + (double) field.Height > (double) this.m_clinetSize.Height - (double) subform.Margins.Bottom)
            {
              this.m_currentPage = this.m_currentPage.Section.Add();
              this.m_xfaPage.DrawPageBackgroundTemplate(this.m_currentPage);
              this.m_clinetSize = this.m_currentPage.GetClientSize();
              subform.currentPoint = subform.startPoint;
              if (subform.parent != null)
                this.SetCurrentPosition(subform.parent as PdfLoadedXfaForm, subform.trackingHeight);
              this.DrawBackground(subform, true);
              subform.m_size.Height -= subform.trackingHeight;
              subform.trackingHeight = 0.0f;
              subform.maxHeight = 0.0f;
            }
            this.DrawField((PdfLoadedXfaField) field, this.m_currentPage.Graphics, new RectangleF(new PointF(field.Location.X + subform.currentPoint.X, field.Location.Y + subform.currentPoint.Y), new SizeF(field.Width, field.Height)));
            subform.trackingHeight = field.Location.Y + subform.currentPoint.Y + field.Height;
          }
          else if (subform.FlowDirection == PdfLoadedXfaFlowDirection.TopToBottom)
          {
            if ((double) subform.currentPoint.Y + (double) field.Height > (double) this.m_clinetSize.Height - (double) subform.Margins.Bottom)
            {
              this.m_currentPage = this.m_currentPage.Section.Add();
              this.m_xfaPage.DrawPageBackgroundTemplate(this.m_currentPage);
              this.m_clinetSize = this.m_currentPage.GetClientSize();
              subform.currentPoint = subform.startPoint;
              if (subform.parent != null)
                this.SetCurrentPosition(subform.parent as PdfLoadedXfaForm, subform.trackingHeight);
              this.DrawBackground(subform, true);
              subform.m_size.Height -= subform.trackingHeight;
              subform.trackingHeight = 0.0f;
              subform.maxHeight = 0.0f;
            }
            this.DrawField((PdfLoadedXfaField) field, this.m_currentPage.Graphics, new RectangleF(subform.currentPoint, new SizeF(field.Width, field.Height)));
            subform.currentPoint.Y += field.Height;
            subform.trackingHeight += field.Height;
          }
          else if (subform.FlowDirection == PdfLoadedXfaFlowDirection.LeftToRight || subform.FlowDirection == PdfLoadedXfaFlowDirection.RightToLeft || subform.FlowDirection == PdfLoadedXfaFlowDirection.Row)
          {
            if ((double) (subform.currentPoint.X + field.Width) - (double) subform.cStartPoint.X > (double) (subform.m_size.Width - subform.Margins.Right))
            {
              subform.currentPoint.X = subform.startPoint.X + subform.cStartPoint.X;
              subform.currentPoint.Y += subform.maxHeight;
              subform.trackingHeight += subform.maxHeight;
              subform.maxHeight = 0.0f;
            }
            if ((double) subform.currentPoint.Y + (double) field.Height > (double) this.m_clinetSize.Height - (double) subform.Margins.Bottom)
            {
              this.m_currentPage = this.m_currentPage.Section.Add();
              this.m_xfaPage.DrawPageBackgroundTemplate(this.m_currentPage);
              this.m_clinetSize = this.m_currentPage.GetClientSize();
              subform.currentPoint = subform.startPoint;
              if (subform.parent != null)
                this.SetCurrentPosition(subform.parent as PdfLoadedXfaForm, subform.trackingHeight);
              this.DrawBackground(subform, true);
              subform.m_size.Height -= subform.trackingHeight;
              subform.trackingHeight = 0.0f;
              subform.maxHeight = 0.0f;
            }
            this.DrawField((PdfLoadedXfaField) field, this.m_currentPage.Graphics, new RectangleF(subform.currentPoint, new SizeF(field.Width, field.Height)));
            subform.currentPoint.X += field.Width;
          }
          if ((double) subform.maxHeight < (double) field.Height)
            subform.maxHeight = field.Height;
          if ((double) subform.maxWidth < (double) field.Width)
            subform.maxWidth = field.Width;
        }
      }
    }
  }

  internal void DrawPageSet(PdfLoadedXfaForm subform, PdfTemplate template)
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    PointF empty = PointF.Empty;
    int num3 = 0;
    foreach (KeyValuePair<string, PdfXfaField> completeField in subform.Fields.CompleteFields)
    {
      if (completeField.Value is PdfLoadedXfaForm)
      {
        PdfLoadedXfaForm subform1 = completeField.Value as PdfLoadedXfaForm;
        if (subform1.Visibility != PdfXfaVisibility.Hidden)
        {
          SizeF size = new SizeF((double) subform1.Width > 0.0 ? subform1.Width : subform1.m_size.Width, (double) subform1.Height > 0.0 ? subform1.Height : subform1.m_size.Height);
          if (subform1.columnWidths.Count > 0)
            size.Width = this.GetColumnWidths(subform1.columnWidths);
          PdfTemplate template1 = new PdfTemplate(size);
          this.DrawPageSet(subform1, template1);
          if (subform1.FlowDirection == PdfLoadedXfaFlowDirection.Table)
          {
            if (subform1.Border != null)
            {
              if (subform1.Border.Visibility != PdfXfaVisibility.Hidden && (subform1.Border.LeftEdge == null || subform1.Border.LeftEdge.Visibility != PdfXfaVisibility.Hidden || subform1.Border.RightEdge != null))
                this.DrawTabeBorder(subform1, template1);
            }
            else
              this.DrawTabeBorder(subform1, template1);
          }
          if (subform.FlowDirection == PdfLoadedXfaFlowDirection.None)
          {
            if (subform.currentNode != null && subform.currentNode.Name == "subformSet")
            {
              if ((double) empty.X + (double) template1.Width > (double) subform.Width - ((double) subform.Margins.Right + (double) subform.Margins.Left))
              {
                empty.X = subform.Margins.Left;
                empty.Y += num2;
                num2 = 0.0f;
                template.Graphics.DrawPdfTemplate(template1, empty);
                empty.X += template1.Width;
              }
              else
              {
                template.Graphics.DrawPdfTemplate(template1, empty);
                empty.X += template1.Width;
              }
            }
            else
              template.Graphics.DrawPdfTemplate(template1, subform1.Location);
          }
          else if (subform.FlowDirection == PdfLoadedXfaFlowDirection.TopToBottom || subform.FlowDirection == PdfLoadedXfaFlowDirection.Table)
          {
            template.Graphics.DrawPdfTemplate(template1, empty);
            empty.Y += template1.Height;
          }
          else if (subform.FlowDirection == PdfLoadedXfaFlowDirection.LeftToRight || subform.FlowDirection == PdfLoadedXfaFlowDirection.RightToLeft)
          {
            if ((double) empty.X + (double) template1.Width > (double) subform.Width - ((double) subform.Margins.Right + (double) subform.Margins.Left))
            {
              empty.X = subform.Margins.Left;
              empty.Y += num2;
              num2 = 0.0f;
              template.Graphics.DrawPdfTemplate(template1, empty);
              empty.X += template1.Width;
            }
            else
            {
              template.Graphics.DrawPdfTemplate(template1, empty);
              empty.X += template1.Width;
            }
          }
          if ((double) num2 < (double) template1.Height)
            num2 = template1.Height;
          if ((double) num1 < (double) template1.Width)
            num1 = template1.Width;
        }
      }
      else if (completeField.Value is PdfLoadedXfaStyledField)
      {
        PdfLoadedXfaStyledField field = completeField.Value as PdfLoadedXfaStyledField;
        if (field.Visibility != PdfXfaVisibility.Hidden)
        {
          if (subform.FlowDirection == PdfLoadedXfaFlowDirection.Row)
            field.Width = (subform.parent as PdfLoadedXfaForm).columnWidths[num3++];
          if ((double) field.Height == 0.0)
          {
            if (field is PdfLoadedXfaTextBoxField)
            {
              PdfLoadedXfaTextBoxField loadedXfaTextBoxField = field as PdfLoadedXfaTextBoxField;
              if ((double) loadedXfaTextBoxField.MaximumHeight > 0.0)
                field.Height = loadedXfaTextBoxField.MaximumHeight;
              else if ((double) loadedXfaTextBoxField.MinimumHeight > 0.0)
                field.Height = loadedXfaTextBoxField.MinimumHeight;
            }
            else
            {
              if (field.currentNode.Attributes["maxH"] != null)
                field.Height = field.ConvertToPoint(field.currentNode.Attributes["maxH"].Value);
              if (field.currentNode.Attributes["minH"] != null)
                field.Height = field.ConvertToPoint(field.currentNode.Attributes["minH"].Value);
            }
          }
          if (subform.FlowDirection == PdfLoadedXfaFlowDirection.None)
            this.DrawField((PdfLoadedXfaField) field, template.Graphics, new RectangleF(field.Location, new SizeF(field.GetSize().Width, field.Height)));
          else if (subform.FlowDirection == PdfLoadedXfaFlowDirection.TopToBottom)
          {
            this.DrawField((PdfLoadedXfaField) field, template.Graphics, new RectangleF(empty, new SizeF(field.Width, field.Height)));
            empty.Y += field.Height;
          }
          else if (subform.FlowDirection == PdfLoadedXfaFlowDirection.LeftToRight || subform.FlowDirection == PdfLoadedXfaFlowDirection.RightToLeft)
          {
            if ((double) empty.X + (double) field.Width > (double) subform.Width - ((double) subform.Margins.Right + (double) subform.Margins.Left))
            {
              empty.X = subform.Margins.Left;
              empty.Y += num2;
              num2 = 0.0f;
              this.DrawField((PdfLoadedXfaField) field, template.Graphics, new RectangleF(empty, new SizeF(field.Width, field.Height)));
              empty.X += field.Width;
            }
            else
            {
              this.DrawField((PdfLoadedXfaField) field, template.Graphics, new RectangleF(empty, new SizeF(field.Width, field.Height)));
              empty.X += field.Width;
            }
          }
          else if (subform.FlowDirection == PdfLoadedXfaFlowDirection.Row)
          {
            if ((int) ((double) empty.X + (double) field.Width) > (int) ((double) subform.m_size.Width - ((double) subform.Margins.Right + (double) subform.Margins.Left)))
            {
              empty.X = subform.Margins.Left;
              empty.Y += num2;
              num2 = 0.0f;
              this.DrawField((PdfLoadedXfaField) field, template.Graphics, new RectangleF(empty, new SizeF(field.Width, field.Height)));
              empty.X += field.Width;
            }
            else
            {
              this.DrawField((PdfLoadedXfaField) field, template.Graphics, new RectangleF(empty, new SizeF(field.Width, field.Height)));
              empty.X += field.Width;
            }
          }
          if ((double) num2 < (double) field.Height)
            num2 = field.Height;
          if ((double) num1 < (double) field.Width)
            num1 = field.Width;
        }
      }
      else if (completeField.Value is PdfLoadedXfaRadioButtonGroup)
      {
        PdfLoadedXfaRadioButtonGroup field = completeField.Value as PdfLoadedXfaRadioButtonGroup;
        if (subform.FlowDirection == PdfLoadedXfaFlowDirection.Row)
          field.Width = (subform.parent as PdfLoadedXfaForm).columnWidths[num3++];
        if (field.Visibility != PdfXfaVisibility.Hidden)
        {
          if (subform.FlowDirection == PdfLoadedXfaFlowDirection.None)
          {
            this.DrawField((PdfLoadedXfaField) field, template.Graphics, new RectangleF(new PointF(field.Location.X + subform.currentPoint.X, field.Location.Y + subform.currentPoint.Y), new SizeF(field.Width, field.Height)));
            subform.trackingHeight = field.Location.Y + subform.currentPoint.Y + field.Height;
          }
          else if (subform.FlowDirection == PdfLoadedXfaFlowDirection.TopToBottom)
          {
            this.DrawField((PdfLoadedXfaField) field, template.Graphics, new RectangleF(subform.currentPoint, new SizeF(field.Width, field.Height)));
            subform.currentPoint.Y += field.Height;
            subform.trackingHeight += field.Height;
          }
          else if (subform.FlowDirection == PdfLoadedXfaFlowDirection.LeftToRight || subform.FlowDirection == PdfLoadedXfaFlowDirection.RightToLeft || subform.FlowDirection == PdfLoadedXfaFlowDirection.Row)
          {
            if ((double) (subform.currentPoint.X + field.Width) - (double) subform.cStartPoint.X > (double) (subform.m_size.Width - subform.Margins.Right))
            {
              subform.currentPoint.X = subform.startPoint.X + subform.cStartPoint.X;
              subform.currentPoint.Y += subform.maxHeight;
              subform.trackingHeight += subform.maxHeight;
              subform.maxHeight = 0.0f;
            }
            this.DrawField((PdfLoadedXfaField) field, template.Graphics, new RectangleF(subform.currentPoint, new SizeF(field.Width, field.Height)));
            subform.currentPoint.X += field.Width;
          }
          if ((double) subform.maxHeight < (double) field.Height)
            subform.maxHeight = field.Height;
          if ((double) subform.maxWidth < (double) field.Width)
            subform.maxWidth = field.Width;
        }
      }
    }
  }

  private void DrawTabeBorder(PdfLoadedXfaForm subform, PdfTemplate template)
  {
    int count = subform.Fields.CompleteFields.Count;
    List<float> floatList = new List<float>();
    foreach (KeyValuePair<string, PdfXfaField> completeField1 in subform.Fields.CompleteFields)
    {
      if (completeField1.Value is PdfLoadedXfaForm)
      {
        float num = 0.0f;
        foreach (KeyValuePair<string, PdfXfaField> completeField2 in (completeField1.Value as PdfLoadedXfaForm).Fields.CompleteFields)
        {
          if (completeField2.Value is PdfLoadedXfaStyledField && (double) (completeField2.Value as PdfLoadedXfaStyledField).GetSize().Height > (double) num)
            num = (completeField2.Value as PdfLoadedXfaStyledField).GetSize().Height;
        }
        floatList.Add(num);
      }
    }
    PdfPen pen = new PdfPen(Color.Black, 1f);
    float x1 = 0.0f;
    float num1 = 0.0f;
    template.Graphics.DrawLine(pen, x1, num1, template.Width, num1);
    for (int index = 0; index < count; ++index)
    {
      num1 += floatList[index];
      template.Graphics.DrawLine(pen, x1, num1, template.Width, num1);
    }
    float y1;
    float num2 = y1 = 0.0f;
    template.Graphics.DrawLine(pen, num2, y1, num2, template.Height);
    for (int index = 0; index < subform.columnWidths.Count; ++index)
    {
      num2 += subform.columnWidths[index];
      template.Graphics.DrawLine(pen, num2, 0.0f, num2, template.Height);
    }
  }

  private void CalculateBackgroundHeight(PdfLoadedXfaForm subform)
  {
    int num = 0;
    if (subform.PageBreak != null)
    {
      PdfLoadedXfaPageBreak pageBreak = subform.PageBreak;
      if (pageBreak.Overflow != null && pageBreak.Overflow.OverFlowID != string.Empty)
        this.m_nextPageId = subform.PageBreak.Overflow.OverFlowID;
      else if (pageBreak.BeforeBreak != null)
        this.m_nextPageId = pageBreak.BeforeBreak.BeforeTargetID;
      else if (pageBreak.BeforeTargetID != string.Empty)
        this.m_nextPageId = pageBreak.BeforeTargetID;
    }
    foreach (KeyValuePair<string, PdfXfaField> completeField in subform.Fields.CompleteFields)
    {
      if (completeField.Value is PdfLoadedXfaForm)
      {
        PdfLoadedXfaForm subform1 = completeField.Value as PdfLoadedXfaForm;
        if (subform1.columnWidths.Count > 0)
          subform1.bgSize.Width = this.GetColumnWidths(subform1.columnWidths);
        subform1.startPoint = new PointF(subform.startPoint.X + subform1.Margins.Left, subform.startPoint.Y + subform1.Margins.Top);
        SizeF sizeF = new SizeF((double) subform1.Width > 0.0 ? subform1.Width : subform1.bgSize.Width, (double) subform1.Height > 0.0 ? subform1.Height : subform1.bgSize.Height);
        if (subform.FlowDirection == PdfLoadedXfaFlowDirection.None)
        {
          if (subform.currentNode.Name == "subformSet" && (double) subform.currentPoint.X + (double) sizeF.Width >= (double) subform.bgSize.Width - ((double) subform.Margins.Right + (double) subform.Margins.Left))
          {
            subform.currentPoint.X = subform.startPoint.X;
            subform.currentPoint.Y += subform.maxHeight;
            subform.trackingHeight += subform.maxHeight;
            subform.maxHeight = 0.0f;
          }
        }
        else if ((subform.FlowDirection == PdfLoadedXfaFlowDirection.LeftToRight || subform.FlowDirection == PdfLoadedXfaFlowDirection.RightToLeft) && (double) subform.currentPoint.X + (double) sizeF.Width > (double) subform.bgSize.Width - ((double) subform.Margins.Right + (double) subform.Margins.Left))
        {
          subform.currentPoint.X = subform.startPoint.X;
          subform.currentPoint.Y += subform.maxHeight;
          subform.trackingHeight += subform.maxHeight;
          subform.maxHeight = 0.0f;
        }
        bool flag1 = false;
        bool flag2 = false;
        if ((double) subform.currentPoint.Y + (double) subform1.bgSize.Height > (double) this.m_clinetSize.Height - ((double) subform.Margins.Top + (double) subform.Margins.Bottom) && subform1.currentNode["keep"] != null && subform1.FlowDirection != PdfLoadedXfaFlowDirection.Table && subform1.currentNode["keep"].Attributes["intact"] != null && subform1.currentNode["keep"].Attributes["intact"].Value == "contentArea")
          flag1 = true;
        if (subform1.FlowDirection == PdfLoadedXfaFlowDirection.Table && (double) subform.currentPoint.Y + (double) subform1.bgSize.Height > (double) this.m_clinetSize.Height - ((double) subform.Margins.Top + (double) subform.Margins.Bottom))
          flag2 = true;
        if (subform1.PageBreak != null)
        {
          string str = string.Empty;
          PdfLoadedXfaPageBreak pageBreak = subform1.PageBreak;
          bool flag3 = false;
          if (pageBreak.BeforeBreak != null)
          {
            str = pageBreak.BeforeBreak.BeforeTargetID;
            if (pageBreak.BeforeBreak.IsStartNew)
              flag3 = true;
          }
          else if (pageBreak.BeforeTargetID != string.Empty)
            str = pageBreak.BeforeTargetID;
          if (str != string.Empty || flag3)
          {
            if (this.m_pageCollection.ContainsKey(str))
            {
              this.m_xfaPage = this.m_pageCollection[str];
            }
            else
            {
              PdfLoadedXfaPage pageById = this.GetPageByID(str);
              if (pageById != null)
                this.m_xfaPage = pageById;
            }
            this.m_nextBgPageId = string.Empty;
            this.m_clinetSize = this.m_xfaPage.GetClientSize();
            subform.currentPoint = subform.startPoint;
            if (subform.parent != null)
              this.SetCurrentBackgroundHeight(subform.parent as PdfLoadedXfaForm, subform.trackingHeight + subform.Margins.Bottom);
            subform.bgSize.Height -= subform.trackingHeight;
            subform.backgroundHeight.Add(subform.trackingHeight + subform.Margins.Bottom);
            subform.trackingHeight = 0.0f;
            subform.maxHeight = 0.0f;
          }
        }
        else if (flag1 || flag2)
        {
          if (this.m_nextBgPageId != string.Empty)
          {
            if (this.m_pageCollection.ContainsKey(this.m_nextBgPageId))
            {
              this.m_xfaPage = this.m_pageCollection[this.m_nextBgPageId];
            }
            else
            {
              PdfLoadedXfaPage pageById = this.GetPageByID(this.m_nextBgPageId);
              if (pageById != null)
                this.m_xfaPage = pageById;
            }
          }
          this.m_nextBgPageId = string.Empty;
          this.m_clinetSize = this.m_xfaPage.GetClientSize();
          subform.currentPoint = subform.startPoint;
          if (subform.parent != null)
            this.SetCurrentBackgroundHeight(subform.parent as PdfLoadedXfaForm, subform.trackingHeight + subform.Margins.Bottom);
          subform.bgSize.Height -= subform.trackingHeight;
          subform.backgroundHeight.Add(subform.trackingHeight + subform.Margins.Bottom);
          subform.trackingHeight = 0.0f;
          subform.maxHeight = 0.0f;
        }
        subform1.currentPoint = new PointF(subform1.startPoint.X + subform.currentPoint.X, subform1.startPoint.Y + subform.currentPoint.Y);
        if (subform.FlowDirection == PdfLoadedXfaFlowDirection.None && subform.currentNode.Name != "subformSet")
        {
          subform1.currentPoint.X += subform1.Location.X;
          subform1.currentPoint.Y += subform1.Location.Y;
        }
        subform1.cStartPoint = subform1.currentPoint;
        this.CalculateBackgroundHeight(subform1);
        if (subform1.FlowDirection == PdfLoadedXfaFlowDirection.None && subform1.currentNode.Name != "subformSet")
          subform1.bgSize = new SizeF(subform1.Width, subform1.Height);
        if (subform.FlowDirection == PdfLoadedXfaFlowDirection.None)
        {
          if (subform.currentNode.Name == "subformSet")
          {
            if ((double) subform.currentPoint.X + (double) sizeF.Width > (double) subform.bgSize.Width - ((double) subform.Margins.Right + (double) subform.Margins.Left))
            {
              subform.currentPoint.X = subform.startPoint.X;
              subform.currentPoint.Y += subform.maxHeight;
              subform.trackingHeight += subform.maxHeight;
              subform.maxHeight = 0.0f;
            }
            else
              subform.currentPoint.X += subform1.bgSize.Width;
          }
        }
        else if (subform.FlowDirection == PdfLoadedXfaFlowDirection.LeftToRight || subform.FlowDirection == PdfLoadedXfaFlowDirection.RightToLeft)
        {
          if ((double) subform.currentPoint.X + (double) sizeF.Width > (double) subform.bgSize.Width - ((double) subform.Margins.Right + (double) subform.Margins.Left))
          {
            subform.currentPoint.X = subform.startPoint.X;
            subform.currentPoint.Y += subform.maxHeight;
            subform.trackingHeight += subform.maxHeight;
            subform.maxHeight = 0.0f;
          }
          else
            subform.currentPoint.X += sizeF.Width;
        }
        else
        {
          subform.currentPoint.Y += subform1.bgSize.Height;
          subform.trackingHeight += subform1.bgSize.Height;
        }
        if ((double) subform.maxHeight < (double) subform1.bgSize.Height)
          subform.maxHeight = subform1.bgSize.Height;
        if ((double) subform.maxWidth < (double) subform1.bgSize.Width)
          subform.maxWidth = subform1.bgSize.Width;
      }
      else if (completeField.Value is PdfLoadedXfaStyledField)
      {
        PdfLoadedXfaStyledField loadedXfaStyledField = completeField.Value as PdfLoadedXfaStyledField;
        if (subform.FlowDirection == PdfLoadedXfaFlowDirection.Row)
          loadedXfaStyledField.Width = (subform.parent as PdfLoadedXfaForm).columnWidths[num++];
        if ((double) loadedXfaStyledField.Height == 0.0)
        {
          if (loadedXfaStyledField is PdfLoadedXfaTextBoxField)
          {
            PdfLoadedXfaTextBoxField loadedXfaTextBoxField = loadedXfaStyledField as PdfLoadedXfaTextBoxField;
            SizeF fieldSize = loadedXfaTextBoxField.GetFieldSize();
            loadedXfaTextBoxField.Height = fieldSize.Height;
          }
          else
          {
            if (loadedXfaStyledField.currentNode.Attributes["maxH"] != null)
              loadedXfaStyledField.Height = loadedXfaStyledField.ConvertToPoint(loadedXfaStyledField.currentNode.Attributes["maxH"].Value);
            if (loadedXfaStyledField.currentNode.Attributes["minH"] != null)
              loadedXfaStyledField.Height = loadedXfaStyledField.ConvertToPoint(loadedXfaStyledField.currentNode.Attributes["minH"].Value);
          }
        }
        if (loadedXfaStyledField.Visibility != PdfXfaVisibility.Hidden)
        {
          if (subform.FlowDirection == PdfLoadedXfaFlowDirection.None)
          {
            if ((double) subform.currentPoint.Y + ((double) loadedXfaStyledField.Location.Y - (double) subform.extraSize) + (double) loadedXfaStyledField.Height > (double) this.m_clinetSize.Height - (double) subform.Margins.Bottom)
            {
              if (this.m_nextBgPageId != string.Empty)
              {
                if (this.m_pageCollection.ContainsKey(this.m_nextBgPageId))
                {
                  this.m_xfaPage = this.m_pageCollection[this.m_nextBgPageId];
                }
                else
                {
                  PdfLoadedXfaPage pageById = this.GetPageByID(this.m_nextBgPageId);
                  if (pageById != null)
                    this.m_xfaPage = pageById;
                }
                this.m_nextBgPageId = string.Empty;
              }
              this.m_clinetSize = this.m_xfaPage.GetClientSize();
              subform.currentPoint = subform.startPoint;
              subform.extraSize = subform.trackingHeight;
              if (subform.parent != null)
                this.SetCurrentBackgroundHeight(subform.parent as PdfLoadedXfaForm, subform.trackingHeight + subform.Margins.Bottom);
              subform.bgSize.Height -= subform.trackingHeight;
              subform.backgroundHeight.Add(subform.trackingHeight + subform.Margins.Bottom);
              subform.trackingHeight = 0.0f;
              subform.maxHeight = 0.0f;
            }
            subform.trackingHeight = loadedXfaStyledField.Location.Y + loadedXfaStyledField.Height;
          }
          else if (subform.FlowDirection == PdfLoadedXfaFlowDirection.TopToBottom)
          {
            if ((double) subform.currentPoint.Y + (double) loadedXfaStyledField.Height > (double) this.m_clinetSize.Height - (double) subform.Margins.Bottom)
            {
              if (this.m_nextBgPageId != string.Empty)
              {
                if (this.m_pageCollection.ContainsKey(this.m_nextBgPageId))
                {
                  this.m_xfaPage = this.m_pageCollection[this.m_nextBgPageId];
                }
                else
                {
                  PdfLoadedXfaPage pageById = this.GetPageByID(this.m_nextBgPageId);
                  if (pageById != null)
                    this.m_xfaPage = pageById;
                }
                this.m_nextBgPageId = string.Empty;
              }
              this.m_clinetSize = this.m_xfaPage.GetClientSize();
              subform.currentPoint = subform.startPoint;
              if (subform.parent != null)
                this.SetCurrentBackgroundHeight(subform.parent as PdfLoadedXfaForm, subform.trackingHeight + subform.Margins.Bottom);
              subform.bgSize.Height -= subform.trackingHeight;
              subform.backgroundHeight.Add(subform.trackingHeight + subform.Margins.Bottom);
              subform.trackingHeight = 0.0f;
              subform.maxHeight = 0.0f;
            }
            subform.currentPoint.Y += loadedXfaStyledField.Height;
            subform.trackingHeight += loadedXfaStyledField.Height;
          }
          else if (subform.FlowDirection == PdfLoadedXfaFlowDirection.LeftToRight || subform.FlowDirection == PdfLoadedXfaFlowDirection.RightToLeft || subform.FlowDirection == PdfLoadedXfaFlowDirection.Row)
          {
            if ((double) (subform.currentPoint.X + loadedXfaStyledField.Width - subform.cStartPoint.X) > (double) (subform.bgSize.Width - subform.Margins.Right))
            {
              subform.currentPoint.X = subform.startPoint.X + subform.cStartPoint.X;
              subform.currentPoint.Y += subform.maxHeight;
              subform.trackingHeight += subform.maxHeight;
              subform.maxHeight = 0.0f;
            }
            if ((double) subform.currentPoint.Y + (double) loadedXfaStyledField.Height > (double) this.m_clinetSize.Height - (double) subform.Margins.Bottom)
            {
              if (this.m_nextBgPageId != string.Empty)
              {
                if (this.m_pageCollection.ContainsKey(this.m_nextBgPageId))
                {
                  this.m_xfaPage = this.m_pageCollection[this.m_nextBgPageId];
                }
                else
                {
                  PdfLoadedXfaPage pageById = this.GetPageByID(this.m_nextBgPageId);
                  if (pageById != null)
                    this.m_xfaPage = pageById;
                }
                this.m_nextBgPageId = string.Empty;
              }
              this.m_clinetSize = this.m_xfaPage.GetClientSize();
              subform.currentPoint = subform.startPoint;
              if (subform.parent != null)
                this.SetCurrentBackgroundHeight(subform.parent as PdfLoadedXfaForm, subform.trackingHeight + subform.Margins.Bottom);
              subform.bgSize.Height -= subform.trackingHeight;
              subform.backgroundHeight.Add(subform.trackingHeight + subform.Margins.Bottom);
              subform.trackingHeight = 0.0f;
              subform.maxHeight = 0.0f;
            }
            subform.currentPoint.X += loadedXfaStyledField.Width;
          }
          if ((double) subform.maxHeight < (double) loadedXfaStyledField.Height)
            subform.maxHeight = loadedXfaStyledField.Height;
          if ((double) subform.maxWidth < (double) loadedXfaStyledField.Width)
            subform.maxWidth = loadedXfaStyledField.Width;
        }
      }
      else if (completeField.Value is PdfLoadedXfaRadioButtonGroup)
      {
        PdfLoadedXfaRadioButtonGroup radioButtonGroup = completeField.Value as PdfLoadedXfaRadioButtonGroup;
        if (subform.FlowDirection == PdfLoadedXfaFlowDirection.Row)
          radioButtonGroup.Width = (subform.parent as PdfLoadedXfaForm).columnWidths[num++];
        if (radioButtonGroup.Visibility != PdfXfaVisibility.Hidden)
        {
          if (subform.FlowDirection == PdfLoadedXfaFlowDirection.None)
          {
            if ((double) subform.currentPoint.Y + (double) radioButtonGroup.Location.Y + (double) radioButtonGroup.Height > (double) this.m_clinetSize.Height - (double) subform.Margins.Bottom)
            {
              if (this.m_nextBgPageId != string.Empty)
              {
                if (this.m_pageCollection.ContainsKey(this.m_nextBgPageId))
                {
                  this.m_xfaPage = this.m_pageCollection[this.m_nextBgPageId];
                }
                else
                {
                  PdfLoadedXfaPage pageById = this.GetPageByID(this.m_nextBgPageId);
                  if (pageById != null)
                    this.m_xfaPage = pageById;
                }
                this.m_nextBgPageId = string.Empty;
              }
              this.m_clinetSize = this.m_xfaPage.GetClientSize();
              subform.currentPoint = subform.startPoint;
              if (subform.parent != null)
                this.SetCurrentBackgroundHeight(subform.parent as PdfLoadedXfaForm, subform.trackingHeight + subform.Margins.Bottom);
              subform.bgSize.Height -= subform.trackingHeight;
              subform.backgroundHeight.Add(subform.trackingHeight + subform.Margins.Bottom);
              subform.trackingHeight = 0.0f;
              subform.maxHeight = 0.0f;
            }
            subform.trackingHeight = radioButtonGroup.Location.Y + subform.currentPoint.Y + radioButtonGroup.Height;
          }
          else if (subform.FlowDirection == PdfLoadedXfaFlowDirection.TopToBottom)
          {
            if ((double) subform.currentPoint.Y + (double) radioButtonGroup.Height > (double) this.m_clinetSize.Height - (double) subform.Margins.Bottom)
            {
              if (this.m_nextBgPageId != string.Empty)
              {
                if (this.m_pageCollection.ContainsKey(this.m_nextBgPageId))
                {
                  this.m_xfaPage = this.m_pageCollection[this.m_nextBgPageId];
                }
                else
                {
                  PdfLoadedXfaPage pageById = this.GetPageByID(this.m_nextBgPageId);
                  if (pageById != null)
                    this.m_xfaPage = pageById;
                }
                this.m_nextBgPageId = string.Empty;
              }
              this.m_clinetSize = this.m_xfaPage.GetClientSize();
              subform.currentPoint = subform.startPoint;
              if (subform.parent != null)
                this.SetCurrentBackgroundHeight(subform.parent as PdfLoadedXfaForm, subform.trackingHeight + subform.Margins.Bottom);
              subform.bgSize.Height -= subform.trackingHeight;
              subform.backgroundHeight.Add(subform.trackingHeight + subform.Margins.Bottom);
              subform.trackingHeight = 0.0f;
              subform.maxHeight = 0.0f;
            }
            subform.currentPoint.Y += radioButtonGroup.Height;
            subform.trackingHeight += radioButtonGroup.Height;
          }
          else if (subform.FlowDirection == PdfLoadedXfaFlowDirection.LeftToRight || subform.FlowDirection == PdfLoadedXfaFlowDirection.RightToLeft || subform.FlowDirection == PdfLoadedXfaFlowDirection.Row)
          {
            if ((double) (subform.currentPoint.X + radioButtonGroup.Width) > (double) (subform.bgSize.Width - subform.Margins.Right))
            {
              subform.currentPoint.X = subform.startPoint.X + subform.cStartPoint.X;
              subform.currentPoint.Y += subform.maxHeight;
              subform.trackingHeight += subform.maxHeight;
              subform.maxHeight = 0.0f;
            }
            if ((double) subform.currentPoint.Y + (double) radioButtonGroup.Height > (double) this.m_clinetSize.Height - (double) subform.Margins.Bottom)
            {
              if (this.m_nextBgPageId != string.Empty)
              {
                if (this.m_pageCollection.ContainsKey(this.m_nextBgPageId))
                {
                  this.m_xfaPage = this.m_pageCollection[this.m_nextBgPageId];
                }
                else
                {
                  PdfLoadedXfaPage pageById = this.GetPageByID(this.m_nextBgPageId);
                  if (pageById != null)
                    this.m_xfaPage = pageById;
                }
                this.m_nextBgPageId = string.Empty;
              }
              this.m_clinetSize = this.m_xfaPage.GetClientSize();
              subform.currentPoint = subform.startPoint;
              if (subform.parent != null)
                this.SetCurrentBackgroundHeight(subform.parent as PdfLoadedXfaForm, subform.trackingHeight + subform.Margins.Bottom);
              subform.bgSize.Height -= subform.trackingHeight;
              subform.backgroundHeight.Add(subform.trackingHeight + subform.Margins.Bottom);
              subform.trackingHeight = 0.0f;
              subform.maxHeight = 0.0f;
            }
            subform.currentPoint.X += radioButtonGroup.Width;
          }
          if ((double) subform.maxHeight < (double) radioButtonGroup.Height)
            subform.maxHeight = radioButtonGroup.Height;
          if ((double) subform.maxWidth < (double) radioButtonGroup.Width)
            subform.maxWidth = radioButtonGroup.Width;
        }
      }
    }
  }
}
