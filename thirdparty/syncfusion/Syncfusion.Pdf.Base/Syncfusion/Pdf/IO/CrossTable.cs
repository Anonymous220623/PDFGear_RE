// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.IO.CrossTable
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using Syncfusion.Pdf.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.IO;

internal class CrossTable
{
  private const int m_generationNumber = 65535 /*0xFFFF*/;
  private Stream m_stream;
  private PdfReader m_reader;
  private PdfParser m_parser;
  internal Dictionary<long, ObjectInformation> m_objects;
  private PdfDictionary m_trailer;
  private PdfReferenceHolder m_documentCatalog;
  private long m_startXRef;
  private Dictionary<PdfStream, PdfParser> m_readersTable = new Dictionary<PdfStream, PdfParser>();
  private Dictionary<long, PdfStream> m_archives = new Dictionary<long, PdfStream>();
  private PdfEncryptor m_encryptor;
  private PdfCrossTable m_crossTable;
  internal long m_initialNumberOfSubsection;
  internal long m_initialSubsectionCount;
  internal long m_totalNumberOfSubsection;
  private bool m_isStructureAltered;
  internal bool m_isOpenAndRepair;
  private long m_whiteSpace;
  internal bool validateSyntax;
  private Dictionary<long, List<ObjectInformation>> m_allTables = new Dictionary<long, List<ObjectInformation>>();
  internal Dictionary<PdfStream, long[]> archiveIndices = new Dictionary<PdfStream, long[]>();
  private bool m_repair;

  internal ObjectInformation this[long index]
  {
    get
    {
      object obj = this.m_objects.ContainsKey(index) ? (object) this.m_objects[index] : (object) (ObjectInformation) null;
      return obj != null ? obj as ObjectInformation : (ObjectInformation) null;
    }
  }

  internal Dictionary<long, List<ObjectInformation>> AllTables => this.m_allTables;

  public long Count => (long) this.m_objects.Count;

  public PdfReferenceHolder DocumentCatalog
  {
    get
    {
      if (this.m_documentCatalog == (PdfReferenceHolder) null)
      {
        IPdfPrimitive pdfPrimitive = this.Trailer["Root"];
        this.m_documentCatalog = (object) (pdfPrimitive as PdfReferenceHolder) != null ? pdfPrimitive as PdfReferenceHolder : throw new PdfDocumentException("Invalid/Unknown/Unsupported format");
      }
      return this.m_documentCatalog;
    }
  }

  internal Stream Stream => this.m_stream;

  internal long XRefOffset => this.m_startXRef;

  public PdfReader Reader
  {
    get
    {
      if (this.m_reader == null)
        this.m_reader = new PdfReader(this.m_stream);
      return this.m_reader;
    }
  }

  public PdfParser Parser
  {
    get
    {
      if (this.m_parser == null)
        this.m_parser = new PdfParser(this, this.Reader, this.m_crossTable);
      return this.m_parser;
    }
  }

  internal PdfDictionary Trailer => this.m_trailer;

  internal bool IsStructureAltered => this.m_isStructureAltered;

  internal PdfEncryptor Encryptor
  {
    get => this.m_encryptor;
    set
    {
      this.m_encryptor = value != null ? value : throw new ArgumentNullException("m_encryptor");
    }
  }

