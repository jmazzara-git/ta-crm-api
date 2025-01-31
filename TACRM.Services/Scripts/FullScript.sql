-- Create Enums
CREATE TYPE contact_status AS ENUM ('New', 'InProgress', 'Waiting', 'Converted', 'Future', 'Lost');
CREATE TYPE subscription_status AS ENUM ('Active', 'Canceled', 'Expired');
CREATE TYPE sale_product_status AS ENUM ('Active', 'Canceled');
CREATE TYPE user_type AS ENUM ('AGENT', 'AGENCY', 'ADMIN');

-- Table for Users
CREATE TABLE "Users" (
    "UserID" SERIAL PRIMARY KEY,
    "ParentUserID" INT NULL, 
    "UserType" user_type NOT NULL DEFAULT 'AGENT',
    "IdpID" TEXT NOT NULL UNIQUE,
    "Email" TEXT NOT NULL UNIQUE,
    "UserName" TEXT,
    "UserContactInfo" TEXT,
    "UserPicturePath" TEXT,
    "AgencyName" TEXT,
    "AgencyContactInfo" TEXT,
    "AgencyPicturePath" TEXT,
    "DefaultBudgetMessage" TEXT,
    "DefaultWelcomeMessage" TEXT,
    "DefaultThanksMessage" TEXT,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    "IsDisabled" BOOLEAN DEFAULT FALSE,
    FOREIGN KEY ("ParentUserID") REFERENCES "Users"("UserID")
);

-- Table for Subscriptions
CREATE TABLE "Subscriptions" (
    "SubscriptionID" SERIAL PRIMARY KEY,
    "UserID" INT NOT NULL,
    "PlanName" TEXT NOT NULL,
    "StartDate" TIMESTAMP NOT NULL,
    "EndDate" TIMESTAMP NOT NULL,
    "Status" subscription_status NOT NULL DEFAULT 'Active', 
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY ("UserID") REFERENCES "Users"("UserID")
);

-- Table for Product Types
CREATE TABLE "ProductType" (
    "ProductTypeID" SERIAL PRIMARY KEY,
    "ProductTypeKey" TEXT NOT NULL UNIQUE
);

INSERT INTO "ProductType" ("ProductTypeKey") VALUES
('Package'),
('Hotel'),
('Ticket'),
('Attraction'),
('Car'),
('Insurance');

-- Table for Product Types localization
CREATE TABLE "ProductTypeTranslations" (
    "TranslationID" SERIAL PRIMARY KEY,
    "ProductTypeID" INT NOT NULL,
    "LanguageCode" TEXT NOT NULL,
    "DisplayName" TEXT NOT NULL,
    FOREIGN KEY ("ProductTypeID") REFERENCES "ProductType"("ProductTypeID")
);

INSERT INTO "ProductTypeTranslations" ("ProductTypeID", "LanguageCode", "DisplayName") VALUES
(1, 'en', 'Package'),
(1, 'es', 'Paquete'),
(2, 'en', 'Hotel'),
(2, 'es', 'Hotel'),
(3, 'en', 'Ticket'),
(3, 'es', 'Boleto'),
(4, 'en', 'Attraction'),
(4, 'es', 'Atracción'),
(5, 'en', 'Car'),
(5, 'es', 'Auto'),
(6, 'en', 'Insurance'),
(6, 'es', 'Seguro');

-- Table for Products
CREATE TABLE "Products" (
    "ProductID" SERIAL PRIMARY KEY,
    "UserID" INT NOT NULL,
    "ProductTypeID" INT NOT NULL,
    "ProductName" TEXT NOT NULL,
    "ProductDetails" TEXT NULL,
    "IsShared" BOOLEAN DEFAULT FALSE,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    "IsDisabled" BOOLEAN DEFAULT FALSE,
    FOREIGN KEY ("ProductTypeID") REFERENCES "ProductType"("ProductTypeID"),
    FOREIGN KEY ("UserID") REFERENCES "Users"("UserID")
);

-- Table for Providers
CREATE TABLE "Providers" (
    "ProviderID" SERIAL PRIMARY KEY,
    "UserID" INT NOT NULL,
    "ProviderName" TEXT NOT NULL,
	"ProviderDetails" TEXT NOT NULL,
    "IsShared" BOOLEAN DEFAULT FALSE,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    "IsDisabled" BOOLEAN DEFAULT FALSE,
    FOREIGN KEY ("UserID") REFERENCES "Users"("UserID")
);

