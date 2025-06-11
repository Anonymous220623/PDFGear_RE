// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfPageBase
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Exporting;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using Syncfusion.Pdf.Xmp;
using Syncfusion.PdfViewer.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

#nullable disable
namespace Syncfusion.Pdf;

public abstract class PdfPageBase : IPdfWrapper
{
  internal List<string> pageWords = new List<string>();
  private bool isExtractWithFormat;
  private PdfDictionary m_pageDictionary;
  private PdfResources m_resources;
  private PdfPageLayerCollection m_layers;
  private PdfLoadedAnnotationCollection m_annotations;
  private int m_defLayerIndex = -1;
  private List<PdfName> m_fontNames = new List<PdfName>();
  internal List<IPdfPrimitive> m_fontReference = new List<IPdfPrimitive>();
  private System.Collections.Generic.Dictionary<PdfName, IPdfPrimitive> m_fontcollect;
  private PdfTemplate m_contentTemplate;
  private int m_annotCount;
  private int m_layersCount;
  private long m_pageContentLength;
  private bool m_imported;
  private static object s_syncLockTemplate = new object();
  private int m_fieldCount;
  internal List<PdfStream> m_xObjectContentStream;
  private PdfFormFieldsTabOrder m_formFieldsTabOrder;
  private PdfPageRotateAngle m_LoadedrotateAngle;
  internal bool m_isProgressOn;
  internal bool m_removedPage;
  internal bool isExtractImages;
  internal bool isFlateCompress;
  private bool m_modified;
  private System.Collections.Generic.Dictionary<int, string> m_imageLengthDict = new System.Collections.Generic.Dictionary<int, string>();
  private bool m_isContainsImage;
  private int m_nonBreakingSpaceCharValue = 160 /*0xA0*/;
  private long resourceNumber;
  internal List<RectangleF> m_RedactionBounds = new List<RectangleF>();
  internal bool is_Contains_Redaction;
  private float pt = 1.3333f;
  internal PdfArray m_childSTR = new PdfArray();
  internal int m_id;
  private List<PdfMatrix> extractedImageMatrix = new List<PdfMatrix>();
  private List<bool> maskImageCollection = new List<bool>();
  private PageResourceLoader m_resourceLoader = new PageResourceLoader();
  private List<RectangleF> m_extractedImagesBounds = new List<RectangleF>();
  internal PdfImageInfo[] m_imageinfo;
  internal PdfRecordCollection m_recordCollection;
  internal List<IPdfPrimitive> m_image_reference = new List<IPdfPrimitive>();
  private List<PdfName> m_image_Names = new List<PdfName>();
  internal PdfPageResources m_pageResources;
  private char[] m_symbolChars = new char[6]
  {
    '(',
    ')',
    '[',
    ']',
    '<',
    '>'
  };
  private ArrayList extractedImages = new ArrayList();
  private List<PdfImageInfo> m_imageInfoList = new List<PdfImageInfo>();
  private List<string> m_ImageKey = new List<string>();
  internal Stack<GraphicsStateData> m_currentMatrix = new Stack<GraphicsStateData>();
  internal bool isRotate;
  private System.Collections.Generic.Dictionary<PdfName, float> m_roatedImages = new System.Collections.Generic.Dictionary<PdfName, float>();
  private float m_characterSpacing;
  internal List<RectangleF> m_headerFooterBounds;
  private string m_currentFont;
  private bool m_isLayoutTextExtraction;
  private Stack<GraphicsState> m_graphicsState = new Stack<GraphicsState>();
  private PdfColorSpace m_colorSpace;
  private string resultantText;
  private float m_wordSpacing;
  private float TextHorizontalScaling = 100f;
  private float m_fontSize;
  private float m_previousFontSize;
  private Syncfusion.PdfViewer.Base.Matrix m_previousTextMatrix;
  private Syncfusion.PdfViewer.Base.Matrix m_textLineMatrix;
  private Syncfusion.PdfViewer.Base.Matrix m_currentTextMatrix;
  internal Syncfusion.PdfViewer.Base.Matrix Ctm = Syncfusion.PdfViewer.Base.Matrix.Identity;
  internal int m_rise;
  private int m_horizontalScaling = 100;
  private int m_charID;
  private FontStructure m_structure;
  internal System.Collections.Generic.Dictionary<int, int> FontGlyphWidths;
  private double m_advancedWidth;
  private float m_charSizeMultiplier = 1f / 1000f;
  private double m_characterWidth;
  private Syncfusion.PdfViewer.Base.Matrix m_textMatrix;
  private PdfUnitConvertor m_unitConvertor;
  private TransformationStack m_transformations;
  private System.Drawing.Graphics m_graphics;
  private bool m_isTextMatrix;
  private PointF m_currentLocation = PointF.Empty;
  private bool m_isRotated;
  private RectangleF m_boundingRect;
  private RectangleF m_tempBoundingRectangle = new RectangleF();
  private float m_textLeading;
  private string m_strBackup = string.Empty;
  private bool m_hasNoSpacing;
  private bool m_hasLeading;
  private bool m_hasTj;
  private bool m_hasTm;
  private bool m_hasET;
  private bool m_hasBDC;
  private bool spaceBetweenWord;

  public PdfImageInfo[] ImagesInfo
  {
    get
    {
      if (this.m_imageinfo != null)
        return this.m_imageinfo;
      try
      {
        Image[] images = this.ExtractImages(true);
        int index1 = 0;
        int index2 = 0;
        IEnumerator enumerator = (IEnumerator) this.m_extractedImagesBounds.GetEnumerator();
        while (enumerator.MoveNext())
        {
          if (!this.m_imageinfo[index1].IsImageExtracted)
          {
            this.m_imageinfo[index1].Bounds = (RectangleF) enumerator.Current;
            this.m_imageinfo[index1].Image = (Image) null;
            this.m_imageinfo[index1].Index = index1;
            ++index1;
          }
          else if (this.m_imageinfo[index1].IsImageExtracted)
          {
            Image image = images[index2];
            this.m_imageinfo[index1].Bounds = (RectangleF) enumerator.Current;
            this.m_imageinfo[index1].Image = image;
            this.m_imageinfo[index1].Index = index1;
            ++index1;
            ++index2;
          }
        }
      }
      catch (Exception ex)
      {
      }
      this.ClearImageResources();
      return this.m_imageinfo;
    }
  }

  private void ClearImageResources()
  {
    if (this.m_pageResources != null)
      this.m_pageResources.Resources.Clear();
    this.m_pageResources = (PdfPageResources) null;
    if (this.m_recordCollection != null)
      this.m_recordCollection.RecordCollection.Clear();
    this.m_recordCollection = (PdfRecordCollection) null;
  }

  private GraphicsStateData Objects => this.m_currentMatrix.Peek();

  public PdfGraphics Graphics => this.DefaultLayer.Graphics;

  internal bool Imported
  {
    get => this.m_imported;
    set => this.m_imported = value;
  }

  public PdfPageLayerCollection Layers
  {
    get
    {
      if (this.m_layers == null)
        this.m_layers = new PdfPageLayerCollection(this);
      return this.m_layers;
    }
  }

  public PdfLoadedAnnotationCollection Annotations
  {
    get
    {
      PdfLoadedAnnotationCollection annotations = (this as PdfLoadedPage).Annotations;
      if (this.m_annotations == null || this.m_annotations.Annotations.Count == 0 && this.m_annotations.Count != 0)
        this.m_annotations = new PdfLoadedAnnotationCollection(this as PdfLoadedPage);
      return this.m_annotations;
    }
  }

  public PdfFormFieldsTabOrder FormFieldsTabOrder
  {
    get => this.m_formFieldsTabOrder;
    set
    {
      this.m_formFieldsTabOrder = value;
      if (this.m_formFieldsTabOrder == PdfFormFieldsTabOrder.None)
        return;
      string str = "";
      if (this.m_formFieldsTabOrder == PdfFormFieldsTabOrder.Row)
        str = "R";
      if (this.m_formFieldsTabOrder == PdfFormFieldsTabOrder.Column)
        str = "C";
      if (this.m_formFieldsTabOrder == PdfFormFieldsTabOrder.Structure)
        str = "S";
      this.Dictionary["Tabs"] = (IPdfPrimitive) new PdfName(str);
    }
  }

  public int DefaultLayerIndex
  {
    get
    {
      if (this.Layers.Count == 0 || this.m_defLayerIndex == -1)
        this.m_defLayerIndex = this.Layers.IndexOf(this.Layers.Add());
      return this.m_defLayerIndex;
    }
    set
    {
      if (value < 0 || value > this.Layers.Count - 1)
        throw new ArgumentOutOfRangeException(nameof (value), "Index can not be less 0 and greater Layers.Count - 1");
      this.m_defLayerIndex = value;
      this.m_modified = true;
    }
  }

  public PdfPageLayer DefaultLayer => this.Layers[this.DefaultLayerIndex];

  public abstract SizeF Size { get; }

  internal abstract PointF Origin { get; }

