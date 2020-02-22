using System;
using System.Collections.Generic;
using Models;

namespace BusTimetable.Generator.Generators
{
    static class BusStopGenerator
    {
        private static readonly HashSet<(int slotShiftX, int slotShiftY)> UsedSlots = new HashSet<(int slotShiftX, int slotShiftY)>();
        private static readonly Random SlotRandom = new Random(Guid.NewGuid().GetHashCode());
        private static readonly Random XRandom = new Random(Guid.NewGuid().GetHashCode());
        private static readonly Random YRandom = new Random(Guid.NewGuid().GetHashCode());

        public static BusStop[] Generate(int size, int busStopsNumber)
        {
            var slotsConfig = GetSlots(size, busStopsNumber);
            var stops = new BusStop[busStopsNumber];
            for (var i = 0; i < busStopsNumber; i++)
            {
                var stop = GetNextStop(slotsConfig.SlotsNumber, slotsConfig.SlotSize);
                stops[i] = new BusStop
                {
                    Id = IdGenerator.GetId(i),
                    Color = ColorHexGenerator.GetColor(),
                    X = stop.X,
                    Y = stop.Y
                };
            }

            return stops;
        }

        private static (int SlotsNumber, int SlotSize) GetSlots(int size, int busStopsNumber)
        {
            var slotsNumber = busStopsNumber / 2;
            var slotSize = size / slotsNumber;
            return (slotsNumber, slotSize);
        }

        private static (int X, int Y) GetNextStop(int slotsNumber, int slotsSize)
        {
            (int slotShiftX, int slotShiftY) slot;
            while (true)
            {
                slot = (SlotRandom.Next(1, slotsNumber - 1), SlotRandom.Next(1, slotsNumber -1));
                if (!UsedSlots.Contains(slot))
                {
                    UsedSlots.Add(slot);
                    break;
                }
            }

            return (XRandom.Next(slot.slotShiftX * slotsSize, slot.slotShiftX * (slotsSize + 1)),
                YRandom.Next(slot.slotShiftY * slotsSize, slot.slotShiftY * (slotsSize + 1)));
        }
    }
}
