
-- Table for Agencies
CREATE TABLE "Agencies" (
    "AgencyID" SERIAL PRIMARY KEY,
    "AgencyName" VARCHAR(255) NOT NULL,
    "CreatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
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
    "CreatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP, -- Timestamp when the user was created
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
    "CreatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP, -- Timestamp when the subscription was created
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
    "CreatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP, -- Timestamp when the product was created
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
    "CreatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP, -- Timestamp when the provider was created
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
    "ContactStatusID" INT, -- Foreign key to ContactStatus table
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
    "CreatedAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP, -- Timestamp when the contact was created
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
CREATE TABLE Budgets (
    BudgetID SERIAL PRIMARY KEY,
    ContactID INT NOT NULL, -- Foreign key to Contacts table
    BudgetName VARCHAR(255) NOT NULL,
    BudgetDetails TEXT, -- Free-text details for the budget
    FilePath TEXT, -- Path to the uploaded budget file (e.g., PDF)
    Currency VARCHAR(10) NOT NULL DEFAULT 'USD', -- Currency for the budget
    TotalPrice DECIMAL(10, 2) NOT NULL DEFAULT 0.00, -- Total price of all products in the budget
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (ContactID) REFERENCES Contacts(ContactID) ON DELETE CASCADE,
    CONSTRAINT uq_contact_budgetname UNIQUE (ContactID, BudgetName), -- Unique budget names per contact
    CONSTRAINT chk_budget_currency CHECK (Currency IN ('USD', 'EUR', 'ARS')) -- Valid currencies
);

-- Table for BudgetProducts
CREATE TABLE BudgetProducts (
    BudgetProductID SERIAL PRIMARY KEY,
    BudgetID INT NOT NULL, -- Foreign key to Budgets table
    ProductID INT NOT NULL, -- Foreign key to Products table
    ProductDetails TEXT, -- Free-text details about the product in the budget
    Currency VARCHAR(10) NOT NULL DEFAULT 'USD', -- Currency of the product price
    BasePrice DECIMAL(10, 2) NOT NULL, -- Base price before taxes
    FinalPrice DECIMAL(10, 2) NOT NULL, -- Final price of the product after taxes
    ProviderID INT, -- Foreign key to Providers table, optional field
    BudgetDate DATE NOT NULL DEFAULT CURRENT_DATE, -- Date when the budget was made
    CheckinDate DATE, -- Check-in date for the product
    CheckoutDate DATE, -- Check-out date for the product
    FOREIGN KEY (BudgetID) REFERENCES Budgets(BudgetID) ON DELETE CASCADE,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID) ON DELETE CASCADE,
    FOREIGN KEY (ProviderID) REFERENCES Providers(ProviderID) ON DELETE SET NULL,
    CONSTRAINT chk_budgetproduct_currency CHECK (Currency IN ('USD', 'EUR', 'ARS')) -- Valid currencies
);


-- Table for Sales
CREATE TABLE Sales (
    SaleID SERIAL PRIMARY KEY,
    UserID INT NOT NULL, -- Foreign key to Users table, representing the owner of the sale
    ContactID INT NOT NULL, -- Foreign key to Contacts table
    SaleGUID UUID DEFAULT gen_random_uuid(), -- Public GUID for customer access
    SaleName VARCHAR(255) NOT NULL, -- Name of the sale, e.g., "Trip to Disney"
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE,
    FOREIGN KEY (ContactID) REFERENCES Contacts(ContactID) ON DELETE CASCADE
);

