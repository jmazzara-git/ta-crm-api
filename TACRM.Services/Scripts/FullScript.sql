-- CRM Db Model

-- Create schema for the project
CREATE SCHEMA ta_crm;

-- Create database user
CREATE USER ta_crm_user WITH PASSWORD 'SecureP@ss123';
GRANT CONNECT ON DATABASE ta_crm TO ta_crm_user;
GRANT USAGE ON SCHEMA ta_crm TO ta_crm_user;
GRANT ALL PRIVILEGES ON SCHEMA ta_crm TO ta_crm_user;

-- Set default schema for this script
SET search_path TO ta_crm;

-- Table for Agencies
CREATE TABLE Agencies (
    AgencyID SERIAL PRIMARY KEY,
    AgencyName VARCHAR(255) NOT NULL,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Table for Users
CREATE TABLE Users (
    UserID SERIAL PRIMARY KEY,
    AgencyID INT, -- Foreign key to Agencies table
    Email VARCHAR(255) NOT NULL UNIQUE, -- Email registered in the 3rd party identity manager
    FullName VARCHAR(255),
    UserType VARCHAR(50) NOT NULL DEFAULT 'AGENT', -- UserType: AGENT (default), AGENCY, or ADMIN
    DefaultBudgetMessage TEXT, -- Default message to be included in budgets
    DefaultWelcomeMessage TEXT, -- Default message for public contact form
    DefaultThanksMessage TEXT, -- Default thank-you message for public pages
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (AgencyID) REFERENCES Agencies(AgencyID) ON DELETE SET NULL
);



-- Table for Subscriptions
CREATE TABLE Subscriptions (
    SubscriptionID SERIAL PRIMARY KEY,
    UserID INT NOT NULL, -- Foreign key to Users table
    StartDate DATE NOT NULL, -- Subscription start date
    EndDate DATE, -- Subscription end date, NULL if active
    Status VARCHAR(50) NOT NULL, -- Subscription status: Active, Cancelled, Expired
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE
);

-- Table for Contacts
CREATE TABLE Contacts (
    ContactID SERIAL PRIMARY KEY,
    UserID INT NOT NULL, -- Foreign key to Users table
    ContactSourceID INT, -- Foreign key to ContactSources table
    StatusID INT, -- Foreign key to ContactStatuses table
    Name VARCHAR(255) NOT NULL,
    Email VARCHAR(255),
    Phone VARCHAR(50),
    TravelDateStart DATE,
    TravelDateEnd DATE,
    Adults INT DEFAULT 0, -- Number of adults in the party
    Kids INT DEFAULT 0, -- Number of kids in the party
    KidsAges TEXT, -- JSON or comma-separated list of ages
    Comments TEXT, -- Free-text additional information about the contact
    EnableWhatsAppNotifications BOOLEAN DEFAULT FALSE, -- Flag for WhatsApp notifications
    EnableEmailNotifications BOOLEAN DEFAULT FALSE, -- Flag for email notifications
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE,
    FOREIGN KEY (ContactSourceID) REFERENCES ContactSources(ContactSourceID) ON DELETE SET NULL,
    FOREIGN KEY (StatusID) REFERENCES ContactStatuses(StatusID) ON DELETE SET NULL
);


-- Table for ContactSources
CREATE TABLE ContactSources (
    ContactSourceID SERIAL PRIMARY KEY,
    SourceName VARCHAR(255) NOT NULL -- e.g., Instagram, WhatsApp, TikTok
);

-- Table for ContactStatuses
CREATE TABLE ContactStatuses (
    StatusID SERIAL PRIMARY KEY,
    Key VARCHAR(50) NOT NULL UNIQUE -- Internal identifier for status
);

CREATE TABLE ContactStatusTranslations (
    TranslationID SERIAL PRIMARY KEY,
    StatusID INT NOT NULL, -- Foreign key to ContactStatuses
    LanguageCode VARCHAR(5) NOT NULL, -- e.g., 'en', 'es'
    DisplayName VARCHAR(255) NOT NULL, -- Translated name
    FOREIGN KEY (StatusID) REFERENCES ContactStatuses(StatusID) ON DELETE CASCADE
);

INSERT INTO ContactStatuses (Key) VALUES ('New'), ('InProgress'), ('Converted'), ('Future'), ('Lost');

INSERT INTO ContactStatusTranslations (StatusID, LanguageCode, DisplayName)
VALUES
(1, 'en', 'New'),
(1, 'es', 'Nuevo'),
(2, 'en', 'In Progress'),
(2, 'es', 'En Progreso'),
(3, 'en', 'Converted'),
(3, 'es', 'Convertido'),
(4, 'en', 'Future'),
(4, 'es', 'Futuro'),
(5, 'en', 'Lost'),
(6, 'es', 'Perdido');


-- Table for ContactProductInterest
CREATE TABLE ContactProductInterest (
    ContactProductInterestID SERIAL PRIMARY KEY,
    ContactID INT NOT NULL, -- Foreign key to Contacts table
    ProductID INT NOT NULL, -- Foreign key to Products table
    FOREIGN KEY (ContactID) REFERENCES Contacts(ContactID) ON DELETE CASCADE,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID) ON DELETE CASCADE
);

-- Table for Products
CREATE TABLE Products (
    ProductID SERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    ProductType VARCHAR(50) NOT NULL -- Types: Package, Hotel, Tickets, Car Rental, Insurance
);

-- Table for UserProducts
CREATE TABLE UserProducts (
    UserProductID SERIAL PRIMARY KEY,
    UserID INT NOT NULL, -- Foreign key to Users table
    ProductID INT NOT NULL, -- Foreign key to Products table
    FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID) ON DELETE CASCADE
);


-- Table for Providers
CREATE TABLE Providers (
    ProviderID SERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    ContactInfo TEXT -- Additional information about the provider (e.g., phone, email)
);


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

