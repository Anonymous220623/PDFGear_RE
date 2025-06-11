// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.MetafileLayouter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics.Images.Metafiles;
using Syncfusion.Pdf.HtmlToPdf;
using Syncfusion.Pdf.Interactive;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

internal class MetafileLayouter(PdfMetafile element) : ShapeLayouter((PdfShapeElement) element)
{
  private List<string> IRadioButtons = new List<string>();
  private ArrayList InputRadioGroup = new ArrayList();

  public PdfMetafile Element => base.Element as PdfMetafile;

  private TextRegionManager TextRegions => this.Element.TextRegions;

  private ImageRegionManager ImageRegions => this.Element.ImageRegions;

  internal void RepositionLinks(ArrayList list, float height)
  {
    foreach (HtmlHyperLink htmlHyperLink in list)
      this.Element.HtmlHyperlinksCollection.Remove((object) htmlHyperLink);
    list.Clear();
    list = this.Element.HtmlHyperlinksCollection.Clone() as ArrayList;
    this.Element.HtmlHyperlinksCollection.Clear();
    foreach (HtmlHyperLink htmlHyperLink in list)
    {
      float y = htmlHyperLink.Bounds.Y - height;
      htmlHyperLink.Bounds = new RectangleF(htmlHyperLink.Bounds.X, y, htmlHyperLink.Bounds.Width, htmlHyperLink.Bounds.Height);
      this.Element.HtmlHyperlinksCollection.Add((object) htmlHyperLink);
    }
  }

