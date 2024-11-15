﻿using System.Numerics;
using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface.CustomControls;
using Robust.Client.UserInterface.XAML;
using Robust.Shared.Prototypes;

namespace ArabicaCliento.UI.Controls
{
    [GenerateTypedNameReferences]
    [Virtual]
    public partial class ArabicaWindow : BaseWindow
    {
        private const int DRAG_MARGIN_SIZE = 7;

        public ArabicaWindow()
        {
            RobustXamlLoader.Load(this);
        }

        protected override DragMode GetDragModeFor(Vector2 relativeMousePos)
        {
            var mode = DragMode.Move;

            if (!Resizable) return mode;
            if (relativeMousePos.Y < DRAG_MARGIN_SIZE)
            {
                mode = DragMode.Top;
            }
            else if (relativeMousePos.Y > Size.Y - DRAG_MARGIN_SIZE)
            {
                mode = DragMode.Bottom;
            }

            if (relativeMousePos.X < DRAG_MARGIN_SIZE)
            {
                mode |= DragMode.Left;
            }
            else if (relativeMousePos.X > Size.X - DRAG_MARGIN_SIZE)
            {
                mode |= DragMode.Right;
            }

            return mode;
        }
    }
}
