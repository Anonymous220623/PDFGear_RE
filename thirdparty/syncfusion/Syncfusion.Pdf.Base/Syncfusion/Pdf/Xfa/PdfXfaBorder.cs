// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfXfaBorder
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Xml;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

public class PdfXfaBorder
{
  private PdfXfaHandedness m_handedness;
  private PdfXfaVisibility m_visibility;
  private PdfXfaEdge m_left;
  private PdfXfaEdge m_right;
  private PdfXfaEdge m_bottom;
  private PdfXfaEdge m_top;
  private PdfColor m_color;
  private float m_width = 0.5f;
  private PdfXfaBorderStyle m_borderStyle;
  private string m_presenceTxt;
  private PdfXfaBrush m_fillColor;

  public PdfXfaBrush FillColor
  {
    get => this.m_fillColor;
    set
    {
      if (value == null)
        return;
      this.m_fillColor = value;
    }
  }

  public PdfXfaHandedness Handedness
  {
    get => this.m_handedness;
    set => this.m_handedness = value;
  }

  public PdfXfaVisibility Visibility
  {
    get => this.m_visibility;
    set => this.m_visibility = value;
  }

  public PdfXfaEdge LeftEdge
  {
    get => this.m_left;
    set
    {
      if (value == null)
        return;
      this.m_left = value;
    }
  }

  public PdfXfaEdge RightEdge
  {
    get => this.m_right;
    set
    {
      if (value == null)
        return;
      this.m_right = value;
    }
  }

  public PdfXfaEdge TopEdge
  {
    get => this.m_top;
    set
    {
      if (value == null)
        return;
      this.m_top = value;
    }
  }

  public PdfXfaEdge BottomEdge
  {
    get => this.m_bottom;
    set
    {
      if (value == null)
        return;
      this.m_bottom = value;
    }
  }

  public PdfColor Color
  {
    get => this.m_color;
    set => this.m_color = value;
  }

  public float Width
  {
    get => this.m_width;
    set => this.m_width = value;
  }

  public PdfXfaBorderStyle Style
  {
    get => this.m_borderStyle;
    set => this.m_borderStyle = value;
  }

  public PdfXfaBorder()
  {
  }

  public PdfXfaBorder(PdfColor color) => this.Color = color;

  internal void DrawBorder(PdfGraphics graphics, RectangleF bounds)
  {
    if (this.Visibility != PdfXfaVisibility.Hidden)
    {
      PdfPen flattenPen = this.GetFlattenPen();
      PdfBrush brush = this.GetBrush(bounds);
      graphics.DrawRectangle(flattenPen, brush, bounds);
    }
    if (this.LeftEdge == null || this.RightEdge == null)
      return;
    PdfPen pen1 = PdfPens.Black;
    if (this.LeftEdge.Visibility != PdfXfaVisibility.Hidden)
    {
      if (!this.LeftEdge.Color.IsEmpty)
        pen1 = new PdfPen(this.LeftEdge.Color);
      pen1.Width = (double) this.LeftEdge.Thickness <= 0.0 ? this.Width : this.LeftEdge.Thickness;
      this.DrawEdge(graphics, pen1, bounds.Location, new PointF(bounds.X, bounds.Y + bounds.Height));
    }
    if (this.RightEdge.Visibility != PdfXfaVisibility.Hidden)
    {
      PdfPen pen2 = PdfPens.Black;
      if (!this.RightEdge.Color.IsEmpty)
        pen2 = new PdfPen(this.RightEdge.Color);
      pen2.Width = (double) this.RightEdge.Thickness <= 0.0 ? this.Width : this.RightEdge.Thickness;
      this.DrawEdge(graphics, pen2, new PointF(bounds.X + bounds.Width, bounds.Y), new PointF(bounds.X + bounds.Width, bounds.Y + bounds.Height));
    }
    if (this.TopEdge != null && this.TopEdge.Visibility != PdfXfaVisibility.Hidden)
    {
      PdfPen pen3 = PdfPens.Black;
      if (!this.TopEdge.Color.IsEmpty)
        pen3 = new PdfPen(this.TopEdge.Color);
      pen3.Width = (double) this.TopEdge.Thickness <= 0.0 ? this.Width : this.TopEdge.Thickness;
      this.DrawEdge(graphics, pen3, new PointF(bounds.X, bounds.Y), new PointF(bounds.X + bounds.Width, bounds.Y));
    }
    if (this.BottomEdge == null || this.BottomEdge.Visibility == PdfXfaVisibility.Hidden)
      return;
    this.DrawEdge(graphics, new PdfPen(this.BottomEdge.Color)
    {
      Width = (double) this.BottomEdge.Thickness <= 0.0 ? this.Width : this.BottomEdge.Thickness
    }, new PointF(bounds.X, bounds.Y + bounds.Height), new PointF(bounds.X + bounds.Width, bounds.Y + bounds.Height));
  }

