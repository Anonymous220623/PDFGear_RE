// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.IO.ObjectInformation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;

#nullable disable
namespace Syncfusion.Pdf.IO;

internal class ObjectInformation
{
  private ObjectType m_type;
  private ArchiveInformation m_archive;
  private PdfParser m_parser;
  private long m_offset;
  private CrossTable m_crossTable;
  public IPdfPrimitive Obj;

  public ObjectType Type => this.m_type;

  public PdfParser Parser
  {
    get
    {
      if (this.m_parser == null)
        this.m_parser = this.m_crossTable.RetrieveParser(this.m_archive);
      return this.m_parser;
    }
  }

  public long Offset
  {
    get
    {
      if (this.m_offset == 0L)
      {
        PdfParser parser = this.Parser;
        parser.StartFrom(0L);
        long num = 0;
        if (this.Archive != null)
        {
          if (this.Archive.Archive["N"] is PdfNumber pdfNumber1)
            num = pdfNumber1.LongValue;
          long[] numArray = new long[num * 2L];
          if (this.m_crossTable.archiveIndices.ContainsKey(this.Archive.Archive))
          {
            numArray = this.m_crossTable.archiveIndices[this.Archive.Archive];
          }
          else
          {
            for (int index = 0; (long) index < num; ++index)
            {
              if (parser.Simple() is PdfNumber pdfNumber2)
                numArray[index * 2] = pdfNumber2.LongValue;
              if (parser.Simple() is PdfNumber pdfNumber3)
                numArray[index * 2 + 1] = pdfNumber3.LongValue;
            }
            this.m_crossTable.archiveIndices[this.Archive.Archive] = numArray;
          }
          long index1 = this.Archive.Index;
          if (index1 * 2L >= (long) numArray.Length)
            throw new PdfDocumentException("Missing indexes in archive #" + (object) this.Archive.ArchiveNumber);
          this.m_offset = numArray[index1 * 2L + 1L];
          this.m_offset += (long) (this.Archive.Archive["First"] as PdfNumber).IntValue;
        }
      }
      return this.m_offset;
    }
  }

  public ArchiveInformation Archive => this.m_archive;

  public static implicit operator long(ObjectInformation oi) => oi.Offset;

  public ObjectInformation(
    ObjectType type,
    long offset,
    ArchiveInformation arciveInfo,
    CrossTable crossTable)
  {
    this.m_type = type;
    this.m_offset = offset;
    this.m_archive = arciveInfo;
    this.m_crossTable = crossTable;
  }
}
