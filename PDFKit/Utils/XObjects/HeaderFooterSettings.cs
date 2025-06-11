// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.XObjects.HeaderFooterSettings
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Linq;

#nullable disable
namespace PDFKit.Utils.XObjects;

public class HeaderFooterSettings
{
  public HeaderFooterSettings()
  {
    this.Font = new HeaderFooterSettings.FontModel();
    this.Color = new HeaderFooterSettings.ColorModel();
    this.Margin = new HeaderFooterSettings.MarginModel();
    this.Appearance = new HeaderFooterSettings.AppearanceModel();
    this.PageRange = new HeaderFooterSettings.PageRangeModel();
    this.Page = new HeaderFooterSettings.PageModel();
    this.Date = new HeaderFooterSettings.DateModel();
    this.Header = new HeaderFooterSettings.HeaderFooterModel()
    {
      Name = nameof (Header)
    };
    this.Footer = new HeaderFooterSettings.HeaderFooterModel()
    {
      Name = nameof (Footer)
    };
  }

  public string Version { get; set; }

  public HeaderFooterSettings.FontModel Font { get; }

  public HeaderFooterSettings.ColorModel Color { get; }

  public HeaderFooterSettings.MarginModel Margin { get; }

  public HeaderFooterSettings.AppearanceModel Appearance { get; }

  public HeaderFooterSettings.PageRangeModel PageRange { get; }

  public HeaderFooterSettings.PageModel Page { get; }

  public HeaderFooterSettings.DateModel Date { get; }

  public HeaderFooterSettings.HeaderFooterModel Header { get; }

  public HeaderFooterSettings.HeaderFooterModel Footer { get; }

  public XDocument CreateXDocument()
  {
    XDocument xdocument = new XDocument(new XDeclaration("1.0", "utf-8", (string) null), Array.Empty<object>());
    XElement content = new XElement((XName) nameof (HeaderFooterSettings));
    content.SetAttributeValue((XName) "version", (object) "8.0");
    xdocument.Add((object) content);
    content.Add((object) this.Font.ToXElement());
    content.Add((object) this.Color.ToXElement());
    content.Add((object) this.Margin.ToXElement());
    content.Add((object) this.Appearance.ToXElement());
    content.Add((object) this.PageRange.ToXElement());
    content.Add((object) this.Page.ToXElement());
    content.Add((object) this.Date.ToXElement());
    content.Add((object) this.Header.ToXElement());
    content.Add((object) this.Footer.ToXElement());
    return xdocument;
  }

  public override string ToString()
  {
    using (MemoryStream memoryStream = new MemoryStream())
    {
      using (StreamWriter streamWriter = new StreamWriter((Stream) memoryStream, Encoding.UTF8, 1024 /*0x0400*/, true))
      {
        this.CreateXDocument().Save((TextWriter) streamWriter, SaveOptions.DisableFormatting);
        streamWriter.Flush();
      }
      memoryStream.Seek(0L, SeekOrigin.Begin);
      byte[] numArray = ArrayPool<byte>.Shared.Rent((int) memoryStream.Length);
      memoryStream.Read(numArray, 0, (int) memoryStream.Length);
      string str = Encoding.UTF8.GetString(numArray, 0, (int) memoryStream.Length);
      ArrayPool<byte>.Shared.Return(numArray);
      return str;
    }
  }

  private static string ToAttributeName(string propName)
  {
    return string.IsNullOrEmpty(propName) ? string.Empty : propName.ToLowerInvariant();
  }

  public class FontModel : HeaderFooterSettings.IXElement
  {
    public FontModel()
    {
      this.Name = "Arial";
      this.Size = 9.0;
    }

    public string Name { get; set; }

    public double Size { get; set; }

    public XElement ToXElement()
    {
      XElement xelement = new XElement((XName) "Font");
      xelement.SetAttributeValue((XName) HeaderFooterSettings.ToAttributeName("Name"), (object) this.Name);
      xelement.SetAttributeValue((XName) HeaderFooterSettings.ToAttributeName("Size"), (object) this.Size.ToString("0.0", (IFormatProvider) CultureInfo.InvariantCulture));
      return xelement;
    }
  }

  public class ColorModel : HeaderFooterSettings.IXElement
  {
    public ColorModel()
    {
      this.R = 0.0;
      this.G = 0.0;
      this.B = 0.0;
    }

    public double R { get; set; }

    public double G { get; set; }

    public double B { get; set; }

