namespace Keepr.Api.Features.Email;

/// <summary>
/// Localized copy for the outbound emails (#30 Phase 3). One instance per supported locale, selected
/// by the recipient's preferred language via <see cref="For"/>; English is the default and the
/// fallback for any unknown/unset locale. Kept as plain fields + two expiry formatters rather than
/// resx — matching <see cref="EmailTemplates"/>'s dependency-light approach. The English instance's
/// values are byte-identical to the old hardcoded strings, so the default-locale output is unchanged.
/// Interpolations use <c>{0}</c>.
/// </summary>
public sealed class EmailStrings
{
    // ---- shared ----
    public required string LangTag;               // <html lang="…">
    public required string IgnoreFooter;          // footer boilerplate on every email
    public required string ExpiresFooterFormat;   // "This link expires in {0}. If the button…"
    public required string ExpiresTextFormat;     // text-body "This link expires in {0}."
    public required string Day;
    public required string Days;
    public required string Minute;
    public required string Minutes;
    public required string Hour;
    public required string Hours;

    // ---- invite ----
    public required string InviteSubject;
    public required string InvitePreheader;
    public required string InviteIntroGeneric;
    public required string InviteIntroByFormat;   // "{0} has invited you to Keepr…"
    public required string InviteBody2;           // HTML paragraph
    public required string InviteBody2Text;       // text line (ends with ':')
    public required string InviteCta;

    // ---- password reset ----
    public required string ResetSubject;
    public required string ResetHeadline;
    public required string ResetPreheader;
    public required string ResetBody1;
    public required string ResetBody2;            // HTML paragraph (incl. "you can ignore…")
    public required string ResetBody1Text;        // text line (ends with ':')
    public required string ResetCta;
    public required string ResetIgnoreText;       // text-body ignore reassurance

    // ---- confirm email change ----
    public required string ConfirmSubject;
    public required string ConfirmHeadline;
    public required string ConfirmPreheader;
    public required string ConfirmBody1;          // HTML paragraph
    public required string ConfirmBody2;          // HTML paragraph ("you can ignore…")
    public required string ConfirmBody1Text;      // text line (ends with ':')
    public required string ConfirmCta;
    public required string ConfirmIgnoreText;

    // ---- email changed (heads-up to the old address) ----
    public required string ChangedSubject;
    public required string ChangedPreheader;
    public required string ChangedBody1Format;    // "…was changed to {0}."
    public required string ChangedBody2;          // "If this wasn't you…"

    /// <summary>"1 day" / "7 days", localized.</summary>
    public string FormatDays(int days) => $"{days} {(days == 1 ? Day : Days)}";

    /// <summary>"1 minute" / "60 minutes", localized — always minutes, never rolled up to hours
    /// (the reset link's lifetime is short and shown as-is).</summary>
    public string FormatMinutesOnly(int minutes) => $"{minutes} {(minutes == 1 ? Minute : Minutes)}";

    /// <summary>"1 minute" / "90 minutes" / "1 hour" / "24 hours", localized — hours when the minutes
    /// divide evenly.</summary>
    public string FormatMinutes(int minutes)
    {
        if (minutes % 60 == 0)
        {
            var hours = minutes / 60;
            return $"{hours} {(hours == 1 ? Hour : Hours)}";
        }
        return $"{minutes} {(minutes == 1 ? Minute : Minutes)}";
    }

    /// <summary>The copy for a recipient's preferred language; English for null/unknown/unset.</summary>
    public static EmailStrings For(string? locale) => locale switch
    {
        "es" => Es,
        "fr" => Fr,
        _ => En,
    };

