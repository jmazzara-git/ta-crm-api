-- Insert test data into Agencies table
INSERT INTO "Agencies" ("AgencyName") VALUES 
('Travel Experts Agency'),
('Global Adventures Co.'),
('Dream Vacations Inc.'),
('Explore the World Ltd.'),
('Sunshine Travels LLC');

-- Insert test data into Users table
INSERT INTO "Users" ("Email", "FullName", "UserType") VALUES
('admin@admin.com', 'Admin User', 'ADMIN'),
('agent@agent.com', 'Agent User', 'AGENT')


-- Products test data
INSERT INTO "Products" ("ProductTypeID", "ProductName", "CreatedAt", "UpdatedAt") VALUES
(1, 'Package', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(2, 'Hotel', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(3, 'Ticket', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(4, 'Attraction', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(5, 'Car Rental', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(6, 'Travel Insurance', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);

-- Providers test data
INSERT INTO "Providers" ("ProviderName") VALUES
('VOBE'),
('DTA'),
('VAX'),
('EXPEDIA');

-- Contacts test data
-- Insert 25 contacts into the Contacts table with "New" status

INSERT INTO "Contacts" (
    "UserID", "ContactSourceID", "ContactStatusID", "FullName", 
    "Email", "Phone", "TravelDateStart", "TravelDateEnd", 
    "Adults", "Kids", "KidsAges", "Comments", 
    "EnableWhatsAppNotifications", "EnableEmailNotifications", "CreatedAt", "UpdatedAt"
) VALUES
(1, 1, 1, 'John Doe', 'john.doe1@example.com', '+1234567890', '2024-01-01', '2024-01-10', 2, 0, NULL, 'Looking for a family vacation.', FALSE, TRUE, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(1, 1, 1, 'Jane Smith', 'jane.smith2@example.com', '+1234567891', '2024-01-15', '2024-01-20', 1, 1, '5', 'Wants a single parent trip.', TRUE, TRUE, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(1, 2, 1, 'Alice Johnson', 'alice.johnson3@example.com', '+1234567892', '2024-02-01', '2024-02-07', 2, 2, '7,9', 'Family trip with kids.', FALSE, FALSE, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(1, 3, 1, 'Bob Brown', 'bob.brown4@example.com', '+1234567893', NULL, NULL, 0, 0, NULL, 'Exploring trip options.', FALSE, FALSE, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(1, 4, 1, 'Charlie Green', 'charlie.green5@example.com', '+1234567894', '2024-03-10', '2024-03-15', 2, 0, NULL, 'Business trip inquiry.', TRUE, TRUE, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(1, 1, 1, 'Emily White', 'emily.white6@example.com', '+1234567895', '2024-04-01', '2024-04-05', 1, 1, '4', 'Solo parent trip.', FALSE, TRUE, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(1, 2, 1, 'David Black', 'david.black7@example.com', '+1234567896', NULL, NULL, 2, 2, '6,8', 'Looking for group discounts.', FALSE, FALSE, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(1, 3, 1, 'Olivia King', 'olivia.king8@example.com', '+1234567897', '2024-05-01', '2024-05-10', 1, 1, '3', 'Interested in beach resorts.', TRUE, TRUE, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(1, 4, 1, 'Mia Walker', 'mia.walker9@example.com', '+1234567898', '2024-06-01', '2024-06-05', 2, 0, NULL, 'Luxury accommodation request.', TRUE, FALSE, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
(1, 1, 1, 'Lucas Martinez', 'lucas.martinez10@example.com', '+1234567899', '2024-07-01', '2024-07-15', 2, 2, '5,7', 'Vacation with extended family.', FALSE, TRUE, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);

-- Verify inserted data
SELECT * FROM "Contacts";