  public CrossTable(Stream docStream, PdfCrossTable crossTable)
  {
    if (docStream == null)
      throw new ArgumentNullException(nameof (docStream));
    if (!docStream.CanSeek || !docStream.CanRead)
      throw new PdfDocumentException("Ivalid stream.");
    if (crossTable == null)
      throw new ArgumentNullException(nameof (crossTable));
    if (crossTable.loadedPdfDocument != null)
      this.validateSyntax = true;
    this.m_isOpenAndRepair = crossTable.isOpenAndRepair;
    this.m_repair = crossTable.isRepair;
    this.m_stream = docStream;
    int num1 = this.CheckJunk();
    if (num1 < 0)
      throw new PdfException("Could not find valid signature (%PDF-).");
    this.m_crossTable = crossTable;
    this.m_objects = new Dictionary<long, ObjectInformation>();
    PdfReader reader = this.Reader;
    PdfParser parser = this.Parser;
    reader.Position = (long) num1;
    reader.SkipWS();
    this.m_whiteSpace = reader.Position;
    long num2 = reader.Seek(0L, SeekOrigin.End);
    this.CheckStartxref();
    long num3 = reader.SearchBack("%%EOF");
    if (num3 != -1L && !this.m_isOpenAndRepair)
    {
      if (num2 != num3 + 5L)
      {
        reader.Position = num3 + 5L;
        string nextToken = reader.GetNextToken();
        if (nextToken != string.Empty && nextToken[0] != char.MinValue && nextToken[0] != '0')
        {
          Stream stream = (Stream) new MemoryStream();
          this.m_stream.Position = 0L;
          byte[] buffer = new byte[num3 + 5L];
          this.m_stream.Read(buffer, 0, buffer.Length);
          stream.Write(buffer, 0, buffer.Length);
          reader = new PdfReader(stream);
          parser = new PdfParser(this, reader, this.m_crossTable);
          this.m_reader = reader;
          this.m_parser = parser;
        }
      }
    }
    else
    {
      if (this.validateSyntax)
      {
        PdfException pdfException = new PdfException("The document does not contain EOF.");
        crossTable.loadedPdfDocument.pdfException.Add(pdfException);
      }
      if (num3 == -1L)
        reader.Position = num2;
    }
    long position1 = reader.Position;
    long offset1 = reader.SearchBack("startxref");
    long num4 = offset1;
    bool flag = false;
    long offset2;
    if (offset1 < 0L)
    {
      reader.Position = position1;
      string str = "startxref";
      for (int startIndex = 0; startIndex < str.Length; ++startIndex)
      {
        string token = str.Remove(startIndex, 1);
        if (reader.SearchBack(token) <= 0L)
          reader.Position = position1;
        else
          break;
      }
      reader.Position += (long) "startxref".Length;
      offset2 = Convert.ToInt64(reader.ReadLine());
      if (!crossTable.isOpenAndRepair)
        throw new Exception("Document has corrupted cross reference table");
      parser.RebuildXrefTable(this.m_objects, this);
      long num5 = reader.SearchBack("xref");
      if (num5 != -1L)
        offset2 = num5;
      parser.SetOffset(offset2);
    }
    else
    {
      parser.SetOffset(offset1);
      offset2 = parser.StartXRef();
      this.m_startXRef = offset2;
      parser.SetOffset(offset2);
      if (this.m_whiteSpace != 0L)
      {
        long num6 = reader.SearchForward("xref");
        if (num6 == -1L)
        {
          flag = false;
          offset2 += this.m_whiteSpace;
          this.m_stream.Position = offset2;
        }
        else
        {
          offset2 = num6;
          parser.SetOffset(offset2);
          flag = true;
        }
      }
    }
    string str1 = reader.ReadLine();
    if (!str1.Contains("xref") && !str1.Contains("obj") && !flag)
    {
      long position2 = reader.Position;
      string str2 = reader.ReadLine();
      if (str2.Contains("xref"))
      {
        str1 = str2;
        offset2 = position2;
      }
      else
        reader.Position = !crossTable.isOpenAndRepair ? position2 : num4;
    }
    if (crossTable.isRepair && !str1.Contains("xref") && !str1.Contains("obj") && !flag)
    {
      crossTable.isRepair = false;
      crossTable.isOpenAndRepair = true;
      this.m_isOpenAndRepair = true;
    }
    if (!str1.Contains("xref") && !str1.Contains("obj") && !flag && !crossTable.isRepair)
    {
      if (offset2 > this.m_stream.Length)
      {
        long length = this.m_stream.Length;
        reader.Position = length;
        offset2 = reader.SearchBack("startxref");
      }
      long num7 = reader.SearchBack("xref");
      if (num7 != -1L)
        offset2 = num7;
      if (crossTable.isOpenAndRepair)
      {
        if (num7 == -1L)
        {
          long num8 = reader.SearchForward("xref");
          if (num8 != -1L)
            offset2 = num8;
        }
        parser.RebuildXrefTable(this.m_objects, this);
      }
      parser.SetOffset(offset2);
    }
    else if (this.m_isOpenAndRepair)
    {
      if (str1 != "xref")
      {
        long num9 = reader.SearchBack("xref");
        if (num9 != -1L)
          offset2 = num9;
        if (num9 == -1L)
        {
          long num10 = reader.SearchForward("xref");
          if (num10 != -1L)
            offset2 = num10;
        }
      }
      parser.RebuildXrefTable(this.m_objects, this);
      parser.SetOffset(offset2);
      flag = false;
    }
    reader.Position = offset2;
    try
    {
      this.m_trailer = parser.ParseXRefTable(this.m_objects, this) as PdfDictionary;
    }
    catch (FileLoadException ex)
    {
      throw ex;
    }
    catch (Exception ex)
    {
      throw new Exception("Invalid cross reference table.");
    }
    PdfDictionary pdfDictionary = this.m_trailer;
    while (pdfDictionary.ContainsKey("Prev"))
    {
      if (this.m_whiteSpace != 0L || this.m_isOpenAndRepair)
      {
        (pdfDictionary["Prev"] as PdfNumber).IntValue += (int) this.m_whiteSpace;
        this.m_isStructureAltered = true;
      }
      long intValue = (long) (pdfDictionary["Prev"] as PdfNumber).IntValue;
      PdfReader pdfReader = new PdfReader(this.m_reader.Stream);
      pdfReader.Position = intValue;
      long num11 = intValue;
      if (!pdfReader.GetNextToken().Equals("xref"))
      {
        string nextToken = pdfReader.GetNextToken();
        int result = 0;
        if (int.TryParse(nextToken, out result) && result >= 0 && result <= 9 && pdfReader.GetNextToken().Equals("obj"))
        {
          parser.SetOffset(intValue);
          pdfDictionary = parser.ParseXRefTable(this.m_objects, this) as PdfDictionary;
        }
        else
        {
          parser.RebuildXrefTable(this.m_objects, this);
          break;
        }
      }
      else
      {
        parser.SetOffset(intValue);
        pdfDictionary = parser.ParseXRefTable(this.m_objects, this) as PdfDictionary;
        if (pdfDictionary.ContainsKey("Size") && this.m_trailer.ContainsKey("Size") && (pdfDictionary["Size"] as PdfNumber).IntValue > (this.m_trailer["Size"] as PdfNumber).IntValue)
          (this.m_trailer["Size"] as PdfNumber).IntValue = (pdfDictionary["Size"] as PdfNumber).IntValue;
        if (crossTable.isRepair && this.m_whiteSpace == 0L)
          this.ReadAllObjects(this.m_objects, parser);
        if (this.m_isOpenAndRepair && pdfDictionary != null && pdfDictionary.ContainsKey("Prev") && (long) (pdfDictionary["Prev"] as PdfNumber).IntValue == num11)
          break;
      }
    }
    if (this.m_whiteSpace != 0L && flag)
    {
      long[] array = new long[this.m_objects.Count];
      this.m_objects.Keys.CopyTo(array, 0);
      for (int index = 0; index < array.Length; ++index)
      {
        long key = array[index];
        ObjectInformation objectInformation = this.m_objects[key];
        this.m_objects[key] = new ObjectInformation(ObjectType.Normal, objectInformation.Offset + this.m_whiteSpace, (ArchiveInformation) null, this);
      }
      this.m_isStructureAltered = true;
    }
    else if (this.m_whiteSpace != 0L && this.m_whiteSpace > 0L && !this.m_isOpenAndRepair && !this.m_isStructureAltered && !pdfDictionary.ContainsKey("Prev"))
      this.m_isStructureAltered = true;
    if (!this.m_isOpenAndRepair || !this.m_trailer.ContainsKey("Prev") || !this.m_isStructureAltered)
      return;
    this.m_trailer.Remove("Prev");
  }