  private void DrawEdge(PdfGraphics graphics, PdfPen pen, PointF point, PointF point1)
  {
    graphics.Save();
    graphics.DrawLine(pen, point, point1);
    graphics.Restore();
  }

  internal PdfBorderStyle GetBorderStyle()
  {
    PdfBorderStyle borderStyle = PdfBorderStyle.Solid;
    switch (this.Style)
    {
      case PdfXfaBorderStyle.Dashed:
      case PdfXfaBorderStyle.Dotted:
      case PdfXfaBorderStyle.DashDot:
      case PdfXfaBorderStyle.DashDotDot:
        borderStyle = PdfBorderStyle.Dashed;
        break;
      case PdfXfaBorderStyle.Embossed:
        borderStyle = PdfBorderStyle.Beveled;
        break;
    }
    return borderStyle;
  }

  internal void ApplyAcroBorder(PdfStyledField field)
  {
    if (this.Color != PdfColor.Empty && (this.Color.R != (byte) 0 || this.Color.G != (byte) 0 || this.Color.B != (byte) 0))
      field.BorderColor = this.Color;
    switch (this.Style)
    {
      case PdfXfaBorderStyle.Solid:
        field.BorderStyle = PdfBorderStyle.Solid;
        break;
      case PdfXfaBorderStyle.Dashed:
      case PdfXfaBorderStyle.Dotted:
      case PdfXfaBorderStyle.DashDot:
      case PdfXfaBorderStyle.DashDotDot:
        field.BorderStyle = PdfBorderStyle.Dashed;
        break;
      case PdfXfaBorderStyle.Embossed:
        field.BorderStyle = PdfBorderStyle.Beveled;
        break;
    }
    if ((double) this.Width != 0.0 && (double) this.Width > 1.0)
      field.BorderWidth = this.Width;
    if (this.FillColor != null && this.FillColor is PdfXfaSolidBrush)
      field.BackColor = (this.FillColor as PdfXfaSolidBrush).Color;
    else
      field.BackColor = PdfColor.Empty;
  }

  internal PdfBrush GetBrush(RectangleF bounds)
  {
    PdfBrush brush = (PdfBrush) null;
    if (this.FillColor != null)
    {
      if (this.FillColor is PdfXfaSolidBrush)
        brush = (PdfBrush) new PdfSolidBrush((this.FillColor as PdfXfaSolidBrush).Color);
      else if (this.FillColor is PdfXfaLinearBrush)
      {
        PdfLinearGradientMode mode = PdfLinearGradientMode.Horizontal;
        PdfXfaLinearBrush fillColor = this.FillColor as PdfXfaLinearBrush;
        switch (fillColor.Type)
        {
          case PdfXfaLinearType.LeftToRight:
          case PdfXfaLinearType.RightToLeft:
            mode = PdfLinearGradientMode.Horizontal;
            break;
          case PdfXfaLinearType.BottomToTop:
          case PdfXfaLinearType.TopToBottom:
            mode = PdfLinearGradientMode.Vertical;
            break;
        }
        brush = fillColor.Type == PdfXfaLinearType.RightToLeft || fillColor.Type == PdfXfaLinearType.BottomToTop ? (PdfBrush) new PdfLinearGradientBrush(bounds, fillColor.StartColor, fillColor.EndColor, mode) : (PdfBrush) new PdfLinearGradientBrush(bounds, fillColor.EndColor, fillColor.StartColor, mode);
      }
      else if (this.FillColor is PdfXfaRadialBrush)
      {
        PdfXfaRadialBrush fillColor = this.FillColor as PdfXfaRadialBrush;
        if ((double) bounds.Height <= (double) bounds.Width)
        {
          double width = (double) bounds.Width;
          double height = (double) bounds.Height;
        }
        else
        {
          double height = (double) bounds.Height;
          double width = (double) bounds.Width;
        }
        float num = (double) bounds.Height > (double) bounds.Width ? bounds.Height : bounds.Width;
        PointF pointF = new PointF(bounds.Location.X + bounds.Width / 2f, bounds.Location.Y + bounds.Height / 2f);
        brush = fillColor.Type != PdfXfaRadialType.CenterToEdge ? (PdfBrush) new PdfRadialGradientBrush(pointF, num, pointF, 0.0f, fillColor.StartColor, fillColor.EndColor) : (PdfBrush) new PdfRadialGradientBrush(pointF, 0.0f, pointF, num, fillColor.StartColor, fillColor.EndColor);
      }
    }
    return brush;
  }

