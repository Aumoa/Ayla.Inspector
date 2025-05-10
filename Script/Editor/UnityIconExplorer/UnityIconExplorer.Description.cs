#if UNITY_EDITOR
#nullable enable

using UnityEditor;
using UnityEngine;

namespace Ayla.Inspector
{
    public partial class UnityIconExplorer
    {
        private static class Description
        {
            public const float Height = LabelHeight + 1 + DescriptionHeight;

            private const float LabelHeight = 18;
            private const float DescriptionHeight = LabelHeight * 4;
            private const float MarginDescription = 5;
            private const float DescriptionHeaderWidth = 100;

            public static void OnGUI(IUnityIconExplorer context, DrawingArgs drawingArgs)
            {
                var headerLayout = drawingArgs.FillTop(LabelHeight);
                GUI.Label(headerLayout.DrawingRect, "Selected Icon", EditorStyles.boldLabel);
                drawingArgs = drawingArgs.MarginTop(LabelHeight);

                HorizontalBorder.Draw(drawingArgs);
                drawingArgs = drawingArgs.MarginTop(1);

                var icon = UnityIconCollection.GetIconSafe(context.Selected);
                if (icon.Id == 0)
                {
                    return;
                }

                var previewLayout = drawingArgs.FillLeft(DescriptionHeight);
                EditorGUI.DrawTextureTransparent(previewLayout.FillLeft(DescriptionHeight).DrawingRect, icon.Texture, ScaleMode.ScaleToFit);
                drawingArgs = drawingArgs.MarginLeft(DescriptionHeight);

                VerticalBorder.Draw(drawingArgs);
                drawingArgs = drawingArgs.MarginLeft(1);

                drawingArgs = drawingArgs.MarginLeft(MarginDescription);

                var nameLayout = drawingArgs.FillTop(LabelHeight);
                GUI.Label(nameLayout.DrawingRect, "Name", EditorStyles.boldLabel);
                nameLayout = nameLayout.MarginLeft(DescriptionHeaderWidth);
                EditorGUI.SelectableLabel(nameLayout.DrawingRect, icon.Texture.name);

                drawingArgs = drawingArgs.MarginTop(LabelHeight);

                var descriptionLayout = drawingArgs.FillTop(LabelHeight);
                GUI.Label(descriptionLayout.DrawingRect, "Asset Path", EditorStyles.boldLabel);
                descriptionLayout = descriptionLayout.MarginLeft(DescriptionHeaderWidth);
                EditorGUI.SelectableLabel(descriptionLayout.DrawingRect, icon.AssetPath);

                drawingArgs = drawingArgs.MarginTop(LabelHeight);

                var sizeLayout = drawingArgs.FillTop(LabelHeight);
                GUI.Label(sizeLayout.DrawingRect, "Size", EditorStyles.boldLabel);
                sizeLayout = sizeLayout.MarginLeft(DescriptionHeaderWidth);
                EditorGUI.SelectableLabel(sizeLayout.DrawingRect, $"{icon.Texture.width} x {icon.Texture.height}");

                drawingArgs = drawingArgs.MarginTop(LabelHeight);

                var formatLayout = drawingArgs.FillTop(LabelHeight);
                GUI.Label(formatLayout.DrawingRect, "Graphics", EditorStyles.boldLabel);
                formatLayout = formatLayout.MarginLeft(DescriptionHeaderWidth);
                EditorGUI.SelectableLabel(formatLayout.DrawingRect, icon.Texture.graphicsFormat.ToString());
            }
        }
    }
}
#endif