// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.PdfSignatureDigest
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Native;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class PdfSignatureDigest
{
  private List<object> m_objList;
  private PdfPrimitiveId m_id;

  internal PdfSignatureDigest()
  {
    this.m_objList = new List<object>();
    this.m_id = new PdfPrimitiveId();
  }

  public byte[] HashDocument(PdfDocumentBase doc)
  {
    if (doc == null)
      throw new ArgumentNullException(nameof (doc));
    Md5_Ctx md5Ctx1 = new Md5_Ctx();
    Md5_Ctx md5Ctx2 = new Md5_Ctx();
    CryptoApi.MD5Init(ref md5Ctx1);
    CryptoApi.MD5Init(ref md5Ctx2);
    PdfCatalog catalog = doc.Catalog;
    this.HashDictionaryName((PdfDictionary) catalog, "AA", ref md5Ctx2, this.m_objList, false, false);
    this.HashDictionaryName((PdfDictionary) catalog, "Legal", ref md5Ctx2, this.m_objList, false, false);
    this.HashDictionaryName((PdfDictionary) catalog, "Perms", ref md5Ctx2, this.m_objList, false, false);
    CryptoApi.MD5Final(ref md5Ctx2);
    byte[] digest1 = md5Ctx2.digest;
    int length1 = digest1.Length;
    CryptoApi.MD5Update(ref md5Ctx1, digest1, length1);
    md5Ctx2 = new Md5_Ctx();
    CryptoApi.MD5Init(ref md5Ctx2);
    PdfDictionary dictionary = doc.DocumentInformation.Dictionary;
    this.HashDictionaryName(dictionary, "Title", ref md5Ctx2, this.m_objList, false, false);
    this.HashDictionaryName(dictionary, "Author", ref md5Ctx2, this.m_objList, false, false);
    this.HashDictionaryName(dictionary, "Keywords", ref md5Ctx2, this.m_objList, false, false);
    this.HashDictionaryName(dictionary, "Subject", ref md5Ctx2, this.m_objList, false, false);
    CryptoApi.MD5Final(ref md5Ctx2);
    byte[] digest2 = md5Ctx2.digest;
    int length2 = digest2.Length;
    CryptoApi.MD5Update(ref md5Ctx1, digest2, length2);
    md5Ctx2 = new Md5_Ctx();
    CryptoApi.MD5Init(ref md5Ctx2);
    CryptoApi.MD5Final(ref md5Ctx2);
    byte[] digest3 = md5Ctx2.digest;
    int length3 = digest3.Length;
    CryptoApi.MD5Update(ref md5Ctx1, digest3, length3);
    if (doc is PdfDocument pdfDocument)
    {
      this.HashPages(pdfDocument.Pages, ref md5Ctx1, this.m_objList);
      CryptoApi.MD5Update(ref md5Ctx1, digest3, length3);
      this.HashEmbeddedFiles(pdfDocument.Pages, ref md5Ctx1, this.m_objList);
      CryptoApi.MD5Update(ref md5Ctx1, digest3, length3);
    }
    CryptoApi.MD5Final(ref md5Ctx1);
    return md5Ctx1.digest;
  }

  public byte[] HashDocument(PdfWriter writer, int firstRangeEnd, int secondRangeStart)
  {
    writer.Position = (long) firstRangeEnd;
    writer.Write("<");
    writer.Position = (long) secondRangeStart;
    writer.Write(">");
    byte[] numArray1 = new byte[firstRangeEnd + 1];
    writer.Position = 0L;
    writer.ObtainStream().Read(numArray1, 0, numArray1.Length);
    byte[] numArray2 = new byte[writer.Length - (long) secondRangeStart];
    writer.Position = (long) secondRangeStart;
    writer.ObtainStream().Read(numArray2, 0, numArray2.Length);
    byte[] numArray3 = new byte[numArray1.Length + numArray2.Length];
    Array.Copy((Array) numArray1, (Array) numArray3, numArray1.Length);
    Array.Copy((Array) numArray2, 0, (Array) numArray3, numArray1.Length, numArray2.Length);
    File.WriteAllBytes("tt.pdf", numArray3);
    return SHA1.Create().ComputeHash(numArray3, 0, numArray3.Length);
  }

  public byte[] HashSignatureFields(PdfPage ipage)
  {
    byte[] numArray = new byte[16 /*0x10*/];
    if (ipage != null)
    {
      PdfArray annotations = ipage.Annotations.Annotations;
      if (annotations != null)
      {
        Md5_Ctx context = new Md5_Ctx();
        CryptoApi.MD5Init(ref context);
        int count = annotations.Count;
        Md5_Ctx md5Ctx = new Md5_Ctx();
        CryptoApi.MD5Init(ref md5Ctx);
        for (int index = 0; index < count; ++index)
        {
          object obj = (object) annotations[index];
          if ((object) (obj as PdfReferenceHolder) != null)
            obj = (object) (obj as PdfReferenceHolder).Object;
          if (obj is PdfSignature)
            this.HashField((obj as PdfSignature).Field, ref md5Ctx, this.m_objList);
        }
        CryptoApi.MD5Final(ref md5Ctx);
        byte[] digest = md5Ctx.digest;
        int length = digest.Length;
        CryptoApi.MD5Update(ref context, digest, length);
        CryptoApi.MD5Final(ref context);
        numArray = context.digest;
      }
    }
    return numArray;
  }

  private int HashDictionaryItem(
    PdfDictionary dic,
    string item,
    ref Md5_Ctx ctx,
    List<object> list)
  {
    object obj = (object) dic[item];
    if (obj == null)
      return 0;
    if ((object) (obj as PdfReferenceHolder) != null)
      obj = (object) (obj as PdfReferenceHolder).Object;
    CryptoApi.MD5Update(ref ctx, this.m_id.Name, 1);
    int length = item.Length;
    CryptoApi.MD5Update(ref ctx, this.LittleToBigEndian(BitConverter.GetBytes(length)), 4);
    byte[] input = PdfString.StringToByte(item);
    CryptoApi.MD5Update(ref ctx, input, length);
    this.HashObject(obj, ref ctx, list);
    return 1;
  }

  private bool HashDictionaryName(
    PdfDictionary dic,
    string name,
    ref Md5_Ctx ctx,
    List<object> list,
    bool isInheritable,
    bool isNull)
  {
    bool flag = false;
    object obj = (object) dic[name];
    if (dic != null)
    {
      while (obj == null)
      {
        if (isInheritable)
        {
          obj = (object) dic["Parent"];
          if ((object) (obj as PdfReferenceHolder) != null)
            obj = (object) (obj as PdfReferenceHolder).Object;
          dic = !(obj is PdfDictionary) ? (PdfDictionary) null : obj as PdfDictionary;
          if (dic == null)
            goto label_9;
        }
        else
          goto label_9;
      }
      if ((object) (obj as PdfReferenceHolder) != null)
        obj = (object) (obj as PdfReferenceHolder).Object;
      flag = true;
      this.HashObject(obj, ref ctx, list);
    }
label_9:
    if (!flag && isNull)
      CryptoApi.MD5Update(ref ctx, this.m_id.Null, 1);
    return flag;
  }

  private void HashObject(object obj, ref Md5_Ctx ctx, List<object> list)
  {
    if ((object) (obj as PdfReferenceHolder) != null)
      obj = (object) (obj as PdfReferenceHolder).Object;
    switch (obj)
    {
      case PdfNull _:
        CryptoApi.MD5Update(ref ctx, this.m_id.Null, 1);
        break;
      case PdfPage _:
        this.HashPage(obj as PdfPage, ref ctx, list);
        break;
      case PdfNumber _:
        PdfNumber pdfNumber = obj as PdfNumber;
        if (pdfNumber.IsInteger)
        {
          CryptoApi.MD5Update(ref ctx, this.m_id.Integer, 1);
          byte[] bigEndian = this.LittleToBigEndian(BitConverter.GetBytes(pdfNumber.IntValue));
          CryptoApi.MD5Update(ref ctx, bigEndian, 4);
          break;
        }
        if (pdfNumber.IsLong)
        {
          CryptoApi.MD5Update(ref ctx, this.m_id.Integer, 1);
          byte[] bigEndian = this.LittleToBigEndian(BitConverter.GetBytes(pdfNumber.LongValue));
          CryptoApi.MD5Update(ref ctx, bigEndian, 4);
          break;
        }
        CryptoApi.MD5Update(ref ctx, this.m_id.Real, 1);
        byte[] bigEndian1 = this.LittleToBigEndian(BitConverter.GetBytes((int) pdfNumber.FloatValue));
        CryptoApi.MD5Update(ref ctx, bigEndian1, 4);
        break;
      case PdfBoolean _:
        CryptoApi.MD5Update(ref ctx, this.m_id.Boolean, 1);
        if ((obj as PdfBoolean).Value)
        {
          CryptoApi.MD5Update(ref ctx, this.m_id.True, 1);
          break;
        }
        CryptoApi.MD5Update(ref ctx, this.m_id.False, 1);
        break;
      default:
        if ((object) (obj as PdfName) != null)
        {
          CryptoApi.MD5Update(ref ctx, this.m_id.Name, 1);
          string data = (obj as PdfName).Value;
          byte[] bigEndian2 = this.LittleToBigEndian(BitConverter.GetBytes(data.Length));
          CryptoApi.MD5Update(ref ctx, bigEndian2, 4);
          byte[] input = PdfString.StringToByte(data);
          CryptoApi.MD5Update(ref ctx, input, input.Length);
          break;
        }
        switch (obj)
        {
          case PdfString _:
            CryptoApi.MD5Update(ref ctx, this.m_id.String, 1);
            string data1 = (obj as PdfString).Value;
            byte[] bigEndian3 = this.LittleToBigEndian(BitConverter.GetBytes(data1.Length));
            CryptoApi.MD5Update(ref ctx, bigEndian3, 4);
            byte[] input1 = PdfString.StringToByte(data1);
            CryptoApi.MD5Update(ref ctx, input1, input1.Length);
            return;
          case PdfArray _:
            Md5_Ctx context1 = new Md5_Ctx();
            CryptoApi.MD5Update(ref context1, this.m_id.Array, 1);
            if (!list.Contains(obj))
            {
              Md5_Ctx md5Ctx = new Md5_Ctx();
              CryptoApi.MD5Init(ref md5Ctx);
              PdfArray pdfArray = obj as PdfArray;
              int count = pdfArray.Count;
              byte[] bigEndian4 = this.LittleToBigEndian(BitConverter.GetBytes(count));
              CryptoApi.MD5Update(ref context1, bigEndian4, 4);
              list.Add(obj);
              for (int index = 0; index < count; ++index)
                this.HashObject((object) pdfArray[index], ref md5Ctx, list);
              CryptoApi.MD5Final(ref md5Ctx);
              byte[] digest = md5Ctx.digest;
              int length = digest.Length;
              CryptoApi.MD5Update(ref context1, digest, length);
              list.Remove(obj);
            }
            else
              CryptoApi.MD5Update(ref context1, this.m_id.Visited, 4);
            CryptoApi.MD5Final(ref context1);
            byte[] digest1 = context1.digest;
            int length1 = digest1.Length;
            CryptoApi.MD5Update(ref ctx, digest1, length1);
            return;
          case PdfDictionary _:
            Md5_Ctx context2 = new Md5_Ctx();
            CryptoApi.MD5Init(ref context2);
            CryptoApi.MD5Update(ref context2, this.m_id.Dictionary, 1);
            if (!list.Contains(obj))
            {
              Md5_Ctx md5Ctx = new Md5_Ctx();
              CryptoApi.MD5Init(ref md5Ctx);
              list.Add(obj);
              PdfDictionary pdfDictionary = obj as PdfDictionary;
              int count = pdfDictionary.Count;
              byte[] bigEndian5 = this.LittleToBigEndian(BitConverter.GetBytes(count));
              CryptoApi.MD5Update(ref context2, bigEndian5, 4);
              ArrayList arrayList = new ArrayList();
              arrayList.AddRange((ICollection) pdfDictionary.Keys);
              for (int index = 0; index < count; ++index)
              {
                CryptoApi.MD5Update(ref md5Ctx, this.m_id.Name, 1);
                PdfName key = arrayList[index] as PdfName;
                string data2 = key.Value;
                byte[] bigEndian6 = this.LittleToBigEndian(BitConverter.GetBytes(data2.Length));
                CryptoApi.MD5Update(ref md5Ctx, bigEndian6, 4);
                byte[] input2 = PdfString.StringToByte(data2);
                CryptoApi.MD5Update(ref md5Ctx, input2, input2.Length);
                this.HashObject((object) pdfDictionary[key], ref md5Ctx, list);
              }
              CryptoApi.MD5Final(ref md5Ctx);
              byte[] digest2 = md5Ctx.digest;
              int length2 = digest2.Length;
              CryptoApi.MD5Update(ref context2, digest2, length2);
              list.Remove(obj);
            }
            else
              CryptoApi.MD5Update(ref context2, this.m_id.Visited, 4);
            CryptoApi.MD5Final(ref context2);
            byte[] digest3 = context2.digest;
            int length3 = digest3.Length;
            CryptoApi.MD5Update(ref ctx, digest3, length3);
            return;
          case PdfStream _:
            PdfDictionary dic = obj as PdfDictionary;
            PdfStream pdfStream = obj as PdfStream;
            Md5_Ctx context3 = new Md5_Ctx();
            CryptoApi.MD5Update(ref context3, this.m_id.Stream, 1);
            if (!list.Contains(obj))
            {
              Md5_Ctx md5Ctx = new Md5_Ctx();
              CryptoApi.MD5Init(ref md5Ctx);
              list.Add(obj);
              int num = this.HashDictionaryItem(dic, "F", ref md5Ctx, list) + this.HashDictionaryItem(dic, "Filter", ref md5Ctx, list) + this.HashDictionaryItem(dic, "Length", ref md5Ctx, list) + this.HashDictionaryItem(dic, "FFilter", ref md5Ctx, list);
              CryptoApi.MD5Update(ref md5Ctx, this.LittleToBigEndian(BitConverter.GetBytes(num)), 4);
              CryptoApi.MD5Final(ref md5Ctx);
              byte[] digest4 = md5Ctx.digest;
              int length4 = digest4.Length;
              CryptoApi.MD5Update(ref context3, digest4, length4);
              list.Remove(obj);
              byte[] data3 = pdfStream.Data;
              byte[] bigEndian7 = this.LittleToBigEndian(BitConverter.GetBytes(pdfStream.Data.Length));
              CryptoApi.MD5Update(ref context3, bigEndian7, bigEndian7.Length);
              CryptoApi.MD5Update(ref context3, data3, data3.Length);
              CryptoApi.MD5Final(ref context3);
              byte[] digest5 = context3.digest;
              int length5 = digest5.Length;
              CryptoApi.MD5Update(ref ctx, digest5, length5);
              this.HashDictionaryName(dic, "Resources", ref ctx, list, false, false);
              return;
            }
            CryptoApi.MD5Update(ref ctx, this.m_id.Visited, 4);
            return;
          default:
            return;
        }
    }
  }

  private void HashPage(PdfPage page, ref Md5_Ctx ctx, List<object> list)
  {
    list.Add((object) page);
    Md5_Ctx md5Ctx = new Md5_Ctx();
    CryptoApi.MD5Init(ref md5Ctx);
    this.HashDictionaryName(page.Dictionary, "MediaBox", ref md5Ctx, list, true, false);
    if (!this.HashDictionaryName(page.Dictionary, "CropBox", ref md5Ctx, list, true, false))
      this.HashDictionaryName(page.Dictionary, "MediaBox", ref md5Ctx, list, true, false);
    this.HashDictionaryName(page.Dictionary, "Resources", ref md5Ctx, list, true, true);
    this.HashDictionaryName(page.Dictionary, "Contents", ref md5Ctx, list, false, true);
    this.HashDictionaryName(page.Dictionary, "Rotate", ref md5Ctx, list, true, true);
    this.HashDictionaryName(page.Dictionary, "AA", ref md5Ctx, list, false, false);
    CryptoApi.MD5Final(ref md5Ctx);
    byte[] digest = md5Ctx.digest;
    int length = digest.Length;
    CryptoApi.MD5Update(ref ctx, digest, length);
    list.Remove((object) page);
  }

  private void HashPages(PdfDocumentPageCollection pages, ref Md5_Ctx ctx, List<object> list)
  {
    int index = 0;
    for (int count = pages.Count; index < count; ++index)
    {
      this.HashPage(pages[index], ref ctx, list);
      this.HashAnnots(pages[index], ref ctx, list);
    }
  }

  private void HashField(PdfField field, ref Md5_Ctx ctx, List<object> list)
  {
    if (field == null)
      return;
    Md5_Ctx md5Ctx = new Md5_Ctx();
    CryptoApi.MD5Init(ref md5Ctx);
    PdfDictionary dictionary = field.Dictionary;
    this.HashObject((object) dictionary["T"], ref md5Ctx, list);
    this.HashObject((object) dictionary["FT"], ref md5Ctx, list);
    this.HashObject((object) dictionary["DV"] ?? (object) new PdfNull(), ref md5Ctx, list);
    if (dictionary["Lock"] == null)
      this.HashObject((object) dictionary["V"] ?? (object) dictionary["AS"] ?? (object) new PdfNull(), ref md5Ctx, list);
    object action = (object) dictionary["A"];
    if ((object) (action as PdfReferenceHolder) != null)
      action = (object) (action as PdfReferenceHolder).Object;
    if (action is PdfDictionary)
      this.HashAction(action as PdfDictionary, ref md5Ctx, list);
    if (!(dictionary["Ff"] is PdfNumber))
    {
      action = (object) new PdfNumber(0);
    }
    else
    {
      int num = (int) (action as PdfNumber).FloatValue & -2;
      (action as PdfNumber).FloatValue = (float) num;
    }
    this.HashObject(action, ref md5Ctx, list);
    if (!(dictionary["F"] is PdfNumber))
    {
      action = (object) new PdfNumber(0);
    }
    else
    {
      int num = (int) (action as PdfNumber).FloatValue & (int) sbyte.MaxValue;
      (action as PdfNumber).FloatValue = (float) num;
    }
    this.HashObject(action, ref md5Ctx, list);
    object obj = (object) dictionary["Lock"];
    if ((object) (obj as PdfReferenceHolder) != null)
      obj = (object) (obj as PdfReferenceHolder).Object;
    if (obj is PdfDictionary)
      this.HashObject(obj, ref md5Ctx, list);
    CryptoApi.MD5Final(ref md5Ctx);
    byte[] digest = md5Ctx.digest;
    int length = digest.Length;
    CryptoApi.MD5Update(ref ctx, digest, length);
  }

  private void HashAction(PdfDictionary action, ref Md5_Ctx ctx, List<object> list)
  {
    Md5_Ctx md5Ctx = new Md5_Ctx();
    CryptoApi.MD5Init(ref md5Ctx);
    this.HashDictionaryName(action, "S", ref md5Ctx, list, false, false);
    this.HashDictionaryName(action, "D", ref md5Ctx, list, false, false);
    this.HashDictionaryName(action, "F", ref md5Ctx, list, false, false);
    this.HashDictionaryName(action, "NewWindow", ref md5Ctx, list, false, false);
    this.HashDictionaryName(action, "O", ref md5Ctx, list, false, false);
    this.HashDictionaryName(action, "P", ref md5Ctx, list, false, false);
    this.HashDictionaryName(action, "B", ref md5Ctx, list, false, false);
    this.HashDictionaryName(action, "Base", ref md5Ctx, list, false, false);
    this.HashDictionaryName(action, "Sound", ref md5Ctx, list, false, false);
    this.HashDictionaryName(action, "Vol", ref md5Ctx, list, false, false);
    this.HashDictionaryName(action, "Annot", ref md5Ctx, list, false, false);
    this.HashDictionaryName(action, "T", ref md5Ctx, list, false, false);
    this.HashDictionaryName(action, "H", ref md5Ctx, list, false, false);
    this.HashDictionaryName(action, "N", ref md5Ctx, list, false, false);
    this.HashDictionaryName(action, "JS", ref md5Ctx, list, false, false);
    this.HashDictionaryName(action, "URI", ref md5Ctx, list, false, false);
    CryptoApi.MD5Final(ref md5Ctx);
    byte[] digest = md5Ctx.digest;
    int length = digest.Length;
    CryptoApi.MD5Update(ref ctx, digest, length);
  }

  private void HashAnnots(PdfPage page, ref Md5_Ctx ctx, List<object> list)
  {
    PdfArray annotations = page.Annotations.Annotations;
    if (annotations == null)
      return;
    for (int index = 0; index < annotations.Count; ++index)
    {
      object obj = (object) annotations[index];
      if ((object) (obj as PdfReferenceHolder) != null)
        obj = (object) (obj as PdfReferenceHolder).Object;
      if (obj is PdfSignature)
        this.HashField((obj as PdfSignature).Field, ref ctx, list);
    }
  }

  private void HashAnnotation(PdfDictionary annot, ref Md5_Ctx ctx, List<object> list)
  {
    list.Add((object) annot);
    Md5_Ctx md5Ctx = new Md5_Ctx();
    CryptoApi.MD5Init(ref md5Ctx);
    this.HashDictionaryName(annot, "T", ref md5Ctx, list, false, false);
    if (!this.HashDictionaryName(annot, "F", ref md5Ctx, list, false, false))
    {
      byte[] input = new byte[5]
      {
        (byte) 1,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0
      };
      CryptoApi.MD5Update(ref md5Ctx, input, 5);
    }
    object action1 = (object) annot["A"];
    if ((object) (action1 as PdfReferenceHolder) != null)
      action1 = (object) (action1 as PdfReferenceHolder).Object;
    if (action1 is PdfDictionary)
      this.HashAction(action1 as PdfDictionary, ref md5Ctx, list);
    object action2 = (object) annot["AA"];
    if ((object) (action2 as PdfReferenceHolder) != null)
      action2 = (object) (action2 as PdfReferenceHolder).Object;
    if (action2 is PdfDictionary)
      this.HashAction(action2 as PdfDictionary, ref md5Ctx, list);
    this.HashDictionaryName(annot, "Dest", ref md5Ctx, list, false, false);
    this.HashDictionaryName(annot, "QuadPoints", ref md5Ctx, list, false, false);
    this.HashDictionaryName(annot, "Inklist", ref md5Ctx, list, false, false);
    this.HashDictionaryName(annot, "Name", ref md5Ctx, list, false, false);
    this.HashDictionaryName(annot, "FS", ref md5Ctx, list, false, false);
    this.HashDictionaryName(annot, "Sound", ref md5Ctx, list, false, false);
    this.HashDictionaryName(annot, "AP", ref md5Ctx, list, false, false);
    CryptoApi.MD5Final(ref md5Ctx);
    byte[] digest = md5Ctx.digest;
    int length = digest.Length;
    CryptoApi.MD5Update(ref ctx, digest, length);
    list.Remove((object) annot);
  }

  private void HashEmbeddedFiles(
    PdfDocumentPageCollection pages,
    ref Md5_Ctx ctx,
    List<object> list)
  {
    Md5_Ctx md5Ctx = new Md5_Ctx();
    CryptoApi.MD5Init(ref md5Ctx);
    int count = pages.Count;
    for (int index = 0; index < count; ++index)
    {
      object obj1 = (object) pages[index].Dictionary["Names"];
      if (obj1 != null)
      {
        if ((object) (obj1 as PdfReferenceHolder) != null)
          obj1 = (object) (obj1 as PdfReferenceHolder).Object;
        if (obj1 is PdfDictionary)
        {
          object obj2 = (object) (obj1 as PdfDictionary)["EmbeddedFiles"];
          if (obj2 != null)
          {
            list.Add(obj2);
            this.HashObject(obj2, ref md5Ctx, list);
            list.Remove(obj2);
          }
        }
      }
    }
    CryptoApi.MD5Final(ref md5Ctx);
    byte[] digest = md5Ctx.digest;
    int length = digest.Length;
    CryptoApi.MD5Update(ref ctx, digest, length);
  }

  private byte[] LittleToBigEndian(byte[] buffer)
  {
    if (buffer != null)
    {
      int length = buffer.Length;
      byte[] numArray = new byte[length];
      int num = length - 1;
      for (int index = num; index >= 0; --index)
        numArray[num - index] = buffer[index];
      buffer = numArray;
    }
    return buffer;
  }
}
