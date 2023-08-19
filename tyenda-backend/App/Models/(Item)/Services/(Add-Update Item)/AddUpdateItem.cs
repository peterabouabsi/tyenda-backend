using MediatR;
using tyenda_backend.App.Models._Item_.Services._Add_Update_Item_.Forms;

namespace tyenda_backend.App.Models._Item_.Services._Add_Update_Item_
{
    public class AddUpdateItem : IRequest<Item>
    {
        public AddUpdateItem(AddUpdateItemForm addUpdateItemForm)
        {
            AddUpdateItemForm = addUpdateItemForm;
        }

        public AddUpdateItemForm AddUpdateItemForm { get; set; }
    }
}