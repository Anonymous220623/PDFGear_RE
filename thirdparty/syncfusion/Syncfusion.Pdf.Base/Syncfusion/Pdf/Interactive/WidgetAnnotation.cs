// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.WidgetAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

internal class WidgetAnnotation : PdfAnnotation
{
  private PdfField m_parent;
  private PdfExtendedAppearance m_extendedAppearance;
  private WidgetBorder m_border = new WidgetBorder();
  private WidgetAppearance m_widgetAppearance = new WidgetAppearance();
  private PdfHighlightMode m_highlightMode = PdfHighlightMode.Invert;
  private PdfDefaultAppearance m_defaultAppearance;
  private PdfTextAlignment m_alignment;
  private PdfAnnotationActions m_actions;
  private PdfAppearance m_appearance;
  private string m_appearanceState;
  internal bool isAutoResize;
  internal PdfSignatureField m_signatureField;

  public PdfField Parent
  {
    get => this.m_parent;
    set
    {
      if (this.m_parent == value)
        return;
      this.m_parent = value;
      if (this.m_parent != null)
        this.Dictionary.SetProperty(nameof (Parent), (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_parent));
      else
        this.Dictionary.Remove(nameof (Parent));
    }
  }

  public PdfExtendedAppearance ExtendedAppearance
  {
    get
    {
      if (this.m_extendedAppearance == null)
        this.m_extendedAppearance = new PdfExtendedAppearance();
      return this.m_extendedAppearance;
    }
    set => this.m_extendedAppearance = value;
  }

  public PdfDefaultAppearance DefaultAppearance
  {
    get
    {
      if (this.m_defaultAppearance == null)
        this.m_defaultAppearance = new PdfDefaultAppearance();
      return this.m_defaultAppearance;
    }
  }

  public WidgetBorder WidgetBorder => this.m_border;

  public WidgetAppearance WidgetAppearance => this.m_widgetAppearance;

  public PdfHighlightMode HighlightMode
  {
    get => this.m_highlightMode;
    set
    {
      this.m_highlightMode = value;
      this.Dictionary.SetName("H", this.HighlightModeToString(this.m_highlightMode));
    }
  }

  public PdfTextAlignment TextAlignment
  {
    get => this.m_alignment;
    set
    {
      if (this.m_alignment == value)
        return;
      this.m_alignment = value;
      this.Dictionary.SetProperty("Q", (IPdfPrimitive) new PdfNumber((int) this.m_alignment));
    }
  }

  public PdfAnnotationActions Actions
  {
    get
    {
      if (this.m_actions == null)
      {
        this.m_actions = new PdfAnnotationActions();
        this.Dictionary.SetProperty("AA", (IPdfWrapper) this.m_actions);
      }
      return this.m_actions;
    }
  }

  public new PdfAppearance Appearance
  {
    get
    {
      if (this.m_appearance == null)
        this.m_appearance = new PdfAppearance((PdfAnnotation) this);
      return this.m_appearance;
    }
    set
    {
      if (this.m_appearance == value)
        return;
      this.m_appearance = value;
    }
  }

  internal string AppearanceState
  {
    get => this.m_appearanceState;
    set
    {
      if (!(this.m_appearanceState != value))
        return;
      this.m_appearanceState = value;
      this.Dictionary.SetName("AS", value);
    }
  }

  internal event EventHandler BeginSave;

  protected override void Initialize()
  {
    base.Initialize();
    this.AnnotationFlags |= PdfAnnotationFlags.Print;
    this.Dictionary.SetProperty("Subtype", (IPdfPrimitive) new PdfName("Widget"));
    this.Dictionary.SetProperty("BS", (IPdfWrapper) this.m_border);
  }

  protected virtual void OnBeginSave(EventArgs args)
  {
    if (this.BeginSave == null)
      return;
    this.BeginSave((object) this, args);
  }

  protected override void Save()
  {
    base.Save();
    if (this.Parent is PdfCheckBoxField && this.Dictionary.isXfa)
      this.Parent.Save();
    if (this.m_signatureField != null && this.m_signatureField.Signature == null)
    {
      this.m_signatureField.Save();
      if (this.m_appearance != null)
      {
        if (!this.m_signatureField.m_containsBG)
          this.m_widgetAppearance.BackColor = PdfColor.Empty;
        if ((double) this.m_signatureField.BorderWidth <= 0.0 || !this.m_signatureField.m_containsBW)
          this.m_widgetAppearance.BorderColor = PdfColor.Empty;
        this.Dictionary.SetProperty("MK", (IPdfWrapper) this.m_widgetAppearance);
      }
    }
    this.OnBeginSave(new EventArgs());
    if (this.m_extendedAppearance != null)
    {
      this.Dictionary.SetProperty("AP", (IPdfWrapper) this.m_extendedAppearance);
      this.Dictionary.SetProperty("MK", (IPdfWrapper) this.m_widgetAppearance);
    }
    else
    {
      if (this.m_appearance != null && this.m_appearance.GetNormalTemplate() != null)
        this.Dictionary.SetProperty("AP", (IPdfWrapper) this.m_appearance);
      else
        this.Dictionary.SetProperty("AP", (IPdfPrimitive) null);
      bool flag = false;
      if (this.Dictionary.ContainsKey("FT"))
      {
        PdfName pdfName = PdfCrossTable.Dereference(this.Dictionary["FT"]) as PdfName;
        if (pdfName != (PdfName) null && pdfName.Value == "Sig")
          flag = true;
      }
      if (!flag)
        this.Dictionary.SetProperty("MK", (IPdfWrapper) this.m_widgetAppearance);
      this.Dictionary.SetProperty("AS", (IPdfPrimitive) null);
    }
    if (this.m_defaultAppearance == null || this.isAutoResize)
      return;
    this.Dictionary.SetProperty("DA", (IPdfPrimitive) new PdfString(this.m_defaultAppearance.ToString()));
  }

  private string HighlightModeToString(PdfHighlightMode m_highlightingMode)
  {
    switch (m_highlightingMode)
    {
      case PdfHighlightMode.NoHighlighting:
        return "N";
      case PdfHighlightMode.Outline:
        return "O";
      case PdfHighlightMode.Push:
        return "P";
      default:
        return "I";
    }
  }

  internal PdfAppearance ObtainAppearance() => this.m_appearance;
}
