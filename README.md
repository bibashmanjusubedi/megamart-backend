MegaMart.Api/
├── Controllers/                 # REST API endpoints
│   ├── AuthController.cs        # POST /api/auth/register, POST /api/auth/login
│   ├── ProductsController.cs    # GET, POST, PUT, DELETE /api/products
│   ├── CategoriesController.cs  # GET, POST /api/categories
│   └── OrdersController.cs      # GET /api/orders, POST /api/orders, PUT /api/orders/{id}/status
│
├── Models/                      # Database entities (EF Core models)
│   ├── User.cs                  # Id, Name, Email, PasswordHash, Role, CreatedAt
│   ├── Product.cs               # Id, CategoryId, Name, Price, StockQuantity, ImageUrl, etc.
│   ├── Category.cs              # Id, Name
│   ├── Order.cs                 # Id, UserId, TotalAmount, Status, CreatedAt
│   ├── OrderItem.cs             # Id, OrderId, ProductId, Quantity, UnitPrice
│   ├── SecondaryImage.cs        # Owned type for JSON storage (ImageUrl, ImagePublicId)
│   └── Specifications.cs        # Owned type for JSON storage (Model, Warranty, Delivery)
│
├── Data/                        # Database context and migrations
│   ├── AppDbContext.cs          # EF Core DbContext with JSON column configurations
│   └── Migrations/              # Generated EF Core migration files
│
├── DTOs/                        # Request and Response transfer objects
│   ├── AuthDtos.cs              # RegisterRequest, LoginRequest, AuthResponse
│   ├── ProductDtos.cs           # ProductResponse, CreateProductRequest
│   └── OrderDtos.cs             # OrderResponse, CreateOrderRequest, UpdateOrderStatusRequest
│
├── Services/                    # Shared helper services
│   ├── TokenService.cs          # JWT token generation
│   └── CloudinaryService.cs     # Direct Cloudinary image upload and deletion
│
├── Middleware/                  # HTTP pipeline middleware
│   └── ExceptionMiddleware.cs   # Global error handling and formatting
│
├── appsettings.json             # DB connection strings, JWT secret, Cloudinary keys
├── appsettings.Development.json
├── Program.cs                   # All DI registrations, EF configuration, and middleware pipeline
└── MegaMart.Api.csproj          # Single project file


# ER Diagram
* [OptionAPreferred](./Screenshots/OptionA.PNG)
* [OptionB](./Screenshots/OptionB.PNG)