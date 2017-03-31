﻿using System.Collections.Generic;
using Draw2D.Models.Renderers;

namespace Draw2D.Models.Shapes
{
    public class CubicBezierShape : ConnectableShape
    {
        private PointShape _startPoint;
        private PointShape _point1;
        private PointShape _point2;
        private PointShape _point3;

        public PointShape StartPoint
        {
            get => _startPoint;
            set => Update(ref _startPoint, value);
        }

        public PointShape Point1
        {
            get => _point1;
            set => Update(ref _point1, value);
        }

        public PointShape Point2
        {
            get => _point2;
            set => Update(ref _point2, value);
        }

        public PointShape Point3
        {
            get => _point3;
            set => Update(ref _point3, value);
        }

        public CubicBezierShape()
            : base()
        {
        }

        public CubicBezierShape(PointShape startPoint, PointShape point1, PointShape point2, PointShape point3)
            : base()
        {
            this.StartPoint = startPoint;
            this.Point1 = point1;
            this.Point2 = point2;
            this.Point3 = point3;
        }

        public override void Draw(object dc, ShapeRenderer renderer, double dx, double dy)
        {
            base.BeginTransform(dc, renderer);

            renderer.DrawCubicBezier(dc, this, Style, dx, dy);

            _startPoint.Draw(dc, renderer, dx, dy);
            _point1.Draw(dc, renderer, dx, dy);
            _point2.Draw(dc, renderer, dx, dy);
            _point3.Draw(dc, renderer, dx, dy);

            base.Draw(dc, renderer, dx, dy);
            base.EndTransform(dc, renderer);
        }

        public override void Move(ISet<ShapeObject> selected, double dx, double dy)
        {
            if (!selected.Contains(_startPoint))
            {
                _startPoint.Move(selected, dx, dy);
            }

            if (!selected.Contains(_point1))
            {
                _point1.Move(selected, dx, dy);
            }

            if (!selected.Contains(_point2))
            {
                _point2.Move(selected, dx, dy);
            }

            if (!selected.Contains(_point3))
            {
                _point3.Move(selected, dx, dy);
            }

            base.Move(selected, dx, dy);
        }

        public override void Select(ISet<ShapeObject> selected)
        {
            base.Select(selected);
            StartPoint.Select(selected);
            Point1.Select(selected);
            Point2.Select(selected);
            Point3.Select(selected);
        }

        public override void Deselect(ISet<ShapeObject> selected)
        {
            base.Deselect(selected);
            StartPoint.Deselect(selected);
            Point1.Deselect(selected);
            Point2.Deselect(selected);
            Point3.Deselect(selected);
        }
    }
}