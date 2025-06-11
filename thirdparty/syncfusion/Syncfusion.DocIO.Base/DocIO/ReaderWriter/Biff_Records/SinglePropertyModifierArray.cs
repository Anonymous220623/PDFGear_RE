// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.Biff_Records.SinglePropertyModifierArray
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.Biff_Records;

[CLSCompliant(false)]
internal class SinglePropertyModifierArray : BaseWordRecord, ICollection, IEnumerable
{
  private List<SinglePropertyModifierRecord> m_arrModifiers = new List<SinglePropertyModifierRecord>();

  internal SinglePropertyModifierArray()
  {
  }

  internal SinglePropertyModifierArray(byte[] data)
    : base(data)
  {
  }

  internal SinglePropertyModifierArray(byte[] arrData, int iOffset)
    : base(arrData, iOffset)
  {
  }

  internal SinglePropertyModifierArray(byte[] arrData, int iOffset, int iCount)
    : base(arrData, iOffset, iCount)
  {
  }

  internal SinglePropertyModifierArray(Stream stream, int iCount)
    : base(stream, iCount)
  {
  }

  internal void RemoveValue(int options)
  {
    for (SinglePropertyModifierRecord propertyModifierRecord = this[options]; propertyModifierRecord != null; propertyModifierRecord = this[options])
      this.m_arrModifiers.Remove(propertyModifierRecord);
  }

  internal override void Parse(byte[] arrData, int iOffset, int iCount)
  {
    if (arrData == null)
      throw new ArgumentNullException(nameof (arrData));
    if (iOffset < 0)
      throw new ArgumentOutOfRangeException("iOffset < 0");
    if (iCount < 0)
      throw new ArgumentOutOfRangeException("iCount < 0 ");
    if (iOffset + iCount > arrData.Length)
      throw new ArgumentOutOfRangeException("iOffset + iCount");
    this.Clear();
    int num = iOffset;
    while (iCount - (iOffset - num) > 1)
    {
      SinglePropertyModifierRecord sprm = new SinglePropertyModifierRecord();
      iOffset = sprm.Parse(arrData, iOffset);
      if (this.IsValidParagraphPropertySprm(sprm))
      {
        this.CheckDuplicateSprms(sprm);
        this.m_arrModifiers.Add(sprm);
      }
      else if (this.IsCorrectSprm(sprm))
        this.m_arrModifiers.Add(sprm);
    }
  }

  internal void CheckDuplicateSprms(SinglePropertyModifierRecord sprm)
  {
    for (int index = this.m_arrModifiers.Count - 1; index >= 0; --index)
    {
      SinglePropertyModifierRecord arrModifier = this.m_arrModifiers[index];
      if (arrModifier.OptionType == WordSprmOptionType.sprmPWall || arrModifier.OptionType == WordSprmOptionType.sprmCWall)
        break;
      if (sprm.OptionType == arrModifier.OptionType)
        this.m_arrModifiers.Remove(arrModifier);
    }
  }

  private bool IsCorrectSprm(SinglePropertyModifierRecord sprm)
  {
    return this.IsValidCharacterPropertySprm(sprm) || this.IsValidTablePropertySprm(sprm) || this.IsValidSectionPropertySprm(sprm) || this.IsValidPicturePropertySprm(sprm);
  }

  internal bool IsValidCharacterPropertySprm(SinglePropertyModifierRecord sprm)
  {
    switch (sprm.Options)
    {
      case 2048 /*0x0800*/:
        if (sprm.ByteValue != (byte) 0 && sprm.ByteValue != (byte) 1 && sprm.ByteValue != (byte) 128 /*0x80*/ && sprm.ByteValue != (byte) 129)
          sprm.ByteValue = (byte) 0;
        return true;
      case 2049:
      case 2050:
      case 2054:
      case 2058:
      case 2065:
      case 2072:
      case 2101:
      case 2102:
      case 2103:
      case 2104:
      case 2105:
      case 2106:
      case 2107:
      case 2108:
      case 2132:
      case 2133:
      case 2134:
      case 2136:
      case 2138:
      case 2140:
      case 2141:
      case 2152:
      case 2165:
      case 2178:
      case 10329:
      case 10351:
      case 10361:
      case 10764:
      case 10803:
      case 10804:
      case 10814:
      case 10818:
      case 10824:
      case 10835:
      case 10883:
      case 10886:
      case 10896:
      case 18436:
      case 18439:
      case 18501:
      case 18507:
      case 18510:
      case 18514:
      case 18527:
      case 18531:
      case 18534:
      case 18535:
      case 18541:
      case 18542:
      case 18547:
      case 18548:
      case 18568:
      case 18992:
      case 19011:
      case 19023:
      case 19024:
      case 19025:
      case 19038:
      case 19040:
      case 19041:
      case 26629:
      case 26645:
      case 26646:
      case 26647:
      case 26724:
      case 26725:
      case 26736:
      case 26743:
      case 26759:
      case 27139:
      case 27145:
      case 34880:
      case 51226:
      case 51761:
      case 51783:
      case 51799:
      case 51810:
      case 51825:
      case 51826:
      case 51830:
      case 51832:
      case 51845:
      case 51849:
        return true;
      default:
        return false;
    }
  }

