-- Create Enums
CREATE TYPE user_type AS ENUM ('Agent', 'Agency', 'Admin');
CREATE TYPE contact_status AS ENUM ('New', 'InProgress', 'Won', 'Future', 'Lost');
CREATE TYPE product_type AS ENUM ('Package', 'Hotel', 'Ticket', 'Activity', 'Car', 'Insurance', 'Flight');


-- Table for Contacts Status localization
CREATE TABLE "ContactStatusTranslation" (
    "ContactStatus" contact_status PRIMARY KEY,
    "DisplayNameEN" TEXT NOT NULL,
    "DisplayNameES" TEXT NOT NULL
);

INSERT INTO "ContactStatusTranslation" ("ContactStatus", "DisplayNameEN", "DisplayNameES") VALUES
('New', 'New', 'Nuevo'),
('InProgress', 'In Progress', 'En Progreso'),
('Won', 'Won', 'Ganado'),
('Future', 'Future', 'Futuro'),
('Lost', 'Lost', 'Perdido');

-- Table for Product Type localization
CREATE TABLE "ProductTypeTranslation" (
    "ProductType" product_type PRIMARY KEY,
    "DisplayNameEN" TEXT NOT NULL,
    "DisplayNameES" TEXT NOT NULL
);

INSERT INTO "ProductTypeTranslation" ("ProductType", "DisplayNameEN", "DisplayNameES") VALUES
('Package', 'Package', 'Paquete'),
('Hotel', 'Hotel', 'Hotel'),
('Ticket', 'Ticket', 'Boleto'),
('Activity', 'Activity', 'Actividad'),
('Car', 'Car', 'Auto'),
('Insurance', 'Insurance', 'Seguro'),
('Flight', 'Flight', 'Vuelo');


--
--

-- Table for Users
CREATE TABLE "User" (
    "UserId" SERIAL PRIMARY KEY,
    "AgencyUserId" INT NULL, 
    "UserType" user_type NOT NULL DEFAULT 'Agent',
    "IdpId" TEXT NOT NULL UNIQUE,
    "Email" TEXT NOT NULL UNIQUE,
    "UserName" TEXT,
    "UserContactInfo" TEXT,
    "UserPicturePath" TEXT,
    "AgencyName" TEXT,
    "AgencyContactInfo" TEXT,
    "AgencyPicturePath" TEXT,
    "AboutMeMessage" TEXT,
    "WelcomeMessage" TEXT,
    "BudgetMessage" TEXT,
    "ThanksMessage" TEXT,
    "CreatedAt" TIMESTAMP NOT NULL,
    "UpdatedAt" TIMESTAMP,
    "IsDisabled" BOOLEAN DEFAULT FALSE,
    FOREIGN KEY ("AgencyUserId") REFERENCES "User"("UserId")
);

-- Table for Products
CREATE TABLE "Product" (
    "ProductId" SERIAL PRIMARY KEY,
    "UserId" INT NOT NULL,
    "ProductType" product_type NOT NULL,
    "ProductName" TEXT NOT NULL,
    "ProductDetails" TEXT NULL,
    "IsShared" BOOLEAN DEFAULT FALSE,
    "CreatedAt" TIMESTAMP NOT NULL,
    "UpdatedAt" TIMESTAMP,
    "IsDisabled" BOOLEAN DEFAULT FALSE,
    FOREIGN KEY ("UserId") REFERENCES "User"("UserId")
);

-- Table for Providers
CREATE TABLE "Provider" (
    "ProviderId" SERIAL PRIMARY KEY,
    "UserId" INT NOT NULL,
    "ProviderName" TEXT NOT NULL,
	"ProviderDetails" TEXT NOT NULL,
    "IsShared" BOOLEAN DEFAULT FALSE,
    "CreatedAt" TIMESTAMP NOT NULL,
    "UpdatedAt" TIMESTAMP,
    "IsDisabled" BOOLEAN DEFAULT FALSE,
    FOREIGN KEY ("UserId") REFERENCES "User"("UserId")
);

-- Table for Contacts Sources
CREATE TABLE "ContactSource" (
    "ContactSourceId" SERIAL PRIMARY KEY,
    "ContactSourceName" TEXT NOT NULL
);

INSERT INTO "ContactSource" ("ContactSourceName") VALUES 
('WhatsApp'),
('Instagram'),
('Website'),
('Paxy');

