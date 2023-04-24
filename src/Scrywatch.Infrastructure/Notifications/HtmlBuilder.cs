using Scrywatch.Core.Interests;
using System.Text;

namespace Scrywatch.Infrastructure.Notifications;

public sealed class HtmlBuilder
{
    private StringBuilder Builder { get; set; }

    private const string Head = @"
    <head>
        <meta charset=""utf-8"">
        <title>Notification</title>
        <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
        <style>
            html { height: fit-content; }
            body {
                overflow: hidden;
                background: linear-gradient(to bottom, #1d1c25, #431e3f);
                color: white;
                margin: 0;
                height: fit-content;
            }
            main {
                display: flex;
                flex-direction: column;
                gap: 2rem;
                padding: 1rem;
            }
            .card {
                display: flex;
            }
            .card-img {
                border-radius: 30px;
                margin: 0;
                width: 300px;
                height: 430px;
            }
            .card-info {
                display: flex;
                flex-direction: column;
            }
            .price {
                display: flex;
                justify-content: space-evenly;
            }
            .price h2 {
                font-size: 1rem;
            }
            .chart {
                min-width: 500px;
            }
        </style>
    </head>";

    public HtmlBuilder(IEnumerable<Interest> interests)
    {
        Builder = new StringBuilder("<!DOCTYPE html>");
        Builder.AppendLine("<html>");
        Builder.AppendLine(Head);
        Builder.AppendLine("<body>");
        Builder.AppendLine("<main>");

        foreach(var interest in interests)
        {
            var chart = new Chart(interest);
            Builder.AppendLine(@"<div class=""card"">");
            Builder.AppendLine($"<img class=\"card-img\" src=\"{interest.Card.Face.Front}\">");
            Builder.AppendLine($"<img class=\"chart\" src=\"{chart.Url}\">");
            Builder.AppendLine("</div>");
        }

        Builder.AppendLine("</main>");
        Builder.AppendLine("</body>");
        Builder.AppendLine("</html>");
    }

    public override string ToString() => Builder.ToString();
}
