namespace FourTails.DTOs;

public record class Message
(
    int Id,
    string IncomingMessage,
    string OutgoingMessage
);