// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.X509Name
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;
using System.Collections;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class X509Name : Asn1Encode
{
  internal static readonly DerObjectID C = new DerObjectID("2.5.4.6");
  internal static readonly DerObjectID O = new DerObjectID("2.5.4.10");
  internal static readonly DerObjectID OU = new DerObjectID("2.5.4.11");
  internal static readonly DerObjectID T = new DerObjectID("2.5.4.12");
  internal static readonly DerObjectID CN = new DerObjectID("2.5.4.3");
  internal static readonly DerObjectID Street = new DerObjectID("2.5.4.9");
  internal static readonly DerObjectID SerialNumber = new DerObjectID("2.5.4.5");
  internal static readonly DerObjectID L = new DerObjectID("2.5.4.7");
  internal static readonly DerObjectID ST = new DerObjectID("2.5.4.8");
  internal static readonly DerObjectID Surname = new DerObjectID("2.5.4.4");
  internal static readonly DerObjectID GivenName = new DerObjectID("2.5.4.42");
  internal static readonly DerObjectID Initials = new DerObjectID("2.5.4.43");
  internal static readonly DerObjectID Generation = new DerObjectID("2.5.4.44");
  internal static readonly DerObjectID UniqueIdentifier = new DerObjectID("2.5.4.45");
  internal static readonly DerObjectID BusinessCategory = new DerObjectID("2.5.4.15");
  internal static readonly DerObjectID PostalCode = new DerObjectID("2.5.4.17");
  internal static readonly DerObjectID DnQualifier = new DerObjectID("2.5.4.46");
  internal static readonly DerObjectID Pseudonym = new DerObjectID("2.5.4.65");
  internal static readonly DerObjectID DateOfBirth = new DerObjectID("1.3.6.1.5.5.7.9.1");
  internal static readonly DerObjectID PlaceOfBirth = new DerObjectID("1.3.6.1.5.5.7.9.2");
  internal static readonly DerObjectID Gender = new DerObjectID("1.3.6.1.5.5.7.9.3");
  internal static readonly DerObjectID CountryOfCitizenship = new DerObjectID("1.3.6.1.5.5.7.9.4");
  internal static readonly DerObjectID CountryOfResidence = new DerObjectID("1.3.6.1.5.5.7.9.5");
  internal static readonly DerObjectID NameAtBirth = new DerObjectID("1.3.36.8.3.14");
  internal static readonly DerObjectID PostalAddress = new DerObjectID("2.5.4.16");
  internal static readonly DerObjectID DmdName = new DerObjectID("2.5.4.54");
  internal static readonly DerObjectID TelephoneNumber = X509Objects.TelephoneNumberID;
  internal static readonly DerObjectID Name = X509Objects.NameID;
  internal static readonly DerObjectID EmailAddress = PKCSOIDs.Pkcs9AtEmailAddress;
  internal static readonly DerObjectID UnstructuredName = PKCSOIDs.Pkcs9AtUnstructuredName;
  internal static readonly DerObjectID UnstructuredAddress = PKCSOIDs.Pkcs9AtUnstructuredAddress;
  internal static readonly DerObjectID E = X509Name.EmailAddress;
  internal static readonly DerObjectID DC = new DerObjectID("0.9.2342.19200300.100.1.25");
  internal static readonly DerObjectID UID = new DerObjectID("0.9.2342.19200300.100.1.1");
  private static readonly bool[] defaultReverse = new bool[1];
  internal static readonly Hashtable DefaultSymbols = new Hashtable();
  internal static readonly Hashtable RFC2253Symbols = new Hashtable();
  internal static readonly Hashtable RFC1779Symbols = new Hashtable();
  internal static readonly Hashtable DefaultLookup = new Hashtable();
  private readonly IList m_ordering = (IList) new ArrayList();
  private IList m_values = (IList) new ArrayList();
  private IList m_added = (IList) new ArrayList();
  private Asn1Sequence m_sequence;

  internal static bool DefaultReverse
  {
    get => X509Name.defaultReverse[0];
    set => X509Name.defaultReverse[0] = value;
  }

  static X509Name()
  {
    X509Name.DefaultSymbols.Add((object) X509Name.C, (object) nameof (C));
    X509Name.DefaultSymbols.Add((object) X509Name.O, (object) nameof (O));
    X509Name.DefaultSymbols.Add((object) X509Name.T, (object) nameof (T));
    X509Name.DefaultSymbols.Add((object) X509Name.OU, (object) nameof (OU));
    X509Name.DefaultSymbols.Add((object) X509Name.CN, (object) nameof (CN));
    X509Name.DefaultSymbols.Add((object) X509Name.L, (object) nameof (L));
    X509Name.DefaultSymbols.Add((object) X509Name.ST, (object) nameof (ST));
    X509Name.DefaultSymbols.Add((object) X509Name.SerialNumber, (object) "SERIALNUMBER");
    X509Name.DefaultSymbols.Add((object) X509Name.EmailAddress, (object) nameof (E));
    X509Name.DefaultSymbols.Add((object) X509Name.DC, (object) nameof (DC));
    X509Name.DefaultSymbols.Add((object) X509Name.UID, (object) nameof (UID));
    X509Name.DefaultSymbols.Add((object) X509Name.Street, (object) "STREET");
    X509Name.DefaultSymbols.Add((object) X509Name.Surname, (object) "SURNAME");
    X509Name.DefaultSymbols.Add((object) X509Name.GivenName, (object) "GIVENNAME");
    X509Name.DefaultSymbols.Add((object) X509Name.Initials, (object) "INITIALS");
    X509Name.DefaultSymbols.Add((object) X509Name.Generation, (object) "GENERATION");
    X509Name.DefaultSymbols.Add((object) X509Name.UnstructuredAddress, (object) "unstructuredAddress");
    X509Name.DefaultSymbols.Add((object) X509Name.UnstructuredName, (object) "unstructuredName");
    X509Name.DefaultSymbols.Add((object) X509Name.UniqueIdentifier, (object) nameof (UniqueIdentifier));
    X509Name.DefaultSymbols.Add((object) X509Name.DnQualifier, (object) "DN");
    X509Name.DefaultSymbols.Add((object) X509Name.Pseudonym, (object) nameof (Pseudonym));
    X509Name.DefaultSymbols.Add((object) X509Name.PostalAddress, (object) nameof (PostalAddress));
    X509Name.DefaultSymbols.Add((object) X509Name.NameAtBirth, (object) nameof (NameAtBirth));
    X509Name.DefaultSymbols.Add((object) X509Name.CountryOfCitizenship, (object) nameof (CountryOfCitizenship));
    X509Name.DefaultSymbols.Add((object) X509Name.CountryOfResidence, (object) nameof (CountryOfResidence));
    X509Name.DefaultSymbols.Add((object) X509Name.Gender, (object) nameof (Gender));
    X509Name.DefaultSymbols.Add((object) X509Name.PlaceOfBirth, (object) nameof (PlaceOfBirth));
    X509Name.DefaultSymbols.Add((object) X509Name.DateOfBirth, (object) nameof (DateOfBirth));
    X509Name.DefaultSymbols.Add((object) X509Name.PostalCode, (object) nameof (PostalCode));
    X509Name.DefaultSymbols.Add((object) X509Name.BusinessCategory, (object) nameof (BusinessCategory));
    X509Name.DefaultSymbols.Add((object) X509Name.TelephoneNumber, (object) nameof (TelephoneNumber));
    X509Name.RFC2253Symbols.Add((object) X509Name.C, (object) nameof (C));
    X509Name.RFC2253Symbols.Add((object) X509Name.O, (object) nameof (O));
    X509Name.RFC2253Symbols.Add((object) X509Name.OU, (object) nameof (OU));
    X509Name.RFC2253Symbols.Add((object) X509Name.CN, (object) nameof (CN));
    X509Name.RFC2253Symbols.Add((object) X509Name.L, (object) nameof (L));
    X509Name.RFC2253Symbols.Add((object) X509Name.ST, (object) nameof (ST));
    X509Name.RFC2253Symbols.Add((object) X509Name.Street, (object) "STREET");
    X509Name.RFC2253Symbols.Add((object) X509Name.DC, (object) nameof (DC));
    X509Name.RFC2253Symbols.Add((object) X509Name.UID, (object) nameof (UID));
    X509Name.RFC1779Symbols.Add((object) X509Name.C, (object) nameof (C));
    X509Name.RFC1779Symbols.Add((object) X509Name.O, (object) nameof (O));
    X509Name.RFC1779Symbols.Add((object) X509Name.OU, (object) nameof (OU));
    X509Name.RFC1779Symbols.Add((object) X509Name.CN, (object) nameof (CN));
    X509Name.RFC1779Symbols.Add((object) X509Name.L, (object) nameof (L));
    X509Name.RFC1779Symbols.Add((object) X509Name.ST, (object) nameof (ST));
    X509Name.RFC1779Symbols.Add((object) X509Name.Street, (object) "STREET");
    X509Name.DefaultLookup.Add((object) "c", (object) X509Name.C);
    X509Name.DefaultLookup.Add((object) "o", (object) X509Name.O);
    X509Name.DefaultLookup.Add((object) "t", (object) X509Name.T);
    X509Name.DefaultLookup.Add((object) "ou", (object) X509Name.OU);
    X509Name.DefaultLookup.Add((object) "cn", (object) X509Name.CN);
    X509Name.DefaultLookup.Add((object) "l", (object) X509Name.L);
    X509Name.DefaultLookup.Add((object) "st", (object) X509Name.ST);
    X509Name.DefaultLookup.Add((object) "serialnumber", (object) X509Name.SerialNumber);
    X509Name.DefaultLookup.Add((object) "street", (object) X509Name.Street);
    X509Name.DefaultLookup.Add((object) "emailaddress", (object) X509Name.E);
    X509Name.DefaultLookup.Add((object) "dc", (object) X509Name.DC);
    X509Name.DefaultLookup.Add((object) "e", (object) X509Name.E);
    X509Name.DefaultLookup.Add((object) "uid", (object) X509Name.UID);
    X509Name.DefaultLookup.Add((object) "surname", (object) X509Name.Surname);
    X509Name.DefaultLookup.Add((object) "givenname", (object) X509Name.GivenName);
    X509Name.DefaultLookup.Add((object) "initials", (object) X509Name.Initials);
    X509Name.DefaultLookup.Add((object) "generation", (object) X509Name.Generation);
    X509Name.DefaultLookup.Add((object) "unstructuredaddress", (object) X509Name.UnstructuredAddress);
    X509Name.DefaultLookup.Add((object) "unstructuredname", (object) X509Name.UnstructuredName);
    X509Name.DefaultLookup.Add((object) "uniqueidentifier", (object) X509Name.UniqueIdentifier);
    X509Name.DefaultLookup.Add((object) "dn", (object) X509Name.DnQualifier);
    X509Name.DefaultLookup.Add((object) "pseudonym", (object) X509Name.Pseudonym);
    X509Name.DefaultLookup.Add((object) "postaladdress", (object) X509Name.PostalAddress);
    X509Name.DefaultLookup.Add((object) "nameofbirth", (object) X509Name.NameAtBirth);
    X509Name.DefaultLookup.Add((object) "countryofcitizenship", (object) X509Name.CountryOfCitizenship);
    X509Name.DefaultLookup.Add((object) "countryofresidence", (object) X509Name.CountryOfResidence);
    X509Name.DefaultLookup.Add((object) "gender", (object) X509Name.Gender);
    X509Name.DefaultLookup.Add((object) "placeofbirth", (object) X509Name.PlaceOfBirth);
    X509Name.DefaultLookup.Add((object) "dateofbirth", (object) X509Name.DateOfBirth);
    X509Name.DefaultLookup.Add((object) "postalcode", (object) X509Name.PostalCode);
    X509Name.DefaultLookup.Add((object) "businesscategory", (object) X509Name.BusinessCategory);
    X509Name.DefaultLookup.Add((object) "telephonenumber", (object) X509Name.TelephoneNumber);
  }

  internal static X509Name GetName(Asn1Tag tag, bool isExplicit)
  {
    return X509Name.GetName((object) Asn1Sequence.GetSequence(tag, isExplicit));
  }

  internal static X509Name GetName(object obj)
  {
    switch (obj)
    {
      case null:
      case X509Name _:
        return (X509Name) obj;
      case null:
        throw new ArgumentException("Invalid entry");
      default:
        return new X509Name(Asn1Sequence.GetSequence(obj));
    }
  }

  protected X509Name(Asn1Sequence sequence)
  {
    this.m_sequence = sequence;
    foreach (Asn1Encode asn1Encode in sequence)
    {
      Asn1Set asn1Set = Asn1Set.GetAsn1Set((object) asn1Encode.GetAsn1());
      for (int index = 0; index < asn1Set.Count; ++index)
      {
        Asn1Sequence sequence1 = Asn1Sequence.GetSequence((object) asn1Set[index].GetAsn1());
        if (sequence1.Count != 2)
          throw new ArgumentException("Invalid length in sequence");
        this.m_ordering.Add((object) DerObjectID.GetID((object) sequence1[0].GetAsn1()));
        Asn1 asn1 = sequence1[1].GetAsn1();
        if (asn1 is IAsn1String)
        {
          string str = ((IAsn1String) asn1).GetString();
          if (str.StartsWith("#"))
            str = "\\" + str;
          this.m_values.Add((object) str);
        }
        this.m_added.Add((object) (index != 0));
      }
    }
  }

  public override Asn1 GetAsn1()
  {
    if (this.m_sequence == null)
    {
      Asn1EncodeCollection collection1 = new Asn1EncodeCollection(new Asn1Encode[0]);
      Asn1EncodeCollection collection2 = new Asn1EncodeCollection(new Asn1Encode[0]);
      DerObjectID derObjectId1 = (DerObjectID) null;
      for (int index = 0; index != this.m_ordering.Count; ++index)
      {
        DerObjectID derObjectId2 = (DerObjectID) this.m_ordering[index];
        string str = (string) this.m_values[index];
        if (derObjectId1 != null && !(bool) this.m_added[index])
        {
          collection1.Add((Asn1Encode) new DerSet(collection2));
          collection2 = new Asn1EncodeCollection(new Asn1Encode[0]);
        }
        derObjectId1 = derObjectId2;
      }
      collection1.Add((Asn1Encode) new DerSet(collection2));
      this.m_sequence = (Asn1Sequence) new DerSequence(collection1);
    }
    return (Asn1) this.m_sequence;
  }

  private void AppendValue(
    StringBuilder builder,
    IDictionary symbols,
    DerObjectID id,
    string value)
  {
    string symbol = (string) symbols[(object) id];
    if (symbol != null)
      builder.Append(symbol);
    else
      builder.Append(id.ID);
    builder.Append('=');
    int length1 = builder.Length;
    builder.Append(value);
    int length2 = builder.Length;
    if (value.StartsWith("\\#"))
      length1 += 2;
    for (; length1 != length2; ++length1)
    {
      if (builder[length1] == ',' || builder[length1] == '"' || builder[length1] == '\\' || builder[length1] == '+' || builder[length1] == '=' || builder[length1] == '<' || builder[length1] == '>' || builder[length1] == ';')
      {
        builder.Insert(length1++, "\\");
        ++length2;
      }
    }
  }

  internal string ToString(bool isReverse, IDictionary symbols)
  {
    ArrayList arrayList = new ArrayList();
    StringBuilder builder = (StringBuilder) null;
    for (int index = 0; index < this.m_ordering.Count; ++index)
    {
      if ((bool) this.m_added[index])
      {
        builder.Append('+');
        this.AppendValue(builder, symbols, (DerObjectID) this.m_ordering[index], (string) this.m_values[index]);
      }
      else
      {
        builder = new StringBuilder();
        this.AppendValue(builder, symbols, (DerObjectID) this.m_ordering[index], (string) this.m_values[index]);
        arrayList.Add((object) builder);
      }
    }
    if (isReverse)
      arrayList.Reverse();
    StringBuilder stringBuilder = new StringBuilder();
    if (arrayList.Count > 0)
    {
      stringBuilder.Append(arrayList[0].ToString());
      for (int index = 1; index < arrayList.Count; ++index)
      {
        stringBuilder.Append(',');
        stringBuilder.Append(arrayList[index].ToString());
      }
    }
    return stringBuilder.ToString();
  }

  public override string ToString()
  {
    return this.ToString(X509Name.DefaultReverse, (IDictionary) X509Name.DefaultSymbols);
  }

  internal bool Equivalent(X509Name other)
  {
    if (other == null)
      return false;
    if (other == this)
      return true;
    int count = this.m_ordering.Count;
    if (count != other.m_ordering.Count)
      return false;
    bool[] flagArray = new bool[count];
    int num1;
    int num2;
    int num3;
    if (this.m_ordering[0].Equals(other.m_ordering[0]))
    {
      num1 = 0;
      num2 = count;
      num3 = 1;
    }
    else
    {
      num1 = count - 1;
      num2 = -1;
      num3 = -1;
    }
    for (int index1 = num1; index1 != num2; index1 += num3)
    {
      bool flag = false;
      DerObjectID derObjectId1 = (DerObjectID) this.m_ordering[index1];
      string text = (string) this.m_values[index1];
      for (int index2 = 0; index2 < count; ++index2)
      {
        if (!flagArray[index2])
        {
          DerObjectID derObjectId2 = (DerObjectID) other.m_ordering[index2];
          if (derObjectId1.Equals((object) derObjectId2))
          {
            string other1 = (string) other.m_values[index2];
            if (this.CheckStringEquivalent(text, other1))
            {
              flagArray[index2] = true;
              flag = true;
              break;
            }
          }
        }
      }
      if (!flag)
        return false;
    }
    return true;
  }

  private bool CheckStringEquivalent(string text, string other)
  {
    string sequence1 = this.RecognizeText(text);
    string sequence2 = this.RecognizeText(other);
    return sequence1.Equals(sequence2) || this.SkipSequence(sequence1).Equals(this.SkipSequence(sequence2));
  }

  private string RecognizeText(string text)
  {
    string str = text.ToLowerInvariant().Trim();
    if (str.StartsWith("#"))
    {
      Asn1 asn1 = Asn1.FromByteArray(new PdfString().HexToBytes(str.Substring(1)));
      if (asn1 is IAsn1String)
        str = ((IAsn1String) asn1).GetString().ToLowerInvariant().Trim();
    }
    return str;
  }

  private string SkipSequence(string sequence)
  {
    StringBuilder stringBuilder = new StringBuilder();
    if (sequence.Length != 0)
    {
      char ch1 = sequence[0];
      stringBuilder.Append(ch1);
      for (int index = 1; index < sequence.Length; ++index)
      {
        char ch2 = sequence[index];
        if (ch1 != ' ' || ch2 != ' ')
          stringBuilder.Append(ch2);
        ch1 = ch2;
      }
    }
    return stringBuilder.ToString();
  }
}
