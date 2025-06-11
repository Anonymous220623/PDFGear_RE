// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.PdfLoadedStateField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using Syncfusion.Pdf.Security;
using Syncfusion.Pdf.Xfa;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

public abstract class PdfLoadedStateField : PdfLoadedStyledField
{
  private PdfLoadedStateItemCollection m_items;
  private bool m_bUnchecking;

  public PdfLoadedStateItemCollection Items
  {
    get => this.m_items;
    internal set => this.m_items = value;
  }

  internal PdfLoadedStateField(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    PdfLoadedStateItemCollection items)
    : base(dictionary, crossTable)
  {
    if (crossTable == null)
      throw new ArgumentNullException(nameof (crossTable));
    if (dictionary == null)
      throw new ArgumentNullException(nameof (dictionary));
    if (items == null)
      throw new ArgumentNullException(nameof (items));
    PdfArray kids = this.Kids;
    this.m_items = items;
    if (kids != null)
    {
      for (int index = 0; index < kids.Count; ++index)
      {
        PdfDictionary itemDictionary = crossTable.GetObject(kids[index]) as PdfDictionary;
        this.m_items.Add(this.GetItem(index, itemDictionary));
      }
    }
    else
    {
      bool flag = false;
      if (dictionary.ContainsKey("Parent"))
      {
        PdfDictionary pdfDictionary = (dictionary["Parent"] as PdfReferenceHolder).Object as PdfDictionary;
        if (pdfDictionary.ContainsKey("Kids") && pdfDictionary["Kids"] is PdfArray && pdfDictionary.ContainsKey("FT") && (pdfDictionary["FT"] as PdfName).Value == "Btn")
        {
          kids = this.CrossTable.GetObject(pdfDictionary["Kids"]) as PdfArray;
          for (int index = 0; index < kids.Count; ++index)
          {
            PdfDictionary itemDictionary = crossTable.GetObject(kids[index]) as PdfDictionary;
            this.m_items.Add(this.GetItem(index, itemDictionary));
            flag = true;
          }
        }
      }
      if (kids != null && flag)
        return;
      PdfLoadedStateItem pdfLoadedStateItem = this.GetItem(0, dictionary);
      if (pdfLoadedStateItem is PdfLoadedRadioButtonItem)
      {
        if (!((pdfLoadedStateItem as PdfLoadedRadioButtonItem).Value != ""))
          return;
        this.m_items.Add(pdfLoadedStateItem);
      }
      else
        this.m_items.Add(pdfLoadedStateItem);
    }
  }

  internal abstract PdfLoadedStateItem GetItem(int index, PdfDictionary itemDictionary);

  private PdfTemplate GetStateTemplate(PdfCheckFieldState state, PdfLoadedStateItem item)
  {
    PdfDictionary dictionary = item != null ? item.Dictionary : this.Dictionary;
    string key = state == PdfCheckFieldState.Checked ? PdfLoadedStateField.GetItemValue(dictionary, this.CrossTable) : "Off";
    PdfTemplate stateTemplate = (PdfTemplate) null;
    if (dictionary.ContainsKey("AP"))
    {
      PdfDictionary pdfDictionary = PdfCrossTable.Dereference((PdfCrossTable.Dereference(dictionary["AP"]) as PdfDictionary)["N"]) as PdfDictionary;
      if (!string.IsNullOrEmpty(key))
      {
        if (PdfCrossTable.Dereference(pdfDictionary[key]) is PdfStream template && template.InternalStream != null)
          stateTemplate = new PdfTemplate(template);
        if (item == null && stateTemplate != null && key == "Off" && template.Encrypt && template.Decrypted && !template.IsDecrypted)
          stateTemplate = (PdfTemplate) null;
      }
    }
    return stateTemplate;
  }

  protected void SetCheckedStatus(bool value)
  {
    if (value)
    {
      string itemValue = PdfLoadedStateField.GetItemValue(this.Dictionary, this.CrossTable);
      this.Dictionary.SetName("V", itemValue);
      this.Dictionary.SetProperty("AS", (IPdfPrimitive) new PdfName(itemValue));
    }
    else
    {
      this.Dictionary.Remove("V");
      this.Dictionary.SetProperty("AS", (IPdfPrimitive) new PdfName("Off"));
    }
    this.Changed = true;
  }

  internal static string GetItemValue(PdfDictionary dictionary, PdfCrossTable crossTable)
  {
    string empty = string.Empty;
    if (dictionary.ContainsKey("AS"))
    {
      PdfName pdfName = crossTable.GetObject(dictionary["AS"]) as PdfName;
      if (pdfName != (PdfName) null && pdfName.Value != "Off")
        empty = pdfName.Value;
    }
    if (empty == string.Empty && dictionary.ContainsKey("AP"))
    {
      PdfDictionary pdfDictionary1 = crossTable.GetObject(dictionary["AP"]) as PdfDictionary;
      if (pdfDictionary1.ContainsKey("N"))
      {
        PdfReference reference = crossTable.GetReference(pdfDictionary1["N"]);
        PdfDictionary pdfDictionary2 = crossTable.GetObject((IPdfPrimitive) reference) as PdfDictionary;
        List<object> objectList = new List<object>();
        foreach (PdfName key in pdfDictionary2.Keys)
          objectList.Add((object) key);
        int index = 0;
        for (int count = objectList.Count; index < count; ++index)
        {
          PdfName pdfName = objectList[index] as PdfName;
          if (pdfName.Value != "Off")
          {
            empty = pdfName.Value;
            break;
          }
        }
      }
    }
    return empty;
  }

