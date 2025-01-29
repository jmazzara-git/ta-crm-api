-- Insert test data into Agencies table
INSERT INTO "Agencies" ("AgencyName", "ContactEmail", "ContactPhone") VALUES 
('Travel Experts Agency', 'info@travelexperts.com', '+1234567890'),
('Global Adventures Co.', 'contact@globaladventures.com', '+1234567891');

-- Insert test data into Users table
INSERT INTO "Users" (
    "AgencyID", "Email", "FullName", "UserType", "DefaultBudgetMessage", "DefaultWelcomeMessage", "DefaultThanksMessage"
) VALUES
(NULL, 'admin@admin.com', 'Admin User', 'ADMIN', 'Welcome!', 'Thank you for choosing us!', 'We appreciate your business!'), -- Super Admin
(1, 'owner@travelexperts.com', 'Agency Owner', 'AGENCY', 'Welcome!', 'Thank you for choosing us!', 'We appreciate your business!'), -- Owner of Travel Experts Agency
(1, 'agent1@travelexperts.com', 'Agent One', 'AGENT', 'Welcome!', 'Thank you for choosing us!', 'We appreciate your business!'), -- Agent belonging to Travel Experts Agency
(2, 'owner@globaladventures.com', 'Agency Owner', 'AGENCY', 'Welcome!', 'Thank you for choosing us!', 'We appreciate your business!'), -- Owner of Global Adventures Co.
(2, 'agent2@globaladventures.com', 'Agent Two', 'AGENT', 'Welcome!', 'Thank you for choosing us!', 'We appreciate your business!'), -- Agent belonging to Global Adventures Co.
(NULL, 'independent@agent.com', 'Independent Agent', 'AGENT', 'Welcome!', 'Thank you for choosing us!', 'We appreciate your business!'); -- Independent Agent

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