  protected override RectangleF CheckCorrectCurrentBounds(
    PdfPage currentPage,
    RectangleF currentBounds,
    RectangleF shapeLayoutBounds,
    PdfLayoutParams param)
  {
    if (param == null)
      throw new ArgumentNullException(nameof (param));
    RectangleF rectangleF = base.CheckCorrectCurrentBounds(currentPage, currentBounds, shapeLayoutBounds, param);
    bool flag1 = !(param.Format is PdfMetafileLayoutFormat format) || format.SplitTextLines;
    bool flag2 = format != null && format.SplitImages;
    bool flag3 = format != null && format.IsHTMLPageBreak;
    if (format != null && format.m_enableDirectLayout)
    {
      flag2 = true;
      flag1 = true;
    }
    if (!this.IsImagePath)
    {
      SizeF sizeF;
      if (this.TextRegions != null && !flag1 && !flag3 && (double) rectangleF.Height > 0.0)
      {
        float num1 = shapeLayoutBounds.Y + rectangleF.Height;
        PdfUnitConvertor pdfUnitConvertor = new PdfUnitConvertor(this.Element.VerticalResolution);
        float topCoordinate1 = this.TextRegions.GetTopCoordinate(pdfUnitConvertor.ConvertToPixels(num1, PdfGraphicsUnit.Point));
        bool flag4 = (double) topCoordinate1 != 0.0;
        double height1 = (double) rectangleF.Height;
        sizeF = currentPage.GetClientSize();
        double height2 = (double) sizeF.Height;
        if (height1 > height2)
          topCoordinate1 = this.TextRegions.GetTopCoordinate(topCoordinate1 - 2f);
        float num2 = pdfUnitConvertor.ConvertFromPixels(topCoordinate1, PdfGraphicsUnit.Point);
        float num3 = 0.0f;
        if ((double) num2 > (double) shapeLayoutBounds.Y)
          num3 = num2 - shapeLayoutBounds.Y;
        else if (flag4)
          num3 = currentPage.GetClientSize().Height;
        rectangleF.Height = currentPage == null || (double) currentPage.GetClientSize().Height >= (double) num3 ? num3 : currentPage.GetClientSize().Height;
        if ((double) rectangleF.Y != 0.0)
        {
          float num4 = rectangleF.Y + num3;
          if ((double) num4 > (double) currentPage.GetClientSize().Height)
          {
            sizeF = currentPage.GetClientSize();
            float height3 = sizeF.Height;
            float num5 = num4 - height3;
            rectangleF.Height = num3 - num5;
            float num6 = shapeLayoutBounds.Y + rectangleF.Height;
            float topCoordinate2 = this.TextRegions.GetTopCoordinate(pdfUnitConvertor.ConvertToPixels(num6, PdfGraphicsUnit.Point));
            float num7 = pdfUnitConvertor.ConvertFromPixels(topCoordinate2, PdfGraphicsUnit.Point);
            if ((double) num7 > (double) shapeLayoutBounds.Y)
              num3 = num7 - shapeLayoutBounds.Y;
            ref RectangleF local = ref rectangleF;
            double num8;
            if (currentPage != null)
            {
              sizeF = currentPage.GetClientSize();
              if ((double) sizeF.Height < (double) num3)
              {
                sizeF = currentPage.GetClientSize();
                num8 = (double) sizeF.Height;
                goto label_20;
              }
            }
            num8 = (double) num3;
label_20:
            local.Height = (float) num8;
          }
        }
      }
      if (this.ImageRegions != null && !flag2 && !flag3 && (double) rectangleF.Height > 0.0)
      {
        float height4 = rectangleF.Height;
        float num9 = shapeLayoutBounds.Y + rectangleF.Height;
        PdfUnitConvertor pdfUnitConvertor = new PdfUnitConvertor(this.Element.VerticalResolution);
        float topCoordinate3 = this.ImageRegions.GetTopCoordinate(pdfUnitConvertor.ConvertToPixels(num9, PdfGraphicsUnit.Point));
        float num10 = pdfUnitConvertor.ConvertFromPixels(topCoordinate3, PdfGraphicsUnit.Point);
        if (Math.Round((double) num10) != Math.Round((double) shapeLayoutBounds.Y + (double) rectangleF.Height))
          num10 = (float) Math.Floor((double) num10);
        float num11 = 0.0f;
        if ((double) num10 > (double) shapeLayoutBounds.Y)
          num11 = num10 - shapeLayoutBounds.Y;
        if ((double) num11 == 0.0 || this.TextRegions.Count == 0)
        {
          rectangleF.Height = height4;
        }
        else
        {
          PdfPage page = param.Page;
          double height5 = (double) shapeLayoutBounds.Height;
          sizeF = page.Size;
          double height6 = (double) sizeF.Height;
          if (height5 > height6)
            rectangleF.Height = num11;
          if (this.TextRegions != null && !flag1)
          {
            float num12 = shapeLayoutBounds.Y + rectangleF.Height;
            float topCoordinate4 = this.TextRegions.GetTopCoordinate(pdfUnitConvertor.ConvertToPixels(num12, PdfGraphicsUnit.Point));
            float num13 = pdfUnitConvertor.ConvertFromPixels(topCoordinate4, PdfGraphicsUnit.Point);
            if ((double) num13 > (double) shapeLayoutBounds.Y)
              num11 = num13 - shapeLayoutBounds.Y;
            ref RectangleF local = ref rectangleF;
            double num14;
            if (currentPage != null)
            {
              sizeF = currentPage.GetClientSize();
              if ((double) sizeF.Height < (double) num11)
              {
                sizeF = currentPage.GetClientSize();
                num14 = (double) sizeF.Height;
                goto label_37;
              }
            }
            num14 = (double) num11;
label_37:
            local.Height = (float) num14;
            if ((double) rectangleF.Height == 0.0)
            {
              sizeF = currentPage.GetClientSize();
              if ((double) sizeF.Height > (double) num11)
                rectangleF.Height = height4;
            }
            else if ((double) currentBounds.Height != 0.0)
            {
              sizeF = currentPage.GetClientSize();
              if ((double) sizeF.Height > (double) num11 && (double) (int) currentBounds.Height == 2.0 + (double) (int) rectangleF.Height)
                rectangleF.Height = currentBounds.Height;
            }
          }
          else
            rectangleF.Height = num11;
        }
      }
    }
    ArrayList list = new ArrayList();
    foreach (HtmlHyperLink htmlHyperlinks in this.Element.HtmlHyperlinksCollection)
    {
      if ((double) rectangleF.Height > (double) htmlHyperlinks.Bounds.Y)
      {
        if (string.IsNullOrEmpty(htmlHyperlinks.Hash))
        {
          PdfUriAnnotation annotation = new PdfUriAnnotation(htmlHyperlinks.Bounds, htmlHyperlinks.Href);
          annotation.Border.Width = 0.0f;
          currentPage.Annotations.Add((PdfAnnotation) annotation);
        }
        else
        {
          PdfDocumentLinkAnnotation annotation = new PdfDocumentLinkAnnotation(htmlHyperlinks.Bounds);
          annotation.Border.Width = 0.0f;
          annotation.ApplyText(htmlHyperlinks.Hash);
          currentPage.Annotations.Add((PdfAnnotation) annotation);
        }
        list.Add((object) htmlHyperlinks);
      }
    }
    foreach (HtmlHyperLink documentLinks in this.Element.DocumentLinksCollection)
    {
      float height = rectangleF.Height;
      double y1 = (double) shapeLayoutBounds.Y;
      RectangleF bounds1 = documentLinks.Bounds;
      double y2 = (double) bounds1.Y;
      if (y1 < y2)
      {
        double num = (double) height + (double) shapeLayoutBounds.Y;
        bounds1 = documentLinks.Bounds;
        double y3 = (double) bounds1.Y;
        if (num > y3)
        {
          RectangleF bounds2 = documentLinks.Bounds;
          bounds2.Y -= shapeLayoutBounds.Y;
          PdfUriAnnotation annotation = new PdfUriAnnotation(bounds2);
          annotation.ApplyText(documentLinks.Name);
          currentPage.Annotations.Add((PdfAnnotation) annotation);
          list.Add((object) documentLinks);
        }
      }
    }
    if (this.Element.InputElementCollection != null)
    {
      PdfRadioButtonListField radioButtonListField = new PdfRadioButtonListField((PdfPageBase) currentPage, "name");
      foreach (HtmlFormElement inputElement in this.Element.InputElementCollection)
      {
        float height7 = rectangleF.Height;
        double y4 = (double) shapeLayoutBounds.Y;
        RectangleF bounds3 = inputElement.Bounds;
        double y5 = (double) bounds3.Y;
        if (y4 <= y5)
        {
          double num15 = (double) height7 + (double) shapeLayoutBounds.Y;
          bounds3 = inputElement.Bounds;
          double y6 = (double) bounds3.Y;
          if (num15 >= y6)
          {
            bounds3 = inputElement.Bounds;
            double y7 = (double) bounds3.Y;
            bounds3 = inputElement.Bounds;
            double height8 = (double) bounds3.Height;
            if (y7 + height8 > (double) currentPage.Graphics.ClientSize.Height + (double) shapeLayoutBounds.Y)
            {
              ref RectangleF local = ref rectangleF;
              bounds3 = inputElement.Bounds;
              double num16 = (double) bounds3.Y - 0.0099999997764825821 - (double) shapeLayoutBounds.Y;
              local.Height = (float) num16;
              break;
            }
            RectangleF bounds4 = inputElement.Bounds;
            bounds4.Y -= shapeLayoutBounds.Y;
            if (inputElement.Type == "text")
            {
              PdfTextBoxField field = new PdfTextBoxField((PdfPageBase) currentPage, inputElement.Name);
              field.Bounds = bounds4;
              field.BackRectColor = (PdfColor) inputElement.BackRectColor;
              field.BackColor = (PdfColor) inputElement.BackgroundColor;
              field.ForeColor = (PdfColor) inputElement.TextColor;
              field.BorderColor = (PdfColor) inputElement.BorderColor;
              if (inputElement.Align == "right")
                field.TextAlignment = PdfTextAlignment.Right;
              else if (inputElement.Align == "left" || inputElement.Align == null)
                field.TextAlignment = PdfTextAlignment.Left;
              else if (inputElement.Align == "center")
                field.TextAlignment = PdfTextAlignment.Center;
              else if (inputElement.Align == "justify")
                field.TextAlignment = PdfTextAlignment.Justify;
              field.Text = inputElement.Value == null ? "" : inputElement.Value;
              field.MaxLength = inputElement.MaxLength;
              field.ReadOnly = inputElement.ReadOnly;
              currentPage.Document.Form.Fields.Add((PdfField) field);
            }
            else if (inputElement.Type == "radio")
            {
              PdfRadioButtonListField field;
              if (!this.InputRadioGroup.Contains((object) $"{inputElement.Name}:P:{inputElement.ParentName}"))
              {
                this.InputRadioGroup.Add((object) $"{inputElement.Name}:P:{inputElement.ParentName}");
                this.InputRadioGroup.Add((object) 0);
                this.InputRadioGroup.Add((object) (inputElement.Checked ? 0 : -1));
                field = new PdfRadioButtonListField((PdfPageBase) currentPage, inputElement.Name);
              }
              else
              {
                field = this.InputRadioGroup[this.InputRadioGroup.IndexOf((object) $"{inputElement.Name}:P:{inputElement.ParentName}") + 3] as PdfRadioButtonListField;
                field.Page = (PdfPageBase) currentPage;
                this.InputRadioGroup[this.InputRadioGroup.IndexOf((object) $"{inputElement.Name}:P:{inputElement.ParentName}") + 1] = (object) (Convert.ToInt32(this.InputRadioGroup[this.InputRadioGroup.IndexOf((object) $"{inputElement.Name}:P:{inputElement.ParentName}") + 1]) + 1);
                if (inputElement.Checked)
                  this.InputRadioGroup[this.InputRadioGroup.IndexOf((object) $"{inputElement.Name}:P:{inputElement.ParentName}") + 2] = this.InputRadioGroup[this.InputRadioGroup.IndexOf((object) $"{inputElement.Name}:P:{inputElement.ParentName}") + 1];
              }
              if (!this.IRadioButtons.Contains($"{inputElement.Name}:P:{inputElement.ParentName}"))
                this.IRadioButtons.Add($"{inputElement.Name}:P:{inputElement.ParentName}");
              PdfRadioButtonListItem radioButtonListItem = new PdfRadioButtonListItem(inputElement.Value);
              radioButtonListItem.Bounds = bounds4;
              radioButtonListItem.BackRectColor = (PdfColor) inputElement.BackRectColor;
              radioButtonListItem.Style = PdfCheckBoxStyle.Circle;
              field.Items.Add(radioButtonListItem);
              if (Convert.ToInt32(this.InputRadioGroup[this.InputRadioGroup.IndexOf((object) $"{inputElement.Name}:P:{inputElement.ParentName}") + 2]) != -1)
                field.SelectedIndex = Convert.ToInt32(this.InputRadioGroup[this.InputRadioGroup.IndexOf((object) $"{inputElement.Name}:P:{inputElement.ParentName}") + 2]);
              if (Convert.ToInt32(this.InputRadioGroup[this.InputRadioGroup.IndexOf((object) $"{inputElement.Name}:P:{inputElement.ParentName}") + 1]) == 0)
                this.InputRadioGroup.Add((object) field);
              else
                this.InputRadioGroup[this.InputRadioGroup.IndexOf((object) $"{inputElement.Name}:P:{inputElement.ParentName}") + 3] = (object) field;
              currentPage.Document.Form.Fields.Add((PdfField) field);
            }
            else if (inputElement.Type == "textarea")
            {
              PdfTextBoxField field = new PdfTextBoxField((PdfPageBase) currentPage, inputElement.Name);
              field.Multiline = true;
              field.Bounds = bounds4;
              field.BackRectColor = (PdfColor) inputElement.BackRectColor;
              field.BackColor = (PdfColor) inputElement.BackgroundColor;
              field.ForeColor = (PdfColor) inputElement.TextColor;
              field.BorderColor = (PdfColor) inputElement.BorderColor;
              if (inputElement.Align == "right")
                field.TextAlignment = PdfTextAlignment.Right;
              else if (inputElement.Align == "left" || inputElement.Align == null)
                field.TextAlignment = PdfTextAlignment.Left;
              else if (inputElement.Align == "center")
                field.TextAlignment = PdfTextAlignment.Center;
              else if (inputElement.Align == "justify")
                field.TextAlignment = PdfTextAlignment.Justify;
              field.ReadOnly = inputElement.ReadOnly;
              field.Text = inputElement.Value == null ? "" : inputElement.Value;
              currentPage.Document.Form.Fields.Add((PdfField) field);
            }
            else if (inputElement.Type == "submit" || inputElement.Type == "button" || inputElement.Type == "reset")
            {
              PdfButtonField field = new PdfButtonField((PdfPageBase) currentPage, inputElement.Name);
              field.Bounds = bounds4;
              field.BackRectColor = (PdfColor) inputElement.BackRectColor;
              field.Text = inputElement.Value == null ? "" : inputElement.Value;
              currentPage.Document.Form.Fields.Add((PdfField) field);
            }
            else if (inputElement.Type == "checkbox")
            {
              PdfCheckBoxField field = new PdfCheckBoxField((PdfPageBase) currentPage, inputElement.Name);
              field.Bounds = bounds4;
              field.BackRectColor = (PdfColor) inputElement.BackRectColor;
              field.Checked = inputElement.Checked;
              RectangleF bounds5 = field.Bounds;
              field.Bounds = bounds5;
              currentPage.Document.Form.Fields.Add((PdfField) field);
            }
            else if (inputElement.Type == "password")
            {
              PdfTextBoxField field = new PdfTextBoxField((PdfPageBase) currentPage, inputElement.Name);
              field.Multiline = true;
              field.Bounds = bounds4;
              field.BackRectColor = (PdfColor) inputElement.BackRectColor;
              field.ReadOnly = inputElement.ReadOnly;
              field.Text = inputElement.Value == null ? "" : inputElement.Value;
              currentPage.Document.Form.Fields.Add((PdfField) field);
            }
            else if (inputElement.Type == "number")
            {
              PdfTextBoxField field = new PdfTextBoxField((PdfPageBase) currentPage, inputElement.Name);
              field.Multiline = true;
              field.Bounds = bounds4;
              field.BackRectColor = (PdfColor) inputElement.BackRectColor;
              field.ReadOnly = inputElement.ReadOnly;
              field.Text = inputElement.Value == null ? "" : inputElement.Value;
              currentPage.Document.Form.Fields.Add((PdfField) field);
            }
          }
        }
      }
    }
    if (this.Element.SelectElementCollection != null)
    {
      foreach (HtmlFormElement selectElement in this.Element.SelectElementCollection)
      {
        float height9 = rectangleF.Height;
        double y8 = (double) shapeLayoutBounds.Y;
        RectangleF bounds6 = selectElement.Bounds;
        double y9 = (double) bounds6.Y;
        if (y8 <= y9)
        {
          double num17 = (double) height9 + (double) shapeLayoutBounds.Y;
          bounds6 = selectElement.Bounds;
          double y10 = (double) bounds6.Y;
          if (num17 >= y10)
          {
            bounds6 = selectElement.Bounds;
            double y11 = (double) bounds6.Y;
            bounds6 = selectElement.Bounds;
            double height10 = (double) bounds6.Height;
            if (y11 + height10 > (double) currentPage.Graphics.ClientSize.Height + (double) shapeLayoutBounds.Y)
            {
              ref RectangleF local = ref rectangleF;
              bounds6 = selectElement.Bounds;
              double num18 = (double) bounds6.Y - 0.0099999997764825821 - (double) shapeLayoutBounds.Y;
              local.Height = (float) num18;
              break;
            }
            if (selectElement.Type == "select-one")
            {
              PdfComboBoxField field = new PdfComboBoxField((PdfPageBase) currentPage, selectElement.Name);
              RectangleF bounds7 = selectElement.Bounds;
              bounds7.Y -= shapeLayoutBounds.Y;
              field.Bounds = bounds7;
              field.BackRectColor = (PdfColor) selectElement.BackRectColor;
              for (int index = 0; index < selectElement.List.Count; ++index)
              {
                ArrayList arrayList = selectElement.List[index];
                PdfListFieldItem pdfListFieldItem = new PdfListFieldItem(arrayList[0].ToString(), arrayList[1].ToString());
                field.Items.Add(pdfListFieldItem);
                if (selectElement.SelectedIndex[0] == index)
                  field.SelectedIndex = index;
              }
              currentPage.Document.Form.Fields.Add((PdfField) field);
            }
            else if (selectElement.Type == "select-multiple")
            {
              PdfListBoxField field = new PdfListBoxField((PdfPageBase) currentPage, selectElement.Name);
              RectangleF bounds8 = selectElement.Bounds;
              bounds8.Y -= shapeLayoutBounds.Y;
              field.Bounds = bounds8;
              field.BackRectColor = (PdfColor) selectElement.BackRectColor;
              field.MultiSelect = true;
              for (int index = 0; index < selectElement.List.Count; ++index)
              {
                ArrayList arrayList = selectElement.List[index];
                PdfListFieldItem pdfListFieldItem = new PdfListFieldItem(arrayList[0].ToString(), arrayList[1].ToString());
                field.Items.Add(pdfListFieldItem);
              }
              if (selectElement.SelectedIndex.Length == 1)
                field.SelectedIndex = selectElement.SelectedIndex[0];
              else
                field.SelectedIndexes = selectElement.SelectedIndex;
              currentPage.Document.Form.Fields.Add((PdfField) field);
            }
          }
        }
      }
    }
    if (this.Element.ButtonElementCollection != null)
    {
      foreach (HtmlFormElement buttonElement in this.Element.ButtonElementCollection)
      {
        float height11 = rectangleF.Height;
        double y12 = (double) shapeLayoutBounds.Y;
        RectangleF bounds9 = buttonElement.Bounds;
        double y13 = (double) bounds9.Y;
        if (y12 <= y13)
        {
          double num19 = (double) height11 + (double) shapeLayoutBounds.Y;
          bounds9 = buttonElement.Bounds;
          double y14 = (double) bounds9.Y;
          if (num19 >= y14)
          {
            bounds9 = buttonElement.Bounds;
            double y15 = (double) bounds9.Y;
            bounds9 = buttonElement.Bounds;
            double height12 = (double) bounds9.Height;
            if (y15 + height12 > (double) currentPage.Graphics.ClientSize.Height + (double) shapeLayoutBounds.Y)
            {
              ref RectangleF local = ref rectangleF;
              bounds9 = buttonElement.Bounds;
              double num20 = (double) bounds9.Y - (double) shapeLayoutBounds.Y;
              local.Height = (float) num20;
              break;
            }
            PdfButtonField field = new PdfButtonField((PdfPageBase) currentPage, buttonElement.Name);
            RectangleF bounds10 = buttonElement.Bounds;
            bounds10.Y -= shapeLayoutBounds.Y;
            field.Bounds = bounds10;
            field.BackRectColor = (PdfColor) buttonElement.BackRectColor;
            field.Text = buttonElement.Value == null ? "" : buttonElement.Value;
            currentPage.Document.Form.Fields.Add((PdfField) field);
          }
        }
      }
    }
    this.RepositionLinks(list, rectangleF.Height);
    return rectangleF;
  }

