# MoeLib

A simple library of Entity-Framework content implementations, as well as various helper functions that make my life easier.

## ENTITY-COMPONENT DESIGN
Instead of prioritizing an object-oriented approach, MoeLib utilizes a component model design which allows for greater modularity and specification of content.
MoeLib also provides various components which can assist with general activities, such as the DeprecatedComponent for Items, which automatically removes an Item from basic play.

## JSON ENTITY INSTANTIATION
MoeLib implements a simple backend for loading content (primarily Items) through JSON files as opposed to creating separate class files.