  internal PdfPen GetFlattenPen()
  {
    PdfPen flattenPen = (PdfPen) null;
    if (!this.Color.IsEmpty)
      flattenPen = new PdfPen(this.Color, this.Width);
    else if (this.FillColor != null && (double) this.Width > 0.0)
      flattenPen = new PdfPen(new PdfColor(System.Drawing.Color.Black), this.Width);
    if (this.LeftEdge != null && this.RightEdge == null && this.TopEdge == null && this.BottomEdge == null)
      flattenPen = this.LeftEdge.Color.IsEmpty || this.LeftEdge.Visibility == PdfXfaVisibility.Hidden || this.LeftEdge.Visibility == PdfXfaVisibility.Invisible ? (PdfPen) null : ((double) this.LeftEdge.Thickness <= 0.0 ? ((double) this.Width <= 0.0 ? (PdfPen) null : new PdfPen(this.LeftEdge.Color, this.Width)) : new PdfPen(this.LeftEdge.Color, this.LeftEdge.Thickness));
    return flattenPen;
  }

  internal PdfPen GetPen() => this.Color != PdfColor.Empty ? new PdfPen(this.Color) : (PdfPen) null;

  internal void Read(XmlNode node)
  {
    XmlAttributeCollection attributes = node.Attributes;
    if (attributes != null)
    {
      if (attributes["hand"] != null)
      {
        switch (attributes["hand"].Value)
        {
          case "left":
            this.m_handedness = PdfXfaHandedness.Left;
            break;
          case "right":
            this.m_handedness = PdfXfaHandedness.Right;
            break;
          case "even":
            this.m_handedness = PdfXfaHandedness.Even;
            break;
        }
      }
      if (attributes["presence"] != null)
      {
        this.m_presenceTxt = attributes["presence"].Value;
        switch (this.m_presenceTxt)
        {
          case "hidden":
            this.m_visibility = PdfXfaVisibility.Hidden;
            break;
          case "visible":
            this.m_visibility = PdfXfaVisibility.Visible;
            break;
          case "invisible":
            this.m_visibility = PdfXfaVisibility.Invisible;
            break;
          case "inactive":
            this.m_visibility = PdfXfaVisibility.Inactive;
            break;
        }
      }
    }
    XmlNodeList childNodes = node.ChildNodes;
    bool flag = false;
    if (childNodes != null)
    {
      if (childNodes.Count == 1 && childNodes[0].Name == "edge")
      {
        if (childNodes[0].Attributes.Count != 0 || childNodes[0].InnerText != string.Empty)
        {
          PdfXfaEdge edge = new PdfXfaEdge();
          edge.Thickness = 0.0f;
          edge.Read(childNodes[0], edge);
          if (edge.BorderStyle != PdfXfaBorderStyle.None)
            edge.Thickness = 1f;
          this.Color = edge.Color;
          this.Style = edge.BorderStyle;
          this.Width = edge.Thickness;
          this.Visibility = edge.Visibility;
          flag = true;
        }
      }
      else if (childNodes.Count > 1)
      {
        flag = true;
        foreach (XmlNode node1 in childNodes)
        {
          if (node1.Name == "edge")
          {
            PdfXfaEdge edge = new PdfXfaEdge();
            edge.Thickness = 0.0f;
            edge.Read(node1, edge);
            if (edge.BorderStyle != PdfXfaBorderStyle.None)
              edge.Thickness = 1f;
            if (this.LeftEdge == null)
              this.LeftEdge = edge.Clone() as PdfXfaEdge;
            else if (this.RightEdge == null)
              this.RightEdge = edge.Clone() as PdfXfaEdge;
            else if (this.TopEdge == null)
              this.TopEdge = edge.Clone() as PdfXfaEdge;
            else if (this.BottomEdge == null)
              this.BottomEdge = edge.Clone() as PdfXfaEdge;
          }
        }
      }
    }
    this.ReadFillBrush(node);
    if (flag)
      return;
    PdfXfaEdge pdfXfaEdge = new PdfXfaEdge();
    this.Color = pdfXfaEdge.Color;
    this.Style = pdfXfaEdge.BorderStyle;
    this.Width = pdfXfaEdge.Thickness;
  }

