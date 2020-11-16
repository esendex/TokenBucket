using NUnit.Framework;
using System;
namespace CasCap.Apis.TokenBucket.Tests
{
    public class TokenBucketsBuilderTests
    {
        readonly TokenBuckets.Builder _builder = TokenBuckets.Construct();

        [Test]
        public void WithNegativeCapacity()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _builder.WithCapacity(-1));
        }

        [Test]
        public void WithZeroCapacity()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _builder.WithCapacity(0));
        }

        [Test]
        public void WithNullRefillStrategy()
        {
            Assert.Throws<ArgumentNullException>(() => _builder.WithRefillStrategy(null));
        }

        [Test]
        public void WithNullSleepStrategy()
        {
            Assert.Throws<ArgumentNullException>(() => _builder.WithSleepStrategy(null));
        }

        [Test]
        public void BuildWhenCapacityNotSpecified()
        {
            Assert.Throws<InvalidOperationException>(() => _builder.Build());
        }
    }
}