  private bool IsValidParagraphPropertySprm(SinglePropertyModifierRecord sprm)
  {
    switch (sprm.Options)
    {
      case 9219:
      case 9221:
      case 9222:
      case 9223:
      case 9228:
      case 9238:
      case 9239:
      case 9251:
      case 9258:
      case 9264:
      case 9265:
      case 9267:
      case 9268:
      case 9269:
      case 9270:
      case 9271:
      case 9272:
      case 9281:
      case 9283:
      case 9287:
      case 9288:
      case 9291:
      case 9292:
      case 9306:
      case 9307:
      case 9308:
      case 9313:
      case 9314:
      case 9325:
      case 9328:
      case 9329:
      case 9730:
      case 9738:
      case 9755:
      case 9792:
      case 9828:
      case 17451:
      case 17452:
      case 17453:
      case 17465:
      case 17466:
      case 17493:
      case 17494:
      case 17495:
      case 17496:
      case 17497:
      case 17920:
      case 17931:
      case 17936:
      case 18015:
      case 25618:
      case 25636:
      case 25637:
      case 25638:
      case 25639:
      case 25640:
      case 25701:
      case 25703:
      case 25707:
      case 26153:
      case 26182:
      case 26185:
      case 26186:
      case 33806:
      case 33807:
      case 33809:
      case 33816:
      case 33817:
      case 33818:
      case 33838:
      case 33839:
      case 33885:
      case 33886:
      case 33888:
      case 42003:
      case 42004:
      case 50689:
      case 50701:
      case 50709:
      case 50757:
      case 50765:
      case 50766:
      case 50767:
      case 50768:
      case 50769:
      case 50770:
      case 50771:
      case 50790:
      case 50793:
      case 50796:
      case 50799:
        return true;
      default:
        return false;
    }
  }

  private bool IsValidTablePropertySprm(SinglePropertyModifierRecord sprm)
  {
    switch (sprm.Options)
    {
      case 13315:
      case 13316:
      case 13413:
      case 13414:
      case 13436:
      case 13437:
      case 13448:
      case 13449:
      case 13837:
      case 13845:
      case 13849:
      case 13928:
      case 21504:
      case 21642:
      case 22027:
      case 22050:
      case 22052:
      case 22053:
      case 22074:
      case 22116:
      case 29706:
      case 29801:
      case 29817:
      case 30241:
      case 30243:
      case 30249:
      case 37895:
      case 37902:
      case 37903:
      case 37904:
      case 37905:
      case 37918:
      case 37919:
      case 38401:
      case 38402:
      case 54399:
      case 54789:
      case 54792:
      case 54793:
      case 54796:
      case 54802:
      case 54803:
      case 54806:
      case 54810:
      case 54811:
      case 54812:
      case 54813:
      case 54816:
      case 54827:
      case 54828:
      case 54829:
      case 54830:
      case 54831:
      case 54834:
      case 54835:
      case 54836:
      case 54837:
      case 54841:
      case 54846:
      case 54850:
      case 54880:
      case 54882:
      case 54887:
      case 54890:
      case 54896:
      case 54897:
      case 54898:
      case 54912:
      case 54913:
      case 54914:
      case 54915:
      case 54916:
      case 54917:
      case 54918:
      case 54919:
      case 62996:
      case 62999:
      case 63000:
      case 63030:
      case 63073:
        return true;
      default:
        return false;
    }
  }

