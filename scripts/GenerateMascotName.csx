#!/usr/bin/env dotnet-script
// GenerateMascotName.csx
// Generates a random, repeatable mascot name seeded by the release tag.
// Usage: dotnet script GenerateMascotName.csx -- <tag>
// Outputs the name to stdout so the workflow can capture it.

using System;
using System.Linq;

var tag = Args.FirstOrDefault() ?? "1.0.0";

// Seed the random number generator with a hash of the tag
// so the same tag always produces the same name.
var seed = tag.Aggregate(0, (acc, c) => acc * 31 + c);
var rng = new Random(seed);

// Pokémon / anime style names are built from two or three fused syllable chunks,
// mimicking how names like Char-man-der, Pika-chu, Syl-ve-on, or Gar-cho-mp are constructed.

string[] prefixes = {
    "Flam", "Volt", "Aqua", "Glaci", "Umbra", "Solari", "Pyro", "Cryo",
    "Lumi", "Noxu", "Tempra", "Ferri", "Sylva", "Dracо", "Aero", "Terra",
    "Magni", "Spectra", "Venom", "Chrono", "Zephyr", "Astra", "Blaze", "Frost"
};

string[] middles = {
    "chi", "ra", "on", "ku", "zen", "mi", "vo", "rex",
    "lix", "dra", "wyn", "pha", "lor", "tsu", "ven", "shi",
    "mon", "kai", "ryu", "zel", "bu", "fur", "nat", "gon"
};

string[] suffixes = {
    "ix", "on", "ara", "eon", "ux", "aki", "oru", "ash",
    "en", "iro", "oth", "el", "us", "ina", "ax", "chu",
    "ion", "yx", "ori", "eki", "amon", "zar", "nis", "oth"
};

var prefix  = prefixes[rng.Next(prefixes.Length)];
var middle  = middles[rng.Next(middles.Length)];
var suffix  = suffixes[rng.Next(suffixes.Length)];

Console.WriteLine($"{prefix}{middle}{suffix}");
