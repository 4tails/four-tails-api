namespace FourTails.DTOs.PayLoad;

public record class Message
(
    int Id,
    string IncomingMessage,
    string OutgoingMessage
);