  private bool IsValidSectionPropertySprm(SinglePropertyModifierRecord sprm)
  {
    switch (sprm.Options)
    {
      case 12288 /*0x3000*/:
      case 12289:
      case 12293:
      case 12294:
      case 12297:
      case 12298:
      case 12302:
      case 12305:
      case 12306:
      case 12307:
      case 12313:
      case 12314:
      case 12317:
      case 12347:
      case 12348:
      case 12350:
      case 12840:
      case 12842:
      case 12857:
      case 20487:
      case 20488:
      case 20491:
      case 20501:
      case 20507:
      case 20508:
      case 20518:
      case 20530:
      case 20531:
      case 20543:
      case 20544:
      case 20545:
      case 20546:
      case 21039:
      case 28715:
      case 28716:
      case 28717:
      case 28718:
      case 28720:
      case 28730:
      case 28740:
      case 36876:
      case 36886:
      case 36899:
      case 36900:
      case 36913:
      case 45079:
      case 45080:
      case 45087:
      case 45088:
      case 45089:
      case 45090:
      case 45093:
      case 53812:
      case 53813:
      case 53814:
      case 53815:
      case 53827:
      case 61955:
      case 61956:
        return true;
      default:
        return false;
    }
  }

  private bool IsValidPicturePropertySprm(SinglePropertyModifierRecord sprm)
  {
    switch (sprm.Options)
    {
      case 27650:
      case 27651:
      case 27652:
      case 27653:
      case 52744:
      case 52745:
      case 52746:
      case 52747:
        return true;
      default:
        return false;
    }
  }

  internal override int Save(byte[] arrData, int iOffset)
  {
    int length = this.Length;
    if (length == 0)
      return 0;
    if (arrData == null)
      throw new ArgumentNullException(nameof (arrData));
    if (iOffset < 0 || iOffset + length > arrData.Length)
      throw new ArgumentOutOfRangeException(nameof (iOffset));
    int num1 = 0;
    int index = 0;
    for (int count = this.Count; index < count; ++index)
    {
      int num2 = this.m_arrModifiers[index].Save(arrData, iOffset);
      num1 += num2;
      iOffset += num2;
    }
    return num1;
  }

  internal int Save(BinaryWriter writer, Stream stream, int length)
  {
    if (length == 0)
      return 0;
    if (writer == null)
      throw new ArgumentNullException(nameof (stream));
    int num1 = 0;
    int index = 0;
    for (int count = this.Count; index < count; ++index)
    {
      SinglePropertyModifierRecord arrModifier = this.m_arrModifiers[index];
      if (arrModifier.Operand != null)
      {
        int num2 = arrModifier.Save(writer, stream);
        num1 += num2;
      }
    }
    return num1;
  }

  internal int Save(BinaryWriter writer, Stream stream)
  {
    long position = stream.Position;
    if (this.Modifiers.Count == 0)
      return 0;
    if (writer == null)
      throw new ArgumentNullException(nameof (stream));
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      this.m_arrModifiers[index].Save(writer, stream);
    return (int) (stream.Position - position);
  }

  internal void Clear() => this.m_arrModifiers.Clear();

  internal void Add(SinglePropertyModifierRecord modifier) => this.m_arrModifiers.Add(modifier);

  internal void SortSprms()
  {
    int trackChangeSprmIndex = 0;
    if (this.IsContainTrackChangesSprm(out trackChangeSprmIndex))
    {
      for (int index1 = 0; index1 < trackChangeSprmIndex; ++index1)
      {
        for (int index2 = index1 + 1; index2 < trackChangeSprmIndex; ++index2)
        {
          if (this.m_arrModifiers[index1].UniqueID > this.m_arrModifiers[index2].UniqueID)
          {
            SinglePropertyModifierRecord arrModifier = this.m_arrModifiers[index1];
            this.m_arrModifiers[index1] = this.m_arrModifiers[index2];
            this.m_arrModifiers[index2] = arrModifier;
          }
        }
      }
      for (int index3 = trackChangeSprmIndex + 1; index3 < this.m_arrModifiers.Count; ++index3)
      {
        for (int index4 = index3 + 1; index4 < this.m_arrModifiers.Count; ++index4)
        {
          if (this.m_arrModifiers[index3].UniqueID > this.m_arrModifiers[index4].UniqueID)
          {
            SinglePropertyModifierRecord arrModifier = this.m_arrModifiers[index3];
            this.m_arrModifiers[index3] = this.m_arrModifiers[index4];
            this.m_arrModifiers[index4] = arrModifier;
          }
        }
      }
    }
    else
    {
      for (int index5 = 0; index5 < this.m_arrModifiers.Count; ++index5)
      {
        for (int index6 = index5 + 1; index6 < this.m_arrModifiers.Count; ++index6)
        {
          if (this.m_arrModifiers[index5].UniqueID > this.m_arrModifiers[index6].UniqueID)
          {
            SinglePropertyModifierRecord arrModifier = this.m_arrModifiers[index5];
            this.m_arrModifiers[index5] = this.m_arrModifiers[index6];
            this.m_arrModifiers[index6] = arrModifier;
          }
        }
      }
    }
  }