    public static readonly EmailStrings En = new()
    {
        LangTag = "en",
        IgnoreFooter = "If you weren't expecting this, you can ignore this email.",
        ExpiresFooterFormat = "This link expires in {0}. If the button doesn't work, paste this address into your browser:",
        ExpiresTextFormat = "This link expires in {0}.",
        Day = "day", Days = "days", Minute = "minute", Minutes = "minutes", Hour = "hour", Hours = "hours",

        InviteSubject = "You're invited to Keepr",
        InvitePreheader = "Set your password to activate your Keepr account.",
        InviteIntroGeneric = "You've been invited to Keepr, a private place to keep your files.",
        InviteIntroByFormat = "{0} has invited you to Keepr, a private place to keep your files.",
        InviteBody2 = "Set a password to activate your account and sign in.",
        InviteBody2Text = "Set a password to activate your account and sign in:",
        InviteCta = "Set your password",

        ResetSubject = "Reset your Keepr password",
        ResetHeadline = "Reset your password",
        ResetPreheader = "Choose a new password for your Keepr account.",
        ResetBody1 = "We received a request to reset the password on your Keepr account.",
        ResetBody2 = "Choose a new password to finish. If you didn't ask for this, you can ignore this email — your password won't change.",
        ResetBody1Text = "Choose a new password to finish:",
        ResetCta = "Choose a new password",
        ResetIgnoreText = "If you didn't request this, you can ignore this email — your password won't change.",

        ConfirmSubject = "Confirm your new Keepr email",
        ConfirmHeadline = "Confirm your new email",
        ConfirmPreheader = "Confirm your new email for your Keepr account.",
        ConfirmBody1 = "Confirm this address to start using it to sign in to Keepr.",
        ConfirmBody2 = "If you didn't request this, you can ignore this email — nothing will change.",
        ConfirmBody1Text = "Confirm this address to start using it to sign in to Keepr:",
        ConfirmCta = "Confirm this email",
        ConfirmIgnoreText = "If you didn't request this, you can ignore this email — nothing will change.",

        ChangedSubject = "Your Keepr email was changed",
        ChangedPreheader = "The email address on your Keepr account was changed.",
        ChangedBody1Format = "The email address on your Keepr account was changed to {0}.",
        ChangedBody2 = "If this wasn't you, contact your admin right away.",
    };

    public static readonly EmailStrings Es = new()
    {
        LangTag = "es",
        IgnoreFooter = "Si no esperabas esto, puedes ignorar este correo.",
        ExpiresFooterFormat = "Este enlace caduca en {0}. Si el botón no funciona, pega esta dirección en tu navegador:",
        ExpiresTextFormat = "Este enlace caduca en {0}.",
        Day = "día", Days = "días", Minute = "minuto", Minutes = "minutos", Hour = "hora", Hours = "horas",

        InviteSubject = "Te han invitado a Keepr",
        InvitePreheader = "Establece tu contraseña para activar tu cuenta de Keepr.",
        InviteIntroGeneric = "Te han invitado a Keepr, un lugar privado para guardar tus archivos.",
        InviteIntroByFormat = "{0} te ha invitado a Keepr, un lugar privado para guardar tus archivos.",
        InviteBody2 = "Establece una contraseña para activar tu cuenta e iniciar sesión.",
        InviteBody2Text = "Establece una contraseña para activar tu cuenta e iniciar sesión:",
        InviteCta = "Establecer contraseña",

        ResetSubject = "Restablece tu contraseña de Keepr",
        ResetHeadline = "Restablece tu contraseña",
        ResetPreheader = "Elige una nueva contraseña para tu cuenta de Keepr.",
        ResetBody1 = "Recibimos una solicitud para restablecer la contraseña de tu cuenta de Keepr.",
        ResetBody2 = "Elige una nueva contraseña para terminar. Si no lo solicitaste, puedes ignorar este correo: tu contraseña no cambiará.",
        ResetBody1Text = "Elige una nueva contraseña para terminar:",
        ResetCta = "Elegir una nueva contraseña",
        ResetIgnoreText = "Si no lo solicitaste, puedes ignorar este correo: tu contraseña no cambiará.",

        ConfirmSubject = "Confirma tu nuevo correo de Keepr",
        ConfirmHeadline = "Confirma tu nuevo correo electrónico",
        ConfirmPreheader = "Confirma tu nuevo correo electrónico para tu cuenta de Keepr.",
        ConfirmBody1 = "Confirma esta dirección para empezar a usarla para iniciar sesión en Keepr.",
        ConfirmBody2 = "Si no lo solicitaste, puedes ignorar este correo: nada cambiará.",
        ConfirmBody1Text = "Confirma esta dirección para empezar a usarla para iniciar sesión en Keepr:",
        ConfirmCta = "Confirmar este correo",
        ConfirmIgnoreText = "Si no lo solicitaste, puedes ignorar este correo: nada cambiará.",

        ChangedSubject = "Se cambió tu correo de Keepr",
        ChangedPreheader = "La dirección de correo de tu cuenta de Keepr fue cambiada.",
        ChangedBody1Format = "La dirección de correo de tu cuenta de Keepr se cambió a {0}.",
        ChangedBody2 = "Si no fuiste tú, contacta a tu administrador de inmediato.",
    };