  internal PdfArray Contents
  {
    get
    {
      IPdfPrimitive page = this.m_pageDictionary[nameof (Contents)];
      contents = page as PdfArray;
      PdfReferenceHolder pdfReferenceHolder = page as PdfReferenceHolder;
      if (pdfReferenceHolder != (PdfReferenceHolder) null && !(pdfReferenceHolder.Object is PdfArray contents) && pdfReferenceHolder.Object is PdfStream pdfStream)
      {
        contents = new PdfArray();
        contents.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfStream));
        this.m_pageDictionary[nameof (Contents)] = (IPdfPrimitive) contents;
      }
      if (contents == null)
      {
        contents = new PdfArray();
        this.m_pageDictionary[nameof (Contents)] = (IPdfPrimitive) contents;
      }
      return contents;
    }
  }

  internal PdfDictionary Dictionary => this.m_pageDictionary;

  public PdfPageRotateAngle Rotation
  {
    get
    {
      this.m_LoadedrotateAngle = this.ObtainRotation();
      return this.m_LoadedrotateAngle;
    }
    set
    {
      PdfPage pdfPage = this as PdfPage;
      if (!(this is PdfLoadedPage) && (pdfPage == null || !pdfPage.Imported))
        return;
      this.m_LoadedrotateAngle = value;
      int num1 = 90;
      int num2 = 360;
      int num3 = (int) this.m_LoadedrotateAngle * num1;
      if (num3 >= 360)
        num3 %= num2;
      this.Dictionary["Rotate"] = (IPdfPrimitive) new PdfNumber(num3);
    }
  }

  internal PdfPageOrientation Orientation => this.ObtainOrientation();

  internal PdfTemplate ContentTemplate
  {
    get
    {
      bool flag = false;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        this.Layers.CombineContent((Stream) memoryStream);
        if (this.m_pageContentLength != memoryStream.Length)
          flag = true;
      }
      if (this.m_contentTemplate == null || this.m_contentTemplate.m_content.Data.Length == 0 || this.m_layersCount != (this.m_layers == null ? 0 : this.m_layers.Count) || this.m_annotCount != this.GetAnnotationCount())
        flag = true;
      if (this.m_modified || flag)
        this.m_contentTemplate = this.GetContent();
      return this.m_contentTemplate;
    }
  }

  internal int FieldsCount => this.m_fieldCount;

  internal PdfPageBase(PdfDictionary dic)
  {
    this.m_transformations = new TransformationStack();
    if (this.m_transformations != null)
      this.m_transformations.Clear();
    this.m_pageDictionary = dic != null ? dic : throw new ArgumentNullException(nameof (dic));
  }

  public void ReplaceImage(int imageIndex, PdfImage image)
  {
    if (image is PdfMetafile)
      throw new NotSupportedException("Meta file image can't replaced");
    PdfPageBase currentpage = this;
    if (imageIndex < 0)
      throw new ArgumentException("Image index is not valid");
    if (image == null)
      throw new NullReferenceException(nameof (image));
    this.m_modified = true;
    try
    {
      PdfImageInfo[] imagesInfo = this.ImagesInfo;
      float num1;
      if (this.m_roatedImages.ContainsKey(new PdfName(imagesInfo[imageIndex].Name)) && this.m_roatedImages.TryGetValue(new PdfName(imagesInfo[imageIndex].Name), out num1))
      {
        if ((double) num1 == 90.0)
        {
          imagesInfo[imageIndex].Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
          image.InternalImage.RotateFlip(RotateFlipType.Rotate270FlipNone);
        }
        else if ((double) num1 == 180.0)
        {
          imagesInfo[imageIndex].Image.RotateFlip(RotateFlipType.RotateNoneFlipNone);
          image.InternalImage.RotateFlip(RotateFlipType.RotateNoneFlipNone);
        }
        else if ((double) num1 == 270.0)
        {
          imagesInfo[imageIndex].Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
          image.InternalImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
        }
      }
      image.Save();
      PdfReferenceHolder imageReference = (PdfReferenceHolder) null;
      PdfResources resources = this.GetResources();
      for (int index = 0; index < imagesInfo.Length; ++index)
      {
        if (!this.m_imageLengthDict.ContainsKey(imagesInfo[index].Index))
          this.m_imageLengthDict.Add(imagesInfo[index].Index, imagesInfo[index].Name);
      }
      if (!this.m_imageLengthDict.ContainsKey(imageIndex))
        throw new ArgumentException("Image Index is not valid");
      if (!resources.ContainsKey("XObject") || !(resources["XObject"] is PdfDictionary) && (object) (resources["XObject"] as PdfReferenceHolder) == null)
        return;
      PdfDictionary pdfDictionary1 = resources["XObject"] as PdfDictionary;
      if ((object) (resources["XObject"] as PdfReferenceHolder) != null && pdfDictionary1 == null)
        pdfDictionary1 = (resources["XObject"] as PdfReferenceHolder).Object as PdfDictionary;
      PdfDictionary pdfDictionary2 = new PdfDictionary();
      if (pdfDictionary1 == null || pdfDictionary1 == null)
        return;
      foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair1 in pdfDictionary1.Items)
      {
        KeyValuePair<PdfName, IPdfPrimitive> keyValuePair2 = keyValuePair1;
        bool imageReplaced = false;
        keyValuePair2 = this.FetchImageDictionary(keyValuePair2, imagesInfo[imageIndex].Name, (SizeF) imagesInfo[imageIndex].Image.Size, out imageReplaced);
        PdfDictionary fontDictionary;
        PdfDictionary pdfDictionary3;
        if ((object) (keyValuePair2.Value as PdfReferenceHolder) != null)
        {
          fontDictionary = (keyValuePair2.Value as PdfReferenceHolder).Object as PdfDictionary;
          PdfDictionary dictionary = new PdfDictionary();
          PdfDictionary.SetProperty(dictionary, keyValuePair2.Key.Value, keyValuePair2.Value);
          pdfDictionary3 = dictionary;
        }
        else
        {
          fontDictionary = keyValuePair2.Value as PdfDictionary;
          pdfDictionary3 = fontDictionary;
        }
        if (fontDictionary.ContainsKey("Subtype") && imageReplaced && (fontDictionary["Subtype"] as PdfName).Value == "Image")
        {
          IPdfPrimitive pdfPrimitive = resources["XObject"];
          string key = this.StripSlashes(keyValuePair2.Key.ToString());
          PdfArray pdfArray = (PdfArray) null;
          if (fontDictionary.ContainsKey("ColorSpace"))
            pdfArray = PdfCrossTable.Dereference(fontDictionary["ColorSpace"]) as PdfArray;
          ImageStructure imageStructure = new ImageStructure((IPdfPrimitive) fontDictionary, new PdfMatrix());
          SizeF sizeF = new SizeF(imageStructure.Width, imageStructure.Height);
          if (imageStructure.ImageDictionary.ContainsKey("SMask") && (double) imageStructure.EmbeddedImage.Height > (double) sizeF.Height && (double) imageStructure.EmbeddedImage.Width > (double) sizeF.Width)
            sizeF = (SizeF) imageStructure.EmbeddedImage.Size;
          if (pdfArray != null)
          {
            PdfName pdfName = PdfCrossTable.Dereference(pdfArray[0]) as PdfName;
            if (pdfName != (PdfName) null && pdfName.Value == "ICCBased")
              image.Stream["ColorSpace"] = (IPdfPrimitive) pdfArray;
          }
          if (imageReference == (PdfReferenceHolder) null)
            imageReference = new PdfReferenceHolder((IPdfWrapper) image);
          if (imagesInfo[imageIndex].Name == key && (SizeF) imagesInfo[imageIndex].Image.Size == sizeF)
          {
            PdfImageInfo[] pdfImageInfoArray = imagesInfo;
            int index = 0;
            if (index < pdfImageInfoArray.Length)
            {
              PdfImageInfo pdfImageInfo = pdfImageInfoArray[index];
              imageReplaced = true;
              if (imagesInfo[imageIndex].Image != null)
              {
                long objNum = (pdfDictionary3[key] as PdfReferenceHolder).Reference.ObjNum;
                int num2 = 0;
                foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair3 in pdfDictionary2.Items)
                {
                  if (num2 == pdfDictionary2.Count - 1)
                    pdfDictionary3 = pdfDictionary2[keyValuePair3.Key.Value] as PdfDictionary;
                  ++num2;
                }
                pdfDictionary2.Clear();
                if ((keyValuePair2.Value as PdfReferenceHolder).Object is PdfStream)
                {
                  PdfStream pdfStream = (keyValuePair2.Value as PdfReferenceHolder).Object as PdfStream;
                  switch (currentpage)
                  {
                    case PdfPage _:
                      if ((currentpage as PdfPage).Document.FileStructure.IncrementalUpdate)
                      {
                        pdfStream.Modify();
                        pdfStream.Clear();
                        break;
                      }
                      pdfStream.Clear();
                      break;
                    case PdfLoadedPage _:
                      pdfStream.Clear();
                      break;
                  }
                }
                float height = (float) imagesInfo[imageIndex].Image.Height;
                if ((double) (this.Size.Height - (imagesInfo[imageIndex].Bounds.Top + height)) >= 0.0)
                {
                  if (currentpage is PdfPage)
                  {
                    pdfDictionary3.Items.Remove(keyValuePair2.Key);
                    pdfDictionary3.Items.Add(keyValuePair2.Key, (IPdfPrimitive) imageReference);
                    pdfDictionary3.Modify();
                  }
                  else if (currentpage is PdfLoadedPage)
                    this.ReplaceImageStream(objNum, imageReference, currentpage);
                }
                else if (currentpage is PdfPage)
                {
                  pdfDictionary3.Items.Remove(keyValuePair2.Key);
                  pdfDictionary3.Items.Add(keyValuePair2.Key, (IPdfPrimitive) imageReference);
                  pdfDictionary3.Modify();
                }
                else if (currentpage is PdfLoadedPage)
                  this.ReplaceImageStream(objNum, imageReference, currentpage);
              }
              else
              {
                long objNum = (keyValuePair2.Value as PdfReferenceHolder).Reference.ObjNum;
                if ((keyValuePair2.Value as PdfReferenceHolder).Object is PdfStream)
                {
                  PdfStream pdfStream = (keyValuePair2.Value as PdfReferenceHolder).Object as PdfStream;
                  switch (currentpage)
                  {
                    case PdfPage _:
                      if ((currentpage as PdfPage).Document.FileStructure.IncrementalUpdate)
                      {
                        pdfStream.Modify();
                        pdfStream.Clear();
                        break;
                      }
                      pdfStream.Clear();
                      break;
                    case PdfLoadedPage _:
                      pdfStream.Clear();
                      break;
                  }
                }
                float height = imagesInfo[imageIndex].Bounds.Height;
                if ((double) (this.Size.Height - (imagesInfo[imageIndex].Bounds.Top + height)) >= 0.0)
                {
                  if (currentpage is PdfPage)
                  {
                    pdfDictionary3.Items.Remove(keyValuePair2.Key);
                    pdfDictionary3.Items.Add(keyValuePair2.Key, (IPdfPrimitive) imageReference);
                    pdfDictionary3.Modify();
                  }
                  else if (currentpage is PdfLoadedPage)
                    this.ReplaceImageStream(objNum, imageReference, currentpage);
                }
                else if (currentpage is PdfPage)
                {
                  pdfDictionary3.Items.Remove(keyValuePair2.Key);
                  pdfDictionary3.Items.Add(keyValuePair2.Key, (IPdfPrimitive) imageReference);
                  pdfDictionary3.Modify();
                }
                else if (currentpage is PdfLoadedPage)
                  this.ReplaceImageStream(objNum, imageReference, currentpage);
              }
            }
          }
        }
        if (imageReplaced)
        {
          this.m_imageLengthDict.Clear();
          break;
        }
      }
    }
    catch (Exception ex)
    {
      if (ex is ArgumentException)
        throw ex;
    }
  }

  private KeyValuePair<PdfName, IPdfPrimitive> FetchImageDictionary(
    KeyValuePair<PdfName, IPdfPrimitive> item,
    string ImageName,
    SizeF sizeImage,
    out bool imageReplaced)
  {
    imageReplaced = false;
    KeyValuePair<PdfName, IPdfPrimitive> keyValuePair1 = item;
    PdfDictionary pdfDictionary1 = new PdfDictionary();
    PdfDictionary fontDictionary = (object) (item.Value as PdfReferenceHolder) == null ? item.Value as PdfDictionary : (item.Value as PdfReferenceHolder).Object as PdfDictionary;
    if (fontDictionary.ContainsKey("Subtype") && (fontDictionary["Subtype"] as PdfName).Value == "Image")
    {
      string str = this.StripSlashes(item.Key.ToString());
      ImageStructure imageStructure = new ImageStructure((IPdfPrimitive) fontDictionary, new PdfMatrix());
      SizeF sizeF = new SizeF(imageStructure.Width, imageStructure.Height);
      if (imageStructure.ImageDictionary.ContainsKey("SMask") && (double) imageStructure.EmbeddedImage.Height > (double) sizeF.Height && (double) imageStructure.EmbeddedImage.Width > (double) sizeF.Width)
        sizeF = (SizeF) imageStructure.EmbeddedImage.Size;
      bool flag = false;
      if (this.m_roatedImages.Count != 0 && this.m_roatedImages.ContainsKey(new PdfName(ImageName)))
        flag = true;
      if (str == ImageName && (sizeF == sizeImage || flag))
      {
        imageReplaced = true;
        keyValuePair1 = item;
      }
    }
    if (fontDictionary.ContainsKey("Resources"))
    {
      PdfDictionary pdfDictionary2 = (object) (fontDictionary["Resources"] as PdfReferenceHolder) == null ? fontDictionary["Resources"] as PdfDictionary : (fontDictionary["Resources"] as PdfReferenceHolder).Object as PdfDictionary;
      if (pdfDictionary2.ContainsKey("XObject"))
      {
        this.m_isContainsImage = true;
        PdfDictionary pdfDictionary3 = pdfDictionary2["XObject"] as PdfDictionary;
        if (pdfDictionary3.ContainsKey("Subtype"))
        {
          keyValuePair1 = this.FetchImageDictionary(new KeyValuePair<PdfName, IPdfPrimitive>(item.Key, (IPdfPrimitive) pdfDictionary3), ImageName, sizeImage, out imageReplaced);
        }
        else
        {
          foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair2 in pdfDictionary3.Items)
          {
            keyValuePair1 = this.FetchImageDictionary(keyValuePair2, ImageName, sizeImage, out imageReplaced);
            if (imageReplaced)
              break;
          }
        }
      }
    }
    return keyValuePair1;
  }

  internal void ReplaceImageStream(
    long objIndex,
    PdfReferenceHolder imageReference,
    PdfPageBase currentpage)
  {
    PdfDictionary pdfDictionary1 = ((currentpage is PdfPage ? (currentpage as PdfPage).CrossTable : (currentpage as PdfLoadedPage).CrossTable) ?? ((currentpage as PdfLoadedPage).Document as PdfLoadedDocument).CrossTable).GetObject((IPdfPrimitive) new PdfReference(objIndex, 0)) as PdfDictionary;
    IPdfPrimitive pdfPrimitive1 = (IPdfPrimitive) null;
    if (pdfDictionary1.ContainsKey("SMask"))
      pdfPrimitive1 = pdfDictionary1["SMask"];
    pdfDictionary1.Clear();
    PdfDictionary pdfDictionary2 = imageReference.Object as PdfDictionary;
    PdfStream pdfStream1 = imageReference.Object as PdfStream;
    foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair1 in pdfDictionary2.Items)
    {
      bool flag = false;
      if (keyValuePair1.Key.Value == "SMask" && pdfPrimitive1 != null)
      {
        IPdfPrimitive pdfPrimitive2 = PdfCrossTable.Dereference(pdfPrimitive1);
        IPdfPrimitive pdfPrimitive3 = PdfCrossTable.Dereference(keyValuePair1.Value);
        if (pdfPrimitive2 != null && pdfPrimitive2 is PdfDictionary && pdfPrimitive3 != null && pdfPrimitive3 is PdfDictionary)
        {
          PdfDictionary pdfDictionary3 = pdfPrimitive2 as PdfDictionary;
          if (!pdfDictionary3.isSkip)
          {
            pdfDictionary3.Clear();
            PdfDictionary pdfDictionary4 = pdfPrimitive3 as PdfDictionary;
            foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair2 in pdfDictionary4.Items)
              pdfDictionary3.Items.Add(keyValuePair2.Key, keyValuePair2.Value);
            pdfDictionary3.Modify();
            PdfStream pdfStream2 = pdfDictionary3 as PdfStream;
            if (this.isFlateCompress)
            {
              if (pdfDictionary3.ContainsKey("DecodeParms"))
              {
                if (pdfDictionary3["DecodeParms"] is PdfDictionary pdfDictionary5)
                {
                  pdfDictionary3.SetProperty("DecodeParms", (IPdfPrimitive) new PdfArray()
                  {
                    (IPdfPrimitive) new PdfDictionary(),
                    (IPdfPrimitive) pdfDictionary5
                  });
                  pdfStream2.Compress = true;
                  pdfStream2.isImageDualFilter = true;
                }
                else
                  pdfStream2.Compress = false;
              }
              else
                pdfStream2.Compress = false;
            }
            else
              pdfStream2.Compress = !pdfDictionary3.ContainsKey("Filter");
            pdfStream2.Data = (pdfDictionary4 as PdfStream).Data;
            pdfDictionary1.Items.Add(keyValuePair1.Key, pdfPrimitive1);
            flag = true;
          }
        }
      }
      if (!flag)
        pdfDictionary1.Items.Add(keyValuePair1.Key, keyValuePair1.Value);
    }
    pdfDictionary1.Modify();
    PdfStream pdfStream3 = pdfDictionary1 as PdfStream;
    if (this.isFlateCompress)
    {
      if (pdfDictionary1.ContainsKey("DecodeParms"))
      {
        if (pdfDictionary1["DecodeParms"] is PdfDictionary pdfDictionary6)
        {
          pdfDictionary1.SetProperty("DecodeParms", (IPdfPrimitive) new PdfArray()
          {
            (IPdfPrimitive) new PdfDictionary(),
            (IPdfPrimitive) pdfDictionary6
          });
          pdfStream3.Compress = true;
          pdfStream3.isImageDualFilter = true;
        }
        else
          pdfStream3.Compress = false;
      }
      else
        pdfStream3.Compress = false;
    }
    else
      pdfStream3.Compress = !pdfDictionary1.ContainsKey("Filter");
    pdfStream3.Data = pdfStream1.Data;
  }

  internal void ReplaceImageByName(string imgName, PdfImage image)
  {
    if (image is PdfMetafile)
      throw new NotSupportedException("Meta file image can't replaced");
    if (image == null)
      throw new NullReferenceException(nameof (image));
    this.m_modified = true;
    try
    {
      PdfImageInfo[] imagesInfo = this.ImagesInfo;
      image.Save();
      PdfReferenceHolder imageReference = new PdfReferenceHolder((IPdfWrapper) image);
      PdfResources resources = this.GetResources();
      if (!resources.ContainsKey("XObject"))
        return;
      PdfDictionary pdfDictionary1 = (PdfDictionary) null;
      if (resources["XObject"] is PdfDictionary)
        pdfDictionary1 = resources["XObject"] as PdfDictionary;
      else if ((object) (resources["XObject"] as PdfReferenceHolder) != null)
        pdfDictionary1 = (resources["XObject"] as PdfReferenceHolder).Object as PdfDictionary;
      if (pdfDictionary1 == null)
        return;
      PdfDictionary primitive = pdfDictionary1;
      PdfDictionary dictionary = new PdfDictionary();
      while (primitive != null && primitive != null)
      {
        foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair1 in primitive.Items)
        {
          bool flag = false;
          PdfDictionary pdfDictionary2 = (object) (keyValuePair1.Value as PdfReferenceHolder) == null ? keyValuePair1.Value as PdfDictionary : (keyValuePair1.Value as PdfReferenceHolder).Object as PdfDictionary;
          if (pdfDictionary2.ContainsKey("Subtype") && (pdfDictionary2["Subtype"] as PdfName).Value == "Image")
          {
            IPdfPrimitive pdfPrimitive = resources["XObject"];
            string str = keyValuePair1.Key.ToString();
            foreach (PdfImageInfo pdfImageInfo in imagesInfo)
            {
              str = this.StripSlashes(str);
              if (pdfImageInfo.Name == str && str == imgName)
              {
                flag = true;
                if (pdfImageInfo.Image != null)
                {
                  long objNum = (primitive[str] as PdfReferenceHolder).Reference.ObjNum;
                  int num1 = 0;
                  foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair2 in dictionary.Items)
                  {
                    if (num1 == dictionary.Count - 1)
                      primitive = dictionary[keyValuePair2.Key.Value] as PdfDictionary;
                    ++num1;
                  }
                  dictionary.Clear();
                  if ((keyValuePair1.Value as PdfReferenceHolder).Object is PdfStream)
                  {
                    PdfStream pdfStream = (keyValuePair1.Value as PdfReferenceHolder).Object as PdfStream;
                    switch (this)
                    {
                      case PdfPage _:
                        if ((this as PdfPage).Document.FileStructure.IncrementalUpdate)
                        {
                          pdfStream.Modify();
                          pdfStream.Clear();
                          break;
                        }
                        pdfStream.Clear();
                        break;
                      case PdfLoadedPage _:
                        if ((this as PdfLoadedPage).Document.FileStructure.IncrementalUpdate)
                        {
                          pdfStream.Modify();
                          pdfStream.Clear();
                          break;
                        }
                        pdfStream.Clear();
                        break;
                    }
                  }
                  primitive.Items.Remove(keyValuePair1.Key);
                  float height = (float) pdfImageInfo.Image.Height;
                  if ((double) (this.Size.Height - (pdfImageInfo.Bounds.Top + height)) >= 0.0)
                  {
                    primitive.Items.Add(keyValuePair1.Key, (IPdfPrimitive) imageReference);
                    primitive.Modify();
                    break;
                  }
                  primitive.Items.Add(keyValuePair1.Key, (IPdfPrimitive) imageReference);
                  primitive.Modify();
                  if (this is PdfLoadedPage)
                  {
                    int num2 = ((this as PdfLoadedPage).Document as PdfLoadedDocument).Pages.IndexOf(this);
                    int count = ((this as PdfLoadedPage).Document as PdfLoadedDocument).Pages.Count;
                    if (num2 < ((this as PdfLoadedPage).Document as PdfLoadedDocument).Pages.Count - 1)
                    {
                      if (!this.ReplacePaginatedImage(((this as PdfLoadedPage).Document as PdfLoadedDocument).Pages[num2 + 1] as PdfLoadedPage, keyValuePair1.Key.ToString(), imageReference, objNum) && num2 > 0)
                      {
                        this.ReplacePaginatedImage(((this as PdfLoadedPage).Document as PdfLoadedDocument).Pages[num2 - 1] as PdfLoadedPage, keyValuePair1.Key.ToString(), imageReference, objNum);
                        break;
                      }
                      break;
                    }
                    if (num2 > 0 && !this.ReplacePaginatedImage(((this as PdfLoadedPage).Document as PdfLoadedDocument).Pages[num2 - 1] as PdfLoadedPage, keyValuePair1.Key.ToString(), imageReference, objNum) && num2 < count - 1)
                    {
                      this.ReplacePaginatedImage(((this as PdfLoadedPage).Document as PdfLoadedDocument).Pages[num2 + 1] as PdfLoadedPage, keyValuePair1.Key.ToString(), imageReference, objNum);
                      break;
                    }
                    break;
                  }
                  break;
                }
                if ((keyValuePair1.Value as PdfReferenceHolder).Object is PdfStream)
                {
                  PdfStream pdfStream = (keyValuePair1.Value as PdfReferenceHolder).Object as PdfStream;
                  switch (this)
                  {
                    case PdfPage _:
                      if ((this as PdfPage).Document.FileStructure.IncrementalUpdate)
                      {
                        pdfStream.Modify();
                        pdfStream.Clear();
                        break;
                      }
                      pdfStream.Clear();
                      break;
                    case PdfLoadedPage _:
                      if ((this as PdfLoadedPage).Document.FileStructure.IncrementalUpdate)
                      {
                        pdfStream.Modify();
                        pdfStream.Clear();
                        break;
                      }
                      pdfStream.Clear();
                      break;
                  }
                }
                long objNum1 = (keyValuePair1.Value as PdfReferenceHolder).Reference.ObjNum;
                primitive.Items.Remove(keyValuePair1.Key);
                float height1 = pdfImageInfo.Bounds.Height;
                if ((double) (this.Size.Height - (pdfImageInfo.Bounds.Top + height1)) >= 0.0)
                {
                  primitive.Items.Add(keyValuePair1.Key, (IPdfPrimitive) imageReference);
                  primitive.Modify();
                  break;
                }
                primitive.Items.Add(keyValuePair1.Key, (IPdfPrimitive) imageReference);
                primitive.Modify();
                if (this is PdfLoadedPage)
                {
                  int num = ((this as PdfLoadedPage).Document as PdfLoadedDocument).Pages.IndexOf(this);
                  int count = ((this as PdfLoadedPage).Document as PdfLoadedDocument).Pages.Count;
                  if (num < ((this as PdfLoadedPage).Document as PdfLoadedDocument).Pages.Count - 1)
                  {
                    if (!this.ReplacePaginatedImage(((this as PdfLoadedPage).Document as PdfLoadedDocument).Pages[num + 1] as PdfLoadedPage, keyValuePair1.Key.ToString(), imageReference, objNum1) && num > 0)
                    {
                      this.ReplacePaginatedImage(((this as PdfLoadedPage).Document as PdfLoadedDocument).Pages[num - 1] as PdfLoadedPage, keyValuePair1.Key.ToString(), imageReference, objNum1);
                      break;
                    }
                    break;
                  }
                  if (num > 0 && !this.ReplacePaginatedImage(((this as PdfLoadedPage).Document as PdfLoadedDocument).Pages[num - 1] as PdfLoadedPage, keyValuePair1.Key.ToString(), imageReference, objNum1) && num < count - 1)
                  {
                    this.ReplacePaginatedImage(((this as PdfLoadedPage).Document as PdfLoadedDocument).Pages[num + 1] as PdfLoadedPage, keyValuePair1.Key.ToString(), imageReference, objNum1);
                    break;
                  }
                  break;
                }
                break;
              }
            }
          }
          if (pdfDictionary2.ContainsKey("Resources"))
          {
            PdfDictionary pdfDictionary3 = (object) (pdfDictionary2["Resources"] as PdfReferenceHolder) == null ? pdfDictionary2["Resources"] as PdfDictionary : (pdfDictionary2["Resources"] as PdfReferenceHolder).Object as PdfDictionary;
            if (pdfDictionary3.ContainsKey("XObject"))
            {
              primitive = pdfDictionary3["XObject"] as PdfDictionary;
              PdfDictionary.SetProperty(dictionary, keyValuePair1.Key.Value, (IPdfPrimitive) primitive);
            }
          }
          else if (flag)
          {
            primitive = (PdfDictionary) null;
            break;
          }
        }
      }
    }
    catch (Exception ex)
    {
      if (ex is ArgumentException)
        throw ex;
    }
  }

  private bool ReplacePaginatedImage(
    PdfLoadedPage page,
    string name,
    PdfReferenceHolder imageReference,
    long objIndex)
  {
    this.m_modified = true;
    PdfResources resources = page.GetResources();
    if (resources.ContainsKey("XObject") && resources["XObject"] is PdfDictionary)
    {
      PdfDictionary pdfDictionary = resources["XObject"] as PdfDictionary;
      foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in pdfDictionary.Items)
      {
        if ((keyValuePair.Value as PdfReferenceHolder).Reference.ObjNum == objIndex)
        {
          pdfDictionary.Items.Remove(keyValuePair.Key);
          pdfDictionary.Items.Add(keyValuePair.Key, (IPdfPrimitive) imageReference);
          pdfDictionary.Modify();
          return true;
        }
      }
    }
    return false;
  }

  public PdfTemplate CreateTemplate() => this.GetContent();

  internal System.Drawing.Graphics PageGraphics => this.m_graphics;

  private PointF CurrentLocation
  {
    get => this.m_currentLocation;
    set => this.m_currentLocation = value;
  }

  internal PdfUnitConvertor UnitConvertor
  {
    get
    {
      if (this.m_unitConvertor == null)
        this.m_unitConvertor = new PdfUnitConvertor();
      return this.m_unitConvertor;
    }
  }

  public bool IsColored
  {
    get
    {
      CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
      Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        this.Layers.CombineContent((Stream) memoryStream);
        memoryStream.Position = 0L;
        this.m_recordCollection = new ContentParser(memoryStream.ToArray()).ReadContent();
      }
      this.m_pageResources = PageResourceLoader.Instance.GetPageResources(this);
      bool isColored = this.CheckColorContent(this.m_recordCollection, this.m_pageResources);
      Thread.CurrentThread.CurrentCulture = currentCulture;
      return isColored;
    }
  }

  public bool IsBlank
  {
    get
    {
      switch (this)
      {
        case PdfLoadedPage _:
          int count1 = this.Annotations.Count;
          int contentLength1 = this.GetContentLength();
          int num1 = 0;
          PdfResources resources1 = this.GetResources();
          if (resources1.ContainsKey("XObject") && (resources1["XObject"] is PdfDictionary || (object) (resources1["XObject"] as PdfReferenceHolder) != null))
          {
            if (!(PdfCrossTable.Dereference(resources1["XObject"]) is PdfDictionary pdfDictionary))
              PdfCrossTable.Dereference((IPdfPrimitive) (PdfCrossTable.Dereference(resources1["XObject"]) as PdfReferenceHolder));
            if (pdfDictionary != null)
              num1 = pdfDictionary.Count;
          }
          return count1 <= 0 && contentLength1 <= 0 && num1 <= 0;
        case PdfPage _:
          int count2 = (this as PdfPage).Annotations.Count;
          int contentLength2 = this.GetContentLength();
          int num2 = 0;
          PdfResources resources2 = this.GetResources();
          if (resources2.ContainsKey("XObject") && (resources2["XObject"] is PdfDictionary || (object) (resources2["XObject"] as PdfReferenceHolder) != null))
          {
            if (!(PdfCrossTable.Dereference(resources2["XObject"]) is PdfDictionary pdfDictionary))
              PdfCrossTable.Dereference((IPdfPrimitive) (PdfCrossTable.Dereference(resources2["XObject"]) as PdfReferenceHolder));
            if (pdfDictionary != null)
              num2 = pdfDictionary.Count;
          }
          return count2 <= 0 && contentLength2 <= 0 && num2 <= 0;
        default:
          return false;
      }
    }
  }

  private int GetContentLength()
  {
    int contentLength = 0;
    if (this.Dictionary.ContainsKey("Contents"))
    {
      if (this.Dictionary["Contents"] is PdfArray && this.Dictionary["Contents"] is PdfArray pdfArray)
        contentLength = pdfArray.Count;
      if ((object) (this.Dictionary["Contents"] as PdfReferenceHolder) != null)
      {
        PdfReferenceHolder pdfReferenceHolder = PdfCrossTable.Dereference(this.Dictionary["Contents"]) as PdfReferenceHolder;
        if (pdfReferenceHolder != (PdfReferenceHolder) null && PdfCrossTable.Dereference((PdfCrossTable.Dereference((IPdfPrimitive) pdfReferenceHolder) as PdfDictionary)["Length"]) is PdfNumber pdfNumber)
          contentLength = pdfNumber.IntValue;
      }
    }
    return contentLength;
  }

  public string ExtractText()
  {
    CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
    using (MemoryStream memoryStream = new MemoryStream())
    {
      this.Layers.CombineContent((Stream) memoryStream);
      memoryStream.Position = 0L;
      ContentParser contentParser = new ContentParser(memoryStream.ToArray());
      contentParser.IsTextExtractionProcess = true;
      this.m_recordCollection = contentParser.ReadContent();
      contentParser.IsTextExtractionProcess = false;
    }
    this.m_pageResources = PageResourceLoader.Instance.GetPageResources(this);
    this.resultantText = (string) null;
    this.RenderText(this.m_recordCollection, this.m_pageResources);
    this.m_recordCollection.RecordCollection.Clear();
    this.m_recordCollection = (PdfRecordCollection) null;
    this.m_pageResources.Resources.Clear();
    if (this.m_pageResources.fontCollection != null)
      this.m_pageResources.fontCollection.Clear();
    this.m_pageResources = (PdfPageResources) null;
    if (this.resultantText != null)
      this.resultantText = this.SkipEscapeSequence(this.resultantText);
    Thread.CurrentThread.CurrentCulture = currentCulture;
    return this.resultantText;
  }

  public string ExtractText(bool IsLayout)
  {
    CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
    this.m_isLayoutTextExtraction = IsLayout;
    this.resultantText = (string) null;
    if (!IsLayout)
    {
      this.resultantText = this.ExtractText();
    }
    else
    {
      SizeF size = this.Size;
      float pixels1;
      float pixels2;
      if (this.Rotation == PdfPageRotateAngle.RotateAngle90 || this.Rotation == PdfPageRotateAngle.RotateAngle270)
      {
        RectangleF cropBox = (this as PdfLoadedPage).CropBox;
        if ((double) (this as PdfLoadedPage).CropBox.Width > 0.0 && (double) (this as PdfLoadedPage).CropBox.Height > 0.0 && (this as PdfLoadedPage).CropBox != (this as PdfLoadedPage).MediaBox)
        {
          pixels1 = this.UnitConvertor.ConvertToPixels((this as PdfLoadedPage).CropBox.Height - (this as PdfLoadedPage).CropBox.Y, PdfGraphicsUnit.Point);
          pixels2 = this.UnitConvertor.ConvertToPixels((this as PdfLoadedPage).CropBox.Width - (this as PdfLoadedPage).CropBox.X, PdfGraphicsUnit.Point);
        }
        else
        {
          pixels1 = this.UnitConvertor.ConvertToPixels(size.Height, PdfGraphicsUnit.Point);
          pixels2 = this.UnitConvertor.ConvertToPixels(size.Width, PdfGraphicsUnit.Point);
        }
      }
      else
      {
        RectangleF cropBox = (this as PdfLoadedPage).CropBox;
        if ((double) (this as PdfLoadedPage).CropBox.Width > 0.0 && (double) (this as PdfLoadedPage).CropBox.Height > 0.0 && (this as PdfLoadedPage).CropBox != (this as PdfLoadedPage).MediaBox)
        {
          pixels1 = this.UnitConvertor.ConvertToPixels((this as PdfLoadedPage).CropBox.Width - (this as PdfLoadedPage).CropBox.X, PdfGraphicsUnit.Point);
          pixels2 = this.UnitConvertor.ConvertToPixels((this as PdfLoadedPage).CropBox.Height - (this as PdfLoadedPage).CropBox.Y, PdfGraphicsUnit.Point);
        }
        else
        {
          pixels1 = this.UnitConvertor.ConvertToPixels(size.Width, PdfGraphicsUnit.Point);
          pixels2 = this.UnitConvertor.ConvertToPixels(size.Height, PdfGraphicsUnit.Point);
        }
      }
      Image image;
      if (this.Rotation == PdfPageRotateAngle.RotateAngle90 || this.Rotation == PdfPageRotateAngle.RotateAngle270)
      {
        RectangleF cropBox = (this as PdfLoadedPage).CropBox;
        image = (double) (this as PdfLoadedPage).CropBox.Width <= 0.0 || (double) (this as PdfLoadedPage).CropBox.Height <= 0.0 || !((this as PdfLoadedPage).CropBox != (this as PdfLoadedPage).MediaBox) ? (Image) new Bitmap((int) pixels2 / 100, (int) pixels1 / 100) : (Image) new Bitmap((int) (((double) (this as PdfLoadedPage).CropBox.Width - (double) (this as PdfLoadedPage).CropBox.X) / 100.0), (int) (((double) (this as PdfLoadedPage).CropBox.Height - (double) (this as PdfLoadedPage).CropBox.Y) / 100.0));
      }
      else
        image = (double) (this as PdfLoadedPage).CropBox.Width == 0.0 || (double) (this as PdfLoadedPage).CropBox.Height == 0.0 || !((this as PdfLoadedPage).CropBox != (this as PdfLoadedPage).MediaBox) ? (Image) new Bitmap((int) pixels1 / 100, (int) ((double) pixels2 - 4.0) / 100) : (Image) new Bitmap((int) (((double) (this as PdfLoadedPage).CropBox.Width - (double) (this as PdfLoadedPage).CropBox.X) / 100.0), (int) (((double) (this as PdfLoadedPage).CropBox.Height - (double) (this as PdfLoadedPage).CropBox.Y - 4.0) / 100.0));
      System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(image);
      this.m_graphics = graphics;
      System.Drawing.Drawing2D.Matrix transform = this.m_graphics.Transform;
      this.m_transformations = new TransformationStack(new Syncfusion.PdfViewer.Base.Matrix(4.0 / 3.0 * ((double) this.m_graphics.DpiX / 96.0) * (double) transform.Elements[0], 0.0, 0.0, -4.0 / 3.0 * ((double) this.m_graphics.DpiX / 96.0) * (double) transform.Elements[3], 0.0, (double) this.m_graphics.VisibleClipBounds.Bottom * (double) transform.Elements[3]));
      this.m_graphics.PageUnit = GraphicsUnit.Point;
      this.m_graphics.TranslateTransform(0.0f, this.m_graphics.VisibleClipBounds.Bottom);
      using (MemoryStream memoryStream = new MemoryStream())
      {
        this.Layers.CombineContent((Stream) memoryStream);
        memoryStream.Position = 0L;
        ContentParser contentParser = new ContentParser(memoryStream.ToArray());
        contentParser.IsTextExtractionProcess = true;
        this.m_recordCollection = contentParser.ReadContent();
        contentParser.IsTextExtractionProcess = false;
      }
      this.m_pageResources = PageResourceLoader.Instance.GetPageResources(this);
      this.RenderTextAsLayout(this.m_recordCollection, this.m_pageResources);
      this.m_recordCollection.RecordCollection.Clear();
      this.m_recordCollection = (PdfRecordCollection) null;
      this.m_pageResources.Resources.Clear();
      if (this.m_pageResources.fontCollection != null)
        this.m_pageResources.fontCollection.Clear();
      this.m_pageResources = (PdfPageResources) null;
      graphics.Dispose();
      this.m_graphics.Dispose();
      this.resultantText += "\r\n\r\n";
      if (this.resultantText != null)
        this.resultantText = this.SkipEscapeSequence(this.resultantText);
    }
    Thread.CurrentThread.CurrentCulture = currentCulture;
    return this.resultantText;
  }

  private string GetFontName(Syncfusion.PdfViewer.Base.Glyph glyph)
  {
    string fontName = "";
    if (glyph != null)
    {
      fontName = glyph.FontFamily;
      if (!string.IsNullOrEmpty(glyph.EmbededFontFamily) && glyph.FontFamily != glyph.EmbededFontFamily)
        fontName = glyph.EmbededFontFamily;
    }
    return fontName;
  }

  private double GetFontSize(Syncfusion.PdfViewer.Base.Glyph glyph)
  {
    double fontSize = 0.0;
    if (glyph != null)
    {
      fontSize = glyph.FontSize;
      if (!double.IsNaN(glyph.MatrixFontSize) && glyph.MatrixFontSize > 0.0 && glyph.FontSize != glyph.MatrixFontSize)
        fontSize = glyph.MatrixFontSize;
    }
    return fontSize;
  }

  public string ExtractText(out List<TextData> textDataCollection)
  {
    CultureInfo currentCulture1 = Thread.CurrentThread.CurrentCulture;
    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
    textDataCollection = new List<TextData>();
    using (MemoryStream memoryStream = new MemoryStream())
    {
      this.Layers.CombineContent((Stream) memoryStream);
      memoryStream.Position = 0L;
      ContentParser contentParser = new ContentParser(memoryStream.ToArray());
      contentParser.IsTextExtractionProcess = true;
      this.m_recordCollection = contentParser.ReadContent();
      contentParser.IsTextExtractionProcess = false;
    }
    this.m_pageResources = PageResourceLoader.Instance.GetPageResources(this);
    this.resultantText = (string) null;
    this.isExtractWithFormat = true;
    this.RenderText(this.m_recordCollection, this.m_pageResources);
    this.isExtractWithFormat = false;
    Page page = new Page(this);
    page.Initialize(this, true);
    List<Syncfusion.PdfViewer.Base.Glyph> glyphList = new List<Syncfusion.PdfViewer.Base.Glyph>();
    double height1 = (double) page.Height;
    double width1 = (double) page.Width;
    if (page.Rotation == 90.0 || page.Rotation == 270.0)
    {
      width1 = (double) page.Width;
      height1 = (double) page.Height;
    }
    using (Image image = (Image) new Bitmap((int) width1, (int) height1))
    {
      using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image))
      {
        g.TranslateTransform(0.0f, 0.0f);
        g.FillRectangle(Brushes.White, new Rectangle(0, 0, (int) width1, (int) height1));
        if (this.m_recordCollection == null)
          page.Initialize(this, true);
        ImageRenderer imageRenderer = new ImageRenderer(page.RecordCollection, page.Resources, g, true, new Syncfusion.PdfViewer.Base.DeviceCMYK(), (float) height1);
        CultureInfo currentCulture2 = Thread.CurrentThread.CurrentCulture;
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        imageRenderer.IsExtractTextData = true;
        imageRenderer.pageRotation = (float) page.Rotation;
        imageRenderer.RenderAsImage();
        imageRenderer.IsExtractTextData = false;
        if (this.pageWords.Count > 0 && imageRenderer.imageRenderGlyphList.Count > 0)
        {
          int num1 = 0;
          int index1 = 1;
          List<Syncfusion.PdfViewer.Base.Glyph> imageRenderGlyphList = imageRenderer.imageRenderGlyphList;
          for (int index2 = 0; index2 < this.pageWords.Count; ++index2)
          {
            string pageWord = this.pageWords[index2];
            if (!string.IsNullOrEmpty(pageWord))
            {
              RectangleF bounds = new RectangleF();
              Color color = new Color();
              for (int index3 = num1; index3 < imageRenderGlyphList.Count; ++index3)
              {
                string toUnicode = imageRenderGlyphList[index3].ToUnicode;
                if (!string.IsNullOrEmpty(pageWord) && toUnicode == pageWord[0].ToString())
                {
                  string fontName = this.GetFontName(imageRenderGlyphList[index3]);
                  FontStyle fontStyle = imageRenderGlyphList[index3].FontStyle;
                  double fontSize = this.GetFontSize(imageRenderGlyphList[index3]);
                  Color fontColor = imageRenderGlyphList[index3].Stroke != null ? (imageRenderGlyphList[index3].Stroke as SolidBrush).Color : (imageRenderGlyphList[index3 - 1].Stroke as SolidBrush).Color;
                  Rect boundingRect = imageRenderGlyphList[index3].BoundingRect;
                  double left = boundingRect.Left;
                  double top1 = imageRenderGlyphList[index3].BoundingRect.Top;
                  double right1 = imageRenderGlyphList[index3].BoundingRect.Right;
                  double bottom1 = imageRenderGlyphList[index3].BoundingRect.Bottom;
                  bool flag1 = false;
                  bool flag2 = false;
                  string str1 = (string) null + toUnicode;
                  if (pageWord.Length == str1.Length)
                  {
                    bounds = new RectangleF((float) left, (float) top1, (float) (right1 - left), (float) (bottom1 - top1));
                    textDataCollection.Add(new TextData(pageWord, fontName, fontStyle, fontSize, fontColor, bounds));
                    num1 = index3 + 1;
                    break;
                  }
                  int index4 = index3 == 0 ? index3 + 1 : index3;
                  while (index4 < imageRenderGlyphList.Count)
                  {
                    if (index1 == 0)
                    {
                      index1 = 1;
                      ++index4;
                    }
                    if (pageWord.Length > index1 && imageRenderGlyphList.Count > index4)
                    {
                      string str2 = pageWord[index1].ToString();
                      if ((int) str2[0] == this.m_nonBreakingSpaceCharValue)
                        str2 = " ";
                      if (imageRenderGlyphList[index4].ToUnicode == str2)
                      {
                        if (left > imageRenderGlyphList[index4].BoundingRect.Left)
                        {
                          boundingRect = imageRenderGlyphList[index4].BoundingRect;
                          left = boundingRect.Left;
                        }
                        double num2 = top1;
                        boundingRect = imageRenderGlyphList[index4].BoundingRect;
                        double top2 = boundingRect.Top;
                        if (num2 > top2)
                        {
                          boundingRect = imageRenderGlyphList[index4].BoundingRect;
                          top1 = boundingRect.Top;
                        }
                        double num3 = right1;
                        boundingRect = imageRenderGlyphList[index4].BoundingRect;
                        double right2 = boundingRect.Right;
                        if (num3 < right2)
                        {
                          boundingRect = imageRenderGlyphList[index4].BoundingRect;
                          right1 = boundingRect.Right;
                        }
                        double num4 = bottom1;
                        boundingRect = imageRenderGlyphList[index4].BoundingRect;
                        double bottom2 = boundingRect.Bottom;
                        if (num4 < bottom2)
                        {
                          boundingRect = imageRenderGlyphList[index4].BoundingRect;
                          bottom1 = boundingRect.Bottom;
                        }
                        fontName = this.GetFontName(imageRenderGlyphList[index4]);
                        fontStyle = imageRenderGlyphList[index4].FontStyle;
                        fontSize = this.GetFontSize(imageRenderGlyphList[index4]);
                        fontColor = imageRenderGlyphList[index4].Stroke != null ? (imageRenderGlyphList[index4].Stroke as SolidBrush).Color : (imageRenderGlyphList[index4 - 1].Stroke as SolidBrush).Color;
                        str1 += imageRenderGlyphList[index4].ToUnicode;
                      }
                      else
                      {
                        flag2 = true;
                        num1 += pageWord.Length;
                        index1 = 0;
                        break;
                      }
                    }
                    if (pageWord.Length == str1.Length && imageRenderGlyphList.Count > index4)
                    {
                      switch ((int) page.Rotation)
                      {
                        case 90:
                          if (imageRenderGlyphList[index4].IsRotated)
                          {
                            double y = left - (imageRenderGlyphList[index4].BoundingRect._width - imageRenderGlyphList[index4].BoundingRect._height / imageRenderGlyphList[index4].BoundingRect._width);
                            bounds = new RectangleF((float) ((double) this.UnitConvertor.ConvertFromPixels((float) page.ActualHeight, PdfGraphicsUnit.Point) - bottom1 + imageRenderGlyphList[index4].BoundingRect._width / 2.0), (float) y, (float) (bottom1 - top1), (float) imageRenderGlyphList[index4].BoundingRect._width);
                            break;
                          }
                          if (!imageRenderGlyphList[index4].IsRotated)
                          {
                            double y = left;
                            double num5 = (double) this.UnitConvertor.ConvertFromPixels((float) page.ActualHeight, PdfGraphicsUnit.Point) - top1;
                            boundingRect = imageRenderGlyphList[index4].BoundingRect;
                            double height2 = boundingRect.Height;
                            bounds = new RectangleF((float) (num5 - height2), (float) y, (float) imageRenderGlyphList[index4].BoundingRect._height, (float) (right1 - left));
                            break;
                          }
                          break;
                        case 180:
                          if (imageRenderGlyphList[index4].IsRotated)
                          {
                            float num6 = this.UnitConvertor.ConvertFromPixels((float) page.ActualHeight, PdfGraphicsUnit.Point);
                            float num7 = this.UnitConvertor.ConvertFromPixels((float) page.ActualWidth, PdfGraphicsUnit.Point);
                            double num8 = (double) num6 - top1;
                            boundingRect = imageRenderGlyphList[index4].BoundingRect;
                            double height3 = boundingRect.Height;
                            double y = num8 - height3;
                            bounds = new RectangleF(num7 - (float) left, (float) y, -(float) (right1 - left), (float) imageRenderGlyphList[index4].BoundingRect._height);
                            break;
                          }
                          if (!imageRenderGlyphList[index4].IsRotated)
                          {
                            float num9 = this.UnitConvertor.ConvertFromPixels((float) page.ActualHeight, PdfGraphicsUnit.Point);
                            float num10 = this.UnitConvertor.ConvertFromPixels((float) page.ActualWidth, PdfGraphicsUnit.Point);
                            double num11 = (double) num9 - top1;
                            boundingRect = imageRenderGlyphList[index4].BoundingRect;
                            double height4 = boundingRect.Height;
                            boundingRect = imageRenderGlyphList[index4].BoundingRect;
                            double width2 = boundingRect.Width;
                            double num12 = height4 + width2;
                            double y = num11 - num12;
                            double num13 = (double) num10 - left;
                            boundingRect = imageRenderGlyphList[index4].BoundingRect;
                            double num14 = boundingRect.Height / 2.0;
                            bounds = new RectangleF((float) (num13 + num14), (float) y, -(float) (right1 - left), (float) imageRenderGlyphList[index4].BoundingRect._height);
                            break;
                          }
                          break;
                        case 270:
                          if (imageRenderGlyphList[index4].IsRotated && imageRenderGlyphList[index4].RotationAngle == 90)
                          {
                            double x = top1;
                            double num15 = (double) this.UnitConvertor.ConvertFromPixels((float) page.ActualWidth, PdfGraphicsUnit.Point) - left;
                            boundingRect = imageRenderGlyphList[index4].BoundingRect;
                            double height5 = boundingRect.Height;
                            double y = num15 - height5;
                            bounds = new RectangleF((float) x, (float) y, (float) (bottom1 - top1), (float) imageRenderGlyphList[index4].BoundingRect._height);
                            break;
                          }
                          if (imageRenderGlyphList[index4].IsRotated)
                          {
                            float num16 = this.UnitConvertor.ConvertFromPixels((float) page.ActualWidth, PdfGraphicsUnit.Point);
                            bounds = new RectangleF((float) (top1 - imageRenderGlyphList[index4].BoundingRect._width / imageRenderGlyphList[index4].BoundingRect._height), (float) ((double) num16 - right1 + imageRenderGlyphList[index4].BoundingRect._height / 4.0), (float) (bottom1 - top1), (float) imageRenderGlyphList[index4].BoundingRect._width);
                            break;
                          }
                          if (!imageRenderGlyphList[index4].IsRotated)
                          {
                            bounds = new RectangleF((float) top1, this.UnitConvertor.ConvertFromPixels((float) page.ActualWidth, PdfGraphicsUnit.Point) - (float) left, (float) imageRenderGlyphList[index4].BoundingRect._height, (float) -(right1 - left));
                            break;
                          }
                          break;
                        default:
                          bounds = new RectangleF((float) left, (float) top1, (float) (right1 - left), (float) (bottom1 - top1));
                          break;
                      }
                      textDataCollection.Add(new TextData(pageWord, fontName, fontStyle, fontSize, fontColor, bounds));
                      flag1 = true;
                      num1 += pageWord.Length;
                      index1 = 0;
                      break;
                    }
                    ++index4;
                    ++index1;
                  }
                  if (flag1 || flag2)
                    break;
                }
              }
            }
          }
        }
        Thread.CurrentThread.CurrentCulture = currentCulture2;
      }
    }
    return this.resultantText;
  }

  public string ExtractText(out TextLines textLines)
  {
    StringBuilder stringBuilder = new StringBuilder();
    TextLines lineCollection = new TextLines();
    Syncfusion.PdfViewer.Base.DeviceCMYK deviceCmyk = new Syncfusion.PdfViewer.Base.DeviceCMYK();
    TextLine textLine1 = (TextLine) null;
    using (MemoryStream memoryStream = new MemoryStream())
    {
      this.Layers.CombineContent((Stream) memoryStream);
      memoryStream.Position = 0L;
      ContentParser contentParser = new ContentParser(memoryStream.ToArray());
      contentParser.IsTextExtractionProcess = true;
      this.m_recordCollection = contentParser.ReadContent();
      contentParser.IsTextExtractionProcess = false;
    }
    this.m_pageResources = PageResourceLoader.Instance.GetPageResources(this);
    this.resultantText = (string) null;
    this.isExtractWithFormat = true;
    this.RenderText(this.m_recordCollection, this.m_pageResources);
    this.isExtractWithFormat = false;
    Page page = new Page(this);
    page.Initialize(this, true);
    double height1 = (double) page.Height;
    double width1 = (double) page.Width;
    if (page.Rotation == 90.0 || page.Rotation == 270.0)
    {
      width1 = (double) page.Width;
      height1 = (double) page.Height;
    }
    using (Image image = (Image) new Bitmap((int) width1, (int) height1))
    {
      using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image))
      {
        g.TranslateTransform(0.0f, 0.0f);
        g.FillRectangle(Brushes.White, new Rectangle(0, 0, (int) width1, (int) height1));
        if (this.m_recordCollection == null)
          page.Initialize(this, true);
        ImageRenderer renderer = new ImageRenderer(page.RecordCollection, page.Resources, g, true, new Syncfusion.PdfViewer.Base.DeviceCMYK(), (float) height1);
        CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        renderer.pageRotation = (float) page.Rotation;
        renderer.isExtractLineCollection = true;
        renderer.RenderAsImage();
        renderer.isExtractLineCollection = false;
        string empty1 = string.Empty;
        int index1 = 0;
        double width2 = 0.0;
        double height2 = 0.0;
        int num1 = 0;
        double currentBottomPosition = 0.0;
        double previousBottomPosition = 0.0;
        double previousYPosition = 0.0;
        double previousXPosition = 0.0;
        double currentXPosition = 0.0;
        string empty2 = string.Empty;
        int lineStartIndex = 0;
        int glyphIndex = 0;
        if (empty2 == "")
        {
          foreach (Syncfusion.PdfViewer.Base.Glyph imageRenderGlyph in renderer.imageRenderGlyphList)
            empty2 += imageRenderGlyph.ToUnicode;
        }
        if (renderer.imageRenderGlyphList.Count > 0)
          textLine1 = new TextLine();
        for (int index2 = 0; index2 < renderer.extractTextElement.Count; ++index2)
        {
          if (index1 < renderer.imageRenderGlyphList.Count)
          {
            Rect boundingRect;
            double currentYPosition;
            if (page.Rotation == 270.0)
            {
              boundingRect = renderer.imageRenderGlyphList[index1].BoundingRect;
              currentYPosition = boundingRect.X;
              currentBottomPosition = renderer.imageRenderGlyphList[index1].BoundingRect.Right;
              currentXPosition = renderer.imageRenderGlyphList[index1].BoundingRect.Y;
              if (index2 == 0)
              {
                previousBottomPosition = renderer.imageRenderGlyphList[index1].BoundingRect.Right;
                previousYPosition = (double) (int) renderer.imageRenderGlyphList[index1].BoundingRect.X;
                previousXPosition = renderer.imageRenderGlyphList[index1].BoundingRect.Y;
              }
            }
            else if (page.Rotation == 0.0)
            {
              currentYPosition = renderer.imageRenderGlyphList[index1].BoundingRect.Y;
              currentBottomPosition = renderer.imageRenderGlyphList[index1].BoundingRect.Bottom;
              currentXPosition = renderer.imageRenderGlyphList[index1].BoundingRect.X;
              if (index2 == 0)
              {
                previousBottomPosition = renderer.imageRenderGlyphList[index1].BoundingRect.Bottom;
                previousYPosition = (double) (int) renderer.imageRenderGlyphList[index1].BoundingRect.Y;
                previousXPosition = renderer.imageRenderGlyphList[index1].BoundingRect.X;
              }
            }
            else
              currentYPosition = renderer.imageRenderGlyphList[index1].BoundingRect.Y;
            if ((index1 != 0 && (int) currentYPosition != num1 && renderer.extractTextElement[index2].renderedText != string.Empty && renderer.extractTextElement[index2].renderedText != "" && renderer.extractTextElement[index2].renderedText != " " || index1 == renderer.imageRenderGlyphList.Count - 1) && this.IsNotInSameLine(page.Rotation, currentYPosition, currentBottomPosition, currentXPosition, previousYPosition, previousBottomPosition, previousXPosition, index2))
            {
              num1 = (int) currentYPosition;
              if (textLine1.WordCollection.Count > 0)
              {
                string str = this.AddLineCollection(textLine1, lineCollection, renderer, lineStartIndex, glyphIndex);
                stringBuilder.AppendLine(str);
              }
              lineStartIndex = index1;
              textLine1 = new TextLine();
              width2 = 0.0;
              height2 = 0.0;
            }
            TextElement textElement = renderer.extractTextElement[index2];
            string[] strArray = textElement.renderedText.Split(' ');
            textElement.Text = " ";
            bool flag = false;
            for (int index3 = 0; index3 < strArray.Length; ++index3)
            {
              if (empty2.Contains(strArray[index3]) && strArray[index3].Length != 0)
              {
                TextWord textWord = new TextWord();
                for (int index4 = index1; index4 < index1 + strArray[index3].Length; ++index4)
                {
                  TextGlyph textGlyph = new TextGlyph();
                  textGlyph.FontName = textElement.FontName;
                  textGlyph.FontSize = textElement.FontSize;
                  textGlyph.FontStyle = textElement.FontStyle;
                  if (renderer.imageRenderGlyphList[index4].ToUnicode != "")
                    textGlyph.Text = Convert.ToChar(renderer.imageRenderGlyphList[index4].ToUnicode);
                  textGlyph.Bounds = new RectangleF((float) renderer.imageRenderGlyphList[index4].BoundingRect.X, (float) renderer.imageRenderGlyphList[index4].BoundingRect.Y, (float) renderer.imageRenderGlyphList[index4].BoundingRect.Width, (float) renderer.imageRenderGlyphList[index4].BoundingRect.Height);
                  textWord.Glyphs.Add(textGlyph);
                  if (renderer.imageRenderGlyphList[index4].IsRotated)
                    flag = true;
                  if (flag && (page.Rotation == 270.0 || page.Rotation == 90.0))
                    height2 += renderer.imageRenderGlyphList[index4].BoundingRect.Height;
                  else
                    width2 += renderer.imageRenderGlyphList[index4].BoundingRect.Width;
                }
                boundingRect = renderer.imageRenderGlyphList[index1].BoundingRect;
                double x1 = boundingRect.X;
                boundingRect = renderer.imageRenderGlyphList[index1].BoundingRect;
                double y = boundingRect.Y;
                if (flag && (page.Rotation == 270.0 || page.Rotation == 90.0))
                {
                  boundingRect = renderer.imageRenderGlyphList[index1].BoundingRect;
                  width2 = boundingRect.Width;
                }
                else
                {
                  boundingRect = renderer.imageRenderGlyphList[index1].BoundingRect;
                  height2 = boundingRect.Height;
                }
                double num2 = x1;
                boundingRect = renderer.imageRenderGlyphList[index1 + strArray[index3].Length - 1].BoundingRect;
                double x2 = boundingRect.X;
                if (num2 > x2)
                {
                  double num3 = x1;
                  boundingRect = renderer.imageRenderGlyphList[index1 + strArray[index3].Length - 1].BoundingRect;
                  double x3 = boundingRect.X;
                  double num4 = num3 - x3;
                  boundingRect = renderer.imageRenderGlyphList[index1 + strArray[index3].Length - 1].BoundingRect;
                  double width3 = boundingRect.Width;
                  width2 = num4 + width3;
                }
                else if (!flag || page.Rotation != 270.0 && page.Rotation != 90.0)
                {
                  boundingRect = renderer.imageRenderGlyphList[index1 + strArray[index3].Length - 1].BoundingRect;
                  double x4 = boundingRect.X;
                  boundingRect = renderer.imageRenderGlyphList[index1 + strArray[index3].Length - 1].BoundingRect;
                  double width4 = boundingRect.Width;
                  width2 = x4 + width4;
                }
                if (page.Rotation == 270.0 && index1 != renderer.imageRenderGlyphList.Count)
                {
                  boundingRect = renderer.imageRenderGlyphList[index1].BoundingRect;
                  previousBottomPosition = boundingRect.Right;
                  boundingRect = renderer.imageRenderGlyphList[index1].BoundingRect;
                  previousYPosition = (double) (int) boundingRect.X;
                  previousXPosition = currentXPosition;
                }
                else if (page.Rotation == 0.0 && index1 != renderer.imageRenderGlyphList.Count)
                {
                  previousBottomPosition = currentBottomPosition;
                  previousYPosition = currentYPosition;
                  previousXPosition = currentXPosition;
                }
                index1 += strArray[index3].Length;
                textWord.Bounds = !flag || page.Rotation != 270.0 && page.Rotation != 90.0 ? new RectangleF((float) x1, (float) y, (float) (width2 - x1), (float) height2) : new RectangleF((float) x1, (float) y, (float) width2, (float) height2);
                textWord.Text = strArray[index3];
                textWord.FontName = textElement.FontName;
                textWord.FontSize = textElement.FontSize;
                textWord.FontStyle = textElement.FontStyle;
                textLine1.WordCollection.Add(textWord);
                width2 = 0.0;
                height2 = 0.0;
                glyphIndex = index1;
              }
              textElement.Text = strArray[index3];
              if (textElement.Text.Length > 0)
              {
                if (index3 < strArray.Length - 1)
                  ++index1;
                if (index3 < strArray.Length - 1 && (index1 <= renderer.imageRenderGlyphList.Count - 1 ? (renderer.imageRenderGlyphList[index1].ToUnicode == " " ? 1 : 0) : 0) != 0)
                  ++index1;
              }
              else if (index1 <= renderer.imageRenderGlyphList.Count - 1 && index3 != strArray.Length - 1 && renderer.imageRenderGlyphList[index1].ToUnicode == " ")
                ++index1;
            }
            if ((index1 != 0 && (int) currentYPosition != num1 && renderer.extractTextElement[index2].renderedText != string.Empty && renderer.extractTextElement[index2].renderedText != "" && renderer.extractTextElement[index2].renderedText != " " || index1 == renderer.imageRenderGlyphList.Count - 1) && this.IsNotInSameLine(page.Rotation, currentYPosition, currentBottomPosition, currentXPosition, previousYPosition, previousBottomPosition, previousXPosition, index2) && renderer.extractTextElement.Count > 0 && index2 == 0)
            {
              num1 = (int) currentYPosition;
              if (textLine1.WordCollection.Count > 0)
              {
                string str = this.AddLineCollection(textLine1, lineCollection, renderer, lineStartIndex, glyphIndex);
                stringBuilder.AppendLine(str);
              }
              lineStartIndex = index1;
              textLine1 = new TextLine();
            }
          }
        }
        if (textLine1 != null)
        {
          if (textLine1.WordCollection != null)
          {
            if (textLine1.WordCollection.Count > 0)
            {
              if (!lineCollection.Contains(textLine1))
              {
                if (renderer.extractTextElement[renderer.extractTextElement.Count - 1].renderedText != string.Empty)
                {
                  if (!(renderer.extractTextElement[renderer.extractTextElement.Count - 1].renderedText != ""))
                  {
                    if (!(renderer.extractTextElement[renderer.extractTextElement.Count - 1].renderedText != " "))
                      goto label_85;
                  }
                  string str = this.AddLineCollection(textLine1, lineCollection, renderer, lineStartIndex, glyphIndex);
                  stringBuilder.AppendLine(str);
                  TextLine textLine2 = new TextLine();
                }
              }
            }
          }
        }
      }
    }
