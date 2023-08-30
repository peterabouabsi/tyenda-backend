using System.Collections.Generic;
using MediatR;
using tyenda_backend.App.Models._Store_.Services._Add_Update_Branches_.Form;

namespace tyenda_backend.App.Models._Store_.Services._Add_Update_Branches_
{
    public class AddUpdateBranch : IRequest<bool>
    {
        public AddUpdateBranch(ICollection<AddUpdateBranchForm> addUpdateBranchForm)
        {
            AddUpdateBranchForm = addUpdateBranchForm;
        }

        public ICollection<AddUpdateBranchForm> AddUpdateBranchForm { get; set; }
    }
}