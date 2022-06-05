using ScopoERP.Commercial.BLL;
using ScopoERP.Domain.Models;
using ScopoERP.Domain.Repositories;
using ScopoERP.MaterialManagement.BLL;
using ScopoERP.OrderManagement.BLL;
using ScopoERP.Production.BLL;
using ScopoERP.Stackholder.BLL;
using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;
using System.Reflection;
using System.Web.Mvc;

namespace CodeWarriors.API.App_Start

{
    public static class SimpleInjectorWebApiInitializer
    {
        public static void Initialize()
        {
            var container = new Container();

            InitializeContainer(container);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            container.RegisterMvcIntegratedFilterProvider();

            container.Verify();

            DependencyResolver.SetResolver(
                new SimpleInjectorDependencyResolver(container));
        }

        private static void InitializeContainer(Container container)
        {
            container.Register<ScopoContext>();
            container.Register<UnitOfWork>();

            container.Register<ExportInvoiceLogic>();
            container.Register<AdvancedCMLogic>();
            container.Register<BuyerLogic>();
            container.Register<CustomerLogic>();
            container.Register<PurchaseOrderLogic>();
            container.Register<StyleLogic>();
            container.Register<ProductionStatusLogic>();
            container.Register<ProductionFloorLogic>();
        }
    }
}