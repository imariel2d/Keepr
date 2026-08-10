using System.Net;
using System.Text;

namespace Keepr.Api.Features.Email;

/// <summary>A rendered email: subject plus both body representations.</summary>
public sealed record EmailContent(string Subject, string HtmlBody, string TextBody);

/// <summary>
/// Server-side email rendering, Cove's look re-expressed with email-safe primitives. Mail clients
/// (Outlook especially) don't support CSS variables, flexbox/grid, external webfonts, or
/// <c>@media(prefers-color-scheme)</c> reliably, so this does <b>not</b> import the app's CSS — it
/// hand-writes a table layout with inlined literal hex colors (the actual values from
/// <c>tokens.css</c>) and a web-safe font stack. A plain-<c>StringBuilder</c> templater rather than
/// Razor: three fields don't warrant the dependency. See docs/feature-36-account-provisioning.md §10.
/// </summary>
public static class EmailTemplates
{
    // Cove palette, literal (custom properties don't resolve in mail clients). See tokens.css.
    private const string Paper = "#FBF9F6";     // --gray-25  page canvas
    private const string Card = "#FFFFFF";      // --surface-card
    private const string BorderSubtle = "#EDE7DD"; // --gray-100
    private const string Ink = "#221D18";       // --gray-900 text-primary
    private const string InkSoft = "#695D4E";   // --gray-600 text-secondary
    private const string InkFaint = "#A3937A";  // --gray-400 text-tertiary
    private const string Terracotta = "#E8703A"; // --brand-500 accent
    private const string TerracottaDark = "#B04519"; // --brand-700 (button border for depth)

    // Web-safe cousins of Sora/Manrope — mail clients drop the Google Fonts import.
    private const string FontStack =
        "-apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Helvetica, Arial, sans-serif";

    /// <summary>The invite email: "you've been invited", a Set-your-password button to the claim
    /// link, the raw link as a fallback, and the expiry. <paramref name="invitedBy"/> is the inviter's
    /// display name (their first/last name), not their email — when it's null/blank the template uses
    /// a generic line rather than exposing any address.</summary>
    public static EmailContent Invite(string claimUrl, string? invitedBy, int expiryDays, string? locale = null)
    {
        var s = EmailStrings.For(locale);
        var by = string.IsNullOrWhiteSpace(invitedBy) ? null : invitedBy;
        var expiry = s.FormatDays(expiryDays);

        var intro = by is null ? s.InviteIntroGeneric : string.Format(s.InviteIntroByFormat, by);

        var bodyHtml = Paragraph(intro) + Paragraph(s.InviteBody2);

        var html = Layout(s,
            preheader: s.InvitePreheader,
            headline: s.InviteSubject,
            bodyHtml: bodyHtml,
            ctaText: s.InviteCta,
            ctaUrl: claimUrl,
            footerNote: string.Format(s.ExpiresFooterFormat, expiry));

        var text = new StringBuilder()
            .AppendLine(s.InviteSubject)
            .AppendLine()
            .AppendLine(intro)
            .AppendLine()
            .AppendLine(s.InviteBody2Text)
            .AppendLine(claimUrl)
            .AppendLine()
            .AppendLine(string.Format(s.ExpiresTextFormat, expiry))
            .ToString();

        return new EmailContent(s.InviteSubject, html, text);
    }

    /// <summary>The password-reset email: a Choose-a-new-password button to the reset link, the raw
    /// link as a fallback, the expiry, and a "you can ignore this" reassurance. Carries no account
    /// detail beyond the link itself. <paramref name="expiryMinutes"/> is the link lifetime (§4).</summary>
    public static EmailContent PasswordReset(string resetUrl, int expiryMinutes, string? locale = null)
    {
        var s = EmailStrings.For(locale);
        var minutes = s.FormatMinutesOnly(expiryMinutes);

        var bodyHtml = Paragraph(s.ResetBody1) + Paragraph(s.ResetBody2);

        var html = Layout(s,
            preheader: s.ResetPreheader,
            headline: s.ResetHeadline,
            bodyHtml: bodyHtml,
            ctaText: s.ResetCta,
            ctaUrl: resetUrl,
            footerNote: string.Format(s.ExpiresFooterFormat, minutes));

        var text = new StringBuilder()
            .AppendLine(s.ResetSubject)
            .AppendLine()
            .AppendLine(s.ResetBody1)
            .AppendLine(s.ResetBody1Text)
            .AppendLine(resetUrl)
            .AppendLine()
            .AppendLine(string.Format(s.ExpiresTextFormat, minutes))
            .AppendLine(s.ResetIgnoreText)
            .ToString();

        return new EmailContent(s.ResetSubject, html, text);
    }

