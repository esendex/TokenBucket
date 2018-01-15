using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace Esendex.TokenBucket.Tests
{
    public class TokenBucketsBuilderTests
    {
        private readonly TokenBuckets.Builder _builder = TokenBuckets.Construct();

        [Test, ExpectedException(typeof (ArgumentOutOfRangeException))]
        public void WithNegativeCapacity()
        {
            _builder.WithCapacity(-1);
        }

        [Test, ExpectedException(typeof (ArgumentOutOfRangeException))]
        public void WithZeroCapacity()
        {
            _builder.WithCapacity(0);
        }

        [Test, ExpectedException(typeof (ArgumentNullException))]
        public void WithNullRefillStrategy()
        {
            _builder.WithRefillStrategy(null);
        }

        [Test, ExpectedException(typeof (ArgumentNullException))]
        public void WithNullSleepStrategy()
        {
            _builder.WithSleepStrategy(null);
        }

        [Test, ExpectedException(typeof (InvalidOperationException))]
        public void BuildWhenCapacityNotSpecified()
        {
            _builder.Build();
        }
    }
}
