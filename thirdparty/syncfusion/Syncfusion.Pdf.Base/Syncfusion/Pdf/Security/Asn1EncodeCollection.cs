// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.Asn1EncodeCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class Asn1EncodeCollection : IEnumerable
{
  private IList m_encodableObjects = (IList) new ArrayList();

  internal Asn1Encode this[int index] => (Asn1Encode) this.m_encodableObjects[index];

  internal int Count => this.m_encodableObjects.Count;

  internal Asn1EncodeCollection(params Asn1Encode[] vector) => this.Add(vector);

  internal static Asn1EncodeCollection FromEnumerable(IEnumerable e)
  {
    Asn1EncodeCollection encodeCollection = new Asn1EncodeCollection(new Asn1Encode[0]);
    foreach (Asn1Encode asn1Encode in e)
      encodeCollection.Add(asn1Encode);
    return encodeCollection;
  }

  internal void Add(params Asn1Encode[] objs)
  {
    foreach (object obj in objs)
      this.m_encodableObjects.Add(obj);
  }

  public IEnumerator GetEnumerator() => this.m_encodableObjects.GetEnumerator();
}