  internal void ReadFillBrush(XmlNode node)
  {
    if (node["fill"] == null)
      return;
    XmlNode xmlNode = (XmlNode) node["fill"];
    if (xmlNode["color"] != null && (xmlNode["linear"] != null || xmlNode["radial"] != null))
    {
      PdfColor pdfColor1 = PdfColor.Empty;
      PdfColor pdfColor2 = PdfColor.Empty;
      if (xmlNode["color"].Attributes["value"] != null)
      {
        string[] words = xmlNode["color"].Attributes["value"].Value.Split(',');
        if (this.ValidateColor(words))
          pdfColor1 = new PdfColor(byte.Parse(words[0]), byte.Parse(words[1]), byte.Parse(words[2]));
      }
      if (xmlNode["linear"] != null)
      {
        PdfXfaLinearType type = PdfXfaLinearType.LeftToRight;
        if (xmlNode["linear"].Attributes["type"] != null)
          type = this.GetLinearType(xmlNode["linear"].Attributes["type"].Value);
        if (xmlNode["linear"]["color"] != null)
        {
          string[] words = xmlNode["linear"]["color"].Attributes["value"].Value.Split(',');
          if (!this.ValidateColor(words))
            return;
          pdfColor2 = new PdfColor(byte.Parse(words[0], (IFormatProvider) CultureInfo.InvariantCulture), byte.Parse(words[1], (IFormatProvider) CultureInfo.InvariantCulture), byte.Parse(words[2], (IFormatProvider) CultureInfo.InvariantCulture));
          this.FillColor = (PdfXfaBrush) new PdfXfaLinearBrush(pdfColor2, pdfColor1, type);
        }
        else
          this.FillColor = (PdfXfaBrush) new PdfXfaLinearBrush(new PdfColor(System.Drawing.Color.Black), pdfColor1, type);
      }
      else
      {
        if (xmlNode["radial"] == null)
          return;
        PdfXfaRadialType type = PdfXfaRadialType.CenterToEdge;
        if (xmlNode["radial"].Attributes["type"] != null)
          type = this.GetRadialType(xmlNode["radial"].Attributes["type"].Value);
        if (xmlNode["radial"]["color"] == null)
          return;
        string[] words = xmlNode["radial"]["color"].Attributes["value"].Value.Split(',');
        if (!this.ValidateColor(words))
          return;
        pdfColor2 = new PdfColor(byte.Parse(words[0]), byte.Parse(words[1]), byte.Parse(words[2]));
        this.FillColor = (PdfXfaBrush) new PdfXfaRadialBrush(pdfColor1, pdfColor2, type);
      }
    }
    else
    {
      if (xmlNode["color"] == null || xmlNode["color"].Attributes["value"] == null)
        return;
      string[] words = xmlNode["color"].Attributes["value"].Value.Split(',');
      if (!this.ValidateColor(words))
        return;
      this.FillColor = (PdfXfaBrush) new PdfXfaSolidBrush(new PdfColor(byte.Parse(words[0]), byte.Parse(words[1]), byte.Parse(words[2])));
    }
  }