-- Table for Contacts Sources
CREATE TABLE "ContactSource" (
    "ContactSourceID" SERIAL PRIMARY KEY,
    "ContactSourceName" TEXT NOT NULL
);

INSERT INTO "ContactSource" ("ContactSourceName") VALUES 
('WhatsApp'),
('Instagram'),
('Website'),
('Other');

-- Table for Contacts Statuses
CREATE TABLE "ContactStatus" (
    "ContactStatusID" SERIAL PRIMARY KEY,
    "ContactStatusKey" contact_status NOT NULL UNIQUE
);

INSERT INTO "ContactStatus" ("ContactStatusKey") VALUES 
('New'), 
('InProgress'), 
('Converted'), 
('Future'), 
('Lost');

-- Table for Contacts Statuses localization
CREATE TABLE "ContactStatusTranslations" (
    "TranslationID" SERIAL PRIMARY KEY,
    "ContactStatusID" INT NOT NULL,
    "LanguageCode" TEXT NOT NULL,
    "DisplayName" TEXT NOT NULL,
    FOREIGN KEY ("ContactStatusID") REFERENCES "ContactStatus"("ContactStatusID")
);

INSERT INTO "ContactStatusTranslations" ("ContactStatusID", "LanguageCode", "DisplayName") VALUES
(1, 'en', 'New'),
(1, 'es', 'Nuevo'),
(2, 'en', 'In Progress'),
(2, 'es', 'En Progreso'),
(3, 'en', 'Converted'),
(3, 'es', 'Ganado'),
(4, 'en', 'Future'),
(4, 'es', 'Futuro'),
(5, 'en', 'Lost'),
(5, 'es', 'Perdido');

-- Table for Contacts
CREATE TABLE "Contacts" (
    "ContactID" SERIAL PRIMARY KEY,
    "UserID" INT NOT NULL,
    "ContactSourceID" INT,
    "ContactStatusID" INT NOT NULL,
    "FullName" TEXT NOT NULL,
    "Email" TEXT,
    "Phone" TEXT,
    "FromDate" DATE,
    "ToDate" DATE,
    "Adults" INT DEFAULT 0,
    "Kids" INT DEFAULT 0,
    "KidsAges" TEXT,
    "Comments" TEXT,
    "EnableWhatsAppNotifications" BOOLEAN DEFAULT FALSE,
    "EnableEmailNotifications" BOOLEAN DEFAULT FALSE,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    "IsDisabled" BOOLEAN DEFAULT FALSE,
    FOREIGN KEY ("UserID") REFERENCES "Users"("UserID"),
    FOREIGN KEY ("ContactSourceID") REFERENCES "ContactSource"("ContactSourceID"),
    FOREIGN KEY ("ContactStatusID") REFERENCES "ContactStatus"("ContactStatusID")
);

-- Table for ContactProducts
CREATE TABLE "ContactProducts" (
    "ContactProductID" SERIAL PRIMARY KEY,
    "ContactID" INT NOT NULL,
    "ProductID" INT NOT NULL,
    FOREIGN KEY ("ContactID") REFERENCES "Contacts"("ContactID"),
    FOREIGN KEY ("ProductID") REFERENCES "Products"("ProductID")
);

-- Table for Budgets
CREATE TABLE "Budgets" (
    "BudgetID" SERIAL PRIMARY KEY,
    "UserID" INT NOT NULL,    
    "ContactID" INT NOT NULL,
    "BudgetName" TEXT NOT NULL,
    "BudgetDetails" TEXT,
    "Adults" INT DEFAULT 0,
    "Kids" INT DEFAULT 0,
    "KidsAges" TEXT,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY ("UserID") REFERENCES "Users"("UserID"),
    FOREIGN KEY ("ContactID") REFERENCES "Contacts"("ContactID")
);

-- Table for BudgetProducts
CREATE TABLE "BudgetProducts" (
    "BudgetProductID" SERIAL PRIMARY KEY,
    "BudgetID" INT NOT NULL,
    "ProductID" INT NOT NULL,
    "ProductDetails" TEXT,
    "BudgetDate" DATE,
    "FromDate" DATE,
    "ToDate" DATE,
    "Currency" TEXT NOT NULL DEFAULT 'USD',
    "BasePrice" DECIMAL(10, 2),
    "FinalPrice" DECIMAL(10, 2) NOT NULL,
    "Commission" DECIMAL(10, 2),
    "ProviderID" INT,
    "BookingID" TEXT,
    "BookingDate" DATE,
    "ExpirationDate" DATE,
    "FilePath" TEXT,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY ("BudgetID") REFERENCES "Budgets"("BudgetID"),
    FOREIGN KEY ("ProductID") REFERENCES "Products"("ProductID"),
    FOREIGN KEY ("ProviderID") REFERENCES "Providers"("ProviderID"),
    CHECK ("Currency" IN ('USD', 'ARS'))
);

