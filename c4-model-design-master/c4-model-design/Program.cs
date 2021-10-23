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
            styles.Add(new ElementStyle("delivery") { Background = "#2f95c7", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("payment") { Background = "#00A719", Color = "#ffffff", Shape = Shape.RoundedBox });

            
            // 2. Diagrama de Contenedores
            Container mobileApplication = GrocySystem.AddContainer("Mobile App", "Permite a los usuarios visualizar, comprar, y vender los productos desde sus dispositivos moviles.");
            Container webApplication = GrocySystem.AddContainer("Web App", "Permite a los usuarios visualizar, comprar, y vender los productos desde sus dispositivos moviles.");
            Container landingPage = GrocySystem.AddContainer("Landing Page", "Permite a los visitantes conocer de que trata la aplicacion");
            Container apiRest = GrocySystem.AddContainer("API Rest", "API Rest", "NodeJS (Vuejs) port 3000");
            Container database = GrocySystem.AddContainer("Database", "", "MySQL");

            Container checkoutProcessContext = GrocySystem.AddContainer("Checkout Process Context", "Bounded Context para revisar si el pago a sido efectuado", "NodeJS (NestJS)");
            Container paymentProcessContext = GrocySystem.AddContainer("Payment Process Context", "Bounded Context para realizar el pago", "NodeJS (NestJS)");
            Container orderDeliveryContext = GrocySystem.AddContainer("Order Delivery Context", "Bounded Context para planificar el delivery", "NodeJS (NestJS)");
            Container offersContext = GrocySystem.AddContainer("Offers Context", "Bounded Context para administrar las ofertas", "NodeJS (NestJS)");
            Container orderFulfillmentContext = GrocySystem.AddContainer("Order Fulfillment Context", "Bounded Context para cumplimiento o satisfaccion de pedidos", "NodeJS (NestJS)");
            Container addProductContext = GrocySystem.AddContainer("Add Product Context", "Bounded Context para agregar productos al carrito de compras", "NodeJS (NestJS)");
            Container purchaseRecordContext = GrocySystem.AddContainer("Purchase Record Context", "Bounded Context para realizar el registro de compras", "NodeJS (NestJS)");
            Container userRegisterContext = GrocySystem.AddContainer("User Register Context", "Bounded Context para el registro de usuarios", "NodeJS (NestJS)");
            

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
            apiRest.Uses(orderFulfillmentContext, "API Request", "JSON/HTTPS");
            apiRest.Uses(addProductContext, "API Request", "JSON/HTTPS");
            apiRest.Uses(purchaseRecordContext, "API Request", "JSON/HTTPS");
            apiRest.Uses(userRegisterContext, "API Request", "JSON/HTTPS");

            checkoutProcessContext.Uses(database, "QUERY", "JDBC");
            paymentProcessContext.Uses(database, "QUERY", "JDBC");
            orderDeliveryContext.Uses(database, "QUERY", "JDBC");
            offersContext.Uses(database, "QUERY", "JDBC");
            orderFulfillmentContext.Uses(database, "QUERY", "JDBC");
            addProductContext.Uses(database, "QUERY", "JDBC");
            purchaseRecordContext.Uses(database, "QUERY", "JDBC");
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
            orderFulfillmentContext.AddTags("component");
            addProductContext.AddTags("component");
            purchaseRecordContext.AddTags("component");
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

            // 3.1 Diagrama de Componentes (offersContext)
            Component domainLayer = offersContext.AddComponent("Domain Layer", "", "NodeJS (NestJS)");
            Component offerController = offersContext.AddComponent("Offer Controller", "REST API endpoints de control de ofertas.", "NodeJS (NestJS) REST Controller");
            Component offerApplicationService = offersContext.AddComponent("Offer Application Service", "Provee métodos para el control de ofertas, pertenece a la capa Application de DDD", "NestJS Component");
            Component transportRepository = offersContext.AddComponent("Transport Repository", "Información del transporte", "NestJS Component");
            Component productLoteRepository = offersContext.AddComponent("ProductLote Repository", "Información del lote de productos", "NestJS Component");
            Component locationRepository = offersContext.AddComponent("Location Repository", "Ubicación del producto transportado", "NestJS Component");
            
            // Tags
            domainLayer.AddTags("DomainLayer");
            offerController.AddTags("OfferController");
            offerApplicationService.AddTags("OfferApplicationService");
            transportRepository.AddTags("TransportRepository");
            productLoteRepository.AddTags("ProductLoteRepository");
            locationRepository.AddTags("LocationRepository");

            styles.Add(new ElementStyle("DomainLayer") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("OfferController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("OfferApplicationService") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("OfferDomainModel") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("TransportStatus") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("TransportRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("ProductLoteRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("LocationRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });

            apiRest.Uses(offerController, "", "JSON/HTTPS");
            offerController.Uses(offerApplicationService, "Invoca métodos de control de ofertas");
            offerApplicationService.Uses(domainLayer, "Usa", "");
            offerApplicationService.Uses(transportRepository, "", "JDBC");
            offerApplicationService.Uses(productLoteRepository, "", "JDBC");
            offerApplicationService.Uses(locationRepository, "", "JDBC");
            transportRepository.Uses(database, "", "JDBC");
            productLoteRepository.Uses(database, "", "JDBC");
            locationRepository.Uses(database, "", "JDBC");
            
            ComponentView componentView = viewSet.CreateComponentView(offersContext, "Components", "Component Diagram");
            componentView.PaperSize = PaperSize.A4_Landscape;
            componentView.Add(mobileApplication);
            componentView.Add(webApplication);
            componentView.Add(apiRest);
            componentView.Add(database);
            componentView.AddAllComponents();

            // 3.2 Diagrama de Componentes (purchaseRecordContext)
            Component domainLayer2 = purchaseRecordContext.AddComponent("Domain Layer", "", "NodeJS (NestJS)");
            Component purchaseRecordController = purchaseRecordContext.AddComponent("Purchase Record Controller", "REST API endpoints de control del registro de compras.", "NodeJS (NestJS) REST Controller");
            Component purchaseRecordApplicationService = purchaseRecordContext.AddComponent("Purchase Record Application Service", "Provee métodos para el control del registro de compras, pertenece a la capa Application de DDD", "NestJS Component");
            Component recordRepository = purchaseRecordContext.AddComponent("Record Repository", "Información del registro de compras", "NestJS Component");
            Component expenseRepository = purchaseRecordContext.AddComponent("Expense Repository", "Información de los gastos de Grocy", "NestJS Component");
            Component supplierRepository = purchaseRecordContext.AddComponent("Supplier Repository", "Información de los proveedores", "NestJS Component");
            
            // Tags
            domainLayer2.AddTags("DomainLayer2");
            purchaseRecordController.AddTags("PurchaseRecordController");
            purchaseRecordApplicationService.AddTags("PurchaseRecordApplicationService");
            recordRepository.AddTags("RecordRepository");
            expenseRepository.AddTags("ExpenseRepository");
            supplierRepository.AddTags("SupplierRepository");

            styles.Add(new ElementStyle("DomainLayer2") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("PurchaseRecordController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("PurchaseRecordApplicationService") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("RecordRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("ExpenseRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("SupplierRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });

            apiRest.Uses(purchaseRecordController, "", "JSON/HTTPS");
            purchaseRecordController.Uses(purchaseRecordApplicationService, "Invoca métodos de control del registro de compras");
            purchaseRecordApplicationService.Uses(domainLayer2, "Usa", "");
            purchaseRecordApplicationService.Uses(recordRepository, "", "JDBC");
            purchaseRecordApplicationService.Uses(expenseRepository, "", "JDBC");
            purchaseRecordApplicationService.Uses(supplierRepository, "", "JDBC");
            recordRepository.Uses(database, "", "JDBC");
            expenseRepository.Uses(database, "", "JDBC");
            supplierRepository.Uses(database, "", "JDBC");
            
            ComponentView componentView2 = viewSet.CreateComponentView(purchaseRecordContext, "Components2", "Component Diagram");
            componentView2.PaperSize = PaperSize.A4_Landscape;
            componentView2.Add(mobileApplication);
            componentView2.Add(webApplication);
            componentView2.Add(apiRest);
            componentView2.Add(database);
            componentView2.AddAllComponents();
            
            // 3.3 Diagrama de Componentes (addProductContext)
            Component domainLayer3 = addProductContext.AddComponent("Domain Layer", "", "NodeJS (NestJS)");
            Component addProductController = addProductContext.AddComponent("Add Product Controller", "REST API endpoints de control al agregar productos", "NodeJS (NestJS) REST Controller");
            Component addProductApplicationService = addProductContext.AddComponent("Add Product Application Service", "Provee métodos para el control al agregar productos, pertenece a la capa Application de DDD", "NestJS Component");
            Component shoppingCartRepository = addProductContext.AddComponent("Shopping Cart Repository", "Información del carrito de compras", "NestJS Component");
            Component productRepository = addProductContext.AddComponent("Registry Repository", "Información del registro de nuevos productos", "NestJS Component");
            
            // Tags
            domainLayer3.AddTags("DomainLayer3");
            addProductController.AddTags("AddProductController");
            addProductApplicationService.AddTags("AddProductApplicationService");
            shoppingCartRepository.AddTags("ShoppingCartRepository");
            productRepository.AddTags("RegistryRepository");

            styles.Add(new ElementStyle("DomainLayer3") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("AddProductController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("AddProductApplicationService") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("ShoppingCartRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("RegistryRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("UserRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });

            apiRest.Uses(addProductController, "", "JSON/HTTPS");
            addProductController.Uses(addProductApplicationService, "Invoca métodos de control al agregar productos");
            addProductApplicationService.Uses(domainLayer3, "Usa", "");
            addProductApplicationService.Uses(shoppingCartRepository, "", "JDBC");
            addProductApplicationService.Uses(productRepository, "", "JDBC");
            shoppingCartRepository.Uses(database, "", "JDBC");
            productRepository.Uses(database, "", "JDBC");
            
            ComponentView componentView3 = viewSet.CreateComponentView(addProductContext, "Components3", "Component Diagram");
            componentView3.PaperSize = PaperSize.A4_Landscape;
            componentView3.Add(mobileApplication);
            componentView3.Add(webApplication);
            componentView3.Add(apiRest);
            componentView3.Add(database);
            componentView3.AddAllComponents();
            
            // 3.4 Diagrama de Componentes (paymentProcessContext)
            Component domainLayer4 = paymentProcessContext.AddComponent("Domain Layer", "", "NodeJS (NestJS)");
            Component paymentController = paymentProcessContext.AddComponent("Payment Controller", "REST API endpoints del proceso de pago.", "NodeJS (NestJS) REST Controller");
            Component paymentApplicationService = paymentProcessContext.AddComponent("Payment Application Service", "Provee métodos para el proceso de pago, pertenece a la capa Application de DDD", "NestJS Component");
            Component orderRepository = paymentProcessContext.AddComponent("Order Repository", "Información de la orden a pagar", "NestJS Component");
            Component dataRepository = paymentProcessContext.AddComponent("Data Repository", "Información del usuario que realiza el pago", "NestJS Component");
            Component purchaseRepository = paymentProcessContext.AddComponent("Purchase Repository", "Información del método de pago", "NestJS Component");

            // Tags
            domainLayer4.AddTags("DomainLayer4");
            paymentController.AddTags("PaymentController");
            paymentApplicationService.AddTags("PaymentApplicationService");
            orderRepository.AddTags("OrderRepository");
            dataRepository.AddTags("DataRepository");
            purchaseRepository.AddTags("PurchaseRepository");

            styles.Add(new ElementStyle("DomainLayer4") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("PaymentController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("PaymentApplicationService") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("OrderRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("DataRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("PurchaseRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });

            apiRest.Uses(paymentController, "", "JSON/HTTPS");
            paymentController.Uses(paymentApplicationService, "Invoca métodos de proceso de pago");
            paymentApplicationService.Uses(domainLayer4, "Usa", "");
            paymentApplicationService.Uses(orderRepository, "", "JDBC");
            paymentApplicationService.Uses(dataRepository, "", "JDBC");
            paymentApplicationService.Uses(purchaseRepository, "", "JDBC");
            orderRepository.Uses(database, "", "JDBC");
            dataRepository.Uses(database, "", "JDBC");
            purchaseRepository.Uses(database, "", "JDBC");
            purchaseRepository.Uses(PaymentSystem, "", "JSON/HTTPS");

            ComponentView componentView4 = viewSet.CreateComponentView(paymentProcessContext, "Components4", "Component Diagram");
            componentView4.PaperSize = PaperSize.A4_Landscape;
            componentView4.Add(mobileApplication);
            componentView4.Add(webApplication);
            componentView4.Add(apiRest);
            componentView4.Add(database);
            componentView4.Add(PaymentSystem);
            componentView4.AddAllComponents();
            
            // 3.5 Diagrama de Componentes (checkoutProcessContext)
            Component domainLayer5 = checkoutProcessContext.AddComponent("Domain Layer", "", "NodeJS (NestJS)");
            Component checkoutController = checkoutProcessContext.AddComponent("Checkout Controller", "REST API endpoints de verificación.", "NodeJS (NestJS) REST Controller");
            Component checkoutApplicationService = checkoutProcessContext.AddComponent("Checkout Application Service", "Provee métodos para la verificación del pago de una orden, pertenece a la capa Application de DDD", "NestJS Component");
            Component salesRepository = checkoutProcessContext.AddComponent("Sales Repository", "Información de todas las ventas realizadas", "NestJS Component");
            Component addressRepository = checkoutProcessContext.AddComponent("Address Repository", "Información de la dirección para el delivery", "NestJS Component");
            Component validationRepository = checkoutProcessContext.AddComponent("Validation Repository", "Información de la validación de la compra", "NestJS Component");

            // Tags
            domainLayer5.AddTags("DomainLayer5");
            checkoutController.AddTags("CheckoutController");
            checkoutApplicationService.AddTags("CheckoutApplicationService");
            salesRepository.AddTags("SalesRepository");
            addressRepository.AddTags("AddressRepository");
            validationRepository.AddTags("ValidationRepository");

            styles.Add(new ElementStyle("DomainLayer5") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("CheckoutController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("CheckoutApplicationService") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("SalesRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("AddressRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("ValidationRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });

            apiRest.Uses(checkoutController, "", "JSON/HTTPS");
            checkoutController.Uses(checkoutApplicationService, "Invoca métodos de verificación para confirmar si el pago ha sido realizado");
            checkoutApplicationService.Uses(domainLayer5, "Usa", "");
            checkoutApplicationService.Uses(salesRepository, "", "JDBC");
            checkoutApplicationService.Uses(addressRepository, "", "JDBC");
            checkoutApplicationService.Uses(validationRepository, "", "JDBC");
            salesRepository.Uses(database, "", "JDBC");
            addressRepository.Uses(database, "", "JDBC");
            validationRepository.Uses(database, "", "JDBC");
            validationRepository.Uses(PaymentSystem, "", "JSON/HTTPS");

            ComponentView componentView5 = viewSet.CreateComponentView(checkoutProcessContext, "Components5", "Component Diagram");
            componentView5.PaperSize = PaperSize.A4_Landscape;
            componentView5.Add(mobileApplication);
            componentView5.Add(webApplication);
            componentView5.Add(apiRest);
            componentView5.Add(database);
            componentView5.Add(PaymentSystem);
            componentView5.AddAllComponents();

            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);
        }
    }
}