  internal void UncheckOthers(PdfLoadedStateItem child, string value, bool check)
  {
    if (this.m_bUnchecking)
      return;
    this.m_bUnchecking = true;
    int num = -1;
    int index1 = 0;
    for (int count = this.Items.Count; index1 < count; ++index1)
    {
      PdfLoadedStateItem pdfLoadedStateItem = this.Items[index1];
      if (pdfLoadedStateItem != child)
      {
        if (PdfLoadedStateField.GetItemValue(pdfLoadedStateItem.Dictionary, this.CrossTable) == value && check)
        {
          num = index1;
          pdfLoadedStateItem.Checked = true;
        }
        else
          pdfLoadedStateItem.Checked = false;
      }
      else
      {
        if (!pdfLoadedStateItem.Checked && value != null)
          pdfLoadedStateItem.Checked = true;
        num = index1;
      }
    }
    if (this.Form != null && this.Form.m_enableXfaFormfill && (this.Form as PdfLoadedForm).IsXFAForm)
    {
      PdfLoadedXfaField xfaField = (this.Form as PdfLoadedForm).GetXfaField(this.Name.Replace("\\", string.Empty));
      if (xfaField != null && xfaField is PdfLoadedXfaCheckBoxField)
        (xfaField as PdfLoadedXfaCheckBoxField).IsChecked = true;
      else if (xfaField != null && xfaField is PdfLoadedXfaRadioButtonGroup)
      {
        PdfLoadedXfaRadioButtonGroup radioButtonGroup = xfaField as PdfLoadedXfaRadioButtonGroup;
        for (int index2 = 0; index2 < radioButtonGroup.Fields.Length; ++index2)
          radioButtonGroup.Fields[index2].IsChecked = index2 == num;
      }
    }
    this.m_bUnchecking = false;
  }

