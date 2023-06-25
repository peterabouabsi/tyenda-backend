using System.Collections.Generic;
using MediatR;
namespace tyenda_backend.App.Models._Category_.Services._GetCategories_
{
    public class GetCategories : IRequest<ICollection<Category>>
    {
    }
}