    public XElement ToXElement()
    {
      XElement xelement = new XElement((XName) "Color");
      xelement.SetAttributeValue((XName) HeaderFooterSettings.ToAttributeName("B"), (object) this.B.ToString("0.000000", (IFormatProvider) CultureInfo.InvariantCulture));
      xelement.SetAttributeValue((XName) HeaderFooterSettings.ToAttributeName("R"), (object) this.R.ToString("0.000000", (IFormatProvider) CultureInfo.InvariantCulture));
      xelement.SetAttributeValue((XName) HeaderFooterSettings.ToAttributeName("G"), (object) this.G.ToString("0.000000", (IFormatProvider) CultureInfo.InvariantCulture));
      return xelement;
    }
  }

  public class MarginModel : HeaderFooterSettings.IXElement
  {
    public MarginModel()
    {
      this.Top = 36.0;
      this.Left = 72.0;
      this.Right = 72.0;
      this.Bottom = 36.0;
    }

    public double Top { get; set; }

    public double Left { get; set; }

    public double Right { get; set; }

    public double Bottom { get; set; }

    public XElement ToXElement()
    {
      XElement xelement = new XElement((XName) "Margin");
      xelement.SetAttributeValue((XName) HeaderFooterSettings.ToAttributeName("Top"), (object) this.Top.ToString("0.0", (IFormatProvider) CultureInfo.InvariantCulture));
      xelement.SetAttributeValue((XName) HeaderFooterSettings.ToAttributeName("Left"), (object) this.Left.ToString("0.0", (IFormatProvider) CultureInfo.InvariantCulture));
      xelement.SetAttributeValue((XName) HeaderFooterSettings.ToAttributeName("Right"), (object) this.Right.ToString("0.0", (IFormatProvider) CultureInfo.InvariantCulture));
      xelement.SetAttributeValue((XName) HeaderFooterSettings.ToAttributeName("Bottom"), (object) this.Bottom.ToString("0.0", (IFormatProvider) CultureInfo.InvariantCulture));
      return xelement;
    }
  }

  public class AppearanceModel : HeaderFooterSettings.IXElement
  {
    public AppearanceModel()
    {
      this.Shrink = false;
      this.FixedPrint = false;
    }

    public bool Shrink { get; set; }

    public bool FixedPrint { get; set; }

    public XElement ToXElement()
    {
      XElement xelement = new XElement((XName) "Appearance");
      xelement.SetAttributeValue((XName) HeaderFooterSettings.ToAttributeName("Shrink"), this.Shrink ? (object) "1" : (object) "0");
      xelement.SetAttributeValue((XName) HeaderFooterSettings.ToAttributeName("FixedPrint"), this.FixedPrint ? (object) "1" : (object) "0");
      return xelement;
    }
  }

  public class PageRangeModel : HeaderFooterSettings.IXElement
  {
    public PageRangeModel()
    {
      this.Start = -1;
      this.End = -1;
      this.Even = true;
      this.Odd = true;
    }

    public int End { get; set; }

    public int Start { get; set; }

    public bool Even { get; set; }

    public bool Odd { get; set; }

    public XElement ToXElement()
    {
      XElement xelement = new XElement((XName) "PageRange");
      xelement.SetAttributeValue((XName) HeaderFooterSettings.ToAttributeName("End"), (object) $"{this.End}");
      xelement.SetAttributeValue((XName) HeaderFooterSettings.ToAttributeName("Start"), (object) $"{this.Start}");
      xelement.SetAttributeValue((XName) HeaderFooterSettings.ToAttributeName("Even"), this.Even ? (object) "1" : (object) "0");
      xelement.SetAttributeValue((XName) HeaderFooterSettings.ToAttributeName("Odd"), this.Odd ? (object) "1" : (object) "0");
      return xelement;
    }
  }

  public class PageModel : HeaderFooterSettings.VariableCollection, HeaderFooterSettings.IXElement
  {
    public PageModel()
      : base("Page")
    {
      this.Offset = 0;
    }

    public int Offset { get; set; }

    protected override bool IsValidItem(object item)
    {
      switch (item)
      {
        case HeaderFooterSettings.VariableModel variableModel:
          return variableModel.Name == "PageIndex" || variableModel.Name == "PageTotalNum";
        case string _:
          return true;
        default:
          return false;
      }
    }

    protected override void ToXElementCore(XElement ele)
    {
      base.ToXElementCore(ele);
      ele.SetAttributeValue((XName) "offset", (object) this.Offset);
    }
  }

