// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.Asn1Set
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class Asn1Set : Asn1, IEnumerable
{
  private IList m_objects;

  internal object Objects => (object) this.m_objects;

  internal Syncfusion.Pdf.Security.Asn1Encode this[int index] => (Syncfusion.Pdf.Security.Asn1Encode) this.m_objects[index];

  internal int Count => this.m_objects.Count;

  internal IAsn1SetHelper Parser => (IAsn1SetHelper) new Asn1Set.Asn1SetHelper(this);

  protected internal Asn1Set(int capacity) => this.m_objects = (IList) new ArrayList(capacity);

  internal Syncfusion.Pdf.Security.Asn1Encode[] ToArray()
  {
    Syncfusion.Pdf.Security.Asn1Encode[] array = new Syncfusion.Pdf.Security.Asn1Encode[this.Count];
    for (int index = 0; index < this.Count; ++index)
      array[index] = this[index];
    return array;
  }

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
    if (!(asn1Object is Asn1Set asn1Set) || this.Count != asn1Set.Count)
      return false;
    IEnumerator enumerator1 = this.GetEnumerator();
    IEnumerator enumerator2 = asn1Set.GetEnumerator();
    while (enumerator1.MoveNext() && enumerator2.MoveNext())
    {
      if (!this.GetCurrentSet(enumerator1).GetAsn1().Equals((object) this.GetCurrentSet(enumerator2).GetAsn1()))
        return false;
    }
    return true;
  }

  private Syncfusion.Pdf.Security.Asn1Encode GetCurrentSet(IEnumerator e)
  {
    return (Syncfusion.Pdf.Security.Asn1Encode) e.Current ?? (Syncfusion.Pdf.Security.Asn1Encode) DerNull.Value;
  }

  private bool LessThanOrEqual(byte[] a, byte[] b)
  {
    int num = Math.Min(a.Length, b.Length);
    for (int index = 0; index != num; ++index)
    {
      if ((int) a[index] != (int) b[index])
        return (int) a[index] < (int) b[index];
    }
    return num == a.Length;
  }

  protected internal void AddObject(Syncfusion.Pdf.Security.Asn1Encode obj)
  {
    this.m_objects.Add((object) obj);
  }

  public override string ToString() => this.m_objects.ToString();

  internal override void Encode(DerStream derOut) => throw new NotImplementedException();

  public static Asn1Set GetAsn1Set(object obj)
  {
    switch (obj)
    {
      case null:
      case Asn1Set _:
        return (Asn1Set) obj;
      case IAsn1SetHelper _:
        return Asn1Set.GetAsn1Set((object) ((IAsn1) obj).GetAsn1());
      case byte[] _:
        try
        {
          return Asn1Set.GetAsn1Set((object) Asn1.FromByteArray((byte[]) obj));
        }
        catch (IOException ex)
        {
          throw new ArgumentException("Invalid byte array to create Asn1Set: " + ex.Message);
        }
      case Syncfusion.Pdf.Security.Asn1Encode _:
        Asn1 asn1 = ((Syncfusion.Pdf.Security.Asn1Encode) obj).GetAsn1();
        if (asn1 is Asn1Set)
          return (Asn1Set) asn1;
        break;
    }
    throw new ArgumentException("Invalid entry in sequence " + obj.GetType().FullName, nameof (obj));
  }

  public static Asn1Set GetAsn1Set(Asn1Tag taggedObject, bool isExplicit)
  {
    Asn1 asn1Set = taggedObject.GetObject();
    if (isExplicit)
    {
      if (!taggedObject.IsExplicit)
        throw new ArgumentException("Tagged object is implicit.");
      return (Asn1Set) asn1Set;
    }
    if (taggedObject.IsExplicit)
      return (Asn1Set) new DerSet(new Syncfusion.Pdf.Security.Asn1Encode[1]
      {
        (Syncfusion.Pdf.Security.Asn1Encode) asn1Set
      });
    switch (asn1Set)
    {
      case Asn1Set _:
        return (Asn1Set) asn1Set;
      case Asn1Sequence _:
        Asn1EncodeCollection collection = new Asn1EncodeCollection(new Syncfusion.Pdf.Security.Asn1Encode[0]);
        foreach (Syncfusion.Pdf.Security.Asn1Encode asn1Encode in (Asn1Sequence) asn1Set)
          collection.Add(asn1Encode);
        return (Asn1Set) new DerSet(collection, false);
      default:
        throw new ArgumentException("Invalid entry in sequence " + taggedObject.GetType().FullName, "obj");
    }
  }

  public virtual IEnumerator GetEnumerator() => this.m_objects.GetEnumerator();

  protected internal void SortObjects()
  {
    if (this.m_objects.Count <= 1)
      return;
    bool flag = true;
    int num1 = this.m_objects.Count - 1;
    while (flag)
    {
      int index = 0;
      int num2 = 0;
      byte[] a = ((Syncfusion.Pdf.Security.Asn1Encode) this.m_objects[0]).GetEncoded();
      flag = false;
      for (; index != num1; ++index)
      {
        byte[] encoded = ((Syncfusion.Pdf.Security.Asn1Encode) this.m_objects[index + 1]).GetEncoded();
        if (this.LessThanOrEqual(a, encoded))
        {
          a = encoded;
        }
        else
        {
          object obj = this.m_objects[index];
          this.m_objects[index] = this.m_objects[index + 1];
          this.m_objects[index + 1] = obj;
          flag = true;
          num2 = index;
        }
      }
      num1 = num2;
    }
  }

  private class Asn1SetHelper : IAsn1SetHelper, IAsn1
  {
    private readonly Asn1Set m_set;
    private readonly int m_max;
    private int m_index;

    public Asn1SetHelper(Asn1Set outer)
    {
      this.m_set = outer;
      this.m_max = outer.Count;
    }

    public IAsn1 ReadObject()
    {
      if (this.m_index == this.m_max)
        return (IAsn1) null;
      Syncfusion.Pdf.Security.Asn1Encode asn1Encode = this.m_set[this.m_index++];
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

    public virtual Asn1 GetAsn1() => (Asn1) this.m_set;
  }
}
