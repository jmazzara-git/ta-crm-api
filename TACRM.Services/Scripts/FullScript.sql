-- Create Enums
CREATE TYPE contact_status AS ENUM ('New', 'InProgress', 'Converted', 'Future', 'Lost');
CREATE TYPE subscription_status AS ENUM ('Active', 'Canceled', 'Expired');
CREATE TYPE sale_product_status AS ENUM ('Active', 'Cancelled');

-- Table for Agencies
CREATE TABLE "Agencies" (
    "AgencyID" SERIAL PRIMARY KEY,
    "AgencyName" VARCHAR(255) NOT NULL,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Table for Users
CREATE TABLE "Users" (
    "UserID" SERIAL PRIMARY KEY,
    "AgencyID" INT,
    "Email" VARCHAR(255) NOT NULL UNIQUE,
    "FullName" VARCHAR(255),
    "UserType" VARCHAR(50) NOT NULL DEFAULT 'AGENT',
    "DefaultBudgetMessage" TEXT,
    "DefaultWelcomeMessage" TEXT,
    "DefaultThanksMessage" TEXT,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY ("AgencyID") REFERENCES "Agencies"("AgencyID") ON DELETE SET NULL
);

-- Table for Subscriptions
CREATE TABLE "Subscriptions" (
    "SubscriptionID" SERIAL PRIMARY KEY,
    "UserID" INT NOT NULL,
    "StartDate" TIMESTAMP NOT NULL,
    "EndDate" TIMESTAMP NOT NULL,
    "Status" subscription_status NOT NULL DEFAULT 'Active',
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY ("UserID") REFERENCES "Users"("UserID") ON DELETE CASCADE
);

-- Table for Product Types
CREATE TABLE "ProductType" (
    "ProductTypeID" SERIAL PRIMARY KEY,
    "ProductTypeKey" VARCHAR(50) NOT NULL UNIQUE
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
    "LanguageCode" VARCHAR(5) NOT NULL,
    "DisplayName" VARCHAR(255) NOT NULL,
    FOREIGN KEY ("ProductTypeID") REFERENCES "ProductType"("ProductTypeID") ON DELETE CASCADE
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
(5, 'es', 'Coche'),
(6, 'en', 'Insurance'),
(6, 'es', 'Seguro');

-- Table for Products
CREATE TABLE "Products" (
    "ProductID" SERIAL PRIMARY KEY,
    "UserID" INT,
    "ProductTypeID" INT NOT NULL,
    "ProductName" VARCHAR(255) NOT NULL,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY ("ProductTypeID") REFERENCES "ProductType"("ProductTypeID") ON DELETE CASCADE,
    FOREIGN KEY ("UserID") REFERENCES "Users"("UserID") ON DELETE SET NULL
);

-- Table for Providers
CREATE TABLE "Providers" (
    "ProviderID" SERIAL PRIMARY KEY,
    "UserID" INT,
    "ProviderName" VARCHAR(255) NOT NULL,
    "ContactInfo" TEXT,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY ("UserID") REFERENCES "Users"("UserID") ON DELETE SET NULL
);

-- Table for Contacts Sources
CREATE TABLE "ContactSource" (
    "ContactSourceID" SERIAL PRIMARY KEY,
    "ContactSourceName" VARCHAR(255) NOT NULL
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
    "LanguageCode" VARCHAR(5) NOT NULL,
    "DisplayName" VARCHAR(255) NOT NULL,
    FOREIGN KEY ("ContactStatusID") REFERENCES "ContactStatus"("ContactStatusID") ON DELETE CASCADE
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
    "FullName" VARCHAR(255) NOT NULL,
    "Email" VARCHAR(255),
    "Phone" VARCHAR(50),
    "TravelDateStart" DATE,
    "TravelDateEnd" DATE,
    "Adults" INT DEFAULT 0,
    "Kids" INT DEFAULT 0,
    "KidsAges" TEXT,
    "Comments" TEXT,
    "EnableWhatsAppNotifications" BOOLEAN DEFAULT FALSE,
    "EnableEmailNotifications" BOOLEAN DEFAULT FALSE,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY ("UserID") REFERENCES "Users"("UserID") ON DELETE CASCADE,
    FOREIGN KEY ("ContactSourceID") REFERENCES "ContactSource"("ContactSourceID") ON DELETE SET NULL,
    FOREIGN KEY ("ContactStatusID") REFERENCES "ContactStatus"("ContactStatusID") ON DELETE SET NULL
);

-- Table for ContactProductInterest
CREATE TABLE "ContactProductInterest" (
    "ContactProductInterestID" SERIAL PRIMARY KEY,
    "ContactID" INT NOT NULL,
    "ProductID" INT NOT NULL,
    FOREIGN KEY ("ContactID") REFERENCES "Contacts"("ContactID") ON DELETE CASCADE,
    FOREIGN KEY ("ProductID") REFERENCES "Products"("ProductID") ON DELETE CASCADE
);

-- Table for Budgets
CREATE TABLE "Budgets" (
    "BudgetID" SERIAL PRIMARY KEY,
    "ContactID" INT NOT NULL,
    "BudgetName" VARCHAR(255) NOT NULL,
    "BudgetDetails" TEXT,
    "Adults" INT DEFAULT 0,
    "Kids" INT DEFAULT 0,
    "KidsAges" TEXT,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY ("ContactID") REFERENCES "Contacts"("ContactID") ON DELETE CASCADE
);

-- Table for BudgetProducts
CREATE TABLE "BudgetProducts" (
    "BudgetProductID" SERIAL PRIMARY KEY,
    "BudgetID" INT NOT NULL,
    "ProductID" INT NOT NULL,
    "ProductDetails" TEXT,
    "BudgetDate" DATE,
    "CheckinDate" DATE,
    "CheckoutDate" DATE,
    "ProviderID" INT,
    "Currency" VARCHAR(10) NOT NULL DEFAULT 'USD',
    "BasePrice" DECIMAL(10, 2),
    "FinalPrice" DECIMAL(10, 2) NOT NULL,
    "Commission" DECIMAL(10, 2),
    "BookingID" VARCHAR(50),
    "BookingDate" DATE,
    "ExpirationDate" DATE,
    "FilePath" TEXT,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY ("BudgetID") REFERENCES "Budgets"("BudgetID") ON DELETE CASCADE,
    FOREIGN KEY ("ProductID") REFERENCES "Products"("ProductID") ON DELETE CASCADE,
    FOREIGN KEY ("ProviderID") REFERENCES "Providers"("ProviderID") ON DELETE SET NULL,
    CHECK ("Currency" IN ('USD', 'ARS'))
);

-- Table for Sales
CREATE TABLE "Sales" (
    "SaleID" SERIAL PRIMARY KEY,
    "UserID" INT NOT NULL,
    "ContactID" INT NOT NULL,
    "SaleGUID" UUID DEFAULT gen_random_uuid(),
    "SaleName" VARCHAR(255) NOT NULL,
    "SaleDetails" TEXT,
    "Adults" INT DEFAULT 0,
    "Kids" INT DEFAULT 0,
    "KidsAges" TEXT,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY ("UserID") REFERENCES "Users"("UserID") ON DELETE CASCADE,
    FOREIGN KEY ("ContactID") REFERENCES "Contacts"("ContactID") ON DELETE CASCADE
);

-- Table for SaleTravelers
CREATE TABLE "SaleTravelers" (
    "SaleTravelerID" SERIAL PRIMARY KEY,
    "SaleID" INT NOT NULL,
    "FirstName" VARCHAR(255) NOT NULL,
    "LastName" VARCHAR(255) NOT NULL,
    "Age" INT,
    "SpecialRequirements" TEXT,
    "IsPrimary" BOOLEAN DEFAULT FALSE,
    FOREIGN KEY ("SaleID") REFERENCES "Sales"("SaleID") ON DELETE CASCADE
);

-- Table for SaleProducts
CREATE TABLE "SaleProducts" (
    "SaleProductID" SERIAL PRIMARY KEY,
    "SaleID" INT NOT NULL,
    "ProductID" INT NOT NULL,
    "ProviderID" INT,
    "BookingID" VARCHAR(50) NOT NULL,
    "BookingDate" DATE,
    "CheckinDate" DATE,
    "CheckoutDate" DATE,
    "Currency" VARCHAR(10) NOT NULL DEFAULT 'USD',
    "BasePrice" DECIMAL(10, 2) NOT NULL,
    "FinalPrice" DECIMAL(10, 2) NOT NULL,
    "PaymentDueDate" DATE,
    "Commission" DECIMAL(10, 2) NOT NULL,
    "Status" sale_product_status DEFAULT 'Active',
    "CancellationReason" TEXT,
    "CancellationDate" TIMESTAMP,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY ("SaleID") REFERENCES "Sales"("SaleID") ON DELETE CASCADE,
    FOREIGN KEY ("ProductID") REFERENCES "Products"("ProductID") ON DELETE CASCADE,
    FOREIGN KEY ("ProviderID") REFERENCES "Providers"("ProviderID") ON DELETE SET NULL,
    CHECK ("Currency" IN ('USD', 'ARS'))
);

-- Table for Payments
CREATE TABLE "Payments" (
    "PaymentID" SERIAL PRIMARY KEY,
    "SaleProductID" INT NOT NULL,
    "Currency" VARCHAR(10) NOT NULL DEFAULT 'USD',
    "PaymentAmount" DECIMAL(10, 2) NOT NULL,
    "PaymentDate" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    "PaymentMethod" VARCHAR(50),
    FOREIGN KEY ("SaleProductID") REFERENCES "SaleProducts"("SaleProductID") ON DELETE CASCADE,
    CHECK ("Currency" IN ('USD', 'ARS'))
);

-- Table for CalendarEvents
CREATE TABLE "CalendarEvents" (
    "EventID" SERIAL PRIMARY KEY,
    "UserID" INT NOT NULL,
    "EventType" VARCHAR(50) NOT NULL,
    "Title" VARCHAR(255) NOT NULL,
    "Description" TEXT,
    "StartDateTime" TIMESTAMP NOT NULL,
    "EndDateTime" TIMESTAMP,
    "IsCustom" BOOLEAN DEFAULT FALSE,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY ("UserID") REFERENCES "Users"("UserID") ON DELETE CASCADE
);

-- Table for Notifications
CREATE TABLE "Notifications" (
    "NotificationID" SERIAL PRIMARY KEY,
    "UserID" INT NOT NULL,
    "NotificationType" VARCHAR(50) NOT NULL,
    "Message" TEXT NOT NULL,
    "IsRead" BOOLEAN DEFAULT FALSE,
    "EventID" INT,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY ("UserID") REFERENCES "Users"("UserID") ON DELETE CASCADE,
    FOREIGN KEY ("EventID") REFERENCES "CalendarEvents"("EventID") ON DELETE SET NULL
);