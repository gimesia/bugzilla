using System;
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
            
            context.Roles.AddRange(roles);
            context.SaveChanges();

            context.Developers.AddRange(developers);
            context.SaveChanges();

            var bugs = GetRandomBugs(context.Developers.ToList());

            context.Bugs.AddRange(bugs);
            context.SaveChanges();

            var fixes = GetRandomFixes(context.Bugs.ToList(), context.Developers.ToList());

            context.Fixes.AddRange(fixes);
            context.SaveChanges();

            var reviews = GetRandomReviews(context.Fixes.ToList(), context.Developers.ToList());

            context.Reviews.AddRange(reviews);
            context.SaveChanges();
        }
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
            "Racz",
            "Sztrepcsik",
            "Forro",
            "Rokus",
            "Erelyes",
            "Szundi",
            "Csizmadia",
            "Oltvany",
            "Redei",
            "Eross",
            "Hajnal"
        };
        var firstNames = new string[]
        {
            "Endre",
            "Hanga",
            "Pali",
            "Fiona",
            "Luis",
            "Lujza",
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
        return firstNames[rnd.Next(firstNames.Length)] + " " + lastNames[rnd.Next(lastNames.Length)];
    }

    private static List<Developer> GetRandomDevelopers(List<Role> roles)
    {
        var lead = roles[0];
        var devs = new List<Developer>();
        var rnd = new Random();

        foreach (var i in Enumerable.Range(1, 15))
        {
            devs.Add(new Developer
            {
                Id = Guid.NewGuid(),
                Name = GetRandomName(),
                Role = i == 1 ? lead : roles[rnd.Next(roles.Count)]
            });
        }

        return devs;
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
            "Nem működik a program",
            "Hiányzik egy karakter",
            "Hiányzik egy sor",
            "Hiányzik egy programrészlet",
            "Hiányzik egy osztály",
            "Hiányzik egy import",
            "Hiányzik egy export",
            "Hiányzik pár karakter",
            "Hiányzik pár sor",
            "Hiányzik pár programrészlet",
            "Hiányzik pár osztály",
            "Hiányzik pár import",
            "Hiányzik pár export",
            "Csúnya a kód",
            "Nagyon csúnya a kód"
        };

        var rnd = new Random();
        var bugs = new List<Bug>();
        foreach (var i in Enumerable.Range(0, 50))
        {
            bugs.Add(new Bug
            {
                Dev = devs[rnd.Next(0, devs.Count)], Closed = rnd.Next() % 3 == 0,
                Description = descriptions[rnd.Next(descriptions.Length)], Id = Guid.NewGuid()
            });
        }

        return bugs;
    }

    private static List<Fix> GetRandomFixes(List<Bug> bugs, List<Developer> devs)
    {
        var rnd = new Random();
        var fixes = new List<Fix>();

        foreach (var i in Enumerable.Range(0, 25))
        {
            fixes.Add(new Fix
                {Id = Guid.NewGuid(), Bug = bugs[rnd.Next(bugs.Count)], Dev = devs[rnd.Next(devs.Count)]});
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
                Fix = fixes[i], Approved = rnd.Next() % 2 == 0, Id = Guid.NewGuid(),
                Dev = devs[rnd.Next(devs.Count)]
            });
        }

        return reviews;
    }
}