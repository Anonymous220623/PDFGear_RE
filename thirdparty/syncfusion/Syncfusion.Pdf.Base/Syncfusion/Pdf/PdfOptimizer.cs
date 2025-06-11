// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfOptimizer
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Compression;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Graphics.Fonts;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using Syncfusion.Pdf.Security;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf;

internal class PdfOptimizer
{
  private PdfCompressionOptions m_options;
  private List<PdfReference> m_fontReferenceCollection = new List<PdfReference>();
  private List<PdfReference> m_imageReferenceCollecction = new List<PdfReference>();
  private List<string> TtTableList = new List<string>();
  private List<PdfReference> m_xobjectReferenceCollection = new List<PdfReference>();
  private List<PdfReference> m_compressedXobject = new List<PdfReference>();
  private PdfLoadedDocument m_loadedDocument;
  private List<string> m_usedFontList = new List<string>();
  private Dictionary<string, IPdfPrimitive> m_resourceCollection = new Dictionary<string, IPdfPrimitive>();
  private bool m_identicalResource;
  private List<PdfReference> m_sMaskReference = new List<PdfReference>();
  private Dictionary<string, List<string>> fontUsedText = new Dictionary<string, List<string>>();
  private bool isPdfPage;

  internal PdfOptimizer(PdfLoadedDocument loadedDocument)
  {
    this.m_options = loadedDocument.CompressionOptions;
    this.TtTableList.Add("OS/2");
    this.TtTableList.Add("cmap");
    this.TtTableList.Add("cvt ");
    this.TtTableList.Add("fpgm");
    this.TtTableList.Add("glyf");
    this.TtTableList.Add("head");
    this.TtTableList.Add("hhea");
    this.TtTableList.Add("hmtx");
    this.TtTableList.Add("loca");
    this.TtTableList.Add("maxp");
    this.TtTableList.Add("name");
    this.TtTableList.Add("post");
    this.TtTableList.Add("prep");
    this.Optimize(loadedDocument);
  }

  internal PdfOptimizer()
  {
    this.TtTableList.Add("OS/2");
    this.TtTableList.Add("cmap");
    this.TtTableList.Add("cvt ");
    this.TtTableList.Add("fpgm");
    this.TtTableList.Add("glyf");
    this.TtTableList.Add("head");
    this.TtTableList.Add("hhea");
    this.TtTableList.Add("hmtx");
    this.TtTableList.Add("loca");
    this.TtTableList.Add("maxp");
    this.TtTableList.Add("name");
    this.TtTableList.Add("post");
    this.TtTableList.Add("prep");
  }

  internal void Optimize(PdfLoadedDocument lDoc)
  {
    this.m_loadedDocument = lDoc;
    lDoc.FileStructure.IncrementalUpdate = false;
    lDoc.isCompressed = true;
    if (this.m_options.CompressImages || this.m_options.OptimizeFont || this.m_options.OptimizePageContents)
    {
      this.FindIdenticalResoucres(lDoc);
      foreach (PdfPageBase page in lDoc.Pages)
      {
        if (this.m_options.OptimizePageContents)
          this.OptimizePageContent(page);
        this.OptimizePageResources(page);
        if (this.m_options.OptimizePageContents && page is PdfLoadedPage)
          this.OptimizeAnnotations(page as PdfLoadedPage);
      }
    }
    if (this.m_options.RemoveMetadata)
      this.RemoveMetaData(lDoc.Catalog);
    this.m_sMaskReference.Clear();
    this.fontUsedText.Clear();
  }

  internal void Close()
  {
    this.m_fontReferenceCollection.Clear();
    this.m_imageReferenceCollecction.Clear();
  }

  private void OptimizeAnnotations(PdfLoadedPage lPage)
  {
    lPage.GetWidgetReferences();
    if (!lPage.Dictionary.ContainsKey("Annots"))
      return;
    PdfArray pdfArray = lPage.CrossTable.GetObject(lPage.Dictionary["Annots"]) as PdfArray;
    PdfDocumentBase document = lPage.Document;
    if (pdfArray == null)
      return;
    for (int index = 0; index < pdfArray.Count; ++index)
      this.OptimizeApperance(lPage.CrossTable.GetObject(pdfArray[index]) as PdfDictionary);
  }

  private void FindIdenticalResoucres(PdfLoadedDocument pdfLoaded)
  {
    PdfReferenceHolder pdfReferenceHolder1 = (PdfReferenceHolder) null;
    foreach (PdfPageBase page in pdfLoaded.Pages)
    {
      if (page.Dictionary.ContainsKey("Resources"))
      {
        PdfReferenceHolder pdfReferenceHolder2 = page.Dictionary["Resources"] as PdfReferenceHolder;
        if (pdfReferenceHolder2 != (PdfReferenceHolder) null && pdfReferenceHolder1 == (PdfReferenceHolder) null)
          pdfReferenceHolder1 = pdfReferenceHolder2;
        else if (pdfReferenceHolder2 != (PdfReferenceHolder) null && pdfReferenceHolder1 != (PdfReferenceHolder) null && pdfReferenceHolder2.Reference == pdfReferenceHolder1.Reference)
        {
          this.m_identicalResource = true;
          break;
        }
      }
    }
  }

  private void OptimizeApperance(PdfDictionary widgetDictionary)
  {
    if (!this.m_options.OptimizePageContents || widgetDictionary == null || !widgetDictionary.ContainsKey("AP") || !(this.GetObject(widgetDictionary, new PdfName("AP")) is PdfDictionary pdfDictionary) || !pdfDictionary.ContainsKey("N") || !(this.GetObject(pdfDictionary["N"]) is PdfStream contentStream))
      return;
    this.OptimizeContent(contentStream, false);
  }

  private void OptimizePageResources(PdfPageBase lPage)
  {
    this.OptimizeResources((PdfDictionary) lPage.GetResources(), lPage);
  }