  private bool IsContainTrackChangesSprm(out int trackChangeSprmIndex)
  {
    trackChangeSprmIndex = 0;
    for (int index = 0; index < this.m_arrModifiers.Count; ++index)
    {
      if (this.m_arrModifiers[index].Options == (ushort) 10883 || this.m_arrModifiers[index].Options == (ushort) 9828 || this.m_arrModifiers[index].Options == (ushort) 13928 || this.m_arrModifiers[index].Options == (ushort) 12857)
      {
        trackChangeSprmIndex = index;
        return true;
      }
    }
    return false;
  }

  internal void InsertAt(SinglePropertyModifierRecord modifier, int index)
  {
    this.m_arrModifiers.Insert(index, modifier);
  }

  internal void InsertRangeAt(SinglePropertyModifierArray modifiers, int index)
  {
    this.m_arrModifiers.InsertRange(index, (IEnumerable<SinglePropertyModifierRecord>) modifiers.Modifiers);
  }

  internal bool GetBoolean(int options, bool defValue)
  {
    SinglePropertyModifierRecord sprm = this.TryGetSprm(options);
    return sprm != null ? sprm.BoolValue : defValue;
  }

  internal byte GetByte(int options, byte defValue)
  {
    SinglePropertyModifierRecord sprm = this.TryGetSprm(options);
    return sprm != null ? sprm.ByteValue : defValue;
  }

  internal bool HasSprm(int options) => this[options] != null;

  internal ushort GetUShort(int options, ushort defValue)
  {
    SinglePropertyModifierRecord sprm = this.TryGetSprm(options);
    return sprm != null ? sprm.UshortValue : defValue;
  }

  internal short GetShort(int options, short defValue)
  {
    SinglePropertyModifierRecord sprm = this.TryGetSprm(options);
    return sprm != null ? sprm.ShortValue : defValue;
  }

  internal int GetInt(int icoe, int defVal)
  {
    SinglePropertyModifierRecord sprm = this.TryGetSprm(icoe);
    return sprm != null ? sprm.IntValue : defVal;
  }

  internal uint GetUInt(int icoe, uint defVal)
  {
    SinglePropertyModifierRecord sprm = this.TryGetSprm(icoe);
    return sprm != null ? sprm.UIntValue : defVal;
  }

  internal byte[] GetByteArray(int options) => this.TryGetSprm(options)?.ByteArray;

  internal void SetBoolValue(int options, bool flag) => this.GetSPRM(options).BoolValue = flag;

  internal void SetByteValue(int options, byte value) => this.GetSPRM(options).ByteValue = value;

  internal void SetUShortValue(int options, ushort value)
  {
    this.GetSPRM(options).UshortValue = value;
  }

  internal void SetShortValue(int options, short value) => this.GetSPRM(options).ShortValue = value;

  internal void SetIntValue(int options, int value) => this.GetSPRM(options).IntValue = value;

  internal void SetUIntValue(int options, uint value) => this.GetSPRM(options).UIntValue = value;

  internal void SetByteArrayValue(int options, byte[] value)
  {
    this.GetSPRM(options).ByteArray = value;
  }

  internal SinglePropertyModifierArray Clone()
  {
    SinglePropertyModifierArray propertyModifierArray = new SinglePropertyModifierArray();
    int sprmIndex = 0;
    for (int count = this.m_arrModifiers.Count; sprmIndex < count; ++sprmIndex)
    {
      SinglePropertyModifierRecord modifier = this.GetSprmByIndex(sprmIndex).Clone();
      if (modifier != null)
        propertyModifierArray.Add(modifier);
    }
    return propertyModifierArray;
  }

  internal new void Close()
  {
    if (this.m_arrModifiers == null)
      return;
    this.m_arrModifiers.Clear();
    this.m_arrModifiers = (List<SinglePropertyModifierRecord>) null;
  }

  internal List<SinglePropertyModifierRecord> Modifiers => this.m_arrModifiers;

  internal SinglePropertyModifierRecord this[int option]
  {
    get
    {
      int index = 0;
      for (int count = this.m_arrModifiers.Count; index < count; ++index)
      {
        if (this.m_arrModifiers[index].TypedOptions == option)
          return this.m_arrModifiers[index];
      }
      return (SinglePropertyModifierRecord) null;
    }
  }

