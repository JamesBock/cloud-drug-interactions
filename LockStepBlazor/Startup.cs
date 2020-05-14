using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using LockStepBlazor.Data;
using System.Reflection;
using MediatR;
using UWPLockStep.ApplicationLayer.Patients.Queries;
using UWPLockStep.ApplicationLayer.FHIR.Queries;

using LockStepBlazor.Data.Services;
using LockStepBlazor.Handlers;
using Hl7.Fhir.Rest;
using LockStepBlazor.Shared;
using UWPLockStep.ApplicationLayer.DrugInteractions;
using UWPLockStep.ApplicationLayer.Interfaces.MediatR;
using System.Threading.Channels;

namespace LockStepBlazor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //This is needed to register the correct Handler for MedicationRequests. 
            //The AddMediatR() method registers the first one for the Interface based on alphabetical order, it seems like.
            //This overrides AddMediatR().
            //I believe this implementation is the same as the DrugInteraction Interface but it can be chaged without explicitly registering the RequestHandler, meaning only the Interface needs to be registered...I think
            services.AddSingleton<IRequestHandler<IGetFhirMedications.Query, IGetFhirMedications.Model>, GetFhirR4MedicationsAPIHandler>();
            //services.AddSingleton<IRequestHandler<GetRxCuiListAPI.Query, GetRxCuiListAPI.Model>, GetRxCuiListAPIHandler>();//w/o interface, this only is needed with the MOck and crashes the app if using the API

            //This registration is still needed also to register the correct Handler to the PatientDateService.
            services.AddSingleton<IGetFhirMedications, GetFhirMedicationsAPI>();
            //services.AddMediatR(typeof(GetFhirMedicationsJSONHandler));//This did not fix it.
            services.AddSingleton<IFhirClient, FhirClient>(c=> new FhirClient(Constants.FHIR_URI) { PreferredFormat = ResourceFormat.Json });

            services.AddHttpClient<IRequestHandler<GetRxCuiListAPI.Query, GetRxCuiListAPI.Model>, GetRxCuiListAPIHandler>("RXCUI", client =>
            { client.BaseAddress = new Uri(Constants.RXCUI_API_URI); });

            services.AddSingleton<IGetDrugInteractions, GetDrugInteractionsAPI>();//these two must match implementations
            services.AddHttpClient<IRequestHandler<IGetDrugInteractions.Query, IGetDrugInteractions.Model>, GetDrugInteractionsAPIHandler>("Interactions", client =>
            { client.BaseAddress = new Uri(Constants.NLM_INTERACTION_API_URI); });//Could make this a base class and have all handlers inheirt? cannot be abstract, virtual seem to explicitly implement the base class and not the inheiriting class
            //services.AddControllers();//NEW
            //services.AddHostedService<RxCuisNotificationDispatcher>();//NEW
            //services.AddMediatR(typeof(GetMedicationRequestsHandler).GetTypeInfo().Assembly);//this is not needed now that all Handlers are in the Blazor Assembly
            services.AddRazorPages();
            services.AddServerSideBlazor().AddCircuitOptions(o => { o.DetailedErrors = true; }) ;
            services.AddMediatR(typeof(Startup));
            services.AddSingleton<IDrugInteractionParser, DrugInteractionParser>();
            //services.AddSingleton(Channel.CreateUnbounded<string>());
            services.AddSingleton<IPatientDataService, PatientDataService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
                //endpoints.MapDefaultControllerRoute();//NEW
            });
        }
    }
}
