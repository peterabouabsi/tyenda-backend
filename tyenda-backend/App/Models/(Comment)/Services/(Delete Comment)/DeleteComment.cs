using MediatR;

namespace tyenda_backend.App.Models._Comment_.Services._Delete_Comment_
{
    public class DeleteComment : IRequest<bool>
    {
        public DeleteComment(string commentId)
        {
            CommentId = commentId;
        }

        public string CommentId { get; set; }
    }
}