using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameArchitecture.UI
{
    public class FlexibleGridLayout : LayoutGroup
    {
        [Serializable] 
        public enum FitType
        {
            Uniform,
            Width,
            Height,
            FixedRows,
            FixedColumns
        }
        
        public FitType fitType;
        [Space] public int rows;
        public int columns;
        public Vector2 cellSize;
        public Vector2 spacing;

        public bool fitX;
        public bool fitY;

        public override void CalculateLayoutInputVertical()
        {
            base.CalculateLayoutInputHorizontal();

            if (fitType == FitType.Width || fitType == FitType.Height || fitType == FitType.Uniform)
            {
                fitX = true;
                fitY = true;
                
                float sqrRt = Mathf.Sqrt(transform.childCount);
                rows = Mathf.CeilToInt(sqrRt);
                columns = Mathf.CeilToInt(sqrRt);
            }

            if (fitType == FitType.Width || fitType == FitType.FixedColumns)
            {
                rows = Mathf.CeilToInt(transform.childCount / (float) columns);
            }
            else if (fitType == FitType.Height || fitType == FitType.FixedRows)
            {
                columns = Mathf.CeilToInt(transform.childCount / (float) rows);
            }

            var rect = rectTransform.rect;
            float parentWidth = rect.width;
            float parentHeight = rect.height;

            float cellWidth = parentWidth / columns - spacing.x/columns * 2f - 
                              (padding.left / columns) - (padding.right / columns);
            float cellHeight = parentHeight / rows - spacing.y/rows * 2f - 
                               (padding.top / rows) - (padding.bottom / rows);
            
            cellSize.x = fitX ? cellWidth : cellSize.x;
            cellSize.y = fitY ? cellHeight : cellSize.y;

            for (int i = 0, columnCount = 0, rowCount = 0; 
                 i < rectChildren.Count; i++)
            {
                rowCount = i / columns;
                columnCount = i % columns;
                
                var item = rectChildren[i];
                var xPos = cellSize.x * columnCount + spacing.x * columnCount + padding.left;
                var yPos = cellSize.y * rowCount + spacing.y * rowCount + padding.top;
                
                SetChildAlongAxis(item, 0, xPos, cellSize.x);
                SetChildAlongAxis(item, 1, yPos, cellSize.y);
            }
        }

        public override void SetLayoutHorizontal()
        {
            
        }

        public override void SetLayoutVertical()
        {
            
        }
    }
}