// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.IO.PdfCrossTable
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Interactive;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using Syncfusion.Pdf.Security;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.IO;

internal class PdfCrossTable : IDisposable
{
  private CrossTable m_crossTable;
  private PdfDictionary m_documentCatalog;
  private Stream m_stream;
  private Dictionary<long, PdfCrossTable.RegisteredObject> m_objects = new Dictionary<long, PdfCrossTable.RegisteredObject>();
  private int m_count;
  private bool m_bDisposed;
  private IPdfPrimitive m_trailer;
  private PdfDocumentBase m_document;
  private bool m_bForceNew;
  private Stack<PdfReference> m_objNumbers = new Stack<PdfReference>();
  private long m_maxGenNumIndex;
  private PdfArchiveStream m_archive;
  private Dictionary<PdfReference, PdfReference> m_mappedReferences;
  private int m_storedCount;
  private List<PdfCrossTable.ArchiveInfo> m_archives;
  private PdfDictionary m_encryptorDictionary;
  private PdfMainObjectCollection m_items;
  private bool m_bEncrypt;
  private Dictionary<IPdfPrimitive, object> m_pageCorrespondance;
  private List<PdfReference> m_preReference;
  private bool m_isMerging;
  private bool m_isColorSpace;
  internal bool isOpenAndRepair;
  internal bool isRepair;
  private bool isIndexGreaterthanTotalObjectCount;
  internal bool isCompletely;
  internal bool isDisposed;
  internal PdfLoadedDocument loadedPdfDocument;
  internal PdfDictionary m_pdfDocumentEncoding;

  internal bool Encrypted
  {
    get => this.m_bEncrypt;
    set => this.m_bEncrypt = value;
  }

  public PdfDictionary DocumentCatalog
  {
    get
    {
      if (this.m_documentCatalog == null && this.m_crossTable != null)
        this.m_documentCatalog = PdfCrossTable.Dereference((IPdfPrimitive) this.m_crossTable.DocumentCatalog) as PdfDictionary;
      return this.m_documentCatalog;
    }
  }

  internal Stream Stream => this.m_crossTable.Stream;

  internal int NextObjNumber
  {
    get
    {
      if (this.Count == 0)
        ++this.Count;
      return this.Count++;
    }
  }

  internal CrossTable CrossTable => this.m_crossTable;

  internal int Count
  {
    get
    {
      if (this.m_count == 0)
      {
        IPdfPrimitive pdfPrimitive = (IPdfPrimitive) null;
        if (this.m_crossTable != null)
          pdfPrimitive = this.m_crossTable.Trailer["Size"];
        this.m_count = (pdfPrimitive == null ? new PdfNumber(0) : PdfCrossTable.Dereference(pdfPrimitive) as PdfNumber).IntValue;
      }
      return this.m_count;
    }
    set
    {
      this.m_count = value != 0 ? value : throw new ArgumentException("The value can't be 0.", nameof (Count));
    }
  }

  internal PdfDocumentBase Document
  {
    get => this.m_document;
    set
    {
      this.m_document = value != null ? value : throw new ArgumentNullException(nameof (Document));
      this.m_items = this.m_document.PdfObjects;
    }
  }

  internal PdfMainObjectCollection PdfObjects => this.m_items;

  internal PdfDictionary Trailer
  {
    get
    {
      if (this.m_trailer == null)
        this.m_trailer = this.m_crossTable == null ? (IPdfPrimitive) new PdfStream() : (IPdfPrimitive) this.m_crossTable.Trailer;
      if ((this.m_trailer as PdfDictionary).ContainsKey("XRefStm"))
        (this.m_trailer as PdfDictionary).Remove(new PdfName("XRefStm"));
      return this.m_trailer as PdfDictionary;
    }
  }

  internal bool IsMerging
  {
    get => this.m_isMerging;
    set => this.m_isMerging = value;
  }

  internal PdfEncryptor Encryptor
  {
    get => this.m_crossTable != null ? this.m_crossTable.Encryptor : (PdfEncryptor) null;
    set
    {
      this.m_crossTable.Encryptor = value != null ? value.Clone() : throw new ArgumentNullException(nameof (Encryptor));
    }
  }

  private PdfMainObjectCollection ObjectCollection => this.m_document.PdfObjects;

  internal PdfDictionary EncryptorDictionary
  {
    get
    {
      if (this.m_encryptorDictionary == null)
      {
        this.m_bEncrypt = true;
        this.m_encryptorDictionary = PdfCrossTable.Dereference(this.Trailer["Encrypt"]) as PdfDictionary;
      }
      this.m_bEncrypt = false;
      return this.m_encryptorDictionary;
    }
  }

  internal Dictionary<IPdfPrimitive, object> PageCorrespondance
  {
    get
    {
      if (this.m_pageCorrespondance == null)
        this.m_pageCorrespondance = new Dictionary<IPdfPrimitive, object>();
      return this.m_pageCorrespondance;
    }
    set => this.m_pageCorrespondance = value;
  }

  internal List<PdfReference> PrevReference
  {
    get
    {
      if (this.m_preReference == null)
        this.m_preReference = new List<PdfReference>();
      return this.m_preReference;
    }
    set => this.m_preReference = value;
  }

  internal bool StructureAltered => this.m_crossTable.IsStructureAltered;

  public PdfCrossTable(Stream docStream)
  {
    this.m_stream = docStream != null ? docStream : throw new ArgumentNullException("stream");
    this.m_crossTable = new CrossTable(docStream, this);
  }

  public PdfCrossTable(Stream docStream, bool openAndRepair)
  {
    if (docStream == null)
      throw new ArgumentNullException("stream");
    this.isOpenAndRepair = openAndRepair;
    this.m_stream = docStream;
    this.m_crossTable = new CrossTable(docStream, this);
  }

  internal PdfCrossTable(Stream docStream, bool openAndRepair, bool repair)
  {
    if (docStream == null)
      throw new ArgumentNullException("stream");
    this.isOpenAndRepair = openAndRepair;
    this.isRepair = repair;
    this.m_stream = docStream;
    this.m_crossTable = new CrossTable(docStream, this);
  }

  public PdfCrossTable()
  {
  }

  internal PdfCrossTable(
    int count,
    PdfDictionary encryptionDictionary,
    PdfDictionary documentCatalog)
    : this()
  {
    this.m_storedCount = count;
    this.m_bForceNew = true;
    this.m_encryptorDictionary = encryptionDictionary;
    this.m_documentCatalog = documentCatalog;
  }

  internal PdfCrossTable(
    int count,
    PdfDictionary encryptionDictionary,
    PdfDictionary documentCatalog,
    CrossTable cTable)
    : this()
  {
    this.m_storedCount = count;
    this.m_bForceNew = true;
    this.m_encryptorDictionary = encryptionDictionary;
    this.m_documentCatalog = documentCatalog;
    this.m_crossTable = cTable;
  }