-- Table for Contacts
CREATE TABLE "Contact" (
    "ContactId" SERIAL PRIMARY KEY,
    "UserId" INT NOT NULL,
    "ContactStatus" contact_status NOT NULL DEFAULT 'New',
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
    "ContactSourceId" INT,
    "CreatedAt" TIMESTAMP NOT NULL,
    "UpdatedAt" TIMESTAMP,
    "IsDisabled" BOOLEAN DEFAULT FALSE,
    FOREIGN KEY ("UserId") REFERENCES "User"("UserId"),
    FOREIGN KEY ("ContactSourceId") REFERENCES "ContactSource"("ContactSourceId")
);

-- Table for ContactProducts
CREATE TABLE "ContactProduct" (
    "ContactProductId" SERIAL PRIMARY KEY,
    "ContactId" INT NOT NULL,
    "ProductId" INT NOT NULL,
    FOREIGN KEY ("ContactId") REFERENCES "Contact"("ContactId"),
    FOREIGN KEY ("ProductId") REFERENCES "Product"("ProductId")
);

-- Table for Budgets
CREATE TABLE "Budget" (
    "BudgetId" SERIAL PRIMARY KEY,
    "UserId" INT NOT NULL,    
    "ContactId" INT NOT NULL,
    "BudgetGUID" UUId NOT NULL DEFAULT gen_random_uuid(),
    "BudgetName" TEXT NOT NULL,
    "BudgetDetails" TEXT,
    "Adults" INT DEFAULT 0,
    "Kids" INT DEFAULT 0,
    "KidsAges" TEXT,
    "IsSent" BOOLEAN DEFAULT FALSE,
    "SentDate" DATE,
    "CreatedAt" TIMESTAMP NOT NULL,
    "UpdatedAt" TIMESTAMP,
    FOREIGN KEY ("UserId") REFERENCES "User"("UserId"),
    FOREIGN KEY ("ContactId") REFERENCES "Contact"("ContactId")
);

-- Table for Budget Products
CREATE TABLE "BudgetProduct" (
    "BudgetProductId" SERIAL PRIMARY KEY,
    "BudgetId" INT NOT NULL,
    "ProductId" INT NOT NULL,
    "ProductDetails" TEXT,
    "ProviderId" INT NOT NULL,
    "BudgetDate" DATE NOT NULL,
    "FromDate" DATE,
    "ToDate" DATE,
    "Currency" TEXT NOT NULL DEFAULT 'USD',
    "BasePrice" DECIMAL(10, 2),
    "FinalPrice" DECIMAL(10, 2) NOT NULL,
    "Commission" DECIMAL(10, 2),
    "BookingId" TEXT,
    "BookingDate" DATE,
    "ExpirationDate" DATE,
    "FilePath" TEXT,
    "CreatedAt" TIMESTAMP NOT NULL,
    "UpdatedAt" TIMESTAMP,
    FOREIGN KEY ("BudgetId") REFERENCES "Budget"("BudgetId"),
    FOREIGN KEY ("ProductId") REFERENCES "Product"("ProductId"),
    FOREIGN KEY ("ProviderId") REFERENCES "Provider"("ProviderId"),
    CHECK ("Currency" IN ('USD', 'AR$'))
);

-- Table for Sales
CREATE TABLE "Sale" (
    "SaleId" SERIAL PRIMARY KEY,
    "UserId" INT NOT NULL,
    "ContactId" INT NOT NULL,
    "SaleGUID" UUId DEFAULT gen_random_uuid(),
    "SaleName" TEXT NOT NULL,
    "SaleDetails" TEXT,
    "StartDate" DATE,
    "EndDate" DATE,
    "Adults" INT DEFAULT 0,
    "Kids" INT DEFAULT 0,
    "KidsAges" TEXT,
    "IsSent" BOOLEAN DEFAULT FALSE,
    "SentDate" DATE,
    "CreatedAt" TIMESTAMP NOT NULL,
    "UpdatedAt" TIMESTAMP,
    FOREIGN KEY ("UserId") REFERENCES "User"("UserId"),
    FOREIGN KEY ("ContactId") REFERENCES "Contact"("ContactId")
);

-- Table for Sale Travelers
CREATE TABLE "SaleTraveler" (
    "SaleTravelerId" SERIAL PRIMARY KEY,
    "SaleId" INT NOT NULL,
    "FirstName" TEXT NOT NULL,
    "LastName" TEXT NOT NULL,
    "Age" INT,
    "SpecialRequirements" TEXT,
    "IsPrimary" BOOLEAN DEFAULT FALSE,
    "CreatedAt" TIMESTAMP NOT NULL,
    "UpdatedAt" TIMESTAMP,
    FOREIGN KEY ("SaleId") REFERENCES "Sale"("SaleId")
);

