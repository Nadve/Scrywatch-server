using Scrywatch.Core.Interests;
using Scrywatch.Core.ValueObjects;
using System.Text.Json;

namespace Scrywatch.Infrastructure.Notifications;

public sealed class Chart
{
    private const string BaseUrl = "https://quickchart.io/chart?c=";

    public string Url { get; init; }

    public Currency Currency { get; init; }

    public int Goal { get; init; }

    public ICollection<DateOnly> Labels { get; init; }

    public ICollection<float> Data { get; init; }

    public float Price => Data.Last();

    public double Average { get; init; }

    public double GoalValue { get; init; }

    public int Trend => (int)Math.Round((Price - Average) / Average * 100);

    public Chart(Interest interest)
    {
        Goal = interest.Goal;

        var prices = interest.Card.Prices.First();
        Currency = prices.Currency;

        var dateValuePairs = prices.DateValuePairs;

        // Fill in missing dates
        Labels = new List<DateOnly>();
        var keys = dateValuePairs.Keys;
        for(var startDate = keys.First(); startDate <= keys.Last(); startDate = startDate.AddDays(1))
        {
            Labels.Add(startDate);
        }

        // Fill in missing values by copying previous one
        Data = new List<float>();
        var values = dateValuePairs.Values;
        float prevValue = 0;
        for(int i = 0, j = 0; i < Labels.Count; i++)
        {
            if (j < keys.Count &&
                keys.ElementAt(j).Equals(Labels.ElementAt(i)))
            {
                prevValue = values.ElementAt(j);
                j++;
            }
            Data.Add(prevValue);
        }

        float goal = Goal;
        Average = Math.Round(Data.Aggregate((p, n) => p + n) / Data.Count, 2);
        GoalValue = Average + goal / 100 * Average;

        Labels = Labels.TakeLast(100).ToList();
        Data = Data.TakeLast(100).ToList();
        Url = BaseUrl + Uri.EscapeDataString(Configuration);
    }

    private static double?[] ConstantLine(int length, double value)
    {
        double?[] line = new double?[length];
        line[0] = value;
        for(int i = 1; i < length; i++)
        {
            line[i] = null;
        }
        line[length - 1] = value;
        return line;
    }

    private string Configuration => JsonSerializer.Serialize(new
    {
        type = "line",
        data = new
        {
            labels = Labels,
            datasets = new dynamic[]
            {
                new {
                    label = $"Price: {Price}{Currency.Symbol}",
                    data = Data,
                    lineTension = 0.3,
                    pointRadius = 0.1,
                    borderWidth = 2
                },
                new
                {
                    label = $"Goal: {Goal}%",
                    data = ConstantLine(Data.Count, GoalValue),
                    spanGaps = true,
                    fill = 3,
                    pointRadius = 0.1,
                    borderWidth = 1.5
                },
                new
                {
                    label = $"Trend: {Trend}%",
                    data = ConstantLine(Data.Count, Price),
                    spanGaps = true,
                    fill = 3,
                    pointRadius = 0.1,
                    borderWidth = 1.5
                },
                new
                {
                    label = $"Average: {Average}{Currency.Symbol}",
                    data = ConstantLine(Data.Count, Average),
                    spanGaps = true,
                    fill = false,
                    pointRadius = 0.1,
                    borderWidth = 1.5
                }
            }
        },
        options = new {
            legend = new
            {
                labels = new
                {
                    fontColor = "#ffffffd4",
                    usePointStyle = true,
                    pointStyle = "rectRounded"
                }
            },
            scales = new
            {
                yAxes = new dynamic[]
                {
                    new
                    {
                        ticks = new
                        {
                            fontColor = "#ffffffd4"
                        },
                        gridLines = new
                        {
                            color = "#ffffff61"
                        }
                    }
                },
                xAxes = new dynamic[]
                {
                    new
                    {
                        ticks = new
                        {
                            fontColor = "#ffffffd4"
                        },
                        gridLines = new
                        {
                            display = false
                        }
                    }
                }
            }
        }
    });
}