  internal PdfCrossTable(bool isFdf, Stream docStream)
  {
    this.m_stream = docStream != null ? docStream : throw new ArgumentNullException("stream");
    this.m_crossTable = new CrossTable(docStream, this, isFdf);
  }

  ~PdfCrossTable() => this.Dispose(false);

  internal PdfCrossTable(Stream docStream, PdfLoadedDocument ldoc)
  {
    if (docStream == null)
      throw new ArgumentNullException("stream");
    this.loadedPdfDocument = ldoc;
    this.m_stream = docStream;
    this.m_crossTable = new CrossTable(docStream, this);
  }

  public static IPdfPrimitive Dereference(IPdfPrimitive obj)
  {
    PdfReferenceHolder pdfReferenceHolder = obj as PdfReferenceHolder;
    if (pdfReferenceHolder != (PdfReferenceHolder) null)
      obj = pdfReferenceHolder.Object;
    return obj;
  }

  public IPdfPrimitive GetObject(IPdfPrimitive pointer)
  {
    IPdfPrimitive pdfPrimitive1 = pointer;
    bool flag = true;
    if ((object) (pointer as PdfReferenceHolder) != null)
      pdfPrimitive1 = (pointer as PdfReferenceHolder).Object;
    else if ((object) (pointer as PdfReference) != null)
    {
      PdfReference reference = pointer as PdfReference;
      if (reference != (PdfReference) null)
        this.m_objNumbers.Push(reference);
      IPdfPrimitive pdfPrimitive2 = (IPdfPrimitive) null;
      if (this.m_crossTable != null)
        pdfPrimitive2 = this.m_crossTable.GetObject(pointer);
      else if (this.PdfObjects.GetObjectIndex(reference) == 0)
        pdfPrimitive2 = this.PdfObjects.GetObject(reference);
      IPdfPrimitive pdfPrimitive3 = this.PageProceed(pdfPrimitive2);
      PdfMainObjectCollection pdfObjects = this.PdfObjects;
      if (pdfPrimitive3 != null)
      {
        if (pdfObjects.ContainsReference(reference))
        {
          pdfObjects.GetObjectIndex(reference);
          pdfPrimitive3 = pdfObjects.GetObject(reference);
        }
        else
        {
          pdfObjects.Add(pdfPrimitive3, reference);
          if (!this.m_isMerging)
          {
            pdfPrimitive3.Position = -1;
            reference.Position = -1;
          }
        }
      }
      pdfPrimitive1 = pdfPrimitive3;
      if (pdfPrimitive3 is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey("Type"))
      {
        PdfName pdfName = pdfDictionary.GetValue("Type", (string) null) as PdfName;
        if (pdfName != (PdfName) null && pdfName.Value == "Metadata".ToString() && this.Encryptor != null)
          flag = this.Encryptor.EncryptMetaData;
      }
      if (this.Document.WasEncrypted && flag)
        this.Decrypt(pdfPrimitive1);
    }
    if (this.Document.WasEncrypted && flag)
      this.Decrypt(pdfPrimitive1 as IPdfDecryptable);
    if ((object) (pointer as PdfReference) != null && this.m_objNumbers.Count > 0)
      this.m_objNumbers.Pop();
    return pdfPrimitive1;
  }

  private void Decrypt(IPdfDecryptable obj)
  {
    if (!this.Document.WasEncrypted || obj == null || obj.Decrypted || this.m_objNumbers.Count <= 0 || this.Encryptor == null || this.Encryptor.EncryptOnlyAttachment)
      return;
    PdfEncryptor encryptor = this.Encryptor;
    long objNum = this.m_objNumbers.Peek().ObjNum;
    obj.Decrypt(encryptor, objNum);
  }

  private void Decrypt(IPdfPrimitive obj)
  {
    PdfDictionary pdfDictionary = obj as PdfDictionary;
    PdfArray pdfArray = obj as PdfArray;
    if (pdfDictionary != null && !pdfDictionary.IsDecrypted)
    {
      foreach (IPdfPrimitive pdfPrimitive in pdfDictionary.Values)
        this.Decrypt(pdfPrimitive);
      this.Decrypt(pdfDictionary as IPdfDecryptable);
    }
    else if (pdfArray != null)
    {
      foreach (IPdfPrimitive pdfPrimitive in pdfArray)
      {
        PdfName pdfName = pdfPrimitive as PdfName;
        if (pdfName != (PdfName) null && pdfName.Value.Equals("Indexed"))
          this.m_isColorSpace = true;
        this.Decrypt(pdfPrimitive);
      }
      this.m_isColorSpace = false;
    }
    else if (obj is PdfString)
    {
      PdfString pdfString = obj as PdfString;
      if ((pdfString.Decrypted || pdfString.Hex || pdfString.IsPacked) && (pdfString.Decrypted || !this.m_isColorSpace || pdfString.IsPacked))
        return;
      this.Decrypt(obj as IPdfDecryptable);
    }
    else
      this.Decrypt(obj as IPdfDecryptable);
  }

  public byte[] GetStream(IPdfPrimitive streamRef)
  {
    return streamRef != null ? this.m_crossTable.GetStream(streamRef) : throw new ArgumentNullException(nameof (streamRef));
  }

  public void RegisterObject(long offset, PdfReference reference)
  {
    this.m_objects[reference.ObjNum] = !(reference == (PdfReference) null) ? new PdfCrossTable.RegisteredObject(offset, reference) : throw new ArgumentNullException(nameof (reference));
    this.m_maxGenNumIndex = Math.Max(this.m_maxGenNumIndex, (long) reference.GenNum);
  }

  public void RegisterObject(PdfArchiveStream archive, PdfReference reference)
  {
    this.m_objects[reference.ObjNum] = new PdfCrossTable.RegisteredObject(this, archive, reference);
    this.m_maxGenNumIndex = Math.Max(this.m_maxGenNumIndex, (long) archive.Count);
  }

  public void RegisterObject(long offset, PdfReference reference, bool free)
  {
    if (reference == (PdfReference) null)
      throw new ArgumentNullException(nameof (reference));
    this.m_objects[reference.ObjNum] = new PdfCrossTable.RegisteredObject(offset, reference, free);
    this.m_maxGenNumIndex = Math.Max(this.m_maxGenNumIndex, (long) reference.GenNum);
  }

