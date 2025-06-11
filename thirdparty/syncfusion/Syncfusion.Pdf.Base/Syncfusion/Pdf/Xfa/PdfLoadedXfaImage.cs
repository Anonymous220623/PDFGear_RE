// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfLoadedXfaImage
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;
using System.IO;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

internal class PdfLoadedXfaImage : PdfLoadedXfaStyledField
{
  internal string m_base64ImageData = string.Empty;
  internal XfaImageAspectRadio aspectRadio;
  internal string m_imageReference = string.Empty;

  internal void ReadField(XmlNode node, XmlDocument dataSetDoc)
  {
    this.currentNode = node;
    this.ReadCommonProperties(node);
    if (node["value"] != null && node["value"]["image"] != null)
    {
      XmlNode xmlNode = (XmlNode) node["value"]["image"];
      if (xmlNode.Attributes["aspect"] != null)
      {
        switch (xmlNode.Attributes["aspect"].Value.ToLower())
        {
          case "actual":
            this.aspectRadio = XfaImageAspectRadio.Actual;
            break;
          case "none":
            this.aspectRadio = XfaImageAspectRadio.None;
            break;
          default:
            this.aspectRadio = XfaImageAspectRadio.Proportionally;
            break;
        }
      }
      if (xmlNode.Attributes["href"] != null)
        this.m_imageReference = xmlNode.Attributes["href"].Value;
      string innerText = xmlNode.InnerText;
      try
      {
        Convert.FromBase64String(innerText);
        this.m_base64ImageData = innerText;
      }
      catch (Exception ex)
      {
      }
    }
    char[] chArray = new char[1]{ '.' };
    string str1 = string.Empty;
    if (node["bind"] != null)
    {
      XmlNode xmlNode = (XmlNode) node["bind"];
      if (xmlNode.Attributes["ref"] != null)
        str1 = xmlNode.Attributes["ref"].Value.Replace("$record.", "");
    }
    if (!(str1 != string.Empty))
      return;
    if (str1.StartsWith("$"))
      str1 = str1.Remove(0, 1);
    string str2 = str1.Replace(".", "/");
    string xpath = "//" + $"{this.parent.nodeName.Split(chArray)[0].Replace("[0]", "")}/{str2}";
    if (dataSetDoc == null)
      return;
    XmlNode xmlNode1 = dataSetDoc.SelectSingleNode(xpath);
    if (xmlNode1 == null || xmlNode1.FirstChild == null)
      return;
    this.m_base64ImageData = xmlNode1.FirstChild.InnerText;
  }

  internal void DrawImage(PdfGraphics graphics, RectangleF bounds, PdfLoadedDocument ldoc)
  {
    PdfBitmap image = (PdfBitmap) null;
    PdfDictionary fontDictionary = (PdfDictionary) null;
    if (this.m_imageReference != string.Empty)
    {
      PdfReferenceHolder pdfReferenceHolder = (PdfReferenceHolder) null;
      if (ldoc.Catalog != null)
        pdfReferenceHolder = ldoc.Catalog["Names"] as PdfReferenceHolder;
      if (pdfReferenceHolder != (PdfReferenceHolder) null)
      {
        if (pdfReferenceHolder.Object is PdfDictionary pdfDictionary1 && pdfDictionary1.Items.ContainsKey(new PdfName("XFAImages")))
          pdfReferenceHolder = pdfDictionary1.Items[new PdfName("XFAImages")] as PdfReferenceHolder;
        if (pdfReferenceHolder != (PdfReferenceHolder) null && pdfReferenceHolder.Object is PdfDictionary pdfDictionary2 && pdfDictionary2.Items.ContainsKey(new PdfName("Names")))
        {
          if (pdfDictionary2.Items[new PdfName("Names")] is PdfArray pdfArray)
          {
            for (int index = 0; index < pdfArray.Count; index += 2)
            {
              if ((pdfArray.Elements[index] as PdfString).Value == this.m_imageReference)
              {
                fontDictionary = (pdfArray.Elements[index + 1] as PdfReferenceHolder).Object as PdfDictionary;
                break;
              }
            }
          }
          if (fontDictionary != null)
          {
            ImageStructure imageStructure = new ImageStructure((IPdfPrimitive) fontDictionary, new PdfMatrix());
            if (imageStructure.EmbeddedImage != null)
            {
              image = new PdfBitmap(imageStructure.EmbeddedImage);
            }
            else
            {
              try
              {
                (fontDictionary as PdfStream).Decompress();
                image = new PdfBitmap(Image.FromStream((Stream) new MemoryStream((fontDictionary as PdfStream).Data)));
              }
              catch
              {
              }
            }
          }
        }
      }
    }
    if (this.m_base64ImageData != string.Empty)
    {
      try
      {
        image = new PdfBitmap((Stream) new MemoryStream(Convert.FromBase64String(this.m_base64ImageData)));
      }
      catch (Exception ex)
      {
      }
    }
    if (image == null)
      return;
    SizeF size = this.GetSize();
    RectangleF tempBounds = new RectangleF();
    tempBounds.Location = new PointF(bounds.Location.X + this.Margins.Left, bounds.Location.Y + this.Margins.Top);
    tempBounds.Size = new SizeF(size.Width - (this.Margins.Right + this.Margins.Left), size.Height - (this.Margins.Top + this.Margins.Bottom));
    graphics.Save();
    graphics.TranslateTransform(tempBounds.X, tempBounds.Y);
    graphics.RotateTransform((float) -this.GetRotationAngle());
    PdfUnitConvertor pdfUnitConvertor = new PdfUnitConvertor();
    double num1 = (double) pdfUnitConvertor.ConvertFromPixels((float) image.Width, PdfGraphicsUnit.Point);
    double num2 = (double) pdfUnitConvertor.ConvertFromPixels((float) image.Height, PdfGraphicsUnit.Point);
    if (this.aspectRadio == XfaImageAspectRadio.Proportionally)
    {
      float num3 = Math.Min(tempBounds.Width / (float) image.Width, tempBounds.Height / (float) image.Height);
      tempBounds.Width = (float) image.Width * num3;
      tempBounds.Height = (float) image.Height * num3;
    }
    RectangleF renderingRect = this.GetRenderingRect(tempBounds);
    graphics.DrawImage((PdfImage) image, renderingRect);
    graphics.Restore();
  }
}