  private void ReadAllObjects(Dictionary<long, ObjectInformation> objects, PdfParser parser)
  {
    foreach (KeyValuePair<long, ObjectInformation> keyValuePair in objects)
      parser.Parse(keyValuePair.Value.Offset);
  }

  internal CrossTable(Stream docStream, PdfCrossTable crossTable, bool isFdf)
  {
    this.m_stream = docStream;
    this.m_crossTable = crossTable;
    this.m_objects = new Dictionary<long, ObjectInformation>();
  }

  private void CheckStartxref()
  {
    long count = 1024 /*0x0400*/;
    long offset = this.m_stream.Length - count;
    if (offset < 1L)
      offset = 1L;
    byte[] numArray = new byte[count];
    for (; offset > 0L; offset = offset - count + 9L)
    {
      this.m_stream.Seek(offset, SeekOrigin.Begin);
      this.m_stream.Read(numArray, 0, (int) count);
      if (Encoding.Default.GetString(numArray).LastIndexOf("startxref") >= 0)
        break;
    }
    if (offset < 0L && !this.m_crossTable.isOpenAndRepair)
      throw new Exception("Cannot load document, the document structure has been corrupted.");
  }

  public IPdfPrimitive GetObject(IPdfPrimitive pointer)
  {
    if (pointer == null)
      throw new ArgumentNullException(nameof (pointer));
    if ((object) (pointer as PdfReference) == null)
      return pointer;
    PdfReference pdfReference = pointer as PdfReference;
    ObjectInformation objectInformation = this[pdfReference.ObjNum];
    if (objectInformation == null)
      return (IPdfPrimitive) new PdfNull();
    if (this.m_crossTable.Encrypted)
      objectInformation.Parser.Encrypted = true;
    PdfParser parser = objectInformation.Parser;
    long offset = objectInformation.Offset;
    IPdfPrimitive pdfPrimitive;
    if (objectInformation.Obj != null)
      pdfPrimitive = objectInformation.Obj;
    else if (objectInformation.Archive == null)
    {
      pdfPrimitive = parser.Parse(offset);
    }
    else
    {
      pdfPrimitive = this.GetObject(parser, offset);
      if (this.Encryptor != null)
      {
        if (pdfPrimitive is PdfDictionary)
        {
          PdfDictionary pdfDictionary = pdfPrimitive as PdfDictionary;
          pdfDictionary.IsDecrypted = true;
          foreach (object obj in pdfDictionary.Items.Values)
          {
            if (obj is PdfString)
              (obj as PdfString).IsParentDecrypted = true;
          }
        }
        if (pdfPrimitive is PdfArray)
        {
          foreach (object obj1 in pdfPrimitive as PdfArray)
          {
            if (obj1 is PdfString)
            {
              if (objectInformation.Type == ObjectType.Packed)
                (obj1 as PdfString).IsPacked = true;
            }
            else if (obj1 is PdfArray)
            {
              foreach (object obj2 in obj1 as PdfArray)
              {
                if (obj2 is PdfString && objectInformation.Type == ObjectType.Packed)
                  (obj2 as PdfString).IsPacked = true;
              }
            }
          }
        }
        if (pdfPrimitive is IPdfDecryptable pdfDecryptable)
          pdfDecryptable.Decrypt(this.Encryptor, pdfReference.ObjNum);
      }
    }
    objectInformation.Obj = pdfPrimitive;
    return pdfPrimitive;
  }

