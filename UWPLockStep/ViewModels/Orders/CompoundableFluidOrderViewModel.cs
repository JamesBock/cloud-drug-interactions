using DynamicData;
using DynamicData.Binding;
using MediatR;
using PropertyChanged;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPLockStep.ApplicationLayer.Policy.Queries;
using UWPLockStep.Domain.Common;
using UWPLockStep.Domain.Common.Units;
using UWPLockStep.Domain.Entities.Factors;
using UWPLockStep.Domain.Entities.Ingredients;
using UWPLockStep.Domain.Entities.Intermediates;
using UWPLockStep.Domain.Entities.Orders;
using UWPLockStep.Domain.Entities.Orders.OrderDecorators;
using UWPLockStep.Domain.Entities.People;
using UWPLockStep.Domain.Interfaces;
using UWPLockStep.Helpers;
using UWPLockStep.Persistance;
using UWPLockStep.Persistance.Services;
using UWPLockStep.ViewModels.DisplayObjects;
using UWPLockStep.ViewModels.Orders;

namespace UWPLockStep.ViewModels.Orders
{
    //[AddINotifyPropertyChangedInterface]
    public class CompoundableFluidOrderViewModel : OrderBaseViewModel //: ReactiveObject, IRoutableViewModel 
    {      //private ILockStepContextSql _context;
        [Reactive]
        public IList<OrderFactorUnitValueViewModel> DisplayFactorOrderList{ get; set; }
        [Reactive]
        public ICollection<PatientFactorUnitValueViewModel> DisplayFactorPatientList{ get; set; }
        [Reactive]
        public ICollection<PatientFactorUnitValueViewModel> DisplayFactorTimeAdjustedPatientList{ get; set; }

        public Patient ActivePatient { get; }

        public ICollection<IPolicyEvaluationService<FactorBase>> PolicyEvaluators { get; }

        public CompoundedFluidOrderItem OrderBeingPlaced { get; }

        private ReadOnlyObservableCollection<IIngredientViewModel> _ingredientControls;

        public ReadOnlyObservableCollection<IIngredientViewModel> IngredientControls => _ingredientControls;


        //private ReadOnlyObservableCollection<IPolicyEvaluationService<FactorBase>> _policyEvaluators;

        //public ReadOnlyObservableCollection<IPolicyEvaluationService<FactorBase>> PolicyEvaluatorsHelper => _policyEvaluators;

        //private readonly IObservableCache<IIngredientViewModel, string> ingredientControlCollection;

        //public IObservable<IChangeSet<IIngredientViewModel, string>> Connect() => ingredientControlCollection.Connect(); /*{ get => ingredientControlCollection; set => this.RaiseAndSetIfChanged(ref ingredientControlCollection, value); }*/


