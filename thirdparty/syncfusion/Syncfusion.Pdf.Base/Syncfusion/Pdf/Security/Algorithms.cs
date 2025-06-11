// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.Algorithms
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class Algorithms : Asn1Encode
{
  private Asn1Sequence m_sequence;
  private DerObjectID m_objectID;
  private Asn1Encode m_parameters;
  private bool m_parametersDefined;

  internal Algorithms(Asn1Identifier id, Asn1 asn1)
  {
    this.m_sequence = new Asn1Sequence();
    this.m_sequence.Objects.Add((object) id);
    this.m_sequence.Objects.Add((object) asn1);
  }

  internal static Algorithms GetAlgorithms(object obj)
  {
    switch (obj)
    {
      case null:
      case Algorithms _:
        return (Algorithms) obj;
      case DerObjectID _:
        return new Algorithms((DerObjectID) obj);
      case string _:
        return new Algorithms((string) obj);
      default:
        return new Algorithms(Asn1Sequence.GetSequence(obj));
    }
  }

  internal Algorithms(DerObjectID objectID) => this.m_objectID = objectID;

  internal Algorithms(string objectID) => this.m_objectID = new DerObjectID(objectID);

  internal Algorithms(DerObjectID objectID, Asn1Encode parameters)
  {
    this.m_objectID = objectID;
    this.m_parameters = parameters;
    this.m_parametersDefined = true;
  }

  internal Algorithms(Asn1Sequence sequence)
  {
    this.m_objectID = sequence.Count >= 1 && sequence.Count <= 2 ? DerObjectID.GetID((object) sequence[0]) : throw new ArgumentException("Invalid length in sequence");
    this.m_parametersDefined = sequence.Count == 2;
    if (!this.m_parametersDefined)
      return;
    this.m_parameters = sequence[1];
  }

  public virtual DerObjectID ObjectID => this.m_objectID;

  public Asn1Encode Parameters => this.m_parameters;

  internal byte[] AsnEncode() => this.m_sequence.AsnEncode();

  public override Asn1 GetAsn1()
  {
    Asn1EncodeCollection collection = new Asn1EncodeCollection(new Asn1Encode[1]
    {
      (Asn1Encode) this.m_objectID
    });
    if (this.m_parametersDefined)
    {
      if (this.m_parameters != null)
        collection.Add(this.m_parameters);
      else
        collection.Add((Asn1Encode) DerNull.Value);
    }
    return (Asn1) new DerSequence(collection);
  }
}