label_85:
    textLines = lineCollection;
    return stringBuilder.ToString();
  }

  private bool IsNotInSameLine(
    double pageRotation,
    double currentYPosition,
    double currentBottomPosition,
    double currentXPosition,
    double previousYPosition,
    double previousBottomPosition,
    double previousXPosition,
    int extractTextElementIndex)
  {
    return pageRotation != 0.0 && pageRotation != 270.0 || (currentYPosition < previousYPosition || currentYPosition > previousBottomPosition) && (currentBottomPosition < previousYPosition || currentBottomPosition > previousBottomPosition) && (previousYPosition < currentYPosition || previousYPosition > currentBottomPosition) && (previousBottomPosition < currentYPosition || previousBottomPosition > currentBottomPosition) || previousXPosition > currentXPosition;
  }

  private string AddLineCollection(
    TextLine textLine,
    TextLines lineCollection,
    ImageRenderer renderer,
    int lineStartIndex,
    int glyphIndex)
  {
    bool flag1 = true;
    bool flag2 = true;
    bool flag3 = true;
    string str = string.Empty;
    float num1 = 0.0f;
    FontStyle fontStyle = FontStyle.Regular;
    Rect boundingRect1 = renderer.imageRenderGlyphList[lineStartIndex].BoundingRect;
    Rect boundingRect2 = renderer.imageRenderGlyphList[glyphIndex - 1].BoundingRect;
    double x = boundingRect1.Y < boundingRect2.Y ? boundingRect1.Y : boundingRect2.Y;
    double num2 = boundingRect1.Bottom > boundingRect2.Bottom ? boundingRect1.Bottom : boundingRect2.Bottom;
    if (this.Rotation == PdfPageRotateAngle.RotateAngle270)
    {
      x = boundingRect1.X < boundingRect2.X ? boundingRect1.X : boundingRect2.X;
      num2 = boundingRect1.Right > boundingRect2.Right ? boundingRect1.Right : boundingRect2.Right;
    }
    textLine.Bounds = this.Rotation != PdfPageRotateAngle.RotateAngle270 && this.Rotation != PdfPageRotateAngle.RotateAngle90 || textLine.WordCollection.Count != 1 || (double) textLine.WordCollection[0].Bounds.Y == boundingRect2.Y ? new RectangleF((float) boundingRect1.X, (float) boundingRect2.Y, (float) (boundingRect2.X + boundingRect2.Width - boundingRect1.X), (float) (num2 - x)) : textLine.WordCollection[0].Bounds;
    if (this.Rotation == PdfPageRotateAngle.RotateAngle270)
      textLine.Bounds = new RectangleF((float) x, (float) boundingRect1.Y, (float) (num2 - x), (float) (boundingRect2.Y + boundingRect2.Height - boundingRect1.Y));
    for (int index = lineStartIndex; index < glyphIndex; ++index)
    {
      Syncfusion.PdfViewer.Base.Glyph imageRenderGlyph = renderer.imageRenderGlyphList[index];
      if (index == 0)
      {
        str = imageRenderGlyph.FontFamily;
        num1 = (float) imageRenderGlyph.FontSize;
        fontStyle = imageRenderGlyph.FontStyle;
      }
      if (str == imageRenderGlyph.FontFamily && flag1)
      {
        textLine.FontName = str;
      }
      else
      {
        flag1 = false;
        textLine.FontName = string.Empty;
      }
      if ((double) num1 == imageRenderGlyph.FontSize && flag2)
      {
        textLine.FontSize = num1;
      }
      else
      {
        flag2 = false;
        textLine.FontSize = 0.0f;
      }
      if (fontStyle == imageRenderGlyph.FontStyle && flag3)
      {
        textLine.FontStyle = fontStyle;
      }
      else
      {
        flag3 = false;
        textLine.FontStyle = FontStyle.Regular;
      }
      if (!flag1)
        flag1 = true;
      if (!flag2)
        flag2 = true;
      if (!flag3)
        flag3 = true;
    }
    textLine.Text = "";
    foreach (TextWord word in textLine.WordCollection)
    {
      textLine.Text += word.Text;
      if (word != textLine.WordCollection[textLine.WordCollection.Count - 1])
        textLine.Text += " ";
    }
    lineCollection.Add(textLine);
    return textLine.Text;
  }

  private bool CheckColorContent(
    PdfRecordCollection recordCollection,
    PdfPageResources m_pageResources)
  {
    Colorspace m_currentColorspace = (Colorspace) null;
    if (recordCollection != null)
    {
      foreach (PdfRecord record in recordCollection)
      {
        string str = record.OperatorName;
        string[] operands = record.Operands;
        foreach (char symbolChar in this.m_symbolChars)
        {
          if (str.Contains(symbolChar.ToString()))
            str = str.Replace(symbolChar.ToString(), "");
        }
        switch (str.Trim())
        {
          case "rg":
          case "RG":
            Color color1 = this.GetColor(operands, "RGB");
            if (!(color1.Name == "ff000000") && !(color1.Name == "ffffffff"))
            {
              this.m_colorSpace = PdfColorSpace.RGB;
              return true;
            }
            continue;
          case "k":
          case "K":
            Color color2 = this.GetColor(operands, "DeviceCMYK");
            if (!(color2.Name == "ff000000") && !(color2.Name == "ffffffff"))
            {
              this.m_colorSpace = PdfColorSpace.CMYK;
              return true;
            }
            continue;
          case "cs":
          case "CS":
            m_currentColorspace = this.GetColorSpace(operands, m_pageResources, m_currentColorspace);
            continue;
          case "sc":
          case "SC":
          case "scn":
          case "SCN":
            if (m_currentColorspace != null)
            {
              Color color3 = m_currentColorspace.GetColor(operands);
              if (!(color3.Name == "ff000000") && !(color3.Name == "ffffffff"))
                return true;
              continue;
            }
            if (operands.Length == 4)
            {
              Color color4 = this.GetColor(operands, "DeviceCMYK");
              if (!(color4.Name == "ff000000") && !(color4.Name == "ffffffff"))
              {
                this.m_colorSpace = PdfColorSpace.CMYK;
                return true;
              }
              continue;
            }
            if (operands.Length == 3)
            {
              Color color5 = this.GetColor(operands, "RGB");
              if (!(color5.Name == "ff000000") && !(color5.Name == "ffffffff"))
              {
                this.m_colorSpace = PdfColorSpace.RGB;
                return true;
              }
              continue;
            }
            continue;
          case "Do":
            if (this.CheckColorContentXobject(operands, m_pageResources))
              return true;
            continue;
          default:
            continue;
        }
      }
    }
    return false;
  }

  internal string[] GetColorspace()
  {
    string[] colorspace = new string[0];
    System.Collections.Generic.Dictionary<string, object> colorSpaceResource = PageResourceLoader.Instance.GetColorSpaceResource((PdfDictionary) this.GetResources());
    if (colorSpaceResource != null && colorSpaceResource.Count != 0)
    {
      string[] strArray = new string[colorSpaceResource.Count];
      int length = 0;
      foreach (string key in colorSpaceResource.Keys)
      {
        string str = string.Empty;
        if (colorSpaceResource[key] is ExtendColorspace extendColorspace)
        {
          if (PdfCrossTable.Dereference(extendColorspace.ColorSpaceValueArray) is PdfArray pdfArray1 && pdfArray1.Count > 2)
          {
            if ((object) (pdfArray1[1] as PdfName) != null && (object) (pdfArray1[2] as PdfName) != null)
              str = $"{(pdfArray1[1] as PdfName).Value} + {(pdfArray1[2] as PdfName).Value}";
            else if (pdfArray1[1] is PdfArray && (object) (pdfArray1[2] as PdfName) != null)
            {
              PdfArray pdfArray = pdfArray1[1] as PdfArray;
              if (pdfArray.Count > 1)
              {
                for (int index = 0; index < pdfArray.Count - 1; ++index)
                  str = $"{str}{(pdfArray[index] as PdfName).Value} + ";
              }
              str = $"{str + (pdfArray[pdfArray.Count - 1] as PdfName).Value} + {(pdfArray1[2] as PdfName).Value}";
            }
            else if ((object) (pdfArray1[1] as PdfName) != null)
              str = (pdfArray1[1] as PdfName).Value;
            else if ((object) (pdfArray1[0] as PdfName) != null)
              str = (pdfArray1[0] as PdfName).Value;
          }
          else if (pdfArray1 != null && pdfArray1.Count == 2 && (object) (pdfArray1[0] as PdfName) != null)
            str = (pdfArray1[0] as PdfName).Value;
          PdfName colorSpaceValueArray = extendColorspace.ColorSpaceValueArray as PdfName;
          if (colorSpaceValueArray != (PdfName) null)
            str = colorSpaceValueArray.Value;
          if (str.Contains("#20"))
            str = str.Replace("#20", " ");
          bool flag = false;
          for (int index = 0; index < length; ++index)
          {
            if (strArray[index] == str)
              flag = true;
          }
          if (!flag)
          {
            strArray[length] = str;
            ++length;
          }
        }
      }
      colorspace = new string[length];
      for (int index = 0; index < length; ++index)
        colorspace[index] = strArray[index];
    }
    if (this.IsColored && colorspace.Length == 0)
      colorspace = new string[1]
      {
        this.m_colorSpace.ToString()
      };
    return colorspace;
  }

  private bool CheckColorContentXobject(string[] element, PdfPageResources pageResources)
  {
    if (!(pageResources.Resources[element[0].Replace("/", "")] is ImageStructure))
    {
      XObjectElement pageResource = pageResources[element[0].Replace("/", "")] as XObjectElement;
      PdfStream xobjectDictionary = pageResource.XObjectDictionary as PdfStream;
      xobjectDictionary.Decompress();
      PdfDictionary pdfDictionary1 = new PdfDictionary();
      PdfPageResources pdfPageResources = new PdfPageResources();
      if (pageResource.XObjectDictionary.ContainsKey("Resources"))
      {
        PdfDictionary pdfDictionary2 = (object) (pageResource.XObjectDictionary["Resources"] as PdfReference) == null ? ((object) (pageResource.XObjectDictionary["Resources"] as PdfReferenceHolder) == null ? pageResource.XObjectDictionary["Resources"] as PdfDictionary : (pageResource.XObjectDictionary["Resources"] as PdfReferenceHolder).Object as PdfDictionary) : (pageResource.XObjectDictionary["Resources"] as PdfReferenceHolder).Object as PdfDictionary;
        System.Collections.Generic.Dictionary<string, PdfMatrix> commonMatrix = new System.Collections.Generic.Dictionary<string, PdfMatrix>();
        pdfPageResources = this.m_resourceLoader.UpdatePageResources(this.m_resourceLoader.UpdatePageResources(this.m_resourceLoader.UpdatePageResources(this.m_resourceLoader.UpdatePageResources(this.m_resourceLoader.UpdatePageResources(this.m_resourceLoader.UpdatePageResources(pdfPageResources, this.m_resourceLoader.GetImageResources(pdfDictionary2, (PdfPageBase) null, ref commonMatrix)), this.m_resourceLoader.GetFontResources(pdfDictionary2)), this.m_resourceLoader.GetExtendedGraphicResources(pdfDictionary2)), this.m_resourceLoader.GetColorSpaceResource(pdfDictionary2)), this.m_resourceLoader.GetShadingResource(pdfDictionary2)), this.m_resourceLoader.GetPatternResource(pdfDictionary2));
      }
      if (this.CheckColorContent(new ContentParser(xobjectDictionary.InternalStream.ToArray()).ReadContent(), pdfPageResources))
        return true;
    }
    else if (pageResources.Resources[element[0].Replace("/", "")] is ImageStructure)
    {
      ImageStructure resource = pageResources.Resources[element[0].Replace("/", "")] as ImageStructure;
      if (resource.ColorSpace != null && !(resource.ColorSpace == "DeviceGray") || resource.ImageFilter != null && !(resource.ImageFilter[0] == "CCITTFaxDecode") && !(resource.ImageFilter[0] == "JBIG2Decode"))
        return true;
    }
    return false;
  }

  private Color GetColor(string[] colorElement, string colorspace)
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    float num3 = 0.0f;
    float num4 = 1f;
    if (colorspace == "RGB" && colorElement.Length == 3)
    {
      num1 = float.Parse(colorElement[0]);
      num2 = float.Parse(colorElement[1]);
      num3 = float.Parse(colorElement[2]);
    }
    else if (colorspace == "DeviceCMYK" && colorElement.Length == 4)
    {
      float result1;
      float.TryParse(colorElement[0], out result1);
      float result2;
      float.TryParse(colorElement[1], out result2);
      float result3;
      float.TryParse(colorElement[2], out result3);
      float result4;
      float.TryParse(colorElement[3], out result4);
      return this.ConvertCMYKtoRGB(result1, result2, result3, result4);
    }
    return Color.FromArgb((int) (byte) ((double) num4 * (double) byte.MaxValue), (int) (byte) ((double) num1 * (double) byte.MaxValue), (int) (byte) ((double) num2 * (double) byte.MaxValue), (int) (byte) ((double) num3 * (double) byte.MaxValue));
  }

  private Colorspace GetColorSpace(
    string[] element,
    PdfPageResources resources,
    Colorspace m_currentColorspace)
  {
    if (Colorspace.IsColorSpace(element[0].Replace("/", "")))
    {
      m_currentColorspace = Colorspace.CreateColorSpace(element[0].Replace("/", ""));
      if (element[0].Replace("/", "") == "Pattern")
      {
        object resource = resources[element[0].Replace("/", "")];
      }
    }
    else if (resources.ContainsKey(element[0].Replace("/", "")) && resources[element[0].Replace("/", "")] is ExtendColorspace)
    {
      ExtendColorspace resource = resources[element[0].Replace("/", "")] as ExtendColorspace;
      if (resource.ColorSpaceValueArray is PdfArray colorSpaceValueArray1)
        m_currentColorspace = Colorspace.CreateColorSpace((colorSpaceValueArray1[0] as PdfName).Value, (IPdfPrimitive) colorSpaceValueArray1);
      PdfName colorSpaceValueArray2 = resource.ColorSpaceValueArray as PdfName;
      if (colorSpaceValueArray2 != (PdfName) null)
        m_currentColorspace = Colorspace.CreateColorSpace(colorSpaceValueArray2.Value);
      if (resource.ColorSpaceValueArray is PdfDictionary colorSpaceValueArray3)
        m_currentColorspace = Colorspace.CreateColorSpace("Shading", (IPdfPrimitive) colorSpaceValueArray3);
    }
    return m_currentColorspace;
  }

  private Color ConvertCMYKtoRGB(float c, float m, float y, float k)
  {
    float num1 = (float) ((double) byte.MaxValue * (1.0 - (double) c) * (1.0 - (double) k));
    float num2 = (float) ((double) byte.MaxValue * (1.0 - (double) m) * (1.0 - (double) k));
    float num3 = (float) ((double) byte.MaxValue * (1.0 - (double) y) * (1.0 - (double) k));
    return Color.FromArgb((int) byte.MaxValue, (double) num1 > (double) byte.MaxValue ? (int) byte.MaxValue : ((double) num1 < 0.0 ? 0 : (int) num1), (double) num2 > (double) byte.MaxValue ? (int) byte.MaxValue : ((double) num2 < 0.0 ? 0 : (int) num2), (double) num3 > (double) byte.MaxValue ? (int) byte.MaxValue : ((double) num3 < 0.0 ? 0 : (int) num3));
  }

  private void RenderText(PdfRecordCollection recordCollection, PdfPageResources m_pageResources)
  {
    if (recordCollection == null)
      return;
    foreach (PdfRecord record in recordCollection)
    {
      string tokenType = record.OperatorName;
      string[] operands = record.Operands;
      foreach (char symbolChar in this.m_symbolChars)
      {
        if (tokenType.Contains(symbolChar.ToString()))
          tokenType = tokenType.Replace(symbolChar.ToString(), "");
      }
      switch (tokenType.Trim())
      {
        case "Tc":
          this.GetCharacterSpacing(operands);
          continue;
        case "Tw":
          this.GetWordSpacing(operands);
          continue;
        case "T*":
          this.resultantText += "\r\n";
          continue;
        case "Tf":
          this.RenderFont(operands);
          continue;
        case "ET":
          this.resultantText += "\r\n";
          continue;
        case "TJ":
        case "Tj":
        case "'":
          this.resultantText += this.RenderTextElement(operands, tokenType, m_pageResources);
          if (tokenType == "'")
          {
            this.resultantText += "\r\n";
            continue;
          }
          continue;
        case "Do":
          this.GetXObject(operands, m_pageResources);
          continue;
        default:
          continue;
      }
    }
  }

  private void GetCharacterSpacing(string[] element)
  {
    this.m_characterSpacing = float.Parse(element[0], (IFormatProvider) CultureInfo.InvariantCulture);
  }

  private void GetWordSpacing(string[] element)
  {
    this.m_wordSpacing = float.Parse(element[0], (IFormatProvider) CultureInfo.InvariantCulture);
    if ((double) this.m_wordSpacing <= 1.0)
      return;
    this.m_wordSpacing = 1f;
  }

  private string RenderTextElementTJ(
    string[] textElements,
    string token,
    PdfPageResources m_pageResources,
    Syncfusion.PdfViewer.Base.Matrix m_textLineMatrix)
  {
    try
    {
      List<string> stringList = new List<string>();
      string textToDecode = string.Join("", textElements);
      string str = textToDecode;
      if (m_pageResources.ContainsKey(this.m_currentFont))
      {
        this.m_structure = m_pageResources[this.m_currentFont] as FontStructure;
        this.m_structure.IsTextExtraction = true;
        if (this.m_structure != null)
          this.m_structure.FontSize = this.m_fontSize;
        List<string> decodedList = this.m_structure.DecodeTextExtractionTJ(textToDecode, true);
        this.m_structure.IsTextExtraction = false;
        str = this.RenderTextFromTJ(decodedList, m_textLineMatrix);
        if (this.m_structure.IsAdobeJapanFont && this.m_structure.AdobeJapanCidMapTable != null && this.m_structure.AdobeJapanCidMapTable.Count > 0)
        {
          string empty = string.Empty;
          foreach (char ch in str)
          {
            string mapChar = ch.ToString();
            empty += this.m_structure.AdobeJapanCidMapTableGlyphParser(mapChar);
          }
          str = empty;
        }
      }
      return str;
    }
    catch
    {
      return (string) null;
    }
  }

  private string RenderTextFromLeading(string decodedText, Syncfusion.PdfViewer.Base.Matrix m_textLineMatrix)
  {
    string empty = string.Empty;
    foreach (char character in decodedText)
    {
      string str = character.ToString();
      if (this.m_strBackup == " " && this.m_strBackup.Length == 1 && this.m_hasNoSpacing)
      {
        this.resultantText = this.resultantText.Remove(this.resultantText.Length - 1);
        this.m_hasNoSpacing = false;
      }
      System.Drawing.Drawing2D.Matrix matrix1 = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
      this.m_charID = (int) character;
      this.m_characterWidth = this.GetCharacterWidth(character);
      this.m_textMatrix = this.GetTextRenderingMatrix();
      Syncfusion.PdfViewer.Base.Matrix identity = Syncfusion.PdfViewer.Base.Matrix.Identity;
      identity.Scale(0.01, 0.01, 0.0, 0.0);
      identity.Translate(0.0, 1.0);
      this.m_transformations.PushTransform(identity * this.m_textMatrix);
      System.Drawing.Drawing2D.Matrix matrix2 = matrix1.Clone();
      matrix2.Multiply(this.GetTransformationMatrix(this.m_transformations.CurrentTransform));
      float num1 = this.m_textMatrix.M11 <= 0.0 ? (this.m_textMatrix.M12 == 0.0 || this.m_textMatrix.M21 == 0.0 ? this.m_structure.FontSize : (this.m_textMatrix.M12 >= 0.0 ? (float) this.m_textMatrix.M12 : (float) -this.m_textMatrix.M12)) : (float) this.m_textMatrix.M11;
      if (this.Rotation == PdfPageRotateAngle.RotateAngle90 || this.Rotation == PdfPageRotateAngle.RotateAngle270)
      {
        if ((double) matrix2.Elements[1] == 0.0 && (double) matrix2.Elements[2] == 0.0)
        {
          this.m_isRotated = false;
          this.m_boundingRect = new RectangleF((PointF) new Syncfusion.PdfViewer.Base.Point((double) this.UnitConvertor.ConvertFromPixels(matrix2.OffsetX, PdfGraphicsUnit.Point) / 1.0, ((double) this.UnitConvertor.ConvertFromPixels(matrix2.OffsetY, PdfGraphicsUnit.Point) - (double) this.UnitConvertor.ConvertFromPixels(num1 * 1f, PdfGraphicsUnit.Point)) / 1.0), (SizeF) new System.Drawing.Size((int) (this.m_characterWidth * (double) num1), (int) num1));
        }
        else
        {
          this.m_isRotated = true;
          this.m_boundingRect = new RectangleF((PointF) new Syncfusion.PdfViewer.Base.Point((double) this.UnitConvertor.ConvertFromPixels(matrix2.OffsetX, PdfGraphicsUnit.Point) / 1.0, ((double) this.UnitConvertor.ConvertFromPixels(matrix2.OffsetY, PdfGraphicsUnit.Point) - (double) this.UnitConvertor.ConvertFromPixels(num1 * 1f, PdfGraphicsUnit.Point)) / 1.0), (SizeF) new System.Drawing.Size((int) num1, (int) (this.m_characterWidth * (double) num1)));
        }
      }
      else
        this.m_boundingRect = new RectangleF((PointF) new Syncfusion.PdfViewer.Base.Point((double) this.UnitConvertor.ConvertFromPixels(matrix2.OffsetX, PdfGraphicsUnit.Point) / 1.0, ((double) this.UnitConvertor.ConvertFromPixels(matrix2.OffsetY, PdfGraphicsUnit.Point) - (double) this.UnitConvertor.ConvertFromPixels(num1 * 1f, PdfGraphicsUnit.Point)) / 1.0), (SizeF) new System.Drawing.Size((int) (this.m_characterWidth * (double) num1), (int) num1));
      double num2 = Math.Round(((double) this.m_boundingRect.Left - (double) this.m_tempBoundingRectangle.Right) / 10.0, 1);
      if ((double) this.m_tempBoundingRectangle.Right != 0.0 && (double) this.m_boundingRect.Left != 0.0 && num2 >= 1.0 && this.m_hasLeading)
        empty += (string) (object) Convert.ToChar(32 /*0x20*/);
      this.m_transformations.PopTransform();
      empty += str;
      this.UpdateTextMatrix();
      this.m_tempBoundingRectangle = this.m_boundingRect;
    }
    this.m_strBackup = empty;
    return empty;
  }

  private string RenderTextFromTJ(List<string> decodedList, Syncfusion.PdfViewer.Base.Matrix m_textLineMatrix)
  {
    string str = string.Empty;
    foreach (string decoded in decodedList)
    {
      double result;
      if (double.TryParse(decoded, out result))
      {
        this.UpdateTextMatrix(result);
        if ((int) (this.m_textLineMatrix.OffsetX - this.m_textMatrix.OffsetX) > 1 && !this.m_hasBDC)
          str += (string) (object) Convert.ToChar(32 /*0x20*/);
      }
      else
      {
        foreach (char character in decoded.Remove(decoded.Length - 1, 1))
        {
          System.Drawing.Drawing2D.Matrix matrix1 = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f);
          this.m_charID = (int) character;
          this.m_characterWidth = this.GetCharacterWidth(character);
          this.m_textMatrix = this.GetTextRenderingMatrix();
          Syncfusion.PdfViewer.Base.Matrix identity = Syncfusion.PdfViewer.Base.Matrix.Identity;
          identity.Scale(0.01, 0.01, 0.0, 0.0);
          identity.Translate(0.0, 1.0);
          this.m_transformations.PushTransform(identity * this.m_textMatrix);
          System.Drawing.Drawing2D.Matrix matrix2 = matrix1.Clone();
          matrix2.Multiply(this.GetTransformationMatrix(this.m_transformations.CurrentTransform));
          float num1 = this.m_textMatrix.M11 <= 0.0 ? (this.m_textMatrix.M12 == 0.0 || this.m_textMatrix.M21 == 0.0 ? this.m_structure.FontSize : (this.m_textMatrix.M12 >= 0.0 ? (float) this.m_textMatrix.M12 : (float) -this.m_textMatrix.M12)) : (float) this.m_textMatrix.M11;
          if (this.Rotation == PdfPageRotateAngle.RotateAngle90 || this.Rotation == PdfPageRotateAngle.RotateAngle270)
          {
            if ((double) matrix2.Elements[1] == 0.0 && (double) matrix2.Elements[2] == 0.0)
            {
              this.m_isRotated = false;
              this.m_boundingRect = new RectangleF((PointF) new Syncfusion.PdfViewer.Base.Point((double) this.UnitConvertor.ConvertFromPixels(matrix2.OffsetX, PdfGraphicsUnit.Point) / 1.0, ((double) this.UnitConvertor.ConvertFromPixels(matrix2.OffsetY, PdfGraphicsUnit.Point) - (double) this.UnitConvertor.ConvertFromPixels(num1 * 1f, PdfGraphicsUnit.Point)) / 1.0), (SizeF) new System.Drawing.Size((int) (this.m_characterWidth * (double) num1), (int) num1));
            }
            else
            {
              this.m_isRotated = true;
              this.m_boundingRect = new RectangleF((PointF) new Syncfusion.PdfViewer.Base.Point((double) this.UnitConvertor.ConvertFromPixels(matrix2.OffsetX, PdfGraphicsUnit.Point) / 1.0, ((double) this.UnitConvertor.ConvertFromPixels(matrix2.OffsetY, PdfGraphicsUnit.Point) - (double) this.UnitConvertor.ConvertFromPixels(num1 * 1f, PdfGraphicsUnit.Point)) / 1.0), (SizeF) new System.Drawing.Size((int) num1, (int) (this.m_characterWidth * (double) num1)));
            }
          }
          else
            this.m_boundingRect = new RectangleF((PointF) new Syncfusion.PdfViewer.Base.Point((double) this.UnitConvertor.ConvertFromPixels(matrix2.OffsetX, PdfGraphicsUnit.Point) / 1.0, ((double) this.UnitConvertor.ConvertFromPixels(matrix2.OffsetY, PdfGraphicsUnit.Point) - (double) this.UnitConvertor.ConvertFromPixels(num1 * 1f, PdfGraphicsUnit.Point)) / 1.0), (SizeF) new System.Drawing.Size((int) (this.m_characterWidth * (double) num1), (int) num1));
          double num2 = Math.Round(((double) this.m_boundingRect.Left - (double) this.m_tempBoundingRectangle.Right) / 10.0);
          if ((double) this.m_tempBoundingRectangle.Right != 0.0 && (double) this.m_boundingRect.Left != 0.0 && num2 > 1.0)
            str += (string) (object) Convert.ToChar(32 /*0x20*/);
          this.m_transformations.PopTransform();
          str += character.ToString();
          this.UpdateTextMatrix();
          this.m_tempBoundingRectangle = this.m_boundingRect;
          this.m_textMatrix = this.m_textLineMatrix;
        }
      }
    }
    if (str.Contains("\u0092"))
      str = str.Replace("\u0092", "");
    return str;
  }

  private System.Drawing.Drawing2D.Matrix GetTransformationMatrix(Syncfusion.PdfViewer.Base.Matrix transform)
  {
    return new System.Drawing.Drawing2D.Matrix((float) transform.M11, (float) transform.M12, (float) transform.M21, (float) transform.M22, (float) transform.OffsetX, (float) transform.OffsetY);
  }

  private void UpdateTextMatrix()
  {
    this.m_textLineMatrix = this.CalculateTextMatrix(this.m_textLineMatrix);
  }

  private Syncfusion.PdfViewer.Base.Matrix CalculateTextMatrix(Syncfusion.PdfViewer.Base.Matrix m)
  {
    if (this.m_charID == 32 /*0x20*/)
      this.m_wordSpacing = this.m_wordSpacing;
    return new Syncfusion.PdfViewer.Base.Matrix(1.0, 0.0, 0.0, 1.0, (this.m_characterWidth * (double) this.m_fontSize + (double) this.m_characterSpacing + (double) this.m_wordSpacing) * (double) (this.m_horizontalScaling / 100), 0.0) * m;
  }

  private double GetCharacterWidth(char character)
  {
    int key = (int) character;
    this.m_advancedWidth = this.m_structure.FontGlyphWidths == null || !(this.m_structure.fontType.Value == "TrueType") || !this.m_structure.FontGlyphWidths.ContainsKey(key) ? 1.0 : (double) this.m_structure.FontGlyphWidths[key] * (double) this.m_charSizeMultiplier;
    return this.m_advancedWidth;
  }

  private void UpdateTextMatrix(double tj)
  {
    double x = -(tj * 0.001 * (double) this.m_fontSize * (double) this.TextHorizontalScaling / 100.0);
    Syncfusion.PdfViewer.Base.Point point1 = this.m_textLineMatrix.Transform(new Syncfusion.PdfViewer.Base.Point(0.0, 0.0));
    Syncfusion.PdfViewer.Base.Point point2 = this.m_textLineMatrix.Transform(new Syncfusion.PdfViewer.Base.Point(x, 0.0));
    if (point1.X != point2.X)
      this.m_textLineMatrix.OffsetX = point2.X;
    else
      this.m_textLineMatrix.OffsetY = point2.Y;
  }

  private Syncfusion.PdfViewer.Base.Matrix GetTextRenderingMatrix()
  {
    return new Syncfusion.PdfViewer.Base.Matrix()
    {
      M11 = (double) this.m_fontSize * ((double) this.TextHorizontalScaling / 100.0),
      M22 = -(double) this.m_fontSize,
      OffsetY = ((double) this.m_fontSize + (double) this.m_rise)
    } * this.m_textLineMatrix * this.Ctm;
  }

  private void RenderTextAsLayout(
    PdfRecordCollection recordCollection,
    PdfPageResources m_pageResources)
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    int num3 = 0;
    float num4 = 0.0f;
    string empty = string.Empty;
    string str1 = string.Empty;
    if (recordCollection == null)
      return;
    foreach (PdfRecord record in recordCollection)
    {
      string str2 = record.OperatorName;
      string[] operands = record.Operands;
      foreach (char symbolChar in this.m_symbolChars)
      {
        if (str2.Contains(symbolChar.ToString()))
          str2 = str2.Replace(symbolChar.ToString(), "");
      }
      switch (str2.Trim())
      {
        case "q":
          this.m_hasET = false;
          continue;
        case "Tc":
          this.GetCharacterSpacing(operands);
          continue;
        case "Tw":
          this.GetWordSpacing(operands);
          continue;
        case "Tm":
          this.m_hasTm = true;
          float m11 = float.Parse(operands[0], (IFormatProvider) CultureInfo.InvariantCulture);
          float m12 = float.Parse(operands[1], (IFormatProvider) CultureInfo.InvariantCulture);
          float m21 = float.Parse(operands[2], (IFormatProvider) CultureInfo.InvariantCulture);
          float m22 = float.Parse(operands[3], (IFormatProvider) CultureInfo.InvariantCulture);
          float num5 = float.Parse(operands[4], (IFormatProvider) CultureInfo.InvariantCulture);
          float offsetY1 = float.Parse(operands[5], (IFormatProvider) CultureInfo.InvariantCulture);
          this.m_textMatrix = this.m_textLineMatrix = new Syncfusion.PdfViewer.Base.Matrix((double) m11, (double) m12, (double) m21, (double) m22, (double) num5, (double) offsetY1);
          if (this.m_isTextMatrix)
            this.PageGraphics.Restore(this.m_graphicsState.Pop());
          this.m_graphicsState.Push(this.PageGraphics.Save());
          if ((double) m11 != 0.0 || (double) m12 != 0.0 || (double) m21 != 0.0)
            this.PageGraphics.MultiplyTransform(new System.Drawing.Drawing2D.Matrix(m11, -m12, -m21, m22, num5, -offsetY1));
          else
            this.PageGraphics.MultiplyTransform(new System.Drawing.Drawing2D.Matrix(0.0001f, -m12, -m21, m22, num5, -offsetY1));
          this.CurrentLocation = (PointF) new Syncfusion.PdfViewer.Base.Point(0.0, 0.0);
          this.m_isTextMatrix = true;
          if (this.m_textMatrix.OffsetY == this.m_textLineMatrix.OffsetY && this.m_textMatrix.OffsetX != this.m_textLineMatrix.OffsetX)
            this.m_textLineMatrix = this.m_textMatrix;
          if (this.m_textLineMatrix.OffsetY != this.m_currentTextMatrix.OffsetY || this.m_textLineMatrix.OffsetX != this.m_currentTextMatrix.OffsetX && this.m_hasBDC && !this.m_hasTj)
          {
            this.m_tempBoundingRectangle = new RectangleF();
            this.m_hasBDC = false;
            continue;
          }
          continue;
        case "Tf":
          this.RenderFont(operands);
          continue;
        case "TL":
          this.SetTextLeading(float.Parse(operands[0]));
          continue;
        case "T*":
          this.MoveToNextLine(0.0f, this.m_textLeading);
          continue;
        case "BT":
          this.m_textLineMatrix = this.m_textMatrix = Syncfusion.PdfViewer.Base.Matrix.Identity;
          continue;
        case "ET":
          this.m_hasET = true;
          float num6 = ((float) this.m_textLineMatrix.OffsetX - this.m_tempBoundingRectangle.Right) / 10f;
          if (this.m_hasLeading && (double) num6 == 0.0 && this.m_hasNoSpacing)
          {
            this.resultantText += (string) (object) ' ';
            this.m_tempBoundingRectangle = new RectangleF();
            this.m_hasLeading = false;
          }
          this.CurrentLocation = PointF.Empty;
          if (this.m_isTextMatrix)
          {
            this.PageGraphics.Restore(this.m_graphicsState.Pop());
            this.m_isTextMatrix = false;
            continue;
          }
          continue;
        case "cm":
          this.m_hasET = false;
          float num7 = float.Parse(operands[5], (IFormatProvider) CultureInfo.InvariantCulture);
          int num8 = (int) num7;
          int num9 = (int) num1;
          int num10 = (num8 - num9) / 10;
          if (num8 != num9 && this.m_hasTm && (num10 < 0 || num10 >= 1))
          {
            this.resultantText += "\r\n";
            ++num3;
            this.m_hasTm = false;
          }
          num1 = num7;
          continue;
        case "BDC":
          this.m_hasBDC = true;
          this.m_hasET = true;
          continue;
        case "TD":
          this.SetTextLeading(-float.Parse(operands[1], (IFormatProvider) CultureInfo.InvariantCulture));
          this.MoveToNextLine(float.Parse(operands[0], (IFormatProvider) CultureInfo.InvariantCulture), float.Parse(operands[1], (IFormatProvider) CultureInfo.InvariantCulture));
          if (this.m_textLineMatrix.OffsetY != this.m_currentTextMatrix.OffsetY || this.m_hasBDC && this.m_textLineMatrix.OffsetX != this.m_currentTextMatrix.OffsetX && !this.m_hasTj)
          {
            this.m_tempBoundingRectangle = new RectangleF();
            this.m_hasBDC = false;
            continue;
          }
          continue;
        case "Td":
          this.MoveToNextLine(float.Parse(operands[0], (IFormatProvider) CultureInfo.InvariantCulture), float.Parse(operands[1], (IFormatProvider) CultureInfo.InvariantCulture));
          if (this.m_textLineMatrix.OffsetY != this.m_currentTextMatrix.OffsetY || this.m_hasBDC && this.m_textLineMatrix.OffsetX != this.m_currentTextMatrix.OffsetX)
          {
            this.m_tempBoundingRectangle = new RectangleF();
            this.m_hasBDC = false;
          }
          if (Math.Abs(this.m_textLineMatrix.OffsetX - this.m_currentTextMatrix.OffsetX) > 0.0 && !this.spaceBetweenWord && this.m_hasTj)
          {
            num4 = (float) Math.Abs(this.m_textLineMatrix.OffsetX - this.m_currentTextMatrix.OffsetX);
            this.spaceBetweenWord = true;
            continue;
          }
          continue;
        case "Tz":
          this.GetScalingFactor(operands);
          continue;
        case "'":
          this.MoveToNextLine(0.0f, this.m_textLeading);
          float offsetY2 = (float) this.m_textMatrix.OffsetY;
          this.m_hasNoSpacing = false;
          float num11 = (double) this.m_fontSize < 10.0 ? (float) Math.Round(((double) offsetY2 - (double) num2) / (double) this.m_fontSize) : (float) Math.Round(((double) offsetY2 - (double) num2) / 10.0);
          if ((double) num11 < 0.0)
            num11 = -num11;
          this.m_hasLeading = true;
          if ((double) num2 != 0.0 && (double) num11 >= 1.0)
            this.resultantText += "\r\n";
          long int64 = Convert.ToInt64(this.m_textLineMatrix.OffsetX);
          if (Convert.ToInt64(this.m_currentTextMatrix.OffsetX) - int64 > 0L)
            this.m_hasNoSpacing = true;
          string str3 = this.RenderTextElement(operands, str2, m_pageResources);
          this.m_currentTextMatrix = this.m_textLineMatrix;
          num2 = offsetY2;
          this.resultantText += str3;
          this.m_textMatrix = this.m_textLineMatrix;
          continue;
        case "TJ":
          float offsetY3 = (float) this.m_textMatrix.OffsetY;
          float num12 = (double) this.m_fontSize < 10.0 ? (float) Math.Round(((double) offsetY3 - (double) num2) / (double) this.m_fontSize) : (float) Math.Round(((double) offsetY3 - (double) num2) / 10.0);
          if ((double) num12 < 0.0)
            num12 = -num12;
          if (this.spaceBetweenWord)
          {
            if ((double) num4 > (double) this.m_fontSize)
            {
              double num13 = (double) num4 / (double) this.m_fontSize;
              num4 = 0.0f;
            }
            this.spaceBetweenWord = false;
          }
          this.m_hasTj = true;
          if ((double) num2 != 0.0 && (double) num12 >= 1.0)
            this.resultantText += "\r\n";
          string str4 = this.RenderTextElementTJ(operands, str2, m_pageResources, this.m_textLineMatrix);
          this.m_currentTextMatrix = this.m_textLineMatrix;
          num2 = offsetY3;
          this.resultantText += str4;
          this.m_textMatrix = this.m_textLineMatrix;
          this.m_hasET = false;
          this.m_hasBDC = true;
          continue;
        case "Tj":
          float offsetY4 = (float) this.m_textMatrix.OffsetY;
          float num14 = (double) this.m_fontSize < 10.0 ? (float) Math.Round(((double) offsetY4 - (double) num2) / (double) this.m_fontSize) : (float) Math.Round(((double) offsetY4 - (double) num2) / 10.0);
          if ((double) num14 < 0.0)
            num14 = -num14;
          if (this.spaceBetweenWord)
          {
            if ((double) num4 > (double) this.m_fontSize)
            {
              double num15 = (double) num4 / (double) this.m_fontSize;
              num4 = 0.0f;
            }
            if (this.m_hasET)
              this.resultantText += " ";
            this.m_hasET = false;
            this.spaceBetweenWord = false;
          }
          this.m_hasTj = true;
          if ((double) num2 != 0.0 && (double) num14 >= 1.0)
            this.resultantText += "\r\n";
          string str5 = this.RenderTextElement(operands, str2, m_pageResources);
          if (!(str5 == str1) || (double) offsetY4 < 1.0)
          {
            this.m_currentTextMatrix = this.m_textLineMatrix;
            num2 = offsetY4;
            str1 = str5;
            if (this.m_previousTextMatrix.OffsetY != 0.0 && this.m_currentTextMatrix.OffsetY != 0.0 && this.m_previousTextMatrix.OffsetY + (double) this.m_previousFontSize > this.m_currentTextMatrix.OffsetY + (double) this.m_fontSize && this.m_previousTextMatrix.OffsetY < this.m_currentTextMatrix.OffsetY && this.resultantText.Length >= 2 && this.resultantText.Substring(this.resultantText.Length - 2) == "\r\n")
              this.resultantText = this.resultantText.Remove(this.resultantText.Length - 2, 2);
            this.m_previousFontSize = this.m_fontSize;
            this.resultantText += str5;
            this.m_textMatrix = this.m_textLineMatrix;
            this.m_previousTextMatrix = this.m_textLineMatrix;
            continue;
          }
          continue;
        case "Do":
          this.m_isLayoutTextExtraction = true;
          this.GetXObject(operands, m_pageResources);
          this.m_isLayoutTextExtraction = false;
          continue;
        default:
          continue;
      }
    }
  }

  private void MoveToNextLine(float tx, float ty)
  {
    this.m_textLineMatrix = this.m_textMatrix = new Syncfusion.PdfViewer.Base.Matrix()
    {
      M11 = 1.0,
      M12 = 0.0,
      OffsetX = (double) tx,
      M21 = 0.0,
      M22 = 1.0,
      OffsetY = (double) ty
    } * this.m_textLineMatrix;
  }

  private void SetTextLeading(float txtLeading) => this.m_textLeading = -txtLeading;

  private void GetScalingFactor(string[] scaling)
  {
    this.TextHorizontalScaling = float.Parse(scaling[0], (IFormatProvider) CultureInfo.InvariantCulture);
  }

  private void GetXObject(string[] xobjectElement, PdfPageResources m_pageResources)
  {
    string key = this.StripSlashes(xobjectElement[0]);
    if (!m_pageResources.ContainsKey(key) || m_pageResources[key] is ImageStructure || !(m_pageResources[key] is XObjectElement))
      return;
    PdfRecordCollection recordCollection = (m_pageResources[key] as XObjectElement).Render(m_pageResources, this.m_graphicsState);
    PdfDictionary xobjectDictionary = (m_pageResources[key] as XObjectElement).XObjectDictionary;
    PdfPageResources pageResources = new PdfPageResources();
    System.Collections.Generic.Dictionary<string, PdfMatrix> commonMatrix = new System.Collections.Generic.Dictionary<string, PdfMatrix>();
    PdfPageResources m_pageResources1;
    if (xobjectDictionary.ContainsKey("Resources"))
    {
      PdfDictionary pdfDictionary = new PdfDictionary();
      PdfDictionary resourceDictionary = (object) (xobjectDictionary["Resources"] as PdfReferenceHolder) == null ? xobjectDictionary["Resources"] as PdfDictionary : (xobjectDictionary["Resources"] as PdfReferenceHolder).Object as PdfDictionary;
      m_pageResources1 = this.m_resourceLoader.UpdatePageResources(this.m_resourceLoader.UpdatePageResources(pageResources, this.m_resourceLoader.GetImageResources(resourceDictionary, this, ref commonMatrix)), this.m_resourceLoader.GetFontResources(resourceDictionary));
    }
    else
      m_pageResources1 = this.UpdateFontResources(m_pageResources);
    if (!this.m_isLayoutTextExtraction)
    {
      this.RenderText(recordCollection, m_pageResources1);
    }
    else
    {
      this.RenderTextAsLayout(recordCollection, m_pageResources1);
      this.resultantText += "\r\n";
    }
    recordCollection.RecordCollection.Clear();
    commonMatrix.Clear();
  }

  private PdfPageResources UpdateFontResources(PdfPageResources pageResources)
  {
    PdfPageResources pdfPageResources = new PdfPageResources();
    foreach (KeyValuePair<string, object> resource in pageResources.Resources)
    {
      if (resource.Value is FontStructure)
      {
        pdfPageResources.Resources.Add(resource.Key, resource.Value);
        pdfPageResources.fontCollection.Add(resource.Key, resource.Value as FontStructure);
      }
    }
    return pdfPageResources;
  }

  private string RenderTextElement(
    string[] textElements,
    string tokenType,
    PdfPageResources m_pageResources)
  {
    try
    {
      string str = string.Join("", textElements);
      if (!m_pageResources.ContainsKey(this.m_currentFont) && this.m_currentFont != null && this.m_currentFont.Contains("-"))
        this.m_currentFont = this.m_currentFont.Replace("-", "#2D");
      if (m_pageResources.ContainsKey(this.m_currentFont))
      {
        FontStructure mPageResource = m_pageResources[this.m_currentFont] as FontStructure;
        mPageResource.IsTextExtraction = true;
        if (mPageResource != null)
          mPageResource.FontSize = this.m_fontSize;
        str = mPageResource.DecodeTextExtraction(str, true);
        this.pageWords.Add(str);
        mPageResource.IsTextExtraction = false;
        if (mPageResource.IsAdobeJapanFont && mPageResource.AdobeJapanCidMapTable != null && mPageResource.AdobeJapanCidMapTable.Count > 0)
        {
          string empty = string.Empty;
          foreach (char ch in str)
          {
            string mapChar = ch.ToString();
            empty += mPageResource.AdobeJapanCidMapTableGlyphParser(mapChar);
          }
          str = empty;
        }
        if (this.m_isLayoutTextExtraction)
        {
          this.m_structure = mPageResource;
          if (tokenType == "Tj")
            this.m_tempBoundingRectangle = new RectangleF();
          str = this.RenderTextFromLeading(str, this.m_textLineMatrix);
        }
      }
      return str;
    }
    catch
    {
      return (string) null;
    }
  }

  private void RenderFont(string[] fontElements)
  {
    int index;
    for (index = 0; index < fontElements.Length; ++index)
    {
      if (fontElements[index].Contains("/"))
      {
        this.m_currentFont = fontElements[index].Replace("/", "");
        break;
      }
    }
    this.m_fontSize = float.Parse(fontElements[index + 1]);
  }

  private string SkipEscapeSequence(string text)
  {
    int startIndex = -1;
    do
    {
      startIndex = text.IndexOf("\\", startIndex + 1);
      if (text.Length > startIndex + 1)
      {
        string str = text[startIndex + 1].ToString();
        if (startIndex >= 0 && (str == "\\" || str == "(" || str == ")"))
          text = text.Remove(startIndex, 1);
      }
      else
      {
        text = text.Remove(startIndex, 1);
        startIndex = -1;
      }
    }
    while (startIndex >= 0);
    return text;
  }

  internal System.Collections.Generic.Dictionary<PdfName, IPdfPrimitive> GetFontDictionary(
    PdfDictionary xobjectStream)
  {
    if (xobjectStream == null || !xobjectStream.ContainsKey("Font") || !(xobjectStream["Font"] is PdfDictionary))
      return (System.Collections.Generic.Dictionary<PdfName, IPdfPrimitive>) null;
    PdfDictionary pdfDictionary = xobjectStream["Font"] as PdfDictionary;
    List<IPdfPrimitive> pdfPrimitiveList = new List<IPdfPrimitive>();
    List<PdfName> pdfNameList = new List<PdfName>();
    System.Collections.Generic.Dictionary<PdfName, IPdfPrimitive> items = pdfDictionary.Items;
    foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in items)
    {
      pdfNameList.Add(keyValuePair.Key);
      pdfPrimitiveList.Add(keyValuePair.Value);
    }
    return items;
  }

  private PdfDictionary GetPDFFontDictionary(PdfDictionary resources)
  {
    PdfDictionary pdfFontDictionary1 = new PdfDictionary();
    IPdfPrimitive pdfxObject = this.GetPDFXObject(resources);
    if (pdfxObject != null && pdfxObject is PdfDictionary)
    {
      foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair1 in ((PdfDictionary) pdfxObject).Items)
      {
        if ((object) (keyValuePair1.Value as PdfReferenceHolder) != null)
        {
          PdfStream pdfStream = ((PdfReferenceHolder) keyValuePair1.Value).Object as PdfStream;
          PdfDictionary pdfDictionary1 = (PdfDictionary) pdfStream;
          if (pdfDictionary1.ContainsKey("Resources"))
          {
            if (!(pdfDictionary1["Resources"] is PdfDictionary resources1) && (object) (pdfDictionary1["Resources"] as PdfReferenceHolder) != null)
            {
              PdfReferenceHolder pdfReferenceHolder = pdfDictionary1["Resources"] as PdfReferenceHolder;
              resources1 = (pdfDictionary1["Resources"] as PdfReferenceHolder).Object as PdfDictionary;
              if (this.resourceNumber != pdfReferenceHolder.Reference.ObjNum)
                this.resourceNumber = pdfReferenceHolder.Reference.ObjNum;
              else
                continue;
            }
            PdfDictionary pdfDictionary2 = (PdfDictionary) null;
            if (resources1.ContainsKey("Font"))
              pdfDictionary2 = resources1["Font"] as PdfDictionary;
            if (resources1.ContainsKey("XObject") && pdfDictionary2 == null)
            {
              PdfDictionary pdfFontDictionary2 = this.GetPDFFontDictionary(resources1);
              if (pdfFontDictionary2 != null)
              {
                foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair2 in pdfFontDictionary2.Items)
                {
                  if (!pdfFontDictionary1.ContainsKey(keyValuePair2.Key))
                    pdfFontDictionary1.Items.Add(keyValuePair2.Key, keyValuePair2.Value);
                }
              }
            }
            if (resources1.ContainsKey("Font"))
            {
              if (resources1.Items[new PdfName("Font")] is PdfDictionary pdfDictionary3)
              {
                foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair3 in pdfDictionary3.Items)
                {
                  if (!pdfFontDictionary1.ContainsKey(keyValuePair3.Key))
                    pdfFontDictionary1.Items.Add(keyValuePair3.Key, keyValuePair3.Value);
                }
              }
              if (pdfDictionary1.ContainsKey("Subtype") && (pdfDictionary1.Items[new PdfName("Subtype")] as PdfName).Value != "Image")
              {
                if (this.m_xObjectContentStream == null)
                  this.m_xObjectContentStream = new List<PdfStream>();
                this.m_xObjectContentStream.Add(pdfStream);
              }
            }
          }
        }
      }
    }
    return pdfFontDictionary1;
  }

  internal void GetFontStream()
  {
    PdfDictionary dictionary = this.Dictionary;
    if (!this.Dictionary.ContainsKey("Resources"))
      return;
    if (this.Dictionary["Resources"] is PdfDictionary pdfDictionary)
    {
      pdfDictionary4 = (PdfDictionary) null;
      PdfDictionary pdfDictionary1 = (PdfDictionary) null;
      bool flag = false;
      if (pdfDictionary.ContainsKey("Font"))
      {
        pdfDictionary4 = pdfDictionary["Font"] as PdfDictionary;
        PdfDictionary pdfFontDictionary = this.GetPDFFontDictionary((PdfDictionary) this.GetResources());
        if (pdfFontDictionary != null)
        {
          foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in pdfFontDictionary.Items)
          {
            if (!pdfDictionary4.ContainsKey(keyValuePair.Key))
              pdfDictionary4.Items.Add(keyValuePair.Key, keyValuePair.Value);
          }
        }
        flag = true;
      }
      if (pdfDictionary.ContainsKey("XObject"))
      {
        PdfDictionary pdfDictionary2 = (PdfDictionary) null;
        IPdfPrimitive xobject = this.GetXObject(this.GetResources());
        while (true)
        {
          if (xobject != null && xobject is PdfDictionary)
          {
            foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in ((PdfDictionary) xobject).Items)
            {
              if ((object) (keyValuePair.Value as PdfReferenceHolder) != null)
              {
                PdfDictionary pdfDictionary3 = (PdfDictionary) (((PdfReferenceHolder) keyValuePair.Value).Object as PdfStream);
                if (pdfDictionary3.ContainsKey("Resources"))
                  pdfDictionary2 = pdfDictionary3["Resources"] as PdfDictionary;
              }
            }
          }
          if (pdfDictionary2 != null)
          {
            if (!pdfDictionary2.ContainsKey("Font"))
            {
              if (xobject != null && pdfDictionary2.ContainsKey("XObject") && PdfCrossTable.Dereference(pdfDictionary2["XObject"]) is PdfDictionary baseDictionary)
                xobject = this.GetXObject(new PdfResources(baseDictionary));
              else
                goto label_27;
            }
            else
              break;
          }
          else
            goto label_27;
        }
        pdfDictionary1 = pdfDictionary2["Font"] as PdfDictionary;
      }
