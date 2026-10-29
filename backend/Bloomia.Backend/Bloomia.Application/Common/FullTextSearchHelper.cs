using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bloomia.Application.Common;

/// <summary>
/// Pretvara slobodan korisnicki unos (npr. "anksioznost san") u validan SQL Server
/// full-text CONTAINS izraz sa prefiksnom pretragom po svakoj rijeci
/// (npr. "\"anksioznost*\" OR \"san*\"").
/// Koristi se zajedno sa EF.Functions.Contains(kolona, izraz) nad kolonama koje
/// imaju kreiran FULLTEXT INDEX (vidi Bloomia.Infrastructure/Database/Scripts).
/// </summary>
public static class FullTextSearchHelper
{
    /// <summary>Vraca null ako nakon ciscenja nema nijedne validne rijeci za pretragu.</summary>
    public static string? BuildContainsExpression(string? rawSearch)
    {
        if (string.IsNullOrWhiteSpace(rawSearch))
            return null;

        // Full-text CONTAINS ima svoju sintaksu (", *, AND/OR...), zato uklanjamo
        // Remove all characters that are not letters or numbers from the user's input.
        var words = rawSearch
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(w => Regex.Replace(w, "[^\\p{L}\\p{Nd}]", ""))
            .Where(w => w.Length > 0)
            .Distinct()
            .ToArray();

        return words.Length == 0 ? null : string.Join(" OR ", words.Select(w => $"\"{w}*\""));
    }
}