  public byte[] GetStream(IPdfPrimitive streamRef)
  {
    if (streamRef == null)
      throw new ArgumentNullException(nameof (streamRef));
    return this.GetObject(streamRef) is PdfStream pdfStream ? pdfStream.Data : (byte[]) null;
  }

  internal void ParseNewTable(PdfStream stream, Dictionary<long, ObjectInformation> hashTable)
  {
    if (stream == null)
      throw new PdfDocumentException("Invalid/Unknown/Unsupported format");
    stream.Decompress();
    List<CrossTable.SubSection> sections = this.GetSections(stream);
    int startIndex = 0;
    foreach (CrossTable.SubSection subsection in sections)
      startIndex = this.ParseSubsection(stream, subsection, hashTable, startIndex);
  }

  internal void ParseSubsection(PdfParser parser, Dictionary<long, ObjectInformation> table)
  {
    this.m_initialNumberOfSubsection = (long) (parser.Simple() as PdfNumber).IntValue;
    this.m_totalNumberOfSubsection = (long) (parser.Simple() as PdfNumber).IntValue;
    this.m_initialSubsectionCount = this.m_initialNumberOfSubsection;
    for (int index = 0; (long) index < this.m_totalNumberOfSubsection; ++index)
    {
      long longValue = (parser.Simple() as PdfNumber).LongValue;
      int intValue = (parser.Simple() as PdfNumber).IntValue;
      if (parser.GetObjectFlag() == 'n')
      {
        ObjectInformation oi = new ObjectInformation(ObjectType.Normal, longValue, (ArchiveInformation) null, this);
        long num = this.m_initialSubsectionCount != this.m_initialNumberOfSubsection ? this.m_initialSubsectionCount + (long) index : this.m_initialNumberOfSubsection + (long) index;
        if (!table.ContainsKey(num))
          table[num] = oi;
        this.AddTables(num, oi);
      }
      else if (this.m_initialNumberOfSubsection != 0L && longValue == 0L && intValue == (int) ushort.MaxValue)
      {
        --this.m_initialNumberOfSubsection;
        if (index == 0 && this.m_initialNumberOfSubsection == 0L)
          this.m_initialSubsectionCount = this.m_initialNumberOfSubsection;
      }
    }
  }

