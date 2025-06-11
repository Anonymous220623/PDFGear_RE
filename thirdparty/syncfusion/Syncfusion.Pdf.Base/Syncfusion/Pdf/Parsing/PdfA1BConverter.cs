// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.PdfA1BConverter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.ColorSpace;
using Syncfusion.Pdf.Compression;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Graphics.Fonts;
using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using Syncfusion.Pdf.Security;
using Syncfusion.Pdf.Xmp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class PdfA1BConverter
{
  private const float MaxColourChannelValue = 255f;
  private const string c_cmapPrefix = "/CIDInit /ProcSet findresource begin\n12 dict begin\nbegincmap\r\n/CIDSystemInfo << /Registry (Adobe)/Ordering (UCS)/Supplement 0>> def\n/CMapName /Adobe-Identity-UCS def\n/CMapType 2 def\n1 begincodespacerange\r\n";
  private const string c_cmapEndCodespaceRange = "endcodespacerange\r\n";
  private const string c_cmapSuffix = "endbfrange\nendcmap\nCMapName currentdict /CMap defineresource pop\nend end\r\n";
  private const string c_cmapBeginRange = "beginbfrange\r\n";
  private const string c_cmapEndRange = "endbfrange\r\n";
  private const int c_cmapNextRangeValue = 100;
  private const string c_rdfPdfa = "http://www.aiim.org/pdfa/ns/id/";
  private const string c_rdfUri = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
  private const string c_pdfschema = "http://ns.adobe.com/pdf/1.3/";
  private const string c_dublinSchema = "http://purl.org/dc/elements/1.1/";
  private const string c_pdfaExtension = "http://www.aiim.org/pdfa/ns/extension/";
  private PdfLoadedDocument ldoc;
  private bool isPdfPage;
  private Colorspace colorspace;
  private string appliedSCColorSpace;
  private string appliedSCNColorSpace;
  private float nonStrokingOpacity;
  private float strokingOpacity;
  private bool hasNonStroking;
  private bool isPdfFontFamily = true;
  private bool hasStroking;
  private Dictionary<string, PdfReferenceHolder> formObjects;
  private List<string> m_usedChars;
  private Dictionary<string, PdfTrueTypeFont> m_replaceFonts;
  private Dictionary<string, PdfDictionary> m_oldFonts;
  private Dictionary<string, PdfDictionary> m_replaceFontDictionary;
  private List<string> TtTableList = new List<string>();
  internal PdfConformanceLevel PdfALevel;
  private List<PdfRecordCollection> recordCollectionList = new List<PdfRecordCollection>();
  private Dictionary<PdfResources, PdfRecordCollection> recordList = new Dictionary<PdfResources, PdfRecordCollection>();
  private PdfAConversionProgressEventArgs args;

  private List<string> UsedFonts
  {
    get
    {
      if (this.m_usedChars == null)
        this.m_usedChars = new List<string>();
      return this.m_usedChars;
    }
  }

  private Dictionary<string, PdfTrueTypeFont> ReplaceFonts
  {
    get
    {
      if (this.m_replaceFonts == null)
        this.m_replaceFonts = new Dictionary<string, PdfTrueTypeFont>();
      return this.m_replaceFonts;
    }
  }

  private Dictionary<string, PdfDictionary> ReplaceFontDictionary
  {
    get
    {
      if (this.m_replaceFontDictionary == null)
        this.m_replaceFontDictionary = new Dictionary<string, PdfDictionary>();
      return this.m_replaceFontDictionary;
    }
  }

  private Dictionary<string, PdfDictionary> OldFonts
  {
    get
    {
      if (this.m_oldFonts == null)
        this.m_oldFonts = new Dictionary<string, PdfDictionary>();
      return this.m_oldFonts;
    }
  }

  internal PdfLoadedDocument Convert(PdfLoadedDocument document)
  {
    this.ldoc = document;
    PdfDocument.EnableCache = false;
    document.FileStructure.CrossReferenceType = PdfCrossReferenceType.CrossReferenceTable;
    document.FileStructure.IncrementalUpdate = false;
    if (this.PdfALevel != PdfConformanceLevel.Pdf_A3B)
      this.AttachmentsConsideration(document);
    else if (this.PdfALevel == PdfConformanceLevel.Pdf_A3B)
    {
      PdfName key = new PdfName("AFRelationship");
      PdfArray pdfArray = new PdfArray();
      if (document.Attachments != null)
      {
        for (int index = 0; index < document.Attachments.Count; ++index)
        {
          if (!document.Attachments[index].Dictionary.Items.ContainsKey(key))
            document.Attachments[index].Dictionary.Items[key] = (IPdfPrimitive) new PdfName((Enum) PdfAttachmentRelationship.Alternative);
          pdfArray.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) document.Attachments[index].Dictionary));
        }
        if (!document.Catalog.ContainsKey("AF"))
          document.Catalog.Items.Add(new PdfName("AF"), (IPdfPrimitive) pdfArray);
      }
    }
    if (document.RaiseTrackPdfAConversionProgress)
    {
      this.args = new PdfAConversionProgressEventArgs();
      this.args.m_progressValue = 0.0f;
      document.OnPdfAConversionTrackProgress(this.args);
      this.ParseForm(document);
      this.args.m_progressValue = 10f;
      document.OnPdfAConversionTrackProgress(this.args);
      this.RetrieveFontData(document);
      if ((double) this.args.m_progressValue != 35.0)
      {
        this.args.m_progressValue = 35f;
        document.OnPdfAConversionTrackProgress(this.args);
      }
      this.ContentStreamParsing(document);
      if ((double) this.args.m_progressValue != 85.0)
      {
        this.args.m_progressValue = 85f;
        document.OnPdfAConversionTrackProgress(this.args);
      }
      this.AddDocumentColorProfile(document);
      this.args.m_progressValue = 90f;
      document.OnPdfAConversionTrackProgress(this.args);
      this.AddMetaDataInfo(document);
      this.args.m_progressValue = 95f;
      document.OnPdfAConversionTrackProgress(this.args);
      this.AddTrailerID(document);
      this.args.m_progressValue = 100f;
      document.OnPdfAConversionTrackProgress(this.args);
    }
    else
    {
      this.ParseForm(document);
      this.RetrieveFontData(document);
      this.ContentStreamParsing(document);
      this.AddDocumentColorProfile(document);
      this.AddMetaDataInfo(document);
      this.AddTrailerID(document);
    }
    this.m_usedChars = (List<string>) null;
    this.m_replaceFonts = (Dictionary<string, PdfTrueTypeFont>) null;
    this.m_oldFonts = (Dictionary<string, PdfDictionary>) null;
    this.m_replaceFontDictionary = (Dictionary<string, PdfDictionary>) null;
    return document;
  }

  internal PdfArray GetArrayFromReferenceHolder(IPdfPrimitive primitive)
  {
    if ((object) (primitive as PdfReferenceHolder) == null)
      return primitive as PdfArray;
    return (object) ((primitive as PdfReferenceHolder).Object as PdfReferenceHolder) != null ? this.GetArrayFromReferenceHolder((IPdfPrimitive) ((primitive as PdfReferenceHolder).Object as PdfReferenceHolder)) : (primitive as PdfReferenceHolder).Object as PdfArray;
  }

  internal PdfDictionary GetDictionaryFromRefernceHolder(IPdfPrimitive primitive)
  {
    if ((object) (primitive as PdfReferenceHolder) == null)
      return primitive as PdfDictionary;
    return (object) ((primitive as PdfReferenceHolder).Object as PdfReferenceHolder) != null ? this.GetDictionaryFromRefernceHolder((IPdfPrimitive) ((primitive as PdfReferenceHolder).Object as PdfReferenceHolder)) : (primitive as PdfReferenceHolder).Object as PdfDictionary;
  }

  internal PdfResources GetResourceFromRefernceHolder(IPdfPrimitive primitive)
  {
    if ((object) (primitive as PdfReferenceHolder) == null)
      return primitive as PdfResources;
    return (object) ((primitive as PdfReferenceHolder).Object as PdfReferenceHolder) != null ? this.GetResourceFromRefernceHolder((IPdfPrimitive) ((primitive as PdfReferenceHolder).Object as PdfResources)) : (primitive as PdfReferenceHolder).Object as PdfResources;
  }

  internal PdfStream GetStreamFromRefernceHolder(IPdfPrimitive primitive)
  {
    if ((object) (primitive as PdfReferenceHolder) == null)
      return primitive as PdfStream;
    return (object) ((primitive as PdfReferenceHolder).Object as PdfReferenceHolder) != null ? this.GetStreamFromRefernceHolder((IPdfPrimitive) ((primitive as PdfReferenceHolder).Object as PdfReferenceHolder)) : (primitive as PdfReferenceHolder).Object as PdfStream;
  }

  internal PdfBoolean GetPdfBooleanFromReferenceHolder(IPdfPrimitive primitive)
  {
    if ((object) (primitive as PdfReferenceHolder) == null)
      return primitive as PdfBoolean;
    return (object) ((primitive as PdfReferenceHolder).Object as PdfReferenceHolder) != null ? this.GetPdfBooleanFromReferenceHolder((IPdfPrimitive) ((primitive as PdfReferenceHolder).Object as PdfReferenceHolder)) : (primitive as PdfReferenceHolder).Object as PdfBoolean;
  }

  private void ParseFormStreamFromXObject(PdfDictionary xObjectDictionary, string key)
  {
    if (xObjectDictionary == null || !(xObjectDictionary is PdfStream) || !xObjectDictionary.ContainsKey("Type") || !((xObjectDictionary["Type"] as PdfName).Value == "XObject") || !xObjectDictionary.ContainsKey("Subtype") || !((xObjectDictionary["Subtype"] as PdfName).Value == "Form"))
      return;
    this.ParseFormStream(xObjectDictionary as PdfStream, key, this.GetDictionaryFromRefernceHolder(xObjectDictionary["Resources"]), true);
  }

  private void ParseFormStream(
    PdfStream internalStream,
    string xObjectKey,
    PdfDictionary resources,
    bool enableRecrusiveCall)
  {
    if (internalStream.Data.Length > 0)
    {
      PdfRecordCollection recordCollection = new XObjectElement((PdfDictionary) internalStream, xObjectKey).Render((PdfPageResources) null, (Stack<GraphicsState>) null);
      PdfStream pdfStream = new PdfStream();
      if (recordCollection != null)
      {
        int count = recordCollection.RecordCollection.Count;
        bool flag1 = false;
        for (int index1 = 0; index1 < count; ++index1)
        {
          PdfRecord record = recordCollection.RecordCollection[index1];
          if (record.Operands != null && record.Operands.Length >= 1)
          {
            if (record.OperatorName == "CS" || record.OperatorName == "cs")
            {
              StringBuilder stringBuilder = new StringBuilder();
              switch (record.Operands[0].Replace("/", ""))
              {
                case "DeviceCMYK":
                  stringBuilder.Append("/DeviceRGB");
                  stringBuilder.Append(" ");
                  byte[] bytes1 = Encoding.Default.GetBytes(stringBuilder.ToString());
                  pdfStream.Write(bytes1);
                  flag1 = false;
                  break;
                case "Pattern":
                  stringBuilder.Append(record.Operands[0]);
                  stringBuilder.Append(" ");
                  byte[] bytes2 = Encoding.Default.GetBytes(stringBuilder.ToString());
                  pdfStream.Write(bytes2);
                  flag1 = true;
                  break;
                default:
                  stringBuilder.Append(record.Operands[0]);
                  stringBuilder.Append(" ");
                  byte[] bytes3 = Encoding.Default.GetBytes(stringBuilder.ToString());
                  pdfStream.Write(bytes3);
                  flag1 = false;
                  break;
              }
            }
            else if (flag1 && (record.OperatorName == "SCN" || record.OperatorName == "scn"))
            {
              StringBuilder stringBuilder = new StringBuilder();
              PdfDictionary fromRefernceHolder = this.GetDictionaryFromRefernceHolder(resources["Pattern"]);
              string key = record.Operands[0].ToString((IFormatProvider) CultureInfo.InvariantCulture).Replace("/", "");
              if (fromRefernceHolder != null && fromRefernceHolder.ContainsKey(key))
                this.RemoveSMaskFromPattern(this.GetDictionaryFromRefernceHolder(fromRefernceHolder[key]));
              stringBuilder.Append(record.Operands[0]);
              stringBuilder.Append(" ");
              byte[] bytes = Encoding.Default.GetBytes(stringBuilder.ToString());
              pdfStream.Write(bytes);
            }
            else if (record.OperatorName == "gs" && resources != null)
            {
              StringBuilder stringBuilder = new StringBuilder();
              PdfDictionary fromRefernceHolder1 = this.GetDictionaryFromRefernceHolder(resources["ExtGState"]);
              string str = record.Operands[0].ToString((IFormatProvider) CultureInfo.InvariantCulture).Replace("/", "");
              if (fromRefernceHolder1 != null && fromRefernceHolder1.Items.ContainsKey(new PdfName(str)))
              {
                PdfDictionary fromRefernceHolder2 = this.GetDictionaryFromRefernceHolder(this.GetDictionaryFromRefernceHolder((IPdfPrimitive) fromRefernceHolder1).Items[new PdfName(str)]);
                if (fromRefernceHolder2.ContainsKey("SMask"))
                {
                  PdfDictionary fromRefernceHolder3 = this.GetDictionaryFromRefernceHolder(fromRefernceHolder2["SMask"]);
                  if (fromRefernceHolder3 == null || !fromRefernceHolder3.ContainsKey("S") || !((fromRefernceHolder3["S"] as PdfName).Value == "Luminosity"))
                  {
                    fromRefernceHolder2.Remove("SMask");
                    fromRefernceHolder2.Modify();
                  }
                }
                if (fromRefernceHolder2.ContainsKey("BM"))
                {
                  bool flag2 = false;
                  if ((object) (fromRefernceHolder2["BM"] as PdfName) != null)
                  {
                    if ((fromRefernceHolder2["BM"] as PdfName).Value == "Normal" || (fromRefernceHolder2["BM"] as PdfName).Value == "Compatible")
                    {
                      flag2 = true;
                    }
                    else
                    {
                      fromRefernceHolder2["BM"] = (IPdfPrimitive) new PdfName("Normal");
                      fromRefernceHolder2.Modify();
                      flag2 = true;
                    }
                  }
                  else if (fromRefernceHolder2["BM"] is PdfArray)
                  {
                    List<IPdfPrimitive> pdfPrimitiveList = new List<IPdfPrimitive>();
                    foreach (IPdfPrimitive pdfPrimitive in fromRefernceHolder2["BM"] as PdfArray)
                      pdfPrimitiveList.Add(pdfPrimitive);
                    for (int index2 = 0; index2 < pdfPrimitiveList.Count; ++index2)
                    {
                      if ((object) (pdfPrimitiveList[index2] as PdfName) != null && ((pdfPrimitiveList[index2] as PdfName).Value == "Normal" || (pdfPrimitiveList[index2] as PdfName).Value == "Compatible"))
                        flag2 = true;
                      else if ((object) (pdfPrimitiveList[index2] as PdfName) != null)
                      {
                        (fromRefernceHolder2["BM"] as PdfArray).Elements[index2] = (IPdfPrimitive) new PdfName("Normal");
                        (fromRefernceHolder2["BM"] as PdfArray).MarkChanged();
                      }
                    }
                  }
                  if (flag2)
                  {
                    if (fromRefernceHolder2.Items.ContainsKey(new PdfName("ca")))
                      fromRefernceHolder2.Items[new PdfName("ca")] = (IPdfPrimitive) new PdfNumber(1);
                    if (fromRefernceHolder2.Items.ContainsKey(new PdfName("CA")))
                      fromRefernceHolder2.Items[new PdfName("CA")] = (IPdfPrimitive) new PdfNumber(1);
                  }
                }
                else
                {
                  if (fromRefernceHolder2.Items.ContainsKey(new PdfName("ca")))
                    fromRefernceHolder2.Items[new PdfName("ca")] = (IPdfPrimitive) new PdfNumber(1);
                  if (fromRefernceHolder2.Items.ContainsKey(new PdfName("CA")))
                    fromRefernceHolder2.Items[new PdfName("CA")] = (IPdfPrimitive) new PdfNumber(1);
                }
              }
              stringBuilder.Append(record.Operands[0]);
              stringBuilder.Append(" ");
              string s = stringBuilder.ToString();
              pdfStream.Write(Encoding.Default.GetBytes(s));
            }
            else if (record.OperatorName == "Do")
            {
              StringBuilder stringBuilder = new StringBuilder();
              if (internalStream.ContainsKey("Resources"))
              {
                IPdfPrimitive primitive = this.GetDictionaryFromRefernceHolder(internalStream["Resources"])["XObject"];
                string str = record.Operands[0].Replace("/", "");
                if (primitive != null && this.GetDictionaryFromRefernceHolder(primitive).Items.ContainsKey(new PdfName(str)))
                  this.RemoveSMask(this.GetDictionaryFromRefernceHolder(primitive)[new PdfName(str)] as PdfReferenceHolder, internalStream, record.Operands[0]);
              }
              stringBuilder.Append(record.Operands[0]);
              stringBuilder.Append(" ");
              string s = stringBuilder.ToString();
              pdfStream.Write(Encoding.Default.GetBytes(s));
            }
            else if (record.OperatorName == "K" || record.OperatorName == "k")
            {
              StringBuilder stringBuilder = new StringBuilder();
              Color color = new DeviceCMYK().GetColor(record.Operands);
              float[] numArray = new float[3]
              {
                (float) color.R,
                (float) color.G,
                (float) color.B
              };
              numArray[0] = numArray[0] / (float) byte.MaxValue;
              numArray[1] = numArray[1] / (float) byte.MaxValue;
              numArray[2] = numArray[2] / (float) byte.MaxValue;
              stringBuilder.Append(Math.Round((double) numArray[0], 2).ToString((IFormatProvider) CultureInfo.InvariantCulture));
              stringBuilder.Append(" ");
              stringBuilder.Append(Math.Round((double) numArray[1], 2).ToString((IFormatProvider) CultureInfo.InvariantCulture));
              stringBuilder.Append(" ");
              stringBuilder.Append(Math.Round((double) numArray[2], 2).ToString((IFormatProvider) CultureInfo.InvariantCulture));
              stringBuilder.Append(" ");
              byte[] bytes = Encoding.Default.GetBytes(stringBuilder.ToString());
              pdfStream.Write(bytes);
            }
            else if (record.OperatorName == "Tj" || record.OperatorName == "TJ")
            {
              for (int index3 = 0; index3 < record.Operands.Length; ++index3)
              {
                PdfString pdfString = new PdfString(this.TrimOperand(record.Operands[index3]));
                pdfStream.Write(pdfString.Bytes);
                pdfStream.Write(" ");
              }
            }
            else
            {
              for (int index4 = 0; index4 < record.Operands.Length; ++index4)
              {
                string operand = record.Operands[index4];
                if (record.OperatorName != "Tj" && record.OperatorName != "'" && record.OperatorName != "\"" && record.OperatorName != "TJ")
                  operand = this.TrimOperand(operand);
                PdfString pdfString = new PdfString(operand);
                pdfStream.Write(pdfString.Bytes);
                if (record.OperatorName != "'" && record.OperatorName != "\"")
                  pdfStream.Write(" ");
              }
            }
          }
          if (record.OperatorName == "K")
            pdfStream.Write("RG");
          else if (record.OperatorName == "k")
          {
            pdfStream.Write("rg");
          }
          else
          {
            if (record.OperatorName == "Q" || record.OperatorName == "E")
              flag1 = false;
            pdfStream.Write(record.OperatorName);
          }
          if (index1 + 1 < count && (record.OperatorName == "W" || record.OperatorName == "W*") && recordCollection.RecordCollection[index1 + 1].OperatorName == "n")
            pdfStream.Write(" ");
          else if (record.OperatorName == "w")
            pdfStream.Write(" ");
          else
            pdfStream.Write("\r\n");
        }
        if (count == 0)
        {
          pdfStream.Write("q");
          pdfStream.Write("\r\n");
          pdfStream.Write("Q");
          pdfStream.Write("\r\n");
        }
        internalStream.Items.Remove(new PdfName("Length"));
        internalStream.Data = pdfStream.Data;
        internalStream.isSkip = false;
        internalStream.Modify();
        pdfStream.Clear();
        pdfStream.InternalStream.Dispose();
        pdfStream.InternalStream.Close();
        pdfStream.InternalStream = (MemoryStream) null;
      }
    }
    else
    {
      internalStream.Data = new byte[1]
      {
        (byte) 32 /*0x20*/
      };
      internalStream.isSkip = false;
      internalStream.Modify();
    }
    if (!enableRecrusiveCall || resources == null || !resources.ContainsKey("XObject"))
      return;
    PdfDictionary fromRefernceHolder4 = this.GetDictionaryFromRefernceHolder(resources["XObject"]);
    if (fromRefernceHolder4 == null || fromRefernceHolder4.Items.Count <= 0)
      return;
    List<PdfName> pdfNameList = new List<PdfName>((IEnumerable<PdfName>) fromRefernceHolder4.Keys);
    bool flag = true;
    foreach (PdfName pdfName in pdfNameList)
    {
      if (pdfName.Value == xObjectKey)
      {
        flag = false;
        break;
      }
    }
    if (!flag)
      return;
    foreach (PdfName key in pdfNameList)
      this.ParseFormStreamFromXObject(this.GetDictionaryFromRefernceHolder(fromRefernceHolder4[key]), key.Value);
  }

  private void RemoveSMaskFromPattern(PdfDictionary pattern)
  {
    if (pattern == null || !pattern.ContainsKey("Resources"))
      return;
    PdfDictionary fromRefernceHolder1 = this.GetDictionaryFromRefernceHolder(pattern["Resources"]);
    if (fromRefernceHolder1 == null || !fromRefernceHolder1.ContainsKey("XObject"))
      return;
    PdfDictionary fromRefernceHolder2 = this.GetDictionaryFromRefernceHolder(fromRefernceHolder1["XObject"]);
    if (fromRefernceHolder2 == null || fromRefernceHolder2.Count <= 0)
      return;
    foreach (IPdfPrimitive primitive in fromRefernceHolder2.Values)
    {
      PdfDictionary fromRefernceHolder3 = this.GetDictionaryFromRefernceHolder(primitive);
      if (fromRefernceHolder3 != null)
      {
        if (fromRefernceHolder3.ContainsKey("Subtype") && (fromRefernceHolder3["Subtype"] as PdfName).Value == "Image")
        {
          if (fromRefernceHolder3.ContainsKey("SMask"))
          {
            fromRefernceHolder3.Remove("SMask");
            fromRefernceHolder3.Modify();
          }
          if (fromRefernceHolder3.ContainsKey("Interpolate"))
          {
            this.GetPdfBooleanFromReferenceHolder(fromRefernceHolder3["Interpolate"]).Value = false;
            fromRefernceHolder3.Modify();
          }
        }
        else if (fromRefernceHolder3.ContainsKey("Resources"))
          this.RemoveSMaskFromPattern(fromRefernceHolder3);
      }
    }
  }

  private string TrimOperand(string operand)
  {
    if (operand == ".00" || operand == "-.00")
      operand = "0";
    if (operand.Contains(".00"))
    {
      string[] strArray = operand.Split('.');
      if (strArray.Length == 2 && strArray[1] == "00")
        operand = strArray[0];
    }
    return operand;
  }

  private void RemoveSMask(PdfReferenceHolder imageObject, PdfStream internalStream, string key)
  {
    while (imageObject != (PdfReferenceHolder) null && (object) (imageObject.Object as PdfReferenceHolder) != null)
      imageObject = imageObject.Object as PdfReferenceHolder;
    if (!(imageObject.Object is PdfStream))
      return;
    PdfStream pdfStream = imageObject.Object as PdfStream;
    if (!((pdfStream["Subtype"] as PdfName).Value == "Image"))
      return;
    if (this.PdfALevel == PdfConformanceLevel.Pdf_A1B && pdfStream.ContainsKey("SMask"))
    {
      pdfStream.Remove("SMask");
      pdfStream.Modify();
    }
    if (pdfStream.ContainsKey("Mask"))
    {
      PdfDictionary fromRefernceHolder = this.GetDictionaryFromRefernceHolder(pdfStream["Mask"]);
      if (fromRefernceHolder != null && fromRefernceHolder.ContainsKey("Interpolate"))
      {
        this.GetPdfBooleanFromReferenceHolder(fromRefernceHolder["Interpolate"]).Value = false;
        fromRefernceHolder.Modify();
      }
    }
    if (!pdfStream.ContainsKey("Interpolate"))
      return;
    this.GetPdfBooleanFromReferenceHolder(pdfStream["Interpolate"]).Value = false;
    pdfStream.Modify();
  }

  internal void GetFontInternals(
    PdfDictionary fontDictionary,
    out PdfDictionary fontDescriptor,
    out PdfDictionary descendantFont)
  {
    fontDescriptor = (PdfDictionary) null;
    descendantFont = (PdfDictionary) null;
    if (fontDictionary == null)
      return;
    fontDescriptor = this.GetDictionaryFromRefernceHolder(fontDictionary["FontDescriptor"]);
    if (!fontDictionary.ContainsKey("DescendantFonts"))
      return;
    if (fontDictionary["DescendantFonts"] is PdfArray)
    {
      descendantFont = this.GetDictionaryFromRefernceHolder((fontDictionary["DescendantFonts"] as PdfArray).Elements[0]);
      if (!descendantFont.ContainsKey("FontDescriptor"))
        return;
      fontDescriptor = this.GetDictionaryFromRefernceHolder(descendantFont["FontDescriptor"]);
    }
    else
    {
      if ((object) (fontDictionary["DescendantFonts"] as PdfReferenceHolder) == null)
        return;
      PdfArray fromReferenceHolder = this.GetArrayFromReferenceHolder(fontDictionary["DescendantFonts"]);
      descendantFont = this.GetDictionaryFromRefernceHolder(fromReferenceHolder.Elements[0]);
      if (!descendantFont.ContainsKey("FontDescriptor"))
        return;
      fontDescriptor = this.GetDictionaryFromRefernceHolder(descendantFont["FontDescriptor"]);
    }
  }

  private void RetrieveFontData(PdfLoadedDocument document)
  {
    int num1 = 0;
    foreach (PdfPageBase page in document.Pages)
    {
      if (page is PdfPage)
        this.isPdfPage = true;
      MemoryStream memoryStream = new MemoryStream();
      page.Layers.CombineContent((Stream) memoryStream);
      PdfRecordCollection recordCollection = new ContentParser(memoryStream.ToArray())
      {
        ConformanceEnabled = true
      }.ReadContent();
      this.recordCollectionList.Add(recordCollection);
      PdfStream pdfStream = new PdfStream();
      PdfResources resources = page.GetResources();
      if (resources != null && !this.recordList.ContainsKey(resources))
        this.recordList.Add(resources, recordCollection);
      PdfDictionary fontResources = (PdfDictionary) null;
      if (resources != null && resources.ContainsKey("Font"))
        fontResources = this.GetDictionaryFromRefernceHolder(resources["Font"]);
      this.RetrieveFontData(recordCollection, fontResources);
      ++num1;
      if (document.RaiseTrackPdfAConversionProgress)
      {
        float num2 = 25f / (float) document.Pages.Count + this.args.m_progressValue;
        if ((int) this.args.m_progressValue != (int) num2)
        {
          this.args.m_progressValue = (float) (int) num2;
          document.OnPdfAConversionTrackProgress(this.args);
          this.args.m_progressValue = num2;
        }
        else
          this.args.m_progressValue = num2;
      }
    }
  }

  private void RetrieveFontData(PdfRecordCollection recordCollection, PdfDictionary fontResources)
  {
    if (fontResources == null)
      return;
    int count = recordCollection.RecordCollection.Count;
    for (int index = 0; index < count; ++index)
    {
      PdfRecord record = recordCollection.RecordCollection[index];
      if (record.Operands != null && record.Operands.Length >= 1 && record.OperatorName == "Tf")
      {
        string key = record.Operands[0].Replace("/", "");
        string operand = record.Operands[1];
        if (fontResources[key] != null && !this.UsedFonts.Contains(key))
        {
          PdfDictionary fromRefernceHolder = this.GetDictionaryFromRefernceHolder(fontResources[key]);
          PdfDictionary fontDescriptor = (PdfDictionary) null;
          PdfDictionary descendantFont = (PdfDictionary) null;
          this.GetFontInternals(fromRefernceHolder, out fontDescriptor, out descendantFont);
          if (fromRefernceHolder.ContainsKey("Subtype") && (fromRefernceHolder["Subtype"] as PdfName).Value == "Type1" && fromRefernceHolder.ContainsKey("BaseFont"))
          {
            switch (this.GetFontFamily((fromRefernceHolder["BaseFont"] as PdfName).Value))
            {
              case PdfFontFamily.Helvetica:
              case PdfFontFamily.TimesRoman:
                if (fontDescriptor == null || !fontDescriptor.ContainsKey("FontFile") && !fontDescriptor.ContainsKey("FontFile2") && !fontDescriptor.ContainsKey("FontFile3"))
                {
                  FontStructure fontStructure = new FontStructure((IPdfPrimitive) fromRefernceHolder);
                  if ((fontStructure.CharacterMapTable == null || fontStructure.CharacterMapTable.Count <= 0) && (fontStructure.DifferencesDictionary == null || fontStructure.DifferencesDictionary.Count <= 0) && (fontDescriptor == null || fontDescriptor != null && fontDescriptor.ContainsKey("Flags") && (fontDescriptor["Flags"] as PdfNumber).IntValue != 4 && (fontDescriptor["Flags"] as PdfNumber).IntValue != 32 /*0x20*/))
                  {
                    this.UsedFonts.Add(key);
                    break;
                  }
                  break;
                }
                break;
            }
          }
          else if (fontDescriptor == null || !fontDescriptor.ContainsKey("FontFile") && !fontDescriptor.ContainsKey("FontFile2") && !fontDescriptor.ContainsKey("FontFile3"))
          {
            FontStructure fontStructure = new FontStructure((IPdfPrimitive) fromRefernceHolder);
            if ((fontStructure.CharacterMapTable == null || fontStructure.CharacterMapTable.Count <= 0) && (fontStructure.DifferencesDictionary == null || fontStructure.DifferencesDictionary.Count <= 0) && (fontDescriptor == null || fontDescriptor != null && fontDescriptor.ContainsKey("Flags") && (fontDescriptor["Flags"] as PdfNumber).IntValue != 4 && (fontDescriptor["Flags"] as PdfNumber).IntValue != 32 /*0x20*/))
              this.UsedFonts.Add(key);
          }
          descendantFont = (PdfDictionary) null;
        }
      }
    }
  }

  private MemoryStream ParseContentStream(
    MemoryStream contentStream,
    PdfPageBase lPage,
    int pageNo)
  {
    List<string> stringList = new List<string>();
    new ContentParser(contentStream.ToArray()).ConformanceEnabled = true;
    PdfStream pdfStream = new PdfStream();
    PdfResources resources = lPage.GetResources();
    PdfDictionary pdfDictionary1 = (PdfDictionary) null;
    if (resources != null && resources.ContainsKey("Font"))
      pdfDictionary1 = this.GetDictionaryFromRefernceHolder(resources["Font"]);
    string key1 = (string) null;
    float num1 = 12f;
    PdfDictionary fontDictionary = (PdfDictionary) null;
    PdfRecordCollection recordCollection = this.recordCollectionList[pageNo];
    int count1 = recordCollection.RecordCollection.Count;
    bool flag1 = false;
    bool flag2 = false;
    for (int index1 = 0; index1 < count1; ++index1)
    {
      PdfRecord record = recordCollection.RecordCollection[index1];
      double num2;
      if (record.Operands != null && record.Operands.Length >= 1)
      {
        if (record.OperatorName == "gs")
        {
          flag1 = false;
          StringBuilder stringBuilder = new StringBuilder();
          IPdfPrimitive primitive = resources["ExtGState"];
          string str = record.Operands[0].ToString((IFormatProvider) CultureInfo.InvariantCulture).Replace("/", "");
          if (primitive != null && this.GetDictionaryFromRefernceHolder(primitive).Items.ContainsKey(new PdfName(str)))
          {
            PdfDictionary fromRefernceHolder = this.GetDictionaryFromRefernceHolder(this.GetDictionaryFromRefernceHolder(primitive).Items[new PdfName(str)]);
            bool flag3 = false;
            if (fromRefernceHolder.ContainsKey("BM"))
            {
              if ((object) (fromRefernceHolder["BM"] as PdfName) != null)
              {
                if ((fromRefernceHolder["BM"] as PdfName).Value == "Normal" || (fromRefernceHolder["BM"] as PdfName).Value == "Compatible")
                {
                  flag3 = true;
                }
                else
                {
                  fromRefernceHolder["BM"] = (IPdfPrimitive) new PdfName("Normal");
                  fromRefernceHolder.Modify();
                  flag3 = true;
                }
              }
              else if (fromRefernceHolder["BM"] is PdfArray)
              {
                List<IPdfPrimitive> pdfPrimitiveList = new List<IPdfPrimitive>();
                foreach (IPdfPrimitive pdfPrimitive in fromRefernceHolder["BM"] as PdfArray)
                  pdfPrimitiveList.Add(pdfPrimitive);
                for (int index2 = 0; index2 < pdfPrimitiveList.Count; ++index2)
                {
                  if ((object) (pdfPrimitiveList[index2] as PdfName) != null && ((pdfPrimitiveList[index2] as PdfName).Value == "Normal" || (pdfPrimitiveList[index2] as PdfName).Value == "Compatible"))
                    flag3 = true;
                  else if ((object) (pdfPrimitiveList[index2] as PdfName) != null)
                  {
                    (fromRefernceHolder["BM"] as PdfArray).Elements[index2] = (IPdfPrimitive) new PdfName("Normal");
                    (fromRefernceHolder["BM"] as PdfArray).MarkChanged();
                  }
                }
              }
            }
            if (flag3)
            {
              if (fromRefernceHolder.ContainsKey("ca"))
              {
                this.nonStrokingOpacity = (fromRefernceHolder["ca"] as PdfNumber).FloatValue;
                this.hasNonStroking = true;
              }
              else
                this.hasNonStroking = false;
              if (fromRefernceHolder.ContainsKey("CA"))
              {
                this.strokingOpacity = (fromRefernceHolder["CA"] as PdfNumber).FloatValue;
                this.hasStroking = true;
              }
              else
                this.hasStroking = false;
              if (!stringList.Contains(str))
                stringList.Add(str);
              flag1 = true;
            }
            else
            {
              if (fromRefernceHolder.ContainsKey("ca"))
              {
                fromRefernceHolder["ca"] = (IPdfPrimitive) new PdfNumber(1);
                fromRefernceHolder.Modify();
              }
              if (fromRefernceHolder.ContainsKey("CA"))
              {
                fromRefernceHolder["CA"] = (IPdfPrimitive) new PdfNumber(1);
                fromRefernceHolder.Modify();
              }
            }
            if (fromRefernceHolder.ContainsKey("TR"))
              fromRefernceHolder.Remove("TR");
            if (fromRefernceHolder.ContainsKey("TR2"))
              fromRefernceHolder.Remove("TR2");
          }
          if (!flag1 && primitive != null)
          {
            stringBuilder.Append(record.Operands[0]);
            stringBuilder.Append(" ");
            byte[] bytes = Encoding.Default.GetBytes(stringBuilder.ToString());
            pdfStream.Write(bytes);
          }
        }
        else if (record.OperatorName == "Tf" && pdfDictionary1 != null)
        {
          int index3;
          for (index3 = 0; index3 < record.Operands.Length; ++index3)
          {
            if (record.Operands[index3].Contains("/"))
            {
              key1 = record.Operands[index3].Replace("/", "");
              break;
            }
          }
          num1 = float.Parse(record.Operands[index3 + 1], (IFormatProvider) CultureInfo.InvariantCulture);
          if (pdfDictionary1.ContainsKey(key1))
            fontDictionary = this.GetDictionaryFromRefernceHolder(pdfDictionary1[key1]);
          for (int index4 = 0; index4 < record.Operands.Length; ++index4)
          {
            PdfString pdfString = new PdfString(this.TrimOperand(record.Operands[index4]));
            pdfStream.Write(pdfString.Bytes);
            pdfStream.Write(" ");
          }
        }
        else if (record.OperatorName == "Do")
        {
          StringBuilder stringBuilder = new StringBuilder();
          IPdfPrimitive primitive = resources["XObject"];
          string str = record.Operands[0].Replace("/", "");
          if (primitive != null && this.GetDictionaryFromRefernceHolder(primitive).Items.ContainsKey(new PdfName(str)))
            this.RemoveSMask(this.GetDictionaryFromRefernceHolder(primitive)[new PdfName(str)] as PdfReferenceHolder, resources, record.Operands[0]);
          stringBuilder.Append(record.Operands[0]);
          stringBuilder.Append(" ");
          byte[] bytes = Encoding.Default.GetBytes(stringBuilder.ToString());
          pdfStream.Write(bytes);
        }
        else if (record.OperatorName == "CS" || record.OperatorName == "cs")
        {
          StringBuilder stringBuilder = new StringBuilder();
          string key2 = record.Operands[0].Replace("/", "");
          Colorspace colorspace = this.GetColorspace(record.Operands, resources);
          switch (key2)
          {
            case "DeviceRGB":
              this.colorspace = (Colorspace) new DeviceRGB();
              break;
            case "DeviceCMYK":
              stringBuilder.Append("/DeviceRGB");
              stringBuilder.Append(" ");
              byte[] bytes1 = Encoding.Default.GetBytes(stringBuilder.ToString());
              pdfStream.Write(bytes1);
              this.appliedSCColorSpace = "DeviceCMYK";
              this.colorspace = (Colorspace) new DeviceCMYK();
              break;
            case "DeviceGray":
              this.colorspace = (Colorspace) new DeviceGray();
              break;
            default:
              if (resources.ContainsKey(new PdfName("ColorSpace")))
              {
                PdfDictionary fromRefernceHolder = this.GetDictionaryFromRefernceHolder(resources["ColorSpace"]);
                if (fromRefernceHolder != null && fromRefernceHolder.ContainsKey(key2))
                {
                  IPdfPrimitive pdfPrimitive = fromRefernceHolder[key2];
                  List<IPdfPrimitive> pdfPrimitiveList = new List<IPdfPrimitive>();
                  if ((object) (pdfPrimitive as PdfReferenceHolder) != null)
                    pdfPrimitiveList = ((pdfPrimitive as PdfReferenceHolder).Object as PdfArray).Elements;
                  else if (pdfPrimitive is PdfArray)
                    pdfPrimitiveList = (pdfPrimitive as PdfArray).Elements;
                  if (pdfPrimitiveList != null)
                  {
                    switch (colorspace)
                    {
                      case Separation _:
                        this.colorspace = (Colorspace) new Separation();
                        (this.colorspace as Separation).SetValue((pdfPrimitive as PdfReferenceHolder).Object as PdfArray);
                        this.appliedSCNColorSpace = "Separation";
                        if (pdfPrimitiveList.Contains((IPdfPrimitive) new PdfName("DeviceCMYK")))
                        {
                          PdfReferenceHolder pdfReferenceHolder = new PdfReferenceHolder((IPdfWrapper) new PdfICCColorSpace());
                          fromRefernceHolder[new PdfName("DefaultCMYK")] = (IPdfPrimitive) pdfReferenceHolder;
                          break;
                        }
                        break;
                      case Pattern _:
                        this.colorspace = (Colorspace) new Pattern();
                        (this.colorspace as Pattern).SetValue((IPdfPrimitive) ((pdfPrimitive as PdfReferenceHolder).Object as PdfArray));
                        this.appliedSCNColorSpace = "Pattern";
                        break;
                      case Indexed _:
                        this.colorspace = (Colorspace) new Indexed();
                        (this.colorspace as Indexed).SetValue((pdfPrimitive as PdfReferenceHolder).Object as PdfArray);
                        this.appliedSCColorSpace = "Indexed";
                        break;
                      case CalRGB _:
                        this.colorspace = (Colorspace) new CalRGB();
                        (this.colorspace as CalRGB).SetValue((pdfPrimitive as PdfReferenceHolder).Object as PdfArray);
                        this.appliedSCColorSpace = "CalRGB";
                        break;
                      case CalGray _:
                        this.colorspace = (Colorspace) new CalGray();
                        (this.colorspace as CalGray).SetValue((pdfPrimitive as PdfReferenceHolder).Object as PdfArray);
                        this.appliedSCColorSpace = "CalGray";
                        break;
                      case LabColor _:
                        this.colorspace = (Colorspace) new LabColor();
                        (this.colorspace as LabColor).SetValue((pdfPrimitive as PdfReferenceHolder).Object as PdfArray);
                        this.appliedSCColorSpace = "Lab";
                        break;
                      case DeviceN _:
                        this.colorspace = (Colorspace) new DeviceN();
                        (this.colorspace as DeviceN).SetValue((pdfPrimitive as PdfReferenceHolder).Object as PdfArray);
                        this.appliedSCNColorSpace = "DeviceN";
                        break;
                      case ICCBased _:
                        this.colorspace = (Colorspace) new ICCBased();
                        (this.colorspace as ICCBased).Profile = new ICCProfile((pdfPrimitive as PdfReferenceHolder).Object as PdfArray);
                        this.appliedSCNColorSpace = "ICCBased";
                        break;
                      case DeviceCMYK _:
                        this.colorspace = (Colorspace) new DeviceCMYK();
                        break;
                      case DeviceRGB _:
                        this.colorspace = (Colorspace) new DeviceRGB();
                        break;
                      case DeviceGray _:
                        this.colorspace = (Colorspace) new DeviceGray();
                        break;
                    }
                  }
                  else
                    break;
                }
                else
                  break;
              }
              else
                break;
              break;
          }
          if (key2 != "DeviceCMYK")
          {
            stringBuilder.Append(record.Operands[0]);
            stringBuilder.Append(" ");
            byte[] bytes2 = Encoding.Default.GetBytes(stringBuilder.ToString());
            pdfStream.Write(bytes2);
          }
        }
        else if ((record.OperatorName == "RG" || record.OperatorName == "rg") && flag1)
        {
          StringBuilder stringBuilder1 = new StringBuilder();
          Color color = new DeviceRGB().GetColor(record.Operands);
          float[] numArray = !(record.OperatorName == "RG") ? (!this.hasNonStroking ? this.ConvertTransparencyToRGB((float) color.R, (float) color.G, (float) color.B, -1f) : this.ConvertTransparencyToRGB((float) color.R, (float) color.G, (float) color.B, this.nonStrokingOpacity)) : (!this.hasStroking ? this.ConvertTransparencyToRGB((float) color.R, (float) color.G, (float) color.B, -1f) : this.ConvertTransparencyToRGB((float) color.R, (float) color.G, (float) color.B, this.strokingOpacity));
          StringBuilder stringBuilder2 = stringBuilder1;
          num2 = Math.Round((double) numArray[0], 2);
          string str1 = num2.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          stringBuilder2.Append(str1);
          stringBuilder1.Append(" ");
          StringBuilder stringBuilder3 = stringBuilder1;
          num2 = Math.Round((double) numArray[1], 2);
          string str2 = num2.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          stringBuilder3.Append(str2);
          stringBuilder1.Append(" ");
          StringBuilder stringBuilder4 = stringBuilder1;
          num2 = Math.Round((double) numArray[2], 2);
          string str3 = num2.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          stringBuilder4.Append(str3);
          stringBuilder1.Append(" ");
          byte[] bytes = Encoding.Default.GetBytes(stringBuilder1.ToString());
          pdfStream.Write(bytes);
        }
        else if (record.OperatorName == "K" || record.OperatorName == "k")
        {
          StringBuilder stringBuilder5 = new StringBuilder();
          Color color = new DeviceCMYK().GetColor(record.Operands);
          float[] numArray = new float[3]
          {
            (float) color.R,
            (float) color.G,
            (float) color.B
          };
          if (flag1)
          {
            numArray = !(record.OperatorName == "K") ? (!this.hasNonStroking ? this.ConvertTransparencyToRGB(numArray[0], numArray[1], numArray[2], -1f) : this.ConvertTransparencyToRGB(numArray[0], numArray[1], numArray[2], this.nonStrokingOpacity)) : (!this.hasStroking ? this.ConvertTransparencyToRGB(numArray[0], numArray[1], numArray[2], -1f) : this.ConvertTransparencyToRGB(numArray[0], numArray[1], numArray[2], this.strokingOpacity));
          }
          else
          {
            numArray[0] = numArray[0] / (float) byte.MaxValue;
            numArray[1] = numArray[1] / (float) byte.MaxValue;
            numArray[2] = numArray[2] / (float) byte.MaxValue;
          }
          StringBuilder stringBuilder6 = stringBuilder5;
          num2 = Math.Round((double) numArray[0], 2);
          string str4 = num2.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          stringBuilder6.Append(str4);
          stringBuilder5.Append(" ");
          StringBuilder stringBuilder7 = stringBuilder5;
          num2 = Math.Round((double) numArray[1], 2);
          string str5 = num2.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          stringBuilder7.Append(str5);
          stringBuilder5.Append(" ");
          StringBuilder stringBuilder8 = stringBuilder5;
          num2 = Math.Round((double) numArray[2], 2);
          string str6 = num2.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          stringBuilder8.Append(str6);
          stringBuilder5.Append(" ");
          byte[] bytes = Encoding.Default.GetBytes(stringBuilder5.ToString());
          pdfStream.Write(bytes);
        }
        else if ((record.OperatorName == "G" || record.OperatorName == "g") && flag1)
        {
          StringBuilder stringBuilder = new StringBuilder();
          float num3 = (float) System.Convert.ToDouble(record.Operands[0], (IFormatProvider) CultureInfo.InvariantCulture);
          if (record.OperatorName == "G" && this.hasStroking)
            num3 = (float) ((double) num3 * (double) this.strokingOpacity + (1.0 - (double) this.strokingOpacity));
          else if (record.OperatorName == "G" && this.hasNonStroking)
            num3 = (float) ((double) num3 * (double) this.nonStrokingOpacity + (1.0 - (double) this.nonStrokingOpacity));
          num3 = (float) Math.Round((double) num3, 2);
          stringBuilder.Append(num3.ToString((IFormatProvider) CultureInfo.InvariantCulture));
          stringBuilder.Append(" ");
          stringBuilder.Append(num3.ToString((IFormatProvider) CultureInfo.InvariantCulture));
          stringBuilder.Append(" ");
          stringBuilder.Append(num3.ToString((IFormatProvider) CultureInfo.InvariantCulture));
          stringBuilder.Append(" ");
          byte[] bytes = Encoding.Default.GetBytes(stringBuilder.ToString());
          pdfStream.Write(bytes);
        }
        else if ((record.OperatorName == "SCN" || record.OperatorName == "scn") && flag1)
        {
          StringBuilder stringBuilder9 = new StringBuilder();
          if (this.colorspace == null)
            this.colorspace = (Colorspace) new DeviceCMYK();
          bool flag4 = false;
          if (record.Operands.Length == 1 && record.Operands[0][0] == '/')
          {
            string key3 = record.Operands[0].Substring(1);
            if (resources.ContainsKey("Pattern"))
            {
              PdfDictionary fromRefernceHolder = this.GetDictionaryFromRefernceHolder(resources["Pattern"]);
              if (fromRefernceHolder != null && fromRefernceHolder.ContainsKey(key3))
                flag4 = true;
            }
          }
          if (flag4)
          {
            stringBuilder9.Append(record.Operands[0]);
            stringBuilder9.Append(" ");
            stringBuilder9.Append(record.OperatorName);
          }
          else
          {
            Color color = this.colorspace.GetColor(record.Operands);
            float[] numArray1 = new float[3]
            {
              (float) color.R,
              (float) color.G,
              (float) color.B
            };
            float[] numArray2 = !(record.OperatorName == "SCN") ? (!this.hasNonStroking ? this.ConvertTransparencyToRGB(numArray1[0], numArray1[1], numArray1[2], -1f) : this.ConvertTransparencyToRGB(numArray1[0], numArray1[1], numArray1[2], this.nonStrokingOpacity)) : (!this.hasStroking ? this.ConvertTransparencyToRGB(numArray1[0], numArray1[1], numArray1[2], -1f) : this.ConvertTransparencyToRGB(numArray1[0], numArray1[1], numArray1[2], this.strokingOpacity));
            StringBuilder stringBuilder10 = stringBuilder9;
            num2 = Math.Round((double) numArray2[0], 2);
            string str7 = num2.ToString((IFormatProvider) CultureInfo.InvariantCulture);
            stringBuilder10.Append(str7);
            stringBuilder9.Append(" ");
            StringBuilder stringBuilder11 = stringBuilder9;
            num2 = Math.Round((double) numArray2[1], 2);
            string str8 = num2.ToString((IFormatProvider) CultureInfo.InvariantCulture);
            stringBuilder11.Append(str8);
            stringBuilder9.Append(" ");
            StringBuilder stringBuilder12 = stringBuilder9;
            num2 = Math.Round((double) numArray2[2], 2);
            string str9 = num2.ToString((IFormatProvider) CultureInfo.InvariantCulture);
            stringBuilder12.Append(str9);
            stringBuilder9.Append(" ");
            if (record.OperatorName == "SCN")
              stringBuilder9.Append("RG");
            else
              stringBuilder9.Append("rg");
          }
          stringBuilder9.Append("\r\n");
          byte[] bytes = Encoding.Default.GetBytes(stringBuilder9.ToString());
          pdfStream.Write(bytes);
        }
        else if ((record.OperatorName == "SC" || record.OperatorName == "sc") && flag1)
        {
          StringBuilder stringBuilder13 = new StringBuilder();
          if (record.Operands.Length == 1)
          {
            float num4 = (float) System.Convert.ToDouble(record.Operands[0], (IFormatProvider) CultureInfo.InvariantCulture);
            if (record.OperatorName == "G" && this.hasStroking)
              num4 = (float) ((double) num4 * (double) this.strokingOpacity + (1.0 - (double) this.strokingOpacity));
            else if (record.OperatorName == "G" && this.hasNonStroking)
              num4 = (float) ((double) num4 * (double) this.nonStrokingOpacity + (1.0 - (double) this.nonStrokingOpacity));
            float num5 = (float) Math.Round((double) num4, 2);
            stringBuilder13.Append(num5.ToString((IFormatProvider) CultureInfo.InvariantCulture));
            stringBuilder13.Append(" ");
            if (record.OperatorName == "SC")
              stringBuilder13.Append("G");
            else
              stringBuilder13.Append("g");
          }
          else if (record.Operands.Length == 3)
          {
            Color color = this.colorspace.GetColor(record.Operands);
            float[] numArray3 = new float[3]
            {
              (float) color.R,
              (float) color.G,
              (float) color.B
            };
            float[] numArray4 = !(record.OperatorName == "SC") ? (!this.hasNonStroking ? this.ConvertTransparencyToRGB(numArray3[0], numArray3[1], numArray3[2], -1f) : this.ConvertTransparencyToRGB(numArray3[0], numArray3[1], numArray3[2], this.nonStrokingOpacity)) : (!this.hasStroking ? this.ConvertTransparencyToRGB(numArray3[0], numArray3[1], numArray3[2], -1f) : this.ConvertTransparencyToRGB(numArray3[0], numArray3[1], numArray3[2], this.strokingOpacity));
            StringBuilder stringBuilder14 = stringBuilder13;
            num2 = Math.Round((double) numArray4[0], 2);
            string str10 = num2.ToString((IFormatProvider) CultureInfo.InvariantCulture);
            stringBuilder14.Append(str10);
            stringBuilder13.Append(" ");
            StringBuilder stringBuilder15 = stringBuilder13;
            num2 = Math.Round((double) numArray4[1], 2);
            string str11 = num2.ToString((IFormatProvider) CultureInfo.InvariantCulture);
            stringBuilder15.Append(str11);
            stringBuilder13.Append(" ");
            StringBuilder stringBuilder16 = stringBuilder13;
            num2 = Math.Round((double) numArray4[2], 2);
            string str12 = num2.ToString((IFormatProvider) CultureInfo.InvariantCulture);
            stringBuilder16.Append(str12);
            stringBuilder13.Append(" ");
            if (record.OperatorName == "SC")
              stringBuilder13.Append("RG");
            else
              stringBuilder13.Append("rg");
          }
          stringBuilder13.Append("\r\n");
          byte[] bytes = Encoding.Default.GetBytes(stringBuilder13.ToString());
          pdfStream.Write(bytes);
        }
        else if (record.OperatorName == "Tj" || record.OperatorName == "'")
        {
          PdfDictionary fontDescriptor = (PdfDictionary) null;
          PdfDictionary descendantFont = (PdfDictionary) null;
          this.GetFontInternals(fontDictionary, out fontDescriptor, out descendantFont);
          bool flag5 = false;
          if (!string.IsNullOrEmpty(key1) && this.OldFonts.ContainsKey(key1) && this.ReplaceFonts.ContainsKey(key1) && this.ReplaceFonts[key1] != null)
          {
            PdfDictionary fromRefernceHolder = this.GetDictionaryFromRefernceHolder(this.ReplaceFonts[key1].FontInternal);
            if (fromRefernceHolder != null && fromRefernceHolder.ContainsKey("Subtype") && (fromRefernceHolder["Subtype"] as PdfName).Value == "Type0")
            {
              PdfString unicodeString = this.GetUnicodeString(this.ConvertToUnicode(new FontStructure((IPdfPrimitive) this.OldFonts[key1])
              {
                FontSize = num1
              }.DecodeTextExtraction(string.Join("", record.Operands), true), this.ReplaceFonts[key1]));
              resources.Remove(key1);
              resources.Add((PdfFont) this.ReplaceFonts[key1], new PdfName(key1));
              pdfStream.Write(unicodeString.PdfEncode((PdfDocumentBase) null));
              pdfStream.Write(" ");
              flag5 = true;
            }
          }
          else if (!string.IsNullOrEmpty(key1) && this.ReplaceFontDictionary.ContainsKey(key1) && this.OldFonts.ContainsKey(key1))
          {
            fontDictionary = this.OldFonts[key1];
            if (fontDictionary.ContainsKey("Encoding") && PdfCrossTable.Dereference(fontDictionary["Encoding"]) is PdfDictionary pdfDictionary3 && pdfDictionary3.ContainsKey("Differences"))
            {
              resources.Remove(key1);
              IPdfPrimitive pdfPrimitive = resources["Font"];
              PdfDictionary pdfDictionary2;
              if (pdfPrimitive != null)
              {
                PdfReferenceHolder pdfReferenceHolder = pdfPrimitive as PdfReferenceHolder;
                pdfDictionary2 = pdfPrimitive as PdfDictionary;
                if (pdfReferenceHolder != (PdfReferenceHolder) null)
                  pdfDictionary2 = PdfCrossTable.Dereference(pdfPrimitive) as PdfDictionary;
              }
              else
              {
                pdfDictionary2 = new PdfDictionary();
                resources["Font"] = (IPdfPrimitive) pdfDictionary2;
              }
              pdfDictionary2[new PdfName(key1)] = (IPdfPrimitive) this.ReplaceFontDictionary[key1];
            }
          }
          if (!flag5)
          {
            PdfString pdfString = new PdfString(this.TrimOperand(record.Operands[0]));
            pdfStream.Write(pdfString.Bytes);
          }
        }
        else if (record.OperatorName == "TJ")
        {
          string empty = string.Empty;
          PdfDictionary fontDescriptor = (PdfDictionary) null;
          PdfDictionary descendantFont = (PdfDictionary) null;
          this.GetFontInternals(fontDictionary, out fontDescriptor, out descendantFont);
          bool flag6 = false;
          if (fontDescriptor == null || fontDescriptor != null && fontDescriptor.ContainsKey("Flags") && (fontDescriptor["Flags"] as PdfNumber).IntValue != 4 && (fontDescriptor["Flags"] as PdfNumber).IntValue != 32 /*0x20*/)
          {
            if (!string.IsNullOrEmpty(key1) && this.OldFonts.ContainsKey(key1) && this.ReplaceFonts.ContainsKey(key1) && this.ReplaceFonts[key1] != null)
            {
              PdfDictionary fromRefernceHolder = this.GetDictionaryFromRefernceHolder(this.ReplaceFonts[key1].FontInternal);
              if (fromRefernceHolder != null && fromRefernceHolder.ContainsKey("Subtype") && (fromRefernceHolder["Subtype"] as PdfName).Value == "Type0")
              {
                PdfString unicodeString = this.GetUnicodeString(this.ConvertToUnicode(new FontStructure((IPdfPrimitive) this.OldFonts[key1])
                {
                  FontSize = num1
                }.DecodeTextExtraction(string.Join("", record.Operands), true), this.ReplaceFonts[key1]));
                resources.Remove(key1);
                resources.Add((PdfFont) this.ReplaceFonts[key1], new PdfName(key1));
                pdfStream.Write(unicodeString.PdfEncode((PdfDocumentBase) null));
                pdfStream.Write(" ");
                flag6 = true;
                record.OperatorName = "Tj";
              }
            }
          }
          else if (this.ReplaceFonts.ContainsKey(key1) && this.OldFonts.ContainsKey(key1))
          {
            fontDictionary = this.OldFonts[key1];
            if (fontDictionary.ContainsKey("Encoding") && PdfCrossTable.Dereference(fontDictionary["Encoding"]) is PdfDictionary pdfDictionary4 && pdfDictionary4.ContainsKey("Differences"))
            {
              PdfString unicodeString = this.GetUnicodeString(this.ConvertToUnicode(new FontStructure((IPdfPrimitive) fontDictionary)
              {
                FontSize = num1
              }.DecodeTextExtraction(string.Join("", record.Operands), true), this.ReplaceFonts[key1]));
              resources.Remove(key1);
              resources.Add((PdfFont) this.ReplaceFonts[key1], new PdfName(key1));
              pdfStream.Write(unicodeString.PdfEncode((PdfDocumentBase) null));
              pdfStream.Write(" ");
              flag6 = true;
              record.OperatorName = "Tj";
            }
          }
          else if (!string.IsNullOrEmpty(key1) && this.ReplaceFontDictionary.ContainsKey(key1) && this.OldFonts.ContainsKey(key1))
          {
            fontDictionary = this.OldFonts[key1];
            if (fontDictionary.ContainsKey("Encoding") && PdfCrossTable.Dereference(fontDictionary["Encoding"]) is PdfDictionary pdfDictionary6 && pdfDictionary6.ContainsKey("Differences"))
            {
              resources.Remove(key1);
              IPdfPrimitive pdfPrimitive = resources["Font"];
              PdfDictionary pdfDictionary5;
              if (pdfPrimitive != null)
              {
                PdfReferenceHolder pdfReferenceHolder = pdfPrimitive as PdfReferenceHolder;
                pdfDictionary5 = pdfPrimitive as PdfDictionary;
                if (pdfReferenceHolder != (PdfReferenceHolder) null)
                  pdfDictionary5 = PdfCrossTable.Dereference(pdfPrimitive) as PdfDictionary;
              }
              else
              {
                pdfDictionary5 = new PdfDictionary();
                resources["Font"] = (IPdfPrimitive) pdfDictionary5;
              }
              pdfDictionary5[new PdfName(key1)] = (IPdfPrimitive) this.ReplaceFontDictionary[key1];
            }
          }
          if (!flag6)
          {
            PdfString pdfString = new PdfString(this.TrimOperand(record.Operands[0]));
            pdfStream.Write(pdfString.Bytes);
            pdfStream.Write(" ");
          }
        }
        else if (record.OperatorName == "\"")
        {
          PdfDictionary fontDescriptor = (PdfDictionary) null;
          PdfDictionary descendantFont = (PdfDictionary) null;
          this.GetFontInternals(fontDictionary, out fontDescriptor, out descendantFont);
          bool flag7 = false;
          if (fontDescriptor == null || fontDescriptor != null && fontDescriptor.ContainsKey("Flags") && (fontDescriptor["Flags"] as PdfNumber).IntValue != 4 && (fontDescriptor["Flags"] as PdfNumber).IntValue != 32 /*0x20*/)
          {
            if (!string.IsNullOrEmpty(key1) && this.OldFonts.ContainsKey(key1) && this.ReplaceFonts.ContainsKey(key1) && this.ReplaceFonts[key1] != null)
            {
              PdfDictionary fromRefernceHolder = this.GetDictionaryFromRefernceHolder(this.ReplaceFonts[key1].FontInternal);
              if (fromRefernceHolder != null && fromRefernceHolder.ContainsKey("Subtype") && (fromRefernceHolder["Subtype"] as PdfName).Value == "Type0")
              {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(this.TrimOperand(record.Operands[0]));
                stringBuilder.Append(" Tw");
                stringBuilder.Append("\r\n");
                stringBuilder.Append(this.TrimOperand(record.Operands[1]));
                stringBuilder.Append(" Tc");
                stringBuilder.Append("\r\n");
                PdfString unicodeString = this.GetUnicodeString(this.ConvertToUnicode(new FontStructure((IPdfPrimitive) this.OldFonts[key1])
                {
                  FontSize = num1
                }.DecodeTextExtraction(record.Operands[2], true), this.ReplaceFonts[key1]));
                resources.Remove(key1);
                resources.Add((PdfFont) this.ReplaceFonts[key1], new PdfName(key1));
                pdfStream.Write(unicodeString.PdfEncode((PdfDocumentBase) null));
                pdfStream.Write(" ");
                flag7 = true;
              }
            }
            else if (!string.IsNullOrEmpty(key1) && this.ReplaceFontDictionary.ContainsKey(key1) && this.OldFonts.ContainsKey(key1))
            {
              fontDictionary = this.OldFonts[key1];
              if (fontDictionary.ContainsKey("Encoding") && PdfCrossTable.Dereference(fontDictionary["Encoding"]) is PdfDictionary pdfDictionary8 && pdfDictionary8.ContainsKey("Differences"))
              {
                resources.Remove(key1);
                IPdfPrimitive pdfPrimitive = resources["Font"];
                PdfDictionary pdfDictionary7;
                if (pdfPrimitive != null)
                {
                  PdfReferenceHolder pdfReferenceHolder = pdfPrimitive as PdfReferenceHolder;
                  pdfDictionary7 = pdfPrimitive as PdfDictionary;
                  if (pdfReferenceHolder != (PdfReferenceHolder) null)
                    pdfDictionary7 = PdfCrossTable.Dereference(pdfPrimitive) as PdfDictionary;
                }
                else
                {
                  pdfDictionary7 = new PdfDictionary();
                  resources["Font"] = (IPdfPrimitive) pdfDictionary7;
                }
                pdfDictionary7[new PdfName(key1)] = (IPdfPrimitive) this.ReplaceFontDictionary[key1];
              }
            }
          }
          if (!flag7)
          {
            for (int index5 = 0; index5 < record.Operands.Length; ++index5)
            {
              PdfString pdfString = new PdfString(this.TrimOperand(record.Operands[index5]));
              pdfStream.Write(pdfString.Bytes);
              pdfStream.Write(" ");
            }
          }
        }
        else if (record.OperatorName == "ID")
        {
          for (int index6 = 0; index6 < record.Operands.Length; ++index6)
          {
            string str = record.Operands[index6];
            if (str.IndexOf("/") == 0)
              str = str.Replace("/", "");
            switch (str)
            {
              case "F":
                string operand = record.Operands[index6 + 1];
                if (operand.Contains("LZWDecode") || operand.Contains("LZW"))
                {
                  record.Operands[index6 + 1] = "[/FlateDecode]";
                  flag2 = true;
                  break;
                }
                flag2 = false;
                break;
              case "Filter":
                if (record.Operands.Length <= index6 + 1)
                  goto default;
                goto case "F";
              default:
                flag2 = false;
                break;
            }
            if (record.OperatorName != "Tj" && record.OperatorName != "'" && record.OperatorName != "\"" && record.OperatorName != "TJ")
              str = this.TrimOperand(record.Operands[index6]);
            PdfString pdfString = new PdfString(str);
            pdfStream.Write(pdfString.Bytes);
            if (record.OperatorName != "'" && record.OperatorName != "\"")
              pdfStream.Write(" ");
          }
        }
        else
        {
          for (int index7 = 0; index7 < record.Operands.Length; ++index7)
          {
            string operand = record.Operands[index7];
            if (record.OperatorName != "Tj" && record.OperatorName != "'" && record.OperatorName != "\"" && record.OperatorName != "TJ")
              operand = this.TrimOperand(operand);
            PdfString pdfString = new PdfString(operand);
            pdfStream.Write(pdfString.Bytes);
            if (record.OperatorName != "'" && record.OperatorName != "\"")
              pdfStream.Write(" ");
          }
        }
      }
      if (record.OperatorName == "\"" && !string.IsNullOrEmpty(key1) && this.ReplaceFonts.ContainsKey(key1))
        pdfStream.Write("'");
      if (record.OperatorName == "K" || record.OperatorName == "G" && flag1)
        pdfStream.Write("RG");
      else if (record.OperatorName == "k" || record.OperatorName == "g" && flag1)
        pdfStream.Write("rg");
      else if ((!(record.OperatorName == "gs") || !flag1) && (!(record.OperatorName == "SCN") && !(record.OperatorName == "scn") && !(record.OperatorName == "SC") && !(record.OperatorName == "sc") || !flag1))
      {
        if ((record.OperatorName == "SC" || record.OperatorName == "sc") && !flag1)
        {
          bool flag8 = record.OperatorName == "SC";
          if (record.Operands.Length == 1)
            pdfStream.Write(flag8 ? "G" : "g");
          if (record.Operands.Length == 3)
            pdfStream.Write(flag8 ? "RG" : "rg");
          if (record.Operands.Length == 4)
            pdfStream.Write(flag8 ? "K" : "k");
        }
        else if (record.OperatorName == "EI")
        {
          if (record.InlineImageBytes.Length != 0)
          {
            if (flag2)
            {
              byte[] data = new PdfLzwCompressor().Decompress(record.InlineImageBytes);
              PdfZlibCompressor pdfZlibCompressor = new PdfZlibCompressor();
              record.InlineImageBytes = pdfZlibCompressor.Compress(data);
            }
            pdfStream.Write(record.InlineImageBytes);
            Encoding.UTF8.GetString(record.InlineImageBytes);
            pdfStream.Write(record.OperatorName);
          }
          flag2 = false;
        }
        else
        {
          if (record.OperatorName == "Q" || record.OperatorName == "ET")
            flag1 = false;
          pdfStream.Write(record.OperatorName);
        }
      }
      else
        continue;
      if (index1 + 1 < count1 || this.isPdfPage)
      {
        if (index1 + 1 < count1 && (record.OperatorName == "W" || record.OperatorName == "W*") && recordCollection.RecordCollection[index1 + 1].OperatorName == "n")
          pdfStream.Write(" ");
        else if (record.OperatorName == "w" || record.OperatorName == "ID")
          pdfStream.Write(" ");
        else
          pdfStream.Write("\r\n");
      }
    }
    if (count1 == 0)
    {
      pdfStream.Write("q");
      pdfStream.Write("\r\n");
      pdfStream.Write("Q");
      pdfStream.Write("\r\n");
    }
    if (stringList.Count > 0 && resources.ContainsKey("ExtGState"))
    {
      IPdfPrimitive primitive = resources["ExtGState"];
      foreach (string str in stringList)
      {
        if (this.GetDictionaryFromRefernceHolder(primitive).Items.Count == 1)
        {
          lPage.GetResources().Remove("ExtGState");
          lPage.GetResources().Modify();
        }
        else
        {
          this.GetDictionaryFromRefernceHolder(primitive).Remove(new PdfName(str));
          this.GetDictionaryFromRefernceHolder(primitive).Modify();
        }
      }
    }
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
    this.ResetPageResources();
    return contentStream;
  }

  private PdfString GetUnicodeString(string token)
  {
    return new PdfString(token)
    {
      Converted = true,
      Encode = PdfString.ForceEncoding.ASCII
    };
  }

  public string ConvertString(string text, TtfReader ttfReader)
  {
    char[] chArray = text != null ? new char[text.Length] : throw new ArgumentNullException(nameof (text));
    int length1 = 0;
    int index = 0;
    for (int length2 = text.Length; index < length2; ++index)
    {
      char charCode = text[index];
      TtfGlyphInfo glyph = ttfReader.GetGlyph(charCode);
      if (!glyph.Empty)
        chArray[length1++] = (char) glyph.Index;
    }
    return new string(chArray, 0, length1);
  }

  private string ConvertToUnicode(string text, PdfTrueTypeFont ttfFont)
  {
    string empty = string.Empty;
    if (ttfFont.InternalFont is UnicodeTrueTypeFont)
    {
      TtfReader ttfReader = (ttfFont.InternalFont as UnicodeTrueTypeFont).TtfReader;
      UnicodeTrueTypeFont internalFont = ttfFont.InternalFont as UnicodeTrueTypeFont;
      List<TtfGlyphInfo> ttfGlyphInfoList = new List<TtfGlyphInfo>();
      ttfFont.SetSymbols(text);
      internalFont.SetGlyphInfo(internalFont.GetGlyphInfo());
      PdfArray descendantWidth = internalFont.GetDescendantWidth();
      PdfDictionary pdfDictionary = new PdfDictionary();
      List<TtfGlyphInfo> allGlyphs = ttfReader.GetAllGlyphs();
      if (internalFont.GetUsedCharsCount() > allGlyphs.Count)
        internalFont.m_isIncreasedUsedChar = true;
      pdfDictionary["W"] = (IPdfPrimitive) descendantWidth;
      empty = PdfString.ByteToString(PdfString.ToUnicodeArray(ttfReader.ConvertString(text), false));
    }
    return empty;
  }

  private void ContentStreamParsing(PdfLoadedDocument document)
  {
    int pageNo = 0;
    foreach (PdfPageBase page in document.Pages)
    {
      if (page is PdfPage)
        this.isPdfPage = true;
      PdfArray contents = page.Contents;
      if (page.Dictionary.ContainsKey("AA"))
      {
        page.Dictionary.Remove("AA");
        page.Dictionary.Modify();
      }
      this.AnnotationConsideration(page, document);
      if (this.PdfALevel == PdfConformanceLevel.Pdf_A1B)
        this.RemoveTransparencyGroup(page);
      this.CompressionConsideration(page);
      this.EmbedCompleteFonts(page, document);
      this.ParseFormResources(page);
      MemoryStream contentStream1 = new MemoryStream();
      page.Layers.CombineContent((Stream) contentStream1);
      MemoryStream contentStream2 = this.ParseContentStream(contentStream1, page, pageNo);
      ++pageNo;
      this.ReplaceCMYKColorSpace(page);
      PdfStream pdfStream = new PdfStream();
      pdfStream.Data = contentStream2.ToArray();
      contentStream2.Dispose();
      if (page.Dictionary.ContainsKey("Contents"))
      {
        PdfArray fromReferenceHolder = this.GetArrayFromReferenceHolder(page.Dictionary["Contents"]);
        if (fromReferenceHolder != null)
        {
          foreach (IPdfPrimitive content in page.Contents)
          {
            PdfDictionary pdfDictionary = this.GetObject(content);
            if (pdfDictionary != null)
              pdfDictionary.isSkip = true;
          }
          fromReferenceHolder.Clear();
          fromReferenceHolder.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfStream));
        }
        else
        {
          PdfStream fromRefernceHolder = this.GetStreamFromRefernceHolder(page.Dictionary["Contents"]);
          if (fromRefernceHolder != null)
          {
            fromRefernceHolder.Clear();
            fromRefernceHolder.Items.Remove(new PdfName("Length"));
            fromRefernceHolder.Data = pdfStream.Data;
          }
        }
      }
      if (document.RaiseTrackPdfAConversionProgress)
      {
        float num = 50f / (float) document.Pages.Count + this.args.m_progressValue;
        if ((int) this.args.m_progressValue != (int) num)
        {
          this.args.m_progressValue = (float) (int) num;
          document.OnPdfAConversionTrackProgress(this.args);
          this.args.m_progressValue = num;
        }
        else
          this.args.m_progressValue = num;
      }
    }
  }

  private void ParseFormResources(PdfPageBase lPage)
  {
    PdfResources resources = lPage.GetResources();
    if (!resources.ContainsKey("XObject"))
      return;
    this.ParseXObjectDictionary(this.GetDictionaryFromRefernceHolder(resources["XObject"]));
  }

  private void ParseXObjectDictionary(PdfDictionary xObjectDictionary)
  {
    if (xObjectDictionary == null || xObjectDictionary.Items.Count <= 0)
      return;
    foreach (PdfName key in xObjectDictionary.Keys)
    {
      PdfStream fromRefernceHolder1 = this.GetStreamFromRefernceHolder(xObjectDictionary[key]);
      if (fromRefernceHolder1 != null && fromRefernceHolder1.ContainsKey("Subtype") && (fromRefernceHolder1["Subtype"] as PdfName).Value == "Form")
      {
        if (fromRefernceHolder1.Data.Length <= 0)
        {
          fromRefernceHolder1.Data = new byte[1]
          {
            (byte) 32 /*0x20*/
          };
          fromRefernceHolder1.Modify();
        }
        else
          this.ParseFormStream(fromRefernceHolder1, key.Value, this.GetDictionaryFromRefernceHolder(fromRefernceHolder1["Resources"]), false);
        if (fromRefernceHolder1.ContainsKey("Resources"))
        {
          PdfDictionary fromRefernceHolder2 = this.GetDictionaryFromRefernceHolder(fromRefernceHolder1["Resources"]);
          if (fromRefernceHolder2 != null && fromRefernceHolder2.Items.Count > 0 && fromRefernceHolder2.ContainsKey("XObject"))
          {
            PdfDictionary fromRefernceHolder3 = this.GetDictionaryFromRefernceHolder(fromRefernceHolder2["XObject"]);
            bool flag = false;
            if (fromRefernceHolder3.Count == xObjectDictionary.Count)
            {
              flag = true;
              foreach (KeyValuePair<PdfName, IPdfPrimitive> keyValuePair in fromRefernceHolder3.Items)
              {
                IPdfPrimitive pdfPrimitive;
                if (xObjectDictionary.Items.TryGetValue(keyValuePair.Key, out pdfPrimitive))
                {
                  if ((pdfPrimitive as PdfReferenceHolder).Reference != (keyValuePair.Value as PdfReferenceHolder).Reference)
                  {
                    flag = false;
                    break;
                  }
                }
                else
                {
                  flag = false;
                  break;
                }
              }
            }
            if (!flag)
              this.ParseXObjectDictionary(this.GetDictionaryFromRefernceHolder(fromRefernceHolder2["XObject"]));
            else
              fromRefernceHolder2.Remove("XObject");
          }
        }
      }
    }
  }

  private bool RemoveEmbeddedFilesReference(PdfDictionary elementDictionary)
  {
    bool flag = false;
    if (elementDictionary != null && elementDictionary.ContainsKey("EmbeddedFiles"))
    {
      elementDictionary.Remove("EmbeddedFiles");
      elementDictionary.Modify();
      flag = true;
    }
    if (elementDictionary != null && elementDictionary.ContainsKey("JavaScript"))
    {
      elementDictionary.Remove("JavaScript");
      elementDictionary.Modify();
      flag = true;
    }
    return flag;
  }

  private void AttachmentsConsideration(PdfLoadedDocument document)
  {
    if (document.Catalog.ContainsKey("Names"))
    {
      if ((object) (document.Catalog["Names"] as PdfReferenceHolder) != null)
      {
        PdfDictionary fromRefernceHolder1 = this.GetDictionaryFromRefernceHolder((IPdfPrimitive) (document.Catalog["Names"] as PdfReferenceHolder));
        if (this.RemoveEmbeddedFilesReference(fromRefernceHolder1))
        {
          document.Catalog.Remove("Names");
          document.Catalog.Modify();
        }
        else if (fromRefernceHolder1 != null && fromRefernceHolder1.ContainsKey("AP"))
        {
          PdfDictionary fromRefernceHolder2 = this.GetDictionaryFromRefernceHolder(fromRefernceHolder1["AP"]);
          if (fromRefernceHolder2 != null && fromRefernceHolder2.ContainsKey("Names"))
          {
            IPdfPrimitive pdfPrimitive = fromRefernceHolder2["Names"];
            if (pdfPrimitive is PdfArray)
            {
              foreach (IPdfPrimitive element in (pdfPrimitive as PdfArray).Elements)
              {
                if ((object) (element as PdfReferenceHolder) != null)
                  this.ParseFormStreamFromXObject(this.GetDictionaryFromRefernceHolder(element), "XObject");
              }
            }
          }
        }
      }
      else if (document.Catalog["Names"] is PdfDictionary && this.RemoveEmbeddedFilesReference(this.GetDictionaryFromRefernceHolder(document.Catalog["Names"])))
      {
        document.Catalog.Remove("Names");
        document.Catalog.Modify();
      }
    }
    PdfAttachmentCollection attachments = document.Attachments;
    if (attachments == null || attachments.Count == 0)
      return;
    for (int index = 0; index < attachments.Count; ++index)
    {
      PdfAttachment attachment = attachments[index];
      if (attachment.EmbeddedFile != null)
        document.Attachments.Remove(attachment);
    }
  }

  private bool IsValidAction(string action)
  {
    return !(action == "ResetForm") && !(action == "SubmitForm") && !(action == "Sound") && !(action == "Movie") && !(action == "Launch") && !(action == "JavaScript");
  }

  private void AnnotationConsideration(PdfPageBase lPage, PdfLoadedDocument document)
  {
    if (!this.isPdfPage && (lPage as PdfLoadedPage).Annotations.Count > 0)
    {
      List<PdfLoadedAnnotation> loadedAnnotationList = new List<PdfLoadedAnnotation>();
      foreach (PdfLoadedAnnotation annotation in (PdfCollection) (lPage as PdfLoadedPage).Annotations)
        loadedAnnotationList.Add(annotation);
      foreach (PdfLoadedAnnotation annot in loadedAnnotationList)
      {
        switch (annot)
        {
          case PdfLoadedSoundAnnotation _:
          case PdfLoadedAttachmentAnnotation _:
            (lPage as PdfLoadedPage).Annotations.Remove((PdfAnnotation) annot);
            continue;
          default:
            PdfDictionary fromRefernceHolder = this.GetDictionaryFromRefernceHolder((IPdfPrimitive) annot.Dictionary);
            if (fromRefernceHolder != null)
            {
              if (fromRefernceHolder.ContainsKey(new PdfName("RichMediaContent")) || fromRefernceHolder.ContainsKey(new PdfName("RichMediaSettings")))
              {
                (lPage as PdfLoadedPage).Annotations.Remove((PdfAnnotation) annot);
                continue;
              }
              if (annot is PdfLoadedWidgetAnnotation)
              {
                if (!fromRefernceHolder.ContainsKey("AP"))
                {
                  if (fromRefernceHolder.ContainsKey("FT"))
                  {
                    PdfName pdfName = fromRefernceHolder["FT"] as PdfName;
                    fromRefernceHolder["AP"] = !(pdfName != (PdfName) null) || !(pdfName.Value == "Btn") ? (IPdfPrimitive) this.CreateNewAppearanceDictionary(false) : (IPdfPrimitive) this.CreateNewAppearanceDictionary(true);
                  }
                  else
                    fromRefernceHolder["AP"] = (IPdfPrimitive) this.CreateNewAppearanceDictionary(false);
                }
              }
              else if (!fromRefernceHolder.ContainsKey("AP"))
                annot.SetAppearance(true);
              this.RemoveInvalidAnnotations(fromRefernceHolder, lPage, document);
              continue;
            }
            continue;
        }
      }
    }
    if (this.isPdfPage || (lPage as PdfLoadedPage).AnnotsReference == null || (lPage as PdfLoadedPage).AnnotsReference.Count <= 0)
      return;
    foreach (PdfReference pointer in (lPage as PdfLoadedPage).AnnotsReference)
    {
      PdfDictionary fromRefernceHolder = this.GetDictionaryFromRefernceHolder(document.CrossTable.GetObject((IPdfPrimitive) pointer));
      if (fromRefernceHolder != null)
      {
        if (fromRefernceHolder.ContainsKey(new PdfName("RichMediaContent")) || fromRefernceHolder.ContainsKey(new PdfName("RichMediaSettings")) || (fromRefernceHolder["Subtype"] as PdfName).Value == "3D")
        {
          if (lPage.Dictionary.ContainsKey("Annots"))
          {
            PdfArray fromReferenceHolder = this.GetArrayFromReferenceHolder(lPage.Dictionary["Annots"]);
            foreach (PdfReferenceHolder element in fromReferenceHolder)
            {
              if (element != (PdfReferenceHolder) null && element.Reference.Equals((object) pointer))
              {
                fromReferenceHolder.Remove((IPdfPrimitive) element);
                fromReferenceHolder.MarkChanged();
                break;
              }
            }
          }
        }
        else
        {
          if (fromRefernceHolder.ContainsKey("Subtype") && (fromRefernceHolder["Subtype"] as PdfName).Value == "Widget" && !fromRefernceHolder.ContainsKey("AP"))
          {
            if (fromRefernceHolder.ContainsKey("FT"))
            {
              PdfName pdfName = fromRefernceHolder["FT"] as PdfName;
              fromRefernceHolder["AP"] = !(pdfName != (PdfName) null) || !(pdfName.Value == "Btn") ? (IPdfPrimitive) this.CreateNewAppearanceDictionary(false) : (IPdfPrimitive) this.CreateNewAppearanceDictionary(true);
            }
            else
              fromRefernceHolder["AP"] = (IPdfPrimitive) this.CreateNewAppearanceDictionary(false);
          }
          this.RemoveInvalidAnnotations(fromRefernceHolder, lPage, document);
        }
      }
    }
  }

  private void RemoveInvalidAnnotations(
    PdfDictionary annotationDictionary,
    PdfPageBase lPage,
    PdfLoadedDocument document)
  {
    bool isWidget = false;
    if (annotationDictionary.ContainsKey("Subtype") && (annotationDictionary["Subtype"] as PdfName).Value == "Widget")
      isWidget = true;
    if (annotationDictionary.ContainsKey("CA"))
    {
      annotationDictionary.Remove("CA");
      annotationDictionary.Modify();
    }
    if (annotationDictionary.ContainsKey("AP"))
    {
      PdfDictionary fromRefernceHolder1 = this.GetDictionaryFromRefernceHolder(annotationDictionary["AP"]);
      if (fromRefernceHolder1 != null)
      {
        if (fromRefernceHolder1.ContainsKey("N"))
        {
          if (!(fromRefernceHolder1["N"] is PdfStream))
          {
            PdfDictionary fromRefernceHolder2 = this.GetDictionaryFromRefernceHolder(fromRefernceHolder1["N"]);
            if (fromRefernceHolder2 is PdfStream)
              this.ParseAppearanceStreamInAnnotations(fromRefernceHolder2 as PdfStream, "N", isWidget);
            else if (fromRefernceHolder2.Keys.Count > 0)
            {
              foreach (PdfName key in new List<PdfName>((IEnumerable<PdfName>) fromRefernceHolder2.Keys))
                this.ParseAppearanceStreamInAnnotations(this.GetStreamFromRefernceHolder(fromRefernceHolder2[key]), key.Value, isWidget);
            }
          }
          else
            this.ParseAppearanceStreamInAnnotations(this.GetStreamFromRefernceHolder(fromRefernceHolder1["N"]), "N", isWidget);
        }
        if (fromRefernceHolder1.ContainsKey("D"))
        {
          if (fromRefernceHolder1.ContainsKey("N"))
          {
            fromRefernceHolder1.Remove("D");
            fromRefernceHolder1.Modify();
          }
          else if (!(fromRefernceHolder1["D"] is PdfStream))
          {
            PdfDictionary fromRefernceHolder3 = this.GetDictionaryFromRefernceHolder(fromRefernceHolder1["D"]);
            if (fromRefernceHolder3 is PdfStream)
              this.ParseAppearanceStreamInAnnotations(fromRefernceHolder3 as PdfStream, "D", isWidget);
            else if (fromRefernceHolder3.Keys.Count > 0)
            {
              foreach (PdfName key in new List<PdfName>((IEnumerable<PdfName>) fromRefernceHolder3.Keys))
                this.ParseAppearanceStreamInAnnotations(this.GetStreamFromRefernceHolder(fromRefernceHolder3[key]), key.Value, isWidget);
            }
          }
          else
            this.ParseAppearanceStreamInAnnotations(this.GetStreamFromRefernceHolder(fromRefernceHolder1["D"]), "D", isWidget);
        }
      }
    }
    if (annotationDictionary.ContainsKey("A"))
      annotationDictionary.Remove("A");
    if (annotationDictionary.ContainsKey("AA"))
      annotationDictionary.Remove("AA");
    annotationDictionary["F"] = (IPdfPrimitive) new PdfNumber(4);
    annotationDictionary.Modify();
  }

  private void ParseAppearanceStreamInAnnotations(
    PdfStream appearanceStream,
    string appearanceKey,
    bool isWidget)
  {
    if (appearanceStream == null)
      return;
    if (appearanceStream.Data.Length <= 0)
    {
      appearanceStream.Data = new byte[1]
      {
        (byte) 32 /*0x20*/
      };
      appearanceStream.Modify();
    }
    if (!isWidget)
      this.ParseFormStream(appearanceStream, appearanceKey, this.GetDictionaryFromRefernceHolder(appearanceStream["Resources"]), true);
    if (!appearanceStream.ContainsKey("Resources"))
      return;
    PdfDictionary fromRefernceHolder1 = this.GetDictionaryFromRefernceHolder(appearanceStream["Resources"]);
    if (fromRefernceHolder1 != null && fromRefernceHolder1.ContainsKey("ExtGState"))
    {
      PdfDictionary fromRefernceHolder2 = this.GetDictionaryFromRefernceHolder(fromRefernceHolder1["ExtGState"]);
      if (fromRefernceHolder2 != null && fromRefernceHolder2.Keys.Count > 0)
      {
        foreach (PdfName key in new List<PdfName>((IEnumerable<PdfName>) fromRefernceHolder2.Keys))
        {
          PdfDictionary fromRefernceHolder3 = this.GetDictionaryFromRefernceHolder(fromRefernceHolder2[key]);
          if (fromRefernceHolder3 != null)
          {
            if (fromRefernceHolder3.ContainsKey("CA"))
            {
              fromRefernceHolder3["CA"] = (IPdfPrimitive) new PdfNumber(1);
              fromRefernceHolder3.Modify();
            }
            if (fromRefernceHolder3.ContainsKey("ca"))
            {
              fromRefernceHolder3["ca"] = (IPdfPrimitive) new PdfNumber(1);
              fromRefernceHolder3.Modify();
            }
            if (fromRefernceHolder3.ContainsKey("BM") && (object) (fromRefernceHolder3["BM"] as PdfName) != null && ((fromRefernceHolder3["BM"] as PdfName).Value != "Normal" || (fromRefernceHolder3["BM"] as PdfName).Value != "Compatible"))
            {
              fromRefernceHolder3["BM"] = (IPdfPrimitive) new PdfName("Normal");
              fromRefernceHolder3.Modify();
            }
          }
        }
      }
    }
    if (fromRefernceHolder1 != null && fromRefernceHolder1.ContainsKey("XObject"))
    {
      PdfDictionary fromRefernceHolder4 = this.GetDictionaryFromRefernceHolder(fromRefernceHolder1["XObject"]);
      if (fromRefernceHolder4 != null && fromRefernceHolder4.Keys.Count > 0)
      {
        foreach (PdfName key in new List<PdfName>((IEnumerable<PdfName>) fromRefernceHolder4.Keys))
        {
          PdfStream fromRefernceHolder5 = this.GetStreamFromRefernceHolder(fromRefernceHolder4[key]);
          if (this.PdfALevel == PdfConformanceLevel.Pdf_A1B && fromRefernceHolder5 != null)
            this.RemoveSMaskFromPattern((PdfDictionary) fromRefernceHolder5);
          if (fromRefernceHolder5 != null && fromRefernceHolder5.ContainsKey("Group"))
          {
            PdfDictionary fromRefernceHolder6 = this.GetDictionaryFromRefernceHolder(fromRefernceHolder5["Group"]);
            if (fromRefernceHolder6 != null && fromRefernceHolder6.ContainsKey("S") && (object) (fromRefernceHolder6["S"] as PdfName) != null && (fromRefernceHolder6["S"] as PdfName).Value == "Transparency")
            {
              fromRefernceHolder5.Remove("Group");
              fromRefernceHolder5.Modify();
            }
          }
        }
      }
    }
    if (fromRefernceHolder1 == null || !fromRefernceHolder1.ContainsKey("Font"))
      return;
    this.EmbedCompleteFonts((PdfResources) null, this.GetDictionaryFromRefernceHolder(fromRefernceHolder1["Font"]), true);
  }

  private void RemoveLZWCompression(PdfArray lzwCompressionFilterArray, PdfStream lzwStream)
  {
    foreach (IPdfPrimitive element in lzwCompressionFilterArray.Elements)
    {
      if ((object) (element as PdfName) != null && ((element as PdfName).Value == "LZWDecode" || (element as PdfName).Value == "JPXDecode"))
        this.RemoveLZWCompression(lzwStream, (element as PdfName).Value);
    }
  }

  private void RemoveLZWCompression(PdfStream lzwStream, string FilterType)
  {
    byte[] data = FilterType == "LZWDecode" || FilterType == "LZW" ? new PdfLzwCompressor().Decompress(lzwStream.Data) : new DefaultCompressor().Decompress(lzwStream.Data);
    PdfZlibCompressor pdfZlibCompressor = new PdfZlibCompressor();
    lzwStream.Data = pdfZlibCompressor.Compress(data);
    lzwStream["Filter"] = (IPdfPrimitive) new PdfName("FlateDecode");
    lzwStream["Length"] = (IPdfPrimitive) new PdfNumber(lzwStream.Data.Length);
    lzwStream.Modify();
  }

  private void CompressionConsideration(PdfPageBase lPage)
  {
    if (!lPage.Dictionary.ContainsKey("Thumb"))
      return;
    PdfStream fromRefernceHolder1 = this.GetStreamFromRefernceHolder(lPage.Dictionary["Thumb"]);
    if (fromRefernceHolder1 == null)
      return;
    if (fromRefernceHolder1.ContainsKey("Filter"))
    {
      if ((object) (fromRefernceHolder1["Filter"] as PdfName) != null && (fromRefernceHolder1["Filter"] as PdfName).Value == "LZWDecode")
        this.RemoveLZWCompression(fromRefernceHolder1, "LZWDecode");
      else if (fromRefernceHolder1["Filter"] is PdfArray)
        this.RemoveLZWCompression(fromRefernceHolder1["Filter"] as PdfArray, fromRefernceHolder1);
    }
    if (!fromRefernceHolder1.ContainsKey("ColorSpace") || (object) (fromRefernceHolder1["ColorSpace"] as PdfReferenceHolder) == null && this.GetArrayFromReferenceHolder(fromRefernceHolder1["ColorSpace"]) == null)
      return;
    PdfArray fromReferenceHolder = this.GetArrayFromReferenceHolder(fromRefernceHolder1["ColorSpace"]);
    if (fromReferenceHolder == null || fromReferenceHolder.Elements.Count <= 0 || (object) (fromReferenceHolder[fromReferenceHolder.Elements.Count - 1] as PdfReferenceHolder) == null)
      return;
    PdfStream fromRefernceHolder2 = this.GetStreamFromRefernceHolder(fromReferenceHolder[fromReferenceHolder.Elements.Count - 1]);
    if (fromRefernceHolder2 == null || !fromRefernceHolder2.ContainsKey("Filter"))
      return;
    if ((object) (fromRefernceHolder2["Filter"] as PdfName) != null && (fromRefernceHolder2["Filter"] as PdfName).Value == "LZWDecode")
    {
      this.RemoveLZWCompression(fromRefernceHolder2, "LZWDecode");
    }
    else
    {
      if (!(fromRefernceHolder2["Filter"] is PdfArray))
        return;
      this.RemoveLZWCompression(fromRefernceHolder2["Filter"] as PdfArray, fromRefernceHolder2);
    }
  }

  private void RemoveIndirectCMYKReference(
    List<IPdfPrimitive> elements,
    IPdfPrimitive color,
    PdfResources resources,
    PdfPageBase lPage)
  {
    int index = elements.IndexOf((IPdfPrimitive) new PdfName("DeviceCMYK"));
    elements.Remove((IPdfPrimitive) new PdfName("DeviceCMYK"));
    elements.Insert(index, (IPdfPrimitive) new PdfName("DeviceRGB"));
    if ((object) (color as PdfReferenceHolder) != null)
      ((color as PdfReferenceHolder).Object as PdfArray).MarkChanged();
    else
      (color as PdfArray).MarkChanged();
    PdfDictionary fromRefernceHolder = this.GetDictionaryFromRefernceHolder(elements[index + 1]);
    if (fromRefernceHolder.ContainsKey("C0") && fromRefernceHolder["C0"] != null)
    {
      List<IPdfPrimitive> elements1 = (fromRefernceHolder["C0"] as PdfArray).Elements;
      float[] equivalentRgb = this.GetEquivalentRGB((elements1[0] as PdfNumber).FloatValue, (elements1[1] as PdfNumber).FloatValue, (elements1[2] as PdfNumber).FloatValue, (elements1[3] as PdfNumber).FloatValue);
      fromRefernceHolder.Remove("C0");
      fromRefernceHolder["C0"] = (IPdfPrimitive) new PdfArray(equivalentRgb);
      fromRefernceHolder.Modify();
    }
    if (fromRefernceHolder.ContainsKey("C1") && fromRefernceHolder["C1"] != null)
    {
      List<IPdfPrimitive> elements2 = (fromRefernceHolder["C1"] as PdfArray).Elements;
      float[] equivalentRgb = this.GetEquivalentRGB((elements2[0] as PdfNumber).FloatValue, (elements2[1] as PdfNumber).FloatValue, (elements2[2] as PdfNumber).FloatValue, (elements2[3] as PdfNumber).FloatValue);
      fromRefernceHolder.Remove("C1");
      fromRefernceHolder["C1"] = (IPdfPrimitive) new PdfArray(equivalentRgb);
      fromRefernceHolder.Modify();
    }
    if (fromRefernceHolder.ContainsKey("Range") && fromRefernceHolder["Range"] != null)
    {
      List<IPdfPrimitive> elements3 = (fromRefernceHolder["Range"] as PdfArray).Elements;
      float[] array = new float[6]
      {
        (elements3[0] as PdfNumber).FloatValue,
        (elements3[1] as PdfNumber).FloatValue,
        (elements3[2] as PdfNumber).FloatValue,
        (elements3[3] as PdfNumber).FloatValue,
        (elements3[4] as PdfNumber).FloatValue,
        (elements3[5] as PdfNumber).FloatValue
      };
      fromRefernceHolder.Remove("Range");
      fromRefernceHolder["Range"] = (IPdfPrimitive) new PdfArray(array);
      fromRefernceHolder.Modify();
    }
    if (!fromRefernceHolder.ContainsKey("Decode") || fromRefernceHolder["Decode"] == null)
      return;
    List<IPdfPrimitive> elements4 = (fromRefernceHolder["Decode"] as PdfArray).Elements;
    float[] array1 = new float[6]
    {
      (elements4[0] as PdfNumber).FloatValue,
      (elements4[1] as PdfNumber).FloatValue,
      (elements4[2] as PdfNumber).FloatValue,
      (elements4[3] as PdfNumber).FloatValue,
      (elements4[4] as PdfNumber).FloatValue,
      (elements4[5] as PdfNumber).FloatValue
    };
    fromRefernceHolder.Remove("Decode");
    fromRefernceHolder["Decode"] = (IPdfPrimitive) new PdfArray(array1);
    fromRefernceHolder.Modify();
  }

  private void RemoveTransparencyGroup(PdfPageBase lPage)
  {
    if (!lPage.Dictionary.ContainsKey("Group"))
      return;
    lPage.Dictionary.Remove("Group");
    lPage.Dictionary.Modify();
  }

  private void RemoveSMask(PdfReferenceHolder imageObject, PdfResources resources, string key)
  {
    while (imageObject != (PdfReferenceHolder) null && (object) (imageObject.Object as PdfReferenceHolder) != null)
      imageObject = imageObject.Object as PdfReferenceHolder;
    if (!(imageObject.Object is PdfStream))
      return;
    PdfStream lzwStream = imageObject.Object as PdfStream;
    if ((lzwStream["Subtype"] as PdfName).Value == "Image")
    {
      if (this.PdfALevel == PdfConformanceLevel.Pdf_A1B)
      {
        if (lzwStream.ContainsKey("SMask"))
        {
          for (int index = 0; index < this.ldoc.Pages.Count; ++index)
          {
            PdfPageBase page = this.ldoc.Pages[index];
            Image[] images = page.ExtractImages();
            for (int imageIndex = 0; imageIndex < images.Length; ++imageIndex)
            {
              PdfBitmap image = new PdfBitmap((Image) this.ReplaceTransparency(new Bitmap(images[imageIndex]), Color.White));
              page.ReplaceImage(imageIndex, (PdfImage) image);
            }
          }
        }
        if (lzwStream.ContainsKey("Filter"))
        {
          if ((object) (lzwStream["Filter"] as PdfName) != null && ((lzwStream["Filter"] as PdfName).Value == "JPXDecode" || (lzwStream["Filter"] as PdfName).Value == "LZWDecode"))
            this.RemoveLZWCompression(lzwStream, (lzwStream["Filter"] as PdfName).Value);
          else if (lzwStream["Filter"] is PdfArray)
            this.RemoveLZWCompression(lzwStream["Filter"] as PdfArray, lzwStream);
        }
        else
          lzwStream.Compress = true;
      }
      if (lzwStream.ContainsKey("Interpolate"))
      {
        this.GetPdfBooleanFromReferenceHolder(lzwStream["Interpolate"]).Value = false;
        lzwStream.Modify();
      }
      if (!lzwStream.ContainsKey("Mask"))
        return;
      PdfReferenceHolder pdfReferenceHolder = lzwStream["Mask"] as PdfReferenceHolder;
      if (!(pdfReferenceHolder != (PdfReferenceHolder) null) || !(pdfReferenceHolder.Object is PdfDictionary pdfDictionary) || !pdfDictionary.ContainsKey("Interpolate"))
        return;
      this.GetPdfBooleanFromReferenceHolder(pdfDictionary["Interpolate"]).Value = false;
      pdfDictionary.Modify();
      lzwStream.Modify();
    }
    else
    {
      if (!((lzwStream["Subtype"] as PdfName).Value == "Form"))
        return;
      if (lzwStream.ContainsKey("Group"))
      {
        lzwStream.Remove("Group");
        lzwStream.Modify();
      }
      if (!lzwStream.ContainsKey("Resources"))
        return;
      PdfDictionary fromRefernceHolder1 = this.GetDictionaryFromRefernceHolder(lzwStream["Resources"]);
      if (fromRefernceHolder1.ContainsKey("Reference"))
      {
        fromRefernceHolder1.Remove("Reference");
        fromRefernceHolder1.Modify();
      }
      if (fromRefernceHolder1.ContainsKey("Ref"))
      {
        fromRefernceHolder1.Remove("Ref");
        fromRefernceHolder1.Modify();
      }
      if (fromRefernceHolder1.ContainsKey("Font"))
      {
        PdfDictionary fromRefernceHolder2 = this.GetDictionaryFromRefernceHolder(fromRefernceHolder1["Font"]);
        foreach (PdfName key1 in new List<PdfName>((IEnumerable<PdfName>) fromRefernceHolder2.Keys))
        {
          if (resources.ContainsKey("Font"))
          {
            PdfDictionary fromRefernceHolder3 = this.GetDictionaryFromRefernceHolder(resources["Font"]);
            if (fromRefernceHolder3.ContainsKey(key1))
            {
              PdfReferenceHolder pdfReferenceHolder1 = fromRefernceHolder2[key1.Value] as PdfReferenceHolder;
              PdfReferenceHolder pdfReferenceHolder2 = fromRefernceHolder3[key1.Value] as PdfReferenceHolder;
              if (pdfReferenceHolder1 != (PdfReferenceHolder) null && pdfReferenceHolder2 != (PdfReferenceHolder) null && pdfReferenceHolder1.Reference != (PdfReference) null && pdfReferenceHolder2.Reference != (PdfReference) null && pdfReferenceHolder1.Reference.ObjNum == pdfReferenceHolder2.Reference.ObjNum)
              {
                fromRefernceHolder2.Remove(key1);
                fromRefernceHolder2.Items.Add(key1, fromRefernceHolder3[key1]);
                fromRefernceHolder2.Modify();
              }
            }
          }
        }
        this.EmbedCompleteFonts((PdfResources) null, fromRefernceHolder2, true);
      }
      if (!fromRefernceHolder1.ContainsKey("XObject"))
        return;
      PdfDictionary fromRefernceHolder4 = this.GetDictionaryFromRefernceHolder(fromRefernceHolder1["XObject"]);
      foreach (PdfName key2 in new List<PdfName>((IEnumerable<PdfName>) fromRefernceHolder4.Keys))
      {
        if ((object) (fromRefernceHolder4[key2] as PdfReferenceHolder) != null)
          this.RemoveSMask(fromRefernceHolder4[key2] as PdfReferenceHolder, resources, key);
      }
    }
  }

  public Bitmap ReplaceTransparency(Bitmap bitmap, Color background)
  {
    Bitmap bitmap1 = new Bitmap(bitmap.Size.Width, bitmap.Size.Height, PixelFormat.Format24bppRgb);
    System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage((Image) bitmap1);
    graphics.Clear(background);
    graphics.CompositingMode = CompositingMode.SourceOver;
    graphics.InterpolationMode = InterpolationMode.High;
    graphics.DrawImage((Image) bitmap, 0, 0);
    return bitmap1;
  }

  public Stream CopyStream(MemoryStream input)
  {
    byte[] buffer = new byte[32768 /*0x8000*/];
    Stream stream = (Stream) new MemoryStream(input.GetBuffer());
    int count;
    while ((count = input.Read(buffer, 0, buffer.Length)) > 0)
      stream.Write(buffer, 0, count);
    return stream;
  }

  private float[] ConvertTransparencyToRGB(float r, float g, float b, float opacity)
  {
    if ((double) opacity != -1.0)
    {
      r *= opacity;
      g *= opacity;
      b *= opacity;
    }
    return new float[3]
    {
      r / (float) byte.MaxValue,
      g / (float) byte.MaxValue,
      b / (float) byte.MaxValue
    };
  }

  private Colorspace GetColorspace(string[] colorSpaceElement, PdfResources resources)
  {
    Colorspace colorspace = (Colorspace) null;
    if (Colorspace.IsColorSpace(colorSpaceElement[0].Replace("/", "")))
    {
      colorspace = Colorspace.CreateColorSpace(colorSpaceElement[0].Replace("/", ""));
      if (colorSpaceElement[0].Replace("/", "") == "Pattern")
      {
        PdfDictionary fromRefernceHolder = this.GetDictionaryFromRefernceHolder(resources[colorSpaceElement[0].Replace("/", "")]);
        if (fromRefernceHolder != null && fromRefernceHolder.Values.Count > 0)
        {
          foreach (IPdfPrimitive primitive in fromRefernceHolder.Values)
            this.RemoveSMaskFromPattern(this.GetDictionaryFromRefernceHolder(primitive));
        }
      }
    }
    else if (this.colorspace is Separation && !resources.ContainsKey("ColorSpace"))
      colorspace = (Colorspace) new Separation();
    else if (this.GetDictionaryFromRefernceHolder(resources["ColorSpace"]).ContainsKey(colorSpaceElement[0].Replace("/", "")))
    {
      IPdfPrimitive pdfPrimitive = this.GetDictionaryFromRefernceHolder(resources["ColorSpace"])[colorSpaceElement[0].Replace("/", "")];
      if ((object) (pdfPrimitive as PdfReferenceHolder) != null)
      {
        PdfArray pdfArray = (pdfPrimitive as PdfReferenceHolder).Object as PdfArray;
        if ((object) (pdfArray.Elements[0] as PdfName) != null)
        {
          if ((pdfArray.Elements[0] as PdfName).Value == "DeviceRGB")
            colorspace = (Colorspace) new DeviceRGB();
          else if ((pdfArray.Elements[0] as PdfName).Value == "DeviceGray")
            colorspace = (Colorspace) new DeviceGray();
          else if ((pdfArray.Elements[0] as PdfName).Value == "DeviceCMYK")
            colorspace = (Colorspace) new DeviceCMYK();
          else if ((pdfArray.Elements[0] as PdfName).Value == "CalGray")
            colorspace = (Colorspace) new CalGray();
          else if ((pdfArray.Elements[0] as PdfName).Value == "CalRGB")
            colorspace = (Colorspace) new CalRGB();
          else if ((pdfArray.Elements[0] as PdfName).Value == "Lab")
            colorspace = (Colorspace) new LabColor();
          if ((pdfArray.Elements[0] as PdfName).Value == "ICCBased")
            colorspace = (Colorspace) new ICCBased();
          else if ((pdfArray.Elements[0] as PdfName).Value == "Separation")
            colorspace = (Colorspace) new Separation();
          else if ((pdfArray.Elements[0] as PdfName).Value == "DeviceN")
            colorspace = (Colorspace) new DeviceN();
          else if ((pdfArray.Elements[0] as PdfName).Value == "Pattern")
            colorspace = (Colorspace) new Pattern();
          else if ((pdfArray.Elements[0] as PdfName).Value == "Indexed")
            colorspace = (Colorspace) new Indexed();
        }
      }
    }
    return colorspace ?? (Colorspace) null;
  }

  private void ResetPageResources()
  {
    this.isPdfPage = false;
    this.appliedSCColorSpace = "";
    this.appliedSCNColorSpace = "";
    this.nonStrokingOpacity = -1f;
    this.strokingOpacity = -1f;
    this.hasNonStroking = false;
    this.hasStroking = false;
  }

  private float[] GetEquivalentRGB(float cyan, float magenta, float yellow, float black)
  {
    return new float[3]
    {
      (float) ((1.0 - (double) cyan) * (1.0 - (double) black)),
      (float) ((1.0 - (double) magenta) * (1.0 - (double) black)),
      (float) ((1.0 - (double) yellow) * (1.0 - (double) black))
    };
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

  private void ReplaceCMYKColorSpace(PdfPageBase lPage)
  {
    PdfResources resources = lPage.GetResources();
    if (resources != null && resources.ContainsKey("ColorSpace"))
    {
      PdfDictionary fromRefernceHolder = this.GetDictionaryFromRefernceHolder(resources["ColorSpace"]);
      if (fromRefernceHolder != null && fromRefernceHolder.Keys.Count > 0)
      {
        foreach (PdfName key in new List<PdfName>((IEnumerable<PdfName>) fromRefernceHolder.Keys))
        {
          IPdfPrimitive pdfPrimitive = fromRefernceHolder[key];
          List<IPdfPrimitive> pdfPrimitiveList = new List<IPdfPrimitive>();
          if ((object) (pdfPrimitive as PdfReferenceHolder) != null && (pdfPrimitive as PdfReferenceHolder).Object is PdfArray)
            pdfPrimitiveList = ((pdfPrimitive as PdfReferenceHolder).Object as PdfArray).Elements;
          else if (pdfPrimitive is PdfArray)
            pdfPrimitiveList = (pdfPrimitive as PdfArray).Elements;
          if (pdfPrimitiveList != null && pdfPrimitiveList.Count > 0 && (pdfPrimitiveList[0] as PdfName).Value == "Separation" && pdfPrimitiveList.Contains((IPdfPrimitive) new PdfName("DeviceCMYK")))
          {
            PdfReferenceHolder pdfReferenceHolder = new PdfReferenceHolder((IPdfWrapper) new PdfICCColorSpace());
            fromRefernceHolder[new PdfName("DefaultCMYK")] = (IPdfPrimitive) pdfReferenceHolder;
          }
        }
      }
    }
    if (resources.ContainsKey("Pattern"))
    {
      PdfDictionary fromRefernceHolder1 = this.GetDictionaryFromRefernceHolder(resources["Pattern"]);
      foreach (PdfName key in fromRefernceHolder1.Keys)
      {
        PdfDictionary fromRefernceHolder2 = this.GetDictionaryFromRefernceHolder(fromRefernceHolder1[key]);
        if (fromRefernceHolder2.ContainsKey("PatternType") && (fromRefernceHolder2["PatternType"] as PdfNumber).IntValue == 2)
          this.OptimizeShading(fromRefernceHolder2["Shading"]);
      }
    }
    if (!resources.ContainsKey("Shading"))
      return;
    PdfDictionary fromRefernceHolder3 = this.GetDictionaryFromRefernceHolder(resources["Shading"]);
    foreach (PdfName key in fromRefernceHolder3.Keys)
      this.OptimizeShading(fromRefernceHolder3[key]);
  }

  private void OptimizeShading(IPdfPrimitive shade)
  {
    PdfDictionary fromRefernceHolder1 = this.GetDictionaryFromRefernceHolder(shade);
    if (fromRefernceHolder1 == null)
      return;
    PdfName pdfName = fromRefernceHolder1["ColorSpace"] as PdfName;
    if (!(pdfName != (PdfName) null) || !(pdfName.Value == "DeviceCMYK") || !this.ValidateShadingType(fromRefernceHolder1) || !fromRefernceHolder1.ContainsKey("Function") || !this.ValidateFunctionType(fromRefernceHolder1))
      return;
    (fromRefernceHolder1["ColorSpace"] as PdfName).Value = "DeviceRGB";
    if (fromRefernceHolder1.ContainsKey("Background"))
    {
      float[] rgbFromCmyk = this.GetRGBFromCMYK(this.GetArrayFromReferenceHolder(fromRefernceHolder1["Background"]));
      fromRefernceHolder1["Background"] = (IPdfPrimitive) new PdfArray(rgbFromCmyk);
    }
    PdfStream fromRefernceHolder2 = this.GetStreamFromRefernceHolder(fromRefernceHolder1["Function"]);
    if (fromRefernceHolder2 != null && (fromRefernceHolder2["FunctionType"] as PdfNumber).IntValue == 0)
    {
      this.ReplaceCMYKFromType0Function(fromRefernceHolder2);
    }
    else
    {
      PdfDictionary fromRefernceHolder3 = this.GetDictionaryFromRefernceHolder(fromRefernceHolder1["Function"]);
      if (fromRefernceHolder3 != null && (fromRefernceHolder3["FunctionType"] as PdfNumber).IntValue == 3)
      {
        this.ReplaceCMYKFromType3Function(this.GetArrayFromReferenceHolder(fromRefernceHolder3["Functions"]));
      }
      else
      {
        if (fromRefernceHolder3 == null || (fromRefernceHolder3["FunctionType"] as PdfNumber).IntValue != 2)
          return;
        this.ReplaceCMYKFromType2Function(fromRefernceHolder3);
      }
    }
  }

  private void ReplaceCMYKFromType0Function(PdfStream function)
  {
    function.Decompress();
    if (function.Data.Length % 4 != 0)
      return;
    int[] numArray1 = new int[function.Data.Length];
    int index1 = 0;
    foreach (byte num in function.Data)
    {
      numArray1[index1] = (int) num;
      ++index1;
      if (index1 == numArray1.Length)
        break;
    }
    byte[] numArray2 = new byte[numArray1.Length / 4 * 3];
    for (int index2 = 0; index2 < numArray1.Length / 4; ++index2)
    {
      numArray2[index2 * 3] = (byte) ((int) byte.MaxValue * (1 - numArray1[index2 * 4] / (int) byte.MaxValue) * (1 - numArray1[index2 * 4 + 3] / (int) byte.MaxValue));
      numArray2[index2 * 3 + 1] = (byte) ((int) byte.MaxValue * (1 - numArray1[index2 * 4 + 1] / (int) byte.MaxValue) * (1 - numArray1[index2 * 4 + 3] / (int) byte.MaxValue));
      numArray2[index2 * 3 + 2] = (byte) ((int) byte.MaxValue * (1 - numArray1[index2 * 4 + 2] / (int) byte.MaxValue) * (1 - numArray1[index2 * 4 + 3] / (int) byte.MaxValue));
    }
    function.Data = numArray2;
    function.Compress = true;
    function.Modify();
    if (!function.ContainsKey("Range"))
      return;
    function["Range"] = (IPdfPrimitive) new PdfArray(new int[6]
    {
      0,
      1,
      0,
      1,
      0,
      1
    });
  }

  private void ReplaceCMYKFromType2Function(PdfDictionary function)
  {
    if (function.ContainsKey("Range"))
      function["Range"] = (IPdfPrimitive) new PdfArray(new int[6]
      {
        0,
        1,
        0,
        1,
        0,
        1
      });
    if (function.ContainsKey("C0"))
    {
      float[] rgbFromCmyk = this.GetRGBFromCMYK(this.GetArrayFromReferenceHolder(function["C0"]));
      function["C0"] = (IPdfPrimitive) new PdfArray(rgbFromCmyk);
    }
    if (!function.ContainsKey("C1"))
      return;
    float[] rgbFromCmyk1 = this.GetRGBFromCMYK(this.GetArrayFromReferenceHolder(function["C1"]));
    function["C1"] = (IPdfPrimitive) new PdfArray(rgbFromCmyk1);
  }

  private void ReplaceCMYKFromType3Function(PdfArray functions)
  {
    foreach (IPdfPrimitive function in functions)
    {
      PdfStream fromRefernceHolder1 = this.GetStreamFromRefernceHolder(function);
      if (fromRefernceHolder1 != null && (fromRefernceHolder1["FunctionType"] as PdfNumber).IntValue == 0)
      {
        this.ReplaceCMYKFromType0Function(fromRefernceHolder1);
      }
      else
      {
        PdfDictionary fromRefernceHolder2 = this.GetDictionaryFromRefernceHolder(function);
        if (fromRefernceHolder2 != null && (fromRefernceHolder2["FunctionType"] as PdfNumber).IntValue == 2)
          this.ReplaceCMYKFromType2Function(fromRefernceHolder2);
        else if (fromRefernceHolder2 != null && (fromRefernceHolder2["FunctionType"] as PdfNumber).IntValue == 3)
          this.ReplaceCMYKFromType3Function(this.GetArrayFromReferenceHolder(fromRefernceHolder2["Functions"]));
      }
    }
  }

  private float[] GetRGBFromCMYK(PdfArray colorArray)
  {
    float[] numArray = new float[4];
    int index = 0;
    foreach (PdfNumber color in colorArray)
    {
      numArray[index] = color.FloatValue;
      ++index;
      if (index == 4)
        break;
    }
    return new float[3]
    {
      (float) ((double) byte.MaxValue * (1.0 - (double) numArray[0]) * (1.0 - (double) numArray[3])),
      (float) ((double) byte.MaxValue * (1.0 - (double) numArray[1]) * (1.0 - (double) numArray[3])),
      (float) ((double) byte.MaxValue * (1.0 - (double) numArray[2]) * (1.0 - (double) numArray[3]))
    };
  }

  private bool ValidateFunctionType(PdfDictionary shadingDictionary)
  {
    PdfStream fromRefernceHolder1 = this.GetStreamFromRefernceHolder(shadingDictionary["Function"]);
    if (fromRefernceHolder1 != null && (fromRefernceHolder1["FunctionType"] as PdfNumber).IntValue == 4)
      return false;
    if (fromRefernceHolder1 == null)
    {
      PdfDictionary fromRefernceHolder2 = this.GetDictionaryFromRefernceHolder(shadingDictionary["Function"]);
      if (fromRefernceHolder2 != null && (fromRefernceHolder2["FunctionType"] as PdfNumber).IntValue == 3)
      {
        PdfStream fromRefernceHolder3 = this.GetStreamFromRefernceHolder(this.GetArrayFromReferenceHolder(fromRefernceHolder2["Functions"]).Elements[0]);
        if (fromRefernceHolder3 != null && (fromRefernceHolder3["FunctionType"] as PdfNumber).IntValue == 4)
          return false;
      }
    }
    return true;
  }

  private bool ValidateShadingType(PdfDictionary shadingDictionary)
  {
    return (shadingDictionary["ShadingType"] as PdfNumber).IntValue == 1 || (shadingDictionary["ShadingType"] as PdfNumber).IntValue == 2 || (shadingDictionary["ShadingType"] as PdfNumber).IntValue == 3;
  }

  private FontStyle GetDeviceFontStyle(PdfFontStyle style)
  {
    FontStyle deviceFontStyle = FontStyle.Regular;
    if ((style & PdfFontStyle.Bold) > PdfFontStyle.Regular)
      deviceFontStyle |= FontStyle.Bold;
    if ((style & PdfFontStyle.Italic) > PdfFontStyle.Regular)
      deviceFontStyle |= FontStyle.Italic;
    if ((style & PdfFontStyle.Underline) > PdfFontStyle.Regular)
      deviceFontStyle |= FontStyle.Underline;
    if ((style & PdfFontStyle.Strikeout) > PdfFontStyle.Regular)
      deviceFontStyle |= FontStyle.Strikeout;
    return deviceFontStyle;
  }

  private PdfFontStyle GetFontStyle(string fontFamilyString)
  {
    int num = fontFamilyString.IndexOf("-");
    PdfFontStyle fontStyle = PdfFontStyle.Regular;
    if (num >= 0)
    {
      switch (fontFamilyString.Substring(num + 1, fontFamilyString.Length - num - 1))
      {
        case "Italic":
        case "Oblique":
          fontStyle = PdfFontStyle.Italic;
          break;
        case "Bold":
          fontStyle = PdfFontStyle.Bold;
          break;
        case "BoldItalic":
        case "BoldOblique":
          fontStyle = PdfFontStyle.Bold | PdfFontStyle.Italic;
          break;
      }
    }
    return fontStyle;
  }

  private PdfFontMetrics CreateFont(PdfDictionary fontDictionary, float height, PdfName baseFont)
  {
    PdfFontMetrics font1 = new PdfFontMetrics();
    if (fontDictionary.ContainsKey("FontDescriptor"))
    {
      PdfDictionary pdfDictionary = (fontDictionary["FontDescriptor"] as PdfReferenceHolder).Object as PdfDictionary;
      font1.Ascent = (float) (pdfDictionary["Ascent"] as PdfNumber).IntValue;
      font1.Descent = (float) (pdfDictionary["Descent"] as PdfNumber).IntValue;
      font1.Size = height;
      font1.Height = font1.Ascent - font1.Descent;
      font1.PostScriptName = baseFont.Value;
      if (fontDictionary.ContainsKey("Widths"))
      {
        if (!(fontDictionary["Widths"] is PdfArray font2))
          font2 = (fontDictionary["Widths"] as PdfReferenceHolder).Object as PdfArray;
        int[] widths = new int[font2.Count];
        for (int index = 0; index < font2.Count; ++index)
          widths[index] = (font2[index] as PdfNumber).IntValue;
        font1.WidthTable = (WidthTable) new StandardWidthTable(widths);
      }
      font1.Name = baseFont.Value;
    }
    return font1;
  }

  private PdfFontFamily GetFontFamily(string fontFamilyString)
  {
    int length = fontFamilyString.IndexOf("-");
    string str = fontFamilyString;
    if (length >= 0)
      str = fontFamilyString.Substring(0, length);
    if (str == "Times")
      return PdfFontFamily.TimesRoman;
    try
    {
      return (PdfFontFamily) Enum.Parse(typeof (PdfFontFamily), str, true);
    }
    catch (Exception ex)
    {
      this.isPdfFontFamily = false;
      return PdfFontFamily.Helvetica;
    }
  }

  internal void EmbedCompleteFonts(PdfPageBase page, PdfLoadedDocument document)
  {
    this.EmbedCompleteFonts(page.GetResources(), this.GetDictionaryFromRefernceHolder(page.GetResources()["Font"]), false);
  }

  internal void EmbedCompleteFonts(
    PdfResources pageResources,
    PdfDictionary usedFonts,
    bool isForm)
  {
    if (usedFonts == null)
      return;
    foreach (PdfName key in new List<PdfName>((IEnumerable<PdfName>) usedFonts.Keys))
      this.RepairFontDictionary(pageResources, usedFonts, isForm, key);
  }

  private void RepairFontDictionary(
    PdfResources pageResources,
    PdfDictionary usedFonts,
    bool isForm,
    PdfName key)
  {
    PdfDictionary fromRefernceHolder1 = this.GetDictionaryFromRefernceHolder(usedFonts[key]);
    bool flag1 = true;
    this.isPdfFontFamily = true;
    if (fromRefernceHolder1.ContainsKey("Subtype"))
    {
      PdfName pdfName1 = PdfCrossTable.Dereference(fromRefernceHolder1["Subtype"]) as PdfName;
      float num1 = 12f;
      if (pdfName1 != (PdfName) null && pdfName1.Value == "Type1")
      {
        PdfDictionary fontDescriptor = (PdfDictionary) null;
        PdfDictionary descendantFont = (PdfDictionary) null;
        this.GetFontInternals(fromRefernceHolder1, out fontDescriptor, out descendantFont);
        if (fontDescriptor == null || !fontDescriptor.ContainsKey("FontFile") && !fontDescriptor.ContainsKey("FontFile2") && !fontDescriptor.ContainsKey("FontFile3"))
        {
          if (fromRefernceHolder1.ContainsKey("BaseFont"))
          {
            PdfName pdfName2 = PdfCrossTable.Dereference(fromRefernceHolder1["BaseFont"]) as PdfName;
            PdfFontStyle fontStyle = this.GetFontStyle(pdfName2.Value);
            string familyName;
            switch (this.GetFontFamily(pdfName2.Value))
            {
              case PdfFontFamily.Helvetica:
                familyName = "Helvetica";
                break;
              case PdfFontFamily.Courier:
                familyName = "Courier New";
                break;
              case PdfFontFamily.TimesRoman:
                familyName = "Times New Roman";
                break;
              case PdfFontFamily.Symbol:
                familyName = "Symbol";
                break;
              case PdfFontFamily.ZapfDingbats:
                familyName = "ZapfDingbats";
                break;
              default:
                familyName = "Arial";
                break;
            }
            if (familyName == "Helvetica" && (pdfName2.Value.Contains("Narrow") || fontStyle == PdfFontStyle.Italic || fontStyle == (PdfFontStyle.Bold | PdfFontStyle.Italic)))
            {
              if (pdfName2.Value.Contains("Narrow"))
              {
                familyName = "Helvetica Narrow";
                if (pdfName2.Value.Contains("Bold"))
                  fontStyle |= PdfFontStyle.Bold;
              }
              else
                familyName = "Arial";
            }
            if (pdfName2.Value != familyName && !this.isPdfFontFamily)
              flag1 = false;
            if (flag1)
            {
              if (fromRefernceHolder1.ContainsKey("Encoding"))
                PdfCrossTable.Dereference(fromRefernceHolder1["Encoding"]);
              int num2 = int.MinValue;
              if (fontDescriptor != null && fontDescriptor.ContainsKey("Flags") && PdfCrossTable.Dereference(fontDescriptor["Flags"]) is PdfNumber pdfNumber)
                num2 = pdfNumber.IntValue;
              bool flag2 = false;
              if (pageResources != null && this.recordList.ContainsKey(pageResources))
              {
                flag2 = true;
                string str = string.Empty;
                PdfRecordCollection record1 = this.recordList[pageResources];
                for (int index = 0; index < record1.RecordCollection.Count; ++index)
                {
                  PdfRecord record2 = record1.RecordCollection[index];
                  if (record2.OperatorName == "Tf")
                    str = record2.Operands[0].TrimStart('/');
                  if (record2.OperatorName == "TJ" && str == key.Value)
                  {
                    flag2 = false;
                    break;
                  }
                }
              }
              PdfTrueTypeFont pdfTrueTypeFont;
              if (this.UsedFonts.Contains(key.Value) && !this.ReplaceFonts.ContainsKey(key.Value) && num2 != 4 && num2 != 32 /*0x20*/ && flag2)
              {
                this.OldFonts.Add(key.Value, fromRefernceHolder1);
                pdfTrueTypeFont = new PdfTrueTypeFont(new Font(familyName, num1, this.GetDeviceFontStyle(fontStyle), GraphicsUnit.Point), true, false, true);
                this.ReplaceFonts.Add(key.Value, pdfTrueTypeFont);
              }
              else
                pdfTrueTypeFont = new PdfTrueTypeFont(new Font(familyName, num1, this.GetDeviceFontStyle(fontStyle), GraphicsUnit.Point), true, true, true, num1, true);
              usedFonts.Remove(key);
              if (!isForm)
              {
                pageResources.Add((PdfFont) pdfTrueTypeFont, key);
                if (this.UsedFonts.Contains(key.Value))
                {
                  if (this.ReplaceFonts.ContainsKey(key.Value))
                    this.ReplaceFonts[key.Value] = pdfTrueTypeFont;
                  else
                    this.ReplaceFonts.Add(key.Value, pdfTrueTypeFont);
                }
              }
              else
              {
                PdfReferenceHolder pdfReferenceHolder = new PdfReferenceHolder((IPdfWrapper) pdfTrueTypeFont);
                usedFonts.Items.Add(key, (IPdfPrimitive) pdfReferenceHolder);
                usedFonts.Modify();
              }
            }
          }
          else if (fontDescriptor != null)
          {
            PdfStream pdfStream = (PdfStream) null;
            if (fontDescriptor.ContainsKey("FontFile2"))
              pdfStream = this.GetStreamFromRefernceHolder(fontDescriptor["FontFile2"]);
            else if (fontDescriptor.ContainsKey("FontFile3"))
              pdfStream = this.GetStreamFromRefernceHolder(fontDescriptor["FontFile3"]);
            else if (fontDescriptor.ContainsKey("FontFile"))
              pdfStream = this.GetStreamFromRefernceHolder(fontDescriptor["FontFile"]);
            if (pdfStream != null && pdfStream.ContainsKey("Subtype"))
            {
              PdfName pdfName3 = pdfStream["Subtype"] as PdfName;
              if (pdfName3 != (PdfName) null && pdfName3.Value == "OpenType")
              {
                pdfStream["Subtype"] = (IPdfPrimitive) new PdfName("TrueType");
                pdfStream.Modify();
              }
            }
          }
        }
        else if (fromRefernceHolder1.ContainsKey("BaseFont"))
        {
          string empty = string.Empty;
          PdfName pdfName4 = PdfCrossTable.Dereference(fromRefernceHolder1["BaseFont"]) as PdfName;
          if (pdfName4 != (PdfName) null && pdfName4.Value.Contains("+"))
          {
            string str = pdfName4.Value.Substring(pdfName4.Value.IndexOf('+') + 1);
            fromRefernceHolder1["BaseFont"] = (IPdfPrimitive) new PdfName(str);
          }
        }
      }
      if (pdfName1 != (PdfName) null && pdfName1.Value == "TrueType" || !flag1)
      {
        PdfDictionary fontDescriptor = (PdfDictionary) null;
        PdfDictionary descendantFont = (PdfDictionary) null;
        this.GetFontInternals(fromRefernceHolder1, out fontDescriptor, out descendantFont);
        if (fontDescriptor == null || !fontDescriptor.ContainsKey("FontFile") && !fontDescriptor.ContainsKey("FontFile2") && !fontDescriptor.ContainsKey("FontFile3"))
        {
          string familyName = string.Empty;
          if (fromRefernceHolder1.ContainsKey("BaseFont"))
          {
            PdfName pdfName5 = PdfCrossTable.Dereference(fromRefernceHolder1["BaseFont"]) as PdfName;
            if (pdfName5 != (PdfName) null)
              familyName = pdfName5.Value.Substring(pdfName5.Value.IndexOf('+') + 1);
          }
          FontStyle style = FontStyle.Regular;
          bool flag3 = false;
          if (familyName.Contains("PSMT"))
          {
            familyName = familyName.Remove(familyName.IndexOf("PSMT"));
            flag3 = true;
          }
          else if (familyName.Contains("BoldItalic"))
          {
            style = FontStyle.Bold | FontStyle.Italic;
            familyName = familyName.Remove(familyName.IndexOf("BoldItalic"));
          }
          else if (familyName.Contains("Bold"))
          {
            style = FontStyle.Bold;
            familyName = familyName.Remove(familyName.IndexOf("Bold"));
          }
          else if (familyName.Contains("Italic"))
          {
            style = FontStyle.Italic;
            familyName = familyName.Remove(familyName.IndexOf("Italic"));
          }
          if (familyName.Contains("PS"))
          {
            familyName = familyName.Remove(familyName.IndexOf("PS"));
            flag3 = true;
          }
          if (familyName.Contains("-"))
            familyName = familyName.Remove(familyName.IndexOf("-"));
          if (familyName.Contains(","))
            familyName = familyName.Remove(familyName.IndexOf(","));
          if (familyName.Contains("#20"))
            familyName = familyName.Replace("#20", " ");
          foreach (FontFamily family in FontFamily.Families)
          {
            string str = family.Name.Replace(" ", string.Empty);
            if (str != string.Empty && familyName.Contains(str))
            {
              familyName = family.Name;
              break;
            }
          }
          int num3 = int.MinValue;
          if (fontDescriptor != null && fontDescriptor.ContainsKey("Flags") && PdfCrossTable.Dereference(fontDescriptor["Flags"]) is PdfNumber pdfNumber)
            num3 = pdfNumber.IntValue;
          bool flag4 = false;
          if (pageResources != null && num3 != 96 /*0x60*/ && this.recordList.ContainsKey(pageResources))
          {
            flag4 = true;
            string str = string.Empty;
            PdfRecordCollection record3 = this.recordList[pageResources];
            for (int index = 0; index < record3.RecordCollection.Count; ++index)
            {
              PdfRecord record4 = record3.RecordCollection[index];
              if (record4.OperatorName == "Tf")
                str = record4.Operands[0].TrimStart('/');
              if (record4.OperatorName == "TJ" && str == key.Value)
              {
                flag4 = false;
                break;
              }
            }
          }
          PdfTrueTypeFont pdfTrueTypeFont;
          if (this.UsedFonts.Contains(key.Value) && !this.ReplaceFonts.ContainsKey(key.Value) && num3 != 4 && num3 != 32 /*0x20*/ && !flag3 && flag4)
          {
            this.OldFonts.Add(key.Value, fromRefernceHolder1);
            pdfTrueTypeFont = new PdfTrueTypeFont(new Font(familyName, num1, style, GraphicsUnit.Point), true, false, true);
          }
          else
            pdfTrueTypeFont = new PdfTrueTypeFont(new Font(familyName, num1, style, GraphicsUnit.Point), true, true, true, num1, true);
          usedFonts.Remove(key);
          if (!isForm)
          {
            pageResources.Add((PdfFont) pdfTrueTypeFont, key);
            if (this.UsedFonts.Contains(key.Value))
            {
              if (this.ReplaceFonts.ContainsKey(key.Value))
                this.ReplaceFonts[key.Value] = pdfTrueTypeFont;
              else
                this.ReplaceFonts.Add(key.Value, pdfTrueTypeFont);
            }
          }
          else
          {
            PdfReferenceHolder pdfReferenceHolder = new PdfReferenceHolder((IPdfWrapper) pdfTrueTypeFont);
            usedFonts.Items.Add(key, (IPdfPrimitive) pdfReferenceHolder);
            usedFonts.Modify();
          }
        }
        else if (fromRefernceHolder1.ContainsKey("Encoding") && PdfCrossTable.Dereference(fromRefernceHolder1["Encoding"]) is PdfDictionary pdfDictionary1 && pdfDictionary1.ContainsKey("Differences"))
        {
          if (!this.OldFonts.ContainsKey(key.Value))
            this.OldFonts.Add(key.Value, fromRefernceHolder1);
          if (this.PdfALevel != PdfConformanceLevel.Pdf_A1B)
          {
            if (!this.ReplaceFontDictionary.ContainsKey(key.Value))
            {
              PdfDictionary pdfDictionary = new PdfDictionary(fromRefernceHolder1);
              pdfDictionary.Remove("Encoding");
              this.ReplaceFontDictionary.Add(key.Value, pdfDictionary);
            }
          }
          else
          {
            PdfTrueTypeFont pdfTrueTypeFont = new PdfTrueTypeFont(new Font("Arial", num1, FontStyle.Regular, GraphicsUnit.Point), true, false, true);
            if (!this.ReplaceFonts.ContainsKey(key.Value))
              this.ReplaceFonts.Add(key.Value, pdfTrueTypeFont);
          }
        }
      }
      else if (pdfName1 != (PdfName) null && pdfName1.Value == "Type0")
      {
        PdfDictionary fontDescriptor = (PdfDictionary) null;
        PdfDictionary descendantFont = (PdfDictionary) null;
        if (fromRefernceHolder1.ContainsKey("DescendantFonts"))
        {
          if (fromRefernceHolder1["DescendantFonts"] is PdfArray)
          {
            descendantFont = this.GetDictionaryFromRefernceHolder((fromRefernceHolder1["DescendantFonts"] as PdfArray).Elements[0]);
            if (descendantFont.ContainsKey("FontDescriptor"))
              fontDescriptor = this.GetDictionaryFromRefernceHolder(descendantFont["FontDescriptor"]);
          }
          else if ((object) (fromRefernceHolder1["DescendantFonts"] as PdfReferenceHolder) != null)
          {
            descendantFont = this.GetDictionaryFromRefernceHolder(this.GetArrayFromReferenceHolder(fromRefernceHolder1["DescendantFonts"]).Elements[0]);
            if (descendantFont.ContainsKey("FontDescriptor"))
              fontDescriptor = this.GetDictionaryFromRefernceHolder(descendantFont["FontDescriptor"]);
          }
        }
        if (fontDescriptor != null)
        {
          if (fontDescriptor.ContainsKey("FontFile3"))
          {
            PdfDictionary fromRefernceHolder2 = this.GetDictionaryFromRefernceHolder(fontDescriptor["FontFile3"]);
            if (fromRefernceHolder2 != null && descendantFont != null && descendantFont.ContainsKey("Subtype") && (descendantFont["Subtype"] as PdfName).Value == "CIDFontType0")
              fromRefernceHolder2["Subtype"] = (IPdfPrimitive) new PdfName("CIDFontType0C");
          }
          FontStructure fontStructure = new FontStructure((IPdfPrimitive) fromRefernceHolder1);
          if (fromRefernceHolder1.ContainsKey("ToUnicode"))
          {
            byte[] numArray1 = new byte[8]
            {
              (byte) 128 /*0x80*/,
              (byte) 64 /*0x40*/,
              (byte) 32 /*0x20*/,
              (byte) 16 /*0x10*/,
              (byte) 8,
              (byte) 4,
              (byte) 2,
              (byte) 1
            };
            Dictionary<int, int> fontGlyphWidths = fontStructure.FontGlyphWidths;
            if (fontStructure.CharacterMapTable != null && fontStructure.CharacterMapTable.Count != 0)
            {
              Dictionary<double, string> characterMapTable = fontStructure.CharacterMapTable;
              byte[] data = (byte[]) null;
              if (fontStructure.FontGlyphWidths != null && fontStructure.FontGlyphWidths.Count > 0 && fontStructure.FontGlyphWidths.Count >= fontStructure.CharacterMapTable.Count)
              {
                int[] array = new int[fontStructure.FontGlyphWidths.Count];
                fontStructure.FontGlyphWidths.Keys.CopyTo(array, 0);
                System.Array.Sort<int>(array);
                data = new byte[array[array.Length - 1] / 8 + 1];
                for (int index = 0; index < array.Length; ++index)
                {
                  int num4 = array[index];
                  data[num4 / 8] |= numArray1[num4 % 8];
                }
              }
              if (data == null)
              {
                double[] array = new double[characterMapTable.Count];
                characterMapTable.Keys.CopyTo(array, 0);
                System.Array.Sort<double>(array);
                data = new byte[(int) array[array.Length - 1] / 8 + 1];
                for (int index = 0; index < array.Length; ++index)
                {
                  int num5 = (int) array[index];
                  data[num5 / 8] |= numArray1[num5 % 8];
                }
              }
              fromRefernceHolder1.Modify();
              PdfStream pdfStream = new PdfStream();
              pdfStream.Write(data);
              if (this.ldoc.Conformance != PdfConformanceLevel.Pdf_A3B)
                fontDescriptor["CIDSet"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfStream);
              if (this.ldoc.Conformance == PdfConformanceLevel.Pdf_A3B && fontDescriptor.ContainsKey("CIDSet"))
                fontDescriptor.Remove("CIDSet");
              fontDescriptor.Modify();
            }
            else if (fontStructure.UnicodeCharMapTable != null && fontStructure.UnicodeCharMapTable.Count > 0)
            {
              Dictionary<int, string> unicodeCharMapTable = fontStructure.UnicodeCharMapTable;
              byte[] numArray2 = (byte[]) null;
              int[] array = new int[unicodeCharMapTable.Count];
              unicodeCharMapTable.Keys.CopyTo(array, 0);
              System.Array.Sort<int>(array);
              byte[] data = new byte[array[array.Length - 1] / 8 + 1];
              for (int index = 0; index < array.Length; ++index)
              {
                int num6 = array[index];
                data[num6 / 8] |= numArray1[num6 % 8];
              }
              PdfStream pdfStream = new PdfStream();
              pdfStream.Write(data);
              fontDescriptor["CIDSet"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfStream);
              fontDescriptor.Modify();
              numArray2 = (byte[]) null;
            }
            string empty = string.Empty;
            if (fromRefernceHolder1.ContainsKey("BaseFont"))
            {
              PdfName pdfName6 = PdfCrossTable.Dereference(fromRefernceHolder1["BaseFont"]) as PdfName;
              if (pdfName6 != (PdfName) null && !pdfName6.Value.Contains("+"))
                this.MapWidthTable(fontDescriptor, descendantFont);
            }
          }
          else
          {
            byte[] numArray = new byte[8]
            {
              (byte) 128 /*0x80*/,
              (byte) 64 /*0x40*/,
              (byte) 32 /*0x20*/,
              (byte) 16 /*0x10*/,
              (byte) 8,
              (byte) 4,
              (byte) 2,
              (byte) 1
            };
            byte[] data = (byte[]) null;
            if (fontStructure.FontGlyphWidths != null && fontStructure.FontGlyphWidths.Count > 0)
            {
              int[] array = new int[fontStructure.FontGlyphWidths.Count];
              fontStructure.FontGlyphWidths.Keys.CopyTo(array, 0);
              System.Array.Sort<int>(array);
              data = new byte[array[array.Length - 1] / 8 + 1];
              for (int index = 0; index < array.Length; ++index)
              {
                int num7 = array[index];
                data[num7 / 8] |= numArray[num7 % 8];
              }
            }
            if (descendantFont != null && !descendantFont.ContainsKey("CIDToGIDMap"))
            {
              descendantFont.Items.Add(new PdfName("CIDToGIDMap"), (IPdfPrimitive) new PdfName("Identity"));
              descendantFont.Modify();
            }
            if (data != null)
            {
              PdfStream pdfStream = new PdfStream();
              pdfStream.Write(data);
              fontDescriptor["CIDSet"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfStream);
              if (this.ldoc.Conformance == PdfConformanceLevel.Pdf_A2B && fontDescriptor.ContainsKey("CIDSet"))
                fontDescriptor.Remove("CIDSet");
              fontDescriptor.Modify();
            }
          }
          if (fontDescriptor.ContainsKey("Flags"))
          {
            int num8 = int.MinValue;
            if (PdfCrossTable.Dereference(fontDescriptor["Flags"]) is PdfNumber pdfNumber)
              num8 = pdfNumber.IntValue;
            if ((num8 & 4) != 0)
            {
              fontDescriptor["Flags"] = (IPdfPrimitive) new PdfNumber(34);
              fromRefernceHolder1.Modify();
            }
          }
          if (fontStructure.IsCID && descendantFont != null && !descendantFont.ContainsKey("CIDToGIDMap"))
          {
            descendantFont["CIDToGIDMap"] = (IPdfPrimitive) new PdfName("Identity");
            descendantFont.Modify();
          }
        }
      }
    }
    if (!fromRefernceHolder1.ContainsKey("ToUnicode"))
      return;
    PdfDictionary pdfDictionary2 = (PdfDictionary) (PdfCrossTable.Dereference(fromRefernceHolder1["ToUnicode"]) as PdfStream);
    if (pdfDictionary2 == null || !pdfDictionary2.isSkip)
      return;
    pdfDictionary2.isSkip = false;
  }

  private void ParseForm(PdfLoadedDocument document)
  {
    if (document.Form != null)
    {
      PdfDictionary fromRefernceHolder1 = this.GetDictionaryFromRefernceHolder((IPdfPrimitive) document.Form.Dictionary);
      if (fromRefernceHolder1.ContainsKey("NeedAppearances"))
      {
        fromRefernceHolder1.Remove("NeedAppearances");
        fromRefernceHolder1.Modify();
        document.Form.NeedAppearances = false;
      }
      if (document.Form.Resources.ContainsKey("Font"))
        this.EmbedCompleteFonts((PdfResources) null, this.GetDictionaryFromRefernceHolder(document.Form.Resources["Font"]), true);
      if (document.Form.Fields != null && document.Form.Fields.Count > 0)
      {
        foreach (PdfLoadedField field in (PdfCollection) document.Form.Fields)
        {
          PdfDictionary dictionary = field.Dictionary;
          if (dictionary.ContainsKey("DR"))
          {
            if (dictionary["DR"] is PdfResources)
            {
              PdfResources fromRefernceHolder2 = this.GetResourceFromRefernceHolder(dictionary["DR"]);
              if (fromRefernceHolder2.ContainsKey("Font"))
                this.EmbedCompleteFonts((PdfResources) null, this.GetDictionaryFromRefernceHolder(fromRefernceHolder2["Font"]), true);
            }
            else if (dictionary["DR"] is PdfDictionary)
            {
              PdfDictionary fromRefernceHolder3 = this.GetDictionaryFromRefernceHolder(dictionary["DR"]);
              if (fromRefernceHolder3.ContainsKey("Font"))
                this.EmbedCompleteFonts((PdfResources) null, this.GetDictionaryFromRefernceHolder(fromRefernceHolder3["Font"]), true);
            }
          }
          if (field.Dictionary.ContainsKey("Kids"))
          {
            switch (field)
            {
              case PdfLoadedRadioButtonListField _:
                IEnumerator enumerator1 = (field as PdfLoadedRadioButtonListField).Items.GetEnumerator();
                try
                {
                  while (enumerator1.MoveNext())
                    this.RepairFormAppearenaceDirectory(((PdfLoadedFieldItem) enumerator1.Current).Dictionary, false);
                  break;
                }
                finally
                {
                  if (enumerator1 is IDisposable disposable)
                    disposable.Dispose();
                }
              case PdfLoadedButtonField _:
                IEnumerator enumerator2 = (field as PdfLoadedButtonField).Items.GetEnumerator();
                try
                {
                  while (enumerator2.MoveNext())
                    this.RepairFormAppearenaceDirectory(((PdfLoadedFieldItem) enumerator2.Current).Dictionary, true);
                  break;
                }
                finally
                {
                  if (enumerator2 is IDisposable disposable)
                    disposable.Dispose();
                }
              case PdfLoadedCheckBoxField _:
                IEnumerator enumerator3 = (field as PdfLoadedCheckBoxField).Items.GetEnumerator();
                try
                {
                  while (enumerator3.MoveNext())
                    this.RepairFormAppearenaceDirectory(((PdfLoadedFieldItem) enumerator3.Current).Dictionary, true);
                  break;
                }
                finally
                {
                  if (enumerator3 is IDisposable disposable)
                    disposable.Dispose();
                }
              case PdfLoadedComboBoxField _:
                IEnumerator enumerator4 = (field as PdfLoadedComboBoxField).Items.GetEnumerator();
                try
                {
                  while (enumerator4.MoveNext())
                    this.RepairFormAppearenaceDirectory(((PdfLoadedFieldItem) enumerator4.Current).Dictionary, false);
                  break;
                }
                finally
                {
                  if (enumerator4 is IDisposable disposable)
                    disposable.Dispose();
                }
              case PdfLoadedStateField _:
                IEnumerator enumerator5 = (field as PdfLoadedStateField).Items.GetEnumerator();
                try
                {
                  while (enumerator5.MoveNext())
                    this.RepairFormAppearenaceDirectory(((PdfLoadedFieldItem) enumerator5.Current).Dictionary, false);
                  break;
                }
                finally
                {
                  if (enumerator5 is IDisposable disposable)
                    disposable.Dispose();
                }
              case PdfLoadedTextBoxField _:
                IEnumerator enumerator6 = (field as PdfLoadedTextBoxField).Items.GetEnumerator();
                try
                {
                  while (enumerator6.MoveNext())
                    this.RepairFormAppearenaceDirectory(((PdfLoadedFieldItem) enumerator6.Current).Dictionary, false);
                  break;
                }
                finally
                {
                  if (enumerator6 is IDisposable disposable)
                    disposable.Dispose();
                }
              case PdfLoadedListBoxField _:
                IEnumerator enumerator7 = (field as PdfLoadedListBoxField).Items.GetEnumerator();
                try
                {
                  while (enumerator7.MoveNext())
                    this.RepairFormAppearenaceDirectory(((PdfLoadedFieldItem) enumerator7.Current).Dictionary, false);
                  break;
                }
                finally
                {
                  if (enumerator7 is IDisposable disposable)
                    disposable.Dispose();
                }
            }
          }
          this.RepairFormFontDirectory((IPdfPrimitive) dictionary, field);
        }
      }
      else
        this.ParseFormDictionary(document);
    }
    else
      this.ParseFormDictionary(document);
  }

  private void ParseFormDictionary(PdfLoadedDocument document)
  {
    foreach (PdfPageBase page in document.Pages)
    {
      PdfResources resources = page.GetResources();
      if (resources != null && resources.ContainsKey("XObject"))
      {
        PdfDictionary fromRefernceHolder = this.GetDictionaryFromRefernceHolder(resources["XObject"]);
        if (fromRefernceHolder != null)
        {
          foreach (PdfName key in new List<PdfName>((IEnumerable<PdfName>) fromRefernceHolder.Keys))
            this.ParseFormStreamFromXObject(this.GetDictionaryFromRefernceHolder(fromRefernceHolder[key]), key.Value);
        }
      }
    }
  }

  private void RepairFormAppearenaceDirectory(
    PdfDictionary loadedFieldDictionary,
    bool isButtonField)
  {
    if (loadedFieldDictionary.ContainsKey("AA"))
    {
      loadedFieldDictionary.Remove("AA");
      loadedFieldDictionary.Modify();
    }
    if (loadedFieldDictionary.ContainsKey("A"))
    {
      loadedFieldDictionary.Remove("A");
      loadedFieldDictionary.Modify();
    }
    if (loadedFieldDictionary.ContainsKey("AP"))
    {
      PdfDictionary fromRefernceHolder1 = this.GetDictionaryFromRefernceHolder(loadedFieldDictionary["AP"]);
      if (fromRefernceHolder1.ContainsKey("D") && fromRefernceHolder1.ContainsKey("N"))
      {
        if (fromRefernceHolder1["N"] is PdfNull)
        {
          if (isButtonField)
          {
            loadedFieldDictionary["AP"] = (IPdfPrimitive) this.CreateNewAppearanceDictionary(true);
            loadedFieldDictionary["AS"] = (IPdfPrimitive) new PdfName("Off");
          }
          else
            loadedFieldDictionary["AP"] = (IPdfPrimitive) this.CreateNewAppearanceDictionary(false);
          loadedFieldDictionary.Modify();
        }
        else if (isButtonField)
        {
          PdfDictionary fromRefernceHolder2 = this.GetDictionaryFromRefernceHolder(fromRefernceHolder1["N"]);
          if (fromRefernceHolder2 != null && fromRefernceHolder2 is PdfStream)
          {
            PdfReferenceHolder pdfReferenceHolder = new PdfReferenceHolder(fromRefernceHolder1["N"]);
            fromRefernceHolder1["N"] = (IPdfPrimitive) new PdfDictionary();
            (fromRefernceHolder1["N"] as PdfDictionary)["Off"] = (IPdfPrimitive) pdfReferenceHolder;
            loadedFieldDictionary["AS"] = (IPdfPrimitive) new PdfName("Off");
            fromRefernceHolder1.Modify();
            loadedFieldDictionary.Modify();
          }
        }
        fromRefernceHolder1.Remove("D");
        fromRefernceHolder1.Modify();
      }
      else if (fromRefernceHolder1.ContainsKey("N"))
      {
        if (fromRefernceHolder1["N"] is PdfNull)
        {
          if (isButtonField)
          {
            loadedFieldDictionary["AP"] = (IPdfPrimitive) this.CreateNewAppearanceDictionary(true);
            loadedFieldDictionary["AS"] = (IPdfPrimitive) new PdfName("Off");
          }
          else
            loadedFieldDictionary["AP"] = (IPdfPrimitive) this.CreateNewAppearanceDictionary(false);
        }
        else if (isButtonField)
        {
          PdfReferenceHolder pdfReferenceHolder = new PdfReferenceHolder(fromRefernceHolder1["N"]);
          fromRefernceHolder1["N"] = (IPdfPrimitive) new PdfDictionary();
          (fromRefernceHolder1["N"] as PdfDictionary)["Off"] = (IPdfPrimitive) pdfReferenceHolder;
          loadedFieldDictionary["AS"] = (IPdfPrimitive) new PdfName("Off");
          fromRefernceHolder1.Modify();
          loadedFieldDictionary.Modify();
        }
        loadedFieldDictionary.Modify();
      }
      if (!fromRefernceHolder1.ContainsKey("N"))
        return;
      foreach (PdfName key in new List<PdfName>((IEnumerable<PdfName>) fromRefernceHolder1.Items.Keys))
      {
        PdfStream fromRefernceHolder3 = this.GetStreamFromRefernceHolder(fromRefernceHolder1.Items[key]);
        if (fromRefernceHolder3 != null)
          this.ParseFormStream(fromRefernceHolder3, key.Value, (PdfDictionary) null, true);
      }
    }
    else
    {
      if (isButtonField)
      {
        loadedFieldDictionary["AP"] = (IPdfPrimitive) this.CreateNewAppearanceDictionary(true);
        loadedFieldDictionary["AS"] = (IPdfPrimitive) new PdfName("Off");
      }
      else
        loadedFieldDictionary["AP"] = (IPdfPrimitive) this.CreateNewAppearanceDictionary(false);
      loadedFieldDictionary.Modify();
    }
  }

  private void RepairFormFontDirectory(IPdfPrimitive formField, PdfLoadedField loadedField)
  {
    PdfDictionary fromRefernceHolder1 = this.GetDictionaryFromRefernceHolder(formField);
    if (fromRefernceHolder1.ContainsKey("AA"))
    {
      fromRefernceHolder1.Remove("AA");
      fromRefernceHolder1.Modify();
    }
    if (fromRefernceHolder1.ContainsKey("A"))
    {
      fromRefernceHolder1.Remove("A");
      fromRefernceHolder1.Modify();
    }
    if (fromRefernceHolder1.ContainsKey("NeedAppearances"))
      fromRefernceHolder1["NeedAppearances"] = (IPdfPrimitive) new PdfBoolean(false);
    if (fromRefernceHolder1.ContainsKey("AP"))
    {
      PdfDictionary fromRefernceHolder2 = this.GetDictionaryFromRefernceHolder(fromRefernceHolder1["AP"]);
      if (fromRefernceHolder2.ContainsKey("D"))
      {
        if (fromRefernceHolder2.ContainsKey("N"))
        {
          fromRefernceHolder2.Remove("D");
          fromRefernceHolder2.Modify();
        }
        else
        {
          PdfDictionary fromRefernceHolder3 = this.GetDictionaryFromRefernceHolder(fromRefernceHolder2["D"]);
          if (fromRefernceHolder3.ContainsKey(new PdfName("Home")))
          {
            PdfStream fromRefernceHolder4 = this.GetStreamFromRefernceHolder(fromRefernceHolder3[new PdfName("Home")]);
            if (fromRefernceHolder4.ContainsKey("Resources"))
            {
              PdfDictionary fromRefernceHolder5 = this.GetDictionaryFromRefernceHolder(fromRefernceHolder4["Resources"]);
              if (fromRefernceHolder5.ContainsKey("Font"))
                this.EmbedCompleteFonts((PdfResources) null, this.GetDictionaryFromRefernceHolder(fromRefernceHolder5["Font"]), true);
            }
          }
          if (fromRefernceHolder3.ContainsKey(new PdfName("Off")))
          {
            PdfStream fromRefernceHolder6 = this.GetStreamFromRefernceHolder(fromRefernceHolder3[new PdfName("Off")]);
            if (fromRefernceHolder6.ContainsKey("Resources"))
            {
              PdfDictionary fromRefernceHolder7 = this.GetDictionaryFromRefernceHolder(fromRefernceHolder6["Resources"]);
              if (fromRefernceHolder7.ContainsKey("Font"))
                this.EmbedCompleteFonts((PdfResources) null, this.GetDictionaryFromRefernceHolder(fromRefernceHolder7["Font"]), true);
            }
          }
        }
      }
      if (fromRefernceHolder2.ContainsKey("N"))
      {
        PdfStream fromRefernceHolder8 = this.GetStreamFromRefernceHolder(fromRefernceHolder2["N"]);
        if (fromRefernceHolder8 != null)
        {
          if (loadedField is PdfLoadedButtonField)
          {
            PdfReferenceHolder pdfReferenceHolder = new PdfReferenceHolder(fromRefernceHolder2["N"]);
            fromRefernceHolder2["N"] = (IPdfPrimitive) new PdfDictionary();
            (fromRefernceHolder2["N"] as PdfDictionary)["Off"] = (IPdfPrimitive) pdfReferenceHolder;
            fromRefernceHolder1["AS"] = (IPdfPrimitive) new PdfName("Off");
            fromRefernceHolder2.Modify();
          }
          if (fromRefernceHolder8.ContainsKey("Resources"))
          {
            PdfDictionary fromRefernceHolder9 = this.GetDictionaryFromRefernceHolder(fromRefernceHolder8["Resources"]);
            if (fromRefernceHolder9 != null && fromRefernceHolder9.ContainsKey("Font"))
              this.EmbedCompleteFonts((PdfResources) null, this.GetDictionaryFromRefernceHolder(fromRefernceHolder9["Font"]), true);
          }
          this.ParseFormStream(fromRefernceHolder8, "", (PdfDictionary) null, true);
        }
        else
        {
          PdfDictionary fromRefernceHolder10 = this.GetDictionaryFromRefernceHolder(fromRefernceHolder2["N"]);
          if (fromRefernceHolder10 != null)
          {
            if (loadedField is PdfLoadedButtonField)
            {
              PdfReferenceHolder pdfReferenceHolder = new PdfReferenceHolder(fromRefernceHolder2["N"]);
              fromRefernceHolder2["N"] = (IPdfPrimitive) new PdfDictionary();
              (fromRefernceHolder2["N"] as PdfDictionary)["Off"] = (IPdfPrimitive) pdfReferenceHolder;
              fromRefernceHolder1["AS"] = (IPdfPrimitive) new PdfName("Off");
              fromRefernceHolder2.Modify();
            }
            else
            {
              foreach (PdfName key in new List<PdfName>((IEnumerable<PdfName>) fromRefernceHolder10.Keys))
              {
                PdfStream fromRefernceHolder11 = this.GetStreamFromRefernceHolder(fromRefernceHolder10[key]);
                if (fromRefernceHolder11 != null)
                {
                  if (fromRefernceHolder11.ContainsKey("Resources"))
                  {
                    PdfDictionary fromRefernceHolder12 = this.GetDictionaryFromRefernceHolder(fromRefernceHolder11["Resources"]);
                    if (fromRefernceHolder12.ContainsKey("Font"))
                      this.EmbedCompleteFonts((PdfResources) null, this.GetDictionaryFromRefernceHolder(fromRefernceHolder12["Font"]), true);
                  }
                }
                else
                {
                  if (loadedField is PdfLoadedButtonField)
                  {
                    fromRefernceHolder1["AP"] = (IPdfPrimitive) this.CreateNewAppearanceDictionary(true);
                    fromRefernceHolder1["AS"] = (IPdfPrimitive) new PdfName("Off");
                  }
                  else
                    fromRefernceHolder1["AP"] = (IPdfPrimitive) this.CreateNewAppearanceDictionary(false);
                  fromRefernceHolder1.Modify();
                }
              }
            }
            if (fromRefernceHolder10.ContainsKey("Resources"))
            {
              PdfDictionary fromRefernceHolder13 = this.GetDictionaryFromRefernceHolder(fromRefernceHolder10["Resources"]);
              if (fromRefernceHolder13 != null && fromRefernceHolder13.ContainsKey("Font"))
                this.EmbedCompleteFonts((PdfResources) null, this.GetDictionaryFromRefernceHolder(fromRefernceHolder13["Font"]), true);
            }
            foreach (PdfName key in new List<PdfName>((IEnumerable<PdfName>) fromRefernceHolder10.Items.Keys))
            {
              PdfStream fromRefernceHolder14 = this.GetStreamFromRefernceHolder(fromRefernceHolder10.Items[key]);
              if (fromRefernceHolder14 != null)
                this.ParseFormStream(fromRefernceHolder14, key.Value, (PdfDictionary) null, true);
            }
          }
          else if (fromRefernceHolder2["N"] is PdfNull)
          {
            if (loadedField is PdfLoadedButtonField)
            {
              fromRefernceHolder1["AP"] = (IPdfPrimitive) this.CreateNewAppearanceDictionary(true);
              fromRefernceHolder1["AS"] = (IPdfPrimitive) new PdfName("Off");
            }
            else
              fromRefernceHolder1["AP"] = (IPdfPrimitive) this.CreateNewAppearanceDictionary(false);
            fromRefernceHolder1.Modify();
          }
        }
      }
      if (!fromRefernceHolder2.ContainsKey("N") && !fromRefernceHolder2.ContainsKey("D"))
      {
        if (loadedField is PdfLoadedButtonField)
        {
          fromRefernceHolder1["AP"] = (IPdfPrimitive) this.CreateNewAppearanceDictionary(true);
          fromRefernceHolder1["AS"] = (IPdfPrimitive) new PdfName("Off");
        }
        else
          fromRefernceHolder1["AP"] = (IPdfPrimitive) this.CreateNewAppearanceDictionary(false);
        fromRefernceHolder1.Modify();
      }
    }
    else
    {
      if (loadedField is PdfLoadedButtonField)
      {
        fromRefernceHolder1["AP"] = (IPdfPrimitive) this.CreateNewAppearanceDictionary(true);
        fromRefernceHolder1["AS"] = (IPdfPrimitive) new PdfName("Off");
      }
      else
        fromRefernceHolder1["AP"] = (IPdfPrimitive) this.CreateNewAppearanceDictionary(false);
      fromRefernceHolder1.Modify();
    }
    if (!fromRefernceHolder1.ContainsKey("MK"))
      return;
    PdfDictionary fromRefernceHolder15 = this.GetDictionaryFromRefernceHolder(fromRefernceHolder1["MK"]);
    if (fromRefernceHolder15 == null || fromRefernceHolder15.Items.Count <= 0)
      return;
    if (fromRefernceHolder15.ContainsKey("D"))
    {
      PdfDictionary fromRefernceHolder16 = this.GetDictionaryFromRefernceHolder(fromRefernceHolder15["D"]);
      if (fromRefernceHolder16.ContainsKey(new PdfName("Home")))
      {
        PdfStream fromRefernceHolder17 = this.GetStreamFromRefernceHolder(fromRefernceHolder16[new PdfName("Home")]);
        if (fromRefernceHolder17.ContainsKey("Resources"))
        {
          PdfDictionary fromRefernceHolder18 = this.GetDictionaryFromRefernceHolder(fromRefernceHolder17["Resources"]);
          if (fromRefernceHolder18.ContainsKey("Font"))
            this.EmbedCompleteFonts((PdfResources) null, this.GetDictionaryFromRefernceHolder(fromRefernceHolder18["Font"]), true);
        }
      }
      if (fromRefernceHolder16.ContainsKey(new PdfName("Yes")))
      {
        PdfStream fromRefernceHolder19 = this.GetStreamFromRefernceHolder(fromRefernceHolder16[new PdfName("Yes")]);
        if (fromRefernceHolder19.ContainsKey("Resources"))
        {
          PdfDictionary fromRefernceHolder20 = this.GetDictionaryFromRefernceHolder(fromRefernceHolder19["Resources"]);
          if (fromRefernceHolder20.ContainsKey("Font"))
            this.EmbedCompleteFonts((PdfResources) null, this.GetDictionaryFromRefernceHolder(fromRefernceHolder20["Font"]), true);
        }
      }
      if (fromRefernceHolder16.ContainsKey(new PdfName("Off")))
      {
        PdfStream fromRefernceHolder21 = this.GetStreamFromRefernceHolder(fromRefernceHolder16[new PdfName("Off")]);
        if (fromRefernceHolder21.ContainsKey("Resources"))
        {
          PdfDictionary fromRefernceHolder22 = this.GetDictionaryFromRefernceHolder(fromRefernceHolder21["Resources"]);
          if (fromRefernceHolder22.ContainsKey("Font"))
            this.EmbedCompleteFonts((PdfResources) null, this.GetDictionaryFromRefernceHolder(fromRefernceHolder22["Font"]), true);
        }
      }
    }
    if (!fromRefernceHolder15.ContainsKey("N"))
      return;
    PdfDictionary fromRefernceHolder23 = this.GetDictionaryFromRefernceHolder(fromRefernceHolder15["N"]);
    if (fromRefernceHolder23.ContainsKey(new PdfName("Home")))
    {
      PdfStream fromRefernceHolder24 = this.GetStreamFromRefernceHolder(fromRefernceHolder23[new PdfName("Home")]);
      if (fromRefernceHolder24.ContainsKey("Resources"))
      {
        PdfDictionary fromRefernceHolder25 = this.GetDictionaryFromRefernceHolder(fromRefernceHolder24["Resources"]);
        if (fromRefernceHolder25.ContainsKey("Font"))
          this.EmbedCompleteFonts((PdfResources) null, this.GetDictionaryFromRefernceHolder(fromRefernceHolder25["Font"]), true);
      }
    }
    if (fromRefernceHolder23.ContainsKey(new PdfName("Yes")))
    {
      PdfStream fromRefernceHolder26 = this.GetStreamFromRefernceHolder(fromRefernceHolder23[new PdfName("Yes")]);
      if (fromRefernceHolder26.ContainsKey("Resources"))
      {
        PdfDictionary fromRefernceHolder27 = this.GetDictionaryFromRefernceHolder(fromRefernceHolder26["Resources"]);
        if (fromRefernceHolder27.ContainsKey("Font"))
          this.EmbedCompleteFonts((PdfResources) null, this.GetDictionaryFromRefernceHolder(fromRefernceHolder27["Font"]), true);
      }
    }
    if (!fromRefernceHolder23.ContainsKey(new PdfName("Off")))
      return;
    PdfStream fromRefernceHolder28 = this.GetStreamFromRefernceHolder(fromRefernceHolder23[new PdfName("Off")]);
    if (!fromRefernceHolder28.ContainsKey("Resources"))
      return;
    PdfDictionary fromRefernceHolder29 = this.GetDictionaryFromRefernceHolder(fromRefernceHolder28["Resources"]);
    if (!fromRefernceHolder29.ContainsKey("Font"))
      return;
    this.EmbedCompleteFonts((PdfResources) null, this.GetDictionaryFromRefernceHolder(fromRefernceHolder29["Font"]), true);
  }

  private PdfDictionary CreateNewAppearanceDictionary(bool isButtonField)
  {
    PdfDictionary appearanceDictionary = new PdfDictionary();
    PdfStream pdfStream = new PdfStream(new PdfDictionary(), Encoding.UTF8.GetBytes(" "));
    pdfStream["Type"] = (IPdfPrimitive) new PdfName("XObject");
    pdfStream["Resources"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) new PdfNull());
    pdfStream["Filter"] = (IPdfPrimitive) new PdfName("FlateDecode");
    double[] array = new double[4]
    {
      0.0,
      0.0,
      139.5,
      13.1151
    };
    pdfStream["BBox"] = (IPdfPrimitive) new PdfArray(array);
    pdfStream["Subtype"] = (IPdfPrimitive) new PdfName("Form");
    if (!isButtonField)
      appearanceDictionary["N"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfStream);
    else
      appearanceDictionary["N"] = (IPdfPrimitive) new PdfDictionary()
      {
        ["Off"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfStream)
      };
    return appearanceDictionary;
  }

  protected void AddDocumentColorProfile(PdfLoadedDocument document)
  {
    if (document.Catalog.ContainsKey("OCProperties"))
    {
      document.Catalog.Remove("OCProperties");
      document.Catalog.Modify();
    }
    if (document.Catalog.ContainsKey("OpenAction") && document.Catalog["OpenAction"] is PdfDictionary && (document.Catalog["OpenAction"] as PdfDictionary).ContainsKey("S"))
    {
      PdfName pdfName = (document.Catalog["OpenAction"] as PdfDictionary)["S"] as PdfName;
      if (pdfName != (PdfName) null && !this.IsValidAction(pdfName.Value))
      {
        document.Catalog.Remove("OpenAction");
        document.Catalog.Modify();
      }
    }
    if (document.Catalog.ContainsKey("AA"))
    {
      document.Catalog.Remove("AA");
      document.Catalog.Modify();
    }
    if (document.Catalog.ContainsKey("StructTreeRoot"))
    {
      document.Catalog.Remove("StructTreeRoot");
      document.Catalog.Modify();
    }
    PdfDictionary pdfDictionary = new PdfDictionary();
    pdfDictionary["Info"] = (IPdfPrimitive) new PdfString("sRGB IEC61966-2.1");
    pdfDictionary["S"] = (IPdfPrimitive) new PdfName("GTS_PDFA1");
    pdfDictionary["OutputConditionIdentifier"] = (IPdfPrimitive) new PdfString("custom");
    pdfDictionary["Type"] = (IPdfPrimitive) new PdfName("OutputIntent");
    pdfDictionary["OutputCondition"] = (IPdfPrimitive) new PdfString("");
    pdfDictionary["RegistryName"] = (IPdfPrimitive) new PdfString("");
    PdfICCColorProfile wrapper = new PdfICCColorProfile();
    (wrapper.Element as PdfStream)["Range"] = (IPdfPrimitive) new PdfArray(new int[6]
    {
      0,
      1,
      0,
      1,
      0,
      1
    });
    pdfDictionary["DestOutputProfile"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) wrapper);
    document.Catalog["OutputIntents"] = (IPdfPrimitive) new PdfArray()
    {
      (IPdfPrimitive) pdfDictionary
    };
  }

  protected void AddTrailerID(PdfLoadedDocument document)
  {
    if (document.CrossTable.Trailer.ContainsKey("ID"))
      document.CrossTable.Trailer.Items.Remove(new PdfName("ID"));
    if (document.CrossTable.Trailer.ContainsKey("Encrypt"))
    {
      if ((object) (document.CrossTable.Trailer["Encrypt"] as PdfReferenceHolder) != null && (object) (document.CrossTable.Trailer["Encrypt"] as PdfReferenceHolder).Reference != null)
      {
        int objectIndex = document.CrossTable.PdfObjects.GetObjectIndex((document.CrossTable.Trailer["Encrypt"] as PdfReferenceHolder).Reference);
        if (objectIndex >= 0)
          document.CrossTable.PdfObjects.Remove(objectIndex);
      }
      document.CrossTable.Trailer.Items.Remove(new PdfName("Encrypt"));
    }
    PdfSecurity security = document.Security;
    security.Encryptor = new PdfEncryptor();
    document.CrossTable.Trailer["ID"] = (IPdfPrimitive) security.Encryptor.FileID;
    document.CrossTable.Trailer.Modify();
    PdfDocument.ConformanceLevel = PdfConformanceLevel.Pdf_A1B;
  }

  protected void AddMetaDataInfo(PdfLoadedDocument document)
  {
    XmpMetadata metadata = document.Catalog.Metadata;
    PdfDocumentInformation documentInformation = document.DocumentInformation;
    documentInformation.ConformanceEnabled = true;
    document.Conformance = PdfConformanceLevel.None;
    if (document.Catalog.ContainsKey("Metadata"))
    {
      document.Catalog.Remove("Metadata");
      document.Catalog.Metadata = (XmpMetadata) null;
    }
    if (documentInformation.Dictionary != null)
    {
      if (documentInformation.Dictionary.ContainsKey("CreationDate") && PdfCrossTable.Dereference(documentInformation.Dictionary["CreationDate"]) is PdfString pdfString && !string.IsNullOrEmpty(pdfString.Value))
      {
        documentInformation.Dictionary.Remove("CreationDate");
        documentInformation.CreationDate = DateTime.Now;
      }
      if (documentInformation.Dictionary.ContainsKey("ModDate"))
        documentInformation.Dictionary.Remove("ModDate");
    }
    if (metadata != null)
    {
      if (metadata.BasicSchema != null && !string.IsNullOrEmpty(metadata.BasicSchema.CreatorTool) && string.IsNullOrEmpty(documentInformation.Creator))
        documentInformation.Creator = metadata.BasicSchema.CreatorTool;
      if (metadata.PDFSchema != null && documentInformation.Dictionary.ContainsKey("Producer") && !string.IsNullOrEmpty(metadata.PDFSchema.Producer) && string.IsNullOrEmpty(documentInformation.Producer))
        documentInformation.Producer = metadata.PDFSchema.Producer;
    }
    documentInformation.ModificationDate = DateTime.Now;
    string str = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(document.DocumentInformation.Title));
    if (this.PdfALevel == PdfConformanceLevel.Pdf_A1B)
    {
      if (!string.IsNullOrEmpty(str))
        document.DocumentInformation.Title = str;
    }
    else
      document.DocumentInformation.Title = str;
    XmpMetadata xmpMetadata = documentInformation.XmpMetadata;
    xmpMetadata.NamespaceManager.AddNamespace("pdfaid", "http://www.aiim.org/pdfa/ns/id/");
    XmlElement element = xmpMetadata.CreateElement("rdf", "Description", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
    XmlAttribute attribute1 = xmpMetadata.CreateAttribute("rdf", "about", "http://www.w3.org/1999/02/22-rdf-syntax-ns#", "");
    XmlAttribute attribute2 = xmpMetadata.CreateAttribute("pdfaid", "part", "http://www.aiim.org/pdfa/ns/id/", "1");
    if (this.PdfALevel == PdfConformanceLevel.Pdf_A1B)
      attribute2 = xmpMetadata.CreateAttribute("pdfaid", "part", "http://www.aiim.org/pdfa/ns/id/", "1");
    else if (this.PdfALevel == PdfConformanceLevel.Pdf_A2B)
      attribute2 = xmpMetadata.CreateAttribute("pdfaid", "part", "http://www.aiim.org/pdfa/ns/id/", "2");
    else if (this.PdfALevel == PdfConformanceLevel.Pdf_A3B)
      attribute2 = xmpMetadata.CreateAttribute("pdfaid", "part", "http://www.aiim.org/pdfa/ns/id/", "3");
    XmlAttribute attribute3 = xmpMetadata.CreateAttribute("pdfaid", "conformance", "http://www.aiim.org/pdfa/ns/id/", "B");
    element.Attributes.Append(attribute1);
    element.Attributes.Append(attribute2);
    element.Attributes.Append(attribute3);
    xmpMetadata.Xmpmeta.AppendChild((XmlNode) element);
    xmpMetadata.Rdf.AppendChild((XmlNode) element);
    document.Catalog["Metadata"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) xmpMetadata);
    document.Catalog.Modify();
    documentInformation.ConformanceEnabled = false;
    documentInformation.Dictionary.Modify();
    this.EnsureImageMetadataInfo(document);
  }

  private void EnsureImageMetadataInfo(PdfLoadedDocument document)
  {
    for (int index1 = 0; index1 < document.Pages.Count; ++index1)
    {
      PdfDictionary fromRefernceHolder1 = this.GetDictionaryFromRefernceHolder(document.Pages[index1].Dictionary["Resources"]);
      if (fromRefernceHolder1 != null)
      {
        PdfDictionary fromRefernceHolder2 = this.GetDictionaryFromRefernceHolder(fromRefernceHolder1["XObject"]);
        if (fromRefernceHolder2 != null)
        {
          foreach (IPdfPrimitive primitive1 in fromRefernceHolder2.Values)
          {
            PdfDictionary fromRefernceHolder3 = this.GetDictionaryFromRefernceHolder(primitive1);
            if (fromRefernceHolder3 != null && fromRefernceHolder3.ContainsKey("Subtype") && (fromRefernceHolder3["Subtype"] as PdfName).Value == "Image" && fromRefernceHolder3.ContainsKey("Metadata"))
            {
              if (fromRefernceHolder3 is PdfStream)
              {
                XmpMetadata metadata = this.GetMetadata(fromRefernceHolder3 as PdfStream);
                if (metadata != null)
                {
                  foreach (XmlNode childNode in metadata.Rdf.ChildNodes)
                    this.EnsureImageSchemaData(childNode);
                  XmlDocument xmlData = metadata.XmlData;
                  if (xmlData.FirstChild.Name != "xpacket")
                  {
                    XmlProcessingInstruction processingInstruction1 = xmlData.CreateProcessingInstruction("xpacket", "begin=\"\uFEFF\" id=\"W5M0MpCehiHzreSzNTczkc9d\"");
                    xmlData.InsertBefore((XmlNode) processingInstruction1, xmlData.FirstChild);
                    XmlProcessingInstruction processingInstruction2 = xmlData.CreateProcessingInstruction("xpacket", "end=\"r\"");
                    xmlData.AppendChild((XmlNode) processingInstruction2);
                  }
                  PdfReferenceHolder primitive2 = new PdfReferenceHolder((IPdfWrapper) new XmpMetadata(xmlData));
                  fromRefernceHolder3.SetProperty("Metadata", (IPdfPrimitive) primitive2);
                  if (fromRefernceHolder3.ContainsKey("SMask"))
                  {
                    PdfDictionary fromRefernceHolder4 = this.GetDictionaryFromRefernceHolder(fromRefernceHolder3["SMask"]);
                    if (fromRefernceHolder4.ContainsKey("Metadata"))
                    {
                      int index2 = document.PdfObjects.IndexOf((IPdfPrimitive) this.GetDictionaryFromRefernceHolder(fromRefernceHolder4["Metadata"]));
                      document.PdfObjects.Remove(index2);
                      fromRefernceHolder4.SetProperty("Metadata", (IPdfPrimitive) primitive2);
                    }
                  }
                }
                else
                  fromRefernceHolder3.Remove("Metadata");
              }
              else
                fromRefernceHolder3.Remove("Metadata");
            }
          }
        }
      }
    }
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

  private void EnsureImageSchemaData(XmlNode childNode)
  {
    if (childNode == null)
      return;
    Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>()
    {
      {
        "xmp",
        new List<string>() { "CreateDate" }
      },
      {
        "photoshop",
        new List<string>()
        {
          "LegacyIPTCDigest",
          "DocumentAncestors",
          "ColorMode"
        }
      },
      {
        "xmpMM",
        new List<string>() { "OriginalDocumentID" }
      },
      {
        "stRef",
        new List<string>() { "originalDocumentID" }
      },
      {
        "exif",
        new List<string>() { "DateTimeOriginal" }
      },
      {
        "aux",
        new List<string>()
        {
          "LensInfo",
          "ImageNumber",
          "FlashCompensation",
          "OwnerName",
          "Firmware"
        }
      },
      {
        "crs",
        new List<string>()
        {
          "FillLight",
          "Vibrance",
          "HighlightRecovery",
          "HueAdjustmentRed",
          "HueAdjustmentOrange",
          "HueAdjustmentYellow",
          "HueAdjustmentGreen",
          "HueAdjustmentAqua",
          "HueAdjustmentBlue",
          "HueAdjustmentPurple",
          "HueAdjustmentMagenta",
          "SaturationAdjustmentRed",
          "SaturationAdjustmentOrange",
          "SaturationAdjustmentYellow",
          "SaturationAdjustmentGreen",
          "SaturationAdjustmentAqua",
          "SaturationAdjustmentBlue",
          "SaturationAdjustmentPurple",
          "SaturationAdjustmentMagenta",
          "LuminanceAdjustmentRed",
          "LuminanceAdjustmentOrange",
          "LuminanceAdjustmentYellow",
          "LuminanceAdjustmentGreen",
          "LuminanceAdjustmentAqua",
          "LuminanceAdjustmentBlue",
          "LuminanceAdjustmentPurple",
          "LuminanceAdjustmentMagenta",
          "SplitToningShadowHue",
          "SplitToningShadowSaturation",
          "SplitToningHighlightHue",
          "SplitToningHighlightSaturation",
          "SplitToningBalance",
          "ParametricShadows",
          "ParametricDarks",
          "ParametricLights",
          "ParametricHighlights",
          "ParametricShadowSplit",
          "ParametricMidtoneSplit",
          "ParametricHighlightSplit",
          "ConvertToGrayscale",
          "AutoGrayscaleWeights",
          "AlreadyApplied"
        }
      },
      {
        "stEvt",
        new List<string>() { "changed" }
      }
    };
    if (childNode.Attributes != null && childNode.Attributes.Count > 0)
    {
      for (int i = childNode.Attributes.Count - 1; i >= 0; --i)
      {
        XmlAttribute attribute = childNode.Attributes[i];
        if (dictionary.ContainsKey(attribute.Prefix) && dictionary[attribute.Prefix].Contains(attribute.LocalName))
          childNode.Attributes.Remove(attribute);
      }
    }
    if (childNode.ChildNodes != null && childNode.ChildNodes.Count > 0)
    {
      for (int i = childNode.ChildNodes.Count - 1; i >= 0; --i)
      {
        XmlNode childNode1 = childNode.ChildNodes[i];
        bool flag = true;
        if (dictionary.ContainsKey(childNode1.Prefix) && dictionary[childNode1.Prefix].Contains(childNode1.LocalName))
        {
          childNode.RemoveChild(childNode1);
          flag = false;
        }
        if (flag)
          this.EnsureImageSchemaData(childNode1);
      }
    }
    dictionary.Clear();
  }

  private void MapWidthTable(PdfDictionary fontDescriptor, PdfDictionary descendantFont)
  {
    if (fontDescriptor == null || !fontDescriptor.ContainsKey("FontFile2"))
      return;
    PdfReferenceHolder pdfReferenceHolder = fontDescriptor["FontFile2"] as PdfReferenceHolder;
    if (!(pdfReferenceHolder != (PdfReferenceHolder) null) || !(pdfReferenceHolder.Object is PdfStream pdfStream) || pdfStream.Data.Length <= 0)
      return;
    pdfStream.Decompress();
    TtfReader ttfReader = new TtfReader(!pdfStream.InternalStream.CanSeek ? new BinaryReader((Stream) new MemoryStream(pdfStream.Data), TtfReader.Encoding) : new BinaryReader((Stream) pdfStream.InternalStream, TtfReader.Encoding));
    ttfReader.CreateInternals();
    List<TtfGlyphInfo> ttfGlyphInfoList = new List<TtfGlyphInfo>();
    foreach (TtfGlyphInfo ttfGlyphInfo in ttfReader.CompleteGlyph.Values)
      ttfGlyphInfoList.Add(ttfGlyphInfo);
    ttfGlyphInfoList.Sort();
    PdfArray element = new PdfArray();
    PdfArray pdfArray = new PdfArray();
    List<int> intList = new List<int>();
    int index1 = 0;
    for (int count = ttfGlyphInfoList.Count; index1 < count; ++index1)
    {
      TtfGlyphInfo ttfGlyphInfo = ttfGlyphInfoList[index1];
      int index2 = ttfGlyphInfo.Index;
      if (!intList.Contains(ttfGlyphInfo.Index))
      {
        element.Add((IPdfPrimitive) new PdfNumber(ttfGlyphInfo.Width));
        pdfArray.Add((IPdfPrimitive) new PdfNumber(index2));
        pdfArray.Add((IPdfPrimitive) element);
        intList.Add(index2);
        element = new PdfArray();
        if (pdfArray.Count >= 8190)
          break;
      }
    }
    intList.Clear();
    descendantFont["W"] = (IPdfPrimitive) pdfArray;
  }
}