  protected override float ToCorrectBounds(
    RectangleF currentBounds,
    RectangleF shapeLayoutBounds,
    PdfPage currentPage)
  {
    RectangleF rectangleF = currentBounds;
    int num1 = 0;
    bool flag1 = false;
    do
    {
      for (int height = (int) rectangleF.Height; height > 0; --height)
      {
        float num2 = shapeLayoutBounds.Y + (float) height;
        PdfUnitConvertor pdfUnitConvertor = new PdfUnitConvertor(this.Element.VerticalResolution);
        float coordinate1 = this.TextRegions.GetCoordinate(pdfUnitConvertor.ConvertToPixels(num2, PdfGraphicsUnit.Point));
        float num3 = pdfUnitConvertor.ConvertFromPixels(coordinate1, PdfGraphicsUnit.Point);
        bool flag2 = (double) num3 != 0.0;
        float coordinate2 = this.ImageRegions.GetCoordinate(pdfUnitConvertor.ConvertToPixels(num3, PdfGraphicsUnit.Point));
        float num4 = pdfUnitConvertor.ConvertFromPixels(coordinate2, PdfGraphicsUnit.Point);
        bool flag3 = (double) num4 != 0.0;
        flag1 = flag2 || flag3;
        if (flag1)
        {
          float num5 = num4 - shapeLayoutBounds.Y;
          rectangleF.Height = num5 - 1f;
          ++num1;
          break;
        }
      }
    }
    while (flag1 && num1 < 2);
    return rectangleF.Height;
  }
}
