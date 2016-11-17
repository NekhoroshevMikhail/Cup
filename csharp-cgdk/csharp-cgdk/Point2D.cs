// ---------------------------------------------------------------------------------------------------------------------------------------------------
// Copyright ElcomPlus LLC. All rights reserved.
// Author: Нехорошев М. В.
// ---------------------------------------------------------------------------------------------------------------------------------------------------

using System;
using Com.CodeGame.CodeWizards2016.DevKit.CSharpCgdk.Model;

namespace Com.CodeGame.CodeWizards2016.DevKit.CSharpCgdk
{
    /**
       * Находим ближайшую цель для атаки, независимо от её типа и других характеристик.
       */
    /**
     * Вспомогательный класс для хранения позиций на карте.
     */

    public class Point2D
    {
        private double _x;
        private double _y;

        public Point2D(double x, double y)
        {
            _x = x;
            _y = y;
        }

        public double X
        {
            get { return _x; }
        }

        public double Y
        {
            get { return _y; }
        }

        public double getDistanceTo(double x, double y)
        {
            return Math.Sqrt(Math.Pow(_x - x, 2) + Math.Pow(_y - y, 2));
        }

        public double getDistanceTo(Point2D point)
        {
            return getDistanceTo(point.X, point.Y);
        }

        public double getDistanceTo(Unit unit)
        {
            return getDistanceTo(unit.X, unit.Y);
        }
    }
}
