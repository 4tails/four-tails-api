namespace FourTails.Core.DomainModels;

public class Message
{
    public int Id {get; set;}
    public string IncomingMessage {get; init;}
    public User MessageAuthor {get; set;}
    public int MessageAuthorId {get; set;}
    public string OutgoingMessage {get; init;}


    public Message (int id, string incomingMessage, string outgoingMessage, User messageAuthor, int messageAuthorId) 
    {
        Id = id;
        IncomingMessage = incomingMessage ?? throw new ArgumentNullException(nameof(incomingMessage));
        OutgoingMessage = outgoingMessage ?? throw new ArgumentNullException(nameof(outgoingMessage));
        MessageAuthor = messageAuthor ?? throw new ArgumentNullException(nameof(messageAuthor));
        MessageAuthorId = messageAuthorId;
    }
};