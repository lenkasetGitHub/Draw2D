﻿// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Windows.Media;
using Draw2D.Core.Style;

namespace Draw2D.Wpf.Renderers
{
    public struct WpfBrushCache
    {
        public readonly Brush Stroke;
        public readonly Pen StrokePen;
        public readonly Brush Fill;

        public WpfBrushCache(Brush stroke, Pen strokePen, Brush fill)
        {
            this.Stroke = stroke;
            this.StrokePen = strokePen;
            this.Fill = fill;
        }

        public static Color FromDrawColor(DrawColor color)
        {
            return Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static WpfBrushCache FromDrawStyle(DrawStyle style)
        {
            var stroke = new SolidColorBrush(FromDrawColor(style.Stroke));
            var strokePen = new Pen(stroke, style.Thickness);
            var fill = new SolidColorBrush(FromDrawColor(style.Fill));
            stroke.Freeze();
            strokePen.Freeze();
            fill.Freeze();
            return new WpfBrushCache(stroke, strokePen, fill);
        }
    }
}