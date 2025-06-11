// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.OutlineWrapperUtility
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class OutlineWrapperUtility
{
  private Dictionary<int, List<Point>> m_outlineLevels;
  private bool m_bIsLevelInOrder;

  public Dictionary<int, List<Point>> OutlineLevels
  {
    get => this.m_outlineLevels;
    set => this.m_outlineLevels = value;
  }

  public void AddRowLevel(
    WorksheetImpl sheet,
    int firstRow,
    int lastRow,
    int rowlevel,
    bool isParse)
  {
    this.AddRowLevel(sheet, firstRow, lastRow, rowlevel, isParse, false);
  }

  internal void AddRowLevel(
    WorksheetImpl sheet,
    int firstRow,
    int lastRow,
    int rowlevel,
    bool isParse,
    bool isImport)
  {
    Dictionary<int, List<GroupPoint>> rowOutlineLevels = sheet.RowOutlineLevels;
    List<GroupPoint> outlineLevel = new List<GroupPoint>();
    if (!rowOutlineLevels.ContainsKey(rowlevel))
    {
      outlineLevel.Add(new GroupPoint(firstRow, lastRow)
      {
        IsParse = isParse
      });
      rowOutlineLevels.CustomAdd(rowlevel, outlineLevel);
    }
    else
    {
      rowOutlineLevels.TryGetValue(rowlevel, out outlineLevel);
      GroupPoint newLevel = new GroupPoint(firstRow, lastRow);
      newLevel.IsParse = isParse;
      if (isParse)
        outlineLevel.Add(newLevel);
      else
        this.InsertGroup(outlineLevel, firstRow, newLevel, isImport);
    }
  }

  public void SubRowLevel(WorksheetImpl sheet, int firstRow, int lastRow, int rowLevel)
  {
    Dictionary<int, List<GroupPoint>> rowOutlineLevels = sheet.RowOutlineLevels;
    sheet.AddGroupsinLevel(rowOutlineLevels);
    List<GroupPoint> groupPointList = (List<GroupPoint>) null;
    rowOutlineLevels.TryGetValue(rowLevel, out groupPointList);
    if (groupPointList == null)
      return;
    for (int index = groupPointList.Count - 1; index >= 0; --index)
    {
      GroupPoint groupPoint1 = groupPointList[index];
      if (groupPoint1.X < firstRow && groupPoint1.Y > lastRow && groupPoint1.Y > firstRow)
      {
        GroupPoint groupPoint2 = new GroupPoint(groupPoint1.X, firstRow - 1);
        GroupPoint groupPoint3 = new GroupPoint(lastRow + 1, groupPoint1.Y);
        groupPointList.RemoveAt(index);
        groupPointList.Insert(index, groupPoint2);
        groupPointList.Insert(index + 1, groupPoint3);
        break;
      }
      if (groupPoint1.Y == lastRow && firstRow > groupPoint1.X)
      {
        GroupPoint groupPoint4 = new GroupPoint(groupPoint1.X, firstRow - 1);
        groupPointList.RemoveAt(index);
        groupPointList.Insert(index, groupPoint4);
        break;
      }
      if (groupPoint1.X == firstRow && groupPoint1.Y > lastRow)
      {
        GroupPoint groupPoint5 = new GroupPoint(lastRow + 1, groupPoint1.Y);
        groupPointList.RemoveAt(index);
        groupPointList.Insert(index, groupPoint5);
        break;
      }
      if (groupPoint1.X == firstRow && groupPoint1.Y == lastRow)
      {
        groupPointList.RemoveAt(index);
        if (groupPointList.Count != 0)
          break;
        rowOutlineLevels.Remove(rowLevel);
        break;
      }
    }
  }

  public void SubColumnLevel(WorksheetImpl sheet, int firstRow, int lastRow, int columnLevel)
  {
    Dictionary<int, List<GroupPoint>> columnOutlineLevels = sheet.ColumnOutlineLevels;
    sheet.AddGroupsinLevel(columnOutlineLevels);
    List<GroupPoint> groupPointList = (List<GroupPoint>) null;
    columnOutlineLevels.TryGetValue(columnLevel, out groupPointList);
    if (groupPointList == null)
      return;
    for (int index = groupPointList.Count - 1; index >= 0; --index)
    {
      GroupPoint groupPoint1 = groupPointList[index];
      if (groupPoint1.X < firstRow && groupPoint1.Y > lastRow && groupPoint1.Y > firstRow)
      {
        GroupPoint groupPoint2 = new GroupPoint(groupPoint1.X, firstRow - 1);
        GroupPoint groupPoint3 = new GroupPoint(lastRow + 1, groupPoint1.Y);
        groupPointList.RemoveAt(index);
        groupPointList.Insert(index, groupPoint2);
        groupPointList.Insert(index + 1, groupPoint3);
        break;
      }
      if (groupPoint1.Y == lastRow && firstRow > groupPoint1.X)
      {
        GroupPoint groupPoint4 = new GroupPoint(groupPoint1.X, firstRow - 1);
        groupPointList.RemoveAt(index);
        groupPointList.Insert(index, groupPoint4);
        break;
      }
      if (groupPoint1.X == firstRow && groupPoint1.Y > lastRow)
      {
        GroupPoint groupPoint5 = new GroupPoint(lastRow + 1, groupPoint1.Y);
        groupPointList.RemoveAt(index);
        groupPointList.Insert(index, groupPoint5);
        break;
      }
      if (groupPoint1.X == firstRow && groupPoint1.Y == lastRow)
      {
        groupPointList.RemoveAt(index);
        if (groupPointList.Count != 0)
          break;
        columnOutlineLevels.Remove(columnLevel);
        break;
      }
    }
  }

  public void AddColumnLevel(
    WorksheetImpl sheet,
    int columnLevel,
    int firstColumn,
    int lastColumn,
    bool isParse)
  {
    this.AddColumnLevel(sheet, columnLevel, firstColumn, lastColumn, isParse, false);
  }

  internal void AddColumnLevel(
    WorksheetImpl sheet,
    int columnLevel,
    int firstColumn,
    int lastColumn,
    bool isParse,
    bool isImport)
  {
    Dictionary<int, List<GroupPoint>> columnOutlineLevels = sheet.ColumnOutlineLevels;
    List<GroupPoint> outlineLevel = new List<GroupPoint>();
    if (!columnOutlineLevels.ContainsKey(columnLevel))
    {
      outlineLevel.Add(new GroupPoint(firstColumn, lastColumn)
      {
        IsParse = isParse
      });
      columnOutlineLevels.CustomAdd(columnLevel, outlineLevel);
    }
    else
    {
      columnOutlineLevels.TryGetValue(columnLevel, out outlineLevel);
      GroupPoint newLevel = new GroupPoint(firstColumn, lastColumn);
      newLevel.IsParse = isParse;
      if (isParse)
        outlineLevel.Add(newLevel);
      else
        this.InsertGroup(outlineLevel, firstColumn, newLevel, isImport);
    }
  }

  internal void InsertGroup(
    List<GroupPoint> outlineLevel,
    int first,
    GroupPoint newLevel,
    bool isImport)
  {
    int index1 = 0;
    bool flag = true;
    if (!isImport)
    {
      for (int index2 = 0; index2 < outlineLevel.Count; ++index2)
      {
        if (first < outlineLevel[index2].X)
        {
          index1 = index2;
          flag = false;
          break;
        }
      }
    }
    if (flag)
      outlineLevel.Add(newLevel);
    else
      outlineLevel.Insert(index1, newLevel);
  }
}
