using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPLockStep.ApplicationLayer.DrugInteractions;
using UWPLockStep.ApplicationLayer.Interfaces.MediatR;
using UWPLockStep.Domain.Common;

namespace UWPLockStep.ApplicationLayer.MediatrExtensions
{
    public static class DrugInteractionsExtension
    {
        public async static Task<IGetDrugInteractions.Model> GetDrugInteractions(this IMediator mediator, List<string> medDtos)
        {
            return await mediator.Send(new IGetDrugInteractions.Query(medDtos));
        }
    }
}
