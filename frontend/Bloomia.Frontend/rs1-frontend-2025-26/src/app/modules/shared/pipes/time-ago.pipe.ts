import { Pipe, PipeTransform } from '@angular/core';

/**
 * Formats a date as a short relative "time ago" string (e.g. "5m ago", "2h ago"),
 * used for chat message timestamps instead of a raw shortTime display.
 * Falls back to a plain bs-BA formatted date for anything older than a week,
 * consistent with the app's central locale policy (see LOCALE_ID in app-module.ts).
 */
@Pipe({
  name: 'timeAgo',
  standalone: false
})
export class TimeAgoPipe implements PipeTransform {
  transform(value: string | Date | null | undefined): string {
    if (!value) {
      return '';
    }

    const date = this.toUtcDate(value);
    if (isNaN(date.getTime())) {
      return '';
    }

    const seconds = Math.floor((Date.now() - date.getTime()) / 1000);

    if (seconds < 5) {
      return 'just now';
    }
    if (seconds < 60) {
      return `${seconds}s ago`;
    }

    const minutes = Math.floor(seconds / 60);
    if (minutes < 60) {
      return `${minutes}m ago`;
    }

    const hours = Math.floor(minutes / 60);
    if (hours < 24) {
      return `${hours}h ago`;
    }

    const days = Math.floor(hours / 24);
    if (days < 7) {
      return `${days}d ago`;
    }

    return date.toLocaleDateString('bs-BA');
  }

  /**
   * The backend always sends SentAt as UTC (DateTime.UtcNow), but after an EF Core /
   * SQL Server round-trip DateTime.Kind can become "Unspecified", so ASP.NET Core's
   * JSON serializer sometimes omits the trailing "Z" (e.g. "2026-09-20T21:30:00"
   * instead of "2026-09-20T21:30:00Z"). The browser then parses a string with no
   * timezone marker as LOCAL time instead of UTC, which made a message sent seconds
   * ago show as "2h ago" for anyone outside the UTC+0 timezone. If the string has no
   * "Z"/offset suffix, treat it as UTC explicitly instead of trusting the browser's
   * default (local-time) interpretation.
   */
  private toUtcDate(value: string | Date): Date {
    if (value instanceof Date) {
      return value;
    }

    const hasTimezone = /Z$|[+-]\d{2}:\d{2}$/.test(value);
    const normalized = hasTimezone ? value : `${value}Z`;
    return new Date(normalized);
  }
}
