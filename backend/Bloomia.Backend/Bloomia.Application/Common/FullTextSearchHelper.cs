using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bloomia.Application.Common;

/// <summary>
/// Converts free user input (e.g. "anxiety sleep") into a valid SQL Server
/// full-text CONTAINS expression with a prefix search on every word
/// (e.g. "\"anxiety*\" OR \"sleep*\"").
/// Used together with EF.Functions.Contains(column, expression) on columns that
/// have a FULLTEXT INDEX (see Bloomia.Infrastructure/Database/Scripts).
/// </summary>
public static class FullTextSearchHelper
{
    /// <summary>Returns null if no valid search word remains after cleaning.</summary>
    public static string? BuildContainsExpression(string? rawSearch)
    {
        if (string.IsNullOrWhiteSpace(rawSearch))
            return null;

        // Full-text CONTAINS has its own syntax (", *, AND/OR...), so remove all
        // characters that are not letters or numbers from the user's input.
        var words = rawSearch
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(w => Regex.Replace(w, "[^\\p{L}\\p{Nd}]", ""))
            .Where(w => w.Length > 0)
            .Distinct()
            .ToArray();

        return words.Length == 0 ? null : string.Join(" OR ", words.Select(w => $"\"{w}*\""));
    }
}
