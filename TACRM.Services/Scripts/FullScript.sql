
-- Table for Agencies
CREATE TABLE "Agencies" (
    "AgencyID" SERIAL PRIMARY KEY,
    "AgencyName" VARCHAR(255) NOT NULL,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Table for Users
CREATE TABLE "Users" (
    "UserID" SERIAL PRIMARY KEY, -- Unique identifier for each user
    "AgencyID" INT, -- Foreign key to Agencies table
    "Email" VARCHAR(255) NOT NULL UNIQUE, -- Email address of the user
    "FullName" VARCHAR(255), -- Full name of the user
    "UserType" VARCHAR(50) NOT NULL DEFAULT 'AGENT', -- User type: AGENT (default), AGENCY, ADMIN
    "DefaultBudgetMessage" TEXT, -- Default message to include in all budgets
    "DefaultWelcomeMessage" TEXT, -- Default message displayed in the public form for new contacts
    "DefaultThanksMessage" TEXT, -- Default thank-you message for the public page for a sale
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP, -- Timestamp when the user was created
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP, -- Timestamp when the user was last updated
    FOREIGN KEY ("AgencyID") REFERENCES "Agencies"("AgencyID") ON DELETE SET NULL -- Reference to Agencies table
);


-- Table for Subscriptions
CREATE TABLE "Subscriptions" (
    "SubscriptionID" SERIAL PRIMARY KEY, -- Unique identifier for each subscription
    "UserID" INT NOT NULL, -- Foreign key to Users table
    "StartDate" TIMESTAMP NOT NULL, -- The start date of the subscription
    "EndDate" TIMESTAMP NOT NULL, -- The end date of the subscription
    "Status" VARCHAR(50) NOT NULL DEFAULT 'Active', -- Subscription status: Active, Canceled, or Expired
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP, -- Timestamp when the subscription was created
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP, -- Timestamp when the subscription was last updated
    FOREIGN KEY ("UserID") REFERENCES "Users"("UserID") ON DELETE CASCADE -- Reference to Users table
);

--
--

-- Table for Product Types
CREATE TABLE "ProductType" (
    "ProductTypeID" SERIAL PRIMARY KEY, -- Unique identifier for each product type
    "ProductTypeKey" VARCHAR(50) NOT NULL UNIQUE -- Key identifier for the product type (e.g., Package, Hotel)
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
    "TranslationID" SERIAL PRIMARY KEY, -- Unique identifier for each translation
    "ProductTypeID" INT NOT NULL, -- Foreign key to ProductType table
    "LanguageCode" VARCHAR(5) NOT NULL, -- Language code (e.g., 'en', 'es')
    "DisplayName" VARCHAR(255) NOT NULL, -- Localized name of the product type
    FOREIGN KEY ("ProductTypeID") REFERENCES "ProductType"("ProductTypeID") ON DELETE CASCADE -- Reference to ProductType table
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
    "ProductID" SERIAL PRIMARY KEY, -- Unique identifier for each product
    "UserID" INT, -- Foreign key to Users table (nullable for shared products)
    "ProductTypeID" INT NOT NULL, -- Foreign key to ProductType table
    "ProductName" VARCHAR(255) NOT NULL, -- Name of the product
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP, -- Timestamp when the product was created
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP, -- Timestamp when the product was last updated
    FOREIGN KEY ("ProductTypeID") REFERENCES "ProductType"("ProductTypeID") ON DELETE CASCADE, -- Reference to ProductType
    FOREIGN KEY ("UserID") REFERENCES "Users"("UserID") ON DELETE SET NULL -- Reference to Users table
);

-- Table for Providers
CREATE TABLE "Providers" (
    "ProviderID" SERIAL PRIMARY KEY, -- Unique identifier for each provider
    "UserID" INT, -- Foreign key to Users table (nullable for shared providers)
    "ProviderName" VARCHAR(255) NOT NULL, -- Name of the provider
    "ContactInfo" TEXT, -- Additional contact information for the provider (e.g., phone, email)
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP, -- Timestamp when the provider was created
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP, -- Timestamp when the provider was last updated
    FOREIGN KEY ("UserID") REFERENCES "Users"("UserID") ON DELETE SET NULL -- Reference to Users table
);


--
--

-- Table for Contacts Sources
CREATE TABLE "ContactSource" (
    "ContactSourceID" SERIAL PRIMARY KEY, -- Unique identifier for each contact source
    "ContactSourceName" VARCHAR(255) NOT NULL -- Name of the contact source (e.g., Instagram, WhatsApp)
);

INSERT INTO "ContactSource" ("ContactSourceName") VALUES 
('WhatsApp'),
('Instagram'),
('Website'),
('Other')

-- Table for Contacts Statuses
CREATE TABLE "ContactStatus" (
    "ContactStatusID" SERIAL PRIMARY KEY, -- Unique identifier for each contact status
    "ContactStatusKey" VARCHAR(50) NOT NULL UNIQUE -- Internal key for the status (e.g., New, InProgress, Converted)
);

INSERT INTO "ContactStatus" ("ContactStatusKey") VALUES 
('New'), 
('InProgress'), 
('Converted'), 
('Future'), 
('Lost');

-- Table for Contacts Statuses localization
CREATE TABLE "ContactStatusTranslations" (
    "TranslationID" SERIAL PRIMARY KEY, -- Unique identifier for each translation
    "ContactStatusID" INT NOT NULL, -- Foreign key to ContactStatus table
    "LanguageCode" VARCHAR(5) NOT NULL, -- Language code (e.g., 'en', 'es')
    "DisplayName" VARCHAR(255) NOT NULL, -- Localized display name for the status
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
    "ContactID" SERIAL PRIMARY KEY, -- Unique identifier for each contact
    "UserID" INT NOT NULL, -- Foreign key to Users table
    "ContactSourceID" INT, -- Foreign key to ContactSource table
    "ContactStatusID" INT NOT NULL, -- Foreign key to ContactStatus table
    "FullName" VARCHAR(255) NOT NULL, -- Full name of the contact
    "Email" VARCHAR(255), -- Email address of the contact
    "Phone" VARCHAR(50), -- Phone number of the contact
    "TravelDateStart" DATE, -- Planned travel start date
    "TravelDateEnd" DATE, -- Planned travel end date
    "Adults" INT DEFAULT 0, -- Number of adults in the party
    "Kids" INT DEFAULT 0, -- Number of kids in the party
    "KidsAges" TEXT, -- Comma-separated list of ages for children
    "Comments" TEXT, -- Additional free-text information about the contact
    "EnableWhatsAppNotifications" BOOLEAN DEFAULT FALSE, -- Flag for WhatsApp notifications
    "EnableEmailNotifications" BOOLEAN DEFAULT FALSE, -- Flag for email notifications
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP, -- Timestamp when the contact was created
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP, -- Timestamp when the contact was last updated
    FOREIGN KEY ("UserID") REFERENCES "Users"("UserID") ON DELETE CASCADE,
    FOREIGN KEY ("ContactSourceID") REFERENCES "ContactSource"("ContactSourceID") ON DELETE SET NULL,
    FOREIGN KEY ("ContactStatusID") REFERENCES "ContactStatus"("ContactStatusID") ON DELETE SET NULL
);

-- Table for ContactProductInterest
CREATE TABLE "ContactProductInterest" (
    "ContactProductInterestID" SERIAL PRIMARY KEY, -- Unique identifier for each contact-product relationship
    "ContactID" INT NOT NULL, -- Foreign key to Contacts table
    "ProductID" INT NOT NULL, -- Foreign key to Products table
    FOREIGN KEY ("ContactID") REFERENCES "Contacts"("ContactID") ON DELETE CASCADE,
    FOREIGN KEY ("ProductID") REFERENCES "Products"("ProductID") ON DELETE CASCADE
);


--
--

-- Table for Budgets
CREATE TABLE "Budgets" (
    "BudgetID" SERIAL PRIMARY KEY, -- Unique identifier for each budget
    "ContactID" INT NOT NULL, -- Foreign key to Contacts table
    "BudgetName" VARCHAR(255) NOT NULL, -- Name of the budget
    "BudgetDetails" TEXT, -- Free-text details for the budget
    "Adults" INT DEFAULT 0, -- Number of adults in the party
    "Kids" INT DEFAULT 0, -- Number of kids in the party
    "KidsAges" TEXT, -- Comma-separated list of ages for children
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP, -- Timestamp when the budget was created
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP, -- Timestamp when the budget was last updated
    FOREIGN KEY ("ContactID") REFERENCES "Contacts"("ContactID") ON DELETE CASCADE
);


-- Table for BudgetProducts
CREATE TABLE "BudgetProducts" (
    "BudgetProductID" SERIAL PRIMARY KEY, -- Unique identifier for each budget-product relationship
    "BudgetID" INT NOT NULL, -- Foreign key to Budgets table
    "ProductID" INT NOT NULL, -- Foreign key to Products table
    "ProductDetails" TEXT, -- Free-text details about the product in the budget
    "BudgetDate" DATE, -- Date when the product budget was created
    "CheckinDate" DATE, -- Check-in date for the product
    "CheckoutDate" DATE, -- Check-out date for the product
    "ProviderID" INT, -- Foreign key to Providers table 
    "Currency" VARCHAR(10) NOT NULL DEFAULT 'USD', -- Currency of the product price
    "BasePrice" DECIMAL(10, 2), -- Base price before taxes
    "FinalPrice" DECIMAL(10, 2) NOT NULL, -- Final price including taxes
    "Commission" DECIMAL(10, 2), -- Commission for the product
    "BookingID" VARCHAR(50), -- Unique identifier for the booking
    "BookingDate" DATE, -- Date when the temporary booking was made
    "ExpirationDate" DATE, -- Expiration date of the temporary reservation
    "FilePath" TEXT, -- Path to an uploaded file
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP, -- Timestamp when the budget-product entry was created
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP, -- Timestamp when the budget-product entry was last updated
    FOREIGN KEY ("BudgetID") REFERENCES "Budgets"("BudgetID") ON DELETE CASCADE,
    FOREIGN KEY ("ProductID") REFERENCES "Products"("ProductID") ON DELETE CASCADE,
    FOREIGN KEY ("ProviderID") REFERENCES "Providers"("ProviderID") ON DELETE SET NULL,
    CHECK ("Currency" IN ('USD', 'ARS')) -- Valid currencies
);

--
--

-- Table for Sales
CREATE TABLE "Sales" (
    "SaleID" SERIAL PRIMARY KEY,
    "UserID" INT NOT NULL, -- Foreign key to Users table
    "ContactID" INT NOT NULL, -- Foreign key to Contacts table
    "SaleGUID" UUID DEFAULT gen_random_uuid(), -- Public GUID for customer access
    "SaleName" VARCHAR(255) NOT NULL, -- Name of the sale
    "SaleDetails" TEXT, -- Free-text details for the budget
    "Adults" INT DEFAULT 0, -- Number of adults in the party
    "Kids" INT DEFAULT 0, -- Number of kids in the party
    "KidsAges" TEXT, -- Comma-separated list of ages for children
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY ("UserID") REFERENCES "Users"("UserID") ON DELETE CASCADE,
    FOREIGN KEY ("ContactID") REFERENCES "Contacts"("ContactID") ON DELETE CASCADE
);

-- Table for SaleTravelers
CREATE TABLE "SaleTravelers" (
    "SaleTravelerID" SERIAL PRIMARY KEY,
    "SaleID" INT NOT NULL, -- Foreign key to Sales table
    "FirstName" VARCHAR(255) NOT NULL, -- First name of the traveler
    "LastName" VARCHAR(255) NOT NULL, -- Last name of the traveler
    "Age" INT, -- Age of the traveler
    "SpecialRequirements" TEXT, -- Additional details for the traveler
    "IsPrimary" BOOLEAN DEFAULT FALSE, -- Indicates the primary traveler
    FOREIGN KEY ("SaleID") REFERENCES "Sales"("SaleID") ON DELETE CASCADE
);

-- Table for SaleProducts
CREATE TABLE "SaleProducts" (
    "SaleProductID" SERIAL PRIMARY KEY,
    "SaleID" INT NOT NULL, -- Foreign key to Sales table
    "ProductID" INT NOT NULL, -- Foreign key to Products table
    "ProviderID" INT, -- Foreign key to Providers table
    "BookingID" VARCHAR(50) NOT NULL, -- Unique booking identifier
    "BookingDate" DATE, -- Date when the booking was made
    "CheckinDate" DATE, -- Check-in date for the product
    "CheckoutDate" DATE, -- Check-out date for the product
    "Currency" VARCHAR(10) NOT NULL DEFAULT 'USD', -- Currency of the product price
    "BasePrice" DECIMAL(10, 2) NOT NULL, -- Base price before taxes
    "FinalPrice" DECIMAL(10, 2) NOT NULL, -- Final price after taxes
    "PaymentDueDate" DATE, -- Payment due date
    "Commission" DECIMAL(10, 2) NOT NULL, -- Commission for the product
    "Status" VARCHAR(50) DEFAULT 'Active', -- Status of the product sale
    "CancellationReason" TEXT, -- Reason for cancellation
    "CancellationDate" TIMESTAMP, -- Date of cancellation
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP, -- Timestamp when the product entry was created
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP, -- Timestamp when the product entry was last updated,
    FOREIGN KEY ("SaleID") REFERENCES "Sales"("SaleID") ON DELETE CASCADE,
    FOREIGN KEY ("ProductID") REFERENCES "Products"("ProductID") ON DELETE CASCADE,
    FOREIGN KEY ("ProviderID") REFERENCES "Providers"("ProviderID") ON DELETE SET NULL,
    CHECK ("Currency" IN ('USD', 'ARS')) -- Check constraint for allowed currencies
);

-- Table for Payments
CREATE TABLE "Payments" (
    "PaymentID" SERIAL PRIMARY KEY,
    "SaleProductID" INT NOT NULL, -- Foreign key to SaleProducts table
    "Currency" VARCHAR(10) NOT NULL DEFAULT 'USD', -- Currency of the payment
    "PaymentAmount" DECIMAL(10, 2) NOT NULL, -- Payment amount
    "PaymentDate" TIMESTAMP DEFAULT CURRENT_TIMESTAMP, -- Date of the payment
    "PaymentMethod" VARCHAR(50), -- Method of payment
    FOREIGN KEY ("SaleProductID") REFERENCES "SaleProducts"("SaleProductID") ON DELETE CASCADE,
    CHECK ("Currency" IN ('USD', 'ARS')) -- Check constraint for allowed currencies
);


--
--

-- Table for CalendarEvents
CREATE TABLE "CalendarEvents" (
    "EventID" SERIAL PRIMARY KEY, -- Unique identifier for each event
    "UserID" INT NOT NULL, -- Foreign key to Users table
    "EventType" VARCHAR(50) NOT NULL, -- Type of event (e.g., Trip, Payment Reminder, Custom)
    "Title" VARCHAR(255) NOT NULL, -- Title of the event
    "Description" TEXT, -- Optional details about the event
    "StartDateTime" TIMESTAMP NOT NULL, -- Start date and time of the event
    "EndDateTime" TIMESTAMP, -- End date and time of the event
    "IsCustom" BOOLEAN DEFAULT FALSE, -- Indicates if the event is user-created or system-generated
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP, -- Event creation timestamp
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP, -- Event last updated timestamp
    FOREIGN KEY ("UserID") REFERENCES "Users"("UserID") ON DELETE CASCADE
);


-- Table for Notifications
CREATE TABLE "Notifications" (
    "NotificationID" SERIAL PRIMARY KEY, -- Unique identifier for each notification
    "UserID" INT NOT NULL, -- Foreign key to Users table
    "NotificationType" VARCHAR(50) NOT NULL, -- Type of notification (e.g., Payment Reminder, Inactive Contact)
    "Message" TEXT NOT NULL, -- Notification message
    "IsRead" BOOLEAN DEFAULT FALSE, -- Flag to indicate if the notification has been read
    "EventID" INT, -- Optional foreign key to CalendarEvents table
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP, -- Notification creation timestamp
    "UpdatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP, -- Notification last updated timestamp
    FOREIGN KEY ("UserID") REFERENCES "Users"("UserID") ON DELETE CASCADE,
    FOREIGN KEY ("EventID") REFERENCES "CalendarEvents"("EventID") ON DELETE SET NULL
);