  public int Count => this.m_arrModifiers.Count;

  internal override int Length
  {
    get
    {
      int length = 0;
      int sprmIndex = 0;
      for (int count = this.Count; sprmIndex < count; ++sprmIndex)
        length += this.GetSprmByIndex(sprmIndex).Length;
      return length;
    }
  }

  public bool IsSynchronized => false;

  public void CopyTo(Array array, int index)
  {
  }

  public object SyncRoot => (object) null;

  public IEnumerator GetEnumerator()
  {
    return (IEnumerator) new SinglePropertyModifierArray.SPRMEnumerator(this);
  }

  private SinglePropertyModifierRecord GetSPRM(int options)
  {
    SinglePropertyModifierRecord modifier = this[options];
    if (modifier == null)
    {
      modifier = new SinglePropertyModifierRecord(options);
      this.Add(modifier);
    }
    return modifier;
  }

  internal SinglePropertyModifierRecord GetSprmByIndex(int sprmIndex)
  {
    if (sprmIndex < 0 || sprmIndex >= this.m_arrModifiers.Count)
      throw new ArgumentOutOfRangeException("index", "Value can not be less than 0 and greater than Length");
    return this.m_arrModifiers[sprmIndex];
  }

  internal bool Contain(int option)
  {
    foreach (SinglePropertyModifierRecord arrModifier in this.m_arrModifiers)
    {
      if ((int) arrModifier.Options == option)
        return true;
    }
    return false;
  }

  internal SinglePropertyModifierRecord TryGetSprm(int options)
  {
    SinglePropertyModifierRecord sprm = this[options];
    if (this.HasSprm(21039) || this.HasSprm(53827))
    {
      for (int index = this.m_arrModifiers.Count - 1; index >= 0; --index)
      {
        if ((int) this.m_arrModifiers[index].Options == options)
          return this.m_arrModifiers[index];
      }
    }
    return sprm;
  }

  internal SinglePropertyModifierRecord GetNewSprm(int option, int wallSprmOption)
  {
    SinglePropertyModifierRecord newSprm = this[option];
    int newPropsStartIndex = this.GetNewPropsStartIndex(wallSprmOption);
    if (newPropsStartIndex == -1)
      return newSprm;
    int sprmIndex = newPropsStartIndex;
    for (int count = this.m_arrModifiers.Count; sprmIndex < count; ++sprmIndex)
    {
      SinglePropertyModifierRecord sprmByIndex = this.GetSprmByIndex(sprmIndex);
      if (sprmByIndex.OptionType == (WordSprmOptionType) option)
        return sprmByIndex;
    }
    return (SinglePropertyModifierRecord) null;
  }

  private int GetNewPropsStartIndex(int wallSprmOption)
  {
    SinglePropertyModifierRecord propertyModifierRecord = this[wallSprmOption];
    return propertyModifierRecord != null ? this.m_arrModifiers.IndexOf(propertyModifierRecord) + 1 : -1;
  }

  internal SinglePropertyModifierRecord GetOldSprm(int option, int wallSprmOption)
  {
    SinglePropertyModifierRecord oldSprm = this[option];
    int newPropsStartIndex = this.GetNewPropsStartIndex(wallSprmOption);
    if (newPropsStartIndex == -1)
      return oldSprm;
    for (int sprmIndex = 0; sprmIndex < newPropsStartIndex; ++sprmIndex)
    {
      SinglePropertyModifierRecord sprmByIndex = this.GetSprmByIndex(sprmIndex);
      if (sprmByIndex.OptionType == (WordSprmOptionType) option)
        return sprmByIndex;
    }
    return (SinglePropertyModifierRecord) null;
  }

  private class SPRMEnumerator : IEnumerator
  {
    private int m_iIndex = -1;
    private SinglePropertyModifierArray m_parent;

    internal SPRMEnumerator(SinglePropertyModifierArray parent)
    {
      this.m_parent = parent != null ? parent : throw new ArgumentNullException(nameof (parent));
    }

    public void Reset() => this.m_iIndex = -1;

    public object Current
    {
      get
      {
        return this.m_iIndex < 0 || this.m_iIndex >= this.m_parent.Count ? (object) null : (object) this.m_parent.GetSprmByIndex(this.m_iIndex);
      }
    }

    public bool MoveNext()
    {
      ++this.m_iIndex;
      return this.m_iIndex >= 0 && this.m_iIndex < this.m_parent.Count;
    }
  }
}
