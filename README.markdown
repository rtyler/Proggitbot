Proggitbot
==========

Probably a bit waste of an evening on my part, but a simple .NET 
based IRC bot that will ping [Proggit](http://reddit.com/r/programming)
and post new stories to the IRC channel ([##proggit](irc://irc.freenode.net/##proggit))
as they hit the proggit home page.

Some values are still hard-coded, primarily out of developer (see: me) laziness.


Pre-requisites
---------------
Building relies on [NAnt](http://nant.sf.net) and testing relies on 
[NUnit](http://nunit.org), Proggitbot has been built and tested against 
[Mono](http://www.mono-project.org) 2.3 at the earliest.