-- Table for Sale Products
CREATE TABLE "SaleProduct" (
    "SaleProductId" SERIAL PRIMARY KEY,
    "SaleId" INT NOT NULL,
    "ProductId" INT NOT NULL,
    "ProductDetails" TEXT,
    "ProviderId" INT NOT NULL,
    "SaleDate" DATE NOT NULL,
    "FromDate" DATE,
    "ToDate" DATE,
    "Currency" TEXT NOT NULL DEFAULT 'USD',
    "BasePrice" DECIMAL(10, 2) NOT NULL,
    "FinalPrice" DECIMAL(10, 2) NOT NULL,
    "Commission" DECIMAL(10, 2) NOT NULL,
    "BookingId" TEXT NOT NULL,
    "BookingDate" DATE,
    "PaymentDueDate" DATE,
    "IsCancelled" BOOLEAN DEFAULT FALSE,
    "CancellationReason" TEXT,
    "CancellationDate" DATE,
    "IsFullPaid" BOOLEAN DEFAULT FALSE,
    "FullPaidDate" DATE NULL,
    "IsComissionPaid" BOOLEAN DEFAULT FALSE,
    "ComissionPaidDate" DATE NULL,
    "CreatedAt" TIMESTAMP NOT NULL,
    "UpdatedAt" TIMESTAMP,
    FOREIGN KEY ("SaleId") REFERENCES "Sale"("SaleId"),
    FOREIGN KEY ("ProductId") REFERENCES "Product"("ProductId"),
    FOREIGN KEY ("ProviderId") REFERENCES "Provider"("ProviderId"),
    CHECK ("Currency" IN ('USD', 'AR$'))
);

-- Table for Sale Products Payments
CREATE TABLE "SaleProductPayment" (
    "SaleProductPaymentId" SERIAL PRIMARY KEY,
    "SaleProductId" INT NOT NULL,
    "Currency" TEXT NOT NULL DEFAULT 'USD',
    "PaymentAmount" DECIMAL(10, 2) NOT NULL,
    "PaymentDate" DATE,
    "PaymentMethod" TEXT,
    "CreatedAt" TIMESTAMP NOT NULL,
    "UpdatedAt" TIMESTAMP,
    FOREIGN KEY ("SaleProductId") REFERENCES "SaleProduct"("SaleProductId"),
    CHECK ("Currency" IN ('USD', 'AR$'))
);

/*
TO REVIEW:
* Maybe add only one table for "Reminders"
* Reminders for payments
* Reminders for reservations
* Reminders for new sales
* Etc 

-- Table for Events
CREATE TABLE "Events" (
    "EventId" SERIAL PRIMARY KEY,
    "UserId" INT NOT NULL,
    "EventType" TEXT NOT NULL,
    "Title" TEXT NOT NULL,
    "Description" TEXT,
    "StartDate" DATE NOT NULL,
    "EndDate" DATE,
    "IsCustom" BOOLEAN DEFAULT FALSE,
    "CreatedAt" TIMESTAMP NOT NULL ,
    "UpdatedAt" TIMESTAMP ,
    FOREIGN KEY ("UserId") REFERENCES "Users"("UserId")
);

-- Table for Notifications
CREATE TABLE "Notifications" (
    "NotificationId" SERIAL PRIMARY KEY,
    "UserId" INT NOT NULL,
    "NotificationType" TEXT NOT NULL,
    "Message" TEXT NOT NULL,
    "IsRead" BOOLEAN DEFAULT FALSE,
    "EventId" INT,
    "CreatedAt" TIMESTAMP NOT NULL ,
    "UpdatedAt" TIMESTAMP ,
    FOREIGN KEY ("UserId") REFERENCES "Users"("UserId"),
    FOREIGN KEY ("EventId") REFERENCES "Events"("EventId")
);
*/

/*
TO REVIEW:
* Review how to handle subscriptions, 3rd party tools, etc

CREATE TYPE subscription_status AS ENUM ('Active', 'Canceled', 'Expired');

-- Table for Subscriptions
CREATE TABLE "Subscriptions" (
    "SubscriptionId" SERIAL PRIMARY KEY,
    "UserId" INT NOT NULL,
    "PlanName" TEXT NOT NULL,
    "StartDate" DATE NOT NULL,
    "EndDate" DATE NOT NULL,
    "Status" subscription_status NOT NULL DEFAULT 'Active', 
    "CreatedAt" TIMESTAMP NOT NULL ,
    "UpdatedAt" TIMESTAMP ,
    FOREIGN KEY ("UserId") REFERENCES "Users"("UserId")
);
*/