  private void AddTables(long objectOffset, ObjectInformation oi)
  {
    if (this.m_allTables.ContainsKey(objectOffset))
      this.m_allTables[objectOffset].Add(oi);
    else
      this.m_allTables.Add(objectOffset, new List<ObjectInformation>()
      {
        oi
      });
  }

  internal PdfParser RetrieveParser(ArchiveInformation archive)
  {
    if (archive == null)
      return this.m_parser;
    PdfStream archive1 = archive.Archive;
    PdfParser pdfParser = (PdfParser) null;
    if (this.m_readersTable.ContainsKey(archive1))
      pdfParser = this.m_readersTable[archive1];
    if (pdfParser == null)
    {
      pdfParser = new PdfParser(this, new PdfReader((Stream) new MemoryStream(archive1.Data, false)), this.m_crossTable);
      this.m_readersTable[archive1] = pdfParser;
    }
    return pdfParser;
  }

  private PdfStream RetrieveArchive(long archiveNumber)
  {
    PdfStream pdfStream = (PdfStream) null;
    if (this.m_archives.ContainsKey(archiveNumber))
      pdfStream = this.m_archives[archiveNumber];
    if (pdfStream == null)
    {
      ObjectInformation objectInformation = this[archiveNumber];
      pdfStream = objectInformation.Parser.Parse(objectInformation.Offset) as PdfStream;
      if (this.Encryptor != null && !this.Encryptor.EncryptOnlyAttachment)
        pdfStream.Decrypt(this.Encryptor, archiveNumber);
      pdfStream.Decompress();
      this.m_archives[archiveNumber] = pdfStream;
    }
    return pdfStream;
  }

  private List<CrossTable.SubSection> GetSections(PdfStream stream)
  {
    List<CrossTable.SubSection> sections = new List<CrossTable.SubSection>();
    int intValue1 = (stream["Size"] as PdfNumber).IntValue;
    if (intValue1 == 0)
      throw new PdfDocumentException("Invalid/Unknown/Unsupported format");
    IPdfPrimitive pointer = stream["Index"];
    if (pointer == null)
    {
      sections.Add(new CrossTable.SubSection(intValue1));
    }
    else
    {
      if (!(this.GetObject(pointer) is PdfArray pdfArray))
        throw new PdfDocumentException("Invalid/Unknown/Unsupported format");
      if ((pdfArray.Count & 1) != 0)
        throw new PdfDocumentException("Invalid/Unknown/Unsupported format");
      int index1;
      for (int index2 = 0; index2 < pdfArray.Count; index2 = index1 + 1)
      {
        int intValue2 = (pdfArray[index2] as PdfNumber).IntValue;
        index1 = index2 + 1;
        int intValue3 = (pdfArray[index1] as PdfNumber).IntValue;
        sections.Add(new CrossTable.SubSection(intValue2, intValue3));
      }
    }
    return sections;
  }

