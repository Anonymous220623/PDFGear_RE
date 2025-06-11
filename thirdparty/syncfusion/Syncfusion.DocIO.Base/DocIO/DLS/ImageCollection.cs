// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.ImageCollection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.Compression;
using Syncfusion.DocIO.ReaderWriter.Security;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class ImageCollection
{
  private Dictionary<int, ImageRecord> m_collection = new Dictionary<int, ImageRecord>();
  private List<int> m_removedImageIds = new List<int>();
  private int m_maxId;
  private WordDocument m_doc;

  internal ImageRecord this[int imageId]
  {
    get => this.m_collection.ContainsKey(imageId) ? this.m_collection[imageId] : (ImageRecord) null;
  }

  internal WordDocument Document => this.m_doc;

  internal ImageCollection(WordDocument doc) => this.m_doc = doc;

  internal void Add(ImageRecord image)
  {
    if (image.IsAdded)
      return;
    int key = 1;
    if (this.m_removedImageIds.Count > 0)
    {
      key = this.m_removedImageIds[0];
      this.m_removedImageIds.RemoveAt(0);
    }
    else if (this.m_collection.Count > 0)
      key = ++this.m_maxId;
    else
      ++this.m_maxId;
    image.ImageId = key;
    image.IsAdded = true;
    this.m_collection.Add(key, image);
  }

  internal bool Remove(int imageId)
  {
    if (!this.m_collection.ContainsKey(imageId))
      return false;
    this.m_collection[imageId].IsAdded = false;
    this.m_collection.Remove(imageId);
    this.m_removedImageIds.Add(imageId);
    return true;
  }

  internal void Clear()
  {
    if (this.m_collection != null)
    {
      foreach (ImageRecord imageRecord in this.m_collection.Values)
        imageRecord.Close();
      this.m_collection.Clear();
      this.m_collection = (Dictionary<int, ImageRecord>) null;
    }
    if (this.m_removedImageIds != null)
    {
      this.m_removedImageIds.Clear();
      this.m_removedImageIds = (List<int>) null;
    }
    this.m_maxId = 0;
    this.m_doc = (WordDocument) null;
  }

  internal ImageRecord LoadImage(byte[] imageBytes)
  {
    HMACSHA1 hmacshA1 = ImageRecord.GetHMACSHA1();
    SecurityHelper securityHelper = new SecurityHelper();
    ImageRecord image = (ImageRecord) null;
    foreach (ImageRecord imageRecord in this.m_collection.Values)
    {
      if (!imageRecord.IsMetafile && imageRecord.m_imageBytes.Length == imageBytes.Length && securityHelper.CompareArray(imageRecord.ImageHash, hmacshA1.ComputeHash(imageBytes)))
      {
        image = imageRecord;
        break;
      }
    }
    if (image == null)
    {
      image = new ImageRecord(this.m_doc, imageBytes);
      this.Add(image);
    }
    imageBytes = (byte[]) null;
    ++image.OccurenceCount;
    return image;
  }

  internal ImageRecord LoadMetaFileImage(byte[] imageBytes, bool isCompressed)
  {
    int length = imageBytes.Length;
    if (!isCompressed)
      imageBytes = this.CompressImageBytes(imageBytes);
    HMACSHA1 hmacshA1 = ImageRecord.GetHMACSHA1();
    SecurityHelper securityHelper = new SecurityHelper();
    ImageRecord image = (ImageRecord) null;
    foreach (ImageRecord imageRecord in this.m_collection.Values)
    {
      if (imageRecord.IsMetafile && imageRecord.m_imageBytes.Length == imageBytes.Length && securityHelper.CompareArray(imageRecord.ImageHash, hmacshA1.ComputeHash(imageBytes)))
      {
        image = imageRecord;
        break;
      }
    }
    if (image == null)
    {
      image = new ImageRecord(this.m_doc, imageBytes);
      this.Add(image);
      if (!isCompressed)
        image.Length = length;
    }
    imageBytes = (byte[]) null;
    ++image.OccurenceCount;
    image.IsMetafile = true;
    return image;
  }

  internal ImageRecord LoadXmlItemImage(byte[] imageBytes)
  {
    return !(ImageRecord.GetImage(imageBytes) is Metafile) ? this.LoadImage(imageBytes) : this.LoadMetaFileImage(imageBytes, false);
  }

  internal byte[] CompressImageBytes(byte[] imageBytes)
  {
    byte[] array;
    try
    {
      MemoryStream outputStream = new MemoryStream();
      new CompressedStreamWriter((Stream) outputStream, true).Write(imageBytes, 0, imageBytes.Length, true);
      outputStream.Close();
      array = outputStream.ToArray();
    }
    catch
    {
      MemoryStream memoryStream = new MemoryStream();
      GZipStream gzipStream = new GZipStream((Stream) memoryStream, CompressionMode.Compress, true);
      gzipStream.Write(imageBytes, 0, imageBytes.Length);
      gzipStream.Close();
      array = memoryStream.ToArray();
      memoryStream.Close();
    }
    return array;
  }

  internal byte[] DecompressImageBytes(byte[] compressedImage)
  {
    byte[] numArray;
    try
    {
      MemoryStream memoryStream1 = new MemoryStream(compressedImage);
      CompressedStreamReader compressedStreamReader = new CompressedStreamReader((Stream) memoryStream1);
      MemoryStream memoryStream2 = new MemoryStream();
      byte[] buffer = new byte[4096 /*0x1000*/];
      while (true)
      {
        int count = compressedStreamReader.Read(buffer, 0, buffer.Length);
        if (count > 0)
          memoryStream2.Write(buffer, 0, count);
        else
          break;
      }
      memoryStream1.Close();
      numArray = memoryStream2.ToArray();
      memoryStream2.Close();
    }
    catch (Exception ex)
    {
      try
      {
        using (GZipStream gzipStream = new GZipStream((Stream) new MemoryStream(compressedImage), CompressionMode.Decompress, true))
        {
          byte[] buffer = new byte[4096 /*0x1000*/];
          using (MemoryStream memoryStream = new MemoryStream())
          {
            int count;
            do
            {
              count = gzipStream.Read(buffer, 0, buffer.Length);
              if (count > 0)
                memoryStream.Write(buffer, 0, count);
            }
            while (count > 0);
            numArray = memoryStream.ToArray();
          }
        }
      }
      catch
      {
        compressedImage = this.CompressImageBytes(compressedImage);
        numArray = this.DecompressImageBytes(compressedImage);
      }
    }
    return numArray;
  }
}