-- Table for Sales
CREATE TABLE "Sales" (
    "SaleID" SERIAL PRIMARY KEY,
    "UserID" INT NOT NULL,
    "ContactID" INT NOT NULL,
    "SaleGUID" UUID DEFAULT gen_random_uuid(),
    "SaleName" TEXT NOT NULL,
    "SaleDetails" TEXT,
    "StartDate" DATE,
    "Adults" INT DEFAULT 0,
    "Kids" INT DEFAULT 0,
    "KidsAges" TEXT,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY ("UserID") REFERENCES "Users"("UserID"),
    FOREIGN KEY ("ContactID") REFERENCES "Contacts"("ContactID")
);

-- Table for SaleTravelers
CREATE TABLE "SaleTravelers" (
    "SaleTravelerID" SERIAL PRIMARY KEY,
    "SaleID" INT NOT NULL,
    "FirstName" TEXT NOT NULL,
    "LastName" TEXT NOT NULL,
    "Age" INT,
    "SpecialRequirements" TEXT,
    "IsPrimary" BOOLEAN DEFAULT FALSE,
    FOREIGN KEY ("SaleID") REFERENCES "Sales"("SaleID")
);

-- Table for SaleProducts
CREATE TABLE "SaleProducts" (
    "SaleProductID" SERIAL PRIMARY KEY,
    "SaleID" INT NOT NULL,
    "ProductID" INT NOT NULL,
    "ProviderID" INT,
    "BookingID" TEXT NOT NULL,
    "BookingDate" DATE,
    "FromDate" DATE,
    "ToDate" DATE,
    "Currency" TEXT NOT NULL DEFAULT 'USD',
    "BasePrice" DECIMAL(10, 2) NOT NULL,
    "FinalPrice" DECIMAL(10, 2) NOT NULL,
    "PaymentDueDate" DATE,
    "Commission" DECIMAL(10, 2) NOT NULL,
    "Status" sale_product_status DEFAULT 'Active',
    "CancellationReason" TEXT,
    "CancellationDate" TIMESTAMP,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY ("SaleID") REFERENCES "Sales"("SaleID"),
    FOREIGN KEY ("ProductID") REFERENCES "Products"("ProductID"),
    FOREIGN KEY ("ProviderID") REFERENCES "Providers"("ProviderID"),
    CHECK ("Currency" IN ('USD', 'ARS'))
);

-- Table for Payments
CREATE TABLE "Payments" (
    "PaymentID" SERIAL PRIMARY KEY,
    "SaleProductID" INT NOT NULL,
    "Currency" TEXT NOT NULL DEFAULT 'USD',
    "PaymentAmount" DECIMAL(10, 2) NOT NULL,
    "PaymentDate" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    "PaymentMethod" TEXT,
    FOREIGN KEY ("SaleProductID") REFERENCES "SaleProducts"("SaleProductID"),
    CHECK ("Currency" IN ('USD', 'ARS'))
);

-- Table for Events
CREATE TABLE "Events" (
    "EventID" SERIAL PRIMARY KEY,
    "UserID" INT NOT NULL,
    "EventType" TEXT NOT NULL,
    "Title" TEXT NOT NULL,
    "Description" TEXT,
    "StartDateTime" TIMESTAMP NOT NULL,
    "EndDateTime" TIMESTAMP,
    "IsCustom" BOOLEAN DEFAULT FALSE,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY ("UserID") REFERENCES "Users"("UserID")
);

-- Table for Notifications
CREATE TABLE "Notifications" (
    "NotificationID" SERIAL PRIMARY KEY,
    "UserID" INT NOT NULL,
    "NotificationType" TEXT NOT NULL,
    "Message" TEXT NOT NULL,
    "IsRead" BOOLEAN DEFAULT FALSE,
    "EventID" INT,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY ("UserID") REFERENCES "Users"("UserID"),
    FOREIGN KEY ("EventID") REFERENCES "Events"("EventID")
);