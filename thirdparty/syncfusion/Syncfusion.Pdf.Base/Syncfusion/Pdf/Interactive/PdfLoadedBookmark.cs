// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfLoadedBookmark
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Primitives;
using System.Drawing;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfLoadedBookmark : PdfBookmark
{
  private PdfNamedDestination m_namedDestination;
  private Regex regex = new Regex("[\u0080-ÿ]");
  private char[] pdfEncodingByteToChar = new char[256 /*0x0100*/]
  {
    char.MinValue,
    '\u0001',
    '\u0002',
    '\u0003',
    '\u0004',
    '\u0005',
    '\u0006',
    '\a',
    '\b',
    '\t',
    '\n',
    '\v',
    '\f',
    '\r',
    '\u000E',
    '\u000F',
    '\u0010',
    '\u0011',
    '\u0012',
    '\u0013',
    '\u0014',
    '\u0015',
    '\u0016',
    '\u0017',
    '\u0018',
    '\u0019',
    '\u001A',
    '\u001B',
    '\u001C',
    '\u001D',
    '\u001E',
    '\u001F',
    ' ',
    '!',
    '"',
    '#',
    '$',
    '%',
    '&',
    '\'',
    '(',
    ')',
    '*',
    '+',
    ',',
    '-',
    '.',
    '/',
    '0',
    '1',
    '2',
    '3',
    '4',
    '5',
    '6',
    '7',
    '8',
    '9',
    ':',
    ';',
    '<',
    '=',
    '>',
    '?',
    '@',
    'A',
    'B',
    'C',
    'D',
    'E',
    'F',
    'G',
    'H',
    'I',
    'J',
    'K',
    'L',
    'M',
    'N',
    'O',
    'P',
    'Q',
    'R',
    'S',
    'T',
    'U',
    'V',
    'W',
    'X',
    'Y',
    'Z',
    '[',
    '\\',
    ']',
    '^',
    '_',
    '`',
    'a',
    'b',
    'c',
    'd',
    'e',
    'f',
    'g',
    'h',
    'i',
    'j',
    'k',
    'l',
    'm',
    'n',
    'o',
    'p',
    'q',
    'r',
    's',
    't',
    'u',
    'v',
    'w',
    'x',
    'y',
    'z',
    '{',
    '|',
    '}',
    '~',
    '\u007F',
    '•',
    '†',
    '‡',
    '…',
    '—',
    '–',
    'ƒ',
    '⁄',
    '‹',
    '›',
    '−',
    '‰',
    '„',
    '“',
    '”',
    '‘',
    '’',
    '‚',
    '™',
    'ﬁ',
    'ﬂ',
    'Ł',
    'Œ',
    'Š',
    'Ÿ',
    'Ž',
    'ı',
    'ł',
    'œ',
    'š',
    'ž',
    '�',
    '€',
    '¡',
    '¢',
    '£',
    '¤',
    '¥',
    '¦',
    '§',
    '¨',
    '©',
    'ª',
    '«',
    '¬',
    '\u00AD',
    '®',
    '¯',
    '°',
    '±',
    '\u00B2',
    '\u00B3',
    '´',
    'µ',
    '¶',
    '·',
    '¸',
    '\u00B9',
    'º',
    '»',
    '\u00BC',
    '\u00BD',
    '\u00BE',
    '¿',
    'À',
    'Á',
    'Â',
    'Ã',
    'Ä',
    'Å',
    'Æ',
    'Ç',
    'È',
    'É',
    'Ê',
    'Ë',
    'Ì',
    'Í',
    'Î',
    'Ï',
    'Ð',
    'Ñ',
    'Ò',
    'Ó',
    'Ô',
    'Õ',
    'Ö',
    '×',
    'Ø',
    'Ù',
    'Ú',
    'Û',
    'Ü',
    'Ý',
    'Þ',
    'ß',
    'à',
    'á',
    'â',
    'ã',
    'ä',
    'å',
    'æ',
    'ç',
    'è',
    'é',
    'ê',
    'ë',
    'ì',
    'í',
    'î',
    'ï',
    'ð',
    'ñ',
    'ò',
    'ó',
    'ô',
    'õ',
    'ö',
    '÷',
    'ø',
    'ù',
    'ú',
    'û',
    'ü',
    'ý',
    'þ',
    'ÿ'
  };

  internal PdfLoadedBookmark(PdfDictionary dictionary, PdfCrossTable crossTable)
    : base(dictionary, crossTable)
  {
  }

  public override PdfDestination Destination
  {
    get
    {
      PdfDestination destination = (PdfDestination) null;
      if (this.ObtainNamedDestination() == null)
        destination = this.ObtainDestination();
      return destination;
    }
    set => base.Destination = value;
  }

  public override PdfNamedDestination NamedDestination
  {
    get
    {
      if (this.m_namedDestination == null)
        this.m_namedDestination = (PdfNamedDestination) this.ObtainNamedDestination();
      return this.m_namedDestination;
    }
    set => base.NamedDestination = value;
  }

  private PdfLoadedNamedDestination ObtainNamedDestination()
  {
    PdfLoadedDocument document = this.CrossTable.Document as PdfLoadedDocument;
    PdfNamedDestinationCollection destinationCollection = (PdfNamedDestinationCollection) null;
    if (document != null)
      destinationCollection = document.NamedDestinationCollection;
    PdfLoadedNamedDestination namedDestination1 = (PdfLoadedNamedDestination) null;
    IPdfPrimitive pdfPrimitive = (IPdfPrimitive) null;
    if (destinationCollection != null)
    {
      if (this.Dictionary.ContainsKey("A"))
      {
        if (PdfCrossTable.Dereference(this.Dictionary["A"]) is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey("D"))
          pdfPrimitive = (IPdfPrimitive) (PdfCrossTable.Dereference(pdfDictionary["D"]) as PdfString);
      }
      else if (this.Dictionary.ContainsKey("Dest"))
        pdfPrimitive = this.CrossTable.GetObject(this.Dictionary["Dest"]);
      if (pdfPrimitive != null)
      {
        PdfName pdfName = pdfPrimitive as PdfName;
        PdfString pdfString = pdfPrimitive as PdfString;
        string str = (string) null;
        if (pdfName != (PdfName) null)
          str = pdfName.Value;
        else if (pdfString != null)
          str = pdfString.Value;
        if (str != null)
        {
          for (int index = 0; index < destinationCollection.Count; ++index)
          {
            if (destinationCollection[index] is PdfLoadedNamedDestination namedDestination2 && namedDestination2.Title.Equals(str))
            {
              namedDestination1 = namedDestination2;
              break;
            }
          }
        }
      }
    }
    return namedDestination1;
  }

  public override string Title
  {
    get => this.ObtainTitle();
    set => base.Title = value;
  }

  public override PdfColor Color
  {
    get => this.ObtainColor();
    set => this.AssignColor(value);
  }

  public override PdfTextStyle TextStyle
  {
    get => this.ObtainTextStyle();
    set => this.AssignTextStyle(value);
  }

  internal override PdfBookmark Next
  {
    get => this.ObtainNext();
    set => base.Next = value;
  }

  internal override PdfBookmark Previous
  {
    get => this.ObtainPrevious();
    set => base.Previous = value;
  }

  internal override PdfBookmarkBase Parent => base.Parent;

  internal override System.Collections.Generic.List<PdfBookmarkBase> List
  {
    get
    {
      System.Collections.Generic.List<PdfBookmarkBase> list = base.List;
      if (list.Count == 0)
        this.ReproduceTree();
      return list;
    }
  }

  private string ObtainTitle()
  {
    string empty = string.Empty;
    if (this.Dictionary.ContainsKey("Title"))
    {
      if (PdfCrossTable.Dereference(this.Dictionary["Title"]) is PdfString pdfString)
        empty = pdfString.Value;
      if (this.regex.IsMatch(empty))
        empty = this.ConvertUnicodeToString(empty);
    }
    return empty;
  }

  private string ConvertUnicodeToString(string text)
  {
    for (int index = 0; index < text.Length; ++index)
    {
      if (this.regex.IsMatch(text[index].ToString()))
      {
        char newChar = this.pdfEncodingByteToChar[(int) (byte) text[index] & (int) byte.MaxValue];
        text = text.Replace(text[index], newChar);
      }
    }
    return text;
  }

  private PdfColor ObtainColor()
  {
    PdfColor color = new PdfColor((byte) 0, (byte) 0, (byte) 0);
    if (this.Dictionary.ContainsKey("C") && PdfCrossTable.Dereference(this.Dictionary["C"]) is PdfArray pdfArray && pdfArray.Count > 2)
    {
      float red = 0.0f;
      float green = 0.0f;
      float blue = 0.0f;
      if (PdfCrossTable.Dereference(pdfArray[0]) is PdfNumber pdfNumber1)
        red = pdfNumber1.FloatValue;
      if (PdfCrossTable.Dereference(pdfArray[1]) is PdfNumber pdfNumber2)
        green = pdfNumber2.FloatValue;
      if (PdfCrossTable.Dereference(pdfArray[2]) is PdfNumber pdfNumber3)
        blue = pdfNumber3.FloatValue;
      color = new PdfColor(red, green, blue);
    }
    return color;
  }

  private PdfTextStyle ObtainTextStyle()
  {
    PdfTextStyle textStyle = PdfTextStyle.Regular;
    if (this.Dictionary.ContainsKey("F"))
    {
      PdfNumber pdfNumber = PdfCrossTable.Dereference(this.Dictionary["F"]) as PdfNumber;
      int num = 0;
      if (pdfNumber != null)
        num = pdfNumber.IntValue;
      textStyle |= (PdfTextStyle) num;
    }
    return textStyle;
  }

  private PdfBookmark ObtainNext()
  {
    PdfBookmark next = (PdfBookmark) null;
    int index = this.Parent.List.IndexOf((PdfBookmarkBase) this) + 1;
    if (index < this.Parent.List.Count)
      next = this.Parent.List[index] as PdfBookmark;
    else if (this.Dictionary.ContainsKey("Next"))
    {
      PdfDictionary dictionary = this.CrossTable.GetObject(this.Dictionary["Next"]) as PdfDictionary;
      PdfReferenceHolder pdfReferenceHolder = this.Dictionary["Next"] as PdfReferenceHolder;
      if (pdfReferenceHolder != (PdfReferenceHolder) null && pdfReferenceHolder.Reference != (PdfReference) null)
      {
        if (this.Parent.m_bookmarkReference.Contains(pdfReferenceHolder.Reference.ObjNum))
        {
          this.Parent.m_bookmarkReference.Clear();
          return next;
        }
        this.Parent.m_bookmarkReference.Add(pdfReferenceHolder.Reference.ObjNum);
      }
      next = (PdfBookmark) new PdfLoadedBookmark(dictionary, this.CrossTable);
    }
    return next;
  }

  private PdfBookmark ObtainPrevious()
  {
    PdfBookmark previous = (PdfBookmark) null;
    int index = this.List.IndexOf((PdfBookmarkBase) this) - 1;
    if (index >= 0)
      previous = this.List[index] as PdfBookmark;
    else if (this.Dictionary.ContainsKey("Prev"))
      previous = (PdfBookmark) new PdfLoadedBookmark(this.CrossTable.GetObject(this.Dictionary["Prev"]) as PdfDictionary, this.CrossTable);
    return previous;
  }

  private void AssignColor(PdfColor color)
  {
    this.Dictionary.SetProperty("C", (IPdfPrimitive) new PdfArray(new float[3]
    {
      color.Red,
      color.Green,
      color.Blue
    }));
  }

  private void AssignTextStyle(PdfTextStyle value)
  {
    this.Dictionary.SetNumber("F", (int) (this.ObtainTextStyle() | value));
  }

  private PdfDestination ObtainDestination()
  {
    if (this.Dictionary.ContainsKey("Dest") && base.Destination == null)
    {
      IPdfPrimitive pdfPrimitive = this.CrossTable.GetObject(this.Dictionary["Dest"]);
      PdfArray pdfArray = pdfPrimitive as PdfArray;
      PdfName name1 = pdfPrimitive as PdfName;
      PdfString name2 = pdfPrimitive as PdfString;
      if (this.CrossTable.Document is PdfLoadedDocument document)
      {
        if (name1 != (PdfName) null)
          pdfArray = document.GetNamedDestination(name1);
        else if (name2 != null)
          pdfArray = document.GetNamedDestination(name2);
      }
      if (pdfArray != null)
      {
        PdfReferenceHolder pointer = pdfArray[0] as PdfReferenceHolder;
        PdfPageBase page = (PdfPageBase) null;
        if (pointer == (PdfReferenceHolder) null && pdfArray[0] is PdfNumber)
        {
          PdfNumber pdfNumber1 = pdfArray[0] as PdfNumber;
          if (pdfNumber1.IntValue >= 0)
          {
            if (document != null)
              page = document.Pages[pdfNumber1.IntValue];
            PdfName pdfName = (PdfName) null;
            if (pdfArray.Count > 1)
              pdfName = pdfArray[1] as PdfName;
            if (pdfName != (PdfName) null)
            {
              if (pdfName.Value == "XYZ")
              {
                PdfNumber left = (PdfNumber) null;
                PdfNumber top = (PdfNumber) null;
                if (pdfArray.Count > 2)
                  left = pdfArray[2] as PdfNumber;
                if (pdfArray.Count > 3)
                  top = pdfArray[3] as PdfNumber;
                PdfNumber pdfNumber2 = (PdfNumber) null;
                if (pdfArray.Count > 4)
                  pdfNumber2 = pdfArray[4] as PdfNumber;
                if (page != null)
                {
                  float y = top == null ? 0.0f : page.Size.Height - top.FloatValue;
                  float x = left == null ? 0.0f : left.FloatValue;
                  if (page is PdfLoadedPage && page.Rotation != PdfPageRotateAngle.RotateAngle0)
                    y = this.CheckRotation(page, top, left);
                  base.Destination = new PdfDestination(page, new PointF(x, y));
                  base.Destination.PageIndex = document.Pages.IndexOf(page);
                  if (pdfNumber2 != null)
                    base.Destination.Zoom = pdfNumber2.FloatValue;
                  if (left == null || top == null || pdfNumber2 == null)
                    base.Destination.SetValidation(false);
                }
              }
            }
            else if (page != null)
            {
              base.Destination = new PdfDestination(page);
              base.Destination.PageIndex = document.Pages.IndexOf(page);
              base.Destination.Mode = PdfDestinationMode.FitToPage;
            }
          }
        }
        if (pointer != (PdfReferenceHolder) null)
        {
          PdfDictionary dic = this.CrossTable.GetObject((IPdfPrimitive) pointer) as PdfDictionary;
          if (document != null && dic != null)
            page = document.Pages.GetPage(dic);
          PdfName pdfName = (PdfName) null;
          if (pdfArray.Count > 1)
            pdfName = pdfArray[1] as PdfName;
          if (pdfName != (PdfName) null)
          {
            if (pdfName.Value == "XYZ")
            {
              PdfNumber left = (PdfNumber) null;
              PdfNumber top = (PdfNumber) null;
              if (pdfArray.Count > 2)
                left = pdfArray[2] as PdfNumber;
              if (pdfArray.Count > 3)
                top = pdfArray[3] as PdfNumber;
              PdfNumber pdfNumber = (PdfNumber) null;
              if (pdfArray.Count > 4)
                pdfNumber = pdfArray[4] as PdfNumber;
              if (page != null)
              {
                float y = top == null ? 0.0f : page.Size.Height - top.FloatValue;
                float x = left == null ? 0.0f : left.FloatValue;
                if (page is PdfLoadedPage && page.Rotation != PdfPageRotateAngle.RotateAngle0)
                  y = this.CheckRotation(page, top, left);
                base.Destination = new PdfDestination(page, new PointF(x, y));
                base.Destination.PageIndex = document.Pages.IndexOf(page);
                if (pdfNumber != null)
                  base.Destination.Zoom = pdfNumber.FloatValue;
                if (left == null || top == null || pdfNumber == null)
                  base.Destination.SetValidation(false);
              }
            }
            else if (pdfName.Value == "FitR")
            {
              PdfNumber pdfNumber3 = (PdfNumber) null;
              PdfNumber pdfNumber4 = (PdfNumber) null;
              PdfNumber pdfNumber5 = (PdfNumber) null;
              PdfNumber pdfNumber6 = (PdfNumber) null;
              if (pdfArray.Count > 2)
                pdfNumber3 = pdfArray[2] as PdfNumber;
              if (pdfArray.Count > 3)
                pdfNumber4 = pdfArray[3] as PdfNumber;
              if (pdfArray.Count > 4)
                pdfNumber5 = pdfArray[4] as PdfNumber;
              if (pdfArray.Count > 5)
                pdfNumber6 = pdfArray[5] as PdfNumber;
              if (page != null)
              {
                PdfNumber pdfNumber7 = pdfNumber3 == null ? new PdfNumber(0) : pdfNumber3;
                PdfNumber pdfNumber8 = pdfNumber4 == null ? new PdfNumber(0) : pdfNumber4;
                PdfNumber pdfNumber9 = pdfNumber5 == null ? new PdfNumber(0) : pdfNumber5;
                PdfNumber pdfNumber10 = pdfNumber6 == null ? new PdfNumber(0) : pdfNumber6;
                base.Destination = new PdfDestination(page, new RectangleF(pdfNumber7.FloatValue, pdfNumber8.FloatValue, pdfNumber9.FloatValue, pdfNumber10.FloatValue));
                base.Destination.PageIndex = document.Pages.IndexOf(page);
                base.Destination.Mode = PdfDestinationMode.FitR;
              }
            }
            else if (pdfName.Value == "FitBH" || pdfName.Value == "FitH")
            {
              PdfNumber pdfNumber = (PdfNumber) null;
              if (pdfArray.Count >= 3)
                pdfNumber = pdfArray[2] as PdfNumber;
              if (page != null)
              {
                float y = pdfNumber == null ? 0.0f : page.Size.Height - pdfNumber.FloatValue;
                base.Destination = new PdfDestination(page, new PointF(0.0f, y));
                base.Destination.PageIndex = document.Pages.IndexOf(page);
                base.Destination.Mode = PdfDestinationMode.FitH;
                if (pdfNumber == null)
                  base.Destination.SetValidation(false);
              }
            }
            else if (page != null && pdfName.Value == "Fit")
            {
              base.Destination = new PdfDestination(page);
              base.Destination.PageIndex = document.Pages.IndexOf(page);
              base.Destination.Mode = PdfDestinationMode.FitToPage;
            }
          }
          else if (page != null)
          {
            base.Destination = new PdfDestination(page);
            base.Destination.PageIndex = document.Pages.IndexOf(page);
            base.Destination.Mode = PdfDestinationMode.FitToPage;
          }
        }
      }
    }
    else if (this.Dictionary.ContainsKey("A") && base.Destination == null)
    {
      IPdfPrimitive pdfPrimitive = this.CrossTable.GetObject(this.Dictionary["A"]);
      if (pdfPrimitive is PdfDictionary pdfDictionary)
        pdfPrimitive = pdfDictionary["D"];
      if ((object) (pdfPrimitive as PdfReferenceHolder) != null)
        pdfPrimitive = (pdfPrimitive as PdfReferenceHolder).Object;
      PdfArray pdfArray = pdfPrimitive as PdfArray;
      PdfName name3 = pdfPrimitive as PdfName;
      PdfString name4 = pdfPrimitive as PdfString;
      if (this.CrossTable.Document is PdfLoadedDocument document)
      {
        if (name3 != (PdfName) null)
          pdfArray = document.GetNamedDestination(name3);
        else if (name4 != null)
          pdfArray = document.GetNamedDestination(name4);
      }
      if (pdfArray != null)
      {
        PdfReferenceHolder pointer = pdfArray[0] as PdfReferenceHolder;
        PdfPageBase page = (PdfPageBase) null;
        if (pointer != (PdfReferenceHolder) null && this.CrossTable.GetObject((IPdfPrimitive) pointer) is PdfDictionary dic && document != null)
          page = document.Pages.GetPage(dic);
        PdfName pdfName = (PdfName) null;
        if (pdfArray.Count > 1)
          pdfName = pdfArray[1] as PdfName;
        if (pdfName != (PdfName) null)
        {
          if (pdfName.Value == "FitBH" || pdfName.Value == "FitH")
          {
            PdfNumber pdfNumber = (PdfNumber) null;
            if (pdfArray.Count >= 3)
              pdfNumber = pdfArray[2] as PdfNumber;
            if (page != null)
            {
              float y = pdfNumber == null ? 0.0f : page.Size.Height - pdfNumber.FloatValue;
              base.Destination = new PdfDestination(page, new PointF(0.0f, y));
              base.Destination.PageIndex = document.Pages.IndexOf(page);
              base.Destination.Mode = PdfDestinationMode.FitH;
              if (pdfNumber == null)
                base.Destination.SetValidation(false);
            }
          }
          else if (pdfName.Value == "XYZ")
          {
            PdfNumber pdfNumber11 = (PdfNumber) null;
            PdfNumber pdfNumber12 = (PdfNumber) null;
            if (pdfArray.Count > 2)
              pdfNumber11 = pdfArray[2] as PdfNumber;
            if (pdfArray.Count > 3)
              pdfNumber12 = pdfArray[3] as PdfNumber;
            PdfNumber pdfNumber13 = (PdfNumber) null;
            if (pdfArray.Count > 4)
              pdfNumber13 = pdfArray[4] as PdfNumber;
            if (page != null)
            {
              float y = pdfNumber12 == null ? 0.0f : page.Size.Height - pdfNumber12.FloatValue;
              float x = pdfNumber11 == null ? 0.0f : pdfNumber11.FloatValue;
              base.Destination = new PdfDestination(page, new PointF(x, y));
              base.Destination.PageIndex = document.Pages.IndexOf(page);
              if (pdfNumber13 != null)
                base.Destination.Zoom = pdfNumber13.FloatValue;
              if (pdfNumber11 == null || pdfNumber12 == null || pdfNumber13 == null)
                base.Destination.SetValidation(false);
            }
          }
          else if (page != null && pdfName.Value == "Fit")
          {
            base.Destination = new PdfDestination(page);
            base.Destination.PageIndex = document.Pages.IndexOf(page);
            base.Destination.Mode = PdfDestinationMode.FitToPage;
          }
        }
        else if (page != null)
        {
          base.Destination = new PdfDestination(page);
          base.Destination.PageIndex = document.Pages.IndexOf(page);
          base.Destination.Mode = PdfDestinationMode.FitToPage;
        }
      }
    }
    return base.Destination;
  }

  private float CheckRotation(PdfPageBase page, PdfNumber top, PdfNumber left)
  {
    float num = 0.0f;
    left = left == null ? new PdfNumber(0) : left;
    if (page.Rotation == PdfPageRotateAngle.RotateAngle90)
      num = top == null ? 0.0f : left.FloatValue;
    else if (page.Rotation == PdfPageRotateAngle.RotateAngle180)
      num = top == null ? 0.0f : top.FloatValue;
    else if (page.Rotation == PdfPageRotateAngle.RotateAngle270)
      num = top == null ? 0.0f : page.Size.Width - left.FloatValue;
    return num;
  }
}