  private int ParseSubsection(
    PdfStream stream,
    CrossTable.SubSection subsection,
    Dictionary<long, ObjectInformation> table,
    int startIndex)
  {
    int subsection1 = startIndex;
    PdfArray pdfArray = this.GetObject(stream["W"]) as PdfArray;
    long count1 = (long) pdfArray.Count;
    long[] numArray1 = new long[count1];
    for (int index = 0; (long) index < count1; ++index)
    {
      if (pdfArray[index] is PdfNumber pdfNumber)
        numArray1[index] = pdfNumber.LongValue;
    }
    long[] numArray2 = new long[count1];
    byte[] data = stream.Data;
    int num1 = 0;
    for (int count2 = subsection.Count; num1 < count2; ++num1)
    {
      for (long index1 = 0; index1 < count1; ++index1)
      {
        long num2 = 0;
        if (index1 == 0L)
          num2 = numArray1[index1] <= 0L ? 1L : 0L;
        for (int index2 = 0; (long) index2 < numArray1[index1]; ++index2)
          num2 = (num2 << 8) + (long) data[subsection1++];
        numArray2[index1] = num2;
      }
      long offset = 0;
      ArchiveInformation arciveInfo = (ArchiveInformation) null;
      if (numArray2[0] == 1L)
        offset = this.m_whiteSpace != 0L || this.m_isOpenAndRepair ? numArray2[1] + this.m_whiteSpace : numArray2[1];
      else if (numArray2[0] == 2L)
        arciveInfo = new ArchiveInformation(numArray2[1], numArray2[2], new GetArchive(this.RetrieveArchive));
      ObjectInformation oi = (ObjectInformation) null;
      if (numArray2[0] != 0L)
        oi = new ObjectInformation((ObjectType) numArray2[0], offset, arciveInfo, this);
      if (oi != null)
      {
        long num3 = (long) (subsection.StartNumber + num1);
        if (!table.ContainsKey(num3))
          table[num3] = oi;
        this.AddTables(num3, oi);
      }
    }
    return subsection1;
  }

  private IPdfPrimitive GetObject(PdfParser parser, long position)
  {
    parser.StartFrom(position);
    return parser.Simple();
  }

  private int CheckJunk()
  {
    long length = 1024 /*0x0400*/;
    long num1 = 0;
    int num2;
    do
    {
      byte[] numArray = new byte[length];
      this.m_stream.Position = num1;
      int count = (int) length;
      if (this.m_stream.Length - num1 < (long) count)
        count = (int) (this.m_stream.Length - num1);
      this.m_stream.Read(numArray, 0, count);
      num2 = PdfString.ByteToString(numArray).IndexOf("%PDF-");
      if (num2 != -1 && (this.m_isOpenAndRepair || this.m_repair))
        num2 = (int) num1 + num2;
      num1 = this.m_stream.Position;
    }
    while (num2 < 0 && this.Stream.Position != this.Stream.Length);
    this.m_stream.Position = 0L;
    return num2;
  }

  internal void Dispose()
  {
    if (this.m_archives != null)
    {
      foreach (KeyValuePair<long, PdfStream> archive in this.m_archives)
        archive.Value.Dispose();
    }
    if (this.m_allTables != null)
    {
      this.m_allTables.Clear();
      this.m_allTables = (Dictionary<long, List<ObjectInformation>>) null;
    }
    if (this.archiveIndices != null)
    {
      this.archiveIndices.Clear();
      this.archiveIndices = (Dictionary<PdfStream, long[]>) null;
    }
    this.m_archives.Clear();
    this.m_documentCatalog = (PdfReferenceHolder) null;
    this.m_stream.Dispose();
    this.m_stream.Close();
    if (this.m_trailer != null)
    {
      if (this.m_trailer is PdfStream)
        (this.m_trailer as PdfStream).Dispose();
      else
        this.m_trailer.Clear();
    }
    this.m_parser = (PdfParser) null;
    this.m_reader = (PdfReader) null;
    if (this.m_objects == null)
      return;
    this.m_objects.Clear();
    this.m_objects = (Dictionary<long, ObjectInformation>) null;
  }

  private struct SubSection
  {
    public int StartNumber;
    public int Count;

    public SubSection(int start, int count)
    {
      this.StartNumber = start;
      this.Count = count;
    }

    public SubSection(int count)
    {
      this.StartNumber = 0;
      this.Count = count;
    }
  }
}
