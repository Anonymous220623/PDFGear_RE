// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.DerObjectID
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class DerObjectID : Asn1
{
  private const long m_limit = 72057594037927808;
  private string m_id;
  private byte[] m_bytes;
  private static readonly DerObjectID[] m_objects = new DerObjectID[1024 /*0x0400*/];

  public string ID => this.m_id;

  internal DerObjectID(string id)
  {
    if (id == null)
      throw new ArgumentNullException(nameof (id));
    this.m_id = DerObjectID.IsValidIdentifier(id) ? id : throw new FormatException("Invalid ID");
  }

  internal DerObjectID(DerObjectID id, string branchId)
  {
    if (!DerObjectID.IsValidBranchID(branchId, 0))
      throw new ArgumentException("Invalid branch ID");
    this.m_id = $"{id.ID}.{branchId}";
  }

  internal DerObjectID(byte[] bytes)
  {
    this.m_id = DerObjectID.GetID(bytes);
    this.m_bytes = Asn1Constants.Clone(bytes);
  }

  public virtual DerObjectID Branch(string id) => new DerObjectID(this, id);

  private void WriteField(Stream stream, long fieldValue)
  {
    byte[] buffer = new byte[9];
    int offset = 8;
    buffer[offset] = (byte) ((ulong) fieldValue & (ulong) sbyte.MaxValue);
    while (fieldValue >= 128L /*0x80*/)
    {
      fieldValue >>= 7;
      buffer[--offset] = (byte) ((ulong) (fieldValue & (long) sbyte.MaxValue) | 128UL /*0x80*/);
    }
    stream.Write(buffer, offset, 9 - offset);
  }

  private void WriteField(Stream stream, Number fieldValue)
  {
    int length = (fieldValue.BitLength + 6) / 7;
    if (length == 0)
    {
      stream.WriteByte((byte) 0);
    }
    else
    {
      Number number = fieldValue;
      byte[] buffer = new byte[length];
      for (int index = length - 1; index >= 0; --index)
      {
        buffer[index] = (byte) (number.IntValue & (int) sbyte.MaxValue | 128 /*0x80*/);
        number = number.ShiftRight(7);
      }
      buffer[length - 1] &= (byte) 127 /*0x7F*/;
      stream.Write(buffer, 0, buffer.Length);
    }
  }

  private void GetOutput(MemoryStream stream)
  {
    ObjectIdentityToken objectIdentityToken = new ObjectIdentityToken(this.m_id);
    int num = int.Parse(objectIdentityToken.NextToken()) * 40;
    string s1 = objectIdentityToken.NextToken();
    if (s1.Length <= 18)
      this.WriteField((Stream) stream, (long) num + long.Parse(s1));
    else
      this.WriteField((Stream) stream, new Number(s1).Add(Number.ValueOf((long) num)));
    while (objectIdentityToken.HasMoreTokens)
    {
      string s2 = objectIdentityToken.NextToken();
      if (s2.Length <= 18)
        this.WriteField((Stream) stream, long.Parse(s2));
      else
        this.WriteField((Stream) stream, new Number(s2));
    }
  }

  internal byte[] GetBytes()
  {
    lock (this)
    {
      if (this.m_bytes == null)
      {
        MemoryStream stream = new MemoryStream();
        this.GetOutput(stream);
        this.m_bytes = stream.ToArray();
      }
    }
    return this.m_bytes;
  }

  internal override void Encode(DerStream stream) => stream.WriteEncoded(6, this.GetBytes());

  public override int GetHashCode() => this.m_id.GetHashCode();

  protected override bool IsEquals(Asn1 asn1)
  {
    return asn1 is DerObjectID derObjectId && this.m_id.Equals(derObjectId.m_id);
  }

  public override string ToString() => this.m_id;

  internal static DerObjectID GetID(object obj)
  {
    switch (obj)
    {
      case null:
      case DerObjectID _:
        return (DerObjectID) obj;
      case byte[] _:
        return DerObjectID.FromOctetString((byte[]) obj);
      default:
        throw new ArgumentException("Illegal object");
    }
  }

  internal static DerObjectID GetID(Asn1Tag obj, bool isExplicit)
  {
    return DerObjectID.GetID((object) obj.GetObject());
  }

  private static bool IsValidBranchID(string branchID, int start)
  {
    bool flag = false;
    int length = branchID.Length;
    while (--length >= start)
    {
      char ch = branchID[length];
      if ('0' <= ch && ch <= '9')
      {
        flag = true;
      }
      else
      {
        if (ch != '.' || !flag)
          return false;
        flag = false;
      }
    }
    return flag;
  }

  private static bool IsValidIdentifier(string id)
  {
    if (id.Length < 3 || id[1] != '.')
      return false;
    char ch = id[0];
    return ch >= '0' && ch <= '2' && DerObjectID.IsValidBranchID(id, 2);
  }

  private static string GetID(byte[] bytes)
  {
    StringBuilder stringBuilder = new StringBuilder();
    long num1 = 0;
    Number number1 = (Number) null;
    bool flag = true;
    for (int index = 0; index != bytes.Length; ++index)
    {
      int num2 = (int) bytes[index];
      if (num1 <= 72057594037927808L)
      {
        long num3 = num1 + (long) (num2 & (int) sbyte.MaxValue);
        if ((num2 & 128 /*0x80*/) == 0)
        {
          if (flag)
          {
            if (num3 < 40L)
              stringBuilder.Append('0');
            else if (num3 < 80L /*0x50*/)
            {
              stringBuilder.Append('1');
              num3 -= 40L;
            }
            else
            {
              stringBuilder.Append('2');
              num3 -= 80L /*0x50*/;
            }
            flag = false;
          }
          stringBuilder.Append('.');
          stringBuilder.Append(num3);
          num1 = 0L;
        }
        else
          num1 = num3 << 7;
      }
      else
      {
        if (number1 == null)
          number1 = Number.ValueOf(num1);
        Number number2 = number1.Or(Number.ValueOf((long) (num2 & (int) sbyte.MaxValue)));
        if ((num2 & 128 /*0x80*/) == 0)
        {
          if (flag)
          {
            stringBuilder.Append('2');
            number2 = number2.Subtract(Number.ValueOf(80L /*0x50*/));
            flag = false;
          }
          stringBuilder.Append('.');
          stringBuilder.Append((object) number2);
          number1 = (Number) null;
          num1 = 0L;
        }
        else
          number1 = number2.ShiftLeft(7);
      }
    }
    return stringBuilder.ToString();
  }

  internal static DerObjectID FromOctetString(byte[] bytes)
  {
    int index = Asn1Constants.GetHashCode(bytes) & 1023 /*0x03FF*/;
    lock (DerObjectID.m_objects)
    {
      DerObjectID derObjectId = DerObjectID.m_objects[index];
      return derObjectId != null && Asn1Constants.AreEqual(bytes, derObjectId.GetBytes()) ? derObjectId : (DerObjectID.m_objects[index] = new DerObjectID(bytes));
    }
  }
}
