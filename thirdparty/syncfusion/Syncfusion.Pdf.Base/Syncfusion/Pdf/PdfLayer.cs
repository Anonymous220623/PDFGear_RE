// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfLayer
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfLayer : IPdfWrapper
{
  private PdfPageBase m_page;
  private PdfGraphics m_graphics;
  internal PdfStream m_content;
  private PdfGraphicsState m_graphicsState;
  private bool m_clipPageTemplates;
  private bool m_bSaved;
  private PdfColorSpace m_colorspace;
  private string m_layerid;
  private string m_name;
  internal bool m_visible = true;
  internal PdfDictionary m_printOption;
  internal PdfDictionary m_usage;
  private PdfPrintState printState;
  internal bool m_isEndState;
  private PdfDictionary m_dictionary = new PdfDictionary();
  private PdfReferenceHolder m_refholder;
  private PdfLayer layer;
  private PdfDocumentBase document;
  internal List<PdfPageBase> pages = new List<PdfPageBase>();
  private PdfDocumentLayerCollection m_layer;
  private bool m_sublayer;
  internal int m_sublayerposition;
  internal PdfArray sublayer = new PdfArray();
  internal bool m_locked;
  internal PdfArray m_lock;
  internal List<PdfLayer> m_parentLayer = new List<PdfLayer>();
  internal List<PdfLayer> m_child = new List<PdfLayer>();
  internal PdfLayer parent;
  private System.Collections.Generic.Dictionary<PdfGraphics, PdfGraphics> graphics = new System.Collections.Generic.Dictionary<PdfGraphics, PdfGraphics>();
  private System.Collections.Generic.Dictionary<PdfPageBase, PdfGraphics> pageGraphics = new System.Collections.Generic.Dictionary<PdfPageBase, PdfGraphics>();
  private bool m_pagePasrsed;
  private bool m_contentParsed;
  internal List<string> xobject = new List<string>();

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

  internal PdfPageBase Page
  {
    get
    {
      if (!this.m_pagePasrsed)
        this.ParsingLayerPage();
      return this.m_page;
    }
    set => this.m_page = value;
  }

  internal PdfDocumentBase Document
  {
    get => this.document;
    set => this.document = value;
  }

  internal string LayerId
  {
    get
    {
      if (!this.m_pagePasrsed)
        this.ParsingLayerPage();
      return this.m_layerid;
    }
    set => this.m_layerid = value;
  }

  internal PdfLayer Layer
  {
    get => this.layer;
    set => this.layer = value;
  }

  public string Name
  {
    get => this.m_name;
    set
    {
      this.m_name = value;
      if (this.Dictionary == null || this.m_name == null || !(this.Name != string.Empty))
        return;
      this.Dictionary.SetProperty(nameof (Name), (IPdfPrimitive) new PdfString(this.m_name));
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

  private PdfGraphics Graphics
  {
    get
    {
      if (this.m_graphics == null || this.m_bSaved)
        this.CreateGraphics(this.Page);
      return this.m_graphics;
    }
  }

  public PdfPrintState PrintState
  {
    get => this.printState;
    set
    {
      this.printState = value;
      if (this.m_printOption != null)
      {
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
      else
        this.SetPrintState();
    }
  }

  public PdfDocumentLayerCollection Layers
  {
    get
    {
      if (this.m_layer == null)
      {
        this.m_layer = new PdfDocumentLayerCollection(this.document, this.layer);
        this.m_layer.m_sublayer = true;
      }
      return this.m_layer;
    }
  }

  public bool Locked
  {
    get => this.m_locked;
    set
    {
      this.m_locked = value;
      this.SetLock(value);
    }
  }

  internal PdfStream ContentStream
  {
    get
    {
      if (!this.m_contentParsed)
        this.ParseContent();
      return this.m_content;
    }
  }

  internal PdfLayer()
  {
    this.m_clipPageTemplates = true;
    this.m_content = new PdfStream();
  }

  public PdfGraphics CreateGraphics(PdfPageBase page)
  {
    PdfPage page1 = page as PdfPage;
    this.m_page = page;
    if (this.m_graphics == null)
    {
      PdfReferenceHolder element = new PdfReferenceHolder((IPdfWrapper) this.layer);
      page.Contents.Add((IPdfPrimitive) element);
    }
    PdfResources resources1 = page.GetResources();
    if (string.IsNullOrEmpty(this.layer.LayerId))
      this.layer.LayerId = "OCG_" + Guid.NewGuid().ToString();
    if (resources1 != null && resources1.ContainsKey("Properties"))
    {
      if (resources1["Properties"] is PdfDictionary pdfDictionary)
      {
        string key = this.layer.LayerId.TrimStart('/');
        PdfReferenceHolder referenceHolder = this.layer.ReferenceHolder;
        pdfDictionary[key] = (IPdfPrimitive) referenceHolder;
      }
      else
        resources1.AddProperties(this.layer.LayerId.TrimStart('/'), this.layer.ReferenceHolder);
    }
    else
      resources1.AddProperties(this.layer.LayerId.TrimStart('/'), this.layer.ReferenceHolder);
    if (this.m_graphics == null)
    {
      PdfGraphics.GetResources resources2 = new PdfGraphics.GetResources(this.Page.GetResources);
      bool flag = false;
      if (page.Dictionary.ContainsKey("MediaBox"))
        flag = true;
      PdfArray pdfArray1 = page.Dictionary.GetValue("MediaBox", "Parent") as PdfArray;
      float val1_1 = 0.0f;
      float val1_2 = 0.0f;
      float val2_1 = 0.0f;
      float val2_2 = 0.0f;
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
          this.m_graphics = new PdfGraphics(new SizeF(rectangleF.Width, rectangleF.Height), resources2, this.m_content);
        }
        else
        {
          this.m_graphics = new PdfGraphics(page.Size, resources2, this.m_content);
          this.m_graphics.m_cropBox = pdfArray2;
        }
      }
      else if (((double) val1_1 < 0.0 || (double) val1_2 < 0.0 || (double) val2_1 < 0.0 || (double) val2_2 < 0.0) && Math.Floor((double) Math.Abs(val1_2)) == Math.Floor((double) Math.Abs(page.Size.Height)) && Math.Floor((double) Math.Abs(val2_1)) == Math.Floor((double) page.Size.Width))
      {
        RectangleF rectangleF = new RectangleF(Math.Min(val1_1, val2_1), Math.Min(val1_2, val2_2), Math.Max(val1_1, val2_1), Math.Max(val1_2, val2_2));
        this.m_graphics = new PdfGraphics(new SizeF(rectangleF.Width, rectangleF.Height), resources2, this.m_content);
      }
      else
        this.m_graphics = new PdfGraphics(page.Size, resources2, this.m_content);
      if (flag)
        this.m_graphics.MediaBoxUpperRightBound = val2_2;
      if (page1 != null)
      {
        PdfSectionCollection parent = page1.Section.Parent;
        if (parent != null)
        {
          this.m_graphics.ColorSpace = parent.Document.ColorSpace;
          this.Colorspace = parent.Document.ColorSpace;
        }
      }
      if (!this.graphics.ContainsKey(this.m_graphics))
        this.graphics[this.m_graphics] = this.m_graphics;
      if (!this.pageGraphics.ContainsKey(page))
        this.pageGraphics[page] = this.m_graphics;
      this.m_content.BeginSave += new SavePdfPrimitiveEventHandler(this.BeginSaveContent);
    }
    else if (!this.pages.Contains(page))
      this.GraphicsContent(page);
    else if (page is PdfLoadedPage && !this.graphics.ContainsKey(this.m_graphics))
      this.GraphicsContent(page);
    else if (this.pageGraphics.ContainsKey(page))
    {
      this.m_graphics = this.pageGraphics[page];
      return this.m_graphics;
    }
    this.m_graphics.StreamWriter.Write(Environment.NewLine);
    this.m_graphicsState = this.m_graphics.Save();
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
        this.m_graphics.TranslateTransform(0.0f, page.Size.Height);
        this.m_graphics.RotateTransform(-90f);
        this.m_graphics.m_clipBounds.Size = new SizeF(page.Size.Height, page.Size.Width);
      }
      else if ((double) pdfNumber.FloatValue == 180.0)
      {
        this.m_graphics.TranslateTransform(page.Size.Width, page.Size.Height);
        this.m_graphics.RotateTransform(-180f);
      }
      else if ((double) pdfNumber.FloatValue == 270.0)
      {
        this.m_graphics.TranslateTransform(page.Size.Width, 0.0f);
        this.m_graphics.RotateTransform(-270f);
        this.m_graphics.m_clipBounds.Size = new SizeF(page.Size.Height, page.Size.Width);
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
    if (!this.pages.Contains(page))
      this.pages.Add(page);
    this.m_graphics.SetLayer(this);
    this.m_bSaved = false;
    return this.m_graphics;
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
    PdfDictionary catalog = (PdfDictionary) this.document.Catalog;
    pdfDictionary1 = (PdfDictionary) null;
    if (catalog.ContainsKey("OCProperties") && !(catalog["OCProperties"] is PdfDictionary pdfDictionary1))
      pdfDictionary1 = (this.document.Catalog["OCProperties"] as PdfReferenceHolder).Object as PdfDictionary;
    if (pdfDictionary1 == null)
      return;
    pdfArray2 = (PdfArray) null;
    pdfArray1 = (PdfArray) null;
    if (!(pdfDictionary1["D"] is PdfDictionary pdfDictionary2))
      pdfDictionary2 = (pdfDictionary1["D"] as PdfReferenceHolder).Object as PdfDictionary;
    if (pdfDictionary2 == null)
      return;
    if (pdfDictionary2.ContainsKey("ON") && !(pdfDictionary2["ON"] is PdfArray pdfArray1))
      pdfArray1 = (pdfDictionary2["ON"] as PdfReferenceHolder).Object as PdfArray;
    if (pdfDictionary2.ContainsKey("OFF") && !(pdfDictionary2["OFF"] is PdfArray pdfArray2))
      pdfArray2 = (pdfDictionary2["OFF"] as PdfReferenceHolder).Object as PdfArray;
    if (!(this.m_refholder != (PdfReferenceHolder) null))
      return;
    if (!value)
    {
      pdfArray1?.Remove((IPdfPrimitive) this.m_refholder);
      if (pdfArray2 == null)
      {
        pdfArray2 = new PdfArray();
        pdfDictionary2.Items.Add(new PdfName("OFF"), (IPdfPrimitive) pdfArray2);
      }
      pdfArray2?.Remove((IPdfPrimitive) this.m_refholder);
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
        pdfDictionary2.Items.Add(new PdfName("ON"), (IPdfPrimitive) pdfArray1);
      }
      pdfArray1?.Remove((IPdfPrimitive) this.m_refholder);
      pdfArray1.Add((IPdfPrimitive) this.m_refholder);
    }
  }

  private void SetLock(bool isSetLock)
  {
    PdfDictionary catalog = (PdfDictionary) this.document.Catalog;
    pdfDictionary1 = (PdfDictionary) null;
    if (catalog.ContainsKey("OCProperties") && !(catalog["OCProperties"] is PdfDictionary pdfDictionary1))
      pdfDictionary1 = (this.document.Catalog["OCProperties"] as PdfReferenceHolder).Object as PdfDictionary;
    if (pdfDictionary1 == null)
      return;
    if (!(pdfDictionary1["D"] is PdfDictionary pdfDictionary2))
      pdfDictionary2 = (pdfDictionary1["D"] as PdfReferenceHolder).Object as PdfDictionary;
    if (pdfDictionary2 == null)
      return;
    PdfArray pdfArray = pdfDictionary2["Locked"] as PdfArray;
    if (!(this.m_refholder != (PdfReferenceHolder) null))
      return;
    if (isSetLock)
    {
      if (pdfArray != null)
      {
        if (pdfArray.Contains((IPdfPrimitive) this.m_refholder))
          return;
        pdfArray.Add((IPdfPrimitive) this.m_refholder);
      }
      else
      {
        this.m_lock = new PdfArray();
        this.m_lock.Add((IPdfPrimitive) this.m_refholder);
        pdfDictionary2.Items.Add(new PdfName("Locked"), (IPdfPrimitive) this.m_lock);
      }
    }
    else
    {
      if (isSetLock || pdfArray == null)
        return;
      pdfArray.Remove((IPdfPrimitive) this.m_refholder);
    }
  }

  private void SetPrintState()
  {
    PdfDictionary catalog = (PdfDictionary) this.document.Catalog;
    pdfDictionary1 = (PdfDictionary) null;
    if (catalog.ContainsKey("OCProperties") && !(catalog["OCProperties"] is PdfDictionary pdfDictionary1))
      pdfDictionary1 = (this.document.Catalog["OCProperties"] as PdfReferenceHolder).Object as PdfDictionary;
    if (!(pdfDictionary1["OCGs"] is PdfArray primitive1))
      primitive1 = (pdfDictionary1["OCGs"] as PdfReferenceHolder).Object as PdfArray;
    PdfDictionary pdfDictionary2 = this.Dictionary.ContainsKey("Usage") ? PdfCrossTable.Dereference(this.Dictionary["Usage"]) as PdfDictionary : new PdfDictionary();
    this.layer.m_printOption = new PdfDictionary();
    this.layer.m_printOption.SetProperty("Subtype", (IPdfPrimitive) new PdfName("Print"));
    if (this.layer.PrintState.Equals((object) PdfPrintState.NeverPrint))
      this.layer.m_printOption.SetProperty("PrintState", (IPdfPrimitive) new PdfName("OFF"));
    else if (this.layer.PrintState.Equals((object) PdfPrintState.AlwaysPrint))
      this.layer.m_printOption.SetProperty("PrintState", (IPdfPrimitive) new PdfName("ON"));
    pdfDictionary2.SetProperty("Print", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) this.layer.m_printOption));
    this.layer.m_usage = pdfDictionary2;
    this.Dictionary.SetProperty("Usage", (IPdfPrimitive) this.layer.m_usage);
    PdfArray primitive2 = new PdfArray();
    primitive2.Add((IPdfPrimitive) new PdfName("Print"));
    PdfDictionary pdfDictionary3 = new PdfDictionary();
    pdfDictionary3.SetProperty("Category", (IPdfPrimitive) primitive2);
    pdfDictionary3.SetProperty("OCGs", (IPdfPrimitive) primitive1);
    pdfDictionary3.SetProperty("Event", (IPdfPrimitive) new PdfName("Print"));
    PdfArray pdfArray = new PdfArray();
    pdfArray.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary3));
    if (!(pdfDictionary1["D"] is PdfDictionary pdfDictionary4))
      pdfDictionary4 = (pdfDictionary1["D"] as PdfReferenceHolder).Object as PdfDictionary;
    pdfDictionary4["AS"] = (IPdfPrimitive) pdfArray;
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_content;

  private void BeginSaveContent(object sender, SavePdfPrimitiveEventArgs e)
  {
    if (this.layer.graphics == null)
      return;
    using (System.Collections.Generic.Dictionary<PdfGraphics, PdfGraphics>.Enumerator enumerator = this.graphics.GetEnumerator())
    {
      if (!enumerator.MoveNext())
        return;
      KeyValuePair<PdfGraphics, PdfGraphics> current = enumerator.Current;
      this.m_graphics = current.Key;
      if (!this.m_graphics.isEmptyLayer)
      {
        this.BeginLayer(this.m_graphics);
        this.m_graphics.EndMarkContent();
      }
      this.m_graphics.StreamWriter.Write("Q" + Environment.NewLine);
      this.graphics.Remove(current.Key);
    }
  }

  internal void BeginLayer(PdfGraphics currentGraphics)
  {
    if (this.graphics != null)
      this.m_graphics = !this.graphics.ContainsKey(currentGraphics) ? currentGraphics : this.graphics[currentGraphics];
    if (this.m_graphics == null || string.IsNullOrEmpty(this.m_name))
      return;
    this.m_graphics.isEmptyLayer = true;
    if (this.m_parentLayer.Count != 0)
    {
      for (int index = 0; index < this.m_parentLayer.Count; ++index)
      {
        if (!string.IsNullOrEmpty(this.m_parentLayer[index].LayerId))
          this.m_graphics.StreamWriter.Write(Encoding.ASCII.GetBytes($"/OC /{this.m_parentLayer[index].LayerId.TrimStart('/')} BDC\n"));
      }
    }
    byte[] bytes = Encoding.ASCII.GetBytes($"/OC /{this.LayerId.TrimStart('/')} BDC\n");
    if (this.Name != null && this.Name != string.Empty)
    {
      this.m_graphics.StreamWriter.Write(bytes);
      this.m_isEndState = true;
    }
    else
      this.m_content.Write(bytes);
  }

  private void GraphicsContent(PdfPageBase page)
  {
    PdfGraphics.GetResources resources = new PdfGraphics.GetResources(page.GetResources);
    PdfStream stream = new PdfStream();
    this.m_graphics = new PdfGraphics(page.Size, resources, stream);
    page.Contents.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) stream));
    stream.BeginSave += new SavePdfPrimitiveEventHandler(this.BeginSaveContent);
    if (!this.graphics.ContainsKey(this.m_graphics))
      this.graphics[this.m_graphics] = this.m_graphics;
    if (this.pageGraphics.ContainsKey(page))
      return;
    this.pageGraphics[page] = this.m_graphics;
  }

  private void ParseContent()
  {
    if (this.layer.Page == null || !(this.layer.Page is PdfLoadedPage))
      return;
    PdfPageLayerCollection layers = this.layer.Page.Layers;
    if (layers == null)
      return;
    for (int index = 0; index < layers.Count; ++index)
    {
      PdfPageLayer pdfPageLayer = layers[index];
      if (pdfPageLayer.Name != null && pdfPageLayer.Name != string.Empty && pdfPageLayer.ReferenceHolder == this.layer.ReferenceHolder && pdfPageLayer != null)
      {
        this.layer.m_content = pdfPageLayer.m_content;
        this.layer.m_graphics = new PdfGraphics(this.layer.Page.Size, new PdfGraphics.GetResources(this.layer.Page.GetResources), this.layer.m_content);
        break;
      }
    }
    this.m_contentParsed = true;
  }

  private void ParsingLayerPage()
  {
    if (this.document == null || !(this.document is PdfLoadedDocument))
      return;
    for (int index = 0; index < this.document.PageCount; ++index)
    {
      PdfDictionary dictionary1 = (this.document as PdfLoadedDocument).Pages[index].Dictionary;
      PdfPageBase page = (this.document as PdfLoadedDocument).Pages[index];
      if (dictionary1.ContainsKey("Resources") && (PdfCrossTable.Dereference(dictionary1["Resources"]) is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey("Properties") || pdfDictionary.ContainsKey("XObject")))
      {
        PdfDictionary pdfDictionary1 = PdfCrossTable.Dereference(pdfDictionary["Properties"]) as PdfDictionary;
        PdfDictionary pdfDictionary2 = PdfCrossTable.Dereference(pdfDictionary["XObject"]) as PdfDictionary;
        if (pdfDictionary1 != null)
        {
          foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in pdfDictionary1.Items)
          {
            if ((object) (keyValuePair.Value as PdfReferenceHolder) != null)
            {
              PdfReferenceHolder reference = keyValuePair.Value as PdfReferenceHolder;
              PdfDictionary dictionary2 = reference.Object as PdfDictionary;
              PdfName key = keyValuePair.Key;
              if (this.ParsingDictionary(dictionary2, reference, page, key))
                break;
            }
          }
        }
        if (pdfDictionary2 != null)
        {
          foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in pdfDictionary2.Items)
          {
            PdfDictionary pdfDictionary3 = (keyValuePair.Value as PdfReferenceHolder).Object as PdfDictionary;
            if (pdfDictionary3.ContainsKey("OC"))
            {
              PdfName key = keyValuePair.Key;
              PdfReferenceHolder reference = pdfDictionary3["OC"] as PdfReferenceHolder;
              if (this.ParsingDictionary(PdfCrossTable.Dereference(pdfDictionary3["OC"]) as PdfDictionary, reference, page, key))
              {
                this.layer.xobject.Add(key.Value.TrimStart('/'));
                break;
              }
            }
          }
        }
      }
    }
  }

  private bool ParsingDictionary(
    PdfDictionary dictionary,
    PdfReferenceHolder reference,
    PdfPageBase pagebase,
    PdfName layerID)
  {
    bool flag = false;
    if (!dictionary.ContainsKey("Name") && dictionary.ContainsKey("OCGs"))
    {
      if (dictionary.ContainsKey("OCGs"))
      {
        if (!(PdfCrossTable.Dereference(dictionary["OCGs"]) is PdfArray pdfArray))
        {
          reference = dictionary["OCGs"] as PdfReferenceHolder;
          dictionary = PdfCrossTable.Dereference(dictionary["OCGs"]) as PdfDictionary;
          if (dictionary != null && dictionary.ContainsKey("Name"))
            flag = this.SetLayerPage(reference, pagebase, layerID);
        }
        else
        {
          for (int index = 0; index < pdfArray.Count; ++index)
          {
            if ((object) (pdfArray[index] as PdfReferenceHolder) != null)
            {
              reference = pdfArray[index] as PdfReferenceHolder;
              dictionary = (pdfArray[index] as PdfReferenceHolder).Object as PdfDictionary;
              flag = this.SetLayerPage(reference, pagebase, layerID);
            }
          }
        }
      }
    }
    else if (dictionary.ContainsKey("Name"))
      flag = this.SetLayerPage(reference, pagebase, layerID);
    return flag;
  }

  private bool SetLayerPage(PdfReferenceHolder reference, PdfPageBase pagebase, PdfName layerID)
  {
    bool flag = false;
    if (this.layer.ReferenceHolder != (PdfReferenceHolder) null && this.layer.ReferenceHolder.Equals((object) reference))
    {
      this.layer.m_pagePasrsed = true;
      flag = true;
      this.layer.LayerId = layerID.Value.TrimStart('/');
      this.layer.Page = pagebase;
      if (!this.layer.pages.Contains(pagebase))
        this.layer.pages.Add(pagebase);
    }
    return flag;
  }
}
