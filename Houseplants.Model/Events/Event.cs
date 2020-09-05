using System;
using System.Linq;

namespace Houseplants.Model.Events
{
    public interface IPotEvent
    {
        Pot Pot { get; }
    }

    public interface IFlowerEvent
    {
        Flower Flower { get; }
    }

    /// <summary>
    /// Событие не меняет состояние объекта, а добавляет запись в его "летопись".
    /// Можно получить состояние объекта в любой момент времени.
    /// </summary>
    public abstract class Event
    {
        public Event()
        {
            Date = DateTime.UtcNow;
        }

        public Event(DateTime date, string descr)
        {
            Date = date;
            Description = descr;
        }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", Date, Description);
        }
    }

    /// <summary>
    /// Пересадка
    /// </summary>
    public class Replanting : Event
    {
        public Replanting(Flower flower, Soil soil, Pot pot)
        {
        }
    }

    /// <summary>
    /// Перестановка на новое место.
    /// </summary>
    public class Transposing : Event
    {
        public readonly Pot Pot;

        public Transposing(Pot pot, Place place)
        {
        }
    }

    /// <summary>
    /// Обрезка
    /// </summary>
    public class Cutting : Event
    {
        public Cutting(Flower flower)
        {
        }
    }
}