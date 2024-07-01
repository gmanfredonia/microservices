using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Building.Base.Exceptions
{
    public interface IMappedExceptionCollection : IReadOnlyCollection<MappedException>
    {
        void Map(Type exceptionType, int statusCode, string messageKey);
    }
    public class MappedException
    {
        public required Type ExceptionType { get; init; }
        public required int StatusCode { get; init; }
        public required string MessageKey { get; init; }
    }

    public class MappedExceptionCollection : IMappedExceptionCollection
    {
        public MappedExceptionCollection() => v = [];

        public int Count => v.Count;
        public IEnumerator<MappedException> GetEnumerator() => v.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => v.GetEnumerator();

        public void Map(Type exceptionType, int statusCode, string messageKey)
            => v.Add(new MappedException() { ExceptionType = exceptionType, StatusCode = statusCode, MessageKey = messageKey });

        private readonly ICollection<MappedException> v;
    }
}
