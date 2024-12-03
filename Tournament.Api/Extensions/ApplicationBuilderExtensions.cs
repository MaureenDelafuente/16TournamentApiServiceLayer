using Bogus;
using Microsoft.EntityFrameworkCore;
using Tournament.Core.Entities;
using Tournament.Data.Data;

namespace Tournament.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    // The "this" parameter adds an extension method to the class/interface that follows
    public static async Task SeedDataAsync(this IApplicationBuilder builder)
    {
        using (var scope = builder.ApplicationServices.CreateScope())
        {
            var serviceprovider = scope.ServiceProvider;
            var db = serviceprovider.GetRequiredService<TournamentApiContext>();

            await db.Database.MigrateAsync();
            if (await db.TournamentDetails.AnyAsync()) return;

            try
            {
                var tournaments = GenerateTournaments(4);
                db.AddRange(tournaments);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw; // TODO: handle somehow?
            }
        }
    }

    private static IEnumerable<TournamentDetails> GenerateTournaments(int nrOfTournaments)
    {
        var faker = new Faker<TournamentDetails>("sv").Rules((f, t) =>
            {
                t.Title = $"{f.Address.Country()} {f.Hacker.IngVerb()} Championship";
                t.StartDate = f.Date.Soon(f.Random.Int(1, 14));
                t.Games = GenerateGames(f.Random.Int(2,7), t.Title, t.StartDate);
            }
        );

        return faker.Generate(nrOfTournaments);
    }

    private static ICollection<Game> GenerateGames(int nrOfGames, string tournamentTitle, DateTime tournamentStart)
    {
        var faker = new Faker<Game>("sv").Rules((f, g) =>
        {
            g.Title = $"{tournamentTitle} {f.Commerce.Color()}";
            g.Time = f.Date.Soon(f.Random.Int(1,5),tournamentStart);
        });

        return faker.Generate(nrOfGames);
    }
}