  public void Save(PdfWriter writer)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (this.m_documentCatalog != null && this.m_documentCatalog.ContainsKey("Perms") && this.Document.FileStructure.IncrementalUpdate)
    {
      PdfDictionary pdfDictionary = (PdfDictionary) null;
      if (this.m_documentCatalog["Perms"] is PdfDictionary)
        pdfDictionary = this.m_documentCatalog["Perms"] as PdfDictionary;
      else if ((object) (this.m_documentCatalog["Perms"] as PdfReferenceHolder) != null)
        pdfDictionary = (this.m_documentCatalog["Perms"] as PdfReferenceHolder).Object as PdfDictionary;
      if (!pdfDictionary.ContainsKey("UR3"))
        this.SaveHead(writer);
    }
    else
      this.SaveHead(writer);
    bool flag = false;
    PdfSecurity security = this.m_document.Security;
    this.m_mappedReferences = (Dictionary<PdfReference, PdfReference>) null;
    if (this.m_archives != null)
      this.m_archives.Clear();
    this.m_archive = (PdfArchiveStream) null;
    if (this.m_objects != null)
      this.m_objects.Clear();
    this.MarkTrailerReferences();
    if (this.Document != null && this.Document is PdfDocument && this.Document.FileStructure.Version == PdfVersion.Version2_0)
    {
      if (security != null && security.KeySize == PdfEncryptionKeySize.Key256Bit)
        security.KeySize = PdfEncryptionKeySize.Key256BitRevision6;
      PdfCatalog catalog = this.Document.Catalog;
      if (catalog != null && catalog.ContainsKey("AcroForm"))
      {
        IPdfPrimitive pdfPrimitive = catalog["AcroForm"];
        if (((object) (pdfPrimitive as PdfReferenceHolder) != null ? (pdfPrimitive as PdfReferenceHolder).Object : pdfPrimitive) is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey("NeedAppearances"))
        {
          pdfDictionary.Remove("NeedAppearances");
          pdfDictionary.Modify();
        }
        PdfForm form = (this.Document as PdfDocument).Form;
        if (form != null)
          form.SetAppearanceDictionary = false;
      }
      if (this.Trailer != null && this.Trailer.ContainsKey("Info"))
      {
        IPdfPrimitive pdfPrimitive = this.Trailer["Info"];
        if (((object) (pdfPrimitive as PdfReferenceHolder) != null ? (pdfPrimitive as PdfReferenceHolder).Object : pdfPrimitive) is PdfDictionary pdfDictionary)
        {
          if (pdfDictionary.ContainsKey("Title"))
            pdfDictionary.Remove("Title");
          if (pdfDictionary.ContainsKey("Author"))
            pdfDictionary.Remove("Author");
          if (pdfDictionary.ContainsKey("Subject"))
            pdfDictionary.Remove("Subject");
          if (pdfDictionary.ContainsKey("Keywords"))
            pdfDictionary.Remove("Keywords");
          if (pdfDictionary.ContainsKey("Producer"))
            pdfDictionary.Remove("Producer");
          if (pdfDictionary.ContainsKey("Creator"))
            pdfDictionary.Remove("Creator");
          if (pdfDictionary.ContainsKey("Trapped"))
            pdfDictionary.Remove("Trapped");
          pdfDictionary.Modify();
        }
      }
    }
    if (this.m_document.FileStructure.CrossReferenceType == PdfCrossReferenceType.CrossReferenceTable && security != null && security.Enabled && security.Encryptor.Encrypt && this.m_document is PdfDocument && security.Encryptor.UserPassword.Length == 0 && security.Encryptor.OwnerPassword.Length == 0)
    {
      flag = security.Enabled;
      security.Enabled = false;
    }
    this.SaveObjects(writer);
    if (this.m_document.FileStructure.CrossReferenceType == PdfCrossReferenceType.CrossReferenceTable && security != null && security.Enabled && security.Encryptor.Encrypt && this.m_document is PdfDocument && security.Encryptor.UserPassword.Length == 0 && security.Encryptor.OwnerPassword.Length == 0)
      security.Enabled = flag;
    int count = this.Count;
    this.SaveArchives(writer);
    if (writer.ObtainStream().CanSeek)
      writer.Position = writer.Length;
    long position = writer.Position;
    this.RegisterObject(0L, new PdfReference(0L, -1), true);
    long xrefOffset = this.m_crossTable == null ? 0L : this.m_crossTable.XRefOffset;
    long prevXRef = this.m_bForceNew ? 0L : xrefOffset;
    if (this.IsCrossReferenceStream(writer.Document))
    {
      PdfReference reference;
      PdfStream pdfStream = this.PrepareXRefStream(prevXRef, position, out reference);
      pdfStream.BlockEncryption();
      this.DoSaveObject((IPdfPrimitive) pdfStream, reference, writer);
    }
    else
    {
      writer.Write("xref");
      writer.Write("\r\n");
      this.SaveSections(writer);
      this.SaveTrailer(writer, (long) this.Count, prevXRef);
    }
    this.SaveTheEndess(writer, position);
    this.Count = count;
    for (int index = 0; index < this.ObjectCollection.Count; ++index)
      this.ObjectCollection[index].Object.IsSaving = false;
  }

  internal PdfReference GetReference(IPdfPrimitive obj) => this.GetReference(obj, out bool _);

  internal PdfReference GetReference(IPdfPrimitive obj, out bool bNew)
  {
    bool flag1 = false;
    if (obj is PdfArchiveStream)
    {
      PdfReference archiveReference = this.FindArchiveReference(obj as PdfArchiveStream);
      bNew = flag1;
      return archiveReference;
    }
    if ((object) (obj as PdfReferenceHolder) != null)
    {
      obj = (obj as PdfReferenceHolder).Object;
      if (this.m_document is PdfDocument)
        obj.IsSaving = true;
    }
    if (obj is IPdfWrapper)
      obj = (obj as IPdfWrapper).Element;
    PdfReference reference = (PdfReference) null;
    bool isNew;
    if (obj.IsSaving)
    {
      if (this.m_items.Count > 0 && obj.ObjectCollectionIndex > 0 && this.m_items.Count > obj.ObjectCollectionIndex - 1)
        reference = !this.m_items[obj.ObjectCollectionIndex - 1].Equals((object) obj) ? this.Document.PdfObjects.GetReference(obj, out isNew) : this.Document.PdfObjects.GetReference(obj.ObjectCollectionIndex - 1);
    }
    else
      reference = this.Document.PdfObjects.GetReference(obj, out isNew);
    isNew = reference == (PdfReference) null && obj.Status != ObjectStatus.Registered;
    if (this.m_bForceNew)
    {
      if (reference == (PdfReference) null)
      {
        long num = this.m_storedCount > 0 ? (long) this.m_storedCount++ : (long) this.Document.PdfObjects.Count;
        if (this.CrossTable != null && this.CrossTable.m_isOpenAndRepair)
        {
          bool flag2;
          while (true)
          {
            flag2 = false;
            if (this.CrossTable.m_objects.ContainsKey(num))
              ++num;
            else
              break;
          }
          if (flag2)
            this.m_storedCount = (int) num;
        }
        if (num <= 0L)
        {
          num = 1L;
          this.m_storedCount = 2;
        }
        while (this.Document.PdfObjects.mainObjectCollection.ContainsKey(num))
          ++num;
        reference = new PdfReference(num, 0);
        if (isNew)
        {
          this.Document.PdfObjects.Add(obj, reference);
          if (!this.m_isMerging)
          {
            obj.Position = -1;
            reference.Position = -1;
          }
        }
        else
          this.Document.PdfObjects.TrySetReference(obj, reference, out bool _);
      }
      reference = this.GetMappedReference(reference);
    }
    if (reference == (PdfReference) null)
    {
      int nextObjNumber = this.NextObjNumber;
      if (this.m_crossTable != null && this.m_crossTable.m_objects != null)
      {
        while (this.m_crossTable.m_objects.ContainsKey((long) nextObjNumber))
          nextObjNumber = this.NextObjNumber;
      }
      if (this.Document.PdfObjects.mainObjectCollection.ContainsKey((long) nextObjNumber))
      {
        reference = new PdfReference((long) this.NextObjNumber, 0);
      }
      else
      {
        PdfNumber pdfNumber = (PdfNumber) null;
        IPdfPrimitive pdfPrimitive = (IPdfPrimitive) null;
        if (this.m_crossTable != null)
          pdfPrimitive = this.m_crossTable.Trailer["Size"];
        if (pdfPrimitive != null)
          pdfNumber = PdfCrossTable.Dereference(pdfPrimitive) as PdfNumber;
        reference = pdfNumber == null || nextObjNumber != pdfNumber.IntValue ? new PdfReference((long) nextObjNumber, 0) : new PdfReference((long) this.NextObjNumber, 0);
      }
      bool found;
      if (isNew)
      {
        this.Document.PdfObjects.Add(obj);
        this.Document.PdfObjects.TrySetReference(obj, reference, out found);
        long objNum = this.Document.PdfObjects[this.Document.PdfObjects.Count - 1].Reference.ObjNum;
        if (!this.Document.PdfObjects.mainObjectCollection.ContainsKey(objNum))
          this.Document.PdfObjects.mainObjectCollection.Add(objNum, this.Document.PdfObjects[this.Document.PdfObjects.Count - 1]);
        if (!this.m_isMerging && !this.m_isMerging)
          obj.Position = -1;
      }
      else
        this.Document.PdfObjects.TrySetReference(obj, reference, out found);
      obj.ObjectCollectionIndex = (int) reference.ObjNum;
      obj.Status = ObjectStatus.None;
      flag1 = true;
    }
    bNew = flag1 || this.m_bForceNew;
    return reference;
  }

  internal void ForceNew()
  {
    this.m_crossTable.Trailer.Remove("Size");
    this.m_crossTable.Trailer.Remove("Prev");
    if (this.m_count > 0)
      this.m_storedCount = this.m_count;
    this.m_count = 0;
    this.m_bForceNew = true;
  }

  private void MarkTrailerReferences()
  {
    foreach (IPdfPrimitive pdfPrimitive in this.Trailer.Values)
    {
      PdfReferenceHolder pdfReferenceHolder = pdfPrimitive as PdfReferenceHolder;
      if (pdfReferenceHolder != (PdfReferenceHolder) null && !this.Document.PdfObjects.Contains(pdfReferenceHolder.Object))
      {
        this.Document.PdfObjects.Add(pdfReferenceHolder.Object);
        if (!this.m_isMerging)
          pdfReferenceHolder.Object.Position = -1;
      }
    }
  }

  private IPdfPrimitive PageProceed(IPdfPrimitive obj)
  {
    switch (obj)
    {
      case PdfLoadedPage _:
        return obj;
      case PdfDictionary pdfDictionary:
        if (!(obj is PdfPage) && pdfDictionary.ContainsKey("Type"))
        {
          IPdfPrimitive pointer = pdfDictionary["Type"];
          if (pointer.GetType().Name == "PdfName" && (this.GetObject(pointer) as PdfName).Value == "Page" && !pdfDictionary.ContainsKey("Kids"))
          {
            obj = ((IPdfWrapper) (this.Document as PdfLoadedDocument).Pages.GetPage(pdfDictionary)).Element;
            PdfMainObjectCollection pdfObjects = this.Document.PdfObjects;
            int oldObjIndex = pdfObjects.IndexOf((IPdfPrimitive) pdfDictionary);
            if (oldObjIndex >= 0)
            {
              pdfObjects.ReregisterReference(oldObjIndex, obj);
              if (!this.m_isMerging)
              {
                obj.Position = -1;
                break;
              }
              break;
            }
            break;
          }
          break;
        }
        break;
    }
    return obj;
  }

  private PdfStream PrepareXRefStream(long prevXRef, long position, out PdfReference reference)
  {
    if (!(this.Trailer is PdfStream trailer1))
    {
      trailer1 = new PdfStream();
    }
    else
    {
      trailer1.Remove("Filter");
      trailer1.Remove("DecodeParms");
    }
    PdfArray pdfArray = new PdfArray();
    reference = new PdfReference((long) this.NextObjNumber, 0);
    this.RegisterObject(position, reference);
    long objectNum = 0;
    int[] numArray = new int[3]
    {
      1,
      Math.Max(this.GetSize((ulong) position), this.GetSize((ulong) this.Count)),
      this.GetSize((ulong) this.m_maxGenNumIndex)
    };
    using (MemoryStream output = new MemoryStream(100))
    {
      using (BinaryWriter xRefStream = new BinaryWriter((Stream) output))
      {
        long count;
        for (; (count = this.PrepareSubsection(ref objectNum)) > 0L; objectNum += count)
        {
          pdfArray.Add((IPdfPrimitive) new PdfNumber(objectNum));
          pdfArray.Add((IPdfPrimitive) new PdfNumber(count));
          this.SaveSubsection(xRefStream, objectNum, count, numArray);
        }
        xRefStream.Flush();
        trailer1.Data = output.ToArray();
      }
    }
    trailer1["Index"] = (IPdfPrimitive) pdfArray;
    trailer1["Size"] = (IPdfPrimitive) new PdfNumber(this.Count);
    if (prevXRef != 0L)
      trailer1["Prev"] = (IPdfPrimitive) new PdfNumber(prevXRef);
    trailer1["Type"] = (IPdfPrimitive) new PdfName("XRef");
    trailer1["W"] = (IPdfPrimitive) new PdfArray(numArray);
    if (this.m_crossTable != null)
    {
      PdfDictionary trailer2 = this.m_crossTable.Trailer;
      foreach (PdfName key in trailer2.Keys)
      {
        if (!trailer1.ContainsKey(key) && key.Value != "DecodeParms" && key.Value != "Filter")
          trailer1[key] = trailer2[key];
      }
    }
    if (prevXRef == 0L && this.m_bForceNew && trailer1.ContainsKey("Prev"))
      trailer1.Remove("Prev");
    this.ForceIDHex((PdfDictionary) trailer1);
    trailer1.Encrypt = false;
    return trailer1;
  }

  private int GetSize(ulong number)
  {
    return number >= (ulong) uint.MaxValue ? 8 : (number >= (ulong) ushort.MaxValue ? (number >= 16777215UL /*0xFFFFFF*/ ? 4 : 3) : (number >= (ulong) byte.MaxValue ? 2 : 1));
  }

  private void SaveSubsection(BinaryWriter xRefStream, long objectNum, long count, int[] format)
  {
    for (long key = objectNum; key < objectNum + count; ++key)
    {
      PdfCrossTable.RegisteredObject registeredObject = this.m_objects[key];
      xRefStream.Write((byte) registeredObject.Type);
      switch (registeredObject.Type)
      {
        case ObjectType.Free:
          this.SaveLong(xRefStream, registeredObject.ObjectNumber, format[1]);
          this.SaveLong(xRefStream, (long) registeredObject.GenerationNumber, format[2]);
          break;
        case ObjectType.Normal:
          this.SaveLong(xRefStream, registeredObject.Offset, format[1]);
          this.SaveLong(xRefStream, (long) registeredObject.GenerationNumber, format[2]);
          break;
        case ObjectType.Packed:
          this.SaveLong(xRefStream, registeredObject.ObjectNumber, format[1]);
          this.SaveLong(xRefStream, registeredObject.Offset, format[2]);
          break;
        default:
          throw new PdfDocumentException("Internal error: Undefined object type.");
      }
    }
  }

  private void SaveLong(BinaryWriter xRefStream, long number, int count)
  {
    for (int index = count - 1; index >= 0; --index)
    {
      byte num = (byte) ((ulong) (number >> (index << 3)) & (ulong) byte.MaxValue);
      xRefStream.Write(num);
    }
  }

  private void SetSecurity()
  {
    PdfSecurity security = this.m_document.Security;
    this.Trailer.Encrypt = false;
    if (security.Encryptor.Encrypt)
    {
      PdfDictionary pdfDictionary = this.EncryptorDictionary;
      if (pdfDictionary == null)
      {
        pdfDictionary = new PdfDictionary();
        pdfDictionary.Encrypt = false;
        this.m_document.PdfObjects.Add((IPdfPrimitive) pdfDictionary);
        if (!this.m_isMerging)
          pdfDictionary.Position = -1;
        this.Trailer["Encrypt"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary);
      }
      else if (!this.m_document.PdfObjects.Contains((IPdfPrimitive) pdfDictionary))
      {
        this.m_document.PdfObjects.Add((IPdfPrimitive) pdfDictionary);
        if (!this.m_isMerging)
          pdfDictionary.Position = -1;
        this.Trailer["Encrypt"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary);
      }
      security.Encryptor.SaveToDictionary(pdfDictionary);
      this.Trailer["ID"] = (IPdfPrimitive) security.Encryptor.FileID;
      this.Trailer["Encrypt"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary);
    }
    else
    {
      if (security.m_encryptOnlyAttachment)
        return;
      if (this.Trailer.ContainsKey("Encrypt"))
        this.Trailer.Remove("Encrypt");
      bool fileId = this.m_document.FileStructure.m_fileID;
      bool flag1 = false;
      if (this.m_document != null)
      {
        PdfLoadedDocument document = this.m_document as PdfLoadedDocument;
        bool flag2 = this.Trailer.ContainsKey("Info");
        if (document != null && document.Conformance != PdfConformanceLevel.None)
          flag1 = true;
        if (!flag2)
          this.Trailer.Remove("Info");
      }
      if (!this.Trailer.ContainsKey("ID") || fileId || flag1)
        return;
      this.Trailer.Remove("ID");
    }
  }

  private PdfArray GetFileID()
  {
    string str1 = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
    string str2 = "";
    if (this.CrossTable != null)
      str2 = this.CrossTable.Stream.Length.ToString();
    byte[] bytes = Encoding.UTF8.GetBytes(str1 + str2);
    PdfString pdfString = new PdfString(MD5.Create().ComputeHash(bytes));
    return new PdfArray()
    {
      (IPdfPrimitive) pdfString,
      (IPdfPrimitive) pdfString
    };
  }

  private void SaveObjects(PdfWriter writer)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    PdfMainObjectCollection objectCollection = this.ObjectCollection;
    if (this.m_bForceNew)
    {
      this.Count = 1;
      this.m_mappedReferences = (Dictionary<PdfReference, PdfReference>) null;
    }
    this.SetSecurity();
    if (this.m_document.FileStructure.m_fileID && !this.m_document.Security.Enabled)
    {
      if (this.Trailer.ContainsKey("ID"))
      {
        PdfArray pdfArray1 = new PdfArray();
        if (this.Trailer["ID"] is PdfArray pdfArray2 && pdfArray2.Count > 0)
        {
          IPdfPrimitive element = pdfArray2[0];
          pdfArray1.Add(element);
        }
        IPdfPrimitive element1 = this.GetFileID()[0];
        pdfArray1.Add(element1);
        this.Trailer["ID"] = (IPdfPrimitive) pdfArray1;
      }
      else
        this.Trailer["ID"] = (IPdfPrimitive) this.GetFileID();
    }
    List<IPdfPrimitive> pdfPrimitiveList = (List<IPdfPrimitive>) null;
    for (int index1 = 0; index1 < objectCollection.Count; ++index1)
    {
      ObjectInfo objectInfo = objectCollection[index1];
      if (objectInfo.Modified || this.m_bForceNew)
      {
        IPdfPrimitive pdfPrimitive = objectInfo.Object;
        if (pdfPrimitive is PdfStructTreeRoot)
        {
          if (pdfPrimitiveList == null)
            pdfPrimitiveList = new List<IPdfPrimitive>();
          if (!pdfPrimitiveList.Contains(pdfPrimitive))
            pdfPrimitiveList.Add(pdfPrimitive);
        }
        else
        {
          PdfLoadedDocument document = this.m_document as PdfLoadedDocument;
          if (pdfPrimitive is PdfStream && document != null)
          {
            PdfStream pdfStream = pdfPrimitive as PdfStream;
            if (pdfStream.ContainsKey("Type") && (pdfStream["Type"] as PdfName).Value == "Metadata" && document.CompressionOptions != null && document.CompressionOptions.RemoveMetadata)
              continue;
          }
          this.SavePrimitive(pdfPrimitive, writer);
          if (index1 == objectCollection.Count - 1 && pdfPrimitiveList != null && pdfPrimitiveList.Count > 0)
          {
            for (int index2 = 0; index2 < pdfPrimitiveList.Count; ++index2)
              this.SavePrimitive(pdfPrimitiveList[index2], writer);
            pdfPrimitiveList.Clear();
          }
        }
      }
    }
  }

  private void SavePrimitive(IPdfPrimitive obj, PdfWriter writer)
  {
    if (this.Document is PdfDocument)
      obj.IsSaving = true;
    if (obj == this.Trailer)
      return;
    switch (obj)
    {
      case PdfCatalog _ when this.m_documentCatalog != null && this.m_documentCatalog.ContainsKey("Perms") && this.Document.FileStructure.IncrementalUpdate:
        PdfDictionary pdfDictionary = (PdfDictionary) null;
        if (this.m_documentCatalog["Perms"] is PdfDictionary)
          pdfDictionary = this.m_documentCatalog["Perms"] as PdfDictionary;
        else if ((object) (this.m_documentCatalog["Perms"] as PdfReferenceHolder) != null)
          pdfDictionary = (this.m_documentCatalog["Perms"] as PdfReferenceHolder).Object as PdfDictionary;
        if (pdfDictionary.ContainsKey("UR3"))
          return;
        this.SaveIndirectObject(obj, writer);
        return;
      case PdfStructTreeRoot _:
        this.StructureRootElements(obj as PdfStructTreeRoot);
        break;
    }
    bool flag = false;
    if (obj != null && obj is PdfDictionary && (obj as PdfDictionary).isSkip)
      flag = true;
    if (flag)
      return;
    this.SaveIndirectObject(obj, writer);
  }

  private void StructureRootElements(PdfStructTreeRoot treeRoot)
  {
    if (treeRoot == null || PdfDocument.ConformanceLevel != PdfConformanceLevel.None || !(PdfCrossTable.Dereference(treeRoot["K"]) is PdfArray pdfArray))
      return;
    PdfDictionary pdfDictionary1 = new PdfDictionary();
    for (int index = 0; index < pdfArray.Count; ++index)
    {
      if (PdfCrossTable.Dereference(pdfArray[index]) is PdfDictionary pdfDictionary2)
        pdfDictionary2["P"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary1);
    }
    pdfDictionary1["S"] = (IPdfPrimitive) new PdfName("Document");
    pdfDictionary1["K"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfArray);
    pdfDictionary1["P"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) treeRoot);
    treeRoot["K"] = (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary1);
  }

  private void SaveArchives(PdfWriter writer)
  {
    if (this.m_archives == null)
      return;
    foreach (PdfCrossTable.ArchiveInfo archive in this.m_archives)
    {
      PdfReference reference = archive.Reference;
      if (reference == (PdfReference) null)
      {
        reference = new PdfReference((long) this.NextObjNumber, 0);
        archive.Reference = reference;
      }
      this.m_document.CurrentSavingObj = reference;
      this.RegisterObject(writer.Position, reference);
      this.DoSaveObject((IPdfPrimitive) archive.Archive, reference, writer);
    }
  }

  private PdfReference GetMappedReference(PdfReference reference)
  {
    if (reference == (PdfReference) null)
      return (PdfReference) null;
    if (this.m_mappedReferences == null)
      this.m_mappedReferences = new Dictionary<PdfReference, PdfReference>(100);
    PdfReference mappedReference = (PdfReference) null;
    this.m_mappedReferences.TryGetValue(reference, out mappedReference);
    if (mappedReference == (PdfReference) null)
    {
      mappedReference = new PdfReference((long) this.NextObjNumber, 0);
      this.m_mappedReferences[reference] = mappedReference;
    }
    return mappedReference;
  }

  private PdfReference FindArchiveReference(PdfArchiveStream archive)
  {
    int index = 0;
    PdfCrossTable.ArchiveInfo archiveInfo = (PdfCrossTable.ArchiveInfo) null;
    for (int count = this.m_archives.Count; index < count; ++index)
    {
      archiveInfo = this.m_archives[index];
      if (archiveInfo.Archive == archive)
        break;
    }
    PdfReference archiveReference = archiveInfo.Reference;
    if (archiveReference == (PdfReference) null)
      archiveReference = new PdfReference((long) this.NextObjNumber, 0);
    archiveInfo.Reference = archiveReference;
    return archiveReference;
  }

  internal void SaveIndirectObject(IPdfPrimitive obj, PdfWriter writer)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (obj == null)
      throw new ArgumentNullException(nameof (obj));
    if (obj is PdfEncryptor && !(obj as PdfEncryptor).Encrypt)
      return;
    PdfReference reference = this.GetReference(obj);
    if (obj is PdfCatalog)
    {
      this.Trailer["Root"] = (IPdfPrimitive) reference;
      if (PdfDocument.ConformanceLevel != PdfConformanceLevel.None)
        this.Trailer["ID"] = (IPdfPrimitive) this.m_document.Security.Encryptor.FileID;
    }
    this.m_document.CurrentSavingObj = reference;
    bool flag1 = !(obj is PdfDictionary) || (obj as PdfDictionary).Archive;
    if (obj is PdfDictionary && this.Document.FileStructure.CrossReferenceType == PdfCrossReferenceType.CrossReferenceStream && this.Document is PdfLoadedDocument && this.Document.Catalog.LoadedForm != null && this.Document.Catalog.LoadedForm.SignatureFlags == (SignatureFlags.SignaturesExists | SignatureFlags.AppendOnly))
      flag1 = false;
    bool flag2 = !(obj is PdfStream) && flag1 && !(obj is PdfCatalog) && !(obj is Pdf3DStream);
    bool flag3 = false;
    if (obj is PdfDictionary && this.Document.FileStructure.CrossReferenceType == PdfCrossReferenceType.CrossReferenceStream)
    {
      PdfDictionary pdfDictionary = obj as PdfDictionary;
      if (pdfDictionary.ContainsKey("Type") && pdfDictionary["Type"] as PdfName == new PdfName("Sig"))
        flag3 = true;
      if (pdfDictionary.ContainsKey("FT") && pdfDictionary["FT"] as PdfName == new PdfName("Sig"))
        flag3 = true;
    }
    if (flag2 && this.IsCrossReferenceStream(writer.Document) && reference.GenNum == 0 && !flag3)
    {
      this.DoArchiveObject(obj, reference, writer);
    }
    else
    {
      this.RegisterObject(writer.Position, reference);
      this.DoSaveObject(obj, reference, writer);
      if (obj != this.m_archive)
        return;
      this.m_archive = (PdfArchiveStream) null;
    }
  }

  private void DoArchiveObject(IPdfPrimitive obj, PdfReference reference, PdfWriter writer)
  {
    if (this.m_archive == null)
    {
      this.m_archive = new PdfArchiveStream(this.m_document);
      this.SaveArchive(writer);
    }
    int objCount = this.m_archive.ObjCount;
    this.RegisterObject(this.m_archive, reference);
    this.m_archive.SaveObject(obj, reference);
    if (this.m_archive.ObjCount < 100)
      return;
    this.m_archive = (PdfArchiveStream) null;
  }

  private void SaveArchive(PdfWriter writer)
  {
    PdfCrossTable.ArchiveInfo archiveInfo = new PdfCrossTable.ArchiveInfo((PdfReference) null, this.m_archive);
    if (this.m_archives == null)
      this.m_archives = new List<PdfCrossTable.ArchiveInfo>(10);
    this.m_archives.Add(archiveInfo);
  }

  private void DoSaveObject(IPdfPrimitive obj, PdfReference reference, PdfWriter writer)
  {
    bool flag = false;
    if (writer != null && writer.isCompress)
      flag = true;
    long length = writer.Length;
    if (writer.ObtainStream().CanSeek && writer.Position != length)
      writer.Position = length;
    writer.Write(reference.ObjNum.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    writer.Write(" ");
    writer.Write(reference.GenNum.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    writer.Write(" ");
    writer.Write(nameof (obj));
    writer.Write(flag ? " " : "\r\n");
    lock (PdfDocument.Cache)
      obj.Save((IPdfWriter) writer);
    if ((object) (obj as PdfName) != null || obj is PdfNumber || obj is PdfNull)
      writer.Write("\r\n");
    if (writer.ObtainStream().CanRead)
    {
      Stream stream = writer.ObtainStream();
      BinaryReader binaryReader = new BinaryReader(stream);
      if (binaryReader.BaseStream.CanRead)
      {
        binaryReader.BaseStream.Position = stream.Length - 1L;
        if (binaryReader.ReadChar() != '\n')
          writer.Write("\r\n");
      }
    }
    else if (writer.ObtainStream().CanWrite)
      writer.Write("\r\n");
    writer.Write("endobj");
    writer.Write("\r\n");
  }

  private PdfDictionary GeneratePagesRoot()
  {
    IPdfPrimitive pdfPrimitive = this.DocumentCatalog["Pages"];
    if (pdfPrimitive == null)
      throw new PdfDocumentException("Invalid/Unknown/Unsupported format");
    return pdfPrimitive is PdfDictionary pdfDictionary ? pdfDictionary : throw new PdfDocumentException("Invalid/Unknown/Unsupported format");
  }

  private void SaveSections(PdfWriter writer)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    long objectNum = 0;
    long count;
    do
    {
      count = this.PrepareSubsection(ref objectNum);
      this.SaveSubsection(writer, objectNum, count);
      objectNum += count;
    }
    while (count != 0L);
  }

  private long PrepareSubsection(ref long objectNum)
  {
    long num1 = 0;
    int num2 = this.Count;
    if (num2 <= 0)
      num2 = this.Document.PdfObjects.Count + 1;
    if (num2 < this.Document.PdfObjects.m_maximumReferenceObjNumber)
    {
      num2 = this.Document.PdfObjects.m_maximumReferenceObjNumber;
      this.isIndexGreaterthanTotalObjectCount = true;
    }
    if (objectNum >= (long) num2)
      return num1;
    long key = objectNum;
    while (key < (long) num2 && !this.m_objects.ContainsKey(key))
      ++key;
    objectNum = key;
    for (; key < (long) num2 && this.m_objects.ContainsKey(key); ++key)
      ++num1;
    return num1;
  }

  private void SaveSubsection(PdfWriter writer, long objectNum, long count)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    if (count <= 0L || objectNum >= (long) this.Count && !this.isIndexGreaterthanTotalObjectCount)
      return;
    writer.Write($"{objectNum} {count}{"\r\n"}");
    for (long key = objectNum; key < objectNum + count; ++key)
    {
      PdfCrossTable.RegisteredObject registeredObject = this.m_objects[key];
      string text = PdfCrossTable.GetItem(registeredObject.Offset, (long) registeredObject.GenerationNumber, registeredObject.Type == ObjectType.Free);
      writer.Write(text);
    }
  }

  internal static string GetItem(long offset, long genNumber, bool isFree)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(offset.ToString("0000000000 "));
    stringBuilder.Append(((ushort) genNumber).ToString("00000 "));
    stringBuilder.Append(isFree ? "f" : "n");
    stringBuilder.Append("\r\n");
    return stringBuilder.ToString();
  }

  private void SaveTrailer(PdfWriter writer, long count, long prevXRef)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.Write("trailer\r\n");
    PdfDictionary trailer = this.Trailer;
    if (prevXRef != 0L)
      trailer["Prev"] = (IPdfPrimitive) new PdfNumber(prevXRef);
    this.ForceIDHex(trailer);
    trailer["Size"] = (IPdfPrimitive) new PdfNumber(this.m_count);
    new PdfDictionary(trailer) { Encrypt = false }.Save((IPdfWriter) writer);
  }

  private void ForceIDHex(PdfDictionary trailer)
  {
    if (!(PdfCrossTable.Dereference(trailer["ID"]) is PdfArray pdfArray))
      return;
    foreach (PdfString pdfString in pdfArray)
    {
      pdfString.Encode = PdfString.ForceEncoding.ASCII;
      pdfString.ToHex();
    }
  }

  private void SaveTheEndess(PdfWriter writer, long xrefPos)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof (writer));
    writer.Write("\r\nstartxref\r\n");
    writer.Write(xrefPos.ToString() + "\r\n");
    writer.Write("%%EOF\r\n");
  }

  private void SaveHead(PdfWriter writer)
  {
    byte[] data = new byte[5]
    {
      (byte) 37,
      (byte) 131,
      (byte) 146,
      (byte) 250,
      (byte) 254
    };
    writer.Write("%PDF-");
    string fileVersion = this.GenerateFileVersion(writer.Document);
    writer.Write(fileVersion);
    writer.Write("\r\n");
    writer.Write(data);
    writer.Write("\r\n");
  }

  private string GenerateFileVersion(PdfDocumentBase document)
  {
    if (document == null)
      throw new ArgumentNullException(nameof (document));
    if (PdfDocument.ConformanceLevel == PdfConformanceLevel.Pdf_X1A2001)
      document.FileStructure.Version = PdfVersion.Version1_3;
    int version = (int) document.FileStructure.Version;
    if (document.FileStructure.Version != PdfVersion.Version2_0)
      return "1." + version.ToString();
    string fileVersion = "2.0";
    if (document is PdfLoadedDocument && document.FileStructure.IncrementalUpdate)
      document.Catalog["Version"] = (IPdfPrimitive) new PdfName(fileVersion);
    return fileVersion;
  }

  private bool IsCrossReferenceStream(PdfDocumentBase document)
  {
    if (document == null)
      throw new ArgumentNullException(nameof (document));
    return this.m_crossTable == null ? document.FileStructure.CrossReferenceType == PdfCrossReferenceType.CrossReferenceStream : this.m_crossTable.Trailer is PdfStream;
  }

  public void Dispose() => this.Dispose(true);

  internal void Close(bool completely)
  {
    if (completely)
    {
      if (this.m_archives != null)
      {
        this.m_archives.Clear();
        this.m_archives = (List<PdfCrossTable.ArchiveInfo>) null;
      }
      if (this.m_archive != null)
      {
        this.m_archive.Clear();
        this.m_archive = (PdfArchiveStream) null;
      }
      if (this.m_items != null && this.m_items.Count > 0 && completely)
      {
        for (int index = this.m_items.Count - 1; index >= 0; --index)
        {
          ObjectInfo objectInfo = this.m_items[index];
          this.m_items.Remove(index);
          if (objectInfo.Object is PdfStream)
            (objectInfo.Object as PdfStream).Clear();
          else if (objectInfo.Object is PdfCatalog)
            (objectInfo.Object as PdfCatalog).Clear();
          else if (objectInfo.Object is PdfArray)
            (objectInfo.Object as PdfArray).Clear();
        }
        if (this.m_items.mainObjectCollection != null)
        {
          foreach (KeyValuePair<long, ObjectInfo> mainObject in this.m_items.mainObjectCollection)
          {
            if (mainObject.Value.Object is PdfStream)
              (mainObject.Value.Object as PdfStream).Clear();
            else if (mainObject.Value.Object is PdfCatalog)
              (mainObject.Value.Object as PdfCatalog).Clear();
            if (mainObject.Value.Object is PdfArray)
              (mainObject.Value.Object as PdfArray).Clear();
          }
          this.m_items.mainObjectCollection.Clear();
        }
      }
      if (this.m_preReference != null)
      {
        this.m_preReference.Clear();
        this.m_preReference = (List<PdfReference>) null;
      }
      if (this.m_mappedReferences != null)
      {
        this.m_mappedReferences.Clear();
        this.m_mappedReferences = (Dictionary<PdfReference, PdfReference>) null;
      }
      if (this.m_objNumbers != null)
      {
        this.m_objNumbers.Clear();
        this.m_objNumbers = (Stack<PdfReference>) null;
      }
      if (this.m_pageCorrespondance != null)
      {
        if (this.isCompletely)
        {
          foreach (KeyValuePair<IPdfPrimitive, object> keyValuePair in this.m_pageCorrespondance)
          {
            if (keyValuePair.Key is PdfStream)
              (keyValuePair.Key as PdfStream).Dispose();
            else if (keyValuePair.Key is PdfDictionary)
              (keyValuePair.Key as PdfDictionary).Clear();
            else if (keyValuePair.Key is PdfArray)
              (keyValuePair.Key as PdfArray).Clear();
            else if ((object) (keyValuePair.Key as PdfReferenceHolder) != null)
            {
              IPdfPrimitive pdfPrimitive = (keyValuePair.Key as PdfReferenceHolder).Object;
              if (pdfPrimitive is PdfStream pdfStream)
                pdfStream.Clear();
              if (pdfPrimitive is PdfDictionary pdfDictionary)
                pdfDictionary.Clear();
              if (pdfPrimitive as PdfReference != (PdfReference) null)
                ;
            }
          }
        }
        this.m_pageCorrespondance.Clear();
        this.m_pageCorrespondance = (Dictionary<IPdfPrimitive, object>) null;
      }
    }
    this.Dispose();
  }

  public void Dispose(bool completely)
  {
    if (this.m_bDisposed)
      return;
    if (this.m_stream != null && this.isDisposed)
    {
      this.m_stream.Dispose();
      this.m_stream = (Stream) null;
    }
    if (this.m_objects != null)
    {
      this.m_objects.Clear();
      this.m_objects = (Dictionary<long, PdfCrossTable.RegisteredObject>) null;
    }
    if (this.isCompletely && this.m_crossTable != null)
      this.m_crossTable.Dispose();
    this.m_crossTable = (CrossTable) null;
    this.m_documentCatalog = (PdfDictionary) null;
    this.m_trailer = (IPdfPrimitive) null;
    this.m_document = (PdfDocumentBase) null;
    this.m_bDisposed = true;
    this.m_items = (PdfMainObjectCollection) null;
  }

  public class RegisteredObject
  {
    private long m_objectNumber;
    public int GenerationNumber;
    private long m_offset;
    private PdfArchiveStream m_archive;
    public ObjectType Type;
    private PdfCrossTable m_xrefTable;

    internal long ObjectNumber
    {
      get
      {
        if (this.m_objectNumber == 0L && this.m_archive != null)
          this.m_objectNumber = this.m_xrefTable.GetReference((IPdfPrimitive) this.m_archive).ObjNum;
        return this.m_objectNumber;
      }
    }

    internal long Offset
    {
      get => this.m_archive == null ? this.m_offset : (long) this.m_archive.GetIndex(this.m_offset);
    }

    public RegisteredObject(long offset, PdfReference reference)
    {
      if (reference == (PdfReference) null)
        throw new ArgumentNullException(nameof (reference));
      this.m_offset = offset;
      this.GenerationNumber = reference.GenNum;
      this.m_objectNumber = reference.ObjNum;
      this.Type = ObjectType.Normal;
    }

    public RegisteredObject(long offset, PdfReference reference, bool free)
      : this(offset, reference)
    {
      if (reference == (PdfReference) null)
        throw new ArgumentNullException(nameof (reference));
      this.Type = free ? ObjectType.Free : ObjectType.Normal;
    }

    public RegisteredObject(
      PdfCrossTable xrefTable,
      PdfArchiveStream archive,
      PdfReference reference)
    {
      this.m_xrefTable = xrefTable;
      this.m_archive = archive;
      this.m_offset = reference.ObjNum;
      this.Type = ObjectType.Packed;
    }
  }

  internal class ArchiveInfo
  {
    public PdfReference Reference;
    public PdfArchiveStream Archive;

    public ArchiveInfo(PdfReference reference, PdfArchiveStream archive)
    {
      this.Reference = reference;
      this.Archive = archive;
    }
  }
}
