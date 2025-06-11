// Decompiled with JetBrains decompiler
// Type: Syncfusion.XPS.OXPSDocumentReader
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Compression.Zip;
using Syncfusion.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

#nullable disable
namespace Syncfusion.XPS;

internal class OXPSDocumentReader : IDisposable
{
  private ZipArchive xpsFile;
  private OXPSFixedDocumentSequence documentSequence;
  private List<OXPSFixedDocument> documents;
  private List<OXPSFixedPage> pages;
  private Dictionary<string, PdfTrueTypeFont> m_fonts;
  private Regex FixedDocumentSequence = new Regex("fdseq");
  private int fPageCount = -1;
  private bool isfPageMoreThanOnce;

  internal OXPSDocumentReader(string fileName)
  {
    if (!File.Exists(fileName))
      throw new FileNotFoundException(nameof (fileName));
    this.xpsFile = new ZipArchive();
    this.xpsFile.Open(fileName);
  }

  internal OXPSDocumentReader(Stream stream)
  {
    if (!stream.CanRead && !stream.CanSeek)
      throw new ArgumentException("Unable to open the specified document stream");
    this.xpsFile = new ZipArchive();
    this.xpsFile.Open(stream, true);
  }

  internal OXPSFixedDocumentSequence DocumentSequence => this.documentSequence;

  internal OXPSFixedDocument[] Documents
  {
    get
    {
      return this.documents != null ? this.documents.ToArray() : throw new ArgumentOutOfRangeException("FixedDocuments");
    }
  }

  internal OXPSFixedPage[] Pages
  {
    get
    {
      return this.pages != null ? this.pages.ToArray() : throw new ArgumentOutOfRangeException("FixedPages");
    }
  }

  internal Dictionary<string, PdfTrueTypeFont> Fonts
  {
    get
    {
      if (this.m_fonts == null)
        this.m_fonts = new Dictionary<string, PdfTrueTypeFont>();
      return this.m_fonts;
    }
  }

  internal void Read() => this.ReadFixedDocumentSequence();

  private void ReadFixedDocumentSequence()
  {
    this.documentSequence = (OXPSFixedDocumentSequence) this.ReadElement(this.FixedDocumentSequence, typeof (OXPSFixedDocumentSequence));
    this.ReadFixedDocuments();
  }

  private void ReadFixedDocuments()
  {
    this.documents = new List<OXPSFixedDocument>();
    foreach (OXPSDocumentReference documentReference in this.documentSequence.DocumentReference)
      this.documents.Add((OXPSFixedDocument) this.ReadElement(this.GetSafeName(documentReference.Source), typeof (OXPSFixedDocument)));
    this.ReadFixedPages();
  }

  internal Stream ReadImage(string elementName) => this.ReadElement(elementName);

  private void ReadFixedPages()
  {
    this.pages = new List<OXPSFixedPage>();
    foreach (OXPSFixedDocument document in this.documents)
    {
      foreach (OXPSPageContent oxpsPageContent in document.PageContent)
        this.pages.Add((OXPSFixedPage) this.ReadElement(this.GetSafeName(oxpsPageContent.Source), typeof (OXPSFixedPage)));
    }
  }

  private object ReadElement(Regex pattern, Type elementType)
  {
    int index1 = 0;
    if (pattern.ToString() == "fdseq")
      index1 = this.xpsFile.Find(elementType.Name + ".fdseq");
    if (index1 == -1)
      index1 = this.xpsFile.Find(pattern);
    if (index1 == -1)
      throw new ArgumentException("Element not found : " + pattern.ToString());
    using (ZipArchiveItem zipArchiveItem1 = this.xpsFile.Items[index1])
    {
      if (System.IO.Path.GetExtension(zipArchiveItem1.ItemName) == ".piece")
      {
        pattern = new Regex(pattern.ToString() + "/");
        int index2 = this.xpsFile.Find(pattern);
        if (index2 == -1)
          throw new ArgumentException("Element not found : " + pattern.ToString());
        string s = string.Empty;
        for (; index2 != -1; index2 = this.Find(pattern, index2 + 1))
        {
          using (ZipArchiveItem zipArchiveItem2 = this.xpsFile.Items[index2])
          {
            zipArchiveItem2.DataStream.Position = 0L;
            StreamReader streamReader = new StreamReader(zipArchiveItem2.DataStream);
            string end = streamReader.ReadToEnd();
            s += !string.IsNullOrEmpty(end) ? end : "";
            streamReader.Dispose();
            int length = s.LastIndexOf('>') + 1;
            if (length < s.Length)
              s = s.Substring(0, length);
          }
        }
        MemoryStream memoryStream = new MemoryStream(Encoding.Default.GetBytes(s));
        return new XmlSerializer(elementType).Deserialize((Stream) memoryStream);
      }
      zipArchiveItem1.DataStream.Position = 0L;
      using (TextReader textReader = (TextReader) new StreamReader(zipArchiveItem1.DataStream))
        return new XmlSerializer(elementType).Deserialize(textReader);
    }
  }