  private void OptimizePageContent(PdfPageBase lPage)
  {
    if (lPage is PdfPage)
      this.isPdfPage = true;
    PdfArray contents = lPage.Contents;
    MemoryStream contentStream = new MemoryStream();
    lPage.Layers.isOptimizeContent = true;
    lPage.Layers.CombineContent((Stream) contentStream);
    MemoryStream memoryStream = this.OptimizeContent(contentStream);
    PdfStream pdfStream1 = new PdfStream();
    pdfStream1.Data = memoryStream.ToArray();
    pdfStream1.Compress = true;
    memoryStream.Dispose();
    if (lPage.Dictionary.ContainsKey("Contents"))
    {
      if (lPage.Dictionary["Contents"] is PdfArray pdfArray)
      {
        foreach (IPdfPrimitive content in lPage.Contents)
        {
          PdfDictionary pdfDictionary = this.GetObject(content);
          if (pdfDictionary != null)
            pdfDictionary.isSkip = true;
        }
        pdfArray.Clear();
        pdfArray.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfStream1));
      }
      else if (lPage.Dictionary["Contents"] is PdfStream pdfStream2)
      {
        pdfStream2.Clear();
        pdfStream2.Items.Remove(new PdfName("Length"));
        pdfStream2.Data = pdfStream1.Data;
        pdfStream2.Compress = true;
      }
    }
    if (lPage.Dictionary.ContainsKey("PieceInfo"))
      lPage.Dictionary.Remove("PieceInfo");
    lPage.Layers.isOptimizeContent = false;
  }

  private MemoryStream OptimizeContent(MemoryStream contentStream)
  {
    if (contentStream != null && contentStream.Length > 0L && contentStream.Length > 1L)
    {
      PdfRecordCollection recordCollection = new ContentParser(contentStream.ToArray()).ReadContent();
      PdfStream pdfStream = new PdfStream();
      string currentFont = (string) null;
      int count1 = recordCollection.RecordCollection.Count;
      for (int index1 = 0; index1 < count1; ++index1)
      {
        PdfRecord record = recordCollection.RecordCollection[index1];
        if (record.Operands != null && record.Operands.Length >= 1)
        {
          if (this.m_options.OptimizeFont && record.OperatorName == "Tf")
          {
            if (!this.m_usedFontList.Contains(record.Operands[0].TrimStart('/')))
              this.m_usedFontList.Add(record.Operands[0].TrimStart('/'));
            currentFont = record.Operands[0].TrimStart('/');
          }
          if (this.m_options.OptimizeFont && (record.OperatorName == "Tj" || record.OperatorName == "TJ" || record.OperatorName == "'"))
            this.AddUsedFontText(currentFont, record);
          if (record.OperatorName == "ID")
          {
            StringBuilder stringBuilder = new StringBuilder();
            for (int index2 = 0; index2 < record.Operands.Length; ++index2)
            {
              if (index2 + 1 < record.Operands.Length && record.Operands[index2].Contains("/") && record.Operands[index2 + 1].Contains("/"))
              {
                stringBuilder.Append(record.Operands[index2]);
                stringBuilder.Append(" ");
                stringBuilder.Append(record.Operands[index2 + 1]);
                stringBuilder.Append("\r\n");
                ++index2;
              }
              else if (index2 + 1 < record.Operands.Length && record.Operands[index2].Contains("/"))
              {
                stringBuilder.Append(record.Operands[index2]);
                stringBuilder.Append(" ");
                stringBuilder.Append(record.Operands[index2 + 1]);
                stringBuilder.Append("\r\n");
                ++index2;
              }
              else
              {
                stringBuilder.Append(record.Operands[index2]);
                stringBuilder.Append("\r\n");
              }
            }
            byte[] bytes = Encoding.Default.GetBytes(stringBuilder.ToString());
            pdfStream.Write(bytes);
          }
          else
          {
            for (int index3 = 0; index3 < record.Operands.Length; ++index3)
            {
              string operand = record.Operands[index3];
              if (record.OperatorName != "Tj" && record.OperatorName != "'" && record.OperatorName != "\"" && record.OperatorName != "TJ")
                operand = this.TrimOperand(operand);
              PdfString pdfString = new PdfString(operand);
              pdfStream.Write(pdfString.Bytes);
              if (record.OperatorName != "Tj" && record.OperatorName != "'" && record.OperatorName != "\"" && record.OperatorName != "TJ")
                pdfStream.Write(" ");
            }
          }
        }
        else if (record.Operands == null && record.InlineImageBytes != null)
        {
          byte[] bytes = Encoding.Default.GetBytes(Encoding.Default.GetString(record.InlineImageBytes));
          pdfStream.Write(bytes);
          pdfStream.Write(" ");
        }
        pdfStream.Write(record.OperatorName);
        if (index1 + 1 < count1 || this.isPdfPage)
        {
          if (record.OperatorName == "ID")
            pdfStream.Write("\n");
          else if (index1 + 1 < count1 && (record.OperatorName == "W" || record.OperatorName == "W*") && recordCollection.RecordCollection[index1 + 1].OperatorName == "n")
            pdfStream.Write(" ");
          else if (record.OperatorName == "w" || record.OperatorName == "EI")
            pdfStream.Write(" ");
          else
            pdfStream.Write("\r\n");
        }
      }
      if ((long) pdfStream.Data.Length < contentStream.Length)
      {
        contentStream.Close();
        contentStream = new MemoryStream();
        byte[] buffer = new byte[4096 /*0x1000*/];
        pdfStream.InternalStream.Position = 0L;
        int count2;
        while ((count2 = pdfStream.InternalStream.Read(buffer, 0, buffer.Length)) > 0)
          contentStream.Write(buffer, 0, count2);
        pdfStream.Clear();
        pdfStream.InternalStream.Dispose();
        pdfStream.InternalStream.Close();
        pdfStream.InternalStream = (MemoryStream) null;
      }
    }
    return contentStream;
  }

  private void OptimizeContent(PdfStream contentStream, bool isPageContent)
  {
    if (contentStream == null || contentStream.Data.Length <= 0)
      return;
    contentStream.Decompress();
    if (contentStream.Data.Length <= 1)
      return;
    PdfRecordCollection recordCollection = new ContentParser(contentStream.InternalStream.ToArray()).ReadContent();
    PdfStream pdfStream = new PdfStream();
    string currentFont = (string) null;
    int count = recordCollection.RecordCollection.Count;
    for (int index1 = 0; index1 < count; ++index1)
    {
      PdfRecord record = recordCollection.RecordCollection[index1];
      if (record.Operands != null && record.Operands.Length >= 1)
      {
        if (this.m_options.OptimizeFont && record.OperatorName == "Tf")
        {
          if (!this.m_usedFontList.Contains(record.Operands[0].TrimStart('/')))
            this.m_usedFontList.Add(record.Operands[0].TrimStart('/'));
          currentFont = record.Operands[0].TrimStart('/');
        }
        if (this.m_options.OptimizeFont && (record.OperatorName == "Tj" || record.OperatorName == "TJ" || record.OperatorName == "'"))
          this.AddUsedFontText(currentFont, record);
        if (record.OperatorName == "ID")
        {
          StringBuilder stringBuilder = new StringBuilder();
          for (int index2 = 0; index2 < record.Operands.Length; ++index2)
          {
            if (index2 + 1 < record.Operands.Length && record.Operands[index2].Contains("/") && record.Operands[index2 + 1].Contains("/"))
            {
              stringBuilder.Append(record.Operands[index2]);
              stringBuilder.Append(" ");
              stringBuilder.Append(record.Operands[index2 + 1]);
              stringBuilder.Append("\r\n");
              ++index2;
            }
            else if (index2 + 1 < record.Operands.Length && record.Operands[index2].Contains("/"))
            {
              stringBuilder.Append(record.Operands[index2]);
              stringBuilder.Append(" ");
              stringBuilder.Append(record.Operands[index2 + 1]);
              stringBuilder.Append("\r\n");
              ++index2;
            }
            else
            {
              stringBuilder.Append(record.Operands[index2]);
              stringBuilder.Append("\r\n");
            }
          }
          byte[] bytes = Encoding.Default.GetBytes(stringBuilder.ToString());
          pdfStream.Write(bytes);
        }
        else
        {
          for (int index3 = 0; index3 < record.Operands.Length; ++index3)
          {
            string operand = record.Operands[index3];
            if (record.OperatorName != "Tj" && record.OperatorName != "'" && record.OperatorName != "\"" && record.OperatorName != "TJ")
              operand = this.TrimOperand(operand);
            PdfString pdfString = new PdfString(operand);
            pdfStream.Write(pdfString.Bytes);
            if (record.OperatorName != "Tj" && record.OperatorName != "'" && record.OperatorName != "\"" && record.OperatorName != "TJ")
              pdfStream.Write(" ");
          }
        }
      }
      else if (record.Operands == null && record.InlineImageBytes != null)
      {
        byte[] bytes = Encoding.Default.GetBytes(Encoding.Default.GetString(record.InlineImageBytes));
        pdfStream.Write(bytes);
        pdfStream.Write(" ");
      }
      pdfStream.Write(record.OperatorName);
      if (index1 + 1 < count || this.isPdfPage)
      {
        if (record.OperatorName == "ID")
          pdfStream.Write("\n");
        else if (index1 + 1 < count && (record.OperatorName == "W" || record.OperatorName == "W*") && recordCollection.RecordCollection[index1 + 1].OperatorName == "n")
          pdfStream.Write(" ");
        else if (record.OperatorName == "w" || record.OperatorName == "EI")
          pdfStream.Write(" ");
        else
          pdfStream.Write("\r\n");
      }
    }
    if (isPageContent || pdfStream.Data.Length < contentStream.Data.Length)
    {
      contentStream.Clear();
      contentStream.Items.Remove(new PdfName("Length"));
      contentStream.Data = pdfStream.Data;
      pdfStream.Clear();
      pdfStream.InternalStream.Dispose();
      pdfStream.InternalStream.Close();
      pdfStream.InternalStream = (MemoryStream) null;
    }
    contentStream.Compress = true;
  }

  private string TrimOperand(string operand)
  {
    if (operand == ".00")
      operand = "0";
    if (operand.Contains(".00"))
    {
      string[] strArray = operand.Split('.');
      if (strArray.Length == 2 && strArray[1] == "00" && strArray[0] != "-")
        operand = strArray[0];
    }
    return operand;
  }

  private void RemoveMetaData(PdfCatalog catalog)
  {
    if (!catalog.ContainsKey("Metadata"))
      return;
    if (!(catalog["Metadata"] is PdfDictionary pdfDictionary))
      pdfDictionary = (catalog["Metadata"] as PdfReferenceHolder).Object as PdfDictionary;
    if (pdfDictionary != null)
      pdfDictionary.isSkip = true;
    catalog.Remove(new PdfName("Metadata"));
  }

  private void OptimizeResources(PdfDictionary resource, PdfPageBase lPage)
  {
    if (resource.ContainsKey("Font") && this.m_options.OptimizeFont)
      this.OptimizeFont(resource, lPage);
    if (resource.ContainsKey("XObject"))
      this.OptimizeXObect(resource, lPage);
    if (!resource.ContainsKey("Resources") || !(this.GetObject(resource, new PdfName("Resources")) is PdfDictionary resource1))
      return;
    this.OptimizeResources(resource1, lPage);
  }

  private void OptimizeFont(PdfDictionary resource, PdfPageBase lPage)
  {
    if (!(this.GetObject(resource, new PdfName("Font")) is PdfDictionary pdfDictionary1))
      return;
    Dictionary<PdfName, IPdfPrimitive> dictionary = new Dictionary<PdfName, IPdfPrimitive>();
    foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in pdfDictionary1.Items)
      dictionary.Add(keyValuePair.Key, keyValuePair.Value);
    foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in dictionary)
    {
      if ((this.m_options.OptimizePageContents && this.m_usedFontList.Contains(keyValuePair.Key.Value) || !this.m_options.OptimizePageContents) && (object) (keyValuePair.Value as PdfReferenceHolder) != null)
      {
        PdfReferenceHolder pdfReferenceHolder1 = keyValuePair.Value as PdfReferenceHolder;
        if (!this.m_fontReferenceCollection.Contains(pdfReferenceHolder1.Reference))
        {
          this.m_fontReferenceCollection.Add(pdfReferenceHolder1.Reference);
          PdfDictionary pdfDictionary2 = pdfReferenceHolder1.Object as PdfDictionary;
          PdfStream fontFile2 = (PdfStream) null;
          PdfCrossTable pdfCrossTable = lPage is PdfPage ? (lPage as PdfPage).CrossTable : (lPage as PdfLoadedPage).CrossTable;
          pdfCrossTable.GetObject(pdfDictionary2["BaseFont"]);
          string str = string.Empty;
          if (pdfDictionary2.ContainsKey("Subtype"))
          {
            PdfName pdfName = pdfCrossTable.GetObject(pdfDictionary2["Subtype"]) as PdfName;
            if (pdfName.Value == "TrueType")
            {
              str = "TrueType";
              PdfReferenceHolder pdfReferenceHolder2 = pdfDictionary2["FontDescriptor"] as PdfReferenceHolder;
              if (pdfReferenceHolder2 != (PdfReferenceHolder) null && pdfReferenceHolder2.Object is PdfDictionary pdfDictionary3 && pdfDictionary3.ContainsKey("FontFile2"))
              {
                PdfReferenceHolder pdfReferenceHolder3 = pdfDictionary3["FontFile2"] as PdfReferenceHolder;
                if (pdfReferenceHolder3 != (PdfReferenceHolder) null)
                  fontFile2 = pdfReferenceHolder3.Object as PdfStream;
              }
            }
            else if (pdfName.Value == "Type0")
            {
              str = "Type0";
              if (!(pdfDictionary2["DescendantFonts"] is PdfArray pdfArray))
              {
                PdfReferenceHolder pdfReferenceHolder4 = pdfDictionary2["DescendantFonts"] as PdfReferenceHolder;
                if (pdfReferenceHolder4 != (PdfReferenceHolder) null)
                  pdfArray = pdfReferenceHolder4.Object as PdfArray;
              }
              PdfDictionary pdfDictionary4 = (object) (pdfArray[0] as PdfReferenceHolder) != null ? (pdfArray[0] as PdfReferenceHolder).Object as PdfDictionary : pdfArray[0] as PdfDictionary;
              if (pdfDictionary4 != null && PdfCrossTable.Dereference(pdfDictionary4["FontDescriptor"]) is PdfDictionary pdfDictionary5 && pdfDictionary5.ContainsKey("FontFile2"))
              {
                PdfReferenceHolder pdfReferenceHolder5 = pdfDictionary5["FontFile2"] as PdfReferenceHolder;
                if (pdfReferenceHolder5 != (PdfReferenceHolder) null && lPage is PdfLoadedPage)
                  fontFile2 = pdfReferenceHolder5.Object as PdfStream;
              }
            }
          }
          if (fontFile2 != null)
          {
            switch (str)
            {
              case "Type0":
                if (this.m_options.OptimizeFont)
                {
                  fontFile2.Decompress();
                  if (fontFile2.Data.Length > 0)
                  {
                    FontFile2 ff2 = new FontFile2(fontFile2.Data);
                    this.OptimizeType0Font(keyValuePair.Key.Value, ff2, fontFile2, pdfDictionary1[keyValuePair.Key] as PdfReferenceHolder);
                    continue;
                  }
                  continue;
                }
                continue;
              case "TrueType":
                if (this.m_options.OptimizeFont)
                {
                  fontFile2.Decompress();
                  FontFile2 f2 = new FontFile2(fontFile2.Data);
                  if (f2.tableList.Contains("glyf") && f2.tableList.Contains("loca"))
                  {
                    MemoryStream fontStream = this.OptimizeTrueTypeFont(f2) as MemoryStream;
                    this.UpdateFontData(fontStream, fontFile2);
                    fontStream.Dispose();
                    continue;
                  }
                  fontFile2.Compress = true;
                  continue;
                }
                continue;
              default:
                continue;
            }
          }
        }
      }
      else if (!this.m_identicalResource)
        pdfDictionary1.Remove(keyValuePair.Key);
    }
  }

  private void OptimizeXObect(PdfDictionary resource, PdfPageBase lPage)
  {
    if (!(this.GetObject(resource, new PdfName("XObject")) is PdfDictionary xObjectDictionary))
      return;
    Dictionary<PdfName, IPdfPrimitive> xObjectChilds = new Dictionary<PdfName, IPdfPrimitive>();
    foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in xObjectDictionary.Items)
      xObjectChilds.Add(keyValuePair.Key, keyValuePair.Value);
    this.GetSmaskReference(xObjectChilds, lPage);
    foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in xObjectChilds)
    {
      PdfDictionary pdfDictionary = this.GetObject(keyValuePair.Value);
      if (pdfDictionary != null && pdfDictionary.ContainsKey("Subtype"))
      {
        PdfName pdfName = pdfDictionary["Subtype"] as PdfName;
        if (pdfName.Value == "Image")
        {
          bool flag = false;
          if (pdfDictionary is PdfStream)
            flag = (pdfDictionary as PdfStream).isCustomQuality;
          if (this.m_options.CompressImages && !flag)
          {
            PdfReference reference = (lPage is PdfPage ? (lPage as PdfPage).CrossTable : (lPage as PdfLoadedPage).CrossTable).GetReference(keyValuePair.Value);
            this.ReplaceImage(lPage, keyValuePair.Key, reference, pdfDictionary, xObjectDictionary);
          }
          if (pdfDictionary is PdfStream pdfStream)
          {
            string hashValue = string.Empty;
            if (pdfStream.InternalStream.CanWrite && this.CompareStream(pdfStream.InternalStream, out hashValue))
            {
              if (this.m_resourceCollection[hashValue] is PdfDictionary resource1 && resource1.ContainsKey("SMask") && pdfStream.ContainsKey("SMask"))
              {
                string str1 = string.Empty;
                string str2 = string.Empty;
                PdfStream pdfStream1 = PdfCrossTable.Dereference(pdfStream["SMask"]) as PdfStream;
                PdfStream pdfStream2 = PdfCrossTable.Dereference(resource1["SMask"]) as PdfStream;
                if (pdfStream1 != null && pdfStream2 != null)
                {
                  pdfStream1.InternalStream.Position = 0L;
                  str1 = this.CreateHashFromStream(pdfStream1.InternalStream.ToArray());
                  pdfStream2.InternalStream.Position = 0L;
                  str2 = this.CreateHashFromStream(pdfStream2.InternalStream.ToArray());
                }
                if (str1 != string.Empty && str2 != string.Empty && str1 == str2)
                {
                  xObjectDictionary.Items.Remove(keyValuePair.Key);
                  xObjectDictionary.Items.Add(keyValuePair.Key, (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) resource1));
                }
              }
              else if (resource1 != null && !pdfStream.ContainsKey("SMask") && !resource1.ContainsKey("SMask"))
              {
                xObjectDictionary.Items.Remove(keyValuePair.Key);
                xObjectDictionary.Items.Add(keyValuePair.Key, (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) resource1));
              }
            }
            else if (hashValue != string.Empty)
              this.m_resourceCollection.Add(hashValue, (IPdfPrimitive) pdfDictionary);
          }
        }
        else if (pdfName.Value == "Form")
        {
          if (this.m_options.OptimizePageContents)
          {
            bool flag = true;
            if ((object) (keyValuePair.Value as PdfReferenceHolder) != null)
            {
              PdfReference reference = (keyValuePair.Value as PdfReferenceHolder).Reference;
              if (reference != (PdfReference) null)
              {
                if (!this.m_compressedXobject.Contains(reference))
                  this.m_compressedXobject.Add(reference);
                else
                  flag = false;
              }
            }
            if (flag)
              this.OptimizeContent(pdfDictionary as PdfStream, false);
          }
          bool flag1 = false;
          if ((object) (keyValuePair.Value as PdfReferenceHolder) != null)
          {
            PdfReference reference = (keyValuePair.Value as PdfReferenceHolder).Reference;
            if (reference != (PdfReference) null)
            {
              if (this.m_xobjectReferenceCollection.Contains(reference))
                flag1 = true;
              else
                this.m_xobjectReferenceCollection.Add(reference);
            }
          }
          if (!flag1)
            this.OptimizeResources(pdfDictionary, lPage);
        }
      }
    }
  }

  private FontFile2 UpdateFontStream(
    string fontKey,
    PdfReferenceHolder fontList,
    TtfReader ttfReader)
  {
    FontFile2 fontFile2 = (FontFile2) null;
    List<string> stringList = this.fontUsedText[fontKey];
    string empty = string.Empty;
    FontStructure fontStructure = new FontStructure(fontList.Object, fontList.Reference.ToString());
    for (int index = 0; index < stringList.Count; ++index)
      empty += fontStructure.Decode(stringList[index], false);
    if (empty.Length > 0)
    {
      char[] charArray = empty.ToCharArray();
      ConcurrentDictionary<char, char> chars = new ConcurrentDictionary<char, char>();
      for (int index = 0; index < charArray.Length; ++index)
      {
        char key = charArray[index];
        chars[key] = char.MinValue;
      }
      byte[] data = ttfReader.ReadFontProgram(chars);
      if (fontList.Object is PdfDictionary)
        this.UpdateFontName(fontList.Object as PdfDictionary, ttfReader);
      fontFile2 = new FontFile2(data);
    }
    return fontFile2;
  }

  private void AddUsedFontText(string currentFont, PdfRecord record)
  {
    if (this.fontUsedText.ContainsKey(currentFont))
    {
      List<string> stringList = this.fontUsedText[currentFont];
      if (stringList.Contains(record.Operands[0]))
        return;
      stringList.Add(record.Operands[0]);
    }
    else
      this.fontUsedText[currentFont] = new List<string>()
      {
        record.Operands[0]
      };
  }

  private void ReplaceImage(
    PdfPageBase lPage,
    PdfName key,
    PdfReference oldReference,
    PdfDictionary imgDict,
    PdfDictionary xObjectDictionary)
  {
    if (this.m_imageReferenceCollecction.Contains(oldReference) || this.m_sMaskReference.Contains(oldReference))
      return;
    this.m_imageReferenceCollecction.Add(oldReference);
    bool flag1 = false;
    if (imgDict is PdfStream)
      flag1 = (imgDict as PdfStream).isImageDualFilter;
    if (flag1 || this.ImageInterpolated(imgDict))
      return;
    ImageStructure imageStructure = new ImageStructure((IPdfPrimitive) imgDict, new PdfMatrix());
    PdfStream imageDictionary = imageStructure.ImageDictionary as PdfStream;
    if (imageStructure.EmbeddedImage == null || !imageDictionary.InternalStream.CanWrite)
      return;
    PdfArray pdfArray = PdfCrossTable.Dereference(imgDict["ColorSpace"]) as PdfArray;
    PdfBitmap pdfBitmap = new PdfBitmap(imageStructure.EmbeddedImage);
    pdfBitmap.Quality = (long) this.m_options.ImageQuality;
    pdfBitmap.Save();
    PdfStream stream = pdfBitmap.Stream;
    if (stream.Data.Length < (imgDict as PdfStream).Data.Length)
    {
      if (imgDict.ContainsKey("SMask") && this.GetObject(this.GetObject(xObjectDictionary, key) as PdfDictionary, new PdfName("SMask")) is PdfStream pdfStream)
        pdfStream.isSkip = true;
      if (pdfArray != null)
      {
        PdfName pdfName = PdfCrossTable.Dereference(pdfArray[0]) as PdfName;
        if (pdfName != (PdfName) null && pdfName.Value == "ICCBased")
          stream["ColorSpace"] = (IPdfPrimitive) pdfArray;
      }
      PdfReferenceHolder imageReference = new PdfReferenceHolder((IPdfPrimitive) stream);
      lPage.isFlateCompress = true;
      lPage.ReplaceImageStream(oldReference.ObjNum, imageReference, lPage);
      lPage.isFlateCompress = false;
    }
    else if (imageStructure.EmbeddedImage.RawFormat.Equals((object) ImageFormat.Png) && imgDict.ContainsKey("Filter"))
    {
      bool flag2 = false;
      if ((object) (imgDict["Filter"] as PdfName) != null)
        flag2 = (imgDict["Filter"] as PdfName).Value == "FlateDecode" && !imgDict.ContainsKey("Interpolate");
      if (flag2 && stream.ContainsKey("Filter") && "DCTDecode" == (stream["Filter"] as PdfName).Value && stream.ContainsKey("DecodeParms"))
      {
        PdfCompressionLevel level = this.m_loadedDocument.Compression;
        if (this.m_loadedDocument.isCompressed)
          level = PdfCompressionLevel.Best;
        byte[] numArray = new PdfZlibCompressor(level).Compress(stream.Data);
        if (numArray.Length < (imgDict as PdfStream).Data.Length)
        {
          if (imgDict.ContainsKey("SMask") && this.GetObject(this.GetObject(xObjectDictionary, key) as PdfDictionary, new PdfName("SMask")) is PdfStream pdfStream)
            pdfStream.isSkip = true;
          PdfReferenceHolder imageReference = new PdfReferenceHolder((IPdfPrimitive) stream);
          lPage.isFlateCompress = true;
          lPage.ReplaceImageStream(oldReference.ObjNum, imageReference, lPage);
          lPage.isFlateCompress = false;
        }
        System.Array.Clear((System.Array) numArray, 0, numArray.Length);
      }
    }
    pdfBitmap.Dispose();
  }

  private bool ImageInterpolated(PdfDictionary imageDictionary)
  {
    bool flag = false;
    if (imageDictionary != null && imageDictionary.ContainsKey("Interpolate"))
      flag = (imageDictionary["Interpolate"] as PdfBoolean).Value;
    return (!flag || imageDictionary.ContainsKey("SMask")) && flag;
  }

  private Stream OptimizeTrueTypeFont(FontFile2 f2)
  {
    List<TableEntry> entries = new List<TableEntry>();
    foreach (string table in f2.tableList)
    {
      if (table == "OS/2" || table == "cmap" || table == "cvt " || table == "fpgm" || table == "glyf" || table == "head" || table == "hhea" || table == "hmtx" || table == "loca" || table == "maxp" || table == "name" || table == "post" || table == "prep")
      {
        int tableId = f2.getTableID(table);
        byte[] tableBytes = f2.getTableBytes(tableId, true);
        if (tableBytes != null)
          entries.Add(this.GetTableEntry(table == "OS2" ? "OS/2" : table, tableBytes));
      }
    }
    MemoryStream fontStream = new FontDecode().CreateFontStream(entries);
    fontStream.Position = 0L;
    return (Stream) fontStream;
  }

  private void OptimizeType0Font(
    string fontKey,
    FontFile2 ff2,
    PdfStream fontFile2,
    PdfReferenceHolder fontList)
  {
    FontFile2 f2 = ff2;
    int localTableLength1 = 0;
    int missedGlyphs = 0;
    int localTableGlyph = 0;
    bool isSkip = false;
    bool flag1 = false;
    bool bIsLocaShort = false;
    bool isSymbol = false;
    bool flag2 = false;
    PdfDictionary fontDictionary1 = PdfCrossTable.Dereference((IPdfPrimitive) fontList) as PdfDictionary;
    bool flag3 = false;
    if (fontDictionary1 != null && fontDictionary1.ContainsKey("ToUnicode"))
      flag3 = this.IsOptimizeUsedCharacters(fontDictionary1, fontFile2);
    if (flag3 || (f2.tableList[0] == "DSIG" || f2.tableList[0] == "GDEF" || f2.tableList[0] == "LTSH") && f2.tableList.Count > 13 && !f2.tableList.Contains("EBLC") && !f2.tableList.Contains("EBDT"))
    {
      TtfReader ttfReader = new TtfReader(!fontFile2.InternalStream.CanSeek ? new BinaryReader((Stream) new MemoryStream(fontFile2.Data), TtfReader.Encoding) : new BinaryReader((Stream) fontFile2.InternalStream, TtfReader.Encoding));
      ttfReader.CreateInternals();
      if (ttfReader.Metrics.PostScriptName != null && !ttfReader.Metrics.IsSymbol && !ttfReader.Metrics.PostScriptName.Contains("Symbol"))
      {
        flag2 = true;
        FontStructure fontStructure = new FontStructure(fontList.Object, fontList.Reference.ToString());
        ConcurrentDictionary<char, char> chars = new ConcurrentDictionary<char, char>();
        Dictionary<double, string> characterMapTable = fontStructure.CharacterMapTable;
        Dictionary<int, OtfGlyphInfo> otGlyphs = new Dictionary<int, OtfGlyphInfo>();
        bool flag4 = ttfReader.isOTFFont();
        if (characterMapTable.Count > 0)
        {
          if (flag4)
          {
            foreach (KeyValuePair<double, string> keyValuePair in characterMapTable)
            {
              TtfGlyphInfo ttfGlyphInfo = ttfReader.ReadGlyph((int) keyValuePair.Key, true);
              if (ttfGlyphInfo.Index > -1)
                otGlyphs[ttfGlyphInfo.Index] = ttfGlyphInfo.CharCode == -1 ? new OtfGlyphInfo((int) keyValuePair.Key, ttfGlyphInfo.Index, (float) ttfGlyphInfo.Width) : new OtfGlyphInfo(ttfGlyphInfo.CharCode, ttfGlyphInfo.Index, (float) ttfGlyphInfo.Width);
            }
          }
          else
          {
            foreach (KeyValuePair<double, string> keyValuePair in characterMapTable)
            {
              char key = keyValuePair.Value.ToCharArray()[0];
              chars[key] = char.MinValue;
            }
          }
          byte[] data = !flag4 ? ttfReader.ReadFontProgram(chars) : ttfReader.ReadOpenTypeFontProgram(otGlyphs);
          missedGlyphs = ttfReader.m_missedGlyphs;
          if (fontList.Object is PdfDictionary)
            this.UpdateFontName(fontList.Object as PdfDictionary, ttfReader);
          f2 = new FontFile2(data);
        }
        else
        {
          PdfDictionary fontDictionary2 = this.GetObject((IPdfPrimitive) fontList);
          if (fontDictionary2 != null && !fontDictionary2.ContainsKey("ToUnicode") && this.fontUsedText.ContainsKey(fontKey))
          {
            if (this.IsCIDToGIDMap(fontDictionary2))
              f2 = this.UpdateFontStream(fontKey, fontList, ttfReader);
            if (f2 == null)
              f2 = ff2;
          }
          else
          {
            isSymbol = true;
            localTableGlyph = this.GetLocalTableGlyph(fontList, ttfReader, out missedGlyphs);
          }
        }
      }
      else
      {
        isSymbol = true;
        localTableGlyph = this.GetLocalTableGlyph(fontList, ttfReader, out missedGlyphs);
      }
      bIsLocaShort = ttfReader.m_bIsLocaShort;
      ttfReader.Close();
    }
    else if (f2.tableList[0] == "cvt " || f2.tableList[0] == "OS/2" || f2.tableList.Contains("EBLC") && f2.tableList.Contains("EBDT"))
    {
      bIsLocaShort = f2.Header.IndexToLocFormat == (short) 0;
      TtfReader ttfReader = new TtfReader(!fontFile2.InternalStream.CanSeek ? new BinaryReader((Stream) new MemoryStream(fontFile2.Data), TtfReader.Encoding) : new BinaryReader((Stream) fontFile2.InternalStream, TtfReader.Encoding));
      ttfReader.CreateInternals();
      localTableGlyph = this.GetLocalTableGlyph(fontList, ttfReader, out missedGlyphs);
      if (localTableGlyph == 0)
      {
        PdfDictionary fontDictionary3 = this.GetObject((IPdfPrimitive) fontList);
        if (fontDictionary3 != null && !fontDictionary3.ContainsKey("ToUnicode") && this.fontUsedText.ContainsKey(fontKey))
        {
          if (this.IsCIDToGIDMap(fontDictionary3))
            f2 = this.UpdateFontStream(fontKey, fontList, ttfReader);
          if (f2 == null)
            f2 = ff2;
        }
        else if (fontDictionary3 != null && !fontDictionary3.ContainsKey("ToUnicode"))
          flag1 = true;
      }
    }
    else
      flag1 = true;
    if (!flag1)
    {
      this.CalculateLocalTableLength(f2, bIsLocaShort, localTableGlyph, out localTableLength1, out isSkip);
      if (!isSkip && localTableLength1 > 0)
      {
        if (missedGlyphs != 0)
        {
          int localTableLength2 = localTableLength1 + missedGlyphs * (bIsLocaShort ? 2 : 1);
          int tableId = f2.getTableID("loca");
          if (f2.getTableBytes(tableId).Length >= localTableLength2)
          {
            MemoryStream fontStream = this.ResetFontTables(f2, isSymbol, localTableLength2, bIsLocaShort) as MemoryStream;
            this.UpdateFontData(fontStream, fontFile2);
            fontStream.Dispose();
          }
          else
            fontFile2.Compress = true;
        }
        else
        {
          MemoryStream fontStream = this.ResetFontTables(f2, isSymbol, localTableLength1, bIsLocaShort) as MemoryStream;
          this.UpdateFontData(fontStream, fontFile2);
          fontStream.Dispose();
        }
      }
      else if (isSkip && flag2)
      {
        MemoryStream fontStream = this.OptimizeTrueTypeFont(f2) as MemoryStream;
        this.UpdateFontData(fontStream, fontFile2);
        fontStream.Dispose();
      }
      else
        fontFile2.Compress = true;
    }
    else
      fontFile2.Compress = true;
  }

  private int GetLocalTableGlyph(
    PdfReferenceHolder fontList,
    TtfReader ttfReader,
    out int missedGlyphs)
  {
    missedGlyphs = 0;
    FontStructure fontStructure = !(fontList.Reference != (PdfReference) null) ? new FontStructure(fontList.Object, (string) null) : new FontStructure(fontList.Object, fontList.Reference.ToString());
    if (fontStructure == null)
      return 0;
    Dictionary<double, string> characterMapTable = fontStructure.CharacterMapTable;
    if (fontStructure.CharacterMapTable.Count <= 0)
      return 0;
    ConcurrentDictionary<char, char> chars = new ConcurrentDictionary<char, char>();
    double[] array = new double[characterMapTable.Count];
    int num = 0;
    foreach (KeyValuePair<double, string> keyValuePair in characterMapTable)
    {
      array[num++] = keyValuePair.Key;
      char key = keyValuePair.Value.ToCharArray()[0];
      chars[key] = char.MinValue;
    }
    System.Array.Sort<double>(array);
    if (ttfReader != null)
    {
      Dictionary<int, int> glyphChars = ttfReader.GetGlyphChars(chars);
      if (glyphChars.Count < chars.Count)
        missedGlyphs = chars.Count - glyphChars.Count;
    }
    return (int) array[array.Length - 1];
  }

  internal byte[] OptimizeType0Font(
    MemoryStream fontData,
    ConcurrentDictionary<char, char> usedChars)
  {
    int localTableGlyph = 0;
    if (usedChars.Count > 0)
    {
      int num = 0;
      double[] array = new double[usedChars.Count];
      foreach (KeyValuePair<char, char> usedChar in usedChars)
        array[num++] = (double) usedChar.Key;
      System.Array.Sort<double>(array);
      localTableGlyph = (int) array[array.Length - 1];
    }
    FontFile2 f2 = new FontFile2(fontData.ToArray());
    int localTableLength = 0;
    bool isSkip = false;
    bool isSymbol = false;
    bool bIsLocaShort = f2.Header.IndexToLocFormat == (short) 0;
    this.CalculateLocalTableLength(f2, bIsLocaShort, localTableGlyph, out localTableLength, out isSkip);
    if (isSkip || localTableLength <= 0)
      return fontData.ToArray();
    MemoryStream memoryStream = this.ResetFontTables(f2, isSymbol, localTableLength, bIsLocaShort) as MemoryStream;
    byte[] array1 = memoryStream.ToArray();
    memoryStream.Dispose();
    return array1;
  }

  private void UpdateFontData(MemoryStream fontStream, PdfStream fontFile2)
  {
    fontStream.Position = 0L;
    fontFile2.InternalStream = fontStream;
    fontFile2.Remove("Length1");
    fontFile2.Items.Add(new PdfName("Length1"), (IPdfPrimitive) new PdfNumber(fontStream.Length));
    fontFile2.Compress = true;
  }

  private Stream ResetFontTables(
    FontFile2 f2,
    bool isSymbol,
    int localTableLength,
    bool bIsLocaShort)
  {
    List<TableEntry> entries = new List<TableEntry>();
    foreach (string table in f2.tableList)
    {
      bool flag = false;
      if (isSymbol && !this.TtTableList.Contains(table))
        flag = true;
      if (!flag)
      {
        string tableName = table;
        int tableId = f2.getTableID(table);
        if (tableId > -1)
        {
          byte[] tableBytes = f2.getTableBytes(tableId);
          if (tableBytes.Length == 0)
            tableBytes = f2.getTableBytes(tableId, true);
          if (table == "OS2")
            tableName = "OS/2";
          switch (tableName)
          {
            case "loca":
              byte[] numArray1 = new byte[localTableLength];
              System.Array.Copy((System.Array) tableBytes, (System.Array) numArray1, localTableLength);
              entries.Add(this.GetTableEntry(tableName, numArray1));
              continue;
            case "hmtx":
              if (bIsLocaShort)
              {
                byte[] numArray2 = new byte[localTableLength * 2 - 4];
                if (numArray2.Length <= tableBytes.Length)
                {
                  System.Array.Copy((System.Array) tableBytes, (System.Array) numArray2, localTableLength * 2 - 4);
                  entries.Add(this.GetTableEntry(tableName, numArray2));
                  continue;
                }
                entries.Add(this.GetTableEntry(tableName, tableBytes));
                continue;
              }
              byte[] numArray3 = new byte[localTableLength - 4];
              if (numArray3.Length <= tableBytes.Length)
              {
                System.Array.Copy((System.Array) tableBytes, (System.Array) numArray3, localTableLength - 4);
                entries.Add(this.GetTableEntry(tableName, numArray3));
                continue;
              }
              entries.Add(this.GetTableEntry(tableName, numArray3));
              continue;
            default:
              entries.Add(this.GetTableEntry(tableName, tableBytes));
              continue;
          }
        }
      }
    }
    return (Stream) new FontDecode().CreateFontStream(entries);
  }

  private void UpdateFontName(PdfDictionary fontDic, TtfReader ttfReader)
  {
    if (!fontDic.ContainsKey("BaseFont"))
      return;
    PdfName pdfName1 = fontDic[new PdfName("BaseFont")] as PdfName;
    if (!(pdfName1 != (PdfName) null) || pdfName1.Value.Contains("+"))
      return;
    StringBuilder stringBuilder = new StringBuilder();
    Random random = new Random();
    for (int index1 = 0; index1 < 6; ++index1)
    {
      int index2 = random.Next("ABCDEFGHIJKLMNOPQRSTUVWXYZ".Length);
      stringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ"[index2]);
    }
    stringBuilder.Append('+');
    stringBuilder.Append(ttfReader.Metrics.PostScriptName);
    PdfName pdfName2 = new PdfName(stringBuilder.ToString());
    fontDic.Items[new PdfName("BaseFont")] = (IPdfPrimitive) pdfName2;
    if (!fontDic.ContainsKey("DescendantFonts"))
      return;
    if (!(fontDic["DescendantFonts"] is PdfArray pdfArray))
    {
      PdfReferenceHolder pdfReferenceHolder = fontDic["DescendantFonts"] as PdfReferenceHolder;
      if (pdfReferenceHolder != (PdfReferenceHolder) null)
        pdfArray = pdfReferenceHolder.Object as PdfArray;
    }
    PdfDictionary pdfDictionary1 = (object) (pdfArray[0] as PdfReferenceHolder) != null ? (pdfArray[0] as PdfReferenceHolder).Object as PdfDictionary : pdfArray[0] as PdfDictionary;
    if (pdfDictionary1 == null)
      return;
    if (pdfDictionary1.ContainsKey("BaseFont"))
      pdfDictionary1[new PdfName("BaseFont")] = (IPdfPrimitive) pdfName2;
    PdfReferenceHolder pdfReferenceHolder1 = pdfDictionary1["FontDescriptor"] as PdfReferenceHolder;
    if (!(pdfReferenceHolder1 != (PdfReferenceHolder) null) || !(pdfReferenceHolder1.Object is PdfDictionary pdfDictionary2) || !pdfDictionary2.ContainsKey("FontName"))
      return;
    pdfDictionary2[new PdfName("FontName")] = (IPdfPrimitive) pdfName2;
  }

  private IPdfPrimitive GetObject(PdfDictionary parent, PdfName key)
  {
    PdfDictionary pdfDictionary = (PdfDictionary) null;
    if (parent[key] is PdfDictionary)
      pdfDictionary = parent[key] as PdfDictionary;
    else if ((object) (parent[key] as PdfReferenceHolder) != null)
      pdfDictionary = (parent[key] as PdfReferenceHolder).Object as PdfDictionary;
    return (IPdfPrimitive) pdfDictionary;
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

  private int CalculateCheckSum(byte[] bytes)
  {
    if (bytes == null)
      throw new ArgumentNullException(nameof (bytes));
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    int num4 = 0;
    int num5 = 0;
    int num6 = 0;
    for (int index1 = bytes.Length / 4; num6 < index1; ++num6)
    {
      int num7 = num5;
      byte[] numArray1 = bytes;
      int index2 = num1;
      int num8 = index2 + 1;
      int num9 = (int) numArray1[index2] & (int) byte.MaxValue;
      num5 = num7 + num9;
      int num10 = num4;
      byte[] numArray2 = bytes;
      int index3 = num8;
      int num11 = index3 + 1;
      int num12 = (int) numArray2[index3] & (int) byte.MaxValue;
      num4 = num10 + num12;
      int num13 = num3;
      byte[] numArray3 = bytes;
      int index4 = num11;
      int num14 = index4 + 1;
      int num15 = (int) numArray3[index4] & (int) byte.MaxValue;
      num3 = num13 + num15;
      int num16 = num2;
      byte[] numArray4 = bytes;
      int index5 = num14;
      num1 = index5 + 1;
      int num17 = (int) numArray4[index5] & (int) byte.MaxValue;
      num2 = num16 + num17;
    }
    return num2 + (num3 << 8) + (num4 << 16 /*0x10*/) + (num5 << 24);
  }

  private TableEntry GetTableEntry(string tableName, byte[] tableData)
  {
    TableEntry tableEntry = new TableEntry()
    {
      id = tableName,
      bytes = tableData
    };
    tableEntry.checkSum = this.CalculateCheckSum(tableEntry.bytes);
    tableEntry.length = tableEntry.bytes.Length;
    return tableEntry;
  }

  private void CalculateLocalTableLength(
    FontFile2 f2,
    bool bIsLocaShort,
    int localTableGlyph,
    out int localTableLength,
    out bool isSkip)
  {
    bool flag1 = false;
    isSkip = false;
    int num1 = 0;
    localTableLength = 0;
    foreach (string table in f2.tableList)
    {
      string str = table;
      if (str == "glyf" || str == "loca")
      {
        int tableId = f2.getTableID(table);
        byte[] tableBytes = f2.getTableBytes(tableId);
        switch (str)
        {
          case "glyf":
            num1 = tableBytes.Length;
            continue;
          case "loca":
            byte[] numArray = !bIsLocaShort ? BitConverter.GetBytes(num1) : BitConverter.GetBytes(num1 / 2);
            System.Array.Reverse((System.Array) numArray);
            bool flag2;
            if (localTableGlyph != 0)
            {
              int num2 = (localTableGlyph + 2) * (bIsLocaShort ? 2 : 4);
              if (num2 < tableBytes.Length && numArray.Length > 3 && (int) numArray[2] == (int) tableBytes[num2 - 2] && (int) numArray[3] == (int) tableBytes[num2 - 1])
              {
                if (num2 == tableBytes.Length)
                  isSkip = true;
                localTableLength = num2;
                return;
              }
              flag2 = true;
            }
            else
              flag2 = true;
            if (flag2)
            {
              for (int index = 0; index < tableBytes.Length; ++index)
              {
                if (bIsLocaShort)
                {
                  if (index + 1 < tableBytes.Length && numArray.Length > 3 && (int) tableBytes[index] == (int) numArray[2] && (int) tableBytes[index + 1] == (int) numArray[3])
                  {
                    localTableLength = index + 2;
                    if (localTableLength == tableBytes.Length)
                      isSkip = true;
                    else if (localTableLength - 4 == 0)
                      isSkip = true;
                    flag1 = true;
                    break;
                  }
                }
                else if (index + 3 < tableBytes.Length && numArray.Length > 3 && (int) tableBytes[index] == (int) numArray[0] && (int) tableBytes[index + 1] == (int) numArray[1] && (int) tableBytes[index + 2] == (int) numArray[2] && (int) tableBytes[index + 3] == (int) numArray[3])
                {
                  localTableLength = index + 4;
                  if (localTableLength == tableBytes.Length)
                    isSkip = true;
                  flag1 = true;
                  break;
                }
              }
            }
            if (flag1)
              return;
            continue;
          default:
            continue;
        }
      }
    }
  }

  private void GetSmaskReference(
    Dictionary<PdfName, IPdfPrimitive> xObjectChilds,
    PdfPageBase lPage)
  {
    List<PdfReference> pdfReferenceList = new List<PdfReference>();
    foreach (KeyValuePair<PdfName, IPdfPrimitive> xObjectChild in xObjectChilds)
    {
      PdfDictionary pdfDictionary = this.GetObject(xObjectChild.Value);
      if (pdfDictionary != null && pdfDictionary.ContainsKey("Subtype") && (pdfDictionary["Subtype"] as PdfName).Value == "Image")
      {
        PdfReference reference = (lPage is PdfPage ? (lPage as PdfPage).CrossTable : (lPage as PdfLoadedPage).CrossTable).GetReference(xObjectChild.Value);
        pdfReferenceList.Add(reference);
        if (pdfDictionary.ContainsKey("SMask"))
        {
          PdfReferenceHolder pdfReferenceHolder = pdfDictionary["SMask"] as PdfReferenceHolder;
          if (pdfReferenceList.Contains(pdfReferenceHolder.Reference))
          {
            this.m_sMaskReference.Add(reference);
            this.m_sMaskReference.Add(pdfReferenceHolder.Reference);
          }
          else if (pdfReferenceHolder != (PdfReferenceHolder) null)
            this.m_sMaskReference.Add(pdfReferenceHolder.Reference);
        }
      }
    }
    pdfReferenceList.Clear();
  }

  private bool CompareStream(MemoryStream stream, out string hashValue)
  {
    hashValue = string.Empty;
    stream.Position = 0L;
    byte[] numArray = new byte[(int) stream.Length];
    stream.Read(numArray, 0, numArray.Length);
    stream.Position = 0L;
    hashValue = this.CreateHashFromStream(numArray);
    return this.m_resourceCollection.ContainsKey(hashValue);
  }

  private string CreateHashFromStream(byte[] streamBytes)
  {
    SHA256Managed shA256Managed = new SHA256Managed();
    byte[] bytes = new byte[32 /*0x20*/];
    IMessageDigest digest = new MessageDigestFinder().GetDigest("SHA256");
    digest.Update(streamBytes, 0, streamBytes.Length);
    digest.DoFinal(bytes, 0);
    StringBuilder stringBuilder = new StringBuilder();
    for (int index = 0; index < bytes.Length; ++index)
      stringBuilder.Append(bytes[index].ToString("x2"));
    return stringBuilder.ToString();
  }

  private bool IsOptimizeUsedCharacters(PdfDictionary fontDictionary, PdfStream fontFile2)
  {
    bool flag = false;
    if (PdfCrossTable.Dereference(fontDictionary["DescendantFonts"]) is PdfArray pdfArray && pdfArray.Count > 0 && PdfCrossTable.Dereference(pdfArray[0]) is PdfDictionary pdfDictionary1 && pdfDictionary1.ContainsKey("FontDescriptor"))
    {
      PdfDictionary pdfDictionary = PdfCrossTable.Dereference(pdfDictionary1["FontDescriptor"]) as PdfDictionary;
      if (pdfDictionary.ContainsKey("Flags") && PdfCrossTable.Dereference(pdfDictionary["Flags"]) is PdfNumber pdfNumber)
      {
        if (pdfNumber.IntValue == 6)
        {
          try
          {
            TtfReader ttfReader = new TtfReader(!fontFile2.InternalStream.CanSeek ? new BinaryReader((Stream) new MemoryStream(fontFile2.Data), TtfReader.Encoding) : new BinaryReader((Stream) fontFile2.InternalStream, TtfReader.Encoding));
            ttfReader.CreateInternals();
            if (ttfReader.TableDirectory.ContainsKey("cmap"))
              flag = true;
          }
          catch
          {
          }
        }
      }
    }
    return flag;
  }

  private bool IsCIDToGIDMap(PdfDictionary fontDictionary)
  {
    bool gidMap = false;
    if (PdfCrossTable.Dereference(fontDictionary["DescendantFonts"]) is PdfArray pdfArray && pdfArray.Count > 0 && PdfCrossTable.Dereference(pdfArray[0]) is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey("FontDescriptor"))
      gidMap = pdfDictionary.ContainsKey("CIDToGIDMap");
    return gidMap;
  }
}
