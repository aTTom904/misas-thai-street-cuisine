-- Misa's Thai Street Cuisine Database Schema
-- Run this script in Azure SQL Database after creation

-- Create Orders table
CREATE TABLE Orders (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    OrderNumber NVARCHAR(50) NOT NULL UNIQUE,
    CustomerName NVARCHAR(100) NOT NULL,
    CustomerEmail NVARCHAR(255) NOT NULL,
    CustomerPhone NVARCHAR(20) NULL,
    ConsentToUpdates BIT NOT NULL DEFAULT 0,
    Total DECIMAL(10,2) NOT NULL,
    OrderDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    PaymentToken NVARCHAR(255) NULL,
    Status NVARCHAR(50) NOT NULL DEFAULT 'Pending',
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- Create OrderItems table
CREATE TABLE OrderItems (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL,
    ItemName NVARCHAR(100) NOT NULL,
    Category NVARCHAR(50) NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    Quantity INT NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (OrderId) REFERENCES Orders(Id) ON DELETE CASCADE
);

-- Create Customers table (for future use)
CREATE TABLE Customers (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    Phone NVARCHAR(20) NULL,
    ConsentToUpdates BIT NOT NULL DEFAULT 0,
    CreatedDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    LastOrderDate DATETIME2 NULL,
    TotalOrders INT NOT NULL DEFAULT 0,
    TotalSpent DECIMAL(10,2) NOT NULL DEFAULT 0,
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- Create indexes for better performance
CREATE INDEX IX_Orders_CustomerEmail ON Orders(CustomerEmail);
CREATE INDEX IX_Orders_OrderDate ON Orders(OrderDate DESC);
CREATE INDEX IX_Orders_Status ON Orders(Status);
CREATE INDEX IX_OrderItems_OrderId ON OrderItems(OrderId);
CREATE INDEX IX_Customers_Email ON Customers(Email);

-- Create a view for order summary
CREATE VIEW OrderSummary AS
SELECT 
    o.Id,
    o.OrderNumber,
    o.CustomerName,
    o.CustomerEmail,
    o.CustomerPhone,
    o.ConsentToUpdates,
    o.Total,
    o.OrderDate,
    o.Status,
    COUNT(oi.Id) as ItemCount,
    STRING_AGG(CONCAT(oi.ItemName, ' (', oi.Quantity, ')'), ', ') as ItemsSummary
FROM Orders o
LEFT JOIN OrderItems oi ON o.Id = oi.OrderId
GROUP BY o.Id, o.OrderNumber, o.CustomerName, o.CustomerEmail, 
         o.CustomerPhone, o.ConsentToUpdates, o.Total, o.OrderDate, o.Status;

-- Insert sample data for testing (optional)
/*
INSERT INTO Orders (OrderNumber, CustomerName, CustomerEmail, CustomerPhone, ConsentToUpdates, Total, Status)
VALUES ('ORDER-20250913-TEST', 'Test Customer', 'test@example.com', '555-0123', 1, 25.50, 'Completed');

INSERT INTO OrderItems (OrderId, ItemName, Category, Price, Quantity)
VALUES 
    (1, 'Pad Thai', 'Main', 12.95, 1),
    (1, 'Thai Iced Tea', 'Beverage', 3.50, 2),
    (1, 'Spring Rolls', 'Appetizer', 5.95, 1);
*/

PRINT 'Database schema created successfully!';
PRINT 'Tables created: Orders, OrderItems, Customers';
PRINT 'Indexes and views created for optimal performance';