  internal void ApplyAppearance(PdfDictionary widget, PdfLoadedStateItem item)
  {
    if (widget != null && widget.ContainsKey("AP"))
    {
      if (this.CrossTable.GetObject(widget["AP"]) is PdfDictionary primitive && primitive.ContainsKey("N"))
      {
        string empty = string.Empty;
        string key = item == null ? PdfLoadedStateField.GetItemValue(this.Dictionary, this.CrossTable) : PdfLoadedStateField.GetItemValue(item.Dictionary, item.CrossTable);
        if ((!this.Flatten || !((PdfField) this).Form.Flatten) && this.isRotationModified)
        {
          this.Changed = true;
          this.FieldChanged = true;
        }
        RectangleF rectangleF = item == null ? this.Bounds : item.Bounds;
        if (!(PdfCrossTable.Dereference(primitive["N"]) is PdfDictionary))
        {
          PdfDictionary pdfDictionary = new PdfDictionary();
          PdfTemplate wrapper1 = new PdfTemplate(rectangleF.Size);
          PdfTemplate wrapper2 = new PdfTemplate(rectangleF.Size);
          this.DrawStateItem(wrapper1.Graphics, PdfCheckFieldState.Checked, item);
          this.DrawStateItem(wrapper2.Graphics, PdfCheckFieldState.Unchecked, item);
          pdfDictionary.SetProperty("Off", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) wrapper2));
          pdfDictionary.SetProperty(key, (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) wrapper1));
          primitive["N"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary);
        }
        else if (this.FieldChanged)
        {
          PdfDictionary pdfDictionary = new PdfDictionary();
          PdfTemplate wrapper3 = new PdfTemplate(rectangleF.Size);
          PdfTemplate wrapper4 = new PdfTemplate(rectangleF.Size);
          this.DrawStateItem(wrapper3.Graphics, PdfCheckFieldState.Checked, item);
          this.DrawStateItem(wrapper4.Graphics, PdfCheckFieldState.Unchecked, item);
          pdfDictionary.SetProperty(key, (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) wrapper3));
          pdfDictionary.SetProperty("Off", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) wrapper4));
          primitive.Remove("N");
          primitive["N"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary);
        }
        if (!(PdfCrossTable.Dereference(primitive["D"]) is PdfDictionary pdfDictionary1))
        {
          PdfTemplate wrapper5 = new PdfTemplate(rectangleF.Size);
          PdfTemplate wrapper6 = new PdfTemplate(rectangleF.Size);
          this.DrawStateItem(wrapper5.Graphics, PdfCheckFieldState.PressedChecked, item);
          this.DrawStateItem(wrapper6.Graphics, PdfCheckFieldState.PressedUnchecked, item);
          if (pdfDictionary1 != null)
          {
            pdfDictionary1.SetProperty("Off", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) wrapper6));
            pdfDictionary1.SetProperty(key, (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) wrapper5));
            primitive["D"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary1);
          }
        }
        else if (this.FieldChanged)
        {
          PdfDictionary pdfDictionary = new PdfDictionary();
          PdfTemplate wrapper7 = new PdfTemplate(rectangleF.Size);
          PdfTemplate wrapper8 = new PdfTemplate(rectangleF.Size);
          this.DrawStateItem(wrapper7.Graphics, PdfCheckFieldState.PressedChecked, item);
          this.DrawStateItem(wrapper8.Graphics, PdfCheckFieldState.PressedUnchecked, item);
          if (pdfDictionary != null)
          {
            pdfDictionary.SetProperty("Off", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) wrapper8));
            pdfDictionary.SetProperty(key, (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) wrapper7));
            primitive.Remove("D");
            primitive["D"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary);
          }
        }
      }
      widget.SetProperty("AP", (IPdfPrimitive) primitive);
    }
    else
    {
      if (!((PdfField) this).Form.SetAppearanceDictionary)
        return;
      ((PdfField) this).Form.NeedAppearances = true;
    }
  }

  internal void DrawStateItem(
    PdfGraphics graphics,
    PdfCheckFieldState state,
    PdfLoadedStateItem item)
  {
    PdfLoadedStyledField.GraphicsProperties graphicsProperties;
    this.GetGraphicsProperties(out graphicsProperties, (PdfLoadedFieldItem) item);
    if (graphics != null && graphics.Page != null)
    {
      graphics.Save();
      if (graphics.Page.Rotation == PdfPageRotateAngle.RotateAngle90)
      {
        graphics.TranslateTransform(graphics.Size.Width, graphics.Size.Height);
        graphics.RotateTransform(90f);
      }
      else if (graphics.Page.Rotation == PdfPageRotateAngle.RotateAngle180)
      {
        graphics.TranslateTransform(graphics.Size.Width, graphics.Size.Height);
        graphics.RotateTransform(-180f);
      }
      else if (graphics.Page.Rotation == PdfPageRotateAngle.RotateAngle270)
      {
        graphics.TranslateTransform(graphics.Size.Width, graphics.Size.Height);
        graphics.RotateTransform(270f);
      }
    }
    if (!this.Flatten)
      graphicsProperties.Rect.Location = PointF.Empty;
    PaintParams paintParams = new PaintParams(graphicsProperties.Rect, graphicsProperties.BackBrush, graphicsProperties.ForeBrush, graphicsProperties.Pen, graphicsProperties.Style, graphicsProperties.BorderWidth, graphicsProperties.ShadowBrush, graphicsProperties.RotationAngle);
    if (this.FieldChanged || this.Flatten && !this.Form.NeedAppearances && graphicsProperties.RotationAngle == 0 && !this.Form.Dictionary.ContainsKey("NeedAppearances"))
    {
      if ((double) graphicsProperties.Font.Size >= 0.0)
        graphicsProperties.Font = (PdfFont) null;
      switch (this)
      {
        case PdfLoadedCheckBoxField _:
          FieldPainter.DrawCheckBox(graphics, paintParams, this.StyleToString(this.Style), state, graphicsProperties.Font);
          break;
        case PdfLoadedRadioButtonListField _:
          FieldPainter.DrawRadioButton(graphics, paintParams, this.StyleToString(this.Style), state);
          break;
      }
    }
    else
    {
      graphics.StreamWriter.SetTextRenderingMode(TextRenderingMode.Fill);
      PdfTemplate stateTemplate = this.GetStateTemplate(state, item);
      if (stateTemplate != null)
      {
        RectangleF rectangleF = item == null ? this.Bounds : item.Bounds;
        bool flag = false;
        if (this.CrossTable != null && this.CrossTable.Document != null && this.CrossTable.Document is PdfLoadedDocument document && document.Security != null && document.IsEncrypted && document.Security.Enabled && document.Security.EncryptionOptions == PdfEncryptionOptions.EncryptAllContents)
          flag = true;
        PdfStream content = stateTemplate.m_content;
        if (content != null && flag && content.Encrypt && !content.IsDecrypted && this is PdfLoadedCheckBoxField)
        {
          graphicsProperties.Font = (PdfFont) null;
          FieldPainter.DrawCheckBox(graphics, paintParams, this.StyleToString(this.Style), state, graphicsProperties.Font);
        }
        else
          graphics.DrawPdfTemplate(stateTemplate, rectangleF.Location, rectangleF.Size);
      }
    }
    graphics.Restore();
  }

  public void RemoveAt(int index)
  {
    if (this.Items == null || this.Items.Count == 0)
      return;
    this.Remove(this.Items[index]);
  }

  public void Remove(PdfLoadedStateItem item)
  {
    int index = item != null ? this.Items.Remove(item) : throw new NullReferenceException(nameof (item));
    if (!(this.Dictionary["Kids"] is PdfArray pdfArray))
      return;
    pdfArray.RemoveAt(index);
    pdfArray.MarkChanged();
  }
}
