// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfPageTemplateElement
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfPageTemplateElement
{
  private bool m_foreground;
  private PdfDockStyle m_dockStyle;
  private PdfAlignmentStyle m_alignmentStyle;
  private PdfTemplate m_template;
  private TemplateType m_type;
  private PointF m_location;
  private PdfTag m_tag;

  public PdfDockStyle Dock
  {
    get => this.m_dockStyle;
    set
    {
      if (this.m_dockStyle == value || this.Type != TemplateType.None)
        return;
      this.m_dockStyle = value;
      this.ResetAlignment();
    }
  }

  public PdfAlignmentStyle Alignment
  {
    get => this.m_alignmentStyle;
    set
    {
      if (this.m_alignmentStyle == value)
        return;
      this.AssignAlignment(value);
    }
  }

  public bool Foreground
  {
    get => this.m_foreground;
    set
    {
      if (this.m_foreground == value)
        return;
      this.m_foreground = value;
    }
  }

  public bool Background
  {
    get => !this.m_foreground;
    set => this.m_foreground = !value;
  }

  public PointF Location
  {
    get => this.m_location;
    set
    {
      if (this.Type != TemplateType.None)
        return;
      this.m_location = value;
    }
  }

  public float X
  {
    get => this.m_location.X;
    set
    {
      if (this.Type != TemplateType.None)
        return;
      this.m_location.X = value;
    }
  }

  public float Y
  {
    get => this.m_location.Y;
    set
    {
      if (this.Type != TemplateType.None)
        return;
      this.m_location.Y = value;
    }
  }

  public SizeF Size
  {
    get => this.m_template.Size;
    set
    {
      if (!(this.m_template.Size != value) || this.Type != TemplateType.None)
        return;
      this.m_template.Reset(value);
    }
  }

  public float Width
  {
    get => this.m_template.Width;
    set
    {
      if ((double) this.m_template.Width == (double) value || this.Type != TemplateType.None)
        return;
      this.m_template.Reset(this.m_template.Size with
      {
        Width = value
      });
    }
  }

  public float Height
  {
    get => this.m_template.Height;
    set
    {
      if ((double) this.m_template.Height == (double) value || this.Type != TemplateType.None)
        return;
      this.m_template.Reset(this.m_template.Size with
      {
        Height = value
      });
    }
  }

  public RectangleF Bounds
  {
    get => new RectangleF(this.Location, this.Size);
    set
    {
      if (this.Type != TemplateType.None)
        return;
      this.Location = value.Location;
      this.Size = value.Size;
    }
  }

  public PdfGraphics Graphics => this.Template.Graphics;

  internal PdfTemplate Template
  {
    get
    {
      if (this.m_template == null)
        this.m_template = new PdfTemplate(this.Size);
      return this.m_template;
    }
  }

  internal TemplateType Type
  {
    get => this.m_type;
    set
    {
      if (this.m_type == value)
        return;
      this.UpdateDocking(value);
      this.m_type = value;
    }
  }

  public PdfTag PdfTag
  {
    get => this.m_tag;
    set
    {
      this.m_tag = value;
      this.m_template.Graphics.Tag = this.m_tag;
    }
  }

  public PdfPageTemplateElement(RectangleF bounds)
    : this(bounds.X, bounds.Y, bounds.Width, bounds.Height)
  {
  }

  public PdfPageTemplateElement(RectangleF bounds, PdfPage page)
    : this(bounds.X, bounds.Y, bounds.Width, bounds.Height, page)
  {
  }

  public PdfPageTemplateElement(PointF location, SizeF size)
    : this(location.X, location.Y, size.Width, size.Height)
  {
  }

  public PdfPageTemplateElement(PointF location, SizeF size, PdfPage page)
    : this(location.X, location.Y, size.Width, size.Height, page)
  {
  }

  public PdfPageTemplateElement(SizeF size)
    : this(size.Width, size.Height)
  {
  }

  public PdfPageTemplateElement(float width, float height)
    : this(0.0f, 0.0f, width, height)
  {
  }

  public PdfPageTemplateElement(float width, float height, PdfPage page)
    : this(0.0f, 0.0f, width, height, page)
  {
  }

  public PdfPageTemplateElement(float x, float y, float width, float height)
  {
    this.X = x;
    this.Y = this.Y;
    this.m_template = new PdfTemplate(width, height);
  }

  public PdfPageTemplateElement(float x, float y, float width, float height, PdfPage page)
  {
    this.X = x;
    this.Y = this.Y;
    this.m_template = new PdfTemplate(width, height);
    this.Graphics.ColorSpace = page.Document.ColorSpace;
  }

  internal void Draw(PdfPageLayer layer, PdfDocument document)
  {
    if (layer == null)
      throw new ArgumentNullException(nameof (layer));
    if (document == null)
      throw new ArgumentNullException(nameof (document));
    RectangleF bounds = this.CalculateBounds(layer.Page as PdfPage, document);
    if (this.PdfTag != null && this.Template.Graphics.Tag == null)
      this.Template.Graphics.Tag = this.PdfTag;
    else if (document.AutoTag && this.PdfTag == null && this.Template.Graphics.Tag == null)
      this.Template.Graphics.Tag = (PdfTag) new PdfStructureElement(PdfTagType.Figure);
    if (document.Template.blinkMargin == null)
    {
      layer.Graphics.DrawPdfTemplate(this.Template, bounds.Location, bounds.Size);
    }
    else
    {
      PdfMargins blinkMargin = document.Template.blinkMargin;
      float x = bounds.X + blinkMargin.Left;
      float y1 = bounds.Y;
      float width = bounds.Size.Width - blinkMargin.Left - blinkMargin.Right;
      float height = bounds.Size.Height;
      float y2 = this.Dock != PdfDockStyle.Top ? y1 - blinkMargin.Bottom : y1 + blinkMargin.Top;
      layer.Graphics.DrawPdfTemplate(this.Template, new PointF(x, y2), new SizeF(width, height));
    }
  }

  private void UpdateDocking(TemplateType type)
  {
    switch (type)
    {
      case TemplateType.None:
        return;
      case TemplateType.Top:
        this.Dock = PdfDockStyle.Top;
        break;
      case TemplateType.Bottom:
        this.Dock = PdfDockStyle.Bottom;
        break;
      case TemplateType.Left:
        this.Dock = PdfDockStyle.Left;
        break;
      case TemplateType.Right:
        this.Dock = PdfDockStyle.Right;
        break;
    }
    this.ResetAlignment();
  }

  private void ResetAlignment() => this.Alignment = PdfAlignmentStyle.None;

  private void AssignAlignment(PdfAlignmentStyle alignment)
  {
    if (this.Dock == PdfDockStyle.None)
    {
      this.m_alignmentStyle = alignment;
    }
    else
    {
      bool flag = false;
      switch (this.Dock)
      {
        case PdfDockStyle.Bottom:
          flag = alignment == PdfAlignmentStyle.BottomLeft || alignment == PdfAlignmentStyle.BottomCenter || alignment == PdfAlignmentStyle.BottomRight || alignment == PdfAlignmentStyle.None;
          break;
        case PdfDockStyle.Top:
          flag = alignment == PdfAlignmentStyle.TopLeft || alignment == PdfAlignmentStyle.TopCenter || alignment == PdfAlignmentStyle.TopRight || alignment == PdfAlignmentStyle.None;
          break;
        case PdfDockStyle.Left:
          flag = alignment == PdfAlignmentStyle.TopLeft || alignment == PdfAlignmentStyle.MiddleLeft || alignment == PdfAlignmentStyle.BottomLeft || alignment == PdfAlignmentStyle.None;
          break;
        case PdfDockStyle.Right:
          flag = alignment == PdfAlignmentStyle.TopRight || alignment == PdfAlignmentStyle.MiddleRight || alignment == PdfAlignmentStyle.BottomRight || alignment == PdfAlignmentStyle.None;
          break;
        case PdfDockStyle.Fill:
          flag = alignment == PdfAlignmentStyle.MiddleCenter || alignment == PdfAlignmentStyle.None;
          break;
      }
      if (!flag)
        return;
      this.m_alignmentStyle = alignment;
    }
  }

  private RectangleF CalculateBounds(PdfPage page, PdfDocument document)
  {
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    if (document == null)
      throw new ArgumentNullException(nameof (document));
    RectangleF bounds = this.Bounds;
    if (this.m_alignmentStyle != PdfAlignmentStyle.None)
      bounds = this.GetAlignmentBounds(page, document);
    else if (this.m_dockStyle != PdfDockStyle.None)
      bounds = this.GetDockBounds(page, document);
    return bounds;
  }

  private RectangleF GetAlignmentBounds(PdfPage page, PdfDocument document)
  {
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    if (document == null)
      throw new ArgumentNullException(nameof (document));
    RectangleF bounds = this.Bounds;
    return this.Type != TemplateType.None ? this.GetTemplateAlignmentBounds(page, document) : this.GetSimpleAlignmentBounds(page, document);
  }

  private RectangleF GetSimpleAlignmentBounds(PdfPage page, PdfDocument document)
  {
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    if (document == null)
      throw new ArgumentNullException(nameof (document));
    RectangleF bounds = this.Bounds;
    RectangleF actualBounds = page.Section.GetActualBounds(document, page, false);
    float num1 = this.X;
    float num2 = this.Y;
    switch (this.m_alignmentStyle)
    {
      case PdfAlignmentStyle.TopLeft:
        num1 = 0.0f;
        num2 = 0.0f;
        break;
      case PdfAlignmentStyle.TopCenter:
        num1 = (float) (((double) actualBounds.Width - (double) this.Width) / 2.0);
        num2 = 0.0f;
        break;
      case PdfAlignmentStyle.TopRight:
        num1 = actualBounds.Width - this.Width;
        num2 = 0.0f;
        break;
      case PdfAlignmentStyle.MiddleLeft:
        num1 = 0.0f;
        num2 = (float) (((double) actualBounds.Height - (double) this.Height) / 2.0);
        break;
      case PdfAlignmentStyle.MiddleCenter:
        num1 = (float) (((double) actualBounds.Width - (double) this.Width) / 2.0);
        num2 = (float) (((double) actualBounds.Height - (double) this.Height) / 2.0);
        break;
      case PdfAlignmentStyle.MiddleRight:
        num1 = actualBounds.Width - this.Width;
        num2 = (float) (((double) actualBounds.Height - (double) this.Height) / 2.0);
        break;
      case PdfAlignmentStyle.BottomLeft:
        num1 = 0.0f;
        num2 = actualBounds.Height - this.Height;
        break;
      case PdfAlignmentStyle.BottomCenter:
        num1 = (float) (((double) actualBounds.Width - (double) this.Width) / 2.0);
        num2 = actualBounds.Height - this.Height;
        break;
      case PdfAlignmentStyle.BottomRight:
        num1 = actualBounds.Width - this.Width;
        num2 = actualBounds.Height - this.Height;
        break;
    }
    bounds.X = num1;
    bounds.Y = num2;
    return bounds;
  }

  private RectangleF GetTemplateAlignmentBounds(PdfPage page, PdfDocument document)
  {
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    if (document == null)
      throw new ArgumentNullException(nameof (document));
    RectangleF bounds = this.Bounds;
    PdfSection section = page.Section;
    RectangleF actualBounds = section.GetActualBounds(document, page, false);
    float num1 = this.X;
    float num2 = this.Y;
    switch (this.m_alignmentStyle)
    {
      case PdfAlignmentStyle.TopLeft:
        if (this.Type == TemplateType.Left)
        {
          num1 = -actualBounds.X;
          num2 = 0.0f;
          break;
        }
        if (this.Type == TemplateType.Top)
        {
          num1 = -actualBounds.X;
          num2 = -actualBounds.Y;
          break;
        }
        break;
      case PdfAlignmentStyle.TopCenter:
        num1 = (float) (((double) actualBounds.Width - (double) this.Width) / 2.0);
        num2 = -actualBounds.Y;
        break;
      case PdfAlignmentStyle.TopRight:
        if (this.Type == TemplateType.Right)
        {
          num1 = actualBounds.Width + section.GetRightIndentWidth(document, page, false) - this.Width;
          num2 = 0.0f;
          break;
        }
        if (this.Type == TemplateType.Top)
        {
          num1 = actualBounds.Width + section.GetRightIndentWidth(document, page, false) - this.Width;
          num2 = -actualBounds.Y;
          break;
        }
        break;
      case PdfAlignmentStyle.MiddleLeft:
        num1 = -actualBounds.X;
        num2 = (float) (((double) actualBounds.Height - (double) this.Height) / 2.0);
        break;
      case PdfAlignmentStyle.MiddleCenter:
        num1 = (float) (((double) actualBounds.Width - (double) this.Width) / 2.0);
        num2 = (float) (((double) actualBounds.Height - (double) this.Height) / 2.0);
        break;
      case PdfAlignmentStyle.MiddleRight:
        num1 = actualBounds.Width + section.GetRightIndentWidth(document, page, false) - this.Width;
        num2 = (float) (((double) actualBounds.Height - (double) this.Height) / 2.0);
        break;
      case PdfAlignmentStyle.BottomLeft:
        if (this.Type == TemplateType.Left)
        {
          num1 = -actualBounds.X;
          num2 = actualBounds.Height - this.Height;
          break;
        }
        if (this.Type == TemplateType.Bottom)
        {
          num1 = -actualBounds.X;
          num2 = actualBounds.Height + section.GetBottomIndentHeight(document, page, false) - this.Height;
          break;
        }
        break;
      case PdfAlignmentStyle.BottomCenter:
        num1 = (float) (((double) actualBounds.Width - (double) this.Width) / 2.0);
        num2 = actualBounds.Height + section.GetBottomIndentHeight(document, page, false) - this.Height;
        break;
      case PdfAlignmentStyle.BottomRight:
        if (this.Type == TemplateType.Right)
        {
          num1 = actualBounds.Width + section.GetRightIndentWidth(document, page, false) - this.Width;
          num2 = actualBounds.Height - this.Height;
          break;
        }
        if (this.Type == TemplateType.Bottom)
        {
          num1 = actualBounds.Width + section.GetRightIndentWidth(document, page, false) - this.Width;
          num2 = actualBounds.Height + section.GetBottomIndentHeight(document, page, false) - this.Height;
          break;
        }
        break;
    }
    bounds.X = num1;
    bounds.Y = num2;
    return bounds;
  }

  private RectangleF GetDockBounds(PdfPage page, PdfDocument document)
  {
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    if (document == null)
      throw new ArgumentNullException(nameof (document));
    RectangleF bounds = this.Bounds;
    return this.Type != TemplateType.None ? this.GetTemplateDockBounds(page, document) : this.GetSimpleDockBounds(page, document);
  }

  private RectangleF GetSimpleDockBounds(PdfPage page, PdfDocument document)
  {
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    if (document == null)
      throw new ArgumentNullException(nameof (document));
    RectangleF simpleDockBounds = this.Bounds;
    RectangleF actualBounds = page.Section.GetActualBounds(document, page, false);
    float x = this.X;
    float y = this.Y;
    float width = this.Width;
    float height = this.Height;
    switch (this.m_dockStyle)
    {
      case PdfDockStyle.Bottom:
        x = 0.0f;
        y = actualBounds.Height - this.Height;
        width = actualBounds.Width;
        height = this.Height;
        break;
      case PdfDockStyle.Top:
        x = 0.0f;
        y = 0.0f;
        width = actualBounds.Width;
        height = this.Height;
        break;
      case PdfDockStyle.Left:
        x = 0.0f;
        y = 0.0f;
        width = this.Width;
        height = actualBounds.Height;
        break;
      case PdfDockStyle.Right:
        x = actualBounds.Width - this.Width;
        y = 0.0f;
        width = this.Width;
        height = actualBounds.Height;
        break;
      case PdfDockStyle.Fill:
        x = 0.0f;
        width = actualBounds.Width;
        height = actualBounds.Height;
        break;
    }
    simpleDockBounds = new RectangleF(x, y, width, height);
    return simpleDockBounds;
  }

  private RectangleF GetTemplateDockBounds(PdfPage page, PdfDocument document)
  {
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    if (document == null)
      throw new ArgumentNullException(nameof (document));
    RectangleF templateDockBounds = this.Bounds;
    PdfSection section = page.Section;
    RectangleF actualBounds = section.GetActualBounds(document, page, false);
    SizeF actualSize = section.PageSettings.GetActualSize();
    float x = this.X;
    float y = this.Y;
    float width = this.Width;
    float height = this.Height;
    switch (this.m_dockStyle)
    {
      case PdfDockStyle.Bottom:
        x = -actualBounds.X;
        y = actualBounds.Height + section.GetBottomIndentHeight(document, page, false) - this.Height;
        width = actualSize.Width;
        height = this.Height;
        if ((double) actualBounds.Height < 0.0)
        {
          y -= actualSize.Height;
          break;
        }
        break;
      case PdfDockStyle.Top:
        x = -actualBounds.X;
        y = -actualBounds.Y;
        width = actualSize.Width;
        height = this.Height;
        if ((double) actualBounds.Height < 0.0)
        {
          y = -actualBounds.Y + actualSize.Height;
          break;
        }
        break;
      case PdfDockStyle.Left:
        x = -actualBounds.X;
        y = 0.0f;
        width = this.Width;
        height = actualBounds.Height;
        break;
      case PdfDockStyle.Right:
        x = actualBounds.Width + section.GetRightIndentWidth(document, page, false) - this.Width;
        y = 0.0f;
        width = this.Width;
        height = actualBounds.Height;
        break;
      case PdfDockStyle.Fill:
        x = 0.0f;
        width = actualBounds.Width;
        height = actualBounds.Height;
        break;
    }
    templateDockBounds = new RectangleF(x, y, width, height);
    return templateDockBounds;
  }
}
