// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.Asn1Sequence
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class Asn1Sequence : Asn1, IEnumerable
{
  private IList m_objects;

  internal IList Objects => this.m_objects;

  internal virtual Syncfusion.Pdf.Security.Asn1Encode this[int index]
  {
    get => (Syncfusion.Pdf.Security.Asn1Encode) this.m_objects[index];
  }

  internal virtual IAsn1Collection Parser
  {
    get => (IAsn1Collection) new Asn1Sequence.Asn1SequenceHelper(this);
  }

  internal virtual int Count => this.m_objects.Count;

  public Asn1Sequence()
    : this(0)
  {
    this.m_objects = (IList) new ArrayList();
  }

  public Asn1Sequence(params Syncfusion.Pdf.Security.Asn1Encode[] asn1EncodableArray)
    : this(asn1EncodableArray.Length)
  {
    foreach (Syncfusion.Pdf.Security.Asn1Encode asn1Encodable in asn1EncodableArray)
      this.AddObject(asn1Encodable);
  }

  public Asn1Sequence(List<Asn1> sequence)
    : base(Asn1UniversalTags.Sequence | Asn1UniversalTags.Constructed)
  {
    this.m_objects = (IList) new List<object>();
    foreach (object obj in sequence)
      this.m_objects.Add(obj);
  }

  internal Asn1Sequence(Asn1EncodeCollection collection)
    : this(collection.Count)
  {
    foreach (Syncfusion.Pdf.Security.Asn1Encode asn1Encode in collection)
      this.AddObject(asn1Encode);
  }

  internal Asn1Sequence(int capacity)
    : base(Asn1UniversalTags.Sequence | Asn1UniversalTags.Constructed)
  {
    this.m_objects = (IList) new ArrayList();
  }

  internal static Asn1Sequence GetSequence(object obj)
  {
    switch (obj)
    {
      case null:
      case Asn1Sequence _:
        return (Asn1Sequence) obj;
      case IAsn1Collection _:
        return Asn1Sequence.GetSequence((object) ((IAsn1) obj).GetAsn1());
      case byte[] _:
        try
        {
          return Asn1Sequence.GetSequence((object) Asn1.FromByteArray((byte[]) obj));
        }
        catch (IOException ex)
        {
          throw new ArgumentException(ex.Message);
        }
      case Syncfusion.Pdf.Security.Asn1Encode _:
        Asn1 asn1 = ((Syncfusion.Pdf.Security.Asn1Encode) obj).GetAsn1();
        if (asn1 is Asn1Sequence)
          return (Asn1Sequence) asn1;
        break;
    }
    throw new ArgumentException("Invalid entry in sequence " + obj.GetType().FullName, nameof (obj));
  }

  internal static Asn1Sequence GetSequence(Asn1Tag obj, bool explicitly)
  {
    Asn1 asn1 = obj.GetObject();
    if (explicitly)
    {
      if (!obj.IsExplicit)
        throw new ArgumentException("Invalid entry in sequence");
      return (Asn1Sequence) asn1;
    }
    if (obj.IsExplicit)
      return obj is BerTag ? (Asn1Sequence) new BerSequence((Syncfusion.Pdf.Security.Asn1Encode) asn1) : (Asn1Sequence) new DerSequence((Syncfusion.Pdf.Security.Asn1Encode) asn1);
    return asn1 is Asn1Sequence ? (Asn1Sequence) asn1 : throw new ArgumentException("Invalid entry in sequence " + obj.GetType().FullName, nameof (obj));
  }

  public virtual IEnumerator GetEnumerator() => this.m_objects.GetEnumerator();

  public override int GetHashCode()
  {
    int count = this.Count;
    foreach (object obj in this)
    {
      count *= 17;
      if (obj == null)
        count ^= DerNull.Value.GetAsn1Hash();
      else
        count ^= obj.GetHashCode();
    }
    return count;
  }

  protected override bool IsEquals(Asn1 asn1Object)
  {
    if (!(asn1Object is Asn1Sequence asn1Sequence) || this.Count != asn1Sequence.Count)
      return false;
    IEnumerator enumerator1 = this.GetEnumerator();
    IEnumerator enumerator2 = asn1Sequence.GetEnumerator();
    while (enumerator1.MoveNext() && enumerator2.MoveNext())
    {
      if (!this.GetCurrentObject(enumerator1).GetAsn1().Equals((object) this.GetCurrentObject(enumerator2).GetAsn1()))
        return false;
    }
    return true;
  }

  private Syncfusion.Pdf.Security.Asn1Encode GetCurrentObject(IEnumerator e)
  {
    return (Syncfusion.Pdf.Security.Asn1Encode) e.Current ?? (Syncfusion.Pdf.Security.Asn1Encode) DerNull.Value;
  }

  protected internal void AddObject(Syncfusion.Pdf.Security.Asn1Encode obj)
  {
    this.m_objects.Add((object) obj);
  }

  public override string ToString() => this.ToString((IEnumerable) this.m_objects);

  internal string ToString(IEnumerable e)
  {
    StringBuilder stringBuilder = new StringBuilder("[");
    IEnumerator enumerator = e.GetEnumerator();
    if (enumerator.MoveNext())
    {
      stringBuilder.Append(enumerator.Current.ToString());
      while (enumerator.MoveNext())
      {
        stringBuilder.Append(", ");
        stringBuilder.Append(enumerator.Current.ToString());
      }
    }
    stringBuilder.Append(']');
    return stringBuilder.ToString();
  }

  private byte[] ToArray()
  {
    MemoryStream memoryStream = new MemoryStream();
    foreach (Syncfusion.Pdf.Security.Asn1Encode asn1Encode in (IEnumerable) this.m_objects)
    {
      byte[] buffer = (byte[]) null;
      switch (asn1Encode)
      {
        case Asn1Integer _:
          buffer = (asn1Encode as Asn1Integer).AsnEncode();
          break;
        case Asn1Boolean _:
          buffer = (asn1Encode as Asn1Boolean).AsnEncode();
          break;
        case Asn1Null _:
          buffer = (asn1Encode as Asn1Null).AsnEncode();
          break;
        case Asn1Identifier _:
          buffer = (asn1Encode as Asn1Identifier).Asn1Encode();
          break;
        case Asn1Octet _:
          buffer = (asn1Encode as Asn1Octet).AsnEncode();
          break;
        case Asn1Sequence _:
          buffer = (asn1Encode as Asn1Sequence).AsnEncode();
          break;
        case Algorithms _:
          buffer = (asn1Encode as Algorithms).AsnEncode();
          break;
      }
      memoryStream.Write(buffer, 0, buffer.Length);
    }
    return memoryStream.ToArray();
  }

  internal override void Encode(DerStream derOut) => throw new NotImplementedException();

  internal byte[] AsnEncode() => this.Asn1Encode(this.ToArray());

  private class Asn1SequenceHelper : IAsn1Collection, IAsn1
  {
    private readonly Asn1Sequence m_sequence;
    private readonly int m_max;
    private int m_index;

    internal Asn1SequenceHelper(Asn1Sequence sequence)
    {
      this.m_sequence = sequence;
      this.m_max = sequence.Count;
    }

    public IAsn1 ReadObject()
    {
      if (this.m_index == this.m_max)
        return (IAsn1) null;
      Syncfusion.Pdf.Security.Asn1Encode asn1Encode = this.m_sequence[this.m_index++];
      switch (asn1Encode)
      {
        case Asn1Sequence _:
          return (IAsn1) ((Asn1Sequence) asn1Encode).Parser;
        case Asn1Set _:
          return (IAsn1) ((Asn1Set) asn1Encode).Parser;
        default:
          return (IAsn1) asn1Encode;
      }
    }

    public Asn1 GetAsn1() => (Asn1) this.m_sequence;
  }
}