    /// <summary>The change-email confirmation, sent to the <b>new</b> address: a Confirm button to the
    /// confirmation link, the raw link as a fallback, the expiry, and a "you can ignore this" line.
    /// Carries no account detail beyond the link itself. <paramref name="expiryMinutes"/> is the link
    /// lifetime (§5.5); rendered as hours when it divides evenly.</summary>
    public static EmailContent ConfirmEmailChange(string confirmUrl, int expiryMinutes, string? locale = null)
    {
        var s = EmailStrings.For(locale);
        var expiry = s.FormatMinutes(expiryMinutes);

        var bodyHtml = Paragraph(s.ConfirmBody1) + Paragraph(s.ConfirmBody2);

        var html = Layout(s,
            preheader: s.ConfirmPreheader,
            headline: s.ConfirmHeadline,
            bodyHtml: bodyHtml,
            ctaText: s.ConfirmCta,
            ctaUrl: confirmUrl,
            footerNote: string.Format(s.ExpiresFooterFormat, expiry));

        var text = new StringBuilder()
            .AppendLine(s.ConfirmSubject)
            .AppendLine()
            .AppendLine(s.ConfirmBody1Text)
            .AppendLine(confirmUrl)
            .AppendLine()
            .AppendLine(string.Format(s.ExpiresTextFormat, expiry))
            .AppendLine(s.ConfirmIgnoreText)
            .ToString();

        return new EmailContent(s.ConfirmSubject, html, text);
    }

    /// <summary>The heads-up sent to the <b>old</b> address once an email change completes, so the
    /// original owner can react if it wasn't them. No CTA. <paramref name="newEmailMasked"/> is the new
    /// address partially masked (e.g. <c>n•••@example.com</c>) so the old inbox doesn't spell out the
    /// full new address. See docs/feature-27-change-email.md §11.</summary>
    public static EmailContent EmailChanged(string newEmailMasked, string? locale = null)
    {
        var s = EmailStrings.For(locale);
        var body1 = string.Format(s.ChangedBody1Format, newEmailMasked);

        var bodyHtml = Paragraph(body1) + Paragraph(s.ChangedBody2);

        var html = Layout(s,
            preheader: s.ChangedPreheader,
            headline: s.ChangedSubject,
            bodyHtml: bodyHtml,
            ctaText: null,
            ctaUrl: null,
            footerNote: null);

        var text = new StringBuilder()
            .AppendLine(s.ChangedSubject)
            .AppendLine()
            .AppendLine(body1)
            .AppendLine(s.ChangedBody2)
            .ToString();

        return new EmailContent(s.ChangedSubject, html, text);
    }

