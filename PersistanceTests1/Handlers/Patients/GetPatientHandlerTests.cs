using Microsoft.VisualStudio.TestTools.UnitTesting;
using UWPLockStep.Persistance.Handlers.Patients;
using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Moq;
using UWPLockStep.ApplicationLayer.Patients.Queries;
using System.Threading.Tasks;
using LockStepBlazor.Handlers;

namespace UWPLockStep.Persistance.Handlers.Patients.Tests
{
    [TestClass()]
    public class GetPatientHandlerTests
    {
        //private IMediator _mediator;

        [TestMethod()]
        public async Task HandleTest()
        {
            //var query = new GetPatient();
            //var hand = new GetPatientHandler();
            //var _mediator = new Mock<IMediator>();
            //var x = await hand.Handle(new GetPatient.Query(), new System.Threading.CancellationToken());

            //Assert.AreEqual("John", x.QueriedPatient.GivenNames[0]);

    }
    }
}