-- Insert test data into Users table
INSERT INTO "User" ("UserType", "IdpId", "Email", "UserName", "CreatedAt") VALUES
('Admin'::tacrm.user_type, '33aaab41-622e-4e4f-9891-617dfd91835f', 'mazzarajuan@hotmail.com', 'Paxy Admin', CURRENT_TIMESTAMP), -- Super Admin
('Agent'::tacrm.user_type, '626ba612-f639-4030-9275-220a427dc311', 'mazzarajuan@gmail.com', 'Paxy Agent', CURRENT_TIMESTAMP) -- Idependent Agent

-- Insert test data into Contacts table
INSERT INTO "Contact" ("UserId", "ContactStatusCode", "FullName", "Email", "Phone", "FromDate", "ToDate", "Adults", "Kids", "KidsAges", "CreatedAt") VALUES
(2, 'WIP', 'Patricia Jensen', 'patricia.jensen@example.com', '512-555-0190', '2025-05-17', '2025-06-25', 3, 2, '5,3', CURRENT_TIMESTAMP),
(2, 'NEW', 'Anthony Knight', NULL, '201-555-0137', NULL, '2025-07-09', 1, 0, '', CURRENT_TIMESTAMP),
(2, 'FUTURE', 'Andrea Lawson', 'andrea.lawson@example.com', '406-555-0119', NULL, NULL, 2, 1, '4', CURRENT_TIMESTAMP),
(2, 'WON', 'Bruce Hamilton', 'bruce.hamilton@example.com', NULL, '2025-03-01', '2025-03-10', 2, 0, '', CURRENT_TIMESTAMP),
(2, 'LOST', 'Rebecca Moore', 'rebecca.moore@example.com', '315-555-0198', '2025-08-12', '2025-08-20', 0, 0, '', CURRENT_TIMESTAMP)









-- Insert test data into Subscriptions table
INSERT INTO "Subscriptions" (
    "UserID", "PlanName", "StartDate", "EndDate", "Status"
) VALUES
(2, 'Premium', '2023-01-01', '2024-01-01', 'Active'), -- Travel Experts Agency's subscription
(3, 'Basic', '2023-01-01', '2024-01-01', 'Active'), -- Agent One's subscription
(4, 'Premium', '2023-01-01', '2024-01-01', 'Active'), -- Global Adventures Co.'s subscription
(5, 'Basic', '2023-01-01', '2024-01-01', 'Active'), -- Agent Two's subscription
(6, 'Basic', '2023-01-01', '2024-01-01', 'Active'); -- Independent Agent's subscription

-- Insert test data into Products table
INSERT INTO "Products" ("ProductTypeID", "ProductName", "CreatedAt", "UpdatedAt") VALUES
(1, 'Package', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(2, 'Hotel', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(3, 'Tickets', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(4, 'Attractions', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(5, 'Car Rental', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(6, 'Travel Insurance', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);

-- Insert test data into Providers table
INSERT INTO "Providers" ("ProviderName", "ContactInfo") VALUES
('VOBE', 'support@vobe.com'),
('DTA', 'info@dta.com'),
('VAX', 'help@vax.com'),
('EXPEDIA', 'contact@expedia.com');

-- Insert test data into Contacts table
INSERT INTO "Contacts" (
    "UserID", "ContactSourceID", "ContactStatusID", "FullName", 
    "Email", "Phone", "TravelDateStart", "TravelDateEnd", 
    "Adults", "Kids", "KidsAges", "Comments", 
    "EnableWhatsAppNotifications", "EnableEmailNotifications", "CreatedAt", "UpdatedAt"
) VALUES
(3, 1, 1, 'John Doe', 'john.doe1@example.com', '+1234567890', '2024-01-01', '2024-01-10', 2, 0, NULL, 'Looking for a family vacation.', FALSE, TRUE, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(5, 1, 1, 'Jane Smith', 'jane.smith2@example.com', '+1234567891', '2024-01-15', '2024-01-20', 1, 1, '5', 'Wants a single parent trip.', TRUE, TRUE, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);

-- Insert test data into Budgets table
INSERT INTO "Budgets" (
    "ContactID", "BudgetName", "BudgetDetails", "Adults", "Kids", "KidsAges", "CreatedAt", "UpdatedAt"
) VALUES
(1, 'Family Vacation Budget', 'Includes flights and hotel.', 2, 0, NULL, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(2, 'Single Parent Trip Budget', 'Includes hotel and activities.', 1, 1, '5', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);

-- Insert test data into Sales table
INSERT INTO "Sales" (
    "UserID", "ContactID", "SaleName", "SaleDetails", "Adults", "Kids", "KidsAges", "CreatedAt", "UpdatedAt"
) VALUES
(3, 1, 'Family Vacation Sale', 'Confirmed booking for family vacation.', 2, 0, NULL, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(5, 2, 'Single Parent Trip Sale', 'Confirmed booking for single parent trip.', 1, 1, '5', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);

-- Insert test data into SaleProducts table
INSERT INTO "SaleProducts" (
    "SaleID", "ProductID", "ProviderID", "BookingID", "BookingDate", "CheckinDate", "CheckoutDate", 
    "Currency", "BasePrice", "FinalPrice", "PaymentDueDate", "Commission", "Status", "CreatedAt", "UpdatedAt"
) VALUES
(1, 1, 1, 'SALE123', '2023-12-15', '2024-01-01', '2024-01-10', 'USD', 500.00, 550.00, '2023-12-20', 50.00, 'Active', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(2, 2, 2, 'SALE456', '2023-12-20', '2024-01-15', '2024-01-20', 'USD', 800.00, 880.00, '2023-12-25', 80.00, 'Active', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);

-- Insert test data into Payments table
INSERT INTO "Payments" (
    "SaleProductID", "Currency", "PaymentAmount", "PaymentDate", "PaymentMethod"
) VALUES
(1, 'USD', 550.00, '2023-12-18', 'Credit Card'),
(2, 'USD', 880.00, '2023-12-22', 'Bank Transfer');

-- Insert test data into CalendarEvents table
INSERT INTO "CalendarEvents" (
    "UserID", "EventType", "Title", "Description", "StartDateTime", "EndDateTime", "IsCustom", "CreatedAt", "UpdatedAt"
) VALUES
(3, 'Trip', 'Family Vacation', 'Trip to the beach resort.', '2024-01-01 10:00:00', '2024-01-10 18:00:00', FALSE, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(5, 'Payment Reminder', 'Payment Due', 'Reminder for payment due on 2023-12-25.', '2023-12-24 09:00:00', '2023-12-24 10:00:00', FALSE, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);

-- Insert test data into Notifications table
INSERT INTO "Notifications" (
    "UserID", "NotificationType", "Message", "IsRead", "EventID", "CreatedAt", "UpdatedAt"
) VALUES
(3, 'Payment Reminder', 'Payment for booking BOOK123 is due soon.', FALSE, 2, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(5, 'Trip Reminder', 'Your trip to the beach resort starts tomorrow.', FALSE, 1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);