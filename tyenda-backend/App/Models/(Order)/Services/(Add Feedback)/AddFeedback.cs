using MediatR;
using tyenda_backend.App.Models._Order_.Services._Add_Feedback_.Form;

namespace tyenda_backend.App.Models._Order_.Services._Add_Feedback_
{
    public class AddFeedback : IRequest<bool>
    {
        public AddFeedback(AddFeedbackForm addFeedbackForm)
        {
            AddFeedbackForm = addFeedbackForm;
        }

        public AddFeedbackForm AddFeedbackForm { get; set; }
    }
}