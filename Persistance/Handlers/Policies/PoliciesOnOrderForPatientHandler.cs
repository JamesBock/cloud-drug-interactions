using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UWPLockStep.ApplicationLayer.Interfaces;
using UWPLockStep.ApplicationLayer.Policy.Queries;

using UWPLockStep.Persistance.Services;

using MediatR;

namespace UWPLockStep.Persistance.Handlers.Policies
{
    public class PoliciesOnOrderForPatientHandler : PoliciesOnOrderForPatient.IHandler
    {
        private readonly LockStepContextSql _db;
        private DemoData _fakedb => new DemoData();
       

        public PoliciesOnOrderForPatientHandler(/*LockStepContextSql db*/)
        {
           // _db = db;// these can't have parameters unless they are registered with the DI container, otherwise. there is no way for the mediator to know what to pass as a paramenter and the build will fail
        }
        public async Task<PoliciesOnOrderForPatient.Model> Handle(PoliciesOnOrderForPatient.Query request, CancellationToken cancellationToken)
        {
            //var factorList = await _db.Ingredients.Select(i => i.FactorItems.Select(fi => fi.FactorId)).ToListAsync();

            var patientType = await _db.Patients.Where(p => p.LockStepId == request.PatientId)
                                                .Select(p => p.PatientTypeStateless.PatientType)  //Would like to test if this works.
                                                .SingleOrDefaultAsync();

            var list = await _db.CombinablePolicies.Join(request.CurrentOrder.IngredientItems.SelectMany(ii => ii.Ingredient.FactorItems),
                                                    p => p.Target.Id,
                                                    fi => fi.FactorId,
                                                    (p, fi) => p)
                                                    .Where(p => p.AdministrationRoutes.Contains(request.CurrentOrder.AdministrationRoute)
                                                         && p.PatientTypes.Contains(patientType))
                                                    .ToListAsync();


            //.Where(p => p.Target.Id == request.IngredientsOnOrder.Select(f=> f.Id) 
            //     && p.AdministrationRoutes.Contains(request.RouteEnumeration) 
            //     && p.PatientTypes.Contains(patientType))
            //.ToListAsync();



            return new PoliciesOnOrderForPatient.Model() { PolicyList = list };
        }
    }
}