  private object ReadElement(string elementName, Type elementType)
  {
    int index1 = this.Find(elementName);
    if (elementName.Contains("1.fpage"))
    {
      for (int index2 = this.Find(elementName) + 1; index2 < this.xpsFile.Items.Length; ++index2)
      {
        if (this.xpsFile.Items[index2].ItemName != null && this.xpsFile.Items[index2].ItemName.Contains("1.fpage"))
        {
          this.isfPageMoreThanOnce = true;
          break;
        }
      }
    }
    if (index1 == -1)
      throw new ArgumentException("Element not found : " + elementName);
    ZipArchiveItem zipArchiveItem1 = this.xpsFile.Items[index1];
    if (System.IO.Path.GetExtension(zipArchiveItem1.ItemName) == ".piece")
    {
      string s = string.Empty;
      while (index1 != -1)
      {
        using (ZipArchiveItem zipArchiveItem2 = this.xpsFile[index1])
        {
          zipArchiveItem2.DataStream.Position = 0L;
          StreamReader streamReader = new StreamReader(zipArchiveItem2.DataStream);
          s += streamReader.ReadToEnd();
          streamReader.Dispose();
          int length = s.LastIndexOf('>') + 1;
          if (length < s.Length)
            s = s.Substring(0, length);
          index1 = this.Find(elementName, index1 + 1);
        }
      }
      MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(s));
      return new XmlSerializer(elementType).Deserialize((Stream) memoryStream);
    }
    zipArchiveItem1.DataStream.Position = 0L;
    TextReader textReader = (TextReader) new StreamReader(zipArchiveItem1.DataStream);
    object obj = new XmlSerializer(elementType).Deserialize(textReader);
    if (obj is OXPSFixedDocument && this.isfPageMoreThanOnce)
    {
      OXPSFixedDocument oxpsFixedDocument = obj as OXPSFixedDocument;
      for (int index3 = 0; index3 < oxpsFixedDocument.PageContent.Length; ++index3)
        oxpsFixedDocument.PageContent[index3].Source = elementName.Replace("FixedDocument.fdoc", "") + oxpsFixedDocument.PageContent[index3].Source;
    }
    return obj;
  }

  private Stream ReadElement(string elementName)
  {
    int index = this.Find(elementName);
    if (index == -1)
      throw new ArgumentException("Element not found : " + elementName);
    return this.xpsFile.Items[index].DataStream;
  }

  private string GetSafeName(string elementName) => elementName.TrimStart('/', '.');

  internal int Find(string elementName) => this.Find(elementName, 0);

  internal int Find(string elementName, int startIndex)
  {
    if (string.IsNullOrEmpty(elementName))
      return -1;
    int num = this.xpsFile.Find(elementName);
    if (num == -1)
    {
      elementName = this.GetSafeName(elementName);
      for (int index = startIndex; index < this.xpsFile.Items.Length; ++index)
      {
        if (this.xpsFile.Items[index].ItemName != null && this.xpsFile.Items[index].ItemName.IndexOf(elementName, StringComparison.OrdinalIgnoreCase) >= 0 && this.xpsFile.Items[index].OriginalSize > 0L)
        {
          num = index;
          break;
        }
      }
    }
    return num;
  }

  internal int Find(Regex element, int startIndex)
  {
    if (element == null)
      return -1;
    for (int index = startIndex; index < this.xpsFile.Items.Length; ++index)
    {
      if (element.IsMatch(this.xpsFile.Items[index].ItemName) && this.xpsFile.Items[index].OriginalSize > 0L)
        return index;
    }
    return -1;
  }

  internal Stream ReadFont(string fontUri)
  {
    MemoryStream outStream = new MemoryStream();
    int index = this.Find(fontUri);
    string[] strArray = fontUri.Split('.');
    if (strArray[strArray.Length - 1].ToLower() == "odttf")
    {
      int startIndex = fontUri.LastIndexOf('/') + 1;
      int length = fontUri.LastIndexOf('.') - startIndex;
      string fontGuid = new Guid(fontUri.Substring(startIndex, length)).ToString("N");
      Stream dataStream = this.xpsFile.Items[index].DataStream;
      dataStream.Position = 0L;
      this.DeObfuscateFont(dataStream, (Stream) outStream, fontGuid);
      return (Stream) outStream;
    }
    Stream dataStream1 = this.xpsFile.Items[index].DataStream;
    dataStream1.Position = 0L;
    int length1 = (int) dataStream1.Length;
    byte[] buffer = new byte[length1];
    dataStream1.Read(buffer, 0, length1);
    return (Stream) new MemoryStream(buffer);
  }

  internal FontStyle GetDeviceFontStyle(string fontUri)
  {
    FontStyle deviceFontStyle = FontStyle.Regular;
    MemoryStream memoryStream = new MemoryStream();
    int index = this.Find(fontUri);
    int startIndex = fontUri.LastIndexOf('/') + 1;
    int length = fontUri.LastIndexOf('.') - startIndex;
    string fontGuid = new Guid(fontUri.Substring(startIndex, length)).ToString("N");
    Stream dataStream = this.xpsFile.Items[index].DataStream;
    dataStream.Position = 0L;
    this.DeObfuscateFont(dataStream, (Stream) memoryStream, fontGuid);
    using (BinaryReader reader = new BinaryReader((Stream) memoryStream))
    {
      TtfReader ttfReader = new TtfReader(reader);
      if (ttfReader.Metrics.IsBold && ttfReader.Metrics.IsItalic)
        deviceFontStyle = FontStyle.Bold | FontStyle.Italic;
      else if (ttfReader.Metrics.IsBold)
        deviceFontStyle = FontStyle.Bold;
      else if (ttfReader.Metrics.IsItalic)
        deviceFontStyle = FontStyle.Italic;
    }
    return deviceFontStyle;
  }

  private void DeObfuscateFont(Stream font, Stream outStream, string fontGuid)
  {
    byte[] numArray = new byte[16 /*0x10*/];
    for (int index = 0; index < numArray.Length; ++index)
      numArray[index] = Convert.ToByte(fontGuid.Substring(index * 2, 2), 16 /*0x10*/);
    byte[] buffer1 = new byte[32 /*0x20*/];
    font.Read(buffer1, 0, 32 /*0x20*/);
    for (int index1 = 0; index1 < 32 /*0x20*/; ++index1)
    {
      int index2 = numArray.Length - index1 % numArray.Length - 1;
      buffer1[index1] = (byte) ((uint) buffer1[index1] ^ (uint) numArray[index2]);
    }
    outStream.Write(buffer1, 0, 32 /*0x20*/);
    byte[] buffer2 = new byte[4096 /*0x1000*/];
    int count;
    while ((count = font.Read(buffer2, 0, 4096 /*0x1000*/)) > 0)
      outStream.Write(buffer2, 0, count);
  }

  internal Stream ReadResource(string elementName)
  {
    int index = this.Find(elementName);
    if (index == -1)
      throw new Exception("Specified resource not found.");
    Stream dataStream1 = this.xpsFile.Items[index].DataStream;
    string str = "</ResourceDictionary>";
    while (true)
    {
      byte[] numArray = new byte[str.Length + 2];
      dataStream1.Position = dataStream1.Length - (long) numArray.Length;
      dataStream1.Read(numArray, 0, numArray.Length);
      if (!Encoding.Default.GetString(numArray).Contains(str))
      {
        index = this.Find(elementName, index + 1);
        Stream dataStream2 = this.xpsFile.Items[index].DataStream;
        byte[] buffer = new byte[dataStream2.Length];
        dataStream2.Read(buffer, 0, (int) dataStream2.Length);
        dataStream1.Position = dataStream1.Length;
        dataStream1.Write(buffer, 0, buffer.Length);
      }
      else
        break;
    }
    dataStream1.Position = 0L;
    return dataStream1;
  }

  public void Dispose()
  {
    this.xpsFile.Dispose();
    this.documentSequence = (OXPSFixedDocumentSequence) null;
    if (this.documents != null)
      this.documents.Clear();
    if (this.pages == null)
      return;
    this.pages.Clear();
  }
}
