// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfXfaEdge
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System;
using System.Globalization;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

public class PdfXfaEdge
{
  private float m_thikness = 1f;
  private PdfXfaBorderStyle m_borderStyle;
  private PdfColor m_borderColor = new PdfColor(System.Drawing.Color.Black);
  private PdfXfaVisibility m_visibility;
  private string m_presenceTxt;

  public PdfColor Color
  {
    get => this.m_borderColor;
    set => this.m_borderColor = value;
  }

  public PdfXfaVisibility Visibility
  {
    get => this.m_visibility;
    set => this.m_visibility = value;
  }

  public float Thickness
  {
    get => this.m_thikness;
    set => this.m_thikness = value;
  }

  public PdfXfaBorderStyle BorderStyle
  {
    get => this.m_borderStyle;
    set => this.m_borderStyle = value;
  }

  internal void Read(XmlNode node, PdfXfaEdge edge)
  {
    if (node.Attributes != null)
    {
      if (node.Attributes["stroke"] != null)
        this.ReadStroke(node.Attributes["stroke"].Value, edge);
      if (node.Attributes["thickness"] != null)
        edge.Thickness = this.ConvertToPoint(node.Attributes["thickness"].Value);
      if (node.Attributes["presence"] != null)
      {
        edge.m_presenceTxt = node.Attributes["presence"].Value;
        switch (edge.m_presenceTxt)
        {
          case "hidden":
            edge.Visibility = PdfXfaVisibility.Hidden;
            break;
          case "visible":
            edge.Visibility = PdfXfaVisibility.Visible;
            break;
          case "invisible":
            edge.Visibility = PdfXfaVisibility.Invisible;
            break;
          case "inactive":
            edge.Visibility = PdfXfaVisibility.Inactive;
            break;
          default:
            edge.Visibility = PdfXfaVisibility.Visible;
            break;
        }
      }
    }
    if (node["color"] == null || node["color"].Attributes["value"] == null)
      return;
    string[] strArray = node["color"].Attributes["value"].Value.Split(',');
    edge.Color = new PdfColor(byte.Parse(strArray[0]), byte.Parse(strArray[1]), byte.Parse(strArray[2]));
  }

  private float ConvertToPoint(string value)
  {
    float point = float.NaN;
    if (value.Contains("pt"))
      point = Convert.ToSingle(value.Trim('p', 't', 'm'), (IFormatProvider) CultureInfo.InvariantCulture);
    else if (value.Contains("m"))
      point = Convert.ToSingle(value.Trim('p', 't', 'm'), (IFormatProvider) CultureInfo.InvariantCulture) * 2.83464575f;
    else if (value.Contains("in"))
      point = Convert.ToSingle(value.Trim('i', 'n'), (IFormatProvider) CultureInfo.InvariantCulture) * 72f;
    return point;
  }

  private void ReadStroke(string value, PdfXfaEdge edge)
  {
    switch (value)
    {
      case "solid":
        edge.BorderStyle = PdfXfaBorderStyle.Solid;
        break;
      case "dashed":
        edge.BorderStyle = PdfXfaBorderStyle.Dashed;
        break;
      case "dotted":
        edge.BorderStyle = PdfXfaBorderStyle.Dotted;
        break;
      case "dashDot":
        edge.BorderStyle = PdfXfaBorderStyle.DashDot;
        break;
      case "dashDotDot":
        edge.BorderStyle = PdfXfaBorderStyle.DashDotDot;
        break;
      case "lowered":
        edge.BorderStyle = PdfXfaBorderStyle.Lowered;
        break;
      case "raised":
        edge.BorderStyle = PdfXfaBorderStyle.Raised;
        break;
      case "etched":
        edge.BorderStyle = PdfXfaBorderStyle.Etched;
        break;
      case "embossed":
        edge.BorderStyle = PdfXfaBorderStyle.Embossed;
        break;
      default:
        edge.BorderStyle = PdfXfaBorderStyle.None;
        break;
    }
  }

  internal void Save(XmlNode node)
  {
    if (this.BorderStyle != PdfXfaBorderStyle.None)
    {
      string str1 = this.BorderStyle.ToString();
      string str2 = char.ToLower(str1[0]).ToString() + str1.Substring(1);
      if (node.Attributes != null)
      {
        if (node.Attributes["stroke"] != null)
          node.Attributes["stroke"].Value = str2;
        else
          this.SetNewAttribute(node, "stroke", str2);
      }
      else
        this.SetNewAttribute(node, "stroke", str2);
    }
    if (!float.IsNaN(this.Thickness))
    {
      if (node.Attributes["thickness"] != null)
        node.Attributes["thickness"].Value = this.Thickness.ToString() + "pt";
      else
        this.SetNewAttribute(node, "thickness", this.Thickness.ToString() + "pt");
    }
    if ((this.m_presenceTxt != null || this.m_presenceTxt != "") && this.Visibility != PdfXfaVisibility.Visible && this.m_presenceTxt != this.Visibility.ToString().ToLower())
    {
      if (node.Attributes["presence"] != null)
        node.Attributes["presence"].Value = this.Visibility.ToString().ToLower();
      else
        this.SetNewAttribute(node, "presence", this.Visibility.ToString().ToLower());
    }
    PdfColor color = this.Color;
    string str = $"{this.Color.R.ToString()},{this.Color.G.ToString()},{this.Color.B.ToString()}";
    if (node["color"] != null)
    {
      if (node["color"].Attributes["value"] != null)
        node["color"].Attributes["value"].Value = str;
      else
        this.SetNewAttribute((XmlNode) node["color"], "value", str);
    }
    else
    {
      XmlNode node1 = node.OwnerDocument.CreateNode(XmlNodeType.Element, "color", "");
      this.SetNewAttribute(node1, "value", str);
      node.AppendChild(node1);
    }
  }

  private void SetNewAttribute(XmlNode node, string name, string value)
  {
    XmlAttribute attribute = node.OwnerDocument.CreateAttribute(name);
    attribute.Value = value;
    node.Attributes.Append(attribute);
  }

  internal object Clone() => this.MemberwiseClone();
}
