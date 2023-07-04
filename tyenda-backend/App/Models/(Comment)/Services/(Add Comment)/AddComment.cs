using MediatR;
using tyenda_backend.App.Models._Comment_.Services._Add_Comment_.Form;

namespace tyenda_backend.App.Models._Comment_.Services._Add_Comment_
{
    public class AddComment : IRequest<bool>
    {
        public AddComment(AddCommentForm addCommentForm)
        {
            AddCommentForm = addCommentForm;
        }

        public AddCommentForm AddCommentForm { get; set; }
    }
}