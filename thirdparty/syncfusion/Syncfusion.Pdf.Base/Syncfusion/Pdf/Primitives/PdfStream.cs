// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Primitives.PdfStream
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Compression;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Primitives;

internal class PdfStream : PdfDictionary, IPdfDecryptable
{
  private const string Prefix = "stream";
  private const string Suffix = "endstream";
  private MemoryStream m_dataStream;
  private bool m_blockEncryption;
  private bool m_bDecrypted;
  private bool m_bCompress;
  private bool m_bEncrypted;
  private PdfStream m_clonedObject;
  internal bool isCustomQuality;
  internal bool isImageDualFilter;

  internal MemoryStream InternalStream
  {
    get => this.m_dataStream;
    set => this.m_dataStream = value;
  }

  internal byte[] Data
  {
    get => this.m_dataStream.ToArray();
    set
    {
      this.m_dataStream.SetLength(0L);
      this.m_dataStream.Write(value, 0, value.Length);
      this.Modify();
    }
  }

  internal bool Compress
  {
    get => this.m_bCompress;
    set
    {
      this.m_bCompress = value;
      this.Modify();
    }
  }

  public override IPdfPrimitive ClonedObject => (IPdfPrimitive) this.m_clonedObject;

  internal PdfStream()
  {
    this.m_dataStream = new MemoryStream(100);
    this.m_bCompress = true;
  }

  internal PdfStream(PdfDictionary dictionary, byte[] data)
    : base(dictionary)
  {
    this.m_dataStream = new MemoryStream(data.Length);
    this.Data = data;
    this.m_bCompress = false;
    this["Length"] = (IPdfPrimitive) new PdfNumber(data.Length);
    if (this.ContainsKey("Length3") && this.ContainsKey("Filter") && this.m_bDecrypted)
    {
      string filterName = this.GetFilterName((PdfDictionary) this);
      if (filterName != null && (object) (this["Length3"] as PdfReferenceHolder) != null)
      {
        byte[] bytes = this.Decompress(data, filterName);
        string[] strArray1 = Encoding.UTF8.GetString(bytes, 0, bytes.Length).Split(new string[1]
        {
          "eexec"
        }, StringSplitOptions.None);
        string empty = string.Empty;
        for (int index = 0; index < 32 /*0x20*/; ++index)
          empty += "0";
        int num1 = strArray1[0].Length + 7;
        if (strArray1.Length > 1)
        {
          string[] strArray2 = strArray1[1].Split(new string[1]
          {
            empty
          }, StringSplitOptions.None);
          int num2 = 0;
          for (int index = 1; index < strArray2.Length; ++index)
            num2 += strArray2[index].Length;
          int num3 = 32 /*0x20*/ * (strArray2.Length - 1) + num2;
          Encoding.UTF8.GetBytes(strArray2[0]);
          this["Length1"] = (IPdfPrimitive) new PdfNumber(num1);
          this["Length2"] = (IPdfPrimitive) new PdfNumber(bytes.Length - num1 - num3);
          this["Length3"] = (IPdfPrimitive) new PdfNumber(num3);
        }
      }
    }
    if (!this.ContainsKey("Length1") || !this.ContainsKey("Filter") || (object) (this["Length1"] as PdfReferenceHolder) == null || !(this["Filter"] is PdfArray))
      return;
    this["Length1"] = (IPdfPrimitive) new PdfNumber(data.Length);
  }

  public static byte[] StreamToBytes(Stream stream)
  {
    return stream != null ? PdfStream.StreamToBytes(stream, false) : throw new ArgumentNullException(nameof (stream));
  }

  public static byte[] StreamToBytes(Stream stream, bool writeWholeStream)
  {
    long num1 = stream != null ? stream.Position : throw new ArgumentNullException(nameof (stream));
    long num2 = stream.Position != 0L ? stream.Position : stream.Length;
    long count = writeWholeStream ? stream.Length : num2;
    byte[] buffer = new byte[count];
    stream.Position = 0L;
    stream.Read(buffer, 0, (int) count);
    stream.Position = num1;
    return buffer;
  }

  public static byte[] StreamToBigEndian(Stream stream)
  {
    return Encoding.Convert(Encoding.Unicode, Encoding.BigEndianUnicode, PdfStream.StreamToBytes(stream));
  }

