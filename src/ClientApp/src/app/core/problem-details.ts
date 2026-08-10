// Readers for the RFC 7807 problem+json bodies the API returns on error. Centralized here so every
// feature screen parses the same server contract the same way, and a shape change lands in one
// place. See docs/feature-36-account-provisioning.md §12.

import { MIN_PASSWORD_LENGTH } from './password-policy';

/** The user-facing `detail` string, or a fallback when the error carries none. */
export function problemDetail(e: unknown, fallback: string): string {
  const detail = (e as { error?: { detail?: string } })?.error?.detail;
  return typeof detail === 'string' && detail ? detail : fallback;
}

/** The HTTP status of a failed HttpClient call, if present. */
export function problemStatus(e: unknown): number | undefined {
  return (e as { status?: number })?.status;
}

/** A machine-readable `code` extension member, when the server tags a problem with one (e.g.
 *  distinguishing two different 409s). */
export function problemCode(e: unknown): string | undefined {
  const code = (e as { error?: { code?: string } })?.error?.code;
  return typeof code === 'string' && code ? code : undefined;
}

/** The per-field validation map from a 400, keyed by field name; empty when the error isn't one. */
export function validationErrors(e: unknown): Record<string, string[]> {
  const errors = (e as { error?: { errors?: Record<string, string[]> } })?.error?.errors;
  return errors && typeof errors === 'object' ? errors : {};
}

// Localized copy for the per-field validation codes the credential validators emit (#30 §5.3).
// A field `errors` entry is a stable code (e.g. `password_too_short`); this maps it to translated
// text. Values that aren't known codes (framework validators, the email-settings screen) pass
// through unchanged, so `fieldErrors` is safe to use for every validation response.
const FIELD_MESSAGES: Record<string, () => string> = {
  email_malformed: () => $localize`:@@fields.email_malformed:Enter a valid email address.`,
  email_disposable: () =>
    $localize`:@@fields.email_disposable:That email provider isn't supported. Use a permanent address.`,
  password_too_short: () =>
    $localize`:@@fields.password_too_short:Use at least ${MIN_PASSWORD_LENGTH}:count: characters.`,
  password_too_long: () =>
    $localize`:@@fields.password_too_long:That password is too long. Keep it under 72 bytes (about 72 characters).`,
  password_contains_email: () =>
    $localize`:@@fields.password_contains_email:Don't use your email address in your password.`,
  password_breached: () =>
    $localize`:@@fields.password_breached:This password has appeared in a data breach. Choose another.`,
};

/**
 * The per-field validation map with each known code translated to the user's language, and any
 * unrecognized value passed through as-is. Prefer this over `validationErrors` anywhere the messages
 * are shown to a user (#30 §5.3).
 */
export function fieldErrors(e: unknown): Record<string, string[]> {
  const raw = validationErrors(e);
  const out: Record<string, string[]> = {};
  for (const [field, values] of Object.entries(raw)) {
    out[field] = values.map((v) => FIELD_MESSAGES[v]?.() ?? v);
  }
  return out;
}

// Localized copy for the server's stable error `code`s (#30). Each entry is a `$localize` string,
// translated in es/fr like any UI copy. The server owns the code (src/Api/Http/ErrorCodes.cs) and
// keeps the English `detail` as the fallback; this map lets the client render the message in the
// user's language instead. Grows as screens are localized — a code with no entry falls back to the
// server `detail`, never a blank or a raw code. See docs/feature-30-localization.md §5.2.
const ERROR_MESSAGES: Record<string, () => string> = {
  invalid_credentials: () => $localize`:@@errors.invalid_credentials:Invalid credentials.`,
  email_registered: () => $localize`:@@errors.email_registered:Email already registered.`,
  registration_closed: () =>
    $localize`:@@errors.registration_closed:Registration is closed. Ask an admin to set up an account.`,
  password_incorrect: () =>
    $localize`:@@errors.password_incorrect:Your current password is incorrect.`,
  invalid_language: () => $localize`:@@errors.invalid_language:That isn't a supported language.`,
  share_not_found: () => $localize`:@@errors.share_not_found:This share link doesn't exist.`,
  share_unavailable: () =>
    $localize`:@@errors.share_unavailable:This share link is no longer available.`,
};

/**
 * The localized, user-facing message for a failed call: the server's stable `code` mapped to
 * translated copy, falling back to the server's English `detail`, then to a generic message. Prefer
 * this over `problemDetail` anywhere the message is shown to a user (#30 §5.2).
 */
export function errorMessage(e: unknown): string {
  const code = problemCode(e);
  const mapped = code ? ERROR_MESSAGES[code] : undefined;
  if (mapped) return mapped();
  return problemDetail(e, $localize`:@@errors.generic:Something went wrong. Please try again.`);
}