  private bool ValidateColor(string[] words)
  {
    bool flag = true;
    foreach (string word in words)
    {
      if (word.Contains("-"))
      {
        flag = false;
        break;
      }
    }
    return flag;
  }

  private PdfXfaLinearType GetLinearType(string type)
  {
    PdfXfaLinearType linearType = PdfXfaLinearType.LeftToRight;
    switch (type)
    {
      case "toBottom":
        linearType = PdfXfaLinearType.TopToBottom;
        break;
      case "toLeft":
        linearType = PdfXfaLinearType.RightToLeft;
        break;
      case "toRight":
        linearType = PdfXfaLinearType.LeftToRight;
        break;
      case "toTop":
        linearType = PdfXfaLinearType.BottomToTop;
        break;
    }
    return linearType;
  }

  private PdfXfaRadialType GetRadialType(string type)
  {
    PdfXfaRadialType radialType = PdfXfaRadialType.CenterToEdge;
    switch (type)
    {
      case "toCenter":
        radialType = PdfXfaRadialType.EdgeToCenter;
        break;
      case "toEdge":
        radialType = PdfXfaRadialType.CenterToEdge;
        break;
    }
    return radialType;
  }

  internal static PdfXfaBorder ReadBorder(XmlNode node)
  {
    PdfXfaBorder pdfXfaBorder = new PdfXfaBorder();
    XmlAttributeCollection attributes = node.Attributes;
    if (attributes != null)
    {
      if (attributes["hand"] != null)
      {
        switch (attributes["hand"].Value)
        {
          case "left":
            pdfXfaBorder.m_handedness = PdfXfaHandedness.Left;
            break;
          case "right":
            pdfXfaBorder.m_handedness = PdfXfaHandedness.Right;
            break;
          case "even":
            pdfXfaBorder.m_handedness = PdfXfaHandedness.Even;
            break;
        }
      }
      if (attributes["presence"] != null)
      {
        pdfXfaBorder.m_presenceTxt = attributes["presence"].Value;
        switch (pdfXfaBorder.m_presenceTxt)
        {
          case "hidden":
            pdfXfaBorder.m_visibility = PdfXfaVisibility.Hidden;
            break;
          case "visible":
            pdfXfaBorder.m_visibility = PdfXfaVisibility.Visible;
            break;
          case "invisible":
            pdfXfaBorder.m_visibility = PdfXfaVisibility.Invisible;
            break;
          case "inactive":
            pdfXfaBorder.m_visibility = PdfXfaVisibility.Inactive;
            break;
        }
      }
    }
    XmlNodeList childNodes = node.ChildNodes;
    if (childNodes != null)
    {
      if (childNodes.Count == 1)
      {
        PdfXfaEdge edge = new PdfXfaEdge();
        edge.Read(childNodes[0], edge);
        pdfXfaBorder.Color = edge.Color;
        pdfXfaBorder.Style = edge.BorderStyle;
        pdfXfaBorder.Width = edge.Thickness;
      }
      else
      {
        foreach (XmlNode node1 in childNodes)
        {
          if (node1.Name == "edge")
          {
            PdfXfaEdge edge = new PdfXfaEdge();
            edge.Read(node1, edge);
            if (pdfXfaBorder.LeftEdge == null)
              pdfXfaBorder.LeftEdge = edge.Clone() as PdfXfaEdge;
            else if (pdfXfaBorder.RightEdge == null)
              pdfXfaBorder.RightEdge = edge.Clone() as PdfXfaEdge;
            else if (pdfXfaBorder.TopEdge == null)
              pdfXfaBorder.TopEdge = edge.Clone() as PdfXfaEdge;
            else if (pdfXfaBorder.BottomEdge == null)
              pdfXfaBorder.BottomEdge = edge.Clone() as PdfXfaEdge;
          }
        }
      }
    }
    if (node["fill"] != null && node["fill"]["color"] != null && node["fill"]["color"].Attributes["value"] != null)
    {
      string[] strArray = node["fill"]["color"].Attributes["value"].Value.Split(',');
      pdfXfaBorder.FillColor = (PdfXfaBrush) new PdfXfaSolidBrush(new PdfColor(byte.Parse(strArray[0]), byte.Parse(strArray[1]), byte.Parse(strArray[2])));
    }
    return pdfXfaBorder;
  }

