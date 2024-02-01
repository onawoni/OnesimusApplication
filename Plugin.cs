using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Crm.Sdk.Messages;

namespace ClassLibrary1
{
    public class UpdatePassengersName : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            // Extract the tracing service for use in debugging sandboxed plug-ins.  
            // If you are not registering the plug-in in the sandbox, then you do  
            // not have to add any tracing service related code.  
            ITracingService tracingService =
                (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the execution context from the service provider.  
            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

            // Obtain the organization service reference which you will need for  
            // web service calls.  
            IOrganizationServiceFactory serviceFactory =
                (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);


            // The InputParameters collection contains all the data passed in the message request.  
            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.  
                Entity entity = (Entity)context.InputParameters["Target"];

                string passenger1 = null;
                string passenger2 = null;
                string passenger3 = null;

                try
                {
                    if (context.Depth > 1)
                    {
                        return;
                    }
                    ColumnSet BookingCol = new ColumnSet("cr916_passengername", "cr916_secondpassengername", "cr916_thirdpassengername", "cr916_passengersnames");
                    Entity BookingEntity = service.Retrieve(entity.LogicalName, entity.Id, BookingCol);
                    tracingService.Trace("started retrieving passengers details");
                    EntityReference passengerdata1 = BookingEntity.GetAttributeValue<EntityReference>("cr916_passengername");
                    ColumnSet cols = new ColumnSet("fullname");
                    Entity firstContact = service.Retrieve("contact", passengerdata1.Id, cols);
                    passenger1 = firstContact["fullname"].ToString();
                    tracingService.Trace("finished retrieving passengers details "+ passenger1);

                    if (BookingEntity.Attributes.Contains("cr916_secondpassengername"))
                    {
                        tracingService.Trace("started retrieving passenger2 details");
                        EntityReference passengerdata2 = BookingEntity.GetAttributeValue<EntityReference>("cr916_secondpassengername");
                        ColumnSet _cols = new ColumnSet("fullname");
                        Entity secondContact = service.Retrieve("contact", passengerdata2.Id, _cols);
                        passenger2 = secondContact["fullname"].ToString();
                        tracingService.Trace("finished retrieving passenger2 details " + passenger2);
                    }


                    if (BookingEntity.Attributes.Contains("cr916_thirdpassengername"))
                    {
                        tracingService.Trace("started retrieving passenger3 details");
                        EntityReference passengerdata3 = BookingEntity.GetAttributeValue<EntityReference>("cr916_thirdpassengername");
                        ColumnSet __cols = new ColumnSet("fullname");
                        Entity secondContact = service.Retrieve("contact", passengerdata3.Id, __cols);
                        passenger3 = secondContact["fullname"].ToString();
                        tracingService.Trace("finished retrieving passenger3 details " + passenger3);
                    }


                    //
                  //  Entity booking = new Entity("cr916_booking", entity.Id);

                    if (passenger2 != null && passenger3 == null)
                    {
                        BookingEntity["cr916_passengersnames"] = passenger1 + ", " + passenger2;
                       // booking["cr916_passengersnames"] = passenger1 + ", " + passenger2;
                    }

                    if (passenger2 == null && passenger3 != null)
                    {
                        BookingEntity["cr916_passengersnames"] = passenger1 + ", " + passenger3;
                        //booking["cr916_passengersnames"] = passenger1 + ", " + passenger3;
                    }

                    if (passenger2 != null && passenger3 != null)
                    {
                        BookingEntity["cr916_passengersnames"] = passenger1 + ", " + passenger2 + ", " + passenger3;
                        //booking["cr916_passengersnames"] = passenger1 + ", " + passenger2 + ", " + passenger3;
                    }
                    if (passenger2 == null && passenger3 == null)
                    {
                        BookingEntity["cr916_passengersnames"] = passenger1;
                        //booking["cr916_passengersnames"] = passenger1;
                    }
                  //  tracingService.Trace("final value of passenger " + booking["cr916_passengersnames"]);
                    service.Update(BookingEntity);


                }

                catch (Exception ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in MyPlug-in. "+ ex);
                }


            }
        }
    }
}