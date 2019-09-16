using System;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;

namespace Esendex.TokenBucket.Tests
{
    [TestFixture]
    public class TokenBucketRefillTests
    {
        [Test, Explicit("Long Running")]
        public void RateLimitTests()
        {
            const int totalConsumes = 500;
            const int refillRate = 40;

            var tokenBucket = TokenBuckets.Construct()
                                          .WithCapacity(refillRate)
                                          .WithYieldingSleepStrategy()
                                          .WithFixedIntervalRefillStrategy(1, TimeSpan.FromMilliseconds(1000d / refillRate))
                                          .Build();

            var sw = new Stopwatch();
            sw.Start();
            for (var i = 0; i < totalConsumes; i++)
            {
                if (i % 3 == 0) Thread.Sleep(1000 / refillRate * 2);
                tokenBucket.Consume();
            }

            sw.Stop();

            Assert.That(totalConsumes / (sw.Elapsed.TotalSeconds + 1), Is.EqualTo(refillRate).Within(0.1));
        }
    }
}