  internal void Save(XmlNode node)
  {
    XmlAttributeCollection attributes = node.Attributes;
    if (attributes["hand"] != null)
      attributes["hand"].Value = this.Handedness.ToString().ToLower();
    else
      this.SetNewAttribute(node, "hand", this.Handedness.ToString().ToLower());
    if ((this.m_presenceTxt != null || this.m_presenceTxt != "") && this.Visibility != PdfXfaVisibility.Visible && this.m_presenceTxt != this.Visibility.ToString().ToLower())
    {
      if (node.Attributes["presence"] != null)
        node.Attributes["presence"].Value = this.Visibility.ToString().ToLower();
      else
        this.SetNewAttribute(node, "presence", this.Visibility.ToString().ToLower());
    }
    if (this.LeftEdge != null || this.RightEdge != null || this.BottomEdge != null || this.TopEdge != null)
    {
      List<XmlNode> xmlNodeList = new List<XmlNode>();
      foreach (XmlNode childNode in node.ChildNodes)
      {
        if (childNode.Name == "edge")
          xmlNodeList.Add(childNode);
      }
      if (xmlNodeList.Count <= 0)
        return;
      if (this.LeftEdge != null)
        this.LeftEdge.Save(xmlNodeList[0]);
      if (this.RightEdge != null)
      {
        if (xmlNodeList.Count > 1)
          this.RightEdge.Save(xmlNodeList[1]);
        else
          this.CreateNewEdgeNode(node, this.RightEdge);
      }
      if (this.BottomEdge != null)
      {
        foreach (XmlNode childNode in node.ChildNodes)
        {
          if (childNode.Name == "edge")
            xmlNodeList.Add(node);
        }
        if (xmlNodeList.Count > 2)
          this.BottomEdge.Save(xmlNodeList[2]);
        else if (xmlNodeList.Count > 1)
        {
          this.CreateNewEdgeNode(node, this.BottomEdge);
        }
        else
        {
          if (this.RightEdge == null)
            this.CreateNewEdgeNode(node, new PdfXfaEdge());
          this.CreateNewEdgeNode(node, this.BottomEdge);
        }
      }
      if (this.TopEdge == null)
        return;
      foreach (XmlNode childNode in node.ChildNodes)
      {
        if (childNode.Name == "edge")
          xmlNodeList.Add(node);
      }
      if (xmlNodeList.Count > 3)
      {
        this.TopEdge.Save(xmlNodeList[3]);
      }
      else
      {
        if (this.RightEdge == null)
          this.CreateNewEdgeNode(node, new PdfXfaEdge());
        if (this.BottomEdge == null)
          this.CreateNewEdgeNode(node, new PdfXfaEdge());
        this.CreateNewEdgeNode(node, this.TopEdge);
      }
    }
    else
    {
      PdfXfaEdge edge = new PdfXfaEdge();
      edge.BorderStyle = this.Style;
      edge.Color = this.Color;
      edge.Visibility = this.Visibility;
      edge.Thickness = this.Width;
      if (node["edge"] != null)
        edge.Save((XmlNode) node["edge"]);
      else
        this.CreateNewEdgeNode(node, edge);
    }
  }

  private void CreateNewEdgeNode(XmlNode node, PdfXfaEdge edge)
  {
    XmlNode node1 = node.OwnerDocument.CreateNode(XmlNodeType.Element, nameof (edge), "");
    edge.Save(node1);
    node.AppendChild(node1);
  }

  private void SetNewAttribute(XmlNode node, string name, string value)
  {
    XmlAttribute attribute = node.OwnerDocument.CreateAttribute(name);
    attribute.Value = value;
    node.Attributes.Append(attribute);
  }
}
