// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.JPXFormatReader
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf;

internal class JPXFormatReader
{
  private JPXRandomAccessStream in_Renamed;
  private List<object> codeStreamPos;
  private List<object> codeStreamLength;
  public bool JP2FFUsed;

  public virtual long[] CodeStreamPos
  {
    get
    {
      int count = this.codeStreamPos.Count;
      long[] codeStreamPos = new long[count];
      for (int index = 0; index < count; ++index)
        codeStreamPos[index] = (long) (int) this.codeStreamPos[index];
      return codeStreamPos;
    }
  }

  public virtual int FirstCodeStreamPos => (int) this.codeStreamPos[0];

  public virtual int FirstCodeStreamLength => (int) this.codeStreamLength[0];

  internal JPXFormatReader(JPXRandomAccessStream in_Renamed) => this.in_Renamed = in_Renamed;

  public virtual void readFileFormat()
  {
    long longLength = 0;
    bool flag1 = false;
    bool flag2 = false;
    try
    {
      if (this.in_Renamed.readInt() != 12 || this.in_Renamed.readInt() != 1783636000 || this.in_Renamed.readInt() != 218793738)
      {
        this.in_Renamed.seek(0);
        if (this.in_Renamed.readShort() != (short) -177)
          this.JP2FFUsed = false;
        this.in_Renamed.seek(0);
        return;
      }
      this.JP2FFUsed = true;
      if (this.readFileTypeBox())
        ;
      while (!flag2)
      {
        int pos = this.in_Renamed.Pos;
        int length = this.in_Renamed.readInt();
        if (pos + length == this.in_Renamed.length())
          flag2 = true;
        int num1 = this.in_Renamed.readInt();
        switch (length)
        {
          case 0:
            flag2 = true;
            length = this.in_Renamed.length() - this.in_Renamed.Pos;
            break;
          case 1:
            this.in_Renamed.readLong();
            throw new IOException("File too long.");
          default:
            longLength = 0L;
            break;
        }
        switch (num1)
        {
          case 1785737827:
            int num2 = flag1 ? 1 : 0;
            this.readContiguousCodeStreamBox((long) pos, length, longLength);
            break;
          case 1785737832:
            if (flag1)
              this.readJP2HeaderBox((long) pos, length, longLength);
            flag1 = true;
            break;
        }
        if (!flag2)
          this.in_Renamed.seek(pos + length);
      }
    }
    catch (EndOfStreamException ex)
    {
    }
    int count = this.codeStreamPos.Count;
  }

  public virtual bool readFileTypeBox()
  {
    bool flag = false;
    int pos = this.in_Renamed.Pos;
    int num = this.in_Renamed.readInt();
    if (this.in_Renamed.readInt() != 1718909296)
      return false;
    if (num == 1)
    {
      this.in_Renamed.readLong();
      throw new IOException("File too long.");
    }
    this.in_Renamed.readInt();
    this.in_Renamed.readInt();
    for (int index = (num - 16 /*0x10*/) / 4; index > 0; --index)
    {
      if (this.in_Renamed.readInt() == 1785737760)
        flag = true;
    }
    return flag;
  }

  public virtual bool readJP2HeaderBox(long pos, int length, long longLength) => true;

  public virtual bool readContiguousCodeStreamBox(long pos, int length, long longLength)
  {
    int pos1 = this.in_Renamed.Pos;
    if (this.codeStreamPos == null)
      this.codeStreamPos = new List<object>(10);
    this.codeStreamPos.Add((object) pos1);
    if (this.codeStreamLength == null)
      this.codeStreamLength = new List<object>(10);
    this.codeStreamLength.Add((object) length);
    return true;
  }
}