label_27:
      if (pdfDictionary4 != null)
      {
        this.m_fontcollect = pdfDictionary4.Items;
        foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in this.m_fontcollect)
        {
          this.m_fontNames.Add(keyValuePair.Key);
          this.m_fontReference.Add(keyValuePair.Value);
        }
      }
      if (pdfDictionary1 != null)
      {
        this.m_fontcollect = pdfDictionary1.Items;
        foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in this.m_fontcollect)
        {
          this.m_fontNames.Add(keyValuePair.Key);
          this.m_fontReference.Add(keyValuePair.Value);
        }
      }
      else if ((object) (pdfDictionary["Font"] as PdfReferenceHolder) != null && (pdfDictionary["Font"] as PdfReferenceHolder).Object is PdfDictionary pdfDictionary4)
      {
        this.m_fontNames = new List<PdfName>();
        this.m_fontReference = new List<IPdfPrimitive>();
        foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in pdfDictionary4.Items)
        {
          this.m_fontNames.Add(keyValuePair.Key);
          this.m_fontReference.Add(keyValuePair.Value);
        }
      }
      if (pdfDictionary4 == null && pdfDictionary.ContainsKey("XObject") && pdfDictionary["XObject"] is PdfDictionary pdfDictionary8)
      {
        foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair1 in pdfDictionary8.Items)
        {
          if ((object) (keyValuePair1.Value as PdfReferenceHolder) != null)
          {
            PdfDictionary pdfDictionary5 = (keyValuePair1.Value as PdfReferenceHolder).Object as PdfDictionary;
            if (pdfDictionary5.ContainsKey("Resources"))
            {
              PdfDictionary pdfDictionary6 = pdfDictionary5["Resources"] as PdfDictionary;
              if (pdfDictionary6.ContainsKey("Font"))
              {
                this.m_fontNames = new List<PdfName>();
                this.m_fontReference = new List<IPdfPrimitive>();
                if (!(pdfDictionary6["Font"] is PdfDictionary pdfDictionary7))
                {
                  PdfReferenceHolder pdfReferenceHolder = pdfDictionary6["Font"] as PdfReferenceHolder;
                  if (pdfReferenceHolder != (PdfReferenceHolder) null)
                    pdfDictionary7 = pdfReferenceHolder.Object as PdfDictionary;
                }
                foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair2 in pdfDictionary7.Items)
                {
                  this.m_fontNames.Add(keyValuePair2.Key);
                  this.m_fontReference.Add(keyValuePair2.Value);
                }
              }
            }
          }
        }
      }
      if (flag || pdfDictionary1 != null)
        return;
      PdfDictionary pdfFontDictionary1 = this.GetPDFFontDictionary((PdfDictionary) this.GetResources());
      if (pdfFontDictionary1 == null)
        return;
      this.m_fontNames = new List<PdfName>();
      this.m_fontReference = new List<IPdfPrimitive>();
      foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in pdfFontDictionary1.Items)
      {
        this.m_fontNames.Add(keyValuePair.Key);
        this.m_fontReference.Add(keyValuePair.Value);
      }
    }
    else
    {
      if (pdfDictionary != null)
        return;
      PdfReferenceHolder pdfReferenceHolder = this.Dictionary["Resources"] as PdfReferenceHolder;
      if (!(pdfReferenceHolder != (PdfReferenceHolder) null) || !(pdfReferenceHolder.Object is PdfDictionary))
        return;
      PdfDictionary pdfDictionary9 = pdfReferenceHolder.Object as PdfDictionary;
      if (pdfDictionary9["Font"] is PdfDictionary)
      {
        if (!(pdfDictionary9["Font"] is PdfDictionary pdfDictionary10))
          return;
        this.m_fontNames = new List<PdfName>();
        this.m_fontReference = new List<IPdfPrimitive>();
        foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in pdfDictionary10.Items)
        {
          this.m_fontNames.Add(keyValuePair.Key);
          this.m_fontReference.Add(keyValuePair.Value);
        }
      }
      else
      {
        if ((object) (pdfDictionary9["Font"] as PdfReferenceHolder) == null || !((pdfDictionary9["Font"] as PdfReferenceHolder).Object is PdfDictionary pdfDictionary11))
          return;
        this.m_fontNames = new List<PdfName>();
        this.m_fontReference = new List<IPdfPrimitive>();
        foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in pdfDictionary11.Items)
        {
          this.m_fontNames.Add(keyValuePair.Key);
          this.m_fontReference.Add(keyValuePair.Value);
        }
      }
    }
  }

  internal int GetImageCount()
  {
    int imageCount = 0;
    PdfDictionary dictionary = this.Dictionary;
    if (this.Dictionary.ContainsKey("Resources") && this.Dictionary["Resources"] is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey("XObject"))
    {
      PdfDictionary pdfDictionary1 = (PdfDictionary) null;
      IPdfPrimitive xobject = this.GetXObject(this.GetResources());
      while (true)
      {
        do
        {
          if (xobject != null && xobject is PdfDictionary)
          {
            foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in ((PdfDictionary) xobject).Items)
            {
              if ((object) (keyValuePair.Value as PdfReferenceHolder) != null)
              {
                PdfStream pdfStream = ((PdfReferenceHolder) keyValuePair.Value).Object as PdfStream;
                PdfDictionary pdfDictionary2 = (PdfDictionary) pdfStream;
                if (pdfStream.ContainsKey("Subtype") && (pdfStream["Subtype"] as PdfName).Value == "Image" && !this.m_image_reference.Contains(keyValuePair.Value) && !this.m_image_Names.Contains(keyValuePair.Key))
                {
                  ++imageCount;
                  this.m_image_reference.Add(keyValuePair.Value);
                  this.m_image_Names.Add(keyValuePair.Key);
                }
                if (pdfDictionary2.ContainsKey("Resources"))
                  pdfDictionary1 = pdfDictionary2["Resources"] as PdfDictionary;
              }
            }
          }
          if (pdfDictionary1 == null || xobject == null || !pdfDictionary1.ContainsKey("XObject"))
            goto label_16;
        }
        while (!pdfDictionary1.ContainsKey("XObject"));
        if (PdfCrossTable.Dereference(pdfDictionary1["XObject"]) is PdfDictionary baseDictionary)
          xobject = this.GetXObject(new PdfResources(baseDictionary));
        else
          break;
      }
    }
