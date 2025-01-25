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

-- Table for Tenants
CREATE TABLE Tenants (
    TenantID SERIAL PRIMARY KEY,
    TenantName VARCHAR(255) NOT NULL,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Table for Users
CREATE TABLE Users (
    UserID SERIAL PRIMARY KEY,
    TenantID INT NOT NULL, -- Foreign key to Tenants table
    Email VARCHAR(255) NOT NULL UNIQUE, -- Email registered in the 3rd party identity manager
    FullName VARCHAR(255),
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (TenantID) REFERENCES Tenants(TenantID) ON DELETE CASCADE
);

-- Table for Contacts
CREATE TABLE Contacts (
    ContactID SERIAL PRIMARY KEY,
    TenantID INT NOT NULL, -- Foreign key for multi-tenancy
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
    Comments TEXT, -- Additional free-text information about the contact
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CreatedBy INT, -- Foreign key to Users table, who created the contact
    UpdatedBy INT, -- Foreign key to Users table, who last updated the contact
    FOREIGN KEY (TenantID) REFERENCES Tenants(TenantID) ON DELETE CASCADE,
    FOREIGN KEY (ContactSourceID) REFERENCES ContactSources(ContactSourceID) ON DELETE SET NULL,
    FOREIGN KEY (StatusID) REFERENCES ContactStatuses(StatusID) ON DELETE SET NULL,
    FOREIGN KEY (CreatedBy) REFERENCES Users(UserID) ON DELETE SET NULL,
    FOREIGN KEY (UpdatedBy) REFERENCES Users(UserID) ON DELETE SET NULL
);

-- Table for ContactSources
CREATE TABLE ContactSources (
    ContactSourceID SERIAL PRIMARY KEY,
    SourceName VARCHAR(255) NOT NULL -- e.g., Instagram, WhatsApp, TikTok
);

-- Table for ContactStatuses
CREATE TABLE ContactStatuses (
    StatusID SERIAL PRIMARY KEY,
    StatusName VARCHAR(50) NOT NULL -- e.g., New, In Progress, Converted, Lost
);

-- Table for Products
CREATE TABLE Products (
    ProductID SERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL
);

-- Table for Providers
CREATE TABLE Providers (
    ProviderID SERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    ContactInfo TEXT -- Additional information about the provider (e.g., phone, email)
);

-- Table for ContactProductInterest
CREATE TABLE ContactProductInterest (
    ContactProductInterestID SERIAL PRIMARY KEY,
    ContactID INT NOT NULL,
    ProductID INT NOT NULL,
    FOREIGN KEY (ContactID) REFERENCES Contacts(ContactID) ON DELETE CASCADE,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID) ON DELETE CASCADE
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
    CreatedBy INT, -- Foreign key to Users table, who created the budget
    UpdatedBy INT, -- Foreign key to Users table, who last updated the budget
    FOREIGN KEY (ContactID) REFERENCES Contacts(ContactID) ON DELETE CASCADE,
    FOREIGN KEY (CreatedBy) REFERENCES Users(UserID) ON DELETE SET NULL,
    FOREIGN KEY (UpdatedBy) REFERENCES Users(UserID) ON DELETE SET NULL
);

-- Table for Sales
CREATE TABLE Sales (
    SaleID SERIAL PRIMARY KEY,
    TenantID INT NOT NULL, -- Foreign key for multi-tenancy
    ContactID INT NOT NULL, -- Foreign key to Contacts table
    SaleName VARCHAR(255) NOT NULL, -- Name of the sale, e.g., "Trip to Disney"
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CreatedBy INT, -- Foreign key to Users table, who created the sale
    UpdatedBy INT, -- Foreign key to Users table, who last updated the sale
    FOREIGN KEY (TenantID) REFERENCES Tenants(TenantID) ON DELETE CASCADE,
    FOREIGN KEY (ContactID) REFERENCES Contacts(ContactID) ON DELETE CASCADE,
    FOREIGN KEY (CreatedBy) REFERENCES Users(UserID) ON DELETE SET NULL,
    FOREIGN KEY (UpdatedBy) REFERENCES Users(UserID) ON DELETE SET NULL
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
    TenantID INT NOT NULL, -- Foreign key for multi-tenancy
    EventType VARCHAR(50) NOT NULL, -- Type of event (e.g., Trip, Payment, Reminder)
    Title VARCHAR(255) NOT NULL, -- Title of the event
    Description TEXT, -- Optional details about the event
    StartDateTime TIMESTAMP NOT NULL, -- Start date and time of the event
    EndDateTime TIMESTAMP, -- End date and time of the event (if applicable)
    IsCustom BOOLEAN DEFAULT FALSE, -- Identifies user-created vs. system-generated events
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP, -- Event creation timestamp
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP, -- Event last updated timestamp
    CreatedBy INT, -- Foreign key to Users table, who created the event
    FOREIGN KEY (TenantID) REFERENCES Tenants(TenantID) ON DELETE CASCADE,
    FOREIGN KEY (CreatedBy) REFERENCES Users(UserID) ON DELETE SET NULL
);