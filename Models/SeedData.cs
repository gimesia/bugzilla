﻿using System;
using System.Collections.Generic;
using System.Linq;
using bugzilla.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;


public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (
            var context = new BugzillaDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<BugzillaDbContext>>()
            ))
        {
            //If DB isn't empty
            if (EnumerableExtensions.Any(context.Bugs) || EnumerableExtensions.Any(context.Developers) ||
                EnumerableExtensions.Any(context.Reviews) || EnumerableExtensions.Any(context.Fixes) ||
                EnumerableExtensions.Any(context.Roles))
            {
                return;
            }

            //If DB is empty
            context.Database.EnsureCreated();

            var roles = GetRandomRoles();
            var developers = GetRandomDevelopers(roles);
            var bugs = GetRandomBugs(developers);
            var fixes = GetRandomFixes(bugs,developers);
            var reviews = GetRandomReviews(fixes, developers);

            context.Roles.AddRange(roles);
            context.Developers.AddRange(developers);
            context.Bugs.AddRange(bugs);
            context.Fixes.AddRange(fixes);
            context.Reviews.AddRange(reviews);

            context.SaveChanges();
        }
    }

    private static List<Developer> GetRandomDevelopers(List<Role> roles)
    {
        var lead = roles[0];
        var devs = new List<Developer>();
        var rnd = new Random();

        foreach (var i in Enumerable.Range(1, 7))
        {
            devs.Add(new Developer
            {
                Id = Guid.NewGuid(),
                Name = GetRandomName(),
                Role = i == 1 ? lead : roles[rnd.Next(1, roles.Count - 1)]
            });
        }

        return devs;
    }

    private static List<Role> GetRandomRoles()
    {
        return new List<Role>
        {
            new Role {Id = Guid.Parse("845dad38-6761-4717-b66e-000000000000"), Name = "Dev Lead"},
            new Role {Id = Guid.NewGuid(), Name = "Senior Dev"},
            new Role {Id = Guid.NewGuid(), Name = "Junior Dev"},
            new Role {Id = Guid.NewGuid(), Name = "Manual Tester"}
        };
    }

    private static string GetRandomName()
    {
        var lastNames = new string[]
        {
            "Toth",
            "Krekics",
            "Bibo",
            "Marten",
            "Schultheisz",
            "Kender",
            "Szebeni",
            "Markos",
            "Kocsis",
            "Kecskes",
            "Lassu",
            "Racz"
        };
        var firstNames = new string[]
        {
            "Veca",
            "Tibi",
            "Lajos",
            "Joci",
            "Erno",
            "Sara",
            "Anna",
            "Peti",
            "Andras",
            "Jozsi",
            "Kata",
            "Bela",
            "Vazul"
        };
        var rnd = new Random();
        return firstNames[rnd.Next(0, firstNames.Length - 1)] + " " +lastNames[rnd.Next(0, lastNames.Length - 1)];
    }

    private static List<Bug> GetRandomBugs(List<Developer> devs)
    {
        var descriptions = new string[]
        {
            "Nem működik a kód",
            "Nem működik a kód egy része",
            "Nem működik a kód egy másik része",
            "Nem működik a kód egy másik része se",
            "Nem működik a kód egy kicsi része se",
            "Nem működik a kód egy kicsi része",
            "Nem működik a kód egy nagy része",
            "Nem működik a render egy nagy része",
            "Nem működik a pre-render egy nagy része",
            "Nem működik a function egy nagy része",
            "Nem működik a metódus egy nagy része",
            "Nem működik a osztály egy nagy része",
            "Nem működik semmi",
            "Nem működik az életem",
            "Nem működik a gépem",
            "Nem működik a program"
        };

        var rnd = new Random();
        var bugs = new List<Bug>();
        foreach (var i in Enumerable.Range(0, 50))
        {
            bugs.Add(new Bug
            {
                Dev = devs[rnd.Next(0, devs.Count - 1)], Closed = rnd.Next() % 5 != 0,
                Description = descriptions[rnd.Next(0, descriptions.Length - 1)], Id = Guid.NewGuid()
            });
        }
        return bugs;
    }

    private static List<Fix> GetRandomFixes(List<Bug> bugs, List<Developer> devs)
    {
        var rnd = new Random();
        var fixes = new List<Fix>();
        
        foreach (var i in Enumerable.Range(0, 50))
        {
            fixes.Add(new Fix
                {Id = Guid.NewGuid(), Bug = bugs[rnd.Next(0, bugs.Count - 1)], Dev = devs[rnd.Next(0, devs.Count - 1)]});
        }
        return fixes;
    }

    private static List<Review> GetRandomReviews(List<Fix> fixes, List<Developer> devs)
    {
        var reviews = new List<Review>();
        var rnd = new Random();
        for (int i = 0; i < fixes.Count; i += 5)
        {
            reviews.Add(new Review
            {
                Fix = fixes[i], Approved = rnd.Next() % 5 != 0, Id = Guid.NewGuid(),
                Dev = devs[rnd.Next(0, devs.Count - 1)]
            });
        }

        return reviews;
    }
}