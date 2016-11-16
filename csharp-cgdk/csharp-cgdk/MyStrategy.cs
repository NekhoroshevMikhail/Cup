using Com.CodeGame.CodeWizards2016.DevKit.CSharpCgdk.Model;
using System;
using System.Collections.Generic;

namespace Com.CodeGame.CodeWizards2016.DevKit.CSharpCgdk {
    public sealed class MyStrategy : IStrategy {
        private static double WAYPOINT_RADIUS = 100.0D;

        private static double LOW_HP_FACTOR = 0.25D;

        /**
         * Ключевые точки для каждой линии, позволяющие упростить управление перемещением волшебника.
         * <p>
         * Если всё хорошо, двигаемся к следующей точке и атакуем противников.
         * Если осталось мало жизненной энергии, отступаем к предыдущей точке.
         */
        private Dictionary<LaneType, Point2D[]> waypointsByLine = new Dictionary<LaneType, Point2D[]>();

    private Random random;

        private LaneType line;
        private Point2D[] waypoints;

        private Wizard self;
        private World world;
        private Game game;
        private Move move;


        public void Move(Wizard self, World world, Game game, Move move) {

            var myOwnWizard = new MyWizard(self);
            myOwnWizard.GetNewSkillIfICan(move);

            

            initializeStrategy(self, game);
            initializeTick(self, world, game, move);
            
            // Постоянно двигаемся из-стороны в сторону, чтобы по нам было сложнее попасть.
            // Считаете, что сможете придумать более эффективный алгоритм уклонения? Попробуйте! ;)
            move.StrafeSpeed = 0;//random.nextBoolean() ? game.getWizardStrafeSpeed() : -game.getWizardStrafeSpeed());

            // Если осталось мало жизненной энергии, отступаем к предыдущей ключевой точке на линии.
            if (self.Life < self.MaxLife * LOW_HP_FACTOR)
            {
                goTo(getPreviousWaypoint());
                return;
            }

            LivingUnit nearestTarget = getNearestTarget();

            // Если видим противника ...
            if (nearestTarget != null)
            {
                double distance = self.GetDistanceTo(nearestTarget);

                // ... и он в пределах досягаемости наших заклинаний, ...
                if (distance <= self.CastRange)
                {
                    double angle = self.GetAngleTo(nearestTarget);

                    // ... то поворачиваемся к цели.
                    move.Turn = angle;

                    // Если цель перед нами, ...
                    if (Math.Abs(angle) < game.StaffSector / 2.0D)
                    {
                        // ... то атакуем.
                        move.Action = myOwnWizard.ChoseBestAttackMethod(); ;
                        move.CastAngle = angle;
                        move.MinCastDistance = distance - nearestTarget.Radius + game.MagicMissileRadius;
                    }

                    return;
                }
            }

            // Если нет других действий, просто продвигаемся вперёд.
            goTo(getNextWaypoint());
        }


        private void initializeStrategy(Wizard self, Game game)
        {
            if (random == null)
            {
                //random = new Random((int)game.RandomSeed);
                random = new Random();
                //random.Next() == 1
                bool result = random.Next(0, 1) == 1;
                double mapSize = game.MapSize;

                waypointsByLine.Add(LaneType.Middle, new Point2D[]{
                    new Point2D(100.0D, mapSize - 100.0D),
                    result
                            ? new Point2D(600.0D, mapSize - 200.0D)
                            : new Point2D(200.0D, mapSize - 600.0D),
                            
                    new Point2D(800.0D, mapSize - 800.0D),
                    new Point2D(mapSize - 600.0D, 600.0D)
            });

                waypointsByLine.Add(LaneType.Top, new Point2D[]{
                    new Point2D(100.0D, mapSize - 100.0D),
                    new Point2D(100.0D, mapSize - 400.0D),
                    new Point2D(200.0D, mapSize - 800.0D),
                    new Point2D(200.0D, mapSize * 0.75D),
                    new Point2D(200.0D, mapSize * 0.5D),
                    new Point2D(200.0D, mapSize * 0.25D),
                    new Point2D(200.0D, 200.0D),
                    new Point2D(mapSize * 0.25D, 200.0D),
                    new Point2D(mapSize * 0.5D, 200.0D),
                    new Point2D(mapSize * 0.75D, 200.0D),
                    new Point2D(mapSize - 200.0D, 200.0D)
            });

                waypointsByLine.Add(LaneType.Bottom, new Point2D[]{
                    new Point2D(100.0D, mapSize - 100.0D),
                    new Point2D(400.0D, mapSize - 100.0D),
                    new Point2D(800.0D, mapSize - 200.0D),
                    new Point2D(mapSize * 0.25D, mapSize - 200.0D),
                    new Point2D(mapSize * 0.5D, mapSize - 200.0D),
                    new Point2D(mapSize * 0.75D, mapSize - 200.0D),
                    new Point2D(mapSize - 200.0D, mapSize - 200.0D),
                    new Point2D(mapSize - 200.0D, mapSize * 0.75D),
                    new Point2D(mapSize - 200.0D, mapSize * 0.5D),
                    new Point2D(mapSize - 200.0D, mapSize * 0.25D),
                    new Point2D(mapSize - 200.0D, 200.0D)
            });

                switch ((int)self.Id)
                {
                    case 1:
                    case 2:
                    case 6:
                    case 7:
                        line = LaneType.Top;
                        break;
                    case 3:
                    case 8:
                        line = LaneType.Middle;
                        break;
                    case 4:
                    case 5:
                    case 9:
                    case 10:
                        line = LaneType.Bottom;
                        break;
                    default:
                        break;
                }

                waypoints = waypointsByLine[line];

                // Наша стратегия исходит из предположения, что заданные нами ключевые точки упорядочены по убыванию
                // дальности до последней ключевой точки. Сейчас проверка этого факта отключена, однако вы можете
                // написать свою проверку, если решите изменить координаты ключевых точек.

                /*Point2D lastWaypoint = waypoints[waypoints.length - 1];

                Preconditions.checkState(ArrayUtils.isSorted(waypoints, (waypointA, waypointB) -> Double.compare(
                        waypointB.getDistanceTo(lastWaypoint), waypointA.getDistanceTo(lastWaypoint)
                )));*/
            }
        }

