// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.FloatingItem
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal class FloatingItem
{
  private RectangleF m_textWrappingBounds = new RectangleF();
  private Entity m_FloatingEntity;
  private int m_wrapcollectionindex = -1;
  private byte m_bFlags;

  internal RectangleF TextWrappingBounds
  {
    get => this.m_textWrappingBounds;
    set => this.m_textWrappingBounds = value;
  }

  internal Entity FloatingEntity
  {
    get => this.m_FloatingEntity;
    set => this.m_FloatingEntity = value;
  }

  internal TextWrappingStyle TextWrappingStyle
  {
    get
    {
      if (this.m_FloatingEntity is WTable)
      {
        WTable floatingEntity = this.m_FloatingEntity as WTable;
        if (floatingEntity.IsFrame && (floatingEntity.Rows.Count <= 0 || floatingEntity.Rows[0].Cells.Count <= 0 || floatingEntity.Rows[0].Cells[0].Paragraphs.Count <= 0 || floatingEntity.Rows[0].Cells[0].Paragraphs[0].ParagraphFormat.WrapFrameAround == FrameWrapMode.NotBeside || floatingEntity.Rows[0].Cells[0].Paragraphs[0].ParagraphFormat.WrapFrameAround == FrameWrapMode.None))
          return TextWrappingStyle.TopAndBottom;
      }
      else if (this.m_FloatingEntity is WParagraph)
      {
        WParagraph floatingEntity = this.m_FloatingEntity as WParagraph;
        if (floatingEntity.ParagraphFormat.WrapFrameAround == FrameWrapMode.None || floatingEntity.ParagraphFormat.WrapFrameAround == FrameWrapMode.NotBeside)
          return TextWrappingStyle.TopAndBottom;
      }
      else
      {
        if (this.m_FloatingEntity is WPicture)
          return (this.m_FloatingEntity as WPicture).TextWrappingStyle;
        if (this.m_FloatingEntity is WTextBox)
          return (this.m_FloatingEntity as WTextBox).TextBoxFormat.TextWrappingStyle;
        if (this.m_FloatingEntity is Shape)
          return (this.m_FloatingEntity as Shape).WrapFormat.TextWrappingStyle;
        if (this.m_FloatingEntity is WChart)
          return (this.m_FloatingEntity as WChart).WrapFormat.TextWrappingStyle;
        if (this.m_FloatingEntity is GroupShape)
          return (this.m_FloatingEntity as GroupShape).WrapFormat.TextWrappingStyle;
      }
      return TextWrappingStyle.Square;
    }
  }

  internal TextWrappingType TextWrappingType
  {
    get
    {
      if (this.m_FloatingEntity is WPicture)
        return (this.m_FloatingEntity as WPicture).TextWrappingType;
      if (this.m_FloatingEntity is Shape)
        return (this.m_FloatingEntity as Shape).WrapFormat.TextWrappingType;
      if (this.m_FloatingEntity is WChart)
        return (this.m_FloatingEntity as WChart).WrapFormat.TextWrappingType;
      if (this.m_FloatingEntity is WTextBox)
        return (this.m_FloatingEntity as WTextBox).TextBoxFormat.TextWrappingType;
      return this.m_FloatingEntity is GroupShape ? (this.m_FloatingEntity as GroupShape).WrapFormat.TextWrappingType : TextWrappingType.Both;
    }
  }

  internal WParagraph OwnerParagraph
  {
    get
    {
      return this.m_FloatingEntity is ParagraphItem ? (this.m_FloatingEntity as ParagraphItem).GetOwnerParagraphValue() : (WParagraph) null;
    }
  }

  internal bool AllowOverlap
  {
    get
    {
      if (this.m_FloatingEntity is WTable)
      {
        WTable floatingEntity = this.m_FloatingEntity as WTable;
        return floatingEntity.TableFormat == null || !floatingEntity.TableFormat.WrapTextAround || floatingEntity.TableFormat.Positioning.AllowOverlap;
      }
      if (this.m_FloatingEntity is WTextBox && (this.m_FloatingEntity as WTextBox).TextBoxFormat.TextWrappingStyle != TextWrappingStyle.Inline)
        return (this.m_FloatingEntity as WTextBox).TextBoxFormat.AllowOverlap;
      if (this.m_FloatingEntity is WPicture && (this.m_FloatingEntity as WPicture).TextWrappingStyle != TextWrappingStyle.Inline)
        return (this.m_FloatingEntity as WPicture).AllowOverlap;
      if (this.m_FloatingEntity is Shape && (this.m_FloatingEntity as Shape).WrapFormat.TextWrappingStyle != TextWrappingStyle.Inline)
        return (this.m_FloatingEntity as Shape).WrapFormat.AllowOverlap;
      if (this.m_FloatingEntity is WChart && (this.m_FloatingEntity as WChart).WrapFormat.TextWrappingStyle != TextWrappingStyle.Inline)
        return (this.m_FloatingEntity as WChart).WrapFormat.AllowOverlap;
      if (this.m_FloatingEntity is GroupShape && (this.m_FloatingEntity as GroupShape).WrapFormat.TextWrappingStyle != TextWrappingStyle.Inline)
        return (this.m_FloatingEntity as GroupShape).WrapFormat.AllowOverlap;
      return this.m_FloatingEntity is WTextBox && (this.m_FloatingEntity as WTextBox).TextBoxFormat.TextWrappingStyle != TextWrappingStyle.Inline && (this.m_FloatingEntity as WTextBox).TextBoxFormat.AllowOverlap;
    }
  }

  internal bool LayoutInCell
  {
    get
    {
      if (this.m_FloatingEntity is WPicture)
        return (this.m_FloatingEntity as WPicture).LayoutInCell;
      if (this.m_FloatingEntity is Shape)
        return (this.m_FloatingEntity as Shape).LayoutInCell;
      if (this.m_FloatingEntity is GroupShape)
        return (this.m_FloatingEntity as GroupShape).LayoutInCell;
      if (this.m_FloatingEntity is WTextBox)
      {
        Shape shape = (this.m_FloatingEntity as WTextBox).Shape;
        return shape != null ? shape.LayoutInCell : (this.m_FloatingEntity as WTextBox).TextBoxFormat.AllowInCell;
      }
      return this.m_FloatingEntity is WChart && (this.m_FloatingEntity as WChart).LayoutInCell;
    }
  }

  internal int WrapCollectionIndex
  {
    get => this.m_wrapcollectionindex;
    set => this.m_wrapcollectionindex = value;
  }

  internal bool IsFloatingItemFit
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal bool IsDoesNotDenotesRectangle
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  internal static void SortFloatingItems(
    List<FloatingItem> floatingItems,
    SortPosition sortPosition)
  {
    for (int index1 = 0; index1 < floatingItems.Count; ++index1)
    {
      FloatingItem floatingItem1 = floatingItems[index1];
      int index2 = int.MinValue;
      for (int index3 = index1; index3 < floatingItems.Count; ++index3)
      {
        if (FloatingItem.IsNeedTobeChangeSortedItem(sortPosition, floatingItem1.TextWrappingBounds, floatingItems[index3].TextWrappingBounds))
        {
          index2 = index3;
          floatingItem1 = floatingItems[index3];
        }
      }
      if (index2 != int.MinValue)
      {
        FloatingItem floatingItem2 = floatingItems[index2];
        floatingItems.RemoveAt(index2);
        floatingItems.Insert(index1, floatingItem2);
      }
    }
  }

  internal static bool IsYPositionIntersect(
    RectangleF floatingItemBounds,
    RectangleF currentItemBounds)
  {
    return Math.Round((double) currentItemBounds.Y, 2) >= Math.Round((double) floatingItemBounds.Y, 2) && Math.Round((double) currentItemBounds.Y, 2) <= Math.Round((double) floatingItemBounds.Bottom, 2) || Math.Round((double) currentItemBounds.Bottom, 2) >= Math.Round((double) floatingItemBounds.Y, 2) && Math.Round((double) currentItemBounds.Bottom, 2) <= Math.Round((double) floatingItemBounds.Bottom, 2) || Math.Round((double) currentItemBounds.Y, 2) <= Math.Round((double) floatingItemBounds.Bottom, 2) && Math.Round((double) currentItemBounds.Y, 2) >= Math.Round((double) floatingItemBounds.Y, 2);
  }

  internal static void SortSameYPostionFloatingItems(
    List<FloatingItem> floatingItems,
    SortPosition sortPosition)
  {
    for (int index1 = 0; index1 < floatingItems.Count; ++index1)
    {
      FloatingItem floatingItem1 = floatingItems[index1];
      int index2 = int.MinValue;
      for (int index3 = index1; index3 < floatingItems.Count; ++index3)
      {
        if ((floatingItem1.TextWrappingStyle != TextWrappingStyle.Tight && floatingItem1.TextWrappingStyle != TextWrappingStyle.Through || !floatingItem1.IsDoesNotDenotesRectangle) && Math.Round((double) floatingItem1.TextWrappingBounds.Y) == Math.Round((double) floatingItems[index3].TextWrappingBounds.Y) && FloatingItem.IsNeedTobeChangeSortedItem(sortPosition, floatingItem1.TextWrappingBounds, floatingItems[index3].TextWrappingBounds))
        {
          index2 = index3;
          floatingItem1 = floatingItems[index3];
        }
      }
      if (index2 != int.MinValue)
      {
        FloatingItem floatingItem2 = floatingItems[index2];
        floatingItems.RemoveAt(index2);
        floatingItems.Insert(index1, floatingItem2);
      }
    }
  }

  internal static void SortIntersectedYPostionFloatingItems(
    List<FloatingItem> floatingItems,
    SortPosition sortPosition)
  {
    for (int index1 = 0; index1 < floatingItems.Count; ++index1)
    {
      FloatingItem floatingItem1 = floatingItems[index1];
      int index2 = int.MinValue;
      for (int index3 = index1; index3 < floatingItems.Count; ++index3)
      {
        if ((floatingItem1.TextWrappingStyle != TextWrappingStyle.Tight && floatingItem1.TextWrappingStyle != TextWrappingStyle.Through || !floatingItem1.IsDoesNotDenotesRectangle) && FloatingItem.IsYPositionIntersect(floatingItem1.TextWrappingBounds, floatingItems[index3].TextWrappingBounds) && FloatingItem.IsNeedTobeChangeSortedItem(sortPosition, floatingItem1.TextWrappingBounds, floatingItems[index3].TextWrappingBounds))
        {
          index2 = index3;
          floatingItem1 = floatingItems[index3];
        }
      }
      if (index2 != int.MinValue)
      {
        FloatingItem floatingItem2 = floatingItems[index2];
        floatingItems.RemoveAt(index2);
        floatingItems.Insert(index1, floatingItem2);
      }
    }
  }

  internal static void SortXYPostionFloatingItems(
    List<FloatingItem> floatingItems,
    RectangleF rect,
    SizeF size)
  {
    List<int> intList = new List<int>();
    for (int index = 0; index < floatingItems.Count; ++index)
    {
      if (FloatingItem.IsYPositionIntersect(floatingItems[index].TextWrappingBounds, rect, size.Height))
        intList.Add(index);
    }
    for (int index = 0; index < intList.Count - 1; ++index)
    {
      FloatingItem floatingItem1 = floatingItems[intList[index]];
      FloatingItem floatingItem2 = floatingItems[intList[index] + 1];
      if ((double) floatingItem1.TextWrappingBounds.X > (double) floatingItem2.TextWrappingBounds.X && (double) floatingItem1.TextWrappingBounds.Y < (double) floatingItem2.TextWrappingBounds.Y)
      {
        FloatingItem floatingItem3 = floatingItem1;
        floatingItems[intList[index]] = floatingItem2;
        floatingItems[intList[index] + 1] = floatingItem3;
      }
    }
  }

  internal static bool IsYPositionIntersect(
    RectangleF floatingItemBounds,
    RectangleF currentItemBounds,
    float height)
  {
    return Math.Round((double) currentItemBounds.Y, 2) > Math.Round((double) floatingItemBounds.Y, 2) && Math.Round((double) currentItemBounds.Y, 2) < Math.Round((double) floatingItemBounds.Bottom, 2) || Math.Round((double) currentItemBounds.Y + (double) height, 2) > Math.Round((double) floatingItemBounds.Y, 2) && Math.Round((double) currentItemBounds.Y + (double) height, 2) < Math.Round((double) floatingItemBounds.Bottom, 2) || Math.Round((double) currentItemBounds.Y, 2) < Math.Round((double) floatingItemBounds.Bottom, 2) && Math.Round((double) currentItemBounds.Y, 2) > Math.Round((double) floatingItemBounds.Y, 2);
  }

  private static bool IsNeedTobeChangeSortedItem(
    SortPosition SortPosition,
    RectangleF firstItem,
    RectangleF secondItem)
  {
    switch (SortPosition)
    {
      case SortPosition.X:
        return (double) firstItem.X > (double) secondItem.X;
      case SortPosition.Y:
        return (double) firstItem.Y > (double) secondItem.Y;
      case SortPosition.Bottom:
        return (double) firstItem.Bottom > (double) secondItem.Bottom;
      default:
        return false;
    }
  }

  internal static RectangleF GetIntersectingItemBounds(
    Layouter m_lcOperator,
    FloatingItem intersectedFloatingItem,
    float yPosition)
  {
    FloatingItem bottomFloatingItem = FloatingItem.GetMinBottomFloatingItem(FloatingItem.GetIntersectingFloatingItems(m_lcOperator, intersectedFloatingItem, yPosition));
    return bottomFloatingItem != null ? bottomFloatingItem.TextWrappingBounds : RectangleF.Empty;
  }

  internal static List<FloatingItem> GetIntersectingFloatingItems(
    Layouter m_lcOperator,
    FloatingItem intersectedFloatingItem,
    float yPosition)
  {
    List<FloatingItem> intersectingFloatingItems = new List<FloatingItem>();
    foreach (FloatingItem floatingItem in m_lcOperator.FloatingItems)
    {
      if ((double) yPosition <= (double) floatingItem.TextWrappingBounds.Bottom && (double) intersectedFloatingItem.TextWrappingBounds.Bottom >= (double) floatingItem.TextWrappingBounds.Bottom && (double) floatingItem.TextWrappingBounds.Right > (double) m_lcOperator.ClientLayoutArea.X && (double) floatingItem.TextWrappingBounds.X < (double) intersectedFloatingItem.TextWrappingBounds.X)
        intersectingFloatingItems.Add(floatingItem);
    }
    return intersectingFloatingItems;
  }

  internal static FloatingItem GetMinBottomFloatingItem(List<FloatingItem> fItems)
  {
    int index = -1;
    float num1 = float.MaxValue;
    int num2 = 0;
    FloatingItem.SortFloatingItems(fItems, SortPosition.X);
    foreach (FloatingItem fItem in fItems)
    {
      if ((double) num1 > (double) fItem.TextWrappingBounds.Bottom)
      {
        if (fItem.FloatingEntity is WParagraph && fItems.IndexOf(fItem) + 1 < fItems.Count && fItems[fItems.IndexOf(fItem) + 1].FloatingEntity is WParagraph && (fItem.FloatingEntity as WParagraph).ParagraphFormat.IsInSameFrame((fItems[fItems.IndexOf(fItem) + 1].FloatingEntity as WParagraph).ParagraphFormat))
        {
          ++num2;
        }
        else
        {
          num1 = fItem.TextWrappingBounds.Bottom;
          index = fItems.IndexOf(fItem);
        }
      }
    }
    return index - num2 != 0 ? (FloatingItem) null : fItems[index];
  }
}
