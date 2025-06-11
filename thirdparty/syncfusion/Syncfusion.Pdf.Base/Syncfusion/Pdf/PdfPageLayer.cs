// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfPageLayer
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfPageLayer : IPdfWrapper
{
  private PdfPageBase m_page;
  internal PdfGraphics m_graphics;
  internal PdfStream m_content;
  internal PdfGraphicsState m_graphicsState;
  private bool m_clipPageTemplates;
  private bool m_bSaved;
  private PdfColorSpace m_colorspace;
  private string m_layerid;
  private string m_name;
  internal bool m_visible = true;
  private PdfPageLayerCollection m_layer;
  internal bool m_sublayer;
  internal long m_contentLength;
  internal PdfDictionary m_printOption;
  internal PdfDictionary m_usage;
  private PdfPrintState printState;
  private bool m_isEndState;
  private PdfDictionary m_dictionary = new PdfDictionary();
  private PdfReferenceHolder m_refholder;
  internal bool isResourceLayer;

  internal PdfReferenceHolder ReferenceHolder
  {
    get => this.m_refholder;
    set => this.m_refholder = value;
  }

  internal PdfDictionary Dictionary
  {
    get => this.m_dictionary;
    set => this.m_dictionary = value;
  }

  internal PdfColorSpace Colorspace
  {
    get => this.m_colorspace;
    set => this.m_colorspace = value;
  }

  public PdfPageBase Page => this.m_page;

  internal string LayerId
  {
    get => this.m_layerid;
    set => this.m_layerid = value;
  }

  public string Name
  {
    get => this.m_name;
    set
    {
      this.m_name = value;
      if (this.Dictionary == null || value == null)
        return;
      this.Dictionary[nameof (Name)] = (IPdfPrimitive) new PdfString(value);
    }
  }

  public bool Visible
  {
    get
    {
      if (this.Dictionary != null && this.Dictionary.ContainsKey(nameof (Visible)))
        this.m_visible = (this.Dictionary[nameof (Visible)] as PdfBoolean).Value;
      return this.m_visible;
    }
    set
    {
      this.m_visible = value;
      if (this.Dictionary != null)
        this.Dictionary.SetProperty(nameof (Visible), (IPdfPrimitive) new PdfBoolean(value));
      this.SetVisibility(value);
    }
  }

  public PdfGraphics Graphics
  {
    get
    {
      if (this.m_graphics == null || this.m_bSaved)
        this.InitializeGraphics(this.Page);
      return this.m_graphics;
    }
  }

  public PdfPrintState PrintState
  {
    get => this.printState;
    set
    {
      this.printState = value;
      if (this.m_printOption == null)
        return;
      if (this.printState.Equals((object) PdfPrintState.AlwaysPrint))
      {
        this.m_printOption.SetProperty(nameof (PrintState), (IPdfPrimitive) new PdfName("ON"));
      }
      else
      {
        if (!this.PrintState.Equals((object) PdfPrintState.NeverPrint))
          return;
        this.m_printOption.SetProperty(nameof (PrintState), (IPdfPrimitive) new PdfName("OFF"));
      }
    }
  }

  public PdfPageLayer Add()
  {
    return new PdfPageLayer(this.m_page)
    {
      Name = string.Empty
    };
  }

  public PdfPageLayerCollection Layers
  {
    get
    {
      if (this.m_layer == null)
        this.m_layer = new PdfPageLayerCollection(this.Page);
      this.m_layer.m_sublayer = true;
      return this.m_layer;
    }
  }

  public PdfPageLayer(PdfPageBase page)
  {
    this.m_page = page != null ? page : throw new ArgumentNullException(nameof (page));
    this.m_clipPageTemplates = true;
    this.m_content = new PdfStream();
  }

  internal PdfPageLayer(PdfPageBase page, PdfStream stream)
  {
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    this.m_page = page;
    this.m_content = stream;
  }

  internal PdfPageLayer(PdfPageBase page, bool clipPageTemplates)
    : this(page)
  {
    this.m_clipPageTemplates = clipPageTemplates;
  }

  private void InitializeGraphics(PdfPageBase page)
  {
    PdfPage page1 = page as PdfPage;
    if (this.m_graphics == null)
    {
      PdfGraphics.GetResources resources = new PdfGraphics.GetResources(this.Page.GetResources);
      bool flag1 = false;
      bool flag2 = false;
      if (page.Dictionary.ContainsKey("MediaBox"))
        flag1 = true;
      PdfArray pdfArray1 = page.Dictionary.GetValue("MediaBox", "Parent") as PdfArray;
      float val1_1 = 0.0f;
      float val1_2 = 0.0f;
      float val2_1 = 0.0f;
      float val2_2 = 0.0f;
      PdfReferenceHolder element = new PdfReferenceHolder((IPdfWrapper) this);
      if (pdfArray1 != null)
      {
        val1_1 = (pdfArray1[0] as PdfNumber).FloatValue;
        val1_2 = (pdfArray1[1] as PdfNumber).FloatValue;
        val2_1 = (pdfArray1[2] as PdfNumber).FloatValue;
        val2_2 = (pdfArray1[3] as PdfNumber).FloatValue;
      }
      if (page.Dictionary.ContainsKey("CropBox"))
      {
        PdfArray pdfArray2 = page.Dictionary.GetValue("CropBox", "Parent") as PdfArray;
        float floatValue1 = (pdfArray2[0] as PdfNumber).FloatValue;
        float floatValue2 = (pdfArray2[1] as PdfNumber).FloatValue;
        float floatValue3 = (pdfArray2[2] as PdfNumber).FloatValue;
        float floatValue4 = (pdfArray2[3] as PdfNumber).FloatValue;
        if (((double) floatValue1 < 0.0 || (double) floatValue2 < 0.0 || (double) floatValue3 < 0.0 || (double) floatValue4 < 0.0) && Math.Floor((double) Math.Abs(floatValue2)) == Math.Floor((double) Math.Abs(page.Size.Height)) && Math.Floor((double) Math.Abs(floatValue1)) == Math.Floor((double) Math.Abs(page.Size.Width)))
        {
          RectangleF rectangleF = new RectangleF(Math.Min(floatValue1, floatValue3), Math.Min(floatValue2, floatValue4), Math.Max(floatValue1, floatValue3), Math.Max(floatValue2, floatValue4));
          this.m_graphics = new PdfGraphics(new SizeF(rectangleF.Width, rectangleF.Height), resources, this.m_content);
          if (!page.Contents.Contains((IPdfPrimitive) element))
            page.Contents.Add((IPdfPrimitive) element);
        }
        else
        {
          this.m_graphics = new PdfGraphics(page.Size, resources, this.m_content);
          this.m_graphics.m_cropBox = pdfArray2;
          if (!page.Contents.Contains((IPdfPrimitive) element))
            page.Contents.Add((IPdfPrimitive) element);
        }
      }
      else if (((double) val1_1 < 0.0 || (double) val1_2 < 0.0 || (double) val2_1 < 0.0 || (double) val2_2 < 0.0) && Math.Floor((double) Math.Abs(val1_2)) == Math.Floor((double) Math.Abs(page.Size.Height)) && Math.Floor((double) Math.Abs(val2_1)) == Math.Floor((double) page.Size.Width))
      {
        RectangleF rectangleF = new RectangleF(Math.Min(val1_1, val2_1), Math.Min(val1_2, val2_2), Math.Max(val1_1, val2_1), Math.Max(val1_2, val2_2));
        if ((double) rectangleF.Width <= 0.0 || (double) rectangleF.Height <= 0.0)
        {
          flag2 = true;
          if ((double) val1_1 < 0.0)
            val1_1 = -val1_1;
          else if ((double) val2_1 < 0.0)
            val2_1 = -val2_1;
          if ((double) val1_2 < 0.0)
            val1_2 = -val1_2;
          else if ((double) val2_2 < 0.0)
            val2_2 = -val2_2;
          rectangleF.Size = new SizeF(Math.Max(val1_1, val2_1), Math.Max(val1_2, val2_2));
        }
        this.m_graphics = new PdfGraphics(new SizeF(rectangleF.Width, rectangleF.Height), resources, this.m_content);
        if (!page.Contents.Contains((IPdfPrimitive) element))
          page.Contents.Add((IPdfPrimitive) element);
      }
      else
      {
        this.m_graphics = new PdfGraphics(page.Size, resources, this.m_content);
        if (!page.Contents.Contains((IPdfPrimitive) element))
          page.Contents.Add((IPdfPrimitive) element);
      }
      if (flag1)
        this.m_graphics.MediaBoxUpperRightBound = flag2 ? -val1_2 : val2_2;
      if (page1 != null)
      {
        PdfSectionCollection parent = page1.Section.Parent;
        if (parent != null)
        {
          this.m_graphics.ColorSpace = parent.Document.ColorSpace;
          this.Colorspace = parent.Document.ColorSpace;
        }
      }
      this.m_content.BeginSave += new SavePdfPrimitiveEventHandler(this.BeginSaveContent);
    }
    this.m_graphicsState = this.m_graphics.Save();
    if (!string.IsNullOrEmpty(this.m_name))
    {
      if (this.isResourceLayer && this.LayerId != null)
      {
        page.GetResources().AddProperties(this.LayerId, this.ReferenceHolder);
        if (this.isResourceLayer && !this.Dictionary.ContainsKey("LayerID"))
          this.Dictionary["LayerID"] = (IPdfPrimitive) new PdfName(this.LayerId);
      }
      byte[] bytes = Encoding.ASCII.GetBytes($"/OC /{this.LayerId} BDC\n");
      if (this.Name != null && this.Name != string.Empty)
      {
        this.m_graphics.StreamWriter.Write(bytes);
        this.m_isEndState = true;
      }
      else
        this.m_content.Write(bytes);
    }
    if ((double) page.Origin.X >= 0.0 && (double) page.Origin.Y >= 0.0 || Math.Sign(page.Origin.X) != Math.Sign(page.Origin.Y))
      this.m_graphics.InitializeCoordinates();
    else
      this.m_graphics.InitializeCoordinates(page);
    if (PdfGraphics.TransparencyObject)
      this.m_graphics.SetTransparencyGroup(page);
    if (this.Page is PdfLoadedPage && page.Rotation != PdfPageRotateAngle.RotateAngle0 || page.Dictionary.ContainsKey("Rotate"))
    {
      pdfNumber = (PdfNumber) null;
      if (page.Dictionary.ContainsKey("Rotate"))
      {
        if (!(page.Dictionary["Rotate"] is PdfNumber pdfNumber))
          pdfNumber = PdfCrossTable.Dereference(page.Dictionary["Rotate"]) as PdfNumber;
      }
      else if (page.Rotation != PdfPageRotateAngle.RotateAngle0)
      {
        pdfNumber = new PdfNumber(0);
        if (page.Rotation == PdfPageRotateAngle.RotateAngle90)
          pdfNumber.FloatValue = 90f;
        if (page.Rotation == PdfPageRotateAngle.RotateAngle180)
          pdfNumber.FloatValue = 180f;
        if (page.Rotation == PdfPageRotateAngle.RotateAngle270)
          pdfNumber.FloatValue = 270f;
      }
      if ((double) pdfNumber.FloatValue == 90.0)
      {
        if (page is PdfPage pdfPage1 && pdfPage1.Imported)
        {
          this.m_graphics.TranslateTransform(0.0f, page.Size.Width);
          this.m_graphics.RotateTransform(-90f);
          this.m_graphics.m_clipBounds.Size = new SizeF(page.Size.Width, page.Size.Height);
        }
        else
        {
          this.m_graphics.TranslateTransform(0.0f, page.Size.Height);
          this.m_graphics.RotateTransform(-90f);
          this.m_graphics.m_clipBounds.Size = new SizeF(page.Size.Height, page.Size.Width);
        }
      }
      else if ((double) pdfNumber.FloatValue == 180.0)
      {
        this.m_graphics.TranslateTransform(page.Size.Width, page.Size.Height);
        this.m_graphics.RotateTransform(-180f);
      }
      else if ((double) pdfNumber.FloatValue == 270.0)
      {
        if (page is PdfPage pdfPage2 && pdfPage2.Imported)
        {
          this.m_graphics.TranslateTransform(page.Size.Height, 0.0f);
          this.m_graphics.RotateTransform(-270f);
          this.m_graphics.m_clipBounds.Size = new SizeF(page.Size.Height, page.Size.Width);
        }
        else
        {
          this.m_graphics.TranslateTransform(page.Size.Width, 0.0f);
          this.m_graphics.RotateTransform(-270f);
          this.m_graphics.m_clipBounds.Size = new SizeF(page.Size.Height, page.Size.Width);
        }
      }
    }
    if (page1 != null)
    {
      RectangleF actualBounds = page1.Section.GetActualBounds(page1, true);
      PdfMargins margins = page1.Section.PageSettings.Margins;
      if (this.m_clipPageTemplates)
      {
        if ((double) page.Origin.X >= 0.0 && (double) page.Origin.Y >= 0.0)
          this.m_graphics.ClipTranslateMargins(actualBounds);
      }
      else
        this.m_graphics.ClipTranslateMargins(actualBounds.X, actualBounds.Y, margins.Left, margins.Top, margins.Right, margins.Bottom);
    }
    this.m_graphics.SetLayer(this);
    this.m_bSaved = false;
  }

  internal void Clear()
  {
    if (this.m_graphics != null)
      this.m_graphics.StreamWriter.Clear();
    if (this.m_content != null)
      this.m_content = (PdfStream) null;
    if (this.m_graphics == null)
      return;
    this.m_graphics = (PdfGraphics) null;
  }

  private void SetVisibility(bool value)
  {
    PdfDictionary pdfDictionary1 = this.m_page is PdfPage ? (PdfDictionary) (this.m_page as PdfPage).Document.Catalog : (PdfDictionary) (this.m_page as PdfLoadedPage).Document.Catalog;
    PdfDictionary pdfDictionary2 = (PdfDictionary) null;
    if (pdfDictionary1.ContainsKey("OCProperties"))
    {
      if (this.m_page is PdfPage)
        pdfDictionary2 = PdfCrossTable.Dereference((this.m_page as PdfPage).Document.Catalog["OCProperties"]) as PdfDictionary;
      else if (this.m_page is PdfLoadedPage)
        pdfDictionary2 = PdfCrossTable.Dereference((this.m_page as PdfLoadedPage).Document.Catalog["OCProperties"]) as PdfDictionary;
    }
    if (pdfDictionary2 == null || !(pdfDictionary2["D"] is PdfDictionary pdfDictionary3))
      return;
    PdfArray pdfArray1 = pdfDictionary3["ON"] as PdfArray;
    PdfArray pdfArray2 = pdfDictionary3["OFF"] as PdfArray;
    if (!(this.m_refholder != (PdfReferenceHolder) null))
      return;
    if (!value)
    {
      pdfArray1?.Remove((IPdfPrimitive) this.m_refholder);
      if (pdfArray2 == null)
      {
        pdfArray2 = new PdfArray();
        pdfDictionary3.Items.Add(new PdfName("OFF"), (IPdfPrimitive) pdfArray2);
      }
      pdfArray2.Add((IPdfPrimitive) this.m_refholder);
    }
    else
    {
      if (!value)
        return;
      pdfArray2?.Remove((IPdfPrimitive) this.m_refholder);
      if (pdfArray1 == null)
      {
        pdfArray1 = new PdfArray();
        pdfDictionary3.Items.Add(new PdfName("ON"), (IPdfPrimitive) pdfArray1);
      }
      pdfArray1.Add((IPdfPrimitive) this.m_refholder);
    }
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_content;

  private void BeginSaveContent(object sender, SavePdfPrimitiveEventArgs e)
  {
    if (this.m_graphicsState != null)
    {
      if (this.m_isEndState)
      {
        this.Graphics.StreamWriter.Write("EMC\n");
        this.m_isEndState = false;
      }
      this.Graphics.Restore(this.m_graphicsState);
      this.m_graphicsState = (PdfGraphicsState) null;
    }
    this.m_bSaved = true;
  }
}