label_16:
    return imageCount;
  }

  internal IPdfPrimitive GetXObject(PdfResources resources)
  {
    IPdfPrimitive xobject = (IPdfPrimitive) null;
    foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in resources.Items)
    {
      if (keyValuePair.Key.ToString() == "/XObject")
        xobject = PdfCrossTable.Dereference(keyValuePair.Value);
    }
    return xobject;
  }

  private IPdfPrimitive GetPDFXObject(PdfDictionary resources)
  {
    IPdfPrimitive pdfxObject = (IPdfPrimitive) null;
    foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in resources.Items)
    {
      if (keyValuePair.Key.ToString() == "/XObject")
        pdfxObject = PdfCrossTable.Dereference(keyValuePair.Value);
    }
    return pdfxObject;
  }

  private string StripSlashes(string text) => text.Replace("/", "");

  internal Image[] ExtractImages(bool isImageExtraction)
  {
    this.isExtractImages = true;
    float num = 0.0f;
    PdfArray contents = this.Contents;
    PdfDictionary pdfDictionary1 = new PdfDictionary();
    PdfStream pdfStream1 = (PdfStream) null;
    string empty = string.Empty;
    CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
    RectangleF rectangleF1 = new RectangleF();
    this.m_pageResources = PageResourceLoader.Instance.GetPageResources(this);
    this.m_currentMatrix.Push(new GraphicsStateData()
    {
      m_drawing2dMatrixCTM = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f)
    });
    ContentParser contentParser = (ContentParser) null;
    if (this.GetResources().ContainsKey("XObject"))
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        this.CombineContent((Stream) memoryStream);
        memoryStream.Position = 0L;
        contentParser = new ContentParser(memoryStream.ToArray());
        this.m_recordCollection = contentParser.ReadContent();
      }
      for (int index1 = 0; index1 < this.m_recordCollection.RecordCollection.Count; ++index1)
      {
        string str = this.m_recordCollection.RecordCollection[index1].OperatorName;
        string[] operands = this.m_recordCollection.RecordCollection[index1].Operands;
        foreach (char symbolChar in this.m_symbolChars)
        {
          if (str.Contains(symbolChar.ToString()))
            str = str.Replace(symbolChar.ToString(), "");
        }
        switch (str.Trim())
        {
          case "q":
            GraphicsStateData graphicsStateData1 = new GraphicsStateData();
            if (this.m_currentMatrix.Count > 0)
            {
              GraphicsStateData graphicsStateData2 = this.m_currentMatrix.Peek();
              graphicsStateData1.m_drawing2dMatrixCTM = graphicsStateData2.m_drawing2dMatrixCTM;
            }
            this.m_currentMatrix.Push(graphicsStateData1);
            break;
          case "cm":
            System.Drawing.Drawing2D.Matrix matrix1 = new System.Drawing.Drawing2D.Matrix(float.Parse(operands[0]), float.Parse(operands[1]), float.Parse(operands[2]), float.Parse(operands[3]), float.Parse(operands[4]), float.Parse(operands[5]));
            this.m_currentMatrix.Peek().m_drawing2dMatrixCTM = this.Multiply(matrix1, this.m_currentMatrix.Peek().m_drawing2dMatrixCTM);
            break;
          case "Do":
            ImageStructure imageStructure = (ImageStructure) null;
            string key = operands[0].Replace("/", "");
            if (!(this.m_pageResources.Resources[operands[0].Replace("/", "")] is ImageStructure))
            {
              XObjectElement pageResource = this.m_pageResources[operands[0].Replace("/", "")] as XObjectElement;
              PdfStream xobjectDictionary = pageResource.XObjectDictionary as PdfStream;
              xobjectDictionary.Decompress();
              PdfDictionary pdfDictionary2 = new PdfDictionary();
              PdfPageResources pdfPageResources = new PdfPageResources();
              if (pageResource.XObjectDictionary.ContainsKey("Resources"))
              {
                PdfDictionary pdfDictionary3 = (object) (pageResource.XObjectDictionary["Resources"] as PdfReference) == null ? ((object) (pageResource.XObjectDictionary["Resources"] as PdfReferenceHolder) == null ? pageResource.XObjectDictionary["Resources"] as PdfDictionary : (pageResource.XObjectDictionary["Resources"] as PdfReferenceHolder).Object as PdfDictionary) : (pageResource.XObjectDictionary["Resources"] as PdfReferenceHolder).Object as PdfDictionary;
                System.Collections.Generic.Dictionary<string, PdfMatrix> commonMatrix = new System.Collections.Generic.Dictionary<string, PdfMatrix>();
                pdfPageResources = this.m_resourceLoader.UpdatePageResources(this.m_resourceLoader.UpdatePageResources(this.m_resourceLoader.UpdatePageResources(this.m_resourceLoader.UpdatePageResources(this.m_resourceLoader.UpdatePageResources(this.m_resourceLoader.UpdatePageResources(pdfPageResources, this.m_resourceLoader.GetImageResources(pdfDictionary3, (PdfPageBase) null, ref commonMatrix)), this.m_resourceLoader.GetFontResources(pdfDictionary3)), this.m_resourceLoader.GetExtendedGraphicResources(pdfDictionary3)), this.m_resourceLoader.GetColorSpaceResource(pdfDictionary3)), this.m_resourceLoader.GetShadingResource(pdfDictionary3)), this.m_resourceLoader.GetPatternResource(pdfDictionary3));
              }
              this.ExtractInnerXObjectImages(xobjectDictionary, pdfPageResources);
              break;
            }
            if (this.m_pageResources.Resources[operands[0].Replace("/", "")] is ImageStructure)
              imageStructure = this.m_pageResources.Resources[operands[0].Replace("/", "")] as ImageStructure;
            try
            {
              PdfDictionary imageDictionary1 = imageStructure.ImageDictionary;
              if (imageDictionary1 != null)
              {
                PdfArray pdfArray1 = (PdfArray) null;
                PdfArray pdfArray2 = (PdfArray) null;
                if (imageDictionary1["ColorSpace"] is PdfArray)
                  pdfArray1 = imageDictionary1["ColorSpace"] as PdfArray;
                if ((object) (imageDictionary1["ColorSpace"] as PdfReferenceHolder) != null)
                  pdfArray1 = (imageDictionary1["ColorSpace"] as PdfReferenceHolder).Object as PdfArray;
                if (pdfArray1 != null && (object) (pdfArray1[1] as PdfReferenceHolder) != null)
                  pdfArray2 = (pdfArray1[1] as PdfReferenceHolder).Object as PdfArray;
                if (pdfArray2 != null && (object) (pdfArray2[1] as PdfReferenceHolder) != null)
                {
                  IPdfPrimitive pdfPrimitive = (pdfArray2[1] as PdfReferenceHolder).Object;
                }
              }
              PdfStream imageDictionary2 = imageStructure.ImageDictionary as PdfStream;
              PdfMatrix pdfMatrix = imageStructure.ImageInfo;
              for (int index2 = 0; index2 < this.Contents.Count; ++index2)
              {
                PdfStream pdfStream2 = pdfStream1 == null ? (this.Contents[index2] as PdfReferenceHolder).Object as PdfStream : pdfStream1;
                pdfStream2.Decompress();
                MemoryStream internalStream = pdfStream2.InternalStream;
                PdfReader ContentStream = new PdfReader((Stream) internalStream);
                if (ContentStream.ReadStream().Contains(key))
                {
                  internalStream.Position = 0L;
                  ContentStream.Position = 0L;
                  pdfMatrix = this.Rotation != PdfPageRotateAngle.RotateAngle90 ? new PdfMatrix(ContentStream, key, this.Size) : new PdfMatrix(ContentStream, key, this.Size);
                  break;
                }
              }
              bool flag1 = false;
              if (imageDictionary1.ContainsKey("Mask"))
                flag1 = true;
              if (imageDictionary2.ContainsKey("Width"))
              {
                if (imageDictionary2["Width"] is PdfNumber)
                {
                  int intValue1 = (imageDictionary2["Width"] as PdfNumber).IntValue;
                }
                if ((object) (imageDictionary2["Width"] as PdfReferenceHolder) != null)
                {
                  int intValue2 = ((imageDictionary2["Width"] as PdfReferenceHolder).Object as PdfNumber).IntValue;
                }
                if (imageDictionary2["Height"] is PdfNumber)
                {
                  int intValue3 = (imageDictionary2["Height"] as PdfNumber).IntValue;
                }
                if ((object) (imageDictionary2["Height"] as PdfReferenceHolder) != null)
                {
                  int intValue4 = ((imageDictionary2["Height"] as PdfReferenceHolder).Object as PdfNumber).IntValue;
                }
              }
              else if (imageDictionary2.ContainsKey("BBox"))
              {
                PdfArray pdfArray = imageDictionary2["BBox"] as PdfArray;
                int intValue5 = (pdfArray[2] as PdfNumber).IntValue;
                int intValue6 = (pdfArray[3] as PdfNumber).IntValue;
              }
              System.Drawing.Drawing2D.Matrix matrix = this.Multiply(this.Multiply(new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, -1.01f, 0.0f, 1f), this.m_currentMatrix.Peek().m_drawing2dMatrixCTM), new System.Drawing.Drawing2D.Matrix(1.33333337f, 0.0f, 0.0f, -1.33333337f, 0.0f, this.Size.Height * 1.33333337f));
              RectangleF rectangleF2;
              if (this.Rotation == PdfPageRotateAngle.RotateAngle270)
                rectangleF2 = (double) matrix.Elements[0] == 0.0 || (double) matrix.Elements[3] == 0.0 ? new RectangleF((float) Math.Floor((double) matrix.OffsetY / 1.3333333730697632), (float) (Math.Floor((double) this.Size.Width) - (Math.Round((double) (matrix.OffsetX / 1.33333337f), 5) + Math.Floor((double) matrix.Elements[0] / 1.3333333730697632))), Math.Abs(this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[1]), Math.Abs(this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[2])) : new RectangleF((float) Math.Floor((double) matrix.OffsetY / 1.3333333730697632), this.Size.Width - (float) (Math.Round((double) (matrix.OffsetX / 1.33333337f), 5) + (double) matrix.Elements[0] / 1.3333333730697632), this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[3], this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[0]);
              else if (this.Rotation == PdfPageRotateAngle.RotateAngle90)
                rectangleF2 = (double) matrix.Elements[0] != 0.0 || (double) matrix.Elements[3] != 0.0 ? new RectangleF(new PointF(this.Size.Height - matrix.OffsetY / 1.33333337f - this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[3], matrix.Elements[4] / 1.33333337f), new SizeF(this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[3], this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[0])) : new RectangleF(new PointF(this.Size.Height - matrix.Elements[5] / 1.33333337f, matrix.Elements[4] / 1.33333337f), new SizeF(this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[1], -this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[2]));
              else if (this.Rotation == PdfPageRotateAngle.RotateAngle180)
                rectangleF2 = new RectangleF(this.Size.Width - matrix.OffsetX / 1.33333337f - this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[0], (float) ((double) this.Size.Height - (double) this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[3] - Math.Round((double) (matrix.OffsetY / 1.33333337f), 5)), this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[0], this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[3]);
              else if ((double) matrix.Elements[0] == 0.0 && (double) matrix.Elements[3] == 0.0)
              {
                if ((double) matrix.Elements[1] < 0.0 && (double) matrix.Elements[2] > 0.0)
                {
                  num = 270f;
                  rectangleF2 = new RectangleF(this.Size.Height - matrix.Elements[5] / 1.33333337f, (float) Math.Round((double) (matrix.Elements[4] / 1.33333337f), 5), -this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[2], this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[1]);
                }
                else if ((double) matrix.Elements[1] > 0.0 && (double) matrix.Elements[2] < 0.0)
                {
                  num = 90f;
                  rectangleF2 = new RectangleF(matrix.Elements[5] / 1.33333337f, (float) Math.Round((double) (this.Size.Width - matrix.Elements[4] / 1.33333337f), 5), -this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[2], -this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[1]);
                }
                else if ((double) matrix.Elements[1] < 0.0 && (double) matrix.Elements[2] < 0.0)
                {
                  num = 180f;
                  rectangleF2 = new RectangleF(this.Size.Width - matrix.OffsetX / 1.33333337f - this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[0], (float) ((double) this.Size.Height - (double) this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[3] - Math.Round((double) (matrix.OffsetY / 1.33333337f), 5)), this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[0], this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[3]);
                }
                else
                  rectangleF2 = new RectangleF(this.Size.Width - matrix.OffsetX / 1.33333337f - this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[0], (float) ((double) this.Size.Height - (double) this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[3] - Math.Round((double) (matrix.OffsetY / 1.33333337f), 5)), this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[0], this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[3]);
              }
              else
                rectangleF2 = new RectangleF(matrix.OffsetX / 1.33333337f, (float) Math.Round((double) (matrix.OffsetY / 1.33333337f), 5), this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[0], this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[3]);
              bool flag2 = false;
              if (this.is_Contains_Redaction)
              {
                RectangleF b = rectangleF2;
                b.X *= this.pt;
                b.Y *= this.pt;
                b.Width *= this.pt;
                b.Height *= this.pt;
                for (int index3 = 0; index3 < this.m_RedactionBounds.Count; ++index3)
                {
                  RectangleF redactionBound = this.m_RedactionBounds[index3];
                  if (RectangleF.Intersect(new RectangleF(redactionBound.X * this.pt, redactionBound.Y * this.pt, redactionBound.Width * this.pt, redactionBound.Height * this.pt), b) != RectangleF.Empty)
                    flag2 = true;
                }
              }
              if (this.is_Contains_Redaction)
              {
                if (!flag2)
                  break;
              }
              PdfImageInfo pdfImageInfo = new PdfImageInfo();
              pdfImageInfo.Name = operands[0].Replace("/", "").ToString();
              pdfImageInfo.IsImageExtracted = true;
              pdfImageInfo.Metadata = this.GetMetadata(imageDictionary2);
              imageStructure.IsImageForExtraction = true;
              Image embeddedImage = imageStructure.EmbeddedImage;
              pdfImageInfo.m_isImageMasked = imageStructure.IsImageMasked;
              pdfImageInfo.m_isImageInterpolated = imageStructure.IsImageInterpolated;
              pdfImageInfo.m_isSoftMasked = imageStructure.IsSoftMasked;
              if ((double) num == 270.0)
              {
                embeddedImage.RotateFlip(RotateFlipType.Rotate270FlipNone);
                this.isRotate = true;
              }
              else if ((double) num == 90.0)
              {
                embeddedImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
                this.isRotate = true;
              }
              else if ((double) num == 180.0)
              {
                embeddedImage.RotateFlip(RotateFlipType.Rotate180FlipNone);
                this.isRotate = true;
              }
              pdfImageInfo.IsImageExtracted = true;
              if (this.m_extractedImagesBounds.Contains(rectangleF2) && this.m_ImageKey.Contains(pdfImageInfo.Name))
              {
                this.RegenerateImages(embeddedImage, pdfImageInfo.Name);
                break;
              }
              if (this.isRotate)
                this.m_roatedImages.Add(new PdfName(pdfImageInfo.Name), num);
              if (embeddedImage != null)
              {
                this.extractedImages.Add((object) embeddedImage);
                this.m_extractedImagesBounds.Add(rectangleF2);
                this.maskImageCollection.Add(flag1);
                this.extractedImageMatrix.Add(pdfMatrix);
                this.m_imageInfoList.Add(pdfImageInfo);
                this.m_ImageKey.Add(pdfImageInfo.Name);
                break;
              }
              break;
            }
            catch (Exception ex)
            {
              if (ex.Message.Equals("Document contains one or more images with unsupported encoding."))
                throw new Exception(ex.Message);
              break;
            }
          case "Q":
            this.m_currentMatrix.Pop();
            break;
        }
      }
    }
    this.m_imageinfo = this.m_imageInfoList.ToArray();
    Image[] images = new Image[this.extractedImages.Count];
    this.extractedImages.CopyTo((System.Array) images);
    int index4 = 0;
    int index5 = 0;
    IEnumerator enumerator = (IEnumerator) this.m_extractedImagesBounds.GetEnumerator();
    while (enumerator.MoveNext())
    {
      if (!this.m_imageinfo[index4].IsImageExtracted)
      {
        this.m_imageinfo[index4].Bounds = (RectangleF) enumerator.Current;
        this.m_imageinfo[index4].Image = (Image) null;
        this.m_imageinfo[index4].Index = index4;
        this.m_imageinfo[index4].Matrix = this.extractedImageMatrix[index4];
        this.m_imageinfo[index4].MaskImage = this.maskImageCollection[index4];
        ++index4;
      }
      else if (this.m_imageinfo[index4].IsImageExtracted)
      {
        Image image = images[index5];
        this.m_imageinfo[index4].Bounds = (RectangleF) enumerator.Current;
        this.m_imageinfo[index4].Image = image;
        this.m_imageinfo[index4].Index = index4;
        this.m_imageinfo[index4].Matrix = this.extractedImageMatrix[index4];
        this.m_imageinfo[index4].MaskImage = this.maskImageCollection[index4];
        ++index4;
        ++index5;
      }
    }
    Thread.CurrentThread.CurrentCulture = currentCulture;
    this.isExtractImages = false;
    contentParser?.Dispose();
    this.ClearImageResources();
    return images;
  }

  internal void ExtractInnerXObjectImages(PdfStream contentstream, PdfPageResources childResources)
  {
    float num = 0.0f;
    PdfDictionary pdfDictionary1 = new PdfDictionary();
    PdfStream pdfStream1 = (PdfStream) null;
    string empty = string.Empty;
    RectangleF rectangleF1 = new RectangleF();
    PdfRecordCollection recordCollection = new ContentParser(contentstream.InternalStream.ToArray()).ReadContent();
    for (int index1 = 0; index1 < recordCollection.RecordCollection.Count; ++index1)
    {
      string str = recordCollection.RecordCollection[index1].OperatorName;
      string[] operands = recordCollection.RecordCollection[index1].Operands;
      foreach (char symbolChar in this.m_symbolChars)
      {
        if (str.Contains(symbolChar.ToString()))
          str = str.Replace(symbolChar.ToString(), "");
      }
      switch (str.Trim())
      {
        case "q":
          GraphicsStateData graphicsStateData1 = new GraphicsStateData();
          if (this.m_currentMatrix.Count > 0)
          {
            GraphicsStateData graphicsStateData2 = this.m_currentMatrix.Peek();
            graphicsStateData1.m_drawing2dMatrixCTM = graphicsStateData2.m_drawing2dMatrixCTM;
          }
          this.m_currentMatrix.Push(graphicsStateData1);
          break;
        case "cm":
          System.Drawing.Drawing2D.Matrix matrix1 = new System.Drawing.Drawing2D.Matrix(float.Parse(operands[0]), float.Parse(operands[1]), float.Parse(operands[2]), float.Parse(operands[3]), float.Parse(operands[4]), float.Parse(operands[5]));
          this.m_currentMatrix.Peek().m_drawing2dMatrixCTM = this.Multiply(matrix1, this.m_currentMatrix.Peek().m_drawing2dMatrixCTM);
          break;
        case "Do":
          ImageStructure imageStructure = (ImageStructure) null;
          string key = operands[0].Replace("/", "");
          if (childResources != null && childResources.Resources.Count > 0 && !(childResources.Resources[operands[0].Replace("/", "")] is ImageStructure))
          {
            XObjectElement childResource = childResources[operands[0].Replace("/", "")] as XObjectElement;
            PdfStream xobjectDictionary = childResource.XObjectDictionary as PdfStream;
            xobjectDictionary.Decompress();
            PdfDictionary pdfDictionary2 = new PdfDictionary();
            PdfPageResources pdfPageResources = new PdfPageResources();
            if (childResource.XObjectDictionary.ContainsKey("Resources"))
            {
              PdfDictionary pdfDictionary3 = (object) (childResource.XObjectDictionary["Resources"] as PdfReference) == null ? ((object) (childResource.XObjectDictionary["Resources"] as PdfReferenceHolder) == null ? childResource.XObjectDictionary["Resources"] as PdfDictionary : (childResource.XObjectDictionary["Resources"] as PdfReferenceHolder).Object as PdfDictionary) : (childResource.XObjectDictionary["Resources"] as PdfReferenceHolder).Object as PdfDictionary;
              System.Collections.Generic.Dictionary<string, PdfMatrix> commonMatrix = new System.Collections.Generic.Dictionary<string, PdfMatrix>();
              pdfPageResources = this.m_resourceLoader.UpdatePageResources(this.m_resourceLoader.UpdatePageResources(this.m_resourceLoader.UpdatePageResources(this.m_resourceLoader.UpdatePageResources(this.m_resourceLoader.UpdatePageResources(this.m_resourceLoader.UpdatePageResources(pdfPageResources, this.m_resourceLoader.GetImageResources(pdfDictionary3, (PdfPageBase) null, ref commonMatrix)), this.m_resourceLoader.GetFontResources(pdfDictionary3)), this.m_resourceLoader.GetExtendedGraphicResources(pdfDictionary3)), this.m_resourceLoader.GetColorSpaceResource(pdfDictionary3)), this.m_resourceLoader.GetShadingResource(pdfDictionary3)), this.m_resourceLoader.GetPatternResource(pdfDictionary3));
            }
            this.ExtractInnerXObjectImages(xobjectDictionary, pdfPageResources);
            break;
          }
          if (childResources != null)
          {
            if (childResources.Resources.Count > 0)
            {
              if (childResources.Resources[operands[0].Replace("/", "")] is ImageStructure)
                imageStructure = childResources.Resources[operands[0].Replace("/", "")] as ImageStructure;
            }
          }
          try
          {
            PdfDictionary imageDictionary1 = imageStructure.ImageDictionary;
            if (imageDictionary1 != null)
            {
              PdfArray pdfArray1 = (PdfArray) null;
              PdfArray pdfArray2 = (PdfArray) null;
              if (imageDictionary1["ColorSpace"] is PdfArray)
                pdfArray1 = imageDictionary1["ColorSpace"] as PdfArray;
              if ((object) (imageDictionary1["ColorSpace"] as PdfReferenceHolder) != null)
                pdfArray1 = (imageDictionary1["ColorSpace"] as PdfReferenceHolder).Object as PdfArray;
              if (pdfArray1 != null && (object) (pdfArray1[1] as PdfReferenceHolder) != null)
                pdfArray2 = (pdfArray1[1] as PdfReferenceHolder).Object as PdfArray;
              if (pdfArray2 != null && (object) (pdfArray2[1] as PdfReferenceHolder) != null)
              {
                IPdfPrimitive pdfPrimitive = (pdfArray2[1] as PdfReferenceHolder).Object;
              }
            }
            PdfStream imageDictionary2 = imageStructure.ImageDictionary as PdfStream;
            PdfMatrix pdfMatrix = imageStructure.ImageInfo;
            for (int index2 = 0; index2 < this.Contents.Count; ++index2)
            {
              PdfStream pdfStream2 = pdfStream1 == null ? (this.Contents[index2] as PdfReferenceHolder).Object as PdfStream : pdfStream1;
              pdfStream2.Decompress();
              MemoryStream internalStream = pdfStream2.InternalStream;
              internalStream.Position = 0L;
              PdfReader ContentStream = new PdfReader((Stream) internalStream);
              ContentStream.Position = 0L;
              if (ContentStream.ReadStream().Contains(key))
              {
                pdfMatrix = this.Rotation != PdfPageRotateAngle.RotateAngle90 ? new PdfMatrix(ContentStream, key, this.Size) : new PdfMatrix(ContentStream, key, this.Size);
                break;
              }
            }
            bool flag = false;
            if (imageDictionary1.ContainsKey("Mask"))
              flag = true;
            if (imageDictionary2.ContainsKey("Width"))
            {
              if (imageDictionary2["Width"] is PdfNumber)
              {
                int intValue1 = (imageDictionary2["Width"] as PdfNumber).IntValue;
              }
              if ((object) (imageDictionary2["Width"] as PdfReferenceHolder) != null)
              {
                int intValue2 = ((imageDictionary2["Width"] as PdfReferenceHolder).Object as PdfNumber).IntValue;
              }
              if (imageDictionary2["Height"] is PdfNumber)
              {
                int intValue3 = (imageDictionary2["Height"] as PdfNumber).IntValue;
              }
              if ((object) (imageDictionary2["Height"] as PdfReferenceHolder) != null)
              {
                int intValue4 = ((imageDictionary2["Height"] as PdfReferenceHolder).Object as PdfNumber).IntValue;
              }
            }
            else if (imageDictionary2.ContainsKey("BBox"))
            {
              PdfArray pdfArray = imageDictionary2["BBox"] as PdfArray;
              int intValue5 = (pdfArray[2] as PdfNumber).IntValue;
              int intValue6 = (pdfArray[3] as PdfNumber).IntValue;
            }
            System.Drawing.Drawing2D.Matrix matrix = this.Multiply(this.Multiply(new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, -1.01f, 0.0f, 1f), this.m_currentMatrix.Peek().m_drawing2dMatrixCTM), new System.Drawing.Drawing2D.Matrix(1.33333337f, 0.0f, 0.0f, -1.33333337f, 0.0f, this.Size.Height * 1.33333337f));
            RectangleF rectangleF2;
            if (this.Rotation == PdfPageRotateAngle.RotateAngle270)
              rectangleF2 = (double) matrix.Elements[0] == 0.0 || (double) matrix.Elements[3] == 0.0 ? new RectangleF((float) Math.Floor((double) matrix.OffsetY / 1.3333333730697632), (float) (Math.Floor((double) this.Size.Width) - (Math.Round((double) (matrix.OffsetX / 1.33333337f), 5) + Math.Floor((double) matrix.Elements[0] / 1.3333333730697632))), Math.Abs(this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[1]), Math.Abs(this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[2])) : new RectangleF((float) Math.Floor((double) matrix.OffsetY / 1.3333333730697632), this.Size.Width - (float) (Math.Round((double) (matrix.OffsetX / 1.33333337f), 5) + (double) matrix.Elements[0] / 1.3333333730697632), this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[3], this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[0]);
            else if (this.Rotation == PdfPageRotateAngle.RotateAngle90)
            {
              if ((double) matrix.Elements[0] == 0.0 && (double) matrix.Elements[3] == 0.0)
              {
                rectangleF2 = new RectangleF(new PointF(this.Size.Height - matrix.Elements[5] / 1.33333337f, matrix.Elements[4] / 1.33333337f), new SizeF(this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[1], -this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[2]));
              }
              else
              {
                num = 90f;
                rectangleF2 = new RectangleF(new PointF(this.Size.Height - matrix.OffsetY / 1.33333337f - this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[3], matrix.Elements[4] / 1.33333337f), new SizeF(this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[3], this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[0]));
              }
            }
            else if (this.Rotation == PdfPageRotateAngle.RotateAngle180)
              rectangleF2 = new RectangleF(this.Size.Width - matrix.OffsetX / 1.33333337f - this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[0], (float) ((double) this.Size.Height - (double) this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[3] - Math.Round((double) (matrix.OffsetY / 1.33333337f), 5)), this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[0], this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[3]);
            else if ((double) matrix.Elements[0] == 0.0 && (double) matrix.Elements[3] == 0.0)
            {
              if ((double) matrix.Elements[1] < 0.0 && (double) matrix.Elements[2] > 0.0)
              {
                num = 270f;
                rectangleF2 = new RectangleF(this.Size.Height - matrix.Elements[5] / 1.33333337f, (float) Math.Round((double) (matrix.Elements[4] / 1.33333337f), 5), -this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[2], this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[1]);
              }
              else if ((double) matrix.Elements[1] > 0.0 && (double) matrix.Elements[2] < 0.0)
              {
                num = 90f;
                rectangleF2 = new RectangleF(matrix.Elements[5] / 1.33333337f, (float) Math.Round((double) (this.Size.Width - matrix.Elements[4] / 1.33333337f), 5), -this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[2], -this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[1]);
              }
              else if ((double) matrix.Elements[1] < 0.0 && (double) matrix.Elements[2] < 0.0)
              {
                num = 180f;
                rectangleF2 = new RectangleF(this.Size.Width - matrix.OffsetX / 1.33333337f - this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[0], (float) ((double) this.Size.Height - (double) this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[3] - Math.Round((double) (matrix.OffsetY / 1.33333337f), 5)), this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[0], this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[3]);
              }
              else
                rectangleF2 = new RectangleF(this.Size.Width - matrix.OffsetX / 1.33333337f - this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[0], (float) ((double) this.Size.Height - (double) this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[3] - Math.Round((double) (matrix.OffsetY / 1.33333337f), 5)), this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[0], this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[3]);
            }
            else
              rectangleF2 = new RectangleF(matrix.OffsetX / 1.33333337f, (float) Math.Round((double) (matrix.OffsetY / 1.33333337f), 5), this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[0], this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[3]);
            PdfImageInfo pdfImageInfo = new PdfImageInfo();
            pdfImageInfo.Name = operands[0].Replace("/", "").ToString();
            pdfImageInfo.IsImageExtracted = true;
            imageStructure.IsImageForExtraction = true;
            pdfImageInfo.Metadata = this.GetMetadata(imageDictionary2);
            Image embeddedImage = imageStructure.EmbeddedImage;
            pdfImageInfo.m_isImageMasked = imageStructure.IsImageMasked;
            pdfImageInfo.m_isImageInterpolated = imageStructure.IsImageInterpolated;
            pdfImageInfo.m_isSoftMasked = imageStructure.IsSoftMasked;
            if ((double) num == 270.0)
            {
              embeddedImage.RotateFlip(RotateFlipType.Rotate270FlipNone);
              this.isRotate = true;
            }
            else if ((double) num == 90.0)
            {
              embeddedImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
              this.isRotate = true;
            }
            else if ((double) num == 180.0)
            {
              embeddedImage.RotateFlip(RotateFlipType.Rotate180FlipNone);
              this.isRotate = true;
            }
            if (this.m_extractedImagesBounds.Contains(rectangleF2) && this.m_ImageKey.Contains(pdfImageInfo.Name))
            {
              this.RegenerateImages(embeddedImage, pdfImageInfo.Name);
              break;
            }
            if (embeddedImage != null)
            {
              this.extractedImages.Add((object) embeddedImage);
              this.m_extractedImagesBounds.Add(rectangleF2);
              this.maskImageCollection.Add(flag);
              this.extractedImageMatrix.Add(pdfMatrix);
              this.m_imageInfoList.Add(pdfImageInfo);
              this.m_ImageKey.Add(pdfImageInfo.Name);
              break;
            }
            break;
          }
          catch (Exception ex)
          {
            break;
          }
        case "Q":
          this.m_currentMatrix.Pop();
          break;
      }
    }
  }

  internal void CombineContent(Stream stream)
  {
    bool flag = this is PdfLoadedPage;
    PdfLoadedPage pdfLoadedPage = this as PdfLoadedPage;
    byte[] buffer1 = PdfString.StringToByte("\r\n");
    if (pdfLoadedPage != null)
    {
      for (int index = 0; index < pdfLoadedPage.Contents.Count; ++index)
      {
        PdfStream pdfStream = (PdfStream) null;
        IPdfPrimitive content = pdfLoadedPage.Contents[index];
        if ((object) (content as PdfReferenceHolder) != null)
          pdfStream = (pdfLoadedPage.Contents[index] as PdfReferenceHolder).Object as PdfStream;
        else if (content is PdfStream)
          pdfStream = content as PdfStream;
        if (pdfStream != null)
        {
          if (flag)
          {
            byte[] decompressedData = pdfStream.GetDecompressedData();
            using (MemoryStream memoryStream = new MemoryStream(decompressedData))
            {
              byte[] buffer2 = new byte[32 /*0x20*/];
              memoryStream.Position = 0L;
              int count;
              while ((count = memoryStream.Read(buffer2, 0, buffer2.Length)) > 0)
                stream.Write(buffer2, 0, count);
            }
            System.Array.Clear((System.Array) decompressedData, 0, decompressedData.Length);
          }
          stream.Write(buffer1, 0, buffer1.Length);
        }
      }
    }
    else
      this.Layers.CombineContent(stream);
  }

  public Image[] ExtractImages()
  {
    this.isExtractImages = true;
    CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
    System.Drawing.Drawing2D.Matrix currentMatrix = (System.Drawing.Drawing2D.Matrix) null;
    this.m_pageResources = PageResourceLoader.Instance.GetPageResources(this);
    this.m_currentMatrix.Push(new GraphicsStateData()
    {
      m_drawing2dMatrixCTM = new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, 1f, 0.0f, 0.0f)
    });
    if (PageResourceLoader.Instance.HasImages)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        this.Layers.CombineContent((Stream) memoryStream);
        memoryStream.Position = 0L;
        this.m_recordCollection = new ContentParser(memoryStream.ToArray()).ReadContent();
      }
      for (int index = 0; index < this.m_recordCollection.RecordCollection.Count; ++index)
        this.ParseContentStream(index, currentMatrix, (List<string>) null);
    }
    this.m_imageinfo = this.m_imageInfoList.ToArray();
    Image[] images = new Image[this.extractedImages.Count];
    this.extractedImages.CopyTo((System.Array) images);
    int index1 = 0;
    int index2 = 0;
    IEnumerator enumerator = (IEnumerator) this.m_extractedImagesBounds.GetEnumerator();
    while (enumerator.MoveNext())
    {
      if (!this.m_imageinfo[index1].IsImageExtracted)
      {
        this.m_imageinfo[index1].Bounds = (RectangleF) enumerator.Current;
        this.m_imageinfo[index1].Image = (Image) null;
        this.m_imageinfo[index1].Index = index1;
        ++index1;
      }
      else if (this.m_imageinfo[index1].IsImageExtracted)
      {
        Image image = images[index2];
        this.m_imageinfo[index1].Bounds = (RectangleF) enumerator.Current;
        this.m_imageinfo[index1].Image = image;
        this.m_imageinfo[index1].Index = index1;
        ++index1;
        ++index2;
      }
    }
    Thread.CurrentThread.CurrentCulture = currentCulture;
    this.isExtractImages = false;
    PageResourceLoader.Instance.HasImages = false;
    return images;
  }

  private PdfDictionary GetObject(IPdfPrimitive primitive)
  {
    PdfDictionary pdfDictionary = (PdfDictionary) null;
    if (primitive is PdfDictionary)
      pdfDictionary = primitive as PdfDictionary;
    else if ((object) (primitive as PdfReferenceHolder) != null)
      pdfDictionary = (primitive as PdfReferenceHolder).Object as PdfDictionary;
    return pdfDictionary;
  }

  private void RemovedResourceImage(string imageName)
  {
    PdfDictionary dictionary = this.Dictionary;
    if (!dictionary.ContainsKey("Resources") || !(dictionary["Resources"] is PdfDictionary pdfDictionary1) || !pdfDictionary1.ContainsKey("XObject"))
      return;
    PdfDictionary baseDictionary = (PdfDictionary) null;
    IPdfPrimitive pdfPrimitive = this.GetXObject(this.GetResources());
    while (true)
    {
      do
      {
        if (pdfPrimitive != null && pdfPrimitive is PdfDictionary)
        {
          System.Collections.Generic.Dictionary<PdfName, IPdfPrimitive> items = ((PdfDictionary) pdfPrimitive).Items;
          foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in items)
          {
            if ((object) (keyValuePair.Value as PdfReferenceHolder) != null)
            {
              PdfStream pdfStream = ((PdfReferenceHolder) keyValuePair.Value).Object as PdfStream;
              PdfDictionary pdfDictionary2 = (PdfDictionary) pdfStream;
              if (pdfStream.ContainsKey("Subtype") && (pdfStream["Subtype"] as PdfName).Value == "Image" && keyValuePair.Key.Value == imageName)
              {
                items.Remove(keyValuePair.Key);
                break;
              }
              if (pdfDictionary2.ContainsKey("Resources"))
                baseDictionary = pdfDictionary2["Resources"] as PdfDictionary;
            }
          }
        }
        if (baseDictionary == null || !(pdfPrimitive is PdfDictionary) || (pdfPrimitive as PdfDictionary).Items.Count == 0 || pdfPrimitive == null || !baseDictionary.ContainsKey("XObject"))
          goto label_18;
      }
      while (!baseDictionary.ContainsKey("XObject"));
      if (PdfCrossTable.Dereference(baseDictionary["XObject"]) is PdfDictionary)
      {
        IPdfPrimitive xobject = this.GetXObject(new PdfResources(baseDictionary));
        if (pdfPrimitive != xobject)
          pdfPrimitive = xobject;
        else
          goto label_20;
      }
      else
        goto label_19;
    }