    /// <summary>
    /// The shared Cove-styled shell: centered ~600px card on the paper canvas, wordmark, headline,
    /// body, a bulletproof button, and a footer with the raw link. A dark-mode block is included as
    /// progressive enhancement (Apple/iOS Mail honor it); the light design is the standalone baseline.
    /// </summary>
    private static string Layout(
        EmailStrings s, string preheader, string headline, string bodyHtml,
        string? ctaText, string? ctaUrl, string? footerNote)
    {
        // The CTA button and the footer's raw-link line render only for emails that carry a link
        // (invite / reset / confirm). A link-less notice — the email-changed heads-up — omits both and
        // its footer note, leaving just the headline, body, and the standard "weren't expecting this" line.
        var ctaBlock = string.IsNullOrEmpty(ctaUrl) || string.IsNullOrEmpty(ctaText) ? "" : $$"""
              <table role="presentation" cellpadding="0" cellspacing="0" style="margin:28px 0 8px 0;">
                <tr>
                  <td align="center" bgcolor="{{Terracotta}}" style="border-radius:10px; background:{{Terracotta}};">
                    <a href="{{HtmlAttr(ctaUrl)}}" target="_blank" style="display:inline-block; padding:14px 28px; font-family:{{FontStack}}; font-size:16px; font-weight:700; color:#FFFFFF; text-decoration:none; border-radius:10px; border:1px solid {{TerracottaDark}};">{{HtmlText(ctaText)}}</a>
                  </td>
                </tr>
              </table>
""";

        var footerNoteLine = string.IsNullOrEmpty(footerNote) ? "" :
            $"""<p class="kp-faint" style="margin:0 0 8px 0; font-family:{FontStack}; font-size:13px; line-height:1.5; color:{InkFaint};">{HtmlText(footerNote)}</p>""";

        var footerLink = string.IsNullOrEmpty(ctaUrl) ? "" :
            $"""<p style="margin:0 0 16px 0; font-family:{FontStack}; font-size:13px; line-height:1.5; word-break:break-all;"><a href="{HtmlAttr(ctaUrl)}" target="_blank" style="color:{Terracotta}; text-decoration:underline;">{HtmlText(ctaUrl)}</a></p>""";

        return $$"""
<!DOCTYPE html>
<html lang="{{s.LangTag}}">
<head>
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <meta name="color-scheme" content="light dark">
  <title>{{HtmlText(headline)}}</title>
  <style>
    @media (prefers-color-scheme: dark) {
      .kp-body { background: #16120F !important; }
      .kp-card { background: #362F28 !important; border-color: #4A4038 !important; }
      .kp-ink { color: #F4EFE7 !important; }
      .kp-ink-soft { color: #C4B8A2 !important; }
      .kp-faint { color: #857562 !important; }
    }
  </style>
</head>
<body class="kp-body" style="margin:0; padding:0; background:{{Paper}};">
  <!-- preheader: shown in the inbox preview, hidden in the body -->
  <div style="display:none; max-height:0; overflow:hidden; opacity:0;">{{HtmlText(preheader)}}</div>
  <table role="presentation" width="100%" cellpadding="0" cellspacing="0" style="background:{{Paper}};">
    <tr>
      <td align="center" style="padding:32px 16px;">
        <table role="presentation" width="600" cellpadding="0" cellspacing="0" style="width:600px; max-width:100%;">
          <tr>
            <td style="padding:8px 4px 20px 4px;">
              <span style="font-family:{{FontStack}}; font-size:22px; font-weight:800; letter-spacing:-0.01em; color:{{Terracotta}};">Keepr</span>
            </td>
          </tr>
          <tr>
            <td class="kp-card" style="background:{{Card}}; border:1px solid {{BorderSubtle}}; border-radius:14px; padding:36px 36px 32px 36px;">
              <h1 class="kp-ink" style="margin:0 0 16px 0; font-family:{{FontStack}}; font-size:26px; line-height:1.2; font-weight:700; letter-spacing:-0.01em; color:{{Ink}};">{{HtmlText(headline)}}</h1>
              {{bodyHtml}}
              {{ctaBlock}}
            </td>
          </tr>
          <tr>
            <td style="padding:20px 8px 8px 8px;">
              {{footerNoteLine}}
              {{footerLink}}
              <p class="kp-faint" style="margin:0; font-family:{{FontStack}}; font-size:12px; line-height:1.5; color:{{InkFaint}};">{{HtmlText(s.IgnoreFooter)}}</p>
            </td>
          </tr>
        </table>
      </td>
    </tr>
  </table>
</body>
</html>
""";
    }

    private static string Paragraph(string text) =>
        $"""<p class="kp-ink-soft" style="margin:0 0 14px 0; font-family:{FontStack}; font-size:16px; line-height:1.6; color:{InkSoft};">{HtmlText(text)}</p>""";

    // Body/attribute text comes from user-controlled data (invitee, inviter email), so encode it.
    private static string HtmlText(string value) => WebUtility.HtmlEncode(value);

    // Attribute context (href): encode quotes/angle brackets so the value can't break out of the
    // attribute. The claim URL we build is already URL-safe, but defense in depth is free.
    private static string HtmlAttr(string value) =>
        WebUtility.HtmlEncode(value).Replace("\"", "&quot;");
}
