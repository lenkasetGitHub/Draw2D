﻿// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Linq;
using Draw2D.Core.Editor.Selection;
using Draw2D.Core.Shapes;
using Draw2D.Spatial;

namespace Draw2D.Core.Editor.Tools
{
    public class SelectionTool : ToolBase
    {
        private RectangleShape _rectangle;
        private double _originX;
        private double _originY;
        private double _previousX;
        private double _previousY;
        private bool _haveSelection;

        public enum State { TopLeft, BottomRight, Move };
        public State CurrentState = State.TopLeft;

        public override string Name { get { return "Selection"; } }

        public SelectionToolSettings Settings { get; set; }

        public override void LeftDown(IToolContext context, double x, double y, Modifier modifier)
        {
            base.LeftDown(context, x, y, modifier);

            switch (CurrentState)
            {
                case State.TopLeft:
                    {
                        _originX = x;
                        _originY = y;
                        _previousX = x;
                        _previousY = y;

                        Filters?.ForEach(f => f.Clear(context));
                        Filters?.Any(f => f.Process(context, ref _originX, ref _originY));
                        _previousX = _originX;
                        _previousY = _originY;

                        var target = new Point2(x, y);
                        var result = SelectionHelper.TryToSelect(
                            context,
                            Settings?.Mode ?? SelectionMode.Shape,
                            Settings?.Targets ?? SelectionTargets.Shapes,
                            target,
                            Settings?.HitTestRadius ?? 7.0,
                            modifier);
                        if (result)
                        {
                            _haveSelection = true;
                            CurrentState = State.Move;
                        }
                        else
                        {
                            _haveSelection = false;

                            if (!modifier.HasFlag(Modifier.Control))
                            {
                                context.Selected.Clear();
                            }

                            if (_rectangle == null)
                            {
                                _rectangle = new RectangleShape()
                                {
                                    TopLeft = new PointShape(),
                                    BottomRight = new PointShape()
                                };
                            }
                            _rectangle.TopLeft.X = x;
                            _rectangle.TopLeft.Y = y;
                            _rectangle.BottomRight.X = x;
                            _rectangle.BottomRight.Y = y;
                            _rectangle.Style = Settings?.SelectionStyle;
                            context.WorkingContainer.Shapes.Add(_rectangle);
                            CurrentState = State.BottomRight;
                        }
                    }
                    break;
                case State.BottomRight:
                    {
                        CurrentState = State.TopLeft;
                        _rectangle.BottomRight.X = x;
                        _rectangle.BottomRight.Y = y;
                    }
                    break;
            }
        }

        public override void LeftUp(IToolContext context, double x, double y, Modifier modifier)
        {
            base.LeftUp(context, x, y, modifier);

            Filters?.ForEach(f => f.Clear(context));

            switch (CurrentState)
            {
                case State.BottomRight:
                    {
                        var target = _rectangle.ToRect2();
                        var result = SelectionHelper.TryToSelect(
                            context,
                            Settings?.Mode ?? SelectionMode.Shape,
                            Settings?.Targets ?? SelectionTargets.Shapes,
                            target,
                            Settings?.HitTestRadius ?? 7.0,
                            modifier);
                        if (result)
                        {
                            _haveSelection = true;
                        }

                        context.WorkingContainer.Shapes.Remove(_rectangle);
                        _rectangle = null;
                        CurrentState = State.TopLeft;
                    }
                    break;
                case State.Move:
                    {
                        CurrentState = State.TopLeft;
                    }
                    break;
            }
        }

        public override void RightDown(IToolContext context, double x, double y, Modifier modifier)
        {
            base.RightDown(context, x, y, modifier);

            switch (CurrentState)
            {
                case State.BottomRight:
                    {
                        this.Clean(context);
                    }
                    break;
                case State.Move:
                    {
                        CurrentState = State.TopLeft;
                    }
                    break;
            }
        }

        public override void Move(IToolContext context, double x, double y, Modifier modifier)
        {
            base.Move(context, x, y, modifier);

            switch (CurrentState)
            {
                case State.TopLeft:
                    {
                        if (!_haveSelection)
                        {
                            var target = new Point2(x, y);
                            SelectionHelper.TryToHover(
                                context, 
                                Settings?.Mode ?? SelectionMode.Shape, 
                                Settings?.Targets ?? SelectionTargets.Shapes, 
                                target, 
                                Settings?.HitTestRadius ?? 7.0);
                        }
                    }
                    break;
                case State.BottomRight:
                    {
                        _rectangle.BottomRight.X = x;
                        _rectangle.BottomRight.Y = y;
                    }
                    break;
                case State.Move:
                    {
                        Filters?.ForEach(f => f.Clear(context));
                        Filters?.Any(f => f.Process(context, ref x, ref y));

                        double dx = x - _previousX;
                        double dy = y - _previousY;
                        _previousX = x;
                        _previousY = y;

                        if (context.Selected.Count == 1)
                        {
                            var shape = context.Selected.FirstOrDefault();

                            shape.Move(context.Selected, dx, dy);

                            if (shape.GetType() == typeof(PointShape))
                            {
                                var point = shape as PointShape;

                                if (Settings.ConnectPoints)
                                {
                                    PointShape result = context.HitTest.TryToGetPoint(
                                        context.CurrentContainer.Shapes, 
                                        new Point2(point.X, point.Y), 
                                        Settings?.ConnectTestRadius ?? 7.0);
                                    if (result != point)
                                    {
                                        // TODO: Connect point.
                                    }
                                }

                                if (Settings.DisconnectPoints)
                                {
                                    if ((Math.Abs(_originX - point.X) > Settings.DisconnectTestRadius)
                                        || (Math.Abs(_originY - point.Y) > Settings.DisconnectTestRadius))
                                    {
                                        // TODO: Disconnect point.
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (var shape in context.Selected)
                            {
                                shape.Move(context.Selected, dx, dy);
                            }
                        }
                    }
                    break;
            }
        }

        public override void Clean(IToolContext context)
        {
            base.Clean(context);

            CurrentState = State.TopLeft;
            _haveSelection = false;

            if (_rectangle != null)
            {
                context.WorkingContainer.Shapes.Remove(_rectangle);
                _rectangle = null;
            }

            context.Selected.Clear();

            Filters?.ForEach(f => f.Clear(context));
        }
    }
}