label_18:
    return;
label_19:
    return;
label_20:;
  }

  private PdfArray GetArrayFromReferenceHolder(IPdfPrimitive primitive)
  {
    if ((object) (primitive as PdfReferenceHolder) == null)
      return primitive as PdfArray;
    PdfReferenceHolder pdfReferenceHolder = primitive as PdfReferenceHolder;
    return (object) (pdfReferenceHolder.Object as PdfReferenceHolder) != null ? this.GetArrayFromReferenceHolder(pdfReferenceHolder.Object) : pdfReferenceHolder.Object as PdfArray;
  }

  public void RemoveImage(PdfImageInfo imageInfo)
  {
    PdfDictionary dictionary = this.Dictionary;
    MemoryStream input = new MemoryStream();
    this.Layers.CombineContent((Stream) input);
    PdfPageResources pageResources = PageResourceLoader.Instance.GetPageResources(this);
    PdfStream pdfStream = this.RemovedImageObject(input, imageInfo.Name, pageResources);
    if (dictionary.ContainsKey("Contents"))
    {
      PdfArray fromReferenceHolder = this.GetArrayFromReferenceHolder(dictionary["Contents"]);
      pdfStream.Compress = true;
      if (fromReferenceHolder != null)
      {
        foreach (IPdfPrimitive content in this.Contents)
        {
          if (PdfCrossTable.Dereference(content) is PdfDictionary pdfDictionary)
            pdfDictionary.isSkip = true;
        }
        fromReferenceHolder.Clear();
        fromReferenceHolder.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfStream));
      }
      else
      {
        if (dictionary["Contents"] is PdfStream stream)
        {
          stream.Clear();
          stream.Items.Remove(new PdfName("Length"));
          stream.Data = pdfStream.Data;
          stream.Compress = true;
        }
        this.Graphics.StreamWriter = new PdfStreamWriter(stream);
      }
    }
    this.RemovedResourceImage(imageInfo.Name);
    if (!(this is PdfLoadedPage) || (this as PdfLoadedPage).Document == null || !((this as PdfLoadedPage).Document is PdfLoadedDocument))
      return;
    ((this as PdfLoadedPage).Document as PdfLoadedDocument).FileStructure.IncrementalUpdate = false;
  }

  private PdfStream RemovedImageObject(
    MemoryStream input,
    string Name,
    PdfPageResources pageResources)
  {
    PdfRecordCollection recordCollection = new ContentParser(input.ToArray()).ReadContent();
    for (int index = 0; index < recordCollection.RecordCollection.Count; ++index)
    {
      string str = recordCollection.RecordCollection[index].OperatorName;
      string[] operands = recordCollection.RecordCollection[index].Operands;
      foreach (char symbolChar in this.m_symbolChars)
      {
        if (str.Contains(symbolChar.ToString()))
          str = str.Replace(symbolChar.ToString(), "");
      }
      switch (str.Trim())
      {
        case "Do":
          if (pageResources.ContainsKey(operands[0].Replace("/", "")))
          {
            if (pageResources[operands[0].Replace("/", "")] is XObjectElement)
            {
              string empty = string.Empty;
              XObjectElement pageResource = pageResources[operands[0].Replace("/", "")] as XObjectElement;
              if (pageResource.ObjectType == "Form")
              {
                PdfStream xobjectDictionary1 = pageResource.XObjectDictionary as PdfStream;
                xobjectDictionary1.Decompress();
                PageResourceLoader pageResourceLoader = new PageResourceLoader();
                PdfDictionary pdfDictionary1 = new PdfDictionary();
                PdfDictionary xobjectDictionary2 = pageResource.XObjectDictionary;
                PdfPageResources pageResources1 = new PdfPageResources();
                if (xobjectDictionary2.ContainsKey("Resources"))
                {
                  PdfDictionary pdfDictionary2 = (object) (xobjectDictionary2["Resources"] as PdfReference) == null ? ((object) (xobjectDictionary2["Resources"] as PdfReferenceHolder) == null ? pageResource.XObjectDictionary["Resources"] as PdfDictionary : (xobjectDictionary2["Resources"] as PdfReferenceHolder).Object as PdfDictionary) : (xobjectDictionary2["Resources"] as PdfReferenceHolder).Object as PdfDictionary;
                  System.Collections.Generic.Dictionary<string, PdfMatrix> commonMatrix = new System.Collections.Generic.Dictionary<string, PdfMatrix>();
                  PdfPageResources pageResources2 = pageResourceLoader.UpdatePageResources(pageResources1, pageResourceLoader.GetImageResources(pdfDictionary2, (PdfPageBase) null, ref commonMatrix));
                  PdfPageResources pageResources3 = pageResourceLoader.UpdatePageResources(pageResources2, pageResourceLoader.GetFontResources(pdfDictionary2));
                  PdfPageResources pageResources4 = pageResourceLoader.UpdatePageResources(pageResources3, pageResourceLoader.GetExtendedGraphicResources(pdfDictionary2));
                  PdfPageResources pageResources5 = pageResourceLoader.UpdatePageResources(pageResources4, pageResourceLoader.GetColorSpaceResource(pdfDictionary2));
                  PdfPageResources pageResources6 = pageResourceLoader.UpdatePageResources(pageResources5, pageResourceLoader.GetShadingResource(pdfDictionary2));
                  pageResources1 = pageResourceLoader.UpdatePageResources(pageResources6, pageResourceLoader.GetPatternResource(pdfDictionary2));
                }
                PdfStream pdfStream1 = this.RemovedImageObject(xobjectDictionary1.InternalStream, Name, pageResources1);
                PdfStream pdfStream2 = new PdfStream();
                pdfStream2.Data = pdfStream1.Data;
                pdfStream2.Compress = true;
                pdfStream1.Dispose();
                xobjectDictionary1.Clear();
                xobjectDictionary1.Items.Remove(new PdfName("Length"));
                xobjectDictionary1.Data = pdfStream2.Data;
                xobjectDictionary1.Compress = true;
                xobjectDictionary1.Modify();
                break;
              }
              break;
            }
            if (operands[0].Replace("/", "") == Name)
            {
              recordCollection.Remove(recordCollection.RecordCollection[index]);
              break;
            }
            break;
          }
          if (operands[0].Replace("/", "") == Name)
          {
            recordCollection.Remove(recordCollection.RecordCollection[index]);
            break;
          }
          break;
      }
    }
    PdfStream stream = new PdfStream();
    for (int i = 0; i < recordCollection.RecordCollection.Count; ++i)
      this.OptimizeContent(recordCollection, i, (string) null, stream);
    return stream;
  }

  internal void OptimizeContent(
    PdfRecordCollection recordCollection,
    int i,
    string updatedText,
    PdfStream stream)
  {
    int count = recordCollection.RecordCollection.Count;
    PdfRecord record = recordCollection.RecordCollection[i];
    if (record.Operands != null && record.Operands.Length >= 1)
    {
      if (record.OperatorName == "ID")
      {
        StringBuilder stringBuilder = new StringBuilder();
        for (int index = 0; index < record.Operands.Length; ++index)
        {
          if (index + 1 < record.Operands.Length && record.Operands[index].Contains("/") && record.Operands[index + 1].Contains("/"))
          {
            stringBuilder.Append(record.Operands[index]);
            stringBuilder.Append(" ");
            stringBuilder.Append(record.Operands[index + 1]);
            stringBuilder.Append("\r\n");
            ++index;
          }
          else if (index + 1 < record.Operands.Length && record.Operands[index].Contains("/"))
          {
            stringBuilder.Append(record.Operands[index]);
            stringBuilder.Append(" ");
            stringBuilder.Append(record.Operands[index + 1]);
            stringBuilder.Append("\r\n");
            ++index;
          }
          else
          {
            stringBuilder.Append(record.Operands[index]);
            stringBuilder.Append("\r\n");
          }
        }
        byte[] bytes = Encoding.Default.GetBytes(stringBuilder.ToString());
        stream.Write(bytes);
      }
      else
      {
        for (int index = 0; index < record.Operands.Length; ++index)
        {
          string str = record.Operands[index];
          if ((record.OperatorName == "Tj" || record.OperatorName == "'" || record.OperatorName == "\"" || record.OperatorName == "TJ") && updatedText != null)
          {
            str = updatedText;
            if (record.OperatorName == "'")
            {
              stream.Write("T*");
              stream.Write(" ");
            }
            record.OperatorName = "TJ";
          }
          PdfString pdfString = new PdfString(str);
          stream.Write(pdfString.Bytes);
          if (record.OperatorName != "Tj" && record.OperatorName != "'" && record.OperatorName != "\"" && record.OperatorName != "TJ")
            stream.Write(" ");
        }
      }
    }
    else if (record.Operands == null && record.InlineImageBytes != null)
    {
      byte[] bytes = Encoding.Default.GetBytes(Encoding.Default.GetString(record.InlineImageBytes));
      stream.Write(bytes);
      stream.Write(" ");
    }
    stream.Write(record.OperatorName);
    if (i + 1 >= count)
      return;
    if (record.OperatorName == "ID")
      stream.Write("\n");
    else if (i + 1 < count && (record.OperatorName == "W" || record.OperatorName == "W*") && recordCollection.RecordCollection[i + 1].OperatorName == "n")
      stream.Write(" ");
    else if (record.OperatorName == "w" || record.OperatorName == "EI")
      stream.Write(" ");
    else
      stream.Write("\r\n");
  }

  protected bool ParseContentStream(int index, System.Drawing.Drawing2D.Matrix currentMatrix, List<string> keys)
  {
    bool contentStream = false;
    float num = 0.0f;
    string str = this.m_recordCollection.RecordCollection[index].OperatorName;
    string[] operands = this.m_recordCollection.RecordCollection[index].Operands;
    foreach (char symbolChar in this.m_symbolChars)
    {
      if (str.Contains(symbolChar.ToString()))
        str = str.Replace(symbolChar.ToString(), "");
    }
    switch (str.Trim())
    {
      case "q":
        GraphicsStateData graphicsStateData1 = new GraphicsStateData();
        if (this.m_currentMatrix.Count > 0)
        {
          GraphicsStateData graphicsStateData2 = this.m_currentMatrix.Peek();
          graphicsStateData1.m_drawing2dMatrixCTM = graphicsStateData2.m_drawing2dMatrixCTM;
        }
        this.m_currentMatrix.Push(graphicsStateData1);
        break;
      case "cm":
        currentMatrix = new System.Drawing.Drawing2D.Matrix(float.Parse(operands[0]), float.Parse(operands[1]), float.Parse(operands[2]), float.Parse(operands[3]), float.Parse(operands[4]), float.Parse(operands[5]));
        this.m_currentMatrix.Peek().m_drawing2dMatrixCTM = this.Multiply(currentMatrix, this.m_currentMatrix.Peek().m_drawing2dMatrixCTM);
        break;
      case "Do":
        string key = operands[0].Replace("/", "");
        if (this.isExtractImages)
        {
          ImageStructure imageStructure = (ImageStructure) null;
          if (!(this.m_pageResources.Resources[key] is ImageStructure))
          {
            XObjectElement pageResource = this.m_pageResources[key] as XObjectElement;
            PdfStream xobjectDictionary = pageResource.XObjectDictionary as PdfStream;
            xobjectDictionary.Decompress();
            PdfDictionary pdfDictionary1 = new PdfDictionary();
            PdfPageResources pdfPageResources = new PdfPageResources();
            if (pageResource.XObjectDictionary.ContainsKey("Resources"))
            {
              PdfDictionary pdfDictionary2 = (object) (pageResource.XObjectDictionary["Resources"] as PdfReference) == null ? ((object) (pageResource.XObjectDictionary["Resources"] as PdfReferenceHolder) == null ? pageResource.XObjectDictionary["Resources"] as PdfDictionary : (pageResource.XObjectDictionary["Resources"] as PdfReferenceHolder).Object as PdfDictionary) : (pageResource.XObjectDictionary["Resources"] as PdfReferenceHolder).Object as PdfDictionary;
              System.Collections.Generic.Dictionary<string, PdfMatrix> commonMatrix = new System.Collections.Generic.Dictionary<string, PdfMatrix>();
              pdfPageResources = this.m_resourceLoader.UpdatePageResources(this.m_resourceLoader.UpdatePageResources(this.m_resourceLoader.UpdatePageResources(this.m_resourceLoader.UpdatePageResources(this.m_resourceLoader.UpdatePageResources(this.m_resourceLoader.UpdatePageResources(pdfPageResources, this.m_resourceLoader.GetImageResources(pdfDictionary2, (PdfPageBase) null, ref commonMatrix)), this.m_resourceLoader.GetFontResources(pdfDictionary2)), this.m_resourceLoader.GetExtendedGraphicResources(pdfDictionary2)), this.m_resourceLoader.GetColorSpaceResource(pdfDictionary2)), this.m_resourceLoader.GetShadingResource(pdfDictionary2)), this.m_resourceLoader.GetPatternResource(pdfDictionary2));
            }
            this.ExtractInnerXObjectImages(xobjectDictionary, pdfPageResources);
            return true;
          }
          if (this.m_pageResources.Resources[key] is ImageStructure)
            imageStructure = this.m_pageResources.Resources[key] as ImageStructure;
          try
          {
            PdfDictionary imageDictionary1 = imageStructure.ImageDictionary;
            if (imageDictionary1 != null)
            {
              PdfArray pdfArray1 = (PdfArray) null;
              PdfArray pdfArray2 = (PdfArray) null;
              if (imageDictionary1["ColorSpace"] is PdfArray)
                pdfArray1 = imageDictionary1["ColorSpace"] as PdfArray;
              if ((object) (imageDictionary1["ColorSpace"] as PdfReferenceHolder) != null)
                pdfArray1 = (imageDictionary1["ColorSpace"] as PdfReferenceHolder).Object as PdfArray;
              if (pdfArray1 != null && (object) (pdfArray1[1] as PdfReferenceHolder) != null)
                pdfArray2 = (pdfArray1[1] as PdfReferenceHolder).Object as PdfArray;
              if (pdfArray2 != null && (object) (pdfArray2[1] as PdfReferenceHolder) != null)
              {
                IPdfPrimitive pdfPrimitive = (pdfArray2[1] as PdfReferenceHolder).Object;
              }
            }
            PdfStream imageDictionary2 = imageStructure.ImageDictionary as PdfStream;
            SizeF xobjectSize = this.GetXObjectSize(imageDictionary2);
            double width = (double) xobjectSize.Width;
            double height = (double) xobjectSize.Height;
            System.Drawing.Drawing2D.Matrix matrix = this.Multiply(this.Multiply(new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, -1.01f, 0.0f, 1f), this.m_currentMatrix.Peek().m_drawing2dMatrixCTM), new System.Drawing.Drawing2D.Matrix(1.33333337f, 0.0f, 0.0f, -1.33333337f, 0.0f, this.Size.Height * 1.33333337f));
            RectangleF rectangleF;
            if (this.Rotation == PdfPageRotateAngle.RotateAngle270)
              rectangleF = (double) matrix.Elements[0] == 0.0 || (double) matrix.Elements[3] == 0.0 ? new RectangleF((float) Math.Floor((double) matrix.OffsetY / 1.3333333730697632), (float) (Math.Floor((double) this.Size.Width) - (Math.Round((double) (matrix.OffsetX / 1.33333337f), 5) + Math.Floor((double) matrix.Elements[0] / 1.3333333730697632))), Math.Abs(this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[1]), Math.Abs(this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[2])) : new RectangleF((float) Math.Floor((double) matrix.OffsetY / 1.3333333730697632), this.Size.Width - (float) (Math.Round((double) (matrix.OffsetX / 1.33333337f), 5) + (double) matrix.Elements[0] / 1.3333333730697632), this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[3], this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[0]);
            else if (this.Rotation == PdfPageRotateAngle.RotateAngle90)
            {
              if ((double) matrix.Elements[0] == 0.0 && (double) matrix.Elements[3] == 0.0)
              {
                rectangleF = new RectangleF(new PointF(this.Size.Height - matrix.Elements[5] / 1.33333337f, matrix.Elements[4] / 1.33333337f), new SizeF(this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[1], -this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[2]));
              }
              else
              {
                num = 90f;
                rectangleF = new RectangleF(new PointF(this.Size.Height - matrix.OffsetY / 1.33333337f - this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[3], matrix.Elements[4] / 1.33333337f), new SizeF(this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[3], this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[0]));
              }
            }
            else if (this.Rotation == PdfPageRotateAngle.RotateAngle180)
              rectangleF = new RectangleF(this.Size.Width - matrix.OffsetX / 1.33333337f - this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[0], (float) ((double) this.Size.Height - (double) this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[3] - Math.Round((double) (matrix.OffsetY / 1.33333337f), 5)), this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[0], this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[3]);
            else if ((double) matrix.Elements[0] == 0.0 && (double) matrix.Elements[3] == 0.0)
            {
              if ((double) matrix.Elements[1] < 0.0 && (double) matrix.Elements[2] > 0.0)
              {
                num = 270f;
                rectangleF = new RectangleF(this.Size.Height - matrix.Elements[5] / 1.33333337f, (float) Math.Round((double) (matrix.Elements[4] / 1.33333337f), 5), -this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[2], this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[1]);
              }
              else if ((double) matrix.Elements[1] > 0.0 && (double) matrix.Elements[2] < 0.0)
              {
                num = 90f;
                rectangleF = new RectangleF(matrix.Elements[5] / 1.33333337f, (float) Math.Round((double) (this.Size.Width - matrix.Elements[4] / 1.33333337f), 5), -this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[2], -this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[1]);
              }
              else if ((double) matrix.Elements[1] < 0.0 && (double) matrix.Elements[2] < 0.0)
              {
                num = 180f;
                rectangleF = new RectangleF(this.Size.Width - matrix.OffsetX / 1.33333337f - this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[0], (float) ((double) this.Size.Height - (double) this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[3] - Math.Round((double) (matrix.OffsetY / 1.33333337f), 5)), this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[0], this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[3]);
              }
              else
                rectangleF = new RectangleF(this.Size.Width - matrix.OffsetX / 1.33333337f - this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[0], (float) ((double) this.Size.Height - (double) this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[3] - Math.Round((double) (matrix.OffsetY / 1.33333337f), 5)), this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[0], this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[3]);
            }
            else
              rectangleF = new RectangleF(matrix.OffsetX / 1.33333337f, (float) Math.Round((double) (matrix.OffsetY / 1.33333337f), 5), this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[0], this.m_currentMatrix.Peek().m_drawing2dMatrixCTM.Elements[3]);
            PdfImageInfo pdfImageInfo = new PdfImageInfo();
            pdfImageInfo.Name = key.ToString();
            pdfImageInfo.IsImageExtracted = true;
            pdfImageInfo.Metadata = this.GetMetadata(imageDictionary2);
            imageStructure.IsImageForExtraction = true;
            Image embeddedImage = imageStructure.EmbeddedImage;
            pdfImageInfo.m_isImageMasked = imageStructure.IsImageMasked;
            pdfImageInfo.m_isImageInterpolated = imageStructure.IsImageInterpolated;
            pdfImageInfo.m_isSoftMasked = imageStructure.IsSoftMasked;
            if ((double) num == 270.0)
            {
              embeddedImage.RotateFlip(RotateFlipType.Rotate270FlipNone);
              this.isRotate = true;
            }
            else if ((double) num == 90.0)
            {
              embeddedImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
              this.isRotate = true;
            }
            else if ((double) num == 180.0)
            {
              embeddedImage.RotateFlip(RotateFlipType.Rotate180FlipNone);
              this.isRotate = true;
            }
            pdfImageInfo.IsImageExtracted = true;
            if (this.m_extractedImagesBounds.Contains(rectangleF) && this.m_ImageKey.Contains(pdfImageInfo.Name))
            {
              this.RegenerateImages(embeddedImage, pdfImageInfo.Name);
              break;
            }
            if (this.isRotate)
              this.m_roatedImages.Add(new PdfName(pdfImageInfo.Name), num);
            if (embeddedImage != null)
            {
              this.extractedImages.Add((object) embeddedImage);
              this.m_extractedImagesBounds.Add(rectangleF);
              this.m_imageInfoList.Add(pdfImageInfo);
              this.m_ImageKey.Add(pdfImageInfo.Name);
              break;
            }
            break;
          }
          catch (Exception ex)
          {
            break;
          }
        }
        else
        {
          if (this.m_pageResources.Resources[key] is XObjectElement)
          {
            XObjectElement pageResource = this.m_pageResources[key] as XObjectElement;
            if (pageResource.XObjectDictionary != null && pageResource.XObjectDictionary is PdfStream)
            {
              SizeF xobjectSize = this.GetXObjectSize(pageResource.XObjectDictionary as PdfStream);
              PointF xobjectPosition = this.GetXObjectPosition(pageResource.XObjectDictionary as PdfStream);
              try
              {
                System.Drawing.Drawing2D.Matrix matrix = this.Multiply(this.Multiply(new System.Drawing.Drawing2D.Matrix(1f, 0.0f, 0.0f, -1f, 0.0f, 1f), this.Multiply(new System.Drawing.Drawing2D.Matrix(xobjectSize.Width - xobjectPosition.X, 0.0f, 0.0f, xobjectSize.Height - xobjectPosition.Y, xobjectPosition.X, xobjectPosition.Y), this.m_currentMatrix.Peek().m_drawing2dMatrixCTM)), new System.Drawing.Drawing2D.Matrix(1.33333337f, 0.0f, 0.0f, -1.33333337f, 0.0f, this.Size.Height * 1.33333337f));
                RectangleF rect = this.Rotation != PdfPageRotateAngle.RotateAngle90 ? (this.Rotation != PdfPageRotateAngle.RotateAngle180 ? (this.Rotation != PdfPageRotateAngle.RotateAngle270 ? new RectangleF(matrix.OffsetX / 1.33333337f, (float) Math.Round((double) (matrix.OffsetY / 1.33333337f), 5), (float) Math.Round((double) (matrix.Elements[0] / 1.33333337f), 5), (float) Math.Round((double) (matrix.Elements[3] / 1.33333337f), 5)) : ((double) matrix.Elements[0] == 0.0 || (double) matrix.Elements[3] == 0.0 ? new RectangleF((float) Math.Floor((double) matrix.OffsetY / 1.3333333730697632), (float) (Math.Floor((double) this.Size.Width) - (Math.Round((double) (matrix.OffsetX / 1.33333337f), 5) + Math.Floor((double) matrix.Elements[0] / 1.3333333730697632))), (float) Math.Round((double) (matrix.Elements[1] / 1.33333337f), 5), (float) Math.Round(-(double) matrix.Elements[2] / 1.3333333730697632, 5)) : new RectangleF((float) Math.Floor((double) matrix.OffsetY / 1.3333333730697632), this.Size.Width - (float) (Math.Round((double) (matrix.OffsetX / 1.33333337f), 5) + (double) matrix.Elements[0] / 1.3333333730697632), (float) Math.Round((double) (matrix.Elements[3] / 1.33333337f), 5), (float) Math.Round((double) (matrix.Elements[0] / 1.33333337f), 5)))) : new RectangleF((float) ((double) this.Size.Width - (double) matrix.OffsetX / 1.3333333730697632 - Math.Round((double) (matrix.Elements[0] / 1.33333337f), 5)), (float) ((double) this.Size.Height - Math.Round((double) (matrix.Elements[3] / 1.33333337f), 5) - Math.Round((double) (matrix.OffsetY / 1.33333337f), 5)), (float) Math.Round((double) (matrix.Elements[0] / 1.33333337f), 5), (float) Math.Round((double) (matrix.Elements[3] / 1.33333337f), 5))) : ((double) matrix.Elements[0] != 0.0 || (double) matrix.Elements[3] != 0.0 ? new RectangleF((float) ((double) this.Size.Height - (double) matrix.OffsetY / 1.3333333730697632 - Math.Round((double) (matrix.Elements[3] / 1.33333337f), 5)), matrix.Elements[4] / 1.33333337f, (float) Math.Round((double) (matrix.Elements[3] / 1.33333337f), 5), (float) Math.Round((double) (matrix.Elements[0] / 1.33333337f), 5)) : new RectangleF(this.Size.Height - matrix.Elements[5] / 1.33333337f, matrix.Elements[4] / 1.33333337f, (float) (-(double) matrix.Elements[1] / 1.3333333730697632), matrix.Elements[2] / 1.33333337f));
                foreach (RectangleF headerFooterBound in this.m_headerFooterBounds)
                {
                  PointF pt1 = new PointF(rect.X, rect.Y);
                  PointF pt2 = new PointF(rect.X + rect.Width, rect.Y);
                  PointF pt3 = new PointF(rect.X, rect.Y + rect.Height);
                  PointF pt4 = new PointF(rect.X + rect.Width, rect.Y + rect.Height);
                  if (headerFooterBound.Contains(rect) || headerFooterBound.IntersectsWith(rect) || headerFooterBound.Contains(pt1) || headerFooterBound.Contains(pt2) || headerFooterBound.Contains(pt3) || headerFooterBound.Contains(pt4))
                  {
                    contentStream = true;
                    break;
                  }
                }
              }
              catch (Exception ex)
              {
                if (ex.Message.Equals("Document contains one or more images with unsupported encoding."))
                  throw new Exception(ex.Message);
              }
            }
          }
          if (!contentStream)
          {
            keys.Add(key);
            break;
          }
          break;
        }
      case "Q":
        this.m_currentMatrix.Pop();
        break;
    }
    return contentStream;
  }

  private XmpMetadata GetMetadata(PdfStream imageStream)
  {
    if (!imageStream.ContainsKey("Metadata"))
      return (XmpMetadata) null;
    IPdfPrimitive stream = imageStream["Metadata"];
    PdfReferenceHolder pdfReferenceHolder = stream as PdfReferenceHolder;
    return pdfReferenceHolder != (PdfReferenceHolder) null ? this.TryGetMetadata(pdfReferenceHolder.Object as PdfStream) : this.TryGetMetadata(stream as PdfStream);
  }

  private XmpMetadata TryGetMetadata(PdfStream stream)
  {
    if (stream != null)
    {
      byte[] decompressedData = stream.GetDecompressedData();
      if (decompressedData.Length > 0)
        return new ImageMetadataParser((Stream) new MemoryStream(decompressedData)).TryGetMetadata();
    }
    return (XmpMetadata) null;
  }

  private void RegenerateImages(Image image, string imageName)
  {
    for (int index = 0; index < this.m_imageInfoList.Count; ++index)
    {
      if (this.m_imageInfoList[index].Name == imageName)
      {
        MemoryStream memoryStream = new MemoryStream();
        image.Save((Stream) memoryStream, image.RawFormat);
        Image image1 = Image.FromStream((Stream) memoryStream);
        if (index < this.extractedImages.Count && this.extractedImages[index] is Image)
        {
          this.extractedImages.RemoveAt(index);
          if (image1 != null)
          {
            this.extractedImages.Add((object) image1);
            break;
          }
        }
      }
    }
  }

  private SizeF GetXObjectSize(PdfStream element)
  {
    SizeF empty = SizeF.Empty;
    if (element.ContainsKey("Width"))
    {
      if (element["Width"] is PdfNumber)
        empty.Width = (float) (element["Width"] as PdfNumber).IntValue;
      if ((object) (element["Width"] as PdfReferenceHolder) != null)
        empty.Width = (float) ((element["Width"] as PdfReferenceHolder).Object as PdfNumber).IntValue;
      if (element["Height"] is PdfNumber)
        empty.Height = (float) (element["Height"] as PdfNumber).IntValue;
      if ((object) (element["Height"] as PdfReferenceHolder) != null)
        empty.Height = (float) ((element["Height"] as PdfReferenceHolder).Object as PdfNumber).IntValue;
    }
    else if (element.ContainsKey("BBox"))
    {
      PdfArray pdfArray = element["BBox"] as PdfArray;
      empty.Width = (pdfArray[2] as PdfNumber).FloatValue;
      empty.Height = (pdfArray[3] as PdfNumber).FloatValue;
    }
    return empty;
  }

  private PointF GetXObjectPosition(PdfStream element)
  {
    PointF empty = PointF.Empty;
    if (element.ContainsKey("BBox"))
    {
      PdfArray pdfArray = element["BBox"] as PdfArray;
      empty.X = (pdfArray[0] as PdfNumber).FloatValue;
      empty.Y = (pdfArray[1] as PdfNumber).FloatValue;
    }
    return empty;
  }

  private System.Drawing.Drawing2D.Matrix Multiply(System.Drawing.Drawing2D.Matrix matrix1, System.Drawing.Drawing2D.Matrix matrix2)
  {
    return new System.Drawing.Drawing2D.Matrix((float) ((double) matrix1.Elements[0] * (double) matrix2.Elements[0] + (double) matrix1.Elements[1] * (double) matrix2.Elements[2]), (float) ((double) matrix1.Elements[0] * (double) matrix2.Elements[1] + (double) matrix1.Elements[1] * (double) matrix2.Elements[3]), (float) ((double) matrix1.Elements[2] * (double) matrix2.Elements[0] + (double) matrix1.Elements[3] * (double) matrix2.Elements[2]), (float) ((double) matrix1.Elements[2] * (double) matrix2.Elements[1] + (double) matrix1.Elements[3] * (double) matrix2.Elements[3]), (float) ((double) matrix1.OffsetX * (double) matrix2.Elements[0] + (double) matrix1.OffsetY * (double) matrix2.Elements[2]) + matrix2.OffsetX, (float) ((double) matrix1.OffsetX * (double) matrix2.Elements[1] + (double) matrix1.OffsetY * (double) matrix2.Elements[3]) + matrix2.OffsetY);
  }

  internal virtual PdfResources GetResources()
  {
    if (this.m_resources == null)
    {
      this.m_resources = new PdfResources();
      this.Dictionary["Resources"] = (IPdfPrimitive) this.m_resources;
    }
    return this.m_resources;
  }

  internal void SetResources(PdfResources res)
  {
    this.m_resources = res;
    this.Dictionary["Resources"] = (IPdfPrimitive) this.m_resources;
    this.m_modified = true;
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_pageDictionary;

  internal void SetProgress() => this.m_isProgressOn = true;

  internal void ResetProgress() => this.m_isProgressOn = false;

  internal PdfTemplate GetContent()
  {
    lock (PdfPageBase.s_syncLockTemplate)
    {
      this.m_modified = false;
      this.m_layersCount = this.m_layers == null ? 0 : this.m_layers.Count;
      this.m_annotCount = this.GetAnnotationCount();
      MemoryStream stream = new MemoryStream();
      this.Layers.CombineContent((Stream) stream);
      this.m_pageContentLength = stream.Length;
      bool isLoadedPage = false;
      if (this is PdfLoadedPage)
        isLoadedPage = true;
      PdfDictionary resources = PdfCrossTable.Dereference(this.Dictionary["Resources"]) as PdfDictionary;
      return new PdfTemplate(this.Origin, this.Size, stream, resources, isLoadedPage, this);
    }
  }

  internal PdfArray ReInitializeContentReference()
  {
    IPdfPrimitive page = this.m_pageDictionary["Contents"];
    PdfArray pdfArray1 = page as PdfArray;
    PdfArray pdfArray2 = new PdfArray();
    if (pdfArray1 != null)
    {
      for (int index = 0; index < pdfArray1.Elements.Count; ++index)
      {
        if (pdfArray1.Elements[index] as PdfReferenceHolder != (PdfReferenceHolder) null && (pdfArray1.Elements[index] as PdfReferenceHolder).Object is PdfStream pdfStream)
          pdfArray2.Elements.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfStream));
      }
    }
    else if (page as PdfReferenceHolder != (PdfReferenceHolder) null)
    {
      if ((page as PdfReferenceHolder).Object is PdfStream pdfStream1)
        this.m_pageDictionary["Contents"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfStream1);
      else if (pdfStream1 == null && (page as PdfReferenceHolder).Object is PdfArray)
      {
        PdfArray pdfArray3 = (page as PdfReferenceHolder).Object as PdfArray;
        for (int index = 0; index < pdfArray3.Elements.Count; ++index)
        {
          if (pdfArray3.Elements[index] as PdfReferenceHolder != (PdfReferenceHolder) null && (pdfArray3.Elements[index] as PdfReferenceHolder).Object is PdfStream pdfStream)
            pdfArray2.Elements.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfStream));
        }
      }
    }
    return pdfArray2;
  }

  internal PdfDictionary CheckTypeOfXObject(PdfDictionary xObjectDictionary)
  {
    PdfDictionary pdfDictionary = new PdfDictionary();
    foreach (PdfName key in xObjectDictionary.Keys)
    {
      PdfStream pdfStream = PdfCrossTable.Dereference(xObjectDictionary[key]) as PdfStream;
      switch ((pdfStream["Subtype"] as PdfName).Value)
      {
        case "Image":
          this.ReInitializeImageData(pdfStream);
          break;
        case "Form":
          this.ReInitializeFormXObject(pdfStream);
          break;
      }
      if (pdfStream != null)
        pdfDictionary.Items.Add(key, (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfStream));
    }
    return pdfDictionary;
  }

  internal void ReInitializeFormXObject(PdfStream formXObjectData)
  {
    if (formXObjectData.ContainsKey("Resources"))
    {
      PdfDictionary xObjectData = !(formXObjectData["Resources"] as PdfReferenceHolder != (PdfReferenceHolder) null) ? formXObjectData["Resources"] as PdfDictionary : PdfCrossTable.Dereference(formXObjectData["Resources"]) as PdfDictionary;
      this.ReInitializeXobjectResources(xObjectData);
      if (formXObjectData["Resources"] as PdfReferenceHolder != (PdfReferenceHolder) null)
        formXObjectData["Resources"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) xObjectData);
      else
        formXObjectData["Resources"] = (IPdfPrimitive) xObjectData;
    }
    if (formXObjectData.ContainsKey((PdfName) "OC"))
    {
      PdfDictionary pdfDictionary = this.CheckOptionalContent(formXObjectData);
      formXObjectData.Items[(PdfName) "OC"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary);
    }
    if (formXObjectData.ContainsKey((PdfName) "PieceInfo"))
    {
      PdfDictionary pdfDictionary1 = formXObjectData.Items[(PdfName) "PieceInfo"] as PdfDictionary;
      foreach (PdfName key1 in pdfDictionary1.Keys)
      {
        PdfDictionary pdfDictionary2 = pdfDictionary1[key1] as PdfDictionary;
        foreach (PdfName key2 in pdfDictionary2.Keys)
        {
          if (pdfDictionary2[key2] as PdfReferenceHolder != (PdfReferenceHolder) null)
          {
            if ((pdfDictionary2[key2] as PdfReferenceHolder).Object is PdfStream pdfStream)
            {
              pdfDictionary2[key2] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfStream);
              break;
            }
            break;
          }
        }
      }
    }
    if (!formXObjectData.ContainsKey("Group"))
      return;
    PdfDictionary pdfDictionary3 = !(formXObjectData["Group"] as PdfReferenceHolder != (PdfReferenceHolder) null) ? formXObjectData["Group"] as PdfDictionary : PdfCrossTable.Dereference(formXObjectData["Group"]) as PdfDictionary;
    if (pdfDictionary3.ContainsKey("CS") && PdfCrossTable.Dereference(pdfDictionary3["CS"]) is PdfArray colorSpaceCollection)
    {
      this.ReinitializeColorSpace(colorSpaceCollection);
      pdfDictionary3["CS"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) colorSpaceCollection);
    }
    if (formXObjectData["Group"] as PdfReferenceHolder != (PdfReferenceHolder) null)
      formXObjectData["Group"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary3);
    else
      formXObjectData["Group"] = (IPdfPrimitive) pdfDictionary3;
  }

  internal PdfDictionary CheckOptionalContent(PdfStream xObjectData)
  {
    PdfDictionary pdfDictionary1 = (xObjectData.Items[(PdfName) "OC"] as PdfReferenceHolder).Object as PdfDictionary;
    PdfDictionary pdfDictionary2 = new PdfDictionary();
    foreach (PdfName key in pdfDictionary1.Keys)
    {
      if (pdfDictionary1[key] as PdfReferenceHolder != (PdfReferenceHolder) null)
      {
        PdfReferenceHolder pdfReferenceHolder = pdfDictionary1[key] as PdfReferenceHolder;
        pdfDictionary2.Items.Add(key, (IPdfPrimitive) (pdfReferenceHolder.Object as PdfDictionary));
      }
    }
    foreach (PdfName key in pdfDictionary2.Keys)
    {
      if (pdfDictionary1[key] as PdfReferenceHolder != (PdfReferenceHolder) null)
        pdfDictionary1[key] = (IPdfPrimitive) new PdfReferenceHolder(pdfDictionary2[key]);
    }
    return pdfDictionary1;
  }

  internal void ReInitializeImageData(PdfStream imageData)
  {
    if (imageData.ContainsKey("OC"))
    {
      PdfDictionary pdfDictionary = this.CheckOptionalContent(imageData);
      imageData.Items[(PdfName) "OC"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary);
    }
    if (imageData.ContainsKey("SMask"))
    {
      PdfDictionary pdfDictionary = PdfCrossTable.Dereference(imageData["SMask"]) as PdfDictionary;
      imageData.Items[(PdfName) "SMask"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary);
    }
    if (imageData.ContainsKey("ColorSpace"))
    {
      IPdfPrimitive pdfPrimitive = imageData.Items[(PdfName) "ColorSpace"];
      if (pdfPrimitive as PdfReferenceHolder != (PdfReferenceHolder) null)
        pdfPrimitive = (IPdfPrimitive) (PdfCrossTable.Dereference(pdfPrimitive) as PdfArray);
      if (pdfPrimitive is PdfArray)
      {
        PdfArray colorSpaceCollection = pdfPrimitive as PdfArray;
        this.ReinitializeColorSpace(colorSpaceCollection);
        if (imageData["ColorSpace"] as PdfReferenceHolder != (PdfReferenceHolder) null)
          imageData["ColorSpace"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) colorSpaceCollection);
        else
          imageData["ColorSpace"] = (IPdfPrimitive) colorSpaceCollection;
      }
    }
    if (imageData.Items.ContainsKey((PdfName) "Metadata"))
    {
      PdfStream pdfStream = PdfCrossTable.Dereference(imageData["Metadata"]) as PdfStream;
      imageData["Metadata"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfStream);
    }
    if (imageData.ContainsKey("Filter") && imageData["Filter"] as PdfReferenceHolder != (PdfReferenceHolder) null)
      imageData["Filter"] = (IPdfPrimitive) new PdfReferenceHolder(PdfCrossTable.Dereference(imageData["Filter"]));
    if (!imageData.ContainsKey("DecodeParms"))
      return;
    PdfDictionary pdfDictionary1 = imageData["DecodeParms"] as PdfDictionary;
    PdfDictionary pdfDictionary2 = new PdfDictionary();
    if (pdfDictionary1 != null)
    {
      foreach (PdfName key in pdfDictionary1.Keys)
      {
        if (pdfDictionary1[key] as PdfReferenceHolder != (PdfReferenceHolder) null && PdfCrossTable.Dereference(pdfDictionary1[key]) is PdfStream pdfStream)
          pdfDictionary2.Items.Add(key, (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfStream));
      }
      foreach (PdfName key in pdfDictionary2.Keys)
      {
        if (pdfDictionary1.ContainsKey(key))
          pdfDictionary1[key] = pdfDictionary2[key];
      }
    }
    if (pdfDictionary1 == null)
      return;
    imageData["DecodeParms"] = (IPdfPrimitive) pdfDictionary1;
  }

  internal PdfDictionary ReinitializePageResources()
  {
    IPdfPrimitive page = this.m_pageDictionary["Resources"];
    PdfDictionary pdfDictionary1 = !(page as PdfReferenceHolder != (PdfReferenceHolder) null) ? page as PdfDictionary : PdfCrossTable.Dereference(page) as PdfDictionary;
    PdfDictionary pdfDictionary2 = new PdfDictionary();
    if (pdfDictionary1.ContainsKey("Font"))
    {
      PdfDictionary pdfDictionary3 = !(pdfDictionary1["Font"] as PdfReferenceHolder != (PdfReferenceHolder) null) ? pdfDictionary1["Font"] as PdfDictionary : PdfCrossTable.Dereference(pdfDictionary1["Font"]) as PdfDictionary;
      PdfDictionary pdfDictionary4 = new PdfDictionary();
      foreach (PdfName key in pdfDictionary3.Keys)
      {
        if (pdfDictionary3[key] as PdfReferenceHolder != (PdfReferenceHolder) null && PdfCrossTable.Dereference(pdfDictionary3[key]) is PdfDictionary fontDictionary)
        {
          this.CheckFontInternalReference(fontDictionary);
          pdfDictionary4.Items.Add(key, (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) fontDictionary));
        }
      }
      pdfDictionary1["Font"] = !(pdfDictionary1["Font"] as PdfReferenceHolder != (PdfReferenceHolder) null) ? (IPdfPrimitive) pdfDictionary4 : (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary4);
    }
    if (pdfDictionary1.ContainsKey("XObject"))
    {
      PdfDictionary pdfDictionary5 = this.CheckTypeOfXObject(!(pdfDictionary1["XObject"] as PdfReferenceHolder != (PdfReferenceHolder) null) ? pdfDictionary1["XObject"] as PdfDictionary : PdfCrossTable.Dereference(pdfDictionary1["XObject"]) as PdfDictionary);
      pdfDictionary1["XObject"] = !(pdfDictionary1["XObject"] as PdfReferenceHolder != (PdfReferenceHolder) null) ? (IPdfPrimitive) pdfDictionary5 : (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary5);
    }
    if (pdfDictionary1.ContainsKey("Pattern"))
    {
      IPdfPrimitive pdfPrimitive = pdfDictionary1["Pattern"];
      if (PdfCrossTable.Dereference(pdfDictionary1["Pattern"]) is PdfDictionary pagePattern)
        this.ReInitializePagePatterns(pagePattern);
    }
    if (pdfDictionary1.ContainsKey("ColorSpace"))
    {
      PdfDictionary colorSpaceItems = !(pdfDictionary1["ColorSpace"] as PdfReferenceHolder != (PdfReferenceHolder) null) ? pdfDictionary1["ColorSpace"] as PdfDictionary : PdfCrossTable.Dereference(pdfDictionary1["ColorSpace"]) as PdfDictionary;
      this.ReinitializeColorSpaceItem(colorSpaceItems);
      pdfDictionary1["ColorSpace"] = (IPdfPrimitive) colorSpaceItems;
    }
    if (pdfDictionary1.ContainsKey("ExtGState"))
    {
      PdfDictionary pdfDictionary6 = this.ReInitializeExtGState(!(pdfDictionary1["ExtGState"] as PdfReferenceHolder != (PdfReferenceHolder) null) ? pdfDictionary1["ExtGState"] as PdfDictionary : PdfCrossTable.Dereference(pdfDictionary1["ExtGState"]) as PdfDictionary);
      pdfDictionary1["ExtGState"] = (IPdfPrimitive) pdfDictionary6;
    }
    if (pdfDictionary1.ContainsKey("Shading"))
    {
      PdfDictionary pageShadingItems = !(pdfDictionary1["Shading"] as PdfReferenceHolder != (PdfReferenceHolder) null) ? pdfDictionary1["Shading"] as PdfDictionary : PdfCrossTable.Dereference(pdfDictionary1["Shading"]) as PdfDictionary;
      this.CheckPageShadingReference(pageShadingItems);
      pdfDictionary1["Shading"] = !(pdfDictionary1["Shading"] as PdfReferenceHolder != (PdfReferenceHolder) null) ? (IPdfPrimitive) pageShadingItems : (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pageShadingItems);
    }
    return pdfDictionary1;
  }

  internal void CheckPageShadingReference(PdfDictionary pageShadingItems)
  {
    PdfDictionary pdfDictionary1 = new PdfDictionary();
    foreach (PdfName key in pageShadingItems.Keys)
    {
      PdfDictionary pdfDictionary2 = new PdfDictionary();
      if (pageShadingItems[key] as PdfReferenceHolder != (PdfReferenceHolder) null)
      {
        PdfDictionary pdfDictionary3 = PdfCrossTable.Dereference(pageShadingItems[key]) as PdfDictionary;
        if (pdfDictionary3.ContainsKey("Function") && (pdfDictionary3["Function"] as PdfReferenceHolder).Object is PdfDictionary pdfDictionary4)
          pdfDictionary2.Items.Add((PdfName) "Function", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary4));
        if (pdfDictionary3.ContainsKey("ColorSpace") && pdfDictionary3["ColorSpace"] as PdfReferenceHolder != (PdfReferenceHolder) null)
        {
          if (PdfCrossTable.Dereference(pdfDictionary3["ColorSpace"]) as PdfName != (PdfName) null)
            pdfDictionary2.Items.Add((PdfName) "ColorSpace", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) (PdfCrossTable.Dereference(pdfDictionary3["ColorSpace"]) as PdfName)));
          else if (PdfCrossTable.Dereference(pdfDictionary3["ColorSpace"]) is PdfStream)
            pdfDictionary2.Items.Add((PdfName) "ColorSpace", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) (PdfCrossTable.Dereference(pdfDictionary3["ColorSpace"]) as PdfStream)));
          else if (PdfCrossTable.Dereference(pdfDictionary3["ColorSpace"]) is PdfArray)
          {
            this.ReinitializeColorSpace(PdfCrossTable.Dereference(pdfDictionary3["ColorSpace"]) as PdfArray);
            pdfDictionary2.Items.Add((PdfName) "ColorSpace", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) (PdfCrossTable.Dereference(pdfDictionary3["ColorSpace"]) as PdfArray)));
          }
        }
      }
      pdfDictionary1.Items.Add(key, (IPdfPrimitive) pdfDictionary2);
    }
    foreach (PdfName key1 in pdfDictionary1.Keys)
    {
      PdfDictionary pdfDictionary5 = PdfCrossTable.Dereference(pdfDictionary1[key1]) as PdfDictionary;
      PdfDictionary pdfDictionary6 = PdfCrossTable.Dereference(pageShadingItems[key1]) as PdfDictionary;
      foreach (PdfName key2 in pdfDictionary5.Keys)
        pdfDictionary6[key2] = pdfDictionary5[key2];
      pageShadingItems[key1] = !(pageShadingItems[key1] as PdfReferenceHolder != (PdfReferenceHolder) null) ? (IPdfPrimitive) pdfDictionary6 : (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary6);
    }
  }

  internal void ReInitializePageAnnotation(PdfDictionary acroFormData)
  {
    if (acroFormData != null)
      this.InitializeAcroformReference(acroFormData);
    IPdfPrimitive page = this.m_pageDictionary["Annots"];
    PdfArray pdfArray1 = !(page as PdfReferenceHolder != (PdfReferenceHolder) null) ? page as PdfArray : PdfCrossTable.Dereference(page) as PdfArray;
    PdfArray pdfArray2 = new PdfArray();
    for (int index1 = 0; index1 < pdfArray1.Count; ++index1)
    {
      PdfDictionary pdfDictionary1 = !(pdfArray1[index1] as PdfReferenceHolder != (PdfReferenceHolder) null) ? pdfArray1[index1] as PdfDictionary : PdfCrossTable.Dereference(pdfArray1[index1]) as PdfDictionary;
      if (pdfDictionary1.ContainsKey("AP"))
        this.CheckAnnotAppearanceData(pdfDictionary1["AP"] as PdfDictionary);
      if (pdfDictionary1.ContainsKey("P"))
      {
        PdfDictionary pdfDictionary2 = (pdfDictionary1["P"] as PdfReferenceHolder).Object as PdfDictionary;
        pdfDictionary1["P"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary2);
      }
      if (pdfDictionary1.ContainsKey("Popup") && pdfDictionary1["Popup"] as PdfReferenceHolder != (PdfReferenceHolder) null)
      {
        PdfDictionary pdfDictionary3 = PdfCrossTable.Dereference(pdfDictionary1["Popup"]) as PdfDictionary;
        if (pdfDictionary3.ContainsKey("Parent") && pdfDictionary3["Parent"] as PdfReferenceHolder != (PdfReferenceHolder) null)
          pdfDictionary3["Parent"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) (PdfCrossTable.Dereference(pdfDictionary3["Parent"]) as PdfDictionary));
      }
      if (pdfDictionary1.ContainsKey("Parent"))
      {
        PdfDictionary pdfDictionary4 = (pdfDictionary1["Parent"] as PdfReferenceHolder).Object as PdfDictionary;
        if (pdfDictionary4.ContainsKey("Kids"))
        {
          PdfArray pdfArray3 = pdfDictionary4["Kids"] as PdfArray;
          PdfArray pdfArray4 = new PdfArray();
          for (int index2 = 0; index2 < pdfArray3.Count; ++index2)
          {
            if (pdfArray3[index2] as PdfReferenceHolder != (PdfReferenceHolder) null)
              pdfArray4.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) (PdfCrossTable.Dereference(pdfArray3[index2]) as PdfDictionary)));
          }
          pdfDictionary4["Kids"] = (IPdfPrimitive) pdfArray4;
        }
        pdfDictionary1["Parent"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary4);
      }
      if (pdfDictionary1.ContainsKey("BS"))
      {
        PdfReferenceHolder pdfReferenceHolder = pdfDictionary1["BS"] as PdfReferenceHolder;
        if (pdfReferenceHolder != (PdfReferenceHolder) null)
        {
          PdfDictionary pdfDictionary5 = pdfReferenceHolder.Object as PdfDictionary;
          pdfDictionary1["BS"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary5);
        }
      }
      if (pdfDictionary1.ContainsKey("A") && pdfDictionary1["A"] as PdfReferenceHolder != (PdfReferenceHolder) null)
        pdfDictionary1["A"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) (PdfCrossTable.Dereference(pdfDictionary1["A"]) as PdfDictionary));
      if (pdfArray1.Elements[index1] as PdfReferenceHolder != (PdfReferenceHolder) null)
        pdfArray2.Elements.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary1));
      else
        pdfArray2.Elements.Add((IPdfPrimitive) pdfDictionary1);
    }
    if (this.m_pageDictionary["Annots"] as PdfReferenceHolder != (PdfReferenceHolder) null)
      this.m_pageDictionary["Annots"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfArray2);
    else
      this.m_pageDictionary["Annots"] = (IPdfPrimitive) pdfArray2;
  }

  internal void CheckAnnotAppearanceData(PdfDictionary annotAppearanceData)
  {
    PdfDictionary pdfDictionary1 = new PdfDictionary();
    foreach (PdfName key1 in annotAppearanceData.Keys)
    {
      pdfStream1 = (PdfStream) null;
      PdfDictionary pdfDictionary2 = (PdfDictionary) null;
      if (annotAppearanceData[key1] as PdfReferenceHolder != (PdfReferenceHolder) null)
      {
        if (!(PdfCrossTable.Dereference(annotAppearanceData[key1]) is PdfStream pdfStream1))
          pdfDictionary2 = PdfCrossTable.Dereference(annotAppearanceData[key1]) as PdfDictionary;
      }
      else if (annotAppearanceData[key1] is PdfDictionary)
        pdfDictionary2 = annotAppearanceData[key1] as PdfDictionary;
      if (pdfStream1 != null && pdfStream1.ContainsKey("Resources"))
      {
        this.ReInitializeXobjectResources(pdfStream1["Resources"] as PdfDictionary);
        pdfDictionary1.Items.Add(key1, (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfStream1));
      }
      if (pdfDictionary2 != null)
      {
        PdfDictionary pdfDictionary3 = new PdfDictionary();
        foreach (PdfName key2 in pdfDictionary2.Keys)
        {
          if (PdfCrossTable.Dereference(pdfDictionary2[key2]) is PdfStream pdfStream2 && pdfStream2.ContainsKey("Resources"))
            this.ReInitializeXobjectResources(pdfStream2["Resources"] as PdfDictionary);
          pdfDictionary3.Items.Add(key2, (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfStream2));
        }
        pdfDictionary1.Items.Add(key1, (IPdfPrimitive) pdfDictionary3);
      }
    }
    foreach (PdfName key in pdfDictionary1.Keys)
      annotAppearanceData[key] = !(annotAppearanceData[key] as PdfReferenceHolder != (PdfReferenceHolder) null) ? pdfDictionary1[key] : (IPdfPrimitive) new PdfReferenceHolder(pdfDictionary1[key]);
  }

  internal void InitializeAcroformReference(PdfDictionary acroFormData)
  {
    if (acroFormData.ContainsKey("DR"))
    {
      PdfDictionary pdfDictionary1 = !(acroFormData["DR"] as PdfReferenceHolder != (PdfReferenceHolder) null) ? acroFormData["DR"] as PdfDictionary : PdfCrossTable.Dereference(acroFormData["DR"]) as PdfDictionary;
      if (pdfDictionary1.ContainsKey("Font"))
      {
        PdfDictionary pdfDictionary2 = pdfDictionary1["Font"] as PdfDictionary;
        PdfDictionary pdfDictionary3 = new PdfDictionary();
        foreach (PdfName key in pdfDictionary2.Keys)
        {
          PdfDictionary fontDictionary = PdfCrossTable.Dereference(pdfDictionary2[key]) as PdfDictionary;
          this.CheckFontInternalReference(fontDictionary);
          pdfDictionary3.Items.Add(key, (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) fontDictionary));
        }
        foreach (PdfName key in pdfDictionary3.Keys)
          pdfDictionary2[key] = pdfDictionary3[key];
      }
      if (pdfDictionary1.ContainsKey("Encoding"))
      {
        PdfDictionary pdfDictionary4 = !(pdfDictionary1["Encoding"] as PdfReferenceHolder != (PdfReferenceHolder) null) ? pdfDictionary1["Encoding"] as PdfDictionary : PdfCrossTable.Dereference(pdfDictionary1["Encoding"]) as PdfDictionary;
        PdfDictionary pdfDictionary5 = new PdfDictionary();
        foreach (PdfName key in pdfDictionary4.Keys)
        {
          if (pdfDictionary4[key] as PdfReferenceHolder != (PdfReferenceHolder) null)
            pdfDictionary5.Items.Add(key, (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) (PdfCrossTable.Dereference(pdfDictionary4[key]) as PdfDictionary)));
        }
        foreach (PdfName key in pdfDictionary5.Keys)
          pdfDictionary4[key] = pdfDictionary5[key];
      }
    }
    if (acroFormData.ContainsKey("Fields"))
    {
      PdfArray pdfArray1 = !(acroFormData["Fields"] as PdfReferenceHolder != (PdfReferenceHolder) null) ? acroFormData["Fields"] as PdfArray : PdfCrossTable.Dereference(acroFormData["Fields"]) as PdfArray;
      PdfArray pdfArray2 = new PdfArray();
      for (int index = 0; index < pdfArray1.Count; ++index)
      {
        if (pdfArray1[index] as PdfReferenceHolder != (PdfReferenceHolder) null)
          pdfArray2.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) (PdfCrossTable.Dereference(pdfArray1[index]) as PdfDictionary)));
      }
      acroFormData["Fields"] = !(acroFormData["Fields"] as PdfReferenceHolder != (PdfReferenceHolder) null) ? (IPdfPrimitive) pdfArray2 : (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfArray2);
    }
    if (!acroFormData.ContainsKey("XFA"))
      return;
    PdfArray pdfArray = acroFormData["XFA"] as PdfArray;
    for (int index = 0; index < pdfArray.Count; ++index)
    {
      if (pdfArray[index] as PdfReferenceHolder != (PdfReferenceHolder) null && (pdfArray[index] as PdfReferenceHolder).Object is PdfStream pdfStream)
      {
        pdfArray.RemoveAt(index);
        pdfArray.Elements.Insert(index, (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfStream));
      }
    }
  }

  internal void ReInitializeThumbnail()
  {
    PdfStream pdfStream = !(this.m_pageDictionary["Thumb"] as PdfReferenceHolder != (PdfReferenceHolder) null) ? this.m_pageDictionary["Thumb"] as PdfStream : PdfCrossTable.Dereference(this.m_pageDictionary["Thumb"]) as PdfStream;
    if (pdfStream.ContainsKey("ColorSpace"))
    {
      PdfArray colorSpaceCollection = !(pdfStream["ColorSpace"] as PdfReferenceHolder != (PdfReferenceHolder) null) ? pdfStream["ColorSpace"] as PdfArray : PdfCrossTable.Dereference(pdfStream["ColorSpace"]) as PdfArray;
      this.ReinitializeColorSpace(colorSpaceCollection);
      if (pdfStream["ColorSpace"] as PdfReferenceHolder != (PdfReferenceHolder) null)
        pdfStream["ColorSpace"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) colorSpaceCollection);
      else
        pdfStream["ColorSpace"] = (IPdfPrimitive) colorSpaceCollection;
    }
    if (this.m_pageDictionary["Thumb"] as PdfReferenceHolder != (PdfReferenceHolder) null)
      this.m_pageDictionary["Thumb"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfStream);
    else
      this.m_pageDictionary["Thumb"] = (IPdfPrimitive) pdfStream;
  }

  internal void ReinitializeColorSpace(PdfArray colorSpaceCollection)
  {
    for (int index = 0; index < colorSpaceCollection.Count; ++index)
    {
      if (colorSpaceCollection[index] as PdfReferenceHolder != (PdfReferenceHolder) null)
      {
        PdfReferenceHolder pdfReferenceHolder = colorSpaceCollection[index] as PdfReferenceHolder;
        if (pdfReferenceHolder.Object is PdfArray)
        {
          this.ReinitializeColorSpace(pdfReferenceHolder.Object as PdfArray);
          colorSpaceCollection.RemoveAt(index);
          colorSpaceCollection.Elements.Insert(index, (IPdfPrimitive) new PdfReferenceHolder(pdfReferenceHolder.Object));
        }
        else
        {
          if (pdfReferenceHolder.Object is PdfStream pdfStream)
            pdfReferenceHolder = new PdfReferenceHolder((IPdfPrimitive) pdfStream);
          colorSpaceCollection.RemoveAt(index);
          colorSpaceCollection.Elements.Insert(index, (IPdfPrimitive) pdfReferenceHolder);
        }
      }
    }
  }

  internal void ReinitializeColorSpaceItem(PdfDictionary colorSpaceItems)
  {
    PdfDictionary pdfDictionary = new PdfDictionary();
    foreach (PdfName key in colorSpaceItems.Keys)
    {
      if (PdfCrossTable.Dereference(colorSpaceItems[key]) is PdfArray colorSpaceCollection)
      {
        this.ReinitializeColorSpace(colorSpaceCollection);
        pdfDictionary[key] = !(colorSpaceItems[key] as PdfReferenceHolder != (PdfReferenceHolder) null) ? (IPdfPrimitive) colorSpaceCollection : (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) colorSpaceCollection);
      }
    }
    foreach (PdfName key in pdfDictionary.Keys)
      colorSpaceItems[key] = pdfDictionary[key];
  }

  internal void ReInitializePagePatterns(PdfDictionary pagePattern)
  {
    PdfDictionary pdfDictionary1 = new PdfDictionary();
    foreach (PdfName key in pagePattern.Keys)
    {
      PdfDictionary pdfDictionary2 = (pagePattern[key] as PdfReferenceHolder).Object as PdfDictionary;
      if (pdfDictionary2.ContainsKey("Shading"))
      {
        PdfDictionary pdfDictionary3 = (pdfDictionary2["Shading"] as PdfReferenceHolder).Object as PdfDictionary;
        if (pdfDictionary3.ContainsKey("Function"))
        {
          PdfReferenceHolder pdfReferenceHolder = pdfDictionary3["Function"] as PdfReferenceHolder;
          if (pdfReferenceHolder.Object is PdfStream pdfStream)
            pdfDictionary3["Function"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfStream);
          else if (pdfReferenceHolder.Object is PdfDictionary)
            pdfDictionary3["Function"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) (pdfReferenceHolder.Object as PdfDictionary));
        }
        pdfDictionary2["Shading"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary3);
      }
      if (pdfDictionary2.ContainsKey("Resources"))
      {
        PdfDictionary xObjectData = !(pdfDictionary2["Resources"] as PdfReferenceHolder != (PdfReferenceHolder) null) ? pdfDictionary2["Resources"] as PdfDictionary : PdfCrossTable.Dereference(pdfDictionary2["Resources"]) as PdfDictionary;
        this.ReInitializeXobjectResources(xObjectData);
        pdfDictionary2["Resources"] = !(pdfDictionary2["Resources"] as PdfReferenceHolder != (PdfReferenceHolder) null) ? (IPdfPrimitive) xObjectData : (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) xObjectData);
      }
      pdfDictionary1[key] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary2);
    }
    foreach (PdfName key in pdfDictionary1.Keys)
      pagePattern[key] = pdfDictionary1[key];
  }

  internal void CheckFontInternalReference(PdfDictionary fontDictionary)
  {
    if (fontDictionary.ContainsKey("DescendantFonts"))
    {
      PdfArray pdfArray1 = !(fontDictionary["DescendantFonts"] as PdfReferenceHolder != (PdfReferenceHolder) null) ? fontDictionary[(PdfName) "DescendantFonts"] as PdfArray : PdfCrossTable.Dereference(fontDictionary["DescendantFonts"]) as PdfArray;
      PdfArray pdfArray2 = new PdfArray();
      for (int index = 0; index < pdfArray1.Count; ++index)
      {
        PdfDictionary fontDictionary1 = PdfCrossTable.Dereference(pdfArray1[index]) as PdfDictionary;
        PdfDictionary pdfDictionary = this.ReInitializeFontDescriptor(fontDictionary1);
        foreach (PdfName key in pdfDictionary.Keys)
        {
          if (fontDictionary1.ContainsKey(key))
            fontDictionary1[key] = pdfDictionary[key];
        }
        pdfArray2.Elements.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) fontDictionary1));
      }
      fontDictionary["DescendantFonts"] = (IPdfPrimitive) pdfArray2;
    }
    if (fontDictionary.ContainsKey("FontDescriptor"))
    {
      PdfDictionary fontDictionary2 = !(fontDictionary["FontDescriptor"] as PdfReferenceHolder != (PdfReferenceHolder) null) ? fontDictionary["FontDescriptor"] as PdfDictionary : PdfCrossTable.Dereference(fontDictionary["FontDescriptor"]) as PdfDictionary;
      PdfDictionary pdfDictionary = this.ReInitializeFontDescriptor(fontDictionary2);
      foreach (PdfName key in pdfDictionary.Keys)
      {
        if (fontDictionary2.ContainsKey(key))
          fontDictionary2[key] = pdfDictionary[key];
      }
      fontDictionary["FontDescriptor"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) fontDictionary2);
    }
    if (fontDictionary.ContainsKey("Widths") && fontDictionary["Widths"] as PdfReferenceHolder != (PdfReferenceHolder) null && (fontDictionary["Widths"] as PdfReferenceHolder).Object is PdfArray pdfArray)
      fontDictionary["Widths"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfArray);
    if (fontDictionary.ContainsKey("ToUnicode") && fontDictionary["ToUnicode"] as PdfReferenceHolder != (PdfReferenceHolder) null && PdfCrossTable.Dereference(fontDictionary["ToUnicode"]) is PdfStream pdfStream)
      fontDictionary["ToUnicode"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfStream);
    if (!fontDictionary.ContainsKey("Encoding") || !(fontDictionary["Encoding"] as PdfReferenceHolder != (PdfReferenceHolder) null))
      return;
    PdfDictionary pdfDictionary1 = PdfCrossTable.Dereference(fontDictionary["Encoding"]) as PdfDictionary;
    if (pdfDictionary1.ContainsKey("Differences") && pdfDictionary1["Differences"] as PdfReferenceHolder != (PdfReferenceHolder) null)
    {
      if (PdfCrossTable.Dereference(pdfDictionary1["Differences"]) is PdfDictionary pdfDictionary2)
        pdfDictionary1["Differences"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary2);
      else if (PdfCrossTable.Dereference(pdfDictionary1["Differences"]) is PdfArray)
        pdfDictionary1["Differences"] = (IPdfPrimitive) new PdfReferenceHolder(PdfCrossTable.Dereference(pdfDictionary1["Differences"]));
    }
    fontDictionary["Encoding"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary1);
  }

  internal PdfDictionary ReInitializeFontDescriptor(PdfDictionary fontDictionary)
  {
    PdfDictionary pdfDictionary1 = new PdfDictionary();
    foreach (PdfName key1 in fontDictionary.Keys)
    {
      if (fontDictionary[key1] as PdfReferenceHolder != (PdfReferenceHolder) null)
      {
        PdfDictionary pdfDictionary2 = PdfCrossTable.Dereference(fontDictionary[key1]) as PdfDictionary;
        PdfDictionary pdfDictionary3 = new PdfDictionary();
        if (pdfDictionary2 != null)
        {
          foreach (PdfName key2 in pdfDictionary2.Keys)
          {
            if (pdfDictionary2[key2] as PdfReferenceHolder != (PdfReferenceHolder) null)
            {
              PdfReferenceHolder pdfReferenceHolder = pdfDictionary2[key2] as PdfReferenceHolder;
              if (pdfReferenceHolder.Object is PdfNumber)
                pdfDictionary3[key2] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) (pdfReferenceHolder.Object as PdfNumber));
              else if (pdfReferenceHolder.Object is PdfStream pdfStream)
                pdfDictionary3[key2] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfStream);
            }
          }
          foreach (PdfName key3 in pdfDictionary3.Keys)
          {
            if (pdfDictionary2.ContainsKey(key3))
              pdfDictionary2[key3] = pdfDictionary3[key3];
          }
          pdfDictionary1[key1] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary2);
        }
        else if (pdfDictionary2 == null && key1.Value == "W" && (fontDictionary[key1] as PdfReferenceHolder).Object is PdfArray pdfArray)
          pdfDictionary1[key1] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfArray);
      }
    }
    return pdfDictionary1;
  }

  internal PdfDictionary ReInitializeExtGState(PdfDictionary extStateData)
  {
    PdfDictionary pdfDictionary1 = new PdfDictionary();
    foreach (PdfName key1 in extStateData.Keys)
    {
      PdfDictionary pdfDictionary2 = !(extStateData[key1] as PdfReferenceHolder != (PdfReferenceHolder) null) ? extStateData[key1] as PdfDictionary : PdfCrossTable.Dereference(extStateData[key1]) as PdfDictionary;
      if (pdfDictionary2.ContainsKey("SMask") && pdfDictionary2["SMask"] as PdfReferenceHolder != (PdfReferenceHolder) null)
        pdfDictionary2["SMask"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) (PdfCrossTable.Dereference(pdfDictionary2["SMask"]) as PdfDictionary));
      PdfDictionary pdfDictionary3 = new PdfDictionary();
      foreach (PdfName key2 in pdfDictionary2.Keys)
      {
        if (pdfDictionary2[key2] as PdfReferenceHolder != (PdfReferenceHolder) null && PdfCrossTable.Dereference(pdfDictionary2[key2]) is PdfStream pdfStream)
          pdfDictionary3.Items.Add(key2, (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfStream));
      }
      foreach (PdfName key3 in pdfDictionary3.Keys)
        pdfDictionary2[key3] = pdfDictionary3[key3];
      pdfDictionary1.Items.Add(key1, (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary2));
    }
    return pdfDictionary1;
  }

  internal void ReInitializeXobjectResources(PdfDictionary xObjectData)
  {
    if (xObjectData.ContainsKey("Font"))
    {
      PdfDictionary pdfDictionary1 = !(xObjectData["Font"] as PdfReferenceHolder != (PdfReferenceHolder) null) ? xObjectData["Font"] as PdfDictionary : PdfCrossTable.Dereference(xObjectData["Font"]) as PdfDictionary;
      PdfDictionary pdfDictionary2 = new PdfDictionary();
      foreach (PdfName key in pdfDictionary1.Keys)
      {
        PdfDictionary fontDictionary = PdfCrossTable.Dereference(pdfDictionary1[key]) as PdfDictionary;
        this.CheckFontInternalReference(fontDictionary);
        pdfDictionary2.Items.Add(key, (IPdfPrimitive) fontDictionary);
      }
      foreach (PdfName key in pdfDictionary2.Keys)
      {
        if (pdfDictionary1.ContainsKey(key))
          pdfDictionary1[key] = (IPdfPrimitive) new PdfReferenceHolder(pdfDictionary2[key]);
      }
      xObjectData["Font"] = !(xObjectData["Font"] as PdfReferenceHolder != (PdfReferenceHolder) null) ? (IPdfPrimitive) pdfDictionary1 : (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary1);
    }
    if (xObjectData.ContainsKey("ExtGState"))
    {
      PdfDictionary pdfDictionary = this.ReInitializeExtGState(xObjectData["ExtGState"] as PdfDictionary);
      xObjectData["ExtGState"] = (IPdfPrimitive) pdfDictionary;
    }
    if (xObjectData.ContainsKey("Properties"))
    {
      PdfDictionary pdfDictionary3 = xObjectData["Properties"] as PdfDictionary;
      PdfDictionary pdfDictionary4 = new PdfDictionary();
      foreach (PdfName key in pdfDictionary3.Keys)
      {
        PdfDictionary pdfDictionary5 = PdfCrossTable.Dereference(pdfDictionary3[key]) as PdfDictionary;
        pdfDictionary4.Items.Add(key, (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary5));
      }
      xObjectData["Properties"] = (IPdfPrimitive) pdfDictionary4;
    }
    if (xObjectData.ContainsKey("XObject"))
    {
      PdfDictionary pdfDictionary = this.CheckTypeOfXObject(xObjectData["XObject"] as PdfDictionary);
      xObjectData["XObject"] = (IPdfPrimitive) pdfDictionary;
    }
    if (xObjectData.ContainsKey("Pattern"))
    {
      IPdfPrimitive pagePattern = xObjectData["Pattern"];
      if (pagePattern is PdfDictionary)
        this.ReInitializePagePatterns(pagePattern as PdfDictionary);
    }
    if (!xObjectData.ContainsKey("ColorSpace"))
      return;
    PdfDictionary pdfDictionary6 = xObjectData["ColorSpace"] as PdfDictionary;
    PdfDictionary pdfDictionary7 = new PdfDictionary();
    foreach (PdfName key in pdfDictionary6.Keys)
    {
      if (pdfDictionary6[key] as PdfReferenceHolder != (PdfReferenceHolder) null)
      {
        PdfArray colorSpaceCollection = PdfCrossTable.Dereference(pdfDictionary6[key]) as PdfArray;
        this.ReinitializeColorSpace(colorSpaceCollection);
        pdfDictionary7[key] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) colorSpaceCollection);
      }
    }
    xObjectData["ColorSpace"] = (IPdfPrimitive) pdfDictionary7;
  }

  private PdfPageOrientation ObtainOrientation()
  {
    return (double) this.Size.Width > (double) this.Size.Height ? PdfPageOrientation.Landscape : PdfPageOrientation.Portrait;
  }

  internal virtual void Clear()
  {
    if (this.m_pageResources != null)
    {
      this.m_pageResources.Resources.Clear();
      this.m_pageResources = (PdfPageResources) null;
    }
    this.m_graphicsState = (Stack<GraphicsState>) null;
    if (this.m_layers != null)
      this.m_layers.Clear();
    this.m_layers = (PdfPageLayerCollection) null;
    this.m_resources = (PdfResources) null;
    this.m_pageDictionary = (PdfDictionary) null;
    this.m_annotations = (PdfLoadedAnnotationCollection) null;
    this.m_fontNames = (List<PdfName>) null;
    this.m_fontReference = (List<IPdfPrimitive>) null;
    if (this.m_contentTemplate == null)
      return;
    this.m_contentTemplate = (PdfTemplate) null;
  }

  internal void ImportAnnotations(
    PdfLoadedDocument ldDoc,
    PdfPageBase page,
    List<PdfArray> destinations)
  {
    if (ldDoc.Security.EncryptOnlyAttachment)
    {
      PdfLoadedAnnotationCollection annotations1 = page.Annotations;
    }
    PdfArray annotations2 = page.ObtainAnnotations();
    if (annotations2 == null)
      return;
    PdfArray pdfArray1 = new PdfArray();
    if (this.Dictionary.ContainsKey("Annots"))
      pdfArray1 = this.Dictionary["Annots"] as PdfArray;
    else
      this.Dictionary["Annots"] = (IPdfPrimitive) pdfArray1;
    foreach (IPdfPrimitive pdfPrimitive in annotations2)
    {
      if (!(pdfPrimitive is PdfNull))
      {
        PdfDictionary dictionary = PdfCrossTable.Dereference(pdfPrimitive) as PdfDictionary;
        if ((!(this is PdfPage) ? (this as PdfLoadedPage).Document.EnableMemoryOptimization : (this as PdfPage).Section.ParentDocument.EnableMemoryOptimization) && dictionary != null)
        {
          if (ldDoc.Form != null && dictionary.ContainsKey("Subtype") && (dictionary["Subtype"] as PdfName).Value == "Widget" && ldDoc.Form.Fields.Count > 0)
          {
            ++this.m_fieldCount;
          }
          else
          {
            this.m_modified = true;
            PdfDictionary annotation = new PdfDictionary(dictionary);
            PdfArray pdfArray2 = (PdfArray) null;
            if (annotation.ContainsKey("Dest"))
              pdfArray2 = this.GetDestination(ldDoc, annotation);
            annotation.Remove("Dest");
            if (annotation.ContainsKey("A") && (object) (annotation["A"] as PdfReferenceHolder) != null)
              ((annotation["A"] as PdfReferenceHolder).Object as PdfDictionary).Remove("AN");
            annotation.Remove("Popup");
            annotation.Remove("P");
            annotation.Remove("Parent");
            PdfCrossTable crossTable = !(this is PdfPage) ? (this as PdfLoadedPage).Document.CrossTable : (this as PdfPage).Section.ParentDocument.CrossTable;
            PdfDictionary pdfDictionary = annotation.Clone(crossTable) as PdfDictionary;
            PdfReferenceHolder primitive = new PdfReferenceHolder((IPdfWrapper) this);
            pdfDictionary.SetProperty("P", (IPdfPrimitive) primitive);
            pdfArray1.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary));
            if (pdfArray2 != null)
              pdfDictionary["Dest"] = pdfArray2.Clone(crossTable);
          }
        }
        else if (dictionary != null)
        {
          PdfDictionary annotation = new PdfDictionary(dictionary);
          annotation.SetProperty("P", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this));
          pdfArray1.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) annotation));
          if (annotation.ContainsKey("A") && ((object) (annotation["A"] as PdfReferenceHolder) != null || annotation["A"] is PdfDictionary))
          {
            PdfDictionary pdfDictionary1 = (PdfDictionary) null;
            if ((object) (annotation["A"] as PdfReferenceHolder) != null)
              pdfDictionary1 = (annotation["A"] as PdfReferenceHolder).Object as PdfDictionary;
            else if (annotation["A"] is PdfDictionary)
              pdfDictionary1 = annotation["A"] as PdfDictionary;
            pdfDictionary1.Remove("AN");
            if (pdfDictionary1["D"] is PdfArray pdfArray3)
            {
              for (int index = 0; index < pdfArray3.Count; ++index)
              {
                IPdfPrimitive element = pdfArray3[index];
                if ((object) (element as PdfReferenceHolder) != null && (element as PdfReferenceHolder).Object is PdfDictionary pdfDictionary2 && pdfDictionary2.ContainsKey("Type") && pdfDictionary2["Type"].ToString().ToLower().Contains(nameof (page)))
                {
                  pdfArray3.Remove(element);
                  pdfArray3.Insert(index, (IPdfPrimitive) new PdfNull());
                  break;
                }
              }
            }
          }
          if (annotation.ContainsKey("Dest"))
          {
            PdfArray destination = this.GetDestination(ldDoc, annotation);
            if (destination != null)
            {
              destinations.Add(destination);
              annotation["Dest"] = (IPdfPrimitive) destination;
            }
          }
          else if (annotation.ContainsKey("Popup"))
          {
            PdfDictionary pdfDictionary = (annotation.Items[new PdfName("Popup")] as PdfReferenceHolder).Object as PdfDictionary;
            if (pdfDictionary.ContainsKey("Parent"))
              pdfDictionary.Items.Remove(new PdfName("Parent"));
          }
        }
      }
    }
  }

  internal void ImportAnnotations(PdfLoadedDocument ldDoc, PdfPageBase page)
  {
    PdfArray annotations = page.ObtainAnnotations();
    if (annotations == null)
      return;
    PdfArray pdfArray = new PdfArray();
    PdfReferenceHolder primitive = new PdfReferenceHolder((IPdfWrapper) this);
    this.Dictionary["Annots"] = (IPdfPrimitive) pdfArray;
    this.m_modified = true;
    foreach (IPdfPrimitive pdfPrimitive in annotations)
    {
      PdfDictionary pdfDictionary = new PdfDictionary(PdfCrossTable.Dereference(pdfPrimitive) as PdfDictionary);
      pdfDictionary.SetProperty("P", (IPdfPrimitive) primitive);
      pdfArray.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary));
    }
  }

  private PdfArray GetDestination(PdfLoadedDocument ldDoc, PdfDictionary annotation)
  {
    IPdfPrimitive pdfPrimitive = PdfCrossTable.Dereference(annotation["Dest"]);
    PdfName name1 = pdfPrimitive as PdfName;
    PdfString name2 = pdfPrimitive as PdfString;
    PdfArray array = pdfPrimitive as PdfArray;
    if (ldDoc.Catalog.Destinations != null)
    {
      if (name1 != (PdfName) null)
        array = ldDoc.GetNamedDestination(name1);
      else if (name2 != null)
        array = ldDoc.GetNamedDestination(name2);
    }
    if (array != null)
      array = new PdfArray(array);
    return array;
  }

  internal PdfArray ObtainAnnotations()
  {
    IPdfPrimitive pdfPrimitive = this.Dictionary.GetValue("Annots", "Parent");
    PdfReferenceHolder pdfReferenceHolder = pdfPrimitive as PdfReferenceHolder;
    return !(pdfReferenceHolder != (PdfReferenceHolder) null) ? pdfPrimitive as PdfArray : pdfReferenceHolder.Object as PdfArray;
  }

  private int GetAnnotationCount() => this.m_annotations != null ? this.m_annotations.Count : 0;

  private PdfPageRotateAngle ObtainRotation()
  {
    int num = 90;
    PdfDictionary pdfDictionary = this.Dictionary;
    PdfNumber pdfNumber;
    for (pdfNumber = (PdfNumber) null; pdfDictionary != null && pdfNumber == null; pdfDictionary = PdfCrossTable.Dereference(pdfDictionary["Parent"]) as PdfDictionary)
      pdfNumber = (object) (pdfDictionary["Rotate"] as PdfReferenceHolder) == null ? (PdfNumber) pdfDictionary["Rotate"] : (PdfNumber) (pdfDictionary["Rotate"] as PdfReferenceHolder).Object;
    if (pdfNumber == null)
      pdfNumber = new PdfNumber(0);
    if (pdfNumber.IntValue < 0)
      pdfNumber.IntValue = 360 + pdfNumber.IntValue;
    return (PdfPageRotateAngle) (pdfNumber.IntValue / num);
  }

  private void DrawAnnotationTemplates(PdfGraphics g)
  {
    PdfArray annotations = this.ObtainAnnotations();
    if (annotations == null)
      return;
    foreach (IPdfPrimitive pdfPrimitive in annotations)
    {
      PdfReferenceHolder pdfReferenceHolder = pdfPrimitive as PdfReferenceHolder;
      PdfDictionary annotation = !(pdfReferenceHolder != (PdfReferenceHolder) null) ? pdfPrimitive as PdfDictionary : pdfReferenceHolder.Object as PdfDictionary;
      PdfTemplate annotTemplate = this.GetAnnotTemplate(annotation);
      if (annotTemplate != null)
      {
        PointF location = this.NormalizeAnnotationLocation(this.GetAnnotationLocation(annotation), g, annotTemplate);
        g.DrawPdfTemplate(annotTemplate, location);
      }
    }
  }

  private PointF NormalizeAnnotationLocation(
    PointF location,
    PdfGraphics graphics,
    PdfTemplate template)
  {
    location.Y = graphics.Size.Height - location.Y - template.Height;
    return location;
  }

  private PointF GetAnnotationLocation(PdfDictionary annotation)
  {
    if (!(PdfCrossTable.Dereference(annotation["Rect"]) is PdfArray pdfArray))
      throw new PdfDocumentException("Invalid format: annotation dictionary doesn't contain rectangle array.");
    PdfNumber pdfNumber1 = pdfArray.Count >= 4 ? pdfArray[0] as PdfNumber : throw new PdfDocumentException("Invalid format: annotation rectangle has less then four elements.");
    PdfNumber pdfNumber2 = pdfArray[1] as PdfNumber;
    PdfNumber pdfNumber3 = pdfArray[2] as PdfNumber;
    PdfNumber pdfNumber4 = pdfArray[3] as PdfNumber;
    return new PointF(Math.Min(pdfNumber1.FloatValue, pdfNumber3.FloatValue), Math.Min(pdfNumber2.FloatValue, pdfNumber4.FloatValue));
  }

  private SizeF GetAnnotationSize(PdfDictionary annotation)
  {
    return this.GetElementSize(annotation, "Rect");
  }

  private SizeF GetElementSize(PdfDictionary dictionary, string propertyName)
  {
    if (!(PdfCrossTable.Dereference(dictionary[propertyName]) is PdfArray pdfArray))
      throw new PdfDocumentException("Invalid format: dictionary doesn't contain rectangle array.");
    PdfNumber pdfNumber1 = pdfArray.Count >= 4 ? pdfArray[0] as PdfNumber : throw new PdfDocumentException("Invalid format: rectangle array has less then four elements.");
    PdfNumber pdfNumber2 = pdfArray[1] as PdfNumber;
    PdfNumber pdfNumber3 = pdfArray[2] as PdfNumber;
    PdfNumber pdfNumber4 = pdfArray[3] as PdfNumber;
    return new SizeF(Math.Abs(pdfNumber1.FloatValue - pdfNumber3.FloatValue), Math.Abs(pdfNumber2.FloatValue - pdfNumber4.FloatValue));
  }

  private PdfTemplate GetAnnotTemplate(PdfDictionary annotation)
  {
    PdfDictionary pdfDictionary1 = PdfCrossTable.Dereference(annotation["AP"]) as PdfDictionary;
    PdfTemplate annotTemplate = (PdfTemplate) null;
    if (pdfDictionary1 != null && PdfCrossTable.Dereference(pdfDictionary1["N"]) is PdfDictionary pdfDictionary2)
    {
      if (!(pdfDictionary2 is PdfStream dictionary))
      {
        PdfName key = (PdfName) null;
        if (annotation.ContainsKey("AS"))
        {
          key = PdfCrossTable.Dereference(annotation["AS"]) as PdfName;
        }
        else
        {
          IEnumerator enumerator = (IEnumerator) pdfDictionary2.Keys.GetEnumerator();
          if (enumerator.MoveNext())
            key = enumerator.Current as PdfName;
        }
        if (key != (PdfName) null)
          dictionary = PdfCrossTable.Dereference(pdfDictionary2[key]) as PdfStream;
      }
      if (dictionary != null)
      {
        PdfDictionary resources = PdfCrossTable.Dereference(dictionary["Resources"]) as PdfDictionary;
        annotTemplate = new PdfTemplate(this.GetElementSize((PdfDictionary) dictionary, "BBox"), dictionary.InternalStream, resources);
      }
    }
    return annotTemplate;
  }
}
