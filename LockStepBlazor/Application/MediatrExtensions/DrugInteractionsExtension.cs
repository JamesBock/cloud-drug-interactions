using LockStepBlazor.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockStepBlazor.Application.Extensions
{
    public static class DrugInteractionsExtension
    {
        public async static Task<IGetDrugInteractions.Model> GetDrugInteractions(this IMediator mediator, List<string> medDtos)
        {
            return await mediator.Send(new IGetDrugInteractions.Query(medDtos));
        }
    }
}