    public static readonly EmailStrings Fr = new()
    {
        LangTag = "fr",
        IgnoreFooter = "Si vous n'attendiez pas cet e-mail, vous pouvez l'ignorer.",
        ExpiresFooterFormat = "Ce lien expire dans {0}. Si le bouton ne fonctionne pas, collez cette adresse dans votre navigateur :",
        ExpiresTextFormat = "Ce lien expire dans {0}.",
        Day = "jour", Days = "jours", Minute = "minute", Minutes = "minutes", Hour = "heure", Hours = "heures",

        InviteSubject = "Vous êtes invité sur Keepr",
        InvitePreheader = "Définissez votre mot de passe pour activer votre compte Keepr.",
        InviteIntroGeneric = "Vous avez été invité sur Keepr, un espace privé pour conserver vos fichiers.",
        InviteIntroByFormat = "{0} vous a invité sur Keepr, un espace privé pour conserver vos fichiers.",
        InviteBody2 = "Définissez un mot de passe pour activer votre compte et vous connecter.",
        InviteBody2Text = "Définissez un mot de passe pour activer votre compte et vous connecter :",
        InviteCta = "Définir votre mot de passe",

        ResetSubject = "Réinitialisez votre mot de passe Keepr",
        ResetHeadline = "Réinitialisez votre mot de passe",
        ResetPreheader = "Choisissez un nouveau mot de passe pour votre compte Keepr.",
        ResetBody1 = "Nous avons reçu une demande de réinitialisation du mot de passe de votre compte Keepr.",
        ResetBody2 = "Choisissez un nouveau mot de passe pour terminer. Si vous n'êtes pas à l'origine de cette demande, ignorez cet e-mail — votre mot de passe ne changera pas.",
        ResetBody1Text = "Choisissez un nouveau mot de passe pour terminer :",
        ResetCta = "Choisir un nouveau mot de passe",
        ResetIgnoreText = "Si vous n'êtes pas à l'origine de cette demande, ignorez cet e-mail — votre mot de passe ne changera pas.",

        ConfirmSubject = "Confirmez votre nouvel e-mail Keepr",
        ConfirmHeadline = "Confirmez votre nouvel e-mail",
        ConfirmPreheader = "Confirmez votre nouvel e-mail pour votre compte Keepr.",
        ConfirmBody1 = "Confirmez cette adresse pour commencer à l'utiliser pour vous connecter à Keepr.",
        ConfirmBody2 = "Si vous n'êtes pas à l'origine de cette demande, ignorez cet e-mail — rien ne changera.",
        ConfirmBody1Text = "Confirmez cette adresse pour commencer à l'utiliser pour vous connecter à Keepr :",
        ConfirmCta = "Confirmer cet e-mail",
        ConfirmIgnoreText = "Si vous n'êtes pas à l'origine de cette demande, ignorez cet e-mail — rien ne changera.",

        ChangedSubject = "Votre e-mail Keepr a été modifié",
        ChangedPreheader = "L'adresse e-mail de votre compte Keepr a été modifiée.",
        ChangedBody1Format = "L'adresse e-mail de votre compte Keepr a été remplacée par {0}.",
        ChangedBody2 = "Si ce n'était pas vous, contactez immédiatement votre administrateur.",
    };
}