  public class DateModel : HeaderFooterSettings.VariableCollection
  {
    public DateModel()
      : base("Date")
    {
    }

    protected override bool IsValidItem(object item)
    {
      switch (item)
      {
        case HeaderFooterSettings.VariableModel variableModel:
          if (variableModel.Name != "Month" && variableModel.Name != "Day" && variableModel.Name != "Year")
            return false;
          break;
        case string str:
          if (string.IsNullOrEmpty(str))
            return false;
          break;
        default:
          return false;
      }
      return true;
    }
  }

  public class HeaderFooterModel : HeaderFooterSettings.IXElement
  {
    public HeaderFooterModel()
    {
      this.Left = new HeaderFooterSettings.LocationModel(nameof (Left));
      this.Center = new HeaderFooterSettings.LocationModel(nameof (Center));
      this.Right = new HeaderFooterSettings.LocationModel(nameof (Right));
    }

    public string Name { get; set; }

    public HeaderFooterSettings.LocationModel Left { get; set; }

    public HeaderFooterSettings.LocationModel Center { get; set; }

    public HeaderFooterSettings.LocationModel Right { get; set; }

    public XElement ToXElement()
    {
      XElement xelement = new XElement((XName) this.Name);
      xelement.Add((object) this.Left.ToXElement());
      xelement.Add((object) this.Center.ToXElement());
      xelement.Add((object) this.Right.ToXElement());
      return xelement;
    }
  }

  public class LocationModel(string name) : HeaderFooterSettings.VariableCollection(name)
  {
    protected override bool IsValidItem(object item)
    {
      int num;
      switch (item)
      {
        case string str:
          if (string.IsNullOrEmpty(str))
            return false;
          goto label_7;
        case HeaderFooterSettings.PageModel _:
          num = 0;
          break;
        default:
          num = !(item is HeaderFooterSettings.DateModel) ? 1 : 0;
          break;
      }
      if (num != 0)
        return false;
label_7:
      return true;
    }
  }

  public class VariableModel : HeaderFooterSettings.IXElement
  {
    public VariableModel(string name) => this.Name = name;

    public string Name { get; }

    public string Format { get; set; }

    public XElement ToXElement()
    {
      XElement xelement = new XElement((XName) this.Name);
      xelement.SetAttributeValue((XName) HeaderFooterSettings.ToAttributeName("Format"), (object) this.Format);
      return xelement;
    }
  }

  public class VariableCollection : 
    IList<object>,
    ICollection<object>,
    IEnumerable<object>,
    IEnumerable,
    HeaderFooterSettings.IXElement
  {
    private List<object> list;

    public VariableCollection(string name)
    {
      this.list = new List<object>();
      this.Name = name;
    }

    public string Name { get; }

    public object this[int index]
    {
      get => this.list[index];
      set
      {
        if (!this.IsValidItem(value))
          return;
        this.list[index] = value;
      }
    }

    public int Count => this.list.Count;

    public bool IsReadOnly => ((ICollection<object>) this.list).IsReadOnly;

    public void Add(object item)
    {
      if (!this.IsValidItem(item))
        return;
      this.list.Add(item);
    }

    public void Clear() => this.list.Clear();

    public bool Contains(object item) => this.list.Contains(item);

    public void CopyTo(object[] array, int arrayIndex) => this.list.CopyTo(array, arrayIndex);

    public IEnumerator<object> GetEnumerator() => ((IEnumerable<object>) this.list).GetEnumerator();

    public int IndexOf(object item) => this.list.IndexOf(item);

    public void Insert(int index, object item)
    {
      if (!this.IsValidItem(item))
        return;
      this.list.Insert(index, item);
    }

    public bool Remove(object item) => this.list.Remove(item);

    public void RemoveAt(int index) => this.list.RemoveAt(index);

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) this.list).GetEnumerator();

    protected virtual bool IsValidItem(object item) => true;

    public XElement ToXElement()
    {
      XElement ele = new XElement((XName) this.Name);
      this.ToXElementCore(ele);
      foreach (object obj in this)
      {
        if (obj is HeaderFooterSettings.IXElement xelement)
          ele.Add((object) xelement.ToXElement());
        else if (obj is string str)
          ele.Add((object) new XText(str));
      }
      return ele;
    }

    protected virtual void ToXElementCore(XElement ele)
    {
    }
  }

  public interface IXElement
  {
    XElement ToXElement();
  }
}
