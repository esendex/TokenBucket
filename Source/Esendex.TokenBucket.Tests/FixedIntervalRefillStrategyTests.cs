using System;
using NUnit.Framework;

namespace Esendex.TokenBucket.Tests
{
    public class FixedIntervalRefillStrategyTest
    {
        private const long N = 5; // 5 tokens
        private readonly TimeSpan _p = TimeSpan.FromSeconds(10);

        private MockTicker _ticker;
        private FixedIntervalRefillStrategy _strategy;

        [SetUp]
        public void SetUp()
        {
            _ticker = new MockTicker();
            _strategy = new FixedIntervalRefillStrategy(_ticker, N, _p);
        }

        [Test]
        public void FirstRefill()
        {
            Assert.AreEqual(N, _strategy.Refill());
        }

        [Test]
        public void NoRefillUntilPeriodUp()
        {
            _strategy.Refill();

            // Another refill shouldn't come for P units.
            for (var i = 0; i < _p.TotalSeconds - 1; i++)
            {
                _ticker.Advance(TimeSpan.FromSeconds(1));
                Assert.AreEqual(0, _strategy.Refill());
            }
        }

        [Test]
        public void RefillEveryPeriod()
        {
            for (var i = 0; i < 10; i++)
            {
                Assert.AreEqual(N, _strategy.Refill());
                _ticker.Advance(_p);
            }
        }

        private sealed class MockTicker : Ticker
        {
            private long _now;

            public override long Read()
            {
                return _now;
            }

            public void Advance(TimeSpan delta)
            {
                _now += delta.Ticks;
            }
        }
    }
}
