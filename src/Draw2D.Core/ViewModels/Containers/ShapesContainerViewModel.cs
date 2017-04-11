﻿// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Draw2D.Core.Containers;
using Draw2D.Core.Editor;
using Draw2D.Core.Editor.Bounds;
using Draw2D.Core.Presenters;
using Draw2D.Core.Renderers;
using Draw2D.Core.Shapes;
using Draw2D.Core.Style;
using Draw2D.Spatial;

namespace Draw2D.Core.ViewModels.Containers
{
    public class ShapesContainerViewModel : NamedObject, IToolContext
    {
        private ObservableCollection<ToolBase> _tools;
        private ToolBase _currentTool;
        private ShapePresenter _presenter;

        private ShapeRenderer _renderer;
        private ISet<ShapeObject> _selected;
        private IHitTest _hitTest;
        private IShapesContainer _currentContainer;
        private IShapesContainer _workingContainer;
        private DrawStyle _currentStyle;
        private ShapeObject _pointShape;

        public ObservableCollection<ToolBase> Tools
        {
            get => _tools;
            set => Update(ref _tools, value);
        }

        public ToolBase CurrentTool
        {
            get => _currentTool;
            set
            {
                _currentTool?.Clean(this);
                Update(ref _currentTool, value);
            }
        }

        public ShapePresenter Presenter
        {
            get => _presenter;
            set => Update(ref _presenter, value);
        }

        public ShapeRenderer Renderer
        {
            get => _renderer;
            set => Update(ref _renderer, value);
        }

        public ISet<ShapeObject> Selected
        {
            get => _selected;
            set => Update(ref _selected, value);
        }

        public IHitTest HitTest
        {
            get => _hitTest;
            set => Update(ref _hitTest, value);
        }

        public IShapesContainer CurrentContainer
        {
            get => _currentContainer;
            set => Update(ref _currentContainer, value);
        }

        public IShapesContainer WorkingContainer
        {
            get => _workingContainer;
            set => Update(ref _workingContainer, value);
        }

        public DrawStyle CurrentStyle
        {
            get => _currentStyle;
            set => Update(ref _currentStyle, value);
        }

        public ShapeObject PointShape
        {
            get => _pointShape;
            set => Update(ref _pointShape, value);
        }

        public Action Capture { get; set; }

        public Action Release { get; set; }

        public Action Invalidate { get; set; }

        public PointShape GetNextPoint(double x, double y, bool connect, double radius)
        {
            if (connect == true)
            {
                PointShape point = HitTest.TryToGetPoint(CurrentContainer.Shapes, new Point2(x, y), radius, null);
                if (point != null)
                {
                    return point;
                }
            }
            return new PointShape(x, y, PointShape);
        }
    }
}
