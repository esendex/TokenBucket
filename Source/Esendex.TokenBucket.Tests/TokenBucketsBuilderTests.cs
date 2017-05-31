using System;
using NUnit.Framework;

namespace Esendex.TokenBucket.Tests
{
    public class TokenBucketsBuilderTests
    {
        private readonly TokenBuckets.Builder _builder = TokenBuckets.Construct();

        [Test]
        public void WithNegativeCapacity()
        {
            Assert.That(() => _builder.WithCapacity(-1), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void WithZeroCapacity()
        {
            Assert.That(() => _builder.WithCapacity(0), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void WithNullRefillStrategy()
        {
            Assert.That(() => _builder.WithRefillStrategy(null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void WithNullSleepStrategy()
        {
            Assert.That(() => _builder.WithSleepStrategy(null), Throws.InstanceOf<ArgumentNullException>());
        }

        [Test]
        public void BuildWhenCapacityNotSpecified()
        {
            Assert.That(() => _builder.Build(), Throws.InstanceOf<InvalidOperationException>());
        }
    }
}
