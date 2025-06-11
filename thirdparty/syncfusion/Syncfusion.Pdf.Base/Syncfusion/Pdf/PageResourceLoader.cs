// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PageResourceLoader
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

#nullable disable
namespace Syncfusion.Pdf;

internal sealed class PageResourceLoader
{
  private static PageResourceLoader s_instance;
  private static object s_lock = new object();
  private bool m_isExtractImages;
  internal Dictionary<string, PdfMatrix> m_commonMatrix;
  private object m_resourceLock = new object();
  private bool m_hasImages;

  public static PageResourceLoader Instance
  {
    get
    {
      if (PageResourceLoader.s_instance == null)
      {
        lock (PageResourceLoader.s_lock)
          PageResourceLoader.s_instance = new PageResourceLoader();
      }
      return PageResourceLoader.s_instance;
    }
  }

  internal bool HasImages
  {
    get => this.m_hasImages;
    set => this.m_hasImages = value;
  }

  public PdfPageResources GetPageResources(PdfPageBase page)
  {
    PdfPageResources pageResources1 = new PdfPageResources();
    this.m_isExtractImages = page.isExtractImages;
    float num = 0.0f;
    if (PdfDocument.EnableThreadSafe)
      Monitor.Enter(this.m_resourceLock);
    PdfDictionary pdfDictionary1 = (PdfDictionary) page.GetResources();
    if (PdfDocument.EnableThreadSafe)
      Monitor.Exit(this.m_resourceLock);
    PdfArray annotations = page.ObtainAnnotations();
    Dictionary<string, PdfMatrix> commonMatrix = new Dictionary<string, PdfMatrix>();
    PdfPageResources pageResources2 = this.UpdatePageResources(this.UpdatePageResources(this.UpdatePageResources(this.UpdatePageResources(this.UpdatePageResources(this.UpdatePageResources(pageResources1, this.GetFontResources(pdfDictionary1, page)), this.GetImageResources(pdfDictionary1, page, ref commonMatrix)), this.GetExtendedGraphicResources(pdfDictionary1)), this.GetColorSpaceResource(pdfDictionary1)), this.GetPatternResource(pdfDictionary1)), this.GetShadingResource(pdfDictionary1));
    if (annotations != null)
      pageResources2.Add("Annotations", (object) annotations);
    while (pdfDictionary1 != null && pdfDictionary1.ContainsKey("XObject"))
    {
      PdfDictionary pdfDictionary2 = pdfDictionary1;
      PdfDictionary pdfDictionary3 = (object) (pdfDictionary1["XObject"] as PdfReferenceHolder) == null ? pdfDictionary1["XObject"] as PdfDictionary : (pdfDictionary1["XObject"] as PdfReferenceHolder).Object as PdfDictionary;
      pdfDictionary1 = pdfDictionary3["Resources"] as PdfDictionary;
      foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in pdfDictionary3.Items)
      {
        PdfDictionary pdfDictionary4 = (object) (keyValuePair.Value as PdfReferenceHolder) == null ? keyValuePair.Value as PdfDictionary : (keyValuePair.Value as PdfReferenceHolder).Object as PdfDictionary;
        if (pdfDictionary4 != null && pdfDictionary4.ContainsKey("Resources"))
        {
          if ((object) (pdfDictionary4["Resources"] as PdfReferenceHolder) != null)
          {
            PdfReferenceHolder pdfReferenceHolder = pdfDictionary4["Resources"] as PdfReferenceHolder;
            if ((double) num != (double) pdfReferenceHolder.Reference.ObjNum)
            {
              pdfDictionary1 = pdfReferenceHolder.Object as PdfDictionary;
              num = (float) pdfReferenceHolder.Reference.ObjNum;
            }
            else
              continue;
          }
          else
            pdfDictionary1 = pdfDictionary4["Resources"] as PdfDictionary;
          if (pdfDictionary1.Equals((object) pdfDictionary2))
          {
            pdfDictionary1 = (PdfDictionary) null;
            pdfDictionary2 = (PdfDictionary) null;
          }
          pageResources2 = this.UpdatePageResources(pageResources2, this.GetFontResources(pdfDictionary1, page));
          pageResources2 = this.UpdatePageResources(pageResources2, this.GetImageResources(pdfDictionary1, page, ref commonMatrix));
        }
      }
    }
    this.m_commonMatrix = commonMatrix;
    if (page.Rotation == PdfPageRotateAngle.RotateAngle90)
      pageResources2.Add("Rotate", (object) 90f);
    else if (page.Rotation == PdfPageRotateAngle.RotateAngle180)
      pageResources2.Add("Rotate", (object) 180f);
    else if (page.Rotation == PdfPageRotateAngle.RotateAngle270)
      pageResources2.Add("Rotate", (object) 270f);
    return pageResources2;
  }

  internal Dictionary<string, object> GetExtendedGraphicResources(PdfDictionary resourceDictionary)
  {
    Dictionary<string, object> graphicResources = new Dictionary<string, object>();
    if (resourceDictionary != null && resourceDictionary.ContainsKey("ExtGState"))
    {
      IPdfPrimitive pdfPrimitive = !(resourceDictionary["ExtGState"] is PdfDictionary) ? (resourceDictionary["ExtGState"] as PdfReferenceHolder).Object : resourceDictionary["ExtGState"];
      if (pdfPrimitive is PdfDictionary)
      {
        foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in ((PdfDictionary) pdfPrimitive).Items)
        {
          if ((object) (keyValuePair.Value as PdfReferenceHolder) != null)
          {
            PdfDictionary xobjectDictionary = (keyValuePair.Value as PdfReferenceHolder).Object as PdfDictionary;
            graphicResources.Add(keyValuePair.Key.Value, (object) new XObjectElement(xobjectDictionary, keyValuePair.Key.Value));
          }
          else
          {
            PdfDictionary xobjectDictionary = keyValuePair.Value as PdfDictionary;
            graphicResources.Add(keyValuePair.Key.Value, (object) new XObjectElement(xobjectDictionary, keyValuePair.Key.Value));
          }
        }
      }
    }
    return graphicResources;
  }

  internal Dictionary<string, object> GetColorSpaceResource(PdfDictionary resourceDic)
  {
    Dictionary<string, object> colorSpaceResource = new Dictionary<string, object>();
    if (resourceDic != null && resourceDic.ContainsKey("ColorSpace"))
    {
      IPdfPrimitive pdfPrimitive = !(resourceDic["ColorSpace"] is PdfDictionary) ? (resourceDic["ColorSpace"] as PdfReferenceHolder).Object : (IPdfPrimitive) (resourceDic["ColorSpace"] as PdfDictionary);
      if (pdfPrimitive is PdfDictionary)
      {
        foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in ((PdfDictionary) pdfPrimitive).Items)
        {
          if ((object) (keyValuePair.Value as PdfReferenceHolder) != null)
          {
            PdfArray refHolderColorspace = (keyValuePair.Value as PdfReferenceHolder).Object as PdfArray;
            colorSpaceResource.Add(keyValuePair.Key.Value, (object) new ExtendColorspace((IPdfPrimitive) refHolderColorspace));
          }
          if ((object) (keyValuePair.Value as PdfName) != null)
          {
            PdfName refHolderColorspace = keyValuePair.Value as PdfName;
            colorSpaceResource.Add(keyValuePair.Key.Value, (object) new ExtendColorspace((IPdfPrimitive) refHolderColorspace));
          }
          if (keyValuePair.Value is PdfArray refHolderColorspace1)
            colorSpaceResource.Add(keyValuePair.Key.Value, (object) new ExtendColorspace((IPdfPrimitive) refHolderColorspace1));
        }
      }
    }
    return colorSpaceResource;
  }

  internal Dictionary<string, object> GetShadingResource(PdfDictionary resourceDictionary)
  {
    Dictionary<string, object> shadingResource = new Dictionary<string, object>();
    if (resourceDictionary != null && resourceDictionary.ContainsKey("Shading") && resourceDictionary["Shading"] is PdfDictionary resource)
    {
      foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in resource.Items)
      {
        PdfReferenceHolder pdfReferenceHolder = keyValuePair.Value as PdfReferenceHolder;
        if (pdfReferenceHolder != (PdfReferenceHolder) null)
          shadingResource.Add(keyValuePair.Key.Value, (object) new ExtendColorspace(pdfReferenceHolder.Object));
      }
    }
    return shadingResource;
  }

  internal Dictionary<string, object> GetPatternResource(PdfDictionary resourceDictionary)
  {
    Dictionary<string, object> patternResource = new Dictionary<string, object>();
    if (resourceDictionary != null && resourceDictionary.ContainsKey("Pattern"))
    {
      IPdfPrimitive pdfPrimitive = !(resourceDictionary["Pattern"] is PdfDictionary) ? (resourceDictionary["Pattern"] as PdfReferenceHolder).Object : (IPdfPrimitive) (resourceDictionary["Pattern"] as PdfDictionary);
      if (pdfPrimitive is PdfDictionary)
      {
        foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in ((PdfDictionary) pdfPrimitive).Items)
        {
          if ((object) (keyValuePair.Value as PdfReferenceHolder) != null)
          {
            IPdfPrimitive refHolderColorspace1 = (IPdfPrimitive) ((keyValuePair.Value as PdfReferenceHolder).Object as PdfArray);
            if (refHolderColorspace1 != null)
              patternResource.Add(keyValuePair.Key.Value, (object) new ExtendColorspace((IPdfPrimitive) (refHolderColorspace1 as PdfArray)));
            IPdfPrimitive refHolderColorspace2 = (IPdfPrimitive) ((keyValuePair.Value as PdfReferenceHolder).Object as PdfDictionary);
            if (refHolderColorspace2 != null)
              patternResource.Add(keyValuePair.Key.Value, (object) new ExtendColorspace((IPdfPrimitive) (refHolderColorspace2 as PdfDictionary)));
          }
        }
      }
    }
    return patternResource;
  }

  internal Dictionary<string, object> GetFontResources(PdfDictionary resourceDictionary)
  {
    Dictionary<string, object> fontResources = new Dictionary<string, object>();
    if (resourceDictionary != null)
    {
      IPdfPrimitive resource = resourceDictionary["Font"];
      if (resource != null)
      {
        PdfDictionary pdfDictionary = (object) (resource as PdfReferenceHolder) == null ? resource as PdfDictionary : (resource as PdfReferenceHolder).Object as PdfDictionary;
        if (pdfDictionary != null)
        {
          foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in pdfDictionary.Items)
          {
            if ((object) (keyValuePair.Value as PdfReferenceHolder) != null)
            {
              if ((keyValuePair.Value as PdfReferenceHolder).Reference != (PdfReference) null)
                fontResources.Add(keyValuePair.Key.Value, (object) new FontStructure((keyValuePair.Value as PdfReferenceHolder).Object, (keyValuePair.Value as PdfReferenceHolder).Reference.ToString()));
              else
                fontResources.Add(keyValuePair.Key.Value, (object) new FontStructure((keyValuePair.Value as PdfReferenceHolder).Object));
            }
            else if (keyValuePair.Value is PdfDictionary fontDictionary)
              fontResources.Add(keyValuePair.Key.Value, (object) new FontStructure((IPdfPrimitive) fontDictionary));
            else
              fontResources.Add(keyValuePair.Key.Value, (object) new FontStructure(keyValuePair.Value, (keyValuePair.Value as PdfReferenceHolder).Reference.ToString()));
          }
        }
      }
    }
    return fontResources;
  }

  public string DecodeTest(PdfPageBase page, string fontName, string textToDecode)
  {
    PdfPageResources pageResources = this.GetPageResources(page);
    Encoding.Default.GetBytes(textToDecode);
    return (pageResources[fontName] as FontStructure).Decode(textToDecode, pageResources.isSameFont());
  }

  internal Dictionary<string, object> GetFontResources(
    PdfDictionary resourceDictionary,
    PdfPageBase page)
  {
    Dictionary<string, object> fontResources = new Dictionary<string, object>();
    if (resourceDictionary != null)
    {
      IPdfPrimitive resource = resourceDictionary["Font"];
      if (resource != null)
      {
        PdfDictionary pdfDictionary = (object) (resource as PdfReferenceHolder) == null ? resource as PdfDictionary : (resource as PdfReferenceHolder).Object as PdfDictionary;
        if (pdfDictionary != null)
        {
          foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in pdfDictionary.Items)
          {
            if ((object) (keyValuePair.Value as PdfReferenceHolder) != null)
            {
              if ((keyValuePair.Value as PdfReferenceHolder).Reference != (PdfReference) null)
                fontResources.Add(keyValuePair.Key.Value, (object) new FontStructure((keyValuePair.Value as PdfReferenceHolder).Object, (keyValuePair.Value as PdfReferenceHolder).Reference.ToString()));
              else
                fontResources.Add(keyValuePair.Key.Value, (object) new FontStructure((keyValuePair.Value as PdfReferenceHolder).Object));
            }
            else if (keyValuePair.Value is PdfDictionary fontDictionary)
              fontResources.Add(keyValuePair.Key.Value, (object) new FontStructure((IPdfPrimitive) fontDictionary));
            else
              fontResources.Add(keyValuePair.Key.Value, (object) new FontStructure(keyValuePair.Value, (keyValuePair.Value as PdfReferenceHolder).Reference.ToString()));
          }
        }
      }
      IPdfPrimitive pdfPrimitive1 = page.Dictionary["Parent"];
      if (pdfPrimitive1 != null)
      {
        IPdfPrimitive pdfPrimitive2 = new PdfResources((pdfPrimitive1 as PdfReferenceHolder).Object as PdfDictionary)["Font"];
        if (pdfPrimitive2 != null && pdfPrimitive2 is PdfDictionary pdfDictionary)
        {
          foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in pdfDictionary.Items)
          {
            if (keyValuePair.Value is PdfDictionary)
              fontResources.Add(keyValuePair.Key.Value, (object) (keyValuePair.Value as PdfReferenceHolder).Object);
            fontResources.Add(keyValuePair.Key.Value, (object) new FontStructure(keyValuePair.Value, (keyValuePair.Value as PdfReferenceHolder).Reference.ToString()));
          }
        }
      }
    }
    return fontResources;
  }

  internal Dictionary<string, object> GetImageResources(
    PdfDictionary resourceDictionary,
    PdfPageBase page,
    ref Dictionary<string, PdfMatrix> commonMatrix)
  {
    Dictionary<string, object> imageResources = new Dictionary<string, object>();
    if (resourceDictionary != null && resourceDictionary.ContainsKey("XObject"))
    {
      IPdfPrimitive pdfPrimitive = !(resourceDictionary["XObject"] is PdfDictionary) ? (resourceDictionary["XObject"] as PdfReferenceHolder).Object : resourceDictionary["XObject"];
      if (pdfPrimitive is PdfDictionary)
      {
        foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair1 in ((PdfDictionary) pdfPrimitive).Items)
        {
          if ((object) (keyValuePair1.Value as PdfReferenceHolder) != null)
          {
            if ((keyValuePair1.Value as PdfReferenceHolder).Object is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey("Subtype"))
            {
              if ((pdfDictionary["Subtype"] as PdfName).Value == "Image")
              {
                ImageStructure imageStructure = new ImageStructure((IPdfPrimitive) pdfDictionary, new PdfMatrix());
                if (commonMatrix.ContainsKey(keyValuePair1.Key.Value))
                  imageStructure = new ImageStructure((IPdfPrimitive) pdfDictionary, commonMatrix[keyValuePair1.Key.Value]);
                imageResources.Add(keyValuePair1.Key.Value, (object) imageStructure);
                this.m_hasImages = true;
              }
              else if ((pdfDictionary["Subtype"] as PdfName).Value == "Form" && page != null)
              {
                if (pdfDictionary.ContainsKey("Resources") && pdfDictionary["Resources"] is PdfDictionary)
                {
                  foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair2 in (pdfDictionary["Resources"] as PdfDictionary).Items)
                  {
                    if (keyValuePair2.Key.Value == "XObject" && keyValuePair2.Value is PdfDictionary)
                    {
                      Dictionary<PdfName, IPdfPrimitive> items = (keyValuePair2.Value as PdfDictionary).Items;
                      if (this.m_isExtractImages)
                      {
                        foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair3 in items)
                        {
                          if (!commonMatrix.ContainsKey(keyValuePair3.Key.Value))
                          {
                            PdfStream pdfStream = pdfDictionary as PdfStream;
                            pdfStream.Decompress();
                            MemoryStream internalStream = pdfStream.InternalStream;
                            internalStream.Position = 0L;
                            PdfMatrix pdfMatrix = new PdfMatrix(new PdfReader((Stream) internalStream)
                            {
                              Position = 0L
                            }, keyValuePair3.Key.Value, page.Size);
                            commonMatrix.Add(keyValuePair3.Key.Value, pdfMatrix);
                          }
                        }
                      }
                    }
                  }
                }
                imageResources.Add(keyValuePair1.Key.Value, (object) new XObjectElement(pdfDictionary, keyValuePair1.Key.Value));
              }
              if (!imageResources.ContainsKey(keyValuePair1.Key.Value))
              {
                imageResources.Add(keyValuePair1.Key.Value, (object) new XObjectElement(pdfDictionary, keyValuePair1.Key.Value));
                this.m_hasImages = true;
              }
            }
          }
          else if (keyValuePair1.Value is PdfDictionary xobjectDictionary)
          {
            imageResources.Add(keyValuePair1.Key.Value, (object) new XObjectElement(xobjectDictionary, keyValuePair1.Key.Value));
            this.m_hasImages = true;
          }
        }
      }
    }
    return imageResources;
  }

  internal PdfPageResources UpdatePageResources(
    PdfPageResources pageResources,
    Dictionary<string, object> objects)
  {
    foreach (KeyValuePair<string, object> keyValuePair in objects)
      pageResources.Add(keyValuePair.Key, keyValuePair.Value);
    return pageResources;
  }
}