        public CompoundableFluidOrderViewModel(IScreen screen/*, IMediator mediator*/) : base(screen/*, mediator*/)
        {

            ActivePatient = _fakeDataBase.DemoPatient;

            //var builder = new CompoundedFluidOrderItemBuilder(_fakeDataBase.DemoFluidOrderOne);

            //OrderBeingPlaced = builder.BuildCompoundedOrder(ActivePatient, 168m, new DurationValue(Duration.Hour, 24), 1, UnitValueFactory.Create(Volume.Milliliter, 100));
            // OrderBeingPlaced.IngredientItems = OrderBeingPlaced.IngredientItems.Select(ii => ii.GetCopy()).ToList();
            OrderBeingPlaced = new CompoundedFluidOrderItem(ActivePatient, _fakeDataBase.DemoFluidOrderOne, 168m, new DurationValue(Duration.Hour, 24), 1, UnitValueFactory.Create(Volume.Milliliter, 100)) { TimeExecuted = DateTime.Now, AdministrationRoute = InjectionRoute.IVCentral };

            //if IngredientVMs are created before the PolicyEval, the IngredientItems on the CurrentOrder can be swapped out and then the Evals are created

            var factorList = OrderBeingPlaced.IngredientItems
               .SelectMany(ii => ii.Ingredient.FactorItems);
            var factorListGroupd = OrderBeingPlaced.IngredientItems
               .SelectMany(ii => ii.Ingredient.FactorItems).GroupBy(x=>x.FactorId).Select(f=>f.First());

            var factorsWithPolicies = factorList
               .Where(f => _fakeDataBase.Policies
               .Any(pf => pf.Target.Id == f.ItemFactor.Id))
               .GroupBy(f => f.ItemFactor)
               .Select(fi => fi.First())
               .ToList();

            //This is incomplete? When Patient aged out of the Neonate PatientType, no Policies were added to the Evaluators because the Evaluators have a query that includes AdminRoute and PatientType.
            PolicyEvaluators = new ObservableCollection<IPolicyEvaluationService<FactorBase>>(factorsWithPolicies
                               .Select(fi => new FactorPolicyEvaluationService(/*(Factor)*/fi.ItemFactor, _fakeDataBase.Policies, ActivePatient, OrderBeingPlaced)));

            //var ordersMatchingFactorsJoin = from o in OrderFactors
            //                            join f in factorList on factorList.Select(fi => fi.FactorId) equals OrderFactors.SelectMany(o => o.Fact.Select(id => id.FactorId))
            //                            select new { Ordy = o.Ord, Facty = f };


            //factorList.Join(OrderFactors.Select(o=>o.Fact),
            //                                            f => f.FactorId,
            //                                            fi => fi.Select(x=>x.FactorId),
            //                                            (f, o) => new { fact = f, ordr = o })
            //                                           ;


            //Flatens Orders to an annonymous object that has all Factors the Order contains
            var orderFactors = ActivePatient.Orders.SelectMany(o => o.IngredientItems
                                                   .Select(ii => ii.Ingredient.FactorItems)
                                                   .Select(x => new
                                                   {
                                                       Ord = o,
                                                       Fact = x
                                                       
                                                   }))
                                                   .GroupBy(d=>d.Ord.OrderItemId)
                                                   .Select(x=>x.First());//TODO: Should this be FirstOrDefault?

            //Orders that have Factors in common with the CurrentOrder
            var ordersMatchingFactors = orderFactors.Where(o => o.Fact
                                                    .Select(fi => fi.FactorId)
                                                    .Any(f => factorList
                                                    .Select(fi => fi.FactorId)
                                                    .Contains(f)))
                                                    ?? orderFactors ;//If query comes back null if reverts to orderFactors...this is not desired

            //.GroupBy(ors => ors.Ord.OrderItemId).First()

            //var times2 = new DateTime(ordersMatchingFactors.Select(r => r.Ord).Select(o=>o.TimeExecuted.Add(o.OrderDuration.TimeSpanDuration).Subtract(OrderBeingPlaced.TimeExecuted).Ticks).Ticks / (o.OrderDuration.TimeSpanDuration.Ticks)));
            var times = ordersMatchingFactors.Select(r => r.Ord).Select(o => o.TimeExecuted.Add(o.OrderDuration.TimeSpanDuration).Subtract(OrderBeingPlaced.TimeExecuted) / o.OrderDuration.TimeSpanDuration);

            //This a static list of Factors and the amount of them in the Orders on the Patient. It will not change but should be combined with the current order in the View
          

            //var faclist = new ObservableCollection<FactorItemUnitValue>((FactorItemUnitValue)factorAggregations);
            ////Finds all of the Factors in the Order that have Policies
            //var factorList = GetPoliciesOnOrderForPatient().Result.PolicyList //This is awesome! This replaces DemoData
            //    .Select(t => t.Target)
            //    .GroupBy(f => f)
            //    .Select(fi => fi.First())
            //    .ToList();


            //var policyEvaluators = new SourceCache<IPolicyEvaluationService<FactorBase>, FactorBase>(pe => pe.Target);



            //policyEvaluators.AddOrUpdate(new ObservableCollection<IPolicyEvaluationService<FactorBase>>(factorList
            //                   .Select(fi => new FactorPolicyEvaluationService(/*(Factor)*/fi.ItemFactor, _fakeDataBase.Policies, ActivePatient, OrderBeingPlaced))));

            ////IObservable<IEnumerable<UnitValueBase>> pooooo = null;

            //policyEvaluators.Connect()
            //    //.AutoRefresh(pe=> pe.ObservableRulingMax)
            //    //This isn't working because these are only reference to the actual objects, which are on the OrderVM
            //    .ObserveOn(RxApp.MainThreadScheduler)
            //    .Bind(out _policyEvaluators)
            //     .Subscribe(
            //    x =>
            //    {
            //        // PolicyEvaluatorsHelper.OrderByDescending(pe => pe.RulingMaximum);
            //        Debug.WriteLine("----------------------------");
            //    }
            //    );



            var ingredientControlCollection = new SourceCache<IIngredientViewModel, string>(vm => vm.IngredientViewMModelId);

            ingredientControlCollection.AddOrUpdate(new ObservableCollection<IIngredientViewModel>(OrderBeingPlaced.IngredientItems.Select(ii => IngredientViewModelFactory.Create(ii, _fakeDataBase.Policies, PolicyEvaluators))));

            ingredientControlCollection.Connect()
                                       //.DeferUntilLoaded()/*.Throttle(TimeSpan.FromMilliseconds(1000))*/
                                       //ideally this would switch on Ingredient Amount but it is causing a loop in intialization. Even when the IngredientAmount is 0 it doesnt get passed the first one.
                                       .AutoRefresh(d => d.SliderAmount)//.WhereReasonsAreNot(ChangeReason.Refresh)
                                                                        //.IgnoreSameReferenceUpdate()
                                       .ObserveOn(RxApp.MainThreadScheduler)
                                       .Bind(out _ingredientControls)
                                         .Subscribe(x =>
                                         {
                                             for (int i = 0; i < _ingredientControls.Count; i++)
                                             {
                                                 var num = _ingredientControls[i];
                                                 if (_ingredientControls[i].GetType()
                                                 == typeof(CompoundableIngredientViewModel))
                                                 {
                                                     _ingredientControls[i].AdjustAmount();

                                                 }
                                             }
                                             //this should return all Orders that contain Factors incommon with the CurrentOrder and will be active once the CurrentOrder will be executed. This does not look at Factors that don't have Policies.
                                             //var factorAggregation = PolicyEvaluators.SelectMany(pe => pe.GetDelayedExecutionAdjustedOrdersForFactor()).GroupBy(p => p.OrderItemId).First();


                                             #region Foreach loop that's is same as for loop above. This is a bottleneck so i'm trying to use the fastest method I can. 
                                             //    foreach (var item in _ingredientControls)
                                             //{
                                             //    if (item.GetType() == typeof(CompoundableIngredientViewModel))
                                             //    {

                                             //    item.AdjustAmount();
                                             //    Debug.WriteLine(item.SliderAmount + "SliderAmount");
                                             //    }
                                             //}

                                             #endregion

                                         }
                                         );

            DisplayFactorTimeAdjustedPatientList = factorList.Select(fi =>

          new PatientFactorUnitValueViewModel(fi, ordersMatchingFactors.Select(ao => ao.Ord)
                          .Select(o => o.IngredientItems
                          .SelectMany(f => f.Ingredient.FactorItems)
                          .Where(f => f.FactorId == fi.FactorId)
                          .Aggregate(UnitValueFactory.Create(fi.FactorPerIngredientUnit.UnitPairs.Values.First().Unit, 0m), (total, next) => total + next.GetTotal(o.IngredientItems
                          .Where(i => i.Ingredient.FactorItems
                          .Any(fid => fid.FactorId == fi.FactorId))
                          .Single(i => i.Ingredient.Id == next.IngredientId).GetTotal(o.OrderUnitValue)))

                          * new DateTime(o.TimeExecuted.Add(o.OrderDuration.TimeSpanDuration).Subtract(OrderBeingPlaced.TimeExecuted).Ticks).Ticks / (o.OrderDuration.TimeSpanDuration.Ticks))
                          .Aggregate(UnitValueFactory.Create(fi.FactorPerIngredientUnit.UnitPairs.Values.First().Unit, 0m), (total, next) => total + next), PolicyEvaluators.Where(p => p.Target == fi.ItemFactor).FirstOrDefault(), OrderBeingPlaced, IngredientControls)

          ).GroupBy(d => d.DisplayFactorItem.ItemFactor.Id)
          .Select(x => x.First())
          .ToList();

            DisplayFactorPatientList = factorList.Select(fi =>

            new PatientFactorUnitValueViewModel(fi, ordersMatchingFactors.Select(ao => ao.Ord)
                    .Select(o => o.IngredientItems
                    .SelectMany(f => f.Ingredient.FactorItems)
                    .Where(f => f.FactorId == fi.FactorId)
                    .Aggregate(UnitValueFactory.Create(fi.FactorPerIngredientUnit.UnitPairs.Values.First().Unit, 0m), (total, next) => total + next.GetTotal(o.IngredientItems
                    .Where(i => i.Ingredient.FactorItems
                    .Any(fid => fid.FactorId == fi.FactorId))
                    .Single(i => i.Ingredient.Id == next.IngredientId).GetTotal(o.OrderUnitValue))))
                    .Aggregate(UnitValueFactory.Create(fi.FactorPerIngredientUnit.UnitPairs.Values.First().Unit, 0m), (total, next) => total + next), PolicyEvaluators.Where(p => p.Target == fi.ItemFactor).FirstOrDefault(), OrderBeingPlaced, IngredientControls)
            ).GroupBy(d => d.DisplayFactorItem.ItemFactor.Id)
            .Select(x => x.First())
            .ToList();

            //DisplayFactorOrderList = OrderBeingPlaced.IngredientItems.SelectMany(ii=>ii.Ingredient.FactorItems
            //         .Select(fi => new FactorItemUnitValue(fi, fi.GetTotal(UnitValueFactory.Create(ii.EditableUnitValue.Unit.UnitPairs[ii.Ingredient.Id].Unit, ii.EditableUnitValue.Amount)))))
            //         .ToList();

            DisplayFactorOrderList = OrderBeingPlaced.IngredientItems
                      .SelectMany(ii => ii.Ingredient.FactorItems
                      .Select(fi => new OrderFactorUnitValueViewModel(fi, fi.GetTotal(UnitValueFactory.Create(ii.EditableUnitValue.Unit.UnitPairs[ii.Ingredient.Id].Unit, ii.EditableUnitValue.Amount)), PolicyEvaluators.Where(p => p.Target == fi.ItemFactor).FirstOrDefault(), OrderBeingPlaced, IngredientControls))).GroupBy(x => x.DisplayFactorItem.FactorId).Select(f => f.First())
                      .ToList();

            //var controlKeys = ingredientControlCollection.Keys;

            //var chargedIngredientID = ingredientControlCollection.Connect()
            //                             .WhenPropertyChanged(vm => vm)
            //                             .Select(v => v.Sender.IngredientViewMModelId);

            //chargedIngredientID
            //                  .Subscribe(w =>
            //                   {

            //                   });



        }
       

        //This could seriously change the functionality of other objects that use INPC
        //protected void RaisePropertyChanged(PropertyChangedEventArgs args)
        //{
        //    ((IReactiveObject)this).RaisePropertyChanged(args);
        //}

        public ObservableCollection<IIngredientViewModel> CreateIngredientControls()
        {
            return new ObservableCollection<IIngredientViewModel>(OrderBeingPlaced.IngredientItems.Select(ii => IngredientViewModelFactory.Create(ii, _fakeDataBase.Policies, PolicyEvaluators)))
                /*.ToObservableChangeSet()*/;
        }

        //public async Task<PoliciesOnOrderForPatient.Model> GetPoliciesOnOrderForPatient()
        //{
           
        //    var policyList = await  _mediator.Send(new PoliciesOnOrderForPatient.Query());

        //    return policyList;
        //}
    }
}
