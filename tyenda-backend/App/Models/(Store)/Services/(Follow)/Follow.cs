using MediatR;
using tyenda_backend.App.Models._Store_.Services._Follow_.Form;

namespace tyenda_backend.App.Models._Store_.Services._Follow_
{
    public class Follow : IRequest<bool>
    {
        public Follow(FollowForm followForm)
        {
            FollowForm = followForm;
        }

        public FollowForm FollowForm { get; set; }
    }
}