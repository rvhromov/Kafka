namespace KafkaJourney.Shared.Models;

public enum EventType : byte
{
    ButtonClicked,
    UrlClicked,
    EmailClicked,
    PhoneClicked,
    ImageClicked,
    UrlCopied,
    EmailCopied,
    PhoneCopied,
    ImageSaved,
    VideoPlayed
}