  internal void Write(char symbol) => this.Write(symbol.ToString());

  internal void Write(string text)
  {
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    if (text.Length <= 0)
      throw new ArgumentException("Can't write an empty string.", nameof (text));
    this.Write(Encoding.UTF8.GetBytes(text));
  }

  internal void Write(byte[] data)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    if (data.Length <= 0)
      throw new ArgumentException("Can't write an empty array.", nameof (data));
    this.m_dataStream.Write(data, 0, data.Length);
    this.Modify();
  }

  internal void BlockEncryption() => this.m_blockEncryption = true;

  internal void Decompress()
  {
    IPdfPrimitive pdfPrimitive1 = this["Filter"];
    if ((object) (pdfPrimitive1 as PdfReferenceHolder) != null)
      pdfPrimitive1 = (pdfPrimitive1 as PdfReferenceHolder).Object;
    if (pdfPrimitive1 != null)
    {
      if ((object) (pdfPrimitive1 as PdfName) != null)
      {
        PdfName pdfName = pdfPrimitive1 as PdfName;
        this.Data = !(pdfName.Value == "ASCIIHexDecode") ? this.Decompress(this.Data, pdfName.Value) : this.Decode(this.Data);
      }
      else
      {
        if (!(pdfPrimitive1 is PdfArray))
          throw new PdfDocumentException("Invalid/Unknown/Unsupported formatUnexpected object for filter.");
        foreach (IPdfPrimitive pdfPrimitive2 in pdfPrimitive1 as PdfArray)
        {
          string filter = (pdfPrimitive2 as PdfName).Value;
          switch (filter)
          {
            case null:
              throw new PdfDocumentException("Invalid/Unknown/Unsupported format");
            case "ASCIIHexDecode":
              this.Data = this.Decode(this.Data);
              continue;
            default:
              this.Data = this.Decompress(this.Data, filter);
              continue;
          }
        }
      }
    }
    this.Remove("Filter");
    this.m_bCompress = true;
  }

  internal byte[] GetDecompressedData()
  {
    byte[] decompressedData = this.Data;
    IPdfPrimitive pdfPrimitive1 = this["Filter"];
    if ((object) (pdfPrimitive1 as PdfReferenceHolder) != null)
      pdfPrimitive1 = (pdfPrimitive1 as PdfReferenceHolder).Object;
    if (pdfPrimitive1 != null)
    {
      if ((object) (pdfPrimitive1 as PdfName) != null)
      {
        decompressedData = this.Decompress(this.Data, (pdfPrimitive1 as PdfName).Value);
      }
      else
      {
        if (!(pdfPrimitive1 is PdfArray))
          throw new PdfDocumentException("Invalid/Unknown/Unsupported formatUnexpected object for filter.");
        foreach (IPdfPrimitive pdfPrimitive2 in pdfPrimitive1 as PdfArray)
        {
          string filter = (pdfPrimitive2 as PdfName).Value;
          switch (filter)
          {
            case null:
              throw new PdfDocumentException("Invalid/Unknown/Unsupported format");
            case "ASCIIHexDecode":
              decompressedData = this.Decode(this.Data);
              continue;
            default:
              decompressedData = this.Decompress(this.Data, filter);
              continue;
          }
        }
      }
    }
    return decompressedData;
  }

  private byte HexToDecimalConversion(char hex)
  {
    if (hex >= '0' && hex <= '9')
      return (byte) ((uint) hex - 48U /*0x30*/);
    return hex >= 'a' && hex <= 'f' ? (byte) ((int) hex - 97 + 10) : (byte) ((int) hex - 65 + 10);
  }

  private byte[] Decode(byte[] inputData)
  {
    inputData = this.RemoveWhiteSpace(inputData);
    List<byte> byteList = new List<byte>(inputData.Length);
    int index1 = 0;
    while (index1 < inputData.Length)
    {
      int index2 = index1 + 1;
      byte hex1;
      if ((hex1 = inputData[index1]) != (byte) 62)
      {
        byte hex2 = inputData[index2];
        index1 = index2 + 1;
        if (hex2 == (byte) 62)
          hex2 = (byte) 48 /*0x30*/;
        byte num = (byte) ((uint) this.HexToDecimalConversion((char) hex1) << 4 | (uint) this.HexToDecimalConversion((char) hex2));
        byteList.Add(num);
      }
      else
        break;
    }
    byte[] array = byteList.ToArray();
    byteList.Clear();
    return array;
  }

  private byte[] RemoveWhiteSpace(byte[] data)
  {
    List<byte> byteList = new List<byte>(data.Length);
    for (int index = 0; index < data.Length; ++index)
    {
      if (!char.IsWhiteSpace((char) data[index]))
        byteList.Add(data[index]);
    }
    byte[] array = byteList.ToArray();
    byteList.Clear();
    return array;
  }

  internal new void Clear()
  {
    if (this.InternalStream != null && this.InternalStream.CanWrite)
    {
      this.InternalStream.SetLength(0L);
      this.InternalStream.Position = 0L;
    }
    this.Remove("Filter");
    this.m_bCompress = true;
    this.Modify();
  }

  internal void Dispose()
  {
    if (this.InternalStream == null)
      return;
    this.InternalStream.Dispose();
    this.InternalStream.Close();
    this.InternalStream = (MemoryStream) null;
    this.Items.Clear();
    this.m_bCompress = true;
    this.Modify();
  }

  public override void Save(IPdfWriter writer)
  {
    this.OnBeginSave(new SavePdfPrimitiveEventArgs(writer));
    byte[] data = this.CompressContent(writer);
    PdfSecurity security = writer.Document.Security;
    if (security != null && security.Encryptor != null && security.Encryptor.Encrypt && security.Encryptor.EncryptOnlyAttachment)
    {
      bool flag = false;
      if (this.ContainsKey("Type"))
      {
        PdfName pdfName1 = this["Type"] as PdfName;
        if (pdfName1 != (PdfName) null && pdfName1.Value == "EmbeddedFile")
        {
          PdfArray pdfArray = this["Filter"] as PdfArray;
          PdfName pdfName2 = this["Filter"] as PdfName;
          if (pdfArray == null || pdfArray != null && !pdfArray.Contains((IPdfPrimitive) new PdfName("Crypt")))
          {
            if (this.m_bCompress || !this.ContainsKey("Filter") || pdfArray != null && !pdfArray.Contains((IPdfPrimitive) new PdfName("FlateDecode")) || pdfName2 != (PdfName) null && pdfName2.Value != "FlateDecode")
              data = this.CompressStream();
            flag = true;
            data = this.EncryptContent(data, writer);
            this.AddFilter("Crypt");
          }
          if (!this.ContainsKey("DecodeParms"))
          {
            PdfArray primitive = new PdfArray();
            primitive.Add((IPdfPrimitive) new PdfDictionary()
            {
              [new PdfName("Name")] = (IPdfPrimitive) new PdfName("StdCF")
            });
            PdfNull element = new PdfNull();
            primitive.Add((IPdfPrimitive) element);
            this.SetProperty("DecodeParms", (IPdfPrimitive) primitive);
          }
        }
      }
      if (!flag)
      {
        if (this.ContainsKey("DecodeParms"))
        {
          if (this["DecodeParms"] is PdfArray pdfArray1 && pdfArray1[0] is PdfDictionary pdfDictionary)
          {
            PdfName pdfName = pdfDictionary["Name"] as PdfName;
            if (pdfName != (PdfName) null && pdfName.Value == "StdCF" && (!(this["Filter"] is PdfArray pdfArray) || pdfArray != null && !pdfArray.Contains((IPdfPrimitive) new PdfName("Crypt"))))
            {
              if (this.m_bCompress)
                data = this.CompressStream();
              data = this.EncryptContent(data, writer);
              this.AddFilter("Crypt");
            }
          }
        }
        else if (this.ContainsKey("DL"))
        {
          if (this.m_bCompress)
            data = this.CompressStream();
          data = this.EncryptContent(data, writer);
          this.AddFilter("Crypt");
          if (!this.ContainsKey("DecodeParms"))
          {
            PdfArray primitive = new PdfArray();
            primitive.Add((IPdfPrimitive) new PdfDictionary()
            {
              [new PdfName("Name")] = (IPdfPrimitive) new PdfName("StdCF")
            });
            PdfNull element = new PdfNull();
            primitive.Add((IPdfPrimitive) element);
            this.SetProperty("DecodeParms", (IPdfPrimitive) primitive);
          }
        }
      }
    }
    else if (security != null && security.Encryptor != null && !security.Encryptor.EncryptMetaData && this.ContainsKey("Type"))
    {
      PdfName pdfName = this["Type"] as PdfName;
      if (pdfName == (PdfName) null || pdfName != (PdfName) null && pdfName.Value != "Metadata".ToString())
        data = this.EncryptContent(data, writer);
    }
    else
      data = this.EncryptContent(data, writer);
    this["Length"] = (IPdfPrimitive) new PdfNumber(data.Length);
    if (this.ContainsKey("Length1") && this.ContainsKey("Filter") && (object) (this["Length1"] as PdfReferenceHolder) != null && this["Filter"] is PdfArray)
      this["Length1"] = (IPdfPrimitive) new PdfNumber(data.Length);
    if (this.ContainsKey("Length3") && this.ContainsKey("Filter") && !this.m_bEncrypted)
    {
      string filterName = this.GetFilterName((PdfDictionary) this);
      if (filterName != null && (object) (this["Length3"] as PdfReferenceHolder) != null)
      {
        byte[] bytes = this.Decompress(data, filterName);
        string[] strArray1 = Encoding.UTF8.GetString(bytes, 0, bytes.Length).Split(new string[1]
        {
          "eexec"
        }, StringSplitOptions.None);
        string empty = string.Empty;
        for (int index = 0; index < 32 /*0x20*/; ++index)
          empty += "0";
        int num1 = strArray1[0].Length + 7;
        if (strArray1.Length > 1)
        {
          string[] strArray2 = strArray1[1].Split(new string[1]
          {
            empty
          }, StringSplitOptions.None);
          int num2 = 0;
          for (int index = 1; index < strArray2.Length; ++index)
            num2 += strArray2[index].Length;
          int num3 = 32 /*0x20*/ * (strArray2.Length - 1) + num2;
          Encoding.UTF8.GetBytes(strArray2[0]);
          this["Length1"] = (IPdfPrimitive) new PdfNumber(num1);
          this["Length2"] = (IPdfPrimitive) new PdfNumber(bytes.Length - num1 - num3);
          this["Length3"] = (IPdfPrimitive) new PdfNumber(num3);
        }
      }
    }
    this.Save(writer, false);
    writer.Write("stream");
    writer.Write("\r\n");
    if (data.Length > 0)
    {
      writer.Write(data);
      writer.Write("\r\n");
    }
    writer.Write("endstream");
    writer.Write("\r\n");
    this.OnEndSave(new SavePdfPrimitiveEventArgs(writer));
    if (!this.m_bCompress)
      return;
    if (this.isImageDualFilter)
      this.RemoveFilter();
    else
      this.Remove("Filter");
  }

  private void RemoveFilter()
  {
    IPdfPrimitive pdfPrimitive = this["Filter"];
    if ((object) (pdfPrimitive as PdfReferenceHolder) != null)
      pdfPrimitive = (pdfPrimitive as PdfReferenceHolder).Object;
    if (pdfPrimitive is PdfArray)
    {
      PdfName element = new PdfName("FlateDecode");
      PdfArray pdfArray = pdfPrimitive as PdfArray;
      if (!pdfArray.Contains((IPdfPrimitive) element))
        return;
      pdfArray.Remove((IPdfPrimitive) element);
    }
    else
    {
      if ((object) (pdfPrimitive as PdfName) == null)
        return;
      this.Remove("Filter");
    }
  }

  private byte[] CompressStream()
  {
    bool flag = false;
    byte[] numArray = this.Data;
    if (numArray != null)
    {
      numArray = new PdfZlibCompressor(PdfCompressionLevel.Best).Compress(numArray);
      flag = true;
    }
    this.Clear();
    this.Compress = false;
    if (numArray != null)
    {
      this.InternalStream.Write(numArray, 0, numArray.Length);
      if (flag)
        this.AddFilter("FlateDecode");
    }
    return numArray;
  }

  public override IPdfPrimitive Clone(PdfCrossTable crossTable)
  {
    if (this.m_clonedObject != null && this.m_clonedObject.CrossTable == crossTable)
      return (IPdfPrimitive) this.m_clonedObject;
    this.m_clonedObject = (PdfStream) null;
    PdfStream pdfStream = new PdfStream(base.Clone(crossTable) as PdfDictionary, this.m_dataStream.ToArray());
    pdfStream.Compress = this.m_bCompress;
    pdfStream.m_bDecrypted = this.m_bDecrypted;
    this.m_clonedObject = pdfStream;
    return (IPdfPrimitive) pdfStream;
  }

  public bool WasEncrypted => throw new Exception("The method or operation is not implemented.");

  public bool Decrypted => this.m_bDecrypted;

  public void Decrypt(PdfEncryptor encryptor, long currObjNumber)
  {
    if (encryptor == null || this.m_bDecrypted)
      return;
    this.m_bDecrypted = true;
    this.Data = encryptor.EncryptData(currObjNumber, this.Data, false);
  }

  private byte[] Decompress(byte[] data, string filter)
  {
    if (data == null)
      throw new ArgumentNullException(nameof (data));
    if (filter == null)
      throw new ArgumentNullException(nameof (filter));
    if (data.Length == 0 || !(filter != "Crypt"))
      return data;
    if (filter.Equals("RunLengthDecode"))
    {
      Stream stream = (Stream) new MemoryStream(data);
      MemoryStream memoryStream = new MemoryStream();
      byte[] buffer = new byte[128 /*0x80*/];
      int num1;
      while ((num1 = stream.ReadByte()) != -1 && num1 != 128 /*0x80*/)
      {
        if (num1 <= (int) sbyte.MaxValue)
        {
          int count1 = num1 + 1;
          int count2;
          for (; count1 > 0; count1 -= count2)
          {
            count2 = stream.Read(buffer, 0, count1);
            memoryStream.Write(buffer, 0, count2);
          }
        }
        else
        {
          int num2 = stream.ReadByte();
          for (int index = 0; index < 257 - num1; ++index)
            memoryStream.WriteByte((byte) num2);
        }
      }
      memoryStream.Position = 0L;
      return memoryStream.ToArray();
    }
    IPdfCompressor compressor = this.DetermineCompressor(filter);
    if (!(filter == "FlateDecode"))
      return this.PostProcess(compressor.Decompress(data), filter);
    try
    {
      return this.PostProcess(compressor.Decompress(data), filter);
    }
    catch (Exception ex)
    {
      return data;
    }
  }

  private IPdfCompressor DetermineCompressor(string filter)
  {
    if (filter == null)
      throw new ArgumentNullException(nameof (filter));
    switch (filter)
    {
      case "FlateDecode":
      case "Fl":
        return (IPdfCompressor) new PdfZlibCompressor();
      case "LZWDecode":
      case "LZW":
        return (IPdfCompressor) new PdfLzwCompressor();
      case "ASCII85Decode":
      case "A85":
        return (IPdfCompressor) new PdfASCII85Compressor();
      default:
        throw new PdfDocumentException($"Invalid/Unknown/Unsupported format Unsupported compressor ({filter}).");
    }
  }

  private byte[] PostProcess(byte[] data, string filter)
  {
    if (!(filter == "FlateDecode"))
      return data;
    IPdfPrimitive pdfPrimitive1 = this["DecodeParms"];
    if (pdfPrimitive1 == null)
      return data;
    PdfDictionary pdfDictionary1 = pdfPrimitive1 as PdfDictionary;
    PdfArray pdfArray = pdfPrimitive1 as PdfArray;
    PdfNull pdfNull = pdfPrimitive1 as PdfNull;
    if ((object) (pdfPrimitive1 as PdfReferenceHolder) != null)
    {
      pdfDictionary1 = PdfCrossTable.Dereference(pdfPrimitive1) as PdfDictionary;
      pdfArray = PdfCrossTable.Dereference(pdfPrimitive1) as PdfArray;
      pdfNull = PdfCrossTable.Dereference(pdfPrimitive1) as PdfNull;
    }
    if (pdfNull != null)
      return data;
    if (pdfDictionary1 == null && pdfArray == null)
      throw new PdfDocumentException("Invalid/Unknown/Unsupported format");
    if (pdfArray != null && pdfArray.Elements.Count > 0 && pdfArray[0] is PdfDictionary pdfDictionary2)
    {
      PdfName pdfName = pdfDictionary2["Name"] as PdfName;
      if (pdfName != (PdfName) null && pdfName.Value == "StdCF")
        return data;
    }
    int num1 = 1;
    if (pdfDictionary1 != null)
    {
      if (pdfDictionary1["Predictor"] is PdfNumber pdfNumber)
        num1 = pdfNumber.IntValue;
    }
    else if (pdfArray != null && pdfArray.Count > 0)
      num1 = !(pdfArray[0] is PdfDictionary pdfDictionary3) || !pdfDictionary3.ContainsKey("Predictor") ? 1 : pdfDictionary3.GetInt("Predictor");
    if (num1 == 1)
      return data;
    if (num1 == 2)
      throw new PdfDocumentException("Unsupported predictor: TIFF 2.");
    if (num1 >= 16 /*0x10*/ || num1 <= 2)
      throw new PdfDocumentException("Invalid/Unknown/Unsupported format Unknown predictor code: " + num1.ToString());
    int num2 = 1;
    int num3 = 1;
    IPdfPrimitive pdfPrimitive2 = pdfDictionary1["Colors"];
    if (pdfPrimitive2 != null)
      num2 = (pdfPrimitive2 as PdfNumber).IntValue;
    IPdfPrimitive pdfPrimitive3 = pdfDictionary1["Columns"];
    if (pdfPrimitive3 != null)
      num3 = (pdfPrimitive3 as PdfNumber).IntValue;
    IPdfPrimitive pdfPrimitive4 = pdfDictionary1["BitsPerComponent"];
    if (pdfPrimitive4 != null)
    {
      int intValue = (pdfPrimitive4 as PdfNumber).IntValue;
    }
    return PdfPngFilter.Decompress(data, num2 * num3);
  }

  private void NormalizeFilter()
  {
    if (!(this["Filter"] is PdfArray pdfArray) || pdfArray.Count != 1)
      return;
    this["Filter"] = pdfArray[0];
  }

  private byte[] CompressContent(IPdfWriter writer)
  {
    PdfCompressionLevel level = writer.Document.Compression;
    if (writer.Document.isCompressed)
      level = PdfCompressionLevel.Best;
    bool flag = level != PdfCompressionLevel.None;
    byte[] data = this.Data;
    if (flag && this.m_bCompress)
    {
      data = new PdfZlibCompressor(level).Compress(data);
      this.AddFilter("FlateDecode");
    }
    return data;
  }

  internal void AddFilter(string filterName)
  {
    IPdfPrimitive pdfPrimitive = this["Filter"];
    if ((object) (pdfPrimitive as PdfReferenceHolder) != null)
      pdfPrimitive = (pdfPrimitive as PdfReferenceHolder).Object;
    PdfArray pdfArray = pdfPrimitive as PdfArray;
    PdfName element1 = pdfPrimitive as PdfName;
    if (element1 != (PdfName) null)
    {
      pdfArray = new PdfArray();
      pdfArray.Insert(0, (IPdfPrimitive) element1);
      this["Filter"] = (IPdfPrimitive) pdfArray;
    }
    PdfName element2 = new PdfName(filterName);
    if (pdfArray == null)
      this["Filter"] = (IPdfPrimitive) element2;
    else
      pdfArray.Insert(0, (IPdfPrimitive) element2);
  }

  private byte[] EncryptContent(byte[] data, IPdfWriter writer)
  {
    PdfDocumentBase document = writer.Document;
    PdfEncryptor encryptor = document.Security.Encryptor;
    if (encryptor.Encrypt && !this.m_blockEncryption)
    {
      this.m_bEncrypted = true;
      data = encryptor.EncryptData(document.CurrentSavingObj.ObjNum, data, true);
    }
    return data;
  }

  private string GetFilterName(PdfDictionary dictionary)
  {
    string filterName = (string) null;
    IPdfPrimitive pdfPrimitive1 = this["Filter"];
    if (pdfPrimitive1 != null && (object) (pdfPrimitive1 as PdfReferenceHolder) != null)
      pdfPrimitive1 = (pdfPrimitive1 as PdfReferenceHolder).Object;
    if (pdfPrimitive1 != null)
    {
      if ((object) (pdfPrimitive1 as PdfName) != null)
        filterName = (pdfPrimitive1 as PdfName).Value;
      else if (pdfPrimitive1 is PdfArray)
      {
        foreach (IPdfPrimitive pdfPrimitive2 in pdfPrimitive1 as PdfArray)
          filterName = (pdfPrimitive2 as PdfName).Value;
      }
    }
    return filterName;
  }
}