-- Table for SaleProducts
CREATE TABLE SaleProducts (
    SaleProductID SERIAL PRIMARY KEY,
    SaleID INT NOT NULL, -- Foreign key to Sales table
    ProductID INT NOT NULL, -- Foreign key to Products table
    ProviderID INT, -- Foreign key to Providers table, optional field
    BookingID VARCHAR(50) NOT NULL, -- Unique identifier for the booking
    BookingDate DATE, -- Date when the booking was made
    CheckinDate DATE, -- Check-in date for the product
    CheckoutDate DATE, -- Check-out date for the product
    Currency VARCHAR(10) NOT NULL DEFAULT 'USD', -- Currency of the product price
    BasePrice DECIMAL(10, 2) NOT NULL, -- Base price before taxes
    FinalPrice DECIMAL(10, 2) NOT NULL, -- Final price of the product after taxes
    PaymentDueDate DATE, -- Payment due date
    Commission DECIMAL(10, 2) NOT NULL, -- Commission for the product
    Status VARCHAR(50) DEFAULT 'Active', -- Status of the product sale (Active, Cancelled)
    CancellationReason TEXT, -- Reason for cancellation, if applicable
    CancellationDate TIMESTAMP, -- Date of cancellation, if applicable
    FOREIGN KEY (SaleID) REFERENCES Sales(SaleID) ON DELETE CASCADE,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID) ON DELETE CASCADE,
    FOREIGN KEY (ProviderID) REFERENCES Providers(ProviderID) ON DELETE SET NULL
);

-- Table for SaleTravelers
CREATE TABLE SaleTravelers (
    SaleTravelerID SERIAL PRIMARY KEY,
    SaleID INT NOT NULL, -- Foreign key to Sales table
    FirstName VARCHAR(255) NOT NULL, -- First name of the trip member
    LastName VARCHAR(255) NOT NULL, -- Last name of the trip member
    Age INT, -- Age of the trip member
    SpecialRequirements TEXT, -- Additional details for the member
    IsPrimary BOOLEAN DEFAULT FALSE, -- Indicates the primary traveler of the reservations
    FOREIGN KEY (SaleID) REFERENCES Sales(SaleID) ON DELETE CASCADE
);

-- Table for Payments
CREATE TABLE Payments (
    PaymentID SERIAL PRIMARY KEY,
    SaleProductID INT NOT NULL, -- Foreign key to SaleProducts table
    Currency VARCHAR(10) NOT NULL DEFAULT 'USD', -- Currency of the payment
    PaymentAmount DECIMAL(10, 2) NOT NULL, -- Payment amount
    PaymentDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP, -- Date of the payment
    PaymentMethod VARCHAR(50), -- Method of payment (e.g., Credit Card, Bank Transfer)
    FOREIGN KEY (SaleProductID) REFERENCES SaleProducts(SaleProductID) ON DELETE CASCADE
);

-- Table for CalendarEvents
CREATE TABLE CalendarEvents (
    EventID SERIAL PRIMARY KEY, -- Unique identifier for each event
    UserID INT NOT NULL, -- Foreign key to Users table, representing the owner of the calendar event
    EventType VARCHAR(50) NOT NULL, -- Type of event (e.g., Trip, Payment, Reminder)
    Title VARCHAR(255) NOT NULL, -- Title of the event
    Description TEXT, -- Optional details about the event
    StartDateTime TIMESTAMP NOT NULL, -- Start date and time of the event
    EndDateTime TIMESTAMP, -- End date and time of the event (if applicable)
    IsCustom BOOLEAN DEFAULT FALSE, -- Identifies user-created vs. system-generated events
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE
);

-- Table for Notifications
CREATE TABLE Notifications (
    NotificationID SERIAL PRIMARY KEY,
    UserID INT NOT NULL, -- Foreign key to Users table
    Message TEXT NOT NULL, -- Notification message
    Type VARCHAR(50) NOT NULL, -- Type of notification (e.g., Reminder, Alert)
    EntityID INT, -- Optional: ID of the related entity (e.g., SaleID, ContactID)
    EntityType VARCHAR(50), -- Optional: Type of the related entity (e.g., Sale, Contact)
    IsRead BOOLEAN DEFAULT FALSE, -- Whether the notification has been read
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP, -- Timestamp of the notification
    FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE
);
