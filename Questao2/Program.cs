using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main(string[] args)
    {
        await RunGoals();
    }

    public static async Task RunGoals()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = await getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = await getTotalScoredGoals(teamName, year);
        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);
    }

    public static async Task<int> getTotalScoredGoals(string team, int year)
    {
        int totalGoals = 0;
        totalGoals = await getTotalGoalsByTeam(team, year, 1, totalGoals);
        totalGoals = await getTotalGoalsByTeam(team, year, 2, totalGoals);

        return totalGoals;
    }

    private static async Task<int> getTotalGoalsByTeam(string team, int year, int teamNumber, int totalGoals)
    {
        using HttpClient client = new HttpClient();

        string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team{teamNumber}={team}&page=1";
        
        try
        {
            string responseBody = await client.GetStringAsync(url);
            JObject json = JObject.Parse(responseBody);

            totalGoals = CalculateTotalGoals(team, totalGoals, json);

            int totalPages = (int)json["total_pages"];

            if (totalPages > 1)
            {
                for (int page = 2; page <= totalPages; page++)
                {
                    string pageUrl = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team{teamNumber}={team}&page={page}";
                    responseBody = await client.GetStringAsync(pageUrl);
                    json = JObject.Parse(responseBody);

                    totalGoals = CalculateTotalGoals(team, totalGoals, json);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }

        return totalGoals;
    }

    private static int CalculateTotalGoals(string team, int totalGoals, JObject json)
    {
        if (json["data"] != null && json["data"].Type == JTokenType.Array)
        {
            foreach (var match in json["data"])
            {
                if (match["team1"].ToString() == team)
                {
                    totalGoals += int.Parse(match["team1goals"].ToString());
                }
                else if (match["team2"].ToString() == team)
                {
                    totalGoals += int.Parse(match["team2goals"].ToString());
                }
            }
        }

        return totalGoals;
    }
}
