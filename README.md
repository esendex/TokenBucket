[azdo-badge]: https://dev.azure.com/f2calv/github/_apis/build/status/f2calv.CasCap.Apis.TokenBucket?branchName=master
[azdo-url]: https://dev.azure.com/f2calv/github/_build/latest?definitionId=9&branchName=master
[azdo-coverage-url]: https://img.shields.io/azure-devops/coverage/f2calv/github/9
[cascap.apis.tokenbucket-badge]: https://img.shields.io/nuget/v/CasCap.Apis.TokenBucket?color=blue
[cascap.apis.tokenbucket-url]: https://nuget.org/packages/CasCap.Apis.TokenBucket

[![Build Status][azdo-badge]][azdo-url] <!-- ![Code Coverage][azdo-coverage-url] --> [![Nuget][cascap.apis.tokenbucket-badge]][cascap.apis.tokenbucket-url]


# Introduction

This library provides an implementation of a token bucket algorithm which is useful for providing rate limited access
to a portion of code.  The implementation provided is that of a "leaky bucket" in the sense that the bucket has a finite
capacity and any added tokens that would exceed this capacity will "overflow" out of the bucket and be lost forever.

In this implementation the rules for refilling the bucket are encapsulated in a provided IRefillStrategy instance.  Prior
to attempting to consume any tokens the refill strategy will be consulted to see how many tokens should be added to the
bucket

See also:

* [Wikipedia - Token Bucket](http://en.wikipedia.org/wiki/Token_bucket)
* [Wikipedia - Leaky Bucket](http://en.wikipedia.org/wiki/Leaky_bucket)

This is an update of the existing [esendex](https://github.com/esendex/TokenBucket) library to now target .NET Standard 2.0 & .NET 5.0.
The [esendex](https://github.com/esendex/TokenBucket) library was in turn a port to C# of the original work by [Brandon Beck](https://github.com/bbeck/token-bucket).

## Usage

Using a token bucket is incredibly easy and is best illustrated by an example.  Suppose you have a piece of code that
polls a website and you would only like to be able to access the site once per second:

```C#
// Create a token bucket with a capacity of 1 token that refills at a fixed interval of 1 token/sec.
ITokenBucket bucket = TokenBuckets.Construct()
  .WithCapacity(1)
  .WithFixedIntervalRefillStrategy(1, TimeSpan.FromSeconds(1))
  .Build();

// ...

while (true)
{
  // Consume a token from the token bucket.  If a token is not available this method will block until
  // the refill strategy adds one to the bucket.
  bucket.Consume(1);

  Poll();
}
```

As another example suppose you wanted to rate limit the size response of a server to the client to 20 kb/sec but want to
allow for a periodic burst rate of 40 kb/sec:

```C#
// Create a token bucket with a capacity of 40 kb tokens that refills at a fixed interval of 20 kb tokens per second
ITokenBucket bucket = TokenBuckets.Construct()
  .WithCapacity(40960)
  .WithFixedIntervalRefillStrategy(20480, TimeSpan.FromSeconds(1))
  .Build();

// ...

while (true) {
  String response = PrepareResponse();

  // Consume tokens from the bucket commensurate with the size of the response
  bucket.Consume(response.Length);

  Send(response);
}
```

## NuGet

Install from NuGet

```PowerShell
Install-Package CasCap.Apis.TokenBucket
```

## License

Copyright 2020 Alex Vincent

Copyright 2015 Esendex Ltd

Copyright 2012-2014 Brandon Beck

Licensed under the Apache Software License, Version 2.0: <http://www.apache.org/licenses/LICENSE-2.0>.