        private void initializeTick(Wizard self, World world, Game game, Move move)
        {
            this.self = self;
            this.world = world;
            this.game = game;
            this.move = move;
        }

        private Point2D getNextWaypoint()
        {
            int lastWaypointIndex = waypoints.Length - 1;
            Point2D lastWaypoint = waypoints[lastWaypointIndex];

            for (int waypointIndex = 0; waypointIndex < lastWaypointIndex; ++waypointIndex)
            {
                Point2D waypoint = waypoints[waypointIndex];

                if (waypoint.getDistanceTo(self) <= WAYPOINT_RADIUS)
                {
                    return waypoints[waypointIndex + 1];
                }

                if (lastWaypoint.getDistanceTo(waypoint) < lastWaypoint.getDistanceTo(self))
                {
                    return waypoint;
                }
            }

            return lastWaypoint;
        }

        /**
         * Действие данного метода абсолютно идентично действию метода {@code getNextWaypoint}, если перевернуть массив
         * {@code waypoints}.
         */
        private Point2D getPreviousWaypoint()
        {
            Point2D firstWaypoint = waypoints[0];

            for (int waypointIndex = waypoints.Length - 1; waypointIndex > 0; --waypointIndex)
            {
                Point2D waypoint = waypoints[waypointIndex];

                if (waypoint.getDistanceTo(self) <= WAYPOINT_RADIUS)
                {
                    return waypoints[waypointIndex - 1];
                }

                if (firstWaypoint.getDistanceTo(waypoint) < firstWaypoint.getDistanceTo(self))
                {
                    return waypoint;
                }
            }

            return firstWaypoint;
        }

        /**
         * Простейший способ перемещения волшебника.
         */
        private void goTo(Point2D point)
        {
            double angle = self.GetAngleTo(point.X, point.Y);

            move.Turn = angle;

            if (Math.Abs(angle) < game.StaffSector / 4.0D)
            {
                move.Speed = game.WizardForwardSpeed;
            }
        }

        /**
         * Находим ближайшую цель для атаки, независимо от её типа и других характеристик.
         */
        private LivingUnit getNearestTarget()
        {
            List<LivingUnit> targets = new List<LivingUnit>();
            targets.AddRange(world.Buildings);
            targets.AddRange(world.Wizards);
            targets.AddRange(world.Minions);

            LivingUnit nearestTarget = null;
            double nearestTargetDistance = Double.MaxValue;

            foreach (LivingUnit target in targets)
            {
                if (target.Faction == Faction.Neutral || target.Faction == self.Faction)
                {
                    continue;
                }

                double distance = self.GetDistanceTo(target);

                if (distance < nearestTargetDistance)
                {
                    nearestTarget = target;
                    nearestTargetDistance = distance;
                }
            }

            return nearestTarget;
        }

        /**
         * Вспомогательный класс для хранения позиций на карте.
         */
        private class Point2D
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
                get {
                    return _y;
                }
            }

            public double getDistanceTo(double x, double y)
            {
                //return Math.Hypot(_x - x, _y - y);
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
    
}