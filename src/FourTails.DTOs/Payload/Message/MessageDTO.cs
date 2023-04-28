namespace FourTails.DTOs.PayLoad.Message;

public record class Message
(
    int Id,
    string IncomingMessage,
    string OutgoingMessage
);