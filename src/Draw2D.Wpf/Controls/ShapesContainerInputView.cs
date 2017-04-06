﻿// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Windows.Controls;
using System.Windows.Input;
using Draw2D.Core.Editor;
using Draw2D.Core.ViewModels.Containers;

namespace Draw2D.Wpf.Controls
{
    public class ShapesContainerInputView : Border
    {
        private Modifier GetModifier()
        {
            Modifier modifier = Modifier.None;

            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Alt))
                modifier |= Modifier.Alt;

            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
                modifier |= Modifier.Control;

            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
                modifier |= Modifier.Shift;

            return modifier;
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);

            if (this.DataContext is ShapesContainerViewModel vm)
            {
                var point = e.GetPosition(Child);
                vm.CurrentTool.LeftDown(vm, point.X, point.Y, GetModifier());
                Child.InvalidateVisual();
            }
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonUp(e);

            if (this.DataContext is ShapesContainerViewModel vm)
            {
                var point = e.GetPosition(Child);
                vm.CurrentTool.LeftUp(vm, point.X, point.Y, GetModifier());
                Child.InvalidateVisual();
            }
        }

        protected override void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseRightButtonDown(e);

            if (this.DataContext is ShapesContainerViewModel vm)
            {
                var point = e.GetPosition(Child);
                vm.CurrentTool.RightDown(vm, point.X, point.Y, GetModifier());
                Child.InvalidateVisual();
            }
        }

        protected override void OnPreviewMouseRightButtonUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseRightButtonUp(e);

            if (this.DataContext is ShapesContainerViewModel vm)
            {
                var point = e.GetPosition(Child);
                vm.CurrentTool.RightUp(vm, point.X, point.Y, GetModifier());
                Child.InvalidateVisual();
            }
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);

            if (this.DataContext is ShapesContainerViewModel vm)
            {
                var point = e.GetPosition(Child);
                vm.CurrentTool.Move(vm, point.X, point.Y, GetModifier());
                Child.InvalidateVisual();
            }
        }
    }
}