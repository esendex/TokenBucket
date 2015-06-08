using System;
using NUnit.Framework;

namespace Esendex.TokenBucket.Tests
{
    public class FixedIntervalRefillStrategyTest
    {
        private const long NumberOfTokens = 5;
        private readonly TimeSpan _period = TimeSpan.FromSeconds(10);

        private MockTicker _ticker;
        private FixedIntervalRefillStrategy _strategy;

        [SetUp]
        public void SetUp()
        {
            _ticker = new MockTicker();
            _strategy = new FixedIntervalRefillStrategy(_ticker, NumberOfTokens, _period);
        }

        [Test]
        public void FirstRefill()
        {
            Assert.AreEqual(NumberOfTokens, _strategy.Refill());
        }

        [Test]
        public void NoRefillUntilPeriodUp()
        {
            _strategy.Refill();

            // Another refill shouldn't come for P units.
            for (var i = 0; i < _period.TotalSeconds - 1; i++)
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
                Assert.AreEqual(NumberOfTokens, _strategy.Refill());
                _ticker.Advance(_period);
            }
        }

        [Test]
        public void RefillMultipleTokensWhenMultiplePeriodsElapse()
        {
            _ticker.Advance(TimeSpan.FromSeconds(_period.TotalSeconds*3));
            Assert.That(_strategy.Refill(), Is.EqualTo(NumberOfTokens*3));

            _ticker.Advance(_period);
            Assert.That(_strategy.Refill(), Is.EqualTo(NumberOfTokens));
        }

        [Test]
        public void RefillAtFixedRateWhenCalledWithInconsistentRate()
        {
            _ticker.Advance(TimeSpan.FromSeconds(_period.TotalSeconds/2));
            Assert.That(_strategy.Refill(), Is.EqualTo(NumberOfTokens));

            _ticker.Advance(TimeSpan.FromSeconds(_period.TotalSeconds/2));
            Assert.That(_strategy.Refill(), Is.EqualTo(NumberOfTokens));
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
