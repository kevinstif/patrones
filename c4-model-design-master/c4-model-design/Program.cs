using Structurizr;
using Structurizr.Api;

namespace c4_model_design
{
    class Program
    {
        static void Main(string[] args)
        {
            Banking();
        }

        static void Banking()
        {
            const long workspaceId = 70071;
            const string apiKey = "580817e9-d080-4821-9e8b-b4df84cf69c9";
            const string apiSecret = "0a5b9278-529a-4e4d-a61c-e8570d4a2463";

            StructurizrClient structurizrClient = new StructurizrClient(apiKey, apiSecret);
            Workspace workspace = new Workspace("Grocy aplicacion de venta de insumos", "venta de insumos con desperfectos superficiales a menor precio");
            ViewSet viewSet = workspace.Views;
            Model model = workspace.Model;

            // 1. Diagrama de Contexto
            SoftwareSystem GrocySystem = model.AddSoftwareSystem("Grocy App", "aplicacion de venta de insumos con desperfectos superficiales a menor precio");
            SoftwareSystem DeliverySystem = model.AddSoftwareSystem("Delivery System", "Permite realizar deliverys");
            SoftwareSystem PaymentSystem = model.AddSoftwareSystem("Payment Api", "Plataforma que ofrece una API para realizar pagos");
            
            Person Consumer = model.AddPerson("Consumidor", "Consumidores que necesitan comprar productos de canasta familiar a menor costo");
            Person Vendor = model.AddPerson("Vendedor", "Vendedores de supermercados que ofrezcan productos con fallas en el empaquetado o proximos a vencimiento");

            Consumer.Uses(GrocySystem, "Realiza compras de canasta familiar");
            Vendor.Uses(GrocySystem, "Ofrece productos que presenten fallos de empaquetado y proximos a expirar");

            GrocySystem.Uses(DeliverySystem, "permite localizar los puntos de carga y vehiculos");
            GrocySystem.Uses(PaymentSystem, "Permite tener informacion acerca del clima");

            SystemContextView contextView = viewSet.CreateSystemContextView(GrocySystem, "Contexto", "Diagrama de contexto");
            contextView.PaperSize = PaperSize.A4_Landscape;
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();

            // Tags
            Consumer.AddTags("Ciudadano");
            Vendor.AddTags("Ciudadano");
            GrocySystem.AddTags("Grocy");
            DeliverySystem.AddTags("delivery");
            PaymentSystem.AddTags("payment");

            Styles styles = viewSet.Configuration.Styles;
            styles.Add(new ElementStyle("Ciudadano") { Background = "#0a60ff", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("Grocy") { Background = "#008f39", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("delivery") { Background = "#00A719", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("payment") { Background = "#00A719", Color = "#ffffff", Shape = Shape.RoundedBox });

            
            // 2. Diagrama de Contenedores
            Container mobileApplication = GrocySystem.AddContainer("Mobile App", "Permite a los usuarios visualizar, comprar, y vender los productos desde sus dispositivos moviles.");
            Container webApplication = GrocySystem.AddContainer("Web App", "Permite a los usuarios visualizar, comprar, y vender los productos desde sus dispositivos moviles.");
            Container landingPage = GrocySystem.AddContainer("Landing Page", "Permite a los visitantes conocer de que trata la aplicacion");
            Container apiRest = GrocySystem.AddContainer("API Rest", "API Rest", "NodeJS (Vuejs) port 3000");
            Container database = GrocySystem.AddContainer("Database", "", "MySQL");

            Container checkoutProcessContext = GrocySystem.AddContainer("Localize to puntos de Carga", "Bounded Context para revisar si el pago a sido efectuado", "NodeJS (NestJS)");
            Container paymentProcessContext = GrocySystem.AddContainer("calculate charge for ride", "Bounded Context para realizar el pago", "NodeJS (NestJS)");
            Container orderDeliveryContext = GrocySystem.AddContainer("calculate offers", "Bounded Context para planificar el delivery", "NodeJS (NestJS)");
            Container offersContext = GrocySystem.AddContainer("time of max client", "Bounded Context para administrar las ofertas", "NodeJS (NestJS)");
            Container orderFullfillmentContext = GrocySystem.AddContainer("monitor of net", "Bounded Context para cumpliento o satisfaccion de pedidos", "NodeJS (NestJS)");
            Container addProductContext = GrocySystem.AddContainer("float reserve", "Bounded Context para agregar productos al carrito de compras", "NodeJS (NestJS)");
            Container purchaceRecordContext = GrocySystem.AddContainer("localize vehicle", "Bounded Context para realizar el registro de compras", "NodeJS (NestJS)");
            Container userRegisterContext = GrocySystem.AddContainer("Weather Channel", "Bounded Context para el registro de usuarios", "NodeJS (NestJS)");
            

            Consumer.Uses(mobileApplication, "Consulta");
            Consumer.Uses(webApplication, "Consulta");
            Consumer.Uses(landingPage, "Consulta");
            Vendor.Uses(mobileApplication, "Consulta");
            Vendor.Uses(webApplication, "Consulta");
            Vendor.Uses(landingPage, "Consulta");

            mobileApplication.Uses(apiRest, "API Request", "JSON/HTTPS");
            webApplication.Uses(apiRest, "API Request", "JSON/HTTPS");

            apiRest.Uses(checkoutProcessContext, "API Request", "JSON/HTTPS");
            apiRest.Uses(paymentProcessContext, "API Request", "JSON/HTTPS");
            apiRest.Uses(orderDeliveryContext, "API Request", "JSON/HTTPS");
            apiRest.Uses(offersContext, "API Request", "JSON/HTTPS");
            apiRest.Uses(orderFullfillmentContext, "API Request", "JSON/HTTPS");
            apiRest.Uses(addProductContext, "API Request", "JSON/HTTPS");
            apiRest.Uses(purchaceRecordContext, "API Request", "JSON/HTTPS");
            apiRest.Uses(userRegisterContext, "API Request", "JSON/HTTPS");

            checkoutProcessContext.Uses(database, "QUERY", "JDBC");
            paymentProcessContext.Uses(database, "QUERY", "JDBC");
            orderDeliveryContext.Uses(database, "QUERY", "JDBC");
            offersContext.Uses(database, "QUERY", "JDBC");
            orderFullfillmentContext.Uses(database, "QUERY", "JDBC");
            addProductContext.Uses(database, "QUERY", "JDBC");
            purchaceRecordContext.Uses(database, "QUERY", "JDBC");
            userRegisterContext.Uses(database, "QUERY", "JDBC");

            checkoutProcessContext.Uses(PaymentSystem, "API Request", "JSON/HTTPS");
            paymentProcessContext.Uses(PaymentSystem, "API Request", "JSON/HTTPS");
            orderDeliveryContext.Uses(DeliverySystem, "API Request", "JSON/HTTPS");

            // Tags
            mobileApplication.AddTags("MobileApp");
            webApplication.AddTags("WebApp");
            landingPage.AddTags("LandingPage");
            apiRest.AddTags("APIRest");
            database.AddTags("Database");
            checkoutProcessContext.AddTags("component");
            paymentProcessContext.AddTags("component");
            orderDeliveryContext.AddTags("component");
            offersContext.AddTags("component");
            orderFullfillmentContext.AddTags("component");
            addProductContext.AddTags("component");
            purchaceRecordContext.AddTags("component");
            userRegisterContext.AddTags("component");

            styles.Add(new ElementStyle("MobileApp") { Background = "#9d33d6", Color = "#ffffff", Shape = Shape.MobileDevicePortrait, Icon = "" });
            styles.Add(new ElementStyle("WebApp") { Background = "#9d33d6", Color = "#ffffff", Shape = Shape.WebBrowser, Icon = "" });
            styles.Add(new ElementStyle("LandingPage") { Background = "#929000", Color = "#ffffff", Shape = Shape.WebBrowser, Icon = "" });
            styles.Add(new ElementStyle("APIRest") { Shape = Shape.RoundedBox, Background = "#0000ff", Color = "#ffffff", Icon = "" });
            styles.Add(new ElementStyle("Database") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });
            styles.Add(new ElementStyle("component") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            

            ContainerView containerView = viewSet.CreateContainerView(GrocySystem, "Contenedor", "Diagrama de contenedores");
            contextView.PaperSize = PaperSize.Slide_4_3;
            containerView.AddAllElements();
/*
            // 3. Diagrama de Componentes
            Component domainLayer = monitoringContext.AddComponent("Domain Layer", "", "NodeJS (NestJS)");
            Component monitoringController = monitoringContext.AddComponent("Monitoring Controller", "REST API endpoints de monitoreo.", "NodeJS (NestJS) REST Controller");
            Component monitoringApplicationService = monitoringContext.AddComponent("Monitoring Application Service", "Provee métodos para el monitoreo, pertenece a la capa Application de DDD", "NestJS Component");
            Component flightRepository = monitoringContext.AddComponent("Flight Repository", "Información del vuelo", "NestJS Component");
            Component vaccineLoteRepository = monitoringContext.AddComponent("VaccineLote Repository", "Información de lote de vacunas", "NestJS Component");
            Component locationRepository = monitoringContext.AddComponent("Location Repository", "Ubicación del vuelo", "NestJS Component");
            Component aircraftSystemFacade = monitoringContext.AddComponent("Aircraft System Facade", "", "NestJS Component");

            apiRest.Uses(monitoringController, "", "JSON/HTTPS");
            monitoringController.Uses(monitoringApplicationService, "Invoca métodos de monitoreo");
            monitoringController.Uses(aircraftSystemFacade, "Usa");
            monitoringApplicationService.Uses(domainLayer, "Usa", "");
            monitoringApplicationService.Uses(flightRepository, "", "JDBC");
            monitoringApplicationService.Uses(vaccineLoteRepository, "", "JDBC");
            monitoringApplicationService.Uses(locationRepository, "", "JDBC");
            flightRepository.Uses(database, "", "JDBC");
            vaccineLoteRepository.Uses(database, "", "JDBC");
            locationRepository.Uses(database, "", "JDBC");
            locationRepository.Uses(googleMaps, "", "JSON/HTTPS");
            aircraftSystemFacade.Uses(aircraftSystem, "JSON/HTTPS");
            
            // Tags
            domainLayer.AddTags("DomainLayer");
            monitoringController.AddTags("MonitoringController");
            monitoringApplicationService.AddTags("MonitoringApplicationService");
            flightRepository.AddTags("FlightRepository");
            vaccineLoteRepository.AddTags("VaccineLoteRepository");
            locationRepository.AddTags("LocationRepository");
            aircraftSystemFacade.AddTags("AircraftSystemFacade");
            
            styles.Add(new ElementStyle("DomainLayer") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("MonitoringController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("MonitoringApplicationService") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("MonitoringDomainModel") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("FlightStatus") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("FlightRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("VaccineLoteRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("LocationRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("AircraftSystemFacade") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });

            ComponentView componentView = viewSet.CreateComponentView(monitoringContext, "Components", "Component Diagram");
            componentView.PaperSize = PaperSize.A4_Landscape;
            componentView.Add(mobileApplication);
            componentView.Add(webApplication);
            componentView.Add(apiRest);
            componentView.Add(database);
            componentView.Add(aircraftSystem);
            componentView.Add(googleMaps);
            componentView.AddAllComponents();*/

            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);
        }
    }
}