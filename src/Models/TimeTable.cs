using System;

namespace Models
{
    public class TimeTable<TKey, TValue>
    {
        //todo use dictionary and linked list for fast updates
        public void Update(TKey key, TValue value)
        {

        }

        public (TKey key, TValue value)[] ToArray()
        {
            throw new NotImplementedException();
        }
    }
}
