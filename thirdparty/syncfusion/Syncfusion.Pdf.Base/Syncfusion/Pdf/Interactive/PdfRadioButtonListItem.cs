// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfRadioButtonListItem
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfRadioButtonListItem : PdfCheckFieldBase, IPdfWrapper
{
  private PdfRadioButtonListField m_field;
  private string m_value = string.Empty;

  public PdfRadioButtonListItem()
  {
  }

  public PdfRadioButtonListItem(string value) => this.Value = value;

  public override PdfForm Form => this.m_field != null ? this.m_field.Form : (PdfForm) null;

  public override RectangleF Bounds
  {
    get => base.Bounds;
    set => base.Bounds = value;
  }

  public string Value
  {
    get => this.m_value;
    set
    {
      switch (value)
      {
        case null:
          throw new ArgumentNullException(nameof (Value));
        case "":
          throw new ArgumentException("Value can't be an empty string.");
        default:
          this.m_value = value;
          break;
      }
    }
  }

  protected override void Initialize()
  {
    base.Initialize();
    this.Widget.BeginSave += new EventHandler(this.Widget_Save);
    this.Style = PdfCheckBoxStyle.Circle;
  }

  internal void SetField(PdfRadioButtonListField field)
  {
    this.Widget.Parent = (PdfField) field;
    PdfPage pdfPage = field != null ? field.Page as PdfPage : this.m_field.Page as PdfPage;
    if (pdfPage != null)
    {
      if (field == null)
      {
        int index = pdfPage.Annotations.IndexOf((PdfAnnotation) this.Widget);
        pdfPage.Annotations.RemoveAt(index);
      }
      else
        pdfPage.Annotations.Add((PdfAnnotation) this.Widget);
    }
    else
    {
      PdfLoadedPage page = field.Page as PdfLoadedPage;
      PdfDictionary dictionary = page.Dictionary;
      PdfArray primitive = !dictionary.ContainsKey("Annots") ? new PdfArray() : page.CrossTable.GetObject(dictionary["Annots"]) as PdfArray;
      PdfReferenceHolder element = new PdfReferenceHolder((IPdfWrapper) this.Widget);
      if (field == null)
      {
        int index = primitive.IndexOf((IPdfPrimitive) element);
        if (index >= 0)
          primitive.RemoveAt(index);
      }
      else
      {
        primitive.Add((IPdfPrimitive) element);
        if (!field.Page.Annotations.Contains((PdfAnnotation) this.Widget))
          field.Page.Annotations.Add((PdfAnnotation) this.Widget);
      }
      field.Page.Dictionary.SetProperty("Annots", (IPdfPrimitive) primitive);
    }
    this.m_field = field;
  }

  private void Widget_Save(object sender, EventArgs e) => this.Save();

  internal override void Save()
  {
    base.Save();
    if (this.Form == null && !this.isXfa)
      return;
    string str = this.ObtainValue();
    this.Widget.ExtendedAppearance.Normal.OnMappingName = str;
    this.Widget.ExtendedAppearance.Pressed.OnMappingName = str;
    if (this.m_field.SelectedItem == this)
      this.Widget.AppearanceState = this.ObtainValue();
    else
      this.Widget.AppearanceState = "Off";
  }

  protected override void DrawAppearance()
  {
    base.DrawAppearance();
    PaintParams paintParams = new PaintParams(new RectangleF(PointF.Empty, this.Size), this.BackBrush, this.ForeBrush, this.BorderPen, this.BorderStyle, this.BorderWidth, this.ShadowBrush, this.RotationAngle);
    FieldPainter.DrawRadioButton(this.Widget.ExtendedAppearance.Normal.On.Graphics, paintParams, this.StyleToString(this.Style), PdfCheckFieldState.Checked);
    FieldPainter.DrawRadioButton(this.Widget.ExtendedAppearance.Normal.Off.Graphics, paintParams, this.StyleToString(this.Style), PdfCheckFieldState.Unchecked);
    FieldPainter.DrawRadioButton(this.Widget.ExtendedAppearance.Pressed.On.Graphics, paintParams, this.StyleToString(this.Style), PdfCheckFieldState.PressedChecked);
    FieldPainter.DrawRadioButton(this.Widget.ExtendedAppearance.Pressed.Off.Graphics, paintParams, this.StyleToString(this.Style), PdfCheckFieldState.PressedUnchecked);
  }

  private string ObtainValue()
  {
    return this.m_value == string.Empty ? this.m_field.Items.IndexOf(this).ToString() : this.m_value;
  }

  internal override void Draw()
  {
    this.RemoveAnnoationFromPage(this.m_field.Page, (PdfAnnotation) this.Widget);
    PaintParams paintParams = new PaintParams(this.Bounds, this.BackBrush, this.ForeBrush, this.BorderPen, this.BorderStyle, this.BorderWidth, this.ShadowBrush, this.RotationAngle);
    PdfCheckFieldState state = PdfCheckFieldState.Unchecked;
    if (this.m_field.SelectedIndex >= 0 && this.m_field.SelectedValue == this.Value)
      state = PdfCheckFieldState.Checked;
    FieldPainter.DrawRadioButton(this.m_field.Page.Graphics, paintParams, this.StyleToString(this.Style), state);
  }

  IPdfPrimitive IPdfWrapper.Element => ((IPdfWrapper) this.Widget).Element;
}
