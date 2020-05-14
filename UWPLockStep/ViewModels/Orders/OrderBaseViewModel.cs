using MediatR;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPLockStep.Domain.Common;
using UWPLockStep.Domain.Common.Units;
using UWPLockStep.Domain.Entities.Factors;
using UWPLockStep.Domain.Entities.Intermediates;
using UWPLockStep.Domain.Entities.People;
using UWPLockStep.Persistance;

namespace UWPLockStep.ViewModels.Orders
{
    public abstract class OrderBaseViewModel : ReactiveObject, IRoutableViewModel
    {
        protected  DemoData _fakeDataBase { get; }
        //protected IMediator _mediator;
        //IMediator Mediator => _mediator ??= RequestServices.GetService<IMediator>();
        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

        public IScreen HostScreen { get; }


        protected OrderBaseViewModel(IScreen screen/*, IMediator mediator*//*, OrderItemBase orderBeginPlaced*/ )//not in ctor at this point for demo purposes
        {
            _fakeDataBase = new DemoData();
            //_mediator = mediator;
            HostScreen = screen;

        }
    }

}
